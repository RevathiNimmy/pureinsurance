Imports System.Collections.ObjectModel
Imports System.Math
Imports System.Xml

Friend NotInheritable Class Payment_Import : Inherits ImportBase
#Region "Fields"
    ' Working variables
    Private m_iCashListID As Integer = 0
    Private m_iCashListItemID As Integer = 0
    Private m_iTransDetailID As Integer = 0
    Private m_cBaseAmount As Decimal = 0D
    Private m_nNoOfTotalRecords As Integer
    Private m_nNoOfRejections As Integer
#End Region

#Region "Properties"
    ''' <summary>
    ''' Specifies the batch code for this interface
    ''' </summary>
    Public Overrides ReadOnly Property BatchCode() As String
        Get
            Return "SPYI"
        End Get
    End Property

    ''' <summary>
    ''' Specifies the active interface name
    ''' </summary>
    Public Overrides ReadOnly Property InterfaceName() As String
        Get
            Return "Payment_Import"
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

#Region "Methods"
    ''' <summary>
    ''' Allocates this payment to the supplied transaction
    ''' </summary>
    Private Sub AllocatePayment(ByVal lAllocationTransDetailID As Integer)
        Dim iReturn As Integer
        Dim oAllocationManual As bACTAllocationManual.Business
        Dim vKeys(1, 2) As Object

        Try
            ' Get outstanding amount on allocation transaction
            Dim cOutstanding As Decimal = GetTransactionAmountOS(lAllocationTransDetailID)

            ' Allocation amount is lower of outstanding amount or payment amount
            Dim cAllocateAmount As Decimal = Math.Min(Abs(cOutstanding), Abs(m_cBaseAmount)) * Sign(m_cBaseAmount)

            ' We can't allocate if either amount is zero
            If cAllocateAmount = 0 Then
                Return
            End If

            ' Check payment is opposite of transaction
            ' Note: Do this here, we've already done the zero checks
            If Sign(cOutstanding) = Sign(m_cBaseAmount) Then
                Throw New Exception("Payment amount cannot be same sign as allocation transaction")
            End If

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

            ' Declare trans detail array
            Dim vTransdetailIDs() As String = {String.Format("{0}|{1}", lAllocationTransDetailID, -cAllocateAmount)}

            ' Populate keys array with transactions and cash list item id
            vKeys(PMENavKeys.PMKeyName, 0) = ACTKeyNameTransDetailID
            vKeys(PMENavKeys.PMKeyValue, 0) = String.Format("{0}|{1}", m_iTransDetailID, cAllocateAmount)
            vKeys(PMENavKeys.PMKeyName, 1) = ACTKeyNameTransDetailIDs
            vKeys(PMENavKeys.PMKeyValue, 1) = vTransdetailIDs
            vKeys(PMENavKeys.PMKeyName, 2) = ACTKeyNameCashListItemId
            vKeys(PMENavKeys.PMKeyValue, 2) = m_iCashListItemID

            ' Set process mode
            iReturn = oAllocationManual.SetProcessModes(vTask:=PMEComponentAction.PMEdit)
            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("oAllocationManual.SetProcessModes failed.")
            End If

            ' Set the keys
            iReturn = oAllocationManual.SetKeys(vKeyArray:=vKeys)
            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("oAllocationManual.SetKeys failed.")
            End If

            ' Start it
            iReturn = oAllocationManual.Start()
            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("oAllocationManual.Start failed.")
            End If

        Catch ex As Exception
            Throw New Exception("Failed to allocate payment", ex)

        Finally
            ' Release business object
            CloseBusiness(oAllocationManual)

        End Try
    End Sub

    ''' <summary>
    ''' Creates a cashlist for this transaction based on the band account and currency code.
    ''' If a cashlist matching the given criteria already exists then it's id is returned.
    ''' </summary>
    Private Sub CreateCashList()
        Dim iReturn As PMEReturnCode

        Dim sBankAccount As String = GetAttribute("bankaccount_code")
        Dim sCurrencyCode As String = GetAttribute("currency_code")

        Try
            ' Add parameters
            AddParameterLite(m_oDatabase, "batch_id", m_iBatchID, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
            AddParameterLite(m_oDatabase, "bankaccount_code", sBankAccount, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "currency_code", sCurrencyCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "username", GetAttribute("user_name"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "cashlist_id", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)

            ' Execute sql
            iReturn = m_oDatabase.SQLAction("spu_ACT_Import_Payment_CreateCashList", "Create Cash List", True)
            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spu_ACT_Import_Payment_CreateCashList'")
            End If

            ' Get cashlist id
            m_iCashListID = m_oDatabase.Parameters.Item("cashlist_id").Value
        Catch ex As Exception
            Throw New Exception("Unable to get or create cash list", ex)
        Finally
            ' Check for successful creation of cashlist
            If m_iCashListID = 0 Then
                Throw New Exception(String.Format("Unable to get or create cash list for bank account '{0}' and currency '{1}'", sBankAccount, sCurrencyCode))
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Creates a cashlist item for this payment
    ''' </summary>
    Private Sub CreateCashListItem()
        Dim iReturn As PMEReturnCode

        Try
            ' Add parameters
            AddParameterLite(m_oDatabase, "cashlistitem_id", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong, True)
            AddParameterLite(m_oDatabase, "mediatype_code", GetAttribute("mediatype_code"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "cashlist_id", m_iCashListID, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "account_code", GetAttribute("account_code"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "media_ref", GetAttribute("cheque_number"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "amount", GetAttribute("amount"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "contact_name", GetAttribute("contact_name"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "address1", GetAttribute("address1"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "address2", GetAttribute("address2"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "address3", GetAttribute("address3"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "address4", GetAttribute("address4"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "postal_code", GetAttribute("postal_code"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "country_code", GetAttribute("country_code"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "payment_name", GetAttribute("payment_name"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "payment_account_code", GetAttribute("payment_account_code"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "payment_branch_code", GetAttribute("payment_branch_code"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "payment_expiry_date", GetAttribute("payment_expiry_date"), PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
            AddParameterLite(m_oDatabase, "payment_reference1", GetAttribute("payment_reference1"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "payment_reference2", GetAttribute("payment_reference2"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "batch_id", m_iBatchID, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "username", GetAttribute("user_name"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "transaction_date", GetAttribute("transaction_date"), PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
            AddParameterLite(m_oDatabase, "cheque_date", GetAttribute("cheque_date"), PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
            AddParameterLite(m_oDatabase, "cc_number", GetAttribute("cc_number"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "cc_expiry_date", GetAttribute("cc_expiry_date"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "cc_start_date", GetAttribute("cc_start_date"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "cc_issue", GetAttribute("cc_issue"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "cc_pin", GetAttribute("cc_pin"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "cc_auth_code", GetAttribute("cc_auth_code"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "payment_type_code", GetAttribute("payment_type_code"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "mediatype_issuer_code", GetAttribute("mediatype_issuer_code"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "cc_name", GetAttribute("cc_name"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "cc_customer", GetAttribute("cc_customer"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "cc_manual_auth_code", GetAttribute("cc_manual_auth_code"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "cc_transaction_code", GetAttribute("cc_transaction_code"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "sBusinessIdentifierCode", GetAttribute("sepa_payment_bic"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "sInternationalBankAccountNumber", GetAttribute("sepa_payment_iban"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)

            ' Execute sql
            iReturn = m_oDatabase.SQLAction("spu_ACT_Import_Payment_CreateCashListItem", "Create Cash List Item", True)
            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spu_ACT_Import_Payment_CreateCashListItem'")
            End If

            ' Get cashlist id
            m_iCashListItemID = m_oDatabase.Parameters.Item("cashlistitem_id").Value
        Catch ex As Exception
            Throw New Exception("Unable to create cash list item", ex)
        Finally
            ' Check for successful creation of cash list item
            If m_iCashListItemID = 0 Then
                Throw New Exception("Unable to create cash list item")
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Gets the outstanding transaction amount for the given transaction
    ''' </summary>
    Public Function GetTransactionAmountOS(ByVal lAllocationTransDetailID As Integer) As Decimal
        Dim iReturn As PMEReturnCode
        Dim vResults(,) As Object = Nothing

        Try
            AddParameterLite(m_oDatabase, "transdetail_id", lAllocationTransDetailID, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)

            ' Execute sql
            iReturn = m_oDatabase.SQLSelect("spu_ACT_Select_TransDetail", "Get TransDetail", True, vResultArray:=vResults, bKeepNulls:=True)
            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spu_ACT_Select_TransDetail'")
            End If

            ' Check outstanding amount
            If vResults(48, 0).Equals(DBNull.Value) Then
                Return 0 ' No outstanding amount specified so assume fully allocated for safety
            Else
                Return vResults(48, 0)  ' Return outstanding amount
            End If
        Catch ex As Exception
            Throw New Exception("Unable to check transdetail outstanding amount", ex)
        End Try
    End Function

    ''' <summary>
    ''' Posts the cashlist item for this payment
    ''' </summary>
    Public Sub PostCashListItem()
        Dim iReturn As PMEReturnCode
        Dim oBusiness As bACTCashListPost.Automated
        Dim cBaseAmount As Decimal

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
            iReturn = oBusiness.PostUnallocatedCash( _
                v_vCashListID:=m_iCashListID, _
                v_vCashListItemID:=m_iCashListItemID, _
                v_vBatchId:=m_iBatchID, _
                r_cBaseAmount:=cBaseAmount)
            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to post unallocated cash list item")
            End If

            ' Get the trans detail from our cash list item
            m_iTransDetailID = oBusiness.CashTransId
            m_cBaseAmount = cBaseAmount
        Catch ex As Exception
            Throw New Exception("Unable to post cash list item", ex)
        Finally
            CloseBusiness(oBusiness)
        End Try

    End Sub

    ''' <summary>
    ''' Process an individual payment item
    ''' </summary>
    Protected Overrides Sub ProcessElement()
        Try
            NoOfTotalRecords += 1
            ' Get the cash list for this item
            CreateCashList()

            ' Create the cash list item
            CreateCashListItem()

            ' Post the cash list and get base amount
            PostCashListItem()

            ' Check if we should allocate this payment
            Dim lAllocationTransDetailID As Integer = GetAttribute("allocation_transdetail_id")
            If Not lAllocationTransDetailID.Equals(0) Then
                ' Allocate payment
                AllocatePayment(lAllocationTransDetailID)
            End If
        Catch ex As Exception
            NoOfRejections += 1
            Throw New Exception("Unable to process payment", ex)
        End Try
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

