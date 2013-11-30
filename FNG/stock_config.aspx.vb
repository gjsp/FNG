
Partial Class stock_config
    Inherits basePage

    Protected Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        If Not Page.IsPostBack Then
            Try
                If Session(clsManage.iSession.user_role_center.ToString) IsNot Nothing Then
                    Dim dtRole As New Data.DataTable : dtRole = Session(clsManage.iSession.user_role_center.ToString)
                    If dtRole.Rows.Find(Me.Page.Page.ToString) IsNot Nothing Then
                        Dim dr As Data.DataRow = dtRole.Rows.Find(Me.Page.Page.ToString)
                        'Update
                        If dr(clsDB.roles.update) = False Then
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
            txtBid96.Attributes.Add("onkeypress", "checkNumber();")
            txtbid99.Attributes.Add("onkeypress", "checkNumber();")
            txtAsk96.Attributes.Add("onkeypress", "checkNumber();")
            txtAsk99.Attributes.Add("onkeypress", "checkNumber();")

            txtMargin.Attributes.Add("onkeypress", "checkNumber();")
            Dim dt As New Data.DataTable
            dt = clsDB.getStockNow
            If dt.Rows.Count > 0 Then
                txtBid96.Text = dt.Rows(0)("bid96").ToString
                txtAsk96.Text = dt.Rows(0)("ask96").ToString
                txtBid99.Text = dt.Rows(0)("bid99").ToString
                txtAsk99.Text = dt.Rows(0)("ask99").ToString
                txtPwdAuth.Text = dt.Rows(0)("pwd_auth").ToString
                txtMargin.Text = dt.Rows(0)("Margin").ToString
            End If
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If txtBid96.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill)
            If txtAsk96.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill)
            If txtBid99.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill)
            If txtAsk99.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill)
            If txtMargin.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill)
            If Not IsNumeric(txtBid96.Text) Then clsManage.alert(Page, "Not Number")
            If Not IsNumeric(txtAsk96.Text) Then clsManage.alert(Page, "Not Number")
            If Not IsNumeric(txtBid99.Text) Then clsManage.alert(Page, "Not Number")
            If Not IsNumeric(txtAsk99.Text) Then clsManage.alert(Page, "Not Number")
            If Not IsNumeric(txtMargin.Text) Then clsManage.alert(Page, "Not Number")

            Dim result As Integer = clsDB.updatePriceNow(txtBid96.Text, txtAsk96.Text, txtBid99.Text, txtAsk99.Text, txtPwdAuth.Text, txtMargin.Text)

            If result Then
                clsManage.alert(Page, "Savecomplete")
            Else
                clsManage.alert(Page, "Cannot Save. Please Try again.")
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try

    End Sub
End Class
