<%@ Page Language="VB" AutoEventWireup="false" CodeFile="trans2.aspx.vb" Inherits="admin_trans2"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Transaction Data</title>
    <link href="../../button/css/buttonPro.css" rel="stylesheet" type="text/css" />
    <link href="../style.css" rel="stylesheet" type="text/css" />
    <script src="../js/function.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <style type="text/css">
        .halt
        {
            background-image: url('image/halt.gif');
            background-repeat: no-repeat;
            background-color: red;
        }
    </style>
</head>
<body>
    <script type="text/javascript">
        var _source;
        var _popup;

        function showConfirmPer(source) {
            this._source = source;
            this._popup = $find('mppPer');

            //  find the confirm ModalPopup and show it    
            this._popup.show();
            $get('<%=txtPwdPer.ClientId %>').value = '';
            $get('<%=txtPwdPer.ClientId %>').focus();
        }

        function okClickPer() {
            if ($get('<%=txtPwdPer.ClientId %>').value == '') { $get('<%=txtPwdPer.ClientId %>').focus(); return };
            if ($get('<%=txtPwdPer.ClientId %>').value != $get('<%=hdfPwd.ClientId %>').value) {
                $get('<%=txtPwdPer.ClientId %>').focus();
                alert('Password Invalid.');
            }
            else {
                //  find the confirm ModalPopup and hide it   
                $get('<%=txtPwdPer.ClientId %>').value = '';
                this._popup.hide();
                //  use the cached button as the postback source
                __doPostBack(this._source.name, '');
            }
        }

        function cancelClickPer() {
            //  find the confirm ModalPopup and hide it 
            this._popup.hide();
            //  clear the event source
            this._source = null;
            this._popup = null;
        }
    </script>
    <script type="text/javascript">
        var _source1;
        var _popup1;

        function showConfirmPassword(source) {
            this._source1 = source;
            this._popup1 = $find('mppDel');

            //  find the confirm ModalPopup and show it    
            this._popup1.show();
            $get('<%=txtPwd.ClientId %>').value = '';
            $get('<%=txtPwd.ClientId %>').focus();
        }

        function okClick1() {
            if ($get('<%=txtPwd.ClientId %>').value == '') { $get('<%=txtPwd.ClientId %>').focus(); return };
            if ($get('<%=txtPwd.ClientId %>').value != $get('<%=hdfPwd.ClientId %>').value) {
                $get('<%=txtPwd.ClientId %>').focus();
                alert('Password Invalid.');
            }
            else {
                //  find the confirm ModalPopup and hide it   
                $get('<%=txtPwd.ClientId %>').value = '';
                this._popup1.hide();
                //  use the cached button as the postback source
                __doPostBack(this._source1.name, '');
            }
        }

        function cancelClick1() {
            //  find the confirm ModalPopup and hide it 
            this._popup1.hide();
            //  clear the event source
            this._source1 = null;
            this._popup1 = null;
        }
    </script>
    <script language="javascript" type="text/javascript">
        var _source;
        var _popup;

        function showConfirm(source, row, msg) {
            //if fasttrade is check get value from radio 
            //else get value dropdownlist
            $get('pContent').innerText = '';
            if ($get('<%=cbConfirm.ClientId %>').checked == true) {
                //Fast 
                var rdo = $get('<%=rdoFashTrade.ClientId %>');
                var rdoValue = '';
                if (rdo.cells[0].children[0].checked == true) { rdoValue = 'accept' }
                else if (rdo.cells[1].children[0].checked == true) { rdoValue = 'same' }
                else if (rdo.cells[2].children[0].checked == true) { rdoValue = 'lowerOrHigher' }
                $get('<%=hdfFastMode.ClientId %>').value = rdoValue;
                if (msg != '') {
                    //ถ้า hiegher or lower (leave order) ให้ show message 
                    div_ctrl.style.display = 'none';
                    $get('pContent').innerText = msg;
                    this._source = source;
                    this._popup = $find('mppAccept');
                    this._popup.show();
                } else {
                    __doPostBack(source.name, '')
                }    
            }
            else {
                //No Fast
                if (msg != '') {
                    $get('pContent').innerText = msg;
                } 
                div_ctrl.style.display = 'block';
                $get('<%=ddlConfirm.ClientId %>').selectedIndex = 0;
                this._source = source;
                this._popup = $find('mppAccept');
                this._popup.show();
            }
        }

        function setConfirmSave(source, msg, ctrl) {
            var flag;
            if ($get(source).checked == true) {
                flag = true;
            }
            else {
                flag = confirm(msg);
                if (flag == 'False') { return false; }
            }
            if (flag == true) {
                var bid = $get('<%=hdfBid99.ClientId %>').value.replace(',', '');
                var newbid = $get('<%=txt99Bid1.ClientId %>').value.replace(',', '');
                var per = $get('<%=txtMaxPer.ClientId %>').value;
                var bidper = parseFloat(bid) * (parseFloat(per) / 100);
                if (parseFloat(bid) == parseFloat(newbid)) {
                    flag = true;
                } else {
                    if (parseFloat(newbid) > parseFloat(bid)) {
                        if (parseFloat(newbid) > (parseFloat(bid) + parseFloat(bidper))) {
                            showConfirmPer(ctrl);
                            return false;
                        }
                    } else {
                        if (parseFloat(newbid) < (parseFloat(bid) - parseFloat(bidper))) {
                            showConfirmPer(ctrl);
                            return false;
                        }
                    }
                    flag = true;
                }
            }
            return flag;
        }

        function setConfirm(source) {
            if (source.checked == true) {

                div_fasttrade.style.display = 'block';
            }
            else {
                div_fasttrade.style.display = 'none';
            }
        }

        function okClick() {
            this._popup.hide();
            __doPostBack(this._source.name, '');
        }

        function cancelClick() {
            this._popup.hide();
            this._source = null;
            this._popup = null;
        }


        function Check() {
            if ($get('hdfIsRealtime').value != 'n') {
                //PageMethods.getTradeMaxId($get('hdfTradeLogId').value, OnSucceeded, OnFailed);
                //gtc.getTrans($get('hdfTradeLogId').value, OnSucceeded, OnTimeOut, OnFailed);
            }
        }
        function OnSucceeded(result, userContext, methodName) {
            var trade_max_id = parseInt(result.split(',')[0]);
            var pre_trade_max_id = parseInt($get('hdfTradeLogId').value);

            if (trade_max_id > pre_trade_max_id) {
                $get('hdfTradeLogIdForGrid').value = pre_trade_max_id; //For Gridview to know new trade
                $get('hdfTradeLogId').value = trade_max_id;
                __doPostBack('upSell', '');
                //__doPostBack('upBuy', '');
            }
            else {
                setTimeout("Check()", 4000);
            }
        }
        //        function OnSucceeded(result, userContext, methodName) {
        //            var trade_max_id = parseInt(result);
        //            var pre_trade_max_id = parseInt($get('hdfTradeLogId').value);

        //            if (trade_max_id > pre_trade_max_id) {
        //                $get('hdfTradeLogIdForGrid').value = pre_trade_max_id; //For Gridview to know new trade
        //                $get('hdfTradeLogId').value = trade_max_id;
        //                __doPostBack('upSell', '');
        //                //__doPostBack('upBuy', '');
        //            }
        //            else {
        //                setTimeout("Check()", 4000);
        //            }
        //        }
        function pageLoad() {
            //setTimer();
            //setTimeout("Check()", 4000);
        }
        function refreshGrid() {
            __doPostBack('upSell', '');
            __doPostBack('upBuy', '');
        }
        function setIframeHeight(name, h) {
            top.window.document.getElementById(name).style.height = h + "px";
        }
        function setPriceColor() {
            fsPrice.style.backgroundColor = red;
        }
        function OnTimeOut(arg) {
            alert("TimeOut.");
        }
        function OnFailed(error, userContext, methodName) { }
    </script>
    <script language="javascript" type="text/javascript">
        function addCommas(nStr) {
            nStr += '';
            x = nStr.split('.');
            x1 = x[0];
            x2 = x.length > 1 ? '.' + x[1] : '';
            var rgx = /(\d+)(\d{3})/;
            while (rgx.test(x1)) {
                x1 = x1.replace(rgx, '$1' + ',' + '$2');
            }
            return x1 + x2;
        }
        function changeBid(ctrl) {

        }

        function addBid(lv, plus, purity) {
            if (plus.value == '') return;
            if (purity == '99') {
                var bid = parseFloat($get('<%=txt99Bid1.clientId %>').value.replace(',', ''));
                var bidPlus = parseFloat(plus.value.replace(',', ''));

                if (lv == 2) {
                    $get('<%=txt99Bid2.clientId %>').value = addCommas((bid + bidPlus).toFixed(2));
                }
                if (lv == 3) {
                    $get('<%=txt99Bid3.clientId %>').value = addCommas((bid + bidPlus).toFixed(2));
                }
            }
            else {
                var bid = parseFloat($get('<%=txt96Bid1.clientId %>').value.replace(',', ''));
                var bidPlus = parseFloat(plus.value.replace(',', ''));
                if (lv == 1) {
                    bid = parseFloat($get('<%=txt96Ask1.clientId %>').value.replace(',', ''))
                    $get('<%=txt96Bid1.clientId %>').value = addCommas((bid + bidPlus).toFixed(2));
                }
                if (lv == 2) {
                    $get('<%=txt96Bid2.clientId %>').value = addCommas((bid + bidPlus).toFixed(2));
                }
                if (lv == 3) {
                    $get('<%=txt96Bid3.clientId %>').value = addCommas((bid + bidPlus).toFixed(2));
                }
            }



        }
        function addAsk(lv, plus, purity) {
            if (plus.value == '') return;
            if (purity == '99') {
                var ask = parseFloat($get('<%=txt99Ask1.clientId %>').value.replace(',', ''));
                var askPlus = parseFloat(plus.value.replace(',', ''));
                if (lv == 1) {
                    BidPlus = parseFloat($get('<%=txt99Bid1.clientId %>').value.replace(',', ''));
                    $get('<%=txt99Ask1.clientId %>').value = addCommas((BidPlus - askPlus).toFixed(2));
                }
                if (lv == 2) {
                    $get('<%=txt99Ask2.clientId %>').value = addCommas((ask - askPlus).toFixed(2));
                }
                if (lv == 3) {
                    $get('<%=txt99Ask3.clientId %>').value = addCommas((ask - askPlus).toFixed(2));
                }
            }
            else {
                var ask = parseFloat($get('<%=txt96Ask1.clientId %>').value.replace(',', ''));
                var askPlus = parseFloat(plus.value.replace(',', ''));

                if (lv == 2) {
                    $get('<%=txt96Ask2.clientId %>').value = addCommas((ask - askPlus).toFixed(2));
                }
                if (lv == 3) {
                    $get('<%=txt96Ask3.clientId %>').value = addCommas((ask - askPlus).toFixed(2));
                }
            }
        }
    </script>
    <script language="javascript" type="text/javascript">
        function colorKeyup() {
            $get('<%=hdfChangeColor.clientId%>').value = 'n';
            div_price_color.style.backgroundColor = 'white';
        }
        function colorBlur() {
            $get('<%=hdfChangeColor.clientId%>').value = 'y';
        }
    </script>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="SM" EnablePageMethods="true" runat="server">
    </asp:ScriptManager>
    <asp:UpdatePanel ID="upTime" runat="server">
        <ContentTemplate>
            <asp:Timer ID="tmTime" Interval="4000" runat="server">
            </asp:Timer>
            <asp:HiddenField ID="hdfMode" runat="server" />
            <asp:HiddenField ID="hdfIsRealtime" runat="server" />
            <asp:HiddenField ID="hdfTradeLogIdForGrid" runat="server" />
            <asp:HiddenField ID="hdfConfirm" runat="server" />
            <asp:HiddenField ID="hdfTradeLogId" runat="server" />
            <asp:HiddenField ID="hdfFastMode" runat="server" />
            <asp:HiddenField ID="hdfConfirmSave" runat="server" />
            <asp:HiddenField ID="hdfPwd" runat="server" />
            <asp:HiddenField ID="hdfRealPrice" runat="server" />
        </ContentTemplate>
    </asp:UpdatePanel>
    <div id="div_area">
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
                                <td style="width: 200px; text-align: right">
                                    Purity :
                                </td>
                                <td style="width: 200px">
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
                                <td style="width: 200px; text-align: right">
                                    &nbsp;
                                </td>
                                <td style="width: 200px">
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
            <asp:UpdateProgress ID="pgSell" runat="server" AssociatedUpdatePanelID="upSell">
                <ProgressTemplate>
                    <div id="divPgSell" align="center" valign="middle" runat="server" style="position: absolute;
                        left: 43%; padding: 10px 10px 10px 10px; visibility: visible; border-color: silver;
                        border-style: solid; border-width: 1px; background-color: White;">
                        <img src="../img/indicator.gif" style="vertical-align: middle" alt="Processing" />
                        Loading ... &nbsp;
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:UpdatePanel ID="upSell" runat="server">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="click" />
                </Triggers>
                <ContentTemplate>
                    <fieldset class="k-fieldset" style="width: 98%;">
                        <legend class="topic">Customer Sell</legend>
                        <asp:GridView ID="gvTrade" runat="server" AutoGenerateColumns="False" BackColor="White"
                            BorderColor="#999999" BorderStyle="None" BorderWidth="0px" CellPadding="3" DataKeyNames="trade_id,type,price,gold_type_id"
                            Width="99%" AllowSorting="True" DataSourceID="objSrcTrade">
                            <Columns>
                                <asp:BoundField DataField="trade_id" HeaderText="Ref_No" SortExpression="trade_id">
                                    <ItemStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="leave_order" HeaderText="Status" SortExpression="leave_order">
                                    <ItemStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="modifier_date" HeaderText="Deal Date" DataFormatString="{0:dd/MM/yyyy HH:mm}"
                                    SortExpression="created_date">
                                    <ItemStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="firstname" HeaderText="Customer Name" SortExpression="firstname">
                                </asp:BoundField>
                                <%--FNG No level--%>
                                <asp:BoundField DataField="cust_level" HeaderText="Level" SortExpression="cust_level" Visible="false">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="gold_type_id" HeaderText="Purity" SortExpression="gold_type_id">
                                    <ItemStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="mode" HeaderText="Mode" SortExpression="mode" Visible="False">
                                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="price" HeaderText="Price" SortExpression="price" DataFormatString="{0:#,##0.00}">
                                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="quantity" HeaderText="Quantity" SortExpression="quantity"
                                    DataFormatString="{0:#,##0.00000}">
                                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Total Quantity">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="amount" HeaderText="Amount" SortExpression="amount" DataFormatString="{0:#,##0.00}"
                                    Visible="False">
                                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
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
                            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                            <AlternatingRowStyle BackColor="Gainsboro" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="objSrcTrade" runat="server" SelectMethod="getTradeByMode"
                            TypeName="clsMain" OldValuesParameterFormatString="original_{0}">
                            <SelectParameters>
                                <asp:Parameter ConvertEmptyStringToNull="False" Name="mode" Type="String" />
                                <asp:Parameter ConvertEmptyStringToNull="False" Name="type" Type="String" />
                                <asp:Parameter ConvertEmptyStringToNull="False" Name="purity" Type="String" />
                                <asp:Parameter ConvertEmptyStringToNull="False" Name="cust_id" Type="String" />
                                <asp:Parameter ConvertEmptyStringToNull="False" Name="max_trade_id" Type="String" />
                                <asp:Parameter ConvertEmptyStringToNull="False" Name="sortPrice" Type="String" />
                                <asp:Parameter ConvertEmptyStringToNull="False" Name="period" Type="String" />
                                <asp:Parameter ConvertEmptyStringToNull="False" Name="pDate1" Type="DateTime" />
                                <asp:Parameter ConvertEmptyStringToNull="False" Name="pDate2" Type="DateTime" />
                                <asp:Parameter ConvertEmptyStringToNull="False" Name="onlyLeave" Type="String" />
                                <asp:Parameter ConvertEmptyStringToNull="False" Name="sale_id" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                        <asp:HiddenField ID="hdfTimeDB" runat="server" />
                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>
            <div>
                <asp:UpdatePanel ID="upPriceNonDisplay" runat="server">
                    <ContentTemplate>
                        <fieldset class="k-fieldset" style="width: 98%; float: left; display: none">
                            <legend class="topic">Update Price</legend>
                            <div id="div_price">
                                <asp:UpdatePanel ID="upUpdatePrice" runat="server">
                                    <Triggers>
                                        <asp:AsyncPostBackTrigger ControlID="btnRefresh" EventName="click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="click" />
                                        <asp:AsyncPostBackTrigger ControlID="editbt" EventName="click" />
                                        <asp:AsyncPostBackTrigger ControlID="btnHalt" EventName="click" />
                                        <asp:AsyncPostBackTrigger ControlID="imgUp1" EventName="click" />
                                        <asp:AsyncPostBackTrigger ControlID="imgUp2" EventName="click" />
                                        <asp:AsyncPostBackTrigger ControlID="imgUp3" EventName="click" />
                                        <asp:AsyncPostBackTrigger ControlID="imgDown1" EventName="click" />
                                        <asp:AsyncPostBackTrigger ControlID="imgDown2" EventName="click" />
                                        <asp:AsyncPostBackTrigger ControlID="imgDown3" EventName="click" />
                                    </Triggers>
                                    <ContentTemplate>
                                        <div style="width: 50%; float: left">
                                            <div style="text-align: left;">
                                                <div style="float: left; width: 220px; text-align: right; padding-top: 2px;">
                                                    <span style="color: #0066FF">Bid99.99</span>-<span style="color: #FF6600">Ask96.50
                                                    </span>:&nbsp;
                                                </div>
                                                <div style="float: left; width: 70px; text-align: right; margin-left: 5px;">
                                                </div>
                                                <div style="float: left; width: 50px; text-align: right; padding-top: 2px;">
                                                    &nbsp;
                                                </div>
                                                <div style="float: left; width: 50px; text-align: right; margin-left: 5px;">
                                                </div>
                                                <div style="clear: both; height: 14px;">
                                                </div>
                                            </div>
                                            <asp:Panel ID="pnLv3" runat="server" Visible="true">
                                                <div style="text-align: left;">
                                                    <div style="float: left; width: 50px; text-align: left; font-weight: bold; padding-top: 2px;">
                                                        Level 2</div>
                                                    <div style="float: left; width: 50px; text-align: right; color: #0066FF; padding-top: 2px;">
                                                        Bid :&nbsp;
                                                    </div>
                                                    <div style="float: left; width: 70px; text-align: right; margin-left: 5px;">
                                                        <asp:TextBox ID="txt99Bid2" runat="server" MaxLength="50" Style="text-align: right;
                                                            width: 70px; font-size: 12px; height: 12px;" Height="16px"></asp:TextBox>
                                                    </div>
                                                    <div style="float: left; width: 50px; text-align: right; color: #FF6600; padding-top: 2px;">
                                                        Ask :&nbsp;
                                                    </div>
                                                    <div style="float: left; width: 70px; text-align: right; margin-left: 5px;">
                                                        <asp:TextBox ID="txt99Ask2" runat="server" MaxLength="50" Style="text-align: right;
                                                            width: 70px; font-size: 12px; height: 12px;"></asp:TextBox>
                                                    </div>
                                                    <div style="float: left; width: 50px; text-align: right; color: #0066FF; padding-top: 2px;">
                                                        Bid :&nbsp;
                                                    </div>
                                                    <div style="float: left; width: 50px; text-align: right; margin-left: 5px;">
                                                        <asp:TextBox ID="txt99BidDif2" runat="server" MaxLength="50" Style="text-align: right;
                                                            width: 50px; font-size: 12px; height: 12px;"></asp:TextBox>
                                                    </div>
                                                    <div style="float: left; width: 50px; text-align: right; color: #FF6600; padding-top: 2px;">
                                                        Ask :&nbsp;
                                                    </div>
                                                    <div style="float: left; width: 50px; text-align: right; margin-left: 5px;">
                                                        <asp:TextBox ID="txt99AskDif2" runat="server" MaxLength="50" Style="text-align: right;
                                                            width: 50px; font-size: 12px; height: 12px;"></asp:TextBox>
                                                    </div>
                                                    <div style="clear: both;">
                                                    </div>
                                                </div>
                                                <div style="text-align: left;">
                                                    <div style="float: left; width: 50px; text-align: left; font-weight: bold; padding-top: 2px;">
                                                        Level 3</div>
                                                    <div style="float: left; width: 50px; text-align: right; color: #0066FF; padding-top: 2px;">
                                                        Bid :&nbsp;
                                                    </div>
                                                    <div style="float: left; width: 70px; text-align: right; margin-left: 5px;">
                                                        <asp:TextBox ID="txt99Bid3" runat="server" MaxLength="50" Style="text-align: right;
                                                            width: 70px; font-size: 12px; height: 12px;"></asp:TextBox>
                                                    </div>
                                                    <div style="float: left; width: 50px; text-align: right; color: #FF6600; padding-top: 2px;">
                                                        Ask :&nbsp;
                                                    </div>
                                                    <div style="float: left; width: 70px; text-align: right; margin-left: 5px;">
                                                        <asp:TextBox ID="txt99Ask3" runat="server" MaxLength="50" Style="text-align: right;
                                                            width: 70px; font-size: 12px; height: 12px;"></asp:TextBox>
                                                    </div>
                                                    <div style="float: left; width: 50px; text-align: right; color: #0066FF; padding-top: 2px;">
                                                        Bid :&nbsp;
                                                    </div>
                                                    <div style="float: left; width: 50px; text-align: right; margin-left: 5px;">
                                                        <asp:TextBox ID="txt99BidDif3" runat="server" MaxLength="50" Style="text-align: right;
                                                            width: 50px; font-size: 12px; height: 12px;"></asp:TextBox>
                                                    </div>
                                                    <div style="float: left; width: 50px; text-align: right; color: #FF6600; padding-top: 2px;">
                                                        Ask :&nbsp;
                                                    </div>
                                                    <div style="float: left; width: 50px; text-align: right; margin-left: 5px;">
                                                        <asp:TextBox ID="txt99AskDif3" runat="server" MaxLength="50" Style="text-align: right;
                                                            width: 50px; font-size: 12px; height: 12px;"></asp:TextBox>
                                                    </div>
                                                    <div style="clear: both;">
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                        <div style="width: 50%; float: right">
                                            <div style="text-align: left;">
                                                <div style="float: left; width: 220px; text-align: right; padding-top: 2px;">
                                                    Min Bid - Max Bid
                                                </div>
                                                <div style="float: left; width: 70px; text-align: right; margin-left: 5px;">
                                                    <asp:TextBox ID="txtBidMin" runat="server" MaxLength="50" Style="text-align: right;
                                                        width: 60px; font-size: 12px; height: 12px;"></asp:TextBox>
                                                </div>
                                                <div style="float: left; width: 50px; text-align: right; padding-top: 2px;">
                                                    &nbsp;
                                                </div>
                                                <div style="float: left; width: 50px; text-align: right; margin-left: 5px;">
                                                    <asp:TextBox ID="txtBidMax" runat="server" Height="16px" MaxLength="50" Style="text-align: right;
                                                        width: 60px; font-size: 12px; height: 12px;"></asp:TextBox>
                                                </div>
                                                <div style="clear: both;">
                                                </div>
                                            </div>
                                            <asp:Panel ID="pnLv4" runat="server" Visible="true">
                                                <div style="text-align: left;">
                                                    <div style="float: left; width: 50px; text-align: left; font-weight: bold; padding-top: 2px;">
                                                        Level 2</div>
                                                    <div style="float: left; width: 50px; text-align: right; color: #0066FF; padding-top: 2px;">
                                                        Bid :&nbsp;
                                                    </div>
                                                    <div style="float: left; width: 70px; text-align: right; margin-left: 5px;">
                                                        <asp:TextBox ID="txt96Bid2" runat="server" MaxLength="50" Style="text-align: right;
                                                            width: 70px; font-size: 12px; height: 12px;"></asp:TextBox>
                                                    </div>
                                                    <div style="float: left; width: 50px; text-align: right; color: #FF6600; padding-top: 2px;">
                                                        Ask :&nbsp;
                                                    </div>
                                                    <div style="float: left; width: 70px; text-align: right; margin-left: 5px;">
                                                        <asp:TextBox ID="txt96Ask2" runat="server" MaxLength="50" Style="text-align: right;
                                                            width: 70px; font-size: 12px; height: 12px;"></asp:TextBox>
                                                    </div>
                                                    <div style="float: left; width: 50px; text-align: right; color: #0066FF; padding-top: 2px;">
                                                        Bid :&nbsp;
                                                    </div>
                                                    <div style="float: left; width: 50px; text-align: right; margin-left: 5px;">
                                                        <asp:TextBox ID="txt96BidDif2" runat="server" MaxLength="50" Style="text-align: right;
                                                            width: 50px; font-size: 12px; height: 12px;"></asp:TextBox>
                                                    </div>
                                                    <div style="float: left; width: 50px; text-align: right; color: #FF6600; padding-top: 2px;">
                                                        Ask :&nbsp;
                                                    </div>
                                                    <div style="float: left; width: 50px; text-align: right; margin-left: 5px;">
                                                        <asp:TextBox ID="txt96AskDif2" runat="server" MaxLength="50" Style="text-align: right;
                                                            width: 50px; font-size: 12px; height: 12px;"></asp:TextBox>
                                                    </div>
                                                    <div style="clear: both;">
                                                    </div>
                                                </div>
                                                <div style="text-align: left;">
                                                    <div style="float: left; width: 50px; text-align: left; font-weight: bold; padding-top: 2px;">
                                                        Level 3</div>
                                                    <div style="float: left; width: 50px; text-align: right; color: #0066FF; padding-top: 2px;">
                                                        Bid :&nbsp;
                                                    </div>
                                                    <div style="float: left; width: 70px; text-align: right; margin-left: 5px;">
                                                        <asp:TextBox ID="txt96Bid3" runat="server" MaxLength="50" Style="text-align: right;
                                                            width: 70px; font-size: 12px; height: 12px;"></asp:TextBox>
                                                    </div>
                                                    <div style="float: left; width: 50px; text-align: right; color: #FF6600; padding-top: 2px;">
                                                        Ask :&nbsp;
                                                    </div>
                                                    <div style="float: left; width: 70px; text-align: right; margin-left: 5px;">
                                                        <asp:TextBox ID="txt96Ask3" runat="server" MaxLength="50" Style="text-align: right;
                                                            width: 70px; font-size: 12px; height: 12px;"></asp:TextBox>
                                                    </div>
                                                    <div style="float: left; width: 50px; text-align: right; color: #0066FF; padding-top: 2px;">
                                                        Bid :&nbsp;
                                                    </div>
                                                    <div style="float: left; width: 50px; text-align: right; margin-left: 5px;">
                                                        <asp:TextBox ID="txt96BidDif3" runat="server" MaxLength="50" Style="text-align: right;
                                                            width: 50px; font-size: 12px; height: 12px;"></asp:TextBox>
                                                    </div>
                                                    <div style="float: left; width: 50px; text-align: right; color: #FF6600;">
                                                        Ask :&nbsp;
                                                    </div>
                                                    <div style="float: left; width: 50px; text-align: right; margin-left: 5px;">
                                                        <asp:TextBox ID="txt96AskDif3" runat="server" MaxLength="50" Style="text-align: right;
                                                            width: 50px; font-size: 12px; height: 12px;"></asp:TextBox>
                                                    </div>
                                                    <div style="clear: both;">
                                                    </div>
                                                </div>
                                            </asp:Panel>
                                        </div>
                                    </ContentTemplate>
                                </asp:UpdatePanel>
                                <br />
                                &nbsp; &nbsp;</div>
                            <div style="text-align: center; margin-top: 10px; clear: both;">
                            </div>
                        </fieldset>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <fieldset id="fsPrice" class="k-fieldset" style="width: 98%; display: none;">
                    <legend class="topic">Update Price</legend>
                    <div id="div_price_color" style="background-color: #fff">
                        <asp:UpdatePanel ID="upPrice" runat="server" UpdateMode="Conditional">
                            <ContentTemplate>
                                <table cellpadding="1" cellspacing="1" border="0" align="center">
                                    <tr>
                                        <td style="width: 70px; font-weight: bold">
                                            Max BAHT
                                        </td>
                                        <td style="width: 70px; font-weight: bold">
                                            Max KG.
                                        </td>
                                        <td style="width: 30px; font-weight: bold">
                                            &nbsp;
                                        </td>
                                        <td style="width: 100px; font-weight: bold">
                                            Net 96.50
                                        </td>
                                        <td style="width: 100px; font-weight: bold">
                                            Net 99.99
                                        </td>
                                        <td style="width: 20px; font-weight: bold">
                                            &nbsp;
                                        </td>
                                        <td style="width: 70px; font-weight: bold">
                                            96.50
                                        </td>
                                        <td style="width: 70px; font-weight: bold">
                                            &nbsp;
                                        </td>
                                        <td style="width: 70px; font-weight: bold">
                                            99.99
                                        </td>
                                        <td style="width: 70px; font-weight: bold">
                                            &nbsp;
                                        </td>
                                        <td style="width: 20px; font-weight: bold">
                                            &nbsp;
                                        </td>
                                        <td style="font-weight: bold">
                                            Ask99.99 - Ask96.50
                                        </td>
                                        <td style="width: 30px">
                                        </td>
                                        <td>
                                            <div style="text-align: left;">
                                                <div style="float: left; width: 40px; text-align: center; margin-left: 5px;">
                                                    <asp:ImageButton ID="imgUp1" runat="server" ImageUrl="~/trade/admin/image/arrow_up.gif" />
                                                </div>
                                                <div style="float: left; width: 40px; text-align: center; margin-left: 5px;">
                                                    <asp:ImageButton ID="imgUp2" runat="server" ImageUrl="~/trade/admin/image/arrow_up.gif" />
                                                </div>
                                                <div style="float: left; width: 40px; text-align: center; margin-left: 5px;">
                                                    <asp:ImageButton ID="imgUp3" runat="server" ImageUrl="~/trade/admin/image/arrow_up.gif" />
                                                </div>
                                                <div style="clear: both;">
                                                </div>
                                            </div>
                                        </td>
                                        <td rowspan="4">
                                            <table cellpadding="1" cellspacing="1">
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnHalt" runat="server" CssClass="buttonPro small grey" BorderColor="Black"
                                                            Text="Halt" Width="60px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnRefresh" runat="server" CssClass="buttonPro small grey" Text="Cal"
                                                            Width="60px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="btnReset" runat="server" CssClass="buttonPro small grey" Text="Reset"
                                                            Width="60px" />
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td>
                                                        <asp:Button ID="editbt" runat="server" CssClass="buttonPro small grey" Text="Save"
                                                            Width="60px" />
                                                    </td>
                                                </tr>
                                            </table>
                                        </td>
                                        <td rowspan="4">
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:TextBox ID="txtMaxBaht" runat="server" CssClass="nortxt" MaxLength="10"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtMaxKg" runat="server" CssClass="nortxt" MaxLength="10"></asp:TextBox>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="upNet96" runat="server" RenderMode="Inline">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblNet96" runat="server" Font-Bold="True" Text="0.00000"></asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            <asp:UpdatePanel ID="UpdatePanel1" runat="server" RenderMode="Inline">
                                                <ContentTemplate>
                                                    <asp:Label ID="lblNet99" runat="server" Font-Bold="True" Text="0.00000"></asp:Label>
                                                </ContentTemplate>
                                            </asp:UpdatePanel>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt96Bid1" runat="server" CssClass="bidtxt" MaxLength="10" ReadOnly="True"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt96Ask1" runat="server" CssClass="asktxt" MaxLength="10" ReadOnly="True"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt99Bid1" runat="server" CssClass="bidtxt" MaxLength="9"></asp:TextBox>
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt99Ask1" runat="server" CssClass="asktxt" MaxLength="9"></asp:TextBox>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDif9699" runat="server" MaxLength="10" CssClass="asktxt"></asp:TextBox>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <div style="text-align: left;">
                                                <div style="float: left; width: 40px; text-align: center; margin-left: 5px;">
                                                    <asp:TextBox ID="txtBidChangeNumber1" runat="server" MaxLength="2" Style="text-align: right;
                                                        width: 20px; font-size: 12px; height: 12px; font-weight: bold" Height="16px"></asp:TextBox>
                                                </div>
                                                <div style="float: left; width: 40px; text-align: center; margin-left: 5px;">
                                                    <asp:TextBox ID="txtBidChangeNumber2" runat="server" MaxLength="2" Style="text-align: right;
                                                        width: 20px; font-size: 12px; height: 12px; font-weight: bold"></asp:TextBox>
                                                </div>
                                                <div style="float: left; width: 40px; text-align: center; margin-left: 5px;">
                                                    <asp:TextBox ID="txtBidChangeNumber3" runat="server" MaxLength="2" Style="text-align: right;
                                                        width: 20px; font-size: 12px; height: 12px; font-weight: bold"></asp:TextBox>
                                                </div>
                                                <div style="clear: both;">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: left; font-weight: bold;">
                                            Leave Time
                                        </td>
                                        <td style="text-align: left; font-weight: bold;">
                                            <asp:TextBox ID="txtLeaveTimeout" runat="server" CssClass="nortxt" MaxLength="3"></asp:TextBox>
                                        </td>
                                        <td style="text-align: left; font-weight: bold;">
                                            &nbsp;
                                        </td>
                                        <td style="text-align: left; font-weight: bold;">
                                            Max %
                                        </td>
                                        <td style="text-align: left; font-weight: bold;">
                                            <asp:TextBox ID="txtMaxPer" runat="server" CssClass="nortxt" MaxLength="6"></asp:TextBox>
                                        </td>
                                        <td style="text-align: left; font-weight: bold;">
                                            &nbsp;
                                        </td>
                                        <td style="text-align: left; font-weight: bold;">
                                            Diff
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt96DifBidAsk" runat="server" CssClass="nortxt" MaxLength="10"></asp:TextBox>
                                        </td>
                                        <td style="text-align: left; font-weight: bold;">
                                            Diff
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txt99DifBidAsk" runat="server" CssClass="nortxt" MaxLength="10"></asp:TextBox>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td style="text-align: center; font-weight: bold">
                                            Inactive Time
                                            <asp:TextBox ID="TimeTxtBox" runat="server" MaxLength="5" Width="40px" CssClass="nortxt"></asp:TextBox>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            <div style="text-align: left;">
                                                <div style="float: left; width: 40px; text-align: center; margin-left: 5px;">
                                                    <asp:ImageButton ID="imgDown1" runat="server" ImageUrl="~/trade/admin/image/arrow_down.gif" />
                                                </div>
                                                <div style="float: left; width: 40px; text-align: center; margin-left: 5px;">
                                                    <asp:ImageButton ID="imgDown2" runat="server" ImageUrl="~/trade/admin/image/arrow_down.gif" />
                                                </div>
                                                <div style="float: left; width: 40px; text-align: center; margin-left: 5px;">
                                                    <asp:ImageButton ID="imgDown3" runat="server" ImageUrl="~/trade/admin/image/arrow_down.gif" />
                                                </div>
                                                <div style="clear: both;">
                                                </div>
                                            </div>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="14">
                                            <div style="text-align: left; float: left">
                                            </div>
                                            <div style="text-align: right; float: right">
                                            </div>
                                        </td>
                                    </tr>
                                </table>
                                <asp:HiddenField ID="hdfChangeColor" runat="server" />
                                <asp:HiddenField ID="hdfBid99" runat="server" />
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </div>
                </fieldset>
            </div>
            <div style="margin-left:.5cm">
                <asp:UpdatePanel ID="upFast" runat="server" UpdateMode="Conditional">
                    <ContentTemplate>
                        <table cellpadding="2" cellspacing="2" border="0" align="left">
                            <tr>
                                <td>
                                    <asp:CheckBox ID="cbConfirmSave" runat="server" Text="Fast Confirm" />
                                </td>
                                <td style="padding-left: 10px">
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server" RenderMode="Inline">
                                        <ContentTemplate>
                                            <asp:CheckBox ID="cbSortPrice" runat="server" Text="Sort By Purity Price" />
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                                <td style="padding-left: 10px">
                                    <asp:CheckBox ID="cbConfirm" runat="server" Text="Fast Trade" />
                                </td>
                                <td style="padding-left: 10px; height: 25px">
                                    <div id="div_fasttrade" style="display: none">
                                        <asp:RadioButtonList ID="rdoFashTrade" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Selected="True" Text="One Deal" Value="accept" />
                                            <asp:ListItem Text="All at the same price" Value="same" />
                                            <asp:ListItem Text="All at the same and lower/higher price" Value="lowerOrHigher" />
                                        </asp:RadioButtonList>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
            <br /><br />
            <div align="left" >
            <div>
            <asp:UpdateProgress ID="pgBuy" runat="server" AssociatedUpdatePanelID="upBuy">
                <ProgressTemplate>
                    <div id="divPgBuy" align="center" valign="middle" runat="server" style="position: absolute;
                        left: 43%; padding: 10px 10px 10px 10px; visibility: visible; border-color: silver;
                        border-style: solid; border-width: 1px; background-color: White;">
                        <img src="../img/indicator.gif" style="vertical-align: middle" alt="Processing" />
                        Loading ... &nbsp;
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            </div>
               <div style="clear:both"></div>
            <asp:UpdatePanel ID="upBuy" runat="server">
                <ContentTemplate>
                    <fieldset class="k-fieldset" style="width: 98%;">
                        <legend class="topic">Customer Buy</legend>
                        <asp:GridView ID="gvTradeBuy" runat="server" AutoGenerateColumns="False" BackColor="White"
                            BorderColor="#999999" BorderStyle="None" BorderWidth="0px" CellPadding="3" DataKeyNames="trade_id,type,price,gold_type_id"
                            Width="99%" DataSourceID="objSrcTradeBuy" AllowSorting="True">
                            <Columns>
                                <asp:BoundField DataField="trade_id" HeaderText="Ref_No" SortExpression="trade_id" FooterText="trade_id">
                                    <FooterStyle HorizontalAlign="Center" />
                                    <ItemStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="leave_order" HeaderText="Status" SortExpression="leave_order"
                                    FooterText="Status">
                                    <FooterStyle HorizontalAlign="Center" />
                                    <ItemStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="modifier_date" HeaderText="Deal Date" DataFormatString="{0:dd/MM/yyyy HH:mm}"
                                    SortExpression="created_date" FooterText="Deal Date">
                                    <FooterStyle HorizontalAlign="Center" />
                                    <ItemStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="firstname" HeaderText="Customer Name" SortExpression="firstname"
                                    FooterText="Customer Name">
                                    <FooterStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                 <%--FNG No level--%>
                                <asp:BoundField DataField="cust_level" HeaderText="Level" SortExpression="cust_level" Visible="false">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="gold_type_id" HeaderText="Purity" SortExpression="gold_type_id"
                                    FooterText="Purity">
                                    <FooterStyle HorizontalAlign="Center" />
                                    <ItemStyle Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="mode" HeaderText="Mode" SortExpression="mode" FooterText="Mode"
                                    Visible="False">
                                    <FooterStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="price" HeaderText="Price" SortExpression="price" DataFormatString="{0:#,##0.00}"
                                    FooterText="Price">
                                    <FooterStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField DataField="quantity" HeaderText="Quantity" SortExpression="quantity"
                                    DataFormatString="{0:#,##0.00000}" FooterText="Quantity">
                                    <FooterStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                </asp:BoundField>
                                <asp:BoundField HeaderText="Total Order">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="amount" HeaderText="Amount" SortExpression="amount" DataFormatString="{0:#,##0.00}"
                                    FooterText="Amount" Visible="False">
                                    <FooterStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Right" Wrap="False" />
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
                            <FooterStyle BackColor="#4F9502" ForeColor="White" Font-Bold="True" />
                            <HeaderStyle BackColor="#666666" ForeColor="White" />
                            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                            <AlternatingRowStyle BackColor="Gainsboro" />
                        </asp:GridView>
                        <asp:ObjectDataSource ID="objSrcTradeBuy" runat="server" SelectMethod="getTradeByMode"
                            TypeName="clsMain" OldValuesParameterFormatString="original_{0}">
                            <SelectParameters>
                                <asp:Parameter ConvertEmptyStringToNull="False" Name="mode" Type="String" />
                                <asp:Parameter ConvertEmptyStringToNull="False" Name="type" Type="String" />
                                <asp:Parameter ConvertEmptyStringToNull="False" Name="purity" Type="String" />
                                <asp:Parameter ConvertEmptyStringToNull="False" Name="cust_id" Type="String" />
                                <asp:Parameter ConvertEmptyStringToNull="False" Name="max_trade_id" Type="String" />
                                <asp:Parameter ConvertEmptyStringToNull="False" Name="sortPrice" Type="String" />
                                <asp:Parameter ConvertEmptyStringToNull="False" Name="period" Type="String" />
                                <asp:Parameter ConvertEmptyStringToNull="False" Name="pDate1" Type="DateTime" />
                                <asp:Parameter ConvertEmptyStringToNull="False" Name="pDate2" Type="DateTime" />
                                <asp:Parameter ConvertEmptyStringToNull="False" Name="onlyLeave" Type="String" />
                                <asp:Parameter ConvertEmptyStringToNull="False" Name="sale_id" Type="String" />
                            </SelectParameters>
                        </asp:ObjectDataSource>
                    </fieldset>
                </ContentTemplate>
            </asp:UpdatePanel>
            </div>
            <ajaxToolkit:ModalPopupExtender ID="mppAccept" BehaviorID="mppAccept" runat="server"
                TargetControlID="Panel1" PopupControlID="Panel1" OkControlID="btnOk" OnOkScript="okClick();"
                CancelControlID="btnNo" OnCancelScript="cancelClick();" BackgroundCssClass="modalBackground">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="Panel1" runat="server" Style="display: none" CssClass="modalPopup">
                <asp:Panel ID="Panel2" runat="server" Style="cursor: move; background-color: #DDDDDD;
                    border: solid 1px Gray; color: Black">
                    <div style="text-align: center">
                        <p>
                            <h4>
                                Do you want to Accept?</h4>
                        </p>
                    </div>
                </asp:Panel>
                <div style="text-align: center">
                    <p id="pContent" style="text-align: center; color: Red" />
                    <p>
                    </p>
                    <div id="div_ctrl" align="center" style="margin-top: .5cm; margin-bottom: .5cm" style="display: block">
                        <div id="div_normal" style="display: none">
                            <asp:DropDownList ID="ddlNormal" runat="server">
                                <asp:ListItem Selected="True" Text="Accept" Value="accept"></asp:ListItem>
                                <asp:ListItem Text="Accept All" Value="same"></asp:ListItem>
                            </asp:DropDownList>
                        </div>
                        <div id="div_fast" style="display: none">
                            <asp:DropDownList ID="ddlFast" runat="server">
                                <asp:ListItem Selected="True" Text="All at the same price" Value="same"></asp:ListItem>
                                <asp:ListItem Text="All at the same and lower/higher price" Value="lowerOrHigher"></asp:ListItem>
                            </asp:DropDownList>
                            
                        </div>
                        <asp:DropDownList ID="ddlConfirm" runat="server">
                            <asp:ListItem Selected="True" Text="Accept" Value="accept"></asp:ListItem>
                            <asp:ListItem Text="All at the same price" Value="same"></asp:ListItem>
                            <asp:ListItem Text="All at the same and lower/higher price" Value="lowerOrHigher"></asp:ListItem>
                        </asp:DropDownList>
                     
                    </div>
                    <asp:Button ID="btnOk" runat="server" CssClass="buttonPro small grey" Text="OK" Width="50px" />
                    &nbsp;<asp:Button ID="btnNo" runat="server" CssClass="buttonPro small grey" Text="No"
                        Width="50px" />
                    <p>
                    </p>
                    <p>
                    </p>
                    <p>
                    </p>
                </div>
            </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="mppDel" BehaviorID="mppDel" runat="server" TargetControlID="Panel3"
                PopupControlID="Panel3" OkControlID="btnOk1" OnOkScript="okClick1();" CancelControlID="btnNo1"
                OnCancelScript="cancelClick1();" BackgroundCssClass="modalBackground">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="Panel3" runat="server" Style="display: none" CssClass="modalPopup">
                <asp:Panel ID="Panel4" runat="server" Style="cursor: move; background-color: #DDDDDD;
                    border: solid 1px Gray; color: Black">
                    <div style="text-align: center">
                        <p id="p1" style="text-align: justify">
                        </p>
                        <br />
                        <div>
                            <h4>
                                Please Enter Password to Reject.</h4>
                            <asp:TextBox ID="txtPwd" runat="server" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPwd" ValidationGroup="mppPwd" ControlToValidate="txtPwd"
                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </asp:Panel>
                <div>
                    <p style="text-align: center;">
                        <asp:Button ID="btnOk1" runat="server" CssClass="buttonPro small grey" ValidationGroup="mppPwd"
                            Text="Yes" Width="50px" />
                        <asp:Button ID="btnNo1" runat="server" CssClass="buttonPro small grey" Text="No"
                            Width="50px" />
                    </p>
                </div>
            </asp:Panel>
            <ajaxToolkit:ModalPopupExtender ID="mppPer" BehaviorID="mppPer" runat="server" TargetControlID="Panel5"
                PopupControlID="Panel5" OkControlID="btnOkPer" OnOkScript="okClickPer();" CancelControlID="btnNoPer"
                OnCancelScript="cancelClickPer();" BackgroundCssClass="modalBackground">
            </ajaxToolkit:ModalPopupExtender>
            <asp:Panel ID="Panel5" runat="server" Style="display: none" CssClass="modalPopup">
                <asp:Panel ID="Panel6" runat="server" Style="cursor: move; background-color: #DDDDDD;
                    border: solid 1px Gray; color: Black">
                    <div style="text-align: center">
                        <p id="p2" style="text-align: justify">
                        </p>
                        <br />
                        <div>
                            <h4>
                                Bid 99.99 more than the percentage specified. Please Enter Password to Save.</h4>
                            <asp:TextBox ID="txtPwdPer" runat="server" TextMode="Password"></asp:TextBox>
                            <asp:RequiredFieldValidator ID="rfvPer" ValidationGroup="mppPer" ControlToValidate="txtPwdPer"
                                runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                        </div>
                    </div>
                </asp:Panel>
                <div>
                    <p style="text-align: center;">
                        <asp:Button ID="btnOkPer" runat="server" CssClass="buttonPro small grey" ValidationGroup="mppPer"
                            Text="Yes" Width="50px" />
                        <asp:Button ID="btnNoPer" runat="server" CssClass="buttonPro small grey" Text="No"
                            Width="50px" />
                    </p>
                </div>
            </asp:Panel>
        </div>
    </div>
    </form>
</body>
</html>
