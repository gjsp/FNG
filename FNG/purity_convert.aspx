<%@ Page Title="" Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false"
    CodeFile="purity_convert.aspx.vb" Inherits="purity_convert" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register Src="user_control/ucPortFolio.ascx" TagName="ucPortFolio" TagPrefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
        function pageLoad() {
            $get('<%=txtCustName.ClientId %>').focus();
        }
        function initialResult() {
            var ddlPF = $get('<%=ddlPurityFrom.ClientId %>');
            var ddlPT = $get('<%=ddlPurityTo.ClientId %>');
            var lblRS = $get('<%=lblResult.ClientId %>');
            var btnCV = $get('<%=btnCV.ClientId %>');
            var btnSave = $get('<%=btnSave.ClientId %>');
            //btnCV.disabled = '';
            lblRS.innerHTML = '';
            //btnSave.disabled = 'disabled';
//            if (ddlPF.selectedIndex == ddlPT.selectedIndex) {
//                btnCV.disabled = 'disabled';
//            }
        }
        //เวลากดแล้วมันจะ disabled ตลอด ต้องให้ disabled = '' 
        function validateCV(type) {
            //$get('<%=btnCV.ClientId %>').disabled = '';
            if (type == 'q') {
                alert('โปรดใส่จำนวนทอง');
            } else if(type=='d') {
                alert('โปรดเลือกประเภททองที่ต่างกัน');
            }
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea">
        <div class="demoheading">
            Convert Gold 96.50</div>
        <div style="margin-left: 1em">
            <asp:TextBox ID="txtCustName" Width="200px" runat="server" />
            <ajaxToolkit:AutoCompleteExtender ID="txtCustName_AutoCompleteExtender" runat="server"
                DelimiterCharacters="" CompletionInterval="300" Enabled="True" ServicePath="~/gtc.asmx"
                UseContextKey="True" CompletionSetCount="20" OnClientItemSelected="onCustNameAutoCompleteClick"
                TargetControlID="txtCustName" ServiceMethod="getCust_nameList">
            </ajaxToolkit:AutoCompleteExtender>
            <asp:HiddenField ID="hdfCust_id" runat="server" />
        </div>
        <span style="display: none">
            <asp:Button ID="btnSearch" runat="server" Text="Search" />
        </span>
        <uc1:ucPortFolio ID="ucPortFolio1" runat="server" />
        <br />
        <br />
        <asp:Panel ID="pn" runat="server">
            <asp:UpdatePanel ID="up" runat="server">
                <ContentTemplate>
                    <div>
                        <table cellpadding="2" cellspacing="2">
                            <tr>
                                <td align="right">
                                    Convert Gold From :
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlPurityFrom" runat="server" Width="80px">
                                        <asp:ListItem Text="บาท" Value="96"></asp:ListItem>
                                        <asp:ListItem Text="กรัม" Value="96G"></asp:ListItem>
                                        <asp:ListItem Text="มินิ" Value="96M"></asp:ListItem>
                                    </asp:DropDownList>
                                    &nbsp; To &nbsp;<asp:DropDownList ID="ddlPurityTo" runat="server" Width="80px">
                                        <asp:ListItem Text="บาท" Value="96"></asp:ListItem>
                                        <asp:ListItem Text="กรัม" Value="96G"></asp:ListItem>
                                        <asp:ListItem Text="มินิ" Value="96M"></asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td>
                                    &nbsp;<asp:Button ID="btnCV" runat="server" Text="Convert" Width="80px" />
                                    &nbsp;
                                </td>
                                <td style="height: 30px">
                                 <asp:UpdateProgress ID="upgSearch" runat="server">
                                    <ProgressTemplate>
                                        <div align="left" style="margin-left: 1cm; overflow: auto">
                                            <img alt="" src="images/loading.gif" />
                                            Loading...
                                        </div>
                                    </ProgressTemplate>
                                </asp:UpdateProgress>
                                </td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Quantity :
                                </td>
                                <td>
                                    <asp:TextBox ID="txtQuan" runat="server" Width="100px" MaxLength="10" />
                                </td>
                                <td>
                                    &nbsp;</td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td align="right">
                                    Result :
                                </td>
                                <td><div style="width:100px;text-align:right">
                                <asp:Label ID="lblResult" runat="server" CssClass="lbl" Text=""></asp:Label>
                                </div>
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                            <tr>
                                <td align="right">
                                    &nbsp;
                                </td>
                                <td>
                                    <asp:Button ID="btnSave" runat="server" Text="Save" Width="80px" />
                                </td>
                                <td>
                                    &nbsp;
                                </td>
                                <td>
                                    &nbsp;</td>
                            </tr>
                        </table>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
    <div class="demobottom">
    </div>
</asp:Content>
