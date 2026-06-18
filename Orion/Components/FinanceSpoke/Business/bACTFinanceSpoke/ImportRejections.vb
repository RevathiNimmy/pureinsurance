Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
'Developer Guide no.129
Imports SharedFiles
Friend NotInheritable Class ImportRejections

    ' RAW 12/03/2003 : ISS2893 : ReverseAllocation parameters referenced by name


    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_oBusiness As Business
    Private m_oDatabase As dPMDAO.Database

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

    Const ACClass As String = "ImportRejections"

    'developer guide no. 39
    Private Const ACUpdateCLIForReversalSQL As String = "spu_ACT_Update_Reverse_CashListItem"
    Private Const ACUpdateCLIForReversalName As String = "UpdateCLIForReversal"

    'developer guide no. 39
    Private Const ACAddBatchRejectionSQL As String = "spu_ACT_Add_Batch_Rejection"
    Private Const ACAddBatchRejectionName As String = "AddBatchRejection"

    'developer guide no. 39
    Private Const ACUpdateBatchRecordSQL As String = "spu_ACT_Update_Batch_For_Rejection"
    Private Const ACUpdateBatchRecordName As String = "UpdateBatchRecord"

    'developer guide no. 39
    Private Const ACGetInstalmentCollectionDetailsSQL As String = "spu_ACT_Get_Instalment_Collection_Details"
    Private Const ACGetInstalmentCollectionDetailsName As String = "GetInstalmentDetails"

    'developer guide no. 39
    Private Const ACUpdateInstalmentDueDateSQL As String = "spu_ACT_Update_Instalment_Due_Date"
    Private Const ACUpdateInstalmentDueDateName As String = "UpdateInstalmentDueDate"

    'developer guide no. 39
    Private Const ACMarkInstalmentFailedSQL As String = "spu_ACT_Update_Instalment_Failure_Reason"
    Private Const ACMarkInstalmentFailedName As String = "UpdateInstalmentFailureReason"

    'developer guide no. 39
    Private Const ACGetCashListItemSQL As String = "spu_ACT_Select_CashListItem"
    Private Const ACGetCashListItemName As String = "GetCashListItem"

    'developer guide no. 39
    Private Const ACGetCashListItemReciptTypeCodeSQL As String = "spu_ACT_Get_CashListItem_ReceiptType"
    Private Const ACGetCashListItemReciptTypeCodeName As String = "GetCashListItemReceipttypeCode"

    'developer guide no. 39
    Private Const ACGetTransDetailsForCashListItemSQL As String = "spu_ACT_Get_TransDetailIDs_For_CashListItem"
    Private Const ACGetTransDetailsForCashListItemName As String = "GetTransDetailsIDsForCashListItem"

    'developer guide no. 39
    Private Const ACUpdateInstalmentStatusSQL As String = "spu_PFPremiumFinance_UpdateStatus_By_TransDetail_ID"
    Private Const ACUpdateInstalmentStatusName As String = "UpdateInstalmentStatus"

    '#End Region
    '====================================================================
    '   Class/Module: ImportRejections

    '
    '====================================================================
    '   Maintenance History
    '
    'sw 09/01/2003 - Created


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


    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: Start
    ' PURPOSE: Process Rejection Batch passed from Business Class
    ' AUTHOR: Steve Watton
    ' DATE: 16/01/2003
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    Public Function Start(ByRef r_vDetailData() As Object, ByRef r_vHeaderData() As Object, ByRef r_sStatusCode As String, ByRef r_sMessage As String, ByRef r_sBatchInterfaceCode As String, ByRef r_lBatchID As Integer, ByRef r_sInterfaceCode As String, ByRef r_sBatchRef As String) As Integer

        Dim result As Integer = 0
        Dim vNewDebitsAccountIDsAndAmount, vNewCreditAccountIDsAndAmount, vOriginalReceiptTransDetails, vNewDebitTransDetails, vDTDSaved, vCTDSaved, vORTASaved As Object
        Dim oAllocationManual As bACTAllocationManual.Business
        Dim lCollectionAccountID As Integer
        Dim dAmount As Double
        Dim sComment As String = ""
        Dim vKeys As Object
        Dim cCurrencyAmount, cCurCheckAmount, cTotal As Decimal
        Dim vMatchTrans As Object

        'SMJB 14/10/03
        Dim vAssociatedTransDetailIDs As Object
        Dim bNoDetailData As Boolean

        Const conCurrencyAmount As Integer = 1

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            'SMJB CQ2866 16/10/03: It is valid to have no detail data as long as the TotalNumberOfRejectTrans
            'And TotalRejectAmount in the header array are both 0.  In this case we are just trying to close
            'the batch with no rejections

            If Not Information.IsArray(r_vDetailData(0)) Then
                bNoDetailData = True 'We have no detail data
            End If

            'SMJB 20/08/03: CQ 2144 Check the totals match
            If Not Information.IsArray(r_vHeaderData) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_IMPORT_FAILED
                r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_IMPORT_FAILED
                Return result
            End If

            'If we have no detail data then the number of transactions must be zero
            If bNoDetailData Then

                If CDbl(r_vHeaderData(conValue)(ACIRTotalNoOfTransactions)) <> 0 Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_IMPORT_FAILED
                    r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_CHECK_TOTALS_MISMATCH
                    Return result
                End If
            Else
                'Check the number of transactions against the header XML


                If r_vDetailData(conValue).GetUpperBound(ACIRHeaderAmount - 1) <> CDbl(CDbl(r_vHeaderData(conValue)(ACIRTotalNoOfTransactions)) - 1) Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_IMPORT_FAILED
                    r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_CHECK_TOTALS_MISMATCH
                    Return result
                End If
            End If

            'Get the contro total

            cCurCheckAmount = CDec(r_vHeaderData(conValue)(ACIRControlTotal))

            'If we have no detail data then this must be zero
            If bNoDetailData Then
                If cCurCheckAmount <> 0 Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_IMPORT_FAILED
                    r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_CHECK_TOTALS_MISMATCH
                    Return result
                End If
            Else
                'We have detail data so sum the transaction totals in each detail record, and check they match

                For iLoop As Integer = 0 To r_vDetailData(conValue).GetUpperBound(1)

                    cTotal += CDec(r_vDetailData(conValue)(ACIRDetailAmount, iLoop))
                Next

                'If they don't match exit now
                If cTotal <> cCurCheckAmount Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_IMPORT_FAILED
                    r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_CHECK_TOTALS_MISMATCH
                    Return result
                End If
            End If
            'SMJB End

            If m_oDatabase.SQLBeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_IMPORT_FAILED
                r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_IMPORT_FAILED
                Return result
            End If

            If Not bNoDetailData Then

                For iLoop As Integer = 0 To r_vDetailData(conValue).GetUpperBound(conRows - 1)

                    If m_oDatabase.SQLBeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_IMPORT_FAILED
                        r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_IMPORT_FAILED
                        Return result
                    End If

                    'sw 01/04/2003, save the credit and debit transaction details prior to processing current record, also
                    'save OriginalReceipts for later allocation


                    vDTDSaved = vNewDebitsAccountIDsAndAmount


                    vCTDSaved = vNewCreditAccountIDsAndAmount


                    vORTASaved = vOriginalReceiptTransDetails




                    m_lReturn = ProcessBatchRecord(iLoop, r_vDetailData, r_sStatusCode, r_sMessage, r_sBatchInterfaceCode, r_lBatchID, r_sInterfaceCode, r_sBatchRef, vNewCreditAccountIDsAndAmount, vNewDebitsAccountIDsAndAmount, vOriginalReceiptTransDetails, r_vAssociatedTransDetailIDs:=vAssociatedTransDetailIDs)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        'Set Return Status Code and Message for Record Import for Traceability

                        r_vDetailData(conValue)(iddRecordStatus, iLoop) = gHUBSpokeConstants.k_STATUS_SIRIUS_RECORD_TRANSACTION_COMMIT_FAILED

                        r_vDetailData(conValue)(iddRecordMessage, iLoop) = gHUBSpokeConstants.k_MESSAGE_SIRIUS_RECORD_TRANSACTION_COMMIT_FAILED

                        If m_oDatabase.SQLRollbackTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_IMPORT_FAILED
                            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_IMPORT_FAILED
                            Return gPMConstants.PMEReturnCode.PMError
                        End If

                        'revert the transaction details back to their states prior to processing current record


                        vNewDebitsAccountIDsAndAmount = vDTDSaved


                        vNewCreditAccountIDsAndAmount = vCTDSaved


                        vOriginalReceiptTransDetails = vORTASaved

                    Else
                        If m_oDatabase.SQLCommitTrans() = gPMConstants.PMEReturnCode.PMTrue Then
                            'Set Return Status Code and Message for Record Import for Traceability

                            dAmount += CDbl(r_vDetailData(conValue)(iddAmount, iLoop))


                            r_vDetailData(conValue)(iddRecordStatus, iLoop) = gHUBSpokeConstants.k_STATUS_SIRIUS_RECORD_TRANSACTION_COMMIT_SUCCESS

                            r_vDetailData(conValue)(iddRecordMessage, iLoop) = gHUBSpokeConstants.k_MESSAGE_SIRIUS_RECORD_TRANSACTION_COMMIT_SUCCESS
                        Else
                            If m_oDatabase.SQLRollbackTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                                r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_IMPORT_FAILED
                                r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_IMPORT_FAILED
                                Return gPMConstants.PMEReturnCode.PMError
                            Else

                                r_vDetailData(conValue)(iddRecordStatus, iLoop) = gHUBSpokeConstants.k_STATUS_SIRIUS_RECORD_TRANSACTION_COMMIT_FAILED

                                r_vDetailData(conValue)(iddRecordMessage, iLoop) = gHUBSpokeConstants.k_MESSAGE_SIRIUS_RECORD_TRANSACTION_COMMIT_FAILED
                            End If
                        End If
                    End If

                Next iLoop

                If r_sBatchInterfaceCode.ToUpper() = gHUBSpokeConstants.ksICOneOff Then
                    ' Need to post the reject values to the accounts.

                    If dAmount = 0 Then
                        m_oDatabase.SQLRollbackTrans()
                        r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_IMPORT_FAILED
                        r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_IMPORT_FAILED
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    sComment = r_sInterfaceCode & r_sBatchRef


                    ReDim vNewDebitTransDetails(vOriginalReceiptTransDetails.GetUpperBound(0))

                    'SMJB 14/10/03 Changed Document type to IDR instead of default ICR

                    m_lReturn = CType(m_oBusiness.PostTransaction(v_vCreditAccount:=vNewCreditAccountIDsAndAmount, v_vDebitAccount:=vNewDebitsAccountIDsAndAmount, v_sComment:=sComment, r_vNewDebitTransDetailId:=vNewDebitTransDetails, v_sDocumentRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeRvj), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_oDatabase.SQLRollbackTrans()
                        r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_IMPORT_FAILED
                        r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_IMPORT_FAILED
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'we have 3 arrays
                    'vNewDebitTransDetails = List of TransDetailIDs for the new Debit transactions
                    'vOriginalReceiptTransDetails = List of original receipt transdetails
                    'vNewDebitsAccountIDsAndAmount = multi dimension array containing account_id's and amounts

                    'check arrays contain equal no of rows



                    If vNewDebitTransDetails.GetUpperBound(0) <> vOriginalReceiptTransDetails.GetUpperBound(0) Then
                        m_oDatabase.SQLRollbackTrans()
                        r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_IMPORT_FAILED
                        r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_IMPORT_FAILED
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If



                    If vNewDebitTransDetails.GetUpperBound(0) <> vNewDebitsAccountIDsAndAmount.GetUpperBound(1) Then
                        m_oDatabase.SQLRollbackTrans()
                        r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_IMPORT_FAILED
                        r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_IMPORT_FAILED
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'OK, so all arrays have equal number of rows, containing related transactions

                    'create the allocation manual business object required for processing

                    oAllocationManual = New bACTAllocationManual.Business
                    m_lReturn = oAllocationManual.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                    'check the return code
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_oDatabase.SQLRollbackTrans()
                        r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_IMPORT_FAILED
                        r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_IMPORT_FAILED
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'now allocate the original receipts against the balancing debit transactions

                    For lRow As Integer = vNewDebitTransDetails.GetLowerBound(0) To vNewDebitTransDetails.GetUpperBound(0)

                        'get the currency amount

                        cCurrencyAmount = CDec(vNewDebitsAccountIDsAndAmount(conAmount, lRow))

                        ReDim vKeys(1, 2)
                        ' AccountID

                        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID


                        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = CInt(vNewDebitsAccountIDsAndAmount(conAccountId, lRow))

                        ' transdetailID | Amount of debiting trans

                        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameTransDetailID


                        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = CStr(vNewDebitTransDetails(lRow)) & "|" & CStr(cCurrencyAmount)

                        'If r_vAssociatedTransDetailIDs is an array then we have a list of TransDetails to
                        'allocate against, so add them all to the array
                        If Information.IsArray(vAssociatedTransDetailIDs) Then

                            ReDim vMatchTrans(vAssociatedTransDetailIDs.GetUpperBound(1))

                            For lCount As Integer = 0 To vMatchTrans.GetUpperBound(0)
                                vMatchTrans(lCount) = CStr(vAssociatedTransDetailIDs(0, lCount)) & "|" & _
                                                    CDbl(vAssociatedTransDetailIDs(1, lCount)) * -1

                                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameTransDetailIDs


                                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = vMatchTrans
                            Next
                        Else

                            ' CashListItemID
                            ReDim vMatchTrans(0)

                            vMatchTrans(0) = CStr(vOriginalReceiptTransDetails(lRow)) & "|" & (CStr(cCurrencyAmount * -1))

                            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameTransDetailIDs

                            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = vMatchTrans
                        End If

                        m_lReturn = oAllocationManual.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

                        ' Set the keys

                        m_lReturn = oAllocationManual.SetKeys(vKeyArray:=vKeys)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_oDatabase.SQLRollbackTrans()
                            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_IMPORT_FAILED
                            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_IMPORT_FAILED
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        'oAllocationManual.CompanyId = m_iSourceID


                        m_lReturn = oAllocationManual.Start()

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            m_oDatabase.SQLRollbackTrans()
                            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_IMPORT_FAILED
                            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_IMPORT_FAILED
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Next

                End If


                m_lReturn = CType(UpdateBatchRecord(DateTime.Today, dAmount, r_vDetailData(conValue).GetUpperBound(conRows - 1) + 1, r_lBatchID), gPMConstants.PMEReturnCode)
            Else
                'No records so update Batch accordingly
                m_lReturn = CType(UpdateBatchRecord(DateTime.Today, 0, 0, r_lBatchID), gPMConstants.PMEReturnCode)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                m_oDatabase.SQLRollbackTrans()

                r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_IMPORT_FAILED
                r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_IMPORT_FAILED
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            result = gPMConstants.PMEReturnCode.PMTrue
            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_IMPORT_COMPLETE
            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_IMPORT_COMPLETE

            m_oDatabase.SQLCommitTrans()

            Return result



            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                    Return gPMConstants.PMEReturnCode.PMError

            End Select


            Return result
        End Try
    End Function


    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: ProcessBatchRecord
    ' PURPOSE: Process's the current record
    ' AUTHOR: Steve Watton
    ' DATE: 16/01/2003
    ' CHANGES: 14/10/03 SMJB: Wasn't handling instalment transactions correctly, put logic in to check
    ' receipt type, and if it's an instalment transaction then reverse alloate each ICR using the TransDetail ID
    ' ---------------------------------------------------------------------------

    Private Function ProcessBatchRecord(ByVal iLoop As Integer, ByRef r_vDetailData() As Object, ByRef r_sStatusCode As String, ByRef r_sMessage As String, ByRef r_sBatchInterfaceCode As String, ByRef r_lBatchID As Integer, ByRef r_sInterfaceCode As String, ByRef r_sBatchRef As String, ByRef r_vNewCreditAccountIDsAndAmount(,) As Object, ByRef r_vNewDebitsAccountIDsAndAmount(,) As Object, ByRef r_vOriginalReceiptTransDetails() As Object, ByRef r_vAssociatedTransDetailIDs As Object) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim oAllocationPost As bACTAllocationPost.Automated
        Dim iPKIDElement, iCreditElement, iDebitElement, iMediaElement As Integer
        Dim vInstalmentDetails(,) As Object
        Dim lCLIReverseReasonId, lPFInstalmentsResultsId As Integer
        Dim vIDField As Object
        Dim iRowsReturned As Integer
        Dim lBatchStatusID, lCreditAccountID As Integer
        Dim bFound As Boolean
        Dim lNewUbound, lOriginalCreditTransDetail As Integer
        Dim cOriginalCreditAmount As Decimal

        'DD 12/08/2003
        Dim sCCValue As String = ""
        Dim oCreditControl As bACTCreditControlItem.Business
        Dim sFailureDescription As String = ""

        'SMJB 14/10/03
        Dim lCashListItemReceiptType As Integer
        Dim sCashListItemReceiptTypeCode As String = ""
        Dim vTransDetailIDs(,) As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        iPKIDElement = 0
        lCLIReverseReasonId = 0

        '****************************************************************************************************************************************
        ' Retrieve current One Off / Recurring Transaction By Criteria Contained in r_vDetailData
        '****************************************************************************************************************************************


        m_lReturn = m_oBusiness.FindImportRecord(v_sInterfaceCode:=r_sBatchInterfaceCode, r_vIDField:=vIDField, r_iRowsReturned:=iRowsReturned, v_vDetailData:=r_vDetailData(conValue), v_vRow:=iLoop)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        '****************************************************************************************************************************************
        ' END OF Retrieve current One Off / Recurring Transaction By Criteria Contained in r_vDetailData
        '****************************************************************************************************************************************


        Select Case r_sBatchInterfaceCode.ToUpper()
            '****************************************************************************************************************************************
            ' Cash List Item (One Off) Operations for REJECTIONS
            '****************************************************************************************************************************************
            Case gHUBSpokeConstants.ksICOneOff

                '****************************************************************************************************************************************
                ' Get Cash List Item Reverse Reason Id for supplied code
                '****************************************************************************************************************************************

                m_lReturn = CType(m_oBusiness.GetIDValueFromCode(v_sTableName:="CashListItem_Reverse_Reason", v_bGettingCode:=False, r_sCode:=CStr(r_vDetailData(conValue)(iddFailureCode, iLoop)), r_lID:=lCLIReverseReasonId), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                '****************************************************************************************************************************************
                ' END OF Get Cash List Item Reverse Reason Id for supplied code
                '****************************************************************************************************************************************

                'SW made some changes here 01/04/2003.

                '###### NOTE this differs from the tech spec dated prior to            ##################
                '######      01/04/2003 see Danny Davis or Steve Watton for details.   ##################

                'We need to work out the the [collection* (See below)] account of the cash drawer if that does not apply
                'then the account_ID of the cashlist bankaccount
                '*SMJB CQ1738/1739/2256 Changed to Suspense account

                For iLoopCol As Integer = 0 To vIDField.GetUpperBound(conRows - 1)

                    If CStr(vIDField(conIDFieldName, iLoopCol)).ToUpper() = clnSuspenseAccountId Then
                        Dim auxVar As Object = vIDField(conIDFieldValue, iLoopCol)


                        If Not (Convert.IsDBNull(auxVar) Or IsNothing(auxVar)) Then
                            'we have a cashdrawer transaction

                            lCreditAccountID = CInt(vIDField(conIDFieldValue, iLoopCol))
                            Exit For
                        Else
                            'we have a non cash drawer transaction, so pick up the account ID from the bankaccount

                            For iLoopCol2 As Integer = 0 To vIDField.GetUpperBound(conRows - 1)

                                If CStr(vIDField(conIDFieldName, iLoopCol2)).ToUpper() = clnAccountID Then

                                    lCreditAccountID = CInt(vIDField(conIDFieldValue, iLoopCol2))
                                    Exit For
                                End If
                            Next iLoopCol2
                            Exit For
                        End If
                    End If
                Next iLoopCol

                'now we need to decifer whether the Credit Account ID is the same as one previously
                'added to the vCreditTransactionDetails array. If it is then add the amount to this record,
                'if not then add a new record to the array

                If Not Information.IsArray(r_vNewCreditAccountIDsAndAmount) Then
                    'declare the array of credit trans to contain one record, if further records are to be added then
                    'the array will be redimensioned dynamically
                    r_vNewCreditAccountIDsAndAmount = Array.CreateInstance(GetType(Object), New Integer() {conAmount - conAccountId + 1, 1}, New Integer() {conAccountId, 0})


                    r_vNewCreditAccountIDsAndAmount(conAccountId, 0) = lCreditAccountID


                    r_vNewCreditAccountIDsAndAmount(conAmount, 0) = CDec(r_vDetailData(conValue)(iddAmount, iLoop))
                Else

                    bFound = False

                    'loop through the existing credit account details
                    For iLoopCol As Integer = 0 To r_vNewCreditAccountIDsAndAmount.GetUpperBound(1)

                        If CInt(r_vNewCreditAccountIDsAndAmount(conAccountId, iLoopCol)) = lCreditAccountID Then
                            ' the account has already been added to the array so total the amounts



                            r_vNewCreditAccountIDsAndAmount(conAmount, iLoopCol) = CDec(r_vNewCreditAccountIDsAndAmount(conAmount, iLoopCol)) + CDec(r_vDetailData(conValue)(iddAmount, iLoop))
                            bFound = True
                            Exit For
                        End If
                    Next

                    If Not bFound Then
                        'we need to add another row to the array
                        lNewUbound = r_vNewCreditAccountIDsAndAmount.GetUpperBound(1) + 1

                        r_vNewCreditAccountIDsAndAmount = ArraysHelper.RedimPreserve(Of Object(,))(r_vNewCreditAccountIDsAndAmount, New Integer() {conAmount - conAccountId + 1, lNewUbound + 1}, New Integer() {conAccountId, 0})

                        'assign the current records details to the new row

                        r_vNewCreditAccountIDsAndAmount(conAccountId, lNewUbound) = lCreditAccountID


                        r_vNewCreditAccountIDsAndAmount(conAmount, lNewUbound) = CDec(r_vDetailData(conValue)(iddAmount, iLoop))
                    End If

                End If

                'store the transdetail_id of the original Credit for later allocation to the balancing
                'debit transactions

                'first find the transdetail id

                For iLoopCol As Integer = 0 To vIDField.GetUpperBound(conRows - 1)

                    If CStr(vIDField(conIDFieldName, iLoopCol)).ToUpper() = clnTransDetailID Then

                        lOriginalCreditTransDetail = CInt(vIDField(conIDFieldValue, iLoopCol))
                        Exit For
                    End If
                Next iLoopCol

                If Not Information.IsArray(r_vOriginalReceiptTransDetails) Then
                    'declare the array of credit trans to contain one record, if further records are to be added then
                    'the array will be redimensioned dynamically
                    ReDim r_vOriginalReceiptTransDetails(0)

                    r_vOriginalReceiptTransDetails(0) = lOriginalCreditTransDetail
                Else
                    lNewUbound = r_vOriginalReceiptTransDetails.GetUpperBound(0) + 1
                    ReDim Preserve r_vOriginalReceiptTransDetails(lNewUbound)

                    r_vOriginalReceiptTransDetails(lNewUbound) = lOriginalCreditTransDetail
                End If

                'SW 01/04/2003 locate the debtor account ID to be debited

                For iLoopCol As Integer = 0 To vIDField.GetUpperBound(conRows - 1)

                    If CStr(vIDField(conIDFieldName, iLoopCol)).ToUpper() = clnClientAccountId Then
                        iPKIDElement = iLoopCol
                        Exit For
                    End If
                Next iLoopCol

                'Added sw 01/04/2003
                If Not Information.IsArray(r_vNewDebitsAccountIDsAndAmount) Then
                    'declare the array of debit trans to contain one record, if further records are to be added then
                    'the array will be redimensioned dynamically
                    r_vNewDebitsAccountIDsAndAmount = Array.CreateInstance(GetType(Object), New Integer() {conAmount - conAccountId + 1, 1}, New Integer() {conAccountId, 0})


                    r_vNewDebitsAccountIDsAndAmount(conAccountId, 0) = CInt(vIDField(conIDFieldValue, iPKIDElement))

                    'now get the amount

                    For iLoopCol As Integer = 0 To vIDField.GetUpperBound(conRows - 1)

                        If CStr(vIDField(conIDFieldName, iLoopCol)).ToUpper() = clnAmount Then
                            iPKIDElement = iLoopCol
                            Exit For
                        End If
                    Next iLoopCol



                    r_vNewDebitsAccountIDsAndAmount(conAmount, 0) = CDec(vIDField(conIDFieldValue, iPKIDElement))
                Else
                    lNewUbound = r_vNewDebitsAccountIDsAndAmount.GetUpperBound(1) + 1
                    r_vNewDebitsAccountIDsAndAmount = ArraysHelper.RedimPreserve(Of Object(,))(r_vNewDebitsAccountIDsAndAmount, New Integer() {conAmount - conAccountId + 1, lNewUbound + 1}, New Integer() {conAccountId, 0})


                    r_vNewDebitsAccountIDsAndAmount(conAccountId, lNewUbound) = CInt(vIDField(conIDFieldValue, iPKIDElement))

                    'now get the amount

                    For iLoopCol As Integer = 0 To vIDField.GetUpperBound(conRows - 1)

                        If CStr(vIDField(conIDFieldName, iLoopCol)).ToUpper() = clnAmount Then
                            iPKIDElement = iLoopCol
                            Exit For
                        End If
                    Next iLoopCol



                    r_vNewDebitsAccountIDsAndAmount(conAmount, lNewUbound) = CDec(vIDField(conIDFieldValue, iPKIDElement))

                End If
                'end sw 01/04/2003


                '****************************************************************************************************************************************
                ' UPDATE cash list item reverse reason id and PM User id
                '****************************************************************************************************************************************


                For iLoopCol As Integer = 0 To vIDField.GetUpperBound(conRows - 1)

                    If CStr(vIDField(conIDFieldName, iLoopCol)).ToUpper() = clnCashListItemID Then
                        iPKIDElement = iLoopCol
                        Exit For
                    End If
                Next iLoopCol


                m_lReturn = CType(UpdateCLIForReversal(lCLIReverseReasonId, m_iUserID, CInt(vIDField(conIDFieldValue, iPKIDElement))), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                '****************************************************************************************************************************************
                ' END OF UPDATE cash list item reverse reason id and PM User id
                '****************************************************************************************************************************************

                '****************************************************************************************************************************************
                ' INSERT Record into Batch Rejection
                '****************************************************************************************************************************************



                m_lReturn = CType(AddBatchRejection(r_lBatchID, lCLIReverseReasonId, DateTime.Today, m_iUserID, CInt(vIDField(conIDFieldValue, iPKIDElement)), 0, CDec(r_vDetailData(conValue)(iddAmount, iLoop)), 0), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                '****************************************************************************************************************************************
                ' END OF INSERT Record into Batch Rejection
                '****************************************************************************************************************************************

                '****************************************************************************************************************************************
                ' Reverse allocate the original debt
                '****************************************************************************************************************************************


                'create the allocation post business object required for processing

                oAllocationPost = New bACTAllocationPost.Automated
                m_lReturn = oAllocationPost.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                'check the return code
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If



                m_lReturn = CType(GetCashListItemReceiptType(v_lCashListItemID:=CInt(vIDField(conIDFieldValue, iPKIDElement)), r_lCashListItemReceipttype:=lCashListItemReceiptType), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = CType(GetCashListItemReceiptCodeFromType(v_lCashListItemReceiptType:=lCashListItemReceiptType, r_sCashListItemReceiptTypeCode:=sCashListItemReceiptTypeCode), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                If sCashListItemReceiptTypeCode.ToUpper() = "INST" Then
                    'We are on an instalment receipt so we must reverse each transdetail associated with the cash list item

                    m_lReturn = CType(GetTransDetailIDsForCashListItem(v_lCashListItemID:=CInt(vIDField(conIDFieldValue, iPKIDElement)), r_vTransDetailsArray:=vTransDetailIDs), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Return the Transdetail array for the calling function


                    r_vAssociatedTransDetailIDs = vTransDetailIDs


                    For lCount As Integer = 0 To vTransDetailIDs.GetUpperBound(1)


                        m_lReturn = oAllocationPost.ReverseAllocation(v_lTransDetailID:=vTransDetailIDs(0, lCount))

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        'Update the status to Failed


                        m_lReturn = CType(UpdateInstalmentStatus(v_lTransDetailID:=CInt(vTransDetailIDs(0, lCount)), v_lStatusCode:="R", v_sResultCode:=CStr(r_vDetailData(conValue)(iddFailureCode, iLoop))), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    Next
                Else
                    ' RAW 12/03/2003 : ISS2893 : parameters referenced by name
                    ' DD 15/11/2003: Cannot pass Cash List Item as receipt may be manually
                    ' allocation and CashListItem_id is not stored. Use TransDetail instead

                    m_lReturn = oAllocationPost.ReverseAllocation(v_lTransDetailID:=lOriginalCreditTransDetail)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
                oAllocationPost = Nothing




                '****************************************************************************************************************************************
                ' END OF Reverse allocate the original debt
                '****************************************************************************************************************************************

                '****************************************************************************************************************************************
                ' END OF Cash List Item (One Off) Operations for REJECTIONS
                '****************************************************************************************************************************************

                '****************************************************************************************************************************************
                ' PFInstalments (Recurring) Operations for REJECTIONS
                '****************************************************************************************************************************************
            Case gHUBSpokeConstants.ksICRecurring


                '****************************************************************************************************************************************
                ' Get Instalment Reverse Reason Id for supplied code
                '****************************************************************************************************************************************

                m_lReturn = CType(m_oBusiness.GetIDValueFromCode(v_sTableName:="PFInstalments_Result", v_bGettingCode:=False, r_sCode:=CStr(r_vDetailData(conValue)(iddFailureCode, iLoop)), r_lID:=lPFInstalmentsResultsId), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'get the instalmentdetails required for processing


                If Conversion.Val(CStr(r_vDetailData(conValue)(iddTransactionId, iLoop))) > 0 Then


                    m_lReturn = CType(GetInstalmentDetails(CInt(Conversion.Val(CStr(r_vDetailData(conValue)(iddTransactionId, iLoop)))), 0, vInstalmentDetails), gPMConstants.PMEReturnCode)
                Else


                    m_lReturn = CType(GetInstalmentDetails(0, CInt(vIDField(conIDFieldValue, iLoop)), vInstalmentDetails), gPMConstants.PMEReturnCode)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Determine whether credit control is switched on
                m_lReturn = CType(bPMFunc.GetSystemOption(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, 4, sCCValue, 1), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If sCCValue = "1" Then

                    oCreditControl = New bACTCreditControlItem.Business
                    m_lReturn = oCreditControl.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                'SW 30/05/2003 The transaction ID actually applies to the group id which could be more than one instalment
                'so loop through the instalment records

                For lInsCount As Integer = 0 To vInstalmentDetails.GetUpperBound(1)

                    'mark the instalment as failed


                    m_lReturn = CType(MarkInstalmentFailed(CInt(vInstalmentDetails(ACInstalmentID, lInsCount)), CStr(r_vDetailData(conValue)(iddFailureCode, iLoop)), sFailureDescription), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'advance the instalment collection date

                    If CInt(vInstalmentDetails(ACRecollectOnNext, lInsCount)) = gPMConstants.PMEReturnCode.PMTrue Then
                        'recollect on next has been set
                        If Information.IsDate(vInstalmentDetails(ACNextDueDate, lInsCount)) Then
                            'next is available


                            m_lReturn = CType(UpdateInstalmentDueDate(CInt(vInstalmentDetails(ACInstalmentID, lInsCount)), CDate(vInstalmentDetails(ACNextDueDate, lInsCount))), gPMConstants.PMEReturnCode)
                        Else
                            'next not available so add on one week


                            m_lReturn = CType(UpdateInstalmentDueDate(CInt(vInstalmentDetails(ACInstalmentID, lInsCount)), DateAndTime.DateAdd("ww", 1, CDate(vInstalmentDetails(ACDueDate, lInsCount)))), gPMConstants.PMEReturnCode)
                        End If

                    Else
                        'recollect on next has not been set so check if recollect days has been set

                        If Conversion.Val(CStr(vInstalmentDetails(ACRecollectDays, lInsCount))) > 0 Then
                            'recollect days has been set



                            m_lReturn = CType(UpdateInstalmentDueDate(CInt(vInstalmentDetails(ACInstalmentID, lInsCount)), CDate(vInstalmentDetails(ACDueDate, lInsCount)).AddDays(CDbl(vInstalmentDetails(ACRecollectDays, lInsCount)))), gPMConstants.PMEReturnCode)
                        Else
                            'add on one week as neither recollect on next or recollect days has been set
                            'next not available so add on one week


                            m_lReturn = CType(UpdateInstalmentDueDate(CInt(vInstalmentDetails(ACInstalmentID, lInsCount)), DateAndTime.DateAdd("ww", 1, CDate(vInstalmentDetails(ACDueDate, lInsCount)))), gPMConstants.PMEReturnCode)
                        End If
                    End If

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'DD 12/08/2003: Add Credit Control Item for this instalment
                    If sCCValue = "1" Then



                        m_lReturn = oCreditControl.AddInstalment(v_lPFInstalmentsID:=CInt(vInstalmentDetails(ACInstalmentID, lInsCount)), v_sReason:=sFailureDescription, v_cAmount:=CDec(vInstalmentDetails(ACInstalmentAmount, lInsCount)))
                    End If


                    For iLoopCol As Integer = 0 To vIDField.GetUpperBound(conRows - 1)

                        If CStr(vIDField(conIDFieldName, iLoopCol)).ToUpper() = clnMediaTypeID Then
                            iMediaElement = iLoopCol
                        ElseIf CStr(vIDField(conIDFieldName, iLoopCol)).ToUpper() = clnCollectionAccountID Then
                            iDebitElement = iLoopCol
                        ElseIf CStr(vIDField(conIDFieldName, iLoopCol)).ToUpper() = clnAccountID Then
                            iCreditElement = iLoopCol
                        End If
                    Next iLoopCol



                    m_lReturn = CType(AddBatchRejection(r_lBatchID, 0, DateTime.Today, m_iUserID, 0, CInt(vInstalmentDetails(ACInstalmentID, lInsCount)), CDec(vInstalmentDetails(ACInstalmentAmount, lInsCount)), lPFInstalmentsResultsId), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Next

                'Shutdown credit control
                If sCCValue = "1" Then

                    oCreditControl.Dispose()
                    oCreditControl = Nothing
                End If
        End Select

        Return result

    End Function



    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: GetInstalmentDetails
    ' PURPOSE: gets instalment details
    ' AUTHOR: Steve Watton
    ' DATE: 16/01/2003
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    Private Function GetInstalmentDetails(ByVal v_lTransactionID As Integer, ByVal v_lPFInstalmentsID As Integer, ByRef v_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMFalse

        With m_oDatabase
            ' Clear the Database Parameters Collection
            .Parameters.Clear()

            ' Add transactionid as an input param
            If .Parameters.Add(sName:="transactionid", vValue:=CStr(v_lTransactionID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If .Parameters.Add(sName:="pfinstalments_id", vValue:=CStr(v_lPFInstalmentsID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Execute SQL Statement
            If .SQLSelect(sSQL:=ACGetInstalmentCollectionDetailsSQL, sSQLName:=ACGetInstalmentCollectionDetailsName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=v_vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        End With


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function


    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: UpdateInstalmentDueDate
    ' PURPOSE:Updates instalment record
    ' AUTHOR: Steve Watton
    ' DATE: 16/01/2003
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    Private Function UpdateInstalmentDueDate(ByVal v_lInstalmentID As Integer, ByVal v_dtNewDueDate As Date) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMFalse

        With m_oDatabase
            ' Clear the Database Parameters Collection
            .Parameters.Clear()

            ' Add due date as an input param
            If .Parameters.Add(sName:="DueDate", vValue:=DateTimeHelper.ToString(v_dtNewDueDate), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add instalmentid as an input param
            If .Parameters.Add(sName:="instalmentid", vValue:=CStr(v_lInstalmentID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Execute SQL Statement
            If .SQLAction(sSQL:=ACUpdateInstalmentDueDateSQL, sSQLName:=ACUpdateInstalmentDueDateName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        End With


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function


    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: MarkInstalmentFailed
    ' PURPOSE:Updates instalment record
    ' AUTHOR: Steve Watton
    ' DATE: 16/01/2003
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    Private Function MarkInstalmentFailed(ByVal v_lInstalmentID As Integer, ByVal v_sResultCode As String, ByRef r_sResultDescription As String) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMFalse

        With m_oDatabase
            ' Clear the Database Parameters Collection
            .Parameters.Clear()

            ' Add instalment id as an input param
            If .Parameters.Add(sName:="instalmentid", vValue:=CStr(v_lInstalmentID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add result code as an input param
            If .Parameters.Add(sName:="failurecode", vValue:=v_sResultCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add result code as an input param
            If .Parameters.Add(sName:="failuredescription", vValue:="", idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Execute SQL Statement
            If .SQLAction(sSQL:=ACMarkInstalmentFailedSQL, sSQLName:=ACMarkInstalmentFailedName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Get failure description
            r_sResultDescription = .Parameters.Item("failuredescription").Value
        End With


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function


    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: UpdateBatchRecord
    ' PURPOSE:Updates Batch Record with rejection details
    ' AUTHOR: Steve Watton
    ' DATE: 16/01/2003
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    Private Function UpdateBatchRecord(ByVal v_dtRejectImportedDate As Date, ByVal v_cRejectAmount As Decimal, ByVal v_lRejectTrans As Integer, ByVal v_lBatchID As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMFalse

        With m_oDatabase
            ' Clear the Database Parameters Collection
            .Parameters.Clear()

            ' Add rejectimportdate as an input param
            If .Parameters.Add(sName:="rejectimportdate", vValue:=DateTimeHelper.ToString(v_dtRejectImportedDate), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add rejectamount as an input param
            If .Parameters.Add(sName:="rejectamount", vValue:=CStr(v_cRejectAmount), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add rejecttransaction as an input param
            If .Parameters.Add(sName:="rejecttransaction", vValue:=CStr(v_lRejectTrans), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add batchid as an input param
            If .Parameters.Add(sName:="batchid", vValue:=CStr(v_lBatchID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Execute SQL Statement
            If .SQLAction(sSQL:=ACUpdateBatchRecordSQL, sSQLName:=ACUpdateBatchRecordName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        End With


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function


    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: UpdateCLIForReversal
    ' PURPOSE:Updates CashListItem with reverse reason and user id
    ' AUTHOR: Steve Watton
    ' DATE: 16/01/2003
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    Private Function UpdateCLIForReversal(ByVal v_lReverseReasonID As Integer, ByVal v_lReversePMUserID As Integer, ByVal v_lCashListItemID As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMFalse

        With m_oDatabase
            ' Clear the Database Parameters Collection
            .Parameters.Clear()

            ' Add reversereasonid as an input param
            If .Parameters.Add(sName:="reversereasonid", vValue:=CStr(v_lReverseReasonID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add reversepmuserid as an input param
            If .Parameters.Add(sName:="reversepmuserid", vValue:=CStr(v_lReversePMUserID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add cashlistitemid as an input param
            If .Parameters.Add(sName:="cashlistitemid", vValue:=CStr(v_lCashListItemID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Execute SQL Statement
            If .SQLAction(sSQL:=ACUpdateCLIForReversalSQL, sSQLName:=ACUpdateCLIForReversalName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        End With


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function



    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: AddBatchRejection
    ' PURPOSE:Adds as batch rejection record
    ' AUTHOR: Steve Watton
    ' DATE: 16/01/2003
    ' CHANGES:
    ' ---------------------------------------------------------------------------
    Private Function AddBatchRejection(ByVal v_lBatchID As Integer, ByVal v_lCashListItemReverseReasonID As Integer, ByVal v_dtRejectionDate As Date, ByVal v_lPMuserID As Integer, ByVal v_lCashListItemID As Integer, ByVal v_lPFInstalmentsID As Integer, ByVal v_cAmount As Decimal, ByVal v_lPFInstalmentsResultID As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMFalse

        With m_oDatabase
            ' Clear the Database Parameters Collection
            .Parameters.Clear()

            ' Add batchid as an input param
            If .Parameters.Add(sName:="batchid", vValue:=CStr(v_lBatchID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add cashlistitemreversereasonid as an input param

            'Developer Guide no.85
            If .Parameters.Add(sName:="cashlistitemreversereasonid", vValue:=IIf(v_lCashListItemReverseReasonID <> 0, CStr(v_lCashListItemReverseReasonID), DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add rejectiondate as an input param
            If .Parameters.Add(sName:="rejectiondate", vValue:=DateTimeHelper.ToString(v_dtRejectionDate), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add pmuserid as an input param
            If .Parameters.Add(sName:="pmuserid", vValue:=CStr(v_lPMuserID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add cashlistitemid as an input param

            'Developer Guide no.85
            If .Parameters.Add(sName:="cashlistitemid", vValue:=IIf(v_lCashListItemID <> 0, CStr(v_lCashListItemID), DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add pfinstalmenntsresultid as an input param

            'Developer Guide no.85
            If .Parameters.Add(sName:="pfinstalmentsresultid", vValue:=IIf(v_lPFInstalmentsResultID <> 0, CStr(v_lPFInstalmentsResultID), DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add pfinstalmentsid as an input param

            'Developer Guide no.85
            If .Parameters.Add(sName:="pfinstalmentsid", vValue:=IIf(v_lPFInstalmentsID <> 0, CStr(v_lPFInstalmentsID), DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            ' Add amount as an input param
            If .Parameters.Add(sName:="amount", vValue:=CStr(v_cAmount), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


            ' Execute SQL Statement
            If .SQLAction(sSQL:=ACAddBatchRejectionSQL, sSQLName:=ACAddBatchRejectionName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        End With


        Return gPMConstants.PMEReturnCode.PMTrue

    End Function



    ' ***************************************************************** '
    ' Name: GetCashListItemReceiptType
    '
    ' Description: Gets the receipt type for the cash list item.
    '
    ' Written By: Simon.Baynes
    '
    ' Returns: The numeric identifier from the cash list item table
    '
    ' Date: 14/10/2003
    '
    ' ***************************************************************** '
    Private Function GetCashListItemReceiptType(ByVal v_lCashListItemID As Integer, ByRef r_lCashListItemReceipttype As Integer) As Integer

        Dim result As Integer = 0
        Dim vResults(,) As Object
        Dim lCashListItemReceiptType As Integer
        Const ACCashListItemReceiptField As Integer = 30



        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="cashlistitem_id", vValue:=CStr(v_lCashListItemID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = .SQLSelect(sSQL:=ACGetCashListItemSQL, sSQLName:=ACGetCashListItemName, bStoredProcedure:=True, vResultArray:=vResults)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetCashListItemReceiptType failed", vApp:=ACApp, vClass:=ACClass, vMethod:=result)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End With

        If Not Information.IsArray(vResults) Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get cash list item", vApp:=ACApp, vClass:=ACClass, vMethod:=result)
            Return gPMConstants.PMEReturnCode.PMFalse
        Else

            lCashListItemReceiptType = CInt(vResults(ACCashListItemReceiptField, 0))
        End If

        r_lCashListItemReceipttype = lCashListItemReceiptType

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetCashListItemReceiptCodeFromType
    '
    ' Description: Gets the receipt code for the given identifier.
    '
    ' Written By: Simon.Baynes
    '
    ' Returns: The string code from the cashlistitem_receipt_type table
    '
    ' Date: 14/10/2003
    '
    ' ***************************************************************** '
    Private Function GetCashListItemReceiptCodeFromType(ByVal v_lCashListItemReceiptType As Integer, ByRef r_sCashListItemReceiptTypeCode As String) As Integer

        Dim result As Integer = 0
        Dim vResults(,) As Object
        Dim sCashListItemReceiptTypeCode As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="cashlistitem_receipt_type_id", vValue:=CStr(v_lCashListItemReceiptType), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = .SQLSelect(sSQL:=ACGetCashListItemReciptTypeCodeSQL, sSQLName:=ACGetCashListItemReciptTypeCodeName, bStoredProcedure:=True, vResultArray:=vResults)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetCashListItemReceiptCodeFromType failed", vApp:=ACApp, vClass:=ACClass, vMethod:=result)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End With
        If Not Information.IsArray(vResults) Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get cash list item receipt type code", vApp:=ACApp, vClass:=ACClass, vMethod:=result)
            Return gPMConstants.PMEReturnCode.PMFalse
        Else

            sCashListItemReceiptTypeCode = CStr(vResults(0, 0))
        End If

        r_sCashListItemReceiptTypeCode = sCashListItemReceiptTypeCode

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetTransDetailIDsForCashListItem
    '
    ' Description: Gets the TransDetail IDs associated with a cash list item
    '               used to pay off instalments.
    '
    ' Written By: Simon.Baynes
    '
    ' Returns: An array of TransDetailIDs
    '
    ' Date: 14/10/2003
    '
    ' ***************************************************************** '
    Private Function GetTransDetailIDsForCashListItem(ByVal v_lCashListItemID As Integer, ByRef r_vTransDetailsArray As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim vResults(,) As Object

        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="CashListItemID", vValue:=CStr(v_lCashListItemID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = .SQLSelect(sSQL:=ACGetTransDetailsForCashListItemSQL, sSQLName:=ACGetTransDetailsForCashListItemName, bStoredProcedure:=True, vResultArray:=vResults)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetTransDetailIDsForCashListItem failed", vApp:=ACApp, vClass:=ACClass, vMethod:=result)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End With

        If Not Information.IsArray(vResults) Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get TransDetail IDs for cash list item", vApp:=ACApp, vClass:=ACClass, vMethod:=result)
            Return gPMConstants.PMEReturnCode.PMFalse
        Else


            r_vTransDetailsArray = vResults
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateInstalmentStatus
    '
    ' Description: Updates the status of a PFInstalments record by TransDetail ID.
    '
    ' Written By: Simon.Baynes
    '
    ' Returns: PMTrue for success
    '
    ' Date: 16/10/2003
    '
    ' ***************************************************************** '
    Private Function UpdateInstalmentStatus(ByVal v_lTransDetailID As Integer, ByVal v_lStatusCode As String, Optional ByVal v_sResultCode As String = "") As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="TransDetailID", vValue:=CStr(v_lTransDetailID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .Parameters.Add(sName:="StatusCode", vValue:=v_lStatusCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = .Parameters.Add(sName:="ResultCode", vValue:=v_sResultCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = .SQLAction(sSQL:=ACUpdateInstalmentStatusSQL, sSQLName:=ACUpdateInstalmentStatusName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="UpdateInstalmentStatus failed", vApp:=ACApp, vClass:=ACClass, vMethod:=result)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End With


        Return result

    End Function
End Class
