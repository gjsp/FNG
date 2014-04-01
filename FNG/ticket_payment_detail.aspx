<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false"
    CodeFile="ticket_payment_detail.aspx.vb" Inherits="ticket_payment_detail" Title="Payment Detail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script type="text/javascript">

        function modalHide() {
            $find("ctl00_SampleContent_modalPopUpExtender1").hide();

        }

        function switchCheckbox(id) {
            var frm = document.getElementById('aspnetForm');

            for (i = 0; i < frm.elements.length; i++) {
                if (frm.elements[i].type == "checkbox") {
                    if (frm.elements[i].name.split('$')[4] == 'cbHead' || frm.elements[i].name.split('$')[4] == 'cbRow') {
                        frm.elements[i].checked = id.checked;
                    }
                }
            }
        }

    </script>
    <style type="text/css">
        .modalBackground
        { 
            background-color: black;
            filter: alpha(opacity=90);
            opacity: 0.9;
        }
        .auto-style1 {
            height: 35px;
        }
        .auto-style2 {
            height: 29px;
        }
        .auto-style3 {
            height: 30px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea">
        <div class="demoheading">
            Payment Detail
        </div>
        <asp:Panel ID="pnMain" runat="server">
            <div style="width: 1100px">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <fieldset>
                            <legend class="topic">Payment</legend>
                            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                                <ContentTemplate>
                                    <table width="100%" style="color: black">
                                         <tr style="height:30px">
                                            <td>
                                                Payment ID :<asp:Label ID="lblpid" runat="server" Font-Bold="true"></asp:Label>
                                            </td>
                                            <td>
                                                Create By :<asp:Label ID="lblBy" runat="server" Font-Bold="true"></asp:Label>
                                            </td>
                                            <td style="width:400px">
                                                Create Date :
                                                <asp:Label ID="lblDate" runat="server" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                         <tr style="height:30px">
                                            <td>
                                                Purity :
                                                <asp:Label ID="lblPurity" runat="server" Font-Bold="true"></asp:Label>
                                            </td>
                                            <td>
                                                Billing :<asp:Label ID="lblBill" runat="server" Font-Bold="true"></asp:Label>
                                            </td>
                                            <td style="width:400px">
                                                ทองฝาก 99.99 :<asp:Label ID="lblGoldDep99" runat="server" Font-Bold="true"></asp:Label>
                                            </td>
                                        </tr>
                                         <tr style="height:30px">
                                             <td>Cust ID :
                                                 <asp:Label ID="lblCustId" runat="server" Font-Bold="true"></asp:Label>
                                             </td>
                                             <td>Name :<asp:Label ID="lblName" runat="server" Font-Bold="true"></asp:Label>
                                             </td>
                                             <td style="width:400px">&nbsp;ทองฝาก 96.50 :
                                                 <asp:Label ID="lblGoldDep96" runat="server" Font-Bold="true"></asp:Label>
                                             </td>
                                         </tr>
                                        <tr>
                                            <td class="auto-style3">
                                                จำนวนเงินที่ต้องชำระ :
                                                <asp:Label ID="lblPayCash" runat="server" Font-Bold="True"></asp:Label>
                                            </td>
                                            <td class="auto-style3">
                                                จำนวนเงินที่ชำระแล้ว :
                                                <asp:TextBox ID="txtPaidCash" runat="server" Font-Bold="true" Width="80"></asp:TextBox>
                                                <asp:Label ID="lblPaidCash" runat="server" Font-Bold="True"></asp:Label>
                                            </td>
                                            <td style="text-align: left" class="auto-style3">
                                                Status :
                                                <asp:Label ID="lblStatus" runat="server" Font-Bold="True"></asp:Label>
                                            </td>
                                        </tr>
                                         <tr>
                                             <td class="auto-style2">จำนวนทองที่ต้องชำระ :
                                                 <asp:Label ID="lblPayGold" runat="server" Font-Bold="True"></asp:Label>
                                             </td>
                                             <td class="auto-style2">จำนวนทองที่ชำระแล้ว :
                                                 <asp:TextBox ID="txtPaidGold" runat="server" Font-Bold="true" Width="80"></asp:TextBox>
                                                 <asp:Label ID="lblPaidGold" runat="server" Font-Bold="True"></asp:Label>
                                             </td>
                                             <td style="text-align: left" class="auto-style2">

                                                 <asp:DropDownList ID="ddlStatus" runat="server" Width="140px">
                                                     <asp:ListItem Text="Complete ส่งมอบ" Value="901"></asp:ListItem>
                                                     <%--<asp:ListItem Text="Complete ตัดทองฝาก" Value="903"></asp:ListItem>--%>
                                                 </asp:DropDownList>
                                                 <asp:LinkButton ID="linkComplete" runat="server" >Save</asp:LinkButton>

                                             </td>
                                         </tr>
                                         <tr>
                                             <td colspan="3" class="auto-style1"><span style="vertical-align:top">Note :</span><asp:TextBox ID="txtNote" runat="server" Width="500px"></asp:TextBox>
                                                 <asp:Label ID="lblNote" runat="server" Font-Bold="true"></asp:Label>
                                             </td>
                                         </tr>
                                        <tr>
                                            <td colspan="3" style="text-align: center">
                                                <asp:Button ID="btnEdit" runat="server" Text="Edit" Width="60px" />
                                                <asp:Button ID="btnSave" runat="server" Text="Save" Width="60px" />
                                                &nbsp;
                                                <asp:Button ID="btnCancel" runat="server" Text="Cancel" Width="60px" />
                                            </td>
                                        </tr>
                                    </table>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </fieldset>
                    </td>
                </tr>
                <tr>
                    <td>
                        <fieldset>
                            <legend class="topic">Ticket Listt</legend>
                            <asp:UpdatePanel ID="upGv" runat="server">
                                <ContentTemplate>
                                    <div style="width: 700px">
                                        <div style="text-align: right">
                                            <asp:LinkButton ID="linkAdd" runat="server" Text="Add Ticket"></asp:LinkButton>
                                            &nbsp;&nbsp;
                                            <asp:LinkButton ID="linkReport" runat="server" Text="Receipt"></asp:LinkButton>
                                        </div>
                                        <asp:GridView ID="gv" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                            BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                            CellPadding="3" GridLines="Vertical" Width="100%" DataKeyNames="ticket_id,ref_no">
                                            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                            <AlternatingRowStyle BackColor="Gainsboro" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="Ticket_ref" SortExpression="ref_no">
                                                    <ItemTemplate>
                                                        <asp:HyperLink ID="linkRef" runat="server" Target="_blank"><%#Eval("ref_no")%></asp:HyperLink>
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:TemplateField>
                                                <asp:BoundField DataField="type" HeaderText="Type">
                                                <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="price" HeaderText="Price" DataFormatString="{0:#,##0}">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="quantity" HeaderText="Quantity" DataFormatString="{0:#,##0}">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="amount" HeaderText="Amount" DataFormatString="{0:#,##0.00}">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="payment_by_name" HeaderText="Created_By">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:ImageButton ID="imgDel" runat="server" ImageUrl="~/images/i_del.png" OnClick="imgDel_Click" />
                                                    </ItemTemplate>
                                                    <ItemStyle HorizontalAlign="Center" Width="20px" />
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                            <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                        </asp:GridView>
                                    </div>
                                    <asp:Button runat="server" ID="btnModalPopUp" Style="display: none" />
                                    <AjaxToolkit:ModalPopupExtender ID="modalPopUpExtender1" runat="server" TargetControlID="btnModalPopUp"
                                        PopupControlID="pnlPopUp" BackgroundCssClass="modalBackground" OkControlID="btnOk">
                                    </AjaxToolkit:ModalPopupExtender>
                                    <asp:Panel runat="Server" ID="pnlPopUp">
                                        <div style="text-align: center">
                                            <div style="overflow: auto;height: 500px">
                                                <asp:GridView ID="gvPopup" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                    BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                                    CellPadding="3" GridLines="Vertical" Width="700px" class="GridviewTable" DataKeyNames="ticket_id,ref_no">
                                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                                    <AlternatingRowStyle BackColor="Gainsboro" />
                                                    <Columns>
                                                        <asp:BoundField DataField="ref_no" HeaderText="Ticket_Ref">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="type" HeaderText="Type">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="price" HeaderText="Price" DataFormatString="{0:#,##0}">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="quantity" HeaderText="Quantity" DataFormatString="{0:#,##0}">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="amount" HeaderText="Amount" DataFormatString="{0:#,##0.00}">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="payment_by_name" HeaderText="Created_By">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="link" runat="server" OnClick="link_Click">Add</asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                    <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                                </asp:GridView> 
                                                 <div style="text-align: center">
                                                <asp:Button ID="btnOk" runat="server" Text="Cancel" OnClientClick="modalHide();" />
                                                </div>
                                            </div>
                                           
                                        </div>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </fieldset>
                    </td>
                </tr>
            </table>
        </div>
        </asp:Panel>
        <asp:Panel ID="pnNon" runat="server">
        <div style='color:red;text-align:center;border:solid 1px silver;'>Data not Found.</div>
        </asp:Panel>
    </div>
</asp:Content>
