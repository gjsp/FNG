<%@ Page Language="VB" AutoEventWireup="false" CodeFile="log_in.aspx.vb" Inherits="log_in" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Login Page</title>
    <link href="login.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form_index" runat="server">
    <div id="main">
        <div class="border">
            <div class="border2">
                <h2 style="background-image: url(images/nation_logo.gif)">
                    &nbsp;&nbsp;Gold Trade System</h2>
            </div>
            <div class="border2">
                <div align="center" id="div_login">
                    <div align="center" class="login">
                        <h3 style="background-image: url(images/key.png)">
                            Log In</h3>
                        <table>
                            <tbody>
                                <tr>
                                    <td align="center" style="text-align: right">
                                        <asp:Label ID="lbl_username" runat="server" Text="Username : ">
                                        </asp:Label>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txt_username" runat="server" Width="150px">
                                        </asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="text-align: right">
                                    </td>
                                    <td style="text-align: left">
                                        <asp:RequiredFieldValidator ID="rfv_username" runat="server" ControlToValidate="txt_username"
                                            CssClass="validator" Display="Dynamic" ErrorMessage="Username is require !!!"
                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="center" style="text-align: right">
                                        <asp:Label ID="lbl_password" runat="server" Text="Password : "></asp:Label>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:TextBox ID="txt_password" runat="server" TextMode="Password" Width="150px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:RequiredFieldValidator ID="rfv_password" runat="server" ControlToValidate="txt_password"
                                            CssClass="validator" Display="Dynamic" ErrorMessage="Password is require !!!"
                                            SetFocusOnError="True"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                    </td>
                                    <td style="text-align: left">
                                        <asp:Button ID="btn_submit" runat="server" Text="Logon" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left">
                                        &nbsp;
                                    </td>
                                </tr>
                               <%-- <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left">
                                        <a style="text-decoration: none" href="trade/login.aspx">Trade Online</a>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        &nbsp;
                                    </td>
                                    <td style="text-align: left">
                                        <a style="text-decoration: none" href="phase3/cust/login.aspx">Trade Online 2</a>
                                    </td>
                                </tr>--%>
                            </tbody>
                        </table>
                    </div>
                </div>
            </div>
        </div>
        <div id="footer">
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td align="left" nowrap="nowrap">
                        <div align="left" class="copyright">
                            Copyright &#169; 2010 Gold Trade System. All Rights Reserved.
                        </div>
                    </td>
                    <td align="right" nowrap="nowrap">
                        <div align="right" class="copyright">
                        </div>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
