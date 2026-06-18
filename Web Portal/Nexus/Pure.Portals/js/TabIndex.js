// JScript File

var sTabIndexID;
var ctrlInputHiddenTabs;
var ctrlInputVisibleTabs;
var Timer;
var iTabPosition = 0;
var iTabWidth = 0;
var iTabOffSet = 0;
var iScrollStep = 3;
var bScrollInProgress = false;
var bEndScroll = false;

/**************************************************************************************************
Pad a string to pad_length fillig it with pad_char.
By default the function performs a left pad, unless pad_right is set to true.

If the value of pad_length is negative, less than, or equal to the length of the input string, no padding takes place.
**************************************************************************************************/
if (!String.prototype.pad)
    String.prototype.pad = function (pad_char, pad_length, pad_right) {
        var result = this;
        if ((typeof pad_char === 'string') && (pad_char.length === 1) && (pad_length > this.length)) {
            var padding = new Array(pad_length - this.length + 1).join(pad_char); //thanks to http://stackoverflow.com/questions/202605/repeat-string-javascript/2433358#2433358
            result = (pad_right ? result + padding : padding + result);
        }
        return result;
    }
function HideTab(sTabID) {

   // debugger;
    //<input name="ctl00$cntMainBody$ctrlTabIndex$inpHiddenTabs" type="hidden" id="ctl00_cntMainBody_ctrlTabIndex_inpHiddenTabs" />
    //<input name="ctl00$cntMainBody$ctrlTabIndex$inpVisibleTabs" type="hidden" id="ctl00_cntMainBody_ctrlTabIndex_inpVisibleTabs" />
    try {
        if (sTabIndexID == undefined) {
            sTabIndexID = "ctl00_cntMainBody_ctrlTabIndex";
        }
        if (ctrlInputHiddenTabs == undefined) {
            ctrlInputHiddenTabs = "ctl00_cntMainBody_ctrlTabIndex_inpHiddenTabs";
        }
        if (ctrlInputVisibleTabs == undefined) {
            ctrlInputVisibleTabs = "ctl00_cntMainBody_ctrlTabIndex_inpVisibleTabs";
        }


        var sNewTabID = sTabID.pad("0", 2);
        var iExistInHiddenTabs = document.getElementById(ctrlInputHiddenTabs).value.indexOf(sNewTabID + ',');
        //Add the hidden field if already not exists 
        if (iExistInHiddenTabs == -1) {
            document.getElementById(ctrlInputHiddenTabs).value += sNewTabID + ',';
        }
        var iExistInVisibleTabs = document.getElementById(ctrlInputVisibleTabs).value.indexOf(sNewTabID + ',');

        if (iExistInVisibleTabs > -1) {
            //Remove hidden tab from visible tabs
            var subStr = document.getElementById(ctrlInputVisibleTabs).value.replace(sNewTabID + ',', "");
            document.getElementById(ctrlInputVisibleTabs).value = subStr;
        }

        //Hide tab
        document.getElementById(sTabIndexID + '_' + sTabID).style.display = 'none';
    }
     catch (e) {
        sTabIndexID = "ctl00_cntMainBody_TabIndex";
        ctrlInputHiddenTabs = "ctl00_cntMainBody_TabIndex_inpHiddenTabs";
        ctrlInputVisibleTabs = "ctl00_cntMainBody_TabIndex_inpVisibleTabs";
        var iExistInHiddenTabs = document.getElementById(ctrlInputHiddenTabs).value.indexOf(sTabID + ',');
         //Add the hidden field if already not exists 
        if (iExistInHiddenTabs == -1) {
            document.getElementById(ctrlInputHiddenTabs).value += sTabID + ',';
        }
        var iExistInVisibleTabs = document.getElementById(ctrlInputVisibleTabs).value.indexOf(sTabID + ',');

        if (iExistInVisibleTabs > -1) {
            //Remove hidden tab from visible tabs
            var subStr = document.getElementById(ctrlInputVisibleTabs).value.replace(sTabID + ',', "");
            document.getElementById(ctrlInputVisibleTabs).value = subStr;
        }

         //Hide tab
        document.getElementById(sTabIndexID + '_' + sTabID).style.display = 'none';
    }
}

function ShowTab(sTabID) {
   // debugger;
    if (sTabIndexID == undefined) {
        sTabIndexID = "ctl00_cntMainBody_ctrlTabIndex";
    }
    if (ctrlInputHiddenTabs == undefined) {
        ctrlInputHiddenTabs = "ctl00_cntMainBody_ctrlTabIndex_inpHiddenTabs";
    }
    if (ctrlInputVisibleTabs == undefined) {
        ctrlInputVisibleTabs = "ctl00_cntMainBody_ctrlTabIndex_inpVisibleTabs";
    }
    var sNewTabID = sTabID.pad("0", 2);
    var iExistInVisibleTabs = document.getElementById(ctrlInputVisibleTabs).value.indexOf(sNewTabID + ',');
    //Add the visible tabs if already not exists 
    if (iExistInVisibleTabs == -1) {
        document.getElementById(ctrlInputVisibleTabs).value += sNewTabID + ',';
    }

    var iExistInHiddenTabs = document.getElementById(ctrlInputHiddenTabs).value.indexOf(sNewTabID + ',');

    if (iExistInHiddenTabs > -1) {
        //Remove from hidden tabs
        var subStr = document.getElementById(ctrlInputHiddenTabs).value.replace(sNewTabID + ',', "");
        document.getElementById(ctrlInputHiddenTabs).value = subStr;
    }

   document.getElementById(sTabIndexID + '_' + sTabID).style.display = '';
}

function CancelScroll() {
    bEndScroll = true;
}

function Scroll(bScrollLeft) {
    iTabOffSet -= iScrollStep;

    if (iTabOffSet <= 0) {
        if (bEndScroll == true) {
            if (bScrollLeft == true) {
                document.getElementById(sTabIndexID + '_tabholder').scrollLeft -= (iScrollStep + iTabOffSet);
            }
            else {
                document.getElementById(sTabIndexID + '_tabholder').scrollLeft += (iScrollStep + iTabOffSet);
            }

            bScrollInProgress = false;
            bEndScroll = true;
            clearInterval(Timer);
            iTabWidth = 0;
            iTabOffSet = 0;
            iScrollStep = 3;
        }
        else {
            iTabOffSet += iScrollStep;

            if (bScrollLeft == true) {
                ScrollLeft();
            }
            else {
                ScrollRight();
            }
            iScrollStep++;
        }
    }
    else {
        if (bScrollLeft == true) {
            document.getElementById(sTabIndexID + '_tabholder').scrollLeft -= iScrollStep;
        }
        else {
            document.getElementById(sTabIndexID + '_tabholder').scrollLeft += iScrollStep;
        }
    }
}

function ScrollLeft() {
    iTabPosition--;

    if (iTabPosition < 0) {
        iTabPosition = 0;
        bScrollInProgress = false;
        bEndScroll = true;
    }
    else {
        while (document.getElementById(sTabIndexID + '_tabs').childNodes[iTabPosition].offsetWidth == null) {
            iTabPosition--;
        }
        iTabWidth = document.getElementById(sTabIndexID + '_tabs').childNodes[iTabPosition].offsetWidth;
        iTabOffSet += iTabWidth;

        if (bScrollInProgress == false) {
            Timer = setInterval("Scroll(true);", 5);
            bScrollInProgress = true;
            bEndScroll = false;
        }
    }
}

function ScrollRight() {
    if (iTabPosition == document.getElementById(sTabIndexID + '_tabs').childNodes.length - 1) {
        bScrollInProgress = false;
        bEndScroll = true;
    }
    else {
        while (document.getElementById(sTabIndexID + '_tabs').childNodes[iTabPosition].offsetWidth == null) {
            iTabPosition++;
        }

        iTabWidth = document.getElementById(sTabIndexID + '_tabs').childNodes[iTabPosition].offsetWidth
        iTabOffSet += iTabWidth;
        iTabPosition++;

        if (bScrollInProgress == false) {
            Timer = setInterval("Scroll(false);", 5);
            bScrollInProgress = true;
            bEndScroll = false;
        }
    }
}