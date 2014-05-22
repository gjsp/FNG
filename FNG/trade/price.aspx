<%@ Page Language="VB" AutoEventWireup="false" CodeFile="price.aspx.vb" Inherits="price" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="style_cust.css" rel="stylesheet" type="text/css" />
    <link href="style/style_cust.css" rel="stylesheet" type="text/css" />
    <script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css"
        rel="stylesheet" type="text/css" />
    <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js"></script>
    <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"></script>
    <script language="javascript" type="text/javascript">

        function Check() {
            var cust_id = $get('hdfCust_id').value
            PageMethods.getPrice(cust_id, OnSucceeded, OnFailed);
        }

        function setColor(color) {
            var colorBid = color;
            var colorAsk = color;

            if (color == 'black') {
                colorBid = 'red';
                colorAsk = '#0000A0'; //blue
            }
            $get('lblBid99').style.color = colorBid;
            $get('lblAsk99').style.color = colorAsk;
            $get('lblBid96').style.color = colorBid;
            $get('lblAsk96').style.color = colorAsk;
            $get('lblBid96Mini').style.color = colorBid;
            $get('lblAsk96Mini').style.color = colorAsk;

        }

        function OnSucceeded(result, userContext, methodName) {

            //if($get('hdfLv').value==''){return;}
            var systemHalt = result.toString().split('|')[3];
            var newBidAsk = result.toString().split('|')[0];
            var bidask = $get('lblBid99').innerHTML + ',' + $get('lblAsk99').innerHTML + ',' + $get('lblBid96').innerHTML + ',' + $get('lblAsk96').innerHTML + ',' + $get('lblBid96Mini').innerHTML + ',' + $get('lblAsk96Mini').innerHTML

            $get('hdfMax').value = result.toString().split('|')[2].split(',');
            $get('lblMax99').innerHTML = '(ปริมาณซื้อขายสูงสุด ' + result.toString().split('|')[2].split(',')[0] + " กิโล ต่อครั้ง)";
            $get('lblMax96').innerHTML = '(ปริมาณซื้อขายสูงสุด ' + result.toString().split('|')[2].split(',')[1] + " บาท ต่อครั้ง)";
            $get('lblMax96Mini').innerHTML = '(ปริมาณซื้อขายสูงสุด ' + result.toString().split('|')[2].split(',')[2] + " บาท ต่อครั้ง)";

            //0 check system halt
            //1 check halt
            //2 check time out
            if (systemHalt == 'y') {
                //top.window.location = 'login.aspx?h=y';
                top.window.location = 'cust_halt.aspx';
            }

            if (bidask != newBidAsk) {
                __doPostBack('upPrice', '');
            }
            else {
                setTimeout("Check()", 3000);
            }
        }

        function pageLoad() {
            setTimeout("Check()", 3000);
        }

        function setPriceToTrade(bid99, ask99, bid96, ask96, bid96Mini, ask96Mini) {
            $(document).ready(function () {
                var preBid99 = top.window.document.getElementById('ctl00_MainContent_hdfBid99').value;
                var preAsk99 = top.window.document.getElementById('ctl00_MainContent_hdfAsk99').value;
                var preBid96 = top.window.document.getElementById('ctl00_MainContent_hdfBid96').value;
                var preAsk96 = top.window.document.getElementById('ctl00_MainContent_hdfAsk96').value;
                var preBid96Mini = top.window.document.getElementById('ctl00_MainContent_hdfBid96Mini').value;
                var preAsk96Mini = top.window.document.getElementById('ctl00_MainContent_hdfAsk96Mini').value;

                var eff = 'bounce'; var effCount = 3; effTime = 500;
                var upColor = 'green'; var downColor = 'red';

                //99
                if (parseFloat(preBid99) > parseFloat(bid99)) {
                    //$get('lblBid99').style.color = downColor;
                    $("#dBid99").effect(eff, { times: effCount }, effTime);
                }
                if (parseFloat(preBid99) < parseFloat(bid99)) {
                    //$get('lblBid99').style.color = upColor;
                    $("#dBid99").effect(eff, { times: effCount }, effTime);
                }

                if (parseFloat(preAsk99) > parseFloat(ask99)) {
                    //$get('lblAsk99').style.color = downColor;
                    $("#dAsk99").effect(eff, { times: effCount }, effTime);
                }
                if (parseFloat(preAsk99) < parseFloat(ask99)) {
                    //$get('lblAsk99').style.color = upColor;
                    $("#dAsk99").effect(eff, { times: effCount }, effTime);
                }

                //96
                if (parseFloat(preBid96) > parseFloat(bid96)) {
                    //$get('lblBid96').style.color = downColor;
                    $("#dBid96").effect(eff, { times: effCount }, effTime);
                }
                if (parseFloat(preBid96) < parseFloat(bid96)) {
                    //$get('lblBid96').style.color = upColor;
                    $("#dBid96").effect(eff, { times: effCount }, effTime);
                }

                if (parseFloat(preAsk96) > parseFloat(ask96)) {
                    //$get('lblAsk96').style.color = downColor;
                    $("#dAsk96").effect(eff, { times: effCount }, effTime);
                }
                if (parseFloat(preAsk96) < parseFloat(ask96)) {
                    //$get('lblAsk96').style.color = upColor;
                    $("#dAsk96").effect(eff, { times: effCount }, effTime);
                }

                //96Mini
                if (parseFloat(preBid96Mini) > parseFloat(bid96Mini)) {
                    //$get('lblBid96Mini').style.color = downColor;
                    $("#dBid96Mini").effect(eff, { times: effCount }, effTime);
                }
                if (parseFloat(preBid96Mini) < parseFloat(bid96Mini)) {
                    //$get('lblBid96Mini').style.color = upColor;
                    $("#dBid96Mini").effect(eff, { times: effCount }, effTime);
                }

                if (parseFloat(preAsk96Mini) > parseFloat(ask96Mini)) {
                    //$get('lblAsk96Mini').style.color = downColor;
                    $("#dAsk96Mini").effect(eff, { times: effCount }, effTime);
                }
                if (parseFloat(preAsk96Mini) < parseFloat(ask96Mini)) {
                    //$get('lblAsk96Mini').style.color = upColor;
                    $("#dAsk96Mini").effect(eff, { times: effCount }, effTime);
                }
                top.window.document.getElementById('ctl00_MainContent_hdfBid99').value = bid99;
                top.window.document.getElementById('ctl00_MainContent_hdfAsk99').value = ask99;
                top.window.document.getElementById('ctl00_MainContent_hdfBid96').value = bid96;
                top.window.document.getElementById('ctl00_MainContent_hdfAsk96').value = ask96;
                top.window.document.getElementById('ctl00_MainContent_hdfBid96Mini').value = bid96Mini;
                top.window.document.getElementById('ctl00_MainContent_hdfAsk96Mini').value = ask96Mini;
            });
        }

        function OnFailed(error, userContext, methodName) {
            top.window.location = 'login.aspx?h=y';
        }
    </script>
    <style type="text/css">
        .strBid
        {
            color: #006600;
        }
        .strAsk
        {
            color: #CC3300;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="SM" EnablePageMethods="true" runat="server" />
    <asp:UpdatePanel runat="server" ID="upPrice" UpdateMode="Conditional">
        <ContentTemplate>
        <div align="center">
            <table cellpadding="5" cellspacing="5" border="0" class="text_head2" width="96%">
                <tr>
                    <td align="right">
                    Gold Spot 
                    </td>
                    <td>
                        ลูกค้าขาย <asp:Label ID="lblbidSpot" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                    <td>
                        ลูกค้าซื้อ <asp:Label ID="lblaskSpot" runat="server" ForeColor="#0000A0"></asp:Label>
                    </td>
                    <td align="left">
                        ดอลลาร์/ออนซ์ 
                    </td>
                    <td align="right">
                    อัตราแลกเปลี่ยน 
                    </td>
                    <td>
                        ลูกค้าขาย <asp:Label ID="lblbidThb" runat="server" ForeColor="Red"></asp:Label>
                    </td>
                    <td>
                         ลูกค้าซื้อ <asp:Label ID="lblaskThb" runat="server" ForeColor="#0000A0"></asp:Label>
                    </td>
                    <td align="left">
                   บาท
                    </td>
                </tr>
                
            </table>
        </div>
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td>
                        <div id="div99" style="text-align: center; float: left; padding-left: 20px">
                            <table cellpadding="1" cellspacing="1">
                                <tr>
                                    <td colspan="2" class="text_head2" background="image_cust/head_bg2.jpg" height="40"
                                        align="center">
                                        ราคา 99.99
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div id="dv_bid_c99">
                                            <table width="200" cellspacing="0" style="border: 2px solid #CC3300" cellpadding="0">
                                                <tr>
                                                    <td height="30" align="center" class="text_white18" style="color: #CC3300">
                                                        ลูกค้าขาย 99.99
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="80" align="center" class="text_white">
                                                        <div id="dBid99">
                                                            <asp:Label runat="server" ID="lblBid99" ForeColor="red" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                    <td>
                                        <div id="dv_ask_c99">
                                            <table width="200" style="border: 2px solid #0000A0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td height="30" align="center" class="text_white18" style="color: #0000A0">
                                                        ลูกค้าซื้อ 99.99
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="80" align="center" class="text_white">
                                                        <div id="dAsk99">
                                                            <asp:Label runat="server" ID="lblAsk99" ForeColor="#0000A0" /></div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center" height="30" class="textbody_th">
                                        <asp:Label ID="lblMax99" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td>
                        <div id="div96" style="text-align: center; float: left; padding-left: 20px;">
                            <table cellpadding="1" cellspacing="1">
                                <tr>
                                    <td colspan="2" class="text_head2" background="image_cust/head_bg2.jpg" height="40"
                                        align="center">
                                        ราคา 96.50
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div id="dv_bid_c96">
                                            <table width="200" cellspacing="0" style="border: 2px solid #CC3300" cellpadding="0">
                                                <tr>
                                                    <td height="30" align="center" class="text_white18" style="color: #CC3300">
                                                        ลูกค้าขาย 96.50
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="80" align="center" class="text_white">
                                                        <div id="dBid96">
                                                            <asp:Label runat="server" ID="lblBid96" ForeColor="red" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                    <td>
                                        <div id="dv_ask_c96">
                                            <table width="200" style="border: 2px solid #0000A0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td height="30" align="center" class="text_white18" style="color: #0000A0">
                                                        ลูกค้าซื้อ 96.50
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="80" align="center" class="text_white">
                                                        <div id="dAsk96">
                                                            <asp:Label runat="server" ID="lblAsk96" ForeColor="#0000A0" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center" height="30" class="textbody_th">
                                        <asp:Label ID="lblMax96" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                    <td>
                        <div id="divMini" style="text-align: center; float: left; padding-left: 20px">
                            <table cellpadding="1" cellspacing="1">
                                <tr>
                                    <td colspan="2" class="text_head2" background="image_cust/head_bg2.jpg" height="40"
                                        align="center">
                                        ราคา 96.50 Mini
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <div id="Div2">
                                            <table width="200" cellspacing="0" style="border: 2px solid #CC3300" cellpadding="0">
                                                <tr>
                                                    <td height="30" align="center" class="text_white18" style="color: #CC3300">
                                                        ลูกค้าขาย 96.50 Mini
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="80" align="center" class="text_white">
                                                        <div id="dBid96Mini">
                                                            <asp:Label runat="server" ID="lblBid96Mini" ForeColor="red" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                    <td>
                                        <div id="Div4">
                                            <table width="200" style="border: 2px solid #0000A0" cellspacing="0" cellpadding="0">
                                                <tr>
                                                    <td height="30" align="center" class="text_white18" style="color: #0000A0">
                                                        ลูกค้าซื้อ 96.50 Mini
                                                    </td>
                                                </tr>
                                                <tr>
                                                    <td height="80" align="center" class="text_white">
                                                        <div id="dAsk96Mini">
                                                            <asp:Label runat="server" ID="lblAsk96Mini" ForeColor="#0000A0" />
                                                        </div>
                                                    </td>
                                                </tr>
                                            </table>
                                        </div>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="2" align="center" height="30" class="textbody_th">
                                        <asp:Label ID="lblMax96Mini" runat="server"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
            </table>
        </ContentTemplate>
    </asp:UpdatePanel>
    <div style="display: none">
        <asp:HiddenField ID="hdfMax" runat="server" />
        <asp:HiddenField ID="hdfLv" runat="server" />
        <asp:HiddenField ID="hdfCust_id" runat="server" />
        <input id="hdfPrice" type="hidden" />
    </div>
    </form>
</body>
<%--<script type="text/javascript">
    blinkMe();
    function blinkMe() {
        if (document.getElementById('sBid99').style.color == 'white') {
            document.getElementById('sBid99').style.color = "silver";
        }
        else {
            document.getElementById('sBid99').style.color = 'white';
        }
        setTimeout("blinkMe();", 900);
    }
</script>--%>
</html>
