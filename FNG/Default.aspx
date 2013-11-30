<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false"
    CodeFile="Default.aspx.vb" Inherits="_Default" Title="Home" %>

<%@ Register Src="user_control/ucPortFolio.ascx" TagName="ucPortFolio" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea">
        <div class="demoheading">
            
        </div>
        <div align="center" style="font-size:medium">
                    Page Permission!
                </div>
        <asp:Label ID="lbl" runat="server"></asp:Label>
        &nbsp;<div style="height: 450px">
        </div>
    </div>
    <div class="demobottom">
    </div>
</asp:Content>
