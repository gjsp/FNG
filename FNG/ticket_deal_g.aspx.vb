
Partial Class ticket_deal_g
    Inherits basePage

    Protected Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        If Not Page.IsPostBack Then
            Try
                If Session(clsManage.iSession.user_role_center.ToString) IsNot Nothing Then
                    Dim dtRole As New Data.DataTable : dtRole = Session(clsManage.iSession.user_role_center.ToString)
                    If dtRole.Rows.Find(Me.Page.Page.ToString) IsNot Nothing Then
                        Dim dr As Data.DataRow = dtRole.Rows.Find(Me.Page.Page.ToString)
                        If dr(clsDB.roles.update) = False Then
                            btnUpdate.Enabled = False
                        End If
                        If dr(clsDB.roles.add) = False Then
                            btnDealTicket.Enabled = False
                        End If
                    End If
                End If
            Catch ex As Exception
                clsManage.alert(Page, ex.Message)
            End Try
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                txtCustName.Focus()
                hdfTradeLimit.Value = "y"
                lblTitle.Text = "Tickets"
                hdfPrice99Control.Value = "500000"
                'txtBookNo.Attributes.Add("onkeypress", "return disableCtrlAltrKeyCombination(event);")
                'txtBookNo.Attributes.Add("onKeyDown", "return disableCtrlAltrKeyCombination(event);")

                btnUpdate.Visible = False : btnDealTicket.Visible = True : btnClear.Visible = True
                clsDB.getBank(ddlBank)
                txtDateNow_CalendarExtender.Format = clsManage.formatDateTime
                txtDateDelivery_CalendarExtender.Format = clsManage.formatDateTime
                txtDuedate_CalendarExtender.Format = clsManage.formatDateTime
                'change requirment
                'txtDateNow.Text = IIf(DateTime.Now.Hour > 15, DateTime.Now.AddDays(1).ToString(clsManage.formatDateTime), DateTime.Now.ToString(clsManage.formatDateTime))
                txtDateNow.Text = DateTime.Now.ToString(clsManage.formatDateTime)
                txtDateDelivery.Text = "" 'DateTime.Now.AddDays(2).ToString(clsManage.formatDateTime)

                txtDateNow.Attributes.Add("onkeypress", "return false;")
                For i = 1 To 20 : ddlClearing.Items.Add(New ListItem(i.ToString, i.ToString)) : Next
                ddlClearing.SelectedValue = "3"

                txtDateDelivery.Attributes.Add("onkeypress", "return false;")

                txtAmount.Attributes.Add("onkeypress", "return false;")
                txtQuan.Attributes.Add("onkeypress", "checkNumber();")
                txtPrice.Attributes.Add("onkeypress", "checkNumber();")

                txtCustRef.Attributes.Add("onkeypress", "if(event.keyCode==13){checkCust_id();}")
                txtCustName.Attributes.Add("onkeypress", "if(event.keyCode==13){checkCust_name();}")
                txtDuedate.Attributes.Add("onkeypress", "return false;")

                Dim scriptCal_price As String = String.Format("calculate_price('{0}','{1}','{2}');", txtQuan.ClientID, txtPrice.ClientID, txtAmount.ClientID)

                txtQuan.Attributes.Add("onblur", scriptCal_price)
                txtPrice.Attributes.Add("onblur", scriptCal_price)
                txtAmount.Attributes.Add("onblur", scriptCal_price)
                'rdoGoldType.Attributes.Add("onclick", "clearPriceQuanAmount();")
                rdoType.Attributes.Add("onchange", "changeBgColor();clearPriceQuanAmount();")

                'txtCustRef.Attributes.Add("onblur", "checkCust_id();")
                'txtCustName.Attributes.Add("onblur", "checkCust_name();")
                imgSearchCustRef.Attributes.Add("onclick", "openCustomerList('customer');return false;")

                linkPwd.Visible = False
                txtPwdAuth.Visible = False
                Dim scriptCheckAmount As String = "return checkTradeLimit();"


                btnDealTicket.Attributes.Add("onclick", scriptCheckAmount)
                btnUpdate.Attributes.Add("onclick", scriptCheckAmount)

                'For short cut Ctrl+S,B
                If Request.QueryString("type") IsNot Nothing Then
                    If Request.QueryString("type").ToString = "sell" Then
                        clsManage.Script(Page, " setOrderType('sell');", "sell")
                        rdoType.SelectedIndex = 0
                    ElseIf Request.QueryString("type").ToString = "buy" Then
                        clsManage.Script(Page, " setOrderType('buy');", "buy")
                        rdoType.SelectedIndex = 1
                    End If
                End If

                'check position  Account: only edit payment,trader:edit price
                'Account,1|Trader,2|Marketing,3|Admin,4
                Select Case Session(clsManage.iSession.user_position_center.ToString)
                    Case "1"
                        rdoType.Enabled = False
                        txtQuan.Enabled = False
                        txtPrice.Enabled = False
                        txtAmount.Enabled = False
                        rdoDelivery.Enabled = False
                        rdoBilling.Enabled = False
                        txtCustName.Enabled = False
                        imgSearchCustRef.Enabled = False
                    Case "2"
                        ddlPayment.Enabled = False
                        ddlBank.Enabled = False
                        txtCheq.Enabled = False
                        txtDuedate.Enabled = False
                        imgDuedate.Enabled = False
                End Select

                If Request.QueryString("id") IsNot Nothing Then

                    objSrcLog.SelectParameters("ref_no").DefaultValue = Request.QueryString("id").ToString
                    gvLog.EmptyDataText = clsManage.EmptyDataText
                    If Request.QueryString("id").ToString.Substring(0, 1) = clsFng.strOnline Then
                        rdoBilling.Enabled = False
                    End If

                    btnDealTicket.Visible = False : btnUpdate.Visible = True : btnClear.Visible = False
                    Dim dt As New Data.DataTable
                    dt = clsDB.getTicket(Request.QueryString("id").ToString)
                    If dt.Rows.Count > 0 Then
                        Dim dr As Data.DataRow = dt.Rows(0)
                        'If dr("trade") IsNot DBNull.Value Then btnUpdate.Enabled = False

                        lblRefNo.Text = dr("ticket_id").ToString
                        lblStatus.Text = dr("status_name").ToString
                        If dr("ticket_date") IsNot DBNull.Value Then lblDate.Text = CDate(dr("ticket_date")).ToString(clsManage.formatDateTime + " hh:mm") Else lblDate.Text = ""
                        If dr("status_id").ToString = "901" Or dr("status_id").ToString = "902" Or dr("status_id").ToString = "903" Then
                            lblStatus.ForeColor = Drawing.Color.Green
                        Else
                            lblStatus.ForeColor = Drawing.Color.Black
                        End If
                        txtCustRef.Text = dr("cust_id").ToString
                        txtCustName.Text = ""
                        clsManage.Script(Page, "checkCust_id();", "check")

                        If dr("ticket_date") IsNot DBNull.Value Then txtDateNow.Text = CDate(dr("ticket_date")).ToString(clsManage.formatDateTime) Else txtDateNow.Text = ""
                        If dr("delivery_date_null") IsNot DBNull.Value Then txtDateDelivery.Text = CDate(dr("delivery_date_null")).ToString(clsManage.formatDateTime) Else txtDateDelivery.Text = ""

                        If dr("gold_dep").ToString = "y" Then
                            cbGoldDep.Checked = True
                        End If
                        If txtCustRef.Text <> "" Then
                            btnSearch_Click(Nothing, Nothing)
                        End If

                        If dr("type").ToString = "buy" Then
                            rdoType.SelectedValue = "buy"
                        Else
                            rdoType.SelectedValue = "sell"
                        End If

                        ddlClearing.SelectedValue = dr("clearingday").ToString
                        rdoBilling.SelectedValue = dr("billing").ToString
                        rdoDelivery.SelectedValue = dr("delivery").ToString
                        'rdoGoldType.SelectedValue = dr("gold_type_id").ToString
                        ddlPayment.SelectedValue = dr("payment").ToString
                        If dr("payment_duedate") IsNot DBNull.Value Then txtDuedate.Text = CDate(dr("payment_duedate")).ToString(clsManage.formatDateTime)
                        ddlBank.SelectedValue = dr("payment_bank").ToString
                        txtCheq.Text = dr("payment_cheq_no").ToString
                        txtRemark.Text = dr("remark").ToString
                        txtQuan.Text = clsManage.convert2QuantityGram(dr("quantity").ToString)
                        txtPrice.Text = dr("price").ToString
                        txtAmount.Text = dr("amount").ToString
                        txtInvoice.Text = dr("invoice").ToString
                        hdfType.Value = dr("type").ToString
                        ' sp_quan <> null แสดงว่ามีการ splitbill
                        If dr("sp_quan") IsNot DBNull.Value Then
                            rdoType.Enabled = False
                            'rdoGoldType.Enabled = False
                            txtQuan.Enabled = False
                            txtPrice.Enabled = False
                            txtAmount.Enabled = False
                            lblAlertSplitBill.Visible = True
                        End If
                    Else
                        'ไม่มี refno ในระบบ
                        clsManage.alert(Page, "No this ref no in system.")
                    End If
                    'Click From customer list
                ElseIf Request.QueryString("cust_id") IsNot Nothing Then
                    txtCustRef.Text = Request.QueryString("cust_id").ToString
                    If txtCustRef.Text <> "" Then
                        btnSearch_Click(Nothing, Nothing)
                    End If
                End If
            End If

            If Request.QueryString("id") IsNot Nothing Then
                clsManage.Script(Page, "trRefno.style.display='block';trStatus.style.display='block';trDate.style.display='block';div_log.style.display='block';setBgColor();", "displayRefNo")
            Else
                clsManage.Script(Page, "trRefno.style.display='none';trStatus.style.display='none';trDate.style.display='none';div_log.style.display='none';", "displayRefNo")
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Protected Sub btnDealTicket_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnDealTicket.Click
        Try
            If Not validatetion() Then Exit Sub
            Dim bank As String = ""
            Dim duedate As String = ""
            Dim cheq_no As String = ""

            If ddlPayment.SelectedValue <> clsManage.payment.cash.ToString Then
                bank = ddlBank.SelectedValue
            End If
            If ddlPayment.SelectedValue = clsManage.payment.cheq.ToString Then
                duedate = txtDuedate.Text
                cheq_no = txtCheq.Text
            End If

            Dim gold_dep As Boolean = False
            If cbGoldDep.Checked And rdoType.SelectedValue = "buy" Then
                gold_dep = True
            End If

            Dim result As String = clsDB.insertTicket(txtCustRef.Text, "", "", Session(clsManage.iSession.user_id_center.ToString), clsFng.p96g, rdoType.SelectedValue, _
                                                  txtDateDelivery.Text, _
                                                  DateTime.Now, rdoBilling.SelectedValue, ddlPayment.SelectedValue, _
                                                  bank, duedate, cheq_no, _
                                                  txtRemark.Text, rdoDelivery.SelectedValue, Session(clsManage.iSession.user_id_center.ToString), txtQuan.Text, txtPrice.Text, txtAmount.Text, True, gold_dep, txtInvoice.Text, ddlClearing.SelectedValue)
            If result <> "" Then
                clsManage.alert(Page, "Deal Ticket Complete Reference No : " & result & "|nCustomer Name : " + ucPortFolio1.Name + " |nPurity : " + clsFng.p96g + " |nType : " + rdoType.SelectedItem.Text + " |nPrice : " + txtPrice.Text + "|nQuantity : " + txtQuan.Text + "", , "stock_tickets.aspx")
            Else
                clsManage.alert(Page, "Deal Ticket Error. Please Try Again.")
            End If

        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try

    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Try
            If Not validatetion() Then Exit Sub
            Dim bank As String = ""
            Dim duedate As String = ""
            Dim cheq_no As String = ""
            If ddlPayment.SelectedValue <> clsManage.payment.cash.ToString Then
                bank = ddlBank.SelectedValue
            End If
            If ddlPayment.SelectedValue = clsManage.payment.cheq.ToString Then
                duedate = txtDuedate.Text
                cheq_no = txtCheq.Text
            End If

            Dim gold_dep As Boolean = False
            If cbGoldDep.Checked And rdoType.SelectedValue = "buy" Then
                gold_dep = True
            End If
            Dim result As String = clsDB.updatetTicket(lblRefNo.Text, txtCustRef.Text, rdoType.SelectedValue, clsFng.p96g, _
                                    txtDateDelivery.Text, DateTime.ParseExact(txtDateNow.Text, clsManage.formatDateTime, Nothing), _
                                    rdoBilling.SelectedValue, ddlPayment.SelectedValue, _
                                    bank, duedate, cheq_no, txtRemark.Text, rdoDelivery.SelectedValue, Session(clsManage.iSession.user_id_center.ToString).ToString, _
                                    txtQuan.Text, txtPrice.Text, txtAmount.Text, True, gold_dep, txtInvoice.Text, ddlClearing.SelectedValue)
            If result <> "" Then
                'clsManage.alert(Page, "Deal Ticket Complete Reference No : " & lblRefNo.Text & "|nBook No : " + txtBookNo.Text + "|nNo : " + txtRunNo.Text + "|nCustomer Name : " + lblName.Text + " |nType : " + rdoType.SelectedItem.Text + " |nPrice : " + txtPrice.Text + "|nQuantity : " + txtQuan.Text + "", )
                hdfType.Value = rdoType.SelectedValue 'now type for check dupp book no.
                clsManage.alert(Page, "Update Complete.")
                gvLog.DataBind()
            Else
                clsManage.alert(Page, "Update Ticket Error. Please Try Again.")
            End If

        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try

    End Sub

    Function validatetion() As Boolean
        If rdoType.SelectedIndex = -1 Then clsManage.alert(Page, "Please Choose Type.") : Return False
        If txtCustRef.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill, txtCustName.ClientID) : Return False
        'If rdoGoldType.SelectedIndex = -1 Then clsManage.alert(Page, "Please Choose Purity of Gold.") : Return False
        If txtQuan.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill, txtQuan.ClientID) : Return False
        If txtAmount.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill, txtAmount.ClientID) : Return False
        If Not (Request.QueryString("id") IsNot Nothing AndAlso Request.QueryString("id").ToString.Substring(0, 1) = clsFng.strOnline) Then
            If rdoBilling.SelectedIndex = -1 Then clsManage.alert(Page, "Please Choose Billing.") : Return False
        End If

        Try
            If ddlPayment.SelectedValue = clsManage.payment.cheq.ToString AndAlso txtDuedate.Text.Trim <> "" Then
                DateTime.ParseExact(txtDuedate.Text, clsManage.formatDateTime, Nothing)
            End If
        Catch ex As Exception
            clsManage.alert(Page, "Invalid Date Time.", txtDuedate.ClientID)
            Return False
        End Try
        If txtDateDelivery.Text <> "" AndAlso DateTime.ParseExact(txtDateDelivery.Text, clsManage.formatDateTime, Nothing) < DateTime.ParseExact(txtDateNow.Text, clsManage.formatDateTime, Nothing) Then clsManage.alert(Page, "Delivery Date must more then Ticket Date.") : Return False

        If cbGoldDep.Checked And rdoType.SelectedValue = "buy" Then
            Dim quan_gold As Double = 0
            quan_gold = clsManage.convert2zero(ucPortFolio1.Gold96g)
        
        If clsManage.convert2zero(txtQuan.Text) > quan_gold Then
            clsManage.alert(Page, "ตัดทองฝากเกินจำนวนที่ฝากไว้.")
            Return False
        End If
        End If

        Return True
    End Function

    <System.Web.Services.WebMethod()> _
    Public Shared Function checkCustomer(ByVal cust_id As String, ByVal cust_name As String) As String
        Return clsDB.checkCustomer(cust_id, cust_name)
    End Function

    Protected Sub btnClear_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnClear.Click
        Response.Redirect("ticket_deal.aspx")
    End Sub

    Protected Sub btnBack_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnBack.Click
        If Request.QueryString("page") IsNot Nothing Then
            If Request.QueryString("page").ToString = "stock" Then
                Response.Redirect("ticket_order.aspx")
            ElseIf Request.QueryString("page").ToString = "view" Then
                Response.Redirect("ticket_report.aspx")
            Else
                Response.Redirect("ticket_list.aspx")
            End If
        End If
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            ucPortFolio1.CustID = txtCustRef.Text
            ucPortFolio1.LoadPortFolio()
            hdfPwd.Value = ucPortFolio1.PasswordAuthen
            lblName.Text = ucPortFolio1.Name
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try

    End Sub

    Protected Sub linkColor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkColor.Click
        Try
            If ucPortFolio1.CustID = "" Then Exit Sub
            'If clsManage.checkTradeLimit(ucPortFolio1.CustID, txtQuan.Text, txtAmount.Text, rdoType.SelectedValue, rdoGoldType.SelectedValue) = False Then
            '    trade_disable()
            'Else
            '    trade_able()
            'End If

            If clsFng.checkGuarantee(ucPortFolio1.CustID, rdoType.SelectedValue, clsFng.p96g, txtQuan.Text) <> "" Then
                trade_disable()
            Else
                trade_able()
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub linkPwd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkPwd.Click
        If txtPwdAuth.Text.Trim = "" Then clsManage.alert(Page, "Please Enter Password.", txtPwdAuth.ClientID) : Exit Sub
        If hdfPwd.Value = txtPwdAuth.Text Then
            txtAmount.ForeColor = Drawing.Color.Black
            linkPwd.Visible = False
            txtPwdAuth.Visible = False
            hdfTradeLimit.Value = "y"
            ddlPayment.Focus()
        Else
            clsManage.alert(Page, "Password Invalid.", txtPwdAuth.ClientID)
        End If
    End Sub

    Sub trade_able()
        txtAmount.ForeColor = Drawing.Color.Black
        linkPwd.Visible = False
        txtPwdAuth.Visible = False
        hdfTradeLimit.Value = "y"
        ddlPayment.Focus()
    End Sub

    Sub trade_disable()
        txtAmount.ForeColor = Drawing.Color.Red
        linkPwd.Visible = True
        txtPwdAuth.Visible = True
        hdfTradeLimit.Value = "n"
        txtPwdAuth.Focus()
    End Sub

    'Protected Sub rdoGoldType_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rdoGoldType.SelectedIndexChanged
    '    txtQuan.Text = ""
    '    txtAmount.Text = ""
    '    txtPrice.Text = ""
    '    trade_able()
    '    txtQuan.Focus()
    'End Sub

    Protected Sub linkClearPrice_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkClearPrice.Click
        txtQuan.Text = ""
        txtAmount.Text = ""
        txtPrice.Text = ""
        trade_able()
    End Sub

    Protected Sub gvLog_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvLog.RowDataBound

        If e.Row.RowType = DataControlRowType.Header Then
            If Request.QueryString("id") Is Nothing Then Exit Sub
            Dim dt As New Data.DataTable
            dt = clsDB.getTicketLogUpdate(Request.QueryString("id").ToString)
            If dt.Rows.Count > 0 Then
                ViewState("log") = dt
            End If
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            Try

                '****** ต้องเรืยงลำดับ column ใน sql ให้ตรงกับ gridview
                If ViewState("log") IsNot Nothing Then
                    If e.Row.RowIndex <> 0 Then
                        Dim dr As Data.DataRow
                        dr = CType(ViewState("log"), Data.DataTable).Rows(e.Row.RowIndex - 1)
                        For i As Integer = 1 To 22

                            If i <> 11 And i <> 12 And i <> 17 And i <> 20 Then
                                If dr(i).ToString <> e.Row.DataItem(i).ToString Then
                                    e.Row.Cells(i).ForeColor = Drawing.Color.Red
                                End If
                            Else
                                'only date cell 11,12,17,20 (delivert_date,ticket_date,duedate,update_date)
                                Dim logDate1 As String = ""
                                Dim logDate2 As String = ""
                                If dr(i).ToString <> "" Then
                                    logDate1 = CDate(dr(i)).ToString("dd/MM/yyyy")
                                End If
                                If e.Row.DataItem(i).ToString <> "" Then
                                    logDate2 = CDate(e.Row.DataItem(i)).ToString("dd/MM/yyyy")
                                End If

                                If logDate1 <> logDate2 Then
                                    e.Row.Cells(i).ForeColor = Drawing.Color.Red
                                End If
                            End If
                        Next
                    End If
                End If
            Catch ex As Exception
                Throw ex
            End Try
        End If
    End Sub
End Class
