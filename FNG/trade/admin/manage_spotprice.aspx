<%@ Page Title="" Language="VB" MasterPageFile="~/trade/admin/MasterPageAdmin.master"
    AutoEventWireup="false" CodeFile="manage_spotprice.aspx.vb" Inherits="trade_admin_manage_spotprice" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 1200px; margin: auto;">
        <fieldset class="k-fieldset" style="width: 98%;">
            <legend class="topic">Price</legend>
            <iframe id="ifmPriceView" width="100%" height="240px" frameborder="0" scrolling="no"
                    marginheight="0" marginwidth="0" src="price_spot_view.aspx"></iframe>
        </fieldset>
        <fieldset class="k-fieldset" style="width: 98%;">
            <legend class="topic">Setting Price</legend>
            <iframe id="ifmPriceChange" width="100%" height="380px" frameborder="0" scrolling="no"
                    marginheight="0" marginwidth="0" src="price_spot.aspx"></iframe>
        </fieldset>
    </div>
</asp:Content>
