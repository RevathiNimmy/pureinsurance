<Serializable()> Public Class ReinsuranceBands
    Private iClaimID As Integer
    Private iMode As Integer

    Private iBandID As Integer
    Private sBand As String
    Private iBandKey As Integer

    Public Property ClaimID() As Integer
        Get
            Return Me.iClaimID
        End Get
        Set(ByVal value As Integer)
            Me.iClaimID = value
        End Set
    End Property

    Public Property Mode() As Integer
        Get
            Return Me.iMode
        End Get
        Set(ByVal value As Integer)
            Me.iMode = value
        End Set
    End Property

    Public Property BandID() As Integer
        Get
            Return Me.iBandID
        End Get
        Set(ByVal value As Integer)
            Me.iBandID = value
        End Set
    End Property

    Public Property Band() As String
        Get
            Return Me.sBand
        End Get
        Set(ByVal value As String)
            Me.sBand = value
        End Set
    End Property
    Public Property BandKey() As Integer
        Get
            Return Me.iBandKey
        End Get
        Set(ByVal value As Integer)
            Me.iBandKey = value
        End Set
    End Property
End Class

<Serializable()> Public Class ReinsuranceBandsCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string of the objects contents</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        'For Each oReinsuranceBands As RecoveryType In List
        '    sbPrint.AppendLine(oReinsuranceBands.Print())
        '    sbPrint.AppendLine("<br />")
        'Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add a ReinsuranceBands object to the collection
    ''' </summary>
    Public Function Add(ByVal v_oReinsuranceBands As ReinsuranceBands) As Integer
        Return List.Add(v_oReinsuranceBands)
    End Function

    ''' <summary>
    ''' Remove an ReinsuranceBands object from the collection
    ''' </summary>
    Public Sub Remove(ByVal v_oReinsuranceBands As ReinsuranceBands)
        List.Remove(v_oReinsuranceBands)
    End Sub

    ''' <summary>
    ''' Remove an ReinsuranceBands object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the ReinsuranceBands object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an ReinsuranceBands object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the ReinsuranceBands object</param>
    ''' <value>The replacement ReinsuranceBands object</value>
    ''' <returns>The ReinsuranceBands object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As ReinsuranceBands
        Get
            Return List(i)
        End Get
        Set(ByVal value As ReinsuranceBands)
            List(i) = value
        End Set
    End Property

End Class