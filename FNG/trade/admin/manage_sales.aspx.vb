
Partial Class admin_manage_sales
    Inherits basePageTrade
    Protected Sub gvUser_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvUser.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("DelButton"), ImageButton).Attributes.Add("onclick", "return confirm('Do you want to delete.');")
        End If
    End Sub
    Protected Sub EditButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim i As Integer = CType(CType(sender, ImageButton).Parent.Parent, GridViewRow).RowIndex
        Dim id As String = gvUser.DataKeys(i).Value
        Response.Redirect("change_password.aspx?id=" + id + "&type=sale")
    End Sub
    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Response.Redirect("add_sales.aspx")
    End Sub

    Protected Sub DelButton_Click1(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim i As Integer = CType(CType(sender, ImageButton).Parent.Parent, GridViewRow).RowIndex
        Dim id As String = gvUser.DataKeys(i).Value
        clsMain.DelUsername(id)
        gvUser.DataBind()
    End Sub
End Class
