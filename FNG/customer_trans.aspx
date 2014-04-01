<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false"
    CodeFile="customer_trans.aspx.vb" Inherits="customer_trans" Title="Deposit Transaction" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="user_control/ucPortFolio.ascx" TagName="ucPortFolio" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style4
        {
            width: 100px;
            text-align: right;
        }
        .style6
        {
            width: 124px;
        }
        .lbl
        {
            color: Black;
        }
        .style40
        {
            text-align:right;
            width: 72px;
        }
        .style48
        {
            width: 229px;
            text-align: left;
        }
    </style>
    <script language="javascript" type="text/javascript">
        function pageLoad() {
            $get('<%=txtCustName.ClientId %>').focus();
        }

        function switchCheckbox(id,index,grid) {
            var grid = document.getElementById("ctl00_SampleContent_tcTrans_" + grid);
            var cell;

            if (grid.rows.length > 0) {

                for (i = 1; i < grid.rows.length; i++) {

                    cell = grid.rows[i].cells[index];
                    for (j = 0; j < cell.childNodes.length; j++) {
                        if (cell.childNodes[j].type == "checkbox") {
                            cell.childNodes[j].checked = id.checked;
                        }
                    }
                }
            }

            

        }


    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea">
        <div class="demoheading">
            <table border="0" width="100%">
                <tr>
                    <td align="left" class="style1">
                        Deposit Transaction
                    </td>
                    <td align="right" class="style1">
                    </td>
                </tr>
            </table>
        </div>
        <div style="margin-left: 1em">
            <asp:TextBox ID="txtCustName" Width="200px" runat="server" />
            <ajaxToolkit:AutoCompleteExtender ID="txtCustName_AutoCompleteExtender" runat="server"
                DelimiterCharacters="" CompletionInterval="300" Enabled="True" ServicePath="~/gtc.asmx"
                UseContextKey="True" CompletionSetCount="20" OnClientItemSelected="onCustNameAutoCompleteClick"
                TargetControlID="txtCustName" ServiceMethod="getCust_nameList">
            </ajaxToolkit:AutoCompleteExtender>
            <asp:HiddenField ID="hdfCust_id" runat="server" />
        </div>
        <br />
        <span style="display: none">
        <asp:Button ID="btnSearch" runat="server" Text="Search" />
        </span>
        <uc1:ucPortFolio ID="ucPortFolio1" runat="server" />
        <ajaxToolkit:TabContainer ID="tcTrans" runat="server" ActiveTabIndex="1" 
            Width="100%">
            <ajaxToolkit:TabPanel runat="server" HeaderText="Cash" ID="tpCash">
                <ContentTemplate>
                    <asp:UpdatePanel ID="udpMain" runat="server">
                        <Triggers>
                            <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="click" />
                        </Triggers>
                        <ContentTemplate>
                            <table border="0" cellpadding="2" width="90%">
                                <tr>
                                    <td class="style40">
                                        Date* :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtDate" runat="server" Width="100px" /><ajaxToolkit:CalendarExtender
                                            ID="txtDate_CalendarExtender" runat="server" Enabled="True" PopupPosition="BottomRight"
                                            TargetControlID="txtDate" PopupButtonID="imgCalDate">
                                        </ajaxToolkit:CalendarExtender>
                                        <asp:ImageButton ID="imgCalDate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                        &nbsp;&nbsp;
                                        <asp:CheckBox ID="cbPre" runat="server" Visible = "false" Text="ถอนล่วงหน้า" />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" class="style40">
                                        Type* :
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rdoType" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem>Deposit</asp:ListItem>
                                            <asp:ListItem>Withdraw</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style40">
                                        Payment* :
                                    </td>
                                    <td valign="top">
                                        <table>
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
                                                            <td id="bank1" style="display: block">
                                                                Bank :
                                                            </td>
                                                            <td id="bank2" style="display: block">
                                                                <asp:DropDownList ID="ddlBank" runat="server">
                                                                </asp:DropDownList>
                                                            </td>
                                                            <td id="cheq1" style="display: block">
                                                                Cheque no :
                                                            </td>
                                                            <td id="cheq2" style="display: block">
                                                                <asp:TextBox ID="txtCheq" runat="server" Width="80px"></asp:TextBox>
                                                            </td>
                                                            <td id="cheq3" style="display: block">
                                                                Due date :
                                                            </td>
                                                            <td id="cheq4" style="display: block">
                                                                <asp:TextBox ID="txtDuedate" runat="server" Width="70px"></asp:TextBox><ajaxToolkit:CalendarExtender
                                                                    ID="txtDuedate_CalendarExtender" runat="server" Enabled="True" PopupButtonID="imgDuedate"
                                                                    PopupPosition="BottomRight" TargetControlID="txtDuedate">
                                                                </ajaxToolkit:CalendarExtender>
                                                                <asp:ImageButton ID="imgDuedate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                                            </td>
                                                        </tr>
                                                    </table>
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style40">
                                        Bill No :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBillNo" runat="server" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" class="style40">
                                        Amount* :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtAmount" runat="server" Width="200px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" class="style40">
                                        Ref No :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRef_no" runat="server" Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" class="style40">
                                        Remark :
                                    </td>
                                    <td valign="top">
                                        <asp:TextBox ID="txtRemark" runat="server" Width="400px" Height="50px" TextMode="MultiLine"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" class="style40">
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="top">
                                        <asp:Button ID="btnSave1" runat="server" Text="Add" Width="50px" />&nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" class="style40">
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="top">
                                         <div style="text-align: right">
                                            <asp:LinkButton ID="linkReportCash" runat="server" Text="Receipt"></asp:LinkButton>
                                        </div>
                                        <asp:GridView ID="gvTrans" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                            BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                            CellPadding="3" DataKeyNames="cust_tran_id" DataSourceID="objSrcTrans" GridLines="Vertical"
                                            Width="100%">
                                            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                            <Columns>
                                                <%--<asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="linkReport" ImageUrl="~/images/ticket.ico" runat="server" 
                                                            Width="20px" Height="20px" onclick="linkReport_Click" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                </asp:TemplateField>--%>
                                                <asp:BoundField DataField="datetime" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Datetime"
                                                    SortExpression="datetime" >
                                                <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Ticket_ref" SortExpression="ref_no">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="link" runat="server" Target="_blank"><%#Eval("ref_no")%></asp:HyperLink></ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="type" HeaderText="Type" SortExpression="type" />
                                                <asp:BoundField DataField="cash_type" HeaderText="Cash_Type" SortExpression="cash_type"
                                                    Visible="False" />
                                                <asp:BoundField DataField="bill_no" HeaderText="Bill_no" SortExpression="bill_no" />
                                                <asp:TemplateField HeaderText="Amount" SortExpression="amount">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAmount" runat="server"></asp:Label><asp:TextBox ID="txtAmount"
                                                            runat="server" MaxLength="15" Visible="false" Width="100px" Wrap="False"></asp:TextBox></ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" Width="100px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="remark" HeaderText="Remark" SortExpression="remark" >
                                                <ItemStyle Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="user_name" HeaderText="Created_by" SortExpression="user_name" />
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="linkUpdate" runat="server" OnClick="linkUpdate_Click">Update</asp:LinkButton><asp:LinkButton
                                                            ID="linkSave" runat="server" OnClick="linkSave_Click" Visible="false">Save</asp:LinkButton>
                                                        &nbsp;&nbsp;<asp:LinkButton ID="linkCancel" runat="server" OnClick="linkCancel_Click"
                                                            Visible="false">Cancel</asp:LinkButton></ItemTemplate>
                                                </asp:TemplateField>
                                                 <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <input id="cbHead" runat="server" onclick="switchCheckbox(this,8,'tpCash_gvTrans');" type="checkbox" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <input id="cbRow" runat="server" type="checkbox" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" Width="20px" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                                            <AlternatingRowStyle BackColor="Gainsboro" />
                                        </asp:GridView>
                                        <asp:ObjectDataSource ID="objSrcTrans" runat="server" OldValuesParameterFormatString="original_{0}"
                                            SelectMethod="getCustomerTran" TypeName="clsDB">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="hdfCust_id" Name="cust_id" PropertyName="Value"
                                                    Type="String" />
                                                <asp:Parameter ConvertEmptyStringToNull="False" Name="transCash" Type="String" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
            <ajaxToolkit:TabPanel ID="tpGold" runat="server" HeaderText="Gold">
                <HeaderTemplate>
                    Gold
                </HeaderTemplate>
                <ContentTemplate>
                    <asp:UpdatePanel ID="udpMain0" runat="server">
                        <ContentTemplate>
                            <table border="0" cellpadding="2" width="90%">
                                <tr>
                                    <td class="style40">
                                        Date* :
                                    </td>
                                    <td class="style48">
                                        <asp:TextBox ID="txtDate2" runat="server" Width="100px" /><ajaxToolkit:CalendarExtender
                                            ID="txtDate2_CalendarExtender" runat="server" Enabled="True" PopupButtonID="imgCalDate2"
                                            PopupPosition="BottomRight" TargetControlID="txtDate2">
                                        </ajaxToolkit:CalendarExtender>
                                        <asp:ImageButton ID="imgCalDate2" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                        &nbsp;&nbsp;
                                        <asp:CheckBox ID="cbPre2" runat="server" Visible="false" Text="ถอนล่วงหน้า" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style40" valign="top">
                                        Type* :
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rdoType2" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem>Deposit</asp:ListItem>
                                            <asp:ListItem>Withdraw</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style40">
                                        Pure :
                                    </td>
                                    <td>
                                        <asp:RadioButtonList ID="rdoPure" runat="server" RepeatDirection="Horizontal">
                                            <asp:ListItem Value="96">96.50(บาท)</asp:ListItem>
                                            <asp:ListItem Value="96G">96.50(กรัม)</asp:ListItem>
                                            <asp:ListItem Value="96M">96.50(มินิ)</asp:ListItem>
                                            <asp:ListItem Value="99">99.99</asp:ListItem>
                                        </asp:RadioButtonList>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style40" valign="top">
                                        Quantity* :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtQuan" runat="server" Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td class="style40" valign="top">
                                        Ref No :
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtRefNo2" runat="server" Width="200px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" class="style40">
                                        Remark :
                                    </td>
                                    <td valign="top">
                                        <asp:TextBox ID="txtRemark2" runat="server" Height="50px" TextMode="MultiLine" Width="400px" />
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" class="style40">
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="top">
                                        <asp:Button ID="btnSave2" runat="server" Text="Add" Width="50px" />&nbsp;&nbsp;
                                    </td>
                                </tr>
                                <tr>
                                    <td valign="top" class="style40">
                                        &nbsp;&nbsp;
                                    </td>
                                    <td valign="top">
                                        <div style="text-align: right">
                                            <asp:LinkButton ID="linkReportGold" runat="server" Text="Receipt"></asp:LinkButton>
                                        </div>

                                        <asp:GridView ID="gvTrans2" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                            BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                            CellPadding="3" DataKeyNames="cust_tran_id" DataSourceID="objSrcTrans2" GridLines="Vertical"
                                            Width="100%">
                                            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                            <Columns>
                                                <%--<asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="linkReport" ImageUrl="~/images/ticket.ico" runat="server" 
                                                            Width="20px" Height="20px" onclick="linkReport_Click" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="40px" />
                                                </asp:TemplateField>--%>
                                                <asp:BoundField DataField="datetime" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Datetime"
                                                    SortExpression="datetime" >
                                                <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:TemplateField HeaderText="Ticket_ref" SortExpression="ticket_refno">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="link" runat="server" Target="_blank"><%#Eval("ticket_refno")%></asp:HyperLink>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="type" HeaderText="Type"
                                                    SortExpression="type" />
                                                <asp:BoundField DataField="cash_type" HeaderText="Cash_Type" SortExpression="cash_type"
                                                    Visible="False" />
                                                <asp:BoundField DataField="gold_type_id" HeaderText="Pure" SortExpression="gold_type_id" />
                                                <asp:TemplateField HeaderText="Quantity" SortExpression="quantity">
                                                    <ItemTemplate>
                                                        <asp:Label ID="lblAmount" runat="server"></asp:Label>
                                                        <asp:TextBox ID="txtAmount" runat="server" MaxLength="15" Visible="false" Width="100px"
                                                            Wrap="False"></asp:TextBox>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Right" Width="100px" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="Price" DataFormatString="{0:#,##0}" HeaderText="Price"
                                                    SortExpression="Price">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="remark" HeaderText="Remark" SortExpression="remark" >
                                                <ItemStyle Wrap="False" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="user_name" HeaderText="Created_by" SortExpression="user_name" />
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="linkUpdate" runat="server" OnClick="linkUpdate_Click">Update</asp:LinkButton>
                                                        <asp:LinkButton ID="linkSave" runat="server" OnClick="linkSave_Click" Visible="false">Save</asp:LinkButton>
                                                        &nbsp;
                                                        <asp:LinkButton ID="linkCancel" runat="server" OnClick="linkCancel_Click" Visible="false">Cancel</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <%--<asp:TemplateField Visible="true">
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgDel2" runat="server" ImageUrl="~/images/i_del.png" OnClick="imgDel2_Click" />
                                                    </ItemTemplate>
                                                </asp:TemplateField>--%>
                                                <asp:TemplateField>
                                                    <HeaderTemplate>
                                                        <input id="cbHead" runat="server" onclick="switchCheckbox(this,9,'tpGold_gvTrans2');" type="checkbox" />
                                                    </HeaderTemplate>
                                                    <ItemTemplate>
                                                        <input id="cbRow" runat="server" type="checkbox" />
                                                    </ItemTemplate>
                                                    <HeaderStyle HorizontalAlign="Center" />
                                                    <ItemStyle HorizontalAlign="Center" Width="20px" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                            <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                                            <AlternatingRowStyle BackColor="Gainsboro" />
                                        </asp:GridView>
                                        <asp:ObjectDataSource ID="objSrcTrans2" runat="server" OldValuesParameterFormatString="original_{0}"
                                            SelectMethod="getCustomerTran" TypeName="clsDB">
                                            <SelectParameters>
                                                <asp:ControlParameter ControlID="hdfCust_id" Name="cust_id" PropertyName="Value"
                                                    Type="String" />
                                                <asp:ControlParameter ControlID="hdfCust_id" ConvertEmptyStringToNull="False" Name="transCash"
                                                    PropertyName="Value" Type="String" />
                                            </SelectParameters>
                                        </asp:ObjectDataSource>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </ajaxToolkit:TabPanel>
        </ajaxToolkit:TabContainer>
    </div>
    <div class="demobottom">
    </div>
</asp:Content>
