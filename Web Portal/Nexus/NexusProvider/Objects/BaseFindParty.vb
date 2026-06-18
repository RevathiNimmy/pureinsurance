Public Class BaseFindParty
    Partial Public Class BaseFindPartyRequestType
        Inherits BaseRequestType

        Private partyTypeField As PartyTypeType

        Private partyTypeFieldSpecified As Boolean

        Private shortnameField As String

        Private alternativeIdField As String

        Private nameField As String

        Private firstnameField As String

        Private addressLine1Field As String

        Private addressLine2Field As String

        Private addressLine3Field As String

        Private addressLine4Field As String

        Private postCodeField As String

        Private areaCodeField As String

        Private telephoneNumberField As String

        Private dateOfBirthField As Date

        Private dateOfBirthFieldSpecified As Boolean

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

        '''<remarks/>
        Public Property PartyType() As PartyTypeType
            Get
                Return Me.partyTypeField
            End Get
            Set(ByVal value As PartyTypeType)
                Me.partyTypeField = value
            End Set
        End Property

        '''<remarks/>
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
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
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
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
        Public Property FileCode() As String
            Get
                Return Me.fileCodeField
            End Get
            Set(ByVal value As String)
                Me.fileCodeField = value
            End Set
        End Property

        '''<remarks/>
        Public Property RiskRequestdex() As String
            Get
                Return Me.riskRequestdexField
            End Get
            Set(ByVal value As String)
                Me.riskRequestdexField = value
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
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
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
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
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
        <System.Xml.Serialization.XmlIgnoreAttribute()> _
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
    End Class
End Class
