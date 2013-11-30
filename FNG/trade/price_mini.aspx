<%@ Page Language="VB" AutoEventWireup="false" CodeFile="price_mini.aspx.vb" Inherits="price_mini" %>

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

            $get('lblBid96').style.color = color;
            $get('lblAsk96').style.color = color;

        }

        function OnSucceeded(result, userContext, methodName) {

            //if($get('hdfLv').value==''){return;}
            var halt = result.toString().split('|')[3];
            var systemHalt = result.toString().split('|')[4];
            var newBidAsk = result.toString().split('|')[0];
            var bidask = $get('lblBid96').innerHTML + ',' + $get('lblAsk96').innerHTML

            $get('hdfMax').value = result.toString().split('|')[2].split(',');
            $get('lblMax96').innerHTML = '(ปริมาณซื้อขายสูงสุด ' + result.toString().split('|')[2].split(',')[1] + " บาท ต่อครั้ง)";

            //0 check system halt
            //1 check halt
            //2 check time out
            if (systemHalt == 'y') {
                top.window.location = 'login.aspx?h=y';
            }
            if (halt == 'y') {
                setColor('silver');
            } else {
                if (result.toString().split('|')[1] == 'y') {
                    setColor('white');

                } else {
                    setColor('silver');
                }
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

        function setPriceToTrade(bid96, ask96) {
            $(document).ready(function () {

                var preBid96 = top.window.document.getElementById('ctl00_MainContent_hdfBid96Mini').value;
                var preAsk96 = top.window.document.getElementById('ctl00_MainContent_hdfAsk96Mini').value;

                var eff = 'bounce'; var effCount = 3; effTime = 500;
                var upColor = '#00CC00'; var downColor = 'red';

                //96
                if (parseFloat(preBid96) > parseFloat(bid96)) {
                    $get('lblBid96').style.color = downColor;
                    $("#dBid96").effect(eff, { times: effCount }, effTime);
                }
                if (parseFloat(preBid96) < parseFloat(bid96)) {
                    $get('lblBid96').style.color = upColor;
                    $("#dBid96").effect(eff, { times: effCount }, effTime);
                }

                if (parseFloat(preAsk96) > parseFloat(ask96)) {
                    $get('lblAsk96').style.color = downColor;
                    $("#dAsk96").effect(eff, { times: effCount }, effTime);
                }
                if (parseFloat(preAsk96) < parseFloat(ask96)) {
                    $get('lblAsk96').style.color = upColor;
                    $("#dAsk96").effect(eff, { times: effCount }, effTime);
                }

                top.window.document.getElementById('ctl00_MainContent_hdfBid96Mini').value = bid96;
                top.window.document.getElementById('ctl00_MainContent_hdfAsk96Mini').value = ask96;
            });
        }

        function OnFailed(error, userContext, methodName) { }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="SM" EnablePageMethods="true" runat="server" />
    <div align="center">
        <asp:UpdatePanel runat="server" ID="upPrice" UpdateMode="Conditional">
            <ContentTemplate>
                <div id="div96" style="text-align: center; padding-right: 50px; width: 600px;margin-left:40px">
                    <table cellpadding="1" cellspacing="1" style="text-align: center">
                        <%--<tr>
                            <td colspan="2" class="text_head2" background="image_cust/head_bg2.jpg" height="40"
                                align="center">
                                ราคา 96.50
                            </td>
                        </tr>--%>
                        <tr>
                            <td>
                                <div id="dv_bid_c96">
                                    <table background="image_cust/price_bg.jpg" width="250" height="150" border="0" cellspacing="0"
                                        cellpadding="0">
                                        <tr>
                                            <td height="60" align="center" class="text_white18">
                                                ลูกค้าขาย 96.50
                                            </td>
                                        </tr>
                                        <tr>
                                            <td height="80" align="center" class="text_white">
                                                <div id="dBid96">
                                                    <asp:Label runat="server" ID="lblBid96" />
                                                </div>
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                            </td>
                            <td>
                                
                                    <div id="dv_ask_c96">
                                        <table background="image_cust/price_bg_red.jpg" width="250" height="150" border="0"
                                            cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td height="60" align="center" class="text_white18">
                                                    ลูกค้าซื้อ 96.50
                                                </td>
                                            </tr>
                                            <tr>
                                                <td height="80" align="center" class="text_white">
                                                    <div id="dAsk96">
                                                        <asp:Label runat="server" ID="lblAsk96" />
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
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <asp:HiddenField ID="hdfMax" runat="server" />
    <asp:HiddenField ID="hdfLv" runat="server" />
    <asp:HiddenField ID="hdfCust_id" runat="server" />
    <input id="hdfPrice" type="hidden" />
    </form>
</body>
</html>
