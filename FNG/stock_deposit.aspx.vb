
Partial Class stock_deposit
    Inherits basePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            clsManage.getDropDownlistValue(ddlTeam, clsDB.getTeam, "-- All --")
            ddlTeam.Items.Insert(IIf(ddlTeam.Items.Count > 0, 1, 0), New ListItem(clsManage.msgRequireSelect, "none"))
            ddlTeam.Attributes.Add("onchange", "$get('" & btnSearch.ClientID & "').click();")
            objSrcDep.SelectParameters("team_id").DefaultValue = ddlTeam.SelectedValue
            gvDeposit.DataBind()
        End If
    End Sub

    Protected Sub gvDeposit_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDeposit.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.Header Then
                ViewState("netcash") = 0
                ViewState("netgold96") = 0
                ViewState("netgold99N") = 0
                ViewState("netgold99L") = 0
            End If
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.FindControl("linkCust"), HyperLink).NavigateUrl = "customer_trans.aspx?cust_id=" + gvDeposit.DataKeys(e.Row.RowIndex).Value

                ViewState("netcash") += clsManage.convert2zero(e.Row.DataItem("cash").ToString)
                ViewState("netgold96") += clsManage.convert2zero(e.Row.DataItem("G96").ToString)
                ViewState("netgold99N") += clsManage.convert2zero(e.Row.DataItem("G99N").ToString)
                ViewState("netgold99L") += clsManage.convert2zero(e.Row.DataItem("G99L").ToString)

                'e.Row.Cells(gvDeposit.Columns.Count - 4).Text = clsManage.convert2Currency(ViewState("netcash"))
                'e.Row.Cells(gvDeposit.Columns.Count - 3).Text = clsManage.convert2Quantity(ViewState("netgold96"))
                'e.Row.Cells(gvDeposit.Columns.Count - 2).Text = clsManage.convert2Quantity(ViewState("netgold99N"))
                'e.Row.Cells(gvDeposit.Columns.Count - 1).Text = clsManage.convert2Quantity(ViewState("netgold99L"))
                'If e.Row.DataItem("type") = "Withdraw" Then e.Row.Cells(2).ForeColor = Drawing.Color.Red
            End If
            If e.Row.RowType = DataControlRowType.Footer Then
                lblNetCash.Text = clsManage.convert2Currency(ViewState("netcash"))
                lblNet96.Text = clsManage.convert2Quantity(ViewState("netgold96"))
                lblNet99N.Text = clsManage.convert2Quantity(ViewState("netgold99N"))
                lblNet99L.Text = clsManage.convert2Quantity(ViewState("netgold99L"))
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Protected Sub upgSearch_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles upgSearch.Load
        System.Threading.Thread.Sleep(upgSearch.DisplayAfter)
    End Sub

    Protected Sub btnSearch_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSearch.Click
        objSrcDep.SelectParameters("team_id").DefaultValue = ddlTeam.SelectedValue
        gvDeposit.DataBind()
    End Sub
    'Sub bindData()

    '    Dim purity As String = ""
    '    If cblPurity.Items(0).Selected = True Then purity = "96"
    '    If cblPurity.Items(1).Selected = True Then
    '        If purity = "" Then purity = "99N" Else purity += "','" + "99N"
    '    End If

    '    If cblPurity.Items(2).Selected = True Then
    '        If purity = "" Then purity = "99L" Else purity += "','" + "99L"
    '    End If

    '    If cblPurity.Items(0).Selected = False And cblPurity.Items(1).Selected = False And cblPurity.Items(2).Selected = False Then
    '        purity = "96','99N','99L"
    '    End If

    '    objSrcAssetCash.SelectParameters("purity").DefaultValue = ""
    '    gvAssetCash.DataBind()

    '    objSrcAssetGold96.SelectParameters("purity").DefaultValue = purity
    '    gvAssetGold96.DataBind()
    'End Sub


End Class
