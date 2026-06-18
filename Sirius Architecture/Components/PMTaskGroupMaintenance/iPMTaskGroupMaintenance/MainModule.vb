Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
'Developer Guide no. 129
Imports SharedFiles
Module MainModule
    'Developer Guide no. 50
    'developer guide no. 107
    <ThreadStatic()> _
    Public objfrmInterface As frmInterface
	Public Const ACApp As String = "TaskGroupMaintenance"
	
	' RDC 13062002 gPMLibraries replaced with gPM* BAS modules
	'Private m_lReturn As gPMLibraries.PMEReturnCode
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	Public Const USRAddGroup As Integer = 0
	Public Const USREditGroup As Integer = 1
	
	Public Const ACCancelDetailsTitleText As String = "Cancel Details"
	Public Const ACCancelDetailsText As String = "Cancelling will lose any changes" & Strings.Chr(13) & Strings.Chr(10) & "Do you want to cancel?"
	
	Public Const ACSaveDetailsTitleText As String = "Save Details"
	Public Const ACSaveDetailsText As String = "Details have changed" & Strings.Chr(13) & Strings.Chr(10) & "Do you want to save?"
	
	Public Const ACBusinessFailTitleText As String = "Business Object"
	Public Const ACBusinessFailText As String = "Unable to gain access to the business object" & Strings.Chr(13) & Strings.Chr(10) & "Please try later"
	
    ' Username.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sUsername As String = ""
	
	' Password.
	Public g_sPassword As New FixedLengthString(30)
	
    ' Calling Application
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sCallingAppName As String = ""
    ' Source ID
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    ' Language ID
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    ' Currency ID
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iCurrencyID As Integer
    ' LogLevel
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLogLevel As Integer
    ' UserID
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iUserID As Integer
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	' Form
	
	' Buttons
	
	' Messages
	
	' Menus
	
	' {* USER DEFINED CODE (End) *}
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the maxiumum search details.
	Public Const ACMaxSearchDetails As Integer = 500
	
	' Constant for the miniumum search length.
	Public Const ACMinSearchLength As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
End Module