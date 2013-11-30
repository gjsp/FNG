<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="rpt_bill.aspx.vb" Inherits="report_rpt_bill" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea">
         <CR:CrystalReportViewer ID="CRV" runat="server" AutoDataBind="true" ToolPanelView="None"
            DisplayStatusbar="False" EnableDrillDown="False" GroupTreeStyle-ShowLines="False"
            HasCrystalLogo="False" HasDrilldownTabs="False" HasDrillUpButton="False" HasPageNavigationButtons="True"
            HasToggleGroupTreeButton="False" HasExportButton="True" ReuseParameterValuesOnRefresh="True" ReportSourceID="CRS" />
        <CR:CrystalReportSource ID="CRS" runat="server">
            <Report FileName="rptBill.rpt">
                <DataSources>
                    <CR:DataSourceRef DataSourceID="ODS" TableName="rpt_ticket_order" />
                </DataSources>
            </Report>
        </CR:CrystalReportSource>
        <asp:ObjectDataSource ID="ODS" runat="server" SelectMethod="getTicketOrder" TypeName="clsDB"
            OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
                <asp:Parameter Name="refno" Type="String" ConvertEmptyStringToNull="False" />
                <asp:Parameter Name="view" Type="Boolean" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
