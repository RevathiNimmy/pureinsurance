
Option Strict Off
Option Explicit On

Imports System.Runtime.Serialization
Imports System.CodeDom
Imports Sirius.Architecture.ExceptionHandling

Namespace SFI.Messaging.WCF

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="NewBusinessRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(NewBusinessRequestType))>
    Partial Public Class NewBusinessRequestType
        Inherits BaseNBQuoteRequestType
    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseNBQuoteRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(PolicyProcessRequestType)),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(NewBusinessRequestType)),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseNBQuoteRequestType))>
    Partial Public Class BaseNBQuoteRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AgentCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CurrencyCode As CurrencyType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CurrencyCodeSpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property UpdateParty As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Item As BasePartyType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Policy As BaseQuoteRiskMsgType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ClientID As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ClientCodeSpecified As Boolean = False
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038"),
        System.SerializableAttribute(),
        System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929")>
    Public Enum CurrencyType

        <EnumMember>
        GBP

        <EnumMember>
        USD

        <EnumMember>
        EUR
    End Enum

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BasePartyCCType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BasePartyCCType))>
    Partial Public Class BasePartyCCType
        Inherits BasePartyType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CompanyName As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property BusinessCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property MainContact As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property NumberOfOffices As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property NumberOfOfficesSpecified As Boolean = False = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property NumberOfEmployees As String = "" = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BasePartyType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BasePartyCCType)),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BasePartyPCType)),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BasePartyType))>
    Partial Public Class BasePartyType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Addresses As System.Collections.Generic.List(Of BaseAddressType)

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TPUserCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TPIntroducer As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Contacts As System.Collections.Generic.List(Of BaseContactType)

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AccountExecutive As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Currency As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property FileCode As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseAddressType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddAddressRequestType)),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddressType))>
    Partial Public Class BaseAddressType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AddressTypeCode As AddressTypeType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AddressLine1 As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AddressLine2 As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AddressLine3 As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AddressLine4 As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CountryCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PostCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AddressLine5() As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AddressLine6() As String=""
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AddressLine7() As String = ""
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AddressLine8() As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AddressLine9() As String = ""
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AddressLine10() As String = ""
            
      




    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038"),
    System.SerializableAttribute(),
    System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929")>
    Public Enum AddressTypeType

        <EnumMember>
        Item3131001

        <EnumMember>
        Item3131002

        <EnumMember>
        Item31310X9

        <EnumMember>
        Item31310XR

        <EnumMember>
        Item3131XBA

        <EnumMember>
        Item3131XBI

        <EnumMember>
        Item3131XCO

        <EnumMember>
        Item3131XPR

        <EnumMember>
        Item3131XRE

        <EnumMember>
        Item3131XSA
    End Enum

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseTransactType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseTransactRequestType)),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(NBTransactRequestType)),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseTransactType))>
    Partial Public Class BaseTransactType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property QuoteRef As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseTransactRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(NBTransactRequestType)),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseTransactRequestType))>
    Partial Public Class BaseTransactRequestType
        Inherits BaseTransactType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Risks As System.Collections.Generic.List(Of BaseRiskType)

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseRiskType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddRiskRequestType)),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseRiskType))>
    Partial Public Class BaseRiskType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RiskTypeCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ScreenCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RiskDescription As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property DataModelCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property QuoteTimeStamp As Byte()

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RunDefaultRules As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RiskFolderKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RiskFolderKeySpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property OriginalRiskKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property OriginalRiskKeySpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property XMLDataSet As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ProductBuilderDetail As System.Collections.Generic.List(Of BaseProductBuilderRiskType)

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Taxes As System.Collections.Generic.List(Of BaseTaxesType)

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseProductBuilderRiskType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseProductBuilderRiskType))>
    Partial Public Class BaseProductBuilderRiskType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ProductBuilderData As BaseProductBuilderRiskTypeProductBuilderData

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseProductBuilderRiskTypeProductBuilderData", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseProductBuilderRiskTypeProductBuilderData))>
    Partial Public Class BaseProductBuilderRiskTypeProductBuilderData

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ItemName As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Value As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseTaxesType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseTaxesType))>
    Partial Public Class BaseTaxesType
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Description As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Amount As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TaxBandCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TaxRate As Decimal

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseCoInsurerType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseCoInsurerType))>
    Partial Public Class BaseCoInsurerType
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ParticipantCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsurerKey() As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ArrangementRef() As String = ""


        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SharePerc() As Double = 0


        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CommPerc() As Double = 0


    End Class
    '''<remarks/>
    <System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetOnlineClientListRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetHistoricalTransactionsRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetOpenTransactionsRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetClaimsRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateRiskRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdatePartyRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateQuoteRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseRunDefaultRulesEditRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseRunDefaultRulesAddRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseRiskType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddRiskRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseQuoteType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseQuoteRiskMsgType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddQuoteRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseNBQuoteRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(PolicyProcessRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(NewBusinessRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetRiskRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetPartySummaryRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetPartyRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetListRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetInstalmentQuotesRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetHeaderAndSummariesByRefRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetHeaderAndSummariesByKeyRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetDefaultDatasetRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetDatasetSchemaRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(GetDatasetSchemaRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetDatasetDefinitionRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetAllPolicyVersionsRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetAddressRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetRiskByProductRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetRatingDetailsRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetProductByAgentRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetCurrenciesByBranchRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGenerateDocumentRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseForgottenPasswordRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseFindPartyRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseFindControlSearchRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseDeleteRiskRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseCreateEventRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseChangePasswordRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseBindQuoteRequestType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BasePartyType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BasePartyCCType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BasePartyPCType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddPartyRequestType)),
        System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Runtime.Serialization.DataContractAttribute(Name:="BaseRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseRequestType))>
    Partial Public MustInherit Class BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute(IsRequired:=True, EmitDefaultValue:=False)>
        Public Property BranchCode As String

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetOnlineClientListRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetOnlineClientListRequestType))>
    Partial Public Class BaseGetOnlineClientListRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ChangedAfterDateTime As Date

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetHistoricalTransactionsRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetHistoricalTransactionsRequestType))>
    Partial Public Class BaseGetHistoricalTransactionsRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PartyKey As Integer = 0

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetOpenTransactionsRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetOpenTransactionsRequestType))>
    Partial Public Class BaseGetOpenTransactionsRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PartyKey As Integer = 0

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetClaimsRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetClaimsRequestType))>
    Partial Public Class BaseGetClaimsRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PartyKey As Integer = 0

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
        System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
        System.Runtime.Serialization.DataContractAttribute(Name:="BaseUpdateRiskRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateRiskRequestType))>
    Partial Public Class BaseUpdateRiskRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SubBranchCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFolderKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFileKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RiskKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ScreenCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RiskDescription As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property XMLDataSet As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property QuoteTimeStamp As Byte()

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
            System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
            System.Runtime.Serialization.DataContractAttribute(Name:="BaseUpdatePartyRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
            System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdatePartyRequestType))>
    Partial Public Class BaseUpdatePartyRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SubBranchCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PartyKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PartyTimestamp As Byte()

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Item As BasePartyType

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
                System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
                System.Runtime.Serialization.DataContractAttribute(Name:="BasePartyPCType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
                System.Runtime.Serialization.KnownTypeAttribute(GetType(BasePartyPCType))>
    Partial Public Class BasePartyPCType
        Inherits BasePartyType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Surname As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Forename As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property DateOfBirth As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property DateOfBirthSpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Title As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property MaritalStatusCode As MaritalStatusCodeType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property MaritalStatusCodeSpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property GenderCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Initials As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property OccupationCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property EmployersBusinessCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property EmploymentStatusCode As EmploymentStatusCodeType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property EmploymentStatusCodeSpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AlternativeId As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property NationalityCode As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
            System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
            System.Runtime.Serialization.DataContractAttribute(Name:="MaritalStatusCodeType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
            System.Runtime.Serialization.KnownTypeAttribute(GetType(MaritalStatusCodeType))>
    Public Enum MaritalStatusCodeType

        <EnumMember>
        D

        <EnumMember>
        C

        <EnumMember>
        M

        <EnumMember>
        N

        <EnumMember>
        O

        <EnumMember>
        P

        <EnumMember>
        A

        <EnumMember>
        S

        <EnumMember>
        W
    End Enum

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
            System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
            System.Runtime.Serialization.DataContractAttribute(Name:="EmploymentStatusCodeType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
            System.Runtime.Serialization.KnownTypeAttribute(GetType(EmploymentStatusCodeType))>
    Public Enum EmploymentStatusCodeType

        <EnumMember>
        C

        <EnumMember>
        E

        <EnumMember>
        H

        <EnumMember>
        F

        <EnumMember>
        I

        <EnumMember>
        N

        <EnumMember>
        R

        <EnumMember>
        S

        <EnumMember>
        U

        <EnumMember>
        V
    End Enum

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
        System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
        System.Runtime.Serialization.DataContractAttribute(Name:="BaseUpdateQuoteRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateQuoteRequestType))>
    Partial Public Class BaseUpdateQuoteRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFolderKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFileKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CoverStartDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CoverEndDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuredParties As System.Xml.XmlElement

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Description As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property QuoteTimeStamp As Byte()

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CurrencyCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ConsolidatedLeadAgentCommission As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ConsolidatedLeadAgentCommissionSpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ConsolidatedSubAgentCommission As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ConsolidatedSubAgentCommissionSpecified As Boolean = False

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
        System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
        System.Runtime.Serialization.DataContractAttribute(Name:="BaseRunDefaultRulesEditRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseRunDefaultRulesEditRequestType))>
    Partial Public Class BaseRunDefaultRulesEditRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ScreenCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property XMLDataSet As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
        System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
        System.Runtime.Serialization.DataContractAttribute(Name:="BaseRunDefaultRulesAddRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseRunDefaultRulesAddRequestType))>
    Partial Public Class BaseRunDefaultRulesAddRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ScreenCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property XMLDataSet As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseQuoteType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseQuoteRiskMsgType)),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddQuoteRequestType)),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseQuoteType))>
    Partial Public Class BaseQuoteType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property MTAReasonCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property BusinessTypeCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AnalysisCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property LastTransDescription As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property OldPolicyNumber As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TransactionDueDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CoverStartDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CoverEndDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ProductCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Description As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property QuoteRef As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property NewQuoteRef As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuredName As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CurrencyCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TransactionTypeCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyStatusCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AlternateReference As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CoInsurancePlacement As String

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property IsMarketPlacePolicy As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyProcessType As PolicyProcessTypes

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SkipNumberingScheme As Boolean

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PartyKey As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public UnderwritingYearCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PaymentTermCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CollectionFrequencyCode As String = ""
    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
        System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
        System.Runtime.Serialization.DataContractAttribute(Name:="BaseQuoteRiskMsgType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseQuoteRiskMsgType))>
    Partial Public Class BaseQuoteRiskMsgType
        Inherits BaseQuoteType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Risks As System.Collections.Generic.List(Of BaseRiskType)

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CommissionRate As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CommissionValue As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Taxes As System.Collections.Generic.List(Of BaseTaxesType)

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property OverrideNetPremium As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property DoNotCopyRiskAtRenSelection As Integer

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property DeletePolicyUnderRenewal As Integer

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CoInsurers As System.Collections.Generic.List(Of BaseCoInsurerType)
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property IsBDXRequest As Boolean = False

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
        System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
        System.Runtime.Serialization.DataContractAttribute(Name:="BaseAddQuoteRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddQuoteRequestType))>
    Partial Public Class BaseAddQuoteRequestType
        Inherits BaseQuoteType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SubBranchCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ConsolidatedLeadAgentCommission As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ConsolidatedLeadAgentCommissionSpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ConsolidatedSubAgentCommission As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ConsolidatedSubAgentCommissionSpecified As Boolean = False

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetRiskRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetRiskRequestType))>
    Partial Public Class BaseGetRiskRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RiskKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFileKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFolderKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property QuoteTimeStamp As Byte()

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetPartySummaryRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetPartySummaryRequestType))>
    Partial Public Class BaseGetPartySummaryRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PartyKey As Integer = 0

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetPartyRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetPartyRequestType))>
    Partial Public Class BaseGetPartyRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PartyKey As Integer = 0

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetListRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetListRequestType))>
    Partial Public Class BaseGetListRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ListType() As STSListType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ListCode As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="STSListType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(STSListType))>
    Public Enum STSListType

        <EnumMember>
        GisList

        <EnumMember>
        Missing

        <EnumMember>
        PMLookup

        <EnumMember>
        UserDefinedTable
    End Enum

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetInstalmentQuotesRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetInstalmentQuotesRequestType))>
    Partial Public Class BaseGetInstalmentQuotesRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFileKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property QuoteDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property StartDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property EndDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PreferredDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property MonthDay As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property WeekDay As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AmountToFinance As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PaymentProtection As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property OverrideRate As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property OverrideInterestRate As Double

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetHeaderAndSummariesByRefRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetHeaderAndSummariesByRefRequestType))>
    Partial Public Class BaseGetHeaderAndSummariesByRefRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceRef As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetHeaderAndSummariesByKeyRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetHeaderAndSummariesByKeyRequestType))>
    Partial Public Class BaseGetHeaderAndSummariesByKeyRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFileKey As Integer = 0

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetDefaultDatasetRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetDefaultDatasetRequestType))>
    Partial Public Class BaseGetDefaultDatasetRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ScreenCode As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetDatasetSchemaRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetDatasetSchemaRequestType)),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(GetDatasetSchemaRequestType))>
    Partial Public Class BaseGetDatasetSchemaRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property DataModelCode As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetDatasetDefinitionRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetDatasetDefinitionRequestType))>
    Partial Public Class BaseGetDatasetDefinitionRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property DataModelCode As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetAllPolicyVersionsRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetAllPolicyVersionsRequestType))>
    Partial Public Class BaseGetAllPolicyVersionsRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFolderKey As Integer = 0

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetAddressRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetAddressRequestType))>
    Partial Public Class BaseGetAddressRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AddressKey As Integer = 0

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetRiskByProductRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetRiskByProductRequestType))>
    Partial Public Class BaseGetRiskByProductRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ProductCode As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetRatingDetailsRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetRatingDetailsRequestType))>
    Partial Public Class BaseGetRatingDetailsRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RiskKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFileKey As Integer = 0

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetProductByAgentRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetProductByAgentRequestType))>
    Partial Public Class BaseGetProductByAgentRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AgentKey As Integer = 0

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetCurrenciesByBranchRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetCurrenciesByBranchRequestType))>
    Partial Public Class BaseGetCurrenciesByBranchRequestType
        Inherits BaseRequestType
    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGenerateDocumentRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGenerateDocumentRequestType))>
    Partial Public Class BaseGenerateDocumentRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property DocumentTemplateCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Mode As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PartyKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFileKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFolderKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ParameterXML As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property OutputAsHTML As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property OutputAsPDF As Boolean = False

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseForgottenPasswordRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseForgottenPasswordRequestType))>
    Partial Public Class BaseForgottenPasswordRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property UserName As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseFindPartyRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseFindPartyRequestType))>
    Partial Public Class BaseFindPartyRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PartyType As PartyTypeType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PartyTypeSpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Shortname As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AlternativeId As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Name As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Firstname As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AddressLine1 As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AddressLine2 As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AddressLine3 As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AddressLine4 As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AddressLine5 As String = ""
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AddressLine6 As String = ""
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AddressLine7 As String = ""
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AddressLine8 As String = ""
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AddressLine9 As String = ""
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AddressLine10 As String = ""
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PostCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AreaCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TelephoneNumber As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property DateOfBirth As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property DateOfBirthSpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyRef As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RiskIndex As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property FileCode As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="PartyTypeType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(PartyTypeType))>
    Public Enum PartyTypeType

        <EnumMember>
        PC

        <EnumMember>
        GC

        <EnumMember>
        AG

        <EnumMember>
        CC

        <EnumMember>
        CO

        <EnumMember>
        AH

        <EnumMember>
        [IN]

        <EnumMember>
        BR

        <EnumMember>
        FE

        <EnumMember>
        EX

        <EnumMember>
        DI

        <EnumMember>
        CM

        <EnumMember>
        NC

        <EnumMember>
        OTDRIVER

        <EnumMember>
        OTWITNESS

        <EnumMember>
        OTREPAIRER

        <EnumMember>
        OTTHIRD

        <EnumMember>
        FP

        <EnumMember>
        AGG

        <EnumMember>
        OTSUPPLIER

        <EnumMember>
        OTLOSS

        <EnumMember>
        OTSOL

        <EnumMember>
        OTDOCTOR

        <EnumMember>
        OTSURVEYOR

        <EnumMember>
        HC
    End Enum

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseFindControlSearchRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseFindControlSearchRequestType))>
    Partial Public Class BaseFindControlSearchRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property FindControlKey As Integer = 0


        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SearchCriteria As System.Collections.Generic.List(Of BaseSearchCriteriaType)

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseSearchCriteriaType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseSearchCriteriaType))>
    Partial Public Class BaseSearchCriteriaType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ObjectName As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PropertyName As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Value As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
        System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
        System.Runtime.Serialization.DataContractAttribute(Name:="BaseDeleteRiskRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseDeleteRiskRequestType))>
    Partial Public Class BaseDeleteRiskRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFolderKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFileKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RiskKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property QuoteTimeStamp As Byte()

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
        System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
        System.Runtime.Serialization.DataContractAttribute(Name:="BaseCreateEventRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseCreateEventRequestType))>
    Partial Public Class BaseCreateEventRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PartyKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFileKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFileKeySpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFolderKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFolderKeySpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ClaimKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ClaimKeySpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property EventText As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property EventNote As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AttachedFile As Byte()

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Filename As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
        System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
        System.Runtime.Serialization.DataContractAttribute(Name:="BaseChangePasswordRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseChangePasswordRequestType))>
    Partial Public Class BaseChangePasswordRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Password As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property NewPassword As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseBindQuoteRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseBindQuoteRequestType))>
    Partial Public Class BaseBindQuoteRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFileKey As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PaymentMethod() As PaymentMethodType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PaymentMethodSpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SelectedInstalmentQuote As BaseSelectedInstalmentQuoteType

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="PaymentMethodType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(PaymentMethodType))>
    Public Enum PaymentMethodType

        <EnumMember>
        None

        <EnumMember>
        BankersDraft

        <EnumMember>
        Cash

        <EnumMember>
        Cheque

        <EnumMember>
        CreditCard

        <EnumMember>
        DebitCard

        <EnumMember>
        AgentCollection
    End Enum

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseSelectedInstalmentQuoteType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseSelectedInstalmentQuoteType))>
    Partial Public Class BaseSelectedInstalmentQuoteType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SelectedSchemeNo As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SelectedSchemeVersion As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property QuoteDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property StartDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property EndDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PreferredDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property MonthDay As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property WeekDay As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AmountToFinance As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PaymentProtection As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property OverrideRate As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property OverrideInterestRate As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AmountPaid As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property BankName As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property BankSortCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property BankAccountNo As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property BankAccountName As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property BankBranch As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property BankAddress As BaseAddressType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property BankAreaCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property BankPhone As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property BankExtn As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property BankFaxCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property BankFax As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PFRF_ID As Integer = 0

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseAddPartyRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddPartyRequestType))>
    Partial Public Class BaseAddPartyRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Item As BasePartyType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SubBranchCode As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseClaimProcessRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseClaimProcessRequestType))>
    Partial Public Class BaseClaimProcessRequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Claim As BaseClaimProcessType

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseClaimProcessType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseClaimProcessType))>
    Partial Public Class BaseClaimProcessType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Description As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ProgressStatusCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PrimaryCauseCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property LossFromDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ReportedDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property HandlerCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InfoOnly As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property LikelyClaim As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SecondaryCauseCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CatastropheCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property LossToDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property LossToDateSpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Comments As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ClaimVersionDescription As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ClaimVersion As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ClaimStatus As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ClaimStatusDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property LastModifiedDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ClientName As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ClientFaxNo As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ClientMobileNo As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ClientEmail As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ClientTelNo As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property BaseClaimKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ClaimPeril As System.Collections.Generic.List(Of BaseClaimProcessPerilType)

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFileKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RiskKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CurrencyCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property UnderwritingYearCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property IgnoreWarnings As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ExternalHandler As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ClaimBuilderDetail As System.Collections.Generic.List(Of BaseClaimProcessBuilderRiskType)

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ClaimNumber As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFileRef As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CloseClaimOnZeroReserveRecoveryBalance As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CloseClaimOnZeroReserveRecoveryBalanceSpecified As Boolean = False

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseClaimProcessPerilType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseClaimProcessPerilMaintainType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseClaimProcessPerilType))>
    Partial Public Class BaseClaimProcessPerilType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TypeCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Description As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Reserve As System.Collections.Generic.List(Of BaseClaimProcessPerilReserveType)

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Recovery As System.Collections.Generic.List(Of BaseClaimProcessPerilRecoveryType)

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseClaimProcessPerilReserveType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseClaimProcessPerilReserveType))>
    Partial Public Class BaseClaimProcessPerilReserveType

        Public Sub New()
            MyBase.New()
        End Sub

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TypeCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Amount As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TaxGroupCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PaymentAmount As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PaymentAmountSpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PaymentDetails As BaseClaimProcessPaymentDetailsType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ReverseExcess As Boolean = False

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
        System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
        System.Runtime.Serialization.DataContractAttribute(Name:="BaseClaimProcessPaymentDetailsType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseClaimProcessPaymentDetailsType))>
    Partial Public Class BaseClaimProcessPaymentDetailsType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PaymentMediaTypeCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PaymentMediaReference As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PaymentPayee As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PaymentBankCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property UltimatePayee As String = ""
    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
        System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
        System.Runtime.Serialization.DataContractAttribute(Name:="BaseClaimProcessPaymentDetailsType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseClaimProcessPaymentDetailsType))>
    Partial Public Class BaseClaimProcessReceiptDetailsType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ReceiptMediaTypeCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ReceiptMediaReference As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ReceiptPayee As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ReceiptBankCode As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
        System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
        System.Runtime.Serialization.DataContractAttribute(Name:="BaseClaimProcessPerilRecoveryType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseClaimProcessPerilRecoveryType))>
    Partial Public Class BaseClaimProcessPerilRecoveryType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TypeCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Amount As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property taxGroupCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RecoveryAmount As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RecoveryAmountSpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RecoveryDetails As BaseClaimProcessReceiptDetailsType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RecoveryPartyTypeCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RecoveryPartyCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property IsSalvageRecovery As Boolean = False

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
        System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
        System.Runtime.Serialization.DataContractAttribute(Name:="BaseClaimProcessPerilMaintainType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseClaimProcessPerilMaintainType))>
    Partial Public Class BaseClaimProcessPerilMaintainType
        Inherits BaseClaimProcessPerilType
    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseClaimProcessBuilderRiskType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseClaimProcessBuilderRiskType))>
    Partial Public Class BaseClaimProcessBuilderRiskType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ClaimBuilderData As BaseClaimProcessBuilderRiskTypeClaimBuilderData

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseClaimProcessBuilderRiskTypeClaimBuilderData", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseClaimProcessBuilderRiskTypeClaimBuilderData))>
    Partial Public Class BaseClaimProcessBuilderRiskTypeClaimBuilderData

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ItemName As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Value As String = ""

    End Class
    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseAddRiskRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddRiskRequestType))>
    Partial Public Class BaseAddRiskRequestType
        Inherits BaseRiskType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SubBranchCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFolderKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFileKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ProductCode As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseRiskResultType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseRiskResultType))>
    Partial Public Class BaseRiskResultType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RiskFolderID As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RiskID As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property XMLDataSet As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PremiumDueNet As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PremiumDueTax As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PremiumDueGross As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TotalAnnualTax As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CommissionAmount As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyLevelTax As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyLevelTaxSpecified As Boolean = False

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseBranchType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseBranchType))>
    Partial Public Class BaseBranchType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property BranchCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Description As String = ""

    End Class

    '''<remarks/>
    <System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetOnlineClientListResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetHistoricalTransactionsResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetOpenTransactionsResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetClaimsResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateRiskResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdatePartyResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateQuoteResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseTransactResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseBindQuoteResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(NBTransactResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseRunDefaultRulesEditResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseRunDefaultRulesAddResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseNBQuoteResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(PolicyProcessResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(NewBusinessResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseLogoffResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseLoginResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetRiskResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetPartySummaryResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetPartyResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetListResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetInstalmentQuotesResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetHeaderAndSummariesResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetDefaultDatasetResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetDatasetSchemaResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(GetDatasetSchemaResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetDatasetDefinitionResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetAllPolicyVersionsResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetAddressResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetRiskByProductResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetRatingDetailsResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetProductByAgentResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGenerateDocumentResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetCurrenciesByBranchResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseForgottenPasswordResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseFindPartyResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseFindControlSearchResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseDeleteRiskResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseCreateEventResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseChangePasswordResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddRiskResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddQuoteResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddPartyResponseType)),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddAddressResponseType)),
        System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.42"),
        System.SerializableAttribute(),
        System.Diagnostics.DebuggerStepThroughAttribute(),
        System.ComponentModel.DesignerCategoryAttribute("code"),
        System.Runtime.Serialization.DataContractAttribute(Name:="BaseResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseResponseType))>
    Partial Public MustInherit Class BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property STSErrorType() As STSErrorType
    End Class

    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="STSErrorType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(STSErrorType))>
    Partial Public Class STSErrorType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InvalidData As System.Collections.Generic.List(Of STSErrorInvalidDataType)

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property STSBusinessRule As STSErrorSTSBusinessRuleType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SiriusBackOffice As STSErrorSiriusBackOfficeType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InternalException As STSErrorInternalExceptionType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property WebService As String

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property WebMethod As String

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Label As String

    End Class

    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="STSErrorInvalidDataType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(STSErrorInvalidDataType))>
    Partial Public Class STSErrorInvalidDataType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property FieldName As String

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Code As String

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Description As String

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SuppliedValue As String

    End Class

    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="STSErrorSTSBusinessRuleType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(STSErrorSTSBusinessRuleType))>
    Partial Public Class STSErrorSTSBusinessRuleType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Code As String

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Description As String

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Detail As String

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="STSErrorSiriusBackOfficeType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(STSErrorSiriusBackOfficeType))>
    Partial Public Class STSErrorSiriusBackOfficeType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ReturnValue As String

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Description As String

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="STSErrorInternalExceptionType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(STSErrorInternalExceptionType))>
    Partial Public Class STSErrorInternalExceptionType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Description As String

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetOnlineClientListResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetOnlineClientListResponseType))>
    Partial Public Class BaseGetOnlineClientListResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Clients As BaseGetOnlineClientListResponseTypeClients

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetOnlineClientListResponseTypeClients", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetOnlineClientListResponseTypeClients))>
    Partial Public Class BaseGetOnlineClientListResponseTypeClients

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Row As System.Collections.Generic.List(Of BaseGetOnlineClientListResponseTypeClientsRow)

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetOnlineClientListResponseTypeClientsRow", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetOnlineClientListResponseTypeClientsRow))>
    Partial Public Class BaseGetOnlineClientListResponseTypeClientsRow

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PartyKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property EmailAddress As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property OnlineAccess As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ResolvedName As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetHistoricalTransactionsResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetHistoricalTransactionsResponseType))>
    Partial Public Class BaseGetHistoricalTransactionsResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Transactions As BaseGetHistoricalTransactionsResponseTypeTransactions

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetHistoricalTransactionsResponseTypeTransactions", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetHistoricalTransactionsResponseTypeTransactions))>
    Partial Public Class BaseGetHistoricalTransactionsResponseTypeTransactions

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Row As System.Collections.Generic.List(Of BaseGetHistoricalTransactionsResponseTypeTransactionsRow)

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetHistoricalTransactionsResponseTypeTransactionsRow", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetHistoricalTransactionsResponseTypeTransactionsRow))>
    Partial Public Class BaseGetHistoricalTransactionsResponseTypeTransactionsRow

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property DocumentKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TransactionDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property EffectiveDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property DocRef As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Reference As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Amount As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ISO As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PaidDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Reason As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetOpenTransactionsResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetOpenTransactionsResponseType))>
    Partial Public Class BaseGetOpenTransactionsResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Transactions As BaseGetOpenTransactionsResponseTypeTransactions

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetOpenTransactionsResponseTypeTransactions", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetOpenTransactionsResponseTypeTransactions))>
    Partial Public Class BaseGetOpenTransactionsResponseTypeTransactions

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Row As System.Collections.Generic.List(Of BaseGetOpenTransactionsResponseTypeTransactionsRow)

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetOpenTransactionsResponseTypeTransactionsRow", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetOpenTransactionsResponseTypeTransactionsRow))>
    Partial Public Class BaseGetOpenTransactionsResponseTypeTransactionsRow

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property DocumentKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TransactionDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property EffectiveDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property DocRef As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Reference As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Amount As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ISO As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PaidDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property OSAmount As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Reason As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Balance As Double

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetClaimsResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetClaimsResponseType))>
    Partial Public Class BaseGetClaimsResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Claims As BaseGetClaimsResponseTypeClaims

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetClaimsResponseTypeClaims", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetClaimsResponseTypeClaims))>
    Partial Public Class BaseGetClaimsResponseTypeClaims


        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Row As System.Collections.Generic.List(Of BaseGetClaimsResponseTypeClaimsRow)

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetClaimsResponseTypeClaimsRow", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetClaimsResponseTypeClaimsRow))>
    Partial Public Class BaseGetClaimsResponseTypeClaimsRow

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ClaimKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ClaimNumber As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyNumber As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ClaimStatus As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Regarding As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Handler As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PrimaryCause As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property LossDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ReportedDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Insurer As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseUpdateRiskResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateRiskResponseType))>
    Partial Public Class BaseUpdateRiskResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PremiumDueNet As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PremiumDueTax As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PremiumDueGross As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TotalAnnualTax As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CommissionAmount As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property XMLDataSet As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property QuoteTimeStamp As Byte()

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyLevelTax As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyLevelTaxSpecified As Boolean = False

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseUpdatePartyResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdatePartyResponseType))>
    Partial Public Class BaseUpdatePartyResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PartyTimestamp As Byte()

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseUpdateQuoteResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseUpdateQuoteResponseType))>
    Partial Public Class BaseUpdateQuoteResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property QuoteTimeStamp As Byte()

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseTransactResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseBindQuoteResponseType)),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(NBTransactResponseType)),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseTransactResponseType))>
    Partial Public Class BaseTransactResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Policy As BaseTransactResponseTypePolicy

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseTransactResponseTypePolicy", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseTransactResponseTypePolicy))>
    Partial Public Class BaseTransactResponseTypePolicy

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyRef As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PremiumDueNet As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PremiumDueTax As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PremiumDueGross As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TotalAnnualTax As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CommissionAmount As Decimal

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseBindQuoteResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseBindQuoteResponseType))>
    Partial Public Class BaseBindQuoteResponseType
        Inherits BaseTransactResponseType
    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseRunDefaultRulesEditResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseRunDefaultRulesEditResponseType))>
    Partial Public Class BaseRunDefaultRulesEditResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property XMLDataSet As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseRunDefaultRulesAddResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseRunDefaultRulesAddResponseType))>
    Partial Public Class BaseRunDefaultRulesAddResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property XMLDataSet As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseNBQuoteResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(PolicyProcessResponseType)),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(NewBusinessResponseType)),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseNBQuoteResponseType))>
    Partial Public Class BaseNBQuoteResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Insured As BaseAddPartyResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Policy As BaseNBQuoteResponseTypePolicy

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseAddPartyResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddPartyResponseType))>
    Partial Public Class BaseAddPartyResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PartyKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Shortname As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PartyTimestamp As Byte()

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ResolvedName As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseNBQuoteResponseTypePolicy", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseNBQuoteResponseTypePolicy))>
    Partial Public Class BaseNBQuoteResponseTypePolicy

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyID As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property QuoteRef As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Risks As System.Collections.Generic.List(Of BaseRiskResultType)

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PremiumDueNet As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PremiumDueTax As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PremiumDueGross As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TotalAnnualTax As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyLevelTax As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyLevelTaxSpecified As Boolean = False

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseLogoffResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseLogoffResponseType))>
    Partial Public Class BaseLogoffResponseType
        Inherits BaseResponseType
    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseLoginResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseLoginResponseType))>
    Partial Public Class BaseLoginResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PasswordChangeDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property LastLogin As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property FullUsername As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property EmailAddress As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SourceList As System.Collections.Generic.List(Of BaseBranchType)

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ConsolidatedAgentCommission As Boolean = False

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetRiskResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetRiskResponseType))>
    Partial Public Class BaseGetRiskResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property XMLDataSet As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PremiumDueNet As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PremiumDueTax As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PremiumDueGross As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TotalAnnualTax As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CommissionAmount As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property QuoteTimeStamp As Byte()

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyLevelTax As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyLevelTaxSpecified As Boolean = False

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetPartySummaryResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetPartySummaryResponseType))>
    Partial Public Class BaseGetPartySummaryResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Item As BasePartyType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Policies As BaseGetPartySummaryResponseTypePolicies

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PartyTimestamp As Byte()

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetPartySummaryResponseTypePolicies", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetPartySummaryResponseTypePolicies))>
    Partial Public Class BaseGetPartySummaryResponseTypePolicies

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Row As System.Collections.Generic.List(Of BaseGetPartySummaryResponseTypePoliciesRow)

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetPartySummaryResponseTypePoliciesRow", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetPartySummaryResponseTypePoliciesRow))>
    Partial Public Class BaseGetPartySummaryResponseTypePoliciesRow

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFileId As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property BranchKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property BranchCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFileKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyRef As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFolderKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyTypeId As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyTypeIdSpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property LeadInsurerKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property LeadInsurerKeySpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property DateIssued As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property DateIssuedSpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CoverStartDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CoverStartDateSpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ExpiryDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ExpiryDateSpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RenewalDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RenewalDateSpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuredKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuredKeySpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ProductKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property LeadAgentKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property LeadAgentKeySpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ThisPremium As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ThisPremiumSpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AnnualPremium As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AnnualPremiumSpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property NetPremium As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property NetPremiumSpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TaxAmount As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TaxAmountSpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property GeminiPolicyStatus As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property GeminiPolicyStatusSpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PartyShortname As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ProductCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ProductDesc As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFileTypeCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyStatusCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsurerShortName As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AgentShortName As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyTypeCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyTypeDesc As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CurrencyCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Regarding As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetPartyResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetPartyResponseType))>
    Partial Public Class BaseGetPartyResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Item As BasePartyType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PartyTimestamp As Byte()

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetListResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetListResponseType))>
    Partial Public Class BaseGetListResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property List As BaseGetListResponseTypeList

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetListResponseTypeList", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetListResponseTypeList))>
    Partial Public Class BaseGetListResponseTypeList

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Row As System.Collections.Generic.List(Of BaseGetListResponseTypeListRow)

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
        System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
        System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetListResponseTypeListRow", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetListResponseTypeListRow))>
    Partial Public Class BaseGetListResponseTypeListRow

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Key As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Description As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Code As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property EffectiveDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property IsDeleted As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ParentKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ParentKeySpecified As Boolean = False

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
        System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
        System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetInstalmentQuotesResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetInstalmentQuotesResponseType))>
    Partial Public Class BaseGetInstalmentQuotesResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Quotes As BaseGetInstalmentQuotesResponseTypeQuotes

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
        System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
        System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetInstalmentQuotesResponseTypeQuotes", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
        System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetInstalmentQuotesResponseTypeQuotes))>
    Partial Public Class BaseGetInstalmentQuotesResponseTypeQuotes

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Row As System.Collections.Generic.List(Of BaseGetInstalmentQuotesResponseTypeQuotesRow)

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetInstalmentQuotesResponseTypeQuotesRow", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetInstalmentQuotesResponseTypeQuotesRow))>
    Partial Public Class BaseGetInstalmentQuotesResponseTypeQuotesRow

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CompanyNo As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CompanyName As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SchemeNo As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SchemeVersion As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SchemeName As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property FrequencyID As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property FrequencyDescription As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property MediaTypeID As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property MediaTypeDescription As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ProductClass As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ProductCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TotalAmountInput As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InstalmentsToPay As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property FirstInstalmentDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property NextInstalmentDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property LastInstalmentDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property FirstInstalmentAmount As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property OtherInstalmentAmount As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TotalInstalmentsAmount As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AprRate As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InterestRate As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property DaysDelay As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property DepositAmount As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InterestAmount As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TaxAmount As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property FinanceCharge As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ProtectionAmount As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property OriginalOtherInstalmentAmount As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property HighlightCell As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SchemeTypeCode As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property MediaTypeValidation As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property FrequencyPerYear As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PFRF_ID As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property FrequencyPeriod As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property FrequencyAmount As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property OriginalAmount As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ClaimDebtID As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property UserID As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AgentCnt As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AgentRef As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property LastInstalmentAmount As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Username As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Password As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property BrokerID As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property BrokerURL As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Timeout As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ProviderCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Terms As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Ref As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property OriginalRate As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RefundType As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property MinMTA As Integer = 0

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetHeaderAndSummariesResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetHeaderAndSummariesResponseType))>
    Partial Public Class BaseGetHeaderAndSummariesResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PartyKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CoverStartDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CoverEndDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Description As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ProductCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFolderKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFileKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFileRef As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property QuoteExpiryDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property QuoteTimeStamp As Byte()

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property QuoteIsLocked As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Risks As BaseGetHeaderAndSummariesResponseTypeRisks

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SubBranchCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ConsolidatedLeadAgentCommission As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ConsolidatedSubAgentCommission As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CurrencyCode As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetHeaderAndSummariesResponseTypeRisks", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetHeaderAndSummariesResponseTypeRisks))>
    Partial Public Class BaseGetHeaderAndSummariesResponseTypeRisks

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Row As System.Collections.Generic.List(Of BaseGetHeaderAndSummariesResponseTypeRisksRow)

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetHeaderAndSummariesResponseTypeRisksRow", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetHeaderAndSummariesResponseTypeRisksRow))>
    Partial Public Class BaseGetHeaderAndSummariesResponseTypeRisksRow

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RiskKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RiskFolderKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RiskTypeCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Description As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TotalSumInsured As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TotalSumInsuredSpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Premium As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PremiumSpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property StatusCode As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetDefaultDatasetResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetDefaultDatasetResponseType))>
    Partial Public Class BaseGetDefaultDatasetResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property XMLDataSet As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetDatasetSchemaResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(GetDatasetSchemaResponseType)),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetDatasetSchemaResponseType))>
    Partial Public Class BaseGetDatasetSchemaResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property DatasetSchema As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetDatasetDefinitionResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetDatasetDefinitionResponseType))>
    Partial Public Class BaseGetDatasetDefinitionResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property XMLDatasetDefinition As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetAllPolicyVersionsResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetAllPolicyVersionsResponseType))>
    Partial Public Class BaseGetAllPolicyVersionsResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Policies As BaseGetAllPolicyVersionsResponseTypePolicies

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetAllPolicyVersionsResponseTypePolicies", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetAllPolicyVersionsResponseTypePolicies))>
    Partial Public Class BaseGetAllPolicyVersionsResponseTypePolicies

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Row As System.Collections.Generic.List(Of BaseGetAllPolicyVersionsResponseTypePoliciesRow)

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetAllPolicyVersionsResponseTypePoliciesRow", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetAllPolicyVersionsResponseTypePoliciesRow))>
    Partial Public Class BaseGetAllPolicyVersionsResponseTypePoliciesRow

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFolderKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property insuranceFileKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceHolderKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyTypeCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyRef As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFileTypeDesc As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ProductDesc As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RenewalDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PartyShortName As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Premium As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PremiumSpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFileTypeCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFileTypeKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CoverStartDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ExpiryDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property QuoteExpiryDate As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property QuoteExpiryDateSpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property EventDesc As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TaxAmount As Double

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TaxAmountSpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property GracePeriod As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ProductCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyVersion As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PaymentMethod As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetAddressResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetAddressResponseType))>
    Partial Public Class BaseGetAddressResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Address As BaseAddressType

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetRiskByProductResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetRiskByProductResponseType))>
    Partial Public Class BaseGetRiskByProductResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Risks As BaseGetRiskByProductResponseTypeRisks

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetRiskByProductResponseTypeRisks", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetRiskByProductResponseTypeRisks))>
    Partial Public Class BaseGetRiskByProductResponseTypeRisks

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Row As System.Collections.Generic.List(Of BaseGetRiskByProductResponseTypeRisksRow)

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetRiskByProductResponseTypeRisksRow", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetRiskByProductResponseTypeRisksRow))>
    Partial Public Class BaseGetRiskByProductResponseTypeRisksRow

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RiskTypeKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RiskTypeCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Description As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ScreenKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property DataModelCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ScreenCode As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetRatingDetailsResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetRatingDetailsResponseType))>
    Partial Public Class BaseGetRatingDetailsResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RatingDetails As BaseGetRatingDetailsResponseTypeRatingDetails

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetRatingDetailsResponseTypeRatingDetails", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetRatingDetailsResponseTypeRatingDetails))>
    Partial Public Class BaseGetRatingDetailsResponseTypeRatingDetails

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Row As System.Collections.Generic.List(Of BaseGetRatingDetailsResponseTypeRatingDetailsRow)

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetRatingDetailsResponseTypeRatingDetailsRow", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetRatingDetailsResponseTypeRatingDetailsRow))>
    Partial Public Class BaseGetRatingDetailsResponseTypeRatingDetailsRow

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RatingSectionType As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicySectionType As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RateType As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AnnualRate As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SumInsured As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ThisPremium As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AnnualPremium As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Country As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property State As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RatingSectionId As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RatingSectionTypeId As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicySectionTypeId As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RateTypeId As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property OriginalFlag As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CurrencyId As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CountryId As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property StateId As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property IsAmended As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CalculatedPremium As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property OverrideReason As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetProductByAgentResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetProductByAgentResponseType))>
    Partial Public Class BaseGetProductByAgentResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Products As BaseGetProductByAgentResponseTypeProducts

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetProductByAgentResponseTypeProducts", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetProductByAgentResponseTypeProducts))>
    Partial Public Class BaseGetProductByAgentResponseTypeProducts
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Row As System.Collections.Generic.List(Of BaseGetProductByAgentResponseTypeProductsRow)

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetProductByAgentResponseTypeProductsRow", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetProductByAgentResponseTypeProductsRow))>
    Partial Public Class BaseGetProductByAgentResponseTypeProductsRow

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ProductKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ProductCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Description As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SchemeAgencyRef As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property BlockNumber As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ConsolidatedLeadAgentCommission As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ConsolidatedSubAgentCommission As Boolean = False

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGenerateDocumentResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGenerateDocumentResponseType))>
    Partial Public Class BaseGenerateDocumentResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SpooledZipFile As Byte()

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property MergedFilePath As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetCurrenciesByBranchResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetCurrenciesByBranchResponseType))>
    Partial Public Class BaseGetCurrenciesByBranchResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Currencies As BaseGetCurrenciesByBranchResponseTypeCurrencies

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetCurrenciesByBranchResponseTypeCurrencies", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetCurrenciesByBranchResponseTypeCurrencies))>
    Partial Public Class BaseGetCurrenciesByBranchResponseTypeCurrencies
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Row As System.Collections.Generic.List(Of BaseGetCurrenciesByBranchResponseTypeCurrenciesRow)

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseGetCurrenciesByBranchResponseTypeCurrenciesRow", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGetCurrenciesByBranchResponseTypeCurrenciesRow))>
    Partial Public Class BaseGetCurrenciesByBranchResponseTypeCurrenciesRow

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CurrencyCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Description As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseForgottenPasswordResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseForgottenPasswordResponseType))>
    Partial Public Class BaseForgottenPasswordResponseType
        Inherits BaseResponseType
    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseFindPartyResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseFindPartyResponseType))>
    Partial Public Class BaseFindPartyResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Parties As BaseFindPartyResponseTypeParties

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseFindPartyResponseTypeParties", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseFindPartyResponseTypeParties))>
    Partial Public Class BaseFindPartyResponseTypeParties

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Row As System.Collections.Generic.List(Of BaseFindPartyResponseTypePartiesRow)

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseFindPartyResponseTypePartiesRow", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseFindPartyResponseTypePartiesRow))>
    Partial Public Class BaseFindPartyResponseTypePartiesRow

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PartyKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ShortName As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ResolvedName As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AddressLine1 As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PostCode As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ContactTelephoneNumber As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property DateOfBirth As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property DateOfBirthSpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AgentKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AgentKeySpecified As Boolean = False

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseFindControlSearchResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseFindControlSearchResponseType))>
    Partial Public Class BaseFindControlSearchResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Matches() As System.Xml.XmlElement

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseDeleteRiskResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseDeleteRiskResponseType))>
    Partial Public Class BaseDeleteRiskResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property QuoteTimeStamp As Byte()

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseCreateEventResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseCreateEventResponseType))>
    Partial Public Class BaseCreateEventResponseType
        Inherits BaseResponseType
    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseChangePasswordResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseChangePasswordResponseType))>
    Partial Public Class BaseChangePasswordResponseType
        Inherits BaseResponseType
    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseAddRiskResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddRiskResponseType))>
    Partial Public Class BaseAddRiskResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RiskFolderKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RiskKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property XMLDataSet As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property QuoteTimeStamp As Byte()

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseAddQuoteResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddQuoteResponseType))>
    Partial Public Class BaseAddQuoteResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFolderKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFileKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFileRef As String

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property QuoteTimeStamp As Byte()

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property QuoteExpiryDate As Date

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseAddAddressResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddAddressResponseType))>
    Partial Public Class BaseAddAddressResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AddressKey As Integer = 0

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseClaimProcessResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseClaimProcessResponseType))>
    Partial Public Class BaseClaimProcessResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ClaimKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property BaseClaimKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ClaimNumber As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Version As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TimeStamp As Byte()

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ResultingStatus As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Warnings As System.Collections.Generic.List(Of BaseClaimProcessResponseTypeWarnings)

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseClaimProcessResponseTypeWarnings", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseClaimProcessResponseTypeWarnings))>
    Partial Public Class BaseClaimProcessResponseTypeWarnings

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Code As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Description As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseContactDetailType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseContactDetailType))>
    Partial Public Class BaseContactDetailType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Item As String = ""

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ItemElementName() As ItemChoiceType

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="ItemChoiceType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(ItemChoiceType))>
    Public Enum ItemChoiceType

        <EnumMember>
        EmailAddress

        <EnumMember>
        Number
    End Enum

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseContactType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseContactType))>
    Partial Public Class BaseContactType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ContactTypeCode() As ContactTypeType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ContactDetail As BaseContactDetailType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AreaCode As String = ""

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="ContactTypeType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(ContactTypeType))>
    Public Enum ContactTypeType

        <EnumMember>
        EMAIL

        <EnumMember>
        HOMEPHONE

        <EnumMember>
        MOBILE

        <EnumMember>
        FAX

        <EnumMember>
        WEB

        <EnumMember>
        MAIN

        <EnumMember>
        OTHER

        <EnumMember>
        MAINEMAILCONTACT

    End Enum

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseAddAddressRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAddAddressRequestType))>
    Partial Public Class BaseAddAddressRequestType
        Inherits BaseAddressType
    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="NewBusinessResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(NewBusinessResponseType))>
    Partial Public Class NewBusinessResponseType
        Inherits BaseNBQuoteResponseType
    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="NBTransactRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(NBTransactRequestType))>
    Partial Public Class NBTransactRequestType
        Inherits BaseTransactRequestType
    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="NBTransactResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(NBTransactResponseType))>
    Partial Public Class NBTransactResponseType
        Inherits BaseTransactResponseType
    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="PolicyProcessRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(PolicyProcessRequestType))>
    Partial Public Class PolicyProcessRequestType
        Inherits BaseNBQuoteRequestType
    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="PolicyProcessResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(PolicyProcessResponseType))>
    Partial Public Class PolicyProcessResponseType
        Inherits BaseNBQuoteResponseType
    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="GetDatasetSchemaRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(GetDatasetSchemaRequestType))>
    Partial Public Class GetDatasetSchemaRequestType
        Inherits BaseGetDatasetSchemaRequestType
    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="GetDatasetSchemaResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(GetDatasetSchemaResponseType))>
    Partial Public Class GetDatasetSchemaResponseType
        Inherits BaseGetDatasetSchemaResponseType
    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="PolicyProcessV2ResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(PolicyProcessV2ResponseType))>
    Partial Public Class PolicyProcessV2ResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Insured As BasePolicyProcessV2ResponseTypeInsured

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Policy As BasePolicyProcessV2ResponseTypePolicy


    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BasePolicyProcessV2ResponseTypeInsured", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(PolicyProcessV2ResponseType)),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BasePolicyProcessV2ResponseTypeInsured))>
    Partial Public Class BasePolicyProcessV2ResponseTypeInsured
        Inherits BasePartyV2ResponseType
    End Class

    <System.Diagnostics.DebuggerStepThroughAttribute(),
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
     System.Runtime.Serialization.DataContractAttribute(Name:="BasePartyV2ResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BasePartyV2ResponseType))>
    Partial Public Class BasePartyV2ResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PartyKey() As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ShortName() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ResolvedName() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AddressLine1() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AddressLine2() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PostCode() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property FileCode() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ContactTelephoneNumber() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Type() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Status() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property DateOfBirth() As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AgentKey() As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SwiftLink() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AgentType() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PartySourceId() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PartySourceDescription() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ReinsuranceType() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AllowConsolidatedCommission() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property IsProspect() As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property IsRIBroker() As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Name() As String = String.Empty

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BasePolicyProcessV2ResponseTypePolicy", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BasePolicyProcessV2ResponseTypePolicy))>
    Partial Public Class BasePolicyProcessV2ResponseTypePolicy
        Inherits BasePolicyProcessV2ResponseTypePolicyDetails
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Risks As System.Collections.Generic.List(Of BasePolicyProcessV2ResponseTypeRisks)

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Commission As BasePolicyProcessV2ResponseTypeCommission

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Taxes As System.Collections.Generic.List(Of BasePolicyProcessV2ResponseTypePolicyTaxes)

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Fees As System.Collections.Generic.List(Of BasePolicyProcessV2ResponseTypePolicyFees)
    End Class

    <System.Diagnostics.DebuggerStepThroughAttribute(),
System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
System.Runtime.Serialization.DataContractAttribute(Name:="BasePolicyProcessV2ResponseTypePolicyDetails", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
   System.Runtime.Serialization.KnownTypeAttribute(GetType(BasePolicyProcessV2ResponseTypePolicyDetails))>
    Partial Public Class BasePolicyProcessV2ResponseTypePolicyDetails
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property HasClaimLink As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PartyKey() As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CoverStartDate() As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CoverEndDate() As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Description() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ProductCode() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFolderKey() As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFileKey() As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFileRef() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property QuoteExpiryDate() As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property QuoteTimeStamp() As Byte()

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property QuoteIsLocked() As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SubBranchCode() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ConsolidatedLeadAgentCommission() As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ConsolidatedSubAgentCommission() As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CurrencyCode() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ThisPremium() As Double = 0.0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ThisPremiumSpecified() As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AnnualPremium() As Double = 0.0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AnnualPremiumSpecified() As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property NetPremium() As Double = 0.0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property NetPremiumSpecified() As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TaxAmount() As Double = 0.0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TaxAmountSpecified() As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyLevelTaxesAndFees() As BaseTaxesAndFeesType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AlternativeRef() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuredName() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ProductName() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property BranchCode() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Regarding() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyStatusCode() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AnalysisCode() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property BusinessTypeCode() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InceptionDt() As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RenewalDate() As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyTypeCode() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InceptionTPI() As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property IssueDate() As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property IssueDateSpecified() As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ProposalDate() As Date

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ProposalDateSpecified() As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RenewalFrequencyCode() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property LTUExpiryDate() As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property LTUExpiryDateSpecified() As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property StopReasonCode() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RenewedCount() As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RenewalMethodCode() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property LapsedReasonCode() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property LapseDate() As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property LapseDateSpecified() As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ReferredAtRenewal() As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ReferredOnMTA() As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property StandardPolicyWordingCode() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property StandardPolicyDescription() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyStyleCode() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AccountHandlerCode() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AccountHandler() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AccountHandlerCnt() As Integer

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AccountHandlerCntSpecified() As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RenewalStatusTypeCode() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RenewalStatusTypeDesc() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property LeadAgentKey() As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property LeadAgentCode() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property LeadAgent() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property HCExpiryDate() As Date

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyDeductible() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyLimits() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property UnderwritingYear() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property MarkedForCollection() As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property BaseInsuranceFolderKey() As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property QuoteStatusKey() As QuoteStatusType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property QuoteVersion() As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ContactuserKey() As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ContactUserName() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property IsDeletedContactuser() As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ContactUserFullName() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ContactUserEmail() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PutOnNextMTAInstallmentRenewal() As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AnniversaryCopy() As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RenewalDayNo() As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property UnderwritingYearId() As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property IsValidAnniversaryToAccept As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property GrossPremium() As Double = 0.0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TotalCommission() As Double = 0.0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TotalFees() As Double = 0.0
    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BasePolicyProcessV2ResponseTypeRisks", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BasePolicyProcessV2ResponseTypeRisks))>
    Partial Public Class BasePolicyProcessV2ResponseTypeRisks
        Inherits BaseRiskResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Taxes As System.Collections.Generic.List(Of BasePolicyProcessV2ResponseTypeRiskTaxes)

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Fees As System.Collections.Generic.List(Of BasePolicyProcessV2ResponseTypeRiskFees)

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RatingSections As System.Collections.Generic.List(Of BasePolicyProcessV2ResponseTypeRatingSections)
    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BasePolicyProcessV2ResponseTypeRiskTaxes", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BasePolicyProcessV2ResponseTypeRiskTaxes))>
    Partial Public Class BasePolicyProcessV2ResponseTypeRiskTaxes
        Inherits BaseTaxesResponseType
    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BasePolicyProcessV2ResponseTypeRiskFees", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BasePolicyProcessV2ResponseTypeRiskFees))>
    Partial Public Class BasePolicyProcessV2ResponseTypeRiskFees
        Inherits BaseFeesResponseType
    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BasePolicyProcessV2ResponseTypeRatingSections", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BasePolicyProcessV2ResponseTypeRatingSections))>
    Partial Public Class BasePolicyProcessV2ResponseTypeRatingSections
        Inherits BaseRatingDetailsResponseType
    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BasePolicyProcessV2ResponseTypeCommission", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BasePolicyProcessV2ResponseTypeCommission))>
    Partial Public Class BasePolicyProcessV2ResponseTypeCommission
        Inherits BaseAgentCommissionResponseType
    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BasePolicyProcessV2ResponseTypePolicyTaxes", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BasePolicyProcessV2ResponseTypePolicyTaxes))>
    Partial Public Class BasePolicyProcessV2ResponseTypePolicyTaxes
        Inherits BaseTaxesResponseType
    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BasePolicyProcessV2ResponseTypePolicyFees", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BasePolicyProcessV2ResponseTypePolicyFees))>
    Partial Public Class BasePolicyProcessV2ResponseTypePolicyFees
        Inherits BaseFeesResponseType
    End Class

    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseTaxesResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseTaxesResponseType))>
    Partial Public Class BaseTaxesResponseType

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TaxCalculationKey() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RiskKey() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TaxBandKey() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Premium() As Decimal = CDec(0.0)

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property IsValue() As Boolean = False

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TaxPercentage() As Decimal = CDec(0.0)

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TaxValue() As Decimal = CDec(0.0)

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property IsManuallyChanged() As Boolean = False

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CalculationBasis() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property BasisValue() As Decimal = CDec(0.0)

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SumInsured() As Decimal = CDec(0.0)

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SumInsuredRounded() As Decimal = CDec(0.0)

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CurrencyKey() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AllowTaxCredit() As Byte

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property OriginalSumInsured() As Decimal = CDec(0.0)

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CountryKey() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property StateKey() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ClassOfBusinessKey() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TaxGroupKey() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Sequence() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyFeeUKey() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AgentCommissionKey() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RIPartyKey() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RIArrangementLineKey() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceSectionKey() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyFeeKey() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyAgentsKey() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsurerPartyKey() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ClaimPerilKey() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ClaimPaymentKey() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ClaimReceiptKey() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ClaimPaymentItemKey() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ClaimReceiptItemKey() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property IsNotAppliedToClient() As Boolean = False

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property IncludeTaxInInstalments() As Boolean = False

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SpreadTaxAcrossInstalments() As Boolean = False

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property BaseTaxCalculationKey() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property VersionKey() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PfPremFinanceKey() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PfPremFinanceVersion() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyCoinsurersSectionKey() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property IsCommissionTax() As Boolean = False

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ApplyTaxBy() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TaxBandRateKey() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property IsSuspended() As Boolean = False

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TransType() As String = String.Empty

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TaxBandCode() As String = String.Empty

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TaxBandDescription() As String = String.Empty

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TaxGroupCode() As String = String.Empty

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TaxGroupDescription() As String = String.Empty

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TaxBandRateDescription() As String = String.Empty

    End Class

    <System.Diagnostics.DebuggerStepThroughAttribute(),
System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
System.Runtime.Serialization.DataContractAttribute(Name:="BaseFeesResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseFeesResponseType))>
    Partial Public Class BaseFeesResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property FeeName() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CurrencyCode() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AppliedTo() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Premium() As Double = 0.0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Rate() As Double = 0.0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property FeeAmount() As Double = 0.0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TaxAmount() As Double = 0.0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TotalAmount() As Double = 0.0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TaxGroup() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property IncludeInInstallment() As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SpreadAcrossInstallment() As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property IsValue() As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyFeeKey() As Integer = 0

    End Class

    <System.Diagnostics.DebuggerStepThroughAttribute(),
System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
System.Runtime.Serialization.DataContractAttribute(Name:="BaseAgentCommissionResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAgentCommissionResponseType))>
    Partial Public Class BaseAgentCommissionResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property LeadAgentTotalCommission() As Double = 0.0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property LeadAgentTotalTax() As Double = 0.0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property LeadAgentNet() As Double = 0.0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SubAgentTotalCommission() As Double = 0.0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SubAgentTotalTax() As Double = 0.0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SubAgentNet() As Double = 0.0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AgentCommission() As System.Collections.Generic.List(Of BaseAgentCommissionResponseTypeRow)

        Public Overridable Function ShouldSerializeAgentCommission() As Boolean
            Return ((Not (Me.AgentCommission) Is Nothing) _
                        AndAlso (Me.AgentCommission.Count > 0))
        End Function
    End Class

    <System.Diagnostics.DebuggerStepThroughAttribute(),
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
     System.Runtime.Serialization.DataContractAttribute(Name:="BaseAgentCommissionResponseTypeRow", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
     System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseAgentCommissionResponseTypeRow))>
    Partial Public Class BaseAgentCommissionResponseTypeRow

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Agent() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AgentType() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RiskType() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CommissionBand() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Premium() As Double = 0.0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CommissionRate() As Double = 0.0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CommissionValue() As Double = 0.0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property IsLeadAgent() As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TaxGroup() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TaxValue() As Double = 0.0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property MaximumRate() As Double = 0.0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property IsValue() As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TaxGroupDescription() As String = String.Empty

    End Class

    <System.Diagnostics.DebuggerStepThroughAttribute(),
System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
System.Runtime.Serialization.DataContractAttribute(Name:="BaseRatingDetailsResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseRatingDetailsResponseType))>
    Partial Public Class BaseRatingDetailsResponseType

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RatingSectionType() As String = String.Empty

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicySectionType() As String = String.Empty

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RateType() As String = String.Empty

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AnnualRate() As Decimal = CDec(0.0)

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SumInsured() As Decimal = CDec(0.0)

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ThisPremium() As Decimal = CDec(0.0)

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AnnualPremium() As Decimal = CDec(0.0)

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Country() As String = String.Empty

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property State() As String = String.Empty

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RatingSectionId() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RatingSectionTypeId() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicySectionTypeId() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RateTypeId() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property OriginalFlag() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CurrencyId() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CountryId() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property StateId() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property IsAmended() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CalculatedPremium() As Decimal = CDec(0.0)

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property OverrideReason() As String = String.Empty

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property EarningPattern() As String = String.Empty

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property EarningPatternId() As Integer = 0

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property StateCode() As String = String.Empty

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CountryCode() As String = String.Empty

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RatingTypeCode() As String = String.Empty

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RatingSectionTypeCode() As String = String.Empty

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property EarningPatternCode() As String = String.Empty

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CurrencyCode() As String = String.Empty

    End Class

    <System.Diagnostics.DebuggerStepThroughAttribute(),
System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
System.Runtime.Serialization.DataContractAttribute(Name:="BaseRiskResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseRiskResponseType))>
    Partial Public Class BaseRiskResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property XMLDataSet() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PremiumDueNet() As Decimal = CDec(0.0)

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PremiumDueTax() As Decimal = CDec(0.0)

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PremiumDueGross() As Decimal = CDec(0.0)

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property TotalAnnualTax() As Decimal = CDec(0.0)

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CommissionAmount() As Decimal = CDec(0.0)

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property proRataRate() As Decimal

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property QuoteTimeStamp() As Byte()

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyLevelTax() As Decimal = CDec(0.0)

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyLevelTaxesAndFees() As BaseTaxesAndFeesType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RiskLevelTaxesAndFees() As BaseTaxesAndFeesType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RiskStatus() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property RiskFolderKey() As Integer = 0
    End Class

    <System.Diagnostics.DebuggerStepThroughAttribute(),
   System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
   System.Runtime.Serialization.DataContractAttribute(Name:="BaseTaxesAndFeesType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
   System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseTaxesAndFeesType))>
    Partial Public Class BaseTaxesAndFeesType

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute(name:="Fees")>
        Public Property Fees() As System.Collections.Generic.List(Of BaseFeesType)

        '''<remarks/>
        <System.Runtime.Serialization.DataMemberAttribute(name:="Taxes")>
        Public Property Taxes() As System.Collections.Generic.List(Of BaseTaxesType)

    End Class

    <System.Diagnostics.DebuggerStepThroughAttribute(),
System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
System.Runtime.Serialization.DataContractAttribute(Name:="BaseFeesType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseFeesType))>
    Partial Public Class BaseFeesType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Description() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Amount() As Decimal = CDec(0.0)

    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="GenerateDocumentsV2RequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(GenerateDocumentsV2RequestType))>
    Partial Public Class GenerateDocumentsV2RequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property DocumentProcessType As DocumentProcessType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PolicyProcessType As PolicyProcessTypes

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property InsuranceFileKey As Integer = 0

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property ClaimKey As Integer = 0
    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="GenerateDocumentsV2ResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(GenerateDocumentsV2ResponseType))>
    Partial Public Class GenerateDocumentsV2ResponseType
        Inherits BaseGenerateDocumentV2ResponseType
    End Class

    <System.Diagnostics.DebuggerStepThroughAttribute(),
System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
System.Runtime.Serialization.DataContractAttribute(Name:="BaseGenerateDocumentV2ResponseType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGenerateDocumentV2ResponseType))>
    Partial Public Class BaseGenerateDocumentV2ResponseType
        Inherits BaseResponseType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Documents As System.Collections.Generic.List(Of BaseGenerateDocumentV2ResponseTypeDocument)
    End Class

    <System.Diagnostics.DebuggerStepThroughAttribute(),
System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
System.Runtime.Serialization.DataContractAttribute(Name:="BaseGenerateDocumentV2ResponseTypeDocument", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseGenerateDocumentV2ResponseTypeDocument))>
    Partial Public Class BaseGenerateDocumentV2ResponseTypeDocument

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property SpooledZipFile() As Byte()

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property MergedFilePath() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property DocumentCode() As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property DocumentDescription() As String = String.Empty
    End Class


    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="PolicyProcessV2RequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(PolicyProcessV2RequestType))>
    Partial Public Class PolicyProcessV2RequestType
        Inherits BaseNBQuoteV2RequestType
    End Class

    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="BaseNBQuoteV2RequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(BaseNBQuoteV2RequestType))>
    Partial Public Class BaseNBQuoteV2RequestType
        Inherits BaseRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property AgentCode As String = String.Empty

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CurrencyCode As CurrencyType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CurrencyCodeSpecified As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property UpdateParty As Boolean = False

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property PersonalClient As BasePartyPCType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CorporateClient As BasePartyCCType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property Policy As BaseQuoteRiskMsgType

    End Class
    '''<remarks/>
    <System.Diagnostics.DebuggerStepThroughAttribute(),
    System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
    System.Runtime.Serialization.DataContractAttribute(Name:="LoadServiceRequestType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
    System.Runtime.Serialization.KnownTypeAttribute(GetType(LoadServiceRequestType))>
    Partial Public Class LoadServiceRequestType

        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property CallingApp As String = String.Empty

    End Class

    <System.Diagnostics.DebuggerStepThroughAttribute(),
System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
System.Runtime.Serialization.DataContractAttribute(Name:="QuoteStatusType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
System.Runtime.Serialization.KnownTypeAttribute(GetType(QuoteStatusType))>
    Public Enum QuoteStatusType

        <EnumMember>
        None

        <EnumMember>
        Pending

        <EnumMember>
        AgentPending

        <EnumMember>
        AgentComplete

        <EnumMember>
        Issued

        <EnumMember>
        Live

        <EnumMember>
        Declined
    End Enum

    <System.Diagnostics.DebuggerStepThroughAttribute(),
System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
System.Runtime.Serialization.DataContractAttribute(Name:="PolicyProcessTypes", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
System.Runtime.Serialization.KnownTypeAttribute(GetType(PolicyProcessTypes))>
    Public Enum PolicyProcessTypes

        <EnumMember>
        Bind

        <EnumMember>
        Quote

        <EnumMember>
        GetPolicy
    End Enum

    <System.Diagnostics.DebuggerStepThroughAttribute(),
System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
System.Runtime.Serialization.DataContractAttribute(Name:="DocumentProcessType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
System.Runtime.Serialization.KnownTypeAttribute(GetType(DocumentProcessType))>
    Public Enum DocumentProcessType

        <EnumMember>
        NewBusiness = 1
        <EnumMember>
        MTAAdditionalPremium = 2
        <EnumMember>
        MTAReturnPremium = 3
        <EnumMember>
        MTAZeroPremium = 4
        <EnumMember>
        MTAReinstatement = 5
        <EnumMember>
        RenewalAccept = 6
        <EnumMember>
        RenewalInvite = 7
        <EnumMember>
        LossAdvice = 8
        <EnumMember>
        LargeLossAdvice = 9
        <EnumMember>
        ClaimJacket = 10
        <EnumMember>
        ClaimnotificationtoClient = 11
        <EnumMember>
        ClaimnotificationtoAgent = 12
        <EnumMember>
        ClaimnotificationtoInsurer = 13
        <EnumMember>
        ExternalHandlerNotification = 14
        <EnumMember>
        ClaimAdvicetoAgent = 15
        <EnumMember>
        ChequeRequisition = 16
        <EnumMember>
        ClaimAcceptanceForm = 17
        <EnumMember>
        AdvicetoReinsurer = 18
        <EnumMember>
        ClaimPaymentAdvice = 19
        <EnumMember>
        NewBusinessWritten = 20
        <EnumMember>
        RenewalAcceptanceWritten = 21
    End Enum

    <System.Diagnostics.DebuggerStepThroughAttribute(),
System.CodeDom.Compiler.GeneratedCodeAttribute("System.Runtime.Serialization", "4.0.0.0"),
System.Runtime.Serialization.DataContractAttribute(Name:="DocumentType", [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929"),
System.Runtime.Serialization.KnownTypeAttribute(GetType(DocumentType))>
    Public Enum DocumentType

        <EnumMember>
        Quotation
        <EnumMember>
        Proposal
        <EnumMember>
        DebitNote
        <EnumMember>
        Schedule
        <EnumMember>
        Certificate
        <EnumMember>
        RenewalNotice
        <EnumMember>
        Claims
        <EnumMember>
        Lapse
        <EnumMember>
        Decline
        <EnumMember>
        Cancellation
        <EnumMember>
        PurchaseOrder
        <EnumMember>
        Reinstate
        <EnumMember>
        PaymentReminder
    End Enum
End Namespace
