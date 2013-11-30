<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false"
    EnableEventValidation="false" CodeFile="stock_actual.aspx.vb" Inherits="stock_actual"
    Title="Actual Stock" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript" type="text/javascript">
        function search() {
            $get('ctl00_SampleContent_imgSearch').click();
        }
        function setStockPage(client_id) {
            var i = 0;
            var planmode = '0';
            if ($get('ctl00_SampleContent_cbPlan').check == true) {
                planmode = '1';
            }
            var day = $get(client_id).value.split('/')[0];
            var month = $get(client_id).value.split('/')[1];
            var year = $get(client_id).value.split('/')[2];

            i = setPlanningDay(day, month, year);
            $get('ctl00_SampleContent_ifm').src = "stock_now.aspx?count=" + i + "&planMode=" + planmode + "";
        }
        function setPlanningDay(dd, MM, yyyy) {
            toDay = new Date();
            now = new Date(toDay.getFullYear(), toDay.getMonth(), toDay.getDate());
            iDate = new Date(yyyy, MM - 1, dd);

            var one_day = 1000 * 60 * 60 * 24;
            var defDate = (iDate.getTime() - now.getTime()) / one_day
            return defDate
        }
    </script>
    <style type="text/css">
        .style13
        {
            width: 116px;
            height: 27px;
        }
        .style14
        {
            height: 30px;
        }
        .style15
        {
            width: 89px;
            height: 27px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea" style="width: 1500px">
        <div class="demoheading">
            Actual Stock</div>
        <div>
            <asp:UpdatePanel ID="upd" runat="server" RenderMode="Inline">
                <ContentTemplate>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="style15">
                                Actual Day :
                            </td>
                            <td class="style13">
                                <asp:TextBox ID="txtDate" runat="server" Width="80px"></asp:TextBox>
                                <cc1:CalendarExtender ID="txtDate_CalendarExtender" runat="server" Enabled="True"
                                    PopupButtonID="imgDate" PopupPosition="BottomRight" TargetControlID="txtDate"
                                    OnClientDateSelectionChanged="search">
                                </cc1:CalendarExtender>
                                <asp:ImageButton ID="imgDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                            </td>
                            <td class="style14">
                                &nbsp;
                                <asp:ImageButton ID="imgSearch" runat="server" ImageUrl="~/images/search.bmp" OnClick="imgSearch_Click"
                                    Style="width: 12px;" />
                            </td>
                            <td class="style14">
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
                </ContentTemplate>
            </asp:UpdatePanel>
            <div>
                <asp:UpdatePanel ID="udpStock" runat="server" RenderMode="Inline">
                    <ContentTemplate>
                        <fieldset>
                            <legend class="topic">Stocks</legend>
                            <table border="0" cellpadding="0" cellspacing="5">
                                <tr>
                                    <td valign="top">
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Net Cash :
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblNetCash" runat="server" CssClass="lblBold"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Net Trans :
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblNetTrans" runat="server" CssClass="lblBold"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Net Cheq :
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblNetCheq" runat="server" CssClass="lblBold"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Net :
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblNetMoney" runat="server" CssClass="lblBold"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Net 96 :
                                        <asp:Label ID="lblNet96" runat="server" CssClass="lblBold"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Net 96G :
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblNet96G" runat="server" CssClass="lblBold"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Net 99 :
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblNet99" runat="server" CssClass="lblBold"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div>
                <cc1:TabContainer ID="tapImport" runat="server" ActiveTabIndex="0" Width="100%">
                    <cc1:TabPanel ID="tpCash" runat="server" HeaderText="Cash" >
                        <HeaderTemplate>
                            Cash</HeaderTemplate>
                        <ContentTemplate>
                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upGv" runat="server">
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="linkExportCash" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:LinkButton ID="linkExportCash" runat="server">Export</asp:LinkButton>
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:GridView ID="gvAssetCash" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                    BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                                    CellPadding="3" DataKeyNames="asset_id" DataSourceID="objSrcAssetCash" GridLines="Vertical"
                                                    Width="100%">
                                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                                    <AlternatingRowStyle BackColor="Gainsboro" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="ref_no">
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="link" runat="server" Target="_blank"><%#Eval("ref_no")%></asp:HyperLink></ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="datetime" HeaderText="Datetime" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                                                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Customer_Name" SortExpression="Firstname">
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="linkCust" runat="server" Font-Underline="false" Target="_blank"><%#Eval("firstname")%></asp:HyperLink></ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="type" HeaderText="Type">
                                                            <ItemStyle HorizontalAlign="Center" Width="180px" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="status_name" HeaderText="Remark">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="user_name" HeaderText="created_by">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="payment" HeaderText="Payment">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="amount" HeaderText="Amount" DataFormatString="{0:#,##0.00}">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="cash" HeaderText="NetCash" DataFormatString="{0:#,##0.00}">
                                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="trans" HeaderText="NetTrans" DataFormatString="{0:#,##0.00}">
                                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="cheq" HeaderText="NetCheque" DataFormatString="{0:#,##0.00}">
                                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Net" DataFormatString="{0:#,##0.00}">
                                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                    <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                                </asp:GridView>
                                                <asp:ObjectDataSource ID="objSrcAssetCash" runat="server" OldValuesParameterFormatString="original_{0}"
                                                    SelectMethod="getActual" TypeName="clsDB">
                                                    <SelectParameters>
                                                        <asp:Parameter DefaultValue="" Name="assetType" Type="String" ConvertEmptyStringToNull="False" />
                                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="purity" Type="String" />
                                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="actualDay" Type="String" />
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
                            Gold</HeaderTemplate>
                        <ContentTemplate>
                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                            <Triggers>
                                                <asp:PostBackTrigger ControlID="linkExportGold" />
                                            </Triggers>
                                            <ContentTemplate>
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:LinkButton ID="linkExportGold" runat="server">Export</asp:LinkButton>&nbsp;&nbsp;
                                                        </td>
                                                        <td>
                                                            <asp:CheckBoxList ID="cblPurity" runat="server" RepeatDirection="Horizontal">
                                                                <asp:ListItem Value="96">96.50</asp:ListItem>
                                                                <asp:ListItem Value="99">99.99</asp:ListItem>
                                                            </asp:CheckBoxList>
                                                        </td>
                                                        <td>
                                                            &#160;&#160;
                                                        </td>
                                                    </tr>
                                                </table>
                                                <asp:GridView ID="gvAssetGold96" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                    BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                                    CellPadding="3" DataKeyNames="asset_id" DataSourceID="objSrcAssetGold96" GridLines="Vertical"
                                                    Width="100%">
                                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                                    <AlternatingRowStyle BackColor="Gainsboro" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="ref_no">
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="link" runat="server" Target="_blank"><%#Eval("ref_no")%></asp:HyperLink></ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="datetime" HeaderText="Datetime" DataFormatString="{0:dd/MM/yyyy HH:mm:ss}">
                                                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Customer_Name" SortExpression="Firstname">
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="linkCust" runat="server" Font-Underline="false" Target="_blank"><%#Eval("firstname")%></asp:HyperLink></ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="type" HeaderText="Type">
                                                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="purity" HeaderText="Purity">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="status_name" HeaderText="Remark">
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="user_name" HeaderText="created_by">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="quantity" HeaderText="Quantity" DataFormatString="{0:#,##0.00}">
                                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Net96" DataField="G96_base" DataFormatString="{0:#,##0.00}">
                                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Net96G" DataField="G96G_base" DataFormatString="{0:#,##0.00}">
                                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Net99" DataField="G99_base" DataFormatString="{0:#,##0.00}">
                                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                    <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                                </asp:GridView>
                                                <asp:ObjectDataSource ID="objSrcAssetGold96" runat="server" OldValuesParameterFormatString="original_{0}"
                                                    SelectMethod="getActual" TypeName="clsDB">
                                                    <SelectParameters>
                                                        <asp:Parameter DefaultValue="" Name="assetType" Type="String" ConvertEmptyStringToNull="False" />
                                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="purity" Type="String" />
                                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="actualDay" Type="String" />
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
