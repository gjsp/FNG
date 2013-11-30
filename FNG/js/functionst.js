//Function

      function setFocus(cid) {
      if (cid != "") {var currentTextBox = document.getElementById(cid);
      currentTextBox.focus();
      if (currentTextBox.createTextRange) {
      var range = currentTextBox.createTextRange();
      range.move('character', currentTextBox.value.length);
      range.select();}}} 


function enableWindow(mode)
{
    var window1 = document.getElementById('tr1');
    var window2 = document.getElementById('tr2');
    var window3 = document.getElementById('tr3');
    if(mode=="1")
    {
        window1.style.display = 'block';
        window2.style.display = 'block';
        window3.style.display = 'none';
    }
    else
    {
        window1.style.display = 'none';
        window2.style.display = 'none';
        window3.style.display = 'block';
    }
}

function onCustNameAutoCompleteClick(source, eventArgs) 
{   
    $get('ctl00_SampleContent_hdfCust_id').value = eventArgs.get_value();
    $get('ctl00_SampleContent_btnSearch').click();
}


function disableCtrlAltrKeyCombination(e)
{
        var ctrlb='b';
        var ctrls='s';
        var key;
        var isCtrl;
        var isAltr;


//For Ctrl
//For Internet Explorer


        if(window.event)
        {
                key = window.event.keyCode;
                if(window.event.ctrlKey)
                        isCtrl = true;
                else
                        isCtrl = false;
        }
//For Firefox
        else
        {
                key = e.which;     //firefox
                if(e.ctrlKey)
                        isCtrl = true;
                else
                        isCtrl = false;
        }




//For Alter
//For Internet Explorer
//        if(window.event)
//        {
//                key = window.event.keyCode;     //IE
//                if(window.event.altrKey)
//                        isAltr = true;
//                else
//                        isAltr = false;
//        }
//For Firefox
//        else
//        {
//                key = e.which;     //firefox
//                if(e.altrKey)
//                        isAltr = true;
//                else
//                        isAltr= false;
//        }




//Check For CTRL+U
        if(isCtrl)
        {
		    if(ctrls.toLowerCase() == String.fromCharCode(key).toLowerCase())
		    {
                		    $get('ctl00_SampleContent_rdoType').cells[0].parentNode.cells[0].parentNode.childNodes[0].childNodes[0].checked = true;
                		    div_main.style.backgroundColor = '#FFFFEA';
                    		
		    }
	        if(ctrlb.toLowerCase() == String.fromCharCode(key).toLowerCase())
		    {
                		    $get('ctl00_SampleContent_rdoType').cells[1].parentNode.cells[1].parentNode.childNodes[1].childNodes[0].checked = true;
                		    div_main.style.backgroundColor = '#E1F0FF';
                    		
		    }
             return false;
        }
        
        




//Check For ALTR+S
//        if(isCtrl)
//        {
//                if(altrs.toLowerCase() == String.fromCharCode(key).toLowerCase())
//		{
//                		alert('Key combination ALTR + S has been disabled.');
//		}
//                return false;
//        }
//        return true;
}


function change_payment(rdo)
    {
      var pp = '';
     
       if (pp == "cash")
       {
        bank1.style.display = none;
        bank2.style.display = none;
        cheq1.style.display = none;
        cheq2.style.display = none;
        cheq3.style.display = none;
        cheq4.style.display = none;
       } 
       else if (pp == "trans")
       {
        bank1.style.display = block;
        bank2.style.display = block;
        cheq1.style.display = none;
        cheq2.style.display = none;
        cheq3.style.display = none;
        cheq4.style.display = none;
       }
       else if (pp == "cheq")
       {
        bank1.style.display = none;
        bank2.style.display = none;
        cheq1.style.display = block;
        cheq2.style.display = block;
        cheq3.style.display = block;
        cheq4.style.display = block;
       }
    }

function formatWithComma(number) {
var formattedNumberString = (number%1000).toString();
var x = parseInt(number/1000);
while(x > 0) {
formattedNumberString = x%1000 + ',' + formattedNumberString;
x = parseInt(x/1000);
}
return formattedNumberString;
}



function isCheck(){
var frm = document.getElementById('aspnetForm');
    var IsCheck= "false";
    for (i=0;i<frm.elements.length;i++){
        if (frm.elements[i].type == "checkbox"){
            if( frm.elements[i].name.split('$')[4] == 'cbRow')
            {
                if(frm.elements[i].checked == true)
                {
                    IsCheck = 'true'
                }
            }
         }
    }
    if(IsCheck=='false'){
        alert('Please checked Item to Receipt');
        return false;
    }else
    {
        return true;
    }
}
function switchCheckbox(id){
var frm = document.getElementById('aspnetForm');
    
    for (i=0;i<frm.elements.length;i++)
    {
        if (frm.elements[i].type == "checkbox")
        {
            if(frm.elements[i].name.split('$')[4] == 'cbHead' || frm.elements[i].name.split('$')[4] == 'cbRow')
            {
                frm.elements[i].checked = id.checked;
            }
        }
    }
}


function openCustomerList(page) {
        var showCustomer;
        var MyLeft = (screen.width - 650) / 2;
        var MyTop = (screen.height - 600) / 2;

        var page_option = "width=750,height=400,left=" + MyLeft + ",top=" + MyTop + ",resizable=no,scrollbars=no,status=yes,titlebar=no,toolbar=no,location=no";

        if (showCustomer) {
            if (window.focus) { showCustomer.focus(); }
        }
        else {
            showCustomer = window.open("customer_popup.aspx?page=" + page + "", "showPopup", page_option);
            if (window.focus) { showCustomer.focus(); }
        }
    }
    
    
function numberOnly() {
    e_k=event.keyCode;
    // !=0
    //if($get(txt_id).value=='' && e_k==48)
    //    {event.returnValue= false;};
    //only number
    if (e_k < 48 || (e_k > 57)) {
    event.returnValue = false;
    }
}

function checkNumber() {
    e_k=event.keyCode;
    // !=0
    //if($get(txt_id).value=='' && e_k==48)
    //    {event.returnValue= false;};
    //only number
    //if (e_k != 45){
        if (e_k < 48 || e_k > 57) {
            if(e_k!=46){
                event.returnValue = false;
            }   
        }
    //}
}

function IsNumeric(sText)
{
   var ValidChars = "0123456789.";
   var IsNumber=true;
   var Char;

 
   for (i = 0; i < sText.length && IsNumber == true; i++) 
      { 
      Char = sText.charAt(i); 
      if (ValidChars.indexOf(Char) == -1) 
         {
         IsNumber = false;
         }
      }
   return IsNumber;
   
   }



//function checkNumber(num){
//    var digits = ".0123456789";
//    var temp;
//    for(var i=0;i<num.length;i++)
//    {
//        temp=num.substring(i,i+1)
//        if(digits.indexOf(temp)==-1)
//        {
//            alert("Number Only");
//            return false;
//        }
//     }

//}

function number_format(number, decimals, dec_point, thousands_sep) {
    var exponent = "";
    var numberstr = number.toString ();
    var eindex = numberstr.indexOf ("e");
    if (eindex > -1) {
    exponent = numberstr.substring (eindex);
    number = parseFloat (numberstr.substring (0, eindex));
    }
    if (decimals != null) {
    var temp = Math.pow (10, decimals);
    number = Math.round (number * temp) / temp;
    }
    var sign = number < 0 ? "-" : "";
    var integer = (number > 0 ?
    Math.floor (number) : Math.abs (Math.ceil (number))).toString ();
    var fractional = number.toString ().substring (integer.length + sign.length);
    dec_point = dec_point != null ? dec_point : ".";
    fractional = decimals != null && decimals > 0 || fractional.length > 1 ? (dec_point + fractional.substring (1)) : "";
    if (decimals != null && decimals > 0) {
    for (i = fractional.length - 1, z = decimals; i < z; ++i) {
    fractional += "0";
    }
    }
    thousands_sep = (thousands_sep != dec_point || fractional.length == 0) ? thousands_sep : null;
    if (thousands_sep != null && thousands_sep != "") {
    for (i = integer.length - 3; i > 0; i -= 3){
    integer = integer.substring (0 , i) + thousands_sep + integer.substring (i);
    }
    }
    return sign + integer + fractional + exponent;
}


function nextFocusName(name, count, limit) {
    e_k = event.keyCode;
    if (e_k == 9) { return; };
    id = name.id.split('z')[0] + 'z' + (count + 1);
    if (e_k == 8) {
        id = name.id.split('z')[0] + 'z' + (count - 1);
        if (count == 1) { return false; }
    }
    if (e_k < 48 || (e_k > 57)) {
        if (count == limit && e_k != 8) { return false; }
        txt = $get(id);
        txt.focus();

        if (txt.createTextRange) {
            var range = txt.createTextRange();
            range.move('character', txt.value.length);
            range.select();
        }
    }
}

function nextFocusId(name, count, limit) {
    e_k = event.keyCode;
    if (e_k == 9) { return; };
    id = name.id.split('z')[0] + 'z' + (count + 1);
    if (e_k == 8) {
        id = name.id.split('z')[0] + 'z' + (count - 1);
        if (count == 1) { return false; }
    }
    if ((e_k < 48 || (e_k > 57)) && (e_k < 96 || (e_k > 105))) {

    }
    else {
        if (count == limit && e_k != 8) { return false; }
        txt = $get(id);
        txt.focus();

        if (txt.createTextRange) {
            var range = txt.createTextRange();
            range.move('character', txt.value.length);
            range.select();
        }
    }
    if (e_k == 8) {
        txt = $get(id);
        txt.focus();

        if (txt.createTextRange) {
            var range = txt.createTextRange();
            range.move('character', txt.value.length);
            range.select();
        }
    }
}