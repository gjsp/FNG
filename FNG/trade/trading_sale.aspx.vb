
Partial Class trading_sale
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

                tcTrade.ActiveTabIndex = 0
                hdfSale_id.Value = Session(clsManage.iSession.cust_id.ToString).ToString

                'check trade able ในตอนแรกถ้า excess<0 จะโชว์ label
                'change requir
                'getPortFolioForTradeAble()

                hdfConfirm.Value = "n"
                cbConfirm.Attributes.Add("onclick", "setConfirm(this);")
                dbTime = DateTime.Now.ToString("dd/MM/yyyy|hh:mm:ss|tt")
                Dim milTime As String = getMilisecondsTime()
                clsManage.Script(Page, "getClientTime('" + milTime + "');", "getLocalTime")

                clsFng.getQuantity96ToDDL(ddl96Quan, "96")
                clsFng.getQuantity96ToDDL(ddl99Quan, "99", True)
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

            Dim msgTradeLimit As String = clsFng.checkGuarantee(hdfCust_id.Value, type, purity, quantity, False, True)
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
            If Session(clsManage.iSession.cust_id.ToString).ToString <> hdfSale_id.Value Then
                Session.Clear()
                Response.Redirect("login.aspx")
                Exit Sub
            End If

            If Me.Session(clsManage.iSession.timetrade.ToString) = False Then
                clsManage.alert(Page, "หมดเวลาในการซื้อขาย โปรดทำรายการใหม่อีกครั้ง") : Exit Sub
            End If

            If ucCustomer.CustID = "" Then
                clsManage.alert(Page, "โปรดเลือกรายชื่อลูกค้า") : Exit Sub
            End If

            Dim spo As New clsSpot.SpotPrice
            spo = clsSpot.getSpotPriceForCust(hdfCust_id.Value, hdfSale_id.Value)
            If Not validateAccept(spo, gold_type, IIf(gold_type = clsFng.p96, ddl96Quan, ddl99Quan), type) Then Exit Sub

            Threading.Thread.Sleep(1000)
            Dim da As New dsTradeTableAdapters.tradeTableAdapter
            Dim dt As New dsTrade.tradeDataTable
            Dim dr As Data.DataRow = dt.NewRow

            Dim price As String = hdfPriceCust.Value
          
            With dt
                dr(.cust_idColumn) = hdfCust_id.Value
                dr(dt.ref_noColumn) = clsFng.getTicketRunNo()
                dr(dt.typeColumn) = type
                dr(dt.gold_type_idColumn) = gold_type
                dr(dt.priceColumn) = price
                Dim amount As Double = 0
                Dim discount As Double = 0
                ' customer Sell ต้องบวกราคาให้,Customer Buy ต้องลดราคาให้                '
                If type = clsFng.buy Then
                    discount = -spo.discountBuy
                Else
                    discount = spo.discountSell
                End If

                If mini = True Then 'mini = true  then type is 96
                    dr(dt.purity96Column) = "M"
                    dr(dt.quantityColumn) = ddl96MiniQuan.SelectedValue
                    amount = (dr(dt.quantityColumn) * dr(dt.priceColumn))
                Else
                    dr(dt.quantityColumn) = IIf(gold_type = "96", ddl96Quan.SelectedValue, ddl99Quan.SelectedValue)
                    If gold_type = "96" Then
                        amount = dr(dt.quantityColumn) * (dr(dt.priceColumn) + discount)
                    Else
                        amount = dr(dt.quantityColumn) * (((dr(dt.priceColumn) + discount) * 656) / 10)
                    End If
                End If

                dr(dt.amountColumn) = amount
                dr(dt.leave_orderColumn) = "n"
                dr(dt.ipColumn) = Request.UserHostAddress
                dr(dt.created_byColumn) = Session(clsManage.iSession.user_id.ToString).ToString
                dr(dt.modifier_byColumn) = Session(clsManage.iSession.user_id.ToString).ToString
                dr(dt.created_dateColumn) = DateTime.Now
                dr(dt.modifier_dateColumn) = DateTime.Now
                'For Insert in tbl Tickets
                dr(dt.sale_idColumn) = Session(clsManage.iSession.cust_id.ToString).ToString

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

            Return clsMain.checkTradeLimitOnline(hdfCust_id.Value, quan, Math.Round(amount, 2), type, gold_type, isLeave)

        Catch ex As Exception
            Throw ex
        End Try
    End Function

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

#End Region

#Region "Mini"
    Protected Sub btn96MiniSellAccept_Click(sender As Object, e As System.EventArgs) Handles btn96MiniSellAccept.Click
        Accept("sell", "96", True)
    End Sub

    Protected Sub btn96MiniBuyAccept_Click(sender As Object, e As System.EventArgs) Handles btn96MiniBuyAccept.Click
        Accept("buy", "96", True)
    End Sub

#End Region

    Protected Sub imgSearchCust_Click(sender As Object, e As ImageClickEventArgs) Handles imgSearchCust.Click
        Try
            Dim cust_id As String = txtCustName.Text
            If Not hdfCust_id.Value = "" And cust_id.Contains(hdfCust_id.Value) Then
                ucCustomer.CustID = hdfCust_id.Value
                ucCustomer.LoadPortFolio()
            Else
                ucCustomer.CustID = cust_id
                ucCustomer.LoadPortFolio()
                clsManage.Script(Page, " document.getElementById('ctl00_MainContent_hdfCust_id').value = '" + ucCustomer.CustID + "';", "cust")
            End If
            'no set hdfcustid.value in code behide

            'If IsNumeric(cust_id) And cust_id.Length = 5 Then
            '    ucCustomer.CustID = cust_id
            '    ucCustomer.LoadPortFolio()

            '    'hdfCust_id.Value = cust_id
            '    clsManage.Script(Page, " document.getElementById('ctl00_MainContent_hdfCust_id').value = '" + cust_id + "';", "cust")
            'Else
            '    If hdfCust_id.Value = "" Then
            '        ucCustomer.CustID = ""
            '    Else
            '        ucCustomer.CustID = hdfCust_id.Value
            '    End If
            '    ucCustomer.LoadPortFolio()
            'End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub
End Class
