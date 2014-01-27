<%@ Control Language="VB" AutoEventWireup="false" CodeFile="ucUser.ascx.vb" Inherits="user_control_ucUser" %>
<link href="../main.css" rel="stylesheet" />

        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <fieldset style="background-color:#EEEEEE">
                    <legend class="topic">User Detail</legend>
                    <asp:MultiView ID="mvMain" runat="server">
                        <asp:View ID="vMain" runat="server">
                            <table cellpadding="2" cellspacing="2" border="0" style="background-color:#EEEEEE">
                            <tr>
                                <td style="width: 100px; text-align: right">User_ID:</td>
                                <td style="width: 200px">
                                    <asp:Label ID="lblUserId" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="text-align: right;">User_Name:</td>
                                <td>
                                    <asp:Label ID="lblUsername" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="text-align: right">First_Name:</td>
                                <td>
                                    <asp:Label ID="lblFname" runat="server"></asp:Label></td>
                            </tr>
                            <tr>
                                <td style="text-align: right">Last_Name:</td>
                                <td>
                                    <asp:Label ID="lblLname" runat="server"></asp:Label></td>
                            </tr>
                        </table>
                        </asp:View>
                        <asp:View ID="vNone" runat="server">
                            <div style='color:red;text-align:center;border:solid 1px silver;'>Data not Found.</div>
                        </asp:View>
                    </asp:MultiView>
                   
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>