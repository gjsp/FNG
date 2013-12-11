Imports System
Imports System.Web
Imports System.Security
Imports System.Web.UI

Imports System.Web.UI.HtmlControls
Imports System.Web.UI.WebControls
Imports System.Web.UI.WebControls.WebParts
'Imports iTextSharp.text
'Imports iTextSharp.text.pdf
Imports iTextSharp.text.html.simpleparser
Imports System.IO


Partial Class ticket_daily
    Inherits basePage
    Dim colBegin As Integer = 7

    Protected Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        If Not Page.IsPostBack Then
            Try
                If Session(clsManage.iSession.user_role_center.ToString) IsNot Nothing Then
                    Dim dtRole As New Data.DataTable : dtRole = Session(clsManage.iSession.user_role_center.ToString)
                    If dtRole.Rows.Find(Me.Page.Page.ToString) IsNot Nothing Then
                        Dim dr As Data.DataRow = dtRole.Rows.Find(Me.Page.Page.ToString)
                        'Print
                        If dr(clsDB.roles.delete) = False Then
                            ViewState(clsDB.roles.delete) = False
                        End If
                        'Export
                        If dr(clsDB.roles.export) = False Then
                            linkExport.Enabled = False
                        End If

                    End If
                End If

            Catch ex As Exception
                clsManage.alert(Page, ex.Message)
            End Try
        End If

        'cbRefresh.Attributes("value") = cbRefresh.Text

        'clsManage.Script(Page, "$get('cbRefresh').checked = 

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Try
                imgSearchCustRef.Attributes.Add("onclick", "openCustomerList('report');return false;")
                gvTicket.EmptyDataText = clsManage.EmptyDataText
                txtCustName.Attributes.Add("onkeypress", "if(event.keyCode==13){$get('" & btnSearchAdv.ClientID & "').focus();}")
                txtTicketRef.Attributes.Add("onkeypress", "if(event.keyCode==13){$get('" & btnSearchAdv.ClientID & "').focus();}")
                txtPrice.Attributes.Add("onkeypress", "checkNumber();")
                txtAmount.Attributes.Add("onkeypress", "checkNumber();")
                If Session(clsManage.iSession.user_id_center.ToString) IsNot Nothing Then
                    ddlSelf.Items.Add(New ListItem("Self", Session(clsManage.iSession.user_id_center.ToString).ToString))
                End If
                ddlSelf.Items.Add(New ListItem("View All", ""))
                'ddlSelf.Attributes.Add("onchange", "$get('" & btnSearchAdv.ClientID & "').click();")
                'ddlRecycle.Attributes.Add("onchange", "$get('" & btnSearchAdv.ClientID & "').click();")
                'ddlTeam.Attributes.Add("onchange", "$get('" & btnSearchAdv.ClientID & "').click();")

                clsManage.getDropDownlistValue(ddlTeam, clsDB.getTeam, "-- All --")
                ddlTeam.Items.Insert(IIf(ddlTeam.Items.Count > 0, 1, 0), New ListItem(clsManage.msgRequireSelect, "none"))
                gvTicket.EmptyDataText = clsManage.EmptyDataText
                'gvTicket.ShowHeader = False
                txtDate.Attributes.Add("onkeypress", "return false;")
                txtDate2.Attributes.Add("onkeypress", "return false;")


                txtDate_CalendarExtender.Format = clsManage.formatDateTime
                txtDate2_CalendarExtender.Format = clsManage.formatDateTime
                'txtDate_CalendarExtender.OnClientDateSelectionChanged = "search"

                For i As Integer = 0 To 23 : ddl1Hour.Items.Add(i) : ddl2Hour.Items.Add(i) : Next
                For i As Integer = 0 To 59 : ddl1Min.Items.Add(i.ToString("00")) : ddl2Min.Items.Add(i.ToString("00")) : Next
                ddl1Hour.SelectedValue = "9" : ddl1Min.SelectedValue = "30"
                ddl2Hour.SelectedValue = "23" : ddl2Min.SelectedValue = "59"


                'If DateTime.Now.Hour > 15 And Request.QueryString("page") IsNot Nothing Then
                '    txtDate.Text = Now.AddDays(1).ToString(clsManage.formatDateTime)
                '    txtDate2.Text = Now.AddDays(1).ToString(clsManage.formatDateTime)
                'Else
                txtDate.Text = Now.ToString(clsManage.formatDateTime)
                txtDate2.Text = Now.ToString(clsManage.formatDateTime)
                'End If

                txtDelDate1_CalendarExtender.Format = clsManage.formatDateTime
                txtDelDate2_CalendarExtender.Format = clsManage.formatDateTime
                txtDelDate1.Attributes.Add("onkeypress", "return false;")
                txtDelDate2.Attributes.Add("onkeypress", "return false;")
                clsManage.getRadioList(cblStatus, clsDB.getTicket_status)

                btnSearchAdv_Click(Nothing, Nothing)

                'get password authen for confirm delete
                hdfPwd.Value = clsDB.getStockField("pwd_auth")

            Catch ex As Exception
                Throw ex
            End Try
        End If

    End Sub

    Function validatetion() As Boolean
        If txtDate.Text.Trim = "" Then gvTicket.DataSourceID = Nothing : clsManage.alert(Page, "Please Select Date.") : Return False
        If txtDate2.Text.Trim = "" Then gvTicket.DataSourceID = Nothing : clsManage.alert(Page, "Please Select Date.") : Return False
        If txtPrice.Text.Trim <> "" And Not IsNumeric(txtPrice.Text) Then gvTicket.DataSourceID = Nothing : clsManage.alert(Page, "Please Number Only.") : Return False
        If txtAmount.Text.Trim <> "" And Not IsNumeric(txtAmount.Text) Then gvTicket.DataSourceID = Nothing : clsManage.alert(Page, "Please Number Only.") : Return False
        If (txtDelDate1.Text.Trim = "" And txtDelDate2.Text.Trim <> "") Or (txtDelDate1.Text.Trim <> "" And txtDelDate2.Text.Trim = "") Then gvTicket.DataSourceID = Nothing : clsManage.alert(Page, "Please Select Date.") : Return False
        Try
            If txtDate.Text.Trim <> "" Then DateTime.ParseExact(txtDate.Text, clsManage.formatDateTime, Nothing)
            If txtDate2.Text.Trim <> "" Then DateTime.ParseExact(txtDate2.Text, clsManage.formatDateTime, Nothing)
            If txtDelDate1.Text.Trim <> "" Then DateTime.ParseExact(txtDelDate1.Text, clsManage.formatDateTime, Nothing)
            If txtDelDate2.Text.Trim <> "" Then DateTime.ParseExact(txtDelDate2.Text, clsManage.formatDateTime, Nothing)
        Catch ex As Exception
            gvTicket.DataSourceID = Nothing
            clsManage.alert(Page, "Invalid Date Time.")
            Return False
        End Try
        Return True
    End Function

    Protected Sub btnSearchAdv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchAdv.Click
        Try
            If Not validatetion() Then Exit Sub
            gvTicket.DataSourceID = objSrcTicket.ID
            Dim status_id As String = clsManage.getItemListSql(cblStatus)
            Dim tempDate1 As DateTime = DateTime.ParseExact(txtDate.Text, clsManage.formatDateTime, Nothing)
            Dim tempDate2 As DateTime = DateTime.ParseExact(txtDate2.Text, clsManage.formatDateTime, Nothing)
            Dim pDate1 As DateTime = pDate1.AddDays(tempDate1.Day - 1).AddMonths(tempDate1.Month - 1).AddYears(tempDate1.Year - 1).AddHours(ddl1Hour.SelectedValue).AddMinutes(ddl1Min.SelectedValue)
            Dim pDate2 As DateTime = pDate2.AddDays(tempDate2.Day - 1).AddMonths(tempDate2.Month - 1).AddYears(tempDate2.Year - 1).AddHours(ddl2Hour.SelectedValue).AddMinutes(ddl2Min.SelectedValue).AddSeconds(59)

            'case 29/2/yyyy
            If pDate1.Day.ToString <> txtDate.Text.Split("/")(0) Then pDate1 = pDate1.AddDays(1)
            If pDate2.Day.ToString <> txtDate2.Text.Split("/")(0) Then pDate2 = pDate2.AddDays(1)

            objSrcTicket.SelectParameters("ticket_id").DefaultValue = txtTicketRef.Text
            objSrcTicket.SelectParameters("book_no").DefaultValue = txtBookNo.Text
            objSrcTicket.SelectParameters("run_no").DefaultValue = txtNo.Text
            objSrcTicket.SelectParameters("cust_name").DefaultValue = txtCustName.Text
            objSrcTicket.SelectParameters("gold_type_id").DefaultValue = rdoGoldType.SelectedValue
            objSrcTicket.SelectParameters("ticket_date").DefaultValue = pDate1
            objSrcTicket.SelectParameters("ticket_date2").DefaultValue = pDate2
            objSrcTicket.SelectParameters("del_date1").DefaultValue = txtDelDate1.Text
            objSrcTicket.SelectParameters("del_date2").DefaultValue = txtDelDate2.Text
            objSrcTicket.SelectParameters("price").DefaultValue = txtPrice.Text
            objSrcTicket.SelectParameters("user_id").DefaultValue = ddlSelf.SelectedValue
            objSrcTicket.SelectParameters("team_id").DefaultValue = ddlTeam.SelectedValue
            objSrcTicket.SelectParameters("status_id").DefaultValue = status_id
            objSrcTicket.SelectParameters("amount").DefaultValue = txtAmount.Text
            objSrcTicket.SelectParameters("type").DefaultValue = rdoType.SelectedValue
            objSrcTicket.SelectParameters("isCenter").DefaultValue = rdoCenter.SelectedValue
            gvTicket.DataBind()
            If gvTicket.Rows.Count > 0 Then
                clsManage.Script(Page, "$get('" + linkExport.ClientID + "').style.display='block';", "block")
            Else
                clsManage.Script(Page, "$get('" + linkExport.ClientID + "').style.display='none'; ", "none")
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try

    End Sub

    Protected Sub gvTicket_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTicket.RowCreated

        If e.Row.RowType = DataControlRowType.Header Then

            'Build custom header. 
            Dim oGridView As GridView = DirectCast(sender, GridView)
            Dim oGridViewRow As New GridViewRow(1, 0, DataControlRowType.Header, DataControlRowState.Insert)
            Dim oTableCell As New TableCell()

            oTableCell.Text = ""
            oTableCell.RowSpan = 2
            oGridViewRow.Cells.Add(oTableCell)

            oTableCell = New TableCell()
            oTableCell.HorizontalAlign = HorizontalAlign.Center
            oTableCell.Text = "Datetime"
            oTableCell.RowSpan = 2
            oGridViewRow.Cells.Add(oTableCell)

            oTableCell = New TableCell()
            oTableCell.HorizontalAlign = HorizontalAlign.Center
            oTableCell.Text = "Ref_no"
            oTableCell.RowSpan = 2
            oGridViewRow.Cells.Add(oTableCell)

            'oTableCell = New TableCell()
            'oTableCell.HorizontalAlign = HorizontalAlign.Center
            'oTableCell.Text = "Book_no"
            'oTableCell.RowSpan = 2
            'oGridViewRow.Cells.Add(oTableCell)

            'oTableCell = New TableCell()
            'oTableCell.HorizontalAlign = HorizontalAlign.Center
            'oTableCell.Text = "No"
            'oTableCell.RowSpan = 2
            'oGridViewRow.Cells.Add(oTableCell)

            oTableCell = New TableCell()
            oTableCell.HorizontalAlign = HorizontalAlign.Center
            oTableCell.Wrap = False
            oTableCell.Text = "Customer Name"
            oTableCell.RowSpan = 2
            oGridViewRow.Cells.Add(oTableCell)

            oTableCell = New TableCell()
            oTableCell.HorizontalAlign = HorizontalAlign.Center
            oTableCell.Text = "Pure"
            oTableCell.RowSpan = 2
            oGridViewRow.Cells.Add(oTableCell)

            e.Row.Cells(0).Visible = False
            e.Row.Cells(1).Visible = False
            e.Row.Cells(2).Visible = False
            e.Row.Cells(3).Visible = False
            e.Row.Cells(4).Visible = False


            oTableCell = New TableCell()
            oTableCell.HorizontalAlign = HorizontalAlign.Center
            oTableCell.Text = "Buy"
            oTableCell.ColumnSpan = 3
            oGridViewRow.Cells.Add(oTableCell)

            oTableCell = New TableCell()
            oTableCell.HorizontalAlign = HorizontalAlign.Center
            oTableCell.Text = "Sell"
            oTableCell.ColumnSpan = 3
            oGridViewRow.Cells.Add(oTableCell)

            oGridView.Controls(0).Controls.AddAt(0, oGridViewRow)


        End If

    End Sub

    Protected Sub gvTicket_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTicket.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            ViewState("sumQuanBuy96") = 0
            ViewState("sumAmountBuy96") = 0
            ViewState("sumQuanBuy99") = 0
            ViewState("sumAmountBuy99") = 0

            ViewState("sumQuanSell96") = 0
            ViewState("sumAmountSell96") = 0
            ViewState("sumQuanSell99") = 0
            ViewState("sumAmountSell99") = 0

            ViewState("PriceSell96") = 0
            ViewState("PriceBuy96") = 0
            ViewState("PriceSell99") = 0
            ViewState("PriceBuy99") = 0

            ViewState("averagePriceSell96") = 0
            ViewState("averagePriceBuy96") = 0
            ViewState("averagePriceSell99") = 0
            ViewState("averagePriceBuy99") = 0

        End If

        If e.Row.RowType = DataControlRowType.DataRow Then

            Dim i As Integer = e.Row.RowIndex
           If e.Row.DataItem("gold_type_name").ToString = "96.50" Then
                ViewState("sumQuanBuy96") += clsManage.convert2zero(e.Row.DataItem("buy_quantity"))
                ViewState("sumAmountBuy96") += clsManage.convert2zero(e.Row.DataItem("buy_amount"))
                ViewState("sumQuanSell96") += clsManage.convert2zero(e.Row.DataItem("sell_quantity"))
                ViewState("sumAmountSell96") += clsManage.convert2zero(e.Row.DataItem("sell_amount"))
            Else
                ViewState("sumQuanBuy99") += clsManage.convert2zero(e.Row.DataItem("buy_quantity"))
                ViewState("sumAmountBuy99") += clsManage.convert2zero(e.Row.DataItem("buy_amount"))
                ViewState("sumQuanSell99") += clsManage.convert2zero(e.Row.DataItem("sell_quantity"))
                ViewState("sumAmountSell99") += clsManage.convert2zero(e.Row.DataItem("sell_amount"))

            End If
            CType(e.Row.FindControl("link"), HyperLink).NavigateUrl = "ticket_deal.aspx?page=dialy&id=" + e.Row.DataItem("ticket_id").ToString
            CType(e.Row.FindControl("linkCust"), HyperLink).NavigateUrl = "customer_detail.aspx?id=" + e.Row.DataItem("cust_id").ToString

            'data from tbl is string
            e.Row.Cells(5).Text = clsManage.convert2Currency(e.Row.DataItem("buy_price"))
            e.Row.Cells(5 + 1).Text = clsManage.convert2Quantity(e.Row.DataItem("buy_quantity"))
            e.Row.Cells(5 + 2).Text = clsManage.convert2Currency(e.Row.DataItem("buy_amount"))

            e.Row.Cells(5 + 3).Text = clsManage.convert2Currency(e.Row.DataItem("sell_price"))
            e.Row.Cells(5 + 4).Text = clsManage.convert2Quantity(e.Row.DataItem("sell_quantity"))
            e.Row.Cells(5 + 5).Text = clsManage.convert2Currency(e.Row.DataItem("sell_amount"))

            If e.Row.DataItem("ticket_id").ToString().Substring(0, 1) = clsFng.strOnline Then
                CType(e.Row.FindControl("imgDel"), ImageButton).Enabled = False
                CType(e.Row.FindControl("imgDel"), ImageButton).ImageUrl = "~/images/i_del2.png"
            End If

            If ViewState(clsDB.roles.delete) IsNot Nothing AndAlso ViewState(clsDB.roles.delete) = False Then
                CType(e.Row.FindControl("imgDel"), ImageButton).Enabled = False
                CType(e.Row.FindControl("imgDel"), ImageButton).ImageUrl = "~/images/i_del2.png"
            End If
        End If

        If e.Row.RowType = DataControlRowType.Footer Then

            Dim buy_quan96 As String = Double.Parse(ViewState("sumQuanBuy96"))
            Dim buy_amount96 As String = Double.Parse(ViewState("sumAmountBuy96"))
            Dim buy_quan99 As String = Double.Parse(ViewState("sumQuanBuy99"))
            Dim buy_amount99 As String = Double.Parse(ViewState("sumAmountBuy99"))

            Dim sell_quan96 As String = Double.Parse(ViewState("sumQuanSell96"))
            Dim sell_amount96 As String = Double.Parse(ViewState("sumAmountSell96"))
            Dim sell_quan99 As String = Double.Parse(ViewState("sumQuanSell99"))
            Dim sell_amount99 As String = Double.Parse(ViewState("sumAmountSell99"))

            Dim averagePriceSell96 As String = "0"
            Dim averagePriceBuy96 As String = "0"
            Dim averagePriceSell99 As String = "0"
            Dim averagePriceBuy99 As String = "0"

            If Double.Parse(ViewState("sumQuanSell96")) <> 0 Then
                averagePriceSell96 = Double.Parse(ViewState("sumAmountSell96")) / Double.Parse(ViewState("sumQuanSell96"))
                ViewState("averagePriceSell96") = averagePriceSell96
            End If
            If Double.Parse(ViewState("sumQuanBuy96")) <> 0 Then
                averagePriceBuy96 = Double.Parse(ViewState("sumAmountBuy96")) / Double.Parse(ViewState("sumQuanBuy96"))
                ViewState("averagePriceBuy96") = averagePriceBuy96
            End If
            If Double.Parse(ViewState("sumQuanSell99")) <> 0 Then
                Dim avgSell99 As Double = Double.Parse(ViewState("sumAmountSell99")) / Double.Parse(ViewState("sumQuanSell99"))
                averagePriceSell99 = avgSell99 / 65.6
                ViewState("averagePriceSell99") = averagePriceSell99
            End If
            If Double.Parse(ViewState("sumQuanBuy99")) <> 0 Then
                Dim avgBuy99 As Double = Double.Parse(ViewState("sumAmountBuy99")) / Double.Parse(ViewState("sumQuanBuy99"))
                averagePriceBuy99 = avgBuy99 / 65.6
                ViewState("averagePriceBuy99") = averagePriceBuy99
            End If


            Dim formatQuan As String = "##0"
            Dim formatAmount As String = "##0"

            Dim creatCels As New SortedList
            creatCels.Add("1", "Total,4,2") ' add rows this
            creatCels.Add("2", "96.50,1,1")
            creatCels.Add("3", "" + Double.Parse(averagePriceBuy96).ToString(formatAmount) + ",1,1")
            creatCels.Add("4", "" + Double.Parse(buy_quan96).ToString(formatQuan) + ",1,1")
            creatCels.Add("5", "" + Double.Parse(buy_amount96).ToString(formatAmount) + ",1,1")
            creatCels.Add("6", "" + Double.Parse(averagePriceSell96).ToString(formatAmount) + ",1,1")
            creatCels.Add("7", "" + Double.Parse(sell_quan96).ToString(formatQuan) + ",1,1")
            creatCels.Add("8", "" + Double.Parse(sell_amount96).ToString(formatAmount) + ",1,1")


            Dim creatCels1 As New SortedList

            creatCels1.Add("2", "99.99,1,1")
            creatCels1.Add("3", "" + Double.Parse(averagePriceBuy99).ToString(formatAmount) + ",1,1")
            creatCels1.Add("4", "" + Double.Parse(buy_quan99).ToString(formatQuan) + ",1,1")
            creatCels1.Add("5", "" + Double.Parse(buy_amount99).ToString(formatAmount) + ",1,1")
            creatCels1.Add("6", "" + Double.Parse(averagePriceSell99).ToString(formatAmount) + ",1,1")
            creatCels1.Add("7", "" + Double.Parse(sell_quan99).ToString(formatQuan) + ",1,1")
            creatCels1.Add("8", "" + Double.Parse(sell_amount99).ToString(formatAmount) + ",1,1")

            Dim balQuan96 As Double = Double.Parse(buy_quan96) - Double.Parse(sell_quan96)
            Dim balQuan99 As Double = Double.Parse(buy_quan99) - Double.Parse(sell_quan99)
            Dim balAmount96 As Double = Double.Parse(buy_amount96) - Double.Parse(sell_amount96)
            Dim balAmount99 As Double = Double.Parse(buy_amount99) - Double.Parse(sell_amount99)
            Dim strBalance As String = String.Format("Quantity 96.50 : {0} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Amount 96.50 : {1} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Quantity 99.99 : {2} &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Amount 99.99 : {3}", balQuan96.ToString(formatQuan), balAmount96.ToString(formatAmount), balQuan99.ToString(formatQuan), balAmount99.ToString(formatAmount))

            Dim creatCels2 As New SortedList
            creatCels2.Add("1", "Balance 96.50/99.99,4,2")
            creatCels2.Add("2", strBalance + ",7,1")

            getMultiFooter(e, creatCels2)
            getMultiFooter(e, creatCels1)
            getMultiFooter(e, creatCels)
        End If
    End Sub

    Private Sub getMultiFooter(ByVal e As GridViewRowEventArgs, ByVal getCels As SortedList)
        Dim row As GridViewRow
        Dim enumCels As IDictionaryEnumerator = getCels.GetEnumerator
        row = New GridViewRow(-1, -1, DataControlRowType.Footer, DataControlRowState.Normal)

        While (enumCels.MoveNext)
            Dim cont As String() = enumCels.Value.ToString.Split(",")
            Dim cell As New TableCell
            cell.RowSpan = cont(2).ToString
            cell.ColumnSpan = cont(1).ToString
            cell.Controls.Add(New LiteralControl(clsManage.convert2Summary(cont(0).ToString)))
            cell.BackColor = New Drawing.ColorConverter().ConvertFromString("#274B98")
            cell.ForeColor = System.Drawing.Color.White
            cell.Font.Bold = True

            If enumCels.Key.ToString = "1" Or enumCels.Key.ToString = "2" Then
                cell.HorizontalAlign = HorizontalAlign.Center
            Else
                cell.HorizontalAlign = HorizontalAlign.Right
            End If

            row.Cells.Add(cell)

            e.Row.Parent.Controls.AddAt(gvTicket.Rows.Count + 3, row)
        End While
    End Sub

    Protected Sub linkExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkExport.Click
        Dim dt As New Data.DataTable
        Dim status As String = clsManage.getItemListSql(cblStatus)

        Dim tempDate1 As DateTime = DateTime.ParseExact(txtDate.Text, clsManage.formatDateTime, Nothing)
        Dim tempDate2 As DateTime = DateTime.ParseExact(txtDate2.Text, clsManage.formatDateTime, Nothing)
        Dim pDate1 As DateTime = pDate1.AddDays(tempDate1.Day - 1).AddMonths(tempDate1.Month - 1).AddYears(tempDate1.Year - 1).AddHours(ddl1Hour.SelectedValue).AddMinutes(ddl1Min.SelectedValue)
        Dim pDate2 As DateTime = pDate2.AddDays(tempDate2.Day - 1).AddMonths(tempDate2.Month - 1).AddYears(tempDate2.Year - 1).AddHours(ddl2Hour.SelectedValue).AddMinutes(ddl2Min.SelectedValue).AddSeconds(59)

        'case 29/2/yyyy
        If pDate1.Day.ToString <> txtDate.Text.Split("/")(0) Then pDate1 = pDate1.AddDays(1)
        If pDate2.Day.ToString <> txtDate2.Text.Split("/")(0) Then pDate2 = pDate2.AddDays(1)

        dt = clsDB.getTicketDialy(txtTicketRef.Text, txtBookNo.Text, txtNo.Text, txtCustName.Text, rdoGoldType.SelectedValue, pDate1, pDate2, txtDelDate1.Text, txtDelDate2.Text, txtPrice.Text, ddlSelf.SelectedValue, ddlTeam.SelectedValue, ddlRecycle.SelectedValue, status, txtAmount.Text, rdoType.SelectedValue, rdoCenter.SelectedValue)
        If dt.Rows.Count > 0 Then
            Dim htSum As New Hashtable

            htSum.Add("qb96", ViewState("sumQuanBuy96"))
            htSum.Add("ab96", ViewState("sumAmountBuy96"))
            htSum.Add("qs96", ViewState("sumQuanSell96"))
            htSum.Add("as96", ViewState("sumAmountSell96"))

            htSum.Add("qb99", ViewState("sumQuanBuy99"))
            htSum.Add("ab99", ViewState("sumAmountBuy99"))
            htSum.Add("qs99", ViewState("sumQuanSell99"))
            htSum.Add("as99", ViewState("sumAmountSell99"))

            htSum.Add("avs96", ViewState("averagePriceSell96"))
            htSum.Add("avb96", ViewState("averagePriceBuy96"))
            htSum.Add("avs99", ViewState("averagePriceSell99"))
            htSum.Add("avb99", ViewState("averagePriceBuy99"))

            clsManage.ExportSummaryOrderToPDF(dt, "Summary_Order_" + txtDate.Text, htSum, Server.MapPath(""))

        End If
    End Sub

    Public Overloads Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    End Sub

    Protected Sub upgSearch_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles upgSearch.Load
        System.Threading.Thread.Sleep(upgSearch.DisplayAfter)
    End Sub

    Protected Sub imgDel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)

        Dim i As Integer = CType(CType(sender, ImageButton).Parent.Parent, GridViewRow).RowIndex
        Dim id As String = gvTicket.DataKeys(i).Value
        Try
            Dim result As String = ""
            If ddlRecycle.SelectedIndex = 0 Then
                result = clsDB.updateActiveTicket(id, False, Session(clsManage.iSession.user_id_center.ToString).ToString)
            Else
                result = clsDB.updateActiveTicket(id, True, Session(clsManage.iSession.user_id_center.ToString).ToString)
            End If
            gvTicket.DataBind()
            If result <> "" Then clsManage.alert(Page, result)
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "error")
        End Try
    End Sub

End Class
