<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false"
    CodeFile="stock_config.aspx.vb" Inherits="stock_config" Title="Stock Config" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea">
        <div class="demoheading">
            Setting Price</div>
        <div>
            <asp:UpdatePanel ID="udpMain" runat="server">
                <ContentTemplate>
                    <div>
                        <table border="0">
                            <tr>
                                <td>
                                    Bid 96.50% :</td>
                                <td>
                                    <asp:TextBox ID="txtBid96" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Ask 96.50%</td>
                                <td>
                                    <asp:TextBox ID="txtAsk96" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Bid 99.99%</td>
                                <td>
                                    <asp:TextBox ID="txtBid99" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Ask 99.99%</td>
                                <td>
                                    <asp:TextBox ID="txtAsk99" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    Margin Level:
                                </td>
                                <td>
                                    <asp:TextBox ID="txtMargin" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            
                            <tr>
                                <td>
                                    Password Aut :</td>
                                <td>
                                    <asp:TextBox ID="txtPwdAuth" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            
                            <tr>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" Text="Update" />
                                </td>
                            </tr>
                            
                        </table>
                    </div>
                    <div align="center">
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
</asp:Content>
