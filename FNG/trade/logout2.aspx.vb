
Partial Class trade_logout2
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load

        If Session(clsManage.iSession.cust_id.ToString) IsNot Nothing Then
            pnMain.Visible = True
        Else
            pnMain.Visible = False
        End If

    End Sub
End Class
