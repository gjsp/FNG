
Partial Class customer_detail
    Inherits basePage
    Dim docPath As String = "doc/user_guide.doc"

    Protected Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        If Not Page.IsPostBack Then
            Try
                If Session(clsManage.iSession.user_role_center.ToString) IsNot Nothing Then
                    Dim dtRole As New Data.DataTable : dtRole = Session(clsManage.iSession.user_role_center.ToString)
                    If dtRole.Rows.Find(Me.Page.Page.ToString) IsNot Nothing Then
                        Dim dr As Data.DataRow = dtRole.Rows.Find(Me.Page.Page.ToString)
                        If dr(clsDB.roles.delete) = False Then
                            btnReset.Enabled = False
                        End If
                        If dr(clsDB.roles.update) = False Then
                            btnUpdate.Enabled = False
                        End If
                        If dr(clsDB.roles.add) = False Then
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
            txtDisBuy.Attributes.Add("style", "text-align:right;")
            txtDisSell.Attributes.Add("style", "text-align:right;")
            txtCredit_cash.Attributes.Add("style", "text-align:right;")
            txtQuan96.Attributes.Add("style", "text-align:right;")
            txtQuan99N.Attributes.Add("style", "text-align:right;")
            txtFreeMargin.Attributes.Add("style", "text-align:right;")
            clsManage.getrdoList(rdoType, clsDB.getCustomerType())
            txtQuan96.Attributes.Add("onkeypress", "checkNumber();")
            txtQuan99N.Attributes.Add("onkeypress", "checkNumber();")
            txtMobilePhone.Attributes.Add("onkeypress", "numberOnly();")
            txtIDCard.Attributes.Add("onkeypress", "numberOnly();")
            txtFreeMargin.Attributes.Add("onkeypress", "numberOnly();")
            txtDisBuy.Attributes.Add("onkeypress", "numberOnly();")
            txtDisSell.Attributes.Add("onkeypress", "numberOnly();")
            hdfPwd.Value = clsDB.getStockField("pwd_auth")

            txtCredit_cash.Text = "0"

            For i As Integer = 1 To 31 : ddlday.Items.Add(i) : Next
            For i As Integer = 1 To 12 : ddlMonth.Items.Add(i) : Next

            For i As Integer = Now.Year To 1900 Step -1 : ddlYear.Items.Add(i) : Next

            cbCreateTradeOnline.Enabled = False

            If Request.QueryString("id") IsNot Nothing Then
                btnUpdate.Visible = True : btnSave.Visible = False : btnReset.Visible = False

                Dim dt As New Data.DataTable
                dt = clsDB.getCustomerByCust_id(Request.QueryString("id").ToString())
                If dt.Rows.Count > 0 Then
                    Dim dr As Data.DataRow = dt.Rows(0)
                    If Not dr("trade_type").ToString = "Call" Then
                        btnReset.Visible = True
                        pnApprove.Visible = False
                        cbHalt.Visible = True
                        ViewState("halt") = dr("halt").ToString
                        cbHalt.Checked = IIf(dr("halt").ToString = "n", False, True)
                        ViewState("active") = dr("active").ToString
                    End If

                    hdfUserId.Value = dr("user_id").ToString
                    lblTradeType.Text = dr("trade_type").ToString
                    lblRefNo.Text = dr("cust_id").ToString
                    txtFname.Text = dr("firstname").ToString
                    txtLname.Text = dr("lastname").ToString

                    txtFnameEng.Text = dr("firstname_eng").ToString
                    txtLnameEng.Text = dr("lastname_eng").ToString
                    txtIDCard.Text = dr("id_card").ToString

                    Dim birthday As Date
                    If dr("birthday") IsNot DBNull.Value Then
                        birthday = dr("birthday")
                        ddlday.SelectedValue = birthday.Day
                        ddlMonth.SelectedValue = birthday.Month
                        ddlYear.SelectedValue = birthday.Year
                    End If

                    txtEmail.Text = dr("email").ToString
                    ddlTitle.SelectedValue = dr("titlename").ToString
                    txtPerson.Text = dr("person_contact").ToString
                    txtAddress.Text = dr("bill_address").ToString


                    txtBankName1.Text = dr("bank1").ToString
                    txtBankAccNo1.Text = dr("account_no1").ToString
                    txtBankAccName1.Text = dr("account_name1").ToString
                    txtBankAccBranch1.Text = dr("account_branch1").ToString
                    txtBankAccType1.Text = dr("account_type1").ToString

                    txtBankName2.Text = dr("bank2").ToString
                    txtBankAccNo2.Text = dr("account_no2").ToString
                    txtBankAccName2.Text = dr("account_name2").ToString
                    txtBankAccBranch2.Text = dr("account_branch2").ToString
                    txtBankAccType2.Text = dr("account_type2").ToString

                    txtBankName3.Text = dr("bank3").ToString
                    txtBankAccNo3.Text = dr("account_no3").ToString
                    txtBankAccName3.Text = dr("account_name3").ToString
                    txtBankAccBranch3.Text = dr("account_branch3").ToString
                    txtBankAccType3.Text = dr("account_type3").ToString

                    txtMobilePhone.Text = dr("mobile").ToString
                    txtTel.Text = dr("tel").ToString
                    txtFax.Text = dr("fax").ToString
                    rdoType.SelectedValue = dr("cust_type_id").ToString
                    txtRemark.Text = dr("remark").ToString
                    txtCredit_cash.Text = clsManage.convert2StringNormal(dr("cash_credit").ToString)
                   
                    txtQuan96.Text = clsManage.convert2StringNormal(dr("quan_96").ToString)
                    txtQuan99N.Text = clsManage.convert2StringNormal(dr("quan_99N").ToString)
                    txtFreeMargin.Text = clsManage.convert2StringNormal(dr("free_margin").ToString)
                    'If dr("VIP") IsNot DBNull.Value Then
                    '    cbVIP.Checked = dr("VIP")
                    '    If Not dr("VIP") Then
                    '        txtDiscount.Enabled = False
                    '    End If
                    'Else
                    '    txtDiscount.Enabled = False
                    'End If
                    txtDisBuy.Text = dr("discount_buy").ToString
                    txtDisSell.Text = dr("discount_sell").ToString

                    If Not IsDBNull(dr("margin_unlimit")) Then
                        cbUnlimitedMargin.Checked = dr("margin_unlimit")
                    End If


                End If
                pnMain.DefaultButton = btnUpdate.ID
            Else
                pnMain.DefaultButton = btnSave.ID
                btnSave.Visible = True : btnUpdate.Visible = False : btnReset.Visible = False

            End If
        End If
        If Request.QueryString("id") IsNot Nothing Then
            'clsManage.Script(Page, "trRef.style.display='block';trTrade.style.display='block';trCash.style.display='none';tr96.style.display='none';tr99N.style.display='none';trDep.style.display='block';", "displayRefNo")
            clsManage.Script(Page, "trRef.style.display='block';trTrade.style.display='block';trDep.style.display='block';trFolio.style.display='block'", "displayRefNo")
        Else
            'clsManage.Script(Page, "trRef.style.display='none';trTrade.style.display='none';trCash.style.display='block';tr96.style.display='block';tr99N.style.display='block';trDep.style.display='none';", "displayRefNo")
            clsManage.Script(Page, "trRef.style.display='none';trTrade.style.display='none';trDep.style.display='none';trFolio.style.display='none'", "displayRefNo")
        End If
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If Not validatetion() Then Exit Sub
            'check Email and id card duplicate
            Dim idCard As String = txtIDCard.Text 
            Dim checkdupEmailAndIdcard = clsDB.checkDuplicateEmailAndIdcard(txtEmail.Text, idCard)
            If checkdupEmailAndIdcard.ToString.Substring(0, 1) = "1" Then
                clsManage.alert(Page, "ID Card already have in system.") : Exit Sub
            End If
            If checkdupEmailAndIdcard.ToString.Substring(1, 1) = "1" Then
                clsManage.alert(Page, "Email already have in system.") : Exit Sub
            End If
            Dim cash_credit As Double = 0
            Dim quan96 As Double = 0
            Dim quan99 As Double = 0
            Dim quan99L As Double = 0
            Dim freeMargin As Double = IIf(txtFreeMargin.Text.Trim = "", 0, txtFreeMargin.Text)

            If txtQuan96.Text.Trim <> "" Then quan96 = txtQuan96.Text
            If txtQuan99N.Text.Trim <> "" Then quan99 = txtQuan99N.Text
            'If txtQuan99L.Text.Trim <> "" Then quan99L = txtQuan99L.Text

            If txtCredit_cash.Text.Trim <> "" Then cash_credit = txtCredit_cash.Text
            If Request.QueryString("id") Is Nothing Then
                Dim birthday As Date = DateTime.ParseExact(ddlday.SelectedValue + "/" + ddlMonth.SelectedValue + "/" + ddlYear.SelectedValue, clsManage.formatDateTime, Nothing)
                Dim result As String = clsDB.insertCustomer("", rdoType.SelectedValue, ddlTitle.SelectedValue, txtFname.Text, txtLname.Text, txtFnameEng.Text, txtLnameEng.Text, idCard, birthday, txtEmail.Text, txtPerson.Text, txtAddress.Text, _
                txtBankName1.Text, txtBankAccNo1.Text, txtBankAccName1.Text, txtBankAccType1.Text, txtBankAccBranch1.Text, _
                txtBankName2.Text, txtBankAccNo2.Text, txtBankAccName2.Text, txtBankAccType2.Text, txtBankAccBranch2.Text, _
                txtBankName3.Text, txtBankAccNo3.Text, txtBankAccName3.Text, txtBankAccType3.Text, txtBankAccBranch3.Text, _
                txtMobilePhone.Text, txtTel.Text, txtFax.Text, cash_credit, txtRemark.Text, 0, 0, quan96, quan99, Session(clsManage.iSession.user_id_center.ToString).ToString, "", _
                0, freeMargin, cbUnlimitedMargin.Checked, cbVIP.Checked, IIf(txtDisBuy.Text.Trim = "", 0, txtDisBuy.Text), IIf(txtDisSell.Text.Trim = "", 0, txtDisSell.Text))
                If result <> "" Then
                    Dim type As String = ""
                    Dim pure As String = ""
                    Dim cust_id As String = result
                    Dim isUsed As Boolean = False
                    Dim adjust_value As Double = 0

                    If cash_credit <> 0 Then
                        'ฝากเงินพร้อมลงทะเบียน เฉพาะแบบ cash
                        type = "ฝากเงิน"
                        clsDB.insert_actual2("", "", Session(clsManage.iSession.user_id_center.ToString).ToString, "", 0, cash_credit, "999", txtRemark.Text, type, "D/W", cust_id, "000", "ฝากเงินในการลงทะเบียนครั้งแรก", "y", "n", "y", "Cash")
                    End If

                    If cbCreateTradeOnline.Checked = True Then
                        Dim username As String = genUsername()
                        Dim pwd As String = clsManage.genPwd(8)
                        Dim regCode As String = Session.SessionID

                        Dim resultTrade As Integer = clsMain.InsertUsernames(username, pwd, cust_id, Session(clsManage.iSession.user_id_center.ToString).ToString, "cust", "1", regCode, "n")
                        If resultTrade > 0 Then
                            'send email to customer
                            Dim msgBody As String = String.Format("Please Click Link to Confirm Register.<br/>User Name : {0} <br/>Password : {3}<br/>" & _
                             "<a href='{2}/trade/register_confirm.aspx?u={0}&r={1}'>{2}/trade/register_confirm.aspx?u={0}&r={1}</a>", username, regCode, ConfigurationManager.AppSettings("DOMAIN_NAME").ToString, pwd)
                            clsMain.sendEmailRegisterTrade(txtEmail.Text, txtFname.Text, msgBody, , Server.MapPath(docPath))

                        End If
                    End If

                End If
                clsManage.alert(Page, "Save Complete.", , "customers.aspx")
            End If

        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Function genUsername() As String
        Dim username As String = clsManage.genPwd(8)
        Dim dt As New Data.DataTable
        dt = clsMain.getChkUsersAdd(username)

        If dt.Rows.Count > 0 Then
            Return genUsername()
        Else
            Return username
        End If
    End Function

    Function validatetion() As Boolean

        If txtFname.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill, txtFname.ClientID) : Return False
        If txtMobilePhone.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill, txtMobilePhone.ClientID) : Return False
        If txtMobilePhone.Text.Trim.Length <> Integer.Parse(clsManage.phoneDigit) Then clsManage.alert(Page, String.Format("Mobile Phone Must be Only Number {0} digit", clsManage.phoneDigit.ToString), txtMobilePhone.ClientID) : Return False
        If rdoType.SelectedIndex = -1 Then clsManage.alert(Page, "Please Select Type.") : Return False
        If txtEmail.Text.Trim = "" Then clsManage.alert(Page, clsManage.msgRequiredFill, txtEmail.ClientID) : Return False

        Dim idCard As String = txtIDCard.Text
        If idCard.Length <> 13 Then clsManage.alert(Page, clsManage.msgRequiredFill) : Return False

        If txtCredit_cash.Text.Trim <> "" AndAlso Not IsNumeric(txtCredit_cash.Text) Then clsManage.alert(Page, "Number Only.") : Return False
        If txtQuan96.Text.Trim <> "" AndAlso Not IsNumeric(txtQuan96.Text) Then clsManage.alert(Page, "Number Only.") : Return False
        If txtQuan99N.Text.Trim <> "" AndAlso Not IsNumeric(txtQuan99N.Text) Then clsManage.alert(Page, "Number Only.") : Return False
        If Regex.IsMatch(txtEmail.Text, "\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*") = False Then clsManage.alert(Page, "Email format is invalid.", txtEmail.ClientID) : Return False
       
        Return True
    End Function

    Protected Sub lblDeposit_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lblDeposit.Click
        If Request.QueryString("id") IsNot Nothing Then
            Dim cust_id As String = Request.QueryString("id").ToString
            clsManage.Script(Page, "window.open('customer_trans.aspx?cust_id=" & cust_id + "')")
        End If
    End Sub

    Protected Sub linkPortfolio_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkPortfolio.Click
        If Request.QueryString("id") IsNot Nothing Then
            Dim cust_id As String = Request.QueryString("id").ToString
            clsManage.Script(Page, "window.open('customer_portfolio.aspx?id=" & cust_id + "')")
        End If
    End Sub

    Protected Sub btnUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUpdate.Click
        Try
            If Request.QueryString("id") IsNot Nothing Then
                If Not validatetion() Then Exit Sub
                Dim cust_id = Request.QueryString("id").ToString
                Dim cash_credit As Double = 0 : If txtCredit_cash.Text.Trim <> "" Then cash_credit = txtCredit_cash.Text
                Dim quan96 As Double = 0 : If txtQuan96.Text.Trim <> "" Then quan96 = txtQuan96.Text
                Dim quan99 As Double = 0 : If txtQuan99N.Text.Trim <> "" Then quan99 = txtQuan99N.Text
                'Dim quan99L As Double = 0
                Dim tradeLimit As Double = 0
                Dim freeMargin As Double = 0 : If txtFreeMargin.Text.Trim <> "" Then freeMargin = txtFreeMargin.Text ': IIf(txtFreeMargin.Text.Trim = "", 0, txtFreeMargin.Text)

                Dim da As New dsTableAdapters.customerTableAdapter
                Dim birthday As Date = DateTime.ParseExact(ddlday.SelectedValue + "/" + ddlMonth.SelectedValue + "/" + ddlYear.SelectedValue, clsManage.formatDateTime, Nothing) ' ddlday.SelectedValue + ddlMonth.SelectedValue + ddlYear.SelectedValue

                Dim idCard As String = txtIDCard.Text
                Dim disBuy As Double = 0 : If txtDisBuy.Text.Trim <> "" Then disBuy = txtDisBuy.Text
                Dim disSell As Double = 0 : If txtDisSell.Text.Trim <> "" Then disSell = txtDisSell.Text

                Dim result As Integer = da.UpdateQuery(cust_id, "", rdoType.SelectedValue, ddlTitle.SelectedValue, txtFname.Text, txtLname.Text, txtPerson.Text, txtAddress.Text, _
                txtBankName1.Text, txtBankAccNo1.Text, txtBankAccName1.Text, txtBankAccType1.Text, txtBankAccBranch1.Text, _
                txtBankName2.Text, txtBankAccNo2.Text, txtBankAccName2.Text, txtBankAccType2.Text, txtBankAccBranch2.Text, _
                txtBankName3.Text, txtBankAccNo3.Text, txtBankAccName3.Text, txtBankAccType3.Text, txtBankAccBranch3.Text, _
                txtMobilePhone.Text, txtTel.Text, txtFax.Text, txtRemark.Text, cash_credit, 0, 0, _
                quan96, quan99, "", txtEmail.Text, txtFnameEng.Text, txtLnameEng.Text, idCard, birthday, freeMargin, tradeLimit, cbUnlimitedMargin.Checked, cbVIP.Checked, disBuy, disSell)

                If result > 0 Then

                    If cbCreateTradeOnline.Checked = True Then
                        Dim username As String = genUsername()
                        Dim pwd As String = clsManage.genPwd(8)
                        Dim regCode As String = Session.SessionID

                        Dim resultTrade As Integer = clsMain.InsertUsernames(username, pwd, cust_id, Session(clsManage.iSession.user_id_center.ToString).ToString, "cust", "1", regCode, "n")
                        If resultTrade > 0 Then
                            'send email to customer
                            Dim msgBody As String = String.Format("Please Click Link to Confirm Register.<br/>User Name : {0} <br/>Password : {3}<br/>" & _
                             "<a href='{2}/trade/register_confirm.aspx?u={0}&r={1}'>{2}/trade/register_confirm.aspx?u={0}&r={1}</a>", username, regCode, ConfigurationManager.AppSettings("DOMAIN_NAME").ToString, pwd)
                            clsMain.sendEmailRegisterTrade(txtEmail.Text, txtFname.Text, msgBody, , Server.MapPath(docPath))
                        End If
                    End If
                    'Update Halt from trade Online
                    Dim halt As String = "n"
                    If ViewState("halt") IsNot Nothing Then
                        If Not ViewState("halt").ToString = IIf(cbHalt.Checked, "y", "n") Then
                            halt = IIf(cbHalt.Checked, "y", "n")
                            clsMain.UpdateUsersHalt(hdfUserId.Value, halt)
                        End If
                    End If

                    clsManage.alert(Page, "Update Complete", , "customers.aspx")
                End If
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "error")
        End Try
    End Sub

    Protected Sub cbApprove_CheckedChanged(sender As Object, e As System.EventArgs) Handles cbApprove.CheckedChanged
        If cbApprove.Checked Then
            cbCreateTradeOnline.Enabled = True
        Else
            cbCreateTradeOnline.Enabled = False
            cbCreateTradeOnline.Checked = False
        End If

    End Sub

    Protected Sub btnReset_Click(sender As Object, e As System.EventArgs) Handles btnReset.Click
        'Send email to cusotmer
        Dim username As String = genUsername()
        Dim pwd As String = clsManage.genPwd(8)
        Dim active As String = "y"
        If ViewState("active") IsNot Nothing Then
            active = ViewState("active").ToString
        End If
        Dim regCode As String = ""
        If active = "n" Then
            regCode = Session.SessionID
        End If

        Dim result As Integer = clsMain.UpdateUsernamesReset(hdfUserId.Value, pwd, username, regCode)
        If result > 0 Then
            'No set First Trade
            btnReset.Enabled = False
            Dim msgBody As String = ""
            If active = "y" Then
                msgBody = String.Format("Reset Username and Password Complete Please Click Link to Login.<br/>User Name : {0} <br/>Password : {2}<br/>" & _
                                "<a href='{1}/trade/login.aspx'>{1}/trade/login.aspx</a>", username, ConfigurationManager.AppSettings("DOMAIN_NAME").ToString, pwd)
                clsMain.sendEmailResetUsernamePwd(txtEmail.Text, txtFname.Text, msgBody, Server.MapPath(docPath))
            Else
                msgBody = String.Format("Please Click Link to Confirm Register.<br/>User Name : {0} <br/>Password : {3}<br/>" & _
                           "<a href='{2}/trade/register_confirm.aspx?u={0}&r={1}'>{2}/trade/register_confirm.aspx?u={0}&r={1}</a>", username, regCode, ConfigurationManager.AppSettings("DOMAIN_NAME").ToString, pwd)
                clsMain.sendEmailRegisterTrade(txtEmail.Text, txtFname.Text, msgBody, , Server.MapPath(docPath))
            End If
            clsManage.alert(Page, "Reset Username Password And Send E-Mail Complete.")
        End If
    End Sub

    Protected Sub udPgs_Load(sender As Object, e As System.EventArgs) Handles udPgs.Load
        System.Threading.Thread.Sleep(udPgs.DisplayAfter + 300)
    End Sub

    'Protected Sub cbVIP_CheckedChanged(sender As Object, e As EventArgs) Handles cbVIP.CheckedChanged
    '    txtDiscount.Enabled = cbVIP.Checked
    '    txtDiscount.Text = ""
    '    If cbVIP.Checked Then
    '        txtDiscount.Focus()
    '    End If
    'End Sub
End Class
