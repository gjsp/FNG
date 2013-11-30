<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false"
    CodeFile="stock_deposit.aspx.vb" Inherits="stock_deposit" Title="Deposit Stock" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style2
        {
            width: 56px;
        }
        .style3
        {
            width: 44px;
        }
    </style>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea">
        <div class="demoheading">
            Deposit Stock</div>
            <div style="width: 1200px">
                <asp:UpdatePanel ID="udpStock" runat="server" RenderMode="Inline">
                    <ContentTemplate>
                        <fieldset>
                            <legend class="topic">Summary</legend>
                            <table border="0" cellpadding="0" cellspacing="5">
                                <tr>
                                    <td valign="top">
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Net Cash :
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblNetCash" runat="server" CssClass="lblBold"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Net 96 :
                                        <asp:Label ID="lblNet96" runat="server" CssClass="lblBold"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Net 99N :
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblNet99N" runat="server" CssClass="lblBold"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Net 99L :
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblNet99L" runat="server" CssClass="lblBold"></asp:Label>
                                    </td>
                                    <td valign="top" class="style3">
                                        &nbsp;</td>
                                    <td class="style2" valign="top" align="right">
                                        Team :</td>
                                    <td valign="top">
                                        <asp:DropDownList ID="ddlTeam" runat="server">
                                        </asp:DropDownList>
                                        <div style="display:none">
                                        <asp:Button ID="btnSearch" runat="server" Text="Search" /></div>
                                    </td>
                                    <td valign="top" style="height:30">
                                     <asp:UpdateProgress ID="upgSearch" runat="server">
                                    <ProgressTemplate>
                                        <div align="left" style="margin-left: 1cm; overflow: auto">
                                            <img alt="" src="images/loading.gif" />
                                            Loading...
                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                     </td>
                                </tr>
                            </table>
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        <div style="width: 1200px">
               
                        
                      
                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upGv" runat="server">
                                            <ContentTemplate>
                                           <%-- <table>
                                                    <tr>
                                                        <td>
                                                            &nbsp;</td>
                                                        <td class="style17">
                                                            &nbsp;</td>
                                                        <td class="style16">
                                                            <asp:UpdateProgress ID="upgSearch" runat="server">
                                                                <ProgressTemplate>
                                                                    <div align="left" style="margin-left: 1cm; overflow: auto">
                                                                        <img alt="" src="images/loading.gif" /> Loading...
                                                                    </div>
                                                                </ProgressTemplate>
                                                            </asp:UpdateProgress>
                                                        </td>
                                                    </tr>
                                                </table>--%>
                                                <asp:GridView ID="gvDeposit" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                    BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                                    CellPadding="3" DataKeyNames="cust_id" DataSourceID="objSrcDep" GridLines="Vertical"
                                                    Width="100%">
                                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                                    <AlternatingRowStyle BackColor="Gainsboro" />
                                                    <Columns>
                                                        <asp:BoundField DataField="cust_id" HeaderText="Cust_id" 
                                                            SortExpression="cust_id" >
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Customer_Name" SortExpression="firstname">
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="linkCust" runat="server" Target="_blank"><%#Eval("firstname")%></asp:HyperLink></ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="cash" HeaderText="Cash" SortExpression="Cash" DataFormatString="{0:#,##0.00}">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="G96" HeaderText="Gold_96" SortExpression="G96" DataFormatString="{0:#,##0.00000}" >
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="G99N" HeaderText="Gold_99N" SortExpression="G99N" DataFormatString="{0:#,##0.00000}" >
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="G99L" HeaderText="Gold_99L" SortExpression="G99L" DataFormatString="{0:#,##0.00000}" >
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                    <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                                </asp:GridView>
                                                <asp:ObjectDataSource ID="objSrcDep" runat="server" OldValuesParameterFormatString="original_{0}"
                                                    SelectMethod="getStockDeposit" TypeName="clsDB">
                                                    <SelectParameters>
                                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="team_id" Type="String" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        
                    
            </div>
    </div>
</asp:Content>
