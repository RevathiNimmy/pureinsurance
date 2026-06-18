function FindControl(result, context) {
    document.getElementById(context + "_Results").innerHTML = result;
    document.getElementById(context + "_Results").className = "overdiv";
}

function FindSearch(ctrlResults, oControls) {
  
    document.getElementById(ctrlResults).innerHTML = 'Searching ...';
    var sValues = '<Search>';

    for (i=0;i<oControls.length;i++)
    {
        sValues += '<Item>'
        
        ctrlObject = document.getElementById(oControls[i]);
        if (ctrlObject != null )
        {
            switch (ctrlObject.tagName)
            {
                case 'INPUT' :
                {
                    sValues += ctrlObject.value;
                    break;
                }
                case 'SELECT' :
                {
                    if (ctrlObject.selectedIndex >=0 )
                    {
                        sValues += ctrlObject.options[ctrlObject.selectedIndex].text;
                    }
                    break;
                }
            }
        }
        sValues += '</Item>';
    }
    
    sValues += '</Search>';
    
    return sValues
}

function SelectionPopulate(result, context) {
    
    var oSelection = result.split(';');
    var ctrlObject = null;
    
    for (i=0;i<oSelection.length-1;i++)
    {
        ctrlObject = document.getElementById(context[i]);
        if (ctrlObject != null )
        {
            switch (ctrlObject.tagName)
            {
                case 'INPUT' :
                {
                    ctrlObject.value = oSelection[i];
                    break;
                }
                case 'SELECT' :
                {
                    for (x=0; x <= ctrlObject.childNodes.length -1; x++)
                    {
                        if (ctrlObject.childNodes[x].tagName == "OPTION")
                        {
                            if (ctrlObject.childNodes[x].text == oSelection[i])
                            {
                                ctrlObject.selectedIndex = ctrlObject.childNodes[x].index
                                ctrlObject.childNodes[x].selected = true;
                            }
                            else
                            {
                                ctrlObject.childNodes[x].selected = false;
                            }
                        }
                    }
                    break;
                }
            }
         }   
    }
}

function Select(ctrlSource) {
   
    var sValues = '<Selection>' + ctrlSource.value + '</Selection>';
    return sValues
    
}

function ClearSearch(ctrlResults, oControls)
{
    document.getElementById(ctrlResults).innerHTML = '';

    for (i=0;i<oControls.length;i++)
    {
        //document.getElementById(oControls[i]).value = '';
      ctrlObject = document.getElementById(oControls[i]);
        ctrlNext = ctrlObject.nextSibling;
    if (ctrlObject != null || ctrlNext != null)
      {
        switch (ctrlObject.tagName)
        {
            case 'INPUT' :
            {
                ctrlObject.value='';
                break;
            }
            case 'SELECT' :
            {
               if (ctrlObject.selectedIndex >= 0 )
                {
                  ctrlObject.options[ctrlObject.selectedIndex].value= -1;
                }
                break;
            }
        }
        if (ctrlNext.id == "") {
            switch (ctrlNext.tagName) {
                case 'INPUT':
                    {
                        ctrlNext.value = '';
                        break;
                    }
                case 'SELECT':
                    {
                        if (ctrlNext.selectedIndex >= 0) {
                            ctrlNext.options[ctrlNext.selectedIndex].value = -1;
                        }
                        break;
                    }
            }
        }
      }
    }
    return false;
}

