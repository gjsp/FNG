
Partial Class customer_trans
    Inherits basePage

    Protected Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        If Not Page.IsPostBack Then
            Try
                If Session(clsManage.iSession.user_role_center.ToString) IsNot Nothing Then
                    Dim dtRole As New Data.DataTable : dtRole = Session(clsManage.iSession.user_role_center.ToString)
                    If dtRole.Rows.Find(Me.Page.Page.ToString) IsNot Nothing Then
                        Dim dr As Data.DataRow = dtRole.Rows.Find(Me.Page.Page.ToString)
                        If dr(clsDB.roles.update) = False Then
                            ViewState(clsDB.roles.update) = False
                        End If
                        If dr(clsDB.roles.add) = False Then
                            btnSave1.Enabled = False
                            btnSave2.Enabled = False
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
            
            clsDB.getBank(ddlBank)
            txtDuedate_CalendarExtender.Format = clsManage.formatDateTime

            If Request.QueryString("cust_id") IsNot Nothing Then
                hdfCust_id.Value = Request.QueryString("cust_id").ToString
                btnSearch_Click(Nothing, Nothing)
            Else
                hdfCust_id.Value = ""
            End If
            gvTrans.EmptyDataText = clsManage.EmptyDataText
            txtDate.Text = DateTime.Now.ToString(clsManage.formatDateTime)
            txtAmount.Attributes.Add("onkeypress", "checkNumber();")
            txtDate.Attributes.Add("onkeypress", "return false;")
            txtDate_CalendarExtender.Format = clsManage.formatDateTime
            objSrcTrans.SelectParameters("transCash").DefaultValue = ""
            gvTrans.DataBind()

            gvTrans2.EmptyDataText = clsManage.EmptyDataText
            txtDate2.Text = DateTime.Now.ToString(clsManage.formatDateTime)
            txtQuan.Attributes.Add("onkeypress", "checkNumber();")
            txtDate2.Attributes.Add("onkeypress", "return false;")
            txtDate2_CalendarExtender.Format = clsManage.formatDateTime
            gvTrans2.DataBind()
            tcTrans.ActiveTabIndex = 1
        End If
    End Sub

    Protected Sub btnSave1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave1.Click
        '"Cash"
        If Not validatetion() Then Exit Sub

        Dim bank As String = ""
        Dim duedate As Object = Nothing
        Dim cheq_no As String = ""

        If ddlPayment.SelectedValue <> clsManage.payment.cash.ToString Then
            bank = ddlBank.SelectedValue
        End If
        If ddlPayment.SelectedValue = clsManage.payment.cheq.ToString Then
            If txtDuedate.Text <> "" Then
                Try
                    duedate = DateTime.ParseExact(txtDuedate.Text, clsManage.formatDateTime, Nothing)
                Catch ex As Exception
                    clsManage.alert(Page, "Invalid Date Time.", txtDuedate.ClientID)
                End Try
            End If
            cheq_no = txtCheq.Text
        End If

        Dim da As New dsTableAdapters.customer_transTableAdapter
        Dim pre As String = "n"
        If cbPre.Checked = True Then pre = "y"
        Dim result As Integer = da.InsertQuery(hdfCust_id.Value, DateTime.ParseExact(txtDate.Text, clsManage.formatDateTime, Nothing), rdoType.SelectedValue, txtBillNo.Text, "", 0, txtAmount.Text, 0, txtRemark.Text, Session(clsManage.iSession.user_id_center.ToString).ToString, "", pre, txtRef_no.Text, ddlPayment.SelectedValue, bank, duedate, cheq_no)
        If result > 0 Then
            Dim type As String = ""
            If rdoType.SelectedValue = "Deposit" Then
                type = "ฝากเงิน"
            Else
                type = "ถอนเงิน"
            End If

            clsDB.insert_actual2("", "", Session(clsManage.iSession.user_id_center.ToString).ToString, "", 0, txtAmount.Text, "999", txtRemark.Text, type, "D/W", hdfCust_id.Value, "000", "", "y", "n", "y", ddlPayment.SelectedValue)
            gvTrans.DataBind()
            btnSearch_Click(Nothing, Nothing)
            clsManage.alert(Page, "Save Complete.")
        Else
            clsManage.alert(Page, "Cannot Save Please Try Again.")
        End If
    End Sub

    Protected Sub btnSave2_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave2.Click
        If Not validatetion2() Then Exit Sub
        Dim da As New dsTableAdapters.customer_transTableAdapter
        Dim pre As String = "n"
        If cbPre2.Checked = True Then pre = "y"
        Dim result As Integer = da.InsertQuery(hdfCust_id.Value, DateTime.ParseExact(txtDate2.Text, clsManage.formatDateTime, Nothing), rdoType2.SelectedValue, "", rdoPure.SelectedValue, txtQuan.Text, 0, 0, txtRemark2.Text, Session(clsManage.iSession.user_id_center.ToString).ToString, "", pre, txtRefNo2.Text, "", "", Nothing, "")
        If result > 0 Then
            Dim type As String = ""
            If rdoType2.SelectedValue = "Deposit" Then
                type = "ฝากทอง"
            Else
                type = "ถอนทอง"
            End If
            clsDB.insert_actual2("", "", Session(clsManage.iSession.user_id_center.ToString).ToString, rdoPure.SelectedValue, txtQuan.Text, 0, "999", txtRemark2.Text, type, "D/W", hdfCust_id.Value, "000", "", "n", "y", "n", "")
            gvTrans2.DataBind()
            btnSearch_Click(Nothing, Nothing)
            clsManage.alert(Page, "Save Complete.")
        Else
            clsManage.alert(Page, "Cannot Save Please Try Again.")
        End If
    End Sub

    Function validatetion() As Boolean
        If hdfCust_id.Value = "" Then clsManage.alert(Page, clsManage.msgRequiredFill) : Return False
        If txtDate.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill) : Return False
        If rdoType.SelectedIndex = -1 Then clsManage.alert(Page, clsManage.msgRequiredFill) : Return False
        If Not IsNumeric(txtAmount.Text) Then clsManage.alert(Page, "Please Only Number.") : Return False

        Try
            DateTime.ParseExact(txtDate.Text, clsManage.formatDateTime, Nothing)

        Catch ex As Exception
            clsManage.alert(Page, "Invalid Date Time.")
            Return False
        End Try

        If rdoType.SelectedValue = "Withdraw" Then
            Dim goldLimit As String = clsManage.checkExcessShortCash(hdfCust_id.Value, txtAmount.Text)
            If goldLimit <> "" Then
                clsManage.alert(Page, goldLimit)
                Return False
            End If
        End If


        Return True
    End Function

    Function validatetion2() As Boolean
        If hdfCust_id.Value = "" Then clsManage.alert(Page, clsManage.msgRequiredFill) : Return False
        If txtDate2.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill) : Return False
        If rdoType2.SelectedIndex = -1 Then clsManage.alert(Page, clsManage.msgRequiredFill) : Return False
        If rdoPure.SelectedIndex = -1 Then clsManage.alert(Page, clsManage.msgRequiredFill) : Return False
        If txtQuan.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill) : Return False
        If Not IsNumeric(txtQuan.Text) Then clsManage.alert(Page, "Please Only Number.") : Return False
        Try
            DateTime.ParseExact(txtDate2.Text, clsManage.formatDateTime, Nothing)
        Catch ex As Exception
            clsManage.alert(Page, "Invalid Date Time.")
            Return False
        End Try

        If rdoType2.SelectedValue = "Withdraw" Then
            Dim goldLimit As String = clsManage.checkExcessShort(hdfCust_id.Value, txtQuan.Text, rdoPure.SelectedValue, "Withdraw")
            If goldLimit <> "" Then
                clsManage.alert(Page, goldLimit)
                Return False
            End If
        End If

        Return True
    End Function

    Protected Sub imgDel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim da As New dsTableAdapters.customer_transTableAdapter
        Dim i As Integer = CType(CType(sender, ImageButton).Parent.Parent, GridViewRow).RowIndex
        Dim result As Integer = da.DeleteQuery(gvTrans.DataKeys(i).Value)
        If result > 0 Then
            btnSearch_Click(Nothing, Nothing)
            gvTrans.DataBind()
        End If
    End Sub

    Protected Sub gvTrans_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTrans.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.Cells(gvTrans.Columns.Count - 1).FindControl("imgDel"), ImageButton).Attributes.Add("onclick", "return confirm('" + clsManage.msgDel + "');")

            If Not IsDBNull(e.Row.DataItem("ref_no")) AndAlso e.Row.DataItem("ref_no").ToString <> "" Then
                If e.Row.DataItem("ref_no").ToString.Length = 11 And e.Row.DataItem("ref_no").ToString.Substring(0, 1) = "T" Then
                    CType(e.Row.FindControl("link"), HyperLink).NavigateUrl = "ticket_deal.aspx?page=dialy&id=" + e.Row.DataItem("ref_no").ToString
                End If
            End If

            If e.Row.DataItem("amount") = 0 Then
                CType(e.Row.FindControl("linkReport"), ImageButton).Enabled = False
            End If

            'cash = Amount
            CType(e.Row.FindControl("txtAmount"), TextBox).Attributes.Add("style", "text-align:right")
            CType(e.Row.FindControl("txtAmount"), TextBox).Text = e.Row.DataItem("amount").ToString
            CType(e.Row.FindControl("lblAmount"), Label).Text = clsManage.convert2Currency(e.Row.DataItem("amount").ToString)

            If ViewState(clsDB.roles.update) IsNot Nothing AndAlso ViewState(clsDB.roles.update) = False Then
                CType(e.Row.FindControl("linkUpdate"), LinkButton).Enabled = False
            End If
        End If
    End Sub

    Protected Sub gvTrans2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTrans2.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.Cells(gvTrans.Columns.Count - 1).FindControl("imgDel2"), ImageButton).Attributes.Add("onclick", "return confirm('" + clsManage.msgDel + "');")
            'CType(e.Row.FindControl("link"), HyperLink).NavigateUrl = "ticket_deal.aspx?page=dialy&id=" + e.Row.DataItem("ticket_refno").ToString
            'e.Row.Cells(0).Text = String.Format("<a href='{1}'>{0}</a>", e.Row.Cells(1).Text, "")
            'CType(e.Row.FindControl("lblTrans"), LinkButton).Text = CDate(e.Row.DataItem("datetime")).ToString("dd/MM/yyyy")
            'gold = Quantity
            CType(e.Row.FindControl("txtAmount"), TextBox).Attributes.Add("style", "text-align:right")
            CType(e.Row.FindControl("txtAmount"), TextBox).Text = CType(e.Row.DataItem("quantity").ToString, Double) '.ToString("###0.00000")
            CType(e.Row.FindControl("lblAmount"), Label).Text = clsManage.convert2StringNormal(e.Row.DataItem("quantity").ToString)
            If e.Row.DataItem("ticket_refno").ToString <> "" Then
                CType(e.Row.FindControl("linkUpdate"), LinkButton).Visible = False
            End If

            If ViewState(clsDB.roles.update) IsNot Nothing AndAlso ViewState(clsDB.roles.update) = False Then
                CType(e.Row.FindControl("linkUpdate"), ImageButton).Enabled = False
            End If

        End If
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        Try
            If hdfCust_id.Value = "" Then Exit Sub
            ucPortFolio1.CustID = hdfCust_id.Value
            ucPortFolio1.LoadPortFolio()
            gvTrans.DataBind()
            gvTrans2.DataBind()
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    'Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
    '    Try
    '        If hdfCust_id.Value = "" Then Exit Sub
    '        Dim dt As New Data.DataTable
    '        dt = clsDB.getCustomerByCust_id(hdfCust_id.Value)
    '        If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
    '            Dim dr As Data.DataRow = dt.Rows(0)
    '            txtCustName.Text = dr("FirstName").ToString
    '            lblCustRef.Text = dr("cust_id").ToString
    '            lblName.Text = dr("FirstName").ToString + " " + dr("lastname").ToString
    '            lblAdd.Text = dr("bill_address").ToString
    '            lblPhone.Text = dr("tel").ToString
    '            lblFax.Text = dr("fax").ToString
    '            lblMobilePhone.Text = dr("mobile").ToString
    '            lblAccBank.Text = dr("bank1").ToString
    '            lblAccNo.Text = dr("account_no1").ToString
    '            lblPerson.Text = dr("person_contact").ToString
    '            lblBranch.Text = dr("account_branch1").ToString
    '            lblAccName.Text = dr("account_name1").ToString
    '            lblMargin.Text = dr("margin").ToString
    '            lblRemark.Text = dr("remark").ToString
    '            Dim dtSumQuan As Data.DataTable
    '            dtSumQuan = clsDB.getSumQuanTicketPortfolioByCust_id(hdfCust_id.Value)
    '            If dtSumQuan.Rows.Count > 0 Then

    '                lblCash.Text = clsManage.convert2Currency(dtSumQuan.Rows(0)("cash").ToString)
    '                lblGold96.Text = clsManage.convert2Quantity(dtSumQuan.Rows(0)("gold96").ToString)
    '                lblGold99.Text = clsManage.convert2Quantity(dtSumQuan.Rows(0)("gold99").ToString)

    '                Dim bid96 As String = dtSumQuan.Rows(0)("bid96").ToString
    '                Dim bid99 As String = dtSumQuan.Rows(0)("bid99").ToString
    '                Dim ask96 As String = dtSumQuan.Rows(0)("ask96").ToString
    '                Dim ask99 As String = dtSumQuan.Rows(0)("ask99").ToString

    '                Dim sumquan96 As Double = 0
    '                Dim sumquan99 As Double = 0
    '                Dim sumAmount96 As Double = 0
    '                Dim sumAmount99 As Double = 0
    '                Dim sumAsset96 As Double = 0
    '                Dim sumAsset99 As Double = 0
    '                Dim sumCashCredit As Double = 0

    '                If dtSumQuan.Rows(0)("cash").ToString <> "" Then sumCashCredit = dtSumQuan.Rows(0)("cash").ToString
    '                If dtSumQuan.Rows(0)("quan96").ToString <> "" Then sumquan96 = dtSumQuan.Rows(0)("quan96").ToString
    '                If dtSumQuan.Rows(0)("quan99").ToString <> "" Then sumquan99 = dtSumQuan.Rows(0)("quan99").ToString
    '                If dtSumQuan.Rows(0)("amount96").ToString <> "" Then sumAmount96 = dtSumQuan.Rows(0)("amount96").ToString
    '                If dtSumQuan.Rows(0)("amount99").ToString <> "" Then sumAmount99 = dtSumQuan.Rows(0)("amount99").ToString
    '                If dtSumQuan.Rows(0)("gold96").ToString <> "" Then sumAsset96 = dtSumQuan.Rows(0)("gold96").ToString
    '                If dtSumQuan.Rows(0)("gold99").ToString <> "" Then sumAsset99 = dtSumQuan.Rows(0)("gold99").ToString

    '                lblMargin2.ForeColor = Drawing.Color.Black
    '                lblExcess.ForeColor = Drawing.Color.Black

    '                Dim cash_balance As Double = 0
    '                cash_balance = clsManage.cash_balance(sumCashCredit, sumAmount96, sumAmount99)
    '                lblCashBalance.Text = clsManage.minus_value(cash_balance)


    '                Dim revaluation As Double = clsManage.revaluation(bid96, bid99, ask96, ask99, sumquan96, sumquan99, sumAsset96, sumAsset99)
    '                If revaluation < 0 Then
    '                    lblRevaluation.Text = clsManage.minus_value(revaluation * -1)
    '                Else
    '                    lblRevaluation.Text = clsManage.minus_value(revaluation)
    '                End If
    '                lblNetEquity.Text = clsManage.minus_value(cash_balance + revaluation)

    '                Dim revaluation_margin As Double = clsManage.revaluationForMargin(bid96, bid99, ask96, ask99, sumquan96, sumquan99, sumAsset96, sumAsset99)
    '                Dim margin As Double = (Math.Abs(revaluation_margin) * -1) * (Double.Parse(dr("margin").ToString) / 100)
    '                lblMargin2.Text = margin.ToString(clsManage.formatCurrency)
    '                Dim excessShort As Double = cash_balance + revaluation + margin
    '                lblExcess.Text = excessShort.ToString(clsManage.formatCurrency)
    '                If margin < 0 Then
    '                    lblMargin2.ForeColor = Drawing.Color.Red
    '                End If
    '                If cash_balance + revaluation + margin < 0 Then
    '                    lblExcess.ForeColor = Drawing.Color.Red
    '                End If
    '            End If
    '        End If
    '        gvTrans.DataBind()
    '        gvTrans2.DataBind()
    '    Catch ex As Exception
    '        clsManage.alert(Page, ex.Message)
    '    End Try
    'End Sub

    Protected Sub imgDel2_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim da As New dsTableAdapters.customer_transTableAdapter
        Dim i As Integer = CType(CType(sender, ImageButton).Parent.Parent, GridViewRow).RowIndex
        Dim result As Integer = da.DeleteQuery(gvTrans2.DataKeys(i).Value)
        If result > 0 Then
            btnSearch_Click(Nothing, Nothing)
            gvTrans2.DataBind()
        End If
    End Sub

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
            CType(gv.Rows(i).FindControl("txtAmount"), TextBox).Text = "0" ' Chenge zero only
            CType(gv.Rows(i).FindControl("txtAmount"), TextBox).Enabled = False
            gv.Rows(i).FindControl("txtAmount").Visible = True
            gv.Rows(i).FindControl("lblAmount").Visible = False

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
            gv.Rows(i).FindControl("txtAmount").Visible = False
            gv.Rows(i).FindControl("lblAmount").Visible = True

            Dim purity As String = ""
            Dim ref_no As String = gv.Rows(i).Cells(1).Text
            Dim amount As String = "0"
            Dim quantity As String = "0"
            Dim order_type As String = "Cancel_D/W"
            Dim type As String = ""
            Dim remark As String = gv.Rows(i).Cells(6).Text
            Dim result As Boolean
            Dim IsCash As String = "n"
            Dim update_cash As String = "n"
            Dim update_gold As String = "n"
            If gv.ID = "gvTrans" Then
                'case cash purity = ""
                If gv.Rows(i).Cells(2).Text = "Deposit" Then
                    type = "ยกเลิก ฝากเงิน"

                ElseIf gv.Rows(i).Cells(2).Text = "Withdraw" Then
                    type = "ยกเลิก ถอนเงิน"
                End If
                amount = CType(gv.Rows(i).FindControl("lblAmount"), Label).Text
                result = clsDB.updateDopositWithdraw(gv.DataKeys(i).Value, CType(gv.Rows(i).FindControl("txtAmount"), TextBox).Text, 0)
                IsCash = "y"
            Else
                'Gold
                update_gold = "y"
                If gv.Rows(i).Cells(2).Text = "Deposit" Then
                    type = "ยกเลิก ฝากทอง"
                ElseIf gv.Rows(i).Cells(2).Text = "Withdraw" Then
                    type = "ยกเลิก ถอนทอง"
                End If
                purity = gv.Rows(i).Cells(4).Text
                quantity = CType(gv.Rows(i).FindControl("lblAmount"), Label).Text

                result = clsDB.updateDopositWithdraw(gv.DataKeys(i).Value, 0, CType(gv.Rows(i).FindControl("txtAmount"), TextBox).Text)
            End If

            If result Then
                clsDB.insert_actual2("", "", Session(clsManage.iSession.user_id_center.ToString).ToString, purity, quantity, amount, "997", type, type, order_type, hdfCust_id.Value, "000", "", update_cash, update_gold, IsCash, "")
                gv.DataBind()
                btnSearch_Click(Nothing, Nothing)
                clsManage.alert(Page, "Save Complete.")
            End If
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
            gv.Rows(i).FindControl("txtAmount").Visible = False
            gv.Rows(i).FindControl("lblAmount").Visible = True
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Protected Sub linkReport_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs)
        Try
            Dim i As Integer = CType(CType(sender, ImageButton).Parent.Parent, GridViewRow).RowIndex
            Dim gv As GridView = CType(CType(sender, ImageButton).Parent.Parent.Parent.Parent, GridView)
            Dim id As String = gv.DataKeys(i).Value
            Dim type As String = ""
            Dim url As String = ""
            If gv.ID = gvTrans.ID Then
                url = "report/rpt_cash.aspx"
            ElseIf gv.ID = gvTrans2.ID Then
                url = "report/rpt_transfer.aspx"
            End If
            clsManage.Script(Page, "window.open('" + url + "?ref=" + id + "','_blank');")
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub
End Class
