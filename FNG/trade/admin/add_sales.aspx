<%@ Page Language="VB" MasterPageFile="~/trade/admin/MasterPageAdmin.master" AutoEventWireup="false" CodeFile="add_sales.aspx.vb" Inherits="admin_add_sales" title="Manage Sales" %>

<%@ Register Src="~/user_control/ucUser.ascx" TagPrefix="uc1" TagName="ucUser" %>


<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <link href="style.css" rel="stylesheet" type="text/css">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <asp:UpdatePanel ID="upMain" runat="server">
        <ContentTemplate>
            <div style="margin: auto; text-align: left; width: 600px;">
                <div style="text-align: center; font-weight: bold; font-size: 18px;">
                    Add New Sales
                </div>

                <div style="text-align: left; margin-top: 25px;">
                    <div style="float: left; width: 150px; text-align: right; padding-top: 3px;">User ID&nbsp; :</div>
                    <asp:Panel ID="pnMain" runat="server" DefaultButton="btnSearch">
                        <div style="float: left; width: 400px; text-align: left; margin-left: 5px;">
                            <asp:TextBox ID="txtUserId" runat="server" Width="170px" MaxLength="50"></asp:TextBox>
                            <span>
                                <asp:Button ID="btnSearch" runat="server" CssClass="buttonPro small grey" Text="Search" />
                                &nbsp;<asp:Button ID="Addbt" runat="server" CssClass="buttonPro small grey" Text="Create" />
                                &nbsp;<asp:Button ID="Cancelbt" runat="server" CssClass="buttonPro small grey" Text="Back" />
                                <asp:HiddenField ID="hdfUserId" runat="server" />
                            </span>
                        </div>
                    </asp:Panel>
                   
                    <div style="clear: both;"></div>
                </div>
                
                <div style="text-align: left; margin-top: 25px;">
                    <div style="float: left; width: 150px; text-align: right; padding-top: 3px;"></div>
                    <div style="float: left; width: 345px; text-align: left; margin-left: 5px;">
                        <uc1:ucUser runat="server" ID="ucUser" />
                    </div>
                    <div style="clear: both;"></div>
                </div>

            </div>

        </ContentTemplate>

    </asp:UpdatePanel>

</asp:Content>

