Imports System.Configuration.ConfigurationManager

Partial Class report_rpt_asset
    Inherits basePage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.QueryString("cid") IsNot Nothing Then
                Dim cust_id As String = Request.QueryString("cid").ToString
                Try
                    ODS.SelectParameters("custId").DefaultValue = cust_id

                    CRS.ReportDocument.SetDatabaseLogon(AppSettings("user"), AppSettings("pwd"), AppSettings("server"), AppSettings("db"))
                    CRS.DataBind()
                Catch ex As Exception
                    clsManage.alert(Page, ex.Message)
                End Try
            End If
        End If
    End Sub
End Class
