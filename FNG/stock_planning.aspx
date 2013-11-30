<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false"
    CodeFile="stock_planning.aspx.vb" Inherits="stock_planning" Title="Planning Stock" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
    function search()
    {
        $get('ctl00_SampleContent_imgSearch').click();
    }

    function setStockPage(client_id)
    {
       var i = 0;
       var planmode = '0';
       if ($get('ctl00_SampleContent_cbPlan').check == true)
       {
        planmode  = '1';
       }
        var day = $get(client_id).value.split('/')[0];
        var month = $get(client_id).value.split('/')[1];
        var year = $get(client_id).value.split('/')[2];
        
        i=setPlanningDay(day,month,year);
        $get('ctl00_SampleContent_ifm').src="stock_now.aspx?count=" + i + "&planMode="+ planmode +"";
    }
    function setPlanningDay(dd,MM,yyyy)
    {
        toDay = new Date();
        now = new Date(toDay.getFullYear(),toDay.getMonth(),toDay.getDate());
        iDate = new Date(yyyy,MM-1,dd);

        var one_day = 1000*60*60*24;
        var defDate = (iDate.getTime() - now.getTime()) / one_day
        return defDate
    }
    function swepMenu(xMenu1,xMenu2)
{
    var menu_display1 = document.getElementById(xMenu1);
    var menu_display2 = document.getElementById(xMenu2);
    
    if (menu_display2.style.display == 'block')
    {
        menu_display2.style.display = 'none';
        menu_display1.style.display = 'block';
    }
    else if(menu_display2.style.display=='none')
    {
        menu_display2.style.display = 'block';
        menu_display1.style.display = 'none';
    }
    

}
    </script>

    <style type="text/css">
        .style1
        {
            width: 113px;
        }
        .style2
        {
            width: 89px;
        }
        .style3
        {
            width: 48px;
        }
        .style4
        {
            width: 133px;
        }
        .style6
        {
            height: 22px;
        }
        .style7
        {
            width: 113px;
            height: 22px;
        }
        .style9
        {
            width: 200px;
            height: 22px;
        }
        .style10
        {
            width: 204px;
        }
        .style13
        {
            width: 204px;
            height: 22px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea">
        <div class="demoheading">
            Planning Stock</div>
        <div>
            <%--<table cellpadding="0" cellspacing="0" width="1200px" style="display: none">
                <tr id="tr1" style="display: block;">
                    <td class="style12">
                        <iframe id="ifm" runat="server" src="stock_now.aspx" style="height: 350px; width: 1200px"
                            frameborder="0" scrolling="no" marginheight="0" marginwidth="0"></iframe>
                    </td>
                </tr>
                <tr id="tr2" style="display: block">
                    <td style="text-align: center; height: 2px">
                        <img id="imgRoute1" src="images/arrow_up.gif" onclick="enableWindow('0');" style="cursor: pointer;" />
                    </td>
                </tr>
                <tr id="tr3" style="display: none">
                    <td style="text-align: center; height: 2px">
                        <img id="img1" src="images/arrow_down.gif" onclick="enableWindow('1');" style="cursor: pointer;" />
                    </td>
                </tr>
            </table>--%>
            <asp:UpdatePanel ID="upd" runat="server" RenderMode="Inline">
                <ContentTemplate>
                    <table cellpadding="0" cellspacing="0">
                        <tr>
                            <td class="style2">
                                &nbsp;
                            </td>
                            <td class="style1">
                                &nbsp;
                            </td>
                            <td class="style10">
                                &nbsp;
                            </td>
                            <td class="style3">
                                &nbsp;
                            </td>
                            <td class="style4">
                                &nbsp;
                            </td>
                            <td style="width: 200px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td class="style6">
                                Planning Day :
                            </td>
                            <td class="style7">
                                <asp:TextBox ID="txtDate" runat="server" Width="80px"></asp:TextBox>
                                <cc1:CalendarExtender ID="txtDate_CalendarExtender" runat="server" Enabled="True"
                                    PopupButtonID="imgDate" PopupPosition="BottomRight" TargetControlID="txtDate"
                                    OnClientDateSelectionChanged="search">
                                </cc1:CalendarExtender>
                                <asp:ImageButton ID="imgDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                            </td>
                            <td class="style13">
                                <asp:CheckBox ID="cbCall" runat="server" Text="ไม่ระบุวันส่ง" />
                                &nbsp;<asp:CheckBox ID="cbDeposit" Text="หักทองฝาก" runat="server" />
                                &nbsp;<asp:ImageButton ID="imgSearch" runat="server" ImageUrl="~/images/search.bmp" />
                            </td>
                            <td class="style9">
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
                        <tr id="tr1">
                            <td class="style2">
                                &nbsp;
                            </td>
                            <td class="style1">
                                &nbsp;
                            </td>
                            <td class="style10">
                                &nbsp;
                            </td>
                            <td class="style3">
                                &nbsp;
                            </td>
                            <td class="style4">
                                &nbsp;
                            </td>
                            <td style="width: 200px">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div style="display: none">
                <asp:CheckBox ID="cbPlan" Text="สถานะคงเหลือ" runat="server" />
            </div>
            <div style="width: 1200px">
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
                                        <asp:Label ID="lblCash" runat="server" CssClass="lblBold"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Net Trans :
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblTrans" runat="server" CssClass="lblBold"></asp:Label>
                                    </td>
                                    <td valign="top">
                                        &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;Net Cheque :
                                    </td>
                                    <td valign="top">
                                        <asp:Label ID="lblCheq" runat="server" CssClass="lblBold"></asp:Label>
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
                                </tr>
                            </table>
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <div style="width: 1200px">
                <cc1:TabContainer ID="tapImport" runat="server" ActiveTabIndex="1" Width="100%">
                    <cc1:TabPanel ID="tpCash" runat="server" HeaderText="Cash">
                        <ContentTemplate>
                            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                                <tr>
                                    <td>
                                        <asp:UpdatePanel ID="upGv" runat="server">
                                            <ContentTemplate>
                                                <asp:GridView ID="gvAssetCash" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                    BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                                    CellPadding="3" DataKeyNames="asset_id" DataSourceID="objSrcAssetCash" GridLines="Vertical"
                                                    Width="100%">
                                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                                    <AlternatingRowStyle BackColor="Gainsboro" />
                                                    <Columns>
                                                        <asp:TemplateField HeaderText="ref_no">
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="link" runat="server" Target="_blank"><%#Eval("ref_no")%></asp:HyperLink>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="datetime" HeaderText="Datetime" DataFormatString="{0:dd/MM/yyyy}">
                                                            <ItemStyle HorizontalAlign="Center" Wrap="false" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Customer_Name" >
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="linkCust" runat="server" Target="_blank"><%#Eval("firstname")%></asp:HyperLink>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="type" HeaderText="Type">
                                                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="remark" HeaderText="remark" />
                                                        <asp:BoundField DataField="user_name" HeaderText="created_by">
                                                            <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="payment" HeaderText="Payment" >
                                                            <ItemStyle Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="quantity" HeaderText="Amount" 
                                                            DataFormatString="{0:#,##0.00}">
                                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="NetCash">
                                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="NetTrans">
                                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="NetCheque">
                                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Net">
                                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                    <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                                </asp:GridView>
                                                <asp:ObjectDataSource ID="objSrcAssetCash" runat="server" OldValuesParameterFormatString="original_{0}"
                                                    SelectMethod="getPlanning" TypeName="clsDB">
                                                    <SelectParameters>
                                                        <asp:Parameter DefaultValue="" Name="assetType" Type="String" ConvertEmptyStringToNull="False" />
                                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="purity" Type="String" />
                                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="planningDay" Type="String" />
                                                        <asp:Parameter ConvertEmptyStringToNull="False" DefaultValue="0" Name="delivery_date_null"
                                                            Type="String" />
                                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="deposit" Type="String" />
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
                                                <table>
                                                    <tr>
                                                        <td>
                                                            <asp:CheckBoxList ID="cblPurity" runat="server" RepeatDirection="Horizontal">
                                                                <asp:ListItem Value="96">96.50</asp:ListItem>
                                                                <asp:ListItem Value="99N">99.99N</asp:ListItem>
                                                                <asp:ListItem Value="99L">99.99L</asp:ListItem>
                                                            </asp:CheckBoxList>
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
                                                                <asp:HyperLink ID="link" runat="server" Target="_blank"><%#Eval("ref_no")%></asp:HyperLink>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="datetime" HeaderText="Datetime" DataFormatString="{0:dd/MM/yyyy}">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField HeaderText="Customer_Name" >
                                                            <ItemTemplate>
                                                                <asp:HyperLink ID="linkCust" runat="server" Target="_blank"><%#Eval("firstname")%></asp:HyperLink>
                                                            </ItemTemplate>
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:TemplateField>
                                                        <asp:BoundField DataField="type" HeaderText="Type">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="purity" HeaderText="Purity">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="remark" HeaderText="remark" />
                                                        <asp:BoundField DataField="user_name" HeaderText="created_by">
                                                            <ItemStyle HorizontalAlign="Left" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="gold_base" HeaderText="ทองตั้งต้น" Visible="False">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="gold_company" HeaderText="ทองบริษัท" Visible="False">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="gold_branch" HeaderText="ทองสาขา" Visible="False">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="gold_customer" HeaderText="ทองลูกค้า" Visible="False">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="quantity" HeaderText="Quantity" DataFormatString="{0:#,##0.00000}">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Net96">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Net99N">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField HeaderText="Net99L">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                    </Columns>
                                                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                    <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                                </asp:GridView>
                                                <asp:ObjectDataSource ID="objSrcAssetGold96" runat="server" OldValuesParameterFormatString="original_{0}"
                                                    SelectMethod="getPlanning" TypeName="clsDB">
                                                    <SelectParameters>
                                                        <asp:Parameter DefaultValue="" Name="assetType" Type="String" ConvertEmptyStringToNull="False" />
                                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="purity" Type="String" />
                                                        <asp:Parameter ConvertEmptyStringToNull="False" DefaultValue="" Name="planningDay"
                                                            Type="String" />
                                                        <asp:Parameter ConvertEmptyStringToNull="False" DefaultValue="0" Name="delivery_date_null"
                                                            Type="String" />
                                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="deposit" Type="String" />
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
