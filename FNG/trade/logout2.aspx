<%@ Page Title="" Language="VB" MasterPageFile="~/trade/tradeMasterPageBlank.master"
    AutoEventWireup="false" CodeFile="logout2.aspx.vb" Inherits="trade_logout2" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Panel ID="pnMain" runat="server">
         <div style="height: 500px; background-color:#fff; font-size: medium; font-weight: bold;
            color: black; text-align: center; vertical-align: middle">
            <br />
            <br />
            <br />
            เครื่องคอมพิวเตอร์ของคุณได้มีการใช้งานระบบอยู่แล้ว
            <br />
            โปรดออกจากระบบก่อนเข้าระบบอีกครั้ง<p>
            </p>
            <input id="btnClose" type="button" value="Close" onclick="window.close();" />
        </div>
    </asp:Panel>
</asp:Content>
