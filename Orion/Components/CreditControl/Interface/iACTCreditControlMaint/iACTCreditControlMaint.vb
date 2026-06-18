Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
'refer developer guide no. 129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 02nd October 2002
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
    'developer guide no.69
    'developer guide no. 107
    <ThreadStatic()> _
    Public ofrmStep As frmStep
    'developer guide no. 107
    <ThreadStatic()> _
    Public ofrmDetails As frmDetails

	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iACTCreditControlMaint"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	' General Icons
	
	' Task Group Task Data Array Positions
	Public Const ACTaskGroupTaskGroupId As Integer = 0
	Public Const ACTaskGroupDescription As Integer = 1
	Public Const ACTaskgroupTaskId As Integer = 2
	Public Const ACTaskGroupTaskDescription As Integer = 3
	
	' Task Group User Group Data Array Positions
	Public Const ACTaskGroupUserGroup_TaskGroupId As Integer = 0
	Public Const ACTaskGroupUserGroup_UserGroupId As Integer = 2
	Public Const ACTaskGroupUserGroup_UserGroupDescription As Integer = 3
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACAddDetailsTitle As Integer = 102
	Public Const ACEditDetailsTitle As Integer = 104
	Public Const ACAddNonInsTitle As Integer = 105
	Public Const ACEditNonInsTitle As Integer = 106
	Public Const ACAddInsTitle As Integer = 107
	Public Const ACEditInsTitle As Integer = 108
	
	' Buttons
	Public Const ACOKCaption As Integer = 200
	Public Const ACCancelCaption As Integer = 201
	Public Const ACHelpCaption As Integer = 202
	Public Const ACAddCaption As Integer = 203
	Public Const ACDeleteCaption As Integer = 204
	Public Const ACEditCaption As Integer = 205
	
	' Menus
	
	'Form Components
	Public Const ACMainTabTitle0 As Integer = 300

	' Messages
	Public Const ACCancelDetailsTitle As Integer = 700
	Public Const ACCancelDetails As Integer = 701
	Public Const ACBusinessFailTitle As Integer = 702
	Public Const ACBusinessFail As Integer = 703
    Public Const ACFailedToUpdateTitle As Integer = 706
	Public Const ACFailedUpdateDetail As Integer = 707
	
	' Messages
	Public Const ACInvalidStepNumberTitle As Integer = 708
	Public Const ACInvalidStepNumberDetails As Integer = 709
	
	Public Const ACNoSelectionTitle As Integer = 710
	Public Const ACNoSelectionDetails As Integer = 711
	
	Public Const ACConfirmDeleteTitle As Integer = 712
	Public Const ACConfirmDeleteDetails As Integer = 713
	
	Public Const ACRuleUsedTitle As Integer = 714
	Public Const ACRuleUsedDetails As Integer = 715
	
	Public Const ACNoBusinessTypeTitle As Integer = 716
	Public Const ACNoBusinessTypeDetails As Integer = 717
	Public Const ACNoBranchSelected As Integer = 718
	Public Const ACNoFrequencySelected As Integer = 719
	Public Const ACNoBusTypeSelected As Integer = 720
	Public Const ACMissingInformation As Integer = 721
	Public Const ACEditRENWTGUpdateTitle As Integer = 731
	
	' {* USER DEFINED CODE (End) *}
	
	' Public contants
	Public Const ACBusTypeBlank As String = "(" & """""" & ")"
	Public Const ACBusTypeINS As String = "INS"
	Public Const ACBusTypeMTA As String = "MTA"
	Public Const ACBusTypeNB As String = "NB"
	Public Const ACBusTypeREN As String = "REN"
	Public Const ACBusTypeTRANS As String = "TRANS"
	Public Const ACBusTypeRENUPD As String = "REN WTG UPDATE"
	Public Const AcBusTypeINSH As String = "INSH"
	Public Const AcBusTypeINSC As String = "INSC"
	
	Public Const ACBusTypeCLF As String = "CLF"
	Public Const ACBusTypeCLD As String = "CLD"
	Public Const ACBusTypeCIN As String = "CIN"
	Public Const ACBusTypeCIA As String = "CIA"
	
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	Public g_sUserName As String = ""
	
	' Extra variables for component services
	Public g_sPassword As String = ""
	Public g_iUserID As Integer
	Public g_iCurrencyID As Integer
	Public g_iLogLevel As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	

	Private m_oSystemOption As bSIROptions.Business
	Private m_lReturn As Integer
	
	' lookup tables
	Public Const ACLookupTablePMWrkTaskGroup As String = "PMWrk_Task_Group"
	
	' lookup detail constants
	Public Const ACDetailKey As Integer = 0
	Public Const ACDetailDesc As Integer = 1
	Public Const ACDetailCode As Integer = 2
	
	
	' ***************************************************************** '
	' Name: GetOption (Private)
	'
	' Description: Get an option.
	'
	' ***************************************************************** '
	Public Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As String, Optional ByRef vDatabase As Object = Nothing) As Integer
		
		
		Dim result As Integer = 0
		Try 
			
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If m_oSystemOption Is Nothing Then
				
				' Get an instance of the business object via
				' the public object manager.
				Dim temp_m_oSystemOption As Object
				m_lReturn = g_oObjectManager.GetInstance(temp_m_oSystemOption, "bSIROptions.Business", vInstanceManager:="ClientManager")
				m_oSystemOption = temp_m_oSystemOption
				
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			End If
			

			m_lReturn = m_oSystemOption.GetOption(iOptionNumber:=v_iOptionNumber, sValue:=r_sOptionValue)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				r_sOptionValue = "0"
				
			End If

		m_oSystemOption.Dispose()
			
			
			
			m_oSystemOption = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Public Function DisplayMessage(ByRef r_lTitleId As Integer, ByRef r_lMessageId As Integer, ByRef r_lOptions As Integer) As Integer
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: DisplayMessage
		' PURPOSE: Displays a message based on passed resource file Ids
		' AUTHOR: Sirius Financial Systems Plc
		' DATE: 09 October 2002, 16:03:54
		' RETURNS: PMTrue for success
		' CHANGES:
		' ---------------------------------------------------------------------------
		
		Dim result As Integer = 0
		Dim sTitle, sMessage As String
		
		
		Try
		

        'Developer Guide No. 243
        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=r_lTitleId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


        'Developer Guide No. 243
        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=r_lMessageId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
		
		result = Interaction.MsgBox(sMessage, r_lOptions, sTitle)
		
		
		
		'----------------------------------------------------------------------------------------
		'Only for Debugging, the code will never execute this line
		'----------------------------------------------------------------------------------------
		  
		
		Catch ex As Exception
		Select Case Information.Err().Number
			Case Else
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayMessage", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
				
				result = gPMConstants.PMEReturnCode.PMFalse
				
		End Select
		
		Finally
		
		
		
		End Try
		Return result
    End Function

End Module
