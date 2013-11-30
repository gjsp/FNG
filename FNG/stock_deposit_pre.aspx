<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false"
    CodeFile="stock_deposit_pre.aspx.vb" Inherits="stock_deposit_pre" Title="Deposit Stock Update" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style16
        {
            width: 237px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea">
        <div class="demoheading">
            Deposit Stock Update</div>
        <div>
            <div style="width: 1200px">
                <cc1:TabContainer ID="tapImport" runat="server" ActiveTabIndex="1" Width="100%">
                    <cc1:TabPanel ID="tpCash" runat="server" HeaderText="Cash">
                        <HeaderTemplate>
                            Cash
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upGv" runat="server">
                                            <ContentTemplate>
                                                <table cellpadding="2" cellspacing="2">
                                                    <tr>
                                                        <td>
                                                            Date From
                                                            <asp:TextBox ID="txtCDate1" runat="server" Width="70px" />
                                                            <ajaxToolkit:CalendarExtender ID="txtCDate1_CalendarExtender" runat="server" Enabled="True"
                                                                PopupPosition="BottomRight" TargetControlID="txtCDate1" PopupButtonID="imgCCalDate1">
                                                            </ajaxToolkit:CalendarExtender>
                                                            <asp:ImageButton ID="imgCCalDate1" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                                            &nbsp; To
                                                            <asp:TextBox ID="txtCDate2" runat="server" Width="70px" />
                                                            <asp:ImageButton ID="imgCCalDate2" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                                            <ajaxToolkit:CalendarExtender ID="txtCDate2_CalendarExtender" runat="server" Enabled="True"
                                                                PopupButtonID="imgCCalDate2" PopupPosition="BottomRight" TargetControlID="txtCDate2">
                                                            </ajaxToolkit:CalendarExtender>
                                                        </td>
                                                        <td style="margin-left: 1cm; width: 20px; text-align: right">
                                                            &nbsp;<asp:ImageButton ID="imgCashSearch" runat="server" ImageUrl="~/images/search.bmp"
                                                                Style="width: 12px; height: 13px;" />
                                                        </td>
                                                        <td class="style16">
                                                            <asp:UpdateProgress ID="upgCSearch" runat="server">
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
                                                <asp:GridView ID="gvAssetCash" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                    BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                                    CellPadding="3" DataKeyNames="cust_tran_id" GridLines="Vertical"
                                                    Width="100%">
                                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                                    <AlternatingRowStyle BackColor="Gainsboro" />
                                                    <Columns>
                                                        <%--<asp:BoundField DataField="remark" HeaderText="remark" />--%>
                                                        <%--<asp:BoundField DataField="amount" HeaderText="Amount" 
                                                            DataFormatString="{0:#,##0.00}">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>--%>
                                                        <asp:BoundField DataField="datetime" HeaderText="Datetime" DataFormatString="{0:dd/MM/yyyy}"
                                                            SortExpression="datetime">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Customer_Name" SortExpression="firstname">
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="linkCust" runat="server" Target="_blank"><%#Eval("firstname")%></asp:HyperLink></ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="type" HeaderText="Type" SortExpression="type">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="user_name" HeaderText="created_by" SortExpression="user_name">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Remark" SortExpression="remark">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNote" runat="server" Text='<%# Bind("remark") %>'></asp:Label>
                                                                <asp:TextBox ID="txtNote" runat="server" Text='<%# Bind("remark") %>' Visible="False"
                                                                    Width="180px"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="150px" Wrap="False" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Amount" SortExpression="amount">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblQuantity" runat="server"></asp:Label>
                                                                <asp:TextBox ID="txtQuantity" runat="server" Visible="false" MaxLength="12" Width="100px"
                                                                    Wrap="False"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" Width="100px" Wrap="False" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="linkUpdate" runat="server" OnClick="linkUpdate_Click">Update</asp:LinkButton>
                                                                <asp:LinkButton ID="linkSave" Visible="false" runat="server" OnClick="linkSave_Click">Save</asp:LinkButton>&nbsp;
                                                                <asp:LinkButton ID="linkCancel" Visible="false" runat="server" OnClick="linkCancel_Click">Cancel</asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                    <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                                </asp:GridView>
                                                <asp:ObjectDataSource ID="objSrcAssetCash" runat="server" OldValuesParameterFormatString="original_{0}"
                                                    SelectMethod="getStockDepositPre" TypeName="clsDB">
                                                    <SelectParameters>
                                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="purity" Type="String" />
                                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="date1" Type="String" />
                                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="date2" Type="String" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                    <cc1:TabPanel ID="tpGold" runat="server" HeaderText="Gold">
                        <HeaderTemplate>
                            Gold
                        </HeaderTemplate>
                        <ContentTemplate>
                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <ContentTemplate>
                                                <table cellpadding="2" cellspacing="2">
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBoxList ID="cblPurity" runat="server" RepeatDirection="Horizontal">
                                                                <asp:ListItem Value="96">96.50</asp:ListItem>
                                                                <asp:ListItem Value="96M">96.50 Mini</asp:ListItem>
                                                                <asp:ListItem Value="96G">96.50 Gram</asp:ListItem>
                                                                <asp:ListItem Value="99">99.99</asp:ListItem>
                                                            </asp:CheckBoxList>
                                                        </td>
                                                        <td>
                                                            Date From
                                                            <asp:TextBox ID="txtDate1" runat="server" Width="70px" />
                                                            <ajaxToolkit:CalendarExtender ID="txtDate1_CalendarExtender" runat="server" Enabled="True"
                                                                PopupPosition="BottomRight" TargetControlID="txtDate1" PopupButtonID="imgCalDate1">
                                                            </ajaxToolkit:CalendarExtender>
                                                            <asp:ImageButton ID="imgCalDate1" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                                            &nbsp; To
                                                            <asp:TextBox ID="txtDate2" runat="server" Width="70px" />
                                                            <asp:ImageButton ID="imgCalDate2" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                                            <ajaxToolkit:CalendarExtender ID="txtDate2_CalendarExtender" runat="server" Enabled="True"
                                                                PopupButtonID="imgCalDate2" PopupPosition="BottomRight" TargetControlID="txtDate2">
                                                            </ajaxToolkit:CalendarExtender>
                                                        </td>
                                                        <td style="margin-left: 1cm; width: 20px; text-align: right">
                                                            &nbsp;<asp:ImageButton ID="imgSearch" runat="server" ImageUrl="~/images/search.bmp"
                                                                OnClick="imgSearch_Click" Style="width: 12px;" />
                                                        </td>
                                                        <td class="style16">
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
                                                <asp:GridView ID="gvAssetGold96" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                    BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                                    CellPadding="3" DataKeyNames="cust_tran_id" GridLines="Vertical" Width="100%">
                                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                                    <AlternatingRowStyle BackColor="Gainsboro" />
                                                    <Columns>
                                                        <%--<asp:BoundField DataField="datetime" HeaderText="Datetime" DataFormatString="{0:dd/MM/yyyy}"
                                                            SortExpression="datetime">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>--%>
                                                        <asp:TemplateField HeaderText="Datetime" SortExpression="datetime">
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="lblTrans" runat="server" onclick="lblTrans_Click"></asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                        <asp:TemplateField HeaderText="Customer_Name" SortExpression="firstname">
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="linkCust" runat="server" Target="_blank"><%#Eval("firstname")%></asp:HyperLink></ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="type" HeaderText="Type" SortExpression="type">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="gold_type_id" HeaderText="Purity" SortExpression="gold_type_id">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="user_name" HeaderText="created_by" SortExpression="user_name">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Remark" SortExpression="remark">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblNote" runat="server" Text='<%# Bind("remark") %>'></asp:Label>
                                                                <asp:TextBox ID="txtNote" runat="server" Text='<%# Bind("remark") %>' Visible="False"
                                                                    Width="180px"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ItemStyle Width="150px" Wrap="False" />
                                                        </asp:TemplateField>
                                                        <%--<asp:BoundField DataField="quantity" HeaderText="Quantity" DataFormatString="{0:#,##0.00000}">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>--%>
                                                        <asp:TemplateField HeaderText="Quantity" SortExpression="quantity">
                                                            <ItemTemplate>
                                                                <asp:Label ID="lblQuantity" runat="server" Text='<%# Bind("quantity") %>'></asp:Label>
                                                                <asp:TextBox ID="txtQuantity" runat="server" Visible="false" MaxLength="12" Text='<%# Bind("quantity") %>'
                                                                    Width="100px" Wrap="False"></asp:TextBox>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Right" Width="100px" Wrap="False" />
                                                        </asp:TemplateField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="linkUpdate" runat="server" OnClick="linkUpdate_Click">Update</asp:LinkButton>
                                                                <asp:LinkButton ID="linkSave" Visible="false" runat="server" OnClick="linkSave_Click">Save</asp:LinkButton>&nbsp;
                                                                <asp:LinkButton ID="linkCancel" Visible="false" runat="server" OnClick="linkCancel_Click">Cancel</asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                    <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                                </asp:GridView>
                                                <asp:ObjectDataSource ID="objSrcAssetGold96" runat="server" OldValuesParameterFormatString="original_{0}"
                                                    SelectMethod="getStockDepositPre" TypeName="clsDB">
                                                    <SelectParameters>
                                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="purity" Type="String" />
                                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="date1" Type="String" />
                                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="date2" Type="String" />
                                                    </SelectParameters>
                                                </asp:ObjectDataSource>
                                            </ContentTemplate>
                                        </asp:UpdatePanel>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </cc1:TabPanel>
                </cc1:TabContainer>
            </div>
        </div>
    </div>
</asp:Content>
