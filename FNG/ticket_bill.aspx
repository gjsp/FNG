<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="ticket_bill.aspx.vb" Inherits="ticket_bill" Title="Receipt" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea">
        <CR:CrystalReportViewer ID="CRV" runat="server" AutoDataBind="true" ToolPanelView="None"
            DisplayStatusbar="False" EnableDrillDown="False" GroupTreeStyle-ShowLines="False"
            HasCrystalLogo="False" HasDrilldownTabs="False" HasDrillUpButton="False" HasPageNavigationButtons="False"
            HasToggleGroupTreeButton="False" HasExportButton="False" ReuseParameterValuesOnRefresh="True" />
       
        <CR:CrystalReportSource ID="crsNB" runat="server">
            <Report FileName="report/rptNobill.rpt">
                <DataSources>
                    <CR:DataSourceRef DataSourceID="odsNB" TableName="rpt_ticket_order_nobill" />
                </DataSources>
            </Report>
        </CR:CrystalReportSource>
        <asp:ObjectDataSource ID="odsNB" runat="server" SelectMethod="getTicketOrderNobill"
            TypeName="clsDB" OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:Parameter Name="refno" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="preview" Type="Boolean" />
                <asp:Parameter Name="view" Type="Boolean" />
            </SelectParameters>
        </asp:ObjectDataSource>

        <CR:CrystalReportSource ID="crsSplit" runat="server">
            <Report FileName="report/rptNobill.rpt">
                <DataSources>
                    <CR:DataSourceRef DataSourceID="odsSplit" TableName="rpt_ticket_order_nobill" />
                </DataSources>
            </Report>
        </CR:CrystalReportSource>
        <asp:ObjectDataSource ID="odsSplit" runat="server" SelectMethod="getTicketOrderNobillSplit"
            TypeName="clsDB" OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:Parameter Name="sp_id" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="preview" Type="Boolean" />
            </SelectParameters>
        </asp:ObjectDataSource>


        <CR:CrystalReportSource ID="crsBill" runat="server">
            <Report FileName="report/rptBill.rpt">
                <DataSources>
                    <CR:DataSourceRef DataSourceID="odsBill" TableName="rpt_ticket_order" />
                </DataSources>
            </Report>
        </CR:CrystalReportSource>
        <asp:ObjectDataSource ID="odsBill" runat="server" SelectMethod="getTicketOrder" TypeName="clsDB"
            OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:Parameter Name="refno" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="view" Type="Boolean" />
            </SelectParameters>
        </asp:ObjectDataSource>
        <CR:CrystalReportSource ID="crsTrans" runat="server">
            <Report FileName="report/rptTran.rpt">
                <DataSources>
                    <CR:DataSourceRef DataSourceID="odsTrans" TableName="transfer" />
                </DataSources>
            </Report>
        </CR:CrystalReportSource>
        <asp:ObjectDataSource ID="odsTrans" runat="server" SelectMethod="getReportTrans"
            TypeName="clsDB" OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:Parameter Name="refno" Type="String" ConvertEmptyStringToNull="False" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
