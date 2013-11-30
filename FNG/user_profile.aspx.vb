
Partial Class user_profile
    Inherits basePage

    Protected Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        If Not Page.IsPostBack Then
            Try
                If Session(clsManage.iSession.user_role_center.ToString) IsNot Nothing Then
                    Dim dtRole As New Data.DataTable : dtRole = Session(clsManage.iSession.user_role_center.ToString)
                    If dtRole.Rows.Find(Me.Page.Page.ToString) IsNot Nothing Then
                        Dim dr As Data.DataRow = dtRole.Rows.Find(Me.Page.Page.ToString)
                        If dr(clsDB.roles.add) = False Then
                            btnSave.Enabled = False
                        End If
                    End If
                End If
            Catch ex As Exception
                clsManage.alert(Page, ex.Message)
            End Try
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            clsManage.getDropDownlistValue(ddlTeam, clsDB.getTeam, "-- All --")
            ddlTeam.Items.Insert(IIf(ddlTeam.Items.Count > 0, 1, 0), New ListItem(clsManage.msgRequireSelect, "none"))

            hdfUser_id.Value = Session(clsManage.iSession.user_id_center.ToString).ToString

            If hdfUser_id.Value <> "" Then
                Dim da As New dsTableAdapters.usersTableAdapter
                Dim dt As New ds.usersDataTable
                da.FillByUser_id(dt, hdfUser_id.Value)
                If dt.Rows.Count > 0 Then
                    lblRef.Text = dt.Rows(0)(dt.user_idColumn).ToString
                    txtUsername.Text = dt.Rows(0)(dt.user_nameColumn).ToString
                    txtPassword.Text = dt.Rows(0)("password").ToString
                    txtFname.Text = dt.Rows(0)(dt.firstnameColumn).ToString
                    txtLname.Text = dt.Rows(0)(dt.lastnameColumn).ToString
                    txtPosition.Text = dt.Rows(0)(dt.positionColumn).ToString
                    ddlTeam.SelectedValue = dt.Rows(0)(dt.team_idColumn).ToString
                End If
            End If

        End If
        txtPassword.Attributes("value") = txtPassword.Text

    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If Not validatetion() Then Exit Sub
            Dim dc As New dcDBDataContext()
            If hdfUser_id.Value = "" Then
                Dim u As New user
                u.user_name = txtUsername.Text
                u.firstname = txtFname.Text
                u.lastname = txtLname.Text
                u.status = "active"
                u.position = txtPosition.Text
                u.password = txtPassword.Text
                If ddlTeam.SelectedValue <> "none" Then
                    u.team_id = ddlTeam.SelectedValue
                End If
                dc.users.InsertOnSubmit(u)
                dc.SubmitChanges()
                clsManage.alert(Page, "Save complete.", , "users.aspx")
            Else
                Dim u = (From usr In dc.users Where usr.user_id = hdfUser_id.Value Select usr).FirstOrDefault
                If u IsNot Nothing Then
                    u.user_name = txtUsername.Text
                    u.firstname = txtFname.Text
                    u.lastname = txtLname.Text
                    u.status = "active"
                    u.position = txtPosition.Text
                    u.password = txtPassword.Text
                    If ddlTeam.SelectedValue <> "none" Then
                        u.team_id = ddlTeam.SelectedValue
                    End If
                    dc.SubmitChanges()
                    clsManage.alert(Page, "Update complete.")
                End If
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try

    End Sub

    Function validatetion() As Boolean
        If txtUsername.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill, txtUsername.ClientID) : Return False
        If txtPassword.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill, txtPassword.ClientID) : Return False
        If txtFname.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill, txtFname.ClientID) : Return False

        Dim dc As New dcDBDataContext()
        Dim u = (From usr In dc.users Where usr.user_name = txtUsername.Text.Trim Select usr).FirstOrDefault
        If u IsNot Nothing And hdfUser_id.Value = "" Then
            clsManage.alert(Page, "Username already have in system.", txtUsername.ClientID) : Return False
        End If

        Return True
    End Function
End Class
