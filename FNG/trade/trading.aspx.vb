
Partial Class trading
    Inherits System.Web.UI.Page
    Public dbTime As String
    Dim msg As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(clsManage.iSession.cust_id.ToString) Is Nothing Then
            Response.Redirect("login.aspx")
        End If
        If Session(clsManage.iSession.first_trade.ToString).ToString = "y" Then
            Response.Redirect("cust_first_trade.aspx")
        End If

        'update online time
        msg = clsMain.updateTimeOnline(Session(clsManage.iSession.cust_id.ToString).ToString, Session.SessionID)
        If msg <> "" Then
            Session.Clear()
            clsManage.alert(Page, msg, , "login.aspx", "loginDup")
            Exit Sub
        End If

        If Not Page.IsPostBack Then
            If Session(clsManage.iSession.cust_id.ToString) IsNot Nothing Then

                Me.Session(clsManage.iSession.timetrade.ToString) = False

                txt99PriceLeave.Attributes.Add("style", "text-align:right")
                txt99PriceLeave.Attributes.Add("onkeypress", "checkNumber();")

                txt96PriceLeave.Attributes.Add("style", "text-align:right")
                txt96PriceLeave.Attributes.Add("onkeypress", "checkNumber();")

                tcTrade.ActiveTabIndex = 0
                hdfCust_id.Value = Session(clsManage.iSession.cust_id.ToString).ToString

                'check trade able ในตอนแรกถ้า excess<0 จะโชว์ label
                'change requir
                'getPortFolioForTradeAble()

                hdfConfirm.Value = "n"
                cbConfirm.Attributes.Add("onclick", "setConfirm(this);")
                dbTime = DateTime.Now.ToString("dd/MM/yyyy|hh:mm:ss|tt")
                Dim milTime As String = getMilisecondsTime()
                clsManage.Script(Page, "getClientTime('" + milTime + "');", "getLocalTime")

                clsFng.getQuantity96ToDDL(ddl96Quan, "96")
                clsFng.getQuantity96ToDDL(ddl99Quan, "99")
                clsFng.getQuantity96ToDDL(ddl96QuanLeave, "96")
                clsFng.getQuantity96ToDDL(ddl99QuanLeave, "99")
                clsFng.getQuantity96ToDDL(ddl96MiniQuan, "96Mini")

            End If
        End If
    End Sub

    Private Function getMilisecondsTime() As String
        Dim retval As Int64 = 0
        Dim st As DateTime = New DateTime(1970, 1, 1)
        Dim t As TimeSpan = (clsMain.getDatetimeServer().ToUniversalTime() - st)
        retval = CLng(t.TotalMilliseconds + 0.5)
        Return retval.ToString()
    End Function

    <System.Web.Services.WebMethod()> _
    Public Shared Function getMessage() As String
        Return clsMain.getMsg()
    End Function

    Sub trade_able(ByVal bl As Boolean)
        btn99Accept.Enabled = bl
        btn99Accept2.Enabled = bl
        btn99AcceptLeave.Enabled = bl
        btn99AcceptLeave2.Enabled = bl


        btn96Accept.Enabled = bl
        btn96Accept2.Enabled = bl
        btn96AcceptLeave.Enabled = bl
        btn96AcceptLeave2.Enabled = bl

    End Sub

    Function validateAccept(ByVal sto As clsSpot.SpotPrice, ByVal purity As String, ByVal ddl As DropDownList, ByVal type As String) As Boolean
        Try
            'If sto.selfPrice = clsManage.no Then 'ถ้า get price มา ไม่ต้อง check halt
            '    'check My halt
            '    If sto.selfHalt = "y" Then
            '        clsManage.alert(Page, "ขออภัย ขณะนี้ระบบได้หยุดการซื้อขายชั่วคราว")
            '        Return False
            '    End If

            '    'check admin halt
            '    If sto.adminHalt = "y" Then
            '        clsManage.alert(Page, "ขออภัย ขณะนี้ระบบได้หยุดการซื้อขายชั่วคราว")
            '        Return False
            '    End If
            'End If

            ''check admin timeout
            'If sto.timeTrade = "n" Then
            '    clsManage.alert(Page, "โปรดส่งคำสั่งซื้อขายอีกครั้ง รายการของท่านไม่ได้ถูกดำเนินการ")
            '    Return False
            'End If

            'check customer halt
            If sto.custHalt = "y" Then
                clsManage.alert(Page, "คำสั่งซื้อขายของท่านไม่สามารถดำเนินการได้")
                Return False
            End If

            'check max trade
            Dim quantity As Integer = clsManage.convert2zero(ddl.SelectedValue)
            Dim iQuantity As Integer = IIf(purity = "96", sto.maxBg, sto.maxKg)
            If quantity > iQuantity Then
                clsManage.alert(Page, "ซื้อขายเกินกว่าปริมาณที่กำหนด", ddl.ClientID)
                Return False
            End If

            Dim msgTradeLimit As String = clsFng.checkGuarantee(hdfCust_id.Value, type, purity, quantity)
            If msgTradeLimit <> "" Then
                clsManage.alert(Page, msgTradeLimit)
                Return False
            End If

            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Sub Accept(ByVal type As String, ByVal gold_type As String, Optional mini As Boolean = False)
        Try
            If msg <> "" Then Exit Sub
            If Session(clsManage.iSession.cust_id.ToString).ToString <> hdfCust_id.Value Then
                Session.Clear()
                Response.Redirect("login.aspx")
                Exit Sub
            End If

            If Me.Session(clsManage.iSession.timetrade.ToString) = False Then
                clsManage.alert(Page, "หมดเวลาในการซื้อขาย โปรดทำรายการใหม่อีกครั้ง") : Exit Sub
            End If

            Dim spo As New clsSpot.SpotPrice
            spo = clsSpot.getSpotPriceForCust(hdfCust_id.Value)
            If Not validateAccept(spo, gold_type, IIf(gold_type = clsFng.p96, ddl96Quan, ddl99Quan), type) Then Exit Sub

            Threading.Thread.Sleep(1000)
            Dim da As New dsTradeTableAdapters.tradeTableAdapter
            Dim dt As New dsTrade.tradeDataTable
            Dim dr As Data.DataRow = dt.NewRow

            Dim price As String = hdfPriceCust.Value
            'If gold_type = "96" Then
            '    If mini = True Then
            '        price = IIf(type = "sell", spo.bid96Mn.ToString, spo.ask96Mn.ToString)
            '    Else
            '        price = IIf(type = "sell", spo.bid96Bg.ToString, spo.ask96Bg.ToString)
            '    End If
            'Else
            '    price = IIf(type = "sell", spo.bid99Bg.ToString, spo.ask99Bg.ToString)
            'End If

            'If clsManage.convert2zero(hdfPriceCust.Value) <> clsManage.convert2zero(price) Then
            '    clsManage.alert(Page, "มีการเปลี่ยนแปลงราคา กรุณาทำรายการใหม่", , "trading.aspx") : Exit Sub
            'End If

            With dt
                dr(.cust_idColumn) = hdfCust_id.Value
                dr(dt.ref_noColumn) = clsFng.getTicketRunNo()
                dr(dt.typeColumn) = type
                dr(dt.gold_type_idColumn) = gold_type
                dr(dt.priceColumn) = price
                Dim amount As Double = 0

                If mini = True Then 'mini = true  then type is 96
                    'dr(dt.priceColumn) = IIf(type = "sell", hdfBid96Mini.Value, hdfAsk96Mini.Value)
                    dr(dt.purity96Column) = "M"
                    dr(dt.quantityColumn) = ddl96MiniQuan.SelectedValue
                    amount = (dr(dt.quantityColumn) * dr(dt.priceColumn))
                Else
                    'dr(dt.priceColumn) = price
                    dr(dt.quantityColumn) = IIf(gold_type = "96", ddl96Quan.SelectedValue, ddl99Quan.SelectedValue)
                    If gold_type = "96" Then
                        amount = dr(dt.quantityColumn) * dr(dt.priceColumn)
                    Else
                        amount = dr(dt.quantityColumn) * ((dr(dt.priceColumn) * 656) / 10)
                    End If
                End If

                dr(dt.amountColumn) = amount
                dr(dt.leave_orderColumn) = "n"
                dr(dt.ipColumn) = Request.UserHostAddress
                dr(dt.created_byColumn) = Session(clsManage.iSession.user_id.ToString).ToString
                dr(dt.created_dateColumn) = DateTime.Now
                dr(dt.modifier_dateColumn) = DateTime.Now

                If clsMain.checkTranBeforeAccept(CInt(price).ToString, type, IIf(gold_type = clsFng.p96, gold_type, clsFng.p99), dr(dt.quantityColumn).ToString) Then
                    dr(dt.accept_typeColumn) = "A"
                    dr(dt.modeColumn) = "accept"
                Else
                    dr(dt.modeColumn) = "tran"
                    dr(dt.accept_typeColumn) = "M"
                End If
                dt.Rows.Add(dr)
            End With

            Dim result As Integer = 0
            result = da.Update(dr)
            If result > 0 Then
                Dim script As String = ""
                If mini Then
                    script = "$get('" + ddl96MiniQuan.ClientID + "').selectedIndex = 0;$get('" + ddl96MiniQuan.ClientID + "').focus()"
                Else
                    If gold_type = "96" Then
                        script = "$get('" + ddl96Quan.ClientID + "').selectedIndex = 0;$get('" + ddl96Quan.ClientID + "').focus()"
                    Else
                        script = "$get('" + ddl99Quan.ClientID + "').selectedIndex = 0;$get('" + ddl99Quan.ClientID + "').focus()"
                    End If
                End If

                clsManage.Script(Page, "$find('" + tcTrade.ClientID + "').set_activeTabIndex(0);" + script + "", "openTab")

            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "error")
        End Try
    End Sub

    Sub LeaveAccept(ByVal type As String, ByVal gold_type As String, Optional mini As Boolean = False)
        Try
            If msg <> "" Then Exit Sub
            If Session(clsManage.iSession.cust_id.ToString).ToString <> hdfCust_id.Value Then
                Session.Clear()
                Response.Redirect("login.aspx")
                Exit Sub
            End If

            'Dim sto As New clsStore
            'sto = clsStore.getPriceStore(hdfCust_id.Value)
            Dim spo As New clsSpot.SpotPrice
            spo = clsSpot.getSpotPriceForCust(hdfCust_id.Value)


            If Not checkAdminHalt() Then clsManage.alert(Page, "ขออภัย ขณะนี้ระบบได้หยุดการซื้อขายชั่วคราว") : Exit Sub
            If Not checkHalt() Then clsManage.alert(Page, "คำสั่งซื้อขายของท่านไม่สามารถดำเนินการได้") : Exit Sub
            If mini = True Then
                'If Not checkMaxTrade(ddl96LeaveMiniQuan.SelectedValue, gold_type, sto) Then clsManage.alert(Page, "ซื้อขายเกินกว่าปริมาณที่กำหนด", ddl96LeaveMiniQuan.ClientID) : Exit Sub
            Else
                If Not checkMaxTrade(IIf(gold_type = clsFng.p96, ddl96QuanLeave.SelectedValue, ddl99QuanLeave.SelectedValue), gold_type, spo) Then clsManage.alert(Page, "ซื้อขายเกินกว่าปริมาณที่กำหนด", IIf(gold_type = "96", ddl96QuanLeave.ClientID, ddl99QuanLeave.ClientID)) : Exit Sub
            End If

            Dim msgTradeLimit As String = clsFng.checkGuarantee(hdfCust_id.Value, type, gold_type, IIf(gold_type = clsFng.p96, ddl96QuanLeave.SelectedValue, ddl99QuanLeave.SelectedValue), True)
            If msgTradeLimit <> "" Then clsManage.alert(Page, msgTradeLimit) : Exit Sub
            Threading.Thread.Sleep(2000)

            Dim range_leave As Double = clsMain.getStockRangeLeaveOrder() / 100 ''solution % of range_leave_order 
            Dim bid96 As Double = clsManage.convert2zero(hdfBid96.Value)
            Dim bid99 As Double = clsManage.convert2zero(hdfBid99.Value)
            Dim ask96 As Double = clsManage.convert2zero(hdfAsk96.Value)
            Dim ask99 As Double = clsManage.convert2zero(hdfAsk99.Value)


            Dim PriceValue As Double = 0
            If mini Then
                'PriceValue = txt96MiniPriceLeave.Text
            Else
                PriceValue = IIf(gold_type = "96", txt96PriceLeave.Text, txt99PriceLeave.Text)
            End If

            If type = "sell" Then
                'sell  เหลือเศษปัดลง
                ask96 = clsManage.convert2zero(hdfBid96.Value) + Math.Round(clsManage.convert2zero(hdfBid96.Value) * range_leave)
                ask99 = clsManage.convert2zero(hdfBid99.Value) + Math.Round(clsManage.convert2zero(hdfBid99.Value) * range_leave)
                If PriceValue < Double.Parse(IIf(gold_type = "96", clsManage.convert2Currency(bid96), clsManage.convert2Currency(bid99))) Then
                    clsManage.alert(Page, "ท่านไม่สามารถตั้งขายต่ำกว่าราคาเสนอซื้อ") : Exit Sub
                End If
                If PriceValue > Double.Parse(IIf(gold_type = "96", ask96, ask99)) Then
                    clsManage.alert(Page, String.Format("ต้องส่งคำสั่งขายไม่สูงกว่า {0} และไม่ต่ำกว่าราคาเสนอซื้อ", IIf(gold_type = "96", clsManage.convert2Currency(ask96), clsManage.convert2Currency(ask99)))) : Exit Sub
                End If
            Else
                'buy  เหลือเศษปัดขึ้น
                bid96 = clsManage.convert2zero(hdfAsk96.Value) - Math.Round(clsManage.convert2zero(hdfAsk96.Value) * range_leave)
                bid99 = clsManage.convert2zero(hdfAsk99.Value) - Math.Round(clsManage.convert2zero(hdfAsk99.Value) * range_leave)
                If PriceValue < Double.Parse(IIf(gold_type = "96", bid96, bid99)) Then
                    clsManage.alert(Page, String.Format("ต้องส่งคำสั่งซื้อไม่ต่ำกว่า {0} และไม่สูงกว่าราคาเสนอขาย", IIf(gold_type = "96", clsManage.convert2Currency(bid96), clsManage.convert2Currency(bid99)))) : Exit Sub
                End If
                If PriceValue > Double.Parse(IIf(gold_type = "96", clsManage.convert2Currency(ask96), clsManage.convert2Currency(ask99))) Then
                    clsManage.alert(Page, "ท่านไม่สามารถตั้งซื้อสูงกว่าราคาเสนอขาย") : Exit Sub
                End If
            End If

            Dim da As New dsTradeTableAdapters.tradeTableAdapter
            Dim dt As New dsTrade.tradeDataTable
            Dim dr As Data.DataRow = dt.NewRow


            'case purity 99 > get 99N or 99L from  stock online
            'Dim dtStockOnline As New Data.DataTable
            'dtStockOnline = clsMain.getStockOnlineUsers(hdfCust_id.Value)

           

            'If gold_type <> "96" Then
            '    gold_type = dtStockOnline.Rows(0)("purity99_default").ToString
            'End If
            Dim priceNow As String = ""
            Dim price As String = ""
            If gold_type = "96" Then
                price = IIf(type = "sell", hdfBid96.Value, hdfAsk96.Value)
                priceNow = IIf(type = "sell", spo.bid96Bg, spo.ask96Bg)
            Else
                price = IIf(type = "sell", hdfBid99.Value, hdfAsk99.Value)
                priceNow = IIf(type = "sell", spo.bid99Bg, spo.ask99Bg)
            End If
            With dt
                dr(dt.cust_idColumn) = hdfCust_id.Value
                dr(dt.ref_noColumn) = clsFng.getTicketRunNo()
                dr(dt.typeColumn) = type
                dr(dt.gold_type_idColumn) = gold_type

                Dim amount As Double = 0
                If mini = True Then
                    'No Mini
                Else
                    dr(dt.quantityColumn) = IIf(gold_type = "96", ddl96QuanLeave.SelectedValue, ddl99QuanLeave.SelectedValue)
                    dr(dt.priceColumn) = IIf(gold_type = "96", txt96PriceLeave.Text, txt99PriceLeave.Text)
                    If gold_type = "96" Then
                        amount = dr(dt.quantityColumn) * dr(dt.priceColumn)
                    Else
                        amount = dr(dt.quantityColumn) * ((dr(dt.priceColumn) * 656) / 10)
                    End If
                End If

                    dr(dt.amountColumn) = amount
                    dr(dt.leave_orderColumn) = "y"
                    dr(dt.ipColumn) = Request.UserHostAddress
                    dr(dt.created_byColumn) = Session(clsManage.iSession.user_id.ToString).ToString
                dr(dt.created_dateColumn) = DateTime.Now
                dr(dt.modifier_dateColumn) = DateTime.Now

                    'only leave order must check price 
                    Dim bidOrAskColumn As String = ""
                'If type = "sell" Then bidOrAskColumn = "bid" Else bidOrAskColumn = "ask"
                'If gold_type = "96" Then bidOrAskColumn += "96_" Else bidOrAskColumn += "99_"
                'bidOrAskColumn += dtStockOnline.Rows(0)("cust_level").ToString
                    'check price if < bit and ask >> accept now
                    If type = "sell" Then
                    If clsManage.convert2zero(dr(dt.priceColumn)) <= clsManage.convert2zero(priceNow) Then
                        If clsMain.checkTranBeforeAccept(CInt(price).ToString, type, IIf(gold_type = "96", "96", "99"), dr(dt.quantityColumn).ToString) Then
                            dr(dt.accept_typeColumn) = "A"
                            dr(dt.modeColumn) = "accept"
                        Else
                            dr(dt.modeColumn) = "tran"
                            dr(dt.accept_typeColumn) = "M"
                        End If
                    Else
                        dr(dt.modeColumn) = "tran"
                        dr(dt.accept_typeColumn) = "P"
                    End If
                    Else
                    If clsManage.convert2zero(dr(dt.priceColumn)) >= clsManage.convert2zero(priceNow) Then
                        If clsMain.checkTranBeforeAccept(CInt(price).ToString, type, IIf(gold_type = "96", "96", "99"), dr(dt.quantityColumn).ToString) Then
                            dr(dt.accept_typeColumn) = "A"
                            dr(dt.modeColumn) = "accept"
                        Else
                            dr(dt.modeColumn) = "tran"
                            dr(dt.accept_typeColumn) = "M"
                        End If
                    Else
                        dr(dt.modeColumn) = "tran"
                        dr(dt.accept_typeColumn) = "P"
                    End If
                    End If
                    dt.Rows.Add(dr)
            End With
            Dim result As Integer = 0
            result = da.Update(dr)
            If result > 0 Then
                Dim script As String = ""
                If mini Then
                    'txt96PriceLeave.Text = ""
                    'script = "$get('" + ddl96LeaveMiniQuan.ClientID + "').selectedIndex = 0;$get('" + txt96MiniPriceLeave.ClientID + "').value = '';$get('" + txt96MiniPriceLeave.ClientID + "').focus()"
                Else
                    If gold_type = "96" Then
                        txt96PriceLeave.Text = ""
                        script = "$get('" + ddl96QuanLeave.ClientID + "').selectedIndex = 0;$get('" + txt96PriceLeave.ClientID + "').value = '';$get('" + txt96PriceLeave.ClientID + "').focus()"
                    Else
                        txt99PriceLeave.Text = ""
                        script = "$get('" + ddl99QuanLeave.ClientID + "').selectedIndex = 0;$get('" + txt99PriceLeave.ClientID + "').value = '';$get('" + txt99PriceLeave.ClientID + "').focus()"
                    End If
                End If
                clsManage.Script(Page, "$find('" + tcTrade.ClientID + "').set_activeTabIndex(1);" + script, "openTab")
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "error")
        End Try
    End Sub

    Function checkTimeout() As Boolean
        Dim dt As Data.DataTable
        dt = clsMain.getPriceOnline()
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("time_end").ToString <> "" Then
                If CDate(dt.Rows(0)("time_end")) < DateTime.Now Then
                    Return False
                End If
            End If
        End If
        Return True
    End Function

    Function checkMaxTrade(ByVal quantity As Double, ByVal purity As String, ByVal spo As clsSpot.SpotPrice) As Boolean
        Dim iQuantity As Integer = 0
        If purity = "96" Then
            iQuantity = spo.maxBg
        Else
            iQuantity = spo.maxKg
        End If

        If quantity > iQuantity Then
            Return False
        End If
        Return True
    End Function

    Function checkAdminHalt() As Boolean
        Dim dt As Data.DataTable
        dt = clsMain.getPriceOnline()

        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("halt").ToString = "y" Then
                Return False
            End If
        End If
        Return True
    End Function

    Function checkHalt() As Boolean
        Dim dt As Data.DataTable
        dt = clsMain.getChkCust_id(hdfCust_id.Value)

        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("halt").ToString = "y" Then
                Return False
            End If
        End If
        Return True
    End Function

    Function checkTradeLimit(ByVal type As String, ByVal isLeave As String, ByVal gold_type As String) As String
        '*****Solution Check Trade limit
        '1 check margin if = 0 --> trade able else step2
        '2 check trade limit
        '3 check excess if < 0 --> Not Trade able else step4
        '4 check trade limit if true --> trade able else Not trade able
        Try

            Dim price As Double = 0
            Dim quan As Double = 0
            Dim amount As Double = 0
            If isLeave = "y" Then
                'Leave order (leave sell = buy)
                'Sell Buy
                If gold_type = "96" Then
                    'Purity96
                    price = clsManage.convert2zero(txt96PriceLeave.Text)
                    quan = clsManage.convert2zero(ddl96QuanLeave.SelectedValue)
                    amount = price * quan
                Else
                    'Purity99
                    price = clsManage.convert2zero(txt99PriceLeave.Text)
                    quan = clsManage.convert2zero(ddl99QuanLeave.SelectedValue)
                    amount = ((price * quan) * 656) / 10
                End If
            Else
                'order
                If type = "sell" Then
                    'Sell
                    If gold_type = "96" Then
                        'Purity96
                        price = clsManage.convert2zero(hdfBid96.Value)
                        quan = clsManage.convert2zero(ddl96Quan.SelectedValue)
                        amount = price * quan
                    Else
                        'Purity99
                        price = clsManage.convert2zero(hdfBid99.Value)
                        quan = clsManage.convert2zero(ddl99Quan.SelectedValue)
                        amount = ((price * quan) * 656) / 10
                    End If
                Else
                    'Buy
                    If gold_type = "96" Then
                        'Purity96
                        price = clsManage.convert2zero(hdfAsk96.Value)
                        quan = clsManage.convert2zero(ddl96Quan.SelectedValue)
                        amount = price * quan
                    Else
                        'Purity99
                        price = clsManage.convert2zero(hdfAsk99.Value)
                        quan = clsManage.convert2zero(ddl99Quan.SelectedValue)
                        amount = ((price * quan) * 656) / 10
                    End If
                End If
            End If

            Return clsMain.checkTradeLimitOnline(hdfCust_id.Value, quan, Math.Round(amount, 2), type, gold_type, isLeave)

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'Function checkGuarantee(cust_id As String, type As String, purity As String, quan As Double) As String
    '    Dim dc As New dcDBDataContext
    '    Dim ctm = (From c In dc.customers Where c.cust_id = cust_id).FirstOrDefault

    '    Dim credit96 As Double = 0
    '    Dim credit99 As Double = 0

    '    'Cash
    '    Dim cashMin96 As Double = (From g In dc.guarantees Where g.grt_trade96 = 10).FirstOrDefault.grt_value
    '    Dim cashMin99 As Double = (From g In dc.guarantees Where g.grt_trade99 = 1).FirstOrDefault.grt_value
    '    If ctm.cash_credit >= cashMin96 Then
    '        credit96 = Math.Floor(clsManage.convert2zero(ctm.cash_credit.ToString) / 1000)
    '    End If
    '    If ctm.cash_credit >= cashMin99 Then
    '        credit99 = Math.Floor(clsManage.convert2zero(ctm.cash_credit.ToString) / 65000)
    '    End If

    '    'Gold96
    '    Dim gold96Min = (From g In dc.guarantees Where g.grt_trade96 = 200 And g.grt_trade99 = 3).FirstOrDefault
    '    If ctm.quan_96 >= gold96Min.grt_value Then
    '        credit96 += Math.Floor(clsManage.convert2zero(ctm.quan_96) * 20)
    '        credit99 += Math.Floor(clsManage.convert2zero(ctm.quan_96) * 0.3)
    '    End If

    '    'Gold99
    '    Dim gold99Min = (From g In dc.guarantees Where g.grt_trade96 = 1500 And g.grt_trade99 = 22).FirstOrDefault
    '    If ctm.quan_99N >= gold99Min.grt_value Then
    '        credit96 += Math.Floor(clsManage.convert2zero(ctm.quan_99N) * 1500)
    '        credit99 += Math.Floor(clsManage.convert2zero(ctm.quan_99N) * 22)
    '    End If

    '    Dim type_ticket As String = IIf(type = "sell", "buy", "sell") 'สลับ buy sell

    '    Dim quanPending As Double = 0
    '    Dim tk = (From t In dc.tickets Where t.cust_id = cust_id And t.type = type_ticket And t.gold_type_id = purity And t.status_id = "101" Select t.quantity)
    '    If tk.Count > 0 Then
    '        quanPending = tk.Sum
    '    End If
    '    Dim quanTradePending As Double = 0
    '    Dim tr = (From tra In dc.trades Where tra.cust_id = cust_id And tra.type = type And tra.gold_type_id = purity And tra.mode = "tran" Select tra.quantity)
    '    If tr.Count > 0 Then
    '        quanTradePending = tr.Sum
    '    End If
    '    quanPending = quan + quanPending + quanTradePending

    '    If purity = clsFng.p96 Then
    '        If quanPending > credit96 Then
    '            Dim sms As String = String.Format("หลักประกันไม่พอ หลักประกันในการ{0} ของคูณจำกัดที่ " + credit96.ToString + " กิโล", IIf(type = "buy", "ซื้อ", "ขาย"))
    '            Return sms
    '        End If
    '    Else
    '        If quanPending > credit99 Then
    '            Dim sms As String = String.Format("หลักประกันไม่พอ หลักประกันในการ{0} ของคูณจำกัดที่ " + credit99.ToString + " กิโล", IIf(type = "buy", "ซื้อ", "ขาย"))
    '            Return sms
    '        End If
    '    End If

    '    Return ""
    'End Function

#Region "Buy Sell 96 99"

    Protected Sub btn99SellAccept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn99SellAccept.Click
        Accept("sell", "99")
    End Sub

    Protected Sub btn99BuyAccept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn99BuyAccept.Click
        Accept("buy", "99")
    End Sub

    Protected Sub btn96SellAccept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn96SellAccept.Click
        Accept("sell", "96")
    End Sub

    Protected Sub btn96BuyAccept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn96BuyAccept.Click
        Accept("buy", "96")
    End Sub

    'Leave Order
    Protected Sub btnAcceptLeave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn99AcceptLeave.Click
        LeaveAccept("sell", "99")
    End Sub
    Protected Sub btnAcceptLeave2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn99AcceptLeave2.Click
        LeaveAccept("buy", "99")
    End Sub
    Protected Sub btn96AcceptLeave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn96AcceptLeave.Click
        LeaveAccept("sell", "96")
    End Sub
    Protected Sub btn96AcceptLeave2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn96AcceptLeave2.Click
        LeaveAccept("buy", "96")
    End Sub

#End Region

#Region "Mini"
    Protected Sub btn96MiniSellAccept_Click(sender As Object, e As System.EventArgs) Handles btn96MiniSellAccept.Click
        Accept("sell", "96", True)
    End Sub

    Protected Sub btn96MiniBuyAccept_Click(sender As Object, e As System.EventArgs) Handles btn96MiniBuyAccept.Click
        Accept("buy", "96", True)
    End Sub

    'mini leave
    Protected Sub btn96LeaveMiniSellAccept_Click(sender As Object, e As System.EventArgs) Handles btn96LeaveMiniSellAccept.Click
        LeaveAccept("sell", "96", True)
    End Sub

    Protected Sub btn96LeaveMiniBuyAccept_Click(sender As Object, e As System.EventArgs) Handles btn96LeaveMiniBuyAccept.Click
        LeaveAccept("buy", "96", True)
    End Sub
#End Region


End Class
