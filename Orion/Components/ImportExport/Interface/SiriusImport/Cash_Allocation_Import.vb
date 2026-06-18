Option Explicit On
Option Strict On

Friend NotInheritable Class Cash_Allocation_Import : Inherits ImportBase 
#Region " Private Fields"
    ' Working variables
    Private m_nCashListID As Integer = 0
    Private m_nCashListItemID As Integer = 0
    Private m_nTransDetailID As Integer = 0
    Private m_nAccountID As Integer = 0
    Private m_crBaseAmount As Decimal = 0D
    Private m_dtTransactionDate As DateTime = DateTime.Now
    Private m_nInsuranceFileCnt As Integer = 0
    Private m_oInstalmentsDetails As Object = Nothing
    Private m_oOSTransactions(,) As Object = Nothing
    Private m_crOSBaseBalance As Decimal = 0D
    Private m_nCurrencyId As Integer = 0
    Private m_nBankAccountId As Integer = 0
    Private sBankAccount As String
    Private sUserName As String
    Private nCashListTypeID As Integer
    Private m_sDocumentRef As String
    Private m_nNoOfTotalRecords As Integer
    Private m_nNoOfRejections As Integer
#End Region

#Region "Public Properties"
    ''' <summary>
    ''' Specifies the batch code for this interface
    ''' </summary>
    Public Overrides ReadOnly Property BatchCode() As String
        Get
            Return "CAALLOC"
        End Get
    End Property

    ''' <summary>
    ''' Specifies the active interface name
    ''' </summary>
    Public Overrides ReadOnly Property InterfaceName() As String
        Get
            Return "Cash_Allocation_Import"
        End Get
    End Property

    ''' <summary>
    ''' Indicates if this interface supports bulk imports
    ''' </summary>
    Public Overrides ReadOnly Property SupportsBulkImport() As Boolean
        Get
            Return False
        End Get
    End Property

    ''' <summary>
    ''' Specifies the number of records in batch for this Class
    ''' </summary>
    ''' <returns></returns>
    Public Overrides Property NoOfTotalRecords() As Integer
        Get
            Return m_nNoOfTotalRecords
        End Get
        Set(ByVal value As Integer)
            m_nNoOfTotalRecords = value
        End Set
    End Property

    ''' <summary>
    ''' Specifies the no of rejected records in the batch for this Class
    ''' </summary>
    ''' <returns></returns>
    Public Overrides Property NoOfRejections() As Integer
        Get
            Return m_nNoOfRejections
        End Get
        Set(ByVal value As Integer)
            m_nNoOfRejections = value
        End Set
    End Property

#End Region

#Region " Private Methods"

    ''' <summary>
    ''' Allocates this receipt to the supplied transaction
    ''' </summary>
    Private Sub AllocateCash()

        Dim nReturn As Integer
        Dim oAllocationManual As bACTAllocationManual.Business
        Dim nChildCount As Integer
        Dim iCnt As Integer
        Dim nNumberOfRecords As Integer
        Dim oResult As Object = Nothing
        Dim sWriteOffReasonCode As String
        Dim nWriteOffReasonId As Integer
        Dim dtTransactionDate As DateTime


        ' Try to get the transaction date, if it's not there use the header date
        Try
            dtTransactionDate = DateTime.Parse(GetAttribute("transaction_date").ToString())
        Catch ex As Exception
            dtTransactionDate = m_dtTransactionDate
        End Try

        Try
            nReturn = nReturn >> 2
            ' Create allocation business object
            oAllocationManual = New bACTAllocationManual.Business
            oAllocationManual.Initialise( _
                    sUsername:="", _
                    sPassword:="", _
                    iUserID:=1, _
                    iSourceID:=1, _
                    iLanguageID:=1, _
                    iCurrencyID:=26, _
                    iLogLevel:=PMELogLevel.PMLogError, _
                    sCallingAppName:=ACApp, _
                    vDatabase:=m_oDatabase)
           
            nChildCount = m_oElement.ChildNodes.Count
            ' Declare trans detail array
            Dim oTransdetailIDs(nChildCount - 1) As String

            ' Loop OS array to create trans detail array

            For iCnt = 0 To nChildCount - 1
                If Convert.ToBoolean(m_oElement.ChildNodes(iCnt).Attributes("writeoff").Value) = False Then
                    oTransdetailIDs(iCnt) = String.Format("{0}|{1}|{2}|{3}|{4}", _
                        m_oElement.ChildNodes(iCnt).Attributes("transdetail_id").Value, _
                            m_oElement.ChildNodes(iCnt).Attributes("amount").Value, _
                                m_oElement.ChildNodes(iCnt).Attributes("transdetailex_id").Value, _
                                0, 0)
                Else
                    If m_oElement.ChildNodes(iCnt).Attributes("wo_reason_code") Is Nothing Then
                        SetAttribute("error_indicator", "FAIL")
                        SetAttribute("error_message", "WriteOff reason missing..")
                        Throw New Exception("WriteOff reason missing.")
                    Else
                        sWriteOffReasonCode = m_oElement.ChildNodes(iCnt).Attributes("wo_reason_code").Value

                        AddParameterLite(m_oDatabase, "table_name", "Write_Off_Reason", PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)
                        AddParameterLite(m_oDatabase, "code", sWriteOffReasonCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)

                        nReturn = m_oDatabase.SQLSelect("spu_SIR_Get_Id_From_Code", "Get Id for code", True, nNumberOfRecords, oResult)

                        If nReturn <> PMEReturnCode.PMTrue Then
                            SetAttribute("error_indicator", "FAIL")
                            SetAttribute("error_message", "Invalid WriteOff reason.")
                            Throw New Exception("Invalid WriteOff reason.")
                        End If
                        If IsArray(oResult) Then
                            nWriteOffReasonId = Convert.ToInt32(DirectCast(oResult, Object(,))(0, 0))
                        Else
                            SetAttribute("error_indicator", "FAIL")
                            SetAttribute("error_message", "Invalid WriteOff reason.")
                            Throw New Exception("Invalid WriteOff reason.")
                        End If
                    End If

                    oTransdetailIDs(iCnt) = String.Format("{0}|{1}|{2}|{3}|{4}", _
                        m_oElement.ChildNodes(iCnt).Attributes("transdetail_id").Value, _
                            m_oElement.ChildNodes(iCnt).Attributes("amount").Value, _
                                m_oElement.ChildNodes(iCnt).Attributes("transdetailex_id").Value, _
                                    nWriteOffReasonId, _
                                    m_oElement.ChildNodes(iCnt).Attributes("wo_amount").Value)
                End If
            Next iCnt

            Dim aKeys(1, 3) As Object
            ' Populate keys array with transactions and cash list item id
            aKeys(PMENavKeys.PMKeyName, 0) = ACTKeyNameTransDetailID
            aKeys(PMENavKeys.PMKeyValue, 0) = String.Format("{0}|{1}", m_nTransDetailID, m_crBaseAmount)
            aKeys(PMENavKeys.PMKeyName, 1) = ACTKeyNameTransDetailIDs
            aKeys(PMENavKeys.PMKeyValue, 1) = oTransdetailIDs
            aKeys(PMENavKeys.PMKeyName, 2) = ACTKeyNameCashListItemId
            aKeys(PMENavKeys.PMKeyValue, 2) = m_nCashListItemID
            aKeys(PMENavKeys.PMKeyName, 3) = ACTKeyNameTransactionDate
            aKeys(PMENavKeys.PMKeyValue, 3) = dtTransactionDate

            ' Set process mode
            nReturn = oAllocationManual.SetProcessModes(vTask:=PMEComponentAction.PMEdit)
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("oAllocationManual.SetProcessModes failed.")
            End If

            ' Set the keys
            nReturn = oAllocationManual.SetKeys(vKeyArray:=aKeys)
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("oAllocationManual.SetKeys failed.")
            End If

            ' Start it
            nReturn = oAllocationManual.Start()
            If nReturn <> PMEReturnCode.PMTrue Then
                SetAttribute("error_indicator", "FAIL")
                SetAttribute("error_message", "Allocation failed.")
                Throw New Exception("oAllocationManual.Start failed.")
            End If

        Catch ex As Exception
            Throw New Exception("Failed to allocate receipt", ex)

        Finally
            ' Release allocation object
            oAllocationManual.Dispose()

        End Try
    End Sub

    ''' <summary>
    ''' Creates a cashlist for this transaction based on the band account and currency code.
    ''' If a cashlist matching the given criteria already exists then it's id is returned so
    ''' the new cashlistitem can be appended to it's existing items.
    ''' </summary>
    Private Sub CreateCashList()
        Dim nReturn As Integer
        Dim crCashAmount As Decimal = Util.ToSafeDecimal(GetAttribute("cash_amount"), 0)

        sBankAccount = GetAttribute("bankaccount_code").ToString
        Dim sCurrencyCode As String = GetAttribute("currency_code").ToString

        Dim nNumberOfRecords As Integer
        Dim oResult As Object = Nothing

        Try
            If crCashAmount > 0 Then
                nCashListTypeID = 2
            Else
                nCashListTypeID = 1
            End If

            ' Add parameters
            AddParameterLite(m_oDatabase, "batch_id", m_iBatchID, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
            AddParameterLite(m_oDatabase, "bankaccount_code", sBankAccount, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "currency_code", sCurrencyCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "username", sUserName, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "cashlisttype", nCashListTypeID, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "cashlist_id", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)

            ' Execute sql
            nReturn = m_oDatabase.SQLAction("spu_ACT_Import_CreateCashList", "Create Cash List", True)
            If nReturn <> PMEReturnCode.PMTrue Then
                SetAttribute("error_indicator", "FAIL")
                SetAttribute("error_message", "Unable to create cashlist")
                Throw New Exception("Unable to execute 'spu_ACT_Import_Receipt_CreateCashList'")
            End If

            ' Get cashlist id
            m_nCashListID = Util.ToSafeInt(m_oDatabase.Parameters.Item("cashlist_id").Value, 0)

            AddParameterLite(m_oDatabase, "table_name", "Currency", PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)
            AddParameterLite(m_oDatabase, "code", sCurrencyCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)

            nReturn = m_oDatabase.SQLSelect("spu_SIR_Get_Id_From_Code", "Get Id for code", True, nNumberOfRecords, oResult)

            If nReturn <> PMEReturnCode.PMTrue Then
                SetAttribute("error_indicator", "FAIL")
                SetAttribute("error_message", "Unable to get currency id")
                Throw New Exception("Unable to get currency Id")
            End If
            If (oResult) IsNot Nothing AndAlso IsArray(oResult) Then
                m_nCurrencyId = Convert.ToInt32(DirectCast(oResult, Object(,))(0, 0))
            End If

            AddParameterLite(m_oDatabase, "table_name", "BankAccount", PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)
            AddParameterLite(m_oDatabase, "field_to_return_name", "account_id", PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "field_to_validate_name", "bank_account_name", PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "field_to_validate_value", sBankAccount, PMEParameterDirection.PMParamInput, PMEDataType.PMString)

            nReturn = m_oDatabase.SQLSelect("spu_SAM_Get_And_Validate_Field", "Get Account Id for bank Account name", True, nNumberOfRecords, oResult)

            If nReturn <> PMEReturnCode.PMTrue Then
                SetAttribute("error_indicator", "FAIL")
                SetAttribute("error_message", "Unable to get bank account id")
                Throw New Exception("Unable to get bank account code Id")
            End If
            If (oResult) IsNot Nothing AndAlso IsArray(oResult) Then
                m_nBankAccountId = Convert.ToInt32(DirectCast(oResult, Object(,))(0, 0))
            End If

        Catch ex As Exception
            Throw New Exception("Unable to get or create cash list", ex)
        Finally
            ' Check for successful creation of cashlist
            If m_nCashListID = 0 Then
                Throw New Exception(String.Format("Unable to get or create cash list for bank account '{0}' and currency '{1}'", sBankAccount, sCurrencyCode))
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Creates a cashlist item for this receipt'''''
    ''' </summary>
    Private Sub CreateCashListItem()
        Dim nReturn As Integer
        Dim dtTransactionDate As DateTime

        ' Try to get the transaction date, if it's not there use the header date
        Try
            dtTransactionDate = DateTime.Parse(GetAttribute("transaction_date").ToString())
        Catch ex As Exception
            dtTransactionDate = m_dtTransactionDate
        End Try

        Try
            ' Add parameters
            AddParameterLite(m_oDatabase, "cashlistitem_id", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong, True)
            AddParameterLite(m_oDatabase, "account_id", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "batch_id", m_iBatchID, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "cashlist_id", m_nCashListID, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "cash_type_code", GetAttribute("cash_type_code"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "mediatype_code", GetAttribute("mediatype_code"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "account_code", GetAttribute("account_code"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "transaction_date", dtTransactionDate, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
            AddParameterLite(m_oDatabase, "amount", GetAttribute("cash_amount"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "media_ref", GetAttribute("media_reference"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "our_ref", GetAttribute("our_reference"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "their_ref", GetAttribute("their_reference"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "contact_name", GetAttribute("contact_name"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "address1", GetAttribute("address1"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "address2", GetAttribute("address2"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "address3", GetAttribute("address3"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "address4", GetAttribute("address4"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "postal_code", GetAttribute("postal_code"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "country_code", GetAttribute("country_code"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "payment_name", GetAttribute("cheque_name"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "cheque_date", GetAttribute("cheque_date"), PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
            AddParameterLite(m_oDatabase, "cc_name", GetAttribute("cc_name"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "cc_number", GetAttribute("cc_number"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "cc_expiry_date", GetAttribute("cc_expiry_date"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "cc_start_date", GetAttribute("cc_start_date"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "cc_issue", GetAttribute("cc_issue"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "cc_pin", GetAttribute("cc_pin"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "cc_auth_code", GetAttribute("cc_auth_code"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "cc_manual_auth_code", GetAttribute("cc_manual_auth_code"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "cc_transaction_code", GetAttribute("cc_transaction_code"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "cc_customer", GetAttribute("cc_customer"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "username", sUserName, PMEParameterDirection.PMParamInput, PMEDataType.PMString)

            ' Execute sql
            nReturn = m_oDatabase.SQLAction("spu_ACT_Import_CreateCashListItem", "Create Cash List Item", True)
            If nReturn <> PMEReturnCode.PMTrue Then
                SetAttribute("error_indicator", "FAIL")
                SetAttribute("error_message", "Unable to create cashlist")
                Throw New Exception("Unable to execute 'spu_ACT_Import_Receipt_CreateCashListItem'")
            End If

            ' Get cashlist id
            m_nCashListItemID = Util.ToSafeInt(m_oDatabase.Parameters.Item("cashlistitem_id").Value, 0)
            m_nAccountID = Util.ToSafeInt(m_oDatabase.Parameters.Item("account_id").Value, 0)

        Catch ex As Exception
            Throw New Exception("Unable to create cash list item", ex)
        Finally
            ' Check for successful creation of cash list item
            If m_nCashListItemID = 0 Then
                Throw New Exception("Unable to create cash list item")
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Gets the outstanding transaction amount for the given transaction
    ''' </summary>
    Public Overloads Sub GetAccountAmountOS()
        Dim nReturn As Integer
        Dim oBusiness As bACTAccount.Form
        Dim oTransaction As Object(,) = Nothing

        Try
            ' Create business object
            oBusiness = New bACTAccount.Form
            oBusiness.Initialise( _
                sUsername:="", _
                sPassword:="", _
                iUserID:=1, _
                iSourceID:=1, _
                iLanguageID:=1, _
                iCurrencyID:=26, _
                iLogLevel:=PMELogLevel.PMLogError, _
                sCallingAppName:=ACApp, _
                vDatabase:=m_oDatabase)


            ' Get the outstanding base amount and transactions
            nReturn = oBusiness.GetAccountOSTransactions( _
                    vAccount_id:=DirectCast(m_nAccountID, Object), _
                    vOSTransactions:=oTransaction, _
                    r_cAccountBaseBalance:=m_crOSBaseBalance)

            If nReturn = PMEReturnCode.PMTrue Then
                m_oOSTransactions = DirectCast(oTransaction, [Object](,))
            End If

        Catch ex As Exception
            Throw New Exception("Unable to check account outstanding amount", ex)

        Finally
            ' Release business object
            oBusiness.Dispose()

        End Try
    End Sub

    ''' <summary>
    ''' Gets the outstanding transaction amount for the given transaction
    ''' </summary>
    Public Overloads Sub GetAccountAmountOS(ByVal nInsuranceFileCnt As Integer)
        Dim nReturn As Integer
        Dim nNumberOfRecords As Integer = 0

        Try

            ' Add parameters
            AddParameterLite(m_oDatabase, "insurance_file_cnt", nInsuranceFileCnt, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
            AddParameterLite(m_oDatabase, "account_code", GetAttribute("account_code"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "company_id", DBNull.Value, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            AddParameterLite(m_oDatabase, "base_balance", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMCurrency)
            AddParameterLite(m_oDatabase, "base_currency_count", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMInteger)
            AddParameterLite(m_oDatabase, "account_balance", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMDouble)
            AddParameterLite(m_oDatabase, "account_currency_count", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMCurrency)

            Dim oTransaction As Object = Nothing

            ' Execute sql
            nReturn = m_oDatabase.SQLSelect("spu_ACT_Select_Trans_For_Allocation_FilterBy_Ins_File", "Select Trans for allocation by Ins File", True, nNumberOfRecords, oTransaction)
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spu_ACT_Select_Trans_For_Allocation_FilterBy_Ins_File for insurance file record - " & m_nInsuranceFileCnt & "'")
            End If

            If nNumberOfRecords > 0 Then
                m_oOSTransactions = CType(oTransaction, [Object](,))
            End If
            m_crOSBaseBalance = Util.ToSafeDecimal(m_oDatabase.Parameters.Item("base_balance").Value, 0)

        Catch ex As Exception
            Throw New Exception("Unable to check account outstanding amount", ex)

        End Try
    End Sub

    ''' <summary>
    ''' Posts the cashlist item for this receipt
    ''' </summary>
    Public Sub PostCashListItem()
        Dim nReturn As Integer
        Dim oBusiness As bACTCashListPost.Automated
        Dim crBaseAmount As Decimal
        Dim dtTransactionDate As DateTime

        ' Try to get the transaction date, if it's not there use the header date
        Try
            dtTransactionDate = DateTime.Parse(GetAttribute("transaction_date").ToString())
        Catch ex As Exception
            dtTransactionDate = m_dtTransactionDate
        End Try
        Try
            ' Create the business object
            oBusiness = New bACTCashListPost.Automated
            oBusiness.Initialise( _
                sUsername:="", _
                sPassword:="", _
                iUserID:=1, _
                iSourceID:=1, _
                iLanguageID:=1, _
                iCurrencyID:=26, _
                iLogLevel:=PMELogLevel.PMLogError, _
                sCallingAppName:=ACApp, _
                vDatabase:=m_oDatabase)

            ' Post the unallocated cash
            nReturn = oBusiness.PostUnallocatedCash( _
                v_vCashListID:=m_nCashListID, _
                v_vCashListItemID:=m_nCashListItemID, _
                v_vBatchId:=m_iBatchID, _
                r_cBaseAmount:=crBaseAmount, _
                v_dTransactionDate:=dtTransactionDate)
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to post unallocated cash list item")
            End If

            ' Get the trans detail and base amount from our cash list item
            m_nTransDetailID = oBusiness.CashTransId
            m_crBaseAmount = crBaseAmount
            m_sDocumentRef = oBusiness.DocumentRef

            SetAttribute("cash_transdetail_id", m_nTransDetailID)
            SetAttribute("document_ref", m_sDocumentRef)
        Catch ex As Exception
            Throw New Exception("Unable to post cash list item", ex)

        Finally
            oBusiness.Dispose()

        End Try

    End Sub
    ''' <summary>
    ''' ProcessHeader
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub ProcessHeader()

        ' First get the transaction date. We don't use it in the header but if it's 
        ' not supplied for a transaction line we will use the date supplied here.
        m_dtTransactionDate = Util.ToSafeDate(GetAttribute("date"), DateTime.Now)
        sUserName = GetAttribute("user_name").ToString
    End Sub

    ''' <summary>
    ''' Process an individual receipt item
    ''' </summary>
    Protected Overrides Sub ProcessElement()

        Dim nChildCount As Integer
        Dim iCnt As Integer
        Dim crAmount As Decimal = 0D
        Dim crCashamount As Decimal = Convert.ToDecimal(GetAttribute("cash_amount"))
        Dim crWriteoffamount As Decimal = 0D

        Try
            nChildCount = m_oElement.ChildNodes.Count
            NoOfTotalRecords += 1
            ' Process each reference
            For iCnt = 0 To nChildCount - 1
                crAmount += Convert.ToDecimal(m_oElement.ChildNodes(iCnt).Attributes("amount").Value)
                If Convert.ToBoolean(m_oElement.ChildNodes(iCnt).Attributes("writeoff").Value) = True Then
                    'check if any available for write-off
                    crWriteoffamount += Convert.ToDecimal(m_oElement.ChildNodes(iCnt).Attributes("wo_amount").Value)
                End If
            Next iCnt

            If crCashamount = crAmount - crWriteoffamount Then
                ' Create the cash list
                CreateCashList()
                ' Create the cash list item
                CreateCashListItem()

                ' Post the cash list and get base amount
                PostCashListItem()

                ' Allocate the receipt
                AllocateCash()

                SetAttribute("error_indicator", "SUCCESS")
                SetAttribute("error_message", "")
            Else
                NoOfRejections += 1
                SetAttribute("error_indicator", "FAIL")
                SetAttribute("error_message", "Record not processed.  The Debit and Credit do not match.")
            End If

        Catch ex As Exception
            Throw New Exception("Unable to process Receipt", ex)
        End Try
    End Sub
    ''' <summary>
    ''' FinaliseImport
    ''' </summary>
    ''' <remarks></remarks>
    Protected Overrides Sub FinaliseImport()
        ' The import is complete set the header status to "POSTED"
        SetAttribute("status", "POSTED")
    End Sub
    ''' <summary>
    ''' Update batch Status
    ''' </summary>
    Protected Overrides Sub UpdateBatchStatus()
        UpdateImportBatchStatus(kBatchStatusComplete, NoOfTotalRecords, NoOfRejections)
    End Sub

#End Region

#Region "Creator"
    Public Sub New(ByVal oXML As XmlDocument)
        MyBase.New(oXML)
    End Sub
#End Region
End Class
