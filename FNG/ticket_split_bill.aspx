<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="ticket_split_bill.aspx.vb" Inherits="ticket_split_bill" Title="Ticket Split Bill" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            width: 172px;
        }
        .style4
        {
            width: 172px;
            height: 18px;
        }
        .style5
        {
            width: 97px;
            height: 18px;
            text-align: right;
        }
        .style6
        {
            width: 97px;
            text-align: right;
        }
        .lbl
        {
            color: Black;
        }
        .style7
        {
            width: 98px;
            height: 18px;
            text-align: right;
        }
        .style8
        {
            width: 98px;
            text-align: right;
        }
    </style>
    <script language="javascript" type="text/javascript">
        //only page split bill change [4]-->[6]
        function switchCheckboxSplit(id) {
            var frm = document.getElementById('aspnetForm');
            for (i = 0; i < frm.elements.length; i++) {
                if (frm.elements[i].type == "checkbox") {
                    if (frm.elements[i].name.split('$')[6] == 'cbHead' || frm.elements[i].name.split('$')[6] == 'cbRow') {
                        frm.elements[i].checked = id.checked;
                    }
                }
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div id="div_splitbill" style="padding-left: 5px; display: block">
        <asp:UpdatePanel ID="upMain" runat="server">
            <ContentTemplate>
                <fieldset>
                    <legend class="topic">Ticket Detail</legend>
                    <table>
                        <tr>
                            <td class="style7">
                                &nbsp;
                            </td>
                            <td class="style4">
                                &nbsp;
                            </td>
                            <td class="style5">
                                &nbsp;
                            </td>
                            <td class="style4">
                                <asp:HiddenField ID="hdfType" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td class="style7">
                                Ref No :
                            </td>
                            <td class="style4">
                                <asp:Label ID="lblRefno" CssClass="lbl" runat="server" Text=""></asp:Label>
                            </td>
                            <td class="style5">
                                Purity&nbsp;:
                            </td>
                            <td class="style4">
                                <asp:Label ID="lblGoldType" CssClass="lbl" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="style8">
                                Type :
                            </td>
                            <td class="style1">
                                <asp:Label ID="lblOrderType" CssClass="lbl" runat="server"></asp:Label>
                            </td>
                            <td class="style6">
                                Price :
                            </td>
                            <td class="style1">
                                <asp:Label ID="lblPrice" CssClass="lbl" runat="server" Text=""></asp:Label>
                                &nbsp;baht
                            </td>
                        </tr>
                        <tr>
                            <td class="style8">
                                Quantity :
                            </td>
                            <td class="style1">
                                <asp:Label ID="lblQuan" CssClass="lbl" runat="server" Text=""></asp:Label>
                            </td>
                            <td class="style6">
                                Amount :
                            </td>
                            <td class="style1">
                                <asp:Label ID="lblAmount" CssClass="lbl" runat="server" Text=""></asp:Label>
                                &nbsp;baht
                            </td>
                        </tr>
                        <tr>
                            <td class="style8">
                                Quantity คงเหลือ :
                            </td>
                            <td class="style1">
                                <asp:Label ID="lblQuanBalance" runat="server" CssClass="lbl" Text=""></asp:Label>
                            </td>
                            <td class="style6">
                                Amount คงเหลือ :
                            </td>
                            <td class="style1">
                                <asp:Label ID="lblAmountBalance" runat="server" CssClass="lbl" Text=""></asp:Label>
                                &nbsp;baht
                            </td>
                        </tr>
                    </table>
                </fieldset>
                <ajaxToolkit:TabContainer ID="tabTicket" runat="server" ActiveTabIndex="0">
                    <ajaxToolkit:TabPanel runat="server" HeaderText="Cash" ID="tpCash" Visible="false">
                        <ContentTemplate>
                            <asp:UpdatePanel ID="udpCash" runat="server">
                                <ContentTemplate>
                                    <div style="width: 1100px">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Amount* :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtQuanCash" runat="server" Width="100px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    Payment :
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlpaymentCash" runat="server">
                                                        <asp:ListItem>cash</asp:ListItem>
                                                        <asp:ListItem>cheq</asp:ListItem>
                                                        <asp:ListItem Value="trans">payin</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    Status :
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlCashStatus" runat="server">
                                                        <asp:ListItem Value="901">Complete ส่งมอบ</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    Bank :
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlBankCash" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    Cheque no :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCheqCash" runat="server" Width="80px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    Due date :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDuedateCash" runat="server" Width="70px"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="txtDuedateCash_CalendarExtender" runat="server"
                                                        Enabled="True" PopupButtonID="imgCalDateDeliveryCash" PopupPosition="BottomRight"
                                                        TargetControlID="txtDuedateCash">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <asp:ImageButton ID="imgCalDateDeliveryCash" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
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
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnAddCash" runat="server" Text="Add" Width="60px" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
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
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="12">
                                                    <asp:GridView ID="gvCash" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataKeyNames="ref_no,ticket_sp_id"
                                                        GridLines="Vertical" Width="99%" ShowFooter="True">
                                                        <AlternatingRowStyle BackColor="Gainsboro" />
                                                        <Columns>
                                                            <asp:BoundField HeaderText="No.">
                                                                <ItemStyle HorizontalAlign="Center" Width="20px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="payment" HeaderText="payment" SortExpression="payment">
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="amount" DataFormatString="{0:#,##0.00}" HeaderText="Amount(baht)"
                                                                SortExpression="amount">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="bank_name" HeaderText="Bank">
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Cheque_No" DataField="payment_cheq_no">
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="payment_duedate" HeaderText="Duedate_Cheque" DataFormatString="{0:d/M/yyyy}">
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="status_id" HeaderText="Status">
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField HeaderText="Status" SortExpression="status_id" Visible="False">
                                                                <ItemTemplate>
                                                                    <select id="ddlBuy" runat="server" style="width: 130px">
                                                                        <option value="901" style="color: Green">Complete ส่งมอบ</option>
                                                                    </select>
                                                                    <select id="ddlSell" runat="server" style="width: 120px">
                                                                        <option value="901" style="color: Green">Complete ส่งมอบ</option>
                                                                    </select>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField HeaderText="ฝาก/ถอน" Visible="False">
                                                                <ItemTemplate>
                                                                    &nbsp;<asp:Label ID="lblDep" runat="server" Text="Label"></asp:Label>
                                                                    <select id="ddlDep" runat="server" name="D1">
                                                                        <option style="color: Green" value="dep">ฝาก</option>
                                                                        <option style="color: red" value="wit">ถอน</option>
                                                                    </select>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField Visible="False">
                                                                <ItemTemplate>
                                                                    <asp:LinkButton ID="linkUpdate" runat="server" OnClick="linkUpdate_Click">ฝาก/ถอน</asp:LinkButton>
                                                                    <asp:LinkButton ID="linkSave" Visible="false" runat="server" OnClick="linkSave_Click">Save</asp:LinkButton>
                                                                    &nbsp;
                                                                    <asp:LinkButton ID="linkCancel" Visible="false" runat="server" OnClick="linkCancel_Click">Cancel</asp:LinkButton>
                                                                </ItemTemplate>
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgDelCash" runat="server" ImageUrl="~/images/i_del.png" OnClick="imgDelCash_Click" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="20px" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <FooterStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="Black" />
                                                        <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                                                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnSaveCash" runat="server" Text="Save" Width="60px" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel ID="tpSplitbill" runat="server" HeaderText="Split Bill">
                        <HeaderTemplate>
                            Split Bill
                        </HeaderTemplate>
                        <ContentTemplate>
                            <div style="width: 1100px">
                                <table width="100%">
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            Quantity* :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtQuan" runat="server" Width="100px"></asp:TextBox>
                                        </td>
                                        <td>
                                            Payment :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlPayment" runat="server">
                                                <asp:ListItem>cash</asp:ListItem>
                                                <asp:ListItem>cheq</asp:ListItem>
                                                <asp:ListItem Value="trans">payin</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            Status :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlBillStatus" runat="server">
                                                <asp:ListItem Value="901">Complete ส่งมอบ</asp:ListItem>
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            Bank :
                                        </td>
                                        <td>
                                            <asp:DropDownList ID="ddlBank" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                        <td>
                                            Cheque no :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCheq" runat="server" Width="80px"></asp:TextBox>
                                        </td>
                                        <td>
                                            Due date :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtDuedate" runat="server" Width="70px"></asp:TextBox>
                                            <ajaxToolkit:CalendarExtender ID="txtDuedate_CalendarExtender" runat="server" Enabled="True"
                                                PopupButtonID="imgCalDateDelivery" PopupPosition="BottomRight" TargetControlID="txtDuedate">
                                            </ajaxToolkit:CalendarExtender>
                                            <asp:ImageButton ID="imgCalDateDelivery" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="12" align="center">
                                           
                                            <asp:Button ID="btnAddBill" runat="server" Text="Add Bill" />
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnRefresh" runat="server" Text="Refresh" />
                                          
                                            &nbsp;&nbsp;
                                            <asp:Button ID="btnSave" runat="server" Style="height: 26px" Text="Save" 
                                                Width="60px" />
                                          
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="12">
                                            <div style="text-align: right; width: 99%; margin-bottom: 4px">
                                                <asp:LinkButton ID="linkRptSell" runat="server">Receipt</asp:LinkButton></div>
                                            <asp:GridView ID="gvTicket" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataKeyNames="ref_no,billing,ticket_sp_id"
                                                GridLines="Vertical" Width="99%" ShowFooter="True">
                                                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                                <AlternatingRowStyle BackColor="Gainsboro" />
                                                <Columns>
                                                    <asp:BoundField HeaderText="No.">
                                                        <ItemStyle HorizontalAlign="Center" Width="20px" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="payment" HeaderText="payment" SortExpression="payment">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="quantity" HeaderText="Quantity">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="amount" DataFormatString="{0:#,##0.00}" HeaderText="Amount(baht)"
                                                        SortExpression="amount">
                                                        <ItemStyle HorizontalAlign="Right" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="bank_name" HeaderText="Bank">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="Cheque_No" DataField="payment_cheq_no">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="payment_duedate" HeaderText="Duedate_Cheque" DataFormatString="{0:d/M/yyyy}">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                     <asp:BoundField DataField="status_id" HeaderText="Status">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="created_by" HeaderText="Create by">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:TemplateField HeaderText="Status" SortExpression="status_id" Visible="False">
                                                        <ItemTemplate>
                                                            <select id="ddlBuy" runat="server" style="width: 120px">
                                                                <option value="901" style="color: Green">Complete ส่งมอบ</option>
                                                            </select>
                                                            <select id="ddlSell" runat="server" style="width: 120px">
                                                                <option value="901" style="color: Green">Complete ส่งมอบ</option>
                                                                <option value="902" style="color: #003300">Complete ฝาก</option>
                                                            </select>
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField HeaderText="Receipt">
                                                        <ItemTemplate>
                                                            <asp:Label ID="lblReceipt" runat="server" Text='<%# Bind("run_no") %>'></asp:Label>
                                                            &nbsp;
                                                            <asp:ImageButton ID="imgDelReceipt" runat="server" ImageUrl="~/images/i_del.png"
                                                                OnClick="imgDelReceipt_Click" Height="12" Width="12" />
                                                        </ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <ItemTemplate>
                                                            <asp:ImageButton ID="imgDel" runat="server" ImageUrl="~/images/i_del.png" OnClick="imgDel_Click" />
                                                        </ItemTemplate>
                                                        <ItemStyle Width="50px" HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:TemplateField>
                                                        <HeaderTemplate>
                                                            <input id="cbHead" runat="server" onclick="switchCheckboxSplit(this);" type="checkbox" />
                                                        </HeaderTemplate>
                                                        <ItemTemplate>
                                                            <input id="cbRow" runat="server" type="checkbox" />
                                                        </ItemTemplate>
                                                        <HeaderStyle HorizontalAlign="Center" />
                                                        <ItemStyle HorizontalAlign="Center" Width="20px" />
                                                    </asp:TemplateField>
                                                </Columns>
                                                <FooterStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="Black" />
                                                <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                                                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                    <ajaxToolkit:TabPanel runat="server" HeaderText="Gold Deposit" ID="tpGold" Visible="false">
                        <HeaderTemplate>
                            Gold Deposit
                        </HeaderTemplate>
                        <ContentTemplate>
                            <asp:UpdatePanel ID="udpGOld" runat="server">
                                <ContentTemplate>
                                    <div style="width: 850px">
                                        <table width="100%">
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    Quantity* :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtQuanGold" runat="server" Width="100px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    Payment :
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlPaymentGold" runat="server">
                                                        <asp:ListItem>cash</asp:ListItem>
                                                        <asp:ListItem>cheq</asp:ListItem>
                                                        <asp:ListItem Value="trans">payin</asp:ListItem>
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    Bank :
                                                </td>
                                                <td>
                                                    <asp:DropDownList ID="ddlBankGold" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                                <td>
                                                    Cheque no :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtCheqGold" runat="server" Width="80px"></asp:TextBox>
                                                </td>
                                                <td>
                                                    Due date :
                                                </td>
                                                <td>
                                                    <asp:TextBox ID="txtDuedateGold" runat="server" Width="70px"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="txtDuedateGold_CalendarExtender" runat="server"
                                                        Enabled="True" PopupButtonID="imgCalDateDeliveryGold" PopupPosition="BottomRight"
                                                        TargetControlID="txtDuedateGold">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <asp:ImageButton ID="imgCalDateDeliveryGold" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
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
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnAddGold" runat="server" Text="Add" Width="60px" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
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
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                            <tr>
                                                <td colspan="10">
                                                    <asp:GridView ID="gvGold" runat="server" AutoGenerateColumns="False" BackColor="White"
                                                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataKeyNames="ref_no"
                                                        GridLines="Vertical" Width="99%" ShowFooter="True">
                                                        <AlternatingRowStyle BackColor="Gainsboro" />
                                                        <Columns>
                                                            <asp:BoundField HeaderText="No.">
                                                                <ItemStyle HorizontalAlign="Center" Width="20px" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="payment" HeaderText="payment" SortExpression="payment">
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="Quantity" DataFormatString="{0:#,##0.00}" HeaderText="Quantity"
                                                                SortExpression="Quantity">
                                                                <ItemStyle HorizontalAlign="Right" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="bank_name" HeaderText="Bank">
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField HeaderText="Cheque_No" DataField="payment_cheq_no">
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:BoundField DataField="payment_duedate" HeaderText="Duedate_Cheque" DataFormatString="{0:d/M/yyyy}">
                                                                <ItemStyle HorizontalAlign="Center" />
                                                            </asp:BoundField>
                                                            <asp:TemplateField>
                                                                <ItemTemplate>
                                                                    <asp:ImageButton ID="imgDelGold" runat="server" ImageUrl="~/images/i_del.png" OnClick="imgDelGold_Click" />
                                                                </ItemTemplate>
                                                                <ItemStyle Width="20px" HorizontalAlign="Center" />
                                                            </asp:TemplateField>
                                                        </Columns>
                                                        <FooterStyle BackColor="#CCCCCC" Font-Bold="True" ForeColor="Black" />
                                                        <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                                                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                                    </asp:GridView>
                                                </td>
                                            </tr>
                                            <tr>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    <asp:Button ID="btnSaveGold" runat="server" Text="Save" />
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                                <td>
                                                    &nbsp;
                                                </td>
                                            </tr>
                                        </table>
                                    </div>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </ContentTemplate>
                    </ajaxToolkit:TabPanel>
                </ajaxToolkit:TabContainer>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
