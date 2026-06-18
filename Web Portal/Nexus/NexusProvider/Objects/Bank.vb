<Serializable()> Public Class Bank

    Private sKey As String
    Private sBankKey As Integer

    Private sBankName As String

    Private sCode As String

    Private sBranchCode As String

    Private sHeadOffice As String

    Private sBankAddress As String
    Private sPayeeName As String
    Private sAccountCode As String
    Private dExpiryDate As Date
    Private bExpiryDateSpecified As Boolean
    Private sReference1 As String
    Private sReference2 As String
    Private dChequeDate As Date
    Private sInstrumentNumber, sDraweeBankName, sDraweeBankLocation, sChequeType, sChequeClearingType, sDraweeBankBranch As String

    ''WPR12
    Private iPartyBankKey, iRowKey As Integer
    Private sBankPaymentTypeCode As String
    Private sAccountType As String
    Private sAccountHolderName As String
    Private sAccountNumber As String
    Private sBankBranch As String
    Private sBankCode As String
    Private sStreetName As String
    Private sLocality As String
    Private sPostTown As String
    Private sCounty As String
    Private sPostCode As String
    Private sCountry As String
    Private iCountryKey As Integer
    Private iAccountKey As Integer
    Private bIsExternalCreditCardHandling, bIsBankItem, bIsDeleted As Boolean
    Private iLastTransactedPartyBankKey As Integer
    Private bTimeStamp() As Byte

    Private iTaskMode As Integer
    Private oPartyBankAddress As Address
    Private oBankHistory As BankHistoryCollection
    Private bIsActive As Boolean
    Private bIsPartyBankInUse As Boolean
    Private bIsPartyBankLinkedWithInst As Boolean
    Private oCreditCard As CreditCard
    Private sBIC As String
    Private sIBAN As String

    Public Sub New()
        MyBase.New()
        iTaskMode = Mode.Unchanged
        oPartyBankAddress = New Address
    End Sub

    Public Overridable Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Type : " & Me.GetType.Name & "<br />")
        sbPrint.AppendLine("Short Code : " & sBankKey & "<br />")
        sbPrint.AppendLine("Bank Name : " & sBankName & "<br />")
        sbPrint.AppendLine("Short Code : " & sCode & "<br />")
        sbPrint.AppendLine("Branch Code : " & sBranchCode & "<br />")
        sbPrint.AppendLine("Head Office : " & sHeadOffice & "<br />")
        sbPrint.AppendLine("Bank Address : " & sBankAddress & "<br />")

        sbPrint.AppendLine("<br />")

        Return sbPrint.ToString

    End Function
    ''' <summary>
    ''' Rowkey value 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property RowKey() As Integer
        Get
            Return Me.iRowKey
        End Get
        Set(ByVal value As Integer)
            Me.iRowKey = value
        End Set
    End Property

    ''' <summary>
    ''' Used for Active status for Bank details
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsDeleted() As Boolean
        Get
            Return Me.bIsDeleted
        End Get
        Set(ByVal value As Boolean)
            Me.bIsDeleted = value
        End Set
    End Property
    ''' <summary>
    ''' Bank Items value
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsBankItem() As Boolean
        Get
            Return Me.bIsBankItem
        End Get
        Set(ByVal value As Boolean)
            Me.bIsBankItem = value
        End Set
    End Property
    ''' <summary>
    ''' Key vaiue
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Key() As String
        Get
            Return Me.sKey
        End Get
        Set(ByVal value As String)
            Me.sKey = value
        End Set
    End Property

    ''' <summary>
    ''' Drawee Bank Branch
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DraweeBankBranch() As String
        Get
            Return Me.sDraweeBankBranch
        End Get
        Set(ByVal value As String)
            Me.sDraweeBankBranch = value
        End Set
    End Property
    ''' <summary>
    ''' Cheque Clearing Type
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChequeClearingType() As String
        Get
            Return Me.sChequeClearingType
        End Get
        Set(ByVal value As String)
            Me.sChequeClearingType = value
        End Set
    End Property
    ''' <summary>
    ''' Cheque Type
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChequeType() As String
        Get
            Return Me.sChequeType
        End Get
        Set(ByVal value As String)
            Me.sChequeType = value
        End Set
    End Property
    ''' <summary>
    ''' Drawee Bank Location
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DraweeBankLocation() As String
        Get
            Return Me.sDraweeBankLocation
        End Get
        Set(ByVal value As String)
            Me.sDraweeBankLocation = value
        End Set
    End Property
    ''' <summary>
    ''' Drawee Bank Name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DraweeBankName() As String
        Get
            Return Me.sDraweeBankName
        End Get
        Set(ByVal value As String)
            Me.sDraweeBankName = value
        End Set
    End Property
    ''' <summary>
    ''' Instrument Number
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property InstrumentNumber() As String
        Get
            Return Me.sInstrumentNumber
        End Get
        Set(ByVal value As String)
            Me.sInstrumentNumber = value
        End Set
    End Property
    ''' <summary>
    ''' Cheque Date
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ChequeDate() As Date
        Get
            Return Me.dChequeDate
        End Get
        Set(ByVal value As Date)
            Me.dChequeDate = value
        End Set
    End Property
    ''' <summary>
    ''' Bank Key
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BankKey() As Integer
        Get
            Return Me.sBankKey
        End Get
        Set(ByVal value As Integer)
            Me.sBankKey = value
        End Set
    End Property

    ''' <summary>
    ''' Bank Name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BankName() As String
        Get
            Return Me.sBankName
        End Get
        Set(ByVal value As String)
            Me.sBankName = value
        End Set
    End Property

    ''' <summary>
    ''' Bank Code
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BankCode() As String
        Get
            Return Me.sBankCode
        End Get
        Set(ByVal value As String)
            Me.sBankCode = value
        End Set
    End Property

    ''' <summary>
    ''' Bank Address
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BankAddress() As String
        Get
            Return Me.sBankAddress
        End Get
        Set(ByVal value As String)
            Me.sBankAddress = value
        End Set
    End Property

    ''' <summary>
    ''' Code
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Code() As String
        Get
            Return Me.sCode
        End Get
        Set(ByVal value As String)
            Me.sCode = value
        End Set
    End Property

    ''' <summary>
    ''' Branch Code
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BranchCode() As String
        Get
            Return Me.sBranchCode
        End Get
        Set(ByVal value As String)
            Me.sBranchCode = value
        End Set
    End Property

    ''' <summary>
    ''' Head Office Name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property HeadOffice() As String
        Get
            Return Me.sHeadOffice
        End Get
        Set(ByVal value As String)
            Me.sHeadOffice = value
        End Set
    End Property

    ''' <summary>
    ''' Party Bank Address
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PartyBankAddress() As Address
        Get
            Return oPartyBankAddress
        End Get
        Set(ByVal value As Address)
            oPartyBankAddress = value
        End Set
    End Property
    ''' <summary>
    ''' Payee Name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PayeeName() As String
        Get
            Return Me.sPayeeName
        End Get
        Set(ByVal value As String)
            Me.sPayeeName = value
        End Set
    End Property
    ''' <summary>
    ''' Account Code
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AccountCode() As String
        Get
            Return Me.sAccountCode
        End Get
        Set(ByVal value As String)
            Me.sAccountCode = value
        End Set
    End Property
    ''' <summary>
    ''' Reference1
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Reference1() As String
        Get
            Return Me.sReference1
        End Get
        Set(ByVal value As String)
            Me.sReference1 = value
        End Set
    End Property
    ''' <summary>
    ''' Reference2
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Reference2() As String
        Get
            Return Me.sReference2
        End Get
        Set(ByVal value As String)
            Me.sReference2 = value
        End Set
    End Property
    ''' <summary>
    ''' Expiry Date
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ExpiryDate() As Date
        Get
            Return Me.dExpiryDate
        End Get
        Set(ByVal value As Date)
            Me.dExpiryDate = value
        End Set
    End Property
    ''' <summary>
    ''' Expiry Date Specified
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ExpiryDateSpecified() As Boolean
        Get
            Return Me.bExpiryDateSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bExpiryDateSpecified = value
        End Set
    End Property

    ''' <summary>
    ''' Bank Payment Type Code
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BankPaymentTypeCode() As String
        Get
            Return Me.sBankPaymentTypeCode
        End Get
        Set(ByVal value As String)
            Me.sBankPaymentTypeCode = value
        End Set
    End Property
    ''' <summary>
    ''' Account Type
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AccountType() As String
        Get
            Return Me.sAccountType
        End Get
        Set(ByVal value As String)
            Me.sAccountType = value
        End Set
    End Property
    ''' <summary>
    ''' Task Mode
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TaskMode() As Integer
        Get
            Return Me.iTaskMode
        End Get
        Set(ByVal value As Integer)
            Me.iTaskMode = value
        End Set
    End Property

    ''' <summary>
    ''' Bank History Collection
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property History() As BankHistoryCollection
        Get
            Return Me.oBankHistory
        End Get
        Set(ByVal value As BankHistoryCollection)
            Me.oBankHistory = value
        End Set
    End Property
    ''' <summary>
    ''' Account Holder Name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AccountHolderName() As String
        Get
            Return Me.sAccountHolderName
        End Get
        Set(ByVal value As String)
            Me.sAccountHolderName = value
        End Set
    End Property

    ''' <summary>
    ''' Account Number
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AccountNumber() As String
        Get
            Return Me.sAccountNumber
        End Get
        Set(ByVal value As String)
            Me.sAccountNumber = value
        End Set
    End Property

    ''' <summary>
    ''' Bank Branch
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BankBranch() As String
        Get
            Return Me.sBankBranch
        End Get
        Set(ByVal value As String)
            Me.sBankBranch = value
        End Set
    End Property

    ''' <summary>
    ''' Street Name
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property StreetName() As String
        Get
            Return Me.sStreetName
        End Get
        Set(ByVal value As String)
            Me.sStreetName = value
        End Set
    End Property

    ''' <summary>
    ''' Locality
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Locality() As String
        Get
            Return Me.sLocality
        End Get
        Set(ByVal value As String)
            Me.sLocality = value
        End Set
    End Property

    ''' <summary>
    ''' Post Town
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PostTown() As String
        Get
            Return Me.sPostTown
        End Get
        Set(ByVal value As String)
            Me.sPostTown = value
        End Set
    End Property

    ''' <summary>
    ''' County
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property County() As String
        Get
            Return Me.sCounty
        End Get
        Set(ByVal value As String)
            Me.sCounty = value
        End Set
    End Property

    ''' <summary>
    ''' Post Code
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PostCode() As String
        Get
            Return Me.sPostCode
        End Get
        Set(ByVal value As String)
            Me.sPostCode = value
        End Set
    End Property

    ''' <summary>
    ''' Country
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Country() As String
        Get
            Return Me.sCountry
        End Get
        Set(ByVal value As String)
            Me.sCountry = value
        End Set
    End Property

    ''' <summary>
    ''' Country Key
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CountryKey() As Integer
        Get
            Return Me.iCountryKey
        End Get
        Set(ByVal value As Integer)
            Me.iCountryKey = value
        End Set
    End Property

    ''' <summary>
    ''' Account Key
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AccountKey() As Integer
        Get
            Return Me.iAccountKey
        End Get
        Set(ByVal value As Integer)
            Me.iAccountKey = value
        End Set
    End Property

    ''' <summary>
    ''' External Credit Card Handling
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsExternalCreditCardHandling() As Boolean
        Get
            Return Me.bIsExternalCreditCardHandling
        End Get
        Set(ByVal value As Boolean)
            Me.bIsExternalCreditCardHandling = value
        End Set
    End Property
    ''' <summary>
    ''' Last Transacted Party Bank Key
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property LastTransactedPartyBankKey() As Integer
        Get
            Return Me.iLastTransactedPartyBankKey
        End Get
        Set(ByVal value As Integer)
            Me.iLastTransactedPartyBankKey = value
        End Set
    End Property
    ''' <summary>
    ''' Party Bank Key
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PartyBankKey() As Integer
        Get
            Return Me.iPartyBankKey
        End Get
        Set(ByVal value As Integer)
            Me.iPartyBankKey = value
        End Set
    End Property
    ''' <summary>
    ''' Active Status For Bank
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsActive() As Boolean
        Get
            Return Me.bIsActive
        End Get
        Set(ByVal value As Boolean)
            Me.bIsActive = value
        End Set
    End Property

    ''' <summary>
    ''' Time Stamp
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TimeStamp() As Byte()
        Get
            Return bTimeStamp
        End Get
        Set(ByVal value As Byte())
            bTimeStamp = value
        End Set
    End Property
    ''' <summary>
    ''' Party Bank In Use
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsPartyBankInUse() As Boolean
        Get
            Return Me.bIsPartyBankInUse
        End Get
        Set(ByVal value As Boolean)
            Me.bIsPartyBankInUse = value
        End Set
    End Property

    ''' <summary>
    ''' Party Bank In Use For Installment
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsPartyBankLinkedWithInst() As Boolean
        Get
            Return Me.bIsPartyBankLinkedWithInst
        End Get
        Set(ByVal value As Boolean)
            Me.bIsPartyBankLinkedWithInst = value
        End Set
    End Property

    Public Property CreditCard() As CreditCard
        Get
            Return Me.oCreditCard
        End Get
        Set(ByVal value As CreditCard)
            Me.oCreditCard = value
        End Set
    End Property
    ''' <summary>
    ''' Business Identifier Codes(BIC) used in Party,Instalments,Claim,Cash/Cheque Payment and Receipt
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property BIC() As String
        Get
            Return Me.sBIC
        End Get
        Set(ByVal value As String)
            Me.sBIC = value
        End Set
    End Property

    ''' <summary>
    ''' International Bank Account Number(IBAN) used in Party,Instalments,Claim,Cash/Cheque Payment and Receipt
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IBAN() As String
        Get
            Return Me.sIBAN
        End Get
        Set(ByVal value As String)
            Me.sIBAN = value
        End Set
    End Property

    ''' <summary>
    ''' Mode For Active
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum Mode
        Unchanged = 0
        Add = 1
        Edit = 2
        Delete = 3
        MakeActive = 4
        MakeInactive = 5
        EditAndMakeActive = 6
    End Enum

End Class


''' <summary>
''' Collection of Bank objects
''' </summary>
<Serializable()> Public Class BankCollection : Inherits SortableCollectionBase
    Public Sub New()
        MyBase.SortObjectType = GetType(Bank)
    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oBank As Bank In List
            sbPrint.AppendLine(oBank.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add a Bank object to the collection
    ''' </summary>
    ''' <param name="v_oBank">The Bank object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oBank As Bank) As Integer
        v_oBank.Key = List.Add(v_oBank)
        Return v_oBank.Key
    End Function

    ''' <summary>
    ''' Remove a Bank object from the collection
    ''' </summary>
    ''' <param name="v_oBank">The Bank object to be removed</param>
    Public Sub Remove(ByVal v_oBank As Bank)
        List.Remove(v_oBank)
    End Sub

    ''' <summary>
    ''' Add a Bank object to the collection
    ''' </summary>
    ''' <param name="v_oCreditCard">The Credit Card object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oCreditCard As CreditCard) As Integer
        v_oCreditCard.Key = List.Add(v_oCreditCard)
        Return v_oCreditCard.Key
    End Function

    ''' <summary>
    ''' Remove a CreditCard object from the collection
    ''' </summary>
    ''' <param name="v_oCreditCard">The Bank object to be removed</param>
    Public Sub Remove(ByVal v_oCreditCard As CreditCard)
        List.Remove(v_oCreditCard)
    End Sub

    ''' <summary>
    ''' Remove a Bank object from the collection at a particular location
    ''' </summary>
    ''' <param name="index"></param>
    ''' <remarks></remarks>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub
    ''' <summary>
    ''' Update a Bank List from the collection
    ''' </summary>
    ''' <param name="v_oBank"></param>
    ''' <remarks></remarks>
    Public Sub Update(ByVal v_oBank As Bank)
        List.Item(v_oBank.Key) = v_oBank
    End Sub
    ''' <summary>
    ''' Update a CreditCard List from the collection
    ''' </summary>
    ''' <param name="v_oCreditCard"></param>
    ''' <remarks></remarks>
    Public Sub Update(ByVal v_oCreditCard As CreditCard)
        List.Item(v_oCreditCard.Key) = v_oCreditCard
    End Sub
    ''' <summary>
    ''' Update  a Bank List and index from the collection
    ''' </summary>
    ''' <param name="v_oBank"></param>
    ''' <param name="index"></param>
    ''' <remarks></remarks>
    Public Sub Update(ByVal v_oBank As Bank, ByVal index As Integer)
        List.Item(index) = v_oBank
    End Sub
    ''' <summary>
    ''' Update  a Bank List and index from the collection
    ''' </summary>
    ''' <param name="v_oCreditCard"></param>
    ''' <param name="index"></param>
    ''' <remarks></remarks>
    Public Sub Update(ByVal v_oCreditCard As CreditCard, ByVal index As Integer)
        List.Item(index) = v_oCreditCard
    End Sub
    ''' <summary>
    ''' Bank Item
    ''' </summary>
    ''' <param name="i"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Default Public Property Item(ByVal i As Integer) As Bank
        Get
            Return List(i)
        End Get
        Set(ByVal value As Bank)
            List(i) = value
        End Set
    End Property

End Class
'WPR12
<Serializable()> Public Class BankHistory : Inherits Bank

    Private sActionCode As String
    Private dtActionDate As DateTime
    Private sUserName As String

    ''' <summary>
    ''' Action Code
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ActionCode() As String
        Get
            Return Me.sActionCode
        End Get
        Set(ByVal value As String)
            Me.sActionCode = value
        End Set
    End Property

    ''' <summary>
    ''' Action Date
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ActionDate() As DateTime
        Get
            Return Me.dtActionDate
        End Get
        Set(ByVal value As DateTime)
            Me.dtActionDate = value
        End Set
    End Property

    ''' <summary>
    ''' User Name 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UserName() As String
        Get
            Return Me.sUserName
        End Get
        Set(ByVal value As String)
            Me.sUserName = value
        End Set
    End Property

End Class

''' <summary>
''' Collection of Bank objects
''' </summary>
<Serializable()> Public Class BankHistoryCollection : Inherits SortableCollectionBase

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oBankHistory As BankHistory In List
            sbPrint.AppendLine(oBankHistory.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add a Bank object to the collection
    ''' </summary>
    ''' <param name="v_oBank">The Bank object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oBank As Bank) As Integer
        Return List.Add(v_oBank)
    End Function

    ''' <summary>
    ''' Remove an Bank object from the collection
    ''' </summary>
    ''' <param name="v_oBank">The Bank object to be removed</param>
    Public Sub Remove(ByVal v_oBank As Bank)
        List.Remove(v_oBank)
    End Sub
    ''' <summary>
    ''' Update a Bank List from the collection 
    ''' </summary>
    ''' <param name="v_oBank"></param>
    ''' <remarks></remarks>
    Public Sub Update(ByVal v_oBank As Bank)
        'List.Item(v_oBank.Key) = v_oBank
        List.Item(v_oBank.BankKey) = v_oBank
    End Sub
    ''' <summary>
    ''' Update a Bank List and Index from the collection
    ''' </summary>
    ''' <param name="v_oBank"></param>
    ''' <param name="index"></param>
    ''' <remarks></remarks>
    Public Sub Update(ByVal v_oBank As Bank, ByVal index As Integer)
        List.Item(index) = v_oBank
    End Sub

    ''' <summary>
    ''' Bank History Item
    ''' </summary>
    ''' <param name="i"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Default Public Property Item(ByVal i As Integer) As BankHistory
        Get
            Return List(i)
        End Get
        Set(ByVal value As BankHistory)
            List(i) = value
        End Set
    End Property

End Class


