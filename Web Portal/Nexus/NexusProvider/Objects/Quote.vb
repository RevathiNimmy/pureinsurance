Imports System.Text

<Serializable()> Public Class Quote

    Private iInsuranceFileKey As Integer
    Private sInsuranceFileRef As String
    Private iInsuranceFolderKey As Integer
    Private dtExpiryDate As DateTime
    Private bTimeStamp() As Byte

    Private dtCoverEndDate As DateTime
    Private dtCoverStartDate As DateTime
    Private dtPreviousLiveVersionCoverStartDate As DateTime
    Private sDescription As String
    Private sInsuredName As String
    'Private xmlInsuredParties As System.Xml.XmlElement
    Private iPartyKey As Integer
    Private sReference As String

    Private dtInceptionDate As DateTime
    Private sInsuranceFileStatusCode As String
    Private sInsuranceFileTypeCode As String
    Private iInsuranceFileVersion As Integer
    Private sPaymentMethodCode As String
    Private bQuoteIsLocked As Boolean
    Private sSubBranchCode As String
    Private sAnalysisCode As String
    Private bConsolidatedLeadAgentCommission As Boolean
    Private bConsolidatedSubAgentCommission As Boolean
    Private sCurrencyCode As String
    Private sProductCode As String


    'from here Added 
    Private sCoverNoteBookNumber As String
    Private iCoverNoteSheetNumber As Integer
    Private sBusinessTypeCode As String
    Private dtQuoteExpiryDate As DateTime
    Private sHandlerCode As String
    Private sRegarding As String
    Private sPolicyStatusCode As String
    Private dtRenewalDate As DateTime
    Private dtInceptionTPI As DateTime
    Private dtIssuedDate As DateTime
    Private dtProposalDate As DateTime
    Private sFrequencyCode As String
    Private sRenewalMethodCode As String
    Private sLapseCancelReasonCode As String
    Private dtLTUExpiryDate As DateTime
    Private sStopReasonCode As String
    Private dtLapseCancelDate As DateTime
    Private bReferredAtRenewal As Boolean
    Private bReferredAtMTA As Boolean
    Private sPaymentMethod As String

    'this is added for storing PartyName returned from GetQuoteMarkedforCollection
    Private sPartyName As String
    Private bMarkedQuoteforCollection As Boolean
    Private dMarkedDateforCollection As Date
    'these are for UpdateRiskSelection

    Private iIsSelected As Integer
    Private oRisks As RiskCollection

    Private sTransactionType As Enum_TransactionType
    Private iAccountHandlerCnt As Integer
    Private sAlternativeRef As String

    Private sClientCode As String
    Private sAgent As String
    Private sAgentDesc As String
    Private dNetTotal As Double
    Private dTaxTotal As Double
    Private dFeeTotal As Double

    Private dGrossTotal As Double
    Private dTotalSumInsured As Double
    Private dTaxTotalRate As Double

    Dim oRiskTaxes As TaxCollection
    Dim oPolicyFees As FeeCollection
    Dim oPolicyTaxes As TaxCollection
    Dim oRiskFees As FeeCollection

    'for Update Risk
    Private dPolicyLevelTax As Decimal
    Private dPolicyLevelFees As Decimal

    Private iProRata As Integer
    Private dProRataRate As Decimal
    Private sProRataMessage As String
    Private dAmount As String
    Private iRenewalCount As Integer
    Private dtHCExpiryDate As Date
    Private sPolicyDeductible, sPolicyLimits, sUnderwritingYear As String

    Private dTotalRiskFees, dTotalPolicyFeesEligibleForFinancing, dTotalPolicyFeesExcludedFromFinancing, dTotalRiskFeesEligibleForFinancing, dTotalRiskFeesExcludedFromFinancing As Double
    'Newly Added Properties 
    Private bAccountHandlerCntSpecified, bAgentkeySpecified, bConsolidatedLeadAgentCommissionSpecified, bConsolidatedSubAgentCommissionSpecified, bCoverNoteSheetNumberSpecified, bIssuedDateSpecified, bProposalDateSpecified, bLapseCancelDateSpecified, bLTUExpiryDateSpecified, bReferredAtRenewalSpecified, bReferredAtMTASpecified As Boolean
    Private dTotalRiskTaxEligibleForFinancing, dTotalRiskTaxExcludedFromFinancing, dTotalRiskTax, dTotalPolicyTaxEligibleForFinancing, dTotalPolicyTaxExcludedFromFinancing, dTotalPolicyTax As Double
    Private dTotalCommissionLeadAgent, dTotalTaxLeadAgent As Double

    Private dAnnualPremium, dThisPremium As Double
    Private sPolicyStyleCode, sBranchCode, sAgentCode, sPolicyTypeCode, sProductName, sRenewalFrequencyCode, sStandardPolicyDiscription, sStandardPolicyWordingCode As String
    Private dtLapseDate As Date
    Private oSWColl As NexusProvider.StandardWordings
    Private sAccountHandlerCode, sAccountHandlerName As String
    Private iRenewalDayNo, iUnderwritingYearId As Integer
    Private dTotalFeesOnDeposit, dTotalTaxOnDeposit As Double

    'Properties Added as per WPR 63
    Private iBaseInsuranceFolderKey As Integer
    Private iQuoteVersion As Integer
    Private iQuoteStatus As Integer
    Private iQuoteStatusKey As Integer
    Private dtQuoteORLiveDate As Date
    Private iAgentKey As Integer
    Private sCoinsurancePlacement As String

    Private dInstDepositAmount As Double
    Private bDepositTransactasInstalment As Boolean
    Private bAnniversaryCopy As Boolean
    Private bIsValidAnniversaryToAccept As Boolean
    Private sDefaultPaymentMethod As String
    Private nDefaultInstalmentPlan As Integer
    Private nDefaultInstalmentPlanVersion As Integer
    Private nDefaultSchemeNumber As Integer
    Private nDefaultSchemeVersion As Integer
    Private nDefaultPFRF_Id As Integer

    Private nOriginalInsuranceFileKey As Integer
    Private nOriginalPremiumFinanceCnt As Integer
    Private nOriginalPremFinanceVersion As Integer
    Private nActivePlans As Integer

    Private bIsMigratedPolicy As Boolean
    Private bIsPolicyInAnnualRenewal As Boolean
    Private bIsPolicyInRenewal As Boolean
    Private nCollectionFrequency, nPaymentTerm As Integer
    Private sBranchName As String
    Private BaseCurrencyIDField As Integer
    Private TransCurrencyIDField As Integer
    'WPR 3.1
    Private CorrespondenceTypeField As String
    Private DefaultPreferredCorrespondenceField As String
    Private IsAgentReceiveCorrespondenceField As Boolean

    Private sMailContacts As String

    Private nDefaultSchemeNumberField As Integer

    Private nDefaultSchemeVersionField As Integer

    Private nActivePlan As Integer


    Private bIsInBackDatedMode As Boolean
    Private bDeleteRenQuoteReRunRenewal As Boolean
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

   

    Public Property BaseCurrencyID() As Integer
        Get
            Return BaseCurrencyIDField
        End Get
        Set(ByVal value As Integer)
            BaseCurrencyIDField = value
        End Set
    End Property

    Public Property TransCurrencyID() As Integer
        Get
            Return TransCurrencyIDField
        End Get
        Set(ByVal value As Integer)
            TransCurrencyIDField = value
        End Set
    End Property

    Public Property CoinsurancePlacement() As String
        Get
            Return sCoinsurancePlacement
        End Get
        Set(ByVal value As String)
            sCoinsurancePlacement = value
        End Set
    End Property

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

    Public Property QuoteStatus() As QuoteStatusType
        Get
            Return iQuoteStatus
        End Get
        Set(ByVal value As QuoteStatusType)
            iQuoteStatus = value
        End Set
    End Property

    Public Property QuoteStatusKey() As QuoteStatusType
        Get
            Return iQuoteStatusKey
        End Get
        Set(ByVal value As QuoteStatusType)
            iQuoteStatusKey = value
        End Set
    End Property

    Public Property QuoteORLiveDate() As Date
        Get
            Return dtQuoteORLiveDate
        End Get
        Set(ByVal value As Date)
            dtQuoteORLiveDate = value
        End Set
    End Property

    Public Property AgentKey() As Integer
        Get
            Return iAgentKey
        End Get
        Set(ByVal value As Integer)
            iAgentKey = value
        End Set
    End Property
    Public Property AgentDesc() As String
        Get
            Return sAgentDesc
        End Get
        Set(ByVal value As String)
            sAgentDesc = value
        End Set
    End Property

    '[END] added for WPR63

    '[Start]Properties Added as per WPR 73_74
    Private iContactUserKey As Integer
    Private sContactUserName As String
    Private iIsDeletedVersionUser As Integer
    Private sEmailAddress As String


    Public Property ContactUserKey() As Integer
        Get
            Return iContactUserKey
        End Get
        Set(ByVal value As Integer)
            iContactUserKey = value
        End Set
    End Property

    Public Property ContactUserName() As String
        Get
            Return sContactUserName
        End Get
        Set(ByVal value As String)
            sContactUserName = value
        End Set
    End Property

    Public Property IsDeletedVersionUser() As Integer
        Get
            Return iIsDeletedVersionUser
        End Get
        Set(ByVal value As Integer)
            iIsDeletedVersionUser = value
        End Set
    End Property


    Public Property EmailAddress() As String
        Get
            Return sEmailAddress
        End Get
        Set(ByVal value As String)
            sEmailAddress = value
        End Set
    End Property
    '[END]Properties Added as per WPR 73_74

   
    ''' <summary>
    ''' Anniversary date for True monthly policy
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AnniversaryDate As Date

    ''' <summary>
    ''' To identify if any previous version of policy is not live
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsValidAnniversaryToAccept() As Boolean
        Get
            Return bIsValidAnniversaryToAccept
        End Get
        Set(ByVal value As Boolean)
            bIsValidAnniversaryToAccept = value
        End Set
    End Property
    ''' <summary>
    ''' To identify if annual version for the policy is in renewal
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsPolicyInAnnualRenewal() As Boolean
        Get
            Return bIsPolicyInAnnualRenewal
        End Get
        Set(ByVal value As Boolean)
            bIsPolicyInAnnualRenewal = value
        End Set
    End Property
    Public Property MailContacts As String
        Get
            Return sMailContacts
        End Get
        Set(ByVal value As String)
            sMailContacts = value
        End Set
    End Property

    ''' <summary>
    ''' To identify if any version of policy is in renewal
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsPolicyInRenewal() As Boolean
        Get
            Return bIsPolicyInRenewal
        End Get
        Set(ByVal value As Boolean)
            bIsPolicyInRenewal = value
        End Set
    End Property
    ''' <summary>
    ''' PaymentTermId
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property PaymentTerm() As Integer
        Get
            Return nPaymentTerm
        End Get
        Set(ByVal value As Integer)
            nPaymentTerm = value
        End Set
    End Property

    ''' <summary>
    ''' Collection frequency id
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CollectionFrequency() As Integer
        Get
            Return nCollectionFrequency
        End Get
        Set(ByVal value As Integer)
            nCollectionFrequency = value
        End Set
    End Property
    'WPR 3.1

    Public Property CorrespondenceType As String
        Get
            Return Me.CorrespondenceTypeField
        End Get
        Set(value As String)
            Me.CorrespondenceTypeField = value
        End Set
    End Property


    Public Property DefaultPreferredCorrespondence As String
        Get
            Return Me.DefaultPreferredCorrespondenceField
        End Get
        Set(value As String)
            Me.DefaultPreferredCorrespondenceField = value
        End Set
    End Property


    Public Property IsAgentReceiveCorrespondence As Boolean
        Get
            Return Me.IsAgentReceiveCorrespondenceField
        End Get
        Set(value As Boolean)
            Me.IsAgentReceiveCorrespondenceField = value
        End Set
    End Property

    Public Sub New(ByVal v_dtCoverStartDate As DateTime, ByVal v_dtCoverEndDate As DateTime, ByVal v_sDescription As String)

        oRisks = New RiskCollection

        dtCoverStartDate = v_dtCoverStartDate
        dtCoverEndDate = v_dtCoverEndDate
        sDescription = v_sDescription
        oRiskTaxes = New TaxCollection
        oPolicyFees = New FeeCollection
        oPolicyTaxes = New TaxCollection
        oRiskFees = New FeeCollection
        oSWColl = New StandardWordings
    End Sub
    Public Sub New()
        oRisks = New RiskCollection
        oRiskTaxes = New TaxCollection
        oPolicyFees = New FeeCollection
        oPolicyTaxes = New TaxCollection
        oRiskFees = New FeeCollection
    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Insurance File Key : " & iInsuranceFileKey.ToString() & "<br />")
        sbPrint.AppendLine("Insurance File Ref : " & sInsuranceFileRef & "<br />")
        sbPrint.AppendLine("Insurance Folder Key : " & iInsuranceFolderKey & "<br />")
        sbPrint.AppendLine("Expiry Date : " & dtExpiryDate.ToString() & "<br />")
        sbPrint.Append("TimeStamp : ")

        If bTimeStamp IsNot Nothing Then

            For Each oByte As Byte In bTimeStamp
                sbPrint.Append(oByte.ToString & " | ")
            Next

        End If

        sbPrint.AppendLine("<br />")
        sbPrint.AppendLine("Cover End Date : " & dtCoverEndDate & "<br />")
        sbPrint.AppendLine("Cover Start Date : " & dtCoverStartDate & "<br />")
        sbPrint.AppendLine("Description : " & sDescription & "<br />")
        sbPrint.AppendLine("Insured Name : " & sInsuredName & "<br />")
        sbPrint.AppendLine("Insured Parties : ")
        'If xmlInsuredParties Is Nothing Then
        '    sbPrint.Append("<i>Nothing</i><br />")
        'Else
        '    sbPrint.Append(xmlInsuredParties.Value() & "<br />")
        'End If
        sbPrint.AppendLine("Party Key : " & iPartyKey.ToString() & "<br />")
        sbPrint.AppendLine("Reference : " & sReference & "<br />")
        sbPrint.AppendLine("Inception Date : " & dtInceptionDate & "<br />")
        sbPrint.AppendLine("Insurance File Status Code : " & sInsuranceFileStatusCode & "<br />")
        sbPrint.AppendLine("Insurance File Type Code : " & sInsuranceFileTypeCode & "<br />")
        sbPrint.AppendLine("Insurance File Version : " & iInsuranceFileVersion & "<br />")
        sbPrint.AppendLine("Payment Method Code : " & sPaymentMethodCode & "<br />")
        sbPrint.AppendLine("Quote Is Locked : " & IIf(bQuoteIsLocked, "true", "false") & "<br />")
        sbPrint.AppendLine("Sub Branch Code : " & sSubBranchCode & "<br />")
        sbPrint.AppendLine("Analysis Code : " & sAnalysisCode & "<br />")
        sbPrint.AppendLine("Consolidated Lead Agent Commission : " & IIf(bConsolidatedLeadAgentCommission, "true", "false") & "<br />")
        sbPrint.AppendLine("Consolidated Sub Agent Commission : " & IIf(bConsolidatedSubAgentCommission, "true", "false") & "<br />")
        sbPrint.AppendLine("Currency Code : " & sCurrencyCode & "<br />")
        sbPrint.AppendLine("Product Code : " & sProductCode & "<br />")
        sbPrint.AppendLine("Risks ---------------><br />")

        If oRisks IsNot Nothing Then
            sbPrint.AppendLine(oRisks.Print())
            sbPrint.AppendLine("<br />")
        End If

        sbPrint.AppendLine("Account Handler Code : " & sAccountHandlerCode & "<br />")
        sbPrint.AppendLine("Renewal Day No : " & iRenewalDayNo.ToString() & "<br />")
        Return sbPrint.ToString()

    End Function

    Public Property AccountHandlerName() As String
        Get
            Return sAccountHandlerName
        End Get
        Set(ByVal value As String)
            sAccountHandlerName = value
        End Set
    End Property
    Public Property UnderwritingYear() As String
        Get
            Return sUnderwritingYear
        End Get
        Set(ByVal value As String)
            sUnderwritingYear = value
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
    Public Property PolicyLimits() As String
        Get
            Return sPolicyLimits
        End Get
        Set(ByVal value As String)
            sPolicyLimits = value
        End Set
    End Property
    Public Property PolicyDeductible() As String
        Get
            Return sPolicyDeductible
        End Get
        Set(ByVal value As String)
            sPolicyDeductible = value
        End Set
    End Property
    Public Property HCExpiryDate() As Date
        Get
            Return dtHCExpiryDate
        End Get
        Set(ByVal value As Date)
            dtHCExpiryDate = value
        End Set
    End Property

    Public Property LapseDate() As Date
        Get
            Return dtLapseDate
        End Get
        Set(ByVal value As Date)
            dtLapseDate = value
        End Set
    End Property
    Public Property StandardPolicyWordings() As StandardWordings
        Get
            Return oSWColl
        End Get
        Set(ByVal value As StandardWordings)
            oSWColl = value
        End Set
    End Property

    Public Property RenewalFrequencyCode() As String
        Get
            Return sRenewalFrequencyCode
        End Get
        Set(ByVal value As String)
            sRenewalFrequencyCode = value
        End Set
    End Property
    Public Property ProductName() As String
        Get
            Return sProductName
        End Get
        Set(ByVal value As String)
            sProductName = value
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
    Public Property PolicyStyleCode() As String
        Get
            Return sPolicyStyleCode
        End Get
        Set(ByVal value As String)
            sPolicyStyleCode = value
        End Set
    End Property

    Public Property AnnualPremium() As Decimal
        Get
            Return dAnnualPremium
        End Get
        Set(ByVal value As Decimal)
            dAnnualPremium = value
        End Set
    End Property
    Public Property ThisPremium() As Decimal
        Get
            Return dThisPremium
        End Get
        Set(ByVal value As Decimal)
            dThisPremium = value
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
    Public Property AgentCode() As String
        Get
            Return sAgentCode
        End Get
        Set(ByVal value As String)
            sAgentCode = value
        End Set
    End Property

    Public Property Amount() As Decimal
        Get
            Return dAmount
        End Get
        Set(ByVal value As Decimal)
            dAmount = value
        End Set
    End Property
    Public Property ProRataRate() As Decimal
        Get
            Return dProRataRate
        End Get
        Set(ByVal value As Decimal)
            dProRataRate = value
        End Set
    End Property
    Public Property ProRata() As Integer
        Get
            Return iProRata
        End Get
        Set(ByVal value As Integer)
            iProRata = value
        End Set
    End Property
    Public Property PolicyLevelFees() As Decimal
        Get
            Return dPolicyLevelFees
        End Get
        Set(ByVal value As Decimal)
            dPolicyLevelFees = value
        End Set
    End Property
    Public Property PolicyLevelTax() As Decimal
        Get
            Return dPolicyLevelTax
        End Get
        Set(ByVal value As Decimal)
            dPolicyLevelTax = value
        End Set
    End Property
    Public Property MarkedQuoteForCollection() As Boolean
        Get
            Return bMarkedQuoteforCollection
        End Get
        Set(ByVal value As Boolean)
            bMarkedQuoteforCollection = value
        End Set
    End Property

    Public Property MarkedDateforCollection() As Date
        Get
            Return dMarkedDateforCollection
        End Get
        Set(ByVal value As Date)
            dMarkedDateforCollection = value
        End Set
    End Property

    Public Property ProRataMessage() As String
        Get
            Return sProRataMessage
        End Get
        Set(ByVal value As String)
            sProRataMessage = value
        End Set
    End Property

    Public Property TotalRiskFees() As Double
        Get
            Return dTotalRiskFees
        End Get
        Set(ByVal value As Double)
            dTotalRiskFees = value
        End Set
    End Property
    Public Property TotalRiskFeesExcludedFromFinancing() As Double
        Get
            Return dTotalRiskFeesExcludedFromFinancing
        End Get
        Set(ByVal value As Double)
            dTotalRiskFeesExcludedFromFinancing = value
        End Set
    End Property
    Public Property TotalRiskFeesEligibleForFinancing() As Double
        Get
            Return dTotalRiskFeesEligibleForFinancing
        End Get
        Set(ByVal value As Double)
            dTotalRiskFeesEligibleForFinancing = value
        End Set
    End Property
    Public Property TotalPolicyFeesEligibleForFinancing() As Double
        Get
            Return dTotalPolicyFeesEligibleForFinancing
        End Get
        Set(ByVal value As Double)
            dTotalPolicyFeesEligibleForFinancing = value
        End Set
    End Property
    Public Property TotalPolicyFeesExcludedFromFinancing() As Double
        Get
            Return dTotalPolicyFeesExcludedFromFinancing
        End Get
        Set(ByVal value As Double)
            dTotalPolicyFeesExcludedFromFinancing = value
        End Set
    End Property
    Public Property TotalRiskTaxEligibleForFinancing() As Double
        Get
            Return dTotalRiskTaxEligibleForFinancing
        End Get
        Set(ByVal value As Double)
            dTotalRiskTaxEligibleForFinancing = value
        End Set
    End Property
    Public Property TotalRiskTaxExcludedFromFinancing() As Double
        Get
            Return dTotalRiskTaxExcludedFromFinancing
        End Get
        Set(ByVal value As Double)
            dTotalRiskTaxExcludedFromFinancing = value
        End Set
    End Property
    Public Property TotalRiskTax() As Double
        Get
            Return dTotalRiskTax
        End Get
        Set(ByVal value As Double)
            dTotalRiskTax = value
        End Set
    End Property
    Public Property TotalPolicyTaxEligibleForFinancing() As Double
        Get
            Return dTotalPolicyTaxEligibleForFinancing
        End Get
        Set(ByVal value As Double)
            dTotalPolicyTaxEligibleForFinancing = value
        End Set
    End Property
    Public Property TotalPolicyTaxExcludedFromFinancing() As Double
        Get
            Return dTotalPolicyTaxExcludedFromFinancing
        End Get
        Set(ByVal value As Double)
            dTotalPolicyTaxExcludedFromFinancing = value
        End Set
    End Property
    Public Property TotalPolicyTax() As Double
        Get
            Return dTotalPolicyTax
        End Get
        Set(ByVal value As Double)
            dTotalPolicyTax = value
        End Set
    End Property
    Public Property TotalCommissionLeadAgent() As Double
        Get
            Return dTotalCommissionLeadAgent
        End Get
        Set(ByVal value As Double)
            dTotalCommissionLeadAgent = value
        End Set
    End Property
    Public Property TotalTaxLeadAgent() As Double
        Get
            Return dTotalTaxLeadAgent
        End Get
        Set(ByVal value As Double)
            dTotalTaxLeadAgent = value
        End Set
    End Property

    Public Property PolicyFees() As FeeCollection
        Get
            Return oPolicyFees
        End Get
        Set(ByVal value As FeeCollection)
            oPolicyFees = value
        End Set
    End Property
    Public Property RiskTaxes() As TaxCollection
        Get
            Return oRiskTaxes
        End Get
        Set(ByVal value As TaxCollection)
            oRiskTaxes = value
        End Set
    End Property
    Public Property PolicyTaxes() As TaxCollection
        Get
            Return oPolicyTaxes
        End Get
        Set(ByVal value As TaxCollection)
            oPolicyTaxes = value
        End Set
    End Property
    Public Property RiskFees() As FeeCollection
        Get
            Return oRiskFees
        End Get
        Set(ByVal value As FeeCollection)
            oRiskFees = value
        End Set
    End Property
    Public Property GrossTotal() As Double
        Get
            Return dGrossTotal
        End Get
        Set(ByVal value As Double)
            dGrossTotal = value
        End Set
    End Property
    Public Property FeeTotal() As Double
        Get
            Return dFeeTotal
        End Get
        Set(ByVal value As Double)
            dFeeTotal = value
        End Set
    End Property
    Public Property TaxTotal() As Double
        Get
            Return dTaxTotal
        End Get
        Set(ByVal value As Double)
            dTaxTotal = value
        End Set
    End Property
    Public Property NetTotal() As Double
        Get
            Return dNetTotal
        End Get
        Set(ByVal value As Double)
            dNetTotal = value
        End Set
    End Property

    Public Property TotalSumInsured() As Double
        Get
            Return dTotalSumInsured
        End Get
        Set(ByVal value As Double)
            dTotalSumInsured = value
        End Set
    End Property

    Public Property TaxTotalRate() As Double
        Get
            Return dTaxTotalRate
        End Get
        Set(ByVal value As Double)
            dTaxTotalRate = value
        End Set
    End Property

    Public Property Agent() As String
        Get
            Return sAgent
        End Get
        Set(ByVal value As String)
            sAgent = value
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

    Public Property AlternativeRef() As String
        Get
            Return sAlternativeRef
        End Get
        Set(ByVal value As String)
            sAlternativeRef = value
        End Set
    End Property
    Public Property InsuranceFileKey() As Integer
        Get
            Return iInsuranceFileKey
        End Get
        Set(ByVal value As Integer)
            iInsuranceFileKey = value
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

    Public Property IsInBackDatedMode() As Boolean
        Get
            Return bIsInBackDatedMode
        End Get
        Set(ByVal value As Boolean)
            bIsInBackDatedMode = value
        End Set
    End Property

    Public Property DeleteRenQuoteReRunRenewal() As Boolean
        Get
            Return bDeleteRenQuoteReRunRenewal
        End Get
        Set(ByVal value As Boolean)
            bDeleteRenQuoteReRunRenewal = value
        End Set
    End Property

    Public Property InsuranceFolderKey() As Integer
        Get
            Return iInsuranceFolderKey
        End Get
        Set(ByVal value As Integer)
            iInsuranceFolderKey = value
        End Set
    End Property

    Public Property ExpiryDate() As DateTime
        Get
            Return dtExpiryDate
        End Get
        Set(ByVal value As DateTime)
            dtExpiryDate = value
        End Set
    End Property

    Public Property TimeStamp() As Byte()
        Get
            Return bTimeStamp
        End Get
        Set(ByVal value As Byte())
            bTimeStamp = value
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
    '''<remarks/>
    Public Property RenewalCount() As Integer
        Get
            Return Me.iRenewalCount
        End Get
        Set(ByVal value As Integer)
            Me.iRenewalCount = value
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

    Public Property Description() As String
        Get
            Return sDescription
        End Get
        Set(ByVal value As String)
            sDescription = value
        End Set
    End Property

    Public Property InsuredName() As String
        Get
            Return sInsuredName
        End Get
        Set(ByVal value As String)
            sInsuredName = value
        End Set
    End Property

    Public Property PartyKey() As Integer
        Get
            Return iPartyKey
        End Get
        Set(ByVal value As Integer)
            iPartyKey = value
        End Set
    End Property

    Public Property Reference() As String
        Get
            Return sReference
        End Get
        Set(ByVal value As String)
            sReference = value
        End Set
    End Property

    Public Property InceptionDate() As DateTime
        Get
            Return dtInceptionDate
        End Get
        Set(ByVal value As DateTime)
            dtInceptionDate = value
        End Set
    End Property

    Public Property InsuranceFileStatusCode() As String
        Get
            Return sInsuranceFileStatusCode
        End Get
        Set(ByVal value As String)
            sInsuranceFileStatusCode = value
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

    Public Property InsuranceFileVersion() As Integer
        Get
            Return iInsuranceFileVersion
        End Get
        Set(ByVal value As Integer)
            iInsuranceFileVersion = value
        End Set
    End Property

    Public Property PaymentMethodCode() As String
        Get
            Return sPaymentMethodCode
        End Get
        Set(ByVal value As String)
            sPaymentMethodCode = value
        End Set
    End Property

    Public Property QuoteIsLocked() As Boolean
        Get
            Return bQuoteIsLocked
        End Get
        Set(ByVal value As Boolean)
            bQuoteIsLocked = value
        End Set
    End Property

    Public Property SubBranchCode() As String
        Get
            Return sSubBranchCode
        End Get
        Set(ByVal value As String)
            sSubBranchCode = value
        End Set
    End Property

    Public Property Risks() As RiskCollection
        Get
            Return oRisks
        End Get
        Set(ByVal value As RiskCollection)
            oRisks = value
        End Set
    End Property

    Public Property AnalysisCode() As String
        Get
            Return sAnalysisCode
        End Get
        Set(ByVal value As String)
            sAnalysisCode = value
        End Set
    End Property

    Public Property ConsolidatedLeadAgentCommission() As Boolean
        Get
            Return bConsolidatedLeadAgentCommission
        End Get
        Set(ByVal value As Boolean)
            bConsolidatedLeadAgentCommission = value
        End Set
    End Property

    Public Property ConsolidatedSubAgentCommission() As Boolean
        Get
            Return bConsolidatedSubAgentCommission
        End Get
        Set(ByVal value As Boolean)
            bConsolidatedSubAgentCommission = value
        End Set
    End Property

    Public Property CurrencyCode() As String
        Get
            Return sCurrencyCode
        End Get
        Set(ByVal value As String)
            sCurrencyCode = value
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

    Public Property TransactionType() As Enum_TransactionType
        Get
            Return sTransactionType
        End Get
        Set(ByVal value As Enum_TransactionType)
            sTransactionType = value
        End Set
    End Property

    Public Property CoverNoteBookNumber() As String
        Get
            Return sCoverNoteBookNumber
        End Get
        Set(ByVal value As String)
            sCoverNoteBookNumber = value
        End Set
    End Property

    Public Property CoverNoteSheetNumber() As Integer
        Get
            Return iCoverNoteSheetNumber
        End Get
        Set(ByVal value As Integer)
            iCoverNoteSheetNumber = value
        End Set
    End Property

    Public Property BusinessTypeCode() As String
        Get
            Return sBusinessTypeCode
        End Get
        Set(ByVal value As String)
            sBusinessTypeCode = value
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

    Public Property HandlerCode() As String
        Get
            Return sHandlerCode
        End Get
        Set(ByVal value As String)
            sHandlerCode = value
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

    Public Property PolicyStatusCode() As String
        Get
            Return sPolicyStatusCode
        End Get
        Set(ByVal value As String)
            sPolicyStatusCode = value
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
    Public Property InceptionTPI() As DateTime
        Get
            Return dtInceptionTPI
        End Get
        Set(ByVal value As DateTime)
            dtInceptionTPI = value
        End Set
    End Property
    Public Property IssuedDate() As DateTime
        Get
            Return dtIssuedDate
        End Get
        Set(ByVal value As DateTime)
            dtIssuedDate = value
        End Set
    End Property
    Public Property ProposalDate() As DateTime
        Get
            Return dtProposalDate
        End Get
        Set(ByVal value As DateTime)
            dtProposalDate = value
        End Set
    End Property

    Public Property FrequencyCode() As String
        Get
            Return sFrequencyCode
        End Get
        Set(ByVal value As String)
            sFrequencyCode = value
        End Set
    End Property

    Public Property RenewalMethodCode() As String
        Get
            Return sRenewalMethodCode
        End Get
        Set(ByVal value As String)
            sRenewalMethodCode = value
        End Set
    End Property

    Public Property LapseCancelReasonCode() As String
        Get
            Return sLapseCancelReasonCode
        End Get
        Set(ByVal value As String)
            sLapseCancelReasonCode = value
        End Set
    End Property

    Public Property LTUExpiryDate() As DateTime
        Get
            Return dtLTUExpiryDate
        End Get
        Set(ByVal value As DateTime)
            dtLTUExpiryDate = value
        End Set
    End Property
    Public Property StopReasonCode() As String
        Get
            Return sStopReasonCode
        End Get
        Set(ByVal value As String)
            sStopReasonCode = value
        End Set
    End Property

    Public Property LapseCancelDate() As DateTime
        Get
            Return dtLapseCancelDate
        End Get
        Set(ByVal value As DateTime)
            dtLapseCancelDate = value
        End Set
    End Property

    Public Property ReferredAtRenewal() As Boolean
        Get
            Return bReferredAtRenewal
        End Get
        Set(ByVal value As Boolean)
            bReferredAtRenewal = value
        End Set
    End Property
    Public Property ReferredAtMTA() As Boolean
        Get
            Return bReferredAtMTA
        End Get
        Set(ByVal value As Boolean)
            bReferredAtMTA = value
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

    Public Property IsSelected() As Integer
        Get
            Return iIsSelected
        End Get
        Set(ByVal value As Integer)
            iIsSelected = value
        End Set
    End Property
    '(Abhishek)
    Public Property AccountHandlerCnt() As Integer
        Get
            Return iAccountHandlerCnt
        End Get
        Set(ByVal value As Integer)
            iAccountHandlerCnt = value
        End Set
    End Property
    'Public Properties for the newlyadded
    Public ReadOnly Property AccountHandlerCntSpecified() As Boolean
        Get
            Return bAccountHandlerCntSpecified
        End Get
    End Property
    Public ReadOnly Property AgentkeySpecified() As Boolean
        Get
            Return bAgentkeySpecified
        End Get
    End Property

    Public ReadOnly Property ConsolidatedLeadAgentCommissionSpecified() As Boolean
        Get
            Return bConsolidatedLeadAgentCommissionSpecified
        End Get
    End Property
    Public ReadOnly Property ConsolidatedSubAgentCommissionSpecified() As Boolean
        Get
            Return bConsolidatedSubAgentCommissionSpecified
        End Get
    End Property
    Public Property CoverNoteSheetNumberSpecified() As Boolean
        Get
            Return bCoverNoteSheetNumberSpecified
        End Get
        Set(ByVal value As Boolean)
            bCoverNoteSheetNumberSpecified = value
        End Set
    End Property
    Public ReadOnly Property IssuedDateSpecified() As Boolean
        Get
            Return bIssuedDateSpecified
        End Get
    End Property
    Public ReadOnly Property ProposalDateSpecified() As Boolean
        Get
            Return bProposalDateSpecified
        End Get
    End Property
    Public ReadOnly Property LapseCancelDateSpecified() As Boolean
        Get
            Return bLapseCancelDateSpecified
        End Get
    End Property
    Public ReadOnly Property LTUExpiryDateSpecified() As Boolean
        Get
            Return bLTUExpiryDateSpecified
        End Get
    End Property
    Public ReadOnly Property ReferredAtRenewalSpecified() As Boolean
        Get
            Return bReferredAtRenewalSpecified
        End Get
    End Property
    Public ReadOnly Property ReferredAtMTASpecified() As Boolean
        Get
            Return bReferredAtMTASpecified
        End Get
    End Property

    Public Property AccountHandlerCode() As String
        Get
            Return sAccountHandlerCode
        End Get
        Set(ByVal value As String)
            sAccountHandlerCode = value
        End Set
    End Property
    Public Property RenewalDayNo() As Integer
        Get
            Return iRenewalDayNo
        End Get
        Set(ByVal value As Integer)
            iRenewalDayNo = value
        End Set
    End Property
    Public Property TotalFeesOnDeposit() As Double
        Get
            Return dTotalFeesOnDeposit
        End Get
        Set(ByVal value As Double)
            dTotalFeesOnDeposit = value
        End Set
    End Property
    Public Property TotalTaxOnDeposit() As Double
        Get
            Return dTotalTaxOnDeposit
        End Get
        Set(ByVal value As Double)
            dTotalTaxOnDeposit = value
        End Set
    End Property
    Public Property UnderwritingYearId() As Integer
        Get
            Return iUnderwritingYearId
        End Get
        Set(ByVal value As Integer)
            iUnderwritingYearId = value
        End Set
    End Property
    Public Property InstDepositAmount() As Double
        Get
            Return dInstDepositAmount
        End Get
        Set(ByVal value As Double)
            dInstDepositAmount = value
        End Set
    End Property
    Public Property DepositTransactasInstalment() As Boolean
        Get
            Return bDepositTransactasInstalment
        End Get
        Set(ByVal value As Boolean)
            bDepositTransactasInstalment = value
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
    Public Property PreviousLiveVersionCoverStartDate() As DateTime
        Get
            Return dtPreviousLiveVersionCoverStartDate
        End Get
        Set(ByVal value As DateTime)
            dtPreviousLiveVersionCoverStartDate = value
        End Set
    End Property
    Public Property DefaultPaymentMethod() As String
        Get
            Return sDefaultPaymentMethod
        End Get
        Set(ByVal value As String)
            sDefaultPaymentMethod = value
        End Set
    End Property

    Public Property DefaultInstalmentPlan() As Integer
        Get
            Return nDefaultInstalmentPlan
        End Get
        Set(ByVal value As Integer)
            nDefaultInstalmentPlan = value
        End Set
    End Property

    Public Property DefaultInstalmentPlanVersion() As Integer
        Get
            Return nDefaultInstalmentPlanVersion
        End Get
        Set(ByVal value As Integer)
            nDefaultInstalmentPlanVersion = value
        End Set
    End Property

    Public Property DefaultSchemeNumber() As Integer
        Get
            Return nDefaultSchemeNumber
        End Get
        Set(ByVal value As Integer)
            nDefaultSchemeNumber = value
        End Set
    End Property

    Public Property DefaultSchemeVersion() As Integer
        Get
            Return nDefaultSchemeVersion
        End Get
        Set(ByVal value As Integer)
            nDefaultSchemeVersion = value
        End Set
    End Property
    Public Property DefaultPFRF_Id() As Integer
        Get
            Return nDefaultPFRF_Id
        End Get
        Set(ByVal value As Integer)
            nDefaultPFRF_Id = value
        End Set
    End Property

    ''' <summary>
    ''' Previous live version Insurance File key
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OriginalInsuranceFileKey() As Integer
        Get
            Return nOriginalInsuranceFileKey
        End Get
        Set(ByVal value As Integer)
            nOriginalInsuranceFileKey = value
        End Set
    End Property

    ''' <summary>
    ''' Previous live version Premium Finance key(if previous version made live with instalment)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OriginalPremiumFinanceCnt() As Integer
        Get
            Return nOriginalPremiumFinanceCnt
        End Get
        Set(ByVal value As Integer)
            nOriginalPremiumFinanceCnt = value
        End Set
    End Property

    ''' <summary>
    ''' Previous live version Premium Finance Version(if previous version made live with instalment)
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OriginalPremFinanceVersion() As Integer
        Get
            Return nOriginalPremFinanceVersion
        End Get
        Set(ByVal value As Integer)
            nOriginalPremFinanceVersion = value
        End Set
    End Property
    ''' <summary>
    ''' Holds Active plan count against Insurance_Folder_cnt
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ActivePlans() As Integer
        Get
            Return nActivePlans
        End Get
        Set(ByVal value As Integer)
            nActivePlans = value
        End Set
    End Property
    ''' <summary>
    ''' To identify a migrated policy-This will be true if policy is migrated from I90 and have status under renewal but without any previous version in database
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsMigratedPolicy() As Boolean
        Get
            Return bIsMigratedPolicy
        End Get
        Set(ByVal value As Boolean)
            bIsMigratedPolicy = value
        End Set
    End Property

    Public Property BranchName() As String
        Get
            Return sBranchName
        End Get
        Set(ByVal value As String)
            sBranchName = value
        End Set
    End Property
    Private oldPolicyNumberField As String
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property OldPolicyNumber() As String
        Get
            Return Me.oldPolicyNumberField
        End Get
        Set(ByVal value As String)
            Me.oldPolicyNumberField = value
        End Set
    End Property
    ''' <summary>
    ''' Holds Active plan count against Insurance_Folder_cnt
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property ActivePlan() As Integer
        Get
            Return nActivePlan
        End Get
        Set(ByVal value As Integer)
            nActivePlan = value
        End Set
    End Property

    Public Property MtaReasonId() As Integer
    Public Property MtaReasonCode() As String

    Public Property IsMarketPlacePolicy As Boolean = False

    Public Property SenderEmail As String

    Public Property ReceiverEmail As String
    Public Property SavedPreferredDate As DateTime = DateTime.MinValue
    Public Property SavedDayInMonth As Integer = 0
    Public Property Frequency As String
    ''' <summary>
    ''' Transaction Types
    ''' </summary>
    Enum Enum_TransactionType

        ''' <summary>
        ''' New Business
        ''' </summary>
        NB

        ''' <summary>
        ''' Mid Term Adjustment
        ''' </summary>
        MTA

        ''' <summary>
        ''' Mid Term Cancelation
        ''' </summary>
        MTC

        ''' <summary>
        ''' 
        ''' </summary>
        MTR

        ''' <summary>
        ''' Renewal
        ''' </summary>
        REN

        ''' <summary>
        ''' Edit
        ''' </summary>
        EDIT


    End Enum
End Class
<Serializable()> Public Class QuoteCollection : Inherits Collections.CollectionBase
    Public Function Print() As String

        Dim sbPrint As New StringBuilder
        sbPrint.AppendLine()
        For Each oQuote As Quote In List
            sbPrint.AppendLine(oQuote.Print())
            sbPrint.AppendLine("<Br/>")
        Next
        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oQuote As Quote) As Integer

        Return List.Add(v_oQuote)

    End Function
    Public Sub Remove(ByVal v_oQuote As Quote)
        List.Remove(v_oQuote)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="index"></param>
    ''' <remarks></remarks>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="i"></param>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Default Public Property Item(ByVal i As Integer) As Quote

        Get
            Return List(i)
        End Get
        Set(ByVal value As Quote)
            List(i) = value
        End Set

    End Property

End Class


'Partial Public Class TaxesAndFeesType

'    Private oFees() As FeesType

'    Private oTaxes() As TaxesType

'    '''<remarks/>
'    Public Property Fees() As FeesType()
'        Get
'            Return Me.oFees
'        End Get
'        Set(ByVal value As FeesType())
'            Me.oFees = value
'        End Set
'    End Property

'    '''<remarks/>
'    Public Property Taxes() As TaxesType()
'        Get
'            Return Me.oTaxes
'        End Get
'        Set(ByVal value As TaxesType())
'            Me.oTaxes = value
'        End Set
'    End Property
'End Class

<Serializable()> Partial Public Class FeesType

    Private sDescription As String

    Private dAmount As Decimal

    '''<remarks/>
    Public Property Description() As String
        Get
            Return Me.sDescription
        End Get
        Set(ByVal value As String)
            Me.sDescription = value
        End Set
    End Property

    '''<remarks/>
    Public Property Amount() As Decimal
        Get
            Return Me.dAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dAmount = value
        End Set
    End Property
End Class


<Serializable()> Partial Public Class TaxesType

    Private sDescription As String

    Private dAmount As Decimal

    '''<remarks/>
    Public Property Description() As String
        Get
            Return Me.sDescription
        End Get
        Set(ByVal value As String)
            Me.sDescription = value
        End Set
    End Property

    '''<remarks/>
    Public Property Amount() As Decimal
        Get
            Return Me.dAmount
        End Get
        Set(ByVal value As Decimal)
            Me.dAmount = value
        End Set
    End Property


End Class
