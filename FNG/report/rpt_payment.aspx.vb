Imports System.Configuration.ConfigurationManager

Partial Class report_rpt_payment
    Inherits basePage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.QueryString("pid") IsNot Nothing Then
                Try
                    ODS.SelectParameters("payment_id").DefaultValue = Request.QueryString("pid").ToString
                    ODS.SelectParameters("report_by").DefaultValue = Session(clsManage.iSession.user_name_center.ToString)

                    CRS.ReportDocument.SetDatabaseLogon(AppSettings("user"), AppSettings("pwd"), AppSettings("server"), AppSettings("db"))
                    CRS.DataBind()
                Catch ex As Exception
                    clsManage.alert(Page, ex.Message)
                End Try
            End If
        End If
    End Sub
End Class
