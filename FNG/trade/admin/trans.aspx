<%@ Page Language="VB" AutoEventWireup="false" CodeFile="trans.aspx.vb" Inherits="admin_trans"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Transaction Data</title>
    <link href="../style.css" rel="stylesheet" type="text/css" />
    <link href="../../button/css/buttonPro.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script src="../js/function.js" type="text/javascript"></script>
</head>
<body>
    <script language="javascript" type="text/javascript">
        function Check() {
            if ($get('hdfIsRealtime').value != 'n') {
                PageMethods.getTradeMaxId($get('hdfMode').value, OnSucceeded, OnFailed);
            }
        }
        function OnSucceeded(result, userContext, methodName) {
            var trade_max_id = parseInt(result);
            var pre_trade_max_id = parseInt($get('hdfTradeLogId').value);

            if (trade_max_id > pre_trade_max_id) {
                $get('hdfTradeLogIdForGrid').value = pre_trade_max_id; //For Gridview to know new trade
                $get('hdfTradeLogId').value = trade_max_id;
                __doPostBack('upGrid', '');
            }
            else {
                setTimeout("Check()", 4000);
            }
        }
        function pageLoad() {
            //setTimeout("Check()", 4000);
        }
        function refreshGrid() {
            __doPostBack('upGrid', '');
        }
        function setIframeHeight(name, h) {
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
        function OnFailed(error, userContext, methodName) { }
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
                    <img src="../img/indicator.gif" style="vertical-align: middle" alt="Processing" />
                    Loading ... &nbsp;
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <div id="div_content">
            <table width="100%">
                <tr>
                    <td style="width: 50%" valign="top">
                        <table>
                            <tr>
                                <td style="width: 50px">
                                    &nbsp;
                                </td>
                                <td style="width: 200px">
                                    &nbsp;
                                </td>
                                <td style="width: 50px">
                                    Purity :
                                </td>
                                <td style="width: 350px">
                                    <asp:CheckBoxList ID="cbPurity" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="96" Selected="True">96.50</asp:ListItem>
                                        <asp:ListItem Value="99" Selected="True">99.99</asp:ListItem>
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 50px">
                                    &nbsp;
                                </td>
                                <td style="width: 200px">
                                    &nbsp;
                                </td>
                                <td style="width: 50px">
                                    Type :
                                </td>
                                <td style="width: 350px">
                                    <asp:RadioButtonList ID="rdoType" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Value="" Selected="True">All</asp:ListItem>
                                        <asp:ListItem Value="sell">Sell</asp:ListItem>
                                        <asp:ListItem Value="buy">Buy</asp:ListItem>
                                    </asp:RadioButtonList>
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
                                        <td style="width: 300px">
                                            From
                                            <asp:TextBox ID="txt3Date" runat="server" Width="65"></asp:TextBox>
                                            <cc1:CalendarExtender ID="txt3Date_CalendarExtender" runat="server" Enabled="True"
                                                TargetControlID="txt3Date">
                                            </cc1:CalendarExtender>
                                            <asp:DropDownList ID="ddl3Hour" runat="server">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddl3Min" runat="server">
                                            </asp:DropDownList>
                                            &nbsp;
                                        </td>
                                        <td style="width: 400px">
                                            To Now
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="width: 50px">
                                            <asp:RadioButton ID="rdoTime2" runat="server" GroupName="time" />
                                        </td>
                                        <td style="width: 300px">
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
                                                    <td>
                                                        To&nbsp; &nbsp;
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
                <asp:Button ID="btnSearch" CssClass="buttonPro small grey" runat="server" Text="Search" />
            </div>
            <div id="div_excel" style="padding-bottom: 5px; disply: none">
                <asp:LinkButton ID="linkExcel" runat="server" Text="Export"></asp:LinkButton>
            </div>
            <asp:UpdatePanel ID="upGrid" runat="server">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="click" />
                </Triggers>
                <ContentTemplate>
                    <asp:GridView ID="gvTrade" runat="server" AutoGenerateColumns="False" BackColor="White"
                        BorderColor="#999999" BorderStyle="None" BorderWidth="0px" CellPadding="3" DataKeyNames="trade_id,type,price,gold_type_id"
                        Width="99%" AllowSorting="True" DataSourceID="objSrcTrade" AllowPaging="True"
                        PageSize="20">
                        <Columns>
                            <asp:BoundField DataField="ref_no" HeaderText="Ref_No" SortExpression="ref_no">
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="leave_order" HeaderText="Status" SortExpression="leave_order">
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
                            <ItemStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="ip" HeaderText="IP Address" SortExpression="ip">
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="gold_type_id" HeaderText="Purity" SortExpression="gold_type_id">
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="mode" HeaderText="Mode" SortExpression="mode">
                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="price" HeaderText="Price" SortExpression="price" DataFormatString="{0:#,##0.00}">
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="quantity" HeaderText="Quantity" SortExpression="quantity"
                                DataFormatString="{0:#,##0.00000}">
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="amount" HeaderText="Amount" SortExpression="amount" DataFormatString="{0:#,##0.00}">
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="modifier_date" DataFormatString="{0:dd/MM/yyyy HH:mm}"
                                HeaderText="Filled Time" SortExpression="modifier_date">
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnAccept99" runat="server" ImageUrl="image/accept.png" OnClick="btnAccept99_Click"
                                        Width="18px" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="40px" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnReject99" runat="server" ImageUrl="image/reject.png" OnClick="btnReject99_Click"
                                        Width="18px" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="40px" />
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#666666" ForeColor="White" />
                        <HeaderStyle BackColor="#333333" ForeColor="White" />
                        <PagerStyle Font-Size="Medium" HorizontalAlign="Center" />
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                    </asp:GridView>
                    <asp:ObjectDataSource ID="objSrcTrade" runat="server" SelectMethod="getTradeByAdminNoTran"
                        TypeName="clsMain" OldValuesParameterFormatString="original_{0}">
                        <SelectParameters>
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="mode" Type="String" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="type" Type="String" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="purity" Type="String" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="cust_id" Type="String" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="max_trade_id" Type="String" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="period" Type="String" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="pDate1" Type="DateTime" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="pDate2" Type="DateTime" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="onlyLeave" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <asp:HiddenField ID="hdfMode" runat="server" />
                    <asp:HiddenField ID="hdfIsRealtime" runat="server" />
                    <asp:HiddenField ID="hdfTradeLogIdForGrid" runat="server" />
                    <asp:HiddenField ID="hdfConfirm" runat="server" />
                    <asp:HiddenField ID="hdfTradeLogId" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    </form>
</body>
</html>
