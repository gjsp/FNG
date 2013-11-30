<%@ Page Language="VB" MasterPageFile="~/trade/admin/MasterPageAdmin.master" AutoEventWireup="false"
    CodeFile="trade_trans.aspx.vb" Inherits="admin_trade_trans" Title="Transactions" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">

    <script src="../../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript">
    function setConfirm(source)
    {
      if(source.checked == true)
      {
        document.getElementById('ctl00_ContentPlaceHolder1_hdfConfirm').value = 'y';
        ifmTran.document.getElementById('hdfConfirm').value = 'y';
        div_fast.style.display = 'block';
      }
      else
      {
        document.getElementById('ctl00_ContentPlaceHolder1_hdfConfirm').value = 'n';
        ifmTran.document.getElementById('hdfConfirm').value = 'n';
        div_fast.style.display = 'none';
      }
  }

  function setConfirmSave(source) {
      if (source.checked == true) {
          ifmTran.document.getElementById('hdfConfirmSave').value = 'y';
      }
      else {
          ifmTran.document.getElementById('hdfConfirmSave').value = 'n';
      }
  }
    
//    function managePrice(){
//        if(div_price.style.display=='none')
//        {
//            div_price.style.display = 'block';
//            $get('img_price').src='image/collapse.jpg';
//        }
//        else
//        {
//            div_price.style.display = 'none';
//            $get('img_price').src='image/expand.jpg';
//            
//        }
//    }
    function refresh()
    {
       __doPostBack('udpSum', '');
    }
//    function setFastMode() 
//    {
//        var x = $get('ctl00_ContentPlaceHolder1_rdoFashTrade');
    //    }

    function setSummary(b96,s96,n96,b99,s99,n99) {

        document.getElementById('ctl00_ContentPlaceHolder1_lblSumBuy96').innerText = b96;
        document.getElementById('ctl00_ContentPlaceHolder1_lblSumSell96').innerText = s96;
        document.getElementById('ctl00_ContentPlaceHolder1_lblSumNet96').innerText = n96;
        
        document.getElementById('ctl00_ContentPlaceHolder1_lblSumBuy99').innerText = b99;
        document.getElementById('ctl00_ContentPlaceHolder1_lblSumSell99').innerText = s99;
        document.getElementById('ctl00_ContentPlaceHolder1_lblSumNet99').innerText = n99;
    }
    function updateFrameHeightToMatchContents(jFrame) {
        jFrame.css("height", (jFrame.contents()).height());
    }
 
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div style="margin: auto; text-align: left; width: 1200px;">
        <%--<div class="heading" style="display:block">
            <img id="img_price" src="image/expand.jpg" onclick="managePrice()" />
            Update Price
        </div>
        <div id="div_price" style="display: block">
            <iframe id="ifrPrice" width="100%" frameborder="0" scrolling="no" marginheight="0"
                height="440px" marginwidth="0" src="manage_price.aspx?m=sub"></iframe>
        </div>--%>
        <fieldset class="k-fieldset" style="display:none">
            <legend class="topic">Transaction</legend>
            <asp:UpdatePanel ID="UpdatePanel1" runat="server" UpdateMode="Conditional">
                <Triggers>
                    <asp:AsyncPostBackTrigger ControlID="btnAccept" EventName="click" />
                </Triggers>
            </asp:UpdatePanel>
            <div style="margin-top: 10px; margin-left: 10px; padding: 2 2 2 2; text-align: left;"
                class="demoarea">
                <asp:HiddenField ID="hdfConfirm" runat="server" />
                <span class="lblHead1">Price from</span>
                <asp:TextBox ID="txtPrice1" runat="server"></asp:TextBox>&nbsp;<span class="lblHead1">To</span>
                &nbsp;
                <asp:TextBox ID="txtPrice2" runat="server"></asp:TextBox>
                &nbsp;<asp:Button ID="btnAccept" CssClass="btn" runat="server" Width="120px" Text="Accept" />
            </div>
        </fieldset>
        <div style="text-align: left">
            <fieldset class="k-fieldset">
                <legend class="topic">Summary Quantity</legend>
                <asp:UpdatePanel ID="udpSum" runat="server">
                    <ContentTemplate>
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
                        <table>
                            <tr>
                                <td style="width: 180px">
                                    Summary Customer Buy 96.50 :
                                </td>
                                <td style="width: 100px">
                                    <asp:Label ID="lblSumBuy96" Font-Bold="True" runat="server" Text="0.00000"></asp:Label>
                                </td>
                                <td style="width: 180px">
                                    Summary Customer Sell 96.50 :
                                </td>
                                <td style="width: 100px">
                                    <asp:Label ID="lblSumSell96" Font-Bold="True" runat="server" Text="0.00000"></asp:Label>
                                </td>
                                <td style="width: 100px">
                                    Net 96.50 :
                                </td>
                                <td style="width: 100px">
                                    <asp:Label ID="lblSumNet96" Font-Bold="True" runat="server" Text="0.00000"></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td style="width: 180px">
                                    Summary Customer Buy 99.99 :
                                </td>
                                <td style="width: 100px">
                                    <asp:Label ID="lblSumBuy99" Font-Bold="True" runat="server" Text="0.00000"></asp:Label>
                                </td>
                                <td style="width: 180px">
                                    Summary Customer Sell 99.99 :
                                </td>
                                <td style="width: 100px">
                                    <asp:Label ID="lblSumSell99" Font-Bold="True" runat="server" Text="0.00000"></asp:Label>
                                </td>
                                <td style="width: 100px">
                                    Net 99.99 :
                                </td>
                                <td style="width: 100px">
                                    <asp:Label ID="lblSumNet99" Font-Bold="True" runat="server" Text="0.00000"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </ContentTemplate>
                    <Triggers>
                        <asp:AsyncPostBackTrigger ControlID="btnRefresh" />
                    </Triggers>
                </asp:UpdatePanel>
                <div style="display: none">
                    <asp:Button ID="btnRefresh" runat="server" Text="Refresh" /></div>
            </fieldset>
            
<%--               <fieldset class="k-fieldset" style="display:none">
                <legend class="topic">Time Left</legend>
                 <iframe id="ifmTime" width="100%" height="25px" frameborder="0" scrolling="no" marginheight="0"
                        marginwidth="0"></iframe>
                </fieldset>--%>
                
<%--            <div style="margin-left: 10px; padding: 2 2 2 2; text-align: left;"
                class="demoarea">
                 <table cellpadding="0" cellspacing="0" >
                    <tr style="height:30px">
                        <td style="width:100px">
                          <asp:CheckBox ID="cbConfirmSave" Text="Fast Confirm" runat="server" />
                        </td>
                        <td style="width:100px">
                          <asp:CheckBox ID="cbConfirm" Text="Fast Trade" runat="server" />
                        </td>
                        <td>
                            <div id="div_fast" style="display:none">
                            <asp:RadioButtonList ID="rdoFashTrade" runat="server" 
                                RepeatDirection="Horizontal">
                            <asp:ListItem Text="One Deal" Value="accept" Selected="True" />
                            <asp:ListItem Text="All at the same price" Value="same" />
                            <asp:ListItem Text="All at the same and lower/higher price" Value="lowerOrHigher" />
                            </asp:RadioButtonList></div>
                        </td>
                    </tr>
                    </table>
            </div>--%>
            
            <cc1:TabContainer ID="TabContainer1" runat="server" ActiveTabIndex="0" Width="100%"
                CssClass="Tab">
                <cc1:TabPanel ID="TabPanel1" runat="server" HeaderText="Order">
                    <ContentTemplate>
                        <iframe id="ifmTran" width="100%" frameborder="0" scrolling="no" marginheight="0" onload="updateFrameHeightToMatchContents(jQuery(this));"
                            marginwidth="0" src="trans2.aspx?m=tran"></iframe>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="TabPanel4" runat="server" HeaderText="Leave Order">
                    <ContentTemplate>
                        <iframe id="ifmLeave" width="100%" frameborder="0" scrolling="no" marginheight="0"
                            marginwidth="0" src="trans.aspx?m=leave"></iframe>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="TabPanel2" runat="server" HeaderText="Accept">
                    <ContentTemplate>
                        <iframe id="ifmAcc" width="100%" frameborder="0" scrolling="no" marginheight="0"
                            marginwidth="0" src="trans.aspx?m=accept"></iframe>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="TabPanel3" runat="server" HeaderText="Reject">
                    <ContentTemplate>
                        <iframe id="ifmRej" width="100%" frameborder="0" scrolling="no" marginheight="0"
                            marginwidth="0" src="trans.aspx?m=reject"></iframe>
                    </ContentTemplate>
                </cc1:TabPanel>
            </cc1:TabContainer>
             <iframe id="ifmRejectTime" width="100%" style="display:none" frameborder="0" scrolling="no" marginheight="0"
                            marginwidth="0" src="reject_timer.aspx"></iframe>
        </div>
    </div>
</asp:Content>
