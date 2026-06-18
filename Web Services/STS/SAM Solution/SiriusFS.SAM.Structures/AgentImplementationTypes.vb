Option Strict Off

' Changes:
' 170505 CJB PN20978 Changes in Broking to allow document producton to be used in Swift (SJP) via the STS

#Region " Imports "

Imports system
Imports System.Text
Imports System.Xml.Serialization
Imports SiriusFS.SAM.Structure.BaseImplementationTypes

#End Region

Namespace AgentImplementationTypes

#Region " Agent Declarations"

    Public Class AddAddressRequestType
        Inherits BaseAddAddressRequestType

        Private userNameField As String

        Private agentKeyField As Long


        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property


        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class

    Public Class AddAddressResponseType
        Inherits BaseAddAddressResponseType
    End Class

    Public Class AddPartyRequestType
        Inherits BaseAddPartyRequestType

        Private userNameField As String

        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property

    End Class

    Public Class AddPartyResponseType
        Inherits BaseAddPartyResponseType
    End Class

    Public Class AddQuoteRequestType
        Inherits BaseAddQuoteRequestType

        Private userNameField As String

        Private insuredPartiesField As System.Xml.XmlElement

        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property

        Public Property InsuredParties() As System.Xml.XmlElement
            Get
                Return Me.insuredPartiesField
            End Get
            Set(ByVal value As System.Xml.XmlElement)
                Me.insuredPartiesField = value
            End Set
        End Property
    End Class

    Public Class AddQuoteResponseType
        Inherits BaseAddQuoteResponseType
    End Class

    Public Class AddRiskRequestType
        Inherits BaseAddRiskRequestType

        Private userNameField As String

        Private agentKeyField As Long


        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property


        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class


    Public Class AddRiskResponseType
        Inherits BaseAddRiskResponseType
    End Class


    Public Class BindQuoteRequestType
        Inherits BaseBindQuoteRequestType
    End Class


    Public Class BindQuoteResponseType
        Inherits BaseBindQuoteResponseType
    End Class


    Public Class ChangePasswordRequestType
        Inherits BaseChangePasswordRequestType

        Private userNameField As String

        Private agentKeyField As Long


        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property


        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class


    Public Class ChangePasswordResponseType
        Inherits BaseChangePasswordResponseType
    End Class


    Public Class DeleteRiskRequestType
        Inherits BaseDeleteRiskRequestType

        Private userNameField As String

        Private agentKeyField As Long


        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property


        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class


    Public Class DeleteRiskResponseType
        Inherits BaseDeleteRiskResponseType
    End Class

    Public Class FindControlSearchRequestType
        Inherits BaseFindControlSearchRequestType
    End Class


    Public Class FindControlSearchResponseType
        Inherits BaseFindControlSearchResponseType
    End Class


    Public Class FindPartyRequestType
        Inherits BaseFindPartyRequestType

        Private userNameField As String

        Private agentKeyField As Long


        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property


        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class


    Public Class FindPartyResponseType
        Inherits BaseFindPartyResponseType
    End Class


    Public Class GenerateDocumentRequestType
        Inherits BaseGenerateDocumentRequestType

        Private userNameField As String

        Private agentKeyField As Long


        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property


        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class


    Public Class GenerateDocumentResponseType
        Inherits BaseGenerateDocumentResponseType
    End Class


    Public Class GetProductByAgentRequestType
        Inherits BaseGetProductByAgentRequestType

        Private userNameField As String


        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property

    End Class


    Public Class GetProductByAgentResponseType
        Inherits BaseGetProductByAgentResponseType
    End Class


    Public Class GetRatingDetailsRequestType
        Inherits BaseGetRatingDetailsRequestType

        Private userNameField As String

        Private agentKeyField As Long


        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property


        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class


    Public Class GetRatingDetailsResponseType
        Inherits BaseGetRatingDetailsResponseType
    End Class


    Public Class GetRiskByProductRequestType
        Inherits BaseGetRiskByProductRequestType

        Private userNameField As String

        Private agentKeyField As Long


        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property


        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class


    Public Class GetRiskByProductResponseType
        Inherits BaseGetRiskByProductResponseType
    End Class


    Public Class GetAddressRequestType
        Inherits BaseGetAddressRequestType

        Private userNameField As String

        Private agentKeyField As Long


        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property


        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class


    Public Class GetAddressResponseType
        Inherits BaseGetAddressResponseType
    End Class


    Public Class GetAllPolicyVersionsRequestType
        Inherits BaseGetAllPolicyVersionsRequestType

        Private userNameField As String

        Private agentKeyField As Long


        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property


        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class


    Public Class GetAllPolicyVersionsResponseType
        Inherits BaseGetAllPolicyVersionsResponseType
    End Class


    Partial Public Class GetCurrenciesByBranchRequestType
        Inherits BaseGetCurrenciesByBranchRequestType

        Private userNameField As String

        Private agentKeyField As Long


        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property


        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class


    Partial Public Class GetCurrenciesByBranchResponseType
        Inherits BaseGetCurrenciesByBranchResponseType
    End Class


    Public Class GetDatasetDefinitionRequestType
        Inherits BaseGetDatasetDefinitionRequestType

        Private userNameField As String

        Private agentKeyField As Long


        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property


        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class


    Public Class GetDatasetDefinitionResponseType
        Inherits BaseGetDatasetDefinitionResponseType
    End Class


    Public Class GetDefaultDatasetRequestType
        Inherits BaseGetDefaultDatasetRequestType

        Private userNameField As String

        Private agentKeyField As Long


        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property


        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class


    Public Class GetDefaultDatasetResponseType
        Inherits BaseGetDefaultDatasetResponseType
    End Class


    Public Class GetHeaderAndSummariesByKeyRequestType
        Inherits BaseGetHeaderAndSummariesByKeyRequestType

        Private userNameField As String

        Private agentKeyField As Long


        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property


        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class


    Public Class GetHeaderAndSummariesByKeyResponseType
        Inherits BaseGetHeaderAndSummariesResponseType

        Private inceptionDateField As Date

        Private insuranceFileVersionField As Integer

        Private insuranceFileTypeCodeField As String

        Private insuranceFileStatusCodeField As String

        Private paymentMethodCodeField As String

        Private insuredPartiesField As System.Xml.XmlElement

        Private reseultdatafieldInsuredParties As DataSet

        Public Property InceptionDate() As Date
            Get
                Return Me.inceptionDateField
            End Get
            Set(ByVal value As Date)
                Me.inceptionDateField = value
            End Set
        End Property


        Public Property InsuranceFileVersion() As Integer
            Get
                Return Me.insuranceFileVersionField
            End Get
            Set(ByVal value As Integer)
                Me.insuranceFileVersionField = value
            End Set
        End Property


        Public Property InsuranceFileTypeCode() As String
            Get
                Return Me.insuranceFileTypeCodeField
            End Get
            Set(ByVal value As String)
                Me.insuranceFileTypeCodeField = value
            End Set
        End Property


        Public Property InsuranceFileStatusCode() As String
            Get
                Return Me.insuranceFileStatusCodeField
            End Get
            Set(ByVal value As String)
                Me.insuranceFileStatusCodeField = value
            End Set
        End Property


        Public Property PaymentMethodCode() As String
            Get
                Return Me.paymentMethodCodeField
            End Get
            Set(ByVal value As String)
                Me.paymentMethodCodeField = value
            End Set
        End Property


        Public Property InsuredParties() As System.Xml.XmlElement
            Get
                Return Me.insuredPartiesField
            End Get
            Set(ByVal value As System.Xml.XmlElement)
                Me.insuredPartiesField = value
            End Set
        End Property


        '''<remarks/>
        Public Property ResultDataInsuredParties() As DataSet
            Get
                Return Me.reseultdatafieldInsuredParties
            End Get
            Set(ByVal value As DataSet)
                Me.reseultdatafieldInsuredParties = value
            End Set
        End Property


    End Class


    Public Class GetHeaderAndSummariesByRefRequestType
        Inherits BaseGetHeaderAndSummariesByRefRequestType

        Private userNameField As String

        Private agentKeyField As Long


        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property


        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class


    Public Class GetHeaderAndSummariesByRefResponseType
        Inherits BaseGetHeaderAndSummariesResponseType

        Private inceptionDateField As Date

        Private insuranceFileVersionField As Integer

        Private insuranceFileTypeCodeField As String

        Private insuranceFileStatusCodeField As String

        Private paymentMethodCodeField As String

        Private insuredPartiesField As System.Xml.XmlElement


        Public Property InceptionDate() As Date
            Get
                Return Me.inceptionDateField
            End Get
            Set(ByVal value As Date)
                Me.inceptionDateField = value
            End Set
        End Property


        Public Property InsuranceFileVersion() As Integer
            Get
                Return Me.insuranceFileVersionField
            End Get
            Set(ByVal value As Integer)
                Me.insuranceFileVersionField = value
            End Set
        End Property


        Public Property InsuranceFileTypeCode() As String
            Get
                Return Me.insuranceFileTypeCodeField
            End Get
            Set(ByVal value As String)
                Me.insuranceFileTypeCodeField = value
            End Set
        End Property


        Public Property InsuranceFileStatusCode() As String
            Get
                Return Me.insuranceFileStatusCodeField
            End Get
            Set(ByVal value As String)
                Me.insuranceFileStatusCodeField = value
            End Set
        End Property


        Public Property PaymentMethodCode() As String
            Get
                Return Me.paymentMethodCodeField
            End Get
            Set(ByVal value As String)
                Me.paymentMethodCodeField = value
            End Set
        End Property


        Public Property InsuredParties() As System.Xml.XmlElement
            Get
                Return Me.insuredPartiesField
            End Get
            Set(ByVal value As System.Xml.XmlElement)
                Me.insuredPartiesField = value
            End Set
        End Property
        Private reseultdatafieldInsuredParties As DataSet
        '''<remarks/>
        Public Property ResultDataInsuredParties() As DataSet
            Get
                Return Me.reseultdatafieldInsuredParties
            End Get
            Set(ByVal value As DataSet)
                Me.reseultdatafieldInsuredParties = value
            End Set
        End Property


    End Class


    Public Class GetInstalmentQuotesRequestType
        Inherits BaseGetInstalmentQuotesRequestType

        Private userNameField As String

        Private agentKeyField As Long


        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property


        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class


    Public Class GetInstalmentQuotesResponseType
        Inherits BaseGetInstalmentQuotesResponseType
    End Class


    Public Class GetListRequestType
        Inherits BaseGetListRequestType

        Private userNameField As String

        Private agentKeyField As Long


        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property


        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class


    Public Class GetListResponseType
        Inherits BaseGetListResponseType
    End Class


    Public Class GetPartyRequestType
        Inherits BaseGetPartyRequestType

        Private userNameField As String

        Private agentKeyField As Long


        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property


        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class


    Public Class GetPartyResponseType
        Inherits BaseGetPartyResponseType
    End Class


    Public Class GetPartySummaryRequestType
        Inherits BaseGetPartySummaryRequestType

        Private userNameField As String

        Private agentKeyField As Long


        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property


        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class


    Public Class GetPartySummaryResponseType
        Inherits BaseGetPartySummaryResponseType
    End Class


    Public Class GetRiskRequestType
        Inherits BaseGetRiskRequestType

        Private userNameField As String

        Private agentKeyField As Long


        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property


        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class


    Public Class GetRiskResponseType
        Inherits BaseGetRiskResponseType
    End Class


    Public Class LoginRequestType
        Inherits BaseLoginRequestType
    End Class


    Public Class LoginResponseType
        Inherits BaseLoginResponseType

        Private agentKeyField As Integer

        Private agentNameField As String

        Private agentTypeField As String


        Public Property AgentKey() As Integer
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Integer)
                Me.agentKeyField = value
            End Set
        End Property


        Public Property AgentName() As String
            Get
                Return Me.agentNameField
            End Get
            Set(ByVal value As String)
                Me.agentNameField = value
            End Set
        End Property

        Public Property AgentType() As String
            Get
                Return Me.agentTypeField
            End Get
            Set(ByVal value As String)
                Me.agentTypeField = value
            End Set
        End Property
    End Class


    Public Class LogoffRequestType
        Inherits BaseLogoffRequestType

        Private agentKeyField As Long


        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class


    Public Class LogoffResponseType
        Inherits BaseLogoffResponseType
    End Class


    Public Class RunDefaultRulesAddRequestType
        Inherits BaseRunDefaultRulesAddRequestType

        Private userNameField As String

        Private agentKeyField As Long


        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property


        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class


    Public Class RunDefaultRulesAddResponseType
        Inherits BaseRunDefaultRulesAddResponseType
    End Class


    Public Class RunDefaultRulesEditRequestType
        Inherits BaseRunDefaultRulesEditRequestType

        Private userNameField As String

        Private agentKeyField As Long


        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property


        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class


    Public Class RunDefaultRulesEditResponseType
        Inherits BaseRunDefaultRulesEditResponseType
    End Class


    Public Class UpdateQuoteRequestType
        Inherits BaseUpdateQuoteRequestType

        Private userNameField As String

        Private agentKeyField As Long


        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property


        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class


    Public Class UpdateQuoteResponseType
        Inherits BaseUpdateQuoteResponseType
    End Class


    Public Class UpdatePartyRequestType
        Inherits BaseUpdatePartyRequestType

        Private userNameField As String

        Private agentKeyField As Long


        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property


        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class


    Public Class UpdatePartyResponseType
        Inherits BaseUpdatePartyResponseType

        Private partyTimestamp1Field() As Byte


        Public Property PartyTimestamp1() As Byte()
            Get
                Return Me.partyTimestamp1Field
            End Get
            Set(ByVal value As Byte())
                Me.partyTimestamp1Field = value
            End Set
        End Property
    End Class


    Public Class UpdateRiskRequestType
        Inherits BaseUpdateRiskRequestType

        Private subBranchCode1Field As String

        Private userNameField As String

        Private agentKeyField As Long


        Public Property SubBranchCode1() As String
            Get
                Return Me.subBranchCode1Field
            End Get
            Set(ByVal value As String)
                Me.subBranchCode1Field = value
            End Set
        End Property


        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property


        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class


    Public Class UpdateRiskResponseType
        Inherits BaseUpdateRiskResponseType
    End Class

    Public Class CreateEventRequestType
        Inherits BaseCreateEventRequestType

        Private userNameField As String

        Private agentKeyField As Long

        Private agentKeyFieldSpecified As Boolean


        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property


        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property

    End Class
    Public Class CreateEventResponseType
        Inherits BaseCreateEventResponseType
    End Class


    Public Class ForgottenPasswordRequestType
        Inherits BaseForgottenPasswordRequestType
    End Class

    Public Class ForgottenPasswordResponseType
        Inherits BaseForgottenPasswordResponseType
    End Class
#End Region

End Namespace
