<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false"
    CodeFile="ticket_payment.aspx.vb" Inherits="ticket_payment" Title="Payment" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
  
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea">
        <div class="demoheading">
            Payment
        </div>

        <div style="width: 1200px">

            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <asp:UpdatePanel ID="upGv" runat="server">
                            <ContentTemplate>
                             
                                <asp:GridView ID="gv" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                    BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                    CellPadding="3" DataSourceID="objSrc" GridLines="Vertical" DataKeyNames="payment_id"
                                    Width="100%">
                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                    <AlternatingRowStyle BackColor="Gainsboro" />
                                    <Columns>
                                        <asp:BoundField DataField="payment_id" HeaderText="Payment_ID" />
                                        <asp:BoundField DataField="created_date" HeaderText="Create Date"  DataFormatString="{0:d/M/yyyy}" />
                                        <asp:BoundField DataField="created_by" HeaderText="Created_By" />
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:HyperLink ID="linkDetail" runat="server">Detail</asp:HyperLink>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                    <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                </asp:GridView>
                                <asp:ObjectDataSource ID="objSrc" runat="server" OldValuesParameterFormatString="original_{0}"
                                    SelectMethod="getPayment" TypeName="clsFng">
                                </asp:ObjectDataSource>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>


        </div>
    </div>
</asp:Content>
