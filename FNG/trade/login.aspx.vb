
Partial Class login
    Inherits System.Web.UI.Page
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        'If Not Request.ServerVariables("HTTP_USER_AGENT").Contains("MSIE") Then Response.Redirect("logout3.aspx")
        If Request.QueryString("h") = "y" Then
            Session.Clear()
            clsManage.alert(Page, "ระบบถูกปิดชั่วคราว")
            Exit Sub
        End If

        If Not Page.IsPostBack Then
            hasSession()

            If Session(clsManage.iSession.cust_id.ToString) IsNot Nothing Then
                Response.Redirect("trading.aspx")
            End If

            If Session(clsManage.iSession.user_id.ToString) IsNot Nothing Then
                Response.Redirect("admin/trade_trans.aspx")
            End If
            ViewState("lock") = 0
            username.Focus()

        End If
    End Sub

    Sub hasSession()
        If Session(clsManage.iSession.user_id.ToString) IsNot Nothing Then
            If Session(clsManage.iSession.role.ToString) = "cust" Then
                Response.Redirect("trading.aspx")
            Else
                Response.Redirect("admin/trade_trans.aspx")
            End If
            Exit Sub
        End If
    End Sub

    Protected Sub linkForget_Click(sender As Object, e As System.EventArgs) Handles linkForget.Click
        Response.Redirect("cust_forget.aspx")
    End Sub

    Protected Sub linkLogin_Click(sender As Object, e As System.EventArgs) Handles linkLogin.Click
        Try
            'hasSession()
            If username.Text.Trim = "" Then clsManage.alert(Page, "Please Enter Username.", username.ClientID) : Exit Sub
            If password.Text.Trim = "" Then clsManage.alert(Page, "Please Enter Password.", password.ClientID) : Exit Sub

            Dim ds As New Data.DataSet
            Dim dtUsername As New Data.DataTable
            Dim dtPwd As New Data.DataTable

            ds = clsMain.getUsernameLogin(username.Text, password.Text)
            dtUsername = ds.Tables(0)
            dtPwd = ds.Tables(1)


            If dtUsername.Rows.Count > 0 Then
                If dtUsername.Rows(0)("lock_time") IsNot DBNull.Value Then
                    If dtUsername.Rows(0)("lock") = "y" Then
                        Dim lockTime As DateTime = dtUsername.Rows(0)("lock_time")
                        Dim timeDiff As TimeSpan = DateTime.Now - lockTime
                        Dim minuteDiff As Integer = CInt(ConfigurationManager.AppSettings("UNLOCK_TIME")) - CInt(timeDiff.Minutes)
                        clsManage.alert(Page, "This Account is Lock.Please Wait " + minuteDiff.ToString + " minutes to New Login")
                        Exit Sub
                    Else
                        If dtUsername.Rows(0)("lock_time") IsNot DBNull.Value Then
                            clsManage.alert(Page, "This Account is Lock Please enter the information for receive information via email.", , "cust_forget.aspx")
                        End If
                    End If
                End If
            End If

            If dtPwd.Rows.Count > 0 Then
                'check system halt only customer
                If dtPwd.Rows(0)("role").ToString = "cust" And dtPwd.Rows(0)("system_halt").ToString = "y" Then
                    clsManage.alert(Page, "ระบบถูกปิดชั่วคราว")
                    Exit Sub
                End If
                'check session ถ้ามีให้เตือนว่า มีlogin ซ้อน
                If Session(clsManage.iSession.user_id.ToString) IsNot Nothing Then
                    If Session(clsManage.iSession.role.ToString) = "cust" Then
                        If Session(clsManage.iSession.user_id.ToString).ToString = dtPwd.Rows(0)("user_id").ToString Then
                            'Same User Login
                            Response.Redirect("trading.aspx")
                            Exit Sub
                        Else
                            'Diff User Login
                            'clsManage.alert(Page, "เครื่องคอมพิวเตอร์ของคุณได้มีการใช้งานระบบอยู่แล้ว โปรดออกจากระบบก่อนเข้าระบบอีกครั้ง", , "logout.aspx?s=n", "Dup")
                            Response.Redirect("logout2.aspx")
                            Exit Sub
                        End If
                    Else
                        Response.Redirect("admin/trade_trans.aspx")
                    End If

                Else
                    Session(clsManage.iSession.user_id.ToString) = dtPwd.Rows(0)("user_id").ToString
                    Session(clsManage.iSession.user_name.ToString) = dtPwd.Rows(0)("username").ToString
                    Session(clsManage.iSession.cust_lv.ToString) = dtPwd.Rows(0)("cust_level").ToString
                    Session(clsManage.iSession.role.ToString) = dtPwd.Rows(0)("role").ToString
                    Session(clsManage.iSession.first_trade.ToString) = dtPwd.Rows(0)("first_trade").ToString
                    Session(clsManage.iSession.online_id.ToString) = Session.SessionID


                    If dtPwd.Rows(0)("role").ToString = "admin" Then
                        Response.Redirect("admin/trade_trans.aspx")
                    ElseIf dtPwd.Rows(0)("role").ToString = "cust" Then
                        'update status online
                        clsMain.updateLoginStatus(dtPwd.Rows(0)("cust_id").ToString, "y", Session.SessionID)

                        Session(clsManage.iSession.cust_id.ToString) = dtPwd.Rows(0)("cust_id")
                        Response.Redirect("trading.aspx")
                    End If
                End If
            Else
                If dtUsername.Rows.Count > 0 Then
                    ViewState("lock") += 1
                    'login 3 miss then lock and wait 30 min 
                    If CInt(ViewState("lock")) = 3 Then
                        clsMain.updateUsernameLockTime(username.Text)
                        clsManage.alert(Page, "This Account is Lock.Please Wait 30 minutes to New Login", password.ClientID)
                        Exit Sub
                    Else
                        clsManage.alert(Page, "Password Invalid Please Try Again.", password.ClientID)
                        Exit Sub
                    End If
                End If
                clsManage.alert(Page, "Username Or Password Invalid Please Try Again.", password.ClientID)
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "error")
        End Try
    End Sub
End Class
