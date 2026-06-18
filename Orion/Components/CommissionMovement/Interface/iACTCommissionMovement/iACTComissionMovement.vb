Option Strict Off
Option Explicit On
Imports System
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: {TodaysDate}
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iACTDocument"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACDocumentRefCaption As Integer = 102
	Public Const ACDocumentDateCaption As Integer = 103
	Public Const ACCommentCaption As Integer = 104
	Public Const ACDocumentTypeCaption As Integer = 112
	
	' Buttons
	Public Const ACNavigateButton As Integer = 200
	Public Const ACHelpButton As Integer = 201
	Public Const ACCancelButton As Integer = 202
	Public Const ACOKButton As Integer = 203
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
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
	Public g_iSourceID As Integer
	Public g_iCompanyID As Integer
	Public g_iLanguageID As Integer
	Public g_iUserID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	'Product Family Name for Help
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFOrion
	
	' Screen Context ID for Help
	Public Const ScreenHelpID As Integer = 35000
	
	

	Public Sub Main()
		
		MoveCommission()
	End Sub
	Sub MoveCommission()
        'developer guide no.88
        Dim oInterface As New Object
		' Call the initialise method passing this interface
		' and the business object as parameters.
		Dim lReturn As gPMConstants.PMEReturnCode = CType(CType(oInterface, SSP.S4I.Interfaces.ILocalInterface).Initialise(), gPMConstants.PMEReturnCode)
		
		' Check for errors.
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			Exit Sub
		End If
		
		lReturn = CType(oInterface.Start(), gPMConstants.PMEReturnCode)
		
		' Check for errors.
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			Exit Sub
		End If
		
        oInterface.Dispose()
		
		
	End Sub
End Module