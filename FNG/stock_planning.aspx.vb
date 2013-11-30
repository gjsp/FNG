
Partial Class stock_planning
    Inherits basePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            txtDate_CalendarExtender.Format = clsManage.formatDateTime
            txtDate.Text = DateTime.Now.ToString(clsManage.formatDateTime)
            Dim strNodata As String = "<div style='color:red;text-align:center'>No Data Planning.</div>"
            gvAssetCash.EmptyDataText = strNodata
            gvAssetGold96.EmptyDataText = strNodata
            'imgSearch.Attributes.Add("onclick", String.Format("setStockPage('{0}');", txtDate.ClientID))
            'clsManage.Script(Page, "enableWindow('0');")
            bindData()
            tapImport.ActiveTabIndex = 1
        End If
    End Sub

    Protected Sub upgSearch_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles upgSearch.Load
        System.Threading.Thread.Sleep(upgSearch.DisplayAfter)
    End Sub

    Protected Sub imgSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSearch.Click
        bindData()
    End Sub

    Protected Sub gvAssetCash_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAssetCash.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("link"), HyperLink).NavigateUrl = "ticket_deal.aspx?page=dialy&id=" + e.Row.DataItem("ref_no").ToString
            CType(e.Row.FindControl("linkCust"), HyperLink).NavigateUrl = "customer_detail.aspx?id=" + e.Row.DataItem("cust_id").ToString

            Dim summaryMoney As Double = 0
            Dim sumAmount As Double = clsManage.convert2zero(e.Row.DataItem("quantity").ToString)
            If e.Row.DataItem("payment").ToString = "cash" Then
                ViewState("netCash") += sumAmount
            ElseIf e.Row.DataItem("payment").ToString = "trans" Then
                ViewState("netTrans") += sumAmount
            ElseIf e.Row.DataItem("payment").ToString = "cheq" Then
                ViewState("netCheq") += sumAmount
            End If
            summaryMoney = clsManage.convert2zero(ViewState("netCash").ToString) + clsManage.convert2zero(ViewState("netTrans").ToString) + clsManage.convert2zero(ViewState("netCheq").ToString)

            e.Row.Cells(gvAssetCash.Columns.Count - 4).Text = clsManage.convert2zero(ViewState("netCash")).ToString(clsManage.formatCurrency)
            e.Row.Cells(gvAssetCash.Columns.Count - 3).Text = clsManage.convert2zero(ViewState("netTrans")).ToString(clsManage.formatCurrency)
            e.Row.Cells(gvAssetCash.Columns.Count - 2).Text = clsManage.convert2zero(ViewState("netCheq")).ToString(clsManage.formatCurrency)
            e.Row.Cells(gvAssetCash.Columns.Count - 1).Text = clsManage.convert2Currency(summaryMoney.ToString)

            If CDate(e.Row.DataItem("datetime")).Day = DateTime.ParseExact(txtDate.Text, clsManage.formatDateTime, Nothing).Day + 1 Then
                e.Row.Cells(1).Text = ""
            End If
        End If
    End Sub

    Protected Sub gvAssetGold96_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAssetGold96.RowDataBound
       
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("link"), HyperLink).NavigateUrl = "ticket_deal.aspx?page=dialy&id=" + e.Row.DataItem("ref_no").ToString
            CType(e.Row.FindControl("linkCust"), HyperLink).NavigateUrl = "customer_detail.aspx?id=" + e.Row.DataItem("cust_id").ToString
            Dim sumGold As Double = clsManage.convert2zero(e.Row.DataItem("quantity").ToString)
            If e.Row.DataItem("purity").ToString = "96" Then
                ViewState("netgold96") += sumGold
            ElseIf e.Row.DataItem("purity").ToString = "99N" Then
                ViewState("netgold99N") += sumGold
            ElseIf e.Row.DataItem("purity").ToString = "99L" Then
                ViewState("netgold99L") += sumGold
            End If
            e.Row.Cells(gvAssetGold96.Columns.Count - 3).Text = clsManage.convert2zero(ViewState("netgold96")).ToString(clsManage.formatQuantity)
            e.Row.Cells(gvAssetGold96.Columns.Count - 2).Text = clsManage.convert2zero(ViewState("netgold99N")).ToString(clsManage.formatQuantity)
            e.Row.Cells(gvAssetGold96.Columns.Count - 1).Text = clsManage.convert2zero(ViewState("netgold99L")).ToString(clsManage.formatQuantity)

            If CDate(e.Row.DataItem("datetime")).Day = DateTime.ParseExact(txtDate.Text, clsManage.formatDateTime, Nothing).Day + 1 Then
                e.Row.Cells(1).Text = ""
            End If
        End If
    End Sub

    Sub bindData()
        Dim iDate As DateTime = DateTime.ParseExact(txtDate.Text, clsManage.formatDateTime, Nothing)
        Dim i As Integer = DateDiff(DateInterval.Day, DateTime.Now.Today , iDate)

        If iDate < Now.Today Then
            clsManage.alert(Page, "Please Select Date > more then Now.") : Exit Sub
        End If

        'get Net Stocoks
        Dim dtStockDep As New Data.DataTable
        Dim IsDep As String = "y"
        If cbDeposit.Checked = True Then IsDep = "n"
        dtStockDep = clsDB.getStockAndSumDeposit(IsDep)
        If dtStockDep.Rows.Count > 0 Then
            Dim sumaryMoney As Double = clsManage.convert2zero(dtStockDep.Rows(0)("cash").ToString()) + clsManage.convert2zero(dtStockDep.Rows(0)("trans").ToString()) + clsManage.convert2zero(dtStockDep.Rows(0)("cheq").ToString())
            lblNetMoney.Text = clsManage.convert2Currency(sumaryMoney.ToString)
            lblCash.Text = clsManage.convert2Currency(dtStockDep.Rows(0)("cash").ToString())
            lblTrans.Text = clsManage.convert2Currency(dtStockDep.Rows(0)("trans").ToString())
            lblCheq.Text = clsManage.convert2Currency(dtStockDep.Rows(0)("cheq").ToString())

            lblNet96.Text = clsManage.convert2Quantity(dtStockDep.Rows(0)("G96").ToString())
            lblNet99N.Text = clsManage.convert2Quantity(dtStockDep.Rows(0)("G99N").ToString())
            lblNet99L.Text = clsManage.convert2Quantity(dtStockDep.Rows(0)("G99L").ToString())

            ViewState("netCash") = clsManage.convert2zero(dtStockDep.Rows(0)("cash"))
            ViewState("netTrans") = clsManage.convert2zero(dtStockDep.Rows(0)("trans"))
            ViewState("netCheq") = clsManage.convert2zero(dtStockDep.Rows(0)("cheq"))

            'ViewState("netMoney") = clsManage.convert2zero(dtStockDep.Rows(0)("price"))
            ViewState("netgold96") = clsManage.convert2zero(dtStockDep.Rows(0)("G96"))
            ViewState("netgold99N") = clsManage.convert2zero(dtStockDep.Rows(0)("G99N"))
            ViewState("netgold99L") = clsManage.convert2zero(dtStockDep.Rows(0)("G99L"))
        End If

        Dim purity As String = ""
        If cblPurity.Items(0).Selected = True Then purity = "96"
        If cblPurity.Items(1).Selected = True Then
            If purity = "" Then purity = "99N" Else purity += "','" + "99N"
        End If

        If cblPurity.Items(2).Selected = True Then
            If purity = "" Then purity = "99L" Else purity += "','" + "99L"
        End If

        Dim isDeposit As String = "n"
        If cbDeposit.Checked Then
            isDeposit = "y"
        End If

        objSrcAssetCash.SelectParameters("assetType").DefaultValue = "Cash"
        objSrcAssetCash.SelectParameters("purity").DefaultValue = ""
        objSrcAssetCash.SelectParameters("planningDay").DefaultValue = i
        objSrcAssetCash.SelectParameters("deposit").DefaultValue = isDeposit

        If cbCall.Checked Then
            objSrcAssetCash.SelectParameters("delivery_date_null").DefaultValue = "1"
        Else
            objSrcAssetCash.SelectParameters("delivery_date_null").DefaultValue = "0"
        End If
        gvAssetCash.DataBind()

        objSrcAssetGold96.SelectParameters("assetType").DefaultValue = "Gold"
        objSrcAssetGold96.SelectParameters("purity").DefaultValue = purity
        objSrcAssetGold96.SelectParameters("planningDay").DefaultValue = i
        objSrcAssetGold96.SelectParameters("deposit").DefaultValue = isDeposit
        If cbCall.Checked Then
            objSrcAssetGold96.SelectParameters("delivery_date_null").DefaultValue = "1"
        Else
            objSrcAssetGold96.SelectParameters("delivery_date_null").DefaultValue = "0"
        End If
        gvAssetGold96.DataBind()
    End Sub
End Class
