
Partial Class _Default
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            'Dim ws As New gcap.gtc()
            'lbl.Text = ws.getStockOnline()
        End If
    End Sub
End Class
