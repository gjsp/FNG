<%@ Page Title="" Language="VB" MasterPageFile="~/trade/tradeMasterPageBlank.master"
    AutoEventWireup="false" CodeFile="logout.aspx.vb" Inherits="trade_logout" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Panel ID="pnMain" runat="server">
        <div style="height: 500px; background-color:#fff; font-size: medium; font-weight: bold;
            color: black; text-align: center; vertical-align: middle">
            <br />
            <br />
            <br />
            คุณได้ออกจากระบบเรียบร้อยแล้ว<br />
            แนะนำให้ปิดบราวเซอร์ทุกครั้งเมื่อเสร็จสิ้นการใช้งาน<p>
            </p>
            <input id="btnClose" type="button" value="Close" onclick="window.close();" />&nbsp;
            <input id="btnHome" type="button" value="Login Page" onclick="location.href='login.aspx'" />
        </div>
    </asp:Panel>
</asp:Content>
