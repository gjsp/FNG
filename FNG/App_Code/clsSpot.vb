Imports Microsoft.VisualBasic

Public Class clsSpot
    Public Structure SpotPrice
        Public premium As Double

        Public bidSpot As Double
        Public bidTHB As Double
        Public bid99Kg As Double
        Public bid99Bg As Double
        Public bid96Bg As Double
        Public bid96Mn As Double

        Public askSpot As Double
        Public askTHB As Double
        Public ask99Kg As Double
        Public ask99Bg As Double
        Public ask96Bg As Double
        Public ask96Mn As Double

        Public fxBid As Double
        Public fxAsk As Double
        Public meltingCost As Double

        Public space99Kg As Double
        Public space99Bg As Double
        Public space96Bg As Double
        'Public selfHalt As String
        Public custHalt As String
        Public sysHalt As String
        'Public adminHalt As String

        Public maxBg As Double
        Public maxKg As Double
        Public maxMn As Double
        Public rangeLeave As Double

    End Structure

    Public Shared Function getSpotPrice() As SpotPrice

        'Calculate Solution Finest Gold
        Dim sp As SpotPrice
        Dim sto = New dcDBDataContext().getStockOnline().Single()
        Dim spot = New dcDBDataContext().getSpot().Single()

        sp.premium = sto.premium
        sp.space99Kg = sto.space_kg99
        sp.space99Bg = sto.space_bg99
        sp.space96Bg = sto.space_bg96
        'sp.selfHalt = sto.halt
        sp.sysHalt = sto.system_halt
        sp.maxKg = sto.max_kg
        sp.maxBg = sto.max_baht
        sp.maxMn = sto.max_mini
        sp.rangeLeave = sto.range_leave_order
    

        sp.fxAsk = sto.fx_ask
        sp.fxBid = sto.fx_bid
        sp.meltingCost = sto.melting_cost

        sp.bidSpot = spot.bid
        sp.bidTHB = spot.bid_thb

        sp.askSpot = spot.ask
        sp.askTHB = spot.ask_thb

        ''ราคา 99.99/kg
        sp.ask99Kg = (spot.ask + sto.premium) * 32.148 * spot.ask_thb
        sp.ask99Kg = Math.Round(sp.ask99Kg, 2)
        sp.bid99Kg = sp.ask99Kg - sto.space_kg99

        ''price 99.99/baht gold
        Dim ask99BgTemp As Double = sp.ask99Kg / 65.6
        Dim mod99bg As Double = ask99BgTemp Mod 5
        If mod99bg < 2.5 Then
            sp.ask99Bg = ask99BgTemp - mod99bg
        Else
            sp.ask99Bg = ask99BgTemp + 5 - mod99bg
        End If
        sp.bid99Bg = sp.ask99Bg - sto.space_bg99

        ''price 96.50/baht gold
        Dim ask96BgTemp As Double = sp.ask99Bg * 96.5 / 99.99
        Dim mod96 As Double = ask96BgTemp Mod 5
        If mod96 < 2.5 Then
            sp.ask96Bg = ask96BgTemp - mod96
        Else
            sp.ask96Bg = ask96BgTemp + 5 - mod96
        End If
        sp.bid96Bg = sp.ask96Bg - sto.space_bg96

        ''Price 96.50 Mini
        sp.ask96Mn = sp.ask96Bg + 10
        sp.bid96Mn = sp.bid96Bg - 10

        Return sp

    End Function

    Public Shared Function getSpotPriceForCust(cust_id As String) As SpotPrice

        'Calculate Solution Finest Gold
        Dim sp As SpotPrice
        'Dim sto = New dcDBDataContext().getStockOnline().Single()
        Dim st = New dcDBDataContext().getStockOnlineForPrice(cust_id).Single()
        Dim spot = New dcDBDataContext().getSpot().Single()

        sp.premium = st.premium.ToString
        sp.space99Kg = st.space_kg99
        sp.space99Bg = st.space_bg99
        sp.space96Bg = st.space_bg96
        'sp.selfHalt = st.halt
        sp.sysHalt = st.system_halt
        sp.custHalt = st.cust_halt

        sp.maxKg = st.max_kg
        sp.maxBg = st.max_baht
        sp.maxMn = st.max_mini

        '' Gold Sport
        sp.bidSpot = spot.bid
        sp.bidTHB = spot.bid_thb - st.fx_bid

        sp.askSpot = spot.ask + st.premium
        sp.askTHB = spot.ask_thb + st.fx_ask

        ''ราคา 99.99/kg
        sp.ask99Kg = (spot.ask + st.premium) * 32.148 * spot.ask_thb
        sp.ask99Kg = Math.Round(sp.ask99Kg, 2)
        sp.bid99Kg = sp.ask99Kg - st.space_kg99

        ''price 99.99/baht gold
        Dim ask99BgTemp As Double = sp.ask99Kg / 65.6
        Dim mod99bg As Double = ask99BgTemp Mod 5
        If mod99bg < 2.5 Then
            sp.ask99Bg = ask99BgTemp - mod99bg
        Else
            sp.ask99Bg = ask99BgTemp + 5 - mod99bg
        End If
        sp.bid99Bg = sp.ask99Bg - st.space_bg99

        ''price 96.50/baht gold
        Dim ask96BgTemp As Double = sp.ask99Bg * 96.5 / 99.99
        Dim mod96 As Double = ask96BgTemp Mod 5
        If mod96 < 2.5 Then
            sp.ask96Bg = ask96BgTemp - mod96
        Else
            sp.ask96Bg = ask96BgTemp + 5 - mod96
        End If
        sp.bid96Bg = sp.ask96Bg - st.space_bg96
        sp.ask96Bg += st.melting_cost

        ''Price 96.50 Mini
        sp.ask96Mn = sp.ask96Bg + 10
        sp.bid96Mn = sp.bid96Bg - 10

        Return sp

    End Function
End Class
