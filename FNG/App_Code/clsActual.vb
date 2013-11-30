Imports Microsoft.VisualBasic

Public Class clsActual

    Private _asset_id As String
    Public Property asset_id() As String
        Get
            Return _asset_id
        End Get
        Set(ByVal value As String)
            _asset_id = value
        End Set
    End Property

    Private _ref_no As String
    Public Property ref_no() As String
        Get
            Return _ref_no
        End Get
        Set(ByVal value As String)
            _ref_no = value
        End Set
    End Property

    Private _created_by As String
    Public Property created_by() As String
        Get
            Return _created_by
        End Get
        Set(ByVal value As String)
            _created_by = value
        End Set
    End Property

    Private _purity As String
    Public Property purity() As String
        Get
            Return _purity
        End Get
        Set(ByVal value As String)
            _purity = value
        End Set
    End Property

    Private _quantity As String
    Public Property quantity() As String
        Get
            Return _quantity
        End Get
        Set(ByVal value As String)
            _quantity = value
        End Set
    End Property

    Private _amount As String
    Public Property amount() As String
        Get
            Return _amount
        End Get
        Set(ByVal value As String)
            _amount = value
        End Set
    End Property

    Private _status_id As String
    Public Property status_id() As String
        Get
            Return _status_id
        End Get
        Set(ByVal value As String)
            _status_id = value
        End Set
    End Property

    Private _status_name As String
    Public Property status_name() As String
        Get
            Return _status_name
        End Get
        Set(ByVal value As String)
            _status_name = value
        End Set
    End Property

    Private _type As String
    Public Property type() As String
        Get
            Return _type
        End Get
        Set(ByVal value As String)
            _type = value
        End Set
    End Property

    Private _order_type As String
    Public Property order_type() As String
        Get
            Return _order_type
        End Get
        Set(ByVal value As String)
            _order_type = value
        End Set
    End Property

    Private _cust_id As String
    Public Property cust_id() As String
        Get
            Return _cust_id
        End Get
        Set(ByVal value As String)
            _cust_id = value
        End Set
    End Property

    Private _before_status_id As String
    Public Property before_status_id() As String
        Get
            Return _before_status_id
        End Get
        Set(ByVal value As String)
            _before_status_id = value
        End Set
    End Property

    Private _note As String
    Public Property note() As String
        Get
            Return _note
        End Get
        Set(ByVal value As String)
            _note = value
        End Set
    End Property

    Private _update_cash As String
    Public Property update_cash() As String
        Get
            Return _update_cash
        End Get
        Set(ByVal value As String)
            _update_cash = value
        End Set
    End Property

    Private _update_gold As String
    Public Property update_gold() As String
        Get
            Return _update_gold
        End Get
        Set(ByVal value As String)
            _update_gold = value
        End Set
    End Property

    Private _cash As String
    Public Property cash() As String
        Get
            Return _cash
        End Get
        Set(ByVal value As String)
            _cash = value
        End Set
    End Property

    Private _payment As String
    Public Property payment() As String
        Get
            Return _payment
        End Get
        Set(ByVal value As String)
            _payment = value
        End Set
    End Property

End Class
