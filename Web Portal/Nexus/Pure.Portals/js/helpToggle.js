// JScript File

    function helptoggle(obj)
	{
	if(obj.parentNode.parentNode.className=='panel')
		{
		obj.parentNode.parentNode.className='panelhelp';
		}
	else
		{
		obj.parentNode.parentNode.className='panel';
		}
	}