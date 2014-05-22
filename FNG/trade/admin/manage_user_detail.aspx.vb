
Partial Class admin_manage_user_detail
    Inherits basePageTrade
    Dim cssLock As String = "buttonPro small red"
    Dim cssUnlock As String = "buttonPro small blue"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.QueryString("id") IsNot Nothing Then
                Dim id As String = Request.QueryString("id")
                Dim dt As New Data.DataTable
                dt = clsMain.getUsernamesValue(id)
                If dt.Rows.Count > 0 Then
                    ddlLv.SelectedValue = dt.Rows(0)("cust_level").ToString
                    cbHalt.Checked = IIf(dt.Rows(0)("halt").ToString = "n", False, True)
                    If dt.Rows(0)("lock") = "y" Then
                        switchLock(True)
                        btnLock.Attributes.Add("onclick", "return confirm('Do you want to Unlock this user');")
                    Else
                        switchLock(False)
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub switchLock(lock As Boolean)
        If lock Then
            btnLock.Text = "Lock"
            btnLock.CssClass = cssLock
        Else
            btnLock.Text = "UnLock"
            btnLock.CssClass = cssUnlock
            btnLock.Enabled = False
        End If
    End Sub

    Function saveData() As Boolean
        Try
            Dim id As String = Request.QueryString("id")
            Dim result As Integer = clsMain.UpdateUserLevel(id, ddlLv.SelectedValue, IIf(cbHalt.Checked, "y", "n"))
            If result > 0 Then
                Return True
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
        Return False
    End Function

    Protected Sub Savebt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Savebt.Click
        Try
            If saveData() Then
                clsManage.alert(Page, "Update Complete", , "manage_users.aspx")
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub


    Protected Sub Cancelbt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cancelbt.Click

        Response.Redirect("manage_users.aspx")

    End Sub

    Protected Sub btnLock_Click(sender As Object, e As System.EventArgs) Handles btnLock.Click
        Dim dc As New dcDBDataContext
        Dim time_unlock As String = ConfigurationManager.AppSettings("UNLOCK_TIME").ToString

        Dim usr = (From u In dc.usernames Where u.user_id = Request.QueryString("id").ToString).FirstOrDefault
        usr.lock_time = DateTime.Now.AddHours(-Integer.Parse(time_unlock))
        usr.modifier_by = Session(clsManage.iSession.user_name.ToString).ToString
        usr.modifier_date = DateTime.Now
        dc.SubmitChanges()

        switchLock(False)
    End Sub
End Class
