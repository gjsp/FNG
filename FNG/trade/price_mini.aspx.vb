
Partial Class price_mini
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            If Session(clsManage.iSession.cust_id.ToString) IsNot Nothing Then
                hdfLv.Value = Session(clsManage.iSession.cust_lv.ToString).ToString
                hdfCust_id.Value = Session(clsManage.iSession.cust_id.ToString).ToString
            Else
                hdfLv.Value = ""
                hdfCust_id.Value = ""
            End If
            getStock()
        End If
    End Sub

    Private Sub getStock()
        Try
            If Session(clsManage.iSession.cust_id.ToString) Is Nothing Then Exit Sub
            If Session(clsManage.iSession.cust_id.ToString).ToString <> hdfCust_id.Value Then
                Session.Clear()
                clsManage.Script(Page, "top.window.location.href='login.aspx'", "loginDup3")
                Exit Sub
            End If

            Dim sto As New clsStore
            sto = clsStore.getPriceStore(Session(clsManage.iSession.cust_id.ToString).ToString)

            If Not Page.IsPostBack And sto.timeTrade = "n" Then
                lblBid96.ForeColor = Drawing.Color.Silver
                lblAsk96.ForeColor = Drawing.Color.Silver
            Else
                lblBid96.ForeColor = Drawing.Color.White
                lblAsk96.ForeColor = Drawing.Color.White
            End If

            Dim bid96 As Double = sto.bid96 - 10
            Dim ask96 As Double = sto.ask96 + 10

            lblBid96.Text = bid96.ToString("###0.00")
            lblAsk96.Text = ask96.ToString("###0.00")

            hdfMax.Value = sto.maxKg.ToString + "," + sto.maxBaht.ToString
            lblMax96.Text = "(ปริมาณซื้อขายสูงสุด " + clsManage.convert2Price(sto.maxBaht.ToString) + " บาท ต่อครั้ง)"

            hdfLv.Value = sto.level.ToString
            clsManage.Script(Page, String.Format("setPriceToTrade('{0}','{1}')", bid96.ToString("###0.00"), ask96.ToString("###0.00")))

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    <System.Web.Services.WebMethod()> _
    Public Shared Function getPrice(ByVal cust_id As String) As String
        Dim result As String = ""
        Try
            Dim cma = "," : Dim l As String = "|" : Dim shp As String = "#"
            Dim sto As New clsStore
            sto = clsStore.getPriceStore(cust_id)
            result = sto.bid96.ToString("###0.00") + cma + sto.ask96.ToString("###0.00") + l + _
                         sto.timeTrade + l + sto.maxKg.ToString("###0.00") + cma + sto.maxBaht.ToString("###0.00") + l + sto.adminHalt + l + sto.systemHalt

            Return result
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Protected Sub upPrice_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles upPrice.Load
        getStock()
    End Sub

End Class
