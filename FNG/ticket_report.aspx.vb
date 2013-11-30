
Partial Class ticket_report
    Inherits basePage

    Protected Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        If Not Page.IsPostBack Then
            Try
                If Session(clsManage.iSession.user_role_center.ToString) IsNot Nothing Then
                    Dim dtRole As New Data.DataTable : dtRole = Session(clsManage.iSession.user_role_center.ToString)
                    If dtRole.Rows.Find(Me.Page.Page.ToString) IsNot Nothing Then
                        Dim dr As Data.DataRow = dtRole.Rows.Find(Me.Page.Page.ToString)
                        If dr(clsDB.roles.export) = False Then
                            linkExcel.Enabled = False
                        End If
                    End If
                End If
            Catch ex As Exception
                clsManage.alert(Page, ex.Message)
            End Try
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            imgSearchCustRef.Attributes.Add("onclick", "openCustomerList('report');return false;")
            gvTicket.EmptyDataText = clsManage.EmptyDataText
            txtDate1.Attributes.Add("onkeypress", "return false;")
            txtDate2.Attributes.Add("onkeypress", "return false;")
            txtDelDate1.Attributes.Add("onkeypress", "return false;")
            txtDelDate2.Attributes.Add("onkeypress", "return false;")
            txtCustName.Attributes.Add("onkeypress", "if(event.keyCode==13){$get('" & btnSearch.ClientID & "').focus();}")
            txtTicketRef.Attributes.Add("onkeypress", "if(event.keyCode==13){$get('" & btnSearch.ClientID & "').focus();}")
            'ddl_goldtype.Attributes.Add("onchange", "$get('" & btnSearchAdv.ClientID & "').click();")
            'ddl_status.Attributes.Add("onchange", "$get('" & btnSearchAdv.ClientID & "').click();")
            txtAmount.Attributes.Add("onkeypress", "checkNumber();")
            txtPrice.Attributes.Add("onkeypress", "checkNumber();")
            clsManage.getRadioList(cblStatus, clsDB.getTicket_status)
            'clsManage.getDropDownlistValue(ddl_goldtype, clsDB.getGoldType, "-- All --")
            'clsManage.getDropDownlistValue(ddl_status, clsDB.getTicket_status, "-- All --")
            'ddlTeam.Attributes.Add("onchange", "$get('" & btnSearch.ClientID & "').click();")
            'ddlRecycle.Attributes.Add("onchange", "$get('" & btnSearch.ClientID & "').click();")
            'rdoType.Attributes.Add("onclick", "$get('" & btnSearch.ClientID & "').click();")
            'clsManage.getDropDownlistValue(ddlTeam, clsDB.getTeam, "-- All --")
            'ddlTeam.Items.Insert(IIf(ddlTeam.Items.Count > 0, 1, 0), New ListItem(clsManage.msgRequireSelect, "none"))
            txtDate1_CalendarExtender.Format = clsManage.formatDateTime
            txtDate2_CalendarExtender.Format = clsManage.formatDateTime
            txtDelDate1_CalendarExtender.Format = clsManage.formatDateTime
            txtDelDate2_CalendarExtender.Format = clsManage.formatDateTime
            txtDate1.Text = Now.ToString(clsManage.formatDateTime)
            txtDate2.Text = Now.ToString(clsManage.formatDateTime)

            btnSearch_Click(Nothing, Nothing)
        End If
    End Sub

    Protected Sub upgSearch_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles upgSearch.Load
        System.Threading.Thread.Sleep(upgSearch.DisplayAfter)
    End Sub

    Function validatetion() As Boolean

        If txtPrice.Text.Trim <> "" And Not IsNumeric(txtPrice.Text) Then clsManage.alert(Page, "Please Number Only.") : Return False
        If txtAmount.Text.Trim <> "" And Not IsNumeric(txtAmount.Text) Then clsManage.alert(Page, "Please Number Only.") : Return False
        If txtPrice.Text.Trim <> "" And Not IsNumeric(txtPrice.Text) Then clsManage.alert(Page, "Please Number Only.") : Return False
        'If (txtDate1.Text.Trim = "" And txtDate2.Text.Trim <> "") Or (txtDate1.Text.Trim <> "" And txtDate2.Text.Trim = "") Then gvTicket.DataSourceID = Nothing : clsManage.alert(Page, "Please Select Date.") : Return False
        'If (txtDelDate1.Text.Trim = "" And txtDelDate2.Text.Trim <> "") Or (txtDelDate1.Text.Trim <> "" And txtDelDate2.Text.Trim = "") Then gvTicket.DataSourceID = Nothing : clsManage.alert(Page, "Please Select Date.") : Return False
        Try
            If txtDate1.Text.Trim <> "" Then DateTime.ParseExact(txtDate1.Text, clsManage.formatDateTime, Nothing)
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

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            If Not validatetion() Then Exit Sub

            'If (txtDate1.Text <> "" And txtDate2.Text = "") OrElse (txtDate1.Text = "" And txtDate2.Text <> "") Then clsManage.alert(Page, "Please Select Date") : Exit Sub
            'If (txtDelDate1.Text <> "" And txtDelDate2.Text = "") OrElse (txtDelDate1.Text = "" And txtDelDate2.Text <> "") Then clsManage.alert(Page, "Please Select Date") : Exit Sub
            gvTicket.DataSourceID = objSrcTicket.ID
            'Dim purity As String = clsManage.getItemListSql(cblPurity)
            Dim status_id As String = clsManage.getItemListSql(cblStatus)

            objSrcTicket.SelectParameters("ticket_id").DefaultValue = txtTicketRef.Text
            objSrcTicket.SelectParameters("cust_name").DefaultValue = txtCustName.Text
            objSrcTicket.SelectParameters("type").DefaultValue = rdoType.SelectedValue
            objSrcTicket.SelectParameters("billing").DefaultValue = ""
            objSrcTicket.SelectParameters("status_id").DefaultValue = status_id
            objSrcTicket.SelectParameters("gold_type_id").DefaultValue = rdoPurity.SelectedValue

            objSrcTicket.SelectParameters("ticket_date1").DefaultValue = txtDate1.Text
            objSrcTicket.SelectParameters("ticket_date2").DefaultValue = txtDate2.Text

            objSrcTicket.SelectParameters("del_date1").DefaultValue = txtDelDate1.Text
            objSrcTicket.SelectParameters("del_date2").DefaultValue = txtDelDate2.Text
            objSrcTicket.SelectParameters("created_by").DefaultValue = Session(clsManage.iSession.user_id_center.ToString)
            objSrcTicket.SelectParameters("amount").DefaultValue = txtAmount.Text
            objSrcTicket.SelectParameters("price").DefaultValue = txtPrice.Text
            objSrcTicket.SelectParameters("quantity").DefaultValue = txtQuantity.Text
            objSrcTicket.SelectParameters("isCenter").DefaultValue = rdoCenter.SelectedValue

            gvTicket.DataBind()
            If gvTicket.Rows.Count > 0 Then
                'เก็บ html ไว้ก่อน เพราะมี column 2 row ทำให้ไม่เป็น row 0 ของ data
                htmlExcel(gvTicket)
            End If

        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try

    End Sub

    Private Sub getMultiHeader(ByVal e As GridViewRowEventArgs, ByVal getCels As SortedList)
        Dim row As GridViewRow
        Dim enumCels As IDictionaryEnumerator = getCels.GetEnumerator
        row = New GridViewRow(-1, -1, DataControlRowType.Header, DataControlRowState.Normal)

        While (enumCels.MoveNext)
            Dim cont As String() = enumCels.Value.ToString.Split(",")
            Dim cell As New TableCell
            cell.RowSpan = cont(2).ToString
            cell.ColumnSpan = cont(1).ToString
            cell.Controls.Add(New LiteralControl(clsManage.convert2Summary(cont(0).ToString)))
            cell.HorizontalAlign = HorizontalAlign.Center
            cell.BackColor = New Drawing.ColorConverter().ConvertFromString("#CCCCCC")
            cell.ForeColor = System.Drawing.Color.Black
            cell.Font.Bold = True
            row.Cells.Add(cell)
            If IsNumeric(cont(0)) Then
                cell.HorizontalAlign = HorizontalAlign.Right
            End If

            e.Row.Parent.Controls.AddAt(0, row)
        End While
    End Sub

    Protected Sub gvTicket_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTicket.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.Header Then
                Dim dt As New Data.DataTable
                Dim purity As String = rdoPurity.SelectedValue 'clsManage.getItemListSql(cblPurity)
                Dim status As String = clsManage.getItemListSql(cblStatus)

                dt = clsDB.getTicketReport(txtTicketRef.Text, txtCustName.Text, rdoType.SelectedValue, "", status, purity, txtDate1.Text, txtDate2.Text, txtDelDate1.Text, txtDelDate2.Text, Session(clsManage.iSession.user_id_center.ToString), txtAmount.Text, txtPrice.Text, txtQuantity.Text, ddlRecycle.SelectedValue, rdoCenter.SelectedValue)
                'Dim drv As Data.DataRowView = e.Row.DataItem
                'dt = drv.DataView.Table

                Dim sumQuan96G As String = ""
                Dim sumQuan96 As String = ""
                Dim sumQuan99 As String = ""
                Dim sumAmount As String = ""
                If dt.Rows.Count > 0 Then
                    sumQuan96G = dt.Compute("SUM(quan96G)", "").ToString
                    sumQuan96 = dt.Compute("SUM(quan96)", "").ToString
                    sumQuan99 = dt.Compute("SUM(quan99)", "").ToString
                    sumAmount = dt.Compute("SUM(amount)", "").ToString

                    ViewState("sumQuan96G") = sumQuan96G
                    ViewState("sumQuan96") = sumQuan96
                    ViewState("sumQuan99") = sumQuan99
                    ViewState("sumAmount") = sumAmount
                End If

                Dim creatCels As New SortedList

                creatCels.Add("00", "Summary,5,1")
                creatCels.Add("01", ",1,1")
                creatCels.Add("02", "" + sumQuan96G.ToString + ",1,1")
                creatCels.Add("03", "" + sumQuan96.ToString + ",1,1")
                creatCels.Add("04", "" + sumQuan99.ToString + ",1,1")
                creatCels.Add("05", "" + sumAmount.ToString + ",1,1")

                creatCels.Add("06", ",1,1")
                creatCels.Add("07", ",1,1")
                creatCels.Add("08", ",1,1")
                creatCels.Add("09", ",1,1")
                creatCels.Add("10", ",1,1")
                getMultiHeader(e, creatCels)

            End If

            If e.Row.RowType = DataControlRowType.DataRow Then

                If e.Row.DataItem("status_id") = "901" Then
                    e.Row.Cells(gvTicket.Columns.Count - 1).Attributes.Add("style", "color:green")
                End If
                If e.Row.DataItem("status_id") = "902" Then
                    e.Row.Cells(gvTicket.Columns.Count - 1).Attributes.Add("style", "color:blue")
                End If

                'ViewState("sumQuan") += Double.Parse(e.Row.DataItem("quantity"))
                'ViewState("sumAmount") += Double.Parse(e.Row.DataItem("amount"))

                CType(e.Row.FindControl("linkLog"), HyperLink).Text = CDate(e.Row.DataItem("ticket_date")).ToString("dd/MM/yyyy HH:mm")
                CType(e.Row.FindControl("linkLog"), HyperLink).NavigateUrl = "ticket_log_detail.aspx?id=" + e.Row.DataItem("ticket_id").ToString

                'ถ้าเป็นสถานะ delete ไม่ต้องโชว์ link
                If ddlRecycle.SelectedValue = "y" Then
                    CType(e.Row.FindControl("link"), HyperLink).NavigateUrl = "ticket_deal.aspx?page=dialy&id=" + e.Row.DataItem("ticket_id").ToString
                End If

                CType(e.Row.FindControl("linkCust"), HyperLink).NavigateUrl = "customer_detail.aspx?id=" + e.Row.DataItem("cust_id").ToString
                'If e.Row.DataItem("billing") = "y" Then
                '    CType(e.Row.FindControl("cbBill"), CheckBox).Checked = True
                'Else
                '    CType(e.Row.FindControl("cbBill"), CheckBox).Checked = False
                'End If

            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub gvTicket_SelectedIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSelectEventArgs) Handles gvTicket.SelectedIndexChanging
        Dim id As String = gvTicket.DataKeys(e.NewSelectedIndex).Value
        Dim page_name As String = "view" 'gvTicket.Rows(e.NewSelectedIndex).Cells(gvTicket.Columns.Count - 1).Text
        clsManage.Script(Page, "top.window.location.href='ticket_deal.aspx?page=" & page_name & "&id=" & id & " '")
    End Sub

    Protected Sub linkExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkExcel.Click
        Try
            If gvTicket.Rows.Count > 0 Then

                Dim hp As New HtmlToPdf(ViewState("htmlExcel").ToString(), Server.MapPath("") + "\font\TAHOMA.TTF", 4)
                hp.Render(HttpContext.Current.Response, "TicketReport__" & txtDate1.Text & "" + ".pdf")

            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, "error")
        End Try
    End Sub

    Private Sub htmlExcel(ByVal gv As GridView)
        Try
            If gvTicket.Rows.Count > 0 Then
                Dim htSum As New Hashtable

                htSum.Add("Quan96G", ViewState("sumQuan96G"))
                htSum.Add("Quan96", ViewState("sumQuan96"))
                htSum.Add("Quan99", ViewState("sumQuan99"))
                htSum.Add("Amount", ViewState("sumAmount"))
                Dim htmlHead As String = ""
                htmlHead += "<table cellspacing='0' rules='all' border='.3' style='border-collapse:collapse;'>"
                htmlHead += "<TR>"
                htmlHead += "<TD style='BACKGROUND-COLOR: #808080; color: white; FONT-WEIGHT: bold; text-align:center' colSpan=8 >Summary</TD>"
                htmlHead += "<TD style='BACKGROUND-COLOR: #808080; COLOR: white; FONT-WEIGHT: bold' colSpan=1 align=right>{0}</TD>"
                htmlHead += "<TD style='BACKGROUND-COLOR: #808080; COLOR: white; FONT-WEIGHT: bold' colSpan=1 align=right>{1}</TD>"
                htmlHead += "<TD style='BACKGROUND-COLOR: #808080; COLOR: white; FONT-WEIGHT: bold' colSpan=1 align=right>{2}</TD>"
                htmlHead += "<TD style='BACKGROUND-COLOR: #808080; COLOR: white; FONT-WEIGHT: bold' colSpan=1 align=right>{3}</TD>"
                htmlHead += "<TD style='BACKGROUND-COLOR: #808080; COLOR: white; FONT-WEIGHT: bold' colSpan=1 align=right></TD>"
                htmlHead += "<TD style='BACKGROUND-COLOR: #808080; COLOR: white; FONT-WEIGHT: bold' colSpan=1 align=right></TD>"
                htmlHead += "<TD style='BACKGROUND-COLOR: #808080; COLOR: white; FONT-WEIGHT: bold' colSpan=1 align=right></TD>"
                htmlHead += "<TD style='BACKGROUND-COLOR: #808080; COLOR: white; FONT-WEIGHT: bold' colSpan=1 align=right></TD>"
                htmlHead += "<TD style='BACKGROUND-COLOR: #808080; COLOR: white; FONT-WEIGHT: bold' colSpan=1 align=right></TD>"
                htmlHead += "</TR>"
                htmlHead += "<tr>"
                htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold; text-align:center' colSpan=2 >Date</TD>"
                htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold; text-align:center' colSpan=1 >ticket_ref</TD>"
                htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold; text-align:center' colSpan=2 >Customer</TD>"
                htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold; text-align:center' colSpan=1 >Sell/Buy</TD>"
                htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold; text-align:center' colSpan=1 >Purity</TD>"
                htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold; text-align:center' colSpan=1 >Price</TD>"
                htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold; text-align:center' colSpan=1 >Q96.50(กรัม)</TD>"
                htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold; text-align:center' colSpan=1 >Q96.50(บาท)</TD>"
                htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold; text-align:center' colSpan=1 >Q99.99(kg)</TD>"
                htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold; text-align:center' colSpan=1 >Amount</TD>"
                htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold; text-align:center' colSpan=1 >Delivery_date</TD>"
                htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold; text-align:center' colSpan=1 >Payment</TD>"
                htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold; text-align:center' colSpan=1 >Billing</TD>"
                htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold; text-align:center' colSpan=1 >Created_by</TD>"
                htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold; text-align:center' colSpan=1 >Status</TD></TR>"
                htmlHead = String.Format(htmlHead, clsManage.convert2QuantityGram(htSum("Quan96G")), clsManage.convert2Quantity(htSum("Quan96")), clsManage.convert2Currency(htSum("Quan99")), clsManage.convert2Currency(htSum("Amount")))

                Dim htmlBody As String = ""

                Dim rowBody As String = ""
                rowBody += "<TR style='BACKGROUND-COLOR: #eeeeee; COLOR: black'>"
                rowBody += "<TD valign='top' colSpan=2 align=middle>{0}</TD>"
                rowBody += "<TD valign='top' align=middle>{1}</TD>"
                rowBody += "<TD valign='top' colSpan=2 align=left>{2}</TD>"
                rowBody += "<TD valign='top' align=middle>{3}</TD>"
                rowBody += "<TD valign='top' align=middle>{4}</TD>"
                rowBody += "<TD valign='top' align=right>{5}</TD>"
                rowBody += "<TD valign='top' align=right>{6}</TD>"
                rowBody += "<TD valign='top' align=right>{7}</TD>"
                rowBody += "<TD valign='top' align=right>{8}</TD>"
                rowBody += "<TD valign='top' align=right>{9}</TD>"
                rowBody += "<TD valign='top' align=middle>{10}</TD>"
                rowBody += "<TD valign='top' align=middle>{11}</TD>"
                rowBody += "<TD valign='top' align=middle>{12}</TD>"
                rowBody += "<TD valign='top' align=middle>{13}</TD>"
                rowBody += "<TD valign='top' align=middle>{14}</TD></TR>"

                For Each gvr As GridViewRow In gv.Rows
                    htmlBody += String.Format(rowBody, CType(gvr.FindControl("linkLog"), HyperLink).Text, _
                                            CType(gvr.FindControl("link"), HyperLink).Text, _
                                            CType(gvr.FindControl("linkCust"), HyperLink).Text, _
                                            gvr.Cells(4).Text, _
                                            gvr.Cells(5).Text, _
                                            gvr.Cells(6).Text, _
                                            gvr.Cells(7).Text, _
                                            gvr.Cells(8).Text, _
                                            gvr.Cells(9).Text, _
                                            gvr.Cells(10).Text, _
                                            gvr.Cells(11).Text, _
                                            gvr.Cells(12).Text, _
                                            gvr.Cells(13).Text, _
                                            gvr.Cells(14).Text, _
                                            gvr.Cells(15).Text)
                Next
                Dim htmlFoot As String = "</table>"
                ViewState("htmlExcel") = htmlHead + htmlBody + htmlFoot
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
End Class
