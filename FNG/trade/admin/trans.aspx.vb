
Partial Class admin_trans
    Inherits System.Web.UI.Page
    Dim htPrice As Hashtable

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.QueryString("m") IsNot Nothing Then
                If Request.QueryString("m").ToString = "leave" Then
                    hdfMode.Value = clsManage.tradeMode.tran.ToString
                Else
                    hdfMode.Value = Request.QueryString("m").ToString
                End If


                hdfTradeLogId.Value = clsMain.getTradeLogId(hdfMode.Value)
                hdfTradeLogIdForGrid.Value = hdfTradeLogId.Value
                gvTrade.EmptyDataText = clsManage.EmptyDataText
                'rdoType.Attributes.Add("onclick", "refreshGrid()")
                'cbPurity.Items(0).Attributes.Add("onclick", "refreshGrid()")
                'cbPurity.Items(1).Attributes.Add("onclick", "refreshGrid()")
                'cbPurity.Items(2).Attributes.Add("onclick", "refreshGrid()")
                hdfIsRealtime.Value = "y"
                hdfConfirm.Value = "n"
                For i As Integer = 0 To 23 : ddl1Hour.Items.Add(i) : ddl2Hour.Items.Add(i) : ddl3Hour.Items.Add(i) : Next
                For i As Integer = 0 To 59 : ddl1Min.Items.Add(i.ToString("00")) : ddl2Min.Items.Add(i.ToString("00")) : ddl3Min.Items.Add(i.ToString("00")) : Next
                ddl1Hour.SelectedValue = "9" : ddl1Min.SelectedValue = "30"
                ddl2Hour.SelectedValue = "23" : ddl2Min.SelectedValue = "59"
                ddl3Hour.SelectedValue = "9" : ddl3Min.SelectedValue = "30"
                txt1Date_CalendarExtender.Format = clsManage.formatDateTime : txt2Date_CalendarExtender.Format = clsManage.formatDateTime : txt3Date_CalendarExtender.Format = clsManage.formatDateTime
                txt1Date.Text = Now.ToString(clsManage.formatDateTime) : txt2Date.Text = Now.ToString(clsManage.formatDateTime) : txt3Date.Text = Now.ToString(clsManage.formatDateTime)


                Dim dt As New Data.DataTable
                dt = clsMain.getPriceOnline()
                Dim dr As Data.DataRow = dt.Rows(0)
                'set Price to Dictinary for check current price
                htPrice = New Hashtable
                htPrice.Add("bid99_1", clsManage.convert2zero(dr("bid99_1")))
                htPrice.Add("bid99_2", clsManage.convert2zero(dr("bid99_2")))
                htPrice.Add("bid99_3", clsManage.convert2zero(dr("bid99_3")))

                htPrice.Add("ask99_1", clsManage.convert2zero(dr("ask99_1")))
                htPrice.Add("ask99_2", clsManage.convert2zero(dr("ask99_2")))
                htPrice.Add("ask99_3", clsManage.convert2zero(dr("ask99_3")))

                htPrice.Add("bid96_1", clsManage.convert2zero(dr("bid96_1")))
                htPrice.Add("bid96_2", clsManage.convert2zero(dr("bid96_2")))
                htPrice.Add("bid96_3", clsManage.convert2zero(dr("bid96_3")))

                htPrice.Add("ask96_1", clsManage.convert2zero(dr("ask96_1")))
                htPrice.Add("ask96_2", clsManage.convert2zero(dr("ask96_2")))
                htPrice.Add("ask96_3", clsManage.convert2zero(dr("ask96_3")))
                ViewState("price") = htPrice

            End If
        End If
    End Sub

    Protected Sub gvTrade_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTrade.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            htPrice = CType(ViewState("price"), Hashtable)
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then

            'Change Wording
            If e.Row.DataItem("leave_order").ToString = "order" Then
                e.Row.Cells(1).Text = "Order"
            Else
                e.Row.Cells(1).Text = "Leave Order"
            End If
            If e.Row.DataItem("type").ToString = "sell" Then
                e.Row.Cells(3).Text = "Sell"
            Else
                e.Row.Cells(3).Text = "Buy"
            End If

            CType(e.Row.FindControl("btnAccept99"), ImageButton).Attributes.Add("onclick", "return fastConfirm('" + hdfConfirm.ClientID + "','Do you want to accept.');")
            CType(e.Row.FindControl("btnReject99"), ImageButton).Attributes.Add("onclick", "return fastConfirm('" + hdfConfirm.ClientID + "','Do you want to reject.');")

            'If e.Row.DataItem("gold_type_id").ToString = "96" Then
            '    If e.Row.DataItem("type").ToString = "sell" Then
            '        Me.ViewState("sum_sell_96") += clsManage.convert2zero(e.Row.DataItem("quantity"))
            '    Else
            '        Me.ViewState("sum_buy_96") += clsManage.convert2zero(e.Row.DataItem("quantity"))
            '    End If
            'Else
            '    If e.Row.DataItem("type").ToString = "sell" Then
            '        Me.ViewState("sum_sell_99") += clsManage.convert2zero(e.Row.DataItem("quantity"))
            '    Else
            '        Me.ViewState("sum_buy_99") += clsManage.convert2zero(e.Row.DataItem("quantity"))
            '    End If
            'End If

            ''check customer leave order is visible
            'If Session(clsManage.iSession.cust_id.ToString) IsNot Nothing And hdfMode.Value = clsManage.tradeMode.tran.ToString Then

            'End If
            'check unvisible not tran from admin
            If hdfMode.Value = clsManage.tradeMode.tran.ToString Then
                If Session(clsManage.iSession.cust_id.ToString) Is Nothing Then 'admin
                    If e.Row.DataItem("mode").ToString = clsManage.tradeMode.tran.ToString Then
                        CType(e.Row.FindControl("btnReject99"), ImageButton).Visible = True
                        CType(e.Row.FindControl("btnAccept99"), ImageButton).Visible = True
                        If e.Row.DataItem("leave_order").ToString <> "order" Then
                            Dim columnName As String = IIf(e.Row.DataItem("type").ToString = "sell", "bid", "ask") + IIf(e.Row.DataItem("gold_type_id").ToString = "96", "96", "99") + "_" + e.Row.DataItem("cust_level").ToString
                            If clsManage.convert2zero(e.Row.DataItem("price").ToString) = clsManage.convert2zero(htPrice(columnName).ToString) Then
                                e.Row.BackColor = Drawing.Color.Pink
                            End If
                        End If
                    Else
                        CType(e.Row.FindControl("btnReject99"), ImageButton).Visible = False
                        CType(e.Row.FindControl("btnAccept99"), ImageButton).Visible = False
                    End If

                Else 'customer
                    If e.Row.DataItem("mode").ToString = clsManage.tradeMode.tran.ToString Then
                        If e.Row.DataItem("leave_order").ToString <> "order" Then
                            CType(e.Row.FindControl("btnReject99"), ImageButton).Visible = True
                        Else
                            CType(e.Row.FindControl("btnReject99"), ImageButton).Visible = False
                        End If
                    Else
                        CType(e.Row.FindControl("btnReject99"), ImageButton).Visible = False
                    End If
                End If
            End If

            'If e.Row.DataItem("new_trade").ToString = "y" Then
            '    'e.Row.BackColor = New Drawing.ColorConverter().ConvertFromString("#FFCE9D")
            '    e.Row.Font.Bold = True
            'End If

            If e.Row.DataItem("mode") = clsManage.tradeMode.reject.ToString And e.Row.DataItem("reject_type").ToString <> "" Then
                e.Row.Cells(7).Text = "Reject By " + e.Row.DataItem("reject_type").ToString
            End If

            If hdfMode.Value = clsManage.tradeMode.tran.ToString Then
                If e.Row.DataItem("mode") = clsManage.tradeMode.reject.ToString Then
                    e.Row.BackColor = New Drawing.ColorConverter().ConvertFromString("#FFA4A4") '#FFA4A4")
                ElseIf e.Row.DataItem("mode") = clsManage.tradeMode.accept.ToString Then
                    e.Row.BackColor = New Drawing.ColorConverter().ConvertFromString("#66CCFF") '"#66CCFF")

                ElseIf e.Row.DataItem("mode") = clsManage.tradeMode.tran.ToString Then
                    'e.Row.BackColor = New Drawing.ColorConverter().ConvertFromString("#DFDFDF")
                End If
            End If
            If hdfMode.Value = clsManage.tradeMode.accept.ToString Then
                If e.Row.DataItem("type") = "sell" Then
                    e.Row.BackColor = New Drawing.ColorConverter().ConvertFromString("#EEEEEE")
                Else
                    e.Row.BackColor = New Drawing.ColorConverter().ConvertFromString("#B8CCE4")
                End If
            End If

        End If
    End Sub

#Region "Update Mode"
    Sub updateMode(ByVal sender As Object, ByVal gv As GridView, ByVal mode As String)
        Try
            Dim i As Integer = CType(CType(sender, ImageButton).Parent.Parent, GridViewRow).RowIndex
            Dim id As String = gv.DataKeys(i).Value
            Dim type As String = gv.DataKeys(i).Values(1).ToString
            Dim price As String = gv.DataKeys(i).Values(2).ToString
            Dim purity As String = gv.DataKeys(i).Values(3).ToString

            'validate only accept mode
            If mode = clsManage.tradeMode.accept.ToString Then
                If Not clsMain.checkPriceLimitLeaveOrder(id, price, purity, type) Then
                    If type = "sell" Then
                        clsManage.alert(Page, "Price Bid Higher than Ask", , , "safetylevel")
                        Exit Sub
                    Else
                        clsManage.alert(Page, "Price Ask Lower then Bid", , , "safetylevel")
                        Exit Sub
                    End If
                End If
            End If

            Dim result As Integer = clsMain.UpdateTicketPortfolioByMode(id, mode, Me.Session(clsManage.iSession.user_id.ToString).ToString, type, price, purity, "", mode)
            If result > 0 Then
                'plus for not update data
                hdfTradeLogId.Value = clsManage.convert2zero(hdfTradeLogId.Value) + 1
            End If
            refreshGrid()
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub
    Protected Sub btnAccept96_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        updateMode(sender, CType(CType(sender, ImageButton).Parent.Parent.NamingContainer, GridView), clsManage.tradeMode.accept.ToString)
    End Sub
    Protected Sub btnAccept99_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        updateMode(sender, CType(CType(sender, ImageButton).Parent.Parent.NamingContainer, GridView), clsManage.tradeMode.accept.ToString)
    End Sub
    Protected Sub btnReject96_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        updateMode(sender, CType(CType(sender, ImageButton).Parent.Parent.NamingContainer, GridView), clsManage.tradeMode.reject.ToString)
    End Sub
    Protected Sub btnReject99_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        updateMode(sender, CType(CType(sender, ImageButton).Parent.Parent.NamingContainer, GridView), clsManage.tradeMode.reject.ToString)
    End Sub
#End Region

    <System.Web.Services.WebMethod()> _
    Public Shared Function getTradeMaxId(ByVal mode As String) As String
        Return clsMain.getTradeLogId(mode)
    End Function

    Protected Sub upGrid_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles upGrid.Load
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
            objSrcTrade.SelectParameters("type").DefaultValue = rdoType.SelectedValue
            objSrcTrade.SelectParameters("purity").DefaultValue = purity
            objSrcTrade.SelectParameters("cust_id").DefaultValue = cust_id
            objSrcTrade.SelectParameters("max_trade_id").DefaultValue = hdfTradeLogIdForGrid.Value
            objSrcTrade.SelectParameters("period").DefaultValue = period
            objSrcTrade.SelectParameters("pDate1").DefaultValue = pDate1
            objSrcTrade.SelectParameters("pDate2").DefaultValue = pDate2
            objSrcTrade.SelectParameters("onlyLeave").DefaultValue = onlyLeave

            gvTrade.DataBind()
           
            'set Gridview(background-color,show column)
            'set iframe height
            Dim strColor As String = ""
            Dim iFrameMode As String = ""
            Dim excelName As String = ""
            'Dim scriptRefreshSumAccept As String = ""

            If hdfMode.Value = clsManage.tradeMode.tran.ToString Then
                If Request.QueryString("m").ToString = "leave" Then
                    strColor = "#808080"
                    iFrameMode = "ifmLeave"
                    excelName = "Leave_Order_"
                    gvTrade.Columns(gvTrade.Columns.Count - 3).Visible = False
                Else
                    strColor = "#4F9502"
                    iFrameMode = "ifmTran"
                    excelName = "Order_"
                End If

                'ถ้าเป็น customer ไม่ต้องโชว์ปุ่ม update mode and reject
                If Session(clsManage.iSession.cust_id.ToString) IsNot Nothing Then
                    gvTrade.Columns(gvTrade.Columns.Count - 1).Visible = True
                    gvTrade.Columns(gvTrade.Columns.Count - 2).Visible = False
                Else
                    gvTrade.Columns(gvTrade.Columns.Count - 1).Visible = True
                    gvTrade.Columns(gvTrade.Columns.Count - 2).Visible = True
                End If

            ElseIf hdfMode.Value = clsManage.tradeMode.accept.ToString Then
                strColor = "#104587"
                iFrameMode = "ifmAcc"
                excelName = "Order_Accept_"
                gvTrade.Columns(7).Visible = False 'colomn mode
                gvTrade.Columns(gvTrade.Columns.Count - 1).Visible = False
                gvTrade.Columns(gvTrade.Columns.Count - 2).Visible = False
                'If cust_id = "" Then scriptRefreshSumAccept = "top.window.document.getElementById('ctl00_ContentPlaceHolder1_btnRefresh').click()"
            ElseIf hdfMode.Value = clsManage.tradeMode.reject.ToString Then
                strColor = "#990000"
                iFrameMode = "ifmRej"
                excelName = "Order_Reject_"
                'gvTrade.Columns(7).Visible = False 'colomn mode
                gvTrade.Columns(gvTrade.Columns.Count - 1).Visible = False
                gvTrade.Columns(gvTrade.Columns.Count - 2).Visible = False
            End If
            Dim h As Integer = 220
            If gvTrade.Rows.Count <> 0 Then
                h = 140 + (gvTrade.Rows.Count) * 25
            End If

            Dim scriptSummary As String = ""
            If hdfMode.Value = clsManage.tradeMode.accept.ToString And cust_id = "" Then
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
                Dim net96 As String = clsManage.convert2Quantity(clsManage.convert2zero(sell96) - clsManage.convert2zero(buy96))
                Dim net99 As String = clsManage.convert2Quantity(clsManage.convert2zero(sell99) - clsManage.convert2zero(buy99))

                'ส่งออกนอก iframe
                scriptSummary = String.Format("top.window.setSummary('{0}','{1}','{2}','{3}','{4}','{5}');", _
                clsManage.convert2Quantity(buy96), clsManage.convert2Quantity(sell96), net96, clsManage.convert2Quantity(buy99), clsManage.convert2Quantity(sell99), net99)

            End If
            Dim script_excel As String = ""
            If gvTrade.Rows.Count > 0 Then
                script_excel = "div_excel.style.display='block';"
            Else
                script_excel = "div_excel.style.display='none';"
            End If
            ViewState("excel_name") = excelName
            clsManage.Script(Page, "setIframeHeight('" + iFrameMode + "','" & h & "');" + scriptSummary + script_excel + "")
            gvTrade.HeaderStyle.BackColor = New Drawing.ColorConverter().ConvertFromString(strColor)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        refreshGrid()
    End Sub

    Protected Sub UpdateProg1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles UpdateProg1.Load
        System.Threading.Thread.Sleep(UpdateProg1.DisplayAfter + 300)
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    End Sub
    Protected Sub linkExcel_Click(sender As Object, e As System.EventArgs) Handles linkExcel.Click
        Try
            clsManage.ExportToExcelTradeOrder(gvTrade, ViewState("excel_name").ToString + DateTime.Now.ToString("ddmmyyyy"))
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "err")
        End Try
    End Sub
End Class
