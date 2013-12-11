<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false" EnableEventValidation="false"
    CodeFile="ticket_daily.aspx.vb" Inherits="ticket_daily" Title="ticket_daily" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript" language="javascript">
        function pageLoad() {
            setTimeout("autoRefresh();", 3000);
        }

        function autoRefresh() {
            
            if ($get('<%=cbRefresh.ClientID%>').checked == true) {
                $get('<%=btnSearchAdv.ClientId %>').click();
            }
        }

        function search() {
            $get('<%=btnSearchAdv.ClientId %>').click();
        }
    </script>
    <script type="text/javascript">

        //  keeps track of the delete button for the row
        //  that is going to be removed
        var _source;
        // keep track of the popup div
        var _popup;

        function showConfirm(source) {
            this._source = source;
            this._popup = $find('mppDel');

            //  find the confirm ModalPopup and show it    
            this._popup.show();
            $get('<%=txtPwd.ClientId %>').value = '';
            $get('<%=txtPwd.ClientId %>').focus();
        }

        function okClick() {
            if ($get('<%=txtPwd.ClientId %>').value == '') { $get('<%=txtPwd.ClientId %>').focus(); return };
            if ($get('<%=txtPwd.ClientId %>').value != $get('<%=hdfPwd.ClientId %>').value) {
                $get('<%=txtPwd.ClientId %>').focus();
                alert('Password Invalid.');
            }
            else {
                //  find the confirm ModalPopup and hide it   
                $get('<%=txtPwd.ClientId %>').value = '';
                this._popup.hide();
                //  use the cached button as the postback source
                __doPostBack(this._source.name, '');
            }
        }

        function cancelClick() {
            //  find the confirm ModalPopup and hide it 
            this._popup.hide();
            //  clear the event source
            this._source = null;
            this._popup = null;
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
        .style5
        {
            width: 16px;
        }
        .button
        {
            border: solid 1px #c0c0c0;
            background-color: #eeeeee;
            font-family: verdana;
            font-size: 11px;
        }
        
        .modalBg
        {
            background-color: #cccccc;
            filter: alpha(opacity=80);
            opacity: 0.8;
        }
        
        .modalPanel
        {
            background-color: #ffffff;
            border-width: 3px;
            border-style: solid;
            border-color: Gray;
            padding: 3px;
            width: 320px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea">
        <div class="demoheading">
            Summary Order</div>
        <div>
            <div id="div_ticket">
                <asp:UpdatePanel ID="udpMain" runat="server">
                    <ContentTemplate>
                        <asp:Panel ID="pnMain" runat="server" DefaultButton="btnSearchAdv">
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
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Customer Name :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCustName" runat="server" />
                                                </td>
                                                <td>
                                                    <asp:ImageButton ID="imgSearchCustRef" runat="server" ImageUrl="~/images/search.bmp"
                                                        Style="width: 12px" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Price :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtPrice" runat="server" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Amount :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtAmount" runat="server" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Type :
                                                </td>
                                                <td>
                                                    <asp:RadioButtonList ID="rdoType" runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                                                        <asp:ListItem Value="Sell">Sell</asp:ListItem>
                                                        <asp:ListItem Value="Buy">Buy</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                    <td class="style5">
                                        &nbsp;
                                    </td>
                                    <td valign="top">
                                        <table border="0">
                                            <tr>
                                                <td>
                                                    Date* :
                                                </td>
                                                <td>
                                                    From
                                                    <asp:TextBox ID="txtDate" runat="server" Width="70px" />
                                                    <ajaxToolkit:CalendarExtender ID="txtDate_CalendarExtender" runat="server" Enabled="True"
                                                        PopupButtonID="imgCalDate1" PopupPosition="BottomRight" TargetControlID="txtDate">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <asp:ImageButton ID="imgCalDate1" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                                    <asp:DropDownList ID="ddl1Hour" runat="server">
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="ddl1Min" runat="server">
                                                    </asp:DropDownList>
                                                    &nbsp; To
                                                    <asp:TextBox ID="txtDate2" runat="server" Width="70px" />
                                                    <asp:ImageButton ID="imgCalDate2" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                                    <ajaxToolkit:CalendarExtender ID="txtDate2_CalendarExtender" runat="server" Enabled="True"
                                                        PopupButtonID="imgCalDate2" PopupPosition="BottomRight" TargetControlID="txtDate2">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <asp:DropDownList ID="ddl2Hour" runat="server">
                                                    </asp:DropDownList>
                                                    <asp:DropDownList ID="ddl2Min" runat="server">
                                                    </asp:DropDownList>
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
                                                    All/Self :
                                                </td>
                                                <td class="style2">
                                                    &nbsp;<asp:DropDownList ID="ddlSelf" runat="server">
                                                    </asp:DropDownList>
                                                    &nbsp;Active/Delete :<asp:DropDownList ID="ddlRecycle" runat="server">
                                                        <asp:ListItem Value="y">Active</asp:ListItem>
                                                        <asp:ListItem Value="n">Delete</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style3">
                                                    Call/Online :</td>
                                                <td class="style3">
                                                    <asp:RadioButtonList ID="rdoCenter" runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                                                        <asp:ListItem Value="y">Call</asp:ListItem>
                                                        <asp:ListItem Value="n">Trade Online</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                    <div style="display:none">
                                                    <asp:DropDownList ID="ddlTeam" runat="server">
                                                    </asp:DropDownList>
                                                    <asp:TextBox ID="txtBookNo" runat="server" Width="80px" />
                                                    <asp:TextBox ID="txtNo" runat="server" Width="80px" />
                                                    </div>
                                                    
                                                </td>
                                            </tr>
                                            <tr>
                                                <td class="style3">
                                                    Purity :
                                                </td>
                                                <td class="style3">
                                                    <asp:RadioButtonList ID="rdoGoldType" runat="server" RepeatDirection="Horizontal">
                                                        <asp:ListItem Selected="True" Value="">All</asp:ListItem>
                                                        <asp:ListItem Value="96">96.50</asp:ListItem>
                                                        <asp:ListItem Value="99">99.99</asp:ListItem>
                                                    </asp:RadioButtonList>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="3" valign="top">
                                        <table>
                                            <tr>
                                                <td>
                                        <asp:CheckBoxList ID="cblStatus" runat="server" RepeatDirection="Horizontal">
                                        </asp:CheckBoxList>
                                                    <td>
                                                        <div style="margin-left: 2cm">
                                                        <input type="checkbox" id="cbRefresh" onchange="autoRefresh()" runat="server" />Auto Refresh
                                                    </div>
                                                    </td>
                                            </tr>
                                        </table>
                                        <asp:HiddenField ID="hdfPwd" runat="server" /></td>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top">
                                        &nbsp;
                                    </td>
                                    <td class="style5">
                                        <asp:Button ID="btnSearchAdv" runat="server" Text="Search" />
                                    </td>
                                  
                                    <td valign="top">
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
                            </table>
                        </asp:Panel>
                    </ContentTemplate>
                </asp:UpdatePanel>
                <asp:LinkButton ID="linkExport" runat="server">Export</asp:LinkButton>
                &nbsp;
                <div>
                    <asp:UpdatePanel ID="upnMain" runat="server">
                        <ContentTemplate>
                            <asp:GridView ID="gvTicket" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                Width="100%" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                CellPadding="3" DataSourceID="objSrcTicket" DataKeyNames="ticket_id,type">
                                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                <Columns>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:ImageButton ID="imgDel" runat="server" ImageUrl="~/images/i_del.png" OnClick="imgDel_Click"
                                                OnClientClick="showConfirm(this); return false;" Height="16px" Width="16px" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="30px" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="ticket_date" HeaderText="Date" DataFormatString="{0:dd/MM/yyyy HH:mm}"
                                        SortExpression="ticket_date">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:TemplateField HeaderText="ticket_ref" SortExpression="ticket_id">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="link" runat="server" Target="_blank"><%#Eval("ticket_id")%></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:TemplateField>
                                   <%-- <asp:BoundField DataField="book_no" HeaderText="book_no" SortExpression="book_no">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="run_no" HeaderText="No" SortExpression="run_no">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>--%>
                                    <asp:TemplateField HeaderText="Name_of_Customer" SortExpression="firstname">
                                        <ItemTemplate>
                                            <asp:HyperLink ID="linkCust" runat="server" Target="_blank"><%#Eval("firstname")%></asp:HyperLink>
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="gold_type_name" HeaderText="Pure" Visible="True" SortExpression="gold_type_name">
                                        <ItemStyle HorizontalAlign="Center" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="buy_price" HeaderText="Price" DataFormatString="{0:#,##0}"
                                        SortExpression="buy_price">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="buy_quantity" HeaderText="Quan" DataFormatString="{0:#,##0}"
                                        SortExpression="buy_quantity">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="buy_amount" HeaderText="Amount" DataFormatString="{0:#,##0}"
                                        SortExpression="buy_amount">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="sell_price" HeaderText="Price" DataFormatString="{0:#,##0}"
                                        SortExpression="sell_price">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="sell_quantity" HeaderText="Quan" DataFormatString="{0:#,##0}"
                                        SortExpression="sell_quantity">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="sell_amount" HeaderText="Amount" DataFormatString="{0:#,##0}"
                                        SortExpression="sell_amount">
                                        <ItemStyle HorizontalAlign="Right" />
                                    </asp:BoundField>
                                </Columns>
                                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" Font-Bold="True" />
                                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                <SelectedRowStyle Font-Bold="False" />
                                <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                                <AlternatingRowStyle BackColor="Gainsboro" />
                            </asp:GridView>
                            <ajaxToolkit:ModalPopupExtender ID="mppDelete" BehaviorID="mppDel" runat="server"
                                TargetControlID="Panel1" PopupControlID="Panel1" OkControlID="btnOk" OnOkScript="okClick();"
                                CancelControlID="btnNo" OnCancelScript="cancelClick();" BackgroundCssClass="modalBackground">
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:Panel ID="Panel1" runat="server" Style="display: none" CssClass="modalPopup" DefaultButton="btnOk">
                                <asp:Panel ID="Panel2" runat="server" Style="cursor: move; background-color: #DDDDDD;
                                    border: solid 1px Gray; color: Black">
                                    <div style="text-align: center">
                                        <p>
                                            Please Enter Password.
                                        </p>
                                        <asp:TextBox ID="txtPwd" runat="server" TextMode="Password"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvPwd" ValidationGroup="mppPwd" ControlToValidate="txtPwd"
                                            runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                    </div>
                                </asp:Panel>
                                <div>
                                    <p style="text-align: center;">
                                        <asp:Button ID="btnOk" runat="server" ValidationGroup="mppPwd" Text="Yes" Width="50px" />
                                        <asp:Button ID="btnNo" runat="server" Text="No" Width="50px" />
                                    </p>
                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
                <asp:ObjectDataSource ID="objSrcTicket" runat="server" OldValuesParameterFormatString="original_{0}"
                    SelectMethod="getTicketDialy" TypeName="clsDB">
                    <SelectParameters>
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="ticket_id" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="book_no" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="run_no" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="cust_name" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="gold_type_id" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="ticket_date" Type="DateTime" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="ticket_date2" Type="DateTime" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="del_date1" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="del_date2" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="price" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="user_id" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="team_id" Type="String" />
                        <asp:ControlParameter ControlID="ddlRecycle" ConvertEmptyStringToNull="False" Name="active"
                            PropertyName="SelectedValue" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="status_id" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="amount" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="type" Type="String" />
                        <asp:Parameter ConvertEmptyStringToNull="False" Name="isCenter" Type="String" />
                    </SelectParameters>
                </asp:ObjectDataSource>
            </div>
        </div>
    </div>
    <div class="demobottom">
    </div>
</asp:Content>
