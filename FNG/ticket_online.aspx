<%@ Page Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false"
    CodeFile="ticket_online.aspx.vb" Inherits="ticket_online" Title="Ticket_online" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea">
        <CR:CrystalReportViewer ID="CRV" runat="server" AutoDataBind="true" ToolPanelView="None"
            DisplayStatusbar="False" EnableDrillDown="False" GroupTreeStyle-ShowLines="False"
            HasCrystalLogo="False" HasDrilldownTabs="False" HasDrillUpButton="False" HasPageNavigationButtons="False"
            HasToggleGroupTreeButton="False" HasExportButton="False" ReportSourceID="CRS"
            ReuseParameterValuesOnRefresh="True" />
        <CR:CrystalReportSource ID="CRS" runat="server">
            <Report FileName="report\rptTrade.rpt">
                <DataSources>
                    <CR:DataSourceRef DataSourceID="ODS" TableName="rpt_trade_online" />
                </DataSources>
            </Report>
        </CR:CrystalReportSource>
        <asp:ObjectDataSource ID="ODS" runat="server" SelectMethod="getTradeBuySellReport"
            TypeName="clsDB">
            <SelectParameters>
                <asp:Parameter ConvertEmptyStringToNull="False" Name="refno" Type="String" />
                <asp:Parameter ConvertEmptyStringToNull="False" Name="bill" Type="String" />
                <asp:Parameter ConvertEmptyStringToNull="False" Name="editType" Type="String" />
            </SelectParameters>
        </asp:ObjectDataSource>
    </div>
</asp:Content>
