function toggleChecked(oElement) 
{ 
  oForm = oElement.form; 
  oElement = oForm.elements[oElement.name]; 
  if(oElement.length) 
  { 
    bChecked = oElement[0].checked; 
    for(i = 1; i < oElement.length; i++)
    {
      oElement[i].checked = bChecked;
    }
  }   
} 

function toggleIndeterminate(oElement) 
{ 
  oForm = oElement.form; 
  oElement = oForm.elements[oElement.name]; 
  if(oElement.length) 
  { 
    bIndeterminate = false; 
    bChecked = true; 
    nChecked = 0;
    for(i = 1; i < oElement.length; i++)
    {
      if(oElement[i].checked)
      {
        nChecked++;
      }
    }
    if(nChecked < oElement.length - 1)
    { 
      if(nChecked)
      {
        bIndeterminate = true; 
      }
      else 
      { 
        bIndeterminate = false; 
        bChecked = false; 
      } 
    } 
    else 
    { 
      bIndeterminate = false;
    } 
    oElement[0].indeterminate = bIndeterminate;
    oElement[0].checked = bChecked;
  } 
}

function toggleController(oElement)
{
    oForm=oElement.form;
    oElement=oForm.elements[oElement.name];
    if(oElement.length)
    {
        bChecked=true;
        nChecked=0;
        for(i=0;i<oElement.length-1;i++)
        {
            if(oElement[i].checked)
            {
                nChecked++;
            }
            if(nChecked<oElement.length)
            {
                bChecked=false;
                oElement[0].checked=bChecked;
            }
        }
     }
}