<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false" 
    CodeFile="customers.aspx.vb" Inherits="customers" Title="CustomerManagement" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
    .cssWatermark
    {
        background-color:#FFFFCA;
        font-style:italic;  
    }
    .search
    {
    background:#FFFFCA url(images/search.bmp) no-repeat 2px 2px;
	
    }
    .unwatermarked 
    {
	height:16px;
	width:148px;
}

.watermarked {
	height:18px;
	width:150px;
	padding:2px 0 0 2px;
	border:1px solid #BEBEBE;
	background-color:#F0F8FF;
	color:gray;
}	
th.sortasc a     
{   
    display:block; padding:0 4px 0 15px;  
    background:url(images/arrow_up.gif) no-repeat; 
}   
th.sortdesc a   
{   
    display:block; 
    padding:0 4px 0 15px;   
    background:url(images/arrow_down.gif) no-repeat; 
}
.gvPagerCss span    { font-weight:bold; text-decoration:underline; }      
.gvPagerCss td    {padding-left: 5px;padding-right: 5px;      }
</style>
  
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea">
        <asp:UpdatePanel ID="upnUser" runat="server">
            <%-- <Triggers>
                <asp:AsyncPostBackTrigger ControlID="imgSearch" />
            </Triggers>--%>
            <ContentTemplate>
                <table border="0" width="100%">
                    <tr>
                        <td align="left">
                            <div class="demoheading">
                                Customers
                            </div>
                        </td>
                        <td align="right" width="80%">
                            <asp:Panel ID="pnSearch" runat="server" DefaultButton="imgSearch">
                                <table border="0" cellpadding="1" cellspacing="1">
                                    <tr>
                                        <td>
                                        Created_Date From
                                            <asp:TextBox ID="txtDate" Width="100px" runat="server" CssClass="unwatermarked"></asp:TextBox>
                                            <%--<ajaxToolkit:TextBoxWatermarkExtender ID="txtDate_TextBoxWatermarkExtender" 
                                                runat="server" Enabled="True" TargetControlID="txtDate" WatermarkCssClass="watermarked" WatermarkText="Created_date">
                                            </ajaxToolkit:TextBoxWatermarkExtender>--%>
                                            <ajaxToolkit:CalendarExtender ID="txtDate_CalendarExtender" runat="server" Enabled="True" PopupButtonID="imgCalDate"
                                                TargetControlID="txtDate">
                                            </ajaxToolkit:CalendarExtender>
                                            <asp:ImageButton ID="imgCalDate" runat="server" 
                                                ImageUrl="~/images/Calendar_scheduleHS.png" />
                                            To
                                            <asp:TextBox ID="txtDate2" Width="100px" runat="server" CssClass="unwatermarked"></asp:TextBox>
                                            <%--<ajaxToolkit:TextBoxWatermarkExtender ID="txtDate2_TextBoxWatermarkExtender" 
                                                runat="server" Enabled="True" TargetControlID="txtDate2" WatermarkCssClass="watermarked" WatermarkText="Created_date">
                                            </ajaxToolkit:TextBoxWatermarkExtender>--%>
                                             <ajaxToolkit:CalendarExtender ID="txtDate2_CalendarExtender" runat="server" Enabled="True" PopupButtonID="imgCalDate2"
                                                TargetControlID="txtDate2">
                                            </ajaxToolkit:CalendarExtender>
                                            <asp:ImageButton ID="imgCalDate2" runat="server" 
                                                ImageUrl="~/images/Calendar_scheduleHS.png" />
                                        </td>
                                        <td>
                                            <asp:RadioButtonList ID="rdoTradeOnline" runat="server" RepeatDirection="Horizontal">
                                                <asp:ListItem Selected="True" Text="All" Value=""></asp:ListItem>
                                                <asp:ListItem Text="Call" Value="y"></asp:ListItem>
                                                <asp:ListItem Text="Online" Value="n"></asp:ListItem>
                                            </asp:RadioButtonList>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtSearch" runat="server" CssClass="unwatermarked"></asp:TextBox>
                                        </td>
                                        <td style="display:none">
                                            <asp:DropDownList ID="ddlTeam" runat="server" Height="20px">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            <asp:CheckBox ID="cbSelf" runat="server" Text="Self" />
                                        </td>
                                        <td>
                                            <asp:ImageButton ID="imgSearch" runat="server" ImageUrl="~/images/search.bmp" Style="height: 13px" />
                                        </td>
                                    </tr>
                                </table>
                            </asp:Panel>
                        </td>
                    </tr>
                </table>
                <br />
                <div align="left" style="padding-bottom: 5px">
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td>
                                <asp:LinkButton ID="linkCust" PostBackUrl="~/customer_detail.aspx" runat="server">New Customer</asp:LinkButton>
                            </td>
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
                        </tr>
                    </table>
                </div>
                               
                
                <asp:GridView ID="gvCust" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                    BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                    Width="100%" CellPadding="3" DataKeyNames="cust_id" GridLines="Vertical" PageSize="30">
                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                    <Columns>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="linkFolio" runat="server" OnClick="linkFolio_Click">Portfolio</asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="40px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="linkDepositCash" runat="server" OnClick="linkDeposit_Click">Doposit</asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="40px" />
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="linkDealTicket" runat="server" OnClick="linkDealTicket_Click">Tickets</asp:LinkButton>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="40px" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="cust_id" HeaderText="Ref_no" ReadOnly="True" SortExpression="cust_id">
                            <ItemStyle HorizontalAlign="Center" />
                        </asp:BoundField>
                        <asp:BoundField DataField="cust_type" HeaderText="Cust_type" SortExpression="cust_type" />
                        <asp:TemplateField HeaderText="Firstname" SortExpression="Firstname">
                            <ItemTemplate>
                                <asp:HyperLink ID="linkCust" runat="server" Font-Underline="false" Target="_blank"><%#Eval("firstname")%></asp:HyperLink>
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                        </asp:TemplateField>
                        <asp:BoundField DataField="lastname" HeaderText="Lastname" SortExpression="lastname" />
                        <asp:BoundField DataField="tel" HeaderText="Tel" SortExpression="tel" />
                        <asp:BoundField DataField="trade_type" HeaderText="Call/Online" SortExpression="trade_type" />
                        <asp:BoundField DataField="created_date" HeaderText="Created_date" SortExpression="Created_date"
                            DataFormatString="{0:dd/MM/yyyy}" />
                        <%--<asp:BoundField HeaderText="Revaluation" DataField="revaluation" DataFormatString="{0:#,##0.00}"
                            SortExpression="revaluation">
                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="NetEquity" DataField="netequity" SortExpression="netequity"
                            DataFormatString="{0:#,##0.00}">
                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                        </asp:BoundField>
                        <asp:BoundField HeaderText="Excess/Short" DataField="excess" SortExpression="excess"
                            DataFormatString="{0:#,##0.00}">
                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                        </asp:BoundField>--%>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgDel" runat="server" ImageUrl="~/images/i_del.png" OnClick="imgDel_Click" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="20px" />
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" CssClass="gvPagerCss" />
                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="Gainsboro" />
                    <SortedAscendingHeaderStyle CssClass="sortasc" />
                    <SortedDescendingHeaderStyle CssClass="sortdesc" />
                </asp:GridView>
                <asp:ObjectDataSource ID="objSrcCust" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="getCustomer" TypeName="clsDB">
                    <SelectParameters>
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="str" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="isCall" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="user_id" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="team_id" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="created_date" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="created_date2" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="demobottom">
    </div>
</asp:Content>
