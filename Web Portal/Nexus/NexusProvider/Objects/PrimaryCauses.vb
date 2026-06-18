''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class PrimaryCauses

#Region "Private Variables"

    Private sCode As String
    Private sDescription As String
    Private iPrimaryCauseId As Integer

#End Region

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub New()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        sbPrint.AppendLine("Code : " & sCode.ToString() & "<br />")
        sbPrint.AppendLine("Description : " & sDescription.ToString() & "<br />")
        sbPrint.AppendLine("Primary Cause Id : " & iPrimaryCauseId.ToString() & "<br />")

        Return sbPrint.ToString()

    End Function

#Region "Public Properties"

    Public Property Code() As String
        Get
            Return Me.sCode
        End Get
        Set(ByVal value As String)
            Me.sCode = value
        End Set
    End Property

    Public Property Description() As String
        Get
            Return Me.sDescription
        End Get
        Set(ByVal value As String)
            Me.sDescription = value
        End Set
    End Property

    Public Property PrimaryCauseId() As Integer
        Get
            Return Me.iPrimaryCauseId
        End Get
        Set(ByVal value As Integer)
            Me.iPrimaryCauseId = value
        End Set
    End Property

#End Region

End Class

''' <summary>
''' 
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class PrimaryCausesCollections : Inherits SortableCollectionBase
    Public Sub New()
        MyBase.SortObjectType = GetType(PrimaryCauses)
    End Sub

    Public Function Print() As String
        Dim sbPrint As New Text.StringBuilder()

        For Each oPrimaryCauses As PrimaryCauses In List
            sbPrint.AppendLine(oPrimaryCauses.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    Public Function Add(ByVal v_oPrimaryCauses As PrimaryCauses) As Integer
        Return List.Add(v_oPrimaryCauses)
    End Function

    Public Sub Remove(ByVal v_oPrimaryCauses As PrimaryCauses)
        List.Remove(v_oPrimaryCauses)
    End Sub

    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    Default Public Property Item(ByVal i As Integer) As PrimaryCauses
        Get
            Return List(i)
        End Get
        Set(ByVal value As PrimaryCauses)
            List(i) = value
        End Set
    End Property

End Class
