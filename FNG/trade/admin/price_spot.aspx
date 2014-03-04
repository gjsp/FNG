<%@ Page Language="VB" AutoEventWireup="false" CodeFile="price_spot.aspx.vb" Inherits="trade_admin_price_spot" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="style.css" rel="stylesheet" type="text/css" />
    <script src="../../js/functionst.js" type="text/javascript"></script>
    <link href="../../button/css/buttonPro.css" rel="stylesheet" type="text/css" />
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        var _source;
        var _popup;

        function showConfirm(source) {
            try {
                var premium = $get('<%=txtPremium.ClientID%>').value;
                var fxbid = $get('<%=txtFxBid.ClientID%>').value;
                var fxask = $get('<%=txtFxAsk.ClientID%>').value;
                var space99kg = $get('<%=txtSpace99kg.ClientID%>').value;
                var space99bg = $get('<%=txtSpace99Bg.ClientID%>').value;
                var space96bg = $get('<%=txtSpace96Bg.ClientID%>').value;
                var melting = $get('<%=txtMeltingCost.ClientID%>').value;
                var cbSelf = $get('<%=cbFxSelf.ClientID%>').checked;
                var fxSelf = '0';

                if (cbSelf) {
                    fxSelf = $get('<%=txtFxSelf.ClientID%>').value;
                }

                $.ajax({
                    type: "POST",
                    url: "price_spot.aspx/getSpotPriceConfirm",
                    data: '{"premium":' + premium + ',"fxbid":' + fxbid + ',"fxask":' + fxask + ',"space99kg":' + space99kg + ',"space99bg":' + space99bg + ',"space96bg":' + space96bg + ',"melting":' + melting + ',"fxSelf":' + fxSelf + '}',
                    contentType: "application/json; charset=utf-8",
                    dataType: "json",
                    success: function (result) {
                        $get('pContent').innerHTML = result.d;
                    },
                    error: function () {
                        alert('Error Code JSON 103');
                    }
                });
                                
                this._source = source;
                this._popup = $find('mppAccept');
                this._popup.show();
                $get('<%=btnOk.ClientId %>').focus();               
            } catch (e) {
                alert(e.message);
            }
        }
        function okClick() {
            try {
                this._popup.hide();
                __doPostBack(this._source.name, '');
                
            } catch (e) {
                alert('Error Code 102.');
            }
        }

        function cancelClick() {
            this._popup.hide();
            this._source = null;
            this._popup = null;
        }


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
  
       
        function OnSucceeded(result, userContext, methodName) {
            
            $get('pContent').innerHTML = 'Summary Spot';
            this._source = source;
            this._popup = $find('mppAccept');
            this._popup.show();

        }
        function OnFailed(error, userContext, methodName) { }

        //function x1 {
        //$(document).ready(function () {
        //    $.ajax({
        //        type: "POST",
        //        url: "WebForm1.aspx/ServerSideMethod",
        //        data: "{}",
        //        contentType: "application/json; charset=utf-8",
        //        dataType: "json",
        //        async: true,
        //        cache: false,
        //        success: function (msg) {
        //            $('#myDiv').text(msg.d); 
        //        }
        //    })
        //    return false;
        //});
        //}
        
    </script>
    <style type="text/css">
        /*Modal Popup*/
        .modalAdminBG
        {
            background-color: Black;
            filter: alpha(opacity=70);
            opacity: 0.7;
        }
        .modalAdminPopup {
            vertical-align:top;
            background-color: white;
            border-style: none;
            width: 700px;
            height: 350px;
        }
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
                    <table cellpadding="5" cellspacing="5">
                        <tr>
                            <td style="font-weight: bold" width="150px" >
                                Premium</td>
                            <td width="200px">
                                <asp:TextBox ID="txtPremium" runat="server" MaxLength="5" 
                                    Style="text-align: right" Width="60px" Font-Size="X-Large"></asp:TextBox>
                            </td>
                            <td style="font-weight: bold" width="150px">
                                Self Fx Ask<asp:CheckBox ID="cbFxSelf" runat="server" />
                            </td>
                            <td width="200px">
                                <asp:TextBox ID="txtFxSelf" runat="server" Font-Size="X-Large" MaxLength="5" Style="text-align: right" Width="60px"></asp:TextBox>
                            </td>
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
                                <asp:TextBox ID="txtMeltingCost" runat="server" MaxLength="5" 
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
                                &nbsp;<asp:Button ID="btnSave" runat="server" OnClientClick="showConfirm(this); return false;" CssClass="buttonPro small black" 
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
        <ajaxToolkit:ModalPopupExtender ID="mppAccept" BehaviorID="mppAccept" runat="server"
            TargetControlID="Panel1" PopupControlID="Panel1" OkControlID="btnOk" OnOkScript="okClick();"
            CancelControlID="btnNo" OnCancelScript="cancelClick();" BackgroundCssClass="modalAdminBG">
        </ajaxToolkit:ModalPopupExtender>
        <asp:Panel ID="Panel1" runat="server" Style="display: block" CssClass="modalAdminPopup" DefaultButton="btnOk">
           <asp:Panel ID="Panel2" runat="server" Style="border:none;color: black">
                <div style="text-align:center">
                    <p id="pContent" style="text-align:center; margin: 50px 20px 20px 20px; font-size:large;">
                    </p>
                </div>
            </asp:Panel>
            <div>
                <p style="text-align: center;margin-top:2cm">
                    <asp:Button ID="btnOk" CssClass="buttonPro small black" runat="server" Text="Yes"
                        Width="90px" />&nbsp;&nbsp;
                    <asp:Button ID="btnNo" CssClass="buttonPro small black" runat="server" Text="No"
                        Width="90px" />
                </p>
            </div>
        </asp:Panel>

    </div>
    </form>
</body>
</html>
