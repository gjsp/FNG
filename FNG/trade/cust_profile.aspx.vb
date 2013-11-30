
Partial Class cust_profile
    Inherits System.Web.UI.Page

    Dim colPrice As Integer = 7
    Dim colQuan As Integer = 8
    Dim colAmount As Integer = 9
    Dim colPL As Integer = 12

    Dim colQuanHis As Integer = 7
    Dim colAmountHis As Integer = 8

    Dim colFootQuan As Integer = 2
    Dim colFootAmount As Integer = 3
    Dim colFootPL As Integer = 6

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        If Session(clsManage.iSession.cust_id.ToString) Is Nothing Then
            Response.Redirect("login.aspx")
        End If

        'update online time
        Dim msg = clsMain.updateTimeOnline(Session(clsManage.iSession.cust_id.ToString).ToString, Session.SessionID)
        If msg <> "" Then

            Session.Remove(clsManage.iSession.cust_id.ToString)
            Session.Remove(clsManage.iSession.user_id.ToString)
            Session.Remove(clsManage.iSession.user_name.ToString)
            Session.Remove(clsManage.iSession.cust_lv.ToString)
            Session.Remove(clsManage.iSession.role.ToString)
            Session.Remove(clsManage.iSession.first_trade.ToString)
            Session.Remove(clsManage.iSession.online_id.ToString)

            clsManage.alert(Page, msg, , "login.aspx", "loginDup")
            Exit Sub
        End If

        If Not Page.IsPostBack Then
            If Session(clsManage.iSession.cust_id.ToString) IsNot Nothing Then
                hdfCust_id.Value = Session(clsManage.iSession.cust_id.ToString).ToString
                Dim dt As New Data.DataTable
                dt = clsMain.getCustomerByCust_id(hdfCust_id.Value)
                If dt.Rows.Count > 0 Then
                    Dim dr As Data.DataRow = dt.Rows(0)
                    'Gold Movement
                    lblRefNo.Text = dr("cust_id").ToString
                    lblName.Text = dr("FirstName").ToString + " " + dr("lastname").ToString
                    lblAdd.Text = dr("bill_address").ToString
                    lblPhone.Text = dr("tel").ToString
                    lblFax.Text = dr("fax").ToString
                    lblMobilePhone.Text = dr("mobile").ToString.Remove(dr("mobile").ToString.Length - 4, 4) + "xxxx"
                    lblAccBank.Text = dr("bank1").ToString
                    lblAccNo.Text = dr("account_no1").ToString
                    lblPerson.Text = dr("person_contact").ToString
                    lblBranch.Text = dr("account_branch1").ToString
                    lblAccName.Text = dr("account_name1").ToString

                    'lblCashDep.Text = clsManage.convert2Currency(dr("cash_credit").ToString) + " บาท"
                    'lblGoldDep96.Text = clsManage.convert2Quantity(dr("quan_96").ToString) + " บาท"
                    'lblGoldDep99.Text = clsManage.convert2Quantity(dr("quan_99N").ToString) + " กิโล"

                    lblGrtCash.Text = clsManage.convert2Currency(dr("cash_credit").ToString)
                    lblGrt96.Text = clsManage.convert2Currency(dr("quan_96").ToString)
                    lblGrt99.Text = clsManage.convert2Currency(dr("quan_99N").ToString)

                    Dim tradelimit As New Dictionary(Of String, String)
                    tradelimit = clsFng.checkGuaranteeTradeCount(hdfCust_id.Value)
                    lblTradelimit96.Text = tradelimit.Keys(0).ToString
                    lblTradelimit99.Text = tradelimit.Values(0).ToString

                    Dim dtSumQuan As Data.DataTable
                    dtSumQuan = clsDB.getSumQuanTicketPortfolioByCust_id(hdfCust_id.Value)
                    If dtSumQuan.Rows.Count > 0 Then
                        '***set Property
                        ViewState("_PasswordAuthen") = dtSumQuan.Rows(0)("pwd_auth").ToString
                        ViewState("_Name") = dr("FirstName").ToString
                        ViewState("_Gold96") = clsManage.convert2Quantity(dtSumQuan.Rows(0)("gold96").ToString)
                        ViewState("_Gold96m") = clsManage.convert2Quantity(dtSumQuan.Rows(0)("gold96M").ToString)
                        ViewState("_Gold96g") = clsManage.convert2Quantity(dtSumQuan.Rows(0)("gold96G").ToString)
                        ViewState("_Gold99") = clsManage.convert2Quantity(dtSumQuan.Rows(0)("gold99").ToString)

                        lblCashDep.Text = clsManage.convert2Currency(dtSumQuan.Rows(0)("cash").ToString) + " บาท"
                        lblGoldDep96.Text = clsManage.convert2Quantity(dtSumQuan.Rows(0)("gold96").ToString) + " บาท"
                        lblGoldDep96G.Text = clsManage.convert2QuantityGram(dtSumQuan.Rows(0)("gold96G").ToString) + " กรัม"
                        lblGoldDep96M.Text = clsManage.convert2Quantity(dtSumQuan.Rows(0)("gold96M").ToString) + " บาท"
                        lblGoldDep99.Text = clsManage.convert2Quantity(dtSumQuan.Rows(0)("gold99").ToString) + " กิโล"
                    End If
                    Dim dc As New dcDBDataContext
                    Dim sumBS = dc.getSummaryBuySellByCust_id(hdfCust_id.Value).FirstOrDefault
                    If sumBS IsNot Nothing Then
                        lblSQ96.Text = clsManage.convert2Quantity(sumBS.SQ96)
                        lblSA96.Text = clsManage.convert2Quantity(sumBS.SA96)
                        lblBQ96.Text = clsManage.convert2Quantity(sumBS.BQ96)
                        lblBA96.Text = clsManage.convert2Quantity(sumBS.BA96)
                        'sum
                        lblSumQ96.Text = clsManage.convert2Quantity(sumBS.SQ96 - sumBS.BQ96)
                        lblSumA96.Text = clsManage.convert2Quantity(-sumBS.SA96 + sumBS.BA96)

                        lblSQ99.Text = clsManage.convert2Quantity(sumBS.SQ99)
                        lblSA99.Text = clsManage.convert2Quantity(sumBS.SA99)
                        lblBQ99.Text = clsManage.convert2Quantity(sumBS.BQ99)
                        lblBA99.Text = clsManage.convert2Quantity(sumBS.BA99)
                        'sum
                        lblSumQ99.Text = clsManage.convert2Quantity(sumBS.SQ99 - sumBS.BQ99)
                        lblSumA99.Text = clsManage.convert2Quantity(-sumBS.SA99 + sumBS.BA99)
                    End If

                End If
            End If
        End If
    End Sub

    Protected Sub linkPwd_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkPwd.Click
        If Session(clsManage.iSession.cust_id.ToString) IsNot Nothing Then
            If Session(clsManage.iSession.cust_id.ToString).ToString <> hdfCust_id.Value Then

                Session.Remove(clsManage.iSession.cust_id.ToString)
                Session.Remove(clsManage.iSession.user_id.ToString)
                Session.Remove(clsManage.iSession.user_name.ToString)
                Session.Remove(clsManage.iSession.cust_lv.ToString)
                Session.Remove(clsManage.iSession.role.ToString)
                Session.Remove(clsManage.iSession.first_trade.ToString)
                Session.Remove(clsManage.iSession.online_id.ToString)

                Response.Redirect("login.aspx")
                Exit Sub
            End If
        Else
            Response.Redirect("login.aspx")
            Exit Sub
        End If

        Response.Redirect("change_password.aspx?t=pwd")
    End Sub

    Protected Sub linkUser_Click(sender As Object, e As System.EventArgs) Handles linkUser.Click
        If Session(clsManage.iSession.cust_id.ToString) IsNot Nothing Then
            If Session(clsManage.iSession.cust_id.ToString).ToString <> hdfCust_id.Value Then

                Session.Remove(clsManage.iSession.cust_id.ToString)
                Session.Remove(clsManage.iSession.user_id.ToString)
                Session.Remove(clsManage.iSession.user_name.ToString)
                Session.Remove(clsManage.iSession.cust_lv.ToString)
                Session.Remove(clsManage.iSession.role.ToString)
                Session.Remove(clsManage.iSession.first_trade.ToString)
                Session.Remove(clsManage.iSession.online_id.ToString)

                Response.Redirect("login.aspx")
                Exit Sub
            End If
        Else
            Response.Redirect("login.aspx")
            Exit Sub
        End If

        Response.Redirect("change_password.aspx?t=username")
    End Sub
End Class
