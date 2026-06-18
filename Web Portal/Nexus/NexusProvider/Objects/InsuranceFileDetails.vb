<Serializable()> Public Class InsuranceFileDetails
#Region "Private Variables"

    Private iInsuranceFileKey As Integer
    Private sInsuranceRef As String
    Private sInsuranceFileType As String
    Private sClientName As String
    Private sClientShortName As String
    Private iPartyKey As Integer
    Private dtLastModifiedDate As DateTime
    Private iInsuranceFolderKey As Integer
    Private iInsuranceFileCnt As Integer
    Private sProductCode As String
    Private sProductDescription As String
    Private sRiskIndex As String
    Private sValue As String
    Private sStatus As String

    'Fields for FindInsuranceFileForClaims - Begin
    Private sInsuranceFileRef As String
    Private dtSearchDate As Date
    Private iCoverNoteSheetNumber As Integer
    Private sPostCode As String
    Private dtInForceFrom As Date
    Private dtInForceTo As Date

    Private iLeadAgentKey As Integer
    Private iIsSourceClosed As Integer
    Private dtRenewalDate As Date
    Private sClientAddressLine1 As String
    Private dtLossDate As Date
    Private iStatusId As Integer
    Private iCountryKey As Integer
    Private oAddress As NexusProvider.Address
    'End
    Private oInsuranceFileDetails As InsuranceFileDetailsCollection
    Private sFax, sEmail, sMobile, sTelOff, sTelHome, sInsuranceFileStatusCode As String, sInsuranceFileTypeCode
    Private iMaxRowsToFetch As Integer
    Private dtCoverFrom, dtCoverTo As Date, dtInceptionDate As Date, dtLapseDate As Date
    Private sLeadAgentName As String
    Private allowedClosedBranchClaimsField As Integer
    Private sAssociatedClients As String = String.Empty
#End Region

    Sub New()
        oAddress = New NexusProvider.Address
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Insurance File Key : " & iInsuranceFileKey.ToString() & "<br />")
        sbPrint.AppendLine("Insurance Ref : " & sInsuranceRef & "<br />")
        sbPrint.AppendLine("Insurance File Type : " & sInsuranceFileType & "<br />")
        sbPrint.AppendLine("Client Name : " & sClientName & "<br />")
        sbPrint.AppendLine("Client Short Name : " & sClientShortName & "<br />")
        sbPrint.AppendLine("Party Key : " & iPartyKey.ToString() & "<br />")
        sbPrint.AppendLine("Last Modified Date : " & dtLastModifiedDate.ToString() & "<br />")
        sbPrint.AppendLine("Insurance Folder Key : " & iInsuranceFolderKey.ToString() & "<br />")
        sbPrint.AppendLine("Product Code : " & sProductCode & "<br />")
        sbPrint.AppendLine("Product Description : " & sProductDescription & "<br />")
        sbPrint.AppendLine("Risk Index : " & sRiskIndex & "<br />")
        sbPrint.AppendLine("Value : " & sValue & "<br />")
        sbPrint.AppendLine("Status : " & sStatus & "<br />")
        sbPrint.AppendLine("Max Rows ToFetch: " & iMaxRowsToFetch.ToString & "<br />")

        Return sbPrint.ToString()

    End Function
   
#Region "Public Properties"
    Public Property InsuranceFileStatusCode() As String
        Get
            Return Me.sInsuranceFileStatusCode
        End Get
        Set(ByVal value As String)
            Me.sInsuranceFileStatusCode = value
        End Set
    End Property
    Public Property CountryKey() As Integer
        Get
            Return iCountryKey
        End Get
        Set(ByVal value As Integer)
            iCountryKey = value
        End Set
    End Property
    Public Property TelHome() As String
        Get
            Return Me.sTelHome
        End Get
        Set(ByVal value As String)
            Me.sTelHome = value
        End Set
    End Property
    Public Property TelOff() As String
        Get
            Return Me.sTelOff
        End Get
        Set(ByVal value As String)
            Me.sTelOff = value
        End Set
    End Property
    Public Property Mobile() As String
        Get
            Return Me.sMobile
        End Get
        Set(ByVal value As String)
            Me.sMobile = value
        End Set
    End Property
    Public Property Email() As String
        Get
            Return Me.sEmail
        End Get
        Set(ByVal value As String)
            Me.sEmail = value
        End Set
    End Property
    Public Property Fax() As String
        Get
            Return Me.sFax
        End Get
        Set(ByVal value As String)
            Me.sFax = value
        End Set
    End Property


    Public Property Address() As NexusProvider.Address
        Get
            Return Me.oAddress
        End Get
        Set(ByVal value As NexusProvider.Address)
            Me.oAddress = value
        End Set
    End Property

    Public Property InsuranceFileKey() As Integer
        Get
            Return Me.iInsuranceFileKey
        End Get
        Set(ByVal value As Integer)
            Me.iInsuranceFileKey = value
        End Set
    End Property

    Public Property InsuranceRef() As String
        Get
            Return Me.sInsuranceRef
        End Get
        Set(ByVal value As String)
            Me.sInsuranceRef = value
        End Set
    End Property

    Public Property InsuranceFileType() As String
        Get
            Return Me.sInsuranceFileType
        End Get
        Set(ByVal value As String)
            Me.sInsuranceFileType = value
        End Set
    End Property

    Public Property ClientName() As String
        Get
            Return Me.sClientName
        End Get
        Set(ByVal value As String)
            Me.sClientName = value
        End Set
    End Property

    Public Property ClientShortName() As String
        Get
            Return Me.sClientShortName
        End Get
        Set(ByVal value As String)
            Me.sClientShortName = value
        End Set
    End Property

    Public Property PartyKey() As Integer
        Get
            Return Me.iPartyKey
        End Get
        Set(ByVal value As Integer)
            Me.iPartyKey = value
        End Set
    End Property

    Public Property LastModifiedDate() As DateTime
        Get
            Return Me.dtLastModifiedDate
        End Get
        Set(ByVal value As DateTime)
            Me.dtLastModifiedDate = value
        End Set
    End Property

    Public Property InsuranceFolderKey() As Integer
        Get
            Return Me.iInsuranceFolderKey
        End Get
        Set(ByVal value As Integer)
            Me.iInsuranceFolderKey = value
        End Set
    End Property
    Public Property InsuranceFileCnt() As Integer
        Get
            Return Me.iInsuranceFileCnt
        End Get
        Set(ByVal value As Integer)
            Me.iInsuranceFileCnt = value
        End Set
    End Property
    Public Property StatusID() As Integer
        Get
            Return Me.iStatusId
        End Get
        Set(ByVal value As Integer)
            Me.iStatusId = value
        End Set
    End Property

    Public Property ProductCode() As String
        Get
            Return Me.sProductCode
        End Get
        Set(ByVal value As String)
            Me.sProductCode = value
        End Set
    End Property

    Public Property ProductDescription() As String
        Get
            Return Me.sProductDescription
        End Get
        Set(ByVal value As String)
            Me.sProductDescription = value
        End Set
    End Property

    Public Property RiskIndex() As String
        Get
            Return Me.sRiskIndex
        End Get
        Set(ByVal value As String)
            Me.sRiskIndex = value
        End Set
    End Property

    Public Property Value() As String
        Get
            Return Me.sValue
        End Get
        Set(ByVal value As String)
            Me.sValue = value
        End Set
    End Property

    Public Property Status() As String
        Get
            Return Me.sStatus
        End Get
        Set(ByVal value As String)
            Me.sStatus = value
        End Set
    End Property

    ' FindInsuranceFilefor claims
    '''<remarks/>
    Public Property InsuranceFileRef() As String
        Get
            Return Me.sInsuranceFileRef
        End Get
        Set(ByVal value As String)
            Me.sInsuranceFileRef = value
        End Set
    End Property

    '''<remarks/>
    Public Property SearchDate() As Date
        Get
            Return Me.dtSearchDate
        End Get
        Set(ByVal value As Date)
            Me.dtSearchDate = value
        End Set
    End Property
    '''<remarks/>
    Public Property CoverNoteSheetNumber() As Integer
        Get
            Return Me.iCoverNoteSheetNumber
        End Get
        Set(ByVal value As Integer)
            Me.iCoverNoteSheetNumber = value
        End Set
    End Property
    '''<remarks/>
    Public Property PostCode() As String
        Get
            Return Me.sPostCode
        End Get
        Set(ByVal value As String)
            Me.sPostCode = value
        End Set
    End Property
    '''<remarks/>
    Public Property InForceFrom() As Date
        Get
            Return Me.dtInForceFrom
        End Get
        Set(ByVal value As Date)
            Me.dtInForceFrom = value
        End Set
    End Property
    '''<remarks/>
    Public Property InForceTo() As Date
        Get
            Return Me.dtInForceTo
        End Get
        Set(ByVal value As Date)
            Me.dtInForceTo = value
        End Set
    End Property

    '''<remarks/>
    Public Property IsSourceClosed() As Integer
        Get
            Return Me.iIsSourceClosed
        End Get
        Set(ByVal value As Integer)
            Me.iIsSourceClosed = value
        End Set
    End Property
    '''<remarks/>
    Public Property RenewalDate() As Date
        Get
            Return Me.dtRenewalDate
        End Get
        Set(ByVal value As Date)
            Me.dtRenewalDate = value
        End Set
    End Property
    '''<remarks/>
    Public Property LossDate() As Date
        Get
            Return Me.dtLossDate
        End Get
        Set(ByVal value As DateTime)
            Me.dtLossDate = value
        End Set
    End Property
    '''<remarks/>
    Public Property ClientAddressLine1() As String
        Get
            Return Me.sClientAddressLine1
        End Get
        Set(ByVal value As String)
            Me.sClientAddressLine1 = value
        End Set
    End Property
    '''<remarks/>
    Public Property LeadAgentKey() As Integer
        Get
            Return Me.iLeadAgentKey
        End Get
        Set(ByVal value As Integer)
            Me.iLeadAgentKey = value
        End Set
    End Property

    Public Property AllowedClosedBranchClaims() As Integer
        Get
            Return Me.AllowedClosedBranchClaimsField
        End Get
        Set(ByVal value As Integer)
            Me.AllowedClosedBranchClaimsField = value
        End Set
    End Property


    Public Property InsuranceFileDetails() As InsuranceFileDetailsCollection
        Get
            Return Me.oInsuranceFileDetails
        End Get
        Set(ByVal value As InsuranceFileDetailsCollection)
            Me.oInsuranceFileDetails = value
        End Set
    End Property

    Public Property MaxRowsToFetch() As Integer
        Get
            Return iMaxRowsToFetch
        End Get
        Set(ByVal value As Integer)
            iMaxRowsToFetch = value
        End Set
    End Property

    '''<remarks/>
    Public Property CoverFrom() As Date
        Get
            Return Me.dtCoverFrom
        End Get
        Set(ByVal value As Date)
            Me.dtCoverFrom = value
        End Set
    End Property

    '''<remarks/>
    Public Property CoverTo() As Date
        Get
            Return Me.dtCoverTo
        End Get
        Set(ByVal value As Date)
            Me.dtCoverTo = value
        End Set
    End Property

    '''<remarks/>
    Public Property LeadAgentName() As String
        Get
            Return Me.sLeadAgentName
        End Get
        Set(ByVal value As String)
            Me.sLeadAgentName = value
        End Set
    End Property

    Public Property InceptionDate() As Date
        Get
            Return Me.dtInceptionDate
        End Get
        Set(ByVal value As Date)
            Me.dtInceptionDate = value
        End Set
    End Property

    Public Property InsuranceFileTypeCode() As String
        Get
            Return Me.sInsuranceFileTypeCode
        End Get
        Set(ByVal value As String)
            Me.sInsuranceFileTypeCode = value
        End Set
    End Property

    Public Property LapseDate() As Date
        Get
            Return Me.dtLapseDate
        End Get
        Set(ByVal value As Date)
            Me.dtLapseDate = value
        End Set
    End Property

    ''' <summary>
    ''' To check policy created from Market Place 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsMarketPlacePolicy As Boolean = False

    ''' <summary>
    ''' Issue date for polcy version
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IssuedDate As Date

    ''' <summary>
    ''' Transaction date for policy version
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property TransactionDate As Date

    ''' <summary>
    ''' Is policy version read only?
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsReadOnly As Boolean

    ''' <summary>
    ''' Is policy version Cancelled?
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property IsCancelled As Boolean

    ''' <summary>
    ''' Number of versions available for the policy
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NoOfVersions As Integer

    ''' <summary>
    ''' Number of renewal versions available for the policy
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property NoOfRenewalVersions As Integer

    ''' <summary>
    ''' Policy status
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property InsuranceFileStatusDescription() As String

    ''' <summary>
    ''' To check if marked quote for collection
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property MarkedQuoteForCollection As Boolean = False

    ''' <summary>
    ''' quote expiry date
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property QuoteExpiryDate As Date

    ''' <summary>
    ''' Associated Clients
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property AssociatedClients As String

    Public Property FileCode() As String
      
    '''<remarks/>
    Public Property DOBirth() As Date
#End Region
   

End Class
<Serializable()> Public Class InsuranceFileDetailsCollection : Inherits SortableCollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oInsuranceFileDetails As InsuranceFileDetails In List
            sbPrint.AppendLine(oInsuranceFileDetails.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Sub New()
        MyBase.SortObjectType = GetType(InsuranceFileDetails)
    End Sub


    Public Function Add(ByVal v_oInsuranceFileDetails As InsuranceFileDetails) As Integer
        Return List.Add(v_oInsuranceFileDetails)
    End Function

    Public Sub Remove(ByVal v_oInsuranceFileDetails As InsuranceFileDetails)
        List.Remove(v_oInsuranceFileDetails)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As InsuranceFileDetails
        Get
            Return List(i)
        End Get
        Set(ByVal value As InsuranceFileDetails)
            List(i) = value
        End Set
    End Property

End Class
Public Enum InsuranceFileType
    ALL = 0
    QUOTE = 1
    MTAQUOTE = 2
    POLICY = 3
    RENEWAL = 4
End Enum