
Partial Class center_guarantee
    Inherits basePage

    Protected Sub Page_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        If Not Page.IsPostBack Then
            Try
                If Session(clsManage.iSession.user_role_center.ToString) IsNot Nothing Then
                    Dim dtRole As New Data.DataTable : dtRole = Session(clsManage.iSession.user_role_center.ToString)
                    If dtRole.Rows.Find(Me.Page.Page.ToString) IsNot Nothing Then
                        Dim dr As Data.DataRow = dtRole.Rows.Find(Me.Page.Page.ToString)
                        'Update
                        If dr(clsDB.roles.update) = False Then
                            btnSaveGrt.Enabled = False
                        End If

                    End If
                End If
            Catch ex As Exception
                clsManage.alert(Page, ex.Message)
            End Try
        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim dc As New dcDBDataContext
            Dim sto = dc.getStockOnline().FirstOrDefault
            rdoCash.SelectedValue = sto.grt_pay.ToString
            tp.ActiveTabIndex = 0
        End If
    End Sub
    Protected Sub btnSaveGrt_Click(sender As Object, e As System.EventArgs) Handles btnSaveGrt.Click
        Try
            Dim dc As New dcDBDataContext
            Dim sto = (From s In dc.stock_onlines).FirstOrDefault
            sto.grt_pay = rdoCash.SelectedValue
            dc.SubmitChanges()
            gvCash.DataBind()
            gv96.DataBind()
            gv99.DataBind()
            clsManage.alert(Page, "Update Complete")
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub
    Protected Sub upgSearch_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles upgSearch.Load
        System.Threading.Thread.Sleep(upgSearch.DisplayAfter)
    End Sub
End Class
