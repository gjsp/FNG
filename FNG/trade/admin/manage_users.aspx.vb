
Partial Class admin_manage_users
    Inherits basePageTrade

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            gvUser.EmptyDataText = clsManage.EmptyDataText
            txtSearch.Attributes.Add("onkeypress", "if(event.keyCode==13){$get('" & imgSearch.ClientID & "').focus();}")
            clsManage.Script(Page, "$get('" + txtSearch.ClientID + "').focus();")
        End If
    End Sub

    Protected Sub gvUser_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvUser.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("DelButton"), ImageButton).Attributes.Add("onclick", "return confirm('Do you want to delete.');")
            Dim cb As CheckBox = CType(e.Row.FindControl("cbHalt"), CheckBox)
            cb.Enabled = False
            If e.Row.DataItem("halt") = "n" Then
                cb.Checked = False
            Else
                cb.Checked = True
            End If
        End If
    End Sub

    Protected Sub UpdateProg1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles UpdateProg1.Load
        System.Threading.Thread.Sleep(UpdateProg1.DisplayAfter + 300)
    End Sub

    Protected Sub EditButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim i As Integer = CType(CType(sender, ImageButton).Parent.Parent, GridViewRow).RowIndex
        Dim id As String = gvUser.DataKeys(i).Value
        Response.Redirect("manage_user_detail.aspx?id=" + id + "")
    End Sub

    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        Response.Redirect("add_user.aspx")
    End Sub

    Protected Sub DelButton_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim i As Integer = CType(CType(sender, ImageButton).Parent.Parent, GridViewRow).RowIndex
        Dim id As String = gvUser.DataKeys(i).Value
        clsMain.DelUsername(id)
        gvUser.DataBind()
    End Sub

    Protected Sub imgSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSearch.Click
        clsManage.Script(Page, "$get('" + txtSearch.ClientID + "').select();")
    End Sub

    'Protected Sub linkExport_Click(sender As Object, e As System.EventArgs) Handles linkExport.Click
    '    Dim filename = "xx.xls"
    '    Response.ClearContent()
    '    Response.AddHeader("content-disposition", "attachment;filename=" + filename)

    '    Response.Cache.SetCacheability(HttpCacheability.NoCache)

    '    Response.ContentType = "application/vnd.xls"

    '    Dim sw As New IO.StringWriter
    '    Dim htw As New HtmlTextWriter(sw)
    '    Dim frm As New HtmlForm()
    '    frm.Attributes("runat") = "server"
    '    gvExcel = gvUser
    '    gvExcel.DataBind()


    '    frm.Controls.Add(gvExcel)
    '    gvExcel.RenderControl(htw)
    '    Response.Write(sw.ToString)
    '    Response.End()

    'End Sub
End Class
