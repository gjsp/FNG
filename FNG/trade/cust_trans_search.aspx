<%@ Page Language="VB" AutoEventWireup="false" CodeFile="cust_trans_search.aspx.vb" Inherits="cust_trans_search"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Transaction Data</title>
    <link href="style.css" rel="stylesheet" type="text/css" />
    <link href="../button/css/buttonPro.css" rel="stylesheet" type="text/css" />
    <script src="js/function.js" type="text/javascript"></script>

    <style type="text/css">
        .style1
        {
            width: 50px;
            height: 29px;
        }
        .style2
        {
            width: 100px;
            height: 29px;
        }
        .style3
        {
            width: 220px;
            height: 29px;
        }
        .style4
        {
            width: 200px;
            height: 29px;
        }
    </style>

</head>
<body>

    <script language="javascript" type="text/javascript">
    function Check()
    {
        if($get('hdfIsRealtime').value!='n')
        {
            PageMethods.getTradeMaxId($get('hdfMode').value,OnSucceeded, OnFailed);
        }
    }
    function OnSucceeded(result, userContext, methodName)
    {
      var trade_max_id = parseInt(result);
      var pre_trade_max_id = parseInt($get('hdfTradeLogId').value); 

      if (trade_max_id > pre_trade_max_id)
      {
        $get('hdfTradeLogIdForGrid').value = pre_trade_max_id;//For Gridview to know new trade
        $get('hdfTradeLogId').value = trade_max_id;
        __doPostBack('upGrid', '');
      }
      else
      {
        setTimeout("Check()", 3000);
      }
    }
    function pageLoad()
    {
      setTimeout("Check()", 3000);
    }
    function refreshGrid()
    {
       __doPostBack('upGrid', '');
    }
    function setIframeHeight(name,h)
    {
      top.window.document.getElementById(name).style.height = h + "px";
    }
//    function setSummary(buy96,sell96,net96,buy99,sell99,net99)
//    {
//     top.window.document.getElementById('ctl00_ContentPlaceHolder1_hdfBuy96').innerText = buy96;
//     top.window.document.getElementById('ctl00_ContentPlaceHolder1_hdfSell96').innerText = sell96;
//     
//     top.window.document.getElementById('ctl00_ContentPlaceHolder1_hdfBuy99').innerText = buy99;
//     top.window.document.getElementById('ctl00_ContentPlaceHolder1_hdfSell99').innerText = sell99;
//     top.window.document.getElementById('ctl00_ContentPlaceHolder1_btnRefresh').click();   
//     
////     top.window.document.getElementById('ctl00_ContentPlaceHolder1_lblSumBuy96').innerText = buy96;
////     top.window.document.getElementById('ctl00_ContentPlaceHolder1_lblSumSell96').innerText = sell96;
////     top.window.document.getElementById('ctl00_ContentPlaceHolder1_lblSumNet96').innerText = net96;
////     
////     top.window.document.getElementById('ctl00_ContentPlaceHolder1_lblSumBuy99').innerText = buy99;
////     top.window.document.getElementById('ctl00_ContentPlaceHolder1_lblSumSell99').innerText = sell99;
////     top.window.document.getElementById('ctl00_ContentPlaceHolder1_lblSumNet99').innerText = net99;
////     top.window.document.getElementById('ctl00_ContentPlaceHolder1_btnRefresh').click();     
//    }
    function OnFailed(error, userContext, methodName) {}
    </script>

    <form id="form1" runat="server">
    <asp:ScriptManager ID="SM" EnablePageMethods="true" runat="server">
    </asp:ScriptManager>
    <div id="div_area">
        <asp:UpdateProgress ID="UpdateProg1" runat="server">
            <ProgressTemplate>
                <div align="center" valign="middle" runat="server" style="position: absolute; left: 43%;
                    padding: 10px 10px 10px 10px; visibility: visible; border-color: silver; border-style: solid;
                    border-width: 1px; background-color: White;">
                    <img src="img/indicator.gif" style="vertical-align: middle" alt="Processing" />
                    Loading ... &nbsp;
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <div id="div_content">
        <div id="div_search" runat="server">
            <table width="100%">
                <tr>
                    <td style="width: 50%" valign="top">
                        <table>
                            <tr>
                                <td style="width: 50px;text-align:right">
                                    &nbsp;</td>
                                <td style="width: 100px;text-align:right">
                                    Type :</td>
                                <td style="width: 220px;text-align:left">
                                    <asp:RadioButtonList ID="rdoType" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="" Selected="True">All</asp:ListItem>
                                        <asp:ListItem Value="sell">Sell</asp:ListItem>
                                        <asp:ListItem Value="buy">Buy</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td style="width: 50px">
                                    Purity :
                                </td>
                                <td style="width: 200px">
                                    <asp:RadioButtonList ID="rdoPurity" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="" Selected="True">All</asp:ListItem>
                                        <asp:ListItem Value="96">96.50</asp:ListItem>
                                        <asp:ListItem Value="99">99.99</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align:right" class="style1">
                                    </td>
                                <td style="text-align:right" class="style2">
                                    Mode :</td>
                                <td style="text-align:left" class="style3">
                                    <asp:RadioButtonList ID="rdoOrder" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="" Selected="True">All</asp:ListItem>
                                        <asp:ListItem Value="n">Order</asp:ListItem>
                                        <asp:ListItem Value="y">Leave Order</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td class="style1">
                                    Status :</td>
                                <td class="style4">
                                    <asp:CheckBoxList ID="cbMode" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="tran" Selected="True">Pending</asp:ListItem>
                                        <asp:ListItem Value="accept" Selected="True">Accept</asp:ListItem>
                                        <asp:ListItem Value="reject" Selected="True">Reject</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            </table>
                    </td>
                    <td style="width: 50%" valign="top">
                        <asp:UpdatePanel ID="udpSum" runat="server">
                            <ContentTemplate>
                                <table>
                                    <tr>
                                        <td style="width: 50px">
                                            <asp:RadioButton ID="rdoTime1" runat="server" GroupName="time" Checked="true" />
                                        </td>
                                        <td style="width: 200px">
                                            Today
                                        </td>
                                        <td style="width: 50px">
                                            &nbsp;
                                        </td>
                                        <td style="width: 400px">
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50px">
                                            <asp:RadioButton ID="rdoTime2" runat="server" GroupName="time" />
                                        </td>
                                        <td colspan="2">
                                            From
                                            <asp:TextBox ID="txt1Date" runat="server" Width="65"></asp:TextBox>
                                            <cc1:CalendarExtender ID="txt1Date_CalendarExtender" runat="server" Enabled="True"
                                                TargetControlID="txt1Date">
                                            </cc1:CalendarExtender>
                                            <asp:DropDownList ID="ddl1Hour" runat="server">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddl1Min" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td style="width: 400px">
                                           
                                            <table cellpadding="0" cellspacing="0" border="0">
                                                <tr>
                                                    <td> To&nbsp; &nbsp;
                                                        <asp:TextBox ID="txt2Date" runat="server" Width="65"></asp:TextBox>
                                                        <cc1:CalendarExtender ID="txt2Date_CalendarExtender" runat="server" Enabled="True"
                                                            TargetControlID="txt2Date">
                                                        </cc1:CalendarExtender>
                                                        <asp:DropDownList ID="ddl2Hour" runat="server">
                                                        </asp:DropDownList>
                                                        <asp:DropDownList ID="ddl2Min" runat="server">
                                                        </asp:DropDownList>
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                    </tr>
                                </table>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </td>
                </tr>
            </table>
            <div align="center">
                <asp:Button ID="btnSearch" CssClass="buttonPro small black" runat="server" Text="Search" />
            </div>
            </div>
            <div style="padding-bottom:5px">
                <asp:LinkButton ID="linkExcel" runat="server" Text="Export"></asp:LinkButton>
            </div>
            <asp:UpdatePanel ID="upGrid" runat="server">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="click" />
                </Triggers>
                <ContentTemplate>
                    <asp:GridView ID="gvTrade" runat="server" AutoGenerateColumns="False" BackColor="White"
                        BorderColor="#999999" BorderStyle="None" BorderWidth="0px" CellPadding="3" DataKeyNames="trade_id,type,price,gold_type_id"
                        Width="99%" AllowSorting="True" DataSourceID="objSrcTrade">
                        <Columns>
                            <asp:BoundField DataField="trade_id" HeaderText="Ref_No" SortExpression="trade_id">
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="leave_order" HeaderText="Mode" SortExpression="leave_order">
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="created_date" HeaderText="Deal Date" DataFormatString="{0:dd/MM/yyyy HH:mm}"
                                SortExpression="created_date">
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="type" HeaderText="Sell/Buy" SortExpression="type">
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="firstname" HeaderText="Customer Name" SortExpression="firstname">
                            </asp:BoundField>
                            <asp:BoundField DataField="ip" HeaderText="IP Address" SortExpression="ip">
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="gold_type_id" HeaderText="Purity" SortExpression="gold_type_id">
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="mode" HeaderText="Status" SortExpression="mode">
                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="price" HeaderText="Price" SortExpression="price" DataFormatString="{0:#,##0}">
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="quantity" HeaderText="Quantity" SortExpression="quantity"
                                DataFormatString="{0:#,##0}">
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="amount" HeaderText="Amount" SortExpression="amount" DataFormatString="{0:#,##0}">
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="modifier_date" 
                                DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Filled Time" 
                                SortExpression="modifier_date" />
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnAccept99" runat="server" ImageUrl="admin/image/accept.png" OnClick="btnAccept99_Click"
                                        Width="18px" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="40px" />
                            </asp:TemplateField>
                            <asp:TemplateField Visible="False">
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnReject99" runat="server" ImageUrl="admin/image/reject.png" OnClick="btnReject99_Click"
                                        Width="18px" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="40px" />
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#666666" ForeColor="White" />
                        <HeaderStyle BackColor="#4F9502" ForeColor="White" />
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                    </asp:GridView>
                    <asp:ObjectDataSource ID="objSrcTrade" runat="server" SelectMethod="getTradeBlotter"
                        TypeName="clsMain" OldValuesParameterFormatString="original_{0}">
                        <SelectParameters>
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="mode" Type="String" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="order" Type="String" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="type" Type="String" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="purity" Type="String" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="cust_id" Type="String" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="period" Type="String" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="pDate1" Type="DateTime" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="pDate2" Type="DateTime" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="sale_id" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <asp:HiddenField ID="hdfMode" runat="server" />
                    <asp:HiddenField ID="hdfIsRealtime" runat="server" />
                    <asp:HiddenField ID="hdfTradeLogIdForGrid" runat="server" />
                    <asp:HiddenField ID="hdfConfirm" runat="server" />
                    <asp:HiddenField ID="hdfTradeLogId" runat="server" />
                    <asp:HiddenField ID="hdfSale_id" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    </form>
</body>
</html>
