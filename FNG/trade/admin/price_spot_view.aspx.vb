
Partial Class trade_admin_price_spot_view
    Inherits System.Web.UI.Page
    Dim cssHalt As String = "buttonPro small red"
    Dim cssNor As String = "buttonPro small blue"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            getStock()
        End If
    End Sub

    Private Sub getStock()
        Try
            Dim sp As New clsSpot.SpotPrice
            sp = clsSpot.getSpotPrice()
            Dim priceFormatKg As String = "#,##0.00"
            Dim priceFormat As String = "#,##0"

            lbl99Bid.Text = sp.bidSpot.ToString(priceFormatKg)
            lbl99Ask.Text = sp.askSpot.ToString(priceFormatKg)
            lblBidThb.Text = sp.bidTHB.ToString(priceFormatKg)
            lblAskThb.Text = sp.askTHB.ToString(priceFormatKg)

            lbl99BidKg.Text = sp.bid99Kg.ToString(priceFormatKg)
            lbl99AskKg.Text = sp.ask99Kg.ToString(priceFormatKg)

            lbl99BidBg.Text = sp.bid99Bg.ToString(priceFormat)
            lbl99AskBg.Text = sp.ask99Bg.ToString(priceFormat)

            lbl96Bid.Text = sp.bid96Bg.ToString(priceFormat)
            lbl96Ask.Text = sp.ask96Bg.ToString(priceFormat)


        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub upPrice_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles upPrice.Load
       getStock()
    End Sub

    <System.Web.Services.WebMethod()> _
    Public Shared Function getPrice() As String
        Dim priceFormat As String = "#,##0.00"
        Dim result As String = ""
        Try
            Dim cma = "," : Dim l As String = "|" : Dim shp As String = "#"
            Dim sp As New clsSpot.SpotPrice
            sp = clsSpot.getSpotPrice()
            result = sp.bid99Kg.ToString(priceFormat) + l + sp.ask99Kg.ToString(priceFormat) + l + sp.bidTHB.ToString(priceFormat) + l + sp.askTHB.ToString(priceFormat)

            'result = sp.bid99Kg.ToString(priceFormat) + cma + sp.ask99Kg.ToString(priceFormat) + cma + sp.bid96Bg.ToString(priceFormat) + cma + sp.ask96Bg.ToString(priceFormat) + l + _
            '         sp.timeTrade + l + sp.maxKg.ToString(priceFormat) + cma + sp.maxBaht.ToString(priceFormat) + l + sp.adminHalt

            Return result
        Catch ex As Exception
            Throw ex
        End Try
    End Function

End Class
