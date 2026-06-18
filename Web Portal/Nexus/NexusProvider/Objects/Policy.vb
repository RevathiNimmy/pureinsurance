<Serializable()> Public Class Policy : Inherits PolicySummary

    Private sAgentShortName As String
    Private sBranchCode As String
    Private iBranchKey As Integer
    Private dtCoverStartDate As DateTime
    Private sCurrencyCode As String
    Private dtDateIssued As DateTime
    Private dtExpiryDate As DateTime
    Private nGeminiPolicyStatus As Integer
    Private bGeminiPolicyStatusSpecified As Boolean
    Private nInsuranceFileID As Integer
    Private nInsuranceFileKey As Integer
    Private sInsuranceFileTypeCode As String
    Private nInsuranceFolderKey As Integer
    Private nInsuredKey As Integer
    Private bInsuredKeySpecified As Boolean
    Private sInsurerShortName As String
    Private sInsurerName As String
    Private nLeadAgentKey As Integer
    Private bLeadAgentKeySpecified As Boolean
    Private nLeadInsurerKey As Integer
    Private bLeadInsurerKeySpecified As Boolean
    Private bMTAAllowed As Boolean
    Private oMTAs As MTACollection
    Private sPartyShortName As String
    Private sPolicyStatusCode As String
    Private sPolicyTypeCode As String
    Private sPolicyTypeDescription As String
    Private nPolicyTypeID As Integer
    Private bPolicyTypeIDSpecified As Boolean
    Private nPolicyVersion As String
    Private sProductCode As String
    Private sProductDesc As String
    Private iProductKey As Integer
    Private sRegarding As String
    Private dtRenewalDate As DateTime
    Private sRiskCodeDescription As String
    Private sEventDesc As String
    Private nGracePeriod As Integer
    Private sHandler As String
    Private sInsuranceFileTypeDesc As String
    Private nInsuranceFileTypeKey As Integer
    Private nInsuranceHolderKey As Integer
    Private sInsurer As String
    Private sLegalExpenseProvider As String
    Private sPaymentFrequency As String
    Private sPaymentMethod As String
    Private dtQuoteExpiryDate As DateTime
    Private sSchemeName As String
    Private nRenewalStatusKey As Integer
    Private nPartyKey As Integer
    Private sPartyName As String
    Private sInsuranceFileRef As String
    Private sInsuranceFileStatusDescription As String
    Private sInsuranceFileTypeDescription As String
    Private sRenewalStatusTypeCode As String
    Private sRenewalStatusTypeDescription As String
    Private dtCoverEndDate As DateTime
    Private dRenewalPremium As Decimal
    Private sLeadAgent As String
    Private sAccHandler As String
    Private bClaimIndicator As Boolean
    Private bIsClosed As Boolean
    Private bIsTrueMonthlyPolicy As Boolean
    Private bAnniversaryCopy As Boolean
    Private sClientCode As String
    Private sClient As String
    Private bIsAutoRenewable As Boolean
    Private bIsInTransferMode As Boolean
    Private nRenewalCount As Integer
    Private dPremium As Double
    Private dTaxAmount As Double
    Private sInstalmentPlanStatus As String
    Private sPreviousVersionInstalmentPlanStatus As String
    Private sAlternativeRef As String
    Private sPolicyStatus As String
    Private dLapseCancelDate As Date
    Private sInsuredPersons As String
    Private sCurrency As String
    Private sIntermediary As String
    Private dTransactionDate As Date
    Private dThisPremium As Double
    Private dAnnualPremium As Double
    Private dNetPremium As Double
    Private bIsSelected As Boolean
    'added for Marked For Collection 
    Private bMarkedQuoteForCollection As Boolean

    Private sDocumentRef As String

    Private iWriteOffReasonKey As Integer

    Private dWriteOffAmount As Decimal

    Private bIsCurrencyWriteOff, bIsCurrent, bIsReadOnly As Boolean

    Private dAmountTobeAllocated As Decimal

    Private iBGKey As Integer

    Private sPolicyRef As String
    Private sRiskTypeDescription As String
    Private bIsMigratedPolicy As Boolean

    Private sClientName, sAgentCode, sBranchDesc As String

    'Newly Added Properties as per WPR 63
    Private iBaseInsuranceFolderKey As Integer
    Private iQuoteVersion As Integer
    Private iQuoteStatusKey As Integer
    Private iRenewedVersion As Integer
    Private sRiskStatus As String
    Private sCorrespondenceType As String
    Private sDefaultPreferredCorrespondence As String
    Private bIsAgentReceiveCorrespondence As Boolean


    Dim sPTRIStatus As String
    Dim nNewInsuranceFileKey As Integer
    Dim nInsuranceFilePTRIUsageId As Integer
    Private sAssociatedClients As String = String.Empty

    'Newly Added Enum as per WPR 63
    Public Enum QuoteStatusType
        None = 0
        Pending = 1
        AgentPending = 2
        AgentComplete = 3
        Issued = 4
        Live = 5
        Declined = 6
    End Enum



    'For GetPartyPolicies modification
    Private iClosePolicyClaims, iInsuranceFileSourceKey, iOpenPolicyClaims As Integer
    Private sInsuranceDesc, sLastTransDesc, sQuoteStatus, AssociatedClientsField As String

    '[START] added for WPR63
    Public Property BaseInsuranceFolderKey() As Integer
        Get
            Return iBaseInsuranceFolderKey
        End Get
        Set(ByVal value As Integer)
            iBaseInsuranceFolderKey = value
        End Set
    End Property

    Public Property QuoteVersion() As Integer
        Get
            Return iQuoteVersion
        End Get
        Set(ByVal value As Integer)
            iQuoteVersion = value
        End Set
    End Property

    ''' <summary>
    ''' To get or set the quote status
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property QuoteStatus() As String


    Public Property QuoteStatusKey() As QuoteStatusType
        Get
            Return iQuoteStatusKey
        End Get
        Set(ByVal value As QuoteStatusType)
            iQuoteStatusKey = value
        End Set
    End Property

    Public Property RenewedVersion() As Integer
        Get
            Return iRenewedVersion
        End Get
        Set(ByVal value As Integer)
            iRenewedVersion = value
        End Set
    End Property

    Public Property RiskStatus() As String
        Get
            Return sRiskStatus
        End Get
        Set(ByVal value As String)
            sRiskStatus = value
        End Set
    End Property

    '[END] added for WPR63
    'added for Marked For Collection 
    Public Property MarkedQuoteForCollection() As Boolean
        Get
            Return Me.bMarkedQuoteForCollection
        End Get
        Set(ByVal value As Boolean)
            Me.bMarkedQuoteForCollection = value
        End Set
    End Property
    Public Property IsCurrent() As Boolean
        Get
            Return Me.bIsCurrent
        End Get
        Set(ByVal value As Boolean)
            Me.bIsCurrent = value
        End Set
    End Property
    Public Property ClosePolicyClaims() As Integer
        Get
            Return Me.iClosePolicyClaims
        End Get
        Set(ByVal value As Integer)
            Me.iClosePolicyClaims = value
        End Set
    End Property
    Public Property OpenPolicyClaims() As Integer
        Get
            Return Me.iOpenPolicyClaims
        End Get
        Set(ByVal value As Integer)
            Me.iOpenPolicyClaims = value
        End Set
    End Property
    Public Property InsuranceFileSourceKey() As Integer
        Get
            Return Me.iInsuranceFileSourceKey
        End Get
        Set(ByVal value As Integer)
            Me.iInsuranceFileSourceKey = value
        End Set
    End Property
    Public Property InsuranceDesc() As String
        Get
            Return sInsuranceDesc
        End Get
        Set(ByVal value As String)
            sInsuranceDesc = value
        End Set
    End Property
    Public Property LastTransDesc() As String
        Get
            Return sLastTransDesc
        End Get
        Set(ByVal value As String)
            sLastTransDesc = value
        End Set
    End Property

    Public Property RiskTypeDescription() As String
        Get
            Return sRiskTypeDescription
        End Get
        Set(ByVal value As String)
            sRiskTypeDescription = value
        End Set
    End Property
    Public Property NetPremium() As Double
        Get
            Return dNetPremium
        End Get
        Set(ByVal value As Double)
            dNetPremium = value
        End Set
    End Property
    Public Property AnnualPremium() As Double
        Get
            Return dAnnualPremium
        End Get
        Set(ByVal value As Double)
            dAnnualPremium = value
        End Set
    End Property
    Public Property ThisPremium() As Double
        Get
            Return dThisPremium
        End Get
        Set(ByVal value As Double)
            dThisPremium = value
        End Set
    End Property
    Public Property TransactionDate() As Date
        Get
            Return dTransactionDate
        End Get
        Set(ByVal value As Date)
            dTransactionDate = value
        End Set
    End Property
    Public Property Intermediary() As String
        Get
            Return sIntermediary
        End Get
        Set(ByVal value As String)
            sIntermediary = value
        End Set
    End Property
    Public Property Currency() As String
        Get
            Return sCurrency
        End Get
        Set(ByVal value As String)
            sCurrency = value
        End Set
    End Property
    Public Property InsuredPersons() As String
        Get
            Return sInsuredPersons
        End Get
        Set(ByVal value As String)
            sInsuredPersons = value
        End Set
    End Property
    Public Property LapseCancelDate() As Date
        Get
            Return dLapseCancelDate
        End Get
        Set(ByVal value As Date)
            dLapseCancelDate = value
        End Set
    End Property
    Public Property PolicyStatus() As String
        Get
            Return sPolicyStatus
        End Get
        Set(ByVal value As String)
            sPolicyStatus = value
        End Set
    End Property
    Public Property AlternativeRef() As String
        Get
            Return sAlternativeRef
        End Get
        Set(ByVal value As String)
            sAlternativeRef = value
        End Set
    End Property
    Public Property InstalmentPlanStatus() As String
        Get
            Return sInstalmentPlanStatus
        End Get
        Set(ByVal value As String)
            sInstalmentPlanStatus = value
        End Set
    End Property
    Public Property PreviousVersionInstalmentPlanStatus() As String
        Get
            Return sPreviousVersionInstalmentPlanStatus
        End Get
        Set(ByVal value As String)
            sPreviousVersionInstalmentPlanStatus = value
        End Set
    End Property
    Public Property PolicyRef() As String
        Get
            Return sPolicyRef
        End Get
        Set(ByVal value As String)
            sPolicyRef = value
        End Set
    End Property

    Public Property TaxAmount() As Double
        Get
            Return dTaxAmount
        End Get
        Set(ByVal value As Double)
            dTaxAmount = value
        End Set
    End Property
    Public Property Premium() As Double
        Get
            Return dPremium
        End Get
        Set(ByVal value As Double)
            dPremium = value
        End Set
    End Property
    Public Property ClientName() As String
        Get
            Return sClientName
        End Get
        Set(ByVal value As String)
            sClientName = value
        End Set
    End Property
    Public Property AgentCode() As String
        Get
            Return sAgentCode
        End Get
        Set(ByVal value As String)
            sAgentCode = value
        End Set
    End Property
    Public Property BranchDescription() As String
        Get
            Return sBranchDesc
        End Get
        Set(ByVal value As String)
            sBranchDesc = value
        End Set
    End Property


    '''<remarks/>
    Public Property DocumentRef() As String
        Get
            Return Me.sDocumentRef
        End Get
        Set(ByVal value As String)
            Me.sDocumentRef = value
        End Set
    End Property

    '''<remarks/>
    Public Property WriteOffReasonKey() As Integer
        Get
            Return Me.iWriteOffReasonKey
        End Get
        Set(ByVal value As Integer)
            Me.iWriteOffReasonKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property WriteOffAmount() As Decimal
        Get
            Return Me.dWriteOffAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dWriteOffAmount = value
        End Set
    End Property
    '''<remarks/>
    Public Property IsSelected() As Boolean
        Get
            Return Me.bIsSelected
        End Get
        Set(ByVal value As Boolean)
            Me.bIsSelected = value
        End Set
    End Property
    '''<remarks/>
    Public Property IsCurrencyWriteOff() As Boolean
        Get
            Return Me.bIsCurrencyWriteOff
        End Get
        Set(ByVal value As Boolean)
            Me.bIsCurrencyWriteOff = value
        End Set
    End Property

    '''<remarks/>
    Public Property AmountTobeAllocated() As Decimal
        Get
            Return Me.dAmountTobeAllocated
        End Get
        Set(ByVal value As Decimal)
            Me.dAmountTobeAllocated = value
        End Set
    End Property

    '''<remarks/>
    Public Property BGKey() As Integer
        Get
            Return Me.iBGKey
        End Get
        Set(ByVal value As Integer)
            Me.iBGKey = value
        End Set
    End Property

    ''' <summary>
    ''' Initialize Base Insurance File key
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' 
    Public Property BaseInsuranceFileKey As Integer = 0

    ''' <summary>
    ''' Identify Reinstate link should be visible on Policy Version
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsReinstateLinkVersion As Boolean = False

    ''' <summary>
    ''' To check policy created from Market Place 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsMarketPlacePolicy As Boolean = False
    Public Property RiskNumber As Integer = 0

    Public Property RiskDescription As String = String.Empty
    Public Property AssociatedClients As String
    Public Property CorrespondenceType As String
        Get
            Return Me.sCorrespondenceType
        End Get
        Set(value As String)
            Me.sCorrespondenceType = value
        End Set
    End Property

    Public Property DefaultPreferredCorrespondence As String
        Get
            Return Me.sDefaultPreferredCorrespondence
        End Get
        Set(value As String)
            Me.sDefaultPreferredCorrespondence = value
        End Set
    End Property

    Public Property IsAgentReceiveCorrespondence As Boolean
        Get
            Return Me.bIsAgentReceiveCorrespondence
        End Get
        Set(value As Boolean)
            Me.bIsAgentReceiveCorrespondence = value
        End Set
    End Property

    ''' <summary>
    ''' To check if policy version is readonly
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    'Public Property IsReadOnly As Boolean = False
    Public Property IsReadOnly() As Boolean
        Get
            Return Me.bIsReadOnly
        End Get
        Set(ByVal value As Boolean)
            Me.bIsReadOnly = value
        End Set
    End Property

    Public Sub New(ByVal v_sReference As String)
        MyBase.New(v_sReference)

        MTAs = New MTACollection()

        bGeminiPolicyStatusSpecified = False
        bInsuredKeySpecified = False
        bLeadInsurerKeySpecified = False
    End Sub


    ''' <summary>
    ''' A default Constructer added as got a requirement for initializing the class without passing any parameter
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()

    End Sub

    Public Overrides Function Print() As String

        Dim sbPrint As New Text.StringBuilder(MyBase.Print())

        sbPrint.AppendLine("Agent Short Name : " & sAgentShortName & "<br />")
        sbPrint.AppendLine("Branch Code : " & sBranchCode & "<br />")
        sbPrint.AppendLine("Branch Key" & iBranchKey.ToString() & "<br />")
        sbPrint.AppendLine("Cover Start Date : " & dtCoverStartDate.ToString() & "<br />")
        sbPrint.AppendLine("Currency Code : " & sCurrencyCode & "<br />")
        sbPrint.AppendLine("Date Issued : " & dtDateIssued.ToString() & "<br />")
        sbPrint.AppendLine("Expiry Date : " & dtExpiryDate.ToString() & "<br />")
        sbPrint.AppendLine("Gemini Policy Status : " & nGeminiPolicyStatus.ToString() & "<br />")
        sbPrint.AppendLine("Gemini Policy Status Specified : " & IIf(bGeminiPolicyStatusSpecified, "true", "false") & "<br />")
        sbPrint.AppendLine("Insurance File ID : " & nInsuranceFileID.ToString() & "<br />")
        sbPrint.AppendLine("Insurance File Key : " & nInsuranceFileKey.ToString() & "<br />")
        sbPrint.AppendLine("Insurance File Type Code : " & sInsuranceFileTypeCode & "<br />")
        sbPrint.AppendLine("Insurance Folder Key : " & nInsuranceFolderKey.ToString() & "<br />")
        sbPrint.AppendLine("Insured Key : " & nInsuredKey.ToString() & "<br />")
        sbPrint.AppendLine("Insured Key Specified : " & IIf(bInsuredKeySpecified, "true", "false") & "<br />")
        sbPrint.AppendLine("Insurer Short Name : " & sInsurerShortName & "<br />")
        sbPrint.AppendLine("Lead Agent Key : " & nLeadAgentKey.ToString() & "<br />")
        sbPrint.AppendLine("Lead Agent Key Specified : " & IIf(bLeadAgentKeySpecified, "true", "false") & "<br />")
        sbPrint.AppendLine("Lead Insurer Key : " & nLeadInsurerKey.ToString() & "<br />")
        sbPrint.AppendLine("Lead Insurer Key Specified : " & IIf(bLeadInsurerKeySpecified, "true", "false") & "<br />")
        sbPrint.AppendLine("MTA Allowed : " & IIf(bMTAAllowed, "true", "false") & "<br />")
        sbPrint.AppendLine("MTAs ---------------><br />")

        If oMTAs IsNot Nothing Then

            For Each oMTA As MTA In oMTAs
                sbPrint.AppendLine(oMTA.Print())
                sbPrint.AppendLine("<br />")
            Next

        End If

        sbPrint.AppendLine("Party Short Name :" & sPartyShortName & "<br />")
        sbPrint.AppendLine("Policy Status Code : " & sPolicyStatusCode & "<br />")
        sbPrint.AppendLine("Policy Type Code : " & sPolicyTypeCode & "<br />")
        sbPrint.AppendLine("Policy Type Description : " & sPolicyTypeDescription & "<br />")
        sbPrint.AppendLine("Policy Type ID : " & nPolicyTypeID.ToString() & "<br />")
        sbPrint.AppendLine("Policy Type ID Specified : " & IIf(bPolicyTypeIDSpecified, "true", "false") & "<br />")
        sbPrint.AppendLine("Policy Version : " & nPolicyVersion.ToString() & "<br />")
        sbPrint.AppendLine("Product Code : " & sProductCode & "<br />")
        sbPrint.AppendLine("Product Desc : " & sProductDesc & "<br />")
        sbPrint.AppendLine("Product Key : " & iProductKey.ToString() & "<br />")
        sbPrint.AppendLine("Reagrding : " & sRegarding & "<br />")
        sbPrint.AppendLine("Renewal Date : " & dtRenewalDate.ToString() & "<br />")
        sbPrint.AppendLine("Risk Code Description : " & sRiskCodeDescription & "<br />")
        sbPrint.AppendLine("Event Desc : " & sEventDesc & "<br />")
        sbPrint.AppendLine("Grace Period : " & nGracePeriod.ToString() & "<br />")
        sbPrint.AppendLine("Handler : " & sHandler & "<br />")
        sbPrint.AppendLine("Insurance File Type Desc : " & sInsuranceFileTypeDesc & "<br />")
        sbPrint.AppendLine("Insurance File Type Key : " & nInsuranceFileTypeKey.ToString() & "<br />")
        sbPrint.AppendLine("Insurance Holder Key : " & nInsuranceHolderKey.ToString() & "<br />")
        sbPrint.AppendLine("Insurer : " & sInsurer & "<br />")
        sbPrint.AppendLine("Legal Expense Provider : " & sLegalExpenseProvider & "<br />")
        sbPrint.AppendLine("Payment Frequency : " & sPaymentFrequency & "<br />")
        sbPrint.AppendLine("Payment Method : " & sPaymentMethod & "<br />")
        sbPrint.AppendLine("Quote Expiry Date : " & dtQuoteExpiryDate.ToString() & "<br />")
        sbPrint.AppendLine("Scheme Name : " & sSchemeName & "<br />")
        sbPrint.AppendLine("Renewal Status Key: " & nRenewalStatusKey & "<br />")
        sbPrint.AppendLine("Party Key: " & nPartyKey & "<br />")
        sbPrint.AppendLine("Party Name: " & sPartyName & "<br />")
        sbPrint.AppendLine("Insurance File Ref: " & sInsuranceFileRef & "<br />")
        sbPrint.AppendLine("Insurance File Status Description: " & sInsuranceFileStatusDescription & "<br />")
        sbPrint.AppendLine("Insurance File Type Description: " & sInsuranceFileTypeDescription & "<br />")
        sbPrint.AppendLine("Renewal Status TypeCode: " & sRenewalStatusTypeCode & "<br />")
        sbPrint.AppendLine("Renewal Status Type Description: " & sRenewalStatusTypeDescription & "<br />")
        sbPrint.AppendLine("Cover End Date: " & dtCoverEndDate & "<br />")
        sbPrint.AppendLine("Renewal Premium: " & dRenewalPremium & "<br />")
        sbPrint.AppendLine("Lead Agent: " & sLeadAgent & "<br />")
        sbPrint.AppendLine("AccHandler: " & sAccHandler & "<br />")
        sbPrint.AppendLine("Claim Indicator: " & bClaimIndicator & "<br />")
        sbPrint.AppendLine("Is Closed: " & bIsClosed & "<br />")
        sbPrint.AppendLine("Is True Monthly Policy: " & bIsTrueMonthlyPolicy & "<br />")
        sbPrint.AppendLine("Anniversary Copy: " & bAnniversaryCopy & "<br />")
        sbPrint.AppendLine("Client Code: " & sClientCode & "<br />")
        sbPrint.AppendLine("Client: " & sClient & "<br />")
        sbPrint.AppendLine("Is Auto Renewable: " & bIsAutoRenewable & "<br />")
        sbPrint.AppendLine("Is In Transfer Mode: " & bIsInTransferMode & "<br />")
        sbPrint.AppendLine("Renewal Count: " & nRenewalCount & "<br />")

        Return sbPrint.ToString()

    End Function

    Public Property AgentShortName() As String
        Get
            Return sAgentShortName
        End Get
        Set(ByVal value As String)
            sAgentShortName = value
        End Set
    End Property

    Public Property BranchCode() As String
        Get
            Return sBranchCode
        End Get
        Set(ByVal value As String)
            sBranchCode = value
        End Set
    End Property

    Public Property BranchKey() As Integer
        Get
            Return iBranchKey
        End Get
        Set(ByVal value As Integer)
            iBranchKey = value
        End Set
    End Property

    Public Property CoverStartDate() As DateTime
        Get
            Return dtCoverStartDate
        End Get
        Set(ByVal value As DateTime)
            dtCoverStartDate = value
        End Set
    End Property

    Public ReadOnly Property CoverStartDateSpecified() As Boolean
        Get
            IIf(dtCoverStartDate = Date.MinValue, False, True)
        End Get
    End Property

    Public Property CurrencyCode() As String
        Get
            Return sCurrencyCode
        End Get
        Set(ByVal value As String)
            sCurrencyCode = value
        End Set
    End Property

    Public Property DateIssued() As DateTime
        Get
            Return dtDateIssued
        End Get
        Set(ByVal value As DateTime)
            dtDateIssued = value
        End Set
    End Property

    Public ReadOnly Property DateIssuedSpecified() As Boolean
        Get
            Return IIf(dtDateIssued = Date.MinValue, False, True)
        End Get
    End Property

    Public Property ExpiryDate() As DateTime
        Get
            Return dtExpiryDate
        End Get
        Set(ByVal value As DateTime)
            dtExpiryDate = value
        End Set
    End Property

    Public ReadOnly Property ExpiryDateSpecified() As Boolean
        Get
            Return IIf(dtExpiryDate = Date.MinValue, False, True)
        End Get
    End Property

    Public Property GeminiPolicyStatus() As Integer
        Get
            Return nGeminiPolicyStatus
        End Get
        Set(ByVal value As Integer)
            nGeminiPolicyStatus = value
            bGeminiPolicyStatusSpecified = True
        End Set
    End Property

    Public ReadOnly Property GeminiPolicyStatusSpecfied() As Boolean
        Get
            Return bGeminiPolicyStatusSpecified
        End Get
    End Property

    Public Property InsuranceFileID() As Integer
        Get
            Return nInsuranceFileID
        End Get
        Set(ByVal value As Integer)
            nInsuranceFileID = value
        End Set
    End Property

    Public Property InsuranceFileKey() As Integer
        Get
            Return nInsuranceFileKey
        End Get
        Set(ByVal value As Integer)
            nInsuranceFileKey = value
        End Set
    End Property

    Public Property InsuranceFileTypeCode() As String
        Get
            Return sInsuranceFileTypeCode
        End Get
        Set(ByVal value As String)
            sInsuranceFileTypeCode = value
        End Set
    End Property

    Public Property InsuranceFolderKey() As Integer
        Get
            Return nInsuranceFolderKey
        End Get
        Set(ByVal value As Integer)
            nInsuranceFolderKey = value
        End Set
    End Property

    Public Property InsuredKey() As Integer
        Get
            Return nInsuredKey
        End Get
        Set(ByVal value As Integer)
            nInsuredKey = value
            bInsuredKeySpecified = True
        End Set
    End Property

    Public ReadOnly Property InsuredKeySpecified() As Boolean
        Get
            Return bInsuredKeySpecified
        End Get
    End Property

    Public Property InsurerShortName() As String
        Get
            Return sInsurerShortName
        End Get
        Set(ByVal value As String)
            sInsurerShortName = value
        End Set
    End Property

    Public Property InsurerName() As String
        Get
            Return sInsurerName
        End Get
        Set(ByVal value As String)
            sInsurerName = value
        End Set
    End Property

    Public Property LeadAgentKey() As Integer
        Get
            Return nLeadAgentKey
        End Get
        Set(ByVal value As Integer)
            nLeadAgentKey = value
            bLeadAgentKeySpecified = True
        End Set
    End Property

    Public ReadOnly Property LeadAgentKeySpecified() As Boolean
        Get
            Return bLeadAgentKeySpecified
        End Get
    End Property

    Public Property LeadInsurerKey() As Integer
        Get
            Return nLeadInsurerKey
        End Get
        Set(ByVal value As Integer)
            nLeadInsurerKey = value
            bLeadInsurerKeySpecified = True
        End Set
    End Property

    Public ReadOnly Property LeadInsurerKeySpecified() As Boolean
        Get
            Return bLeadInsurerKeySpecified
        End Get
    End Property

    Public Property MTAAllowed() As Boolean
        Get
            Return bMTAAllowed
        End Get
        Set(ByVal value As Boolean)
            bMTAAllowed = value
        End Set
    End Property

    Public Property MTAs() As MTACollection
        Get
            Return oMTAs
        End Get
        Set(ByVal value As MTACollection)
            oMTAs = value
        End Set
    End Property

    Public Property PartyShortName() As String
        Get
            Return sPartyShortName
        End Get
        Set(ByVal value As String)
            sPartyShortName = value
        End Set
    End Property

    Public Property PolicyStatusCode() As String
        Get
            Return sPolicyStatusCode
        End Get
        Set(ByVal value As String)
            sPolicyStatusCode = value
        End Set
    End Property

    Public Property PolicyTypeCode() As String
        Get
            Return sPolicyTypeCode
        End Get
        Set(ByVal value As String)
            sPolicyTypeCode = value
        End Set
    End Property

    Public Property PolicyTypeDescription() As String
        Get
            Return sPolicyTypeDescription
        End Get
        Set(ByVal value As String)
            sPolicyTypeDescription = value
        End Set
    End Property

    Public Property PolicyTypeID() As Integer
        Get
            Return nPolicyTypeID
        End Get
        Set(ByVal value As Integer)
            nPolicyTypeID = value
            bPolicyTypeIDSpecified = True
        End Set
    End Property

    Public ReadOnly Property PolicyTypeIDSpecified() As Boolean
        Get
            Return bPolicyTypeIDSpecified
        End Get
    End Property

    Public Property PolicyVersion() As String
        Get
            Return nPolicyVersion
        End Get
        Set(ByVal value As String)
            nPolicyVersion = value
        End Set
    End Property

    Public Property ProductCode() As String
        Get
            Return sProductCode
        End Get
        Set(ByVal value As String)
            sProductCode = value
        End Set
    End Property

    Public Property ProductDescription() As String
        Get
            Return sProductDesc
        End Get
        Set(ByVal value As String)
            sProductDesc = value
        End Set
    End Property

    Public Property ProductKey() As Integer
        Get
            Return iProductKey
        End Get
        Set(ByVal value As Integer)
            iProductKey = value
        End Set
    End Property

    Public Property Regarding() As String
        Get
            Return sRegarding
        End Get
        Set(ByVal value As String)
            sRegarding = value
        End Set
    End Property

    Public Property RenewalDate() As DateTime
        Get
            Return dtRenewalDate
        End Get
        Set(ByVal value As DateTime)
            dtRenewalDate = value
        End Set
    End Property

    Public ReadOnly Property RenewalDateSpecified() As DateTime
        Get
            Return IIf(dtRenewalDate = Date.MinValue, False, True)
        End Get
    End Property

    Public Property RiskCodeDescription() As String
        Get
            Return sRiskCodeDescription
        End Get
        Set(ByVal value As String)
            sRiskCodeDescription = value
        End Set
    End Property

    Public Property EventDesc() As String
        Get
            Return sEventDesc
        End Get
        Set(ByVal value As String)
            sEventDesc = value
        End Set
    End Property

    Public Property GracePeriod() As Integer
        Get
            Return nGracePeriod
        End Get
        Set(ByVal value As Integer)
            nGracePeriod = value
        End Set
    End Property

    Public Property Handler() As String
        Get
            Return sHandler
        End Get
        Set(ByVal value As String)
            sHandler = value
        End Set
    End Property

    Public Property InsuranceFileTypeDesc() As String
        Get
            Return sInsuranceFileTypeDesc
        End Get
        Set(ByVal value As String)
            sInsuranceFileTypeDesc = value
        End Set
    End Property

    Public Property InsuranceFileTypeKey() As Integer
        Get
            Return nInsuranceFileTypeKey
        End Get
        Set(ByVal value As Integer)
            nInsuranceFileTypeKey = value
        End Set
    End Property

    Public Property InsuranceHolderKey() As Integer
        Get
            Return nInsuranceHolderKey
        End Get
        Set(ByVal value As Integer)
            nInsuranceHolderKey = value
        End Set
    End Property

    Public Property Insurer() As String
        Get
            Return sInsurer
        End Get
        Set(ByVal value As String)
            sInsurer = value
        End Set
    End Property

    Public Property LegalExpenseProvider() As String
        Get
            Return sLegalExpenseProvider
        End Get
        Set(ByVal value As String)
            sLegalExpenseProvider = value
        End Set
    End Property

    Public Property PaymentFrequency() As String
        Get
            Return sPaymentFrequency
        End Get
        Set(ByVal value As String)
            sPaymentFrequency = value
        End Set
    End Property

    Public Property PaymentMethod() As String
        Get
            Return sPaymentMethod
        End Get
        Set(ByVal value As String)
            sPaymentMethod = value
        End Set
    End Property

    Public Property QuoteExpiryDate() As DateTime
        Get
            Return dtQuoteExpiryDate
        End Get
        Set(ByVal value As DateTime)
            dtQuoteExpiryDate = value
        End Set
    End Property

    Public Property SchemeName() As String
        Get
            Return sSchemeName
        End Get
        Set(ByVal value As String)
            sSchemeName = value
        End Set
    End Property

    Public Property RenewalStatusKey() As Integer
        Get
            Return nRenewalStatusKey
        End Get
        Set(ByVal value As Integer)
            nRenewalStatusKey = value
        End Set
    End Property

    Public Property PartyKey() As Integer
        Get
            Return nPartyKey
        End Get
        Set(ByVal value As Integer)
            nPartyKey = value
        End Set
    End Property

    Public Property PartyName() As String
        Get
            Return sPartyName
        End Get
        Set(ByVal value As String)
            sPartyName = value
        End Set
    End Property

    Public Property InsuranceFileRef() As String
        Get
            Return sInsuranceFileRef
        End Get
        Set(ByVal value As String)
            sInsuranceFileRef = value
        End Set
    End Property

    Public Property InsuranceFileStatusDescription() As String
        Get
            Return sInsuranceFileStatusDescription
        End Get
        Set(ByVal value As String)
            sInsuranceFileStatusDescription = value
        End Set
    End Property

    Public Property InsuranceFileTypeDescription() As String
        Get
            Return sInsuranceFileTypeDescription
        End Get
        Set(ByVal value As String)
            sInsuranceFileTypeDescription = value
        End Set
    End Property

    Public Property RenewalStatusTypeCode() As String
        Get
            Return sRenewalStatusTypeCode
        End Get
        Set(ByVal value As String)
            sRenewalStatusTypeCode = value
        End Set
    End Property

    Public Property RenewalStatusTypeDescription() As String
        Get
            Return sRenewalStatusTypeDescription
        End Get
        Set(ByVal value As String)
            sRenewalStatusTypeDescription = value
        End Set
    End Property

    Public Property CoverEndDate() As DateTime
        Get
            Return dtCoverEndDate
        End Get
        Set(ByVal value As DateTime)
            dtCoverEndDate = value
        End Set
    End Property

    Public Property RenewalPremium() As Decimal
        Get
            Return dRenewalPremium
        End Get
        Set(ByVal value As Decimal)
            dRenewalPremium = value
        End Set
    End Property

    Public Property LeadAgent() As String
        Get
            Return sLeadAgent
        End Get
        Set(ByVal value As String)
            sLeadAgent = value
        End Set
    End Property

    Public Property AccHandler() As String
        Get
            Return sAccHandler
        End Get
        Set(ByVal value As String)
            sAccHandler = value
        End Set
    End Property

    Public Property ClaimIndicator() As Boolean
        Get
            Return bClaimIndicator
        End Get
        Set(ByVal value As Boolean)
            bClaimIndicator = value
        End Set
    End Property

    Public Property IsClosed() As Boolean
        Get
            Return bIsClosed
        End Get
        Set(ByVal value As Boolean)
            bIsClosed = value
        End Set
    End Property

    Public Property IsTrueMonthlyPolicy() As Boolean
        Get
            Return bIsTrueMonthlyPolicy
        End Get
        Set(ByVal value As Boolean)
            bIsTrueMonthlyPolicy = value
        End Set
    End Property

    Public Property AnniversaryCopy() As Boolean
        Get
            Return bAnniversaryCopy
        End Get
        Set(ByVal value As Boolean)
            bAnniversaryCopy = value
        End Set
    End Property

    Public Property ClientCode() As String
        Get
            Return sClientCode
        End Get
        Set(ByVal value As String)
            sClientCode = value
        End Set
    End Property

    Public Property Client() As String
        Get
            Return sClient
        End Get
        Set(ByVal value As String)
            sClient = value
        End Set
    End Property

    Public Property IsAutoRenewable() As Boolean
        Get
            Return bIsAutoRenewable
        End Get
        Set(ByVal value As Boolean)
            bIsAutoRenewable = value
        End Set
    End Property

    Public Property IsInTransferMode() As Boolean
        Get
            Return bIsInTransferMode
        End Get
        Set(ByVal value As Boolean)
            bIsInTransferMode = value
        End Set
    End Property

    Public Property RenewalCount() As Integer
        Get
            Return nRenewalCount
        End Get
        Set(ByVal value As Integer)
            nRenewalCount = value
        End Set
    End Property

    Public Property IsMigratedPolicy() As Boolean
        Get
            Return bIsMigratedPolicy
        End Get
        Set(ByVal value As Boolean)
            bIsMigratedPolicy = value
        End Set
    End Property


    Public Property InsuranceFilePTRIUsageId() As Integer
        Get
            Return nInsuranceFilePTRIUsageId
        End Get
        Set(ByVal value As Integer)
            nInsuranceFilePTRIUsageId = value
        End Set
    End Property

    Public Property NewInsuranceFileKey() As Integer
        Get
            Return nNewInsuranceFileKey
        End Get
        Set(ByVal value As Integer)
            nNewInsuranceFileKey = value
        End Set
    End Property


    Public Property PTRIStatus() As String
        Get
            Return sPTRIStatus
        End Get
        Set(ByVal value As String)
            sPTRIStatus = value
        End Set
    End Property


    ''' <summary>
    ''' To get or set the out of sequence replaced variable
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsOutOfSequenceReplaced() As Boolean

    ''' <summary>
    ''' to get or set the original MTA premium
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OriginalMtaPremium() As Double

    ''' <summary>
    ''' to get or set the Reversed insurance file key
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ReversedInsuranceFileCnt() As Integer

    ''' <summary>
    ''' To get or set the original commission
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OriginalCommission() As Double

    ''' <summary>
    ''' To get or set the MTA Commission
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MtaCommission() As Double

    ''' <summary>
    ''' To get or set the Original Fee
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OriginalFee() As Double

    ''' <summary>
    ''' to get or set the MTA Fee
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MtaFee() As Double


    ''' <summary>
    ''' To know the previous changed insurance file key
    ''' This will be used for OOS MTA on backdated policy versions screen
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PrechangeInsuranceFileKey() As Integer


    <Serializable()> Public Class SortByReference : Implements IComparer
        Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare

            Dim oLeft, oRight As Policy

            If TypeOf x Is Policy Then
                oLeft = x
            Else
                Throw New ArgumentNullException(x)
            End If

            If TypeOf y Is Policy Then
                oRight = y
            Else
                Throw New ArgumentNullException(y)
            End If

            If String.IsNullOrEmpty(oLeft.Reference) And String.IsNullOrEmpty(oRight.Reference) Then
                Return 0
            ElseIf String.IsNullOrEmpty(oLeft.Reference) Then
                Return -1
            ElseIf String.IsNullOrEmpty(oRight.Reference) Then
                Return 1
            ElseIf oLeft.Reference < oRight.Reference Then
                Return -1
            ElseIf oLeft.Reference = oRight.Reference Then
                Return 0
            Else
                Return 1
            End If

        End Function

    End Class

End Class

<Serializable()> Public Class PolicyCollection : Inherits SortableCollectionBase

    Public Sub New()
        MyBase.SortObjectType = GetType(Policy)
    End Sub
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oPolicy As Policy In List
            sbPrint.AppendLine(oPolicy.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oPolicy As Policy) As Integer
        Return List.Add(v_oPolicy)
    End Function

    Public Sub Remove(ByVal v_oPolicy As Policy)
        List.Remove(v_oPolicy)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As Policy
        Get
            Return List(i)
        End Get
        Set(ByVal value As Policy)
            List(i) = value
        End Set
    End Property
End Class

<Serializable()> Public Class PolicySummaryCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oPolicy As Policy In List
            sbPrint.AppendLine(oPolicy.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oPolicySummary As PolicySummary) As Integer
        Return List.Add(v_oPolicySummary)
    End Function

    Public Sub Remove(ByVal v_oPolicySummary As PolicySummary)
        List.Remove(v_oPolicySummary)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As PolicySummary
        Get
            Return List(i)
        End Get
        Set(ByVal value As PolicySummary)
            List(i) = value
        End Set
    End Property

End Class

<Serializable()> Public Class PolicySummary

    Private sReference As String
    Private dCommissionAmount As Decimal
    Private dPremiumDueGross As Decimal
    Private dPremiumDueNet As Decimal
    Private dPremiumDueTax As Decimal
    Private dTotalAnnualTax As Decimal

    Private bCommissionAmountSpecified As Boolean
    Private bPremiumDueGrossSpecified As Boolean
    Private bPremiumDueNetSpecified As Boolean
    Private bPremiumDueTaxSpecified As Boolean
    Private bTotalAnnualTaxSpecified As Boolean
    Private sInstalmentPlanRef As String
    Private iInstdepositTransDetailsId As Integer
    Public Property MediaTypeCode As String

    Public Sub New(ByVal v_sReference As String)
        sReference = v_sReference

        bCommissionAmountSpecified = False
        bPremiumDueGrossSpecified = False
        bPremiumDueNetSpecified = False
        bPremiumDueTaxSpecified = False
        bTotalAnnualTaxSpecified = False

    End Sub
    ''' <summary>
    ''' A default Constructer added as got a requirement for initializing the class without passing any parameter
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()

    End Sub


    Public Overridable Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Reference : " & sReference & "<br />")
        sbPrint.AppendLine("Commission Amount : " & dCommissionAmount.ToString() & "<br />")
        sbPrint.AppendLine("Premium Due Gross : " & dPremiumDueGross.ToString() & "<br />")
        sbPrint.AppendLine("Premium Due Net : " & dPremiumDueNet.ToString() & "<br />")
        sbPrint.AppendLine("Premium Due Tax : " & dPremiumDueTax.ToString() & "<br />")
        sbPrint.AppendLine("Total Annual Tax : " & dTotalAnnualTax.ToString() & "<br />")
        sbPrint.AppendLine("Commission Amount Specified : " & IIf(bCommissionAmountSpecified, "true", "false") & "<br />")
        sbPrint.AppendLine("Premium Due Gross Specified : " & IIf(bPremiumDueGrossSpecified, "true", "false") & "<br />")
        sbPrint.AppendLine("Premium Due Net Specified : " & IIf(bPremiumDueNetSpecified, "true", "false") & "<br />")
        sbPrint.AppendLine("Premium Due Tax Specified : " & IIf(bPremiumDueTaxSpecified, "true", "false") & "<br />")
        sbPrint.AppendLine("Total Annual Tax Specified : " & IIf(bTotalAnnualTaxSpecified, "true", "false") & "<br />")

        Return sbPrint.ToString()

    End Function

    Public Property Reference() As String
        Get
            Return sReference
        End Get
        Set(ByVal value As String)
            sReference = value
        End Set
    End Property

    Public Property CommissionAmount() As Decimal
        Get
            Return dCommissionAmount
        End Get
        Set(ByVal value As Decimal)
            dCommissionAmount = value
            bCommissionAmountSpecified = True
        End Set
    End Property

    Public Property PremiumDueGross() As Decimal
        Get
            Return dPremiumDueGross
        End Get
        Set(ByVal value As Decimal)
            dPremiumDueGross = value
            bPremiumDueGrossSpecified = True
        End Set
    End Property

    Public Property PremiumDueNet() As Decimal
        Get
            Return dPremiumDueNet
        End Get
        Set(ByVal value As Decimal)
            dPremiumDueNet = value
            bPremiumDueNetSpecified = True
        End Set
    End Property

    Public Property PremiumDueTax() As Decimal
        Get
            Return dPremiumDueTax
        End Get
        Set(ByVal value As Decimal)
            dPremiumDueTax = value
            bPremiumDueTaxSpecified = True
        End Set
    End Property

    Public Property TotalAnnualTax() As Decimal
        Get
            Return dTotalAnnualTax
        End Get
        Set(ByVal value As Decimal)
            dTotalAnnualTax = value
            bTotalAnnualTaxSpecified = True
        End Set
    End Property

    Public ReadOnly Property CommissionAmountSpecified() As Boolean
        Get
            Return bCommissionAmountSpecified
        End Get
    End Property

    Public ReadOnly Property PremiumDueGrossSpecified() As Boolean
        Get
            Return bPremiumDueGrossSpecified
        End Get
    End Property

    Public ReadOnly Property PremiumDueNetSpecified() As Boolean
        Get
            Return bPremiumDueNetSpecified
        End Get
    End Property

    Public ReadOnly Property PremiumDueTaxSpecified() As Boolean
        Get
            Return bPremiumDueTaxSpecified
        End Get
    End Property

    Public ReadOnly Property TotalAnnualTaxSpecified() As Boolean
        Get
            Return bTotalAnnualTaxSpecified
        End Get
    End Property

    ''' <summary>
    ''' If payment method for policy is instalment then it will contain instalment plan ref number
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property InstalmentPlanRef() As String
        Get
            Return sInstalmentPlanRef
        End Get
        Set(ByVal value As String)
            sInstalmentPlanRef = value
        End Set
    End Property
    ''' <summary>
    ''' If payment method for policy is instalment then it will contain instalment Deposit TransDetailsId
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property InstdepositTransDetailsId() As Integer
        Get
            Return iInstdepositTransDetailsId
        End Get
        Set(ByVal value As Integer)
            iInstdepositTransDetailsId = value
        End Set
    End Property

End Class
Public Enum PolicySort

    ''' <summary>
    ''' Party Key
    ''' </summary>
    Reference = 0
End Enum

''' <summary>
''' Type of installment for MTA
''' </summary>
''' <remarks></remarks>
Public Enum InstalmentType
    ''' <summary>
    ''' Add And Spread among all installments
    ''' </summary>
    ''' <remarks></remarks>
    AddAndSpread

    ''' <summary>
    ''' Add To Next Installment
    ''' </summary>
    ''' <remarks></remarks>
    AddToNext

    ''' <summary>
    ''' Add To New Plan
    ''' </summary>
    ''' <remarks></remarks>
    AddToNewPlan
    ''' <summary>
    ''' No Amount Change
    ''' </summary>
    ''' <remarks></remarks>
    NoAmountChange
End Enum



