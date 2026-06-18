Public Class MMSiteMapNode

    Private iSitemapID As Integer
    Private iParentID As Integer
    Private iDepth As Integer
    Private sPageTypeName As String  'Name will determine folder to use for templates/icons etc ...
    Private sLabel As String
    Private dLiveDate As DateTime
    Private dExpiryDate As DateTime
    Private bHidden As Boolean
    Private bSubmitted As Boolean
    Private bApproved As Boolean
    Private bPublished As Boolean
    Private bDeleted As Boolean
    Private bAuthenticatedOnly As Boolean
    Private bAnonymousOnly As Boolean
    Private sFullFolderPath As String

    Public Sub New(ByVal SiteMapID As Integer, ByVal ParentID As Integer, ByVal Depth As Integer, _
                ByVal PageTypeName As String, ByVal Label As String, ByVal LiveDate As DateTime, _
                ByVal ExpiryDate As DateTime, ByVal Hidden As Boolean, ByVal AuthenticatedOnly As Boolean, ByVal AnonymousOnly As Boolean, ByVal Submitted As Boolean, _
                ByVal Approved As Boolean, ByVal Published As Boolean, ByVal Deleted As Boolean, ByVal FullFolderPath As String)

        iSitemapID = SiteMapID
        iParentID = ParentID
        iDepth = Depth
        sPageTypeName = PageTypeName.TrimEnd
        sLabel = Label
        dLiveDate = LiveDate
        dExpiryDate = ExpiryDate
        bHidden = Hidden
        bAuthenticatedOnly = AuthenticatedOnly
        bAnonymousOnly = AnonymousOnly
        bSubmitted = Submitted
        bApproved = Approved
        bPublished = Published
        bDeleted = Deleted
        sFullFolderPath = FullFolderPath

    End Sub

    Public ReadOnly Property SiteMapID() As Integer
        Get
            Return iSitemapID
        End Get
    End Property

    Public ReadOnly Property ParentID() As Integer
        Get
            Return iParentID
        End Get
    End Property

    Public ReadOnly Property Depth() As Integer
        Get
            Return iDepth
        End Get
    End Property

    Public ReadOnly Property PageTypeName() As String
        Get
            Return sPageTypeName
        End Get
    End Property

    Public ReadOnly Property Label() As String
        Get
            Return sLabel
        End Get
    End Property

    Public ReadOnly Property LiveDate() As DateTime
        Get
            If dLiveDate = DateTime.MinValue Then
                Return Nothing
            Else
                Return dLiveDate
            End If
        End Get
    End Property

    Public ReadOnly Property ExpiryDate() As DateTime
        Get
            If dExpiryDate = DateTime.MinValue Then
                Return Nothing
            Else
                Return dExpiryDate
            End If
        End Get
    End Property

    Public ReadOnly Property Hidden() As Boolean
        Get
            Return bHidden
        End Get
    End Property

    Public ReadOnly Property AnonymousOnly() As Boolean
        Get
            Return bAnonymousOnly
        End Get
    End Property

    Public ReadOnly Property AuthenticatedOnly() As Boolean
        Get
            Return bAuthenticatedOnly
        End Get
    End Property

    Public ReadOnly Property Submitted() As Boolean
        Get
            Return bSubmitted
        End Get
    End Property

    Public ReadOnly Property Approved() As Boolean
        Get
            Return bApproved
        End Get
    End Property

    Public ReadOnly Property Published() As Boolean
        Get
            Return bPublished
        End Get
    End Property

    Public ReadOnly Property Deleted() As Boolean
        Get
            Return bDeleted
        End Get
    End Property


    Public Property FullFolderPath() As String
        Get
            Return sFullFolderPath
        End Get
        Set(ByVal value As String)
            sFullFolderPath = value
        End Set
    End Property

End Class
