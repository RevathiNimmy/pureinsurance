function NexusPickList_MoveOption(sourceListBoxId, targetListBoxId, isTargetListSelected, selectedBoxId, moveAll) {
    var sourceListBox = document.getElementById(sourceListBoxId);
    var targetListBox = document.getElementById(targetListBoxId);
    
    // If the lists passed in haven't already been backed up, we'll do that now
    if (!sourceListBox.bak) {
        backupList(sourceListBox);
    }
    if (!targetListBox.bak) {
        backupList(targetListBox);
    }
   
    var targetOptions = targetListBox.options;
    var nextTargetIndex = targetOptions.length;
    var nextBackupTargetIndex = targetListBox.bak.length;
    var valuesToRemove = new Array();
    var ctr = 0;

    for (var i = 0; i < sourceListBox.options.length; i++) {
        if (sourceListBox.options[i].selected || moveAll) {
            // add option to targetListBox list
            targetOptions[nextTargetIndex] = new Option(sourceListBox.options[i].text);
            targetOptions[nextTargetIndex].value = sourceListBox.options[i].value;
            targetOptions[nextTargetIndex].title = sourceListBox.options[i].title;
            nextTargetIndex += 1;
            //also add option to the target lists backup array
            targetListBox.bak[nextBackupTargetIndex] = new Array(sourceListBox.options[i].value, sourceListBox.options[i].text, sourceListBox.options[i].title);
            nextBackupTargetIndex += 1;
            //finally create an array of values that should be removed from the source list
            valuesToRemove[ctr] = sourceListBox.options[i].value;
            ctr++;
        }
    }
   
    // remove option(s) added to the targetListBox list from the sourceListBox list, and also from the sourcelist backup array
    for (var j = 0; j < valuesToRemove.length; j++) {
        for (var i = 0; i < sourceListBox.options.length; i++) {
            if (valuesToRemove[j] == sourceListBox.options[i].value) {
                sourceListBox.options[i] = null;
                break;
            }
        }
        for (var i = 0; i < sourceListBox.bak.length; i++) {
            if (valuesToRemove[j].toLowerCase() == sourceListBox.bak[i][0].toLowerCase()) {
                //remove from array by making new array composing of everything before and everything after the entry we want to get rid of
                sourceListBox.bak = sourceListBox.bak.slice(0, i).concat(sourceListBox.bak.slice(i + 1));
                break;
            }
        }
    }
    sortSelect(sourceListBox);
    sortSelect(targetListBox);
    buildSelectedList((isTargetListSelected) ? targetListBox : sourceListBox, selectedBoxId);
    
}

function sortSelect(selectToSort) {
    var arrOptions = [];

    for (var i = 0; i < selectToSort.length; i++) {
            arrOptions[i] = [];
        arrOptions[i][0] = selectToSort[i].text;
        arrOptions[i][1] = selectToSort[i].value;
        arrOptions[i][2] = selectToSort[i].selected;
        arrOptions[i][3] = selectToSort[i].title;
        }
    
    //arrOptions.sort();
    arrOptions.sort(function (x, y) {
        var a = String(x).toUpperCase();
        var b = String(y).toUpperCase();
        if (a > b)
            return 1
        if (a < b)
            return -1
        return 0;
    }); 

   for (var i = 0; i < selectToSort.length; i++) {
        selectToSort[i].value = arrOptions[i][1];
        selectToSort[i].text = arrOptions[i][0];
       selectToSort[i].selected = arrOptions[i][2];
       selectToSort[i].title = arrOptions[i][3];
        } 
    return
    }
  

function buildSelectedList(listBox, selectedBoxId) {
    var valuesList = "";

    for (var i = 0; i < listBox.options.length; i++) {
        valuesList += listBox.options[i].value + ",";
    }

    if (valuesList.lastIndexOf(",") > -1) {
        valuesList = valuesList.substring(0, valuesList.lastIndexOf(","));
    }

    var selectedValuesBox = document.getElementById(selectedBoxId);
    selectedValuesBox.value = valuesList;
}

function backupList(list) {
    // Attach an array to the select object where we keep a backup of the original dropdown list
    var n;
    list.bak = new Array();
    for (n = 0; n < list.length; n++) {
        list.bak[list.bak.length] = new Array(list[n].value, list[n].text, list[n].title);
    }
}

function filterList(pattern, targetListBoxId) {
    var list = targetListBoxId;
    // If the list passed in hasn't already been backed up, we'll do that now

    if (!list.bak) {
        backupList(list);
    }


    // Iterate through the backed up dropdown list. If an item matches, it is added to the list of matches.
    var match = new Array();
    for (n = 0; n < list.bak.length; n++) {
        if (list.bak[n][1].toLowerCase().indexOf(pattern.toLowerCase()) != -1) {
            match[match.length] = new Array(list.bak[n][0], list.bak[n][1], list.bak[n][2]);
        }
    }

    // Remove all the options from the list
    for (n = list.length; n >= 0; n--) {
        list.remove(n);
    }

    // Finally rewrite the dropdown list adding in just the matches
    for (n = 0; n < match.length; n++) {
        var opt = document.createElement("option");
        opt.value = match[n][0];
        opt.text = match[n][1];
        list.options.add(opt)
    }
}

function addValueToList(targetListBoxId) {
    var list = document.getElementById(targetListBoxId);
    for (n = list.length - 1; n >= 0; n--) {
        list[n].text = list[n].value + ' - ' + list[n].text;

    }
}

function backupList1(listID) {
    // Attach an array to the select object where we keep a backup of the original dropdown list
    var list = document.getElementById(listID);
    var n;
    list.bak1 = new Array();
    for (n = 0; n < list.length; n++) {
        list.bak1[list.bak1.length] = new Array(list[n].value, list[n].text, list[n].title);
    }
}

function NexusPickList_RefreshSelected(targetListBoxId, sourceListBoxId) {
    var list1 = document.getElementById(targetListBoxId);
    var list2 = document.getElementById(sourceListBoxId);
    if (!list1.bak1) {
        backupList1(targetListBoxId);       
    }
    else {
        loadFromBackup(list1, list1.bak1);            
       }

       if (!list2.bak1) {
           backupList1(sourceListBoxId)
       }
       else {
           loadFromBackup(list2, list2.bak1);
       }
       list2.bak = null;
}       
       
function loadFromBackup(list, listbak) {
    for (var i = list.length; i > 0 ; i--) {
        list.options[i - 1] = null;
       
   }
   for (var i = 0; i < listbak.length; i++) {
       list[i] = new Option(listbak[i][1]);
       list[i].value = listbak[i][0];
       list[i].title = listbak[i][2];
   } 
}