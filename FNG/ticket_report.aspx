<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false"
    CodeFile="ticket_report.aspx.vb" Inherits="ticket_report" Title="Ticket Report" %>

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

    <style type="text/css">
        .style2
        {
            height: 10px;
        }
        .style3
        {
            height: 9px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea">
        <div class="demoheading">
            Ticket Report</div>
        <div>
            <%--<%#Eval("ticket_id")%>--%> 
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
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Customer Name :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtCustName" runat="server" />
                                            </td>
                                            <td>
                                                <asp:ImageButton ID="imgSearchCustRef" runat="server" 
                                                    ImageUrl="~/images/search.bmp" Style="width: 12px" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Price :</td>
                                            <td>
                                                <asp:TextBox ID="txtPrice" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Amount :
                                            </td>
                                            <td>
                                                <asp:TextBox ID="txtAmount" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                        <tr>
                                            <td>
                                                Quantity :&nbsp;</td>
                                            <td>
                                                <asp:TextBox ID="txtQuantity" runat="server" />
                                            </td>
                                            <td>
                                                &nbsp;</td>
                                        </tr>
                                    </table>
                                </td>
                                <td>
                                    &nbsp;</td>
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
                                            <td class="style3">
                                                Purity :
                                            </td>
                                            <td class="style3">
                                                <%--<asp:CheckBoxList ID="cblPurity" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Selected="True" Value="96">96.50</asp:ListItem>
                                                    <asp:ListItem Selected="True" Value="99N">99.99N</asp:ListItem>
                                                    <asp:ListItem Selected="True" Value="99L">99.99L</asp:ListItem>
                                                </asp:CheckBoxList>--%>
                                                <asp:RadioButtonList ID="rdoPurity" runat="server" 
                                                    RepeatDirection="Horizontal">
                                                    <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                                                    <asp:ListItem Value="96G">96.50(กรัม)</asp:ListItem>
                                                    <asp:ListItem Value="96">96.50(บาท)</asp:ListItem>
                                                    <asp:ListItem Value="99">99.99</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style2">
                                                Type :
                                            </td>
                                            <td class="style2">
                                                <asp:RadioButtonList ID="rdoType" runat="server" RepeatDirection="Horizontal">
                                                    <asp:ListItem Selected="True" Value="">view all</asp:ListItem>
                                                    <asp:ListItem Value="Sell">Sell</asp:ListItem>
                                                    <asp:ListItem Value="Buy">Buy</asp:ListItem>
                                                </asp:RadioButtonList>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td class="style2">
                                                Active/Delete</td>
                                            <td class="style2">
                                               <%-- <asp:DropDownList ID="ddlTeam" runat="server">
                                                </asp:DropDownList>--%>
                                                &nbsp;
                                                <asp:DropDownList ID="ddlRecycle" runat="server">
                                                    <asp:ListItem Value="y">Active</asp:ListItem>
                                                    <asp:ListItem Value="n">Delete</asp:ListItem>
                                                </asp:DropDownList>
                                            </td>
                                        </tr>
                                    </table>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                    <asp:RadioButtonList ID="rdoCenter" runat="server" RepeatDirection="Horizontal">
                                        <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                                        <asp:ListItem Value="y">Call</asp:ListItem>
                                        <asp:ListItem Value="n">Online</asp:ListItem>
                                    </asp:RadioButtonList>
                                </td>
                                <td colspan="2">
                                    <asp:CheckBoxList ID="cblStatus" runat="server" RepeatDirection="Horizontal">
                                    </asp:CheckBoxList>
                                </td>
                            </tr>
                            <tr>
                                <td>
                                </td>
                                <td>
                                    <asp:Button ID="btnSearch" runat="server" Text="Search" />
                                </td>
                                <td>
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
                                    &nbsp; </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <div id="divExcel" runat="server" align="left" visible = "true" style="padding-bottom: 5px">
                            &nbsp;<asp:LinkButton ID="linkExcel" runat="server">Export</asp:LinkButton>
                        </div>
                <asp:UpdatePanel ID="upnMain" runat="server">
                    <ContentTemplate>
                        <div>
                            <asp:GridView ID="gvTicket" runat="server" AutoGenerateColumns="False" Width="100%"
                                BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                CellPadding="3" DataSourceID="objSrcTicket" DataKeyNames="ticket_id" AllowSorting="True">
                                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                <Columns>
                                    <asp:CommandField ButtonType="Image" SelectImageUrl="~/images/i_detail.png" ShowSelectButton="True"
                                        Visible="false">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:CommandField>
                                    <%--<asp:BoundField DataField="ticket_date" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy}"
                                        SortExpression="ticket_date">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>--%>
                                    <asp:TemplateField HeaderText="ticket_date" SortExpression="ticket_date">
                                        <ItemTemplate> 
                                            <asp:HyperLink ID="linkLog" runat="server" Target="_blank"></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    
                                    <asp:TemplateField HeaderText="ticket_ref" SortExpression="ticket_id">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="link" runat="server" Target="_blank" Text='<%#Eval("ticket_id")%>'></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Customer" SortExpression="firstname">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="linkCust" runat="server" Target="_blank" Text='<%#Eval("firstname")%>'></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="type" HeaderText="Sell/Buy" SortExpression="type" >
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="gold_type_id" HeaderText="Purity" SortExpression="gold_type_id">
                                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="price" HeaderText="Price" DataFormatString="{0:#,##0.00}"
                                        SortExpression="price">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="quan96G" HeaderText="Q96.50(กรัม)" DataFormatString="{0:#,##0.000}"
                                        SortExpression="quan96G">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="quan96" HeaderText="Q96.50(บาท)" DataFormatString="{0:#,##0}"
                                        SortExpression="quan96">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="quan99" DataFormatString="{0:#,##0}" HeaderText="Q99.99(kg)"
                                        SortExpression="quan99">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="amount" HeaderText="Amount" DataFormatString="{0:#,##0.00}"
                                        SortExpression="amount">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="delivery_date" HeaderText="Delivery_date" DataFormatString="{0:dd/MM/yyyy}"
                                        SortExpression="delivery_date">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="payment" HeaderText="Payment" SortExpression="payment">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <%--<asp:TemplateField HeaderText="Billing" SortExpression="Billing">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="cbBill" runat="server" Enabled="False" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="15px" />
                                    </asp:TemplateField>--%>
                                    <asp:BoundField DataField="Billing" HeaderText="Billing" SortExpression="Billing">
                                        <ItemStyle HorizontalAlign="Center" Width="15px" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="user_name" HeaderText="Created_by" SortExpression="user_name">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField HeaderText="Status" DataField="status_name" SortExpression="status_name">
                                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                    </asp:BoundField>
                                </Columns>
                                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" Font-Bold="True" />
                                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" 
                                    HorizontalAlign="Center" />
                                <AlternatingRowStyle BackColor="Gainsboro" />
                            </asp:GridView>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:ObjectDataSource ID="objSrcTicket" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="getTicketReport" TypeName="clsDB">
                    <SelectParameters>
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="ticket_id" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="cust_name" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="type" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="billing" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="status_id" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="gold_type_id" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="ticket_date1" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="ticket_date2" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="del_date1" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="del_date2" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="created_by" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="amount" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="price" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="quantity" Type="String" />
                        <asp:ControlParameter ControlID="ddlRecycle" Name="active" 
                            PropertyName="SelectedValue" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="isCenter" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </div>
    </div>
    <div class="demobottom">
    </div>
</asp:Content>
