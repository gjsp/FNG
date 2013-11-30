<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false" CodeFile="user_detail.aspx.vb" Inherits="user_detail" title="Untitled Page" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" Runat="Server">
    <div class="demoarea">
        <div class="demoheading">
            User Profile</div>
       <asp:UpdatePanel ID="pnUser" runat="server">
        <ContentTemplate>
        <table border="0">
            <tr id="trRef" style="display:block">
            <td>Reference No :</td>
                <td>
                    <asp:Label ID="lblRef" runat="server" Text=""></asp:Label>
                                    </td>
            </tr>
            <tr>
                <td>User Name* :</td>
                <td><asp:TextBox ID="txtUsername" runat="server" Width="150px" /></td>
            </tr>
            <tr id="trPwd" style="display:block">
                <td>Password* :</td>
                <td><asp:TextBox ID="txtPassword" runat="server" TextMode="Password" Width="150px" /></td>
            </tr>
            <tr>
                <td>First Name* :</td>
                <td><asp:TextBox ID="txtFname" runat="server" Width="150px" /></td>
            </tr>
            <tr>
            <td>Last Name:</td>
                <td><asp:TextBox ID="txtLname" runat="server" Width="150px" /></td>
            </tr>
            <tr>
            <td>Position :</td>
                <td><asp:TextBox ID="txtPosition" runat="server" Width="150px" /></td>
            </tr>
            
            <tr style="display:none">
            <td valign="top">Team :</td>
                <td>
                    <asp:DropDownList ID="ddlTeam" runat="server"
                        DataTextField="team_name" DataValueField="team_id">
                    </asp:DropDownList>
                </td>
            </tr>
            
            <tr>
            <td>&nbsp;</td>
                <td>
                    &nbsp;</td>
            </tr>
            
            <tr>
            <td>&nbsp;</td>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Save" Width="100px" />
                &nbsp;<input id="Button3" type="button" value="Back" style="width:100px" onclick="location.href='users.aspx'" /><asp:HiddenField 
                        ID="hdfUser_id" runat="server" />
                </td>
            </tr>
            </table>
        </ContentTemplate>
       </asp:UpdatePanel>
         
        
    </div>
    <div class="demobottom">
    </div>
</asp:Content>

