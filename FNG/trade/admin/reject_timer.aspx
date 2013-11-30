<%@ Page Language="VB" AutoEventWireup="false" CodeFile="reject_timer.aspx.vb" Inherits="admin_reject_timer" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
  
  <script language="javascript" type="text/javascript">
      function rejectTime() {
          PageMethods.rejectTimeout(OnSucceeded, OnFailed);
      }
      function pageLoad() {
          rejectTime();
      }
      function OnSucceeded(result, userContext, methodName) {
          setTimeout("rejectTime()", 3000);
      }

      function OnFailed(error, userContext, methodName) { 
      }
  </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server" EnablePageMethods="true">
    </asp:ScriptManager>
     <div align="center">
                    <%--<asp:Timer ID="tmTime" OnTick="tmTime_Tick" runat="server" Interval="3000">
                    </asp:Timer>
                     <asp:UpdatePanel ID="upTimer" runat="server" UpdateMode="Conditional">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="tmTime" EventName="Tick" />
                        </Triggers>
                        <ContentTemplate>
                        </ContentTemplate>
                    </asp:UpdatePanel>--%>
                   
                    </div> 
    </form>
</body>
</html>
