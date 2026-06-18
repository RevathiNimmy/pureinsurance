<Serializable()> Public Class InstalmentPlanDetails

    Private financePlanKeyField As Integer

    Private financePlanVersionField As Integer

    Private instalmentDetailsField As Instalment

    Private sKey As String

    Public Property Key() As String
        Get
            Return sKey
        End Get
        Set(ByVal value As String)
            sKey = value
        End Set
    End Property
    '''<remarks/>
    Public Property FinancePlanKey() As Integer
        Get
            Return Me.financePlanKeyField
        End Get
        Set(ByVal value As Integer)
            Me.financePlanKeyField = value
        End Set
    End Property

    '''<remarks/>
    Public Property FinancePlanVersion() As Integer
        Get
            Return Me.financePlanVersionField
        End Get
        Set(ByVal value As Integer)
            Me.financePlanVersionField = value
        End Set
    End Property

    '''<remarks/>
    Public Property InstalmentDetails() As Instalment
        Get
            Return Me.instalmentDetailsField
        End Get
        Set(ByVal value As Instalment)
            Me.instalmentDetailsField = value
        End Set
    End Property
End Class



<Serializable()> Public Class InstalmentPlanDetailsCollection : Inherits CollectionBase
    
    ''' <summary>
    ''' Debug interface to the object
    ''' </summary>
    ''' <returns>An HTML string containining data held within the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oAddress As Address In List
            sbPrint.AppendLine(oAddress.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add an Instalment object to the collection
    ''' </summary>
    ''' <param name="v_oInstalmentPlanDetails">The Associate object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oInstalmentPlanDetails As InstalmentPlanDetails) As Integer
        v_oInstalmentPlanDetails.Key = List.Add(v_oInstalmentPlanDetails)
        Return v_oInstalmentPlanDetails.Key
    End Function

    ''' <summary>
    ''' Remove an Instalment object from the collection
    ''' </summary>
    ''' <param name="v_oInstalmentPlanDetails">The Associate object to be removed</param>
    Public Sub Remove(ByVal v_oInstalmentPlanDetails As InstalmentPlanDetails)
        List.Remove(v_oInstalmentPlanDetails)
    End Sub

    ''' <summary>
    ''' Remove an Instalment object from the collection with a specified index
    ''' </summary>
    ''' <param name="index">The index of the Associate object to be removed</param>
    Public Sub Remove(ByVal index As Integer)
        List.RemoveAt(index)
    End Sub

    ''' <summary>
    ''' Retrieve or replace an Instalment object with a specified index
    ''' </summary>
    ''' <param name="i">The index of the Instalment object</param>
    ''' <value>The replacement Instalment object</value>
    ''' <returns>The Instalment object with the specified index</returns>
    Default Public Property Item(ByVal i As Integer) As InstalmentPlanDetails
        Get
            Return List(i)
        End Get
        Set(ByVal value As InstalmentPlanDetails)
            List(i) = value
        End Set
    End Property

    Public Sub Update(ByVal v_oInstalmentPlanDetails As InstalmentPlanDetails)
        List.Item(v_oInstalmentPlanDetails.Key) = v_oInstalmentPlanDetails
    End Sub

    Public Sub Update(ByVal v_oInstalmentPlanDetails As InstalmentPlanDetails, ByVal index As Integer)
        List.Item(index) = v_oInstalmentPlanDetails
    End Sub

End Class

