function WildCardValidator(val) 
{
var controltovalidateIDs = val.controlstovalidate.split(',');

    switch (val.condition) {
        case 'NoWildCard':
            for(var controltovalidateIDIndex in controltovalidateIDs) {
                var controlID = controltovalidateIDs[controltovalidateIDIndex];
                var text=document.getElementById(controlID).value;
                if (text.length >0)
                { 
                  if (text.search('%') != -1)
                  {
                     return false;
                  }
                }
            }
            return true;
        break;
        case 'AllowWildCardAtEnd':
            for(var controltovalidateIDIndex in controltovalidateIDs) {
                var controlID = controltovalidateIDs[controltovalidateIDIndex];
                var text=document.getElementById(controlID).value;
              if (text.length >0)
              {
                if (text.charAt(text.length-1) != '%' )
                {
                 return false;
                }
              }
            }
            return true;
        break;
        case 'AllowWildCard':
            return true;
        break;
     
    }
}
