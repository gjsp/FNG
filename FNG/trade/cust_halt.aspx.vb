
Partial Class trade_cust_halt
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Session(clsManage.iSession.cust_id.ToString) Is Nothing Then
            Response.Redirect("login.aspx")
        End If

        If Not Page.IsPostBack Then
            hdfRole.Value = Session(clsManage.iSession.role.ToString).ToString
        End If
    End Sub

    <System.Web.Services.WebMethod()> _
    Public Shared Function getHalt() As String
        Try
            Return clsSpot.getSystemHalt
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
