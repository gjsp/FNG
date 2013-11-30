
Partial Class stock_cheq
    Inherits basePage
    Dim wait As String = "รอรับเงิน"
    Dim paid As String = "รับเงินแล้ว"

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
            gvCheq.EmptyDataText = clsManage.EmptyDataText
        End If
    End Sub

    Protected Sub gvCheq_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvCheq.RowDataBound
        Try
            If e.Row.RowType = DataControlRowType.DataRow Then
                'CType(e.Row.FindControl("link"), HyperLink).NavigateUrl = "ticket_deal.aspx?page=dialy&id=" + e.Row.DataItem("ref_no").ToString
                'CType(e.Row.FindControl("linkCust"), HyperLink).NavigateUrl = "customer_trans.aspx?cust_id=" + e.Row.DataItem("cust_id").ToString
                'If e.Row.DataItem("paid").ToString = "True" Then
                '    CType(e.Row.FindControl("lblStatus"), Label).Text = paid
                '    CType(e.Row.FindControl("lblStatus"), Label).ForeColor = Drawing.Color.Green
                'Else
                '    CType(e.Row.FindControl("lblStatus"), Label).Text = wait
                'End If

                'If e.Row.DataItem("paid").ToString <> "1" Then CType(e.Row.FindControl("linkUpdate"), LinkButton).Enabled = False

                If ViewState(clsDB.roles.update) IsNot Nothing AndAlso ViewState(clsDB.roles.update) = False Then
                    CType(e.Row.FindControl("linkUpdate"), LinkButton).Enabled = False
                End If
            End If
        Catch ex As Exception

        End Try
    End Sub

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
            gv.Rows(i).FindControl("ddlStatus").Visible = True
            gv.Rows(i).FindControl("lblStatus").Visible = False

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

            Dim status As String = CType(gv.Rows(i).FindControl("ddlStatus"), HtmlSelect).Items(CType(gv.Rows(i).FindControl("ddlStatus"), HtmlSelect).SelectedIndex).Value
            Dim result As Boolean = clsDB.updateCheque(status, gv.DataKeys(i).Value)
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
            gv.Rows(i).FindControl("ddlStatus").Visible = False
            gv.Rows(i).FindControl("lblStatus").Visible = True
  
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub
End Class
