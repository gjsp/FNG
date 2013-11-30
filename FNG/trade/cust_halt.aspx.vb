
Partial Class trade_cust_halt
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Session(clsManage.iSession.cust_id.ToString) Is Nothing Then
            Response.Redirect("login.aspx")
        End If
        If Not Page.IsPostBack Then
            hdfCust_id.Value = Session(clsManage.iSession.cust_id.ToString).ToString
        End If
    End Sub

    <System.Web.Services.WebMethod()> _
    Public Shared Function getHalt(ByVal cust_id As String) As String
        Dim result As String = ""
        Try
           
            Dim sto As New clsStore
            sto = clsStore.getPriceStore(cust_id)
            result =  sto.systemHalt

            Return result
        Catch ex As Exception
            Throw ex
        End Try
    End Function
End Class
