
Partial Class ticket_list
    Inherits basePage



    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            gvTicket.EmptyDataText = clsManage.EmptyDataText
            txtDate1.Attributes.Add("onkeypress", "return false;")
            txtDate2.Attributes.Add("onkeypress", "return false;")
            txtDelDate1.Attributes.Add("onkeypress", "return false;")
            txtDelDate2.Attributes.Add("onkeypress", "return false;")
            txtCustname.Attributes.Add("onkeypress", "if(event.keyCode==13){$get('" & btnSearchAdv.ClientID & "').focus();}")
            txtTicketRef.Attributes.Add("onkeypress", "if(event.keyCode==13){$get('" & btnSearchAdv.ClientID & "').focus();}")
            ddl_goldtype.Attributes.Add("onchange", "$get('" & btnSearchAdv.ClientID & "').click();")
            ddl_status.Attributes.Add("onchange", "$get('" & btnSearchAdv.ClientID & "').click();")
            txtAmount.Attributes.Add("onkeypress", "checkNumber();")


            clsManage.getDropDownlistValue(ddl_goldtype, clsDB.getGoldType, "-- All --")
            clsManage.getDropDownlistValue(ddl_status, clsDB.getTicket_status, "-- All --")


            txtDate1_CalendarExtender.Format = clsManage.formatDateTime
            txtDate2_CalendarExtender.Format = clsManage.formatDateTime
            txtDelDate1_CalendarExtender.Format = clsManage.formatDateTime
            txtDelDate2_CalendarExtender.Format = clsManage.formatDateTime
            txtDate1.Text = Now.ToString(clsManage.formatDateTime)
            txtDate2.Text = Now.ToString(clsManage.formatDateTime)

            btnSearchAdv_Click(Nothing, Nothing)
        End If
    End Sub

    Protected Sub btnSearchAdv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchAdv.Click
        Try
            If (txtDate1.Text <> "" And txtDate2.Text = "") OrElse (txtDate1.Text = "" And txtDate2.Text <> "") Then clsManage.alert(Page, "Please Select Date") : Exit Sub
            If (txtDelDate1.Text <> "" And txtDelDate2.Text = "") OrElse (txtDelDate1.Text = "" And txtDelDate2.Text <> "") Then clsManage.alert(Page, "Please Select Date") : Exit Sub
            If txtAmount.Text.Trim <> "" And Not IsNumeric(txtAmount.Text) Then clsManage.alert(Page, "Please Number Only.") : Exit Sub
            objSrcTicket.SelectParameters("ticket_id").DefaultValue = txtTicketRef.Text
            objSrcTicket.SelectParameters("book_no").DefaultValue = txtBookNo.Text
            objSrcTicket.SelectParameters("run_no").DefaultValue = txtNo.Text
            objSrcTicket.SelectParameters("cust_name").DefaultValue = txtCustname.Text
            objSrcTicket.SelectParameters("type").DefaultValue = ""
            objSrcTicket.SelectParameters("billing").DefaultValue = ""
            objSrcTicket.SelectParameters("status_id").DefaultValue = ddl_status.SelectedValue
            objSrcTicket.SelectParameters("gold_type_id").DefaultValue = ddl_goldtype.SelectedValue

            objSrcTicket.SelectParameters("ticket_date1").DefaultValue = txtDate1.Text
            objSrcTicket.SelectParameters("ticket_date2").DefaultValue = txtDate2.Text


            objSrcTicket.SelectParameters("del_date1").DefaultValue = txtDelDate1.Text
            objSrcTicket.SelectParameters("del_date2").DefaultValue = txtDelDate2.Text
            objSrcTicket.SelectParameters("created_by").DefaultValue = Session(clsManage.iSession.user_id_center.ToString)
            objSrcTicket.SelectParameters("amount").DefaultValue = txtAmount.Text

            gvTicket.DataBind()
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try

    End Sub

    Protected Sub gvTicket_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTicket.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.DataItem("status_id") = "999" Then
                e.Row.Cells(gvTicket.Columns.Count - 1).Attributes.Add("style", "color:green")
            End If
            If e.Row.DataItem("billing").ToString = "y" Then
                CType(e.Row.Cells(gvTicket.Columns.Count - 2).FindControl("cbBill"), CheckBox).Checked = True
            Else
                CType(e.Row.Cells(gvTicket.Columns.Count - 2).FindControl("cbBill"), CheckBox).Checked = False
            End If
        End If
    End Sub

    Protected Sub gvTicket_SelectedIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSelectEventArgs) Handles gvTicket.SelectedIndexChanging
        Dim id As String = gvTicket.DataKeys(e.NewSelectedIndex).Value
        Dim page_name As String = "list" 'gvTicket.Rows(e.NewSelectedIndex).Cells(gvTicket.Columns.Count - 1).Text
        clsManage.Script(Page, "top.window.location.href='ticket_deal.aspx?page=" & page_name & "&id=" & id & " '")
    End Sub

    Public Overloads Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    End Sub

    Private Sub ExportToExcel(ByVal strFileName As String, ByVal dg As GridView)
        Response.Clear()
        Response.Buffer = True
        Response.ContentType = "application/vnd.ms-excel"
        Response.Charset = ""
        Me.EnableViewState = False
        Dim oStringWriter As New System.IO.StringWriter
        Dim oHtmlTextWriter As New System.Web.UI.HtmlTextWriter(oStringWriter)

        dg.RenderControl(oHtmlTextWriter)

        Response.Write(oStringWriter.ToString())
        Response.End()
      





    End Sub


    Protected Sub linkExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkExport.Click
        'Dim dt As New Data.DataTable
        'dt = clsDB.getTicket("T201110/004")
        'If dt.Rows.Count > 0 Then
        '    clsManage.ExportToExcel(dt, "SupplierDirectoryList_" + Now.ToString("MMMM-d,yyyy"))
        'End If
        ExportToExcel("Report.xls", gvTicket)

    End Sub
End Class
