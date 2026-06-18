<Serializable()> Public Class ReserveType
    Private sCode As String
    Private sDescription As String

    Private oPeril As PerilCollection
    Public Sub New()
        oPeril = New PerilCollection
    End Sub
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
    Public Property Perils() As PerilCollection
        Get
            Return Me.oPeril
        End Get
        Set(ByVal value As PerilCollection)
            Me.oPeril = value
        End Set
    End Property
End Class
<Serializable()> Public Class ReserveTypeCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string of the objects contents</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        'For Each oDocument As Document In List
        '    sbPrint.AppendLine(oDocument.Print())
        '    sbPrint.AppendLine("<br />")
        'Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add a ReserveType object to the collection
    ''' </summary>
    Public Function Add(ByVal v_oReserveType As ReserveType) As Integer
        Return List.Add(v_oReserveType)
    End Function

    ''' <summary>
    ''' Remove an ReserveType object from the collection
    ''' </summary>
    Public Sub Remove(ByVal v_oReserveType As ReserveType)
        List.Remove(v_oReserveType)
    End Sub

    ''' <summary>
    ''' Remove an ReserveType object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the ReserveType object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an ReserveType object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the ReserveType object</param>
    ''' <value>The replacement ReserveType object</value>
    ''' <returns>The SalvageRecovery object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As ReserveType
        Get
            Return List(i)
        End Get
        Set(ByVal value As ReserveType)
            List(i) = value
        End Set
    End Property

End Class