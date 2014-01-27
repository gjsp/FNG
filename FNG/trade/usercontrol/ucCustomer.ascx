<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ucCustomer.ascx.vb" Inherits="user_control_ucCustomer" %>
<link href="../style.css" rel="stylesheet" />

<style type="text/css">
    .style4 {
        width: 120px;
        text-align: right;
    }

    .lbl {
        color: Black;
    }

    .style6 {
        width: 280px;
    }
</style>
<asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:MultiView ID="mvMain" runat="server">
            <asp:View ID="vMain" runat="server">

                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%; background-color: #EEEEEE">
                    <tr>
                        <td valign="top" style="width: 60%">
                            <fieldset>
                                <legend class="topic">Customer Detail</legend>
                                <table border="0" cellpadding="1" cellspacing="1" style="width: 100%">
                                    <tr>
                                        <td valign="top" class="style4">Ref No :
                                        </td>
                                        <td valign="top" class="style6">
                                            <asp:Label ID="lblCustRef" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td valign="top" style="width: 100px; text-align: right">Account Bank :
                                        </td>
                                        <td valign="top">
                                            <asp:Label ID="lblAccBank" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td valign="top">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="style4" valign="top">Name :</td>
                                        <td class="style6" valign="top">
                                            <asp:Label ID="lblName" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td style="width: 100px; text-align: right" valign="top">Account No :</td>
                                        <td valign="top">
                                            <asp:Label ID="lblAccNo" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td valign="top">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="style4" valign="top">Address :</td>
                                        <td class="style6" valign="top">
                                            <asp:Label ID="lblAdd" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td style="width: 100px; text-align: right" valign="top">Branch :</td>
                                        <td valign="top">
                                            <asp:Label ID="lblBranch" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td valign="top">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td valign="top" class="style4">&nbsp;Mobile Phone :</td>
                                        <td class="style6" valign="top">
                                            <asp:Label ID="lblMobilePhone" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td valign="top" style="text-align: right; width: 100px;">Account Name :</td>
                                        <td valign="top">
                                            <asp:Label ID="lblAccName" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td valign="top">&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td valign="top" class="style4">&nbsp;</td>
                                        <td class="style6">&nbsp;</td>
                                        <td valign="top" style="text-align: right">&nbsp;</td>
                                        <td valign="top" class="style6">&nbsp;</td>
                                        <td valign="top">&nbsp;</td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                        <td valign="top" style="width: 18%">
                            <fieldset>
                                <legend class="topic">Customer Asset</legend>
                                <table border="0" cellpadding="1" cellspacing="1" style="width: 220px">
                                    <tr>
                                        <td style="width: 120px; text-align: right">Cash :
                                        </td>
                                        <td style="width: 100px; text-align: right">
                                            <asp:Label ID="lblCash" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;บาท</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">Gold 96.50(บาท) :
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblGold96" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;บาท</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">Gold96.50 (กรัม) :</td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblGold96g" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;กรัม</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">Gold96.50 (มินิ) :</td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblGold96m" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;บาท</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">Gold 99.99 :
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblGold99" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;กิโล</td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                        <td valign="top" style="width: 22%">
                            <fieldset>
                                <legend class="topic">หลักประกัน</legend>
                                <table border="0" cellpadding="1" cellspacing="1" style="width: 270px">
                                    <tr>
                                        <td style="width: 150px; text-align: right">Cash :
                                        </td>
                                        <td style="width: 120px; text-align: right">
                                            <asp:Label ID="lblGrtCash" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;บาท</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">Gold 96.50 :
                                        </td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblGrt96" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;บาท</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">Gold99.99 :</td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblGrt99" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;กิโล</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">เทรดทอง 96.50 ได้ :</td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblTradelimit96" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;บาท</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align: right">เทรดทอง 99.99 ได้ :</td>
                                        <td style="text-align: right">
                                            <asp:Label ID="lblTradelimit99" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;กิโล
                                        </td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>
            </asp:View>
            <asp:View ID="vNone" runat="server">
                <div style='color: red; text-align: center; border: solid 1px silver;'>Data not Found.</div>
            </asp:View>
        </asp:MultiView>
    </ContentTemplate>
</asp:UpdatePanel>
