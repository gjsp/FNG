<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false"
    CodeFile="ticket_deal.aspx.vb" Inherits="ticket_deal" Title="Ticket" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<%@ Register src="user_control/ucPortFolio.ascx" tagname="ucPortFolio" tagprefix="uc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <style type="text/css">
        .style4
        {
            width: 100px;
            text-align: right;
        }
        .lbl
        {
            color: Black;
        }
        .style6
        {
            width: 280px;
        }
        .style13
        {
            width: 108px;
            text-align: right;
        }
        .style15
        {
            height: 14px;
        }
        .style21
        {
            height: 5px;
        }
        .style22 
        {
            height: 25px;
        }
        .style38
        {
            height: 12px;
        }
        .style50
        {
            height: 33px;
        }
        .style54
        {
            width: 120px;
            text-align: right;
        }
    </style>

    <script type="text/javascript" language="javascript">
    
    function checkTradeLimit()
    {
        if($get('ctl00_SampleContent_hdfTradeLimit').value =='n')
        {
            alert('Please Check Amount Value');
            return false;
        }
    }
    
    function setOrderType(type)
    {
	    if(type=='sell')
	    {
	                    $get('ctl00_SampleContent_rdoType').cells[0].children[0].checked = true;
            		    //$get('ctl00_SampleContent_rdoType').cells[0].parentNode.cells[0].parentNode.childNodes[0].childNodes[0].checked = true;
            		    div_main.style.backgroundColor = '#FFFFCC';
                		
	    }
        else if(type=='buy')
	    {
	                    $get('ctl00_SampleContent_rdoType').cells[1].children[0].checked = true;
            		    //$get('ctl00_SampleContent_rdoType').cells[1].parentNode.cells[1].parentNode.childNodes[1].childNodes[0].checked = true;
            		    div_main.style.backgroundColor = '#E1F0FF';
                		
	    }
        //return false;
    }
    
    function OnCompleteCust_id(result) {
        try {
             if(result!=''){
                $get('ifmGrid').style.display = '';
                $get('ifmGrid').src="customer_grid.aspx?id=" + $get('ctl00_SampleContent_txtCustRef').value;
               // calculate_price();
              }else{
                $get('ifmGrid').style.display = '';
                $get('ifmGrid').src="customer_grid.aspx?id="
              }
        }
        catch (err) {
            //alert(err.description);
            return false;
        } 
    }
    function OnCompleteCust_name(result) {
        try 
        {
           if(result!=''){
                $get('ifmGrid').style.display = '';
                $get('ifmGrid').src="customer_grid.aspx?name=" + $get('ctl00_SampleContent_txtCustName').value;
                //calculate_price();
            }else{
               $get('ifmGrid').style.display = '';
               $get('ifmGrid').src="customer_grid.aspx?id=";
             }
        }
        catch (err) {
            //alert(err.description);
            return false;
        } 
    }
    function checkCust_id()    
    {
        var cust_id = $get('ctl00_SampleContent_txtCustRef').value
        if(cust_id !='')
        {
            PageMethods.checkCustomer(cust_id,"",OnCompleteCust_id);
        }
        else
        {
            $get('ifmGrid').src = "";
            $get('ctl00_SampleContent_txtAmount').style.color='black';
        }
    }
    function checkCust_name()    
    {
        var cust_name = $get('ctl00_SampleContent_txtCustName').value
        if(cust_name !='')
        {
            PageMethods.checkCustomer("",cust_name,OnCompleteCust_name);
        }
        else
        {
            $get('ifmGrid').src = "";
            $get('ctl00_SampleContent_txtAmount').style.color='black';
        }
    }
    function onCustAutoCompleteClick(source, eventArgs) 
    {   
        $get('ctl00_SampleContent_txtCustRef').value = eventArgs.get_value();
        $get('ctl00_SampleContent_btnSearch').click();
        //calculate_price('ctl00_SampleContent_txtQuan','ctl00_SampleContent_txtPrice','ctl00_SampleContent_txtAmount');
        //, txtQuan.ClientID, txtPrice.ClientID, txtAmount.ClientID
    }

    function setBgColor()
    {
        var rdo = $get('ctl00_SampleContent_rdoType');
        if(rdo.cells[0].children[0].checked == true)
        {
            div_main.style.backgroundColor = '#FFFFCC';
        }
        else
        {
            div_main.style.backgroundColor = '#E1F0FF';
        }
     }
    function changeBgColor()
    {
        var rdo = $get('ctl00_SampleContent_rdoType');
        if(rdo.cells[0].children[0].checked == true)
        {
            div_main.style.backgroundColor = '#FFFFCC';
        }
        else
        {
            div_main.style.backgroundColor = '#E1F0FF';
        }
        
        $get('ctl00_SampleContent_txtAmount').value='';
        $get('ctl00_SampleContent_txtQuan').value='';
        $get('ctl00_SampleContent_txtPrice').value='';
        $get('ctl00_SampleContent_linkClearPrice').click();
    }
    function checkAmount()
    {
        var excess =  $get('ctl00_SampleContent_hdfExcess').value;
        if( parseFloat(excess) < 0 || parseFloat(result) > parseFloat(excess))
        {
            $get('ctl00_SampleContent_linkColor').click();
        }
        else
        {
             $get('ctl00_SampleContent_txtAmount').style.color='black';
             return false;
        }
    }
    function calculate_price(ctl_quan,ctl_price,ctl_amount)
    {
        var quan =  $get(ctl_quan).value ;
        var price =  $get(ctl_price).value ;
        var price99Control  = $get('ctl00_SampleContent_hdfPrice99Control').value;
        // && !isNaN(parseFloat(price))       
        if(quan != '' && price != '')
        {
            var rdoGoldType =  $get('ctl00_SampleContent_rdoGoldType');
            var rdoType =  $get('ctl00_SampleContent_rdoType');
            var result ;
            
             if (rdoType.cells[0].children[0].checked == false && rdoType.cells[1].children[0].checked == false)
            {
                $get('ctl00_SampleContent_txtAmount').value='';
                return;
            }
            
            if (rdoGoldType.cells[0].children[0].checked == false && rdoGoldType.cells[1].children[0].checked == false && rdoGoldType.cells[2].children[0].checked == false)
            {
                $get('ctl00_SampleContent_txtAmount').value='';
                return;
            }
            
            if((rdoGoldType.cells[1].children[0].checked == true) && parseFloat(price) < parseFloat(price99Control))
            {
                result = (((quan * price) * 656)/10);
            }
            else
            {
                 result = quan * price;
            }
            
            $get('ctl00_SampleContent_txtAmount').value = result.toFixed(2);
            $get('ctl00_SampleContent_txtAmount').style.color='black';
            var excess =  $get('ctl00_SampleContent_hdfExcess').value;
            $get('ctl00_SampleContent_linkColor').click();
         }
         else
         {
            $get('ctl00_SampleContent_txtAmount').value='';
         }
    }
    
    function clearPriceQuanAmount()
    {
        $get('ctl00_SampleContent_txtAmount').value='';
        $get('ctl00_SampleContent_txtQuan').value='';
        $get('ctl00_SampleContent_txtPrice').value='';
    }
     
    </script>

    <script type="text/javascript">
            //  For Model Popup
            //  keeps track of the delete button for the row
            //  that is going to be removed
            var _source;
            // keep track of the popup div
            var _popup;
            
            function showConfirm(source){
                this._source = source;
                this._popup = $find('mppDel');
               
                //  find the confirm ModalPopup and show it    
                this._popup.show();
                //"Deal Ticket Complete Reference No : " & lblRefNo.Text & "|nBook No : " + txtBookNo.Text + "|nNo : " + txtRunNo.Text + "|nCustomer Name : " + lblName.Text + " |nType : " + rdoType.SelectedItem.Text + " |nPrice : " + txtPrice.Text + "|nQuantity : " + txtQuan.Text + ""
                pContent.innerHTML = "Reference No : " + $get('<%=lblRefNo.ClientId %>').innerText + "<br/>Customer Name : " + $get('<%=lblName.ClientId %>').innerText + "<br/>Price : " + $get('<%=txtPrice.ClientId %>').value + "<br/>Quantity : " + $get('<%=txtQuan.ClientId %>').value;
            
                $get('<%=txtPwd.ClientId %>').value = '';
                $get('<%=txtPwd.ClientId %>').focus();
            }
            
            function okClick(){
            if( $get('<%=txtPwd.ClientId %>').value ==''){$get('<%=txtPwd.ClientId %>').focus();return};
            if( $get('<%=txtPwd.ClientId %>').value != $get('<%=hdfPwd.ClientId %>').value)
            {
                $get('<%=txtPwd.ClientId %>').focus();
                alert('Password Invalid.');
            }
            else{
                //  find the confirm ModalPopup and hide it   
                $get('<%=txtPwd.ClientId %>').value = ''; 
                this._popup.hide();
                //  use the cached button as the postback source
                __doPostBack(this._source.name, '');
                }
            }
            
            function cancelClick(){
                //  find the confirm ModalPopup and hide it 
                this._popup.hide();
                //  clear the event source
                this._source = null;
                this._popup = null;
            }
        </script>
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
   
    <div id="div_main" class="demoarea" style="background-color: white">
        <div class="demoheading">
            <table border="0" width="100%">
                <tr>
                    <td align="left" class="style22">
                        <asp:Label ID="lblTitle" runat="server" Text=""></asp:Label>
                    </td>
                    <td align="right" class="style22">
                    </td>
                </tr>
            </table>
        </div>
        <table border="0" width="100%">
            <tr>
                <td class="style13">
                    Customer Name* :
                </td>
                <td class="style15">
                    <asp:TextBox ID="txtCustName" Width="250px" runat="server" />
                    <ajaxToolkit:AutoCompleteExtender ID="txtCustName_AutoCompleteExtender" runat="server"
                        DelimiterCharacters="" CompletionInterval="300" Enabled="True" ServicePath="~/gtc.asmx"
                        UseContextKey="True" CompletionSetCount="20" OnClientItemSelected="onCustAutoCompleteClick"
                        TargetControlID="txtCustName" ServiceMethod="getCust_nameList">
                    </ajaxToolkit:AutoCompleteExtender>
                    &nbsp; <span style="display: none">
                        <asp:TextBox ID="txtCustRef" runat="server" /></span>
                    <ajaxToolkit:AutoCompleteExtender ID="txtCustRef_AutoCompleteExtender" runat="server"
                        DelimiterCharacters="" CompletionInterval="300" Enabled="True" ServicePath="~/gtc.asmx"
                        UseContextKey="True" CompletionSetCount="20" OnClientItemSelected="onCustAutoCompleteClick"
                        TargetControlID="txtCustRef" ServiceMethod="getCust_idList">
                    </ajaxToolkit:AutoCompleteExtender>
                   
                    <asp:ImageButton ID="imgSearchCustRef" runat="server" ImageUrl="~/images/search.bmp"
                        Style="width: 12px" />
                    <span style="display: none">
                        <asp:ImageButton ID="imgSearch" runat="server" ImageUrl="~/images/search.bmp" Style="width: 12px" /></span>
                </td>
            </tr>
            <tr>
                <td class="style13">
                    Type* :
                </td>
                <td class="style21">
                    <asp:RadioButtonList ID="rdoType" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem Value="sell">Sell</asp:ListItem>
                        <asp:ListItem Value="buy">Buy</asp:ListItem>
                    </asp:RadioButtonList>
                </td>
            </tr>
            <tr>
                <td colspan="2"> 
                </td>
            </tr>
        </table>
        
         <asp:UpdatePanel ID="udpFolio" runat="server" RenderMode="Inline">
            <ContentTemplate>
            <span style="display: none">
                <asp:Button ID="btnSearch" runat="server" Text="Search" />
                <asp:Label ID="lblName" runat="server" CssClass="lbl"></asp:Label>
            </span>
            </ContentTemplate>
         </asp:UpdatePanel>
        <uc1:ucPortFolio ID="ucPortFolio1" runat="server" />
        <%--<asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <table cellpadding="0" cellspacing="0" border="0" style="width: 100%">
                    <tr>
                        <td valign="top" style="width: 60%">
                            <fieldset>
                                <legend class="topic">Customer Detail</legend>
                                <table border="0" cellpadding="1" cellspacing="1" style="width: 100%">
                                    <tr>
                                        <td valign="top" class="style4">
                                            Ref No :
                                        </td>
                                        <td valign="top" class="style6">
                                            <asp:Label ID="lblCustRef" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td valign="top" style="width:100px;text-align:right">
                                            บุคคลติดต่อ :
                                        </td>
                                        <td valign="top">
                                            <asp:Label ID="lblPerson" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td valign="top">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td valign="top" class="style4">
                                            Name :
                                        </td>
                                        <td class="style6">
                                            <asp:Label ID="lblName" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td valign="top" style="text-align:right">
                                            Remark :
                                        </td>
                                        <td valign="top" class="style6" rowspan="3">
                                            <asp:Label ID="lblRemark" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td rowspan="3" valign="top">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td valign="top" class="style4">
                                            Address :
                                        </td>
                                        <td class="style6">
                                            <asp:Label ID="lblAdd" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td valign="top" class="style4">
                                            Phone :
                                        </td>
                                        <td class="style6">
                                            <asp:Label ID="lblPhone" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                    </tr>
                                    <tr>
                                        <td valign="top" class="style4">
                                            Fax :
                                        </td>
                                        <td class="style6">
                                            <asp:Label ID="lblFax" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td>
                                           
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td valign="top" class="style4">
                                            Mobile Phone :
                                        </td>
                                        <td class="style6">
                                            <asp:Label ID="lblMobilePhone" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td style="text-align:right">
                                            Trade Type :</td>
                                        <td>
                                            <asp:Label ID="lblTrade_type" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="style4">
                                            Account Bank :
                                        </td>
                                        <td class="style6">
                                            <asp:Label ID="lblAccBank" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td style="text-align:right">
                                            Branch :
                                        </td>
                                        <td>
                                            <asp:Label ID="lblBranch" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td class="style4">
                                            Account No :
                                        </td>
                                        <td class="style6">
                                            <asp:Label ID="lblAccNo" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td style="text-align:right">
                                            Account Name :
                                        </td>
                                        <td class="style2">
                                            <asp:Label ID="lblAccName" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;</td>
                                        <td >
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;</td>
                                        <td >
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;</td>
                                        <td >
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;</td>
                                        <td >
                                        </td>
                                        <td>
                                            &nbsp;&nbsp;</td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                        <td valign="top" style="width: 40%">
                            <fieldset>
                                <legend class="topic">Customer Asset</legend>
                                <table border="0" cellpadding="1" cellspacing="1" style="width: 100%">
                                    <tr>
                                        <td style="width:150px;text-align:right">
                                            Cash :
                                        </td>
                                        <td style="width:80px">
                                            &nbsp;
                                        </td>
                                        <td style="width:100px; text-align:right">
                                            <asp:Label ID="lblCash" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td valign="top">
                                            &nbsp;บาท</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right">
                                            Gold 96.50 :
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td style="text-align:right">
                                            <asp:Label ID="lblGold96" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;บาท</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right">
                                            Gold 99.99 :
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                        <td style="text-align:right">
                                            <asp:Label ID="lblGold99" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;kg&nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right">
                                            Trade Limit :
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                        <td style="text-align:right">
                                            <asp:Label ID="lblTradeLimit" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td>
                                           </td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right">
                                            Cash Balance:
                                        </td>
                                        <td valign="top">
                                            &nbsp;
                                        </td>
                                        <td style="text-align:right">
                                            <asp:Label ID="lblCashBalance" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td valign="top">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right">
                                            Revaluation :
                                        </td>
                                        <td>
                                        </td>
                                        <td style="text-align:right">
                                            <asp:Label ID="lblRevaluation" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right">
                                            Net Equity :
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td style="text-align:right">
                                            <asp:Label ID="lblNetEquity" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right">
                                            Margin Call :
                                        </td>
                                        <td align="center" valign="top">
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="lblMargin" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;%
                                        </td>
                                        <td style="text-align:right">
                                            <asp:Label ID="lblMargin2" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right">
                                            
                                            Free Margin :</td>
                                        <td align="center" valign="top">
                                           
                                        </td>
                                        <td style="text-align:right">
                                            <asp:Label ID="lblFreeMargin" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right">
                                            Excess/Short :
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td style="text-align:right">
                                            <asp:Label ID="lblExcess" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right">
                                            Force Close Level :
                                        </td>
                                        <td align="center" valign="top">
                                            &nbsp;&nbsp;&nbsp;&nbsp;
                                            <asp:Label ID="lblMargin_call" runat="server" CssClass="lbl"></asp:Label>
                                            &nbsp;%
                                        </td>
                                        <td style="text-align:right">
                                            <asp:Label ID="lblMargin_call2" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td style="text-align:right">
                                            Excess/Short Call :
                                        </td>
                                        <td>
                                            &nbsp;
                                        </td>
                                        <td style="text-align:right">
                                            <asp:Label ID="lblExcess_call" runat="server" CssClass="lbl"></asp:Label>
                                        </td>
                                        <td>
                                            &nbsp;</td>
                                    </tr>
                                </table>
                            </fieldset>
                        </td>
                    </tr>
                </table>
            </ContentTemplate>
        </asp:UpdatePanel>--%>
        <asp:UpdatePanel ID="udpMain" runat="server">
            <ContentTemplate>
            <div style="width:50%;float:left">
                <table border="0" width="100%">
                    <tr id="trRefno" style="display: none">
                        <td class="style54">
                            Ref No :
                        </td>
                        <td>
                            <asp:Label ID="lblRefNo" runat="server" Font-Bold="true" ForeColor="Black"></asp:Label>
                        </td>
                    </tr>
                    <tr id="trStatus" style="display: none">
                        <td class="style54">
                            Status :
                        </td>
                        <td valign="top">
                            <asp:Label ID="lblStatus" runat="server" Font-Bold="true"></asp:Label>
                        </td>
                    </tr>
                     <tr id="trDate" style="display: none">
                        <td class="style54" style="word-wrap:break-word">
                            Ticket_Date :
                        </td>
                        <td>
                            <asp:Label ID="lblDate" runat="server" Font-Bold="True" ForeColor="Black" Width="120px"></asp:Label>
                            <div style="display:none">
                            <asp:TextBox ID="txtDateNow" runat="server" />
                            <ajaxToolkit:CalendarExtender ID="txtDateNow_CalendarExtender"  runat="server" Enabled="true"
                                PopupButtonID="imgCalDateNow" PopupPosition="BottomRight" TargetControlID="txtDateNow">
                            </ajaxToolkit:CalendarExtender>
                            <asp:ImageButton ID="imgCalDateNow" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td class="style54">
                            Clearing (T+) :</td>
                        <td>
                            <asp:DropDownList ID="ddlClearing" runat="server" ></asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style54"">
                            Delivery_Date :
                        </td>
                        <td>
                            <asp:TextBox ID="txtDateDelivery" runat="server" />
                            <ajaxToolkit:CalendarExtender ID="txtDateDelivery_CalendarExtender" runat="server"
                                Enabled="True" PopupPosition="BottomRight" PopupButtonID="imgCalDateDelivery"
                                TargetControlID="txtDateDelivery">
                            </ajaxToolkit:CalendarExtender>
                            <asp:ImageButton ID="imgCalDateDelivery" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                        </td>
                    </tr>
                    <tr>
                        <td class="style54">
                            &nbsp;
                        </td>
                        <td>
                            <asp:Label ID="lblAlertSplitBill" runat="server" ForeColor="Red" Text="ไม่สามารถแก้ไขได้เนื่องจากมีการ Spilt Bill แล้ว."
                                Visible="false"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="style54">
                            Purity* :
                        </td>
                        <td valign="top">
                            <asp:RadioButtonList ID="rdoGoldType" runat="server" AppendDataBoundItems="True"
                                RepeatDirection="Horizontal" AutoPostBack="True">
                                <asp:ListItem Value="96">96.50</asp:ListItem>
                                <asp:ListItem Value="99">99.99</asp:ListItem>
                            </asp:RadioButtonList>
                            <asp:ObjectDataSource ID="objSrcGoldType" runat="server" OldValuesParameterFormatString="original_{0}"
                                SelectMethod="GetData" TypeName="dsTableAdapters.gold_typeTableAdapter"></asp:ObjectDataSource>
                            <asp:HiddenField ID="hdfExcess" runat="server" />
                            <asp:HiddenField ID="hdfPwd" runat="server" />
                            <asp:HiddenField ID="hdfPrice99Control" runat="server" />
                            <asp:HiddenField ID="hdfTradeLimit" Value="n" runat="server" />
                            <asp:HiddenField ID="hdfType" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td valign="middle" class="style54">
                            <span style="display: none">
                                <asp:LinkButton ID="linkColor" runat="server">color</asp:LinkButton>
                                <asp:LinkButton ID="linkClearPrice" runat="server">Clear</asp:LinkButton></span>
                        </td>
                        <td>
                            <table id="tblGold" style="display: block; width: 477px;">
                                <tr>
                                    <td>
                                        Quantity* :
                                    </td>
                                    <td id="td_96_2">
                                        <asp:TextBox ID="txtQuan" runat="server" />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Price* :
                                    </td>
                                    <td id="td_96_3">
                                        <asp:TextBox ID="txtPrice" runat="server" />
                                        baht
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        Amount* :
                                    </td>
                                    <td id="td_96_4">
                                        <asp:TextBox ID="txtAmount" runat="server" />
                                        baht
                                    </td>
                                </tr>
                                <tr style="display: none">
                                    <td class="style38">
                                    </td>
                                    <td id="td_96_4" class="style38">
                                        <asp:CheckBox ID="cbGoldDep" runat="server" Text="ตัดทองฝาก" />
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style50">
                                    </td>
                                    <td id="td_96_4" class="style50">
                                        <asp:Panel ID="pnPwd" runat="server" DefaultButton="linkPwd">
                                           <asp:TextBox ID="txtPwdAuth" TextMode="Password" runat="server" />
                                        <asp:LinkButton ID="linkPwd" runat="server" ForeColor="Red">Submit</asp:LinkButton>
                                        </asp:Panel>
                                        
                                    </td>
                                </tr>
                            </table>

                        </td>
                    </tr>

                    <tr>
                        <td valign="top" class="style54">
                            &nbsp;
                        </td>
                        <td>
                            &nbsp;
                        </td>
                    </tr>
                </table>
            </div>
            <div style="width:50%;float:right">
                 <table border="0" width="100%">
                                     <tr>
                        <td class="style54">
                            Payment :
                        </td>
                        <td valign="top">
                            <table>
                                <tr>
                                    <td>
                                        <asp:DropDownList ID="ddlPayment" runat="server">
                                            <asp:ListItem Selected="True" Value="">--None--</asp:ListItem>
                                            <asp:ListItem Value="cash">Cash</asp:ListItem>
                                            <asp:ListItem Value="cheq">Cheque</asp:ListItem>
                                            <asp:ListItem Value="trans">Payin</asp:ListItem>
                                        </asp:DropDownList>
                                        <%-- <asp:RadioButtonList  ID="rdoPayment" runat="server" RepeatDirection="Horizontal" Visible="false">
                                            <asp:ListItem Value="cash">Cash</asp:ListItem>
                                            <asp:ListItem Value="cheq">Cheque</asp:ListItem>
                                            <asp:ListItem Value="trans">Payin</asp:ListItem>
                                        </asp:RadioButtonList>--%>
                                    </td>
                                    <td>
                                        <table width="100%">
                                            <tr>
                                                <td id="bank1">
                                                    Bank :
                                                </td>
                                                <td id="bank2">
                                                    <asp:DropDownList ID="ddlBank" runat="server">
                                                    </asp:DropDownList>
                                                </td>
                                                <td id="cheq1">
                                                    Cheque no :
                                                </td>
                                                <td id="cheq2">
                                                    <asp:TextBox ID="txtCheq" runat="server" Width="80px"></asp:TextBox>
                                                </td>
                                                <td id="cheq3">
                                                    Due date :
                                                </td>
                                                <td id="cheq4">
                                                    <asp:TextBox ID="txtDuedate" runat="server" Width="70px"></asp:TextBox>
                                                    <ajaxToolkit:CalendarExtender ID="txtDuedate_CalendarExtender" runat="server" Enabled="True"
                                                        PopupButtonID="imgDuedate" PopupPosition="BottomRight" TargetControlID="txtDuedate">
                                                    </ajaxToolkit:CalendarExtender>
                                                    <asp:ImageButton ID="imgDuedate" runat="server" ImageUrl="~/images/Calendar_scheduleHS.png" />
                                                </td>
                                            </tr>
                                        </table>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td class="style54">
                            Delivery :
                        </td>
                        <td valign="top">
                            <asp:RadioButtonList ID="rdoDelivery" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="send">Send</asp:ListItem>
                                <asp:ListItem Value="receive">Receive</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style54">
                            Billing* :
                        </td>
                        <td valign="top">
                            <asp:RadioButtonList ID="rdoBilling" runat="server" RepeatDirection="Horizontal">
                                <asp:ListItem Value="y">Yes</asp:ListItem>
                                <asp:ListItem Value="n">No</asp:ListItem>
                            </asp:RadioButtonList>
                        </td>
                    </tr>
                    <tr>
                        <td class="style54">
                            Invoice :
                        </td>
                        <td valign="top">
                            <asp:TextBox ID="txtInvoice" runat="server" Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td valign="top" class="style54">
                            Remark :
                        </td>
                        <td>
                            <asp:TextBox ID="txtRemark" runat="server" Height="50px" TextMode="MultiLine" Width="450px"></asp:TextBox>
                        </td>
                    </tr>
                 </table>
            </div>
            <div style="clear:both;text-align:center">
                      <asp:Button ID="btnDealTicket" runat="server" Text="Deal Ticket" />
                            &nbsp;<asp:Button ID="btnUpdate" runat="server" Text="Update" />
                            &nbsp;<asp:Button ID="btnClear" runat="server" Text="Clear" />
                            &nbsp;<asp:Button ID="btnBack" Visible="false" runat="server" Text="Back" />
                            <ajaxToolkit:ModalPopupExtender ID="mppDelete" BehaviorID="mppDel" runat="server"
                                TargetControlID="Panel1" PopupControlID="Panel1" OkControlID="btnOk" OnOkScript="okClick();"
                                CancelControlID="btnNo" OnCancelScript="cancelClick();" BackgroundCssClass="modalBackground">
                            </ajaxToolkit:ModalPopupExtender>
                            <asp:Panel ID="Panel1" runat="server" Style="display:none" CssClass="modalPopup"  DefaultButton="btnOk">
                                <asp:Panel ID="Panel2" runat="server" Style="cursor: move; background-color: #DDDDDD;
                                    border: solid 1px Gray; color: Black">
                                    <div style="text-align: center">
                                        <p><h4>
                                            Do you want to update ticket.</h4>
                                        </p>
                                        <p id="pContent" style="text-align:justify">
                                        </p>
                                        <br />
                                        <div>
                                        <h4>Please Enter Password.</h4>
                                <asp:TextBox ID="txtPwd" runat="server" TextMode="Password"></asp:TextBox>
                                <asp:RequiredFieldValidator ID="rfvPwd" ValidationGroup="mppPwd" ControlToValidate="txtPwd"  runat="server" ErrorMessage="*"></asp:RequiredFieldValidator>
                                
                                </div>
                                    </div>
                                </asp:Panel>
                                
                                <div>
                                    <p style="text-align: center;">
                                        <asp:Button ID="btnOk" runat="server" ValidationGroup="mppPwd" Text="Yes" Width="50px" />
                                        <asp:Button ID="btnNo" runat="server" Text="No" Width="50px" />
                                    </p>
                                </div>
                            </asp:Panel>
            </div>
                    <div id="div_log">
                                <fieldset>
                                    <legend class="topic">Tickets Log Detail</legend>
                                    <asp:GridView ID="gvLog" runat="server" AutoGenerateColumns="False" BackColor="White"
                                        BorderColor="#999999" BorderStyle="None" BorderWidth="1px" CellPadding="3" DataSourceID="objSrcLog"
                                        GridLines="Vertical" PageSize="30" Width="100%">
                                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Ref_No" Visible="False">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="link" runat="server" Target="_blank"><%#Eval("ref_no")%></asp:HyperLink>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>
                                            <asp:BoundField DataField="firstname" HeaderText="Cust_Name">
                                                <ItemStyle Wrap="False" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="book_no" HeaderText="Book_No" Visible="False" />
                                            <asp:BoundField DataField="run_no" HeaderText="Run_No" Visible="False" />
                                            <asp:BoundField DataField="invoice" HeaderText="Invoice" />
                                            <asp:BoundField DataField="gold_type_id" HeaderText="Purity" />
                                            <asp:BoundField DataField="delivery" HeaderText="Delivery" />
                                            <asp:BoundField DataField="type" HeaderText="Buy/Sell" />
                                            <asp:BoundField DataField="quantity" DataFormatString="{0:#,##0.00000}" HeaderText="Quantity">
                                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="price" DataFormatString="{0:#,##0.00}" HeaderText="Price">
                                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="amount" DataFormatString="{0:#,##0.00}" HeaderText="Amount">
                                                <ItemStyle HorizontalAlign="Right" Wrap="False" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="delivery_date" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Delivery_Date" />
                                            <asp:BoundField DataField="ticket_date" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Ticket_Date" />
                                            <asp:BoundField DataField="billing" HeaderText="Billing" />
                                            <asp:BoundField DataField="remark" HeaderText="Remark" />
                                            <asp:BoundField DataField="payment" HeaderText="Payment" />
                                            <asp:BoundField DataField="payment_bank" HeaderText="Bank" />
                                            <asp:BoundField DataField="payment_duedate" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Duedate" />
                                            <asp:BoundField DataField="payment_cheq_no" HeaderText="Cheq_No" />
                                            <asp:BoundField DataField="status_name" HeaderText="Status">
                                                <ItemStyle Wrap="False" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="modifier_date" DataFormatString="{0:dd/MM/yyyy}" HeaderText="Update_Date" />
                                            <asp:BoundField DataField="update_by" HeaderText="Update_By" />
                                            <asp:BoundField DataField="active" HeaderText="Active" />
                                        </Columns>
                                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                        <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                                        <AlternatingRowStyle BackColor="Gainsboro" />
                                    </asp:GridView>
                                    <asp:ObjectDataSource ID="objSrcLog" runat="server" OldValuesParameterFormatString="original_{0}"
                                        SelectMethod="getTicketLogUpdate" TypeName="clsDB">
                                        <SelectParameters>
                                            <asp:Parameter ConvertEmptyStringToNull="False" Name="ref_no" Type="String" />
                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                </fieldset>
                            </div>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>
</asp:Content>
