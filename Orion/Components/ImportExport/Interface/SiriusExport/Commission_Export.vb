Imports System.Collections.ObjectModel
Imports System.Xml
Imports System.Xml.Schema
Imports SharedFiles

Public NotInheritable Class Commission_Export : Inherits ExportBase

#Region "Fields"

    Private _batchId As Integer = 0
    Private _newBatch As Byte = 0
    Private _leadDays As Integer = 0
    Private _currencyID As Integer = 0
    Private _currencyCode As String = "(ALL)"
    Private _agentTypeCode As String = "(ALL)"
    Private _AllocationDateFrom As Nullable(Of DateTime) = "01/01/1899"
    Private _AllocationDateTo As Nullable(Of DateTime) = DateTime.Today.ToShortDateString()
    Private _agentIds As String = String.Empty
    Private _sessionGUID As String = String.Empty
    Private m_sBankAccountName As String = ""
    Private m_sMediaTypeCode As String = ""

    Public Const _accountIdRank As Short = 0
    Public Const _agentRank As Short = 1
    Public Const _agentNameRank As Short = 2
    Public Const _totalCommRank As Short = 3
    Public Const _CurrencyRank As Short = 4
    Public Const _currencyIDRank As Short = 5
    Public Const _agentAccountIDRank As Short = 0
    Public Const _agentAddressRank As Short = 1
    Public Const _agentMediaTypeRank As Short = 2
    Public Const _bankAccountIdRank As Short = 3

    Private _agentSearchArray As Object = Nothing
    Private _summaryArray As Object = Nothing
    Private _agentSummaryArray(,) As Object = Nothing

#End Region

#Region "Enums"
    Public Enum ACListPaymentSummary
        PSMediaType = 0
        PSPaymentCount = 1
        PSPaymentValue = 2
    End Enum

    Public Enum eCashListItem
        CashlistitemID
        AllocationstatusID
        MediaTypeID
        MediaTypeIssuerID
        CashlistID
        AccountId
        MediaRef
        OurRef
        TheirRef
        Amount
        TransdetailID
        ContactName
        Address1
        Address2
        Address3
        Address4
        PostalCode
        AddressCountry
        PaymentName
        PaymentAccountCode
        PaymentBranchCode
        PaymentExpiryDate
        PaymentReference1
        PaymentReference2
        Letter
        Batch_id
        pmuser_id
        Transaction_Date
        Original_Amount
        Amount_Tendered
        Change
        CashListItem_receipt_type_id
        CashListItem_receipt_status_id
        CashListItem_bank_id
        Cheque_Date
        CC_Name
        CC_Number
        CC_Expiry_Date
        CC_Start_Date
        CC_Issue
        CC_Pin
        CC_Auth_Code
        CC_Customer
        CC_Manual_Auth_Code
        CC_Transaction_Code
        Receipt_Details
        CashListItem_Reverse_PMUser_id
        CashListItem_Reverse_Reason_id
        CashListItem_Payment_Type_id
        CashListItem_Payment_Status_id
        Date_Presented
        Cheque_in_Possession
        Stop_Requested_Date
        Stop_Printed_Date
        Stop_Confirmation_Date
        Reason
        Replaces_CashListItem_id
        XML_Object
        InstalmentArray
        SalvageArray
        CLMUSRecoveryArray
        CLMRVRecoveryArray
        UnderwritingYearID
        CurrencyBaseDate
        CurrencyBaseXrate
        AccountBaseDate
        AccountBaseXrate
        SystemBaseDate
        SystemBaseXrate
        OverrideReason
        CashListItem_Comments_Array
        PartyBankId
        CollectionDate
        Comments
        BGPolicies
        CashListItem_bank
        BankLocation
        BankBranch
        ChequeTypeId
        CCBankId
        CardTypeId
        CardTransSlipNo
        ChequeClearingTypeId
        IsLeadAccount
        SplitTotal
        TaxBandId
        TaxAmount
        BIC
        IBAN
        LastItem
    End Enum
#End Region

#Region "Public Properties"
    ''' <summary>
    ''' Builds the export filename for this interface
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides ReadOnly Property Filename() As String
        Get
            If (m_sFilename.Length = 0) Then
                m_sFilename = String.Format("{0}_{1}_{2}.xml", InterfaceName, _batchId, Now.ToString("yyyyMMddhhmm"))
            End If
            Return m_sFilename
        End Get
    End Property

    ''' <summary>
    ''' Interface name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides ReadOnly Property InterfaceName() As String
        Get
            Return "Commission_Export"
        End Get
    End Property

    ''' <summary>
    ''' Batch id for the export
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Property BatchId() As Integer
        Get
            Return _batchId
        End Get
        Set(ByVal value As Integer)
            _batchId = value
        End Set
    End Property

#End Region

#Region "Public Methods"
    ''' <summary>
    ''' Display help for this interface
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub DisplayHelp()

        ' Write syntax help for this command
        'OutputLine("SIRIUSEXPORT Payment_Export [batchid]")
        OutputLine("Example call : - SIRIUSEXPORT Commission_Export Batch_ID =1 Currency_Code="""" Agent_Type="""" lead_days=0")
        OutputLine()
        OutputLine("BATCH_ID     - If specified a previous batch is recreated")
        OutputLine("               If no batchid is specified a new batch is created")
        OutputLine()
        OutputLine("LEAD_DAYS           - (optional) Numeric")
        OutputLine("CURRENCY_ID          - (Int)")
        OutputLine("AGENT_TYPE_CODE         - (optional) e.g. Brokers")
        OutputLine("ALLOCATION_DATE_FROM     - (optional) ")
        OutputLine("ALLOCATION_DATE_TO     - (optional) ")

    End Sub

    ''' <summary>
    ''' Segregates the arguments supplied from commandLine to this interface
    ''' </summary>
    ''' <param name="arguments"></param>
    ''' <remarks></remarks>
    Public Overrides Sub ProcessCommandLine(ByVal arguments As Collection(Of String))
        Dim argument As String
        Dim argumentValues() As String

        For Each argument In arguments

            'Split the argument into argument name / argument value
            argumentValues = argument.Split(CChar("="))

            Try
                'Determine which argument we are looking at
                Select Case argumentValues(0).ToUpper.Trim()

                    Case "BATCH_ID"
                        _batchId = ToSafeInteger(argumentValues(1))

                    Case "LEAD_DAYS"
                        _leadDays = ToSafeInteger(argumentValues(1))

                    Case "CURRENCY_ID"
                        _currencyID = ToSafeInteger(argumentValues(1))
                        GetCurrencyCode()

                    Case "AGENT_TYPE_CODE"
                        _agentTypeCode = ToSafeString(argumentValues(1))

                    Case "ALLOCATION_DATE_FROM"
                        _AllocationDateFrom = Convert.ToDateTime(argumentValues(1))

                    Case "ALLOCATION_DATE_TO"
                        _AllocationDateTo = Convert.ToDateTime(argumentValues(1))

                    Case "MEDIA_TYPE_CODE"
                        m_sMediaTypeCode = ToSafeString(argumentValues(1))


                    Case "BANK_ACCOUNT_NAME"
                        m_sBankAccountName = ToSafeString(argumentValues(1))
                    Case Else
                        Throw New ArgumentException("Invalid argument " + argumentValues(0).ToString)
                End Select

            Catch ex As Exception

                Throw New ArgumentException("Invalid argument " + argumentValues(0).ToString, ex)
            End Try
        Next
    End Sub

    ''' <summary>
    ''' Process the export
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub ProcessExport()

        ' Write status line
        OutputLine("Payments Export")
        OutputLine()

        ' Existing batch or new one
        If _batchId > 0 Then
            OutputLine(String.Format("Recreating batch {0}", _batchId))
        Else
            Output("Creating new batch...")
            CreateBatch()
            OutputLine(String.Format("Created batch {0}", _batchId))
        End If

        ' Export the batch 
        ExportBatch()
        UpdateBatchTask(kBatchStatusComplete, _batchId, Filename, 0, 0)
    End Sub

#End Region

#Region "Private Methods"

    ''' <summary>
    ''' Method used to create the batch if no batch id is supplied
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub CreateBatch()
        Dim returnCode As PMEReturnCode = PMEReturnCode.PMTrue

        Try
            m_oDatabase.Parameters.Clear()
            ' Add parameters
            AddParameterLite(m_oDatabase, "Batch_id", System.DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMInteger, True)
            AddParameterLite(m_oDatabase, "Agent_Type_Code", _agentTypeCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "Currency_Id", _currencyID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            ' Execute command
            returnCode = m_oDatabase.SQLAction("spu_ACT_CommissionExport_CreateBatch", "Create Batch", True)
            If returnCode <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spu_ACT_CommissionExport_CreateBatch'")
            End If

            ' Get the batch id
            _batchId = m_oDatabase.Parameters.Item("batch_id").Value
            _newBatch = 1
        Catch ex As Exception
            Throw New Exception("Unable to create new export batch", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Auto Release Agent Commission
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub AutoReleaseCommission()

        Dim returnCode As PMEReturnCode = PMEReturnCode.PMFalse
        Dim oBObjectManager As bObjectManager.ObjectManager
        Dim obSIRFindParty As Object = Nothing
        Dim obACTCommissionPayments As Object = Nothing

        Try
            oBObjectManager = New bObjectManager.ObjectManager()
            returnCode = oBObjectManager.Initialise(MainModule.ACApp)
            If returnCode <> PMEReturnCode.PMTrue Then
                OutputLine(String.Format("Unable to Initializing ObjectManager ", _batchId))
                Exit Sub
            End If

            returnCode = oBObjectManager.GetInstance(obSIRFindParty, "bSIRFindParty.Business", vInstanceManager:=PMGetViaClientManager)
            If returnCode <> PMEReturnCode.PMTrue Then
                OutputLine(String.Format("Unable to create bSIRFindParty object ", _batchId))
                Exit Sub
            End If

            returnCode = obSIRFindParty.SearchAgent(r_vResultArray:=_agentSearchArray, vShortname:=String.Empty, vName:=String.Empty, vpartyAgentDesc:=_agentTypeCode, vCurrCode:=_currencyCode, vSubBraDesc:="(ALL)", vIsGrossAgent:=0)
            If returnCode <> PMEReturnCode.PMTrue Then
                OutputLine(String.Format("Unable to search agent ", _batchId))
                Exit Sub
            End If

            If IsArray(_agentSearchArray) Then
                For agentCount As Integer = LBound(_agentSearchArray, 2) To UBound(_agentSearchArray, 2)
                    Dim sLockedBy As String = String.Empty
                    returnCode = LockAgentForCommissionPayment(ToSafeLong(_agentSearchArray(0, agentCount)), sLockedBy)
                    If returnCode = PMEReturnCode.PMTrue Then
                        If agentCount = LBound(_agentSearchArray, 2) Then
                            _agentIds = _agentSearchArray(0, agentCount)
                        Else
                            _agentIds = String.Format("{0},{1}", _agentIds, _agentSearchArray(0, agentCount).ToString())
                        End If
                    End If
                Next
            End If
            If Len(_agentIds) > 0 Then
                returnCode = oBObjectManager.GetInstance(oObject:=obACTCommissionPayments, sClassName:="bACTCommissionPayments.Business", vInstanceManager:=PMGetViaClientManager)
                If returnCode <> PMEReturnCode.PMTrue Then
                    OutputLine(String.Format("Unable to create bACTCommissionPayments object ", _batchId))
                    Exit Sub
                End If

                _AllocationDateTo = ToSafeDate(_AllocationDateTo).AddDays(-_leadDays)
                returnCode = obACTCommissionPayments.PrepareAgentSummaryForAllocatedTrans(ToSafeDate(_AllocationDateFrom), ToSafeDate(_AllocationDateTo), _currencyID, 0, 0, oBObjectManager.UserID, 0, _agentIds, _sessionGUID, _agentSummaryArray)
                If returnCode <> PMEReturnCode.PMTrue Then
                    OutputLine(String.Format("Unable to prepare agent summary ", _batchId))
                    Exit Sub
                ElseIf Information.IsNothing(_agentSummaryArray) Then
                    OutputLine(String.Format("No data returned by PrepareAgentSummary ", _batchId))
                    Exit Sub
                End If

                'Call function to Mark Commission Payments for the selected accounts
                returnCode = MarkCommissionPayments()
                If returnCode <> PMEReturnCode.PMTrue Then
                    OutputLine(String.Format("Unable to mark Commission Payments for agent accounts ", _batchId))
                    Exit Sub
                End If

                If (Not Information.IsNothing(_agentSummaryArray)) Then
                    returnCode = SettleAllPayments()
                    If returnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError("SettleAllPayments", "Function Settle All Payments Failed")
                    End If
                End If
            End If
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="AutoReleaseCommission", r_lFunctionReturn:=returnCode, excep:=ex)
        Finally
            obSIRFindParty.Dispose()
            obSIRFindParty = Nothing
            oBObjectManager = Nothing
            obACTCommissionPayments = Nothing
        End Try

    End Sub

    Private Function LockAgentForCommissionPayment(ByVal agentID As Integer, ByRef lockedBy As String) As Integer

        Dim returnCode As PMEReturnCode = PMEReturnCode.PMTrue
        Dim oBObjectManager As bObjectManager.ObjectManager
        Dim oPMLock As Object = Nothing
        Dim currentlyLockedBy As String = String.Empty
        Dim partyShortName As String

        Try
            oBObjectManager = New bObjectManager.ObjectManager()
            oBObjectManager.Initialise(MainModule.ACApp)

            returnCode = oBObjectManager.GetInstance(oObject:=oPMLock, sClassName:="bPMLock.User", vInstanceManager:=PMGetViaClientManager)
            If (returnCode <> gPMConstants.PMEReturnCode.PMTrue) Then
                LockAgentForCommissionPayment = returnCode
                Exit Function
            End If

            returnCode = oPMLock.LockKey(sKeyName:="CommissionPayment", vKeyValue:=agentID, iUserID:=oBObjectManager.UserID, sCurrentlyLockedBy:=currentlyLockedBy)
            Select Case returnCode

                Case gPMConstants.PMEReturnCode.PMTrue

                Case gPMConstants.PMEReturnCode.PMFalse
                    'Locked or error
                    LockAgentForCommissionPayment = returnCode
                    If (currentlyLockedBy = "ERROR") Then
                        RaiseError("LockAgentForCommissionPayment", "oPMLock.LockKey Failed", gPMConstants.PMELogLevel.PMLogError)
                    Else
                        returnCode = oPMLock.GetShortNameForParty(v_lPartyCnt:=agentID, r_sPartyShortName:=partyShortName)
                        If returnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                            LockAgentForCommissionPayment = returnCode
                            RaiseError("LockAgentForCommissionPayment", "m_oBusiness.GetShortNameForParty Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                        lockedBy = lockedBy & "Agent " & "'" & partyShortName & "'" & " locked for Commission Payments by '" & currentlyLockedBy & " '. Processing cannot be done on this Account." & vbCrLf
                    End If
                Case Else
                    RaiseError("LockAgentForCommissionPayment", "oPMLock.LockKey Failed", gPMConstants.PMELogLevel.PMLogError)
            End Select

            Return returnCode
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="LockAgentForCommissionPayment", r_lFunctionReturn:=LockAgentForCommissionPayment, excep:=ex)

        Finally
            oPMLock.Dispose()
            oPMLock = Nothing
            oObjectManager = Nothing

        End Try

    End Function

    Private Function SettleAllPayments() As Integer

        Const methodName As String = "SettleAllPayments"
        Dim chequeProduction As Boolean = False
        Dim returnCode As PMEReturnCode = PMEReturnCode.PMTrue
        Dim totalAmountReferredForAuthorisation As Decimal = 0

        Try
            SettleAllPayments = gPMConstants.PMEReturnCode.PMTrue

            Dim agentArrayLBound As Integer = _agentSummaryArray.GetLowerBound(1)
            Dim agentArrayUBound As Integer = _agentSummaryArray.GetUpperBound(1)

            For agentCountIndex As Integer = agentArrayLBound To agentArrayUBound
                returnCode = SettlePayment(agentCountIndex)
            Next

            If totalAmountReferredForAuthorisation > 0 Then
                returnCode = SaveSummary("Payment Referred for Auth.", totalAmountReferredForAuthorisation)
                If returnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(methodName, "GetNumberOfSelectedItems Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                _summaryArray(ACListPaymentSummary.PSPaymentCount, UBound(_summaryArray, 2)) = 0

            End If

        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=methodName, r_lFunctionReturn:=SettleAllPayments, excep:=ex)

        End Try

    End Function

    Private Function SettlePayment(ByRef index As Short) As Integer

        Const methodName As String = "SettlePayment"
        Dim oBObjectManager As bObjectManager.ObjectManager

        Dim returnCode As Integer
        Dim agentAccountId As Integer
        Dim mediaTypeId As Integer
        Dim accCurrencyId As Integer
        Dim documentId As Integer
        Dim bankAccountId As Integer
        Dim batchId As Integer
        Dim transDetailID As Integer
        Dim cashListId As Integer
        Dim cashListItemId As Integer
        Dim rowCount As Integer

        Dim documentArray() As Object
        Dim documentTempArray As Object = Nothing
        Dim cashListItem As Object
        Dim keyArray As Object
        Dim cashListItemDetails As Object = Nothing
        Dim oOSTransactions As Object

        Dim agentAddress As String = String.Empty
        Dim commType As String = String.Empty
        Dim paymentType As String = String.Empty
        Dim agentName As String = String.Empty
        Dim mediaType As String = String.Empty

        Dim statementDate As Date
        Dim commPaymentAmount As Decimal
        Dim paymentAuthority As Decimal

        Dim proceedFurther As Boolean
        Dim hasPaymentsAuthority As Boolean
        Dim isDeleted As Boolean
        Dim proceedToPost As Boolean

        Dim oCashList As Object = Nothing
        Dim oCashListItem As Object = Nothing
        Dim oAccount As Object = Nothing
        Dim oBankAccount As Object = Nothing
        Dim oUserAuthorities As Object = Nothing
        Dim oAllocate As Object = Nothing
        Dim oCashListPost As Object = Nothing
        Dim oPartyBank As Object = Nothing

        Try

            SettlePayment = gPMConstants.PMEReturnCode.PMTrue
            'Get the CashList business Object
            oBObjectManager = New bObjectManager.ObjectManager()
            oBObjectManager.Initialise(MainModule.ACApp)

            returnCode = oBObjectManager.GetInstance(oObject:=oCashList, sClassName:="bACTCashList.Form", vInstanceManager:=PMGetViaClientManager)
            If returnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(methodName, "Failed to GetInstance For - bActCashList.Form", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get the CashListItem Business Object

            returnCode = oBObjectManager.GetInstance(oObject:=oCashListItem, sClassName:="bACTCashlistitem.Form", vInstanceManager:=PMGetViaClientManager)
            If returnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(methodName, "Failed to GetInstance For - bActCashListItem.Form", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get the Account Business Object

            returnCode = oBObjectManager.GetInstance(oObject:=oAccount, sClassName:="bACTAccount.Form", vInstanceManager:=PMGetViaClientManager)
            If returnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(methodName, "Failed to GetInstance For - bACTAccount.Form", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get the BankAccount Business Object

            returnCode = oBObjectManager.GetInstance(oObject:=oBankAccount, sClassName:="bACTBankAccount.Form", vInstanceManager:=PMGetViaClientManager)
            If returnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(methodName, "Failed to GetInstance For - bACTBankAccount.Form", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get the UserAuthorities Business Object

            returnCode = oBObjectManager.GetInstance(oObject:=oUserAuthorities, sClassName:="bACTUserAuthorities.Business", vInstanceManager:=PMGetViaClientManager)
            If returnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(methodName, "Failed to GetInstance For - bACTUserAuthorities.Business.Form", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get the UserAuthorities Business Object

            returnCode = oBObjectManager.GetInstance(oObject:=oAllocate, sClassName:="bACTAllocate.Business", vInstanceManager:=PMGetViaClientManager)
            If returnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(methodName, "Failed to GetInstance For - bActAllocate.Business.Form", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Create CashListPost
            If oCashListPost Is Nothing Then

                returnCode = oBObjectManager.GetInstance(oObject:=oCashListPost, sClassName:="bACTCashListPost.Automated", vInstanceManager:=PMGetViaClientManager)
                If returnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(methodName, "Failed to create bACTCashListPost.Automated", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            proceedFurther = True
            proceedToPost = True

            ' Get the required details of Agent
            returnCode = GetCommPaymentDetails(index, agentAccountId, agentAddress, commType, paymentType, statementDate, mediaTypeId, commPaymentAmount, accCurrencyId, documentId, bankAccountId, batchId, agentName)
            If returnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(methodName, "GetCommPaymentDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            returnCode = oBObjectManager.GetInstance(oObject:=oPartyBank, sClassName:="bSIRPartyBank.Business", vInstanceManager:=PMGetViaClientManager)
            If returnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(methodName, "Failed to GetInstance For - bSIRPartyBank.Business.Form", gPMConstants.PMELogLevel.PMLogError)
            End If

            Dim oPartyBankDetails(,) As Object = Nothing
            returnCode = oPartyBank.GetPartyBankDetails(oPartyBankDetails, Nothing, agentAccountId)

            If returnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(methodName, "oPartyBank.GetPartyBankDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            returnCode = oAccount.IsDeleted(agentAccountId, isDeleted)
            If returnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(methodName, "oAccount.IsDeleted Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If isDeleted = True Then
                proceedFurther = False
            End If

            returnCode = GetUserAuthoritiesForPayment(hasPaymentsAuthority, paymentAuthority)
            If returnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(methodName, "GetUserAuthorities Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If hasPaymentsAuthority And paymentAuthority < commPaymentAmount * -1 Then
                proceedToPost = False
                'Create a WorkMAnager Task
            End If

            If proceedFurther Then
                'Add the CashListDetails for payment
                If bankAccountId <> 0 Then
                    returnCode = oCashList.DirectAdd(vCashListID:=cashListId, vCashListStatusID:=1, vCashListTypeID:=1, vCashListRef:="", vCompanyID:=1, vBankAccountID:=bankAccountId, vCurrencyID:=accCurrencyId, vListDate:=ToSafeDate(statementDate), vControlTotal:=0, vItemCount:=0)
                Else
                    returnCode = oCashList.DirectAdd(vCashListID:=cashListId, vCashListStatusID:=1, vCashListTypeID:=1, vCashListRef:="", vCompanyID:=1, vCurrencyID:=accCurrencyId, vListDate:=ToSafeDate(statementDate), vControlTotal:=0, vItemCount:=0)
                End If

                'Add CashListItem Details
                returnCode = PrepareCashListItem(agentAccountId, cashListId, commPaymentAmount * -1, mediaTypeId, bankAccountId, accCurrencyId, cashListItem, agentName, oPartyBankDetails)

                returnCode = oCashListItem.DirectAdd(r_vCashListItem:=cashListItem)

                returnCode = oCashListItem.Update

                cashListItemId = cashListItem(0)
                ReDim keyArray(1, 1)

                keyArray(0, 0) = ACTKeyNameCashListId
                keyArray(1, 0) = cashListId
                keyArray(0, 1) = ACTKeyNameCashListItemId
                keyArray(1, 1) = cashListItemId

                returnCode = oCashListPost.SetKeys(vKeyArray:=keyArray)
                If returnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                    SettlePayment = returnCode
                    RaiseError(methodName, "oCashListPost.SetKeys Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                oCashListPost.ChequeProduction = False
                returnCode = oCashListPost.PostUnallocatedCash(v_vCashListID:=cashListId, sFailureReason:="")
                If returnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                    SettlePayment = returnCode
                    RaiseError(methodName, "oCashListPost.PostUnallocatedCash Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                returnCode = oCashListItem.GetDetails(vCashListID:=cashListId)
                If returnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                    SettlePayment = returnCode
                    RaiseError(methodName, "oCashListItem.GetDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                If returnCode = gPMConstants.PMEReturnCode.PMTrue Then
                    returnCode = oCashListItem.GetNext(cashListItemDetails)
                    transDetailID = cashListItemDetails(eCashListItem.TransdetailID)

                End If

                returnCode = GetDocumentsForAccountBatch(accountId:=agentAccountId, batchID:=batchId, documentIds:=documentTempArray)

                ReDim documentArray(UBound(documentTempArray, 2))

                For rowCount = 0 To UBound(documentTempArray, 2)
                    documentArray(rowCount) = documentTempArray(0, rowCount)
                Next

                If returnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                    SettlePayment = returnCode
                    RaiseError(methodName, "GetDocumentsForAccountBatch Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                returnCode = oAccount.GetAccountOSCommForDocuments(v_lAccountId:=agentAccountId, v_vDocumentIds:=documentArray, r_vOSTransactions:=oOSTransactions)
                If returnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                    SettlePayment = returnCode
                    RaiseError(methodName, "GetAccountOSTransForDocuments Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                If oAllocate.PerformAutoAllocation(r_lAccountid:=agentAccountId, r_lTransdetailid:=transDetailID, v_vOSTransactions:=oOSTransactions, v_lCashlistItemID:=cashListItemId) <> gPMConstants.PMEReturnCode.PMTrue Then
                    If returnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                        SettlePayment = returnCode
                        RaiseError(methodName, "GetAccountOSTransForDocuments Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                Else
                    returnCode = SaveSummary(mediaType, commPaymentAmount * -1)
                End If

            End If

        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=methodName, r_lFunctionReturn:=SettlePayment, excep:=ex)

        Finally
            oAccount = Nothing
            oAllocate = Nothing
            oBankAccount = Nothing
            oCashList = Nothing
            oCashListItem = Nothing
            oUserAuthorities = Nothing
            oCashListPost = Nothing

        End Try
        Return returnCode

    End Function

    Public Function GetCommPaymentDetails(ByVal listItemIndex As Integer, ByRef agentAccountId As Integer, ByRef agentAddress As String, ByRef commType As String,
                                          ByRef paymentType As String, ByRef statementDate As Date, ByRef mediaTypeId As Integer, ByRef commPaymentAmount As Decimal,
                                          ByRef accCurrencyId As Integer, ByRef documentId As Integer, ByRef bankAccountId As Integer, ByRef batchID As Integer,
                                          ByRef agentName As String) As Integer

        Const methodName As String = "GetCommPaymentDetails"
        Dim returnCode As PMEReturnCode = PMEReturnCode.PMTrue
        Dim oAgentData As Object = Nothing
        Dim obACTCommissionPayments As Object = Nothing
        Dim oBObjectManager As bObjectManager.ObjectManager

        Try
            oBObjectManager = New bObjectManager.ObjectManager()
            oBObjectManager.Initialise(MainModule.ACApp)

            agentAccountId = ToSafeLong(_agentSummaryArray(_accountIdRank,listItemIndex))

            returnCode = oBObjectManager.GetInstance(oObject:=obACTCommissionPayments, sClassName:="bACTCommissionPayments.Business", vInstanceManager:=PMGetViaClientManager)
            returnCode = obACTCommissionPayments.GetAgentDetailsforPayments(v_lAccountId:=agentAccountId, v_lSourceId:=oBObjectManager.SourceID, r_vResultArray:=oAgentData)
            If returnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(methodName, "Get Agent Details for Payments Failed", gPMConstants.PMELogLevel.PMLogError)
                GetCommPaymentDetails = returnCode
                Exit Function
            End If

            agentAddress = ToSafeString(oAgentData(_agentAddressRank, 0), CStr(0))
            commType = "Payment"
            paymentType = "Commission"

            mediaTypeId = ToSafeLong(oAgentData(_agentMediaTypeRank, 0), 0)
            commPaymentAmount = ToSafeCurrency(_agentSummaryArray(_totalCommRank, listItemIndex), 0)
            agentName = ToSafeString(_agentSummaryArray(_agentNameRank, listItemIndex), CStr(0))
            accCurrencyId = ToSafeLong(_agentSummaryArray(_currencyIDRank, listItemIndex), 0)
            documentId = 0
            bankAccountId = ToSafeLong(oAgentData(_bankAccountIdRank, 0), 0)
            batchID = _batchId

        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=methodName, r_lFunctionReturn:=GetCommPaymentDetails, excep:=ex)
            Return PMEReturnCode.PMFalse

        Finally
            oAgentData = Nothing
            obACTCommissionPayments = Nothing
            oBObjectManager = Nothing

        End Try
        Return returnCode

    End Function

    Public Function GetDocumentsForAccountBatch(ByVal accountId As Integer, ByVal batchID As Integer, ByRef documentIds As Object) As Integer

        Const methodName As String = "GetDocumentsForAccountBatch"
        Dim returnCode As PMEReturnCode = PMEReturnCode.PMTrue
        Dim oBObjectManager As bObjectManager.ObjectManager
        Dim obACTCommissionPayments As Object = Nothing

        Try
            oBObjectManager = New bObjectManager.ObjectManager()
            oBObjectManager.Initialise(MainModule.ACApp)
            GetDocumentsForAccountBatch = returnCode

            GetDocumentsForAccountBatch = gPMConstants.PMEReturnCode.PMTrue
            returnCode = oBObjectManager.GetInstance(oObject:=obACTCommissionPayments, sClassName:="bACTCommissionPayments.Business", vInstanceManager:=PMGetViaClientManager)
            If (returnCode <> gPMConstants.PMEReturnCode.PMTrue) Then
                GetDocumentsForAccountBatch = returnCode
                RaiseError(methodName, "bACTCommissionPaymentsUnable to create bACTCommissionPayments object", gPMConstants.PMELogLevel.PMLogError)
            End If

            returnCode = obACTCommissionPayments.GetDocumentsForAccountBatch(v_lAccountId:=accountId, v_lBatchID:=batchID, r_vResultArray:=documentIds)
            If (returnCode <> gPMConstants.PMEReturnCode.PMTrue) Then
                GetDocumentsForAccountBatch = returnCode
                RaiseError(methodName, "GetDocumentsForAccountBatch Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=methodName, r_lFunctionReturn:=GetDocumentsForAccountBatch, excep:=ex)

        Finally
            oBObjectManager = Nothing
            obACTCommissionPayments = Nothing

        End Try
        Return returnCode

    End Function

    Private Function PrepareCashListItem(ByVal accountID As Integer, ByVal cashListId As Integer, ByVal amount As Decimal, ByVal mediaTypeId As Short,
                                         ByVal bankAccountId As Short, ByVal currencyId As Short, ByRef cashListItem As Object) As Integer

        Return PrepareCashListItem(accountID, cashListId, amount, mediaTypeId, bankAccountId, currencyId, cashListItem, "", Nothing)
    End Function

    Private Function PrepareCashListItem(ByVal accountID As Integer, ByVal cashListId As Integer, ByVal amount As Decimal, ByVal mediaTypeId As Short,
                                         ByVal bankAccountId As Short, ByVal currencyId As Short, ByRef cashListItem As Object, ByVal payeeName As String) As Integer

        Return PrepareCashListItem(accountID, cashListId, amount, mediaTypeId, bankAccountId, currencyId, cashListItem, payeeName, Nothing)
    End Function

    Private Function PrepareCashListItem(ByVal accountID As Integer, ByVal cashListId As Integer, ByVal amount As Decimal, ByVal mediaTypeId As Short,
                                         ByVal bankAccountId As Short, ByVal currencyId As Short, ByRef cashListItem As Object, ByVal payeeName As String, ByVal vPartyBankDetails(,) As Object) As Integer

        Const methodName As String = "CreateCashListPayment"
        Dim returnCode As PMEReturnCode = PMEReturnCode.PMTrue
        Dim baseCurrencyID As Short
        Dim accountCurrencyID As Short
        Dim systemCurrencyID As Short

        Dim baseCurrentAmount As Decimal
        Dim accountCurrentAmount As Decimal
        Dim systemCurrentAmount As Decimal

        Dim transToBaseExchangeRate As Double
        Dim accountToBaseExchangeRate As Double
        Dim systemToBaseExchangeRate As Double
        Dim obACTCurrencyConvert As Object = Nothing

        Dim oBObjectManager As bObjectManager.ObjectManager

        Try
            oBObjectManager = New bObjectManager.ObjectManager()
            oBObjectManager.Initialise(MainModule.ACApp)
            PrepareCashListItem = returnCode

            ReDim cashListItem(eCashListItem.LastItem)

            returnCode = oBObjectManager.GetInstance(oObject:=obACTCurrencyConvert, sClassName:="bACTCurrencyConvert.Form", vInstanceManager:=PMGetViaClientManager)
            If returnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                PrepareCashListItem = returnCode
                RaiseError("", "Unable to get bACTCurrencyConvert object")
            End If

            returnCode = obACTCurrencyConvert.DoCurrencyConversion(v_lAccountId:=accountID, v_lCompanyID:=oBObjectManager.SourceID, v_iCurrencyID:=currencyId, v_cCurrencyAmountUnrounded:=amount, r_iBaseCurrencyID:=baseCurrencyID, r_cBaseAmount:=baseCurrentAmount, r_iAccountCurrencyID:=accountCurrencyID, r_cAccountAmount:=accountCurrentAmount, r_iSystemCurrencyID:=systemCurrencyID, r_cSystemAmount:=systemCurrentAmount, r_dCurrencyBaseXrate:=transToBaseExchangeRate, r_dtCurrencyBaseDate:=Now, r_dAccountBaseXrate:=accountToBaseExchangeRate, r_dtAccountBaseDate:=Now, r_dSystemBaseXrate:=systemToBaseExchangeRate, r_dtSystemBaseDate:=Now)
            If returnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                PrepareCashListItem = returnCode
                RaiseError("", "Unable to do Currency Conversion")
            End If

            cashListItem(eCashListItem.CashlistitemID) = 0
            cashListItem(eCashListItem.AllocationstatusID) = 1
            cashListItem(eCashListItem.MediaTypeID) = mediaTypeId
            cashListItem(eCashListItem.CashlistID) = cashListId
            cashListItem(eCashListItem.AccountId) = accountID
            cashListItem(eCashListItem.MediaRef) = ""
            cashListItem(eCashListItem.OurRef) = ""
            cashListItem(eCashListItem.TheirRef) = ""
            cashListItem(eCashListItem.Amount) = amount
            cashListItem(eCashListItem.AddressCountry) = "1"
            cashListItem(eCashListItem.Transaction_Date) = Now
            cashListItem(eCashListItem.Amount_Tendered) = -amount
            cashListItem(eCashListItem.CashListItem_Payment_Type_id) = 2 ' For Commission Payment
            cashListItem(eCashListItem.CashListItem_Payment_Status_id) = 1 '
            cashListItem(eCashListItem.CurrencyBaseDate) = Now
            cashListItem(eCashListItem.CurrencyBaseXrate) = transToBaseExchangeRate
            cashListItem(eCashListItem.AccountBaseDate) = Now
            cashListItem(eCashListItem.AccountBaseXrate) = accountToBaseExchangeRate
            cashListItem(eCashListItem.SystemBaseDate) = Now
            cashListItem(eCashListItem.SystemBaseXrate) = systemToBaseExchangeRate
            cashListItem(eCashListItem.OverrideReason) = 0
            cashListItem(eCashListItem.PaymentName) = payeeName

            If Not Information.IsNothing(vPartyBankDetails) AndAlso IsArray(vPartyBankDetails) Then
                If Not Information.IsNothing(vPartyBankDetails(2, 0)) Then
                    cashListItem(eCashListItem.PartyBankId) = vPartyBankDetails(2, 0)
                End If
                cashListItem(eCashListItem.PaymentAccountCode) = vPartyBankDetails(8, 0)
                cashListItem(eCashListItem.PaymentBranchCode) = vPartyBankDetails(11, 0)
                If Not Information.IsNothing(vPartyBankDetails(18, 0)) AndAlso IsArray(vPartyBankDetails(18, 0)) Then
                    cashListItem(eCashListItem.AddressCountry) = vPartyBankDetails(18, 0)(0)
                End If
                cashListItem(eCashListItem.BIC) = vPartyBankDetails(37, 0)
                cashListItem(eCashListItem.IBAN) = vPartyBankDetails(38, 0)
            End If

        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=methodName, r_lFunctionReturn:=PrepareCashListItem, excep:=ex)

        Finally
            obACTCurrencyConvert = Nothing
            oBObjectManager = Nothing

        End Try

    End Function

    Private Function GetUserAuthoritiesForPayment(ByRef hasPaymentsAuthority As Boolean, ByRef paymentsAmount As Decimal) As Integer

        Dim returnCode As PMEReturnCode = PMEReturnCode.PMTrue
        Const methodName As String = "GetUserAuthoritiesForPayment"
        Dim convertedCurrency As Decimal
        Dim paymentsCurrencyID As Short
        Dim obACTUserAuthorities As Object = Nothing
        Dim obACTCurrencyConvert As Object = Nothing
        Dim oBObjectManager As bObjectManager.ObjectManager

        Try
            oBObjectManager = New bObjectManager.ObjectManager()
            oBObjectManager.Initialise(MainModule.ACApp)
            GetUserAuthoritiesForPayment = returnCode

            returnCode = oBObjectManager.GetInstance(oObject:=obACTUserAuthorities, sClassName:="bACTUserAuthorities.Business", vInstanceManager:=PMGetViaClientManager)

            returnCode = obACTUserAuthorities.GetDetails(vUserId:=oBObjectManager.UserID)
            If (returnCode <> gPMConstants.PMEReturnCode.PMTrue) Then
                GetUserAuthoritiesForPayment = returnCode
                RaiseError(methodName, "Unable to get User Authorities Details")
            End If

            returnCode = obACTUserAuthorities.GetNext(vHasPaymentsAuthority:=hasPaymentsAuthority, vPaymentsAmount:=paymentsAmount, vPaymentsCurrencyID:=paymentsCurrencyID)
            If returnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                GetUserAuthoritiesForPayment = returnCode
                RaiseError(methodName, "UserAuthorities.GetNext Failed")
            End If

            If hasPaymentsAuthority = True And oBObjectManager.CurrencyID <> paymentsCurrencyID Then
                returnCode = oBObjectManager.GetInstance(oObject:=obACTCurrencyConvert, sClassName:="bACTCurrencyConvert.Form", vInstanceManager:=PMGetViaClientManager)

                returnCode = obACTCurrencyConvert.CurrencyToCurrencyConversion(v_lCurrencyIdFrom:=paymentsCurrencyID, v_crCurrencyAmountFrom:=paymentsAmount, v_lCompanyID:=g_iSourceID, v_lCurrencyIdTo:=oBObjectManager.CurrencyID, r_crCurrencyAmountTo:=convertedCurrency)
                If returnCode <> gPMConstants.PMEReturnCode.PMTrue Then
                    GetUserAuthoritiesForPayment = returnCode
                    RaiseError(methodName, "CurrencyToCurrencyConversion Failed")
                End If

                paymentsAmount = convertedCurrency
            End If

        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=methodName, r_lFunctionReturn:=GetUserAuthoritiesForPayment, excep:=ex)

        End Try
        Return returnCode

    End Function

    Private Function SaveSummary(ByRef mediaType As String, ByRef amount As Decimal) As Integer

        Const methodName As String = "SaveSummary"
        Dim index As Short
        Dim itemExists As Boolean

        Try

            SaveSummary = gPMConstants.PMEReturnCode.PMTrue

            If IsArray(_summaryArray) Then
                index = UBound(_summaryArray, 2) + 1

                For count As Integer = 0 To UBound(_summaryArray, 2)

                    If _summaryArray(ACListPaymentSummary.PSMediaType, count) = mediaType Then
                        _summaryArray(ACListPaymentSummary.PSPaymentValue, count) = _summaryArray(ACListPaymentSummary.PSPaymentValue, count) + amount
                        _summaryArray(ACListPaymentSummary.PSPaymentCount, count) = _summaryArray(ACListPaymentSummary.PSPaymentCount, count) + 1
                        itemExists = True

                    End If
                Next

                If Not itemExists Then
                    ReDim Preserve _summaryArray(2, index)

                    _summaryArray(ACListPaymentSummary.PSMediaType, index) = mediaType
                    _summaryArray(ACListPaymentSummary.PSPaymentValue, index) = amount
                    _summaryArray(ACListPaymentSummary.PSPaymentCount, index) = 1
                End If
            Else
                ReDim _summaryArray(2, 0)

                _summaryArray(ACListPaymentSummary.PSMediaType, 0) = mediaType
                _summaryArray(ACListPaymentSummary.PSPaymentValue, 0) = amount
                _summaryArray(ACListPaymentSummary.PSPaymentCount, 0) = 1
            End If

        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=methodName, r_lFunctionReturn:=SaveSummary, excep:=ex)

        End Try

    End Function

    ''' <summary>
    ''' Method used to create an XML file in a format defined by Commission_Export.XSD
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub ExportBatch()

        Dim returnCode As PMEReturnCode
        Dim xmlDocument As New XmlDocument
        Dim xmlProcessingInst As Xml.XmlProcessingInstruction ' MSXML2.IXMLDOMProcessingInstruction
        Dim filePath As String
        Dim resultArray(,) As Object = Nothing
        Dim xmlReaderSettings As XmlReaderSettings = New XmlReaderSettings()
        Dim xmlReader As XmlReader = Nothing
        Dim sqlQueryTimeOut As Integer = 0

        filePath = FullPath

        ' Add the parameters required for the SP's execution
        Output("Commission Auto Release In Process...")
        AutoReleaseCommission()

        ' Add the parameters required for the SP's execution
        Output("Retrieving batch...")
        AddParameterLite(m_oDatabase, "batch_id", _batchId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
        AddParameterLite(m_oDatabase, "new_batch", _newBatch, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
        AddParameterLite(m_oDatabase, "lead_days", _leadDays, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
        AddParameterLite(m_oDatabase, "media_type_code", m_sMediaTypeCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        AddParameterLite(m_oDatabase, "bank_account_name", m_sBankAccountName, PMEParameterDirection.PMParamInput, PMEDataType.PMString)

        sqlQueryTimeOut = QueryTimeOut()
        If sqlQueryTimeOut > 0 Then
            m_oDatabase.QueryTimeout = sqlQueryTimeOut
        End If
        ' Execute the SP which will return the XML String and store it in DOMDocument
        returnCode = m_oDatabase.SQLSelectForXML("spu_ACT_CommissionExport_XML_Select", True, xmlDocument)

        ' Check for xml
        If xmlDocument.InnerXml.Length = 0 Then
            OutputLine(String.Format("Batch {0} does not exist", _batchId))
        Else
            OutputLine("Done")
            Output(String.Format("Writing output file...{0}...", Filename))

            ' Create the processing instruction (header) for this file.
            xmlProcessingInst = xmlDocument.CreateProcessingInstruction("xml", "version=""1.0""")
            xmlDocument.InsertBefore(xmlProcessingInst, xmlDocument.FirstChild)

            ' Tidy up the header wrapper
            GenerateSchemaHeader(xmlDocument)

            ' Save XML
            xmlDocument.Save(filePath)
            OutputLine("Done")

            'Validate the XML file against the XSD file
            Try

                OutputLine("Validating Exported XML File Format")

                xmlReaderSettings.Schemas.Add("http://www.siriusfs.com/SFI/Export/Commission_Export/20060420", System.AppDomain.CurrentDomain.BaseDirectory() & "\" & "Commission_Export.xsd")
                xmlReaderSettings.ValidationType = ValidationType.Schema
                xmlReader = XmlReader.Create(filePath, xmlReaderSettings)

                While xmlReader.Read()
                End While
                xmlReader.Close()

                OutputLine("Validation Completed")

            Catch ex As XmlException
                UpdateBatchTask(kBatchStatusFailed, _batchId, filePath, 0, 0)
                OutputLine("Invalid XML File Format")
                Throw New ApplicationException("Export file has an invalid XML File Format", ex)

            Finally
                xmlReader = Nothing
                xmlReaderSettings = Nothing

            End Try

        End If
    End Sub

    Private Function MarkCommissionPayments() As Integer

        Const methodName As String = "MarkCommissionPayments"
        Dim returnCode As PMEReturnCode = PMEReturnCode.PMTrue
        Dim uploadFlag As Boolean

        For count As Integer = LBound(_agentSummaryArray, 2) To UBound(_agentSummaryArray, 2)

            m_oDatabase.Parameters.Clear()

            m_oDatabase.Parameters.Add(sName:="account_Id", vValue:=_agentSummaryArray(0,count), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_oDatabase.Parameters.Add(sName:="session_guid", vValue:=_sessionGUID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            m_oDatabase.Parameters.Add(sName:="batch_id", vValue:=Trim(CStr(_batchId)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If count = 0 Then
                uploadFlag = 1
            Else
                uploadFlag = 0
            End If
            m_oDatabase.Parameters.Add(sName:="bUpdFlag", vValue:=ToSafeBoolean(uploadFlag), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            returnCode = m_oDatabase.SQLAction(sSQL:="spu_ACT_MarkCommissionPaymentsInBatch", sSQLName:="MarkCommissionPaymentsInBatch", bStoredProcedure:=True)
            If (returnCode <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(methodName, "_MarkCommissionPayments SQL Action failed")
            End If
        Next

        Return returnCode

    End Function

    Private Sub GetCurrencyCode()
        Dim returnCode As PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue
        Dim resultArray(,) As Object = Nothing

        Try
            m_oDatabase.Parameters.Clear()
            returnCode = m_oDatabase.SQLSelect(sSQL:="spu_ACT_SelAll_currency", sSQLName:="SelAll_currency", bStoredProcedure:=True, vResultArray:=resultArray, bKeepNulls:=True)

            If returnCode = PMEReturnCode.PMTrue AndAlso Information.IsArray(resultArray) Then
                For count As Integer = LBound(resultArray, 2) To UBound(resultArray, 2)
                    If ToSafeInteger(resultArray(0, count)) = _currencyID Then
                        _currencyCode = ToSafeString(resultArray(2, count))
                    End If
                Next
            End If

        Catch excep As System.Exception
            returnCode = gPMConstants.PMEReturnCode.PMError
            RaiseError("GetCurrencyCode", "GetCurrencyCode SQL Action failed")
        Finally
            resultArray = Nothing
        End Try

    End Sub

    ''' <summary>
    ''' Deallocate objects
    ''' </summary>
    ''' <remarks></remarks>
    Public Overrides Sub CleanUpInterops()
        m_oDatabase = Nothing

    End Sub

#End Region

End Class
