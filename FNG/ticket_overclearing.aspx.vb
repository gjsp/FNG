
Partial Class ticket_overclearing
    Inherits basePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            gv.EmptyDataText = clsManage.EmptyDataText

            For i = 1 To 20 : ddlClearing.Items.Add(New ListItem(i.ToString, i.ToString)) : Next
            ddlClearing.Items.Insert(0, New ListItem("--All--", "0"))
            gv.DataBind()
        End If
    End Sub
    Protected Sub upgSearch_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles upgSearch.Load
        System.Threading.Thread.Sleep(upgSearch.DisplayAfter)
    End Sub

    Protected Sub gvCust_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gv.RowDataBound
        Try
            CType(e.Row.Cells(2).FindControl("link"), HyperLink).NavigateUrl = "ticket_deal.aspx?page=stock&id=" + e.Row.DataItem("ref_no").ToString
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub linkFolio_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim index As Integer = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow).RowIndex
        Dim cust_id As String = gv.DataKeys(index).Value
        clsManage.Script(Page, "window.open('customer_portfolio.aspx?id=" & cust_id + "')")
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As System.EventArgs) Handles btnSearch.Click
        gv.DataBind()
    End Sub
End Class
