<%@ Page Title="" Language="VB" MasterPageFile="~/trade/tradeMasterPageBlank.master"
    AutoEventWireup="false" CodeFile="cust_forget.aspx.vb" Inherits="trade_cust_forget" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="demoarea" style="text-align: center;min-height:500px">
        <fieldset class="k-fieldset">
            <legend class="topic">Forget Password</legend>
            <asp:UpdatePanel ID="upPwd" runat="server">
                <ContentTemplate>
                    <div style="text-align: left; margin-top: 15px;">
                        <div style="float: left; width: 150px; text-align: right; padding-top: 3px;">
                            User Name :</div>
                        <div style="float: left; width: 145px; text-align: left; margin-left: 5px;">
                            <asp:TextBox ID="txtUsername" runat="server" Width="145px" MaxLength="50"></asp:TextBox></div>
                        <div style="clear: both;">
                        </div>
                    </div>
                  
                    <div style="text-align: left; margin-top: 15px;">
                        <div style="float: left; width: 150px; text-align: right; padding-top: 3px;">
                            เลขที่บัตรประจำตัวประชาชน/<br />เลขจดทะเบียน(นิติบุคคล) :</div>
                        <div style="float: left; width: 350px; text-align: left; margin-left: 5px;">
                                <asp:TextBox ID="txtIDCardz1" runat="server" Width="10px" MaxLength="1" />
                                <asp:TextBox ID="txtIDCardz2" runat="server" Width="10px" MaxLength="1" />
                                <asp:TextBox ID="txtIDCardz3" runat="server" Width="10px" MaxLength="1" />
                                <asp:TextBox ID="txtIDCardz4" runat="server" Width="10px" MaxLength="1" />
                                <asp:TextBox ID="txtIDCardz5" runat="server" Width="10px" MaxLength="1" />
                                <asp:TextBox ID="txtIDCardz6" runat="server" Width="10px" MaxLength="1" />
                                <asp:TextBox ID="txtIDCardz7" runat="server" Width="10px" MaxLength="1" />
                                <asp:TextBox ID="txtIDCardz8" runat="server" Width="10px" MaxLength="1" />
                                <asp:TextBox ID="txtIDCardz9" runat="server" Width="10px" MaxLength="1" />
                                <asp:TextBox ID="txtIDCardz10" runat="server" Width="10px" MaxLength="1" />
                                <asp:TextBox ID="txtIDCardz11" runat="server" Width="10px" MaxLength="1" />
                                <asp:TextBox ID="txtIDCardz12" runat="server" Width="10px" MaxLength="1" />
                                <asp:TextBox ID="txtIDCardz13" runat="server" Width="10px" MaxLength="1" />
                            </div>
                        <div style="clear: both;">
                        </div>
                    </div>
                    <div style="text-align: left; margin-top: 15px;">
                        <div style="float: left; width: 150px; text-align: right; padding-top: 3px;">
                            วันเดือนปีเกิด/<br />วันที่จดทะเบียน(นิติบุคคล) :</div>
                        <div style="float: left; width: 350px; text-align: left; margin-left: 5px;">
                            <asp:DropDownList ID="ddlday" runat="server">
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlMonth" runat="server">
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlYear" runat="server">
                            </asp:DropDownList>
                            (วัน:เดือน:ปี)</div>
                        <div style="clear: both;">
                        </div>
                    </div>
                    <div style="text-align: left; margin-top: 15px;">
                        <div style="float: left; width: 150px; text-align: right; padding-top: 3px;">
                           เบอร์โทรศัพท์มือถือ :</div>
                        <div style="float: left; width: 350px; text-align: left; margin-left: 5px;">
                          
                                <asp:TextBox ID="txtMobile" runat="server" Width="200px" />
                            </div>
                        <div style="clear: both;">
                        </div>
                    </div>
                    <div style="text-align: center; margin-top: 25px; width: 400px;">
                        <asp:Button ID="btnSend" runat="server" CssClass="buttonPro small black" Text="Send" Width="70" />
                        &nbsp;<asp:Button ID="btnBack" runat="server" CssClass="buttonPro small black" Text="Back" OnClientClick="location.href='login.aspx';" Width="70"/>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
    </div>
</asp:Content>
