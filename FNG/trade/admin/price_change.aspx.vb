
Partial Class trade_admin_price_change
    Inherits System.Web.UI.Page
    Dim cssHalt As String = "buttonPro small red"
    Dim cssNor As String = "buttonPro small blue"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            txt99Ask.Attributes.Add("onkeypress", "numberOnly();")
            txt99Bid.Attributes.Add("onkeypress", "numberOnly();")
            txt96Ask.Attributes.Add("onkeypress", "numberOnly();")
            txt96Bid.Attributes.Add("onkeypress", "numberOnly();")

            txt99Bid.Attributes.Add("onblur", "calBidAsk();")
            txt99Ask.Attributes.Add("onblur", "calBidAsk();")
            txt96Bid.Attributes.Add("onblur", "calBidAsk();")
            txt96Ask.Attributes.Add("onblur", "calBidAsk();")

            btnSave.Attributes.Add("onclick", "return confirm('Do you want to Save.');")

            img99BidUp.Attributes.Add("onclick", "addBidAsk('up','" + txt99Bid.ClientID + "')")
            img99BidDown.Attributes.Add("onclick", "addBidAsk('down','" + txt99Bid.ClientID + "')")

            img99AskUp.Attributes.Add("onclick", "addBidAsk('up','" + txt99Ask.ClientID + "')")
            img99AskDown.Attributes.Add("onclick", "addBidAsk('down','" + txt99Ask.ClientID + "');")


            img96BidUp.Attributes.Add("onclick", "addBidAsk('up','" + txt96Bid.ClientID + "')")
            img96BidDown.Attributes.Add("onclick", "addBidAsk('down','" + txt96Bid.ClientID + "')")

            img96AskUp.Attributes.Add("onclick", "addBidAsk('up','" + txt96Ask.ClientID + "')")
            img96AskDown.Attributes.Add("onclick", "addBidAsk('down','" + txt96Ask.ClientID + "')")

            Dim stock = New dcDBDataContext().getStockOnline().Single()

            txt99Bid.Text = IIf(stock.bid99_plus Is Nothing, 0, stock.bid99_plus)
            txt99Ask.Text = IIf(stock.ask99_plus Is Nothing, 0, stock.ask99_plus)
            txt96Bid.Text = IIf(stock.bid96_plus Is Nothing, 0, stock.bid96_plus)
            txt96Ask.Text = IIf(stock.ask96_plus Is Nothing, 0, stock.ask96_plus)
            updateHalt(stock.self_halt)

            'ViewState("bid_plus") = stock.bid99_plus.ToString
            'ViewState("ask_plus") = stock.ask99_plus.ToString

            getStock()
            calPrice()
        End If
    End Sub

    Private Sub calPrice()
        lbl99BidSelf.Text = (clsManage.convert2zero(lbl99Bid.Text) - clsManage.convert2zero(txt99Bid.Text)).ToString("###0.00")
        lbl99AskSelf.Text = (clsManage.convert2zero(lbl99Ask.Text) + clsManage.convert2zero(txt99Ask.Text)).ToString("###0.00")

        lbl96BidSelf.Text = (clsManage.convert2zero(lbl96Bid.Text) - clsManage.convert2zero(txt96Bid.Text)).ToString("###0.00")
        lbl96AskSelf.Text = (clsManage.convert2zero(lbl96Ask.Text) + clsManage.convert2zero(txt96Ask.Text)).ToString("###0.00")

    End Sub

    Private Sub getStock()
        Try
            If Session(clsManage.iSession.user_id.ToString) Is Nothing Then Exit Sub
            Dim sto As New clsStore()
            sto = clsStore.getPriceStore()

            If Not Page.IsPostBack And sto.timeTrade = "n" Then
                lbl99Bid.ForeColor = Drawing.Color.Silver
                lbl99Ask.ForeColor = Drawing.Color.Silver
                lbl96Bid.ForeColor = Drawing.Color.Silver
                lbl96Ask.ForeColor = Drawing.Color.Silver
            Else
                lbl99Bid.ForeColor = New Drawing.ColorConverter().ConvertFromString("#0066FF")
                lbl99Ask.ForeColor = New Drawing.ColorConverter().ConvertFromString("#FF6600")
                lbl96Bid.ForeColor = New Drawing.ColorConverter().ConvertFromString("#0066FF")
                lbl96Ask.ForeColor = New Drawing.ColorConverter().ConvertFromString("#FF6600")
            End If
            lbl99Bid.Text = sto.bid99.ToString("###0.00")
            lbl99Ask.Text = sto.ask99.ToString("###0.00")
            lbl96Bid.Text = sto.bid96.ToString("###0.00")
            lbl96Ask.Text = sto.ask96.ToString("###0.00")

            clsManage.Script(Page, String.Format("setPriceToTrade('{0}','{1}','{2}','{3}')", sto.bid99.ToString("###0.00"), sto.ask99.ToString("###0.00"), sto.bid96.ToString("###0.00"), sto.ask96.ToString("###0.00")))
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    <System.Web.Services.WebMethod()> _
    Public Shared Function getPrice() As String
        Dim result As String = ""
        Try
            Dim cma = "," : Dim l As String = "|" : Dim shp As String = "#"
            Dim sto As New clsStore
            sto = clsStore.getPriceStore()
            result = sto.bid99.ToString("###0.00") + cma + sto.ask99.ToString("###0.00") + cma + sto.bid96.ToString("###0.00") + cma + sto.ask96.ToString("###0.00") + l + _
                     sto.timeTrade + l + sto.maxKg.ToString("###0.00") + cma + sto.maxBaht.ToString("###0.00") + l + sto.adminHalt

            Return result
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Protected Sub upPrice_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles upPrice.Load
        'getStock()
        calPrice()
    End Sub

    Function checkTimeout() As Boolean
        Dim dt As Data.DataTable
        dt = clsMain.getPriceOnline()
        If dt.Rows.Count > 0 Then
            If dt.Rows(0)("time_end").ToString <> "" Then
                If CDate(dt.Rows(0)("time_end")) < DateTime.Now Then
                    Return False
                End If
            End If
        End If
        Return True
    End Function

    Function validateSave() As Boolean
        Try
            If txt99Bid.Text.Trim = "" _
                       Or txt99Ask.Text.Trim = "" _
                       Or txt96Bid.Text.Trim = "" _
                       Or txt96Ask.Text.Trim = "" _
                       Then
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
            'If Not checkTimeout() Then clsManage.alert(Page, "Trade Online Time Out.", , , "timeout") : Exit Sub
            Dim dc As New dcDBDataContext
            Dim stock = (From s In dc.stock_onlines Select s).FirstOrDefault
            stock.bid99_plus = txt99Bid.Text
            stock.ask99_plus = txt99Ask.Text
            stock.bid96_plus = txt96Bid.Text
            stock.ask96_plus = txt96Ask.Text
            stock.self_price = "n"
            stock.modifier_by = Session(clsManage.iSession.user_name.ToString).ToString
            dc.SubmitChanges()

        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "err")
        Finally
            Threading.Thread.Sleep(UpdateProg1.DisplayAfter)
        End Try
    End Sub

#Region "UPandDown"

    'Protected Sub img99BidUp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles img99BidUp.Click
    '    Try
    '        If txt99Bid.Text <> "" Then
    '            Dim num As Double = clsManage.convert2zero(txt99Bid.Text) + 1
    '            txt99Bid.Text = num.ToString
    '        End If
    '    Catch ex As Exception
    '        clsManage.alert(Page, ex.Message)
    '    Finally
    '        'Threading.Thread.Sleep(UpdateProg1.DisplayAfter)
    '    End Try

    'End Sub

    'Protected Sub img99BidDown_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles img99BidDown.Click
    '    Try
    '        If txt99Bid.Text <> "" Then
    '            Dim num As Double = clsManage.convert2zero(txt99Bid.Text) - 1
    '            txt99Bid.Text = num.ToString
    '        End If
    '    Catch ex As Exception
    '        clsManage.alert(Page, ex.Message)
    '    Finally
    '        'Threading.Thread.Sleep(UpdateProg1.DisplayAfter)
    '    End Try
    'End Sub

    'Protected Sub img99AskUp_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles img99AskUp.Click
    '    Try
    '        If txt99Ask.Text <> "" Then
    '            Dim num As Double = clsManage.convert2zero(txt99Ask.Text) + 1
    '            txt99Ask.Text = num.ToString
    '        End If
    '    Catch ex As Exception
    '        clsManage.alert(Page, ex.Message)
    '    Finally
    '        'Threading.Thread.Sleep(UpdateProg1.DisplayAfter)
    '    End Try
    'End Sub

    'Protected Sub img99AskDown_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles img99AskDown.Click
    '    Try
    '        If txt99Ask.Text <> "" Then
    '            Dim num As Double = clsManage.convert2zero(txt99Ask.Text) - 1
    '            txt99Ask.Text = num.ToString
    '        End If

    '    Catch ex As Exception
    '        clsManage.alert(Page, ex.Message)
    '    Finally
    '        'Threading.Thread.Sleep(UpdateProg1.DisplayAfter)
    '    End Try
    'End Sub

#End Region

    Protected Sub btnHalt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHalt.Click
        Try
            Threading.Thread.Sleep(1000)
            Dim dc As New dcDBDataContext
            Dim stock = (From s In dc.stock_onlines Select s).FirstOrDefault
            Dim halt As String = ""
            If btnHalt.CommandArgument = "n" Then
                halt = "y"
            Else
                halt = "n"
            End If
            stock.self_halt = halt
            stock.modifier_by = Session(clsManage.iSession.user_name.ToString).ToString
            dc.SubmitChanges()
            updateHalt(halt)
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "err")
        End Try
    End Sub

    Sub updateHalt(ByVal halt As String)
        If halt = "" Then Exit Sub
        If halt = "n" Then
            btnHalt.CssClass = cssNor
        Else
            btnHalt.CssClass = cssHalt
        End If
        btnHalt.CommandArgument = halt
    End Sub

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            Dim stock = New dcDBDataContext().getStockOnline().Single()
            txt99Bid.Text = IIf(stock.bid99_plus Is Nothing, 0, stock.bid99_plus)
            txt99Ask.Text = IIf(stock.ask99_plus Is Nothing, 0, stock.ask99_plus)
            txt96Bid.Text = IIf(stock.bid96_plus Is Nothing, 0, stock.bid96_plus)
            txt96Ask.Text = IIf(stock.ask96_plus Is Nothing, 0, stock.ask96_plus)
            getStock()
            calPrice()
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "err")
        End Try
    End Sub

End Class
