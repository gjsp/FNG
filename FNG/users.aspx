<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false"
    CodeFile="users.aspx.vb" Inherits="users" Title="User Management" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea" style="height: 500px">
        <div class="demoheading">
            <table border="0" width="100%">
                <tr>
                    <td align="left">
                        Users
                    </td>
                    <td align="right">
                    </td>
                </tr>
            </table>
        </div>
         <div align="center">
            <asp:UpdateProgress ID="upPG" runat="server">
                <ProgressTemplate>
                    <div id="divPG" align="center" valign="middle" runat="server" style="position: absolute;
                        left: 47%; padding: 10px 10px 10px 10px; visibility: visible; border-color: silver;
                        border-style: solid; border-width: 1px; background-color: White; margin-top: 10px">
                         <img alt="" src="images/loading.gif" />
                        Loading ...
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
        </div>
        <div align="left" style="padding-bottom: 5px">
            <asp:LinkButton ID="linkRptSell" PostBackUrl="~/user_detail.aspx" runat="server">New User</asp:LinkButton>
        </div>
        <asp:UpdatePanel ID="upnUser" runat="server">
            <ContentTemplate>
                <asp:GridView ID="gvUser" runat="server" AllowPaging="True" AllowSorting="True" AutoGenerateColumns="False"
                    BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                    CellPadding="3" DataKeyNames="user_id" DataSourceID="objSrcUser" GridLines="Vertical"
                    Width="90%" PageSize="20">
                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                    <Columns>
                        <asp:CommandField ButtonType="Image" SelectImageUrl="~/images/i_detail.png" ShowSelectButton="True">
                            <ItemStyle HorizontalAlign="Center" Width="30px" />
                        </asp:CommandField>
                        <asp:BoundField HeaderText="" >
                        <ItemStyle HorizontalAlign="Center" Width="60px" />
                        </asp:BoundField>
                        <asp:BoundField DataField="user_id" HeaderText="user_id" ReadOnly="True" SortExpression="user_id" />
                        <asp:BoundField DataField="user_name" HeaderText="user_name" SortExpression="user_name" />
                        <asp:BoundField DataField="firstname" HeaderText="firstname" SortExpression="firstname" />
                        <asp:BoundField DataField="lastname" HeaderText="lastname" SortExpression="lastname" />
                        <asp:BoundField DataField="status" HeaderText="status" SortExpression="status" />
                        <asp:BoundField DataField="position" HeaderText="position" SortExpression="position" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:ImageButton ID="imgDel" runat="server" ImageUrl="~/images/i_del.png" OnClick="imgDel_Click" />
                            </ItemTemplate>
                            <ItemStyle HorizontalAlign="Center" Width="30px" />
                        </asp:TemplateField>
                    </Columns>
                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" Font-Size="Large" />
                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                    <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                    <AlternatingRowStyle BackColor="Gainsboro" />
                </asp:GridView>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:ObjectDataSource ID="objSrcUser" runat="server" OldValuesParameterFormatString="original_{0}"
            SelectMethod="GetData" TypeName="dsTableAdapters.usersTableAdapter"></asp:ObjectDataSource>
    </div>
    <div class="demobottom">
    </div>
</asp:Content>
