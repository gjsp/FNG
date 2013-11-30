<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ucPortFolio.ascx.vb" Inherits="user_control_ucPortFolio" %>
<link href="../main.css" rel="stylesheet" type="text/css" />

 <style type="text/css">
        .style4
        {
            width: 120px;
            text-align: right;
        }
        .lbl
        {
            color: Black;
        }
        .style6
        {
            width: 280px;
        }
        </style>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%">
                    <tr>
                        <td valign="top" style="width: 60%">
                            <fieldset>
                                <legend class="topic">Customer Detail</legend>
                                <table border="0" cellpadding="1" cellspacing="1" style="width: 100%">
                                    <tr>
                                        <td valign="top" class="style4">
                                            Ref No :
                                        </td>
                                        <td valign="top" class="style6">
                                            <asp:Label ID="lblCustRef" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td valign="top" style="width:100px;text-align:right">
                                            บุคคลติดต่อ :
                                        </td>
                                        <td valign="top">
                                            <asp:Label ID="lblPerson" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td valign="top">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td valign="top" class="style4">
                                            Name :
                                        </td>
                                        <td class="style6">
                                            <asp:Label ID="lblName" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td valign="top" style="text-align:right">
                                            Remark :
                                        </td>
                                        <td valign="top" class="style6" rowspan="3">
                                            <asp:Label ID="lblRemark" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td rowspan="3" valign="top">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td valign="top" class="style4">
                                            Address :
                                        </td>
                                        <td class="style6">
                                            <asp:Label ID="lblAdd" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td valign="top" class="style4">
                                            Phone :
                                        </td>
                                        <td class="style6">
                                            <asp:Label ID="lblPhone" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" class="style4">
                                            Fax :
                                        </td>
                                        <td class="style6">
                                            <asp:Label ID="lblFax" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td>
                                            <span style="display: none">
                                                <asp:Button ID="btnSearch" runat="server" Text="Search" /></span>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td valign="top" class="style4">
                                            Mobile Phone :
                                        </td>
                                        <td class="style6">
                                            <asp:Label ID="lblMobilePhone" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td style="text-align:right">
                                            Trade Type :</td>
                                        <td>
                                            <asp:Label ID="lblTrade_type" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="style4">
                                            Account Bank :
                                        </td>
                                        <td class="style6">
                                            <asp:Label ID="lblAccBank" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td style="text-align:right">
                                            Branch :
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBranch" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="style4">
                                            Account No :
                                        </td>
                                        <td class="style6">
                                            <asp:Label ID="lblAccNo" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td style="text-align:right">
                                            Account Name :
                                        </td>
                                        <td class="style2">
                                            <asp:Label ID="lblAccName" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                        <td valign="top" style="width: 18%">
                            <fieldset>
                                <legend class="topic">Customer Asset</legend>
                                <table border="0" cellpadding="1" cellspacing="1" style="width: 220px">
                                    <tr>
                                        <td style="width:120px;text-align:right">
                                            Cash :
                                        </td>
                                        <td style="width:100px; text-align:right">
                                            <asp:Label ID="lblCash" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;บาท</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right">
                                            Gold 96.50(บาท) :
                                        </td>
                                        <td style="text-align:right">
                                            <asp:Label ID="lblGold96" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;บาท</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right">
                                            Gold96.50 (กรัม) :</td>
                                        <td style="text-align:right">
                                            <asp:Label ID="lblGold96g" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;กรัม</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right">
                                            Gold96.50 (มินิ) :</td>
                                        <td style="text-align:right">
                                            <asp:Label ID="lblGold96m" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;บาท</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right">
                                            Gold 99.99 :
                                        </td>
                                        <td style="text-align:right">
                                            <asp:Label ID="lblGold99" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;กิโล</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right">
                                            &nbsp;</td>
                                        <td style="text-align:right">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right">
                                            &nbsp;</td>
                                        <td style="text-align:right">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right">
                                            &nbsp;</td>
                                        <td style="text-align:right">
                                            &nbsp;</td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                        <td valign="top" style="width: 22%">
                            <fieldset>
                                <legend class="topic">หลักประกัน</legend>
                                <table border="0" cellpadding="1" cellspacing="1" style="width: 270px">
                                    <tr>
                                        <td style="width:150px;text-align:right">
                                            Cash :
                                        </td>
                                        <td style="width:120px; text-align:right">
                                            <asp:Label ID="lblGrtCash" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;บาท</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right">
                                            Gold 96.50 :
                                        </td>
                                        <td style="text-align:right">
                                            <asp:Label ID="lblGrt96" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;บาท</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right">
                                            Gold99.99 :</td>
                                        <td style="text-align:right">
                                            <asp:Label ID="lblGrt99" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;กิโล</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right">
                                            เทรดทอง 96.50 ได้ :</td>
                                        <td style="text-align:right">
                                            <asp:Label ID="lblTradelimit96" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;บาท</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right">
                                            เทรดทอง 99.99 ได้ :</td>
                                        <td style="text-align:right">
                                            <asp:Label ID="lblTradelimit99" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;กิโล
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right">
                                            &nbsp;</td>
                                        <td style="text-align:right">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right">
                                            &nbsp;</td>
                                        <td style="text-align:right">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right">
                                            &nbsp;</td>
                                        <td style="text-align:right">
                                            &nbsp;</td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>