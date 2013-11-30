
Partial Class admin_trade_trans
    Inherits basePageTrade
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            TabContainer1.ActiveTabIndex = 0
            hdfConfirm.Value = "n"
            'txtPrice1.Attributes.Add("onkeypress", "checkNumber();")
            'txtPrice2.Attributes.Add("onkeypress", "checkNumber();")
            'txtPrice1.Attributes.Add("style", "text-align:right")
            'txtPrice2.Attributes.Add("style", "text-align:right")
            'cbConfirm.Attributes.Add("onclick", "setConfirm(this);")
            'cbConfirmSave.Attributes.Add("onclick", "setConfirmSave(this);")
        End If
    End Sub

    'Protected Sub btnAccept_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAccept.Click
    '    If txtPrice1.Text = "" Or txtPrice2.Text = "" Then clsManage.alert(Page, clsManage.msgRequiredFill) : Exit Sub
    '    If Not IsNumeric(txtPrice1.Text) Or Not IsNumeric(txtPrice2.Text) Then clsManage.alert(Page, "Please Enter only Numeric.") : Exit Sub
    '    Dim result As Integer = clsMain.UpdateTicketPortfolioByPeriodPrice(txtPrice1.Text.Trim, txtPrice2.Text.Trim, Me.Session(clsManage.iSession.user_id.ToString).ToString)
    '    If result > 0 Then
    '    End If
    'End Sub

    'Protected Sub UpdateProg1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles UpdateProg1.Load
    '    System.Threading.Thread.Sleep(UpdateProg1.DisplayAfter)
    'End Sub

    'Protected Sub btnRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
    '    Try
    '        'System.Threading.Thread.Sleep(UpdateProg1.DisplayAfter)
    '        Dim buy96 As Double = 0
    '        Dim sell96 As Double = 0
    '        Dim buy99 As Double = 0
    '        Dim sell99 As Double = 0

    '        Dim dt As New Data.DataTable
    '        dt = clsMain.getTradeSummaryAccept()
    '        If dt.Rows.Count > 0 Then
    '            If dt.Rows(0)("buy96") IsNot DBNull.Value Then buy96 = dt.Rows(0)("buy96").ToString
    '            If dt.Rows(0)("sell96") IsNot DBNull.Value Then sell96 = dt.Rows(0)("sell96").ToString
    '            If dt.Rows(0)("buy99") IsNot DBNull.Value Then buy99 = dt.Rows(0)("buy99").ToString
    '            If dt.Rows(0)("sell99") IsNot DBNull.Value Then sell99 = dt.Rows(0)("sell99").ToString
    '        End If

    '        lblSumBuy96.Text = clsManage.convert2Quantity(buy96)
    '        lblSumSell96.Text = clsManage.convert2Quantity(sell96)
    '        lblSumNet96.Text = clsManage.convert2Quantity(clsManage.convert2zero(sell96) - clsManage.convert2zero(buy96))

    '        lblSumBuy99.Text = clsManage.convert2Quantity(buy99)
    '        lblSumSell99.Text = clsManage.convert2Quantity(sell99)
    '        lblSumNet99.Text = clsManage.convert2Quantity(clsManage.convert2zero(sell99) - clsManage.convert2zero(buy99))
    '    Catch ex As Exception
    '        clsManage.alert(Page, ex.Message)
    '    End Try

    'End Sub

    'Protected Sub Page_LoadComplete(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.LoadComplete
    '    If Not Page.IsPostBack Then
    '        clsManage.Script(Page, "$get('ifmTime').src='timer.aspx'")
    '    End If
    'End Sub
End Class
