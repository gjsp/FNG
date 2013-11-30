
Partial Class stock_now
    Inherits basePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim count As String = "0"
            Dim planMode As String = "0"
            If Request.QueryString("count") IsNot Nothing Then
                count = Request.QueryString("count").ToString
            End If
            If Request.QueryString("planMode") IsNot Nothing Then
                planMode = Request.QueryString("planMode").ToString
            End If

            btnRefresh.Attributes.Add("onclick", "PageMethods.getStock(" + count + ",'" + planMode + "',OnComplete);return false;")
            btnRefresh.Attributes.Add("style", "display:none")
            clsManage.Script(Page, "refreshStock();")
        End If
    End Sub

    <System.Web.Services.WebMethod()> _
    Public Shared Function getStock(ByVal planningDay As String, ByVal planMode As String) As String
        Dim dt As New Data.DataTable

        dt = clsDB.getStockToday(planningDay)
        Dim strSplit As String = "|"
        If dt.Rows.Count > 0 Then

            'gold_ticket96.gold_ticket96, gold_ticket99N.gold_ticket99N, gold_ticket99L.gold_ticket99L, 
            'gold_base96.gold_base96,gold_base99N.gold_base99N, gold_base99L.gold_base99L, 
            'gold_Branch96.gold_Branch96, gold_Branch99N.gold_Branch99N, gold_Branch99L.gold_Branch99L, 
            'gold_deposit96.gold_deposit96, gold_deposit99N.gold_deposit99N, gold_deposit99L.gold_deposit99L, 
            'gold_credit96.sumGoldCredit96, gold_credit99N.sumGoldCredit99N, gold_credit99L.sumGoldCredit99L, 
            'cash_base.cash_base, cash_Branch.cash_Branch, cash_deposit.cash_deposit, cust_CreditCash.cust_CreditCash, 
            'gold_pending96.gold_pending96, gold_pending99N.gold_pending99N, gold_pending99L.gold_pending99L

            Dim result As String = ""
            If planningDay <> "1" And planMode = "1" Then
                Dim dtPlan As New Data.DataTable
                dtPlan = clsDB.getStockToday("0")
                If dtPlan.Rows.Count > 0 Then

                    result = (clsManage.convert2zero(dt.Rows(0)("gold_ticket96").ToString()) - clsManage.convert2zero(dtPlan.Rows(0)("gold_ticket96").ToString())).ToString + strSplit & _
                              (clsManage.convert2zero(dt.Rows(0)("gold_ticket99N").ToString()) - clsManage.convert2zero(dtPlan.Rows(0)("gold_ticket99N").ToString())).ToString + strSplit & _
                               (clsManage.convert2zero(dt.Rows(0)("gold_ticket99L").ToString()) - clsManage.convert2zero(dtPlan.Rows(0)("gold_ticket99L").ToString())).ToString + strSplit & _
                              (clsManage.convert2zero(dt.Rows(0)("gold_base96").ToString()) - clsManage.convert2zero(dtPlan.Rows(0)("gold_base96").ToString())).ToString + strSplit & _
                              (clsManage.convert2zero(dt.Rows(0)("gold_base99N").ToString()) - clsManage.convert2zero(dtPlan.Rows(0)("gold_base99N").ToString())).ToString + strSplit & _
                               (clsManage.convert2zero(dt.Rows(0)("gold_base99L").ToString()) - clsManage.convert2zero(dtPlan.Rows(0)("gold_base99L").ToString())).ToString + strSplit & _
                               (clsManage.convert2zero(dt.Rows(0)("gold_Branch96").ToString()) - clsManage.convert2zero(dtPlan.Rows(0)("gold_Branch96").ToString())).ToString + strSplit & _
                              (clsManage.convert2zero(dt.Rows(0)("gold_Branch99N").ToString()) - clsManage.convert2zero(dtPlan.Rows(0)("gold_Branch99N").ToString())).ToString + strSplit & _
                               (clsManage.convert2zero(dt.Rows(0)("gold_Branch99L").ToString()) - clsManage.convert2zero(dtPlan.Rows(0)("gold_Branch99L").ToString())).ToString + strSplit & _
                               (clsManage.convert2zero(dt.Rows(0)("gold_deposit96").ToString()) - clsManage.convert2zero(dtPlan.Rows(0)("gold_deposit96").ToString())).ToString + strSplit & _
                               (clsManage.convert2zero(dt.Rows(0)("gold_deposit99N").ToString()) - clsManage.convert2zero(dtPlan.Rows(0)("gold_deposit99N").ToString())).ToString + strSplit & _
                               (clsManage.convert2zero(dt.Rows(0)("gold_deposit99L").ToString()) - clsManage.convert2zero(dtPlan.Rows(0)("gold_deposit99L").ToString())).ToString + strSplit & _
                               (clsManage.convert2zero(dt.Rows(0)("sumGoldCredit96").ToString()) - clsManage.convert2zero(dtPlan.Rows(0)("sumGoldCredit96").ToString())).ToString + strSplit & _
                               (clsManage.convert2zero(dt.Rows(0)("sumGoldCredit99N").ToString()) - clsManage.convert2zero(dtPlan.Rows(0)("sumGoldCredit99N").ToString())).ToString + strSplit & _
                               (clsManage.convert2zero(dt.Rows(0)("sumGoldCredit99L").ToString()) - clsManage.convert2zero(dtPlan.Rows(0)("sumGoldCredit99L").ToString())).ToString + strSplit & _
                               (clsManage.convert2zero(dt.Rows(0)("cash_ticket_complete").ToString()) - clsManage.convert2zero(dtPlan.Rows(0)("cash_ticket_complete").ToString())).ToString + strSplit & _
                               (clsManage.convert2zero(dt.Rows(0)("cash_base").ToString()) - clsManage.convert2zero(dtPlan.Rows(0)("cash_base").ToString())).ToString + strSplit & _
                              (clsManage.convert2zero(dt.Rows(0)("cash_Branch").ToString()) - clsManage.convert2zero(dtPlan.Rows(0)("cash_Branch").ToString())).ToString + strSplit & _
                               (clsManage.convert2zero(dt.Rows(0)("cash_deposit").ToString()) - clsManage.convert2zero(dtPlan.Rows(0)("cash_deposit").ToString())).ToString + strSplit & _
                              (clsManage.convert2zero(dt.Rows(0)("cust_CreditCash").ToString()) - clsManage.convert2zero(dtPlan.Rows(0)("cust_CreditCash").ToString())).ToString + strSplit & _
                              (clsManage.convert2zero(dt.Rows(0)("gold_pending96").ToString()) - clsManage.convert2zero(dtPlan.Rows(0)("gold_pending96").ToString())).ToString + strSplit & _
                              (clsManage.convert2zero(dt.Rows(0)("gold_pending99N").ToString()) - clsManage.convert2zero(dtPlan.Rows(0)("gold_pending99N").ToString())).ToString + strSplit & _
                              (clsManage.convert2zero(dt.Rows(0)("gold_pending99L").ToString()) - clsManage.convert2zero(dtPlan.Rows(0)("gold_pending99L").ToString())).ToString + strSplit & _
                               (clsManage.convert2zero(dt.Rows(0)("cash_ticket_pending").ToString()) - clsManage.convert2zero(dtPlan.Rows(0)("cash_ticket_pending").ToString())).ToString
                End If
            Else
                result = dt.Rows(0)("gold_ticket96").ToString() + strSplit & _
                           dt.Rows(0)("gold_ticket99N").ToString() + strSplit & _
                           dt.Rows(0)("gold_ticket99L").ToString() + strSplit & _
                           dt.Rows(0)("gold_base96").ToString() + strSplit & _
                           dt.Rows(0)("gold_base99N").ToString() + strSplit & _
                           dt.Rows(0)("gold_base99L").ToString() + strSplit & _
                           dt.Rows(0)("gold_Branch96").ToString() + strSplit & _
                           dt.Rows(0)("gold_Branch99N").ToString() + strSplit & _
                           dt.Rows(0)("gold_Branch99L").ToString() + strSplit & _
                           dt.Rows(0)("gold_deposit96").ToString() + strSplit & _
                           dt.Rows(0)("gold_deposit99N").ToString() + strSplit & _
                           dt.Rows(0)("gold_deposit99L").ToString() + strSplit & _
                           dt.Rows(0)("sumGoldCredit96").ToString() + strSplit & _
                           dt.Rows(0)("sumGoldCredit99N").ToString() + strSplit & _
                           dt.Rows(0)("sumGoldCredit99L").ToString() + strSplit & _
                           dt.Rows(0)("cash_ticket_complete").ToString() + strSplit & _
                           dt.Rows(0)("cash_base").ToString() + strSplit & _
                           dt.Rows(0)("cash_Branch").ToString() + strSplit & _
                           dt.Rows(0)("cash_deposit").ToString() + strSplit & _
                           dt.Rows(0)("cust_CreditCash").ToString() + strSplit & _
                           dt.Rows(0)("gold_pending96").ToString() + strSplit & _
                           dt.Rows(0)("gold_pending99N").ToString() + strSplit & _
                           dt.Rows(0)("gold_pending99L").ToString() + strSplit & _
                           dt.Rows(0)("cash_ticket_pending").ToString()
            End If


            Return result
        Else
            Return ""
        End If
    End Function
End Class
