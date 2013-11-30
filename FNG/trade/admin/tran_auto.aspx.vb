
Partial Class trade_admin_tran_auto
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.QueryString.HasKeys Then

                hdfPurity.Value = Request.QueryString("purity").ToString
                hdfType.Value = Request.QueryString("type").ToString

                txtQMax.Attributes.Add("style", "text-align:right")
                txtQPer.Attributes.Add("style", "text-align:right")
                gvTran.EmptyDataText = "<div style='color:red;text-align:center;border:solid 1px silver;'>Nothing Transaction.</div>"

                txtQMax.Attributes.Add("onkeypress", "numberOnly();")
                txtQPer.Attributes.Add("onkeypress", "numberOnly();")
                If hdfPurity.Value = "96" Then
                    lblUnit1.Text = "BAHT" : lblUnit2.Text = "BAHT"
                Else
                    lblUnit1.Text = "KG." : lblUnit2.Text = "KG."
                End If
                hdfIsRealtime.Value = "y"
                objSrcTran.SelectParameters("type").DefaultValue = hdfType.Value
                objSrcTran.SelectParameters("purity").DefaultValue = hdfPurity.Value
                gvTran.DataBind()

                getQuantityAccept()
            End If
        End If
    End Sub

    Sub getQuantityAccept()
        Dim dt As New Data.DataTable
        dt = clsMain.getQuantityAccept()
        Dim colMax As String = String.Format("quan_max{0}_{1}", hdfPurity.Value, hdfType.Value)
        Dim colPer As String = String.Format("quan_per{0}_{1}", hdfPurity.Value, hdfType.Value)
        If dt.Rows.Count > 0 Then
            hdfMax.Value = dt.Rows(0)(colMax).ToString
            hdfPer.Value = dt.Rows(0)(colPer).ToString
            lblQMax.Text = dt.Rows(0)(colMax).ToString
            lblQPer.Text = dt.Rows(0)(colPer).ToString
        End If
    End Sub

    Protected Sub gvTran_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTran.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then

            CType(e.Row.FindControl("btnAccept"), Button).Attributes.Add("onclick", "return confirm('Do you want to clear.');")
            e.Row.Cells(0).Text = e.Row.RowIndex + 1

            If e.Row.DataItem("max").ToString = "y" Then
                CType(e.Row.FindControl("btnAccept"), Button).CssClass = "buttonPro small rounded red"
            Else
                CType(e.Row.FindControl("btnAccept"), Button).CssClass = "buttonPro small rounded green"
            End If
        End If
    End Sub

    Protected Sub btnAccept_Click(sender As Object, e As System.EventArgs)
        Try
            Dim i As Integer = CType(CType(sender, Button).Parent.Parent, GridViewRow).RowIndex
            Dim result As Integer = clsMain.clearAutoAccept(hdfType.Value, hdfPurity.Value, gvTran.DataKeys(i).Value, lblQPer.Text, lblQMax.Text)
            If result > 0 Then
                gvTran.DataBind()
            Else
                clsManage.alert(Page, "Please Try Again.", , , "clear")
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "Error")
        End Try
    End Sub


    <System.Web.Services.WebMethod()> _
    Public Shared Function getQuantityAcceptAdap(ByVal type As String, purity As String) As String
        Return clsMain.getQuantityAcceptAdap(type, purity)
    End Function

    Protected Sub UpdateProg1_Load(sender As Object, e As System.EventArgs) Handles UpdateProg1.Load
        System.Threading.Thread.Sleep(UpdateProg1.DisplayAfter)
    End Sub

    Protected Sub upGrid_Load(sender As Object, e As System.EventArgs) Handles upGrid.Load
        refreshGrid()
    End Sub

    Sub refreshGrid()
        Try
            objSrcTran.SelectParameters("type").DefaultValue = hdfType.Value
            objSrcTran.SelectParameters("purity").DefaultValue = hdfPurity.Value
            gvTran.DataBind()
            Dim hasTran As String = IIf(gvTran.Rows.Count > 0, "y", "n")
            clsManage.Script(Page, "top.window.setColorClert('" + hdfType.Value + "','" + hdfPurity.Value + "','" + hasTran + "');", "change_color_button_for_have_transaction")
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub imgEdit_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles imgEdit.Click
        Try
            imgEdit.Visible = False
            lblQMax.Visible = False
            lblQPer.Visible = False
            txtQMax.Visible = True
            txtQPer.Visible = True
            linkSave.Visible = True
            linkCancel.Visible = True
            txtQMax.Text = lblQMax.Text
            txtQPer.Text = lblQPer.Text
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "error")
        End Try
    End Sub

    Protected Sub linkSave_Click(sender As Object, e As System.EventArgs) Handles linkSave.Click
        Try
            If Not IsNumeric(txtQMax.Text) OrElse Not IsNumeric(txtQPer.Text) Then
                clsManage.alert(Page, "Please Enter Only Number.")
                Exit Sub
            ElseIf CInt(txtQMax.Text) < CInt(txtQPer.Text) Then
                clsManage.alert(Page, "Max Accept must be more than Quantity/Transaction.")
                Exit Sub
            End If

            If txtQMax.Text.Trim <> "" And txtQPer.Text.Trim <> "" Then
                Dim result As Integer = clsMain.updateQuantityAccept(txtQMax.Text, txtQPer.Text, hdfType.Value, hdfPurity.Value)
                If result > 0 Then
                    lblQMax.Text = txtQMax.Text
                    lblQPer.Text = txtQPer.Text
                    linkCancel_Click(Nothing, Nothing)
                End If
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "error")
        End Try
    End Sub

    Protected Sub linkCancel_Click(sender As Object, e As System.EventArgs) Handles linkCancel.Click
        Try
            imgEdit.Visible = True
            lblQMax.Visible = True
            lblQPer.Visible = True
            txtQMax.Visible = False
            txtQPer.Visible = False
            linkSave.Visible = False
            linkCancel.Visible = False
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "error")
        End Try
    End Sub
End Class
