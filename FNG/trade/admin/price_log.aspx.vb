
Partial Class admin_price_log
    Inherits basePageTrade

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            For i As Integer = 0 To 23 : ddl1Hour.Items.Add(i) : ddl2Hour.Items.Add(i) : Next
            For i As Integer = 0 To 59 : ddl1Min.Items.Add(i.ToString("00")) : ddl2Min.Items.Add(i.ToString("00")) : Next
            ddl1Hour.SelectedValue = "9" : ddl1Min.SelectedValue = "30"
            ddl2Hour.SelectedValue = "23" : ddl2Min.SelectedValue = "59"
            txt1Date_CalendarExtender.Format = clsManage.formatDateTime : txt2Date_CalendarExtender.Format = clsManage.formatDateTime
            txt1Date.Text = Now.ToString(clsManage.formatDateTime) : txt2Date.Text = Now.ToString(clsManage.formatDateTime)

            gvPrice.EmptyDataText = clsManage.EmptyDataText
            btnSearch_Click(Nothing, Nothing)
        End If
    End Sub

    Protected Sub UpdateProg1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles UpdateProg1.Load
        System.Threading.Thread.Sleep(UpdateProg1.DisplayAfter + 300)
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Dim pDate1 As New DateTime
        Dim pDate2 As New DateTime
        Dim isToday As String = "y"

        If rdoTime2.Checked Then
            Dim tempDate1 As New DateTime
            Dim tempDate2 As New DateTime
            tempDate1 = DateTime.ParseExact(txt1Date.Text, clsManage.formatDateTime, Nothing)
            tempDate2 = DateTime.ParseExact(txt2Date.Text, clsManage.formatDateTime, Nothing)
            pDate1 = pDate1.AddDays(tempDate1.Day - 1).AddMonths(tempDate1.Month - 1).AddYears(tempDate1.Year - 1).AddHours(ddl1Hour.SelectedValue).AddMinutes(ddl1Min.SelectedValue)
            pDate2 = pDate2.AddDays(tempDate2.Day - 1).AddMonths(tempDate2.Month - 1).AddYears(tempDate2.Year - 1).AddHours(ddl2Hour.SelectedValue).AddMinutes(ddl2Min.SelectedValue).AddSeconds(59)
            isToday = "n"
        End If

        ObjectDataSource1.SelectParameters("isToday").DefaultValue = isToday
        ObjectDataSource1.SelectParameters("pDate1").DefaultValue = pDate1
        ObjectDataSource1.SelectParameters("pDate2").DefaultValue = pDate2

        gvPrice.DataBind()
    End Sub
    'For Excel
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    End Sub

    Protected Sub linkExcel_Click(sender As Object, e As System.EventArgs) Handles linkExcel.Click
        Try
            clsManage.ExportToExcelTradeOrder(gvPrice, "Price_Log_" + DateTime.Now.ToString("ddmmyyyy"))
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "err")
        End Try
    End Sub

End Class
