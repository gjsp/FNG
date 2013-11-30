
Partial Class customer_list
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            ' lblP.Attributes.Add("style", "margin: 0px; width: 100px; height: 80px; background: green; border: 1px solid black; position: relative;")
        End If
    End Sub

    Protected Sub btn_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btn.Click
        Dim dc As New dcDBDataContext()
        Dim cust = From c In dc.cust_lists


        Dim ct As customer

        For Each c In cust
            'Dim cid As Integer = (From cc In dc.customers Select cc.cust_id).Max + 1
            ct = New customer
            ct.cust_id = (From cc In dc.customers Select cc.cust_id).Max + 1
            ct.cust_type_id = IIf(c.title = "ร้านทอง", 1002, 1001)
            ct.titlename = IIf(c.title = "ร้านทอง", "ร้าน", c.title)
            ct.firstname = c.name
            ct.lastname = c.lastname
            ct.person_contact = ""
            ct.mobile = c.phone
            ct.bank1 = c.bank
            ct.account_no1 = c.account_no
            ct.account_name1 = ""
            ct.account_type1 = ""
            ct.account_branch1 = ""

            ct.cash_credit = 0
            ct.margin = 10
            ct.margin_call = 0
            ct.quan_96 = 0
            ct.quan_99L = 0
            ct.quan_99N = 0
            ct.created_by = 1001
            ct.created_date = DateTime.Now
            ct.team_id = ""
            ct.firstname_eng = ""
            ct.lastname_eng = ""
            ct.email = "tmdgolduser@hotmail.com"
            ct.id_card = ""
            ct.free_margin = 0
            ct.trade_limit = 0
            dc.customers.InsertOnSubmit(ct)

            'un = New username
            'un.cust_id = ct.cust_id
            'un.username = clsManage.genPwd(6)
            'un.password = clsManage.genPwd(6)
            'un.role = "cust"
            'un.cust_level = 1
            'un.active = "y"
            'un.halt = "n"
            'un.first_trade = "y"
            'un.created_date = DateTime.Now
            'un.created_by = 10001

            dc.SubmitChanges()
        Next


    End Sub

    Protected Sub btnUsr_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUsr.Click
        Dim dc As New dcDBDataContext()
        Dim un As username
        Dim stm = From c In dc.customers
        For Each c In stm

            un = New username
            un.cust_id = c.cust_id
            un.username = clsManage.genPwd(6)
            un.password = clsManage.genPwd(6)
            un.role = "cust"
            un.cust_level = 1
            un.active = "y"
            un.halt = "n"
            un.first_trade = "y"
            un.created_date = DateTime.Now
            un.created_by = 10001
            dc.usernames.InsertOnSubmit(un)
            dc.SubmitChanges()
        Next


    End Sub

    Protected Sub btnUsr0_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnUsr0.Click
        'Dim strInsert = "INSERT INTO [TMD].[dbo].[customer]([cust_id],[cust_refno],[cust_type_id],[titlename] ,[firstname],[lastname],[person_contact],[bill_address],[bank1],[account_no1],[account_name1],[account_type1] " & _
        '   ",[account_branch1],[bank2],[account_no2],[account_name2],[account_type2],[account_branch2],[bank3],[account_no3],[account_name3],[account_type3],[account_branch3],[mobile],[tel],[fax]" & _
        '   ",[remark],[cash_credit],[margin],[margin_call],[quan_96],[quan_99N],[quan_99L],[created_by],[created_date],[team_id],[firstname_eng],[lastname_eng]" & _
        '   ",[email],[id_card],[birthday],[free_margin],[trade_limit])" & _
        '"VALUES() " & _
        '   "('{0}'," & _
        '   "'{1}',>" & _
        '   ",<cust_type_id, int,>"
        '   ",<titlename, varchar(50),>
        '   ",<firstname, varchar(50),>
        '   ",<lastname, varchar(50),>
        '   ",<person_contact, varchar(50),>
        '   ,<bill_address, varchar(500),>
        '   ,<bank1, varchar(50),>
        '   ,<account_no1, varchar(50),>
        '   ,<account_name1, varchar(50),>
        '   ,<account_type1, varchar(50),>
        '   ,<account_branch1, varchar(50),>
        '   ,<bank2, varchar(50),>
        '   ,<account_no2, varchar(50),>
        '   ,<account_name2, varchar(50),>
        '   ,<account_type2, varchar(50),>
        '   ,<account_branch2, varchar(50),>
        '   ,<bank3, varchar(50),>
        '   ,<account_no3, varchar(50),>
        '   ,<account_name3, varchar(50),>
        '   ,<account_type3, varchar(50),>
        '   ,<account_branch3, varchar(50),>
        '   ,<mobile, varchar(50),>
        '   ,<tel, varchar(50),>
        '   ,<fax, varchar(50),>
        '   ,<remark, varchar(1000),>
        '   ,<cash_credit, decimal(18,2),>
        '   ,<margin, float,>
        '   ,<margin_call, float,>
        '   ,<quan_96, decimal(18,5),>
        '   ,<quan_99N, decimal(18,5),>
        '   ,<quan_99L, decimal(18,5),>
        '   ,<created_by, varchar(50),>
        '   ,<created_date, datetime,>
        '   ,<team_id, varchar(50),>
        '   ,<firstname_eng, varchar(50),>
        '   ,<lastname_eng, varchar(50),>
        '   ,<email, varchar(50),>
        '   ,<id_card, varchar(50),>
        '   ,<birthday, datetime,>
        '   ,<free_margin, decimal(18,0),>
        '   ,<trade_limit, decimal(18,0),>)
    End Sub
End Class
