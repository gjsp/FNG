<%@ Page Language="VB" MasterPageFile="~/trade/admin/MasterPageAdmin.master" AutoEventWireup="false" CodeFile="manage_user_detail.aspx.vb" Inherits="admin_manage_user_detail" title="manage users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div style="margin: auto; text-align: left; width: 397px;">
   <div style="text-align: center; font-weight: bold;font-size:18px;margin-bottom:10px">
       Change Level
       </div>
 
   
   <table cellpadding="3" cellspacing = "3" border="0">
    <tr>
        <td style="width:150px;text-align:right">Level :</td>
        <td style="width:150px;text-align:left">  
        <asp:DropDownList ID="ddlLv" runat="server">
               <asp:ListItem Value="1">Level 1</asp:ListItem>
               <asp:ListItem Value="2">Level 2</asp:ListItem>
               <asp:ListItem Value="3">Level 3</asp:ListItem>
           </asp:DropDownList>
           </td>
    </tr>
    <tr>
        <td style="width:150px;text-align:right">Halt :</td>
        <td style="width:150px;text-align:left">  
            <asp:CheckBox ID="cbHalt" runat="server" />
           </td>
    </tr>
    <tr>
        <td style="width:150px;text-align:right">Login Lock :</td>
        <td style="width:150px;text-align:left">  
            <asp:Button ID="btnLock" runat="server" CssClass="buttonPro small grey" 
                Text="" />
           </td>
    </tr>
    </table>
   <div style="text-align: center; margin-top:25px;">
        <asp:Button ID="Savebt" runat="server" CssClass="buttonPro small grey" Text="Save" />
        &nbsp;<asp:Button ID="Cancelbt" runat="server" CssClass="buttonPro small grey" Text="Cancel" />
   &nbsp;</div>
</div>
</asp:Content>

