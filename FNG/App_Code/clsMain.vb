Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System.Net.Mail.SmtpClient
Imports System.Net.Mail
Imports System.Configuration.ConfigurationManager
Imports System.Configuration

Public Class clsMain
    Public Shared strcon As String = ConfigurationManager.ConnectionStrings("FNGConnectionString").ConnectionString

    Public Shared Function sendEmailForget(ByVal MailTo As String, ByVal mailToName As String, ByVal msgBody As String) As Boolean
        Try
            Dim smtpclient As New SmtpClient
            Dim message As New MailMessage
            Dim fromAddr As MailAddress

            smtpclient.Host = AppSettings("SMTP_HOST")
            smtpclient.Port = AppSettings("SMTP_PORT")
            smtpclient.UseDefaultCredentials = False
            smtpclient.Credentials = New System.Net.NetworkCredential(AppSettings("SMTP_CREDENTIALS_NAME"), AppSettings("SMTP_CREDENTIALS_PASS"))

            'send customer
            message = New MailMessage
            message.Subject = "Forget Password " + mailToName
            message.To.Add(MailTo)
            message.IsBodyHtml = True
            message.Body = msgBody
            fromAddr = New MailAddress(AppSettings("SMTP_SENDER_MAIL"), AppSettings("SMTP_SENDER_NAME"))
            message.From = fromAddr
            smtpclient.Send(message)

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function sendEmailRegisterTrade(ByVal MailTo As String, ByVal mailToName As String, ByVal msgBody As String, Optional subject As String = "", Optional attPath As String = "") As Boolean
        Try
            Dim smtpclient As New SmtpClient
            Dim message As New MailMessage
            Dim fromAddr As MailAddress

            smtpclient.Host = AppSettings("SMTP_HOST")
            smtpclient.Port = AppSettings("SMTP_PORT")
            smtpclient.UseDefaultCredentials = False
            'smtpclient.EnableSsl = True
            smtpclient.Credentials = New System.Net.NetworkCredential(AppSettings("SMTP_CREDENTIALS_NAME"), AppSettings("SMTP_CREDENTIALS_PASS"))

            'send customer
            message = New MailMessage
            If subject <> "" Then
                message.Subject = subject + mailToName
            Else
                message.Subject = "Confirm Register " + mailToName
            End If
            If attPath <> "" Then
                message.Attachments.Add(New Attachment(attPath))
            End If
            message.To.Add(MailTo)
            message.IsBodyHtml = True
            message.Body = msgBody
            fromAddr = New MailAddress(AppSettings("SMTP_SENDER_MAIL"), AppSettings("SMTP_SENDER_NAME"))
            message.From = fromAddr
            smtpclient.Send(message)

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function


    Public Shared Function sendEmailResetUsernamePwd(ByVal MailTo As String, ByVal mailToName As String, ByVal msgBody As String, Optional attPath As String = "") As Boolean
        Try
            Dim smtpclient As New SmtpClient
            Dim message As New MailMessage
            Dim fromAddr As MailAddress

            smtpclient.Host = AppSettings("SMTP_HOST")
            smtpclient.Port = AppSettings("SMTP_PORT")
            smtpclient.UseDefaultCredentials = False
            smtpclient.Credentials = New System.Net.NetworkCredential(AppSettings("SMTP_CREDENTIALS_NAME"), AppSettings("SMTP_CREDENTIALS_PASS"))


            'send customer

            message = New MailMessage
            message.Subject = "Reset Username Passwod " + mailToName
            If attPath <> "" Then
                message.Attachments.Add(New Attachment(attPath))
            End If
            message.To.Add(MailTo)
            message.IsBodyHtml = True
            message.Body = msgBody
            fromAddr = New MailAddress(AppSettings("SMTP_SENDER_MAIL"), AppSettings("SMTP_SENDER_NAME"))
            message.From = fromAddr
            smtpclient.Send(message)

            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Shared Function getDatetimeServer() As DateTime
        Try
            Dim sql As String = "select dateadd(hour,0, getdate()) from stock_online"

            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable

            da.Fill(dt)
            Return dt.Rows(0)(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function

#Region "Transaction Auto Accept"

    Public Shared Function autoAcceptByPrice2(htPrice As Hashtable) As Boolean
        Dim dt As New DataTable
        Try

            Dim price As String = ""
            Dim type As String = ""
            Dim purity As String = ""
            Dim level As String = ""
            Dim sql_update As String = ""

            Using con As New SqlConnection(strcon)
                Using cmd As New SqlCommand
                    Using da As New SqlDataAdapter

                        For Each dicPrice As DictionaryEntry In htPrice
                            price = dicPrice.Value
                            type = dicPrice.Key.ToString().Split(",")(0)
                            purity = dicPrice.Key.ToString().Split(",")(1)
                            level = dicPrice.Key.ToString().Split(",")(2)


                            Dim sql As String = String.Format("select trade_id,quantity,(select quan_per{0}_{1} from quantity_accept) as per,(select quan_max{0}_{1} from quantity_accept) as max, ISNULL((select quantity from quantity_order where type = '{1}' and purity={0} and price={2}),0) as sum_quantity" & _
                                                              " from trade t inner join usernames u ON t.cust_id = u.cust_id where mode = 'tran' and price = {2} and quantity <= (select quan_per{0}_{1} from quantity_accept) and type = '{1}'" & _
                                                              " and substring(gold_type_id,1,2) = '{0}' and accept_type = 'P' and leave_order='y' and u.cust_level = {3}  order by t.created_date asc", purity, type, clsManage.convert2zero(price), level)

                            da.SelectCommand = New SqlCommand(sql, con)
                            da.Fill(dt)
                            Dim over As Boolean = False
                            If dt.Rows.Count > 0 Then
                                Dim sum As Integer = CInt(dt.Rows(0)("sum_quantity").ToString)
                                Dim max As Integer = CInt(dt.Rows(0)("max").ToString)
                                Dim per As Integer = CInt(dt.Rows(0)("per").ToString)

                                For i As Integer = 0 To dt.Rows.Count - 1
                                    If sum + CInt(dt.Rows(i)("quantity")) <= max And Not over Then
                                        sql_update += String.Format("update trade set mode = '{0}',accept_type = 'F',modifier_date=getdate() where trade_id = {1} ;", clsManage.tradeMode.accept.ToString, dt.Rows(i)("trade_id").ToString)
                                        sum += CInt(dt.Rows(i)("quantity"))
                                        If sum = max Then over = True
                                    Else
                                        over = True
                                        sql_update += String.Format("update trade set accept_type = 'M',modifier_date=getdate() where trade_id = {0} ;", dt.Rows(i)("trade_id").ToString)
                                    End If
                                Next
                                If over Then
                                    sql_update += String.Format("update quantity_order set max = 'y' where price = {0} and type = '{1}' and purity = '{2} ;'  ", clsManage.convert2zero(price), type, purity)
                                End If
                            End If
                        Next
                        If sql_update <> "" Then
                            cmd.CommandText = sql_update
                            con.Open()
                            Dim result As Integer = cmd.ExecuteNonQuery()
                            If result > 0 Then
                                Return True
                            Else
                                Return False
                            End If
                        End If
                        Return True
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            dt.Dispose()
        End Try
    End Function

    Public Shared Function autoAcceptByPrice(type As String, purity As String, price As String, level As String) As Boolean
        Dim dt As New DataTable
        Try
            Dim sql As String = String.Format("select trade_id,quantity,(select quan_per{0}_{1} from quantity_accept) as per,(select quan_max{0}_{1} from quantity_accept) as max, ISNULL((select quantity from quantity_order where type = '{1}' and purity={0} and price={2}),0) as sum_quantity" & _
                                              " from trade t inner join usernames u ON t.cust_id = u.cust_id where mode = 'tran' and price = {2} and quantity <= (select quan_per{0}_{1} from quantity_accept) and type = '{1}'" & _
                                              " and substring(gold_type_id,1,2) = '{0}' and accept_type = 'P' and leave_order='y' and u.cust_level = {3}  order by t.created_date asc", purity, type, clsManage.convert2zero(price), level)
            Using con As New SqlConnection(strcon)
                Using cmd As New SqlCommand(sql, con)
                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(dt)
                        sql = ""
                        Dim over As Boolean = False
                        If dt.Rows.Count > 0 Then
                            Dim sum As Integer = CInt(dt.Rows(0)("sum_quantity").ToString)
                            Dim max As Integer = CInt(dt.Rows(0)("max").ToString)
                            Dim per As Integer = CInt(dt.Rows(0)("per").ToString)


                            For i As Integer = 0 To dt.Rows.Count - 1
                                If sum + CInt(dt.Rows(i)("quantity")) <= max And Not over Then
                                    sql += String.Format("update trade set mode = '{0}',accept_type = 'F',modifier_date=getdate() where trade_id = {1} ;", clsManage.tradeMode.accept.ToString, dt.Rows(i)("trade_id").ToString)
                                    sum += CInt(dt.Rows(i)("quantity"))
                                    If sum = max Then over = True
                                Else
                                    over = True
                                    sql += String.Format("update trade set accept_type = 'M',modifier_date=getdate() where trade_id = {0} ;", dt.Rows(i)("trade_id").ToString)
                                End If
                            Next

                            If over Then
                                sql += String.Format("update quantity_order set max = 'y' where price = {0} and type = '{1}' and purity = '{2}'  ", clsManage.convert2zero(price), type, purity)
                            End If

                            cmd.CommandText = sql
                            con.Open()
                            Dim result As Integer = cmd.ExecuteNonQuery()
                            If result > 0 Then
                                Return True
                            Else
                                Return False
                            End If
                        End If
                        Return True
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            dt.Dispose()
        End Try
    End Function

    Public Shared Function ClearAllAutoAccept2(type As String, purity As String) As Integer
        Dim dt As New DataTable
        Try
            dt = clsMain.getQuantityAccept()
            Dim Max As String = dt.Rows(0)(String.Format("quan_max{0}_{1}", purity, type)).ToString
            Dim Per As String = dt.Rows(0)(String.Format("quan_per{0}_{1}", purity, type)).ToString

            dt = New DataTable
            dt = getTranAutoAccept(type, purity)
            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    clearAutoAccept(type, purity, dr("price").ToString, Per, Max)
                Next
            End If
        Catch ex As Exception
            Throw ex
        Finally
            dt.Dispose()
        End Try
    End Function

    Public Shared Function ClearAllAutoAccept(type As String, purity As String) As Integer
        Dim dt As New DataTable
        Try
            Dim sql As String = String.Format(" select trade_id,quantity from trade where mode = 'tran' and quantity <= (select quan_per{0}_{1} from quantity_accept) and type = '{1}' " & _
                                " and substring(gold_type_id,1,2) = '{0}' and accept_type = 'M'  order by created_date asc", purity, type)
            Using con As New SqlConnection(strcon)
                Using cmd As New SqlCommand(sql, con)
                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(dt)
                        sql = ""
                        If dt.Rows.Count > 0 Then
                            For i As Integer = 0 To dt.Rows.Count - 1
                                sql += String.Format("update trade set mode = '{0}',modifier_date=getdate() where trade_id = {1} ;", clsManage.tradeMode.accept.ToString, dt.Rows(i)("trade_id").ToString)
                            Next
                        End If
                    End Using

                    '** ใช้ update ทีละหลาย record ไม่ได้ ติด trigger
                    'sql = String.Format("DELETE FROM quantity_order WHERE type='{0}' and purity = '{1}';" & _
                    '                                  "UPDATE trade set mode = '{3}',accept_type='A',modifier_date=getdate() " & _
                    '                                  "where mode = '{4}' and type = 'buy' and substring(gold_type_id,1,2) = '{1}' and quantity <={2} ", type, purity, per, clsManage.tradeMode.accept.ToString, clsManage.tradeMode.tran.ToString)

                    sql += String.Format("DELETE FROM quantity_order WHERE type='{0}' and purity = '{1}';", type, purity)
                    cmd.CommandText = sql
                    con.Open()
                    Dim result As Integer = cmd.ExecuteNonQuery()
                    Return result
                End Using
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            dt.Dispose()
        End Try
    End Function

    Public Shared Function clearAutoAccept(type As String, purity As String, price As String, per As String, max As String) As Integer
        Dim dt As New DataTable
        Try
            'Note : clear auto accept ต้องดูว่ามี tran ราคานี้ค้างอยู่หรือไม่ ถ้าค้างให้ auto accept ต่อในรอบถัดไป แต่ต้องไม่เกิน max 

            Dim sql As String = String.Format("select trade_id,quantity from trade where mode = 'tran' and price = {0} and quantity <= {1} and type = '{2}' and substring(gold_type_id,1,2) = '{3}' and accept_type = 'M' order by created_date asc", price, per, type, purity)
            Using con As New SqlConnection(strcon)
                Using cmd As New SqlCommand(sql, con)
                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(dt)
                        If dt.Rows.Count > 0 Then
                            Dim sum As Integer = 0
                            Dim trade_id_list As String = ""
                            Dim over As Boolean = False
                            sql = ""
                            For i As Integer = 0 To dt.Rows.Count - 1
                                If sum + CInt(dt.Rows(i)("quantity")) <= max Then
                                    sql += String.Format("update trade set mode = '{0}',modifier_date=getdate() where trade_id = {1} ;", clsManage.tradeMode.accept.ToString, dt.Rows(i)("trade_id").ToString)
                                    sum += CInt(dt.Rows(i)("quantity"))
                                    If sum = max Then over = True
                                Else
                                    over = True
                                    Exit For
                                End If
                            Next
                            If over Then
                                sql += String.Format("update quantity_order set quantity = {0} where price = {1} and type = '{2}' and purity = '{3}'", sum, price, type, purity)
                            Else
                                sql += String.Format("update quantity_order set quantity = {0},max = NULL where price = {1} and type = '{2}' and purity = '{3}'", sum, price, type, purity)
                            End If

                            cmd.CommandText = sql
                            con.Open()
                            Dim result As Integer = cmd.ExecuteNonQuery()
                            Return result
                        Else
                            sql = String.Format("DELETE FROM quantity_order WHERE price = {0}", price)
                            cmd.CommandText = sql
                            con.Open()
                            Dim result As Integer = cmd.ExecuteNonQuery()
                            Return result
                        End If
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            dt.Dispose()
        End Try
    End Function

    Public Shared Function updateQuantityAccept(ByVal max As String, per As String, type As String, purity As String) As Integer
        Try
            Dim sql As String = String.Format("update quantity_accept set quan_max{0}_{1} = {2},quan_per{0}_{1} = {3} ", purity, type, max, per)

            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)
            con.Open()
            Dim result As Integer = cmd.ExecuteNonQuery()
            con.Close()
            Return result
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function checkTranBeforeAccept(price As String, ByVal type As String, purity As String, quantity As String) As Boolean
        Try
            Dim sql As String = String.Format("select top 1 quantity,max from quantity_order where price = {2} and type = '{0}' and purity = '{1}'" + _
                                              ";select quan_max{1}_{0},quan_per{1}_{0} from quantity_accept", type, purity, price)
            Dim con As New SqlConnection(strcon)
            Try
                Dim dt As New DataTable
                Dim ds As New DataSet
                Dim da As New SqlDataAdapter(sql, con)
                da.Fill(ds)
                Dim quan As Integer = CInt(quantity)
                Dim quantity_order As Integer = 0
                Dim IsMax As String = ""
                If ds.Tables(0).Rows.Count > 0 Then
                    quantity_order = ds.Tables(0).Rows(0)("quantity")
                    IsMax = ds.Tables(0).Rows(0)("max").ToString
                End If

                Dim quan_max As Integer = ds.Tables(1).Rows(0)(0)
                Dim quan_per As Integer = ds.Tables(1).Rows(0)(1)

                'case quantity มากกว่า ต่อครั้ง
                If quan > quan_per Then
                    Return False
                End If

                'case max
                If IsMax = "y" Then
                    Return False
                End If

                'case quantity รวมกับ ที่มี เกิน max
                If quantity_order + quan >= quan_max Then
                    'update tbl ให้รู้ว่า trade ต่อไม่ได้แล้ว
                    Dim sql_update_max As String = String.Format("update quantity_order set max = 'y' where price = {0} and type = '{1}' and purity = '{2}'  ", price, type, purity)
                    Dim cmd As New SqlCommand(sql_update_max, con)
                    con.Open() : Dim result As Integer = cmd.ExecuteNonQuery() : con.Close()
                End If

                If quantity_order + quan > quan_max Then
                    Return False
                End If

            Catch ex As Exception
                Return False
            End Try
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getTranAutoAccept(ByVal type As String, purity As String) As DataTable

        Dim sql As String = "getQuantityOrder"
        Dim dt As New DataTable
        Try

            Using con As New SqlConnection(strcon)
                Using cmd As New SqlCommand()

                    cmd.Connection = con
                    cmd.CommandText = sql
                    cmd.CommandType = CommandType.StoredProcedure

                    Dim parameter As New SqlParameter("@type", SqlDbType.VarChar, 5)
                    parameter.Value = type
                    cmd.Parameters.Add(parameter)

                    parameter = New SqlParameter("@purity", SqlDbType.VarChar, 5)
                    parameter.Value = purity
                    cmd.Parameters.Add(parameter)
                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            dt.Dispose()
        End Try
    End Function

    Public Shared Function getQuantityAccept() As DataTable

        Dim sql As String = "select * from quantity_accept"
        Dim con As New SqlConnection(strcon)
        Try
            Dim dt As New DataTable
            Dim da As New SqlDataAdapter(sql, con)
            da.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getAllQuantityOrder() As String
        Dim dt As New DataTable
        Try
            Dim sql As String = "select type,purity from quantity_order"
            Using con As New SqlConnection(strcon)
                Using da As New SqlDataAdapter(sql, con)
                    da.Fill(dt)

                    Dim buy96 As String = "n"
                    Dim sell96 As String = "n"
                    Dim buy99 As String = "n"
                    Dim sell99 As String = "n"

                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows
                            If dr("type").ToString = "buy" And dr("purity").ToString = "96" Then
                                buy96 = "y"
                            End If
                            If dr("type").ToString = "sell" And dr("purity").ToString = "96" Then
                                sell96 = "y"
                            End If
                            If dr("type").ToString = "buy" And dr("purity").ToString = "99" Then
                                buy99 = "y"
                            End If
                            If dr("type").ToString = "buy" And dr("purity").ToString = "99" Then
                                sell99 = "y"
                            End If
                        Next
                    End If
                    Return buy96 + sell96 + buy99 + sell99
                End Using
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            dt.Dispose()
        End Try
    End Function

    Public Shared Function getQuantityAcceptAdap(type As String, purity As String) As String
        Try
            Dim sql As String = "getQuantityAcceptAdap"
            Using con As New SqlConnection(strcon)
                Using cmd As New SqlCommand()

                    Dim dt As New DataTable
                    cmd.Connection = con
                    cmd.CommandText = sql
                    cmd.CommandType = CommandType.StoredProcedure
                    cmd.Parameters.Add("@type", SqlDbType.VarChar, 5).Value = type
                    cmd.Parameters.Add("@purity", SqlDbType.VarChar, 5).Value = purity
                    Dim da As New SqlDataAdapter(cmd)
                    da.Fill(dt)

                    Dim result As String = ""
                    If dt.Rows.Count > 0 Then
                        For Each dr As DataRow In dt.Rows
                            result += dr(0).ToString + dr(1).ToString
                        Next
                    End If
                    Return result
                End Using
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region

#Region "Trade"

    Public Shared Function rejectTimeout() As Integer

        Dim con As New SqlConnection(strcon)
        Dim sql As String = "EXEC rejectLeaveTimeout"
        Dim cmd As New SqlCommand(sql, con)
        cmd.CommandText = sql
        con.Open()
        Try
            Dim result As Integer = cmd.ExecuteNonQuery()
            Return result

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
    End Function

    Public Shared Function rejectLeaveOrderAll() As Integer

        Dim con As New SqlConnection(strcon)
        Dim sql As String = "Exec [rejectLeaveOrder] "
        Dim cmd As New SqlCommand(sql, con)
        cmd.CommandText = sql
        con.Open()
        Try
            Dim result As Integer = cmd.ExecuteNonQuery()
            Return result

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
    End Function

    Public Shared Function getRealPricePlus() As String
        Dim dc As New dcDBDataContext()
        Dim sto = dc.getStockOnline.Single
        Dim strPrice As String = ""
        Dim cma = "," : Dim l As String = "|" : Dim shp As String = "#"
        If sto.self_price = "n" Then
            Dim gcapPrice As String = clsStore.getPriceStoreAll()

            'sto.ask99_plus
            strPrice = gcapPrice.Split(shp)(0).Split(l)(0)
            strPrice += cma + gcapPrice.Split(shp)(0).Split(l)(1)
            strPrice += cma + gcapPrice.Split(shp)(1).Split(l)(0)
            strPrice += cma + gcapPrice.Split(shp)(1).Split(l)(1)

            strPrice += cma + IIf(sto.bid99_plus Is Nothing, 0, sto.bid99_plus.ToString) + cma + IIf(sto.ask99_plus Is Nothing, 0, sto.ask99_plus.ToString)
            strPrice += cma + IIf(sto.bid96_plus Is Nothing, 0, sto.bid96_plus.ToString) + cma + IIf(sto.ask96_plus Is Nothing, 0, sto.ask96_plus.ToString)
        Else
            strPrice = sto.bid99_1.ToString + cma + sto.bid99_2.ToString + cma + sto.bid99_3.ToString + cma
            strPrice += sto.ask99_1.ToString + cma + sto.ask99_2.ToString + cma + sto.ask99_3.ToString + cma
            strPrice += sto.bid96_1.ToString + cma + sto.bid96_2.ToString + cma + sto.bid96_3.ToString + cma
            strPrice += sto.ask96_1.ToString + cma + sto.ask96_2.ToString + cma + sto.ask96_3.ToString + cma
            strPrice += "0,0,0,0" 'no plus
        End If
        Return strPrice

    End Function

    Public Shared Function getRealPrice() As String
        Dim dc As New dcDBDataContext()
        Dim sto = dc.getStockOnline.Single
        Dim strPrice As String = ""
        Dim cma = "," : Dim l As String = "|" : Dim shp As String = "#"
        If sto.self_price = "n" Then
            Dim gcapPrice As String = clsStore.getPriceStoreAll()

            'sto.ask99_plus
            strPrice = gcapPrice.Split(shp)(0).Split(l)(0)
            strPrice += cma + gcapPrice.Split(shp)(0).Split(l)(1)
            strPrice += cma + gcapPrice.Split(shp)(1).Split(l)(0)
            strPrice += cma + gcapPrice.Split(shp)(1).Split(l)(1)

        Else
            strPrice = sto.bid99_1.ToString + cma + sto.bid99_2.ToString + cma + sto.bid99_3.ToString + cma
            strPrice += sto.ask99_1.ToString + cma + sto.ask99_2.ToString + cma + sto.ask99_3.ToString + cma
            strPrice += sto.bid96_1.ToString + cma + sto.bid96_2.ToString + cma + sto.bid96_3.ToString + cma
            strPrice += sto.ask96_1.ToString + cma + sto.ask96_2.ToString + cma + sto.ask96_3.ToString + cma
        End If
        Return strPrice

        'Dim sql As String = "SELECT CAST( bid99_1 as varchar) +','+ CAST( bid99_2 as varchar)+','+ CAST( bid99_3 as varchar)+','+ CAST( ask99_1 as varchar)+','+ CAST( ask99_2 as varchar)+','+ CAST( ask99_3 as varchar)" & _
        '" +','+ CAST( bid96_1 as varchar)+','+ CAST( bid96_2 as varchar)+','+ CAST( bid96_3 as varchar)+','+ CAST( ask96_1 as varchar)+','+ CAST( ask96_2 as varchar)+','+ CAST( ask96_3 as varchar),self_price" & _
        '" FROM stock_online where stock_id = 1"
        'Using con As New SqlConnection(strcon)
        '    Dim da As New SqlDataAdapter(sql, con)
        '    Dim dt As New DataTable
        '    Try
        '        da.Fill(dt)

        '        Return dt.Rows(0)(0).ToString
        '    Catch ex As Exception
        '        Throw ex
        '    Finally
        '        da.Dispose()
        '        dt.Dispose()
        '    End Try
        'End Using
    End Function

    Public Shared Function getTradeTransLogId(ByVal log_id As String) As String
        Dim sql As String = String.Format("select log_id,t.trade_id,type from log_tran l inner join trade t on t.trade_id = l.trade_id where l.log_id > {0} ", log_id)
        Using con As New SqlConnection(strcon)
            Dim da As New SqlDataAdapter(sql, con)
            Dim dt As New DataTable
            Try
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    'buy:1,sell:2,both:3
                    Dim order_type As String = ""
                    For i = 0 To dt.Rows.Count - 1
                        If order_type = "" Then
                            order_type = dt.Rows(i)("type").ToString
                        Else
                            If order_type <> dt.Rows(i)("type").ToString Then
                                order_type = "3"
                            End If
                        End If
                    Next

                    If order_type = "buy" Then
                        order_type = "1"
                    ElseIf order_type = "sell" Then
                        order_type = "2"
                    End If
                    Return dt.Rows(0)(0).ToString + "," + order_type
                End If
                Return "0,0"
            Catch ex As Exception
                Throw ex
            Finally
                da.Dispose()
                dt.Dispose()
            End Try
        End Using
    End Function

    Public Shared Function getTradeLogId(ByVal mode As String) As String
        If mode = "" Then Return "0"
        Dim sql As String = "select isnull(max(log_id),0) as log_id from log_" + mode
        Dim con As New SqlConnection(strcon)
        Try
            Dim da As New SqlDataAdapter(sql, con)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Return dt.Rows(0)(0).ToString
            End If
            Return "0"
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getTicketRunNo() As String

        Dim sql As String = "select max(run_no)+1 from ticket_runno " & _
                            "where convert(varchar,dateadd(day,0,datetime), 101)= convert(varchar,getdate(), 101) "
        Dim con As New SqlConnection(strcon)
        Try
            Dim da As New SqlDataAdapter(sql, con)
            Dim dt As New DataTable
            da.Fill(dt)
            Dim result As Integer = 1
            If dt.Rows.Count > 0 Then
                If dt.Rows(0)(0).ToString <> "" Then
                    result = dt.Rows(0)(0).ToString
                End If
            End If
            sql = String.Format("UPDATE ticket_runno set run_no = {0},datetime = getdate()", result)

            con.Open()
            Dim tr As SqlTransaction = con.BeginTransaction
            Try
                Dim cmd As New SqlCommand(sql, con)
                cmd.Transaction = tr
                Dim resultUpdate As Integer = cmd.ExecuteNonQuery()
                If resultUpdate > 0 Then
                    tr.Commit()
                    Return "R" + DateTime.Now.ToString("ddMMyy/") + result.ToString("0000")
                End If
            Catch ex As Exception
                tr.Rollback()
            Finally
                con.Close()
            End Try

            Return ""
        Catch ex As Exception
            Throw ex

        End Try
    End Function

    Public Shared Function getTradeSummaryAccept(Optional ByVal period As String = "", Optional ByVal pDate1 As DateTime = Nothing, Optional ByVal pDate2 As DateTime = Nothing) As DataTable
        Try
            Dim sql_period As String = ""
            Dim period1 As String = ""
            Dim period2 As String = ""
            If period = "" Then
                'sql_period = " AND convert(varchar,modifier_date,111)=convert(varchar,getdate(),111) "
                sql_period = " AND (convert(datetime,modifier_date,111) between convert(datetime,@period1,111) and getdate()) "
            Else
                sql_period = " AND (modifier_date between @period1 and @period2) "
            End If
            ' (substring(cast(ticket.gold_type_id as varchar),1,2)  = '99')
            'Dim sql As String = "select " & _
            '" sum(case when(type = 'buy' and gold_type_id = '96') then quantity else 0 end) as buy96," & _
            '" sum(case when(type = 'sell' and gold_type_id = '96') then quantity else 0 end) as sell96," & _
            '" sum(case when(type = 'buy' and gold_type_id <> '96') then quantity else 0 end) as buy99," & _
            '" sum(case when(type = 'sell' and gold_type_id <> '96') then quantity else 0 end) as sell99" & _
            '" from trade where mode = 'accept' " & _
            '" " + sql_period
            Dim sql As String = "select " & _
           " sum(case when(type = 'buy' and substring(cast(gold_type_id as varchar),1,2) = '96') then quantity else 0 end) as buy96," & _
           " sum(case when(type = 'sell' and substring(cast(gold_type_id as varchar),1,2) = '96') then quantity else 0 end) as sell96," & _
           " sum(case when(type = 'buy' and substring(cast(gold_type_id as varchar),1,2) = '99') then quantity else 0 end) as buy99," & _
           " sum(case when(type = 'sell' and substring(cast(gold_type_id as varchar),1,2) = '99') then quantity else 0 end) as sell99" & _
           " from trade where mode = 'accept' " & _
           " " + sql_period

            'and convert(varchar,modifier_date,103) = convert(varchar,getdate(),103)"

            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)

            Dim parameter As New SqlParameter("@period1", SqlDbType.DateTime)
            parameter.Value = pDate1
            cmd.Parameters.Add(parameter)

            If period <> "" Then
                parameter = New SqlParameter("@period2", SqlDbType.DateTime)
                parameter.Value = pDate2
                cmd.Parameters.Add(parameter)
            End If

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable

            da.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getTradeSummaryAcceptManagePrice() As DataTable
        Try
            Dim sql As String = "select sell96 - buy96 as net96 , sell99 - buy99 as net99 from(" & _
            " select " & _
            " sum(case when(type = 'buy' and gold_type_id = '96') then quantity else 0 end) as buy96," & _
            " sum(case when(type = 'sell' and gold_type_id = '96') then quantity else 0 end) as sell96," & _
            " sum(case when(type = 'buy' and gold_type_id <> '96') then quantity else 0 end) as buy99," & _
            " sum(case when(type = 'sell' and gold_type_id <> '96') then quantity else 0 end) as sell99" & _
            " from trade where mode = 'accept' AND convert(varchar,modifier_date,111) = convert(varchar,getdate(),111) )net"

            Using con As New SqlConnection(strcon)
                Using cmd As New SqlCommand(sql, con)
                    Dim da As New SqlDataAdapter(cmd)
                    Dim dt As New DataTable
                    da.Fill(dt)
                    Return dt
                End Using
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Shared Function checkTradeLimitOnline(ByVal cust_id As String, ByVal quantity As Double, ByVal amount As Double, ByVal type As String, ByVal purity As String, Optional ByVal leave As String = "") As String
        'For trade online 
        '**เพิ่ม จำนวน trade ที่ยังเหลือด้วย
        Try
            Dim dtSumQuan As Data.DataTable
            dtSumQuan = clsDB.getSumQuanTicketPortfolioByCust_id(cust_id)
            If dtSumQuan.Rows.Count > 0 Then
                Dim dr As Data.DataRow = dtSumQuan.Rows(0)
                Dim bid96 As String = dtSumQuan.Rows(0)("bid96").ToString
                Dim bid99 As String = dtSumQuan.Rows(0)("bid99").ToString
                Dim ask96 As String = dtSumQuan.Rows(0)("ask96").ToString
                Dim ask99 As String = dtSumQuan.Rows(0)("ask99").ToString

                Dim margin_base As Double = clsManage.convert2zero(dtSumQuan.Rows(0)("margin").ToString)
                Dim trade_limit As Double = clsManage.convert2zero(dtSumQuan.Rows(0)("trade_limit").ToString) * -1
                Dim free_margin As Double = clsManage.convert2zero(dtSumQuan.Rows(0)("free_margin").ToString)

                Dim sumAsset96 As Double = 0 : Dim sumAsset99 As Double = 0 : Dim sumCashCredit As Double = 0
                Dim sumquan96 As Double = 0 : Dim sumquan99 As Double = 0 : Dim sumAmount96 As Double = 0 : Dim sumAmount99 As Double = 0
               

                If dtSumQuan.Rows(0)("cash").ToString <> "" Then sumCashCredit = dtSumQuan.Rows(0)("cash").ToString
                If dtSumQuan.Rows(0)("gold96").ToString <> "" Then sumAsset96 = dtSumQuan.Rows(0)("gold96").ToString
                If dtSumQuan.Rows(0)("gold99").ToString <> "" Then sumAsset99 = dtSumQuan.Rows(0)("gold99").ToString

                If dtSumQuan.Rows(0)("quan96").ToString <> "" Then sumquan96 = dtSumQuan.Rows(0)("quan96").ToString
                If dtSumQuan.Rows(0)("quan99").ToString <> "" Then sumquan99 = dtSumQuan.Rows(0)("quan99").ToString
                If dtSumQuan.Rows(0)("amount96").ToString <> "" Then sumAmount96 = dtSumQuan.Rows(0)("amount96").ToString
                If dtSumQuan.Rows(0)("amount99").ToString <> "" Then sumAmount99 = dtSumQuan.Rows(0)("amount99").ToString

                'บวก  tran ที่ยังไม่ได้ accept ทั้งหมด
                Dim sumquan96_tran_buy As Double = 0 : Dim sumquan99_tran_buy As Double = 0 : Dim sumAmount96_tran_buy As Double = 0 : Dim sumAmount99_tran_buy As Double = 0
                Dim sumquan96_tran_sell As Double = 0 : Dim sumquan99_tran_sell As Double = 0 : Dim sumAmount96_tran_sell As Double = 0 : Dim sumAmount99_tran_sell As Double = 0
               
                Dim dtTran As New DataTable
                dtTran = clsMain.getTradeTranByCustId(cust_id)
                If dtTran.Rows.Count > 0 Then
                    Dim amountTran As Double = 0 : Dim quanTran As Double = 0
                    For Each drTran As DataRow In dtTran.Rows
                        If drTran("type").ToString = "sell" Then
                            If drTran("gold_type_id").ToString() = "96" Then
                                sumAmount96_tran_sell = sumAmount96_tran_sell + clsManage.convert2zero(drTran("amount").ToString)
                                sumquan96_tran_sell = sumquan96_tran_sell - clsManage.convert2zero(drTran("quantity").ToString)
                            Else
                                sumAmount99_tran_sell = sumAmount99_tran_sell + clsManage.convert2zero(drTran("amount").ToString)
                                sumquan99_tran_sell = sumquan99_tran_sell - clsManage.convert2zero(drTran("quantity").ToString)
                            End If
                        Else 'buy
                            If drTran("gold_type_id").ToString = "96" Then
                                sumAmount96_tran_buy = sumAmount96_tran_buy - clsManage.convert2zero(drTran("amount").ToString)
                                sumquan96_tran_buy = sumquan96_tran_buy + clsManage.convert2zero(drTran("quantity").ToString)
                            Else
                                sumAmount99_tran_buy = sumAmount99_tran_buy - clsManage.convert2zero(drTran("amount").ToString)
                                sumquan99_tran_buy = sumquan99_tran_buy + clsManage.convert2zero(drTran("quantity").ToString)
                            End If
                        End If
                    Next
                End If

                sumquan96_tran_buy += sumquan96
                sumquan96_tran_sell += sumquan96
                sumAmount96_tran_sell += sumAmount96
                sumAmount96_tran_buy += sumAmount96

                sumquan99_tran_buy += sumquan99
                sumquan99_tran_sell += sumquan99
                sumAmount99_tran_sell += sumAmount99
                sumAmount99_tran_buy += sumAmount99

                'For Ex/short No trading
                Dim sumQuan96_buy_noTrading As Double = sumquan96_tran_buy
                Dim sumQuan96_sell_noTrading As Double = sumquan96_tran_sell
                Dim sumAmount96_sell_noTrading As Double = sumAmount96_tran_sell
                Dim sumAmount96_buy_noTrading As Double = sumAmount96_tran_buy
                Dim sumQuan99_buy_noTrading As Double = sumquan99_tran_buy
                Dim sumQuan99_sell_noTrading As Double = sumquan99_tran_sell
                Dim sumAmount99_sell_noTrading As Double = sumAmount99_tran_sell
                Dim sumAmount99_buy_noTrading As Double = sumAmount99_tran_buy

                'บวก amount,quan ที่กำลังจะtrade เพิ่มเข้าไป
                'Tran + ที่จะtran
                If type = "sell" Then
                    If purity = "96" Then
                        sumquan96_tran_sell -= quantity
                        sumAmount96_tran_sell += amount
                    Else
                        sumquan99_tran_sell -= quantity
                        sumAmount99_tran_sell += amount
                    End If
                Else
                    If purity = "96" Then
                        sumquan96_tran_buy += quantity
                        sumAmount96_tran_buy -= amount
                    Else
                        sumquan99_tran_buy += quantity
                        sumAmount99_tran_buy -= amount
                    End If
                End If

                Dim cpf As clsManage.CustPortFolio = clsManage.cal_custPortFolio(sumCashCredit, sumAmount96_tran_buy, sumAmount99_tran_buy, sumAmount96_tran_sell, sumAmount99_tran_sell, _
                                               bid96, bid99, ask96, ask99, sumquan96_tran_buy, sumquan99_tran_buy, sumquan96_tran_sell, sumquan99_tran_sell, _
                                               sumAsset96, sumAsset99, margin_base, 0, free_margin)

                Dim cpf_pre As clsManage.CustPortFolio = clsManage.cal_custPortFolio(sumCashCredit, sumAmount96_buy_noTrading, sumAmount99_buy_noTrading, sumAmount96_sell_noTrading, sumAmount99_sell_noTrading, _
                                                  bid96, bid99, ask96, ask99, sumQuan96_buy_noTrading, sumQuan99_buy_noTrading, sumQuan96_sell_noTrading, sumQuan99_sell_noTrading, _
                                                  sumAsset96, sumAsset99, margin_base, 0, free_margin)
                'Priority Trade Limit
                '1 check trade limit >> if margin < trade limit >>วงเงินเต็ม
                '2 check margin >> if margin new < margin old >>วงเงินเต็ม
                '3 check margin 0% if > 0 >> เทรดได้
                '4 check ex/short+free_margin >=0 >> เทรดได้
                '5 check ex/short+free_margin if exใหม่ >= exเก่า เทรดได้

                Dim limitGap As Boolean = True
                If Not trade_limit = 0 Then
                    limitGap = False
                    If cpf.Ma1 >= trade_limit Then
                        limitGap = True
                    Else
                        If cpf.Ma1 >= cpf_pre.Ma1 Then
                            limitGap = True
                        End If
                    End If
                End If

                If limitGap Then
                    If margin_base = 0 Then
                        Return ""
                    End If

                    If cpf.ExFM1 >= 0 Then
                        Return ""
                    Else
                        'If cpf.ExFM1 >= cpf_pre.ExFM1 Then 'ถ้า ติดลบน้อยลงกว่าเดิมยอมให้ trade ได้ 
                       If cpf.Ma1 >= cpf_pre.Ma1 Then
                            Return ""
                        End If
                    End If
                Else
                    Return "วงเงินการซื้อขายเต็ม"
                End If
            End If
            Return "ไม่สามารถซื้อขายได้เนื่องจากหลักประกันไม่พอ"
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Shared Function getTradeTranByCustId(ByVal cust_id As String) As DataTable
        Try
            Dim sql As String = String.Format("select * from trade where  mode = 'tran' and cust_id = '{0}' And Convert(varchar, modifier_date, 111) = Convert(varchar, getdate(), 111)", cust_id)
            Dim dt As New DataTable
            Dim con As New SqlConnection(strcon)
            Dim da As New SqlDataAdapter(sql, con)

            da.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        End Try

    End Function
#End Region

#Region "Price"
    Public Shared Function getPriceOnline() As DataTable

        Dim sql As String = "select price,bid99_1,ask99_1,price,bid99_2,ask99_2,bid99_3,ask99_3,bid96_1,ask96_1,bid96_2,ask96_2,bid96_3,ask96_3,pwd_auth,time,DATEADD(second, time, time_update) as time_end,purity99_default,min,max,max_baht,max_kg,min_ask_dif_9699,max_ask_dif_9699,order_timeout,getdate() as getdate,max_per,halt,range_leave_order from stock_online"

        Dim con As New SqlConnection(strcon)
        Dim cmd As New SqlCommand(sql, con)
        Try

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable

            da.Fill(dt)
            Return dt

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getStockRangeLeaveOrder() As Double

        Dim sql As String = "select range_leave_order from stock_online"

        Dim con As New SqlConnection(strcon)
        Dim cmd As New SqlCommand(sql, con)
        Try
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable

            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Return clsManage.convert2zero(dt.Rows(0)(0).ToString)
            Else
                Return 0
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function UpdatePriceOnline(ByVal bid99_1 As String, ByVal ask99_1 As String, ByVal bid99_2 As String, ByVal ask99_2 As String, ByVal bid99_3 As String, ByVal ask99_3 As String, ByVal bid96_1 As String, ByVal ask96_1 As String, ByVal bid96_2 As String, ByVal ask96_2 As String, ByVal bid96_3 As String, ByVal ask96_3 As String, ByVal times As String, ByVal min As String, ByVal max As String, ByVal max_baht As String, ByVal max_kg As String, ByVal min_ask_dif_9699 As String, ByVal max_ask_dif_9699 As String, ByVal order_timeout As String, ByVal max_per As String, ByVal range_leave_order As String, ByVal self_price As String, ByVal modifier_by As String) As Integer

        Try
            Dim sql As String = String.Format("UPDATE stock_online SET ask99_1=@ask99_1, bid99_1=@bid99_1, ask99_2=@ask99_2, bid99_2=@bid99_2, ask99_3=@ask99_3, bid99_3=@bid99_3, " & _
                                                                     " ask96_1=@ask96_1, bid96_1=@bid96_1, ask96_2=@ask96_2, bid96_2=@bid96_2, ask96_3=@ask96_3, bid96_3=@bid96_3, " & _
                                                                     " time=@times, time_update=getdate(),min = @min,max=@max,max_baht=@max_baht,max_kg=@max_kg,order_timeout=@order_timeout, " & _
                                                                     " self_price = @self_price,max_per=@max_per,min_ask_dif_9699 = @min_ask_dif_9699,max_ask_dif_9699 = @max_ask_dif_9699,range_leave_order = @range_leave_order,modifier_by=@modifier_by where stock_id='1'")
            Using con As New SqlConnection(strcon)
                Using cmd As New SqlCommand(sql, con)

                    Dim parameter As New SqlParameter("@bid99_1", SqlDbType.Decimal)
                    parameter.Value = bid99_1
                    cmd.Parameters.Add(parameter)

                    parameter = New SqlParameter("@ask99_1", SqlDbType.Decimal)
                    parameter.Value = ask99_1
                    cmd.Parameters.Add(parameter)

                    parameter = New SqlParameter("@bid99_2", SqlDbType.Decimal)
                    parameter.Value = bid99_2
                    cmd.Parameters.Add(parameter)

                    parameter = New SqlParameter("@ask99_2", SqlDbType.Decimal)
                    parameter.Value = ask99_2
                    cmd.Parameters.Add(parameter)

                    parameter = New SqlParameter("@bid99_3", SqlDbType.Decimal)
                    parameter.Value = bid99_3
                    cmd.Parameters.Add(parameter)

                    parameter = New SqlParameter("@ask99_3", SqlDbType.Decimal)
                    parameter.Value = ask99_3
                    cmd.Parameters.Add(parameter)


                    parameter = New SqlParameter("@bid96_1", SqlDbType.Decimal)
                    parameter.Value = bid96_1
                    cmd.Parameters.Add(parameter)

                    parameter = New SqlParameter("@ask96_1", SqlDbType.Decimal)
                    parameter.Value = ask96_1
                    cmd.Parameters.Add(parameter)

                    parameter = New SqlParameter("@bid96_2", SqlDbType.Decimal)
                    parameter.Value = bid96_2
                    cmd.Parameters.Add(parameter)

                    parameter = New SqlParameter("@ask96_2", SqlDbType.Decimal)
                    parameter.Value = ask96_2
                    cmd.Parameters.Add(parameter)

                    parameter = New SqlParameter("@bid96_3", SqlDbType.Decimal)
                    parameter.Value = bid96_3
                    cmd.Parameters.Add(parameter)

                    parameter = New SqlParameter("@ask96_3", SqlDbType.Decimal)
                    parameter.Value = ask96_3
                    cmd.Parameters.Add(parameter)

                    parameter = New SqlParameter("@times", SqlDbType.Float)
                    parameter.Value = times
                    cmd.Parameters.Add(parameter)

                    'parameter = New SqlParameter("@purity99_default", SqlDbType.NChar, 3)
                    'parameter.Value = purity99_default
                    'cmd.Parameters.Add(parameter)

                    parameter = New SqlParameter("@min", SqlDbType.Decimal)
                    parameter.Value = min
                    cmd.Parameters.Add(parameter)

                    parameter = New SqlParameter("@max", SqlDbType.Decimal)
                    parameter.Value = max
                    cmd.Parameters.Add(parameter)

                    parameter = New SqlParameter("@max_baht", SqlDbType.Decimal)
                    parameter.Value = max_baht
                    cmd.Parameters.Add(parameter)

                    parameter = New SqlParameter("@max_kg", SqlDbType.Decimal)
                    parameter.Value = max_kg
                    cmd.Parameters.Add(parameter)

                    parameter = New SqlParameter("@min_ask_dif_9699", SqlDbType.Decimal)
                    parameter.Value = min_ask_dif_9699
                    cmd.Parameters.Add(parameter)

                    parameter = New SqlParameter("@max_ask_dif_9699", SqlDbType.Decimal)
                    parameter.Value = max_ask_dif_9699
                    cmd.Parameters.Add(parameter)

                    parameter = New SqlParameter("@order_timeout", SqlDbType.Int)
                    parameter.Value = clsManage.convert2zero(order_timeout)
                    cmd.Parameters.Add(parameter)

                    parameter = New SqlParameter("@max_per", SqlDbType.Decimal)
                    parameter.Value = max_per
                    cmd.Parameters.Add(parameter)

                    parameter = New SqlParameter("@range_leave_order", SqlDbType.Decimal)
                    parameter.Value = range_leave_order
                    cmd.Parameters.Add(parameter)

                    parameter = New SqlParameter("@self_price", SqlDbType.Char)
                    parameter.Value = self_price
                    cmd.Parameters.Add(parameter)

                    parameter = New SqlParameter("@modifier_by", SqlDbType.VarChar, 50)
                    parameter.Value = modifier_by
                    cmd.Parameters.Add(parameter)

                    cmd.CommandText = sql
                    con.Open()
                    Dim result As Integer = cmd.ExecuteNonQuery()
                    Return result

                End Using
            End Using

        Catch ex As Exception
            Throw ex
        Finally

        End Try

    End Function
#End Region

#Region "Customer"


    Public Shared Function haveTicketed(ByVal cust_id As String) As Boolean
        Dim sql As String = " SELECT * FROM ticket " & _
                            " Where cust_id  = @cust_id " + _
                            " AND status_id in (101) "

        Dim con As New SqlConnection(strcon)
        Dim cmd As New SqlCommand(sql, con)
        Try
            Dim parameter As New SqlParameter("@cust_id", SqlDbType.VarChar, 50)
            parameter.Value = cust_id
            cmd.Parameters.Add(parameter)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable

            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getCustomer() As DataTable

        Dim sql As String = "select * from customer order by Cust_id DESC"
        Dim con As New SqlConnection(strcon)
        Try
            Dim da As New SqlDataAdapter(sql, con)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Return dt
            Else
                Return Nothing
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getCustomerType() As DataTable

        Dim sql As String = "select * from cust_type"
        Dim con As New SqlConnection(strcon)
        Try
            Dim da As New SqlDataAdapter(sql, con)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Return dt
            Else
                Return Nothing
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function searchCustomer(ByVal str As String, Optional ByVal user_id As String = "") As DataTable
        Try
            Dim sql_condition As String = ""
            If user_id <> "" Then
                sql_condition = "WHERE active='y' and cust_id in (select distinct cust_id from tickets where created_by ='" + user_id + "')"
            End If


            Dim sql As String = "Select * from (select customer.*,cust_type.cust_type ,0.0 as revaluation,0.0 as netequity ,0.0 as excess " & _
            " FROM         customer LEFT OUTER JOIN " & _
            "          cust_type ON customer.cust_type_id = cust_type.cust_type_id " & _
            " WHERE (customer.cust_id = @cust_id or @cust_id = '') or " & _
            " (firstname like '%' + @firstname + '%' or @firstname='') or (lastname like '%' + @lastname + '%' or @lastname='') ) cust" & _
            " " + sql_condition + "" & _
            " order by 1 desc"
            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)

            Dim parameter As New SqlParameter("@cust_id", SqlDbType.VarChar, 5)
            parameter.Value = str
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@firstname", SqlDbType.VarChar, 50)
            parameter.Value = str
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@lastname", SqlDbType.VarChar, 50)
            parameter.Value = str
            cmd.Parameters.Add(parameter)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                'Dim dtFolio As New Data.DataTable
                'For Each dr As Data.DataRow In dt.Rows
                '    dtFolio = clsManage.getFolioListByCust_id(dr("cust_id").ToString)
                '    If dtFolio.Rows.Count > 0 Then
                '        dr("revaluation") = clsManage.convert2zero(dtFolio.Rows(0)("revaluation"))
                '        dr("netequity") = clsManage.convert2zero(dtFolio.Rows(0)("netequity"))
                '        dr("excess") = clsManage.convert2zero(dtFolio.Rows(0)("excess"))
                '    End If
                'Next

                Return dt
            Else
                Return Nothing
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getCustomer(ByVal str As String, Optional ByVal user_id As String = "", Optional ByVal team_id As String = "") As DataTable
        Try
            Dim sql_condition As String = ""
            If user_id <> "" Then
                sql_condition = "WHERE cust_id in (select distinct cust_id from tickets where created_by ='" + user_id + "')"
            End If

            Dim sql_team As String = ""
            If team_id <> "" Then
                If team_id = "none" Then
                    sql_team = " and (customer.team_id='') "
                Else
                    sql_team = " and (customer.team_id='" + team_id + "') "
                End If
            End If

            Dim sql As String = "Select * from (select customer.*,cust_type.cust_type ,0.0 as revaluation,0.0 as netequity ,0.0 as excess " & _
            " FROM         customer LEFT OUTER JOIN " & _
            "          cust_type ON customer.cust_type_id = cust_type.cust_type_id " & _
            " WHERE ((customer.cust_id = @cust_id or @cust_id = '') or " & _
            " (firstname like '%' + @firstname + '%' or @firstname='') or (lastname like '%' + @lastname + '%' or @lastname='')) " + sql_team + " " & _
            " ) cust" & _
            " " + sql_condition + "" & _
            " order by 1 desc"
            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)

            Dim parameter As New SqlParameter("@cust_id", SqlDbType.VarChar, 5)
            parameter.Value = str
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@firstname", SqlDbType.VarChar, 50)
            parameter.Value = str
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@lastname", SqlDbType.VarChar, 50)
            parameter.Value = str
            cmd.Parameters.Add(parameter)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Dim dtFolio As New Data.DataTable
                For Each dr As Data.DataRow In dt.Rows
                    dtFolio = clsManage.getFolioListByCust_id(dr("cust_id").ToString)
                    If dtFolio.Rows.Count > 0 Then
                        dr("revaluation") = clsManage.convert2zero(dtFolio.Rows(0)("revaluation"))
                        dr("netequity") = clsManage.convert2zero(dtFolio.Rows(0)("netequity"))
                        dr("excess") = clsManage.convert2zero(dtFolio.Rows(0)("excess"))
                    End If
                Next

                Return dt
            Else
                Return Nothing
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function searchCustomerAndFolio(ByVal str As String) As DataTable
        Try
            Dim sql As String = "SELECT * " & _
            " FROM customer " & _
" left outer join ( " & _
    " SELECT ticket.cust_id,sum(ticket.amount) as amount,sum(paid) as paid, sum(quantity) as sumQuan " & _
    " FROM         ticket left outer JOIN " & _
    " ( SELECT     ref_no, SUM(amount) AS paid " & _
    " 	FROM          ticket_split  GROUP BY ref_no " & _
    " ) AS ticket_split ON ticket.ref_no = ticket_split.ref_no where ticket.status_id in (101,103) group by cust_id " & _
" )credit on customer.cust_id = credit.cust_id cross join stock" & _
            " WHERE (customer.cust_id = @cust_id or @cust_id = '') or (firstname like '%' + @firstname + '%' or @firstname='') or (lastname like '%' + @lastname + '%' or @lastname='') order by 1 desc"
            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)

            Dim parameter As New SqlParameter("@cust_id", SqlDbType.VarChar, 5)
            parameter.Value = str
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@firstname", SqlDbType.VarChar, 50)
            parameter.Value = str
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@lastname", SqlDbType.VarChar, 50)
            parameter.Value = str
            cmd.Parameters.Add(parameter)


            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Return dt
            Else
                Return Nothing
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getCustomerByCust_id(ByVal cust_id As String, Optional ByVal cust_name As String = "") As DataTable

        Try
            If cust_id = "" And cust_name = "" Then Return Nothing
            Dim sql As String = "SELECT *  " & _
                        " FROM customer " & _
            " left outer join ( " & _
                " SELECT ticket.cust_id,sum(ticket.amount) as amount,sum(paid) as paid ,sum(quantity) as sumQuan" & _
                " FROM         ticket left outer JOIN " & _
                " ( SELECT     ref_no, SUM(amount) AS paid " & _
                " 	FROM          ticket_split  GROUP BY ref_no " & _
                " ) AS ticket_split ON ticket.ref_no = ticket_split.ref_no where ticket.status_id =101 group by cust_id " & _
            " )credit on customer.cust_id = credit.cust_id cross join stock " & _
                        " WHERE (customer.cust_id = @cust_id or @cust_id = '') and  (customer.firstname = @firstname or @firstname = '')"

            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)

            Dim parameter As New SqlParameter("@cust_id", SqlDbType.VarChar, 5)
            parameter.Value = cust_id
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@firstname", SqlDbType.VarChar, 50)
            parameter.Value = cust_name
            cmd.Parameters.Add(parameter)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Return dt
            Else
                Return Nothing
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getCustMargin(ByVal cust_id As String) As Boolean

        Dim sql As String = "SELECT margin  FROM customer where cust_id = '" + cust_id + "'"
        Dim con As New SqlConnection(strcon)
        Try
            Dim da As New SqlDataAdapter(sql, con)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                If dt.Rows(0)("margin").ToString = "0" Then
                    Return True
                End If
            End If
            Return False
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getSumAssetCustomer(ByVal cust_id As String) As DataTable
        If cust_id = "" Then Return Nothing

        Dim sql As String = String.Format(" select sumcash+cash_credit as cash,sum96+quan_96 as gold96,sum99+quan_99N as gold99 " + _
" from ( " + _
 " select sum(cash) as sumcash from ( " + _
  " select case when type = 'deposit' then amount when type = 'withdraw' then -(amount) else 0 end as cash " + _
        " from customer_trans " + _
  " where gold_type_id = '' and cust_id = {0} " + _
 " )  sumcash " + _
" )  sumcash  " + _
" CROSS JOIN ( " + _
 " select sum(g96) as sum96 from ( " + _
  " select case when type = 'deposit' then quantity when type = 'withdraw' then -(quantity) else 0 end as g96 " + _
        " from customer_trans " + _
  " where gold_type_id = '96' and cust_id = {0} " + _
 " ) g96 " + _
" ) sum96  " + _
" CROSS JOIN ( " + _
  " select sum(g99) as sum99 from ( " + _
  " select case when type = 'deposit' then quantity when type = 'withdraw' then -(quantity) else 0 end as g99 " + _
        " from customer_trans " + _
  " where gold_type_id = '99' and cust_id = {0} " + _
 " ) g99 " + _
" ) sum99 ,customer where customer.cust_id = {0}", cust_id)

        Dim con As New SqlConnection(strcon)
        Try
            Dim da As New SqlDataAdapter(sql, con)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Return dt
            Else
                Return Nothing
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getCustomerTran(ByVal cust_id As String, Optional ByVal transCash As String = "") As DataTable
        Dim sql_pure As String = ""
        If transCash <> "" Then
            sql_pure = " AND gold_type_id <> ''"
        Else
            sql_pure = " AND gold_type_id = ''"
        End If

        Dim sql As String = "SELECT * FROM customer_trans left outer join users on created_by = users.user_id where cust_id = '" & cust_id & "' " & sql_pure & " "
        Dim con As New SqlConnection(strcon)
        Try
            Dim da As New SqlDataAdapter(sql, con)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Return dt
            Else
                Return Nothing
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function checkCustomer(ByVal cust_id As String, ByVal cust_name As String) As String
        If cust_id = "" And cust_name = "" Then Return False
        Dim sql As String = ""
        If cust_id <> "" Then
            sql = "SELECT firstname FROM customer WHERE CAST(cust_id AS VARCHAR) = '" & cust_id & "' "
        Else
            sql = "SELECT firstname FROM customer WHERE CAST(firstname AS VARCHAR) = '" & cust_name & "' "
        End If

        Dim con As New SqlConnection(strcon)
        Try
            Dim da As New SqlDataAdapter(sql, con)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Return dt.Rows(0)(0).ToString
            End If
            Return ""
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function insertCustomer(ByVal cust_refno As String, ByVal cust_type_id As String, ByVal titlename As String, ByVal firstname As String, ByVal lastname As String, ByVal person_contact As String, ByVal bill_address As String, ByVal bank1 As String, ByVal account_no1 As String, ByVal account_name1 As String, ByVal account_type1 As String, ByVal account_branch1 As String, ByVal bank2 As String, ByVal account_no2 As String, ByVal account_name2 As String, ByVal account_type2 As String, ByVal account_branch2 As String, ByVal bank3 As String, ByVal account_no3 As String, ByVal account_name3 As String, ByVal account_type3 As String, ByVal account_branch3 As String, ByVal mobilePhone As String, ByVal tel As String, ByVal fax As String, ByVal cash_credit As String, ByVal remark As String, ByVal margin As String, ByVal margin_call As String, ByVal quan_96 As String, ByVal quan_99N As String, ByVal quan_99L As String, ByVal created_by As String, ByVal team_id As String) As String
        Try
            Dim con As New SqlConnection(strcon)
            con.Open()
            Dim tr As SqlTransaction = con.BeginTransaction
            Try

                Dim sql As String = "SELECT MAX(cust_id)+1 FROM customer"
                Dim cmd As New SqlCommand(sql, con)
                cmd.Transaction = tr
                Dim cust_id As String = cmd.ExecuteScalar.ToString
                If cust_id = "" Then cust_id = "10001"
                cmd = Nothing

                Dim dt_stock As New Data.DataTable
                dt_stock = getStockSumDeposit()
                Dim price As Double = clsManage.convert2zero(dt_stock.Rows(0)("priceDep"))
                Dim G96 As Double = clsManage.convert2zero(dt_stock.Rows(0)("G96Dep"))
                Dim G99N As Double = clsManage.convert2zero(dt_stock.Rows(0)("G99NDep"))
                Dim G99L As Double = clsManage.convert2zero(dt_stock.Rows(0)("G99LDep"))
                Dim cash_stock As Double = clsManage.convert2zero(dt_stock.Rows(0)("cash"))
                Dim trans_stock As Double = clsManage.convert2zero(dt_stock.Rows(0)("trans"))
                Dim cheq_stock As Double = clsManage.convert2zero(dt_stock.Rows(0)("cheq"))

                Dim sql_act As String = ""
                Dim sql_dep As String = ""
                Dim quan_dep As Double = 0
                Dim gold_type As String = ""
                If cash_credit <> 0 Then
                    sql_dep = String.Format(";INSERT INTO [customer_trans] ([cust_id], [datetime], [type], [ticket_refno], [gold_type_id], [quantity], [amount], [remark], [created_by], [created_date]) VALUES " & _
                                          "('{0}', getdate(), 'Deposit', '{1}', '{2}', {3}, {4}, 'Customer Deposit First.', '{5}', getdate())", cust_id, "", gold_type, quan_dep, cash_credit, created_by)
                End If
                If quan_96 <> "0" Then
                    gold_type = "96"
                    quan_dep = quan_96
                    sql_dep += String.Format(";INSERT INTO [customer_trans] ([cust_id], [datetime], [type], [ticket_refno], [gold_type_id], [quantity], [amount], [remark], [created_by], [created_date]) VALUES " & _
                                           "('{0}', getdate(), 'Deposit', '{1}', '{2}', {3}, {4}, 'Customer Deposit First.', '{5}', getdate())", cust_id, "", gold_type, quan_dep, "0", created_by)

                    G96 += quan_96
                    sql_act = String.Format(";INSERT INTO [actual] ([asset_id], [ref_no], [asset_type], [created_by], [datetime], [purity], [quantity], [amount], [status_id], [status_name], [type], [order_type], [price_base], [G96_base], [G99N_base], [G99L_base], [cust_id], [before_status_id], [note], [payment], [cash], [trans], [cheq]) " & _
                                                     "VALUES ('{0}', '{1}', '{2}', '{3}', getdate(), '{4}', {5}, {6}, '{7}', '{8}', '{9}', '{10}', {11}, {12}, {13}, {14}, '{15}','{16}','{17}','{18}',{19},{20},{21})" & _
                                                     "", "0", "", "Gold", created_by, gold_type, quan_96, "0", "999", "", "ฝากทอง", "D/W", price, G96, G99N, G99L, cust_id, "000", "ฝากทองในการลงทะเบียนครั้งแรก", "", cash_stock, trans_stock, cheq_stock)
                End If
                If quan_99N <> "0" Then
                    gold_type = "99N"
                    quan_dep = quan_99N
                    sql_dep += String.Format(";INSERT INTO [customer_trans] ([cust_id], [datetime], [type], [ticket_refno], [gold_type_id], [quantity], [amount], [remark], [created_by], [created_date]) VALUES " & _
                                           "('{0}', getdate(), 'Deposit', '{1}', '{2}', {3}, {4}, 'Customer Deposit First.', '{5}', getdate())", cust_id, "", gold_type, quan_dep, "0", created_by)
                    G99N += quan_99N
                    sql_act += String.Format(";INSERT INTO [actual] ([asset_id], [ref_no], [asset_type], [created_by], [datetime], [purity], [quantity], [amount], [status_id], [status_name], [type], [order_type], [price_base], [G96_base], [G99N_base], [G99L_base], [cust_id], [before_status_id], [note], [payment], [cash], [trans], [cheq]) " & _
                                                    "VALUES ('{0}', '{1}', '{2}', '{3}', getdate(), '{4}', {5}, {6}, '{7}', '{8}', '{9}', '{10}', {11}, {12}, {13}, {14}, '{15}','{16}','{17}','{18}',{19},{20},{21})" & _
                                                    "", "0", "", "Gold", created_by, gold_type, quan_99N, "0", "999", "", "ฝากทอง", "D/W", price, G96, G99N, G99L, cust_id, "000", "ฝากทองในการลงทะเบียนครั้งแรก", "", cash_stock, trans_stock, cheq_stock)
                End If
                If quan_99L <> "0" Then
                    gold_type = "99L"
                    quan_dep = quan_99L
                    sql_dep += String.Format(";INSERT INTO [customer_trans] ([cust_id], [datetime], [type], [ticket_refno], [gold_type_id], [quantity], [amount], [remark], [created_by], [created_date]) VALUES " & _
                                           "('{0}', getdate(), 'Deposit', '{1}', '{2}', {3}, {4}, 'Customer Deposit First.', '{5}', getdate())", cust_id, "", gold_type, quan_dep, "0", created_by)
                    G99L += quan_99L
                    sql_act += String.Format(";INSERT INTO [actual] ([asset_id], [ref_no], [asset_type], [created_by], [datetime], [purity], [quantity], [amount], [status_id], [status_name], [type], [order_type], [price_base], [G96_base], [G99N_base], [G99L_base], [cust_id], [before_status_id], [note], [payment], [cash], [trans], [cheq]) " & _
                                                    "VALUES ('{0}', '{1}', '{2}', '{3}', getdate(), '{4}', {5}, {6}, '{7}', '{8}', '{9}', '{10}', {11}, {12}, {13}, {14}, '{15}','{16}','{17}','{18}',{19},{20},{21})" & _
                                                    "", "0", "", "Gold", created_by, gold_type, quan_99L, "0", "999", "", "ฝากทอง", "D/W", price, G96, G99N, G99L, cust_id, "000", "ฝากทองในการลงทะเบียนครั้งแรก", "", cash_stock, trans_stock, cheq_stock)
                End If

                sql = "INSERT INTO [customer] ([cust_id], [cust_refno], [cust_type_id], [titlename], [firstname], [lastname], [person_contact], [bill_address], [bank1], [account_no1], [account_name1], [account_type1], [account_branch1], [bank2], [account_no2], [account_name2], [account_type2], [account_branch2], [bank3], [account_no3], [account_name3], [account_type3], [account_branch3], [mobile], [tel], [fax], [remark], [cash_credit], [margin], [margin_call], [quan_96], [quan_99N], [quan_99L], [created_by], [created_date], [team_id]) VALUES (@cust_id, @cust_refno, @cust_type_id, @titlename, @firstname, @lastname, @person_contact, @bill_address, @bank1, @account_no1, @account_name1, @account_type1, @account_branch1, @bank2, @account_no2, @account_name2, @account_type2, @account_branch2, @bank3, @account_no3, @account_name3, @account_type3, @account_branch3, @mobile, @tel, @fax, @remark, @cash_credit, @margin, @margin_call, @quan_96, @quan_99N, @quan_99L,@created_by,getdate(),@team_id)"

                cmd = New SqlCommand(sql + sql_dep + sql_act, con)

                Dim parameter As New SqlParameter("@cust_id", SqlDbType.VarChar, 5)
                parameter.Value = cust_id
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@cust_refno", SqlDbType.VarChar, 50)
                parameter.Value = cust_refno
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@cust_type_id", SqlDbType.Int)
                parameter.Value = cust_type_id
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@titlename", SqlDbType.VarChar, 50)
                parameter.Value = titlename
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@firstname", SqlDbType.VarChar, 50)
                parameter.Value = firstname
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@lastname", SqlDbType.VarChar, 50)
                parameter.Value = lastname
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@person_contact", SqlDbType.VarChar, 50)
                parameter.Value = person_contact
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@bill_address", SqlDbType.VarChar, 500)
                parameter.Value = bill_address
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@bank1", SqlDbType.VarChar, 50)
                parameter.Value = bank1
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@account_no1", SqlDbType.VarChar, 50)
                parameter.Value = account_no1
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@account_name1", SqlDbType.VarChar, 50)
                parameter.Value = account_name1
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@account_type1", SqlDbType.VarChar, 50)
                parameter.Value = account_type1
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@account_branch1", SqlDbType.VarChar, 50)
                parameter.Value = account_branch1
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@bank2", SqlDbType.VarChar, 50)
                parameter.Value = bank2
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@account_no2", SqlDbType.VarChar, 50)
                parameter.Value = account_no2
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@account_name2", SqlDbType.VarChar, 50)
                parameter.Value = account_name2
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@account_type2", SqlDbType.VarChar, 50)
                parameter.Value = account_type2
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@account_branch2", SqlDbType.VarChar, 50)
                parameter.Value = account_branch2
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@bank3", SqlDbType.VarChar, 50)
                parameter.Value = bank3
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@account_no3", SqlDbType.VarChar, 50)
                parameter.Value = account_no3
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@account_name3", SqlDbType.VarChar, 50)
                parameter.Value = account_name3
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@account_type3", SqlDbType.VarChar, 50)
                parameter.Value = account_type3
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@account_branch3", SqlDbType.VarChar, 50)
                parameter.Value = account_branch3
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@mobile", SqlDbType.VarChar, 50)
                parameter.Value = mobilePhone
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@tel", SqlDbType.VarChar, 50)
                parameter.Value = tel
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@fax", SqlDbType.VarChar, 50)
                parameter.Value = fax
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@remark", SqlDbType.VarChar)
                parameter.Value = remark
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@cash_credit", SqlDbType.Decimal)
                parameter.Value = clsManage.convert2zero(cash_credit)
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@margin", SqlDbType.Float)
                parameter.Value = margin
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@margin_call", SqlDbType.Float)
                parameter.Value = margin_call
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@quan_96", SqlDbType.Decimal)
                parameter.Value = clsManage.convert2zero(quan_96)
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@quan_99N", SqlDbType.Decimal)
                parameter.Value = clsManage.convert2zero(quan_99N)
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@quan_99L", SqlDbType.Decimal)
                parameter.Value = clsManage.convert2zero(quan_99L)
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@created_by", SqlDbType.VarChar, 50)
                parameter.Value = fax
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@team_id", SqlDbType.VarChar, 50)
                parameter.Value = team_id
                cmd.Parameters.Add(parameter)

                cmd.Transaction = tr
                Dim result2 As Integer = cmd.ExecuteNonQuery()
                If result2 > 0 Then

                    tr.Commit()
                    Return cust_id

                End If
                Return ""
            Catch ex As Exception
                tr.Rollback()
                Throw ex
            Finally
                con.Close()
            End Try
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getStockDepositPre(ByVal purity As String) As DataTable
        Try
            Dim sql As String = ""
            If purity = "" Then
                purity = " deposit.gold_type_id = '' "
            Else
                purity = String.Format(" deposit.gold_type_id in ('{0}')", purity)
            End If

            sql = String.Format(" select * from (  " & _
" SELECT     customer_trans.cust_tran_id, customer_trans.cust_id, customer_trans.datetime, customer_trans.type, " & _
" customer_trans.gold_type_id, customer_trans.remark, customer_trans.created_by, customer_trans.created_date, " & _
" customer_trans.ticket_refno, users.user_name, customer.firstname,customer_trans.pre, " & _
" case when type = 'Deposit' then customer_trans.amount when type='Withdraw' then -(customer_trans.amount) else 0 end as amount," & _
" case when type = 'Deposit' then customer_trans.quantity when type='Withdraw' then -(customer_trans.quantity) else 0 end as quantity" & _
" FROM         customer_trans LEFT OUTER JOIN " & _
" customer ON customer_trans.cust_id = customer.cust_id LEFT OUTER JOIN " & _
" users ON customer_trans.created_by = users.user_id " & _
" )deposit" & _
" Where {0} order by deposit.created_date desc ", purity)

            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Return dt
            Else
                Return Nothing
            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Shared Function getStockDeposit(ByVal team_id As String) As DataTable
        Try
            Dim sql As String = ""
            Dim sql_team As String = ""
            'If team_id <> "" Then
            '    'sql_team = String.Format(" WHERE sum_trans.cust_id in (select distinct cust_id from tickets where created_by in (select user_id from users where team_id = '{0}')) ", team_id)
            'End If
            If team_id <> "" Then
                If team_id = "none" Then
                    sql_team = "WHERE (customer.team_id = '' or customer.team_id is null) "
                Else
                    sql_team = "WHERE (customer.team_id = " + team_id + ") "
                End If
            End If

            sql = "" & _
"   select isnull(cash,0) as cash,isnull(g96,0) as g96,isnull(g99N,0) as g99N,isnull(g99L,0) as g99L,customer.cust_id,firstname from (" & _
" 	select cust_id," & _
"   sum( case when (type = 'deposit' and gold_type_id ='')  then amount when (type = 'withdraw'  and gold_type_id ='') then -(amount) else 0 end) as cash," & _
" 	sum( case when (type = 'deposit' and gold_type_id ='96')  then quantity when (type = 'withdraw'  and gold_type_id ='96') then -(quantity) else 0 end) as g96," & _
" 	sum( case when (type = 'deposit' and gold_type_id ='99N')  then quantity when (type = 'withdraw'  and gold_type_id ='99N') then -(quantity) else 0 end) as g99N," & _
" 	sum( case when (type = 'deposit' and gold_type_id ='99L')  then quantity when (type = 'withdraw'  and gold_type_id ='99L') then -(quantity) else 0 end) as g99L" & _
" 	from customer_trans	group by cust_id" & _
"   )sum_trans right join customer on sum_trans.cust_id = customer.cust_id " & _
"" & sql_team & "" & _
"   order by cust_id desc"

            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Return dt
            Else
                Return Nothing
            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Shared Function updateDepositPre(ByVal id As String) As Boolean
        Try
            Dim sql As String = "UPDATE customer_trans set pre = 'n' WHERE cust_tran_id = '" & id & "' "
            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)
            con.Open()
            Dim result As Integer = cmd.ExecuteNonQuery()
            con.Close()
            If result > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function updatePaidCheq(ByVal id As String) As Boolean
        Try
            Dim sql As String = "UPDATE ticket_split set paid = 1 WHERE ticket_sp_id = '" & id & "' "
            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)
            con.Open()
            Dim result As Integer = cmd.ExecuteNonQuery()
            con.Close()
            If result > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function updateDopositWithdraw(ByVal id As String, ByVal amount As Double, ByVal quan As Double) As Boolean
        Try
            Dim sql As String = String.Format("UPDATE customer_trans set amount = {1},quantity = {2} WHERE cust_tran_id = '{0}' ", id, amount, quan)

            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)
            con.Open()
            Dim result As Integer = cmd.ExecuteNonQuery()
            con.Close()
            If result > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function checkCustomerBeforeDel(ByVal cust_id As String) As DataTable
        Try
            'Dim sql As String = "select * from tickets WHERE cust_id = '" & cust_id & "' "
            Dim sql As String = String.Format("select * from tickets WHERE cust_id = '{0}'  and status_id = 101 ", cust_id)

            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            Return dt

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    'Public Shared Function deleteCustomer(ByVal id As String) As Boolean
    '    Try
    '        Dim sql As String = "UPDATE customer set active = 'n' WHERE cust_id = '" & id & "' "
    '        Dim con As New SqlConnection(strcon)
    '        Dim cmd As New SqlCommand(sql, con)
    '        con.Open()
    '        Dim result As Integer = cmd.ExecuteNonQuery()
    '        con.Close()
    '        If result > 0 Then
    '            Return True
    '        Else
    '            Return False
    '        End If
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function
#End Region

    Public Shared Function getSumQuanTicketPortfolioByCust_id(ByVal cust_id As String) As DataTable
        Dim dt As New DataTable
        Try
            Dim sql As String = String.Format("[getSumPortfolioByCust_id] '{0}'", cust_id)
            Using da As New SqlDataAdapter(sql, strcon)
                da.Fill(dt)
                If dt.Rows.Count > 0 AndAlso dt.Rows(0)("self_price").ToString = clsManage.no Then
                    Dim sto As New clsStore()
                    sto = clsStore.getPriceStore(cust_id)
                    dt.Rows(0)("bid99") = sto.bid99
                    dt.Rows(0)("ask99") = sto.ask99
                    dt.Rows(0)("bid96") = sto.bid96
                    dt.Rows(0)("ask96") = sto.ask96
                End If
                Return dt
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            dt.Dispose()
        End Try
    End Function

#Region "kate"
    Public Shared Function getTicketPortfolioByCust_id(ByVal cust_id As String, ByVal gold_type As String, ByVal isHistory As String) As DataTable
        If cust_id Is Nothing Then Return Nothing
        Dim sql_gold_type As String = ""
        If gold_type <> "" Then
            If gold_type = "99" Then
                sql_gold_type = " AND (ticket.gold_type_id <> '96') "
            Else
                sql_gold_type = " AND (ticket.gold_type_id = '96') "
            End If
        End If

        'Dim sql_status As String = ""
        'If isHistory = "true" Then
        '    sql_status = "AND status_id in (901,902,903) "
        'Else
        '    sql_status = "AND status_id in (101,102,103) "
        'End If

        'Dim sql As String = " SELECT ref_no as ticket_id,ticket_date,book_no,run_no," + _
        '                    " case when type='sell' then 'ซื้อ' when type='buy' then 'ขาย' else '' end as type,created_date,price, " + _
        '                    " case when type='sell' then quantity when type='buy' then quantity*-1 else 0 end as quantity ," + _
        '                    " case when type='sell' then amount * -1 when type='buy' then amount else 0 end as amount, " + _
        '                    " payment_duedate,delivery_date" & _
        '                    " FROM ticket " & _
        '                    " Where cust_id  = @cust_id " + _
        '                    " " + sql_gold_type + " " & _
        '                    " " + sql_status + " " & _
        '                    " ORDER BY created_date desc,type desc"

        Dim sql_status As String = ""
        Dim sql As String = ""

        If isHistory = "true" Then

            sql_status = "AND status_id in (901,902,903) "

            ' for history ticket have split
            'Dim sql_split As String = " UNION select ref_no as ticket_id,ticket_date,book_no,run_no,case when type='sell' then 'ซื้อ' when type='buy' then 'ขาย' else '' end as type,created_date,price, " & _
            '                        " case when type='sell' then sp_quan when type='buy' then sp_quan*-1 else 0 end as quantity ," + _
            '                        " case when type='sell' then sp_amount * -1 when type='buy' then sp_amount else 0 end as amount, " + _
            '                        " payment_duedate,delivery_date" & _
            '                        " where status_id not in (901,902,903,801) and sp_quan is not null  and cust_id  = @cust_id" + sql_gold_type
            Dim sql_split As String = " UNION SELECT     ticket.ref_no AS ticket_id, ticket.ticket_date, ticket.book_no, ticket.run_no, " & _
" CASE WHEN ticket.type = 'sell' THEN 'ซื้อ' WHEN ticket.type = 'buy' THEN 'ขาย' ELSE '' END AS type, ticket.created_date, ticket.price, " & _
" CASE WHEN ticket.type = 'sell' THEN sp_quan WHEN ticket.type = 'buy' THEN sp_quan * - 1 ELSE 0 END AS quantity, " & _
" CASE WHEN ticket.type = 'sell' THEN sp_amount * - 1 WHEN ticket.type = 'buy' THEN sp_amount ELSE 0 END AS amount, " & _
" CASE when ticket.payment_duedate is not null then ticket.payment_duedate else ticket_split.created_date end as payment_duedate, " & _
" ticket_split.created_date as delivery_date " & _
" FROM         v_ticket_sum_split AS ticket LEFT OUTER JOIN " & _
" ticket_split ON ticket.ref_no = ticket_split.ref_no and ticket_split.row = 1 and ticket_split.type= 'Split' " & _
" where ticket.status_id not in (901,902,903,801) and ticket.sp_quan is not null  and ticket.cust_id  = @cust_id" + sql_gold_type


            sql = " SELECT * FROM ( " & _
                                    " SELECT ref_no as ticket_id,ticket_date,book_no,run_no," + _
                                    " case when type='sell' then 'ซื้อ' when type='buy' then 'ขาย' else '' end as type,created_date,price, " + _
                                    " case when type='sell' then quantity when type='buy' then quantity*-1 else 0 end as quantity ," + _
                                    " case when type='sell' then amount * -1 when type='buy' then amount else 0 end as amount, " + _
                                    " case when payment_duedate is not null then payment_duedate else modifier_date end as payment_duedate ,delivery_date" & _
                                    " FROM ticket " & _
                                    " Where cust_id  = @cust_id AND status_id in (901,902,903) " + sql_gold_type + _
                                      " " + sql_split + " " & _
                                    " ) ticket ORDER BY created_date desc,type desc"
        Else
            sql_status = "AND status_id not in (901,902,903,801)"
            sql = " SELECT ref_no as ticket_id,ticket_date,book_no,run_no," + _
                                      " case when type='sell' then 'ซื้อ' when type='buy' then 'ขาย' else '' end as type,created_date,price, " + _
                                      " case when type='sell' then quantity when type='buy' then quantity*-1 else 0 end as quantity ," + _
                                      " case when type='sell' then amount * -1 when type='buy' then amount else 0 end as amount, " + _
                                      " payment_duedate,delivery_date" & _
                                      " FROM v_ticket_sum_split ticket " & _
                                      " Where cust_id  = @cust_id " + _
                                      " " + sql_gold_type + " " & _
                                      " " + sql_status + " " & _
                                      " ORDER BY created_date desc,type desc"
        End If



        Dim con As New SqlConnection(strcon)
        Dim cmd As New SqlCommand(sql, con)
        Try
            Dim parameter As New SqlParameter("@cust_id", SqlDbType.VarChar, 50)
            parameter.Value = cust_id
            cmd.Parameters.Add(parameter)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable

            da.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getCashCreditPortfolioByCust_id(ByVal cust_id As String, ByVal type As String) As DataTable
        Try
            Dim sql As String = ""
            If type = "cash" Then
                sql = String.Format(" select  ref_no, datetime, case when type = 'deposit' then amount else 0 end as deposit, " & _
                " case when type = 'withdraw' then amount else 0 end as withdraw" & _
                " from customer_trans " & _
                " where gold_type_id = '' and cust_id = '{0}'", cust_id)

                'ไม่มีฝากเงินไว้เฉยๆแล้ว (buy)
                '" UNION SELECT ticket_split.ref_no, null as datetime, SUM(ticket_split.amount) AS deposit,0 as withdraw" & _
                '" FROM  ticket_split INNER JOIN ticket ON ticket_split.ref_no = ticket.ref_no " & _
                '" WHERE ticket_split.type = 'DepositCash' and ticket.cust_id = '{0}' " & _
                '" GROUP BY ticket_split.ref_no, ticket.cust_id ", cust_id)
            ElseIf type = "gold" Then
                sql = String.Format(" select ticket_refno as ref_no, datetime, case when type = 'deposit' then quantity else 0 end as deposit, " & _
                " case when type = 'withdraw' then quantity else 0 end as withdraw,gold_type_id from customer_trans   " & _
                " where gold_type_id <> '' and cust_id = '{0}'  " & _
                " UNION ALL SELECT ticket_split.ref_no,   max(ticket_split.created_date)  as datetime, SUM(ticket_split.quantity) AS deposit,0 as withdraw,gold_type_id  FROM  ticket_split  " & _
                " INNER JOIN ticket ON ticket_split.ref_no = ticket.ref_no   " & _
                " WHERE ticket_split.type = 'DepositGold' and ticket.cust_id = '{0}' and ticket.status_id <> '801' " & _
                " GROUP BY ticket_split.ref_no, ticket.cust_id,gold_type_id ", cust_id)
            End If

            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable

            da.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Shared Function getTradeByAdminNoTran(ByVal mode As String, ByVal type As String, ByVal purity As String, ByVal cust_id As String, ByVal max_trade_id As String, ByVal period As String, Optional ByVal pDate1 As DateTime = Nothing, Optional ByVal pDate2 As DateTime = Nothing, Optional ByVal onlyLeave As String = "n") As DataTable

        Dim dt As New DataTable
        Dim con As New SqlConnection()
        Dim cmd As New SqlCommand()
        Dim da As New SqlDataAdapter
        Try
            Dim sql_period As String = ""
            Dim period1 As String = ""
            Dim period2 As String = ""
            If period = "" Then
                sql_period = " AND (convert(datetime,tc.modifier_date,111) between convert(datetime,@period1,111) and getdate()) "
            Else
                sql_period = " AND (tc.modifier_date between @period1 and @period2) "
            End If

            Dim sql_cust As String = ""
            If cust_id <> "" Then
                sql_cust = "AND (cu.cust_id = '" & cust_id & "')"
            End If

            Dim sql_purity As String = ""
            If purity <> "" Then
                sql_purity = " and (tc.gold_type_id in ('" + purity + "')) "
            End If

            Dim sql_max_trade_id As String = ""
            If mode = "tran" Then
                sql_max_trade_id = "'n' as new_trade "
            Else
                sql_max_trade_id = "case when convert(varchar,tc.modifier_date,9) > (select convert(varchar,log_datetime,9) from log_" + mode + " where log_id = '" + max_trade_id + "') then 'y' else 'n' end as new_trade"
            End If

            Dim sql_mode As String = ""
            If mode = "tran" And cust_id <> "" Then
                sql_mode = ""
            Else
                sql_mode = " AND tc.mode=@mode"
            End If

            Dim sql_leave As String = ""
            If onlyLeave = "y" Then
                sql_leave = " AND leave_order = 'y'"
            Else
                If mode = "tran" And cust_id <> "" Then
                    sql_leave = " AND leave_order = 'n'"
                End If
            End If
            Dim sql_orderby As String = "order by modifier_date desc,created_date desc"
            Dim sql As String = " select trade_id,ref_no,case leave_order when 'n' then 'order' when 'y' then 'leave order' else '' end as leave_order, " & _
                                " tc.modifier_date,tc.created_date,type,firstname,ip,gold_type_id,price,quantity,amount,mode,reject_type,cust_level, " & _
                                " " + sql_max_trade_id + " " & _
                                " ,case when gold_type_id= '96' then price/.965 else price end as price_compare " & _
                                " from trade tc,customer cu,usernames un " & _
                                " where tc.cust_id = cu.cust_id and cu.cust_id = un.cust_id " & _
                                " AND (type =  @type or @type = '' ) " & _
                                " " + sql_leave + " " & _
                                " " + sql_period + " " & _
                                " " + sql_mode + " " & _
                                " " + sql_cust + " " & _
                                " " + sql_purity + " " & _
                                " " + sql_orderby + ""

            con = New SqlConnection(strcon)
            cmd = New SqlCommand(sql, con)

            Dim parameter As New SqlParameter("@type", SqlDbType.VarChar, 5)
            parameter.Value = type
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@mode", SqlDbType.VarChar, 10)
            parameter.Value = mode
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@period1", SqlDbType.DateTime)
            parameter.Value = pDate1
            cmd.Parameters.Add(parameter)

            If period <> "" Then
                parameter = New SqlParameter("@period2", SqlDbType.DateTime)
                parameter.Value = pDate2
                cmd.Parameters.Add(parameter)
            End If

            da = New SqlDataAdapter(cmd)
            da.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
            con.Dispose()
            cmd.Dispose()
            da.Dispose()
            dt = Nothing
        End Try
    End Function

    Public Shared Function getTradeByMode(ByVal mode As String, ByVal type As String, ByVal purity As String, ByVal cust_id As String, ByVal max_trade_id As String, ByVal sortPrice As String, ByVal period As String, Optional ByVal pDate1 As DateTime = Nothing, Optional ByVal pDate2 As DateTime = Nothing, Optional ByVal onlyLeave As String = "n") As DataTable
        If mode = "" Then Return Nothing
        Dim dt As New DataTable
        Dim con As New SqlConnection()
        Dim cmd As New SqlCommand()
        Dim da As New SqlDataAdapter
        Try
            Dim sql_period As String = ""
            Dim period1 As String = ""
            Dim period2 As String = ""
            If period = "" Then
                'sql_period = " AND convert(varchar,tc.modifier_date,111)=convert(varchar,getdate(),111) "
                sql_period = " AND (convert(datetime,tc.modifier_date,111) between convert(datetime,@period1,111) and getdate()) "
            Else
                sql_period = " AND (tc.modifier_date between @period1 and @period2) "
            End If

            Dim sql_cust As String = ""
            If cust_id <> "" Then
                sql_cust = "AND (cu.cust_id = '" & cust_id & "')"
            End If

            Dim sql_purity As String = ""
            If purity <> "" Then
                sql_purity = " and (tc.gold_type_id in ('" + purity + "')) "
            End If

            Dim sql_max_trade_id As String = ""
            If mode = "tran" Then
                sql_max_trade_id = "'n' as new_trade "
            Else
                sql_max_trade_id = "case when convert(varchar,tc.modifier_date,9) > (select convert(varchar,log_datetime,9) from log_" + mode + " where log_id = '" + max_trade_id + "') then 'y' else 'n' end as new_trade"
            End If

            Dim sql_mode As String = ""
            If mode = "tran" And cust_id <> "" Then
                sql_mode = ""
            Else
                sql_mode = " AND tc.mode=@mode"
            End If

            Dim sql_leave As String = ""
            If onlyLeave = "y" Then
                sql_leave = " AND leave_order = 'y'"
            Else
                If mode = "tran" And cust_id <> "" Then
                    sql_leave = " AND leave_order = 'n'"
                End If
            End If

            Dim sql_orderby As String = ""
            Dim sortField As String = ""
            If sortPrice = "y" Then
                sortField = "price_compare"
            Else
                sortField = "price"
            End If
            If cust_id = "" Then ' admin
                If type = "sell" Then
                    sql_orderby = "order by " & sortField & " desc, modifier_date desc,created_date desc"
                Else
                    sql_orderby = "order by " & sortField & " desc, modifier_date asc,created_date asc"
                End If
            Else 'customer
                If mode = clsManage.tradeMode.tran.ToString Then
                    sql_orderby = "order by created_date desc" 'customer mode tran ให้อยู่ตำแหน่งเดิม
                Else
                    sql_orderby = "order by modifier_date desc,created_date desc"
                End If
            End If

            Dim sql As String = " select trade_id,ref_no,case leave_order when 'n' then 'order' when 'y' then 'leave order' else '' end as leave_order, " & _
                                " tc.modifier_date,tc.created_date,type,firstname,ip,gold_type_id,price,quantity,amount,mode,reject_type,cust_level, " & _
                                " " + sql_max_trade_id + " " & _
                                " ,case when gold_type_id= '96' then price/.965 else price end as price_compare " & _
                                " from trade tc,customer cu,usernames un " & _
                                " where tc.cust_id = cu.cust_id and cu.cust_id = un.cust_id " & _
                                " AND (type =  @type or @type = '' ) " & _
                                " " + sql_leave + " " & _
                                " " + sql_period + " " & _
                                " " + sql_mode + " " & _
                                " " + sql_cust + " " & _
                                " " + sql_purity + " " & _
                                " " + sql_orderby + ""



            con = New SqlConnection(strcon)
            cmd = New SqlCommand(sql, con)

            Dim parameter As New SqlParameter("@type", SqlDbType.VarChar, 5)
            parameter.Value = type
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@mode", SqlDbType.VarChar, 10)
            parameter.Value = mode
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@period1", SqlDbType.DateTime)
            parameter.Value = pDate1
            cmd.Parameters.Add(parameter)

            If period <> "" Then
                parameter = New SqlParameter("@period2", SqlDbType.DateTime)
                parameter.Value = pDate2
                cmd.Parameters.Add(parameter)
            End If

            da = New SqlDataAdapter(cmd)
            da.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
            con.Dispose()
            cmd.Dispose()
            da.Dispose()
            dt = Nothing
            GC.WaitForPendingFinalizers()

        End Try
    End Function

    Public Shared Function getTradeByTran() As DataTable

        Dim dt As New DataTable
        Dim con As New SqlConnection()
        Dim cmd As New SqlCommand()
        Dim da As New SqlDataAdapter
        Try
            Dim ran As Integer = New Random().Next(5, 20)

            Dim sql As String = " select top " & ran.ToString & " trade_id,ref_no,case leave_order when 'n' then 'order' when 'y' then 'leave order' else '' end as leave_order, " & _
                                " tc.modifier_date,tc.created_date,type,firstname,ip,gold_type_id,price,quantity,amount,mode,reject_type,cust_level, " & _
                                " 'n' as new_trade " & _
                                " ,case when gold_type_id= '96' then price/.965 else price end as price_compare " & _
                                " from trade tc,customer cu,usernames un " & _
                                " where tc.cust_id = cu.cust_id and cu.cust_id = un.cust_id " & _
                                " AND (type =  @type or @type = '' ) "
                                


            con = New SqlConnection(strcon)
            cmd = New SqlCommand(sql, con)

            Dim parameter As New SqlParameter("@type", SqlDbType.VarChar, 5)
            parameter.Value = ""
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@mode", SqlDbType.VarChar, 10)
            parameter.Value = "tran"
            cmd.Parameters.Add(parameter)

           

            da = New SqlDataAdapter(cmd)
            da.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
            con.Dispose()
            cmd.Dispose()
            da.Dispose()
            dt = Nothing
            GC.WaitForPendingFinalizers()

        End Try
    End Function

    Public Shared Function getTradeBlotter(ByVal mode As String, ByVal order As String, ByVal type As String, ByVal purity As String, ByVal cust_id As String, ByVal period As String, Optional ByVal pDate1 As DateTime = Nothing, Optional ByVal pDate2 As DateTime = Nothing) As DataTable
        Try
            If cust_id Is Nothing Then Return Nothing
            Dim sql_period As String = ""
            Dim period1 As String = ""
            Dim period2 As String = ""
            If period = "" Then
                sql_period = " AND convert(varchar,tc.modifier_date,111)=convert(varchar,getdate(),111) "
            Else
                sql_period = " AND (modifier_date between @period1 and @period2) "
            End If

            Dim sql_cust As String = ""
            If cust_id <> "" Then
                sql_cust = "AND (cu.cust_id = '" & cust_id & "')"
            End If

            Dim sql_purity As String = ""
            If purity <> "" Then
                sql_purity = " and (tc.gold_type_id in ('" + purity + "')) "
            End If

            Dim sql_mode As String = ""
            If mode <> "" Then
                sql_mode = " and (tc.mode in ('" + mode + "')) "
            End If

            Dim sql As String = " select trade_id,ref_no,case leave_order when 'n' then 'order' when 'y' then 'leave order' else '' end as leave_order, " & _
                                " tc.modifier_date,tc.created_date,type,firstname,ip,gold_type_id,price,quantity,amount,mode,reject_type, " & _
                                " case when gold_type_id= '96' then price/.965 else price end as price_compare " & _
                                " from trade tc,customer cu " & _
                                " where tc.cust_id = cu.cust_id " & _
                                " AND (type =  @type or @type = '' ) " & _
                                " AND (leave_order =  @leave_order or @leave_order = '' ) " & _
                                " " + sql_cust + " " & _
                                " " + sql_purity + " " & _
                                " " + sql_mode + " " & _
                                " " + sql_period + " " & _
                                " order by created_date desc"
            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)

            Dim parameter As New SqlParameter("@type", SqlDbType.VarChar, 5)
            parameter.Value = type
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@mode", SqlDbType.VarChar, 10)
            parameter.Value = mode
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@leave_order", SqlDbType.VarChar, 1)
            parameter.Value = order
            cmd.Parameters.Add(parameter)

            If period <> "" Then
                parameter = New SqlParameter("@period1", SqlDbType.DateTime)
                parameter.Value = pDate1
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@period2", SqlDbType.DateTime)
                parameter.Value = pDate2
                cmd.Parameters.Add(parameter)
            End If

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable

            da.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Shared Function getTicketPortfolioByMode99(ByVal mode As String, Optional ByVal cust_id As String = "") As DataTable
        Try

            Dim sql_cust As String = ""
            If cust_id <> "" Then
                sql_cust += " and tc.cust_id = '" + cust_id + "'"
            End If
            Dim sql As String = String.Format("select trade_id,ref_no,case leave_order when 'n' then 'order' when 'y' then 'leave order' else '' end as leave_order,tc.created_date,type,firstname,ip,gold_type_id,price,quantity,amount " & _
                               "from trade tc,customer cu " & _
                               "where tc.cust_id = cu.cust_id and tc.mode='{0}' {1} order by trade_id desc", mode, sql_cust)
            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable

            da.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Shared Function getTicketPortfolioByMode96(ByVal mode As String, Optional ByVal cust_id As String = "") As DataTable
        Try
            Dim sql As String = ""
            sql = String.Format("select trade_id,ref_no,case leave_order when 'n' then 'order' when 'y' then 'leave order' else '' end as leave_order,tc.created_date,type,firstname,ip,gold_type_id,price,quantity,amount " & _
                               "from trade tc,customer cu " & _
                               "where tc.cust_id = cu.cust_id and gold_type_id='96' and tc.mode='" + mode + "'")

            If cust_id <> "" Then
                sql += " and tc.cust_id = '" + cust_id + "'"
            End If
            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable

            da.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function checkPriceLimit(ByVal id As String, ByVal price As Double, ByVal purity As String, ByVal type As String) As Boolean ', ByVal bid991 As Double, ByVal bid992 As Double, ByVal bid993 As Double, ByVal ask991 As Double, ByVal ask992 As Double, ByVal ask993 As Double, ByVal bid961 As Double, ByVal bid962 As Double, ByVal bid963 As Double, ByVal ask961 As Double, ByVal ask962 As Double, ByVal ask963 As Double) As Boolean
        Try
            'Dim con As New SqlConnection(strcon)
            'Dim sql As String = ""
            'sql = String.Format("select usernames.cust_level, stock_online.bid99_1, stock_online.bid99_2, stock_online.bid99_3, stock_online.ask99_1, stock_online.ask99_2, stock_online.ask99_3, " & _
            '                    "stock_online.bid96_1, stock_online.bid96_2, stock_online.bid96_3, stock_online.ask96_1, stock_online.ask96_2, stock_online.ask96_3  " & _
            '                    "from usernames cross join stock_online where usernames.cust_id = (select cust_id from trade where trade_id='{0}')", id)

            'Dim cmd As New SqlCommand(sql, con)
            'Dim da As New SqlDataAdapter(cmd)
            'Dim dt As New DataTable
            ''Dim dbPrice As Double = 0
            Dim bid As Double = 0
            Dim ask As Double = 0

            Dim dc As New dcDBDataContext
            Dim trade = (From tr In dc.trades Where tr.trade_id = id Select tr.cust_id).Single
            Dim sto As clsStore = clsStore.getPriceStore(trade)

            If purity <> "96" Then
                bid = sto.bid99
                ask = sto.ask99
            Else
                bid = sto.bid96
                ask = sto.ask96
            End If

            If type = "sell" Then ' Bid
                If price >= ask Then
                    Return False
                End If
            Else 'Ask
                If price <= bid Then
                    Return False
                End If
            End If

            Return True
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Shared Function checkPriceLimitLeaveOrder(ByVal id As String, ByVal price As Double, ByVal purity As String, ByVal type As String) As Boolean
        Try
            Dim con As New SqlConnection(strcon)
            Dim sql As String = ""
            sql = String.Format("select cust_level,bid99_1,ask99_1,bid99_2,ask99_2,bid99_3,ask99_3,bid96_1,ask96_1,bid96_2,ask96_2,bid96_3,ask96_3 from usernames,stock_online where cust_id = (select cust_id from trade where trade_id='{0}')", id)

            Dim cmd As New SqlCommand(sql, con)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            Dim dbPrice As Double = 0
            Dim bid As Double = 0
            Dim ask As Double = 0

            Dim bid991 As Double
            Dim bid992 As Double
            Dim bid993 As Double
            Dim ask991 As Double
            Dim ask992 As Double
            Dim ask993 As Double
            Dim bid961 As Double
            Dim bid962 As Double
            Dim bid963 As Double
            Dim ask961 As Double
            Dim ask962 As Double
            Dim ask963 As Double

            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Dim level As Integer = Integer.Parse(dt.Rows(0)(0).ToString)

                bid991 = dt.Rows(0)("bid99_1").ToString
                bid992 = dt.Rows(0)("bid99_2").ToString
                bid993 = dt.Rows(0)("bid99_3").ToString
                ask991 = dt.Rows(0)("ask99_1").ToString
                ask992 = dt.Rows(0)("ask99_2").ToString
                ask993 = dt.Rows(0)("ask99_3").ToString

                bid961 = dt.Rows(0)("bid96_1").ToString
                bid962 = dt.Rows(0)("bid96_2").ToString
                bid963 = dt.Rows(0)("bid96_3").ToString
                ask961 = dt.Rows(0)("ask96_1").ToString
                ask962 = dt.Rows(0)("ask96_2").ToString
                ask963 = dt.Rows(0)("ask96_3").ToString


                If purity <> "96" Then
                    If level = 1 Then
                        bid = bid991
                        ask = ask991
                    ElseIf level = 2 Then
                        bid = bid992
                        ask = ask992
                    ElseIf level = 3 Then
                        bid = bid993
                        ask = ask993
                    End If
                Else
                    '96
                    If level = 1 Then
                        bid = bid961
                        ask = ask961
                    ElseIf level = 2 Then
                        bid = bid962
                        ask = ask962
                    ElseIf level = 3 Then
                        bid = bid963
                        ask = ask963
                    End If
                End If

                If type = "sell" Then ' Bid
                    If price >= ask Then
                        Return False
                    End If
                Else 'Ask
                    If price <= bid Then
                        Return False
                    End If
                End If

                Return True
            End If

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Shared Function UpdateTicketPortfolioByMode(ByVal id As String, ByVal mode As String, ByVal update_by As String, ByVal type As String, ByVal price As String, ByVal purity As String, ByVal cust_id As String, ByVal fast_trade As String) As Integer
        Try
            Dim con As New SqlConnection(strcon)
            Dim sql As String = ""
            If mode = clsManage.tradeMode.reject.ToString Then
                Dim reject_type As String = ""
                If cust_id = "" Then
                    reject_type = clsManage.rejectType.FinestGOLD.ToString
                Else
                    reject_type = clsManage.rejectType.Customer.ToString
                End If
                sql = String.Format("UPDATE trade SET mode='{0}',reject_type = '{3}',accept_type = '',modifier_date=getdate(),modifier_by='{2}' where trade_id='{1}'", mode, id, update_by, reject_type)
            Else
                If fast_trade = "accept" Then
                    sql = String.Format("UPDATE trade SET mode='{0}',accept_type = 'G',modifier_date=getdate(),modifier_by='{2}' where trade_id='{1}'", mode, id, update_by)
                Else
                    Dim mark As String = ""
                    If type = "sell" Then mark = "<=" Else mark = ">="
                    Dim sql_purity As String = ""
                    If purity = "96" Then
                        sql_purity = "gold_type_id='96' "
                    Else
                        sql_purity = "gold_type_id<>'96' "
                    End If

                    If fast_trade = "same" Then
                        sql = String.Format("UPDATE trade SET mode='{0}',accept_type = 'G',modifier_date=getdate(),modifier_by='{2}' " & _
                                             "where trade_id in(select trade_id from trade where  convert(varchar,modifier_date,103) = convert(varchar,getdate(),103) and type = '{3}' and {6} and mode='tran' and price = {4} )", mode, id, update_by, type, price, mark, sql_purity)
                    ElseIf fast_trade = "lowerOrHigher" Then

                        sql = String.Format("UPDATE trade SET mode='{0}',accept_type = 'G',modifier_date=getdate(),modifier_by='{2}' " & _
                                            "where trade_id in(select trade_id from trade where  convert(varchar,modifier_date,103) = convert(varchar,getdate(),103) and type = '{3}' and {6} and mode='tran' and price {5} {4} )", mode, id, update_by, type, price, mark, sql_purity)
                    End If
                End If

            End If
            Dim cmd As New SqlCommand(sql, con)
            cmd.CommandText = sql
            con.Open()
            Dim result As Integer = cmd.ExecuteNonQuery()
            con.Close()

            Return result

        Catch ex As Exception
            Throw ex
        End Try

    End Function
    Public Shared Function UpdateTicketPortfolioByPeriodPrice(ByVal price1 As String, ByVal price2 As String, ByVal update_by As String) As Integer
        Try
            Dim con As New SqlConnection(strcon)
            Dim sql As String = String.Format("UPDATE trade SET mode='accept',modifier_date=getdate(),modifier_by='{2}' where mode = 'tran' and (price between {0} and {1})", clsManage.convert2zero(price1), clsManage.convert2zero(price2), update_by)

            Dim cmd As New SqlCommand(sql, con)

            cmd.CommandText = sql
            con.Open()
            Dim result As Integer = cmd.ExecuteNonQuery()
            con.Close()

            Return result

        Catch ex As Exception
            Throw ex
        End Try

    End Function
#End Region

#Region "User"

    Public Shared Function updateUserActive(ByVal username As String) As Integer
        Try
            Dim con As New SqlConnection(strcon)

            Dim sql As String
            sql = "UPDATE usernames SET active='y'  where username = @username"

            Dim cmd As New SqlCommand(sql, con)

            Dim parameter As New SqlParameter("@username", SqlDbType.VarChar, 50)
            parameter.Value = username
            cmd.Parameters.Add(parameter)

            cmd.CommandText = sql
            con.Open()
            Dim result As Integer = cmd.ExecuteNonQuery()
            con.Close()

            Return result

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Shared Function getUsernamesByUsername(ByVal username As String) As DataTable

        Try

            Dim sql As String = "SELECT * FROM usernames where username = @username "


            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)

            Dim parameter As New SqlParameter("@username", SqlDbType.VarChar, 50)
            parameter.Value = username
            cmd.Parameters.Add(parameter)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Return dt
            Else
                Return Nothing
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getUsernames() As DataTable

        Dim sql As String = "select user_id,un.cust_id,username,password,firstname,role,cust_level,active,halt from usernames un, customer cm where un.cust_id = cm.cust_id and role='cust'"


        Dim con As New SqlConnection(strcon)
        Dim cmd As New SqlCommand(sql, con)
        Try

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable

            da.Fill(dt)
            Return dt

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getUsernameSearch(ByVal word As String) As DataTable

        Dim sql As String = "select user_id,un.cust_id,username,password,firstname,role,cust_level,active,halt from usernames un, customer cm where un.cust_id = cm.cust_id and role='cust' " & _
                            " and ( (un.username like '%' + @username + '%' or @username='') or (cm.firstname like '%' + @firstname + '%' or @firstname='')   )"


        Dim con As New SqlConnection(strcon)
        Dim cmd As New SqlCommand(sql, con)
        Try

            Dim parameter As New SqlParameter("@username", SqlDbType.VarChar, 50)
            parameter.Value = word
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@firstname", SqlDbType.VarChar, 50)
            parameter.Value = word
            cmd.Parameters.Add(parameter)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable

            da.Fill(dt)
            Return dt

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getUserAdmin() As DataTable

        Dim sql As String = "select user_id,username,password from usernames where role='admin'"


        Dim con As New SqlConnection(strcon)
        Dim cmd As New SqlCommand(sql, con)
        Try

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable

            da.Fill(dt)
            Return dt

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getChkCust_id(ByVal cust_id As String) As Data.DataTable

        Dim sql As String = String.Format("SELECT * From usernames where cust_id=@cust_id ")

        Dim con As New SqlConnection(strcon)
        Dim cmd As New SqlCommand(sql, con)

        Try
            Dim parameter As New SqlParameter("@cust_id", SqlDbType.VarChar)
            parameter.Value = cust_id
            cmd.Parameters.Add(parameter)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getDupUsername(ByVal Uname As String) As Boolean

        Dim sql As String = String.Format("SELECT * From usernames where username=@Uname ")

        Dim con As New SqlConnection(strcon)
        Dim cmd As New SqlCommand(sql, con)

        Try
            Dim parameter As New SqlParameter("@Uname", SqlDbType.VarChar, 50)
            parameter.Value = Uname
            cmd.Parameters.Add(parameter)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Return False
            Else
                Return True
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getChkUsersAdd(ByVal Uname As String) As Data.DataTable

        Dim sql As String = String.Format("SELECT * From usernames where username=@Uname ")

        Dim con As New SqlConnection(strcon)
        Dim cmd As New SqlCommand(sql, con)

        Try
            Dim parameter As New SqlParameter("@Uname", SqlDbType.VarChar, 50)
            parameter.Value = Uname
            cmd.Parameters.Add(parameter)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function DelUsername(ByVal id As String) As Integer

        Dim con As New SqlConnection(strcon)
        Dim sql As String = "Delete from usernames where user_id=" + id
        Dim cmd As New SqlCommand(sql, con)
        cmd.CommandText = sql
        con.Open()
        Try

            Dim result As Integer = cmd.ExecuteNonQuery()
            con.Close()
            Return result

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
    End Function


    Public Shared Function UpdateUsernamesReset(ByVal id As String, ByVal password As String, username As String, Optional regCode As String = "") As Integer
        Try
            Dim con As New SqlConnection(strcon)
            Dim sqlRegCode As String = ""
            If regCode <> "" Then
                sqlRegCode = ",reg_code = @reg_code"
            End If
            Dim sql As String
            sql = "UPDATE usernames SET password=@password,username = @username,first_trade = 'y' " + sqlRegCode + " where user_id =" + id

            Dim cmd As New SqlCommand(sql, con)
            Dim parameter As New SqlParameter("@username", SqlDbType.VarChar, 50)
            parameter.Value = username
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@password", SqlDbType.VarChar, 50)
            parameter.Value = password
            cmd.Parameters.Add(parameter)

            If regCode <> "" Then
                parameter = New SqlParameter("@reg_code", SqlDbType.VarChar, 50)
                parameter.Value = regCode
                cmd.Parameters.Add(parameter)
            End If

            cmd.CommandText = sql
            con.Open()
            Dim result As Integer = cmd.ExecuteNonQuery()
            con.Close()

            Return result

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Shared Function InsertUsernames(ByVal users As String, ByVal pass As String, ByVal type As String, user_id As String, ByVal customerID As String, ByVal customerlevel As String, regCode As String, halt As String) As Integer
        Try
            Dim con As New SqlConnection(strcon)

            Dim sql As String = ""
            If Not customerID = "" Then
                sql = String.Format("INSERT INTO usernames (username,password,role,cust_id,cust_level,reg_code,created_by,created_date,modifier_by,modifier_date,first_trade) VALUES (@users, @pass, @customerID, @type,@customerlevel,@reg_code,@created_by,getdate(),@modifier_by,getdate(),'y')")
            Else
                sql = String.Format("INSERT into usernames (username,password,role,reg_code,created_by,created_date,modifier_by,modifier_date,first_trade,active) VALUES (@users, @pass, @type, @reg_code,@created_by,getdate(),@modifier_by,getdate(),'n','y')")
            End If

            Dim cmd As New SqlCommand(sql, con)
            Dim parameter As New SqlParameter("@users", SqlDbType.VarChar, 50)
            parameter.Value = users
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@pass", SqlDbType.VarChar, 50)
            parameter.Value = pass
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@customerID", SqlDbType.VarChar, 50)
            parameter.Value = customerID
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@type", SqlDbType.VarChar, 50)
            parameter.Value = type
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@customerlevel", SqlDbType.Int)
            parameter.Value = IIf(type = "admin", DBNull.Value, customerlevel)
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@reg_code", SqlDbType.NVarChar, 50)
            parameter.Value = regCode
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@halt", SqlDbType.Char, 1)
            parameter.Value = halt
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@created_by", SqlDbType.VarChar, 10)
            parameter.Value = user_id
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@modifier_by", SqlDbType.VarChar, 10)
            parameter.Value = user_id
            cmd.Parameters.Add(parameter)

            cmd.CommandText = sql
            con.Open()
            Dim result As Integer = cmd.ExecuteNonQuery()
            con.Close()

            Return result

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Shared Function UpdateUsersHalt(ByVal id As String, ByVal halt As String) As Integer
        Try
            Dim con As New SqlConnection(strcon)

            Dim sql As String
            sql = "UPDATE usernames SET halt = @halt where user_id =" + id

            Dim cmd As New SqlCommand(sql, con)
            Dim parameter As New SqlParameter("@halt", SqlDbType.Char, 1)
            parameter.Value = halt
            cmd.Parameters.Add(parameter)

            cmd.CommandText = sql
            con.Open()
            Dim result As Integer = cmd.ExecuteNonQuery()
            con.Close()

            Return result

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Shared Function UpdateUserLevel(ByVal id As String, ByVal lv As String, ByVal halt As String) As Integer
        Try
            Dim con As New SqlConnection(strcon)

            Dim sql As String
            sql = "UPDATE usernames SET cust_level=@cust_level,halt = @halt where user_id =" + id

            Dim cmd As New SqlCommand(sql, con)
            Dim parameter As New SqlParameter("@cust_level", SqlDbType.Int)
            parameter.Value = lv
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@halt", SqlDbType.Char, 1)
            parameter.Value = halt
            cmd.Parameters.Add(parameter)

            cmd.CommandText = sql
            con.Open()
            Dim result As Integer = cmd.ExecuteNonQuery()
            con.Close()

            Return result

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Shared Function UpdateUsernames(ByVal id As String, ByVal password As String) As Integer
        Try
            Dim con As New SqlConnection(strcon)

            Dim sql As String
            sql = "UPDATE usernames SET password=@password where user_id =" + id

            Dim cmd As New SqlCommand(sql, con)
            Dim parameter As New SqlParameter("@password", SqlDbType.VarChar, 50)
            parameter.Value = password
            cmd.Parameters.Add(parameter)

            cmd.CommandText = sql
            con.Open()
            Dim result As Integer = cmd.ExecuteNonQuery()
            con.Close()

            Return result

        Catch ex As Exception
            Throw ex
        End Try

    End Function
    Public Shared Function getUsernamesValue(ByVal id As String) As DataTable

        Dim sql As String = "select * from usernames where user_id =" + id


        Dim con As New SqlConnection(strcon)
        Dim cmd As New SqlCommand(sql, con)
        Try

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable

            da.Fill(dt)
            Return dt

        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Shared Function getTeam() As DataTable

        Dim sql As String = "select team_id,team_name from team"
        Dim con As New SqlConnection(strcon)
        Try
            Dim da As New SqlDataAdapter(sql, con)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Return dt
            Else
                Return Nothing
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getUserLogin(ByVal username As String, ByVal password As String) As DataTable

        Dim sql As String = "SELECT * " & _
                            " FROM  users " & _
                            " Where (user_name = @user_name) AND (password=@password) "


        Dim con As New SqlConnection(strcon)
        Dim cmd As New SqlCommand(sql, con)
        Try
            Dim parameter As New SqlParameter("@user_name", SqlDbType.VarChar, 50)
            parameter.Value = username
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@password", SqlDbType.VarChar, 50)
            parameter.Value = password
            cmd.Parameters.Add(parameter)


            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable

            da.Fill(dt)
            Return dt

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function log_users(ByVal user_id As String, ByVal log_type As String) As Boolean
        Try
            Dim sql As String = String.Format("INSERT INTO users_log (user_id,log_time,log_type) VALUES ('{0}',getdate(),'{1}')", user_id, log_type)
            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)
            con.Open()
            Dim result_log As Integer = cmd.ExecuteNonQuery()
            con.Close()
            If result_log > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function updateUsernamesPasswordByUsername(ByVal username As String, ByVal password As String) As Integer
        Try
            Dim con As New SqlConnection(strcon)

            Dim sql As String
            sql = "UPDATE usernames SET password=@password,lock_time = null where username = @username "

            Dim cmd As New SqlCommand(sql, con)
            Dim parameter As New SqlParameter("@password", SqlDbType.VarChar, 50)
            parameter.Value = password
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@username", SqlDbType.VarChar, 50)
            parameter.Value = username
            cmd.Parameters.Add(parameter)

            cmd.CommandText = sql
            con.Open()
            Dim result As Integer = cmd.ExecuteNonQuery()
            con.Close()

            Return result

        Catch ex As Exception
            Throw ex
        End Try

    End Function
#End Region

#Region "Stock"

    Public Shared Function getStockOnlineUsers(ByVal cust_id As String) As DataTable

        Dim sql As String = "select *,getdate() as getdate from stock_online,usernames where stock_id = 1 And cust_id = '" + cust_id + "'"
        Dim con As New SqlConnection(strcon)
        Try
            Dim da As New SqlDataAdapter(sql, con)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Return dt
            End If
            Return Nothing
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function updateStockHalt(ByVal halt As String) As String
        Try
            Dim sql As String = String.Format("update stock_online set halt = @halt where stock_id = 1")

            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)

            Dim parameter As New SqlParameter("@halt", SqlDbType.VarChar, 1)
            parameter.Value = halt
            cmd.Parameters.Add(parameter)

            con.Open()
            Dim result As Integer = cmd.ExecuteNonQuery()
            con.Close()
            If result > 0 Then
                Return halt
            Else
                Return ""
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Function


    Public Shared Function getStockLogByDate(ByVal isToday As String, Optional ByVal pDate1 As DateTime = Nothing, Optional ByVal pDate2 As DateTime = Nothing) As Data.DataTable
        Try
            Dim sql_period As String = ""
            Dim period1 As String = ""
            Dim period2 As String = ""
            If isToday = "y" Then
                sql_period = " convert(varchar,modifier_date,111)=convert(varchar,getdate(),111) "
            Else
                sql_period = " modifier_date between @period1 and @period2 "
            End If

            Dim sql As String = "SELECT * FROM stock_online_log Where " + sql_period + "ORDER BY id DESC"
            Dim dt As New Data.DataTable
            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)

            If isToday = "n" Then
                Dim parameter As New SqlParameter("@period1", SqlDbType.DateTime)
                parameter.Value = pDate1
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@period2", SqlDbType.DateTime)
                parameter.Value = pDate2
                cmd.Parameters.Add(parameter)
            End If

            Dim da As New SqlDataAdapter(cmd)
            da.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getMsg() As String

        Dim sql As String = "SELECT msg FROM stock_online"
        Dim con As New SqlConnection(strcon)
        Try
            Dim da As New SqlDataAdapter(sql, con)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Return dt.Rows(0)("msg").ToString
            End If
            Return Nothing
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Shared Function getStockToday(ByVal planningDay As String) As DataTable

        Dim sql As String = "exec getSummaryStock"
        If planningDay <> "0" Then
            sql = String.Format("getSummaryStockPlanning '{0}'", planningDay)
        Else
            sql = "getSummaryStockActual"
        End If
        Dim con As New SqlConnection(strcon)
        Try
            Dim da As New SqlDataAdapter(sql, con)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Return dt
            End If
            Return Nothing
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getStockNow() As DataTable

        Dim sql As String = "SELECT * FROM STOCK"
        Dim con As New SqlConnection(strcon)
        Try
            Dim da As New SqlDataAdapter(sql, con)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Return dt
            End If
            Return Nothing
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function updatePriceNow(ByVal bid96 As String, ByVal ask96 As String, ByVal bid99 As String, ByVal ask99 As String, ByVal pwd_auth As String, ByVal margin As String) As Boolean
        Try
            Dim sql As String = String.Format("update stock set bid96 = {0},ask96 = {1},bid99 = {2},ask99 = {3},pwd_auth={4},margin = {5}", bid96, ask96, bid99, ask99, pwd_auth, margin)
            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)
            cmd.CommandText = sql
            con.Open()
            Dim result As Integer = cmd.ExecuteNonQuery()
            con.Close()
            If result > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Shared Function getStockAndSumDeposit(Optional ByVal plusDep As String = "n") As DataTable

        Dim sql_select As String = "select stock.price_base as price, stock.cash as cash, stock.trans as trans, stock.cheq as cheq, stock.G96_base as G96, stock.G99N_base as G99N, stock.G99L_base as G99L"
        If plusDep = "y" Then
            sql_select = " select stock.price_base+depPrice as price, stock.cash+depCash as cash, stock.trans+depTrans as trans, stock.cheq+depCheq as cheq, stock.G96_base+dep96 as G96, stock.G99N_base+dep99N as G99N, stock.G99L_base+dep99L as G99L"
        End If

        Dim sql As String = sql_select + " from stock cross join" + _
" ( select isnull( sum(amount),0) as depPrice  from  (select case when type = 'Deposit' then amount when type = 'Withdraw' then -(amount) else 0 end as amount from customer_trans where gold_type_id = '') depPrice)  price cross join " & _
" ( select isnull( sum(quantity),0) as dep96  from  (select case when type = 'Deposit' then quantity when type = 'Withdraw' then -(quantity) else 0 end as quantity from customer_trans where gold_type_id = '96') dep96)  G96 cross join " & _
" ( select isnull( sum(quantity),0) as dep99N from  (select case when type = 'Deposit' then quantity when type = 'Withdraw' then -(quantity) else 0 end as quantity from customer_trans where gold_type_id = '99N') dep99N)  G99N cross join " & _
" ( select isnull( sum(quantity),0) as dep99L from  (select case when type = 'Deposit' then quantity when type = 'Withdraw' then -(quantity) else 0 end as quantity from customer_trans where gold_type_id = '99L') dep99L)  G99L cross join " & _
" ( select isnull( sum(amount),0) as depCash  from  (select case when type = 'Deposit' then amount when type = 'Withdraw' then -(amount) else 0 end as amount from customer_trans where gold_type_id = '' and payment='cash') depCash)  cash cross join" & _
" ( select isnull( sum(amount),0) as depTrans from  (select case when type = 'Deposit' then amount when type = 'Withdraw' then -(amount) else 0 end as amount from customer_trans where gold_type_id = '' and payment='trans') depTrans)  trans cross join" & _
" ( select isnull( sum(amount),0) as depCheq  from  (select case when type = 'Deposit' then amount when type = 'Withdraw' then -(amount) else 0 end as amount from customer_trans where gold_type_id = '' and payment='cheq') depCheq)  cheq "
        Dim con As New SqlConnection(strcon)
        Try
            Dim da As New SqlDataAdapter(sql, con)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Return dt
            End If
            Return Nothing
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getStockSumDeposit() As DataTable

        Dim sql_select As String = "select stock.cash, stock.cheq, stock.trans, stock.price_base as price_base,stock.G96_base as G96_base,stock.G99N_base as G99N_base,stock.G99L_base as G99L_base, stock.price_base+depPrice as priceDep,stock.G96_base+dep96 as G96Dep,stock.G99N_base+dep99N as G99NDep,stock.G99L_base+dep99L as G99LDep, stock.cash+depCash as cashDep, stock.cheq+depCheq as cheqDep,stock.trans+depTrans as transDep"

        Dim sql As String = sql_select + " from stock cross join" + _
" ( select isnull( sum(amount),0) as depPrice  from  (select case when type = 'Deposit' then amount when type = 'Withdraw' then -(amount) else 0 end as amount from customer_trans where gold_type_id = '') depPrice)  price cross join " & _
" ( select isnull( sum(quantity),0) as dep96  from  (select case when type = 'Deposit' then quantity when type = 'Withdraw' then -(quantity) else 0 end as quantity from customer_trans where gold_type_id = '96') dep96)  G96 cross join " & _
" ( select isnull( sum(quantity),0) as dep99N from  (select case when type = 'Deposit' then quantity when type = 'Withdraw' then -(quantity) else 0 end as quantity from customer_trans where gold_type_id = '99N') dep99N)  G99N cross join " & _
" ( select isnull( sum(quantity),0) as dep99L from  (select case when type = 'Deposit' then quantity when type = 'Withdraw' then -(quantity) else 0 end as quantity from customer_trans where gold_type_id = '99L') dep99L)  G99L cross join " & _
" ( select isnull( sum(amount),0) as depCash  from  (select case when type = 'Deposit' then amount when type = 'Withdraw' then -(amount) else 0 end as amount from customer_trans where gold_type_id = '' and payment='cash') depCash)  cash cross join" & _
" ( select isnull( sum(amount),0) as depTrans from  (select case when type = 'Deposit' then amount when type = 'Withdraw' then -(amount) else 0 end as amount from customer_trans where gold_type_id = '' and payment='trans') depTrans)  trans cross join" & _
" ( select isnull( sum(amount),0) as depCheq  from  (select case when type = 'Deposit' then amount when type = 'Withdraw' then -(amount) else 0 end as amount from customer_trans where gold_type_id = '' and payment='cheq') depCheq)  cheq "
        Dim con As New SqlConnection(strcon)
        Try
            Dim da As New SqlDataAdapter(sql, con)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Return dt
            End If
            Return Nothing
        Catch ex As Exception
            Throw ex
        End Try
    End Function

#End Region

#Region "Usernames"

    'change username and pwd
    Public Shared Function updateUsernameChangUserAndPwd(cust_id As String, username As String, pwd As String) As Integer
        Dim sql As String = "update usernames set username = @username,password=@password where cust_id =@cust_id "
        Dim con As New SqlConnection(strcon)
        Dim cmd As New SqlCommand(sql, con)
        con.Open()
        Try
            cmd.CommandText = sql
            Dim parameter As New SqlParameter("@username", SqlDbType.VarChar, 50)
            parameter.Value = username
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@password", SqlDbType.VarChar, 50)
            parameter.Value = pwd
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@cust_id", SqlDbType.VarChar, 50)
            parameter.Value = cust_id
            cmd.Parameters.Add(parameter)

            Dim result As Integer = cmd.ExecuteNonQuery()

            Return result
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

    End Function

    Public Shared Function updateUsernameChangUsername(cust_id As String, username As String) As Integer
        Dim sql As String = "update usernames set username = @username where cust_id =@cust_id "
        Dim con As New SqlConnection(strcon)
        Dim cmd As New SqlCommand(sql, con)
        con.Open()
        Try
            cmd.CommandText = sql
            Dim parameter As New SqlParameter("@username", SqlDbType.VarChar, 50)
            parameter.Value = username
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@cust_id", SqlDbType.VarChar, 50)
            parameter.Value = cust_id
            cmd.Parameters.Add(parameter)

            Dim result As Integer = cmd.ExecuteNonQuery()

            Return result
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

    End Function

    Public Shared Function updateUsernameChangPassword(cust_id As String, pwd As String) As Integer
        Dim sql As String = "update usernames set password = @password where cust_id =@cust_id "
        Dim con As New SqlConnection(strcon)
        Dim cmd As New SqlCommand(sql, con)
        con.Open()
        Try
            cmd.CommandText = sql
            Dim parameter As New SqlParameter("@password", SqlDbType.VarChar, 50)
            parameter.Value = pwd
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@cust_id", SqlDbType.VarChar, 50)
            parameter.Value = cust_id
            cmd.Parameters.Add(parameter)

            Dim result As Integer = cmd.ExecuteNonQuery()

            Return result
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

    End Function

    Public Shared Function updateUsernameLockTime(username As String) As Boolean
        Try
            Dim sql As String = String.Format("update usernames set lock_time = getdate()  where username = '{0}'", username)
            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)
            cmd.CommandText = sql
            con.Open()
            Dim result As Integer = cmd.ExecuteNonQuery()
            con.Close()
            If result > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Shared Function updateUsernameFirstTrade(user_id As String) As Boolean
        Try
            Dim sql As String = String.Format("update usernames set first_trade = 'n' where user_id = {0}", user_id)
            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)
            cmd.CommandText = sql
            con.Open()
            Dim result As Integer = cmd.ExecuteNonQuery()
            con.Close()
            If result > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    'Public Shared Function checkFirstTrade(ByVal username As String, ByVal idCard As String, birthday As Date, fname As String, lname As String) As DataTable

    '    Dim sql As String = "select customer.email from usernames inner join customer on usernames.cust_id = customer.cust_id " & _
    '                        " Where (usernames.username = @username) AND (customer.id_card=@id_card) AND convert(datetime,customer.birthday,111)=convert(datetime,@birthday,111) " & _
    '                        " AND firstname_eng = @firstname_eng AND lastname_eng = @lastname_eng "


    '    Dim con As New SqlConnection(strcon)
    '    Dim cmd As New SqlCommand(sql, con)
    '    Try
    '        Dim parameter As New SqlParameter("@username", SqlDbType.VarChar, 50)
    '        parameter.Value = username
    '        cmd.Parameters.Add(parameter)

    '        parameter = New SqlParameter("@id_card", SqlDbType.VarChar, 50)
    '        parameter.Value = idCard
    '        cmd.Parameters.Add(parameter)

    '        parameter = New SqlParameter("@firstname_eng", SqlDbType.VarChar, 50)
    '        parameter.Value = fname
    '        cmd.Parameters.Add(parameter)

    '        parameter = New SqlParameter("@lastname_eng", SqlDbType.VarChar, 50)
    '        parameter.Value = lname
    '        cmd.Parameters.Add(parameter)

    '        parameter = New SqlParameter("@birthday", SqlDbType.Date)
    '        parameter.Value = birthday
    '        cmd.Parameters.Add(parameter)


    '        Dim da As New SqlDataAdapter(cmd)
    '        Dim dt As New DataTable

    '        da.Fill(dt)
    '        Return dt

    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function

    Public Shared Function getUsernameForget(ByVal username As String, ByVal idCard As String, birthday As Date, mobile As String) As DataTable

        Dim sql As String = "select customer.email,username,password  from usernames inner join customer on usernames.cust_id = customer.cust_id " & _
                            " Where usernames.username = @username AND customer.id_card=@id_card AND customer.mobile=@mobile AND usernames.active = 'y' AND convert(varchar,customer.birthday,111)=convert(varchar,@birthday,111) "


        Dim con As New SqlConnection(strcon)
        Dim cmd As New SqlCommand(sql, con)
        Try
            Dim parameter As New SqlParameter("@username", SqlDbType.VarChar, 50)
            parameter.Value = username
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@id_card", SqlDbType.VarChar, 50)
            parameter.Value = idCard
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@birthday", SqlDbType.Date)
            parameter.Value = birthday
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@mobile", SqlDbType.VarChar, 50)
            parameter.Value = mobile
            cmd.Parameters.Add(parameter)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable

            da.Fill(dt)
            Return dt

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getUsernameLogin(ByVal username As String, ByVal password As String) As DataSet

        'check ด้วยว่า password ผิด3 ครั้งต้อง lock
        Dim time_unlock As String = AppSettings("UNLOCK_TIME").ToString
        Dim sql As String = "SELECT *,case when getdate() > DATEADD(minute, " + time_unlock + ", lock_time) then 'n' else 'y' end as lock FROM usernames  Where username = @username and active = 'y'; " & _
                            "SELECT *,(select system_halt from stock_online) as system_halt FROM  usernames " & _
                            " Where (username = @username) AND (password=@password) AND (active = 'y') "
        Dim dt As New DataTable
        Dim ds As New DataSet
        Try
            Using con As New SqlConnection(strcon)
                Using cmd As New SqlCommand(sql, con)

                    Dim parameter As New SqlParameter("@username", SqlDbType.VarChar, 50)
                    parameter.Value = username
                    cmd.Parameters.Add(parameter)

                    parameter = New SqlParameter("@password", SqlDbType.VarChar, 50)
                    parameter.Value = password
                    cmd.Parameters.Add(parameter)
                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(ds)
                        'If ds.Tables(1).Rows(0)("role").ToString = "cust" Then
                        '    'update database set login user
                        '    sql = String.Format("update users set active = 'y' where user_id = '{0}'", username)
                        '    cmd.CommandText = sql
                        '    con.Open()
                        '    cmd.ExecuteNonQuery()
                        'End If

                        Return ds
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            ds.Dispose()
            dt.Dispose()
        End Try
    End Function

    Public Shared Function updateLoginStatus(cust_id As String, online_status As String, online_id As String) As Integer
        Try
            Dim sql As String = String.Format("update usernames set online = '{1}',online_time = getdate(),online_id ='{2}'  where cust_id = {0}", cust_id, online_status, online_id)
            Using con As New SqlConnection(strcon)
                Using cmd As New SqlCommand(sql, con)
                    cmd.CommandText = sql
                    con.Open()
                    Return cmd.ExecuteNonQuery()
                End Using
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function updateTimeOnline(cust_id As String, online_id As String) As String
        Dim dt As New DataTable
        Try
            Dim sql As String = String.Format("select online,online_time,online_id,case when DATEADD(Minute, 20, online_time) >  getdate() then 'y' else 'n' end as timeout " & _
                                              " from usernames where cust_id = {0}  ", cust_id)
            Using con As New SqlConnection(strcon)
                Using cmd As New SqlCommand(sql, con)
                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(dt)
                        If dt.Rows.Count > 0 Then
                            If dt.Rows(0)("online_id").ToString = online_id Then
                                If dt.Rows(0)("online").ToString = "y" Then
                                    'sql = String.Format("update usernames set online_time = getdate() where cust_id = {0}", cust_id)
                                    'cmd.CommandText = sql
                                    'con.Open()
                                    'cmd.ExecuteNonQuery()
                                End If
                            Else
                                'If dt.Rows(0)("online").ToString = "y" And dt.Rows(0)("timeout").ToString = "y" Then ' user ใช้งานอยู่
                                'ถ้า session_id เปลี่ยนไปให้ ออกเลย
                                'End If
                                Return "user ของท่านมีการเข้าใช้งานจากเครื่องอื่น"
                            End If
                        End If
                        Return ""
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            dt.Dispose()
        End Try
    End Function

#End Region

End Class
