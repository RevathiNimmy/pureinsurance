<Serializable()> Public Class ArrangementsType

    Private iArrangementId As Integer

    Private iBandId As Integer

    Private iModelId As Integer

    Private dSumInsured As Double

    Private dPremium, dExtendedLimitAmount As Double

    Private bIsOriginal, bIsExtendedLimitapplied As Boolean

    Private bIsModified As Boolean

    Private iFACPremiumType As Integer

    Private sRIModelCode As String

    Private nXOLRIModelID As Integer

    Private sXOLRIModelCode As String

    '''<remarks/>
    Public Property ExtendedLimitAmount() As Double
        Get
            Return Me.dExtendedLimitAmount
        End Get
        Set(ByVal value As Double)
            Me.dExtendedLimitAmount = value
        End Set
    End Property

    '''<remarks/>
    Public Property IsExtendedLimitapplied() As Boolean
        Get
            Return Me.bIsExtendedLimitapplied
        End Get
        Set(ByVal value As Boolean)
            Me.bIsExtendedLimitapplied = value
        End Set
    End Property
    '''<remarks/>
    Public Property ArrangementId() As Integer
        Get
            Return Me.iArrangementId
        End Get
        Set(ByVal value As Integer)
            Me.iArrangementId = value
        End Set
    End Property

    '''<remarks/>
    Public Property BandId() As Integer
        Get
            Return Me.iBandId
        End Get
        Set(ByVal value As Integer)
            Me.iBandId = value
        End Set
    End Property

    '''<remarks/>
    Public Property ModelId() As Integer
        Get
            Return Me.iModelId
        End Get
        Set(ByVal value As Integer)
            Me.iModelId = value
        End Set
    End Property

    '''<remarks/>
    Public Property SumInsured() As Double
        Get
            Return Me.dSumInsured
        End Get
        Set(ByVal value As Double)
            Me.dSumInsured = value
        End Set
    End Property

    '''<remarks/>
    Public Property Premium() As Double
        Get
            Return Me.dPremium
        End Get
        Set(ByVal value As Double)
            Me.dPremium = value
        End Set
    End Property

    '''<remarks/>
    Public Property IsOriginal() As Boolean
        Get
            Return Me.bIsOriginal
        End Get
        Set(ByVal value As Boolean)
            Me.bIsOriginal = value
        End Set
    End Property

    '''<remarks/>
    Public Property IsModified() As Boolean
        Get
            Return Me.bIsModified
        End Get
        Set(ByVal value As Boolean)
            Me.bIsModified = value
        End Set
    End Property

    '''<remarks/>
    Public Property FACPremiumType() As Integer
        Get
            Return Me.iFACPremiumType
        End Get
        Set(ByVal value As Integer)
            Me.iFACPremiumType = value
        End Set
    End Property

    '''<remarks/>
    Public Property RIModelCode() As String
        Get
            Return Me.sRIModelCode
        End Get
        Set(ByVal value As String)
            Me.sRIModelCode = value
        End Set
    End Property

    Public Property XOLRIModelID() As Integer
        Get
            Return Me.nXOLRIModelID
        End Get
        Set(ByVal value As Integer)
            Me.nXOLRIModelID = value
        End Set
    End Property


    Public Property XOLRIModelCode() As String
        Get
            Return Me.sXOLRIModelCode
        End Get
        Set(ByVal value As String)
            Me.sXOLRIModelCode = value
        End Set
    End Property
    Public Property RiOverrideReasonId As Integer
    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string of the objects contents</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("BandId : " & iBandId.ToString() & "<br />")
        sbPrint.AppendLine("ArrangementId : " & iArrangementId.ToString() & "<br />")
        sbPrint.AppendLine("ModelId : " & iModelId.ToString() & "<br />")
        sbPrint.AppendLine("SumInsured : " & dSumInsured.ToString() & "<br />")
        sbPrint.AppendLine("Premium : " & dPremium.ToString() & "<br />")
        sbPrint.AppendLine("IsOriginal : " & bIsOriginal.ToString() & "<br />")
        sbPrint.AppendLine("IsModified : " & bIsModified.ToString() & "<br />")
        sbPrint.AppendLine("FACPremiumType : " & iFACPremiumType.ToString() & "<br />")


        Return sbPrint.ToString()

    End Function
End Class

<Serializable()> Public Class ArrangementsTypeCollection : Inherits CollectionBase



    ''' <summary>
    ''' Add a ArrangementsType object to the collection
    ''' </summary>
    Public Function Add(ByVal v_oArrangementsType As ArrangementsType) As Integer
        Return List.Add(v_oArrangementsType)
    End Function

    ''' <summary>
    ''' Remove an ArrangementsType object from the collection
    ''' </summary>
    Public Sub Remove(ByVal v_oArrangementsType As ArrangementsType)
        List.Remove(v_oArrangementsType)
    End Sub

    ''' <summary>
    ''' Remove an ArrangementsType object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the ArrangementsType object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an ArrangementsType object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the ArrangementsType object</param>
    ''' <value>The replacement ArrangementsType object</value>
    ''' <returns>The ArrangementsType object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As ArrangementsType
        Get
            Return List(i)
        End Get
        Set(ByVal value As ArrangementsType)
            List(i) = value
        End Set
    End Property

End Class

