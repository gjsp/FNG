<%@ Page Language="VB" MasterPageFile="~/trade/admin/MasterPageAdmin.master" AutoEventWireup="false"
    CodeFile="manage_admin.aspx.vb" Inherits="admin_manage_admin" Title="Manage Admin" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <div style="width: 600px;" class="adminarea">
                <div style="text-align: left;">
                    <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="image/add_user.png" Width="120px" />
                </div>
                <div style="padding-top: 7px;">
                    <asp:GridView ID="gvUser" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        DataSourceID="ObjectDataSource1" Width="500px" BackColor="White" BorderColor="#999999"
                        BorderStyle="None" BorderWidth="1px" CellPadding="3" DataKeyNames="user_id" AllowSorting="True">
                        <Columns>
                            <asp:BoundField DataField="username" HeaderText="Username" SortExpression="username">
                                <ItemStyle Width="250px" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="EditButton" runat="server" ImageUrl="image/edit.png" Width="20px"
                                        OnClick="EditButton_Click" />
                                </ItemTemplate>
                                <ItemStyle Width="20px" HorizontalAlign="Center" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="DelButton" runat="server" ImageUrl="image/reject.png" Width="18px"
                                        OnClick="DelButton_Click1" />
                                </ItemTemplate>
                                <ItemStyle Width="20px" HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#666666" ForeColor="White" />
                        <HeaderStyle BackColor="#333333" ForeColor="White" />
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                        <AlternatingRowStyle BackColor="Gainsboro" />
                    </asp:GridView>
                    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="getUserAdmin"
                        TypeName="clsMain" OldValuesParameterFormatString="original_{0}"></asp:ObjectDataSource>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
