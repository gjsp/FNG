
Partial Class admin_trans2
    Inherits System.Web.UI.Page
    Dim ht As Hashtable
    Dim htPrice As Hashtable
    Dim colPrice As Integer = 7
    Dim colQuan As Integer = 8
    Dim colSum As Integer = 9
    Dim cssHalt As String = "buttonPro small red"
    Dim cssNor As String = "buttonPro small blue"


    Protected Sub tmTime_Tick(sender As Object, e As System.EventArgs) Handles tmTime.Tick
        If hdfIsRealtime.Value = "n" Then Exit Sub

        Dim newPrice As String = clsMain.getRealPricePlus()
        If hdfRealPrice.Value <> newPrice Then
            Threading.Thread.Sleep(pgSell.DisplayAfter)
            hdfRealPrice.Value = newPrice

            'set Price to Dictinary for check current price
            'htPrice = New Hashtable
            'htPrice.Add("bid99_1", clsManage.convert2zero(newRealPrice.Split(",")(0)))
            'htPrice.Add("bid99_2", clsManage.convert2zero(newRealPrice.Split(",")(1)))
            'htPrice.Add("bid99_3", clsManage.convert2zero(newRealPrice.Split(",")(2)))

            'htPrice.Add("ask99_1", clsManage.convert2zero(newRealPrice.Split(",")(3)))
            'htPrice.Add("ask99_2", clsManage.convert2zero(newRealPrice.Split(",")(4)))
            'htPrice.Add("ask99_3", clsManage.convert2zero(newRealPrice.Split(",")(5)))

            'htPrice.Add("bid96_1", clsManage.convert2zero(newRealPrice.Split(",")(6)))
            'htPrice.Add("bid96_2", clsManage.convert2zero(newRealPrice.Split(",")(7)))
            'htPrice.Add("bid96_3", clsManage.convert2zero(newRealPrice.Split(",")(8)))

            'htPrice.Add("ask96_1", clsManage.convert2zero(newRealPrice.Split(",")(9)))
            'htPrice.Add("ask96_2", clsManage.convert2zero(newRealPrice.Split(",")(10)))
            'htPrice.Add("ask96_3", clsManage.convert2zero(newRealPrice.Split(",")(11)))
            'ViewState("price") = htPrice

            htPrice = New Hashtable
            Dim cma = ","
            Dim bid99Plus As Double = clsManage.convert2zero(newPrice.Split(cma)(12))
            Dim ask99Plus As Double = clsManage.convert2zero(newPrice.Split(cma)(13))
            Dim bid96Plus As Double = clsManage.convert2zero(newPrice.Split(cma)(14))
            Dim ask96Plus As Double = clsManage.convert2zero(newPrice.Split(cma)(15))

            htPrice.Add("bid99_1", clsManage.convert2zero(newPrice.Split(cma)(0)) - bid99Plus)
            htPrice.Add("bid99_2", clsManage.convert2zero(newPrice.Split(cma)(1)) - bid99Plus)
            htPrice.Add("bid99_3", clsManage.convert2zero(newPrice.Split(cma)(2)) - bid99Plus)

            htPrice.Add("ask99_1", clsManage.convert2zero(newPrice.Split(cma)(3)) + ask99Plus)
            htPrice.Add("ask99_2", clsManage.convert2zero(newPrice.Split(cma)(4)) + ask99Plus)
            htPrice.Add("ask99_3", clsManage.convert2zero(newPrice.Split(cma)(5)) + ask99Plus)

            htPrice.Add("bid96_1", clsManage.convert2zero(newPrice.Split(cma)(6)) - bid96Plus)
            htPrice.Add("bid96_2", clsManage.convert2zero(newPrice.Split(cma)(7)) - bid96Plus)
            htPrice.Add("bid96_3", clsManage.convert2zero(newPrice.Split(cma)(8)) - bid96Plus)

            htPrice.Add("ask96_1", clsManage.convert2zero(newPrice.Split(cma)(9)) + ask96Plus)
            htPrice.Add("ask96_2", clsManage.convert2zero(newPrice.Split(cma)(10)) + ask96Plus)
            htPrice.Add("ask96_3", clsManage.convert2zero(newPrice.Split(cma)(11)) + ask96Plus)
            ViewState("price") = htPrice

            gvTrade.DataBind()
            gvTradeBuy.DataBind()
        Else
            Dim logString = clsMain.getTradeTransLogId(hdfTradeLogId.Value)
            Dim logid As Integer = logString.Split(",")(0)
            Dim type As String = logString.Split(",")(1)
            Dim pre_log_id As Integer = hdfTradeLogId.Value
            If logid > pre_log_id Then
                Threading.Thread.Sleep(pgSell.DisplayAfter)
                hdfTradeLogId.Value = logid
                If type = 1 Then
                    gvTradeBuy.DataBind()
                ElseIf type = 2 Then
                    gvTrade.DataBind()
                ElseIf type = 3 Then
                    gvTradeBuy.DataBind()
                    gvTrade.DataBind()
                Else
                    gvTrade.DataBind()
                    gvTradeBuy.DataBind()
                End If
            End If
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.QueryString("m") IsNot Nothing Then
                If Request.QueryString("m").ToString = "leave" Then
                    hdfMode.Value = clsManage.tradeMode.tran.ToString
                Else
                    hdfMode.Value = Request.QueryString("m").ToString
                End If

                hdfChangeColor.Value = "y"
                hdfRealPrice.Value = clsMain.getRealPrice()

                hdfTradeLogId.Value = clsMain.getTradeLogId(hdfMode.Value)
                hdfTradeLogIdForGrid.Value = hdfTradeLogId.Value
                gvTrade.EmptyDataText = clsManage.EmptyDataText
                gvTradeBuy.EmptyDataText = clsManage.EmptyDataText

                hdfIsRealtime.Value = "y"
                hdfConfirm.Value = "n"
                hdfConfirmSave.Value = "n"

                For i As Integer = 0 To 23 : ddl1Hour.Items.Add(i) : ddl2Hour.Items.Add(i) : ddl3Hour.Items.Add(i) : Next
                For i As Integer = 0 To 59 : ddl1Min.Items.Add(i.ToString("00")) : ddl2Min.Items.Add(i.ToString("00")) : ddl3Min.Items.Add(i.ToString("00")) : Next
                ddl1Hour.SelectedValue = "9" : ddl1Min.SelectedValue = "30"
                ddl2Hour.SelectedValue = "23" : ddl2Min.SelectedValue = "59"
                ddl3Hour.SelectedValue = "9" : ddl3Min.SelectedValue = "30"
                txt1Date_CalendarExtender.Format = clsManage.formatDateTime : txt2Date_CalendarExtender.Format = clsManage.formatDateTime : txt3Date_CalendarExtender.Format = clsManage.formatDateTime
                txt1Date.Text = Now.ToString(clsManage.formatDateTime) : txt2Date.Text = Now.ToString(clsManage.formatDateTime) : txt3Date.Text = Now.ToString(clsManage.formatDateTime)

                txtBidChangeNumber1.Text = "5" : txtBidChangeNumber2.Text = "10" : txtBidChangeNumber3.Text = "20"
                txtBidChangeNumber1.Attributes.Add("onkeypress", "numberOnly();") : txtBidChangeNumber2.Attributes.Add("onkeypress", "numberOnly();") : txtBidChangeNumber3.Attributes.Add("onkeypress", "checkNumber();")

                imgUp1.Attributes.Add("onclick", "changeBid('this');") : imgUp2.Attributes.Add("onclick", "changeBid('this');") : imgUp3.Attributes.Add("onclick", "changeBid('this');")
                imgDown1.Attributes.Add("onclick", "changeBid('this');") : imgDown2.Attributes.Add("onclick", "changeBid('this');") : imgDown3.Attributes.Add("onclick", "changeBid('this');")

                txtMaxPer.Attributes.Add("onkeypress", "checkNumber();")
                txtMaxBaht.Attributes.Add("onkeypress", "checkNumber();")
                txtMaxKg.Attributes.Add("onkeypress", "checkNumber();")
                txtLeaveTimeout.Attributes.Add("onkeypress", "numberOnly();")
                txtBidMin.Attributes.Add("onkeypress", "numberOnly();")
                txtBidMax.Attributes.Add("onkeypress", "numberOnly();")
                txt99Bid1.Attributes.Add("onkeypress", "numberOnly();")
                txt99Ask1.Attributes.Add("onkeypress", "numberOnly();")
                txtDif9699.Attributes.Add("onkeypress", "numberOnly();")
                TimeTxtBox.Attributes.Add("onkeypress", "numberOnly();")
                txt99DifBidAsk.Attributes.Add("onkeypress", "numberOnly();")
                txt96DifBidAsk.Attributes.Add("onkeypress", "numberOnly();")
                editbt.Attributes.Add("onclick", "return setConfirmSave('" + cbConfirmSave.ClientID + "','Do you want to Save?',this);")
                cbSortPrice.Attributes.Add("onclick", "$get('" & btnSearch.ClientID & "').click();return false;")
                cbConfirm.Attributes.Add("onclick", "setConfirm(this);")

                'TimeTxtBox.Attributes.Add("OnBlur", "colorBlur()")
                TimeTxtBox.Attributes.Add("onKeyup", "colorKeyup()")
                txt99Ask1.Attributes.Add("onKeyup", "colorKeyup()")
                txt99Bid1.Attributes.Add("onKeyup", "colorKeyup()")
                txt99DifBidAsk.Attributes.Add("onKeyup", "colorKeyup()")
                txt96DifBidAsk.Attributes.Add("onKeyup", "colorKeyup()")
                txtDif9699.Attributes.Add("onKeyup", "colorKeyup()")
                txtMaxBaht.Attributes.Add("onKeyup", "colorKeyup()")
                txtMaxKg.Attributes.Add("onKeyup", "colorKeyup()")
                txtLeaveTimeout.Attributes.Add("onKeyup", "colorKeyup()")
                txtMaxPer.Attributes.Add("onKeyup", "colorKeyup()")

                getData()

            End If
        End If
    End Sub

    Protected Sub gvTrade_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTrade.RowDataBound

        If e.Row.RowType = DataControlRowType.Header Then
            ht = New Hashtable
            htPrice = New Hashtable
            htPrice = CType(ViewState("price"), Hashtable)
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then

            'Change Wording
            If e.Row.DataItem("leave_order").ToString = "order" Then
                e.Row.Cells(1).Text = "Order"
            Else
                e.Row.Cells(1).Text = "Leave Order"
            End If

            'If e.Row.DataItem("type").ToString = "sell" Then e.Row.Cells(3).Text = "Sell"
            'If e.Row.DataItem("type").ToString = "buy" Then e.Row.Cells(3).Text = "Buy"

            'CType(e.Row.FindControl("btnAccept99"), ImageButton).Attributes.Add("onclick", "return fastConfirm('" + hdfConfirm.ClientID + "','Do you want to accept.');")
            'CType(e.Row.FindControl("btnAccept99"), ImageButton).Attributes.Add("onclick", "showConfirm(this,'" + e.Row.RowIndex.ToString + "');return false;")

            If e.Row.DataItem("leave_order").ToString <> "order" Then
                CType(e.Row.FindControl("btnReject99"), ImageButton).Attributes.Add("onclick", "showConfirmPassword(this); return false;")
            Else
                CType(e.Row.FindControl("btnReject99"), ImageButton).Attributes.Add("onclick", "return fastConfirm('" + hdfConfirm.ClientID + "','Do you want to reject.');")
            End If

            If hdfMode.Value = clsManage.tradeMode.tran.ToString Then
                If Session(clsManage.iSession.cust_id.ToString) Is Nothing Then 'admin
                    If e.Row.DataItem("mode").ToString = clsManage.tradeMode.tran.ToString Then
                        CType(e.Row.FindControl("btnReject99"), ImageButton).Visible = True
                        CType(e.Row.FindControl("btnAccept99"), ImageButton).Visible = True

                        'price ที่แตะขอบพอดีต้อง เปลี่ยนสี
                        'Fng No level in customer
                        Dim columnName As String = IIf(e.Row.DataItem("type").ToString = "sell", "bid", "ask") + IIf(e.Row.DataItem("gold_type_id").ToString = "96", "96", "99") + "_1" ' + e.Row.DataItem("cust_level").ToString
                        If e.Row.DataItem("leave_order").ToString <> "order" Then 'Leave order
                            If clsManage.convert2zero(e.Row.DataItem("price").ToString) = clsManage.convert2zero(htPrice(columnName).ToString) Then
                                e.Row.BackColor = Drawing.Color.Pink
                            End If
                        Else 'Order
                            If clsManage.convert2zero(e.Row.DataItem("price").ToString) = clsManage.convert2zero(htPrice(columnName).ToString) Then
                                e.Row.BackColor = New Drawing.ColorConverter().ConvertFromString("#FFC184")
                            End If
                        End If

                        Dim warning As String
                        If clsManage.convert2zero(e.Row.DataItem("price").ToString) > clsManage.convert2zero(htPrice(columnName).ToString) Then
                            warning = "Price higher than Bid!"
                        Else
                            warning = ""
                        End If
                        CType(e.Row.FindControl("btnAccept99"), ImageButton).Attributes.Add("onclick", "showConfirm(this,'" + e.Row.RowIndex.ToString + "','" + warning + "');return false;")

                    Else
                        CType(e.Row.FindControl("btnReject99"), ImageButton).Visible = False
                        CType(e.Row.FindControl("btnAccept99"), ImageButton).Visible = False
                    End If
                End If
            End If

            'set ToolTip Price and set Total Quantity
            Dim price As String = e.Row.Cells(colPrice).Text
            Dim quantity As Double = e.Row.Cells(colQuan).Text
            If ht.ContainsKey(price) Then
                ht(price) = quantity + Double.Parse(ht(price))
                'e.Row.Cells(e.Row.Cells.Count - 4).Text = clsManage.convert2Quantity(ht(price))
            Else
                ht.Add(price, quantity)
                'e.Row.Cells(e.Row.Cells.Count - 4).Text = clsManage.convert2Quantity(quantity)
            End If

            If e.Row.DataItem("new_trade").ToString = "y" Then
                'e.Row.BackColor = New Drawing.ColorConverter().ConvertFromString("#FFCE9D")
                e.Row.Font.Bold = True
            End If

            If hdfMode.Value = clsManage.tradeMode.tran.ToString Then
                If e.Row.DataItem("mode") = "reject" Then
                    e.Row.BackColor = New Drawing.ColorConverter().ConvertFromString("#FFA4A4") '#FFA4A4")
                ElseIf e.Row.DataItem("mode") = "accept" Then
                    e.Row.BackColor = New Drawing.ColorConverter().ConvertFromString("#66CCFF") '"#66CCFF")
                ElseIf e.Row.DataItem("mode") = "tran" Then
                    'e.Row.BackColor = New Drawing.ColorConverter().ConvertFromString("#DFDFDF")
                End If
            End If
        End If
            If e.Row.RowType = DataControlRowType.Footer Then
                For Each gvr As GridViewRow In gvTrade.Rows
                    Dim price As String = gvr.Cells(colPrice).Text
                    If ht.ContainsKey(price) Then
                        gvr.Cells(colPrice).ToolTip = "Price " + price + "  Total Quantity : " + ht(price).ToString
                    End If
                Next

                Dim htInvert As New Hashtable
                For i As Integer = gvTrade.Rows.Count - 1 To 0 Step -1
                    Dim price As String = gvTrade.Rows(i).Cells(colPrice).Text
                    Dim quantity As Double = gvTrade.Rows(i).Cells(colQuan).Text
                    If htInvert.ContainsKey(price) Then
                        htInvert(price) = quantity + Double.Parse(htInvert(price))
                        gvTrade.Rows(i).Cells(colSum).Text = clsManage.convert2Quantity(htInvert(price))
                    Else
                        htInvert.Add(price, quantity)
                        gvTrade.Rows(i).Cells(colSum).Text = clsManage.convert2Quantity(quantity)
                    End If
                Next
            End If
    End Sub

    Protected Sub gvTradeBuy_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTradeBuy.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            ht = New Hashtable
            htPrice = New Hashtable
            htPrice = CType(ViewState("price"), Hashtable)
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then

            'Change Wording
            If e.Row.DataItem("leave_order").ToString = "order" Then
                e.Row.Cells(1).Text = "Order"
            Else
                e.Row.Cells(1).Text = "Leave Order"
            End If

            'CType(e.Row.FindControl("btnAccept99"), ImageButton).Attributes.Add("onclick", "showConfirm(this,'" + e.Row.RowIndex.ToString + "'); return false;")

            If e.Row.DataItem("leave_order").ToString <> "order" Then
                CType(e.Row.FindControl("btnReject99"), ImageButton).Attributes.Add("onclick", "showConfirmPassword(this); return false;")
            Else
                CType(e.Row.FindControl("btnReject99"), ImageButton).Attributes.Add("onclick", "return fastConfirm('" + hdfConfirm.ClientID + "','Do you want to reject.');")
            End If

            If hdfMode.Value = clsManage.tradeMode.tran.ToString Then
                If Session(clsManage.iSession.cust_id.ToString) Is Nothing Then 'admin
                    If e.Row.DataItem("mode").ToString = clsManage.tradeMode.tran.ToString Then
                        CType(e.Row.FindControl("btnReject99"), ImageButton).Visible = True
                        CType(e.Row.FindControl("btnAccept99"), ImageButton).Visible = True

                        'price ที่แตะขอบพอดีต้อง เปลี่ยนสี
                        'Fng No level in customer
                        Dim columnName As String = IIf(e.Row.DataItem("type").ToString = "sell", "bid", "ask") + IIf(e.Row.DataItem("gold_type_id").ToString = "96", "96", "99") + "_1" ' + e.Row.DataItem("cust_level").ToString
                        If e.Row.DataItem("leave_order").ToString <> "order" Then 'Leave order
                            If clsManage.convert2zero(e.Row.DataItem("price").ToString) = clsManage.convert2zero(htPrice(columnName).ToString) Then
                                e.Row.BackColor = Drawing.Color.Pink
                            End If
                        Else 'Order
                            If clsManage.convert2zero(e.Row.DataItem("price").ToString) = clsManage.convert2zero(htPrice(columnName).ToString) Then
                                e.Row.BackColor = New Drawing.ColorConverter().ConvertFromString("#FFC184")
                            End If
                        End If

                        Dim warning As String
                        If clsManage.convert2zero(e.Row.DataItem("price").ToString) < clsManage.convert2zero(htPrice(columnName).ToString) Then
                            warning = "Price lower than Ask!"
                        Else
                            warning = ""
                        End If
                        CType(e.Row.FindControl("btnAccept99"), ImageButton).Attributes.Add("onclick", "showConfirm(this,'" + e.Row.RowIndex.ToString + "','" + warning + "');return false;")

                    Else
                        CType(e.Row.FindControl("btnReject99"), ImageButton).Visible = False
                        CType(e.Row.FindControl("btnAccept99"), ImageButton).Visible = False
                    End If
                End If
            End If

            'set ToolTip Price and set Total Quantity
            Dim price As String = e.Row.Cells(colPrice).Text
            Dim quantity As Double = e.Row.Cells(colQuan).Text
            If ht.ContainsKey(price) Then
                ht(price) = quantity + Double.Parse(ht(price))
                e.Row.Cells(colSum).Text = clsManage.convert2Quantity(ht(price))
            Else
                ht.Add(price, quantity)
                e.Row.Cells(colSum).Text = clsManage.convert2Quantity(quantity)
            End If

            If e.Row.DataItem("new_trade").ToString = "y" Then
                'e.Row.BackColor = New Drawing.ColorConverter().ConvertFromString("#FFCE9D")
                e.Row.Font.Bold = True
            End If

            If hdfMode.Value = clsManage.tradeMode.tran.ToString Then
                If e.Row.DataItem("mode") = "reject" Then
                    e.Row.BackColor = New Drawing.ColorConverter().ConvertFromString("#FFA4A4") '#FFA4A4")
                ElseIf e.Row.DataItem("mode") = "accept" Then
                    e.Row.BackColor = New Drawing.ColorConverter().ConvertFromString("#66CCFF") '"#66CCFF")
                ElseIf e.Row.DataItem("mode") = "tran" Then
                    'e.Row.BackColor = New Drawing.ColorConverter().ConvertFromString("#DFDFDF")
                End If
            End If
        End If

        If e.Row.RowType = DataControlRowType.Footer Then
            For Each gvr As GridViewRow In gvTradeBuy.Rows
                Dim price As String = gvr.Cells(colPrice).Text
                If ht.ContainsKey(price) Then
                    gvr.Cells(colPrice).ToolTip = "Price " + price + "  Total Quantity : " + ht(price).ToString
                End If
            Next
        End If

    End Sub

#Region "Update Mode"

    Sub updateMode(ByVal sender As Object, ByVal gv As GridView, ByVal mode As String, Optional ByVal fast_trade As String = "")
        Try
            tmTime.Enabled = False
            Threading.Thread.Sleep(pgSell.DisplayAfter)

            Dim i As Integer = CType(CType(sender, ImageButton).Parent.Parent, GridViewRow).RowIndex
            Dim id As String = gv.DataKeys(i).Value
            Dim type As String = gv.DataKeys(i).Values(1).ToString
            Dim price As String = gv.DataKeys(i).Values(2).ToString
            Dim purity As String = gv.DataKeys(i).Values(3).ToString

            'validate only accept mode
            If mode = clsManage.tradeMode.accept.ToString Then

                htPrice = New Hashtable
                htPrice = CType(ViewState("price"), Hashtable)

                If Not clsMain.checkPriceLimit(id, price, purity, type) Then
                    If type = "sell" Then
                        clsManage.alert(Page, "ราคา bid สูงกว่าราคา ask", , , "safetylevel")
                        Exit Sub
                    Else
                        clsManage.alert(Page, "ราคา ask ต่ำกว่าราคา bid", , , "safetylevel")
                        Exit Sub
                    End If
                End If
            End If
            Dim result As Integer = clsMain.UpdateTicketPortfolioByMode(id, mode, Me.Session(clsManage.iSession.user_id.ToString).ToString, type, price, purity, "", fast_trade)
            If result > 0 Then
                'plus for not update data
                'hdfTradeLogId.Value = clsManage.convert2zero(hdfTradeLogId.Value) + 1
                If mode = clsManage.tradeMode.accept.ToString Then
                    gv.Rows(i).BackColor = New Drawing.ColorConverter().ConvertFromString("#66CCFF")
                Else
                    gv.Rows(i).BackColor = New Drawing.ColorConverter().ConvertFromString("#FFA4A4")
                End If
                CType(gv.Rows(i).FindControl("btnReject99"), ImageButton).Visible = False
                CType(gv.Rows(i).FindControl("btnAccept99"), ImageButton).Visible = False

            End If
            'refreshGrid()
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "error")
        Finally
            tmTime.Enabled = True
            tmTime.Interval = 4000
        End Try
    End Sub

    Protected Sub btnAccept96_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        updateMode(sender, CType(CType(sender, ImageButton).Parent.Parent.NamingContainer, GridView), clsManage.tradeMode.accept.ToString)
    End Sub

    Protected Sub btnAccept99_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        ' case Fast Trade
        '       เลือกตาม radio list ถ้ามี heigher หรือ lower จะโชว์ model dialog
        ' case No fast trade
        '       เลือกตาม dropdownlist โชว์ model dialog ตลอด ถ้ามี heigher or lower จะโชว์ message
        Dim fastTrade As String = ""
        If cbConfirm.Checked Then
            'get value from radiolist and assign to hiddenField
            fastTrade = hdfFastMode.Value
        Else
            fastTrade = ddlConfirm.SelectedValue
        End If

        updateMode(sender, CType(CType(sender, ImageButton).Parent.Parent.NamingContainer, GridView), clsManage.tradeMode.accept.ToString, fastTrade)
    End Sub

    Protected Sub btnReject99_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        updateMode(sender, CType(CType(sender, ImageButton).Parent.Parent.NamingContainer, GridView), clsManage.tradeMode.reject.ToString)
    End Sub
#End Region


    <System.Web.Services.WebMethod()> _
    Public Shared Function checkMaxTradePercent(ByVal bid As String, ByVal newbid As String, ByVal per As String) As String
        Return clsManage.checkMaxTradePercent(bid, newbid, per)
    End Function

    <System.Web.Services.WebMethod()> _
    Public Shared Function getTradeMaxId(log_id As String) As String
        Return clsMain.getTradeTransLogId(log_id)
    End Function

    Protected Sub upSell_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles upSell.Load
        upBuy_Load(Nothing, Nothing)
    End Sub

    Protected Sub upBuy_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles upBuy.Load
        refreshGrid()
    End Sub
 
    Sub refreshGrid(Optional ByVal sleep As Boolean = True)
        Try
            'Threading.Thread.Sleep(1000)
            Dim purity As String = clsManage.getItemListSql(cbPurity)
            Dim cust_id As String = ""
            If Session(clsManage.iSession.cust_id.ToString) IsNot Nothing Then
                cust_id = Session(clsManage.iSession.cust_id.ToString).ToString
            End If

            Dim pDate1 As New DateTime
            Dim pDate2 As New DateTime
            Dim period As String = ""
            Dim tempDate1 As New DateTime
            Dim tempDate2 As New DateTime

            If rdoTime2.Checked Then
                tempDate1 = DateTime.ParseExact(txt1Date.Text, clsManage.formatDateTime, Nothing)
                tempDate2 = DateTime.ParseExact(txt2Date.Text, clsManage.formatDateTime, Nothing)
                pDate1 = pDate1.AddDays(tempDate1.Day - 1).AddMonths(tempDate1.Month - 1).AddYears(tempDate1.Year - 1).AddHours(ddl1Hour.SelectedValue).AddMinutes(ddl1Min.SelectedValue)
                pDate2 = pDate2.AddDays(tempDate2.Day - 1).AddMonths(tempDate2.Month - 1).AddYears(tempDate2.Year - 1).AddHours(ddl2Hour.SelectedValue).AddMinutes(ddl2Min.SelectedValue).AddSeconds(59)
                period = "period"
            Else
                tempDate1 = DateTime.ParseExact(txt3Date.Text, clsManage.formatDateTime, Nothing)
                pDate1 = pDate1.AddDays(tempDate1.Day - 1).AddMonths(tempDate1.Month - 1).AddYears(tempDate1.Year - 1).AddHours(ddl3Hour.SelectedValue).AddMinutes(ddl3Min.SelectedValue)

            End If

            Dim onlyLeave As String = "n"
            If Request.QueryString("m").ToString = "leave" Then
                onlyLeave = "y"
            End If

            objSrcTrade.SelectParameters("mode").DefaultValue = hdfMode.Value
            objSrcTrade.SelectParameters("purity").DefaultValue = purity
            objSrcTrade.SelectParameters("cust_id").DefaultValue = cust_id
            objSrcTrade.SelectParameters("max_trade_id").DefaultValue = hdfTradeLogIdForGrid.Value
            objSrcTrade.SelectParameters("period").DefaultValue = period
            objSrcTrade.SelectParameters("pDate1").DefaultValue = pDate1
            objSrcTrade.SelectParameters("pDate2").DefaultValue = pDate2
            objSrcTrade.SelectParameters("onlyLeave").DefaultValue = onlyLeave
            objSrcTrade.SelectParameters("type").DefaultValue = "sell"
            objSrcTrade.SelectParameters("sortPrice").DefaultValue = IIf(cbSortPrice.Checked, "y", "n")
            gvTrade.DataBind()

            objSrcTradeBuy.SelectParameters("mode").DefaultValue = hdfMode.Value
            objSrcTradeBuy.SelectParameters("purity").DefaultValue = purity
            objSrcTradeBuy.SelectParameters("cust_id").DefaultValue = cust_id
            objSrcTradeBuy.SelectParameters("max_trade_id").DefaultValue = hdfTradeLogIdForGrid.Value
            objSrcTradeBuy.SelectParameters("period").DefaultValue = period
            objSrcTradeBuy.SelectParameters("pDate1").DefaultValue = pDate1
            objSrcTradeBuy.SelectParameters("pDate2").DefaultValue = pDate2
            objSrcTradeBuy.SelectParameters("onlyLeave").DefaultValue = onlyLeave
            objSrcTradeBuy.SelectParameters("type").DefaultValue = "buy"
            objSrcTradeBuy.SelectParameters("sortPrice").DefaultValue = IIf(cbSortPrice.Checked, "y", "n")
            gvTradeBuy.DataBind()

            'set Gridview(background-color,show column)
            'set iframe height
            Dim strColor As String = ""
            Dim iFrameMode As String = ""
            'Dim scriptRefreshSumAccept As String = ""

            If hdfMode.Value = clsManage.tradeMode.tran.ToString Then
                If Request.QueryString("m").ToString = "leave" Then
                    strColor = "#808080"
                    iFrameMode = "ifmLeave"
                Else
                    strColor = "#4F9502"
                    iFrameMode = "ifmTran"
                End If

                'ถ้าเป็น customer ไม่ต้องโชว์ปุ่ม update mode
                If Session(clsManage.iSession.cust_id.ToString) IsNot Nothing Then
                    gvTrade.Columns(gvTrade.Columns.Count - 1).Visible = True
                    gvTrade.Columns(gvTrade.Columns.Count - 2).Visible = False
                    gvTradeBuy.Columns(gvTradeBuy.Columns.Count - 1).Visible = True
                    gvTradeBuy.Columns(gvTradeBuy.Columns.Count - 2).Visible = False
                Else
                    gvTrade.Columns(gvTrade.Columns.Count - 1).Visible = True
                    gvTrade.Columns(gvTrade.Columns.Count - 2).Visible = True
                    gvTradeBuy.Columns(gvTradeBuy.Columns.Count - 1).Visible = True
                    gvTradeBuy.Columns(gvTradeBuy.Columns.Count - 2).Visible = True
                End If

            ElseIf hdfMode.Value = clsManage.tradeMode.accept.ToString Then
                strColor = "#104587"
                iFrameMode = "ifmAcc"
                gvTrade.Columns(7).Visible = False 'colomn mode
                gvTrade.Columns(gvTrade.Columns.Count - 1).Visible = False
                gvTrade.Columns(gvTrade.Columns.Count - 2).Visible = False
                gvTradeBuy.Columns(7).Visible = False 'colomn mode
                gvTradeBuy.Columns(gvTradeBuy.Columns.Count - 1).Visible = False
                gvTradeBuy.Columns(gvTradeBuy.Columns.Count - 2).Visible = False
                'If cust_id = "" Then scriptRefreshSumAccept = "top.window.document.getElementById('ctl00_ContentPlaceHolder1_btnRefresh').click()"
            ElseIf hdfMode.Value = clsManage.tradeMode.reject.ToString Then
                strColor = "#990000"
                iFrameMode = "ifmRej"
                gvTrade.Columns(7).Visible = False 'colomn mode
                gvTrade.Columns(gvTrade.Columns.Count - 1).Visible = False
                gvTrade.Columns(gvTrade.Columns.Count - 2).Visible = False
                gvTradeBuy.Columns(7).Visible = False 'colomn mode
                gvTradeBuy.Columns(gvTradeBuy.Columns.Count - 1).Visible = False
                gvTradeBuy.Columns(gvTradeBuy.Columns.Count - 2).Visible = False
            End If

            Dim h As Integer = 380
            If gvTrade.Rows.Count <> 0 Then
                h = h + (gvTrade.Rows.Count * 25)
            End If
            If gvTradeBuy.Rows.Count <> 0 Then
                h = h + (gvTradeBuy.Rows.Count * 25)
            End If



            gvTrade.HeaderStyle.BackColor = New Drawing.ColorConverter().ConvertFromString(strColor)
            gvTradeBuy.HeaderStyle.BackColor = New Drawing.ColorConverter().ConvertFromString(strColor)

            'Sumary between buy and sell
            Dim buy96 As Double = 0
            Dim sell96 As Double = 0
            Dim buy99 As Double = 0
            Dim sell99 As Double = 0

            Dim dt As New Data.DataTable
            dt = clsMain.getTradeSummaryAccept(period, pDate1, pDate2)
            If dt.Rows.Count > 0 Then
                If dt.Rows(0)("buy96") IsNot DBNull.Value Then buy96 = dt.Rows(0)("buy96").ToString
                If dt.Rows(0)("sell96") IsNot DBNull.Value Then sell96 = dt.Rows(0)("sell96").ToString
                If dt.Rows(0)("buy99") IsNot DBNull.Value Then buy99 = dt.Rows(0)("buy99").ToString
                If dt.Rows(0)("sell99") IsNot DBNull.Value Then sell99 = dt.Rows(0)("sell99").ToString
            End If
            lblNet96.Text = clsManage.convert2Quantity(clsManage.convert2zero(sell96) - clsManage.convert2zero(buy96))
            lblNet99.Text = clsManage.convert2Quantity(clsManage.convert2zero(sell99) - clsManage.convert2zero(buy99))

            'ส่งออกนอก iframe
            'lblSumBuy96.Text = clsManage.convert2Quantity(buy96)
            'lblSumSell96.Text = clsManage.convert2Quantity(sell96)

            'lblSumBuy99.Text = clsManage.convert2Quantity(buy99)
            'lblSumSell99.Text = clsManage.convert2Quantity(sell99)


            clsManage.Script(Page, String.Format("setIframeHeight('" + iFrameMode + "','" & h & "');top.window.setSummary('{0}','{1}','{2}','{3}','{4}','{5}');", _
            clsManage.convert2Quantity(buy96), clsManage.convert2Quantity(sell96), lblNet96.Text, clsManage.convert2Quantity(buy99), clsManage.convert2Quantity(sell99), lblNet99.Text))

            'clsManage.Script(Page, String.Format("top.window.setSummary('{0}','{1}','{2}','{3}','{4}','{5}');", _
            'clsManage.convert2Quantity(buy96), clsManage.convert2Quantity(sell96), lblNet96.Text, clsManage.convert2Quantity(buy99), clsManage.convert2Quantity(sell99), lblNet99.Text))

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        refreshGrid()
    End Sub


#Region "update Price"
    Function saveData() As Boolean
        Try
            If txt99Bid1.Text = "" Or txt99Ask1.Text = "" Or txt99Bid2.Text = "" Or txt99Ask2.Text = "" Or txt99Bid1.Text = "" Or txt99Ask1.Text = "" Or txt99Bid3.Text = "" Or txt99Ask3.Text = "" _
            Or txt96Bid1.Text = "" Or txt96Ask1.Text = "" Or txt96Bid2.Text = "" Or txt96Ask2.Text = "" Or txt96Bid1.Text = "" Or txt96Ask1.Text = "" Or txt96Bid3.Text = "" Or txt96Ask3.Text = "" _
            Or TimeTxtBox.Text = "" Or txtLeaveTimeout.Text = "" Or txtMaxBaht.Text = "" Or txtMaxKg.Text = "" Or txtDif9699.Text = "" Or txt96DifBidAsk.Text = "" Or txt99DifBidAsk.Text = "" Then
                clsManage.alert(Page, clsManage.msgRequiredFill)
            Else
                If clsManage.convert2zero(txtLeaveTimeout.Text) >= clsManage.convert2zero(TimeTxtBox.Text) Then
                    clsManage.alert(Page, "Please Enter Timeout Less than Inactive Pricing Time.", txtLeaveTimeout.ClientID, , "timeLess")
                Else
                    clsMain.UpdatePriceOnline(txt99Bid1.Text, txt99Ask1.Text, txt99Bid2.Text, txt99Ask2.Text, txt99Bid3.Text, txt99Ask3.Text, _
                                              txt96Bid1.Text, txt96Ask1.Text, txt96Bid2.Text, txt96Ask2.Text, txt96Bid3.Text, txt96Ask3.Text, _
                                              TimeTxtBox.Text, txtBidMin.Text, txtBidMax.Text, txtMaxBaht.Text, txtMaxKg.Text, ViewState("min_ask9699"), ViewState("max_ask9699"), txtLeaveTimeout.Text, txtMaxPer.Text, ViewState("range_leave"), "y", Session(clsManage.iSession.user_name.ToString).ToString)
                    Return True
                End If
                Return False
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Sub getData()
        Dim dt As New Data.DataTable
        dt = clsMain.getPriceOnline()
        Dim dr As Data.DataRow = dt.Rows(0)
        hdfPwd.Value = dr("pwd_auth").ToString

        htPrice = New Hashtable
        Dim cma = "," : Dim l As String = "|"
        Dim newPrice As String = clsMain.getRealPricePlus()
        Dim bid99Plus As Double = clsManage.convert2zero(newPrice.Split(cma)(12))
        Dim ask99Plus As Double = clsManage.convert2zero(newPrice.Split(cma)(13))
        Dim bid96Plus As Double = clsManage.convert2zero(newPrice.Split(cma)(14))
        Dim ask96Plus As Double = clsManage.convert2zero(newPrice.Split(cma)(15))

        htPrice.Add("bid99_1", clsManage.convert2zero(newPrice.Split(cma)(0)) - bid99Plus)
        htPrice.Add("bid99_2", clsManage.convert2zero(newPrice.Split(cma)(1)) - bid99Plus)
        htPrice.Add("bid99_3", clsManage.convert2zero(newPrice.Split(cma)(2)) - bid99Plus)

        htPrice.Add("ask99_1", clsManage.convert2zero(newPrice.Split(cma)(3)) + ask99Plus)
        htPrice.Add("ask99_2", clsManage.convert2zero(newPrice.Split(cma)(4)) + ask99Plus)
        htPrice.Add("ask99_3", clsManage.convert2zero(newPrice.Split(cma)(5)) + ask99Plus)

        htPrice.Add("bid96_1", clsManage.convert2zero(newPrice.Split(cma)(6)) - bid96Plus)
        htPrice.Add("bid96_2", clsManage.convert2zero(newPrice.Split(cma)(7)) - bid96Plus)
        htPrice.Add("bid96_3", clsManage.convert2zero(newPrice.Split(cma)(8)) - bid96Plus)

        htPrice.Add("ask96_1", clsManage.convert2zero(newPrice.Split(cma)(9)) + ask96Plus)
        htPrice.Add("ask96_2", clsManage.convert2zero(newPrice.Split(cma)(10)) + ask96Plus)
        htPrice.Add("ask96_3", clsManage.convert2zero(newPrice.Split(cma)(11)) + ask96Plus)
        ViewState("price") = htPrice

        '99

        hdfBid99.Value = dr("bid99_1").ToString ' for max %
        txt99Bid1.Text = clsManage.convert2Currency(dr("bid99_1").ToString)
        txt99Ask1.Text = clsManage.convert2Currency(dr("ask99_1").ToString)
        txt99Bid2.Text = dr("bid99_2").ToString
        txt99Ask2.Text = dr("ask99_2").ToString
        txt99Bid3.Text = dr("bid99_3").ToString
        txt99Ask3.Text = dr("ask99_3").ToString

        ViewState("bid") = clsManage.convert2zero(dr("bid99_1").ToString)
        ViewState("ask") = clsManage.convert2zero(dr("ask99_1").ToString)

        '96

        txt96Bid1.Text = clsManage.convert2Currency(dr("bid96_1").ToString)
        txt96Ask1.Text = clsManage.convert2Currency(dr("ask96_1").ToString)
        txt96Bid2.Text = dr("bid96_2").ToString
        txt96Ask2.Text = dr("ask96_2").ToString
        txt96Bid3.Text = dr("bid96_3").ToString
        txt96Ask3.Text = dr("ask96_3").ToString

        txtBidMin.Text = clsManage.convert2Currency(dr("min").ToString)
        txtBidMax.Text = clsManage.convert2Currency(dr("max").ToString)
        txtMaxBaht.Text = clsManage.convert2Currency(dr("max_baht").ToString)
        txtMaxKg.Text = clsManage.convert2Currency(dr("max_kg").ToString)

        ViewState("min_ask9699") = dr("min_ask_dif_9699")
        ViewState("max_ask9699") = dr("max_ask_dif_9699")
        ViewState("range_leave") = dr("range_leave_order")

        txtMaxPer.Text = dr("max_per").ToString

        TimeTxtBox.Text = dr("time").ToString
        txtLeaveTimeout.Text = dr("order_timeout").ToString
        'hdfTime.Value = CDate(dr("time_end")).ToString("dd/MM/yyyy hh:mm:ss tt")

        '####set for calulate
        'diff 96 - 99 >>> change requirment from bid99 >> ask99
        If txt99Ask1.Text <> "" And txt96Ask1.Text <> "" Then
            txtDif9699.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Ask1.Text) - clsManage.convert2zero(txt96Ask1.Text))
        End If
        'diff Bid-ask99
        If txt99Bid1.Text <> "" And txt99Ask1.Text <> "" Then
            txt99DifBidAsk.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Ask1.Text) - clsManage.convert2zero(txt99Bid1.Text))
        End If
        'diff Bid-ask96
        If txt96Bid1.Text <> "" And txt96Ask1.Text <> "" Then
            txt96DifBidAsk.Text = clsManage.convert2Currency(clsManage.convert2zero(txt96Ask1.Text) - clsManage.convert2zero(txt96Bid1.Text))
        End If

        '99 Bid lv2
        If txt99Bid2.Text <> "" And txt99Bid1.Text <> "" Then
            txt99BidDif2.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Bid1.Text) - clsManage.convert2zero(txt99Bid2.Text))
        End If
        '99 Ask lv2'
        If txt99Ask2.Text <> "" And txt99Ask1.Text <> "" Then
            txt99AskDif2.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Ask2.Text) - clsManage.convert2zero(txt99Ask1.Text))
        End If
        '99 Bid lv3
        If txt99Bid3.Text <> "" And txt99Bid1.Text <> "" Then
            txt99BidDif3.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Bid1.Text) - clsManage.convert2zero(txt99Bid3.Text))
        End If
        '99 Ask lv3
        If txt99Ask3.Text <> "" And txt99Ask1.Text <> "" Then
            txt99AskDif3.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Ask3.Text) - clsManage.convert2zero(txt99Ask1.Text))
        End If

        '96 Bid lv2
        If txt96Bid2.Text <> "" And txt96Bid1.Text <> "" Then
            txt96BidDif2.Text = clsManage.convert2Currency(clsManage.convert2zero(txt96Bid1.Text) - clsManage.convert2zero(txt96Bid2.Text))
        End If
        '96 Ask lv2
        If txt96Ask2.Text <> "" And txt96Ask1.Text <> "" Then
            txt96AskDif2.Text = clsManage.convert2Currency(clsManage.convert2zero(txt96Ask2.Text) - clsManage.convert2zero(txt96Ask1.Text))
        End If
        '96 Bid lv3
        If txt96Bid3.Text <> "" And txt96Bid1.Text <> "" Then
            txt96BidDif3.Text = clsManage.convert2Currency(clsManage.convert2zero(txt96Bid1.Text) - clsManage.convert2zero(txt96Bid3.Text))
        End If
        '96 Ask lv3
        If txt96Ask3.Text <> "" And txt96Ask1.Text <> "" Then
            txt96AskDif3.Text = clsManage.convert2Currency(clsManage.convert2zero(txt96Ask3.Text) - clsManage.convert2zero(txt96Ask1.Text))
        End If
        'Halt
        updateHalt(dr("halt").ToString)

    End Sub

    Sub updateHalt(ByVal halt As String)
        If halt = "" Then Exit Sub
        If halt = "n" Then
            btnHalt.CssClass = cssNor
        Else
            btnHalt.CssClass = cssHalt
        End If
        btnHalt.CommandArgument = halt
    End Sub

    Protected Sub editbt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles editbt.Click
        Try
            If refresh() Then
                If saveData() Then
                    getData()

                    'refresh for check price (cheange color row)
                    upSell.DataBind()
                    upBuy.DataBind()
                    Dim scriptRefreshLeave = "top.window.document.getElementById('ifmLeave').src='trans.aspx?m=leave';" 'For Refresh color in row when press Save
                    clsManage.Script(Page, "top.window.document.getElementById('ifmTime').src='timer.aspx';" + scriptRefreshLeave + " ", "reset_time")
               
                    hdfChangeColor.Value = "y"
                End If
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Private Function refresh() As Boolean
        Try
            If txt99Bid1.Text.Trim = "" And txt99Ask1.Text.Trim = "" Then
                clsManage.alert(Page, "Please Enter Bid or Ask for Calculate Price.", "", "", "check")
                Return False
            End If

            If TimeTxtBox.Text = "" Then
                clsManage.alert(Page, "Please Enter Inactive Time.", TimeTxtBox.ClientID, "", "check")
                Return False
            End If

            If txtMaxBaht.Text = "" Then
                clsManage.alert(Page, "Please Enter Max BAHT.", txtMaxBaht.ClientID, "", "check")
                Return False
            End If

            If txtMaxKg.Text = "" Then
                clsManage.alert(Page, "Please Enter Max KG.", txtMaxKg.ClientID, "", "check")
                Return False
            End If

            If txtLeaveTimeout.Text = "" Then
                clsManage.alert(Page, "Please Enter Leave Order Time Out.", txtLeaveTimeout.ClientID, "", "check")
                Return False
            End If

            If txt99Bid1.Text <> "" And Not IsNumeric(txt99Bid1.Text) Then
                clsManage.alert(Page, "Please Enter Only Numbers.", txt99Bid1.ClientID, "", "check")
                Return False
            End If

            If txt99Ask1.Text <> "" And Not IsNumeric(txt99Ask1.Text) Then
                clsManage.alert(Page, "Please Enter Only Numbers.", txt99Ask1.ClientID, "", "check")
                Return False
            End If

            If Not IsNumeric(TimeTxtBox.Text) Then
                clsManage.alert(Page, "Please Enter Only Numbers.", TimeTxtBox.ClientID, "", "check")
                Return False
            End If

            If Not IsNumeric(txtMaxBaht.Text) Then
                clsManage.alert(Page, "Please Enter Only Numbers.", txtMaxBaht.ClientID, "", "check")
                Return False
            End If

            If Not IsNumeric(txtMaxKg.Text) Then
                clsManage.alert(Page, "Please Enter Only Numbers.", txtMaxKg.ClientID, "", "check")
                Return False
            End If

            If Not IsNumeric(txtLeaveTimeout.Text) Then
                clsManage.alert(Page, "Please Enter Only Numbers.", txtLeaveTimeout.ClientID, "", "check")
                Return False
            End If

            'cal Min - max ask dif 96-99
            If clsManage.convert2zero(txtDif9699.Text) > clsManage.convert2zero(ViewState("max_ask9699").ToString) Then
                clsManage.alert(Page, "Ask99.99-Ask96.50 not in Min-Max.", txtDif9699.ClientID, "", "limit") : Return False
            End If
            If clsManage.convert2zero(txtDif9699.Text) < clsManage.convert2zero(ViewState("min_ask9699").ToString) Then
                clsManage.alert(Page, "Ask99.99-Ask96.50 not in Min-Max.", txtDif9699.ClientID, "", "limit") : Return False
            End If


            Dim caseType As Integer ' 0:no,1:bid,2:ask
            If txt99Bid1.Text <> "" And txt99Ask1.Text = "" Then
                caseType = 1
            ElseIf txt99Bid1.Text = "" And txt99Ask1.Text <> "" Then
                caseType = 2
            ElseIf clsManage.convert2zero(ViewState("bid").ToString) <> clsManage.convert2zero(txt99Bid1.Text) Then
                caseType = 1
            ElseIf clsManage.convert2zero(ViewState("ask").ToString) <> clsManage.convert2zero(txt99Ask1.Text) Then
                caseType = 2
            End If

            If caseType = 1 Then
                If clsManage.convert2zero(txt99Bid1.Text) > clsManage.convert2zero(txtBidMax.Text) Then
                    clsManage.alert(Page, "Bid Level 1 not in Min-Max.", txt99Bid1.ClientID, "", "limit") : Return False
                End If
                If clsManage.convert2zero(txt99Bid1.Text) < clsManage.convert2zero(txtBidMin.Text) Then
                    clsManage.alert(Page, "Bid Level 1 not in Min-Max.", txt99Bid1.ClientID, "", "limit") : Return False
                End If
            End If
            If caseType = 2 Then
                'calculate when plus
                If clsManage.convert2zero(txt99Ask1.Text) - clsManage.convert2zero(txt99DifBidAsk.Text) > clsManage.convert2zero(txtBidMax.Text) Then
                    clsManage.alert(Page, "Bid Level 1 not in Min-Max.", , , "limit") : Return False
                End If
                If clsManage.convert2zero(txt99Ask1.Text) - clsManage.convert2zero(txt99DifBidAsk.Text) < clsManage.convert2zero(txtBidMin.Text) Then
                    clsManage.alert(Page, "Bid Level 1 not in Min-Max.", , , "limit") : Return False
                End If
            End If

            ''check 96 when plus
            'If clsManage.convert2zero(txt99Ask1.Text) - clsManage.convert2zero(txtDif9699.Text) < clsManage.convert2zero(txtBidMin.Text) Then
            '    clsManage.alert(Page, "Ask Level 1 not in Min-Max.", , , "limit") : Return False
            'End If

            'If clsManage.convert2zero(txt96Ask1.Text) - clsManage.convert2zero(txt96DifBidAsk.Text) < clsManage.convert2zero(txtBidMin.Text) Then
            '    clsManage.alert(Page, "Bid Level 1 not in Min-Max.", , , "limit") : Return False
            'End If

            'Check Max Percent
            'If Not clsManage.checkMaxTradePercent(ViewState("bid99").ToString, txt99Bid1.Text, txtMaxPer.Text) Then
            '    clsManage.alert(Page, "Bid 99.99 more than the percentage specified.", , , "limitPer") : Return False
            'End If

            If caseType = 1 Then
                'แก้ที่ bid
                txt99Ask1.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Bid1.Text) + clsManage.convert2zero(txt99DifBidAsk.Text))
            ElseIf caseType = 2 Then
                'แก้ที่ ask
                txt99Bid1.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Ask1.Text) - clsManage.convert2zero(txt99DifBidAsk.Text))
            Else
                'แก้ที่ diff ต่างๆ แก้ที่ bid เป็นหลัก
                txt99Ask1.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Bid1.Text) + clsManage.convert2zero(txt99DifBidAsk.Text))
            End If

            txt96Ask1.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Ask1.Text) - clsManage.convert2zero(txtDif9699.Text))
            txt96Bid1.Text = clsManage.convert2Currency(clsManage.convert2zero(txt96Ask1.Text) - clsManage.convert2zero(txt96DifBidAsk.Text))

            ViewState("bid") = clsManage.convert2zero(txt99Bid1.Text)
            ViewState("ask") = clsManage.convert2zero(txt99Ask1.Text)

            '99 Bid Lv2
            If txt99Bid1.Text <> "" And txt99BidDif2.Text <> "" Then
                txt99Bid2.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Bid1.Text) - clsManage.convert2zero(txt99BidDif2.Text))
            End If
            '99 Ask Lv2
            If txt99Ask1.Text <> "" And txt99AskDif2.Text <> "" Then
                txt99Ask2.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Ask1.Text) + clsManage.convert2zero(txt99AskDif2.Text))
            End If

            '99 Bid Lv3
            If txt99Bid1.Text <> "" And txt99BidDif3.Text <> "" Then
                txt99Bid3.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Bid1.Text) - clsManage.convert2zero(txt99BidDif3.Text))
            End If
            '99 Ask Lv3
            If txt99Ask1.Text <> "" And txt99AskDif3.Text <> "" Then
                txt99Ask3.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Ask1.Text) + clsManage.convert2zero(txt99AskDif3.Text))
            End If

            '96 Bid Lv2
            If txt96Bid1.Text <> "" And txt96BidDif2.Text <> "" Then
                txt96Bid2.Text = clsManage.convert2Currency(clsManage.convert2zero(txt96Bid1.Text) - clsManage.convert2zero(txt96BidDif2.Text))
            End If
            '96 Ask Lv2
            If txt96Ask1.Text <> "" And txt96AskDif2.Text <> "" Then
                txt96Ask2.Text = clsManage.convert2Currency(clsManage.convert2zero(txt96Ask1.Text) + clsManage.convert2zero(txt96AskDif2.Text))
            End If

            '96 Bid Lv3
            If txt96Bid1.Text <> "" And txt96BidDif3.Text <> "" Then
                txt96Bid3.Text = clsManage.convert2Currency(clsManage.convert2zero(txt96Bid1.Text) - clsManage.convert2zero(txt96BidDif3.Text))
            End If
            '96 Ask Lv3
            If txt96Ask1.Text <> "" And txt96AskDif3.Text <> "" Then
                txt96Ask3.Text = clsManage.convert2Currency(clsManage.convert2zero(txt96Ask1.Text) + clsManage.convert2zero(txt96AskDif3.Text))
            End If

            Return True
            'txt99Bid1.Text = clsManage.convert2Currency(txt99Bid1.Text)
            'txt99Ask1.Text = clsManage.convert2Currency(txt99Ask1.Text)

        Catch ex As Exception
            clsManage.alert(Page, "Error Please Try Again.", "", "", "error")
            Return False
        End Try
    End Function

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        getData()
        txtBidChangeNumber1.Text = "5"
        txtBidChangeNumber2.Text = "10"
        txtBidChangeNumber3.Text = "20"
    End Sub

    Protected Sub btnRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        refresh()
    End Sub

    Protected Sub btnHalt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHalt.Click
        Try
            Dim halt As String = ""
            If btnHalt.CommandArgument = "n" Then
                halt = "y"
            Else
                halt = "n"
            End If
            Dim result As String = clsMain.updateStockHalt(halt)
            If result <> "" Then
                updateHalt(result)
            End If

        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

#End Region

#Region "change Bid"
    Private Function checkBidLimit(ByVal type As String, ByVal num As String) As Boolean
        If txt99Bid1.Text.Trim = "" Or txt99Ask1.Text.Trim = "" Then
            clsManage.alert(Page, "Please Enter Bid or Ask.", , , "checkEnter")
            Return False
        End If

        If type = "down" Then
            If (clsManage.convert2zero(txt99Bid1.Text) - clsManage.convert2zero(num)) < clsManage.convert2zero(txtBidMin.Text) Then
                clsManage.alert(Page, "Bid Level 1 not in Min-Max.", , , "down")
                Return False
            End If
            If (clsManage.convert2zero(txt99Bid1.Text) - clsManage.convert2zero(num)) > clsManage.convert2zero(txtBidMax.Text) Then
                clsManage.alert(Page, "Bid Level 1 not in Min-Max.", , , "down")
                Return False
            End If
        Else
            If (clsManage.convert2zero(txt99Bid1.Text) + clsManage.convert2zero(num)) < clsManage.convert2zero(txtBidMin.Text) Then
                clsManage.alert(Page, "Bid Level 1 not in Min-Max.", , , "up")
                Return False
            End If
            If (clsManage.convert2zero(txt99Bid1.Text) + clsManage.convert2zero(num)) > clsManage.convert2zero(txtBidMax.Text) Then
                clsManage.alert(Page, "Bid Level 1 not in Min-Max.", , , "up")
                Return False
            End If
        End If
        Return True
    End Function

    Private Sub upBid(ByVal txt As TextBox)
        Try
            If txt.Text.Trim = "" Then clsManage.alert(Page, "Please Enter Number.", , , "number") : Exit Sub
            Dim num = txt.Text.Trim
            If Not checkBidLimit("up", num) Then Exit Sub
            If txt99Bid1.Text <> "" And txt99Ask1.Text <> "" Then
                txt99Bid1.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Bid1.Text) + clsManage.convert2zero(num))
                txt99Ask1.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Ask1.Text) + clsManage.convert2zero(num))
                refresh()
                clsManage.Script(Page, "colorKeyup()", "button")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub DownBid(ByVal txt As TextBox)
        Try
            If txt.Text.Trim = "" Then clsManage.alert(Page, "Please Enter Number.", , , "number") : Exit Sub
            Dim num = txt.Text.Trim

            If Not checkBidLimit("down", num) Then Exit Sub
            If txt99Bid1.Text <> "" And txt99Ask1.Text <> "" Then
                txt99Bid1.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Bid1.Text) - clsManage.convert2zero(num))
                txt99Ask1.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Ask1.Text) - clsManage.convert2zero(num))
                refresh()
                clsManage.Script(Page, "colorKeyup()", "button")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub imgUp1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgUp1.Click
        upBid(txtBidChangeNumber1)
    End Sub

    Protected Sub imgUp2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgUp2.Click
        upBid(txtBidChangeNumber2)
    End Sub

    Protected Sub imgUp3_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgUp3.Click
        upBid(txtBidChangeNumber3)
    End Sub

    Protected Sub imgDown1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgDown1.Click
        DownBid(txtBidChangeNumber1)
    End Sub

    Protected Sub imgDown2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgDown2.Click
        DownBid(txtBidChangeNumber2)
    End Sub

    Protected Sub imgDown3_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgDown3.Click
        DownBid(txtBidChangeNumber3)
    End Sub

#End Region


    'Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    'End Sub
    'Protected Sub linkExcel_Click(sender As Object, e As System.EventArgs) Handles linkExcel.Click
    '    Try
    '        clsManage.ExportToExcelTradeOrder(gvTrade, "Order_Sell" + DateTime.Now.ToString("ddmmyyyy"))
    '    Catch ex As Exception
    '        clsManage.alert(Page, ex.Message, , , "err")
    '    End Try
    'End Sub

    'Protected Sub linkExcelBuy_Click(sender As Object, e As System.EventArgs) Handles linkExcelBuy.Click
    '    Try
    '        clsManage.ExportToExcelTradeOrder(gvTradeBuy, "Order_Buy" + DateTime.Now.ToString("ddmmyyyy"))
    '    Catch ex As Exception
    '        clsManage.alert(Page, ex.Message, , , "err")
    '    End Try
    'End Sub

End Class
