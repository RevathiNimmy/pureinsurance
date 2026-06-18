Option Strict Off
Option Explicit On
Imports System
Public Module ListCustomConst
	' ***************************************************************** '
	' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
	' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
	' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
	' ***************************************************************** '
	
	' ***************************************************************** '
	' Class Name: ListCustomConst
	'
	' Date: 10/02/1999
	'
	' Description: Contains the Field Array positional constants for
	'              the GEMListCustom Interface and Business objects
	'
	' Edit History:
	' ***************************************************************** '
	
	' Constant used to Dimension Field Array
	Public Const ACListCustomFieldArraySize As Integer = 6
	
	' Field Array positional constants
	Public Const ACListCustom_ListCustomID As Integer = 0
	Public Const ACListCustom_PositionID As Integer = 1
	Public Const ACListCustom_ValueID As Integer = 2
	Public Const ACListCustom_Text As Integer = 3
	Public Const ACListCustom_AbiCode As Integer = 4
	Public Const ACListCustom_Command As Integer = 5
	Public Const ACListCustom_PropertyID As Integer = 6
    'Modified by Deepak Sharma on 4/20/2010 3:10:51 PM refer developer guide no. 29 (no Solutions)
    'Shared Sub New()
    '	MainModule.JustForInvokeMain()

End Module