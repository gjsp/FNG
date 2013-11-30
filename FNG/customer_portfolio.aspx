<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false"
    CodeFile="customer_portfolio.aspx.vb" Inherits="customer_portfolio" Title="Customer Portfolio" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Src="user_control/ucPortFolio.ascx" TagName="ucPortFolio" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style2
        {
            width: 280px;
        }
        .style3
        {
            width: 280px;
        }
        .style4
        {
            text-align: right;
            vertical-align: top;
        }
        .style5
        {
            text-align: right;
            vertical-align: top;
        }
        .style6
        {
            width: 109px;
            text-align: right;
        }
        .ralize
        {
            width: 140px;
            text-align: right;
        }
    </style>
    <script language="javascript" type="text/javascript">
        //    var prm = Sys.WebForms.PageRequestManager.getInstance();
        //    prm.add_pageLoaded(PageLoadedHandler);
        //    function PageLoadedHandler(sender, args) {
        //        setTimeout('doSizeUp()', 1);
        //    }
        function doSizeUp() {
            //get the tabContainer for later reference
            var tc = document.getElementById("<%=tabPortfolio.ClientId%>");

            //get the index of the tab you just clicked.
            var tabIndex =
     parseInt($find("<%=tabPortfolio.ClientId%>").get_activeTabIndex(), 10);

            //set the tabcontainer height to the tab panel height.
            tc.childNodes[1].style.height = tc.childNodes[1].childNodes[tabIndex].clientHeight;
        }

        function switchCheckboxFolio(id, name) {
            //$get('ctl00_SampleContent_tabPortfolio_tpGold_gvTicket96').rows[2].cells[0].childNodes[0].checked
            for (i = 1; i < $get('ctl00_SampleContent_tabPortfolio_tpGold_' + name).rows.length - 1; i++) {
                $get('ctl00_SampleContent_tabPortfolio_tpGold_' + name).rows[i].cells[0].children[0].checked = id.checked;
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea">
        <asp:HiddenField ID="hdfCust_id" runat="server" />
        <br />
        <asp:UpdatePanel ID="udpMain" runat="server">
            <ContentTemplate>
                <div style="width: 1200px">
                    <uc1:ucPortFolio ID="ucPortFolio1" runat="server" />
                </div>
                
            </ContentTemplate>
        </asp:UpdatePanel>
        <br />
        <cc1:TabContainer ID="tabPortfolio" runat="server" ActiveTabIndex="0" 
            Width="1200px">
            <cc1:TabPanel runat="server" HeaderText="Gold Movement" ID="tpGold">
                <HeaderTemplate>
                    Standing Trans</HeaderTemplate>
                <ContentTemplate>
                    <asp:UpdatePanel ID="udpGrid" runat="server">
                        <ContentTemplate>
                            <fieldset>
                                <legend class="topic">Purity 96.50</legend>
                                <asp:LinkButton ID="linkExport96" runat="server">Export</asp:LinkButton>
                                <asp:GridView
                                    ID="gvTicket96" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                    Width="100%" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                    CellPadding="3" DataSourceID="objSrcTicket96" DataKeyNames="ticket_id" ShowFooter="True">
                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                    <Columns>
                                        <asp:BoundField DataField="ticket_date" HeaderText="วันที่ซื้อขาย" DataFormatString="{0:dd/MM/yyyy}"
                                            SortExpression="ticket_date">
                                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="created_date" HeaderText="เวลา" DataFormatString="{0:hh:mm tt}"
                                            SortExpression="created_date">
                                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="ticket_ref" SortExpression="ticket_id">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="link" runat="server" Target="_blank"><%#Eval("ticket_id")%></asp:HyperLink></ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="type" HeaderText="คำสั่ง" SortExpression="type">
                                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="price" HeaderText="ราคา" DataFormatString="{0:#,##0}"
                                            SortExpression="price">
                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="quantity" HeaderText="จำนวน" Visible="True" DataFormatString="{0:#,##0}"
                                            SortExpression="quantity">
                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="amount" HeaderText="มูลค่าการซื้อขาย" DataFormatString="{0:#,##0}"
                                            SortExpression="amount">
                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="payment_duedate" HeaderText="วันชำระเงิน" DataFormatString="{0:dd/MM/yyyy}"
                                            SortExpression="payment_duedate">
                                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="delivery_date" HeaderText="วันส่งมอบสินค้า" DataFormatString="{0:dd/MM/yyyy}"
                                            SortExpression="delivery_date">
                                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                        </asp:BoundField>
                                    </Columns>
                                    <FooterStyle BackColor="Gray" ForeColor="White" Font-Bold="true" />
                                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                                    <AlternatingRowStyle BackColor="Gainsboro" />
                                </asp:GridView>
                                <asp:ObjectDataSource ID="objSrcTicket96" runat="server" OldValuesParameterFormatString="original_{0}"
                                    SelectMethod="getTicketPortfolioByCust_id" TypeName="clsDB">
                                    <SelectParameters>
                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="cust_id" Type="String" />
                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="gold_type" Type="String" />
                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="isHistory" Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </fieldset>
                            <fieldset>
                                <legend class="topic">Purity 96.50(กรัม)</legend>
                                <asp:LinkButton ID="linkExport96G" runat="server">Export</asp:LinkButton>
                                <asp:GridView
                                    ID="gvTicket96G" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                    Width="100%" BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                    CellPadding="3" DataSourceID="objSrcTicket96G" DataKeyNames="ticket_id" ShowFooter="True">
                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                    <Columns>
                                        <asp:BoundField DataField="ticket_date" HeaderText="วันที่ซื้อขาย" DataFormatString="{0:dd/MM/yyyy}"
                                            SortExpression="ticket_date">
                                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="created_date" HeaderText="เวลา" DataFormatString="{0:hh:mm tt}"
                                            SortExpression="created_date">
                                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="ticket_ref" SortExpression="ticket_id">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="link" runat="server" Target="_blank"><%#Eval("ticket_id")%></asp:HyperLink></ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="type" HeaderText="คำสั่ง" SortExpression="type">
                                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="price" HeaderText="ราคา" DataFormatString="{0:#,##0}"
                                            SortExpression="price">
                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="quantity" HeaderText="จำนวน" Visible="True" DataFormatString="{0:#,##0}"
                                            SortExpression="quantity">
                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="amount" HeaderText="มูลค่าการซื้อขาย" DataFormatString="{0:#,##0}"
                                            SortExpression="amount">
                                            <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="payment_duedate" HeaderText="วันชำระเงิน" DataFormatString="{0:dd/MM/yyyy}"
                                            SortExpression="payment_duedate">
                                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="delivery_date" HeaderText="วันส่งมอบสินค้า" DataFormatString="{0:dd/MM/yyyy}"
                                            SortExpression="delivery_date">
                                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                        </asp:BoundField>
                                    </Columns>
                                    <FooterStyle BackColor="Gray" ForeColor="White" Font-Bold="true" />
                                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                                    <AlternatingRowStyle BackColor="Gainsboro" />
                                </asp:GridView>
                                <asp:ObjectDataSource ID="objSrcTicket96G" runat="server" OldValuesParameterFormatString="original_{0}"
                                    SelectMethod="getTicketPortfolioByCust_id" TypeName="clsDB">
                                    <SelectParameters>
                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="cust_id" Type="String" />
                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="gold_type" Type="String" />
                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="isHistory" Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </fieldset>
                            <fieldset>
                                <legend class="topic">Purity 99.99</legend>
                                <asp:LinkButton ID="linkExport99" runat="server">Export</asp:LinkButton><asp:GridView
                                    ID="gvTicket99" runat="server" AllowSorting="True" ShowFooter="True" AutoGenerateColumns="False"
                                    BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                    CellPadding="3" DataKeyNames="ticket_id" DataSourceID="objSrcTicket99" Width="100%">
                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                    <Columns>
                                        <asp:BoundField DataField="ticket_date" DataFormatString="{0:dd/MM/yyyy}" HeaderText="วันที่ซื้อขาย"
                                            SortExpression="ticket_date">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="created_date" DataFormatString="{0:hh:mm tt}" HeaderText="เวลา"
                                            SortExpression="created_date">
                                            <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="ticket_ref" SortExpression="ticket_id">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="link" runat="server" Target="_blank"><%#Eval("ticket_id")%></asp:HyperLink></ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="type" HeaderText="คำสั่ง" SortExpression="type">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="price" DataFormatString="{0:#,##0}" HeaderText="ราคา"
                                            SortExpression="price">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="quantity" DataFormatString="{0:#,##0}" HeaderText="จำนวน"
                                            Visible="True" SortExpression="quantity">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="amount" DataFormatString="{0:#,##0}" HeaderText="มูลค่าการซื้อขาย"
                                            SortExpression="amount">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="payment_duedate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="วันชำระเงิน"
                                            SortExpression="payment_duedate">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="delivery_date" DataFormatString="{0:dd/MM/yyyy}" HeaderText="วันส่งมอบสินค้า"
                                            SortExpression="delivery_date">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                    </Columns>
                                    <FooterStyle BackColor="Gray" ForeColor="White" Font-Bold="True" />
                                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                                    <AlternatingRowStyle BackColor="Gainsboro" />
                                </asp:GridView>
                                <asp:ObjectDataSource ID="objSrcTicket99" runat="server" OldValuesParameterFormatString="original_{0}"
                                    SelectMethod="getTicketPortfolioByCust_id" TypeName="clsDB">
                                    <SelectParameters>
                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="cust_id" Type="String" />
                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="gold_type" Type="String" />
                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="isHistory" Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </fieldset>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="linkExport99" />
                            <asp:PostBackTrigger ControlID="linkExport96" />
                        </Triggers>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="tpCash" runat="server" HeaderText="Cash and Gold Movement">
                <ContentTemplate>
                    <asp:UpdatePanel ID="udpCashMovement" runat="server">
                        <ContentTemplate>
                            <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
                                <tr>
                                    <td style="width: 50%" valign="top">
                                        <fieldset>
                                            <legend class="topic">Cash</legend>
                                            <asp:LinkButton ID="linkExportCashDep" runat="server">Export</asp:LinkButton><br>
                                            <asp:GridView ID="gvCashTrans1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                                CellPadding="3" DataSourceID="objSrcCashTrans1" GridLines="Vertical" Width="100%">
                                                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Ref_No" SortExpression="Ref_No">
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="link" runat="server" Target="_blank"><%#Eval("ref_no")%></asp:HyperLink></ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="datetime" DataFormatString="{0:dd/MM/yyyy}" HeaderText="datetime"
                                                        SortExpression="datetime">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="deposit" HeaderText="ฝาก" SortExpression="deposit" DataFormatString="{0:#,##0}">
                                                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="withdraw" HeaderText="ถอน" SortExpression="withdraw" DataFormatString="{0:#,##0}">
                                                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="สุทธิ">
                                                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                                                <AlternatingRowStyle BackColor="Gainsboro" />
                                            </asp:GridView>
                                            <asp:ObjectDataSource ID="objSrcCashTrans1" runat="server" OldValuesParameterFormatString="original_{0}"
                                                SelectMethod="getCashCreditPortfolioByCust_id" TypeName="clsDB">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="hdfCust_id" Name="cust_id" PropertyName="Value"
                                                        Type="String" ConvertEmptyStringToNull="False" />
                                                    <asp:Parameter DefaultValue="cash" Name="type" Type="String" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                        </fieldset>
                                    </td>
                                    <td style="width: 50%" valign="top">
                                        <fieldset>
                                            <legend class="topic">Gold</legend>
                                            <asp:LinkButton ID="linkExportGoldDep" runat="server">Export</asp:LinkButton><br>
                                            <asp:GridView ID="gvCashTrans2" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                                CellPadding="3" DataSourceID="objSrcCashTrans2" GridLines="Vertical" Width="100%">
                                                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                                <Columns>
                                                    <asp:TemplateField HeaderText="Ref_No" SortExpression="Ref_No">
                                                        <ItemTemplate>
                                                            <asp:HyperLink ID="link" runat="server" Target="_blank"><%#Eval("ref_no")%></asp:HyperLink></ItemTemplate>
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="datetime" DataFormatString="{0:dd/MM/yyyy}" HeaderText="datetime"
                                                        SortExpression="datetime">
                                                        <ItemStyle HorizontalAlign="Center" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="gold_type_id" HeaderText="Purity" SortExpression="gold_type_id">
                                                        <ItemStyle HorizontalAlign="Center" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="deposit" HeaderText="ฝาก" SortExpression="deposit" DataFormatString="{0:#,##0}">
                                                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField DataField="withdraw" HeaderText="ถอน" SortExpression="withdraw" DataFormatString="{0:#,##0}">
                                                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="สุทธิ96">
                                                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                    </asp:BoundField>
                                                    <asp:BoundField HeaderText="สุทธิ99">
                                                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                                    </asp:BoundField>
                                                </Columns>
                                                <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                                <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                                                <AlternatingRowStyle BackColor="Gainsboro" />
                                            </asp:GridView>
                                            <asp:ObjectDataSource ID="objSrcCashTrans2" runat="server" OldValuesParameterFormatString="original_{0}"
                                                SelectMethod="getCashCreditPortfolioByCust_id" TypeName="clsDB">
                                                <SelectParameters>
                                                    <asp:ControlParameter ControlID="hdfCust_id" ConvertEmptyStringToNull="False" Name="cust_id"
                                                        PropertyName="Value" Type="String" />
                                                    <asp:Parameter DefaultValue="gold" Name="type" Type="String" />
                                                </SelectParameters>
                                            </asp:ObjectDataSource>
                                        </fieldset>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="linkExportCashDep" />
                            <asp:PostBackTrigger ControlID="linkExportGoldDep" />
                        </Triggers>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </cc1:TabPanel>
            <cc1:TabPanel ID="tpHistory" runat="server" HeaderText="Historical">
                <ContentTemplate>
                    <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                        <ContentTemplate>
                            <fieldset>
                                <legend class="topic">Purity 96.50</legend>
                                <asp:LinkButton ID="linkExportHis96" runat="server">Export</asp:LinkButton><br />
                                <asp:GridView ID="gvTicket96H" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                    BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                    CellPadding="3" DataKeyNames="ticket_id" DataSourceID="objSrcTicket96H" ShowFooter="True"
                                    Width="100%" AllowPaging="True" PageSize="20">
                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                    <Columns>
                                        <asp:BoundField DataField="ticket_date" DataFormatString="{0:dd/MM/yyyy}" HeaderText="วันที่ซื้อขาย"
                                            SortExpression="ticket_date">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="created_date" DataFormatString="{0:hh:mm tt}" HeaderText="เวลา"
                                            SortExpression="created_date">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="ticket_ref" SortExpression="ticket_id">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="link" runat="server" Target="_blank"><%#Eval("ticket_id")%></asp:HyperLink></ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="type" HeaderText="คำสั่ง" SortExpression="type">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="price" DataFormatString="{0:#,##0}" HeaderText="ราคา"
                                            SortExpression="price">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="quantity" DataFormatString="{0:#,##0}" HeaderText="จำนวน"
                                            Visible="True" SortExpression="quantity">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="amount" DataFormatString="{0:#,##0}" HeaderText="มุลค่าการซื้อขาย"
                                            SortExpression="amount">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="payment_duedate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="วันชำระเงิน"
                                            SortExpression="payment_duedate">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="delivery_date" DataFormatString="{0:dd/MM/yyyy}" HeaderText="วันส่งมอบสินค้า"
                                            SortExpression="delivery_date">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                    </Columns>
                                    <FooterStyle BackColor="Gray" Font-Bold="true" ForeColor="White" />
                                    <PagerStyle BackColor="#999999" Font-Size="Medium" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                                    <AlternatingRowStyle BackColor="Gainsboro" />
                                </asp:GridView>
                                <asp:ObjectDataSource ID="objSrcTicket96H" runat="server" OldValuesParameterFormatString="original_{0}"
                                    SelectMethod="getTicketPortfolioByCust_id" TypeName="clsDB">
                                    <SelectParameters>
                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="cust_id" Type="String" />
                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="gold_type" Type="String" />
                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="isHistory" Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </fieldset>
                            <p>
                            </p>
                            <fieldset>
                                <legend class="topic">Purity 96.50(กรัม)</legend>
                                <asp:LinkButton ID="linkExportHis96G" runat="server">Export</asp:LinkButton><br />
                                <asp:GridView ID="gvTicket96GH" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                    BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                    CellPadding="3" DataKeyNames="ticket_id" DataSourceID="objSrcTicket96GH" ShowFooter="True"
                                    Width="100%" AllowPaging="True" PageSize="20">
                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                    <Columns>
                                        <asp:BoundField DataField="ticket_date" DataFormatString="{0:dd/MM/yyyy}" HeaderText="วันที่ซื้อขาย"
                                            SortExpression="ticket_date">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="created_date" DataFormatString="{0:hh:mm tt}" HeaderText="เวลา"
                                            SortExpression="created_date">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="ticket_ref" SortExpression="ticket_id">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="link" runat="server" Target="_blank"><%#Eval("ticket_id")%></asp:HyperLink></ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="type" HeaderText="คำสั่ง" SortExpression="type">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="price" DataFormatString="{0:#,##0}" HeaderText="ราคา"
                                            SortExpression="price">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="quantity" DataFormatString="{0:#,##0}" HeaderText="จำนวน"
                                            Visible="True" SortExpression="quantity">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="amount" DataFormatString="{0:#,##0}" HeaderText="มุลค่าการซื้อขาย"
                                            SortExpression="amount">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="payment_duedate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="วันชำระเงิน"
                                            SortExpression="payment_duedate">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="delivery_date" DataFormatString="{0:dd/MM/yyyy}" HeaderText="วันส่งมอบสินค้า"
                                            SortExpression="delivery_date">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                    </Columns>
                                    <FooterStyle BackColor="Gray" Font-Bold="true" ForeColor="White" />
                                    <PagerStyle BackColor="#999999" Font-Size="Medium" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                                    <AlternatingRowStyle BackColor="Gainsboro" />
                                </asp:GridView>
                                <asp:ObjectDataSource ID="objSrcTicket96GH" runat="server" OldValuesParameterFormatString="original_{0}"
                                    SelectMethod="getTicketPortfolioByCust_id" TypeName="clsDB">
                                    <SelectParameters>
                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="cust_id" Type="String" />
                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="gold_type" Type="String" />
                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="isHistory" Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </fieldset>
                            <p>
                            </p>
                            <fieldset>
                                <legend class="topic">Purity 99.99</legend>
                                <asp:LinkButton ID="linkExportHis99" runat="server">Export</asp:LinkButton><br />
                                <asp:GridView ID="gvTicket99H" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                    BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                    CellPadding="3" DataKeyNames="ticket_id" DataSourceID="objSrcTicket99H" ShowFooter="True"
                                    Width="100%" AllowPaging="True" PageSize="20">
                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                    <Columns>
                                        <asp:BoundField DataField="ticket_date" DataFormatString="{0:dd/MM/yyyy}" HeaderText="วันที่ซื้อขาย"
                                            SortExpression="ticket_date">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="created_date" DataFormatString="{0:hh:mm tt}" HeaderText="เวลา"
                                            SortExpression="created_date">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:TemplateField HeaderText="ticket_ref" SortExpression="ticket_id">
                                            <ItemTemplate>
                                                <asp:HyperLink ID="link" runat="server" Target="_blank"><%#Eval("ticket_id")%></asp:HyperLink></ItemTemplate>
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="type" HeaderText="คำสั่ง" SortExpression="type">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="price" DataFormatString="{0:#,##0}" HeaderText="ราคา"
                                            SortExpression="price">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="quantity" DataFormatString="{0:#,##0}" HeaderText="จำนวน"
                                            Visible="True" SortExpression="quantity">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="amount" DataFormatString="{0:#,##0}" HeaderText="มุลค่าการซื้อขาย"
                                            SortExpression="amount">
                                            <ItemStyle HorizontalAlign="Right" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="payment_duedate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="วันชำระเงิน"
                                            SortExpression="payment_duedate">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                        <asp:BoundField DataField="delivery_date" DataFormatString="{0:dd/MM/yyyy}" HeaderText="วันส่งมอบสินค้า"
                                            SortExpression="delivery_date">
                                            <ItemStyle HorizontalAlign="Center" />
                                        </asp:BoundField>
                                    </Columns>
                                    <FooterStyle BackColor="Gray" Font-Bold="True" ForeColor="White" />
                                    <PagerStyle BackColor="#999999" Font-Size="Medium" HorizontalAlign="Center" />
                                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                    <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                                    <AlternatingRowStyle BackColor="Gainsboro" />
                                </asp:GridView>
                                <asp:ObjectDataSource ID="objSrcTicket99H" runat="server" OldValuesParameterFormatString="original_{0}"
                                    SelectMethod="getTicketPortfolioByCust_id" TypeName="clsDB">
                                    <SelectParameters>
                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="cust_id" Type="String" />
                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="gold_type" Type="String" />
                                        <asp:Parameter ConvertEmptyStringToNull="False" Name="isHistory" Type="String" />
                                    </SelectParameters>
                                </asp:ObjectDataSource>
                            </fieldset>
                        </ContentTemplate>
                        <Triggers>
                            <asp:PostBackTrigger ControlID="linkExportHis99" />
                            <asp:PostBackTrigger ControlID="linkExportHis96" />
                            <asp:PostBackTrigger ControlID="linkExportHis96G" />
                        </Triggers>
                    </asp:UpdatePanel>
                </ContentTemplate>
            </cc1:TabPanel>
        </cc1:TabContainer>
    </div>
    <div class="demobottom">
    </div>
</asp:Content>
