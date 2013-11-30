Imports System.Configuration.ConfigurationManager

Partial Class report_rpt_nobill
    Inherits basePage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.QueryString("type") IsNot Nothing AndAlso Request.QueryString("ref") IsNot Nothing Then
                Dim type As String = Request.QueryString("type").ToString
                Dim ticket_refno As String = Request.QueryString("ref").ToString

                Try
                    If type = "v" Then
                        ODS.SelectParameters("preview").DefaultValue = True
                        ODS.SelectParameters("view").DefaultValue = True
                    ElseIf type = "e" Then
                        ODS.SelectParameters("preview").DefaultValue = False
                    End If
                    ODS.SelectParameters("refno").DefaultValue = ticket_refno
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
