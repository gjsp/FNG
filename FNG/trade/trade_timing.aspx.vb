
Partial Class trade_trade_timing
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            lblCount.Text = String.Empty
            btnOk_Click(Nothing, Nothing)
        End If
    End Sub

    Protected Sub tmTime_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmTime.Tick

        Dim timeEnd As DateTime = ViewState(clsManage.iSession.timetrade.ToString)
        Dim timeDiff As TimeSpan = timeEnd - DateTime.Now

        If timeDiff.Seconds <= 0 Then
            Me.Session(clsManage.iSession.timetrade.ToString) = False
            tmTime.Enabled = False
            lblCount.Text = (0).ToString
        Else
            lblCount.Text = timeDiff.Seconds.ToString()
        End If

    End Sub

    Protected Sub btnOk_Click(sender As Object, e As System.EventArgs) Handles btnOk.Click
        tmTime.Enabled = True

        ViewState(clsManage.iSession.timetrade.ToString) = DateTime.Now.AddSeconds(ConfigurationManager.AppSettings("TRADE_COUNT") + 1)
        Me.Session(clsManage.iSession.timetrade.ToString) = True
        clsManage.Script(Page, "top.window.document.getElementById('div_ifmTime').style.display='block'")
    End Sub

End Class
