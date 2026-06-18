Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO
'Developer Guide no. 129
Imports SharedFiles
Module BusinessMain
	' ***************************************************************** '
	' Module Name: BusinesMain
	'
	' Date:  04-12-2002
	'
	' Description: Main Module.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "bACTCreditControlProcessing"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "BusinessMain"
	Private m_oBusiness As bACTCreditControlProcessing.Business
	
	Public Const ACSpokeInterfaceCode As String = "CREDITCONTROL"
	Public Const ACSpokeStatusCode As String = "A"
	Public Const ACSpokeMessage As String = "A"
	Public Const ACSpokeHeaderXML As String = "<XML>"
	Public Const ACSpokeDetailData As String = ""
	Public Const ACSpokeBatch As String = ""
	
	' To match bACTFinanceSpoke.ExportCreditControl class
	' Constants for the HeaderData array
	Public Const kbHDBranch As Byte = 9
	Public Const kbHDAsOfDate As Byte = 10
	Public Const kbHDSpoolDoc As Byte = 11
	Public Const kbHDArchiveDoc As Byte = 12
	
	' ***************************************************************** '
	' Name: Main
	'
	' Parameters: n/a
	'
	' Description: Main Control Routine for Credit Control Processing
	'
	' History:
	'           Created : MEvans : 10-02-2005 : Credit Control RetroFit
	' ***************************************************************** '
	Public Sub Main()
		
		Const kMethodName As String = "Main"
		
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim sBranchCode, sDate As String
		Dim bSpool, bArchive As Boolean
		Dim sErrDesc As String = ""
		
		Try
		
		
		
		' initialise business class
		lReturn = InitialiseBusiness()
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			sErrDesc = "InitialiseBusiness Failed"
			gPMFunctions.RaiseError(kMethodName, sErrDesc, gPMConstants.PMELogLevel.PMLogError)
		End If
		
		' process any command line argument that might have been passed
		lReturn = CType(ProcessCommandLineArgs(sBranchCode, sDate, bSpool, bArchive), gPMConstants.PMEReturnCode)
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			sErrDesc = "ProcessCommandArgs Failed"
			gPMFunctions.RaiseError(kMethodName, sErrDesc, gPMConstants.PMELogLevel.PMLogError)
		End If
		
		' start credit control processing
		lReturn = CType(StartCreditControlProcessing(sBranchCode, sDate, bSpool, bArchive), gPMConstants.PMEReturnCode)
		
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			If lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
				sErrDesc = "StartCreditControlProcessing Failed"
				gPMFunctions.RaiseError(kMethodName, sErrDesc, gPMConstants.PMELogLevel.PMLogError)
			End If
		End If
		
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		bPMFunc.LogError(v_sUsername:="", v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)
		
		' create process failed memo task for sys-admin group
		' so it is apparent to the user that the credit control processing has failed...
		lReturn = CType(m_oBusiness.CreateProcessFailedTask(sBranchCode, sDate, sErrDesc), gPMConstants.PMEReturnCode)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		
	 
		End Try
	End Sub
	
	' ***************************************************************** '
	' Name: ProcessCommandLineArgs
	'
	' Parameters: n/a
	'
	' Description: Examines passed command line arguments, validates
	'               and then defaults any that havent been passed
	'
	' History:
	'           Created : MEvans : 10-02-2005 : Credit Control RetroFit
	' ***************************************************************** '
	Public Function ProcessCommandLineArgs(ByRef r_sBranchCode As String, ByRef r_sDate As String, ByRef r_bSpool As Boolean, ByRef r_bArchive As Boolean) As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "ProcessCommandLineArgs"
		
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim llBound, lUBound As Integer
		Dim sCommandArg As String = ""
		Dim vCommandArg, vCommandLineArgs As Object
		
		Dim bBranchSpecified, bDateSpecified, bArchiveSpecified, bSpoolSpecified As Boolean
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		' split out command line arguments

		vCommandLineArgs = Interaction.Command().Split(" "c)
		

		llBound = vCommandLineArgs.GetLowerBound(0)

		lUBound = vCommandLineArgs.GetUpperBound(0)
		
		' process each argument
		For lCommandArg As Integer = llBound To lUBound
			
			' get the argument type


            vCommandArg = CStr(vCommandLineArgs(lCommandArg)).Split("="c)

            ' if both parts of the argument have been passed (it is in a valid format)

            If vCommandArg.GetUpperBound(0) = 1 Then

                ' validate and store the arguments


                Select Case CStr(vCommandArg(0)).ToUpper()
                    Case "BRANCH"
                        bBranchSpecified = True


                        lReturn = CType(IsValidBranch(CStr(vCommandArg(1))), gPMConstants.PMEReturnCode)
                        If lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                            r_sBranchCode = CStr(vCommandArg(1))
                        Else

                            gPMFunctions.RaiseError(kMethodName, "IsValidBranch Returned False for branch = " & CStr(vCommandArg(1)), gPMConstants.PMELogLevel.PMLogError)
                        End If

                    Case "DATE"
                        bDateSpecified = True


                        lReturn = CType(IsValidDate(CStr(vCommandArg(1))), gPMConstants.PMEReturnCode)
                        If lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                            r_sDate = CStr(vCommandArg(1))
                        Else

                            gPMFunctions.RaiseError(kMethodName, "IsValidDate Returned False for date = " & CStr(vCommandArg(1)), gPMConstants.PMELogLevel.PMLogError)
                        End If

                    Case "SPOOL"
                        bSpoolSpecified = True


                        lReturn = CType(IsValidSpool(CStr(vCommandArg(1))), gPMConstants.PMEReturnCode)
                        If lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                            r_bSpool = CBool(vCommandArg(1))
                        Else

                            gPMFunctions.RaiseError(kMethodName, "IsValidSpool Returned False for Spool = " & CStr(vCommandArg(1)), gPMConstants.PMELogLevel.PMLogError)
                        End If

                    Case "ARCHIVE"
                        bArchiveSpecified = True


                        lReturn = CType(IsValidArchive(CStr(vCommandArg(1))), gPMConstants.PMEReturnCode)
                        If lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                            r_bArchive = CBool(vCommandArg(1))
                        Else

                            gPMFunctions.RaiseError(kMethodName, "IsValidArchive Returned False for Archive = " & CStr(vCommandArg(1)), gPMConstants.PMELogLevel.PMLogError)
                        End If

                    Case Else
                        ' ignore any additional passed params
                        ' as these are not catered for....

                End Select
            End If
		Next 
		
		' set parameter defaults if command line args have not been passed
		If Not bBranchSpecified Then
			r_sBranchCode = "ALL"
		End If
		
		If Not bDateSpecified Then
			r_sDate = DateTimeHelper.ToString(DateTime.Now)
		End If
		
		If Not bSpoolSpecified Then
			r_bSpool = True
		End If
		
		If Not bArchiveSpecified Then
			r_bArchive = True
		End If
		
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		bPMFunc.LogError(v_sUsername:="", v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		
		
		
'		Return result
'		Resume 
'		Return result
		End Try
		Return result
	End Function
	
	' ***************************************************************** '
	' Name: IsValidBranch
	'
	' Parameters: n/a
	'
	' Description: Validates the specified branch.
	'
	' History:
	'           Created : MEvans : 10-02-2005 : Credit Control RetroFit
	' ***************************************************************** '
	Public Function IsValidBranch(ByVal v_sCommandLineArg As String) As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "IsValidBranch"
		
		Dim lReturn As gPMConstants.PMEReturnCode
		
		Try
		
		
		
		
		
		result = gPMConstants.PMEReturnCode.PMFalse
		
		If v_sCommandLineArg.ToUpper() = "ALL" Then
			result = gPMConstants.PMEReturnCode.PMTrue
		Else
			
			' check the source table to confirm the specified source code is valid
			lReturn = CType(m_oBusiness.IsBranchValid(v_sCommandLineArg), gPMConstants.PMEReturnCode)
			If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMTrue
			Else
				result = gPMConstants.PMEReturnCode.PMFalse
			End If
			
		End If
		
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		bPMFunc.LogError(v_sUsername:="", v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
'		Return result
'		Resume 
'		Return result
		End Try
		Return result
	End Function
	
	
	' ***************************************************************** '
	' Name: IsValidDate
	'
	' Parameters: n/a
	'
	' Description: Validates the specified date
	'
	' History:
	'           Created : MEvans : 10-02-2005 : Credit Control RetroFit
	' ***************************************************************** '
	Public Function IsValidDate(ByVal v_sCommandLineArg As String) As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "IsValidDate"
		
		Dim lReturn As Integer
		
		Try
		
		
		
		Dim dbNumericTemp As Double
		If Information.IsDate(v_sCommandLineArg) Then
			result = gPMConstants.PMEReturnCode.PMTrue
		ElseIf Double.TryParse(v_sCommandLineArg, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then 
			result = gPMConstants.PMEReturnCode.PMTrue
		Else
			result = gPMConstants.PMEReturnCode.PMFalse
		End If
		
		
		Catch ex As Exception
		' DO Not Call any functions before here or the error will be lost
		bPMFunc.LogError(v_sUsername:="", v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		
'		Return result
'		Resume 
'		Return result
		End Try
		Return result
	End Function
	
	' ***************************************************************** '
	' Name: IsValidSpool
	'
	' Parameters: n/a
	'
	' Description: Validates the spool indicator
	'
	' History:
	'           Created : MEvans : 10-02-2005 : Credit Control RetroFit
	' ***************************************************************** '
	Public Function IsValidSpool(ByVal v_sCommandLineArg As String) As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "IsValidSpool"
		
		Dim lReturn As Integer
		
		Try
		
		
		
		If v_sCommandLineArg.ToUpper() = "TRUE" Or v_sCommandLineArg.ToUpper() = "FALSE" Then
			result = gPMConstants.PMEReturnCode.PMTrue
		Else
			result = gPMConstants.PMEReturnCode.PMFalse
		End If
		
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		bPMFunc.LogError(v_sUsername:="", v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		
'		Return result
'		Resume 
'		Return result
		End Try
		Return result
	End Function
	
	' ***************************************************************** '
	' Name: IsValidArchive
	'
	' Parameters: n/a
	'
	' Description: Validates the archive indicator
	'
	' History:
	'           Created : MEvans : 10-02-2005 : Credit Control RetroFit
	' ***************************************************************** '
	Public Function IsValidArchive(ByVal v_sCommandLineArg As String) As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "IsValidArchive"
		
		Dim lReturn As Integer
		
		Try
		
		
		
		If v_sCommandLineArg.ToUpper() = "TRUE" Or v_sCommandLineArg.ToUpper() = "FALSE" Then
			result = gPMConstants.PMEReturnCode.PMTrue
		Else
			result = gPMConstants.PMEReturnCode.PMFalse
		End If
		
		
		Catch ex As Exception
		
		' Do Not Call any functions before here or the error will be lost
		bPMFunc.LogError(v_sUsername:="", v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		
'		Return result
'		Resume 
'		Return result
		End Try
		Return result
	End Function
	
	' ***************************************************************** '
	' Name: StartCreditControlProcessing
	'
	' Parameters: n/a
	'
	' Description: Starts the Credit Control Processing
	'
	' History:
	'           Created : MEvans : 10-02-2004 : Credit Control Retrofit
	' ***************************************************************** '
	Public Function StartCreditControlProcessing(ByVal v_sBranchCode As String, ByVal v_sDate As String, ByVal v_bSpool As Boolean, ByVal v_bArchive As Boolean) As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "StartCreditControlProcessing"
		
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim dtDate As Date
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		' determine the actual date to process
		Dim dbNumericTemp As Double
		If Information.IsDate(v_sDate) Then
			dtDate = CDate(v_sDate)
		ElseIf Double.TryParse(v_sDate, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then 
			dtDate = DateTime.Today.AddDays(CInt(v_sDate))
		End If
		
		' process the credit control items for the specified branch / date
		lReturn = CType(m_oBusiness.CreditControlProcessing(v_sBranchCode, dtDate, v_bSpool, v_bArchive), gPMConstants.PMEReturnCode)
		
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			
			If lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
				result = gPMConstants.PMEReturnCode.PMNotFound
			Else
				gPMFunctions.RaiseError(kMethodName, "CreditControlProcessing Failed", gPMConstants.PMELogLevel.PMLogError)
			End If
		End If
		
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		bPMFunc.LogError(v_sUsername:="", v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		
'		Return result
'		Resume 
'		Return result
		End Try
		Return result
	End Function
	
	' ***************************************************************** '
	' Name: InitialiseBusiness
	'
	' Parameters: n/a
	'
	' Description: creates and sets up the Business class
	'
	' History:
	'           Created : MEvans : 10-02-2005 : Credit Control RetroFit
	' ***************************************************************** '
	Public Function InitialiseBusiness() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "InitialiseBusiness"
		
		Dim lReturn As gPMConstants.PMEReturnCode
		Dim sUsername, sPassword As String
		Dim iUserID As Integer
		Dim sCallingAppName As String = ""
		Dim iSourceID, iLanguageID, iCurrencyID, iLogLevel As Integer
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		'get new instance of business class
		m_oBusiness = New bACTCreditControlProcessing.Business()
		
		' set defaults
		sUsername = "sirius"
		sPassword = "sirius"
		iUserID = 1
		sCallingAppName = ACApp
		iSourceID = 1
		iLanguageID = 1
		iCurrencyID = 26
		iLogLevel = 6
		
		' initialise business class
		lReturn = CType(CType(m_oBusiness, SSP.S4I.Interfaces.IBusiness).Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp), gPMConstants.PMEReturnCode)
		
		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "Business.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		bPMFunc.LogError(v_sUsername:="", v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		
'		Return result
'		Resume 
'		Return result
		End Try
		Return result
	End Function
End Module
