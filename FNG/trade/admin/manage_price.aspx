<%@ Page Language="VB" MasterPageFile="~/trade/admin/MasterPageAdmin.master" AutoEventWireup="false"
    CodeFile="manage_price.aspx.vb" Inherits="admin_manage_price" Title="Manage Price" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../js/function.js" type="text/javascript"></script>
    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">

        $(document).ready(function () {
            $('#<%= ddlSwep.ClientID %>').change(function () {
                $("#div_price_gcap").slideToggle("slow");
                $("#div_price2").slideToggle("slow");
                $("#div_time").slideToggle("slow");
            });
        });

        function addCommas(nStr) {
            nStr += '';
            x = nStr.split('.');
            x1 = x[0];
            x2 = x.length > 1 ? '.' + x[1] : '';
            var rgx = /(\d+)(\d{3})/;
            while (rgx.test(x1)) {
                x1 = x1.replace(rgx, '$1' + ',' + '$2');
            }
            return x1 + x2;
        }
        function changeBid(ctrl) {

        }

        function addBid(lv, plus, purity) {
            if (plus.value == '') return;
            if (purity == '99') {
                var bid = parseFloat($get('<%=txt99Bid1.clientId %>').value.replace(',', ''));
                var bidPlus = parseFloat(plus.value.replace(',', ''));

                if (lv == 2) {
                    $get('<%=txt99Bid2.clientId %>').value = addCommas((bid + bidPlus).toFixed(2));
                }
                if (lv == 3) {
                    $get('<%=txt99Bid3.clientId %>').value = addCommas((bid + bidPlus).toFixed(2));
                }
            }
            else {
                var bid = parseFloat($get('<%=txt96Bid1.clientId %>').value.replace(',', ''));
                var bidPlus = parseFloat(plus.value.replace(',', ''));
                if (lv == 1) {
                    bid = parseFloat($get('<%=txt96Ask1.clientId %>').value.replace(',', ''))
                    $get('<%=txt96Bid1.clientId %>').value = addCommas((bid + bidPlus).toFixed(2));
                }
                if (lv == 2) {
                    $get('<%=txt96Bid2.clientId %>').value = addCommas((bid + bidPlus).toFixed(2));
                }
                if (lv == 3) {
                    $get('<%=txt96Bid3.clientId %>').value = addCommas((bid + bidPlus).toFixed(2));
                }
            }



        }
        function addAsk(lv, plus, purity) {
            if (plus.value == '') return;
            if (purity == '99') {
                var ask = parseFloat($get('<%=txt99Ask1.clientId %>').value.replace(',', ''));
                var askPlus = parseFloat(plus.value.replace(',', ''));
                if (lv == 1) {
                    BidPlus = parseFloat($get('<%=txt99Bid1.clientId %>').value.replace(',', ''));
                    $get('<%=txt99Ask1.clientId %>').value = addCommas((BidPlus - askPlus).toFixed(2));
                }
                if (lv == 2) {
                    $get('<%=txt99Ask2.clientId %>').value = addCommas((ask - askPlus).toFixed(2));
                }
                if (lv == 3) {
                    $get('<%=txt99Ask3.clientId %>').value = addCommas((ask - askPlus).toFixed(2));
                }
            }
            else {
                var ask = parseFloat($get('<%=txt96Ask1.clientId %>').value.replace(',', ''));
                var askPlus = parseFloat(plus.value.replace(',', ''));

                if (lv == 2) {
                    $get('<%=txt96Ask2.clientId %>').value = addCommas((ask - askPlus).toFixed(2));
                }
                if (lv == 3) {
                    $get('<%=txt96Ask3.clientId %>').value = addCommas((ask - askPlus).toFixed(2));
                }
            }

        }
        $(document).ready(function () {
            $(".slide2").click(function () {
                $("#div_price2").slideToggle("slow");
                $(this).toggleClass("active");
                if ($("#imgPrice2").attr("src") == "image/expand.jpg") {
                    $("#imgPrice2").attr("src", "image/collapse.jpg");
                } else {
                    $("#imgPrice2").attr("src", "image/expand.jpg");
                }
                return false;
            });
        });

        $(document).ready(function () {
            $(".slide1").click(function () {
                $("#div_price1").slideToggle("slow");
                $(this).toggleClass("active");
                if ($("#imgPrice1").attr("src") == "image/expand.jpg") {
                    $("#imgPrice1").attr("src", "image/collapse.jpg");
                } else {
                    $("#imgPrice1").attr("src", "image/expand.jpg");
                }
                return false;
            });
        });

    </script>
    <script type="text/javascript">
        var _source;
        var _popup;

        function showConfirmPer(source) {
            this._source = source;
            this._popup = $find('mppPer');

            //  find the confirm ModalPopup and show it    
            this._popup.show();
            $get('<%=txtPwdPer.ClientId %>').value = '';
            $get('<%=txtPwdPer.ClientId %>').focus();
        }

        function okClickPer() {
            if ($get('<%=txtPwdPer.ClientId %>').value == '') { $get('<%=txtPwdPer.ClientId %>').focus(); return };
            if ($get('<%=txtPwdPer.ClientId %>').value != $get('<%=hdfPwd.ClientId %>').value) {
                $get('<%=txtPwdPer.ClientId %>').focus();
                alert('Password Invalid.');
            }
            else {
                //  find the confirm ModalPopup and hide it   
                $get('<%=txtPwdPer.ClientId %>').value = '';
                this._popup.hide();
                //  use the cached button as the postback source
                __doPostBack(this._source.name, '');
            }
        }

        function cancelClickPer() {
            //  find the confirm ModalPopup and hide it 
            this._popup.hide();
            //  clear the event source
            this._source = null;
            this._popup = null;
        }

        function setConfirmSave(msg, ctrl) {
            var flag;
            flag = confirm(msg);
            if (flag == 'False') { return false; }

            if (flag == true) {
                var bid = $get('<%=hdfBid99.ClientId %>').value.replace(',', '');
                var newbid = $get('<%=txt99Bid1.ClientId %>').value.replace(',', '');
                var per = $get('<%=txtMaxPer.ClientId %>').value;
                var bidper = parseFloat(bid) * (parseFloat(per) / 100);
                if (parseFloat(bid) == parseFloat(newbid)) {
                    flag = true;
                } else {
                    if (parseFloat(newbid) > parseFloat(bid)) {
                        if (parseFloat(newbid) > (parseFloat(bid) + parseFloat(bidper))) {
                            showConfirmPer(ctrl);
                            return false;
                        }
                    } else {
                        if (parseFloat(newbid) < (parseFloat(bid) - parseFloat(bidper))) {
                            showConfirmPer(ctrl);
                            return false;
                        }
                    }
                    flag = true;
                }
            }
            return flag;
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 1200px; margin: auto;">
        <fieldset class="k-fieldset" style="width: 98%;">
            <legend class="topic">Price</legend>
            <asp:UpdatePanel ID="udpPrice" runat="server">
                <ContentTemplate>
                    <div style="float: left; text-align: left; width: 50%; margin-top: 10px; margin-bottom: 10px">
                        <div style="text-align: left;">
                            <div style="float: left; width: 50px; text-align: left; font-weight: bold;" class="topic">
                                96.50</div>
                            <div style="float: left; width: 60px; text-align: right; color: #0066FF;">
                            </div>
                            <div style="float: left; width: 100px; text-align: right; margin-left: 5px;">
                            </div>
                            <div style="float: left; width: 60px; text-align: right; color: #FF6600;">
                            </div>
                            <div style="float: left; width: 100px; text-align: right; margin-left: 5px;">
                            </div>
                            <div style="clear: both;">
                            </div>
                        </div>
                        <div style="text-align: left; margin-top: 20px">
                            <div style="float: left; width: 50px; text-align: left; font-weight: bold;">
                                Level 1</div>
                            <div style="float: left; width: 60px; text-align: right; color: #0066FF;">
                                Bid :&nbsp;
                            </div>
                            <div style="float: left; width: 100px; text-align: right; margin-left: 5px;">
                                <asp:Label ID="lbl96Bid1" runat="server" Text="Label"></asp:Label>
                                THB
                            </div>
                            <div style="float: left; width: 60px; text-align: right; color: #FF6600;">
                                Ask :&nbsp;
                            </div>
                            <div style="float: left; width: 100px; text-align: right; margin-left: 5px;">
                                <asp:Label ID="lbl96Ask1" runat="server" Text="Label"></asp:Label>
                                THB
                            </div>
                            <div style="clear: both;">
                            </div>
                        </div>
                        <asp:Panel ID="pnLv1" runat="server" Visible="true">
                            <div style="text-align: left; margin-top: 5px;">
                                <div style="float: left; width: 50px; text-align: left; font-weight: bold;">
                                    Level 2</div>
                                <div style="float: left; width: 60px; text-align: right; color: #0066FF;">
                                    Bid :&nbsp;
                                </div>
                                <div style="float: left; width: 100px; text-align: right; margin-left: 5px;">
                                    <asp:Label ID="lbl96Bid2" runat="server" Text="Label"></asp:Label>
                                    THB
                                </div>
                                <div style="float: left; width: 60px; text-align: right; color: #FF6600;">
                                    Ask :&nbsp;
                                </div>
                                <div style="float: left; width: 100px; text-align: right; margin-left: 5px;">
                                    <asp:Label ID="lbl96Ask2" runat="server" Text="Label"></asp:Label>
                                    THB
                                </div>
                                <div style="clear: both;">
                                </div>
                            </div>
                            <div style="text-align: left; margin-top: 5px;">
                                <div style="float: left; width: 50px; text-align: left; font-weight: bold;">
                                    Level 3</div>
                                <div style="float: left; width: 60px; text-align: right; color: #0066FF;">
                                    Bid :&nbsp;
                                </div>
                                <div style="float: left; width: 100px; text-align: right; margin-left: 5px;">
                                    <asp:Label ID="lbl96Bid3" runat="server" Text="Label"></asp:Label>
                                    THB
                                </div>
                                <div style="float: left; width: 60px; text-align: right; color: #FF6600;">
                                    Ask :&nbsp;
                                </div>
                                <div style="float: left; width: 100px; text-align: right; margin-left: 5px;">
                                    <asp:Label ID="lbl96Ask3" runat="server" Text="Label"></asp:Label>
                                    THB
                                </div>
                                <div style="clear: both;">
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                    <div style="float: right; text-align: left; width: 50%; margin-top: 10px; margin-bottom: 10px">
                        <div style="text-align: left;">
                            <div style="float: left; width: 50px; text-align: left; font-weight: bold;" class="topic">
                                99.99</div>
                            <div style="float: left; width: 60px; text-align: right; color: #0066FF;">
                            </div>
                            <div style="float: left; width: 100px; text-align: right; margin-left: 5px;">
                            </div>
                            <div style="float: left; width: 60px; text-align: right; color: #FF6600;">
                            </div>
                            <div style="float: left; width: 100px; text-align: right; margin-left: 5px;">
                            </div>
                            <div style="clear: both;">
                            </div>
                        </div>
                        <div style="text-align: left; margin-top: 20px">
                            <div style="float: left; width: 50px; text-align: left; font-weight: bold;">
                                Level 1</div>
                            <div style="float: left; width: 60px; text-align: right; color: #0066FF;">
                                Bid :&nbsp;
                            </div>
                            <div style="float: left; width: 100px; text-align: right; margin-left: 5px;">
                                <asp:Label ID="lbl99Bid1" runat="server" Text="Label"></asp:Label>
                                THB
                            </div>
                            <div style="float: left; width: 60px; text-align: right; color: #FF6600;">
                                Ask :&nbsp;
                            </div>
                            <div style="float: left; width: 100px; text-align: right; margin-left: 5px;">
                                <asp:Label ID="lbl99Ask1" runat="server" Text="Label"></asp:Label>
                                THB
                            </div>
                            <div style="clear: both;">
                            </div>
                        </div>
                        <asp:Panel ID="pnLv2" runat="server" Visible="true">
                            <div style="text-align: left; margin-top: 5px;">
                                <div style="float: left; width: 50px; text-align: left; font-weight: bold;">
                                    Level 2</div>
                                <div style="float: left; width: 60px; text-align: right; color: #0066FF;">
                                    Bid :&nbsp;
                                </div>
                                <div style="float: left; width: 100px; text-align: right; margin-left: 5px;">
                                    <asp:Label ID="lbl99Bid2" runat="server" Text="Label"></asp:Label>
                                    THB
                                </div>
                                <div style="float: left; width: 60px; text-align: right; color: #FF6600;">
                                    Ask :&nbsp;
                                </div>
                                <div style="float: left; width: 100px; text-align: right; margin-left: 5px;">
                                    <asp:Label ID="lbl99Ask2" runat="server" Text="Label"></asp:Label>
                                    THB
                                </div>
                                <div style="clear: both;">
                                </div>
                            </div>
                            <div style="text-align: left; margin-top: 5px;">
                                <div style="float: left; width: 50px; text-align: left; font-weight: bold;">
                                    Level 3</div>
                                <div style="float: left; width: 60px; text-align: right; color: #0066FF;">
                                    Bid :&nbsp;
                                </div>
                                <div style="float: left; width: 100px; text-align: right; margin-left: 5px;">
                                    <asp:Label ID="lbl99Bid3" runat="server" Text="Label"></asp:Label>
                                    THB
                                </div>
                                <div style="float: left; width: 60px; text-align: right; color: #FF6600;">
                                    Ask :&nbsp;
                                </div>
                                <div style="float: left; width: 100px; text-align: right; margin-left: 5px;">
                                    <asp:Label ID="lbl99Ask3" runat="server" Text="Label"></asp:Label>
                                    THB
                                </div>
                                <div style="clear: both;">
                                </div>
                            </div>
                        </asp:Panel>
                    </div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </fieldset>
        <div id="div_time" style="display: block">
            <fieldset id="fsTime" class="k-fieldset" style="width: 98%; display: block">
                <legend class="topic">Inactive Pricing Time</legend>
                <iframe id="ifmTime" width="100%" height="30px" frameborder="0" scrolling="no" marginheight="0"
                    marginwidth="0"></iframe>
            </fieldset>
        </div>
        <fieldset class="k-fieldset" style="width: 98%; display: none;">
            <legend class="topic">
                <img alt="" id="imgPrice1" src="image/collapse.jpg" class="slide1" style="cursor: pointer" />
                Set Price</legend>
            <div id="div_price1">
                <iframe id="ifmTran_setting" width="100%" height="130px" frameborder="0" scrolling="no"
                    src="trans_setting.aspx?m=tran" marginheight="0" marginwidth="0"></iframe>
            </div>
        </fieldset>
        <table cellpadding="2" cellspacing="5" border="0">
            <tr>
                <td>
                    <asp:DropDownList ID="ddlSwep" runat="server">
                        <asp:ListItem Text="Price Spot" Selected="True"></asp:ListItem>
                        <asp:ListItem Text="Price Update"></asp:ListItem>
                    </asp:DropDownList>
                </td>
                <td>
                </td>
                <td>
                </td>
            </tr>
        </table>
        <fieldset id="fsPriceChange" class="k-fieldset" style="width: 98%;">
            <legend class="topic">Price From Trade Online</legend>
            <div id="div_price_gcap">
                <iframe id="ifmPriceChange" width="100%" height="300px" frameborder="0" scrolling="no"
                    marginheight="0" marginwidth="0" src="price_spot.aspx"></iframe>
                <asp:HiddenField ID="hdfKgBid99" runat="server" />
                <asp:HiddenField ID="hdfKgAsk99" runat="server" />
                <asp:HiddenField ID="hdfKgBid96" runat="server" />
                <asp:HiddenField ID="hdfKgAsk96" runat="server" />
            </div>
        </fieldset>
        <fieldset class="k-fieldset" style="width: 98%;">
            <legend class="topic">
                <img alt="" id="imgPrice2" src="image/collapse.jpg" class="slide2" style="cursor: pointer;display:none" />
                Update Price</legend>
            <asp:UpdateProgress ID="UpdateProg1" runat="server">
                <ProgressTemplate>
                    <div id="Div1" align="center" valign="middle" runat="server" style="position: absolute;
                        left: 43%; padding: 10px 10px 10px 10px; visibility: visible; border-color: silver;
                        border-style: solid; border-width: 1px; background-color: White;">
                        <img src="../img/indicator.gif" style="vertical-align: middle" alt="Processing" />
                        Loading ... &nbsp;
                    </div>
                </ProgressTemplate>
            </asp:UpdateProgress>
            <div id="div_price2" style="display: block">
                <asp:UpdatePanel ID="upUpdatePrice" runat="server" UpdateMode="Conditional">
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnReset" EventName="click" />
                        <asp:AsyncPostBackTrigger ControlID="btnRefresh" EventName="click" />
                        <asp:AsyncPostBackTrigger ControlID="editbt" EventName="click" />
                        <asp:AsyncPostBackTrigger ControlID="imgUp1" EventName="click" />
                        <asp:AsyncPostBackTrigger ControlID="imgUp2" EventName="click" />
                        <asp:AsyncPostBackTrigger ControlID="imgUp3" EventName="click" />
                        <asp:AsyncPostBackTrigger ControlID="imgDown1" EventName="click" />
                        <asp:AsyncPostBackTrigger ControlID="imgDown2" EventName="click" />
                        <asp:AsyncPostBackTrigger ControlID="imgDown3" EventName="click" />
                    </Triggers>
                    <ContentTemplate>
                        <div style="width: 50%; float: left">
                            <%--<div style="text-align: left; margin-top: 15px;">
                    <div style="float: left; width: 50px; text-align: left; font-weight: bold; padding-top: 2px;"
                        class="topic">
                        96.50</div>
                    <div style="float: left; width: 50px; text-align: right; color: #0066FF; padding-top: 2px;">
                    </div>
                    <div style="float: left; width: 70px; text-align: right; margin-left: 5px;">
                    </div>
                    <div style="float: left; width: 50px; text-align: right; color: #FF6600; padding-top: 2px;">
                    </div>
                    <div style="float: left; width: 70px; text-align: right; margin-left: 5px;">
                    </div>
                    <div style="float: left; width: 50px; text-align: right; padding-top: 2px;">
                    </div>
                    <div style="float: left; width: 50px; text-align: right; margin-left: 5px;">
                    </div>
                    <div style="clear: both;">
                    </div>
                </div>--%>
                            <div style="text-align: left;">
                                <div style="float: left; width: 220px; text-align: left; padding-top: 2px;" class="topic">
                                    96.50</div>
                                <div style="float: left; width: 70px; text-align: right; margin-left: 5px;">
                                </div>
                                <div style="float: left; width: 50px; text-align: right; padding-top: 2px;">
                                    &nbsp;
                                </div>
                                <div style="float: left; width: 50px; text-align: right; margin-left: 5px;">
                                </div>
                                <div style="clear: both;">
                                </div>
                            </div>
                            <div style="text-align: left; margin-top: 15px">
                                <div style="float: left; width: 50px; text-align: left; font-weight: bold; padding-top: 2px;">
                                    Level 1</div>
                                <div style="float: left; width: 50px; text-align: right; color: #0066FF; padding-top: 2px;">
                                    Bid :&nbsp;
                                </div>
                                <div style="float: left; width: 70px; text-align: right; margin-left: 5px;">
                                    <asp:TextBox ID="txt96Bid1" runat="server" MaxLength="50" Style="text-align: right;
                                        width: 70px; font-size: 12px; height: 12px;"></asp:TextBox>
                                </div>
                                <div style="float: left; width: 50px; text-align: right; color: #FF6600; padding-top: 2px;">
                                    Ask :&nbsp;
                                </div>
                                <div style="float: left; width: 70px; text-align: right; margin-left: 5px;">
                                    <asp:TextBox ID="txt96Ask1" runat="server" MaxLength="50" Style="text-align: right;
                                        width: 70px; font-size: 12px; height: 12px;"></asp:TextBox>
                                </div>
                                <div style="float: left; width: 50px; text-align: right; padding-top: 2px;">
                                    diff :&nbsp;
                                </div>
                                <div style="float: left; width: 50px; text-align: right; margin-left: 5px;">
                                    <asp:TextBox ID="txt96DifBidAsk" runat="server" MaxLength="50" Style="text-align: right;
                                        width: 50px; font-size: 12px; height: 12px;"></asp:TextBox>
                                </div>
                                <div style="clear: both;">
                                </div>
                            </div>
                            <asp:Panel ID="pnLv4" runat="server" Visible="true">
                                <div style="text-align: left; margin-top: 15px;">
                                    <div style="float: left; width: 50px; text-align: left; font-weight: bold; padding-top: 2px;">
                                        Level 2</div>
                                    <div style="float: left; width: 50px; text-align: right; color: #0066FF; padding-top: 2px;">
                                        Bid :&nbsp;
                                    </div>
                                    <div style="float: left; width: 70px; text-align: right; margin-left: 5px;">
                                        <asp:TextBox ID="txt96Bid2" runat="server" MaxLength="50" Style="text-align: right;
                                            width: 70px; font-size: 12px; height: 12px;"></asp:TextBox>
                                    </div>
                                    <div style="float: left; width: 50px; text-align: right; color: #FF6600; padding-top: 2px;">
                                        Ask :&nbsp;
                                    </div>
                                    <div style="float: left; width: 70px; text-align: right; margin-left: 5px;">
                                        <asp:TextBox ID="txt96Ask2" runat="server" MaxLength="50" Style="text-align: right;
                                            width: 70px; font-size: 12px; height: 12px;"></asp:TextBox>
                                    </div>
                                    <div style="float: left; width: 50px; text-align: right; color: #0066FF; padding-top: 2px;">
                                        Bid :&nbsp;
                                    </div>
                                    <div style="float: left; width: 50px; text-align: right; margin-left: 5px;">
                                        <asp:TextBox ID="txt96BidDif2" runat="server" MaxLength="4" Style="text-align: right;
                                            width: 50px; font-size: 12px; height: 12px;"></asp:TextBox>
                                    </div>
                                    <div style="float: left; width: 50px; text-align: right; color: #FF6600; padding-top: 2px;">
                                        Ask :&nbsp;
                                    </div>
                                    <div style="float: left; width: 50px; text-align: right; margin-left: 5px;">
                                        <asp:TextBox ID="txt96AskDif2" runat="server" MaxLength="3" Style="text-align: right;
                                            width: 50px; font-size: 12px; height: 12px;"></asp:TextBox>
                                    </div>
                                    <div style="clear: both;">
                                    </div>
                                </div>
                                <div style="text-align: left; margin-top: 15px;">
                                    <div style="float: left; width: 50px; text-align: left; font-weight: bold; padding-top: 2px;">
                                        Level 3</div>
                                    <div style="float: left; width: 50px; text-align: right; color: #0066FF; padding-top: 2px;">
                                        Bid :&nbsp;
                                    </div>
                                    <div style="float: left; width: 70px; text-align: right; margin-left: 5px;">
                                        <asp:TextBox ID="txt96Bid3" runat="server" MaxLength="50" Style="text-align: right;
                                            width: 70px; font-size: 12px; height: 12px;"></asp:TextBox>
                                    </div>
                                    <div style="float: left; width: 50px; text-align: right; color: #FF6600; padding-top: 2px;">
                                        Ask :&nbsp;
                                    </div>
                                    <div style="float: left; width: 70px; text-align: right; margin-left: 5px;">
                                        <asp:TextBox ID="txt96Ask3" runat="server" MaxLength="50" Style="text-align: right;
                                            width: 70px; font-size: 12px; height: 12px;"></asp:TextBox>
                                    </div>
                                    <div style="float: left; width: 50px; text-align: right; color: #0066FF; padding-top: 2px;">
                                        Bid :&nbsp;
                                    </div>
                                    <div style="float: left; width: 50px; text-align: right; margin-left: 5px;">
                                        <asp:TextBox ID="txt96BidDif3" runat="server" MaxLength="4" Style="text-align: right;
                                            width: 50px; font-size: 12px; height: 12px;"></asp:TextBox>
                                    </div>
                                    <div style="float: left; width: 50px; text-align: right; color: #FF6600; padding-top: 2px;">
                                        Ask :&nbsp;
                                    </div>
                                    <div style="float: left; width: 50px; text-align: right; margin-left: 5px;">
                                        <asp:TextBox ID="txt96AskDif3" runat="server" MaxLength="3" Style="text-align: right;
                                            width: 50px; font-size: 12px; height: 12px;"></asp:TextBox>
                                    </div>
                                    <div style="clear: both;">
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                        <div style="width: 50%; float: right">
                            <%--<div style="text-align: left; margin-top: 15px;">
                    <div style="float: left; width: 50px; text-align: left; font-weight: bold; padding-top: 2px;"
                        class="topic">
                        99.99</div>
                    <div style="float: left; width: 50px; text-align: right; color: #0066FF; padding-top: 2px;">
                    </div>
                    <div style="float: left; width: 70px; text-align: right; margin-left: 5px;">
                    </div>
                    <div style="float: left; width: 50px; text-align: right; color: #FF6600; padding-top: 2px;">
                    </div>
                    <div style="float: left; width: 70px; text-align: right; margin-left: 5px;">
                    </div>
                    <div style="float: left; width: 50px; text-align: right; padding-top: 2px;">
                    </div>
                    <div style="float: left; width: 50px; text-align: right; margin-left: 5px;">
                    </div>
                    <div style="clear: both;">
                    </div>
                </div>--%>
                            <div style="text-align: left;">
                                <div style="float: left; width: 120px; text-align: left; padding-top: 2px" class="topic">
                                    99.99
                                </div>
                                <div style="float: left; width: 120px; text-align: right; padding-top: 2px">
                                    Ask99.99-Ask96.50
                                </div>
                                <div style="float: left; width: 70px; text-align: right; margin-left: 5px;">
                                    <asp:TextBox ID="txtDif9699" runat="server" Style="text-align: right; width: 70px;
                                        font-size: 12px; height: 12px;"></asp:TextBox>
                                </div>
                                <div style="float: left; width: 50px; text-align: right; padding-top: 2px;">
                                    &nbsp;
                                </div>
                                <div style="float: left; width: 50px; text-align: right; margin-left: 5px;">
                                </div>
                                <div style="clear: both;">
                                </div>
                            </div>
                            <div style="text-align: left; margin-top: 15px">
                                <div style="float: left; width: 50px; text-align: left; font-weight: bold; padding-top: 2px;">
                                    Level 1</div>
                                <div style="float: left; width: 50px; text-align: right; color: #0066FF; padding-top: 2px;">
                                    Bid :&nbsp;
                                </div>
                                <div style="float: left; width: 70px; text-align: right; margin-left: 5px;">
                                    <asp:TextBox ID="txt99Bid1" runat="server" MaxLength="50" Style="text-align: right;
                                        width: 70px; font-size: 12px; height: 12px;"></asp:TextBox>
                                </div>
                                <div style="float: left; width: 50px; text-align: right; color: #FF6600; padding-top: 2px;">
                                    Ask :&nbsp;
                                </div>
                                <div style="float: left; width: 70px; text-align: right; margin-left: 5px;">
                                    <asp:TextBox ID="txt99Ask1" runat="server" MaxLength="50" Style="text-align: right;
                                        width: 70px; font-size: 12px; height: 12px;"></asp:TextBox>
                                </div>
                                <div style="float: left; width: 50px; text-align: right; padding-top: 2px;">
                                    diff :&nbsp;
                                </div>
                                <div style="float: left; width: 50px; text-align: right; margin-left: 5px;">
                                    <asp:TextBox ID="txt99DifBidAsk" runat="server" MaxLength="50" Style="text-align: right;
                                        width: 50px; font-size: 12px; height: 12px;"></asp:TextBox>
                                </div>
                                <div style="clear: both;">
                                </div>
                            </div>
                            <asp:Panel ID="pnLv3" runat="server" Visible="true">
                                <div style="text-align: left; margin-top: 15px;">
                                    <div style="float: left; width: 50px; text-align: left; font-weight: bold; padding-top: 2px;">
                                        Level 2</div>
                                    <div style="float: left; width: 50px; text-align: right; color: #0066FF; padding-top: 2px;">
                                        Bid :&nbsp;
                                    </div>
                                    <div style="float: left; width: 70px; text-align: right; margin-left: 5px;">
                                        <asp:TextBox ID="txt99Bid2" runat="server" MaxLength="50" Style="text-align: right;
                                            width: 70px; font-size: 12px; height: 12px;"></asp:TextBox>
                                    </div>
                                    <div style="float: left; width: 50px; text-align: right; color: #FF6600; padding-top: 2px;">
                                        Ask :&nbsp;
                                    </div>
                                    <div style="float: left; width: 70px; text-align: right; margin-left: 5px;">
                                        <asp:TextBox ID="txt99Ask2" runat="server" MaxLength="50" Style="text-align: right;
                                            width: 70px; font-size: 12px; height: 12px;"></asp:TextBox>
                                    </div>
                                    <div style="float: left; width: 50px; text-align: right; color: #0066FF; padding-top: 2px;">
                                        Bid :&nbsp;
                                    </div>
                                    <div style="float: left; width: 50px; text-align: right; margin-left: 5px;">
                                        <asp:TextBox ID="txt99BidDif2" runat="server" MaxLength="4" Style="text-align: right;
                                            width: 50px; font-size: 12px; height: 12px;"></asp:TextBox>
                                    </div>
                                    <div style="float: left; width: 50px; text-align: right; color: #FF6600; padding-top: 2px;">
                                        Ask :&nbsp;
                                    </div>
                                    <div style="float: left; width: 50px; text-align: right; margin-left: 5px;">
                                        <asp:TextBox ID="txt99AskDif2" runat="server" MaxLength="3" Style="text-align: right;
                                            width: 50px; font-size: 12px; height: 12px;"></asp:TextBox>
                                    </div>
                                    <div style="clear: both;">
                                    </div>
                                </div>
                                <div style="text-align: left; margin-top: 15px;">
                                    <div style="float: left; width: 50px; text-align: left; font-weight: bold; padding-top: 2px;">
                                        Level 3</div>
                                    <div style="float: left; width: 50px; text-align: right; color: #0066FF; padding-top: 2px;">
                                        Bid :&nbsp;
                                    </div>
                                    <div style="float: left; width: 70px; text-align: right; margin-left: 5px;">
                                        <asp:TextBox ID="txt99Bid3" runat="server" MaxLength="50" Style="text-align: right;
                                            width: 70px; font-size: 12px; height: 12px;"></asp:TextBox>
                                    </div>
                                    <div style="float: left; width: 50px; text-align: right; color: #FF6600; padding-top: 2px;">
                                        Ask :&nbsp;
                                    </div>
                                    <div style="float: left; width: 70px; text-align: right; margin-left: 5px;">
                                        <asp:TextBox ID="txt99Ask3" runat="server" MaxLength="50" Style="text-align: right;
                                            width: 70px; font-size: 12px; height: 12px;"></asp:TextBox>
                                    </div>
                                    <div style="float: left; width: 50px; text-align: right; color: #0066FF; padding-top: 2px;">
                                        Bid :&nbsp;
                                    </div>
                                    <div style="float: left; width: 50px; text-align: right; margin-left: 5px;">
                                        <asp:TextBox ID="txt99BidDif3" runat="server" MaxLength="4" Style="text-align: right;
                                            width: 50px; font-size: 12px; height: 12px;"></asp:TextBox>
                                    </div>
                                    <div style="float: left; width: 50px; text-align: right; color: #FF6600; padding-top: 2px;">
                                        Ask :&nbsp;
                                    </div>
                                    <div style="float: left; width: 50px; text-align: right; margin-left: 5px;">
                                        <asp:TextBox ID="txt99AskDif3" runat="server" MaxLength="3" Style="text-align: right;
                                            width: 50px; font-size: 12px; height: 12px;"></asp:TextBox>
                                    </div>
                                    <div style="clear: both;">
                                    </div>
                                </div>
                            </asp:Panel>
                        </div>
                        <asp:HiddenField ID="hdfBid99" runat="server" />
                    </ContentTemplate>
                </asp:UpdatePanel>
                <br />
                <asp:UpdatePanel ID="upPriceDetail" runat="server">
                    <ContentTemplate>
                        <asp:HiddenField ID="hdfPwd" runat="server" />
                        <br />
                        <div style="float: left; width: 130px; text-align: left; font-weight: bold; padding-top: 2px;">
                            % Range Leave Order</div>
                        <div style="float: left; width: 300px; text-align: left; margin-left: 5px;">
                            <asp:TextBox ID="txtRangeLeave" runat="server" MaxLength="6" Style="text-align: right;
                                width: 80px; font-size: 12px; height: 12px;"></asp:TextBox>
                        </div>
                        <div style="clear: both;">
                        </div>
                        <div style="float: left; width: 130px; text-align: left; font-weight: bold; padding-top: 2px;">
                            Max %</div>
                        <div style="float: left; width: 300px; text-align: left; margin-left: 5px;">
                            <asp:TextBox ID="txtMaxPer" runat="server" MaxLength="6" Style="text-align: right;
                                width: 80px; font-size: 12px; height: 12px;"></asp:TextBox>
                        </div>
                        <div style="clear: both;">
                        </div>
                        <div style="text-align: left; margin-top: 15px;">
                            <div style="float: left; width: 130px; text-align: left; font-weight: bold; padding-top: 2px;">
                                Timeout</div>
                            <div style="float: left; width: 300px; text-align: left; margin-left: 5px;">
                                <asp:TextBox ID="txtLeaveTimeout" runat="server" MaxLength="3" Style="text-align: right;
                                    width: 80px; font-size: 12px; height: 12px;"></asp:TextBox>
                            </div>
                            <div style="clear: both;">
                            </div>
                            <div style="text-align: left; margin-top: 15px;">
                                <div style="float: left; width: 130px; text-align: left; font-weight: bold; padding-top: 2px;">
                                    Min-Max Bid 99.99</div>
                                <div style="float: left; width: 300px; text-align: left; margin-left: 5px;">
                                    <asp:TextBox ID="txtBidMin" runat="server" MaxLength="50" Style="text-align: right;
                                        width: 80px; font-size: 12px; height: 12px;"></asp:TextBox>
                                    <asp:TextBox ID="txtBidMax" runat="server" MaxLength="50" Style="text-align: right;
                                        width: 80px; font-size: 12px; height: 12px;"></asp:TextBox>
                                </div>
                                <div style="clear: both;">
                                </div>
                                <div style="text-align: left; margin-top: 15px;">
                                    <div style="float: left; width: 130px; text-align: left; font-weight: bold; padding-top: 2px;">
                                        Max Baht - Max Kg</div>
                                    <div style="float: left; width: 300px; text-align: left; margin-left: 5px;">
                                        <asp:TextBox ID="txtMaxBaht" runat="server" MaxLength="50" Style="text-align: right;
                                            width: 80px; font-size: 12px; height: 12px;"></asp:TextBox>
                                        <asp:TextBox ID="txtMaxKg" runat="server" MaxLength="50" Style="text-align: right;
                                            width: 80px; font-size: 12px; height: 12px;"></asp:TextBox>
                                    </div>
                                    <div style="clear: both;">
                                    </div>
                                    <div style="text-align: left; margin-top: 15px;">
                                        <div style="float: left; width: 130px; text-align: left; font-weight: bold; padding-top: 2px;">
                                            Min-Max Ask99.99-Ask96.50</div>
                                        <div style="float: left; width: 300px; text-align: left; margin-left: 5px;">
                                            <asp:TextBox ID="txtMinAsk9996" runat="server" MaxLength="50" Style="text-align: right;
                                                width: 80px; font-size: 12px; height: 12px;"></asp:TextBox>
                                            <asp:TextBox ID="txtMaxAsk9996" runat="server" MaxLength="50" Style="text-align: right;
                                                width: 80px; font-size: 12px; height: 12px;"></asp:TextBox>
                                        </div>
                                        <div style="clear: both;">
                                        </div>
                                    </div>
                                    <div style="text-align: left; margin-top: 15px">
                                        <div style="float: left; width: 130px; text-align: left; font-weight: bold; padding-top: 2px;">
                                            Update Bid Ask</div>
                                        <div style="float: left; width: 40px; text-align: right; margin-left: 5px;">
                                            <asp:TextBox ID="txtBidChangeNumber1" runat="server" MaxLength="50" Style="text-align: right;
                                                width: 40px; font-size: 12px; height: 12px;"></asp:TextBox>
                                        </div>
                                        <div style="float: left; width: 40px; text-align: right; margin-left: 5px;">
                                            <asp:TextBox ID="txtBidChangeNumber2" runat="server" MaxLength="50" Style="text-align: right;
                                                width: 40px; font-size: 12px; height: 12px;"></asp:TextBox>
                                        </div>
                                        <div style="float: left; width: 40px; text-align: right; margin-left: 5px;">
                                            <asp:TextBox ID="txtBidChangeNumber3" runat="server" MaxLength="50" Style="text-align: right;
                                                width: 40px; font-size: 12px; height: 12px;"></asp:TextBox>
                                        </div>
                                        <div style="float: left; width: 400px; text-align: left; margin-left: 5px;">
                                        </div>
                                        <div style="clear: both;">
                                        </div>
                                    </div>
                                    <div>
                                        <div style="text-align: left; margin-top: 15px">
                                            <div style="float: left; width: 130px; text-align: left; font-weight: bold; padding-top: 2px;">
                                            </div>
                                            <div style="float: left; width: 40px; text-align: right; margin-left: 5px;">
                                                <asp:ImageButton ID="imgUp1" runat="server" ImageUrl="~/trade/admin/image/arrow_up.gif"
                                                    Width="20px" Height="12px" />
                                            </div>
                                            <div style="float: left; width: 40px; text-align: right; margin-left: 5px;">
                                                <asp:ImageButton ID="imgUp2" runat="server" ImageUrl="~/trade/admin/image/arrow_up.gif"
                                                    Width="20px" Height="12px" />
                                            </div>
                                            <div style="float: left; width: 40px; text-align: right; margin-left: 5px;">
                                                <asp:ImageButton ID="imgUp3" runat="server" ImageUrl="~/trade/admin/image/arrow_up.gif"
                                                    Width="20px" Height="12px" />
                                            </div>
                                            <div style="clear: both;">
                                            </div>
                                        </div>
                                        <div style="text-align: left; margin-top: 15px">
                                            <div style="float: left; width: 130px; text-align: left; font-weight: bold; padding-top: 2px;">
                                            </div>
                                            <div style="float: left; width: 40px; text-align: right; margin-left: 5px;">
                                                <asp:ImageButton ID="imgDown1" runat="server" ImageUrl="~/trade/admin/image/arrow_down.gif"
                                                    Width="20px" Height="12px" />
                                            </div>
                                            <div style="float: left; width: 40px; text-align: right; margin-left: 5px;">
                                                <asp:ImageButton ID="imgDown2" runat="server" ImageUrl="~/trade/admin/image/arrow_down.gif"
                                                    Width="20px" Height="12px" />
                                            </div>
                                            <div style="float: left; width: 40px; text-align: right; margin-left: 5px;">
                                                <asp:ImageButton ID="imgDown3" runat="server" ImageUrl="~/trade/admin/image/arrow_down.gif"
                                                    Width="20px" Height="12px" />
                                            </div>
                                            <div style="clear: both;">
                                            </div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                            &nbsp;<div style="text-align: center; margin-top: 15px; clear: both;">
                                <div style="text-align: left; margin-top: 15px;">
                                    <div style="float: left; width: 130px; text-align: left; font-weight: bold; padding-top: 2px;">
                                        Inactive Pricing Time</div>
                                    <div style="float: left; width: 80px; text-align: left; margin-left: 5px;">
                                        <asp:TextBox ID="TimeTxtBox" runat="server" MaxLength="50" Style="text-align: right;
                                            width: 80px; font-size: 12px; height: 12px;"></asp:TextBox>
                                    </div>
                                    <div style="clear: both;">
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div style="text-align: center; margin-top: 10px; clear: both;">
                            <asp:Button ID="btnRefresh" CssClass="buttonPro small grey" runat="server" Text="Cal" />&nbsp;
                            <asp:Button ID="btnReset" CssClass="buttonPro small grey" runat="server" Text="Reset" />&nbsp;
                            <asp:Button ID="editbt" CssClass="buttonPro small grey" runat="server" Text="Save" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>
            </div>
        </fieldset>
        <div style="margin: auto; clear: both; text-align: left" />
        <ajaxToolkit:ModalPopupExtender ID="mppPer" BehaviorID="mppPer" runat="server" TargetControlID="Panel5"
            PopupControlID="Panel5" OkControlID="btnOkPer" OnOkScript="okClickPer();" CancelControlID="btnNoPer"
            OnCancelScript="cancelClickPer();" BackgroundCssClass="modalBackground">
        </ajaxToolkit:ModalPopupExtender>
        <asp:Panel ID="Panel5" runat="server" Style="display: none" CssClass="modalPopup">
            <asp:Panel ID="Panel6" runat="server" Style="cursor: move; background-color: #DDDDDD;
                border: solid 1px Gray; color: Black">
                <div style="text-align: center">
                    <p id="p2" style="text-align: justify">
                    </p>
                    <br />
                    <div>
                        <h4>
                            Bid 99.99 more than the percentage specified. Please Enter Password to Save.</h4>
                        <asp:TextBox ID="txtPwdPer" runat="server" TextMode="Password"></asp:TextBox>
                        <asp:RequiredFieldValidator ID="rfvPer" ValidationGroup="mppPer" ControlToValidate="txtPwdPer"
                            runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                    </div>
                </div>
            </asp:Panel>
            <div>
                <p style="text-align: center;">
                    <asp:Button ID="btnOkPer" runat="server" CssClass="buttonPro small grey" ValidationGroup="mppPer"
                        Text="Yes" Width="50px" />
                    <asp:Button ID="btnNoPer" runat="server" CssClass="buttonPro small grey" Text="No"
                        Width="50px" />
                </p>
            </div>
        </asp:Panel>
        <br />
    </div>
</asp:Content>
