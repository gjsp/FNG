
Partial Class trade_admin_config_trade
    Inherits basePageTrade
    Dim cssHalt As String = "buttonPro small red"
    Dim cssNor As String = "buttonPro small blue"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            btnRejectAll.Attributes.Add("onclick", "return confirm('Do you want to reject leave order all?');")
            btnHalt.Attributes.Add("onclick", "return confirm('Do you want to System Halt?');")
            txtLimitQuantity.Attributes.Add("onkeypress", "numberOnly();")

        

            Dim da As New dsStockOnlineTableAdapters.getStockOnlineTableAdapter
            Dim dt As New dsStockOnline.getStockOnlineDataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                txtLimitQuantity.Text = dt.Rows(0)(dt.quantity_limitColumn).ToString
                txtMsg.Text = dt.Rows(0)(dt.msgColumn).ToString
                updateHalt(dt.Rows(0)(dt.system_haltColumn).ToString)
            End If

        End If
    End Sub
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSave.Click
        Try
            If txtLimitQuantity.Text.Trim = "" Then clsManage.alert(Page, "Please Enter Limit Quantity") : Exit Sub
            Dim da As New dsStockOnlineTableAdapters.getStockOnlineTableAdapter
            Dim result As Integer = da.updateStockOnlineConfig(txtMsg.Text, txtLimitQuantity.Text)
            If result > 0 Then
                clsManage.alert(Page, "Update Complete.")
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Protected Sub btnRejectAll_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnRejectAll.Click
        Try
            Dim result As Integer
            result = clsMain.rejectLeaveOrderAll()
            If result > 0 Then
                clsManage.alert(Page, "Update Complete.")
            Else
                clsManage.alert(Page, "Sorry No Leave Order to Update.")
            End If
        Catch ex As Exception
            clsManage.alert(Page, ex.Message)
        End Try
    End Sub

    Sub updateHalt(ByVal halt As String)
        If halt = "" Then Exit Sub
        If halt = "n" Then
            btnHalt.CssClass = cssNor
        Else
            btnHalt.CssClass = cssHalt
        End If
        btnHalt.CommandArgument = halt
    End Sub


    Protected Sub btnHalt_Click(sender As Object, e As System.EventArgs) Handles btnHalt.Click
        Try
            Dim dc As New dcDBDataContext
            Dim stock = (From s In dc.stock_onlines Select s).FirstOrDefault
            Dim halt As String = ""
            If btnHalt.CommandArgument = "n" Then
                halt = "y"
            Else
                halt = "n"
            End If
            stock.system_halt = halt
            stock.modifier_by = Session(clsManage.iSession.user_name.ToString).ToString
            dc.SubmitChanges()
            updateHalt(halt)
        Catch ex As Exception
            clsManage.alert(Page, ex.Message, , , "err")
        End Try
    End Sub

    Protected Sub UpdateProg1_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles UpdateProg1.Load
        System.Threading.Thread.Sleep(UpdateProg1.DisplayAfter)
    End Sub
End Class
