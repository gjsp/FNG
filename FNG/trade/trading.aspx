<%@ Page Language="VB" MasterPageFile="~/trade/tradeMasterPage.master" AutoEventWireup="false"
    CodeFile="trading.aspx.vb" Inherits="trading" Title="Trade" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="ajaxToolkit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
    <script src="../Scripts/jquery-1.4.1.js" type="text/javascript"></script>
    <script type="text/javascript" language="javascript">

        //  that is going to be removed
        var _source;
        // keep track of the popup div
        var _popup;
        //var _dbTime = '';

        function showConfirm(source) {
            //validate Control

            try {

                $get('div_ifmTime').style.display = 'none';
                if (!(source.id == $get('<%=btn99AccLeaveSellShow.ClientId %>').id || source.id == $get('<%=btn99AccLeaveBuyShow.ClientId %>').id || source.id == $get('<%=btn96AccLeaveSellShow.ClientId %>').id || source.id == $get('<%=btn96AccLeaveBuyShow.ClientId %>').id)) {
                    $get('ifmTime').src = 'trade_timing.aspx'
                }

                var txt99PriceLeave = $get('<%=txt99PriceLeave.ClientId %>');
                var txt96PriceLeave = $get('<%=txt96PriceLeave.ClientId %>');
               
                //for content
                //((conditions) ? "true" : "false")
                var type = ''; var purity = ''; var price = ''; var quan = ''; var period = ''; var unit = '';
                var orderContent = '';
                var dbTime = '<%=dbTime %>';
                var today = dbTime.toString().split('|')[0]
                period = '24.00'; //FNG Leave order มีเวลาเดียว
                //if (parseInt(dbTime.toString().split('|')[1]) >= 16) {
                //    period = '24.00';
                //} else {
                //    period = '16.00';
                //}

                //Order 99
                if (source.id == $get('<%=btn99Accept.ClientId %>').id || source.id == $get('<%=btn99Accept2.ClientId %>').id) {
                    var ddl99Quan = $get('<%=ddl99Quan.ClientId %>');
                    if (ddl99Quan.selectedIndex == 0) { alert('โปรดเลือกปริมาณที่ต้องการ'); ddl99Quan.focus(); return };
                    if (source.id == $get('<%=btn99Accept.ClientId %>').id) {
                        type = 'Sell';
                        price = $get('<%=hdfBid99.ClientId %>').value;
                    } else {
                        type = 'Buy';
                        price = $get('<%=hdfAsk99.ClientId %>').value;
                    }
                    quan = ddl99Quan.options[ddl99Quan.selectedIndex].value;
                    purity = '99.99';
                    unit = ' KG';
                }
                //Order 96
                if (source.id == $get('<%=btn96Accept.ClientId %>').id || source.id == $get('<%=btn96Accept2.ClientId %>').id) {
                    var ddl96Quan = $get('<%=ddl96Quan.ClientId %>');
                    if (ddl96Quan.selectedIndex == 0) { alert('โปรดเลือกปริมาณที่ต้องการ'); ddl96Quan.focus(); return };

                    if (source.id == $get('<%=btn96Accept.ClientId %>').id) {
                        type = 'Sell';
                        price = $get('<%=hdfBid96.ClientId %>').value;
                    } else {
                        type = 'Buy';
                        price = $get('<%=hdfAsk96.ClientId %>').value;
                    }
                    quan = ddl96Quan.options[ddl96Quan.selectedIndex].value;
                    purity = '96.50';
                    unit = ' บาท';
                }

                //Leave Order 99
                if (source.id == $get('<%=btn99AccLeaveSellShow.ClientId %>').id || source.id == $get('<%=btn99AccLeaveBuyShow.ClientId %>').id) {

                    if (txt99PriceLeave.value == '') { alert('โปรดกำหนดราคาที่ต้องการ'); txt99PriceLeave.focus(); return };
                    if (isNaN(txt99PriceLeave.value)) { alert('ท่านสามารถกรอกได้เฉพาะตัวเลข'); txt99PriceLeave.focus(); return };
                    if (parseInt(txt99PriceLeave.value) <= 0) { alert('ท่านสามารถกรอกได้เฉพาะตัวเลขที่มากกว่าศูนย์'); txt99PriceLeave.focus(); return };

                    var ddl99QuanLeave = $get('<%=ddl99QuanLeave.ClientId %>');
                    if (ddl99QuanLeave.selectedIndex == 0) { alert('โปรดเลือกปริมาณที่ต้องการ'); ddl99QuanLeave.focus(); return };
                    //price leave order ต้องลงท้ายด้วย 0,5
                    if (!(txt99PriceLeave.value.substr(txt99PriceLeave.value.length - 1, 1) == 0 || txt99PriceLeave.value.substr(txt99PriceLeave.value.length - 1, 1) == 5)) { alert('ท่านระบุราคาไม่ถูกต้อง'); return };

                    //set confirm
                    quan = ddl99QuanLeave.options[ddl99QuanLeave.selectedIndex].value;
                    if (source.id == $get('<%=btn99AccLeaveSellShow.ClientId %>').id) { type = 'Sell' } else { type = 'Buy' }
                    price = txt99PriceLeave.value;
                    purity = '99.99';
                    unit = ' KG';
                    orderContent = '\nมีผล ถึงเวลา ' + period + ' \nวันที่  ' + today;
                }
                //Leave Order 96
                if (source.id == $get('<%=btn96AccLeaveSellShow.ClientId %>').id || source.id == $get('<%=btn96AccLeaveBuyShow.ClientId %>').id) {

                    if (txt96PriceLeave.value == '') { alert('โปรดกำหนดราคาที่ต้องการ'); txt96PriceLeave.focus(); return };
                    if (isNaN(txt96PriceLeave.value)) { alert('ท่านสามารถกรอกได้เฉพาะตัวเลข'); txt96PriceLeave.focus(); return };
                    if (parseInt(txt96PriceLeave.value) <= 0) { alert('ท่านสามารถกรอกได้เฉพาะตัวเลขที่มากกว่าศูนย์'); txt96PriceLeave.focus(); return };

                    var ddl96QuanLeave = $get('<%=ddl96QuanLeave.ClientId %>');
                    if (ddl96QuanLeave.selectedIndex == 0) { alert('โปรดเลือกปริมาณที่ต้องการ'); ddl96QuanLeave.focus(); return };
                    //price leave order ต้องลงท้ายด้วย 0,5
                    if (!(txt96PriceLeave.value.substr(txt96PriceLeave.value.length - 1, 1) == 0 || txt96PriceLeave.value.substr(txt96PriceLeave.value.length - 1, 1) == 5)) { alert('ท่านระบุราคาไม่ถูกต้อง'); return };

                    //set confirm
                    quan = ddl96QuanLeave.options[ddl96QuanLeave.selectedIndex].value;
                    if (source.id == $get('<%=btn96AccLeaveSellShow.ClientId %>').id) { type = 'Sell' } else { type = 'Buy' }
                    price = txt96PriceLeave.value;
                    purity = '96.50';
                    unit = ' บาท';
                    orderContent = '\nมีผล ถึงเวลา ' + period + ' \nวันที่  ' + today;
                }

                //Order Mini 96
                if (source.id == $get('<%=btn96MiniSellAcceptShow.ClientId %>').id || source.id == $get('<%=btn96MiniBuyAcceptShow.ClientId %>').id) {
                    var ddl96MiniQuan = $get('<%=ddl96MiniQuan.ClientId %>');
                    if (ddl96MiniQuan.selectedIndex == 0) { alert('โปรดเลือกปริมาณที่ต้องการ'); ddl96MiniQuan.focus(); return };
                    var priceTemp = '';

                    quan = ddl96MiniQuan.options[ddl96MiniQuan.selectedIndex].value;
                    if (source.id == $get('<%=btn96MiniSellAcceptShow.ClientId %>').id) {
                        type = 'Sell';
                        //priceTemp = $get('<%=hdfBid96Mini.ClientId %>').value;
                        //miniPrice = (priceTemp * quan) - (quan * 10);
                        price = $get('<%=hdfBid96Mini.ClientId %>').value;
                    } else {
                        type = 'Buy';
                        //priceTemp = $get('<%=hdfAsk96Mini.ClientId %>').value;
                        //miniPrice = (priceTemp * quan) + (quan * 10);
                        price = $get('<%=hdfAsk96Mini.ClientId %>').value;
                    }
                    purity = '96.50(Mini)';
                    unit = ' บาท';
                    //miniPrice = '\nรวมเป็นเงินทั้งหมด ' + addCommas(miniPrice);
                }

                var type_content = '';
                if (type == 'Sell') { type_content = 'ขาย'; } else { type_content = 'ซื้อ'; }
                $get('pContent').innerHTML = 'คุณต้องการที่จะ' + type_content + 'ทองคำ ' + purity + '\nที่ราคา ' + addCommas(price) + ' จำนวน ' + quan + unit + orderContent ;
                $get('<%=hdfPriceCust.ClientId %>').value = price; //เก็บราคาใน 10 วิ

                this._source = source;
                this._popup = $find('mppAccept');
                this._popup.show();
                $get('<%=btnOk.ClientId %>').focus();
            } catch (e) {
                alert('Error Code 101.');
            }
        }

        function getClientTime(t) {
            var t1 = parseInt(new Date().getTime());
            var t2 = t1 - t;
            $get('<%=hdfDiffTime.ClientId %>').value = t2;
            var iTime = t1 - parseInt($get('<%=hdfDiffTime.ClientId %>').value);
            var x = (new Date(iTime)).toString().substring(11, 13);

        }
        function getHour() {
            var localTime = parseInt(new Date().getTime());
            var iTime = localTime - parseInt($get('<%=hdfDiffTime.ClientId %>').value);
            return (new Date(iTime)).toString().substring(11, 13);
        }

        function OnFailed(error, userContext, methodName) { }

        function okClick() {
            try {
                this._popup.hide();
                //__doPostBack(this._source.name, '');
                switch (this._source.id) {
                    case 'ctl00_MainContent_btn99Accept':
                        document.getElementById('ctl00_MainContent_btn99SellAccept').click();
                        break;
                    case 'ctl00_MainContent_btn99Accept2':
                        document.getElementById('ctl00_MainContent_btn99BuyAccept').click();
                        break;
                    case 'ctl00_MainContent_btn96Accept':
                        document.getElementById('ctl00_MainContent_btn96SellAccept').click();
                        break;
                    case 'ctl00_MainContent_btn96Accept2':
                        document.getElementById('ctl00_MainContent_btn96BuyAccept').click();
                        break;

                    case 'ctl00_MainContent_btn99AccLeaveSellShow':
                        document.getElementById('ctl00_MainContent_btn99AcceptLeave').click();
                        break;
                    case 'ctl00_MainContent_btn99AccLeaveBuyShow':
                        document.getElementById('ctl00_MainContent_btn99AcceptLeave2').click();
                        break;
                    case 'ctl00_MainContent_btn96AccLeaveSellShow':
                        document.getElementById('ctl00_MainContent_btn96AcceptLeave').click();
                        break;
                    case 'ctl00_MainContent_btn96AccLeaveBuyShow':
                        document.getElementById('ctl00_MainContent_btn96AcceptLeave2').click();
                        break;

                    case 'ctl00_MainContent_btn96MiniSellAcceptShow':
                        document.getElementById('ctl00_MainContent_btn96MiniSellAccept').click();
                        break;
                    case 'ctl00_MainContent_btn96MiniBuyAcceptShow':
                        document.getElementById('ctl00_MainContent_btn96MiniBuyAccept').click();
                        break;
                    case 'ctl00_MainContent_btn96MiniLeaveSellAcceptShow':
                        document.getElementById('ctl00_MainContent_btn96LeaveMiniSellAccept').click();
                        break;
                    case 'ctl00_MainContent_btn96MiniLeaveBuyAcceptShow':
                        document.getElementById('ctl00_MainContent_btn96LeaveMiniBuyAccept').click();
                        break;
                }
            } catch (e) {
                alert('Error Code 102.');
            }
        }

        function cancelClick() {
            $get('ifmTime').src = '';
            this._popup.hide();
            this._source = null;
            this._popup = null;
        }
        function setConfirm(source) {
            if (source.checked == true) {
                $get('ctl00_MainContent_hdfConfirm').value = 'y';
                ifmTran.document.getElementById('hdfConfirm').value = 'y';
            }
            else {
                $get('ctl00_MainContent_hdfConfirm').value = 'n';
                ifmTran.document.getElementById('hdfConfirm').value = 'n';
            }
        }

        function pageLoad() {
            getMessage();
        }
        function getMessage() {
            PageMethods.getMessage(OnSucceeded, OnFailed);
        }

        function OnSucceeded(result, userContext, methodName) {
            $get('<%=lblMsg.clientId%>').innerHTML = "<marquee onmouseover='stop();' onmouseout='start();'>" + result + "</marquee>";
            setTimeout("getMessage()", 120000);
        }
        function OnFailed(error, userContext, methodName) { }

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


      

        //Leave Price 99.99
        $(document).ready(function () {
            $("#<%=txt99PriceLeave.clientId%>").keyup(function () {
                var quan = $(this).val();
                if (!isNaN(quan)) {
                    if (quan.indexOf(".") >= 0) {
                        $(this).css('background-color', 'red');
                    } else {
                        var price = $("#<%=txt99PriceLeave.clientId%>").val();
                        if ((price.substr(price.length - 1, 1) == 0 || price.substr(price.length - 1, 1) == 5)) {
                            if (parseFloat(price) <= 0) {
                                $(this).css('background-color', 'red');
                            } else {
                                $(this).css('background-color', 'white');
                            }
                        }
                        else { $(this).css('background-color', 'red'); }
                    }
                }
                else { $(this).css('background-color', 'red'); }
            });
        });
        $(document).ready(function () {
            $("#<%=txt99PriceLeave.clientId%>").focus(function ()
            { $(this).addClass("focus"); });
        });
        $(document).ready(function () {
            $("#<%=txt99PriceLeave.clientId%>").blur(function ()
            { $(this).removeClass("focus"); });
        });

      
        //Leave Price 96.50
        $(document).ready(function () {
            $("#<%=txt96PriceLeave.clientId%>").keyup(function () {
                var quan = $(this).val();
                if (!isNaN(quan)) {
                    if (quan.indexOf(".") >= 0) {
                        $(this).css('background-color', 'red');
                    } else {
                        var price = $("#<%=txt96PriceLeave.clientId%>").val();
                        if ((price.substr(price.length - 1, 1) == 0 || price.substr(price.length - 1, 1) == 5)) {
                            if (parseFloat(price) <= 0) {
                                $(this).css('background-color', 'red');
                            } else {
                                $(this).css('background-color', 'white');
                            }
                        }
                        else { $(this).css('background-color', 'red'); }
                    }
                }
                else { $(this).css('background-color', 'red'); }
            });
        });
        $(document).ready(function () {
            $("#<%=txt96PriceLeave.clientId%>").focus(function ()
            { $(this).addClass("focus"); });
        });
        $(document).ready(function () {
            $("#<%=txt96PriceLeave.clientId%>").blur(function ()
            { $(this).removeClass("focus"); });
        });

      
        $(document).ready(function () {
            $(".btn-slide").click(function () {
                $("#panel").slideToggle("slow");
                $(this).toggleClass("active"); return false;
            });
        });

        $(document).ready(function () {
            $(".slideLeave").click(function () {
                $("#div_leave").slideToggle("slow");
                $(this).toggleClass("active");
                if ($("#imgSlideLeave").attr("src") == "admin/image/expand.jpg") {
                    $("#imgSlideLeave").attr("src", "admin/image/collapse.jpg");
                } else {
                    $("#imgSlideLeave").attr("src", "admin/image/expand.jpg");
                }
                return false;
            });
        });

        $(document).ready(function () {
            $(".slideOrder").click(function () {
                $("#div_order").slideToggle("slow");
                $(this).toggleClass("active");
                if ($("#imgSlideOrder").attr("src") == "admin/image/expand.jpg") {
                    $("#imgSlideOrder").attr("src", "admin/image/collapse.jpg");
                } else {
                    $("#imgSlideOrder").attr("src", "admin/image/expand.jpg");
                }
                return false;
            });
        });

        $(document).ready(function () {
            $(".slideMini").click(function () {
                $("#div_Mini").slideToggle("slow");
                $(this).toggleClass("active");
                if ($("#imgSlideMini").attr("src") == "admin/image/expand.jpg") {
                    $("#imgSlideMini").attr("src", "admin/image/collapse.jpg");
                } else {
                    $("#imgSlideMini").attr("src", "admin/image/expand.jpg");
                }
                return false;
            });
        });

    </script>
    <style type="text/css">
        .focus
        {
            border: 2px solid #0066FF;
        }
        #panel
        {
            background: #fff;
            display: block;
        }
        .slide
        {
            margin: 0;
            padding: 0;
            border-top: solid 4px #fff;
        }
        .slideOrder
        {
            color: #fff;
        }
        .slideLeave
        {
            color: #fff;
        }
        .btn-slide
        {
            background: url(img/white-arrow.gif) no-repeat right -50px;
            background-color: Blue;
            text-align: center;
            width: 100%;
            height: 21px;
            padding: 0px 0px 0 0;
            margin: 0 auto;
            display: block;
            font: bold 120%/100% Arial, Helvetica, sans-serif;
            color: #fff;
            text-decoration: none;
        }
        .active
        {
            background-position: right 12px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="demoarea">
        <div id="div_msg" style="width: 100%; height: 25px;">
            <asp:Label ID="lblMsg" runat="server" Text="" Font-Size="Medium" ForeColor="Red"></asp:Label>
        </div>
        <asp:UpdatePanel runat="server" ID="upPrice" RenderMode="Inline" UpdateMode="Conditional">
            <Triggers>
                <asp:AsyncPostBackTrigger ControlID="btn99Accept" EventName="click" />
                <asp:AsyncPostBackTrigger ControlID="btn99AcceptLeave" EventName="click" />
                <asp:AsyncPostBackTrigger ControlID="btn99Accept2" EventName="click" />
                <asp:AsyncPostBackTrigger ControlID="btn99AcceptLeave2" EventName="click" />
                <asp:AsyncPostBackTrigger ControlID="btn96Accept" EventName="click" />
                <asp:AsyncPostBackTrigger ControlID="btn96AcceptLeave" EventName="click" />
                <asp:AsyncPostBackTrigger ControlID="btn96Accept2" EventName="click" />
                <asp:AsyncPostBackTrigger ControlID="btn96AcceptLeave2" EventName="click" />
                <asp:AsyncPostBackTrigger ControlID="btn99SellAccept" EventName="click" />
                <asp:AsyncPostBackTrigger ControlID="btn99BuyAccept" EventName="click" />
                <asp:AsyncPostBackTrigger ControlID="btn96SellAccept" EventName="click" />
                <asp:AsyncPostBackTrigger ControlID="btn96BuyAccept" EventName="click" />
                <asp:AsyncPostBackTrigger ControlID="btn96MiniSellAccept" EventName="click" />
                <asp:AsyncPostBackTrigger ControlID="btn96MiniBuyAccept" EventName="click" />
                <asp:AsyncPostBackTrigger ControlID="btn96LeaveMiniSellAccept" EventName="click" />
                <asp:AsyncPostBackTrigger ControlID="btn96LeaveMiniBuyAccept" EventName="click" />
            </Triggers>
        </asp:UpdatePanel>
        <fieldset class="k-fieldset">
            <legend class="topic">ราคา</legend>
            <iframe id="ifmPrice" width="100%" height="250px" frameborder="0" scrolling="no"
                marginheight="0" marginwidth="0" src="price.aspx"></iframe>
        </fieldset>
        <asp:Panel ID="pnDelay" runat="server" Style="display: none">
            <div style="text-align: center;">
                <asp:UpdateProgress ID="upDelay" runat="server">
                    <ProgressTemplate>
                        <div id="Div1" align="center" valign="middle" runat="server" style="position: absolute;
                            left: 40%; padding: 10px 10px 10px 10px; visibility: visible; border-color: silver;
                            border-style: solid; border-width: 1px; background-color: White; width: 100px;
                            text-align: left">
                            <img src="img/indicator.gif" style="vertical-align: middle" alt="Processing" />
                            Please Wait...
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
        </asp:Panel>
        &nbsp;
        <fieldset class="k-fieldset">
            <legend class="topic">
                <img alt="" id="imgSlideOrder" src="admin/image/collapse.jpg" class="slideOrder"
                    style="cursor: pointer" />
                ส่งคำสั่งซื้อ/ขาย ณ ราคาปัจจุบัน</legend>
            <div id="div_order">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td>
                            <div id="div99" style="text-align: center; float: left; padding-left: 20px">
                                <table cellpadding="1" cellspacing="1" border="0" width="400px">
                                    <tr>
                                        <td align="right">
                                            ปริมาณ :
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="ddl99Quan" runat="server">
                                            </asp:DropDownList>
                                            &nbsp;KG.
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            &nbsp;
                                        </td>
                                        <td align="left">
                                            <asp:Button ID="btn99Accept" OnClientClick="showConfirm(this); return false;" runat="server"
                                                Text="ลูกค้าขาย" CssClass="buttonPro small red" Width="80px" />
                                            &nbsp;
                                            <asp:Button ID="btn99Accept2" OnClientClick="showConfirm(this); return false;" runat="server"
                                                Text="ลูกค้าซื้อ" CssClass="buttonPro small blue" Width="80px" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <td>
                            <div id="div96" style="text-align: center; float: left; padding-left: 20px;">
                                <table cellpadding="1" cellspacing="1" border="0" width="400px">
                                    <tr>
                                        <td align="right">
                                            ปริมาณ :
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="ddl96Quan" runat="server">
                                            </asp:DropDownList>
                                          
                                            &nbsp;BAHT
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            &nbsp;
                                        </td>
                                        <td align="left">
                                            <asp:Button ID="btn96Accept" OnClientClick="showConfirm(this); return false;" runat="server"
                                                Text="ลูกค้าขาย" CssClass="buttonPro small red" Width="80px" />
                                            &nbsp;
                                            <asp:Button ID="btn96Accept2" OnClientClick="showConfirm(this); return false;" runat="server"
                                                Text="ลูกค้าซื้อ" CssClass="buttonPro small blue" Width="80px" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <td>
                            <div id="divMini" style="text-align: center; float: left; padding-left: 20px;">
                                <table cellpadding="1" cellspacing="1" border="0" style="width: 400px">
                                    <tr>
                                        <td align="right">
                                            ปริมาณ :
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="ddl96MiniQuan" runat="server">
                                            </asp:DropDownList>
                                            &nbsp;BAHT
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td align="left">
                                            <asp:Button ID="btn96MiniSellAcceptShow" OnClientClick="showConfirm(this); return false;"
                                                CssClass="buttonPro small red" runat="server" Text="ลูกค้าขาย" Width="80px" />
                                            &nbsp;
                                            <asp:Button ID="btn96MiniBuyAcceptShow" OnClientClick="showConfirm(this); return false;"
                                                CssClass="buttonPro small blue" runat="server" Text="ลูกค้าซื้อ" Width="80px" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>
        </fieldset>
        &nbsp;
        <fieldset class="k-fieldset" style="display: block">
            <legend class="topic" style="vertical-align: top;">
                <img alt="" id="imgSlideLeave" src="admin/image/collapse.jpg" class="slideLeave"
                    style="cursor: pointer" />
                ตั้งราคาซื้อ/ขายล่วงหน้า</legend>
            <div id="div_leave">
                <table cellpadding="0" cellspacing="0" border="0">
                    <tr>
                        <td>
                            <div id="div2" style="text-align: center; float: left; padding-left: 20px">
                                <table cellpadding="1" cellspacing="1" border="0" width="400px">
                                    <tr>
                                        <td align="right">
                                            ราคา :</td>
                                        <td align="left">
                                    <asp:TextBox ID="txt99PriceLeave" CssClass="txt" runat="server" Width="150px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            ปริมาณ :
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="ddl99QuanLeave" runat="server">
                                    </asp:DropDownList>
                                            &nbsp;KG.</td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            &nbsp;
                                        </td>
                                        <td align="left">
                                    <asp:Button ID="btn99AccLeaveSellShow" OnClientClick="showConfirm(this); return false;"
                                        CssClass="buttonPro small red" runat="server" Text="ลูกค้าขาย" Width="80px" />
                                    &nbsp;
                                    <asp:Button ID="btn99AccLeaveBuyShow" OnClientClick="showConfirm(this); return false;"
                                        CssClass="buttonPro small blue" runat="server" Text="ลูกค้าซื้อ" Width="80px" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <td>
                            <div id="div3" style="text-align: center; float: left; padding-left: 20px;">
                                <table cellpadding="1" cellspacing="1" border="0" width="400px">
                                    <tr>
                                        <td align="right">
                                            ราคา :</td>
                                        <td align="left">
                                    <asp:TextBox ID="txt96PriceLeave" CssClass="txt" runat="server" Width="150px"></asp:TextBox>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            ปริมาณ :
                                        </td>
                                        <td align="left">
                                            <asp:DropDownList ID="ddl96QuanLeave" runat="server">
                                    </asp:DropDownList>
                                    &nbsp;BAHT
                                        </td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            &nbsp;
                                        </td>
                                        <td align="left">
                                    <asp:Button ID="btn96AccLeaveSellShow" OnClientClick="showConfirm(this); return false;"
                                        CssClass="buttonPro small red" runat="server" Text="ลูกค้าขาย" Width="80px" />
                                    &nbsp;
                                    <asp:Button ID="btn96AccLeaveBuyShow" OnClientClick="showConfirm(this); return false;"
                                        CssClass="buttonPro small blue" runat="server" Text="ลูกค้าซื้อ" Width="80px" />
                                        </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                        <td>
                            <div id="div4" style="text-align: center; float: left; padding-left: 20px;">
                                <table cellpadding="1" cellspacing="1" border="0" style="width: 400px">
                                    <tr>
                                        <td align="right">
                                            &nbsp;</td>
                                        <td align="left">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td align="right">
                                            &nbsp;</td>
                                        <td align="left">
                                            &nbsp;</td>
                                    </tr>
                                    <tr>
                                        <td>
                                        </td>
                                        <td align="left">
                                            &nbsp;
                                            </td>
                                    </tr>
                                </table>
                            </div>
                        </td>
                    </tr>
                </table>
            </div>

        </fieldset>
        &nbsp;
<%--        <fieldset class="k-fieldset">
            <legend class="topic">
                <img alt="" id="imgSlideMini" src="admin/image/collapse.jpg" class="slideMini" style="cursor: pointer" />
                Mini 96.50</legend>
            <div id="div_Mini">
                <div style="text-align: center; float: left">
                    <iframe id="ifmPriceGm" width="550px" height="200px" frameborder="0" scrolling="no"
                        marginheight="0" marginwidth="0" src="price_mini.aspx"></iframe>
                </div>
                <div style="text-align: center; float: right">
                </div>
                <div style="display: block">
                </div>
            </div>
        </fieldset>--%>
        <ajaxToolkit:ModalPopupExtender ID="mppAccept" BehaviorID="mppAccept" runat="server"
            TargetControlID="Panel1" PopupControlID="Panel1" OkControlID="btnOk" OnOkScript="okClick();"
            CancelControlID="btnNo" OnCancelScript="cancelClick();" BackgroundCssClass="modalBackground">
        </ajaxToolkit:ModalPopupExtender>
        <asp:Panel ID="Panel1" runat="server" Style="display: none" CssClass="modalPopup" DefaultButton="btnOk">
            <asp:Panel ID="Panel2" runat="server" Style="cursor: move; background-color: #fff;
                border: solid 0px Gray; color: Black">
                <div style="text-align: center;">
                    <p id="pContent" style="text-align: center; margin: 50px 20px 20px 20px; font-size:x-large;">
                    </p>
                    <div style="height: 60px">
                        <div id="div_ifmTime" style="display: none">
                            <iframe id="ifmTime" width="100%" height="60px" frameborder="0" scrolling="no" marginheight="0"
                                marginwidth="0" src="#"></iframe>
                        </div>
                    </div>
                </div>
            </asp:Panel>
            <div>
                <p style="text-align: center;">
                    <asp:Button ID="btnOk" CssClass="buttonPro small black" runat="server" Text="Yes"
                        Width="90px" />&nbsp;&nbsp;
                    <asp:Button ID="btnNo" CssClass="buttonPro small black" runat="server" Text="No"
                        Width="90px" />
                </p>
            </div>
        </asp:Panel>
        <div>
            <ajaxToolkit:ModalPopupExtender ID="mppDelay" runat="server" TargetControlID="pnDelay"
                PopupControlID="pnDelay" BackgroundCssClass="modalBackgroundProgress">
            </ajaxToolkit:ModalPopupExtender>
        </div>
        <div style="display: none">
            <%--Button show confirm popup from iFrame call--%>
            <%--Button Call function from (show confirm function) ที่ทำงานจริงๆ--%>
            <asp:Button ID="btn99SellAccept" runat="server" Text="Sell Order Accept" />
            <asp:Button ID="btn99BuyAccept" runat="server" Text="Buy Order Accept" />
            <asp:Button ID="btn96SellAccept" runat="server" Text="Sell Order Accept" />
            <asp:Button ID="btn96BuyAccept" runat="server" Text="Buy Order Accept" />
            <br />
            <asp:Button ID="btn99AcceptLeave" runat="server" Text="Sell Leave Order Accept" />
            <asp:Button ID="btn99AcceptLeave2" runat="server" Text="Buy Leave Order Accept" />
            <asp:Button ID="btn96AcceptLeave" runat="server" Text="Sell Leave Order Accept" />
            <asp:Button ID="btn96AcceptLeave2" runat="server" Text="Buy Leave Order Accept" />
            <br />
            <asp:Button ID="btn96MiniSellAccept" runat="server" />
            <asp:Button ID="btn96MiniBuyAccept" runat="server" />
            <asp:Button ID="btn96LeaveMiniSellAccept" runat="server" />
            <asp:Button ID="btn96LeaveMiniBuyAccept" runat="server" />
            <br />
            <asp:CheckBox ID="cbConfirm" runat="server" Text="Fast Trade" />
            <asp:HiddenField ID="hdfBid99" runat="server" />
            <asp:HiddenField ID="hdfAsk99" runat="server" />
            <asp:HiddenField ID="hdfBid96" runat="server" />
            <asp:HiddenField ID="hdfAsk96" runat="server" />
            <asp:HiddenField ID="hdfTradeLimit" runat="server" />
            <asp:HiddenField ID="hdfCust_id" runat="server" />
            <asp:HiddenField ID="hdfConfirm" runat="server" />
            <asp:HiddenField ID="hdfBid96Mini" runat="server" />
            <asp:HiddenField ID="hdfAsk96Mini" runat="server" />
            <asp:HiddenField ID="hdfPriceCust" runat="server" />
            <asp:HiddenField ID="hdfDiffTime" runat="server" />
        </div>
        <div>
            <cc1:TabContainer ID="tcTrade" runat="server" ActiveTabIndex="1" Width="100%" 
                CssClass="Tab" Font-Names="Tahoma">
                <cc1:TabPanel ID="tpOrder" runat="server" HeaderText="Order">
                    <HeaderTemplate>
                        ส่งคำสั่งซื้อ/ขาย ณ ราคาปัจจุบัน</HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="ifmTran" width="100%" frameborder="0" scrolling="no" marginheight="0"
                            marginwidth="0" src="cust_trans.aspx?m=tran"></iframe>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="tpLeave" runat="server" HeaderText="Leave Order">
                    <HeaderTemplate>
                        ตั้งราคาซื้อ/ขายล่วงหน้า</HeaderTemplate>
                    <ContentTemplate>
                        <iframe id="ifmLeave" width="100%" frameborder="0" scrolling="no" marginheight="0"
                            marginwidth="0" src="cust_trans.aspx?m=leave"></iframe>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="tpAccept" runat="server" HeaderText="รายการซื้อขายที่เกิดขึ้นแล้ว">
                    <ContentTemplate>
                        <iframe id="ifmAcc" width="100%" frameborder="0" scrolling="no" marginheight="0"
                            marginwidth="0" src="cust_trans.aspx?m=accept"></iframe>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="tpReject" runat="server" HeaderText="รายการซื้อขายที่ถูกปฏิเสธ">
                    <ContentTemplate>
                        <iframe id="ifmRej" width="100%" frameborder="0" scrolling="no" marginheight="0"
                            marginwidth="0" src="cust_trans.aspx?m=reject"></iframe>
                    </ContentTemplate>
                </cc1:TabPanel>
                <cc1:TabPanel ID="tpBlotter" runat="server" HeaderText="สรุปรายการซื้อขาย">
                    <ContentTemplate>
                        <iframe id="ifmBlotter" width="100%" frameborder="0" scrolling="no" marginheight="0"
                            marginwidth="0" src="cust_trans_search.aspx?m=blotter"></iframe>
                    </ContentTemplate>
                </cc1:TabPanel>
            </cc1:TabContainer>
        </div>
    </div>
    <script language="javascript" type="text/javascript">
        var mppDelay = '<%= mppDelay.ClientID %>';
    </script>
    <script src="js/jsUpdateProgress.js" type="text/javascript"></script>
</asp:Content>
