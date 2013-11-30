
Partial Class trade_admin_price_spot
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            btnSave.Attributes.Add("onclick", "return confirm('Do you want to Save.');")

            txtPremium.Attributes.Add("onkeypress", "checkPremium();")
            txtFxAsk.Attributes.Add("onkeypress", "checkNumber();")
            txtFxBid.Attributes.Add("onkeypress", "checkNumber();")
            txtSpace99kg.Attributes.Add("onkeypress", "numberOnly();")
            txtSpace99Bg.Attributes.Add("onkeypress", "numberOnly();")
            txtSpace96Bg.Attributes.Add("onkeypress", "numberOnly();")
            
            txtMaxBg.Attributes.Add("onkeypress", "numberOnly();")
            txtMaxKg.Attributes.Add("onkeypress", "numberOnly();")
            txtMaxMn.Attributes.Add("onkeypress", "numberOnly();")
            txtRangeLeave.Attributes.Add("onkeypress", "checkNumber();")

            getStock()
        End If
    End Sub

    Private Sub getStock()
        Try
            Dim sp As New clsSpot.SpotPrice
            sp = clsSpot.getSpotPrice()

            txtPremium.Text = sp.premium.ToString
            txtSpace99kg.Text = sp.space99Kg.ToString
            txtSpace99Bg.Text = sp.space99Bg.ToString
            txtSpace96Bg.Text = sp.space96Bg.ToString

            txtFxAsk.Text = sp.fxAsk
            txtFxBid.Text = sp.fxBid
            txtMeltingCost.Text = sp.meltingCost

            txtMaxBg.Text = sp.maxBg.ToString
            txtMaxKg.Text = sp.maxKg.ToString
            txtMaxMn.Text = sp.maxMn.ToString
            txtRangeLeave.Text = sp.rangeLeave.ToString

        Catch ex As Exception
            Throw ex
        End Try
    End Sub


    Function validateSave() As Boolean
        Try
            If txtPremium.Text.Trim = "" Or txtFxAsk.Text.Trim = "" Or txtFxBid.Text.Trim = "" Or txtSpace99kg.Text.Trim = "" Or txtSpace99Bg.Text.Trim = "" Or txtSpace96Bg.Text.Trim = "" _
                Or txtMaxBg.Text.Trim = "" Or txtMaxKg.Text.Trim = "" Or txtMaxMn.Text.Trim = "" Or txtRangeLeave.Text.Trim = "" Then
                clsManage.alert(Page, "โปรดใส่ข้อมูลให้เรียบร้อย")
                Return False
            End If
            If IsNumeric(txtPremium.Text) = False Or IsNumeric(txtFxAsk.Text) = False Or IsNumeric(txtFxBid.Text) = False Or IsNumeric(txtSpace99kg.Text) = False Or IsNumeric(txtSpace99Bg.Text) = False Or IsNumeric(txtSpace96Bg.Text) = False _
                Or IsNumeric(txtMaxBg.Text) = False Or IsNumeric(txtMaxKg.Text) = False Or IsNumeric(txtMaxMn.Text) = False Or IsNumeric(txtRangeLeave.Text) = False _
                Then
                clsManage.alert(Page, "โปรดใส่ข้อมูลเฉพาะที่เป็นตัวเลข")
                Return False
            End If
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If Not validateSave() Then Exit Sub

            Dim dc As New dcDBDataContext
            Dim sto = (From s In dc.stock_onlines Select s).FirstOrDefault

            sto.premium = txtPremium.Text
            sto.space_kg99 = txtSpace99kg.Text
            sto.space_bg99 = txtSpace99Bg.Text
            sto.space_bg96 = txtSpace96Bg.Text
            sto.fx_ask = txtFxAsk.Text
            sto.fx_bid = txtFxBid.Text
            sto.melting_cost = txtMeltingCost.Text

            sto.max_baht = txtMaxBg.Text
            sto.max_kg = txtMaxKg.Text
            sto.max_mini = txtMaxMn.Text
            sto.range_leave_order = txtRangeLeave.Text

            sto.modifier_by = Session(clsManage.iSession.user_name.ToString).ToString
            dc.SubmitChanges()

            getStock()
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "err")
        Finally
            Threading.Thread.Sleep(UpdateProg1.DisplayAfter + 300)
        End Try
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            getStock()
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "err")
        Finally
            Threading.Thread.Sleep(UpdateProg1.DisplayAfter)
        End Try
    End Sub

End Class
