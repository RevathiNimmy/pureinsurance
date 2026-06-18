// JScript File

var cntMainBody = "ctl00_cntMainBody_";

function OnlyNumeric()
{
    var key = event.keyCode;
    //alert(event.keyCode); 
    if ((key < 48 || key > 57) && (key < 96 || key > 105) && (key != 8) && (key != 46)  && (key != 9))
            return false; 

}

function blankOnFocus(ctl,def)
{
    if (ctl.value == def)
        ctl.value = "";
}

function setBackOnBlur(ctl,def)
{
    if (ctl.value == "")
        ctl.value = def; 
}

   
   function displayHide(cntl,div)
   {
        if (document.getElementById(cntMainBody+cntl).checked)    
            document.getElementById(div).style.display = "block";
        else
            document.getElementById(div).style.display = "none";           
   }
   
   
   function chkBlank(cntl,vld)
   {
        var myVal = document.getElementById(cntMainBody+vld);
        ValidatorEnable(myVal, true);
        if (document.getElementById(cntMainBody+cntl).checked)    
        {
            ValidatorEnable(myVal, false);
        }
   }
   
function chkBlank1(cntl,vld)
   {
        var myVal = document.getElementById(cntMainBody+vld);
        ValidatorEnable(myVal, true);
        if (!document.getElementById(cntMainBody+cntl).checked)    
        {
            ValidatorEnable(myVal, false);
        }
            else
    {
        ValidatorEnable(myVal, false);
    }

   }
   
   function ValidatorEnable2(cntl, vld)
{
    var myVal = document.getElementById(cntMainBody + vld);
    if (document.getElementById(cntMainBody + cntl).checked)
    {
        ValidatorEnable(myVal, true);
    }
    else
    {
        ValidatorEnable(myVal, false);
    }
} 


function setFocus(cntrlName)
    {
    var control = document.getElementById(cntMainBody+cntrlName);
    if( control != null ){ control.focus(); }
    }
    
    //Set a dropdown list by the list items value
function setSelectedValue(selectCtrlName, val)
{    
    var ctrl = document.getElementById(cntMainBody + selectCtrlName)
    if ( ctrl != null)
    {        
        for (i = 0; i < ctrl.length; i++)
        {            
            if (ctrl.options[i].value == val)
            {
                ctrl.selectedIndex = i;
                return;
            }
        }
    }
}


//Set a dropdown list by the displayed list item text
function setSelectedText(selectCtrlName, txt)
{
    var ctrl = document.getElementById(cntMainBody + selectCtrlName)
    if (ctrl != null)
    {
        for (i = 0; i < ctrl.length; i++)
        {
            if (ctrl.options[i].text == txt)
            {
                ctrl.selectedIndex = i;
                return;
            }
        }
    }
}


function formatNumber(num, decimalNum, bolLeadingZero, bolParens, bolCommas)
/**********************************************************************
IN:
NUM - the number to format
decimalNum - the number of decimal places to format the number to
bolLeadingZero - true / false - display a leading zero for
numbers between -1 and 1
bolParens - true / false - use parenthesis around negative numbers
bolCommas - put commas as number separators.
 
RETVAL:
The formatted number!
**********************************************************************/
{
    if (isNaN(parseInt(num)))
    {
        return "NaN";
    }

    var tmpNum = num;
    var iSign = num < 0 ? -1 : 1; 	// Get sign of number

    // Adjust number so only the specified number of numbers after
    // the decimal point are shown.
    tmpNum *= Math.pow(10, decimalNum);
    tmpNum = Math.round(Math.abs(tmpNum))
    tmpNum /= Math.pow(10, decimalNum);
    tmpNum *= iSign; 				// Readjust for sign


    // Create a string object to do our formatting on
    var tmpNumStr = new String(tmpNum);

    // See if we need to strip out the leading zero or not.
    if (!bolLeadingZero && num < 1 && num > -1 && num != 0)
    {
        alert(1);
        if (num > 0)
        {         
            tmpNumStr = tmpNumStr.substring(1, tmpNumStr.length);
        }
        else
        {            
            tmpNumStr = "-" + tmpNumStr.substring(2, tmpNumStr.length);
        }
    }
    else
    {
        if(num < 10 && num > 0)
        {    
            tmpNumStr = "0" + num;
        }
    }

    // See if we need to put in the commas
    if (bolCommas && (num >= 1000 || num <= -1000))
    {
        var iStart = tmpNumStr.indexOf(".");
        if (iStart < 0)
        {
            iStart = tmpNumStr.length;
        }

        iStart -= 3;
        while (iStart >= 1)
        {
            tmpNumStr = tmpNumStr.substring(0, iStart) + "," + tmpNumStr.substring(iStart, tmpNumStr.length)
            iStart -= 3;
        }
    }

    // See if we need to use parenthesis
    if (bolParens && num < 0)
    {
        tmpNumStr = "(" + tmpNumStr.substring(1, tmpNumStr.length) + ")";
    }

    return tmpNumStr; 	// Return our formatted string!
}



function FormatDateOutput(d, m, y)
{
    return formatNumber(d, 0, true, false, false) + "/" + formatNumber(m, 0, true, false, false) + "/" + y;
}


function GetDateAsText1(d)
{
    return FormatDateOutput(d.getDate(), d.getMonth(), d.getFullYear());
}


function GetDateAsText(d, m, y)
{
    return FormatDateOutput(d, m, y);
}


function checkAll(cntrlName)
{
    var ctrl = document.getElementById(cntMainBody + cntrlName);
    if (ctrl != null)
    {
        for (i = 0; i < ctrl.length; i++)
        {
            ctrl[i].checked = true;
        }
    }
}


function uncheckAll(cntrlName)
{
    var ctrl = document.getElementById(cntMainBody + cntrlName);
    if (ctrl != null)
    {
        for (i = 0; i < ctrl.length; i++)
        {
            ctrl[i].checked = false;
        }
    }
}


/*
Sets a radio button list with standard yes (1)/no (0) values
setCheckValue(cntl24, true)
setCheckValue(cntl24, false)                                 
setCheckValue(cntl24, "") - clears
*/
function setCheckValue(cntrlName, val)
{        
    var list = document.getElementById(cntMainBody + cntrlName);
    if (list != null)
    {
        var inputs = list.getElementsByTagName("input");        
        for (var i = 0; i < inputs.length; i++)
        {     
            inputs[i].checked = (inputs[i].value == val);            
        }
    }
}
