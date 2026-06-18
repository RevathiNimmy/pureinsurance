Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 29/07/1999
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iGISSellerTool"
	
	Public Const ACHTMLStartTag As String = "<HTML>"
	Public Const ACHTMLEndTag As String = "</HTML>"
	Public Const ACHTMLBodyStartTag As String = "<BODY>"
	Public Const ACHTMLBodyEndTag As String = "</BODY>"
	
	Public Const ACOIMGISSubKey As String = "GIS" ' CL230200
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sUsername As String = ""
	
	'Public g_oEmptyClass As iGISSellerTool.EmptyClass
	
	'' ***************************************************************** '
	'' Description: This Method has been added to try and stop the Its4ME
	''              freeze problem. Creating a reference to this class in
	''              Sub Main will cause this component to never be released
	''              and has the same effect as setting the retained in memory
	''              setting in VB6.
	''
	''              See the following MS KB Articles for more information:
	''              Q186273, Q264957
	''
	'' Edit History: RFC Created 16/03/01
	''
	'' ***************************************************************** '
	'Sub Main()
	'
	'    Set g_oEmptyClass = New iGISSellerTool.EmptyClass
	'
	'End Sub
End Module