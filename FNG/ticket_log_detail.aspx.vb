
Partial Class ticket_log_detail
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.QueryString("id") IsNot Nothing Then
                objSrcLog.SelectParameters("ref_no").DefaultValue = Request.QueryString("id").ToString
            End If
            gvLog.EmptyDataText = clsManage.EmptyDataText
        End If
    End Sub

    Protected Sub gvLog_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvLog.RowDataBound
        If e.Row.RowType = DataControlRowType.Header Then
            Dim dt As New Data.DataTable
            dt = clsDB.getTicketLogUpdate(Request.QueryString("id").ToString)
            If dt.Rows.Count > 0 Then
                ViewState("log") = dt
                lblRef_no.Text = Request.QueryString("id").ToString
                lblCreatedBy.Text = dt.Rows(0)("created_by").ToString
                lblCreatedDate.Text = CDate(dt.Rows(0)("created_date")).ToString("dd/MM/yyyy hh:mm:ss")
            End If
        End If
        If e.Row.RowType = DataControlRowType.DataRow Then
            Try
                'CType(e.Row.FindControl("linkCust"), HyperLink).NavigateUrl = "customer_detail.aspx?id=" + e.Row.DataItem("cust_id").ToString
                'CType(e.Row.FindControl("link"), HyperLink).NavigateUrl = "ticket_deal.aspx?page=dialy&id=" + e.Row.DataItem("ref_no").ToString

                '****** ต้องเรืยงลำดับ column ใน sql ให้ตรงกับ gridview
                If ViewState("log") IsNot Nothing Then
                    If e.Row.RowIndex <> 0 Then
                        Dim dr As Data.DataRow
                        dr = CType(ViewState("log"), Data.DataTable).Rows(e.Row.RowIndex - 1)
                        For i As Integer = 1 To 22

                            If i <> 11 And i <> 12 And i <> 17 And i <> 20 Then
                                If dr(i).ToString <> e.Row.DataItem(i).ToString Then
                                    e.Row.Cells(i).ForeColor = Drawing.Color.Red
                                End If
                            Else
                                'only date cell 11,12,17,20 (delivert_date,ticket_date,duedate,update_date)
                                Dim logDate1 As String = ""
                                Dim logDate2 As String = ""
                                If dr(i).ToString <> "" Then
                                    logDate1 = CDate(dr(i)).ToString("dd/MM/yyyy")
                                End If
                                If e.Row.DataItem(i).ToString <> "" Then
                                    logDate2 = CDate(e.Row.DataItem(i)).ToString("dd/MM/yyyy")
                                End If

                                If logDate1 <> logDate2 Then
                                    e.Row.Cells(i).ForeColor = Drawing.Color.Red
                                End If
                            End If
                        Next
                    End If

                End If


            Catch ex As Exception
                Throw ex
            End Try

        End If
    End Sub

End Class
