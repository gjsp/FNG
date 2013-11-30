<%@ Page Language="VB" MasterPageFile="~/trade/admin/MasterPageAdmin.master" AutoEventWireup="false"
    CodeFile="add_user.aspx.vb" Inherits="admin_add_user" Title="Manage User" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="style.css" rel="stylesheet" type="text/css">
    <script type="text/javascript" language="javascript">

        function onCustAutoCompleteClick(source, eventArgs) {
            document.getElementById('ctl00_ContentPlaceHolder1_hdfCust_id').value = eventArgs.get_value();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="margin: auto; text-align: left; width: 500px;">
        <div style="text-align: center; font-weight: bold; font-size: 18px;">
            Add New User
        </div>
        <div style="display:none; text-align: left; margin-top: 25px;">
            <div style="float: left; width: 150px; text-align: right; padding-top: 3px;">
                Username * :</div>
            <div style="float: left; width: 345px; text-align: left; margin-left: 5px;">
                <asp:TextBox ID="txtUsername" runat="server" Width="170px" MaxLength="50"></asp:TextBox>
            </div>
            <div style="clear: both;">
            </div>
        </div>
        <div style="display:none;text-align: left; margin-top: 15px;">
            <div style="float: left; width: 150px; text-align: right; padding-top: 3px;">
                Password * :</div>
            <div style="float: left; width: 345px; text-align: left; margin-left: 5px;">
                <asp:TextBox ID="txtPwd" runat="server" Width="170px" MaxLength="50"></asp:TextBox>
            </div>
            <div style="clear: both;">
            </div>
        </div>
        <div style="text-align: left; margin-top: 15px;">
            <div style="float: left; width: 150px; text-align: right; padding-top: 3px;">
                Customer Name * :</div>
            <div style="float: left; width: 345px; text-align: left; margin-left: 5px;">
                <asp:TextBox ID="txtCustName" runat="server" Width="170px" MaxLength="50"></asp:TextBox>
                <ajaxToolkit:AutoCompleteExtender ID="txtCustName_AutoCompleteExtender" runat="server"
                    DelimiterCharacters="" CompletionInterval="300" Enabled="True" ServicePath="~/gtc.asmx"
                    UseContextKey="True" CompletionSetCount="20" OnClientItemSelected="onCustAutoCompleteClick"
                    TargetControlID="txtCustName" ServiceMethod="getCust_nameList">
                </ajaxToolkit:AutoCompleteExtender>
                <asp:HiddenField ID="hdfCust_id" runat="server" />
            </div>
            <div style="clear: both;">
            </div>
        </div>
        <div style="text-align: left; margin-top: 15px;">
            <div style="float: left; width: 150px; text-align: right; padding-top: 3px;">
                Level :</div>
            <div style="float: left; width: 345px; text-align: left; margin-left: 5px;">
                <asp:DropDownList ID="DropDownLevel" runat="server">
                    <asp:ListItem Value="1">Level 1</asp:ListItem>
                    <asp:ListItem Value="2">Level 2</asp:ListItem>
                    <asp:ListItem Value="3">Level 3</asp:ListItem>
                </asp:DropDownList>
            </div>
            <div style="clear: both;">
            </div>
        </div>
                
       
     
        <div style="text-align: center; margin-top: 25px;">
            <asp:Button ID="btnAdd" runat="server" CssClass="buttonPro small grey" 
                Text="Add" />
            <asp:Button ID="btnCancel" runat="server" CssClass="buttonPro small grey" 
                Text="Cancel" />
        </div>

    </div>
</asp:Content>
