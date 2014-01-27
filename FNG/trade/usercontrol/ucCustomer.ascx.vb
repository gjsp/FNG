
Partial Class user_control_ucCustomer
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
            If CustID = "" Then
                mvMain.SetActiveView(vNone)
                Exit Sub
            End If

            Dim dt As New Data.DataTable
            dt = clsDB.getCustomerByCust_id(CustID)
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim dr As Data.DataRow = dt.Rows(0)
                lblCustRef.Text = dr("cust_id").ToString
                lblName.Text = dr("FirstName").ToString + " " + dr("lastname").ToString
                lblAdd.Text = dr("bill_address").ToString

                lblMobilePhone.Text = dr("mobile").ToString
                lblAccBank.Text = dr("bank1").ToString
                lblAccNo.Text = dr("account_no1").ToString

                lblBranch.Text = dr("account_branch1").ToString
                lblAccName.Text = dr("account_name1").ToString

                lblGrtCash.Text = clsManage.convert2Currency(dr("cash_credit").ToString)
                lblGrt96.Text = clsManage.convert2Currency(dr("quan_96").ToString)
                lblGrt99.Text = clsManage.convert2Currency(dr("quan_99N").ToString)

                Dim tradelimit As New Dictionary(Of String, String)
                tradelimit = clsFng.checkGuaranteeTradeCount(CustID)
                lblTradelimit96.Text = tradelimit.Keys(0).ToString
                lblTradelimit99.Text = tradelimit.Values(0).ToString

                Dim dtSumQuan As Data.DataTable
                dtSumQuan = clsDB.getSumQuanTicketPortfolioByCust_id(CustID)
                If dtSumQuan.Rows.Count > 0 Then

                    '***set Property
                    ViewState("_PasswordAuthen") = dtSumQuan.Rows(0)("pwd_auth").ToString
                    ViewState("_Name") = dr("FirstName").ToString
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
                mvMain.SetActiveView(vMain)
            Else
                '***set Property
                ViewState("_CustID") = ""
                ViewState("_Name") = ""
                mvMain.SetActiveView(vNone)
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

End Class
