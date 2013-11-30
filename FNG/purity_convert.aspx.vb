
Partial Class purity_convert
    Inherits basePage

    Protected Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        If Not Page.IsPostBack Then
            Try
                If Session(clsManage.iSession.user_role_center.ToString) IsNot Nothing Then
                    Dim dtRole As New Data.DataTable : dtRole = Session(clsManage.iSession.user_role_center.ToString)
                    If dtRole.Rows.Find(Me.Page.Page.ToString) IsNot Nothing Then
                        Dim dr As Data.DataRow = dtRole.Rows.Find(Me.Page.Page.ToString)
                        If dr(clsDB.roles.update) = False Then
                            btnSave.Enabled = False
                            btnCV.Enabled = False
                        End If

                    End If
                End If
            Catch ex As Exception
                clsManage.alert(Page, ex.Message)
            End Try
        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            txtQuan.Attributes.Add("style", "text-align:right")
            ddlPurityFrom.Attributes.Add("onchange", "initialResult();")
            ddlPurityTo.Attributes.Add("onchange", "initialResult();")
            txtQuan.Attributes.Add("onkeypress", "checkNumber();")
            pn.Visible = False
            btnSave.Enabled = False
            btnSave.Attributes.Add("onclick", String.Format("return confirm('{0}');", "Do you want to Save?"))
        End If
    End Sub

    Protected Sub btnSearch_Click(sender As Object, e As System.EventArgs) Handles btnSearch.Click
        Try
            If hdfCust_id.Value = "" Then Exit Sub
            ucPortFolio1.CustID = hdfCust_id.Value
            ucPortFolio1.LoadPortFolio()
            pn.Visible = True
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Protected Sub btnCV_Click(sender As Object, e As System.EventArgs) Handles btnCV.Click
        Try
            lblResult.Text = "" : btnSave.Enabled = False
            If txtQuan.Text.Trim = "" Then clsManage.alert(Page, "โปรดใส่จำนวนทอง", txtQuan.ClientID) : Exit Sub
            If Not IsNumeric(txtQuan.Text) Then clsManage.alert(Page, "โปรดใส่จำนวนทองที่เป็นเฉพาะตัวเลข", txtQuan.ClientID) : Exit Sub
            If ddlPurityFrom.SelectedIndex = ddlPurityTo.SelectedIndex Then clsManage.alert(Page, "โปรดเลือกประเภททองที่ต่างกัน", txtQuan.ClientID) : Exit Sub


            Dim result As Double = 0
            Dim quan As Double = txtQuan.Text
            Dim kunG As Double = 15.244
            Dim kunM As Double = 1
            Dim kunB As Double = 1
            Dim caseConvert As String = ddlPurityFrom.SelectedValue + ddlPurityTo.SelectedValue

            Select Case caseConvert
                Case "9696G"
                    result = quan * kunG
                Case "96M96G"
                    result = quan * kunG
                Case "96M96"
                    result = quan * kunB
                Case "9696M"
                    result = quan * kunM
                Case Else
                    clsManage.alert(Page, "ไม่สามารถเปลี่ยนประเภททองนี้ได้", txtQuan.ClientID) : Exit Sub
            End Select
            lblResult.Text = result
            btnSave.Enabled = True
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, "err")
        End Try
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        Try
            Dim purityFrom As String = ddlPurityFrom.SelectedValue
            Dim purityTo As String = ddlPurityTo.SelectedValue
            Dim quan As Double = txtQuan.Text
            Dim quan_cust As Double = 0

            If purityFrom = clsFng.p96 Then
                quan_cust = clsManage.convert2zero(ucPortFolio1.Gold96)
            ElseIf purityFrom = clsFng.p96m Then
                quan_cust = clsManage.convert2zero(ucPortFolio1.Gold96m)
            ElseIf purityFrom = clsFng.p96g Then
                quan_cust = clsManage.convert2zero(ucPortFolio1.Gold96g)
            End If
            If quan > quan_cust Then
                clsManage.alert(Page, "ทองฝากมีไม่พอทำรายการ") : Exit Sub
            End If

            Dim dc As New dcDBDataContext
            Dim ctn_wd As New customer_tran
            With ctn_wd
                .cust_id = hdfCust_id.Value
                .datetime = DateTime.Now
                .type = clsFng.wd
                .gold_type_id = purityFrom
                .quantity = txtQuan.Text
                .amount = 0
                .price = 0
                .remark = "Convert Withdraw"
                .ticket_refno = ""
                .created_by = Session(clsManage.iSession.user_id_center.ToString).ToString
                .created_date = DateTime.Now
                .pre = "n"
            End With

            Dim ctn_dps As New customer_tran
            With ctn_dps
                .cust_id = hdfCust_id.Value
                .datetime = DateTime.Now
                .type = clsFng.dps
                .gold_type_id = purityTo
                .quantity = lblResult.Text
                .amount = 0
                .price = 0
                .remark = "Convert Deposit"
                .ticket_refno = ""
                .created_by = Session(clsManage.iSession.user_id_center.ToString).ToString
                .created_date = DateTime.Now
                .pre = "n"
            End With

            'Dim dt_stock As New Data.DataTable
            'dt_stock = clsDB.getStockSumDeposit()
            'Dim price As Double = clsManage.convert2zero(dt_stock.Rows(0)("priceDep"))
            'Dim G96 As Double = clsManage.convert2zero(dt_stock.Rows(0)("G96Dep"))
            'Dim G99N As Double = clsManage.convert2zero(dt_stock.Rows(0)("G99NDep"))
            'Dim G99L As Double = clsManage.convert2zero(dt_stock.Rows(0)("G99LDep"))
            'Dim cash_stock As Double = clsManage.convert2zero(dt_stock.Rows(0)("cashDep"))
            'Dim trans_stock As Double = clsManage.convert2zero(dt_stock.Rows(0)("transDep"))
            'Dim cheq_stock As Double = clsManage.convert2zero(dt_stock.Rows(0)("cheqDep"))

            'Dim act_wd As New actual
            'With act_wd
            '    .asset_type = "Gold"
            '    .created_by = Session(clsManage.iSession.user_id_center.ToString).ToString
            '    .purity = purityFrom.ToUpper
            '    .quantity = quan
            '    .amount = 0
            '    .status_id = "999"
            '    .status_name = "Convert Withdraw"
            '    .type = "ถอนทอง"
            '    .order_type = "D/W"
            '    .cust_id = hdfCust_id.Value
            '    .before_status_id = "000"
            '    .payment = ""
            '    .datetime = DateTime.Now

            '    .price_base = price
            '    .G96_base = G96
            '    .G99L_base = G99N
            '    .G99L_base = G99L
            '    .cash = cash_stock
            '    .trans = trans_stock
            '    .cheq = cheq_stock
            'End With
            'Dim act_dps As New actual
            'With act_dps
            '    .asset_type = "Gold"
            '    .created_by = Session(clsManage.iSession.user_id_center.ToString).ToString
            '    .purity = purityTo.ToUpper
            '    .quantity = quan
            '    .amount = 0
            '    .status_id = "999"
            '    .status_name = "Convert Deposit"
            '    .type = "ฝากทอง"
            '    .order_type = "D/W"
            '    .cust_id = hdfCust_id.Value
            '    .before_status_id = "000"
            '    .payment = ""
            '    .datetime = DateTime.Now
            'End With

            dc.Connection.Open()
            dc.Transaction = dc.Connection.BeginTransaction

            dc.customer_trans.InsertOnSubmit(ctn_wd)
            dc.customer_trans.InsertOnSubmit(ctn_dps)
            'dc.actuals.InsertOnSubmit(act_wd)
            'dc.actuals.InsertOnSubmit(act_dps)

            dc.SubmitChanges()

            dc.Transaction.Commit()
            dc.Connection.Close()

            ucPortFolio1.CustID = hdfCust_id.Value
            ucPortFolio1.LoadPortFolio()
            btnSave.Enabled = False
            clsManage.alert(Page, "Save Complete")
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, "err")
        End Try
    End Sub

    Protected Sub upgSearch_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles upgSearch.Load
        System.Threading.Thread.Sleep(upgSearch.DisplayAfter)
    End Sub
End Class
