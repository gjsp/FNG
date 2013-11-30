<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false"
    CodeFile="user_team.aspx.vb" Inherits="user_team" Title="Team" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea" style="height: 500px">
        <div class="demoheading">
            Team</div>
        <table border="0">
            <tr>
                <td>
                    Team Name* :
                </td>
                <td>
                    <asp:TextBox ID="txtTeamName" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    Detail :
                </td>
                <td>
                    <asp:TextBox ID="txtDetail" runat="server" Height="60px" TextMode="MultiLine" 
                        Width="300px" />
                </td>
            </tr>
            <tr>
                <td>
                    &nbsp;
                </td>
                <td>
                    <asp:Button ID="btnSave" runat="server" Text="Add Team" />
                </td>
            </tr>
        </table>
        <div>
            <br />
            <asp:GridView ID="gvTeam" runat="server"
                AutoGenerateColumns="False" BackColor="White" BorderColor="#999999"
                BorderStyle="None" BorderWidth="1px" CellPadding="3" DataKeyNames="team_id"
                DataSourceID="objSrcTeam" GridLines="Vertical">
                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                <Columns>
                    <asp:BoundField DataField="team_name" HeaderText="team_name" 
                        SortExpression="team_name" >
                        <ItemStyle Width="200px" Wrap="False" />
                    </asp:BoundField>
                    <asp:BoundField DataField="detail" HeaderText="detail" 
                        SortExpression="detail" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:ImageButton ID="imgDel" runat="server" ImageUrl="~/images/i_del.png" 
                                onclick="imgDel_Click" 
                                onclientclick="return confirm('Do you want to delete?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                <AlternatingRowStyle BackColor="Gainsboro" />
            </asp:GridView>
                        
            <asp:ObjectDataSource ID="objSrcTeam" runat="server" DeleteMethod="Delete" 
                InsertMethod="Insert" OldValuesParameterFormatString="original_{0}" 
                SelectMethod="GetData" TypeName="dsTableAdapters.teamTableAdapter" 
                UpdateMethod="Update">
                <DeleteParameters>
                    <asp:Parameter Name="Original_team_id" Type="Int32" />
                </DeleteParameters>
                <UpdateParameters>
                    <asp:Parameter Name="team_name" Type="String" />
                    <asp:Parameter Name="detail" Type="String" />
                    <asp:Parameter Name="Original_team_id" Type="Int32" />
                </UpdateParameters>
                <InsertParameters>
                    <asp:Parameter Name="team_name" Type="String" />
                    <asp:Parameter Name="detail" Type="String" />
                </InsertParameters>
            </asp:ObjectDataSource>
        </div>
    </div>
    <div class="demobottom">
    </div>
</asp:Content>
