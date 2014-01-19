<%@ Page Language="VB" MasterPageFile="~/trade/admin/MasterPageAdmin.master" AutoEventWireup="false"
    CodeFile="change_password.aspx.vb" Inherits="admin_change_password" Title="Change Password" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="margin: auto; text-align: left; width: 400px;">
        <div style="text-align: center; font-weight: bold; font-size: 18px;">
            Change Password
        </div>
        <asp:UpdatePanel ID="upPwd" runat="server">
            <Triggers>
                <asp:PostBackTrigger ControlID="Savebt" />
            </Triggers>
            <ContentTemplate>
             <div style="text-align: left; margin-top: 25px;">
            <div style="float: left; width: 150px; text-align: right;">
                Old Password :</div>
            <div style="float: left; width: 145px; text-align: left; margin-left: 5px;">
                <asp:Label ID="OldpassTxt" runat="server"></asp:Label></div>
            <div style="clear: both;">
            </div>
        </div>
        <div style="text-align: left; margin-top: 15px;">
            <div style="float: left; width: 150px; text-align: right; padding-top: 3px;">
                New Password :</div>
            <div style="float: left; width: 145px; text-align: left; margin-left: 5px;">
                <asp:TextBox ID="txtNewPwd" runat="server" Width="145px" MaxLength="50" TextMode="Password"></asp:TextBox></div>
            <div style="clear: both;">
            </div>
        </div>
        <div style="text-align: left; margin-top: 15px;">
            <div style="float: left; width: 150px; text-align: right; padding-top: 3px;">
                Confirm New Password :</div>
            <div style="float: left; width: 145px; text-align: left; margin-left: 5px;">
                <asp:TextBox ID="txtConNewPwd" runat="server" Width="145px" MaxLength="50" TextMode="Password"></asp:TextBox></div>
            <div style="clear: both;">
            </div>
        </div>
            </ContentTemplate>
        </asp:UpdatePanel>
       <asp:HiddenField ID="hdfType" runat="server" />
        <div style="text-align: center; margin-top: 25px;">
            <asp:Button ID="Savebt" runat="server" CssClass="buttonPro small grey" Text="Save" />
            &nbsp;<asp:Button ID="Cancelbt" runat="server" CssClass="buttonPro small grey" Text="Cancel" />
        </div>
    </div>
</asp:Content>
