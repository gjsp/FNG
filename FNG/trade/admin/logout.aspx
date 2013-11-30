<%@ Page Title="" Language="VB" MasterPageFile="~/trade/admin/MasterPageBlank.master" AutoEventWireup="false" CodeFile="logout.aspx.vb" Inherits="trade_admin_logout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">

<div style="height:500px;font-size:medium;font-weight:bold;color:Black;text-align:center;vertical-align:middle">
<br /><br /><br />
    คุณได้ออกจากระบบเรียบร้อยแล้ว<br />
    แนะนำให้ปิดบราวเซอร์ทุกครั้งเมื่อเสร็จสิ้นการใช้งาน<p></p>
 <input id="btnClose" type="button" value="Close" onclick="window.close();" />&nbsp;
    <input id="btnHome" type="button" value="Login Page" onclick="location.href='../login.aspx'" />
</div>
   
  
</asp:Content>

