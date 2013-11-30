<%@ Page Language="VB" AutoEventWireup="false" CodeFile="customer_grid.aspx.vb" Inherits="customer_grid" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Customer Search</title>
    <link href="main.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="demoarea">
        <asp:ScriptManager ID="SMCustomerPopup" runat="server">
        </asp:ScriptManager>
        <asp:UpdatePanel ID="upnUser" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gvCust" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                    BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                    CellPadding="3" DataKeyNames="cust_id" DataSourceID="objSrcCust" GridLines="Vertical">
                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                    <Columns>
                        <asp:CommandField ButtonType="Image" SelectImageUrl="~/images/i_edit.gif" ShowSelectButton="True"
                            Visible="False" />
                        <asp:BoundField DataField="cust_id" HeaderText="Ref_no" ReadOnly="True" SortExpression="cust_id" />
                        <asp:BoundField DataField="cust_type_id" HeaderText="cust_type" SortExpression="cust_type_id" />
                        <asp:BoundField DataField="firstname" HeaderText="firstname" SortExpression="firstname" />
                        <asp:BoundField DataField="lastname" HeaderText="lastname" SortExpression="lastname" />
                        <asp:BoundField DataField="account_no" HeaderText="account_no" SortExpression="account_no" />
                        <asp:BoundField DataField="tel" HeaderText="tel" SortExpression="tel" />
                        <asp:BoundField DataField="margin" HeaderText="margin Call(%)" SortExpression="margin">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="cash_credit" HeaderText="cash_credit(baht)" DataFormatString="{0:#,##0.00}"
                            SortExpression="cash_credit" Visible="False">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="gold_credit" HeaderText="gold_credit(baht)" SortExpression="gold_credit"
                            DataFormatString="{0:#,##0.00}" Visible="False">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="cash_balence">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="credit(baht)" DataFormatString="{0:#,##0.00}">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="paid" HeaderText="paid(baht)" 
                            DataFormatString="{0:#,##0.00}">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="profit-loss" />
                    </Columns>
                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="Gainsboro" />
                </asp:GridView>
                <%--<asp:GridView ID="gvCust" runat="server" Width="99%"
                    AutoGenerateColumns="False" BackColor="White" BorderColor="#999999" BorderStyle="None"
                    BorderWidth="1px" CellPadding="3" DataKeyNames="cust_id" DataSourceID="objSrcCust"
                    GridLines="Vertical">
                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                    <Columns>
                        <asp:CommandField ButtonType="Image" SelectImageUrl="~/images/i_edit.gif" 
                            ShowSelectButton="True" Visible="False" />
                        <asp:BoundField DataField="cust_id" HeaderText="Ref_no" ReadOnly="True" 
                            SortExpression="cust_id" InsertVisible="False" />
                        <asp:BoundField DataField="cust_type_id" HeaderText="cust_type" 
                            SortExpression="cust_type_id" />
                        <asp:BoundField DataField="firstname" HeaderText="firstname" 
                            SortExpression="firstname" />
                        <asp:BoundField DataField="lastname" HeaderText="lastname" 
                            SortExpression="lastname" />
                        <asp:BoundField DataField="account_no" HeaderText="account_no" 
                            SortExpression="account_no" />
                             <asp:BoundField DataField="tel" HeaderText="tel" SortExpression="tel" />
                        <asp:BoundField DataField="margin" HeaderText="margin(%)" 
                            SortExpression="margin">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="cash_credit" HeaderText="cash_credit(baht)" 
                            SortExpression="cash_credit" Visible="False">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="gold_credit" HeaderText="gold_credit(baht) " 
                            SortExpression="gold_credit" Visible="False" >
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="credit_guarantee" HeaderText="credit(baht)"  DataFormatString="{0:#,##0.00}"
                            SortExpression="credit_guarantee">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                    </Columns>
                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="Gainsboro" />
                </asp:GridView>--%>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:ObjectDataSource ID="objSrcCust" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="getCustomerByCust_id" TypeName="clsDB">
            <SelectParameters>
                <asp:Parameter ConvertEmptyStringToNull="False" Name="cust_id" Type="String" />
                <asp:Parameter ConvertEmptyStringToNull="False" Name="cust_name" 
                    Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
    </form>
</body>
</html>
