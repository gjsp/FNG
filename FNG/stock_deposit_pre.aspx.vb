
Partial Class stock_deposit_pre
    Inherits basePage

    Protected Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        If Not Page.IsPostBack Then
            Try
                If Session(clsManage.iSession.user_role_center.ToString) IsNot Nothing Then
                    Dim dtRole As New Data.DataTable : dtRole = Session(clsManage.iSession.user_role_center.ToString)
                    If dtRole.Rows.Find(Me.Page.Page.ToString) IsNot Nothing Then
                        Dim dr As Data.DataRow = dtRole.Rows.Find(Me.Page.Page.ToString)

                        If dr(clsDB.roles.update) = False Then
                            ViewState(clsDB.roles.update) = False
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
            'txtDate_CalendarExtender.Format = clsManage.formatDateTime
            'txtDate.Text = DateTime.Now.AddDays(-1).ToString(clsManage.formatDateTime)
            tapImport.ActiveTabIndex = 1
            txtDate1.Attributes.Add("onkeypress", "return false;")
            txtDate2.Attributes.Add("onkeypress", "return false;")
            txtDate1_CalendarExtender.Format = clsManage.formatDateTime
            txtDate2_CalendarExtender.Format = clsManage.formatDateTime
            txtDate1.Text = DateTime.Now.ToString(clsManage.formatDateTime)
            txtDate2.Text = DateTime.Now.ToString(clsManage.formatDateTime)

            txtCDate1.Attributes.Add("onkeypress", "return false;")
            txtCDate2.Attributes.Add("onkeypress", "return false;")
            txtcDate1_CalendarExtender.Format = clsManage.formatDateTime
            txtcDate2_CalendarExtender.Format = clsManage.formatDateTime
            txtCDate1.Text = DateTime.Now.ToString(clsManage.formatDateTime)
            txtCDate2.Text = DateTime.Now.ToString(clsManage.formatDateTime)

            gvAssetCash.EmptyDataText = clsManage.EmptyDataText
            gvAssetGold96.EmptyDataText = clsManage.EmptyDataText

            gvAssetCash.DataBind()
            gvAssetGold96.DataBind()
        End If
    End Sub

    Protected Sub upgSearch_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles upgSearch.Load
        System.Threading.Thread.Sleep(upgSearch.DisplayAfter)
    End Sub

    Protected Sub imgSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSearch.Click
        Try
            gvAssetGold96.DataSourceID = "objSrcAssetGold96"

            Dim purity As String = ""
            Dim cmma As String = "','"
            For i As Integer = 0 To cblPurity.Items.Count - 1
                If cblPurity.Items(i).Selected = True Then
                    purity += IIf(purity = "", cblPurity.Items(i).Value, cmma + cblPurity.Items(i).Value)
                End If
            Next
            If purity = "" Then
                For i As Integer = 0 To cblPurity.Items.Count - 1
                    purity += IIf(purity = "", cblPurity.Items(i).Value, cmma + cblPurity.Items(i).Value)
                Next
            End If

            objSrcAssetGold96.SelectParameters("purity").DefaultValue = purity
            objSrcAssetGold96.SelectParameters("date1").DefaultValue = txtDate1.Text
            objSrcAssetGold96.SelectParameters("date2").DefaultValue = txtDate2.Text
            gvAssetGold96.DataBind()
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Protected Sub imgCashSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgCashSearch.Click
        Try
            gvAssetCash.DataSourceID = "objSrcAssetCash"

            objSrcAssetCash.SelectParameters("purity").DefaultValue = ""
            objSrcAssetCash.SelectParameters("date1").DefaultValue = txtCDate1.Text
            objSrcAssetCash.SelectParameters("date2").DefaultValue = txtCDate2.Text
            gvAssetCash.DataBind()

          
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Protected Sub gvAssetCash_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAssetCash.RowDataBound
        Try
            'If e.Row.RowType = DataControlRowType.Header Then
            '    ViewState("netcash") = 0
            'End If
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.FindControl("linkCust"), HyperLink).NavigateUrl = "customer_trans.aspx?cust_id=" + e.Row.DataItem("cust_id").ToString
                If e.Row.DataItem("pre").ToString <> "y" Then CType(e.Row.FindControl("linkUpdate"), LinkButton).Enabled = False
                'ViewState("netcash") += clsManage.convert2zero(e.Row.DataItem("amount").ToString)
                'e.Row.Cells(gvAssetCash.Columns.Count - 1).Text = clsManage.convert2zero(ViewState("netcash")).ToString(clsManage.formatCurrency)
                'If e.Row.DataItem("type") = "Withdraw" Then e.Row.Cells(2).ForeColor = Drawing.Color.Red
                CType(e.Row.FindControl("txtQuantity"), TextBox).Text = clsManage.convert2Currency(e.Row.DataItem("amount").ToString)
                CType(e.Row.FindControl("lblQuantity"), Label).Text = clsManage.convert2Currency(e.Row.DataItem("amount").ToString)
                CType(e.Row.FindControl("txtQuantity"), TextBox).Attributes.Add("style", "text-align:right")

                If ViewState(clsDB.roles.update) IsNot Nothing AndAlso ViewState(clsDB.roles.update) = False Then
                    CType(e.Row.FindControl("linkUpdate"), LinkButton).Enabled = False
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub gvAssetGold96_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAssetGold96.RowDataBound
        Try
            'If e.Row.RowType = DataControlRowType.Header Then
            '    ViewState("netgold96") = 0
            '    ViewState("netgold99N") = 0
            '    ViewState("netgold99L") = 0
            'End If
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.FindControl("lblTrans"), LinkButton).Text = CDate(e.Row.DataItem("datetime")).ToString("dd/MM/yyyy")
                CType(e.Row.FindControl("linkCust"), HyperLink).NavigateUrl = "customer_trans.aspx?cust_id=" + e.Row.DataItem("cust_id").ToString
                If e.Row.DataItem("pre").ToString <> "y" Then CType(e.Row.FindControl("linkUpdate"), LinkButton).Enabled = False
               
                'Dim sumGold As Double = clsManage.convert2zero(e.Row.DataItem("quantity").ToString)
                'If e.Row.DataItem("gold_type_id").ToString = "96" Then
                '    ViewState("netgold96") += sumGold
                'ElseIf e.Row.DataItem("gold_type_id").ToString = "99N" Then
                '    ViewState("netgold99N") += sumGold
                'ElseIf e.Row.DataItem("gold_type_id").ToString = "99L" Then
                '    ViewState("netgold99L") += sumGold
                'End If
                'e.Row.Cells(gvAssetGold96.Columns.Count - 3).Text = clsManage.convert2zero(ViewState("netgold96")).ToString(clsManage.formatQuantity)
                'e.Row.Cells(gvAssetGold96.Columns.Count - 2).Text = clsManage.convert2zero(ViewState("netgold99N")).ToString(clsManage.formatQuantity)
                'e.Row.Cells(gvAssetGold96.Columns.Count - 1).Text = clsManage.convert2zero(ViewState("netgold99L")).ToString(clsManage.formatQuantity)
                CType(e.Row.FindControl("txtQuantity"), TextBox).Text = clsManage.convert2Currency(e.Row.DataItem("quantity").ToString)
                CType(e.Row.FindControl("lblQuantity"), Label).Text = clsManage.convert2Currency(e.Row.DataItem("quantity").ToString)
                CType(e.Row.FindControl("txtQuantity"), TextBox).Attributes.Add("style", "text-align:right")

                If ViewState(clsDB.roles.update) IsNot Nothing AndAlso ViewState(clsDB.roles.update) = False Then
                    CType(e.Row.FindControl("linkUpdate"), LinkButton).Enabled = False
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

    'Sub bindData()
    '    Try
    '        gvAssetGold96.DataSourceID = "objSrcAssetGold96"

    '        Dim purity As String = ""
    '        If cblPurity.Items(0).Selected = True Then purity = "96"
    '        If cblPurity.Items(1).Selected = True Then
    '            If purity = "" Then purity = "99N" Else purity += "','" + "99N"
    '        End If

    '        If cblPurity.Items(2).Selected = True Then
    '            If purity = "" Then purity = "99L" Else purity += "','" + "99L"
    '        End If

    '        If cblPurity.Items(0).Selected = False And cblPurity.Items(1).Selected = False And cblPurity.Items(2).Selected = False Then
    '            purity = "96','99N','99L"
    '        End If

    '        objSrcAssetCash.SelectParameters("purity").DefaultValue = ""
    '        objSrcAssetCash.SelectParameters("date1").DefaultValue = ""
    '        objSrcAssetCash.SelectParameters("date2").DefaultValue = ""
    '        gvAssetCash.DataBind()

    '        objSrcAssetGold96.SelectParameters("purity").DefaultValue = purity
    '        objSrcAssetGold96.SelectParameters("date1").DefaultValue = txtDate1.Text
    '        objSrcAssetGold96.SelectParameters("date2").DefaultValue = txtDate2.Text
    '        gvAssetGold96.DataBind()
    '    Catch ex As Exception
    '        clsManage.alert(Page, ex.Message)
    '    End Try
    'End Sub

    Protected Sub linkUpdate_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Update(sender, CType(CType(sender, LinkButton).Parent.Parent.Parent.Parent, GridView))
    End Sub

    Protected Sub linkSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Save(sender, CType(CType(sender, LinkButton).Parent.Parent.Parent.Parent, GridView))
    End Sub

    Protected Sub linkCancel_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Cencal(sender, CType(CType(sender, LinkButton).Parent.Parent.Parent.Parent, GridView))
    End Sub

    Public Sub Update(ByVal sender As Object, ByVal gv As GridView)
        Try
            Dim i As Integer = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow).RowIndex
            CType(sender, LinkButton).Visible = False

            gv.Rows(i).FindControl("linkSave").Visible = True
            gv.Rows(i).FindControl("linkCancel").Visible = True
            gv.Rows(i).FindControl("txtQuantity").Visible = True
            gv.Rows(i).FindControl("lblQuantity").Visible = False

            gv.Rows(i).FindControl("txtNote").Visible = True
            gv.Rows(i).FindControl("lblNote").Visible = False
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Public Sub Save(ByVal sender As Object, ByVal gv As GridView)
        Try
            Dim i As Integer = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow).RowIndex
            CType(sender, LinkButton).Visible = False
            gv.Rows(i).FindControl("linkCancel").Visible = False
            gv.Rows(i).FindControl("linkUpdate").Visible = True
            gv.Rows(i).FindControl("txtQuantity").Visible = False
            gv.Rows(i).FindControl("lblQuantity").Visible = True
            gv.Rows(i).FindControl("txtNote").Visible = False
            gv.Rows(i).FindControl("lblNote").Visible = True

            Dim result As Boolean = clsDB.updateDepositPre(gv.DataKeys(0).Value)
            If result Then
                gv.DataBind()
                clsManage.alert(Page, "Save Complete.")
            End If
        Catch ex As Exception
            gv.DataBind()
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Public Sub Cencal(ByVal sender As Object, ByVal gv As GridView)
        Try
            Dim i As Integer = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow).RowIndex
            CType(sender, LinkButton).Visible = False
            gv.Rows(i).FindControl("linkSave").Visible = False
            gv.Rows(i).FindControl("linkUpdate").Visible = True
            gv.Rows(i).FindControl("txtQuantity").Visible = False
            gv.Rows(i).FindControl("lblQuantity").Visible = True
            gv.Rows(i).FindControl("txtNote").Visible = False
            gv.Rows(i).FindControl("lblNote").Visible = True
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub


    Protected Sub lblTrans_Click(sender As Object, e As System.EventArgs)
        Dim i As Integer = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow).RowIndex
        Dim type As String = "D/W"
        Dim billing As String = ""
        clsManage.Script(Page, "window.open('ticket_bill.aspx?ref=" + gvAssetGold96.DataKeys(i).Value.ToString + "&type=" + type + "&billing=" + billing + "','_blank');")
    End Sub
End Class
