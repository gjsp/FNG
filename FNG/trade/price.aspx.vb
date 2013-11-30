
Partial Class price
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

    Private Sub getStock2()
        Try
            If Session(clsManage.iSession.cust_id.ToString) Is Nothing Then Exit Sub
            If Session(clsManage.iSession.cust_id.ToString).ToString <> hdfCust_id.Value Then
                Session.Clear()
                clsManage.Script(Page, "top.window.location.href='login.aspx'", "loginDup3")
                Exit Sub
            End If

            Dim sto As New clsStore
            sto = clsStore.getPriceStore(Session(clsManage.iSession.cust_id.ToString).ToString)


            Dim sp As New clsSpot.SpotPrice
            sp = clsSpot.getSpotPrice()
            Dim priceFormatKg As String = "###0.00"
            Dim priceFormat As String = "###0"

            Dim formatPrice As String = "###0"
            lblBid99.Text = sp.bid99Bg.ToString(formatPrice)
            lblAsk99.Text = sp.ask99Bg.ToString(formatPrice)
            lblBid96.Text = sp.bid96Bg.ToString(formatPrice)
            lblAsk96.Text = sp.ask96Bg.ToString(formatPrice)
            lblBid96Mini.Text = sto.bid96Mini.ToString(formatPrice)
            lblAsk96Mini.Text = sto.ask96Mini.ToString(formatPrice)

            hdfMax.Value = sto.maxKg.ToString + "," + sto.maxBaht.ToString
            lblMax99.Text = "(ปริมาณซื้อขายสูงสุด " + clsManage.convert2Price(sto.maxKg.ToString) + " กิโล ต่อครั้ง)"
            lblMax96.Text = "(ปริมาณซื้อขายสูงสุด " + clsManage.convert2Price(sto.maxBaht.ToString) + " บาท ต่อครั้ง)"
            lblMax96Mini.Text = "(ปริมาณซื้อขายสูงสุด " + clsManage.convert2Price(sto.maxMini.ToString) + " บาท ต่อครั้ง)"

            hdfLv.Value = sto.level.ToString
            clsManage.Script(Page, String.Format("setPriceToTrade('{0}','{1}','{2}','{3}','{4}','{5}')", sp.bid99Bg.ToString(formatPrice), sp.ask99Bg.ToString(formatPrice), sp.bid96Bg.ToString(formatPrice), sp.ask96Bg.ToString(formatPrice), sto.bid96Mini.ToString(formatPrice), sto.ask96Mini.ToString(formatPrice)))

        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    Private Sub getStock()
        Try
            If Session(clsManage.iSession.cust_id.ToString) Is Nothing Then
                Session.Clear()
                clsManage.Script(Page, "top.window.location.href='login.aspx'", "loginDup3")
                Exit Sub
            End If

            If Session(clsManage.iSession.cust_id.ToString).ToString <> hdfCust_id.Value Then
                Session.Clear()
                clsManage.Script(Page, "top.window.location.href='login.aspx'", "loginDup3")
                Exit Sub
            End If

            Dim spo As New clsSpot.SpotPrice
            spo = clsSpot.getSpotPriceForCust(Session(clsManage.iSession.cust_id.ToString).ToString)

            Dim formatPrice2 As String = "###0.00"
            Dim formatPrice As String = "###0"

            lblBid99.Text = spo.bid99Bg.ToString(formatPrice)
            lblAsk99.Text = spo.ask99Bg.ToString(formatPrice)
            lblBid96.Text = spo.bid96Bg.ToString(formatPrice)
            lblAsk96.Text = spo.ask96Bg.ToString(formatPrice)
            lblBid96Mini.Text = spo.bid96Mn.ToString(formatPrice)
            lblAsk96Mini.Text = spo.ask96Mn.ToString(formatPrice)

            lblbidSpot.Text = spo.bidSpot.ToString(formatPrice2)
            lblaskSpot.Text = spo.askSpot.ToString(formatPrice2)
            lblbidThb.Text = spo.bidTHB.ToString(formatPrice2)
            lblaskThb.Text = spo.askTHB.ToString(formatPrice2)

            hdfMax.Value = spo.maxKg.ToString + "," + spo.maxBg.ToString
            lblMax99.Text = "(ปริมาณซื้อขายสูงสุด " + clsManage.convert2Price(spo.maxKg.ToString) + " กิโล ต่อครั้ง)"
            lblMax96.Text = "(ปริมาณซื้อขายสูงสุด " + clsManage.convert2Price(spo.maxBg.ToString) + " บาท ต่อครั้ง)"
            lblMax96Mini.Text = "(ปริมาณซื้อขายสูงสุด " + clsManage.convert2Price(spo.maxMn.ToString) + " บาท ต่อครั้ง)"

            hdfLv.Value = "1"
            clsManage.Script(Page, String.Format("setPriceToTrade('{0}','{1}','{2}','{3}','{4}','{5}')", spo.bid99Bg.ToString(formatPrice), spo.ask99Bg.ToString(formatPrice), _
                                                 spo.bid96Bg.ToString(formatPrice), spo.ask96Bg.ToString(formatPrice), spo.bid96Mn.ToString(formatPrice), spo.ask96Mn.ToString(formatPrice)))

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    <System.Web.Services.WebMethod()> _
    Public Shared Function getPrice(ByVal cust_id As String) As String
        Dim result As String = ""
        Try
            Dim formatPrice As String = "###0"
            Dim cma = "," : Dim l As String = "|" : Dim shp As String = "#"
            Dim spo As New clsSpot.SpotPrice
            spo = clsSpot.getSpotPriceForCust(cust_id)
            result = spo.bid99Bg.ToString(formatPrice) + cma + spo.ask99Bg.ToString(formatPrice) + cma + spo.bid96Bg.ToString(formatPrice) + cma + spo.ask96Bg.ToString(formatPrice) + cma + spo.bid96Mn.ToString(formatPrice) + cma + spo.ask96Mn.ToString(formatPrice) + l + _
                         "y" + l + spo.maxKg.ToString(formatPrice) + cma + spo.maxBg.ToString(formatPrice) + cma + spo.maxMn.ToString(formatPrice) + _
                         l + spo.sysHalt
            'l + spo.bidSpot.ToString(formatPrice) + cma + spo.askSpot.ToString(formatPrice) + _
            'l + spo.bidTHB.ToString(formatPrice) + cma + spo.askSpot.ToString(formatPrice)

            'Dim sto As New clsStore
            'sto = clsStore.getPriceStore(cust_id)
            'result = sto.bid99.ToString(formatPrice) + cma + sto.ask99.ToString(formatPrice) + cma + sto.bid96.ToString(formatPrice) + cma + sto.ask96.ToString(formatPrice) + cma + sto.bid96Mini.ToString(formatPrice) + cma + sto.ask96Mini.ToString(formatPrice) + l + _
            '             sto.timeTrade + l + sto.maxKg.ToString(formatPrice) + cma + sto.maxBaht.ToString(formatPrice) + cma + sto.maxMini.ToString(formatPrice) + _
            '             l + sto.adminHalt + l + sto.systemHalt

            Return result
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Protected Sub upPrice_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles upPrice.Load
        getStock()
    End Sub

End Class
