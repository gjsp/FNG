<%@ Page Language="VB" AutoEventWireup="false" CodeFile="session_time_check.aspx.vb" Inherits="trade_session_time_check" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
     <asp:ScriptManager ID="SM" runat="server">
    </asp:ScriptManager>
        <asp:Timer ID="tm" runat="server">
        </asp:Timer>
    
    </div>
    </form>
</body>
</html>
