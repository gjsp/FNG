Imports System.Web
Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.Data
Imports System.Data.SqlClient

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
<System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://tempuri.org/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class gtc
    Inherits System.Web.Services.WebService
    Public Shared strcon As String = ConfigurationManager.ConnectionStrings("FNGConnectionString").ConnectionString

    <WebMethod()> _
    Public Function getStockOnline() As String
        Dim dt As New DataTable
        Try
            Dim result As String = ""
            Dim sql As String = "select * from gtc.dbo.stock_online"
            Using da As New SqlDataAdapter(sql, strcon)
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    result = dt.Rows(0)("bid99_1").ToString + "," + dt.Rows(0)("ask99_1").ToString + "," + dt.Rows(0)("bid96_1").ToString + "," + dt.Rows(0)("ask96_1").ToString
                End If
            End Using
            Return result
        Catch ex As Exception
            Throw ex
        Finally
            dt.Dispose()
        End Try

    End Function

    <WebMethod()> _
    Public Function getTrans(ByVal log_id As String) As String
        Dim sql As String = String.Format("select log_id,t.trade_id,type from log_tran l inner join trade t on t.trade_id = l.trade_id where l.log_id > {0} ", log_id)
        Using con As New SqlConnection(strcon)
            Dim da As New SqlDataAdapter(sql, con)
            Dim dt As New DataTable
            Try
                da.Fill(dt)
                If dt.Rows.Count > 0 Then
                    'buy:1,sell:2,both:3
                    Dim order_type As String = ""
                    For i = 1 To dt.Rows.Count - 1
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

    <WebMethod()> _
    Public Function getCust_idList(ByVal count As Integer, ByVal contextKey As String, ByVal prefixText As String) As String()
        Dim storedName As String = "[getCust_idList] '" & prefixText & "'"
        Dim dt As New Data.DataTable
        Dim dr As Data.DataRow
        Dim list As New List(Of String)

        Dim con As New SqlConnection(clsDB.strcon)
        Dim cmd As New SqlCommand(storedName, con)


        Dim da As New SqlDataAdapter(cmd)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            For Each dr In dt.Rows
                Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dr("cust_id").ToString, dr("firstname").ToString)
                list.Add(item)
            Next
        End If


        Return list.ToArray()

    End Function
    <WebMethod()> _
   Public Function getCust_nameList(ByVal count As Integer, ByVal contextKey As String, ByVal prefixText As String) As String()
        Dim storedName As String = "[getCust_nameList] '" & prefixText & "'"
        Dim dt As New Data.DataTable
        Dim dr As Data.DataRow
        Dim list As New List(Of String)

        Dim con As New SqlConnection(clsDB.strcon)
        Dim cmd As New SqlCommand(storedName, con)


        Dim da As New SqlDataAdapter(cmd)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            For Each dr In dt.Rows
                Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dr("firstname").ToString, dr("cust_id").ToString)
                list.Add(item)
            Next
        End If


        Return list.ToArray()

    End Function

    'For Search Online
    <WebMethod()> _
    Public Function getCust_nameOnlineList(ByVal count As Integer, ByVal contextKey As String, ByVal prefixText As String) As String()
        Dim storedName As String = "[getCust_nameOnlineList] '" & prefixText & "'"
        Dim dt As New Data.DataTable
        Dim dr As Data.DataRow
        Dim list As New List(Of String)

        Dim con As New SqlConnection(clsDB.strcon)
        Dim cmd As New SqlCommand(storedName, con)


        Dim da As New SqlDataAdapter(cmd)
        da.Fill(dt)

        If dt.Rows.Count > 0 Then
            For Each dr In dt.Rows
                Dim item As String = AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem(dr("cust_id").ToString + " " + dr("firstname").ToString, dr("cust_id").ToString)
                list.Add(item)
            Next
        End If


        Return list.ToArray()

    End Function

    <WebMethod()> _
    Public Function EncodeMD5(ByVal x_string As String) As String
        Dim md5 As New System.Security.Cryptography.MD5CryptoServiceProvider()
        Dim ascii As New System.Text.ASCIIEncoding()

        Dim input As Byte()
        Dim output As Byte()
        Dim encode As String = String.Empty

        input = ascii.GetBytes(x_string)
        output = md5.ComputeHash(input)

        For Each xbyte As Byte In output
            encode &= String.Format("{0:X}", xbyte)
        Next
        Return (encode)
    End Function

    <WebMethod()> _
    Public Function HelloToYou(ByVal name As String) As String
        Return "Hello " + name
    End Function

    <WebMethod()> _
    Public Function sayHello() As String
        Return "hello "
    End Function

    <WebMethod()> _
    Public Function getPriceStore() As String
        Dim s = New dcDBDataContext().getStockOnline().Single()
        Dim c As New clsStore
        Dim result As String = ""
        Dim cma = "," : Dim l As String = "|" : Dim shp As String = "#"
        If s.self_price = clsManage.no Then
            c.selfPrice = clsManage.no
            Dim ws As New GcapProxy.MiniService()
            Dim gcapPrice As String = ws.getPrice("LSH")
            Dim level As Integer = 0
            c.bid99 = gcapPrice.Split(shp)(0).Split(l)(0).Split(cma)(level)
            c.ask99 = gcapPrice.Split(shp)(0).Split(l)(1).Split(cma)(level)
            c.bid96 = gcapPrice.Split(shp)(1).Split(l)(0).Split(cma)(level)
            c.ask96 = gcapPrice.Split(shp)(1).Split(l)(1).Split(cma)(level)
            c.timeTrade = gcapPrice.Split(shp)(2).Split(l)(0)
            c.adminHalt = gcapPrice.Split(shp)(2).Split(l)(1)

            'plus from gold shop
            c.bid99 = c.bid99 - IIf(s.bid99_plus Is Nothing, 0, s.bid99_plus)
            c.ask99 = c.ask99 + IIf(s.ask99_plus Is Nothing, 0, s.ask99_plus)
            c.bid96 = c.bid96 - IIf(s.bid96_plus Is Nothing, 0, s.bid96_plus)
            c.ask96 = c.ask96 + IIf(s.ask96_plus Is Nothing, 0, s.ask96_plus)
        Else
            c.bid99 = s.bid99_1.ToString
            c.ask99 = s.ask99_1.ToString
            c.bid96 = s.bid96_1.ToString
            c.ask96 = s.ask96_1.ToString
        End If

        result = c.bid99.ToString("#,##0") + l + c.ask99.ToString("#,##0") + l + c.bid96.ToString("#,##0") + l + c.ask96.ToString("#,##0") + l + DateTime.Now.ToString("dd-MM-yy   HH:mm") + l + DateTime.Now.ToString("ss")

        Return result
    End Function
End Class
