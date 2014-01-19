<%@ Page Language="VB" MasterPageFile="~/trade/admin/MasterPageAdmin.master" AutoEventWireup="false"
    CodeFile="manage_users.aspx.vb" Inherits="admin_manage_users" Title="Manage Users" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style=" width: 600px;" class="adminarea">
       
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <Triggers>
           <%-- <asp:PostBackTrigger ControlID="linkExport" />--%>
        </Triggers>
            <ContentTemplate>
             <div style="text-align: left;">
            <table cellpadding="0" cellspacing="0" border="0">
                <tr>
                    <td style="width: 200px; text-align: left">
                        <asp:ImageButton ID="ImageButton1" runat="server" ImageUrl="image/add_user.png" Width="120px" />
                    </td>
                    <td style="width: 400px; text-align: right">
                    <asp:Panel ID="pnSearch" runat="server" DefaultButton="imgSearch">
                      <asp:TextBox ID="txtSearch" runat="server"></asp:TextBox>
                        <asp:ImageButton ID="imgSearch" runat="server" ImageUrl="image/search.bmp" />
                    </asp:Panel>
                    </td>
                </tr>
                <tr>
                    <td style="width: 200px; text-align: left">
                        <%--<asp:LinkButton ID="linkExport" runat="server">Export</asp:LinkButton>--%>
                    </td>
                    <td style="width: 400px; text-align: right">
                        &nbsp;</td>
                </tr>
            </table>
        </div>
           <asp:UpdateProgress ID="UpdateProg1" runat="server">
                <ProgressTemplate>
                    <div id="Div1" align="center" valign="middle" runat="server" style="position: absolute;
                        left: 43%; padding: 10px 10px 10px 10px; visibility: visible; border-color: silver;
                        border-style: solid; border-width: 1px; background-color: White;">
                        <img src="../img/indicator.gif" style="vertical-align: middle" alt="Processing" />
                        Loading ... &nbsp;
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            
                <div style="padding-top: 7px;">
                    <asp:GridView ID="gvUser" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        DataSourceID="ObjectDataSource1" Width="600px" BackColor="White" BorderColor="#999999"
                        BorderStyle="None" BorderWidth="1px" CellPadding="3" DataKeyNames="user_id" AllowSorting="True">
                        <Columns>
                            <asp:BoundField DataField="username" HeaderText="Username" SortExpression="username">
                                <ItemStyle Width="135px" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:BoundField DataField="firstname" HeaderText="Firstname" SortExpression="firstname">
                                <ItemStyle HorizontalAlign="Justify" Width="310px" />
                            </asp:BoundField>
                            <asp:BoundField DataField="cust_level" HeaderText="Level" SortExpression="cust_level">
                                <ItemStyle Width="30px" HorizontalAlign="Center" />
                            </asp:BoundField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbHalt" runat="server" />
                                </ItemTemplate>
                                <ItemStyle Width="20px" HorizontalAlign="Center" />
                            </asp:TemplateField>
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
                                        OnClick="DelButton_Click" />
                                </ItemTemplate>
                                <ItemStyle Width="20px" HorizontalAlign="Center" />
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#666666" ForeColor="White" />
                        <HeaderStyle BackColor="#333333" ForeColor="White" />
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                        <AlternatingRowStyle BackColor="Gainsboro" />
                    </asp:GridView>
                    <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" SelectMethod="getUsernameSearch"
                        TypeName="clsMain" OldValuesParameterFormatString="original_{0}">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="txtSearch" ConvertEmptyStringToNull="False" Name="word"
                                PropertyName="Text" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>

                    
                </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
