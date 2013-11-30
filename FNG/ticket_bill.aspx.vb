
Partial Class ticket_bill
    Inherits basePage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            If Request.QueryString("type") IsNot Nothing AndAlso Request.QueryString("billing") IsNot Nothing AndAlso Request.QueryString("ref") IsNot Nothing Then
                Dim type As String = Request.QueryString("type").ToString
                Dim billing As String = Request.QueryString("billing").ToString
                Dim ticket_refno As String = Request.QueryString("ref").ToString


                Try
                    'Dim dsRef As New CrystalDecisions.Web.DataSourceRef
                    'dsRef.TableName = tbName
                    'crsNB.Report.DataSources.Add(dsRef)

                    Dim user As String = ConfigurationManager.AppSettings("user").ToString
                    Dim pwd As String = ConfigurationManager.AppSettings("pwd").ToString
                    Dim serv As String = ConfigurationManager.AppSettings("server").ToString
                    Dim db As String = ConfigurationManager.AppSettings("db").ToString

                    If billing = "n" Then
                        If type = "v" Then
                            Response.Redirect("report/rpt_nobill.aspx?ref=" + ticket_refno + "&type=" + type + "&billing=" + billing + "")
                            ''clsDB.getTicketOrderNobill
                            'With odsNB
                            '    .SelectParameters("refno").DefaultValue = ticket_refno
                            '    .SelectParameters("preview").DefaultValue = True
                            '    .SelectParameters("view").DefaultValue = True
                            '    .Select()
                            '    CRV.ReportSourceID = crsNB.ID
                            '    crsNB.ReportDocument.SetDatabaseLogon(user, pwd, serv, db)
                            '    crsNB.DataBind()
                            'End With
                        ElseIf type = clsManage.splitMode.Split.ToString Then
                            ''only split mode ticket_refno = split_id
                            ''clsDB.getTicketOrderNobillSplit
                            'With odsSplit
                            '    .SelectParameters("sp_id").DefaultValue = ticket_refno
                            '    .SelectParameters("preview").DefaultValue = False
                            '    .Select()
                            '    CRV.ReportSourceID = crsSplit.ID
                            '    crsSplit.ReportDocument.SetDatabaseLogon(user, pwd, serv, db)
                            '    crsSplit.DataBind()
                            'End With

                        ElseIf type = clsManage.splitMode.Split.ToString + clsManage.EditMode.view.ToString Then
                            ''only split mode ticket_refno = split_id
                            ''clsDB.getTicketOrderNobillSplit
                            'With odsSplit
                            '    .SelectParameters("sp_id").DefaultValue = ticket_refno
                            '    .SelectParameters("preview").DefaultValue = True
                            '    .Select()
                            '    CRV.ReportSourceID = crsSplit.ID
                            '    crsSplit.ReportDocument.SetDatabaseLogon(user, pwd, serv, db)
                            '    crsSplit.DataBind()
                            'End With
                        Else
                            Response.Redirect("report/rpt_nobill.aspx?ref=" + ticket_refno + "&type=" + type + "&billing=" + billing + "")
                            'clsDB.getTicketOrderNobill()
                            'With odsNB
                            '    .SelectParameters("refno").DefaultValue = ticket_refno
                            '    .SelectParameters("preview").DefaultValue = False
                            '    .Select()
                            '    CRV.ReportSourceID = crsNB.ID
                            '    crsNB.ReportDocument.SetDatabaseLogon(user, pwd, serv, db)
                            '    crsNB.DataBind()
                            'End With
                        End If
                    ElseIf billing = "y" Then
                        'clsDB.getTicketOrder()
                        'With odsBill
                        '    If type = "v" Then
                        '        .SelectParameters("view").DefaultValue = True
                        '    Else
                        '        .SelectParameters("view").DefaultValue = False
                        '    End If
                        '    .SelectParameters("refno").DefaultValue = ticket_refno
                        '    .Select()
                        '    CRV.ReportSourceID = crsBill.ID
                        '    crsBill.ReportDocument.SetDatabaseLogon(user, pwd, serv, db)
                        '    crsBill.DataBind()
                        'End With
                    ElseIf billing = "p" Then
                        'clsDB.getTicketOrderNobill
                        Response.Redirect("report/rpt_nobill.aspx?ref=" + ticket_refno + "&type=" + type + "&billing=" + billing + "")
                        With odsNB
                            .SelectParameters("refno").DefaultValue = ticket_refno
                            .SelectParameters("preview").DefaultValue = True
                            .Select()
                            CRV.ReportSourceID = crsNB.ID
                            crsNB.ReportDocument.SetDatabaseLogon(user, pwd, serv, db)
                            crsNB.DataBind()
                        End With
                    Else
                        If type = "D/W" Or type = "C" Then
                            ''deposit and withdraw
                            ''clsDB.getReportTrans
                            'With odsTrans
                            '    .SelectParameters("refno").DefaultValue = ticket_refno
                            '    .Select()
                            '    CRV.ReportSourceID = crsTrans.ID
                            '    crsTrans.ReportDocument.SetDatabaseLogon(user, pwd, serv, db)
                            '    crsTrans.DataBind()
                            'End With
                        End If
                    End If
                Catch ex As Exception
                    clsManage.alert(Page, ex.Message)
                End Try
            End If
        End If
    End Sub
End Class
