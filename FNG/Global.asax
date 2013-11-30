<%@ Application Language="VB" %>

<script runat="server">

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        Application("OnlineUser") = 0
    End Sub
    
    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs on application shutdown
    End Sub
        
    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Code that runs when an unhandled error occurs
    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        Application.Lock()
        Application("OnlineUser") = CInt(Application("OnlineUser")) + 1
        Application.UnLock()

    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        Application.Lock()
        Application("OnlineUser") = CInt(Application("OnlineUser")) - 1
        
        'If CType(Application.Item("UserLogin"), Hashtable).ContainsKey(Session(clsManage.iSession.user_id.ToString).ToString) Then
        '    CType(Application.Item("UserLogin"), Hashtable).Remove(Session(clsManage.iSession.user_id.ToString).ToString)
        'End If

        Application.UnLock()
    End Sub
       
</script>