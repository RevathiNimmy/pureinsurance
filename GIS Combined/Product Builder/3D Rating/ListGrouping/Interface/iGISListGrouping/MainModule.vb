Option Strict Off
Option Explicit On
Imports System
Module MainModule
	
	Public Const ACApp As String = "iGISListGrouping"
	
	Private Const ACClass As String = "MainModule"
	
    ' Put it in here as it'll be accessed from multiple forms
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBusiness As Object
	
    ' The GIS Scheme ID we're working with
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_lGISSchemeID As Integer
	
    ' Object Manager
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	Public Const ACSummaryArrayListTypeID As Integer = 0
	Public Const ACSummaryArrayCode As Integer = 1
	Public Const ACSummaryArrayDescription As Integer = 2
	Public Const ACSummaryArrayUsed As Integer = 3
	
	Public Const ACGroupingArrayID As Integer = 0
	Public Const ACGroupingArrayCode As Integer = 1
	Public Const ACGroupingArrayDescription As Integer = 2
	Public Const ACGroupingArrayIsDeleted As Integer = 3
	Public Const ACGroupingArrayItemsCnt As Integer = 4
	
	Public Const ACListArrayID As Integer = 0
	Public Const ACListArrayCode As Integer = 1
	Public Const ACListArrayDescription As Integer = 2
	Public Const ACListArraySelected As Integer = 3
	
	Public Const ACCaptionDelete As String = "&Delete"
	Public Const ACCaptionUnDelete As String = "&Undelete"
	
	Public Const ACDeletedIcon As String = "cross"
	Public Const ACNormalIcon As String = "normal"
End Module