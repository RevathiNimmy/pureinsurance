Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.IO
Module MainModule
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMURenewalLaunch"
	
	' Public source and language ID's from the
    ' Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	

	Public Sub Main()
		
		'Dim o As Object
		'Dim lreturn As Long
		'Dim vkey As Variant
		'
		'
		'    Set o = CreateObject("iPMURenewal.NavigatorV3")
		'    lreturn = o.Initialise()
		'
		'    ReDim vkey(1, 0)
		'    'vkey(0, 0) = "insurance_file_cnt"
		'    'vkey(1, 0) = 1522
		'    vkey(0, 0) = "is_renewal_ammend"
		'    vkey(1, 0) = False
		'    lreturn = o.NavigatorV3_SetKeys(vkey)
		'    lreturn = o.NavigatorV3_Start()
		'    lreturn = o.Terminate()
		'
		'    Set o = Nothing
		
	End Sub
	'UPGRADE_NOTE: (7013) Constructor is just executed once. Please review if Component contains SingleUse classes because they have a different behaviour. More Information: http://www.vbtonet.com/ewis/ewi7013.aspx
	Sub New()
		Main()
	End Sub
	Sub JustForInvokeMain()
	End Sub
End Module