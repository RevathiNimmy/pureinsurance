Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'Developer Guide no.129
Imports SharedFiles
Friend NotInheritable Class Export3rdPartyCollect 
	'====================================================================
	'   Class/Module: Export3rdPartyCollect
	'   Description : Class implementation of use case:
	'Export for InterfaceCode: "3RDPARTYCOLLECT"'
	'
	'====================================================================
	'   Maintenance History
	'
	'    11 November 2002    Paul Cunnigham    Created.
	'
	'====================================================================
	
	
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "Export3rdPartyCollect"
	
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
		ExportDate = 9
		MediaTypeCode = 10
		BankNameShort = 11
	End Enum
	'#End Region
	
	'#Region " Stored Procedures "
    'developer guide no. 39
    Private Const ksSPExport3rdPartyTransSQL As String = "spu_ACT_Spoke_Export3rdPartyTrans"
	Private Const ksSPExport3rdPartyTransName As String = "GetExport3rdPartyTrans "
	Private Const ksSPExport3rdPartyTransStored As Boolean = True
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
	Friend Function Start(ByRef r_sStatusCode As String, ByRef r_sMessage As String, ByRef r_vHeaderData() As Object) As Integer
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: Start
		' PURPOSE: Start process for use case
		' AUTHOR: Paul Cunnigham
		' DATE: 11 November 2002, 11:45:03
		' RETURNS: PMTrue for success
		' CHANGES:
		' ---------------------------------------------------------------------------
		
		Dim result As Integer = 0
		Dim vResultArray, vCreditDetails, vDebitDetails As Object
		Dim sMediaTypeCode, sBankNameShort As String
		Dim dtExportDate As Date
		
		Dim vHeaderArrayValues As Object
		
		
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

        sMediaTypeCode = CStr(vHeaderArrayValues(HeaderDataCols.MediaTypeCode)).Trim()

        sBankNameShort = CStr(vHeaderArrayValues(HeaderDataCols.BankNameShort)).Trim()

        dtExportDate = CDate(vHeaderArrayValues(HeaderDataCols.ExportDate))
		
		'Get the extract transactions from the database based on the passed criteria
		If Get3rdPartyTrans(r_sMediaTypeCode:=sMediaTypeCode, r_sBankShortName:=sBankNameShort, r_dtExportDate:=dtExportDate, r_vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then Return result
		
		'Process any rows returned
		If Information.IsArray(vResultArray) Then
			'Seperate the credits from the debits and load into arrays



			If LoadPostingArrays(r_vResultArray:=vResultArray, r_vCreditAccount:=vCreditDetails, r_vDebitAccount:=vDebitDetails) <> gPMConstants.PMEReturnCode.PMTrue Then Return result
			
			'Begin a transaction
			If m_oDatabase.SQLBeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then Return result
			
			'Post the items in the credit / debit arrays
			If m_oBusiness.PostTransaction(v_vCreditAccount:=vCreditDetails, v_vDebitAccount:=vDebitDetails, v_sComment:=Nothing, v_sDocumentRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeJn) = gPMConstants.PMEReturnCode.PMTrue Then
				
				'Commit the transaction
				If m_oDatabase.SQLCommitTrans() <> gPMConstants.PMEReturnCode.PMTrue Then Return result
				
			Else
				'Rollback the transaction
				If m_oDatabase.SQLRollbackTrans() <> gPMConstants.PMEReturnCode.PMTrue Then Return result
				
			End If
			
			result = gPMConstants.PMEReturnCode.PMTrue
		Else
			result = gPMConstants.PMEReturnCode.PMNotFound
		End If
		
		'Return codes
		r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_COMPLETE
		r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_COMPLETE
		
		
		
		'----------------------------------------------------------------------------------------
		'Only for Debugging, the code will never execute this line
		'----------------------------------------------------------------------------------------
		  
		
		Catch ex As Exception
		Select Case Information.Err().Number
			Case Else
				' Log Error.
				bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
				
				result = gPMConstants.PMEReturnCode.PMError
				
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
	
	Private Function Get3rdPartyTrans(ByRef r_sMediaTypeCode As String, ByRef r_sBankShortName As String, ByRef r_dtExportDate As Date, ByRef r_vResultArray(,) As Object) As Integer 
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: Get3rdPartyTrans
		' PURPOSE: Gets the receipts that need to be transferred from corportate partner accounts
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
		
		' Add bank_short_code as an input param
		If m_oDatabase.Parameters.Add(sName:="bank_short_code", vValue:=r_sBankShortName, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result
		
		' Add mediatype_code as an input param
		If m_oDatabase.Parameters.Add(sName:="export_date", vValue:=DateTimeHelper.ToString(r_dtExportDate), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then Return result
		
		' Execute SQL Statement
		If m_oDatabase.SQLSelect(sSQL:=ksSPExport3rdPartyTransSQL, sSQLName:=ksSPExport3rdPartyTransName, bStoredProcedure:=ksSPExport3rdPartyTransStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then Return result
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
        Return result
		
	End Function
	
	Private Function LoadPostingArrays(ByRef r_vResultArray( ,  ) As Object, ByRef r_vCreditAccount( ,  ) As Object, ByRef r_vDebitAccount( ,  ) As Object) As Integer
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: LoadPostingArrays
		' PURPOSE: Populate the credit and debit arrays with results from the sp call
		' AUTHOR: Paul Cunnigham
		' DATE: 11 November 2002, 13:02:37
		' RETURNS: PMTrue for success
		' CHANGES:
		' ---------------------------------------------------------------------------
		
		Dim result As Integer = 0
		'Used to loop through result array
		Dim lLowerRow As Integer 'Lower element of result array
		Dim lUpperRow As Integer 'Upper element of result array
		Const kRowDimension As Integer = 2 'Inidicates the row dimension of the array
		
		Dim lUpperCreditDebit As Integer 'Upper element of credit array
		
		'constants representing columns in result array
		Const kColCreditBankAccount As Integer = 0
		Const kColDebitBankAccount As Integer = 1
		Const kColTotalAmount As Integer = 2
		
		'constants representing columns in credit / debit arrays
		Const kColAccount As Integer = 0
		Const kColAmount As Integer = 1
		
		
		
		result = gPMConstants.PMEReturnCode.PMFalse
		
		'Initialise upper limits of array
		lUpperCreditDebit = -1
		
		'Get the lower and upper limits of the dimension of the array that stores the row data
		lLowerRow = r_vResultArray.GetLowerBound(kRowDimension - 1)
		lUpperRow = r_vResultArray.GetUpperBound(kRowDimension - 1)
		
		'Loop through the result array and allocate the total as a debit and a credit
		For lRow As Integer = lLowerRow To lUpperRow
			'Increase the array...
			lUpperCreditDebit += 1
			If lUpperCreditDebit = 0 Then
				r_vCreditAccount = Array.CreateInstance(GetType(Object), New Integer(){kColAmount - kColAccount + 1, lUpperCreditDebit + 1}, New Integer(){kColAccount, 0})
				r_vDebitAccount = Array.CreateInstance(GetType(Object), New Integer(){kColAmount - kColAccount + 1, lUpperCreditDebit + 1}, New Integer(){kColAccount, 0})
			Else
				r_vCreditAccount = ArraysHelper.RedimPreserve(Of Object(, ))(r_vCreditAccount, New Integer(){kColAmount - kColAccount + 1, lUpperCreditDebit + 1}, New Integer(){kColAccount, 0})
				r_vDebitAccount = ArraysHelper.RedimPreserve(Of Object(, ))(r_vDebitAccount, New Integer(){kColAmount - kColAccount + 1, lUpperCreditDebit + 1}, New Integer(){kColAccount, 0})
			End If
			
			'...and populate


			r_vCreditAccount(kColAccount, lUpperCreditDebit) = r_vResultArray(kColCreditBankAccount, lRow)


			r_vCreditAccount(kColAmount, lUpperCreditDebit) = r_vResultArray(kColTotalAmount, lRow)
			


			r_vDebitAccount(kColAccount, lUpperCreditDebit) = r_vResultArray(kColDebitBankAccount, lRow)


			r_vDebitAccount(kColAmount, lUpperCreditDebit) = r_vResultArray(kColTotalAmount, lRow)
			
		Next lRow
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		
		Return result
		
	End Function
	
	'#End Region
End Class
