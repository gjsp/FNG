
Partial Class ticket_split_bill
    Inherits basePage


    Protected Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        If Not Page.IsPostBack Then
            Try
                If Session(clsManage.iSession.user_role_center.ToString) IsNot Nothing Then
                    Dim dtRole As New Data.DataTable : dtRole = Session(clsManage.iSession.user_role_center.ToString)
                    If dtRole.Rows.Find(Me.Page.Page.ToString) IsNot Nothing Then
                        Dim dr As Data.DataRow = dtRole.Rows.Find(Me.Page.Page.ToString)
                        'Print
                        If dr(clsDB.roles.print) = False Then
                            linkRptSell.Enabled = False
                            ViewState(clsDB.roles.print) = False
                        End If
                        'Delete
                        If dr(clsDB.roles.delete) = False Then
                            ViewState(clsDB.roles.delete) = False
                        End If
                    End If
                End If
            Catch ex As Exception
                clsManage.alert(Page, ex.Message)
            End Try
        End If
    End Sub

    Protected Sub Page_LoadComplete(sender As Object, e As System.EventArgs) Handles Me.LoadComplete
        If Not Page.IsPostBack Then
            Try
                If Session(clsManage.iSession.user_role_center.ToString) IsNot Nothing Then
                    Dim dtRole As New Data.DataTable : dtRole = Session(clsManage.iSession.user_role_center.ToString)
                    If dtRole.Rows.Find(Me.Page.Page.ToString) IsNot Nothing Then
                        Dim dr As Data.DataRow = dtRole.Rows.Find(Me.Page.Page.ToString)
                        'add
                        If dr(clsDB.roles.add) = False Then
                            btnAddBill.Enabled = False
                            btnRefresh.Enabled = False
                            btnSave.Enabled = False
                        End If

                    End If
                End If
            Catch ex As Exception
                clsManage.alert(Page, ex.Message)
            End Try
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.QueryString("id") IsNot Nothing Then
                tabTicket.ActiveTabIndex = 1
                txtDuedate.Attributes.Add("onkeypress", "return false;")
                txtQuan.Attributes.Add("onkeypress", "checkNumber();")
                txtDuedateCash.Attributes.Add("onkeypress", "return false;")
                txtQuanCash.Attributes.Add("onkeypress", "checkNumber();")
                txtDuedateGold.Attributes.Add("onkeypress", "return false;")
                txtQuanGold.Attributes.Add("onkeypress", "checkNumber();")

                clsDB.getBank(ddlBank)
                clsDB.getBank(ddlBankCash)
                clsDB.getBank(ddlBankGold)

                txtDuedate_CalendarExtender.Format = clsManage.formatDateTime
                txtDuedateCash_CalendarExtender.Format = clsManage.formatDateTime
                txtDuedateGold_CalendarExtender.Format = clsManage.formatDateTime

                gvTicket.EmptyDataText = "<div style='color:red;text-align:center;border:solid 1px silver;'>No Bill</div>"
                gvCash.EmptyDataText = clsManage.EmptyDataText
                gvGold.EmptyDataText = clsManage.EmptyDataText
                btnSave.Enabled = False
                btnSaveCash.Enabled = False

                Dim dt As New Data.DataTable
                dt = clsDB.getTicket(Request.QueryString("id").ToString())
                If dt.Rows.Count > 0 Then
                    Dim dr As Data.DataRow = dt.Rows(0)
                    'validation 

                    If dr("billing").ToString = "y" Then
                        pageNone("Ticket แบบมีบิลไม่สามารถแยกบิลได้")
                        Exit Sub
                    End If
                    'If dr("run_no").ToString <> "" Then
                    '    pageNone("Ticket มีการออกบิลไปแล้ว")
                    '    Exit Sub
                    'End If

                    lblRefno.Text = dr("ticket_id").ToString
                    lblGoldType.Text = dr("gold_type_name").ToString
                    lblOrderType.Text = dr("type").ToString
                    lblPrice.Text = clsManage.convert2Currency(dr("price").ToString)
                    lblQuan.Text = clsManage.convert2Quantity(dr("quantity").ToString)
                    lblAmount.Text = clsManage.convert2Currency(dr("amount").ToString)
                    lblQuanBalance.Text = clsManage.convert2Quantity(0).ToString
                    lblAmountBalance.Text = clsManage.convert2Currency(0).ToString
                    ViewState("quan") = clsManage.convert2zero(dr("quantity").ToString)
                    ViewState("amount") = clsManage.convert2zero(dr("amount").ToString)
                    ViewState("amount_balance") = clsManage.convert2zero(dr("amount").ToString)
                    ViewState("quan_balance") = clsManage.convert2zero(dr("quantity").ToString)

                    hdfType.Value = dr("type").ToString
                    If hdfType.Value = "sell" Then
                        'tpCash.HeaderText = "Cash Receipt"
                    Else
                        'tpCash.HeaderText = "Cash Payment"
                    End If
                    ViewState("cust_id") = dr("cust_id").ToString
                    ViewState("gold_type_id") = dr("gold_type_id").ToString

                    If hdfType.Value = "buy" Then
                        'tpGold.Visible = False
                        ddlCashStatus.Items.Add(New ListItem("Complete ตัดทอง", "903"))
                        ddlBillStatus.Items.Add(New ListItem("Complete ตัดทอง", "903"))
                    Else
                        'tpGold.Visible = True
                        ddlCashStatus.Items.Add(New ListItem("Complete ฝาก", "902"))
                        ddlBillStatus.Items.Add(New ListItem("Complete ฝาก", "902"))
                    End If

                    If dr("status_id").ToString <> "101" Then
                        btnAddBill.Enabled = False : btnSave.Enabled = False
                        btnAddCash.Enabled = False : btnSaveCash.Enabled = False
                        btnAddGold.Enabled = False : btnSaveGold.Enabled = False
                    End If

                    'check payment
                    If dr("payment_id").ToString <> "" Then
                        btnAddBill.Enabled = False
                    End If

                    'Split
                    dt = New Data.DataTable
                    dt = clsDB.getTicketSplit(lblRefno.Text, clsManage.splitMode.Split.ToString)
                    If dt.Rows.Count > 0 Then
                        ViewState(clsManage.splitMode.Split.ToString) = dt
                        gvTicket.DataSource = ViewState(clsManage.splitMode.Split.ToString)
                        gvTicket.DataBind()

                        'case has split ต้องแสดง quantity,amont คงเหลือ
                        Dim quanBalance As String = clsManage.convert2Currency(ViewState("quan").ToString) - clsManage.convert2Currency(CType(ViewState(clsManage.splitMode.Split.ToString), Data.DataTable).Compute("SUM(quantity)", "").ToString())
                        Dim amountBalance As String = clsManage.convert2Currency(ViewState("amount").ToString) - clsManage.convert2Currency(CType(ViewState(clsManage.splitMode.Split.ToString), Data.DataTable).Compute("SUM(amount)", "").ToString())
                       
                        lblQuan.Text = clsManage.convert2Currency(ViewState("quan").ToString)
                        lblAmount.Text = clsManage.convert2Currency(ViewState("amount").ToString)
                        lblQuanBalance.Text = clsManage.convert2Quantity(quanBalance).ToString
                        lblAmountBalance.Text = clsManage.convert2Currency(amountBalance).ToString
                        ViewState("quan_balance") = quanBalance 'for update
                        ViewState("amount_balance") = amountBalance 'for check limmit cash
                    Else
                        ViewState(clsManage.splitMode.Split.ToString) = dt.Clone
                        gvTicket.DataBind()
                    End If

                    'Cash Receipt
                    dt = New Data.DataTable
                    dt = clsDB.getTicketSplit(lblRefno.Text, clsManage.splitMode.Receipt.ToString)
                    If dt.Rows.Count > 0 Then
                        ViewState(clsManage.splitMode.Receipt.ToString) = dt
                        gvCash.DataSource = ViewState(clsManage.splitMode.Receipt.ToString)
                        gvCash.DataBind()
                    Else
                        ViewState(clsManage.splitMode.Receipt.ToString) = dt.Clone
                        gvCash.DataBind()
                    End If

                    'Deposit Gold
                    dt = New Data.DataTable
                    dt = clsDB.getTicketSplit(lblRefno.Text, clsManage.splitMode.DepositGold.ToString)
                    If dt.Rows.Count > 0 Then
                        ViewState(clsManage.splitMode.DepositGold.ToString) = dt
                        gvGold.DataSource = ViewState(clsManage.splitMode.DepositGold.ToString)
                        gvGold.DataBind()
                    Else
                        ViewState(clsManage.splitMode.DepositGold.ToString) = dt.Clone
                        gvGold.DataBind()
                    End If
                Else
                    pageNone("")
                End If
            Else
                pageNone("")
            End If
        End If
    End Sub

    Sub pageNone(msg As String)
        pnMain.Visible = False
        pnNone.Visible = True
        lblMsg.Text = msg
    End Sub

    Protected Sub gvTicket_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTicket.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            ViewState("sumAmount") = 0
            ViewState("sumQuan") = 0
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            Try
                'receipt
                CType(e.Row.FindControl("imgDelReceipt"), ImageButton).CommandArgument = e.Row.DataItem("ticket_sp_id").ToString
                CType(e.Row.FindControl("imgDelReceipt"), ImageButton).OnClientClick = "return confirm(' Do you want to Cancel this Receipt?');"
                If e.Row.DataItem("run_no").ToString = "" Then
                    CType(e.Row.FindControl("imgDelReceipt"), ImageButton).Visible = False
                Else
                    Dim url As String = String.Format("report/rpt_split.aspx?type={0}&ref={1}", _
                                                       clsManage.splitMode.Split.ToString + clsManage.EditMode.view.ToString, _
                                                       e.Row.DataItem("ticket_sp_id").ToString)

                    If ViewState(clsDB.roles.print) IsNot Nothing AndAlso ViewState(clsDB.roles.print) = False Then
                        CType(e.Row.FindControl("lblReceipt"), Label).Text = e.Row.DataItem("run_no").ToString
                    Else
                        CType(e.Row.FindControl("lblReceipt"), Label).Text = String.Format("<a href='{1}' target='_blank'>{0}</a>", e.Row.DataItem("run_no").ToString, url)
                    End If
                End If

                e.Row.Cells(7).Text = ddlBillStatus.Items.FindByValue(e.Row.DataItem("status_id")).Text
                e.Row.Cells(0).Text = e.Row.RowIndex + 1
                If e.Row.DataItem("payment_bank").ToString.Trim <> "" Then
                    e.Row.Cells(4).Text = ddlBank.Items.FindByValue(e.Row.DataItem("payment_bank").ToString).Text
                End If

                If ViewState(clsDB.roles.delete) IsNot Nothing AndAlso ViewState(clsDB.roles.delete) = False Then
                    CType(e.Row.FindControl("imgDelReceipt"), ImageButton).Enabled = False
                    CType(e.Row.FindControl("imgDelReceipt"), ImageButton).ImageUrl = "~/images/i_del2.png"

                    CType(e.Row.FindControl("imgDel"), ImageButton).Enabled = False
                    CType(e.Row.FindControl("imgDel"), ImageButton).ImageUrl = "~/images/i_del2.png"
                End If

                'payment
                If e.Row.DataItem("payment_id").ToString <> "" Then
                    CType(e.Row.FindControl("linkPayment"), HyperLink).NavigateUrl = String.Format("ticket_payment_detail.aspx?pid={0}&cid={1}", e.Row.DataItem("payment_id").ToString, e.Row.DataItem("cust_id").ToString)
                End If

                ViewState("sumAmount") += Double.Parse(e.Row.DataItem("amount"))
                ViewState("sumQuan") += Double.Parse(e.Row.DataItem("quantity"))
            Catch ex As Exception
                Throw ex
            End Try
        End If
    End Sub

    Protected Sub gvTicket_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTicket.RowCreated
        If e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(2).Text = Double.Parse(ViewState("sumQuan")).ToString(clsManage.formatQuantity)
            e.Row.Cells(3).Text = Double.Parse(ViewState("sumAmount")).ToString(clsManage.formatCurrency)
            e.Row.Cells(0).ColumnSpan = 2
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells(0).Text = "Summary"
            e.Row.Cells(0).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(1).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Right
        End If
    End Sub

    Protected Sub gvCash_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCash.RowCreated
        If e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(2).Text = Double.Parse(ViewState("sumAmountCash")).ToString(clsManage.formatCurrency)
            e.Row.Cells(0).ColumnSpan = 2
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells(0).Text = "Summary"
            e.Row.Cells(0).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(1).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Right
        End If
    End Sub

    Protected Sub gvCash_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCash.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            ViewState("sumAmountCash") = 0
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            'If hdfType.Value = "buy" Then
            '    CType(e.Row.Cells(gvTicket.Columns.Count - 2).FindControl("ddlSell"), HtmlSelect).Attributes.Add("style", "display:none")
            '    Dim ddlBuy As New HtmlSelect
            '    ddlBuy = CType(e.Row.Cells(gvTicket.Columns.Count - 2).FindControl("ddlBuy"), HtmlSelect)
            '    If e.Row.DataItem("status_id") Is DBNull.Value Then
            '        ddlBuy.SelectedIndex = 0
            '    Else
            '        ddlBuy.SelectedIndex = ddlBuy.Items.IndexOf(ddlBuy.Items.FindByValue(e.Row.DataItem("status_id")))
            '    End If
            '    If e.Row.DataItem("status_id").ToString <> "101" Then
            '        ddlBuy.Disabled = True
            '    Else
            '        CType(e.Row.FindControl("cbCheq"), CheckBox).Visible = False
            '        CType(e.Row.FindControl("linkUpdate"), LinkButton).Visible = False
            '    End If
            'Else
            '    CType(e.Row.Cells(gvTicket.Columns.Count - 2).FindControl("ddlBuy"), HtmlSelect).Attributes.Add("style", "display:none")
            '    Dim ddlSell As New HtmlSelect
            '    ddlSell = CType(e.Row.Cells(gvTicket.Columns.Count - 2).FindControl("ddlSell"), HtmlSelect)
            '    If e.Row.DataItem("status_id") Is Nothing Then
            '        ddlSell.SelectedIndex = 0
            '    Else
            '        ddlSell.SelectedIndex = ddlSell.Items.IndexOf(ddlSell.Items.FindByValue(e.Row.DataItem("status_id")))
            '    End If

            '    If e.Row.DataItem("status_id").ToString <> "101" Then
            '        ddlSell.Disabled = True
            '    Else
            '        CType(e.Row.FindControl("cbCheq"), CheckBox).Visible = False
            '        CType(e.Row.FindControl("linkUpdate"), LinkButton).Visible = False
            '    End If
            'End If
            'If e.Row.DataItem("status_id").ToString <> "101" Then
            '    CType(e.Row.FindControl("imgDelCash"), ImageButton).Visible = False
            'End If

            'CType(e.Row.FindControl("imgDelCash"), ImageButton).Attributes.Add("onclick", "return confirm(" + clsManage.msgDel + ");")

            If e.Row.DataItem("ticket_sp_id").ToString <> "" Then
                CType(e.Row.FindControl("imgDelCash"), ImageButton).Visible = False
            End If

            CType(e.Row.FindControl("ddlDep"), HtmlSelect).Visible = False

            If e.Row.DataItem("dep").ToString = "d" Then
                CType(e.Row.FindControl("lblDep"), Label).Text = "ฝาก"
            ElseIf e.Row.DataItem("dep").ToString = "w" Then
                CType(e.Row.FindControl("lblDep"), Label).Text = "ถอน"
            Else
                CType(e.Row.FindControl("lblDep"), Label).Text = ""
            End If


            e.Row.Cells(6).Text = ddlCashStatus.Items.FindByValue(e.Row.DataItem("status_id")).Text

            e.Row.Cells(0).Text = e.Row.RowIndex + 1
            If e.Row.DataItem("payment_bank").ToString.Trim <> "" Then
                e.Row.Cells(3).Text = ddlBankCash.Items.FindByValue(e.Row.DataItem("payment_bank").ToString).Text
            End If

            ViewState("sumAmountCash") += Double.Parse(e.Row.DataItem("amount"))

        End If
    End Sub

    Protected Sub gvGold_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvGold.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            ViewState("sumQuantityGold") = 0
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Text = e.Row.RowIndex + 1
            If e.Row.DataItem("payment_bank").ToString.Trim <> "" Then
                e.Row.Cells(3).Text = ddlBankGold.Items.FindByValue(e.Row.DataItem("payment_bank").ToString).Text
            End If

            ViewState("sumQuantityGold") += Double.Parse(e.Row.DataItem("quantity"))

        End If
    End Sub

    Protected Sub gvGold_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvGold.RowCreated
        If e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(2).Text = Double.Parse(ViewState("sumQuantityGold")).ToString(clsManage.formatCurrency)
            e.Row.Cells(0).ColumnSpan = 2
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells(0).Text = "Summary"
            e.Row.Cells(0).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(1).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Right
        End If
    End Sub

    Protected Sub gvTicket_SelectedIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSelectEventArgs) Handles gvTicket.SelectedIndexChanging
        Dim id As String = gvTicket.DataKeys(e.NewSelectedIndex).Value
        clsManage.Script(Page, "top.window.location.href='ticket_split_bill.aspx?id=" & id & " '")
    End Sub

    Function validatetionBill() As Boolean
        Try
            If txtQuan.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill) : Return False
            'If ddlPayment.SelectedValue = "trans" And ddlBank.SelectedIndex = 0 Then clsManage.alert(Page, clsManage.msgRequiredFill) : Return False
            'If ddlPayment.SelectedValue = "cheq" And (ddlBank.SelectedIndex = 0 Or txtCheq.Text.Trim = "" Or txtDuedate.Text.Trim = "") Then clsManage.alert(Page, clsManage.msgRequiredFill) : Return False

            If Not IsNumeric(txtQuan.Text) Then clsManage.alert(Page, "Please Enter Amount Number") : Return False

            Dim sumQuan As Double = Double.Parse(txtQuan.Text)
            For Each gRow As GridViewRow In gvTicket.Rows
                sumQuan += Double.Parse(gRow.Cells(2).Text)
            Next

            If sumQuan > Double.Parse(ViewState("quan").ToString) Then
                clsManage.alert(Page, "New Amount value must be less then Amount.") : Return False
            End If
            ViewState("sumQuan") = sumQuan

            'Check ตัดทองฝาก
            If ddlBillStatus.SelectedValue = "903" Then
                Dim result As String = clsManage.checkExcessShort(ViewState("cust_id").ToString, txtQuan.Text, ViewState("gold_type_id").ToString)
                If result <> "" Then
                    clsManage.alert(Page, result) : Return False
                End If
            End If
           
            Return True
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Function

    Function validatetionCash() As Boolean
        Try
            If txtQuanCash.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill) : Return False
            If Not IsNumeric(txtQuanCash.Text) Then clsManage.alert(Page, "Please Enter Amount Number") : Return False

            Dim sumAmount As Double = Double.Parse(txtQuanCash.Text)
            For Each gRow As GridViewRow In gvCash.Rows
                sumAmount += Double.Parse(gRow.Cells(2).Text)
            Next

            If sumAmount > Double.Parse(ViewState("amount_balance").ToString) Then
                clsManage.alert(Page, "New Amount value must be less then Amount.") : Return False
            End If
            ViewState("sumAmountCash") = sumAmount 'for check = all amount

            'Check ตัดทองฝาก
            If ddlCashStatus.SelectedValue = "903" Then
                If clsManage.convert2zero(ViewState("sumAmountCash").ToString) < clsManage.convert2zero(ViewState("amount_balance").ToString) Then
                    clsManage.alert(Page, "ตัดทองฝากไม่ครบทั้งหมดต้องไปทำ Split Bill ก่อน.") : Return False
                End If

                Dim result As String = clsManage.checkExcessShort(ViewState("cust_id").ToString, ViewState("quan_balance").ToString, ViewState("gold_type_id").ToString)
                If result <> "" Then
                    clsManage.alert(Page, result) : Return False
                End If
            End If

            'check ฝากทอง
            If ddlCashStatus.SelectedValue = "902" Then
                If clsManage.convert2zero(ViewState("sumAmountCash").ToString) < clsManage.convert2zero(ViewState("amount_balance").ToString) Then
                    clsManage.alert(Page, "ฝากทองไม่ครบทั้งหมดต้องไปทำ Split Bill ก่อน.") : Return False
                End If
            End If

            Return True
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Function

    Function validatetionGold() As Boolean
        Try
            If txtQuanGold.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill) : Return False
            If Not IsNumeric(txtQuanGold.Text) Then clsManage.alert(Page, "Please Enter Quantity Number") : Return False

            Dim sumuanGold As Double = Double.Parse(txtQuanGold.Text)
            For Each gRow As GridViewRow In gvGold.Rows
                sumuanGold += Double.Parse(gRow.Cells(2).Text)
            Next

            If sumuanGold > Double.Parse(ViewState("quan").ToString) Then
                clsManage.alert(Page, "New Quantity value must be less then Quantity.") : Return False
            End If

            Return True
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Function

    Protected Sub btnAddBill_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddBill.Click
        If Not validatetionBill() Then Exit Sub
        ViewState(clsManage.splitMode.Split.ToString) = addDTBill(ViewState(clsManage.splitMode.Split.ToString))
        gvTicket.DataSource = ViewState(clsManage.splitMode.Split.ToString)
        gvTicket.DataBind()
        btnAddBill.Enabled = False ' add bill only 1
        btnSave.Enabled = True
        linkRptSell.Enabled = False
    End Sub

    Protected Sub btnAddCash_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddCash.Click
        If Not validatetionCash() Then Exit Sub
        ViewState(clsManage.splitMode.Receipt.ToString) = addDTCash(ViewState(clsManage.splitMode.Receipt.ToString))
        gvCash.DataSource = ViewState(clsManage.splitMode.Receipt.ToString)
        gvCash.DataBind()
        btnAddCash.Enabled = False ' add bill only 1
        btnSaveCash.Enabled = True
    End Sub

    Protected Sub btnAddGold_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAddGold.Click
        If Not validatetionGold() Then Exit Sub
        ViewState(clsManage.splitMode.DepositGold.ToString) = addDTGold(ViewState(clsManage.splitMode.DepositGold.ToString))
        gvGold.DataSource = ViewState(clsManage.splitMode.DepositGold.ToString)
        gvGold.DataBind()
    End Sub

    Function addDTBill(ByVal dt As Data.DataTable) As Data.DataTable

        Dim dr As Data.DataRow = dt.NewRow
        dr("ref_no") = lblRefno.Text
        dr("payment") = ddlPayment.SelectedValue
        dr("quantity") = Double.Parse(txtQuan.Text).ToString(clsManage.formatQuantity)
        dr("status_id") = ddlBillStatus.SelectedValue

        If ViewState("gold_type_id") = "96" Then
            dr("amount") = Double.Parse(txtQuan.Text) * Double.Parse(lblPrice.Text)
        ElseIf ViewState("gold_type_id") = "99" Then
            dr("amount") = ((Double.Parse(txtQuan.Text) * Double.Parse(lblPrice.Text)) * 656) / 10
        ElseIf ViewState("gold_type_id") = "96G" Then
            dr("amount") = (Double.Parse(txtQuan.Text) * Double.Parse(lblPrice.Text)) * 0.0656
        End If
        If ddlPayment.SelectedValue = "trans" Then
            dr("payment_bank") = ddlBank.SelectedValue
        ElseIf ddlPayment.SelectedValue = "cheq" Then
            dr("payment_bank") = ddlBank.SelectedValue
            dr("payment_cheq_no") = txtCheq.Text
            If txtDuedate.Text <> "" Then
                dr("payment_duedate") = DateTime.ParseExact(txtDuedate.Text, clsManage.formatDateTime, Nothing)
            Else
                dr("payment_duedate") = System.DBNull.Value
            End If
        End If
        dt.Rows.Add(dr)
        Return dt
    End Function

    Function addDTCash(ByVal dt As Data.DataTable) As Data.DataTable

        Dim dr As Data.DataRow = dt.NewRow
        dr("ref_no") = lblRefno.Text
        dr("payment") = ddlPaymentcash.SelectedValue
        dr("quantity") = Double.Parse(txtQuanCash.Text).ToString(clsManage.formatQuantity)
        dr("status_id") = ddlCashStatus.SelectedValue
        dr("amount") = txtQuanCash.Text

        If ddlpaymentCash.SelectedValue = "trans" Then
            dr("payment_bank") = ddlBankCash.SelectedValue
        ElseIf ddlpaymentCash.SelectedValue = "cheq" Then
            dr("payment_bank") = ddlBankCash.SelectedValue
            dr("payment_cheq_no") = txtCheqCash.Text
            If txtDuedateCash.Text <> "" Then
                dr("payment_duedate") = DateTime.ParseExact(txtDuedateCash.Text, clsManage.formatDateTime, Nothing)
            Else
                dr("payment_duedate") = System.DBNull.Value
            End If
        End If
        dt.Rows.Add(dr)
        Return dt
    End Function

    Function addDTGold(ByVal dt As Data.DataTable) As Data.DataTable

        Dim dr As Data.DataRow = dt.NewRow
        dr("ref_no") = lblRefno.Text
        dr("payment") = ddlPaymentGold.SelectedValue
        dr("quantity") = Double.Parse(txtQuanGold.Text).ToString(clsManage.formatQuantity)
        dr("status_id") = "101"

        dr("quantity") = txtQuanGold.Text

        If ddlPaymentGold.SelectedValue = "trans" Then
            dr("payment_bank") = ddlBankGold.SelectedValue
        ElseIf ddlPaymentGold.SelectedValue = "cheq" Then
            dr("payment_bank") = ddlBankGold.SelectedValue
            dr("payment_cheq_no") = txtCheqGold.Text
            If txtDuedateGold.Text <> "" Then
                dr("payment_duedate") = DateTime.ParseExact(txtDuedateGold.Text, clsManage.formatDateTime, Nothing)
            Else
                dr("payment_duedate") = System.DBNull.Value
            End If
        End If
        dt.Rows.Add(dr)
        Return dt
    End Function

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            'clsDB.delTicketSplit(lblRefno.Text, clsManage.splitMode.Split.ToString)
            If ViewState("del" + clsManage.splitMode.Split.ToString) IsNot Nothing Then
                Dim sp_id As String = ""
                Dim ref_no As String = ""
                Dim row As Integer = 0
                Dim quan As String = ""
                Dim amount As String = ""
                Dim orderType As String = hdfType.Value
                Dim status As String = ""
                For Each dr As Data.DataRow In CType(ViewState("del" + clsManage.splitMode.Split.ToString), Data.DataTable).Rows
                    sp_id = dr("ticket_sp_id").ToString
                    ref_no = dr("ref_no").ToString
                    row = clsManage.convert2zero(dr("row").ToString)
                    quan = dr("quantity").ToString
                    amount = dr("amount").ToString
                    status = dr("status_id").ToString
                    Dim payment As String = dr("payment").ToString
                    Dim result As Boolean = clsDB.delTicketSplitBySpId(sp_id, ref_no, row, quan, amount, orderType, ViewState("gold_type_id").ToString, status, payment)
                    If result Then
                        clsDB.updateWhenDeleteFullSplit(ref_no)
                        Dim status_name As String = ""
                        Dim update_gold As String = "y"
                        Dim update_cash As String = "y"
                        Dim cash As String = "n"
                        Dim type As String = ""

                        If status = "901" Then
                            status_name = "ยกเลิก complete ส่งมอบ(split bill)"
                            update_cash = "y"
                            If orderType = "sell" Then
                                type = "In"
                            ElseIf orderType = "buy" Then
                                type = "Out"
                            End If
                        ElseIf status = "902" Then
                            status_name = "ยกเลิก complete ฝาก(split bill)"
                            type = "ยกเลิกฝากทอง"
                        ElseIf status = "903" Then
                            status_name = "ยกเลิก complete ตัดทอง(split bill)"
                            type = "ยกเลิกตัดทองฝาก"
                        End If
                        clsDB.insert_actual2("", lblRefno.Text, Session(clsManage.iSession.user_id_center.ToString), ViewState("gold_type_id").ToString, quan, amount, status, status_name, type, hdfType.Value, ViewState("cust_id").ToString, "", "", "y", "y", "n", payment)
                    End If
                Next
                ViewState("del" + clsManage.splitMode.Split.ToString) = Nothing
            End If

            If ViewState(clsManage.splitMode.Split.ToString) IsNot Nothing AndAlso CType(ViewState(clsManage.splitMode.Split.ToString), Data.DataTable).Rows.Count > 0 Then

                Dim dt As New Data.DataTable
                dt = ViewState(clsManage.splitMode.Split.ToString)
                Dim iDate As String = ""
                Dim i As Integer = 0
                Dim ddlType As String = ""
                Dim isReceipt As Boolean = False
                If hdfType.Value = "sell" Then
                    ddlType = "ddlSell"
                Else
                    ddlType = "ddlBuy"
                End If

                Dim dep As String = "n"
                Dim updateTicketStatus As String = "n" ' update ticket ข้างนอก
                Dim updateStock As String = "y" 'split ต้อง update stock
                Dim updateDep As String = "y" 'ข้างใน check status_id อีกรอบ
                If clsManage.convert2zero(ViewState("sumQuan").ToString) = clsManage.convert2zero(ViewState("quan").ToString) Then
                    updateTicketStatus = "y"
                End If

                Dim status_id As String = ""
                For Each dr As Data.DataRow In dt.Rows
                    If dr("payment_duedate").ToString <> "" Then
                        iDate = CDate(dr("payment_duedate")).ToString(clsManage.formatDateTime)
                    End If
                    If dt.Rows.Count - 1 = i And dr("ticket_sp_id").ToString = "" Then
                        isReceipt = True
                        status_id = dr("status_id").ToString
                        Dim result As String = clsDB.insertTicketSplit(lblRefno.Text, i.ToString, dr("quantity").ToString, "0", dr("Amount").ToString, dr("payment").ToString, dr("payment_bank").ToString, dr("payment_cheq_no").ToString, iDate, Session(clsManage.iSession.user_id_center.ToString), status_id, isReceipt, clsManage.splitMode.Split.ToString, ViewState("gold_type_id").ToString, hdfType.Value, _
                                                dep, ViewState("cust_id").ToString, updateDep, updateStock, updateTicketStatus)
                        If result <> "" Then
                            Dim type As String = ""
                            Dim status_name As String = ""
                            If hdfType.Value = "sell" Then
                                type = "Out"
                            ElseIf hdfType.Value = "buy" Then
                                type = "In"
                            End If

                            If status_id = "901" Then
                                status_name = "complete ส่งมอบ(split bill)"
                            ElseIf status_id = "902" Then
                                status_name = "complete ฝากทอง(split bill)"
                                type = "ฝากทอง(split bill)"
                            ElseIf status_id = "903" Then
                                status_name = "complete ตัดทอง"
                                type = "ตัดทองฝาก(split bill)"
                            End If
                            'clsDB.insert_actual("", lblRefno.Text, Session(clsManage.iSession.user_id_center.ToString), ViewState("gold_type_id").ToString,  dt.Rows(i)("quantity").ToString, dt.Rows(i)("Amount").ToString, status_id, "Split_Bill", hdfType.Value, clsManage.splitMode.Split.ToString, 0, 0, 0, 0, ViewState("cust_id").ToString)
                            clsDB.insert_actual2("", lblRefno.Text, Session(clsManage.iSession.user_id_center.ToString), ViewState("gold_type_id").ToString, dt.Rows(i)("quantity").ToString, dt.Rows(i)("Amount").ToString, status_id, status_name, type, hdfType.Value, ViewState("cust_id").ToString, "", "", "y", "y", "n", dr("payment").ToString)
                        End If

                    End If
                    i = i + 1
                Next

                Dim scriptDep As String = ""
                If status_id = "902" Or status_id = "903" Then
                    scriptDep = "window.open('customer_trans.aspx?cust_id=" + ViewState("cust_id").ToString + "');"
                End If
                clsManage.alert(Page, "Save Complete.")
                clsManage.Script(Page, "opener.document.getElementById('ctl00_SampleContent_btnSearchAdv').click();" + scriptDep + " ", "close")
            Else
                'case deleted all window.close();

                clsManage.alert(Page, "Save Complete.")
                clsManage.Script(Page, "opener.document.getElementById('ctl00_SampleContent_btnSearchAdv').click();", "close")

            End If
           bindGrid()
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Protected Sub btnSaveCash_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveCash.Click
        Try

            'clsDB.delTicketSplit(lblRefno.Text, clsManage.splitMode.DepositCash.ToString)
            If ViewState(clsManage.splitMode.Receipt.ToString) IsNot Nothing AndAlso CType(ViewState(clsManage.splitMode.Receipt.ToString), Data.DataTable).Rows.Count > 0 Then
                'If clsManage.convert2zero(ViewState("sumAmountCash").ToString) < clsManage.convert2zero(ViewState("amount_balance").ToString) Then
                '    clsManage.alert(Page, "จ่ายเงินไม่ครบ ต้องไปนำเงินไปฝากก่อน.")
                '    Exit Sub
                'End If
                Dim dt As New Data.DataTable
                dt = ViewState(clsManage.splitMode.Receipt.ToString)
                Dim i As Integer = dt.Rows.Count - 1
                Dim iDate As String = ""
                If dt.Rows(i)("payment_duedate").ToString <> "" Then
                    iDate = CDate(dt.Rows(i)("payment_duedate")).ToString(clsManage.formatDateTime)
                End If

                Dim dep As String = "n"
                Dim updateTicketStatus As String = "n"
                Dim updateStock As String = "n"
                Dim updateDep As String = "y"
                Dim cash_receipt As String = "n"
                Dim payment As String = dt.Rows(i)("payment").ToString
                If clsManage.convert2zero(ViewState("sumAmountCash").ToString) = clsManage.convert2zero(ViewState("amount_balance").ToString) Then
                    updateTicketStatus = "y"
                    updateStock = "y"
                    cash_receipt = "y"
                End If

                Dim status_id = dt.Rows(i)("status_id").ToString
                Dim result As String = clsDB.insertTicketSplit(lblRefno.Text, 0, ViewState("quan_balance").ToString, 0, dt.Rows(i)("Amount").ToString, payment, dt.Rows(i)("payment_bank").ToString, _
                                        dt.Rows(i)("payment_cheq_no").ToString, iDate, Session(clsManage.iSession.user_id_center.ToString), status_id, 0, _
                                        clsManage.splitMode.Receipt.ToString, ViewState("gold_type_id").ToString, hdfType.Value, dep, ViewState("cust_id").ToString, updateDep, updateStock, updateTicketStatus, "", cash_receipt)
                If updateTicketStatus = "y" And result <> "" Then 'case จ่ายครบ
                    clsDB.insert_actual2("", lblRefno.Text, Session(clsManage.iSession.user_id_center.ToString), ViewState("gold_type_id").ToString, ViewState("quan_balance"), ViewState("amount_balance"), "901", tpCash.HeaderText, hdfType.Value, clsManage.splitMode.Split.ToString, ViewState("cust_id").ToString, "995", "", "y", "y", "n", payment)
                End If

                'clsManage.alert(Page, "Save Complete.")
                'clsManage.Script(Page, "window.close();opener.document.getElementById('ctl00_SampleContent_btnSearchAdv').click();", "close")

                Dim scriptDep As String = ""
                If status_id = "902" Or status_id = "903" Then
                    scriptDep = "window.open('customer_trans.aspx?cust_id=" + ViewState("cust_id").ToString + "');"
                End If
                clsManage.alert(Page, "Save Complete.")
                clsManage.Script(Page, "window.close();opener.document.getElementById('ctl00_SampleContent_btnSearchAdv').click();" + scriptDep + " ", "close")
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Protected Sub btnSaveGold_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSaveGold.Click
        Try
            clsDB.delTicketSplit(lblRefno.Text, clsManage.splitMode.DepositGold.ToString)
            If ViewState(clsManage.splitMode.DepositGold.ToString) IsNot Nothing AndAlso CType(ViewState(clsManage.splitMode.DepositGold.ToString), Data.DataTable).Rows.Count > 0 Then
                Dim dt As New Data.DataTable
                dt = ViewState(clsManage.splitMode.DepositGold.ToString)
                Dim iDate As String = ""
                Dim i As Integer = 0
                For Each dr As Data.DataRow In dt.Rows
                    If dr("payment_duedate").ToString <> "" Then
                        iDate = CDate(dr("payment_duedate")).ToString(clsManage.formatDateTime)
                    End If

                    clsDB.insertTicketSplit(lblRefno.Text, i.ToString, dr("quantity").ToString, 0, 0, dr("payment").ToString, dr("payment_bank").ToString, dr("payment_cheq_no").ToString, iDate, _
                                            Session(clsManage.iSession.user_id_center.ToString), "", 0, clsManage.splitMode.DepositGold.ToString, ViewState("gold_type_id").ToString, hdfType.Value, ViewState("cust_id").ToString)
                    i += 1
                Next
            End If
            clsManage.alert(Page, "Save Complete.")
            clsManage.Script(Page, "opener.document.getElementById('ctl00_SampleContent_btnSearchAdv').click();window.close();", "close")

        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Protected Sub imgDelCash_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Try
            Dim i As Integer = CType(CType(sender, ImageButton).Parent.Parent, GridViewRow).RowIndex
            CType(ViewState(clsManage.splitMode.Receipt.ToString), Data.DataTable).Rows.RemoveAt(i)
            gvCash.DataSource = ViewState(clsManage.splitMode.Receipt.ToString)
            gvCash.DataBind()
            btnSaveCash.Enabled = False
            btnAddCash.Enabled = True
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Protected Sub imgDelGold_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Try
            Dim i As Integer = CType(CType(sender, ImageButton).Parent.Parent, GridViewRow).RowIndex
            CType(ViewState(clsManage.splitMode.DepositGold.ToString), Data.DataTable).Rows.RemoveAt(i)
            gvGold.DataSource = ViewState(clsManage.splitMode.DepositGold.ToString)
            gvGold.DataBind()
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Protected Sub imgDel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Try
            Dim i As Integer = CType(CType(sender, ImageButton).Parent.Parent, GridViewRow).RowIndex

            If ViewState("del" + clsManage.splitMode.Split.ToString) Is Nothing Then
                ViewState("del" + clsManage.splitMode.Split.ToString) = CType(ViewState(clsManage.splitMode.Split.ToString), Data.DataTable).Clone()
            End If

            CType(ViewState("del" + clsManage.splitMode.Split.ToString), Data.DataTable).ImportRow(CType(ViewState(clsManage.splitMode.Split.ToString), Data.DataTable).Rows(i))
            CType(ViewState(clsManage.splitMode.Split.ToString), Data.DataTable).Rows.RemoveAt(i)
            gvTicket.DataSource = ViewState(clsManage.splitMode.Split.ToString)
            gvTicket.DataBind()
            btnAddBill.Enabled = True
            btnSave.Enabled = True

        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

#Region "Receipt"
    Protected Sub linkUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Update(sender, CType(CType(sender, LinkButton).Parent.Parent.Parent.Parent, GridView))
    End Sub

    Protected Sub linkSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Save(sender, CType(CType(sender, LinkButton).Parent.Parent.Parent.Parent, GridView))
    End Sub

    Protected Sub linkCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Cencal(sender, CType(CType(sender, LinkButton).Parent.Parent.Parent.Parent, GridView))
    End Sub

    Public Sub Update(ByVal sender As Object, ByVal gv As GridView)
        Try
            Dim i As Integer = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow).RowIndex
            CType(sender, LinkButton).Visible = False

            gv.Rows(i).FindControl("linkSave").Visible = True
            gv.Rows(i).FindControl("linkCancel").Visible = True
            CType(gv.Rows(i).FindControl("lblDep"), Label).Visible = False
            CType(gv.Rows(i).FindControl("ddlDep"), HtmlSelect).Visible = True

        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Public Sub Save(ByVal sender As Object, ByVal gv As GridView)
        Try
            Dim i As Integer = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow).RowIndex
            CType(sender, LinkButton).Visible = False
            gv.Rows(i).FindControl("linkCancel").Visible = False
            gv.Rows(i).FindControl("linkUpdate").Visible = True


            '' Dim result As Boolean = clsDB.updatePaidCheq(gv.DataKeys(i)("ticket_sp_id"))
            'If result Then
            '    clsManage.alert(Page, "Save Complete.", , "ticket_split.aspx?id=" & Request.QueryString("id").ToString & "", )
            'End If
        Catch ex As Exception
            gv.DataBind()
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Public Sub Cencal(ByVal sender As Object, ByVal gv As GridView)
        Try
            Dim i As Integer = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow).RowIndex
            CType(sender, LinkButton).Visible = False
            gv.Rows(i).FindControl("linkSave").Visible = False
            gv.Rows(i).FindControl("linkUpdate").Visible = True
            CType(gv.Rows(i).FindControl("lblDep"), Label).Visible = True
            CType(gv.Rows(i).FindControl("ddlDep"), HtmlSelect).Visible = False

        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub
#End Region

    Protected Sub linkRptSell_Click(sender As Object, e As System.EventArgs) Handles linkRptSell.Click
        Try
            Dim strRefno As String = ""
            Dim type As String = ""
            Dim billing As String = ""

            For Each dr As GridViewRow In gvTicket.Rows
                If CType(dr.Cells(gvTicket.Columns.Count - 1).FindControl("cbRow"), HtmlInputCheckBox).Checked = True Then
                    If strRefno = "" Then
                        strRefno = gvTicket.DataKeys(dr.RowIndex)("ticket_sp_id").ToString
                        type = clsManage.splitMode.Split.ToString
                        billing = gvTicket.DataKeys(dr.RowIndex)("billing").ToString
                    Else
                        strRefno += "," + gvTicket.DataKeys(dr.RowIndex)("ticket_sp_id").ToString
                    End If
                End If
            Next

            If strRefno <> "" Then
                Dim msgReceipt As String = clsFng.checkReceiptNoSplit(strRefno)
                If msgReceipt <> "" Then
                    clsManage.alert(Page, msgReceipt)
                    bindGrid(True)
                    Exit Sub
                End If

                Dim url As String = String.Format("report/rpt_split.aspx?ref=" + strRefno + "&type={0}", type)
                clsManage.Script(Page, "window.open('" + url + "','_blank');")

            End If

        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Protected Sub imgDelReceipt_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs)
        Try
            Dim receipt_no As String = CType(sender, ImageButton).CommandArgument
            If clsFng.updateReceiptNoSplit(receipt_no) Then
               bindGrid()
            End If

        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Sub bindGrid(Optional isRefresh As Boolean = False)
        Try
            If isRefresh = False Then
                btnAddBill.Enabled = True
                linkRptSell.Enabled = True
                btnSave.Enabled = False
            End If
            gvTicket.DataSource = clsDB.getTicketSplit(lblRefno.Text, clsManage.splitMode.Split.ToString)
            gvTicket.DataBind()

            'Split
            Dim dt As New Data.DataTable
            dt = New Data.DataTable
            dt = clsDB.getTicketSplit(lblRefno.Text, clsManage.splitMode.Split.ToString)
            If dt.Rows.Count > 0 Then
                ViewState(clsManage.splitMode.Split.ToString) = dt
                gvTicket.DataSource = ViewState(clsManage.splitMode.Split.ToString)
                gvTicket.DataBind()

                'case has split ต้องแสดง quantity,amont คงเหลือ
                Dim quanBalance As String = clsManage.convert2Currency(ViewState("quan").ToString) - clsManage.convert2Currency(CType(ViewState(clsManage.splitMode.Split.ToString), Data.DataTable).Compute("SUM(quantity)", "").ToString())
                Dim amountBalance As String = clsManage.convert2Currency(ViewState("amount").ToString) - clsManage.convert2Currency(CType(ViewState(clsManage.splitMode.Split.ToString), Data.DataTable).Compute("SUM(amount)", "").ToString())

                lblQuan.Text = clsManage.convert2Currency(ViewState("quan").ToString)
                lblAmount.Text = clsManage.convert2Currency(ViewState("amount").ToString)
                lblQuanBalance.Text = clsManage.convert2Quantity(quanBalance).ToString
                lblAmountBalance.Text = clsManage.convert2Currency(amountBalance).ToString
                ViewState("quan_balance") = quanBalance 'for update
                ViewState("amount_balance") = amountBalance 'for check limmit cash
            Else
                ViewState(clsManage.splitMode.Split.ToString) = dt.Clone
                gvTicket.DataBind()
            End If

        Catch ex As Exception
            Response.Redirect("ticket_split_bill.aspx")
        End Try
    End Sub

    Protected Sub btnRefresh_Click(sender As Object, e As System.EventArgs) Handles btnRefresh.Click
        bindGrid(True)

    End Sub

    Protected Sub gvTicket_Load(sender As Object, e As System.EventArgs) Handles gvTicket.Load
        If gvTicket.Rows.Count > 0 Then
            linkRptSell.Visible = True
        Else
            linkRptSell.Visible = False
        End If
    End Sub

End Class
