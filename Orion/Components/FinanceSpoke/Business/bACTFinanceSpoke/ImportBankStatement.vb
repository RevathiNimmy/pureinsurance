Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
'Developer Guide no.129
Imports SharedFiles
Friend NotInheritable Class ImportBankStatement 
	'====================================================================
	'   Class/Module: ImportBankStatement
	'   Description : Class implementation of use case:
	'Import for InterfaceCode: "BANK_STMT"'
	'
	'====================================================================
	'   Maintenance History
	'
	'    31 January 2003    Paul Cunnigham    Created.
	'
	'====================================================================
	
	
	
	' Constant for the functions to identify which class this is.
	Private Const ACClass As String = "ImportBankStatement"
	
	'#Region " Private fields "
	Private m_lReturn As gPMConstants.PMEReturnCode
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
	'enum representing columns in Header array
	'sw 14-04-2003 Removed MediaTypeCode
	Private Enum ACHeaderArray
		BatchIDSIR = 0
		SourceCode = 10
		TotalDebitTransactions = 13
		TotalDebitamount = 14
		TotalCreditTransactions = 15
		TotalCreditamount = 16
		ClosingBalance = 17
		ClosingBalancePositive = 18
	End Enum
	
	'enum representing columns in Detail array
	'sw 14-04-2003 Added MediaTypeCode
	Private Enum ACDetailArray
		RowCount = 0
		HubReturnCode
		Message
		BankStatementDate
		DebitAmount
		CreditAmount
		Particulars
		TransactionType
		TransactionCode
		Code
		Reference
		DishonourReason
		OtherPartyName
		MediaTypeCode
	End Enum
	
    'developer guide no. 39
    Private Const ACBSAddBankStatementHeaderSQL As String = "spu_ACT_Add_Bank_Statement_Header"
	Private Const ACBSAddBankStatementHeaderName As String = "AddBankStatementHeader"
	
    'developer guide no. 39
    Private Const ACBSAddBankStatementDetailSQL As String = "spu_ACT_Add_Bank_Statement_Detail"
	Private Const ACBSAddBankStatementDetailName As String = "AddBankStatementDetail"
	
    'developer guide no. 39
    Private Const ACBSGetBatchInfoSQL As String = "spu_ACT_Get_Batch_Info"
	Private Const ACBSGetBatchInfoName As String = "GetBatchInfo"
	
	
	
	'enum representing columns in result array for SP: ?
	'Private Enum AC...
	'    ? = 0
	'End Enum
	'#End Region
	
	'#Region " Stored Procedures "
	'Private Const ksSPGetAutoBankingTransactionSQL = "{call spu_ACT_Spoke_ExportAutoBank}"
	'Private Const ksSPGetAutoBankingTransactionName = "GetAutoBankingTransactions"
	'Private Const ksSPGetAutoBankingTransactionStored = True
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
	Friend Function Start(ByRef r_sInterfaceCode As String, ByRef r_sBatchRef As String, ByRef r_sStatusCode As String, ByRef r_sMessage As String, ByRef r_sHeaderXML As String, ByRef r_vHeaderData() As Object, ByRef r_vDetailData() As Object) As Integer
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: Start
		' PURPOSE: Starting routine for use case
		' AUTHOR: Paul Cunnigham/Steve Watton
		' DATE: 31 January 2003, 10:05:03
		' RETURNS: PMTrue for success
		' CHANGES:
		' ---------------------------------------------------------------------------
		
		Dim result As Integer = 0
		Dim vHeaderArrayValues, vDetailArrayValues As Object
		
		Dim oBAReconcile As Object
		
		Dim bTransactionStarted As Boolean
		
		Dim lLowerRowId, lUpperRowId As Integer
		
		Dim cTotal, cDRTotal, cCRTotal As Decimal
		Dim lCount As Integer
		
		Dim lBatchID, lBatchStatusID, lBatchTypeID, lBatchSourceID As Integer
		Dim cTotalAmount As Decimal
		Dim lTotalTransactions, lBankStatementHeaderID, lBatchBankAccountID As Integer
		Dim cClosingBalance As Decimal
		Dim lBankStatementDetailID, lReverseReasonID, lMediaTypeID As Integer
		
		Const ACMethod As String = "Start"
		
		
		Try
		
		result = gPMConstants.PMEReturnCode.PMFalse
		
		'Return codes
		r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_FAILED
		r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_FAILED
		
		bTransactionStarted = False
		
		'Validate the input arrays
		If Not Information.IsArray(r_vHeaderData) Then
			Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Incoming HeaderData is not an array")
		End If
		
		If Not Information.IsArray(r_vDetailData) Then
			Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Incoming DetailData is not an array")
		End If
		
		If r_vHeaderData.GetUpperBound(0) <> conValue Then
			Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Incoming HeaderData Array upper bound is " &  _
			                           "not = " & CStr(conValue))
		End If
		
		'We need valid database and business objects
		If (m_oBusiness Is Nothing) Or (m_oDatabase Is Nothing) Then
			Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Invalid Business or General object")
		End If
		
		'OK do the Import processing...
		
		'Detail data is a 'jagged' array so we need to extract the the values
		' ---------------------------------------------------------------------------
		'See the "Header / Detail Array Format" in the declarations section of the
		'module gHUBSpokeConstants for an explanation of the format of the arrays
		' ---------------------------------------------------------------------------


		vDetailArrayValues = r_vDetailData(conValue)
		
		'Loop through the detail array and total the credits, debits and number of records

		lLowerRowId = vDetailArrayValues.GetLowerBound(conRows - 1)

		lUpperRowId = vDetailArrayValues.GetUpperBound(conRows - 1)
		lCount = lUpperRowId - lLowerRowId + 1 'Total number of rows
		
		For lRow As Integer = lLowerRowId To lUpperRowId

            cDRTotal += CDbl(vDetailArrayValues(ACDetailArray.DebitAmount, lRow))

            cCRTotal += CDbl(vDetailArrayValues(ACDetailArray.CreditAmount, lRow))
        Next lRow

        'Header data is a 'jagged' array so we need to extract the the values
        ' ---------------------------------------------------------------------------
        'See the "Header / Detail Array Format" in the declarations section of the
        'module gHUBSpokeConstants for an explanation of the format of the arrays
        ' ---------------------------------------------------------------------------


        vHeaderArrayValues = r_vHeaderData(conValue)

        'Ensure that the totals balance




        cTotal = (CDbl(vHeaderArrayValues(ACHeaderArray.TotalDebitamount))) + CDec(vHeaderArrayValues(ACHeaderArray.TotalCreditamount)) - (cDRTotal + cCRTotal) + CDbl(vHeaderArrayValues(ACHeaderArray.TotalDebitTransactions)) + CDec(vHeaderArrayValues(ACHeaderArray.TotalCreditTransactions)) - lCount


        If cTotal <> 0 Then
            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_IMPORT_FAILED
            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_CHECK_TOTALS_MISMATCH
		Return result
        End If

        'Begin a transaction
        If m_oDatabase.SQLBeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Failed to begin transaction")
        End If
        bTransactionStarted = True

        'look up various Id's from there code
        If m_oBusiness.GetIDValueFromCode("BatchStatus", False, "BI", lBatchStatusID) <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Failed to get batch status ID from code")
        End If

        If m_oBusiness.GetIDValueFromCode("Batch_Type", False, "BSMT", lBatchTypeID) <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Failed to get batch type ID from code")
        End If

        'sw 14-04-2003 MediaTypeCode now held against each record
        ''    If m_oBusiness.GetIDValueFromCode("mediatype", False, CStr(vHeaderArrayValues(ACHeaderArray.MediaTypeCode)), lMediaTypeID) <> PMTrue Then
        ''        Err.Raise vbObjectError, ACMethod, "Failed to get batch type ID from code"
        ''    End If
        ''


        If GetBatchSourceInfo(CStr(vHeaderArrayValues(ACHeaderArray.SourceCode)), lBatchSourceID, lBatchBankAccountID) <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Failed to get batch source ID and bank " & _
                                       "account from code")
        End If



        cTotalAmount = (CDbl(vHeaderArrayValues(ACHeaderArray.TotalDebitamount)) + CDbl(vHeaderArrayValues(ACHeaderArray.TotalCreditamount)))


        lTotalTransactions = (CDbl(vHeaderArrayValues(ACHeaderArray.TotalDebitTransactions)) + CDbl(vHeaderArrayValues(ACHeaderArray.TotalCreditTransactions)))

        'create the batch record
        If m_oBusiness.CreateBatchRecord(r_lBatchID:=lBatchID, v_lBatchStatusID:=lBatchStatusID, v_lCompanyID:=m_iSourceID, v_lUserID:=m_iUserID, v_sBatchRef:=r_sBatchRef, v_dtCreatedDate:=DateTime.Now, v_sComment:="", v_lBatchTypeID:=lBatchTypeID, v_lBatchSourceID:=lBatchSourceID, v_sXML:=r_sHeaderXML, v_cTotalAmount:=cTotalAmount, v_lTotalTransactions:=lTotalTransactions, v_dtImportedDate:=DateTime.Now, v_sInterfaceCode:=r_sInterfaceCode) <> gPMConstants.PMEReturnCode.PMTrue Then

            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Failed to create the new batch record")
        End If

        '21/05/2003 SW update the header data with the Sirius Batch ID, this needs to be returned to the Hub

        r_vHeaderData(conValue)(ACHeaderArray.BatchIDSIR) = lBatchID

        'calculate the closing balance


        cClosingBalance = CDec(vHeaderArrayValues(ACHeaderArray.ClosingBalance)) * (IIf(CStr(vHeaderArrayValues(ACHeaderArray.ClosingBalancePositive)) = "N", -1, 1))

        'create the bank statement header record
        If AddBankStatementHeader(r_lBankStatementHeaderID:=lBankStatementHeaderID, v_lBatchID:=lBatchID, v_lBankAccountID:=lBatchBankAccountID, v_cClosingBalance:=cClosingBalance, v_dtCreatedDate:=DateTime.Now, v_lPMuserID:=m_iUserID) <> gPMConstants.PMEReturnCode.PMTrue Then

            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Failed to create the bank statement header " & _
                                       "record")

        End If

        'now create the bank statement detail records
        For lRow As Integer = lLowerRowId To lUpperRowId

            'reset return values
            lBankStatementDetailID = 0
            lReverseReasonID = 0
            lMediaTypeID = 0


            If CStr(vDetailArrayValues(ACDetailArray.MediaTypeCode, lRow)).Trim() <> "" Then
                'sw 14-04-2003 Get the mediatypeid from the code for the current record

                If m_oBusiness.GetIDValueFromCode("mediatype", False, CStr(vDetailArrayValues(ACDetailArray.MediaTypeCode, lRow)), lMediaTypeID) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Failed to get media type ID from code")
                End If
            End If


            If CStr(vDetailArrayValues(ACDetailArray.DishonourReason, lRow)) <> "" Then
                'DD 18/09/2003: Handle Dishonour Reason - If DD then we are dealing with
                'Direct Debits on a Bank Statement

                If CStr(vDetailArrayValues(ACDetailArray.TransactionType, lRow)) = "DD" Then

                    If m_oBusiness.GetIDValueFromCode("pfinstalments_result", False, CStr(vDetailArrayValues(ACDetailArray.DishonourReason, lRow)), lReverseReasonID) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Failed to get pfinstalments_result ID from code")
                    End If
                Else

                    If m_oBusiness.GetIDValueFromCode("cashlistitem_reverse_reason", False, CStr(vDetailArrayValues(ACDetailArray.DishonourReason, lRow)), lReverseReasonID) <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Failed to get cashlistitem_reverse_reason ID from code")
                    End If
                End If
            End If










            If AddBankStatementDetail(r_lBankStatementDetailID:=lBankStatementDetailID, v_lBankStatementHeaderID:=lBankStatementHeaderID, v_lMediaTypeID:=lMediaTypeID, v_dtStatementDate:=CDate(vDetailArrayValues(ACDetailArray.BankStatementDate, lRow)), v_cDR:=CDec(vDetailArrayValues(ACDetailArray.DebitAmount, lRow)), v_cCR:=CDec(vDetailArrayValues(ACDetailArray.CreditAmount, lRow)), v_sParticulars:=CStr(vDetailArrayValues(ACDetailArray.Particulars, lRow)), v_sBankTransactionType:=CStr(vDetailArrayValues(ACDetailArray.TransactionType, lRow)), v_sBankTransactionCode:=CStr(vDetailArrayValues(ACDetailArray.TransactionCode, lRow)), v_sCode:=CStr(vDetailArrayValues(ACDetailArray.Code, lRow)), v_sReference:=CStr(vDetailArrayValues(ACDetailArray.Reference, lRow)), v_lDishonourReasonID:=lReverseReasonID, v_sOtherPartyName:=CStr(vDetailArrayValues(ACDetailArray.OtherPartyName, lRow)), v_lPMuserID:=m_iUserID) <> gPMConstants.PMEReturnCode.PMTrue Then



                r_vDetailData(conValue)(ACDetailArray.HubReturnCode, lRow) = gHUBSpokeConstants.k_STATUS_SIRIUS_RECORD_TRANSACTION_COMMIT_FAILED

                r_vDetailData(conValue)(ACDetailArray.Message, lRow) = gHUBSpokeConstants.k_MESSAGE_SIRIUS_RECORD_TRANSACTION_COMMIT_FAILED

                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Failed to create one off the bank statement " & _
                                           "detail records")

            End If


            r_vDetailData(conValue)(ACDetailArray.HubReturnCode, lRow) = gHUBSpokeConstants.k_STATUS_SIRIUS_RECORD_TRANSACTION_COMMIT_SUCCESS

            r_vDetailData(conValue)(ACDetailArray.Message, lRow) = gHUBSpokeConstants.k_MESSAGE_SIRIUS_RECORD_TRANSACTION_COMMIT_SUCCESS

        Next lRow

        'now Auto Reconcile the batch, have to come back to this when the business object has been finished

        'Get Instance of Bank Account reconcile Business object
        m_lReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=oBAReconcile, v_sClassName:="bACTBankAccountReconcile.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Failed to create bank account reconcile " & _
                                       "business object")
        End If

        'reconcile the bank account

        m_lReturn = oBAReconcile.ReconcileBankAccount(v_vBankAccountID:=lBatchBankAccountID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Failed to reconcile the bank account for " & _
                                       "this batch")
        End If


            oBAReconcile.Dispose()

            oBAReconcile = Nothing

        'Commit transaction if started
        If bTransactionStarted Then
            'Commit the transaction
            If m_oDatabase.SQLCommitTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Failed to commit transaction")
            End If

            bTransactionStarted = False
        End If

        'Return codes
        r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_IMPORT_COMPLETE
        r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_IMPORT_COMPLETE



        result = gPMConstants.PMEReturnCode.PMTrue


        '----------------------------------------------------------------------------------------
        'Only for Debugging, the code will never execute this line
        '----------------------------------------------------------------------------------------
         

		Catch ex As Exception
        Select Case Information.Err().Number
            Case Constants.vbObjectError
                ' Log internal failure.
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Information.Err().Source, excep:=ex)

                result = gPMConstants.PMEReturnCode.PMFalse


            Case Else
                ' Log Error.
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                result = gPMConstants.PMEReturnCode.PMError


        End Select

		Finally
        'Rollback any uncommitted transactions
        If bTransactionStarted Then
            bTransactionStarted = False

            If m_oDatabase.SQLRollbackTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Failed to rollback transaction")
            End If
        End If

         
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
    '#End Region


    ' ***************************************************************** '
    ' Name: AddBankStatementHeader (Private)
    '
    ' Description: add a bank statement header record
    '
    ' ***************************************************************** '
    Private Function AddBankStatementHeader(ByRef r_lBankStatementHeaderID As Integer, ByVal v_lBatchID As Integer, ByVal v_lBankAccountID As Integer, ByVal v_cClosingBalance As Decimal, ByVal v_dtCreatedDate As Date, ByVal v_lPMuserID As Integer, Optional ByVal v_vReconciledDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add bankstatementheaderid as an output param
            If m_oDatabase.Parameters.Add(sName:="bank_statement_header_id", vValue:=CStr(r_lBankStatementHeaderID), idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add batchid as an input param
            If m_oDatabase.Parameters.Add(sName:="batch_id", vValue:=CStr(v_lBatchID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add bankaccountid as an input param
            If m_oDatabase.Parameters.Add(sName:="bankaccount_id", vValue:=CStr(v_lBankAccountID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add closingbalance as an input param
            If m_oDatabase.Parameters.Add(sName:="closing_balance", vValue:=CStr(v_cClosingBalance), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add createddate as an input param
            If m_oDatabase.Parameters.Add(sName:="created_date", vValue:=DateTimeHelper.ToString(v_dtCreatedDate), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add pmuserid as an input param
            If m_oDatabase.Parameters.Add(sName:="pmuser_id", vValue:=CStr(v_lPMuserID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


            If Information.IsNothing(v_vReconciledDate) Then
                ' Add reconcilleddate as an input param

                If m_oDatabase.Parameters.Add(sName:="reconciled_date", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then Return result
            Else
                ' Add reconcilleddate as an input param

                If m_oDatabase.Parameters.Add(sName:="reconciled_date", vValue:=DateTimeHelper.ToString(CDate(v_vReconciledDate)), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then Return result
            End If

            ' Execute SQL Statement
            If m_oDatabase.SQLAction(sSQL:=ACBSAddBankStatementHeaderSQL, sSQLName:=ACBSAddBankStatementHeaderName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            r_lBankStatementHeaderID = gPMFunctions.NullToLong(m_oDatabase.Parameters.Item("bank_statement_header_id").Value)


            Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    ' ***************************************************************** '
    ' Name: AddBankStatementDetail (Private)
    '
    ' Description: add a bank statement Detail record
    '
    ' ***************************************************************** '
    Private Function AddBankStatementDetail(ByRef r_lBankStatementDetailID As Integer, ByVal v_lBankStatementHeaderID As Integer, ByVal v_lMediaTypeID As Integer, ByVal v_dtStatementDate As Date, ByVal v_cDR As Decimal, ByVal v_cCR As Decimal, ByVal v_sParticulars As String, ByVal v_sBankTransactionType As String, ByVal v_sBankTransactionCode As String, ByVal v_sCode As String, ByVal v_sReference As String, ByVal v_lDishonourReasonID As Integer, ByVal v_sOtherPartyName As String, ByVal v_lPMuserID As Integer) As Integer

        Dim result As Integer = 0
        

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add bank_statement_detail_id as an output param
            If m_oDatabase.Parameters.Add(sName:="bank_statement_detail_id", vValue:=CStr(r_lBankStatementDetailID), idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


            ' Add bank_statement_header_id as an input param
            If m_oDatabase.Parameters.Add(sName:="bank_statement_header_id", vValue:=CStr(v_lBankStatementHeaderID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add bankaccount_rules_id as an input param

            If m_oDatabase.Parameters.Add(sName:="bankaccount_rules_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add mediatype_id as an input param

            If m_oDatabase.Parameters.Add(sName:="mediatype_id", vValue:=IIf(v_lMediaTypeID = 0, DBNull.Value, CStr(v_lMediaTypeID)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add statement_date as an input param
            If m_oDatabase.Parameters.Add(sName:="statement_date", vValue:=DateTimeHelper.ToString(v_dtStatementDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add dr as an input param
            If m_oDatabase.Parameters.Add(sName:="dr", vValue:=CStr(v_cDR), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add cr as an input param
            If m_oDatabase.Parameters.Add(sName:="cr", vValue:=CStr(v_cCR), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add particulars as an input param

            'Developer Guide no.185
            'Start
            If m_oDatabase.Parameters.Add(sName:="particulars", vValue:=IIf(v_sParticulars = "", DBNull.Value, v_sParticulars), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add bank_transaction_type as an input param

            If m_oDatabase.Parameters.Add(sName:="bank_transaction_type", vValue:=IIf(v_sBankTransactionType = "", DBNull.Value, v_sBankTransactionType), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add bank_transaction_type as an input param

            If m_oDatabase.Parameters.Add(sName:="bank_transaction_code", vValue:=IIf(v_sBankTransactionCode = "", DBNull.Value, v_sBankTransactionCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add code as an input param

            If m_oDatabase.Parameters.Add(sName:="code", vValue:=IIf(v_sCode = "", DBNull.Value, v_sCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add reference as an input param

            If m_oDatabase.Parameters.Add(sName:="reference", vValue:=IIf(v_sReference = "", DBNull.Value, v_sReference), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add pmuserid as an input param

            If m_oDatabase.Parameters.Add(sName:="dishonour_reason_id", vValue:=IIf(v_lDishonourReasonID = 0, DBNull.Value, CStr(v_lDishonourReasonID)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add other_party_name as an input param

            If m_oDatabase.Parameters.Add(sName:="other_party_name", vValue:=IIf(v_sOtherPartyName = "", DBNull.Value, v_sOtherPartyName), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add pmuserid as an input param
            If m_oDatabase.Parameters.Add(sName:="pmuser_id", vValue:=CStr(v_lPMuserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add match_date as an input param

            If m_oDatabase.Parameters.Add(sName:="match_date", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


            ' Add modified_date as an input param

            If m_oDatabase.Parameters.Add(sName:="modified_date", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then Return result
            'End the Modification
            ' Execute SQL Statement
            If m_oDatabase.SQLAction(sSQL:=ACBSAddBankStatementDetailSQL, sSQLName:=ACBSAddBankStatementDetailName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


            Return gPMConstants.PMEReturnCode.PMTrue

    End Function


    ' ***************************************************************** '
    ' Name: GetBathSourceInfo (Private)
    '
    ' Description: gets the batch source id and bankaccount from the batch source code
    '
    ' ***************************************************************** '
    Private Function GetBatchSourceInfo(ByVal v_sBatchSourceCode As String, ByRef r_lBatchSourceID As Integer, ByRef r_lBankAccountID As Integer) As Integer 

        Dim result As Integer = 0 
        Dim vResultArray(,) As Object 

        Const ACBatchSourceID As Integer = 0
        Const ACBankAccountID As Integer = 1

        

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add batchsourcecode as an input param
            If m_oDatabase.Parameters.Add(sName:="batchsourcecode", vValue:=v_sBatchSourceCode.Trim(), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


            ' Execute SQL Statement
            If m_oDatabase.SQLSelect(sSQL:=ACBSGetBatchInfoSQL, sSQLName:=ACBSGetBatchInfoName, bStoredProcedure:=True, vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If Information.IsArray(vResultArray) Then

                r_lBatchSourceID = CInt(vResultArray(ACBatchSourceID, 0))

                r_lBankAccountID = CInt(vResultArray(ACBankAccountID, 0))
            Else
                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

    End Function
End Class

