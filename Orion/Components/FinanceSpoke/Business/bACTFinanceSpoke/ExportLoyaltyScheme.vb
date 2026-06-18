Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
'Developer Guide no.129
Imports SharedFiles
Friend NotInheritable Class ExportLoyaltyScheme 
	'====================================================================
	'   Class/Module: ExportLoyaltyScheme
	'   Description : Class implementation of use case:
	'Export for InterfaceCode: "PartyLoyaltyScheme"'
	'
	'====================================================================
	'   Maintenance History
	'
	' RAW 21/11/2002 : PS005 : Created
	' RAW 28/11/2002 : PS005 : Added enum containing DetailArray column subscripts
	' RAW 28/11/2002 : PS005 : Add an extra level to the Header and DetailArray
	'                          (0 contains column headers and 1 contains data values
	' RAW 29/11/2002 : PS005 : Add code to always set the batch return status and message
	'====================================================================
	
	
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "ExportLoyaltyScheme"
	
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
		BackdatedDays = 9
		LoyaltySchemeCode = 10
		PartyTypeCode = 11
		'SMJB 10/10/03 Added
		LoyaltyNotificationDays = 12
	End Enum
	
	'Columns in DetailData array
	'SW 14-04-2003 removed transcode, scheme and anualamount from Enum, no longer required
	Private Enum DetailDataCols
		' columns 0-2 reserved for use by the HUB - see gHUBSpokeConstants
		TransactionDate = 3
		CoverDate = 4
		MembershipNumber = 5
		OtherRef = 6
		InsuranceRef = 7
		Product = 8
		Amount = 9
		PartyType = 10
		Branch = 11
	End Enum
	'#End Region
	
	'#Region " Stored Procedures "
    'developer guide no. 39
    Private Const ACSelExportPartyLoyaltySchemeSQL As String = "spu_ACT_Spoke_ExportPartyLoyaltyScheme"
	Private Const ACSelExportPartyLoyaltySchemeName As String = "GetExportPartyLoyaltyScheme"
	Private Const ACSelExportPartyLoyaltySchemeStored As Boolean = True
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
		' PROCEDURE NAME: Export
		' PURPOSE: Start process for use case
		' AUTHOR: Rob Wilson
		' DATE: 21 November 2002
		' RETURNS: PMTrue for success
		' CHANGES:
		' ---------------------------------------------------------------------------
		
		Dim result As Integer = 0
		Dim lBackdatedDays As Integer
		Dim sLoyaltySchemeCode, sPartyTypeCode As String
        Dim vResultArray(,) As Object ' RAW 28/11/2002 : PS005 : Added
		Dim vHeaderArrayValues As Object ' RAW 28/11/2002 : PS005 : Added
		Dim lLoyaltyNotificationDays As Integer ' SMJB 10/10/03 Added
		
		
		Try
		
		result = gPMConstants.PMEReturnCode.PMFalse
		
		'Return codes
		r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_FAILED ' RAW 29/11/2002 : PS005 :
		r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_FAILED ' RAW 29/11/2002 : PS005 :
		
		'We need valid database and business objects
		If (m_oBusiness Is Nothing) Or (m_oDatabase Is Nothing) Then Return result
		
		'OK do the Export processing...
		
		' RAW 28/11/2002 : PS005 : Validate the structure of incoming arrays
		If Not Information.IsArray(r_vHeaderData) Then
			bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Incoming HeaderData is not an array", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
			Return result
		End If
		
		If Not Information.IsArray(r_vDetailData) Then
			bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Incoming DetailData is not an array", vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
			Return result
		End If
		
		If r_vHeaderData.GetUpperBound(0) <> conValue Then
			bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Incoming HeaderData Array upper bound is not = " & conValue, vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
			Return result
		End If
		' RAW 28/11/2002 : PS005 : End
		
		'Get the parameters from the header array
		' RAW 28/11/2002 : PS005 : Extract element 1 from incoming HeaderArray then
		' extract the data values from that instead of from the HeaderArray directly


		vHeaderArrayValues = r_vHeaderData(conValue)

        lBackdatedDays = CInt(vHeaderArrayValues(HeaderDataCols.BackdatedDays))

        sLoyaltySchemeCode = CStr(vHeaderArrayValues(HeaderDataCols.LoyaltySchemeCode))

        sPartyTypeCode = CStr(vHeaderArrayValues(HeaderDataCols.PartyTypeCode))

        lLoyaltyNotificationDays = CInt(vHeaderArrayValues(HeaderDataCols.LoyaltyNotificationDays))
		
		' RAW 28/11/2002 : PS005 : End
		
		'Get the extract transactions from the database based on the passed criteria
		' RAW 28/11/2002 : PS005 : Pass an empty ResultArray to be populated by the function
		If GetPartyLoyaltyScheme(v_lBackdatedDays:=lBackdatedDays, v_sLoyaltySchemeCode:=sLoyaltySchemeCode, v_sPartyTypeCode:=sPartyTypeCode, v_lLoyaltyNotificationDays:=lLoyaltyNotificationDays, r_vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then Return result
		
		' If we have reached here then the batch - and all the records - has been successful
		
		' The following approach only work when either accepting or rejecting
		' the whole batch in its entirity.
		' It will not handle individual record rejections.
		
		'Now add the extra columns that the HUB requires
		'(indicates the status of each record)
		' RAW 29/11/2002 : PS005 : moved from GetPartyLoyaltyScheme

		If AddHUBColumnsToDetailArray(v_sStatusCode:=gHUBSpokeConstants.k_STATUS_SIRIUS_RECORD_EXPORT_SUCCESS, v_sStatusMsg:=gHUBSpokeConstants.k_MESSAGE_SIRIUS_RECORD_EXPORT_SUCCESS, r_vResultArray:=vResultArray, v_sUsername:=m_sUsername) <> gPMConstants.PMEReturnCode.PMTrue Then Return result
		
		' RAW 28/11/2002 : PS005 : Add result array as an extra element of the DetailArray
		' Note - When the complete batch has been rejected then we will not have reached here
		' so no records will be returned


		If AddResultArrayToDetailArray(v_vDetailArray:=r_vDetailData, r_vResultArray:=vResultArray, v_sUsername:=m_sUsername) Then
			
		End If
		
		'Return codes
		r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_COMPLETE ' RAW 29/11/2002 : PS005 :
		r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_COMPLETE ' RAW 29/11/2002 : PS005 :
		
		If Information.IsArray(vResultArray) Then
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
				
				result = gPMConstants.PMEReturnCode.PMFalse
				
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
		' AUTHOR: Rob Wilson
		' DATE: 21 November 2002
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
	
	' RAW 29/11/2002 : PS005 : removed parameters r_sStatusCode, r_sMessage
	Private Function GetPartyLoyaltyScheme(ByVal v_lBackdatedDays As Integer, ByVal v_sLoyaltySchemeCode As String, ByVal v_sPartyTypeCode As String, ByVal v_lLoyaltyNotificationDays As Integer, ByRef r_vResultArray(,) As Object) As Integer 
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: GetPartyLoyaltyScheme
		' PURPOSE:
		' AUTHOR: Sirius Financial Systems Plc
		' DATE: 26 November 2002, 15:24:27
		' RETURNS: PMTrue for success
		' CHANGES: SMJB 10/10/03 Added v_lLoyaltyNotificationDays parameter, not optional as
		' it should be provided
		' ---------------------------------------------------------------------------
		
		Dim result As Integer = 0 
		Dim lSqlReturnNo As Integer 
		Dim sStatusCode, sStatusMsg As String 
		
		
		
		result = gPMConstants.PMEReturnCode.PMFalse
		
		' Clear the Database Parameters Collection
		m_oDatabase.Parameters.Clear()
		
		' Add param for return value
		If m_oDatabase.Parameters.Add(sName:="return_value", vValue:=CStr(0), idirection:=gPMConstants.PMEParameterDirection.PMParamReturnValue, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result
		
		' Add mediatype_code as an input param
		If m_oDatabase.Parameters.Add(sName:="v_iBackdatedDays", vValue:=CStr(v_lBackdatedDays), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result
		
		' Add branch_code as an input param
		If m_oDatabase.Parameters.Add(sName:="v_sLoyaltySchemeCode", vValue:=v_sLoyaltySchemeCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result
		
		' Add extract_date as an input param
		If m_oDatabase.Parameters.Add(sName:="v_sPartyTypeCode", vValue:=v_sPartyTypeCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result
		
		' SMJB 10/10/30: Add backdated loyalty days as an input param
		If m_oDatabase.Parameters.Add(sName:="v_iLoyaltyNotificationDays", vValue:=CStr(v_lLoyaltyNotificationDays), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result
		
		' Execute SQL Statement
		If m_oDatabase.SQLSelect(sSQL:=ACSelExportPartyLoyaltySchemeSQL, sSQLName:=ACSelExportPartyLoyaltySchemeName, bStoredProcedure:=ACSelExportPartyLoyaltySchemeStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then Return result
		
		' Check if sql returned an error status
		lSqlReturnNo = m_oDatabase.Parameters.Item("return_value").Value
		If lSqlReturnNo <> 0 Then
			Return result
		End If
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
        Return result
		
	End Function
	
	
	
	'#End Region
End Class
