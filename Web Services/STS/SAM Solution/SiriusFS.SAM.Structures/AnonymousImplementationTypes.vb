Option Strict Off

' Changes:
' 170505 CJB PN20978 Changes in Broking to allow document producton to be used in Swift (SJP) via the STS

#Region " Imports "

Imports system
Imports System.Text
Imports System.Xml.Serialization
Imports SiriusFS.SAM.Structure.BaseImplementationTypes

#End Region

Namespace AnonymousImplementationTypes
    '''<remarks/>
    Public Class AddAddressRequestType
        Inherits BaseAddAddressRequestType
    End Class

    Public Class AddAddressResponseType
        Inherits BaseAddAddressResponseType
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

    '''<remarks/>
    Public Class AddQuoteRequestType
        Inherits BaseAddQuoteRequestType

    End Class

    '''<remarks/>
    Public Class AddQuoteResponseType
        Inherits BaseAddQuoteResponseType
    End Class

    '''<remarks/>
    Public Class AddRiskRequestType
        Inherits BaseAddRiskRequestType
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
    Public Class DeleteRiskRequestType
        Inherits BaseDeleteRiskRequestType
    End Class

    '''<remarks/>
    Public Class DeleteRiskResponseType
        Inherits BaseDeleteRiskResponseType
    End Class

    '''<remarks/>
    Partial Public Class FindControlSearchRequestType
        Inherits BaseFindControlSearchRequestType
    End Class

    '''<remarks/>
    Partial Public Class FindControlSearchResponseType
        Inherits BaseFindControlSearchResponseType
    End Class

    '''<remarks/>
    Public Class GetAddressRequestType
        Inherits BaseGetAddressRequestType
    End Class

    '''<remarks/>
    Public Class GetAddressResponseType
        Inherits BaseGetAddressResponseType
    End Class

    '''<remarks/>
    Public Class GetDatasetDefinitionRequestType
        Inherits BaseGetDatasetDefinitionRequestType
    End Class

    '''<remarks/>
    Public Class GetDatasetDefinitionResponseType
        Inherits BaseGetDatasetDefinitionResponseType
    End Class

    '''<remarks/>
    Public Class GetDefaultDatasetRequestType
        Inherits BaseGetDefaultDatasetRequestType
    End Class

    '''<remarks/>
    Public Class GetDefaultDatasetResponseType
        Inherits BaseGetDefaultDatasetResponseType
    End Class

    '''<remarks/>
    Public Class GetListRequestType
        Inherits BaseGetListRequestType
    End Class

    '''<remarks/>
    Public Class GetListResponseType
        Inherits BaseGetListResponseType
    End Class

    '''<remarks/>
    Public Class GetQuoteAndSummariesByKeyRequestType
        Inherits BaseGetHeaderAndSummariesByKeyRequestType
    End Class

    '''<remarks/>
    Public Class GetQuoteAndSummariesByKeyResponseType
        Inherits BaseGetHeaderAndSummariesResponseType

        Private genderCodeField As String

        Private dateOfBirthField As Date

        Private isAnonymousField As Boolean

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
        Public Property IsAnonymous() As Boolean
            Get
                Return Me.isAnonymousField
            End Get
            Set(ByVal value As Boolean)
                Me.isAnonymousField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class GetQuoteAndSummariesByRefRequestType
        Inherits BaseGetHeaderAndSummariesByRefRequestType

        Private insuranceRef1Field As String

        '''<remarks/>
        Public Property InsuranceRef1() As String
            Get
                Return Me.insuranceRef1Field
            End Get
            Set(ByVal value As String)
                Me.insuranceRef1Field = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class GetQuoteAndSummariesByRefResponseType
        Inherits BaseGetHeaderAndSummariesResponseType

        Private genderCodeField As String

        Private dateOfBirthField As Date

        Private isAnonymousField As Boolean

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
        Public Property IsAnonymous() As Boolean
            Get
                Return Me.isAnonymousField
            End Get
            Set(ByVal value As Boolean)
                Me.isAnonymousField = value
            End Set
        End Property
    End Class

    '''<remarks/>
    Public Class GetRiskRequestType
        Inherits BaseGetRiskRequestType
    End Class

    '''<remarks/>
    Public Class GetRiskResponseType
        Inherits BaseGetRiskResponseType
    End Class

    '''<remarks/>
    Public Class RunDefaultRulesAddRequestType
        Inherits BaseRunDefaultRulesAddRequestType
    End Class

    '''<remarks/>
    Public Class RunDefaultRulesAddResponseType
        Inherits BaseRunDefaultRulesAddResponseType
    End Class

    '''<remarks/>
    Public Class RunDefaultRulesEditRequestType
        Inherits BaseRunDefaultRulesEditRequestType
    End Class

    '''<remarks/>
    Public Class RunDefaultRulesEditResponseType
        Inherits BaseRunDefaultRulesEditResponseType
    End Class

    '''<remarks/>
    Public Class UpdateRiskRequestType
        Inherits BaseUpdateRiskRequestType
    End Class

    '''<remarks/>
    Public Class UpdateRiskResponseType
        Inherits BaseUpdateRiskResponseType
    End Class

    '''<remarks/>
    Public Class UpdatePartyRequestType
        Inherits BaseUpdatePartyRequestType

        Private agentKeyField As Long

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

    End Class

    Public Class ExecutePRERulesetRequestType
        Inherits BaseExecutePRERulesetRequestType
    End Class

    '''<remarks/>
    Public Class ExecutePRERulesetResponseType
        Inherits BaseExecutePRERulesetResponseType
    End Class

End Namespace