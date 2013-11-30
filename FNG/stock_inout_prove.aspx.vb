
Partial Class stock_inout_prove
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
                        'If dr(clsDB.roles.delete) = False Then
                        '    ViewState(clsDB.roles.delete) = False
                        'End If

                    End If
                End If
            Catch ex As Exception
                clsManage.alert(Page, ex.Message)
            End Try
        End If
    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            gvAssetCash.EmptyDataText = clsManage.EmptyDataText
            gvAssetGold96.EmptyDataText = clsManage.EmptyDataText
            gvAssetGold99.EmptyDataText = clsManage.EmptyDataText
            tapImport.ActiveTabIndex = 0
        End If
       
    End Sub

    Protected Sub gvAssetCash_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAssetCash.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("txtQuantity"), TextBox).Text = clsManage.convert2Currency(e.Row.DataItem("quantity").ToString)
            CType(e.Row.FindControl("lblQuantity"), Label).Text = clsManage.convert2Currency(e.Row.DataItem("quantity").ToString)
            CType(e.Row.FindControl("txtQuantity"), TextBox).Attributes.Add("style", "text-align:right")
            'CType(e.Row.FindControl("imgDel"), ImageButton).OnClientClick = "return confirm(' " & clsManage.msgDel & " ');"

            'CType(e.Row.FindControl("imgDel"), ImageButton).Visible = False

            'If ViewState(clsDB.roles.delete) IsNot Nothing AndAlso ViewState(clsDB.roles.delete) = False Then
            '    CType(e.Row.FindControl("imgDel"), ImageButton).Enabled = False
            '    CType(e.Row.FindControl("imgDel"), ImageButton).ImageUrl = "~/images/i_del2.png"
            'End If
            If ViewState(clsDB.roles.update) IsNot Nothing AndAlso ViewState(clsDB.roles.update) = False Then
                CType(e.Row.FindControl("linkUpdate"), LinkButton).Enabled = False
            End If
        End If
    End Sub

    Protected Sub gvAssetGold96_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAssetGold96.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lblQuantity"), Label).Text = clsManage.convert2Quantity(e.Row.DataItem("quantity").ToString)
            CType(e.Row.FindControl("txtQuantity"), TextBox).Attributes.Add("style", "text-align:right")
           
            If ViewState(clsDB.roles.update) IsNot Nothing AndAlso ViewState(clsDB.roles.update) = False Then
                CType(e.Row.FindControl("linkUpdate"), LinkButton).Enabled = False
            End If

        End If
    End Sub

    Protected Sub gvAssetGold99N_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAssetGold99.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("lblQuantity"), Label).Text = clsManage.convert2Quantity(e.Row.DataItem("quantity").ToString)
            CType(e.Row.FindControl("txtQuantity"), TextBox).Attributes.Add("style", "text-align:right")
          
            If ViewState(clsDB.roles.update) IsNot Nothing AndAlso ViewState(clsDB.roles.update) = False Then
                CType(e.Row.FindControl("linkUpdate"), LinkButton).Enabled = False
            End If
        End If
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

            'For Settlement Only
            Dim status As String = "actual"
            'If Request.QueryString("edit") IsNot Nothing Then
            '    status = "actual" 'For Settlement
            'Else
            '    status = CType(gv.Rows(i).FindControl("lblStatus"), Label).Text
            'End If

            Dim before_quantity As Double = 0
            'case actual ไปแล้ว ต้อง ลบค่าเก่าในสต๊อกด้วย
            If status = "actual" And CType(gv.Rows(i).FindControl("lblStatus"), Label).Text = "actual" Then
                before_quantity = CType(gv.Rows(i).FindControl("lblQuantity"), Label).Text
            Else
                'planning
                If CType(gv.Rows(i).FindControl("txtQuantity"), TextBox).Text = "0" Then
                    Exit Sub
                End If
            End If

            Dim asset_type As String = gv.Rows(i).Cells(1).Text
            Dim type As String = gv.Rows(i).Cells(2).Text
            Dim purity As String = gv.Rows(i).Cells(3).Text
            Dim payment As String = ""
            If asset_type = "Cash" Then payment = gv.Rows(i).Cells(4).Text
            Dim result As Boolean = clsDB.updateAssetStatus(status, CType(gv.Rows(i).FindControl("txtQuantity"), TextBox).Text, Session(clsManage.iSession.user_id_center.ToString).ToString, gv.DataKeys(i).Value, CType(gv.Rows(i).FindControl("txtNote"), TextBox).Text, _
                                                           asset_type, type, purity, payment, before_quantity)

            If result Then
                Dim update_cash As String = "n"
                Dim update_gold As String = "n"
                Dim isCash As String = "n"
                Dim remark As String = CType(gv.Rows(i).FindControl("txtNote"), TextBox).Text
                Dim quantity As Double = CType(gv.Rows(i).FindControl("txtQuantity"), TextBox).Text
                Dim quan_actual As Double = quantity
                Dim amount_actual As Double = quantity

                If before_quantity <> 0 Then
                    If type = "In" Then
                        quan_actual = quantity - before_quantity
                    ElseIf type = "Out" Then
                        quan_actual = before_quantity - quantity
                    End If
                End If

                If status = "actual" And CType(gv.Rows(i).FindControl("lblStatus"), Label).Text = "actual" Then
                    If type = "In" And quan_actual < 0 Then
                        type = "Out"
                        quan_actual = quan_actual * -1
                    ElseIf type = "Out" And quan_actual < 0 Then
                        quan_actual = quan_actual * -1
                    ElseIf type = "Out" And quan_actual > 0 Then
                        type = "In"
                    End If
                End If

                If asset_type = "Cash" Then
                    isCash = "y"
                    amount_actual = quan_actual
                    quan_actual = 0
                Else
                    update_gold = "y"
                    amount_actual = 0
                End If

                clsDB.insert_actual2(gv.DataKeys(i).Value, "", Session(clsManage.iSession.user_id_center.ToString).ToString, purity, quan_actual, amount_actual, "998", remark, type, "In/Out", "", "", "", update_cash, update_gold, isCash, payment)

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

End Class
