
function HideShowCopyDivs(ddlValue)
{
    var divCopyLines = document.getElementById('divCopyLines');
    var divCopyGroup = document.getElementById('divCopyGroup');
    if(ddlValue == 'Lines')
    {
        divCopyLines.className = '';
        divCopyGroup.className = 'hidden';
    }
    else
    {
        divCopyLines.className = 'hidden';
        divCopyGroup.className = '';        
    }    
}

function ValidateCopyInformation(ddlCopyClientId, txtCopySourceStartId, txtCopySourceEndId, txtCopyTargetLineNumberId, ddlSourceGroupId, lineNumbers)
{
    var ddlCopy = document.getElementById(ddlCopyClientId);
    var txtCopySourceStart = document.getElementById(txtCopySourceStartId);
    var txtCopySourceEnd = document.getElementById(txtCopySourceEndId);
    var txtCopyTargetLineNumber = document.getElementById(txtCopyTargetLineNumberId);
    var ddlSourceGroup = document.getElementById(ddlSourceGroupId);
     
    var ddlCopyValue = '';
    var txtCopySourceStartValue = '';
    var txtCopySourceEndValue = '';
    var txtCopyTargetLineNumberValue = '';
    var ddlSourceGroupValue = '';
    var existingLineNumbers = lineNumbers.split(',');
    
    if(ddlCopy) ddlCopyValue = ddlCopy.value;
    if(txtCopySourceStart) txtCopySourceStartValue = parseInt(txtCopySourceStart.value);
    if(txtCopySourceEnd) txtCopySourceEndValue = parseInt(txtCopySourceEnd.value);
    if(txtCopyTargetLineNumber) txtCopyTargetLineNumberValue = parseInt(txtCopyTargetLineNumber.value);
    if(ddlSourceGroup) ddlSourceGroupValue = ddlSourceGroup.value;
        

    if(ddlCopyValue == 'Lines')
    {
        if(isNaN(txtCopySourceStartValue))
        {
            alert('Enter start line number for rules to be copied');
            txtCopySourceStart.focus();
            return false;
        }
        else
        {
            if(!LineNumberExists(txtCopySourceStartValue, existingLineNumbers))
            {
                alert('Source start line number ' + txtCopySourceStartValue + ' does not exist');
                txtCopySourceStart.focus();
                return false;            
            }
        }
            
        if(isNaN(txtCopySourceEndValue))
        {
            alert('Enter last line number for rules to be copied');
            txtCopySourceEnd.focus();
            return false;
        }    
        else
        {
            if(!LineNumberExists(txtCopySourceEndValue, existingLineNumbers))
            {
                alert('Source end line number ' + txtCopySourceEndValue + ' does not exist');
                txtCopySourceEnd.focus();
                return false;            
            }
        }
                        
        if(txtCopySourceStartValue > txtCopySourceEndValue)
        {
            alert('Enter a valid range to copy');
            txtCopySourceStart.focus();
            return false;
        }
        if(txtCopyTargetLineNumberValue  > txtCopySourceStartValue  && txtCopyTargetLineNumberValue < txtCopySourceEndValue)
        {
            alert('Source and target lines cannot overlap');
            txtCopySourceStart.focus();
            return false;
        }                                
    }
    else
    {
    }
    if(isNaN(txtCopyTargetLineNumberValue))
    {
        alert('Enter line number where rules are to be copied');
        txtCopyTargetLineNumber.focus();
        return false;
    }
    else
    {
        if(!LineNumberExists(txtCopyTargetLineNumberValue, existingLineNumbers))
        {
            alert('Target line number ' + txtCopyTargetLineNumberValue + ' does not exist');
            txtCopyTargetLineNumber.focus();
            return false;            
        }
    }
    
    return true;
}

function LineNumberExists(lineNumber, lineNumbers)
{
    for (var lineIndex in lineNumbers)
    {
        if(parseInt(lineNumbers[lineIndex]) == lineNumber)
        {
            return true;
        }    
    } 
    return false;
}