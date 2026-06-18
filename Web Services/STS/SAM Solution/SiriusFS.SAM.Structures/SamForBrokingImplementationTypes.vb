
Imports system
Imports System.Text
Imports System.Xml.Serialization
Imports SiriusFS.SAM.Structure.BaseImplementationTypes

Namespace SAMForBrokingImplementationTypes

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

    Public Class GetDocumentListRequestType
        Inherits BaseGetDocumentListRequestType
    End Class

    Public Class GetDocumentListResponseType
        Inherits BaseGetDocumentListResponseType
    End Class

    Public Class FindPartyRequestType
        Inherits BaseFindPartyRequestType

        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String

        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
            End Set
        End Property

    End Class

    Public Class FindPartyResponseType
        Inherits BaseFindPartyResponseType

    End Class

    Public Class GetPartyRequestType
        Inherits BaseGetPartyRequestType

        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String

        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
            End Set
        End Property

    End Class

    Public Class GetPartyResponseType
        Inherits BaseGetPartyResponseType

    End Class

    Public Class GetPartySummaryRequestType
        Inherits BaseGetPartySummaryRequestType

        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String

        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
            End Set
        End Property

    End Class

    Public Class GetPartySummaryResponseType
        Inherits BaseGetPartySummaryResponseType

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

        Private m_oClaimDataset As System.Xml.XmlElement
        Public Property ClaimDataset() As System.Xml.XmlElement
            Get
                Return Me.m_oClaimDataset
            End Get
            Set(ByVal value As System.Xml.XmlElement)
                m_oClaimDataset = value
            End Set
        End Property

    End Class

    Public Class CreateEventRequestType
        Inherits BaseCreateEventRequestType

        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String
        Private m_sDocumentDescription As String

        '''<remarks/>
        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
            End Set
        End Property
        Public Property DocumentDescription() As String
            Get
                DocumentDescription = Me.m_sDocumentDescription
            End Get
            Set(ByVal value As String)
                Me.m_sDocumentDescription = value
            End Set
        End Property
    End Class

    Public Class CreateEventResponseType
        Inherits BaseCreateEventResponseType

    End Class

    Public Class CreateWmTaskRequestType
        Inherits BaseCreateWmTaskRequestType

        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String

        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
            End Set
        End Property
    End Class
    Public Class CreateWmTaskResponseType
        Inherits BaseCreateWmTaskResponseType
    End Class

    Public Class GetUserDetailsRequestType
        Inherits BaseGetUserDetailsRequestType
    End Class

    Public Class GetUserDetailsResponseType
        Inherits BaseGetUserDetailsResponseType
    End Class

    '''<remarks/>
    Public Class FindControlSearchRequestType
        Inherits BaseFindControlSearchRequestType
    End Class

    '''<remarks/>
    Public Class FindControlSearchResponseType
        Inherits BaseFindControlSearchResponseType
    End Class

    Public Class AddAddressRequestType
        Inherits BaseAddAddressRequestType

        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String

        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
            End Set
        End Property
    End Class

    Public Class AddAddressResponseType
        Inherits BaseAddAddressResponseType
    End Class

    Public Class AddMtaQuoteRequestType
        Inherits BaseAddMtaQuoteRequestType
        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String

        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
            End Set
        End Property
    End Class

    Public Class AddMtaQuoteResponseType
        Inherits BaseAddMtaQuoteResponseType
    End Class

    Public Class AddPartyRequestType
        Inherits BaseAddPartyRequestType

        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String

        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
            End Set
        End Property

    End Class

    Public Class AddPartyResponseType
        Inherits BaseAddPartyResponseType
    End Class

    Public Class AddQuoteRequestType
        Inherits BaseAddQuoteRequestType

        Private insuredPartiesField As System.Xml.XmlElement
        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String

        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
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

    Public Class BindMtaQuoteRequestType
        Inherits BaseBindMtaQuoteRequestType

        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String

        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
            End Set
        End Property

    End Class

    Public Class BindMtaQuoteResponseType
        Inherits BaseBindMtaQuoteResponseType

    End Class

    Public Class BindQuoteRequestType
        Inherits BaseBindQuoteRequestType

        Private _financePlanSchemeNo As Integer
        Public Property FinancePlanSchemeNo() As Integer
            Get
                Return _financePlanSchemeNo
            End Get
            Set(ByVal value As Integer)
                _financePlanSchemeNo = value
            End Set
        End Property

        Private _selectedAddOns() As BaseAddOnType
        Public Property SelectedAddOns() As BaseAddOnType()
            Get
                Return _selectedAddOns
            End Get
            Set(ByVal value As BaseAddOnType())
                _selectedAddOns = value
            End Set
        End Property

        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String

        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
            End Set
        End Property

    End Class


    Public Class BindQuoteResponseType
        Inherits BaseBindQuoteResponseType
    End Class


    Public Class GenerateDocumentRequestType
        Inherits BaseGenerateDocumentRequestType

        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String

        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
            End Set
        End Property

    End Class


    Public Class GenerateDocumentResponseType
        Inherits BaseGenerateDocumentResponseType
    End Class

    Public Class GetAddressRequestType
        Inherits BaseGetAddressRequestType

        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String

        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
            End Set
        End Property

    End Class


    Public Class GetAddressResponseType
        Inherits BaseGetAddressResponseType
    End Class


    Public Class GetAllPolicyVersionsRequestType
        Inherits BaseGetAllPolicyVersionsRequestType

        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String

        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
            End Set
        End Property

    End Class


    Public Class GetAllPolicyVersionsResponseType
        Inherits BaseGetAllPolicyVersionsResponseType
    End Class

    Public Class GetDatasetDefinitionRequestType
        Inherits BaseGetDatasetDefinitionRequestType

        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String

        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
            End Set
        End Property

    End Class

    Public Class GetDatasetDefinitionResponseType
        Inherits BaseGetDatasetDefinitionResponseType
    End Class

    Public Class GetDocumentRequestType
        Inherits BaseGetDocumentRequestType
    End Class

    Public Class GetDocumentResponseType
        Inherits BaseGetDocumentResponseType
    End Class

    Public Class GetHeaderAndSummariesByKeyRequestType
        Inherits BaseGetHeaderAndSummariesByKeyRequestType

        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String

        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
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
    End Class


    Public Class GetHeaderAndSummariesByRefRequestType
        Inherits BaseGetHeaderAndSummariesByRefRequestType

        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String

        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
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
    End Class


    Public Class GetInstalmentQuotesRequestType
        Inherits BaseGetInstalmentQuotesRequestType

        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String

        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
            End Set
        End Property

    End Class


    Public Class GetInstalmentQuotesResponseType
        Inherits BaseGetInstalmentQuotesResponseType
    End Class


    Public Class GetListRequestType
        Inherits BaseGetListRequestType

        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String

        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
            End Set
        End Property

    End Class


    Public Class GetListResponseType
        Inherits BaseGetListResponseType
    End Class

    Public Class GetMtaQuotesRequestType
        Inherits BaseGetMtaQuotesRequestType

        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String

        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
            End Set
        End Property

    End Class


    Public Class GetMtaQuotesResponseType
        Inherits BaseGetMtaQuotesResponseType
    End Class

    Public Class GetRiskRequestType
        Inherits BaseGetRiskRequestType

        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String

        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
            End Set
        End Property

    End Class


    Public Class GetRiskResponseType
        Inherits BaseGetRiskResponseType

        Private _PolicyCoinsurerSectionsXml As String
        Public Property PolicyCoinsurerSectionsXml() As String
            Get
                Return _PolicyCoinsurerSectionsXml
            End Get
            Set(ByVal value As String)
                _PolicyCoinsurerSectionsXml = value
            End Set
        End Property

        Private _PolicySectionsXml As String
        Public Property PolicySectionsXml() As String
            Get
                Return _PolicySectionsXml
            End Get
            Set(ByVal value As String)
                _PolicySectionsXml = value
            End Set
        End Property

    End Class

    Public Class IsMtaQuoteValidRequestType
        Inherits BaseIsMtaQuoteValidRequestType

        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String

        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
            End Set
        End Property
    End Class

    Public Class IsMtaQuoteValidResponseType
        Inherits BaseIsMtaQuoteValidResponseType
    End Class

    Public Class RunDefaultRulesAddRequestType
        Inherits BaseRunDefaultRulesAddRequestType

        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String

        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
            End Set
        End Property

    End Class


    Public Class RunDefaultRulesAddResponseType
        Inherits BaseRunDefaultRulesAddResponseType
    End Class


    Public Class RunDefaultRulesEditRequestType
        Inherits BaseRunDefaultRulesEditRequestType

        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String

        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
            End Set
        End Property

    End Class


    Public Class RunDefaultRulesEditResponseType
        Inherits BaseRunDefaultRulesEditResponseType
    End Class


    Public Class UpdateQuoteRequestType
        Inherits BaseUpdateQuoteRequestType

        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String

        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
            End Set
        End Property

    End Class


    Public Class UpdateQuoteResponseType
        Inherits BaseUpdateQuoteResponseType
    End Class


    Public Class UpdatePartyRequestType
        Inherits BaseUpdatePartyRequestType

        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String

        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
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

    Public Class UpdateMtaRiskRequestType
        Inherits BaseUpdateRiskRequestType

        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String

        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
            End Set
        End Property

    End Class


    Public Class UpdateMtaRiskResponseType
        Inherits BaseUpdateRiskResponseType

        Private _pfFirstInstalment As Double
        Public Property PfFirstInstalment() As Double
            Get
                Return _pfFirstInstalment
            End Get
            Set(ByVal value As Double)
                _pfFirstInstalment = value
            End Set
        End Property

        Private _pfOtherInstalments As Double
        Public Property PfOtherInstalments() As Double
            Get
                Return _pfOtherInstalments
            End Get
            Set(ByVal value As Double)
                _pfOtherInstalments = value
            End Set
        End Property

        Private _pfTotal As Double
        Public Property PfTotal() As Double
            Get
                Return _pfTotal
            End Get
            Set(ByVal value As Double)
                _pfTotal = value
            End Set
        End Property

        Private _addToExistingPfPlan As Boolean
        Public Property AddToExistingPfPlan() As Boolean
            Get
                Return _addToExistingPfPlan
            End Get
            Set(ByVal value As Boolean)
                _addToExistingPfPlan = value
            End Set
        End Property

        Private _PolicyCoinsurerSectionsXml As String
        Public Property PolicyCoinsurerSectionsXml() As String
            Get
                Return _PolicyCoinsurerSectionsXml
            End Get
            Set(ByVal value As String)
                _PolicyCoinsurerSectionsXml = value
            End Set
        End Property

        Private _PolicySectionsXml As String
        Public Property PolicySectionsXml() As String
            Get
                Return _PolicySectionsXml
            End Get
            Set(ByVal value As String)
                _PolicySectionsXml = value
            End Set
        End Property

    End Class

    Public Class UpdateRiskRequestType
        Inherits BaseUpdateRiskRequestType

        Private subBranchCode1Field As String

        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String

        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
            End Set
        End Property

        Public Property SubBranchCode1() As String
            Get
                Return Me.subBranchCode1Field
            End Get
            Set(ByVal value As String)
                Me.subBranchCode1Field = value
            End Set
        End Property
    End Class


    Public Class UpdateRiskResponseType
        Inherits BaseUpdateRiskResponseType

        Private _PolicyCoinsurerSectionsXml As String
        Public Property PolicyCoinsurerSectionsXml() As String
            Get
                Return _PolicyCoinsurerSectionsXml
            End Get
            Set(ByVal value As String)
                _PolicyCoinsurerSectionsXml = value
            End Set
        End Property

        Private _PolicySectionsXml As String
        Public Property PolicySectionsXml() As String
            Get
                Return _PolicySectionsXml
            End Get
            Set(ByVal value As String)
                _PolicySectionsXml = value
            End Set
        End Property

        Private _availableAddOnsXml As String
        Public Property AvailableAddOnsXml() As String
            Get
                Return _availableAddOnsXml
            End Get
            Set(ByVal value As String)
                _availableAddOnsXml = value
            End Set
        End Property

        Private _mandatoryAddOnsXml As String
        Public Property MandatoryAddOnsXml() As String
            Get
                Return _mandatoryAddOnsXml
            End Get
            Set(ByVal value As String)
                _mandatoryAddOnsXml = value
            End Set
        End Property

    End Class

    Public Class GetOnlineClientListRequestType
        Inherits BaseGetOnlineClientListRequestType

        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String

        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
            End Set
        End Property
    End Class

    Public Class GetOnlineClientListResponseType
        Inherits BaseGetOnlineClientListResponseType
    End Class

    Public Class AddAnonCustomerRequestType
        Inherits BaseRequestType

        Private genderCodeField As String

        Private dateOfBirthField As Date

        Private subBranchCodeField As String

        '''<remarks/>
        Public Property GenderCode() As String
            Get
                Return Me.genderCodeField
            End Get
            Set(ByVal value As String)
                Me.genderCodeField = value
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
        Public Property SubBranchCode() As String
            Get
                Return Me.subBranchCodeField
            End Get
            Set(ByVal value As String)
                Me.subBranchCodeField = value
            End Set
        End Property
    End Class

    Public Class AddAnonCustomerResponseType
        Inherits BaseResponseType

        Private partyKeyField As Integer

        Private partyTimeStampField() As Byte

        '''<remarks/>
        Public Property PartyKey() As Integer
            Get
                Return Me.partyKeyField
            End Get
            Set(ByVal value As Integer)
                Me.partyKeyField = value
            End Set
        End Property
        Public Property PartyTimeStamp() As Byte()
            Get
                Return Me.partyTimeStampField
            End Get
            Set(ByVal value As Byte())
                Me.partyTimeStampField = value
            End Set
        End Property
    End Class

    Public Class GetClaimsRequestType
        Inherits BaseGetClaimsRequestType

        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String

        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
            End Set
        End Property

    End Class


    Public Class GetClaimsResponseType
        Inherits BaseGetClaimsResponseType
    End Class

    Public Class GetHistoricalTransactionsRequestType
        Inherits BaseGetHistoricalTransactionsRequestType

        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String

        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
            End Set
        End Property

    End Class
    Public Class GetHistoricalTransactionsResponseType
        Inherits BaseGetHistoricalTransactionsResponseType
    End Class


    Public Class GetOpenTransactionsRequestType
        Inherits BaseGetOpenTransactionsRequestType

        Private m_sUserName As String
        Private m_iUserPartyKey As Integer
        Private m_sUserPartyType As String

        Public Property UserName() As String
            Get
                Return Me.m_sUserName
            End Get
            Set(ByVal value As String)
                Me.m_sUserName = value
            End Set
        End Property
        Public Property UserPartyKey() As Integer
            Get
                Return Me.m_iUserPartyKey
            End Get
            Set(ByVal value As Integer)
                Me.m_iUserPartyKey = value
            End Set
        End Property
        Public Property UserPartyType() As String
            Get
                Return Me.m_sUserPartyType
            End Get
            Set(ByVal value As String)
                Me.m_sUserPartyType = value
            End Set
        End Property
    End Class
    Public Class GetOpenTransactionsResponseType
        Inherits BaseGetOpenTransactionsResponseType
    End Class


End Namespace

