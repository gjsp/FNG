Imports Microsoft.VisualBasic

Public Class clsStore

    Private _bid99 As Double
    Public Property bid99() As Double
        Get
            Return _bid99
        End Get
        Set(ByVal value As Double)
            _bid99 = value
        End Set
    End Property

    Private _ask99 As Double
    Public Property ask99() As Double
        Get
            Return _ask99
        End Get
        Set(ByVal value As Double)
            _ask99 = value
        End Set
    End Property

    Private _bid96 As Double
    Public Property bid96() As Double
        Get
            Return _bid96
        End Get
        Set(ByVal value As Double)
            _bid96 = value
        End Set
    End Property

    Private _ask96 As Double
    Public Property ask96() As Double
        Get
            Return _ask96
        End Get
        Set(ByVal value As Double)
            _ask96 = value
        End Set
    End Property

    Private _bid96Mini As Double
    Public Property bid96Mini() As Double
        Get
            Return _bid96Mini
        End Get
        Set(ByVal value As Double)
            _bid96Mini = value
        End Set
    End Property

    Private _ask96Mini As Double
    Public Property ask96Mini() As Double
        Get
            Return _ask96Mini
        End Get
        Set(ByVal value As Double)
            _ask96Mini = value
        End Set
    End Property

    Private _ask96Plus As Integer
    Public Property ask96Plus() As Integer
        Get
            Return _ask96Plus
        End Get
        Set(ByVal value As Integer)
            _ask96Plus = value
        End Set
    End Property

    Private _bid96Plus As Integer
    Public Property bid96plus() As Integer
        Get
            Return _bid96Plus
        End Get
        Set(ByVal value As Integer)
            _bid96Plus = value
        End Set
    End Property

    Private _ask99Plus As Integer
    Public Property ask99Plus() As Integer
        Get
            Return _ask99Plus
        End Get
        Set(ByVal value As Integer)
            _ask99Plus = value
        End Set
    End Property

    Private _bid99Plus As Integer
    Public Property bid99Plus() As Integer
        Get
            Return _bid99Plus
        End Get
        Set(ByVal value As Integer)
            _bid99Plus = value
        End Set
    End Property

    Private _selfPrice As String
    Public Property selfPrice() As String
        Get
            Return _selfPrice
        End Get
        Set(ByVal value As String)
            _selfPrice = value
        End Set
    End Property

    Private _selfHalt As String
    Public Property selfHalt() As String
        Get
            Return _selfHalt
        End Get
        Set(ByVal value As String)
            _selfHalt = value
        End Set
    End Property

    Private _timeTrade As String
    Public Property timeTrade() As String
        Get
            Return _timeTrade
        End Get
        Set(ByVal value As String)
            _timeTrade = value
        End Set
    End Property

    Private _level As Integer
    Public Property level() As Integer
        Get
            Return _level
        End Get
        Set(ByVal value As Integer)
            _level = value
        End Set
    End Property

    Private _maxKg As Integer
    Public Property maxKg() As Integer
        Get
            Return _maxKg
        End Get
        Set(ByVal value As Integer)
            _maxKg = value
        End Set
    End Property

    Private _maxBaht As Integer
    Public Property maxBaht() As Integer
        Get
            Return _maxBaht
        End Get
        Set(ByVal value As Integer)
            _maxBaht = value
        End Set
    End Property

    Private _maxMini As Integer
    Public Property maxMini() As Integer
        Get
            Return _maxMini
        End Get
        Set(ByVal value As Integer)
            _maxMini = value
        End Set
    End Property

    Private _adminHalt As String
    Public Property adminHalt() As String
        Get
            Return _adminHalt
        End Get
        Set(ByVal value As String)
            _adminHalt = value
        End Set
    End Property

    Private _custHalt As String
    Public Property custHalt() As String
        Get
            Return _custHalt
        End Get
        Set(ByVal value As String)
            _custHalt = value
        End Set
    End Property

    Private _systemHalt As String
    Public Property systemHalt() As String
        Get
            Return _systemHalt
        End Get
        Set(ByVal value As String)
            _systemHalt = value
        End Set
    End Property

    Private _modifierDate As String
    Public Property modifierDate() As String
        Get
            Return _modifierDate
        End Get
        Set(ByVal value As String)
            _modifierDate = value
        End Set
    End Property


    Public Shared Function getPriceStore(ByVal cust_id As String) As clsStore
        Dim s = New dcDBDataContext().getStockOnlineForPrice(cust_id).Single()

        Dim c As New clsStore
        If s.self_price = clsManage.no Then
            c.selfPrice = clsManage.no
            Dim ws As New GcapProxy.MiniService()
            Dim gcapPrice As String = ws.getPrice("LSH")

            Dim cma = "," : Dim l As String = "|" : Dim shp As String = "#"
            Dim level As Integer = s.cust_level - 1

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
            Select Case s.cust_level
                Case 1
                    c.bid99 = s.bid99_1.ToString
                    c.ask99 = s.ask99_1.ToString
                    c.bid96 = s.bid96_1.ToString
                    c.ask96 = s.ask96_1.ToString
                Case 2
                    c.bid99 = s.bid99_2.ToString
                    c.ask99 = s.ask99_2.ToString
                    c.bid96 = s.bid96_2.ToString
                    c.ask96 = s.ask96_2.ToString
                Case 3
                    c.bid99 = s.bid99_3.ToString
                    c.ask99 = s.ask99_3.ToString
                    c.bid96 = s.bid96_3.ToString
                    c.ask96 = s.ask96_3.ToString
            End Select
            c.timeTrade = s.time_trade
            c.adminHalt = clsManage.no 'price self nothing halt
            c.selfPrice = clsManage.yes
        End If

        'gold mini +-10
        c.bid96Mini = c.bid96 - 10
        c.ask96Mini = c.ask96 + 10
        'c.modifierDate = s.modifier_date
        c.bid99Plus = IIf(s.bid99_plus Is Nothing, 0, s.bid99_plus)
        c.ask99Plus = IIf(s.ask99_plus Is Nothing, 0, s.ask99_plus)
        c.bid96plus = IIf(s.bid96_plus Is Nothing, 0, s.bid96_plus)
        c.ask96Plus = IIf(s.ask96_plus Is Nothing, 0, s.ask96_plus)
        c.maxBaht = s.max_baht
        c.maxKg = s.max_kg
        c.maxMini = s.max_mini
        c.level = s.cust_level
        c.custHalt = s.cust_halt
        c.selfHalt = s.self_halt
        c.systemHalt = s.system_halt
        Return c
    End Function

    Public Shared Function getPriceStore() As clsStore

        Dim c As New clsStore
        Dim ws As New GcapProxy.MiniService()
        Dim gcapPrice As String = ws.getPrice("LSH")
        Dim cma = "," : Dim l As String = "|" : Dim shp As String = "#"
        Dim level As Integer

        c.bid99 = gcapPrice.Split(shp)(0).Split(l)(0).Split(cma)(level)
        c.ask99 = gcapPrice.Split(shp)(0).Split(l)(1).Split(cma)(level)
        c.bid96 = gcapPrice.Split(shp)(1).Split(l)(0).Split(cma)(level)
        c.ask96 = gcapPrice.Split(shp)(1).Split(l)(1).Split(cma)(level)
        c.timeTrade = gcapPrice.Split(shp)(2).Split(l)(0)
        c.adminHalt = gcapPrice.Split(shp)(2).Split(l)(1)
        Return c
    End Function


    Public Shared Function getPriceStoreAll() As String
        Dim c As New clsStore
        Dim ws As New GcapProxy.MiniService()
        Return ws.getPrice("LSH")
    End Function
End Class
