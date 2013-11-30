
Partial Class customer_grid
    Inherits basePage
    Protected Sub gvCust_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCust.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(2).Text = "1001" Then e.Row.Cells(2).Text = "บุคล"
            If e.Row.Cells(2).Text = "1002" Then e.Row.Cells(2).Text = "ร้านทอง"
            If e.Row.Cells(2).Text = "1003" Then e.Row.Cells(2).Text = "jewelry"


            'Credit
            Dim amount As Double = 0
            Dim paid As Double = 0
            If e.Row.DataItem("amount").ToString <> "" Then amount = e.Row.DataItem("amount").ToString
            If e.Row.DataItem("paid").ToString <> "" Then paid = e.Row.DataItem("paid").ToString
            e.Row.Cells(gvCust.Columns.Count - 3).Text = clsManage.customer_credit(e.Row.DataItem("credit_guarantee").ToString, _
                                                                                   amount, _
                                                                                   e.Row.DataItem("margin").ToString, _
                                                                                   paid).ToString("#.00")
            e.Row.Cells(gvCust.Columns.Count - 2).Text = paid.ToString(clsManage.formatCurrency)


            'Profit-loss
            If e.Row.DataItem("sumQuan").ToString <> "" Then
                Dim profit_loss As Double = clsManage.customer_profitLoss(amount, e.Row.DataItem("sumQuan").ToString, e.Row.DataItem("price_now").ToString)
                Dim cash_balence As Double = clsManage.customer_cashBalance(e.Row.DataItem("credit_guarantee").ToString, amount, e.Row.DataItem("sumQuan").ToString, e.Row.DataItem("price_now").ToString)
                e.Row.Cells(gvCust.Columns.Count - 1).Text = profit_loss.ToString(clsManage.formatCurrency)
                e.Row.Cells(gvCust.Columns.Count - 4).Text = cash_balence.ToString(clsManage.formatCurrency)
                If cash_balence < 0 Then
                    e.Row.Cells(gvCust.Columns.Count - 4).ForeColor = Drawing.Color.Red
                Else
                    e.Row.Cells(gvCust.Columns.Count - 4).ForeColor = Drawing.Color.Black
                End If
            Else
                e.Row.Cells(gvCust.Columns.Count - 1).Text = "0.00"
                e.Row.Cells(gvCust.Columns.Count - 4).Text = Double.Parse(e.Row.DataItem("credit_guarantee")).ToString(clsManage.formatCurrency)
            End If



        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Try
                gvCust.EmptyDataText = "<span style='color:red'>Data Not Found.</span>"
                Dim script As String = ""
                If Request.QueryString("id") IsNot Nothing AndAlso Request.QueryString("id").ToString <> "" Then
                    objSrcCust.SelectParameters("cust_id").DefaultValue = Request.QueryString("id").ToString
                    objSrcCust.SelectParameters("cust_name").DefaultValue = ""
                    script = "top.window.document.getElementById('ctl00_SampleContent_txtCustName').value = '" + gvCust.Rows(0).Cells(3).Text + "';"
                ElseIf Request.QueryString("name") IsNot Nothing AndAlso Request.QueryString("name").ToString <> "" Then
                    objSrcCust.SelectParameters("cust_id").DefaultValue = ""
                    objSrcCust.SelectParameters("cust_name").DefaultValue = Request.QueryString("name").ToString
                    script = "top.window.document.getElementById('ctl00_SampleContent_txtCustRef').value = '" + gvCust.Rows(0).Cells(1).Text + "';"
                Else
                    objSrcCust.SelectParameters("cust_id").DefaultValue = ""
                    objSrcCust.SelectParameters("cust_name").DefaultValue = ""
                End If
                gvCust.DataBind()
                If gvCust.Rows.Count > 0 Then
                    If Request.QueryString("id") IsNot Nothing AndAlso Request.QueryString("id").ToString <> "" Then
                        script = "top.window.document.getElementById('ctl00_SampleContent_txtCustName').value = '" + gvCust.Rows(0).Cells(3).Text + "';"
                    ElseIf Request.QueryString("name") IsNot Nothing AndAlso Request.QueryString("name").ToString <> "" Then
                        script = "top.window.document.getElementById('ctl00_SampleContent_txtCustRef').value = '" + gvCust.Rows(0).Cells(1).Text + "';"
                    End If
                    clsManage.Script(Page, script)
                End If
            Catch ex As Exception
                Throw ex
            End Try

        End If
    End Sub

    Protected Sub gvCust_SelectedIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSelectEventArgs) Handles gvCust.SelectedIndexChanging
        Dim id As String = gvCust.DataKeys(e.NewSelectedIndex).Value
        clsManage.Script(Page, "top.window.location.href='customer_detail.aspx?id=" & id & " '")
    End Sub
End Class
