function _SCS_PickList_MoveOption(sourceListBoxId,  targetListBoxId, isTargetListSelected, selectedBoxId, moveAll)
{
    var sourceListBox = document.getElementById(sourceListBoxId);
    var targetListBox = document.getElementById(targetListBoxId);
    
    var targetOptions = targetListBox.options;
	var nextTargetIndex = targetOptions.length;
	var valuesToRemove = new Array();
	var ctr = 0;
	
	for (var i = 0; i < sourceListBox.options.length; i++) 
	{    	
		if (sourceListBox.options[i].selected || moveAll)
		{				
		    // add option to targetListBox list
			targetOptions[nextTargetIndex] = new Option(sourceListBox.options[i].text);
			targetOptions[nextTargetIndex].value = sourceListBox.options[i].value;
		    nextTargetIndex += 1;		
		    
		    valuesToRemove[ctr] = sourceListBox.options[i].value;
		    ctr++;
		}		
	}	
	
	// remove option(s) added to the targetListBox list from the sourceListBox list	
    for (var j = 0; j < valuesToRemove.length; j++) 
    {
	    for (var i = 0; i < sourceListBox.options.length; i++) 
	    {
	        if (valuesToRemove[j] == sourceListBox.options[i].value)
	        {
	            sourceListBox.options[i] = null;
	            break;
	        }
	    }
	}
	
	buildSelectedList((isTargetListSelected) ? targetListBox : sourceListBox, selectedBoxId);	
}
function buildSelectedList(listBox, selectedBoxId)
{
    var valuesList = "";
    
	for (var i = 0; i < listBox.options.length; i++) 
	{		
		valuesList += listBox.options[i].value + ",";
	} 
		
	if (valuesList.lastIndexOf(",") > -1)
	{
		valuesList = valuesList.substring(0, valuesList.lastIndexOf(","));
	}
	
	var selectedValuesBox = document.getElementById(selectedBoxId);
	selectedValuesBox.value = valuesList;
}