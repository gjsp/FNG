
Partial Class trade_cust_first_trade
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(clsManage.iSession.cust_id.ToString) Is Nothing Then
            Response.Redirect("login.aspx")
        End If

        If Not Page.IsPostBack Then
            For i As Integer = 1 To 31 : ddlday.Items.Add(i) : Next
            For i As Integer = 1 To 12 : ddlMonth.Items.Add(i) : Next
            For i As Integer = Now.Year To 1900 Step -1 : ddlYear.Items.Add(i) : Next

            'For i As Integer = 1 To 16
            '    CType(Master.FindControl("MainContent").FindControl("txtFnameEngz" + i.ToString), TextBox).Attributes.Add("onkeyup", "nextFocusName(this," + i.ToString + ",16);")
            '    CType(Master.FindControl("MainContent").FindControl("txtLnameEngz" + i.ToString), TextBox).Attributes.Add("onkeyup", "nextFocusName(this," + i.ToString + ",16);")
            'Next
            For i As Integer = 1 To 13
                CType(Master.FindControl("MainContent").FindControl("txtIDCardz" + i.ToString), TextBox).Attributes.Add("onkeypress", "numberOnly();")
                CType(Master.FindControl("MainContent").FindControl("txtIDCardz" + i.ToString), TextBox).Attributes.Add("onkeyup", "nextFocusId(this," + i.ToString + ",13);")
            Next
            txtUsername.Attributes.Add("onblur", "getDupUsername();")
        End If
    End Sub

    Protected Sub btnOk_Click(sender As Object, e As System.EventArgs) Handles btnOk.Click
        Try
            Dim idCard As String = txtIDCardz1.Text + txtIDCardz2.Text + txtIDCardz3.Text + txtIDCardz4.Text + txtIDCardz5.Text + txtIDCardz6.Text + txtIDCardz7.Text + txtIDCardz8.Text + txtIDCardz9.Text + txtIDCardz10.Text + txtIDCardz11.Text + txtIDCardz12.Text + txtIDCardz13.Text
            Dim birthday As Date = DateTime.ParseExact(ddlday.SelectedValue + "/" + ddlMonth.SelectedValue + "/" + ddlYear.SelectedValue, clsManage.formatDateTime, Nothing)

            If idCard = "" Then clsManage.alert(Page, clsManage.msgRequiredFill) : Exit Sub
            If txtMobile.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill, txtMobile.ClientID) : Exit Sub
            If txtMobile.Text.Trim.Length <> Integer.Parse(clsManage.phoneDigit) Then clsManage.alert(Page, String.Format("Mobile Phone Must be Only Number {0} digit", clsManage.phoneDigit.ToString), txtMobile.ClientID) : Exit Sub

            If txtOldUsername.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill, txtOldUsername.ClientID) : Exit Sub
            If txtUsername.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill, txtUsername.ClientID) : Exit Sub
            If txtUsernameCon.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill, txtUsernameCon.ClientID) : Exit Sub

            If txtOldPwd.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill, txtOldPwd.ClientID) : Exit Sub
            If txtPwd.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill, txtPwd.ClientID) : Exit Sub
            If txtPwdCon.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill, txtPwdCon.ClientID) : Exit Sub

            Dim username As String = Session(clsManage.iSession.user_name.ToString).ToString
            If username <> txtOldUsername.Text.Trim Then clsManage.alert(Page, "Old Username not Match.", txtOldUsername.ClientID, , "notMatch") : Exit Sub

            Dim dt As New Data.DataTable
            dt = clsMain.getUsernameForget(username, idCard, birthday, txtMobile.Text)
            If dt.Rows.Count > 0 Then
                If dt.Rows(0)("password").ToString <> txtOldPwd.Text.Trim Then clsManage.alert(Page, "Old Password not Match.", txtOldPwd.ClientID, , "notMatch") : Exit Sub

                'username เหมือนเดิมได้
                If txtOldUsername.Text.Trim <> txtUsername.Text.Trim Then
                    If Not clsMain.getDupUsername(txtUsername.Text) Then clsManage.alert(Page, "Username Already have in System.", txtUsername.ClientID, , "dupp") : Exit Sub
                End If

                Dim result As Integer = clsMain.updateUsernameChangUserAndPwd(Session(clsManage.iSession.cust_id.ToString).ToString, txtUsername.Text, txtPwd.Text)
                Dim result2 As Boolean = clsMain.updateUsernameFirstTrade(Session(clsManage.iSession.user_id.ToString).ToString)

                Session(clsManage.iSession.user_name.ToString) = txtUsername.Text
                Session(clsManage.iSession.first_trade.ToString) = "n"
                clsManage.alert(Page, "Change Username and Password Complete.", , "trading.aspx")
            Else
                clsManage.alert(Page, "Please enter the correct information.") : Exit Sub
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "err")
        End Try

    End Sub

    <System.Web.Services.WebMethod()> _
    Public Shared Function getDupUsername(ByVal username As String) As String
        If clsMain.getDupUsername(username) Then
            Return "y"
        Else
            Return "n"
        End If

    End Function
End Class
