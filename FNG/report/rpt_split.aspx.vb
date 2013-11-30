Imports System.Configuration.ConfigurationManager

Partial Class report_rpt_split
    Inherits basePage
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.QueryString("ref") IsNot Nothing AndAlso Request.QueryString("type") IsNot Nothing Then
                Dim ticket_refno As String = Request.QueryString("ref").ToString
                Dim type As String = Request.QueryString("type").ToString
                Try
                    If type = clsManage.splitMode.Split.ToString + clsManage.EditMode.view.ToString Then
                        ODS.SelectParameters("preview").DefaultValue = True
                    ElseIf type = clsManage.splitMode.Split.ToString Then
                        ODS.SelectParameters("preview").DefaultValue = False
                    End If
                    ODS.SelectParameters("sp_id").DefaultValue = ticket_refno
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
