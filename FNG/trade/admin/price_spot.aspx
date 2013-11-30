<%@ Page Language="VB" AutoEventWireup="false" CodeFile="price_spot.aspx.vb" Inherits="trade_admin_price_spot" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="style.css" rel="stylesheet" type="text/css" />
    <script src="../../js/functionst.js" type="text/javascript"></script>
    <link href="../../button/css/buttonPro.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        function checkPremium() {
            e_k = event.keyCode;
            if (e_k != 45){
                if (e_k < 48 || e_k > 57) {
                    if (e_k != 46) {
                        event.returnValue = false;
                    }
                }
            }
            }
        function checkDecimal() {
            e_k = event.keyCode;
            if (e_k != 45) {
                if (e_k < 48 || e_k > 57) {
                    if (e_k != 46) {
                        event.returnValue = false;
                    }
                }
            }
        }
  
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
                                left: 35%; padding: 10px 10px 10px 10px; visibility: visible;
                                border-color: silver; border-style: solid; border-width: 1px; background-color: White;">
                                <img src="../img/indicator.gif" style="vertical-align: middle" alt="Processing" />
                                Loading ... &nbsp;
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <table cellpadding="5" cellspacing="5">
                        <tr>
                            <td style="font-weight: bold" width="150px" >
                                Premium</td>
                            <td width="200px">
                                <asp:TextBox ID="txtPremium" runat="server" MaxLength="5" 
                                    Style="text-align: right" Width="60px" Font-Size="X-Large"></asp:TextBox>
                            </td>
                            <td style="font-weight: bold" width="150px">
                                </td>
                            <td width="200px">
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold">
                                FX Bid</td>
                            <td>
                                <asp:TextBox ID="txtFxBid" runat="server" MaxLength="4" 
                                    Style="text-align: right" Width="60px" Font-Size="X-Large"></asp:TextBox>
                            </td>
                            <td style="font-weight: bold">
                                Melting Cost (Baht)</td>
                            <td>
                                <asp:TextBox ID="txtMeltingCost" runat="server" MaxLength="3" 
                                    Style="text-align: right" Width="60px" Font-Size="X-Large"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold">
                                FX Ask</td>
                            <td>
                                <asp:TextBox ID="txtFxAsk" runat="server" MaxLength="4" 
                                    Style="text-align: right" Width="60px" Font-Size="X-Large"></asp:TextBox>
                            </td>
                            <td style="font-weight: bold">
                                Max Baht</td>
                            <td>
                                <asp:TextBox ID="txtMaxBg" runat="server" MaxLength="4" 
                                    Style="text-align: right" Width="60px" Font-Size="X-Large"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold">
                                Space 99.99/KG</td>
                            <td>
                                <asp:TextBox ID="txtSpace99kg" runat="server" MaxLength="3" 
                                    Style="text-align: right" Width="60px" Font-Size="X-Large"></asp:TextBox>
                            </td>
                            <td style="font-weight: bold">
                                Max KG</td>
                            <td>
                                <asp:TextBox ID="txtMaxKg" runat="server" MaxLength="3" 
                                    Style="text-align: right" Width="60px" Font-Size="X-Large"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold">
                                Space 99.99/Baht</td>
                            <td>
                                <asp:TextBox ID="txtSpace99Bg" runat="server" MaxLength="3" 
                                    Style="text-align: right" Width="60px" Font-Size="X-Large"></asp:TextBox>
                            </td>
                            <td style="font-weight: bold">
                                Max Mini</td>
                            <td>
                                <asp:TextBox ID="txtMaxMn" runat="server" MaxLength="4" 
                                    Style="text-align: right" Width="60px" Font-Size="X-Large"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold">
                                Space 96.50/Baht</td>
                            <td>
                                <asp:TextBox ID="txtSpace96Bg" runat="server" MaxLength="3" 
                                    Style="text-align: right" Width="60px" Font-Size="X-Large"></asp:TextBox>
                            </td>
                            <td style="font-weight: bold">
                                %Range Leave Order</td>
                            <td>
                                <asp:TextBox ID="txtRangeLeave" runat="server" MaxLength="4" 
                                    Style="text-align: right" Width="60px" Font-Size="X-Large"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="font-weight: bold">
                                </td>
                            <td colspan="2" align="center">
                                
                                &nbsp;<asp:Button ID="btnReset" runat="server" CssClass="buttonPro small black" 
                                    Text="Reset" Width="60px" />
                                &nbsp;<asp:Button ID="btnSave" runat="server" CssClass="buttonPro small black" 
                                    Text="Save" Width="60px" /></td>
                            <td>
                                &nbsp;</td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top; font-weight: bold" colspan="2" align="center">
                               
                               
                               
                            </td>
                            <td align="center" style="vertical-align: top; font-weight: bold">
                                &nbsp;</td>
                            <td align="center" style="vertical-align: top; font-weight: bold">
                                &nbsp;</td>
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
