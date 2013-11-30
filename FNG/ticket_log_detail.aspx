<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false"
    CodeFile="ticket_log_detail.aspx.vb" Inherits="ticket_log_detail" Title="Tickets Log Detail" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea" style="height: 800px">
        <asp:UpdatePanel ID="upnUser" runat="server">
            <ContentTemplate>
                <table border="0" width="100%">
                    <tr>
                        <td align="left">
                            <div class="demoheading">
                                Tickets Log Detail</div>
                        </td>
                        <td align="right">
                        </td>
                    </tr>
                </table>
                   <div align="left" style="padding-bottom: 5px">
                    <table cellpadding="1" cellspacing="1">
                        <tr>
                            <td align="right">
                            Ref_No : 
                            </td>
                            <td>
                            <asp:Label ID="lblRef_no" runat="server" Font-Bold=true ForeColor="Black" ></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right">
                                Created By :</td>
                            <td>
                                <asp:Label ID="lblCreatedBy" runat="server" Font-Bold=true ForeColor="Black"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td align="right">
                                Created Date :</td>
                            <td>
                                <asp:Label ID="lblCreatedDate" runat="server" Font-Bold=true 
                                    ForeColor="Black"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                        </tr>
                    </table>
                    
                    </div>
                <div style="width: 1000px">
                </div>
                <asp:GridView ID="gvLog" runat="server" AutoGenerateColumns="False" BackColor="White"
                    BorderColor="#999999" BorderStyle="None" BorderWidth="1px" Width="100%" CellPadding="3"
                    DataSourceID="objSrcLog" GridLines="Vertical" PageSize="30">
                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                    <Columns>
                        <asp:TemplateField HeaderText="Ref_No" Visible="False">
                            <ItemTemplate>
                                <asp:HyperLink ID="link" runat="server" Target="_blank"><%#Eval("ref_no")%></asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:TemplateField>
                       <%-- <asp:TemplateField HeaderText="Cust_Name">
                            <ItemTemplate>
                                <asp:HyperLink ID="linkCust" runat="server" Target="_blank"><%#Eval("firstname")%></asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        </asp:TemplateField>--%>
                         <asp:BoundField DataField="firstname" HeaderText="Cust_Name" >
                             <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="book_no" HeaderText="Book_No" />
                        <asp:BoundField DataField="run_no" HeaderText="Run_No" />
                        <asp:BoundField DataField="invoice" HeaderText="Invoice" />
                        <asp:BoundField DataField="gold_type_id" HeaderText="Purity" />
                        <asp:BoundField DataField="delivery" HeaderText="Delivery" />
                        <asp:BoundField DataField="type" HeaderText="Buy/Sell" />
                        <asp:BoundField DataField="quantity" HeaderText="Quantity" DataFormatString="{0:#,##0.00000}">
                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="price" HeaderText="Price" DataFormatString="{0:#,##0.00}">
                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="amount" HeaderText="Amount" DataFormatString="{0:#,##0.00}">
                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="delivery_date" HeaderText="Delivery_Date" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="ticket_date" HeaderText="Ticket_Date" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="billing" HeaderText="Billing" />
                        <asp:BoundField DataField="remark" HeaderText="Remark" />
                        <asp:BoundField DataField="payment" HeaderText="Payment" />
                        <asp:BoundField DataField="payment_bank" HeaderText="Bank" />
                        <asp:BoundField DataField="payment_duedate" HeaderText="Duedate" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="payment_cheq_no" HeaderText="Cheq_No" />
                        <asp:BoundField DataField="status_name" HeaderText="Status">
                            <ItemStyle Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField DataField="modifier_date" HeaderText="Update_Date" DataFormatString="{0:dd/MM/yyyy}" />
                        <asp:BoundField DataField="update_by" HeaderText="Update_By" />
                        <asp:BoundField DataField="active" HeaderText="Active" />
                    </Columns>
                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="Gainsboro" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:ObjectDataSource ID="objSrcLog" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="getTicketLogUpdate" TypeName="clsDB">
            <SelectParameters>
                <asp:Parameter ConvertEmptyStringToNull="False" Name="ref_no" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
    <div class="demobottom">
    </div>
</asp:Content>
