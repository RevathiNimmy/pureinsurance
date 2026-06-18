<Serializable()>Public Class ManualJournalApprovalDetails
    Private bTimeStamp As Byte()
    Private bApproved As Boolean
    Private iManualJournalId As Integer
     Private iApprovalUserGroup As Integer
 Private sApproverUserName As String
    Private sComment As String
    Private sDocumentType As String
    Private sBranch As String
    Private dJournalDate As Date
        Private cManualJournalItemCollection As NexusProvider.ManualJournalItemCollection
     Sub New()
        cManualJournalItemCollection = New NexusProvider.ManualJournalItemCollection
    End Sub
      Public Property TimeStamp() As Byte()
        Get
            Return bTimeStamp
        End Get
        Set(ByVal value As Byte())
            bTimeStamp = value
        End Set
    End Property
    Public Property Approved() As Integer
        Get
            Return bApproved
        End Get
        Set(ByVal value As Integer)
            bApproved = value
        End Set
    End Property
    Public Property ManualJournalId() As Integer
        Get
            Return iManualJournalId
        End Get
        Set(ByVal value As Integer)
            iManualJournalId = value
        End Set
    End Property
    Public Property ApprovalUserGroup() As Integer
        Get
            Return iApprovalUserGroup
        End Get
        Set(ByVal value As Integer)
            iApprovalUserGroup = value
        End Set
    End Property
     Public Property ApproverUser() As String
        Get
            Return sApproverUserName
        End Get
        Set(ByVal value As String)
            sApproverUserName = value
        End Set
    End Property
     Public Property Comment() As String
        Get
            Return sComment
        End Get
        Set(ByVal value As String)
            sComment = value
        End Set
    End Property

     Public Property DocumentType() As String
        Get
            Return sDocumentType
        End Get
        Set(ByVal value As String)
            sDocumentType = value
        End Set
    End Property

     Public Property Branch() As String
        Get
            Return sBranch
        End Get
        Set(ByVal value As String)
            sBranch = value
        End Set
    End Property
     Public Property JournalDate() As Date
        Get
            Return dJournalDate
        End Get
        Set(ByVal value As Date)
            dJournalDate = value
        End Set
    End Property
     Public Property ManualJournalItemCollection() As NexusProvider.ManualJournalItemCollection
        Get
            Return cManualJournalItemCollection
        End Get
        Set(ByVal value As NexusProvider.ManualJournalItemCollection)
            cManualJournalItemCollection = value
        End Set
    End Property
End Class
