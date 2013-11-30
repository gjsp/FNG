
Partial Class ticket_view
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
            ddl_goldtype.Attributes.Add("onchange", "$get('" & btnSearchAdv.ClientID & "').click();")
            ddl_status.Attributes.Add("onchange", "$get('" & btnSearchAdv.ClientID & "').click();")

            ViewState("sumQuan") = 0
            ViewState("sumPrice") = 0
            ViewState("sumAmount") = 0

            clsManage.getDropDownlistValue(ddl_goldtype, clsDB.getGoldType, "-- All --")
            clsManage.getDropDownlistValue(ddl_status, clsDB.getTicket_status, "-- All --")

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
            objSrcTicket.SelectParameters("status_id").DefaultValue = ddl_status.SelectedValue
            objSrcTicket.SelectParameters("gold_type_id").DefaultValue = ddl_goldtype.SelectedValue

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
            e.Row.Cells(8).Text = Double.Parse(ViewState("sumQuan")).ToString("#,##0.000")
            e.Row.Cells(9).Text = Double.Parse(ViewState("sumPrice")).ToString("#,##0.000")
            e.Row.Cells(10).Text = Double.Parse(ViewState("sumAmount")).ToString("#,##0.000")

            'set the columnspan
            e.Row.Cells(0).ColumnSpan = 7
            'remove the second cell
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells.Remove(e.Row.Cells(1))
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

            If e.Row.DataItem("status_id") = "999" Then
                e.Row.Cells(gvTicket.Columns.Count - 1).Attributes.Add("style", "color:green")
            End If

            'Summary
            ViewState("sumQuan") += Double.Parse(e.Row.DataItem("quantity"))
            ViewState("sumPrice") += Double.Parse(e.Row.DataItem("price"))
            ViewState("sumAmount") += Double.Parse(e.Row.DataItem("amount"))
        End If
    End Sub

    Protected Sub gvTicket_SelectedIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSelectEventArgs) Handles gvTicket.SelectedIndexChanging
        Dim id As String = gvTicket.DataKeys(e.NewSelectedIndex).Value
        Dim page_name As String = "view"
        clsManage.Script(Page, "top.window.location.href='ticket_deal.aspx?page=" & page_name & "&id=" & id & " '")
    End Sub

   
End Class
