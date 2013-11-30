<%@ Page Language="VB" AutoEventWireup="false" CodeFile="stock_now.aspx.vb" Inherits="stock_now" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <link href="main.css" rel="stylesheet" type="text/css" />

    <script src="js/functionst.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">
    function OnComplete(result) 
    {
        try {
                var strSplit = '|';
                $get('lblGoldComplete96').innerText = result.toString().split(strSplit)[0];
                $get('lblGoldComplete99N').innerText = result.toString().split(strSplit)[1];
                $get('lblGoldComplete99L').innerText = result.toString().split(strSplit)[2];
                
                $get('lblGoldBase96').innerText = result.toString().split(strSplit)[3];
                $get('lblGoldBase99N').innerText = result.toString().split(strSplit)[4];
                $get('lblGoldBase99L').innerText = result.toString().split(strSplit)[5];
                
                $get('lblGoldBranch96').innerText = result.toString().split(strSplit)[6];
                $get('lblGoldBranch99N').innerText = result.toString().split(strSplit)[7];
                $get('lblGoldBranch99L').innerText = result.toString().split(strSplit)[8];
                
                $get('lblGoldDep96').innerText = result.toString().split(strSplit)[9];
                $get('lblGoldDep99N').innerText = result.toString().split(strSplit)[10];
                $get('lblGoldDep99L').innerText = result.toString().split(strSplit)[11];
              
                $get('lblGoldCredit96').innerText = result.toString().split(strSplit)[12];
                $get('lblGoldCredit99N').innerText = result.toString().split(strSplit)[13];
                $get('lblGoldCredit99L').innerText = result.toString().split(strSplit)[14];
                
                $get('lblCashComplete').innerText = result.toString().split(strSplit)[15];
                $get('lblCashBase').innerText = result.toString().split(strSplit)[16];
                $get('lblCashBranch').innerText = result.toString().split(strSplit)[17];
                $get('lblCashDeposit').innerText = result.toString().split(strSplit)[18];
                $get('lblCashCredit').innerText = result.toString().split(strSplit)[19];
                
                $get('lblGoldPending96').innerText = result.toString().split(strSplit)[20];
                $get('lblGoldPending99N').innerText = result.toString().split(strSplit)[21];
                $get('lblGoldPending99L').innerText = result.toString().split(strSplit)[22];
                $get('lblCashPending').innerText = result.toString().split(strSplit)[23];
                
            }
        catch (err) {
        //alert(err.description);
            return false;
            } 
    }
    function refreshStock()
    {
        $get('btnRefresh').click();
        setTimeout("refreshStock()",4000)
    }   
    </script>
    <style type="text/css">
        .style1
        {
            text-align:right;
        }
        </style>
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" EnablePageMethods="true" runat="server">
    </asp:ScriptManager>
    <div class="demoarea">
        <asp:UpdatePanel ID="udpMain" runat="server">
            <ContentTemplate>
            <div align="center" 
                    style="background-color:#C4E1FF; color:black;padding-top:20px;padding-bottom:20px">
                <table border="0">
                <tr>
                        <td class="style1">
                            ทองรอส่ง</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            ทอง 96 :</td>
                        <td class="style1">
                            <asp:Label ID="lblGoldPending96" runat="server" Font-Bold="true" Font-Size="Small"></asp:Label>
                        </td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            ทอง 99 Non LBMA :</td>
                        <td class="style1">
                            <asp:Label ID="lblGoldPending99N" runat="server" Font-Bold="true" Font-Size="Small"></asp:Label>
                        </td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            ทอง 99 LBMA :</td>
                        <td class="style1">
                            <asp:Label ID="lblGoldPending99L" runat="server" Font-Bold="true" Font-Size="Small"></asp:Label>
                        </td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style1">
                            เงินรอส่ง</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            เงิน :</td>
                        <td class="style1">
                            <asp:Label ID="lblCashPending" runat="server" Font-Bold="True" 
                                Font-Size="Small"></asp:Label>
                        </td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style1" rowspan="3">
                            ทองบริษัท</td>
                        <td class="style1">
                        </td>
                        <td class="style1">
                            </td>
                        <td class="style1">
                            ทองซื้อขาย96 :</td>
                        <td class="style1">
                            <asp:Label ID="lblGoldComplete96" runat="server" Font-Bold="true" Font-Size="Small"></asp:Label>
                        </td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            ทองซื้อขาย99 Non LBMA :</td>
                        <td class="style1">
                            <asp:Label ID="lblGoldComplete99N" runat="server" Font-Bold="true" Font-Size="Small"></asp:Label>
                        </td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            ทองซื้อขาย99 LBMA :</td>
                        <td class="style1">
                            <asp:Label ID="lblGoldComplete99L" runat="server" Font-Bold="true" Font-Size="Small"></asp:Label>
                        </td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            ทองตั้งต้น96 :</td>
                        <td class="style1">
                            <asp:Label ID="lblGoldBase96" runat="server" Font-Bold="true" Font-Size="Small"></asp:Label>
                        </td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            ทองตั้งต้น 99 Non LBMA :</td>
                        <td class="style1">
                            <asp:Label ID="lblGoldBase99N" runat="server" Font-Bold="True" 
                                Font-Size="Small"></asp:Label>
                        </td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            ทองตั้งต้น 99 LBMA :</td>
                        <td class="style1">
                            <asp:Label ID="lblGoldBase99L" runat="server" Font-Bold="True" 
                                Font-Size="Small"></asp:Label>
                        </td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            ทองฝากสาขา96 :</td>
                        <td class="style1">
                            <asp:Label ID="lblGoldBranch96" runat="server" Font-Bold="true" Font-Size="Small"></asp:Label>
                        </td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            ทองฝากสาขา 99 Non LBMA :</td>
                        <td class="style1">
                            <asp:Label ID="lblGoldBranch99N" runat="server" Font-Bold="true" 
                                Font-Size="Small"></asp:Label>
                        </td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            ทองฝากสาขา 99 LBMA :</td>
                        <td class="style1">
                            <asp:Label ID="lblGoldBranch99L" runat="server" Font-Bold="true" 
                                Font-Size="Small"></asp:Label>
                        </td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style1" rowspan="2">
                            ทองลูกค้า</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            ทองฝาก96 :</td>
                        <td class="style1">
                            <asp:Label ID="lblGoldDep96" runat="server" Font-Bold="true" Font-Size="Small"></asp:Label>
                        </td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            ทองฝาก 99 Non LBMA :</td>
                        <td class="style1">
                            <asp:Label ID="lblGoldDep99N" runat="server" Font-Bold="true" Font-Size="Small"></asp:Label>
                        </td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            ทองฝาก 99 LBMA</td>
                        <td class="style1">
                            <asp:Label ID="lblGoldDep99L" runat="server" Font-Bold="true" Font-Size="Small"></asp:Label>
                        </td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            ทองมัดจำ96 :</td>
                        <td class="style1">
                            <asp:Label ID="lblGoldCredit96" runat="server" Font-Bold="true" Font-Size="Small"></asp:Label>
                        </td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            ทองมัดจำ 99 Non LBMA :</td>
                        <td class="style1">
                            <asp:Label ID="lblGoldCredit99N" runat="server" Font-Bold="true" Font-Size="Small"></asp:Label>
                        </td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            ทองมัดจำ 99 LBMA :</td>
                        <td class="style1">
                            <asp:Label ID="lblGoldCredit99L" runat="server" Font-Bold="true" Font-Size="Small"></asp:Label>
                        </td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style1" rowspan="3">
                            เงินบริษัท
                        </td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            เงินซื้อขาย :</td>
                        <td class="style1">
                            <asp:Label ID="lblCashComplete" runat="server" Font-Bold="True" 
                                Font-Size="Small"></asp:Label>
                        </td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            เงินตั้งต้น :</td>
                        <td class="style1">
                            <asp:Label ID="lblCashBase" runat="server" Font-Bold="True" 
                                Font-Size="Small"></asp:Label>
                        </td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            เงินฝากสาขา :</td>
                        <td class="style1">
                            <asp:Label ID="lblCashBranch" runat="server" Font-Bold="true" Font-Size="Small"></asp:Label>
                        </td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style1" rowspan="2">
                            เงินลูกค้า</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            เงินฝาก :</td>
                        <td class="style1">
                            <asp:Label ID="lblCashDeposit" runat="server" Font-Bold="True" 
                                Font-Size="Small"></asp:Label>
                        </td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            เงินมัดจำ :</td>
                        <td class="style1">
                            <asp:Label ID="lblCashCredit" runat="server" Font-Bold="True" Font-Size="Small"></asp:Label>
                        </td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                        <td class="style1">
                            &nbsp;</td>
                    </tr>
                </table>
            </div>
            </ContentTemplate>
        </asp:UpdatePanel>
        <asp:Button ID="btnRefresh" runat="server" Text="" />
    </div>
    </form>
</body>
</html>
