
Partial Class admin_reject_timer
    Inherits System.Web.UI.Page

    'Protected Sub tmTime_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tmTime.Tick
    '    clsMain.rejectTimeout()
    'End Sub

    <System.Web.Services.WebMethod()> _
    Public Shared Function rejectTimeout() As String
        Return clsMain.rejectTimeout().ToString()
    End Function
End Class
