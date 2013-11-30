
Partial Class ticket_view_stock
    Inherits basePage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            gvTicket.EmptyDataText = clsManage.EmptyDataText
            txtDate1.Attributes.Add("onkeypress", "return false;")
            txtDate2.Attributes.Add("onkeypress", "return false;")
            txtDelDate1.Attributes.Add("onkeypress", "return false;")
            txtDelDate2.Attributes.Add("onkeypress", "return false;")
            txtCustRef.Attributes.Add("onkeypress", "if(event.keyCode==13){$get('" & btnSearchAdv.ClientID & "').focus();}")
            txtTicketRef.Attributes.Add("onkeypress", "if(event.keyCode==13){$get('" & btnSearchAdv.ClientID & "').focus();}")

            ViewState("sumQuan") = 0
            ViewState("sumPrice") = 0
            ViewState("sumAmount") = 0

            rdoBilling.Attributes.Add("onclick", "$get('" & btnSearchAdv.ClientID & "').click();")
            rdoType.Attributes.Add("onclick", "$get('" & btnSearchAdv.ClientID & "').click();")


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
            objSrcTicket.SelectParameters("ticket_id").DefaultValue = txtTicketRef.Text
            objSrcTicket.SelectParameters("cust_id").DefaultValue = txtCustRef.Text
            objSrcTicket.SelectParameters("type").DefaultValue = rdoType.SelectedValue
            objSrcTicket.SelectParameters("billing").DefaultValue = rdoBilling.SelectedValue

            objSrcTicket.SelectParameters("ticket_date1").DefaultValue = txtDate1.Text
            objSrcTicket.SelectParameters("ticket_date2").DefaultValue = txtDate2.Text


            objSrcTicket.SelectParameters("del_date1").DefaultValue = txtDelDate1.Text
            objSrcTicket.SelectParameters("del_date2").DefaultValue = txtDelDate2.Text
            gvTicket.DataBind()
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try

    End Sub

    Function validatetion() As Boolean
        If txtCustRef.Text.Trim = "" Then Return False

        Return True
    End Function

    Protected Sub gvTicket_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTicket.RowCreated

        If e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(6).Text = Double.Parse(ViewState("sumQuan")).ToString("#,##0.000")
            e.Row.Cells(7).Text = Double.Parse(ViewState("sumPrice")).ToString("#,##0.000")
            e.Row.Cells(8).Text = Double.Parse(ViewState("sumAmount")).ToString("#,##0.000")

            'set the columnspan
            e.Row.Cells(0).ColumnSpan = 6
            'remove the second cell
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells(0).Text = "Summary"
            e.Row.Cells(0).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(1).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Right
        End If
    End Sub

    Protected Sub gvTicket_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTicket.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            ViewState("sumQuan") = 0
            ViewState("sumPrice") = 0
            ViewState("sumAmount") = 0
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then

            'Summary
            ViewState("sumQuan") += Double.Parse(e.Row.DataItem("quantity"))
            ViewState("sumPrice") += Double.Parse(e.Row.DataItem("price"))
            ViewState("sumAmount") += Double.Parse(e.Row.DataItem("amount"))
        End If
    End Sub


    'Protected Sub linkExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkExport.Click
    '    Dim dt As New Data.DataTable
    '    dt = clsDB.getTicketViewStock(txtTicketRef.Text, txtCustRef.Text, rdoType.SelectedValue, rdoBilling.SelectedValue, txtDate1.Text, txtDate2.Text, txtDelDate1.Text, txtDelDate2.Text)
    '    If dt.Rows.Count > 0 Then
    '        clsManage.ExportToExcel(dt, "ticket_stock_") ' + Now.ToString("dd/MM/yy"))
    '    End If

    'End Sub
End Class
