Imports iTextSharp.text
Imports iTextSharp.text.pdf
Imports iTextSharp.text.html.simpleparser
Imports System.IO

Partial Class stock_actual
    Inherits basePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            txtDate_CalendarExtender.Format = clsManage.formatDateTime
            txtDate.Text = DateTime.Now.ToString(clsManage.formatDateTime)

            gvAssetCash.EmptyDataText = clsManage.EmptyDataText
            gvAssetGold96.EmptyDataText = clsManage.EmptyDataText
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
            If e.Row.DataItem("ref_no").ToString.Length = 5 Then
                CType(e.Row.FindControl("link"), HyperLink).NavigateUrl = "ticket_payment_detail.aspx?pid=" + e.Row.DataItem("ref_no").ToString
            Else
                CType(e.Row.FindControl("link"), HyperLink).NavigateUrl = "ticket_deal.aspx?page=dialy&id=" + e.Row.DataItem("ref_no").ToString
            End If

            CType(e.Row.FindControl("linkCust"), HyperLink).NavigateUrl = "customer_detail.aspx?id=" + e.Row.DataItem("cust_id").ToString
            Dim summaryMoney As Double = clsManage.convert2zero(e.Row.DataItem("cash").ToString) + clsManage.convert2zero(e.Row.DataItem("trans").ToString) + clsManage.convert2zero(e.Row.DataItem("cheq").ToString)
            e.Row.Cells(e.Row.Cells.Count - 1).Text = clsManage.convert2Currency(summaryMoney.ToString)
        End If
    End Sub

    Protected Sub gvAssetGold96_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAssetGold96.RowDataBound
    
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.DataItem("ref_no").ToString.Length = 5 Then
                CType(e.Row.FindControl("link"), HyperLink).NavigateUrl = "ticket_payment_detail.aspx?pid=" + e.Row.DataItem("ref_no").ToString
            Else
                CType(e.Row.FindControl("link"), HyperLink).NavigateUrl = "ticket_deal.aspx?page=dialy&id=" + e.Row.DataItem("ref_no").ToString
            End If

            CType(e.Row.FindControl("linkCust"), HyperLink).NavigateUrl = "customer_detail.aspx?id=" + e.Row.DataItem("cust_id").ToString
        End If
    End Sub

    Sub bindData()
        Dim iDate As DateTime = DateTime.ParseExact(txtDate.Text, clsManage.formatDateTime, Nothing)
        Dim i As Integer = DateDiff(DateInterval.Day, Now.Today, iDate)

        If iDate > Now.Today Then
            clsManage.alert(Page, "Please Select Date less then Now.") : Exit Sub
        End If

        ''****
        ''โชว์รายการ
        ''****
        'get Net Stocoks  (now stock รวม deposit เข้าไปแล้ว)
        Dim dtStockDep As New Data.DataTable
        dtStockDep = clsDB.getStockSumDeposit
        If dtStockDep.Rows.Count > 0 Then
            lblNetCash.Text = clsManage.convert2Currency(dtStockDep.Rows(0)("cash").ToString())
            lblNetTrans.Text = clsManage.convert2Currency(dtStockDep.Rows(0)("trans").ToString())
            lblNetCheq.Text = clsManage.convert2Currency(dtStockDep.Rows(0)("cheq").ToString())

            Dim summaryMoney As Double = clsManage.convert2zero(dtStockDep.Rows(0)("cash").ToString()) + clsManage.convert2zero(dtStockDep.Rows(0)("trans").ToString()) + clsManage.convert2zero(dtStockDep.Rows(0)("cheq").ToString())
            lblNetMoney.Text = clsManage.convert2Currency(summaryMoney.ToString)
            lblNet96.Text = clsManage.convert2Quantity(dtStockDep.Rows(0)("G96Dep").ToString())
            lblNet96G.Text = clsManage.convert2Quantity(dtStockDep.Rows(0)("G96GDep").ToString())
            lblNet99.Text = clsManage.convert2Quantity(dtStockDep.Rows(0)("G99Dep").ToString())

        End If

        Dim purity As String = ""
        If cblPurity.Items(0).Selected = True Then purity = "96"

        If cblPurity.Items(1).Selected = True Then
            If purity = "" Then purity = "99" Else purity += "','" + "99"
        End If

        objSrcAssetCash.SelectParameters("assetType").DefaultValue = "Cash"
        objSrcAssetCash.SelectParameters("purity").DefaultValue = ""
        objSrcAssetCash.SelectParameters("actualDay").DefaultValue = i
        gvAssetCash.DataBind()

        objSrcAssetGold96.SelectParameters("assetType").DefaultValue = "Gold"
        objSrcAssetGold96.SelectParameters("purity").DefaultValue = purity
        objSrcAssetGold96.SelectParameters("actualDay").DefaultValue = i
        gvAssetGold96.DataBind()

       
    End Sub

    Public Overloads Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)

    End Sub

    Protected Sub linkExportCash_Click(sender As Object, e As System.EventArgs) Handles linkExportCash.Click
        ExportActualPdf(gvAssetCash, "Actual_Cash_")
    End Sub

    Protected Sub linkExportGold_Click(sender As Object, e As System.EventArgs) Handles linkExportGold.Click
        ExportActualPdf(gvAssetGold96, "Actual_Gold_")
    End Sub

    Private Sub ExportActualPdf(gv As GridView, fileName As String)
        Try
            Dim sw As New StringWriter()
            Dim hw As New HtmlTextWriter(sw)
            gv.RenderControl(hw)
            Dim hp As New HtmlToPdf(sw.ToString, Server.MapPath("") + "\font\TAHOMA.TTF", 4)
            hp.Render(HttpContext.Current.Response, fileName + DateTime.Now.ToString(clsManage.formatDateTime) + ".pdf")
        Catch ex As Exception

        End Try
    End Sub
End Class
