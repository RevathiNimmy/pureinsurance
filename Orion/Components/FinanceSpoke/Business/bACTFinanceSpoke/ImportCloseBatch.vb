Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'Developer Guide no.129
Imports SharedFiles
Friend NotInheritable Class ImportCloseBatch
    '====================================================================
    '   Class/Module: ExportCloseBatch
    '   Description : Class implementation of use case:
    'Export for InterfaceCode: "CloseBatch"'
    '
    '====================================================================
    '   Maintenance History
    '
    '    28 November 2002    Paul Cunnigham    Created.
    '
    '====================================================================



    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "ImportCloseBatch"

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
        MediaTypeCode = 0
        BranchCode
        ExtractDate
    End Enum
    '#End Region

    '#Region " Stored Procedures "
    'developer guide no. 39
    Private Const ksSPExportCloseBatchSQL As String = "spu_ACT_Spoke_ExportExtractTrans"
    Private Const ksSPExportCloseBatchName As String = "GetExportExtractTrans"
    Private Const ksSPExportCloseBatchStored As Boolean = True

    'SMJB CQ 2134
    'developer guide no. 39
    Private Const ksSPSelectFinancePlanSQL As String = "spu_ACT_Select_Finance_Plan_From_Batch"
    Private Const ksSPSelectFinancePlanName As String = "SelectFinancePlan"
    Private Const ksSPSelectFinancePlanStored As Boolean = True

    'developer guide no. 39
    Private Const ksSPUpdateInstalmentSQL As String = "spu_ACT_Update_Instalment_Status"
    Private Const ksSPUpdateInstalmentName As String = "UpdateInstalmentStatus"
    Private Const ksSPUpdateInstalmentStored As Boolean = True

    Private Const ACInstalmentAccountID As Integer = 0
    Private Const ACInstalmentAmount As Integer = 1
    Private Const ACInstalmentID As Integer = 2
    Private Const ACInstalmentFinanceversion As Integer = 3
    Private Const ACInstalmentTransDetail As Integer = 4
    Private Const ACInstalmentBankAccountID As Integer = 5
    'SMJB End
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
    Friend Function Start(ByRef r_sInterfaceCode As String, ByRef r_sBatchRef As String, ByRef r_sStatusCode As String, ByRef r_sMessage As String) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: Start
        ' PURPOSE: Start process for use case
        ' AUTHOR: Steve Pollard
        ' DATE: 28 November 2002, 11:45:03
        ' RETURNS: PMTrue for success
        ' CHANGES: 29/11/2002 - PWC - Moved this routine from Import
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim sSQLSelectFields, sSQLUpdate, sBatchInterfaceCode, sComment As String
        Dim lBatchID, lBatchStatusID, iAutoClose As Integer
        'Developer Guide No 17
        Dim vRejectImportDate As Object
        Dim vBatchId As Object
        Dim vCreditAccount(,) As Object, vDebitAccount(,) As Object
        Dim sSQLInstalment As String = ""
        Dim vInstalmentArray(,) As Object
        Dim vInstalmentIDArray(,) As Object
        Dim oInstalments As Object
        Dim sSQLOneOff As String = ""
        Dim vOneOffArray(,) As Object
        Dim bTransStarted As Boolean
        Dim curTotal As Decimal
        Dim lBankAccountID As Integer

        Const cOneOffAmount As Byte = 0
        Const cOneOffMediaType As Byte = 1
        Const cOneOffSuspense As Byte = 2
        Const cOneOffBankAccount As Byte = 3

        Const ACInstalmentIDElement As Byte = 0
        Const conBatchId As Byte = 0
        Const conAutoClose As Byte = 1
        Const conRejectImportDate As Byte = 2
        Const conBatchInterfaceCode As Byte = 3


        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'Return codes
            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_FAILED
            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_FAILED

            'We need valid database and business objects
            If (m_oBusiness Is Nothing) Or (m_oDatabase Is Nothing) Then Return result

            'OK do the Export processing...

            '****************************************************************************************************************************************
            ' Get the Batch ID record for supplied batch ref and also store the transaction type stored in interface id
            ' FOR REJECTIONS AND CLOSE BATCH
            '****************************************************************************************************************************************

            sSQLSelectFields = "SELECT batch_id, auto_close, imported_date, interface_code"


            If m_oBusiness.GetBatchRecord(v_sBatchRef:=r_sBatchRef, r_vBatchResults:=vBatchId, v_sSelectFields:=sSQLSelectFields) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return result
            End If

            'Batch Id contained in first column of results array

            lBatchID = CInt(vBatchId(conBatchId, 0))
            'SMJB: Need to convert Nulls to 0 but as there's no NullToInt function
            'I'll just convert the long to an int (it will only ever be 0 or 1 anyway)

            iAutoClose = gPMFunctions.NullToLong(CStr(vBatchId(conAutoClose, 0)))
            If Information.IsDate(vBatchId(conRejectImportDate, 0)) Then

                vRejectImportDate = CStr(vBatchId(conRejectImportDate, 0))
            Else

                vRejectImportDate = Nothing
            End If

            sBatchInterfaceCode = CStr(vBatchId(conBatchInterfaceCode, 0))

            '****************************************************************************************************************************************
            ' Checks against Batch Record before proceeding
            '****************************************************************************************************************************************

            If (Convert.IsDBNull(vRejectImportDate) Or IsNothing(vRejectImportDate)) And iAutoClose = 0 Then
                Return result
            End If

            'One off or recurring???

            Select Case sBatchInterfaceCode.ToUpper()
                Case gHUBSpokeConstants.ksICOneOff
                    sSQLOneOff = "select sum(cli.amount) Amount, m.code, cd.suspense_account_id, ba.account_id"
                    sSQLOneOff = sSQLOneOff & " From"
                    sSQLOneOff = sSQLOneOff & " cashlistitem cli, cashlist cl, cashlist_drawer cd, bankaccount ba, mediatype m"
                    sSQLOneOff = sSQLOneOff & " Where"
                    sSQLOneOff = sSQLOneOff & " cli.mediatype_id = m.mediatype_id and"
                    sSQLOneOff = sSQLOneOff & " cli.cashlist_id = cl.cashlist_id and"
                    sSQLOneOff = sSQLOneOff & " cl.cashlist_drawer_id = cd.cashlist_drawer_id and"
                    sSQLOneOff = sSQLOneOff & " cl.bankaccount_id = ba.bankaccount_id and"
                    sSQLOneOff = sSQLOneOff & " cli.batch_id = " & CStr(lBatchID) & " and"
                    sSQLOneOff = sSQLOneOff & " cli.cashlistitem_reverse_reason_id is null"
                    sSQLOneOff = sSQLOneOff & " Group By"
                    sSQLOneOff = sSQLOneOff & " m.code, cd.suspense_account_id, ba.account_id"

                    If m_oDatabase.SQLSelect(sSQL:=sSQLOneOff, sSQLName:="Get One Off Trans For Close Batch", bStoredProcedure:=False, vResultArray:=vOneOffArray) <> gPMConstants.PMEReturnCode.PMTrue Then

                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, Failed to get the one off array for posting")
                    End If

                    'SMJB CQ2904 17/10/03: It is possible that no transactions were honoured
                    'in which case just update the batch status as closed
                    If Information.IsArray(vOneOffArray) Then
                        If BeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, Error beginning transaction")
                            bTransStarted = True
                        End If


                        For lRow As Integer = 0 To vOneOffArray.GetUpperBound(1)
                            vCreditAccount = Array.CreateInstance(GetType(Object), New Integer() {conAmount - conAccountId + 1, 1}, New Integer() {conAccountId, 0})
                            vDebitAccount = Array.CreateInstance(GetType(Object), New Integer() {conAmount - conAccountId + 1, 1}, New Integer() {conAccountId, 0})



                            vCreditAccount(conAccountId, 0) = vOneOffArray(cOneOffSuspense, lRow)


                            vCreditAccount(conAmount, 0) = vOneOffArray(cOneOffAmount, lRow)


                            vDebitAccount(conAccountId, 0) = vOneOffArray(cOneOffBankAccount, lRow)


                            vDebitAccount(conAmount, 0) = vOneOffArray(cOneOffAmount, lRow)


                            sComment = r_sInterfaceCode.Trim() & "/" & r_sBatchRef.Trim() & "/" & CStr(vOneOffArray(cOneOffMediaType, lRow)).Trim()

                            If m_oBusiness.PostTransaction(v_vCreditAccount:=vCreditAccount, v_vDebitAccount:=vDebitAccount, v_sComment:=sComment, v_sDocumentRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeJn) <> gPMConstants.PMEReturnCode.PMTrue Then

                                Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, unable to post balancing transaction")
                            End If

                        Next

                        If CommitTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                            RollbackTrans()
                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, unable to commit transaction")
                        End If
                    End If

                Case gHUBSpokeConstants.ksICRecurring
                    m_oDatabase.Parameters.Clear()

                    If m_oDatabase.Parameters.Add(sName:="batch_id", vValue:=CStr(lBatchID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, " + "Failed to add parameter to " & ksSPSelectFinancePlanName)
                    End If

                    ' SMJB CQ 2134 22/08/03: The following section has been re-written
                    ' Credits for instalments are now shown individually in the document
                    ' but debits are shown as one transaction per bank account
                    ' There will usually only be one bank account for direct debits, but we have
                    ' catered for multiple accounts

                    ' Get our array of credit transactions
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ksSPSelectFinancePlanSQL, sSQLName:=ksSPSelectFinancePlanName, bStoredProcedure:=True, vResultArray:=vInstalmentArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, Failed to get the instalmentIDs for posting")
                    End If

                    ' Now use the bank account id on the previous array to
                    ' build totals for each bank account debit
                    ' Note that there will probably only ever be 1 account but just in case...


                    ' RVH 7/11/2003 CQ2844 - START : Check if there was any returned data as using an empty array causes
                    '                        this CQ issue
                    If Information.IsArray(vInstalmentArray) Then
                        ' Grab the first item

                        lBankAccountID = CInt(vInstalmentArray(5, 0))
                        ReDim vDebitAccount(1, 0)

                        ' Loop through the instalment array which is ordered by account_id
                        ' when the account changes add an item to the debit array

                        For lCount As Integer = 0 To vInstalmentArray.GetUpperBound(1)

                            If CDbl(vInstalmentArray(ACInstalmentBankAccountID, lCount)) <> lBankAccountID Then

                                vDebitAccount(0, vDebitAccount.GetUpperBound(1)) = lBankAccountID

                                vDebitAccount(1, vDebitAccount.GetUpperBound(1)) = curTotal
                                ReDim Preserve vDebitAccount(1, vDebitAccount.GetUpperBound(1) + 1)

                                lBankAccountID = CInt(vInstalmentArray(ACInstalmentBankAccountID, lCount))
                                curTotal = 0
                            End If

                            curTotal += CDbl(vInstalmentArray(ACInstalmentAmount, lCount))
                        Next

                        ' Add the last item

                        vDebitAccount(0, vDebitAccount.GetUpperBound(1)) = lBankAccountID

                        vDebitAccount(1, vDebitAccount.GetUpperBound(1)) = curTotal



                        Dim lCreditIDArray(vInstalmentArray.GetUpperBound(1)) As Object

                        ' Post the transactions
                        m_lReturn = m_oBusiness.PostTransaction(v_vCreditAccount:=vInstalmentArray, v_vDebitAccount:=vDebitAccount, v_sComment:="Instalment Payments", r_vNewCreditTransDetailId:=lCreditIDArray, v_sDocumentRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeINC)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, Failed to post transactions")
                        End If

                        ' Now allocate our credits against their instalment transactions

                        For lCount As Integer = 0 To vInstalmentArray.GetUpperBound(1)






                            'Developer Guide no. 101
                            m_lReturn = Allocate(v_lAccountId:=vInstalmentArray(ACInstalmentBankAccountID, lCount), v_vTransaction:=lCreditIDArray(lCount) & "|" & (vInstalmentArray(ACInstalmentAmount, lCount) * -1), v_vTransactions:=vInstalmentArray(ACInstalmentTransDetail, lCount) & "|" & vInstalmentArray(ACInstalmentAmount, lCount))
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, Failed to allocate transactions")
                            End If

                            ' Now update the status of the instalment item
                            With m_oDatabase

                                .Parameters.Clear()


                                .Parameters.Add(sName:="PFInstalmentsID", vValue:=CStr(vInstalmentArray(ACInstalmentID, lCount)), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                                .Parameters.Add(sName:="StatusID", vValue:=CStr(3), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                                .Parameters.Add(sName:="TransDetailID", vValue:=CStr(vInstalmentArray(ACInstalmentTransDetail, lCount)), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                                m_lReturn = .SQLAction(sSQL:=ksSPUpdateInstalmentSQL, sSQLName:=ksSPUpdateInstalmentName, bStoredProcedure:=ksSPUpdateInstalmentStored)
                            End With
                        Next

                        ' SMJB End
                    End If
                    ' RVH 7/11/2003 CQ2844 - END
            End Select
            '****************************************************************************************************************************************
            ' END OF Perform operations on each retrieved One Off / Recurring Transaction row.
            '****************************************************************************************************************************************

            '****************************************************************************************************************************************
            ' Mark the batch as closed.
            '****************************************************************************************************************************************
            If m_oBusiness.GetIDValueFromCode(v_sTableName:="BatchStatus", v_bGettingCode:=False, r_sCode:=conBCCode, r_lID:=lBatchStatusID) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return result
            End If

            sSQLUpdate = "UPDATE Batch" & Strings.Chr(13) & Strings.Chr(10)
            sSQLUpdate = sSQLUpdate & "SET batchstatus_id = " & CStr(lBatchStatusID) & conComma & Strings.Chr(13) & Strings.Chr(10)
            sSQLUpdate = sSQLUpdate & "closed_date = GetDate() " & Strings.Chr(13) & Strings.Chr(10)
            sSQLUpdate = sSQLUpdate & "WHERE batch_id = " & CStr(lBatchID)

            If sSQLUpdate.Length > 0 Then
                If m_oDatabase.SQLAction(sSQL:=sSQLUpdate, sSQLName:="Update Batch", bStoredProcedure:=False) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", Start, Error updating batch record")
                    Return result
                End If
            End If
            '****************************************************************************************************************************************
            ' END OF Mark the batch as closed.
            '****************************************************************************************************************************************

            '****************************************************************************************************************************************
            ' END OF CLOSE BATCH HANDLING
            '****************************************************************************************************************************************

            'Return codes
            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_IMPORT_COMPLETE
            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_IMPORT_COMPLETE

            result = gPMConstants.PMEReturnCode.PMTrue


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception

            If bTransStarted Then
                RollbackTrans()
            End If

            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally
            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function
    '#End Region

    ' ***************************************************************** '
    ' Name: Allocate(Allocate)
    '
    ' Description: Allocate the Instalment Plan credit/cash
    '              against the original debt
    '
    ' ***************************************************************** '
    Private Function Allocate(ByVal v_lAccountId As Integer, ByVal v_vTransaction As String, ByVal v_vTransactions As String) As Integer

        Dim result As Integer = 0
        Dim oPremiumFinance As Object
        Dim vKeys As Object
        Dim oAllocateManual As bACTAllocationManual.Business



        result = gPMConstants.PMEReturnCode.PMTrue


        ReDim vKeys(1, 2)
        ' AccountID

        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID

        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = v_lAccountId '<customer account>

        ' Main Credit Transaction ID

        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameTransDetailID

        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = v_vTransaction '<transdetail of instalment credit just added "|" -ve amount> string

        ' Matched Transactions

        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameTransDetailIDs

        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = v_vTransactions '<transdetail of plan "|" +ve amount> in this case just one 0 entry array

        ' Create an instance of the OrionLink business object


        oAllocateManual = New bACTAllocationManual.Business
        m_lReturn = oAllocateManual.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise the manual alloction", vApp:=ACApp, vClass:=ACClass, vMethod:="Allocate", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return result
        End If



        m_lReturn = oAllocateManual.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

        ' Set the keys

        m_lReturn = oAllocateManual.SetKeys(vKeyArray:=vKeys)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set navigator keys.", vApp:=ACApp, vClass:=ACClass, vMethod:="Allocate", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Start it

        oAllocateManual.CompanyId = m_iSourceID


        m_lReturn = oAllocateManual.Start()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start Allocation Manual.", vApp:=ACApp, vClass:=ACClass, vMethod:="Allocate", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        oAllocateManual.Dispose()


        Return result

    End Function
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
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Private Function BeginTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Private Function CommitTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Private Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
