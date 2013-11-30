<%@ Page Title="" Language="VB" MasterPageFile="~/trade/tradeMasterPageBlank.master"
    AutoEventWireup="false" CodeFile="register_confirm.aspx.vb" Inherits="trade_register_confirm" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="style.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .page_body
        {
            font-family: Tahoma, Arial, sans-serif;
            font-size: 140%;
            margin: 0px;
           
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="page_body">
        <asp:Panel ID="pnSeccess" runat="server" Visible="false">
            <div style="text-align: center; min-height: 600px; margin-top: 1cm;">
                Plaese activate you account via E-mail<br />
                Finest GOLD<br />
                <a href="login.aspx">Click to Login</a>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnFail" runat="server" Visible="false">
            <div style="text-align: center; min-height: 600px; margin-top: 1cm">
                Register Fail!.<br />
                Please contact Finest GOLD
                <br />
            </div>
        </asp:Panel>
    </div>
</asp:Content>
