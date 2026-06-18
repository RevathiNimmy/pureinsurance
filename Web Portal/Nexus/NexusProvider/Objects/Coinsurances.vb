Public Class Coinsurances
    Private iRecoveryKey As Integer

    Private sRecoveryType As String

    Private iPartyKey As Integer

    Private sCoinsurer As String

    Private dSharePercent As Decimal

    Private dRecoveryToDate As Decimal

    '''<remarks/>
    Public Property RecoveryKey() As Integer
        Get
            Return Me.iRecoveryKey
        End Get
        Set(ByVal value As Integer)
            Me.iRecoveryKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property RecoveryType() As String
        Get
            Return Me.sRecoveryType
        End Get
        Set(ByVal value As String)
            Me.sRecoveryType = value
        End Set
    End Property

    '''<remarks/>
    Public Property PartyKey() As Integer
        Get
            Return Me.iPartyKey
        End Get
        Set(ByVal value As Integer)
            Me.iPartyKey = value
        End Set
    End Property

    '''<remarks/>
    Public Property Coinsurer() As String
        Get
            Return Me.sCoinsurer
        End Get
        Set(ByVal value As String)
            Me.sCoinsurer = value
        End Set
    End Property

    '''<remarks/>
    Public Property SharePercent() As Decimal
        Get
            Return Me.dSharePercent
        End Get
        Set(ByVal value As Decimal)
            Me.dSharePercent = value
        End Set
    End Property

    '''<remarks/>
    Public Property RecoveryToDate() As Decimal
        Get
            Return Me.dRecoveryToDate
        End Get
        Set(ByVal value As Decimal)
            Me.dRecoveryToDate = value
        End Set
    End Property
End Class
Public Class CoinsurancesCollection : Inherits CollectionBase



    ''' <summary>
    ''' Add a Coinsurances object to the collection
    ''' </summary>
    Public Function Add(ByVal v_oCoinsurances As Coinsurances) As Integer
        Return List.Add(v_oCoinsurances)
    End Function

    ''' <summary>
    ''' Remove an Coinsurances object from the collection
    ''' </summary>
    Public Sub Remove(ByVal v_oCoinsurances As Coinsurances)
        List.Remove(v_oCoinsurances)
    End Sub

    ''' <summary>
    ''' Remove an Coinsurances object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the Coinsurances object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an Coinsurances object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the Coinsurances object</param>
    ''' <value>The replacement Coinsurances object</value>
    ''' <returns>The Coinsurances object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As Coinsurances
        Get
            Return List(i)
        End Get
        Set(ByVal value As Coinsurances)
            List(i) = value
        End Set
    End Property

End Class