Public Class PartyPCType

    Private sSurname As String

    Private sForename As String

    Private dDateOfBirth As Date

    'Private dateOfBirthSpecified As Boolean

    Private sTitle As String

    Private oMaritalStatusCode As MaritalStatusCodeType

    'Private maritalStatusCodeSpecified As Boolean

    Private sGenderCode As String

    Private sInitials As String

    Private sOccupationCode As String

    Private sEmployersBusinessCode As String

    Private oEmploymentStatusCode As EmploymentStatusCodeType

    'Private employmentStatusCodeSpecified As Boolean

    Private sAlternativeId As String

    Private oClientDetail As BaseClientSharedDataType

    Private sTradingName As String

    Private sSecOccupationCode As String

    Private sSecEmployersBusinessCode As String

    Private oSecEmploymentStatusCode As EmploymentStatusCodeType

    ' Private secEmploymentStatusCodeSpecified As Boolean

    Private sNationalityCode As String

    Private sAccommodationCode As String

    Private oLifestyle() As PartyPCTypeLifestyle

    Private sSalutation As String

    Private bTPS As Boolean

    Private bMPS As Boolean

    'Private mPS As Boolean

    'Private mPSSpecified As Boolean

    Private beMPS As Boolean

    'Private eMPSSpecified As Boolean

    Private sSource As String

    Private bPetOwner As Boolean

    'Private petOwnerSpecified As Boolean

    '''<remarks/>
    Public Property Surname() As String
        Get
            Return Me.sSurname
        End Get
        Set(ByVal value As String)
            Me.sSurname = value
        End Set
    End Property

    '''<remarks/>
    Public Property Forename() As String
        Get
            Return Me.sForename
        End Get
        Set(ByVal value As String)
            Me.sForename = value
        End Set
    End Property

    '''<remarks/>
    Public Property DateOfBirth() As Date
        Get
            Return Me.dDateOfBirth
        End Get
        Set(ByVal value As Date)
            Me.dDateOfBirth = value
        End Set
    End Property

    ''''<remarks/>
    '<System.Xml.Serialization.XmlIgnoreAttribute()> _
    'Public Property DateOfBirthSpecified() As Boolean
    '    Get
    '        Return Me.dateOfBirthSpecified
    '    End Get
    '    Set(ByVal value As Boolean)
    '        Me.dateOfBirthSpecified = value
    '    End Set
    End Property

    '''<remarks/>
    Public Property Title() As String
        Get
            Return Me.sTitle
        End Get
        Set(ByVal value As String)
            Me.sTitle = value
        End Set
    End Property

    '''<remarks/>
    Public Property MaritalStatusCode() As MaritalStatusCodeType
        Get
            Return Me.maritalStatusCode
        End Get
        Set(ByVal value As MaritalStatusCodeType)
            Me.maritalStatusCode = value
        End Set
    End Property

    ''''<remarks/>
    '<System.Xml.Serialization.XmlIgnoreAttribute()> _
    'Public Property MaritalStatusCodeSpecified() As Boolean
    '    Get
    '        Return Me.maritalStatusCodeSpecified
    '    End Get
    '    Set(ByVal value As Boolean)
    '        Me.maritalStatusCodeSpecified = value
    '    End Set
    'End Property

    '''<remarks/>
    Public Property GenderCode() As String
        Get
            Return Me.sGenderCode
        End Get
        Set(ByVal value As String)
            Me.sGenderCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property Initials() As String
        Get
            Return Me.sInitials
        End Get
        Set(ByVal value As String)
            Me.sInitials = value
        End Set
    End Property

    '''<remarks/>
    Public Property OccupationCode() As String
        Get
            Return Me.sOccupationCode
        End Get
        Set(ByVal value As String)
            Me.sOccupationCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property EmployersBusinessCode() As String
        Get
            Return Me.sEmployersBusinessCode
        End Get
        Set(ByVal value As String)
            Me.sEmployersBusinessCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property EmploymentStatusCode() As EmploymentStatusCodeType
        Get
            Return Me.employmentStatusCode
        End Get
        Set(ByVal value As EmploymentStatusCodeType)
            Me.employmentStatusCode = value
        End Set
    End Property

    ''''<remarks/>
    '<System.Xml.Serialization.XmlIgnoreAttribute()> _
    'Public Property EmploymentStatusCodeSpecified() As Boolean
    '    Get
    '        Return Me.employmentStatusCodeSpecified
    '    End Get
    '    Set(ByVal value As Boolean)
    '        Me.employmentStatusCodeSpecified = value
    '    End Set
    'End Property

    '''<remarks/>
    Public Property AlternativeId() As String
        Get
            Return Me.sAlternativeId
        End Get
        Set(ByVal value As String)
            Me.sAlternativeId = value
        End Set
    End Property

    '''<remarks/>
    Public Property ClientDetail() As BaseClientSharedDataType
        Get
            Return Me.clientDetail
        End Get
        Set(ByVal value As BaseClientSharedDataType)
            Me.clientDetail = value
        End Set
    End Property

    '''<remarks/>
    Public Property TradingName() As String
        Get
            Return Me.sTradingName
        End Get
        Set(ByVal value As String)
            Me.sTradingName = value
        End Set
    End Property

    '''<remarks/>
    Public Property SecOccupationCode() As String
        Get
            Return Me.sSecOccupationCode
        End Get
        Set(ByVal value As String)
            Me.sSecOccupationCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property SecEmployersBusinessCode() As String
        Get
            Return Me.sSecEmployersBusinessCode
        End Get
        Set(ByVal value As String)
            Me.sSecEmployersBusinessCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property SecEmploymentStatusCode() As EmploymentStatusCodeType
        Get
            Return Me.secEmploymentStatusCode
        End Get
        Set(ByVal value As EmploymentStatusCodeType)
            Me.secEmploymentStatusCode = value
        End Set
    End Property

    ''''<remarks/>
    '<System.Xml.Serialization.XmlIgnoreAttribute()> _
    'Public Property SecEmploymentStatusCodeSpecified() As Boolean
    '    Get
    '        Return Me.secEmploymentStatusCodeSpecified
    '    End Get
    '    Set(ByVal value As Boolean)
    '        Me.secEmploymentStatusCodeSpecified = value
    '    End Set
    'End Property

    '''<remarks/>
    Public Property NationalityCode() As String
        Get
            Return Me.sNationalityCode
        End Get
        Set(ByVal value As String)
            Me.sNationalityCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property AccommodationCode() As String
        Get
            Return Me.sAccommodationCode
        End Get
        Set(ByVal value As String)
            Me.sAccommodationCode = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute("Lifestyle")> _
    Public Property Lifestyle() As PartyPCTypeLifestyle()
        Get
            Return Me.lifestyle
        End Get
        Set(ByVal value As PartyPCTypeLifestyle())
            Me.lifestyle = value
        End Set
    End Property

    '''<remarks/>
    Public Property Salutation() As String
        Get
            Return Me.sSalutation
        End Get
        Set(ByVal value As String)
            Me.sSalutation = value
        End Set
    End Property

    '''<remarks/>
    Public Property TPS() As Boolean
        Get
            Return Me.bTPS
        End Get
        Set(ByVal value As Boolean)
            Me.bTPS = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlIgnoreAttribute()> _
    Public Property TPSSpecified() As Boolean
        Get
            Return Me.bMPS
        End Get
        Set(ByVal value As Boolean)
            Me.bMPS = value
        End Set
    End Property

    ''''<remarks/>
    'Public Property MPS() As Boolean
    '    Get
    '        Return Me.mPS
    '    End Get
    '    Set(ByVal value As Boolean)
    '        Me.mPS = value
    '    End Set
    'End Property

    ''''<remarks/>
    '<System.Xml.Serialization.XmlIgnoreAttribute()> _
    'Public Property MPSSpecified() As Boolean
    '    Get
    '        Return Me.mPSSpecified
    '    End Get
    '    Set(ByVal value As Boolean)
    '        Me.mPSSpecified = value
    '    End Set
    'End Property

    '''<remarks/>
    Public Property eMPS() As Boolean
        Get
            Return Me.beMPS
        End Get
        Set(ByVal value As Boolean)
            Me.beMPS = value
        End Set
    End Property

    ''''<remarks/>
    '<System.Xml.Serialization.XmlIgnoreAttribute()> _
    'Public Property eMPSSpecified() As Boolean
    '    Get
    '        Return Me.eMPSSpecified
    '    End Get
    '    Set(ByVal value As Boolean)
    '        Me.eMPSSpecified = value
    '    End Set
    'End Property

    '''<remarks/>
    Public Property Source() As String
        Get
            Return Me.sSource
        End Get
        Set(ByVal value As String)
            Me.sSource = value
        End Set
    End Property

    '''<remarks/>
    Public Property PetOwner() As Boolean
        Get
            Return Me.bPetOwner
        End Get
        Set(ByVal value As Boolean)
            Me.bPetOwner = value
        End Set
    End Property

    ''''<remarks/>
    '<System.Xml.Serialization.XmlIgnoreAttribute()> _
    'Public Property PetOwnerSpecified() As Boolean
    '    Get
    '        Return Me.petOwnerSpecified
    '    End Get
    '    Set(ByVal value As Boolean)
    '        Me.petOwnerSpecified = value
    '    End Set
    'End Property

End Class




Partial Public Class PartyPCTypeLifestyle

    Private iLifestyleKey As Integer

    Private sName As String

    Private dDateOfBirth As Date

    'Private dateOfBirthSpecified As Boolean

    Private sCategoryCode As String

    Private oGenderCode As GenderCodeType

    'Private genderCodeSpecified As Boolean

    Private sOccupationCode As String

    Private sSecOccupationCode As String

    Private bSmoker As Boolean

    'Private smokerSpecified As Boolean

    '''<remarks/>
    Public Property LifestyleKey() As Integer
        Get
            Return Me.iLifestyleKey
        End Get
        Set(ByVal value As Integer)
            Me.iLifestyleKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property Name() As String
        Get
            Return Me.sName
        End Get
        Set(ByVal value As String)
            Me.sName = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute(DataType:="date")> _
    Public Property DateOfBirth() As Date
        Get
            Return Me.dDateOfBirth
        End Get
        Set(ByVal value As Date)
            Me.dDateOfBirth = value
        End Set
    End Property

    ''''<remarks/>
    '<System.Xml.Serialization.XmlIgnoreAttribute()> _
    'Public Property DateOfBirthSpecified() As Boolean
    '    Get
    '        Return Me.dateOfBirthSpecified
    '    End Get
    '    Set(ByVal value As Boolean)
    '        Me.dateOfBirthSpecified = value
    '    End Set
    'End Property

    '''<remarks/>
    Public Property CategoryCode() As String
        Get
            Return Me.sCategoryCode
        End Get
        Set(ByVal value As String)
            Me.sCategoryCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property GenderCode() As GenderCodeType
        Get
            Return Me.genderCode
        End Get
        Set(ByVal value As GenderCodeType)
            Me.genderCode = value
        End Set
    End Property

    ''''<remarks/>
    '<System.Xml.Serialization.XmlIgnoreAttribute()> _
    'Public Property GenderCodeSpecified() As Boolean
    '    Get
    '        Return Me.genderCodeSpecified
    '    End Get
    '    Set(ByVal value As Boolean)
    '        Me.genderCodeSpecified = value
    '    End Set
    'End Property

    '''<remarks/>
    Public Property OccupationCode() As String
        Get
            Return Me.sOccupationCode
        End Get
        Set(ByVal value As String)
            Me.sOccupationCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property SecOccupationCode() As String
        Get
            Return Me.sSecOccupationCode
        End Get
        Set(ByVal value As String)
            Me.sSecOccupationCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property Smoker() As Boolean
        Get
            Return Me.bSmoker
        End Get
        Set(ByVal value As Boolean)
            Me.bSmoker = value
        End Set
    End Property

    ''''<remarks/>
    '<System.Xml.Serialization.XmlIgnoreAttribute()> _
    'Public Property SmokerSpecified() As Boolean
    '    Get
    '        Return Me.smokerSpecified
    '    End Get
    '    Set(ByVal value As Boolean)
    '        Me.smokerSpecified = value
    '    End Set
    'End Property
End Class

Public Enum GenderCodeType

    '''<remarks/>
    F

    '''<remarks/>
    M
End Enum

Public Enum MaritalStatusCodeType

    '''<remarks/>
    D

    '''<remarks/>
    C

    '''<remarks/>
    M

    '''<remarks/>
    N

    '''<remarks/>
    O

    '''<remarks/>
    P

    '''<remarks/>
    A

    '''<remarks/>
    S

    '''<remarks/>
    W
End Enum

Public Enum EmploymentStatusCodeType

    '''<remarks/>
    C

    '''<remarks/>
    E

    '''<remarks/>
    H

    '''<remarks/>
    F

    '''<remarks/>
    I

    '''<remarks/>
    N

    '''<remarks/>
    R

    '''<remarks/>
    S

    '''<remarks/>
    U

    '''<remarks/>
    V
End Enum

Partial Public Class BaseClientSharedDataType

    Private sShortName As String

    Private sServiceLevelCode As String

    Private sAreaCode As String

    Private iLeadAgentKey As Integer

    Private bLeadAgentKeySpecified As Boolean

    Private bIsProspect As Boolean

    Private bIsProspectSpecified As Boolean

    Private bIsAgent As Boolean

    Private bIsAgentSpecified As Boolean

    Private sCorrespondenceCode As String

    Private sPaymentCode As String

    Private sReminderCode As String

    Private sPaymentTermCode As String

    Private sRenewalStopCode As String

    Private sLoyaltyNumber As String

    Private sSeasonalGiftCode As String

    Private oAssociates() As BaseAssociateType

    Private oConvictions() As BaseConvictionType

    Private dCountyCourtJudgments As Decimal

    Private bCountyCourtJudgmentsSpecified As Boolean

    Private oLoyaltyScheme() As BaseClientSharedDataTypeLoyaltyScheme

    Private sAgentReference As String

    Private iCurrentIntermediaryKey As Integer

    Private bCurrentIntermediaryKeySpecified As Boolean

    Private sStrengthCode As String

    Private sStatusCode As String

    Private iPreviousInsurerKey As Integer

    Private bPreviousInsurerKeySpecified As Boolean

    Private iPreviousBrokerKey As Integer

    Private bPreviousBrokerKeySpecified As Boolean

    Private oProspectPolicies() As BaseClientSharedDataTypeProspectPolicies

    Private sCurrentIntermediaryName As String

    Private sLeadAgentCode As String

    Private sLeadAgentName As String

    Private sPreviousInsurerCode As String

    Private sPreviousInsurerName As String

    Private sPreviousBrokerCode As String

    Private sPreviousBrokerName As String

    Private dAccountBalance As Decimal

    Private bAccountBalanceSpecified As Boolean

    Private dYearToDateTurnover As Decimal

    Private bYearToDateTurnoverSpecified As Boolean

    Private dLastYearTurnover As Decimal

    Private bLastYearTurnoverSpecified As Boolean

    '''<remarks/>
    Public Property ShortName() As String
        Get
            Return Me.sShortName
        End Get
        Set(ByVal value As String)
            Me.sShortName = value
        End Set
    End Property

    '''<remarks/>
    Public Property ServiceLevelCode() As String
        Get
            Return Me.sServiceLevelCode
        End Get
        Set(ByVal value As String)
            Me.sServiceLevelCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property AreaCode() As String
        Get
            Return Me.sAreaCode
        End Get
        Set(ByVal value As String)
            Me.sAreaCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property LeadAgentKey() As Integer
        Get
            Return Me.iLeadAgentKey
        End Get
        Set(ByVal value As Integer)
            Me.iLeadAgentKey = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlIgnoreAttribute()> _
    Public Property LeadAgentKeySpecified() As Boolean
        Get
            Return Me.bLeadAgentKeySpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bLeadAgentKeySpecified = value
        End Set
    End Property

    '''<remarks/>
    Public Property IsProspect() As Boolean
        Get
            Return Me.bIsProspect
        End Get
        Set(ByVal value As Boolean)
            Me.bIsProspect = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlIgnoreAttribute()> _
    Public Property IsProspectSpecified() As Boolean
        Get
            Return Me.bIsProspectSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bIsProspectSpecified = value
        End Set
    End Property

    '''<remarks/>
    Public Property IsAgent() As Boolean
        Get
            Return Me.bIsAgent
        End Get
        Set(ByVal value As Boolean)
            Me.bIsAgent = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlIgnoreAttribute()> _
    Public Property IsAgentSpecified() As Boolean
        Get
            Return Me.bIsAgentSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bIsAgentSpecified = value
        End Set
    End Property

    '''<remarks/>
    Public Property CorrespondenceCode() As String
        Get
            Return Me.sCorrespondenceCode
        End Get
        Set(ByVal value As String)
            Me.sCorrespondenceCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property PaymentCode() As String
        Get
            Return Me.sPaymentCode
        End Get
        Set(ByVal value As String)
            Me.sPaymentCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property ReminderCode() As String
        Get
            Return Me.sReminderCode
        End Get
        Set(ByVal value As String)
            Me.sReminderCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property PaymentTermCode() As String
        Get
            Return Me.sPaymentTermCode
        End Get
        Set(ByVal value As String)
            Me.sPaymentTermCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property RenewalStopCode() As String
        Get
            Return Me.sRenewalStopCode
        End Get
        Set(ByVal value As String)
            Me.sRenewalStopCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property LoyaltyNumber() As String
        Get
            Return Me.sLoyaltyNumber
        End Get
        Set(ByVal value As String)
            Me.sLoyaltyNumber = value
        End Set
    End Property

    '''<remarks/>
    Public Property SeasonalGiftCode() As String
        Get
            Return Me.sSeasonalGiftCode
        End Get
        Set(ByVal value As String)
            Me.sSeasonalGiftCode = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute("Associates")> _
    Public Property Associates() As BaseAssociateType()
        Get
            Return Me.associates
        End Get
        Set(ByVal value As BaseAssociateType())
            Me.associates = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute("Convictions")> _
    Public Property Convictions() As BaseConvictionType()
        Get
            Return Me.convictions
        End Get
        Set(ByVal value As BaseConvictionType())
            Me.convictions = value
        End Set
    End Property

    '''<remarks/>
    Public Property CountyCourtJudgments() As Decimal
        Get
            Return Me.dCountyCourtJudgments
        End Get
        Set(ByVal value As Decimal)
            Me.dCountyCourtJudgments = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlIgnoreAttribute()> _
    Public Property CountyCourtJudgmentsSpecified() As Boolean
        Get
            Return Me.bCountyCourtJudgmentsSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bCountyCourtJudgmentsSpecified = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute("LoyaltyScheme")> _
    Public Property LoyaltyScheme() As BaseClientSharedDataTypeLoyaltyScheme()
        Get
            Return Me.loyaltyScheme
        End Get
        Set(ByVal value As BaseClientSharedDataTypeLoyaltyScheme())
            Me.loyaltyScheme = value
        End Set
    End Property

    '''<remarks/>
    Public Property AgentReference() As String
        Get
            Return Me.sAgentReference
        End Get
        Set(ByVal value As String)
            Me.sAgentReference = value
        End Set
    End Property

    '''<remarks/>
    Public Property CurrentIntermediaryKey() As Integer
        Get
            Return Me.iCurrentIntermediaryKey
        End Get
        Set(ByVal value As Integer)
            Me.iCurrentIntermediaryKey = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlIgnoreAttribute()> _
    Public Property CurrentIntermediaryKeySpecified() As Boolean
        Get
            Return Me.bCurrentIntermediaryKeySpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bCurrentIntermediaryKeySpecified = value
        End Set
    End Property

    '''<remarks/>
    Public Property StrengthCode() As String
        Get
            Return Me.sStrengthCode
        End Get
        Set(ByVal value As String)
            Me.sStrengthCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property StatusCode() As String
        Get
            Return Me.sStatusCode
        End Get
        Set(ByVal value As String)
            Me.sStatusCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property PreviousInsurerKey() As Integer
        Get
            Return Me.iPreviousInsurerKey
        End Get
        Set(ByVal value As Integer)
            Me.iPreviousInsurerKey = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlIgnoreAttribute()> _
    Public Property PreviousInsurerKeySpecified() As Boolean
        Get
            Return Me.bPreviousInsurerKeySpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bPreviousInsurerKeySpecified = value
        End Set
    End Property

    '''<remarks/>
    Public Property PreviousBrokerKey() As Integer
        Get
            Return Me.iPreviousBrokerKey
        End Get
        Set(ByVal value As Integer)
            Me.iPreviousBrokerKey = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlIgnoreAttribute()> _
    Public Property PreviousBrokerKeySpecified() As Boolean
        Get
            Return Me.bPreviousBrokerKeySpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bPreviousBrokerKeySpecified = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlElementAttribute("ProspectPolicies")> _
    Public Property ProspectPolicies() As BaseClientSharedDataTypeProspectPolicies()
        Get
            Return Me.prospectPolicies
        End Get
        Set(ByVal value As BaseClientSharedDataTypeProspectPolicies())
            Me.prospectPolicies = value
        End Set
    End Property

    '''<remarks/>
    Public Property CurrentIntermediaryName() As String
        Get
            Return Me.sCurrentIntermediaryName
        End Get
        Set(ByVal value As String)
            Me.sCurrentIntermediaryName = value
        End Set
    End Property

    '''<remarks/>
    Public Property LeadAgentCode() As String
        Get
            Return Me.sLeadAgentCode
        End Get
        Set(ByVal value As String)
            Me.sLeadAgentCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property LeadAgentName() As String
        Get
            Return Me.sLeadAgentName
        End Get
        Set(ByVal value As String)
            Me.sLeadAgentName = value
        End Set
    End Property

    '''<remarks/>
    Public Property PreviousInsurerCode() As String
        Get
            Return Me.sPreviousInsurerCode
        End Get
        Set(ByVal value As String)
            Me.sPreviousInsurerCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property PreviousInsurerName() As String
        Get
            Return Me.sPreviousInsurerName
        End Get
        Set(ByVal value As String)
            Me.sPreviousInsurerName = value
        End Set
    End Property

    '''<remarks/>
    Public Property PreviousBrokerCode() As String
        Get
            Return Me.sPreviousBrokerCode
        End Get
        Set(ByVal value As String)
            Me.sPreviousBrokerCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property PreviousBrokerName() As String
        Get
            Return Me.sPreviousBrokerName
        End Get
        Set(ByVal value As String)
            Me.sPreviousBrokerName = value
        End Set
    End Property

    '''<remarks/>
    Public Property AccountBalance() As Decimal
        Get
            Return Me.dAccountBalance
        End Get
        Set(ByVal value As Decimal)
            Me.dAccountBalance = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlIgnoreAttribute()> _
    Public Property AccountBalanceSpecified() As Boolean
        Get
            Return Me.bAccountBalanceSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bAccountBalanceSpecified = value
        End Set
    End Property

    '''<remarks/>
    Public Property YearToDateTurnover() As Decimal
        Get
            Return Me.dYearToDateTurnover
        End Get
        Set(ByVal value As Decimal)
            Me.dYearToDateTurnover = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlIgnoreAttribute()> _
    Public Property YearToDateTurnoverSpecified() As Boolean
        Get
            Return Me.bYearToDateTurnoverSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bYearToDateTurnoverSpecified = value
        End Set
    End Property

    '''<remarks/>
    Public Property LastYearTurnover() As Decimal
        Get
            Return Me.dLastYearTurnover
        End Get
        Set(ByVal value As Decimal)
            Me.dLastYearTurnover = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlIgnoreAttribute()> _
    Public Property LastYearTurnoverSpecified() As Boolean
        Get
            Return Me.bLastYearTurnoverSpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bLastYearTurnoverSpecified = value
        End Set
    End Property
End Class