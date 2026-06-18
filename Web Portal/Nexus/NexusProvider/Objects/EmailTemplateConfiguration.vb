<Serializable()> Public Class EmailTemplateConfiguration
    Private sID As String
    Private sPath As String
    Private sSender As String
    Private sRecipient As String
    Private sSubject As String
    Private sProductCode As String
    Private sEmailTemplateCode As String
    Private sSubjectTemplateCode As String
    Private sTransactionType As String

    Public Property ID() As String
        Get
            Return Me.sID
        End Get
        Set(ByVal value As String)
            Me.sID = value
        End Set
    End Property

    Public Property Path() As String
        Get
            Return Me.sPath
        End Get
        Set(ByVal value As String)
            Me.sPath = value
        End Set
    End Property

    Public Property Sender() As String
        Get
            Return Me.sSender
        End Get
        Set(ByVal value As String)
            Me.sSender = value
        End Set
    End Property

    Public Property Recipient() As String
        Get
            Return Me.sRecipient
        End Get
        Set(ByVal value As String)
            Me.sRecipient = value
        End Set
    End Property

    Public Property Subject() As String
        Get
            Return Me.sSubject
        End Get
        Set(ByVal value As String)
            Me.sSubject = value
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

    Public Property EmailTemplateCode() As String
        Get
            Return Me.sEmailTemplateCode
        End Get
        Set(ByVal value As String)
            Me.sEmailTemplateCode = value
        End Set
    End Property

    Public Property SubjectTemplateCode() As String
        Get
            Return Me.sSubjectTemplateCode
        End Get
        Set(ByVal value As String)
            Me.sSubjectTemplateCode = value
        End Set
    End Property

    Public Property TransactionType() As String
        Get
            Return Me.sTransactionType
        End Get
        Set(ByVal value As String)
            Me.sTransactionType = value
        End Set
    End Property
End Class
