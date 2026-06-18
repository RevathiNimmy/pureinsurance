<Serializable()> Public Class RecoveryType
    Private sCode As String
    Private sDescription As String
    Private iIsSalvage As Integer

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

    Public Property IsSalvage() As Integer
        Get
            Return Me.iIsSalvage
        End Get
        Set(ByVal value As Integer)
            Me.iIsSalvage = value
        End Set
    End Property

End Class

<Serializable()> Public Class RecoveryTypeCollection : Inherits SortableCollectionBase

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string of the objects contents</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        'For Each oRecoveryType As RecoveryType In List
        '    sbPrint.AppendLine(oRecoveryType.Print())
        '    sbPrint.AppendLine("<br />")
        'Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add a RecoveryType object to the collection
    ''' </summary>
    Public Function Add(ByVal v_oRecoveryType As RecoveryType) As Integer
        Return List.Add(v_oRecoveryType)
    End Function

    ''' <summary>
    ''' Remove an RecoveryType object from the collection
    ''' </summary>
    Public Sub Remove(ByVal v_oRecoveryType As RecoveryType)
        List.Remove(v_oRecoveryType)
    End Sub

    ''' <summary>
    ''' Remove an RecoveryType object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the RecoveryType object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an RecoveryType object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the RecoveryType object</param>
    ''' <value>The replacement RecoveryType object</value>
    ''' <returns>The RecoveryType object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As RecoveryType
        Get
            Return List(i)
        End Get
        Set(ByVal value As RecoveryType)
            List(i) = value
        End Set
    End Property

End Class