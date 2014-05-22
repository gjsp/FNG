
Partial Class ticket_payment
    Inherits basePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            gv.EmptyDataText = clsManage.EmptyDataText
            gv.DataBind()
        End If
    End Sub


    Protected Sub gv_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gv.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("linkDetail"), HyperLink).Text = e.Row.DataItem("payment_id").ToString
            CType(e.Row.FindControl("linkDetail"), HyperLink).NavigateUrl = "ticket_payment_detail.aspx?pid=" + e.Row.DataItem("payment_id").ToString + "&cid=" + e.Row.DataItem("cust_id").ToString
            CType(e.Row.FindControl("imgDel"), ImageButton).OnClientClick = "return confirm(' Do you want to Delete?');"
        End If
    End Sub

    Protected Sub imgDel_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs)
        Try
            Dim i As Integer = CType(CType(sender, ImageButton).Parent.Parent, GridViewRow).RowIndex
            Dim pid As Integer = gv.DataKeys(i).Value
            Dim dc As New dcDBDataContext
             'check ว่าเป็น payment ว่างหรือป่าว
            'Dim tp = From t In dc.tickets From s In dc.ticket_splits From c In dc.customer_trans Where t.payment = pid Or s.payment_id = pid Or c.payment_id = pid
            Dim tk = (From t In dc.tickets Where t.payment_id = pid).FirstOrDefault
            Dim ts = (From s In dc.ticket_splits Where s.payment_id = pid).FirstOrDefault
            Dim ct = (From c In dc.customer_trans Where c.payment_id = pid).FirstOrDefault

            If tk Is Nothing And ts Is Nothing And ct Is Nothing Then
                Dim pm = (From p In dc.payments Where p.payment_id = pid).FirstOrDefault
                pm.active = False
                pm.modifier_date = DateTime.Now
                pm.modifier_by = Session(clsManage.iSession.user_id_center.ToString)
                dc.SubmitChanges()
            Else
                clsManage.alert(Page, "ไม่สามารถลบได้ เนื่องจากมี payment detail ค้างอยู่")
            End If

        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub
End Class
