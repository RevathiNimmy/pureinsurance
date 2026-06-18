<Serializable()> Public Class CoverNoteSheetType
    Private iCoverNoteSheetKey As Integer
    Private iCoverNoteSheetNumber As Integer
    Private sCustomerName As String
    Private iCoverNoteSheetStatusKey As Integer
    Private bCoverNoteSheetStatusKeySpecified As Boolean
    Private sCoverNoteSheetStatusCode As String
    Private sCoverNoteSheetStatusDescription As String
    Private sPolicyNumber As String
    Private sBranchName As String
    Private sAgentName As String
    Private iOldCoverNoteSheetNumber As Integer
    Private iNewCoverNoteSheetNumber As Integer
    Private dDateImported As Date
    Public Property CoverNoteSheetKey() As Integer
        Get
            Return Me.iCoverNoteSheetKey
        End Get
        Set(ByVal value As Integer)
            Me.iCoverNoteSheetKey = value
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
    Public Property CustomerName() As String
        Get
            Return Me.sCustomerName
        End Get
        Set(ByVal value As String)
            Me.sCustomerName = value
        End Set
    End Property

    '''<remarks/>
    Public Property CoverNoteSheetStatusKey() As Integer
        Get
            Return Me.iCoverNoteSheetStatusKey
        End Get
        Set(ByVal value As Integer)
            Me.iCoverNoteSheetStatusKey = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlIgnoreAttribute()> _
    Public Property CoverNoteSheetStatusKeySpecified() As Boolean
        Get
            Return Me.bCoverNoteSheetStatusKeySpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bCoverNoteSheetStatusKeySpecified = value
        End Set
    End Property

    '''<remarks/>
    Public Property CoverNoteSheetStatusCode() As String
        Get
            Return Me.sCoverNoteSheetStatusCode
        End Get
        Set(ByVal value As String)
            Me.sCoverNoteSheetStatusCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property CoverNoteSheetStatusDescription() As String
        Get
            Return Me.sCoverNoteSheetStatusDescription
        End Get
        Set(ByVal value As String)
            Me.sCoverNoteSheetStatusDescription = value
        End Set
    End Property

    '''<remarks/>
    Public Property PolicyNumber() As String
        Get
            Return Me.sPolicyNumber
        End Get
        Set(ByVal value As String)
            Me.sPolicyNumber = value
        End Set
    End Property

    '''<remarks/>
    Public Property BranchName() As String
        Get
            Return Me.sBranchName
        End Get
        Set(ByVal value As String)
            Me.sBranchName = value
        End Set
    End Property

    '''<remarks/>
    Public Property AgentName() As String
        Get
            Return Me.sAgentName
        End Get
        Set(ByVal value As String)
            Me.sAgentName = value
        End Set
    End Property
    Public Property OldCoverNoteSheetNumber() As Integer
        Get
            Return Me.iOldCoverNoteSheetNumber
        End Get
        Set(ByVal value As Integer)
            Me.iOldCoverNoteSheetNumber = value
        End Set
    End Property
    Public Property NewCoverNoteSheetNumber() As Integer
        Get
            Return Me.iNewCoverNoteSheetNumber
        End Get
        Set(ByVal value As Integer)
            Me.iNewCoverNoteSheetNumber = value
        End Set
    End Property

    '''<remarks/>
    Public Property DateImported() As Date
        Get
            Return Me.dDateImported
        End Get
        Set(ByVal value As Date)
            Me.dDateImported = value
        End Set
    End Property
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("CoverNoteSheetKey : " & iCoverNoteSheetKey.ToString & "<br />")
        sbPrint.AppendLine("CoverNoteSheetNumber: " & iCoverNoteSheetNumber.ToString & "<br />")
        sbPrint.AppendLine("DateImported : " & dDateImported & "<br />")
        sbPrint.AppendLine("<br />")

        Return sbPrint.ToString()

    End Function
End Class
<Serializable()> Public Class CoverNoteSheetTypeCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oCoverNoteSheetType As CoverNoteSheetType In List
            sbPrint.AppendLine(oCoverNoteSheetType.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oCoverNoteSheetType As CoverNoteSheetType) As Integer
        Return List.Add(v_oCoverNoteSheetType)
    End Function

    Public Sub Remove(ByVal v_oCoverNoteSheetType As CoverNoteSheetType)
        List.Remove(v_oCoverNoteSheetType)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As CoverNoteSheetType
        Get
            Return List(i)
        End Get
        Set(ByVal value As CoverNoteSheetType)
            List(i) = value
        End Set
    End Property

End Class

