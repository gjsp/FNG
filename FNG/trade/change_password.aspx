<%@ Page Language="VB" MasterPageFile="~/trade/tradeMasterPage.master" AutoEventWireup="false"
    CodeFile="change_password.aspx.vb" Inherits="change_password" Title="Change Password" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">
        function getDupUsername() {
            var username = $get('<% =txtUsername.ClientID%>').value;
            if (username != "") {
                PageMethods.getDupUsername(username, OnSucceeded, OnFailed);
            } else {
                $get('ctl00_MainContent_lblDup').innerText = "";
            }
        }
        function OnSucceeded(result, userContext, methodName) {
            var lblDup = $get('<% =lblDup.ClientID%>');
            if (result == "y") {
                lblDup.innerText = "Available";
                lblDup.style.color = "green";
            } else {
                lblDup.innerText = "Not Available";
                lblDup.style.color = "red";
            }
        }
        function OnFailed(error, userContext, methodName) { }

        function validatePwdFormat(sender, args) {
            var str = $get($get(sender.id).controltovalidate).value
            if (str.length < 6) { args.IsValid = false; }

            var numberFlag = false;
            var stringFlag = false

            for (var i = 0; i < str.length; i++) {
                if (isNaN(str.charAt(i)) == false) {
                    numberFlag = true;
                } else {
                    stringFlag = true;
                }
            }
            if (numberFlag == true && stringFlag == true) {
                args.IsValid = true;
            }
            else {
                args.IsValid = false;
            }
        } 

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
     <div class="demoarea">
        <asp:Panel ID="pnUsername" runat="server" Visible = "false">
            <fieldset class="k-fieldset">
                <legend class="topic">Change Username</legend>
                <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                    <ContentTemplate>
                        <div style="text-align: left; margin-top: 15px;">
                            <div style="float: left; width: 150px; text-align: right; padding-top: 3px;">
                                Old Username :</div>
                            <div style="float: left; width: 350px; text-align: left; margin-left: 5px;">
                                <asp:TextBox ID="txtOldUsername" runat="server" Width="200"></asp:TextBox>
                            </div>
                            <div style="clear: both;">
                            </div>
                        </div>
                        <div style="text-align: left; margin-top: 15px;">
                            <div style="float: left; width: 150px; text-align: right; padding-top: 3px;">
                                New Username :</div>
                            <div style="float: left; width: 350px; text-align: left; margin-left: 5px;">
                                <asp:TextBox ID="txtUsername" runat="server" Width="200"></asp:TextBox>
                                <asp:Label ID="lblDup" runat="server" ForeColor="Red" Text=""></asp:Label>
                            </div>
                            <div style="clear: both;">
                            </div>
                        </div>
                        <div style="text-align: left; margin-top: 15px;">
                            <div style="float: left; width: 150px; text-align: right; padding-top: 3px;">
                                Confirm New Username :</div>
                            <div style="float: left; width: 350px; text-align: left; margin-left: 5px;">
                                <asp:TextBox ID="txtUsernameCon" runat="server" Width="200"></asp:TextBox>
                                <asp:CompareValidator ID="cvUser" runat="server" ControlToCompare="txtUsername" ControlToValidate="txtUsernameCon"
                                    ErrorMessage="Username do not match." ValidationGroup="usr" Display="Dynamic"></asp:CompareValidator>
                            </div>
                            <div style="clear: both;">
                            </div>
                        </div>

                         <div style="text-align: left; margin-top: 15px;">
                            <div style="float: left; width: 150px; text-align: right; padding-top: 3px;">
                               </div>
                            <div style="float: left; width: 350px; text-align: left; margin-left: 5px;">
                                <asp:Button ID="btnSaveUser" runat="server" CssClass="buttonPro small black" Text="Save" Width="70" ValidationGroup="usr" />
                                &nbsp;<asp:Button ID="btnCancalUser" runat="server" CssClass="buttonPro small black" Text="Cancel"
                                    Width="70" />
                            </div>
                            <div style="clear: both;">
                            </div>
                        </div>

                    </ContentTemplate>
                </asp:UpdatePanel>
            </fieldset>
        </asp:Panel>
        <asp:Panel ID="pnPwd" runat="server" Visible = "false">
            <fieldset class="k-fieldset">
                <legend class="topic">Change Password</legend>
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div style="text-align: left; margin-top: 15px;">
                            <div style="float: left; width: 150px; text-align: right; padding-top: 3px;">
                                Old Password :</div>
                            <div style="float: left; width: 350px; text-align: left; margin-left: 5px;">
                                <asp:TextBox ID="txtOldPwd" runat="server" Width="200" TextMode="Password"></asp:TextBox>
                            </div>
                            <div style="clear: both;">
                            </div>
                        </div>
                        <div style="text-align: left; margin-top: 15px;">
                            <div style="float: left; width: 150px; text-align: right; padding-top: 3px;">
                                New Password :</div>
                            <div style="float: left; width: 550px; text-align: left; margin-left: 5px;">
                                <asp:TextBox ID="txtNewPwd" runat="server" Width="200" TextMode="Password"></asp:TextBox>
                                <asp:CustomValidator ID="cvPwdFormat" runat="server" ControlToValidate="txtNewPwd"
                                    ClientValidationFunction="validatePwdFormat" ErrorMessage="Password must be at least 6 characters and a combination of letters and numbers."
                                    Display="Dynamic" ValidationGroup="pwd"></asp:CustomValidator>
                            </div>
                            <div style="clear: both;">
                            </div>
                        </div>
                        <div style="text-align: left; margin-top: 15px;">
                            <div style="float: left; width: 150px; text-align: right; padding-top: 3px;">
                                Confirm New Password :</div>
                            <div style="float: left; width: 350px; text-align: left; margin-left: 5px;">
                                <asp:TextBox ID="txtNewPwdCon" runat="server" Width="200" TextMode="Password"></asp:TextBox>
                                <asp:CompareValidator ID="CompareValidator2" runat="server" ControlToCompare="txtNewPwd"
                                    ControlToValidate="txtNewPwdCon" ErrorMessage="Password do not match." ValidationGroup="pwd" Display="Dynamic"></asp:CompareValidator>
                            </div>
                            <div style="clear: both;">
                            </div>
                        </div>

                         <div style="text-align: left; margin-top: 15px;">
                            <div style="float: left; width: 150px; text-align: right; padding-top: 3px;">
                               </div>
                            <div style="float: left; width: 350px; text-align: left; margin-left: 5px;">
                                <asp:Button ID="btnSavePwd" runat="server" CssClass="buttonPro small black" Text="Save" ValidationGroup="pwd" Width="70" />
                                &nbsp;<asp:Button ID="btnCancelPwd" runat="server" CssClass="buttonPro small black" Text="Cancel" Width="70" />
                            </div>
                            <div style="clear: both;">
                            </div>
                        </div>
                       
                    </ContentTemplate>
                </asp:UpdatePanel>
            </fieldset>
        </asp:Panel>
    </div>
</asp:Content>
