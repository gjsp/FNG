
Partial Class admin_add_sales
    Inherits basePageTrade

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            txtUserId.Attributes.Add("onkeypress", "numberOnly();")
            txtUserId.Focus()
            Addbt.Enabled = False
        End If
    End Sub

    Protected Sub Cancelbt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cancelbt.Click
        Response.Redirect("manage_sales.aspx")
    End Sub

    Protected Sub Addbt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Addbt.Click
        Try
            If ucUser.UserID = "" Then
                clsManage.alert(Page, "Please Select Users.") : Exit Sub
            End If

            Dim dc As New dcDBDataContext()
            Dim u = (From usr In dc.usernames Where usr.cust_id = ucUser.UserID Select usr).FirstOrDefault
            If u IsNot Nothing Then
                clsManage.alert(Page, "This user has in the system.")
            Else
                'Add
                Dim usn As New username
                usn.cust_id = ucUser.UserID
                usn.username = ucUser.UserName
                usn.password = ucUser.Password
                usn.role = "sale"
                usn.active = "y"
                usn.first_trade = "n"
                usn.halt = "n"
                usn.modifier_by = Session(clsManage.iSession.user_id.ToString)
                usn.created_by = Session(clsManage.iSession.user_id.ToString)
                usn.modifier_date = DateTime.Now
                usn.created_date = DateTime.Now

                dc.usernames.InsertOnSubmit(usn)
                dc.SubmitChanges()
                clsManage.alert(Page, "Create Sale Complete", "", "manage_sales.aspx")
            End If

        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Try
            If txtUserId.Text.Trim = "" Then
                clsManage.alert(Page, "Please Enter User ID", txtUserId.ClientID)
                Exit Sub
            End If
            If Not IsNumeric(txtUserId.Text) Then
                clsManage.alert(Page, "Please Enter Only Number")
                Exit Sub
            End If

            hdfUserId.Value = txtUserId.Text
            ucUser.UserID = hdfUserId.Value
            ucUser.LoadUser()

            If ucUser.UserID = "" Then
                Addbt.Enabled = False
            Else
                Addbt.Enabled = True
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try

    End Sub

End Class
