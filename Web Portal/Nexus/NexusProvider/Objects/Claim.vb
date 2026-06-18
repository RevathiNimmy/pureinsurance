''' <summary>
''' NF 1.4 get the Claims for the Seleced Client
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class Claim

#Region "Private Variables"
    Private iClaimKey As Integer
    Private sClaimNumber As String
    Private sHandler As String
    Private sInsurer As String
    Private sLocation As String
    Private dtLossDate As DateTime
    Private sPolicyNumber As String
    Private sPolicyType As String
    Private sPrimaryCause As String
    Private sDescription As String
    Private dtReportedDate As DateTime
    Private sSecondaryCause As String
    Private sTown As String
    Private sContact As String
    Private sUnderwritingYearCode As String
    Private iBaseClaimKey As Integer
    Private iTPA As Integer

    Private iInsuranceFileKey As Integer
    Private iInsuranceFolderKey As Integer
    Private sClaimDescription As String
    Private sInsuranceRef As String
    Private sClientShortName As String


    Private productDescriptionField As String
    Private lossDateFromField As Date
    Private clientNameField As String
    Private claimStatusIDField As Integer
    Private claimHandlerDescriptionField As String
    Private insurerClaimNumberField As String
    Private clientClaimNumberField As String
    Private primaryCauseDescriptionField As String
    Private secondaryCauseDescriptionField As String
    Private progressStatusDescriptionField As String
    Private paymentsField As Decimal
    Private reserveField As Decimal
    Private currencyISOCodeField As String
    Private isDeletedField As Boolean
    Private isAllowedClosedClaimsField As Boolean
    Private infoOnlyField As Boolean

    Private oclaimCoInsurerField As ClaimCoInsurerCollection

    Private dTotalShare As Decimal
    Private dTotalCurrentShareValue As Decimal

    Private oClaimPayment As ClaimPayment
    Private oClaimPeril As PerilCollection
    Private oClaims As ClaimCollection

    'Newly added Properties for maintainclaim
    Private sClientTelNoField As String

    Private sClientFaxNoField As String

    Private sClientMobileNoField As String

    Private sClientEmailField As String

    Private sClientTelNoOffField As String

    Private oClientField As ClaimParty

    Private oInsurerField As ClaimPartyInsurer

    Private sDescriptionField As String

    Private sProgressStatusCodeField As String

    Private sPrimaryCauseCodeField As String

    Private dtLossFromDateField As Date

    Private dtReportedDateField As Date

    Private sHandlerCodeField As String

    Private bInfoOnlyField As Boolean

    Private bLikelyClaimField As Boolean

    Private sSecondaryCauseCodeField As String

    Private sCatastropheCodeField As String

    Private sCoinsuranceTreatmentCodeField As String

    Private dtLossToDateField As Date

    Private bLossToDateFieldSpecified As Boolean

    Private sLocationField As String

    Private sTownCodeField As String

    Private sUserDefFldACodeField As String

    Private sUserDefFldBCodeField As String

    Private sUserDefFldCCodeField As String

    Private sUserDefFldDCodeField As String

    Private sUserDefFldECodeField As String

    Private sCommentsField As String

    Private sClaimVersionDescriptionField As String

    Private iClaimVersionField As Integer

    Private sClaimStatusField As String

    Private dtClaimStatusDateField As Date
    Private dtLastModifiedDateField As Date
    Private bIgnoreClaimMaintainField As Boolean
    Private bExternalHandlerField As Boolean
    Private bCloseClaimOnZeroReserveRecoveryBalanceField As Boolean
    Private iRiskKey As Integer
    Private sRiskType As String
    Private sRiskTypeDescription As String
    Private sRiskDescription As String
    Private bReserveOnly As Boolean
    Private iBaseCaseKey As Integer

    Private dtCoverFromField, dtCoverToField, dtNotificationDateField As Date
    Private sLeadAgentNameField As String

    Private sCaseNumber As String
    Private bIsRecovery As Boolean
    Private iCaseKey As Integer
    Private bTimeStamp() As Byte

    Private sSearchResultsCol1 As String
    Private bIsPolicyOutstanding As Boolean
#End Region


    Public Sub New()
        oClaimPeril = New PerilCollection
        oClientField = New ClaimParty
        oInsurerField = New ClaimPartyInsurer
        oClaimPayment = New ClaimPayment
    End Sub
    Public Overridable Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("Claim Key : " & iClaimKey & "<br />")
        sbPrint.AppendLine("Claim Number : " & sClaimNumber & "<br />")
        'sbPrint.AppendLine("Claim Status : " & sClaimStatus & "<br />")
        sbPrint.AppendLine("Handler : " & sHandler & "<br />")
        sbPrint.AppendLine("Insurer : " & sInsurer & "<br />")
        sbPrint.AppendLine("Location : " & sLocation & "<br />")
        sbPrint.AppendLine("Loss Date : " & dtLossDate.ToString & "<br />")
        sbPrint.AppendLine("Policy Number : " & sPolicyNumber & "<br />")
        sbPrint.AppendLine("Policy Type : " & sPolicyType & "<br />")
        sbPrint.AppendLine("Primary Cause : " & sPrimaryCause & "<br />")
        sbPrint.AppendLine("Regarding : " & sDescription & "<br />")
        sbPrint.AppendLine("Reported Date : " & dtReportedDate.ToString & "<br />")
        sbPrint.AppendLine("Secordary Cause : " & sSecondaryCause & "<br />")
        sbPrint.AppendLine("Town : " & sTown & "<br />")
        sbPrint.AppendLine("Contact : " & sContact & "<br />")
        sbPrint.AppendLine("CaseKey : " & iCaseKey & "<br />")
        sbPrint.AppendLine("CaseNumber : " & sCaseNumber & "<br />")
        Return sbPrint.ToString()

    End Function

#Region "Public Properties"
    Public Property ReserveOnly() As Boolean
        Get
            Return Me.bReserveOnly
        End Get
        Set(ByVal value As Boolean)
            Me.bReserveOnly = value
        End Set
    End Property

    Public Property BaseClaimKey() As Integer
        Get
            Return Me.iBaseClaimKey
        End Get
        Set(ByVal value As Integer)
            Me.iBaseClaimKey = value
        End Set
    End Property
    Public Property BaseCaseKey() As Integer
        Get
            Return Me.iBaseCaseKey
        End Get
        Set(ByVal value As Integer)
            Me.iBaseCaseKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property LossDate() As DateTime
        Get
            Return Me.dtLossDate
        End Get
        Set(ByVal value As DateTime)
            Me.dtLossDate = value
        End Set
    End Property

    '''<remarks/>
    Public Property PolicyNumber() As String
        Get
            Return Me.sPolicyNumber
        End Get
        Set(ByVal value As String)
            Me.sPolicyNumber = value
        End Set
    End Property

    '''<remarks/>
    Public Property PolicyType() As String
        Get
            Return Me.sPolicyType
        End Get
        Set(ByVal value As String)
            Me.sPolicyType = value
        End Set
    End Property

    '''<remarks/>
    Public Property PrimaryCause() As String
        Get
            Return Me.sPrimaryCause
        End Get
        Set(ByVal value As String)
            Me.sPrimaryCause = value
        End Set
    End Property




    '''<remarks/>
    Public Property SecondaryCause() As String
        Get
            Return Me.sSecondaryCause
        End Get
        Set(ByVal value As String)
            Me.sSecondaryCause = value
        End Set
    End Property

    '''<remarks/>
    Public Property Town() As String
        Get
            Return Me.sTown
        End Get
        Set(ByVal value As String)
            Me.sTown = value
        End Set
    End Property

    '''<remarks/>
    Public Property Contact() As String
        Get
            Return Me.sContact
        End Get
        Set(ByVal value As String)
            Me.sContact = value
        End Set
    End Property
    '''<remarks/>
    Public Property InsuranceRef() As String
        Get
            Return Me.sInsuranceRef
        End Get
        Set(ByVal value As String)
            Me.sInsuranceRef = value
        End Set
    End Property
    '''<remarks/>
    Public Property ClientShortName() As String
        Get
            Return Me.sClientShortName
        End Get
        Set(ByVal value As String)
            Me.sClientShortName = value
        End Set
    End Property
    '''<remarks/>
    Public Property InsuranceFileKey() As Integer
        Get
            Return Me.iInsuranceFileKey
        End Get
        Set(ByVal value As Integer)
            Me.iInsuranceFileKey = value
        End Set
    End Property
    '''<remarks/>
    Public Property InsuranceFolderKey() As Integer
        Get
            Return Me.iInsuranceFolderKey
        End Get
        Set(ByVal value As Integer)
            Me.iInsuranceFolderKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClaimKey() As Integer
        Get
            Return Me.iClaimKey
        End Get
        Set(ByVal value As Integer)
            Me.iClaimKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClaimDescription() As String
        Get
            Return Me.sClaimDescription
        End Get
        Set(ByVal value As String)
            Me.sClaimDescription = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClaimNumber() As String
        Get
            Return Me.sClaimNumber
        End Get
        Set(ByVal value As String)
            Me.sClaimNumber = value
        End Set
    End Property

    '''<remarks/>
    Public Property ProductDescription() As String
        Get
            Return Me.productDescriptionField
        End Get
        Set(ByVal value As String)
            Me.productDescriptionField = value
        End Set
    End Property

    '''<remarks/>
    Public Property LossDateFrom() As Date
        Get
            Return Me.lossDateFromField
        End Get
        Set(ByVal value As Date)
            Me.lossDateFromField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClientName() As String
        Get
            Return Me.clientNameField
        End Get
        Set(ByVal value As String)
            Me.clientNameField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClaimStatusID() As Integer
        Get
            Return Me.claimStatusIDField
        End Get
        Set(ByVal value As Integer)
            Me.claimStatusIDField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClaimHandlerDescription() As String
        Get
            Return Me.claimHandlerDescriptionField
        End Get
        Set(ByVal value As String)
            Me.claimHandlerDescriptionField = value
        End Set
    End Property

    '''<remarks/>
    Public Property InsurerClaimNumber() As String
        Get
            Return Me.insurerClaimNumberField
        End Get
        Set(ByVal value As String)
            Me.insurerClaimNumberField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClientClaimNumber() As String
        Get
            Return Me.clientClaimNumberField
        End Get
        Set(ByVal value As String)
            Me.clientClaimNumberField = value
        End Set
    End Property



    '''<remarks/>
    Public Property PrimaryCauseDescription() As String
        Get
            Return Me.primaryCauseDescriptionField
        End Get
        Set(ByVal value As String)
            Me.primaryCauseDescriptionField = value
        End Set
    End Property

    '''<remarks/>
    Public Property SecondaryCauseDescription() As String
        Get
            Return Me.secondaryCauseDescriptionField
        End Get
        Set(ByVal value As String)
            Me.secondaryCauseDescriptionField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ProgressStatusDescription() As String
        Get
            Return Me.progressStatusDescriptionField
        End Get
        Set(ByVal value As String)
            Me.progressStatusDescriptionField = value
        End Set
    End Property

    '''<remarks/>
    Public Property Payments() As Decimal
        Get
            Return Me.paymentsField
        End Get
        Set(ByVal value As Decimal)
            Me.paymentsField = value
        End Set
    End Property

    '''<remarks/>
    Public Property Reserve() As Decimal
        Get
            Return Me.reserveField
        End Get
        Set(ByVal value As Decimal)
            Me.reserveField = value
        End Set
    End Property

    Public Property ClaimPayment() As ClaimPayment
        Get
            Return Me.oClaimPayment
        End Get
        Set(ByVal value As ClaimPayment)
            Me.oClaimPayment = value
        End Set
    End Property

    '''<remarks/>
    Public Property CurrencyISOCode() As String
        Get
            Return Me.currencyISOCodeField
        End Get
        Set(ByVal value As String)
            Me.currencyISOCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property IsDeleted() As Boolean
        Get
            Return Me.isDeletedField
        End Get
        Set(ByVal value As Boolean)
            Me.isDeletedField = value
        End Set
    End Property

    '''<remarks/>
    Public Property IsAllowedClosedClaims() As Boolean
        Get
            Return Me.isAllowedClosedClaimsField
        End Get
        Set(ByVal value As Boolean)
            Me.isAllowedClosedClaimsField = value
        End Set
    End Property



    Public Property Claim() As ClaimCollection
        Get
            Return Me.oClaims
        End Get
        Set(ByVal value As ClaimCollection)
            Me.oClaims = value
        End Set
    End Property
    '''<remarks/>
    Public Property TotalShare() As Decimal
        Get
            Return Me.dTotalShare
        End Get
        Set(ByVal value As Decimal)
            Me.dTotalShare = value
        End Set
    End Property

    '''<remarks/>
    Public Property TotalCurrentShareValue() As Decimal
        Get
            Return Me.dTotalCurrentShareValue
        End Get
        Set(ByVal value As Decimal)
            Me.dTotalCurrentShareValue = value
        End Set
    End Property

    Public Property ClaimCoInsurer() As ClaimCoInsurerCollection
        Get
            Return Me.oclaimCoInsurerField
        End Get
        Set(ByVal value As ClaimCoInsurerCollection)
            Me.oclaimCoInsurerField = value
        End Set
    End Property
    Public Property ClaimPeril() As PerilCollection
        Get
            Return Me.oClaimPeril
        End Get
        Set(ByVal value As PerilCollection)
            Me.oClaimPeril = value
        End Set
    End Property
    ''' <summary>
    ''' Will be used in searching and displaying in grid
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CaseKey() As Integer
        Get
            Return Me.iCaseKey
        End Get
        Set(ByVal value As Integer)
            Me.iCaseKey = value
        End Set
    End Property
    ''' <summary>
    ''' Will be used in searching and displaying in grid
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CaseNumber() As String
        Get
            Return Me.sCaseNumber
        End Get
        Set(ByVal value As String)
            Me.sCaseNumber = value
        End Set
    End Property

    ''' <summary>
    ''' To Identify Claim Version to be slavage or TP Recovery
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsRecovery() As Boolean
        Get
            Return Me.bIsRecovery
        End Get
        Set(ByVal value As Boolean)
            Me.bIsRecovery = value
        End Set
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TimeStamp() As Byte()
        Get
            Return Me.bTimeStamp
        End Get
        Set(ByVal value As Byte())
            Me.bTimeStamp = value
        End Set
    End Property

    ''' <summary>
    ''' Used to provide search on Risk Index
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property SearchResultsCol1() As String
        Get
            Return Me.sSearchResultsCol1
        End Get
        Set(ByVal value As String)
            Me.sSearchResultsCol1 = value
        End Set
    End Property
    ''' <summary>
    ''' Will be used in searching and displaying Associated Clients in grid
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AssociatedClients As String

#End Region
#Region "Properties"

    '''<remarks/>
    Public Property ClientTelNo() As String
        Get
            Return Me.sClientTelNoField
        End Get
        Set(ByVal value As String)
            Me.sClientTelNoField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClientFaxNo() As String
        Get
            Return Me.sClientFaxNoField
        End Get
        Set(ByVal value As String)
            Me.sClientFaxNoField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClientMobileNo() As String
        Get
            Return Me.sClientMobileNoField
        End Get
        Set(ByVal value As String)
            Me.sClientMobileNoField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClientEmail() As String
        Get
            Return Me.sClientEmailField
        End Get
        Set(ByVal value As String)
            Me.sClientEmailField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClientTelNoOff() As String
        Get
            Return Me.sClientTelNoOffField
        End Get
        Set(ByVal value As String)
            Me.sClientTelNoOffField = value
        End Set
    End Property

    '''<remarks/>
    Public Property Client() As ClaimParty
        Get
            Return Me.oClientField
        End Get
        Set(ByVal value As ClaimParty)
            Me.oClientField = value
        End Set
    End Property

    '''<remarks/>
    Public Property Insurer() As ClaimPartyInsurer
        Get
            Return Me.oInsurerField
        End Get
        Set(ByVal value As ClaimPartyInsurer)
            Me.oInsurerField = value
        End Set
    End Property

    Public Property UnderwritingYearCode() As String
        Get
            Return Me.sUnderwritingYearCode
        End Get
        Set(ByVal value As String)
            Me.sUnderwritingYearCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property Description() As String
        Get
            Return Me.sDescriptionField
        End Get
        Set(ByVal value As String)
            Me.sDescriptionField = value
        End Set
    End Property

    ''' <summary>
    ''' WPR08 - to enable claim attached with TPA
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TPA() As Integer
        Get
            Return Me.iTPA
        End Get
        Set(ByVal value As Integer)
            Me.iTPA = value
        End Set
    End Property
    '''<remarks/>
    Public Property ProgressStatusCode() As String
        Get
            Return Me.sProgressStatusCodeField
        End Get
        Set(ByVal value As String)
            Me.sProgressStatusCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property PrimaryCauseCode() As String
        Get
            Return Me.sPrimaryCauseCodeField
        End Get
        Set(ByVal value As String)
            Me.sPrimaryCauseCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property LossFromDate() As Date
        Get
            Return Me.dtLossFromDateField
        End Get
        Set(ByVal value As Date)
            Me.dtLossFromDateField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ReportedDate() As Date
        Get
            Return Me.dtReportedDateField
        End Get
        Set(ByVal value As Date)
            Me.dtReportedDateField = value
        End Set
    End Property

    '''<remarks/>
    Public Property HandlerCode() As String
        Get
            Return Me.sHandlerCodeField
        End Get
        Set(ByVal value As String)
            Me.sHandlerCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property InfoOnly() As Boolean
        Get
            Return Me.bInfoOnlyField
        End Get
        Set(ByVal value As Boolean)
            Me.bInfoOnlyField = value
        End Set
    End Property

    '''<remarks/>
    Public Property LikelyClaim() As Boolean
        Get
            Return Me.bLikelyClaimField
        End Get
        Set(ByVal value As Boolean)
            Me.bLikelyClaimField = value
        End Set
    End Property

    '''<remarks/>
    Public Property SecondaryCauseCode() As String
        Get
            Return Me.sSecondaryCauseCodeField
        End Get
        Set(ByVal value As String)
            Me.sSecondaryCauseCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property CatastropheCode() As String
        Get
            Return Me.sCatastropheCodeField
        End Get
        Set(ByVal value As String)
            Me.sCatastropheCodeField = value
        End Set
    End Property

    Public Property CoinsuranceTreatmentCode() As String
        Get
            Return Me.sCoinsuranceTreatmentCodeField
        End Get
        Set(ByVal value As String)
            Me.sCoinsuranceTreatmentCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property LossToDate() As Date
        Get
            Return Me.dtLossToDateField
        End Get
        Set(ByVal value As Date)
            Me.dtLossToDateField = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlIgnoreAttribute()> _
    Public Property LossToDateSpecified() As Boolean
        Get
            Return Me.bLossToDateFieldSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bLossToDateFieldSpecified = value
        End Set
    End Property

    '''<remarks/>
    Public Property Location() As String
        Get
            Return Me.sLocationField
        End Get
        Set(ByVal value As String)
            Me.sLocationField = value
        End Set
    End Property

    '''<remarks/>
    Public Property TownCode() As String
        Get
            Return Me.sTownCodeField
        End Get
        Set(ByVal value As String)
            Me.sTownCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property UserDefFldACode() As String
        Get
            Return Me.sUserDefFldACodeField
        End Get
        Set(ByVal value As String)
            Me.sUserDefFldACodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property UserDefFldBCode() As String
        Get
            Return Me.sUserDefFldBCodeField
        End Get
        Set(ByVal value As String)
            Me.sUserDefFldBCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property UserDefFldCCode() As String
        Get
            Return Me.sUserDefFldCCodeField
        End Get
        Set(ByVal value As String)
            Me.sUserDefFldCCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property UserDefFldDCode() As String
        Get
            Return Me.sUserDefFldDCodeField
        End Get
        Set(ByVal value As String)
            Me.sUserDefFldDCodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property UserDefFldECode() As String
        Get
            Return Me.sUserDefFldECodeField
        End Get
        Set(ByVal value As String)
            Me.sUserDefFldECodeField = value
        End Set
    End Property

    '''<remarks/>
    Public Property Comments() As String
        Get
            Return Me.sCommentsField
        End Get
        Set(ByVal value As String)
            Me.sCommentsField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClaimVersionDescription() As String
        Get
            Return Me.sClaimVersionDescriptionField
        End Get
        Set(ByVal value As String)
            Me.sClaimVersionDescriptionField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClaimVersion() As Integer
        Get
            Return Me.iClaimVersionField
        End Get
        Set(ByVal value As Integer)
            Me.iClaimVersionField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClaimStatus() As String
        Get
            Return Me.sClaimStatusField
        End Get
        Set(ByVal value As String)
            Me.sClaimStatusField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClaimStatusDate() As Date
        Get
            Return Me.dtClaimStatusDateField
        End Get
        Set(ByVal value As Date)
            Me.dtClaimStatusDateField = value
        End Set
    End Property
    '''<remarks/>
    Public Property IgnoreClaimMaintain() As Boolean
        Get
            Return Me.bIgnoreClaimMaintainField
        End Get
        Set(ByVal value As Boolean)
            Me.bIgnoreClaimMaintainField = value
        End Set
    End Property

    '''<remarks/>
    Public Property ExternalHandler() As Boolean
        Get
            Return Me.bExternalHandlerField
        End Get
        Set(ByVal value As Boolean)
            Me.bExternalHandlerField = value
        End Set
    End Property

    '''<remarks/>
    Public Property CloseClaimOnZeroReserveRecoveryBalance() As Boolean
        Get
            Return Me.bCloseClaimOnZeroReserveRecoveryBalanceField
        End Get
        Set(ByVal value As Boolean)
            Me.bCloseClaimOnZeroReserveRecoveryBalanceField = value
        End Set
    End Property

    Public Property CloseClaimOnFinalPayment() As Boolean
    '''<remarks/>
    Public Property LastModifiedDate() As Date
        Get
            Return Me.dtLastModifiedDateField
        End Get
        Set(ByVal value As Date)
            Me.dtLastModifiedDateField = value
        End Set
    End Property
    '''<remarks/>
    Public Property RiskKey() As Integer
        Get
            Return Me.iRiskKey
        End Get
        Set(ByVal value As Integer)
            Me.iRiskKey = value
        End Set
    End Property

    Public Property RiskType() As String
        Get
            Return Me.sRiskType
        End Get
        Set(ByVal value As String)
            Me.sRiskType = value
        End Set
    End Property

    Public Property RiskTypeDescription() As String
        Get
            Return Me.sRiskTypeDescription
        End Get
        Set(ByVal value As String)
            Me.sRiskTypeDescription = value
            End Set
    End Property

    Public Property RiskDescription() As String
        Get
            Return Me.sRiskDescription
        End Get
        Set(ByVal value As String)
            Me.sRiskDescription = value
        End Set
    End Property

    '''<remarks/>
    Public Property CoverFrom() As Date
        Get
            Return Me.dtCoverFromField
        End Get
        Set(ByVal value As Date)
            Me.dtCoverFromField = value
        End Set
    End Property

    '''<remarks/>
    Public Property CoverTo() As Date
        Get
            Return Me.dtCoverToField
        End Get
        Set(ByVal value As Date)
            Me.dtCoverToField = value
        End Set
    End Property

    '''<remarks/>
    Public Property NotificationDate() As Date
        Get
            Return Me.dtNotificationDateField
        End Get
        Set(ByVal value As Date)
            Me.dtNotificationDateField = value
        End Set
    End Property

    Public Property LeadAgentName() As String
        Get
            Return Me.sLeadAgentNameField
        End Get
        Set(ByVal value As String)
            Me.sLeadAgentNameField = value
        End Set
    End Property

    
    ''' <summary>
    ''' Hold the flag to determine that policy is outstanding or not based on true/false values.
    ''' </summary>
    ''' <returns></returns>
    Public Property IsPolicyOutstanding() As Boolean
        Get
            Return Me.bIsPolicyOutstanding
        End Get
        Set(ByVal value As Boolean)
            Me.bIsPolicyOutstanding = value
        End Set
    End Property


    Public Property ClaimRiskField() As String

#End Region

End Class
<Serializable()> Public Class ClaimOpen : Inherits Claim
#Region "PrivateFields"

    Private bIgnoreClaimMaintain As Boolean

    Private sDuplicateClaimOverrideUserName As String

    Private sDuplicateClaimOverrideUserPassword As String    

#End Region
    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()

        MyBase.New()
        bIgnoreClaimMaintain = False
    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    ''' Public Overrides Function Print() As String
    Public Overrides Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("Duplicate Claim Override UserName : " & sDuplicateClaimOverrideUserName & "<br />")
        sbPrint.AppendLine("Duplicate Claim Override UserPassword : " & sDuplicateClaimOverrideUserPassword & "<br />")
        sbPrint.AppendLine("IgnoreClaimMaintain: " & IIf(bIgnoreClaimMaintain, "true", "false") & "<br />")

        Return sbPrint.ToString

    End Function
#Region "Properties"
    ''''<remarks/>
    'Public Property IgnoreClaimMaintain() As Boolean
    '    Get
    '        Return Me.bIgnoreClaimMaintain
    '    End Get
    '    Set(ByVal value As Boolean)
    '        Me.bIgnoreClaimMaintain = value
    '    End Set
    'End Property

    '''<remarks/>
    Public Property DuplicateClaimOverrideUserName() As String
        Get
            Return Me.sDuplicateClaimOverrideUserName
        End Get
        Set(ByVal value As String)
            Me.sDuplicateClaimOverrideUserName = value
        End Set
    End Property

    '''<remarks/>
    Public Property DuplicateClaimOverrideUserPassword() As String
        Get
            Return Me.sDuplicateClaimOverrideUserPassword
        End Get
        Set(ByVal value As String)
            Me.sDuplicateClaimOverrideUserPassword = value
        End Set
    End Property        
#End Region
End Class

<Serializable()> Public Class ClaimCollection : Inherits SortableCollectionBase
    Public Sub New()
        MyBase.SortObjectType = GetType(Claim)
    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oClaim As Claim In List
            sbPrint.AppendLine(oClaim.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function
    Public Function Add(ByVal v_oClaim As Claim) As Integer
        Return List.Add(v_oClaim)
    End Function

    Public Sub Remove(ByVal v_oClaim As Claim)
        List.Remove(v_oClaim)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As Claim
        Get
            Return List(i)
        End Get
        Set(ByVal value As Claim)
            List(i) = value
        End Set
    End Property


End Class
<Serializable()> Public Class ClaimOpenCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oClaimOpen As ClaimOpen In List
            sbPrint.AppendLine(oClaimOpen.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function
    Public Function Add(ByVal v_oClaimOpen As ClaimOpen) As Integer
        Return List.Add(v_oClaimOpen)
    End Function

    Public Sub Remove(ByVal v_oClaimOpen As ClaimOpen)
        List.Remove(v_oClaimOpen)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As ClaimOpen
        Get
            Return List(i)
        End Get
        Set(ByVal value As ClaimOpen)
            List(i) = value
        End Set
    End Property


End Class
