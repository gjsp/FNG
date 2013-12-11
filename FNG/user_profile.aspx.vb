
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

            Dim strPos As String = ConfigurationManager.AppSettings("User_Position") ' "-None-,0|Account,1|Trader,2|Marketing,3"
            For Each pos As String In strPos.Split("|")
                ddlPos.Items.Add(New ListItem(pos.Split(",")(0), pos.Split(",")(1)))
            Next

            hdfUser_id.Value = Session(clsManage.iSession.user_id_center.ToString).ToString
            If hdfUser_id.Value <> "" Then
                Dim dc As New dcDBDataContext()
                Dim us = (From usr In dc.users Where usr.user_id = hdfUser_id.Value Select usr).FirstOrDefault

                If us IsNot Nothing Then
                    lblRef.Text = us.user_id.ToString
                    txtUsername.Text = us.user_name.ToString
                    txtPassword.Text = us.password.ToString
                    txtFname.Text = us.firstname.ToString
                    txtLname.Text = us.lastname.ToString
                    ddlPos.SelectedValue = IIf(us.position_id Is Nothing, 0, us.position_id)

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
                u.password = txtPassword.Text
                If Not ddlPos.SelectedIndex = 0 Then
                    u.position_id = ddlPos.SelectedValue
                    u.position = ddlPos.SelectedItem.Text
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
                    u.password = txtPassword.Text
                    If Not ddlPos.SelectedIndex = 0 Then
                        u.position_id = ddlPos.SelectedValue
                        u.position = ddlPos.SelectedItem.Text
                    Else
                        u.position_id = Nothing
                        u.position = ""
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
