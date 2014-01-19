
Partial Class tradeSaleMasterPage
    Inherits System.Web.UI.MasterPage


    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Session(clsManage.iSession.user_name.ToString) IsNot Nothing Then
                lblOwner.Text = "( " + Session(clsManage.iSession.user_name.ToString).ToString + " )"
            End If
        End If
    End Sub
End Class

