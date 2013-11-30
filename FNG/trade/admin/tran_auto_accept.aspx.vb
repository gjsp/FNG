
Partial Class trade_admin_tran_auto_accept
    Inherits basePageTrade

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            btnBuy96.Attributes.Add("onclick", "return confirm('Do you want to clear.');")
            btnSell96.Attributes.Add("onclick", "return confirm('Do you want to clear.');")
            btnBuy99.Attributes.Add("onclick", "return confirm('Do you want to clear.');")
            btnSell99.Attributes.Add("onclick", "return confirm('Do you want to clear.');")

            Dim clearEach = clsMain.getAllQuantityOrder()
            Dim cssBtnRed As String = "buttonPro rounded red"
            Dim cssBtnGreen As String = "buttonPro rounded green"
            IIf(clearEach.Substring(0, 1) = "y", btnBuy96.CssClass = cssBtnRed, btnBuy96.CssClass = cssBtnGreen)
            IIf(clearEach.Substring(1, 1) = "y", btnSell96.CssClass = cssBtnRed, btnBuy96.CssClass = cssBtnGreen)
            IIf(clearEach.Substring(2, 1) = "y", btnBuy99.CssClass = cssBtnRed, btnBuy96.CssClass = cssBtnGreen)
            IIf(clearEach.Substring(3, 1) = "y", btnSell99.CssClass = cssBtnRed, btnBuy96.CssClass = cssBtnGreen)

        End If
    End Sub

    Protected Sub UpdateProg1_Load(sender As Object, e As System.EventArgs) Handles UpdateProg1.Load
        System.Threading.Thread.Sleep(2000)
    End Sub

    Protected Sub btnBuy96_Click(sender As Object, e As System.EventArgs) Handles btnBuy96.Click
        Try
            Dim result As Integer = clsMain.ClearAllAutoAccept2("buy", "96")
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "Error")
        End Try
    End Sub

    Protected Sub btnSell96_Click(sender As Object, e As System.EventArgs) Handles btnSell96.Click
        Try
            Dim result As Integer = clsMain.ClearAllAutoAccept2("sell", "96")
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "Error")
        End Try
    End Sub

    Protected Sub btnBuy99_Click(sender As Object, e As System.EventArgs) Handles btnBuy99.Click
        Try
            Dim result As Integer = clsMain.ClearAllAutoAccept2("buy", "99")
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "Error")
        End Try
    End Sub

    Protected Sub btnSell99_Click(sender As Object, e As System.EventArgs) Handles btnSell99.Click
        Try
            Dim result As Integer = clsMain.ClearAllAutoAccept2("sell", "99")
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "Error")
        End Try
    End Sub

End Class
