
Imports System.IO.StringWriter
Imports System.IO
Partial Class cust_trans_search
    Inherits System.Web.UI.Page
    Dim msg As String = ""

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Session(clsManage.iSession.cust_id.ToString) IsNot Nothing Then
            'update online time
            msg = clsMain.updateTimeOnline(Session(clsManage.iSession.cust_id.ToString).ToString, Session.SessionID)
            If msg <> "" Then
                Session.Clear()
                clsManage.Script(Page, "alert(' " + msg + "');top.window.location.href='login.aspx'", "loginDup1")
                Exit Sub
            End If


            If Not Page.IsPostBack Then
                If Request.QueryString("m") IsNot Nothing Then
                    hdfMode.Value = clsManage.tradeMode.tran.ToString
                    ViewState(clsManage.iSession.cust_id.ToString) = Session(clsManage.iSession.cust_id.ToString).ToString
                    If Request.QueryString("s") IsNot Nothing Then
                        hdfSale_id.Value = Session(clsManage.iSession.user_id.ToString).ToString
                    Else
                        hdfSale_id.Value = ""
                    End If

                    gvTrade.EmptyDataText = clsManage.EmptyDataText
                    hdfIsRealtime.Value = "y"
                    hdfConfirm.Value = "n"
                    For i As Integer = 0 To 23 : ddl1Hour.Items.Add(i) : ddl2Hour.Items.Add(i) : Next
                    For i As Integer = 0 To 59 : ddl1Min.Items.Add(i.ToString("00")) : ddl2Min.Items.Add(i.ToString("00")) : Next
                    ddl1Hour.SelectedValue = "9" : ddl1Min.SelectedValue = "30"
                    ddl2Hour.SelectedValue = "23" : ddl2Min.SelectedValue = "59"
                    txt1Date_CalendarExtender.Format = clsManage.formatDateTime : txt2Date_CalendarExtender.Format = clsManage.formatDateTime
                    txt1Date.Text = Now.ToString(clsManage.formatDateTime) : txt2Date.Text = Now.ToString(clsManage.formatDateTime)
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
                e.Row.Cells(7).Text = "Reject by " + e.Row.DataItem("reject_type").ToString
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

    Function getPurityCustomer(ByVal purity As String) As String
        Dim result As String = ""
        If purity = "" Then
            result = "96','99"
        ElseIf purity = "96" Then
            result = "96"
        Else
            result = "99"
        End If

        Return result
    End Function

    Sub refreshGrid(Optional ByVal sleep As Boolean = True)
        Try
            If msg <> "" Then Exit Sub
            Dim cust_id As String = ""
            If Session(clsManage.iSession.cust_id.ToString) IsNot Nothing Then
                cust_id = Session(clsManage.iSession.cust_id.ToString).ToString
            Else
                clsManage.Script(Page, "top.window.location.href='login.aspx'", "NoSession")
                Exit Sub
            End If

            If Session(clsManage.iSession.cust_id.ToString).ToString <> ViewState(clsManage.iSession.cust_id.ToString).ToString Then
                Session.Clear()
                clsManage.Script(Page, "top.window.location.href='login.aspx'", "DupLogin")
                Exit Sub
            End If

            Dim mode As String = clsManage.getItemListSql(cbMode)
            Dim purity As String = getPurityCustomer(rdoPurity.SelectedValue)

            Dim pDate1 As New DateTime
            Dim pDate2 As New DateTime
            Dim period As String = ""

            If rdoTime2.Checked Then
                Dim tempDate1 As New DateTime
                Dim tempDate2 As New DateTime
                tempDate1 = DateTime.ParseExact(txt1Date.Text, clsManage.formatDateTime, Nothing)
                tempDate2 = DateTime.ParseExact(txt2Date.Text, clsManage.formatDateTime, Nothing)
                pDate1 = pDate1.AddDays(tempDate1.Day - 1).AddMonths(tempDate1.Month - 1).AddYears(tempDate1.Year - 1).AddHours(ddl1Hour.SelectedValue).AddMinutes(ddl1Min.SelectedValue)
                pDate2 = pDate2.AddDays(tempDate2.Day - 1).AddMonths(tempDate2.Month - 1).AddYears(tempDate2.Year - 1).AddHours(ddl2Hour.SelectedValue).AddMinutes(ddl2Min.SelectedValue).AddSeconds(59)
                period = "period"
            End If

            objSrcTrade.SelectParameters("mode").DefaultValue = mode
            objSrcTrade.SelectParameters("order").DefaultValue = rdoOrder.SelectedValue
            objSrcTrade.SelectParameters("type").DefaultValue = rdoType.SelectedValue
            objSrcTrade.SelectParameters("purity").DefaultValue = purity
            objSrcTrade.SelectParameters("cust_id").DefaultValue = cust_id
            objSrcTrade.SelectParameters("period").DefaultValue = period
            objSrcTrade.SelectParameters("pDate1").DefaultValue = pDate1
            objSrcTrade.SelectParameters("pDate2").DefaultValue = pDate2
            objSrcTrade.SelectParameters("sale_id").DefaultValue = hdfSale_id.Value

            gvTrade.DataBind()


            Dim h As Integer = 300
            If gvTrade.Rows.Count <> 0 Then
                h = 200 + (gvTrade.Rows.Count) * 25
            End If

            clsManage.Script(Page, "setIframeHeight('ifmBlotter','" & h & "');")

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        refreshGrid()
    End Sub

    Protected Sub UpdateProg1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles UpdateProg1.Load
        System.Threading.Thread.Sleep(UpdateProg1.DisplayAfter + 300)
    End Sub
    Public Overrides Sub VerifyRenderingInServerForm(ByVal control As Control)
    End Sub
    Protected Sub linkExcel_Click(sender As Object, e As System.EventArgs) Handles linkExcel.Click
        Try
            clsManage.ExportToExcelTradeOrder(gvTrade, "Order_Blotter" + DateTime.Now.ToString("ddmmyyyy"))
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "err")
        End Try
    End Sub

End Class
