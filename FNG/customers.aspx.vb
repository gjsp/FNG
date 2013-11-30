
Partial Class customers
    Inherits basePage

    Protected Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        If Not Page.IsPostBack Then
            Try
                If Session(clsManage.iSession.user_role_center.ToString) IsNot Nothing Then
                    Dim dtRole As New Data.DataTable : dtRole = Session(clsManage.iSession.user_role_center.ToString)
                    If dtRole.Rows.Find(Me.Page.Page.ToString) IsNot Nothing Then
                        Dim dr As Data.DataRow = dtRole.Rows.Find(Me.Page.Page.ToString)
                        If dr(clsDB.roles.delete) = False Then
                            ViewState(clsDB.roles.delete) = False
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
            gvCust.EmptyDataText = clsManage.EmptyDataText
            txtDate_CalendarExtender.Format = clsManage.formatDateTime
            txtDate2_CalendarExtender.Format = clsManage.formatDateTime
            clsManage.getDropDownlistValue(ddlTeam, clsDB.getTeam, "-- All --")
            ddlTeam.Items.Insert(IIf(ddlTeam.Items.Count > 0, 1, 0), New ListItem(clsManage.msgRequireSelect, "none"))
            txtDate.Attributes.Add("onkeypress", "return false;")
            txtDate2.Attributes.Add("onkeypress", "return false;")
            gvCust.DataBind()
            txtSearch.Focus()

        End If
    End Sub
    Protected Sub upgSearch_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles upgSearch.Load
        System.Threading.Thread.Sleep(upgSearch.DisplayAfter)
    End Sub
    Protected Sub imgDel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Try
            Dim i As Integer = CType(CType(sender, ImageButton).Parent.Parent, GridViewRow).RowIndex
            'check ว่ามีเงิน หรือ ทอง ฝากอยู่หรือป่าว
            Dim dt As New Data.DataTable
            If Not clsDB.checkCustomerDepositBeforeDel(gvCust.DataKeys(i).Value) Then
                clsManage.alert(Page, "ไม่สามารถลบได้ เนื่องจากมีการฝากเงินหรือทองไว้.")
                Exit Sub
            End If

            'check ว่าเคย trading หรือยัง
            If Not clsDB.checkCustomerBeforeDel(gvCust.DataKeys(i).Value) Then
                clsManage.alert(Page, "ไม่สามารถลบได้ เนื่องจากเคยมีการซื้อขายไปแล้ว.")
                Exit Sub
            End If

            'Dim da As New dsTableAdapters.customerTableAdapter()
            'Dim result As Integer = da.DeleteQuery(gvCust.DataKeys(i).Value)
            Dim result As Boolean = clsDB.delCustomerAndUsernames(gvCust.DataKeys(i).Value)
            If result Then
                gvCust.DataBind()
            End If

        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Protected Sub gvUser_SelectedIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSelectEventArgs) Handles gvCust.SelectedIndexChanging
        Dim id As String = gvCust.DataKeys(e.NewSelectedIndex).Value
        'Response.Redirect("customer_detail.aspx?id=" & id)
        clsManage.Script(Page, "window.open('customer_detail.aspx?id=" & id + "')")
    End Sub

    Protected Sub imgSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSearch.Click
        gvCust.DataSourceID = "objSrcCust"
        objSrcCust.SelectParameters("str").DefaultValue = txtSearch.Text
        If cbSelf.Checked Then
            objSrcCust.SelectParameters("user_id").DefaultValue = Session(clsManage.iSession.user_id_center.ToString).ToString
        Else
            objSrcCust.SelectParameters("user_id").DefaultValue = ""
        End If
        objSrcCust.SelectParameters("team_id").DefaultValue = ddlTeam.SelectedValue
        objSrcCust.SelectParameters("isCall").DefaultValue = rdoTradeOnline.SelectedValue
        objSrcCust.SelectParameters("created_date").DefaultValue = txtDate.Text
        objSrcCust.SelectParameters("created_date2").DefaultValue = txtDate2.Text
        gvCust.DataBind()
        clsManage.focusText(Page, txtSearch.ClientID)

    End Sub

    Protected Sub gvCust_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCust.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                CType(e.Row.Cells(gvCust.Columns.Count - 1).Controls(1), ImageButton).OnClientClick = "return confirm(' " & clsManage.msgDel & " ');"
                CType(e.Row.FindControl("linkCust"), HyperLink).NavigateUrl = "customer_detail.aspx?id=" + e.Row.DataItem("cust_id").ToString
                e.Row.Attributes.Add("onmouseover", "this.originalstyle=this.style.backgroundColor;this.style.backgroundColor='#66CCFF'")
                e.Row.Attributes.Add("onmouseout", "this.style.backgroundColor=this.originalstyle;")

                If ViewState(clsDB.roles.delete) IsNot Nothing AndAlso ViewState(clsDB.roles.delete) = False Then
                    CType(e.Row.FindControl("imgDel"), ImageButton).Enabled = False
                    CType(e.Row.FindControl("imgDel"), ImageButton).ImageUrl = "~/images/i_del2.png"
                End If

            End If
        Catch ex As Exception

        End Try
    End Sub

    Protected Sub linkDeposit_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim index As Integer = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow).RowIndex
        Dim cust_id As String = gvCust.DataKeys(index).Value
        'Response.Redirect("customer_trans.aspx?cust_id=" & cust_id + "")
        clsManage.Script(Page, "window.open('customer_trans.aspx?cust_id=" & cust_id + "')")
    End Sub

    Protected Sub linkFolio_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim index As Integer = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow).RowIndex
        Dim cust_id As String = gvCust.DataKeys(index).Value
        'Response.Redirect("customer_portfolio.aspx?id=" & cust_id + "")
        clsManage.Script(Page, "window.open('customer_portfolio.aspx?id=" & cust_id + "')")
    End Sub

    Protected Sub linkDealTicket_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        Dim index As Integer = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow).RowIndex
        Dim cust_id As String = gvCust.DataKeys(index).Value
        clsManage.Script(Page, "window.open('ticket_deal.aspx?cust_id=" & cust_id + "')")
    End Sub

End Class
