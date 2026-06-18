Public Class AddCoverNoteBook : Inherits CoverNoteBookType

    Private sBookNumber As String
    Private iStartNumber As Integer
    Private iEndNumber As Integer
    Private bCoverNoteBookTimestamp() As Byte
    Private oProducts As ProductCollection
    Public Sub New()
        oProducts = New ProductCollection
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

End Class
