
Partial Class admin_change_password
    Inherits basePageTrade

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.QueryString("id") IsNot Nothing Then
                Dim id As String = Request.QueryString("id")
                Dim data As Data.DataTable
                data = clsMain.getUsernamesValue(id)
                OldpassTxt.Text = data.Rows(0)("password").ToString
            End If
        End If
    End Sub
    Function saveData() As Boolean
        Try
            If txtNewPwd.Text.Trim = "" Then
                clsManage.alert(Page, "Please Enter New Password.", txtNewPwd.ClientID) : Return False
            End If

            If txtConNewPwd.Text.Trim = "" Then
                clsManage.alert(Page, "Please Enter Confirm New Password.", txtConNewPwd.ClientID) : Return False
            End If

            If txtConNewPwd.Text.Trim <> txtNewPwd.Text.Trim Then
                clsManage.alert(Page, "Password do not match.", txtNewPwd.ClientID) : Return False
            End If

            If txtNewPwd.Text.Trim = OldpassTxt.Text.Trim Then
                clsManage.alert(Page, "New password Must do not Match Old Password.", txtNewPwd.ClientID) : Return False
            End If

            Dim id As String = Request.QueryString("id").ToString
            clsMain.UpdateUsernames(id, txtNewPwd.Text)
            Return True

        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try

    End Function

    Protected Sub Savebt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Savebt.Click
        Try
            If saveData() Then
                clsManage.alert(Page, "Update Complete", , "manage_admin.aspx")
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub


    Protected Sub Cancelbt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cancelbt.Click

        Response.Redirect("manage_admin.aspx")

    End Sub
End Class
