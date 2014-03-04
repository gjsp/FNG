<%@ Page Title="" Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false"
    CodeFile="guarantee.aspx.vb" Inherits="center_guarantee" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea">
        <div class="demoheading">
            Guarantee
        </div>
        <asp:UpdatePanel ID="upMain" runat="server">
            <ContentTemplate>
                <div>
                    <table cellpadding="4" cellspacing="4" border="0">
                        <tr>
                            <td>
                                อัตราส่วน :
                            </td>
                            <td>
                                <asp:RadioButtonList ID="rdoCash" runat="server" RepeatDirection="Horizontal">
                                    <asp:ListItem Value="0.7">1:700</asp:ListItem>
                                    <asp:ListItem Value="1">1:1000</asp:ListItem>
                                    <asp:ListItem Value="1.5">1:1500</asp:ListItem>
                                    <asp:ListItem Value="2">1:2000</asp:ListItem>
                                    <asp:ListItem Value="2.5">1:2500</asp:ListItem>
                                    <asp:ListItem Value="3">1:3000</asp:ListItem>
                                </asp:RadioButtonList>
                            </td>
                            <td>
                                <div style="margin-left: 20px">
                                    <asp:Button ID="btnSaveGrt" runat="server" Text="Save" Width="80px" /></div>
                            </td>
                             <td style="height: 30px">
                             <asp:UpdateProgress ID="upgSearch" runat="server">
                                    <ProgressTemplate>
                                        <div align="left" style="margin-left: 1cm; overflow: auto">
                                            <img alt="" src="../images/loading.gif" />
                                            Loading...
                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                            </td>
                        </tr>
                    </table>
                    <br />
                </div>
                <cc1:TabContainer ID="tp" runat="server" ActiveTabIndex="1" 
                    Width="600px">
                    <cc1:TabPanel runat="server" HeaderText="Gold Movement" ID="tpCash"><HeaderTemplate>หลักประกันเงินสด</HeaderTemplate><ContentTemplate><asp:GridView ID="gvCash" runat="server" AutoGenerateColumns="False" BackColor="White"
                                BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataSourceID="ldsCash"
                                GridLines="Vertical" Width="100%"><RowStyle BackColor="#EEEEEE" ForeColor="Black" /><Columns><asp:BoundField DataField="grt_value" HeaderText="เงินหลักประกัน" SortExpression="grt_value"
                                        ReadOnly="True" DataFormatString="{0:#,#.##}" ItemStyle-HorizontalAlign="center" /><asp:BoundField DataField="grt_trade96" HeaderText="ทอง96.50 ที่เทรดได้" ItemStyle-HorizontalAlign="center"
                                        SortExpression="grt_trade96" ReadOnly="True"  DataFormatString="{0:#,#.##}"/><asp:BoundField DataField="grt_trade99" HeaderText="ทอง99.99 ที่เทรดได้" ItemStyle-HorizontalAlign="center"
                                        SortExpression="grt_trade99" ReadOnly="True" /></Columns><FooterStyle BackColor="#CCCCCC" ForeColor="Black" /><PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" /><SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" /><HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" /><AlternatingRowStyle BackColor="Gainsboro" /></asp:GridView><asp:LinqDataSource ID="ldsCash" runat="server" ContextTypeName="dcDBDataContext"
                                EntityTypeName="" OrderBy="grt_value" TableName="v_grts" Where="grt_type == @grt_type"
                                Select="new (grt_value, grt_trade96, grt_trade99)"><WhereParameters><asp:Parameter DefaultValue="C" Name="grt_type" Type="Char" /></WhereParameters></asp:LinqDataSource></ContentTemplate></cc1:TabPanel>
                    <cc1:TabPanel ID="tp96" runat="server" HeaderText="หลักประกันทอง 96.50"><ContentTemplate><asp:GridView ID="gv96" runat="server" AutoGenerateColumns="False" BackColor="White"
                                BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataSourceID="ldsCash96"
                                GridLines="Vertical" Width="100%"><RowStyle BackColor="#EEEEEE" ForeColor="Black" /><Columns><asp:BoundField DataField="grt_value" HeaderText="ทองหลักประกัน" SortExpression="grt_value"
                                        ReadOnly="True" DataFormatString="{0:#,#.##}" ><ItemStyle HorizontalAlign="Center" /></asp:BoundField><asp:BoundField DataField="grt_trade96" HeaderText="ทอง96.50 ที่เทรดได้"
                                        SortExpression="grt_trade96" ReadOnly="True"  DataFormatString="{0:#,#.##}"><ItemStyle HorizontalAlign="Center" /></asp:BoundField><asp:BoundField DataField="grt_trade99" HeaderText="ทอง99.99 ที่เทรดได้"
                                        SortExpression="grt_trade99" ReadOnly="True" ><ItemStyle HorizontalAlign="Center" /></asp:BoundField></Columns><FooterStyle BackColor="#CCCCCC" ForeColor="Black" /><PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" /><SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" /><HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" /><AlternatingRowStyle BackColor="Gainsboro" /></asp:GridView><asp:LinqDataSource ID="ldsCash96" runat="server" ContextTypeName="dcDBDataContext"
                                EntityTypeName="" OrderBy="grt_value" TableName="v_grts" Where="grt_type == @grt_type"
                                Select="new (grt_value, grt_trade96, grt_trade99)"><WhereParameters><asp:Parameter DefaultValue="B" Name="grt_type" Type="Char" /></WhereParameters></asp:LinqDataSource></ContentTemplate></cc1:TabPanel>
                    <cc1:TabPanel ID="tp99" runat="server" HeaderText="หลักประกันทอง 99.99"><ContentTemplate><asp:GridView ID="gv99" runat="server" AutoGenerateColumns="False" BackColor="White"
                                BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataSourceID="ldsCash99"
                                GridLines="Vertical" Width="100%"><RowStyle BackColor="#EEEEEE" ForeColor="Black" /><Columns><asp:BoundField DataField="grt_value" HeaderText="ทองหลักประกัน" SortExpression="grt_value"
                                        ReadOnly="True" DataFormatString="{0:#,#.##}" ItemStyle-HorizontalAlign="center" /><asp:BoundField DataField="grt_trade96" HeaderText="ทอง96.50 ที่เทรดได้" ItemStyle-HorizontalAlign="center"
                                        SortExpression="grt_trade96" ReadOnly="True" DataFormatString="{0:#,#.##}" /><asp:BoundField DataField="grt_trade99" HeaderText="ทอง99.99 ที่เทรดได้" ItemStyle-HorizontalAlign="center"
                                        SortExpression="grt_trade99" ReadOnly="True" /></Columns><FooterStyle BackColor="#CCCCCC" ForeColor="Black" /><PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" /><SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" /><HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" /><AlternatingRowStyle BackColor="Gainsboro" /></asp:GridView><asp:LinqDataSource ID="ldsCash99" runat="server" ContextTypeName="dcDBDataContext"
                                EntityTypeName="" OrderBy="grt_value" TableName="v_grts" Where="grt_type == @grt_type"
                                Select="new (grt_value, grt_trade96, grt_trade99)"><WhereParameters><asp:Parameter DefaultValue="K" Name="grt_type" Type="Char" /></WhereParameters></asp:LinqDataSource></ContentTemplate></cc1:TabPanel>
                </cc1:TabContainer>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
