Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
'developer guide no. 129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 23-07-1997
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iACTExplorer"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	'EK 220300
	Private Const ACLevelMax As Integer = 10
	' Public interface constants used when
	' retrieving data from the resource file.
	
	
	' General Icons
	
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	Public Const ACMissingMandatoryFieldsTitle As Integer = 304
	Public Const ACMissingShortCodeText As Integer = 305
	'DC280703 -ISS5503
	Public Const ACMissingAccountNameText As Integer = 306
	
	' Menus
	
	
	' {* USER DEFINED CODE (End) *}
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	
	' Username.
	Public g_sUsername As New FixedLengthString(12)
	Public g_iUserID As Integer
	
	' Password.
	Public g_sPassword As New FixedLengthString(30)
    ' Calling Application
    Public g_sCallingAppName As String = ""
	' Company ID
	Public g_iCompanyId As Integer
	' Currency ID
	Public g_iCurrencyID As Integer
	' LogLevel
	Public g_iLogLevel As Integer
	
	Public Const ScreenHelpID As Integer = 1
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFOrion
	
    Public m_dragSource As New Windows.Forms.Control
	' Win32 API
	Declare Function SetCursorPos Lib "user32" (ByVal x As Integer, ByVal y As Integer) As Integer
	
	Structure POINTAPI
		Dim x As Integer
		Dim y As Integer
	End Structure
	

	Declare Function GetCursorPos Lib "user32" (ByRef lpPoint As POINTAPI) As Integer
	
    'RFC221001 - This had to be added due to GIIFunctions/Constants being added.
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_oGIS As Object
End Module