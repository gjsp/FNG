
Partial Class stock_tickets
    Inherits basePage
    Dim colCustId As Integer = 4
    Dim colQ96G As Integer = 10
    Dim colQuan As Integer = 11
    Dim colPrice As Integer = 12
    Dim colAmount As Integer = 13


    Protected Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        If Not Page.IsPostBack Then
            Try
                If Session(clsManage.iSession.user_role_center.ToString) IsNot Nothing Then
                    Dim dtRole As New Data.DataTable : dtRole = Session(clsManage.iSession.user_role_center.ToString)
                    If dtRole.Rows.Find(Me.Page.Page.ToString) IsNot Nothing Then
                        Dim dr As Data.DataRow = dtRole.Rows.Find(Me.Page.Page.ToString)
                        'Update
                        If dr(clsDB.roles.update) = False Then
                            ViewState(clsDB.roles.update) = False
                        End If
                        'Print
                        If dr(clsDB.roles.print) = False Then
                            linkReviewDoc.Enabled = False
                            linkRptSell.Enabled = False
                            ViewState(clsDB.roles.print) = False
                        End If
                        'Export
                        If dr(clsDB.roles.export) = False Then
                            linkExcel.Enabled = False
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

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then

            imgSearchCustRef.Attributes.Add("onclick", "openCustomerList('report');return false;")
            clsManage.getDropDownlist(ddlStatus, clsDB.getTicket_status)
            ddlStatus.Items.Insert(0, New ListItem("-- All --", ""))
            gvTicket.EmptyDataText = clsManage.EmptyDataText
            txtDate1.Attributes.Add("onkeypress", "return false;")
            txtDate2.Attributes.Add("onkeypress", "return false;")
            txtDelDate1.Attributes.Add("onkeypress", "return false;")
            txtDelDate2.Attributes.Add("onkeypress", "return false;")
            txtCustName.Attributes.Add("onkeypress", "if(event.keyCode==13){$get('" & btnSearchAdv.ClientID & "').focus();}")
            txtTicketRef.Attributes.Add("onkeypress", "if(event.keyCode==13){$get('" & btnSearchAdv.ClientID & "').focus();}")

            ViewState("sumQuan") = 0
            ViewState("sumPrice") = 0
            ViewState("sumAmount") = 0

            rdoBilling.Attributes.Add("onclick", "$get('" & btnSearchAdv.ClientID & "').click();")
            rdoType.Attributes.Add("onclick", "$get('" & btnSearchAdv.ClientID & "').click();")
            rdoCenter.Attributes.Add("onclick", "$get('" & btnSearchAdv.ClientID & "').click();")
            ddlStatus.Attributes.Add("onchange", "$get('" & btnSearchAdv.ClientID & "').click();")

            linkRptSell.Attributes.Add("onclick", "return isCheck();")

            txtDate1_CalendarExtender.Format = clsManage.formatDateTime
            txtDate2_CalendarExtender.Format = clsManage.formatDateTime
            txtDelDate1_CalendarExtender.Format = clsManage.formatDateTime
            txtDelDate2_CalendarExtender.Format = clsManage.formatDateTime
            'txtDate1_CalendarExtender.SelectedDate = DateTime.Now
            'txtDate2_CalendarExtender.SelectedDate = DateTime.Now
            txtDate1.Text = Now.ToString(clsManage.formatDateTime)
            txtDate2.Text = Now.ToString(clsManage.formatDateTime)

            btnSearchAdv_Click(Nothing, Nothing)
        End If
    End Sub

    Protected Sub upgSearch_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles upgSearch.Load
        System.Threading.Thread.Sleep(upgSearch.DisplayAfter)
    End Sub

    Protected Sub btnSearchAdv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchAdv.Click
        Try
            If (txtDate1.Text <> "" And txtDate2.Text = "") OrElse (txtDate1.Text = "" And txtDate2.Text <> "") Then clsManage.alert(Page, "Please Select Date") : Exit Sub
            If (txtDelDate1.Text <> "" And txtDelDate2.Text = "") OrElse (txtDelDate1.Text = "" And txtDelDate2.Text <> "") Then clsManage.alert(Page, "Please Select Date") : Exit Sub

            gvTicket.DataSourceID = objSrcTicket.ID
            objSrcTicket.SelectParameters("ticket_id").DefaultValue = txtTicketRef.Text
            objSrcTicket.SelectParameters("cust_name").DefaultValue = txtCustName.Text
            objSrcTicket.SelectParameters("type").DefaultValue = rdoType.SelectedValue
            objSrcTicket.SelectParameters("billing").DefaultValue = rdoBilling.SelectedValue
            objSrcTicket.SelectParameters("status_id").DefaultValue = ddlStatus.SelectedValue

            objSrcTicket.SelectParameters("ticket_date1").DefaultValue = txtDate1.Text
            objSrcTicket.SelectParameters("ticket_date2").DefaultValue = txtDate2.Text

            objSrcTicket.SelectParameters("del_date1").DefaultValue = txtDelDate1.Text
            objSrcTicket.SelectParameters("del_date2").DefaultValue = txtDelDate2.Text
            objSrcTicket.SelectParameters("isCenter").DefaultValue = rdoCenter.SelectedValue
            gvTicket.DataBind()

            If gvTicket.Rows.Count > 0 Then
                linkRptSell.Visible = True
                linkReviewDoc.Visible = True
            Else
                linkRptSell.Visible = False
                linkReviewDoc.Visible = False
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try

    End Sub

    Function validatetion() As Boolean
        If txtCustName.Text.Trim = "" Then Return False

        Return True
    End Function

    <System.Web.Services.WebMethod()> _
    Public Shared Function updateStatus(ByVal refno As String, ByVal status_id As String, ByVal quan As Double, ByVal amount As Double, ByVal payment As String, ByVal billing As String, ByVal modifier_by As String, ByVal gold_type As String) As String
        Return "" ' clsDB.updateTicket_status(refno, status_id, modifier_by, "", "", amount, gold_type, "", "")
    End Function

    Protected Sub gvTicket_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTicket.RowCreated
        If e.Row.RowType = DataControlRowType.Footer Then
            e.Row.Cells(colQ96G).Text = clsManage.convert2QuantityGram(ViewState("sumQuan96G"))
            e.Row.Cells(colQuan).Text = clsManage.convert2Quantity(ViewState("sumQuan96")) 'Double.Parse(ViewState("sumQuan96")).ToString("#,##0.00000")
            e.Row.Cells(colPrice).Text = clsManage.convert2Quantity(ViewState("sumQuan99")) ' Double.Parse(ViewState("sumQuan99")).ToString("#,##0.00000")
            e.Row.Cells(colAmount).Text = clsManage.convert2Currency(ViewState("sumPrice")) ' Double.Parse(ViewState("sumPrice")).ToString("#,##0.00")
            e.Row.Cells(colAmount + 1).Text = clsManage.convert2Currency(ViewState("sumAmount")) ' Double.Parse(ViewState("sumAmount")).ToString("#,##0.00")

            'set the columnspan
            e.Row.Cells(0).ColumnSpan = colQ96G - 1
            'remove the second cell
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells.Remove(e.Row.Cells(1))
            'e.Row.Cells.Remove(e.Row.Cells(1))

            e.Row.Cells(0).Text = "Summary"
            e.Row.Cells(0).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(1).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(4).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(5).HorizontalAlign = HorizontalAlign.Right
        End If
    End Sub

    Sub disableHtmlSelect(ByVal status_id As String, ByVal ddl As HtmlSelect)
        'ddl.Items(ddl.Items.IndexOf(ddl.Items.FindByValue(status_id))).Attributes.Add("style", "display:none")
        ddl.Items(2).Attributes.Add("style", "display:none;")
    End Sub

    Protected Sub gvTicket_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTicket.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            ViewState("sumQuan96G") = 0
            ViewState("sumQuan96") = 0
            ViewState("sumQuan99") = 0
            ViewState("sumPrice") = 0
            ViewState("sumAmount") = 0

        End If

        If e.Row.RowType = DataControlRowType.DataRow Then

            'For export excel
            Dim drv As Data.DataRowView = e.Row.DataItem
            ViewState("drv") = drv.DataView.Table

            'Status 
            If e.Row.DataItem("ticket_id").ToString().Substring(0, 1) = clsFng.strOnline Then
                If e.Row.DataItem("billing").ToString = "" Then
                    'not set
                    CType(e.Row.FindControl("linkBillEdit"), LinkButton).Visible = True
                    CType(e.Row.FindControl("hdfBill"), HiddenField).Value = ""
                Else
                    'bill or nobill
                    CType(e.Row.FindControl("linkBillEdit"), LinkButton).Visible = False
                    Dim id As String = e.Row.DataItem("ticket_id").ToString
                    Dim type As String = e.Row.DataItem("type").ToString
                    Dim billing As String = e.Row.DataItem("billing").ToString
                    CType(e.Row.FindControl("hdfBill"), HiddenField).Value = billing 'set for go page report
                    CType(e.Row.FindControl("lblBill"), Label).Text = String.Format("<a href='{1}' target='_blank'>{0}</a>", IIf(e.Row.DataItem("billing").ToString = "y", "Yes", "No"), "ticket_online.aspx?type=" + type + "&billing=" + billing + "&t=v&id=" + id)
                End If
            Else
                'CType(e.Row.FindControl("lblBill"), Label).Text = IIf(e.Row.DataItem("billing").ToString = "y", "Yes", "No")
                If ViewState(clsDB.roles.print) IsNot Nothing AndAlso ViewState(clsDB.roles.print) = False Then
                    CType(e.Row.FindControl("lblBill"), Label).Enabled = False
                    CType(e.Row.FindControl("lblBill"), Label).Text = IIf(e.Row.DataItem("billing").ToString = "y", "Yes", "No")
                Else
                    CType(e.Row.FindControl("lblBill"), Label).Text = String.Format("<a href='{1}' target='_blank'>{0}</a>", IIf(e.Row.DataItem("billing").ToString = "y", "Yes", "No"), "ticket_online.aspx?type=" + e.Row.DataItem("type").ToString + "&billing=" + e.Row.DataItem("billing").ToString + "&t=v&id=" + e.Row.DataItem("ticket_id").ToString)
                End If
                'CType(e.Row.FindControl("lblBill"), Label).Text = String.Format("<a href='{1}' target='_blank'>{0}</a>", IIf(e.Row.DataItem("billing").ToString = "y", "Yes", "No"), "ticket_online.aspx?type=" + e.Row.DataItem("type").ToString + "&billing=" + e.Row.DataItem("billing").ToString + "&t=v&id=" + e.Row.DataItem("ticket_id").ToString)
                CType(e.Row.FindControl("hdfBill"), HiddenField).Value = e.Row.DataItem("billing").ToString
                CType(e.Row.FindControl("linkBillEdit"), LinkButton).Visible = False
            End If

            If e.Row.DataItem("gold_type_id").ToString = "96G" Then
                CType(e.Row.Cells(2).FindControl("link"), HyperLink).NavigateUrl = "ticket_deal_g.aspx?page=stock&id=" + e.Row.DataItem("ticket_id").ToString
            Else
                CType(e.Row.Cells(2).FindControl("link"), HyperLink).NavigateUrl = "ticket_deal.aspx?page=stock&id=" + e.Row.DataItem("ticket_id").ToString
            End If
            CType(e.Row.FindControl("linkCust"), HyperLink).NavigateUrl = "customer_detail.aspx?id=" + e.Row.DataItem("cust_id").ToString
            CType(e.Row.FindControl("hdfBefore_status"), HiddenField).Value = e.Row.DataItem("status_id").ToString

            If e.Row.DataItem("type").ToString = "buy" Then
                CType(e.Row.Cells(gvTicket.Columns.Count - 2).FindControl("ddlSell"), HtmlSelect).Attributes.Add("style", "display:none")
                Dim ddlBuy As New HtmlSelect
                ddlBuy = CType(e.Row.Cells(gvTicket.Columns.Count - 2).FindControl("ddlBuy"), HtmlSelect)
                ddlBuy.SelectedIndex = ddlBuy.Items.IndexOf(ddlBuy.Items.FindByValue(e.Row.DataItem("status_id")))
                CType(e.Row.Cells(gvTicket.Columns.Count - 2).FindControl("linkUpdate"), LinkButton).CommandArgument = "ddlBuy"
            Else 'Sell
                CType(e.Row.Cells(gvTicket.Columns.Count - 2).FindControl("ddlBuy"), HtmlSelect).Attributes.Add("style", "display:none")
                Dim ddlSell As New HtmlSelect
                ddlSell = CType(e.Row.Cells(gvTicket.Columns.Count - 2).FindControl("ddlSell"), HtmlSelect)
                ddlSell.SelectedIndex = ddlSell.Items.IndexOf(ddlSell.Items.FindByValue(e.Row.DataItem("status_id")))
                ddlSell.Items(ddlSell.SelectedIndex).Value = e.Row.DataItem("status_id").ToString
                CType(e.Row.Cells(gvTicket.Columns.Count - 2).FindControl("linkUpdate"), LinkButton).CommandArgument = "ddlSell"
            End If

            If e.Row.DataItem("sp_quan") IsNot DBNull.Value Then
                e.Row.Cells(1).ForeColor = Drawing.Color.Red
                'have split bill update status outsite ไม่ได้
                CType(e.Row.Cells(gvTicket.Columns.Count - 2).FindControl("linkUpdate"), LinkButton).Attributes.Add("onclick", "alert('This ticket has split bill. Please Update inside this ticket.');return false;")
            Else
                CType(e.Row.Cells(gvTicket.Columns.Count - 2).FindControl("linkUpdate"), LinkButton).Attributes.Add("onclick", "return confirm('Do you want to update?');")
            End If

            'Summary
            ViewState("sumQuan96G") += Double.Parse(e.Row.DataItem("quan96G"))
            ViewState("sumQuan96") += Double.Parse(e.Row.DataItem("quan96"))
            ViewState("sumQuan99") += Double.Parse(e.Row.DataItem("quan99"))
            ViewState("sumPrice") += Double.Parse(e.Row.DataItem("price"))
            ViewState("sumAmount") += Double.Parse(e.Row.DataItem("amount"))

            'receipt
            CType(e.Row.FindControl("imgDelReceipt"), ImageButton).CommandArgument = e.Row.DataItem("run_no").ToString
            CType(e.Row.FindControl("imgDelReceipt"), ImageButton).OnClientClick = "return confirm(' Do you want to Cancel this Receipt?');"
            If e.Row.DataItem("run_no").ToString = "" Then
                CType(e.Row.FindControl("imgDelReceipt"), ImageButton).Visible = False
            Else
                If e.Row.DataItem("sp_quan") IsNot DBNull.Value Then
                    CType(e.Row.FindControl("lblReceipt"), Label).Text = e.Row.DataItem("run_no").ToString
                Else
                    If ViewState(clsDB.roles.print) IsNot Nothing AndAlso ViewState(clsDB.roles.print) = False Then
                        CType(e.Row.FindControl("lblReceipt"), Label).Text = e.Row.DataItem("run_no").ToString
                    Else
                        If e.Row.DataItem("billing").ToString = clsManage.yes Then
                            CType(e.Row.FindControl("lblReceipt"), Label).Text = String.Format("<a href='{1}' target='_blank'>{0}</a>", e.Row.DataItem("run_no").ToString, "report/rpt_bill.aspx?type=v&ref=" + e.Row.DataItem("ticket_id"))
                        Else
                            CType(e.Row.FindControl("lblReceipt"), Label).Text = String.Format("<a href='{1}' target='_blank'>{0}</a>", e.Row.DataItem("run_no").ToString, "report/rpt_nobill.aspx?type=v&ref=" + e.Row.DataItem("ticket_id"))
                        End If
                    End If

                End If
            End If

            'split bill แล้วลบไม่ได้ ให้ไปลบข้างใน
            If e.Row.DataItem("sp_quan") IsNot DBNull.Value Then
                CType(e.Row.FindControl("imgDelReceipt"), ImageButton).Visible = False
            End If

            If ViewState(clsDB.roles.print) IsNot Nothing AndAlso ViewState(clsDB.roles.print) = False Then
                CType(e.Row.FindControl("lblBill"), Label).Enabled = False
            End If

            If ViewState(clsDB.roles.update) IsNot Nothing AndAlso ViewState(clsDB.roles.update) = False Then
                CType(e.Row.FindControl("linkUpdate"), LinkButton).Enabled = False
            End If

            If ViewState(clsDB.roles.delete) IsNot Nothing AndAlso ViewState(clsDB.roles.delete) = False Then
                CType(e.Row.FindControl("imgDelReceipt"), ImageButton).Enabled = False
                CType(e.Row.FindControl("imgDelReceipt"), ImageButton).ImageUrl = "~/images/i_del2.png"
            End If
        End If
    End Sub

    Protected Sub linkRptSell_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkRptSell.Click
        Try
            Dim strRefno As String = ""
            Dim cust_id As String = ""
            Dim type As String = ""
            Dim billing As String = ""
            Dim trade As String = ""
            For Each dr As GridViewRow In gvTicket.Rows
                If CType(dr.Cells(gvTicket.Columns.Count - 1).FindControl("cbRow"), HtmlInputCheckBox).Checked = True Then
                    If strRefno = "" Then
                        strRefno = gvTicket.DataKeys(dr.RowIndex).Value
                        cust_id = gvTicket.DataKeys(dr.RowIndex)(1).ToString 'gvTicket.Rows(dr.RowIndex).Cells(colCustId).Text
                        type = gvTicket.DataKeys(dr.RowIndex)(2).ToString
                        billing = CType(gvTicket.Rows(dr.RowIndex).FindControl("hdfBill"), HiddenField).Value
                        trade = gvTicket.DataKeys(dr.RowIndex)("trade").ToString
                        If trade = clsManage.yes Then
                            If billing = "" Then clsManage.alert(Page, "โปรดเลือกประเภทของบิล") : Exit Sub
                        End If
                    Else
                        strRefno += "," + gvTicket.DataKeys(dr.RowIndex).Value
                        If gvTicket.DataKeys(dr.RowIndex)(1).ToString <> cust_id Then
                            'case เลือก cust_id ไม่เหมือนกัน
                            clsManage.alert(Page, "Please Select Same Customer Ref No.") : Exit Sub
                        End If

                        If billing = clsManage.yes Then
                            clsManage.alert(Page, "Ticket แบบมีบิลเลือกได้เพียงอันเดียว") : Exit Sub
                        End If

                        If CType(gvTicket.Rows(dr.RowIndex).FindControl("hdfBill"), HiddenField).Value <> billing Then
                            'case เลือก billing ไม่เหมือนกัน
                            clsManage.alert(Page, "โปรดเลือกบิลประเภทเดียวกัน") : Exit Sub
                        End If

                    End If
                End If
            Next
            If strRefno <> "" Then
                Dim msgReceipt As String = clsFng.checkReceiptNoAndSplitBill1(strRefno, billing)
                If msgReceipt <> "" Then
                    clsManage.alert(Page, msgReceipt)
                    btnSearchAdv_Click(Nothing, Nothing)
                    Exit Sub
                End If
                Dim url As String = ""
                If billing = clsManage.yes Then
                    url = "report/rpt_bill.aspx?type=e&ref=" + strRefno + ""
                Else
                    url = "report/rpt_nobill.aspx?type=e&ref=" + strRefno + ""
                End If
                clsManage.Script(Page, "window.open('" + url + "','_blank');")
                ' clsManage.Script(Page, "window.open('" + url + "','_blank','resizable=yes,scrollbars=yes,toolbar=no,titlebar=no,location=no,directories=no,status=no,menubar=no,copyhistory=no');")
            End If

        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Protected Sub imgSplit_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Try
            Dim i As Integer = CType(CType(sender, ImageButton).Parent.Parent, GridViewRow).RowIndex
            Dim id As String = gvTicket.DataKeys(i).Value
            Dim trade As String = gvTicket.DataKeys(i)("trade").ToString
            Dim billing As String = CType(gvTicket.Rows(i).FindControl("hdfBill"), HiddenField).Value

            'check ticket tradeonline must select bill
            If trade = clsManage.yes And billing = "" Then
                clsManage.alert(Page, "โปรดเลือกประเภทของบิล") : Exit Sub
            End If

            'Billing no split
            If billing = clsManage.yes Then
                clsManage.alert(Page, "Ticket แบบมีบิลไม่สามารถแยกบิลได้") : Exit Sub
            End If

            'ออกบิลแล้ว แยกบิลไม่ได้
            If CType(gvTicket.Rows(i).FindControl("imgDelReceipt"), ImageButton).Visible = True Then
                clsManage.alert(Page, "Ticket มีการออกบิลไปแล้ว") : Exit Sub
            End If

            clsManage.Script(Page, "window.open('ticket_split_bill.aspx?id=" + id + "','_blank');")
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try

    End Sub

    Protected Sub linkUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim i As Integer = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow).RowIndex
            Dim before_status As String = CType(gvTicket.Rows(i).FindControl("hdfBefore_status"), HiddenField).Value

            Dim ticket_ref As String = gvTicket.DataKeys(i).Value
            Dim status_id As String = CType(gvTicket.Rows(i).FindControl(CType(sender, LinkButton).CommandArgument), HtmlSelect).Items(CType(gvTicket.Rows(i).FindControl(CType(sender, LinkButton).CommandArgument), HtmlSelect).SelectedIndex).Value

            '************validate Status
            If before_status = status_id Then Exit Sub
            If before_status = "901" Or before_status = "902" Or before_status = "903" Then
                If status_id <> "101" Then
                    clsManage.alert(Page, "Can not Change to this Status.")
                    Exit Sub
                End If
            End If

            If before_status = "104" Or before_status = "105" Then
                If status_id = "102" Or status_id = "902" Or status_id = "903" Then
                    clsManage.alert(Page, "Can not Change to this Status.")
                    Exit Sub
                End If
            End If

            'check Payment
            If gvTicket.Rows(i).Cells(6).Text = "" Or gvTicket.Rows(i).Cells(6).Text = "&nbsp;" Then
                clsManage.alert(Page, "Please Update Payment in Ticket.")
                Exit Sub
            End If

            ''check Payment (don't cheque)
            'If status_id = "901" Or status_id = "902" Or status_id = "903" Then
            '    If gvTicket.Rows(i).Cells(7).Text = "cheq" Then
            '        clsManage.alert(Page, "Please Update Cheque Payment in Tickets.")
            '        Exit Sub
            '    End If
            'End If
            '************validate Status

            If status_id = "903" Then ' check limit customer'gold
                Dim goldLimit As String = clsManage.checkExcessShort(gvTicket.DataKeys(i).Item("cust_id").ToString, gvTicket.DataKeys(i).Item("quantity").ToString, gvTicket.DataKeys(i).Item("gold_type_id").ToString)
                If goldLimit <> "" Then
                    clsManage.alert(Page, goldLimit) : Exit Sub
                End If
            End If

            Dim act As New clsActual
            '********** Acutual
            CType(gvTicket.Rows(i).FindControl("hdfBefore_status"), HiddenField).Value = status_id
            Dim status_name As String = ""
            Dim order_type As String = gvTicket.DataKeys(i).Item("type").ToString
            Dim update_gold As String = "y"
            Dim update_cash As String = "y"
            Dim cash As String = "n"
            Dim note As String = ""
            Dim type As String = "" 'only "In" or "Out"
            Dim payment As String = gvTicket.DataKeys(i).Item("payment").ToString

            If order_type = "sell" Then
                type = "Out"
            ElseIf order_type = "buy" Then
                type = "In"
            End If

            'Complete
            If before_status = "101" And status_id = "901" Then
                status_name = "complete ส่งมอบ"
            ElseIf before_status = "101" And status_id = "902" Then
                status_name = "complete ฝาก"
                type = "ฝากทอง(ticket)"
            ElseIf before_status = "101" And status_id = "903" Then
                status_name = "complete ตัดทอง"
                type = "ตัดทองฝาก"
            ElseIf before_status = "101" And status_id = "104" Then
                status_name = "ทองออก รอเงิน"
                update_cash = "n"
                update_gold = "y"
            ElseIf before_status = "101" And status_id = "105" Then
                status_name = "เงินออก รอทอง"
                update_cash = "y"
                update_gold = "n"

                'Cancel
            ElseIf before_status = "901" And status_id = "101" Then
                status_name = "ยกเลิก complete ส่งมอบ"
                update_cash = "y"
                If type = "Out" Then type = "In" Else type = "Out"
            ElseIf before_status = "902" And status_id = "101" Then
                status_name = "ยกเลิก complete ฝาก"
                type = "ยกเลิกฝากทอง"
            ElseIf before_status = "903" And status_id = "101" Then
                status_name = "ยกเลิก complete ตัดทอง"
                type = "ยกเลิกตัดทองฝาก"
            ElseIf before_status = "104" And status_id = "101" Then
                status_name = "ยกเลิก ทองออก รอเงิน"
                type = "In"
                cash = "n"
                update_cash = "n"
                update_gold = "y"
            ElseIf before_status = "105" And status_id = "101" Then
                status_name = "ยกเลิก เงินออก รอทอง"
                cash = "y"
                update_gold = "n"
                update_cash = "y"
            ElseIf before_status = "104" And status_id = "901" Then
                status_name = "complete ส่งมอบ"
                type = "In"
                update_cash = "n"
                update_gold = "n"
                cash = "y"
            ElseIf before_status = "105" And status_id = "901" Then
                status_name = "complete ส่งมอบ"
                type = "In"
                update_cash = "n"
                update_gold = "y"
                cash = "n"
            End If

            If status_name <> "" And order_type <> "" Then
                'clsDB.insert_actual2("", ticket_ref, Session(clsManage.iSession.user_id_center.ToString).ToString, gvTicket.DataKeys(i).Item("gold_type_id").ToString, _
                '                    gvTicket.DataKeys(i).Item("quantity").ToString, gvTicket.DataKeys(i).Item("amount").ToString, status_id, _
                '                    status_name, type, _
                '                    order_type, gvTicket.DataKeys(i).Item("cust_id").ToString, before_status, note, update_cash, update_gold, cash, payment)
                act.asset_id = ""
                act.ref_no = ticket_ref
                act.created_by = Session(clsManage.iSession.user_id_center.ToString).ToString
                act.purity = gvTicket.DataKeys(i).Item("gold_type_id").ToString
                act.quantity = gvTicket.DataKeys(i).Item("quantity").ToString
                act.amount = gvTicket.DataKeys(i).Item("amount").ToString
                act.status_id = status_id
                act.status_name = status_name
                act.type = type
                act.order_type = order_type
                act.cust_id = gvTicket.DataKeys(i).Item("cust_id").ToString
                act.before_status_id = before_status
                act.note = note
                act.update_cash = update_cash
                act.update_gold = update_gold
                act.cash = cash
                act.payment = payment

            End If

            'case update status มีทั้งหมด 12 case
            '101 > 901
            '101 > 902
            '101 > 903
            '101 > 104
            '101 > 105
            '104 > 901 , 104 > 101
            '105 > 901 , 105 > 101

            '901 > 101
            '902 > 101
            '903 > 101

            '********** Acutual
            Dim result As Integer = clsDB.updateTicket_status(ticket_ref, status_id, Session(clsManage.iSession.user_id_center.ToString).ToString, gvTicket.DataKeys(i).Item("type").ToString, _
                                                              gvTicket.DataKeys(i).Item("gold_type_id").ToString, gvTicket.DataKeys(i).Item("quantity").ToString, _
                                                              gvTicket.DataKeys(i).Item("amount").ToString, gvTicket.DataKeys(i).Item("cust_id").ToString, gvTicket.DataKeys(i).Item("gold_dep").ToString, _
                                                              gvTicket.DataKeys(i).Item("payment").ToString, clsManage.convert2zero(gvTicket.DataKeys(i).Item("price").ToString), before_status, act)
            If result > 0 Then
                If status_id = "901" Or status_id = "101" Or status_id = "104" Or status_id = "105" Then
                    clsManage.alert(Page, "Update Complete.")
                ElseIf status_id = "902" Or status_id = "903" Then
                    clsManage.Script(Page, "alert('Update Complete.');window.open('customer_trans.aspx?cust_id=" + gvTicket.DataKeys(i).Item("cust_id").ToString + "');")
                End If
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "error")
        End Try
    End Sub

    Protected Sub linkExcel_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkExcel.Click
        If ViewState("drv") Is Nothing Then Exit Sub
        Dim dt As New Data.DataTable
        'dt = clsDB.getTicketStock(txtTicketRef.Text, txtCustName.Text, rdoType.SelectedValue, rdoBilling.SelectedValue, ddlStatus.SelectedValue, txtDate1.Text, txtDate2.Text, txtDelDate1.Text, txtDelDate2.Text, rdoCenter.SelectedValue)
        dt = ViewState("drv")
        If dt.Rows.Count > 0 Then
            Dim htSum As New Hashtable

            'For change status_id to status_name
            For Each dr As Data.DataRow In dt.Rows
                dr("status_id") = ddlStatus.Items.FindByValue(dr("status_id").ToString).Text
            Next

            htSum.Add("Quan96G", ViewState("sumQuan96G"))
            htSum.Add("Quan96", ViewState("sumQuan96"))
            htSum.Add("Quan99", ViewState("sumQuan99"))
            htSum.Add("Price", ViewState("sumPrice"))
            htSum.Add("Amount", ViewState("sumAmount"))

            clsManage.stockTicketsExportToExcel(dt, "Stock_ticker" + txtDate1.Text, htSum)
        End If
    End Sub

#Region "BILL"

    Protected Sub linkBillEdit_Click(sender As Object, e As System.EventArgs)
        Try
            CType(sender, LinkButton).Visible = False
            Dim i As Integer = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow).RowIndex
            CType(gvTicket.Rows(i).FindControl("ddlBill"), DropDownList).Visible = True
            CType(gvTicket.Rows(i).FindControl("linkBillSave"), LinkButton).Visible = True
            CType(gvTicket.Rows(i).FindControl("linkBillCancel"), LinkButton).Visible = True
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Protected Sub linkBillSave_Click(sender As Object, e As System.EventArgs)
        Try
            Dim i As Integer = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow).RowIndex
            If CType(gvTicket.Rows(i).FindControl("ddlBill"), DropDownList).SelectedValue = "" Then
                clsManage.alert(Page, "Please Select Bill Type.", , , "warning") : Exit Sub
            End If

            Dim id As String = gvTicket.DataKeys(i).Value
            Dim type As String = gvTicket.DataKeys(i).Item("type").ToString
            Dim billing As String = CType(gvTicket.Rows(i).FindControl("ddlBill"), DropDownList).SelectedValue

            'For status bill yes or no
            CType(gvTicket.Rows(i).FindControl("hdfBill"), HiddenField).Value = billing

            'set default
            CType(gvTicket.Rows(i).FindControl("lblBill"), Label).Visible = True
            CType(gvTicket.Rows(i).FindControl("lblBill"), Label).Text = String.Format("<a href='{1}' target='_blank'>{0}</a>", IIf(billing = "y", "Yes", "No"), "ticket_online.aspx?type=" + type + "&billing=" + billing + "&id=" + id + "&t=v")
            CType(gvTicket.Rows(i).FindControl("ddlBill"), DropDownList).Visible = False
            CType(gvTicket.Rows(i).FindControl("linkBillSave"), LinkButton).Visible = False
            CType(gvTicket.Rows(i).FindControl("linkBillCancel"), LinkButton).Visible = False

            clsManage.Script(Page, "window.open('ticket_online.aspx?type=" + type + "&billing=" + billing + "&id=" + id + "&t=n','_blank');")

        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Protected Sub linkBillCancel_Click(sender As Object, e As System.EventArgs)
        Try
            Dim i As Integer = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow).RowIndex
            CType(gvTicket.Rows(i).FindControl("ddlBill"), DropDownList).Visible = False
            CType(gvTicket.Rows(i).FindControl("linkBillSave"), LinkButton).Visible = False
            CType(gvTicket.Rows(i).FindControl("linkBillEdit"), LinkButton).Visible = True
            CType(sender, LinkButton).Visible = False
        Catch ex As Exception

        End Try
    End Sub

#End Region

    Protected Sub linkReviewDoc_Click(sender As Object, e As System.EventArgs) Handles linkReviewDoc.Click
        Try
            Dim strRefno As String = ""
            Dim cust_id As String = ""
            Dim comma As String = ","

            For Each dr As GridViewRow In gvTicket.Rows
                If CType(dr.Cells(gvTicket.Columns.Count - 1).FindControl("cbRow"), HtmlInputCheckBox).Checked = True Then
                    If strRefno = "" Then
                        strRefno = gvTicket.DataKeys(dr.RowIndex).Value
                        cust_id = gvTicket.DataKeys(dr.RowIndex)(1).ToString
                    Else
                        strRefno += comma + gvTicket.DataKeys(dr.RowIndex).Value
                        If gvTicket.DataKeys(dr.RowIndex)(1).ToString <> cust_id Then
                            'case เลือก cust_id ไม่เหมือนกัน
                            clsManage.alert(Page, "Please Select Same Customer Ref No.") : Exit Sub
                        End If
                    End If
                End If
            Next
            If strRefno <> "" Then
                clsManage.Script(Page, "window.open('report/rpt_preview.aspx?ref=" + strRefno + "','_blank');")
            End If

        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Protected Sub imgDelReceipt_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs)
        Try
            Dim receipt_no As String = CType(sender, ImageButton).CommandArgument
            If clsFng.updateReceiptNo(receipt_no) Then
                btnSearchAdv_Click(Nothing, Nothing)
            End If

        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

End Class
