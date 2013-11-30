<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false"
    CodeFile="ticket_overclearing.aspx.vb" Inherits="ticket_overclearing" Title="ticket Overclearing" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .cssWatermark
        {
            background-color: #FFFFCA;
            font-style: italic;
        }
        .search
        {
            background: #FFFFCA url(images/search.bmp) no-repeat 2px 2px;
        }
        .unwatermarked
        {
            height: 16px;
            width: 148px;
        }
        
        .watermarked
        {
            height: 18px;
            width: 150px;
            padding: 2px 0 0 2px;
            border: 1px solid #BEBEBE;
            background-color: #F0F8FF;
            color: gray;
        }
        th.sortasc a
        {
            display: block;
            padding: 0 4px 0 15px;
            background: url(images/arrow_up.gif) no-repeat;
        }
        th.sortdesc a
        {
            display: block;
            padding: 0 4px 0 15px;
            background: url(images/arrow_down.gif) no-repeat;
        }
        .gvPagerCss span
        {
            font-weight: bold;
            text-decoration: underline;
        }
        .gvPagerCss td
        {
            padding-left: 5px;
            padding-right: 5px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea">
        <asp:UpdatePanel ID="upnUser" runat="server">
            <ContentTemplate>
                <table border="0" width="100%">
                    <tr>
                        <td align="left">
                            <div class="demoheading">
                                Over Clearing </div>
                        </td>
                        <td align="right" width="80%">
                            <asp:Panel ID="pnSearch" runat="server" DefaultButton="btnSearch">
                                <table border="0" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td style="height: 30px">
                                            <asp:UpdateProgress ID="upgSearch" runat="server">
                                                <ProgressTemplate>
                                                    <div align="left" style="margin-left: 1cm; overflow: auto">
                                                        <img alt="" src="images/loading.gif" />
                                                        Loading...
                                                    </div>
                                                </ProgressTemplate>
                                            </asp:UpdateProgress>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            Over Clearing day :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlClearing" runat="server" Height="20px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                        </td>
                                        <td>
                                            <asp:Button ID="btnSearch" runat="server" Text="Search" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <br />
                <div align="left" style="padding-bottom: 5px">
                </div>
                <asp:GridView ID="gv" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                    BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                    Width="100%" CellPadding="3" GridLines="Vertical" PageSize="30" DataSourceID="objSrc">
                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                    <Columns>
                        <asp:TemplateField HeaderText="Date" SortExpression="ticket_date" ItemStyle-Wrap="false">
                            <ItemTemplate>
                                <asp:Label ID="lblDate" runat="server" Text='<%# Bind("ticket_date", "{0:d/M/yyyy hh:mm}") %>'></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Wrap="False" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Ref_No" SortExpression="ref_no">
                            <ItemTemplate>
                                <asp:HyperLink ID="link" runat="server" Target="_blank"><%#Eval("ref_no")%></asp:HyperLink>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Customer_Name" SortExpression="Firstname">
                            <ItemTemplate>
                                <asp:HyperLink ID="linkCust" runat="server" Target="_blank"><%#Eval("firstname")%></asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="clearingday" HeaderText="Clearing_Day T+" SortExpression="clearingday">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="over_clearingday" HeaderText="Over_Clearing_Day" SortExpression="over_clearingday">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="gold_type_name" HeaderText="Purity" SortExpression=" gold_type_name"
                            Visible="False" />
                        <asp:BoundField DataField="type" HeaderText="Type" SortExpression="type">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="delivery_date" HeaderText="Delivery_Date" SortExpression="delivery_date"
                            DataFormatString="{0:d/M/yyyy}" />
                        <asp:BoundField DataField="quantity" DataFormatString="{0:#,##0.00000}" HeaderText="Quantity"
                            SortExpression="quan96">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="price" HeaderText="Price" SortExpression="price" DataFormatString="{0:#,##0.00}">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField DataField="amount" HeaderText="Amount" SortExpression="amount" DataFormatString="{0:#,##0.00}">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                    </Columns>
                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" CssClass="gvPagerCss" />
                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="Gainsboro" />
                    <SortedAscendingHeaderStyle CssClass="sortasc" />
                    <SortedDescendingHeaderStyle CssClass="sortdesc" />
                </asp:GridView>
                <asp:ObjectDataSource ID="objSrc" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="getTicketOverClearing" TypeName="clsFng">
                    <SelectParameters>
                        <asp:ControlParameter ControlID="ddlClearing" Name="Overclearing" PropertyName="SelectedValue"
                            Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="demobottom">
    </div>
</asp:Content>
