﻿
Partial Class trade_admin_timer
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim dt As New Data.DataTable
            dt = clsMain.getPriceOnline()
            If dt.Rows.Count > 0 Then
                hdfTime.Value = CDate(dt.Rows(0)("time_end")).ToString("dd/MM/yyyy hh:mm:ss tt")
                ViewState("first") = True
            End If
        End If

    End Sub

    Protected Sub tmTime_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmTime.Tick
        'Dim dbTime As DateTime = clsMain.getDatetimeServer()
        Dim timeEnd As DateTime = DateTime.ParseExact(hdfTime.Value, "dd/MM/yyyy hh:mm:ss tt", Nothing)
        Dim timeDiff As TimeSpan = timeEnd - DateTime.Now

        If (ViewState("first")) Then
            If timeDiff.Milliseconds > 1 Then
                clsManage.Script(Page, "setPriceColor('#3399FF')")
            Else
                clsManage.Script(Page, "setPriceColor('#FF3737')")
            End If
            ViewState("first") = False
        End If

        If timeDiff.Milliseconds > 1 Then
            If timeDiff.Hours > 0 Then
                lblTimer.Text = timeDiff.Hours.ToString().PadLeft(2, "0") + "." + timeDiff.Minutes.ToString().PadLeft(2, "0") + "." + timeDiff.Seconds.ToString().PadLeft(2, "0")
            Else
                lblTimer.Text = timeDiff.Minutes.ToString().PadLeft(2, "0") + "." + timeDiff.Seconds.ToString().PadLeft(2, "0")
            End If
            clsManage.Script(Page, "setPriceColor('#3399FF')", "settime")
        Else
            lblTimer.Text = "Time Out!!!."
            clsManage.Script(Page, "setPriceColor('#FF3737')", "settime")
        End If

    End Sub

End Class
