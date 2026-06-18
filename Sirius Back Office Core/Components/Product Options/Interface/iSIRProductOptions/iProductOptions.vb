'ADD the GetResData in the project
Option Strict Off
Option Explicit On
Imports System
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
Module MainModule
	' ***************************************************************** '
	' Form Name: Main Module
	'
	' Date: 11th June 2002
	'
	' Description: Contains all the constants
	'
	' Edit History:
	'   11062002 SJP - Created
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iProductOptions"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	'Start-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
	Public Const ACUniqueDocumentEnableWarning As Integer = 304
	Public Const ACUniqueDocumentDisableFail As Integer = 305
	'End-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
	
	' Menus
	
	' {* USER DEFINED CODE (End) *}
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
    ' Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
	
	'   Reference to default branch
	Public Const g_iBCHHeadOffice As Integer = 1
	
	'   Password
	' PW270303 - change password (requested by PK)
	Public Const g_sPasswordSearchString As String = "$1R1U5"
	
	'KB 01082003 PN 5797 The message should be in English!
	Public Const g_sPasswordMessage As String = "Please enter a password if you are a Sirius employee and " & "are configuring the system.  Otherwise please press the Cancel button."
	
	Public Const g_sUserAuthorityDenial As String = "This user does not have authority to perform this task"
	
	Public Const g_sDisabled As String = "Disabled"
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	Public Sub Main_Renamed()
		'   This is used for testing purposes
		
		'developer guide no.108
        Dim o As Interface_Renamed
        Dim vKeyArray(,) As Object = Nothing
		
		Dim lReturn As gPMConstants.PMEReturnCode = CType(CType(o, SSP.S4I.Interfaces.ILocalInterface).Initialise(), gPMConstants.PMEReturnCode)
		If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
			o.CallingAppName = "TEST"
			lReturn = CType(o.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd), gPMConstants.PMEReturnCode)
			
			lReturn = CType(o.Start(), gPMConstants.PMEReturnCode)

			lReturn = CType(o.GetKeys(vKeyArray), gPMConstants.PMEReturnCode)
            o.Dispose()
			
		End If
		
		'End
		
    End Sub
End Module