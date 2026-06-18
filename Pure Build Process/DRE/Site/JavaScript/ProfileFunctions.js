function setSeqRadioButtons(seqChecked)
{
    var radioList = document.getElementById('rbProfileList');
    var radio = radioList.getElementsByTagName("input");
    
    if (seqChecked == true)
    {
        //make sure the shared input/output schema is selected and disable the distinct input/output schema option 
        radio[0].checked = true;
        radio[1].disabled = true;
    }
    else
    {
        //re-enable the distinct input/output schema option.
        radio[1].disabled = false;
    }
    setOutput(radioList);
}

function setOutput(radioList)
{
    var radio = radioList.getElementsByTagName("input");
    var xsdOutput = document.getElementById('xsdOutput');
    var count = 0;
    var innerCount = 0;
    var disabled = false;
    var display = '';
    
    if(radio[0].checked == true)
    {
        disabled = true;
        display = 'none'
    }
    
    xsdOutput.disabled = disabled;
    for(count = 0; count < xsdOutput.children.length; count++)
    {
        //now disable/enable all the children in the xsdOutput div node
        var xsdChild = xsdOutput.children[count];
        xsdChild.disabled = disabled;
        
        for(innerCount = 0; innerCount < xsdChild.children.length; innerCount++)
        { 
            xsdChild.children[innerCount].disabled = disabled;
            if(xsdChild.children[innerCount].href != undefined)
            {
                xsdChild.children[innerCount].style.display = display;
            }
        }
    }
}

function returnFalse()
{
    return false;
}