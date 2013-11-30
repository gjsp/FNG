<%@ Page Language="VB" AutoEventWireup="false" CodeFile="price_change.aspx.vb" Inherits="trade_admin_price_change" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="style.css" rel="stylesheet" type="text/css" />
    <script src="../../js/functionst.js" type="text/javascript"></script>
    <link href="../../button/css/buttonPro.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        function addBidAsk(direct, txt) {

            if (direct == 'up') {
                $get(txt).value = (parseFloat($get(txt).value) + 1).toString();
            }
            else {
                $get(txt).value = (parseFloat($get(txt).value) - 1).toString();
            }
        }

        function calBidAsk() {
            __doPostBack('upPrice', '');
        }

        function calBid() {
            var bidPlus = $get('<%=txt99Bid.ClientId %>').value;
            var bid = $get('<%=lbl99Bid.ClientId %>').innerHTML.replace(',', '');
            var result = parseFloat(bid) - parseFloat(bidPlus)
            $get('<%=lbl99BidSelf.ClientId %>').innerHTML = result + ".00";
        }
        function calAsk() {
            var AskPlus = $get('<%=txt99Ask.ClientId %>').value;
            var Ask = $get('<%=lbl99Ask.ClientId %>').innerHTML.replace(',', '');
            var result = parseFloat(Ask) + parseFloat(AskPlus)
            $get('<%=lbl99AskSelf.ClientId %>').innerHTML = result+ ".00";
        }
        function Check() {

            PageMethods.getPrice(OnSucceeded, OnFailed);
        }
        function setColor(color) {
            if (color == 'black' || color == 'white') {
                $get('lbl99Bid').style.color = '#0066FF';
                $get('lbl99Ask').style.color = '#FF6600';
                $get('lbl96Bid').style.color = '#0066FF';
                $get('lbl96Ask').style.color = '#FF6600';
            }
            else {
                $get('lbl99Bid').style.color = color;
                $get('lbl99Ask').style.color = color;
                $get('lbl96Bid').style.color = color;
                $get('lbl96Ask').style.color = color;
            }
        }
        function OnSucceeded(result, userContext, methodName) {
            var halt = result.toString().split('|')[3];
            var newBidAsk = result.toString().split('|')[0];
            var bidask = $get('lbl99Bid').innerHTML + ',' + $get('lbl99Ask').innerHTML + ',' + $get('lbl96Bid').innerHTML + ',' + $get('lbl96Ask').innerHTML

            //1 check halt
            //2 check time out
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
        function setPriceToTrade(bid99, ask99, bid96, ask96) {
            top.window.document.getElementById('ctl00_ContentPlaceHolder1_hdfKgBid99').value = bid99;
            top.window.document.getElementById('ctl00_ContentPlaceHolder1_hdfKgAsk99').value = ask99;
            top.window.document.getElementById('ctl00_ContentPlaceHolder1_hdfKgBid96').value = bid96;
            top.window.document.getElementById('ctl00_ContentPlaceHolder1_hdfKgAsk96').value = ask96;
        }

        function OnFailed(error, userContext, methodName) { }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="SM" EnablePageMethods="true" runat="server" />
        <asp:UpdatePanel runat="server" ID="upPrice">
            <ContentTemplate>
                <div style="text-align: left; margin-left: 100px">
                                <asp:UpdateProgress ID="UpdateProg1" runat="server">
                                    <ProgressTemplate>
                                        <div id="Div1" align="center" valign="middle" runat="server" style="position: absolute;
                                            left: 35%; padding: 10px 10px 10px 10px; visibility: visible; border-color: silver;
                                            border-style: solid; border-width: 1px; background-color: White;">
                                            <img src="../img/indicator.gif" style="vertical-align: middle" alt="Processing" />
                                            Loading ... &nbsp;
                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>

                    <table width="700px">
                        <tr>
                            <td style="width: 30%; vertical-align: middle; font-weight: bold">
                                Gold 99.99
                            </td>
                            <td>
                                <div style="float: left; width: 40px; text-align: center; margin-left: 5px;">
                                    <asp:ImageButton ID="img99BidUp" runat="server" ImageUrl="~/Trade/admin/Image/arrow_up.gif"
                                        Width="18px" Height="10px" />
                                </div>
                                <br />
                                <div style="float: left; width: 40px; text-align: center; margin-left: 5px; height: 26px;">
                                    <asp:TextBox ID="txt99Bid" runat="server" Height="16px" MaxLength="2" Style="text-align: right;
                                        width: 20px; font-size: 12px; height: 12px; font-weight: bold"></asp:TextBox>
                                </div>
                                <div style="float: left; width: 40px; text-align: center; margin-left: 5px;">
                                    <asp:ImageButton ID="img99BidDown" runat="server" ImageUrl="~/Trade/admin/Image/arrow_down.gif"
                                        Width="18px" Height="10px" />
                                </div>
                            </td>
                            <td style="width: 25%">
                                <div style="text-align: center">
                                  <span style="font-weight:bold">Bid</span>
                                  <br />
                                    <asp:Label ID="lbl99BidSelf" runat="server" CssClass="bidtxt" Font-Bold="true" Text=""></asp:Label><br />
                                </div>
                            </td>
                            <td style="width: 25%">
                                <div style="text-align: center">
                                    <span style="font-weight:bold">Bid GCAP</span>
                                    <br />
                                    <asp:Label ID="lbl99Bid" runat="server" CssClass="bidtxt" Font-Bold="true" Text=""></asp:Label>
                                </div>
                            </td>
                            <td style="width: 25%">
                                <div style="text-align: center">
                                   <span style="font-weight:bold">Ask GCAP</span>
                                  <br />
                                    <asp:Label ID="lbl99Ask" runat="server" CssClass="asktxt" Font-Bold="true" Text=""></asp:Label>
                                </div>
                            </td>
                            <td style="width: 25%">
                                <div style="text-align: center">
                                    <span style="font-weight:bold">Ask</span>
                                    <br />
                                    <asp:Label ID="lbl99AskSelf" runat="server" CssClass="asktxt" Font-Bold="true" Text=""></asp:Label><br />
                                </div>
                            </td>
                            <td>
                                <div style="float: left; width: 40px; text-align: center; margin-left: 5px;">
                                    <asp:ImageButton ID="img99AskUp" runat="server" ImageUrl="~/Trade/admin/Image/arrow_up.gif"
                                        Width="18px" Height="10px" />
                                </div>
                                <br />
                                <div style="float: left; width: 40px; text-align: center; margin-left: 5px; height: 26px;">
                                    <asp:TextBox ID="txt99Ask" runat="server" Height="16px" MaxLength="2" Style="text-align: right;
                                        width: 20px; font-size: 12px; height: 12px; font-weight: bold"></asp:TextBox>
                                </div>
                                <div style="float: left; width: 40px; text-align: center; margin-left: 5px;">
                                    <asp:ImageButton ID="img99AskDown" runat="server" ImageUrl="~/Trade/admin/Image/arrow_down.gif"
                                        Width="18px" Height="10px" />
                                </div>
                            </td>
                            <td align="center" width="200px">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td colspan="8">
                                <hr />
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 30%; vertical-align: middle; font-weight: bold">
                                Gold 96.50
                            </td>
                            <td>
                                <div style="float: left; width: 40px; text-align: center; margin-left: 5px;">
                                    <asp:ImageButton ID="img96BidUp" runat="server" ImageUrl="~/Trade/admin/Image/arrow_up.gif"
                                        Width="18px" Height="10px" />
                                </div>
                                <br />
                                <div style="float: left; width: 40px; text-align: center; margin-left: 5px; height: 26px;">
                                    <asp:TextBox ID="txt96Bid" runat="server" Height="16px" MaxLength="2" Style="text-align: right;
                                        width: 20px; font-size: 12px; height: 12px; font-weight: bold"></asp:TextBox>
                                </div>
                                <div style="float: left; width: 40px; text-align: center; margin-left: 5px;">
                                    <asp:ImageButton ID="img96BidDown" runat="server" ImageUrl="~/Trade/admin/Image/arrow_down.gif"
                                        Width="18px" Height="10px" />
                                </div>
                            </td>
                            <td style="width: 25%">
                                <div style="text-align: center">
                                    <span style="font-weight:bold">Bid</span>
                                    <br />
                                    <asp:Label ID="lbl96BidSelf" runat="server" CssClass="bidtxt" Font-Bold="True"></asp:Label><br />
                                </div>
                            </td>
                            <td style="width: 25%">
                                <div style="text-align: center">
                                    <span style="font-weight:bold">Bid Gcap</span>
                                    <br />
                                    <asp:Label ID="lbl96Bid" runat="server" CssClass="bidtxt" Font-Bold="True"></asp:Label>
                                </div>
                            </td>
                            <td style="width: 25%">
                                <div style="text-align: center">
                                   <span style="font-weight:bold">Ask GCAP</span>
                                   <br />
                                    <asp:Label ID="lbl96Ask" runat="server" CssClass="asktxt" Font-Bold="True"></asp:Label>
                                </div>
                            </td>
                            <td style="width: 25%">
                                <div style="text-align: center">
                                    <span style="font-weight:bold">Ask</span>
                                    <br />
                                    <asp:Label ID="lbl96AskSelf" runat="server" CssClass="asktxt" Font-Bold="True"></asp:Label><br />
                                </div>
                            </td>
                            <td>
                                <div style="float: left; width: 40px; text-align: center; margin-left: 5px;">
                                    <asp:ImageButton ID="img96AskUp" runat="server" ImageUrl="~/Trade/admin/Image/arrow_up.gif"
                                        Width="18px" Height="10px" />
                                </div>
                                <br />
                                <div style="float: left; width: 40px; text-align: center; margin-left: 5px; height: 26px;">
                                    <asp:TextBox ID="txt96Ask" runat="server" Height="16px" MaxLength="2" Style="text-align: right;
                                        width: 20px; font-size: 12px; height: 12px; font-weight: bold"></asp:TextBox>
                                </div>
                                <div style="float: left; width: 40px; text-align: center; margin-left: 5px;">
                                    <asp:ImageButton ID="img96AskDown" runat="server" ImageUrl="~/Trade/admin/Image/arrow_down.gif"
                                        Width="18px" Height="10px" />
                                </div>
                            </td>
                            <td align="center" width="25%">
                                &nbsp;
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 30%; vertical-align: top; font-weight: bold">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td style="width: 25%">
                                &nbsp;
                            </td>
                            <td colspan="2">
                                <asp:Button ID="btnHalt" runat="server" BorderColor="Black" CssClass="buttonPro small black"
                                    Text="Halt" Width="60px" />
                                &nbsp;<asp:Button ID="btnReset" runat="server" CssClass="buttonPro small black" Text="Reset"
                                    Width="60px" />
                                &nbsp;<asp:Button ID="btnSave" runat="server" CssClass="buttonPro small black" Text="Save"
                                    Width="60px" />
                            </td>
                            <td style="width: 25%">
                                &nbsp;
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td align="center" width="25%">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:HiddenField ID="hdfLv" runat="server" />
    </div>
    </form>
</body>
</html>
