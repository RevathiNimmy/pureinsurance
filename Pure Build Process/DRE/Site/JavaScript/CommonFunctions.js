var __globalDisallowedCharacters = new Array();
__globalDisallowedCharacters[__globalDisallowedCharacters.length] = '<';
__globalDisallowedCharacters[__globalDisallowedCharacters.length] = '>';
__globalDisallowedCharacters[__globalDisallowedCharacters.length] = ':';
__globalDisallowedCharacters[__globalDisallowedCharacters.length] = '(';
__globalDisallowedCharacters[__globalDisallowedCharacters.length] = ')';
__globalDisallowedCharacters[__globalDisallowedCharacters.length] = '*';
__globalDisallowedCharacters[__globalDisallowedCharacters.length] = '#';
__globalDisallowedCharacters[__globalDisallowedCharacters.length] = '~';
__globalDisallowedCharacters[__globalDisallowedCharacters.length] = '&';
__globalDisallowedCharacters[__globalDisallowedCharacters.length] = '^';

function AllowNumbersOnly(e) {
    var unicode=e.charCode? e.charCode : e.keyCode;
    if (unicode!=8){ // if the key isn't the backspace key (which we should allow) 
        if (unicode<48||unicode>57){ // if not a number 
            return false; // disable key press 
        }
    }
} 

function DisallowCharacters(e, characters)
{
    var unicode = e.charCode? e.charCode : e.keyCode;
    for(var i=0; i<characters.length; i++){
        if(characters[i].charCodeAt(0) == unicode){
            return false;
        }
    }
}

function IsDecimal(val)
{	
	return /^\d*\.\d*$/.test(val);
}

// IE cannot simply change the type param
function ChangeInputType(oldObject, oType) 
{
    var newObject = document.createElement('input');
    newObject.type = oType;
    if(oldObject.size){newObject.size = oldObject.size;}
    if(oldObject.value){newObject.value = oldObject.value;}
    if(oldObject.name){newObject.name = oldObject.name;}
    if(oldObject.id){newObject.id = oldObject.id;}
    if(oldObject.className){newObject.className = oldObject.className;}
    if(oldObject.disabled){newObject.disabled = oldObject.disabled;}
    oldObject.parentNode.replaceChild(newObject,oldObject);
    return newObject;
}

function showPopup(id, className)
{
    document.getElementById('washoutbox').className = 'washOut';
    document.getElementById(id).className = className;
}

function hidePopup(id)
{
    document.getElementById('washoutbox').className = 'hidden';
    document.getElementById(id).className = 'hidden';
}

function setInputDisabledFlag(clientId, disabledFlag){
    var ele = document.getElementById(clientId);
    if(ele.SetDisabled){
        ele.SetDisabled(disabledFlag);
    }else{
        ele.disabled = disabledFlag;
    }
}