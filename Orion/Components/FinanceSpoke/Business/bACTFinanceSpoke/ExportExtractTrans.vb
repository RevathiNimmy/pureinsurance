Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'Developer Guide no.129
Imports SharedFiles
Friend NotInheritable Class ExportExtractTrans 
	'====================================================================
	'   Class/Module: ExportExtractTrans
	'   Description : Class implementation of use case:
	'Export for InterfaceCode: "ExtractTrans"'
	'
	'====================================================================
	'   Maintenance History
	'
	'    11 November 2002    Paul Cunnigham    Created.
	'
	'====================================================================
	
	
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "ExportExtractTrans"
	
	'#Region " Private fields "
	Private m_lReturn As Integer
	Private m_oBusiness As Business
	Private m_oDatabase As dPMDAO.Database
	'#End Region
	
	' ************************************************
	' Added to replace global variables 24/09/2003
	' Username.
	Private m_sUsername As String = ""
	' Password.
	Private m_sPassword As String = ""
	' User ID
	Private m_iUserID As Integer
	' Calling Application
	Private m_sCallingAppName As String = ""
	' Source ID
	Private m_iSourceID As Integer
	' Language ID
	Private m_iLanguageID As Integer
	' Currency ID
	Private m_iCurrencyID As Integer
	' LogLevel
	Private m_iLogLevel As Integer
	' ************************************************
	
	'#Region " Private Enums "
	'Columns in HeaderData array
	Private Enum HeaderDataCols
		MediaTypeCode = 9
		BranchCode
		ExtractDate
	End Enum
	'#End Region
	
	'#Region " Stored Procedures "
    'developer guide no. 39
    Private Const ksSPExportExtractTransSQL As String = "spu_ACT_Spoke_ExportExtractTrans"
	Private Const ksSPExportExtractTransName As String = "GetExportExtractTrans"
	Private Const ksSPExportExtractTransStored As Boolean = True
	'#End Region
	
	'#Region " Friend Properties "
	Friend WriteOnly Property Business() As Business
		Set(ByVal Value As Business)
			
			m_oBusiness = Value
			
		End Set
	End Property
	
	Friend WriteOnly Property Database() As dPMDAO.Database
		Set(ByVal Value As dPMDAO.Database)
			
			m_oDatabase = Value
			
		End Set
	End Property
	'#End Region
	
	Friend Function PassThroughLogin(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef sCallingAppName As String, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer) As Integer
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: PassThroughLogin
		' PURPOSE: Pass through the module level login information to the Class.
		' This is for COM+. Normally a business class will not require this but the Spoke
		' design means that Classes are instantiated by the Business class and can
		' no longer rely on global variables.
		' AUTHOR: Danny Davis
		' DATE: 24 September 2003, 11:55 AM
		' RETURNS: PMTrue for success
		' CHANGES:
		' ---------------------------------------------------------------------------
		
		Dim result As Integer = 0
		
		Try
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		m_sUsername = sUsername
		m_sPassword = sPassword
		m_iUserID = iUserID
		m_sCallingAppName = sCallingAppName
		m_iSourceID = iSourceID
		m_iLanguageID = iLanguageID
		m_iCurrencyID = iCurrencyID
		m_iLogLevel = iLogLevel
		
		
		'----------------------------------------------------------------------------------------
		'Only for Debugging, the code will never execute this line
		'----------------------------------------------------------------------------------------
		  
		
		Catch ex As Exception
		Select Case Information.Err().Number
			Case Else
				' Log Error.
				bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="PassThroughLogin", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
				
				result = gPMConstants.PMEReturnCode.PMFalse
				
		End Select
		
		Finally
		
		
		
		End Try
		Return result
	End Function
	
	
	'#Region " Friend Methods "
	Friend Function Start(ByRef r_sStatusCode As String, ByRef r_sMessage As String, ByRef r_vHeaderData() As Object, ByRef r_vDetailData As Object) As Integer
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: Start
		' PURPOSE: Start process for use case
		' AUTHOR: Paul Cunnigham
		' DATE: 11 November 2002, 11:45:03
		' RETURNS: PMTrue for success
		' CHANGES:
		' ---------------------------------------------------------------------------
		
		Dim result As Integer = 0
		Dim sMediaTypeCode, sBranchCode As String
		Dim dtExtractDate As Date
		Dim vResults, vHeaderArrayValues As Object
		
		
		Try
		
		result = gPMConstants.PMEReturnCode.PMFalse
		
		'Return codes
		r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_FAILED
		r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_FAILED
		
		'We need valid database and business objects
		If (m_oBusiness Is Nothing) Or (m_oDatabase Is Nothing) Then Return result
		
		'OK do the Export processing...
		
		'Get the values from the header array


		vHeaderArrayValues = r_vHeaderData(conValue)
		
		'Assign the values

        sMediaTypeCode = CStr(vHeaderArrayValues(HeaderDataCols.MediaTypeCode))

        sBranchCode = CStr(vHeaderArrayValues(HeaderDataCols.BranchCode))

        dtExtractDate = CDate(CDate(vHeaderArrayValues(HeaderDataCols.ExtractDate)).ToString("dd-MMM-yyyy"))
		
		'Get the extract transactions from the database based on the passed criteria
		If GetExtractTrans(r_sMediaTypeCode:=sMediaTypeCode, r_sBranchCode:=sBranchCode, r_dtExtractDate:=dtExtractDate, r_vResultArray:=vResults) <> gPMConstants.PMEReturnCode.PMTrue Then Return result
		
		'Now add the extra columns that the HUB requires
		'(indicates the status of each record)

		If AddHUBColumnsToDetailArray(v_sStatusCode:=gHUBSpokeConstants.k_STATUS_SIRIUS_RECORD_EXPORT_SUCCESS, v_sStatusMsg:=gHUBSpokeConstants.k_MESSAGE_SIRIUS_RECORD_EXPORT_SUCCESS, r_vResultArray:=vResults, v_sUsername:=m_sUsername) <> gPMConstants.PMEReturnCode.PMTrue Then Return result
		
		'If we have results then we need to add a new element to the
		'r_vDetailData array


		If AddResultArrayToDetailArray(v_vDetailArray:=r_vDetailData, r_vResultArray:=vResults, v_sUsername:=m_sUsername) <> gPMConstants.PMEReturnCode.PMTrue Then Return result
		
		'Return codes
		r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_COMPLETE
		r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_COMPLETE
		
		If Information.IsArray(vResults) Then
			result = gPMConstants.PMEReturnCode.PMTrue
		Else
			result = gPMConstants.PMEReturnCode.PMNotFound
		End If
		
		
		'----------------------------------------------------------------------------------------
		'Only for Debugging, the code will never execute this line
		'----------------------------------------------------------------------------------------
	
		Catch ex As Exception
		Select Case Information.Err().Number
			Case Else
				' Log Error.
				bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
				
				result = gPMConstants.PMEReturnCode.PMError

                    Return result
		End Select
		
		Finally
		
		
		End Try
		Return result
	End Function
	'#End Region
	
	'#Region " Private Methods "
	Public Sub New()
		MyBase.New()
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: Class_Initialize
		' PURPOSE: Class initialisation
		' AUTHOR: Paul Cunnigham
		' DATE: 11 November 2002, 12:08:23
		' CHANGES:
		' ---------------------------------------------------------------------------
		
		
		Try
		
		'Class initialisation
		m_oBusiness = Nothing
		m_oDatabase = Nothing
		
		
		'----------------------------------------------------------------------------------------
		'Only for Debugging, the code will never execute this line
		'----------------------------------------------------------------------------------------
		 
		
		Catch ex As Exception
		Select Case Information.Err().Number
			Case Else
				' Log Error.
				bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
				
		End Select
		
		Finally
		
		
		End Try
	End Sub
	
	Private Function GetExtractTrans(ByRef r_sMediaTypeCode As String, ByRef r_sBranchCode As String, ByRef r_dtExtractDate As Date, ByRef r_vResultArray(,) As Object) As Integer 
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: GetExtractTrans
		' PURPOSE: Gets the receipt transactions processed for passed criteria
		' AUTHOR: Paul Cunnigham
		' DATE: 11 November 2002, 12:53:53
		' RETURNS: PMTrue for success
		' CHANGES:
		' ---------------------------------------------------------------------------
		
		Dim result As Integer = 0 
		
		
		result = gPMConstants.PMEReturnCode.PMFalse
		
		' Clear the Database Parameters Collection
		m_oDatabase.Parameters.Clear()
		
		' Add mediatype_code as an input param
		If m_oDatabase.Parameters.Add(sName:="mediatype_code", vValue:=r_sMediaTypeCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result
		
		' Add branch_code as an input param
		If m_oDatabase.Parameters.Add(sName:="branch_code", vValue:=r_sBranchCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result
		
		' Add extract_date as an input param
		If m_oDatabase.Parameters.Add(sName:="extract_date", vValue:=DateTimeHelper.ToString(r_dtExtractDate), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then Return result
		
		' Execute SQL Statement
		If m_oDatabase.SQLSelect(sSQL:=ksSPExportExtractTransSQL, sSQLName:=ksSPExportExtractTransName, bStoredProcedure:=ksSPExportExtractTransStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then Return result
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		
		Return result
		
	End Function
	'#End Region
End Class
