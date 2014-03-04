
Partial Class ticket_payment
    Inherits basePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            gv.DataBind()
        End If
    End Sub


    Protected Sub gv_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gv.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("linkDetail"), HyperLink).NavigateUrl = "ticket_payment_detail.aspx?pid=" + e.Row.DataItem("payment_id").ToString
        End If
    End Sub
End Class
