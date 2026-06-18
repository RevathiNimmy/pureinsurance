Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
 Public Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 07/11/2002
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMBPartyLoyalty"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	' Constants used when setting control Tag property as used for formatting by iPMForms
	Public Const ACFormatPartyLoyaltyScheme As String = ""
	Public Const ACFormatMemberNumber As String = ""
	Public Const ACFormatMainMemberNumber As String = ""
	Public Const ACFormatOtherRef As String = ""
	Public Const ACFormatStartDate As String = "FMT;DT"
	Public Const ACFormatEndDate As String = "FMT;DT"
	Public Const ACFormatIsActive As String = "" ' formatis set to boolean according to control type
	
	
	' Constants for defining which entry to use from resource file for caption
	Public Const ACCaptionInterfaceTitle As Integer = 100
	Public Const ACCaptionTabTitle1 As Integer = 101 ' Not used
	Public Const ACCaptionPartyLoyaltyScheme As Integer = 102
	Public Const ACCaptionMemberNumber As Integer = 103
	Public Const ACCaptionOtherRef As Integer = 104
	Public Const ACCaptionStartDate As Integer = 105
	Public Const ACCaptionEndDate As Integer = 106
	Public Const ACCaptionMainMemberNumber As Integer = 107
	Public Const ACCaptionIsActive As Integer = 108
	
	' Buttons
	Public Const ACCaptionOKButton As Integer = 200
	Public Const ACCaptionCancelButton As Integer = 201
	Public Const ACCaptionHelpButton As Integer = 202
	Public Const ACCaptionNavigateButton As Integer = 203
	
	' Messages
	Public Const ACCaptionCancelDetailsTitle As Integer = 300
	Public Const ACCaptionCancelDetails As Integer = 301
	Public Const ACCaptionBusinessFailTitle As Integer = 302
	Public Const ACCaptionBusinessFail As Integer = 303
	Public Const ACCaptionInvalidDateTitle As Integer = 304
	Public Const ACCaptionStartEndDateMismatch As Integer = 305
	Public Const ACCaptionLoyaltySchemeTitle As Integer = 306
	Public Const ACCaptionLoyaltySchemeDuplicate As Integer = 307
	
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
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sUserName As String = ""
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	
	' ***************************************************************** '
	' Name: LogFailedCall
	'
	' Description: Wrapper function to the log message method of the
	'              message object.
	'
	' Changes:
	' ***************************************************************** '
	Public Sub LogFailedCall(ByRef vApp As Object, ByRef vClass As Object, ByRef vMethod As Object, ByRef vChild As Object)
		
		Try 
			
			' Log Error Message

			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Call to " & CStr(vChild) & " Failed ", vApp:=vApp, vClass:=vClass, vMethod:=vMethod)
		
		Catch 
			
			
			
			' not a lot we can do if LogMessage fails!
		End Try
		
		
		
	End Sub
	
	
	
	Public Function DisplayMessage(ByRef r_lTitleId As Integer, ByRef r_lMessageId As Integer, ByRef r_lOptions As Integer, ParamArray ByVal r_vTokens() As Object) As Integer
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: DisplayMessage
		' PURPOSE: Displays a message based on passed resource file Ids
		' AUTHOR: Sirius Financial Systems Plc
		' DATE: 09 October 2002, 16:03:54
		' RETURNS: PMTrue for success
		' CHANGES: PWC 16/10/2002 - Added param array to enable substition of tokens
		' ---------------------------------------------------------------------------
		
		Dim result As Integer = 0
		Const ACMethod As String = "DisplayMessage"
		
		Dim sTitle, sMessage As String
		
		
		Try
		
		'Get the title from the res file

        'developer guide no. 243
        sTitle = CStr(iPMFunc.GetResData(g_iLanguageID, r_lTitleId, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
		
		'Get the message from the res file

        'developer guide no. 243
        sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, r_lMessageId, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

		'Replace Tokens in the title
		ReplaceTokens(sTitle, New Object(){r_vTokens})
		
		'Replace Tokens in the message
		ReplaceTokens(sMessage, New Object(){r_vTokens})
		
		'Now display the message to the user
		result = Interaction.MsgBox(sMessage, r_lOptions, sTitle)
		
		Return result
		
		'----------------------------------------------------------------------------------------
		'Only for Debugging, the code will never execute this line
		'----------------------------------------------------------------------------------------
	
		
		Catch ex As Exception
		Select Case Information.Err().Number
			Case Else
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
				
				result = gPMConstants.PMEReturnCode.PMError
				
				Return result
		End Select
		
		Finally
				
		End Try
		Return result
	End Function
	
	Public Function ReplaceTokens(ByRef r_sMessage As String, ParamArray ByVal r_vTokens() As Object) As Boolean
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: ReplaceTokens
		' PURPOSE: Replace place holders with values
		'          e.g. convert |Username| to "Fred Bloggs"
		' AUTHOR: Paul Cunnigham
		' DATE: 21 October 2002, 10:04:52
		' RETURNS: True for success
		' CHANGES:
		' ---------------------------------------------------------------------------
		
		Dim result As Boolean = False
		Const ACMethod As String = "ReplaceTokens"
		
		Dim lUpper As Integer
		Dim vToken, vValue As Object
		
		
		
		
		'This routine could be called directly like...
		'    ReplaceTokens sMessage, "Usename", m sUserName
		'With the params explicitly listed
		
		'OR by a routine that itself accepts a ParamArray.
		'    ReplaceTokens sMessage, r_vParams
		
		'We need to ensure that we find the 'root' ParamArray as the second
		'calling method would pass Variant(0)(0), Variant(0)(1) into this routine
		'and we need Variant(0), Variant(1)
		

		Try 
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Find the 'root' paramarray
			'(i.e. convert  Variant(0)(0) to Variant(0))
			Dim vParams As Object = r_vTokens

            'developer guide no. 232(latest guide)
            While vParams(0).GetType().Name = "Object[]"
                If Information.Err().Number <> 0 Then
                    'No params passed at all
                    Information.Err().Clear()
                    Return result
                End If


                vParams = vParams(0)

                If vParams.GetUpperBound(0) = -1 Then ' no params
                    Return result
                End If
            End While




            'Any params actually passed?
            'We could just have a blank paramarray
            lUpper = r_vTokens.GetUpperBound(0)
            If lUpper <> -1 Then
                'Loop through the param array
                For iItem As Integer = 0 To lUpper \ 2
                    'Get the token and its replacement value

                    vToken = vParams(iItem * 2)
                    'This will bomb if developer has passed an odd number of params

                    vValue = vParams(iItem * 2 + 1)

                    'Replace the token with the value

                    r_sMessage = r_sMessage.Replace("|" & CStr(vToken) & "|", CStr(vValue))
                Next
            End If
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			GoTo Finally_Renamed
			
			'----------------------------------------------------------------------------------------
			'Only for Debugging, the code will never execute this line
			'----------------------------------------------------------------------------------------


			
Catch_Renamed: 
			Select Case Information.Err().Number
				Case Else
					' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
					
					result = gPMConstants.PMEReturnCode.PMError
					
					GoTo Finally_Renamed
			End Select
			
Finally_Renamed: 
			Return result
		
		Catch exc As System.Exception
			NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
		End Try
		
    End Function

End Module
