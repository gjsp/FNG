<%@ Page Language="VB" MasterPageFile="~/trade/tradeMasterPage.master" AutoEventWireup="false"
    CodeFile="cust_profile.aspx.vb" Inherits="cust_profile" Title="Profile" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    
    <style type="text/css">
        .style1
        {
            font-family: Tahoma, Arial, Verdana, Helvetica, sans-serif;
            font-size: 13px;
            color: #000000;
            padding-right: 10px;
            padding-top: 8px;
            padding-bottom: 8px;
            text-align: right;
            border-bottom: #d6e0e9 1px solid;
            height: 51px;
        }
        .style2
        {
            font-family: Tahoma, Arial, Verdana, Helvetica, sans-serif;
            font-size: 13px;
            color: #000000;
            padding-left: 10px;
            padding-top: 8px;
            padding-bottom: 8px;
            text-align: left;
            border-bottom: #d6e0e9 1px solid;
            border-right: #FFFFFF 4px solid;
            height: 51px;
        }
    </style>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="demoarea">
        <asp:HiddenField ID="hdfCust_id" runat="server" />
        <asp:UpdatePanel ID="udpMain" runat="server">
            <ContentTemplate>
                <table cellpadding="0" cellspacing="0" border="0" style="width: 1300px">
                    <tr>
                        <td valign="top" style="width: 600px">
                            <fieldset class="k-fieldset">
                                <legend class="topic">Customer Detail</legend>
                                <table border="0" cellpadding="0" style="width: 100%; padding: 10px 0px 15px 0px;">
                                    <tr>
                                        <td valign="top" class="ft_1b" style="width:100px">
                                            Ref No :
                                        </td>
                                        <td valign="top" class="ft_3b" style="width:200px">
                                            &nbsp;<asp:Label ID="lblRefNo" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td valign="top" class="ft_1b" style="width:150px">
                                            บุคคลติดต่อ :
                                        </td>
                                        <td valign="top" class="ft_3b" style="width:150px">
                                            &nbsp;<asp:Label ID="lblPerson" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" class="ft_1b">
                                            Name :
                                        </td>
                                        <td valign="top" class="ft_3b">
                                            &nbsp;<asp:Label ID="lblName" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td valign="top" class="ft_1b">
                                            เงินฝาก :</td>
                                        <td valign="top" rowspan="1" class="ft_3b">
                                            <asp:Label ID="lblCashDep" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ft_1b">
                                            Address :
                                        </td>
                                        <td class="ft_3b">
                                            &nbsp;<asp:Label ID="lblAdd" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td class="ft_1b">
                                            ทองฝาก 96.50% : </td>
                                        <td class="ft_3b">
                                            &nbsp;<asp:Label ID="lblGoldDep96" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" class="ft_1b">
                                            Phone :
                                        </td>
                                        <td class="ft_3b">
                                            &nbsp;<asp:Label ID="lblPhone" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td class="ft_1b">
                                            ทองฝาก 96.50% : </td>
                                         <td class="ft_3b">
                                             <asp:Label ID="lblGoldDep96G" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ft_1b">
                                            Fax :
                                        </td>
                                        <td class="ft_3b">
                                            &nbsp;<asp:Label ID="lblFax" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td class="ft_1b">
                                            ทองฝาก 96.50% : </td>
                                        <td class="ft_3b">
                                            <asp:Label ID="lblGoldDep96M" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ft_1b">
                                            Mobile Phone :
                                        </td>
                                        <td class="ft_3b">
                                            &nbsp;<asp:Label ID="lblMobilePhone" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td class="ft_1b">
                                            ทองฝาก 99.99% :</td>
                                        <td class="ft_3b">
                                            &nbsp;<asp:Label ID="lblGoldDep99" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ft_1b">
                                            Account Bank :
                                        </td>
                                        <td class="ft_3b">
                                            &nbsp;<asp:Label ID="lblAccBank" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td class="ft_1b">
                                            Branch :
                                        </td>
                                        <td class="ft_3b">
                                            &nbsp;<asp:Label ID="lblBranch" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ft_1b">
                                            Account No :
                                        </td>
                                        <td class="ft_3b">
                                            &nbsp;<asp:Label ID="lblAccNo" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td class="ft_1b">
                                            Account Name :
                                        </td>
                                        <td class="ft_3b">
                                            &nbsp;<asp:Label ID="lblAccName" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="ft_1b">
                                            &nbsp;</td>
                                        <td class="ft_3b">
                                            &nbsp;</td>
                                        <td class="ft_1b">
                                            &nbsp;</td>
                                        <td class="ft_3b">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="ft_1b">
                                            &nbsp;</td>
                                        <td class="ft_3b">
                                            &nbsp;</td>
                                        <td class="ft_1b">
                                            &nbsp;</td>
                                        <td class="ft_3b">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="ft_1b">
                                            &nbsp;</td>
                                        <td class="ft_3b">
                                            &nbsp;</td>
                                        <td class="ft_1b">
                                            &nbsp;</td>
                                        <td class="ft_3b">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="ft_1b">
                                            &nbsp;</td>
                                        <td class="ft_3b">
                                            &nbsp;</td>
                                        <td class="ft_1b">
                                            &nbsp;</td>
                                        <td class="ft_3b">
                                            &nbsp;</td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                        <td style="vertical-align:top">
                             <fieldset class="k-fieldset">
                                <legend class="topic">หลักประกัน</legend>
                                <table border="0" cellpadding="0" style="width: 95%;padding:   10px 0px 15px 0px;">
                                    <tr>
                                        <td style="width:150px;text-align:right" class="ft_1b">
                                            Cash :
                                        </td>
                                        <td style="width:120px; text-align:right" class="ft_3b">
                                            <asp:Label ID="lblGrtCash" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;บาท</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right" class="ft_1b">
                                            Gold 96.50 :
                                        </td>
                                        <td style="text-align:right" class="ft_3b">
                                            <asp:Label ID="lblGrt96" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;บาท</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right" class="ft_1b">
                                            Gold99.99 :</td>
                                        <td style="text-align:right" class="ft_3b">
                                            <asp:Label ID="lblGrt99" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;กิโล</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right" class="ft_1b">
                                            เทรดทอง 96.50 ได้ :</td>
                                        <td style="text-align:right" class="ft_3b">
                                            <asp:Label ID="lblTradelimit96" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;บาท</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right" class="ft_1b">
                                            เทรดทอง 99.99 ได้ :</td>
                                        <td style="text-align:right" class="ft_3b">
                                            <asp:Label ID="lblTradelimit99" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;กิโล
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right" class="ft_1b">
                                            &nbsp;</td>
                                        <td style="text-align:right" class="ft_3b">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right" class="ft_1b">
                                            &nbsp;</td>
                                        <td style="text-align:right" class="ft_3b">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="ft_1b" style="text-align:right">
                                            &nbsp;</td>
                                        <td class="ft_3b" style="text-align:right">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="ft_1b" style="text-align:right">
                                            &nbsp;</td>
                                        <td class="ft_3b" style="text-align:right">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="ft_1b" style="text-align:right">
                                            &nbsp;</td>
                                        <td class="ft_3b" style="text-align:right">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="ft_1b" style="text-align:right">
                                            &nbsp;</td>
                                        <td class="ft_3b" style="text-align:right">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="ft_1b" style="text-align:right">
                                            &nbsp;</td>
                                        <td class="ft_3b" style="text-align:right">
                                            &nbsp;</td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                        <td style="vertical-align:top">
                             <fieldset class="k-fieldset">
                                <legend class="topic">ยอดรวมการซื้อขาย</legend>
                                <table border="0" cellpadding="0" style="width: 95%;padding: 10px 0px 15px 0px;">
                                    <tr>
                                        <td style="width:150px;text-align:right" class="ft_1b">
                                            Quantity Buy 96:
                                        </td>
                                        <td style="width:100px; text-align:right" class="ft_3b">
                                            <asp:Label ID="lblSQ96" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;บาท</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right" class="ft_1b">
                                            Amount Buy 96:
                                        </td>
                                        <td style="text-align:right" class="ft_3b">
                                            <asp:Label ID="lblSA96" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;บาท</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right" class="ft_1b">
                                            Quantity Sell 96 :</td>
                                        <td style="text-align:right" class="ft_3b">
                                            <asp:Label ID="lblBQ96" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;บาท</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right" class="ft_1b">
                                            Amount Sell 96 :</td>
                                        <td style="text-align:right" class="ft_3b">
                                            <asp:Label ID="lblBA96" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;บาท</td>
                                    </tr>
                                    <tr>
                                        <td class="ft_1b" style="text-align:right">
                                            Sum Quantity 96 :</td>
                                        <td class="ft_3b" style="text-align:right">
                                            <asp:Label ID="lblSumQ96" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;บาท</td>
                                    </tr>
                                    <tr>
                                        <td class="ft_1b" style="text-align:right">
                                            Sum Amount 96 :</td>
                                        <td class="ft_3b" style="text-align:right">
                                            <asp:Label ID="lblSumA96" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;บาท</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right" class="ft_1b">
                                            Quantity Buy 99 :</td>
                                        <td style="text-align:right" class="ft_3b">
                                            <asp:Label ID="lblSQ99" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;กิโล
                                        </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right" class="ft_1b">
                                            Amount Buy 99 : </td>
                                        <td style="text-align:right" class="ft_3b">
                                            <asp:Label ID="lblSA99" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;บาท</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right" class="ft_1b">
                                            Quantity Sell 99 :</td>
                                        <td style="text-align:right" class="ft_3b">
                                            <asp:Label ID="lblBQ99" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;กิโล</td>
                                    </tr>
                                    <tr>
                                        <td class="ft_1b" style="text-align:right">
                                            Amount Sell 99 :</td>
                                        <td class="ft_3b" style="text-align:right">
                                            <asp:Label ID="lblBA99" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;บาท</td>
                                    </tr>
                                    <tr>
                                        <td class="ft_1b" style="text-align:right">
                                            Sum Quantity 99 :</td>
                                        <td class="ft_3b" style="text-align:right">
                                            <asp:Label ID="lblSumQ99" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;กิโล</td>
                                    </tr>
                                    <tr>
                                        <td class="ft_1b" style="text-align:right">
                                            Sum Amount 99 :</td>
                                        <td class="ft_3b" style="text-align:right">
                                            <asp:Label ID="lblSumA99" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;บาท</td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <asp:LinkButton ID="linkUser" CssClass="link3" runat="server">Change Username</asp:LinkButton>
        <span>&nbsp;|&nbsp;</span>
        <asp:LinkButton ID="linkPwd" CssClass="link3" runat="server">Change Password</asp:LinkButton>
    </div>
</asp:Content>
