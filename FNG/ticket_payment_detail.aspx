<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false"
    CodeFile="ticket_payment_detail.aspx.vb" Inherits="ticket_payment_detail" Title="Payment Detail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="button/css/buttonPro.css" rel="stylesheet" />
    <script type="text/javascript">

        function modalHide() {
            $find("ctl00_SampleContent_modalPopUpExtender1").hide();
        }
        function modalHide2() {
            $find("ctl00_SampleContent_modalPopUpExtender2").hide();
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
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea">
        <div class="demoheading">
            Payment Detail
        </div>
        <asp:Panel ID="pnMain" runat="server">
            <div style="width: 1100px">
                <div align="center">
                    <asp:Button ID="btnSave" runat="server" CssClass="buttonPro small rounded light_blue" Text="Save" Width="100px" />&nbsp;
                     <asp:Button ID="btnReceipt" runat="server" CssClass="buttonPro small rounded light_blue" Text="Receipt" Width="100px" />
                </div>
                <fieldset>
                    <legend class="topic">Ticket List</legend>
                    <asp:UpdatePanel ID="upGv" runat="server">
                        <ContentTemplate>
                            <div style="width: 700px">
                                <asp:HiddenField ID="hdfPid" runat="server" />
                                <asp:HiddenField ID="hdfCustId" runat="server" />
                                <div style="text-align: right">
                                    <asp:LinkButton ID="linkAdd" runat="server" Text="Add Ticket"></asp:LinkButton>
                                    &nbsp;&nbsp;
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
                                    <div style="overflow: auto; background-color: white; padding: 30px 30px 30px 30px">
                                        <asp:GridView ID="gvPopup" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                            BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                            CellPadding="3" GridLines="Vertical" Width="700px" class="GridviewTable" DataKeyNames="ticket_id,ref_no">
                                            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                            <AlternatingRowStyle BackColor="Gainsboro" />
                                            <Columns>
                                                <asp:BoundField DataField="ref_no" HeaderText="Ticket_ref">
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
                                                <asp:BoundField DataField="user_name" HeaderText="Created_By">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="link" runat="server" OnClick="linkAddTicket_Click">Add</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                            <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                        </asp:GridView>
                                        <div style="text-align: center; padding: 10px 10px 10px 10px">
                                            <asp:Button ID="btnOk" runat="server" Text="Cancel" OnClientClick="modalHide();" />
                                        </div>
                                    </div>

                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </fieldset>

                 <%--split--%>
                 <fieldset>
                    <legend class="topic">Ticket Split List</legend>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <div style="width: 700px">
                                <div style="text-align: right">
                                    <asp:LinkButton ID="linkOpenSplit" runat="server" Text="Add Ticket Split"></asp:LinkButton>
                                    &nbsp;&nbsp;
                                </div>
                                <asp:GridView ID="gvSplit" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                    BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                    CellPadding="3" GridLines="Vertical" Width="100%" DataKeyNames="ticket_sp_id,ref_no">
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
                                                <asp:ImageButton ID="imgDelSplit" runat="server" ImageUrl="~/images/i_del.png" 
                                                    onclick="imgDelSplit_Click" />
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
                            <asp:Button runat="server" ID="btnModalPopUp3" Style="display: none" />
                            <AjaxToolkit:ModalPopupExtender ID="modalPopUpExtender3" runat="server" TargetControlID="btnModalPopUp3"
                                PopupControlID="pnlPopSplit" BackgroundCssClass="modalBackground" OkControlID="btnOk3">
                            </AjaxToolkit:ModalPopupExtender>
                            <asp:Panel runat="Server" ID="pnlPopSplit">
                                <div style="text-align: center">
                                    <div style="overflow: auto; background-color: white; padding: 30px 30px 30px 30px">
                                        <asp:GridView ID="gvPopSplit" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                            BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                            CellPadding="3" GridLines="Vertical" Width="700px" class="GridviewTable" DataKeyNames="ticket_sp_id,ref_no">
                                            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                            <AlternatingRowStyle BackColor="Gainsboro" />
                                            <Columns>
                                                <asp:BoundField DataField="ref_no" HeaderText="Ticket_ref">
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
                                                <asp:BoundField DataField="user_name" HeaderText="Created_By">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="link" runat="server" OnClick="linkAddSplit_Click">Add</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                            <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                        </asp:GridView>
                                        <div style="text-align: center; padding: 10px 10px 10px 10px">
                                            <asp:Button ID="btnOk3" runat="server" Text="Cancel" OnClientClick="modalHide();" />
                                        </div>
                                    </div>

                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </fieldset>
                 <%--split--%>
                <fieldset>
                    <legend class="topic">Transaction List</legend>
                    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                        <ContentTemplate>
                            <div style="width: 700px">
                                <div style="text-align: right">
                                    <asp:LinkButton ID="linkOpenTrans" runat="server" Text="Add Transaction"></asp:LinkButton>
                                    &nbsp;&nbsp;
                                </div>
                                <asp:GridView ID="gvTran" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                    BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                    CellPadding="3" GridLines="Vertical" Width="100%" DataKeyNames="cust_tran_id">
                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                    <AlternatingRowStyle BackColor="Gainsboro" />
                                    <Columns>
                                        <asp:BoundField DataField="created_date" HeaderText="Created Date">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="gold_type_id" HeaderText="Type" />
                                        <asp:BoundField DataField="type" HeaderText="Transaction">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="quantity" HeaderText="Quantity" DataFormatString="{0:#,##0}">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="amount" HeaderText="Amount" DataFormatString="{0:#,##0}">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="payment_by_name" HeaderText="Created_By">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField>
                                            <ItemTemplate>
                                                <asp:ImageButton ID="imgDelTrans" runat="server" ImageUrl="~/images/i_del.png" OnClick="imgDelTrans_Click" />
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
                            <asp:Button runat="server" ID="btnModalPopUp2" Style="display: none" />
                            <AjaxToolkit:ModalPopupExtender ID="modalPopUpExtender2" runat="server" TargetControlID="btnModalPopUp2"
                                PopupControlID="pnlPopTrans" BackgroundCssClass="modalBackground" OkControlID="btnOk2">
                            </AjaxToolkit:ModalPopupExtender>
                            <asp:Panel runat="Server" ID="pnlPopTrans">
                                <div style="text-align: center">
                                    <div style="overflow: auto; background-color: white; padding: 30px 30px 30px 30px">
                                        <asp:GridView ID="gvPopTrans" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                            BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                            CellPadding="3" GridLines="Vertical" Width="700px" class="GridviewTable" DataKeyNames="cust_tran_id">
                                            <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                            <AlternatingRowStyle BackColor="Gainsboro" />
                                            <Columns>
                                                <asp:BoundField DataField="created_date" HeaderText="Created Date">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="gold_type_id" HeaderText="Type" />
                                                <asp:BoundField DataField="type" HeaderText="Transaction">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="quantity" HeaderText="Quantity" DataFormatString="{0:#,##0}">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="amount" HeaderText="Amount" DataFormatString="{0:#,##0}">
                                                    <ItemStyle HorizontalAlign="Right" />
                                                </asp:BoundField>
                                                <asp:BoundField DataField="user_name" HeaderText="Created_By">
                                                    <ItemStyle HorizontalAlign="Center" />
                                                </asp:BoundField>
                                                <asp:TemplateField>
                                                    <ItemTemplate>
                                                        <asp:LinkButton ID="linkAddTrans" OnClick="linkAddTrans_Click" runat="server">Add</asp:LinkButton>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                            <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                            <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                                            <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                            <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                        </asp:GridView>
                                        <div style="text-align: center; padding: 10px 10px 10px 10px">
                                            <asp:Button ID="btnOk2" runat="server" Text="Cancel" OnClientClick="modalHide2();" />
                                        </div>
                                    </div>

                                </div>
                            </asp:Panel>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </fieldset>
            </div>
        </asp:Panel>
        <asp:Panel ID="pnNon" runat="server">
        <div style='color:red;text-align:center;border:solid 1px silver;'>Data not Found.</div>
        </asp:Panel>
    </div>
</asp:Content>
