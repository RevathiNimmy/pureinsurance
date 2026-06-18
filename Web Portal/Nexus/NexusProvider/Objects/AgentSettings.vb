<Serializable()> Public Class AgentSettings

#Region "Private Variables"

    Private iAllowConsolidatedCommission As Integer
    Private iUseOverrideCommissionRate As Integer
    Private iDomiciledForTax As Integer
    Private iCanMakeLiveInvoice As Integer
    Private iCanMakeLiveInstalments As Integer
    Private iCanMakeLivePaynow As Integer
    Private iCanMakeLiveBankGuarantee As Integer
    Private bAllowConsolidatedCommission As Integer
    Private bUseOverrideCommissionRate As Integer
    Private bDomiciledForTax As Integer
    Private bCanMakeLiveInvoice As Integer
    Private bCanMakeLiveInstalments As Integer
    Private bCanMakeLivePaynow As Integer
    Private bCanMakeLiveBankGuarantee As Integer
    Private bCanMakeLiveInvoiceCashDeposit As Integer
    Private bIsStandardAccount As Boolean
    Private bIsFloatBalanceAccount As Boolean
    Private bIsOverdraftAccount As Boolean
    Private bIsPrepaymentAccount As Boolean
    Private dExpectedDailyPremium As Decimal
    Private iDaysAllowed As Integer
    Private dFloatBalanceLimit As Decimal
    Private dOverdraftLimit As Decimal
    Private dtOverdraftExpiry As Date
    Private bIsAlternateReferenceMandatory As Boolean
    Private iAlternateReferenceForEachTransaction As Integer
    Private sAssociatedUsers As UserCollection
    Private sAgentCorrespondenceType As String
    Private sContactField As ContactCollection
    Private bIsReceiveClientCorrespondenceField As Boolean
    Private oAgentBranchCollection As BranchesCollection
    Private dtAgencyCancellation As Date
    Private nAgentKey As Integer
#End Region


    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        'sbPrint.AppendLine("Name : " & sName.ToString() & "<br />")
        'sbPrint.AppendLine("Short Name : " & sShortname.ToString() & "<br />")
        'sbPrint.AppendLine("Address1 : " & sAddress1.ToString() & "<br />")
        'sbPrint.AppendLine("Address2 : " & sAddress2.ToString() & "<br />")
        'sbPrint.AppendLine("Address3 : " & sAddress3.ToString() & "<br />")
        'sbPrint.AppendLine("Address4 : " & sAddress4.ToString() & "<br />")
        'sbPrint.AppendLine("Postal Code : " & sPostalCode.ToString() & "<br />")
        'sbPrint.AppendLine("Number : " & sNumber.ToString() & "<br />")
        'sbPrint.AppendLine("Extension : " & iExtension.ToString() & "<br />")
        'sbPrint.AppendLine("Contact Type Key : " & iContactTypeKey.ToString() & "<br />")
        'sbPrint.AppendLine("Code : " & sCode.ToString() & "<br />")
        'sbPrint.AppendLine("Country Key : " & iCountryKey.ToString() & "<br />")
        'sbPrint.AppendLine("Description : " & sDescription.ToString() & "<br />")
        'sbPrint.AppendLine("Address Usage Type Key : " & iAddressUsageTypeKey.ToString() & "<br />")
        'sbPrint.AppendLine("AddressKey : " & iAddressKey.ToString() & "<br />")

        sbPrint.AppendLine("Allow Consolidated Commission : " & bAllowConsolidatedCommission.ToString() & "<br />")
        sbPrint.AppendLine("Is Alternate Reference Mandatory : " & bIsAlternateReferenceMandatory.ToString() & "<br />")
        sbPrint.AppendLine("Use Override Commission Rate : " & bUseOverrideCommissionRate.ToString() & "<br />")
        sbPrint.AppendLine("Domiciled For Tax : " & bDomiciledForTax.ToString() & "<br />")
        sbPrint.AppendLine("Can Make Live Invoice : " & bCanMakeLiveInvoice.ToString() & "<br />")
        sbPrint.AppendLine("Can Make Live Instalments : " & bCanMakeLiveInstalments.ToString() & "<br />")
        sbPrint.AppendLine("Can Make Live Paynow : " & bCanMakeLivePaynow.ToString() & "<br />")
        sbPrint.AppendLine("Can Make Live Bank Guarantee : " & bCanMakeLiveBankGuarantee.ToString() & "<br />")
        sbPrint.AppendLine("Can Make Live Invoice Cash Deposit : " & bCanMakeLiveInvoiceCashDeposit.ToString() & "<br />")
        sbPrint.AppendLine("Is Standard Account : " & bIsStandardAccount.ToString() & "<br />")
        sbPrint.AppendLine("Is Float Balance Account : " & bIsFloatBalanceAccount.ToString() & "<br />")
        sbPrint.AppendLine("Is Overdraft Account : " & bIsOverdraftAccount.ToString() & "<br />")
        sbPrint.AppendLine("Expected Daily Premium : " & dExpectedDailyPremium.ToString() & "<br />")
        sbPrint.AppendLine("Days Allowed : " & iDaysAllowed.ToString() & "<br />")
        sbPrint.AppendLine("Float Balance Limit : " & dFloatBalanceLimit.ToString() & "<br />")
        sbPrint.AppendLine("Over draft Limit : " & dOverdraftLimit.ToString() & "<br />")
        sbPrint.AppendLine("Overdraft Expiry Date: " & dtOverdraftExpiry.ToString() & "<br />")
        sbPrint.AppendLine("Alternate Reference For Each Transaction : " & iAlternateReferenceForEachTransaction.ToString() & "<br />")
        sbPrint.AppendLine("Agent Key : " & nAgentKey.ToString() & "<br />")


        Return sbPrint.ToString()

    End Function

#Region "Public Properties"

    ''' <summary>
    ''' AllowConsolidatedCommission=True/False
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AllowConsolidatedCommission() As Boolean
        Get
            Return Me.bAllowConsolidatedCommission
        End Get
        Set(ByVal value As Boolean)
            Me.bAllowConsolidatedCommission = value
        End Set
    End Property

    ''' <summary>
    ''' UseOverrideCommissionRate=True/False
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property UseOverrideCommissionRate() As Boolean
        Get
            Return Me.bUseOverrideCommissionRate
        End Get
        Set(ByVal value As Boolean)
            Me.bUseOverrideCommissionRate = value
        End Set
    End Property

    ''' <summary>
    ''' IsDomiciledForTax=True/False
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsDomiciledForTax() As Boolean
        Get
            Return Me.bDomiciledForTax
        End Get
        Set(ByVal value As Boolean)
            Me.bDomiciledForTax = value
        End Set
    End Property

    ''' <summary>
    ''' CanMakeLiveInvoice=True/False
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CanMakeLiveInvoice() As Boolean
        Get
            Return Me.bCanMakeLiveInvoice
        End Get
        Set(ByVal value As Boolean)
            Me.bCanMakeLiveInvoice = value
        End Set
    End Property

    ''' <summary>
    ''' CanMakeLiveInstalments=True/False
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CanMakeLiveInstalments() As Boolean
        Get
            Return Me.bCanMakeLiveInstalments
        End Get
        Set(ByVal value As Boolean)
            Me.bCanMakeLiveInstalments = value
        End Set
    End Property

    ''' <summary>
    ''' CanMakeLivePaynow=True/False
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CanMakeLivePaynow() As Boolean
        Get
            Return Me.bCanMakeLivePaynow
        End Get
        Set(ByVal value As Boolean)
            Me.bCanMakeLivePaynow = value
        End Set
    End Property

    ''' <summary>
    ''' CanMakeLiveBankGuarantee=True/False
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CanMakeLiveBankGuarantee() As Boolean
        Get
            Return Me.bCanMakeLiveBankGuarantee
        End Get
        Set(ByVal value As Boolean)
            Me.bCanMakeLiveBankGuarantee = value
        End Set
    End Property

    ''' <summary>
    ''' IsStandardAccount=True/False
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsStandardAccount() As Boolean
        Get
            Return Me.bIsStandardAccount
        End Get
        Set(ByVal value As Boolean)
            Me.bIsStandardAccount = value
        End Set
    End Property

    ''' <summary>
    ''' IsFloatBalanceAccount=True/False
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsFloatBalanceAccount() As Boolean
        Get
            Return Me.bIsFloatBalanceAccount
        End Get
        Set(ByVal value As Boolean)
            Me.bIsFloatBalanceAccount = value
        End Set
    End Property

    ''' <summary>
    ''' IsOverdraftAccount=True/False
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsOverdraftAccount() As Boolean
        Get
            Return Me.bIsOverdraftAccount
        End Get
        Set(ByVal value As Boolean)
            Me.bIsOverdraftAccount = value
        End Set
    End Property

    ''' <summary>
    ''' IsPrepaymentAccount=True/False
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsPrepaymentAccount() As Boolean
        Get
            Return Me.bIsPrepaymentAccount
        End Get
        Set(ByVal value As Boolean)
            Me.bIsPrepaymentAccount = value
        End Set
    End Property

    ''' <summary>
    ''' Expected Daily Premium
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ExpectedDailyPremium() As Decimal
        Get
            Return Me.dExpectedDailyPremium
        End Get
        Set(ByVal value As Decimal)
            Me.dExpectedDailyPremium = value
        End Set
    End Property

    ''' <summary>
    ''' Days Allowed
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property DaysAllowed() As Integer
        Get
            Return Me.iDaysAllowed
        End Get
        Set(ByVal value As Integer)
            Me.iDaysAllowed = value
        End Set
    End Property

    ''' <summary>
    ''' Float Balance Limit
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property FloatBalanceLimit() As Decimal
        Get
            Return Me.dFloatBalanceLimit
        End Get
        Set(ByVal value As Decimal)
            Me.dFloatBalanceLimit = value
        End Set
    End Property
    ''' <summary>
    ''' Overdraft Limit
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OverdraftLimit() As Decimal
        Get
            Return Me.dOverdraftLimit
        End Get
        Set(ByVal value As Decimal)
            Me.dOverdraftLimit = value
        End Set
    End Property

    ''' <summary>
    ''' Overdraft Expiry Date
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OverdraftExpiry() As Date
        Get
            Return Me.dtOverdraftExpiry
        End Get
        Set(ByVal value As Date)
            Me.dtOverdraftExpiry = value
        End Set
    End Property
    ''' <summary>
    ''' IsAlternateReferenceMandatory=True/False
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsAlternateReferenceMandatory() As Boolean
        Get
            Return bIsAlternateReferenceMandatory
        End Get
        Set(ByVal value As Boolean)
            bIsAlternateReferenceMandatory = value
        End Set
    End Property

    ''' <summary>
    ''' Alternate Reference For Each Transaction
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AlternateReferenceForEachTransaction() As Integer
        Get
            Return Me.iAlternateReferenceForEachTransaction
        End Get
        Set(ByVal value As Integer)
            Me.iAlternateReferenceForEachTransaction = value
        End Set
    End Property
    ''' <summary>
    ''' CanMakeLiveCashDeposit=True/False
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CanMakeLiveCashDeposit() As Boolean
        Get
            Return Me.bCanMakeLiveInvoiceCashDeposit
        End Get
        Set(ByVal value As Boolean)
            Me.bCanMakeLiveInvoiceCashDeposit = value
        End Set
    End Property

    ''' <summary>
    ''' Associated Users=True/False
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AssociatedUsers() As UserCollection
        Get
            Return sAssociatedUsers
        End Get
        Set(ByVal value As UserCollection)
            sAssociatedUsers = value
        End Set
    End Property
    ''' <summary>
    ''' Collection of branches associted with agent
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AgentBranchCollection() As BranchesCollection
        Get
            Return Me.oAgentBranchCollection
        End Get
        Set(ByVal value As BranchesCollection)
            Me.oAgentBranchCollection = value
        End Set
    End Property

    ''' <summary>
    ''' Agency Cancellation Date
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AgencyCancellationDate() As Date
        Get
            Return Me.dtAgencyCancellation
        End Get
        Set(ByVal value As Date)
            Me.dtAgencyCancellation = value
        End Set
    End Property

    ''' <summary>
    ''' Agency Key
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AgentKey() As Integer
        Get
            Return Me.nAgentKey
        End Get
        Set(ByVal value As Integer)
            Me.nAgentKey = value
        End Set
    End Property


    Public Sub New()
        oAgentBranchCollection = New BranchesCollection
    End Sub

    Public Property CorrespondenceType() As String
        Get
            Return Me.sAgentCorrespondenceType
        End Get
        Set(ByVal value As String)
            Me.sAgentCorrespondenceType = value
        End Set
    End Property

    Public Property Contacts() As ContactCollection
        Get
            Return sContactField
        End Get
        Set(ByVal value As ContactCollection)
            sContactField = value
        End Set
    End Property

    Public Property IsReceiveClientCorrespondence() As Boolean
        Get
            Return Me.bIsReceiveClientCorrespondenceField
        End Get
        Set(value As Boolean)
            Me.bIsReceiveClientCorrespondenceField = value
        End Set
    End Property
#End Region

End Class
