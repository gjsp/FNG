
Partial Class ticket_online
    Inherits basePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.QueryString("type") IsNot Nothing AndAlso Request.QueryString("billing") IsNot Nothing Then
                ODS.SelectParameters("refno").DefaultValue = Request.QueryString("id").ToString
                ODS.SelectParameters("bill").DefaultValue = Request.QueryString("billing").ToString
                ODS.SelectParameters("editType").DefaultValue = Request.QueryString("t").ToString
                ODS.Select()

                Dim user As String = ConfigurationManager.AppSettings("user").ToString
                Dim pwd As String = ConfigurationManager.AppSettings("pwd").ToString
                Dim serv As String = ConfigurationManager.AppSettings("server").ToString
                Dim db As String = ConfigurationManager.AppSettings("db").ToString

                'Dim dsRef As New CrystalDecisions.Web.DataSourceRef
                'dsRef.DataSourceID = ODS.ID
                'dsRef.TableName = "rpt_trade_online"
                'CRS.Report.DataSources.Add(dsRef)
                CRS.ReportDocument.SetDatabaseLogon(user, pwd, serv, db)
                CRS.DataBind()

            End If
        End If

      
    End Sub

End Class
