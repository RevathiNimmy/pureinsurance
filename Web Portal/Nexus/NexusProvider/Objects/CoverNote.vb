<Serializable()> Public Class CoverNote : Inherits CoverNoteBookType

    Private sBookNumber As String
    Private iStartNumber As Integer
    Private iEndNumber As Integer
    Private bCoverNoteBookTimestamp() As Byte
    Private iCoverNoteBookKey As Integer
    Private sAgentName As String
    Private iCoverNoteBranchKey As Integer
    Private iCoverNoteBookStatusKey As Integer
    Private dDateCreated As Date
    Private sCoverNoteBookStatusCode As String
    Private sComments As String
    Private sCode As String
    Private iCoverNoteStatusKey As Integer
    Private sDescription As String
    Private dtAssignedDate As DateTime
    Private oCoverNoteSheets As CoverNoteSheetTypeCollection
    Private oCoverNoteProducts As Product
    Private oInsuranceFileDetails As InsuranceFileDetails
    Private oProducts As ProductCollection

    'For FindCoverNoteBooks Method
    Private dtLastUpdated As DateTime
    Private sPolicyNumber As String
    'Private dtAssignedDate As DateTime
    'Private iCoverNoteStatusKey As Integer
    Private sCoverNoteStatusDescription As String
    Private sCoverNoteBranchDescription As String
    Private iMaxRowsToFetch As Integer

    Public Property CoverNoteBranchDescription() As String
        Get
            Return Me.sCoverNoteBranchDescription
        End Get
        Set(ByVal value As String)
            Me.sCoverNoteBranchDescription = value
        End Set
    End Property
    Public Property CoverNoteStatusDescription() As String
        Get
            Return Me.sCoverNoteStatusDescription
        End Get
        Set(ByVal value As String)
            Me.sCoverNoteStatusDescription = value
        End Set
    End Property
    Public Property CoverNoteStatusKey() As Integer
        Get
            Return Me.iCoverNoteStatusKey
        End Get
        Set(ByVal value As Integer)
            Me.iCoverNoteStatusKey = value
        End Set
    End Property
    Public Property LastUpdated() As DateTime
        Get
            Return Me.dtLastUpdated
        End Get
        Set(ByVal value As DateTime)
            Me.dtLastUpdated = value
        End Set
    End Property
    Public Property PolicyNumber() As String
        Get
            Return Me.sPolicyNumber
        End Get
        Set(ByVal value As String)
            Me.sPolicyNumber = value
        End Set
    End Property

    Public Sub New()
        oProducts = New ProductCollection
        oCoverNoteProducts = New Product
        oCoverNoteSheets = New CoverNoteSheetTypeCollection
        oInsuranceFileDetails = New InsuranceFileDetails
    End Sub
    '''<remarks/>
    Public Property BookNumber() As String
        Get
            Return Me.sBookNumber
        End Get
        Set(ByVal value As String)
            Me.sBookNumber = value
        End Set
    End Property

    '''<remarks/>
    Public Property StartNumber() As Integer
        Get
            Return Me.iStartNumber
        End Get
        Set(ByVal value As Integer)
            Me.iStartNumber = value
        End Set
    End Property

    '''<remarks/>
    Public Property EndNumber() As Integer
        Get
            Return Me.iEndNumber
        End Get
        Set(ByVal value As Integer)
            Me.iEndNumber = value
        End Set
    End Property
    Public Property CoverNoteBookProducts() As ProductCollection
        Get
            Return Me.oProducts
        End Get
        Set(ByVal value As ProductCollection)
            Me.oProducts = value
        End Set
    End Property

    Public Property CoverNoteBookTimestamp() As Byte()
        Get
            Return Me.bCoverNoteBookTimestamp
        End Get
        Set(ByVal value As Byte())
            Me.bCoverNoteBookTimestamp = value
        End Set
    End Property
    '''<remarks/>
    Public Property CoverNoteBookKey() As Integer
        Get
            Return Me.iCoverNoteBookKey
        End Get
        Set(ByVal value As Integer)
            Me.iCoverNoteBookKey = value
        End Set
    End Property
    Public Property AgentName() As String
        Get
            Return Me.sAgentName
        End Get
        Set(ByVal value As String)
            Me.sAgentName = value
        End Set
    End Property
    Public Property CoverNoteBranchKey() As Integer
        Get
            Return Me.iCoverNoteBranchKey
        End Get
        Set(ByVal value As Integer)
            Me.iCoverNoteBranchKey = value
        End Set
    End Property
    '''<remarks/>
    Public Property CoverNoteBookStatusKey() As Integer
        Get
            Return Me.iCoverNoteBookStatusKey
        End Get
        Set(ByVal value As Integer)
            Me.iCoverNoteBookStatusKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property DateCreated() As Date
        Get
            Return Me.dDateCreated
        End Get
        Set(ByVal value As Date)
            Me.dDateCreated = value
        End Set
    End Property
    '''<remarks/>
    Public Property CoverNoteBookStatusCode() As String
        Get
            Return Me.sCoverNoteBookStatusCode
        End Get
        Set(ByVal value As String)
            Me.sCoverNoteBookStatusCode = value
        End Set
    End Property
    Public Property CoverNoteProducts() As Product
        Get
            Return Me.oCoverNoteProducts
        End Get
        Set(ByVal value As Product)
            Me.oCoverNoteProducts = value
        End Set
    End Property
    '''<remarks/>
    Public Property Comments() As String
        Get
            Return Me.sComments
        End Get
        Set(ByVal value As String)
            Me.sComments = value
        End Set
    End Property
    Public Property Code() As String
        Get
            Return Me.sCode
        End Get
        Set(ByVal value As String)
            Me.sCode = value
        End Set
    End Property
    Public Property CoverNoteStausKey() As Integer
        Get
            Return Me.iCoverNoteStatusKey
        End Get
        Set(ByVal value As Integer)
            Me.iCoverNoteStatusKey = value
        End Set
    End Property
    '''<remarks/>
    Public Property Description() As String
        Get
            Return Me.sDescription
        End Get
        Set(ByVal value As String)
            Me.sDescription = value
        End Set
    End Property
    Public Property AssignedDate() As DateTime
        Get
            Return Me.dtAssignedDate
        End Get
        Set(ByVal value As DateTime)
            Me.dtAssignedDate = value
        End Set
    End Property
    Public Property CoverNoteSheets() As CoverNoteSheetTypeCollection
        Get
            Return Me.oCoverNoteSheets
        End Get
        Set(ByVal value As CoverNoteSheetTypeCollection)
            Me.oCoverNoteSheets = value
        End Set
    End Property
    Public Property InsuranceFileDetails() As InsuranceFileDetails
        Get
            Return Me.oInsuranceFileDetails
        End Get
        Set(ByVal value As InsuranceFileDetails)
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

End Class


<Serializable()> Public Class CoverNoteCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oCoverNote As CoverNote In List
            sbPrint.AppendLine(oCoverNote.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oCoverNote As CoverNote) As Integer
        Return List.Add(v_oCoverNote)
    End Function

    Public Sub Remove(ByVal v_oCoverNote As CoverNote)
        List.Remove(v_oCoverNote)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As CoverNote
        Get
            Return List(i)
        End Get
        Set(ByVal value As CoverNote)
            List(i) = value
        End Set
    End Property

End Class