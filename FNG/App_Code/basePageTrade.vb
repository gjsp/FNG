Imports Microsoft.VisualBasic

Public Class basePageTrade
    Inherits System.Web.UI.Page

    'Public Overloads Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    'End Sub

    Protected Overrides Sub OnLoad(ByVal e As EventArgs)
        '... add custom logic here ...
        If Session(clsManage.iSession.user_id.ToString) Is Nothing Then
            clsManage.alert(Page, "Session Time Out Please Login Again.", , "../login.aspx")
        End If
        'Be sure to call the base class's OnLoad method!
        MyBase.OnLoad(e)
    End Sub


End Class
