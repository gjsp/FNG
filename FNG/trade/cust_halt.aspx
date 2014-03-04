<%@ Page Title="" Language="VB" MasterPageFile="~/trade/tradeMasterPageBlank.master"
    AutoEventWireup="false" CodeFile="cust_halt.aspx.vb" Inherits="trade_cust_halt" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function pageLoad() {
            setTimeout("Check()", 5000);
        }

        function Check() {
            PageMethods.getHalt( OnSucceeded, OnFailed);
        }

        function OnFailed(error, userContext, methodName) { }

        function OnSucceeded(result, userContext, methodName) {

            if (result == 'n') {
                
                if ($get('ctl00_MainContent_hdfRole').value == 'sale') {
                    top.window.location = 'trading_sale.aspx';
                } else {
                    top.window.location = 'trading.aspx';
                }
            }
            else {
                setTimeout("Check()", 5000);
            }         
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:Panel ID="pnMain" runat="server">
         <div style="height: 500px; background-color:#fff; font-size: medium; font-weight: bold;
            color: black; text-align: center; vertical-align: middle">
            <br />
            <br />
            <br />
            ราคาถูกปิดชั่วคราวค่ะ
            <br />
            <p>
            </p>
           <asp:HiddenField ID="hdfRole" runat="server" />
        </div>

        <div style="height: 500px; background-color:#fff; font-size: medium; font-weight: bold;
            color: black; text-align: center; vertical-align: middle"></div>

    </asp:Panel>
</asp:Content>
