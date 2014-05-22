Imports Microsoft.VisualBasic
Imports System.Data.SqlClient
Imports System.Data

Public Class clsFng
    Private Shared strcon As String = ConfigurationManager.ConnectionStrings("FNGConnectionString").ConnectionString

    Public Shared msgPriceSelect As String = "--โปรดเลือก--"
    Public Shared p96 As String = "96"
    Public Shared p96g As String = "96G"
    Public Shared p96m As String = "96M"
    Public Shared p99 As String = "99"
    Public Shared buy As String = "buy"
    Public Shared sell As String = "sell"
    Public Shared dps As String = "Deposit"
    Public Shared wd As String = "Withdraw"
    Public Const strOnline As String = "L"
    Public Const strCall As String = "P"


    Public Shared Function getTicketOverClearing(Overclearing As String) As DataTable
        Try
            Dim mark As String = "="
            If Overclearing = "0" Then mark = ">"
            Dim sql As String = String.Format("select *,DATEDIFF(day,dateadd(day,t.clearingday,t.ticket_date),dateadd(day,0,getdate())) as over_clearingday from tickets t inner join customer c on t.cust_id = c.cust_id  where status_id = 101 and active = 'y' and DATEDIFF(day,dateadd(day,t.clearingday,t.ticket_date),dateadd(day,0,getdate())) {1} {0}", Overclearing, mark)
            Using con As New SqlConnection(strcon)
                Using da As New SqlDataAdapter(sql, con)
                    Using dt As New DataTable
                        da.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Sub getQuantity96ToDDL(ddl As DropDownList, type As String, Optional isSale As Boolean = False)

        Try
            Dim dc As New dcDBDataContext
            Dim stock = dc.getStockOnline.SingleOrDefault
            ddl.Items.Clear()

            Select Case type
                Case "96"
                    For i As Integer = 10 To 100 Step 10 : ddl.Items.Add(New ListItem(i.ToString, i.ToString)) : Next
                    Dim quan As New Dictionary(Of Integer, Integer)
                    quan.Add(120, 120)
                    quan.Add(150, 150)
                    quan.Add(200, 200)
                    quan.Add(250, 250)
                    quan.Add(350, 350)
                    quan.Add(400, 400)
                    quan.Add(500, 500)
                    quan.Add(700, 700)
                    quan.Add(1000, 1000)
                    For Each q As Integer In quan.Values
                        If q <= stock.max_baht Then
                            ddl.Items.Add(New ListItem(q.ToString, q.ToString))
                        End If
                    Next
                    ddl.Items.Insert(0, New ListItem(msgPriceSelect, ""))
                Case "99"

                    If isSale = True Then
                        For i As Integer = 1 To 10
                            ddl.Items.Add(New ListItem(i.ToString, i.ToString))
                        Next
                        ddl.Items.Add(New ListItem("15", "15"))
                        ddl.Items.Add(New ListItem("20", "20"))
                        ddl.Items.Add(New ListItem("30", "30"))
                    Else
                        For i As Integer = 1 To 10
                            If i <= stock.max_kg Then
                                ddl.Items.Add(New ListItem(i.ToString, i.ToString))
                            End If
                        Next
                    End If

                    ddl.Items.Insert(0, New ListItem(msgPriceSelect, ""))
                Case "96Mini"
                    For i As Integer = 5 To 50 Step 5
                        ddl.Items.Add(New ListItem(i.ToString, i.ToString))
                    Next
                    ddl.Items.Insert(0, New ListItem(msgPriceSelect, ""))
            End Select
        Catch ex As Exception
            Throw ex

        End Try
    End Sub

#Region "Ornament"
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
        If gold_type_id <> "" Then
            sql_gold_type = " AND (ticket.gold_type_id = '96g') "
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
                            " AND (book_no =  @book_no or @book_no = '') " & _
                            " AND (run_no =  @run_no or @run_no = '') " & _
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
                            " ORDER BY gold_type_id desc,type,ticket.price "

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
#End Region

#Region "Trade"
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
                    Return strOnline + DateTime.Now.ToString("ddMMyy/") + result.ToString("0000")
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
#End Region

#Region "Guarantee"
    Public Shared Function checkGuaranteeTradeCount(cust_id As String) As Dictionary(Of String, String)
        'For count gold trade able

        Dim result As New Dictionary(Of String, String)

        Dim dc As New dcDBDataContext
        Dim ctm = (From c In dc.v_customer_sum_assets Where c.cust_id = cust_id).FirstOrDefault

        If ctm.margin_unlimit Then
            result.Add("Unlimit", "Unlimit")
            Return result
        End If

        'Add กำไรจากการ ซื้อขายของลูกค้า ถ้าไม่ได้เป็น leavr order
        Dim netAmount As Double = calProfitLossTicket(cust_id)

        'Add Free margin in cash
        ctm.cash_credit = ctm.cash_credit + ctm.free_margin + netAmount

        Dim credit96 As Double = 0
        Dim credit99 As Double = 0
        Dim grt_pay As Double = (From stock In dc.stock_onlines).FirstOrDefault.grt_pay
        Dim rate96 As Double = 1000 * grt_pay
        Dim rate99 As Double = 65000 * grt_pay
        'Cash
        Dim cashMin96 As Double = (From g In dc.guarantees Where g.grt_trade96 = 10).FirstOrDefault.grt_value
        Dim cashMin99 As Double = (From g In dc.guarantees Where g.grt_trade99 = 1).FirstOrDefault.grt_value
        If ctm.cash_credit >= cashMin96 Then
            credit96 = Math.Floor(clsManage.convert2zero(ctm.cash_credit.ToString) / rate96)
        End If
        If ctm.cash_credit >= cashMin99 Then
            credit99 = Math.Floor(clsManage.convert2zero(ctm.cash_credit.ToString) / rate99)
        End If

        'Gold96
        Dim gold96Min = (From g In dc.guarantees Where g.grt_trade96 = 200 And g.grt_trade99 = 3).FirstOrDefault
        If ctm.quan_96 >= gold96Min.grt_value Then
            credit96 += Math.Floor(clsManage.convert2zero(ctm.quan_96) * 20)
            credit99 += Math.Floor(clsManage.convert2zero(ctm.quan_96) * 0.3)
        End If

        'Gold99
        Dim gold99Min = (From g In dc.guarantees Where g.grt_trade96 = 1500 And g.grt_trade99 = 22).FirstOrDefault
        If ctm.quan_99N >= gold99Min.grt_value Then
            credit96 += Math.Floor(clsManage.convert2zero(ctm.quan_99N) * 1500)
            credit99 += Math.Floor(clsManage.convert2zero(ctm.quan_99N) * 22)
        End If

        result.Add(credit96, credit99)
        Return result

    End Function

    Public Shared Function checkGuarantee(cust_id As String, type As String, purity As String, quan As Double, leave As Boolean, trade As Boolean) As String
        Dim dc As New dcDBDataContext
        Dim ctm = (From c In dc.v_customer_sum_assets Where c.cust_id = cust_id).FirstOrDefault

        If ctm.margin_unlimit Then
            Return ""
        End If

        'Add กำไรจากการ ซื้อขายของลูกค้า ถ้าไม่ได้เป็น leavr order
        Dim netAmount As Double = 0
        If leave = False Then
            netAmount = calProfitLossTicket(cust_id)
        End If

        'Add Free margin in cash
        ctm.cash_credit = ctm.cash_credit + ctm.free_margin + netAmount

        Dim credit96 As Double = 0
        Dim credit99 As Double = 0
        Dim grt_pay As Double = (From stock In dc.stock_onlines).FirstOrDefault.grt_pay
        Dim rate96 As Double = 1000 * grt_pay
        Dim rate99 As Double = 65000 * grt_pay

        'Dim cash96Grt = (From g In dc.guarantees Where g.grt_trade96 = 10).FirstOrDefault
        'Dim cash99Grt = (From g In dc.guarantees Where g.grt_trade99 = 1).FirstOrDefault

        'Cash
        Dim cashMin96 As Double = (From g In dc.guarantees Where g.grt_trade96 = 10).FirstOrDefault.grt_value
        Dim cashMin99 As Double = (From g In dc.guarantees Where g.grt_trade99 = 1).FirstOrDefault.grt_value
        If ctm.cash_credit >= cashMin96 Then
            credit96 = Math.Floor(clsManage.convert2zero(ctm.cash_credit.ToString) / rate96)
        End If
        If ctm.cash_credit >= cashMin99 Then
            credit99 = Math.Floor(clsManage.convert2zero(ctm.cash_credit.ToString) / rate99)
        End If

        rate96 = 200 / (grt_pay * 10)
        rate99 = 3 / (grt_pay * 10)
        'Gold96
        Dim gold96Min = (From g In dc.guarantees Where g.grt_trade96 = 200 And g.grt_trade99 = 3).FirstOrDefault
        If ctm.quan_96 >= gold96Min.grt_value Then
            credit96 += Math.Floor(clsManage.convert2zero(ctm.quan_96) * rate96)
            credit99 += Math.Floor(clsManage.convert2zero(ctm.quan_96) * rate99)
        End If

        rate96 = 1500 / grt_pay
        rate99 = 22 / grt_pay
        'Gold99
        Dim gold99Min = (From g In dc.guarantees Where g.grt_trade96 = 1500 And g.grt_trade99 = 22).FirstOrDefault
        If ctm.quan_99N >= gold99Min.grt_value Then
            credit96 += Math.Floor(clsManage.convert2zero(ctm.quan_99N) * rate96)
            credit99 += Math.Floor(clsManage.convert2zero(ctm.quan_99N) * rate99)
        End If

        Dim type_ticket As String = type 'สลับ buy sell เฉพาะ trade online
        If trade = True Then
            type_ticket = IIf(type = "sell", "buy", "sell")
        End If

        Dim quanPendingTicket As Double = 0
        Dim tk = (From t In dc.v_ticket_sum_splits Where t.cust_id = cust_id And t.type = type_ticket And t.gold_type_id = purity And t.status_id = "101" Select t.quantity)
        If tk.Count > 0 Then
            quanPendingTicket = tk.Sum
        End If
        Dim quanTradePending As Double = 0
        Dim tr = (From tra In dc.trades Where tra.cust_id = cust_id And tra.type = type And tra.gold_type_id = purity And tra.mode = "tran" Select tra.quantity)
        If tr.Count > 0 Then
            quanTradePending = tr.Sum
        End If

        Dim quanPending As Double = quan + quanPendingTicket + quanTradePending
        Dim quanTradeAble As Double = 0

        If purity = clsFng.p96 Then
            If quanPending > credit96 Then
                quanTradeAble = credit96 - (quanPendingTicket + quanTradePending)
                Dim sms As String = String.Format("หลักประกันไม่พอ หลักประกันในการ{0} ของคูณจำกัดที่ " + quanTradeAble.ToString + " บาท", IIf(type = "buy", "ซื้อ", "ขาย"))
                Return sms
            End If
        Else
            If quanPending > credit99 Then
                quanTradeAble = credit99 - (quanPendingTicket + quanTradePending)
                Dim sms As String = String.Format("หลักประกันไม่พอ หลักประกันในการ{0} ของคูณจำกัดที่ " + quanTradeAble.ToString + " กิโล", IIf(type = "buy", "ซื้อ", "ขาย"))
                Return sms
            End If
        End If

        Return ""
    End Function

    Public Shared Function calProfitLossTicket(cust_id As String) As Double
        Dim netAmount96 As Double = 0
        Dim netAmount99 As Double = 0
        Dim dc As New dcDBDataContext

        '96
        Dim tkB96 = (From t In dc.v_ticket_sum_splits Where t.cust_id = cust_id And t.status_id = 101 And t.type = "buy" And t.gold_type_id = "96" Order By t.ticket_id Ascending)
        Dim tkS96 = (From t In dc.v_ticket_sum_splits Where t.cust_id = cust_id And t.status_id = 101 And t.type = "sell" And t.gold_type_id = "96" Order By t.ticket_id Ascending)
        If tkB96.Count > 0 And tkS96.Count > 0 Then
            Dim sumB As Double = (From t In dc.v_ticket_sum_splits Where t.cust_id = cust_id And t.status_id = 101 And t.type = "buy" And t.gold_type_id = "96" Select t.quantity).Sum
            Dim sumS As Double = (From t In dc.v_ticket_sum_splits Where t.cust_id = cust_id And t.status_id = 101 And t.type = "sell" And t.gold_type_id = "96" Select t.quantity).Sum
            Dim iQuan As Double = 0
            Dim iAmount As Double = 0

            If sumB = sumS Then
                Dim spc = dc.getSumPortfolioByCust_id(cust_id).SingleOrDefault
                If spc.amount96 IsNot Nothing Then
                    netAmount96 += spc.amount96
                End If
            ElseIf sumB > sumS Then
                For Each t In tkB96 'วนในรูปทีมีค่า มากกว่า เพื่อหาค่าที่ quantity เท่ากัน
                    If iQuan < sumS Then 'วนหาค่าที่มี quantity เท่ากัน
                        If t.quantity + iQuan > sumS Then
                            iAmount += (sumS - iQuan) * t.price
                            Exit For
                        Else
                            iQuan += t.quantity
                            iAmount += t.amount
                        End If
                    End If
                Next
                Dim amountSell As Double = (From t In dc.v_ticket_sum_splits Where t.cust_id = cust_id And t.status_id = 101 And t.type = "sell" And t.gold_type_id = "96" Select t.amount).Sum
                netAmount96 = iAmount - amountSell
            ElseIf sumB < sumS Then
                For Each t In tkS96
                    If iQuan < sumB Then
                        If t.quantity + iQuan > sumB Then
                            iAmount += (sumB - iQuan) * t.price
                            Exit For
                        Else
                            iQuan += t.quantity
                            iAmount += t.amount
                        End If
                    End If
                Next
                Dim amountBuy As Double = (From t In dc.v_ticket_sum_splits Where t.cust_id = cust_id And t.status_id = 101 And t.type = "buy" And t.gold_type_id = "96" Select t.amount).Sum
                netAmount96 = amountBuy - iAmount
            End If
        End If

        '99.99
        Dim tkB99 = (From t In dc.v_ticket_sum_splits Where t.cust_id = cust_id And t.status_id = 101 And t.type = "buy" And t.gold_type_id = "99" Order By t.ticket_id Ascending)
        Dim tkS99 = (From t In dc.v_ticket_sum_splits Where t.cust_id = cust_id And t.status_id = 101 And t.type = "sell" And t.gold_type_id = "99" Order By t.ticket_id Ascending)
        If tkB99.Count > 0 And tkS99.Count > 0 Then
            Dim sumB As Double = (From t In dc.v_ticket_sum_splits Where t.cust_id = cust_id And t.status_id = 101 And t.type = "buy" And t.gold_type_id = "99" Select t.quantity).Sum
            Dim sumS As Double = (From t In dc.v_ticket_sum_splits Where t.cust_id = cust_id And t.status_id = 101 And t.type = "sell" And t.gold_type_id = "99" Select t.quantity).Sum
            Dim iQuan As Double = 0
            Dim iAmount As Double = 0

            If sumB = sumS Then
                Dim spc = dc.getSumPortfolioByCust_id(cust_id).SingleOrDefault
                If spc.amount99 IsNot Nothing Then
                    netAmount99 += spc.amount99
                End If
            ElseIf sumB > sumS Then
                For Each t In tkB99 'วนในรูปทีมีค่า มากกว่า เพื่อหาค่าที่ quantity เท่ากัน
                    If iQuan < sumS Then 'วนหาค่าที่มี quantity เท่ากัน
                        If t.quantity + iQuan > sumS Then
                            iAmount += (sumS - iQuan) * t.price * 65.6
                            Exit For
                        Else
                            iQuan += t.quantity
                            iAmount += t.amount
                        End If
                    End If
                Next
                Dim amountSell As Double = (From t In dc.v_ticket_sum_splits Where t.cust_id = cust_id And t.status_id = 101 And t.type = "sell" And t.gold_type_id = "99" Select t.amount).Sum
                netAmount99 = iAmount - amountSell
            ElseIf sumB < sumS Then
                For Each t In tkS99
                    If iQuan < sumB Then
                        If t.quantity + iQuan > sumB Then
                            iAmount += (sumB - iQuan) * t.price * 65.6
                            Exit For
                        Else
                            iQuan += t.quantity
                            iAmount += t.amount
                        End If
                    End If
                Next
                Dim amountBuy As Double = (From t In dc.v_ticket_sum_splits Where t.cust_id = cust_id And t.status_id = 101 And t.type = "buy" And t.gold_type_id = "99" Select t.amount).Sum
                netAmount99 = amountBuy - iAmount
            End If
        End If

        'ถ้ากำไรไม่เอามาคิด
        If netAmount96 > 0 Then netAmount96 = 0
        If netAmount99 > 0 Then netAmount99 = 0

        Return netAmount96 + netAmount99
    End Function

    Public Shared Function getGuarantee() As DataTable
        Try

            Dim sql As New StringBuilder()
            sql.Append("SELECT grt_value, grt_type, grt_rate, grt_trade96, grt_trade99, grt_id ")
            sql.Append("FROM guarantee")

            Using con As New SqlConnection(strcon)
                Using da As New SqlDataAdapter(sql.ToString, con)
                    Using dt As New DataTable
                        da.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getCustomerAsset(cust_id As String) As DataTable
        Try
            If cust_id = "" Then Return Nothing
            Dim sql As New StringBuilder()
            sql.Append("select id,cash_credit,quan96,quan99,free_margin,created_date,modifier_date,user_name as modifier_by from customer_asset c inner join users u on c.modifier_by = u.user_id ")
            sql.Append("where cust_id = '" + cust_id + "' and active = 1")

            Using con As New SqlConnection(strcon)
                Using da As New SqlDataAdapter(sql.ToString, con)
                    Using dt As New DataTable
                        da.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function


#End Region

#Region "Tickets"
    Public Shared Function updateReceiptNo(ByVal runno As String) As Boolean
        Try
            Dim sql As String = String.Format("update tickets set receipt_date = NULL, run_no = '' where run_no  = '{0}' ", runno)
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

    Public Shared Function updateReceiptNoSplit(ByVal sp_id As String) As Boolean
        Try
            Dim sql As String = String.Format("update ticket_split set receipt_split_date = NULL, run_no = NULL where ticket_sp_id  = {0} ", sp_id)
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

    Public Shared Function checkReceiptNoAndSplitBill1(ByVal refno As String, billing As String) As String
        Try
            Dim msg As String = ""
            refno = refno.Replace(",", "','")
            Dim sql As String = String.Format("select billing,run_no,sp_quan from v_ticket_sum_split where ref_no in ('{0}') ", refno)
            Using con As New SqlConnection(strcon)
                Using da As New SqlDataAdapter(sql, con)
                    Using dt As New DataTable
                        da.Fill(dt)
                        If dt.Rows.Count > 0 Then
                            For Each dr As DataRow In dt.Rows
                                'check ว่าbilling ตรงกับ ฐานข้อมูลหรือป่าว
                                If dr("billing").ToString <> billing Then
                                    Return "ข้อมูลมีการเปลี่ยนแปลง โปรดทำรายการใหม่อีกครั้ง"
                                End If
                                If dr("run_no").ToString <> "" Then
                                    msg = "โปรดเลือก Ticket ที่ยังไม่มี Receipt"
                                End If
                                'check ว่ามี split หรือยัง
                                If dr("sp_quan").ToString <> "" Then
                                    msg = "โปรดเลือก Ticket ที่ยังไม่มีการแยกบิล"
                                End If
                            Next
                        End If
                    End Using
                End Using
            End Using
            Return msg
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function checkReceiptNoPayment(ByVal refno As String, billing As String) As String
        Try
            Dim msg As String = ""
            refno = refno.Replace(",", "','")
            Dim sql As String = String.Format("select billing,run_no,sp_quan,payment,payment_id,status_id from v_ticket_sum_split where ref_no in ('{0}') ", refno)
            Using con As New SqlConnection(strcon)
                Using da As New SqlDataAdapter(sql, con)
                    Using dt As New DataTable
                        da.Fill(dt)
                        If dt.Rows.Count > 0 Then
                            For Each dr As DataRow In dt.Rows
                                'check ว่าbilling ตรงกับ ฐานข้อมูลหรือป่าว
                                If Not dr("status_id").ToString.Contains("9") Then
                                    Return "โปรดเลือกรายการที่ Complete แล้ว"
                                End If
                                If dr("billing").ToString <> billing Then
                                    Return "ข้อมูลมีการเปลี่ยนแปลง โปรดทำรายการใหม่อีกครั้ง"
                                End If
                                If dr("run_no").ToString <> "" Then
                                    msg = "โปรดเลือก Ticket ที่ยังไม่มี Receipt"
                                End If
                                If dr("payment").ToString = "" Then
                                    msg = "โปรดเลือกประเภทของการชำระเงิน"
                                End If
                                If dr("payment_id").ToString <> "" Then
                                    msg = "โปรดเลือกรายการที่ยังไม่ออก Payment"
                                End If

                                If dr("sp_quan").ToString <> "" Then
                                    msg = "โปรดเลือก Ticket ที่ยังไม่มีการแยกบิล"
                                End If
                            Next
                        End If
                    End Using
                End Using
            End Using
            Return msg
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Shared Function checkReceiptNoSplit(ByVal sid As String) As String
        Try
            Dim msg As String = ""
            Dim sql As String = String.Format("select run_no from ticket_split where ticket_sp_id in ({0}) ", sid)
            Using con As New SqlConnection(strcon)
                Using da As New SqlDataAdapter(sql, con)
                    Using dt As New DataTable
                        da.Fill(dt)
                        If dt.Rows.Count > 0 Then
                            For Each dr As DataRow In dt.Rows
                                If dr("run_no").ToString <> "" Then
                                    msg = "โปรดเลือก Ticket ที่ยังไม่มี Receipt"
                                End If
                            Next
                        End If
                    End Using
                End Using
            End Using
            Return msg
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'ถ้ามีการออก reciept แล้ว ห้าม update
    Public Shared Function checkTicketReceipt(ByVal ref_no As String) As String
        Try
            Dim msg As String = ""
            Dim sql As String = String.Format("select run_no from tickets where ref_no = '{0}'  ", ref_no)
            Using con As New SqlConnection(strcon)
                Using da As New SqlDataAdapter(sql, con)
                    Using dt As New DataTable
                        da.Fill(dt)
                        If dt.Rows.Count > 0 Then
                            If dt.Rows(0)(0).ToString <> "" Then
                                msg = "ไม่สามารถแก้ไขได้เนื่องจากมีการออก Receipt เรียบร้อยแล้ว"
                            End If
                        End If
                    End Using
                End Using
            End Using
            Return msg
        Catch ex As Exception
            Throw ex
        End Try
    End Function
#End Region

#Region "Payment"
    Public Shared Function getPayment() As DataTable
        Try
            Dim sql As String = "select *,(select  user_name from users where user_id = created_by) as payment_by_name  from payment where active = 1 order by payment_id desc"
            Using con As New SqlConnection(strcon)
                Using da As New SqlDataAdapter(sql, con)
                    Using dt As New DataTable
                        da.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getPaymentById(pid As String) As DataTable
        Try
            Dim sql As String = "select * from payment where payment_id = " + pid
            Using con As New SqlConnection(strcon)
                Using da As New SqlDataAdapter(sql, con)
                    Using dt As New DataTable
                        da.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getPaymentTicket(payment_id As String) As DataTable
        Try
            Dim sql As String = "select *,(select  user_name from users where user_id = payment_by) as payment_by_name  from tickets where payment_id =  " + payment_id
            Using con As New SqlConnection(strcon)
                Using da As New SqlDataAdapter(sql, con)
                    Using dt As New DataTable
                        da.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getPaymentSplit(payment_id As String) As DataTable
        Try
            Dim sql As String = "SELECT s.ticket_sp_id, s.ref_no, s.created_by, s.payment_id, s.payment_by, t.type,s.quantity,t.price,s.amount, u.user_name as  payment_by_name " & _
                                "FROM ticket_split AS s INNER JOIN tickets AS t ON s.ref_no = t.ref_no LEFT OUTER JOIN users AS u ON t.user_id = u.user_id " & _
                                "where s.payment_id = " + payment_id
            Using con As New SqlConnection(strcon)
                Using da As New SqlDataAdapter(sql, con)
                    Using dt As New DataTable
                        da.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getPaymentTrans(payment_id As String) As DataTable
        Try
            Dim sql As String = "select *,(select  user_name from users where user_id = payment_by) as payment_by_name  from customer_trans where payment_id =  " + payment_id
            Using con As New SqlConnection(strcon)
                Using da As New SqlDataAdapter(sql, con)
                    Using dt As New DataTable
                        da.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function


    Public Shared Function getPaymentTicketPopUp(ticket_idList As String, cust_id As String) As DataTable
        Try
            Dim sql As String = "select * from tickets t left join users u on t.created_by = u.user_id " & _
                                "Where status_id in(901,902,903) And payment_id Is NULL and (run_no = '' or run_no is NULL) and cust_id = " + cust_id + " " & _
                                "and ref_no not in ('" + ticket_idList + "')  order by ticket_id desc"
            Using con As New SqlConnection(strcon)
                Using da As New SqlDataAdapter(sql, con)
                    Using dt As New DataTable
                        da.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getPaymentSplitPopUp(ticket_idList As String, cust_id As String) As DataTable
        Try
            Dim sql As String = "select t.ticket_sp_id, t.ref_no, t.run_no, t.quantity, tk.price, t.amount, t.created_by, t.payment_id, t.payment_by, u.user_id, " & _
                                "u.user_name, u.team_id, u.password, u.firstname,u.lastname, u.status, u.position, u.position_id, tk.type " & _
                                "from ticket_split t inner join tickets tk on t.ref_no = tk.ref_no left join users u on t.created_by = u.user_id " & _
                                "Where t.status_id in(901,902,903) And t.payment_id Is NULL and (t.run_no = '' or t.run_no is NULL) " & _
                                "and t.ref_no in (select ref_no from tickets where cust_id = " + cust_id + ") " & _
                                "and t.ticket_sp_id not in ('" + ticket_idList + "')  order by ticket_sp_id desc"
            Using con As New SqlConnection(strcon)
                Using da As New SqlDataAdapter(sql, con)
                    Using dt As New DataTable
                        da.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function getPaymentTransPopUp(idList As String, cust_id As String) As DataTable
        Try
            'Dim sql As String = "select *,(select  user_name from users where user_id = payment_by) as payment_by_name from customer_trans " & _
            '                    "Where payment_id Is NULL and cust_id = " + cust_id + " " & _
            '                    "and cust_tran_id not in ('" + idList + "')  order by cust_tran_id desc"

            Dim sql As String = "select * from customer_trans c right join users u on c.created_by = u.user_id " & _
                                "Where payment_id Is NULL and cust_id = " + cust_id + " " & _
                                "and cust_tran_id not in ('" + idList + "')  order by cust_tran_id desc"


            Using con As New SqlConnection(strcon)
                Using da As New SqlDataAdapter(sql, con)
                    Using dt As New DataTable
                        da.Fill(dt)
                        Return dt
                    End Using
                End Using
            End Using
        Catch ex As Exception
            Throw ex
        End Try
    End Function

#End Region
End Class
