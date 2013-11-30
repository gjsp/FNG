<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false"
    CodeFile="ticket_list.aspx.vb" Inherits="ticket_list" Title="Ticket List" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script type="text/javascript" language="javascript">
//    function updateStatus_id(refno,status_id,quan,amount,goldtype,type)    
//    {
//        if(goldtype != "G96" && goldtype != "SIL")
//        {
//          quan = ((quan * 656)/10).toFixed(3);
//        }
//        if(type == "sell")
//        {
//            quan = quan * -1;
//        }
//        else 
//        {
//            amount  = amount * -1;
//        }
//        PageMethods.updateStatus(refno,status_id.options[status_id.selectedIndex].value,quan,amount,OnComplete);
//      
//    }
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

    <style type="text/css">
        .style2
        {
            height: 10px;
        }
        .style3
        {
            height: 9px;
        }
        .style4
        {
            height: 18px;
        }
    </style>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea">
        <div class="demoheading">
            Tickets</div>
        <div>
        <asp:LinkButton ID="linkExport" runat="server">Export</asp:LinkButton>
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
                                                Customer Name :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCustname" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Amount :</td>
                                            <td>
                                                <asp:TextBox ID="txtAmount" runat="server" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Status :</td>
                                            <td>
                                                <asp:DropDownList ID="ddl_status" runat="server">
                                                </asp:DropDownList>
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
                                            <td class="style2">
                                                Book No :</td>
                                            <td class="style2">
                                                <asp:TextBox ID="txtBookNo" runat="server" Width="80px" />
                                                &nbsp;&nbsp;&nbsp; No :
                                                <asp:TextBox ID="txtNo" runat="server" Width="80px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style3" valign="top">
                                                gold Type :</td>
                                            <td class="style3">
                                                <asp:DropDownList ID="ddl_goldtype" runat="server">
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style4">
                                                &nbsp;
                                                <asp:Button ID="btnSearchAdv" runat="server" Text="Search" />
                                            </td>
                                            <td align="right" class="style4">
                                                &nbsp;
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                &nbsp;</td>
                                            <td align="right">
                                                &nbsp;</td>
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
                            <br />
                            
                            <asp:GridView ID="gvTicket" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                Width="100%" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                CellPadding="3" DataSourceID="objSrcTicket" GridLines="Vertical" 
                                DataKeyNames="ticket_id">
                                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                <Columns>
                                    <asp:CommandField ButtonType="Image" SelectImageUrl="~/images/i_detail.png" ShowSelectButton="True" />
                                    <asp:BoundField DataField="ticket_date" HeaderText="Date" SortExpression="ticket_date"
                                        DataFormatString="{0:dd/MM/yyyy}" />
                                    <asp:BoundField DataField="ticket_id" HeaderText="Ticket_Ref_No" SortExpression="ticket_id" />
                                    <asp:BoundField DataField="book_no" HeaderText="book_no" SortExpression="book_no" />
                                    <asp:BoundField DataField="run_no" HeaderText="No" SortExpression="run_no" />
                                    <asp:BoundField DataField="firstname" HeaderText="Customer_Name" SortExpression="firstname" />
                                    <asp:BoundField DataField="gold_type_name" HeaderText="gold_type" 
                                        SortExpression=" gold_type_name" Visible="False" />
                                    <asp:BoundField DataField="payment" HeaderText="payment" SortExpression="payment" />
                                    <asp:BoundField DataField="delivery_date" HeaderText="Delivery_date" SortExpression="delivery_date"
                                        DataFormatString="{0:d}" />
                                    <asp:BoundField DataField="user_name" HeaderText="Created_by" SortExpression="user_name" />
                                    <asp:BoundField DataField="quantity" HeaderText="Quantity" SortExpression="quantity"
                                        DataFormatString="{0:#,##0.00000}" Visible="False">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="price" HeaderText="price" SortExpression="price" 
                                        DataFormatString="{0:#,##0.00}" Visible="False">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="amount" HeaderText="amount" SortExpression="amount" DataFormatString="{0:#,##0.00}">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="type" HeaderText="Type" SortExpression="type" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="Billing" SortExpression="billing">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbBill" runat="server" Enabled="False" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Justify" Width="15px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="status_name" HeaderText="Status" 
                                        SortExpression="status_name" >
                                        <ItemStyle HorizontalAlign="Center" />
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
                    SelectMethod="getTicketList" TypeName="clsDB">
                    <SelectParameters>
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="ticket_id" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="book_no" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="run_no" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="cust_name" 
                            Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="type" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="billing" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="status_id" 
                            Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="gold_type_id" 
                            Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="ticket_date1" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="ticket_date2" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="del_date1" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="del_date2" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="created_by" 
                            Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="amount" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </div>
    </div>
    <div class="demobottom">
    </div>
</asp:Content>
