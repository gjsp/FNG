
Partial Class trade_session_time_check
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            tm.Interval = 9000
        End If
    End Sub

    Protected Sub tm_Tick(sender As Object, e As System.EventArgs) Handles tm.Tick
        If Session(clsManage.iSession.cust_id.ToString) Is Nothing Then
            clsManage.Script(Page, "top.window.location.href='login.aspx'", "loginDup1")
        End If
    End Sub
End Class
