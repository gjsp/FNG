<%@ Page Language="VB" AutoEventWireup="false" CodeFile="tran_auto.aspx.vb" Inherits="trade_admin_tran_auto"
    EnableEventValidation="false" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="../../button/css/buttonPro.css" rel="stylesheet" type="text/css" />
    <link href="style.css" rel="stylesheet" type="text/css" />
    <script src="../js/function.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function Check() {
            if ($get('hdfIsRealtime').value != 'n') {
                PageMethods.getQuantityAcceptAdap($get('hdfType').value, $get('hdfPurity').value, OnSucceeded, OnFailed);
            }
        }
        function OnSucceeded(result, userContext, methodName) {
            var trade_max_id = result;
            var pre_trade_max_id = $get('hdfQuanNow').value;

            if ($get('hdfQuanNow').value != result) {
                $get('hdfQuanNow').value = result;
                __doPostBack('upGrid', '');
            }
            else {
                setTimeout("Check()", 4000);
            }
        }
        function pageLoad() {
            setTimeout("Check()", 4000);
        }
        function refreshGrid() {
            __doPostBack('upGrid', '');
        }
        function setIframeHeight(name, h) {
            top.window.document.getElementById(name).style.height = h + "px";
        }

        function OnFailed(error, userContext, methodName) { }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager runat="server" ID="SMTran" EnablePageMethods="true">
    </asp:ScriptManager>
    <table cellpadding="2" cellspacing="2">
        <tr valign="top">
            <td>
                <div>
                    <asp:UpdatePanel ID="upGrid" runat="server">
                        <ContentTemplate>
                            <asp:UpdateProgress ID="UpdateProg1" runat="server">
                                <ProgressTemplate>
                                    <div id="Div1" align="center" valign="middle" runat="server" style="position: absolute;
                                        left: 20%; padding: 10px 10px 10px 10px; visibility: visible; border-color: silver;
                                        border-style: solid; border-width: 1px; background-color: White;">
                                        <img src="../img/indicator.gif" style="vertical-align: middle" alt="Processing" />
                                        Loading ... &nbsp;
                                    </div>
                                </ProgressTemplate>
                            </asp:UpdateProgress>
                            <asp:GridView ID="gvTran" runat="server" AutoGenerateColumns="False" BackColor="White"
                                BorderColor="#999999" BorderStyle="None" BorderWidth="0px" CellPadding="3" DataSourceID="objSrcTran"
                                DataKeyNames="price">
                                <Columns>
                                    <asp:BoundField HeaderText="No."></asp:BoundField>
                                    <asp:BoundField DataField="price" HeaderText="Price" DataFormatString="{0:#,##0}">
                                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:BoundField DataField="quantity" HeaderText="Quantity" DataFormatString="{0:#,##0}">
                                        <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                    </asp:BoundField>
                                    <asp:TemplateField>
                                        <ItemTemplate>
                                            <asp:Button ID="btnAccept" runat="server" Text="Clear" BorderColor="Blue" BorderStyle="Groove"
                                                OnClick="btnAccept_Click" />
                                        </ItemTemplate>
                                        <ItemStyle HorizontalAlign="Center" Width="40px" />
                                    </asp:TemplateField>
                                </Columns>
                                <HeaderStyle BackColor="#333333" ForeColor="White" />
                                <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                <AlternatingRowStyle BackColor="Gainsboro" />
                            </asp:GridView>
                            <asp:ObjectDataSource ID="objSrcTran" runat="server" SelectMethod="getTranAutoAccept"
                                TypeName="clsMain" OldValuesParameterFormatString="original_{0}">
                                <SelectParameters>
                                    <asp:Parameter ConvertEmptyStringToNull="False" Name="type" Type="String" />
                                    <asp:Parameter ConvertEmptyStringToNull="False" Name="purity" Type="String" />
                                </SelectParameters>
                            </asp:ObjectDataSource>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </td>
            <td>
                <div>
                    <asp:UpdatePanel ID="upSave" runat="server">
                        <ContentTemplate>
                            <table border="0" cellpadding="3" cellspacing="3">
                                <tr>
                                    <td align="right">
                                        Auto Accept Max (<asp:Label ID="lblUnit1" runat="server" ></asp:Label>)&nbsp;&nbsp;
                                    </td>
                                    <td>&nbsp;&nbsp;
                                        <asp:Label ID="lblQMax" Font-Bold="true" runat="server"></asp:Label>
                                        <asp:TextBox ID="txtQMax" runat="server" MaxLength="5" Width="40px" Visible="false"></asp:TextBox>
                                        <asp:RequiredFieldValidator ID="rfvMax" runat="server" ErrorMessage="*" ControlToValidate="txtQMax" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                    </td>
                                </tr>
                                <tr>
                                    <td align="right">
                                        No more Quantity/Transaction (<asp:Label ID="lblUnit2" runat="server" ></asp:Label>) &nbsp;&nbsp;
                                    </td>
                                    <td>&nbsp;&nbsp;
                                            <asp:Label ID="lblQPer" Font-Bold="true" runat="server"></asp:Label>
                                            <asp:TextBox ID="txtQPer" runat="server" MaxLength="5" Width="40px" Visible="false"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="rfvPer" runat="server" ErrorMessage="*" ControlToValidate="txtQPer" ValidationGroup="edit"></asp:RequiredFieldValidator>
                                            &nbsp;
                                            <asp:ImageButton ID="imgEdit" runat="server" ImageUrl="~/trade/admin/image/edit.png"
                                                Width="20px" Height="20px" />
                                            <asp:LinkButton ID="linkSave" runat="server" Text="Save" Font-Underline="false" Visible="false"  ValidationGroup="edit"></asp:LinkButton>
                                            &nbsp;
                                            <asp:LinkButton ID="linkCancel" runat="server" Text="Cancel" Font-Underline="false"
                                                Visible="false"></asp:LinkButton>
                                    </td>
                                </tr>
                            </table>
                        </ContentTemplate>
                    </asp:UpdatePanel>
                </div>
            </td>
        </tr>
    </table>
    <asp:HiddenField ID="hdfType" runat="server" />
    <asp:HiddenField ID="hdfPurity" runat="server" />
    <asp:HiddenField ID="hdfIsRealtime" runat="server" />
    <asp:HiddenField ID="hdfQuanNow" runat="server" />
    <asp:HiddenField ID="hdfMax" runat="server" />
    <asp:HiddenField ID="hdfPer" runat="server" />
    </form>
</body>
</html>
