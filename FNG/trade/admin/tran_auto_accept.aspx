<%@ Page Language="VB" MasterPageFile="~/trade/admin/MasterPageAdmin.master"
    AutoEventWireup="false" CodeFile="tran_auto_accept.aspx.vb" Inherits="trade_admin_tran_auto_accept" title="Auto Accept" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <link href="../../button/css/buttonPro.css" rel="stylesheet" type="text/css" />
    <script language="javascript" type="text/javascript">
        function setColorClert(type, purity, hasTran) {
            var btn = type.toString() + purity.toString();
            var className = hasTran == 'y' ? 'buttonPro rounded red' : 'buttonPro rounded green';
            //var enabled = hasTran == 'y' ? '' : 'disabled';
            switch (btn) {
                case 'buy96':
                    $get('<%=btnBuy96.clientId %>').className = className;
                    //$get('<%=btnBuy96.clientId %>').disabled = enabled;
                    break;
                case 'sell96':
                    $get('<%=btnSell96.clientId %>').className = className;
                    //$get('<%=btnSell96.clientId %>').disabled = enabled;
                    break;
                case 'buy99':
                    $get('<%=btnBuy99.clientId %>').className = className;
                    //$get('<%=btnBuy99.clientId %>').disabled = enabled;
                    break;
                case 'sell99':
                    $get('<%=btnSell99.clientId %>').className = className;
                    //$get('<%=btnSell99.clientId %>').disabled = enabled;
                    break;
                default:
            }

        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="width: 1300px">
        <asp:UpdatePanel ID="upClear" runat="server">
            <ContentTemplate>
                <asp:UpdateProgress ID="UpdateProg1" runat="server">
                    <ProgressTemplate>
                        <div id="Div1" align="center" valign="middle" runat="server" style="position: absolute;
                            left: 16%; padding: 10px 10px 10px 10px; visibility: visible; border-color: silver;
                            border-style: solid; border-width: 1px; background-color: White;">
                            <img src="../img/indicator.gif" style="vertical-align: middle" alt="Processing" />
                            Loading ... 
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </ContentTemplate>
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btnBuy96" EventName="click" />
                <asp:AsyncPostBackTrigger ControlID="btnSell96" EventName="click" />
                <asp:AsyncPostBackTrigger ControlID="btnBuy99" EventName="click" />
                <asp:AsyncPostBackTrigger ControlID="btnSell99" EventName="click" />
            </Triggers>
        </asp:UpdatePanel>
        <div id="div_clearall"  style="margin-top:5px">
            <asp:Button ID="btnBuy96" runat="server" Text="Clear All Buy 96.50%" BorderColor="Silver"
                BorderStyle="Groove" CssClass="buttonPro rounded" />
            &nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnSell96" runat="server" Text="Clear All Sell 96.50%" BorderColor="Silver"
                BorderStyle="Groove" CssClass="buttonPro rounded" />
           &nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnBuy99" runat="server" Text="Clear All Buy 99.99%" BorderColor="Silver"
                BorderStyle="Groove" CssClass="buttonPro rounded" />
                &nbsp;&nbsp;&nbsp;
            <asp:Button ID="btnSell99" runat="server" Text="Clear All Sell 99.99%" BorderColor="Silver"
                BorderStyle="Groove" CssClass="buttonPro rounded" />
        </div>
        <div style="text-align: left; padding-right: auto">
            <h3 style="background-color: #333333; color: White">
                Purity 96.5%(BAHT)</h3>
            <div style="float: left; width: 650px">
                <fieldset class="k-fieldset">
                    <legend class="topic">Customer Buy</legend>
                    <iframe id="ifmBuy96" width="100%" frameborder="0" scrolling="no" marginheight="0"
                        height="350px" marginwidth="0" src="tran_auto.aspx?type=buy&purity=96"></iframe>
                </fieldset>
            </div>
            <div style="float: right; width: 650px">
                <fieldset class="k-fieldset">
                    <legend class="topic">Customer Sell</legend>
                    <iframe id="ifmSell96" width="100%" frameborder="0" scrolling="no" marginheight="0"
                        height="350px" marginwidth="0" src="tran_auto.aspx?type=sell&purity=96"></iframe>
                </fieldset>
            </div>
            <div style="clear: both">
            </div>
        </div>
        <div style="text-align: left">
            <h3 style="background-color: #333333; color: White">
                Purity 99.99%(KG)</h3>
            <div style="float: left; width: 650px">
                <fieldset class="k-fieldset">
                    <legend class="topic">Customer Buy</legend>
                    <iframe id="Iframe1" width="100%" frameborder="0" scrolling="no" marginheight="0"
                        height="350px" marginwidth="0" src="tran_auto.aspx?type=buy&purity=99"></iframe>
                </fieldset>
            </div>
            <div style="float: right; width: 650px">
                <fieldset class="k-fieldset">
                    <legend class="topic">Customer Sell</legend>
                    <iframe id="Iframe2" width="100%" frameborder="0" scrolling="no" marginheight="0"
                        height="350px" marginwidth="0" src="tran_auto.aspx?type=sell&purity=99"></iframe>
                </fieldset>
            </div>
            <div style="clear: both">
            </div>
        </div>
    </div>
</asp:Content>
