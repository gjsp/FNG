
Partial Class customer_popup
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            gvCust.EmptyDataText = clsManage.EmptyDataText
        End If
    End Sub

    Protected Sub gvCust_SelectedIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSelectEventArgs) Handles gvCust.SelectedIndexChanging
        'ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "ppp", "opener.document.getElementById('txt1').value = '" & gv.Rows(CType(CType(sender, LinkButton).Parent.Parent, GridViewRow).RowIndex).Cells(1).Text & "';window.close();", True)
        Dim script As String = ""
        If Request.QueryString("page").ToString = "report" Then

            Dim script1 As String = "opener.document.getElementById('ctl00_SampleContent_txtCustName').value = """ & gvCust.Rows(e.NewSelectedIndex).Cells(3).Text & """;"

            script = "getStringCustomer(""" & gvCust.Rows(e.NewSelectedIndex).Cells(3).Text.Trim & """);"
        Else
            Dim script1 As String = "opener.document.getElementById('ctl00_SampleContent_txtCustRef').value = '" & gvCust.DataKeys(e.NewSelectedIndex).Value.ToString & "';"
            Dim script2 As String = "opener.document.getElementById('ctl00_SampleContent_txtCustName').value = """ & gvCust.Rows(e.NewSelectedIndex).Cells(3).Text & """;"
            Dim script3 As String = "opener.document.getElementById('ifmGrid').src = 'customer_grid.aspx?id=" & gvCust.DataKeys(e.NewSelectedIndex).Value.ToString & "';"
            Dim script4 As String = "opener.document.getElementById('ctl00_SampleContent_btnSearch').click();"
            'script3 += "opener.document.getElementById('ifmGrid').style.display='block';opener.document.getElementById('ctl00_SampleContent_txtBookNo').focus();"
            'script3 += "opener.document.getElementById('ctl00_SampleContent_lblCustNote').style.display='none';"
            script = script1 + script2 + script4
        End If
        clsManage.Script(Page, Script + "window.close();")
    End Sub

    Protected Sub imgSearch_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgSearch.Click
        If txtSearch.Text.Trim() <> "" Then
            gvCust.DataBind()
        End If
    End Sub

   
End Class
