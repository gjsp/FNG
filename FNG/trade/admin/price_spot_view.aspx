<%@ Page Language="VB" AutoEventWireup="false" CodeFile="price_spot_view.aspx.vb" Inherits="trade_admin_price_spot_view" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="style.css" rel="stylesheet" type="text/css" />
    <script src="../../js/functionst.js" type="text/javascript"></script>
    <link href="../../button/css/buttonPro.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

            function Check() {
            PageMethods.getPrice(OnSucceeded, OnFailed);
        }

        function OnSucceeded(result, userContext, methodName) 
        {
            var newBidAsk = result.toString();
            var l = 'l';
            var bidask = $get('lbl99Bid').innerHTML + l + $get('lbl99Ask').innerHTML + l + $get('lblBidThb').innerHTML + l + $get('lblAskThb').innerHTML

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
     
        function OnFailed(error, userContext, methodName) { }
    </script>
    <style>
    .bidtxt
{
    text-align: right;
    width: 55px;
    font-size: 12px;
    height: 12px;
    color: #0066FF;
    font-weight: bold;
    font-size: x-large;
}
.asktxt{text-align: right; width: 55px; font-size: 12px; height: 12px;color: #FF6600; font-weight: bold;font-size: x-large}
.nortxt{text-align: right; width: 55px; font-size: 12px; height: 12px;color: #000000; font-weight: bold;font-size: x-large}
    </style>
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
                                left: 35%; padding: 10px 10px 10px 10px; visibility: visible;
                                border-color: silver; border-style: solid; border-width: 1px; background-color: White;">
                                <img src="../img/indicator.gif" style="vertical-align: middle" alt="Processing" />
                                Loading ... &nbsp;
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <table width="800px" cellpadding="5" cellspacing="5">
                        <tr>
                            <td style="width:300px; vertical-align: middle; font-weight: bold">
                                Gold Spot 99.99 %</td>
                            <td>
                                &nbsp;</td>
                            <td style="width:200px;text-align:right"">
                               
                                <asp:Label ID="lbl99Bid" runat="server" CssClass="bidtxt" Font-Bold="true" 
                                    Text=""></asp:Label>
                               
                            </td>
                            <td style="width:100px;text-align:center">
                               
                                    &nbsp;</td>
                            <td style="width: 200px;text-align:right">
                               
                                    <asp:Label ID="lbl99Ask" runat="server" CssClass="asktxt" Font-Bold="true" Text=""></asp:Label>
                              
                            </td>
                            <td style="width: 100px">
                                &nbsp;</td>
                            <td style="width: 200px; font-weight: bold">
                               ดอลลาร์/ออนซ์</td>
                        </tr>
                        <tr>
                            <td style="vertical-align: middle; font-weight: bold">
                                อัตราแลกเปลี่ยน</td>
                            <td>
                                &nbsp;</td>
                            <td style="text-align:right"">
                                <asp:Label ID="lblBidThb" runat="server" CssClass="bidtxt" Font-Bold="True"></asp:Label>
                            </td>
                            <td style="text-align:center">
                                &nbsp;</td>
                            <td style="text-align:right">
                                <asp:Label ID="lblAskThb" runat="server" CssClass="asktxt" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td style="width: 200px; font-weight: bold">
                                บาท</td>
                        </tr>
                        <tr>
                            <td style="width:300px; vertical-align: middle; font-weight: bold">
                                Gold 99.99 %</td>
                            <td>
                                &nbsp;</td>
                            <td style="width:200px;text-align:right"">
                               
                                <asp:Label ID="lbl99BidKg" runat="server" CssClass="bidtxt" Font-Bold="true" 
                                    Text=""></asp:Label>
                               
                            </td>
                            <td style="width:100px;text-align:center">
                               
                                    &nbsp;</td>
                            <td style="width: 200px;text-align:right">
                               
                                    <asp:Label ID="lbl99AskKg" runat="server" CssClass="asktxt" Font-Bold="true" Text=""></asp:Label>
                              
                            </td>
                            <td style="width: 100px">
                                &nbsp;</td>
                            <td style="width: 200px; font-weight: bold">
                               กิโลกรัม</td>
                        </tr>
                        <tr>
                            <td style="vertical-align: middle; font-weight: bold">
                                Gold 99.99%</td>
                            <td>
                                &nbsp;</td>
                            <td style="text-align:right"">
                                <asp:Label ID="lbl99BidBg" runat="server" CssClass="bidtxt" Font-Bold="True"></asp:Label>
                            </td>
                            <td style="text-align:center">
                                &nbsp;</td>
                            <td style="text-align:right">
                                <asp:Label ID="lbl99AskBg" runat="server" CssClass="asktxt" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td style="width: 200px; font-weight: bold">
                                บาททอง</td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold">
                                Gold 96.50%</td>
                            <td>
                                &nbsp;</td>
                            <td  style="text-align:right"">
                                <asp:Label ID="lbl96Bid" runat="server" CssClass="bidtxt" Font-Bold="True"></asp:Label>
                            </td>
                            <td  style="text-align:center">
                                &nbsp;</td>
                            <td style="text-align:right">
                                <asp:Label ID="lbl96Ask" runat="server" CssClass="asktxt" Font-Bold="True"></asp:Label>
                            </td>
                            <td>
                                &nbsp;</td>
                            <td style="width: 200px; font-weight: bold">
                                บาททอง</td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top; font-weight: bold" colspan="7" align="center">
                               
                                &nbsp;&nbsp;</td>
                        </tr>
                    </table>
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    </form>
</body>
</html>
