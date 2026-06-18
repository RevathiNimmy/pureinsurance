Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Text
'Developer Guide no.129
Imports SharedFiles
Friend NotInheritable Class ExportAutoBank
    '====================================================================
    '   Class/Module: ExportAutoBank
    '   Description : Class implementation of use case:
    'Export for InterfaceCode: "AutoBank"'
    '
    '====================================================================
    '   Maintenance History
    '
    '    11 November 2002    Paul Cunnigham    Created.
    '
    '====================================================================



    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "ExportAutoBank"

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
    'enum representing columns in result array for SP: spu_ACT_Spoke_ExportAutoBank
    Private Enum ACAutoBankingTransaction
        MerchantNumber = 0
        MediaType
        Amount
        CollectionAccountId
        SuspenseAccountID
        CashListItemId
        TransDetaiId
        MatchingTransDetailId
    End Enum

    '#End Region

    '#Region " Stored Procedures "
    'developer guide no. 39
    Private Const ksSPGetAutoBankingTransactionSQL As String = "spu_ACT_Spoke_ExportAutoBank"
    Private Const ksSPGetAutoBankingTransactionName As String = "GetAutoBankingTransactions"
    Private Const ksSPGetAutoBankingTransactionStored As Boolean = True

    'developer guide no. 39
    Private Const ksSPCloseCashListsSQL As String = "spu_ACT_Spoke_ExportAutoBank_CloseCashLists"
    Private Const ksSPCloseCashListsName As String = "CloseCashLists"
    Private Const ksSPCloseCashListsStored As Boolean = True
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
    Friend Function Start(ByRef r_sStatusCode As String, ByRef r_sMessage As String) As Integer
        Dim result As Integer = 0
        Dim vNewCreditTransDetailIds As Object
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: Export
        ' PURPOSE: Starting routine for use case
        ' AUTHOR: Paul Cunnigham
        ' DATE: 11 November 2002, 11:45:03
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim bTransactionStarted As Boolean
        Dim vAutoBankingTransactions, vCreditTransactions, vDebitTransactions, vOriginalDebits As Object

        Dim vCreditTransaction, vDebitTransaction, vOriginalDebit As Object

        Dim lLowerRowId, lUpperRowId As Integer



        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'Return codes
            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_FAILED
            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_FAILED

            bTransactionStarted = False

            'We need valid database and business objects
            If (m_oBusiness Is Nothing) Or (m_oDatabase Is Nothing) Then Return result

            'OK do the Export processing...

            'Get the individual cashlistitems that need crediting to the collection account
            '(and therefore debiting to the bank account)
            If GetAutoBankingTotals(r_vResultArray:=vAutoBankingTransactions) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return result
            End If

            'Process any rows returned
            If Information.IsArray(vAutoBankingTransactions) Then
                'Load the arrays that will be used for processing the postings




                If LoadPostingArrays(r_vAutoBankingTransactions:=vAutoBankingTransactions, r_vCreditTransactions:=vCreditTransactions, r_vDebitTransactions:=vDebitTransactions, r_vDebitTransForLaterAllocation:=vOriginalDebits) <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                'Begin a transaction
                If m_oDatabase.SQLBeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If

                bTransactionStarted = True


                lLowerRowId = vCreditTransactions.GetLowerBound(conRows - 1)

                lUpperRowId = vCreditTransactions.GetUpperBound(conRows - 1)

                ReDim vCreditTransaction(conAmount, 0)
                ReDim vDebitTransaction(conAmount, 0)
                ReDim vOriginalDebit(0)

                ' RVH 21/11/2003 - CQ957 : Need to loop for each of the items and post transactions individually.
                For lPostLoop As Integer = lLowerRowId To lUpperRowId


                    vCreditTransaction(conAccountId, 0) = vCreditTransactions(conAccountId, lPostLoop)


                    vCreditTransaction(conAmount, 0) = vCreditTransactions(conAmount, lPostLoop)



                    vDebitTransaction(conAccountId, 0) = vDebitTransactions(conAccountId, lPostLoop)


                    vDebitTransaction(conAmount, 0) = vDebitTransactions(conAmount, lPostLoop)



                    vOriginalDebit(0) = vOriginalDebits(lPostLoop)

                    ReDim vNewCreditTransDetailIds(0)

                    'Post the items in the credit / debit arrays, returning the
                    'TransDetailIds of the new postings

                    If m_oBusiness.PostTransaction(v_vCreditAccount:=vCreditTransaction, v_vDebitAccount:=vDebitTransaction, v_sComment:=Nothing, r_vNewCreditTransDetailId:=vNewCreditTransDetailIds, v_sDocumentRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeJn) <> gPMConstants.PMEReturnCode.PMTrue Then

                        'Rollback the transaction
                        m_oDatabase.SQLRollbackTrans()

                        Return result
                    End If

                    'Allocate the NEW credits against the ORIGINAL (matching debit) entries



                    If Allocate(r_vOriginalDebits:=vOriginalDebit, r_vNewCreditTransDetailIds:=vNewCreditTransDetailIds, r_vCreditTransactions:=vCreditTransaction) <> gPMConstants.PMEReturnCode.PMTrue Then

                        'Rollback the transaction
                        m_oDatabase.SQLRollbackTrans()

                        Return result
                    End If

                Next lPostLoop

                'Mark cashlists as closed
                If CloseCashLists() <> gPMConstants.PMEReturnCode.PMTrue Then

                    'Rollback the transaction
                    m_oDatabase.SQLRollbackTrans()

                    Return result
                End If

                'Commit the transaction
                If m_oDatabase.SQLCommitTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Rollback the transaction
                    m_oDatabase.SQLRollbackTrans()

                    Return result
                End If

            Else
                'No outstanding records exist so just close the cashlists
                'Mark cashlists as closed
                If CloseCashLists() <> gPMConstants.PMEReturnCode.PMTrue Then

                    Return result
                End If

            End If

            'Return codes
            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_COMPLETE
            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_COMPLETE

            result = gPMConstants.PMEReturnCode.PMTrue


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception

            'Rollback the transaction is started
            If bTransactionStarted Then
                m_oDatabase.SQLRollbackTrans()
            End If

            Select Case Information.Err().Number
                Case Else
                    'Log Error.
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
        Exit Sub
    End Sub

    Private Function GetAutoBankingTotals(ByRef r_vResultArray(,) As Object) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetAutoBankingTotals
        ' PURPOSE: Gets a list of the cashlistitems that require automatic banking
        ' AUTHOR: Paul Cunnigham
        ' DATE: 11 November 2002, 12:53:53
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMFalse

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Execute SQL Statement
        If m_oDatabase.SQLSelect(sSQL:=ksSPGetAutoBankingTransactionSQL, sSQLName:=ksSPGetAutoBankingTransactionName, bStoredProcedure:=ksSPGetAutoBankingTransactionStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then

            Return result
        End If

        result = gPMConstants.PMEReturnCode.PMTrue


        Return result

    End Function

    Private Function LoadPostingArrays(ByRef r_vAutoBankingTransactions(,) As Object, ByRef r_vCreditTransactions(,) As Object, ByRef r_vDebitTransactions(,) As Object, ByRef r_vDebitTransForLaterAllocation() As Object) As Integer
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


        Dim sMerchantNumber As String = ""
        Dim lMediaTypeID As Integer
        Dim cAmountToPost As Decimal
        Dim lPreviousCollectionAccount, lPreviousSuspenseAccount As Integer
        Dim sDebitTransForLaterAllocation As New StringBuilder
        Dim lNewUbound As Integer



        result = gPMConstants.PMEReturnCode.PMFalse

        lNewUbound = 0

        'Get the lower and upper limits of the dimension of the array that stores the row data
        lLowerRow = r_vAutoBankingTransactions.GetLowerBound(conRows - 1)
        lUpperRow = r_vAutoBankingTransactions.GetUpperBound(conRows - 1)

        'Size the posting arrays
        r_vCreditTransactions = Array.CreateInstance(GetType(Object), New Integer() {conAmount - conAccountId + 1, 1}, New Integer() {conAccountId, 0})
        r_vDebitTransactions = Array.CreateInstance(GetType(Object), New Integer() {conAmount - conAccountId + 1, 1}, New Integer() {conAccountId, 0})
        ReDim r_vDebitTransForLaterAllocation(0)

        'Loop through the result array and populate corresponding row in both
        'the credit array and debit array
        '(Post credit to the collection account and debits to the suspense account)
        'also build array of debit transactions for later allocation

        'we need to group the transactions by merchant number and media type

        'store the details of the first record

        sMerchantNumber = CStr(r_vAutoBankingTransactions(ACAutoBankingTransaction.MerchantNumber, 0))

        lMediaTypeID = CInt(r_vAutoBankingTransactions(ACAutoBankingTransaction.MediaType, 0))

        cAmountToPost = CDec(r_vAutoBankingTransactions(ACAutoBankingTransaction.Amount, 0))

        lPreviousCollectionAccount = CInt(r_vAutoBankingTransactions(ACAutoBankingTransaction.CollectionAccountId, 0))

        lPreviousSuspenseAccount = CInt(r_vAutoBankingTransactions(ACAutoBankingTransaction.SuspenseAccountID, 0))


        sDebitTransForLaterAllocation = New StringBuilder(CStr(CInt(r_vAutoBankingTransactions(ACAutoBankingTransaction.MatchingTransDetailId, 0))) & _
                                        "|" & CStr(CDec(r_vAutoBankingTransactions(ACAutoBankingTransaction.Amount, 0))))

        lNewUbound = 0

        For lRow As Integer = (lLowerRow + 1) To lUpperRow


            If CStr(r_vAutoBankingTransactions(ACAutoBankingTransaction.MerchantNumber, lRow)) = sMerchantNumber And CDbl(r_vAutoBankingTransactions(ACAutoBankingTransaction.MediaType, lRow)) = lMediaTypeID Then


                'This record need to be grouped with the previous one

                cAmountToPost += CDbl(r_vAutoBankingTransactions(ACAutoBankingTransaction.Amount, lRow))

                lPreviousCollectionAccount = CInt(r_vAutoBankingTransactions(ACAutoBankingTransaction.CollectionAccountId, lRow))

                lPreviousSuspenseAccount = CInt(r_vAutoBankingTransactions(ACAutoBankingTransaction.SuspenseAccountID, lRow))

                'store debit trans in comma separated variable


                sDebitTransForLaterAllocation.Append("," & _
                                                     CInt(r_vAutoBankingTransactions(ACAutoBankingTransaction.MatchingTransDetailId, lRow)) & _
                                                     "|" & CStr(CDec(r_vAutoBankingTransactions(ACAutoBankingTransaction.Amount, lRow))))

            Else

                'write the previous grouped records to the arrays

                r_vDebitTransactions(conAccountId, lNewUbound) = lPreviousSuspenseAccount

                r_vDebitTransactions(conAmount, lNewUbound) = cAmountToPost


                r_vCreditTransactions(conAccountId, lNewUbound) = lPreviousCollectionAccount

                r_vCreditTransactions(conAmount, lNewUbound) = cAmountToPost


                r_vDebitTransForLaterAllocation(lNewUbound) = sDebitTransForLaterAllocation.ToString()

                lNewUbound += 1

                'Increase size of posting array by one
                r_vCreditTransactions = ArraysHelper.RedimPreserve(Of Object(,))(r_vCreditTransactions, New Integer() {conAmount - conAccountId + 1, lNewUbound + 1}, New Integer() {conAccountId, 0})
                r_vDebitTransactions = ArraysHelper.RedimPreserve(Of Object(,))(r_vDebitTransactions, New Integer() {conAmount - conAccountId + 1, lNewUbound + 1}, New Integer() {conAccountId, 0})
                ReDim Preserve r_vDebitTransForLaterAllocation(lNewUbound)


                cAmountToPost = CDec(r_vAutoBankingTransactions(ACAutoBankingTransaction.Amount, lRow))

                lPreviousCollectionAccount = CInt(r_vAutoBankingTransactions(ACAutoBankingTransaction.CollectionAccountId, lRow))

                lPreviousSuspenseAccount = CInt(r_vAutoBankingTransactions(ACAutoBankingTransaction.SuspenseAccountID, lRow))


                sDebitTransForLaterAllocation = New StringBuilder(CStr(CInt(r_vAutoBankingTransactions(ACAutoBankingTransaction.MatchingTransDetailId, lRow))) & _
                                                "|" & CStr(CDec(r_vAutoBankingTransactions(ACAutoBankingTransaction.Amount, lRow))))

                ' RVH 21/11/2003 - CQ957 : Reset matching items

                sMerchantNumber = CStr(r_vAutoBankingTransactions(ACAutoBankingTransaction.MerchantNumber, lRow))

                lMediaTypeID = CInt(r_vAutoBankingTransactions(ACAutoBankingTransaction.MediaType, lRow))
            End If

            If lRow = lUpperRow Then


                r_vCreditTransactions(conAccountId, lNewUbound) = lPreviousCollectionAccount

                r_vCreditTransactions(conAmount, lNewUbound) = cAmountToPost


                r_vDebitTransactions(conAccountId, lNewUbound) = lPreviousSuspenseAccount

                r_vDebitTransactions(conAmount, lNewUbound) = cAmountToPost


                r_vDebitTransForLaterAllocation(lNewUbound) = sDebitTransForLaterAllocation.ToString()
            End If

        Next lRow

        result = gPMConstants.PMEReturnCode.PMTrue


        Return result

    End Function

    Private Function CloseCashLists() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: CloseCashLists
        ' PURPOSE: Close all unclosed cash lists that have auto banking set
        ' AUTHOR: Paul Cunningham
        ' DATE: 11 December 2002, 14:30:16
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMFalse

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Execute SQL Statement
        If m_oDatabase.SQLAction(sSQL:=ksSPCloseCashListsSQL, sSQLName:=ksSPCloseCashListsName, bStoredProcedure:=ksSPCloseCashListsStored) <> gPMConstants.PMEReturnCode.PMTrue Then

            Return result
        End If

        result = gPMConstants.PMEReturnCode.PMTrue


        Return result


    End Function

    Private Function Allocate(ByRef r_vOriginalDebits() As Object, ByRef r_vNewCreditTransDetailIds() As Object, ByRef r_vCreditTransactions(,) As Object) As Integer
        Dim result As Integer = 0
        Dim vKeyArray(,) As Object
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: Allocate
        ' PURPOSE: Allocate the NEW credits against the ORIGINAL (matching debit) entries
        ' AUTHOR: Paul Cunningham
        ' DATE: 11 December 2002, 15:55:34
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim lAccountId, lCreditTransDetailId, lDebitTransDetailId, lCreditAmount, lDebitAmount As Integer
        Dim oAllocationManual As bACTAllocationManual.Business
        Dim vAllocationTrans As Object



        result = gPMConstants.PMEReturnCode.PMFalse

        'Use the bACTAllocationManual component to do the allocation
        oAllocationManual = New bACTAllocationManual.Business
        If oAllocationManual.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

            Return result
        End If

        'Format the Outstanding transactions for use with Manual Allocation
        For lRow As Integer = r_vNewCreditTransDetailIds.GetLowerBound(0) To r_vNewCreditTransDetailIds.GetUpperBound(0)

            'Get the detail required for allocation
            'Original debit


            vAllocationTrans = CStr(r_vOriginalDebits(lRow)).Split(","c)


            ReDim Preserve vAllocationTrans(vAllocationTrans.GetUpperBound(0))

            'The new credit

            lAccountId = CInt(r_vCreditTransactions(conAccountId, lRow))

            lCreditTransDetailId = CInt(r_vNewCreditTransDetailIds(lRow))


            lCreditAmount = CInt(-(CDec(r_vCreditTransactions(conAmount, lRow))))

            'Set keys for the AllocationManual component
            vKeyArray = Array.CreateInstance(GetType(Object), New Integer() {gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue - gPMConstants.PMENavLetGetKeyColPosition.PMKeyName + 1, 3}, New Integer() {gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0})

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameTransDetailID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameTransDetailIDs

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = lAccountId

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = CStr(lCreditTransDetailId) & "|" & CStr(lCreditAmount)


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = vAllocationTrans

            'Perform the allocation
            With oAllocationManual

                If .SetKeys(vKeyArray:=vKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Return result
                End If


                If .Start() <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return result
                End If
            End With
        Next

        result = gPMConstants.PMEReturnCode.PMTrue

        If Not (oAllocationManual Is Nothing) Then

            oAllocationManual.Dispose()
            oAllocationManual = Nothing
        End If

        Return result


    End Function
    '#End Region
End Class
