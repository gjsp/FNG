
Partial Class ticket_payment_detail2
    Inherits basePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.QueryString("pid") IsNot Nothing Then
                btnSave.Visible = False
                linkAdd.Enabled = False
                txtPaidCash.Visible = False
                txtPaidGold.Visible = False
                txtNote.Visible = False
                btnCancel.Visible = False
                linkComplete.Enabled = False
                txtPaidCash.Attributes.Add("onkeypress", "numberOnly();")
                txtPaidGold.Attributes.Add("onkeypress", "numberOnly();")
                gv.EmptyDataText = clsManage.EmptyDataText
                gvPopup.EmptyDataText = clsManage.EmptyDataText
                btnSave.Attributes.Add("onclick", "return confirm('Do you want to update?');")
                linkComplete.Attributes.Add("onclick", "return confirm('Do you want to update all complete status?');")

                Dim pid As String = Request.QueryString("pid").ToString
                Dim dc As New dcDBDataContext
                Dim pms = (From pm In dc.payments From ur In dc.users From vpm In dc.v_payments From c In dc.customers Where pm.created_by = ur.user_id And pm.payment_id = pid And pm.payment_id = vpm.payment_id And c.cust_id = pm.cust_id).FirstOrDefault
                If pms IsNot Nothing Then
                    pnMain.Visible = True : pnNon.Visible = False
                    lblpid.Text = pms.pm.payment_id
                    lblBy.Text = pms.ur.user_name.ToString
                    lblDate.Text = CDate(pms.pm.created_date).ToString(clsManage.formatDateTime)
                    lblBill.Text = IIf(pms.pm.billing.ToString = "n", "No", "Yes")
                    lblCustId.Text = pms.pm.cust_id.ToString

                    lblPayCash.Text = Math.Abs(CInt(pms.vpm.amount)).ToString("#,##0")
                    lblPaidCash.Text = CInt(pms.pm.paid_cash).ToString("#,##0")
                    txtPaidCash.Text = CInt(pms.pm.paid_cash).ToString("###0")

                    lblPayGold.Text = Math.Abs(CInt(pms.vpm.quantity)).ToString("#,##0")
                    lblPaidGold.Text = CInt(pms.pm.paid_gold).ToString("#,##0")
                    txtPaidGold.Text = CInt(pms.pm.paid_gold).ToString("###0")

                    lblPurity.Text = IIf(pms.vpm.gold_type_id.ToString = "99", "99.99%", "96.50%")
                    lblName.Text = pms.c.firstname.ToString
                    lblStatus.Text = pms.pm.status.ToString()
                    lblNote.Text = pms.pm.note.ToString()
                    txtNote.Text = pms.pm.note.ToString()
                    If Math.Abs(CInt(lblPayCash.Text)) = Math.Abs(CInt(txtPaidCash.Text)) Then
                        linkComplete.Enabled = True
                    End If
                    ViewState("id") = pms.pm.cust_id.ToString + "," + pms.vpm.gold_type_id.ToString + "," + pms.pm.billing.ToString + "," + pms.pm.cust_id.ToString

                    'get ทองฝาก
                    Dim cpf = dc.getSumPortfolioByCust_id(pms.pm.cust_id).FirstOrDefault
                    lblGoldDep96.Text = clsManage.convert2Quantity(cpf.gold96.ToString)
                    lblGoldDep99.Text = clsManage.convert2Quantity(cpf.gold99.ToString)

                    'check ว่าทองพอตัดหรือไม่
                    Dim goldDep As Decimal = IIf(pms.vpm.gold_type_id = clsFng.p99, cpf.gold99, cpf.gold96)
                    Dim goldRemain As Decimal = Math.Abs(CInt(pms.vpm.quantity)) - pms.pm.paid_gold
                    If goldDep >= goldRemain Then
                        ViewState("refute") = True
                    Else
                        ViewState("refute") = False
                    End If

                    If pms.pm.status = "Complete" Then
                        btnEdit.Visible = False
                        linkComplete.Enabled = False
                        ddlStatus.Visible = False
                        linkComplete.Visible = False
                    End If

                    'check status show 1.complete 2.ตัดทองฝาก

                    If pms.vpm.quantity <> 0 AndAlso pms.vpm.quantity <> pms.pm.paid_gold Then
                        If pms.vpm.amount < 0 Then
                            ddlStatus.Items.Add(New ListItem("Complete ตัดทองฝาก", "903"))
                        ElseIf pms.vpm.amount > 0 Then
                            ddlStatus.Items.Add(New ListItem("Complete ฝาก", "902"))
                        End If
                    End If

                    Dim dt As New Data.DataTable
                    dt = clsFng.getPaymentTrans(pid)
                    ViewState("dt") = dt
                    If dt.Rows.Count > 0 Then
                        gv.DataSource = dt
                        gv.DataBind()
                    End If
                    enableGridDelete(False)
                Else
                    pnMain.Visible = False : pnNon.Visible = True
                End If


            End If

        End If
    End Sub

    Protected Sub imgDel_Click(sender As Object, e As ImageClickEventArgs)
        Try
            Dim i As Integer = CType(CType(sender, ImageButton).Parent.Parent, GridViewRow).RowIndex
            'Dim dc As New dcDBDataContext
            'Dim ticket = (From t In dc.tickets Where t.ticket_id = gv.DataKeys(i).Value.ToString).FirstOrDefault
            'ticket.payment_id = Nothing

            'dc.SubmitChanges()
            'gv.DataBind()
            CType(ViewState("dt"), Data.DataTable).Rows.RemoveAt(i)
            CType(ViewState("dt"), Data.DataTable).AcceptChanges()


            gv.DataSource = CType(ViewState("dt"), Data.DataTable)
            gv.DataBind()

        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Protected Sub gv_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gv.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("imgDel"), ImageButton).OnClientClick = "return confirm(' " & clsManage.msgDel & " ');"
            CType(e.Row.FindControl("linkRef"), HyperLink).NavigateUrl = "ticket_deal.aspx?page=stock&id=" + gv.DataKeys(e.Row.RowIndex)("ref_no").ToString
        End If
    End Sub

    Protected Sub linkAdd_Click(sender As Object, e As EventArgs) Handles linkAdd.Click
        Try
            Dim refNoList As String = ""
            Dim cma As String = "','"
            For Each dr As Data.DataRow In CType(ViewState("dt"), Data.DataTable).Rows
                If refNoList.Length = 0 Then
                    refNoList = dr("ref_no")
                Else
                    refNoList += cma + dr("ref_no")
                End If
            Next
            Dim cust_id As String = CType(ViewState("dt"), Data.DataTable).Rows(0)("cust_id").ToString
            Dim dt As New Data.DataTable
            dt = Nothing ' clsFng.getPaymentDetailPopUp(refNoList, ViewState("id").ToString.Split(",")(0), ViewState("id").ToString.Split(",")(1), ViewState("id").ToString.Split(",")(2))
            gvPopup.DataSource = dt
            gvPopup.DataBind()


            modalPopUpExtender1.Show()
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "err")
        End Try

    End Sub

    Protected Sub link_Click(sender As Object, e As EventArgs)
        Try
            'Dim refnoList As String = ""
            'Dim comma As String = ","
            'For Each gvr As GridViewRow In gv.Rows
            '    If refnoList = "" Then
            '        refnoList = gv.DataKeys(gvr.RowIndex)("ref_no").ToString
            '    Else
            '        refnoList += comma + gv.DataKeys(gvr.RowIndex)("ref_no").ToString
            '    End If
            'Next

            Dim i As Integer = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow).RowIndex
            'refnoList += comma + gvPopup.DataKeys(i)("ref_no").ToString()

            'Dim dc As New dcDBDataContext
            'Dim tks = From tk In dc.tickets Where refnoList.Split(comma).Contains(tk.ref_no)

            'gv.DataSource = tks.ToList
            'gv.DataBind()
            Dim dt As New Data.DataTable
            dt = CType(ViewState("dt"), Data.DataTable)
            Dim dr As Data.DataRow = dt.NewRow
            dr("ref_no") = gvPopup.Rows(i).Cells(0).Text
            dr("type") = gvPopup.Rows(i).Cells(1).Text
            dr("price") = gvPopup.Rows(i).Cells(2).Text
            dr("quantity") = gvPopup.Rows(i).Cells(3).Text
            dr("amount") = gvPopup.Rows(i).Cells(4).Text
            dr("payment_by_name") = IIf(gvPopup.Rows(i).Cells(5).Text = "&nbsp;", "", gvPopup.Rows(i).Cells(5).Text)
            dt.Rows.Add(dr)
            ViewState("dt") = dt
            gv.DataSource = dt
            gv.DataBind()
            linkReport.Enabled = False

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub linkReport_Click(sender As Object, e As EventArgs) Handles linkReport.Click
        clsManage.Script(Page, "window.open('report/rpt_payment.aspx?pid=" + lblpid.Text + "','_blank');")
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click

        'validate
        If Math.Abs(CInt(txtPaidCash.Text)) > Math.Abs(CInt(lblPayCash.Text)) Then
            clsManage.alert(Page, "เกินจำนวนเงินที่ต้องชำระ", , , "limit")
            Exit Sub
        End If
        If Math.Abs(CInt(txtPaidGold.Text)) > Math.Abs(CInt(lblPayGold.Text)) Then
            clsManage.alert(Page, "เกินจำนวนทองที่ต้องชำระ", , , "limit")
            Exit Sub
        End If


        Dim dc As New dcDBDataContext
        Try
            dc.Connection.Open()
            dc.Transaction = dc.Connection.BeginTransaction
            Dim pid As String = Request.QueryString("pid").ToString

            'delete payment_id in ticket tbl all
            Dim ts = From t In dc.tickets Where t.payment_id = pid
            For Each t In ts
                t.payment_id = Nothing
                t.payment_by = Session(clsManage.iSession.user_id_center.ToString).ToString
            Next

            'update payment
            Dim pms = (From pm In dc.payments From vpm In dc.v_payments Where pm.payment_id = pid And vpm.payment_id = pid).FirstOrDefault
            pms.pm.paid_cash = txtPaidCash.Text
            pms.pm.paid_gold = txtPaidGold.Text
            pms.pm.modifier_date = DateTime.Now
            pms.pm.modifier_by = Session(clsManage.iSession.user_id_center.ToString).ToString
            pms.pm.note = txtNote.Text.Trim

            'Add Ticket
            Dim refNoList As String = ""
            Dim cma As String = ","
            For Each dr As Data.DataRow In CType(ViewState("dt"), Data.DataTable).Rows
                If refNoList.Length = 0 Then
                    refNoList = dr("ref_no")
                Else
                    refNoList += cma + dr("ref_no")
                End If
            Next

            Dim tks = From tk In dc.tickets Where refNoList.Split(",").Contains(tk.ref_no)
            For Each tk In tks
                tk.payment_id = pid
                tk.modifier_date = DateTime.Now
                tk.payment_by = Session(clsManage.iSession.user_id_center.ToString).ToString
            Next

            dc.SubmitChanges()
            dc.Transaction.Commit()

            clsManage.Script(Page, String.Format("window.location='ticket_payment_detail.aspx?pid={0}'", Request.QueryString("pid").ToString))
            linkAdd.Enabled = False : linkReport.Enabled = True : btnSave.Visible = False : btnEdit.Visible = True : btnCancel.Visible = False
            txtPaidCash.Visible = False : lblPaidCash.Visible = True
            txtPaidGold.Visible = False : lblPaidGold.Visible = True
            txtNote.Visible = False : lblNote.Visible = True
        Catch ex As Exception
            dc.Transaction.Rollback()
            clsManage.alert(Page, ex.Message, , , "err")
        Finally
            dc.Connection.Close()
        End Try

    End Sub

    Protected Sub btnEdit_Click(sender As Object, e As System.EventArgs) Handles btnEdit.Click
        btnSave.Visible = True
        btnEdit.Visible = False
        linkAdd.Enabled = True
        linkComplete.Enabled = False
        linkReport.Enabled = False
        txtPaidCash.Visible = True
        txtPaidGold.Visible = True
        lblPaidCash.Visible = False
        lblPaidGold.Visible = False
        txtNote.Visible = True
        lblNote.Visible = False
        btnCancel.Visible = True
        txtPaidCash.Text = lblPaidCash.Text
        txtPaidGold.Text = lblPaidGold.Text
        txtNote.Text = lblNote.Text
        enableGridDelete(True)
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As System.EventArgs) Handles btnCancel.Click
        'btnSave.Visible = False
        'btnEdit.Visible = True
        'linkAdd.Enabled = False
        'linkReport.Enabled = True
        'txtPaid.Visible = False
        'lblPaid.Visible = True
        'btnCancel.Visible = False
        Response.Redirect("ticket_payment_detail.aspx?pid=" + Request.QueryString("pid").ToString)
    End Sub

    Private Sub enableGridDelete(enable As Boolean)
        For Each gvr As GridViewRow In gv.Rows
            CType(gvr.FindControl("imgDel"), ImageButton).Enabled = enable
            If enable Then
                CType(gvr.FindControl("imgDel"), ImageButton).ImageUrl = "~/images/i_del.png"
            Else
                CType(gvr.FindControl("imgDel"), ImageButton).ImageUrl = "~/images/i_del2.png"
            End If
        Next
    End Sub

    Protected Sub linkComplete_Click(sender As Object, e As EventArgs) Handles linkComplete.Click
        'validate
        If Not Math.Abs(CInt(lblPayCash.Text)) = Math.Abs(CInt(txtPaidCash.Text)) Then
            clsManage.alert(Page, "โปรดชำระเงินให้ครบจำนวน", , , "limit")
            Exit Sub
        End If

        If ddlStatus.SelectedValue = "903" Then
            If Not ViewState("refute") Then
                clsManage.alert(Page, "จำนวนทองไม่พอในการตัดทองฝาก", , , "limit")
                Exit Sub
            End If
        End If

        If ddlStatus.SelectedValue = "901" Then
            If Math.Abs(CInt(txtPaidGold.Text)) <> Math.Abs(CInt(lblPayGold.Text)) Then
                clsManage.alert(Page, "โปรดชำระทองให้ครบจำนวน", , , "limit")
                Exit Sub
            End If
        End If

        Dim dc As New dcDBDataContext
        Try
            dc.Connection.Open()
            dc.Transaction = dc.Connection.BeginTransaction
            Dim pid As String = Request.QueryString("pid").ToString

            'update payment
            Dim pms = (From pm In dc.payments From vpm In dc.v_payments Where pm.payment_id = pid And vpm.payment_id = pid).FirstOrDefault
            pms.pm.paid_cash = txtPaidCash.Text
            pms.pm.paid_gold = txtPaidGold.Text
            'pms.pm.paid_gold_diff = Math.Abs(CInt(pms.vpm.quantity)) - pms.pm.paid_gold
            pms.pm.modifier_date = DateTime.Now
            pms.pm.modifier_by = Session(clsManage.iSession.user_id_center.ToString).ToString
            pms.pm.note = txtNote.Text.Trim()
            pms.pm.status = "Complete"
            pms.pm.status_id = ddlStatus.SelectedValue

            'Loop For update status complete (901)
            'retrive amount each payment type(cash,trans,cheq) for update stock
            'update actual,ticket log

            Dim payType As New Dictionary(Of String, Integer)
            payType.Add(clsManage.payment.cash.ToString, 0)
            payType.Add(clsManage.payment.trans.ToString, 0)
            payType.Add(clsManage.payment.cheq.ToString, 0)

            'Update Ticket log
            Dim tlogs As ticket_log
            Dim refNoList As String = ""
            Dim cma As String = ","
            For Each dr As Data.DataRow In CType(ViewState("dt"), Data.DataTable).Rows
                If refNoList.Length = 0 Then
                    refNoList = dr("ref_no")
                Else
                    refNoList += cma + dr("ref_no")
                End If
            Next

            Dim tks = From tk In dc.tickets Where refNoList.Split(",").Contains(tk.ref_no)
            For Each tk In tks
                tk.payment_id = pid
                tk.status_id = "901" 'only 901 ,ticket buy = 903,sell = 902
                tk.modifier_date = DateTime.Now
                tk.modifier_by = Session(clsManage.iSession.user_id_center.ToString).ToString
                tk.payment_by = Session(clsManage.iSession.user_id_center.ToString).ToString
                tk.before_status_id = "101"
                tk.delivery_date = DateTime.Now
                If tk.type = "buy" Then
                    payType(tk.payment.ToString) += (-tk.amount)
                Else
                    payType(tk.payment.ToString) += tk.amount
                End If
                tlogs = New ticket_log
                tlogs.datetime = DateTime.Now
                tlogs.ref_no = tk.ref_no
                tlogs.update_by = Session(clsManage.iSession.user_id_center.ToString).ToString
                dc.ticket_logs.InsertOnSubmit(tlogs)
            Next

            Dim stoSumDep = dc.getStockSumDeposit.FirstOrDefault
            Dim sto = (From st In dc.stocks).FirstOrDefault

            sto.price_base += pms.vpm.amount
            sto.cash += payType(clsManage.payment.cash.ToString)
            sto.trans += payType(clsManage.payment.trans.ToString)
            sto.cheq += payType(clsManage.payment.cheq.ToString)

            If pms.vpm.gold_type_id = clsFng.p99 Then
                sto.G99_base += pms.vpm.quantity
            Else
                sto.G96_base += pms.vpm.quantity
            End If

            'Insert Customer Tran : Complete ตัดทองฝาก
            'INSERT INTO [customer_trans] ([cust_id], [datetime], [type], [ticket_refno], [gold_type_id], [quantity], [amount], [price], [remark], [created_by], [created_date]) VALUES " & _
            '('{0}', getdate(), 'Withdraw', '{1}', '{2}', {3}, {4}, {6}, 'ตัดทองฝากจาก {1}.', '{5}', getdate())", cust_id, refno, gold_type_Dep, quan_dep, amount_dep, modifier_by, price)

            Dim ct As New customer_tran
            If ddlStatus.SelectedValue = "903" Then
                With ct
                    .cust_id = pms.pm.cust_id
                    .datetime = DateTime.Now
                    .type = clsFng.wd
                    .ticket_refno = pms.pm.payment_id.ToString
                    .gold_type_id = pms.vpm.gold_type_id
                    .quantity = Math.Abs(CInt(pms.vpm.quantity - pms.pm.paid_gold))
                    .amount = Math.Abs(CInt(pms.vpm.amount))
                    .price = 0
                    .remark = "ตัดทองฝากจาก " + .ticket_refno + "  by payment"
                    .created_by = Session(clsManage.iSession.user_id_center.ToString).ToString
                    .created_date = DateTime.Now
                End With
                dc.customer_trans.InsertOnSubmit(ct)

            ElseIf ddlStatus.SelectedValue = "902" Then
                With ct
                    .cust_id = pms.pm.cust_id
                    .datetime = DateTime.Now
                    .type = clsFng.dps
                    .ticket_refno = pms.pm.payment_id.ToString
                    .gold_type_id = pms.vpm.gold_type_id
                    .quantity = Math.Abs(CInt(pms.vpm.quantity)) - pms.pm.paid_gold
                    .amount = Math.Abs(CInt(pms.vpm.amount))
                    .price = 0
                    .remark = "ฝากทองจาก " + .ticket_refno + "  by payment"
                    .created_by = Session(clsManage.iSession.user_id_center.ToString).ToString
                    .created_date = DateTime.Now
                End With
                dc.customer_trans.InsertOnSubmit(ct)
            End If

            'actual gold
            If ddlStatus.SelectedValue = "901" Then 'ถ้าเป็นการตัดทองฝาก ไม่ต้องบวกเพิ่มไปในสต๊อก , cash เหมือนเดิม
                If pms.vpm.gold_type_id = clsFng.p99 Then
                    stoSumDep.G99Dep += pms.vpm.quantity
                Else
                    stoSumDep.G96Dep += pms.vpm.quantity
                End If
            End If

            stoSumDep.priceDep += pms.vpm.amount
            stoSumDep.cashDep += payType(clsManage.payment.cash.ToString)
            stoSumDep.transDep += payType(clsManage.payment.trans.ToString)
            stoSumDep.cheqDep += payType(clsManage.payment.cheq.ToString)


            Dim acts As New actual
            acts.asset_id = 0
            acts.ref_no = pms.pm.payment_id.ToString()
            acts.asset_type = "Gold"
            acts.created_by = Session(clsManage.iSession.user_id_center.ToString).ToString
            acts.datetime = DateTime.Now
            acts.purity = pms.vpm.gold_type_id
            acts.quantity = IIf(pms.vpm.quantity < 0, -pms.vpm.quantity, pms.vpm.quantity)
            acts.amount = IIf(pms.vpm.amount < 0, -pms.vpm.amount, pms.vpm.amount)
            acts.status_id = ddlStatus.SelectedValue
            acts.status_name = "Payment " + ddlStatus.SelectedItem.Text
            If pms.vpm.amount < 0 Then
                acts.type = "In"
            Else
                acts.type = "Out"
            End If
            If ddlStatus.SelectedValue = "903" Then
                acts.type = "ตัดทองฝาก"
            End If
            acts.order_type = ""
            acts.price_base = stoSumDep.priceDep
            acts.G99_base = stoSumDep.G99Dep
            acts.G96_base = stoSumDep.G96Dep
            acts.G96G_base = stoSumDep.G96GDep
            acts.cust_id = pms.pm.cust_id
            acts.before_status_id = "101"
            acts.note = ""
            acts.payment = ""
            acts.cash = stoSumDep.cashDep
            acts.trans = stoSumDep.transDep
            acts.cheq = stoSumDep.cheqDep
            dc.actuals.InsertOnSubmit(acts)

            'actual Cash
            acts = New actual
            acts.asset_id = 0
            acts.ref_no = pms.pm.payment_id.ToString()
            acts.asset_type = "Cash"
            acts.created_by = Session(clsManage.iSession.user_id_center.ToString).ToString
            acts.datetime = DateTime.Now
            acts.purity = pms.vpm.gold_type_id
            acts.quantity = IIf(pms.vpm.quantity < 0, -pms.vpm.quantity, pms.vpm.quantity)
            acts.amount = IIf(pms.vpm.amount < 0, -pms.vpm.amount, pms.vpm.amount)
            acts.status_id = ddlStatus.SelectedValue
            acts.status_name = "Payment " + ddlStatus.SelectedItem.Text
            If pms.vpm.amount < 0 Then
                acts.type = "Out"
            Else
                acts.type = "In"
            End If
            acts.order_type = ""
            acts.price_base = stoSumDep.priceDep
            acts.G99_base = stoSumDep.G99Dep
            acts.G96_base = stoSumDep.G96Dep
            acts.G96G_base = stoSumDep.G96GDep
            acts.cust_id = pms.pm.cust_id
            acts.before_status_id = "101"
            acts.note = ""
            acts.payment = ""
            acts.cash = stoSumDep.cashDep
            acts.trans = stoSumDep.transDep
            acts.cheq = stoSumDep.cheqDep
            dc.actuals.InsertOnSubmit(acts)

            dc.SubmitChanges()
            dc.Transaction.Commit()

            linkAdd.Enabled = False : linkReport.Enabled = True : btnSave.Visible = False : btnEdit.Visible = True : btnCancel.Visible = False
            txtPaidCash.Visible = False : lblPaidCash.Visible = True
            txtPaidGold.Visible = False : lblPaidGold.Visible = True
            txtNote.Visible = False : lblNote.Visible = True

            'clsManage.Script(Page, String.Format("window.location='ticket_payment_detail.aspx?pid={0}'", Request.QueryString("pid").ToString))
            Dim url As String = String.Format("ticket_payment_detail.aspx?pid={0}", Request.QueryString("pid").ToString)
            Dim msgGoldDep As String = ""

            If ddlStatus.SelectedValue = "903" Then
                msgGoldDep = "|nตัดทองฝากไปจำนวน " + clsManage.convert2Quantity(ct.quantity.ToString) + IIf(pms.vpm.gold_type_id = clsFng.p99, " กิโล", " บาท")
            ElseIf ddlStatus.SelectedValue = "902" Then
                msgGoldDep = "|nฝากทองจำนวน " + clsManage.convert2Quantity(ct.quantity.ToString) + IIf(pms.vpm.gold_type_id = clsFng.p99, " กิโล", " บาท")
            End If
            clsManage.alert(Page, "Update status all complete" + msgGoldDep, , url, "complete")
        Catch ex As Exception
            dc.Transaction.Rollback()
            clsManage.alert(Page, ex.Message, , , "err")
        Finally
            dc.Connection.Close()
        End Try

    End Sub

End Class
