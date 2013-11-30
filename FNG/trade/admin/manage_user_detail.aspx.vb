
Partial Class admin_manage_user_detail
    Inherits basePageTrade

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.QueryString("id") IsNot Nothing Then
                Dim id As String = Request.QueryString("id")
                Dim dt As New Data.DataTable
                dt = clsMain.getUsernamesValue(id)
                If dt.Rows.Count > 0 Then
                    ddlLv.SelectedValue = dt.Rows(0)("cust_level").ToString
                    cbHalt.Checked = IIf(dt.Rows(0)("halt").ToString = "n", False, True)
                End If
            End If
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

End Class
