
function setFocus(cid) {
    if (cid != "") {
        var currentTextBox = document.getElementById(cid);
        currentTextBox.focus();
        if (currentTextBox.createTextRange) {
            var range = currentTextBox.createTextRange();
            range.move('character', currentTextBox.value.length);
            range.select();
        } 
    }
} 

function checkNumber() {
    e_k=event.keyCode;
        if (e_k < 48 || e_k > 57) {
            if(e_k!=46){
                //if(e_k!=45){
                event.returnValue = false;
                //} 
            }   
        }
}

function fastConfirm(ctrl,msg){
    if( $get(ctrl).value=='n')
    {
        return confirm(msg);
    }
    else{
        return true;
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

//function valAskDiff(sender, args) {
//    if ($get(sender.id).value == '') { args.IsValid = false; }
//    
//    args.IsValid = true;
//}

function nextFocusName(name, count,limit) {
    e_k = event.keyCode;
    if (e_k == 9) { return; };
    if (e_k == 16) { return; };
    id = name.id.split('z')[0]+ 'z' + (count + 1);
    if (e_k == 8) {
        id = name.id.split('z')[0] + 'z' + (count - 1);
        if (count == 1) { return false; }
    }
    if (e_k < 48 || (e_k > 57)) {
        if (count == limit && e_k!=8) { return false; }
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
    if (e_k == 16) { return; };
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