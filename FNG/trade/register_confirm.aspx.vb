
Partial Class trade_register_confirm
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.QueryString("r") IsNot Nothing And Request.QueryString("u") IsNot Nothing Then
                Dim code As String = Request.QueryString("r").ToString
                Dim username As String = Request.QueryString("u").ToString

                Dim dt As New Data.DataTable
                dt = clsMain.getUsernamesByUsername(username)
                If dt.Rows.Count > 0 Then
                    If dt.Rows(0)("reg_code") = code Then
                        If clsMain.updateUserActive(username) > 0 Then
                            pnSeccess.Visible = True
                        Else
                            pnFail.Visible = True
                        End If
                    Else
                        pnFail.Visible = True
                    End If
                End If
            End If
        End If
    End Sub
End Class
