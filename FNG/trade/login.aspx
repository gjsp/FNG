<%@ Page Language="VB" AutoEventWireup="false" CodeFile="login.aspx.vb" Inherits="login" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title><%=ConfigurationManager.AppSettings("NAME").ToString%></title>
    <link href="style/style_cust.css" rel="stylesheet" type="text/css" />
</head>

<body leftmargin="0" topmargin="100">
    <form id="form1" runat="server">
    <asp:Panel ID="pnMain" runat="server" DefaultButton="linkLogin">
        <table width="800" height="550" border="0" align="center" cellpadding="0" cellspacing="10"
            background="image_cust/login_bg.jpg" style="background-repeat: no-repeat">
            <tr>
                <td width="237" height="100">
                    &nbsp;
                </td>
                <td width="333">
                    &nbsp;
                </td>
            </tr>
            <tr>
                <td height="20" align="right" class="text_login">
                    <strong>Username</strong>
                </td>
                <td align="left">
                    <asp:TextBox ID="username" runat="server" CssClass="inputtext1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td height="20" align="right" class="text_login">
                    <strong>Password</strong>
                </td>
                <td align="left">
                    <asp:TextBox ID="password" runat="server" TextMode="Password" CssClass="inputtext1"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td height="30">
                    &nbsp;
                </td>
                <td align="left">
                    <asp:LinkButton ID="linkForget" runat="server" CssClass="link" Text="Forgot Password"></asp:LinkButton>
                </td>
            </tr>
            <tr>
                <td height="80" colspan="2" align="center" valign="top">
                    <table width="96" height="56" border="0" cellspacing="0" cellpadding="0" background="image_cust/login_bt.png">
                        <tr>
                            <td align="center">
                                <asp:LinkButton ID="linkLogin" runat="server" CssClass="link2" Text="Login"></asp:LinkButton>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="2" align="center" valign="top" height="30" class="text_copy">
                    Powered by Kit IT Create Co.,Ltd.
                </td>
            </tr>
        </table>
    </asp:Panel>
    </form>
</body>
</html>
