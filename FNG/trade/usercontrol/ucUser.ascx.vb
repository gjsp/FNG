
Partial Class user_control_ucUser
    Inherits System.Web.UI.UserControl

    Public Property UserID() As String
        Get
            Return IIf(ViewState("_UserID").ToString() <> "", ViewState("_UserID").ToString(), "")
        End Get
        Set(ByVal value As String)
            ViewState("_UserID") = value
        End Set
    End Property

    Private _UserName As String
    Public Property UserName() As String
        Get
            Return ViewState("_UserName").ToString
        End Get
        Set(ByVal value As String)
            ViewState("_UserName") = value
        End Set
    End Property

    Private _Password As String
    Public Property Password() As String
        Get
            Return ViewState("_Password").ToString
        End Get
        Set(ByVal value As String)
            ViewState("_Password") = value
        End Set
    End Property

    Private _FirstName As String
    Public Property FirstName() As String
        Get
            Return _FirstName
        End Get
        Set(ByVal value As String)
            _FirstName = value
        End Set
    End Property

    Private _LastName As String
    Public Property LastName() As String
        Get
            Return _LastName
        End Get
        Set(ByVal value As String)
            _LastName = value
        End Set
    End Property

    Public Sub LoadUser()
        Try
            If UserID = "" Then
                mvMain.SetActiveView(vNone)
                Exit Sub
            End If

            Dim dc As New dcDBDataContext()
            Dim us = (From usr In dc.users Where usr.user_id = UserID Select usr).FirstOrDefault

            If us IsNot Nothing Then
                lblUserId.Text = us.user_id.ToString
                lblUsername.Text = us.user_name.ToString

                lblFname.Text = us.firstname.ToString
                lblLname.Text = us.lastname.ToString

                ViewState("_UserID") = us.user_id.ToString
                ViewState("_UserName") = us.firstname.ToString
                ViewState("_Password") = us.password.ToString
                mvMain.SetActiveView(vMain)
            Else
                ViewState("_UserID") = ""
                ViewState("_UserName") = ""
                ViewState("_Password") = ""
                mvMain.SetActiveView(vNone)
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

End Class
