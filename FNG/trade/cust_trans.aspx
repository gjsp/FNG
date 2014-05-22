<%@ Page Language="VB" AutoEventWireup="false" CodeFile="cust_trans.aspx.vb" Inherits="cust_trans"
    EnableEventValidation="false" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Transaction Data</title>
    <link href="style.css" rel="stylesheet" type="text/css" />
    <script src="js/function.js" type="text/javascript"></script>
    </head>
<body>

    <script language="javascript" type="text/javascript">
    function Check()
    {
        if($get('hdfIsRealtime').value!='n')
        {
            PageMethods.getTradeMaxId($get('hdfMode').value,OnSucceeded, OnFailed);
        }
    }
    function OnSucceeded(result, userContext, methodName)
    {
      var trade_max_id = parseInt(result);
      var pre_trade_max_id = parseInt($get('hdfTradeLogId').value); 

      if (trade_max_id > pre_trade_max_id)
      {
        $get('hdfTradeLogIdForGrid').value = pre_trade_max_id;//For Gridview to know new trade
        $get('hdfTradeLogId').value = trade_max_id;
        __doPostBack('upGrid', '');
      }
      else
      {
        setTimeout("Check()", 3000);
      }
    }
    function pageLoad()
    {
      setTimeout("Check()", 3000);
    }
    function refreshGrid()
    {
       __doPostBack('upGrid', '');
    }
    function setIframeHeight(name,h)
    {
      top.window.document.getElementById(name).style.height = h + "px";
    }

    function OnFailed(error, userContext, methodName) {}
    </script>

    <form id="form1" runat="server">
    <asp:ScriptManager ID="SM" EnablePageMethods="true" runat="server">
    </asp:ScriptManager>
    <div id="div_area">
        <asp:UpdateProgress ID="UpdateProg1" runat="server">
            <ProgressTemplate>
                <div align="center" valign="middle" runat="server" style="position: absolute; left: 43%;
                    padding: 10px 10px 10px 10px; visibility: visible; border-color: silver; border-style: solid;
                    border-width: 1px; background-color: White;">
                    <img src="img/indicator.gif" style="vertical-align: middle" alt="Processing" />
                    Loading ... &nbsp;
                </div>
            </ProgressTemplate>
        </asp:UpdateProgress>
        <div id="div_content">
            <div style="padding-bottom:5px;">
                <asp:LinkButton ID="linkExcel" runat="server" Text="Export"></asp:LinkButton>
            </div>
            <asp:UpdatePanel ID="upGrid" runat="server">
                <ContentTemplate>
                    <asp:GridView ID="gvTrade" runat="server" AutoGenerateColumns="False" BackColor="White"
                        BorderColor="#999999" BorderStyle="None" BorderWidth="0px" CellPadding="3" DataKeyNames="trade_id,type,price,gold_type_id"
                        Width="99%" AllowSorting="True" DataSourceID="objSrcTrade" 
                        AllowPaging="True" PageSize="20">
                        <Columns>
                            <asp:BoundField DataField="trade_id" HeaderText="Ref_No" SortExpression="trade_id">
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="leave_order" HeaderText="Mode" SortExpression="leave_order">
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="created_date" HeaderText="Deal Date" DataFormatString="{0:dd/MM/yyyy HH:mm}"
                                SortExpression="created_date">
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="type" HeaderText="Sell/Buy" SortExpression="type">
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="firstname" HeaderText="Customer Name" SortExpression="firstname">
                            </asp:BoundField>
                            <asp:BoundField DataField="ip" HeaderText="IP Address" SortExpression="ip">
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="gold_type_id" HeaderText="Purity" SortExpression="gold_type_id">
                                <ItemStyle Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="mode" HeaderText="Status" SortExpression="mode">
                                <ItemStyle HorizontalAlign="Center" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="price" HeaderText="Price" SortExpression="price" DataFormatString="{0:#,##0}">
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="quantity" HeaderText="Quantity" SortExpression="quantity"
                                DataFormatString="{0:#,##0}">
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="amount" HeaderText="Amount" SortExpression="amount" DataFormatString="{0:#,##0}">
                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                            </asp:BoundField>
                            <asp:BoundField DataField="modifier_date" 
                                DataFormatString="{0:dd/MM/yyyy HH:mm}" HeaderText="Filled Time" 
                                SortExpression="modifier_date" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnAccept99" runat="server" ImageUrl="admin/image/accept.png" OnClick="btnAccept99_Click"
                                        Width="18px" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="40px" />
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:ImageButton ID="btnReject99" runat="server" ImageUrl="admin/image/reject.png" OnClick="btnReject99_Click"
                                        Width="18px" />
                                </ItemTemplate>
                                <ItemStyle HorizontalAlign="Center" Width="40px" />
                            </asp:TemplateField>
                        </Columns>
                        <FooterStyle BackColor="#666666" ForeColor="White" />
                        <HeaderStyle BackColor="#333333" ForeColor="White" />
                        <PagerStyle HorizontalAlign="Center" Font-Size="Medium" />
                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                    </asp:GridView>
                    <asp:ObjectDataSource ID="objSrcTrade" runat="server" SelectMethod="getTradeByMode"
                        TypeName="clsMain" OldValuesParameterFormatString="original_{0}">
                        <SelectParameters>
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="mode" Type="String" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="type" Type="String" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="purity" Type="String" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="cust_id" Type="String" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="max_trade_id" Type="String" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="sortPrice" Type="String" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="period" Type="String" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="pDate1" Type="DateTime" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="pDate2" Type="DateTime" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="onlyLeave" Type="String" />
                            <asp:Parameter ConvertEmptyStringToNull="False" Name="sale_id" Type="String" />
                        </SelectParameters>
                    </asp:ObjectDataSource>
                    <asp:HiddenField ID="hdfMode" runat="server" />
                    <asp:HiddenField ID="hdfIsRealtime" runat="server" />
                    <asp:HiddenField ID="hdfTradeLogIdForGrid" runat="server" />
                    <asp:HiddenField ID="hdfConfirm" runat="server" />
                    <asp:HiddenField ID="hdfTradeLogId" runat="server" />
                    <asp:HiddenField ID="hdfSale_id" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </div>
    </div>
    </form>
</body>
</html>
