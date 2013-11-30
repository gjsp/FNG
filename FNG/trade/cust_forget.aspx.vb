
Partial Class trade_cust_forget
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
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
        End If
    End Sub

    Protected Sub btnSend_Click(sender As Object, e As System.EventArgs) Handles btnSend.Click
        Try
            Dim idCard As String = txtIDCardz1.Text + txtIDCardz2.Text + txtIDCardz3.Text + txtIDCardz4.Text + txtIDCardz5.Text + txtIDCardz6.Text + txtIDCardz7.Text + txtIDCardz8.Text + txtIDCardz9.Text + txtIDCardz10.Text + txtIDCardz11.Text + txtIDCardz12.Text + txtIDCardz13.Text

            If txtUsername.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill, txtUsername.ClientID) : Exit Sub
            If idCard = "" Then clsManage.alert(Page, clsManage.msgRequiredFill) : Exit Sub
            If txtMobile.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill, txtMobile.ClientID) : Exit Sub
            Dim dt As New Data.DataTable
            Dim birthday As Date = DateTime.ParseExact(ddlday.SelectedValue + "/" + ddlMonth.SelectedValue + "/" + ddlYear.SelectedValue, clsManage.formatDateTime, Nothing)

            dt = clsMain.getUsernameForget(txtUsername.Text, idCard, birthday, txtMobile.Text)
            If dt.Rows.Count > 0 Then
                Dim pwd As String = clsManage.genPwd(8)

                Dim msgBody As String = String.Format("New Password : {1}<br/> " & _
                "<a href='{0}/trade/login.aspx'>Click to Login</a> <br/><br/> เพื่อความปลอดภัยของท่าน กรุณาเปลี่ยน password ด้วยค่ะ", ConfigurationManager.AppSettings("DOMAIN_NAME").ToString, pwd)
                Dim result As Integer = clsMain.updateUsernamesPasswordByUsername(txtUsername.Text, pwd)
                clsMain.sendEmailForget(dt.Rows(0)("email").ToString, " ", msgBody)
                clsManage.alert(Page, "Send E-Mail Complete.", , "login.aspx")
            Else
                clsManage.alert(Page, "Please enter the correct information.")
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "err")
        End Try
    End Sub

End Class
