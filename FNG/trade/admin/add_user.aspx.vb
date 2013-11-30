
Partial Class admin_add_user
    Inherits basePageTrade


    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            hdfCust_id.Value = ""
        End If
    End Sub

    Protected Sub Cancelbt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancel.Click
        Response.Redirect("manage_users.aspx")
    End Sub

    Protected Sub Addbt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAdd.Click
        Try

            If hdfCust_id.Value = "" Then clsManage.alert(Page, "Please Select Customer Name.") : Exit Sub
            Dim dt As Data.DataTable
            Dim email As String = ""
            'check has email
            dt = clsMain.getCustomerByCust_id(hdfCust_id.Value)
            If dt.Rows.Count > 0 Then
                If dt.Rows(0)("email").ToString() <> "" And dt.Rows(0)("id_card").ToString <> "" Then
                    email = dt.Rows(0)("email").ToString
                Else
                    clsManage.alert(Page, "Can not Add New User.Not E-Mail Or ID Card for this Client.") : Exit Sub
                End If
            End If

            dt = Nothing
            dt = clsMain.getChkCust_id(hdfCust_id.Value)
            If dt.Rows.Count > 0 Then clsManage.alert(Page, "Customer name already in system.") : Exit Sub

            Dim username As String = genUsername()
            Dim pwd As String = clsManage.genPwd(8)
            Dim regCode As String = Session.SessionID

            Dim result As Integer = clsMain.InsertUsernames(username, pwd, hdfCust_id.Value, Session(clsManage.iSession.user_id.ToString).ToString, "cust", DropDownLevel.SelectedValue, regCode, "n")
            If result > 0 Then
                'send email to customer
                Dim msgBody As String = String.Format("Please Click Link to Confirm Register.<br/>User Name : {0} <br/>Password : {3}<br/>" & _
                 "<a href='{2}/trade/register_confirm.aspx?u={0}&r={1}'>{2}/trade/register_confirm.aspx?u={0}&r={1}</a>", username, regCode, ConfigurationManager.AppSettings("DOMAIN_NAME").ToString, pwd)

                If clsMain.sendEmailRegisterTrade(email, txtCustName.Text, msgBody) Then
                    clsManage.alert(Page, "Save and Send E-Mail Complete", , "manage_users.aspx")
                Else
                    clsManage.alert(Page, "Save Complete", , "manage_users.aspx")
                End If
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub


    Function genUsername() As String
        Dim username As String = clsManage.genPwd(8)
        Dim dt As New Data.DataTable
        dt = clsMain.getChkUsersAdd(username)

        If dt.Rows.Count > 0 Then
            Return genUsername()
        Else
            Return username
        End If
    End Function

End Class
