
Partial Class users
    Inherits basePage

    Dim columnDel As Integer = 8

    Protected Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        If Not Page.IsPostBack Then
            Try
                If Session(clsManage.iSession.user_role_center.ToString) IsNot Nothing Then
                    Dim dtRole As New Data.DataTable : dtRole = Session(clsManage.iSession.user_role_center.ToString)
                    If dtRole.Rows.Find(Me.Page.Page.ToString) IsNot Nothing Then
                        Dim dr As Data.DataRow = dtRole.Rows.Find(Me.Page.Page.ToString)
                        If dr(clsDB.roles.delete) = False Then
                            ViewState(clsDB.roles.delete) = False
                        End If

                    End If
                End If
            Catch ex As Exception
                clsManage.alert(Page, ex.Message)
            End Try
        End If
    End Sub

    Protected Sub gv_user_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvUser.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.Cells(columnDel).Controls(1), ImageButton).OnClientClick = "return confirm(' " & clsManage.msgDel & " ');"
            e.Row.Cells(1).Text = String.Format("<div style='text-align:center'><a href='users_roles.aspx?id={0}' target='_blank'>Role</a></div>", e.Row.DataItem("user_id").ToString)

            If ViewState(clsDB.roles.delete) IsNot Nothing AndAlso ViewState(clsDB.roles.delete) = False Then
                CType(e.Row.FindControl("imgDel"), ImageButton).Enabled = False
                CType(e.Row.FindControl("imgDel"), ImageButton).ImageUrl = "~/images/i_del2.png"
            End If
        End If
    End Sub


    Protected Sub gv_user_SelectedIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSelectEventArgs) Handles gvUser.SelectedIndexChanging
        Dim id As String = gvUser.DataKeys(e.NewSelectedIndex).Value
        If id = Session(clsManage.iSession.user_id_center.ToString) Then
            Response.Redirect("user_profile.aspx")
        Else
            Response.Redirect("user_detail.aspx?m=edit&id=" & id)
        End If
    End Sub


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            gvUser.EmptyDataText = clsManage.EmptyDataText
        End If
    End Sub

    Protected Sub imgDel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Try
            Dim i As Integer = CType(CType(sender, ImageButton).Parent.Parent, GridViewRow).RowIndex
            Dim da As New dsTableAdapters.usersTableAdapter()
            Dim result As Integer = da.DeleteQuery(gvUser.DataKeys(i).Value)
            If result > 0 Then
                gvUser.DataBind()
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub
End Class
