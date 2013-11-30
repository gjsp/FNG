<%@ Page Language="VB" MasterPageFile="~/trade/admin/MasterPageAdmin.master" AutoEventWireup="false" EnableEventValidation = "false"
    CodeFile="price_log.aspx.vb" Inherits="admin_price_log" Title="Price Log" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script language="javascript" type="text/javascript">
        function search() {
            $get('<%=btnSearch.ClientId %>').click();
        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div  class="adminarea">
        <fieldset class="k-fieldset" style="width: 98%;">
            <legend class="topic">Price Log</legend>
            <div align="right" style="margin-bottom: 5px">
                <div>
                    <table>
                        <tr>
                            <td style="width: 50px">
                                <asp:RadioButton ID="rdoTime1" runat="server" GroupName="time" Checked="true" />
                            </td>
                            <td style="width: 50px;text-align:left">
                                Today</td>
                            <td style="width: 50px">
                                <asp:RadioButton ID="rdoTime2" runat="server" GroupName="time" />
                            </td>
                            <td style="text-align:left">
                                from
                                <asp:TextBox ID="txt1Date" runat="server" Width="65"></asp:TextBox>
                                <cc1:CalendarExtender ID="txt1Date_CalendarExtender" runat="server" Enabled="True"
                                    TargetControlID="txt1Date">
                                </cc1:CalendarExtender>
                                <asp:DropDownList ID="ddl1Hour" runat="server">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddl1Min" runat="server">
                                </asp:DropDownList>
                            </td>
                            <td style="width: 400px">
                                <table cellpadding="0" cellspacing="0" border="0">
                                    <tr>
                                        <td>
                                            to&nbsp; &nbsp;
                                            <asp:TextBox ID="txt2Date" runat="server" Width="65"></asp:TextBox>
                                            <cc1:CalendarExtender ID="txt2Date_CalendarExtender" runat="server" Enabled="True"
                                                TargetControlID="txt2Date">
                                            </cc1:CalendarExtender>
                                            <asp:DropDownList ID="ddl2Hour" runat="server">
                                            </asp:DropDownList>
                                            <asp:DropDownList ID="ddl2Min" runat="server">
                                            </asp:DropDownList>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="width: 50px">
                                &nbsp;</td>
                            <td style="width: 50px">
                                &nbsp;</td>
                            <td style="width: 50px">
                                <asp:Button ID="btnSearch" CssClass="buttonPro small grey" runat="server" Text="Search" />
                            </td>
                            <td>
                                &nbsp;
                            </td>
                            <td style="width: 400px">
                                &nbsp;
                            </td>
                        </tr>
                    </table>
                  </div>
            </div>
            <asp:UpdateProgress ID="UpdateProg1" runat="server">
                <ProgressTemplate>
                    <div id="Div1" align="center" valign="middle" runat="server" style="position: absolute;
                        left: 43%; padding: 10px 10px 10px 10px; visibility: visible; border-color: silver;
                        border-style: solid; border-width: 1px; background-color: White;">
                        <img src="../img/indicator.gif" style="vertical-align: middle" alt="Processing" />
                        Loading ... &nbsp;
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
             <div style="padding-bottom:5px;">
                <asp:LinkButton ID="linkExcel" runat="server" Text="Export"></asp:LinkButton>
            </div>
            <asp:UpdatePanel ID="udpMain" runat="server">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnSearch" EventName="click" />
                </Triggers>
                <ContentTemplate>
                    <asp:GridView ID="gvPrice" runat="server" AutoGenerateColumns="False" BackColor="White"
                        BorderColor="#999999" BorderStyle="None" BorderWidth="0px" CellPadding="3" DataKeyNames="id"
                        Width="100%" AllowSorting="True" DataSourceID="ObjectDataSource1">
                        <Columns>
                            <asp:BoundField DataField="bid99_1" HeaderText="Bid99_Lv1" SortExpression="bid99_1">
                            </asp:BoundField>
                            <asp:BoundField DataField="bid99_2" HeaderText="Bid99_Lv2" SortExpression="bid99_2">
                            </asp:BoundField>
                            <asp:BoundField DataField="bid99_3" HeaderText="Bid99_Lv3" SortExpression="bid99_3">
                            </asp:BoundField>
                            <asp:BoundField DataField="ask99_1" HeaderText="Ask99_Lv1" SortExpression="ask99_1">
                            </asp:BoundField>
                            <asp:BoundField DataField="ask99_2" HeaderText="Ask99_Lv2" SortExpression="ask99_2">
                            </asp:BoundField>
                            <asp:BoundField DataField="ask99_3" HeaderText="Ask99_Lv3" SortExpression="ask99_3">
                            </asp:BoundField>
                            <asp:BoundField DataField="bid96_1" HeaderText="Bid96_Lv1" SortExpression="bid96_1">
                            </asp:BoundField>
                            <asp:BoundField DataField="bid96_2" HeaderText="Bid96_Lv2" SortExpression="bid96_2">
                            </asp:BoundField>
                            <asp:BoundField DataField="bid96_3" HeaderText="Bid96_Lv3" SortExpression="bid96_3">
                            </asp:BoundField>
                            <asp:BoundField DataField="ask96_1" HeaderText="Ask96_Lv1" SortExpression="ask96_1">
                            </asp:BoundField>
                            <asp:BoundField DataField="ask96_2" HeaderText="Ask96_Lv2" SortExpression="ask96_2">
                            </asp:BoundField>
                            <asp:BoundField DataField="ask96_3" HeaderText="Ask96_Lv3" SortExpression="ask96_3">
                            </asp:BoundField>
                            <asp:BoundField DataField="modifier_date" HeaderText="Datetime" SortExpression="Datetime" />
                            <asp:BoundField DataField="modifier_by" HeaderText="Update_By" SortExpression="modifier_by" />
                        </Columns>
                        <FooterStyle BackColor="#666666" ForeColor="White" />
                        <HeaderStyle BackColor="#333333" ForeColor="White" />
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                        <AlternatingRowStyle BackColor="Gainsboro" />
                    </asp:GridView>
                </ContentTemplate>
            </asp:UpdatePanel>
            <asp:ObjectDataSource ID="ObjectDataSource1" runat="server" OldValuesParameterFormatString="original_{0}"
                SelectMethod="getStockLogByDate" TypeName="clsMain">
                <SelectParameters>
                    <asp:Parameter ConvertEmptyStringToNull="False" Name="isToday" Type="String" />
                    <asp:Parameter ConvertEmptyStringToNull="False" Name="pDate1" Type="DateTime" />
                    <asp:Parameter ConvertEmptyStringToNull="False" Name="pDate2" Type="DateTime" />
                </SelectParameters>
            </asp:ObjectDataSource>
        </fieldset>
    </div>
</asp:Content>
