<Serializable()> Public Class PartySearchCriteria

    Private sShortname As String
    Private sName As String
    Private sTitle As String
    Private oAddress As Address
    Private sAlternativeID As String
    Private dtDateOfBirth As DateTime
    Private sFirstname As String
    Private oPartyTypes As PartyTypeCollection
    Private sPolicyRef As String
    Private sRiskIndex As String
    Private sPartyIndex As String
    Private sFileCode As String
    'Newly added properties for FindParty 
    Private sPostcode As String
    Private sAreacode As String
    Private sRiskRequestIndex As String
    Private sClaimsRiskIndex As String
    Private bIncludeClosedBranches As Boolean
    Private sClaimNumber As String
    Private sStatus As String
    Private sAgentType As String
    Private bSupressSubAgents As Boolean
    Private bIsAnySelected As Boolean
    Private nPartySourceId As Integer
    Private sTransactionType As String
    Private sPartyType As String
    Private sAddressLine1 As String
    Private sAddressLine2 As String
    Private sAddressLine3 As String
    Private sAddressLine4 As String
    Private sTelephoneNumber As String
    Private dDateOfBirth As Date
    Private oPartyType As PartyTypeType
    Private sAccountHandlerCode As String
    Private sAgentCode As String
    Private sReinsurerCode As String
    Private sTanPan As String
    Private nMaxRowsToFetch As Integer
    Private sOtherPartyTypeCode As String
    Private bIncludeAgent As Boolean
    Private sCaseNumber As String
    Private sAgentGroupCode As String
    Private sSearchType As String

    Public Sub New()
        dtDateOfBirth = DateTime.MinValue
        oPartyTypes = New PartyTypeCollection
        Address = New Address
    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("Short Name : " & sShortname & "<br />")
        sbPrint.AppendLine("Name : " & sName & "<br />")
        sbPrint.AppendLine("Address ---------------><br />")
        If oAddress IsNot Nothing Then
            sbPrint.AppendLine(oAddress.Print())
        End If

        sbPrint.AppendLine("Telephone Number : " & sTelephoneNumber & "<br />")
        sbPrint.AppendLine("Alternative ID : " & sAlternativeID & "<br />")
        sbPrint.AppendLine("Date of Birth : " & dtDateOfBirth & "<br />")
        sbPrint.AppendLine("First Name : " & sFirstname & "<br />")
        sbPrint.AppendLine("Party Types : " & oPartyTypes.Print() & "<br />")
        sbPrint.AppendLine("Policy Ref : " & sPolicyRef & "<br />")
        sbPrint.AppendLine("Risk Index : " & sRiskIndex & "<br />")
        sbPrint.AppendLine("CaseNumber : " & sCaseNumber & "<br />")

        Return sbPrint.ToString()

    End Function
    Public Property SearchType() As String
        Get
            Return sSearchType
        End Get
        Set(value As String)
            sSearchType = value
        End Set
    End Property


    Public Property Title() As String
        Get
            Return sTitle
        End Get
        Set(ByVal value As String)
            sTitle = value
        End Set
    End Property

    Public Property TanPan() As String
        Get
            Return sTanPan
        End Get
        Set(ByVal value As String)
            sTanPan = value
        End Set
    End Property

    Public Property ReinsurerCode() As String
        Get
            Return sReinsurerCode
        End Get
        Set(ByVal value As String)
            sReinsurerCode = value
        End Set
    End Property

    Public Property AgentCode() As String
        Get
            Return sAgentCode
        End Get
        Set(ByVal value As String)
            sAgentCode = value
        End Set
    End Property


    Public Property AccountHandlerCode() As String
        Get
            Return sAccountHandlerCode
        End Get
        Set(ByVal value As String)
            sAccountHandlerCode = value
        End Set
    End Property


    ' Aadlene End
    Public Property PartyType() As String ' PartyTypeType
        Get
            Return sPartyType
        End Get
        Set(ByVal value As String)
            sPartyType = value
        End Set
    End Property

    Public Property ShortName() As String
        Get
            Return sShortname
        End Get
        Set(ByVal value As String)
            sShortname = value
        End Set
    End Property

    Public Property Name() As String
        Get
            Return sName
        End Get
        Set(ByVal value As String)
            sName = value
        End Set
    End Property

    Public Property Address() As Address
        Get
            Return oAddress
        End Get
        Set(ByVal value As Address)
            oAddress = value
        End Set
    End Property

    Public Property TelephoneNumber() As String
        Get
            Return sTelephoneNumber
        End Get
        Set(ByVal value As String)
            sTelephoneNumber = value
        End Set
    End Property

    Public Property DateOfBirth() As DateTime
        Get
            Return dtDateOfBirth
        End Get
        Set(ByVal value As DateTime)
            dtDateOfBirth = value
        End Set
    End Property

    Public Property FirstName() As String
        Get
            Return sFirstname
        End Get
        Set(ByVal value As String)
            sFirstname = value
        End Set
    End Property


    Public Property PolicyRef() As String
        Get
            Return sPolicyRef
        End Get
        Set(ByVal value As String)
            sPolicyRef = value
        End Set
    End Property

    Public ReadOnly Property DateOfBirthSpecified() As Boolean
        Get
            Return IIf(dtDateOfBirth = DateTime.MinValue, False, True)
        End Get
    End Property

    Public Property AlternativeID() As String
        Get
            Return sAlternativeID
        End Get
        Set(ByVal value As String)
            sAlternativeID = value
        End Set
    End Property

    Public Property RiskIndex() As String
        Get
            Return sRiskIndex
        End Get
        Set(ByVal value As String)
            sRiskIndex = value
        End Set
    End Property


    Public Property PartyIndex() As String
        Get
            Return sPartyIndex
        End Get
        Set(ByVal value As String)
            sPartyIndex = value
        End Set
    End Property


    Public Property PartyTypes() As PartyTypeCollection
        Get
            Return oPartyTypes
        End Get
        Set(ByVal value As PartyTypeCollection)
            oPartyTypes = value
        End Set
    End Property

    Public ReadOnly Property PartyTypeSpecified() As Boolean
        Get
            Return IIf(oPartyTypes.Count > 0, True, False)
        End Get
    End Property

    Public Property FileCode() As String
        Get
            Return sFileCode
        End Get
        Set(ByVal value As String)
            sFileCode = value
        End Set
    End Property
    'Newly added Properties for Find Party 
    Public Property PostCode() As String
        Get
            Return sPostcode
        End Get
        Set(ByVal value As String)
            sPostcode = value
        End Set
    End Property
    Public Property AreaCode() As String
        Get
            Return sAreacode
        End Get
        Set(ByVal value As String)
            sAreacode = value
        End Set
    End Property
    Public Property RiskRequestIndex() As String
        Get
            Return sRiskRequestIndex
        End Get
        Set(ByVal value As String)
            sRiskRequestIndex = value
        End Set
    End Property
    Public Property ClaimsRiskIndex() As String
        Get
            Return sClaimsRiskIndex
        End Get
        Set(ByVal value As String)
            sClaimsRiskIndex = value
        End Set
    End Property
    Public Property IncludeClosedBranches() As Boolean
        Get
            Return bIncludeClosedBranches
        End Get
        Set(ByVal value As Boolean)
            bIncludeClosedBranches = value
        End Set
    End Property
    Public Property ClaimNumber() As String
        Get
            Return sClaimNumber
        End Get
        Set(ByVal value As String)
            sClaimNumber = value
        End Set
    End Property
    Public Property Status() As String
        Get
            Return sStatus
        End Get
        Set(ByVal value As String)
            sStatus = value
        End Set
    End Property
    Public Property SupressSubAgents() As Boolean
        Get
            Return bSupressSubAgents
        End Get
        Set(ByVal value As Boolean)
            bSupressSubAgents = value
        End Set
    End Property

    Public Property IsAnySelected() As Boolean
        Get
            Return bIsAnySelected
        End Get
        Set(ByVal value As Boolean)
            bIsAnySelected = value
        End Set
    End Property

    Public Property PartySourceId() As Integer
        Get
            Return nPartySourceId
        End Get
        Set(ByVal value As Integer)
            nPartySourceId = value
        End Set
    End Property

    Public Property TransactionType() As String
        Get
            Return sTransactionType
        End Get
        Set(ByVal value As String)
            sTransactionType = value
        End Set
    End Property

    Public Property AgentType() As String
        Get
            Return sAgentType

        End Get
        Set(ByVal value As String)
            sAgentType = value
        End Set
    End Property

    Public Property MaxRowsToFetch() As Integer
        Get
            Return nMaxRowsToFetch
        End Get
        Set(ByVal value As Integer)
            nMaxRowsToFetch = value
        End Set
    End Property
    Public Property OtherPartyTypeCode() As String
        Get
            Return sOtherPartyTypeCode

        End Get
        Set(ByVal value As String)
            sOtherPartyTypeCode = value
        End Set
    End Property
    ''' <summary>
    ''' will be used to display in grid
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property CaseNumber() As String
        Get
            Return sCaseNumber
        End Get
        Set(ByVal value As String)
            sCaseNumber = value
        End Set
    End Property
    ''' <summary>
    ''' will be used to strore agent code 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AgentGroupCode() As String
        Get
            Return sAgentGroupCode
        End Get
        Set(ByVal value As String)
            sAgentGroupCode = value
        End Set
    End Property

    Public Property IncludeAgent() As Boolean
        Get
            Return bIncludeAgent
        End Get
        Set(ByVal value As Boolean)
            bIncludeAgent = value
        End Set
    End Property

    Public Property IncludeAssociates As Boolean

End Class
''' <summary>
''' "SortableCollectionBase" class internally inherits "CollectionBase" and gives additionaly "Sortable" feture in class.
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class PartyTypeCollection : Inherits SortableCollectionBase

    Public Sub New()
        MyBase.SortObjectType = GetType(PartySearchCriteria)
    End Sub


    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oPartyType As PartyType In List
            sbPrint.AppendLine([Enum].GetName(GetType(PartyType), oPartyType))
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oPartyType As PartyType) As Integer
        Return List.Add(v_oPartyType)
    End Function

    Public Sub Remove(ByVal v_oPartyType As PartyType)
        List.Remove(v_oPartyType)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As PartyType
        Get
            Return List(i)
        End Get
        Set(ByVal value As PartyType)
            List(i) = value
        End Set
    End Property

    Public Function FindItemByType(ByVal v_oPartyType As PartyType) As PartyType

        For Each oItem As PartyType In List
            If oItem = v_oPartyType Then
                Return oItem
            End If
        Next

        Return Nothing

    End Function

    Public ReadOnly Property ToArray() As PartyType()
        Get

            Dim oArray(Count - 1) As PartyType
            For i As Integer = 0 To Count - 1
                oArray(i) = List(i)
            Next

            Return oArray

        End Get
    End Property

End Class

''' <summary>
''' Type of Party represented by the Party object
''' </summary>
Public Enum PartyType

    ''' <summary>
    ''' Personal Party
    ''' </summary>
    Personal = 0

    ''' <summary>
    ''' Corporate Party
    ''' </summary>
    Corporate = 3

    ''' <summary>
    ''' Corporate Party
    ''' </summary>
    Other = 4

End Enum

Public Enum PartyTypeType

    '''<remarks/>
    PC

    '''<remarks/>
    GC

    '''<remarks/>
    AG

    '''<remarks/>
    CC

    '''<remarks/>
    CO

    '''<remarks/>
    AH

    '''<remarks/>
    [IN]

    '''<remarks/>
    BR

    '''<remarks/>
    FE

    '''<remarks/>
    EX

    '''<remarks/>
    DI

    '''<remarks/>
    CM

    '''<remarks/>
    NC

    '''<remarks/>
    OTDRIVER

    '''<remarks/>
    OTWITNESS

    '''<remarks/>
    OTREPAIRER

    '''<remarks/>
    OTTHIRD

    '''<remarks/>
    FP

    '''<remarks/>
    AGG

    '''<remarks/>
    OTSUPPLIER

    '''<remarks/>
    OTLOSS

    '''<remarks/>
    OTSOL

    '''<remarks/>
    OTDOCTOR

    '''<remarks/>
    OTSURVEYOR

    '''<remarks/>
    HC

    '''<remarks/>
    OTOTHERPARTY
End Enum