'WPR - VB 64 - Media Type Status 
<Serializable()> _
Partial Public Class CashListReceipt

    Private sCashListReceiptRowID As Integer
    Private sCommentsField As String

    Private dModifiedDateField As DateTime

    Private iCashListItemKeyField As Integer

    Private iInsuranceFileKeyField As Integer

    Private iMediaTypeKeyField As Integer

    Private sMediaTypeDescriptionField As String

    Private iMediaTypeStatusKeyField As Integer

    Private sMediaTypeStatusDescriptionField As String

    Private sDocumentRefField As String

    Private sBranchDescriptionField As String

    Private sClientCodeField As String

    Private sClientNameField As String

    Private sPolicyNumberField As String

    Private sDrawnBankNameField As String

    Private ipartyKeyField As Integer

    Private bPartyKeyFieldSpecified As Boolean

    Private sBankAccountCodeField As String

    Private sInsuranceRefField As String

    Private dCollectionDateFromField As DateTime

    Private bCollectionDateFromFieldSpecified As Boolean

    Private dCollectionDateToField As DateTime

    Private bCollectionDateToFieldSpecified As Boolean

    Private sMediaReferenceField As String

    Private sMediaTypeStatusCodeField As String

    Private sDrawnBankCodeField As String

    Private iMaxRowsToFetchField As Integer

    Private bMaxRowsToFetchFieldSpecified As Boolean

    Private iIsSelected As Boolean

    Private sMediaTypeCodeField As String

    Private sCurrentStatusField As String

    Public Property CashListReceiptRowID() As Integer
        Get
            Return sCashListReceiptRowID
        End Get
        Set(ByVal value As Integer)
            sCashListReceiptRowID = value
        End Set
    End Property

    Public Property MediaTypeCode() As String
        Get
            Return Me.sMediaTypeCodeField
        End Get
        Set(ByVal value As String)
            Me.sMediaTypeCodeField = value
        End Set
    End Property

    Public Property IsSelected() As Boolean
        Get
            Return iIsSelected
        End Get
        Set(ByVal value As Boolean)
            iIsSelected = value
        End Set
    End Property

    Public Property Comments() As String
        Get
            Return Me.sCommentsField
        End Get
        Set(ByVal value As String)
            Me.sCommentsField = value
        End Set
    End Property

    Public Property ModifiedDate() As DateTime
        Get
            Return Me.dModifiedDateField
        End Get
        Set(ByVal value As DateTime)
            Me.dModifiedDateField = value
        End Set
    End Property

    Public Property PartyKey() As Integer
        Get
            Return Me.ipartyKeyField
        End Get
        Set(ByVal value As Integer)
            Me.ipartyKeyField = value
        End Set
    End Property

    Public Property PartyKeySpecified() As Boolean
        Get
            Return Me.bPartyKeyFieldSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bPartyKeyFieldSpecified = value
        End Set
    End Property

    Public Property BankAccountCode() As String
        Get
            Return Me.sBankAccountCodeField
        End Get
        Set(ByVal value As String)
            Me.sBankAccountCodeField = value
        End Set
    End Property


    Public Property InsuranceRef() As String
        Get
            Return Me.sInsuranceRefField
        End Get
        Set(ByVal value As String)
            Me.sInsuranceRefField = value
        End Set
    End Property

    Public Property CollectionDateFrom() As DateTime
        Get
            Return Me.dCollectionDateFromField
        End Get
        Set(ByVal value As DateTime)
            Me.dCollectionDateFromField = value
        End Set
    End Property

    Public Property CollectionDateFromSpecified() As Boolean
        Get
            Return Me.bCollectionDateFromFieldSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bCollectionDateFromFieldSpecified = value
        End Set
    End Property

    Public Property CollectionDateTo() As DateTime
        Get
            Return Me.dCollectionDateToField
        End Get
        Set(ByVal value As DateTime)
            Me.dCollectionDateToField = value
        End Set
    End Property

    Public Property CollectionDateToSpecified() As Boolean
        Get
            Return Me.bCollectionDateToFieldSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bCollectionDateToFieldSpecified = value
        End Set
    End Property

    Public Property MediaTypeStatusCode() As String
        Get
            Return Me.sMediaTypeStatusCodeField
        End Get
        Set(ByVal value As String)
            Me.sMediaTypeStatusCodeField = value
        End Set
    End Property

    Public Property DrawnBankCode() As String
        Get
            Return Me.sDrawnBankCodeField
        End Get
        Set(ByVal value As String)
            Me.sDrawnBankCodeField = value
        End Set
    End Property

    Public Property MaxRowsToFetch() As Integer
        Get
            Return Me.iMaxRowsToFetchField
        End Get
        Set(ByVal value As Integer)
            Me.iMaxRowsToFetchField = value
        End Set
    End Property

    Public Property MaxRowsToFetchSpecified() As Boolean
        Get
            Return Me.bMaxRowsToFetchFieldSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bMaxRowsToFetchFieldSpecified = value
        End Set
    End Property


    '''<remarks/>
    Public Property CashListItemKey() As Integer
        Get
            Return Me.iCashListItemKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iCashListItemKeyField = value
        End Set
    End Property

    '''<remarks/>
    Public Property InsuranceFileKey() As Integer
        Get
            Return Me.iInsuranceFileKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iInsuranceFileKeyField = value
        End Set
    End Property

    '''<remarks/>
    Public Property MediaTypeKey() As Integer
        Get
            Return Me.iMediaTypeKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iMediaTypeKeyField = value
        End Set
    End Property

    '''<remarks/>
    Public Property MediaTypeDescription() As String
        Get
            Return Me.sMediaTypeDescriptionField
        End Get
        Set(ByVal value As String)
            Me.sMediaTypeDescriptionField = value
        End Set
    End Property

    '''<remarks/>
    Public Property MediaTypeStatusKey() As Integer
        Get
            Return Me.iMediaTypeStatusKeyField
        End Get
        Set(ByVal value As Integer)
            Me.iMediaTypeStatusKeyField = value
        End Set
    End Property

    '''<remarks/>
    Public Property MediaTypeStatusDescription() As String
        Get
            Return Me.sMediaTypeStatusDescriptionField
        End Get
        Set(ByVal value As String)
            Me.sMediaTypeStatusDescriptionField = value
        End Set
    End Property

    '''<remarks/>
    Public Property DocumentRef() As String
        Get
            Return Me.sDocumentRefField
        End Get
        Set(ByVal value As String)
            Me.sDocumentRefField = value
        End Set
    End Property

    '''<remarks/>
    Public Property BranchDescription() As String
        Get
            Return Me.sBranchDescriptionField
        End Get
        Set(ByVal value As String)
            Me.sBranchDescriptionField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClientCode() As String
        Get
            Return Me.sClientCodeField
        End Get
        Set(ByVal value As String)
            Me.sClientCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClientName() As String
        Get
            Return Me.sClientNameField
        End Get
        Set(ByVal value As String)
            Me.sClientNameField = value
        End Set
    End Property

    '''<remarks/>
    Public Property PolicyNumber() As String
        Get
            Return Me.sPolicyNumberField
        End Get
        Set(ByVal value As String)
            Me.sPolicyNumberField = value
        End Set
    End Property

    '''<remarks/>
    Public Property DrawnBankName() As String
        Get
            Return Me.sDrawnBankNameField
        End Get
        Set(ByVal value As String)
            Me.sDrawnBankNameField = value
        End Set
    End Property

    '''<remarks/>
    Public Property MediaReference() As String
        Get
            Return Me.sMediaReferenceField
        End Get
        Set(ByVal value As String)
            Me.sMediaReferenceField = value
        End Set
    End Property

    Public Property CurrentStatus() As String
        Get
            Return Me.sCurrentStatusField
        End Get
        Set(ByVal value As String)
            Me.sCurrentStatusField = value
        End Set
    End Property

End Class


<Serializable()> _
Partial Public Class CashListReceipts : Inherits SortableCollectionBase

    Public Sub New()
        MyBase.SortObjectType = GetType(CashListReceipt)
    End Sub

    Public Function Add(ByVal v_oCashListReceipt As CashListReceipt) As Integer
        Return List.Add(v_oCashListReceipt)
    End Function

    Public Sub Remove(ByVal v_oCashListReceipt As CashListReceipt)
        List.Remove(v_oCashListReceipt)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As CashListReceipt
        Get
            Return List(i)
        End Get
        Set(ByVal value As CashListReceipt)
            List(i) = value
        End Set
    End Property
End Class