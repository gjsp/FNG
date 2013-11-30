
Partial Class log_in
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.QueryString("mode") IsNot Nothing Then
                If Request.QueryString("mode").ToString = "logout" Then
                    clsDB.log_users(Session(clsManage.iSession.user_id_center.ToString).ToString, "logout")
                    Session.Clear()
                End If
            End If
            txt_username.Focus()
        End If
    End Sub
    Protected Sub btn_submit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn_submit.Click
        Try
            Dim dt As Data.DataTable
            dt = clsDB.getUserLogin(txt_username.Text.Trim, txt_password.Text.Trim)
            If dt.Rows.Count > 0 Then
                clsDB.log_users(dt.Rows(0)("user_id").ToString, "login")
                Session(clsManage.iSession.user_id_center.ToString) = dt.Rows(0)("user_id")
                Session(clsManage.iSession.user_name_center.ToString) = dt.Rows(0)("user_name")

                Dim dtRole As Data.DataTable
                dtRole = clsDB.getMenuRole(dt.Rows(0)("user_id").ToString)
                Dim primaryKey(0) As Data.DataColumn
                primaryKey(0) = dtRole.Columns(clsDB.roles.menu)
                dtRole.PrimaryKey = primaryKey
                Session(clsManage.iSession.user_role_center.ToString) = dtRole

                Response.Redirect("customers.aspx")
            Else
                clsManage.alert(Page, "Username Or Password Invalid Please Try Again.")
            End If

        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub
End Class
