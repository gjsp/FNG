
Partial Class user_control_ucPortFolio
    Inherits System.Web.UI.UserControl


    Public Property CustID() As String
        Get
            Return IIf(ViewState("_CustID").ToString() <> "", ViewState("_CustID").ToString(), "")
        End Get
        Set(ByVal value As String)
            ViewState("_CustID") = value
        End Set
    End Property

    Private _PasswordAuthen As String
    Public Property PasswordAuthen() As String
        Get
            Return ViewState("_PasswordAuthen").ToString
        End Get
        Set(ByVal value As String)
            ViewState("_PasswordAuthen") = value
        End Set
    End Property

    Private _Name As String
    Public Property Name() As String
        Get
            Return ViewState("_Name").ToString
        End Get
        Set(ByVal value As String)
            ViewState("_Name") = value
        End Set
    End Property

    Private _Gold96 As String
    Public Property Gold96() As String
        Get
            Return ViewState("_Gold96").ToString
        End Get
        Set(ByVal value As String)
            ViewState("_Gold96") = value
        End Set
    End Property

    Private _Gold96m As String
    Public Property Gold96m() As String
        Get
            Return ViewState("_Gold96m").ToString
        End Get
        Set(ByVal value As String)
            ViewState("_Gold96m") = value
        End Set
    End Property

    Private _Gold96g As String
    Public Property Gold96g() As String
        Get
            Return ViewState("_Gold96g").ToString
        End Get
        Set(ByVal value As String)
            ViewState("_Gold96g") = value
        End Set
    End Property

    Private _Gold99 As String
    Public Property Gold99() As String
        Get
            Return ViewState("_Gold99").ToString
        End Get
        Set(ByVal value As String)
            ViewState("_Gold99") = value
        End Set
    End Property


    Public Sub LoadPortFolio()
        Try
            If CustID = "" Then Exit Sub

            Dim db As New dcDBDataContext
            Dim ca = (From c In db.v_customer_sum_assets Where c.cust_id = CustID).FirstOrDefault
            If ca IsNot Nothing Then
                lblCustRef.Text = ca.cust_id
                lblName.Text = ca.firstname + " " + ca.lastname
                lblAdd.Text = ca.bill_address
                lblPhone.Text = ca.tel
                lblFax.Text = ca.fax
                lblMobilePhone.Text = ca.mobile
                lblAccBank.Text = ca.bank1
                lblAccNo.Text = ca.account_no1
                lblPerson.Text = ca.person_contact
                lblBranch.Text = ca.account_branch1
                lblAccName.Text = ca.account_name1
                lblRemark.Text = ca.remark
                Dim us = (From u In db.usernames Where u.cust_id = CustID And u.active = "y").FirstOrDefault
                If us Is Nothing Then
                    lblTrade_type.Text = "Call"
                Else
                    lblTrade_type.Text = "Call/Online"
                End If
            End If

            lblGrtCash.Text = clsManage.convert2Currency(ca.cash_credit)
            lblGrt96.Text = clsManage.convert2Currency(ca.quan_96)
            lblGrt99.Text = clsManage.convert2Currency(ca.quan_99N)

            Dim tradelimit As New Dictionary(Of String, String)
            tradelimit = clsFng.checkGuaranteeTradeCount(CustID)
            lblTradelimit96.Text = tradelimit.Keys(0).ToString
            lblTradelimit99.Text = tradelimit.Values(0).ToString

            Dim dtSumQuan As Data.DataTable
            dtSumQuan = clsDB.getSumQuanTicketPortfolioByCust_id(CustID)
            If dtSumQuan.Rows.Count > 0 Then

                '***set Property
                ViewState("_PasswordAuthen") = dtSumQuan.Rows(0)("pwd_auth").ToString
                ViewState("_Name") = ca.firstname
                ViewState("_Gold96") = clsManage.convert2Quantity(dtSumQuan.Rows(0)("gold96").ToString)
                ViewState("_Gold96m") = clsManage.convert2Quantity(dtSumQuan.Rows(0)("gold96M").ToString)
                ViewState("_Gold96g") = clsManage.convert2Quantity(dtSumQuan.Rows(0)("gold96G").ToString)
                ViewState("_Gold99") = clsManage.convert2Quantity(dtSumQuan.Rows(0)("gold99").ToString)

                lblCash.Text = clsManage.convert2Currency(dtSumQuan.Rows(0)("cash").ToString)
                lblGold96.Text = clsManage.convert2Quantity(dtSumQuan.Rows(0)("gold96").ToString)
                lblGold96g.Text = clsManage.convert2QuantityGram(dtSumQuan.Rows(0)("gold96G").ToString)
                lblGold96m.Text = clsManage.convert2Quantity(dtSumQuan.Rows(0)("gold96M").ToString)
                lblGold99.Text = clsManage.convert2Quantity(dtSumQuan.Rows(0)("gold99").ToString)

            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

End Class
