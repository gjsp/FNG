
Partial Class admin_edit_price
    Inherits basePageTrade

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            BidTxtbox.Attributes.Add("onkeypress", "checkNumber();")
            AskTxtbox.Attributes.Add("onkeypress", "checkNumber();")
            BidTxtbox2.Attributes.Add("onkeypress", "checkNumber();")
            AskTxtbox2.Attributes.Add("onkeypress", "checkNumber();")
            BidTxtbox3.Attributes.Add("onkeypress", "checkNumber();")
            AskTxtbox3.Attributes.Add("onkeypress", "checkNumber();")
            TimeTxtBox.Attributes.Add("onkeypress", "checkNumber();")
            BidDiffTxtbox2.Attributes.Add("onkeypress", "checkNumber();")
            BidDiffTxtbox3.Attributes.Add("onkeypress", "checkNumber();")
            AskDiffTxtbox2.Attributes.Add("onkeypress", "checkNumber();")
            AskDiffTxtbox3.Attributes.Add("onkeypress", "checkNumber();")

            BidDiffTxtbox2.Attributes.Add("onblur", "addBid(2,this)")
            BidDiffTxtbox3.Attributes.Add("onblur", "addBid(3,this)")
            AskDiffTxtbox2.Attributes.Add("onblur", "addAsk(2,this)")
            AskDiffTxtbox3.Attributes.Add("onblur", "addAsk(3,this)")

            editbt.Attributes.Add("onclick", "return confirm('Do you want to Save?');")

            getData()
        End If
    End Sub

    Sub getData()
        Dim dt As New Data.DataTable
        dt = clsMain.getPriceOnline()
        Dim dr As Data.DataRow = dt.Rows(0)
        LabelBid.Text = dr("bid").ToString
        LabelAsk.Text = dr("ask").ToString
        LabelBid2.Text = dr("bid2").ToString
        LabelAsk2.Text = dr("ask2").ToString
        LabelBid3.Text = dr("bid3").ToString
        LabelAsk3.Text = dr("ask3").ToString

        BidTxtbox.Text = dr("bid").ToString
        AskTxtbox.Text = dr("ask").ToString
        BidTxtbox2.Text = dr("bid2").ToString
        AskTxtbox2.Text = dr("ask2").ToString
        BidTxtbox3.Text = dr("bid3").ToString
        AskTxtbox3.Text = dr("ask3").ToString
        TimeTxtBox.Text = dr("time").ToString
        hdfTime.Value = CDate(dr("time_end")).ToString("dd/MM/yyyy hh:mm:ss tt")

        Dim bidDiff2 As Double = clsManage.convert2zero(dr("bid2").ToString) * 100 - (clsManage.convert2zero(dr("bid").ToString) * 100)
        Dim bidDiff3 As Double = clsManage.convert2zero(dr("bid3").ToString) * 100 - (clsManage.convert2zero(dr("bid").ToString) * 100)
        Dim askDiff2 As Double = clsManage.convert2zero(dr("ask2").ToString) * 100 - (clsManage.convert2zero(dr("ask").ToString) * 100)
        Dim askDiff3 As Double = clsManage.convert2zero(dr("ask3").ToString) * 100 - (clsManage.convert2zero(dr("ask").ToString) * 100)
        BidDiffTxtbox2.Text = (bidDiff2 / 100).ToString("#,###.00")
        BidDiffTxtbox3.Text = (bidDiff3 / 100).ToString("#,###.00")
        AskDiffTxtbox2.Text = (askDiff2 / 100).ToString("#,###.00")
        AskDiffTxtbox3.Text = (askDiff3 / 100).ToString("#,###.00")


    End Sub
    Function saveData() As Boolean
        Try
            If BidTxtbox.Text = "" Or AskTxtbox.Text = "" Or BidTxtbox2.Text = "" Or AskTxtbox2.Text = "" Or BidTxtbox.Text = "" Or AskTxtbox.Text = "" Or BidTxtbox3.Text = "" Or AskTxtbox3.Text = "" Or TimeTxtBox.Text = "" Then
                clsManage.alert(Page, "! Data Incorrect.")
            Else
                'clsMain.UpdatePriceOnline(BidTxtbox.Text, AskTxtbox.Text, BidTxtbox2.Text, AskTxtbox2.Text, BidTxtbox3.Text, AskTxtbox3.Text, TimeTxtBox.Text)
                Return True
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Protected Sub editbt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles editbt.Click
        Try
            If saveData() Then
                getData()
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Protected Sub tmTime_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmTime.Tick
        Dim timeEnd As DateTime = DateTime.ParseExact(hdfTime.Value, "dd/MM/yyyy hh:mm:ss tt", Nothing)
        Dim timeDiff As TimeSpan = timeEnd - DateTime.Now
        If timeDiff.Milliseconds > 1 Then
            If timeDiff.Hours > 0 Then
                lblTimer.Text = timeDiff.Hours.ToString().PadLeft(2, "0") + "." + timeDiff.Minutes.ToString().PadLeft(2, "0") + "." + timeDiff.Seconds.ToString().PadLeft(2, "0")
            Else
                lblTimer.Text = timeDiff.Minutes.ToString().PadLeft(2, "0") + "." + timeDiff.Seconds.ToString().PadLeft(2, "0")
            End If

        Else
            lblTimer.Text = "Time Out!!!."
        End If

    End Sub
End Class
