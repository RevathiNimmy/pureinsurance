<Serializable()> Public Class ClaimPartyClient
#Region "Private Fields"
    Private oClaimContact As ClaimContact
    Private oAddress As Address
    Private sPartyClaimNumber As String
    Private oContact As ContactCollection
    Private sShortName, sName As String
    Private iPartyKey As Integer
    Private sInsurerContact, sInsurerEmail, sInsurerFaxNo, sInsurerTelNo, sInsurerName As String
    Private sClientEmail, sClientFaxNo, sClientMobileNo, sClientTelNo, sClientTelNoOff As String
#End Region
    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()
        'oClaimContact = New ClaimContact
        oAddress = New Address
        oContact = New ContactCollection
    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Overridable Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("PartyClaimNumber : " & sPartyClaimNumber & "<br />")

        sbPrint.Append("ClaimContact: ")

        If oClaimContact IsNot Nothing Then
            sbPrint.AppendLine(oClaimContact.Print())
        End If

        sbPrint.AppendLine("Addresses ---------------><br />")

        If oAddress IsNot Nothing Then
            sbPrint.AppendLine(oAddress.Print())
        End If

        sbPrint.AppendLine("Contacts ---------------><br />")

        If oContact IsNot Nothing Then
            sbPrint.AppendLine(oContact.Print())
        End If

        Return sbPrint.ToString

    End Function
#Region "properties"

    Public Property ClientName() As String
        Get
            Return sName
        End Get
        Set(ByVal value As String)
            sName = value
        End Set
    End Property
    Public Property PartyKey() As Integer
        Get
            Return iPartyKey
        End Get
        Set(ByVal value As Integer)
            iPartyKey = value
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
    Public Property PartyClaimNumber() As String
        Get
            Return sPartyClaimNumber
        End Get
        Set(ByVal value As String)
            sPartyClaimNumber = value
        End Set
    End Property

    Public Property ShortName() As String
        Get
            Return sShortName
        End Get
        Set(ByVal value As String)
            sShortName = value
        End Set
    End Property

    Public Property Contact() As ContactCollection
        Get
            Return oContact
        End Get
        Set(ByVal value As ContactCollection)
            oContact = value
        End Set
    End Property

    Public Property InsurerContact() As String
        Get
            Return sInsurerContact
        End Get
        Set(ByVal value As String)
            sInsurerContact = value
        End Set
    End Property
    Public Property InsurerName() As String
        Get
            Return sInsurerName
        End Get
        Set(ByVal value As String)
            sInsurerName = value
        End Set
    End Property
    Public Property InsurerEmail() As String
        Get
            Return sInsurerEmail
        End Get
        Set(ByVal value As String)
            sInsurerEmail = value
        End Set
    End Property
    Public Property InsurerFaxNo() As String
        Get
            Return sInsurerFaxNo
        End Get
        Set(ByVal value As String)
            sInsurerFaxNo = value
        End Set
    End Property
    Public Property InsurerTelNo() As String
        Get
            Return sInsurerTelNo
        End Get
        Set(ByVal value As String)
            sInsurerTelNo = value
        End Set
    End Property

    Public Property ClientEmail() As String
        Get
            Return sClientEmail
        End Get
        Set(ByVal value As String)
            sClientEmail = value
        End Set
    End Property
    Public Property ClientFaxNo() As String
        Get
            Return sClientFaxNo
        End Get
        Set(ByVal value As String)
            sClientFaxNo = value
        End Set
    End Property
    Public Property ClientMobileNo() As String
        Get
            Return sClientMobileNo
        End Get
        Set(ByVal value As String)
            sClientMobileNo = value
        End Set
    End Property
    Public Property ClientTelNo() As String
        Get
            Return sClientTelNo
        End Get
        Set(ByVal value As String)
            sClientTelNo = value
        End Set
    End Property
    Public Property ClientTelNoOff() As String
        Get
            Return sClientTelNoOff
        End Get
        Set(ByVal value As String)
            sClientTelNoOff = value
        End Set
    End Property
#End Region

End Class
<Serializable()> Public Class ClaimPartyInsurer : Inherits ClaimPartyClient

    Private sContactName As String
    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()
    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Overrides Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("Contact Name : " & sContactName & "<br />")
        Return sbPrint.ToString

    End Function
    '''<remarks/>
    Public Property ContactName() As String
        Get
            Return Me.sContactName
        End Get
        Set(ByVal value As String)
            Me.sContactName = value
        End Set
    End Property
End Class
<Serializable()> Public Class ClaimParty : Inherits ClaimPartyClient

    Private bTaxRegistered As Boolean
    Private sTaxRegistrationNumber As String
    ''' <summary>
    ''' Default constructor
    ''' </summary>
    Public Sub New()
        bTaxRegistered = False
    End Sub
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Overrides Function Print() As String
        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("Tax Registration Number : " & sTaxRegistrationNumber & "<br />")
        sbPrint.AppendLine("Tax Registered : " & IIf(bTaxRegistered, "true", "false") & "<br />")
        Return sbPrint.ToString

    End Function
    Public Property TaxRegistered() As Boolean
        Get
            Return bTaxRegistered
        End Get
        Set(ByVal value As Boolean)
            bTaxRegistered = value
        End Set
    End Property
    Public Property TaxRegistrationNumber() As String
        Get
            Return Me.sTaxRegistrationNumber
        End Get
        Set(ByVal value As String)
            Me.sTaxRegistrationNumber = value
        End Set
    End Property


End Class
<Serializable()> Public Class ClaimPartyClientCollection : Inherits CollectionBase

    Public Function Add(ByVal v_oClaimPartyClient As ClaimParty) As Integer
        Return List.Add(v_oClaimPartyClient)
    End Function

    Public Sub Remove(ByVal v_oClaimPartyClient As ClaimParty)
        List.Remove(v_oClaimPartyClient)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As ClaimParty
        Get
            Return List(i)
        End Get
        Set(ByVal value As ClaimParty)
            List(i) = value
        End Set
    End Property


End Class
<Serializable()> Public Class ClaimPartyInsurerCollection : Inherits CollectionBase

    Public Function Add(ByVal v_oClaimPartyInsurer As ClaimPartyInsurer) As Integer
        Return List.Add(v_oClaimPartyInsurer)
    End Function

    Public Sub Remove(ByVal v_oClaimPartyInsurer As ClaimPartyInsurer)
        List.Remove(v_oClaimPartyInsurer)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As ClaimPartyInsurer
        Get
            Return List(i)
        End Get
        Set(ByVal value As ClaimPartyInsurer)
            List(i) = value
        End Set
    End Property


End Class
