
Partial Class change_password
    Inherits basePageTrade
    <System.Web.Services.WebMethod()> _
    Public Shared Function getDupUsername(ByVal username As String) As String
        If clsMain.getDupUsername(username) Then
            Return "y"
        Else
            Return "n"
        End If

    End Function

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(clsManage.iSession.cust_id.ToString) IsNot Nothing Then
            'update online time
            Dim msg = clsMain.updateTimeOnline(Session(clsManage.iSession.cust_id.ToString).ToString, Session.SessionID)
            If msg <> "" Then
                Session.Clear()
                clsManage.alert(Page, msg, , "login.aspx", "loginDup")
                Exit Sub
            End If

        End If
        If Not Page.IsPostBack Then
            txtUsername.Attributes.Add("onblur", "getDupUsername();")
            If Request.QueryString("t") IsNot Nothing Then
                If Request.QueryString("t").ToString = "pwd" Then
                    pnPwd.Visible = True
                ElseIf Request.QueryString("t").ToString = "username" Then
                    pnUsername.Visible = True
                End If
            End If
        End If
    End Sub

    Protected Sub btnSaveUser_Click(sender As Object, e As System.EventArgs) Handles btnSaveUser.Click
        Try
            If txtOldUsername.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill, txtOldUsername.ClientID) : Exit Sub
            If txtUsername.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill, txtUsername.ClientID) : Exit Sub
            If txtUsernameCon.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill, txtUsernameCon.ClientID) : Exit Sub

            Dim dt As New Data.DataTable
            dt = clsMain.getUsernamesValue(Session(clsManage.iSession.user_id.ToString).ToString)
            If dt.Rows.Count > 0 Then
                If dt.Rows(0)("username").ToString <> txtOldUsername.Text.Trim Then clsManage.alert(Page, "Old Username not Match.", txtOldUsername.ClientID, , "notMatch") : Exit Sub
                If Not clsMain.getDupUsername(txtUsername.Text) Then clsManage.alert(Page, "Username Already have in System.", txtUsername.ClientID, , "dupp") : Exit Sub

                Dim result As Integer = clsMain.updateUsernameChangUsername(Session(clsManage.iSession.cust_id.ToString).ToString, txtUsername.Text)
                If result > 0 Then
                    clsManage.alert(Page, "Change Username Complete.", , "cust_profile.aspx")
                End If
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "err")
        End Try
    End Sub

    Protected Sub btnSavePwd_Click(sender As Object, e As System.EventArgs) Handles btnSavePwd.Click
        Try
            If txtOldPwd.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill, txtOldPwd.ClientID) : Exit Sub
            If txtNewPwd.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill, txtOldPwd.ClientID) : Exit Sub
            If txtNewPwdCon.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill, txtOldPwd.ClientID) : Exit Sub
            Dim dt As New Data.DataTable

            dt = clsMain.getUsernamesValue(Session(clsManage.iSession.user_id.ToString).ToString)
            If dt.Rows.Count > 0 Then
                If dt.Rows(0)("password").ToString <> txtOldPwd.Text.Trim Then clsManage.alert(Page, "Old Password not Match.", txtOldPwd.ClientID, , "notMatch") : Exit Sub

                Dim result As Integer = clsMain.updateUsernameChangPassword(Session(clsManage.iSession.cust_id.ToString).ToString, txtNewPwd.Text)
                If result > 0 Then
                    clsManage.alert(Page, "Change Password Complete.", , "cust_profile.aspx")
                End If
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "err")
        End Try
    End Sub

    Protected Sub btnCancalUser_Click(sender As Object, e As System.EventArgs) Handles btnCancalUser.Click
        Response.Redirect("cust_profile.aspx")
    End Sub


    Protected Sub btnCancelPwd_Click(sender As Object, e As System.EventArgs) Handles btnCancelPwd.Click
        Response.Redirect("cust_profile.aspx")
    End Sub
End Class
