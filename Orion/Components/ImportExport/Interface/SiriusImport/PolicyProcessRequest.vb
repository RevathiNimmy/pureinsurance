
Option Strict Off
Option Explicit On

Imports System.Xml.Serialization

Namespace Messaging

    'Partial Public Class PolicyProcessRequestXMLType
    <System.Xml.Serialization.XmlIncludeAttribute(GetType(BaseRiskType)), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(BaseQuoteType)), _
    System.Xml.Serialization.XmlIncludeAttribute(GetType(BaseQuoteRiskMsgType)), _
       System.Xml.Serialization.XmlIncludeAttribute(GetType(BasePartyType)), _
    System.Xml.Serialization.XmlIncludeAttribute(GetType(BasePartyCCType)), _
    System.Xml.Serialization.XmlIncludeAttribute(GetType(BasePartyPCType)), _
    System.Xml.Serialization.XmlIncludeAttribute(GetType(BaseNBQuoteRequestType)), _
    System.Xml.Serialization.XmlIncludeAttribute(GetType(PolicyProcessRequestType)), _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18058"), _
    System.SerializableAttribute(), _
    System.Diagnostics.DebuggerStepThroughAttribute(), _
    System.ComponentModel.DesignerCategoryAttribute("code"), _
    System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929")> _
    Partial Public MustInherit Class BaseRequestType

        Private branchCodeField As String

        '''<remarks/>
        Public Property BranchCode() As String
            Get
                Return Me.branchCodeField
            End Get
            Set(value As String)
                Me.branchCodeField = value
            End Set
        End Property
    End Class
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18058"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.siriusfs.com/SFI/SAM/MessagingTypes/20050929")> _
    Partial Public Class PolicyProcessRequestType
        Inherits BaseNBQuoteRequestType
    End Class

    <System.Xml.Serialization.XmlIncludeAttribute(GetType(PolicyProcessRequestType)), _
       System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18058"), _
    System.SerializableAttribute(), _
    System.Diagnostics.DebuggerStepThroughAttribute(), _
    System.ComponentModel.DesignerCategoryAttribute("code"), _
    System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929")> _
    Partial Public Class BaseNBQuoteRequestType
        Inherits BaseRequestType

        Private agentCodeField As String

        Private currencyCodeField As CurrencyType

        Private currencyCodeFieldSpecified As Boolean

        Private updatePartyField As Boolean

        Private itemField As BasePartyType

        Private policyField As BaseQuoteRiskMsgType

        '''<remarks/>
        Public Property AgentCode() As String
            Get
                Return Me.agentCodeField
            End Get
            Set(value As String)
                Me.agentCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CurrencyCode() As CurrencyType
            Get
                Return Me.currencyCodeField
            End Get
            Set(value As CurrencyType)
                Me.currencyCodeField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property CurrencyCodeSpecified() As Boolean
            Get
                Return Me.currencyCodeFieldSpecified
            End Get
            Set(value As Boolean)
                Me.currencyCodeFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property UpdateParty() As Boolean
            Get
                Return Me.updatePartyField
            End Get
            Set(value As Boolean)
                Me.updatePartyField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("CorporateClient", GetType(BasePartyCCType)), _
         System.Xml.Serialization.XmlElementAttribute("PersonalClient", GetType(BasePartyPCType))> _
        Public Property Item() As BasePartyType
            Get
                Return Me.itemField
            End Get
            Set(value As BasePartyType)
                Me.itemField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Policy() As BaseQuoteRiskMsgType
            Get
                Return Me.policyField
            End Get
            Set(value As BaseQuoteRiskMsgType)
                Me.policyField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18058"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929")> _
    Public Enum CurrencyType

        '''<remarks/>
        GBP

        '''<remarks/>
        USD

        '''<remarks/>
        EUR
    End Enum
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18058"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929")> _
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

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18058"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929")> _
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

    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18058"), _
    System.SerializableAttribute(), _
    System.Diagnostics.DebuggerStepThroughAttribute(), _
    System.ComponentModel.DesignerCategoryAttribute("code"), _
    System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929")> _
    Partial Public Class BasePartyPCType
        Inherits BasePartyType

        Private surnameField As String

        Private forenameField As String

        Private dateOfBirthField As Date

        Private dateOfBirthFieldSpecified As Boolean

        Private titleField As String

        Private maritalStatusCodeField As MaritalStatusCodeType

        Private maritalStatusCodeFieldSpecified As Boolean

        Private genderCodeField As String

        Private initialsField As String

        Private occupationCodeField As String

        Private employersBusinessCodeField As String

        Private employmentStatusCodeField As EmploymentStatusCodeType

        Private employmentStatusCodeFieldSpecified As Boolean

        Private alternativeIdField As String

        '''<remarks/>
        Public Property Surname() As String
            Get
                Return Me.surnameField
            End Get
            Set(value As String)
                Me.surnameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Forename() As String
            Get
                Return Me.forenameField
            End Get
            Set(value As String)
                Me.forenameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property DateOfBirth() As Date
            Get
                Return Me.dateOfBirthField
            End Get
            Set(value As Date)
                Me.dateOfBirthField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property DateOfBirthSpecified() As Boolean
            Get
                Return Me.dateOfBirthFieldSpecified
            End Get
            Set(value As Boolean)
                Me.dateOfBirthFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property Title() As String
            Get
                Return Me.titleField
            End Get
            Set(value As String)
                Me.titleField = value
            End Set
        End Property

        '''<remarks/>
        Public Property MaritalStatusCode() As MaritalStatusCodeType
            Get
                Return Me.maritalStatusCodeField
            End Get
            Set(value As MaritalStatusCodeType)
                Me.maritalStatusCodeField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property MaritalStatusCodeSpecified() As Boolean
            Get
                Return Me.maritalStatusCodeFieldSpecified
            End Get
            Set(value As Boolean)
                Me.maritalStatusCodeFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property GenderCode() As String
            Get
                Return Me.genderCodeField
            End Get
            Set(value As String)
                Me.genderCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Initials() As String
            Get
                Return Me.initialsField
            End Get
            Set(value As String)
                Me.initialsField = value
            End Set
        End Property

        '''<remarks/>
        Public Property OccupationCode() As String
            Get
                Return Me.occupationCodeField
            End Get
            Set(value As String)
                Me.occupationCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property EmployersBusinessCode() As String
            Get
                Return Me.employersBusinessCodeField
            End Get
            Set(value As String)
                Me.employersBusinessCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property EmploymentStatusCode() As EmploymentStatusCodeType
            Get
                Return Me.employmentStatusCodeField
            End Get
            Set(value As EmploymentStatusCodeType)
                Me.employmentStatusCodeField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property EmploymentStatusCodeSpecified() As Boolean
            Get
                Return Me.employmentStatusCodeFieldSpecified
            End Get
            Set(value As Boolean)
                Me.employmentStatusCodeFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property AlternativeId() As String
            Get
                Return Me.alternativeIdField
            End Get
            Set(value As String)
                Me.alternativeIdField = value
            End Set
        End Property
    End Class
    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18058"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929")> _
    Partial Public Class BasePartyCCType
        Inherits BasePartyType

        Private companyNameField As String

        Private businessCodeField As String

        Private mainContactField As String

        Private numberOfOfficesField As Integer

        Private numberOfOfficesFieldSpecified As Boolean

        Private numberOfEmployeesField As String

        '''<remarks/>
        Public Property CompanyName() As String
            Get
                Return Me.companyNameField
            End Get
            Set(value As String)
                Me.companyNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property BusinessCode() As String
            Get
                Return Me.businessCodeField
            End Get
            Set(value As String)
                Me.businessCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property MainContact() As String
            Get
                Return Me.mainContactField
            End Get
            Set(value As String)
                Me.mainContactField = value
            End Set
        End Property

        '''<remarks/>
        Public Property NumberOfOffices() As Integer
            Get
                Return Me.numberOfOfficesField
            End Get
            Set(value As Integer)
                Me.numberOfOfficesField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property NumberOfOfficesSpecified() As Boolean
            Get
                Return Me.numberOfOfficesFieldSpecified
            End Get
            Set(value As Boolean)
                Me.numberOfOfficesFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property NumberOfEmployees() As String
            Get
                Return Me.numberOfEmployeesField
            End Get
            Set(value As String)
                Me.numberOfEmployeesField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.Xml.Serialization.XmlIncludeAttribute(GetType(BasePartyCCType)), _
     System.Xml.Serialization.XmlIncludeAttribute(GetType(BasePartyPCType)), _
     System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18058"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929")> _
    Partial Public Class BasePartyType
        Inherits BaseRequestType

        Private addressesField() As BaseAddressType

        Private tPUserCodeField As String

        Private tPIntroducerField As String

        Private contactsField() As BaseContactType

        Private accountExecutiveField As String

        Private currencyField As String

        Private fileCodeField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Addresses")> _
        Public Property Addresses() As BaseAddressType()
            Get
                Return Me.addressesField
            End Get
            Set(value As BaseAddressType())
                Me.addressesField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TPUserCode() As String
            Get
                Return Me.tPUserCodeField
            End Get
            Set(value As String)
                Me.tPUserCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TPIntroducer() As String
            Get
                Return Me.tPIntroducerField
            End Get
            Set(value As String)
                Me.tPIntroducerField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Contacts")> _
        Public Property Contacts() As BaseContactType()
            Get
                Return Me.contactsField
            End Get
            Set(value As BaseContactType())
                Me.contactsField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AccountExecutive() As String
            Get
                Return Me.accountExecutiveField
            End Get
            Set(value As String)
                Me.accountExecutiveField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Currency() As String
            Get
                Return Me.currencyField
            End Get
            Set(value As String)
                Me.currencyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property FileCode() As String
            Get
                Return Me.fileCodeField
            End Get
            Set(value As String)
                Me.fileCodeField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18058"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929")> _
    Partial Public Class BaseAddressType

        Private addressTypeCodeField As AddressTypeType

        Private addressLine1Field As String

        Private addressLine2Field As String

        Private addressLine3Field As String

        Private addressLine4Field As String

        Private countryCodeField As String

        Private postCodeField As String

        '''<remarks/>
        Public Property AddressTypeCode() As AddressTypeType
            Get
                Return Me.addressTypeCodeField
            End Get
            Set(value As AddressTypeType)
                Me.addressTypeCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AddressLine1() As String
            Get
                Return Me.addressLine1Field
            End Get
            Set(value As String)
                Me.addressLine1Field = value
            End Set
        End Property

        '''<remarks/>
        Public Property AddressLine2() As String
            Get
                Return Me.addressLine2Field
            End Get
            Set(value As String)
                Me.addressLine2Field = value
            End Set
        End Property

        '''<remarks/>
        Public Property AddressLine3() As String
            Get
                Return Me.addressLine3Field
            End Get
            Set(value As String)
                Me.addressLine3Field = value
            End Set
        End Property

        '''<remarks/>
        Public Property AddressLine4() As String
            Get
                Return Me.addressLine4Field
            End Get
            Set(value As String)
                Me.addressLine4Field = value
            End Set
        End Property

        '''<remarks/>
        Public Property CountryCode() As String
            Get
                Return Me.countryCodeField
            End Get
            Set(value As String)
                Me.countryCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PostCode() As String
            Get
                Return Me.postCodeField
            End Get
            Set(value As String)
                Me.postCodeField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18058"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929")> _
    Public Enum AddressTypeType

        '''<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("3131 001")> _
        Item3131001

        '''<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("3131 002")> _
        Item3131002

        '''<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("3131 0X9")> _
        Item31310X9

        '''<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("3131 0XR")> _
        Item31310XR

        '''<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("3131 XBA")> _
        Item3131XBA

        '''<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("3131 XBI")> _
        Item3131XBI

        '''<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("3131 XCO")> _
        Item3131XCO

        '''<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("3131 XPR")> _
        Item3131XPR

        '''<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("3131 XRE")> _
        Item3131XRE

        '''<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("3131 XSA")> _
        Item3131XSA
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18058"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929")> _
    Partial Public Class BaseQuoteRiskMsgType
        Inherits BaseQuoteType

        Private doNotCopyRiskAtRenSelectionField As Integer

        Private deletePolicyUnderRenewalField As Integer

        Private risksField() As BaseRiskType

        Private commissionRateField As Decimal

        Private commissionValueField As Decimal

        Private taxesField() As BaseTaxesType

        Private partyKeyField As Integer

        '''<remarks/>
        Public Property DoNotCopyRiskAtRenSelection() As Integer
            Get
                Return Me.doNotCopyRiskAtRenSelectionField
            End Get
            Set(value As Integer)
                Me.doNotCopyRiskAtRenSelectionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property DeletePolicyUnderRenewal() As Integer
            Get
                Return Me.deletePolicyUnderRenewalField
            End Get
            Set(value As Integer)
                Me.deletePolicyUnderRenewalField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Risks")> _
        Public Property Risks() As BaseRiskType()
            Get
                Return Me.risksField
            End Get
            Set(value As BaseRiskType())
                Me.risksField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CommissionRate() As Decimal
            Get
                Return Me.commissionRateField
            End Get
            Set(value As Decimal)
                Me.commissionRateField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CommissionValue() As Decimal
            Get
                Return Me.commissionValueField
            End Get
            Set(value As Decimal)
                Me.commissionValueField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Taxes")> _
        Public Property Taxes() As BaseTaxesType()
            Get
                Return Me.taxesField
            End Get
            Set(value As BaseTaxesType())
                Me.taxesField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PartyKey() As Integer
            Get
                Return Me.partyKeyField
            End Get
            Set(value As Integer)
                Me.partyKeyField = value
            End Set
        End Property
    End Class

    <System.Xml.Serialization.XmlIncludeAttribute(GetType(BaseQuoteRiskMsgType)), _
      System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18058"), _
   System.SerializableAttribute(), _
   System.Diagnostics.DebuggerStepThroughAttribute(), _
   System.ComponentModel.DesignerCategoryAttribute("code"), _
   System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929")> _
    Partial Public Class BaseQuoteType
        Inherits BaseRequestType

        Private mTAReasonCodeField As String

        Private businessTypeCodeField As String

        Private analysisCodeField As String

        Private lastTransDescriptionField As String

        Private oldPolicyNumberField As String

        Private transactionDueDateField As Date

        Private coverStartDateField As Date

        Private coverEndDateField As Date

        Private productCodeField As String

        Private descriptionField As String

        Private quoteRefField As String

        Private newQuoteRefField As String

        Private insuredNameField As String

        Private currencyCodeField As String

        Private transactionTypeCodeField As String

        Private policyStatusCodeField As String

        Private alternateReferenceField As String

        Private underwritingYearCodeField As String

        '''<remarks/>
        Public Property MTAReasonCode() As String
            Get
                Return Me.mTAReasonCodeField
            End Get
            Set(value As String)
                Me.mTAReasonCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property BusinessTypeCode() As String
            Get
                Return Me.businessTypeCodeField
            End Get
            Set(value As String)
                Me.businessTypeCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AnalysisCode() As String
            Get
                Return Me.analysisCodeField
            End Get
            Set(value As String)
                Me.analysisCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property LastTransDescription() As String
            Get
                Return Me.lastTransDescriptionField
            End Get
            Set(value As String)
                Me.lastTransDescriptionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property OldPolicyNumber() As String
            Get
                Return Me.oldPolicyNumberField
            End Get
            Set(value As String)
                Me.oldPolicyNumberField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TransactionDueDate() As Date
            Get
                Return Me.transactionDueDateField
            End Get
            Set(value As Date)
                Me.transactionDueDateField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CoverStartDate() As Date
            Get
                Return Me.coverStartDateField
            End Get
            Set(value As Date)
                Me.coverStartDateField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CoverEndDate() As Date
            Get
                Return Me.coverEndDateField
            End Get
            Set(value As Date)
                Me.coverEndDateField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ProductCode() As String
            Get
                Return Me.productCodeField
            End Get
            Set(value As String)
                Me.productCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set(value As String)
                Me.descriptionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property QuoteRef() As String
            Get
                Return Me.quoteRefField
            End Get
            Set(value As String)
                Me.quoteRefField = value
            End Set
        End Property

        '''<remarks/>
        Public Property NewQuoteRef() As String
            Get
                Return Me.newQuoteRefField
            End Get
            Set(value As String)
                Me.newQuoteRefField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InsuredName() As String
            Get
                Return Me.insuredNameField
            End Get
            Set(value As String)
                Me.insuredNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CurrencyCode() As String
            Get
                Return Me.currencyCodeField
            End Get
            Set(value As String)
                Me.currencyCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TransactionTypeCode() As String
            Get
                Return Me.transactionTypeCodeField
            End Get
            Set(value As String)
                Me.transactionTypeCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PolicyStatusCode() As String
            Get
                Return Me.policyStatusCodeField
            End Get
            Set(value As String)
                Me.policyStatusCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AlternateReference() As String
            Get
                Return Me.alternateReferenceField
            End Get
            Set(value As String)
                Me.alternateReferenceField = value
            End Set
        End Property

        '''<remarks/>
        Public Property UnderwritingYearCode() As String
            Get
                Return Me.underwritingYearCodeField
            End Get
            Set(value As String)
                Me.underwritingYearCodeField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18058"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929")> _
    Partial Public Class BaseRiskType
        Inherits BaseRequestType

        Private riskTypeCodeField As String

        Private screenCodeField As String

        Private riskDescriptionField As String

        Private dataModelCodeField As String

        Private quoteTimeStampField() As Byte

        Private runDefaultRulesField As Boolean

        Private riskFolderKeyField As Integer

        Private riskFolderKeyFieldSpecified As Boolean

        Private xMLDataSetField As String

        Private productBuilderDetailField() As BaseProductBuilderRiskType

        Private taxesField() As BaseTaxesType

        '''<remarks/>
        Public Property RiskTypeCode() As String
            Get
                Return Me.riskTypeCodeField
            End Get
            Set(value As String)
                Me.riskTypeCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ScreenCode() As String
            Get
                Return Me.screenCodeField
            End Get
            Set(value As String)
                Me.screenCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RiskDescription() As String
            Get
                Return Me.riskDescriptionField
            End Get
            Set(value As String)
                Me.riskDescriptionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property DataModelCode() As String
            Get
                Return Me.dataModelCodeField
            End Get
            Set(value As String)
                Me.dataModelCodeField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="base64Binary")> _
        Public Property QuoteTimeStamp() As Byte()
            Get
                Return Me.quoteTimeStampField
            End Get
            Set(value As Byte())
                Me.quoteTimeStampField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RunDefaultRules() As Boolean
            Get
                Return Me.runDefaultRulesField
            End Get
            Set(value As Boolean)
                Me.runDefaultRulesField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RiskFolderKey() As Integer
            Get
                Return Me.riskFolderKeyField
            End Get
            Set(value As Integer)
                Me.riskFolderKeyField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property RiskFolderKeySpecified() As Boolean
            Get
                Return Me.riskFolderKeyFieldSpecified
            End Get
            Set(value As Boolean)
                Me.riskFolderKeyFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property XMLDataSet() As String
            Get
                Return Me.xMLDataSetField
            End Get
            Set(value As String)
                Me.xMLDataSetField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("ProductBuilderDetail")> _
        Public Property ProductBuilderDetail() As BaseProductBuilderRiskType()
            Get
                Return Me.productBuilderDetailField
            End Get
            Set(value As BaseProductBuilderRiskType())
                Me.productBuilderDetailField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Taxes")> _
        Public Property Taxes() As BaseTaxesType()
            Get
                Return Me.taxesField
            End Get
            Set(value As BaseTaxesType())
                Me.taxesField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18058"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929")> _
    Partial Public Class BaseProductBuilderRiskType

        Private productBuilderDataField As BaseProductBuilderRiskTypeProductBuilderData

        '''<remarks/>
        Public Property ProductBuilderData() As BaseProductBuilderRiskTypeProductBuilderData
            Get
                Return Me.productBuilderDataField
            End Get
            Set(value As BaseProductBuilderRiskTypeProductBuilderData)
                Me.productBuilderDataField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18058"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute(AnonymousType:=True, [Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929")> _
    Partial Public Class BaseProductBuilderRiskTypeProductBuilderData

        Private itemNameField As String

        Private valueField As String

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Property ItemName() As String
            Get
                Return Me.itemNameField
            End Get
            Set(value As String)
                Me.itemNameField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlAttributeAttribute()> _
        Public Property Value() As String
            Get
                Return Me.valueField
            End Get
            Set(value As String)
                Me.valueField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18058"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929")> _
    Partial Public Class BaseTaxesType

        Private descriptionField As String

        Private amountField As Decimal

        Private taxBandCodeField As String

        Private taxRateField As Decimal

        '''<remarks/>
        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set(value As String)
                Me.descriptionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Amount() As Decimal
            Get
                Return Me.amountField
            End Get
            Set(value As Decimal)
                Me.amountField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TaxBandCode() As String
            Get
                Return Me.taxBandCodeField
            End Get
            Set(value As String)
                Me.taxBandCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TaxRate() As Decimal
            Get
                Return Me.taxRateField
            End Get
            Set(value As Decimal)
                Me.taxRateField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18058"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929")> _
    Partial Public Class BaseContactType

        Private contactTypeCodeField As ContactTypeType

        Private contactDetailField As BaseContactDetailType

        Private areaCodeField As String

        '''<remarks/>
        Public Property ContactTypeCode() As ContactTypeType
            Get
                Return Me.contactTypeCodeField
            End Get
            Set(value As ContactTypeType)
                Me.contactTypeCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ContactDetail() As BaseContactDetailType
            Get
                Return Me.contactDetailField
            End Get
            Set(value As BaseContactDetailType)
                Me.contactDetailField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AreaCode() As String
            Get
                Return Me.areaCodeField
            End Get
            Set(value As String)
                Me.areaCodeField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18058"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929")> _
    Public Enum ContactTypeType

        '''<remarks/>
        EMAIL

        '''<remarks/>
        HOMEPHONE

        '''<remarks/>
        MOBILE

        '''<remarks/>
        FAX

        '''<remarks/>
        WEB

        '''<remarks/>
        MAIN
    End Enum

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18058"), _
     System.SerializableAttribute(), _
     System.Diagnostics.DebuggerStepThroughAttribute(), _
     System.ComponentModel.DesignerCategoryAttribute("code"), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929")> _
    Partial Public Class BaseContactDetailType

        Private itemField As String

        Private itemElementNameField As ItemChoiceType

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("EmailAddress", GetType(String)), _
         System.Xml.Serialization.XmlElementAttribute("Number", GetType(String)), _
         System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")> _
        Public Property Item() As String
            Get
                Return Me.itemField
            End Get
            Set(value As String)
                Me.itemField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
        Public Property ItemElementName() As ItemChoiceType
            Get
                Return Me.itemElementNameField
            End Get
            Set(value As ItemChoiceType)
                Me.itemElementNameField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    <System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.18058"), _
     System.SerializableAttribute(), _
     System.Xml.Serialization.XmlTypeAttribute([Namespace]:="http://www.siriusfs.com/SFI/SAM/BaseTypes/20050929", IncludeInSchema:=False)> _
    Public Enum ItemChoiceType

        '''<remarks/>
        EmailAddress

        '''<remarks/>
        Number
    End Enum
    ' End Class
End Namespace
