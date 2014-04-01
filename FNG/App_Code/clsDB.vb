Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data
Imports System.Net.Mail.SmtpClient
Imports System.Net.Mail
Imports System.Configuration.ConfigurationManager
Imports System.Configuration

Public Class clsDB
    Public Shared strcon As String = ConfigurationManager.ConnectionStrings("FNGConnectionString").ConnectionString
    Public Shared ReadOnly urlHome As String = "Default.aspx"

    Public Shared Function getTime(ByVal tDate As DateTime) As DateTime
        Try
            Dim sql As String = "select DATEADD(DAY,-(DATEDIFF(DAY, @ticket_date, GETDATE())),GETDATE())"

            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable

            Dim parameter As New SqlParameter("@ticket_date", SqlDbType.DateTime)
            parameter.Value = tDate
            cmd.Parameters.Add(parameter)

            da.Fill(dt)
            Return dt.Rows(0)(0)
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#Region "customer And Usernames"

    Public Shared Function delCustomerAndUsernames(ByVal cust_id As String) As Boolean
        Try
            Dim sql As String = String.Format("Delete from customer where cust_id = '{0}';Delete from usernames where cust_id = '{0}'", cust_id)
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
    Public Shared Function checkDuplicateEmailAndIdcard(email As String, idcard As String) As String

        Dim ds As New DataSet
        Try
            Dim sql As String = "select * from customer where id_card = @id_card;select * from customer where email  = @email"
            Using con As New SqlConnection(strcon)
                Using cmd As New SqlCommand(sql, con)
                    Using da As New SqlDataAdapter(cmd)

                        Dim parameter As New SqlParameter("@email", SqlDbType.VarChar, 50)
                        parameter.Value = email
                        cmd.Parameters.Add(parameter)

                        parameter = New SqlParameter("@id_card", SqlDbType.VarChar, 50)
                        parameter.Value = idcard
                        cmd.Parameters.Add(parameter)
                        da.Fill(ds)
                        Dim result As String = ""
                        If ds.Tables(0).Rows.Count > 0 Then
                            result = "1"
                        Else
                            result = "0"
                        End If

                        If ds.Tables(1).Rows.Count > 0 Then
                            result += "1"
                        Else
                            result += "0"
                        End If
                        Return result
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            ds = Nothing
        End Try
    End Function

#End Region

#Region "Customer"
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

   
    Public Shared Function getCustomer(ByVal str As String, isCall As String, created_date As String, created_date2 As String, Optional ByVal user_id As String = "", Optional ByVal team_id As String = "") As DataTable
        Try
            Dim sql_condition As String = ""
            If user_id <> "" Then
                sql_condition = "AND cust_id in (select distinct cust_id from tickets where created_by ='" + user_id + "')"
            End If

            Dim sql_team As String = ""
            If team_id <> "" Then
                If team_id = "none" Then
                    sql_team = " AND (vcf.team_id='') "
                Else
                    sql_team = " AND (vcf.team_id='" + team_id + "') "
                End If
            End If

            Dim sql_isCall As String = ""
            If isCall <> "" Then
                If isCall = "y" Then
                    sql_isCall = " AND cust_level is null"
                Else
                    sql_isCall = " AND cust_level is not null"
                End If
            End If

            Dim sql_created_date As String = ""
            If created_date <> "" AndAlso created_date2 <> "" Then
                sql_created_date = " AND (convert(varchar, created_date,111) BETWEEN  @created_date AND @created_date2)"
            End If

            Dim sql As String = "Select cust_id,cust_type,firstname,lastname,tel,bid96,bid99,ask96,ask99,margin,free_margin,cash,quan96,quan99,amount96,amount99,gold96,gold99,created_date," & _
           "case when cust_level is null then 'Call' else 'Call/Online' end as trade_type " & _
           " from v_customer_folio vcf " & _
           " WHERE ((cust_id = @cust_id or @cust_id = '') or " & _
           " (firstname like '%' + @firstname + '%' or @firstname='') or (lastname like '%' + @lastname + '%' or @lastname='')) " + sql_team + " " & _
           " " + sql_condition + sql_isCall + sql_created_date + "" & _
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

            If created_date <> "" AndAlso created_date2 <> "" Then
                parameter = New SqlParameter("@created_date", SqlDbType.DateTime)
                parameter.Value = DateTime.ParseExact(created_date, clsManage.formatDateTime, Nothing)
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@created_date2", SqlDbType.DateTime)
                parameter.Value = DateTime.ParseExact(created_date2, clsManage.formatDateTime, Nothing)
                cmd.Parameters.Add(parameter)
            End If

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
            Dim sql As String = "SELECT * ,case when cust_level is null then 'Call' else 'Call/Online' end as trade_type " & _
                        " FROM customer LEFT OUTER JOIN " & _
                        " usernames ON customer.cust_id = usernames.cust_id LEFT OUTER JOIN " & _
            " ( " & _
                " SELECT ticket.cust_id,sum(ticket.amount) as amount,sum(paid) as paid ,sum(quantity) as sumQuan" & _
                " FROM         ticket left outer JOIN " & _
                " ( SELECT     ref_no, SUM(amount) AS paid " & _
                " 	FROM          ticket_split  GROUP BY ref_no " & _
                " ) AS ticket_split ON ticket.ref_no = ticket_split.ref_no where ticket.status_id =101 group by cust_id " & _
            " )credit on customer.cust_id = credit.cust_id  cross join stock " & _
                        " WHERE (customer.cust_id = @cust_id or @cust_id = '') and  (customer.firstname = @firstname or @firstname = '')"

            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)

            Dim parameter As New SqlParameter("@cust_id", SqlDbType.VarChar, 10)
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

        Dim sql As String = "SELECT * FROM customer_trans left outer join users on created_by = users.user_id where cust_id = '" & cust_id & "' " & sql_pure & " order by cust_tran_id desc "
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

    Public Shared Function insertCustomer(ByVal cust_refno As String, ByVal cust_type_id As String, ByVal titlename As String, ByVal firstname As String, ByVal lastname As String, ByVal firstnameEng As String, ByVal lastnameEng As String, ByVal idCard As String, ByVal birthday As Date, ByVal email As String, ByVal person_contact As String, ByVal bill_address As String, ByVal bank1 As String, ByVal account_no1 As String, ByVal account_name1 As String, ByVal account_type1 As String, ByVal account_branch1 As String, ByVal bank2 As String, ByVal account_no2 As String, ByVal account_name2 As String, ByVal account_type2 As String, ByVal account_branch2 As String, ByVal bank3 As String, ByVal account_no3 As String, ByVal account_name3 As String, ByVal account_type3 As String, ByVal account_branch3 As String, ByVal mobilePhone As String, ByVal tel As String, ByVal fax As String, ByVal cash_credit As String, ByVal remark As String, ByVal margin As String, ByVal margin_call As String, ByVal quan_96 As String, ByVal quan_99 As String, ByVal created_by As String, ByVal team_id As String, ByVal trade_limit As String, ByVal free_margin As String, margin_unlimit As Boolean, VIP As Boolean, discount_buy As Double, discount_sell As Double) As String
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
                'Dim G99N As Double = clsManage.convert2zero(dt_stock.Rows(0)("G99NDep"))
                'Dim G99L As Double = clsManage.convert2zero(dt_stock.Rows(0)("G99LDep"))
                Dim G99 As Double = clsManage.convert2zero(dt_stock.Rows(0)("G99Dep"))
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
                    sql_act = String.Format(";INSERT INTO [actual] ([asset_id], [ref_no], [asset_type], [created_by], [datetime], [purity], [quantity], [amount], [status_id], [status_name], [type], [order_type], [price_base], [G96_base], [G99_base], [cust_id], [before_status_id], [note], [payment], [cash], [trans], [cheq]) " & _
                                                     "VALUES ('{0}', '{1}', '{2}', '{3}', getdate(), '{4}', {5}, {6}, '{7}', '{8}', '{9}', '{10}', {11}, {12}, {13}, {14},'{15}','{16}','{17}',{18},{19},{20})" & _
                                                     "", "0", "", "Gold", created_by, gold_type, quan_96, "0", "999", "", "ฝากทอง", "D/W", price, G96, G99, cust_id, "000", "ฝากทองในการลงทะเบียนครั้งแรก", "", cash_stock, trans_stock, cheq_stock)
                    '"VALUES ('{0}', '{1}', '{2}', '{3}', getdate(), '{4}', {5}, {6}, '{7}', '{8}', '{9}', '{10}', {11}, {12}, {13}, {14},'{16}','{17}','{18}',{19},{20},{21})" & _
                End If
                If quan_99 <> "0" Then
                    gold_type = "99"
                    quan_dep = quan_99
                    sql_dep += String.Format(";INSERT INTO [customer_trans] ([cust_id], [datetime], [type], [ticket_refno], [gold_type_id], [quantity], [amount], [remark], [created_by], [created_date]) VALUES " & _
                                           "('{0}', getdate(), 'Deposit', '{1}', '{2}', {3}, {4}, 'Customer Deposit First.', '{5}', getdate())", cust_id, "", gold_type, quan_dep, "0", created_by)
                    G99 += quan_99
                    sql_act += String.Format(";INSERT INTO [actual] ([asset_id], [ref_no], [asset_type], [created_by], [datetime], [purity], [quantity], [amount], [status_id], [status_name], [type], [order_type], [price_base], [G96_base], [G99_base], [cust_id], [before_status_id], [note], [payment], [cash], [trans], [cheq]) " & _
                                                    "VALUES ('{0}', '{1}', '{2}', '{3}', getdate(), '{4}', {5}, {6}, '{7}', '{8}', '{9}', '{10}', {11}, {12}, {13}, {14},'{15}','{16}','{17}',{18},{19},{20})" & _
                                                    "", "0", "", "Gold", created_by, gold_type, quan_99, "0", "999", "", "ฝากทอง", "D/W", price, G96, G99, cust_id, "000", "ฝากทองในการลงทะเบียนครั้งแรก", "", cash_stock, trans_stock, cheq_stock)
                End If

                sql = "INSERT INTO [customer] ([cust_id], [cust_refno], [cust_type_id], [titlename], [firstname], [lastname], [firstname_eng], [lastname_eng], [email], [id_card], [birthday], [person_contact], [bill_address], [bank1], [account_no1], [account_name1], [account_type1], [account_branch1], [bank2], [account_no2], [account_name2], [account_type2], [account_branch2], [bank3], [account_no3], [account_name3], [account_type3], [account_branch3], [mobile], [tel], [fax], [remark], [cash_credit], [margin], [margin_call], [quan_96], [quan_99N], [created_by], [created_date], [team_id], [trade_limit], [free_margin],[margin_unlimit],[VIP],[discount_buy],[discount_sell]) VALUES " & _
                      "(@cust_id, @cust_refno, @cust_type_id, @titlename, @firstname, @lastname, @firstname_eng, @lastname_eng, @email, @id_card, @birthday, @person_contact, @bill_address, @bank1, @account_no1, @account_name1, @account_type1, @account_branch1, @bank2, @account_no2, @account_name2, @account_type2, @account_branch2, @bank3, @account_no3, @account_name3, @account_type3, @account_branch3, @mobile, @tel, @fax, @remark, @cash_credit, @margin, @margin_call, @quan_96, @quan_99N,@created_by,getdate(),@team_id,@trade_limit,@free_margin,@margin_unlimit,@VIP,@discount_buy,@discount_sell)"

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

                parameter = New SqlParameter("@firstname_eng", SqlDbType.VarChar, 50)
                parameter.Value = firstnameEng
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@lastname_eng", SqlDbType.VarChar, 50)
                parameter.Value = lastnameEng
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@id_card", SqlDbType.VarChar, 50)
                parameter.Value = idCard
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@birthday", SqlDbType.Date)
                parameter.Value = birthday
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@email", SqlDbType.VarChar, 50)
                parameter.Value = email
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
                parameter.Value = clsManage.convert2zero(quan_99)
                cmd.Parameters.Add(parameter)

                'parameter = New SqlParameter("@quan_99L", SqlDbType.Decimal)
                'parameter.Value = clsManage.convert2zero(quan_99L)
                'cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@created_by", SqlDbType.VarChar, 50)
                parameter.Value = fax
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@team_id", SqlDbType.VarChar, 50)
                parameter.Value = team_id
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@trade_limit", SqlDbType.Decimal)
                parameter.Value = clsManage.convert2zero(trade_limit)
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@free_margin", SqlDbType.Decimal)
                parameter.Value = clsManage.convert2zero(free_margin)
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@margin_unlimit", SqlDbType.Bit)
                parameter.Value = margin_unlimit
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@VIP", SqlDbType.Bit)
                parameter.Value = VIP
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@discount_buy", SqlDbType.Float)
                parameter.Value = discount_buy
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@discount_sell", SqlDbType.Float)
                parameter.Value = discount_sell
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

    Public Shared Function getStockDepositPre(ByVal purity As String, ByVal date1 As String, ByVal date2 As String) As DataTable
        Dim dt As New DataTable
        Dim con As New SqlConnection(strcon)
        Dim cmd As New SqlCommand()
        Try
            Dim sql As String = ""
            If purity = "" Then
                purity = " deposit.gold_type_id = '' "
            Else
                purity = String.Format(" deposit.gold_type_id in ('{0}')", purity)
            End If

            Dim sql_ticket_date As String = ""
            If date1 <> "" AndAlso date2 <> "" Then
                sql_ticket_date = " AND (convert(varchar, deposit.datetime,111) BETWEEN  @date1 AND @date2)"
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
" Where {0}{1} order by deposit.created_date desc ", purity, sql_ticket_date)


            cmd.Connection = con
            cmd.CommandText = sql

            If Not (date1 = "" And date2 = "") Then
                Dim Parameter As New SqlParameter("@date1", SqlDbType.DateTime)
                Parameter.Value = DateTime.ParseExact(date1, clsManage.formatDateTime, Nothing)
                cmd.Parameters.Add(Parameter)

                Parameter = New SqlParameter("@date2", SqlDbType.DateTime)
                Parameter.Value = DateTime.ParseExact(date2, clsManage.formatDateTime, Nothing)
                cmd.Parameters.Add(Parameter)
            End If

            Using da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                Return dt
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            dt.Dispose()
            con.Dispose()
            cmd.Dispose()
        End Try

    End Function

    Public Shared Function getStockDeposit(ByVal team_id As String) As DataTable
        Dim dt As New DataTable
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
            "   ORDER BY cust_id DESC"

          
            Using da As New SqlDataAdapter(sql, strcon)
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    Return dt
                Else
                    Return Nothing
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            dt.Dispose()
        End Try

    End Function

    Public Shared Function getStockDeposit2(ByVal str As String, ByVal team_id As String) As DataTable
        Dim dt As New DataTable
        Dim con As New SqlConnection(strcon)
        Dim cmd As New SqlCommand()
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
            "   AND ((cust_id = @cust_id or @cust_id = '') or (firstname like '%' + @firstname + '%' or @firstname='') or (lastname like '%' + @lastname + '%' or @lastname='')) " & _
            "   ORDER BY cust_id DESC"

            cmd.Connection = con
            cmd.CommandText = sql

            Dim parameter As New SqlParameter("@cust_id", SqlDbType.VarChar, 5)
            parameter.Value = str
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@firstname", SqlDbType.VarChar, 50)
            parameter.Value = str
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@lastname", SqlDbType.VarChar, 50)
            parameter.Value = str
            cmd.Parameters.Add(parameter)

            Using da As New SqlDataAdapter(cmd)
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    Return dt
                Else
                    Return Nothing
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Dispose()
            cmd.Dispose()
            dt.Dispose()
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

    Public Shared Function checkCustomerDepositBeforeDel(ByVal cust_id As String) As Boolean
        Try
            Dim sql As String = String.Format("EXEC getSumPortfolioByCust_id '{0}'", cust_id)
            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)
            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Dim cash As Double = dt.Rows(0)("cash")
                Dim gold96 As Double = dt.Rows(0)("gold96")
                Dim gold99 As Double = dt.Rows(0)("gold99")
                Dim result As Double = cash + gold96 + gold99
                If result <> 0.0 Then
                    Return False
                End If
            End If
            Return True
        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Shared Function checkCustomerBeforeDel(ByVal cust_id As String) As Boolean
        Try
            Dim sql As String = String.Format("select * from tickets WHERE cust_id = '{0}'  and status_id = 101 and active = 'y' ", cust_id)

            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)

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

#Region "Ticket"

    Public Shared Function updateticketOnlineBillOrNobill(ByVal ref_no As String, ByVal bill As String, ByVal update_by As String) As Boolean
        Try
            Dim sql As String = String.Format("UPDATE tickets set billing = '{1}',modifier_by='{2}',modifier_date=getdate() where ref_no = '{0}' ", ref_no, bill, update_by)
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

    Public Shared Function checkDupTicket(ByVal bookNo As String, ByVal newOrUpdate As String, ByVal type As String, ByVal oldType As String) As Boolean
        Dim sql As String = ""
        If newOrUpdate = "new" Then
            sql = "SELECT count(0) from ticket where book_no = @book_no and type = @type and convert(datetime, created_date,101)  >= convert(datetime,dateadd(day,-14, getdate()),101)"
        Else
            sql = "select count(0) from ticket where book_no = @book_no and convert(datetime, created_date,101)  >= convert(datetime,dateadd(day,-14, getdate()),101)"
        End If
        'book_no ห้ามซ้ำภายใน 14 วัน

        Dim con As New SqlConnection(strcon)
        Dim cmd As New SqlCommand(sql, con)
        Try
            Dim parameter As New SqlParameter("book_no", SqlDbType.VarChar, 10)
            parameter.Value = bookNo
            cmd.Parameters.Add(parameter)

            If newOrUpdate = "new" Then
                parameter = New SqlParameter("@type", SqlDbType.VarChar, 10)
                parameter.Value = type
                cmd.Parameters.Add(parameter)
            End If

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)

            If newOrUpdate = "new" Then
                If dt.Rows(0)(0) = 0 Then
                    Return True
                Else
                    Return False
                End If
            Else
                ' Case Update Mode
                ' result = 2 then this ticket have buy and sell >> type = old type
                If dt.Rows(0)(0) = 1 Then
                    Return True
                ElseIf dt.Rows(0)(0) = 2 Then
                    If type = oldType Then
                        Return True
                    Else
                        Return False
                    End If
                Else 'case 0 (update buy change book no)
                    Return True
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function updateActiveTicket(ByVal ref_no As String, ByVal active As Boolean, ByVal update_by As String) As String
        Dim dt As New DataTable
        Try
            Dim yesOrNo As String = ""
            Dim sql As String = ""

            If active Then
                yesOrNo = "y"
                'check cust_id have in database
                sql = String.Format("select 1 from customer where cust_id = (select cust_id from tickets where ref_no = '{0}')", ref_no)
                Using da As New SqlDataAdapter(sql, strcon)
                    da.Fill(dt)
                    If dt.Rows.Count = 0 Then
                        Return "ไม่สามารถเปลี่ยนสถานะเป็น Active ได้เนื่องจากไม่มีลูกค้าคนนี้อยู่ในระบบแล้ว"
                    End If
                End Using
            Else
                yesOrNo = "n"
            End If

            sql = String.Format("UPDATE tickets set active = '{1}',modifier_by='{2}',modifier_date=getdate() where ref_no = '{0}' ", ref_no, yesOrNo, update_by)
            Using con As New SqlConnection(strcon)
                Using cmd As New SqlCommand(sql, con)
                    con.Open()
                    Dim result As Integer = cmd.ExecuteNonQuery()
                    con.Close()
                    If result > 0 Then
                        Return String.Empty
                    Else
                        Return "ไม่สามารถเปลี่ยนสถานะได้"
                    End If
                End Using
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            dt.Dispose()
        End Try
    End Function

    Public Shared Function deleteTicket(ByVal refNo As String) As Boolean

        Try
            Dim con As New SqlConnection(strcon)
            con.Open()
            Dim tr As SqlTransaction = con.BeginTransaction

            Try

                Dim sql As String = "DELETE FROM Tickets WHERE [ref_no] = @ref_no"
                Dim cmd As New SqlCommand(sql, con)

                Dim parameter As New SqlParameter("@ref_no", SqlDbType.VarChar, 20)
                parameter.Value = refNo
                cmd.Parameters.Add(parameter)

                cmd.Transaction = tr
                Dim result As Integer = cmd.ExecuteNonQuery()
                If result > 0 Then
                    tr.Commit()
                    Return True
                End If
                Return False
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

    Public Shared Sub getBank(ByVal ddl As DropDownList)

        Dim sql As String = "SELECT * FROM BANK ORDER BY bank_name "
        Dim con As New SqlConnection(strcon)
        Try
            Dim da As New SqlDataAdapter(sql, con)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                ddl.DataSource = dt
                ddl.DataTextField = "bank_name"
                ddl.DataValueField = "bank_id"
                ddl.DataBind()
                ddl.Items.Insert(0, New ListItem(clsManage.msgRequireSelect, ""))
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Function getTicketRunNo() As String

        Dim sql As String = "select max(run_no)+1,stringWord from ticket_runno " & _
                            "where convert(varchar,dateadd(day,0,datetime), 101)= convert(varchar,getdate(), 101) and type = 'ticket'  group by stringWord"
        Dim con As New SqlConnection(strcon)
        Try
            Dim da As New SqlDataAdapter(sql, con)
            Dim dt As New DataTable
            da.Fill(dt)
            Dim result As Integer = 1
            Dim stringWord As String = clsFng.strCall
            If dt.Rows.Count > 0 Then
                If dt.Rows(0)(0).ToString <> "" Then
                    result = dt.Rows(0)(0).ToString
                    'stringWord = dt.Rows(0)(1).ToString
                End If
            End If

            sql = String.Format("UPDATE ticket_runno set run_no = {0},datetime = getdate() where type = 'ticket'", result)

            con.Open()
            Dim tr As SqlTransaction = con.BeginTransaction
            Try
                Dim cmd As New SqlCommand(sql, con)
                cmd.Transaction = tr
                Dim resultUpdate As Integer = cmd.ExecuteNonQuery()
                If resultUpdate > 0 Then
                    tr.Commit()
                End If
                Return stringWord + DateTime.Now.ToString("ddMMyy/") + result.ToString("0000")
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

    Public Shared Function getTicket(ByVal ticket_id As String, Optional ByVal cust_id As String = "", Optional ByVal type As String = "", Optional ByVal billing As String = "", Optional ByVal ticket_date1 As String = "", Optional ByVal ticket_date2 As String = "", Optional ByVal del_date1 As String = "", Optional ByVal del_date2 As String = "") As DataTable
        Dim sql_ticket_date As String = ""
        If ticket_date1 <> "" AndAlso ticket_date2 <> "" Then
            sql_ticket_date = "AND (ticket_date BETWEEN  @ticket_date1 AND @ticket_date2)"
        End If

        Dim sql_del_date As String = ""
        If del_date1 <> "" AndAlso del_date2 <> "" Then
            sql_del_date = "AND (delivery_date BETWEEN  @del_date1 AND @del_date2)"
        End If


        Dim sql As String = "SELECT ref_no as ticket_id,book_no,run_no, cust_id, ticket.user_id,ticket.gold_type_id,gold_type_name, type, case when substring(cast(ticket.gold_type_id as varchar),1,3)='G99' then cast((ticket.quantity * 65.6) as varchar) else ticket.quantity end as quantity,  " & _
                            " price, amount, delivery_date,delivery_date_null, ticket_date, billing, checker, authorize, remark, payment, payment_detail, delivery, ticket.status_id, users.user_name, bank.bank_name, payment_bank, payment_duedate, payment_cheq_no, created_by, active, gold_dep, ticket_status.status_name, invoice, sp_quan,trade,clearingday,payment_id " & _
                            " FROM   ticket LEFT OUTER JOIN  bank ON ticket.payment_bank = bank.bank_id LEFT OUTER JOIN  users ON ticket.user_id = users.user_id LEFT OUTER JOIN  gold_type ON ticket.gold_type_id = gold_type.gold_type_id LEFT OUTER JOIN ticket_status ON ticket.status_id = ticket_status.status_id" & _
                            " Where (ref_no = @ref_no or @ref_no = '') " & _
                            " AND (cust_id =  @cust_id or @cust_id = '') " & _
                            " AND (type =  @type or @type = '' ) " & _
                            " AND (billing =  @billing or @billing='' ) " & _
                            " " + sql_ticket_date + " " & _
                            " " + sql_del_date + " " & _
                            "ORDER BY ticket_id DESC"
        '" OR (price like '%' + '" + keyword + "' + '%' or @Address='')" & _


        Dim con As New SqlConnection(strcon)
        Dim cmd As New SqlCommand(sql, con)
        Try
            Dim parameter As New SqlParameter("@ref_no", SqlDbType.VarChar, 50)
            parameter.Value = ticket_id
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@cust_id", SqlDbType.VarChar, 5)
            parameter.Value = cust_id
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@type", SqlDbType.VarChar, 5)
            parameter.Value = type
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@billing", SqlDbType.VarChar, 5)
            parameter.Value = billing
            cmd.Parameters.Add(parameter)

            If ticket_date1 <> "" AndAlso ticket_date2 <> "" Then

                parameter = New SqlParameter("@ticket_date1", SqlDbType.DateTime)
                parameter.Value = DateTime.ParseExact(ticket_date1, clsManage.formatDateTime, Nothing)
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@ticket_date2", SqlDbType.DateTime)
                parameter.Value = DateTime.ParseExact(ticket_date2, clsManage.formatDateTime, Nothing)
                cmd.Parameters.Add(parameter)

            End If

            If del_date1 <> "" AndAlso del_date2 <> "" Then

                parameter = New SqlParameter("@del_date1", SqlDbType.DateTime)
                parameter.Value = DateTime.ParseExact(del_date1, clsManage.formatDateTime, Nothing)
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@del_date2", SqlDbType.DateTime)
                parameter.Value = DateTime.ParseExact(del_date2, clsManage.formatDateTime, Nothing)
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

    Public Shared Function getTicketStock(ByVal ticket_id As String, Optional ByVal cust_name As String = "", Optional ByVal type As String = "", Optional ByVal billing As String = "", Optional ByVal status_id As String = "", Optional ByVal ticket_date1 As String = "", Optional ByVal ticket_date2 As String = "", Optional ByVal del_date1 As String = "", Optional ByVal del_date2 As String = "", Optional isCenter As String = "") As DataTable
        Dim sql_ticket_date As String = ""
        If ticket_date1 <> "" AndAlso ticket_date2 <> "" Then
            sql_ticket_date = "AND (convert(varchar, ticket_date,111) BETWEEN  @ticket_date1 AND @ticket_date2)"
        End If

        Dim sql_del_date As String = ""
        If del_date1 <> "" AndAlso del_date2 <> "" Then
            sql_del_date = "AND (convert(varchar, delivery_date,111) BETWEEN  @del_date1 AND @del_date2)"
        End If

        Dim sqlCenter As String = ""
        If isCenter = "y" Then
            sqlCenter = "AND (trade IS NULL)"
        ElseIf isCenter = "n" Then
            sqlCenter = "AND (trade = 'y')"
        End If

        Dim sql As String = " select  ref_no as ticket_id,run_no, customer.cust_id, i_ticket.user_id,i_ticket.gold_type_id,gold_type_name, type, amount,trade," & _
" price, delivery_date, ticket_date, billing, checker, authorize, i_ticket.remark, payment, payment_detail, delivery, status_id, invoice,payment_id, " & _
" users.user_name, bank.bank_name, payment_bank, payment_duedate, payment_cheq_no, i_ticket.created_by,userUpdate.user_name as modifier_by, customer.firstname,sp_quan, gold_dep,quantity," & _
" case when i_ticket.gold_type_id ='96G' then quantity else '0' end as quan96G," & _
" case when i_ticket.gold_type_id ='96' then quantity else '0' end as quan96," & _
" case when i_ticket.gold_type_id ='99' then quantity else '0' end as quan99 " & _
" from v_ticket_sum_split as i_ticket" & _
" LEFT OUTER JOIN  customer ON i_ticket.cust_id = customer.cust_id " & _
" LEFT OUTER JOIN  bank ON i_ticket.payment_bank = bank.bank_id " & _
" LEFT OUTER JOIN   users ON i_ticket.user_id = users.user_id LEFT OUTER JOIN   gold_type ON i_ticket.gold_type_id = gold_type.gold_type_id" & _
" LEFT OUTER JOIN   users userUpdate ON i_ticket.modifier_by = userUpdate.user_id " & _
" Where (ref_no = @ref_no or @ref_no = '') " & _
" AND (customer.firstname  like '%' + @firstname + '%' or @firstname = '') " & _
" AND (type =  @type or @type = '' ) " & _
" AND (billing =  @billing or @billing='' ) " & _
" AND (status_id =  @status_id or @status_id='' ) " & _
" " + sql_ticket_date + " " & _
" " + sql_del_date + " " & _
" " + sqlCenter + " " & _
"ORDER BY i_ticket.created_date DESC"
        '" OR (price like '%' + '" + keyword + "' + '%' or @Address='')" & _


        Dim con As New SqlConnection(strcon)
        Dim cmd As New SqlCommand(sql, con)
        Try
            Dim parameter As New SqlParameter("@ref_no", SqlDbType.VarChar, 50)
            parameter.Value = ticket_id
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@firstname", SqlDbType.VarChar, 50)
            parameter.Value = cust_name
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@type", SqlDbType.VarChar, 5)
            parameter.Value = type
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@billing", SqlDbType.VarChar, 5)
            parameter.Value = billing
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@status_id", SqlDbType.VarChar, 5)
            parameter.Value = status_id
            cmd.Parameters.Add(parameter)

            If ticket_date1 <> "" AndAlso ticket_date2 <> "" Then

                parameter = New SqlParameter("@ticket_date1", SqlDbType.DateTime)
                parameter.Value = DateTime.ParseExact(ticket_date1, clsManage.formatDateTime, Nothing)
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@ticket_date2", SqlDbType.DateTime)
                parameter.Value = DateTime.ParseExact(ticket_date2, clsManage.formatDateTime, Nothing)
                cmd.Parameters.Add(parameter)

            End If

            If del_date1 <> "" AndAlso del_date2 <> "" Then

                parameter = New SqlParameter("@del_date1", SqlDbType.DateTime)
                parameter.Value = DateTime.ParseExact(del_date1, clsManage.formatDateTime, Nothing)
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@del_date2", SqlDbType.DateTime)
                parameter.Value = DateTime.ParseExact(del_date2, clsManage.formatDateTime, Nothing)
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

    Public Shared Function getTicketList(ByVal ticket_id As String, ByVal book_no As String, ByVal run_no As String, ByVal cust_name As String, ByVal type As String, ByVal billing As String, ByVal status_id As String, ByVal gold_type_id As String, ByVal ticket_date1 As String, ByVal ticket_date2 As String, ByVal del_date1 As String, ByVal del_date2 As String, ByVal created_by As String, ByVal amount As String) As DataTable
        Dim sql_ticket_date As String = ""
        If ticket_date1 <> "" AndAlso ticket_date2 <> "" Then
            sql_ticket_date = "AND (ticket_date BETWEEN  @ticket_date1 AND @ticket_date2)"
        End If

        Dim sql_del_date As String = ""
        If del_date1 <> "" AndAlso del_date2 <> "" Then
            sql_del_date = "AND (delivery_date BETWEEN  @del_date1 AND @del_date2)"
        End If

        Dim sql_amount As String = ""
        If amount <> "" Then
            sql_amount = " AND (ticket.amount =  @amount) "
        End If

        Dim sql_gold_type As String = ""
        If gold_type_id <> "" Then
            sql_gold_type = " AND (ticket.amount_" + gold_type_id + " <> 0 ) "
        End If

        Dim sql As String = "SELECT ticket.ref_no AS ticket_id, ticket.cust_id, ticket.user_id, ticket.gold_type_id, ticket.type, CASE WHEN substring(CAST(ticket.gold_type_id AS varchar), 1, 3) = 'G99' THEN CAST((ticket.quantity * 65.6) AS varchar) ELSE ticket.quantity END AS quantity, ticket.price, ticket.amount, ticket.delivery_date, ticket.ticket_date, " & _
                      " ticket.billing, ticket.checker, ticket.authorize, ticket.remark, ticket.payment, ticket.payment_detail, ticket.delivery, ticket_status.status_name, ticket_status.status_id,  " & _
                      " users.user_name, ticket.created_by, ticket.book_no, ticket.run_no, customer.firstname " & _
                      " FROM         ticket LEFT OUTER JOIN " & _
                      " users ON ticket.user_id = users.user_id LEFT OUTER JOIN " & _
                      " ticket_status ON ticket.status_id = ticket_status.status_id LEFT OUTER JOIN " & _
                      " customer ON ticket.cust_id = customer.cust_id " & _
                            " Where (ref_no = @ref_no or @ref_no = '') " & _
                            " AND (book_no =  @book_no or @book_no = '') " & _
                            " AND (run_no =  @run_no or @run_no = '') " & _
                            " AND (customer.firstname  like '%' + @firstname + '%' or @firstname = '') " & _
                            " AND (type =  @type or @type = '' ) " & _
                            " AND (billing =  @billing or @billing='' ) " & _
                            " AND (ticket.status_id =  @status_id or @status_id='' ) " & _
                            " AND (ticket.created_by = @created_by) " & _
                            " " + sql_gold_type + " " & _
                            " " + sql_amount + " " & _
                            " " + sql_ticket_date + " " & _
                            " " + sql_del_date + " " & _
                            "ORDER BY ticket_id DESC"

        Dim con As New SqlConnection(strcon)
        Dim cmd As New SqlCommand(sql, con)
        Try
            Dim parameter As New SqlParameter("@ref_no", SqlDbType.VarChar, 50)
            parameter.Value = ticket_id
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@book_no", SqlDbType.VarChar, 20)
            parameter.Value = book_no
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@run_no", SqlDbType.VarChar, 20)
            parameter.Value = run_no
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@firstname", SqlDbType.VarChar, 5)
            parameter.Value = cust_name
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@type", SqlDbType.VarChar, 5)
            parameter.Value = type
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@billing", SqlDbType.VarChar, 5)
            parameter.Value = billing
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@status_id", SqlDbType.VarChar, 5)
            parameter.Value = status_id
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@gold_type_id", SqlDbType.VarChar, 5)
            parameter.Value = gold_type_id
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@created_by", SqlDbType.VarChar, 10)
            parameter.Value = created_by
            cmd.Parameters.Add(parameter)

            If ticket_date1 <> "" AndAlso ticket_date2 <> "" Then

                parameter = New SqlParameter("@ticket_date1", SqlDbType.DateTime)
                parameter.Value = DateTime.ParseExact(ticket_date1, clsManage.formatDateTime, Nothing)
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@ticket_date2", SqlDbType.DateTime)
                parameter.Value = DateTime.ParseExact(ticket_date2, clsManage.formatDateTime, Nothing)
                cmd.Parameters.Add(parameter)

            End If

            If del_date1 <> "" AndAlso del_date2 <> "" Then

                parameter = New SqlParameter("@del_date1", SqlDbType.DateTime)
                parameter.Value = DateTime.ParseExact(del_date1, clsManage.formatDateTime, Nothing)
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@del_date2", SqlDbType.DateTime)
                parameter.Value = DateTime.ParseExact(del_date2, clsManage.formatDateTime, Nothing)
                cmd.Parameters.Add(parameter)

            End If

            If amount <> "" Then
                parameter = New SqlParameter("@amount", SqlDbType.Decimal)
                parameter.Value = amount
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

    Public Shared Function getTicketReport(ByVal ticket_id As String, ByVal cust_name As String, ByVal type As String, ByVal billing As String, ByVal status_id As String, ByVal gold_type_id As String, ByVal ticket_date1 As String, ByVal ticket_date2 As String, ByVal del_date1 As String, ByVal del_date2 As String, ByVal created_by As String, ByVal amount As String, ByVal price As String, ByVal quantity As String, ByVal active As String, ByVal isCenter As String) As DataTable

        Dim sqlCenter As String = ""
        If isCenter = "y" Then
            sqlCenter = "AND ticket.trade is NULL "
        ElseIf isCenter = "n" Then
            sqlCenter = "AND ticket.trade = 'y' "
        End If

        Dim sql_ticket_date As String = ""
        If ticket_date1 <> "" And ticket_date2 <> "" Then
            sql_ticket_date = "AND (convert(varchar, ticket_date,111) BETWEEN  @ticket_date1 AND @ticket_date2)"
        ElseIf ticket_date1 <> "" And ticket_date2 = "" Then
            sql_ticket_date = "AND (convert(varchar, ticket_date,111) >=  @ticket_date1)"
        ElseIf ticket_date1 = "" And ticket_date2 <> "" Then
            sql_ticket_date = "AND (convert(varchar, ticket_date,111) <=  @ticket_date2)"
        End If

        Dim sql_del_date As String = ""
        If del_date1 <> "" And del_date2 <> "" Then
            sql_del_date = "AND ( convert(varchar,delivery_date,111) BETWEEN  @del_date1 AND @del_date2)"
        ElseIf del_date1 <> "" And del_date2 = "" Then
            sql_del_date = "AND (convert(varchar,delivery_date,111) >=  @del_date1)"
        ElseIf del_date1 = "" And del_date2 <> "" Then
            sql_del_date = "AND (convert(varchar,delivery_date,111) <=  convert(varchar,@del_date2,111))"
        End If


        If del_date1 <> "" AndAlso del_date2 <> "" Then
            sql_del_date = "AND ( convert(varchar,delivery_date,111) BETWEEN  @del_date1 AND @del_date2)"
        End If

        Dim sql_status As String = ""
        If status_id <> "" Then
            sql_status = " AND (ticket.status_id in('" + status_id + "') ) "
        End If

        Dim sql_amount As String = ""
        If amount <> "" Then
            sql_amount = " AND (ticket.amount =  @amount) "
        End If

        Dim sql_price As String = ""
        If price <> "" Then
            sql_price = " AND (ticket.price =  @price) "
        End If

        Dim sql_quantity As String = ""
        If quantity <> "" Then
            sql_quantity = " AND (ticket.quantity =  @quantity) "
        End If

        Dim sql_purity As String = ""
        If gold_type_id <> "" Then
            sql_purity = " AND (ticket.gold_type_id in('" + gold_type_id + "')) "
        End If

        'Dim sql_team As String = ""
        'If team_id <> "" Then
        '    If team_id = "none" Then
        '        sql_team = "AND (customer.team_id = '' or customer.team_id is null) "
        '    Else
        '        sql_team = "AND (customer.team_id = " + team_id + ") "
        '    End If
        'End If

        Dim sql_name As String = ""
        If cust_name <> "" Then
            Dim str As String = ""
            For i As Integer = 0 To cust_name.Split("&").Length - 1
                str = clsManage.checkSingleQuote(cust_name.Split("&")(i))
                If sql_name = "" Then
                    sql_name = String.Format("AND (customer.firstname  like '%' + '{0}' + '%' ", str)
                Else
                    sql_name += String.Format("OR customer.firstname  like '%' + '{0}' + '%' ", str)
                End If
            Next
            sql_name += ")"
        End If

        'view "v_ticket_report" for report case add active for search (add deleted)
        Dim sql As String = " SELECT     ticket.ref_no AS ticket_id, ticket.cust_id, ticket.user_id, ticket.gold_type_id, ticket.type, ticket.quantity, ticket.price, ticket.amount, ticket.delivery_date, ticket.ticket_date, " & _
                      " case when ticket.gold_type_id ='96G' then quantity else '0' end as quan96G, " & _
                      " case when ticket.gold_type_id ='96' then quantity else '0' end as quan96, " & _
                      " case when ticket.gold_type_id ='99' then quantity else '0' end as quan99, " & _
                      " ticket.billing, ticket.remark, ticket.payment, ticket.payment_detail, ticket.delivery, users.user_name, ticket.created_by, " & _
        " ticket.run_no, customer.firstname, gold_type.gold_type_name,ticket.status_id,ticket_status.status_name " & _
        " FROM        v_ticket_report ticket INNER JOIN " & _
                      " gold_type ON ticket.gold_type_id = gold_type.gold_type_id LEFT OUTER JOIN " & _
                      " users ON ticket.user_id = users.user_id LEFT OUTER JOIN " & _
                      " customer ON ticket.cust_id = customer.cust_id LEFT OUTER JOIN " & _
                      " ticket_status ON ticket.status_id = ticket_status.status_id " & _
                            " Where (ref_no = @ref_no or @ref_no = '') " & _
                            " " + sql_name + " " & _
                            " AND (type =  @type or @type = '' ) " & _
                            " AND (billing =  @billing or @billing='' ) " & _
                            " " + sql_status + " " & _
                            " " + sql_purity + " " & _
                            " " + sql_amount + " " & _
                            " " + sql_price + " " & _
                            " " + sql_quantity + " " & _
                            " " + sql_ticket_date + " " & _
                            " " + sql_del_date + " " & _
                            " " + sqlCenter + " " & _
                            " AND (active =  @active ) " & _
                            "ORDER BY ticket_date desc,ticket_id desc"

        '" AND (ticket.created_by = @created_by) " & _
        Dim con As New SqlConnection(strcon)
        Dim cmd As New SqlCommand(sql, con)
        Try
            Dim parameter As New SqlParameter("@ref_no", SqlDbType.VarChar, 50)
            parameter.Value = ticket_id
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@active", SqlDbType.VarChar, 1)
            parameter.Value = active
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@type", SqlDbType.VarChar, 5)
            parameter.Value = type
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@billing", SqlDbType.VarChar, 5)
            parameter.Value = billing
            cmd.Parameters.Add(parameter)

            'parameter = New SqlParameter("@status_id", SqlDbType.VarChar, 5)
            'parameter.Value = "'" + status_id + "'"
            'cmd.Parameters.Add(parameter)

            'parameter = New SqlParameter("@gold_type_id", SqlDbType.VarChar, 5)
            'parameter.Value = gold_type_id
            'cmd.Parameters.Add(parameter)

            'parameter = New SqlParameter("@created_by", SqlDbType.VarChar, 50)
            'parameter.Value = created_by
            'cmd.Parameters.Add(parameter)

            If ticket_date1 <> "" Then
                parameter = New SqlParameter("@ticket_date1", SqlDbType.DateTime)
                parameter.Value = DateTime.ParseExact(ticket_date1, clsManage.formatDateTime, Nothing)
                cmd.Parameters.Add(parameter)
            End If
            If ticket_date2 <> "" Then
                parameter = New SqlParameter("@ticket_date2", SqlDbType.DateTime)
                parameter.Value = DateTime.ParseExact(ticket_date2, clsManage.formatDateTime, Nothing)
                cmd.Parameters.Add(parameter)
            End If

            If del_date1 <> "" Then
                parameter = New SqlParameter("@del_date1", SqlDbType.DateTime)
                parameter.Value = DateTime.ParseExact(del_date1, clsManage.formatDateTime, Nothing)
                cmd.Parameters.Add(parameter)
            End If
            If del_date2 <> "" Then
                parameter = New SqlParameter("@del_date2", SqlDbType.DateTime)
                parameter.Value = DateTime.ParseExact(del_date2, clsManage.formatDateTime, Nothing)
                cmd.Parameters.Add(parameter)
            End If

            If amount <> "" Then
                parameter = New SqlParameter("@amount", SqlDbType.Decimal)
                parameter.Value = amount
                cmd.Parameters.Add(parameter)
            End If

            If price <> "" Then
                parameter = New SqlParameter("@price", SqlDbType.Decimal)
                parameter.Value = price
                cmd.Parameters.Add(parameter)
            End If

            If quantity <> "" Then
                parameter = New SqlParameter("@quantity", SqlDbType.Decimal)
                parameter.Value = quantity
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

    Public Shared Function getTicketView(ByVal ticket_id As String, ByVal cust_id As String, ByVal type As String, ByVal billing As String, ByVal status_id As String, ByVal gold_type_id As String, ByVal ticket_date1 As String, ByVal ticket_date2 As String, ByVal del_date1 As String, ByVal del_date2 As String) As DataTable
        Dim sql_ticket_date As String = ""
        If ticket_date1 <> "" AndAlso ticket_date2 <> "" Then
            sql_ticket_date = "AND (ticket_date BETWEEN  @ticket_date1 AND @ticket_date2)"
        End If

        Dim sql_del_date As String = ""
        If del_date1 <> "" AndAlso del_date2 <> "" Then
            sql_del_date = "AND (delivery_date BETWEEN  @del_date1 AND @del_date2)"
        End If


        'Dim sql As String = "SELECT     ticket.ref_no AS ticket_id, ticket.cust_id, ticket.user_id, ticket.gold_type_id, gold_type.gold_type_name, ticket.type, case when substring(cast(ticket.gold_type_id as varchar),1,3)='G99' then cast((ticket.quantity * 65.6) as varchar) else ticket.quantity end as quantity , " & _
        '                    " ticket.price, ticket.amount, ticket.delivery_date, ticket.ticket_date, ticket.billing, ticket.checker, ticket.authorize, ticket.remark, ticket.payment, ticket.payment_detail, ticket.stock_gold, " & _
        '                    " ticket.stock_money, ticket.delivery, ticket_status.status_name, ticket_status.status_id,users.user_name" & _
        '                    " FROM         ticket LEFT OUTER JOIN" & _
        '                    " users ON ticket.user_id = users.user_id LEFT OUTER JOIN" & _
        '                    " ticket_status ON ticket.status_id = ticket_status.status_id LEFT OUTER JOIN" & _
        '                    " gold_type ON ticket.gold_type_id = gold_type.gold_type_id" & _



        Dim sql As String = "SELECT     ticket.ref_no AS ticket_id, ticket.cust_id, ticket.user_id, ticket.gold_type_id, gold_type.gold_type_name, ticket.type, case when substring(cast(ticket.gold_type_id as varchar),1,3)='G99' then cast((ticket.quantity * 65.6) as varchar) else ticket.quantity end as quantity ,  " & _
" ticket.price,case when spAmount IS NULL then ticket.amount else spAmount end as amount " & _
" , ticket.delivery_date, ticket.ticket_date, ticket.billing, ticket.checker, ticket.authorize, ticket.remark, ticket.payment, ticket.payment_detail,  " & _
" ticket.delivery, ticket_status.status_name, ticket_status.status_id, users.user_name, spAmount " & _
" FROM         ticket LEFT OUTER JOIN " & _
" users ON ticket.user_id = users.user_id LEFT OUTER JOIN " & _
" ticket_status ON ticket.status_id = ticket_status.status_id LEFT OUTER JOIN " & _
" gold_type ON ticket.gold_type_id = gold_type.gold_type_id  LEFT OUTER JOIN " & _
"       (SELECT     ref_no, SUM(amount) AS spAmount " & _
"         FROM ticket_split " & _
"         GROUP BY ref_no) AS ticket_split ON ticket.ref_no = ticket_split.ref_no" & _
                            " Where (ticket.ref_no = @ref_no or @ref_no = '') " & _
                            " AND (ticket.cust_id =  @cust_id or @cust_id = '') " & _
                            " AND (ticket.type =  @type or @type = '' ) " & _
                            " AND (ticket.billing =  @billing or @billing='' ) " & _
                            " AND (ticket.status_id =  @status_id or @status_id='' ) " & _
                            " AND (ticket.gold_type_id =  @gold_type_id or @gold_type_id='' ) " & _
                            " " + sql_ticket_date + " " & _
                            " " + sql_del_date + " " & _
                            "ORDER BY ticket_id DESC"
        '" OR (price like '%' + '" + keyword + "' + '%' or @Address='')" & _


        Dim con As New SqlConnection(strcon)
        Dim cmd As New SqlCommand(sql, con)
        Try
            Dim parameter As New SqlParameter("@ref_no", SqlDbType.VarChar, 50)
            parameter.Value = ticket_id
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@cust_id", SqlDbType.VarChar, 5)
            parameter.Value = cust_id
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@type", SqlDbType.VarChar, 5)
            parameter.Value = type
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@billing", SqlDbType.VarChar, 5)
            parameter.Value = billing
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@status_id", SqlDbType.VarChar, 5)
            parameter.Value = status_id
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@gold_type_id", SqlDbType.VarChar, 5)
            parameter.Value = gold_type_id
            cmd.Parameters.Add(parameter)

            If ticket_date1 <> "" AndAlso ticket_date2 <> "" Then

                parameter = New SqlParameter("@ticket_date1", SqlDbType.DateTime)
                parameter.Value = DateTime.ParseExact(ticket_date1, clsManage.formatDateTime, Nothing)
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@ticket_date2", SqlDbType.DateTime)
                parameter.Value = DateTime.ParseExact(ticket_date2, clsManage.formatDateTime, Nothing)
                cmd.Parameters.Add(parameter)

            End If

            If del_date1 <> "" AndAlso del_date2 <> "" Then

                parameter = New SqlParameter("@del_date1", SqlDbType.DateTime)
                parameter.Value = DateTime.ParseExact(del_date1, clsManage.formatDateTime, Nothing)
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@del_date2", SqlDbType.DateTime)
                parameter.Value = DateTime.ParseExact(del_date2, clsManage.formatDateTime, Nothing)
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
                'sql = String.Format(" select ticket_refno as ref_no, datetime, case when type = 'deposit' then quantity else 0 end as deposit, " & _
                '" case when type = 'withdraw' then quantity else 0 end as withdraw,gold_type_id from customer_trans   " & _
                '" where gold_type_id <> '' and cust_id = '{0}'  " & _
                '" UNION ALL SELECT ticket_split.ref_no,   max(ticket_split.created_date)  as datetime, SUM(ticket_split.quantity) AS deposit,0 as withdraw,gold_type_id  FROM  ticket_split  " & _
                '" INNER JOIN ticket ON ticket_split.ref_no = ticket.ref_no   " & _
                '" WHERE ticket_split.type = 'DepositGold' and ticket.cust_id = '{0}' and ticket.status_id = '101' " & _
                '" GROUP BY ticket_split.ref_no, ticket.cust_id,gold_type_id ", cust_id)
                sql = String.Format(" select ticket_refno as ref_no, datetime, case when type = 'deposit' then quantity else 0 end as deposit, " & _
               " case when type = 'withdraw' then quantity else 0 end as withdraw,gold_type_id from customer_trans   " & _
               " where gold_type_id <> '' and cust_id = '{0}'  " & _
               " ", cust_id)

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

    Public Shared Function getSumQuanTicketPortfolioByCust_id(ByVal cust_id As String) As DataTable
        Dim dt As New DataTable
        Try
            Dim sql As String = String.Format("[getSumPortfolioByCust_id] '{0}'", cust_id)
            Using da As New SqlDataAdapter(sql, strcon)
                da.Fill(dt)
                If dt.Rows.Count > 0 AndAlso dt.Rows(0)("self_price").ToString = clsManage.no Then
                    Dim db As New dcDBDataContext()
                    Dim usr = From username In db.usernames Where username.cust_id = cust_id
                    If usr.Count > 0 Then
                        Dim sto As New clsStore()
                        sto = clsStore.getPriceStore(cust_id)
                        dt.Rows(0)("bid99") = sto.bid99
                        dt.Rows(0)("ask99") = sto.ask99
                        dt.Rows(0)("bid96") = sto.bid96
                        dt.Rows(0)("ask96") = sto.ask96
                    End If
                End If
                Return dt
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            dt.Dispose()
        End Try
    End Function

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

    Public Shared Function getTicketPortfolioByCust_id(ByVal cust_id As String, ByVal gold_type As String, ByVal isHistory As String) As DataTable

        Dim sql_gold_type As String = ""
        If gold_type <> "" Then
            sql_gold_type = String.Format(" AND (ticket.gold_type_id = '{0}') ", gold_type)
        End If

 
        Dim sql_status As String = ""
        Dim sql As String = ""

        If isHistory = "true" Then

            sql_status = "AND status_id in (901,902,903) "

            Dim sql_split As String = " UNION SELECT     ticket.ref_no AS ticket_id, ticket.ticket_date, ticket.book_no, ticket.run_no, " & _
" CASE WHEN ticket.type = 'sell' THEN 'ซื้อ' WHEN ticket.type = 'buy' THEN 'ขาย' ELSE '' END AS type, ticket.created_date, ticket.price, " & _
" CASE WHEN ticket.type = 'sell' THEN sp_quan WHEN ticket.type = 'buy' THEN sp_quan * - 1 ELSE 0 END AS quantity, " & _
" CASE WHEN ticket.type = 'sell' THEN sp_amount * - 1 WHEN ticket.type = 'buy' THEN sp_amount ELSE 0 END AS amount, " & _
" CASE when ticket.payment_duedate is not null then ticket.payment_duedate else ticket_split.created_date end as payment_duedate, " & _
" ticket_split.created_date as delivery_date " & _
" FROM         v_ticket_sum_split AS ticket LEFT OUTER JOIN " & _
" ticket_split ON ticket.ref_no = ticket_split.ref_no and ticket_split.row  in (select max(row) from ticket_split where ref_no = ticket.ref_no) and ticket_split.type= 'Split' " & _
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

    Public Shared Function getTicketDialy(ByVal ticket_id As String, ByVal book_no As String, ByVal run_no As String, ByVal cust_name As String, ByVal gold_type_id As String, ByVal ticket_date As DateTime, ByVal ticket_date2 As DateTime, ByVal del_date1 As String, ByVal del_date2 As String, ByVal price As String, ByVal user_id As String, ByVal team_id As String, ByVal active As String, ByVal status_id As String, ByVal amount As String, ByVal type As String, ByVal isCenter As String) As DataTable

        Dim sqlCenter As String = ""
        If isCenter = "y" Then
            sqlCenter = "AND (ticket.trade IS NULL)"
        ElseIf isCenter = "n" Then
            sqlCenter = "AND (ticket.trade = 'y')"
        End If

        Dim sql_price As String = ""
        If price <> "" Then
            sql_price = " AND (ticket.price =  @price) "
        End If

        Dim sql_amount As String = ""
        If amount <> "" Then
            sql_amount = " AND (ticket.amount =  @amount) "
        End If

        Dim sql_gold_type As String = ""
        If gold_type_id = "" Then
            sql_gold_type = " AND ticket.gold_type_id <> '96G' "
        Else
            sql_gold_type = " AND ticket.gold_type_id = '" + gold_type_id + "' "
        End If

        Dim sql_del_date As String = ""
        If del_date1 <> "" AndAlso del_date2 <> "" Then
            sql_del_date = "AND (convert(varchar, delivery_date,111) BETWEEN  @del_date1 AND @del_date2)"
        End If

        Dim sql_name As String = ""
        If cust_name <> "" Then
            Dim str As String = ""
            For i As Integer = 0 To cust_name.Split("&").Length - 1
                str = clsManage.checkSingleQuote(cust_name.Split("&")(i))
                If sql_name = "" Then
                    sql_name = String.Format("AND (customer.firstname  like '%' + '{0}' + '%' ", str)
                Else
                    sql_name += String.Format("OR customer.firstname  like '%' + '{0}' + '%' ", str)
                End If
            Next
            sql_name += ")"
        End If

        Dim sql_team As String = ""
        If team_id <> "" Then
            If team_id = "none" Then
                sql_team = "AND (customer.team_id = '' or customer.team_id is null) "
            Else
                sql_team = "AND (customer.team_id = " + team_id + ") "
            End If
        End If

        Dim sql_status As String = ""
        If status_id <> "" Then
            sql_status = " AND (ticket.status_id in('" + status_id + "') ) "
        End If


        Dim sql As String = "SELECT ref_no as ticket_id,ticket_date,book_no,run_no,customer.firstname,ticket.cust_id," & _
                            " case when substring(cast(ticket.gold_type_id as varchar),1,2)='96' then '96.50' else '99.99' end as gold_type_name, " & _
                            " case when type = 'buy' then cast( ticket.price as varchar) else '' end as buy_price," & _
                            " case when type = 'buy' then cast( ticket.quantity as varchar) else '' end as buy_quantity," & _
                            " case when type = 'buy' then cast( ticket.amount as varchar) else '' end as buy_amount," & _
                            " case when type = 'sell' then cast( ticket.price as varchar) else '' end as sell_price," & _
                            " case when type = 'sell' then cast( ticket.quantity as varchar) else '' end as sell_quantity," & _
                            " case when type = 'sell' then cast( ticket.amount as varchar) else '' end as sell_amount,users.user_id" & _
                            " ,delivery_date,type " & _
                            " FROM tickets ticket left outer join customer on ticket.cust_id = customer.cust_id left outer join users ON ticket.user_id = users.user_id" & _
                            " Where (ref_no = @ref_no or @ref_no = '') " & _
                            " " + sql_name + " " & _
                            " AND (ticket.user_id =  @user_id or @user_id = '') " & _
                            " AND (active =  @active ) " & _
                            " AND (type =  @type or @type = '' ) " & _
                            " " + sql_team + " " & _
                            " " + sql_gold_type + " " & _
                            " " + sql_price + " " & _
                            " " + sql_amount + " " & _
                            " " + sql_status + " " & _
                            " " + sqlCenter + " " & _
                            " AND ( convert(datetime, ticket_date,111) BETWEEN  @ticket_date AND @ticket_date2)" & _
                            " " + sql_del_date + " " & _
                            " ORDER BY ticket_date desc "

        'change Requirment order by New
        '" ORDER BY gold_type_id desc,type,ticket.price "

        Dim con As New SqlConnection(strcon)
        Dim cmd As New SqlCommand(sql, con)
        Try
            Dim parameter As New SqlParameter("@ref_no", SqlDbType.VarChar, 50)
            parameter.Value = ticket_id
            cmd.Parameters.Add(parameter)

            'parameter = New SqlParameter("@book_no", SqlDbType.VarChar, 20)
            'parameter.Value = book_no
            'cmd.Parameters.Add(parameter)

            'parameter = New SqlParameter("@run_no", SqlDbType.VarChar, 20)
            'parameter.Value = run_no
            'cmd.Parameters.Add(parameter)

            'parameter = New SqlParameter("@firstname", SqlDbType.VarChar, 5)
            'parameter.Value = cust_name
            'cmd.Parameters.Add(parameter)

            'parameter = New SqlParameter("@gold_type_id", SqlDbType.VarChar, 5)
            'parameter.Value = gold_type_id
            'cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@ticket_date", SqlDbType.DateTime)
            parameter.Value = ticket_date ' DateTime.ParseExact(ticket_date, clsManage.formatDateTime, Nothing)
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@ticket_date2", SqlDbType.DateTime)
            parameter.Value = ticket_date2 'DateTime.ParseExact(ticket_date2, clsManage.formatDateTime, Nothing)
            cmd.Parameters.Add(parameter)


            If del_date1 <> "" AndAlso del_date2 <> "" Then
                parameter = New SqlParameter("@del_date1", SqlDbType.DateTime)
                parameter.Value = DateTime.ParseExact(del_date1, clsManage.formatDateTime, Nothing)
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@del_date2", SqlDbType.DateTime)
                parameter.Value = DateTime.ParseExact(del_date2, clsManage.formatDateTime, Nothing)
                cmd.Parameters.Add(parameter)
            End If

            If price <> "" Then
                parameter = New SqlParameter("@price", SqlDbType.Decimal)
                parameter.Value = price
                cmd.Parameters.Add(parameter)
            End If

            If amount <> "" Then
                parameter = New SqlParameter("@amount", SqlDbType.Decimal)
                parameter.Value = amount
                cmd.Parameters.Add(parameter)
            End If

            parameter = New SqlParameter("@user_id", SqlDbType.VarChar, 10)
            parameter.Value = user_id
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@active", SqlDbType.VarChar, 1)
            parameter.Value = active
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@type", SqlDbType.VarChar, 5)
            parameter.Value = type
            cmd.Parameters.Add(parameter)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable

            da.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getStockDialy(ByVal updateDate As String, ByVal status_id As String, ByVal mode As String, ByVal putity As String) As DataTable
        '******  ดึงมาจาก ticket split แล้ว

        Dim sql_status As String = "901,902,903,104,105"
        If status_id <> "" Then
            sql_status = status_id
        End If

        Dim condition_ticket As String = ""
        Dim condition_dep As String = ""
        Dim condition_inout As String = ""
        If putity <> "" Then
            condition_ticket = " and ticket.gold_type_id = '" + putity + "' "
            condition_dep = " and gold_type_id = '" + putity + "' "
            condition_inout = " and purity = '" + putity + "' "
        End If

        Dim tbl_ticket As String = "SELECT     ticket_id, ref_no, ticket_date, cust_id, price, type," & _
        " case when status_id = 104 then 0 when (status_id = 901 and before_status_id = 105 and day(modifier_date) <> day(payment_date)) then 0 else amount end as amount ," & _
        " case when status_id = 105 then 0 when (status_id = 901 and before_status_id = 104 and day(modifier_date) <> day(payment_date)) then 0 else quantity end as quantity," & _
        " gold_type_id, modifier_date, status_id, payment, delivery_date, created_by, remark, payment_duedate, paid, payment_date" & _
        " FROM v_ticket_all_split ticket WHERE  ticket.status_id in (" + sql_status + ") and ticket.amount <> 0 " + condition_ticket + " "

        Dim sql_ticket As String = " SELECT ticket.ref_no as ticket_id,modifier_date as ticket_date,ticket.cust_id,ticket.price,ticket.type,ticket.amount,modifier_date, " & _
" case when (type = 'sell' and ticket.gold_type_id='99') then cast(quantity as varchar) else '' end as 'sell_99', " & _
" case when (type = 'sell' and ticket.gold_type_id='96') then cast(quantity as varchar) else '' end as 'sell_96', " & _
" case when (type = 'sell' and ticket.gold_type_id='96G') then cast(quantity as varchar) else '' end as 'sell_96G', " & _
" case when (type = 'buy' and ticket.gold_type_id='99') then cast(quantity as varchar) else '' end as 'buy_99', " & _
" case when (type = 'buy' and ticket.gold_type_id='96') then cast(quantity as varchar) else '' end as 'buy_96', " & _
" case when (type = 'buy' and ticket.gold_type_id='96G') then cast(quantity as varchar) else '' end as 'buy_96G', " & _
" case when (type = 'sell' and ticket.payment = 'cash') then cast(amount as varchar) else '' end as 'sell_cash', " & _
" case when (type = 'sell' and ticket.payment = 'trans') then cast(amount as varchar) else '' end as 'sell_trans', " & _
" case when (type = 'sell' and ticket.payment = 'cheq') then cast(amount as varchar) else '' end as 'sell_cheq', " & _
" case when (type = 'buy' and ticket.payment = 'cash') then cast(amount as varchar) else '' end as 'buy_cash', " & _
" case when (type = 'buy' and ticket.payment = 'trans') then cast(amount as varchar) else '' end as 'buy_trans', " & _
" case when (type = 'buy' and ticket.payment = 'cheq') then cast(amount as varchar) else '' end as 'buy_cheq' " & _
" FROM ( " & tbl_ticket & ") ticket "
        '" FROM v_ticket_all_split ticket WHERE  ticket.status_id in (" + sql_status + ") and ticket.amount <> 0 " + condition_ticket + " "

        Dim sql_dep As String = " select 'ฝาก/ถอน' as ticket_id,datetime as ticket_date,cust_id,0 as price ,'' as type, 0 as amount,datetime as modifier_date, " & _
" case when (customer_trans.type='Withdraw' and gold_type_id = '99') then  cast(quantity as varchar) else '' end as 'sell_99', " & _
" case when (customer_trans.type='Withdraw' and gold_type_id = '96') then  cast(quantity as varchar) else '' end as 'sell_96', " & _
" case when (customer_trans.type='Withdraw' and gold_type_id = '96G') then  cast(quantity as varchar) else '' end as 'sell_96G', " & _
" case when (customer_trans.type='Deposit' and gold_type_id = '99') then  cast(quantity as varchar) else '' end as 'buy_99', " & _
" case when (customer_trans.type='Deposit' and gold_type_id = '96') then  cast(quantity as varchar) else '' end as 'buy_96', " & _
" case when (customer_trans.type='Deposit' and gold_type_id = '96G') then  cast(quantity as varchar) else '' end as 'buy_96G', " & _
" '' as 'sell_cash', " & _
" '' as 'sell_trans', " & _
" '' as 'sell_cheq', " & _
" '' as 'buy_cash', " & _
" '' as 'buy_trans', " & _
" '' as 'buy_cheq' " & _
" from customer_trans " & _
" where  (pre <> 'y' or pre is null) " + condition_dep + " "

        Dim sql_inout As String = " Select 'In/Out' as ticket_id,datetime as ticket_date,created_by as cust_id,0 as price ,'' as type,quantity as amount, modifier_date,  " & _
" '' as [sell_99],'' as 'sell_96','' as 'sell_96G','' as 'buy_99','' as 'buy_96','' as 'buy_96G', " & _
" case when type = 'In'  then cast(quantity as varchar) else '' end as 'sell_cash',  " & _
"         '' as 'sell_trans', '' as 'sell_cheq',   " & _
" case when type='Out' then cast(quantity as varchar) else '' end as 'buy_cash',   " & _
"         '' as 'buy_trans', '' as 'buy_cheq'   " & _
" from asset  where asset_type = 'cash' and status = 'actual' " + condition_inout + " "

        If mode = "000" Then mode = "111"
        Dim union1 As String = " UNION "
        Dim union2 As String = " UNION "
        If mode.Substring(0, 1) = "0" Then
            sql_ticket = ""
        Else

        End If
        If mode.Substring(1, 1) = "0" Then
            union1 = ""
            sql_dep = ""
        Else
            If sql_ticket = "" Then
                union1 = ""
            End If
        End If
        If mode.Substring(2, 1) = "0" Then
            union2 = ""
            sql_inout = ""
        Else
            If sql_dep = "" And sql_ticket = "" Then
                union2 = ""
            End If
        End If

        Dim sql As String = "" & _
        " SELECT summary_ticket.*,customer.firstname from ( " & _
        " " + sql_ticket + " " & _
        " " + union1 + " " & _
        " " + sql_dep + " " & _
        " " + union2 + " " & _
        " " + sql_inout + " " & _
        " ) summary_ticket " & _
        " left outer join customer on summary_ticket.cust_id = customer.cust_id  " & _
        " Where  convert(varchar,modifier_date,111) = convert(varchar,@modifier_date,111) order by ticket_date desc  "

        Dim con As New SqlConnection(strcon)
        Dim cmd As New SqlCommand(sql, con)
        Try
            Dim Parameter As New SqlParameter("@modifier_date", SqlDbType.DateTime)
            Parameter.Value = DateTime.ParseExact(updateDate, clsManage.formatDateTime, Nothing)
            cmd.Parameters.Add(Parameter)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable

            da.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getTicketViewStock(ByVal ticket_id As String, ByVal cust_id As String, ByVal type As String, ByVal billing As String, ByVal ticket_date1 As String, ByVal ticket_date2 As String, ByVal del_date1 As String, ByVal del_date2 As String) As DataTable
        Dim sql_ticket_date As String = ""
        If ticket_date1 <> "" AndAlso ticket_date2 <> "" Then
            sql_ticket_date = "AND (ticket_date BETWEEN  @ticket_date1 AND @ticket_date2)"
        End If

        Dim sql_del_date As String = ""
        If del_date1 <> "" AndAlso del_date2 <> "" Then
            sql_del_date = "AND (delivery_date BETWEEN  @del_date1 AND @del_date2)"
        End If


        Dim sql As String = "SELECT     ticket.ref_no AS ticket_id, ticket.cust_id, ticket.user_id, ticket.gold_type_id, gold_type.gold_type_name, ticket.type, case when substring(cast(ticket.gold_type_id as varchar),1,3)='G99' then cast((ticket.quantity * 65.6) as varchar) else ticket.quantity end as quantity , ticket.price, ticket.amount, " & _
                            " ticket.delivery_date, ticket.ticket_date, ticket.billing, ticket.checker, ticket.authorize, ticket.remark, ticket.payment, ticket.payment_detail, ticket.stock_gold, " & _
                            " ticket.stock_money, ticket.delivery, ticket_status.status_name, ticket_status.status_id, ticket.payment_date, users.user_name" & _
                            " FROM         ticket LEFT OUTER JOIN" & _
                            " users ON ticket.user_id = users.user_id LEFT OUTER JOIN" & _
                            " ticket_status ON ticket.status_id = ticket_status.status_id LEFT OUTER JOIN" & _
                            " gold_type ON ticket.gold_type_id = gold_type.gold_type_id" & _
                            " Where (ref_no = @ref_no or @ref_no = '') " & _
                            " AND (cust_id =  @cust_id or @cust_id = '') " & _
                            " AND (type =  @type or @type = '' ) " & _
                            " AND (billing =  @billing or @billing='' ) " & _
                            " " + sql_ticket_date + " " & _
                            " " + sql_del_date + " " & _
                            " AND ticket_status.status_id = 999" & _
                            " ORDER BY ticket_id DESC"
        '" OR (price like '%' + '" + keyword + "' + '%' or @Address='')" & _


        Dim con As New SqlConnection(strcon)
        Dim cmd As New SqlCommand(sql, con)
        Try
            Dim parameter As New SqlParameter("@ref_no", SqlDbType.VarChar, 50)
            parameter.Value = ticket_id
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@cust_id", SqlDbType.VarChar, 5)
            parameter.Value = cust_id
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@type", SqlDbType.VarChar, 5)
            parameter.Value = type
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@billing", SqlDbType.VarChar, 5)
            parameter.Value = billing
            cmd.Parameters.Add(parameter)

            If ticket_date1 <> "" AndAlso ticket_date2 <> "" Then

                parameter = New SqlParameter("@ticket_date1", SqlDbType.DateTime)
                parameter.Value = DateTime.ParseExact(ticket_date1, clsManage.formatDateTime, Nothing)
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@ticket_date2", SqlDbType.DateTime)
                parameter.Value = DateTime.ParseExact(ticket_date2, clsManage.formatDateTime, Nothing)
                cmd.Parameters.Add(parameter)

            End If

            If del_date1 <> "" AndAlso del_date2 <> "" Then

                parameter = New SqlParameter("@del_date1", SqlDbType.DateTime)
                parameter.Value = DateTime.ParseExact(del_date1, clsManage.formatDateTime, Nothing)
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@del_date2", SqlDbType.DateTime)
                parameter.Value = DateTime.ParseExact(del_date2, clsManage.formatDateTime, Nothing)
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

    Public Shared Function updateTicket_status(ByVal refno As String, ByVal status_id As String, ByVal modifier_by As String, ByVal type As String, ByVal purity As String, ByVal quantity As Double, ByVal amount As Double, ByVal cust_id As String, ByVal gold_dep As String, ByVal payment As String, ByVal price As Double, ByVal before_status As String, ByVal act As clsActual) As Integer
        Dim con As New SqlConnection(strcon)
        con.Open()
        Dim tr As SqlTransaction = con.BeginTransaction
        Dim gold_type_id As String = ""
        Dim netQuantity As Double = 0
        Dim netAmount As Double = 0
        Dim sql As String = ""
        Dim sql_stock As String = ""
        Dim detail_stock As String = ""

        Try
            Dim cmd As New SqlCommand
            Dim sql_deposit As String = ""
            Dim quantity_stock As Double = 0
            Dim amount_stock As Double = 0
            Dim payment_type As String = payment
            Dim payment_base As Double = 0
            Dim price_base As Double = 0
            Dim G96_base As Double = 0
            Dim G96G_base As Double = 0
            Dim G99_base As Double = 0
            Dim remark As String = ""

            Dim actual_amount As Double = 0 : Dim actual_quan As Double = 0
            Dim dt_stock As New Data.DataTable
            dt_stock = getStockSumDeposit()

            If dt_stock.Rows.Count > 0 Then
                If before_status = "901" Or before_status = "902" Or before_status = "903" Then ' case เคย complete มาแล้ว
                    'case 1
                    If status_id = "101" Or status_id = "901" Then
                        gold_type_id = "G" + purity + "_base"
                        'swep case normal
                        If type = "buy" Then 'Buy : +gold , -cash
                            quantity = quantity * -1
                        Else 'Sell : -gold , +cash
                            amount = amount * -1
                        End If

                        If dt_stock.Rows.Count > 0 Then
                            payment_base = Double.Parse(dt_stock.Rows(0)(payment_type)) + amount
                            amount_stock = Double.Parse(dt_stock.Rows(0)("price_base")) + amount
                            quantity_stock = Double.Parse(dt_stock.Rows(0)(gold_type_id)) + quantity
                        End If
                        sql_stock = String.Format(";update stock set price_base = {0}, {2} = {1},{3} = {4} ", amount_stock, quantity_stock, gold_type_id, payment_type, payment_base)
                        actual_amount = amount
                        actual_quan = quantity
                    End If

                    'case 2
                    'case 902,903 ต้องลบ customer_trans ออก
                    Dim sql_dep As String = ""
                    If status_id = "101" Or status_id = "901" Or status_id = "902" Or status_id = "903" Then
                        If before_status = "902" Or before_status = "903" Then
                            sql_dep = String.Format(";delete from customer_trans where ticket_refno = '{0}'", refno)
                        ElseIf before_status = "901" And (status_id = "902" Or status_id = "903") Then
                            If status_id = "902" Then
                                sql_dep = String.Format(";INSERT INTO [customer_trans] ([cust_id], [datetime], [type], [ticket_refno], [gold_type_id], [quantity], [amount], [price], [remark], [created_by], [created_date]) VALUES " & _
                            "('{0}', getdate(), 'Deposit', '{1}', '{2}', {3}, {4}, {6}, 'Complete Deposit from System.', '{5}', getdate())", cust_id, refno, purity, quantity, amount, modifier_by, price)
                            Else
                                sql_dep = String.Format(";INSERT INTO [customer_trans] ([cust_id], [datetime], [type], [ticket_refno], [gold_type_id], [quantity], [amount], [price], [remark], [created_by], [created_date]) VALUES " & _
                           "('{0}', getdate(), 'Withdraw', '{1}', '{2}', {3}, {4}, {6}, 'ตัดทองฝากจาก {1}.', '{5}', getdate())", cust_id, refno, purity, quantity, amount, modifier_by, price)
                            End If
                        End If
                        sql_stock += sql_dep
                        sql = String.Format("UPDATE tickets SET  status_id ={1},modifier_date=getdate(),modifier_by='{2}',payment_date = NULL WHERE ref_no = '{0}'", refno, status_id, modifier_by)
                    End If

                Else
                    'ไม่เคย complete
                    If status_id = "901" Or status_id = "902" Or status_id = "903" Or status_id = "104" Or status_id = "105" Then
                        Dim gold_type_Dep As String = ""
                        Dim quan_dep As Double = 0
                        Dim amount_dep As Double = 0
                        If type = "sell" Then 'Sell ฝากทอง , buy ฝากเงิน
                            gold_type_Dep = purity
                            quan_dep = quantity
                        Else
                            amount_dep = amount
                        End If

                        If status_id = "902" Then
                            sql_deposit = String.Format(";INSERT INTO [customer_trans] ([cust_id], [datetime], [type], [ticket_refno], [gold_type_id], [quantity], [amount], [price], [remark], [created_by], [created_date]) VALUES " & _
                                                   "('{0}', getdate(), 'Deposit', '{1}', '{2}', {3}, {4}, {6}, 'Complete Deposit from System.', '{5}', getdate())", cust_id, refno, gold_type_Dep, quan_dep, amount_dep, modifier_by, price)
                        End If

                        If status_id = "903" And type = "buy" Then 'case ตัดทองฝาก
                            quan_dep = quantity
                            gold_type_Dep = purity
                            sql_deposit = String.Format(";INSERT INTO [customer_trans] ([cust_id], [datetime], [type], [ticket_refno], [gold_type_id], [quantity], [amount], [price], [remark], [created_by], [created_date]) VALUES " & _
                                                   "('{0}', getdate(), 'Withdraw', '{1}', '{2}', {3}, {4}, {6}, 'ตัดทองฝากจาก {1}.', '{5}', getdate())", cust_id, refno, gold_type_Dep, quan_dep, amount_dep, modifier_by, price)
                        End If

                        gold_type_id = "G" + purity + "_base"
                        If type = "sell" Then 'Sell : -gold , +cash
                            quantity_stock = quantity * -1
                            amount_stock = amount
                        Else 'Buy : +gold , -cash
                            amount_stock = amount * -1
                            quantity_stock = quantity
                        End If

                        actual_amount = amount_stock
                        actual_quan = quantity_stock
                        '************** get Stock

                        For Each dc As DataColumn In dt_stock.Columns
                            detail_stock += "| " + dc.ColumnName + " : " + dt_stock.Rows(0)(dc.ColumnName).ToString
                        Next

                        If status_id = "901" Or status_id = "104" Or status_id = "105" Then 'ตัดทองฝาก ไม่ต้อง บวกในสต๊อกเพิ่ม
                            If purity = "96" Then
                                G96_base += quantity_stock
                            ElseIf purity = "96G" Then
                                G96G_base += quantity_stock
                            ElseIf purity = "99" Then
                                G99_base += quantity_stock
                            End If
                        End If
                        payment_base = Double.Parse(dt_stock.Rows(0)(payment_type)) + amount_stock
                        amount_stock = Double.Parse(dt_stock.Rows(0)("price_base")) + amount_stock '+ before_amount
                        quantity_stock = Double.Parse(dt_stock.Rows(0)(gold_type_id)) + quantity_stock '+ before_quantity
                        '************** get Stock

                        If status_id = "901" Then
                            If before_status = "104" Then 'sell เพิ่ม cash 
                                sql_stock = String.Format(";update stock set price_base = {0},{1} = {2} ", amount_stock, payment_type, payment_base)
                            ElseIf before_status = "105" Then 'buy เพิ่ม gold 
                                sql_stock = String.Format(";update stock set {1} = {0} ", quantity_stock, gold_type_id)
                            Else
                                sql_stock = String.Format(";update stock set price_base = {0}, {2} = {1}, {3} = {4} ", amount_stock, quantity_stock, gold_type_id, payment_type, payment_base)
                            End If
                        ElseIf status_id = "903" Then
                            sql_stock = String.Format(";update stock set price_base = {0}, {2} = {1}, {3} = {4} ", amount_stock, quantity_stock, gold_type_id, payment_type, payment_base) + sql_deposit
                        ElseIf status_id = "902" Then
                            sql_stock = String.Format(";update stock set price_base = {0}, {2} = {1}, {3} = {4} ", amount_stock, quantity_stock, gold_type_id, payment_type, payment_base) + sql_deposit
                        ElseIf status_id = "104" Then 'sell > update gold
                            sql_stock = String.Format(";update stock set {1} = {0} ", quantity_stock, gold_type_id)
                        ElseIf status_id = "105" Then 'buy > update amount
                            '101>105
                            sql_stock = String.Format(";update stock set price_base = {0}, {1}={2} ", amount_stock, payment_type, payment_base)
                        End If

                        If status_id = "901" Or status_id = "902" Or status_id = "903" Then
                            If before_status = "104" Or before_status = "105" Then
                                sql = String.Format("UPDATE tickets SET  status_id ={1},modifier_by='{2}',modifier_date=getdate(),delivery_date=getdate(),before_status_id = {3} WHERE ref_no = '{0}'", refno, status_id, modifier_by, before_status)
                            Else
                                sql = String.Format("UPDATE tickets SET  status_id ={1},modifier_by='{2}',modifier_date=getdate(),delivery_date=getdate(),before_status_id = {3} WHERE ref_no = '{0}'", refno, status_id, modifier_by, before_status)
                            End If
                        Else '104,105 ไม่ต้องใส่ delivery date,payment for ออกเงิน หรือ ออกทอง แล้ว
                            '101>105
                            sql = String.Format("UPDATE tickets SET  status_id ={1},modifier_by='{2}',modifier_date=getdate(),payment_date=getdate(),before_status_id = {3} WHERE ref_no = '{0}'", refno, status_id, modifier_by, before_status)

                            If status_id = "104" Then
                                remark = "ทองออก รอเงิน"
                            ElseIf status_id = "105" Then
                                remark = "เงินออก รอทอง"
                            End If
                        End If

                    Else
                        'case not in 901,902'case อื่นๆ
                        For Each dc As DataColumn In dt_stock.Columns
                            detail_stock += "| " + dc.ColumnName + " : " + dt_stock.Rows(0)(dc.ColumnName).ToString
                        Next

                        price_base = clsManage.convert2zero(dt_stock.Rows(0)("price_base"))
                        G96_base = clsManage.convert2zero(dt_stock.Rows(0)("G96_base"))
                        G96G_base = clsManage.convert2zero(dt_stock.Rows(0)("G96G_base"))
                        G99_base = clsManage.convert2zero(dt_stock.Rows(0)("G99_base"))

                        actual_amount = amount
                        actual_quan = quantity

                        If before_status = "104" Then
                            gold_type_id = "G" + purity + "_base"
                            If purity = "96" Then
                                quantity_stock = G96_base + quantity
                            ElseIf purity = "96G" Then
                                quantity_stock = G96G_base + quantity
                            ElseIf purity = "99" Then
                                quantity_stock = G99_base + quantity
                            End If
                            sql = String.Format("UPDATE tickets SET  status_id ={1},modifier_date=getdate(),modifier_by='{2}',payment_date = NULL,before_status_id = {3} WHERE ref_no = '{0}'", refno, status_id, modifier_by, before_status)
                            sql_stock = String.Format(";update stock set {1} = {0} ", quantity_stock, gold_type_id)
                        ElseIf before_status = "105" Then
                            amount_stock = price_base + amount
                            sql = String.Format("UPDATE tickets SET  status_id ={1},modifier_date=getdate(),modifier_by='{2}',payment_date = NULL,before_status_id = {3} WHERE ref_no = '{0}'", refno, status_id, modifier_by, before_status)
                            sql_stock = String.Format(";update stock set price_base = {0},{1}={2} ", amount_stock, payment_type, payment_base)
                        Else
                            sql = String.Format("UPDATE tickets SET  status_id ={1},modifier_date=getdate(),modifier_by='{2}',before_status_id = {3} WHERE ref_no = '{0}'", refno, status_id, modifier_by, before_status)
                        End If
                    End If
                End If

                cmd = New SqlCommand(sql + sql_stock, con)
                cmd.Transaction = tr
                Dim result As Integer = cmd.ExecuteNonQuery()
                If result > 0 Then
                    'Insert Actual
                    Dim asset_type = "Gold"
                    Dim price_actual As Double = clsManage.convert2zero(dt_stock.Rows(0)("priceDep"))
                    Dim G96 As Double = clsManage.convert2zero(dt_stock.Rows(0)("G96Dep"))
                    Dim G96G As Double = clsManage.convert2zero(dt_stock.Rows(0)("G96GDep"))
                    Dim G99 As Double = clsManage.convert2zero(dt_stock.Rows(0)("G99Dep"))
                    Dim cash_stock As Double = clsManage.convert2zero(dt_stock.Rows(0)("cashDep"))
                    Dim trans_stock As Double = clsManage.convert2zero(dt_stock.Rows(0)("transDep"))
                    Dim cheq_stock As Double = clsManage.convert2zero(dt_stock.Rows(0)("cheqDep"))

                    ''update stock from DB
                    'case ตัดทองฝาก autual ไม่ต้องบวกเพิ่ม (+gold stock ,-gold deposit) หักลบสต๊อก กับทองฝาก ทำให้เหลือเท่าเดิม
                    If Not (status_id = "903" Or before_status = "903" Or status_id = "902" Or before_status = "902") Then
                        Select Case purity
                            Case "96"
                                G96 += actual_quan
                            Case "96G"
                                G96G += actual_quan
                            Case "99"
                                G99 += actual_quan
                        End Select
                    End If

                    Select Case payment_type
                        Case clsManage.payment.cash.ToString
                            cash_stock += actual_amount
                        Case clsManage.payment.trans.ToString
                            trans_stock += actual_amount
                        Case clsManage.payment.cheq.ToString
                            cheq_stock += actual_amount
                    End Select
                    price_actual += actual_amount

                    If act.cash = "n" Then
                        If act.update_gold = "y" Then
                            sql = String.Format(";INSERT INTO [actual] ([asset_id], [ref_no], [asset_type], [created_by], [datetime], [purity], [quantity], [amount], [status_id], [status_name], [type], [order_type], [price_base], [G96_base], [G96G_base], [G99_base], [cust_id], [before_status_id], [note], [payment], [cash], [trans], [cheq]) " & _
                                                              "VALUES ('{0}', '{1}', '{2}', '{3}', getdate(), '{4}', {5}, {6}, '{7}', @status_name, '{9}', '{10}', {11}, {12}, {13}, {14}, '{15}','{16}','{17}','{18}',{19},{20},{21})" & _
                                                              "", act.asset_id, act.ref_no, asset_type, act.created_by, act.purity, act.quantity, act.amount, act.status_id, act.status_name, act.type, act.order_type, price_actual, G96, G96G, G99, act.cust_id, act.before_status_id, act.note, act.payment, cash_stock, trans_stock, cheq_stock)
                        End If
                        If act.update_cash = "y" Then
                            asset_type = "Cash"
                            If act.type = "In" Then
                                act.type = "Out"
                            ElseIf act.type = "ตัดทองฝาก" Then
                                act.type = "Out"
                            Else
                                act.type = "In"
                            End If
                            sql += String.Format(";INSERT INTO [actual] ([asset_id], [ref_no], [asset_type], [created_by], [datetime], [purity], [quantity], [amount], [status_id], [status_name], [type], [order_type], [price_base], [G96_base], [G96G_base], [G99_base], [cust_id], [before_status_id], [note], [payment], [cash], [trans], [cheq]) " & _
                                                          "VALUES ('{0}', '{1}', '{2}', '{3}', getdate(), '{4}', {5}, {6}, '{7}', @status_name, '{9}', '{10}', {11}, {12}, {13}, {14}, '{15}','{16}','{17}','{18}',{19},{20},{21})" & _
                                                          "", act.asset_id, act.ref_no, asset_type, act.created_by, act.purity, act.quantity, act.amount, act.status_id, act.status_name, act.type, act.order_type, price_actual, G96, G96G, G99, act.cust_id, act.before_status_id, act.note, act.payment, cash_stock, trans_stock, cheq_stock)

                        End If
                    Else
                        asset_type = "Cash"

                        sql += String.Format(";INSERT INTO [actual] ([asset_id], [ref_no], [asset_type], [created_by], [datetime], [purity], [quantity], [amount], [status_id], [status_name], [type], [order_type], [price_base], [G96_base], [G96G_base], [G99_base], [cust_id], [before_status_id], [note], [payment], [cash], [trans], [cheq]) " & _
                                                      "VALUES ('{0}', '{1}', '{2}', '{3}', getdate(), '{4}', {5}, {6}, '{7}', @status_name, '{9}', '{10}', {11}, {12}, {13}, {14}, '{15}','{16}','{17}','{18}',{19},{20},{21})" & _
                                                      "", act.asset_id, act.ref_no, asset_type, act.created_by, act.purity, act.quantity, act.amount, act.status_id, act.status_name, act.type, act.order_type, price_actual, G96, G96G, G99, act.cust_id, act.before_status_id, act.note, act.payment, cash_stock, trans_stock, cheq_stock)
                    End If

                    sql += String.Format(";INSERT INTO ticket_log (ref_no,datetime,update_by) VALUES ('{0}',getdate(),'{1}')", refno, modifier_by)
                    cmd.CommandText = sql
                    Dim parameter As New SqlParameter("@status_name", SqlDbType.VarChar)
                    parameter.Value = act.status_name
                    cmd.Parameters.Add(parameter)
                    Dim result_actual As Integer = cmd.ExecuteNonQuery()
                    If result_actual > 0 Then
                        tr.Commit()
                        Return result_actual
                    Else
                        tr.Rollback()
                    End If
                Else
                    tr.Rollback()
                End If
            End If
                Return 0
        Catch ex As Exception
            Dim da As New dsStockOnlineTableAdapters.error_messageTableAdapter
            da.Insert(ex.Message, "Actual", DateTime.Now)
            tr.Rollback()
            Throw ex
        Finally
            '***Check  Don't Update Stock Online 
            Dim da As New dsStockOnlineTableAdapters.error_messageTableAdapter
            If sql_stock = "" Then
                da.Insert(String.Format("Don't Update Stock|sql= {0} |Stock_Detail = {1}", sql + sql_stock, detail_stock), before_status + " > " + status_id, DateTime.Now)
            Else
                da.Insert(String.Format("Update Stock|sql= {0} |Stock_Detail = {1}", sql + sql_stock, detail_stock), before_status + " > " + status_id, DateTime.Now)
            End If
            '***
            con.Close()
        End Try
    End Function

    'not complete
    Public Shared Function updateTicket_status2(ByVal refno As String, ByVal status_id As String, ByVal modifier_by As String, ByVal type As String, ByVal purity As String, ByVal quantity As Double, ByVal amount As Double, ByVal cust_id As String, ByVal gold_dep As String, ByVal payment As String, ByVal price As Double, ByVal before_status As String, ByVal act As clsActual) As Integer
        Dim con As New SqlConnection(strcon)
        con.Open()
        Dim tr As SqlTransaction = con.BeginTransaction
        Dim gold_type_id As String = ""
        Dim netQuantity As Double = 0
        Dim netAmount As Double = 0
        Dim sql As String = ""
        Dim sql_stock As String = ""
        Dim detail_stock As String = ""

        Try
            Dim cmd As New SqlCommand
            Dim sql_deposit As String = ""
            Dim quantity_stock As Double = 0
            Dim amount_stock As Double = 0
            Dim payment_type As String = payment
            Dim payment_base As Double = 0
            Dim price_base As Double = 0
            Dim G96_base As Double = 0
            Dim G99N_base As Double = 0
            Dim G99L_base As Double = 0
            Dim remark As String = ""

            Dim actual_amount As Double = 0 : Dim actual_quan As Double = 0
            Dim dt_stock As New Data.DataTable
            dt_stock = getStockSumDeposit()

            If dt_stock.Rows.Count > 0 Then

                Select Case CInt(before_status)
                    Case 901 To 903 'case เคย complete มาแล้ว
                        gold_type_id = "G" + purity + "_base"
                        'swep case normal
                        'Buy : +gold , -cash
                        'Sell : -gold , +cash
                        If type = "buy" Then
                            quantity = quantity * -1
                        Else
                            amount = amount * -1
                        End If
                        payment_base = Double.Parse(dt_stock.Rows(0)(payment_type)) + amount
                        amount_stock = Double.Parse(dt_stock.Rows(0)("price_base")) + amount
                        quantity_stock = Double.Parse(dt_stock.Rows(0)(gold_type_id)) + quantity
                        sql_stock = String.Format(";update stock set price_base = {0}, {2} = {1},{3} = {4} ", amount_stock, quantity_stock, gold_type_id, payment_type, payment_base)
                        actual_amount = amount
                        actual_quan = quantity

                        Dim sql_dep As String = ""
                        If status_id = "902" Or status_id = "903" Then
                            sql_dep = String.Format(";delete from customer_trans where ticket_refno = '{0}'", refno)
                        End If
                        sql_stock += sql_dep
                        sql = String.Format("UPDATE tickets SET  status_id ={1},modifier_date=getdate(),modifier_by='{2}',payment_date = NULL WHERE ref_no = '{0}'", refno, status_id, modifier_by)

                    Case 101  'case 101 >> 901,902,903,104,105
                        Dim gold_type_Dep As String = ""
                        Dim quan_dep As Double = 0
                        Dim amount_dep As Double = 0
                        If type = "sell" Then 'Sell ฝากทอง , buy ฝากเงิน
                            gold_type_Dep = purity
                            quan_dep = quantity
                        Else
                            amount_dep = amount
                        End If

                        If status_id = "902" Then
                            sql_deposit = String.Format(";INSERT INTO [customer_trans] ([cust_id], [datetime], [type], [ticket_refno], [gold_type_id], [quantity], [amount], [price], [remark], [created_by], [created_date]) VALUES " & _
                                                   "('{0}', getdate(), 'Deposit', '{1}', '{2}', {3}, {4}, {6}, 'Complete Deposit from System.', '{5}', getdate())", cust_id, refno, gold_type_Dep, quan_dep, amount_dep, modifier_by, price)
                        End If

                        If status_id = "903" And type = "buy" Then 'case ตัดทองฝาก
                            quan_dep = quantity
                            gold_type_Dep = purity
                            sql_deposit = String.Format(";INSERT INTO [customer_trans] ([cust_id], [datetime], [type], [ticket_refno], [gold_type_id], [quantity], [amount], [price], [remark], [created_by], [created_date]) VALUES " & _
                                                   "('{0}', getdate(), 'Withdraw', '{1}', '{2}', {3}, {4}, {6}, 'ตัดทองฝากจาก {1}.', '{5}', getdate())", cust_id, refno, gold_type_Dep, quan_dep, amount_dep, modifier_by, price)
                        End If

                        gold_type_id = "G" + purity + "_base"
                        'Sell : -gold , +cash
                        'Buy : +gold , -cash
                        If type = "sell" Then
                            quantity_stock = quantity * -1
                            amount_stock = amount
                        Else
                            amount_stock = amount * -1
                            quantity_stock = quantity
                        End If

                        actual_amount = amount_stock
                        actual_quan = quantity_stock

                        '************** get Stock
                        For Each dc As DataColumn In dt_stock.Columns
                            detail_stock += "| " + dc.ColumnName + " : " + dt_stock.Rows(0)(dc.ColumnName).ToString
                        Next

                        If status_id = "901" Or status_id = "104" Or status_id = "105" Then 'ตัดทองฝาก ไม่ต้อง บวกในสต๊อกเพิ่ม
                            If purity = "96" Then
                                G96_base += quantity_stock
                            ElseIf purity = "99N" Then
                                G99N_base += quantity_stock
                            ElseIf purity = "99L" Then
                                G99L_base += quantity_stock
                            End If
                        End If
                        payment_base = Double.Parse(dt_stock.Rows(0)(payment_type)) + amount_stock
                        amount_stock = Double.Parse(dt_stock.Rows(0)("price_base")) + amount_stock '+ before_amount
                        quantity_stock = Double.Parse(dt_stock.Rows(0)(gold_type_id)) + quantity_stock '+ before_quantity
                        '************** get Stock

                        Select Case CInt(status_id)
                            Case 901
                                sql_stock = String.Format(";update stock set price_base = {0}, {2} = {1}, {3} = {4} ", amount_stock, quantity_stock, gold_type_id, payment_type, payment_base)
                                sql = String.Format("UPDATE tickets SET  status_id ={1},modifier_by='{2}',modifier_date=getdate(),delivery_date=getdate(),before_status_id = {3} WHERE ref_no = '{0}'", refno, status_id, modifier_by, before_status)
                            Case 902 To 903
                                sql_stock = String.Format(";update stock set price_base = {0}, {2} = {1}, {3} = {4} ", amount_stock, quantity_stock, gold_type_id, payment_type, payment_base) + sql_deposit
                                sql = String.Format("UPDATE tickets SET  status_id ={1},modifier_by='{2}',modifier_date=getdate(),delivery_date=getdate(),before_status_id = {3} WHERE ref_no = '{0}'", refno, status_id, modifier_by, before_status)
                            Case 104
                                sql_stock = String.Format(";update stock set {1} = {0} ", quantity_stock, gold_type_id)
                                sql = String.Format("UPDATE tickets SET  status_id ={1},modifier_by='{2}',modifier_date=getdate(),payment_date=getdate(),before_status_id = {3} WHERE ref_no = '{0}'", refno, status_id, modifier_by, before_status)
                                remark = "ทองออก รอเงิน"
                            Case 105
                                sql_stock = String.Format(";update stock set price_base = {0}, {1}={2} ", amount_stock, payment_type, payment_base)
                                sql = String.Format("UPDATE tickets SET  status_id ={1},modifier_by='{2}',modifier_date=getdate(),payment_date=getdate(),before_status_id = {3} WHERE ref_no = '{0}'", refno, status_id, modifier_by, before_status)
                                remark = "เงินออก รอทอง"
                        End Select
                    Case 104 To 105
                        Select Case CInt(status_id)
                            Case 901

                            Case 101

                        End Select

                End Select



                If before_status = "901" Or before_status = "902" Or before_status = "903" Then ' case เคย complete มาแล้ว


                Else
                    'ไม่เคย complete
                    If status_id = "901" Or status_id = "902" Or status_id = "903" Or status_id = "104" Or status_id = "105" Then
                        Dim gold_type_Dep As String = ""
                        Dim quan_dep As Double = 0
                        Dim amount_dep As Double = 0
                        If type = "sell" Then 'Sell ฝากทอง , buy ฝากเงิน
                            gold_type_Dep = purity
                            quan_dep = quantity
                        Else
                            amount_dep = amount
                        End If

                        If status_id = "902" Then
                            sql_deposit = String.Format(";INSERT INTO [customer_trans] ([cust_id], [datetime], [type], [ticket_refno], [gold_type_id], [quantity], [amount], [price], [remark], [created_by], [created_date]) VALUES " & _
                                                   "('{0}', getdate(), 'Deposit', '{1}', '{2}', {3}, {4}, {6}, 'Complete Deposit from System.', '{5}', getdate())", cust_id, refno, gold_type_Dep, quan_dep, amount_dep, modifier_by, price)
                        End If

                        If status_id = "903" And type = "buy" Then 'case ตัดทองฝาก
                            quan_dep = quantity
                            gold_type_Dep = purity
                            sql_deposit = String.Format(";INSERT INTO [customer_trans] ([cust_id], [datetime], [type], [ticket_refno], [gold_type_id], [quantity], [amount], [price], [remark], [created_by], [created_date]) VALUES " & _
                                                   "('{0}', getdate(), 'Withdraw', '{1}', '{2}', {3}, {4}, {6}, 'ตัดทองฝากจาก {1}.', '{5}', getdate())", cust_id, refno, gold_type_Dep, quan_dep, amount_dep, modifier_by, price)
                        End If

                        gold_type_id = "G" + purity + "_base"
                        If type = "sell" Then 'Sell : -gold , +cash
                            quantity_stock = quantity * -1
                            amount_stock = amount
                        Else 'Buy : +gold , -cash
                            amount_stock = amount * -1
                            quantity_stock = quantity
                        End If

                        actual_amount = amount_stock
                        actual_quan = quantity_stock
                        '************** get Stock
                        For Each dc As DataColumn In dt_stock.Columns
                            detail_stock += "| " + dc.ColumnName + " : " + dt_stock.Rows(0)(dc.ColumnName).ToString
                        Next

                        If status_id = "901" Or status_id = "104" Or status_id = "105" Then 'ตัดทองฝาก ไม่ต้อง บวกในสต๊อกเพิ่ม
                            If purity = "96" Then
                                G96_base += quantity_stock
                            ElseIf purity = "99N" Then
                                G99N_base += quantity_stock
                            ElseIf purity = "99L" Then
                                G99L_base += quantity_stock
                            End If
                        End If
                        payment_base = Double.Parse(dt_stock.Rows(0)(payment_type)) + amount_stock
                        amount_stock = Double.Parse(dt_stock.Rows(0)("price_base")) + amount_stock '+ before_amount
                        quantity_stock = Double.Parse(dt_stock.Rows(0)(gold_type_id)) + quantity_stock '+ before_quantity
                        '************** get Stock

                        If status_id = "901" Then
                            If before_status = "104" Then 'sell เพิ่ม cash 
                                sql_stock = String.Format(";update stock set price_base = {0},{1} = {2} ", amount_stock, payment_type, payment_base)
                            ElseIf before_status = "105" Then 'buy เพิ่ม gold 
                                sql_stock = String.Format(";update stock set {1} = {0} ", quantity_stock, gold_type_id)
                            Else '101
                                sql_stock = String.Format(";update stock set price_base = {0}, {2} = {1}, {3} = {4} ", amount_stock, quantity_stock, gold_type_id, payment_type, payment_base)
                            End If
                        ElseIf status_id = "903" Then
                            sql_stock = String.Format(";update stock set price_base = {0}, {2} = {1}, {3} = {4} ", amount_stock, quantity_stock, gold_type_id, payment_type, payment_base) + sql_deposit
                        ElseIf status_id = "902" Then
                            sql_stock = String.Format(";update stock set price_base = {0}, {2} = {1}, {3} = {4} ", amount_stock, quantity_stock, gold_type_id, payment_type, payment_base) + sql_deposit
                        ElseIf status_id = "104" Then 'sell > update gold
                            sql_stock = String.Format(";update stock set {1} = {0} ", quantity_stock, gold_type_id)
                        ElseIf status_id = "105" Then 'buy > update amount
                            sql_stock = String.Format(";update stock set price_base = {0}, {1}={2} ", amount_stock, payment_type, payment_base)
                        End If

                        If status_id = "901" Or status_id = "902" Or status_id = "903" Then
                            If before_status = "104" Or before_status = "105" Then
                                sql = String.Format("UPDATE tickets SET  status_id ={1},modifier_by='{2}',modifier_date=getdate(),delivery_date=getdate(),before_status_id = {3} WHERE ref_no = '{0}'", refno, status_id, modifier_by, before_status)
                            Else
                                sql = String.Format("UPDATE tickets SET  status_id ={1},modifier_by='{2}',modifier_date=getdate(),delivery_date=getdate(),before_status_id = {3} WHERE ref_no = '{0}'", refno, status_id, modifier_by, before_status)
                            End If
                        Else '104,105 ไม่ต้องใส่ delivery date,payment for ออกเงิน หรือ ออกทอง แล้ว
                            sql = String.Format("UPDATE tickets SET  status_id ={1},modifier_by='{2}',modifier_date=getdate(),payment_date=getdate(),before_status_id = {3} WHERE ref_no = '{0}'", refno, status_id, modifier_by, before_status)

                            If status_id = "104" Then
                                remark = "ทองออก รอเงิน"
                            ElseIf status_id = "105" Then
                                remark = "เงินออก รอทอง"
                            End If
                        End If

                    Else
                        'case not in 901,902'case อื่นๆ
                        For Each dc As DataColumn In dt_stock.Columns
                            detail_stock += "| " + dc.ColumnName + " : " + dt_stock.Rows(0)(dc.ColumnName).ToString
                        Next

                        price_base = clsManage.convert2zero(dt_stock.Rows(0)("price_base"))
                        G96_base = clsManage.convert2zero(dt_stock.Rows(0)("G96_base"))
                        G99N_base = clsManage.convert2zero(dt_stock.Rows(0)("G99N_base"))
                        G99L_base = clsManage.convert2zero(dt_stock.Rows(0)("G99L_base"))

                        actual_amount = amount
                        actual_quan = quantity

                        If before_status = "104" Then
                            gold_type_id = "G" + purity + "_base"
                            If purity = "96" Then
                                quantity_stock = G96_base + quantity
                            ElseIf purity = "99N" Then
                                quantity_stock = G99N_base + quantity
                            ElseIf purity = "99L" Then
                                quantity_stock = G99L_base + quantity
                            End If
                            sql = String.Format("UPDATE tickets SET  status_id ={1},modifier_date=getdate(),modifier_by='{2}',payment_date = NULL,before_status_id = {3} WHERE ref_no = '{0}'", refno, status_id, modifier_by, before_status)
                            sql_stock = String.Format(";update stock set {1} = {0} ", quantity_stock, gold_type_id)
                        ElseIf before_status = "105" Then
                            amount_stock = price_base + amount
                            sql = String.Format("UPDATE tickets SET  status_id ={1},modifier_date=getdate(),modifier_by='{2}',payment_date = NULL,before_status_id = {3} WHERE ref_no = '{0}'", refno, status_id, modifier_by, before_status)
                            sql_stock = String.Format(";update stock set price_base = {0},{1}={2} ", amount_stock, payment_type, payment_base)
                        Else
                            sql = String.Format("UPDATE tickets SET  status_id ={1},modifier_date=getdate(),modifier_by='{2}',before_status_id = {3} WHERE ref_no = '{0}'", refno, status_id, modifier_by, before_status)
                        End If
                    End If
                End If

                cmd = New SqlCommand(sql + sql_stock, con)
                cmd.Transaction = tr
                Dim result As Integer = cmd.ExecuteNonQuery()
                If result > 0 Then
                    'Insert Actual
                    Dim asset_type = "Gold"
                    Dim price_actual As Double = clsManage.convert2zero(dt_stock.Rows(0)("priceDep"))
                    Dim G96 As Double = clsManage.convert2zero(dt_stock.Rows(0)("G96Dep"))
                    Dim G99N As Double = clsManage.convert2zero(dt_stock.Rows(0)("G99NDep"))
                    Dim G99L As Double = clsManage.convert2zero(dt_stock.Rows(0)("G99LDep"))
                    Dim cash_stock As Double = clsManage.convert2zero(dt_stock.Rows(0)("cashDep"))
                    Dim trans_stock As Double = clsManage.convert2zero(dt_stock.Rows(0)("transDep"))
                    Dim cheq_stock As Double = clsManage.convert2zero(dt_stock.Rows(0)("cheqDep"))

                    ''update stock from DB
                    If payment_base <> price_actual Then
                        price_actual = payment_base
                    End If
                    Select Case purity
                        Case "96"
                            G96 += actual_quan
                        Case "99N"
                            G99N += actual_quan
                        Case "99L"
                            G99L += actual_quan
                    End Select

                    Select Case payment_type
                        Case clsManage.payment.cash.ToString
                            cash_stock += actual_amount
                        Case clsManage.payment.trans.ToString
                            trans_stock += actual_amount
                        Case clsManage.payment.cheq.ToString
                            cheq_stock += actual_amount
                    End Select


                    'payment_base = Double.Parse(dt_stock.Rows(0)(payment_type)) + amount_stock
                    'amount_stock = Double.Parse(dt_stock.Rows(0)("price_base")) + amount_stock '+ before_amount
                    'quantity_stock = Double.Parse(dt_stock.Rows(0)(gold_type_id)) + quantity_stock '+ before_quantity

                    If act.cash = "n" Then
                        If act.update_gold = "y" Then
                            sql = String.Format(";INSERT INTO [actual] ([asset_id], [ref_no], [asset_type], [created_by], [datetime], [purity], [quantity], [amount], [status_id], [status_name], [type], [order_type], [price_base], [G96_base], [G99N_base], [G99L_base], [cust_id], [before_status_id], [note], [payment], [cash], [trans], [cheq]) " & _
                                                              "VALUES ('{0}', '{1}', '{2}', '{3}', getdate(), '{4}', {5}, {6}, '{7}', @status_name, '{9}', '{10}', {11}, {12}, {13}, {14}, '{15}','{16}','{17}','{18}',{19},{20},{21})" & _
                                                              "", act.asset_id, act.ref_no, asset_type, act.created_by, act.purity, act.quantity, act.amount, act.status_id, act.status_name, act.type, act.order_type, price_actual, G96, G99N, G99L, act.cust_id, act.before_status_id, act.note, act.payment, cash_stock, trans_stock, cheq_stock)
                        End If
                        If act.update_cash = "y" Then
                            asset_type = "Cash"
                            If act.type = "In" Then
                                act.type = "Out"
                            ElseIf act.type = "ตัดทองฝาก" Then
                                act.type = "Out"
                            Else
                                act.type = "In"
                            End If
                            sql += String.Format(";INSERT INTO [actual] ([asset_id], [ref_no], [asset_type], [created_by], [datetime], [purity], [quantity], [amount], [status_id], [status_name], [type], [order_type], [price_base], [G96_base], [G99N_base], [G99L_base], [cust_id], [before_status_id], [note], [payment], [cash], [trans], [cheq]) " & _
                                                          "VALUES ('{0}', '{1}', '{2}', '{3}', getdate(), '{4}', {5}, {6}, '{7}', @status_name, '{9}', '{10}', {11}, {12}, {13}, {14}, '{15}','{16}','{17}','{18}',{19},{20},{21})" & _
                                                          "", act.asset_id, act.ref_no, asset_type, act.created_by, act.purity, act.quantity, act.amount, act.status_id, act.status_name, act.type, act.order_type, price_actual, G96, G99N, G99L, act.cust_id, act.before_status_id, act.note, act.payment, cash_stock, trans_stock, cheq_stock)

                        End If
                    Else
                        asset_type = "Cash"

                        sql += String.Format(";INSERT INTO [actual] ([asset_id], [ref_no], [asset_type], [created_by], [datetime], [purity], [quantity], [amount], [status_id], [status_name], [type], [order_type], [price_base], [G96_base], [G99N_base], [G99L_base], [cust_id], [before_status_id], [note], [payment], [cash], [trans], [cheq]) " & _
                                                      "VALUES ('{0}', '{1}', '{2}', '{3}', getdate(), '{4}', {5}, {6}, '{7}', @status_name, '{9}', '{10}', {11}, {12}, {13}, {14}, '{15}','{16}','{17}','{18}',{19},{20},{21})" & _
                                                      "", act.asset_id, act.ref_no, asset_type, act.created_by, act.purity, act.quantity, act.amount, act.status_id, act.status_name, act.type, act.order_type, price_actual, G96, G99N, G99L, act.cust_id, act.before_status_id, act.note, act.payment, cash_stock, trans_stock, cheq_stock)
                    End If

                    sql += String.Format(";INSERT INTO ticket_log (ref_no,datetime,update_by) VALUES ('{0}',getdate(),'{1}')", refno, modifier_by)
                    'cmd = New SqlCommand(sql, con)
                    cmd.CommandText = sql
                    Dim parameter As New SqlParameter("@status_name", SqlDbType.VarChar)
                    parameter.Value = act.status_name
                    cmd.Parameters.Add(parameter)
                    Dim result_actual As Integer = cmd.ExecuteNonQuery()
                    If result_actual > 0 Then
                        tr.Commit()
                        Return result_actual
                    Else
                        tr.Rollback()
                    End If
                Else
                    tr.Rollback()
                End If
            End If
            Return 0
        Catch ex As Exception
            Dim da As New dsStockOnlineTableAdapters.error_messageTableAdapter
            da.Insert(ex.Message, "Actual", DateTime.Now)
            tr.Rollback()
            Throw ex
        Finally
            '***Check  Don't Update Stock Online 
            Dim da As New dsStockOnlineTableAdapters.error_messageTableAdapter
            If sql_stock = "" Then
                da.Insert(String.Format("Don't Update Stock|sql= {0} |Stock_Detail = {1}", sql + sql_stock, detail_stock), before_status + " > " + status_id, DateTime.Now)
            Else
                da.Insert(String.Format("Update Stock|sql= {0} |Stock_Detail = {1}", sql + sql_stock, detail_stock), before_status + " > " + status_id, DateTime.Now)
            End If
            '***
            con.Close()
        End Try
    End Function

    Public Shared Function getGoldType() As DataTable

        Dim sql As String = "SELECT gold_type_id,gold_type_name FROM gold_type"
        Dim con As New SqlConnection(strcon)
        Try
            Dim da As New SqlDataAdapter(sql, con)
            Dim dt As New DataTable
            da.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getTicket__Byidk(ref_no As String) As DataTable

        Dim sql As String = "SELECT convert(varchar,created_date,8) as ttime from [tickets] where ref_no='" + ref_no + "' "
        Dim con As New SqlConnection(strcon)
        Try
            Dim da As New SqlDataAdapter(sql, con)
            Dim dt As New DataTable
            da.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getTicket_status() As DataTable

        Dim sql As String = "SELECT status_id,status_name FROM [ticket_status]"
        Dim con As New SqlConnection(strcon)
        Try
            Dim da As New SqlDataAdapter(sql, con)
            Dim dt As New DataTable
            da.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function insertTicket(ByVal cust_id As String, ByVal book_no As String, ByVal run_no As String, ByVal user_id As String, ByVal gold_type_id As String, ByVal type As String, ByVal Dalevery_date As String, ByVal ticket_date As DateTime, ByVal billing As String, ByVal payment As String, ByVal payment_bank As String, ByVal payment_duedate As String, ByVal payment_cheq_no As String, ByVal remark As String, ByVal delivery As String, ByVal created_by As String, ByVal quan As String, ByVal price As String, ByVal amount As String, ByVal active As Boolean, ByVal gold_dep As Boolean, ByVal invoice As String, clearingday As Integer, Optional status_id As String = "") As String

        Try
            'get Runing No

            Dim iTicketDate As DateTime = clsDB.getTime(ticket_date)
            Dim con As New SqlConnection(strcon)
            con.Open()
            Dim tr As SqlTransaction = con.BeginTransaction
            Dim running_no As String = getTicketRunNo()
            Try
                If running_no = "" Then Throw New Exception("Can not get RunNo.")

                '1.Insert Ticket
                Dim sql As String = "INSERT INTO [tickets] ([ref_no], [book_no], [run_no], [cust_id], [user_id], [gold_type_id], [type], [delivery_date], [ticket_date], [billing], [payment], [payment_bank], [payment_duedate], [payment_cheq_no], [remark], [delivery],[created_date], [modifier_date], [created_by], [modifier_by],[quantity], [price], [amount], [active], [gold_dep], [invoice],[status_id],[clearingday]) VALUES " & _
                                  "(@ref_no, @book_no, @run_no, @cust_id, @user_id, @gold_type_id, @type, @delivery_date, @ticket_date, @billing, @payment, @payment_bank, @payment_duedate, @payment_cheq_no, @remark, @delivery, getdate(),getdate(), @created_by, @modifier_by, @quantity, @price, @amount, @active, @gold_dep, @invoice,@status_id,@clearingday)"

                Dim cmd As New SqlCommand(sql, con)

                Dim parameter As New SqlParameter("@ref_no", SqlDbType.VarChar, 20)
                parameter.Value = running_no
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@book_no", SqlDbType.VarChar, 10)
                parameter.Value = book_no
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@run_no", SqlDbType.VarChar, 10)
                parameter.Value = run_no
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@cust_id", SqlDbType.Int)
                parameter.Value = cust_id
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@user_id", SqlDbType.Int)
                parameter.Value = user_id
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@gold_type_id", SqlDbType.VarChar, 4)
                parameter.Value = gold_type_id
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@type", SqlDbType.VarChar, 5)
                parameter.Value = type
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@delivery_date", SqlDbType.DateTime)
                If Dalevery_date <> "" Then
                    parameter.Value = DateTime.ParseExact(Dalevery_date, clsManage.formatDateTime, Nothing)
                Else
                    parameter.Value = DBNull.Value
                End If
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@ticket_date", SqlDbType.DateTime)
                parameter.Value = iTicketDate
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@billing", SqlDbType.VarChar, 5)
                parameter.Value = billing
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@payment", SqlDbType.VarChar, 50)
                parameter.Value = payment
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@payment_bank", SqlDbType.VarChar, 10)
                parameter.Value = payment_bank
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@payment_duedate", SqlDbType.DateTime)
                If payment_duedate <> "" Then
                    parameter.Value = DateTime.ParseExact(payment_duedate, clsManage.formatDateTime, Nothing)
                Else
                    parameter.Value = DBNull.Value
                End If
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@payment_cheq_no", SqlDbType.VarChar, 50)
                parameter.Value = payment_cheq_no
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@remark", SqlDbType.VarChar, 4000)
                parameter.Value = remark
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@delivery", SqlDbType.VarChar, 50)
                parameter.Value = delivery
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@created_by", SqlDbType.VarChar, 50)
                parameter.Value = created_by
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@modifier_by", SqlDbType.VarChar, 50)
                parameter.Value = created_by
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@price", SqlDbType.Decimal)
                parameter.Value = price
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@quantity", SqlDbType.Decimal)
                parameter.Value = quan
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@amount", SqlDbType.Decimal)
                parameter.Value = amount
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@invoice", SqlDbType.VarChar)
                parameter.Value = invoice
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@active", SqlDbType.VarChar, 1)
                parameter.Value = IIf(active, "y", "n")
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@gold_dep", SqlDbType.VarChar, 1)
                parameter.Value = IIf(gold_dep, "y", "n")
                cmd.Parameters.Add(parameter)


                parameter = New SqlParameter("@status_id", SqlDbType.VarChar)
                parameter.Value = IIf(status_id = "", "101", status_id)
                cmd.Parameters.Add(parameter)


                parameter = New SqlParameter("@clearingday", SqlDbType.Int)
                parameter.Value = clearingday
                cmd.Parameters.Add(parameter)

                cmd.Transaction = tr
                Dim result As Integer = cmd.ExecuteNonQuery()
                If result > 0 Then
                    tr.Commit()

                    If status_id <> "" Then
                        Dim act As New clsActual
                        '********** Acutual
                        Dim status_name As String = ""
                        Dim order_type As String = type
                        Dim update_gold As String = "y"
                        Dim update_cash As String = "y"
                        Dim cash As String = "n"
                        Dim note As String = ""
                        type = "Out"

                        If order_type = "sell" Then
                            type = "Out"
                        ElseIf order_type = "buy" Then
                            type = "In"
                        End If

                        Dim before_status As String = "101"
                        status_id = "903"
                        status_name = "complete ตัดทอง"
                        type = "ตัดทองฝาก"
                        If status_name <> "" And order_type <> "" Then
                            act.asset_id = ""
                            act.ref_no = running_no
                            act.created_by = created_by
                            act.purity = gold_type_id
                            act.quantity = quan
                            act.amount = amount
                            act.status_id = status_id
                            act.status_name = status_name
                            act.type = type
                            act.order_type = order_type
                            act.cust_id = cust_id
                            act.before_status_id = before_status
                            act.note = note
                            act.update_cash = update_cash
                            act.update_gold = update_gold
                            act.cash = cash
                            act.payment = payment
                        End If
                        clsDB.updateTicket_status(running_no, status_id, created_by, type, _
                                                                          gold_type_id, quan, _
                                                                          amount, cust_id, gold_dep, _
                                                                          payment, clsManage.convert2zero(price), before_status, act)
                    End If
                    Return running_no
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

    Public Shared Function updatetTicket(ByVal refNo As String, ByVal cust_id As String, ByVal type As String, ByVal gold_type_id As String, ByVal Dalevery_date As String, ByVal ticket_date As DateTime, ByVal billing As String, ByVal payment As String, ByVal payment_bank As String, ByVal payment_duedate As String, ByVal payment_cheq_no As String, ByVal remark As String, ByVal delivery As String, ByVal modifier_by As String, ByVal quan As String, ByVal price As String, ByVal amount As String, ByVal active As Boolean, ByVal gold_dep As Boolean, ByVal invoice As String, clearingday As Integer) As String

        Try
            Dim con As New SqlConnection(strcon)
            con.Open()
            Dim tr As SqlTransaction = con.BeginTransaction
            Dim created_t As Data.DataTable
            created_t = clsDB.getTicket__Byidk(refNo)

            Try
                Dim sql As String = "UPDATE [tickets] SET [cust_id] = @cust_id, [type] = @type, [delivery] = @delivery, [delivery_date] = @delivery_date, [ticket_date] = @ticket_date,[gold_type_id]=@gold_type_id, " & _
                                    "[billing] = @billing, [remark] = @remark, [payment] = @payment, [payment_bank] = @payment_bank, [payment_duedate] = @payment_duedate, [payment_cheq_no] = @payment_cheq_no, [modifier_date] = getdate(), [modifier_by] = @modifier_by, [quantity] = @quantity, [price] = @price, [amount] = @amount, [active] = @active ,[gold_dep]= @gold_dep, [invoice] = @invoice, [clearingday] = @clearingday " & _
                                    " WHERE (([ref_no] = @ref_no))"
                Dim cmd As New SqlCommand(sql, con)

                Dim parameter As New SqlParameter("@ref_no", SqlDbType.VarChar, 20)
                parameter.Value = refNo
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@cust_id", SqlDbType.Int)
                parameter.Value = cust_id
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@type", SqlDbType.VarChar, 5)
                parameter.Value = type
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@gold_type_id", SqlDbType.VarChar, 4)
                parameter.Value = gold_type_id
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@delivery", SqlDbType.VarChar, 50)
                parameter.Value = delivery
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@delivery_date", SqlDbType.DateTime)
                If Dalevery_date <> "" Then
                    parameter.Value = DateTime.ParseExact(Dalevery_date, clsManage.formatDateTime, Nothing)
                Else
                    parameter.Value = DBNull.Value
                End If
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@ticket_date", SqlDbType.DateTime)
                parameter.Value = ticket_date + " " + created_t.Rows(0)("ttime")
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@billing", SqlDbType.VarChar, 5)
                parameter.Value = IIf(billing = "", DBNull.Value, billing)
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@remark", SqlDbType.VarChar, 4000)
                parameter.Value = remark
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@payment", SqlDbType.VarChar, 50)
                parameter.Value = payment
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@payment_duedate", SqlDbType.DateTime)
                If payment_duedate <> "" Then
                    parameter.Value = DateTime.ParseExact(payment_duedate, clsManage.formatDateTime, Nothing)
                Else
                    parameter.Value = DBNull.Value
                End If
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@payment_cheq_no", SqlDbType.VarChar, 50)
                parameter.Value = payment_cheq_no
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@payment_bank", SqlDbType.VarChar, 10)
                parameter.Value = payment_bank
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@modifier_by", SqlDbType.VarChar, 50)
                parameter.Value = modifier_by
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@price", SqlDbType.Decimal)
                parameter.Value = price
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@quantity", SqlDbType.Decimal)
                parameter.Value = quan
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@amount", SqlDbType.Decimal)
                parameter.Value = amount
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@invoice", SqlDbType.VarChar)
                parameter.Value = invoice
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@active", SqlDbType.VarChar, 1)
                parameter.Value = IIf(active, "y", "n")
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@gold_dep", SqlDbType.VarChar, 1)
                parameter.Value = IIf(gold_dep, "y", "n")
                cmd.Parameters.Add(parameter)

                parameter = New SqlParameter("@clearingday", SqlDbType.Int)
                parameter.Value = clearingday
                cmd.Parameters.Add(parameter)

                cmd.Transaction = tr
                Dim result As Integer = cmd.ExecuteNonQuery()

                If result > 0 Then
                    tr.Commit()
                    Return result.ToString

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

    Public Shared Function log_ticket(ByVal ref_no As String, ByVal user_id As String, ByVal cmd As SqlCommand) As Boolean
        Try
            Dim sql As String = String.Format("INSERT INTO ticket_log (ref_no,datetime,update_by) VALUES ('{0}',getdate(),'{1}')", ref_no, user_id)
            cmd.CommandText = sql
            Dim result_log As Integer = cmd.ExecuteNonQuery()
            If result_log > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function checkLimitGoldDep(ByVal quan As Double, ByVal cust_id As String, ByVal gold_type As String) As Boolean

        Dim sql As String = String.Format("select sum(quantity) as quantity from customer_trans where cust_id = '{0}' and pre = 'n'", cust_id)
        Dim con As New SqlConnection(strcon)
        Try
            Dim da As New SqlDataAdapter(sql, con)
            Dim dt As New DataTable
            If dt.Rows.Count > 0 Then
                If quan > clsManage.convert2zero(dt.Rows(0)("quantity").ToString) Then

                End If
            End If
            Return True
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'Public Shared Function stock_ticket(ByVal gold As Double, ByVal money As Double, ByVal gold_type As String, ByVal cmd As SqlCommand, ByVal PaymentFieldName As String) As Boolean
    '    Try
    '        Dim sql As String = ""

    '        If gold_type = "SIL" Then
    '            sql = String.Format("UPDATE STOCK SET silver_wait={0},{2}={1}", gold, money, PaymentFieldName)
    '        Else
    '            sql = String.Format("UPDATE STOCK SET gold_wait={0},{2}={1}", gold, money, PaymentFieldName)
    '        End If

    '        cmd.CommandText = sql
    '        Dim result_log As Integer = cmd.ExecuteNonQuery()
    '        If result_log > 0 Then
    '            Return True
    '        Else
    '            Return False
    '        End If
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function

    Public Shared Function getTicketLogUpdate(ByVal ref_no As String) As DataTable
        '****** ต้องเรืยงลำดับ column ใน sql ให้ตรงกับ gridview
        Dim sql As String = String.Format("" & _
" SELECT     ticket_log_update.ref_no, customer.firstname, ticket_log_update.book_no, ticket_log_update.run_no, ticket_log_update.invoice, ticket_log_update.gold_type_id, " & _
"                       ticket_log_update.delivery, ticket_log_update.type, " & _
"                       ticket_log_update.quantity, ticket_log_update.price, ticket_log_update.amount, ticket_log_update.delivery_date, ticket_log_update.ticket_date, " & _
"                       ticket_log_update.billing, ticket_log_update.remark, ticket_log_update.payment, ticket_log_update.payment_bank, ticket_log_update.payment_duedate, " & _
"                       ticket_log_update.payment_cheq_no, ticket_status.status_name, ticket_log_update.modifier_date, users_update.user_name AS update_by, " & _
"                       ticket_log_update.active, ticket_log_update.created_date,users_created.user_name AS created_by, ticket_log_update.cust_id" & _
" FROM         ticket_log_update LEFT OUTER JOIN " & _
"                      users AS users_update ON ticket_log_update.modifier_by = users_update.user_id LEFT OUTER JOIN " & _
"                      users AS users_created ON ticket_log_update.created_by = users_created.user_id LEFT OUTER JOIN " & _
"                      ticket_status ON ticket_log_update.status_id = ticket_status.status_id LEFT OUTER JOIN " & _
"                      customer ON ticket_log_update.cust_id = customer.cust_id " & _
" WHERE ticket_log_update.ref_no = '{0}' order by log_id asc ", ref_no)

        Dim con As New SqlConnection(strcon)
        Try
            Dim da As New SqlDataAdapter(sql, con)
            Dim dt As New DataTable
            da.Fill(dt)
            Return dt
        Catch ex As Exception
            Throw ex
        End Try
    End Function

#End Region

#Region "Ticket Split"

    Public Shared Function getTicketSplit(ByVal refno As String, ByVal type As String) As DataTable
        Try

            Dim sql As String = String.Format("" & _
"SELECT     ticket_split.ticket_sp_id, ticket_split.ref_no, ticket_split.run_no, ticket_split.row, ticket_split.type, ticket_split.quantity, ticket_split.price, ticket_split.amount, ticket_split.payment, " & _
"                      ticket_split.payment_bank, ticket_split.payment_cheq_no, ticket_split.payment_duedate, ticket_split.created_date, ticket_split.status_id, " & _
"                      (select user_name from users where user_id = ticket_split.created_by) as created_by," & _
"                      ticket_split.receipt, ticket_split.paid, ticket_split.dep, bank.bank_id, bank.bank_name, tickets.cust_id, tickets.billing, tickets.trade, tickets.type AS order_type " & _
"FROM         ticket_split INNER JOIN " & _
"                      tickets ON ticket_split.ref_no = tickets.ref_no LEFT OUTER JOIN " & _
"                      bank ON bank.bank_id = ticket_split.payment_bank " & _
"WHERE     (ticket_split.ref_no = '{0}') AND (ticket_split.type = '{1}') ", refno, type)

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

    Public Shared Function insertTicketSplit(ByVal refNo As String, ByVal rowIndex As String, ByVal quantity As String, ByVal price As String, ByVal amount As String, ByVal payment As String, ByVal payment_bank As String, ByVal cheq_no As String, ByVal duedate As String, ByVal created_by As String, ByVal status_id As String, ByVal isReceipt As Boolean, ByVal type As String, ByVal purity As String, ByVal order_type As String, ByVal dep As String, Optional ByVal cust_id As String = "", Optional ByVal updateDeposit As String = "y", Optional ByVal updateStock As String = "y", Optional ByVal updateTicket As String = "n", Optional ByVal before_status As String = "", Optional ByVal cash_receipt As String = "n") As String

        Dim con As New SqlConnection(strcon)
        con.Open()
        Dim tr As SqlTransaction = con.BeginTransaction
        Try
            Dim sql As String = ""
            If duedate = "" Then
                sql = "INSERT INTO [ticket_split] ([ref_no],[row], [quantity], [price], [amount], [payment], [payment_bank], [payment_cheq_no], [created_date], [created_by], [status_id], [receipt], [type], [dep]) VALUES (@ref_no,@row, @quantity, @price, @amount, @payment, @payment_bank, @payment_cheq_no, getdate(), @created_by, @status_id,@receipt,@type,@dep)"
            Else
                sql = "INSERT INTO [ticket_split] ([ref_no],[row], [quantity], [price], [amount], [payment], [payment_bank], [payment_cheq_no], [payment_duedate], [created_date], [created_by], [status_id], [receipt], [type], [dep]) VALUES (@ref_no,@row, @quantity, @price, @amount, @payment, @payment_bank, @payment_cheq_no, @payment_duedate, getdate(), @created_by,@status_id,@receipt,@type,@dep)"
            End If
            Dim cmd As New SqlCommand

            Dim sql_stock As String = ""
            Dim sql_dep As String = ""


            If type = clsManage.splitMode.Split.ToString Or type = clsManage.splitMode.Receipt.ToString Then
                '****begin update Depoist
                If updateDeposit = "y" Then
                    If status_id = "902" Then
                        sql_dep = String.Format(";INSERT INTO [customer_trans] ([cust_id],[row], [datetime], [type], [ticket_refno], [gold_type_id], [quantity], [amount], [remark], [created_by], [created_date]) VALUES " & _
                                                   "('{0}',{6}, getdate(), 'Deposit', '{1}', '{2}', {3}, {4}, 'Complete Deposit from System.', '{5}', getdate())", cust_id, refNo, purity, quantity, amount, created_by, rowIndex)
                    ElseIf status_id = "903" Then
                        sql_dep = String.Format(";INSERT INTO [customer_trans] ([cust_id],[row], [datetime], [type], [ticket_refno], [gold_type_id], [quantity], [amount], [remark], [created_by], [created_date]) VALUES " & _
                                                  "('{0}',{6}, getdate(), 'Withdraw', '{1}', '{2}', {3}, {4}, 'ตัดทองฝาก from System.', '{5}', getdate())", cust_id, refNo, purity, quantity, amount, created_by, rowIndex)
                    End If
                End If
                '****end update Depoist


                '****begin update stock
                If updateStock = "y" Then

                End If
                Dim gold_type_id As String = ""
                Dim amount_base As Double = 0
                Dim amount_by_payment As Double = 0
                Dim amount_price_base As Double = 0
                Dim quantity_base As Double = 0
                gold_type_id = "G" + purity + "_base"
                If order_type = "sell" Then 'Sell : -gold , +cash
                    quantity_base = quantity * -1
                    amount_base = amount
                ElseIf order_type = "buy" Then 'Buy : +gold , -cash
                    quantity_base = quantity
                    amount_base = amount * -1
                Else
                    Throw New Exception("Unknow order_type")
                End If

                Dim dt_stock As New Data.DataTable
                Dim sql_getStock As String = "select * from stock"
                cmd = New SqlCommand(sql_getStock, con)
                Dim da As New SqlDataAdapter(cmd)


                cmd.Transaction = tr
                da.Fill(dt_stock)
                If dt_stock.Rows.Count > 0 Then
                    amount_by_payment = clsManage.convert2zero(dt_stock.Rows(0)(payment)) + amount_base
                    amount_price_base = clsManage.convert2zero(dt_stock.Rows(0)("price_base")) + amount_base
                    quantity_base = clsManage.convert2zero(dt_stock.Rows(0)(gold_type_id)) + quantity_base
                End If

                If updateStock = "y" Then
                    If status_id = "901" Or status_id = "902" Or status_id = "903" Then
                        sql_stock = String.Format(";update stock set price_base = {0}, {2} = {1}, {4} = {3} ", amount_price_base, quantity_base, gold_type_id, amount_by_payment, payment)
                    End If
                End If
                '****end update stock
            End If
            Dim sql_ticket As String = ""
            If updateTicket = "y" Then
                Dim sql_cash_receipt As String = ""
                If status_id = "901" Or status_id = "902" Or status_id = "903" Then
                    sql_ticket = String.Format(";UPDATE tickets SET  status_id ={1},modifier_date=getdate(),modifier_by='{2}',delivery_date=getdate(),cash_receipt = '{3}' WHERE ref_no = '{0}'", refNo, status_id, created_by, cash_receipt)
                End If
            End If

            cmd = New SqlCommand(sql + sql_stock + sql_dep + sql_ticket, con)

            Dim parameter As New SqlParameter("@ref_no", SqlDbType.VarChar, 20)
            parameter.Value = refNo
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@row", SqlDbType.Int)
            parameter.Value = rowIndex
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@quantity", SqlDbType.Decimal)
            parameter.Value = quantity
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@price", SqlDbType.Decimal)
            parameter.Value = price
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@amount", SqlDbType.Decimal)
            parameter.Value = amount
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@payment", SqlDbType.VarChar)
            parameter.Value = payment
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@payment_bank", SqlDbType.VarChar)
            parameter.Value = payment_bank
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@payment_cheq_no", SqlDbType.VarChar)
            parameter.Value = cheq_no
            cmd.Parameters.Add(parameter)

            If duedate <> "" Then
                parameter = New SqlParameter("@payment_duedate", SqlDbType.DateTime)
                parameter.Value = DateTime.ParseExact(duedate, clsManage.formatDateTime, Nothing)
                cmd.Parameters.Add(parameter)
            End If

            parameter = New SqlParameter("@created_by", SqlDbType.VarChar)
            parameter.Value = created_by
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@status_id", SqlDbType.VarChar)
            parameter.Value = status_id
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@receipt", SqlDbType.Bit)
            parameter.Value = isReceipt
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@type", SqlDbType.VarChar)
            parameter.Value = type
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@dep", SqlDbType.Char, 1)
            parameter.Value = dep
            cmd.Parameters.Add(parameter)

            cmd.Transaction = tr
            Dim result As Integer = cmd.ExecuteNonQuery()

            If result > 0 Then
                tr.Commit()
                Return result.ToString

            End If
            Return ""
        Catch ex As Exception
            tr.Rollback()
            Throw ex
        Finally
            con.Close()
        End Try

    End Function

    Public Shared Function delTicketSplitBySpId(ByVal ticket_sp_id As String, ByVal ref_no As String, ByVal rowIndex As Integer, ByVal quantity As Double, ByVal amount As Double, ByVal order_type As String, ByVal gold_type_id As String, ByVal status As String, ByVal payment As String) As Boolean

        Dim con As New SqlConnection(strcon)
        con.Open()
        Dim tr As SqlTransaction = con.BeginTransaction
        Try

            Dim cmd As New SqlCommand()
            Dim sql As String = ""
            Dim sql_stock As String = ""
            Dim sql_splite As String = ""
            Dim sql_dep As String = ""

            gold_type_id = "G" + gold_type_id + "_base"
            'swep case above
            If order_type = "buy" Then 'Buy : +gold , -cash
                quantity = quantity * -1
            Else 'Sell : -gold , +cash
                amount = amount * -1
            End If

            Dim amount_by_payment As Double = 0
            Dim amount_price_base As Double = 0

            Dim dt_stock As New Data.DataTable
            Dim sql_getStock As String = "select * from stock"
            cmd = New SqlCommand(sql_getStock, con)
            Dim da As New SqlDataAdapter(cmd)
            cmd.Transaction = tr
            da.Fill(dt_stock)
            If dt_stock.Rows.Count > 0 Then
                amount_by_payment = clsManage.convert2zero(dt_stock.Rows(0)(payment)) + amount
                amount_price_base = clsManage.convert2zero(dt_stock.Rows(0)("price_base")) + amount
                quantity = clsManage.convert2zero(dt_stock.Rows(0)(gold_type_id)) + quantity
            End If
            sql_stock = String.Format(";update stock set price_base = {0}, {2} = {1}, {4}={3} ", amount_price_base, quantity, gold_type_id, amount_by_payment, payment)
            sql_splite = ";Delete from ticket_split where ticket_sp_id = '" & ticket_sp_id & "' "

            If status = "901" Then 'ถ้า complete แล้วให้เปลี่ยนเป็น pending
                sql_stock += String.Format(";update tickets set status_id = '101' where ref_no = '{0}'", ref_no)
            End If

            If status = "902" Or status = "903" Then
                sql_dep = String.Format(";delete from customer_trans where ticket_refno = '{0}' and row = {1}", ref_no, rowIndex)
            End If
            sql = sql_stock + sql_splite + sql_dep
            cmd = New SqlCommand(sql, con)

            cmd.Transaction = tr
            Dim result As Integer = cmd.ExecuteNonQuery()

            If result > 0 Then
                tr.Commit()
                Return True
            Else
                tr.Rollback()
                Return False
            End If
        Catch ex As Exception
            Throw ex
            tr.Rollback()
        Finally
            con.Close()
        End Try
    End Function

    Public Shared Function updateWhenDeleteFullSplit(ByVal ref_no As String) As Boolean
        Try
            Dim sql As String = String.Format("UPDATE tickets SET run_no = '',rows_no_split=1 WHERE ref_no = '{0}' AND NOT EXISTS (SELECT 1 FROM ticket_split WHERE ref_no = '{0}')", ref_no)
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

    Public Shared Function delTicketSplit(ByVal ref_no As String, ByVal type As String) As Boolean
        Try
            Dim sql As String = "Delete from ticket_split where ref_no = '" & ref_no & "' AND type = '" + type + "' "
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

    Public Shared Function getTicketCheq() As DataTable
        Try
            'Dim sql As String = "SELECT * " + _
            '" FROM         bank RIGHT OUTER JOIN" + _
            '"           ticket_split ON bank.bank_id = ticket_split.payment_bank LEFT OUTER JOIN" + _
            '"           tickets LEFT OUTER JOIN" + _
            '"           customer ON tickets.cust_id = customer.cust_id ON ticket_split.ref_no = tickets.ref_no" + _
            '" where ticket_split.payment = 'cheq' and  ticket_split.type in ('Split','Receipt')"
            '            Dim sql As String = "select * from (" & _
            '            " select 'split' as mode,created_date, amount,'Split_bill' as remark,payment,payment_bank,payment_duedate,payment_cheq_no from ticket_split" & _
            '" where payment = 'cheq' and type in('Split','Receipt')" & _
            '"            union all " & _
            '" select 'ticket' as mode,created_date,amount,remark,payment,payment_bank,payment_duedate,payment_cheq_no from tickets " & _
            '" where status_id = 901 and payment = 'cheq'" & _
            '"             union all" & _
            '" select 'deposit' as mode,created_date,amount,remark,payment,payment_bank,payment_duedate,payment_cheq_no from customer_trans" & _
            '" where gold_type_id = '' and payment = 'cheq'" & _
            '"            union all" & _
            '" select 'in/out' as mode,created_date,quantity as amount,remark,payment,payment_bank,payment_duedate,payment_cheq_no  from asset " & _
            '" where purity = '' and asset_type='Cash' and status = 'actual' and payment = 'cheq') cheque order by created_date desc "
            Dim sql As String = "" & _
            " SELECT     cheque.mode, cheque.created_date,customer.firstname, cheque.cust_id, cheque.amount, cheque.remark, cheque.payment, cheque.payment_duedate, cheque.payment_cheq_no,bank.bank_name " & _
" FROM         (SELECT     'split' AS mode, ticket_split.created_date, tickets.cust_id, ticket_split.amount, 'Split_bill' AS remark, ticket_split.payment, ticket_split.payment_bank,ticket_split.payment_duedate, ticket_split.payment_cheq_no " & _
"                        FROM          ticket_split LEFT OUTER JOIN tickets ON ticket_split.ref_no = tickets.ref_no " & _
"                        WHERE      (ticket_split.payment = 'cheq') AND (ticket_split.type IN ('Split', 'Receipt')) " & _
"                        UNION ALL " & _
"                        SELECT     'ticket' AS mode, created_date, cust_id, amount, remark, payment, payment_bank, payment_duedate, payment_cheq_no " & _
"                        FROM         tickets AS tickets_1 " & _
"                        WHERE     (status_id = 901) AND (payment = 'cheq') " & _
"                        UNION ALL " & _
"                        SELECT     'deposit' AS mode, created_date, cust_id, amount, remark, payment, payment_bank, payment_duedate, payment_cheq_no " & _
"                        FROM         customer_trans " & _
"                        WHERE     (gold_type_id = '') AND (payment = 'cheq') " & _
"                        UNION ALL " & _
"                        SELECT     'in/out' AS mode, created_date, '' AS cust_id, quantity AS amount, remark, payment, payment_bank, payment_duedate, payment_cheq_no " & _
"                        FROM         asset " & _
"                        WHERE     (purity = '') AND (asset_type = 'Cash') AND (status = 'actual') AND (payment = 'cheq')) AS cheque INNER JOIN " & _
"                       bank ON cheque.payment_bank = bank.bank_id LEFT OUTER JOIN customer ON cheque.cust_id = customer.cust_id" & _
" order by created_date desc "
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


    Public Shared Function updateCheque(ByVal paid As Boolean, ByVal id As String) As Boolean
        Try
            Dim status As String = ""
            Dim sql_date As String = ""

            If paid Then
                status = 1
                sql_date = ",payment_duedate = getdate() "
            Else
                status = 0
            End If


            Dim sql As String = String.Format("UPDATE ticket_split set paid = '{0}' {2} WHERE ticket_sp_id = '{1}' ", status, id, sql_date)
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

#End Region

#Region "Stock"
    'Public Shared Function updateStockStatus(ByVal quantity As Double, ByVal amount As Double, ByVal order_type As String, ByVal purity As String, ByVal before_status As String, ByVal cmd As SqlCommand) As Boolean
    '    Try
    '        Dim sql As String = String.Format("update stock set bid96 = {0},ask96 = {1},bid99 = {2},ask99 = {3},pwd_auth={4},margin = {5}", bid96, ask96, bid99, ask99, pwd_auth, margin)
    '        Dim con As New SqlConnection(strcon)
    '        Dim sql_stock As String = ""
    '        Dim gold_type_id As String = ""
    '        Dim amount_base As Double = 0
    '        Dim quantity_base As Double = 0

    '        gold_type_id = "G" + purity + "_base"
    '        If order_type = "sell" Then 'Sell : -gold , +cash
    '            'quantity_base = quantity * -1
    '            'amount_base = amount
    '        ElseIf order_type = "buy" Then 'Buy : +gold , -cash
    '            'quantity_base = quantity
    '            'amount_base = amount * -1
    '        Else
    '            Throw New Exception("Unknow order_type")
    '        End If
    '        sql_stock = String.Format(";update stock set price_base = {0}, {2} = {1} ", amount_base, quantity_base, gold_type_id)

    '        cmd.CommandText = sql
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

    Public Shared Function rollback_status(ByVal status_id As String, ByVal quantity As Double, ByVal amount As Double, ByVal order_type As String) As Double
        Try
            'Rollback all to pending(101)
            If order_type = "sell" Then

            End If
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
        Dim dt As New DataTable
        Try
            Using da As New SqlDataAdapter("exec getStockSumDeposit", strcon)
                da.Fill(dt)
                Return dt
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            dt.Dispose()
        End Try

    End Function

#End Region

#Region "Asset"

    Public Shared Function updateAssetStatus(ByVal status As String, ByVal quantity As Double, ByVal user_id As String, ByVal asset_id As String, ByVal remark As String, ByVal asset_type As String, ByVal type As String, ByVal purity As String, ByVal payment As String, ByVal before_quantity As Double) As Boolean
        Dim con As New SqlConnection(strcon)
        con.Open()
        Dim tr As SqlTransaction = con.BeginTransaction

        Try
            Dim sql As String = String.Format("update asset set status = '{0}',quantity = {1},modifier_by='{2}',remark = @remark,modifier_date=getdate(),datetime=getdate() where asset_id = '{3}'", status, quantity, user_id, asset_id)
            Dim cmd = New SqlCommand
            cmd.Connection = con

            Dim fieldName As String = ""

            If asset_type = "Cash" Then
                fieldName = "price_base"
            Else
                If purity = "96" Then
                    fieldName = "G96_base"
                ElseIf purity = "99" Then
                    fieldName = "G99_base"
                ElseIf purity = "96G" Then
                    fieldName = "G96G_base"
                End If
            End If

            Dim money_stock As Double = 0
            Dim quan_stock As Double = 0
            Dim dt_stock As New Data.DataTable
            dt_stock = getStockSumDeposit()
            cmd.Transaction = tr
            If dt_stock.Rows.Count > 0 Then
                If type = "In" Then
                    quan_stock = clsManage.convert2zero(dt_stock.Rows(0)(fieldName)) + quantity - before_quantity 'for update stock (gold or price_base)
                    If asset_type = "Cash" Then money_stock = clsManage.convert2zero(dt_stock.Rows(0)(payment)) + quantity - before_quantity 'for update stock (field > cash or trans or cheq)
                Else
                    quan_stock = clsManage.convert2zero(dt_stock.Rows(0)(fieldName)) - quantity + before_quantity
                    If asset_type = "Cash" Then money_stock = clsManage.convert2zero(dt_stock.Rows(0)(payment)) - quantity + before_quantity

                End If
            End If

                Dim sql_stock As String = String.Format(";update stock set {1} = {0} ", quan_stock, fieldName)

                If asset_type = "Cash" Then
                    sql_stock += String.Format(",{0} = {1}", payment, money_stock)
                End If


                cmd.CommandText = sql + sql_stock
                Dim parameter As New SqlParameter("@remark", SqlDbType.VarChar)
                parameter.Value = remark
                cmd.Parameters.Add(parameter)

                cmd.Transaction = tr
                Dim result As Integer = cmd.ExecuteNonQuery()

                If result > 0 Then
                    tr.Commit()
                    Return True
                Else
                    tr.Rollback()
                    Return False
                End If
        Catch ex As Exception
            tr.Rollback()
            Throw ex
        Finally
            con.Close()
        End Try

    End Function

    Public Shared Function updateAssetStatus2(ByVal status As String, ByVal quantity As Double, ByVal user_id As String, ByVal asset_id As String, ByVal remark As String, ByVal asset_type As String, ByVal type As String, ByVal purity As String, ByVal payment As String, ByVal before_quantity As Double) As Boolean
        Dim con As New SqlConnection(strcon)
        con.Open()
        Dim tr As SqlTransaction = con.BeginTransaction

        Try
            Dim sql As String = String.Format("update asset set status = '{0}',quantity = {1},modifier_by='{2}',remark = @remark,modifier_date=getdate(),datetime=getdate() where asset_id = '{3}'", status, quantity, user_id, asset_id)
            Dim cmd = New SqlCommand
            cmd.Connection = con

            Dim fieldName As String = ""

            If asset_type = "Cash" Then
                fieldName = "price_base"
            Else
                If purity = "96" Then
                    fieldName = "G96_base"
                ElseIf purity = "99N" Then
                    fieldName = "G99N_base"
                ElseIf purity = "99L" Then
                    fieldName = "G99L_base"
                End If
            End If

            Dim cash As Double = 0 : Dim trans As Double = 0 : Dim cheq As Double = 0 : Dim money As Double = 0 : Dim money_stock As Double = 0
            Dim price_base As Double = 0 : Dim G96_base As Double = 0
            Dim G99N_base As Double = 0 : Dim G99L_base As Double = 0
            Dim quantity_actual As Double = 0 : Dim amount_actual As Double = 0
            Dim dt_stock As New Data.DataTable
            dt_stock = getStockSumDeposit()
            price_base = clsManage.convert2zero(dt_stock.Rows(0)("priceDep"))
            G96_base = clsManage.convert2zero(dt_stock.Rows(0)("G96Dep"))
            G99N_base = clsManage.convert2zero(dt_stock.Rows(0)("G99NDep"))
            G99L_base = clsManage.convert2zero(dt_stock.Rows(0)("G99LDep"))
            cash = clsManage.convert2zero(dt_stock.Rows(0)("cashDep"))
            trans = clsManage.convert2zero(dt_stock.Rows(0)("transDep"))
            cheq = clsManage.convert2zero(dt_stock.Rows(0)("cheqDep"))


            cmd.Transaction = tr

            If dt_stock.Rows.Count > 0 Then
                If type = "In" Then

                    If purity = "96" Then
                        G96_base = G96_base + quantity - before_quantity
                    ElseIf purity = "99N" Then
                        G99N_base = G99N_base + quantity - before_quantity
                    ElseIf purity = "99L" Then
                        G99L_base = G99L_base + quantity - before_quantity
                    ElseIf purity = "" Then
                        price_base = price_base + quantity - before_quantity
                        money = clsManage.convert2zero(dt_stock.Rows(0)(payment + "Dep")) + quantity - before_quantity
                    End If

                    'If asset_type = "Gold" Then
                    '    quantity_actual = quantity - before_quantity
                    '    amount_actual = 0
                    'Else
                    '    quantity_actual = 0
                    '    amount_actual = quantity - before_quantity
                    'End If

                    'sql_actual = clsDB.insert_actual(asset_id, "", user_id, purity, quantity_actual, amount_actual, "998", "", "In", "In/Out", price_base, G96_base, G99N_base, G99L_base, "")
                    quantity = clsManage.convert2zero(dt_stock.Rows(0)(fieldName)) + quantity - before_quantity 'for update stock (gold or price_base)
                    money_stock = clsManage.convert2zero(dt_stock.Rows(0)(payment)) + quantity - before_quantity 'for update stock (field > cash or trans or cheq)
                Else
                    If purity = "96" Then
                        G96_base = G96_base - quantity + before_quantity
                    ElseIf purity = "99N" Then
                        G99N_base = G99N_base - quantity + before_quantity
                    ElseIf purity = "99L" Then
                        G99L_base = G99L_base - quantity + before_quantity
                    ElseIf purity = "" Then
                        price_base = price_base - quantity + before_quantity
                    End If
                    quantity = clsManage.convert2zero(dt_stock.Rows(0)(fieldName)) - quantity + before_quantity
                    money_stock = clsManage.convert2zero(dt_stock.Rows(0)(payment)) - quantity + before_quantity 'for update stock (field > cash or trans or cheq)
                End If
            End If
            Dim sql_stock As String = String.Format(";update stock set {1} = {0} ", quantity, fieldName)

            If asset_type = "Cash" Then
                sql_stock += String.Format(",{0} = {1}", payment, quantity)
            End If


            cmd.CommandText = sql + sql_stock
            Dim parameter As New SqlParameter("@remark", SqlDbType.VarChar)
            parameter.Value = remark
            cmd.Parameters.Add(parameter)

            cmd.Transaction = tr
            Dim result As Integer = cmd.ExecuteNonQuery()

            If result > 0 Then
                tr.Commit()
                Return True
            Else
                tr.Rollback()
                Return False
            End If
        Catch ex As Exception
            tr.Rollback()
            Throw ex
        Finally
            con.Close()
        End Try

    End Function

    Public Shared Function getAsset(ByVal assetType As String, ByVal purity As String) As DataTable
        Try
            Dim sql As String = "select *,users.user_name from ( " & _
" SELECT  asset.asset_id, asset.asset_type, asset.created_by, asset.datetime, asset.purity, asset.quantity, asset.remark, asset.type" & _
 " FROM asset" & _
 " union" & _
" select '' as asset_id,case when type='sell' then 'Cash' when type='buy' then 'Gold' else '' end as asset_type," & _
" created_by,delivery_date as datetime,gold_type_id as purity," & _
" case when type='sell' then amount when type='buy' then quantity else '' end as quantity,remark,'In' as type" & _
 " from ticket where delivery_date is not null and convert(varchar,delivery_date,111) <= convert(varchar,getdate(),111) and status_id = 901" & _
 " union" & _
" select '' as asset_id,case when type='sell' then 'Gold' when type='buy' then 'Cash' else '' end as asset_type," & _
" created_by,delivery_date as datetime,gold_type_id as purity," & _
" case when type='sell' then quantity when type='buy' then amount else '' end as quantity,remark,'Out' as type" & _
 " from ticket where delivery_date is not null and convert(varchar,delivery_date,111) <= convert(varchar,getdate(),111) and status_id = 901" & _
" )importExport  INNER JOIN " & _
" users ON importExport.created_by = users.user_id" & _
" WHERE (asset_type = @asset_type or @asset_type = '') and  (purity = @purity or @purity='') order by datetime desc"
            '**************only status = complete ส่งมอบ

            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)

            Dim parameter As New SqlParameter("@asset_type", SqlDbType.VarChar)
            parameter.Value = assetType
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@purity", SqlDbType.VarChar)
            parameter.Value = purity
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

    Public Shared Function getAssetInout(ByVal assetType As String, ByVal purity As String) As DataTable
        Try
            'case when type = 'In' then quantity when type='Out' then -(quantity) else 0 end as quantity
            Dim sql As String = "select asset.asset_id, asset.asset_type, asset.type, asset.purity, asset.payment, asset.remark, asset.status, asset.datetime, asset.created_date, " & _
            " asset.modifier_date, asset.quantity,user_created.user_name AS created_by, user_updated.user_name AS modifier_by " & _
            " from asset LEFT OUTER JOIN " & _
            " users AS user_created ON asset.created_by = user_created.user_id LEFT OUTER JOIN " & _
            " users AS user_updated ON asset.modifier_by = user_updated.user_id " & _
            " where (asset_type = @asset_type or @asset_type = '') and  (purity = @purity or @purity='') order by created_date desc"

            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)

            Dim parameter As New SqlParameter("@asset_type", SqlDbType.VarChar)
            parameter.Value = assetType
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@purity", SqlDbType.VarChar)
            parameter.Value = purity
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

    Public Shared Function getAssetPlanning(ByVal assetType As String, ByVal purity As String, Optional ByVal planningDay As String = "0", Optional ByVal delivery_date_null As String = "0") As DataTable
        Try
            Dim sql As String = ""
            If purity <> "" Then
                purity = String.Format("AND purity in ('{0}')", purity)
            End If

            Dim sql_delivery_date_null As String = ""
            If delivery_date_null <> "0" Then
                sql_delivery_date_null = "             union" & _
               " select '' as asset_id,ref_no,case when type='sell' then 'Cash' when type='buy' then 'Gold' else '' end as asset_type, " & _
               " created_by,delivery_date as datetime,gold_type_id as purity, case when type='sell' then amount when type='buy' then quantity else '' end as quantity," & _
               " remark,'In' as type,status_id,type as order_type from v_ticket_sum_split " & _
               " where delivery_date is  null  and status_id in(901,902) " & _
               "  union" & _
               " select '' as asset_id,ref_no,case when type='sell' then 'Gold' when type='buy' then 'Cash' else '' end as asset_type, " & _
               " created_by,delivery_date as datetime,gold_type_id as purity, case when type='sell' then quantity when type='buy' then amount else '' end as quantity," & _
               " remark,'Out' as type,status_id,type as order_type from v_ticket_sum_split " & _
               " where delivery_date is  null and status_id in(901,902) "
            End If

            sql = String.Format(" select *,gold_company101 + gold_company102 + cash_company103 + gold_company104 + gold_company901 + gold_company902 as gold_company, " & _
" cash_company101 + cash_company102 + cash_company105 + cash_company901 + cash_company902 as cash_company," & _
" case when (ref_no = '' and type = 'In' and asset_type = 'Cash') then quantity when(ref_no = '' and type = 'Out' and asset_type = 'Cash') then -(quantity) else 0 end as cash_base," & _
" case when (ref_no = '' and type = 'In' and asset_type = 'Gold') then quantity when(ref_no = '' and type = 'Out' and asset_type = 'Gold') then -(quantity) else 0 end as gold_base" & _
" from (" & _
" select *," & _
" case when (status_id = 101 and type = 'Out' and order_type = 'sell') then -(quantity) " & _
" 	 when (status_id = 101 and type = 'In' and order_type = 'buy') then quantity else 0 end as gold_company101," & _
" case when (status_id = 101 and type = 'In' and order_type = 'sell') then quantity " & _
" 	 when (status_id = 101 and type = 'Out' and order_type = 'buy') then -(quantity) else 0 end as cash_company101," & _
" case when (status_id = 103 and type = 'In') then quantity else 0 end as cash_company103," & _
" case when (status_id = 102 and type = 'In' and order_type = 'buy') then quantity else 0 end as gold_branch," & _
" case when (status_id = 102 and type = 'In' and order_type = 'sell') then quantity else 0 end as cash_branch," & _
" case when (status_id = 102 and type = 'Out' and order_type = 'buy') then -(quantity) else 0 end as cash_company102," & _
" case when (status_id = 102 and type = 'Out' and order_type = 'sell') then -(quantity) else 0 end as gold_company102," & _
" case when (status_id = 104 and type = 'Out' and order_type = 'sell') then -(quantity) else 0 end as gold_company104," & _
" case when (status_id = 105 and type = 'Out' and order_type = 'buy') then -(quantity) else 0 end as cash_company105," & _
" case when (status_id = 901 and type = 'Out' and order_type = 'sell') then -(quantity) " & _
" 	 when (status_id = 901 and type = 'In' and order_type = 'buy') then quantity else 0 end as gold_company901," & _
" case when (status_id = 901 and type = 'In' and order_type = 'sell') then quantity " & _
" 	 when (status_id = 901 and type = 'Out' and order_type = 'buy') then -(quantity) else 0 end as cash_company901," & _
" case when (status_id = 902 and type = 'Out' and order_type = 'sell') then -(quantity) " & _
" 	 when (status_id = 902 and type = 'In' and order_type = 'buy') then quantity else 0 end as gold_company902," & _
" case when (status_id = 902 and type = 'In' and order_type = 'sell') then quantity " & _
" 	 when (status_id = 902 and type = 'Out' and order_type = 'buy') then -(quantity) else 0 end as cash_company902," & _
" case when (status_id = 902 and type = 'Out' and order_type = 'sell') then quantity else 0 end as gold_customer," & _
" case when (status_id = 902 and type = 'Out' and order_type = 'buy') then quantity else 0 end as cash_customer" & _
" from ( " & _
" SELECT  asset.asset_id,'' as ref_no, asset.asset_type, asset.created_by," & _
" asset.datetime, asset.purity, case when type='In' then quantity else -(quantity) end as quantity, asset.remark, asset.type,'' as status_id,'' as order_type FROM asset " & _
" where datetime between getdate() and DATEADD(day, @day, getdate())" & _
"             union" & _
" select '' as asset_id,ref_no,case when type='sell' then 'Cash' when type='buy' then 'Gold' else '' end as asset_type, " & _
" created_by,delivery_date as datetime,gold_type_id as purity, case when type='sell' then amount when type='buy' then quantity else '' end as quantity," & _
" remark,'In' as type,status_id,type as order_type from v_ticket_sum_split " & _
" where delivery_date is not null  and status_id in(101,102,103,104,105,901,902) and  delivery_date between getdate() and DATEADD(day, @day, getdate())" & _
"  union" & _
" select '' as asset_id,ref_no,case when type='sell' then 'Gold' when type='buy' then 'Cash' else '' end as asset_type, " & _
" created_by,delivery_date as datetime,gold_type_id as purity, case when type='sell' then quantity when type='buy' then amount else '' end as quantity," & _
" remark,'Out' as type,status_id,type as order_type from v_ticket_sum_split " & _
" where delivery_date is not null and status_id in(101,102,103,104,105,901,902) and  delivery_date between getdate() and DATEADD(day, @day, getdate())" & _
" " + sql_delivery_date_null + " " & _
" )importExport  " & _
" INNER JOIN  users ON importExport.created_by = users.user_id " & _
" ) sumStock" & _
" Where (asset_type = @asset_type or @asset_type ='') {0} order by datetime desc ", purity)

            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)

            Dim parameter As New SqlParameter("@asset_type", SqlDbType.VarChar)
            parameter.Value = assetType
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@day", SqlDbType.Int)
            parameter.Value = planningDay
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

    Public Shared Function getAssetActual(ByVal assetType As String, ByVal purity As String, Optional ByVal actualDay As String = "-1") As DataTable
        Try
            Dim sql As String = ""
            If purity <> "" Then
                purity = String.Format("AND purity in ('{0}')", purity)
            End If

            sql = String.Format(" select *,gold_company101 + gold_company102 + cash_company103 + gold_company104 + gold_company901 + gold_company902 as gold_company, " & _
" cash_company101 + cash_company102 + cash_company105 + cash_company901 + cash_company902 as cash_company," & _
" case when (ref_no = '' and type = 'In' and asset_type = 'Cash') then quantity when(ref_no = '' and type = 'Out' and asset_type = 'Cash') then -(quantity) else 0 end as cash_base," & _
" case when (ref_no = '' and type = 'In' and asset_type = 'Gold') then quantity when(ref_no = '' and type = 'Out' and asset_type = 'Gold') then -(quantity) else 0 end as gold_base" & _
" from (" & _
" select *," & _
" case when (status_id = 101 and type = 'Out' and order_type = 'sell') then -(quantity) " & _
" 	 when (status_id = 101 and type = 'In' and order_type = 'buy') then quantity else 0 end as gold_company101," & _
" case when (status_id = 101 and type = 'In' and order_type = 'sell') then quantity " & _
" 	 when (status_id = 101 and type = 'Out' and order_type = 'buy') then -(quantity) else 0 end as cash_company101," & _
" case when (status_id = 103 and type = 'In') then quantity else 0 end as cash_company103," & _
" case when (status_id = 102 and type = 'In' and order_type = 'buy') then quantity else 0 end as gold_branch," & _
" case when (status_id = 102 and type = 'In' and order_type = 'sell') then quantity else 0 end as cash_branch," & _
" case when (status_id = 102 and type = 'Out' and order_type = 'buy') then -(quantity) else 0 end as cash_company102," & _
" case when (status_id = 102 and type = 'Out' and order_type = 'sell') then -(quantity) else 0 end as gold_company102," & _
" case when (status_id = 104 and type = 'Out' and order_type = 'sell') then -(quantity) else 0 end as gold_company104," & _
" case when (status_id = 105 and type = 'Out' and order_type = 'buy') then -(quantity) else 0 end as cash_company105," & _
" case when (status_id = 901 and type = 'Out' and order_type = 'sell') then -(quantity) " & _
" 	 when (status_id = 901 and type = 'In' and order_type = 'buy') then quantity else 0 end as gold_company901," & _
" case when (status_id = 901 and type = 'In' and order_type = 'sell') then quantity " & _
" 	 when (status_id = 901 and type = 'Out' and order_type = 'buy') then -(quantity) else 0 end as cash_company901," & _
" case when (status_id = 902 and type = 'Out' and order_type = 'sell') then -(quantity) " & _
" 	 when (status_id = 902 and type = 'In' and order_type = 'buy') then quantity else 0 end as gold_company902," & _
" case when (status_id = 902 and type = 'In' and order_type = 'sell') then quantity " & _
" 	 when (status_id = 902 and type = 'Out' and order_type = 'buy') then -(quantity) else 0 end as cash_company902," & _
" case when (status_id = 902 and type = 'Out' and order_type = 'sell') then quantity else 0 end as gold_customer," & _
" case when (status_id = 902 and type = 'Out' and order_type = 'buy') then quantity else 0 end as cash_customer" & _
" from ( " & _
" SELECT  asset.asset_id,'' as ref_no, asset.asset_type, asset.created_by," & _
" asset.datetime, asset.purity, case when type='In' then quantity else -(quantity) end as quantity, asset.remark, asset.type,'' as status_id,'' as order_type FROM asset " & _
" where datetime between  DATEADD(day, @day, getdate()) and getdate()" & _
"             union" & _
" select '' as asset_id,ref_no,case when type='sell' then 'Cash' when type='buy' then 'Gold' else '' end as asset_type, " & _
" created_by,delivery_date as datetime,gold_type_id as purity, case when type='sell' then amount when type='buy' then quantity else '' end as quantity," & _
" remark,'In' as type,status_id,type as order_type from v_ticket_sum_split " & _
" where status_id in(901,902) and delivery_date between  DATEADD(day, @day, getdate()) and getdate() " & _
"  union" & _
" select '' as asset_id,ref_no,case when type='sell' then 'Gold' when type='buy' then 'Cash' else '' end as asset_type, " & _
" created_by,delivery_date as datetime,gold_type_id as purity, case when type='sell' then quantity when type='buy' then amount else '' end as quantity," & _
" remark,'Out' as type,status_id,type as order_type from v_ticket_sum_split " & _
" where status_id in(901,902) and delivery_date between  DATEADD(day, @day, getdate()) and getdate() " & _
" )importExport  " & _
" INNER JOIN  users ON importExport.created_by = users.user_id " & _
" ) sumStock" & _
" Where (asset_type = @asset_type or @asset_type ='') {0} order by datetime desc ", purity)

            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)

            Dim parameter As New SqlParameter("@asset_type", SqlDbType.VarChar)
            parameter.Value = assetType
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@day", SqlDbType.Int)
            parameter.Value = actualDay
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

    'For Gridview
    Public Shared Function getActual2(ByVal assetType As String, ByVal purity As String, Optional ByVal actualDay As String = "0") As DataTable
        Try
            Dim sql As String = ""
            If purity <> "" Then
                purity = purity + "','"
                purity = String.Format("AND purity in ('{0}')", purity)
            End If
            Dim sql_dep As String = ""

            If assetType = "Gold" Then
                sql_dep = " Union select '' as asset_id,'' as ref_no, " & _
                                " 'Gold'  as asset_type,  " & _
                                " created_by,created_date as datetime,gold_type_id as purity, " & _
                                " case when (gold_type_id='' and type='Deposit') then amount  " & _
                                " 	 when (gold_type_id='' and type='Withdraw') then -(amount) " & _
                                " 	 when (gold_type_id<>'' and type='Deposit') then quantity " & _
                                " 	 when (gold_type_id<>'' and type='withdraw') then -(quantity) else '' end as quantity, " & _
                                " remark, [type],'' as status_id,'-' as order_type " & _
                                " from customer_trans where pre ='n' and created_date between  DATEADD(day, @day, convert(varchar,getdate(),111)) and getdate() and gold_type_id <> '' "
            ElseIf assetType = "Cash" Then
                sql_dep = " Union select '' as asset_id,'' as ref_no, " & _
                               " 'Cash'  as asset_type,  " & _
                               " created_by,created_date as datetime,gold_type_id as purity, " & _
                               " case when (gold_type_id='' and type='Deposit') then amount  " & _
                               " 	 when (gold_type_id='' and type='Withdraw') then -(amount) " & _
                               " 	 when (gold_type_id<>'' and type='Deposit') then quantity " & _
                               " 	 when (gold_type_id<>'' and type='withdraw') then -(quantity) else '' end as quantity, " & _
                               " remark, [type],'' as status_id,'-' as order_type " & _
                               " from customer_trans where pre ='n' and created_date between  DATEADD(day, @day, convert(varchar,getdate(),111)) and getdate() and gold_type_id = '' "
            End If
            Dim sql_current As String = "" & _
            " select '' as asset_id,'' as ref_no,'Cash' as asset_type,'' as created_by, DATEADD(day, @day, convert(varchar,getdate(),111)) as datetime,'' as purity," & _
            " 0 as quantity,'Begin ' as remark,'' as type,'' as status_id,'-' as order_type" & _
            " from stock " & _
            " UNION select '' as asset_id,'' as ref_no,'Gold' as asset_type,'' as created_by,DATEADD(day, @day, convert(varchar,getdate(),111))  as datetime,'' as purity," & _
            " 0 as quantity,'Begin ' as remark,'' as type,'' as status_id,'-' as order_type" & _
            " from stock " & _
            " UNION  "
            'sql_current = ""
            '******** ถ้าเป็น deposit จาก ticket pre จะเป็น NULL
            '******** Cash ถ้ามี cheq(payment_duedate) จะไม่สนใจ delivery_date


            '******* for ทองออกรอเงิน,เงินออกรอทอง
            Dim sql_out_wait As String = " Union" & _
            " select '' as asset_id,ref_no," & _
            " case when type='sell' then 'Gold' when type='buy' then 'Cash' else '' end as asset_type," & _
            " created_by,payment_date  as datetime,gold_type_id as purity," & _
            " case when type='sell' then quantity when type='buy' then amount else '' end as quantity," & _
            " case when type='sell' then 'ทองออก รอเงิน' when type='buy' then 'เงินออก รอทอง' else '' end as remark," & _
            " case when type='sell' then 'Out' when type='buy' then 'In' else '' end as type," & _
            " status_id,type as order_type from ticket" & _
            " where payment_date between  DATEADD(day, @day, convert(varchar,getdate(),111)) and getdate() "

            sql = String.Format(" select * " & _
" from ( " & _
" " + sql_current + " " & _
" SELECT  asset.asset_id,'' as ref_no, asset.asset_type, asset.created_by," & _
" asset.datetime, asset.purity, case when type='In' then quantity else -(quantity) end as quantity, asset.remark, asset.type,'' as status_id,'' as order_type FROM asset " & _
" where status='actual' and modifier_date between  DATEADD(day, @day, convert(varchar,getdate(),111)) and getdate()" & _
"             union" & _
" select '' as asset_id,ref_no,case when type='sell' then 'Cash' when type='buy' then 'Gold' else '' end as asset_type, " & _
" created_by,case when payment_duedate is not null then payment_duedate else delivery_date end as datetime,gold_type_id as purity, case when type='sell' then amount when type='buy' then quantity else '' end as quantity, " & _
" remark,'In' as type,status_id,type as order_type from v_ticket_all_split  " & _
" where status_id in(901,902,903) and (delivery_date between  DATEADD(day, @day, convert(varchar,getdate(),111)) and getdate()  and payment_duedate is null) " & _
" or payment_duedate between  DATEADD(day,@day, convert(varchar,getdate(),111)) and getdate()  and payment_date is null " & _
"  union" & _
" select '' as asset_id,ref_no,case when type='sell' then 'Gold' when type='buy' then 'Cash' else '' end as asset_type, " & _
" created_by,delivery_date as datetime,gold_type_id as purity, case when type='sell' then -(quantity) when type='buy' then -(amount) else '' end as quantity," & _
" remark,'Out' as type,status_id,type as order_type from v_ticket_all_split " & _
" where status_id in(901,902,903) and delivery_date between  DATEADD(day, @day, convert(varchar,getdate(),111)) and getdate()  and payment_date is null" & _
" " + sql_out_wait + " " & _
" " + sql_dep + " " & _
" )actual  " & _
" LEFT OUTER JOIN  users ON actual.created_by = users.user_id " & _
" Where (asset_type = @asset_type or @asset_type ='') {0} order by datetime asc ", purity)

            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)

            Dim parameter As New SqlParameter("@asset_type", SqlDbType.VarChar)
            parameter.Value = assetType
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@day", SqlDbType.Int)
            parameter.Value = actualDay
            cmd.Parameters.Add(parameter)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            Return dt
            'If dt.Rows.Count > 0 Then
            '    Return dt
            'Else
            '    Return Nothing
            'End If

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    Public Shared Function getActual(ByVal assetType As String, ByVal purity As String, Optional ByVal actualDay As String = "0") As DataTable
        Try
            If purity <> "" Then
                purity = purity + "','"
                purity = String.Format("AND actual.purity in ('{0}')", purity)
            End If

            Dim sql_begin As String = " select  '' as actual_id,'' as asset_id,'' as ref_no,'' as cust_id,'' as asset_type,'' as created_by,DATEADD(day, @day, convert(varchar,getdate(),111))  as datetime,'' as purity, " & _
            " 0.0 as quantity,0.0 as amount,'' as status_id,'' as before_status_id,'Begin ' as status_name,'' as note ,'' as type,'' as order_type," & _
            " isnull((select price_base from actual where actual_id in(select max(actual_id) as maxid from actual where convert(varchar,datetime,111) < DATEADD(day,@day , convert(varchar,getdate(),111)))),-1) as price_base, " & _
            " isnull((select G96_base from actual where actual_id in(select max(actual_id) as maxid from actual where convert(varchar,datetime,111) < DATEADD(day, @day, convert(varchar,getdate(),111)))),-1) as G96_base," & _
            " isnull((select G96G_base from actual where actual_id in(select max(actual_id) as maxid from actual where convert(varchar,datetime,111) < DATEADD(day, @day, convert(varchar,getdate(),111)))),-1) as G96G_base," & _
            " isnull((select G99_base from actual where actual_id in(select max(actual_id) as maxid from actual where convert(varchar,datetime,111) < DATEADD(day, @day, convert(varchar,getdate(),111)))),-1) as G99_base," & _
            " '' as payment, " & _
            " isnull((select cash from actual where actual_id in(select max(actual_id) as maxid from actual where convert(varchar,datetime,111) < DATEADD(day, 0, convert(varchar,getdate(),111)))),-1) as cash," & _
            " isnull((select trans from actual where actual_id in(select max(actual_id) as maxid from actual where convert(varchar,datetime,111) < DATEADD(day, 0, convert(varchar,getdate(),111)))),-1) as trans, " & _
            " isnull((select cheq from actual where actual_id in(select max(actual_id) as maxid from actual where convert(varchar,datetime,111) < DATEADD(day, 0, convert(varchar,getdate(),111)))),-1) as cheq," & _
            " '' as user_name,'' as firstname from stock "

            Dim sql As String = String.Format(sql_begin + " UNION SELECT actual.actual_id,actual.asset_id, actual.ref_no, actual.cust_id, actual.asset_type, actual.created_by, actual.datetime, actual.purity, actual.quantity, actual.amount, " & _
            " actual.status_id, actual.before_status_id, actual.status_name, actual.note, actual.type, actual.order_type, actual.price_base, actual.G96_base, actual.G96G_base, " & _
            " actual.G99_base, actual.payment, actual.cash, actual.trans, actual.cheq, " & _
            " users.user_name,customer.firstname from " & _
            " customer RIGHT OUTER JOIN actual ON customer.cust_id = actual.cust_id LEFT OUTER JOIN users ON actual.created_by = users.user_id " & _
            " Where (asset_type = @asset_type or @asset_type ='') and datetime between  DATEADD(day, @day, convert(varchar,getdate(),111)) and getdate() {0} order by datetime asc ", purity)

            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)

            Dim parameter As New SqlParameter("@asset_type", SqlDbType.VarChar)
            parameter.Value = assetType
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@day", SqlDbType.Int)
            parameter.Value = actualDay
            cmd.Parameters.Add(parameter)

            Dim da As New SqlDataAdapter(cmd)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows(0)("price_base") = -1 Then
                Return Nothing
            End If
            Return dt

        Catch ex As Exception
            Throw ex
        End Try

    End Function

    'For Gridview
    Public Shared Function getPlanning(ByVal assetType As String, ByVal purity As String, Optional ByVal planningDay As String = "0", Optional ByVal delivery_date_null As String = "0", Optional ByVal deposit As String = "n") As DataTable
        Try
            Dim sql As String = ""
            If purity <> "" Then
                purity = purity + "','"
                purity = String.Format("AND purity in ('{0}')", purity)
            End If
            Dim planningDayPlus = planningDay + 1
            Dim sql_delivery_date_null As String = ""
            If delivery_date_null <> "0" Then
                sql_delivery_date_null = String.Format("             union" & _
               " select cust_id,'' as asset_id,ref_no,case when type='sell' then 'Cash' when type='buy' then 'Gold' else '' end as asset_type, " & _
               " created_by,DATEADD(day, {0}, getdate()) as datetime,gold_type_id as purity, case when type='sell' then amount when type='buy' then quantity else '' end as quantity," & _
               " remark,'In' as type,status_id,type as order_type,payment from v_ticket_sum_split " & _
               " where delivery_date is  null  and status_id in(101) " & _
               "  union" & _
               " select cust_id,'' as asset_id,ref_no,case when type='sell' then 'Gold' when type='buy' then 'Cash' else '' end as asset_type, " & _
               " created_by,DATEADD(day, {0}, getdate()) as datetime,gold_type_id as purity, case when type='sell' then -(quantity) when type='buy' then -(amount) else '' end as quantity," & _
               " remark,'Out' as type,status_id,type as order_type,payment from v_ticket_sum_split " & _
               " where delivery_date is  null and status_id in(101) ", planningDayPlus)
            End If
            Dim sql_deposit As String = ""
            If deposit = "y" Then
                sql_deposit = " Union select cust_id,'' as asset_id,'' as ref_no, " & _
                " case when gold_type_id='' then 'Cash' else 'Gold' end as asset_type,  " & _
                " created_by,datetime,gold_type_id as purity, " & _
                " case when (gold_type_id='' and type='Deposit') then amount  " & _
                " 	 when (gold_type_id='' and type='Withdraw') then -(amount) " & _
                " 	 when (gold_type_id<>'' and type='Deposit') then quantity " & _
                " 	 when (gold_type_id<>'' and type='withdraw') then -(quantity) else '' end as quantity, " & _
                " remark, " & _
                " case when [type]='Deposit' then 'In' when [type]='Withdraw' then 'Out' else '' end as [type],'' as status_id,'-' as order_type,payment " & _
                " from customer_trans where pre = 'y' and datetime between convert(varchar,getdate(),111) and DATEADD(ss,-1, DATEADD(day,@day, convert(varchar,getdate(),111))) "
            End If

            ' cheq in tbl tickets (out)
            Dim sql_cheq_inTicket_out As String = "Union " & _
            " select cust_id,'' as asset_id, ref_no,'Cash' as asset_type, created_by,payment_duedate as datetime,gold_type_id as purity, " & _
            " case when  type='sell' then amount when  type='buy' then -(amount) else '' end as quantity, 'Cheq' as remark, " & _
            " case when  type='sell' then 'In' when type='buy' then 'Out' else '' end as type,status_id,type as order_type,payment  " & _
            " from tickets where payment='cheq' and  ref_no not in (select ref_no from ticket_split) " & _
            " and payment_duedate between  convert(varchar,getdate(),111) and DATEADD(ss,-1, DATEADD(day,@day, convert(varchar,getdate(),111)))  "

            '1 st row  tbl Stock asset_type = ''
            '2 nd row tbl Stock asset_type = Gold
            ' tbl Asset status = 'planning'
            ' tbl ticket type = in and out case not in ref_no(has cheq)
            ' tbl Deposit status = pre
            ' cheq in tbl ticket_split (in)
            ' cheq in tbl tickets (out)
            sql = String.Format(" select planning.*,users.user_name,customer.firstname " & _
" from ( " & _
" select '' as cust_id,'' as asset_id,'' as ref_no,'Cash' as asset_type,'' as created_by,convert(varchar,getdate(),111) as datetime,'' as purity," & _
" 0 as quantity,'Current Stock' as remark,'' as type,'' as status_id,'-' as order_type,'' as payment " & _
" from stock " & _
" UNION select '' as cust_id,'' as asset_id,'' as ref_no,'Gold' as asset_type,'' as created_by,convert(varchar,getdate(),111)  as datetime,'' as purity," & _
" 0 as quantity,'Current Stock' as remark,'' as type,'' as status_id,'-' as order_type,'' as payment " & _
" from stock " & _
" UNION SELECT  '' as cust_id,asset.asset_id,'' as ref_no, asset.asset_type, asset.created_by," & _
" asset.datetime, asset.purity, case when type='In' then quantity else -(quantity) end as quantity, asset.remark, asset.type,'' as status_id,'' as order_type,payment " & _
" FROM asset " & _
" where status='planning' and datetime between  convert(varchar,getdate(),111) and DATEADD(ss,-1, DATEADD(day,@day, convert(varchar,getdate(),111)))" & _
"             union" & _
" select *  from( select cust_id,'' as asset_id,ref_no,case when type='sell' then 'Cash' when type='buy' then 'Gold' else '' end as asset_type,created_by, " & _
" case when (type='sell' and payment='cheq' and payment_duedate is not null) then payment_duedate else  delivery_date end as datetime, " & _
" gold_type_id as purity, case when type='sell' then amount when type='buy' then quantity else '' end as quantity," & _
" remark,'In' as type,status_id,type as order_type,payment from v_ticket_sum_split ) ticket_in " & _
" where datetime is not null  and status_id in(101,102,103,104,105) and  datetime between  convert(varchar,getdate(),111) and DATEADD(ss,-1, DATEADD(day,@day, convert(varchar,getdate(),111))) " & _
"  " & _
"  " & _
"  union" & _
" select * from (select cust_id,'' as asset_id,ref_no,case when type='sell' then 'Gold' when type='buy' then 'Cash' else '' end as asset_type,created_by, " & _
" case when (type='buy' and payment='cheq' and payment_duedate is not null) then payment_duedate else  delivery_date end as datetime, " & _
" gold_type_id as purity, case when type='sell' then -(quantity) when type='buy' then -(amount) else '' end as quantity," & _
" remark,'Out' as type,status_id,type as order_type,payment from v_ticket_sum_split) ticket_out " & _
" where datetime is not null and status_id in(101,102,103,104,105) and  datetime between  convert(varchar,getdate(),111) and DATEADD(ss,-1, DATEADD(day,@day, convert(varchar,getdate(),111)))" & _
" and ref_no not in (SELECT	ticket.ref_no FROM        ticket INNER  JOIN ticket_split  ON ticket_split.ref_no = ticket.ref_no " & _
" where ticket_split.payment='cheq' and ticket_split.paid is null and ticket_split.payment_duedate is not null and ticket.type = 'buy') " & _
" UNION " & _
" SELECT		cust_id,'' as asset_id, ticket_split.ref_no,'Cash' as asset_type, " & _
" 			ticket_split.created_by,ticket_split.payment_duedate as datetime,ticket.gold_type_id as purity,  " & _
" 			case when  ticket.type='sell' then ticket_split.amount when  ticket.type='buy' then -(ticket_split.amount) else '' end as quantity, " & _
" 			'cheq' as remark,case when  ticket.type='sell' then 'In' when  ticket.type='buy' then 'Out' else '' end as type, " & _
" 			ticket_split.status_id,ticket.type as order_type,ticket_split.payment " & _
" FROM        ticket INNER  JOIN  ticket_split  ON ticket_split.ref_no = ticket.ref_no " & _
" where ticket_split.payment='cheq' and ticket_split.paid is null and ticket_split.payment_duedate is not null and  ticket_split.payment_duedate between  convert(varchar,getdate(),111) and DATEADD(ss,-1, DATEADD(day,@day, convert(varchar,getdate(),111)))" & _
" " + sql_cheq_inTicket_out + " " & _
" " + sql_deposit + " " & _
" " + sql_delivery_date_null + " " & _
" )Planning  " & _
" LEFT OUTER JOIN  users ON Planning.created_by = users.user_id LEFT OUTER JOIN  customer ON planning.cust_id = customer.cust_id" & _
" Where (asset_type = @asset_type or @asset_type ='') {0} order by datetime asc  ", purity)

            '" --and ref_no not in (SELECT	ticket.ref_no FROM        ticket INNER  JOIN ticket_split  ON ticket_split.ref_no = ticket.ref_no " & _
            '" --where ticket_split.payment='cheq' and ticket_split.paid is null and ticket_split.payment_duedate is not null and ticket.type = 'sell') " & _

            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)

            Dim parameter As New SqlParameter("@asset_type", SqlDbType.VarChar)
            parameter.Value = assetType
            cmd.Parameters.Add(parameter)

            parameter = New SqlParameter("@day", SqlDbType.Int)
            parameter.Value = planningDayPlus
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

    Public Shared Function getStockField(ByVal fieldname As String) As Double
        Try
            Dim dt As New Data.DataTable
            dt = clsDB.getStockNow()
            If dt.Rows.Count > 0 Then
                If dt.Rows(0)(fieldname) IsNot Nothing Then
                    Return Double.Parse(dt.Rows(0)(fieldname).ToString)
                Else
                    Return 0
                End If
            Else
                Return 0
            End If
        Catch ex As Exception
            Return 0
        End Try
    End Function
#End Region

#Region "Report"

    Public Shared Function getReportTransList(ByVal refno As String) As DataTable

        Dim sql As String = "SELECT        customer.cust_id, customer.cust_refno, customer.firstname, customer_trans.datetime, customer_trans.cust_tran_id, customer_trans.cash_type, customer_trans.type, " & _
                            "customer_trans.bill_no, customer_trans.gold_type_id, customer_trans.quantity, customer_trans.price, customer_trans.amount, '' AS gold96, '' AS gold96G,'' AS gold96M, '' AS gold99, 0 as cash " & _
                            ",case when users.firstname is null then '' else users.firstname end AS modify_by " & _
                            "FROM            customer INNER JOIN  customer_trans ON customer.cust_id = customer_trans.cust_id " & _
                            " LEFT OUTER JOIN users ON customer_trans.created_by = users.user_id " & _
                            "where cust_tran_id in( " & refno & " )"

        Dim con As New SqlConnection(strcon)
        Try
            Dim da As New SqlDataAdapter(sql, con)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Dim dc As New dcDBDataContext
                Dim gd = dc.getSumPortfolioByCust_id(dt.Rows(0)("cust_id").ToString).SingleOrDefault
                dt.Rows(0)("gold96") = clsManage.convert2StringNormal(gd.gold96)
                dt.Rows(0)("gold96G") = clsManage.convert2StringNormal(gd.gold96G)
                dt.Rows(0)("gold96M") = clsManage.convert2StringNormal(gd.gold96M)
                dt.Rows(0)("gold99") = clsManage.convert2StringNormal(gd.gold99)
                dt.Rows(0)("cash") = clsManage.convert2StringNormal(gd.cash)
                Return dt
            End If
            Return Nothing
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Shared Function getReportTrans(ByVal refno As String) As DataTable

        Dim sql As String = "SELECT        customer.cust_id, customer.cust_refno, customer.firstname, customer_trans.datetime, customer_trans.cust_tran_id, customer_trans.cash_type, customer_trans.type, " & _
                            "customer_trans.bill_no, customer_trans.gold_type_id, customer_trans.quantity, customer_trans.price, customer_trans.amount, '' AS gold96, '' AS gold96G,'' AS gold96M, '' AS gold99, 0 as cash " & _
                            ",case when users.firstname is null then '' else users.firstname end AS modify_by " & _
                            "FROM            customer INNER JOIN  customer_trans ON customer.cust_id = customer_trans.cust_id " & _
                            " LEFT OUTER JOIN users ON customer_trans.created_by = users.user_id " & _
                            "where cust_tran_id = " & refno & " "

        Dim con As New SqlConnection(strcon)
        Try
            Dim da As New SqlDataAdapter(sql, con)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                Dim dc As New dcDBDataContext
                Dim gd = dc.getSumPortfolioByCust_id(dt.Rows(0)("cust_id").ToString).SingleOrDefault
                dt.Rows(0)("gold96") = clsManage.convert2StringNormal(gd.gold96)
                dt.Rows(0)("gold96G") = clsManage.convert2StringNormal(gd.gold96G)
                dt.Rows(0)("gold96M") = clsManage.convert2StringNormal(gd.gold96M)
                dt.Rows(0)("gold99") = clsManage.convert2StringNormal(gd.gold99)
                dt.Rows(0)("cash") = clsManage.convert2StringNormal(gd.cash)
                Return dt
            End If
            Return Nothing
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getTicketOrder(ByVal refno As String, view As Boolean) As DataTable

        Dim sql As String = "SELECT customer.firstname, customer.lastname, customer.bill_address, customer.tel, customer.cust_id, " & _
                    "ticket.price, gold_type.detail, gold_type.gold_unit, ticket.ref_no, ticket_date, delivery_date,ticket.type" & _
                    ",case when ticket.type ='sell' then -(ticket.quantity) else ticket.quantity end as quantity" & _
                    ",case when ticket.type ='sell' then -(ticket.amount) else ticket.amount end as amount,ticket.gold_type_id " & _
                    ",case when run_no <> '' then run_no else (SELECT RIGHT('00' + cast( day(getdate()) as varchar),2) + RIGHT('00' + cast( month(getdate()) as varchar),2) + RIGHT('00'+ cast( year(getdate()) as varchar),2) + '-' + RIGHT('000'+ CONVERT(VARCHAR,run_no),3) as bill_runno FROM ticket_runno  where type = 'bill') end as 'bill_runno' " & _
                    " FROM  customer INNER JOIN " & _
                    "ticket ON customer.cust_id = ticket.cust_id INNER JOIN " & _
                    "gold_type ON ticket.gold_type_id = gold_type.gold_type_id  " & _
                    "where ticket.ref_no in ('" & refno & "') "

        '"cross join (SELECT  stringWord + RIGHT('00' + cast( month(getdate()) as varchar),2) + RIGHT('00'+ cast( year(getdate()) as varchar),2) + '-' + RIGHT('000'+ CONVERT(VARCHAR,run_no),3) as bill_runno FROM ticket_runno  where type = 'bill' )runno " & _


        Dim sqlUpdate As String = ""
        If view = False Then
            sqlUpdate = "IF (select top 1 run_no from ticket where ref_no in ('" & refno & "')) = '' BEGIN UPDATE tickets set run_no = (SELECT RIGHT('00' + cast( day(getdate()) as varchar),2) + RIGHT('00' + cast( month(getdate()) as varchar),2) + RIGHT('00'+ cast( year(getdate()) as varchar),2) + '-' + RIGHT('000'+ CONVERT(VARCHAR,run_no),3)  FROM ticket_runno  where type = 'bill' ) where ref_no in ('" & refno & "') " & _
                        ";update ticket_runno set run_no= run_no + 1  where type = 'bill' END"
        End If

        Dim con As New SqlConnection(strcon)
        Try
            Dim da As New SqlDataAdapter(sql + sqlUpdate, con)
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

    Public Shared Function getTicketOrderNobillSplit(ByVal sp_id As String, preview As Boolean, report_by As String) As DataTable
        Dim cma As String = ","
        Dim countSplit As String = "1"
        Dim noPlusRunno As String = IIf(preview, "-1 as rows_no_split", "")
        Dim sql_runno_nb As String = "(SELECT  stringWord + RIGHT('00' + cast( day(getdate()) as varchar),2) + RIGHT('00' + cast( month(getdate()) as varchar),2) + RIGHT('00'+ cast( year(getdate()) as varchar),2) + '-' + RIGHT('0000'+ CONVERT(VARCHAR,run_no),4) FROM ticket_runno  where type = 'NB')"

        Dim sql_runno_viewOrUpdate As String = ",ticket.run_no "
        Dim sql_where As String = ""
        If Not preview Then
            'sql_runno_viewOrUpdate = ",run_no_ticket +'/'+RIGHT('00'+ CONVERT(VARCHAR,rows_no_split) ,2) AS run_no "
            sql_runno_viewOrUpdate = ",case when (select top 1 run_no from ticket_split where ticket_sp_id in (" & sp_id & ")) is null  then run_no_ticket +'/'+RIGHT('00'+ CONVERT(VARCHAR,rows_no_split) ,2) else ticket.run_no end as run_no "
            sql_where = "where ticket_id in (" & sp_id & ")"
        Else
            sql_where = "where run_no = (select run_no from ticket_split where ticket_sp_id = " & sp_id & ")"
        End If

        Dim sqlreceiptDate As String = ""
        If preview = False Then
            sqlreceiptDate = "getdate() as receipt_date" 'update ที่หลังเลยต้องใส่วันที่ก่อน
        Else
            sqlreceiptDate = " receipt_split_date as receipt_date"
        End If

        Dim sql As String = "SELECT " + sqlreceiptDate & _
 ",case when ticket.type='sell' then cast(ticket.price as varchar) else '' end as sell " & _
 ",case when ticket.type='buy' then cast(ticket.price as varchar) else '' end as buy " & _
 ",case when ticket.gold_type_id ='96' then ticket.quantity else 0 end as G96 " & _
 ",case when substring(cast(ticket.gold_type_id as varchar),1,2)='99' then ticket.quantity else 0 end as G99 " & _
 ",case when ticket.gold_type_id ='96G' then ticket.quantity else 0 end as G96G  " & _
 ",ticket.Amount,ticket.ref_no,ticket.cust_id, customer.firstname,split_count,v_ticket_sum_split.quantity as quantity_no_split,ticket.gold_type_id as purity,'" + report_by + "' as report_by   " & _
 "" & sql_runno_viewOrUpdate & "" & _
 "FROM  ( " & _
 "	select receipt_split_date,[type],price,gold_type_id,ref_no,cust_id,run_no,row,rows_no_split" + noPlusRunno + ", " & _
 "	run_no_ticket = case when run_no_ticket <> '' then run_no_ticket else " & sql_runno_nb & " end , " & _
 " 	case when type ='sell' then -(quantity) else quantity end as quantity,  " & _
 "	case when type ='sell' then -(amount) else amount end as amount ,split_count = " + countSplit + " " & _
 "	from v_ticket_split " & _
 "  " & sql_where & " " & _
 ")ticket INNER JOIN  customer ON ticket.cust_id = customer.cust_id INNER JOIN " & _
 "v_ticket_sum_split  ON v_ticket_sum_split.ref_no = ticket.ref_no "

        Dim sqlUpdate As String = ""
        Dim sp_id_one As String = sp_id.Replace("'", "").Split(cma)(0) 'ทุก row มี ref_no เหมือนกัน
        Dim sqlUpdateSplit As String = ""
        Dim sqlRefNo As String = String.Format("(select ref_no from ticket_split where ticket_sp_id = {0})", sp_id_one)
        Dim rows_no_split As String = String.Format("(select rows_no_split from tickets where ref_no={0})", sqlRefNo)
        Dim sql_runno_nb_split As String = String.Format("(SELECT stringWord + RIGHT('00' + cast( day(getdate()) as varchar),2) + RIGHT('00' + cast( month(getdate()) as varchar),2) + RIGHT('00'+ cast( year(getdate()) as varchar),2) + '-' + RIGHT('0000'+ CONVERT(VARCHAR,run_no),4)  +'/'+RIGHT('00'+ CONVERT(VARCHAR,({0})) ,2)  FROM ticket_runno  where type = 'NB')", rows_no_split)

        'Dim sql_case_update As String = String.Format(" case when (select run_no  from tickets where ref_no ={2}) is null then {1} else  (select run_no from tickets where ref_no={2}) end ", "", sql_runno_nb, sqlRefNo)

        Dim sql_case_update As String = String.Format("case when (select run_no  from tickets where ref_no ={2}) = '' then {1} else  (select run_no + '/'+RIGHT('00'+ CONVERT(VARCHAR,({0})) ,2) from tickets where ref_no={2} ) end ", rows_no_split, sql_runno_nb, sqlRefNo)
        Dim sql_case_update_split As String = String.Format("case when (select run_no  from tickets where ref_no ={2}) = '' then {1} else  (select run_no + '/'+RIGHT('00'+ CONVERT(VARCHAR,({0})) ,2)  from tickets where ref_no={2} ) end ", rows_no_split, sql_runno_nb_split, sqlRefNo)
        If preview = False Then
            sqlUpdateSplit = String.Format(";IF (select top 1 run_no from ticket_split where ticket_sp_id in ({0})) is null BEGIN ", sp_id)
            For Each sp In sp_id.Replace("'", "").Split(cma)

                sqlUpdateSplit += String.Format(";update ticket_split set receipt_split_date = getdate(), run_no = {1} " & _
                                " where ticket_sp_id = {0}", sp, sql_case_update_split)

            Next
            sqlUpdate = sqlUpdateSplit
            sqlUpdate += String.Format(";update tickets set run_no = {1} WHERE ref_no = {0} AND EXISTS (SELECT 1 FROM tickets WHERE ref_no = {0} and rows_no_split = 1)", sqlRefNo, sql_runno_nb)
            'update run_no in ticket_runno ในครั้งแรก
            sqlUpdate += String.Format(";update ticket_runno set run_no= run_no + 1 WHERE type = 'NB' AND EXISTS (SELECT 1 FROM tickets WHERE ref_no = {0}  and rows_no_split = 1)", sqlRefNo)
            'update จำนวน rows เพื่อให้เลข run ต่อไป
            sqlUpdate += String.Format(";update tickets set rows_no_split = rows_no_split + {1} where ref_no = {0} ", sqlRefNo, countSplit)
            'update run_no in ticket ในครั้งแรก(run_no = '' คือ ครั้งแรก)
            sqlUpdate += " END "
        Else

        End If

        Dim con As New SqlConnection(strcon)
        Try
            Dim da As New SqlDataAdapter(sql + sqlUpdate, con)
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


    Public Shared Function getTicketOrderNobill(ByVal refno As String, preview As Boolean, report_by As String, Optional view As Boolean = False) As DataTable
        'parameter optional view for ค้นหาจาก runno อาจมีหลาย ticket
        'Change Requirment from jack 07/0/12014 >> report by to modifer_by
        If Not refno.Contains("','") Then
            refno = refno.Replace(",", "','")
        End If

        Dim sqlreceiptDate As String = ""
        If preview = False Then
            sqlreceiptDate = "getdate() as receipt_date" 'update ที่หลังเลยต้องใส่วันที่ก่อน
        Else
            sqlreceiptDate = " receipt_date"
        End If

        Dim sqlCondition As String = ""
        If view = True Then
            sqlCondition = "where ticket.run_no in (select run_no from tickets where ref_no in ('" & refno & "'))"
        Else
            sqlCondition = "where ticket.ref_no in ('" & refno & "')"
        End If
        Dim sql As String = "SELECT " + sqlreceiptDate + "" & _
 ",case when type='sell' then cast(ticket.price as varchar) else '' end as sell " & _
 ",case when type='buy' then cast(ticket.price as varchar) else '' end as buy " & _
 ",case when ticket.gold_type_id ='96' then ticket.quantity else 0 end as G96 " & _
 ",case when substring(cast(ticket.gold_type_id as varchar),1,2)='99' then ticket.quantity else 0 end as G99 " & _
 ",case when ticket.gold_type_id ='96G' then ticket.quantity else 0 end as G96G  " & _
 ",ticket.Amount,ticket.ref_no,ticket.cust_id, customer.firstname,'' as quantity_no_split,ticket.gold_type_id as purity,u.user_name as report_by  " & _
 ",case when run_no <> '' then run_no else (SELECT  stringWord + RIGHT('00' + cast( day(getdate()) as varchar),2) +RIGHT('00' + cast( month(getdate()) as varchar),2) + RIGHT('00'+ cast( year(getdate()) as varchar),2) + '-' + RIGHT('0000'+ CONVERT(VARCHAR,run_no),4) as bill_runno FROM ticket_runno  where type = 'NB') end as 'run_no' " & _
 "FROM  (" & _
 "	select receipt_date,ticket.[type],ticket.price,gold_type_id,ticket.ref_no,ticket.cust_id,run_no," & _
 "	case when ticket.type ='sell' then -(ticket.quantity) else ticket.quantity end as quantity, " & _
 "	case when ticket.type ='sell' then -(ticket.amount) else ticket.amount end as amount,modifier_by " & _
 "	from ticket " & _
 ")ticket INNER JOIN  customer ON ticket.cust_id = customer.cust_id left join users u on u.user_id = ticket.modifier_by " & _
 "" & sqlCondition & ""

        Dim sqlUpdate As String = ""
        If preview = False Then
            sqlUpdate = ";IF (select top 1 run_no from tickets where ref_no in ('" & refno & "')) = '' OR (select top 1 run_no from tickets where ref_no in ('" & refno & "')) is null Begin update tickets set receipt_date = getdate(), run_no = (SELECT  stringWord + RIGHT('00' + cast( day(getdate()) as varchar),2) + RIGHT('00' + cast( month(getdate()) as varchar),2) + RIGHT('00'+ cast( year(getdate()) as varchar),2) + '-' + RIGHT('0000'+ CONVERT(VARCHAR,run_no),4)  FROM ticket_runno  where type = 'NB' ) where ref_no in ('" & refno & "') " & _
                        " update ticket_runno set run_no= run_no + 1  where type = 'NB' END"
        End If

        Dim con As New SqlConnection(strcon)
        Try
            Dim da As New SqlDataAdapter(sql + sqlUpdate, con)
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

    Public Shared Function getTradeBuySellReport(ByVal refno As String, bill As String, editType As String) As DataTable

        ''ดึงค่าออกมาก่อน แล้ว update running no ที่หลัง

        Dim sql As String = "select customer.firstname, ticket.ref_no, ticket.cust_id , ticket.gold_type_id, ticket.delivery_date, ticket.type, ticket.quantity, ticket.price, ticket.amount, ticket.ticket_date, " & _
                            " ticket.payment, case when users.firstname is null then '' else users.firstname end AS modify_by " & _
                            " FROM   customer INNER JOIN   ticket ON customer.cust_id = ticket.cust_id LEFT OUTER JOIN " & _
                            " users ON ticket.modifier_by = users.user_id " & _
                            " where ticket.ref_no in ('" & refno & "')"


        Dim sqlUpdate As String = ""
        If editType = "n" Then
            sql += String.Format(";update tickets set billing = '{0}' where ref_no='{1}'", bill, refno)
        End If

        'Dim sqlCheck As String =
        '" if (select billing from tickets where ref_no in ('" & refno & "')) = '" + bill + "' " & _
        '"	begin " & _
        '"" + sql + "" & _
        '"	end " & _
        '" else " & _
        '"	begin " & _
        '"		select getdate()" & _
        '"	end"

        Dim con As New SqlConnection(strcon)
        Try
            Dim da As New SqlDataAdapter(sql, con)
            Dim dt As New DataTable
            da.Fill(dt)
            If dt.Rows.Count > 0 Then
                'If editType = "n" Then
                '    Dim no_run As Integer = dt.Rows(0)("no_run").ToString
                '    Dim order_type As String = dt.Rows(0)("type").ToString
                '    Dim sql_ins As String = String.Format("update trade_runno set book_run = book_run+1 ,no_run=no_run+1 where type = '{0}'", order_type)
                '    If bill = "y" Then
                '        If no_run Mod 50 = 0 Then
                '            Dim str_book_run As String = dt.Rows(0)("book_run").ToString
                '            If str_book_run.Substring(str_book_run.Length - 3, 3) = "999" Then
                '                sql_ins = String.Format("update trade_runno set book_run_bill = book_run_bill + 1 ,no_run_bill =  1 where type = '{0}'", order_type)
                '            Else
                '                sql_ins = String.Format("update trade_runno set book_run_bill = book_run_bill + 1 ,no_run_bill = no_run_bill + 1 where type = '{0}'", order_type)
                '            End If
                '        Else
                '            sql_ins = String.Format("update trade_runno set no_run_bill = no_run_bill + 1 where type = '{0}'", order_type)
                '        End If
                '    Else
                '        If no_run = 99999 Then
                '            Dim str_book_run As String = dt.Rows(0)("book_run").ToString
                '            If str_book_run = "999" Then
                '                sql_ins = String.Format("update trade_runno set book_run_nobill = 1 ,no_run_nobill = 1 where type = '{0}'", order_type)
                '            Else
                '                sql_ins = String.Format("update trade_runno set book_run_nobill = book_run_nobill + 1 ,no_run_nobill = 1 where type = '{0}'", order_type)
                '            End If
                '        Else
                '            sql_ins = String.Format("update trade_runno set no_run_nobill = no_run_nobill + 1 where type = '{0}'", order_type)
                '        End If
                '    End If
                '    'update ticket
                '    'ถ้าเป็นแบบ มีบิล dt.Rows(0)("book_run") จะมีมากกว่า 3 ตัว ถ้าไม่มีบิล มี 1-3 ตัว
                '    Dim book_no As String = dt.Rows(0)("book_word").ToString + clsManage.formatTextZoro(dt.Rows(0)("book_run"), IIf(dt.Rows(0)("book_run").ToString.Length > 3, 0, 3))
                '    Dim run_no As String = clsManage.formatTextZoro(dt.Rows(0)("no_run"), 5)
                '    Dim sql_update_ticket As String = String.Format(";update tickets set billing = '{0}',book_no='{1}',run_no='{2}' where ref_no='{3}'", bill, book_no, run_no, refno)

                '    Dim cmd As New SqlCommand(sql_ins + sql_update_ticket, con)
                '    con.Open()
                '    cmd.ExecuteNonQuery()
                '    con.Close()
                'End If
                Return dt
            End If
            Return Nothing
        Catch ex As Exception
            Throw ex
        End Try
    End Function

#Region "Payment"
    Public Shared Function getPaymentReport(payment_id As String, report_by As String) As DataTable


        Dim sql As String = "SELECT case when type='sell' then cast(ticket.price as varchar) else '' end as sell " & _
 ",case when type='buy' then cast(ticket.price as varchar) else '' end as buy " & _
 ",case when ticket.gold_type_id ='96' then ticket.quantity else 0 end as G96 " & _
 ",case when substring(cast(ticket.gold_type_id as varchar),1,2)='99' then ticket.quantity else 0 end as G99 " & _
 ",(select top 1  paid_cash from payment where payment_id = " + payment_id + " ) as paid" & _
 ",ticket.Amount,ticket.ref_no,ticket.cust_id, customer.firstname,ticket.gold_type_id as purity,u.user_name as report_by,payment_id  " & _
 "FROM  (" & _
 "	select t.[type],t.price,gold_type_id,t.ref_no,t.cust_id,payment_id," & _
 "	case when t.type ='sell' then -(t.quantity) else t.quantity end as quantity, " & _
 "	case when t.type ='sell' then -(t.amount) else t.amount end as amount,modifier_by " & _
 "	from v_ticket_sum_split t " & _
 ")ticket INNER JOIN  customer ON ticket.cust_id = customer.cust_id left join users u on u.user_id = ticket.modifier_by " & _
 "Where payment_id = " + payment_id

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


    'Public Shared Function getTradeBuySellReport(ByVal refno As String, bill As String, editType As String) As DataTable

    '    ''ดึงค่าออกมาก่อน แล้ว update running no ที่หลัง

    '    Dim sql_bill As String = ""
    '    If editType = "v" Then
    '        sql_bill = ",substring(book_no,1,1) as book_word,SUBSTRING(book_no,2, len(book_no)-1) as book_run,run_no as no_run"
    '    Else
    '        If bill = "y" Then
    '            sql_bill = ",book_word_bill as book_word,book_run_bill as book_run,no_run_bill as no_run"
    '        Else
    '            sql_bill = ",book_word_nobill as book_word,book_run_nobill as book_run,no_run_nobill as no_run"
    '        End If
    '    End If
    '    ', substring(book_no,1,1) as book_word,SUBSTRING(book_no,2, len(book_no)-1) as book_run,run_no as no_run
    '    Dim sql As String = "select customer.firstname, ticket.ref_no, ticket.cust_id , ticket.gold_type_id, ticket.delivery_date, ticket.type, ticket.quantity, ticket.price, ticket.amount, ticket.ticket_date, " & _
    '                        " ticket.payment " + sql_bill + " " & _
    '                        " FROM   customer INNER JOIN   ticket ON customer.cust_id = ticket.cust_id CROSS JOIN trade_runno " & _
    '                        " where ticket.ref_no in ('" & refno & "')"

    '    Dim con As New SqlConnection(strcon)
    '    Try
    '        Dim da As New SqlDataAdapter(sql, con)
    '        Dim dt As New DataTable
    '        da.Fill(dt)
    '        If dt.Rows.Count > 0 Then
    '            If editType = "n" Then
    '                Dim no_run As Integer = dt.Rows(0)("no_run").ToString
    '                Dim sql_ins As String = "update trade_runno set book_run = book_run+1 ,no_run=no_run+1"
    '                If bill = "y" Then
    '                    If no_run Mod 50 = 0 Then
    '                        Dim str_book_run As String = dt.Rows(0)("book_run").ToString
    '                        If str_book_run.Substring(str_book_run.Length - 3, 3) = "999" Then
    '                            sql_ins = "update trade_runno set book_run_bill = book_run_bill + 1 ,no_run_bill =  1"
    '                        Else
    '                            sql_ins = "update trade_runno set book_run_bill = book_run_bill + 1 ,no_run_bill = no_run_bill + 1"
    '                        End If
    '                    Else
    '                        sql_ins = "update trade_runno set no_run_bill = no_run_bill + 1"
    '                    End If
    '                Else
    '                    If no_run = 99999 Then
    '                        Dim str_book_run As String = dt.Rows(0)("book_run").ToString
    '                        If str_book_run = "999" Then
    '                            sql_ins = "update trade_runno set book_run_nobill = 1 ,no_run_nobill = 1"
    '                        Else
    '                            sql_ins = "update trade_runno set book_run_nobill = book_run_nobill + 1 ,no_run_nobill = 1"
    '                        End If
    '                    Else
    '                        sql_ins = "update trade_runno set no_run_nobill = no_run_nobill + 1"
    '                    End If
    '                End If
    '                'update ticket
    '                'ถ้าเป็นแบบ มีบิล dt.Rows(0)("book_run") จะมีมากกว่า 3 ตัว ถ้าไม่มีบิล มี 1-3 ตัว
    '                Dim book_no As String = dt.Rows(0)("book_word").ToString + clsManage.formatTextZoro(dt.Rows(0)("book_run"), IIf(dt.Rows(0)("book_run").ToString.Length > 3, 0, 3))
    '                Dim run_no As String = clsManage.formatTextZoro(dt.Rows(0)("no_run"), 5)
    '                Dim sql_update_ticket As String = String.Format(";update tickets set billing = '{0}',book_no='{1}',run_no='{2}' where ref_no='{3}'", bill, book_no, run_no, refno)

    '                Dim cmd As New SqlCommand(sql_ins + sql_update_ticket, con)
    '                con.Open()
    '                cmd.ExecuteNonQuery()
    '                con.Close()
    '            End If
    '            Return dt
    '        End If
    '        Return Nothing
    '    Catch ex As Exception
    '        Throw ex
    '    End Try
    'End Function

#End Region

#Region "User"


    Public Structure roles

        Public Shared menu As String = "menu_name"
        Public Shared view As String = "role_view"
        Public Shared add As String = "role_add"
        Public Shared update As String = "role_update"
        Public Shared delete As String = "role_delete"
        Public Shared export As String = "role_export"
        Public Shared print As String = "role_print"

    End Structure

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

    Public Shared Function getMenuRole(userId As String) As DataTable
        If userId Is Nothing Or userId = "" Then Return Nothing
        Dim ds As New DataSet
        Try

            Dim sql As String = "SELECT menu_id,menu_name,detail, " &
                                "0 AS role_view, 0 AS role_add, 0 AS role_update,0 AS role_delete, 0 AS role_export, 0 AS role_print " & _
                                "from users_menu where active = 1 order by cast( menu_group as varchar) asc" & _
                                ";SELECT * FROM users_role where user_id =" & userId

            Using con As New SqlConnection(strcon)
                Using cmd As New SqlCommand(sql, con)
                    Using da As New SqlDataAdapter(cmd)
                        da.Fill(ds)
                        If ds.Tables(1).Rows.Count > 0 Then
                            'loop in role for update
                            For Each drRole As DataRow In ds.Tables(1).Rows
                                For Each drMenu As DataRow In ds.Tables(0).Rows
                                    If drRole("menu_id") = drMenu("menu_id") Then
                                        drMenu("role_view") = drRole("role_view")
                                        drMenu("role_add") = drRole("role_add")
                                        drMenu("role_update") = drRole("role_update")
                                        drMenu("role_delete") = drRole("role_delete")
                                        drMenu("role_export") = drRole("role_export")
                                        drMenu("role_print") = drRole("role_print")
                                    End If
                                Next
                            Next

                        End If
                        Return ds.Tables(0)
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            ds.Dispose()
        End Try

    End Function

#End Region

#Region "Actual"

    Public Shared Function insert_actual(ByVal asset_id As String, ByVal ref_no As String, ByVal created_by As String, ByVal purity As String, ByVal quantity As Double, ByVal amount As Double, ByVal status_id As String, ByVal remark As String, ByVal type As String, ByVal order_type As String, ByVal price As Double, ByVal G96 As Double, ByVal G99N As Double, ByVal G99L As Double, ByVal cust_id As String) As String
        Try
            Dim asset_type As String = ""
            If order_type = "sell" Then
                If type = "In" Then
                    asset_type = "Cash"
                Else 'Out
                    asset_type = "Gold"
                End If
            ElseIf order_type = "buy" Then 'buy
                If type = "In" Then
                    asset_type = "Gold"
                Else
                    asset_type = "Cash"
                End If
            ElseIf order_type = "In/Out" Then
                If purity = "" Then
                    asset_type = "Cash"
                Else
                    asset_type = "Gold"
                End If
            ElseIf order_type = "Cancel_D/W" Then
                If purity = "" Then
                    asset_type = "Cash"
                Else
                    asset_type = "Gold"
                End If
            ElseIf order_type = "D/W" Then
                If purity = "" Then
                    asset_type = "Cash"
                Else
                    asset_type = "Gold"
                End If
            ElseIf order_type = clsManage.splitMode.Split.ToString Then
                asset_type = "Gold"
                If type = "sell" Then
                    type = "Out"
                Else
                    type = "In"
                End If
            ElseIf order_type = "Cancel_Complete" Then
                asset_type = "Gold"
                If type = "sell" Then
                    type = "Out"
                Else
                    type = "In"
                End If
            ElseIf order_type = "cancel_104" Then
                asset_type = "Gold"
            ElseIf order_type = "cancel_105" Then
                asset_type = "Cash"
            Else 'ฝาก ถอน
                asset_type = "Gold"
            End If

            Dim sql As String = ""
            If (status_id = "999" Or status_id = "997") Or order_type = clsManage.splitMode.Split.ToString Or order_type = "Cancel_Complete" Or order_type = "cancel_104" Or order_type = "cancel_105" Then
                Dim dt_stock As New Data.DataTable
                dt_stock = getStockSumDeposit()
                price = clsManage.convert2zero(dt_stock.Rows(0)("priceDep"))
                G96 = clsManage.convert2zero(dt_stock.Rows(0)("G96Dep"))
                G99N = clsManage.convert2zero(dt_stock.Rows(0)("G99NDep"))
                G99L = clsManage.convert2zero(dt_stock.Rows(0)("G99LDep"))

                sql = String.Format(";INSERT INTO [actual] ([asset_id], [ref_no], [asset_type], [created_by], [datetime], [purity], [quantity], [amount], [status_id], [remark], [type], [order_type], [price_base], [G96_base], [G99N_base], [G99L_base], [cust_id], [before_status_id]) " & _
                                                  "VALUES ('{0}', '{1}', '{2}', '{3}', getdate(), '{4}', {5}, {6}, '{7}', '{8}', '{9}', '{10}', {11}, {12}, {13}, {14}, '{15}','{16}')" & _
                                                  "", asset_id, ref_no, asset_type, created_by, purity, quantity, amount, status_id, remark, type, order_type, price, G96, G99N, G99L, cust_id)

                If order_type = clsManage.splitMode.Split.ToString Or order_type = "Cancel_Complete" Then
                    asset_type = "Cash"
                    If type = "In" Then
                        type = "Out"
                    Else
                        type = "In"
                    End If
                    sql += String.Format(";INSERT INTO [actual] ([asset_id], [ref_no], [asset_type], [created_by], [datetime], [purity], [quantity], [amount], [status_id], [remark], [type], [order_type], [price_base], [G96_base], [G99N_base], [G99L_base], [cust_id], [before_status_id]) " & _
                                                  "VALUES ('{0}', '{1}', '{2}', '{3}', getdate(), '{4}', {5}, {6}, '{7}', '{8}', '{9}', '{10}', {11}, {12}, {13}, {14}, '{15}','{16}')" & _
                                                  "", asset_id, ref_no, asset_type, created_by, purity, quantity, amount, status_id, remark, type, order_type, price, G96, G99N, G99L, cust_id)

                End If

                Dim con As New SqlConnection(strcon)
                Dim cmd As New SqlCommand(sql, con)
                con.Open()
                cmd.ExecuteNonQuery()
                con.Close()
                Return ""
            Else
                sql = String.Format(";INSERT INTO [actual] ([asset_id], [ref_no], [asset_type], [created_by], [datetime], [purity], [quantity], [amount], [status_id], [remark], [type], [order_type], [price_base], [G96_base], [G99N_base], [G99L_base], [cust_id], [before_status_id]) " & _
                                                              "VALUES ('{0}', '{1}', '{2}', '{3}', getdate(), '{4}', {5}, {6}, '{7}', '{8}', '{9}', '{10}', {11}, {12}, {13}, {14}, '{15}','{16}')" & _
                                                              "", asset_id, ref_no, asset_type, created_by, purity, quantity, amount, status_id, remark, type, order_type, price, G96, G99N, G99L, cust_id)
                Return sql
            End If

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function insert_actual2(ByVal asset_id As String, ByVal ref_no As String, ByVal created_by As String, ByVal purity As String, ByVal quantity As Double, ByVal amount As Double, ByVal status_id As String, ByVal status_name As String, ByVal type As String, ByVal order_type As String, ByVal cust_id As String, ByVal before_status_id As String, ByVal note As String, ByVal update_cash As String, ByVal update_gold As String, ByVal cash As String, ByVal payment As String) As String
        Try

            Dim asset_type = "Gold"
            Dim sql As String = ""
            Dim dt_stock As New Data.DataTable
            dt_stock = getStockSumDeposit()
            Dim price As Double = clsManage.convert2zero(dt_stock.Rows(0)("priceDep"))
            Dim G96 As Double = clsManage.convert2zero(dt_stock.Rows(0)("G96Dep"))
            Dim G96G As Double = clsManage.convert2zero(dt_stock.Rows(0)("G96GDep"))
            Dim G99 As Double = clsManage.convert2zero(dt_stock.Rows(0)("G99Dep"))
            Dim cash_stock As Double = clsManage.convert2zero(dt_stock.Rows(0)("cashDep"))
            Dim trans_stock As Double = clsManage.convert2zero(dt_stock.Rows(0)("transDep"))
            Dim cheq_stock As Double = clsManage.convert2zero(dt_stock.Rows(0)("cheqDep"))

            If cash = "n" Then
                If update_gold = "y" Then
                    sql = String.Format(";INSERT INTO [actual] ([asset_id], [ref_no], [asset_type], [created_by], [datetime], [purity], [quantity], [amount], [status_id], [status_name], [type], [order_type], [price_base], [G96_base], [G96G_base], [G99_base], [cust_id], [before_status_id], [note], [payment], [cash], [trans], [cheq]) " & _
                                                      "VALUES ('{0}', '{1}', '{2}', '{3}', getdate(), '{4}', {5}, {6}, '{7}', @status_name, '{9}', '{10}', {11}, {12}, {13}, {14}, '{15}','{16}','{17}','{18}',{19},{20},{21})" & _
                                                      "", asset_id, ref_no, asset_type, created_by, purity, quantity, amount, status_id, status_name, type, order_type, price, G96, G96G, G99, cust_id, before_status_id, note, payment, cash_stock, trans_stock, cheq_stock)
                End If
                If update_cash = "y" Then
                    asset_type = "Cash"
                    If type = "In" Then
                        type = "Out"
                    ElseIf type = "ตัดทองฝาก" Then
                        type = "Out"
                    Else
                        type = "In"
                    End If
                    sql += String.Format(";INSERT INTO [actual] ([asset_id], [ref_no], [asset_type], [created_by], [datetime], [purity], [quantity], [amount], [status_id], [status_name], [type], [order_type], [price_base], [G96_base], [G96G_base], [G99_base], [cust_id], [before_status_id], [note], [payment], [cash], [trans], [cheq]) " & _
                                                  "VALUES ('{0}', '{1}', '{2}', '{3}', getdate(), '{4}', {5}, {6}, '{7}', @status_name, '{9}', '{10}', {11}, {12}, {13}, {14}, '{15}','{16}','{17}','{18}',{19},{20},{21})" & _
                                                  "", asset_id, ref_no, asset_type, created_by, purity, quantity, amount, status_id, status_name, type, order_type, price, G96, G96G, G99, cust_id, before_status_id, note, payment, cash_stock, trans_stock, cheq_stock)

                End If
            Else
                asset_type = "Cash"

                sql += String.Format(";INSERT INTO [actual] ([asset_id], [ref_no], [asset_type], [created_by], [datetime], [purity], [quantity], [amount], [status_id], [status_name], [type], [order_type], [price_base], [G96_base], [G96G_base], [G99_base], [cust_id], [before_status_id], [note], [payment], [cash], [trans], [cheq]) " & _
                                              "VALUES ('{0}', '{1}', '{2}', '{3}', getdate(), '{4}', {5}, {6}, '{7}', @status_name, '{9}', '{10}', {11}, {12}, {13}, {14}, '{15}','{16}','{17}','{18}',{19},{20},{21})" & _
                                              "", asset_id, ref_no, asset_type, created_by, purity, quantity, amount, status_id, status_name, type, order_type, price, G96, G96G, G99, cust_id, before_status_id, note, payment, cash_stock, trans_stock, cheq_stock)
            End If

            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)
            con.Open()

            Dim parameter As New SqlParameter("@status_name", SqlDbType.VarChar)
            parameter.Value = status_name
            cmd.Parameters.Add(parameter)

            cmd.ExecuteNonQuery()
            con.Close()
            Return ""

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function insert_actual2(ByVal act As clsActual) As String
        Try
            Dim asset_type = "Gold"
            Dim sql As String = ""
            Dim dt_stock As New Data.DataTable
            dt_stock = getStockSumDeposit()
            Dim price As Double = clsManage.convert2zero(dt_stock.Rows(0)("priceDep"))
            Dim G96 As Double = clsManage.convert2zero(dt_stock.Rows(0)("G96Dep"))
            Dim G99N As Double = clsManage.convert2zero(dt_stock.Rows(0)("G99NDep"))
            Dim G99L As Double = clsManage.convert2zero(dt_stock.Rows(0)("G99LDep"))
            Dim cash_stock As Double = clsManage.convert2zero(dt_stock.Rows(0)("cashDep"))
            Dim trans_stock As Double = clsManage.convert2zero(dt_stock.Rows(0)("transDep"))
            Dim cheq_stock As Double = clsManage.convert2zero(dt_stock.Rows(0)("cheqDep"))

            If act.cash = "n" Then
                If act.update_gold = "y" Then
                    sql = String.Format(";INSERT INTO [actual] ([asset_id], [ref_no], [asset_type], [created_by], [datetime], [purity], [quantity], [amount], [status_id], [status_name], [type], [order_type], [price_base], [G96_base], [G99N_base], [G99L_base], [cust_id], [before_status_id], [note], [payment], [cash], [trans], [cheq]) " & _
                                                      "VALUES ('{0}', '{1}', '{2}', '{3}', getdate(), '{4}', {5}, {6}, '{7}', @status_name, '{9}', '{10}', {11}, {12}, {13}, {14}, '{15}','{16}','{17}','{18}',{19},{20},{21})" & _
                                                      "", act.asset_id, act.ref_no, asset_type, act.created_by, act.purity, act.quantity, act.amount, act.status_id, act.status_name, act.type, act.order_type, price, G96, G99N, G99L, act.cust_id, act.before_status_id, act.note, act.payment, cash_stock, trans_stock, cheq_stock)
                End If
                If act.update_cash = "y" Then
                    asset_type = "Cash"
                    If act.type = "In" Then
                        act.type = "Out"
                    ElseIf act.type = "ตัดทองฝาก" Then
                        act.type = "Out"
                    Else
                        act.type = "In"
                    End If
                    sql += String.Format(";INSERT INTO [actual] ([asset_id], [ref_no], [asset_type], [created_by], [datetime], [purity], [quantity], [amount], [status_id], [status_name], [type], [order_type], [price_base], [G96_base], [G99N_base], [G99L_base], [cust_id], [before_status_id], [note], [payment], [cash], [trans], [cheq]) " & _
                                                  "VALUES ('{0}', '{1}', '{2}', '{3}', getdate(), '{4}', {5}, {6}, '{7}', @status_name, '{9}', '{10}', {11}, {12}, {13}, {14}, '{15}','{16}','{17}','{18}',{19},{20},{21})" & _
                                                  "", act.asset_id, act.ref_no, asset_type, act.created_by, act.purity, act.quantity, act.amount, act.status_id, act.status_name, act.type, act.order_type, price, G96, G99N, G99L, act.cust_id, act.before_status_id, act.note, act.payment, cash_stock, trans_stock, cheq_stock)

                End If
            Else
                asset_type = "Cash"

                sql += String.Format(";INSERT INTO [actual] ([asset_id], [ref_no], [asset_type], [created_by], [datetime], [purity], [quantity], [amount], [status_id], [status_name], [type], [order_type], [price_base], [G96_base], [G99N_base], [G99L_base], [cust_id], [before_status_id], [note], [payment], [cash], [trans], [cheq]) " & _
                                              "VALUES ('{0}', '{1}', '{2}', '{3}', getdate(), '{4}', {5}, {6}, '{7}', @status_name, '{9}', '{10}', {11}, {12}, {13}, {14}, '{15}','{16}','{17}','{18}',{19},{20},{21})" & _
                                              "", act.asset_id, act.ref_no, asset_type, act.created_by, act.purity, act.quantity, act.amount, act.status_id, act.status_name, act.type, act.order_type, price, G96, G99N, G99L, act.cust_id, act.before_status_id, act.note, act.payment, cash_stock, trans_stock, cheq_stock)
            End If

            Dim con As New SqlConnection(strcon)
            Dim cmd As New SqlCommand(sql, con)
            con.Open()

            Dim parameter As New SqlParameter("@status_name", SqlDbType.VarChar)
            parameter.Value = act.status_name
            cmd.Parameters.Add(parameter)

            cmd.ExecuteNonQuery()
            con.Close()
            Return ""

        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region
End Class
