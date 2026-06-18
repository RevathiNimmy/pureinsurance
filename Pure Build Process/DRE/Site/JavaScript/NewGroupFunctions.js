var clientIdForEnterGroupPanel;
var clientIdForDataEntryPanel;

function showEnterGroupPanel(pClientIdForEnterGroupPanel, pClientIdForDataEntryPanel)
{
    if(!clientIdForEnterGroupPanel){
        clientIdForEnterGroupPanel = pClientIdForEnterGroupPanel;
    }
    if(!clientIdForDataEntryPanel){
        clientIdForDataEntryPanel = pClientIdForDataEntryPanel;
    }
    var eleCreateGroup = document.getElementById(clientIdForEnterGroupPanel);
    eleCreateGroup.className = 'pnlEnterNewGroup';
    var eleDataEntry = document.getElementById(clientIdForDataEntryPanel);
    eleDataEntry.previousClassName = eleDataEntry.className;
    eleDataEntry.className = 'hidden';
}

function hideEnterGroupPanel()
{
    var eleCreateGroup = document.getElementById(clientIdForEnterGroupPanel);
    eleCreateGroup.className = 'pnlEnterNewGroup hidden';
    var eleDataEntry = document.getElementById(clientIdForDataEntryPanel);
    eleDataEntry.className = eleDataEntry.previousClassName;
}