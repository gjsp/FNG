<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false"
    CodeFile="customer_detail.aspx.vb" Inherits="customer_detail" Title="Customer Detail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style1
        {
            height: 25px;
        }
        .style3
        {
            height: 14px;
        }
        .style4
        {
            width: 80px;
        }
    </style>
    <script language="javascript" type="text/javascript">

        var _source;
        var _popup;

        function showConfirm(source) {
            this._source = source;
            this._popup = $find('mppUpdate');
            this._popup.show();

            $get('<%=txtPwd.ClientId %>').value = '';
            $get('<%=txtPwd.ClientId %>').focus();
        }
        function showConfirmReset(source) {
            this._source = source;
            this._popup = $find('mppReset');
            this._popup.show();

            $get('<%=txtPwdReset.ClientId %>').value = '';
            $get('<%=txtPwdReset.ClientId %>').focus();
        }

        function okClick() {

            if ($get('<%=txtPwd.ClientId %>').value == '') { $get('<%=txtPwd.ClientId %>').focus(); return };
            if ($get('<%=txtPwd.ClientId %>').value != $get('<%=hdfPwd.ClientId %>').value) {
                $get('<%=txtPwd.ClientId %>').focus();
                alert('Password Invalid.');
            }
            else {
                $get('<%=txtPwd.ClientId %>').value = '';
                this._popup.hide();
                __doPostBack(this._source.name, '');
            }
        }

        function okResetClick() {

            if ($get('<%=txtPwdReset.ClientId %>').value == '') { $get('<%=txtPwdReset.ClientId %>').focus(); return };
            if ($get('<%=txtPwdReset.ClientId %>').value != $get('<%=hdfPwd.ClientId %>').value) {
                $get('<%=txtPwdReset.ClientId %>').focus();
                alert('Password Invalid.');
            }
            else {
                $get('<%=txtPwdReset.ClientId %>').value = '';
                this._popup.hide();
                __doPostBack(this._source.name, '');

            }
        }

        function cancelClick() {
            this._popup.hide();
            this._source = null;
            this._popup = null;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea">
        <div class="demoheading">
            <table border="0" width="100%">
                <tr>
                    <td align="left" class="style1">
                        Customers
                        Detail</td>
                    <td align="right" class="style1">
                    </td>
                </tr>
            </table>
        </div>
        <asp:UpdatePanel ID="udpMain" runat="server">
            <ContentTemplate>
                <asp:Panel ID="pnMain" runat="server">
                    <table border="0" cellpadding="2">
                         <tr id="trFolio" style="display: block">
                            <td valign="top">
                                Link to :
                            </td>
                            <td>
                                <asp:LinkButton ID="linkPortfolio" runat="server">PortFolio</asp:LinkButton>
                            </td>
                        </tr>
                        <tr id="trTrade" style="display: block">
                            <td valign="top">
                                Trade Type :
                            </td>
                            <td>
                                <asp:Label ID="lblTradeType" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                            </td>
                        </tr>
                        <tr id="trRef" style="display: block">
                            <td valign="top">
                                Reference no :
                            </td>
                            <td valign="top">
                                <asp:Label ID="lblRefNo" runat="server" Font-Bold="True" ForeColor="Black"></asp:Label>
                                <asp:HiddenField ID="hdfUserId" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                Type :
                            </td>
                            <td valign="top">
                                <asp:RadioButtonList ID="rdoType" RepeatDirection="Horizontal" runat="server">
                                </asp:RadioButtonList>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                Title name :
                            </td>
                            <td valign="top">
                                <asp:DropDownList ID="ddlTitle" runat="server">
                                    <asp:ListItem>ร้าน</asp:ListItem>
                                    <asp:ListItem>นาย</asp:ListItem>
                                    <asp:ListItem>นาง</asp:ListItem>
                                    <asp:ListItem>นางสาว</asp:ListItem>
                                    <asp:ListItem>บริษัทจำกัด</asp:ListItem>
                                    <asp:ListItem>ห้างหุ้นส่วน</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                First name(Thai)* :
                            </td>
                            <td>
                                <asp:TextBox ID="txtFname" runat="server" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Last name(Thai):
                            </td>
                            <td>
                                <asp:TextBox ID="txtLname" runat="server" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                First name(Eng) :
                            </td>
                            <td>
                                <div>
                                    <asp:TextBox ID="txtFnameEng" runat="server" Width="200px" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Last name(Eng) :
                            </td>
                            <td>
                                <div>
                                    <asp:TextBox ID="txtLnameEng" runat="server" Width="200px" />
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                ID Card/เลขจดทะเบียน* :
                            </td>
                            <td>
                                <div>
                                   
                                    <asp:TextBox ID="txtIDCard" runat="server" Width="200px" MaxLength="13" />
                                    <asp:RegularExpressionValidator ID="revIDCard" runat="server" 
                                        ControlToValidate="txtIDCard" Display="Dynamic" 
                                        ErrorMessage="โปรดกรอกเฉพาะตัวเลขจำนวน 13 หลัก" ValidationExpression="\d{13}"></asp:RegularExpressionValidator>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Person Contact :
                            </td>
                            <td>
                                <asp:TextBox ID="txtPerson" runat="server" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                E-Mail* :
                            </td>
                            <td>
                                <asp:TextBox ID="txtEmail" runat="server" Width="200px" />
                                <%--<asp:RegularExpressionValidator ID="revEmail" runat="server" 
                                ControlToValidate="txtEmail"
                                ErrorMessage="Email format is invalid." 
                                ValidationExpression="\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*"></asp:RegularExpressionValidator>--%>
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Birth day/วันที่จดทะเบียน :
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlday" runat="server">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlMonth" runat="server">
                                </asp:DropDownList>
                                <asp:DropDownList ID="ddlYear" runat="server">
                                </asp:DropDownList>
                                &nbsp;(Day:Month:Year)
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Mobile Phone* :
                            </td>
                            <td>
                                <asp:TextBox ID="txtMobilePhone" runat="server" Width="200px" MaxLength="10" />
                                &nbsp;(Only Number
                                <%= clsManage.phoneDigit.ToString()%>
                                digit)
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Telephone :
                            </td>
                            <td>
                                <asp:TextBox ID="txtTel" runat="server" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td>
                                Fax :
                            </td>
                            <td>
                                <asp:TextBox ID="txtFax" runat="server" Width="200px" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                Address :
                            </td>
                            <td>
                                <asp:TextBox ID="txtAddress" runat="server" Width="400px" Height="80px" TextMode="MultiLine" />
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top">
                                Bank 1
                            </td>
                            <td>
                                <table cellpadding="2" cellspacing="0" width="300px">
                                    <tr>
                                        <td class="style4">
                                            Bank Name :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBankName1" runat="server" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style4">
                                            Account No :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBankAccNo1" runat="server" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style4">
                                            Name :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBankAccName1" runat="server" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style4">
                                            type :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBankAccType1" runat="server" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style4">
                                            Branch :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBankAccBranch1" runat="server" Width="150px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top">
                                Bank 2
                            </td>
                            <td>
                                <table cellpadding="2" cellspacing="0" width="300px">
                                    <tr>
                                        <td class="style4">
                                            Bank Name :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBankName2" runat="server" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style4">
                                            Account No :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBankAccNo2" runat="server" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style4">
                                            Name :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBankAccName2" runat="server" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style4">
                                            type :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBankAccType2" runat="server" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style4">
                                            Branch :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBankAccBranch2" runat="server" Width="150px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td style="vertical-align: top">
                                Bank 3
                            </td>
                            <td>
                                <table cellpadding="2" cellspacing="0" width="300px">
                                    <tr>
                                        <td class="style4">
                                            Bank Name :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBankName3" runat="server" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style4">
                                            Account No :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBankAccNo3" runat="server" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style4">
                                            Name :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBankAccName3" runat="server" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style4">
                                            type :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBankAccType3" runat="server" Width="150px" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td class="style4">
                                            Branch :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtBankAccBranch3" runat="server" Width="150px" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                Remark :
                            </td>
                            <td valign="top">
                                <asp:TextBox ID="txtRemark" runat="server" Width="400px" Height="80px" TextMode="MultiLine" />
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                Asset :
                            </td>
                            <td>
                                <table cellpadding="2" cellspacing="2" border="0">
                                    <tr id="trCash">
                                        <td align="right">
                                            เงินสด :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtCredit_cash" runat="server" />
                                            &nbsp;บาท
                                        </td>
                                    </tr>
                                    <tr id="tr96">
                                        <td align="right">
                                            ทองคำ 96.50 :
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtQuan96" runat="server" />
                                            &nbsp;บาท
                                        </td>
                                    </tr>
                                    <tr id="tr99N">
                                        <td align="right" class="style3">
                                            ทองคำ 99.99 :
                                        </td>
                                        <td class="style3">
                                            <asp:TextBox ID="txtQuan99N" runat="server" />
                                            &nbsp;kg
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" class="style3">
                                            Free Margin :</td>
                                        <td class="style3">
                                            <asp:TextBox ID="txtFreeMargin" runat="server" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right" class="style3">
                                            Unlimited Margin :</td>
                                        <td class="style3">
                                            <asp:CheckBox ID="cbUnlimitedMargin" runat="server" Text="" />
                                        </td>
                                    </tr>
                                    <tr id="trDep">
                                        <td align="right" class="style3">
                                            <asp:LinkButton ID="lblDeposit" runat="server">Doposit Data Customer</asp:LinkButton>
                                        </td>
                                        <td class="style3">
                                            &nbsp;
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                Trade Online :
                            </td>
                            <td>
                                <div style="margin-top: 5px; margin-bottom: 5px;">
                                    <asp:UpdateProgress ID="udPgs" runat="server">
                                        <ProgressTemplate>
                                            <div id="Div1" align="left" valign="middle" runat="server" style="position: absolute;
                                                left: 35%; padding: 10px 10px 10px 10px; visibility: visible; border-color: silver;
                                                border-style: solid; border-width: 1px; background-color: White;">
                                                <img src="images/loading.gif" style="vertical-align: middle" alt="Processing" />
                                                Loading ... &nbsp;
                                            </div>
                                        </ProgressTemplate>
                                    </asp:UpdateProgress>
                                    <asp:Button ID="btnReset" runat="server" OnClientClick="showConfirmReset(this); return false;"
                                        Text="Reset Username Password" Width="180px" /><br />
                                    <asp:CheckBox ID="cbHalt" runat="server" Text="Halt" Visible="false" />
                                    <asp:Panel ID="pnApprove" runat="server">
                                        <asp:CheckBox ID="cbApprove" runat="server" Text="Approve Trade Online" AutoPostBack="True" />
                                        <br />
                                        <asp:CheckBox ID="cbCreateTradeOnline" runat="server" AutoPostBack="True" Text="Create Customer Trade Online And Send E-Mail" />
                                        
                                    </asp:Panel>
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td valign="top">
                                &nbsp;</td>
                            <td>
                                <%--  <asp:LinkButton ID="linkReset" runat="server" OnClientClick="showConfirmReset(this); return false;"
                                Text="Reset Username and Password"></asp:LinkButton>--%>
                                <asp:CheckBox ID="cbVIP" runat="server" AutoPostBack="True" Text="VIP" />
                                <div>
                                    Discount
                                    <asp:TextBox ID="txtDiscount" runat="server" Width="100px"></asp:TextBox>
                                </div>
                            </td>
                        </tr>
                         <tr>
                             <td valign="top">&nbsp; </td>
                             <td><%--  <asp:LinkButton ID="linkReset" runat="server" OnClientClick="showConfirmReset(this); return false;"
                                Text="Reset Username and Password"></asp:LinkButton>--%>
                                 <ajaxToolkit:ModalPopupExtender ID="mppReset" runat="server" BackgroundCssClass="modalBackground" BehaviorID="mppReset" CancelControlID="btnNoReset" OkControlID="btnOkReset" OnCancelScript="cancelClick();" OnOkScript="okResetClick();" PopupControlID="Panel3" TargetControlID="Panel3">
                                 </ajaxToolkit:ModalPopupExtender>
                                 <asp:Panel ID="Panel3" runat="server" CssClass="modalPopup" DefaultButton="btnOkReset" Style="display: none">
                                     <asp:Panel ID="Panel4" runat="server" Style="cursor: move; background-color: #DDDDDD;
                                        border: solid 1px Gray; color: Black">
                                         <div style="text-align: center">
                                             <p>
                                                 <h4>Do you want to Reset Username and Password?</h4>
                                             </p>
                                             <p id="p1" style="text-align: justify">
                                             </p>
                                             <br />
                                             <div>
                                                 <h4>Please Enter Password.</h4>
                                                 <asp:TextBox ID="txtPwdReset" runat="server" TextMode="Password"></asp:TextBox>
                                                 <asp:RequiredFieldValidator ID="rfvPwdReset" runat="server" ControlToValidate="txtPwdReset" ErrorMessage="*" ValidationGroup="mppReset"></asp:RequiredFieldValidator>
                                             </div>
                                         </div>
                                     </asp:Panel>
                                     <div>
                                         <p style="text-align: center;">
                                             <asp:Button ID="btnOkReset" runat="server" Text="Yes" ValidationGroup="mppReset" Width="50px" />
                                             <asp:Button ID="btnNoReset" runat="server" Text="No" Width="50px" />
                                         </p>
                                     </div>
                                 </asp:Panel>
                                 <br />
                             </td>
                         </tr>
                        <tr>
                            <td valign="top">
                                &nbsp;
                            </td>
                            <td valign="top">
                                <asp:Button ID="btnSave" runat="server" Text="Save" />
                                <asp:Button ID="btnUpdate" runat="server" OnClientClick="showConfirm(this); return false;"
                                    Text="Update" />
                                &nbsp;<input id="btnBack" onclick="location.href='customers.aspx';" type="button"
                                    value="Back" />
                                <asp:HiddenField ID="hdfPwd" runat="server" />
                                <ajaxToolkit:ModalPopupExtender ID="mppUpdate" runat="server" BackgroundCssClass="modalBackground"
                                    BehaviorID="mppUpdate" CancelControlID="btnNo" OkControlID="btnOk" OnCancelScript="cancelClick();"
                                    OnOkScript="okClick();" PopupControlID="Panel1" TargetControlID="Panel1">
                                </ajaxToolkit:ModalPopupExtender>
                                <asp:Panel ID="Panel1" runat="server" CssClass="modalPopup" Style="display: none" DefaultButton="btnOk">
                                    <asp:Panel ID="Panel2" runat="server" Style="cursor: move; background-color: #DDDDDD;
                                        border: solid 1px Gray; color: Black">
                                        <div style="text-align: center">
                                            <p>
                                                <h4>
                                                    Do you want to update Customer Detail.</h4>
                                            </p>
                                            <p id="pContent" style="text-align: justify">
                                            </p>
                                            <br />
                                            <div>
                                                <h4>
                                                    Please Enter Password.</h4>
                                                <asp:TextBox ID="txtPwd" runat="server" TextMode="Password"></asp:TextBox>
                                                <asp:RequiredFieldValidator ID="rfvPwd" runat="server" ControlToValidate="txtPwd"
                                                    ErrorMessage="*" ValidationGroup="mppPwd"></asp:RequiredFieldValidator>
                                            </div>
                                        </div>
                                    </asp:Panel>
                                    <div>
                                        <p style="text-align: center;">
                                            <asp:Button ID="btnOk" runat="server" Text="Yes" ValidationGroup="mppPwd" Width="50px" />
                                            <asp:Button ID="btnNo" runat="server" Text="No" Width="50px" />
                                        </p>
                                    </div>
                                </asp:Panel>
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
    <div class="demobottom">
    </div>
</asp:Content>
