<Serializable()> Public Class CoverNoteBookType
    Private dEffectiveDate As Date
    Private iAgentKey As Integer
    Private bAgentKeySpecified As Boolean
    Private sCoverNoteStatusCode As String
    Private sCoverNoteBranchCode As String

    Public Property EffectiveDate() As Date
        Get
            Return Me.dEffectiveDate
        End Get
        Set(ByVal value As Date)
            Me.dEffectiveDate = value
        End Set
    End Property

    '''<remarks/>
    Public Property AgentKey() As Integer
        Get
            Return Me.iAgentKey
        End Get
        Set(ByVal value As Integer)
            Me.iAgentKey = value
        End Set
    End Property

    '''<remarks/>
    <System.Xml.Serialization.XmlIgnoreAttribute()> _
    Public Property AgentKeySpecified() As Boolean
        Get
            Return Me.bAgentKeySpecified
        End Get
        Set(ByVal value As Boolean)
            Me.bAgentKeySpecified = value
        End Set
    End Property

    '''<remarks/>
    Public Property CoverNoteStatusCode() As String
        Get
            Return Me.sCoverNoteStatusCode
        End Get
        Set(ByVal value As String)
            Me.sCoverNoteStatusCode = value
        End Set
    End Property

    '''<remarks/>
    Public Property CoverNoteBranchCode() As String
        Get
            Return Me.sCoverNoteBranchCode
        End Get
        Set(ByVal value As String)
            Me.sCoverNoteBranchCode = value
        End Set
    End Property
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("EffectiveDate : " & dEffectiveDate.ToString & "<br />")
        sbPrint.AppendLine("AgentKey : " & iAgentKey.ToString & "<br />")
        sbPrint.AppendLine("AgentKeySpecified : " & IIf(bAgentKeySpecified, "true", "false") & "<br />")
        sbPrint.AppendLine("CoverNoteStatusCode : " & sCoverNoteStatusCode & "<br />")
        sbPrint.AppendLine("CoverNoteBranchCode : " & sCoverNoteBranchCode & "<br />")
        sbPrint.AppendLine("<br />")

        Return sbPrint.ToString()

    End Function
End Class


<Serializable()> Public Class CoverNoteBookTypeCollection : Inherits CollectionBase

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oCoverNoteBookType As CoverNoteBookType In List
            sbPrint.AppendLine(oCoverNoteBookType.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oCoverNoteBookType As CoverNoteBookType) As Integer
        Return List.Add(v_oCoverNoteBookType)
    End Function

    Public Sub Remove(ByVal v_oCoverNoteBookType As CoverNoteBookType)
        List.Remove(v_oCoverNoteBookType)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As CoverNoteBookType
        Get
            Return List(i)
        End Get
        Set(ByVal value As CoverNoteBookType)
            List(i) = value
        End Set
    End Property

End Class
