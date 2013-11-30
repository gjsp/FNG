<%@ Page Language="VB" AutoEventWireup="false" CodeFile="timer_price.aspx.vb" Inherits="trade_admin_timer_price" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../style.css" rel="stylesheet" type="text/css" />
    <%--    <script language="javascript" type="text/javascript">
        function setPriceColor(color) {
            if (top.window.document.getElementById('hdfChangeColor').value=='y') {
               
                top.window.document.getElementById('div_price_color').style.backgroundColor = color;
            }
            
        }
    </script>--%>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="SMTime" EnablePageMethods="true" runat="server">
    </asp:ScriptManager>
    <div align="center">
        <asp:HiddenField ID="hdfTime" runat="server" />
        <asp:UpdatePanel ID="upTimer" runat="server" UpdateMode="Conditional">
            <ContentTemplate>
            
                <asp:Label ID="lblTimer" runat="server" Font-Bold="true" ForeColor="Red" Text="Time Out!!!."></asp:Label>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="tmTime" EventName="Tick" />
            </Triggers>
        </asp:UpdatePanel>
        <asp:Timer ID="tmTime" OnTick="tmTime_Tick" runat="server" Interval="1000">
        </asp:Timer>
    </div>
    </form>
</body>
</html>
