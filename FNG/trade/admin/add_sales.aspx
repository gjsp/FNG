<%@ Page Language="VB" MasterPageFile="~/trade/admin/MasterPageAdmin.master" AutoEventWireup="false" CodeFile="add_sales.aspx.vb" Inherits="admin_add_sales" title="Manage Sales" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
<link href="style.css" rel="stylesheet" type="text/css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
<div style="margin: auto; text-align: left; width: 500px;">
   <div style="text-align: center; font-weight: bold;font-size:18px;">
       Add New Sales
   </div>
   <div style="text-align: left; margin-top:25px;">
        <div style="float:left; width:150px; text-align:right;padding-top:3px;">Username * :</div>
       <div style="float:left; width:345px; text-align:left; margin-left:5px;">
           <asp:TextBox ID="UserTxtbox" runat="server" Width="170px" MaxLength="50"></asp:TextBox>
        </div>
       <div style="clear:both;"></div>
   </div>
   <div style="text-align: left; margin-top:15px;">
        <div style="float:left; width:150px; text-align:right;padding-top:3px;">Password * :</div>
       <div style="float:left; width:345px; text-align:left; margin-left:5px;">
           <asp:TextBox ID="PassTxtbox" runat="server" TextMode="Password" Width="170px" MaxLength="50"></asp:TextBox>
        </div>
       <div style="clear:both;"></div>
   </div>
   <div style="text-align: center; margin-top:25px;">
        <asp:Button ID="Addbt" runat="server" CssClass="buttonPro small grey" Text="Add" />
        &nbsp;<asp:Button ID="Cancelbt" runat="server" CssClass="buttonPro small grey" Text="Cancel" />
   </div>
</div>
</asp:Content>

