Imports System.Configuration.ConfigurationManager

Partial Class report_rpt_preview
    Inherits basePage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.QueryString("ref") IsNot Nothing And Session(clsManage.iSession.user_id_center.ToString) IsNot Nothing Then
                Dim ticket_refno As String = Request.QueryString("ref").ToString
                Try
                    ODS.SelectParameters("refno").DefaultValue = ticket_refno
                    ODS.SelectParameters("preview").DefaultValue = True
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
