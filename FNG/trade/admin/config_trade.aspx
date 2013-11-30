<%@ Page Language="VB" MasterPageFile="~/trade/admin/MasterPageAdmin.master" AutoEventWireup="false"
    CodeFile="config_trade.aspx.vb" Inherits="trade_admin_config_trade" Title="Config" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../style.css" rel="stylesheet" type="text/css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="adminarea">
        <asp:UpdateProgress ID="UpdateProg1" runat="server">
            <ProgressTemplate>
                <div id="Div1" align="center" valign="middle" runat="server" style="position: absolute;
                    left: 46%; padding: 10px 10px 10px 10px; visibility: visible; border-color: silver;
                    border-style: solid; border-width: 1px; background-color: White;">
                    <img src="../img/indicator.gif" style="vertical-align: middle" alt="Processing" />
                    Loading ... &nbsp;
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <asp:UpdatePanel ID="up" runat="server">
            <ContentTemplate>
                <table cellpadding="2" cellspacing="2" border="0">
                    <tr>
                        <td style="width: 150px; text-align: right">
                            Reject Leave Order All :&nbsp;
                        </td>
                        <td style="width: 500px; text-align: left">
                            <asp:Button ID="btnRejectAll" runat="server" CssClass="buttonPro small grey" Text="Reject Leave Order" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 150px; text-align: right">
                            System Halt :
                        </td>
                        <td style="width: 500px; text-align: left">
                            <asp:Button ID="btnHalt" runat="server" CssClass="buttonPro small grey" Text="System Halt" />
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 150px; text-align: right">
                            &nbsp;
                        </td>
                        <td style="width: 500px; text-align: left">
                            &nbsp;
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 150px; text-align: right">
                            Gold Limit System :
                        </td>
                        <td style="width: 500px; text-align: left">
                            <asp:TextBox ID="txtLimitQuantity" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 150px; text-align: right; vertical-align: top">
                            Message :
                        </td>
                        <td style="width: 500px; text-align: left">
                            <asp:TextBox ID="txtMsg" runat="server" Height="70px" TextMode="MultiLine" Width="600px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="width: 150px; text-align: right">
                            &nbsp;
                        </td>
                        <td style="width: 500px; text-align: left">
                            <asp:Button ID="btnSave" runat="server" CssClass="buttonPro small grey" Text="Save" />
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
