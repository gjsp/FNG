
Partial Class admin_add_admin
    Inherits basePageTrade
    Function saveData() As Boolean
        Try
            If PassTxtbox.Text = "" Or UserTxtbox.Text = "" Then
                clsManage.alert(Page, "! Data Incorrect")
            Else
                Dim data As Data.DataTable
                data = clsMain.getChkUsersAdd(UserTxtbox.Text)
                If data.Rows.Count > 0 Then
                    clsManage.alert(Page, "Usernames is not available")
                Else
                    clsMain.InsertUsernames(UserTxtbox.Text, PassTxtbox.Text, "admin", Session(clsManage.iSession.user_id.ToString).ToString, "", "", "", "n")
                    Return True
                End If

            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try

    End Function

    Protected Sub Cancelbt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cancelbt.Click
        Response.Redirect("manage_admin.aspx")
    End Sub

    Protected Sub Addbt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Addbt.Click
        Try
            If saveData() Then
                clsManage.alert(Page, "Save Complete", , "manage_admin.aspx")
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub
End Class
