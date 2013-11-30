<%@ Page Language="VB" AutoEventWireup="false" CodeFile="trade_timing.aspx.vb" Inherits="trade_trade_timing" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link rel="stylesheet" href="http://code.jquery.com/ui/1.10.2/themes/smoothness/jquery-ui.css" />
    <script src="http://code.jquery.com/jquery-1.9.1.js"></script>
    <script src="http://code.jquery.com/ui/1.10.2/jquery-ui.js"></script>
    <link rel="stylesheet" href="/resources/demos/style.css" />
    <link href="../button/css/buttonPro.css" rel="stylesheet" type="text/css" />


</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ScriptManager ID="SMTime" EnablePageMethods="true" runat="server">
        </asp:ScriptManager>
        <div id="dialog" align="center" style="width:400;height:200px">
            <asp:UpdatePanel ID="upTimer" runat="server" UpdateMode="Conditional">
                <ContentTemplate>
                    <asp:Label ID="lblCount" runat="server" Font-Bold="true" ForeColor="Red" Font-Size="XX-Large"></asp:Label>
                </ContentTemplate>
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="tmTime" EventName="Tick" />
                </Triggers>
            </asp:UpdatePanel>
            <asp:Timer ID="tmTime" OnTick="tmTime_Tick" runat="server" Interval="1000" Enabled="false" >
            </asp:Timer>
            <br />
            <br />
            <div style="display:none">
             <asp:Button ID="btnOk" runat="server" CssClass="buttonPro small black" Width="80px" Text="ตกลง"  />
            </div>
             </div>
       
    </div>

    
    </form>
</body>
</html>
