<%@ Page Language="VB" MasterPageFile="~/trade/admin/MasterPageAdmin.master" AutoEventWireup="false" CodeFile="edit_price.aspx.vb" Inherits="admin_edit_price" title="Manage Price" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">

    <script language="javascript" type="text/javascript">
    function addBid(lv,plus)
    {
        if(plus.value =='') return;
        var bid = parseFloat($get('<%=BidTxtbox.clientId %>').value);
        var bidPlus =  parseFloat(plus.value);
        if(lv==2)
        {
           $get('<%=BidTxtbox2.clientId %>').value = (bid+bidPlus).toFixed(2);;
        }
        if(lv==3)
        {
           $get('<%=BidTxtbox3.clientId %>').value = (bid+bidPlus).toFixed(2);;
        }
    }
    function addAsk(lv,plus)
    {
        if(plus.value =='') return;
        var ask = parseFloat($get('<%=AskTxtbox.clientId %>').value);
        var askPlus =  parseFloat(plus.value);
        if(lv==2)
        {
           $get('<%=AskTxtbox2.clientId %>').value = (ask+askPlus).toFixed(2);;
        }
        if(lv==3)
        {
           $get('<%=AskTxtbox3.clientId %>').value = (ask+askPlus).toFixed(2);;
        }
    }
    </script>
           
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
<div style="width:700px;margin:auto;">
<%--<div class="demoheading">Transaction</div>--%>
<fieldset class="k-fieldset" style="width:100%;"><legend class="topic">Price</legend>
<div style="margin: auto; text-align: left; width: 450px; margin-top:10px;">

   <div style="text-align: left;">
        <div style="float:left; width:50px; text-align:left;font-weight:bold;">Level 1</div>
       <div style="float:left; width:60px; text-align:right;color:#0066FF;">Bid :&nbsp; </div>
       <div style="float:left; width:100px; text-align:right; margin-left:5px;">
           <asp:Label ID="LabelBid" runat="server" Text="Label"></asp:Label> THB
        </div>
         <div style="float:left; width:60px; text-align:right;color:#FF6600;">Ask :&nbsp; </div>
       <div style="float:left; width:100px; text-align:right; margin-left:5px;">
            <asp:Label ID="LabelAsk" runat="server" Text="Label"></asp:Label> THB
        </div>
       <div style="clear:both;"></div>
   </div>
   <div style="text-align: left; margin-top:5px;">
        <div style="float:left; width:50px; text-align:left;font-weight:bold;">Level 2</div>
       <div style="float:left; width:60px; text-align:right;color:#0066FF;">Bid :&nbsp; </div>
       <div style="float:left; width:100px; text-align:right; margin-left:5px;">
           <asp:Label ID="LabelBid2" runat="server" Text="Label"></asp:Label> THB
        </div>
         <div style="float:left; width:60px; text-align:right;color:#FF6600;">Ask :&nbsp; </div>
       <div style="float:left; width:100px; text-align:right; margin-left:5px;">
            <asp:Label ID="LabelAsk2" runat="server" Text="Label"></asp:Label> THB
        </div>
       <div style="clear:both;"></div>
   </div>
   <div style="text-align: left; margin-top:5px;">
        <div style="float:left; width:50px; text-align:left;font-weight:bold;">Level 3</div>
       <div style="float:left; width:60px; text-align:right;color:#0066FF;">Bid :&nbsp; </div>
       <div style="float:left; width:100px; text-align:right; margin-left:5px;">
           <asp:Label ID="LabelBid3" runat="server" Text="Label"></asp:Label> THB
        </div>
         <div style="float:left; width:60px; text-align:right;color:#FF6600;">Ask :&nbsp; </div>
       <div style="float:left; width:100px; text-align:right; margin-left:5px;">
            <asp:Label ID="LabelAsk3" runat="server" Text="Label"></asp:Label> THB
        </div>
       <div style="clear:both;"></div>
   </div>
   <div style="text-align: left; margin-top:15px;">
   <asp:Timer ID="tmTime" OnTick="tmTime_Tick" runat="server" Interval="1000"></asp:Timer>
       <asp:HiddenField ID="hdfTime" runat="server" />
   <asp:UpdatePanel ID="upTimer" runat="server" UpdateMode="Conditional">
        <Triggers>
            <asp:AsyncPostBackTrigger ControlID="tmTime" EventName="Tick" />
        </Triggers>
        <ContentTemplate>
          <span style="font-weight:bold;">Inactive Pricing Time</span> <span style="padding-left:20px;"><asp:Label ID="lblTimer" Font-Bold="true" ForeColor="Red" runat="server" Text=""></asp:Label></span> <span style="padding-left:20px;"></span>
        </ContentTemplate>
   </asp:UpdatePanel>
   </div>
</div>
</fieldset>
  <div style="margin: auto;clear:both; text-align: left" />
  <br />
<fieldset class="k-fieldset" style="width:100%;margin:auto;"><legend class="topic">Update Price</legend>
   <div style="text-align: left; margin-top:15px;">
        <div style="float:left; width:50px; text-align:left;font-weight:bold;padding-top:2px;">Level 1</div>
       <div style="float:left; width:60px; text-align:right;color:#0066FF;padding-top:2px;">Bid :&nbsp; </div>
       <div style="float:left; width:100px; text-align:right; margin-left:5px;">
           <asp:TextBox ID="BidTxtbox" runat="server" MaxLength="50" 
               style="text-align:right;width:100px;font-size:12px;height:12px;" ></asp:TextBox>
        </div>
         <div style="float:left; width:60px; text-align:right;color:#FF6600;padding-top:2px;">Ask :&nbsp; </div>
       <div style="float:left; width:100px; text-align:right; margin-left:5px;">
            <asp:TextBox ID="AskTxtbox" runat="server" MaxLength="50" style="text-align:right;width:100px;font-size:12px;height:12px;" ></asp:TextBox>
        </div>
       <div style="clear:both;"></div>
   </div> 
   <div style="text-align: left; margin-top:15px;">
        <div style="float:left; width:50px; text-align:left;font-weight:bold;padding-top:2px;">Level 2</div>
       <div style="float:left; width:60px; text-align:right;color:#0066FF;padding-top:2px;">Bid :&nbsp; </div>
       <div style="float:left; width:100px; text-align:right; margin-left:5px;">
           <asp:TextBox ID="BidTxtbox2" runat="server" MaxLength="50" 
               style="text-align:right;width:100px;font-size:12px;height:12px;" ></asp:TextBox>
        </div>
       <div style="float:left; width:60px; text-align:right;color:#FF6600;padding-top:2px;">Ask :&nbsp; </div>
       <div style="float:left; width:100px; text-align:right; margin-left:5px;">
            <asp:TextBox ID="AskTxtbox2" runat="server" MaxLength="50" style="text-align:right;width:100px;font-size:12px;height:12px;" ></asp:TextBox>
        </div>
         <div style="float:left; width:60px; text-align:right;color:#0066FF;padding-top:2px;">+Bid :&nbsp; </div>
                <div style="float:left; width:50px; text-align:right; margin-left:5px;">
           <asp:TextBox ID="BidDiffTxtbox2" runat="server" MaxLength="50" 
               style="text-align:right;width:50px;font-size:12px;height:12px;" ></asp:TextBox>
        </div>
         <div style="float:left; width:60px; text-align:right;color:#FF6600;padding-top:2px;">-Ask :&nbsp; </div>
       <div style="float:left; width:50px; text-align:right; margin-left:5px;">
            <asp:TextBox ID="AskDiffTxtbox2" runat="server" MaxLength="50" style="text-align:right;width:50px;font-size:12px;height:12px;" ></asp:TextBox>
        </div>
       <div style="clear:both;"></div>
   </div>
   <div style="text-align: left; margin-top:15px;">
        <div style="float:left; width:50px; text-align:left;font-weight:bold;padding-top:2px;">Level 3</div>
       <div style="float:left; width:60px; text-align:right;color:#0066FF;padding-top:2px;">Bid :&nbsp; </div>
       <div style="float:left; width:100px; text-align:right; margin-left:5px;">
           <asp:TextBox ID="BidTxtbox3" runat="server" MaxLength="50" 
               style="text-align:right;width:100px;font-size:12px;height:12px;" ></asp:TextBox>
        </div>
       <div style="float:left; width:60px; text-align:right;color:#FF6600;padding-top:2px;">Ask :&nbsp; </div>
       <div style="float:left; width:100px; text-align:right; margin-left:5px;">
            <asp:TextBox ID="AskTxtbox3" runat="server" MaxLength="50" style="text-align:right;width:100px;font-size:12px;height:12px;" ></asp:TextBox>
        </div>
         <div style="float:left; width:60px; text-align:right;color:#0066FF;padding-top:2px;">+Bid :&nbsp; </div>
                <div style="float:left; width:50px; text-align:right; margin-left:5px;">
           <asp:TextBox ID="BidDiffTxtbox3" runat="server" MaxLength="50" 
               style="text-align:right;width:50px;font-size:12px;height:12px;" ></asp:TextBox>
        </div>
         <div style="float:left; width:60px; text-align:right;color:#FF6600;padding-top:2px;">-Ask :&nbsp; </div>
       <div style="float:left; width:50px; text-align:right; margin-left:5px;">
            <asp:TextBox ID="AskDiffTxtbox3" runat="server" MaxLength="50" style="text-align:right;width:50px;font-size:12px;height:12px;" ></asp:TextBox>
        </div>
       <div style="clear:both;"></div>
   </div>
   <div style="text-align: left; margin-top:15px;">
        <div style="float:left; width:130px; text-align:left;font-weight:bold;padding-top:2px;">Inactive Pricing Time</div>
       <div style="float:left; width:300px; text-align:left; margin-left:5px;">
           <asp:TextBox ID="TimeTxtBox" runat="server" MaxLength="50" 
               style="text-align:right;width:80px;font-size:12px;height:12px;" ></asp:TextBox>
        </div>
       <div style="clear:both;"></div>
   </div>
   <div style="text-align: center; margin-top:30px;">
        <asp:Button ID="editbt" CssClass="buttonPro small grey" runat="server" Text="Save" />
   </div>
   </fieldset>
 </div>
</asp:Content>

