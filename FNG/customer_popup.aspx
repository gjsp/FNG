<%@ Page Language="VB" AutoEventWireup="false" CodeFile="customer_popup.aspx.vb"
    Inherits="customer_popup" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Customer Search</title>
    <link href="main.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function getStringCustomer(strCust)
        {
            if (opener.document.getElementById('ctl00_SampleContent_txtCustName').value !='')
            {
                opener.document.getElementById('ctl00_SampleContent_txtCustName').value = opener.document.getElementById('ctl00_SampleContent_txtCustName').value + '&' + strCust;
            }
            else
            {
                opener.document.getElementById('ctl00_SampleContent_txtCustName').value = strCust;
            }
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="SMCustomerPopup" runat="server">
        </asp:ScriptManager>
    <div class="demoarea">
        
        <asp:UpdatePanel ID="upnUser" runat="server">
            <ContentTemplate>
                <table border="0">
                    <tr>
                        <td>
                            Ref No :
                        </td>
                        <td>
                            <asp:TextBox ID="txtSearch" runat="server" />
                        </td>
                        <td>
                            <asp:ImageButton ID="imgSearch" runat="server" ImageUrl="~/images/search.bmp" Style="height: 13px" />
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
                <asp:GridView ID="gvCust" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                    BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                    CellPadding="3" DataKeyNames="cust_id" DataSourceID="objSrcCust" GridLines="Vertical">
                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                    <Columns>
                        <asp:CommandField ButtonType="Image" SelectImageUrl="~/images/i_detail.png" ShowSelectButton="True" />
                        <asp:BoundField DataField="cust_id" HeaderText="Ref_no" ReadOnly="True" SortExpression="cust_id" />
                        <asp:BoundField DataField="cust_type" HeaderText="cust_type" SortExpression="cust_type" />
                        <asp:BoundField DataField="firstname" HeaderText="firstname" SortExpression="firstname" />
                        <asp:BoundField DataField="lastname" HeaderText="lastname" SortExpression="lastname" />
                        <asp:BoundField DataField="account_no1" HeaderText="account_no" SortExpression="account_no1" />
                        <asp:BoundField DataField="tel" HeaderText="tel" SortExpression="tel" />
                        <asp:BoundField DataField="margin" HeaderText="margin_call(%)" SortExpression="margin">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="margin_call" HeaderText="force_close_level(%)" SortExpression="margin_call">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                    </Columns>
                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="Gainsboro" />
                </asp:GridView>
                <asp:ObjectDataSource ID="objSrcCust" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="searchCustomer" TypeName="clsDB">
            <SelectParameters>
                <asp:ControlParameter ControlID="txtSearch" ConvertEmptyStringToNull="False" Name="str"
                    PropertyName="Text" Type="String" />
                <asp:Parameter ConvertEmptyStringToNull="False" Name="user_id" Type="String" DefaultValue="" />
            </SelectParameters>
        </asp:ObjectDataSource>
            </ContentTemplate>
        </asp:UpdatePanel>
        
    </div>
    </form>
</body>
</html>
