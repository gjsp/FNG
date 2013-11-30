<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false"
    CodeFile="stock_inout_prove.aspx.vb" Inherits="stock_inout_prove" Title="Import Export" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
    function assetOnchange(id)
    {
        if( $get(id).selectedIndex==1)
        {
            trGold.style.display='block';
            trCash.style.display='none';
        }
        else
        {
            trGold.style.display='none';
             trCash.style.display='block';
        }
    }
    </script>

    <style type="text/css">
        .style1
        {
            width: 613px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea">
        <div class="demoheading">
          Actual In/Out</div>
        <div>
            <cc1:TabContainer ID="tapImport" runat="server" ActiveTabIndex="2" Width="100%">
                <cc1:TabPanel ID="tpCash" runat="server" HeaderText="Cash">
                    <ContentTemplate>
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="upGv" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="gvAssetCash" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                                CellPadding="3" DataKeyNames="asset_id" DataSourceID="objSrcAssetCash" GridLines="Vertical"
                                                Width="100%">
                                                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                                <AlternatingRowStyle BackColor="Gainsboro" />
                                                <Columns>
                                                    <asp:BoundField DataField="datetime" HeaderText="Datetime" 
                                                        DataFormatString="{0:dd/MM/yyyy}" SortExpression="datetime" />
                                                    <asp:BoundField DataField="asset_type" HeaderText="Asset_Type" 
                                                        SortExpression="asset_type">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="type" HeaderText="Type" SortExpression="type">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="purity" HeaderText="Purity" SortExpression="purity" 
                                                        Visible="False">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="payment" HeaderText="Payment">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="created_by" HeaderText="Created_by" 
                                                        SortExpression="created_by">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="created_date" HeaderText="Created_Date" 
                                                        DataFormatString="{0:dd/MM/yyyy}" SortExpression="created_date" />
                                                    <asp:BoundField DataField="modifier_by" HeaderText="Updated_by" 
                                                        SortExpression="modifier_by">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="modifier_date" HeaderText="Updated_Date" 
                                                        DataFormatString="{0:dd/MM/yyyy}" SortExpression="modifier_date" />
                                                    <asp:TemplateField HeaderText="Remark" SortExpression="remark">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNote" runat="server" Text='<%# Bind("remark") %>'></asp:Label>
                                                            <asp:TextBox ID="txtNote" runat="server" Text='<%# Bind("remark") %>' 
                                                                Visible="False" Width="180px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="150px" Wrap="False" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Amount" SortExpression="quantity">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQuantity" runat="server"></asp:Label>
                                                            <asp:TextBox ID="txtQuantity" runat="server"  Visible="false" MaxLength="12"
                                                                Width="100px" Wrap="False"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" Width="100px"/>
                                                    </asp:TemplateField>
                                                     <asp:TemplateField HeaderText="Status" SortExpression="status">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("status") %>'></asp:Label>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="linkUpdate" runat="server" OnClick="linkUpdate_Click">Update</asp:LinkButton>
                                                            <asp:LinkButton ID="linkSave" Visible="false" runat="server" OnClick="linkSave_Click">Save</asp:LinkButton>&nbsp;
                                                            <asp:LinkButton ID="linkCancel" Visible="false" runat="server" 
                                                                OnClick="linkCancel_Click">Cancel</asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                            </asp:GridView>
                                            <asp:ObjectDataSource ID="objSrcAssetCash" runat="server" OldValuesParameterFormatString="original_{0}"
                                                SelectMethod="getAssetInout" TypeName="clsDB">
                                                <SelectParameters>
                                                    <asp:Parameter DefaultValue="Cash" Name="assetType" Type="String" ConvertEmptyStringToNull="False" />
                                                    <asp:Parameter ConvertEmptyStringToNull="False" DefaultValue="" Name="purity" Type="String" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="tpGold" runat="server" HeaderText="Gold 96.50">
                    <ContentTemplate>
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="gvAssetGold96" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                                CellPadding="3" DataKeyNames="asset_id" DataSourceID="objSrcAssetGold96" GridLines="Vertical"
                                                Width="100%">
                                                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                                <AlternatingRowStyle BackColor="Gainsboro" />
                                                <Columns>
                                                    <asp:BoundField DataField="datetime" HeaderText="Datetime" 
                                                        DataFormatString="{0:dd/MM/yyyy}" SortExpression="datetime" />
                                                    <asp:BoundField DataField="asset_type" HeaderText="Asset_Type" 
                                                        SortExpression="asset_type">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="type" HeaderText="Type" SortExpression="type">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="purity" HeaderText="Purity" SortExpression="purity">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="created_by" HeaderText="Created_by" 
                                                        SortExpression="created_by">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="created_date" HeaderText="Created_Date" 
                                                        DataFormatString="{0:dd/MM/yyyy}" SortExpression="created_date" />
                                                    <asp:BoundField DataField="modifier_by" HeaderText="Updated_by" 
                                                        SortExpression="modifier_by">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="modifier_date" HeaderText="Updated_Date" 
                                                        DataFormatString="{0:dd/MM/yyyy}" SortExpression="modifier_date" />
                                                    <asp:TemplateField HeaderText="Remark" SortExpression="remark">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNote" runat="server" Text='<%# Bind("remark") %>'></asp:Label>
                                                            <asp:TextBox ID="txtNote" runat="server" Text='<%# Bind("remark") %>' 
                                                                Visible="False" Width="180px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="150px" Wrap="False" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quantity" SortExpression="quantity">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQuantity" runat="server"></asp:Label>
                                                            <asp:TextBox ID="txtQuantity" runat="server" Visible="false" MaxLength="12" Text='<%# Bind("quantity") %>'
                                                                Width="100px" Wrap="False"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Status" SortExpression="status">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("status") %>'></asp:Label>
                                
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="linkUpdate" runat="server" OnClick="linkUpdate_Click">Update</asp:LinkButton>
                                                            <asp:LinkButton ID="linkSave" Visible="false" runat="server" OnClick="linkSave_Click">Save</asp:LinkButton>&nbsp;
                                                            <asp:LinkButton ID="linkCancel" Visible="false" runat="server" 
                                                                OnClick="linkCancel_Click">Cancel</asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                            </asp:GridView>
                                            <asp:ObjectDataSource ID="objSrcAssetGold96" runat="server" OldValuesParameterFormatString="original_{0}"
                                                SelectMethod="getAssetInout" TypeName="clsDB">
                                                <SelectParameters>
                                                    <asp:Parameter DefaultValue="Gold" Name="assetType" Type="String" ConvertEmptyStringToNull="False" />
                                                    <asp:Parameter ConvertEmptyStringToNull="False" DefaultValue="96" Name="purity" Type="String" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="Gold 99.99">
                    <ContentTemplate>
                        <table cellpadding="0" cellspacing="0" border="0">
                            <tr>
                                <td>
                                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                                        <ContentTemplate>
                                            <asp:GridView ID="gvAssetGold99" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                                CellPadding="3" DataKeyNames="asset_id" DataSourceID="objSrcGold99" GridLines="Vertical"
                                                Width="100%">
                                                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                                <AlternatingRowStyle BackColor="Gainsboro" />
                                                <Columns>
                                                    <asp:BoundField DataField="datetime" HeaderText="Datetime" 
                                                        DataFormatString="{0:dd/MM/yyyy}" SortExpression="datetime" />
                                                    <asp:BoundField DataField="asset_type" HeaderText="Asset_Type" 
                                                        SortExpression="asset_type">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="type" HeaderText="Type" SortExpression="type">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="purity" HeaderText="Purity" SortExpression="purity">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="created_by" HeaderText="Created_by" 
                                                        SortExpression="created_by">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="created_date" HeaderText="Created_Date" 
                                                        DataFormatString="{0:dd/MM/yyyy}" SortExpression="created_date" />
                                                    <asp:BoundField DataField="modifier_by" HeaderText="Updated_by" 
                                                        SortExpression="modifier_by">
                                                        <ItemStyle HorizontalAlign="Left" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="modifier_date" HeaderText="Updated_Date" 
                                                        DataFormatString="{0:dd/MM/yyyy}" SortExpression="modifier_date" />
                                                    <asp:TemplateField HeaderText="Remark" SortExpression="remark">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblNote" runat="server" Text='<%# Bind("remark") %>'></asp:Label>
                                                            <asp:TextBox ID="txtNote" runat="server" Text='<%# Bind("remark") %>' 
                                                                Visible="False" Width="180px"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle Width="150px" Wrap="False" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Quantity" SortExpression="quantity">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblQuantity" runat="server"></asp:Label>
                                                            <asp:TextBox ID="txtQuantity" runat="server" Visible="false" MaxLength="12" Text='<%# Bind("quantity") %>'
                                                                Width="100px" Wrap="False"></asp:TextBox>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Right" Width="100px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Status" SortExpression="status">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblStatus" runat="server" Text='<%# Bind("status") %>'></asp:Label>
                                
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="80px" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="linkUpdate" runat="server" OnClick="linkUpdate_Click">Update</asp:LinkButton>
                                                            <asp:LinkButton ID="linkSave" Visible="false" runat="server" OnClick="linkSave_Click">Save</asp:LinkButton>&nbsp;
                                                            <asp:LinkButton ID="linkCancel" Visible="false" runat="server" 
                                                                OnClick="linkCancel_Click">Cancel</asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                            </asp:GridView>
                                            <asp:ObjectDataSource ID="objSrcGold99" runat="server" OldValuesParameterFormatString="original_{0}"
                                                SelectMethod="getAssetInout" TypeName="clsDB">
                                                <SelectParameters>
                                                    <asp:Parameter DefaultValue="Gold" Name="assetType" Type="String" ConvertEmptyStringToNull="False" />
                                                    <asp:Parameter ConvertEmptyStringToNull="False" DefaultValue="99" Name="purity"
                                                        Type="String" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                        </ContentTemplate>
                                    </asp:UpdatePanel>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </cc1:TabPanel>
                
            </cc1:TabContainer>
        </div>
    </div>
</asp:Content>
