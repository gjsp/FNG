
Partial Class ticket_payment_detail
    Inherits basePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.QueryString("pid") IsNot Nothing Then
                Dim pid As String = Request.QueryString("pid").ToString
                Dim dc As New dcDBDataContext
                Dim pms = (From pm In dc.payments From ur In dc.users Where pm.created_by = ur.user_id And pm.payment_id = pid).FirstOrDefault
                lblpid.Text = pms.pm.payment_id
                lblBy.Text = pms.ur.user_name.ToString
                lblDate.Text = CDate(pms.pm.created_date).ToString(clsManage.formatDateTime)

                'objSrc.SelectParameters("payment_id").DefaultValue = pid

                Dim tks = From tk In dc.tickets Where tk.payment_id = pid
                gv.DataSource = tks
                gv.DataBind()
            End If

        End If
    End Sub


    Protected Sub imgDel_Click(sender As Object, e As ImageClickEventArgs)
        Try
            Dim i As Integer = CType(CType(sender, ImageButton).Parent.Parent, GridViewRow).RowIndex
            Dim dc As New dcDBDataContext
            Dim ticket = (From t In dc.tickets Where t.ticket_id = gv.DataKeys(i).Value.ToString).FirstOrDefault
            ticket.payment_id = Nothing

            dc.SubmitChanges()
            gv.DataBind()
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Protected Sub gv_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gv.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("imgDel"), ImageButton).OnClientClick = "return confirm(' " & clsManage.msgDel & " ');"
            CType(e.Row.FindControl("linkRef"), HyperLink).NavigateUrl = "ticket_deal.aspx?page=stock&id=" + gv.DataKeys(e.Row.RowIndex)("ref_no").ToString
        End If
    End Sub

    Protected Sub linkAdd_Click(sender As Object, e As EventArgs) Handles linkAdd.Click
        Try
            Dim dc As New dcDBDataContext
            Dim tks = From tk In dc.tickets Where tk.status_id = 101 And tk.payment_id Is Nothing

            gvPopup.DataSource = tks.ToList
            gvPopup.DataBind()
            modalPopUpExtender1.Show()
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "err")
        End Try

    End Sub

    'Protected Sub btnPopup_Click(sender As Object, e As EventArgs) Handles btnPopup.Click
    '    objSrc.SelectParameters("payment_id").DefaultValue = Request.QueryString("pid").ToString
    '    gv.DataBind()
    'End Sub

    Protected Sub gvPopup_RowCommand(sender As Object, e As GridViewCommandEventArgs) Handles gvPopup.RowCommand
        If e.CommandName = "Select" Then
            Dim refnoList As String = ""
            Dim comma As String = ","
            For Each gvr As GridViewRow In gv.Rows
                If refnoList = "" Then
                    refnoList = gv.DataKeys(gvr.RowIndex)("ref_no").ToString
                Else
                    refnoList = comma + gv.DataKeys(gvr.RowIndex)("ref_no").ToString
                End If
            Next

            Dim dc As New dcDBDataContext
            Dim tks = From tk In dc.tickets Where refnoList.Contains(tk.ref_no)

            gv.DataSource = tks.ToList
            gv.DataBind()

        End If
    End Sub

    Protected Sub link_Click(sender As Object, e As EventArgs)
        Try
            Dim refnoList As String = ""
            Dim comma As String = ","
            For Each gvr As GridViewRow In gv.Rows
                If refnoList = "" Then
                    refnoList = gv.DataKeys(gvr.RowIndex)("ref_no").ToString
                Else
                    refnoList += comma + gv.DataKeys(gvr.RowIndex)("ref_no").ToString
                End If
            Next

            Dim i As Integer = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow).RowIndex
            refnoList += comma + gvPopup.DataKeys(i)("ref_no").ToString()

            Dim dc As New dcDBDataContext
            Dim tks = From tk In dc.tickets Where refnoList.Split(comma).Contains(tk.ref_no)

            gv.DataSource = tks.ToList
            gv.DataBind()


        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub linkReport_Click(sender As Object, e As EventArgs) Handles linkReport.Click
        Dim url As String = ""

        'url = "report/rpt_bill.aspx?type=e&ref=" + strRefno + ""

        'url = "report/rpt_nobill.aspx?type=e&ref=" + strRefno + ""

        clsManage.Script(Page, "window.open('" + url + "','_blank');")
    End Sub
End Class
