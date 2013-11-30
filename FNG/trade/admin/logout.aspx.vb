
Partial Class trade_admin_logout
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Session(clsManage.iSession.user_id.ToString) IsNot Nothing Then
            Session.Abandon()
        End If
    End Sub
End Class
