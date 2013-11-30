<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false" CodeFile="ticket_view_stock.aspx.vb" Inherits="ticket_view_stock" title="Inter Bank Stock" %>


<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
    function updateStatus_id(refno,status_id,quan,amount,goldtype,type)    
    {
        if(goldtype != "G96" && goldtype != "SIL")
        {
          quan = ((quan * 656)/10).toFixed(3);
        }
        if(type == "sell")
        {
            quan = quan * -1;
        }
        else 
        {
            amount  = amount * -1;
        }
        PageMethods.updateStatus(refno,status_id.options[status_id.selectedIndex].value,quan,amount,OnComplete);
      
    }
    function cancelStatus_complete(ddl)
    {
        alert('No Chanage Status Complete.');
        //ddl.options[ddl.selectedIndex].value = '999';
        ddl.selectedIndex = 3;
        
    }
    function OnComplete(result)
    {
        if(result!=0)
        {
            alert('Update Complete');
        }
    }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea">
        <div class="demoheading">
            Stock Report</div>
        <div>
            <iframe src="stock_now.aspx" style="height:300px; width: 100%" frameborder="0" scrolling="no"
                marginheight="0" marginwidth="0"></iframe>
            <div id="div_ticket">
                <asp:UpdatePanel ID="udpMain" runat="server">
                    <ContentTemplate>
                        <table border="0">
                            <tr>
                                <td valign="top">
                                    <table border="0">
                                        <tr>
                                            <td>
                                                Ticket Ref No
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtTicketRef" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Customer Ref No :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCustRef" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Type :
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rdoType" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="Sell" Selected="True">Sell</asp:ListItem>
                                                    <asp:ListItem Value="Buy">Buy</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="right">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    &nbsp;&nbsp;&nbsp;&nbsp;
                                </td>
                                <td valign="top">
                                    <table border="0">
                                        <tr>
                                            <td>
                                                Date :
                                            </td>
                                            <td>
                                                From
                                                <asp:TextBox ID="txtDate1" runat="server" Width="70px" />
                                                <ajaxToolkit:CalendarExtender ID="txtDate1_CalendarExtender" runat="server" Enabled="True"
                                                    PopupPosition="BottomRight" TargetControlID="txtDate1" PopupButtonID="imgCalDate1">
                                                </ajaxToolkit:CalendarExtender>
                                                <asp:ImageButton ID="imgCalDate1" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                                &nbsp; To
                                                <asp:TextBox ID="txtDate2" runat="server" Width="70px" />
                                                <asp:ImageButton ID="imgCalDate2" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                                <ajaxToolkit:CalendarExtender ID="txtDate2_CalendarExtender" runat="server" Enabled="True"
                                                    PopupButtonID="imgCalDate2" PopupPosition="BottomRight" TargetControlID="txtDate2">
                                                </ajaxToolkit:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Delivery Date :
                                            </td>
                                            <td>
                                                From
                                                <asp:TextBox ID="txtDelDate1" runat="server" Width="70px" />
                                                <ajaxToolkit:CalendarExtender ID="txtDelDate1_CalendarExtender" runat="server" Enabled="True"
                                                    PopupPosition="BottomRight" PopupButtonID="imgCalDelDate1" TargetControlID="txtDelDate1">
                                                </ajaxToolkit:CalendarExtender>
                                                <asp:ImageButton ID="imgCalDelDate1" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                                &nbsp; To
                                                <asp:TextBox ID="txtDelDate2" runat="server" Width="70px" />
                                                <asp:ImageButton ID="imgCalDelDate2" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                                <ajaxToolkit:CalendarExtender ID="txtDelDate2_CalendarExtender" runat="server" Enabled="True"
                                                    PopupButtonID="imgCalDelDate2" PopupPosition="BottomRight" TargetControlID="txtDelDate2">
                                                </ajaxToolkit:CalendarExtender>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Billing
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rdoBilling" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Value="y" Selected="True">Yes</asp:ListItem>
                                                    <asp:ListItem Value="n">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                                <asp:Button ID="btnSearchAdv" runat="server" Text="Search" />
                                            </td>
                                            <td>
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;
                                            </td>
                                            <td align="right">
                                                &nbsp;
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:UpdatePanel ID="upnMain" runat="server">
                    <ContentTemplate>
                        <div>
                        <%--<div align="right" style="padding-bottom: 5px">
                             <asp:LinkButton ID="linkExport" runat="server">Export</asp:LinkButton>
                        </div>--%>
                            <asp:GridView ID="gvTicket" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                Width="100%" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                CellPadding="3" DataSourceID="objSrcTicket" GridLines="Vertical" DataKeyNames="ticket_id"
                                ShowFooter="True">
                                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                <Columns>
                                    <asp:BoundField DataField="ticket_date" HeaderText="Date" SortExpression="ticket_date"
                                        DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="ticket_id" HeaderText="Ticket_Ref_No" SortExpression="ticket_id" />
                                    <asp:BoundField DataField="cust_id" HeaderText="Cust_Ref_No" SortExpression="cust_id" />
                                    <asp:BoundField DataField="gold_type_name" HeaderText="gold_type" SortExpression=" gold_type_name" />
                                    <asp:BoundField DataField="payment" HeaderText="payment" SortExpression="payment" />
                                      
                                    <asp:BoundField DataField="user_name" HeaderText="Created_by" 
                                    SortExpression="user_name" />
                                    <asp:BoundField DataField="quantity" HeaderText="Quantity" SortExpression="quantity"
                                        DataFormatString="{0:#,##0.000}">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="price" HeaderText="price" SortExpression="price" DataFormatString="{0:#,##0.000}">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="amount" HeaderText="amount" SortExpression="amount" DataFormatString="{0:#,##0.000}">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="payment_date" HeaderText="payment_date" 
                                        SortExpression="stock_money" DataFormatString="{0:dd/MM/yy HH:mm}" />
                                    <asp:BoundField DataField="stock_gold" HeaderText="stock_gold" 
                                        SortExpression="stock_gold" >
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="stock_money" HeaderText="stock_money" 
                                        SortExpression="stock_money" >
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                </Columns>
                                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" Font-Bold="True" />
                                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                                <AlternatingRowStyle BackColor="Gainsboro" />
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:ObjectDataSource ID="objSrcTicket" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="getTicketViewStock" TypeName="clsDB">
                    <SelectParameters>
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="ticket_id" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="cust_id" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="type" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="billing" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="ticket_date1" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="ticket_date2" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="del_date1" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="del_date2" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </div>
    </div>
    <div class="demobottom">
    </div>
</asp:Content>
