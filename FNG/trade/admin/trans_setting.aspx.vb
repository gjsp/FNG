
Partial Class admin_trans_setting
    Inherits System.Web.UI.Page
    Dim ht As Hashtable
    Dim htPrice As Hashtable
    Dim colPrice As Integer = 7
    Dim colQuan As Integer = 8
    Dim colSum As Integer = 9
    Dim cssHalt As String = "buttonPro small red"
    Dim cssNor As String = "buttonPro small blue"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.QueryString("m") IsNot Nothing Then
                If Request.QueryString("m").ToString = "leave" Then
                    hdfMode.Value = clsManage.tradeMode.tran.ToString
                Else
                    hdfMode.Value = Request.QueryString("m").ToString
                End If

                hdfChangeColor.Value = "y"
                Dim timeDifDb As New TimeSpan
                timeDifDb = DateTime.Now - clsMain.getDatetimeServer()
                ViewState("timedif") = timeDifDb

                hdfTradeLogId.Value = clsMain.getTradeLogId(hdfMode.Value)
                hdfTradeLogIdForGrid.Value = hdfTradeLogId.Value
           
                hdfIsRealtime.Value = "y"
                hdfConfirm.Value = "n"
                hdfConfirmSave.Value = "n"

              
                txtBidChangeNumber1.Text = "5" : txtBidChangeNumber2.Text = "10" : txtBidChangeNumber3.Text = "20"
                txtBidChangeNumber1.Attributes.Add("onkeypress", "numberOnly();") : txtBidChangeNumber2.Attributes.Add("onkeypress", "numberOnly();") : txtBidChangeNumber3.Attributes.Add("onkeypress", "checkNumber();")

                imgUp1.Attributes.Add("onclick", "changeBid('this');") : imgUp2.Attributes.Add("onclick", "changeBid('this');") : imgUp3.Attributes.Add("onclick", "changeBid('this');")
                imgDown1.Attributes.Add("onclick", "changeBid('this');") : imgDown2.Attributes.Add("onclick", "changeBid('this');") : imgDown3.Attributes.Add("onclick", "changeBid('this');")

                txtMaxPer.Attributes.Add("onkeypress", "checkNumber();")
                txtMaxBaht.Attributes.Add("onkeypress", "checkNumber();")
                txtMaxKg.Attributes.Add("onkeypress", "checkNumber();")
                txtLeaveTimeout.Attributes.Add("onkeypress", "numberOnly();")
                txtBidMin.Attributes.Add("onkeypress", "numberOnly();")
                txtBidMax.Attributes.Add("onkeypress", "numberOnly();")
                txt99Bid1.Attributes.Add("onkeypress", "numberOnly();")
                txt99Ask1.Attributes.Add("onkeypress", "numberOnly();")
                txtDif9699.Attributes.Add("onkeypress", "numberOnly();")
                TimeTxtBox.Attributes.Add("onkeypress", "numberOnly();")
                txt99DifBidAsk.Attributes.Add("onkeypress", "numberOnly();")
                txt96DifBidAsk.Attributes.Add("onkeypress", "numberOnly();")
                editbt.Attributes.Add("onclick", "return setConfirmSave('" + cbConfirmSave.ClientID + "','Do you want to Save?',this);")
                'cbConfirm.Attributes.Add("onclick", "setConfirm(this);")

                'TimeTxtBox.Attributes.Add("OnBlur", "colorBlur()")
                TimeTxtBox.Attributes.Add("onKeyup", "colorKeyup()")
                txt99Ask1.Attributes.Add("onKeyup", "colorKeyup()")
                txt99Bid1.Attributes.Add("onKeyup", "colorKeyup()")
                txt99DifBidAsk.Attributes.Add("onKeyup", "colorKeyup()")
                txt96DifBidAsk.Attributes.Add("onKeyup", "colorKeyup()")
                txtDif9699.Attributes.Add("onKeyup", "colorKeyup()")
                txtMaxBaht.Attributes.Add("onKeyup", "colorKeyup()")
                txtMaxKg.Attributes.Add("onKeyup", "colorKeyup()")
                txtLeaveTimeout.Attributes.Add("onKeyup", "colorKeyup()")
                txtMaxPer.Attributes.Add("onKeyup", "colorKeyup()")

                getData()

            End If
        End If
    End Sub

    <System.Web.Services.WebMethod()> _
    Public Shared Function checkMaxTradePercent(ByVal bid As String, ByVal newbid As String, ByVal per As String) As String
        Return clsManage.checkMaxTradePercent(bid, newbid, per)
    End Function

    <System.Web.Services.WebMethod()> _
    Public Shared Function getTradeMaxId(ByVal mode As String) As String
        Return clsMain.getTradeLogId(mode)
    End Function

#Region "update Price"
    Function saveData() As Boolean
        Try
            If txt99Bid1.Text = "" Or txt99Ask1.Text = "" Or txt99Bid2.Text = "" Or txt99Ask2.Text = "" Or txt99Bid1.Text = "" Or txt99Ask1.Text = "" Or txt99Bid3.Text = "" Or txt99Ask3.Text = "" _
            Or txt96Bid1.Text = "" Or txt96Ask1.Text = "" Or txt96Bid2.Text = "" Or txt96Ask2.Text = "" Or txt96Bid1.Text = "" Or txt96Ask1.Text = "" Or txt96Bid3.Text = "" Or txt96Ask3.Text = "" _
            Or TimeTxtBox.Text = "" Or txtLeaveTimeout.Text = "" Or txtMaxBaht.Text = "" Or txtMaxKg.Text = "" Or txtDif9699.Text = "" Or txt96DifBidAsk.Text = "" Or txt99DifBidAsk.Text = "" Then
                clsManage.alert(Page, clsManage.msgRequiredFill)
            Else
                If clsManage.convert2zero(txtLeaveTimeout.Text) >= clsManage.convert2zero(TimeTxtBox.Text) Then
                    clsManage.alert(Page, "Please Enter Timeout Less than Inactive Pricing Time.", txtLeaveTimeout.ClientID, , "timeLess")
                Else
                    Dim result As Integer = clsMain.UpdatePriceOnline(txt99Bid1.Text, txt99Ask1.Text, txt99Bid2.Text, txt99Ask2.Text, txt99Bid3.Text, txt99Ask3.Text, _
                                              txt96Bid1.Text, txt96Ask1.Text, txt96Bid2.Text, txt96Ask2.Text, txt96Bid3.Text, txt96Ask3.Text, _
                                              TimeTxtBox.Text, txtBidMin.Text, txtBidMax.Text, txtMaxBaht.Text, txtMaxKg.Text, ViewState("min_ask9699"), ViewState("max_ask9699"), txtLeaveTimeout.Text, txtMaxPer.Text, ViewState("range_leave"), "y", Session(clsManage.iSession.user_name.ToString).ToString)
                    If result > 0 Then
                        'Dim price As New Hashtable
                        'price.Add("buy,99,1", txt99Ask1.Text)
                        'price.Add("buy,99,2", txt99Ask2.Text)
                        'price.Add("buy,99,3", txt99Ask3.Text)

                        'price.Add("sell,99,1", txt99Bid1.Text)
                        'price.Add("sell,99,2", txt99Bid2.Text)
                        'price.Add("sell,99,3", txt99Bid3.Text)

                        'price.Add("buy,96,1", txt96Ask1.Text)
                        'price.Add("buy,96,2", txt96Ask2.Text)
                        'price.Add("buy,96,3", txt96Ask3.Text)

                        'price.Add("sell,96,1", txt96Bid1.Text)
                        'price.Add("sell,96,2", txt96Bid2.Text)
                        'price.Add("sell,96,3", txt96Bid3.Text)
                        'clsMain.autoAcceptByPrice2(price)


                        clsMain.autoAcceptByPrice("buy", "99", txt99Ask1.Text, "1")
                        clsMain.autoAcceptByPrice("buy", "99", txt99Ask2.Text, "2")
                        clsMain.autoAcceptByPrice("buy", "99", txt99Ask3.Text, "3")

                        clsMain.autoAcceptByPrice("sell", "99", txt99Bid1.Text, "1")
                        clsMain.autoAcceptByPrice("sell", "99", txt99Bid2.Text, "2")
                        clsMain.autoAcceptByPrice("sell", "99", txt99Bid3.Text, "3")


                        clsMain.autoAcceptByPrice("buy", "96", txt96Ask1.Text, "1")
                        clsMain.autoAcceptByPrice("buy", "96", txt96Ask2.Text, "2")
                        clsMain.autoAcceptByPrice("buy", "96", txt96Ask3.Text, "3")

                        clsMain.autoAcceptByPrice("sell", "96", txt96Bid1.Text, "1")
                        clsMain.autoAcceptByPrice("sell", "96", txt96Bid2.Text, "2")
                        clsMain.autoAcceptByPrice("sell", "96", txt96Bid3.Text, "3")
                        Return True
                    End If
                End If
                Return False
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Sub getData()
        Try

            Dim dt As New Data.DataTable
            dt = clsMain.getPriceOnline()
            Dim dr As Data.DataRow = dt.Rows(0)
            hdfPwd.Value = dr("pwd_auth").ToString

            'set Price to Dictinary for check current price
            htPrice = New Hashtable
            htPrice.Add("bid99_1", clsManage.convert2zero(dr("bid99_1")))
            htPrice.Add("bid99_2", clsManage.convert2zero(dr("bid99_2")))
            htPrice.Add("bid99_3", clsManage.convert2zero(dr("bid99_3")))

            htPrice.Add("ask99_1", clsManage.convert2zero(dr("ask99_1")))
            htPrice.Add("ask99_2", clsManage.convert2zero(dr("ask99_2")))
            htPrice.Add("ask99_3", clsManage.convert2zero(dr("ask99_3")))

            htPrice.Add("bid96_1", clsManage.convert2zero(dr("bid96_1")))
            htPrice.Add("bid96_2", clsManage.convert2zero(dr("bid96_2")))
            htPrice.Add("bid96_3", clsManage.convert2zero(dr("bid96_3")))

            htPrice.Add("ask96_1", clsManage.convert2zero(dr("ask96_1")))
            htPrice.Add("ask96_2", clsManage.convert2zero(dr("ask96_2")))
            htPrice.Add("ask96_3", clsManage.convert2zero(dr("ask96_3")))
            ViewState("price") = htPrice

            '99

            hdfBid99.Value = dr("bid99_1").ToString ' for max %
            txt99Bid1.Text = clsManage.convert2Currency(dr("bid99_1").ToString)
            txt99Ask1.Text = clsManage.convert2Currency(dr("ask99_1").ToString)
            txt99Bid2.Text = dr("bid99_2").ToString
            txt99Ask2.Text = dr("ask99_2").ToString
            txt99Bid3.Text = dr("bid99_3").ToString
            txt99Ask3.Text = dr("ask99_3").ToString

            ViewState("bid") = clsManage.convert2zero(dr("bid99_1").ToString)
            ViewState("ask") = clsManage.convert2zero(dr("ask99_1").ToString)

            '96

            txt96Bid1.Text = clsManage.convert2Currency(dr("bid96_1").ToString)
            txt96Ask1.Text = clsManage.convert2Currency(dr("ask96_1").ToString)
            txt96Bid2.Text = dr("bid96_2").ToString
            txt96Ask2.Text = dr("ask96_2").ToString
            txt96Bid3.Text = dr("bid96_3").ToString
            txt96Ask3.Text = dr("ask96_3").ToString

            txtBidMin.Text = clsManage.convert2Currency(dr("min").ToString)
            txtBidMax.Text = clsManage.convert2Currency(dr("max").ToString)
            txtMaxBaht.Text = clsManage.convert2Currency(dr("max_baht").ToString)
            txtMaxKg.Text = clsManage.convert2Currency(dr("max_kg").ToString)

            ViewState("min_ask9699") = dr("min_ask_dif_9699")
            ViewState("max_ask9699") = dr("max_ask_dif_9699")
            ViewState("range_leave") = dr("range_leave_order")

            txtMaxPer.Text = dr("max_per").ToString

            TimeTxtBox.Text = dr("time").ToString
            txtLeaveTimeout.Text = dr("order_timeout").ToString

            'set Summary
            dt = Nothing
            dt = clsMain.getTradeSummaryAcceptManagePrice()
            'lblNet96.Text = clsManage.convert2Quantity(clsManage.convert2zero(dt.Rows(0)("net96").ToString))
            'lblNet99.Text = clsManage.convert2Quantity(clsManage.convert2zero(dt.Rows(0)("net99").ToString))

            '####set for calulate
            'diff 96 - 99 >>> change requirment from bid99 >> ask99
            If txt99Ask1.Text <> "" And txt96Ask1.Text <> "" Then
                txtDif9699.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Ask1.Text) - clsManage.convert2zero(txt96Ask1.Text))
            End If
            'diff Bid-ask99
            If txt99Bid1.Text <> "" And txt99Ask1.Text <> "" Then
                txt99DifBidAsk.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Ask1.Text) - clsManage.convert2zero(txt99Bid1.Text))
            End If
            'diff Bid-ask96
            If txt96Bid1.Text <> "" And txt96Ask1.Text <> "" Then
                txt96DifBidAsk.Text = clsManage.convert2Currency(clsManage.convert2zero(txt96Ask1.Text) - clsManage.convert2zero(txt96Bid1.Text))
            End If

            '99 Bid lv2
            If txt99Bid2.Text <> "" And txt99Bid1.Text <> "" Then
                txt99BidDif2.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Bid1.Text) - clsManage.convert2zero(txt99Bid2.Text))
            End If
            '99 Ask lv2'
            If txt99Ask2.Text <> "" And txt99Ask1.Text <> "" Then
                txt99AskDif2.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Ask2.Text) - clsManage.convert2zero(txt99Ask1.Text))
            End If
            '99 Bid lv3
            If txt99Bid3.Text <> "" And txt99Bid1.Text <> "" Then
                txt99BidDif3.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Bid1.Text) - clsManage.convert2zero(txt99Bid3.Text))
            End If
            '99 Ask lv3
            If txt99Ask3.Text <> "" And txt99Ask1.Text <> "" Then
                txt99AskDif3.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Ask3.Text) - clsManage.convert2zero(txt99Ask1.Text))
            End If

            '96 Bid lv2
            If txt96Bid2.Text <> "" And txt96Bid1.Text <> "" Then
                txt96BidDif2.Text = clsManage.convert2Currency(clsManage.convert2zero(txt96Bid1.Text) - clsManage.convert2zero(txt96Bid2.Text))
            End If
            '96 Ask lv2
            If txt96Ask2.Text <> "" And txt96Ask1.Text <> "" Then
                txt96AskDif2.Text = clsManage.convert2Currency(clsManage.convert2zero(txt96Ask2.Text) - clsManage.convert2zero(txt96Ask1.Text))
            End If
            '96 Bid lv3
            If txt96Bid3.Text <> "" And txt96Bid1.Text <> "" Then
                txt96BidDif3.Text = clsManage.convert2Currency(clsManage.convert2zero(txt96Bid1.Text) - clsManage.convert2zero(txt96Bid3.Text))
            End If
            '96 Ask lv3
            If txt96Ask3.Text <> "" And txt96Ask1.Text <> "" Then
                txt96AskDif3.Text = clsManage.convert2Currency(clsManage.convert2zero(txt96Ask3.Text) - clsManage.convert2zero(txt96Ask1.Text))
            End If
            'Halt
            updateHalt(dr("halt").ToString)
            dt.Dispose()
        Catch ex As Exception
            Throw ex
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

    Protected Sub editbt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles editbt.Click
        Try
            Threading.Thread.Sleep(1000)
            If refresh() Then
                If saveData() Then
                    getData()
                    'refresh for check price (cheange color row)
                    clsManage.Script(Page, "top.window.document.getElementById('ifmTime').src='timer_price.aspx';top.window.document.getElementById('ctl00_ContentPlaceHolder1_btnReset').click();", "reset_time")
                End If
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "updateError")
        End Try
    End Sub

    Private Function refresh() As Boolean
        Try
            If txt99Bid1.Text.Trim = "" And txt99Ask1.Text.Trim = "" Then
                clsManage.alert(Page, "Please Enter Bid or Ask for Calculate Price.", "", "", "check")
                Return False
            End If

            If TimeTxtBox.Text = "" Then
                clsManage.alert(Page, "Please Enter Inactive Time.", TimeTxtBox.ClientID, "", "check")
                Return False
            End If

            If txtMaxBaht.Text = "" Then
                clsManage.alert(Page, "Please Enter Max BAHT.", txtMaxBaht.ClientID, "", "check")
                Return False
            End If

            If txtMaxKg.Text = "" Then
                clsManage.alert(Page, "Please Enter Max KG.", txtMaxKg.ClientID, "", "check")
                Return False
            End If

            If txtLeaveTimeout.Text = "" Then
                clsManage.alert(Page, "Please Enter Leave Order Time Out.", txtLeaveTimeout.ClientID, "", "check")
                Return False
            End If

            If txt99Bid1.Text <> "" And Not IsNumeric(txt99Bid1.Text) Then
                clsManage.alert(Page, "Please Enter Only Numbers.", txt99Bid1.ClientID, "", "check")
                Return False
            End If

            If txt99Ask1.Text <> "" And Not IsNumeric(txt99Ask1.Text) Then
                clsManage.alert(Page, "Please Enter Only Numbers.", txt99Ask1.ClientID, "", "check")
                Return False
            End If

            If Not IsNumeric(TimeTxtBox.Text) Then
                clsManage.alert(Page, "Please Enter Only Numbers.", TimeTxtBox.ClientID, "", "check")
                Return False
            End If

            If Not IsNumeric(txtMaxBaht.Text) Then
                clsManage.alert(Page, "Please Enter Only Numbers.", txtMaxBaht.ClientID, "", "check")
                Return False
            End If

            If Not IsNumeric(txtMaxKg.Text) Then
                clsManage.alert(Page, "Please Enter Only Numbers.", txtMaxKg.ClientID, "", "check")
                Return False
            End If

            If Not IsNumeric(txtLeaveTimeout.Text) Then
                clsManage.alert(Page, "Please Enter Only Numbers.", txtLeaveTimeout.ClientID, "", "check")
                Return False
            End If

            'cal Min - max ask dif 96-99
            If clsManage.convert2zero(txtDif9699.Text) > clsManage.convert2zero(ViewState("max_ask9699").ToString) Then
                clsManage.alert(Page, "Ask99.99-Ask96.50 not in Min-Max.", txtDif9699.ClientID, "", "limit") : Return False
            End If
            If clsManage.convert2zero(txtDif9699.Text) < clsManage.convert2zero(ViewState("min_ask9699").ToString) Then
                clsManage.alert(Page, "Ask99.99-Ask96.50 not in Min-Max.", txtDif9699.ClientID, "", "limit") : Return False
            End If


            Dim caseType As Integer ' 0:no,1:bid,2:ask
            If txt99Bid1.Text <> "" And txt99Ask1.Text = "" Then
                caseType = 1
            ElseIf txt99Bid1.Text = "" And txt99Ask1.Text <> "" Then
                caseType = 2
            ElseIf clsManage.convert2zero(ViewState("bid").ToString) <> clsManage.convert2zero(txt99Bid1.Text) Then
                caseType = 1
            ElseIf clsManage.convert2zero(ViewState("ask").ToString) <> clsManage.convert2zero(txt99Ask1.Text) Then
                caseType = 2
            End If

            If caseType = 1 Then
                If clsManage.convert2zero(txt99Bid1.Text) > clsManage.convert2zero(txtBidMax.Text) Then
                    clsManage.alert(Page, "Bid Level 1 not in Min-Max.", txt99Bid1.ClientID, "", "limit") : Return False
                End If
                If clsManage.convert2zero(txt99Bid1.Text) < clsManage.convert2zero(txtBidMin.Text) Then
                    clsManage.alert(Page, "Bid Level 1 not in Min-Max.", txt99Bid1.ClientID, "", "limit") : Return False
                End If
            End If
            If caseType = 2 Then
                'calculate when plus
                If clsManage.convert2zero(txt99Ask1.Text) - clsManage.convert2zero(txt99DifBidAsk.Text) > clsManage.convert2zero(txtBidMax.Text) Then
                    clsManage.alert(Page, "Bid Level 1 not in Min-Max.", , , "limit") : Return False
                End If
                If clsManage.convert2zero(txt99Ask1.Text) - clsManage.convert2zero(txt99DifBidAsk.Text) < clsManage.convert2zero(txtBidMin.Text) Then
                    clsManage.alert(Page, "Bid Level 1 not in Min-Max.", , , "limit") : Return False
                End If
            End If

            If caseType = 1 Then
                'แก้ที่ bid
                txt99Ask1.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Bid1.Text) + clsManage.convert2zero(txt99DifBidAsk.Text))
            ElseIf caseType = 2 Then
                'แก้ที่ ask
                txt99Bid1.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Ask1.Text) - clsManage.convert2zero(txt99DifBidAsk.Text))
            Else
                'แก้ที่ diff ต่างๆ แก้ที่ bid เป็นหลัก
                txt99Ask1.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Bid1.Text) + clsManage.convert2zero(txt99DifBidAsk.Text))
            End If

            txt96Ask1.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Ask1.Text) - clsManage.convert2zero(txtDif9699.Text))
            txt96Bid1.Text = clsManage.convert2Currency(clsManage.convert2zero(txt96Ask1.Text) - clsManage.convert2zero(txt96DifBidAsk.Text))

            ViewState("bid") = clsManage.convert2zero(txt99Bid1.Text)
            ViewState("ask") = clsManage.convert2zero(txt99Ask1.Text)

            '99 Bid Lv2
            If txt99Bid1.Text <> "" And txt99BidDif2.Text <> "" Then
                txt99Bid2.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Bid1.Text) - clsManage.convert2zero(txt99BidDif2.Text))
            End If
            '99 Ask Lv2
            If txt99Ask1.Text <> "" And txt99AskDif2.Text <> "" Then
                txt99Ask2.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Ask1.Text) + clsManage.convert2zero(txt99AskDif2.Text))
            End If

            '99 Bid Lv3
            If txt99Bid1.Text <> "" And txt99BidDif3.Text <> "" Then
                txt99Bid3.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Bid1.Text) - clsManage.convert2zero(txt99BidDif3.Text))
            End If
            '99 Ask Lv3
            If txt99Ask1.Text <> "" And txt99AskDif3.Text <> "" Then
                txt99Ask3.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Ask1.Text) + clsManage.convert2zero(txt99AskDif3.Text))
            End If

            '96 Bid Lv2
            If txt96Bid1.Text <> "" And txt96BidDif2.Text <> "" Then
                txt96Bid2.Text = clsManage.convert2Currency(clsManage.convert2zero(txt96Bid1.Text) - clsManage.convert2zero(txt96BidDif2.Text))
            End If
            '96 Ask Lv2
            If txt96Ask1.Text <> "" And txt96AskDif2.Text <> "" Then
                txt96Ask2.Text = clsManage.convert2Currency(clsManage.convert2zero(txt96Ask1.Text) + clsManage.convert2zero(txt96AskDif2.Text))
            End If

            '96 Bid Lv3
            If txt96Bid1.Text <> "" And txt96BidDif3.Text <> "" Then
                txt96Bid3.Text = clsManage.convert2Currency(clsManage.convert2zero(txt96Bid1.Text) - clsManage.convert2zero(txt96BidDif3.Text))
            End If
            '96 Ask Lv3
            If txt96Ask1.Text <> "" And txt96AskDif3.Text <> "" Then
                txt96Ask3.Text = clsManage.convert2Currency(clsManage.convert2zero(txt96Ask1.Text) + clsManage.convert2zero(txt96AskDif3.Text))
            End If
            Return True

        Catch ex As Exception
            clsManage.alert(Page, "Error Please Try Again.", "", "", "error")
            Return False
        End Try
    End Function

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            Threading.Thread.Sleep(1000)
            getData()
            txtBidChangeNumber1.Text = "5"
            txtBidChangeNumber2.Text = "10"
            txtBidChangeNumber3.Text = "20"
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "error")
        End Try
    End Sub

    Protected Sub btnRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        Threading.Thread.Sleep(1000)
        refresh()
    End Sub

    Protected Sub btnHalt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnHalt.Click
        Try
            Threading.Thread.Sleep(1000)
            Dim halt As String = ""
            If btnHalt.CommandArgument = "n" Then
                halt = "y"
            Else
                halt = "n"
            End If
            Dim result As String = clsMain.updateStockHalt(halt)
            If result <> "" Then
                updateHalt(result)
            End If

        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "err")
        End Try
    End Sub

#End Region

#Region "change Bid"
    Private Function checkBidLimit(ByVal type As String, ByVal num As String) As Boolean
        If txt99Bid1.Text.Trim = "" Or txt99Ask1.Text.Trim = "" Then
            clsManage.alert(Page, "Please Enter Bid or Ask.", , , "checkEnter")
            Return False
        End If

        If type = "down" Then
            If (clsManage.convert2zero(txt99Bid1.Text) - clsManage.convert2zero(num)) < clsManage.convert2zero(txtBidMin.Text) Then
                clsManage.alert(Page, "Bid Level 1 not in Min-Max.", , , "down")
                Return False
            End If
            If (clsManage.convert2zero(txt99Bid1.Text) - clsManage.convert2zero(num)) > clsManage.convert2zero(txtBidMax.Text) Then
                clsManage.alert(Page, "Bid Level 1 not in Min-Max.", , , "down")
                Return False
            End If
        Else
            If (clsManage.convert2zero(txt99Bid1.Text) + clsManage.convert2zero(num)) < clsManage.convert2zero(txtBidMin.Text) Then
                clsManage.alert(Page, "Bid Level 1 not in Min-Max.", , , "up")
                Return False
            End If
            If (clsManage.convert2zero(txt99Bid1.Text) + clsManage.convert2zero(num)) > clsManage.convert2zero(txtBidMax.Text) Then
                clsManage.alert(Page, "Bid Level 1 not in Min-Max.", , , "up")
                Return False
            End If
        End If
        Return True
    End Function

    Private Sub upBid(ByVal txt As TextBox)
        Try
            If txt.Text.Trim = "" Then clsManage.alert(Page, "Please Enter Number.", , , "number") : Exit Sub
            Dim num = txt.Text.Trim
            If Not checkBidLimit("up", num) Then Exit Sub
            If txt99Bid1.Text <> "" And txt99Ask1.Text <> "" Then
                txt99Bid1.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Bid1.Text) + clsManage.convert2zero(num))
                txt99Ask1.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Ask1.Text) + clsManage.convert2zero(num))
                refresh()
                clsManage.Script(Page, "colorKeyup()", "button")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Private Sub DownBid(ByVal txt As TextBox)
        Try
            If txt.Text.Trim = "" Then clsManage.alert(Page, "Please Enter Number.", , , "number") : Exit Sub
            Dim num = txt.Text.Trim

            If Not checkBidLimit("down", num) Then Exit Sub
            If txt99Bid1.Text <> "" And txt99Ask1.Text <> "" Then
                txt99Bid1.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Bid1.Text) - clsManage.convert2zero(num))
                txt99Ask1.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Ask1.Text) - clsManage.convert2zero(num))
                refresh()
                clsManage.Script(Page, "colorKeyup()", "button")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub imgUp1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgUp1.Click
        upBid(txtBidChangeNumber1)
    End Sub

    Protected Sub imgUp2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgUp2.Click
        upBid(txtBidChangeNumber2)
    End Sub

    Protected Sub imgUp3_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgUp3.Click
        upBid(txtBidChangeNumber3)
    End Sub

    Protected Sub imgDown1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgDown1.Click
        DownBid(txtBidChangeNumber1)
    End Sub

    Protected Sub imgDown2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgDown2.Click
        DownBid(txtBidChangeNumber2)
    End Sub

    Protected Sub imgDown3_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgDown3.Click
        DownBid(txtBidChangeNumber3)
    End Sub

#End Region


End Class
