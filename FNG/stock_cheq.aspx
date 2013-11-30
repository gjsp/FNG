<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false"
    CodeFile="stock_cheq.aspx.vb" Inherits="stock_cheq" Title="Stock Cheque Payment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea">
        <div class="demoheading">
            Deposit Stock Update</div>
        <div>
            <div style="width: 1200px">
            <asp:UpdatePanel ID="udpGv" runat="server">
                <ContentTemplate>
                <asp:GridView ID="gvCheq" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                    BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                    CellPadding="3" DataSourceID="objSrcCheq" GridLines="Vertical"
                    Width="100%">
                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                    <AlternatingRowStyle BackColor="Gainsboro" />
                    <Columns>
                        <asp:BoundField DataField="mode" HeaderText="Type" />
                        <asp:BoundField DataField="firstname" HeaderText="Cust_Name" />
                        <asp:BoundField DataField="amount" DataFormatString="{0:#,##0.00}" HeaderText="Amount(baht)"
                            SortExpression="amount">
                            <ItemStyle HorizontalAlign="Right" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Bank" DataField="bank_name" 
                            SortExpression="bank_name">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="payment_cheq_no" HeaderText="Cheque_No" 
                            SortExpression="payment_cheq_no">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="payment_duedate" HeaderText="Duedate_Cheque" 
                            DataFormatString="{0:d/M/yyyy}" SortExpression="payment_duedate">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:TemplateField HeaderText="Status">
                            <ItemTemplate>
                                <asp:Label ID="lblStatus" runat="server"></asp:Label>
                                <select id="ddlStatus" runat="server" visible="False">
                                    <option value="0" style="color:#003300 ">รอรับเงิน</option>
                                    <option value="1" style="color: Green">รับเงินแล้ว</option>
                                </select>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="100px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="linkUpdate" runat="server" OnClick="linkUpdate_Click">Update</asp:LinkButton>
                                <asp:LinkButton ID="linkSave" Visible="false" runat="server" OnClick="linkSave_Click">Save</asp:LinkButton>
                                &nbsp;
                                <asp:LinkButton ID="linkCancel" Visible="false" runat="server" OnClick="linkCancel_Click">Cancel</asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                </asp:GridView>
                <asp:ObjectDataSource ID="objSrcCheq" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="getTicketCheq" TypeName="clsDB"></asp:ObjectDataSource>
                </ContentTemplate>
            </asp:UpdatePanel>
                
            </div>
        </div>
    </div>
</asp:Content>
