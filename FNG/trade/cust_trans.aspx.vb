
Imports System.IO.StringWriter
Imports System.IO

Partial Class cust_trans
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session(clsManage.iSession.cust_id.ToString) IsNot Nothing Then
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

                clsManage.Script(Page, "alert(' " + msg + "');top.window.location.href='login.aspx'", "loginDup1")
                Exit Sub
            End If


            If Not Page.IsPostBack Then
                ViewState(clsManage.iSession.cust_id.ToString) = Session(clsManage.iSession.cust_id.ToString).ToString
                If Request.QueryString("m") IsNot Nothing Then
                    If Request.QueryString("m").ToString = "leave" Then
                        hdfMode.Value = clsManage.tradeMode.tran.ToString
                    Else
                        hdfMode.Value = Request.QueryString("m").ToString
                    End If

                    If Request.QueryString("s") IsNot Nothing Then
                        hdfSale_id.Value = Session(clsManage.iSession.user_id.ToString).ToString
                    Else
                        hdfSale_id.Value = ""
                    End If

                    hdfTradeLogId.Value = clsMain.getTradeLogId(hdfMode.Value)
                    hdfTradeLogIdForGrid.Value = hdfTradeLogId.Value

                    gvTrade.EmptyDataText = clsManage.EmptyDataText
                    hdfIsRealtime.Value = "y"
                    hdfConfirm.Value = "n"

                End If
            End If
        End If
    End Sub

    Protected Sub gvTrade_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTrade.RowDataBound
       
        If e.Row.RowType = DataControlRowType.DataRow Then

            'Change Wording
            If e.Row.DataItem("leave_order").ToString = "order" Then
                e.Row.Cells(1).Text = "Order"
            Else
                e.Row.Cells(1).Text = "Leave Order"
            End If
            If e.Row.DataItem("type").ToString = "sell" Then
                e.Row.Cells(3).Text = "Sell"
            Else
                e.Row.Cells(3).Text = "Buy"
            End If

            CType(e.Row.FindControl("btnAccept99"), ImageButton).Attributes.Add("onclick", "return fastConfirm('" + hdfConfirm.ClientID + "','Do you want to accept.');")
            CType(e.Row.FindControl("btnReject99"), ImageButton).Attributes.Add("onclick", "return fastConfirm('" + hdfConfirm.ClientID + "','Do you want to reject.');")

            If hdfMode.Value = clsManage.tradeMode.tran.ToString Then
                If e.Row.DataItem("mode").ToString = clsManage.tradeMode.tran.ToString Then
                    If e.Row.DataItem("leave_order").ToString <> "order" Then
                        CType(e.Row.FindControl("btnReject99"), ImageButton).Visible = True
                    Else
                        CType(e.Row.FindControl("btnReject99"), ImageButton).Visible = False
                    End If
                Else
                    CType(e.Row.FindControl("btnReject99"), ImageButton).Visible = False
                End If
            End If

            If e.Row.DataItem("mode").ToString = clsManage.tradeMode.reject.ToString And e.Row.DataItem("reject_type").ToString <> "" Then
                e.Row.Cells(7).Text = "Reject By " + e.Row.DataItem("reject_type").ToString
            End If

            If e.Row.DataItem("new_trade").ToString = "y" Then
                'e.Row.BackColor = New Drawing.ColorConverter().ConvertFromString("#FFCE9D")
                e.Row.Font.Bold = True
            End If

            If e.Row.DataItem("gold_type_id").ToString = "96" Then
                e.Row.Cells(6).Text = "96.50"
            Else
                e.Row.Cells(6).Text = "99.99"
            End If

            If hdfMode.Value = clsManage.tradeMode.tran.ToString Then
                If e.Row.DataItem("mode") = "reject" Then
                    e.Row.BackColor = New Drawing.ColorConverter().ConvertFromString("#FFA4A4") '#FFA4A4")
                ElseIf e.Row.DataItem("mode") = "accept" Then
                    e.Row.Cells(7).Text = "Accept"
                    e.Row.BackColor = New Drawing.ColorConverter().ConvertFromString("#B8CCE4") '"#66CCFF")
                ElseIf e.Row.DataItem("mode") = "tran" Then
                    e.Row.Cells(7).Text = "Pending"
                    e.Row.Cells(gvTrade.Columns.Count - 3).Text = "" 'modifier date = ""
                    'e.Row.BackColor = New Drawing.ColorConverter().ConvertFromString("#DFDFDF")
                End If
            End If
        End If
    End Sub

#Region "Update Mode"
    Sub updateMode(ByVal sender As Object, ByVal gv As GridView, ByVal mode As String)
        Try
            Dim i As Integer = CType(CType(sender, ImageButton).Parent.Parent, GridViewRow).RowIndex
            Dim id As String = gv.DataKeys(i).Value
            Dim type As String = gv.DataKeys(i).Values(1).ToString
            Dim price As String = gv.DataKeys(i).Values(2).ToString
            Dim purity As String = gv.DataKeys(i).Values(3).ToString
            Dim result As Integer = clsMain.UpdateTicketPortfolioByMode(id, mode, Me.Session(clsManage.iSession.user_id.ToString).ToString, type, price, purity, Session(clsManage.iSession.cust_id.ToString).ToString, "")
            If result > 0 Then
                'plus for not update data
                hdfTradeLogId.Value = clsManage.convert2zero(hdfTradeLogId.Value) + 1
            End If
            refreshGrid()
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub
    Protected Sub btnAccept96_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        updateMode(sender, CType(CType(sender, ImageButton).Parent.Parent.NamingContainer, GridView), clsManage.tradeMode.accept.ToString)
    End Sub
    Protected Sub btnAccept99_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        updateMode(sender, CType(CType(sender, ImageButton).Parent.Parent.NamingContainer, GridView), clsManage.tradeMode.accept.ToString)
    End Sub
    Protected Sub btnReject96_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        updateMode(sender, CType(CType(sender, ImageButton).Parent.Parent.NamingContainer, GridView), clsManage.tradeMode.reject.ToString)
    End Sub
    Protected Sub btnReject99_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        updateMode(sender, CType(CType(sender, ImageButton).Parent.Parent.NamingContainer, GridView), clsManage.tradeMode.reject.ToString)
    End Sub
#End Region

    <System.Web.Services.WebMethod()> _
    Public Shared Function getTradeMaxId(ByVal mode As String) As String
        Return clsMain.getTradeLogId(mode)
    End Function

    Protected Sub upGrid_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles upGrid.Load
        refreshGrid()
    End Sub

    Sub refreshGrid()
        Try
            Dim cust_id As String = ""
            If Session(clsManage.iSession.cust_id.ToString) IsNot Nothing Then
                cust_id = Session(clsManage.iSession.cust_id.ToString).ToString
            Else
                clsManage.Script(Page, "top.window.location.href='login.aspx'", "NoSession")
                Exit Sub
            End If
            If Session(clsManage.iSession.cust_id.ToString).ToString <> ViewState(clsManage.iSession.cust_id.ToString).ToString Then

                Session.Remove(clsManage.iSession.cust_id.ToString)
                Session.Remove(clsManage.iSession.user_id.ToString)
                Session.Remove(clsManage.iSession.user_name.ToString)
                Session.Remove(clsManage.iSession.cust_lv.ToString)
                Session.Remove(clsManage.iSession.role.ToString)
                Session.Remove(clsManage.iSession.first_trade.ToString)
                Session.Remove(clsManage.iSession.online_id.ToString)

                clsManage.Script(Page, "top.window.location.href='login.aspx'", "DupLogin")
                Exit Sub
            End If

            Dim pDate1 As New DateTime
            Dim pDate2 As New DateTime
            Dim period As String = ""

            Dim onlyLeave As String = "n"
            If Request.QueryString("m").ToString = "leave" Then
                onlyLeave = "y"
            End If

            objSrcTrade.SelectParameters("mode").DefaultValue = hdfMode.Value
            objSrcTrade.SelectParameters("type").DefaultValue = ""
            objSrcTrade.SelectParameters("purity").DefaultValue = ""
            objSrcTrade.SelectParameters("cust_id").DefaultValue = cust_id
            objSrcTrade.SelectParameters("max_trade_id").DefaultValue = hdfTradeLogIdForGrid.Value
            objSrcTrade.SelectParameters("period").DefaultValue = period
            objSrcTrade.SelectParameters("pDate1").DefaultValue = pDate1
            objSrcTrade.SelectParameters("pDate2").DefaultValue = pDate2
            objSrcTrade.SelectParameters("onlyLeave").DefaultValue = onlyLeave
            objSrcTrade.SelectParameters("sortPrice").DefaultValue = "n"
            objSrcTrade.SelectParameters("sale_id").DefaultValue = hdfSale_id.Value

            gvTrade.DataBind()

            'set Gridview(background-color,show column)
            'set iframe height
            Dim strColor As String = ""
            Dim iFrameMode As String = ""
            Dim ExcelName As String = ""
            'Dim scriptRefreshSumAccept As String = ""

            If hdfMode.Value = clsManage.tradeMode.tran.ToString Then
                If Request.QueryString("m").ToString = "leave" Then
                    strColor = "#4F9502"
                    iFrameMode = "ifmLeave"
                    ExcelName = "Leave_Order_"
                ElseIf Request.QueryString("m").ToString = "blotter" Then
                    strColor = "#4F9502"
                    iFrameMode = "ifmBlotter"
                    ExcelName = "Order_Blotter_"
                Else
                    strColor = "#4F9502"
                    iFrameMode = "ifmTran"
                    ExcelName = "Order_"
                End If

                'ถ้าเป็น customer ไม่ต้องโชว์ปุ่ม update mode and reject
                If Session(clsManage.iSession.cust_id.ToString) IsNot Nothing Then
                    gvTrade.Columns(gvTrade.Columns.Count - 1).Visible = True
                    gvTrade.Columns(gvTrade.Columns.Count - 2).Visible = False
                Else
                    gvTrade.Columns(gvTrade.Columns.Count - 1).Visible = True
                    gvTrade.Columns(gvTrade.Columns.Count - 2).Visible = True
                End If

            ElseIf hdfMode.Value = clsManage.tradeMode.accept.ToString Then
                strColor = "#104587"
                iFrameMode = "ifmAcc"
                ExcelName = "Order_Accept_"
                gvTrade.Columns(7).Visible = False 'colomn mode
                gvTrade.Columns(gvTrade.Columns.Count - 1).Visible = False
                gvTrade.Columns(gvTrade.Columns.Count - 2).Visible = False
            ElseIf hdfMode.Value = clsManage.tradeMode.reject.ToString Then
                strColor = "#990000"
                iFrameMode = "ifmRej"
                ExcelName = "Order_Reject_"
                'gvTrade.Columns(7).Visible = False 'colomn mode
                gvTrade.Columns(gvTrade.Columns.Count - 1).Visible = False
                gvTrade.Columns(gvTrade.Columns.Count - 2).Visible = False
            End If
            Dim h As Integer = 0
            If gvTrade.Rows.Count <> 0 Then
                h = 40 + (gvTrade.Rows.Count) * 25
            End If

            Dim scriptSummary As String = ""
            If hdfMode.Value = clsManage.tradeMode.accept.ToString And cust_id = "" Then
                'Sumary between buy and sell
                Dim buy96 As Double = 0
                Dim sell96 As Double = 0
                Dim buy99 As Double = 0
                Dim sell99 As Double = 0

                Dim dt As New Data.DataTable
                dt = clsMain.getTradeSummaryAccept(period, pDate1, pDate2)
                If dt.Rows.Count > 0 Then
                    If dt.Rows(0)("buy96") IsNot DBNull.Value Then buy96 = dt.Rows(0)("buy96").ToString
                    If dt.Rows(0)("sell96") IsNot DBNull.Value Then sell96 = dt.Rows(0)("sell96").ToString
                    If dt.Rows(0)("buy99") IsNot DBNull.Value Then buy99 = dt.Rows(0)("buy99").ToString
                    If dt.Rows(0)("sell99") IsNot DBNull.Value Then sell99 = dt.Rows(0)("sell99").ToString
                End If
                Dim net96 As String = clsManage.convert2Quantity(clsManage.convert2zero(sell96) - clsManage.convert2zero(buy96))
                Dim net99 As String = clsManage.convert2Quantity(clsManage.convert2zero(sell99) - clsManage.convert2zero(buy99))

                'ส่งออกนอก iframe
                scriptSummary = String.Format("top.window.setSummary('{0}','{1}','{2}','{3}','{4}','{5}');", _
                clsManage.convert2Quantity(buy96), clsManage.convert2Quantity(sell96), net96, clsManage.convert2Quantity(buy99), clsManage.convert2Quantity(sell99), net99)
            End If
            ViewState("excel_name") = ExcelName
            clsManage.Script(Page, "setIframeHeight('" + iFrameMode + "','" & h & "');" + scriptSummary + "")
            gvTrade.HeaderStyle.BackColor = New Drawing.ColorConverter().ConvertFromString(strColor)

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub UpdateProg1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles UpdateProg1.Load
        System.Threading.Thread.Sleep(UpdateProg1.DisplayAfter + 300)
    End Sub

    'For Excel
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    End Sub

    Protected Sub linkExcel_Click(sender As Object, e As System.EventArgs) Handles linkExcel.Click
        Try
            clsManage.ExportToExcelTradeOrder(gvTrade, ViewState("excel_name").ToString + DateTime.Now.ToString("ddmmyyyy"))
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "err")
        End Try
    End Sub

End Class
