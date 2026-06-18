Option Explicit On
Option Strict Off

Imports System.Collections.ObjectModel
Imports System.Math
Imports System.Xml
Imports SharedFiles
Imports System.Text

Friend NotInheritable Class Receipt_Import : Inherits ImportBase
#Region "Fields"
    ' Working variables
    Private m_iCashListID As Integer = 0
    Private m_iCashListItemID As Integer = 0
    Private m_iTransDetailID As Integer = 0
    Private m_iAccountID As Integer = 0
    Private m_cBaseAmount As Decimal = 0D
    Private m_dtTransactionDate As DateTime = DateTime.Now
    Private m_iInsuranceFileCnt As Integer = 0
    Private m_oInstalmentsDetails As Object
    Private m_vOSTransactions(,) As Object = Nothing
    Private m_cOSBaseBalance As Decimal = 0D
    Private m_iCurrencyId As Integer
    Private m_iBankAccountId As Integer
    Private m_sBIC As String = String.Empty
    Private m_sIBAN As String = String.Empty
    Dim sBankAccount As String
    Private m_nNoOfTotalRecords As Integer
    Private m_nNoOfRejections As Integer
    Private m_bTPPFSettlement As Boolean = False
    Private m_sInstalmentPlanRef As String = ""

#End Region

#Region "Properties"
    ''' <summary>
    ''' Specifies the batch code for this interface
    ''' </summary>
    Public Overrides ReadOnly Property BatchCode() As String
        Get
            Return "SRPI"
        End Get
    End Property

    ''' <summary>
    ''' Specifies the active interface name
    ''' </summary>
    Public Overrides ReadOnly Property InterfaceName() As String
        Get
            Return "Receipt_Import"
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

#Region "Methods"
    ''' <summary>
    ''' This Method is used to Sort the given array 
    ''' </summary>
    Public Function Sort2DArray(ByRef r_oUnSortedArray As Object, ByVal o_nSortByColumn As Integer,
                                Optional ByVal o_bSortAscending As Boolean = True) As Integer
        Dim nResult As Integer = PMEReturnCode.PMFalse

        If Not IsArray(r_oUnSortedArray) Then
            Return nResult
        End If

        Dim nTotalRecords As Integer = UBound(r_oUnSortedArray, 2)
        Dim nTotalColumns As Integer = UBound(r_oUnSortedArray, 1)
        Dim iRecord As Integer
        Dim iRecordInner As Integer
        Dim nColumn As Integer
        Dim iFoundIndex As Integer
        Dim crSearchValue As Decimal = 0.0
        Dim aSwapArray(nTotalColumns, 0)

        Try
            For iRecord = 0 To nTotalRecords
                iFoundIndex = -1

                If IsNumeric(SharedFiles.ToSafeDecimal(r_oUnSortedArray(o_nSortByColumn, iRecord))) Then
                    crSearchValue = SharedFiles.ToSafeDecimal(r_oUnSortedArray(o_nSortByColumn, iRecord))
                    For iRecordInner = iRecord + 1 To nTotalRecords

                        'here check the system for Sorting flag  if true then Sort Ascending else decending logic will be triggred
                        If IIf(o_bSortAscending, SharedFiles.ToSafeDecimal(r_oUnSortedArray(o_nSortByColumn, iRecordInner)) < crSearchValue, SharedFiles.ToSafeDecimal(r_oUnSortedArray(o_nSortByColumn, iRecordInner)) > crSearchValue) Then
                            crSearchValue = r_oUnSortedArray(o_nSortByColumn, iRecordInner)
                            iFoundIndex = iRecordInner
                        End If

                    Next

                    If iFoundIndex <> -1 Then
                        For nColumn = 0 To nTotalColumns
                            aSwapArray(nColumn, 0) = r_oUnSortedArray(nColumn, iRecord)
                            r_oUnSortedArray(nColumn, iRecord) = r_oUnSortedArray(nColumn, iFoundIndex)
                            r_oUnSortedArray(nColumn, iFoundIndex) = aSwapArray(nColumn, 0)
                        Next
                    End If
                End If
            Next
            nResult = PMEReturnCode.PMTrue
            Return nResult

        Catch ex As Exception
            nResult = gPMConstants.PMEReturnCode.PMFalse
            Throw New Exception("Failed to Sort 2DArray", ex)
            Return nResult
        Finally
            aSwapArray = Nothing
        End Try

    End Function

    Private Sub ClearNullValuesFromArray(ByVal vTransdetailIDsList As List(Of String), ByRef vTransdetailIDs() As String)
        If (vTransdetailIDsList(vTransdetailIDsList.Count - 1) Is Nothing) Then
            vTransdetailIDsList.RemoveAt(vTransdetailIDsList.Count - 1)
            ReDim Preserve vTransdetailIDs(vTransdetailIDsList.Count - 1)
            vTransdetailIDsList = New List(Of String)
            vTransdetailIDsList.AddRange(vTransdetailIDs)
            If (vTransdetailIDsList(vTransdetailIDsList.Count - 1) Is Nothing) Then
                ClearNullValuesFromArray(vTransdetailIDsList, vTransdetailIDs)
            End If
        End If

    End Sub

    ''' <summary>
    ''' Allocates this receipt to the supplied transaction
    ''' </summary>
    Private Sub AllocateReceipt()

        Dim iReturn As Integer
        Dim oAllocationManual As bACTAllocationManual.Business
        Dim cAmtToBeAllocated As Decimal = 0D
        Dim bClearLastValueFromArrayRequired = False


        iReturn = iReturn >> 2
        Try
            If m_iInsuranceFileCnt = 0 Then
                ' Get outstanding amount on allocation transaction
                GetAccountAmountOS()
                m_cOSBaseBalance = m_cOSBaseBalance + m_cBaseAmount
            Else
                'GetAccountAmountOS(m_iInsuranceFileCnt)
                m_cOSBaseBalance = m_cOSBaseBalance + m_cBaseAmount 'To compare the Insurnace File Premium and Insurance Receipt Amount
                bClearLastValueFromArrayRequired = False
            End If

            ' We only allocate if the receipt will fully clear the account or
            ' the Allow Partial Allocation flag has been set to 1
            If ((m_cOSBaseBalance = 0) Or (GetAttribute("allow_partial_allocation").ToString = "1")) Then
                ' Create allocation business object

                oAllocationManual = New bACTAllocationManual.Business
                oAllocationManual.Initialise(
                    sUsername:="",
                    sPassword:="",
                    iUserID:=1,
                    iSourceID:=1,
                    iLanguageID:=1,
                    iCurrencyID:=26,
                    iLogLevel:=PMELogLevel.PMLogError,
                    sCallingAppName:=ACApp,
                    vDatabase:=m_oDatabase)

                If Not IsNothing(m_vOSTransactions) Then
                    If bClearLastValueFromArrayRequired Then
                        ReDim Preserve m_vOSTransactions(m_vOSTransactions.GetLength(0) - 1, m_vOSTransactions.GetUpperBound(1) - 1)
                    End If
                    Dim lBnd As Integer = m_vOSTransactions.GetLowerBound(1)
                    Dim uBnd As Integer = m_vOSTransactions.GetUpperBound(1)
                    ' Declare trans detail array

                    Dim vTransdetailIDs(uBnd) As String
                    Dim dDifferenceAmount As Decimal
                    Sort2DArray(m_vOSTransactions, 1)
                    cAmtToBeAllocated = m_cBaseAmount
                    ' Loop OS array to create trans detail array
                    Dim lIndex As Integer = 0
                    Dim cAllocationAmount As Decimal = 0D
                    Dim vTransdetailIDsList As New List(Of String)
                    For lOSIndex As Integer = lBnd To uBnd
                        If Abs(cAmtToBeAllocated) >= CDec(m_vOSTransactions(1, lOSIndex)) Then
                            vTransdetailIDs(lIndex) = String.Format("{0}|{1}", m_vOSTransactions(0, lOSIndex), CDec(m_vOSTransactions(1, lOSIndex)))
                            cAmtToBeAllocated = cAmtToBeAllocated + CDec(m_vOSTransactions(1, lOSIndex))
                            cAllocationAmount = cAllocationAmount + CDec(m_vOSTransactions(1, lOSIndex))
                            lIndex += 1
                        ElseIf Abs(cAmtToBeAllocated) < CDec(m_vOSTransactions(1, lOSIndex)) And cAmtToBeAllocated <> 0 Then
                            vTransdetailIDs(lIndex) = String.Format("{0}|{1}", m_vOSTransactions(0, lOSIndex), Abs(CDec(cAmtToBeAllocated)))
                            cAmtToBeAllocated = 0
                            cAllocationAmount = m_cBaseAmount * (-1)
                            lIndex += 1
                            If m_bTPPFSettlement AndAlso Not String.IsNullOrEmpty(m_sInstalmentPlanRef) Then
                                dDifferenceAmount = Abs(CDec(m_vOSTransactions(1, lOSIndex))) - Abs(cAllocationAmount)
                            End If
                        End If
                    Next

                    vTransdetailIDsList.AddRange(vTransdetailIDs)
                    ClearNullValuesFromArray(vTransdetailIDsList, vTransdetailIDs)

                    Dim vKeys(1, 2) As Object
                    ' Populate keys array with transactions and cash list item id
                    vKeys(PMENavKeys.PMKeyName, 0) = ACTKeyNameTransDetailID
                    vKeys(PMENavKeys.PMKeyValue, 0) = String.Format("{0}|{1}", m_iTransDetailID, cAllocationAmount * (-1))
                    vKeys(PMENavKeys.PMKeyName, 1) = ACTKeyNameTransDetailIDs
                    vKeys(PMENavKeys.PMKeyValue, 1) = vTransdetailIDs
                    vKeys(PMENavKeys.PMKeyName, 2) = ACTKeyNameCashListItemId
                    vKeys(PMENavKeys.PMKeyValue, 2) = m_iCashListItemID

                    ' Set process mode
                    iReturn = oAllocationManual.SetProcessModes(vTask:=PMEComponentAction.PMEdit)
                    If iReturn <> PMEReturnCode.PMTrue Then
                        Throw New Exception("oAllocationManual.SetProcessModes failed.")
                    End If
                    'Modified,KeyArr cannot convert to 2-Dimensional Array of object
                    'Dim keyArr As Object = DirectCast(vKeys, Object)
                    Dim keyArr(,) As Object = DirectCast(vKeys, Object(,))

                    ' Set the keys
                    iReturn = oAllocationManual.SetKeys(vKeyArray:=keyArr)
                    If iReturn <> PMEReturnCode.PMTrue Then
                        Throw New Exception("oAllocationManual.SetKeys failed.")
                    End If

                    ' Start it
                    iReturn = oAllocationManual.Start()
                    If iReturn <> PMEReturnCode.PMTrue Then
                        Throw New Exception("oAllocationManual.Start failed.")
                    End If

                    If m_bTPPFSettlement Then
                        If vTransdetailIDs.Length > 0 Then
                            AddParameterLite(m_oDatabase, "transdetail_id", Split(vTransdetailIDs(0), "|")(0), PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)

                            ' Execute sql
                            iReturn = m_oDatabase.SQLAction("spu_instalment_plan_status_update", "Update Instalment Plan Status", True)
                            If iReturn <> PMEReturnCode.PMTrue Then
                                Throw New Exception("Unable to execute 'spu_instalment_plan_status_update'")
                            End If

                            If Abs(cAllocationAmount) < Abs(m_cBaseAmount) Then
                                CreateWrkMgrTask("Premium fully allocated to plan no " + m_sInstalmentPlanRef + ". Extra Premium on the receipt is " + Convert.ToString(Abs(m_cBaseAmount) - Abs(cAllocationAmount)))
                            ElseIf dDifferenceAmount > 0 Then
                                CreateWrkMgrTask("Premium partially allocated to plan no " + m_sInstalmentPlanRef + ". Outstanding premium is " + Convert.ToString(dDifferenceAmount))
                            End If

                        Else
                            'Log Error - The oustanding account balance and the receipt amounts do not match
                            SetAttribute("error_message", "Record not processed.  The Debit and Credit do not match.")
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Throw New Exception("Failed to allocate receipt", ex)

        Finally
            ' Release allocation object
            If oAllocationManual IsNot Nothing Then
                oAllocationManual.Dispose()
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Creates a cashlist for this transaction based on the band account and currency code.
    ''' If a cashlist matching the given criteria already exists then it's id is returned so
    ''' the new cashlistitem can be appended to it's existing items.
    ''' </summary>
    Private Sub CreateCashList()
        Dim iReturn As Integer

        sBankAccount = GetAttribute("bankaccount_code").ToString
        Dim sCurrencyCode As String = GetAttribute("currency_code").ToString

        Dim iNumberOfRecords As Integer
        Dim vResult(,) As Object

        Try
            ' Add parameters
            AddParameterLite(m_oDatabase, "batch_id", m_iBatchID, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
            AddParameterLite(m_oDatabase, "bankaccount_code", sBankAccount, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "currency_code", sCurrencyCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "username", GetAttribute("user_name"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "cashlist_id", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)

            ' Execute sql
            iReturn = m_oDatabase.SQLAction("spu_ACT_Import_Receipt_CreateCashList", "Create Cash List", True)
            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spu_ACT_Import_Receipt_CreateCashList'")
            End If

            ' Get cashlist id
            m_iCashListID = Util.ToSafeInt(m_oDatabase.Parameters.Item("cashlist_id").Value, 0)

            ''Start(Saurabh Agrawal) PN 54414
            AddParameterLite(m_oDatabase, "table_name", "Currency", PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)
            AddParameterLite(m_oDatabase, "code", sCurrencyCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString)

            iReturn = m_oDatabase.SQLSelect("spu_SIR_Get_Id_From_Code", "Get Id for code", True, iNumberOfRecords, vResult)

            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to get currency Id")
            End If
            If IsArray(vResult) Then
                m_iCurrencyId = Convert.ToInt32(DirectCast(vResult, Object(,))(0, 0))
            End If

            AddParameterLite(m_oDatabase, "table_name", "BankAccount", PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)
            AddParameterLite(m_oDatabase, "field_to_return_name", "account_id", PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "field_to_validate_name", "bank_account_name", PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "field_to_validate_value", sBankAccount, PMEParameterDirection.PMParamInput, PMEDataType.PMString)

            iReturn = m_oDatabase.SQLSelect("spu_SAM_Get_And_Validate_Field", "Get Account Id for bank Account name", True, iNumberOfRecords, vResult)

            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to get bank account code Id")
            End If
            If IsArray(vResult) Then
                m_iBankAccountId = Convert.ToInt32(DirectCast(vResult, Object(,))(0, 0))
            End If

            ''End(Saurabh Agrawal) PN 54414
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
    ''' Creates a cashlist item for this receipt
    ''' </summary>
    Private Sub CreateCashListItem()
        Dim iReturn As Integer
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
            AddParameterLite(m_oDatabase, "cashlist_id", m_iCashListID, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "receipt_type_code", GetAttribute("receipt_type_code"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "mediatype_code", GetAttribute("mediatype_code"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "mediatype_issuer_code", GetAttribute("mediatype_issuer_code"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "account_code", GetAttribute("account_code"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "transaction_date", dtTransactionDate, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
            AddParameterLite(m_oDatabase, "amount", GetAttribute("amount"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "media_ref", GetAttribute("media_reference"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            'AddParameterLite(m_oDatabase, "bank_ref", GetAttribute("bank_reference"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
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
            AddParameterLite(m_oDatabase, "username", GetAttribute("user_name"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "sBusinessIdentifierCode", m_sBIC, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "sInternationalBankAccountNumber", m_sIBAN, PMEParameterDirection.PMParamInput, PMEDataType.PMString)

            ' Execute sql
            iReturn = m_oDatabase.SQLAction("spu_ACT_Import_Receipt_CreateCashListItem", "Create Cash List Item", True)
            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spu_ACT_Import_Receipt_CreateCashListItem'")
            End If

            ' Get cashlist id
            m_iCashListItemID = Util.ToSafeInt(m_oDatabase.Parameters.Item("cashlistitem_id").Value, 0)
            m_iAccountID = Util.ToSafeInt(m_oDatabase.Parameters.Item("account_id").Value, 0)

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
    Public Overloads Sub GetAccountAmountOS()
        Dim iReturn As Integer
        Dim oBusiness As bACTAccount.Form
        Dim oTransaction As Object = Nothing

        Try
            ' Create business object
            oBusiness = New bACTAccount.Form
            oBusiness.Initialise(
                sUsername:="",
                sPassword:="",
                iUserID:=1,
                iSourceID:=1,
                iLanguageID:=1,
                iCurrencyID:=26,
                iLogLevel:=PMELogLevel.PMLogError,
                sCallingAppName:=ACApp,
                vDatabase:=m_oDatabase)

            ' Get the outstanding base amount and transactions
            iReturn = oBusiness.GetAccountOSTransactionsForReceipt(
                    vAccount_id:=DirectCast(m_iAccountID, Object),
                    vOSTransactions:=oTransaction,
                    r_cAccountBaseBalance:=m_cOSBaseBalance,
                    receipt_transdetail_id:=m_iTransDetailID,
                    v_sInstalment_Plan_Ref:=m_sInstalmentPlanRef)

            If iReturn = PMEReturnCode.PMTrue Then
                m_vOSTransactions = DirectCast(oTransaction, [Object](,))
            Else
                m_vOSTransactions = Nothing
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
    Public Overloads Sub GetAccountAmountOS(ByVal InsuranceFileCnt As Integer)
        Dim iReturn As Integer

        Dim iAccountId As Integer = 0
        Dim BaseBalance As Double = 0
        Dim BaseCurrencyCount As Int16 = 0
        Dim AccountBalance As Double = 0
        Dim AccountCurrencyCount As Int16 = 0
        Dim NumberOfRecords As Integer = 0

        Try

            ' Add parameters
            AddParameterLite(m_oDatabase, "insurance_file_cnt", InsuranceFileCnt, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
            AddParameterLite(m_oDatabase, "account_code", GetAttribute("account_code"), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "company_id", DBNull.Value, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            AddParameterLite(m_oDatabase, "base_balance", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMCurrency)
            AddParameterLite(m_oDatabase, "base_currency_count", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMInteger)
            AddParameterLite(m_oDatabase, "account_balance", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMDouble)
            AddParameterLite(m_oDatabase, "account_currency_count", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMCurrency)

            Dim oTransaction(,) As Object = Nothing

            ' Execute sql
            iReturn = m_oDatabase.SQLSelect("spu_ACT_Select_Trans_For_Allocation_FilterBy_Ins_File", "Select Trans for allocation by Ins File", True, NumberOfRecords, oTransaction)
            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spu_ACT_Select_Trans_For_Allocation_FilterBy_Ins_File for insurance file record - " & m_iInsuranceFileCnt & "'")
            End If

            If NumberOfRecords > 0 Then
                m_vOSTransactions = CType(oTransaction, [Object](,))
            Else
                m_vOSTransactions = Nothing
            End If
            m_cOSBaseBalance = Util.ToSafeDecimal(m_oDatabase.Parameters.Item("base_balance").Value, 0)

        Catch ex As Exception
            Throw New Exception("Unable to check account outstanding amount", ex)

        End Try
    End Sub

    ''' <summary>
    ''' Posts the cashlist item for this receipt
    ''' </summary>
    Public Sub PostCashListItem()
        Dim iReturn As Integer
        Dim oBusiness As bACTCashListPost.Automated
        Dim cBaseAmount As Decimal

        Try
            ' Create the business object
            oBusiness = New bACTCashListPost.Automated
            oBusiness.Initialise(
                sUsername:="",
                sPassword:="",
                iUserID:=1,
                iSourceID:=1,
                iLanguageID:=1,
                iCurrencyID:=26,
                iLogLevel:=PMELogLevel.PMLogError,
                sCallingAppName:=ACApp,
                vDatabase:=m_oDatabase)
            oBusiness.ReceiptTypeCode = GetAttribute("receipt_type_code")
            ' Post the unallocated cash
            iReturn = oBusiness.PostUnallocatedCash( _
                v_vCashListID:=m_iCashListID, _
                v_vCashListItemID:=m_iCashListItemID, _
                v_vBatchId:=m_iBatchID, _
                r_cBaseAmount:=cBaseAmount)
            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to post unallocated cash list item")
            End If

            ' Get the trans detail and base amount from our cash list item
            m_iTransDetailID = oBusiness.CashTransId
            m_cBaseAmount = cBaseAmount

        Catch ex As Exception
            Throw New Exception("Unable to post cash list item", ex)

        Finally
            oBusiness.Dispose()

        End Try

    End Sub


    ''' <summary>
    ''' Creates a cashlist for this transaction based on the band account and currency code.
    ''' If a cashlist matching the given criteria already exists then it's id is returned so
    ''' the new cashlistitem can be appended to it's existing items.
    ''' </summary>
    Private Function ProcessInsuranceFileReciept() As PMEReturnCode
        Dim iReturn As Integer

        Dim RenewalPremium As Double = 0
        Dim RenewalStatusCnt As Integer = 0
        Dim RenewalStatusTypeId As Integer = 0
        Dim OldInsuranceFileCnt As Integer = 0
        Dim InsuranceFileStatusId As Integer = 0
        Dim InsuranceFileTypeId As Integer = 0

        ProcessInsuranceFileReciept = PMEReturnCode.PMTrue

        Try

            AddParameterLite(m_oDatabase, "insurance_file_cnt", m_iInsuranceFileCnt, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
            AddParameterLite(m_oDatabase, "renewal_premium", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMCurrency)
            AddParameterLite(m_oDatabase, "renewal_status_cnt", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "renewal_status_type_id", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "insurance_file_status_id", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "insurance_file_type_id", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "old_insurance_file_cnt", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "insurance_ref", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMString)

            ' Execute sql
            iReturn = m_oDatabase.SQLAction("spu_Get_Ins_File_Details_And_Renewal_Premium", "spu_Get_Ins_File_Details_And_Renewal_Premium", True)
            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spu_Get_Ins_File_Details_And_Renewal_Premium' for insurance file record - " & m_iInsuranceFileCnt & "'")
            End If

            RenewalPremium = Util.ToSafeDecimal(m_oDatabase.Parameters.Item("renewal_premium").Value, 0)
            RenewalStatusCnt = Util.ToSafeInt(m_oDatabase.Parameters.Item("renewal_status_cnt").Value, 0)
            RenewalStatusTypeId = Util.ToSafeInt(m_oDatabase.Parameters.Item("renewal_status_type_id").Value, 0)
            OldInsuranceFileCnt = Util.ToSafeInt(m_oDatabase.Parameters.Item("old_insurance_file_cnt").Value, 0)
            InsuranceFileStatusId = Util.ToSafeInt(m_oDatabase.Parameters.Item("insurance_file_status_id").Value, 0)
            InsuranceFileTypeId = Util.ToSafeInt(m_oDatabase.Parameters.Item("insurance_file_type_id").Value, 0)

            'Set the Policy reference value in the XML file so they can be identified at a later date.
            SetAttribute("policy_reference", Convert.ToString(m_oDatabase.Parameters.Item("insurance_ref").Value))

            If (InsuranceFileTypeId = 1 _
                Or InsuranceFileTypeId = 4 _
                Or InsuranceFileTypeId = 7 _
                Or InsuranceFileTypeId = 10 _
                Or InsuranceFileStatusId = 2 _
                Or InsuranceFileStatusId = 4 _
                Or InsuranceFileStatusId = 5 _
                Or InsuranceFileStatusId = 6) Then

                ' Log error - Not Live policy
                ProcessInsuranceFileReciept = PMEReturnCode.PMError
                SetAttribute("error_message", "Record not processed.  The policy is not live nor under renewal.")

            Else
                If InsuranceFileTypeId = 3 Then
                    If RenewalStatusTypeId <> 0 Then
                        If RenewalStatusTypeId = 5 Then
                            ProcessRenewal(OldInsuranceFileCnt, m_iInsuranceFileCnt, RenewalStatusCnt)
                        ElseIf RenewalStatusTypeId <> 5 Then
                            ProcessInsuranceFileReciept = PMEReturnCode.PMError
                            SetAttribute("error_message", "Record not processed.  Incorrect Renewal Status.")
                        End If
                    Else
                        ' Log error - Incorrect renewal status
                        ProcessInsuranceFileReciept = PMEReturnCode.PMError
                        SetAttribute("error_message", "Record not processed.  The policy is ready for renewal acceptance.")
                    End If
                End If
            End If

        Catch ex As Exception
            Throw New Exception("Unable to get process insurance file record - " & m_iInsuranceFileCnt, ex)
        Finally

        End Try
    End Function

    ''' <summary>
    ''' Process the renewal for the given Insurance file record
    ''' </summary>
    Public Overloads Sub ProcessRenewal(ByVal OldInsuranceFileCnt As Integer, ByVal NewInsuranceFileCnt As Integer, ByVal RenewalStatusCnt As Integer)
        Dim iReturn As Integer
        'Dim oBusiness As bSIRRenewal.Business = Nothing
        Dim oBusiness As bSIRAutomaticRenewalsAccept.Business
        Dim FailureMassage As String = String.Empty

        ' Create business object
        oBusiness = New bSIRAutomaticRenewalsAccept.Business
        oBusiness.Initialise(
            sUsername:="",
            sPassword:="",
            iUserID:=1,
            iSourceID:=1,
            iLanguageID:=1,
            iCurrencyID:=26,
            iLogLevel:=PMELogLevel.PMLogError,
            sCallingAppName:=ACApp,
            vDatabase:=m_oDatabase)

        ' Set the InsuranceFileCnt to the Renewal InsuranceFileCnt
        oBusiness.InsuranceFileCnt = NewInsuranceFileCnt

        ' Auto accept the renewal
        iReturn = oBusiness.RenewalAcceptForSinglePolicy()

        ' Release business object
        oBusiness.Dispose()

        If iReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("bSIRAutomaticRenewalsAccept.Business.RenewalAcceptForSinglePolicy failed to auto accept the renewal for InsuranceFileCnt - " & NewInsuranceFileCnt & " - and returned the error code - " & iReturn & ".  Check the Sirius logs for more information.")
        End If

    End Sub

    Protected Overrides Sub ProcessHeader()

        ' First get the transaction date. We don't use it in the header but if it's 
        ' not supplied for a transaction line we will use the date supplied here.
        m_dtTransactionDate = Util.ToSafeDate(GetAttribute("date"), DateTime.Now)
        m_sBIC = GetAttribute("sepa_account_bic")
        m_sIBAN = GetAttribute("sepa_account_iban")

        ' Get the cash list for this item as we only create one for the entire batch
        CreateCashList()
    End Sub

    ''' <summary>
    ''' Process an individual receipt item
    ''' </summary>
    Protected Overrides Sub ProcessElement()
        Try

            Dim PostReceipt As Boolean = True
            NoOfTotalRecords += 1
            ' If the Insurance_file_cnt is set then we need to process the element for renewal acceptance
            m_iInsuranceFileCnt = Util.ToSafeInt(GetAttribute("insurance_file_cnt"), 0)
            If m_iInsuranceFileCnt <> 0 Then

                If ProcessInsuranceFileReciept() = PMEReturnCode.PMTrue Then

                    ' Get outstanding amount on allocation transaction filtered on Insurance_File_Cnt
                    GetAccountAmountOS(m_iInsuranceFileCnt)

                    If GetAttribute("do_not_post_unallocated_amount").ToString = "1" Then
                        Dim BaseAmount As Double = Util.ToSafeDecimal(GetAttribute("amount"), 0)
                        If (m_cOSBaseBalance <> BaseAmount) And (GetAttribute("allow_partial_allocation").ToString <> "1") Then
                            'Log Error - The oustanding account balance and the receipt amounts do not match
                            SetAttribute("error_message", "Record not processed.  The Debit and Credit do not match.")
                            PostReceipt = False
                            NoOfRejections += 1
                            Throw New Exception("Allow Partial Allocation Failed")

                        End If
                    End If
                Else
                    NoOfRejections += 1
                    PostReceipt = False
                End If

            End If

            ''Start(Saurabh Agrawal)  Tech Spec VAL P15 Installment Plan(5.1.1)
            Dim InstalmentPayment As Boolean = False
            m_bTPPFSettlement = (GetAttribute("receipt_type_code").ToString = "TPPF")
            m_sInstalmentPlanRef = GetAttribute("inst_plan_reference").ToString
            If m_sInstalmentPlanRef = 0 Then m_sInstalmentPlanRef = ""
            InstalmentPayment = (GetAttribute("receipt_type_code").ToString = "INST")

           If m_bTPPFSettlement Then
                If Not CheckInstalmentPlanAndStatus(m_sInstalmentPlanRef, GetAttribute("account_code").ToString) Then
                    PostReceipt = False
                End If
            End If

            ''End(Saurabh Agrawal)  Tech Spec VAL P15 Installment Plan(5.1.1)
            If PostReceipt = True Then

                ' Create the cash list item
                CreateCashListItem()

                If InstalmentPayment Then
                    CreateCashListItemInstalments()
                Else
                    ' Post the cash list and get base amount
                    PostCashListItem()
                    ' Allocate the receipt
                    AllocateReceipt()
                End If
            End If

        Catch ex As Exception
            If m_bTPPFSettlement Then
                ' Create a work manager task to indicate this file failed
                CreateWrkMgrTask(String.Format("Receipt Import failed for Instalment Plan Ref {0}. Error Details : {1}", m_sInstalmentPlanRef, ex.Message))
            End If
            Throw New Exception("Unable to process Receipt", ex)
        End Try
    End Sub

    Protected Function CheckInstalmentPlanAndStatus(ByVal v_sInstalmentPlanRef As String, ByVal sAccountCode As String) As Boolean
        Dim bProceed As Boolean = True
        Dim nReturn As Integer
        Dim oStringBuilder As StringBuilder
        Dim vResultArray(,) As Object
        If Not String.IsNullOrEmpty(v_sInstalmentPlanRef) Then
            AddParameterLite(m_oDatabase, "sInstalment_Plan_Ref", v_sInstalmentPlanRef, PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)

            ' Execute sql
            nReturn = m_oDatabase.SQLSelect("spu_Get_installment_Plan_Status", "spu Get installment Plan Status", True, vResultArray:=vResultArray)
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spu_Get_installment_Plan_Status' for InstalmentPlanRef - " & v_sInstalmentPlanRef & "'")
            End If

            If vResultArray IsNot Nothing AndAlso vResultArray.Length > 0 Then
                oStringBuilder = New StringBuilder("Import failed for " + FileName + ". Error details:")
                If (vResultArray(0, 0).ToString = "900") Then
                    CreateWrkMgrTask(String.Format(oStringBuilder.ToString + v_sInstalmentPlanRef + " Completed."))
                    bProceed = False
                Else
                    If Not (Not String.IsNullOrEmpty(vResultArray(1, 0).ToString) AndAlso vResultArray(1, 0).ToString = sAccountCode) Then
                        oStringBuilder.AppendLine(" Account code:" + sAccountCode + " Mismatched.")
                        CreateWrkMgrTask(String.Format(oStringBuilder.ToString))
                        bProceed = False
                    End If
                    'If Not (String.IsNullOrEmpty(vResultArray(0, 1).ToString) OrElse vResultArray(0, 1).ToString = sAccountCode) Then
                    '    oStringBuilder.AppendLine(" Currency code:" + sCurrencyCode + " Mismatched.")
                    '    bProceed = False
                    'End If

                End If
                oStringBuilder = Nothing
            End If
        End If
        Return bProceed
    End Function

    Protected Sub CreateWrkMgrTask(ByVal sDescription As String)
        Dim iReturn As PMEReturnCode
        Dim oDatabase As dPMDAO.Database = Nothing

        Try
            ' Connect to db
            DBConnect(oDatabase)

            ' Add parameters
            AddParameterLite(oDatabase, "pmwrk_task_instance_cnt", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong, True)
            AddParameterLite(oDatabase, "pmwrk_task_group_id", 1, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(oDatabase, "pmwrk_task_id", 18, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(oDatabase, "customer", "System adminstration", PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(oDatabase, "task_due_date", DateTime.Now, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
            AddParameterLite(oDatabase, "pmuser_group_id", 1, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(oDatabase, "user_id", DBNull.Value, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(oDatabase, "description", sDescription, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(oDatabase, "task_status", 0, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(oDatabase, "is_urgent", 1, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(oDatabase, "date_created", DateTime.Now, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
            AddParameterLite(oDatabase, "created_by_id", 1, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(oDatabase, "last_modified", DBNull.Value, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
            AddParameterLite(oDatabase, "modified_by_id", DBNull.Value, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(oDatabase, "is_visible", 1, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(oDatabase, "workflow_information", DBNull.Value, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)

            ' Execute command
            iReturn = oDatabase.SQLAction("spe_PMWrk_Task_Instance_add", "Create WMTask", True)
            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spe_PMWrk_Task_Instance_add'")
            End If
        Catch ex As Exception
            Throw New Exception("Unable to create work manager task", ex)
        Finally
            ' Disconnect from db
            DBDisconnect(oDatabase)
        End Try
    End Sub

    ''Start(Saurabh Agrawal)  Tech Spec VAL P15 Installment Plan(5.1.1)
    Private Sub CreateCashListItemInstalments()

        Dim oCahListItem As bACTCashlistitem.Form
        Dim iReturn As Long
        Dim iInstalmentPlanRef As String
        Dim vResultArray(,) As Object
        Dim vInstalmentIdArray(,) As Object
        Dim dAmount As Decimal
        Dim oPFInstalment As bSIRPFInstalments.Business
        Dim lCount As Integer
        Dim lIDArrayCount As Integer
        Dim dtTransactionDate As DateTime
        Dim bSelected As Boolean = False
        Dim dBaseCurrencyAmount As Decimal
        Dim oACTCurConvert As bACTCurrencyConvert.Form
        Dim oACTDocument As bACTDocument.Form
        Dim oACTDocumentPost As bACTDocumentPost.Form
        Dim sGroupcode As String = ""
        Dim sRangeCode As String = ""
        '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        'Dim lNumber As Integer
        Dim sReference As String = ""
        Dim vTransArray(,) As Object
        Dim lDocumentID As Object = 0
        '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        Dim sDocumentReference As String = ""

        Const ACInstalmentsID As Integer = 0
        Const ACInstalmentAmount As Integer = 6
        Const ACInstalmentPlanRef As Integer = 7
        Const ACInstalmentFlagElement As Integer = 8
        Const ACPartialPaymentAmount As Integer = 9
        Const ACReceiptDifferenceOption As Integer = 11

        iInstalmentPlanRef = Convert.ToString(GetAttribute("media_reference"))
        dAmount = ToSafeDecimal(GetAttribute("amount"))

        oCahListItem = New bACTCashlistitem.Form
        oCahListItem.Initialise(
            sUserName:="",
            sPassword:="",
            iUserID:=1,
            iSourceID:=1,
            iLanguageID:=1,
            iCurrencyID:=26,
            iLogLevel:=PMELogLevel.PMLogError,
            sCallingAppName:=ACApp,
            vDatabase:=m_oDatabase)

        oACTCurConvert = New bACTCurrencyConvert.Form
        oACTCurConvert.Initialise(
            sUsername:="",
            sPassword:="",
            iUserID:=1,
            iSourceID:=1,
            iLanguageID:=1,
            iCurrencyID:=26,
            iLogLevel:=PMELogLevel.PMLogError,
            sCallingAppName:=ACApp,
            vDatabase:=m_oDatabase)

        oACTDocument = New bACTDocument.Form
        oACTDocument.Initialise(
            sUsername:="",
            sPassword:="",
            iUserID:=1,
            iSourceID:=1,
            iLanguageID:=1,
            iCurrencyID:=26,
            iLogLevel:=PMELogLevel.PMLogError,
            sCallingAppName:=ACApp,
            vDatabase:=m_oDatabase)

        oACTDocumentPost = New bACTDocumentPost.Form
        oACTDocumentPost.Initialise(
            sUsername:="",
            sPassword:="",
            iUserID:=1,
            iSourceID:=1,
            iLanguageID:=1,
            iCurrencyID:=26,
            iLogLevel:=PMELogLevel.PMLogError,
            sCallingAppName:=ACApp,
            vDatabase:=m_oDatabase)

        iReturn = oACTCurConvert.ConvertCurrencytoBase(m_iCurrencyId, 1, dBaseCurrencyAmount, dAmount)

        If iReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to get base currency")
        End If

        iReturn = oCahListItem.GetInstalmentDetails(m_iAccountID, m_oInstalmentsDetails)


        If iReturn <> PMEReturnCode.PMTrue Then
            Throw New Exception("Unable to get Instalemnts details")
        End If

        If IsArray(m_oInstalmentsDetails) Then
            Dim iUBound As Integer
            vResultArray = DirectCast(m_oInstalmentsDetails, Object(,))
            iUBound = UBound(vResultArray, 2)


            For iRowCount As Integer = 0 To iUBound
                If iInstalmentPlanRef = Convert.ToString(vResultArray(ACInstalmentPlanRef, iRowCount)).Trim Then
                    bSelected = True
                    If (dBaseCurrencyAmount - Convert.ToDouble(vResultArray(ACInstalmentAmount, iRowCount)) >= 0) Then
                        If Convert.ToDouble(vResultArray(ACInstalmentAmount, iRowCount)) > 0 Then
                            vResultArray(ACInstalmentFlagElement, iRowCount) = PMEReturnCode.PMTrue
                            vResultArray(ACPartialPaymentAmount, iRowCount) = 0
                            dBaseCurrencyAmount = dBaseCurrencyAmount - ToSafeDecimal(vResultArray(ACInstalmentAmount, iRowCount))
                            If dBaseCurrencyAmount = 0 Then
                                Exit For
                            End If
                        End If
                    Else
                        vResultArray(ACInstalmentFlagElement, iRowCount) = PMEReturnCode.PMTrue
                        vResultArray(ACPartialPaymentAmount, iRowCount) = dBaseCurrencyAmount
                        dBaseCurrencyAmount = 0
                        Exit For
                    End If
                End If

            Next


            If bSelected Then
                ReDim vInstalmentIdArray(2, 0)
                'loop through the instalments that are paid by the current receipt
                For lCount = 0 To UBound(vResultArray, 2)
                    If CLng(vResultArray(8, lCount)) = PMEReturnCode.PMTrue Then
                        'increase size of array by one (unless first time through)
                        ReDim Preserve vInstalmentIdArray(2, lIDArrayCount)

                        vInstalmentIdArray(0, lIDArrayCount) = CInt(vResultArray(ACInstalmentsID, lCount))
                        vInstalmentIdArray(1, lIDArrayCount) = CDbl(vResultArray(ACPartialPaymentAmount, lCount))
                        vInstalmentIdArray(2, lIDArrayCount) = CInt(vResultArray(ACReceiptDifferenceOption, lCount))

                        'increment count by one
                        lIDArrayCount = lIDArrayCount + 1
                    End If
                Next

                dtTransactionDate = DateTime.Parse(GetAttribute("transaction_date").ToString())

                oPFInstalment = New bSIRPFInstalments.Business
                oPFInstalment.Initialise(
                    sUsername:="",
                    sPassword:="",
                    iUserID:=1,
                    iSourceID:=1,
                    iLanguageID:=1,
                    iCurrencyID:=26,
                    iLogLevel:=PMELogLevel.PMLogError,
                    sCallingAppName:=ACApp,
                    vDatabase:=m_oDatabase)

                iReturn = oPFInstalment.PostMultipleInstalments(v_vInstalmentID:=vInstalmentIdArray,
                                                         v_dtTransactionDate:=CDate(dtTransactionDate),
                                                         v_vCashListItemID:=m_iCashListID)

                If iReturn <> PMEReturnCode.PMTrue Then
                    Throw New Exception("Unable to Run Post Multiple Instalments")
                End If

                iReturn = oCahListItem.CreateCashlistItemInstalments(m_oInstalmentsDetails, m_iCashListItemID)

                If iReturn <> PMEReturnCode.PMTrue Then
                    Throw New Exception("Unable to Create Cash ListItem Instalments")
                End If
            End If
        End If
        If dBaseCurrencyAmount <> 0 Then
            iReturn = oACTDocument.GetAutoNumValues(1, sGroupcode, sRangeCode)

            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to get Auto Num details")
            End If

            iReturn = oACTDocument.GenerateDocumentReferenceNumber(sGroupcode, sRangeCode, 1, 1, sDocumentReference)
            'Start-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            'If iReturn = PMEReturnCode.PMTrue And lNumber > 0 Then
            If iReturn = PMEReturnCode.PMTrue And Trim(sDocumentReference) <> "" Then
                'sReference = sRangeCode & Format(lNumber, "00000000")
                sReference = sRangeCode & sDocumentReference
                'End-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            Else
                Throw New Exception("Failed To Generate document ref")
            End If

            ReDim Preserve vTransArray(1, 16)

            vTransArray(0, 0) = m_iAccountID
            vTransArray(1, 0) = m_iBankAccountId

            vTransArray(0, 1) = GetAttribute("account_code")
            vTransArray(1, 1) = sBankAccount ''The Bank Account name is used as the bankaccount code.

            vTransArray(0, 2) = 26 ''The Base currency Id is hard coded as 26. There is a confusion in what is the base currency Id 
            vTransArray(1, 2) = 26 ''Many places in code it has been hardcoded as 26 so here also we are sending 26

            vTransArray(0, 3) = ""
            vTransArray(1, 3) = ""

            vTransArray(0, 4) = dBaseCurrencyAmount * -1
            vTransArray(1, 4) = dBaseCurrencyAmount

            vTransArray(0, 5) = 1
            vTransArray(1, 5) = 1

            vTransArray(0, 6) = dBaseCurrencyAmount * -1
            vTransArray(1, 6) = dBaseCurrencyAmount

            iReturn = oACTDocumentPost.AddDocumentTransactions(
                                r_vDocumentID:=lDocumentID,
                                v_lDocumentTypeId:=1,
                                v_sBranchID:="",
                                v_sComment:="",
                                v_dtDocumentDate:=Now(),
                                v_vDocSourceID:=1,
                                v_sDocumentRef:=sReference,
                                v_vOperatorID:=1,
                                v_vTransArray:=vTransArray,
                                v_lInsuranceFileCnt:=Nothing)

            If iReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to Create Journal Entry")
            End If

        End If
    End Sub

    ''End(Saurabh Agrawal)  Tech Spec VAL P15 Installment Plan(5.1.1)
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
