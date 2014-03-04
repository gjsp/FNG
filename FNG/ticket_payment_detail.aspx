<%@ Page Language="VB" MasterPageFile="~/DefaultMaster.master" AutoEventWireup="false"
    CodeFile="ticket_payment_detail.aspx.vb" Inherits="ticket_payment_detail" Title="Payment Detail" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="AjaxToolkit" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
  <script type="text/javascript">

      function modalHide() {
          $find("modalPopUpExtender1").hide();
      }

      function switchCheckbox(id) {
          var frm = document.getElementById('aspnetForm');

          for (i = 0; i < frm.elements.length; i++) {
              if (frm.elements[i].type == "checkbox") {
                  if (frm.elements[i].name.split('$')[4] == 'cbHead' || frm.elements[i].name.split('$')[4] == 'cbRow') {
                      frm.elements[i].checked = id.checked;
                  }
              }
          }
      }

    </script>
    <style type="text/css">
    .modalBackground {
        background-color:black;filter:alpha(opacity=70);opacity:0.7;
        } 
    </style>
    </asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="SampleContent" runat="Server">
    <div class="demoarea">
        <div class="demoheading">
            Payment Detail
        </div>
        <div style="width: 900px">
            <table cellpadding="0" cellspacing="0" border="0" width="100%">
                <tr>
                    <td>
                        <fieldset><legend class="topic">Payment</legend>
                       
                            <table width="100%" style="color:black">
                                <tr>
                                    <td>
                                       Payment ID :<asp:Label ID="lblpid" runat="server" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td>
                                         Create By :<asp:Label ID="lblBy" runat="server" Font-Bold="true"></asp:Label>
                                    </td>
                                    <td>
                                         Create Date : <asp:Label ID="lblDate" runat="server" Font-Bold="true"></asp:Label>
                                    </td>
                                </tr>
                            </table>
                        </fieldset>
                        
                    </td>
                </tr>
                <tr>
                    <td>
                        <fieldset>
                            <legend class="topic">Ticket List</legend>
                            <asp:UpdatePanel ID="upGv" runat="server">
                                <ContentTemplate>
                                    <div style="width:700px">
                                    <div style="text-align:right">
                                    <asp:LinkButton ID="linkAdd" runat="server" Text="Add Ticket"></asp:LinkButton>
                                        &nbsp;&nbsp;
                                    <asp:LinkButton ID="linkReport" runat="server" Text="Payment"></asp:LinkButton>
                                    </div>
                                    <asp:GridView ID="gv" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                        BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                        CellPadding="3" GridLines="Vertical" Width="100%"
                                        DataKeyNames="ticket_id,ref_no">
                                        <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                        <AlternatingRowStyle BackColor="Gainsboro" />
                                        <Columns>
                                            <asp:TemplateField HeaderText="Ticket_ref" SortExpression="ref_no">
                                                <ItemTemplate>
                                                    <asp:HyperLink ID="linkRef" runat="server" Target="_blank"><%#Eval("ref_no")%></asp:HyperLink>
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" />
                                            </asp:TemplateField>

                                            <asp:BoundField DataField="price" HeaderText="Price" DataFormatString="{0:#,##0}" >
                                            <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="quantity" HeaderText="Quantity" DataFormatString="{0:#,##0}" >
                                            <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="amount" HeaderText="Amount" DataFormatString="{0:#,##0.00}" >
                                            <ItemStyle HorizontalAlign="Right" />
                                            </asp:BoundField>
                                            <asp:BoundField DataField="payment_by" HeaderText="Created_By" />
                                            <asp:TemplateField>
                                                <ItemTemplate>
                                                    <asp:ImageButton ID="imgDel" runat="server" ImageUrl="~/images/i_del.png" OnClick="imgDel_Click" />
                                                </ItemTemplate>
                                                <ItemStyle HorizontalAlign="Center" Width="20px" />
                                            </asp:TemplateField>
                                        </Columns>
                                        <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                        <HeaderStyle BackColor="#274B98" Font-Bold="True" ForeColor="White" />
                                        <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                        <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                    </asp:GridView>
                                    <asp:ObjectDataSource ID="objSrc" runat="server" OldValuesParameterFormatString="original_{0}"
                                        SelectMethod="getPaymentDetail" TypeName="clsFng">
                                        <SelectParameters>
                                            <asp:Parameter ConvertEmptyStringToNull="False" Name="payment_id" Type="String" />

                                        </SelectParameters>
                                    </asp:ObjectDataSource>
                                    </div>
                                    <asp:Button runat="server" ID="btnModalPopUp" Style="display: none" />
                                    <AjaxToolkit:ModalPopupExtender ID="modalPopUpExtender1" runat="server"
                                        TargetControlID="btnModalPopUp"
                                        PopupControlID="pnlPopUp"
                                        BackgroundCssClass="modalBackground"
                                        OkControlID="btnOk">
                                    </AjaxToolkit:ModalPopupExtender>
                                    <asp:Panel runat="Server" ID="pnlPopUp">
                                        <div style="text-align:center">
                                            <div style="overflow:scroll ; height: 500px">
                                                <asp:GridView ID="gvPopup" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                                    BackColor="White" BorderColor="#999999" BorderStyle="None" BorderWidth="1px"
                                                    CellPadding="3" GridLines="Vertical" Width="700px" class="GridviewTable"
                                                    DataKeyNames="ticket_id,ref_no">
                                                    <RowStyle BackColor="#EEEEEE" ForeColor="Black" />
                                                    <AlternatingRowStyle BackColor="Gainsboro" />
                                                    <Columns>
                                                        <asp:BoundField DataField="ref_no" HeaderText="Ticket_Ref">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="price" HeaderText="Price" DataFormatString="{0:#,##0}">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="quantity" HeaderText="Quantity" DataFormatString="{0:#,##0}">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="amount" HeaderText="Amount" DataFormatString="{0:#,##0.00}">
                                                            <ItemStyle HorizontalAlign="Right" />
                                                        </asp:BoundField>
                                                        <asp:BoundField DataField="payment_by" HeaderText="Created_By" />
                                                  
                                                        <asp:TemplateField>
                                                            <ItemTemplate>
                                                                <asp:LinkButton ID="link" runat="server" OnClick="link_Click">Add</asp:LinkButton>
                                                            </ItemTemplate>
                                                        </asp:TemplateField>
                                                    </Columns>
                                                    <FooterStyle BackColor="#CCCCCC" ForeColor="Black" />
                                                    <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
                                                    <PagerStyle BackColor="#999999" ForeColor="Black" HorizontalAlign="Center" />
                                                    <SelectedRowStyle BackColor="#008A8C" Font-Bold="True" ForeColor="White" />
                                                </asp:GridView>
                                            </div>
                                            <asp:Button ID="btnOk" runat="server" Text="Cancel" OnClientClick="modalHide();" />
                                        </div>
                                    </asp:Panel>
                                </ContentTemplate>
                            </asp:UpdatePanel>
                        </fieldset>
                    </td>
                </tr>
            </table>
            
        </div>
    </div>
</asp:Content>
