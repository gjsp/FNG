
Partial Class users_roles
    Inherits basePage

    Protected Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        If Not Page.IsPostBack Then
            Try
                If Session(clsManage.iSession.user_role_center.ToString) IsNot Nothing Then
                    Dim dtRole As New Data.DataTable : dtRole = Session(clsManage.iSession.user_role_center.ToString)
                    If dtRole.Rows.Find(Me.Page.Page.ToString) IsNot Nothing Then
                        Dim dr As Data.DataRow = dtRole.Rows.Find(Me.Page.Page.ToString)
                        If dr(clsDB.roles.update) = False Then
                            btnSave.Enabled = False
                            btnCheck.Enabled = False
                        End If

                    End If
                End If
            Catch ex As Exception
                clsManage.alert(Page, ex.Message)
            End Try
        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.QueryString("id") IsNot Nothing Then

                hdfUserId.Value = Request.QueryString("id").ToString
                gvRole.EmptyDataText = clsManage.EmptyDataText
                gvRole.DataBind()

            End If
        End If
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        Dim dc As New dcDBDataContext
        Try
            dc.Connection.Open()
            dc.Transaction = dc.Connection.BeginTransaction
            Dim rol = From roles In dc.users_roles Where roles.user_id = hdfUserId.Value Select roles
            For Each roles In rol
                dc.users_roles.DeleteOnSubmit(roles)
                dc.SubmitChanges()
            Next
            For Each gvRow As GridViewRow In gvRole.Rows
                Dim role As New users_role
                role.role_view = CType(gvRow.FindControl("cbView"), CheckBox).Checked
                role.role_add = CType(gvRow.FindControl("cbAdd"), CheckBox).Checked
                role.role_update = CType(gvRow.FindControl("cbUpdate"), CheckBox).Checked
                role.role_delete = CType(gvRow.FindControl("cbDelete"), CheckBox).Checked
                role.role_export = CType(gvRow.FindControl("cbExport"), CheckBox).Checked
                role.role_print = CType(gvRow.FindControl("cbPrint"), CheckBox).Checked
                role.created_date = DateTime.Now
                role.created_by = Session(clsManage.iSession.user_id_center.ToString)
                role.user_id = hdfUserId.Value
                role.menu_id = gvRole.DataKeys(gvRow.RowIndex).Value
                dc.users_roles.InsertOnSubmit(role)
                dc.SubmitChanges()
            Next
            dc.Transaction.Commit()
            clsManage.alert(Page, "Save Complete.", , "users.aspx")
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        Finally
            dc.Connection.Close()
            dc.Dispose()
        End Try
    End Sub

    Protected Sub gvRole_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvRole.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Text = e.Row.DataItem("menu_name").ToString.Split(".")(1)
        End If
    End Sub

End Class
