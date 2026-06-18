Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports SharedFiles
Module MainModule
	
	' You set APPDEBUG in the project properties!!!!
	
	Public Declare Function ShellExecute Lib "shell32.dll"  Alias "ShellExecuteA"(ByVal hwnd As Integer, ByVal lpOperation As String, ByVal lpFile As String, ByVal lpParameters As String, ByVal lpDirectory As String, ByVal nShowCmd As Integer) As Integer
	
	Public m_lError As gPMConstants.PMEReturnCode
	
	' Root node in the tree
	Public Const ACRootNode As String = "ROOT"
	
	' The class
	Public Const ACClass As String = "MainModule"
	' Public Const ACApp <- This is in Configuration.bas now
	
	' Icons
	Public Const ACIconFindForm As String = "findform"
	Public Const ACIconDataForm As String = "dataform"
	Public Const ACIconQuestion As String = "question"
	Public Const ACIconNavigate As String = "navigate"
	Public Const ACIconBusiness As String = "business"
	Public Const ACIconPrint As String = "print"
	
	' Panels
	Public Const ACPanelStatus As String = "status"
	Public Const ACPanelNavType As String = "navtype"
	
	' Nav Types
	Public Const ACNavigatorV3 As String = "navigatorv3"
	Public Const ACNavigatorV2 As String = "navigatorv2"
	Public Const ACAutoForCL As String = "autoforcl"
	
    ' Object Manager
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	' Our type. Just easier to group data this way instead of 2D array
	Public Structure Step_Renamed
		Dim Description As String
		Dim Component As String
		Dim Type As String
		Dim OKAction As String
		Dim CancelAction As String
		Dim OKSteps As Integer
		Dim CancelSteps As Integer
		Dim ComponentAction As Integer
		Dim ServerSide As Boolean
		Dim DefaultKeys As Object
		Dim CreateWorkManagerTask As Boolean
		Dim ResumeStep As Integer
		Public Shared Function CreateInstance() As Step_Renamed
			Dim result As New Step_Renamed
			result.Description = String.Empty
			result.Component = String.Empty
			result.Type = String.Empty
			result.OKAction = String.Empty
			result.CancelAction = String.Empty
			Return result
		End Function
	End Structure
	
    ' The steps
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vSteps() As Step_Renamed = Nothing
	
    ' Global key array
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_vKeyArray As Object
	
    ' Current step
    'developer guide no. 107
    <ThreadStatic()> _
 Public m_lCurrentStep As Integer
	
	'ED 28022002 - Count of Product options switched on steps -
	'              add to this value when a new option is switchable, if
	'              an existing option is switched reduce the value of
    '              ACMaxSteps and add to g_iSwitchedSteps
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSwitchedSteps As Integer
	
	Public Const ACNormalMode As Integer = 0
	Public Const ACMergeMode As Integer = 1
	' CTAF 130600 - Modes for printing
	Public Const ACPrintMode As Integer = 2
	Public Const ACPrintSilentMode As Integer = 3
	Public Const ACSpoolDocMode As Integer = 4
	Public Const ACSpoolReportMode As Integer = 5
	
	' Neuvo
	Public Const PMNavComponentPrintObject As String = "PO"
	
	Public Const ACResumeStepCurrent As Integer = -1
	
	'TF260202 - Transaction Types (from GIIConst.bas)
	'Now in gpmconstants.bas
	'Global Const PMTransactionTypeNB = "G_NB"
	'Global Const PMTransactionTypeMTA = "G_MTA"
	'Global Const PMTransactionTypeRenewals = "G_RENEW"
	Public Const PMTransactionTypeReview As String = "G_REVIEW"
	Public Const PMTransactionTypeDefaults As String = "G_DEFAULTS"
	Public Const PMTransactionTypeMTAFullQuote As String = "G_MTA_FQ"
	
	' Document codes
	Public Const PMDocumentNewBusinessQuoteDisplay As String = "NB_Q"
	' ED 06092002 - Renewals Confirmation Document
	Public Const PMDocumentRenewalsConfirmDocument As String = "RN_C"
	
	Public Const ACDefaultTaskDays As Integer = 7
	
	
	' ***************************************************************** '
	'
	' Name: CheckNav3
	'
	' Description: Checks if an object is nav3 or nav2 compliant
	'
	' History: 13/09/2001 CTAF - Created.
	'
	' ***************************************************************** '
	Public Function CheckNav2or3(ByVal v_oObject As aPMNav.NavigatorV2, ByRef r_bNav2 As Boolean, ByRef r_bNav3 As Boolean) As Integer
		
		Dim result As Integer = 0
		Dim oNav2 As aPMNav.NavigatorV2
		Dim oNav3 As aPMNav.NavigatorV3
		


		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		' Reset status
		r_bNav2 = False
		r_bNav3 = False
		
		' Try for nav2

		Try 
			oNav2 = v_oObject
			
			If Information.Err().Number = 0 Then
				r_bNav2 = True
			Else
				Information.Err().Clear()
				' Try for nav3
				oNav3 = v_oObject
				If Information.Err().Number = 0 Then
					r_bNav3 = True
				End If
			End If
			
			' Reset error status and trapping
			Information.Err().Clear()


			
			Return result
			
Err_CheckNav2or3: 
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckNav2or3 Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckNav2or3", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
			
			Return result
			


			
			Return result
		
		Catch exc As System.Exception
			NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
		End Try
	End Function
End Module