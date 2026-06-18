<Serializable()> Public Class PFBankDetails
    Private nPartyBankKeyField As Integer
    Private sBankNameField As String
    Private sBankSortCodeField As String
    Private sBankAccountNoField As String
    Private sBankAccountNameField As String
    Private sBankBranchField As String
    Private sBankAreaCodeField As String
    Private sBankPhoneField As String
    Private sBankExtnField As String
    Private sBankFaxCodeField As String
    Private sBankFaxField As String
    Private oBankAddressField As Address

    Public Property PartyBankKey() As Integer
        Get
            Return Me.nPartyBankKeyField
        End Get
        Set(ByVal value As Integer)
            Me.nPartyBankKeyField = value
        End Set
    End Property

    Public Property BankName() As String
        Get
            Return Me.sBankNameField
        End Get
        Set(ByVal value As String)
            Me.sBankNameField = value
        End Set
    End Property

    Public Property BankSortCode() As String
        Get
            Return Me.sBankSortCodeField
        End Get
        Set(ByVal value As String)
            Me.sBankSortCodeField = value
        End Set
    End Property

    Public Property BankAccountNo() As String
        Get
            Return Me.sBankAccountNoField
        End Get
        Set(ByVal value As String)
            Me.sBankAccountNoField = value
        End Set
    End Property

    Public Property BankAccountName() As String
        Get
            Return Me.sBankAccountNameField
        End Get
        Set(ByVal value As String)
            Me.sBankAccountNameField = value
        End Set
    End Property

    Public Property BankBranch() As String
        Get
            Return Me.sBankBranchField
        End Get
        Set(ByVal value As String)
            Me.sBankBranchField = value
        End Set
    End Property

    Public Property BankAreaCode() As String
        Get
            Return Me.sBankAreaCodeField
        End Get
        Set(ByVal value As String)
            Me.sBankAreaCodeField = value
        End Set
    End Property

    Public Property BankPhone() As String
        Get
            Return Me.sBankPhoneField
        End Get
        Set(ByVal value As String)
            Me.sBankPhoneField = value
        End Set
    End Property

    Public Property BankExtn() As String
        Get
            Return Me.sBankExtnField
        End Get
        Set(ByVal value As String)
            Me.sBankExtnField = value
        End Set
    End Property

    Public Property BankFaxCode() As String
        Get
            Return Me.sBankFaxCodeField
        End Get
        Set(ByVal value As String)
            Me.sBankFaxCodeField = value
        End Set
    End Property

    Public Property BankFax() As String
        Get
            Return Me.sBankFaxField
        End Get
        Set(ByVal value As String)
            Me.sBankFaxField = value
        End Set
    End Property

    Public Property BankAddress() As Address
        Get
            Return Me.oBankAddressField
        End Get
        Set(ByVal value As Address)
            Me.oBankAddressField = value
        End Set
    End Property
End Class
