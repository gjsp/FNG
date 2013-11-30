<%@ Page Title="" Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false"
    CodeFile="users_roles.aspx.vb" Inherits="users_roles" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script language="javascript" type="text/javascript">

        function switchCheckbox() {

            for (i = 1; i < $get('ctl00_SampleContent_gvRole').rows.length; i++) {
                for (j = 2; j < 8; j++) {
                    $get('ctl00_SampleContent_gvRole').rows[i].cells[j].children[0].checked = $get('cbTemp').checked;
                }
            }

            if ($get('cbTemp').checked == true) {
                $get('cbTemp').checked = false;
            } else {
                $get('cbTemp').checked = true;
            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea" style="height: 500px">
        <div class="demoheading">
            Users Roles
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
        <fieldset>
            <legend class="topic">Manage Users Role</legend>
            <asp:UpdatePanel ID="udpMain" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gvRole" runat="server" Width="100%" AutoGenerateColumns="False"
                        DataKeyNames="menu_id" DataSourceID="odsRole" RowStyle-Height="31px" CellPadding="4">
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                        <Columns>
                            <asp:BoundField DataField="menu_name" HeaderText="Page Name" />
                            <asp:BoundField DataField="detail" HeaderText="Menu Name" />
                            <asp:TemplateField HeaderText="View">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbView" runat="server" Checked='<%# Bind("role_view") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="60px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Create">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbAdd" runat="server" Checked='<%# Bind("role_add") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Update">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbUpdate" runat="server" Checked='<%# Bind("role_update") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Delete">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbDelete" runat="server" Checked='<%# Bind("role_delete") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Export">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbExport" runat="server" Checked='<%# Bind("role_export") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Print">
                                <ItemTemplate>
                                    <asp:CheckBox ID="cbPrint" runat="server" Checked='<%# Bind("role_print") %>' />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="50px" />
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                        <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                        <AlternatingRowStyle BackColor="Gainsboro" />
                    </asp:GridView>
                    <asp:ObjectDataSource ID="odsRole" runat="server" SelectMethod="getMenuRole" TypeName="clsDB"
                        OldValuesParameterFormatString="original_{0}">
                        <SelectParameters>
                            <asp:ControlParameter ControlID="hdfUserId" ConvertEmptyStringToNull="False" Name="userId"
                                PropertyName="Value" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <asp:HiddenField ID="hdfUserId" runat="server" />
                    <div align="center">
                        <br />
                        <asp:Button ID="btnSave" runat="server" CssClass="buttonPro small black" Text="Save"
                            Width="80px" />
                        &nbsp;
                        <asp:Button ID="btnCheck" runat="server" CssClass="buttonPro small black" Width="80px"
                            OnClientClick="switchCheckbox();return false;" Text="Check All" />
                    </div>
                    <div style="display: none">
                        <input id="cbTemp" type="checkbox" checked="checked" />
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
    </div>
    <div class="demobottom">
    </div>
</asp:Content>
