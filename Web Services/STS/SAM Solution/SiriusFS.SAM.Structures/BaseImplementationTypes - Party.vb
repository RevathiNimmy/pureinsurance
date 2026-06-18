Option Strict On

' Changes:
' 170505 CJB PN20978 Changes in Broking to allow document producton to be used in Swift (SJP) via the STS'''

#Region " Imports "

Imports System
Imports System.Text
Imports System.Xml.Serialization
Imports SiriusFS.SAM.Structure
Imports Sirius.Architecture.ExceptionHandling
Imports System.Collections.Generic
'Imports SiriusFS.SAM.Structure.SFI.SAMForInsuranceV2

'Imports SiriusFS.SAM.ServiceAgent.PMEReturnCode

#End Region

Namespace BaseImplementationTypes

    '''<remarks/>
    Public Class BaseAddPartyRequestType
        Inherits BaseRequestType

        Private PartyField As BasePartyType

        Private subBranchCodeField As String


        Private _AgentKey As Integer
        Public Property AgentKey() As Integer
            Get
                Return _AgentKey
            End Get
            Set(ByVal value As Integer)
                _AgentKey = value
            End Set
        End Property



        '''<remarks/>
        Public Property Party() As BasePartyType
            Get
                Return Me.PartyField
            End Get
            Set(ByVal value As BasePartyType)
                Me.PartyField = value
            End Set
        End Property
        '''<remarks/>
        Public Property SubBranchCode() As String
            Get
                Return Me.subBranchCodeField
            End Get
            Set(ByVal value As String)
                Me.subBranchCodeField = value
            End Set
        End Property
    End Class


    '''<remarks/>
    Public Class BaseAddPartyResponseType
        Inherits BaseResponseType

        Private partyKeyField As Integer

        Private shortnameField As String

        Private partyTypeIdField As Integer

        Private partyTimestampField() As Byte

        Private resolvedNameField As String

        Private xMLDatasetField As String

        Public Property XMLDataset() As String
            Get
                Return Me.xMLDatasetField
            End Get
            Set(ByVal value As String)
                Me.xMLDatasetField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PartyKey() As Integer
            Get
                Return Me.partyKeyField
            End Get
            Set(ByVal value As Integer)
                Me.partyKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PartyTypeId() As Integer
            Get
                Return Me.partyTypeIdField
            End Get
            Set(ByVal value As Integer)
                Me.partyTypeIdField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Shortname() As String
            Get
                Return Me.shortnameField
            End Get
            Set(ByVal value As String)
                Me.shortnameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PartyTimestamp() As Byte()
            Get
                Return Me.partyTimestampField
            End Get
            Set(ByVal value As Byte())
                Me.partyTimestampField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ResolvedName() As String
            Get
                Return Me.resolvedNameField
            End Get
            Set(ByVal value As String)
                Me.resolvedNameField = value
            End Set
        End Property
    End Class


    '''<remarks/>
    Public Class BaseGetPartyRequestType
        Inherits BaseRequestType

        Private partyKeyField As Integer

        '''<remarks/>
        Public Property PartyKey() As Integer
            Get
                Return Me.partyKeyField
            End Get
            Set(ByVal value As Integer)
                Me.partyKeyField = value
            End Set
        End Property

        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            MyBase.Validate(oErrorCollection)

            If PartyKey = 0 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                   SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                   "PartyKey")
            End If


        End Sub

    End Class


    '''<remarks/>

    Partial Public Class BaseFindPartyRequestType
        Inherits BaseRequestType

        Private partyTypeField As String

        Private partyTypeFieldSpecified As Boolean

        Private shortnameField As String

        Private alternativeIdField As String

        Private nameField As String

        Private firstnameField As String

        Private addressLine1Field As String

        Private addressLine2Field As String

        Private addressLine3Field As String

        Private addressLine4Field As String

        Private addressLine5Field As String

        Private addressLine6Field As String

        Private addressLine7Field As String

        Private addressLine8Field As String

        Private addressLine9Field As String

        Private addressLine10Field As String
        Private postCodeField As String

        Private areaCodeField As String

        Private telephoneNumberField As String

        Private dateOfBirthField As Date

        Private dateOfBirthFieldSpecified As Boolean

        Private IsAnySelectedField As Boolean

        Private policyRefField As String

        Private riskIndexField As String

        Private fileCodeField As String

        Private riskRequestdexField As String

        Private claimsRiskIndexField As String

        Private includeClosedBranchesField As Boolean

        Private claimNumberField As String

        Private statusField As String

        Private agentTypeField As PartyAgentType

        Private agentTypeFieldSpecified As Boolean

        Private supressSubAgentsField As Boolean

        Private supressSubAgentsFieldSpecified As Boolean

        Private partySourceIdField As Integer

        Private partySourceIdFieldSpecified As Boolean

        Private transactionTypeField As String

        Private agentKeyField As Integer
        Private agentGroupKeyField As Integer

        Private maxRowsToFetchField As Integer

        Private maxRowsToFetchFieldSpecified As Boolean

        Private otherPartyTypeCodeField As String

        Private caseNumberField As String

        Private caseNumberFieldSpecified As Boolean

        Private bIncludeAgentField As Boolean

        Private agentGroupField As String

        Private searchTypeField As String

        '''<remarks/>
        Public Property AgentKey() As Integer
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Integer)
                Me.agentKeyField = value
            End Set
        End Property

        Public Property AgentGroupKey() As Integer
            Get
                Return Me.agentGroupKeyField
            End Get
            Set(ByVal value As Integer)
                Me.agentGroupKeyField = value
            End Set
        End Property

        Public Property PartyType() As String
            Get
                Return Me.partyTypeField
            End Get
            Set(ByVal value As String)
                Me.partyTypeField = value
            End Set
        End Property


        <System.Runtime.Serialization.DataMemberAttribute()>
        Public Property IsAnySelected() As Boolean
            Get
                Return Me.IsAnySelectedField
            End Get
            Set(value As Boolean)
                Me.IsAnySelectedField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property PartyTypeSpecified() As Boolean
            Get
                Return Me.partyTypeFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.partyTypeFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property Shortname() As String
            Get
                Return Me.shortnameField
            End Get
            Set(ByVal value As String)
                Me.shortnameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AlternativeId() As String
            Get
                Return Me.alternativeIdField
            End Get
            Set(ByVal value As String)
                Me.alternativeIdField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Name() As String
            Get
                Return Me.nameField
            End Get
            Set(ByVal value As String)
                Me.nameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Firstname() As String
            Get
                Return Me.firstnameField
            End Get
            Set(ByVal value As String)
                Me.firstnameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AddressLine1() As String
            Get
                Return Me.addressLine1Field
            End Get
            Set(ByVal value As String)
                Me.addressLine1Field = value
            End Set
        End Property

        '''<remarks/>
        Public Property AddressLine2() As String
            Get
                Return Me.addressLine2Field
            End Get
            Set(ByVal value As String)
                Me.addressLine2Field = value
            End Set
        End Property

        '''<remarks/>
        Public Property AddressLine3() As String
            Get
                Return Me.addressLine3Field
            End Get
            Set(ByVal value As String)
                Me.addressLine3Field = value
            End Set
        End Property

        '''<remarks/>
        Public Property AddressLine4() As String
            Get
                Return Me.addressLine4Field
            End Get
            Set(ByVal value As String)
                Me.addressLine4Field = value
            End Set
        End Property

        '''<remarks/>
        Public Property AddressLine5() As String
            Get
                Return Me.addressLine5Field
            End Get
            Set(ByVal value As String)
                Me.addressLine5Field = value
            End Set
        End Property

        '''<remarks/>
        Public Property AddressLine6() As String
            Get
                Return Me.addressLine6Field
            End Get
            Set(ByVal value As String)
                Me.addressLine6Field = value
            End Set
        End Property

        '''<remarks/>
        Public Property AddressLine7() As String
            Get
                Return Me.addressLine7Field
            End Get
            Set(ByVal value As String)
                Me.addressLine7Field = value
            End Set
        End Property

        '''<remarks/>
        Public Property AddressLine8() As String
            Get
                Return Me.addressLine8Field
            End Get
            Set(ByVal value As String)
                Me.addressLine8Field = value
            End Set
        End Property

        '''<remarks/>
        Public Property AddressLine9() As String
            Get
                Return Me.addressLine9Field
            End Get
            Set(ByVal value As String)
                Me.addressLine9Field = value
            End Set
        End Property

        '''<remarks/>
        Public Property AddressLine10() As String
            Get
                Return Me.addressLine10Field
            End Get
            Set(ByVal value As String)
                Me.addressLine10Field = value
            End Set
        End Property
        '''<remarks/>
        Public Property PostCode() As String
            Get
                Return Me.postCodeField
            End Get
            Set(ByVal value As String)
                Me.postCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AreaCode() As String
            Get
                Return Me.areaCodeField
            End Get
            Set(ByVal value As String)
                Me.areaCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TelephoneNumber() As String
            Get
                Return Me.telephoneNumberField
            End Get
            Set(ByVal value As String)
                Me.telephoneNumberField = value
            End Set
        End Property

        '''<remarks/>
        Public Property DateOfBirth() As Date
            Get
                Return Me.dateOfBirthField
            End Get
            Set(ByVal value As Date)
                Me.dateOfBirthField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DateOfBirthSpecified() As Boolean
            Get
                Return Me.dateOfBirthFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.dateOfBirthFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property PolicyRef() As String
            Get
                Return Me.policyRefField
            End Get
            Set(ByVal value As String)
                Me.policyRefField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RiskIndex() As String
            Get
                Return Me.riskIndexField
            End Get
            Set(ByVal value As String)
                Me.riskIndexField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimsRiskIndex() As String
            Get
                Return Me.claimsRiskIndexField
            End Get
            Set(ByVal value As String)
                Me.claimsRiskIndexField = value
            End Set
        End Property

        '''<remarks/>
        Public Property FileCode() As String
            Get
                Return Me.fileCodeField
            End Get
            Set(ByVal value As String)
                Me.fileCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property IncludeClosedBranches() As Boolean
            Get
                Return Me.includeClosedBranchesField
            End Get
            Set(ByVal value As Boolean)
                Me.includeClosedBranchesField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimNumber() As String
            Get
                Return Me.claimNumberField
            End Get
            Set(ByVal value As String)
                Me.claimNumberField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Status() As String
            Get
                Return Me.statusField
            End Get
            Set(ByVal value As String)
                Me.statusField = value
            End Set
        End Property
        Public Property RiskRequestdex() As String
            Get
                Return Me.riskRequestdexField
            End Get
            Set(ByVal value As String)
                Me.riskRequestdexField = value
            End Set
        End Property
        '''<remarks/>
        Public Property AgentType() As PartyAgentType
            Get
                Return Me.agentTypeField
            End Get
            Set(ByVal value As PartyAgentType)
                Me.agentTypeField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property AgentTypeSpecified() As Boolean
            Get
                Return Me.agentTypeFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.agentTypeFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property SupressSubAgents() As Boolean
            Get
                Return Me.supressSubAgentsField
            End Get
            Set(ByVal value As Boolean)
                Me.supressSubAgentsField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property SupressSubAgentsSpecified() As Boolean
            Get
                Return Me.supressSubAgentsFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.supressSubAgentsFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property PartySourceId() As Integer
            Get
                Return Me.partySourceIdField
            End Get
            Set(ByVal value As Integer)
                Me.partySourceIdField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property PartySourceIdSpecified() As Boolean
            Get
                Return Me.partySourceIdFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.partySourceIdFieldSpecified = value
            End Set
        End Property
        '''<remarks/>
        Public Property TransactionType() As String
            Get
                Return Me.transactionTypeField
            End Get
            Set(ByVal value As String)
                Me.transactionTypeField = value
            End Set
        End Property
        '''<remarks/>
        Public Property MaxRowsToFetch() As Integer
            Get
                Return Me.maxRowsToFetchField
            End Get
            Set(ByVal value As Integer)
                Me.maxRowsToFetchField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property MaxRowsToFetchSpecified() As Boolean
            Get
                Return Me.maxRowsToFetchFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.maxRowsToFetchFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property OtherPartyTypeCode() As String
            Get
                Return Me.otherPartyTypeCodeField
            End Get
            Set(ByVal value As String)
                Me.otherPartyTypeCodeField = value
            End Set
        End Property
        '''<remarks/>
        Public Property CaseNumber() As String
            Get
                Return Me.caseNumberField
            End Get
            Set(ByVal value As String)
                Me.caseNumberField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property CaseNumberSpecified() As Boolean
            Get
                Return Me.caseNumberFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.caseNumberFieldSpecified = value
            End Set
        End Property
        '''<remarks/>
        Public Property AgentGroup() As String
            Get
                Return Me.agentGroupField
            End Get
            Set(ByVal value As String)
                Me.agentGroupField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SearchType() As String
            Get
                Return Me.searchTypeField
            End Get
            Set(ByVal value As String)
                Me.searchTypeField = value
            End Set
        End Property

        Public Property PartyIndex() As String

        Public Property IncludeAgent() As Boolean
            Get
                Return Me.bIncludeAgentField
            End Get
            Set(ByVal value As Boolean)
                Me.bIncludeAgentField = value
            End Set
        End Property
#Region "Public Function - Mandatory Validate"
        'Start (Vijayakumar Ramasamy)- (Tech Spec - UIICWR50 - MTC - Party Search) -(7.1.4) 
        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            MyBase.Validate(oErrorCollection)

            If String.IsNullOrEmpty(PartyType) AndAlso partyTypeFieldSpecified Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                   SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                   "PartyType")
            End If

            If String.IsNullOrEmpty(Status) Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                   SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                   "Status")
            End If

            'If String.IsNullOrEmpty(DateOfBirth.ToString) Then
            '    oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing, _
            '                                       SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
            '                                       "DateOfBirth")
            'End If

        End Sub
        'End (Vijayakumar Ramasamy)- (Tech Spec - UIICWR50 - MTC - Party Search) -(7.1.4) 
#End Region
    End Class
#Region "Public Enum PartyAgentType"
    'Start (Vijayakumar Ramasamy)-(Tech Spec - UIICWR22 - Capture Quote Details - Find Party (Agents).doc)-(7.1.2.1)
    '''<summary>
    ''' This is Enum for PartyAgentType
    '''</summary>
    '''<remarks></remarks>
    Public Enum PartyAgentType

        '''<remarks/>
        Broker

        '''<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("Sub-Agent")>
        SubAgent

        '''<remarks/>
        <System.Xml.Serialization.XmlEnumAttribute("Commission Account")>
        CommissionAccount

        '''<remarks/>
        Intermediary
    End Enum
    'End (Vijayakumar Ramasamy)-(Tech Spec - UIICWR22 - Capture Quote Details - Find Party (Agents).doc)-(7.1.2.1)
#End Region

    '''<remarks/>
    Public Class BaseFindPartyResponseType
        Inherits BaseResponseType

        Private resultDatasetField As System.Xml.XmlElement

        '''<remarks/>
        Public Property ResultDataset() As System.Xml.XmlElement
            Get
                Return Me.resultDatasetField
            End Get
            Set(ByVal value As System.Xml.XmlElement)
                Me.resultDatasetField = value
            End Set
        End Property


        Private reseultdatafield As DataSet
        '''<remarks/>
        Public Property ResultData() As DataSet
            Get
                Return Me.reseultdatafield
            End Get
            Set(ByVal value As DataSet)
                Me.reseultdatafield = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class BaseGetPartyResponseType
        Inherits BaseResponseType

        Private itemField As BasePartyType

        Private partyTimestampField() As Byte

        Private noofPoliciesField As Integer

        Private noofOpenClaimsField As Integer

        Private noofClosedClaimsField As Integer


        '''<remarks/>
        Public Property Item() As BasePartyType
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As BasePartyType)
                Me.itemField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PartyTimestamp() As Byte()
            Get
                Return Me.partyTimestampField
            End Get
            Set(ByVal value As Byte())
                Me.partyTimestampField = value
            End Set
        End Property

        '''<remarks/>
        Public Property NoofPolicies() As Integer
            Get
                Return Me.noofPoliciesField
            End Get
            Set(ByVal value As Integer)
                Me.noofPoliciesField = value
            End Set
        End Property

        '''<remarks/>
        Public Property NoofOpenClaims() As Integer
            Get
                Return Me.noofOpenClaimsField
            End Get
            Set(ByVal value As Integer)
                Me.noofOpenClaimsField = value
            End Set
        End Property

        '''<remarks/>
        Public Property NoofClosedClaims() As Integer
            Get
                Return Me.noofClosedClaimsField
            End Get
            Set(ByVal value As Integer)
                Me.noofClosedClaimsField = value
            End Set
        End Property


    End Class


    '''<remarks/>
    Public Class BaseGetPartySummaryResponseType
        Inherits BaseResponseType

        Private itemField As BasePartyType

        Private insuranceFileDatasetField As System.Xml.XmlElement

        Private partyTimestampField() As Byte
        Private reseultdatafield As DataSet
        '''<remarks/>
        Public Property ResultData() As DataSet
            Get
                Return Me.reseultdatafield
            End Get
            Set(ByVal value As DataSet)
                Me.reseultdatafield = value
            End Set
        End Property

        '''<remarks/>
        Public Property Item() As BasePartyType
            Get
                Return Me.itemField
            End Get
            Set(ByVal value As BasePartyType)
                Me.itemField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InsuranceFileDataset() As System.Xml.XmlElement
            Get
                Return Me.insuranceFileDatasetField
            End Get
            Set(ByVal value As System.Xml.XmlElement)
                Me.insuranceFileDatasetField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PartyTimestamp() As Byte()
            Get
                Return Me.partyTimestampField
            End Get
            Set(ByVal value As Byte())
                Me.partyTimestampField = value
            End Set
        End Property
    End Class


    Public Class BasePartyCCType
        Inherits BasePartyType

        Private companyNameField As String

        Private businessCodeField As String

        Private mainContactField As String

        Private numberOfOfficesField As Integer

        Private numberOfOfficesFieldSpecified As Boolean

        Private numberOfEmployeesField As String

        Private tradeCodeField As String

        'Start (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (7.4.3.1.1)

        Private clientDetailField As BaseClientSharedDataType

        Private companyRegField As String

        Private sICCodeField As String

        Private tradingSinceField As Date

        Private tradingSinceFieldSpecified As Boolean

        Private wageRollField As Decimal

        Private wageRollFieldSpecified As Boolean

        Private turnoverCodeField As String

        Private financialYearField As Date

        Private financialYearFieldSpecified As Boolean

        Private salutationField As String

        Private tPSField As Boolean

        Private tPSFieldSpecified As Boolean

        Private mPSField As Boolean

        Private mPSFieldSpecified As Boolean

        Private eMPSField As Boolean

        Private eMPSFieldSpecified As Boolean

        Private sourceField As String

        'End (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (7.4.3.1.1)
        'Start (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (changes on 08-08-08)
        Private alternativeIdField As String
        'End (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (changes on 08-08-08)

        Public Property CompanyName() As String
            Get
                Return Me.companyNameField
            End Get
            Set(ByVal value As String)
                Me.companyNameField = value
            End Set
        End Property
        Public Property BusinessCode() As String
            Get
                Return Me.businessCodeField
            End Get
            Set(ByVal value As String)
                Me.businessCodeField = value
            End Set
        End Property

        Public Property MainContact() As String
            Get
                Return Me.mainContactField
            End Get
            Set(ByVal value As String)
                Me.mainContactField = value
            End Set
        End Property

        Public Property NumberOfOffices() As Integer
            Get
                Return Me.numberOfOfficesField
            End Get
            Set(ByVal value As Integer)
                Me.numberOfOfficesField = value
            End Set
        End Property

        Public Property NumberOfOfficesSpecified() As Boolean
            Get
                Return Me.numberOfOfficesFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.numberOfOfficesFieldSpecified = value
            End Set
        End Property

        Public Property NumberOfEmployees() As String
            Get
                Return Me.numberOfEmployeesField
            End Get
            Set(ByVal value As String)
                Me.numberOfEmployeesField = value
            End Set
        End Property

        Public Property TradeCode() As String
            Get
                Return Me.tradeCodeField
            End Get
            Set(ByVal value As String)
                Me.tradeCodeField = value
            End Set
        End Property

        'Start (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (7.4.3.1.1)
        '''<remarks/>
        Public Property ClientDetail() As BaseClientSharedDataType
            Get
                Return Me.clientDetailField
            End Get
            Set(ByVal value As BaseClientSharedDataType)
                Me.clientDetailField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CompanyReg() As String
            Get
                Return Me.companyRegField
            End Get
            Set(ByVal value As String)
                Me.companyRegField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SICCode() As String
            Get
                Return Me.sICCodeField
            End Get
            Set(ByVal value As String)
                Me.sICCodeField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")>
        Public Property TradingSince() As Date
            Get
                Return Me.tradingSinceField
            End Get
            Set(ByVal value As Date)
                Me.tradingSinceField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property TradingSinceSpecified() As Boolean
            Get
                Return Me.tradingSinceFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.tradingSinceFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property WageRoll() As Decimal
            Get
                Return Me.wageRollField
            End Get
            Set(ByVal value As Decimal)
                Me.wageRollField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property WageRollSpecified() As Boolean
            Get
                Return Me.wageRollFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.wageRollFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property TurnoverCode() As String
            Get
                Return Me.turnoverCodeField
            End Get
            Set(ByVal value As String)
                Me.turnoverCodeField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")>
        Public Property FinancialYear() As Date
            Get
                Return Me.financialYearField
            End Get
            Set(ByVal value As Date)
                Me.financialYearField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property FinancialYearSpecified() As Boolean
            Get
                Return Me.financialYearFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.financialYearFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property Salutation() As String
            Get
                Return Me.salutationField
            End Get
            Set(ByVal value As String)
                Me.salutationField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TPS() As Boolean
            Get
                Return Me.tPSField
            End Get
            Set(ByVal value As Boolean)
                Me.tPSField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property TPSSpecified() As Boolean
            Get
                Return Me.tPSFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.tPSFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property MPS() As Boolean
            Get
                Return Me.mPSField
            End Get
            Set(ByVal value As Boolean)
                Me.mPSField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property MPSSpecified() As Boolean
            Get
                Return Me.mPSFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.mPSFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property eMPS() As Boolean
            Get
                Return Me.eMPSField
            End Get
            Set(ByVal value As Boolean)
                Me.eMPSField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property eMPSSpecified() As Boolean
            Get
                Return Me.eMPSFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.eMPSFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property Source() As String
            Get
                Return Me.sourceField
            End Get
            Set(ByVal value As String)
                Me.sourceField = value
            End Set
        End Property
        'Start (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (changes on 08-08-08)
        '''<remarks/>
        Public Property AlternativeId() As String
            Get
                Return Me.alternativeIdField
            End Get
            Set(ByVal value As String)
                Me.alternativeIdField = value
            End Set
        End Property
        'End (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (changes on 08-08-08)
        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            If String.IsNullOrEmpty(CompanyName) Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "CompanyName", "")
            End If
            If ClientDetail IsNot Nothing Then
                ClientDetail.Validate(CObj(oSAMErrorCollection))
            End If

        End Sub

        'End (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (7.4.3.1.1)
    End Class


    '''<remarks/>
    Public Class BaseGetPartySummaryRequestType
        Inherits BaseRequestType

        Private partyKeyField As Integer

        '''<remarks/>
        Public Property PartyKey() As Integer
            Get
                Return Me.partyKeyField
            End Get
            Set(ByVal value As Integer)
                Me.partyKeyField = value
            End Set
        End Property

        Public Property RetrieveAssociates() As Boolean
    End Class

    Public Class BasePartyOTHERType
        Inherits BasePartyType

        Private codeField As String

        Private nameField As String

        Private typeCodeField As String

        Private licenseTypeCodeField As String

        Private licenseNumberField As String

        Private dateOfBirthField As Date

        Private genderField As String

        Private driverStatusCodeField As String

        Private regNumberField As String

        'Start (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (7.5.3.1.1)
        ' differing from Tech Spec
        Private convictionField() As BasePartyOTHERTypeConviction

        Private convictionsField() As BaseConvictionType

        'End (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (7.5.3.1.1)

        Private accidentField() As BasePartyOTHERTypeAccident

        Private activeIndicatorField As Boolean

        Private activeIndicatorFieldSpecified As Boolean

        Private afterHoursIndicatorField As Boolean

        Private afterHoursIndicatorFieldSpecified As Boolean

        Private priorityIndicatorField As Integer

        Private priorityIndicatorFieldSpecified As Boolean

        Private supplierBusinessField() As BasePartyOTHERTypeSupplierBusiness

        Private branchesField() As BasePartyOTHERTypeBranch

        Private isTPASettleDirectlyField As String

        Private _licenseTypeId As Integer
        Public Property LicenseTypeId() As Integer
            Get
                Return _licenseTypeId
            End Get
            Set(ByVal value As Integer)
                _licenseTypeId = value
            End Set
        End Property

        Private _drivingStatusId As Integer
        Public Property DrivingStatusId() As Integer
            Get
                Return _drivingStatusId
            End Get
            Set(ByVal value As Integer)
                _drivingStatusId = value
            End Set
        End Property

        Private _genderId As Integer
        Public Property GenderId() As Integer
            Get
                Return _genderId
            End Get
            Set(ByVal value As Integer)
                _genderId = value
            End Set
        End Property

        Private _genderDescription As String
        Public Property GenderDescription() As String
            Get
                Return _genderDescription
            End Get
            Set(ByVal value As String)
                _genderDescription = value
            End Set
        End Property

        '''<remarks/>
        Public Property Code() As String
            Get
                Return Me.codeField
            End Get
            Set(ByVal value As String)
                Me.codeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Name() As String
            Get
                Return Me.nameField
            End Get
            Set(ByVal value As String)
                Me.nameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TypeCode() As String
            Get
                Return Me.typeCodeField
            End Get
            Set(ByVal value As String)
                Me.typeCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property LicenseTypeCode() As String
            Get
                Return Me.licenseTypeCodeField
            End Get
            Set(ByVal value As String)
                Me.licenseTypeCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property LicenseNumber() As String
            Get
                Return Me.licenseNumberField
            End Get
            Set(ByVal value As String)
                Me.licenseNumberField = value
            End Set
        End Property

        '''<remarks/>
        Public Property DateOfBirth() As Date
            Get
                Return Me.dateOfBirthField
            End Get
            Set(ByVal value As Date)
                Me.dateOfBirthField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Gender() As String
            Get
                Return Me.genderField
            End Get
            Set(ByVal value As String)
                Me.genderField = value
            End Set
        End Property

        '''<remarks/>
        Public Property DriverStatusCode() As String
            Get
                Return Me.driverStatusCodeField
            End Get
            Set(ByVal value As String)
                Me.driverStatusCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RegNumber() As String
            Get
                Return Me.regNumberField
            End Get
            Set(ByVal value As String)
                Me.regNumberField = value
            End Set
        End Property

        Private _TypeId As Integer
        Public Property TypeId() As Integer
            Get
                Return _TypeId
            End Get
            Set(ByVal value As Integer)
                _TypeId = value
            End Set
        End Property

        'Start (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (7.5.3.1.1)
        ' differing from Tech Spec
        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Conviction")>
        Public Property Conviction() As BasePartyOTHERTypeConviction()
            Get
                Return Me.convictionField
            End Get
            Set(ByVal value As BasePartyOTHERTypeConviction())
                Me.convictionField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Convictions")>
        Public Property Convictions() As BaseConvictionType()
            Get
                Return Me.convictionsField
            End Get
            Set(ByVal value As BaseConvictionType())
                Me.convictionsField = value
            End Set
        End Property
        'End (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (7.5.3.1.1)

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Accident")>
        Public Property Accident() As BasePartyOTHERTypeAccident()
            Get
                Return Me.accidentField
            End Get
            Set(ByVal value As BasePartyOTHERTypeAccident())
                Me.accidentField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ActiveIndicator() As Boolean
            Get
                Return Me.activeIndicatorField
            End Get
            Set(ByVal value As Boolean)
                Me.activeIndicatorField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ActiveIndicatorSpecified() As Boolean
            Get
                Return Me.activeIndicatorFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.activeIndicatorFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property AfterHoursIndicator() As Boolean
            Get
                Return Me.afterHoursIndicatorField
            End Get
            Set(ByVal value As Boolean)
                Me.afterHoursIndicatorField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property AfterHoursIndicatorSpecified() As Boolean
            Get
                Return Me.afterHoursIndicatorFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.afterHoursIndicatorFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property PriorityIndicator() As Integer

            Get
                Return Me.priorityIndicatorField
            End Get
            Set(ByVal value As Integer)
                Me.priorityIndicatorField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property PriorityIndicatorSpecified() As Boolean
            Get
                Return Me.priorityIndicatorFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.priorityIndicatorFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("SupplierBusiness")>
        Public Property SupplierBusiness() As BasePartyOTHERTypeSupplierBusiness()
            Get
                Return Me.supplierBusinessField
            End Get
            Set(ByVal value As BasePartyOTHERTypeSupplierBusiness())
                Me.supplierBusinessField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Branches")>
        Public Property Branches() As BasePartyOTHERTypeBranch()
            Get
                Return Me.branchesField
            End Get
            Set(ByVal value As BasePartyOTHERTypeBranch())
                Me.branchesField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("IsTPASettleDirectly")>
        Public Property IsTPASettleDirectly() As String
            Get
                Return Me.isTPASettleDirectlyField
            End Get
            Set(ByVal value As String)
                Me.isTPASettleDirectlyField = value
            End Set
        End Property

        Public Overrides Sub Validate(ByRef oErrorCollection As Object) 'DONE:05-OCT-2006

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            MyBase.Validate(oErrorCollection)

            If Conviction IsNot Nothing Then
                For ConvictionCNT As Integer = 0 To Conviction.GetUpperBound(0)
                    Conviction(ConvictionCNT).Validate(CObj(oSAMErrorCollection))
                Next
            End If

            'Start (Vivek Athalye) - (UIIC WR27 - MTA Amend Client.doc) - ()
            ' this will be used in case of SAMForInsuranceV2
            If Convictions IsNot Nothing Then
                For ConvictionCNT As Integer = 0 To Convictions.GetUpperBound(0)
                    Convictions(ConvictionCNT).Validate(CObj(oSAMErrorCollection))
                Next
            End If
            'End (Vivek Athalye) - (UIIC WR27 - MTA Amend Client.doc) - ()

            If Accident IsNot Nothing Then
                For AccidentCNT As Integer = 0 To Accident.GetUpperBound(0)
                    Accident(AccidentCNT).Validate(CObj(oSAMErrorCollection))
                Next
            End If

        End Sub


    End Class


    '''<remarks/>
    Public Class BasePartyOTHERTypeConviction

        Private convictionKeyField As Integer

        Private typeCodeField As String

        Private statusCodeField As String

        Private descriptionField As String

        Private dateField As Date

        Private fineAmountField As Decimal

        Private fineAmountFieldSpecified As Boolean

        Private sentenceTypeCodeField As String

        Private sentenceDescriptionField As String

        Private sentenceDurationField As Decimal

        Private sentenceDurationFieldSpecified As Boolean

        Private sentenceDurationQualifierField As String

        Private sentenceEffectiveDateField As Date

        Private sentenceEffectiveDateFieldSpecified As Boolean

        Private alcoholLevelField As Decimal

        Private alcoholLevelFieldSpecified As Boolean

        Private alcoholMeasurementMethodField As String

        Private drivingLicencePenaltyPointsField As Decimal

        Private drivingLicencePenaltyPointsFieldSpecified As Boolean

        Private _ProcessingStatus As Integer
        Public Property ProcessingStatus() As Integer
            Get
                Return _ProcessingStatus
            End Get
            Set(ByVal value As Integer)
                _ProcessingStatus = value
            End Set
        End Property

        '''<remarks/>
        Public Property ConvictionKey() As Integer
            Get
                Return Me.convictionKeyField
            End Get
            Set(ByVal value As Integer)
                Me.convictionKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TypeCode() As String
            Get
                Return Me.typeCodeField
            End Get
            Set(ByVal value As String)
                Me.typeCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property StatusCode() As String
            Get
                Return Me.statusCodeField
            End Get
            Set(ByVal value As String)
                Me.statusCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set(ByVal value As String)
                Me.descriptionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property [Date]() As Date
            Get
                Return Me.dateField
            End Get
            Set(ByVal value As Date)
                Me.dateField = value
            End Set
        End Property

        '''<remarks/>
        Public Property FineAmount() As Decimal
            Get
                Return Me.fineAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.fineAmountField = value
            End Set
        End Property

        '''<remarks/>
        Public Property FineAmountSpecified() As Boolean
            Get
                Return Me.fineAmountFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.fineAmountFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property SentenceTypeCode() As String
            Get
                Return Me.sentenceTypeCodeField
            End Get
            Set(ByVal value As String)
                Me.sentenceTypeCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SentenceDescription() As String
            Get
                Return Me.sentenceDescriptionField
            End Get
            Set(ByVal value As String)
                Me.sentenceDescriptionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SentenceDuration() As Decimal
            Get
                Return Me.sentenceDurationField
            End Get
            Set(ByVal value As Decimal)
                Me.sentenceDurationField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SentenceDurationSpecified() As Boolean
            Get
                Return Me.sentenceDurationFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.sentenceDurationFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property SentenceDurationQualifier() As String
            Get
                Return Me.sentenceDurationQualifierField
            End Get
            Set(ByVal value As String)
                Me.sentenceDurationQualifierField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SentenceEffectiveDate() As Date
            Get
                Return Me.sentenceEffectiveDateField
            End Get
            Set(ByVal value As Date)
                Me.sentenceEffectiveDateField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SentenceEffectiveDateSpecified() As Boolean
            Get
                Return Me.sentenceEffectiveDateFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.sentenceEffectiveDateFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property AlcoholLevel() As Decimal
            Get
                Return Me.alcoholLevelField
            End Get
            Set(ByVal value As Decimal)
                Me.alcoholLevelField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AlcoholLevelSpecified() As Boolean
            Get
                Return Me.alcoholLevelFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.alcoholLevelFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property AlcoholMeasurementMethod() As String
            Get
                Return Me.alcoholMeasurementMethodField
            End Get
            Set(ByVal value As String)
                Me.alcoholMeasurementMethodField = value
            End Set
        End Property

        '''<remarks/>
        Public Property DrivingLicencePenaltyPoints() As Decimal
            Get
                Return Me.drivingLicencePenaltyPointsField
            End Get
            Set(ByVal value As Decimal)
                Me.drivingLicencePenaltyPointsField = value
            End Set
        End Property

        '''<remarks/>
        Public Property DrivingLicencePenaltyPointsSpecified() As Boolean
            Get
                Return Me.drivingLicencePenaltyPointsFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.drivingLicencePenaltyPointsFieldSpecified = value
            End Set
        End Property

        Public Overridable Sub Validate(ByRef oErrorCollection As Object) 'DONE:05-OCT-2006

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            If String.IsNullOrEmpty(TypeCode) Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "ConvictionTypeCode", "")
            End If

            If String.IsNullOrEmpty(StatusCode) Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "ConvictionStatusCode", "")
            End If

            If String.IsNullOrEmpty(Description) Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "ConvictionDescription", "")
            End If

            If String.IsNullOrEmpty([Date].ToString()) Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "ConvictionDate", "")
            End If

        End Sub

    End Class


    '''<remarks/>

    Public Class BasePartyOTHERTypeAccident

        Private accidentKeyField As Integer

        Private dateField As Date

        Private descriptionField As String

        Private isAtFaultField As Boolean

        Private _processingStatus As Integer

        Public Property ProcessingStatus() As Integer
            Get
                Return _processingStatus
            End Get
            Set(ByVal value As Integer)
                _processingStatus = value
            End Set
        End Property


        '''<remarks/>
        Public Property AccidentKey() As Integer
            Get
                Return Me.accidentKeyField
            End Get
            Set(ByVal value As Integer)
                Me.accidentKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property [Date]() As Date
            Get
                Return Me.dateField
            End Get
            Set(ByVal value As Date)
                Me.dateField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set(ByVal value As String)
                Me.descriptionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property IsAtFault() As Boolean
            Get
                Return Me.isAtFaultField
            End Get
            Set(ByVal value As Boolean)
                Me.isAtFaultField = value
            End Set
        End Property


        Public Overridable Sub Validate(ByRef oErrorCollection As Object) 'DONE:05-OCT-2006
            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            If String.IsNullOrEmpty([Date].ToString()) Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "AccidentDate", "")
            End If

            If String.IsNullOrEmpty(Description) Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "AccidentDescription", "")
            End If

            'If String.IsNullOrEmpty(IsAtFault.ToString) Then
            '    oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing, _
            '                                        SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
            '                                        "AccidentIsAtFault", "")
            'End If

        End Sub

    End Class

    '''<remarks/>

    Public Class BasePartyOTHERTypeSupplierBusiness

        Private businessCodeField As String

        Private specialityCodeField As String

        Private _businessId As Integer
        Public Property BusinessId() As Integer
            Get
                Return _businessId
            End Get
            Set(ByVal value As Integer)
                _businessId = value
            End Set
        End Property

        Private _specialityId As Integer
        Public Property SpecialityId() As Integer
            Get
                Return _specialityId
            End Get
            Set(ByVal value As Integer)
                _specialityId = value
            End Set
        End Property

        '''<remarks/>
        Public Property BusinessCode() As String
            Get
                Return Me.businessCodeField
            End Get
            Set(ByVal value As String)
                Me.businessCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SpecialityCode() As String
            Get
                Return Me.specialityCodeField
            End Get
            Set(ByVal value As String)
                Me.specialityCodeField = value
            End Set
        End Property
    End Class
    Public Class BasePartyOTHERTypeBranch

        Public Property BranchId() As Integer

        Public Property Description() As String
        '''<remarks/>
    End Class

    '''<remarks/>
    Public Class BaseUpdatePartyRequestType
        Inherits BaseRequestType

        Private subBranchCodeField As String

        Private partyKeyField As Integer

        'Private nameField As String

        'Private forenameField As String

        'Private tradingNameField As String

        'Private partyTypeField As String

        'Private addressesField() As BaseAddressType

        'Private dateOfBirthField As Date

        'Private dateOfBirthFieldSpecified As Boolean

        'Private tPUserCodeField As String

        'Private tPIntroducerField As String

        'Private titleField As String

        'Private maritalStatusCodeField As String

        'Private genderCodeField As String

        'Private initialsField As String

        'Private occupationCodeField As String

        'Private employersBusinessCodeField As String

        'Private employmentStatusCodeField As String

        'Private partyBusinessCodeField As String

        'Private alternativeIdField As String

        'Private contactsField() As BaseContactType

        Private loyaltiesField As System.Xml.XmlElement

        Private associationsField As System.Xml.XmlElement

        Private partyTimestampField() As Byte

        Private PartyField As BasePartyType

        '''<remarks/>
        Public Property Party() As BasePartyType
            Get
                Return Me.PartyField
            End Get
            Set(ByVal value As BasePartyType)
                Me.PartyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SubBranchCode() As String
            Get
                Return Me.subBranchCodeField
            End Get
            Set(ByVal value As String)
                Me.subBranchCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PartyKey() As Integer
            Get
                Return Me.partyKeyField
            End Get
            Set(ByVal value As Integer)
                Me.partyKeyField = value
            End Set
        End Property

        ''''<remarks/>
        'Public Property Name() As String
        '    Get
        '        Return Me.nameField
        '    End Get
        '    Set(ByVal value As String)
        '        Me.nameField = value
        '    End Set
        'End Property

        ''''<remarks/>
        'Public Property Forename() As String
        '    Get
        '        Return Me.forenameField
        '    End Get
        '    Set(ByVal value As String)
        '        Me.forenameField = value
        '    End Set
        'End Property

        ''''<remarks/>
        'Public Property TradingName() As String
        '    Get
        '        Return Me.tradingNameField
        '    End Get
        '    Set(ByVal value As String)
        '        Me.tradingNameField = value
        '    End Set
        'End Property

        ''''<remarks/>
        'Public Property PartyType() As String
        '    Get
        '        Return Me.partyTypeField
        '    End Get
        '    Set(ByVal value As String)
        '        Me.partyTypeField = value
        '    End Set
        'End Property

        ''''<remarks/>
        'Public Property Addresses() As BaseAddressType()
        '    Get
        '        Return Me.addressesField
        '    End Get
        '    Set(ByVal value As BaseAddressType())
        '        Me.addressesField = value
        '    End Set
        'End Property

        ''''<remarks/>
        'Public Property DateOfBirth() As Date
        '    Get
        '        Return Me.dateOfBirthField
        '    End Get
        '    Set(ByVal value As Date)
        '        Me.dateOfBirthField = value
        '    End Set
        'End Property

        ''''<remarks/>
        'Public Property DateOfBirthSpecified() As Boolean
        '    Get
        '        Return Me.dateOfBirthFieldSpecified
        '    End Get
        '    Set(ByVal value As Boolean)
        '        Me.dateOfBirthFieldSpecified = value
        '    End Set
        'End Property

        ''''<remarks/>
        'Public Property TPUserCode() As String
        '    Get
        '        Return Me.tPUserCodeField
        '    End Get
        '    Set(ByVal value As String)
        '        Me.tPUserCodeField = value
        '    End Set
        'End Property

        ''''<remarks/>
        'Public Property TPIntroducer() As String
        '    Get
        '        Return Me.tPIntroducerField
        '    End Get
        '    Set(ByVal value As String)
        '        Me.tPIntroducerField = value
        '    End Set
        'End Property

        ''''<remarks/>
        'Public Property Title() As String
        '    Get
        '        Return Me.titleField
        '    End Get
        '    Set(ByVal value As String)
        '        Me.titleField = value
        '    End Set
        'End Property

        ''''<remarks/>
        'Public Property MaritalStatusCode() As String
        '    Get
        '        Return Me.maritalStatusCodeField
        '    End Get
        '    Set(ByVal value As String)
        '        Me.maritalStatusCodeField = value
        '    End Set
        'End Property

        ''''<remarks/>
        'Public Property GenderCode() As String
        '    Get
        '        Return Me.genderCodeField
        '    End Get
        '    Set(ByVal value As String)
        '        Me.genderCodeField = value
        '    End Set
        'End Property

        ''''<remarks/>
        'Public Property Initials() As String
        '    Get
        '        Return Me.initialsField
        '    End Get
        '    Set(ByVal value As String)
        '        Me.initialsField = value
        '    End Set
        'End Property

        ''''<remarks/>
        'Public Property OccupationCode() As String
        '    Get
        '        Return Me.occupationCodeField
        '    End Get
        '    Set(ByVal value As String)
        '        Me.occupationCodeField = value
        '    End Set
        'End Property

        ''''<remarks/>
        'Public Property EmployersBusinessCode() As String
        '    Get
        '        Return Me.employersBusinessCodeField
        '    End Get
        '    Set(ByVal value As String)
        '        Me.employersBusinessCodeField = value
        '    End Set
        'End Property

        ''''<remarks/>
        'Public Property EmploymentStatusCode() As String
        '    Get
        '        Return Me.employmentStatusCodeField
        '    End Get
        '    Set(ByVal value As String)
        '        Me.employmentStatusCodeField = value
        '    End Set
        'End Property

        ''''<remarks/>
        'Public Property PartyBusinessCode() As String
        '    Get
        '        Return Me.partyBusinessCodeField
        '    End Get
        '    Set(ByVal value As String)
        '        Me.partyBusinessCodeField = value
        '    End Set
        'End Property

        ''''<remarks/>
        'Public Property AlternativeId() As String
        '    Get
        '        Return Me.alternativeIdField
        '    End Get
        '    Set(ByVal value As String)
        '        Me.alternativeIdField = value
        '    End Set
        'End Property

        ''''<remarks/>
        'Public Property Contacts() As BaseContactType()
        '    Get
        '        Return Me.contactsField
        '    End Get
        '    Set(ByVal value As BaseContactType())
        '        Me.contactsField = value
        '    End Set
        'End Property

        '''<remarks/>
        Public Property Loyalties() As System.Xml.XmlElement
            Get
                Return Me.loyaltiesField
            End Get
            Set(ByVal value As System.Xml.XmlElement)
                Me.loyaltiesField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Associations() As System.Xml.XmlElement
            Get
                Return Me.associationsField
            End Get
            Set(ByVal value As System.Xml.XmlElement)
                Me.associationsField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PartyTimestamp() As Byte()
            Get
                Return Me.partyTimestampField
            End Get
            Set(ByVal value As Byte())
                Me.partyTimestampField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class BaseUpdatePartyResponseType
        Inherits BaseResponseType

        Private partyTimestampField() As Byte

        '''<remarks/>
        Public Property PartyTimestamp() As Byte()
            Get
                Return Me.partyTimestampField
            End Get
            Set(ByVal value As Byte())
                Me.partyTimestampField = value
            End Set
        End Property
    End Class

    Public Class BasePartyPCType
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

        'Start (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (7.6.3.1.1)
        Private clientDetailField As BaseClientSharedDataType

        Private tradingNameField As String

        Private secOccupationCodeField As String

        Private secEmployersBusinessCodeField As String

        Private secEmploymentStatusCodeField As EmploymentStatusCodeType

        Private secEmploymentStatusCodeFieldSpecified As Boolean

        Private nationalityCodeField As String

        Private accommodationCodeField As String

        Private lifestyleField() As BasePartyPCTypeLifestyle

        Private salutationField As String

        Private tPSField As Boolean

        Private tPSFieldSpecified As Boolean

        Private mPSField As Boolean

        Private mPSFieldSpecified As Boolean

        Private eMPSField As Boolean

        Private eMPSFieldSpecified As Boolean

        Private sourceField As String

        Private petOwnerField As Boolean

        Private petOwnerFieldSpecified As Boolean
        'End (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (7.6.3.1.1)

        Public Property Surname() As String
            Get
                Return Me.surnameField
            End Get
            Set(ByVal value As String)
                Me.surnameField = value
            End Set
        End Property

        Public Property Forename() As String
            Get
                Return Me.forenameField
            End Get
            Set(ByVal value As String)
                Me.forenameField = value
            End Set
        End Property

        Public Property DateOfBirth() As Date
            Get
                Return Me.dateOfBirthField
            End Get
            Set(ByVal value As Date)
                Me.dateOfBirthField = value
            End Set
        End Property
        Public Property DateOfBirthSpecified() As Boolean
            Get
                Return Me.dateOfBirthFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.dateOfBirthFieldSpecified = value
            End Set
        End Property

        Public Property Title() As String
            Get
                Return Me.titleField
            End Get
            Set(ByVal value As String)
                Me.titleField = value
            End Set
        End Property

        Public Property MaritalStatusCode() As MaritalStatusCodeType
            Get
                Return Me.maritalStatusCodeField
            End Get
            Set(ByVal value As MaritalStatusCodeType)
                Me.maritalStatusCodeField = value
            End Set
        End Property

        Public Property MaritalStatusCodeSpecified() As Boolean
            Get
                Return Me.maritalStatusCodeFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.maritalStatusCodeFieldSpecified = value
            End Set
        End Property

        Public Property GenderCode() As String
            Get
                Return Me.genderCodeField
            End Get
            Set(ByVal value As String)
                Me.genderCodeField = value
            End Set
        End Property

        Public Property Initials() As String
            Get
                Return Me.initialsField
            End Get
            Set(ByVal value As String)
                Me.initialsField = value
            End Set
        End Property

        Public Property OccupationCode() As String
            Get
                Return Me.occupationCodeField
            End Get
            Set(ByVal value As String)
                Me.occupationCodeField = value
            End Set
        End Property

        Public Property EmployersBusinessCode() As String
            Get
                Return Me.employersBusinessCodeField
            End Get
            Set(ByVal value As String)
                Me.employersBusinessCodeField = value
            End Set
        End Property

        Public Property EmploymentStatusCode() As EmploymentStatusCodeType
            Get
                Return Me.employmentStatusCodeField
            End Get
            Set(ByVal value As EmploymentStatusCodeType)
                Me.employmentStatusCodeField = value
            End Set
        End Property

        Public Property EmploymentStatusCodeSpecified() As Boolean
            Get
                Return Me.employmentStatusCodeFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.employmentStatusCodeFieldSpecified = value
            End Set
        End Property

        Public Property AlternativeId() As String
            Get
                Return Me.alternativeIdField
            End Get
            Set(ByVal value As String)
                Me.alternativeIdField = value
            End Set
        End Property

        'Start (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (7.6.3.1.1)
        '''<remarks/>
        Public Property ClientDetail() As BaseClientSharedDataType
            Get
                Return Me.clientDetailField
            End Get
            Set(ByVal value As BaseClientSharedDataType)
                Me.clientDetailField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TradingName() As String
            Get
                Return Me.tradingNameField
            End Get
            Set(ByVal value As String)
                Me.tradingNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SecOccupationCode() As String
            Get
                Return Me.secOccupationCodeField
            End Get
            Set(ByVal value As String)
                Me.secOccupationCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SecEmployersBusinessCode() As String
            Get
                Return Me.secEmployersBusinessCodeField
            End Get
            Set(ByVal value As String)
                Me.secEmployersBusinessCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SecEmploymentStatusCode() As EmploymentStatusCodeType
            Get
                Return Me.secEmploymentStatusCodeField
            End Get
            Set(ByVal value As EmploymentStatusCodeType)
                Me.secEmploymentStatusCodeField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property SecEmploymentStatusCodeSpecified() As Boolean
            Get
                Return Me.secEmploymentStatusCodeFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.secEmploymentStatusCodeFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property NationalityCode() As String
            Get
                Return Me.nationalityCodeField
            End Get
            Set(ByVal value As String)
                Me.nationalityCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AccommodationCode() As String
            Get
                Return Me.accommodationCodeField
            End Get
            Set(ByVal value As String)
                Me.accommodationCodeField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Lifestyle")>
        Public Property Lifestyle() As BasePartyPCTypeLifestyle()
            Get
                Return Me.lifestyleField
            End Get
            Set(ByVal value As BasePartyPCTypeLifestyle())
                Me.lifestyleField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Salutation() As String
            Get
                Return Me.salutationField
            End Get
            Set(ByVal value As String)
                Me.salutationField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TPS() As Boolean
            Get
                Return Me.tPSField
            End Get
            Set(ByVal value As Boolean)
                Me.tPSField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property TPSSpecified() As Boolean
            Get
                Return Me.tPSFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.tPSFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property MPS() As Boolean
            Get
                Return Me.mPSField
            End Get
            Set(ByVal value As Boolean)
                Me.mPSField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property MPSSpecified() As Boolean
            Get
                Return Me.mPSFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.mPSFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property eMPS() As Boolean
            Get
                Return Me.eMPSField
            End Get
            Set(ByVal value As Boolean)
                Me.eMPSField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property eMPSSpecified() As Boolean
            Get
                Return Me.eMPSFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.eMPSFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property Source() As String
            Get
                Return Me.sourceField
            End Get
            Set(ByVal value As String)
                Me.sourceField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PetOwner() As Boolean
            Get
                Return Me.petOwnerField
            End Get
            Set(ByVal value As Boolean)
                Me.petOwnerField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property PetOwnerSpecified() As Boolean
            Get
                Return Me.petOwnerFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.petOwnerFieldSpecified = value
            End Set
        End Property
        'End (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (7.6.3.1.1)

        'Start (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (7.6.4.1)
        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            MyBase.Validate(CObj(oSAMErrorCollection))

            If String.IsNullOrEmpty(Surname) Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "Surname", "")
            End If
            If String.IsNullOrEmpty(Forename) Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "Forename", "")
            End If

            If String.IsNullOrEmpty(Title) Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "Title", "")
            End If

            If String.IsNullOrEmpty(Initials) Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "Initials", "")
            End If


            'Start (Vivek Athalye) - (UIIC WR27 - MTA Amend Client.doc) - ()
            If Lifestyle IsNot Nothing Then
                For iLifestyleCNT As Integer = 0 To Lifestyle.GetUpperBound(0)
                    Lifestyle(iLifestyleCNT).Validate(CObj(oSAMErrorCollection))
                Next
            End If
            'End (Vivek Athalye) - (UIIC WR27 - MTA Amend Client.doc) - ()
            'Start Praveen - As said by Rahul
            If ClientDetail IsNot Nothing Then
                ClientDetail.Validate(CObj(oSAMErrorCollection))
            End If
            'End Praveen - As said by Rahul
        End Sub
        'End (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (7.6.4.1)
    End Class

    'Start (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (7.6.3.1.2)
    Partial Public Class BasePartyPCTypeLifestyle

        Private lifestyleKeyField As Integer

        Private nameField As String

        Private dateOfBirthField As Date

        Private dateOfBirthFieldSpecified As Boolean

        Private categoryCodeField As String

        Private genderCodeField As GenderCodeType

        Private genderCodeFieldSpecified As Boolean

        Private occupationCodeField As String

        Private secOccupationCodeField As String

        Private smokerField As Boolean

        Private smokerFieldSpecified As Boolean

        Private _ProcessingStatus As Integer
        Public Property ProcessingStatus() As Integer
            Get
                Return _ProcessingStatus
            End Get
            Set(ByVal value As Integer)
                _ProcessingStatus = value
            End Set
        End Property

        '''<remarks/>
        Public Property LifestyleKey() As Integer
            Get
                Return Me.lifestyleKeyField
            End Get
            Set(ByVal value As Integer)
                Me.lifestyleKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Name() As String
            Get
                Return Me.nameField
            End Get
            Set(ByVal value As String)
                Me.nameField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")>
        Public Property DateOfBirth() As Date
            Get
                Return Me.dateOfBirthField
            End Get
            Set(ByVal value As Date)
                Me.dateOfBirthField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DateOfBirthSpecified() As Boolean
            Get
                Return Me.dateOfBirthFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.dateOfBirthFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property CategoryCode() As String
            Get
                Return Me.categoryCodeField
            End Get
            Set(ByVal value As String)
                Me.categoryCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property GenderCode() As GenderCodeType
            Get
                Return Me.genderCodeField
            End Get
            Set(ByVal value As GenderCodeType)
                Me.genderCodeField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property GenderCodeSpecified() As Boolean
            Get
                Return Me.genderCodeFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.genderCodeFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property OccupationCode() As String
            Get
                Return Me.occupationCodeField
            End Get
            Set(ByVal value As String)
                Me.occupationCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SecOccupationCode() As String
            Get
                Return Me.secOccupationCodeField
            End Get
            Set(ByVal value As String)
                Me.secOccupationCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Smoker() As Boolean
            Get
                Return Me.smokerField
            End Get
            Set(ByVal value As Boolean)
                Me.smokerField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property SmokerSpecified() As Boolean
            Get
                Return Me.smokerFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.smokerFieldSpecified = value
            End Set
        End Property
        Public Overridable Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            '20080525
            ' Vivek: this fails in case of addition of new Loyalty Scheme.
            'If LifestyleKey = 0 Then
            '    oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing, _
            '                                            SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
            '                                            "LifestyleKey")
            'End If



        End Sub
    End Class
    'End (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (7.6.3.1.2)

    '''<remarks/>
    Public Class BasePartyType
        Inherits BaseRequestType

        Private addressesField() As BaseAddressType

        Private tPUserCodeField As String

        Private tPIntroducerField As String

        Private contactsField() As BaseContactType

        Private accountExecutiveField As String

        Private currencyField As String

        Private taxNumberField As String

        Private domiciledForTaxField As Boolean

        Private domiciledForTaxFieldSpecified As Boolean

        Private taxExemptField As Boolean

        Private taxExemptFieldSpecified As Boolean

        Private taxPercentageField As Decimal

        Private taxPercentageFieldSpecified As Boolean

        Private fileCodeField As String

        Private xMLDatasetField As String

        Private agentField As String
        'Start (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (changes on 08-08-08)
        Private accountExecutiveCodeField As String

        Private subBranchCodeField As String
        'Start (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (changes on 08-08-08)

        '''<remarks/>
        Public Property XMLDataset() As String
            Get
                Return Me.xMLDatasetField
            End Get
            Set(ByVal value As String)
                Me.xMLDatasetField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Addresses() As BaseAddressType()
            Get
                Return Me.addressesField
            End Get
            Set(ByVal value As BaseAddressType())
                Me.addressesField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TPUserCode() As String
            Get
                Return Me.tPUserCodeField
            End Get
            Set(ByVal value As String)
                Me.tPUserCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TPIntroducer() As String
            Get
                Return Me.tPIntroducerField
            End Get
            Set(ByVal value As String)
                Me.tPIntroducerField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Contacts() As BaseContactType()
            Get
                Return Me.contactsField
            End Get
            Set(ByVal value As BaseContactType())
                Me.contactsField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AccountExecutive() As String
            Get
                Return Me.accountExecutiveField
            End Get
            Set(ByVal value As String)
                Me.accountExecutiveField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Currency() As String
            Get
                Return Me.currencyField
            End Get
            Set(ByVal value As String)
                Me.currencyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TaxNumber() As String
            Get
                Return Me.taxNumberField
            End Get
            Set(ByVal value As String)
                Me.taxNumberField = value
            End Set
        End Property

        '''<remarks/>
        Public Property DomiciledForTax() As Boolean
            Get
                Return Me.domiciledForTaxField
            End Get
            Set(ByVal value As Boolean)
                Me.domiciledForTaxField = value
            End Set
        End Property

        '''<remarks/>

        Public Property DomiciledForTaxSpecified() As Boolean
            Get
                Return Me.domiciledForTaxFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.domiciledForTaxFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property TaxExempt() As Boolean
            Get
                Return Me.taxExemptField
            End Get
            Set(ByVal value As Boolean)
                Me.taxExemptField = value
            End Set
        End Property

        '''<remarks/>

        Public Property TaxExemptSpecified() As Boolean
            Get
                Return Me.taxExemptFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.taxExemptFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property TaxPercentage() As Decimal
            Get
                Return Me.taxPercentageField
            End Get
            Set(ByVal value As Decimal)
                Me.taxPercentageField = value
            End Set
        End Property

        '''<remarks/>

        Public Property TaxPercentageSpecified() As Boolean
            Get
                Return Me.taxPercentageFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.taxPercentageFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property FileCode() As String
            Get
                Return Me.fileCodeField
            End Get
            Set(ByVal value As String)
                Me.fileCodeField = value
            End Set
        End Property

        Public Property Agent() As String
            Get
                Return Me.agentField
            End Get
            Set(ByVal value As String)
                Me.agentField = value
            End Set
        End Property
        'Start (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (changes on 08-08-08)
        '''<remarks/>
        Public Property AccountExecutiveCode() As String
            Get
                Return Me.accountExecutiveCodeField
            End Get
            Set(ByVal value As String)
                Me.accountExecutiveCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SubBranchCode() As String
            Get
                Return Me.subBranchCodeField
            End Get
            Set(ByVal value As String)
                Me.subBranchCodeField = value
            End Set
        End Property
        'End (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (changes on 08-08-08)
    End Class

    Public Class BaseUpdatePartyRiskRequestType
        Inherits BaseRequestType

        Private partyKeyField As Integer

        Private xMLDataSetField As String

        Private timeStampField() As Byte

        '''<remarks/>
        Public Property PartyKey() As Integer
            Get
                Return Me.partyKeyField
            End Get
            Set(ByVal value As Integer)
                Me.partyKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property XMLDataSet() As String
            Get
                Return Me.xMLDataSetField
            End Get
            Set(ByVal value As String)
                Me.xMLDataSetField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="base64Binary")>
        Public Property TimeStamp() As Byte()
            Get
                Return Me.timeStampField
            End Get
            Set(ByVal value As Byte())
                Me.timeStampField = value
            End Set
        End Property
    End Class

    Public Class BaseUpdatePartyRiskResponseType
        Inherits BaseResponseType

        Private xMLDataSetField As String

        Private timeStampField() As Byte

        '''<remarks/>
        Public Property XMLDataSet() As String
            Get
                Return Me.xMLDataSetField
            End Get
            Set(ByVal value As String)
                Me.xMLDataSetField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="base64Binary")>
        Public Property TimeStamp() As Byte()
            Get
                Return Me.timeStampField
            End Get
            Set(ByVal value As Byte())
                Me.timeStampField = value
            End Set
        End Property
    End Class

    'Start (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (7.2.3.1.1)
    Partial Public Class BaseClientSharedDataType

        Private shortNameField As String
        Private serviceLevelCodeField As String

        Private areaCodeField As String

        Private leadAgentKeyField As Integer

        Private leadAgentKeyFieldSpecified As Boolean

        Private isProspectField As Boolean

        Private isProspectFieldSpecified As Boolean

        Private isAgentField As Boolean

        Private isAgentFieldSpecified As Boolean

        Private correspondenceCodeField As String

        Private paymentCodeField As String

        Private reminderCodeField As String

        Private paymentTermCodeField As String

        Private renewalStopCodeField As String

        Private loyaltyNumberField As String

        Private seasonalGiftCodeField As String

        Private associatesField() As BaseAssociateType

        Private convictionsField() As BaseConvictionType

        Private countyCourtJudgmentsField As Decimal

        Private countyCourtJudgmentsFieldSpecified As Boolean

        Private loyaltySchemeField() As BaseClientSharedDataTypeLoyaltyScheme

        Private agentReferenceField As String

        Private currentIntermediaryKeyField As Integer

        Private currentIntermediaryKeyFieldSpecified As Boolean

        Private strengthCodeField As String

        Private statusCodeField As String

        Private previousInsurerKeyField As Integer

        Private previousInsurerKeyFieldSpecified As Boolean

        Private previousBrokerKeyField As Integer

        Private previousBrokerKeyFieldSpecified As Boolean

        Private prospectPoliciesField() As BaseClientSharedDataTypeProspectPolicies

        'Start (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (changes on 08-08-08)
        Private currentIntermediaryNameField As String

        Private leadAgentCodeField As String

        Private leadAgentNameField As String

        Private previousInsurerCodeField As String

        Private previousInsurerNameField As String

        Private previousBrokerCodeField As String

        Private previousBrokerNameField As String

        Private accountBalanceField As Decimal

        Private accountBalanceFieldSpecified As Boolean

        Private yearToDateTurnoverField As Decimal

        Private yearToDateTurnoverFieldSpecified As Boolean

        Private lastYearTurnoverField As Decimal

        Private lastYearTurnoverFieldSpecified As Boolean

        Private sResolvedName As String
        'End (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (changes on 08-08-08)

        '''<remarks/>
        Public Property ShortName() As String
            Get
                Return Me.shortNameField
            End Get
            Set(ByVal value As String)
                Me.shortNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ServiceLevelCode() As String
            Get
                Return Me.serviceLevelCodeField
            End Get
            Set(ByVal value As String)
                Me.serviceLevelCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AreaCode() As String
            Get
                Return Me.areaCodeField
            End Get
            Set(ByVal value As String)
                Me.areaCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property LeadAgentKey() As Integer
            Get
                Return Me.leadAgentKeyField
            End Get
            Set(ByVal value As Integer)
                Me.leadAgentKeyField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property LeadAgentKeySpecified() As Boolean
            Get
                Return Me.leadAgentKeyFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.leadAgentKeyFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property IsProspect() As Boolean
            Get
                Return Me.isProspectField
            End Get
            Set(ByVal value As Boolean)
                Me.isProspectField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property IsProspectSpecified() As Boolean
            Get
                Return Me.isProspectFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.isProspectFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property IsAgent() As Boolean
            Get
                Return Me.isAgentField
            End Get
            Set(ByVal value As Boolean)
                Me.isAgentField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property IsAgentSpecified() As Boolean
            Get
                Return Me.isAgentFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.isAgentFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property CorrespondenceCode() As String
            Get
                Return Me.correspondenceCodeField
            End Get
            Set(ByVal value As String)
                Me.correspondenceCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PaymentCode() As String
            Get
                Return Me.paymentCodeField
            End Get
            Set(ByVal value As String)
                Me.paymentCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ReminderCode() As String
            Get
                Return Me.reminderCodeField
            End Get
            Set(ByVal value As String)
                Me.reminderCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PaymentTermCode() As String
            Get
                Return Me.paymentTermCodeField
            End Get
            Set(ByVal value As String)
                Me.paymentTermCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RenewalStopCode() As String
            Get
                Return Me.renewalStopCodeField
            End Get
            Set(ByVal value As String)
                Me.renewalStopCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property LoyaltyNumber() As String
            Get
                Return Me.loyaltyNumberField
            End Get
            Set(ByVal value As String)
                Me.loyaltyNumberField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SeasonalGiftCode() As String
            Get
                Return Me.seasonalGiftCodeField
            End Get
            Set(ByVal value As String)
                Me.seasonalGiftCodeField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Associates")>
        Public Property Associates() As BaseAssociateType()
            Get
                Return Me.associatesField
            End Get
            Set(ByVal value As BaseAssociateType())
                Me.associatesField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Convictions")>
        Public Property Convictions() As BaseConvictionType()
            Get
                Return Me.convictionsField
            End Get
            Set(ByVal value As BaseConvictionType())
                Me.convictionsField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CountyCourtJudgments() As Decimal
            Get
                Return Me.countyCourtJudgmentsField
            End Get
            Set(ByVal value As Decimal)
                Me.countyCourtJudgmentsField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property CountyCourtJudgmentsSpecified() As Boolean
            Get
                Return Me.countyCourtJudgmentsFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.countyCourtJudgmentsFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("LoyaltyScheme")>
        Public Property LoyaltyScheme() As BaseClientSharedDataTypeLoyaltyScheme()
            Get
                Return Me.loyaltySchemeField
            End Get
            Set(ByVal value As BaseClientSharedDataTypeLoyaltyScheme())
                Me.loyaltySchemeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AgentReference() As String
            Get
                Return Me.agentReferenceField
            End Get
            Set(ByVal value As String)
                Me.agentReferenceField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CurrentIntermediaryKey() As Integer
            Get
                Return Me.currentIntermediaryKeyField
            End Get
            Set(ByVal value As Integer)
                Me.currentIntermediaryKeyField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property CurrentIntermediaryKeySpecified() As Boolean
            Get
                Return Me.currentIntermediaryKeyFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.currentIntermediaryKeyFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property StrengthCode() As String
            Get
                Return Me.strengthCodeField
            End Get
            Set(ByVal value As String)
                Me.strengthCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property StatusCode() As String
            Get
                Return Me.statusCodeField
            End Get
            Set(ByVal value As String)
                Me.statusCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PreviousInsurerKey() As Integer
            Get
                Return Me.previousInsurerKeyField
            End Get
            Set(ByVal value As Integer)
                Me.previousInsurerKeyField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property PreviousInsurerKeySpecified() As Boolean
            Get
                Return Me.previousInsurerKeyFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.previousInsurerKeyFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property PreviousBrokerKey() As Integer
            Get
                Return Me.previousBrokerKeyField
            End Get
            Set(ByVal value As Integer)
                Me.previousBrokerKeyField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property PreviousBrokerKeySpecified() As Boolean
            Get
                Return Me.previousBrokerKeyFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.previousBrokerKeyFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("ProspectPolicies")>
        Public Property ProspectPolicies() As BaseClientSharedDataTypeProspectPolicies()
            Get
                Return Me.prospectPoliciesField
            End Get
            Set(ByVal value As BaseClientSharedDataTypeProspectPolicies())
                Me.prospectPoliciesField = value
            End Set
        End Property
        'Start (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (changes on 08-08-08)
        '''<remarks/>
        Public Property CurrentIntermediaryName() As String
            Get
                Return Me.currentIntermediaryNameField
            End Get
            Set(ByVal value As String)
                Me.currentIntermediaryNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property LeadAgentCode() As String
            Get
                Return Me.leadAgentCodeField
            End Get
            Set(ByVal value As String)
                Me.leadAgentCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property LeadAgentName() As String
            Get
                Return Me.leadAgentNameField
            End Get
            Set(ByVal value As String)
                Me.leadAgentNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PreviousInsurerCode() As String
            Get
                Return Me.previousInsurerCodeField
            End Get
            Set(ByVal value As String)
                Me.previousInsurerCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PreviousInsurerName() As String
            Get
                Return Me.previousInsurerNameField
            End Get
            Set(ByVal value As String)
                Me.previousInsurerNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PreviousBrokerCode() As String
            Get
                Return Me.previousBrokerCodeField
            End Get
            Set(ByVal value As String)
                Me.previousBrokerCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PreviousBrokerName() As String
            Get
                Return Me.previousBrokerNameField
            End Get
            Set(ByVal value As String)
                Me.previousBrokerNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AccountBalance() As Decimal
            Get
                Return Me.accountBalanceField
            End Get
            Set(ByVal value As Decimal)
                Me.accountBalanceField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property AccountBalanceSpecified() As Boolean
            Get
                Return Me.accountBalanceFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.accountBalanceFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property YearToDateTurnover() As Decimal
            Get
                Return Me.yearToDateTurnoverField
            End Get
            Set(ByVal value As Decimal)
                Me.yearToDateTurnoverField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property YearToDateTurnoverSpecified() As Boolean
            Get
                Return Me.yearToDateTurnoverFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.yearToDateTurnoverFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property LastYearTurnover() As Decimal
            Get
                Return Me.lastYearTurnoverField
            End Get
            Set(ByVal value As Decimal)
                Me.lastYearTurnoverField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property LastYearTurnoverSpecified() As Boolean
            Get
                Return Me.lastYearTurnoverFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.lastYearTurnoverFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property ResolvedName() As String
            Get
                Return Me.sResolvedName
            End Get
            Set(ByVal value As String)
                Me.sResolvedName = value
            End Set
        End Property
        'End (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (changes on 08-08-08)

        'Start (Vivek Athalye) - (UIIC WR27 - MTA Amend Client.doc) - ()
        Public Overridable Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            'MyBase.Validate(oErrorCollection)

            If LoyaltyScheme IsNot Nothing Then
                For iLoyaltySchemeCNT As Integer = 0 To LoyaltyScheme.GetUpperBound(0)
                    LoyaltyScheme(iLoyaltySchemeCNT).Validate(CObj(oSAMErrorCollection))
                Next
            End If

            If ProspectPolicies IsNot Nothing Then
                For iProspectPoliciesCNT As Integer = 0 To ProspectPolicies.GetUpperBound(0)
                    ProspectPolicies(iProspectPoliciesCNT).Validate(CObj(oSAMErrorCollection))
                Next
            End If

            If Associates IsNot Nothing Then
                For iAssociatesCNT As Integer = 0 To Associates.GetUpperBound(0)
                    Associates(iAssociatesCNT).Validate(CObj(oSAMErrorCollection))
                Next
            End If

            If Convictions IsNot Nothing Then
                For iConvictionsCNT As Integer = 0 To Convictions.GetUpperBound(0)
                    Convictions(iConvictionsCNT).Validate(CObj(oSAMErrorCollection))
                Next
            End If


        End Sub
        'End (Vivek Athalye) - (UIIC WR27 - MTA Amend Client.doc) - ()

    End Class
    'End (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (7.2.3.1.1)

    'Start (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (7.2.3.1.1)
    Partial Public Class BaseClientSharedDataTypeLoyaltyScheme

        Private loyaltySchemeKeyField As Integer

        Private loyaltySchemeCodeField As String

        Private membershipNumberField As String

        Private otherReferenceField As String

        Private startDateField As Date

        Private endDateField As Date

        Private endDateFieldSpecified As Boolean

        Private mainMemberField As String

        Private activeField As Boolean

        Private activeFieldSpecified As Boolean

        Private _ProcessingStatus As Integer
        Public Property ProcessingStatus() As Integer
            Get
                Return _ProcessingStatus
            End Get
            Set(ByVal value As Integer)
                _ProcessingStatus = value
            End Set
        End Property

        '''<remarks/>
        Public Property LoyaltySchemeKey() As Integer
            Get
                Return Me.loyaltySchemeKeyField
            End Get
            Set(ByVal value As Integer)
                Me.loyaltySchemeKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property LoyaltySchemeCode() As String
            Get
                Return Me.loyaltySchemeCodeField
            End Get
            Set(ByVal value As String)
                Me.loyaltySchemeCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property MembershipNumber() As String
            Get
                Return Me.membershipNumberField
            End Get
            Set(ByVal value As String)
                Me.membershipNumberField = value
            End Set
        End Property

        '''<remarks/>
        Public Property OtherReference() As String
            Get
                Return Me.otherReferenceField
            End Get
            Set(ByVal value As String)
                Me.otherReferenceField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")>
        Public Property StartDate() As Date
            Get
                Return Me.startDateField
            End Get
            Set(ByVal value As Date)
                Me.startDateField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")>
        Public Property EndDate() As Date
            Get
                Return Me.endDateField
            End Get
            Set(ByVal value As Date)
                Me.endDateField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property EndDateSpecified() As Boolean
            Get
                Return Me.endDateFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.endDateFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property MainMember() As String
            Get
                Return Me.mainMemberField
            End Get
            Set(ByVal value As String)
                Me.mainMemberField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Active() As Boolean
            Get
                Return Me.activeField
            End Get
            Set(ByVal value As Boolean)
                Me.activeField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property ActiveSpecified() As Boolean
            Get
                Return Me.activeFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.activeFieldSpecified = value
            End Set
        End Property

        Public Overridable Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)


            '20080525
            ' Vivek: this fails in case of addition of new Loyalty Scheme.
            'If LoyaltySchemeKey = 0 Then
            '    oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing, _
            '                                            SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
            '                                            "LoyaltySchemeKey")
            'End If



            If String.IsNullOrEmpty(MembershipNumber) Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "MembershipNumber", "")
            End If
            'To be checked for StartDate
            If String.IsNullOrEmpty(StartDate.ToString()) Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "StartDate", "")
            End If



        End Sub
    End Class
    'End (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (7.2.3.1.1)

    'Start (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (7.2.3.1.1)
    Partial Public Class BaseClientSharedDataTypeProspectPolicies

        Private prospectPolicyKeyField As Integer

        Private prospectTypeCodeField As String

        Private renewalDateField As Date

        Private renewalDateFieldSpecified As Boolean

        Private timesQuotedField As Decimal

        Private timesQuotedFieldSpecified As Boolean

        Private targetPremiumField As Decimal

        Private targetPremiumFieldSpecified As Boolean

        Private _ProcessingStatus As Integer
        Public Property ProcessingStatus() As Integer
            Get
                Return _ProcessingStatus
            End Get
            Set(ByVal value As Integer)
                _ProcessingStatus = value
            End Set
        End Property

        '''<remarks/>
        Public Property ProspectPolicyKey() As Integer
            Get
                Return Me.prospectPolicyKeyField
            End Get
            Set(ByVal value As Integer)
                Me.prospectPolicyKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ProspectTypeCode() As String
            Get
                Return Me.prospectTypeCodeField
            End Get
            Set(ByVal value As String)
                Me.prospectTypeCodeField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")>
        Public Property RenewalDate() As Date
            Get
                Return Me.renewalDateField
            End Get
            Set(ByVal value As Date)
                Me.renewalDateField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property RenewalDateSpecified() As Boolean
            Get
                Return Me.renewalDateFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.renewalDateFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property TimesQuoted() As Decimal
            Get
                Return Me.timesQuotedField
            End Get
            Set(ByVal value As Decimal)
                Me.timesQuotedField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property TimesQuotedSpecified() As Boolean
            Get
                Return Me.timesQuotedFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.timesQuotedFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property TargetPremium() As Decimal
            Get
                Return Me.targetPremiumField
            End Get
            Set(ByVal value As Decimal)
                Me.targetPremiumField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property TargetPremiumSpecified() As Boolean
            Get
                Return Me.targetPremiumFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.targetPremiumFieldSpecified = value
            End Set
        End Property
        Public Overridable Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)


            '20080525
            ' Vivek: this fails in case of addition of new Prospect Policies.
            'If ProspectPolicyKey = 0 Then
            '    oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing, _
            '                                            SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
            '                                            "ProspectPolicyKey")
            'End If


        End Sub
    End Class
    'End (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (7.2.3.1.1)

    'Start (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (7.1.3.1.1)
    Partial Public Class BaseAssociateType

        Private clientKeyField As Integer

        Private associateKeyField As Integer

        Private relationshipCodeField As String

        Private relationshipDescriptionField As String
        'Start (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (changes on 08-08-08)
        Private associateCodeField As String

        Private associateNameField As String

        Private AccountBalanceField As Decimal

        Private ClaimIncurredField As Decimal

        Private CurrencyCodeField As String

        'End (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (changes on 08-08-08) 


        Private _ProcessingStatus As Integer
        Public Property ProcessingStatus() As Integer
            Get
                Return _ProcessingStatus
            End Get
            Set(ByVal value As Integer)
                _ProcessingStatus = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClientKey() As Integer
            Get
                Return Me.clientKeyField
            End Get
            Set(ByVal value As Integer)
                Me.clientKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AssociateKey() As Integer
            Get
                Return Me.associateKeyField
            End Get
            Set(ByVal value As Integer)
                Me.associateKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RelationshipCode() As String
            Get
                Return Me.relationshipCodeField
            End Get
            Set(ByVal value As String)
                Me.relationshipCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RelationshipDescription() As String
            Get
                Return Me.relationshipDescriptionField
            End Get
            Set(ByVal value As String)
                Me.relationshipDescriptionField = value
            End Set
        End Property
        'Start (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (changes on 08-08-08)
        '''<remarks/>
        Public Property AssociateCode() As String
            Get
                Return Me.associateCodeField
            End Get
            Set(ByVal value As String)
                Me.associateCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AssociateName() As String
            Get
                Return Me.associateNameField
            End Get
            Set(ByVal value As String)
                Me.associateNameField = value
            End Set
        End Property
        Public Property AccountBalance() As Decimal
            Get
                Return Me.AccountBalanceField
            End Get
            Set(ByVal value As Decimal)
                Me.AccountBalanceField = value
            End Set
        End Property

        Public Property ClaimIncurred() As Decimal
            Get
                Return Me.ClaimIncurredField
            End Get
            Set(ByVal value As Decimal)
                Me.ClaimIncurredField = value
            End Set
        End Property
        Public Property CurrencyCode() As String
            Get
                Return Me.CurrencyCodeField
            End Get
            Set(ByVal value As String)
                Me.CurrencyCodeField = value
            End Set
        End Property
        'End (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (changes on 08-08-08)

        Public Overridable Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)


            If ClientKey = 0 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                        SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                        "ClientKey")
            End If

            '20080605
            ' associate key is a foreign key, so we should validate it
            If AssociateKey = 0 Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                        SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                        "AssociateKey")
            End If

            If String.IsNullOrEmpty(RelationshipCode) Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "RelationshipCode", "")
            End If

            If String.IsNullOrEmpty(RelationshipDescription) Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "RelationshipDescription", "")
            End If



        End Sub
    End Class
    'End (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (7.1.3.1.1)

    'Start (Girija Chokkalingam) - (UIIC WR27 - MTA Amend Client.doc) - (7.3.3.1.1)
    Partial Public Class BaseConvictionType

        Private convictionKeyField As Integer

        Private typeCodeField As String

        Private statusCodeField As String

        Private descriptionField As String

        Private dateField As Date

        Private fineAmountField As Decimal

        Private fineAmountFieldSpecified As Boolean

        Private sentenceTypeCodeField As String

        Private sentenceDescriptionField As String

        Private sentenceDurationField As Decimal

        Private sentenceDurationFieldSpecified As Boolean

        Private sentenceDurationQualifierField As String

        Private sentenceEffectiveDateField As Date

        Private sentenceEffectiveDateFieldSpecified As Boolean

        Private alcoholLevelField As Decimal

        Private alcoholLevelFieldSpecified As Boolean

        Private alcoholMeasurementMethodField As String

        Private drivingLicensePenaltyPointsField As Decimal

        Private drivingLicensePenaltyPointsFieldSpecified As Boolean

        Private _ProcessingStatus As Integer
        Public Property ProcessingStatus() As Integer
            Get
                Return _ProcessingStatus
            End Get
            Set(ByVal value As Integer)
                _ProcessingStatus = value
            End Set
        End Property


        '''<remarks/>
        Public Property ConvictionKey() As Integer
            Get
                Return Me.convictionKeyField
            End Get
            Set(ByVal value As Integer)
                Me.convictionKeyField = value
            End Set
        End Property

        '''<remarks/>
        Public Property TypeCode() As String
            Get
                Return Me.typeCodeField
            End Get
            Set(ByVal value As String)
                Me.typeCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property StatusCode() As String
            Get
                Return Me.statusCodeField
            End Get
            Set(ByVal value As String)
                Me.statusCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Description() As String
            Get
                Return Me.descriptionField
            End Get
            Set(ByVal value As String)
                Me.descriptionField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")>
        Public Property [Date]() As Date
            Get
                Return Me.dateField
            End Get
            Set(ByVal value As Date)
                Me.dateField = value
            End Set
        End Property

        '''<remarks/>
        Public Property FineAmount() As Decimal
            Get
                Return Me.fineAmountField
            End Get
            Set(ByVal value As Decimal)
                Me.fineAmountField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property FineAmountSpecified() As Boolean
            Get
                Return Me.fineAmountFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.fineAmountFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property SentenceTypeCode() As String
            Get
                Return Me.sentenceTypeCodeField
            End Get
            Set(ByVal value As String)
                Me.sentenceTypeCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SentenceDescription() As String
            Get
                Return Me.sentenceDescriptionField
            End Get
            Set(ByVal value As String)
                Me.sentenceDescriptionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property SentenceDuration() As Decimal
            Get
                Return Me.sentenceDurationField
            End Get
            Set(ByVal value As Decimal)
                Me.sentenceDurationField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property SentenceDurationSpecified() As Boolean
            Get
                Return Me.sentenceDurationFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.sentenceDurationFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property SentenceDurationQualifier() As String
            Get
                Return Me.sentenceDurationQualifierField
            End Get
            Set(ByVal value As String)
                Me.sentenceDurationQualifierField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute(DataType:="date")>
        Public Property SentenceEffectiveDate() As Date
            Get
                Return Me.sentenceEffectiveDateField
            End Get
            Set(ByVal value As Date)
                Me.sentenceEffectiveDateField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property SentenceEffectiveDateSpecified() As Boolean
            Get
                Return Me.sentenceEffectiveDateFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.sentenceEffectiveDateFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property AlcoholLevel() As Decimal
            Get
                Return Me.alcoholLevelField
            End Get
            Set(ByVal value As Decimal)
                Me.alcoholLevelField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property AlcoholLevelSpecified() As Boolean
            Get
                Return Me.alcoholLevelFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.alcoholLevelFieldSpecified = value
            End Set
        End Property

        '''<remarks/>
        Public Property AlcoholMeasurementMethod() As String
            Get
                Return Me.alcoholMeasurementMethodField
            End Get
            Set(ByVal value As String)
                Me.alcoholMeasurementMethodField = value
            End Set
        End Property

        '''<remarks/>
        Public Property DrivingLicensePenaltyPoints() As Decimal
            Get
                Return Me.drivingLicensePenaltyPointsField
            End Get
            Set(ByVal value As Decimal)
                Me.drivingLicensePenaltyPointsField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()>
        Public Property DrivingLicensePenaltyPointsSpecified() As Boolean
            Get
                Return Me.drivingLicensePenaltyPointsFieldSpecified
            End Get
            Set(ByVal value As Boolean)
                Me.drivingLicensePenaltyPointsFieldSpecified = value
            End Set
        End Property
        Public Overridable Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)


            ' Vivek: this fails in case of addition of new Conviction.
            'If ConvictionKey = 0 Then
            '    oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing, _
            '                                            SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
            '                                            "ConvictionKey")
            'End If



            If String.IsNullOrEmpty(TypeCode) Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "TypeCode", "")
            End If
            If String.IsNullOrEmpty(StatusCode) Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "StatusCode", "")
            End If
            If String.IsNullOrEmpty(Description) Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "Description", "")
            End If
            'To be checked for StartDate
            If String.IsNullOrEmpty([Date].ToString()) Then
                oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing,
                                                    SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                                                    "Date", "")
            End If



        End Sub
    End Class
    Partial Public Class BaseGetDMEFolderRequestType
        Inherits BaseRequestType

        Private folderNumField As Integer

        Private folderPathField As String

        Private includeFilesField As Boolean

        '''<remarks/>
        Public Property FolderNum() As Integer
            Get
                Return Me.folderNumField
            End Get
            Set(ByVal value As Integer)
                Me.folderNumField = value
            End Set
        End Property

        '''<remarks/>
        Public Property FolderPath() As String
            Get
                Return Me.folderPathField
            End Get
            Set(ByVal value As String)
                Me.folderPathField = value
            End Set
        End Property

        '''<remarks/>
        Public Property IncludeFiles() As Boolean
            Get
                Return Me.includeFilesField
            End Get
            Set(ByVal value As Boolean)
                Me.includeFilesField = value
            End Set
        End Property

        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            MyBase.Validate(oErrorCollection)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)


            'If FolderNum = 0 And String.IsNullOrEmpty(FolderPath) Then

            '    oSAMErrorCollection.AddInvalidData(SAMConstants.SAMInvalidData.MandatoryInputMissing, _
            '                                                       SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString, _
            '                                                    "FolderNum Or FolderPath")
            'End If

        End Sub

    End Class

    '''<remarks/>
    Partial Public Class BaseGetDMEFolderResponseType
        Inherits BaseResponseType

        Private subFoldersField() As BaseDMEFolderType

        Private documentsField() As BaseDocumentType

        Private parentNumField As Integer

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("SubFolders", IsNullable:=True)>
        Public Property SubFolders() As BaseDMEFolderType()
            Get
                Return Me.subFoldersField
            End Get
            Set(ByVal value As BaseDMEFolderType())
                Me.subFoldersField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Documents", IsNullable:=True)>
        Public Property Documents() As BaseDocumentType()
            Get
                Return Me.documentsField
            End Get
            Set(ByVal value As BaseDocumentType())
                Me.documentsField = value
            End Set
        End Property


        '''<remarks/>
        Public Property ParentNum() As Integer
            Get
                Return Me.parentNumField
            End Get
            Set(ByVal value As Integer)
                Me.parentNumField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Partial Public Class BaseDMEFolderType

        Private folderNumField As Integer

        Private parentNumField As Integer

        Private nameField As String

        Private externalCodeField As String

        Private folderLevelField As Integer

        Private createDateField As Date

        '''<remarks/>
        Public Property FolderNum() As Integer
            Get
                Return Me.folderNumField
            End Get
            Set(ByVal value As Integer)
                Me.folderNumField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ParentNum() As Integer
            Get
                Return Me.parentNumField
            End Get
            Set(ByVal value As Integer)
                Me.parentNumField = value
            End Set
        End Property

        '''<remarks/>
        Public Property Name() As String
            Get
                Return Me.nameField
            End Get
            Set(ByVal value As String)
                Me.nameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ExternalCode() As String
            Get
                Return Me.externalCodeField
            End Get
            Set(ByVal value As String)
                Me.externalCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property FolderLevel() As Integer
            Get
                Return Me.folderLevelField
            End Get
            Set(ByVal value As Integer)
                Me.folderLevelField = value
            End Set
        End Property

        '''<remarks/>
        Public Property CreateDate() As Date
            Get
                Return Me.createDateField
            End Get
            Set(ByVal value As Date)
                Me.createDateField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Enum DMEDocType

        '''<remarks/>
        Unknown

        '''<remarks/>
        Tif

        '''<remarks/>
        PlainText

        '''<remarks/>
        RTF

        '''<remarks/>
        Word

        '''<remarks/>
        Excel

        '''<remarks/>
        PowerPoint

        '''<remarks/>
        Access

        '''<remarks/>
        HTML

        '''<remarks/>
        GIF

        '''<remarks/>
        JPEG

        '''<remarks/>
        Email

        '''<remarks/>
        PDF

        '''<remarks/>
        HelpFile

        '''<remarks/>
        ZIP

        '''<remarks/>
        MSG
    End Enum

    '''<remarks/>
    Partial Public Class BaseFindDMEDocumentsRequestType
        Inherits BaseRequestType

        Private partyCodeField As String

        Private partyNameField As String

        Private policyNumberField As String

        Private claimNumberField As String

        Private riskIndexField As String

        Private postCodeField As String

        Private documentDescriptionField As String

        Private includeFilesField As Boolean

        Private parentNumField As Integer

        Private folderNameField As String

        Private agentKeyField As Integer

        '''<remarks/>
        Public Property PartyCode() As String
            Get
                Return Me.partyCodeField
            End Get
            Set(ByVal value As String)
                Me.partyCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PartyName() As String
            Get
                Return Me.partyNameField
            End Get
            Set(ByVal value As String)
                Me.partyNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PolicyNumber() As String
            Get
                Return Me.policyNumberField
            End Get
            Set(ByVal value As String)
                Me.policyNumberField = value
            End Set
        End Property

        '''<remarks/>
        Public Property ClaimNumber() As String
            Get
                Return Me.claimNumberField
            End Get
            Set(ByVal value As String)
                Me.claimNumberField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RiskIndex() As String
            Get
                Return Me.riskIndexField
            End Get
            Set(ByVal value As String)
                Me.riskIndexField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PostCode() As String
            Get
                Return Me.postCodeField
            End Get
            Set(ByVal value As String)
                Me.postCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property DocumentDescription() As String
            Get
                Return Me.documentDescriptionField
            End Get
            Set(ByVal value As String)
                Me.documentDescriptionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property IncludeFiles() As Boolean
            Get
                Return Me.includeFilesField
            End Get
            Set(ByVal value As Boolean)
                Me.includeFilesField = value
            End Set
        End Property


        '''<remarks/>
        Public Property ParentNum() As Integer
            Get
                Return Me.parentNumField
            End Get
            Set(ByVal value As Integer)
                Me.parentNumField = value
            End Set
        End Property

        '''<remarks/>
        Public Property FolderName() As String
            Get
                Return Me.folderNameField
            End Get
            Set(ByVal value As String)
                Me.folderNameField = value
            End Set
        End Property

        Public Property AgentKey() As Integer
    End Class


    '''<remarks/>
    Partial Public Class BaseFindDMEDocumentsResponseType
        Inherits BaseResponseType

        Private foldersField() As BaseDMEFolderType

        Private documentsField() As BaseDocumentType

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Folders", IsNullable:=True)>
        Public Property Folders() As BaseDMEFolderType()
            Get
                Return Me.foldersField
            End Get
            Set(ByVal value As BaseDMEFolderType())
                Me.foldersField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlElementAttribute("Documents", IsNullable:=True)>
        Public Property Documents() As BaseDocumentType()
            Get
                Return Me.documentsField
            End Get
            Set(ByVal value As BaseDocumentType())
                Me.documentsField = value
            End Set
        End Property

    End Class

#Region "GetTreatyPartyDetails"
    Partial Public Class BaseGetTreatyPartyDetailsRequestType
        Inherits BaseRequestType

        Private treatyCodeField As String

        '''<remarks/>
        Public Property TreatyCode() As String
            Get
                Return Me.treatyCodeField
            End Get
            Set(ByVal value As String)
                Me.treatyCodeField = value
            End Set
        End Property
        Public Property IsRIDisabled() As Boolean
        Public Overrides Sub Validate(ByRef oErrorCollection As Object)

            Dim oSAMErrorCollection As SAMErrorCollection = CType(oErrorCollection, SAMErrorCollection)

            MyBase.Validate(oErrorCollection)

            If String.IsNullOrEmpty(TreatyCode) Then
                oSAMErrorCollection.AddInvalidData(
                SAMConstants.SAMInvalidData.MandatoryInputMissing,
                SAMConstants.SAMInvalidData.MandatoryInputMissing.ToString,
                "TreatyCode")
            End If
        End Sub
    End Class

    Partial Public Class BaseGetTreatyPartyDetailsResponseType
        Inherits BaseResponseType

        Private partiesField As System.Xml.XmlElement
        Private reseultdatafield As DataSet
        '''<remarks/>
        Public Property ResultData() As DataSet
            Get
                Return Me.reseultdatafield
            End Get
            Set(ByVal value As DataSet)
                Me.reseultdatafield = value
            End Set
        End Property
        '''<remarks/>
        Public Property Parties() As System.Xml.XmlElement
            Get
                Return Me.partiesField
            End Get
            Set(ByVal value As System.Xml.XmlElement)
                Me.partiesField = value
            End Set
        End Property
    End Class
#End Region

    Partial Public Class ValidateUserRequestType
        Inherits BaseRequestType

        ''' <summary>
        ''' Username to validate
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UserName() As String

    End Class

    Partial Public Class ValidateUserResponseType
        Inherits BaseResponseType

        ''' <summary>
        ''' UserId for an user
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UserId() As Integer

        ''' <summary>
        ''' Existing hashed password
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PasswordHash() As String

        ''' <summary>
        ''' PrPassword history
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PasswordHistory() As System.Collections.Generic.List(Of String)

        ''' <summary>
        ''' Existing Old password
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property SystemUpgradeChangePasswordRequired() As Boolean

    End Class

    Partial Public Class UpdateUserDetailRequestType
        Inherits BaseRequestType

        ''' <summary>
        ''' UserId for login
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UserId() As Integer

        ''' <summary>
        ''' Username to login
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property UserName() As String

        ''' <summary>
        ''' Is user authenticated 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IsAuthenticated() As Boolean

    End Class

    Partial Public Class UpdateUserDetailResponseType
        Inherits BaseResponseType

        ''' <summary>
        ''' Full name of user
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property FullName() As String

        ''' <summary>
        ''' Email address of user
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property EmailAddress() As String

        ''' <summary>
        ''' Is user locked due to multiple incorrect password attempt for login
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IsLocked() As Boolean

        ''' <summary>
        ''' Is the current password for an user is temporary
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property IsTempPassword() As Boolean

        ''' <summary>
        ''' Last password change date
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property PasswordChangeDate() As Nullable(Of DateTime)

        ''' <summary>
        ''' Error message(if any) during login/update user attributes
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property ErrorMessage As String

    End Class

    Public Class BaseGetClientDataExtractRequestType
        Inherits BaseRequestType

        Public Property PartyCnt() As Integer

        Public Property FilePassword() As String
    End Class

    Partial Public Class BaseGetClientDataExtractResponseType
        Inherits BaseResponseType

        Public Property ClientDataFile() As Byte()
    End Class

#Region "WPR 05 RI 2007 Off"
    Partial Public Class BaseGetRITreatyPartyDetailsWithTaxRequestType
        Inherits BaseRequestType

        Public Property InsuranceFileID() As Integer
        Public Property RiskID() As Integer
        Public Property RIArrangementLineID() As Integer
        Public Property TreatyID() As Integer
        Public Property Premium() As Decimal
        Public Property Commission() As Decimal
        Public Property PremiumTransType() As String
        Public Property CommissionTransType() As String
        Public Property IgnoreTreatyDetails() As Boolean = False
        Public Property IgnoreTax() As Boolean = False
        Public Property TreatyCode() As String

    End Class

    Partial Public Class BaseGetRITreatyPartyDetailsWithTaxResponseType
        Inherits BaseResponseType

        Public Property PremiumTax() As Decimal
        Public Property CommissionTax() As Decimal

        Public Property CommissionPercent() As Decimal = 0

        Public Property IsRetained() As Integer = 0
        Public Property AgreementCode() As String = String.Empty
    End Class

    Partial Public Class BaseGetRIPropTreatiesRequestType
        Inherits BaseRequestType
    End Class

    Partial Public Class BaseGetRIPropTreatiesResponseType
        Inherits BaseResponseType

        '''<remarks/>
        Public Property ResultData() As DataSet

        '''<remarks/>
        Public Property PropTreaties() As System.Xml.XmlElement

    End Class
#End Region
#Region "UpdateRioverridereason in RI Arrangement table"
    Partial Public Class BaseUpdateRiOverrideReasonInRiArrangementRequestType
        Inherits BaseRequestType

        Public Property RiArrangementId() As Integer = 0
        Public Property RiOverrideReasonId() As Integer = 0
    End Class

    Partial Public Class BaseUpdateRiOverrideReasonInRiArrangementResponseType
        Inherits BaseResponseType
    End Class
#End Region
#Region "CheckRetainedCoInsurerExists"
    Partial Public Class BaseGetRetainedCoInsurerRequestType
        Inherits BaseRequestType
        Public Property CoInsurerKeys As List(Of Integer)
    End Class
    Partial Public Class BaseGetRetainedCoInsurerResponseType
        Inherits BaseResponseType
        Public Property IsRetainedExists As Boolean = False
    End Class
#End Region
End Namespace
