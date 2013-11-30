<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false"
    CodeFile="stock_daily.aspx.vb" Inherits="stock_daily" Title="stock_daily" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
    function search()
    {
        $get('<%=btnSearchAdv.ClientId %>').click();
    }
    </script>

    <style type="text/css">
        .style2
        {
            width: 170px;
        }
        .style3
        {
            width: 211px;
        }
        .style4
        {
            width: 177px;
        }
        .style5
        {
            width: 306px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea">
        <div class="demoheading">
            Summary Report</div>
        <div>
            <div id="div_ticket">
                <asp:UpdatePanel ID="udpMain" runat="server">
                    <ContentTemplate>
                        <table border="0" width="950px">
                            <tr>
                                <td valign="top">
                                    <table border="0">
                                        <tr>
                                            <td>
                                                Date:
                                            </td>
                                           <td class="style2">
                                                <asp:TextBox ID="txtDate" runat="server" Width="90px" />
                                                <ajaxToolkit:CalendarExtender ID="txtDate_CalendarExtender" runat="server" Enabled="True"
                                                    PopupButtonID="imgCalDate1" PopupPosition="BottomRight" TargetControlID="txtDate">
                                                </ajaxToolkit:CalendarExtender>
                                                <asp:ImageButton ID="imgCalDate1" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                                &nbsp;
                                            </td>
                                            <td class="style3">
                                                Status :
                                                <asp:DropDownList ID="ddlStatus" runat="server">
                                                    <asp:ListItem Value="">-- All --</asp:ListItem>
                                                    <asp:ListItem Value="901">Complete ส่งมอบ</asp:ListItem>
                                                    <asp:ListItem Value="902">Complete ฝาก</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                            <td class="style5">
                                                <%--<asp:RadioButtonList ID="rdoMode" runat="server" RepeatDirection="Horizontal" 
                                                    Width="260px">
                                                    <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                                                    <asp:ListItem Value="ticket">Ticket</asp:ListItem>
                                                    <asp:ListItem Value="dep">ฝาก/ถอน</asp:ListItem>
                                                    <asp:ListItem Value="inout">In/Out</asp:ListItem>
                                                </asp:RadioButtonList>--%>
                                                <asp:CheckBoxList ID="cbMode" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Selected="True" Value="ticket">Ticket</asp:ListItem>
                                                    <asp:ListItem Selected="True" Value="dep">ฝาก/ถอน</asp:ListItem>
                                                    <asp:ListItem Selected="True" Value="inout">In/Out</asp:ListItem>
                                                </asp:CheckBoxList>
                                                 <asp:RadioButtonList ID="rdoPurity" runat="server" RepeatDirection="Horizontal" 
                                                    Width="260px">
                                                    <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                                                    <asp:ListItem Value="96">96.50</asp:ListItem>
                                                    <asp:ListItem Value="96G">96.50(กรัม)</asp:ListItem>
                                                    <asp:ListItem Value="99">99.99</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                            <td class="style4">
                                                <asp:UpdateProgress ID="upgSearch" runat="server">
                                                    <ProgressTemplate>
                                                        <div align="left" style="margin-left: 1cm; overflow: auto">
                                                            <img alt="" src="images/loading.gif" />
                                                            Loading...
                                                        </div>
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
                                            </td>
                                            <td>
                                            
                                              <div style="display: none">
                                                    <asp:Button ID="btnSearchAdv" runat="server" Text="Search" />
                                                </div></td>
                                        </tr>
                                        
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div><div style="margin-bottom:5px">
                    <asp:LinkButton ID="linkExport" runat="server">Export</asp:LinkButton></div>
                    <asp:UpdatePanel ID="upnMain" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="gvTicket" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                Width="100%" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                CellPadding="3" DataSourceID="objSrcTicket" DataKeyNames="ticket_id" ShowFooter="True">
                                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                <Columns>
                                    <asp:BoundField Visible="True" />
                                    <asp:BoundField DataField="ticket_date" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="ticket_ref">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="link" runat="server" Target="_blank"><%#Eval("ticket_id")%></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="linkCust" runat="server" Target="_blank"><%#Eval("firstname")%></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="price"  DataFormatString="{0:#,##0}" HeaderText="Price">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="sell_99" HeaderText="99.99" DataFormatString="{0:#,##0}">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="sell_96" HeaderText="96.50" DataFormatString="{0:#,##0}">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="sell_96G" HeaderText="96.50G" DataFormatString="{0:#,##0}">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="buy_99" HeaderText="99.99" DataFormatString="{0:#,##0}">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="buy_96" HeaderText="96.50" DataFormatString="{0:#,##0}">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="buy_96G" HeaderText="96.50G" DataFormatString="{0:#,##0}">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="amount" HeaderText="Amount" DataFormatString="{0:#,##0}">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="sell_cash" HeaderText="Cash" DataFormatString="{0:#,##0}">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="sell_trans" HeaderText="Trans" DataFormatString="{0:#,##0}">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="sell_cheq" HeaderText="Cheq" DataFormatString="{0:#,##0}">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="buy_cash" HeaderText="Cash" DataFormatString="{0:#,##0}">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="buy_trans" HeaderText="Trans" DataFormatString="{0:#,##0}">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="buy_cheq" HeaderText="Cheq" DataFormatString="{0:#,##0}">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                </Columns>
                                <FooterStyle BackColor="Gray" ForeColor="Black" Font-Bold="True" />
                                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                <SelectedRowStyle Font-Bold="False" />
                                <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                                <AlternatingRowStyle BackColor="Gainsboro" />
                            </asp:GridView>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <asp:ObjectDataSource ID="objSrcTicket" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="getStockDialy" TypeName="clsDB">
                    <SelectParameters>
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="updateDate" Type="String" />
                        <asp:ControlParameter ControlID="ddlStatus" ConvertEmptyStringToNull="False" 
                            Name="status_id" PropertyName="SelectedValue" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="mode" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="putity" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </div>
    </div>
    <div class="demobottom">
    </div>
</asp:Content>
