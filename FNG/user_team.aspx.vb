
Partial Class user_team
    Inherits basePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            gvTeam.EmptyDataText = clsManage.EmptyDataText
            gvTeam.DataBind()
        End If
       
    End Sub

    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If txtTeamName.Text.Trim <> "" Then

            Dim adt As New dsTableAdapters.teamTableAdapter
            Dim dt As New ds.teamDataTable
            adt.FillByName(dt, txtTeamName.Text)
            If dt.Rows.Count > 0 Then
                clsManage.alert(Page, "ชื่อทีมซ้ำ โปรดระบุใหม่.")
                Exit Sub
            End If
            Dim result As Integer = adt.Insert(txtTeamName.Text, txtDetail.Text)
            If result > 0 Then
                gvTeam.DataBind()
                clsManage.alert(Page, "Update Complete.")
            End If
        End If
    End Sub


    Protected Sub gvTeam_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvTeam.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            e.Row.Cells(2).Attributes.Add("onclick", "return confirm('" + clsManage.msgDel + "');")
        End If
    End Sub

    Protected Sub imgDel_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim i As Integer = CType(CType(sender, ImageButton).Parent.Parent, GridViewRow).RowIndex
        Dim adt As New dsTableAdapters.teamTableAdapter
        Dim result As Integer = adt.Delete(gvTeam.DataKeys(i).Value)
        If result > 0 Then
            gvTeam.DataBind()
        End If
    End Sub
End Class
