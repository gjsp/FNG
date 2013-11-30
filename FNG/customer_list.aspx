<%@ Page Language="VB" AutoEventWireup="false" CodeFile="customer_list.aspx.vb" Inherits="customer_list" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script src="Scripts/jquery-1.4.1.js" type="text/javascript"></script>
     <link href="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/themes/base/jquery-ui.css" rel="stylesheet" type="text/css"/>
  <script src="http://ajax.googleapis.com/ajax/libs/jquery/1.5/jquery.min.js"></script>
  <script src="http://ajax.googleapis.com/ajax/libs/jqueryui/1.8/jquery-ui.min.js"></script>

    <script language="javascript" type="text/javascript">
        function blink(ele, speed) {
            if ($(ele).css("visibility") == "visible") {
                $(ele).css("visibility", "hidden");
            } else {
                $(ele).css("visibility", "visible");
            }
            setTimeout("blink('" + ele + "','" + speed + "')", speed);
        }
        $(function () {
           
            blink(".blink-text", 800);
            
        });

//        $(document).ready(function () {
//            $("#<%=lblP.clientId%>").click(function ()
//             $("#<%=lblP.clientId%>").effect("pulsate", { times: 3 }, 2000);
//             });
        //        });
        $(document).ready(function () {
            $("#divP").click(function () {
                //$("#divP").effect("pulsate", { times: 3 }, 2000);

                //$("#<%=lblP.clientId%>").effect("pulsate", {}, 5000);
                //$("#spP").effect("pulsate", { times: 3 }, 500);
                //$("#div1").effect("pulsate", { times: 2 }, 200);
                // $(this).effect("highlight", {}, 3000);
                $("#spP").show('highlight', { color: '#C8FB5E' }, 'fast');
                $("#div1").show('highlight', { color: '#C8FB5E' }, 'fast');
                //$('#div1').delay(1000).fadeOut().fadeIn('slow')
            });
        });


    </script>
</head>
<body>
    <form id="form1" runat="server">
    <div id="message_container" style="display: block">
        <span id="msg"></span>
        <a id="clear_ack" href="#" onclick="$(this).parents('div#message_container').fadeOut(4000); return false;">
            clear </a>
    </div>
    <div>
        <asp:Button ID="btn" runat="server" Text="Insert Customer" />
        <br />
        <span id="sBG" style="background-color: white"><span id="blink" style="color: #000000;">
            this text is blinking!</span> </span>&nbsp;<br />
        <asp:Button ID="btnUsr" runat="server" Text="Insert Username" />
        <br />
        <asp:Button ID="btnUsr0" runat="server" Text="Script Insert" />
        
        <br />
       <div class="blink-text">TODO write content<br />
        </div>
        <br />

        <div id="divP" style=" margin: 0px; width: 100px; height: 80px; background: green; border: 1px solid black; position: relative;">
        </div>  
        <asp:Label ID="lblP" runat="server" Text="Pulsate"></asp:Label>
      
        
        <br />
       
        <br />
        <div id="div1" style=" margin: 0px; width: 100px; height: 80px; background: green; border: 1px solid black; position: relative;"></div>
         <span id="spP"style=" margin: 0px; width: 100px; height: 80px; background: green; border: 1px solid black; position: relative;">Span</span>
        
        <br />

        <asp:GridView ID="GridView1" runat="server" BackColor="LightGoldenrodYellow" BorderColor="Tan"
            BorderWidth="1px" CellPadding="2" ForeColor="Black" GridLines="None">
            <AlternatingRowStyle BackColor="PaleGoldenrod" />
            <FooterStyle BackColor="Tan" />
            <HeaderStyle BackColor="Tan" Font-Bold="True" />
            <PagerStyle BackColor="PaleGoldenrod" ForeColor="DarkSlateBlue" HorizontalAlign="Center" />
            <SelectedRowStyle BackColor="DarkSlateBlue" ForeColor="GhostWhite" />
            <SortedAscendingCellStyle BackColor="#FAFAE7" />
            <SortedAscendingHeaderStyle BackColor="#DAC09E" />
            <SortedDescendingCellStyle BackColor="#E1DB9C" />
            <SortedDescendingHeaderStyle BackColor="#C2A47B" />
        </asp:GridView>
    </div>
    </form>
</body>
<script type="text/javascript">
    blinkMe();
    function blinkMe() {
        //if (document.getElementById('blink').style.color == "#000000" || document.getElementById('blink').style.color == "rgb(0, 0, 0)") {
        if (document.getElementById('sBG').style.backgroundColor == 'white') {
            //document.getElementById('blink').style.color = "#FFFFFF";

            //document.getElementById('blink').style.color = "#CCC999";
            document.getElementById('sBG').style.backgroundColor = "silver";
        }
        else {
            //document.getElementById('blink').style.color = "#000000";
            document.getElementById('sBG').style.backgroundColor = 'white';
        }
        setTimeout("blinkMe();", 900);
    }
</script>
</html>
