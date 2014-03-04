<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false"
    CodeFile="stock_inout.aspx.vb" Inherits="stock_inout" Title="Import Export" %>

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
           Planning In/Out</div>
        <div>
            <cc1:TabContainer ID="tapImport" runat="server" ActiveTabIndex="3" Width="100%">
                <cc1:TabPanel runat="server" HeaderText="In/Out" ID="tpImport">
                    <HeaderTemplate>
                        In/Out
                    </HeaderTemplate>
                    <ContentTemplate>
                        <asp:UpdatePanel ID="udpMain" runat="server">
                            <ContentTemplate>
                                <div>
                                    <table border="0" cellpadding="1" cellspacing="1">
                                        <tr>
                                            <td>
                                                Date :
                                            </td>
                                            <td class="style1">
                                                <asp:TextBox ID="txtDate" runat="server" Width="80px"></asp:TextBox><cc1:CalendarExtender
                                                    ID="txtDate_CalendarExtender" runat="server" PopupButtonID="imgDate" Enabled="True"
                                                    PopupPosition="BottomRight" TargetControlID="txtDate">
                                                </cc1:CalendarExtender>
                                                <asp:ImageButton ID="imgDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Type :
                                            </td>
                                            <td class="style1">
                                                <asp:DropDownList ID="ddlType" runat="server" Width="70px">
                                                    <asp:ListItem>In</asp:ListItem>
                                                    <asp:ListItem>Out</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Asset Type :
                                            </td>
                                            <td class="style1">
                                                <asp:DropDownList ID="ddlAssetType" runat="server" Width="70px">
                                                    <asp:ListItem>Cash</asp:ListItem>
                                                    <asp:ListItem>Gold</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr id="trGold" style="display: none">
                                            <td>
                                                Purity :
                                            </td>
                                            <td class="style1">
                                                <asp:DropDownList ID="ddlPure" runat="server">
                                                    <asp:ListItem Value="96">96.50%</asp:ListItem>
                                                    <asp:ListItem Value="99">99.99%</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        
                                        <tr ID="trCash" style="display: none">
                                            <td>
                                                Payment* :
                                            </td>
                                            <td>
                                                <table border="0" cellpadding="0" cellspacing="0">
                                                    <tr>
                                                        <td>
                                                            <asp:DropDownList ID="ddlPayment" runat="server">
                                                                <asp:ListItem Selected="True" Value="cash">Cash</asp:ListItem>
                                                                <asp:ListItem Value="cheq">Cheque</asp:ListItem>
                                                                <asp:ListItem Value="trans">Payin</asp:ListItem>
                                                            </asp:DropDownList>
                                                        </td>
                                                        <td>
                                                            <table>
                                                                <tr>
                                                                    <td ID="bank1" style="display: block">
                                                                        Bank :
                                                                    </td>
                                                                    <td ID="bank2" style="display: block">
                                                                        <asp:DropDownList ID="ddlBank" runat="server">
                                                                        </asp:DropDownList>
                                                                    </td>
                                                                    <td ID="cheq1" style="display: block">
                                                                        Cheque no :
                                                                    </td>
                                                                    <td ID="cheq2" style="display: block">
                                                                        <asp:TextBox ID="txtCheq" runat="server" Width="80px"></asp:TextBox>
                                                                    </td>
                                                                    <td ID="cheq3" style="display: block">
                                                                        Due date :
                                                                    </td>
                                                                    <td ID="cheq4" style="display: block">
                                                                       <%-- <asp:TextBox ID="txtDuedate" runat="server" Width="70px"></asp:TextBox>
                                                                        <calendarextender ID="txtDuedate_CalendarExtender" runat="server" 
                                                                            enabled="True" popupbuttonid="imgDuedate" popupposition="BottomRight" 
                                                                            targetcontrolid="txtDuedate">
                                                                        </calendarextender>--%><asp:TextBox ID="txtDuedate" runat="server" Width="80px"></asp:TextBox>
                                                                        <cc1:CalendarExtender ID="txtDuedate_CalendarExtender" runat="server" 
                                                                            Enabled="True" PopupButtonID="imgDuedate" PopupPosition="BottomRight" 
                                                                            TargetControlID="txtDuedate">
                                                                        </cc1:CalendarExtender>
                                                                        <asp:ImageButton ID="imgDuedate" runat="server" 
                                                                            ImageUrl="~/images/Calendar_scheduleHS.png" />
                                                                    </td>
                                                                </tr>
                                                            </table>
                                                        </td>
                                                    </tr>
                                                </table>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Amount* :
                                            </td>
                                            <td class="style1">
                                                <asp:TextBox ID="txtQuan" runat="server"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td valign="top">
                                                Remark :
                                            </td>
                                            <td class="style1">
                                                <asp:TextBox ID="txtRemark" runat="server" Height="80px" TextMode="MultiLine" 
                                                    Width="400px"></asp:TextBox>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &#160;&#160;
                                            </td>
                                            <td class="style1">
                                                <asp:Button ID="btnSave" runat="server" Text="Save" style="height: 26px;width:100px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;&nbsp;
                                            </td>
                                            <td class="style1">
                                            </td>
                                        </tr>
                                    </table>
                                </div>
                                <div align="center">
                                </div>
                            </ContentTemplate>
                        </asp:UpdatePanel>
                    </ContentTemplate>
                </cc1:TabPanel>
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
                                                    <asp:TemplateField Visible="False">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="linkUpdate" runat="server" OnClick="linkUpdate_Click">Update</asp:LinkButton>
                                                            <asp:LinkButton ID="linkSave" Visible="false" runat="server" OnClick="linkSave_Click">Save</asp:LinkButton>&nbsp;
                                                            <asp:LinkButton ID="linkCancel" Visible="false" runat="server" 
                                                                OnClick="linkCancel_Click">Cancel</asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgDel" runat="server" ImageUrl="~/images/i_del.png" OnClick="imgDel_Click" /></ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="20px" />
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
                                                    <asp:TemplateField Visible="False">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="linkUpdate" runat="server" OnClick="linkUpdate_Click">Update</asp:LinkButton>
                                                            <asp:LinkButton ID="linkSave" Visible="false" runat="server" OnClick="linkSave_Click">Save</asp:LinkButton>&nbsp;
                                                            <asp:LinkButton ID="linkCancel" Visible="false" runat="server" 
                                                                OnClick="linkCancel_Click">Cancel</asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgDel" runat="server" ImageUrl="~/images/i_del.png" OnClick="imgDel_Click" /></ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="20px" />
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
                <cc1:TabPanel ID="tpGold99" runat="server" HeaderText="Gold 99.99">
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
                                                    <asp:TemplateField Visible="False">
                                                        <ItemTemplate>
                                                            <asp:LinkButton ID="linkUpdate" runat="server" OnClick="linkUpdate_Click">Update</asp:LinkButton>
                                                            <asp:LinkButton ID="linkSave" Visible="false" runat="server" OnClick="linkSave_Click">Save</asp:LinkButton>&nbsp;
                                                            <asp:LinkButton ID="linkCancel" Visible="false" runat="server" 
                                                                OnClick="linkCancel_Click">Cancel</asp:LinkButton>
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgDel" runat="server" ImageUrl="~/images/i_del.png" OnClick="imgDel_Click" /></ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Width="20px" />
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
