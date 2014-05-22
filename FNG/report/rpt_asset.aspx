<%@ Page Title="" Language="VB" MasterPageFile="~/MasterPage.master" AutoEventWireup="false" CodeFile="rpt_asset.aspx.vb" Inherits="report_rpt_asset" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304"
    Namespace="CrystalDecisions.Web" TagPrefix="CR" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" Runat="Server">
 <div class="demoarea">
        <CR:CrystalReportViewer ID="CRV" runat="server" AutoDataBind="true" ToolPanelView="None"
            DisplayStatusbar="False" EnableDrillDown="False" GroupTreeStyle-ShowLines="False"
            HasCrystalLogo="False" HasDrilldownTabs="False" HasDrillUpButton="False" HasPageNavigationButtons="True"
            HasToggleGroupTreeButton="False" HasExportButton="True" ReuseParameterValuesOnRefresh="True" ReportSourceID="CRS" />
        <CR:CrystalReportSource ID="CRS" runat="server">
            <Report FileName="rptAsset.rpt">
                <DataSources>
                    <CR:DataSourceRef DataSourceID="ODS" TableName="rpt_asset" />
                </DataSources>
            </Report>
        </CR:CrystalReportSource>
        <asp:ObjectDataSource ID="ODS" runat="server" SelectMethod="getAssetReport"
            TypeName="clsDB" OldValuesParameterFormatString="original_{0}">
            <SelectParameters>
               <asp:Parameter Name="custId" Type="String" ConvertEmptyStringToNull="False" />
            </SelectParameters>
        </asp:ObjectDataSource>

        
    </div>
</asp:Content>

