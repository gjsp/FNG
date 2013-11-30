
Partial Class admin_manage_price
    Inherits basePageTrade

    Protected Sub Page_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        If Request.QueryString("m") IsNot Nothing Then
            MasterPageFile = "~/trade/admin/MasterPageBlank.master"
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            If Request.QueryString("m") IsNot Nothing Then
                pnLv1.Visible = False
                pnLv2.Visible = False
                pnLv3.Visible = False
                pnLv4.Visible = False
            End If

            TimeTxtBox.Attributes.Add("onkeypress", "checkNumber();")
            txt99DifBidAsk.Attributes.Add("onkeypress", "checkNumber();")
            txtDif9699.Attributes.Add("onkeypress", "checkNumber();")
            txt99Bid1.Attributes.Add("onkeypress", "checkNumber();")
            txt99Ask1.Attributes.Add("onkeypress", "checkNumber();")

            txt99Bid2.Attributes.Add("onkeypress", "checkNumber();")
            txt99Ask2.Attributes.Add("onkeypress", "checkNumber();")
            txt99Bid3.Attributes.Add("onkeypress", "checkNumber();")
            txt99Ask3.Attributes.Add("onkeypress", "checkNumber();")
            txt99BidDif2.Attributes.Add("onkeypress", "checkNumber();")
            txt99BidDif3.Attributes.Add("onkeypress", "checkNumber();")
            txt99AskDif2.Attributes.Add("onkeypress", "checkNumber();")
            txt99AskDif3.Attributes.Add("onkeypress", "checkNumber();")

            txt96DifBidAsk.Attributes.Add("onkeypress", "checkNumber();")
            txt96Bid1.Attributes.Add("onkeypress", "checkNumber();")
            txt96Ask1.Attributes.Add("onkeypress", "checkNumber();")
            txt96Bid2.Attributes.Add("onkeypress", "checkNumber();")
            txt96Ask2.Attributes.Add("onkeypress", "checkNumber();")
            txt96Bid3.Attributes.Add("onkeypress", "checkNumber();")
            txt96Ask3.Attributes.Add("onkeypress", "checkNumber();")
            txt96BidDif2.Attributes.Add("onkeypress", "checkNumber();")
            txt96BidDif3.Attributes.Add("onkeypress", "checkNumber();")
            txt96AskDif2.Attributes.Add("onkeypress", "checkNumber();")
            txt96AskDif3.Attributes.Add("onkeypress", "checkNumber();")

            txt99Bid2.ReadOnly = True
            txt99Bid3.ReadOnly = True
            txt99Ask2.ReadOnly = True
            txt99Ask3.ReadOnly = True

            txt96Bid1.ReadOnly = True
            txt96Ask1.ReadOnly = True
            txt96Bid2.ReadOnly = True
            txt96Bid3.ReadOnly = True
            txt96Ask2.ReadOnly = True
            txt96Ask3.ReadOnly = True

            editbt.Attributes.Add("onclick", "return setConfirmSave('Do you want to Save?',this);")

            txtBidChangeNumber1.Text = "5" : txtBidChangeNumber2.Text = "10" : txtBidChangeNumber3.Text = "20"
            txtBidChangeNumber1.Attributes.Add("onkeypress", "checkNumber();") : txtBidChangeNumber2.Attributes.Add("onkeypress", "checkNumber();") : txtBidChangeNumber3.Attributes.Add("onkeypress", "checkNumber();")

            imgUp1.Attributes.Add("onclick", "changeBid('this');") : imgUp2.Attributes.Add("onclick", "changeBid('this');") : imgUp3.Attributes.Add("onclick", "changeBid('this');")
            imgDown1.Attributes.Add("onclick", "changeBid('this');") : imgDown2.Attributes.Add("onclick", "changeBid('this');") : imgDown3.Attributes.Add("onclick", "changeBid('this');")

            txtBidMin.Attributes.Add("onkeypress", "checkNumber();")
            txtBidMax.Attributes.Add("onkeypress", "checkNumber();")

            txtMinAsk9996.Attributes.Add("onkeypress", "checkNumber();")
            txtMaxAsk9996.Attributes.Add("onkeypress", "checkNumber();")

            txtMaxPer.Attributes.Add("onkeypress", "checkNumber();")
            txtMaxBaht.Attributes.Add("onkeypress", "checkNumber();")
            txtMaxKg.Attributes.Add("onkeypress", "checkNumber();")
            txtLeaveTimeout.Attributes.Add("onkeypress", "checkNumber();")

            Dim script As String = ""
            Dim stock = New dcDBDataContext().getStockOnline().Single()
            If stock.self_price = "y" Then
                ddlSwep.SelectedIndex = 1
                script = "div_price_gcap.style.display='none';div_price2.style.display='block';div_time.style.display='block'; "
            Else
                ddlSwep.SelectedIndex = 0
                script = "div_price_gcap.style.display='block';div_price2.style.display='none';div_time.style.display='none'; "
            End If
            clsManage.Script(Page, script)

            getData()
        End If
    End Sub

    Function getMinus(ByVal prices As String) As String
        Try
            If Convert.ToDouble(prices) <= 0 Then
                Return prices
            Else
                Return "-" + prices
            End If
        Catch ex As Exception
            Return prices
        End Try
        Return prices
    End Function

    Sub getData()
        Try

            Dim dt As New Data.DataTable
            dt = clsMain.getPriceOnline()
            Dim dr As Data.DataRow = dt.Rows(0)
            Dim minus As String = "-"
            hdfPwd.Value = dr("pwd_auth").ToString

            'set bid ask
            ViewState("bid") = clsManage.convert2zero(dr("bid99_1").ToString)
            ViewState("ask") = clsManage.convert2zero(dr("ask99_1").ToString)
            hdfBid99.Value = dr("bid99_1").ToString

            '99
            lbl99Bid1.Text = clsManage.convert2Currency(dr("bid99_1").ToString)
            lbl99Ask1.Text = clsManage.convert2Currency(dr("ask99_1").ToString)
            lbl99Bid2.Text = clsManage.convert2Currency(dr("bid99_2").ToString)
            lbl99Ask2.Text = clsManage.convert2Currency(dr("ask99_2").ToString)
            lbl99Bid3.Text = clsManage.convert2Currency(dr("bid99_3").ToString)
            lbl99Ask3.Text = clsManage.convert2Currency(dr("ask99_3").ToString)

            txt99Bid1.Text = clsManage.convert2Currency(dr("bid99_1").ToString)
            txt99Ask1.Text = clsManage.convert2Currency(dr("ask99_1").ToString)
            txt99Bid2.Text = clsManage.convert2Currency(dr("bid99_2").ToString)
            txt99Ask2.Text = clsManage.convert2Currency(dr("ask99_2").ToString)
            txt99Bid3.Text = clsManage.convert2Currency(dr("bid99_3").ToString)
            txt99Ask3.Text = clsManage.convert2Currency(dr("ask99_3").ToString)

            '96
            lbl96Bid1.Text = clsManage.convert2Currency(dr("bid96_1").ToString)
            lbl96Ask1.Text = clsManage.convert2Currency(dr("ask96_1").ToString)
            lbl96Bid2.Text = clsManage.convert2Currency(dr("bid96_2").ToString)
            lbl96Ask2.Text = clsManage.convert2Currency(dr("ask96_2").ToString)
            lbl96Bid3.Text = clsManage.convert2Currency(dr("bid96_3").ToString)
            lbl96Ask3.Text = clsManage.convert2Currency(dr("ask96_3").ToString)

            txt96Bid1.Text = clsManage.convert2Currency(dr("bid96_1").ToString)
            txt96Ask1.Text = clsManage.convert2Currency(dr("ask96_1").ToString)
            txt96Bid2.Text = clsManage.convert2Currency(dr("bid96_2").ToString)
            txt96Ask2.Text = clsManage.convert2Currency(dr("ask96_2").ToString)
            txt96Bid3.Text = clsManage.convert2Currency(dr("bid96_3").ToString)
            txt96Ask3.Text = clsManage.convert2Currency(dr("ask96_3").ToString)

            txtBidMin.Text = clsManage.convert2Currency(dr("min").ToString)
            txtBidMax.Text = clsManage.convert2Currency(dr("max").ToString)
            txtMaxBaht.Text = clsManage.convert2Currency(dr("max_baht").ToString)
            txtMaxKg.Text = clsManage.convert2Currency(dr("max_kg").ToString)

            txtMinAsk9996.Text = clsManage.convert2Currency(dr("min_ask_dif_9699").ToString)
            txtMaxAsk9996.Text = clsManage.convert2Currency(dr("max_ask_dif_9699").ToString)

            txtRangeLeave.Text = dr("range_leave_order").ToString
            txtLeaveTimeout.Text = dr("order_timeout").ToString
            txtMaxPer.Text = dr("max_per").ToString
            TimeTxtBox.Text = dr("time").ToString

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
                txt99BidDif2.Text = getMinus(clsManage.convert2Currency(clsManage.convert2zero(txt99Bid1.Text) - clsManage.convert2zero(txt99Bid2.Text)))
            End If
            '99 Ask lv2'
            If txt99Ask2.Text <> "" And txt99Ask1.Text <> "" Then
                txt99AskDif2.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Ask2.Text) - clsManage.convert2zero(txt99Ask1.Text))
            End If
            '99 Bid lv3
            If txt99Bid3.Text <> "" And txt99Bid1.Text <> "" Then
                txt99BidDif3.Text = getMinus(clsManage.convert2Currency(clsManage.convert2zero(txt99Bid1.Text) - clsManage.convert2zero(txt99Bid3.Text)))
            End If
            '99 Ask lv3
            If txt99Ask3.Text <> "" And txt99Ask1.Text <> "" Then
                txt99AskDif3.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Ask3.Text) - clsManage.convert2zero(txt99Ask1.Text))
            End If

            '96 Bid lv2
            If txt96Bid2.Text <> "" And txt96Bid1.Text <> "" Then
                txt96BidDif2.Text = getMinus(clsManage.convert2Currency(clsManage.convert2zero(txt96Bid1.Text) - clsManage.convert2zero(txt96Bid2.Text)))
            End If
            '96 Ask lv2
            If txt96Ask2.Text <> "" And txt96Ask1.Text <> "" Then
                txt96AskDif2.Text = clsManage.convert2Currency(clsManage.convert2zero(txt96Ask2.Text) - clsManage.convert2zero(txt96Ask1.Text))
            End If
            '96 Bid lv3
            If txt96Bid3.Text <> "" And txt96Bid1.Text <> "" Then
                txt96BidDif3.Text = getMinus(clsManage.convert2Currency(clsManage.convert2zero(txt96Bid1.Text) - clsManage.convert2zero(txt96Bid3.Text)))
            End If
            '96 Ask lv3
            If txt96Ask3.Text <> "" And txt96Ask1.Text <> "" Then
                txt96AskDif3.Text = clsManage.convert2Currency(clsManage.convert2zero(txt96Ask3.Text) - clsManage.convert2zero(txt96Ask1.Text))
            End If
            clsManage.Script(Page, "document.getElementById('ifmTime').src='timer_price.aspx';", "reset_time")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Function saveData() As Boolean
        Try
            If txt99Bid1.Text = "" Or txt99Ask1.Text = "" Or txt99Bid2.Text = "" Or txt99Ask2.Text = "" Or txt99Bid1.Text = "" Or txt99Ask1.Text = "" Or txt99Bid3.Text = "" Or txt99Ask3.Text = "" _
            Or txt96Bid1.Text = "" Or txt96Ask1.Text = "" Or txt96Bid2.Text = "" Or txt96Ask2.Text = "" Or txt96Bid1.Text = "" Or txt96Ask1.Text = "" Or txt96Bid3.Text = "" Or txt96Ask3.Text = "" _
            Or TimeTxtBox.Text = "" Or txtLeaveTimeout.Text = "" Or txtMaxBaht.Text = "" Or txtMaxKg.Text = "" Or txtDif9699.Text = "" Or txt96DifBidAsk.Text = "" Or txt99DifBidAsk.Text = "" _
            Or txtMinAsk9996.Text = "" Or txtMaxAsk9996.Text = "" Or txtRangeLeave.Text = "" Or txtBidMin.Text = "" Or txtBidMax.Text = "" Or txtMaxPer.Text = "" Then
                clsManage.alert(Page, clsManage.msgRequiredFill)
            Else
                If clsManage.convert2zero(txtLeaveTimeout.Text) >= clsManage.convert2zero(TimeTxtBox.Text) Then
                    clsManage.alert(Page, "Please Enter Timeout Less than Inactive Pricing Time.", txtLeaveTimeout.ClientID, , "timeLess")
                Else
                    Dim result As Integer = clsMain.UpdatePriceOnline(txt99Bid1.Text, txt99Ask1.Text, txt99Bid2.Text, txt99Ask2.Text, txt99Bid3.Text, txt99Ask3.Text, _
                                              txt96Bid1.Text, txt96Ask1.Text, txt96Bid2.Text, txt96Ask2.Text, txt96Bid3.Text, txt96Ask3.Text, _
                                              TimeTxtBox.Text, txtBidMin.Text, txtBidMax.Text, txtMaxBaht.Text, txtMaxKg.Text, txtMinAsk9996.Text, txtMaxAsk9996.Text, txtLeaveTimeout.Text, txtMaxPer.Text, txtRangeLeave.Text, "y", Session(clsManage.iSession.user_name.ToString).ToString)
                    If result > 0 Then
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

    Protected Sub editbt_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles editbt.Click
        Try
            If refresh() Then
                If saveData() Then
                    getData()
                    clsManage.Script(Page, "top.window.ifmTran_setting.document.getElementById('btnReset').click();", "reset_price")
                End If

            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub


    Private Function checkBidLimit(ByVal type As String, ByVal num As String) As Boolean
        If type = "down" Then
            If (clsManage.convert2zero(txt99Bid1.Text) - clsManage.convert2zero(num)) < clsManage.convert2zero(txtBidMin.Text) Then
                clsManage.alert(Page, "Bid Level 1 not in Min-Max.")
                Return False
            End If
            If (clsManage.convert2zero(txt99Bid1.Text) - clsManage.convert2zero(num)) > clsManage.convert2zero(txtBidMax.Text) Then
                clsManage.alert(Page, "Bid Level 1 not in Min-Max.")
                Return False
            End If
        Else
            If (clsManage.convert2zero(txt99Bid1.Text) + clsManage.convert2zero(num)) < clsManage.convert2zero(txtBidMin.Text) Then
                clsManage.alert(Page, "Bid Level 1 not in Min-Max.")
                Return False
            End If
            If (clsManage.convert2zero(txt99Bid1.Text) + clsManage.convert2zero(num)) > clsManage.convert2zero(txtBidMax.Text) Then
                clsManage.alert(Page, "Bid Level 1 not in Min-Max.")
                Return False
            End If
        End If
        Return True
    End Function

    Protected Sub btnRefresh_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRefresh.Click
        refresh()
    End Sub

    Function refresh() As Boolean
        Try
            Threading.Thread.Sleep(1000)

            If txt99Bid1.Text.Trim = "" And txt99Ask1.Text.Trim = "" Then
                clsManage.alert(Page, "Please Enter Bid or Ask for Calculate Price.")
                Return False
            End If

            If txt96BidDif2.Text.Trim = "" Then clsManage.alert(Page, "Please Enter Bid Difference Level 2.", txt96BidDif2.ClientID, , "emp") : Return False
            If txt96AskDif2.Text.Trim = "" Then clsManage.alert(Page, "Please Enter Ask Difference Level 2.", txt96AskDif2.ClientID, , "emp") : Return False
            If txt99BidDif2.Text.Trim = "" Then clsManage.alert(Page, "Please Enter Bid Difference Level 2.", txt99BidDif2.ClientID, , "emp") : Return False
            If txt99AskDif2.Text.Trim = "" Then clsManage.alert(Page, "Please Enter Ask Difference Level 2.", txt99AskDif2.ClientID, , "emp") : Return False

            If txt96BidDif3.Text.Trim = "" Then clsManage.alert(Page, "Please Enter Bid Difference Level 3.", txt96BidDif3.ClientID, , "emp") : Return False
            If txt96AskDif3.Text.Trim = "" Then clsManage.alert(Page, "Please Enter Ask Difference Level 3.", txt96AskDif3.ClientID, , "emp") : Return False
            If txt99BidDif3.Text.Trim = "" Then clsManage.alert(Page, "Please Enter Bid Difference Level 3.", txt99BidDif3.ClientID, , "emp") : Return False
            If txt99AskDif3.Text.Trim = "" Then clsManage.alert(Page, "Please Enter Ask Difference Level 3.", txt99AskDif3.ClientID, , "emp") : Return False

            If clsManage.convert2zero(txt96BidDif2.Text) > 0 Then clsManage.alert(Page, "Please Enter Bid Difference Level 2 Less than 0.", txt96BidDif2.ClientID, , "emp") : Return False
            If clsManage.convert2zero(txt96AskDif2.Text) < 0 Then clsManage.alert(Page, "Please Enter Ask Difference Level 2 More than 0.", txt96AskDif2.ClientID, , "emp") : Return False
            If clsManage.convert2zero(txt99BidDif2.Text) > 0 Then clsManage.alert(Page, "Please Enter Bid Difference Level 2 Less than 0.", txt99BidDif2.ClientID, , "emp") : Return False
            If clsManage.convert2zero(txt99AskDif2.Text) < 0 Then clsManage.alert(Page, "Please Enter Ask Difference Level 2 More than 0.", txt99AskDif2.ClientID, , "emp") : Return False

            If clsManage.convert2zero(txt96BidDif3.Text) > 0 Then clsManage.alert(Page, "Please Enter Bid Difference Level 3 Less than 0.", txt96BidDif3.ClientID, , "emp") : Return False
            If clsManage.convert2zero(txt96AskDif3.Text) < 0 Then clsManage.alert(Page, "Please Enter Ask Difference Level 3 More than 0.", txt96AskDif3.ClientID, , "emp") : Return False
            If clsManage.convert2zero(txt99BidDif3.Text) > 0 Then clsManage.alert(Page, "Please Enter Bid Difference Level 3 Less than 0.", txt99BidDif3.ClientID, , "emp") : Return False
            If clsManage.convert2zero(txt99AskDif3.Text) < 0 Then clsManage.alert(Page, "Please Enter Ask Difference Level 3 More than 0.", txt99AskDif3.ClientID, , "emp") : Return False

            'cal Min - max ask dif 96-99
            If clsManage.convert2zero(txtDif9699.Text) > clsManage.convert2zero(txtMaxAsk9996.Text) Then
                clsManage.alert(Page, "Ask99.99-Ask96.50 not in Min-Max.", txtDif9699.ClientID, "", "limit") : Return False
            End If
            If clsManage.convert2zero(txtDif9699.Text) < clsManage.convert2zero(txtMinAsk9996.Text) Then
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

            If caseType = 1 Or caseType = 0 Then
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
            'If txt99Bid1.Text <> "" Then
            '    If clsManage.convert2Currency(clsManage.convert2zero(txt99Bid1.Text)) > clsManage.convert2Currency(clsManage.convert2zero(txtBidMax.Text)) Then
            '        clsManage.alert(Page, "Bid Level 1 not in Min-Max.")
            '        Exit Sub
            '    End If
            '    If clsManage.convert2Currency(clsManage.convert2zero(txt99Bid1.Text)) < clsManage.convert2Currency(clsManage.convert2zero(txtBidMin.Text)) Then
            '        clsManage.alert(Page, "Bid Level 1 not in Min-Max.")
            '        Exit Sub
            '    End If


            If clsManage.convert2zero(ViewState("bid").ToString) <> clsManage.convert2zero(txt99Bid1.Text) And txt99Bid1.Text.Trim <> "" Then
                'แก้ที่ bid
                txt99Ask1.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Bid1.Text) + clsManage.convert2zero(txt99DifBidAsk.Text))

            ElseIf clsManage.convert2zero(ViewState("ask").ToString) <> clsManage.convert2zero(txt99Ask1.Text) And txt99Ask1.Text.Trim <> "" Then
                'แก้ที่ ask
                txt99Bid1.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Ask1.Text) - clsManage.convert2zero(txt99DifBidAsk.Text))
            ElseIf txt99Bid1.Text.Trim = "" And txt99Ask1.Text.Trim <> "" Then
                'แก้ที่ ask
                txt99Bid1.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Ask1.Text) - clsManage.convert2zero(txt99DifBidAsk.Text))
            Else 'แก้ที่ diff ต่างๆ แก้ที่ bid เป็นหลัก
                txt99Ask1.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Bid1.Text) + clsManage.convert2zero(txt99DifBidAsk.Text))
            End If

            txt96Ask1.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Ask1.Text) - clsManage.convert2zero(txtDif9699.Text))
            txt96Bid1.Text = clsManage.convert2Currency(clsManage.convert2zero(txt96Ask1.Text) - clsManage.convert2zero(txt96DifBidAsk.Text))

            ViewState("bid") = clsManage.convert2zero(txt99Bid1.Text)
            ViewState("ask") = clsManage.convert2zero(txt99Ask1.Text)

            '99 Bid Lv2
            If txt99Bid1.Text <> "" And txt99BidDif2.Text <> "" Then
                txt99Bid2.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Bid1.Text) + clsManage.convert2zero(txt99BidDif2.Text))
            End If
            '99 Ask Lv2
            If txt99Ask1.Text <> "" And txt99AskDif2.Text <> "" Then
                txt99Ask2.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Ask1.Text) + clsManage.convert2zero(txt99AskDif2.Text))
            End If

            '99 Bid Lv3
            If txt99Bid1.Text <> "" And txt99BidDif3.Text <> "" Then
                txt99Bid3.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Bid1.Text) + clsManage.convert2zero(txt99BidDif3.Text))
            End If
            '99 Ask Lv3
            If txt99Ask1.Text <> "" And txt99AskDif3.Text <> "" Then
                txt99Ask3.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Ask1.Text) + clsManage.convert2zero(txt99AskDif3.Text))
            End If

            '96 Bid Lv2
            If txt96Bid1.Text <> "" And txt96BidDif2.Text <> "" Then
                txt96Bid2.Text = clsManage.convert2Currency(clsManage.convert2zero(txt96Bid1.Text) + clsManage.convert2zero(txt96BidDif2.Text))
            End If
            '96 Ask Lv2
            If txt96Ask1.Text <> "" And txt96AskDif2.Text <> "" Then
                txt96Ask2.Text = clsManage.convert2Currency(clsManage.convert2zero(txt96Ask1.Text) + clsManage.convert2zero(txt96AskDif2.Text))
            End If

            '96 Bid Lv3
            If txt96Bid1.Text <> "" And txt96BidDif3.Text <> "" Then
                txt96Bid3.Text = clsManage.convert2Currency(clsManage.convert2zero(txt96Bid1.Text) + clsManage.convert2zero(txt96BidDif3.Text))
            End If
            '96 Ask Lv3
            If txt96Ask1.Text <> "" And txt96AskDif3.Text <> "" Then
                txt96Ask3.Text = clsManage.convert2Currency(clsManage.convert2zero(txt96Ask1.Text) + clsManage.convert2zero(txt96AskDif3.Text))
            End If

            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

#Region "change Bid"
    Private Sub upBid(ByVal num As String)
        Try
            If Not checkBidLimit("up", num) Then Exit Sub
            If txt99Bid1.Text <> "" And txt99Ask1.Text <> "" Then
                txt99Bid1.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Bid1.Text) + clsManage.convert2zero(num))
                txt99Ask1.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Ask1.Text) + clsManage.convert2zero(num))
                btnRefresh_Click(Nothing, Nothing)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Private Sub DownBid(ByVal num As String)
        Try
            If Not checkBidLimit("down", num) Then Exit Sub
            If txt99Bid1.Text <> "" Then
                txt99Bid1.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Bid1.Text) - clsManage.convert2zero(num))
                txt99Ask1.Text = clsManage.convert2Currency(clsManage.convert2zero(txt99Ask1.Text) - clsManage.convert2zero(num))
                btnRefresh_Click(Nothing, Nothing)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub imgUp1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgUp1.Click
        upBid(txtBidChangeNumber1.Text)
    End Sub

    Protected Sub imgUp2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgUp2.Click
        upBid(txtBidChangeNumber2.Text)
    End Sub

    Protected Sub imgUp3_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgUp3.Click
        upBid(txtBidChangeNumber3.Text)
    End Sub

    Protected Sub imgDown1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgDown1.Click
        DownBid(txtBidChangeNumber1.Text)
    End Sub

    Protected Sub imgDown2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgDown2.Click
        DownBid(txtBidChangeNumber2.Text)
    End Sub

    Protected Sub imgDown3_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgDown3.Click
        DownBid(txtBidChangeNumber3.Text)
    End Sub

#End Region

    Protected Sub btnReset_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnReset.Click
        Try
            Threading.Thread.Sleep(1000)
            getData()
            txtBidChangeNumber1.Text = "5" : txtBidChangeNumber2.Text = "10" : txtBidChangeNumber3.Text = "20"
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "err")
        End Try

    End Sub
End Class
