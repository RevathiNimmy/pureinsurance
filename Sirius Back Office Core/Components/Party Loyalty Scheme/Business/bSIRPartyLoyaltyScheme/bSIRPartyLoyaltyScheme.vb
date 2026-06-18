Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date:  02nd October 2002
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	' ************************************************
	' Added to replace global variables 27/11/2003
	Private m_sUsername As String = ""
	Private m_sPassword As String = ""
	Private m_iUserID As Integer
	Private m_sCallingAppName As String = ""
	Private m_iSourceID As Integer
	Private m_iLanguageID As Integer
	Private m_iCurrencyID As Integer
	Private m_iLogLevel As Integer
	' ************************************************
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bSIRPartyLoyaltyScheme"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Username.
	Public g_sUsername As New FixedLengthString(12)
	
	' Password.
	Public g_sPassword As New FixedLengthString(30)
	
    ' User ID
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iUserID As Integer
	
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
	
    Public ACMandatoryPartyLoyaltySchemeId As gPMConstants.PMEMandatoryStatus = gPMConstants.PMEMandatoryStatus.PMMandatory
    Public ACMandatoryPartyCnt As gPMConstants.PMEMandatoryStatus = gPMConstants.PMEMandatoryStatus.PMMandatory
    Public ACMandatoryLoyaltySchemeId As gPMConstants.PMEMandatoryStatus = gPMConstants.PMEMandatoryStatus.PMMandatory
    Public ACMandatoryMemberNumber As gPMConstants.PMEMandatoryStatus = gPMConstants.PMEMandatoryStatus.PMMandatory
    Public ACMandatoryMainMemberNumber As gPMConstants.PMEMandatoryStatus = gPMConstants.PMEMandatoryStatus.PMNonMandatory
    Public ACMandatoryOtherRef As gPMConstants.PMEMandatoryStatus = gPMConstants.PMEMandatoryStatus.PMNonMandatory
    Public ACMandatoryStartDate As gPMConstants.PMEMandatoryStatus = gPMConstants.PMEMandatoryStatus.PMMandatory
    Public ACMandatoryEndDate As gPMConstants.PMEMandatoryStatus = gPMConstants.PMEMandatoryStatus.PMNonMandatory
    Public ACMandatoryIsActive As gPMConstants.PMEMandatoryStatus = gPMConstants.PMEMandatoryStatus.PMMandatory
	
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

			bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Call to " & CStr(vChild) & " Failed ", vApp:=vApp, vClass:=vClass, vMethod:=vMethod)
		
		Catch 
			
			
			
			' not a lot we can do if LogMessage fails!
		End Try
		
		
		
	End Sub
	
	
	' ***************************************************************** '
	' Name: ConvertToTrueDataType (Public)
	'
	' Description:
	'
	'
	' ***************************************************************** '
	Public Function ConvertToTrueDataType(ByVal v_vFrom As Object, ByRef r_vTo As Object) As Integer
		Dim Err_ConvertToTrueDataType As Boolean = False
		
		Dim result As Integer = 0
		Const ACMethod As String = "ConvertToTrueDataType"
		
		Try 
			Err_ConvertToTrueDataType = True
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If Not False Then
				


				If (CStr(v_vFrom) = "") Or (Convert.IsDBNull(v_vFrom) Or IsNothing(v_vFrom)) Then
					
					' convert empty string to the correct value according to the
					' true data type of 'TO' variable
					
					
					Select Case Information.VarType(r_vTo)
						Case VariantType.Date
							

							r_vTo = #12/30/1899#
							
						Case VariantType.Short, VariantType.Integer, VariantType.Decimal, VariantType.Single, VariantType.Double, VariantType.Decimal
							

							r_vTo = 0
							
						Case VariantType.Boolean
							

							r_vTo = False
							
						Case Else
							
					End Select
					
				Else


					r_vTo = v_vFrom
				End If
			End If
			
			Return result
		
		Catch excep As System.Exception
			If Not Err_ConvertToTrueDataType Then
				Throw excep
			End If
			


			
			If Err_ConvertToTrueDataType Then
				
				
				' Error.
				result = gPMConstants.PMEReturnCode.PMError
				
				' Log Error Message
				bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to Convert To True Data Type ", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
				
				Return result
				
			End If
		End Try
	End Function
End Module
