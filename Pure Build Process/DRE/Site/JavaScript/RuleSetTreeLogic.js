var lastTreeTargetControlWithFocus;
var arrayOfHiddenControls;

var const_expandlabel = '+';
var const_contractlabel = '-';
var const_outputprefix = 'Output.';

var const_ignored_content = new Array();
const_ignored_content[0] = "math.";
const_ignored_content[1] = "convert.";


// Externally called function to populate target controls
function SetTreeValueToFieldWithFocus(val, isGenericType, isOutputParameter)
{
    if(lastTreeTargetControlWithFocus){
        var hiddenEle = document.getElementById(lastTreeTargetControlWithFocus);
        
        if(hiddenEle.mustBeGenericType && !isGenericType){
            alert('This field only accepts generic types');
        }else{
            hiddenEle.value = (isOutputParameter?const_outputprefix:'')+val;
            UpdateVisibleFieldContent(hiddenEle.id);
        }
    }
}

// Externally called function to setup functionality
// arr: array of strings (the DOM ids of the hidden elements to make targets)
function CreateTargetControls(arr)
{
    arrayOfHiddenControls = arr;
    
    var labels = document.getElementsByTagName('LABEL');
    for (var i=0; i<labels.length; i++) {
        if (labels[i].htmlFor != '') {
            var ele = document.getElementById(labels[i].htmlFor);
            if(ele){           
                ele.labelElement = labels[i];
            }
        }
    }    
    
    for(var i=0; i<arrayOfHiddenControls.length; i++){        
        var hiddenEle = document.getElementById(arrayOfHiddenControls[i]);
        CreateTargetControl(arrayOfHiddenControls[i]);
    }
    
    SetCssClasses(null);
}

function CreateTargetControl(hiddenEleId)
{
    var hiddenEle = document.getElementById(hiddenEleId);
    var parentElement = hiddenEle.parentNode;
    var originalControlWasHidden = (hiddenEle.style.display == 'none');

    var newInput = document.createElement('input');
    parentElement.appendChild(newInput);   
    newInput.id = GetVisibleControlId(hiddenEle.id);
    newInput.hiddenControlElementId = hiddenEle.id;

    newInput.onfocus = function()
        {
            lastTreeTargetControlWithFocus = this.hiddenControlElementId;
            SetCssClasses(this.hiddenControlElementId);
        };

    newInput.onblur = function()
        {
            UpdateHiddenFieldContent(this.hiddenControlElementId);
        };
        
    var switchButton = document.createElement('input');
    switchButton.type = 'button';
    parentElement.appendChild(switchButton); 
    switchButton.id = GetSwitchButtonControlId(hiddenEle.id);
    switchButton.hiddenControlElementId = hiddenEle.id;
    switchButton.value = const_expandlabel; 
    switchButton.className = 'expandedtoggle';
    
    switchButton.onclick = function()
        {
            if(this.value==const_expandlabel){
                this.value = const_contractlabel;
            }else{
                this.value = const_expandlabel;              
            }
            lastTreeTargetControlWithFocus = this.hiddenControlElementId;
            UpdateVisibleFieldContent(this.hiddenControlElementId); 
            SetCssClasses(this.hiddenControlElementId);
        };            

    UpdateVisibleFieldContent(hiddenEle.id); 
    
    if(hiddenEle.labelElement){
        hiddenEle.labelElement.htmlFor = newInput.id;
    }
    
    hiddenEle = ChangeInputType(hiddenEle,'hidden');
    
    hiddenEle.SetDisabled = function(val)
        {
            SetExpandableControlDisabled(this.id, val);
        };         
    
    hiddenEle.SetDisabled(hiddenEle.disabled);
    
    if(originalControlWasHidden) {
        parentElement.style.display = 'none';
    }
}

function SetExpandableControlDisabled(id, val)
{
    var textInput = document.getElementById(GetVisibleControlId(id));
    var button = document.getElementById(GetSwitchButtonControlId(id));
    textInput.disabled = val;
    button.disabled = val;
}

function GetVisibleControlId(hiddenInputId)
{
    var hiddenEle = document.getElementById(hiddenInputId);
    return hiddenEle.id + "_visible";    
}

function GetSwitchButtonControlId(hiddenInputId)
{
    var hiddenEle = document.getElementById(hiddenInputId);
    return hiddenEle.id + "_switchButton";    
}

function UpdateVisibleFieldContent(hiddenInputId)
{
    var hiddenEle = document.getElementById(hiddenInputId);
    var textEle = document.getElementById(GetVisibleControlId(hiddenInputId));
    
    if(FieldIsExpanded(hiddenEle.id)||IsDecimal(hiddenEle.value)){
        //textEle.value = StripOutputPrefix(hiddenEle.value); //Uncomment this line (comment the line below) to get value without Output prefix 
        textEle.value = hiddenEle.value;                      //Uncomment this line (comment the line above) to get value with Output prefix 
    } else {

        if (IsIgnoredContent(hiddenEle.value)) {
            //textEle.value = StripOutputPrefix(hiddenEle.value); //Uncomment this line (comment the line below) to get value without Output prefix 
            textEle.value = hiddenEle.value;                      //Uncomment this line (comment the line above) to get value with Output prefix 
        } else {
            var splitProperty = hiddenEle.value.split('.');
            if (splitProperty != null && splitProperty.length > 0) {
                textEle.value = splitProperty[splitProperty.length - 1];
            }
        }
    }        
}

function IsIgnoredContent(val) 
{
    var result = false;
    for (var i = 0; i < const_ignored_content.length; i++) {
        if (val.toLowerCase().indexOf(const_ignored_content[i]) == 0) {
            result = true;
            break;
        }
    }
    return result;
}

function StripOutputPrefix(val)
{
    if(val.substring(0,const_outputprefix.length)==const_outputprefix){
        val = val.substring(const_outputprefix.length);
    }
    return val;
}

function UpdateHiddenFieldContent(hiddenInputId)
{    
    var hiddenEle = document.getElementById(hiddenInputId);
    var textEle = document.getElementById(GetVisibleControlId(hiddenInputId));
    /*
    if(FieldIsExpanded(hiddenEle.id)||IsDecimal(textEle.value)){
        hiddenEle.value = textEle.value;        
    }else{  
        var splitProperty = hiddenEle.value.split('.');
        if(splitProperty!=null && splitProperty.length>0){
            splitProperty[splitProperty.length-1] = textEle.value;
            hiddenEle.value = splitProperty.join('.');
        }
    } 
    */
    hiddenEle.value = textEle.value;
}

function FieldIsExpanded(hiddenInputId)
{
    var hiddenEle = document.getElementById(hiddenInputId);
    var switchButton = document.getElementById(GetSwitchButtonControlId(hiddenEle.id));    
    return switchButton.value == const_contractlabel;
}

function SetCssClasses(selectedHiddenInputId)
{
    for(var i=0; i<arrayOfHiddenControls.length; i++){
        var ele = document.getElementById(GetVisibleControlId(arrayOfHiddenControls[i]));
        if(FieldIsExpanded(arrayOfHiddenControls[i])){
            ele.className = 'expanded';
        }else{
            ele.className = 'contracted';
        }
    } 
    
    if(selectedHiddenInputId){
        var hiddenEle = document.getElementById(selectedHiddenInputId);
        var textEle = document.getElementById(GetVisibleControlId(hiddenEle.id));
        textEle.className = textEle.className+' selected';
    }
}
