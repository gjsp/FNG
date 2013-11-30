<%@ Page Title="" Language="VB" MasterPageFile="~/trade/tradeMasterPageBlank.master"
    AutoEventWireup="false" CodeFile="cust_first_trade.aspx.vb" Inherits="trade_cust_first_trade" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
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
    <div class="demoarea" style=" width: 400; margin-top: 1cm; text-align: center">
        <fieldset class="k-fieldset">
            <legend class="topic">First Trade</legend>
            <div style="text-align:left">
                <%--Privacy policy : Please enter your information on first time log in to system. That make best security for your account.--%>
                ในการลงทะเบียนครั้งแรก กรุณากรอกข้อมูลให้ครบถ้วน
            </div>
            <asp:UpdatePanel ID="upFirstTrade" runat="server">
                <ContentTemplate>
                  
                   
                    <div style="text-align: left; margin-top: 15px;">
                        <div style="float: left; width: 150px; text-align: right; padding-top: 3px;">
                            เลขที่บัตรประจำตัวประชาชน/<br />เลขจดทะเบียน(นิติบุคคล) :</div>
                        <div style="float: left; width: 350px; text-align: left; margin-left: 5px;">
                            <asp:TextBox ID="txtIDCardz1" runat="server" Width="10px" MaxLength="1" />
                            <asp:TextBox ID="txtIDCardz2" runat="server" Width="10px" MaxLength="1" />
                            <asp:TextBox ID="txtIDCardz3" runat="server" Width="10px" MaxLength="1" />
                            <asp:TextBox ID="txtIDCardz4" runat="server" Width="10px" MaxLength="1" />
                            <asp:TextBox ID="txtIDCardz5" runat="server" Width="10px" MaxLength="1" />
                            <asp:TextBox ID="txtIDCardz6" runat="server" Width="10px" MaxLength="1" />
                            <asp:TextBox ID="txtIDCardz7" runat="server" Width="10px" MaxLength="1" />
                            <asp:TextBox ID="txtIDCardz8" runat="server" Width="10px" MaxLength="1" />
                            <asp:TextBox ID="txtIDCardz9" runat="server" Width="10px" MaxLength="1" />
                            <asp:TextBox ID="txtIDCardz10" runat="server" Width="10px" MaxLength="1" />
                            <asp:TextBox ID="txtIDCardz11" runat="server" Width="10px" MaxLength="1" />
                            <asp:TextBox ID="txtIDCardz12" runat="server" Width="10px" MaxLength="1" />
                            <asp:TextBox ID="txtIDCardz13" runat="server" Width="10px" MaxLength="1" />
                        </div>
                        <div style="clear: both;">
                        </div>
                    </div>
                    <div style="text-align: left; margin-top: 15px;">
                        <div style="float: left; width: 150px; text-align: right; padding-top: 3px;">
                            วันเดือนปีเกิด/<br />วันที่จดทะเบียน(นิติบุคคล) :</div>
                        <div style="float: left; width: 350px; text-align: left; margin-left: 5px;">
                            <asp:DropDownList ID="ddlday" runat="server">
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlMonth" runat="server">
                            </asp:DropDownList>
                            <asp:DropDownList ID="ddlYear" runat="server">
                            </asp:DropDownList>
                            (วัน:เดือน:ปี)</div>
                        <div style="clear: both;">
                        </div>
                    </div>
                     <div style="text-align: left; margin-top: 15px;">
                        <div style="float: left; width: 150px; text-align: right; padding-top: 3px;">
                            เบอร์โทรศัพท์มือถือ :</div>
                        <div style="float: left; width: 350px; text-align: left; margin-left: 5px;">
                            <asp:TextBox ID="txtMobile" runat="server" Width="200px" MaxLength="10"  />
                           
                            &nbsp;(เฉพาะตัวเลข <%= clsManage.phoneDigit.ToString()%> หลัก)</div>
                        <div style="clear: both;">
                        </div>
                    </div>
                    <%--<div style="text-align: center; margin-top: 25px; width: 400px;">
                        &nbsp;</div>--%>
                </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
        <fieldset class="k-fieldset">
            <legend class="topic">Change Username Password</legend>
            <div style="text-align:left">
                เพื่อความปลอดภัยสูงสุด กรุณาเปลี่ยน username และ password ของท่าน<br />
                Password ใหม่ต้องมีอย่างน้อย 6 อักขระ และต้องประกอบด้วยตัวอักษรและตัวเลข
                <br />
            </div>
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
                            <asp:Label ID="lblDup" runat="server" ForeColor="Red" text=""></asp:Label>
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
                                ErrorMessage="Username do not match."></asp:CompareValidator>
                        </div>
                        <div style="clear: both;">
                        </div>
                    </div>
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
                        <div style="float: left; width: 700px; text-align: left; margin-left: 5px;">
                            <asp:TextBox ID="txtPwd" runat="server" Width="200" TextMode="Password"></asp:TextBox>
                            <asp:CustomValidator ID="cvPwdFormat" runat="server" 
                                ControlToValidate="txtPwd"  ClientValidationFunction="validatePwdFormat"
                                ErrorMessage="Password must be at least 6 characters and a combination of letters and numbers." 
                                Display="Dynamic"></asp:CustomValidator>
                        </div>
                        <div style="clear: both;">
                        </div>
                    </div>
                    <div style="text-align: left; margin-top: 15px;">
                        <div style="float: left; width: 150px; text-align: right; padding-top: 3px;">
                            Confirm New Password :</div>
                        <div style="float: left; width: 350px; text-align: left; margin-left: 5px;">
                            <asp:TextBox ID="txtPwdCon" runat="server" Width="200" TextMode="Password"></asp:TextBox>
                            <asp:CompareValidator ID="cvPwd" runat="server" ControlToCompare="txtPwd" ControlToValidate="txtPwdCon"
                                ErrorMessage="Password do not match."></asp:CompareValidator>
                        </div>
                        <div style="clear: both;">
                        </div>
                    </div>
                                     
                </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
 <asp:Button ID="btnOk" runat="server" CssClass="buttonPro small black" Text="OK" Width="70px" CausesValidation="true" />
    </div>
</asp:Content>
