Imports Microsoft.VisualBasic
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.Drawing.Text
Imports System.IO
Imports System.Text.RegularExpressions
Imports System.Data

'Imports iTextSharp.text
'Imports iTextSharp.text.pdf


Public Class clsManage


    Public Shared yes As String = "y"
    Public Shared no As String = "n"
    Public Shared formatDateTime As String = "d/M/yyyy"
    Public Shared formatCash As String = "#,##0"
    Public Shared formatCurrency As String = "#,##0.00"
    Public Shared formatQuantity As String = "#,##0.00000"
    Public Shared msgRequiredFill As String = "Some information is required."
    Public Shared msgRequireSelect As String = "--None--"
    Public Shared msgDel As String = "Do you want to delete?"
    Public Shared msgPay As String = "Do you want to Pay?"
    Public Shared EmptyDataText As String = "<div style='color:red;text-align:center;border:solid 1px silver;'>Data not Found.</div>"
    Public Shared DisplayAfter As Integer = 5000
    Public Shared phoneDigit As Integer = ConfigurationManager.AppSettings("PHONE_DIGIT").ToString

    Enum iSession
        user_id
        user_name
        cust_id
        role
        cust_lv
        first_trade
        online_id
        timetrade

        user_id_center
        user_name_center
        cust_id_center
        user_role_center
        user_position_center

        user_id_gram
        user_name_gram
        cust_id_gram

    End Enum

    Enum iCookie
        username
        password
        online_id
    End Enum

    Enum tradeMode
        tran
        accept
        reject
    End Enum

    Enum rejectType
        Customer
        FinestGOLD
        Session
    End Enum
    Enum EditMode
        add
        edit
        view
        del
    End Enum

    Enum payment
        cash
        cheq
        trans
    End Enum

    Public Enum splitMode
        Split
        Receipt
        DepositGold

    End Enum

#Region "Function"

    Public Shared Function genPwd(ByVal num As Integer) As String
        'This function is used to generate a random PASSWORD that can be  
        'emailed to the user. 
        Const strDefault = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ01234567890"
        Const strFirstChar = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"
        Dim iCount As Integer
        Dim strReturn As String
        Dim iNumber As Integer
        Dim iLength As Integer

        Randomize()

        'Generate first character of PASSWORD which has to be a letter 
        iLength = Len(strFirstChar)
        iNumber = Int((iLength * Rnd()) + 1)
        strReturn = Mid(strFirstChar, iNumber, 1)

        'Generate the next n characters of the PASSWORD 
        iLength = Len(strDefault)
        For iCount = 1 To num - 1
            iNumber = Int((iLength * Rnd()) + 1)
            strReturn += Mid(strDefault, iNumber, 1)
        Next

        Return strReturn

    End Function


    Public Shared Function getNetEquity(ByVal cash As Double, ByVal bit As Double, ByVal margin As Double, ByVal paid As Double) As Double
        Return 0
    End Function

#Region "customer_portfolio"
    Public Structure CustPortFolio
        Public CB As Double
        Public Re As Double
        'Public ReMaABS As Double 'Revaluation Margin ABS Value
        Public NE As Double 'Net Equity
        Public Ma1 As Double
        Public Ma2 As Double
        Public Ex1 As Double
        Public Ex2 As Double
        Public ExFM1 As Double ' Ex/short + free_margin
        Public ExFM2 As Double

    End Structure

    Public Shared Function firstTicket(ByVal cash As Double, ByVal margin As Double, ByVal revolutioin As Double) As Double
        Return (100 / margin) * (cash + revolutioin)
    End Function

    Public Shared Function cash_balance(ByVal cash As Double, ByVal sum96 As Double, ByVal sum99 As Double) As Double
        Return cash + sum96 + sum99
    End Function

    Public Shared Function netEquity(ByVal cash As Double, ByVal sumAmount96Buy As Double, ByVal sumAmount99Buy As Double, ByVal sumAmount96Sell As Double, ByVal sumAmount99Sell As Double, ByVal bid96 As Double, ByVal bid99 As Double, ByVal ask96 As Double, ByVal ask99 As Double, ByVal sumquan96Buy As Double, ByVal sumquan99Buy As Double, ByVal sumquan96Sell As Double, ByVal sumquan99Sell As Double, ByVal gold96 As Double, ByVal gold99 As Double) As Double
        'choose net Equity least between 96(divide buy and sell) and 99(divide buy and sell)

        'purity 96
        Dim CBb96 As Double = cash + sumAmount96Buy
        Dim CBs96 As Double = cash + sumAmount96Sell

        Dim Rb96 As Double = revaluation96(bid96, ask96, sumquan96Buy, gold96)
        Dim Rs96 As Double = revaluation96(bid96, ask96, sumquan96Sell, gold96)

        Dim NE96 As Double = 0

        If (CBb96 + Rb96) < (CBs96 + CBs96) Then
            NE96 = CBb96 + Rb96
        Else
            NE96 = CBs96 + CBs96
        End If

        'purity 99
        Dim CBb99 As Double = cash + sumAmount99Buy
        Dim CBs99 As Double = cash + sumAmount99Sell

        Dim Rb99 As Double = revaluation99(bid99, ask99, sumquan99Buy, gold99)
        Dim Rs99 As Double = revaluation99(bid99, ask99, sumquan99Sell, gold99)

        Dim NE99 As Double = 0

        If (CBb99 + Rb99) < (CBs99 + CBs99) Then
            NE99 = CBb99 + Rb99
        Else
            NE99 = CBs99 + CBs99
        End If

        Return NE96 + NE99
    End Function
    Public Shared Function cal_custPortFolioRealize(ByVal cash As Double, ByVal sumAmount96 As Double, ByVal sumAmount99 As Double, ByVal bid96 As Double, ByVal bid99 As Double, ByVal ask96 As Double, ByVal ask99 As Double, ByVal sumquan96 As Double, ByVal sumquan99 As Double, ByVal gold96 As Double, ByVal gold99 As Double, ByVal margin_base As Double, ByVal margin_base2 As Double, ByVal free_margin As Double) As CustPortFolio
        Dim c As CustPortFolio
        c.CB = cash + sumAmount96 + sumAmount99
        c.Re = revaluation(bid96, bid99, ask96, ask99, sumquan96, sumquan99, gold96, gold99)
        c.NE = c.CB + c.Re
        c.Ma1 = (Math.Abs(c.Re) * -1) * (margin_base / 100)
        c.Ma2 = (Math.Abs(c.Re) * -1) * (margin_base2 / 100)
        c.Ex1 = c.CB + c.Re + c.Ma1
        c.Ex2 = c.CB + c.Re + c.Ma2
        c.ExFM1 = c.CB + c.Re + c.Ma1 + free_margin
        c.ExFM2 = c.CB + c.Re + c.Ma2 + free_margin

        Return c
    End Function

    Public Shared Function cal_custPortFolio(ByVal cash As Double, ByVal sumAmount96Buy As Double, ByVal sumAmount99Buy As Double, ByVal sumAmount96Sell As Double, ByVal sumAmount99Sell As Double, ByVal bid96 As Double, ByVal bid99 As Double, ByVal ask96 As Double, ByVal ask99 As Double, ByVal sumquan96Buy As Double, ByVal sumquan99Buy As Double, ByVal sumquan96Sell As Double, ByVal sumquan99Sell As Double, ByVal gold96 As Double, ByVal gold99 As Double, ByVal margin_base As Double, ByVal margin_base2 As Double, ByVal free_margin As Double) As CustPortFolio
        Dim cpf As CustPortFolio

        '#### Cash Balance and Revaluation
        'choose net Equity least between 96(divide buy and sell) and 99(divide buy and sell)
        'purity 96
        Dim CBb96 As Double = cash + sumAmount96Buy
        Dim CBs96 As Double = cash + sumAmount96Sell

        Dim Rb96 As Double = revaluation96(bid96, ask96, sumquan96Buy, gold96)
        Dim Rs96 As Double = revaluation96(bid96, ask96, sumquan96Sell, gold96)

        Dim CB96 As Double = 0
        Dim R96 As Double = 0
        Dim NE96 As Double = 0

        If (CBb96 + Rb96) < (CBs96 + Rs96) Then
            CB96 = CBb96
            R96 = Rb96
            NE96 = CBb96 + Rb96
        Else
            CB96 = CBs96
            R96 = Rs96
            NE96 = CBs96 + Rs96
        End If

        'purity 99
        Dim CBb99 As Double = cash + sumAmount99Buy
        Dim CBs99 As Double = cash + sumAmount99Sell

        Dim Rb99 As Double = revaluation99(bid99, ask99, sumquan99Buy, gold99)
        Dim Rs99 As Double = revaluation99(bid99, ask99, sumquan99Sell, gold99)

        Dim CB99 As Double = 0
        Dim R99 As Double = 0
        Dim NE99 As Double = 0

        If (CBb99 + Rb99) < (CBs99 + Rs99) Then
            CB99 = CBb99
            R99 = Rb99
            NE99 = CBb99 + Rb99
        Else
            CB99 = CBs99
            R99 = Rs99
            NE99 = CBs99 + Rs99
        End If

        'ลบ cash ออก
        cpf.CB = CB96 + CB99 - cash
        cpf.Re = R96 + R99
        cpf.NE = NE96 + NE99 - cash

        '#### Revaluation Margin
        Dim sumgold96 As Double = 0 : Dim sumgold99 As Double = 0 : Dim sum96 As Double = 0 : Dim sum99 As Double = 0
        Dim sum96Buy As Double = 0 : Dim sum99Buy As Double = 0 : Dim sum96Sell As Double = 0 : Dim sum99Sell As Double = 0

        'cal ทองฝาก 96 99
        If gold96 <> 0 Then
            sumgold96 = gold96 * bid96
        End If

        If gold99 <> 0 Then
            sumgold99 = gold99 * bid99 * 65.6
        End If

        'change requirment Leave Order เลือกทอง 96 99 ที่ทำให้ exceed short น้อยสุด

        If sumquan96Buy > 0 Then
            sum96Buy = bid96 * sumquan96Buy
        Else
            sum96Buy = ask96 * sumquan96Buy
        End If

        If sumquan96Sell > 0 Then
            sum96Sell = bid96 * sumquan96Sell
        Else
            sum96Sell = ask96 * sumquan96Sell
        End If
        If Math.Abs(sum96Sell + sumgold96) > Math.Abs(sum96Buy + sumgold96) Then 'เลือก result ของ ทอง 96 เพื่อให้ ex/short น้อยสุด (ใส่ abs แล้วเลือกค่ามากที่สุด เพื่อ * -1 จะได้ค่าติดลบมากสุด)
            sum96 = sum96Sell
        Else
            sum96 = sum96Buy
        End If


        If sumquan99Buy > 0 Then
            sum99Buy = bid99 * sumquan99Buy * 65.6
        Else
            sum99Buy = ask99 * sumquan99Buy * 65.6
        End If
        If sumquan99Sell > 0 Then
            sum99Sell = bid99 * sumquan99Sell * 65.6
        Else
            sum99Sell = ask99 * sumquan99Sell * 65.6
        End If
        If Math.Abs(sum99Sell + sumgold99) > Math.Abs(sum99Buy + sumgold99) Then 'เลือก result ของ ทอง 99 เพื่อให้ ex/short น้อยสุด (ใส่ abs แล้วเลือกค่ามากที่สุด เพื่อ * -1 จะได้ค่าติดลบมากสุด)
            sum99 = sum99Sell
        Else
            sum99 = sum99Buy
        End If

        Dim result96 As Double = sum96 + sumgold96
        Dim result99 As Double = sum99 + sumgold99
        Dim sum9699 As Double = Math.Abs(result96) + Math.Abs(result99)
        cpf.Ma1 = (sum9699 * -1) * (margin_base / 100)
        cpf.Ma2 = (sum9699 * -1) * (margin_base2 / 100)

        cpf.Ex1 = cpf.CB + cpf.Re + cpf.Ma1
        cpf.Ex2 = cpf.CB + cpf.Re + cpf.Ma2
        cpf.ExFM1 = cpf.CB + cpf.Re + cpf.Ma1 + free_margin
        cpf.ExFM2 = cpf.CB + cpf.Re + cpf.Ma2 + free_margin

        Return cpf

    End Function

    Public Shared Function revaluation_margin_ABS(ByVal bid96 As Double, ByVal bid99 As Double, ByVal ask96 As Double, ByVal ask99 As Double, ByVal sumquan96Buy As Double, ByVal sumquan99Buy As Double, ByVal sumquan96Sell As Double, ByVal sumquan99Sell As Double, ByVal gold96 As Double, ByVal gold99 As Double) As Double
        Dim sumgold96 As Double = 0 : Dim sumgold99 As Double = 0 : Dim sum96 As Double = 0 : Dim sum99 As Double = 0
        Dim sum96Buy As Double = 0 : Dim sum99Buy As Double = 0 : Dim sum96Sell As Double = 0 : Dim sum99Sell As Double = 0

        'cal ทองฝาก 96 99
        If gold96 <> 0 Then
            sumgold96 = gold96 * bid96
        End If

        If gold99 <> 0 Then
            sumgold99 = gold99 * bid99 * 65.6
        End If

        'change requirment Leave Order เลือกทอง 96 99 ที่ทำให้ exceed short น้อยสุด

        If sumquan96Buy > 0 Then
            sum96Buy = bid96 * sumquan96Buy
        Else
            sum96Buy = ask96 * sumquan96Buy
        End If

        If sumquan96Sell > 0 Then
            sum96Sell = bid96 * sumquan96Sell
        Else
            sum96Sell = ask96 * sumquan96Sell
        End If
        If Math.Abs(sum96Sell + sumgold96) > Math.Abs(sum96Buy + sumgold96) Then 'เลือก result ของ ทอง 96 เพื่อให้ ex/short น้อยสุด (ใส่ abs แล้วเลือกค่ามากที่สุด เพื่อ * -1 จะได้ค่าติดลบมากสุด)
            sum96 = sum96Sell
        Else
            sum96 = sum96Buy
        End If


        If sumquan99Buy > 0 Then
            sum99Buy = bid99 * sumquan99Buy * 65.6
        Else
            sum99Buy = ask99 * sumquan99Buy * 65.6
        End If
        If sumquan99Sell > 0 Then
            sum99Sell = bid99 * sumquan99Sell * 65.6
        Else
            sum99Sell = ask99 * sumquan99Sell * 65.6
        End If
        If Math.Abs(sum99Sell + sumgold99) > Math.Abs(sum99Buy + sumgold99) Then 'เลือก result ของ ทอง 99 เพื่อให้ ex/short น้อยสุด (ใส่ abs แล้วเลือกค่ามากที่สุด เพื่อ * -1 จะได้ค่าติดลบมากสุด)
            sum99 = sum99Sell
        Else
            sum99 = sum99Buy
        End If

        Dim result96 As Double = sum96 + sumgold96
        Dim result99 As Double = sum99 + sumgold99
        Dim sum9699 As Double = Math.Abs(result96) + Math.Abs(result99)
        Return sum9699

    End Function

    Public Shared Function revaluationForMargin(ByVal bid96 As Double, ByVal bid99 As Double, ByVal ask96 As Double, ByVal ask99 As Double, ByVal sumquan96 As Double, ByVal sumquan99 As Double, ByVal gold96 As Double, ByVal gold99 As Double) As Double
        Dim bid_ask As Double = 0
        Dim sum96 As Double = 0
        Dim sum99 As Double = 0
        Dim sumgold96 As Double = 0
        Dim sumgold99 As Double = 0

        If sumquan96 > 0 Then
            sum96 = bid96 * sumquan96
        Else
            sum96 = ask96 * sumquan96
        End If

        If sumquan99 > 0 Then
            sum99 = bid99 * sumquan99 * 65.6
        Else
            sum99 = ask99 * sumquan99 * 65.6
        End If

        If gold96 <> 0 Then
            sumgold96 = gold96 * bid96
        End If

        If gold99 <> 0 Then
            sumgold99 = gold99 * bid99 * 65.6
        End If

        Dim revaluation96 As Double = sum96 + sumgold96
        Dim revaluation99 As Double = sum99 + sumgold99
        Dim revaluationSum As Double = Math.Abs(revaluation96) + Math.Abs(revaluation99)
        Return revaluationSum
    End Function

    Public Shared Function revaluation96(ByVal bid96 As Double, ByVal ask96 As Double, ByVal sumquan96 As Double, ByVal gold96 As Double) As Double
        Dim sum96 As Double = 0
        Dim sumgold96 As Double = 0

        If sumquan96 > 0 Then
            sum96 = bid96 * sumquan96
        Else
            sum96 = ask96 * sumquan96
        End If

        If gold96 <> 0 Then
            sumgold96 = gold96 * bid96
        End If


        Return sum96 + sumgold96
    End Function

    Public Shared Function revaluation99(ByVal bid99 As Double, ByVal ask99 As Double, ByVal sumquan99 As Double, ByVal gold99 As Double) As Double
        Dim sum99 As Double = 0
        Dim sumgold99 As Double = 0

        If sumquan99 > 0 Then
            sum99 = bid99 * sumquan99 * 65.6
        Else
            sum99 = ask99 * sumquan99 * 65.6
        End If

        If gold99 <> 0 Then
            sumgold99 = gold99 * bid99 * 65.6
        End If
        Return sum99 + sumgold99
    End Function

    Public Shared Function revaluation(ByVal bid96 As Double, ByVal bid99 As Double, ByVal ask96 As Double, ByVal ask99 As Double, ByVal sumquan96 As Double, ByVal sumquan99 As Double, ByVal gold96 As Double, ByVal gold99 As Double) As Double
        Dim sum96 As Double = 0 : Dim sum99 As Double = 0
        Dim sumgold96 As Double = 0 : Dim sumgold99 As Double = 0

        If sumquan96 > 0 Then
            sum96 = bid96 * sumquan96
        Else
            sum96 = ask96 * sumquan96
        End If

        If sumquan99 > 0 Then
            sum99 = bid99 * sumquan99 * 65.6
        Else
            sum99 = ask99 * sumquan99 * 65.6
        End If

        If gold96 <> 0 Then
            sumgold96 = gold96 * bid96
        End If

        If gold99 <> 0 Then
            sumgold99 = gold99 * bid99 * 65.6
        End If
        Return sum96 + sum99 + sumgold96 + sumgold99
    End Function

    Public Shared Function minus_value(ByVal value As Double) As String
        If value >= 0 Then
            Return value.ToString(clsManage.formatCurrency).ToString
        Else
            Return "(" + (value * -1).ToString(clsManage.formatCurrency) + ")"
        End If
    End Function

    Public Shared Function getFolioListByCust_id(ByVal cust_id As String) As DataTable

        Dim revaluation_value As String = ""
        Dim netEquity_value As String = ""
        Dim excess_value As String = ""

        Dim dtSumQuan As Data.DataTable
        dtSumQuan = clsDB.getSumQuanTicketPortfolioByCust_id(cust_id)
        If dtSumQuan.Rows.Count > 0 Then

            Dim bid96 As String = dtSumQuan.Rows(0)("bid96").ToString
            Dim bid99 As String = dtSumQuan.Rows(0)("bid99").ToString
            Dim ask96 As String = dtSumQuan.Rows(0)("ask96").ToString
            Dim ask99 As String = dtSumQuan.Rows(0)("ask99").ToString
            Dim margin_base As Double = dtSumQuan.Rows(0)("margin").ToString

            Dim sumquan96 As Double = 0
            Dim sumquan99 As Double = 0
            Dim sumAmount96 As Double = 0
            Dim sumAmount99 As Double = 0
            Dim sumAsset96 As Double = 0
            Dim sumAsset99 As Double = 0
            Dim sumCashCredit As Double = 0

            If dtSumQuan.Rows(0)("cash").ToString <> "" Then sumCashCredit = dtSumQuan.Rows(0)("cash").ToString
            If dtSumQuan.Rows(0)("quan96").ToString <> "" Then sumquan96 = dtSumQuan.Rows(0)("quan96").ToString
            If dtSumQuan.Rows(0)("quan99").ToString <> "" Then sumquan99 = dtSumQuan.Rows(0)("quan99").ToString
            If dtSumQuan.Rows(0)("amount96").ToString <> "" Then sumAmount96 = dtSumQuan.Rows(0)("amount96").ToString
            If dtSumQuan.Rows(0)("amount99").ToString <> "" Then sumAmount99 = dtSumQuan.Rows(0)("amount99").ToString
            If dtSumQuan.Rows(0)("gold96").ToString <> "" Then sumAsset96 = dtSumQuan.Rows(0)("gold96").ToString
            If dtSumQuan.Rows(0)("gold99").ToString <> "" Then sumAsset99 = dtSumQuan.Rows(0)("gold99").ToString

            Dim cash_balance As Double = clsManage.cash_balance(sumCashCredit, sumAmount96, sumAmount99)
            Dim revaluation As Double = clsManage.revaluation(bid96, bid99, ask96, ask99, sumquan96, sumquan99, sumAsset96, sumAsset99)
            Dim revaluation_margin As Double = clsManage.revaluationForMargin(bid96, bid99, ask96, ask99, sumquan96, sumquan99, sumAsset96, sumAsset99)

            Dim netEquity As Double = cash_balance + revaluation
            Dim margin As Double = (Math.Abs(revaluation_margin) * -1) * (margin_base / 100)
            Dim excessShort As Double = cash_balance + revaluation + margin

            revaluation_value = revaluation.ToString(clsManage.formatCurrency)
            netEquity_value = netEquity.ToString(clsManage.formatCurrency)
            excess_value = excessShort.ToString(clsManage.formatCurrency)
        End If
        Dim dt As New DataTable
        dt.Columns.Add("revaluation", GetType(Double))
        dt.Columns.Add("netequity", GetType(Double))
        dt.Columns.Add("excess", GetType(Double))
        Dim dr As DataRow
        dr = dt.NewRow
        dr(0) = revaluation_value
        dr(1) = netEquity_value
        dr(2) = excess_value
        dt.Rows.Add(dr)
        Return dt
    End Function

    Public Shared Function checkExcessShortCash(ByVal cust_id As String, ByVal cash As Double) As String
        'case withdraw cash only
        Try
            Dim strAlert As String = "ถอนเงินเกินจำนวนที่ฝากไว้."
            Dim strAlertExcess As String = "Excess/Short ติดลบ ไม่สามารถถอนเงินได้."
            Dim strAlertLimit As String = "วงเงินเต็ม ไม่สามารถถอนเงินได้."

            Dim dtSumQuan As Data.DataTable
            dtSumQuan = clsDB.getSumQuanTicketPortfolioByCust_id(cust_id)
            If dtSumQuan.Rows.Count > 0 Then
                Dim bid96 As String = dtSumQuan.Rows(0)("bid96").ToString
                Dim bid99 As String = dtSumQuan.Rows(0)("bid99").ToString
                Dim ask96 As String = dtSumQuan.Rows(0)("ask96").ToString
                Dim ask99 As String = dtSumQuan.Rows(0)("ask99").ToString
                Dim margin_base As Double = clsManage.convert2zero(dtSumQuan.Rows(0)("margin").ToString)
                Dim trade_limit As Double = clsManage.convert2zero(dtSumQuan.Rows(0)("trade_limit").ToString) * -1
                Dim free_margin As Double = clsManage.convert2zero(dtSumQuan.Rows(0)("free_margin").ToString)

                Dim sumquan96 As Double = 0 : Dim sumquan99 As Double = 0 : Dim sumAmount96 As Double = 0 : Dim sumAmount99 As Double = 0
                Dim sumAsset96 As Double = 0 : Dim sumAsset99 As Double = 0 : Dim sumAsset99N As Double = 0 : Dim sumAsset99L As Double = 0
                Dim sumCashCredit As Double = 0

                If dtSumQuan.Rows(0)("cash").ToString <> "" Then sumCashCredit = dtSumQuan.Rows(0)("cash").ToString
                If dtSumQuan.Rows(0)("quan96").ToString <> "" Then sumquan96 = dtSumQuan.Rows(0)("quan96").ToString
                If dtSumQuan.Rows(0)("quan99").ToString <> "" Then sumquan99 = dtSumQuan.Rows(0)("quan99").ToString
                If dtSumQuan.Rows(0)("amount96").ToString <> "" Then sumAmount96 = dtSumQuan.Rows(0)("amount96").ToString
                If dtSumQuan.Rows(0)("amount99").ToString <> "" Then sumAmount99 = dtSumQuan.Rows(0)("amount99").ToString
                If dtSumQuan.Rows(0)("gold96").ToString <> "" Then sumAsset96 = dtSumQuan.Rows(0)("gold96").ToString
                If dtSumQuan.Rows(0)("gold99").ToString <> "" Then sumAsset99 = dtSumQuan.Rows(0)("gold99").ToString

                Dim sumCashCredit_new As Double = 0
                If cash > sumCashCredit Then
                    Return strAlert
                Else
                    sumCashCredit_new = sumCashCredit - cash
                End If

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

                'cpf : cpf_pre ต่างกันที่เงินฝาก
                Dim cpf As clsManage.CustPortFolio = clsManage.cal_custPortFolio(sumCashCredit_new, sumAmount96_tran_buy, sumAmount99_tran_buy, sumAmount96_tran_sell, sumAmount99_tran_sell, _
                                               bid96, bid99, ask96, ask99, sumquan96_tran_buy, sumquan99_tran_buy, sumquan96_tran_sell, sumquan99_tran_sell, _
                                               sumAsset96, sumAsset99, margin_base, 0, free_margin)

                Dim cpf_pre As clsManage.CustPortFolio = clsManage.cal_custPortFolio(sumCashCredit, sumAmount96_tran_buy, sumAmount99_tran_buy, sumAmount96_tran_sell, sumAmount99_tran_sell, _
                                               bid96, bid99, ask96, ask99, sumquan96_tran_buy, sumquan99_tran_buy, sumquan96_tran_sell, sumquan99_tran_sell, _
                                               sumAsset96, sumAsset99, margin_base, 0, free_margin)

                If Not trade_limit = 0 Then
                    If cpf.Ma1 < trade_limit Then
                        Return strAlertLimit
                    Else
                        If cpf.Ma1 < cpf_pre.Ma1 Then
                            Return strAlertLimit
                        End If
                    End If
                End If

                If margin_base = 0 Then
                    Return ""
                End If

                If cpf.ExFM1 >= 0 Then
                    Return ""
                Else
                    If cpf.Ma1 >= cpf_pre.Ma1 Then
                        Return ""
                    End If
                    Return strAlertExcess
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return "ไม่สามารถถอนได้"
    End Function

    Public Shared Function checkExcessShort(ByVal cust_id As String, ByVal gold_deposit As Double, ByVal purity As String, Optional ByVal mode As String = "") As String
        Try
            Dim revaluation_value As String = ""
            Dim netEquity_value As String = ""
            Dim excess_value As String = ""

            Dim strAlert As String = "ตัดทองฝากเกินจำนวนที่ฝากไว้."
            Dim strAlertExcess As String = "Excess/Short ติดลบ ไม่สามารถตัดทองฝากได้."
            Dim strAlertLimit As String = "วงเงินเต็ม ไม่สามารถตัดทองฝากได้."
            If mode <> "" Then
                strAlert = "ถอนทองเกินจำนวนที่ฝากไว้."
                strAlertExcess = "Excess/Short ติดลบ ไม่สามารถถอนทองได้."
                strAlertLimit = "วงเงินเต็ม ไม่สามารถถอนทองได้."
            End If

            Dim dtSumQuan As Data.DataTable
            dtSumQuan = clsDB.getSumQuanTicketPortfolioByCust_id(cust_id)
            If dtSumQuan.Rows.Count > 0 Then

                Dim bid96 As String = dtSumQuan.Rows(0)("bid96").ToString
                Dim bid99 As String = dtSumQuan.Rows(0)("bid99").ToString
                Dim ask96 As String = dtSumQuan.Rows(0)("ask96").ToString
                Dim ask99 As String = dtSumQuan.Rows(0)("ask99").ToString
                Dim margin_base As Double = clsManage.convert2zero(dtSumQuan.Rows(0)("margin").ToString)
                Dim trade_limit As Double = clsManage.convert2zero(dtSumQuan.Rows(0)("trade_limit").ToString) * -1
                Dim free_margin As Double = clsManage.convert2zero(dtSumQuan.Rows(0)("free_margin").ToString)

                Dim sumquan96 As Double = 0 : Dim sumquan99 As Double = 0 : Dim sumAmount96 As Double = 0 : Dim sumAmount99 As Double = 0
                Dim sumAsset96 As Double = 0 : Dim sumAsset99 As Double = 0 : Dim sumAsset99N As Double = 0 : Dim sumAsset99L As Double = 0
                Dim sumCashCredit As Double = 0

                If dtSumQuan.Rows(0)("cash").ToString <> "" Then sumCashCredit = dtSumQuan.Rows(0)("cash").ToString
                If dtSumQuan.Rows(0)("quan96").ToString <> "" Then sumquan96 = dtSumQuan.Rows(0)("quan96").ToString
                If dtSumQuan.Rows(0)("quan99").ToString <> "" Then sumquan99 = dtSumQuan.Rows(0)("quan99").ToString
                If dtSumQuan.Rows(0)("amount96").ToString <> "" Then sumAmount96 = dtSumQuan.Rows(0)("amount96").ToString
                If dtSumQuan.Rows(0)("amount99").ToString <> "" Then sumAmount99 = dtSumQuan.Rows(0)("amount99").ToString
                If dtSumQuan.Rows(0)("gold96").ToString <> "" Then sumAsset96 = dtSumQuan.Rows(0)("gold96").ToString
                If dtSumQuan.Rows(0)("gold99").ToString <> "" Then sumAsset99 = dtSumQuan.Rows(0)("gold99").ToString
                If dtSumQuan.Rows(0)("gold99N").ToString <> "" Then sumAsset99N = dtSumQuan.Rows(0)("gold99N").ToString
                If dtSumQuan.Rows(0)("gold99L").ToString <> "" Then sumAsset99L = dtSumQuan.Rows(0)("gold99L").ToString

                Dim sumAsset96_new As Double = 0 : Dim sumAsset99_new As Double = 0

                'check ทองถ้ามีน้อยกว่า ไม่ผ่าน
                'ถ้ามีพอให้ไป check ex/short ต่อ
                If purity = "96" Then
                    If gold_deposit > sumAsset96 Then
                        Return strAlert
                    Else
                        sumAsset96_new = sumAsset96 - gold_deposit
                    End If
                ElseIf purity = "99N" Then
                    If gold_deposit > sumAsset99N Then
                        Return strAlert
                    Else
                        sumAsset99N = sumAsset99N - gold_deposit
                        sumAsset99_new = sumAsset99 - gold_deposit
                    End If
                ElseIf purity = "99L" Then
                    If gold_deposit > sumAsset99L Then
                        Return strAlert
                    Else
                        sumAsset99L = sumAsset99L - gold_deposit
                        sumAsset99_new = sumAsset99 - gold_deposit
                    End If
                End If

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

                'cpf : cpf_pre ต่างกันที่ทองฝาก 
                Dim cpf As clsManage.CustPortFolio = clsManage.cal_custPortFolio(sumCashCredit, sumAmount96_tran_buy, sumAmount99_tran_buy, sumAmount96_tran_sell, sumAmount99_tran_sell, _
                                               bid96, bid99, ask96, ask99, sumquan96_tran_buy, sumquan99_tran_buy, sumquan96_tran_sell, sumquan99_tran_sell, _
                                               sumAsset96_new, sumAsset99_new, margin_base, 0, free_margin)

                Dim cpf_pre As clsManage.CustPortFolio = clsManage.cal_custPortFolio(sumCashCredit, sumAmount96_tran_buy, sumAmount99_tran_buy, sumAmount96_tran_sell, sumAmount99_tran_sell, _
                                               bid96, bid99, ask96, ask99, sumquan96_tran_buy, sumquan99_tran_buy, sumquan96_tran_sell, sumquan99_tran_sell, _
                                               sumAsset96, sumAsset99, margin_base, 0, free_margin)

                If Not trade_limit = 0 Then
                    If cpf.Ma1 < trade_limit Then
                        Return strAlertLimit
                    Else
                        If cpf.Ma1 < cpf_pre.Ma1 Then
                            Return strAlertLimit
                        End If
                    End If
                End If

                If margin_base = 0 Then
                    Return ""
                End If

                If cpf.ExFM1 >= 0 Then
                    Return ""
                Else
                    If cpf.Ma1 >= cpf_pre.Ma1 Then
                        Return ""
                    End If
                    Return strAlertExcess
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
        Return "ไม่สามารถทำรายการได้"
    End Function

    'check trade limit old version
    Public Shared Function checkTradeLimit_(ByVal cust_id As String, ByVal quantity As Double, ByVal amount As Double, ByVal type As String, ByVal purity As String) As Boolean
        'For trading ticket
        Try
            Dim revaluation_value As String = ""
            Dim netEquity_value As String = ""
            Dim excess_value As String = ""

            Dim dtSumQuan As Data.DataTable
            dtSumQuan = clsDB.getSumQuanTicketPortfolioByCust_id(cust_id)
            If dtSumQuan.Rows.Count > 0 Then

                Dim bid96 As String = dtSumQuan.Rows(0)("bid96").ToString
                Dim bid99 As String = dtSumQuan.Rows(0)("bid99").ToString
                Dim ask96 As String = dtSumQuan.Rows(0)("ask96").ToString
                Dim ask99 As String = dtSumQuan.Rows(0)("ask99").ToString
                Dim margin_base As Double = dtSumQuan.Rows(0)("margin").ToString
                Dim trade_limit As Double = clsManage.convert2zero(dtSumQuan.Rows(0)("trade_limit").ToString) * -1
                Dim free_margin As Double = clsManage.convert2zero(dtSumQuan.Rows(0)("free_margin").ToString)

                Dim sumquan96 As Double = 0 : Dim sumquan99 As Double = 0 : Dim sumAmount96 As Double = 0
                Dim sumAmount99 As Double = 0 : Dim sumAsset96 As Double = 0 : Dim sumAsset99 As Double = 0
                Dim sumCashCredit As Double = 0

                If dtSumQuan.Rows(0)("cash").ToString <> "" Then sumCashCredit = dtSumQuan.Rows(0)("cash").ToString
                If dtSumQuan.Rows(0)("quan96").ToString <> "" Then sumquan96 = dtSumQuan.Rows(0)("quan96").ToString
                If dtSumQuan.Rows(0)("quan99").ToString <> "" Then sumquan99 = dtSumQuan.Rows(0)("quan99").ToString
                If dtSumQuan.Rows(0)("amount96").ToString <> "" Then sumAmount96 = dtSumQuan.Rows(0)("amount96").ToString
                If dtSumQuan.Rows(0)("amount99").ToString <> "" Then sumAmount99 = dtSumQuan.Rows(0)("amount99").ToString
                If dtSumQuan.Rows(0)("gold96").ToString <> "" Then sumAsset96 = dtSumQuan.Rows(0)("gold96").ToString
                If dtSumQuan.Rows(0)("gold99").ToString <> "" Then sumAsset99 = dtSumQuan.Rows(0)("gold99").ToString


                If type = "sell" Then '* sell ticket +Quantity,-Amount
                    If purity = "96" Then
                        sumAmount96 = sumAmount96 - amount
                        sumquan96 = sumquan96 + quantity
                    Else
                        sumAmount99 = sumAmount99 - amount
                        sumquan99 = sumquan99 + quantity
                    End If
                Else  '* buy ticket -Quantity,+Amount
                    If purity = "96" Then
                        sumAmount96 = sumAmount96 + amount
                        sumquan96 = sumquan96 - quantity
                    Else
                        sumAmount99 = sumAmount99 + amount
                        sumquan99 = sumquan99 - quantity
                    End If
                End If


                Dim cpf As clsManage.CustPortFolio = clsManage.cal_custPortFolio(sumCashCredit, sumAmount96, sumAmount99, sumAmount96, sumAmount99, _
                                             bid96, bid99, ask96, ask99, sumquan96, sumquan99, sumquan96, sumquan99, _
                                             sumAsset96, sumAsset99, margin_base, 0, free_margin)


                Dim cash_balance As Double = clsManage.cash_balance(sumCashCredit, sumAmount96, sumAmount99)
                Dim revaluation As Double = clsManage.revaluation(bid96, bid99, ask96, ask99, sumquan96, sumquan99, sumAsset96, sumAsset99)
                Dim revaluation_margin As Double = clsManage.revaluationForMargin(bid96, bid99, ask96, ask99, sumquan96, sumquan99, sumAsset96, sumAsset99)

                Dim netEquity As Double = cash_balance + revaluation
                Dim margin As Double = (Math.Abs(revaluation_margin) * -1) * (margin_base / 100)
                Dim excessShort As Double = cash_balance + revaluation + margin

                If excessShort < 0 Then
                    Return False
                End If
            End If
            Return True

        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function checkTradeLimit(ByVal cust_id As String, ByVal quantity As Double, ByVal amount As Double, ByVal type As String, ByVal purity As String) As Boolean
        'For trading ticket in center
        'check all same trade online
        Try
            Dim dtSumQuan As Data.DataTable = clsDB.getSumQuanTicketPortfolioByCust_id(cust_id)
            If dtSumQuan.Rows.Count > 0 Then
                Dim bid96 As String = dtSumQuan.Rows(0)("bid96").ToString
                Dim bid99 As String = dtSumQuan.Rows(0)("bid99").ToString
                Dim ask96 As String = dtSumQuan.Rows(0)("ask96").ToString
                Dim ask99 As String = dtSumQuan.Rows(0)("ask99").ToString
                Dim margin_base As Double = dtSumQuan.Rows(0)("margin").ToString
                Dim trade_limit As Double = clsManage.convert2zero(dtSumQuan.Rows(0)("trade_limit").ToString) * -1
                Dim free_margin As Double = clsManage.convert2zero(dtSumQuan.Rows(0)("free_margin").ToString)

                Dim sumquan96 As Double = 0 : Dim sumquan99 As Double = 0 : Dim sumAmount96 As Double = 0
                Dim sumAmount99 As Double = 0 : Dim sumAsset96 As Double = 0 : Dim sumAsset99 As Double = 0
                Dim sumCashCredit As Double = 0

                If dtSumQuan.Rows(0)("cash").ToString <> "" Then sumCashCredit = dtSumQuan.Rows(0)("cash").ToString
                If dtSumQuan.Rows(0)("quan96").ToString <> "" Then sumquan96 = dtSumQuan.Rows(0)("quan96").ToString
                If dtSumQuan.Rows(0)("quan99").ToString <> "" Then sumquan99 = dtSumQuan.Rows(0)("quan99").ToString
                If dtSumQuan.Rows(0)("amount96").ToString <> "" Then sumAmount96 = dtSumQuan.Rows(0)("amount96").ToString
                If dtSumQuan.Rows(0)("amount99").ToString <> "" Then sumAmount99 = dtSumQuan.Rows(0)("amount99").ToString
                If dtSumQuan.Rows(0)("gold96").ToString <> "" Then sumAsset96 = dtSumQuan.Rows(0)("gold96").ToString
                If dtSumQuan.Rows(0)("gold99").ToString <> "" Then sumAsset99 = dtSumQuan.Rows(0)("gold99").ToString



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
                    If margin_base = 0 Then '
                        Return True
                    End If

                    If cpf.ExFM1 >= 0 Then
                        Return True
                    Else
                        'ถ้า ติดลบน้อยลงกว่าเดิมยอมให้ trade ได้ 
                        If cpf.Ma1 >= cpf_pre.Ma1 Then
                            Return True
                        End If
                    End If
                End If
            End If

            Return False
        Catch ex As Exception
            Throw ex
        End Try
    End Function

#End Region

    Public Shared Function customer_credit(ByVal credit As Double, ByVal amount As Double, ByVal margin As Double, ByVal paid As Double) As Double
        If amount = 0 Then 'no deal ticket from this customer
            Return credit
        End If

        If paid = 0 Then 'no pay from this customer
            Return credit - (amount * (margin / 100))
        Else
            Return paid * (margin / 100)
        End If
    End Function
    Public Shared Function customer_profitLoss(ByVal amount As Double, ByVal quantity As Double, ByVal price_now As Double) As Double
        Return ((quantity * price_now) - amount)
    End Function
    Public Shared Function customer_cashBalance(ByVal credit As Double, ByVal amount As Double, ByVal quantity As Double, ByVal price_now As Double) As Double
        Return ((quantity * price_now) - amount) + credit
    End Function

    Public Shared Function checkMaxTradePercent(ByVal bid As Double, ByVal newBid As Double, ByVal per As Double) As Boolean
        Dim result As Double = (bid * (per / 100))

        If newBid = bid Then Return True
        If newBid > bid Then
            If newBid > (bid + result) Then
                Return False
            End If
        Else
            If newBid < (bid - result) Then
                Return False
            End If
        End If
        Return True
    End Function
    Public Shared Function convert2zeroDecimal(ByVal str As String) As String
        Try
            If str.Trim = "" Then
                Return 0
            Else
                Dim strFormat As String = "#,##0"
                Return Double.Parse(str).ToString(strFormat)
            End If
        Catch ex As Exception
            'Return 0
            Throw New Exception()
        End Try
    End Function

    Public Shared Function convert2zero(ByVal str As String) As Double
        Try
            If str.Trim = "" Then
                Return 0
            Else
                Return Double.Parse(str)
            End If
        Catch ex As Exception
            'Return 0
            Throw New Exception()
        End Try
    End Function

    Public Shared Function convert2zeroPlus(ByVal str As String) As Double
        Try
            If str.Trim = "" Then
                Return 0
            Else
                Dim value As Double = 0
                value = Double.Parse(str)
                If value < 0 Then
                    value = value * -1
                End If
                Return value
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function convert2Summary(ByVal str As String) As String
        Try
            If IsNumeric(str) = False Then
                Return str
            Else
                Dim strFormat As String = "#,##0"
                If str.Split(".").Length > 1 Then
                    If str.Split(".")(1).Length = 2 Then
                        strFormat = "#,##0.00"
                    ElseIf str.Split(".")(1).Length = 5 Then
                        strFormat = "#,##0.00000"
                    End If
                End If
                Return Double.Parse(str).ToString(strFormat)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function convert2Currency(ByVal str As String) As String
        Try
            If str.Trim = "" Then
                Return ""
            Else
                Return Double.Parse(str).ToString("#,##0")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function convert2Quantity(ByVal str As String) As String
        Try
            If str.Trim = "" Then
                Return ""
            Else
                Return Double.Parse(str).ToString("#,##0")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function convert2QuantityGram(ByVal str As String) As String
        Try
            If str.Trim = "" Then
                Return ""
            Else
                Return Double.Parse(str).ToString("#,##0.000")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Function convert2Price(ByVal str As String) As String
        Try
            If str.Trim = "" Then
                Return ""
            Else
                Return Double.Parse(str).ToString("###0")
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Function
    Public Shared Function convert2StringNormal(ByVal str As String) As String
        Try
            Dim result As String = ""
            If str.Trim = "" Then
                Return ""
            Else
                result = Double.Parse(str).ToString("###0.000")
                If result.Split(".")(1) = "000" Then
                    result = result.Split(".")(0)
                End If
                Return result
            End If
        Catch ex As Exception
            Return str
        End Try
    End Function

    Public Class PhotoClass
        Public Image As String
        Public Size As Size
        Public Type As String
        Public Path As String
    End Class

    Public Shared Function IsImage(ByVal FileName As String) As Boolean
        Select Case GetFileExtension(FileName).ToLower
            Case "jpg", "jpeg", "gif", "png", "tif", "tiff"
                Return True
            Case Else
                Return False
        End Select
    End Function

    Public Shared Function IsPDF(ByVal FileName As String) As Boolean
        Select Case GetFileExtension(FileName).ToLower
            Case "pdf"
                Return True
            Case Else
                Return False
        End Select
    End Function

    Public Shared Function GetFileExtension(ByVal FileName As String) As String
        Return FileName.Substring(InStrRev(FileName, "."), Len(FileName) - InStrRev(FileName, ".")).ToUpper
    End Function


    Public Shared Function getItemListSql(ByVal cbl As CheckBoxList) As String
        Try
            Dim result As String = ""
            For i As Integer = 0 To cbl.Items.Count - 1
                If cbl.Items(i).Selected = True Then
                    If result = "" Then
                        result = cbl.Items(i).Value
                    Else
                        result += "','" + cbl.Items(i).Value
                    End If
                End If
            Next
            Return result
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    Public Shared Sub getRadioList(ByVal ddl As CheckBoxList, ByVal dt As DataTable, Optional ByVal word As String = "", Optional ByVal IsCheck As Boolean = True)
        Try
            If dt IsNot Nothing Then
                ddl.DataSource = dt
                ddl.DataValueField = dt.Columns(0).ColumnName
                ddl.DataTextField = dt.Columns(1).ColumnName
                ddl.DataBind()
                If Not word = "" Then ddl.Items.Insert(0, word)
                If IsCheck Then
                    For index As Integer = 0 To ddl.Items.Count - 1
                        ddl.Items(index).Selected = IsCheck
                    Next
                End If
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Sub getDropDownlist(ByVal ddl As DropDownList, ByVal dt As DataTable, Optional ByVal word As String = "")
        Try
            If dt IsNot Nothing Then
                ddl.DataSource = dt
                ddl.DataValueField = dt.Columns(0).ColumnName
                ddl.DataTextField = dt.Columns(1).ColumnName
                ddl.DataBind()
                If Not word = "" Then ddl.Items.Insert(0, word)
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Sub getDropDownlistValue(ByVal ddl As DropDownList, ByVal dt As DataTable, Optional ByVal word As String = "")
        Try
            If dt IsNot Nothing Then
                ddl.DataSource = dt
                ddl.DataValueField = dt.Columns(0).ColumnName
                ddl.DataTextField = dt.Columns(1).ColumnName
                ddl.DataBind()
                If Not word = "" Then ddl.Items.Insert(0, New ListItem(word, ""))
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Sub getrdoList(ByVal rdo As RadioButtonList, ByVal dt As DataTable)
        Try
            If dt IsNot Nothing Then
                rdo.DataSource = dt
                rdo.DataValueField = dt.Columns(0).ColumnName
                rdo.DataTextField = dt.Columns(1).ColumnName
                rdo.DataBind()
            End If
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Sub Script(ByVal page As Page, ByVal script As String, Optional ByVal key As String = "")
        Dim strScript As String = "<script language='javascript'>" & script & "</script>"
        ScriptManager.RegisterStartupScript(page, page.GetType(), key, strScript, False)
    End Sub

    Public Shared Sub ScriptInclude(ByVal page As Page, ByVal url As String, Optional ByVal key As String = "")
        ScriptManager.RegisterClientScriptInclude(page, page.GetType(), key, url)
    End Sub

    Public Shared Sub focusText(ByVal page As Page, ByVal clientId As String)
        Dim script As New StringBuilder

        script.Append("<script language='javascript'>")
        script.Append("var currentTextBox = document.getElementById('" + clientId + "');")
        script.Append("currentTextBox.focus();")
        script.Append("if (currentTextBox.createTextRange) {")
        script.Append("var range = currentTextBox.createTextRange();")
        script.Append("range.move('character', currentTextBox.value.length);")
        script.Append("range.select();}")
        script.Append("</script>")

        ScriptManager.RegisterStartupScript(page, page.GetType(), "focus_key", script.ToString, False)
    End Sub

    Public Shared Sub alert(ByVal page As Page, ByVal msg As String, Optional ByVal clientId As String = "", Optional ByVal url As String = "", Optional ByVal key As String = "")
        Dim script_alert As String = ""
        If url = "" And clientId = "" Then
            script_alert = "<script language='javascript'>alert('{0}');</script>"
        ElseIf Not url = "" And clientId = "" Then
            script_alert = "<script language='javascript'>alert('{0}'); window.location='" & url & "'</script>"
        ElseIf Not clientId = "" And url = "" Then
            script_alert = "<script language='javascript'>alert('{0}'); document.getElementById('" & clientId & "').focus();</script>"
        End If
        msg = msg.Replace("'", "\")
        msg = msg.Replace("\n", "\\n")
        msg = msg.Replace("|n", "\n")
        msg = msg.Replace("\r", "\\r")
        ScriptManager.RegisterStartupScript(page, page.GetType(), key, String.Format(script_alert, msg), False)
    End Sub

    Public Shared Function checkSingleQuote(ByVal str As String) As String
        Return str.Replace("'", "''")
    End Function

    Public Shared Function checkComma(ByVal str As String) As String
        Return str.Replace(",", "")
    End Function

    Public Shared Sub setTxtReadOnly(ByVal bool As Boolean, ByVal ParamArray objtext() As TextBox)
        For i As Integer = 0 To objtext.Length - 1
            objtext(i).ReadOnly = bool
        Next
    End Sub
    Public Shared Sub setDDLIndex(ByVal ParamArray objtext() As DropDownList)
        For i As Integer = 0 To objtext.Length - 1
            objtext(i).SelectedIndex = 0
        Next
    End Sub
    Public Shared Sub setTxtEmply(ByVal ParamArray objtext() As TextBox)
        For i As Integer = 0 To objtext.Length - 1
            objtext(i).Text = String.Empty
        Next
    End Sub
    Public Shared Sub setTxtEmply(ByVal ParamArray objtext() As HtmlInputText)
        For i As Integer = 0 To objtext.Length - 1
            objtext(i).Value = String.Empty
        Next
    End Sub
    Public Shared Function ctypeImage(ByVal Filename As String) As Byte()
        Dim fs As FileStream = New FileStream(Filename, FileMode.OpenOrCreate, FileAccess.Read)
        Dim rawData() As Byte = New Byte(fs.Length) {}
        fs.Read(rawData, 0, System.Convert.ToInt32(fs.Length))
        fs.Close()
        Return rawData
    End Function
    Public Shared Sub convertTopic(ByVal pic As Byte(), ByVal filename As String)
        Try
            Dim fs As FileStream = New FileStream(filename, FileMode.OpenOrCreate, FileAccess.Write)
            Dim ArraySize As Integer = New Integer
            ArraySize = pic.GetUpperBound(0)
            fs.Write(pic, 0, ArraySize + 1)
            fs.Close()
            fs.Dispose()
        Catch ex As Exception
            Throw ex
        End Try
    End Sub
    Public Shared Function check_blank(ByVal oCell As String) As String
        If oCell = "&nbsp;" Then
            oCell = " "
        End If
        Return oCell
    End Function

    Public Shared Sub Export(ByVal dt As DataTable, ByVal FileName As String)
        HttpContext.Current.Response.ClearContent()
        HttpContext.Current.Response.Charset = ""
        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.UTF8
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" & FileName & ".xls")
        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"

        Dim StringWrite As New System.IO.StringWriter
        Dim HtmlWrite As New System.Web.UI.HtmlTextWriter(StringWrite)


        Dim grd_export As New GridView
        grd_export.GridLines = GridLines.Both

        grd_export.DataSource = dt
        grd_export.DataBind()

        grd_export.RenderControl(HtmlWrite)
        HttpContext.Current.Response.Write(StringWrite.ToString())
        HttpContext.Current.Response.End()

        grd_export.Dispose()
        dt.Dispose()

    End Sub

    Private Shared Sub PrepareGridViewForExport(ByVal gv As Control)
        Try
            Dim lb As New LinkButton()
            Dim l As New Literal()
            Dim name As String = [String].Empty
            For i As Integer = 0 To gv.Controls.Count - 1
                If gv.Controls(i).[GetType]() = GetType(LinkButton) Then
                    l.Text = TryCast(gv.Controls(i), LinkButton).Text
                    gv.Controls.Remove(gv.Controls(i))
                    gv.Controls.AddAt(i, l)
                ElseIf gv.Controls(i).[GetType]() = GetType(DropDownList) Then
                    l.Text = TryCast(gv.Controls(i), DropDownList).SelectedItem.Text
                    gv.Controls.Remove(gv.Controls(i))
                    gv.Controls.AddAt(i, l)
                ElseIf gv.Controls(i).[GetType]() = GetType(CheckBox) Then
                    l.Text = If(TryCast(gv.Controls(i), CheckBox).Checked, "True", "False")
                    gv.Controls.Remove(gv.Controls(i))
                    gv.Controls.AddAt(i, l)
                ElseIf gv.Controls(i).[GetType]() = GetType(ImageButton) Then
                    l.Text = ""
                    gv.Controls.Remove(gv.Controls(i))
                    gv.Controls.AddAt(i, l)
                End If
                If gv.Controls(i).HasControls() Then
                    PrepareGridViewForExport(gv.Controls(i))
                End If
            Next
        Catch ex As Exception
            Throw ex
        End Try
    End Sub

    Public Shared Sub ExportToExcelTradeOrder(ByVal gv As GridView, ByVal FileName As String)
        Try
            PrepareGridViewForExport(gv)
            Dim sw As New StringWriter()
            Dim htw As New HtmlTextWriter(sw)
            'Dim frm As New HtmlForm
            'frm.Attributes("runat") = "server"
            'frm.Controls.Add(gv)
            gv.RenderControl(htw)

            HttpContext.Current.Response.ClearContent()
            HttpContext.Current.Response.Charset = ""
            HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Unicode
            HttpContext.Current.Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble())
            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" & FileName & ".xls")
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
            HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"
            HttpContext.Current.Response.Write(sw.ToString)
            HttpContext.Current.Response.End()

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Shared Sub ExportToExcel(ByVal dt As DataTable, ByVal FileName As String, ByVal htSum As Hashtable)
        HttpContext.Current.Response.ClearContent()
        HttpContext.Current.Response.Charset = ""

        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Unicode
        HttpContext.Current.Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble())
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" & FileName & ".xls")
        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"

        Dim StringWrite As New System.IO.StringWriter
        Dim HtmlWrite As New System.Web.UI.HtmlTextWriter(StringWrite)

        Dim htmlHead As String = ""
        htmlHead += "<div>"
        htmlHead += "<table cellspacing='0' rules='all' border='1' style='border-collapse:collapse;font-family: Tahoma'>"
        htmlHead += "<tr>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=2 colSpan=1 align=middle>Datetime</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=2 colSpan=1 align=middle>ref_No</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=2 colSpan=1 align=middle>book_No</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=2 colSpan=1 align=middle>No</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=2 colSpan=1 align=middle>Customer Name</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=2 colSpan=1 align=middle>Pure</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=3 align=middle>Buy</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=3 align=middle>Sell</TD></TR>"
        htmlHead += "<TR>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>Price</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>Quan</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>Amount</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>Price</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>Quan</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>Amount</TD></TR>"

        Dim htmlFoot As String = ""
        htmlFoot += "<TR>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=2 colSpan=5 align=middle>Total</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>&nbsp;96.50&nbsp;</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>{8}</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>{0}</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>{1}</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>{9}</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>{2}</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>{3}</TD></TR>"
        htmlFoot += "<TR>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>&nbsp;99.99&nbsp;</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle> {10}</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle> {4} </TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle> {5} </TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle> {11} </TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle> {6} </TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle> {7} </TD>"
        htmlFoot += "</TR>"
        htmlFoot += "</table>"
        htmlFoot += "</div>"
        htmlFoot = String.Format(htmlFoot, clsManage.convert2Quantity(htSum("qb96")), convert2Currency(htSum("ab96")), _
                                 convert2Quantity(htSum("qs96")), convert2Currency(htSum("as96")), _
                                 convert2Quantity(htSum("qb99")), convert2Currency(htSum("ab99")), _
                                 convert2Quantity(htSum("qs99")), convert2Currency(htSum("as99")), _
                                 convert2Quantity(htSum("avb96")), convert2Currency(htSum("avs96")), _
                                 convert2Quantity(htSum("avb99")), convert2Currency(htSum("avs99"))
                                 )
        Dim htmlBody As String = ""

        Dim rowBody As String = ""
        rowBody += "<TR style='BACKGROUND-COLOR: #eeeeee; COLOR: black'>"
        rowBody += "<TD align=middle>&nbsp;{0}&nbsp;</TD>"
        rowBody += "<TD align=middle>&nbsp;{1}&nbsp;</TD>"
        rowBody += "<TD align=middle>&nbsp;{2}&nbsp;</TD>"
        rowBody += "<TD align=middle>&nbsp;{3}&nbsp;</TD>"
        rowBody += "<TD>{4}</TD>"
        rowBody += "<TD align=middle>&nbsp;{5}&nbsp;</TD>"
        rowBody += "<TD align=right> {6} </TD>"
        rowBody += "<TD align=right> {7} </TD>"
        rowBody += "<TD align=right> {8} </TD>"
        rowBody += "<TD align=right> {9} </TD>"
        rowBody += "<TD align=right> {10} </TD>"
        rowBody += "<TD align=right> {11} </TD></TR>"

        For Each dr As Data.DataRow In dt.Rows

            htmlBody += String.Format(rowBody, DateTime.Parse(dr("ticket_date")).ToString(clsManage.formatDateTime), dr("ticket_id").ToString, _
                                      dr("book_no").ToString, dr("run_no").ToString, dr("firstname").ToString, dr("gold_type_name").ToString, _
                                      clsManage.convert2Currency(dr("buy_price")), clsManage.convert2Quantity(dr("buy_quantity")), clsManage.convert2Currency(dr("buy_amount")), _
                                      clsManage.convert2Currency(dr("sell_price")), clsManage.convert2Quantity(dr("sell_quantity")), clsManage.convert2Currency(dr("sell_amount")))
        Next
        HttpContext.Current.Response.Write(htmlHead + htmlBody + htmlFoot)
        HttpContext.Current.Response.End()
    End Sub

    Public Shared Sub ExportSummaryOrderToPDF(ByVal dt As DataTable, ByVal FileName As String, ByVal htSum As Hashtable, path As String)
        Try

            Dim htmlHead As String = ""
            htmlHead += "<div>"
            htmlHead += "<table cellspacing='0' rules='all' border='.3' style='border-collapse:collapse;font-family: Tahoma;font-size:6;'>"
            htmlHead += "<tr>"
            htmlHead += "<TD style='BACKGROUND-COLOR: #376091' colSpan=6></TD>"
            htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold ;text-align:center' colSpan=3>Buy</TD>"
            htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold ;text-align:center' colSpan=3>Sell</TD></TR>"
            htmlHead += "<tr>"
            htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:center' colSpan=1>Datetime</TD>"
            htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:center' colSpan=2>ref_No</TD>"
            htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:center' colSpan=2>Customer_Name</TD>"
            htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:center' colSpan=1>Pure</TD>"
            htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:center' colSpan=1>Price</TD>"
            htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:center' colSpan=1>Quan</TD>"
            htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:center' colSpan=1>Amount</TD>"
            htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:center' colSpan=1>Price</TD>"
            htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:center' colSpan=1>Quan</TD>"
            htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:center' colSpan=1>Amount</TD></TR>"

            Dim htmlFoot As String = ""
            htmlFoot += "<TR>"
            htmlFoot += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold; text-align:center' colSpan=5>Total 96.50</TD>"
            htmlFoot += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold; text-align:center' colSpan=1>&nbsp;96.50&nbsp;</TD>"
            htmlFoot += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:right' colSpan=1>{8}</TD>"
            htmlFoot += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:right' colSpan=1>{0}</TD>"
            htmlFoot += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:right' colSpan=1>{1}</TD>"
            htmlFoot += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:right' colSpan=1>{9}</TD>"
            htmlFoot += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:right' colSpan=1>{2}</TD>"
            htmlFoot += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:right' colSpan=1>{3}</TD></TR>"
            htmlFoot += "<TR>"
            htmlFoot += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold; text-align:center' colSpan=5>Total 99.99</TD>"
            htmlFoot += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:center' colSpan=1>&nbsp;99.99&nbsp;</TD>"
            htmlFoot += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:right' colSpan=1> {10}</TD>"
            htmlFoot += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:right' colSpan=1> {4} </TD>"
            htmlFoot += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:right' colSpan=1> {5} </TD>"
            htmlFoot += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:right' colSpan=1> {11} </TD>"
            htmlFoot += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:right' colSpan=1> {6} </TD>"
            htmlFoot += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:right' colSpan=1> {7} </TD></TR>"
            htmlFoot += "</table>"
            htmlFoot += "</div>"

            htmlFoot = String.Format(htmlFoot, clsManage.convert2Quantity(htSum("qb96")), convert2Currency(htSum("ab96")), _
                                               convert2Quantity(htSum("qs96")), convert2Currency(htSum("as96")), _
                                               convert2Quantity(htSum("qb99")), convert2Currency(htSum("ab99")), _
                                               convert2Quantity(htSum("qs99")), convert2Currency(htSum("as99")), _
                                               convert2Quantity(htSum("avb96")), convert2Currency(htSum("avs96")), _
                                               convert2Quantity(htSum("avb99")), convert2Currency(htSum("avs99"))
                                               )


            Dim htmlBody As String = ""
            Dim rowBody As String = ""
            rowBody += "<TR style='BACKGROUND-COLOR: #eeeeee; COLOR: black'>"
            rowBody += "<TD>&nbsp;{0}&nbsp;</TD>"
            rowBody += "<TD colSpan=2>&nbsp;{1}&nbsp;</TD>"
            rowBody += "<TD colSpan=2>{2}</TD>"
            rowBody += "<TD align=center>&nbsp;{3}&nbsp;</TD>"
            rowBody += "<TD align=right> {4} </TD>"
            rowBody += "<TD align=right> {5} </TD>"
            rowBody += "<TD align=right> {6} </TD>"
            rowBody += "<TD align=right> {7} </TD>"
            rowBody += "<TD align=right> {8} </TD>"
            rowBody += "<TD align=right> {9} </TD></TR>"

            For Each dr As Data.DataRow In dt.Rows
                htmlBody += String.Format(rowBody, DateTime.Parse(dr("ticket_date")).ToString(clsManage.formatDateTime), dr("ticket_id").ToString, _
                                          dr("firstname").ToString, dr("gold_type_name").ToString, _
                                          clsManage.convert2Currency(dr("buy_price")), clsManage.convert2Quantity(dr("buy_quantity")), clsManage.convert2Currency(dr("buy_amount")), _
                                          clsManage.convert2Currency(dr("sell_price")), clsManage.convert2Quantity(dr("sell_quantity")), clsManage.convert2Currency(dr("sell_amount")))

            Next

            Dim hp As New HtmlToPdf(htmlHead + htmlBody + htmlFoot, path + "\font\TAHOMA.TTF", 10)
            hp.Render(HttpContext.Current.Response, FileName + ".pdf", True)

            'Dim sr As New System.IO.StringReader(htmlHead + htmlBody + htmlFoot)
            'Dim pdfDoc As New iTextSharp.text.Document(iTextSharp.text.PageSize.A4, 7.0F, 7.0F, 7.0F, 0.0F)

            'Dim htmlparser As New iTextSharp.text.html.simpleparser.HTMLWorker(pdfDoc)
            'iTextSharp.text.pdf.PdfWriter.GetInstance(pdfDoc, HttpContext.Current.Response.OutputStream)
            'pdfDoc.Open()

            ''BaseFont EnCodefont = BaseFont.CreateFont(Server.MapPath("../font/ANGSA.TTF"), BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
            ''Font Nfont= new Font(EnCodefont, 18, Font.NORMAL, new Color(0, 0, 255));
            ''C:/Windows/Fonts/ARIALUNI.TTF
            'Dim encodefont As iTextSharp.text.pdf.BaseFont = iTextSharp.text.pdf.BaseFont.CreateFont(path + "\font\TAHOMA.TTF", iTextSharp.text.pdf.BaseFont.IDENTITY_H, iTextSharp.text.pdf.BaseFont.EMBEDDED)
            'Dim NFont As New iTextSharp.text.Font(encodefont, 6, iTextSharp.text.Font.NORMAL, New iTextSharp.text.Color(0, 0, 0))

            'Dim table As iTextSharp.text.Table = New iTextSharp.text.Table(10)
            'table.BorderWidth = 1 : table.Padding = 3 : table.Spacing = 1

            'Dim cell As iTextSharp.text.Cell = New iTextSharp.text.Cell(New iTextSharp.text.Phrase("วันที่", NFont))
            'cell.Header = True
            'cell.Rowspan = 2
            'cell.BackgroundColor = New iTextSharp.text.Color(39, 75, 152)
            'cell.BorderWidth = True
            'cell.Width = 250
            'table.AddCell(cell)

            'table.AddCell("Customer Name")
            'table.AddCell("Pure")
            'table.AddCell("Buy")

            'table.AddCell("Sell")
            'cell = New iTextSharp.text.Cell("big cell")
            'cell.Rowspan = 2
            'cell.Colspan = 2
            'cell.HorizontalAlignment = iTextSharp.text.Element.ALIGN_CENTER
            'cell.VerticalAlignment = iTextSharp.text.Element.ALIGN_MIDDLE
            'cell.BackgroundColor = New iTextSharp.text.Color(192, 192, 192)
            'table.AddCell(cell)

            'pdfDoc.Add(New iTextSharp.text.Paragraph("เครื่องบิน เครื้องบิ้น", NFont))
            'pdfDoc.Add(table)

            'htmlparser.Parse(sr)

            'pdfDoc.Close()
            'HttpContext.Current.Response.Write(pdfDoc)
            'HttpContext.Current.Response.End()

        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Shared Sub ExportSummaryOrderOrnamentToPDF(ByVal dt As DataTable, ByVal FileName As String, ByVal htSum As Hashtable, path As String)
        Try

            Dim htmlHead As String = ""
            htmlHead += "<div>"
            htmlHead += "<table cellspacing='0' rules='all' border='.3' style='border-collapse:collapse;font-family: Tahoma;font-size:6;'>"
            htmlHead += "<tr>"
            htmlHead += "<TD style='BACKGROUND-COLOR: #376091' colSpan=5></TD>"
            htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold ;text-align:center' colSpan=3>Buy</TD>"
            htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold ;text-align:center' colSpan=3>Sell</TD></TR>"
            htmlHead += "<tr>"
            htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:center' colSpan=1>Datetime</TD>"
            htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:center' colSpan=1>ref_No</TD>"
            htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:center; width:600px' colSpan=2>Customer_Name</TD>"
            htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:center' colSpan=1>Pure</TD>"
            htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:center' colSpan=1>Price</TD>"
            htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:center' colSpan=1>Quan</TD>"
            htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:center' colSpan=1>Amount</TD>"
            htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:center' colSpan=1>Price</TD>"
            htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:center' colSpan=1>Quan</TD>"
            htmlHead += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:center' colSpan=1>Amount</TD></TR>"

            Dim htmlFoot As String = ""
            htmlFoot += "<TR>"
            htmlFoot += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold; text-align:center' colSpan=4>Total</TD>"
            htmlFoot += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold; text-align:center' colSpan=1>&nbsp;96.50(กรัม)&nbsp;</TD>"
            htmlFoot += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:right' colSpan=1>{8}</TD>"
            htmlFoot += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:right' colSpan=1>{0}</TD>"
            htmlFoot += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:right' colSpan=1>{1}</TD>"
            htmlFoot += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:right' colSpan=1>{9}</TD>"
            htmlFoot += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:right' colSpan=1>{2}</TD>"
            htmlFoot += "<TD style='BACKGROUND-COLOR: #376091; COLOR: white; FONT-WEIGHT: bold;text-align:right' colSpan=1>{3}</TD></TR>"

            htmlFoot += "</table>"
            htmlFoot += "</div>"

            htmlFoot = String.Format(htmlFoot, clsManage.convert2QuantityGram(htSum("qb96")), convert2Currency(htSum("ab96")), _
                                               convert2QuantityGram(htSum("qs96")), convert2Currency(htSum("as96")), _
                                               "", "", "", "", _
                                               convert2Currency(htSum("avb96")), convert2Currency(htSum("avs96")), _
                                               "", ""
                                               )

            Dim htmlBody As String = ""

            Dim rowBody As String = ""
            rowBody += "<TR style='BACKGROUND-COLOR: #eeeeee; COLOR: black'>"
            rowBody += "<TD>&nbsp;{0}&nbsp;</TD>"
            rowBody += "<TD>&nbsp;{1}&nbsp;</TD>"
            rowBody += "<TD colSpan=2>{2}</TD>"
            rowBody += "<TD align=center>&nbsp;{3}&nbsp;</TD>"
            rowBody += "<TD align=right> {4} </TD>"
            rowBody += "<TD align=right> {5} </TD>"
            rowBody += "<TD align=right> {6} </TD>"
            rowBody += "<TD align=right> {7} </TD>"
            rowBody += "<TD align=right> {8} </TD>"
            rowBody += "<TD align=right> {9} </TD></TR>"

            For Each dr As Data.DataRow In dt.Rows

                htmlBody += String.Format(rowBody, DateTime.Parse(dr("ticket_date")).ToString(clsManage.formatDateTime), dr("ticket_id").ToString, _
                                          dr("firstname").ToString, "96.50(กรัม)", _
                                          clsManage.convert2Currency(dr("buy_price")), clsManage.convert2QuantityGram(dr("buy_quantity")), clsManage.convert2Currency(dr("buy_amount")), _
                                          clsManage.convert2Currency(dr("sell_price")), clsManage.convert2QuantityGram(dr("sell_quantity")), clsManage.convert2Currency(dr("sell_amount")))

            Next

            Dim hp As New HtmlToPdf(htmlHead + htmlBody + htmlFoot, path + "\font\TAHOMA.TTF", 6)
            hp.Render(HttpContext.Current.Response, FileName + ".pdf", True)


        Catch ex As Exception
            Throw ex
        End Try

    End Sub

    Public Shared Sub ExportToExcelStock(ByVal dt As DataTable, ByVal FileName As String, ByVal htSum As Hashtable)

        HttpContext.Current.Response.ClearContent()
        HttpContext.Current.Response.Charset = ""

        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Unicode
        HttpContext.Current.Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble())
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" & FileName & ".xls")
        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"

        Dim StringWrite As New System.IO.StringWriter
        Dim HtmlWrite As New System.Web.UI.HtmlTextWriter(StringWrite)

        Dim htmlHead As String = ""
        htmlHead += "<div>"
        htmlHead += "<table cellspacing='0' rules='all' border='1' style='border-collapse:collapse;'>"
        htmlHead += "<tr>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=2 colSpan=1 align=middle></TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=2 colSpan=1 align=middle>Datetime</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=2 colSpan=1 align=middle>Ref No</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=2 colSpan=1 align=middle>Customer Name</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=2 colSpan=1 align=middle>Price</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=2 align=middle>Sell</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=2 align=middle>Buy</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=2 colSpan=1 align=middle>Amount</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=3 align=middle>Receipt</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=3 align=middle>Pay</TD></TR>"
        htmlHead += "<TR>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' align=middle>99.99</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' align=middle>96.5</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' align=middle>99.99</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' align=middle>96.5</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' align=middle>Cash</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' align=middle>Trans</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' align=middle>Cheq</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' align=middle>Cash</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' align=middle>Trans</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold'align=middle>Cheq</TD></TR>"

        Dim htmlFoot As String = ""
        htmlFoot += "<TR>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=5 align=middle>Summary</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=right>{0}</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=right>{1}</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=right>{2}</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=right>{3}</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=right>{4}</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=right>{5}</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=right>{6}</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=right>{7}</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=right>{8}</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=right>{9}</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=right>{10}</TD>"
        htmlFoot += "</TR>"
        htmlFoot += "</table>"
        htmlFoot += "</div>"
        htmlFoot = String.Format(htmlFoot, clsManage.convert2Quantity(htSum("sell_99")), convert2Quantity(htSum("sell_96")), _
                                 convert2Quantity(htSum("buy_99")), convert2Quantity(htSum("buy_96")), convert2Currency(htSum("amount")), _
                                 convert2Currency(htSum("sell_cash")), convert2Currency(htSum("sell_trans")), convert2Currency(htSum("sell_cheq")), _
                                 convert2Currency(htSum("buy_cash")), convert2Currency(htSum("buy_trans")), convert2Currency(htSum("buy_cheq")))
        Dim htmlBody As String = ""

        Dim rowBody As String = ""
        rowBody += "<TR style='BACKGROUND-COLOR: #eeeeee; COLOR: black'>"
        rowBody += "<TD align=middle>&nbsp;{0}&nbsp;</TD>"
        rowBody += "<TD align=middle>&nbsp;{1}&nbsp;</TD>"
        rowBody += "<TD align=middle>&nbsp;{2}&nbsp;</TD>"
        rowBody += "<TD align=left>&nbsp;{3}&nbsp;</TD>"
        rowBody += "<TD align=right> {4} </TD>"
        rowBody += "<TD align=right> {5} </TD>"
        rowBody += "<TD align=right> {6} </TD>"
        rowBody += "<TD align=right> {7} </TD>"
        rowBody += "<TD align=right> {8} </TD>"
        rowBody += "<TD align=right> {9} </TD>"
        rowBody += "<TD align=right> {10} </TD>"
        rowBody += "<TD align=right> {11} </TD>"
        rowBody += "<TD align=right> {12} </TD>"
        rowBody += "<TD align=right> {13} </TD>"
        rowBody += "<TD align=right> {14} </TD>"
        rowBody += "<TD align=right> {15} </TD></TR>"

        For Each dr As Data.DataRow In dt.Rows
            htmlBody += String.Format(rowBody, dt.Rows.IndexOf(dr) + 1, DateTime.Parse(dr("ticket_date")).ToString(clsManage.formatDateTime), dr("ticket_id").ToString, _
                                      dr("firstname").ToString, clsManage.convert2Currency(dr("price")), _
                                      clsManage.convert2Quantity(dr("sell_99")), clsManage.convert2Quantity(dr("sell_96")), clsManage.convert2Quantity(dr("buy_99")), clsManage.convert2Quantity(dr("buy_96")), _
                                       clsManage.convert2Currency(dr("amount")), _
                                      clsManage.convert2Currency(dr("sell_cash")), clsManage.convert2Currency(dr("sell_trans")), clsManage.convert2Currency(dr("sell_cheq")), _
                                      clsManage.convert2Currency(dr("buy_cash")), clsManage.convert2Currency(dr("buy_trans")), clsManage.convert2Currency(dr("buy_cheq")))
        Next

        HttpContext.Current.Response.Write(htmlHead + htmlBody + htmlFoot)
        HttpContext.Current.Response.End()
    End Sub

    Public Shared Sub stockTicketsExportToExcel(ByVal dt As DataTable, ByVal FileName As String, ByVal htSum As Hashtable)
        HttpContext.Current.Response.ClearContent()
        HttpContext.Current.Response.Charset = ""

        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Unicode
        HttpContext.Current.Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble())
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" & FileName & ".xls")
        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"

        Dim StringWrite As New System.IO.StringWriter
        Dim HtmlWrite As New System.Web.UI.HtmlTextWriter(StringWrite)

        Dim htmlHead As String = ""
        htmlHead += "<div>"
        htmlHead += "<table cellspacing='0' rules='all' border='1' style='border-collapse:collapse;font-family: Tahoma'>"
        htmlHead += "<tr>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>Date</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>ticket_ref</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>Customer_name</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>Type</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>Payment</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>Delivery_date</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>Create_by</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>Q96(กรัม)</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>Q96(บาท)</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>Q99(kg)</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>Price(baht)</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>Amount(baht)</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>status</TD>"

        Dim htmlFoot As String = ""
        htmlFoot += "<TR>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=7 align=middle>Summary</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=right>{0}</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=right>{1}</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=right>{2}</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=right>{3}</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=right>{4}</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle></TD></TR>"
        htmlFoot += "<TR>"
        htmlFoot += "</table>"
        htmlFoot += "</div>"
        htmlFoot = String.Format(htmlFoot, clsManage.convert2QuantityGram(htSum("Quan96G")), clsManage.convert2Quantity(htSum("Quan96")), convert2Currency(htSum("Quan99")), _
                                  convert2Quantity(htSum("Price")), convert2Currency(htSum("Amount")))
        Dim htmlBody As String = ""

        Dim rowBody As String = ""
        rowBody += "<TR style='BACKGROUND-COLOR: #eeeeee; COLOR: black'>"
        rowBody += "<TD valign='top' align=middle>{0}</TD>"
        rowBody += "<TD valign='top' align=middle>{1}</TD>"
        rowBody += "<TD valign='top' align=left>{2}</TD>"
        rowBody += "<TD valign='top' align=middle>{3}</TD>"
        rowBody += "<TD valign='top' align=middle>{4}</TD>"
        rowBody += "<TD valign='top' align=middle>{5}</TD>"
        rowBody += "<TD valign='top' align=left>{6}</TD>"
        rowBody += "<TD valign='top' align=right>&nbsp;{7}&nbsp;</TD>"
        rowBody += "<TD valign='top' align=right>&nbsp;{8}&nbsp;</TD>"
        rowBody += "<TD valign='top' align=right>&nbsp;{9}&nbsp;</TD>"
        rowBody += "<TD valign='top' align=right>&nbsp;{10}&nbsp;</TD>"
        rowBody += "<TD valign='top' align=right>&nbsp;{11}&nbsp;</TD>"
        rowBody += "<TD valign='top' align=left>{12}</TD></TR>"
        Dim delivery_date As String = ""
        For Each dr As Data.DataRow In dt.Rows
            delivery_date = ""
            If Not IsDBNull(dr("delivery_date")) Then
                delivery_date = DateTime.Parse(dr("delivery_date")).ToString(clsManage.formatDateTime)
            End If
            htmlBody += String.Format(rowBody, DateTime.Parse(dr("ticket_date")).ToString(clsManage.formatDateTime), dr("ticket_id").ToString, _
                           dr("firstname").ToString, dr("type").ToString, dr("payment").ToString, delivery_date, _
                           dr("user_name").ToString, clsManage.convert2QuantityGram(dr("quan96G")), clsManage.convert2Currency(dr("quan96")), clsManage.convert2Currency(dr("quan99")), _
                           clsManage.convert2Currency(dr("price")), clsManage.convert2Currency(dr("amount")), dr("status_id").ToString)
        Next
        HttpContext.Current.Response.Write(htmlHead + htmlBody + htmlFoot)
        HttpContext.Current.Response.End()
    End Sub

    Public Shared Sub ExportToTicketPortfolioPurity96(ByVal dt As DataTable, ByVal FileName As String, ByVal htSum As Hashtable)
        HttpContext.Current.Response.ClearContent()
        HttpContext.Current.Response.Charset = ""

        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Unicode
        HttpContext.Current.Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble())
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" & FileName & ".xls")
        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"

        Dim StringWrite As New System.IO.StringWriter
        Dim HtmlWrite As New System.Web.UI.HtmlTextWriter(StringWrite)

        Dim htmlHead As String = ""
        htmlHead += "<div>"
        htmlHead += "<TABLE cellspacing='0'>"
        htmlHead += "<TR>"
        htmlHead += "	<TD colspan='9' style='COLOR: #274b98; FONT-WEIGHT: bold'>Customer Detail</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Ref No : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{0}</TD>"
        htmlHead += "	<TD align=right>บุคคลติดต่อ : </TD>"
        htmlHead += "	<TD colspan='3' align=left>{1}</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Name : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{2}</TD>"
        htmlHead += "	<TD align=right>Remark : </TD>"
        htmlHead += "	<TD colspan='3' align=left>{3}</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Address : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{4}</TD>"
        htmlHead += "	<TD></TD>"
        htmlHead += "	<TD colspan='3' align=left></TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Phone : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{5}</TD>"
        htmlHead += "	<TD></TD>"
        htmlHead += "	<TD colspan='3' align=left></TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Fax : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{6}</TD>"
        htmlHead += "	<TD></TD>"
        htmlHead += "	<TD colspan='3' align=left></TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Mobile Phone : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{7}</TD>"
        htmlHead += "	<TD></TD>"
        htmlHead += "	<TD colspan='3' align=left></TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Account bank : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{8}</TD>"
        htmlHead += "	<TD align=right>Branch : </TD>"
        htmlHead += "	<TD colspan='3' align=left>{9}</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Account No : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{10}</TD>"
        htmlHead += "	<TD align=right>Account Name : </TD>"
        htmlHead += "	<TD colspan='3' align=left>{11}</TD>"
        htmlHead += "</TR>"
        htmlHead += "</TABLE><br />"
        htmlHead += "<TABLE cellspacing='0'>"
        htmlHead += "<TR>"
        htmlHead += "	<TD colspan='9' style='COLOR: #274b98; FONT-WEIGHT: bold'>Customer Asset</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Cash : </TD>"
        htmlHead += "	<TD colspan='8'  align=left>{12} บาท</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Glod 96.50 : </TD>"
        htmlHead += "	<TD colspan='8'  align=left>{13} บาท</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right >Glod 99.99 : </TD>"
        htmlHead += "	<TD colspan='8'  align=left>{14} kg</TD>"
        htmlHead += "</TR>"
        htmlHead += "</TABLE><BR>"
        htmlHead += "<table cellspacing='0' rules='all' border='1' style='border-collapse:collapse;'>"
        htmlHead += "<tr>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>วันที่ซื้อขาย</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>เวลา</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>ticket_ref</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>book_no</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>No</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>คำสั่ง</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>ราคา</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>จำนวน</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>มุลค่าการซื้อขาย</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>วันชำระเงิน</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>วันส่งมอบสินค้า</TD></TR>"
        htmlHead = String.Format(htmlHead, htSum("RefNo").ToString, htSum("Person").ToString, htSum("Name").ToString, _
                         htSum("Remark").ToString, htSum("Add").ToString, htSum("Phone").ToString, htSum("Fax").ToString, _
                        htSum("MobilePhone").ToString, htSum("AccBank").ToString, htSum("Branch").ToString, _
                        htSum("AccNo").ToString, htSum("AccName").ToString, htSum("Cash").ToString, _
                        htSum("Gold96").ToString, htSum("Gold99").ToString)

        Dim htmlFoot As String = ""
        htmlFoot += "<TR>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #CFCCD0; COLOR: #000000; FONT-WEIGHT: bold' rowSpan=1 colSpan=7 align=middle>Total</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #CFCCD0; COLOR: #000000; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=right>{0}</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #CFCCD0; COLOR: #000000; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=right>{1}</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #CFCCD0; COLOR: #000000; FONT-WEIGHT: bold' rowSpan=1 colSpan=2 align=middle></TD>"
        htmlFoot += "</TR>"
        htmlFoot += "</table>"
        htmlFoot += "</div>"
        htmlFoot = String.Format(htmlFoot, clsManage.convert2Quantity(htSum("sumQuan96")), convert2Currency(htSum("sumAmount96")))


        Dim htmlBody As String = ""

        Dim rowBody As String = ""
        rowBody += "<TR style='BACKGROUND-COLOR: #eeeeee; COLOR: black'>"
        rowBody += "<TD align=middle> {0} </TD>"
        rowBody += "<TD align=middle> {1} </TD>"
        rowBody += "<TD align=middle> {2} </TD>"
        rowBody += "<TD align=middle> {3} </TD>"
        rowBody += "<TD align=middle> {4} </TD>"
        rowBody += "<TD align=middle> {5} </TD>"
        rowBody += "<TD align=right> {6} </TD>"
        rowBody += "<TD align=right> {7} </TD>"
        rowBody += "<TD align=right> {8} </TD>"
        rowBody += "<TD align=middle> {9} </TD>"
        rowBody += "<TD align=middle> {10} </TD></TR>"


        For Each dr As Data.DataRow In dt.Rows
            Dim payment_duedate As String = ""
            If Not IsDBNull(dr("payment_duedate")) Then
                payment_duedate = DateTime.Parse(dr("payment_duedate")).ToString(clsManage.formatDateTime)
            End If
            Dim delivery_date As String = ""
            If Not IsDBNull(dr("delivery_date")) Then
                delivery_date = DateTime.Parse(dr("delivery_date")).ToString(clsManage.formatDateTime)
            End If
            htmlBody += String.Format(rowBody, DateTime.Parse(dr("ticket_date")).ToString(clsManage.formatDateTime), DateTime.Parse(dr("created_date")).ToString("hh:mm tt"), dr("ticket_id").ToString, _
                                      dr("book_no").ToString, dr("run_no").ToString, dr("type").ToString, clsManage.convert2Currency(dr("price")), _
                                      clsManage.convert2Quantity(dr("quantity")), clsManage.convert2Currency(dr("amount")), _
                                     payment_duedate, delivery_date)
        Next

        HttpContext.Current.Response.Write(htmlHead + htmlBody + htmlFoot)
        HttpContext.Current.Response.End()
    End Sub
    Public Shared Sub ExportToTicketPortfolioPurity96Historical(ByVal dt As DataTable, ByVal FileName As String, ByVal htSum As Hashtable)
        HttpContext.Current.Response.ClearContent()
        HttpContext.Current.Response.Charset = ""

        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Unicode
        HttpContext.Current.Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble())
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" & FileName & ".xls")
        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"

        Dim StringWrite As New System.IO.StringWriter
        Dim HtmlWrite As New System.Web.UI.HtmlTextWriter(StringWrite)

        Dim htmlHead As String = ""
        htmlHead += "<div>"
        htmlHead += "<TABLE cellspacing='0'>"
        htmlHead += "<TR>"
        htmlHead += "	<TD colspan='9' style='COLOR: #274b98; FONT-WEIGHT: bold'>Customer Detail</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Ref No : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{0}</TD>"
        htmlHead += "	<TD align=right>บุคคลติดต่อ : </TD>"
        htmlHead += "	<TD colspan='3' align=left>{1}</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Name : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{2}</TD>"
        htmlHead += "	<TD align=right>Remark : </TD>"
        htmlHead += "	<TD colspan='3' align=left>{3}</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Address : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{4}</TD>"
        htmlHead += "	<TD></TD>"
        htmlHead += "	<TD colspan='3' align=left></TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Phone : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{5}</TD>"
        htmlHead += "	<TD></TD>"
        htmlHead += "	<TD colspan='3' align=left></TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Fax : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{6}</TD>"
        htmlHead += "	<TD></TD>"
        htmlHead += "	<TD colspan='3' align=left></TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Mobile Phone : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{7}</TD>"
        htmlHead += "	<TD></TD>"
        htmlHead += "	<TD colspan='3' align=left></TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Account bank : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{8}</TD>"
        htmlHead += "	<TD align=right>Branch : </TD>"
        htmlHead += "	<TD colspan='3' align=left>{9}</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Account No : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{10}</TD>"
        htmlHead += "	<TD align=right>Account Name : </TD>"
        htmlHead += "	<TD colspan='3' align=left>{11}</TD>"
        htmlHead += "</TR>"
        htmlHead += "</TABLE><br />"
        htmlHead += "<TABLE cellspacing='0'>"
        htmlHead += "<TR>"
        htmlHead += "	<TD colspan='9' style='COLOR: #274b98; FONT-WEIGHT: bold'>Customer Asset</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Cash : </TD>"
        htmlHead += "	<TD colspan='8'  align=left>{12} บาท</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Glod 96.50 : </TD>"
        htmlHead += "	<TD colspan='8'  align=left>{13} บาท</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right >Glod 99.99 : </TD>"
        htmlHead += "	<TD colspan='8'  align=left>{14} kg</TD>"
        htmlHead += "</TR>"
        htmlHead += "</TABLE><BR>"
        htmlHead += "<table cellspacing='0' rules='all' border='1' style='border-collapse:collapse;'>"
        htmlHead += "<tr>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>วันที่ซื้อขาย</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>เวลา</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>ticket_ref</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>book_no</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>No</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>คำสั่ง</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>ราคา</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>จำนวน</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>มุลค่าการซื้อขาย</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>วันชำระเงิน</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>วันส่งมอบสินค้า</TD></TR>"
        htmlHead = String.Format(htmlHead, htSum("RefNo").ToString, htSum("Person").ToString, htSum("Name").ToString, _
                         htSum("Remark").ToString, htSum("Add").ToString, htSum("Phone").ToString, htSum("Fax").ToString, _
                        htSum("MobilePhone").ToString, htSum("AccBank").ToString, htSum("Branch").ToString, _
                        htSum("AccNo").ToString, htSum("AccName").ToString, htSum("Cash").ToString, _
                        htSum("Gold96").ToString, htSum("Gold99").ToString)

        Dim htmlFoot As String = ""
        htmlFoot += "<TR>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #CFCCD0; COLOR: #000000; FONT-WEIGHT: bold' rowSpan=1 colSpan=7 align=middle>Total</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #CFCCD0; COLOR: #000000; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=right>{0}</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #CFCCD0; COLOR: #000000; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=right>{1}</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #CFCCD0; COLOR: #000000; FONT-WEIGHT: bold' rowSpan=1 colSpan=2 align=middle></TD>"
        htmlFoot += "</TR>"
        htmlFoot += "</table>"
        htmlFoot += "</div>"
        htmlFoot = String.Format(htmlFoot, clsManage.convert2Quantity(htSum("sumHisQuan96")), convert2Currency(htSum("sumHisAmount96")))

        Dim htmlBody As String = ""

        Dim rowBody As String = ""
        rowBody += "<TR style='BACKGROUND-COLOR: #eeeeee; COLOR: black'>"
        rowBody += "<TD align=middle> {0} </TD>"
        rowBody += "<TD align=middle> {1} </TD>"
        rowBody += "<TD align=middle> {2} </TD>"
        rowBody += "<TD align=middle> {3} </TD>"
        rowBody += "<TD align=middle> {4} </TD>"
        rowBody += "<TD align=middle> {5} </TD>"
        rowBody += "<TD align=right> {6} </TD>"
        rowBody += "<TD align=right> {7} </TD>"
        rowBody += "<TD align=right> {8} </TD>"
        rowBody += "<TD align=middle> {9} </TD>"
        rowBody += "<TD align=middle> {10} </TD></TR>"


        For Each dr As Data.DataRow In dt.Rows
            Dim payment_duedate As String = ""
            Dim delivery_date As String = ""
            If Not IsDBNull(dr("payment_duedate")) Then
                payment_duedate = DateTime.Parse(dr("payment_duedate")).ToString(clsManage.formatDateTime)
            End If
            If Not IsDBNull(dr("delivery_date")) Then
                delivery_date = DateTime.Parse(dr("delivery_date")).ToString(clsManage.formatDateTime)
            End If
            htmlBody += String.Format(rowBody, DateTime.Parse(dr("ticket_date")).ToString(clsManage.formatDateTime), DateTime.Parse(dr("created_date")).ToString("hh:mm tt"), dr("ticket_id").ToString, _
                                      dr("book_no").ToString, dr("run_no").ToString, dr("type").ToString, clsManage.convert2Currency(dr("price")), _
                                      clsManage.convert2Quantity(dr("quantity")), clsManage.convert2Currency(dr("amount")), _
                                     payment_duedate, delivery_date)
        Next

        HttpContext.Current.Response.Write(htmlHead + htmlBody + htmlFoot)
        HttpContext.Current.Response.End()
    End Sub
    Public Shared Sub ExportToTicketPortfolioPurity99Historical(ByVal dt As DataTable, ByVal FileName As String, ByVal htSum As Hashtable)
        HttpContext.Current.Response.ClearContent()
        HttpContext.Current.Response.Charset = ""

        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Unicode
        HttpContext.Current.Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble())
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" & FileName & ".xls")
        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel;charset=tis-620"

        Dim StringWrite As New System.IO.StringWriter
        Dim HtmlWrite As New System.Web.UI.HtmlTextWriter(StringWrite)

        Dim htmlHead As String = ""
        htmlHead += "<div>"
        htmlHead += "<TABLE cellspacing='0'>"
        htmlHead += "<TR>"
        htmlHead += "	<TD colspan='9' style='COLOR: #274b98; FONT-WEIGHT: bold'>Customer Detail</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Ref No : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{0}</TD>"
        htmlHead += "	<TD align=right>บุคคลติดต่อ : </TD>"
        htmlHead += "	<TD colspan='3' align=left>{1}</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Name : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{2}</TD>"
        htmlHead += "	<TD align=right>Remark : </TD>"
        htmlHead += "	<TD colspan='3' align=left>{3}</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Address : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{4}</TD>"
        htmlHead += "	<TD></TD>"
        htmlHead += "	<TD colspan='3' align=left></TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Phone : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{5}</TD>"
        htmlHead += "	<TD></TD>"
        htmlHead += "	<TD colspan='3' align=left></TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Fax : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{6}</TD>"
        htmlHead += "	<TD></TD>"
        htmlHead += "	<TD colspan='3' align=left></TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Mobile Phone : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{7}</TD>"
        htmlHead += "	<TD></TD>"
        htmlHead += "	<TD colspan='3' align=left></TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Account bank : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{8}</TD>"
        htmlHead += "	<TD align=right>Branch : </TD>"
        htmlHead += "	<TD colspan='3' align=left>{9}</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Account No : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{10}</TD>"
        htmlHead += "	<TD align=right>Account Name : </TD>"
        htmlHead += "	<TD colspan='3' align=left>{11}</TD>"
        htmlHead += "</TR>"
        htmlHead += "</TABLE><br />"
        htmlHead += "<TABLE cellspacing='0'>"
        htmlHead += "<TR>"
        htmlHead += "	<TD colspan='9' style='COLOR: #274b98; FONT-WEIGHT: bold'>Customer Asset</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Cash : </TD>"
        htmlHead += "	<TD colspan='8'  align=left>{12} บาท</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Glod 96.50 : </TD>"
        htmlHead += "	<TD colspan='8'  align=left>{13} บาท</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right >Glod 99.99 : </TD>"
        htmlHead += "	<TD colspan='8'  align=left>{14} kg</TD>"
        htmlHead += "</TR>"
        htmlHead += "</TABLE><BR>"
        htmlHead += "<table cellspacing='0' rules='all' border='1' style='border-collapse:collapse;'>"
        htmlHead += "<tr>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>วันที่ซื้อขาย</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>เวลา</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>ticket_ref</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>book_no</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>No</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>คำสั่ง</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>ราคา</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>จำนวน</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>มุลค่าการซื้อขาย</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>วันชำระเงิน</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>วันส่งมอบสินค้า</TD></TR>"
        htmlHead = String.Format(htmlHead, htSum("RefNo").ToString, htSum("Person").ToString, htSum("Name").ToString, _
                         htSum("Remark").ToString, htSum("Add").ToString, htSum("Phone").ToString, htSum("Fax").ToString, _
                        htSum("MobilePhone").ToString, htSum("AccBank").ToString, htSum("Branch").ToString, _
                        htSum("AccNo").ToString, htSum("AccName").ToString, htSum("Cash").ToString, _
                        htSum("Gold96").ToString, htSum("Gold99").ToString)

        Dim htmlFoot As String = ""
        htmlFoot += "<TR>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #CFCCD0; COLOR: #000000; FONT-WEIGHT: bold' rowSpan=1 colSpan=7 align=middle>Total</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #CFCCD0; COLOR: #000000; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=right>{0}</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #CFCCD0; COLOR: #000000; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=right>{1}</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #CFCCD0; COLOR: #000000; FONT-WEIGHT: bold' rowSpan=1 colSpan=2 align=middle></TD>"
        htmlFoot += "</TR>"
        htmlFoot += "</table>"
        htmlFoot += "</div>"
        htmlFoot = String.Format(htmlFoot, clsManage.convert2Quantity(htSum("sumHisQuan99")), convert2Currency(htSum("sumHisAmount99")))


        Dim htmlBody As String = ""

        Dim rowBody As String = ""
        rowBody += "<TR style='BACKGROUND-COLOR: #eeeeee; COLOR: black'>"
        rowBody += "<TD align=middle> {0} </TD>"
        rowBody += "<TD align=middle> {1} </TD>"
        rowBody += "<TD align=middle> {2} </TD>"
        rowBody += "<TD align=middle> {3} </TD>"
        rowBody += "<TD align=middle> {4} </TD>"
        rowBody += "<TD align=middle> {5} </TD>"
        rowBody += "<TD align=right> {6} </TD>"
        rowBody += "<TD align=right> {7} </TD>"
        rowBody += "<TD align=right> {8} </TD>"
        rowBody += "<TD align=middle> {9} </TD>"
        rowBody += "<TD align=middle> {10} </TD></TR>"


        For Each dr As Data.DataRow In dt.Rows
            Dim payment_duedate As String = ""
            Dim delivery_date As String = ""
            If Not IsDBNull(dr("payment_duedate")) Then
                payment_duedate = DateTime.Parse(dr("payment_duedate")).ToString(clsManage.formatDateTime)
            End If
            If Not IsDBNull(dr("delivery_date")) Then
                delivery_date = DateTime.Parse(dr("delivery_date")).ToString(clsManage.formatDateTime)
            End If
            htmlBody += String.Format(rowBody, DateTime.Parse(dr("ticket_date")).ToString(clsManage.formatDateTime), DateTime.Parse(dr("created_date")).ToString("hh:mm tt"), dr("ticket_id").ToString, _
                                      dr("book_no").ToString, dr("run_no").ToString, dr("type").ToString, clsManage.convert2Currency(dr("price")), _
                                      clsManage.convert2Quantity(dr("quantity")), clsManage.convert2Currency(dr("amount")), _
                                     payment_duedate, delivery_date)
        Next

        HttpContext.Current.Response.Write(htmlHead + htmlBody + htmlFoot)
        HttpContext.Current.Response.End()
    End Sub
    Public Shared Sub ExportToTicketPortfolioPurity99(ByVal dt As DataTable, ByVal FileName As String, ByVal htSum As Hashtable)
        HttpContext.Current.Response.ClearContent()
        HttpContext.Current.Response.Charset = ""

        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Unicode
        HttpContext.Current.Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble())
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" & FileName & ".xls")
        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"

        Dim StringWrite As New System.IO.StringWriter
        Dim HtmlWrite As New System.Web.UI.HtmlTextWriter(StringWrite)

        Dim htmlHead As String = ""
        htmlHead += "<div>"
        htmlHead += "<TABLE cellspacing='0'>"
        htmlHead += "<TR>"
        htmlHead += "	<TD colspan='9' style='COLOR: #274b98; FONT-WEIGHT: bold'>Customer Detail</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Ref No : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{0}</TD>"
        htmlHead += "	<TD align=right>บุคคลติดต่อ : </TD>"
        htmlHead += "	<TD colspan='3' align=left>{1}</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Name : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{2}</TD>"
        htmlHead += "	<TD align=right>Remark : </TD>"
        htmlHead += "	<TD colspan='3' align=left>{3}</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Address : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{4}</TD>"
        htmlHead += "	<TD></TD>"
        htmlHead += "	<TD colspan='3' align=left></TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Phone : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{5}</TD>"
        htmlHead += "	<TD></TD>"
        htmlHead += "	<TD colspan='3' align=left></TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Fax : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{6}</TD>"
        htmlHead += "	<TD></TD>"
        htmlHead += "	<TD colspan='3' align=left></TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Mobile Phone : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{7}</TD>"
        htmlHead += "	<TD></TD>"
        htmlHead += "	<TD colspan='3' align=left></TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Account bank : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{8}</TD>"
        htmlHead += "	<TD align=right>Branch : </TD>"
        htmlHead += "	<TD colspan='3' align=left>{9}</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Account No : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{10}</TD>"
        htmlHead += "	<TD align=right>Account Name : </TD>"
        htmlHead += "	<TD colspan='3' align=left>{11}</TD>"
        htmlHead += "</TR>"
        htmlHead += "</TABLE><br />"
        htmlHead += "<TABLE cellspacing='0'>"
        htmlHead += "<TR>"
        htmlHead += "	<TD colspan='9' style='COLOR: #274b98; FONT-WEIGHT: bold'>Customer Asset</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Cash : </TD>"
        htmlHead += "	<TD colspan='8'  align=left>{12} บาท</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Glod 96.50 : </TD>"
        htmlHead += "	<TD colspan='8'  align=left>{13} บาท</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right >Glod 99.99 : </TD>"
        htmlHead += "	<TD colspan='8'  align=left>{14} kg</TD>"
        htmlHead += "</TR>"
        htmlHead += "</TABLE><BR>"
        htmlHead += "<table cellspacing='0' rules='all' border='1' style='border-collapse:collapse;'>"
        htmlHead += "<tr>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>วันที่ซื้อขาย</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>เวลา</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>ticket_ref</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>book_no</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>No</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>คำสั่ง</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>ราคา</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>จำนวน</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>มุลค่าการซื้อขาย</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>วันชำระเงิน</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>วันส่งมอบสินค้า</TD></TR>"
        htmlHead = String.Format(htmlHead, htSum("RefNo").ToString, htSum("Person").ToString, htSum("Name").ToString, _
                                 htSum("Remark").ToString, htSum("Add").ToString, htSum("Phone").ToString, htSum("Fax").ToString, _
                                htSum("MobilePhone").ToString, htSum("AccBank").ToString, htSum("Branch").ToString, _
                                htSum("AccNo").ToString, htSum("AccName").ToString, htSum("Cash").ToString, _
                                htSum("Gold96").ToString, htSum("Gold99").ToString)


        Dim htmlFoot As String = ""
        htmlFoot += "<TR>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #CFCCD0; COLOR: #000000; FONT-WEIGHT: bold' rowSpan=1 colSpan=7 align=middle>Total</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #CFCCD0; COLOR: #000000; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=right>{0}</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #CFCCD0; COLOR: #000000; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=right>{1}</TD>"
        htmlFoot += "<TD style='BACKGROUND-COLOR: #CFCCD0; COLOR: #000000; FONT-WEIGHT: bold' rowSpan=1 colSpan=2 align=middle></TD>"
        htmlFoot += "</TR>"
        htmlFoot += "</table>"
        htmlFoot += "</div>"
        htmlFoot = String.Format(htmlFoot, clsManage.convert2Quantity(htSum("sumQuan99")), convert2Currency(htSum("sumAmount99")))


        Dim htmlBody As String = ""

        Dim rowBody As String = ""
        rowBody += "<TR style='BACKGROUND-COLOR: #eeeeee; COLOR: black'>"
        rowBody += "<TD align=middle> {0} </TD>"
        rowBody += "<TD align=middle> {1} </TD>"
        rowBody += "<TD align=middle> {2} </TD>"
        rowBody += "<TD align=middle> {3} </TD>"
        rowBody += "<TD align=middle> {4} </TD>"
        rowBody += "<TD align=middle> {5} </TD>"
        rowBody += "<TD align=right> {6} </TD>"
        rowBody += "<TD align=right> {7} </TD>"
        rowBody += "<TD align=right> {8} </TD>"
        rowBody += "<TD align=middle> {9} </TD>"
        rowBody += "<TD align=middle> {10} </TD></TR>"

        For Each dr As Data.DataRow In dt.Rows
            Dim payment_duedate As String = ""
            Dim ticket_date As String = ""
            Dim created_date As String = ""
            Dim price As String = ""
            Dim quantity As String = ""
            Dim amount As String = ""
            Dim delivery_date As String = ""

            If Not IsDBNull(dr("payment_duedate")) Then
                payment_duedate = DateTime.Parse(dr("payment_duedate")).ToString(clsManage.formatDateTime)
            End If
            If Not IsDBNull(dr("ticket_date")) Then
                ticket_date = DateTime.Parse(dr("ticket_date")).ToString(clsManage.formatDateTime)
            End If
            If Not IsDBNull(dr("created_date")) Then
                created_date = DateTime.Parse(dr("created_date")).ToString("hh:mm tt")
            End If
            If Not IsDBNull(dr("price")) Then
                price = clsManage.convert2Currency(dr("price"))
            End If
            If Not IsDBNull(dr("quantity")) Then
                quantity = clsManage.convert2Quantity(dr("quantity"))
            End If
            If Not IsDBNull(dr("amount")) Then
                amount = clsManage.convert2Currency(dr("amount"))
            End If
            If Not IsDBNull(dr("delivery_date")) Then
                delivery_date = DateTime.Parse(dr("delivery_date")).ToString(clsManage.formatDateTime)
            End If


            htmlBody += String.Format(rowBody, ticket_date, created_date, dr("ticket_id").ToString, _
                                      dr("book_no").ToString, dr("run_no").ToString, dr("type").ToString, price, _
                                      quantity, amount, _
                                     payment_duedate, delivery_date)
        Next

        HttpContext.Current.Response.Write(htmlHead + htmlBody + htmlFoot)
        HttpContext.Current.Response.End()
    End Sub
    Public Shared Sub ExportToCustPortfolioCash(ByVal dt As DataTable, ByVal FileName As String, ByVal htSum As Hashtable)
        HttpContext.Current.Response.ClearContent()
        HttpContext.Current.Response.Charset = ""

        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Unicode
        HttpContext.Current.Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble())
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" & FileName & ".xls")
        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"

        Dim StringWrite As New System.IO.StringWriter
        Dim HtmlWrite As New System.Web.UI.HtmlTextWriter(StringWrite)

        Dim htmlHead As String = ""
        htmlHead += "<div>"
        htmlHead += "<TABLE cellspacing='0'>"
        htmlHead += "<TR>"
        htmlHead += "	<TD colspan='9' style='COLOR: #274b98; FONT-WEIGHT: bold'>Customer Detail</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Ref No : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{0}</TD>"
        htmlHead += "	<TD align=right>บุคคลติดต่อ : </TD>"
        htmlHead += "	<TD colspan='3' align=left>{1}</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Name : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{2}</TD>"
        htmlHead += "	<TD align=right>Remark : </TD>"
        htmlHead += "	<TD colspan='3' align=left>{3}</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Address : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{4}</TD>"
        htmlHead += "	<TD></TD>"
        htmlHead += "	<TD colspan='3' align=left></TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Phone : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{5}</TD>"
        htmlHead += "	<TD></TD>"
        htmlHead += "	<TD colspan='3' align=left></TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Fax : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{6}</TD>"
        htmlHead += "	<TD></TD>"
        htmlHead += "	<TD colspan='3' align=left></TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Mobile Phone : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{7}</TD>"
        htmlHead += "	<TD></TD>"
        htmlHead += "	<TD colspan='3' align=left></TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Account bank : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{8}</TD>"
        htmlHead += "	<TD align=right>Branch : </TD>"
        htmlHead += "	<TD colspan='3' align=left>{9}</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Account No : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{10}</TD>"
        htmlHead += "	<TD align=right>Account Name : </TD>"
        htmlHead += "	<TD colspan='3' align=left>{11}</TD>"
        htmlHead += "</TR>"
        htmlHead += "</TABLE><br />"
        htmlHead += "<TABLE cellspacing='0'>"
        htmlHead += "<TR>"
        htmlHead += "	<TD colspan='9' style='COLOR: #274b98; FONT-WEIGHT: bold'>Customer Asset</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Cash : </TD>"
        htmlHead += "	<TD colspan='8'  align=left>{12} บาท</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Glod 96.50 : </TD>"
        htmlHead += "	<TD colspan='8'  align=left>{13} บาท</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right >Glod 99.99 : </TD>"
        htmlHead += "	<TD colspan='8'  align=left>{14} kg</TD>"
        htmlHead += "</TR>"
        htmlHead += "</TABLE><BR>"
        htmlHead += "<table cellspacing='0' rules='all' border='1' style='border-collapse:collapse;'>"
        htmlHead += "<tr>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>Ref_No</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>datetime</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>ฝาก</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>ถอน</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>สุทธิ</TD></TR>"
        htmlHead = String.Format(htmlHead, htSum("RefNo").ToString, htSum("Person").ToString, htSum("Name").ToString, _
                                 htSum("Remark").ToString, htSum("Add").ToString, htSum("Phone").ToString, htSum("Fax").ToString, _
                                htSum("MobilePhone").ToString, htSum("AccBank").ToString, htSum("Branch").ToString, _
                                htSum("AccNo").ToString, htSum("AccName").ToString, htSum("Cash").ToString, _
                                htSum("Gold96").ToString, htSum("Gold99").ToString)


        Dim htmlFoot As String = ""
        htmlFoot += "</table>"
        htmlFoot += "</div>"

        Dim htmlBody As String = ""

        Dim rowBody As String = ""
        rowBody += "<TR style='BACKGROUND-COLOR: #eeeeee; COLOR: black'>"
        rowBody += "<TD align=middle> {0} </TD>"
        rowBody += "<TD align=middle> {1} </TD>"
        rowBody += "<TD align=Right> {2} </TD>"
        rowBody += "<TD align=Right> {3} </TD>"
        rowBody += "<TD align=Right> {4} </TD></TR>"

        For Each dr As Data.DataRow In dt.Rows
            Dim asset As Double
            asset = asset + dr("deposit")
            asset = asset - dr("withdraw")

            Dim Ref As String = ""
            If Not IsDBNull(dr("ref_no")) Then
                Ref = dr("ref_no").ToString
            End If
            htmlBody += String.Format(rowBody, Ref, DateTime.Parse(dr("datetime")).ToString(clsManage.formatDateTime), _
                                      clsManage.convert2Currency(dr("deposit")), clsManage.convert2Currency(dr("withdraw")), clsManage.convert2Currency(asset))
        Next

        HttpContext.Current.Response.Write(htmlHead + htmlBody + htmlFoot)
        HttpContext.Current.Response.End()
    End Sub
    Public Shared Sub ExportToCustPortfolioGold(ByVal dt As DataTable, ByVal FileName As String, ByVal htSum As Hashtable)
        HttpContext.Current.Response.ClearContent()
        HttpContext.Current.Response.Charset = ""

        HttpContext.Current.Response.ContentEncoding = System.Text.Encoding.Unicode
        HttpContext.Current.Response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble())
        HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=" & FileName & ".xls")
        HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
        HttpContext.Current.Response.ContentType = "application/vnd.ms-excel"

        Dim StringWrite As New System.IO.StringWriter
        Dim HtmlWrite As New System.Web.UI.HtmlTextWriter(StringWrite)

        Dim htmlHead As String = ""
        htmlHead += "<div>"
        htmlHead += "<TABLE cellspacing='0'>"
        htmlHead += "<TR>"
        htmlHead += "	<TD colspan='9' style='COLOR: #274b98; FONT-WEIGHT: bold'>Customer Detail</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Ref No : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{0}</TD>"
        htmlHead += "	<TD align=right>บุคคลติดต่อ : </TD>"
        htmlHead += "	<TD colspan='3' align=left>{1}</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Name : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{2}</TD>"
        htmlHead += "	<TD align=right>Remark : </TD>"
        htmlHead += "	<TD colspan='3' align=left>{3}</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Address : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{4}</TD>"
        htmlHead += "	<TD></TD>"
        htmlHead += "	<TD colspan='3' align=left></TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Phone : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{5}</TD>"
        htmlHead += "	<TD></TD>"
        htmlHead += "	<TD colspan='3' align=left></TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Fax : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{6}</TD>"
        htmlHead += "	<TD></TD>"
        htmlHead += "	<TD colspan='3' align=left></TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Mobile Phone : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{7}</TD>"
        htmlHead += "	<TD></TD>"
        htmlHead += "	<TD colspan='3' align=left></TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Account bank : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{8}</TD>"
        htmlHead += "	<TD align=right>Branch : </TD>"
        htmlHead += "	<TD colspan='3' align=left>{9}</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Account No : </TD>"
        htmlHead += "	<TD colspan='4' align=left>{10}</TD>"
        htmlHead += "	<TD align=right>Account Name : </TD>"
        htmlHead += "	<TD colspan='3' align=left>{11}</TD>"
        htmlHead += "</TR>"
        htmlHead += "</TABLE><br />"
        htmlHead += "<TABLE cellspacing='0'>"
        htmlHead += "<TR>"
        htmlHead += "	<TD colspan='9' style='COLOR: #274b98; FONT-WEIGHT: bold'>Customer Asset</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Cash : </TD>"
        htmlHead += "	<TD colspan='8'  align=left>{12} บาท</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right>Glod 96.50 : </TD>"
        htmlHead += "	<TD colspan='8'  align=left>{13} บาท</TD>"
        htmlHead += "</TR>"
        htmlHead += "<TR>"
        htmlHead += "	<TD align=right >Glod 99.99 : </TD>"
        htmlHead += "	<TD colspan='8'  align=left>{14} kg</TD>"
        htmlHead += "</TR>"
        htmlHead += "</TABLE><BR>"
        htmlHead += "<table cellspacing='0' rules='all' border='1' style='border-collapse:collapse;'>"
        htmlHead += "<tr>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>Ref_No</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>datetime</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>Purity</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>ฝาก</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>ถอน</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>สุทธิ96</TD>"
        htmlHead += "<TD style='BACKGROUND-COLOR: #274b98; COLOR: white; FONT-WEIGHT: bold' rowSpan=1 colSpan=1 align=middle>สุทธิ99</TD></TR>"
        htmlHead = String.Format(htmlHead, htSum("RefNo").ToString, htSum("Person").ToString, htSum("Name").ToString, _
                                 htSum("Remark").ToString, htSum("Add").ToString, htSum("Phone").ToString, htSum("Fax").ToString, _
                                htSum("MobilePhone").ToString, htSum("AccBank").ToString, htSum("Branch").ToString, _
                                htSum("AccNo").ToString, htSum("AccName").ToString, htSum("Cash").ToString, _
                                htSum("Gold96").ToString, htSum("Gold99").ToString)


        Dim htmlFoot As String = ""
        htmlFoot += "</table>"
        htmlFoot += "</div>"

        Dim htmlBody As String = ""

        Dim rowBody As String = ""
        rowBody += "<TR style='BACKGROUND-COLOR: #eeeeee; COLOR: black'>"
        rowBody += "<TD align=middle> {0} </TD>"
        rowBody += "<TD align=middle> {1} </TD>"
        rowBody += "<TD align=Right> {2} </TD>"
        rowBody += "<TD align=Right> {3} </TD>"
        rowBody += "<TD align=Right> {4} </TD>"
        rowBody += "<TD align=Right> {5} </TD>"
        rowBody += "<TD align=Right> {6} </TD></TR>"

        For Each dr As Data.DataRow In dt.Rows

            Dim Ref As String = ""
            If Not IsDBNull(dr("ref_no")) Then
                Ref = dr("ref_no").ToString
            End If

            Dim sumgold99 As Double
            Dim sumgold96 As Double
            If dr("gold_type_id") = "96" Then
                sumgold96 += Double.Parse(dr("deposit")) + (-Double.Parse(dr("withdraw")))
            Else
                sumgold99 += Double.Parse(dr("deposit")) + (-Double.Parse(dr("withdraw")))
            End If

            htmlBody += String.Format(rowBody, Ref, DateTime.Parse(dr("datetime")).ToString(clsManage.formatDateTime), dr("gold_type_id").ToString, _
                                      clsManage.convert2Currency(dr("deposit")), clsManage.convert2Currency(dr("withdraw")), clsManage.convert2Currency(sumgold96), clsManage.convert2Currency(sumgold99))
        Next

        HttpContext.Current.Response.Write(htmlHead + htmlBody + htmlFoot)
        HttpContext.Current.Response.End()
    End Sub

#End Region

    Public Shared Function formatTextZoro(ByVal amt As Double, ByVal count As Integer) As String
        Try
            Dim newFormat As String = ""
            For i As Integer = 0 To count - 1
                newFormat += "0"
            Next
            Dim x As String = Format(amt, newFormat)
            Return x
        Catch ex As Exception
            Throw ex
        End Try
    End Function

    'Shared Function formatTextZoro(p1 As String) As Object
    '    Throw New NotImplementedException
    'End Function

End Class
