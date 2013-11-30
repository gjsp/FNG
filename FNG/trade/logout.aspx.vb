
Partial Class trade_logout
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load


        If Session(clsManage.iSession.cust_id.ToString) IsNot Nothing Then
            pnMain.Visible = True
            clsMain.updateLoginStatus(Session(clsManage.iSession.cust_id.ToString).ToString, "n", Session.SessionID)

            Session.Remove(clsManage.iSession.cust_id.ToString)
            Session.Remove(clsManage.iSession.user_id.ToString)
            Session.Remove(clsManage.iSession.user_name.ToString)
            Session.Remove(clsManage.iSession.cust_lv.ToString)
            Session.Remove(clsManage.iSession.role.ToString)
            Session.Remove(clsManage.iSession.first_trade.ToString)
            Session.Remove(clsManage.iSession.online_id.ToString)

        Else
            pnMain.Visible = False
        End If
    End Sub
End Class
