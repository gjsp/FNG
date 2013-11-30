Imports Microsoft.VisualBasic

Public Class basePage
    Inherits System.Web.UI.Page

    'Public Overloads Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    'End Sub

    Private Sub Page_LoadComplete(sender As Object, e As System.EventArgs) Handles Me.LoadComplete
        If Not Page.IsPostBack Then
            'Permission Role
            If Session(clsManage.iSession.user_role_center.ToString) IsNot Nothing Then
                Dim dtRole As New Data.DataTable : dtRole = Session(clsManage.iSession.user_role_center.ToString)
                Dim iPage As String = Me.Page.Page.ToString
                If dtRole.Rows.Find(iPage) IsNot Nothing Then
                    Dim dr As Data.DataRow = dtRole.Rows.Find(iPage)
                    If dr(clsDB.roles.view) = False Then
                        Response.Redirect(clsDB.urlHome)
                    End If
                Else
                    If Not iPage.Contains("rpt") Then 'page report ไม่ต้อง check
                        If Not iPage.Contains("ticket_online") Then
                            Response.Redirect(clsDB.urlHome)
                        End If
                    End If
                    
                End If
            End If
        End If

    End Sub

    Protected Overrides Sub OnLoad(ByVal e As EventArgs)
        '... add custom logic here ...
        If Session(clsManage.iSession.user_id_center.ToString) Is Nothing Then
            clsManage.alert(Page, "Session Time Out Please Login Again.", , "log_in.aspx")
        End If
        'Be sure to call the base class's OnLoad method!
        MyBase.OnLoad(e)
    End Sub

    Sub Page_Error(ByVal sender As Object, ByVal e As EventArgs)
        Server.Transfer("error_page.aspx")
    End Sub


End Class
