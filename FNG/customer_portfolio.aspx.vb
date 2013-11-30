Imports System.Data.SqlClient
Imports System.Data

Partial Class customer_portfolio
    Inherits basePage

    Dim colPrice As Integer = 5
    Dim colQuan As Integer = 6
    Dim colAmount As Integer = 7
    'Dim colPL As Integer = 12

    Dim colQuanHis As Integer = 5
    Dim colAmountHis As Integer = 6

    Dim colFootQuan As Integer = 2
    Dim colFootAmount As Integer = 3
    'Dim colFootPL As Integer = 6

    Protected Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        If Not Page.IsPostBack Then
            Try
                If Session(clsManage.iSession.user_role_center.ToString) IsNot Nothing Then
                    Dim dtRole As New Data.DataTable : dtRole = Session(clsManage.iSession.user_role_center.ToString)
                    If dtRole.Rows.Find(Me.Page.Page.ToString) IsNot Nothing Then
                        Dim dr As Data.DataRow = dtRole.Rows.Find(Me.Page.Page.ToString)
                        If dr(clsDB.roles.export) = False Then
                            linkExport96.Enabled = False
                            linkExport96G.Enabled = False
                            linkExport99.Enabled = False
                            linkExportCashDep.Enabled = False
                            linkExportGoldDep.Enabled = False
                            linkExportHis96.Enabled = False
                            linkExportHis96G.Enabled = False
                            linkExportHis99.Enabled = False
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
            If Request.QueryString("id") IsNot Nothing Then
                hdfCust_id.Value = Request.QueryString("id").ToString
                tabPortfolio.ActiveTabIndex = 0
                ucPortFolio1.CustID = hdfCust_id.Value
                ucPortFolio1.LoadPortFolio()

                gvTicket96.EmptyDataText = clsManage.EmptyDataText
                gvTicket99.EmptyDataText = clsManage.EmptyDataText
                gvTicket96H.EmptyDataText = clsManage.EmptyDataText
                gvTicket99H.EmptyDataText = clsManage.EmptyDataText
                gvCashTrans1.EmptyDataText = clsManage.EmptyDataText
                gvCashTrans2.EmptyDataText = clsManage.EmptyDataText

                objSrcTicket96.SelectParameters("cust_id").DefaultValue = hdfCust_id.Value
                objSrcTicket96.SelectParameters("gold_type").DefaultValue = "96"
                objSrcTicket96.SelectParameters("isHistory").DefaultValue = "false"
                gvTicket96.DataBind()

                objSrcTicket96G.SelectParameters("cust_id").DefaultValue = hdfCust_id.Value
                objSrcTicket96G.SelectParameters("gold_type").DefaultValue = "96G"
                objSrcTicket96G.SelectParameters("isHistory").DefaultValue = "false"
                gvTicket96G.DataBind()

                objSrcTicket99.SelectParameters("cust_id").DefaultValue = hdfCust_id.Value
                objSrcTicket99.SelectParameters("gold_type").DefaultValue = "99"
                objSrcTicket99.SelectParameters("isHistory").DefaultValue = "false"
                gvTicket99.DataBind()

                objSrcTicket96H.SelectParameters("cust_id").DefaultValue = hdfCust_id.Value
                objSrcTicket96H.SelectParameters("gold_type").DefaultValue = "96"
                objSrcTicket96H.SelectParameters("isHistory").DefaultValue = "true"
                gvTicket96H.DataBind()

                objSrcTicket96GH.SelectParameters("cust_id").DefaultValue = hdfCust_id.Value
                objSrcTicket96GH.SelectParameters("gold_type").DefaultValue = "96G"
                objSrcTicket96GH.SelectParameters("isHistory").DefaultValue = "true"
                gvTicket96GH.DataBind()

                objSrcTicket99H.SelectParameters("cust_id").DefaultValue = hdfCust_id.Value
                objSrcTicket99H.SelectParameters("gold_type").DefaultValue = "99"
                objSrcTicket99H.SelectParameters("isHistory").DefaultValue = "true"
                gvTicket99H.DataBind()

            End If
        End If
    End Sub

    Protected Sub gvTicket96_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTicket96.RowCreated
        If e.Row.RowType = DataControlRowType.Footer Then
            'set the columnspan
            e.Row.Cells(0).ColumnSpan = 5
            'remove the second cell
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells(0).Text = "Total"
            e.Row.Cells(0).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(1).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Right

            e.Row.Cells(1).Text = Double.Parse(ViewState("sumQuan96")).ToString("#,##0")
            e.Row.Cells(2).Text = Double.Parse(ViewState("sumAmount96")).ToString("#,##0")
        End If
    End Sub

    Protected Sub gvTicket96_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTicket96.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.Header Then
                ViewState("sumQuan96") = 0
                ViewState("sumAmount96") = 0
            End If

            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.Cells(2).FindControl("link"), HyperLink).NavigateUrl = "ticket_deal.aspx?page=dialy&id=" + e.Row.DataItem("ticket_id").ToString
                ViewState("sumQuan96") += Double.Parse(e.Row.DataItem("quantity"))
                ViewState("sumAmount96") += Double.Parse(e.Row.DataItem("amount"))

                e.Row.Cells(0).HorizontalAlign = HorizontalAlign.Center
                e.Row.Cells(0).Attributes.Add("style", "text-align:center")

                'set color จำนวน 7 , มูลค่าการซื้อขาย 8
                If e.Row.Cells(colQuan).Text <> "&nbsp;" AndAlso clsManage.convert2zero(e.Row.Cells(colQuan).Text) < 0 Then
                    e.Row.Cells(colQuan).ForeColor = Drawing.Color.Red
                End If
                If e.Row.Cells(colAmount).Text <> "&nbsp;" AndAlso clsManage.convert2zero(e.Row.Cells(colAmount).Text) < 0 Then
                    e.Row.Cells(colAmount).ForeColor = Drawing.Color.Red
                End If

            End If
            If e.Row.RowType = DataControlRowType.Footer Then
                'set color
                If e.Row.Cells(colFootAmount).Text <> "&nbsp;" AndAlso clsManage.convert2zero(e.Row.Cells(colFootAmount).Text) < 0 Then
                    e.Row.Cells(colFootAmount).ForeColor = Drawing.Color.Maroon
                End If
                If e.Row.Cells(colFootQuan).Text <> "&nbsp;" AndAlso clsManage.convert2zero(e.Row.Cells(colFootQuan).Text) < 0 Then
                    e.Row.Cells(colFootQuan).ForeColor = Drawing.Color.Maroon
                End If
               
            End If
        Catch ex As Exception
            Throw New Exception
        End Try

    End Sub

    Protected Sub gvTicket99_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTicket99.RowCreated
        If e.Row.RowType = DataControlRowType.Footer Then
           
            'set the columnspan
            e.Row.Cells(0).ColumnSpan = 5
            'remove the second cell
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells(0).Text = "Total"
            e.Row.Cells(0).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(1).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Right

            e.Row.Cells(1).Text = Double.Parse(ViewState("sumQuan99")).ToString("#,##0")
            e.Row.Cells(2).Text = Double.Parse(ViewState("sumAmount99")).ToString("#,##0")

        End If
    End Sub

    Protected Sub gvTicket99_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTicket99.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.Header Then
                ViewState("sumQuan99") = 0
                ViewState("sumAmount99") = 0
            End If

            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.Cells(2).FindControl("link"), HyperLink).NavigateUrl = "ticket_deal.aspx?page=dialy&id=" + e.Row.DataItem("ticket_id").ToString
                ViewState("sumQuan99") += Double.Parse(e.Row.DataItem("quantity"))
                ViewState("sumAmount99") += Double.Parse(e.Row.DataItem("amount"))


                'set color
                If e.Row.Cells(colQuan).Text <> "&nbsp;" AndAlso e.Row.Cells(colQuan).Text < 0 Then
                    e.Row.Cells(colQuan).ForeColor = Drawing.Color.Red
                End If
                If e.Row.Cells(colAmount).Text <> "&nbsp;" AndAlso e.Row.Cells(colAmount).Text < 0 Then
                    e.Row.Cells(colAmount).ForeColor = Drawing.Color.Red
                End If
               
            End If

            If e.Row.RowType = DataControlRowType.Footer Then
                'set color
                If e.Row.Cells(colFootAmount).Text <> "&nbsp;" AndAlso e.Row.Cells(colFootAmount).Text < 0 Then
                    e.Row.Cells(colFootAmount).ForeColor = Drawing.Color.Maroon
                End If
                If e.Row.Cells(colFootQuan).Text <> "&nbsp;" AndAlso e.Row.Cells(colFootQuan).Text < 0 Then
                    e.Row.Cells(colFootQuan).ForeColor = Drawing.Color.Maroon
                End If
               
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gvTicket96H_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTicket96H.RowCreated
        Try
            If e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(colQuanHis).Text = Double.Parse(ViewState("sumHisQuan96")).ToString("#,##0")
                e.Row.Cells(colAmountHis).Text = Double.Parse(ViewState("sumHisAmount96")).ToString("#,##0")

                'set the columnspan
                e.Row.Cells(0).ColumnSpan = 5
                'remove the second cell
                e.Row.Cells.Remove(e.Row.Cells(1))
                e.Row.Cells.Remove(e.Row.Cells(1))
                e.Row.Cells.Remove(e.Row.Cells(1))
                e.Row.Cells.Remove(e.Row.Cells(1))
                e.Row.Cells(0).Text = "Total"
                e.Row.Cells(0).HorizontalAlign = HorizontalAlign.Center
                e.Row.Cells(1).HorizontalAlign = HorizontalAlign.Right
                e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Right
                e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Right
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gvTicket96H_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTicket96H.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.Header Then
                ViewState("sumHisQuan96") = 0
                ViewState("sumHisAmount96") = 0
            End If

            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.Cells(2).FindControl("link"), HyperLink).NavigateUrl = "ticket_deal.aspx?page=dialy&id=" + e.Row.DataItem("ticket_id").ToString
                ViewState("sumHisQuan96") += Double.Parse(e.Row.DataItem("quantity"))
                ViewState("sumHisAmount96") += Double.Parse(e.Row.DataItem("amount"))

                'set color
                If e.Row.Cells(colQuanHis).Text <> "&nbsp;" AndAlso e.Row.Cells(colQuanHis).Text < 0 Then
                    e.Row.Cells(colQuanHis).ForeColor = Drawing.Color.Red
                End If
                If e.Row.Cells(colAmountHis).Text <> "&nbsp;" AndAlso e.Row.Cells(colAmountHis).Text < 0 Then
                    e.Row.Cells(colAmountHis).ForeColor = Drawing.Color.Red
                End If
            End If
            If e.Row.RowType = DataControlRowType.Footer Then
                'set color
                If e.Row.Cells(2).Text <> "&nbsp;" AndAlso e.Row.Cells(2).Text < 0 Then
                    e.Row.Cells(2).ForeColor = Drawing.Color.Maroon
                End If
                If e.Row.Cells(1).Text <> "&nbsp;" AndAlso e.Row.Cells(1).Text < 0 Then
                    e.Row.Cells(1).ForeColor = Drawing.Color.Maroon
                End If
            End If
        Catch ex As Exception

        End Try

    End Sub

    Protected Sub gvTicket99H_RowCreated(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTicket99H.RowCreated
        Try
            If e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(colQuanHis).Text = Double.Parse(ViewState("sumHisQuan99")).ToString("#,##0")
                e.Row.Cells(colAmountHis).Text = Double.Parse(ViewState("sumHisAmount99")).ToString("#,##0")

                'set the columnspan
                e.Row.Cells(0).ColumnSpan = 5
                'remove the second cell
                e.Row.Cells.Remove(e.Row.Cells(1))
                e.Row.Cells.Remove(e.Row.Cells(1))
                e.Row.Cells.Remove(e.Row.Cells(1))
                e.Row.Cells.Remove(e.Row.Cells(1))
                e.Row.Cells(0).Text = "Total"
                e.Row.Cells(0).HorizontalAlign = HorizontalAlign.Center
                e.Row.Cells(1).HorizontalAlign = HorizontalAlign.Right
                e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Right
                e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Right
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gvTicket99H_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTicket99H.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.Header Then
                ViewState("sumHisQuan99") = 0
                ViewState("sumHisAmount99") = 0
            End If

            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.Cells(2).FindControl("link"), HyperLink).NavigateUrl = "ticket_deal.aspx?page=dialy&id=" + e.Row.DataItem("ticket_id").ToString
                ViewState("sumHisQuan99") += Double.Parse(e.Row.DataItem("quantity"))
                ViewState("sumHisAmount99") += Double.Parse(e.Row.DataItem("amount"))

                'set color
                If e.Row.Cells(colQuanHis).Text <> "&nbsp;" AndAlso e.Row.Cells(colQuanHis).Text < 0 Then
                    e.Row.Cells(colQuanHis).ForeColor = Drawing.Color.Red
                End If
                If e.Row.Cells(colAmountHis).Text <> "&nbsp;" AndAlso e.Row.Cells(colAmountHis).Text < 0 Then
                    e.Row.Cells(colAmountHis).ForeColor = Drawing.Color.Red
                End If
            End If
            If e.Row.RowType = DataControlRowType.Footer Then
                'set color
                If e.Row.Cells(2).Text <> "&nbsp;" AndAlso e.Row.Cells(2).Text < 0 Then
                    e.Row.Cells(2).ForeColor = Drawing.Color.Maroon
                End If
                If e.Row.Cells(1).Text <> "&nbsp;" AndAlso e.Row.Cells(1).Text < 0 Then
                    e.Row.Cells(1).ForeColor = Drawing.Color.Maroon
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gvCashTrans1_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCashTrans1.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.Header Then
                ViewState("sumcash1") = 0
            End If
            If e.Row.RowType = DataControlRowType.DataRow Then

                If Not IsDBNull(e.Row.DataItem("ref_no")) AndAlso e.Row.DataItem("ref_no").ToString <> "" Then
                    If e.Row.DataItem("ref_no").ToString.Length = 11 And e.Row.DataItem("ref_no").ToString.Substring(0, 1) = "T" Then
                        CType(e.Row.FindControl("link"), HyperLink).NavigateUrl = "ticket_deal.aspx?page=dialy&id=" + e.Row.DataItem("ref_no").ToString
                    End If
                End If

                If Double.Parse(e.Row.Cells(2).Text) = 0 Then e.Row.Cells(2).Text = ""
                If Double.Parse(e.Row.Cells(3).Text) = 0 Then e.Row.Cells(3).Text = ""
                ViewState("sumcash1") += Double.Parse(e.Row.DataItem("deposit")) + (-Double.Parse(e.Row.DataItem("withdraw")))
                e.Row.Cells(4).Text = Double.Parse(ViewState("sumcash1")).ToString(clsManage.formatCurrency)
            End If
            If e.Row.RowType = DataControlRowType.Footer Then
                'lblCash.Text = Double.Parse(ViewState("sumcash1")).ToString(clsManage.formatCurrency)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub gvCashTrans2_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCashTrans2.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.Header Then
                ViewState("sumgold96") = 0
                ViewState("sumgold99") = 0
            End If
            If e.Row.RowType = DataControlRowType.DataRow Then
                If Not IsDBNull(e.Row.DataItem("ref_no")) Then
                    If e.Row.DataItem("ref_no").ToString <> "" AndAlso (e.Row.DataItem("ref_no").ToString.Length = 11 And e.Row.DataItem("ref_no").ToString.Substring(0, 1) = "T") Then
                        CType(e.Row.FindControl("link"), HyperLink).NavigateUrl = "ticket_deal.aspx?page=dialy&id=" + e.Row.DataItem("ref_no").ToString
                    End If
                End If

                If Double.Parse(e.Row.Cells(3).Text) = 0 Then e.Row.Cells(3).Text = ""
                If Double.Parse(e.Row.Cells(4).Text) = 0 Then e.Row.Cells(4).Text = ""
                If e.Row.DataItem("gold_type_id") = "96" Then
                    ViewState("sumgold96") += Double.Parse(e.Row.DataItem("deposit")) + (-Double.Parse(e.Row.DataItem("withdraw")))
                Else
                    ViewState("sumgold99") += Double.Parse(e.Row.DataItem("deposit")) + (-Double.Parse(e.Row.DataItem("withdraw")))
                End If
                e.Row.Cells(5).Text = Double.Parse(ViewState("sumgold96")).ToString(clsManage.formatQuantity)
                e.Row.Cells(6).Text = Double.Parse(ViewState("sumgold99")).ToString(clsManage.formatQuantity)
            End If
            If e.Row.RowType = DataControlRowType.Footer Then
                'lblGold96.Text = Double.Parse(ViewState("sumgold96")).ToString(clsManage.formatQuantity)
                'lblGold99.Text = Double.Parse(ViewState("sumgold99")).ToString(clsManage.formatQuantity)
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gvTicket96G_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTicket96G.RowCreated
        If e.Row.RowType = DataControlRowType.Footer Then
            'set the columnspan
            e.Row.Cells(0).ColumnSpan = 5
            'remove the second cell
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells.Remove(e.Row.Cells(1))
            e.Row.Cells(0).Text = "Total"
            e.Row.Cells(0).HorizontalAlign = HorizontalAlign.Center
            e.Row.Cells(1).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Right
            e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Right

            e.Row.Cells(1).Text = Double.Parse(ViewState("sumQuan96G")).ToString("#,##0")
            e.Row.Cells(2).Text = Double.Parse(ViewState("sumAmount96G")).ToString("#,##0")
        End If
    End Sub

    Protected Sub gvTicket96G_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTicket96G.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.Header Then
                ViewState("sumQuan96G") = 0
                ViewState("sumAmount96G") = 0
            End If

            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.Cells(2).FindControl("link"), HyperLink).NavigateUrl = "ticket_deal_g.aspx?page=dialy&id=" + e.Row.DataItem("ticket_id").ToString
                ViewState("sumQuan96G") += Double.Parse(e.Row.DataItem("quantity"))
                ViewState("sumAmount96G") += Double.Parse(e.Row.DataItem("amount"))

                e.Row.Cells(0).HorizontalAlign = HorizontalAlign.Center
                e.Row.Cells(0).Attributes.Add("style", "text-align:center")

                'set color จำนวน 7 , มูลค่าการซื้อขาย 8
                If e.Row.Cells(colQuan).Text <> "&nbsp;" AndAlso clsManage.convert2zero(e.Row.Cells(colQuan).Text) < 0 Then
                    e.Row.Cells(colQuan).ForeColor = Drawing.Color.Red
                End If
                If e.Row.Cells(colAmount).Text <> "&nbsp;" AndAlso clsManage.convert2zero(e.Row.Cells(colAmount).Text) < 0 Then
                    e.Row.Cells(colAmount).ForeColor = Drawing.Color.Red
                End If

            End If
            If e.Row.RowType = DataControlRowType.Footer Then
                'set color
                If e.Row.Cells(colFootAmount).Text <> "&nbsp;" AndAlso clsManage.convert2zero(e.Row.Cells(colFootAmount).Text) < 0 Then
                    e.Row.Cells(colFootAmount).ForeColor = Drawing.Color.Maroon
                End If
                If e.Row.Cells(colFootQuan).Text <> "&nbsp;" AndAlso clsManage.convert2zero(e.Row.Cells(colFootQuan).Text) < 0 Then
                    e.Row.Cells(colFootQuan).ForeColor = Drawing.Color.Maroon
                End If

            End If
        Catch ex As Exception
            Throw New Exception
        End Try
    End Sub

    Protected Sub gvTicket96GH_RowCreated(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTicket96GH.RowCreated
        Try
            If e.Row.RowType = DataControlRowType.Footer Then
                e.Row.Cells(colQuanHis).Text = Double.Parse(ViewState("sumHisQuan96")).ToString("#,##0")
                e.Row.Cells(colAmountHis).Text = Double.Parse(ViewState("sumHisAmount96")).ToString("#,##0")

                'set the columnspan
                e.Row.Cells(0).ColumnSpan = 5
                'remove the second cell
                e.Row.Cells.Remove(e.Row.Cells(1))
                e.Row.Cells.Remove(e.Row.Cells(1))
                e.Row.Cells.Remove(e.Row.Cells(1))
                e.Row.Cells.Remove(e.Row.Cells(1))
                e.Row.Cells(0).Text = "Total"
                e.Row.Cells(0).HorizontalAlign = HorizontalAlign.Center
                e.Row.Cells(1).HorizontalAlign = HorizontalAlign.Right
                e.Row.Cells(2).HorizontalAlign = HorizontalAlign.Right
                e.Row.Cells(3).HorizontalAlign = HorizontalAlign.Right
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gvTicket96GH_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTicket96GH.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.Header Then
                ViewState("sumHisQuan96G") = 0
                ViewState("sumHisAmount96G") = 0
            End If

            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.Cells(2).FindControl("link"), HyperLink).NavigateUrl = "ticket_deal_g.aspx?page=dialy&id=" + e.Row.DataItem("ticket_id").ToString
                ViewState("sumHisQuan96G") += Double.Parse(e.Row.DataItem("quantity"))
                ViewState("sumHisAmount96G") += Double.Parse(e.Row.DataItem("amount"))

                'set color
                If e.Row.Cells(colQuanHis).Text <> "&nbsp;" AndAlso e.Row.Cells(colQuanHis).Text < 0 Then
                    e.Row.Cells(colQuanHis).ForeColor = Drawing.Color.Red
                End If
                If e.Row.Cells(colAmountHis).Text <> "&nbsp;" AndAlso e.Row.Cells(colAmountHis).Text < 0 Then
                    e.Row.Cells(colAmountHis).ForeColor = Drawing.Color.Red
                End If
            End If
            If e.Row.RowType = DataControlRowType.Footer Then
                'set color
                If e.Row.Cells(2).Text <> "&nbsp;" AndAlso e.Row.Cells(2).Text < 0 Then
                    e.Row.Cells(2).ForeColor = Drawing.Color.Maroon
                End If
                If e.Row.Cells(1).Text <> "&nbsp;" AndAlso e.Row.Cells(1).Text < 0 Then
                    e.Row.Cells(1).ForeColor = Drawing.Color.Maroon
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub
End Class
