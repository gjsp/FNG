<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false"
    CodeFile="stock_tickets.aspx.vb" Inherits="stock_tickets" Title="Stock Tickets" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" language="javascript">

        function updateStatus_id(refno, status_id, quan, amount, goldtype, type, payment, billing, user_id) {
            if (goldtype != "G96" && goldtype != "SIL") {
                quan = quan.toFixed(5);
            }
            if (type == "sell") {
                quan = quan * -1;
            }
            else {
                amount = amount * -1;
            }
            PageMethods.updateStatus(refno, status_id.options[status_id.selectedIndex].value, quan, amount, payment, billing, user_id, goldtype, OnComplete);

        }
        function cancelStatus_complete(ddl) {
            alert('No Chanage Status Complete.');
            //ddl.options[ddl.selectedIndex].value = '999';
            ddl.selectedIndex = 3;

        }
        function OnComplete(result) {
            if (result != 0) {
                alert('Update Complete');
            }
        }

    </script>
    <style type="text/css">
        .style1
        {
            height: 32px;
        }
        .style2
        {
            height: 3px;
        }
        
        </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea">
        <div class="demoheading">
            Stock</div>
        <div>
            <%#Eval("ticket_id")%>
            <asp:UpdatePanel ID="udpMain" runat="server">
                <ContentTemplate>
                    <asp:Panel ID="pnMain" runat="server" DefaultButton="btnSearchAdv">
                        <table border="0">
                            <tr>
                                <td valign="top">
                                    <table border="0">
                                        <tr>
                                            <td>
                                                Ticket Ref :
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
                                                <asp:TextBox ID="txtCustName" runat="server" />
                                                &nbsp;<asp:ImageButton ID="imgSearchCustRef" runat="server" ImageUrl="~/images/search.bmp"
                                                    Style="width: 12px" Visible="False" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Type :
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rdoType" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Selected="True" Value="">view all</asp:ListItem>
                                                    <asp:ListItem Value="Sell">Sell</asp:ListItem>
                                                    <asp:ListItem Value="Buy">Buy</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Status :
                                            </td>
                                            <td>
                                                <asp:DropDownList ID="ddlStatus" runat="server">
                                                </asp:DropDownList>
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
                                                Billing :
                                            </td>
                                            <td>
                                                <asp:RadioButtonList ID="rdoBilling" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Selected="True" Value="">view all</asp:ListItem>
                                                    <asp:ListItem Value="y">Yes</asp:ListItem>
                                                    <asp:ListItem Value="n">No</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style2">
                                                Call/Online :
                                            </td>
                                            <td class="style2">
                                                <asp:RadioButtonList ID="rdoCenter" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Selected="True" Value="">View All</asp:ListItem>
                                                    <asp:ListItem Value="y">Call</asp:ListItem>
                                                    <asp:ListItem Value="n">Online</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style1">
                                                <asp:Button ID="btnSearchAdv" runat="server" Text="Search" />
                                            </td>
                                            <td class="style1">
                                                <asp:UpdateProgress ID="upgSearch" runat="server">
                                                    <ProgressTemplate>
                                                        <div align="left" style="margin-left: 1cm; overflow: auto">
                                                            <img alt="" src="images/loading.gif" />
                                                            Loading...
                                                        </div>
                                                    </ProgressTemplate>
                                                </asp:UpdateProgress>
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
                    </asp:Panel>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:UpdatePanel ID="upnMain" runat="server">
                <Triggers>
                    <asp:PostBackTrigger ControlID="linkExcel" />
                </Triggers>
                <ContentTemplate>
                    <div>
                        <table style="width: 100%">
                            <tr>
                                <td align="left">
                                    <asp:LinkButton ID="linkExcel" runat="server">Export</asp:LinkButton>
                                </td>
                                <td align="right">
                                    <asp:LinkButton ID="linkReviewDoc" runat="server">PreviewDoc</asp:LinkButton>
                                    &nbsp;&nbsp;
                                    <asp:LinkButton ID="linkRptSell" runat="server">Receipt</asp:LinkButton>
                                </td>
                            </tr>
                        </table>
                        <asp:GridView ID="gvTicket" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                            Width="100%" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                            CellPadding="3" DataSourceID="objSrcTicket" GridLines="Vertical" DataKeyNames="ticket_id,cust_id,type,billing,gold_type_id,quantity,amount,gold_dep,price,payment,trade,run_no"
                            ShowFooter="True">
                            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                            <Columns>
                                <asp:TemplateField>
                                    <ItemTemplate>
                                        <asp:ImageButton ID="imgSplit" runat="server" ImageUrl="~/images/bill_icon.gif" OnClick="imgSplit_Click" />
                                    </ItemTemplate>
                                    <ItemStyle Width="20px" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Date" SortExpression="ticket_date" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDate" runat="server" Text='<%# Bind("ticket_date", "{0:d/M/yyyy}") %>'></asp:Label>
                                    </ItemTemplate>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Ticket_ref" SortExpression="ticket_id">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="link" runat="server" Target="_blank"><%#Eval("ticket_id")%></asp:HyperLink>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Customer_Name" SortExpression="Firstname">
                                    <ItemTemplate>
                                        <asp:HyperLink ID="linkCust" runat="server" Target="_blank"><%#Eval("firstname")%></asp:HyperLink>
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Left" Wrap="False" />
                                </asp:TemplateField>
                                <asp:BoundField DataField="gold_type_name" HeaderText="Gold_type" SortExpression=" gold_type_name"
                                    Visible="False" />
                                <asp:BoundField DataField="type" HeaderText="Type" SortExpression="type">
                                    <ItemStyle HorizontalAlign="Center" />
                                </asp:BoundField>
                                <asp:BoundField DataField="payment" HeaderText="Payment" SortExpression="payment" />
                                <asp:BoundField DataField="delivery_date" HeaderText="Delivery_date" SortExpression="delivery_date"
                                    DataFormatString="{0:d/M/yyyy}" />
                                <asp:BoundField DataField="user_name" HeaderText="Created_by" SortExpression="user_name" />
                                <asp:BoundField DataField="modifier_by" HeaderText="Update_by" SortExpression="modifier_by" />
                                <asp:BoundField DataField="quan96G" DataFormatString="{0:#,##0.000}" HeaderText="Q96(กรัม)"
                                    SortExpression="quan96G">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="quan96" DataFormatString="{0:#,##0}" HeaderText="Q96(บาท)"
                                    SortExpression="quan96">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="quan99" HeaderText="Q99(kg)" SortExpression="quan99" DataFormatString="{0:#,##0}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="price" HeaderText="Price(baht)" SortExpression="price"
                                    DataFormatString="{0:#,##0}">
                                    <ItemStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:BoundField DataField="amount" HeaderText="Amount(baht)" SortExpression="amount"
                                    DataFormatString="{0:#,##0.00}">
                                    <ItemStyle HorizontalAlign="Right" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="Receipt">
                                    <ItemTemplate>
                                        <asp:Label ID="lblReceipt" runat="server" Text='<%# Bind("run_no") %>'></asp:Label>
                                        &nbsp;
                                        <asp:ImageButton ID="imgDelReceipt" runat="server"  
                                            ImageUrl="~/images/i_del.png" onclick="imgDelReceipt_Click" Height="12" Width="12" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Billing" ItemStyle-Wrap="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBill" runat="server"></asp:Label>
                                        <asp:DropDownList ID="ddlBill" runat="server" Visible="false">
                                            <asp:ListItem Selected="True" Value="">--None--</asp:ListItem>
                                            <asp:ListItem Value="y">Yes</asp:ListItem>
                                            <asp:ListItem Value="n">No</asp:ListItem>
                                        </asp:DropDownList>
                                        <asp:LinkButton ID="linkBillEdit" runat="server" OnClick="linkBillEdit_Click">None</asp:LinkButton>
                                        <asp:LinkButton ID="linkBillSave" runat="server" OnClick="linkBillSave_Click" Visible="False">Save</asp:LinkButton>
                                        <asp:LinkButton ID="linkBillCancel" runat="server" Visible="False" OnClick="linkBillCancel_Click">Cancel</asp:LinkButton>
                                        <asp:HiddenField ID="hdfBill" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="Status" SortExpression="status_id">
                                    <ItemTemplate>
                                        <select id="ddlBuy" runat="server" style="width: 140px">
                                            <option value="101" style="color: Black">Pending</option>
                                            <option value="105" style="color: #0099CC">เงินออก รอทอง</option>
                                            <option value="901" style="color: Green">Complete ส่งมอบ</option>
                                            <option value="903" style="color: #333333">Complete ตัดทองฝาก</option>
                                        </select>
                                        <select id="ddlSell" runat="server" style="width: 140px">
                                            <option value="101" style="color: Black">Pending</option>
                                            <option value="104" style="color: #9966FF">ทองออก รอเงิน</option>
                                            <option value="901" style="color: Green">Complete ส่งมอบ</option>
                                            <option value="902" style="color: #663300">Complete ฝาก</option>
                                        </select>
                                        <asp:LinkButton ID="linkUpdate" runat="server" OnClick="linkUpdate_Click">Save</asp:LinkButton>
                                        <asp:HiddenField ID="hdfBefore_status" runat="server" />
                                    </ItemTemplate>
                                    <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                </asp:TemplateField>
                                <asp:TemplateField>
                                    <HeaderTemplate>
                                        <input id="cbHead" runat="server" onclick="switchCheckbox(this);" type="checkbox" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <input id="cbRow" runat="server" type="checkbox" />
                                    </ItemTemplate>
                                    <HeaderStyle HorizontalAlign="Center" />
                                    <ItemStyle HorizontalAlign="Center" Width="20px" />
                                </asp:TemplateField>
                            </Columns>
                            <FooterStyle BackColor="Gray" ForeColor="Black" Font-Bold="True" />
                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                            <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                            <AlternatingRowStyle BackColor="Gainsboro" />
                        </asp:GridView>
                    </div>
                    <asp:ObjectDataSource ID="objSrcTicket" runat="server" OldValuesParameterFormatString="original_{0}"
                        SelectMethod="getTicketStock" TypeName="clsDB" >
                        <SelectParameters>
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="ticket_id" Type="String" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="cust_name" Type="String" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="type" Type="String" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="billing" Type="String" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="status_id" Type="String" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="ticket_date1" Type="String" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="ticket_date2" Type="String" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="del_date1" Type="String" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="del_date2" Type="String" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="isCenter" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    <div class="demobottom">
    </div>
</asp:Content>
