
Partial Class stock_daily
    Inherits basePage
    Dim colQuan As Integer = 5
    Dim colPay As Integer = 12

    Protected Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        If Not Page.IsPostBack Then
            Try
                If Session(clsManage.iSession.user_role_center.ToString) IsNot Nothing Then
                    Dim dtRole As New Data.DataTable : dtRole = Session(clsManage.iSession.user_role_center.ToString)
                    If dtRole.Rows.Find(Me.Page.Page.ToString) IsNot Nothing Then
                        Dim dr As Data.DataRow = dtRole.Rows.Find(Me.Page.Page.ToString)

                        If dr(clsDB.roles.export) = False Then
                            linkExport.Enabled = False
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

            gvTicket.EmptyDataText = clsManage.EmptyDataText
            gvTicket.ShowHeader = False
            gvTicket.ShowFooter = True
            ddlStatus.Attributes.Add("onchange", "search();")
            txtDate.Attributes.Add("onkeypress", "return false;")
            txtDate_CalendarExtender.Format = clsManage.formatDateTime
            txtDate_CalendarExtender.OnClientDateSelectionChanged = "search"
            'rdoMode.Attributes.Add("onclick", "$get('" & btnSearchAdv.ClientID & "').click();")
            cbMode.Attributes.Add("onclick", "$get('" & btnSearchAdv.ClientID & "').click();")
            rdoPurity.Attributes.Add("onclick", "$get('" & btnSearchAdv.ClientID & "').click();")

            txtDate.Text = Now.ToString(clsManage.formatDateTime)
            btnSearchAdv_Click(Nothing, Nothing)
        End If
    End Sub

    Protected Sub btnSearchAdv_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearchAdv.Click
        Try

            Dim mode As String = ""
            If cbMode.Items(0).Selected = True Then mode = "1" Else mode = "0"
            If cbMode.Items(1).Selected = True Then mode += "1" Else mode += "0"
            If cbMode.Items(2).Selected = True Then mode += "1" Else mode += "0"

            objSrcTicket.SelectParameters("updateDate").DefaultValue = txtDate.Text
            objSrcTicket.SelectParameters("mode").DefaultValue = mode
            objSrcTicket.SelectParameters("putity").DefaultValue = rdoPurity.SelectedValue


            gvTicket.DataBind()
            If gvTicket.Rows.Count > 0 Then
                clsManage.Script(Page, "$get('" + linkExport.ClientID + "').style.display='block';", "block")
            Else
                clsManage.Script(Page, "$get('" + linkExport.ClientID + "').style.display='none'; ", "none")
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try

    End Sub

    Protected Sub gvTicket_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTicket.RowCreated
        If e.Row.RowType = DataControlRowType.Footer Then

            e.Row.Cells(colQuan).Text = clsManage.convert2zero(ViewState("sell_99")).ToString("#,##0")
            e.Row.Cells(colQuan + 1).Text = clsManage.convert2zero(ViewState("sell_96")).ToString("#,##0")
            e.Row.Cells(colQuan + 2).Text = clsManage.convert2zero(ViewState("sell_96G")).ToString("#,##0")
            e.Row.Cells(colQuan + 3).Text = clsManage.convert2zero(ViewState("buy_99")).ToString("#,##0")
            e.Row.Cells(colQuan + 4).Text = clsManage.convert2zero(ViewState("buy_96")).ToString("#,##0")
            e.Row.Cells(colQuan + 5).Text = clsManage.convert2zero(ViewState("buy_96G")).ToString("#,##0")
            e.Row.Cells(colQuan + 6).Text = clsManage.convert2zero(ViewState("amount")).ToString("#,##0")

            e.Row.Cells(colPay).Text = clsManage.convert2zero(ViewState("sell_cash")).ToString("#,##0")
            e.Row.Cells(colPay + 1).Text = clsManage.convert2zero(ViewState("sell_trans")).ToString("#,##0")
            e.Row.Cells(colPay + 2).Text = clsManage.convert2zero(ViewState("sell_cheq")).ToString("#,##0")
            e.Row.Cells(colPay + 3).Text = clsManage.convert2zero(ViewState("buy_cash")).ToString("#,##0")
            e.Row.Cells(colPay + 4).Text = clsManage.convert2zero(ViewState("buy_trans")).ToString("#,##0")
            e.Row.Cells(colPay + 5).Text = clsManage.convert2zero(ViewState("buy_cheq")).ToString("#,##0")

            e.Row.Cells(colQuan).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(colQuan + 1).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(colQuan + 2).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(colQuan + 3).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(colQuan + 4).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(colQuan + 5).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(colQuan + 6).HorizontalAlign = HorizontalAlign.Right

            e.Row.Cells(colPay).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(colPay + 1).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(colPay + 2).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(colPay + 3).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(colPay + 4).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(colPay + 5).HorizontalAlign = HorizontalAlign.Right

            ''set the columnspan
            e.Row.Cells(0).ColumnSpan = colQuan
            ''remove the second cell
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells(0).Text = "Summary"
            e.Row.Cells(0).HorizontalAlign = HorizontalAlign.Center
        End If
    End Sub

    Protected Sub gvTicket_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTicket.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            Dim creatCels As New SortedList
            creatCels.Add("0", ",1,2")
            creatCels.Add("1", "Datetime,1,2")
            creatCels.Add("2", "Ref_No,1,2")
            creatCels.Add("3", "Customer Name,1,2")
            creatCels.Add("4", "Price,1,2")
            creatCels.Add("5", "Sell,3,1")
            creatCels.Add("6", "Buy,3,1")
            creatCels.Add("7", "Amount,1,2")
            creatCels.Add("8", "Receipt,3,1")
            creatCels.Add("9", "Pay,3,1")

            Dim creatCels1 As New SortedList
            creatCels1.Add("00", "99.99,1,1")
            creatCels1.Add("01", "96.5,1,1")
            creatCels1.Add("02", "96.5 G,1,1")
            creatCels1.Add("03", "99.99,1,1")
            creatCels1.Add("04", "96.5,1,1")
            creatCels1.Add("05", "96.5 G,1,1")
            creatCels1.Add("06", "Cash,1,1")
            creatCels1.Add("07", "Trans,1,1")
            creatCels1.Add("08", "Cheq,1,1")
            creatCels1.Add("09", "Cash,1,1")
            creatCels1.Add("10", "Trans,1,1")
            creatCels1.Add("11", "Cheq,1,1")

            getMultiHeader(e, creatCels1)
            getMultiHeader(e, creatCels)

            ViewState("amount") = 0
            ViewState("sell_99") = 0
            ViewState("sell_96") = 0
            ViewState("sell_96G") = 0
            ViewState("buy_99") = 0
            ViewState("buy_96") = 0
            ViewState("buy_96G") = 0
            ViewState("sell_cash") = 0
            ViewState("sell_trans") = 0
            ViewState("sell_cheq") = 0
            ViewState("buy_cash") = 0
            ViewState("buy_trans") = 0
            ViewState("buy_cheq") = 0
        End If

        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(0).Text = e.Row.RowIndex + 1
            CType(e.Row.FindControl("linkCust"), HyperLink).NavigateUrl = "customer_detail.aspx?id=" + e.Row.DataItem("cust_id").ToString
            If e.Row.DataItem("ticket_id") = "ฝาก/ถอน" Then
                'CType(e.Row.FindControl("link"), HyperLink).Text = "Deposit"
                CType(e.Row.FindControl("link"), HyperLink).NavigateUrl = "customer_trans.aspx?cust_id=" + e.Row.DataItem("cust_id").ToString
            ElseIf e.Row.DataItem("ticket_id") = "In/Out" Then
                CType(e.Row.FindControl("link"), HyperLink).NavigateUrl = "stock_inout.aspx"
            Else
                CType(e.Row.FindControl("link"), HyperLink).NavigateUrl = "ticket_deal.aspx?page=dialy&id=" + e.Row.DataItem("ticket_id").ToString
            End If

            e.Row.Cells(colPay).Text = clsManage.convert2Currency(e.Row.DataItem("sell_cash"))
            e.Row.Cells(colPay + 1).Text = clsManage.convert2Currency(e.Row.DataItem("sell_trans"))
            e.Row.Cells(colPay + 2).Text = clsManage.convert2Currency(e.Row.DataItem("sell_cheq"))
            e.Row.Cells(colPay + 3).Text = clsManage.convert2Currency(e.Row.DataItem("buy_cash"))
            e.Row.Cells(colPay + 4).Text = clsManage.convert2Currency(e.Row.DataItem("buy_trans"))
            e.Row.Cells(colPay + 5).Text = clsManage.convert2Currency(e.Row.DataItem("buy_cheq"))

            ViewState("amount") += clsManage.convert2zero(e.Row.DataItem("amount"))
            ViewState("sell_99") += clsManage.convert2zero(e.Row.DataItem("sell_99"))
            ViewState("sell_96") += clsManage.convert2zero(e.Row.DataItem("sell_96"))
            ViewState("sell_96G") += clsManage.convert2zero(e.Row.DataItem("sell_96G"))
            ViewState("buy_99") += clsManage.convert2zero(e.Row.DataItem("buy_99"))
            ViewState("buy_96") += clsManage.convert2zero(e.Row.DataItem("buy_96"))
            ViewState("buy_96G") += clsManage.convert2zero(e.Row.DataItem("buy_96G"))

            ViewState("sell_cash") += clsManage.convert2zero(e.Row.DataItem("sell_cash"))
            ViewState("sell_trans") += clsManage.convert2zero(e.Row.DataItem("sell_trans"))
            ViewState("sell_cheq") += clsManage.convert2zero(e.Row.DataItem("sell_cheq"))

            ViewState("buy_cash") += clsManage.convert2zero(e.Row.DataItem("buy_cash"))
            ViewState("buy_trans") += clsManage.convert2zero(e.Row.DataItem("buy_trans"))
            ViewState("buy_cheq") += clsManage.convert2zero(e.Row.DataItem("buy_cheq"))
        End If

        If e.Row.RowType = DataControlRowType.Footer Then
            Dim sw As New IO.StringWriter()
            Dim htw As New HtmlTextWriter(sw)
            gvTicket.RenderControl(htw)
            ViewState("htmlExcel") = sw.ToString
        End If
    End Sub

    Private Sub getMultiHeader(ByVal e As GridViewRowEventArgs, ByVal getCels As SortedList)
        Dim row As GridViewRow
        Dim enumCels As IDictionaryEnumerator = getCels.GetEnumerator
        row = New GridViewRow(-1, -1, DataControlRowType.Header, DataControlRowState.Normal)

        While (enumCels.MoveNext)
            Dim cont As String() = enumCels.Value.ToString.Split(",")
            Dim cell As New TableCell
            cell.RowSpan = cont(2).ToString
            cell.ColumnSpan = cont(1).ToString
            cell.Controls.Add(New LiteralControl(cont(0).ToString))
            cell.HorizontalAlign = HorizontalAlign.Center
            cell.BackColor = New Drawing.ColorConverter().ConvertFromString("#274B98")
            cell.ForeColor = System.Drawing.Color.White
            cell.Font.Bold = True
            row.Cells.Add(cell)

            e.Row.Parent.Controls.AddAt(0, row)
        End While
    End Sub

    Private Sub getMultiFooter(ByVal e As GridViewRowEventArgs, ByVal getCels As SortedList)
        Dim row As GridViewRow
        Dim enumCels As IDictionaryEnumerator = getCels.GetEnumerator
        row = New GridViewRow(-1, -1, DataControlRowType.Footer, DataControlRowState.Normal)

        While (enumCels.MoveNext)
            Dim cont As String() = enumCels.Value.ToString.Split(",")
            Dim cell As New TableCell
            cell.RowSpan = cont(2).ToString
            cell.ColumnSpan = cont(1).ToString
            cell.Controls.Add(New LiteralControl(cont(0).ToString))
            cell.HorizontalAlign = HorizontalAlign.Center
            cell.BackColor = New Drawing.ColorConverter().ConvertFromString("#274B98")
            cell.ForeColor = System.Drawing.Color.White
            cell.Font.Bold = True
            row.Cells.Add(cell)

            e.Row.Parent.Controls.AddAt(gvTicket.Rows.Count + 3, row)
        End While
    End Sub

    Protected Sub linkExport_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles linkExport.Click

        'Dim mode As String = ""
        'If cbMode.Items(0).Selected = True Then mode = "1" Else mode = "0"
        'If cbMode.Items(1).Selected = True Then mode += "1" Else mode += "0"
        'If cbMode.Items(2).Selected = True Then mode += "1" Else mode += "0"

        'Dim dt As New Data.DataTable
        'dt = clsDB.getStockDialy(txtDate.Text, ddlStatus.SelectedValue, mode, rdoPurity.SelectedValue)
        'If dt.Rows.Count > 0 Then
        '    Dim htSum As New Hashtable

        '    htSum.Add("sell_99", ViewState("sell_99"))
        '    htSum.Add("sell_96", ViewState("sell_96"))
        '    htSum.Add("sell_96G", ViewState("sell_96G"))
        '    htSum.Add("buy_99", ViewState("buy_99"))
        '    htSum.Add("buy_96", ViewState("buy_96"))
        '    htSum.Add("buy_96G", ViewState("buy_96G"))
        '    htSum.Add("amount", ViewState("amount"))

        '    htSum.Add("sell_cash", ViewState("sell_cash"))
        '    htSum.Add("sell_trans", ViewState("sell_trans"))
        '    htSum.Add("sell_cheq", ViewState("sell_cheq"))
        '    htSum.Add("buy_cash", ViewState("buy_cash"))
        '    htSum.Add("buy_trans", ViewState("buy_trans"))
        '    htSum.Add("buy_cheq", ViewState("buy_cheq"))


        '    clsManage.ExportToExcelStock(dt, "Summary_Report_" + txtDate.Text, htSum)
        'End If

        Try
            If ViewState("htmlExcel") IsNot Nothing AndAlso ViewState("htmlExcel").ToString <> "" Then
                ExportToExcel(ViewState("htmlExcel").ToString)
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "err")
        End Try

    End Sub

    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As System.Web.UI.Control)
    End Sub

    Private Sub ExportToExcel(ByVal data_temp As String, Optional excelName As String = "Performance_Report")

        HttpContext.Current.Response.Clear()
        HttpContext.Current.Response.Buffer = True
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Unicode
        HttpContext.Current.Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble())
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" & excelName & ".xls")
        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        HttpContext.Current.Response.Charset = String.Empty
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"
        Dim sw As System.IO.StringWriter = New System.IO.StringWriter()
        Dim hw As System.Web.UI.HtmlTextWriter = New HtmlTextWriter(sw)

        HttpContext.Current.Response.Output.Write(data_temp)
        HttpContext.Current.Response.Flush()
        HttpContext.Current.Response.End()

    End Sub

    Protected Sub upgSearch_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles upgSearch.Load
        System.Threading.Thread.Sleep(upgSearch.DisplayAfter)
    End Sub

End Class
