
Partial Class ticket_payment_detail
    Inherits basePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            gv.EmptyDataText = clsManage.EmptyDataText
            gvPopup.EmptyDataText = clsManage.EmptyDataText
            gvTran.EmptyDataText = clsManage.EmptyDataText
            gvPopTrans.EmptyDataText = clsManage.EmptyDataText
            gvSplit.EmptyDataText = clsManage.EmptyDataText
            gvPopSplit.EmptyDataText = clsManage.EmptyDataText
            btnSave.Attributes.Add("onclick", "return confirm('Do you want to save?');")
            pnMain.Visible = True : pnNon.Visible = False
            ViewState("dt") = Nothing : ViewState("dtSplit") = Nothing : ViewState("dtTrans") = Nothing

            If Request.QueryString("pid") IsNot Nothing Then
                hdfPid.Value = Request.QueryString("pid").ToString
                hdfCustId.Value = Request.QueryString("cid").ToString

                Dim dc As New dcDBDataContext
                Dim pms = (From pm In dc.payments Where pm.payment_id = hdfPid.Value).FirstOrDefault
                If pms IsNot Nothing Then


                    Dim dt As New Data.DataTable
                    dt = clsFng.getPaymentTicket(hdfPid.Value)
                    ViewState("dt") = dt
                    gv.DataSource = dt
                    gv.DataBind()

                    dt = clsFng.getPaymentSplit(hdfPid.Value)
                    ViewState("dtSplit") = dt
                    gvSplit.DataSource = dt
                    gvSplit.DataBind()

                    dt = clsFng.getPaymentTrans(hdfPid.Value)
                    ViewState("dtTrans") = dt
                    gvTran.DataSource = dt
                    gvTran.DataBind()

                    dt.Dispose()
                Else
                    pnMain.Visible = False : pnNon.Visible = True
                End If
            End If
        End If
    End Sub

    Protected Sub imgDel_Click(sender As Object, e As ImageClickEventArgs)
        Try
            Dim i As Integer = CType(CType(sender, ImageButton).Parent.Parent, GridViewRow).RowIndex
            CType(ViewState("dt"), Data.DataTable).Rows.RemoveAt(i)
            CType(ViewState("dt"), Data.DataTable).AcceptChanges()

            gv.DataSource = CType(ViewState("dt"), Data.DataTable)
            gv.DataBind()

        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Protected Sub imgDelSplit_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs)
        Try
            Dim i As Integer = CType(CType(sender, ImageButton).Parent.Parent, GridViewRow).RowIndex
            CType(ViewState("dtSplit"), Data.DataTable).Rows.RemoveAt(i)
            CType(ViewState("dtSplit"), Data.DataTable).AcceptChanges()

            gvSplit.DataSource = CType(ViewState("dtSplit"), Data.DataTable)
            gvSplit.DataBind()

        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Protected Sub imgDelTrans_Click(sender As Object, e As ImageClickEventArgs)
        Try
            Dim i As Integer = CType(CType(sender, ImageButton).Parent.Parent, GridViewRow).RowIndex
            CType(ViewState("dtTrans"), Data.DataTable).Rows.RemoveAt(i)
            CType(ViewState("dtTrans"), Data.DataTable).AcceptChanges()


            gvTran.DataSource = CType(ViewState("dtTrans"), Data.DataTable)
            gvTran.DataBind()

        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Protected Sub btnSave_Click(sender As Object, e As System.EventArgs) Handles btnSave.Click
        If ViewState("dt") Is Nothing And ViewState("dtSplit") Is Nothing And ViewState("dtTrans") Is Nothing Then
            clsManage.alert(Page, "โปรดเลือกข้อมูลในการบันทึก") : Exit Sub
        End If
        Dim dc As New dcDBDataContext
        Try
            dc.Connection.Open()
            dc.Transaction = dc.Connection.BeginTransaction
            Dim pid As String = Request.QueryString("pid").ToString

            'delete payment_id in ticket tbl all
            Dim ts = From t In dc.tickets Where t.payment_id = pid
            For Each t In ts
                t.payment_id = Nothing
                t.payment_by = Nothing
            Next

            'delete payment_id in ticket_split tbl all
            Dim sp = From s In dc.ticket_splits Where s.payment_id = pid
            For Each s In sp
                s.payment_id = Nothing
                s.payment_by = Nothing
            Next

            'delete payment_id in customer_trans tbl all
            Dim ct = From c In dc.customer_trans Where c.payment_id = pid
            For Each c In ct
                c.payment_id = Nothing
                c.payment_by = Nothing
            Next

            'Add Ticket
            Dim refNoList As String = ""
            Dim cma As String = ","
            For Each dr As Data.DataRow In CType(ViewState("dt"), Data.DataTable).Rows
                If refNoList.Length = 0 Then
                    refNoList = dr("ref_no")
                Else
                    refNoList += cma + dr("ref_no")
                End If
            Next

            Dim tks = From tk In dc.tickets Where refNoList.Split(",").Contains(tk.ref_no)
            For Each tk In tks
                tk.payment_id = pid
                tk.modifier_date = DateTime.Now
                tk.payment_by = Session(clsManage.iSession.user_id_center.ToString).ToString
            Next

            'Add Split
            refNoList = ""
            For Each dr As Data.DataRow In CType(ViewState("dtSplit"), Data.DataTable).Rows
                If refNoList.Length = 0 Then
                    refNoList = dr("ticket_sp_id").ToString
                Else
                    refNoList += cma + dr("ticket_sp_id").ToString
                End If
            Next

            Dim sps = From s In dc.ticket_splits Where refNoList.Split(",").Contains(s.ticket_sp_id)
            For Each s In sps
                s.payment_id = pid
                's.modifier_date = DateTime.Now
                s.payment_by = Session(clsManage.iSession.user_id_center.ToString).ToString
            Next

            'Add trans
            refNoList = ""
            For Each dr As Data.DataRow In CType(ViewState("dtTrans"), Data.DataTable).Rows
                If refNoList.Length = 0 Then
                    refNoList = dr("cust_tran_id").ToString
                Else
                    refNoList += cma + dr("cust_tran_id").ToString
                End If
            Next

            Dim cts = From c In dc.customer_trans Where refNoList.Split(",").Contains(c.cust_tran_id)
            For Each c In cts
                c.payment_id = pid
                c.payment_by = Session(clsManage.iSession.user_id_center.ToString).ToString
            Next

            dc.SubmitChanges()
            dc.Transaction.Commit()

            ' clsManage.Script(Page, String.Format("window.location='ticket_payment_detail.aspx?pid={0}'", Request.QueryString("pid").ToString))
        Catch ex As Exception
            dc.Transaction.Rollback()
            clsManage.alert(Page, ex.Message, , , "err")
        Finally
            dc.Connection.Close()
        End Try
    End Sub

    Private Sub enableGridDelete(enable As Boolean)
        For Each gvr As GridViewRow In gv.Rows
            CType(gvr.FindControl("imgDel"), ImageButton).Enabled = enable
            If enable Then
                CType(gvr.FindControl("imgDel"), ImageButton).ImageUrl = "~/images/i_del.png"
            Else
                CType(gvr.FindControl("imgDel"), ImageButton).ImageUrl = "~/images/i_del2.png"
            End If
        Next
    End Sub

    Protected Sub linkOpenTicket_Click(sender As Object, e As EventArgs) Handles linkAdd.Click
        Try
            Dim refNoList As String = ""
            Dim cma As String = "','"
            If ViewState("dt") IsNot Nothing Then
                For Each dr As Data.DataRow In CType(ViewState("dt"), Data.DataTable).Rows
                    If refNoList.Length = 0 Then
                        refNoList = dr("ref_no")
                    Else
                        refNoList += cma + dr("ref_no")
                    End If
                Next
            End If

            Dim dt As New Data.DataTable
            dt = clsFng.getPaymentTicketPopUp(refNoList, hdfCustId.Value)
            gvPopup.DataSource = dt
            gvPopup.DataBind()

            modalPopUpExtender1.Show()
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "err")
        End Try

    End Sub

    Protected Sub linkOpenSplit_Click(sender As Object, e As System.EventArgs) Handles linkOpenSplit.Click
        Try
            Dim refNoList As String = ""
            Dim cma As String = "','"
            For Each dr As Data.DataRow In CType(ViewState("dtSplit"), Data.DataTable).Rows
                If refNoList.Length = 0 Then
                    refNoList = dr("ticket_sp_id")
                Else
                    refNoList += cma + dr("ticket_sp_id")
                End If
            Next

            Dim dt As New Data.DataTable
            dt = clsFng.getPaymentSplitPopUp(refNoList, hdfCustId.Value)
            gvPopSplit.DataSource = dt
            gvPopSplit.DataBind()

            modalPopUpExtender3.Show()
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "err")
        End Try
    End Sub

    Protected Sub linkOpenTrans_Click(sender As Object, e As EventArgs) Handles linkOpenTrans.Click
        Try
            Dim refNoList As String = ""
            Dim cma As String = "','"
            For Each dr As Data.DataRow In CType(ViewState("dtTrans"), Data.DataTable).Rows
                If refNoList.Length = 0 Then
                    refNoList = dr("cust_tran_id").ToString
                Else
                    refNoList += cma + dr("cust_tran_id").ToString
                End If
            Next

            Dim dt As New Data.DataTable
            dt = clsFng.getPaymentTransPopUp(refNoList, hdfCustId.Value)
            gvPopTrans.DataSource = dt
            gvPopTrans.DataBind()

            modalPopUpExtender2.Show()
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "err")
        End Try
    End Sub

    Protected Sub gv_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gv.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("imgDel"), ImageButton).OnClientClick = "return confirm(' " & clsManage.msgDel & " ');"
            CType(e.Row.FindControl("linkRef"), HyperLink).NavigateUrl = "ticket_deal.aspx?page=stock&id=" + gv.DataKeys(e.Row.RowIndex)("ref_no").ToString
        End If
    End Sub

    Protected Sub gvSplit_RowDataBound(sender As Object, e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvSplit.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("imgDelSplit"), ImageButton).OnClientClick = "return confirm(' " & clsManage.msgDel & " ');"
            CType(e.Row.FindControl("linkRef"), HyperLink).NavigateUrl = "ticket_split_bill.aspx?id=" + gvSplit.DataKeys(e.Row.RowIndex)("ref_no").ToString
        End If
    End Sub

    Protected Sub gvTran_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvTran.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            CType(e.Row.FindControl("imgDelTrans"), ImageButton).OnClientClick = "return confirm(' " & clsManage.msgDel & " ');"
            If e.Row.Cells(1).Text = "&nbsp;" Then
                e.Row.Cells(1).Text = "Cash"
            End If
        End If
    End Sub

    Protected Sub gvPopTrans_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvPopTrans.RowDataBound
        If e.Row.RowType = DataControlRowType.DataRow Then
            If e.Row.Cells(1).Text = "&nbsp;" Then
                e.Row.Cells(1).Text = "Cash"
            ElseIf e.Row.Cells(1).Text = "99" Then
                e.Row.Cells(1).Text = "Gold 99.99%"
            ElseIf e.Row.Cells(1).Text = "96" Then
                e.Row.Cells(1).Text = "Gold 96.50%"
            End If
        End If
    End Sub

    Protected Sub linkAddTicket_Click(sender As Object, e As EventArgs)
        Try

            Dim i As Integer = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow).RowIndex
            Dim dt As New Data.DataTable
            dt = CType(ViewState("dt"), Data.DataTable)
            Dim dr As Data.DataRow = dt.NewRow
            dr("ref_no") = gvPopup.Rows(i).Cells(0).Text
            dr("type") = gvPopup.Rows(i).Cells(1).Text
            dr("price") = gvPopup.Rows(i).Cells(2).Text
            dr("quantity") = gvPopup.Rows(i).Cells(3).Text
            dr("amount") = gvPopup.Rows(i).Cells(4).Text
            dr("payment_by_name") = IIf(gvPopup.Rows(i).Cells(5).Text = "&nbsp;", "", gvPopup.Rows(i).Cells(5).Text)
            dt.Rows.Add(dr)
            ViewState("dt") = dt
            gv.DataSource = dt
            gv.DataBind()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub linkAddSplit_Click(sender As Object, e As System.EventArgs)
        Try
           
            Dim i As Integer = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow).RowIndex
            Dim dt As New Data.DataTable
            dt = CType(ViewState("dtSplit"), Data.DataTable)
            Dim dr As Data.DataRow = dt.NewRow
            dr("ticket_sp_id") = gvPopSplit.DataKeys(i).Value
            dr("ref_no") = gvPopSplit.Rows(i).Cells(0).Text
            dr("type") = gvPopSplit.Rows(i).Cells(1).Text
            dr("price") = gvPopSplit.Rows(i).Cells(2).Text
            dr("quantity") = gvPopSplit.Rows(i).Cells(3).Text
            dr("amount") = gvPopSplit.Rows(i).Cells(4).Text
            dr("payment_by_name") = IIf(gvPopSplit.Rows(i).Cells(5).Text = "&nbsp;", "", gvPopSplit.Rows(i).Cells(5).Text)
            dt.Rows.Add(dr)
            ViewState("dtSplit") = dt
            gvSplit.DataSource = dt
            gvSplit.DataBind()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub linkAddTrans_Click(sender As Object, e As EventArgs)
        Try
            Dim i As Integer = CType(CType(sender, LinkButton).Parent.Parent, GridViewRow).RowIndex
            Dim dt As New Data.DataTable
            dt = CType(ViewState("dtTrans"), Data.DataTable)
            Dim dr As Data.DataRow = dt.NewRow
            dr("cust_tran_id") = gvPopTrans.DataKeys(i).Value
            dr("created_date") = gvPopTrans.Rows(i).Cells(0).Text
            dr("gold_type_id") = gvPopTrans.Rows(i).Cells(1).Text
            dr("type") = gvPopTrans.Rows(i).Cells(2).Text
            dr("quantity") = gvPopTrans.Rows(i).Cells(3).Text
            dr("amount") = gvPopTrans.Rows(i).Cells(4).Text
            dr("payment_by_name") = IIf(gvPopTrans.Rows(i).Cells(5).Text = "&nbsp;", "", gvPopTrans.Rows(i).Cells(5).Text)
            dt.Rows.Add(dr)
            ViewState("dtTrans") = dt
            gvTran.DataSource = dt
            gvTran.DataBind()

        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Protected Sub btnReceipt_Click(sender As Object, e As EventArgs) Handles btnReceipt.Click
        If gv.Rows.Count > 0 Or gvSplit.Rows.Count > 0 Or gvTran.Rows.Count > 0 Then
            clsManage.Script(Page, "window.open('report/rpt_payment.aspx?pid=" + hdfPid.Value + "','_blank');")
        Else
            clsManage.alert(Page, "โปรดเลือกข้อมูลในการออกบิล")
        End If
    End Sub
End Class
