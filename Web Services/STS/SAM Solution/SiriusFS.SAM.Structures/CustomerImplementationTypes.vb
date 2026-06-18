Option Strict Off

' Changes:
' 170505 CJB PN20978 Changes in Broking to allow document producton to be used in Swift (SJP) via the STS

#Region " Imports "

Imports system
Imports System.Text
Imports System.Xml.Serialization
Imports SiriusFS.SAM.Structure.BaseImplementationTypes

#End Region

Namespace CustomerImplementationTypes

#Region " Customer Declarations"

    Public Class AddAddressRequestType
        Inherits BaseAddAddressRequestType

        Private userNameField As String

        Private agentKeyField As Long

        '''<remarks/>
        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property

        '''<remarks/>
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

        '''<remarks/>
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

        '''<remarks/>
        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property

        '''<remarks/>
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

        '''<remarks/>
        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class AddRiskResponseType
        Inherits BaseAddRiskResponseType
    End Class

    '''<remarks/>
    Public Class BindQuoteRequestType
        Inherits BaseBindQuoteRequestType
    End Class

    '''<remarks/>
    Public Class BindQuoteResponseType
        Inherits BaseBindQuoteResponseType
    End Class

    '''<remarks/>
    Public Class ChangePasswordRequestType
        Inherits BaseChangePasswordRequestType

        Private userNameField As String

        Private agentKeyField As Long

        '''<remarks/>
        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class ChangePasswordResponseType
        Inherits BaseChangePasswordResponseType
    End Class

    '''<remarks/>
    Public Class DeleteRiskRequestType
        Inherits BaseDeleteRiskRequestType

        Private userNameField As String

        Private agentKeyField As Long

        '''<remarks/>
        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class DeleteRiskResponseType
        Inherits BaseDeleteRiskResponseType
    End Class

    '''<remarks/>
    Public Class FindPartyRequestType
        Inherits BaseFindPartyRequestType

        Private userNameField As String

        Private agentKeyField As Long

        '''<remarks/>
        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class FindPartyResponseType
        Inherits BaseFindPartyResponseType
    End Class

    '''<remarks/>
    Public Class GenerateDocumentRequestType
        Inherits BaseGenerateDocumentRequestType

        Private userNameField As String

        Private agentKeyField As Long

        '''<remarks/>
        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class GenerateDocumentResponseType
        Inherits BaseGenerateDocumentResponseType
    End Class

    '''<remarks/>
    Public Class GetAddressRequestType
        Inherits BaseGetAddressRequestType

        Private userNameField As String

        Private agentKeyField As Long

        '''<remarks/>
        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class GetAddressResponseType
        Inherits BaseGetAddressResponseType
    End Class

    '''<remarks/>
    Public Class GetAllPolicyVersionsRequestType
        Inherits BaseGetAllPolicyVersionsRequestType

        Private userNameField As String

        Private agentKeyField As Long

        '''<remarks/>
        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class GetAllPolicyVersionsResponseType
        Inherits BaseGetAllPolicyVersionsResponseType
    End Class

    '''<remarks/>
    Public Class GetDatasetDefinitionRequestType
        Inherits BaseGetDatasetDefinitionRequestType

        Private userNameField As String

        Private agentKeyField As Long

        '''<remarks/>
        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class GetDatasetDefinitionResponseType
        Inherits BaseGetDatasetDefinitionResponseType
    End Class

    '''<remarks/>
    Public Class GetDefaultDatasetRequestType
        Inherits BaseGetDefaultDatasetRequestType

        Private userNameField As String

        Private agentKeyField As Long

        '''<remarks/>
        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class GetDefaultDatasetResponseType
        Inherits BaseGetDefaultDatasetResponseType
    End Class

    '''<remarks/>
    Public Class GetHeaderAndSummariesByKeyRequestType
        Inherits BaseGetHeaderAndSummariesByKeyRequestType

        Private userNameField As String

        Private agentKeyField As Long

        '''<remarks/>
        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class GetHeaderAndSummariesByKeyResponseType
        Inherits BaseGetHeaderAndSummariesResponseType

        Private inceptionDateField As Date

        Private insuranceFileVersionField As Integer

        Private insuranceFileTypeCodeField As String

        Private insuranceFileStatusCodeField As String

        Private paymentMethodCodeField As String

        Private insuredPartiesField As System.Xml.XmlElement

        '''<remarks/>
        Public Property InceptionDate() As Date
            Get
                Return Me.inceptionDateField
            End Get
            Set(ByVal value As Date)
                Me.inceptionDateField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InsuranceFileVersion() As Integer
            Get
                Return Me.insuranceFileVersionField
            End Get
            Set(ByVal value As Integer)
                Me.insuranceFileVersionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InsuranceFileTypeCode() As String
            Get
                Return Me.insuranceFileTypeCodeField
            End Get
            Set(ByVal value As String)
                Me.insuranceFileTypeCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InsuranceFileStatusCode() As String
            Get
                Return Me.insuranceFileStatusCodeField
            End Get
            Set(ByVal value As String)
                Me.insuranceFileStatusCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PaymentMethodCode() As String
            Get
                Return Me.paymentMethodCodeField
            End Get
            Set(ByVal value As String)
                Me.paymentMethodCodeField = value
            End Set
        End Property

        '''<remarks/>
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

    '''<remarks/>
    Public Class GetHeaderAndSummariesByRefRequestType
        Inherits BaseGetHeaderAndSummariesByRefRequestType

        Private userNameField As String

        Private agentKeyField As Long

        '''<remarks/>
        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class GetHeaderAndSummariesByRefResponseType
        Inherits BaseGetHeaderAndSummariesResponseType

        Private inceptionDateField As Date

        Private insuranceFileVersionField As Integer

        Private insuranceFileTypeCodeField As String

        Private insuranceFileStatusCodeField As String

        Private paymentMethodCodeField As String

        Private insuredPartiesField As System.Xml.XmlElement

        '''<remarks/>
        Public Property InceptionDate() As Date
            Get
                Return Me.inceptionDateField
            End Get
            Set(ByVal value As Date)
                Me.inceptionDateField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InsuranceFileVersion() As Integer
            Get
                Return Me.insuranceFileVersionField
            End Get
            Set(ByVal value As Integer)
                Me.insuranceFileVersionField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InsuranceFileTypeCode() As String
            Get
                Return Me.insuranceFileTypeCodeField
            End Get
            Set(ByVal value As String)
                Me.insuranceFileTypeCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InsuranceFileStatusCode() As String
            Get
                Return Me.insuranceFileStatusCodeField
            End Get
            Set(ByVal value As String)
                Me.insuranceFileStatusCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property PaymentMethodCode() As String
            Get
                Return Me.paymentMethodCodeField
            End Get
            Set(ByVal value As String)
                Me.paymentMethodCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property InsuredParties() As System.Xml.XmlElement
            Get
                Return Me.insuredPartiesField
            End Get
            Set(ByVal value As System.Xml.XmlElement)
                Me.insuredPartiesField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class GetListRequestType
        Inherits BaseGetListRequestType

        Private userNameField As String

        Private agentKeyField As Long

        '''<remarks/>
        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class GetListResponseType
        Inherits BaseGetListResponseType
    End Class

    '''<remarks/>
    Public Class GetPartyRequestType
        Inherits BaseGetPartyRequestType

        Private userNameField As String

        Private agentKeyField As Long

        '''<remarks/>
        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class GetPartyResponseType
        Inherits BaseGetPartyResponseType
    End Class

    '''<remarks/>
    Public Class GetPartySummaryRequestType
        Inherits BaseGetPartySummaryRequestType

        Private userNameField As String

        Private agentKeyField As Long

        '''<remarks/>
        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class GetPartySummaryResponseType
        Inherits BaseGetPartySummaryResponseType
    End Class

    '''<remarks/>
    Public Class GetRiskRequestType
        Inherits BaseGetRiskRequestType

        Private userNameField As String

        Private agentKeyField As Long

        '''<remarks/>
        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class GetRiskResponseType
        Inherits BaseGetRiskResponseType
    End Class

    '''<remarks/>
    Public Class RunDefaultRulesAddRequestType
        Inherits BaseRunDefaultRulesAddRequestType

        Private userNameField As String

        Private agentKeyField As Long

        '''<remarks/>
        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class RunDefaultRulesAddResponseType
        Inherits BaseRunDefaultRulesAddResponseType
    End Class

    '''<remarks/>
    Public Class RunDefaultRulesEditRequestType
        Inherits BaseRunDefaultRulesEditRequestType

        Private userNameField As String

        Private agentKeyField As Long

        '''<remarks/>
        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class RunDefaultRulesEditResponseType
        Inherits BaseRunDefaultRulesEditResponseType
    End Class

    '''<remarks/>
    Public Class UpdateQuoteRequestType
        Inherits BaseUpdateQuoteRequestType

        Private userNameField As String

        Private agentKeyField As Long

        '''<remarks/>
        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class UpdateQuoteResponseType
        Inherits BaseUpdateQuoteResponseType
    End Class

    '''<remarks/>
    Public Class UpdatePartyRequestType
        Inherits BaseUpdatePartyRequestType

        Private userNameField As String

        Private agentKeyField As Long

        '''<remarks/>
        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class UpdatePartyResponseType
        Inherits BaseUpdatePartyResponseType

        Private partyTimestamp1Field() As Byte

        '''<remarks/>
        Public Property PartyTimestamp1() As Byte()
            Get
                Return Me.partyTimestamp1Field
            End Get
            Set(ByVal value As Byte())
                Me.partyTimestamp1Field = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class UpdateRiskRequestType
        Inherits BaseUpdateRiskRequestType

        Private subBranchCode1Field As String

        Private userNameField As String

        Private agentKeyField As Long

        '''<remarks/>
        Public Property SubBranchCode1() As String
            Get
                Return Me.subBranchCode1Field
            End Get
            Set(ByVal value As String)
                Me.subBranchCode1Field = value
            End Set
        End Property

        '''<remarks/>
        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class UpdateRiskResponseType
        Inherits BaseUpdateRiskResponseType
    End Class

    Public Class GetClaimsRequestType
        Inherits BaseGetClaimsRequestType

        Private userNameField As String

        Private agentKeyField As Long

        '''<remarks/>
        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class

    Public Class GetClaimsResponseType
        Inherits BaseGetClaimsResponseType
    End Class

    Public Class GetOpenTransactionsRequestType
        Inherits BaseGetOpenTransactionsRequestType

        Private userNameField As String

        Private agentKeyField As Long

        '''<remarks/>
        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class

    Public Class GetOpenTransactionsResponseType
        Inherits BaseGetOpenTransactionsResponseType
    End Class

    Public Class GetHistoricalTransactionsRequestType
        Inherits BaseGetHistoricalTransactionsRequestType

        Private userNameField As String

        Private agentKeyField As Long

        '''<remarks/>
        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property

        '''<remarks/>
        Public Property AgentKey() As Long
            Get
                Return Me.agentKeyField
            End Get
            Set(ByVal value As Long)
                Me.agentKeyField = value
            End Set
        End Property
    End Class

    Public Class GetHistoricalTransactionsResponseType
        Inherits BaseGetHistoricalTransactionsResponseType
    End Class
    Public Class GetOnlineClientListRequestType
        Inherits BaseGetOnlineClientListRequestType
    End Class
    Public Class GetOnlineClientListResponseType
        Inherits BaseGetOnlineClientListResponseType
    End Class
    Public Class CreateEventRequestType
        Inherits BaseCreateEventRequestType

        Private userNameField As String

        Private agentKeyField As Long

        Private agentKeyFieldSpecified As Boolean

        '''<remarks/>
        Public Property UserName() As String
            Get
                Return Me.userNameField
            End Get
            Set(ByVal value As String)
                Me.userNameField = value
            End Set
        End Property

        '''<remarks/>
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

    Public Class FindControlSearchRequestType
        Inherits BaseFindControlSearchRequestType
    End Class

    Public Class FindControlSearchResponseType
        Inherits BaseFindControlSearchResponseType
    End Class

#End Region

End Namespace
