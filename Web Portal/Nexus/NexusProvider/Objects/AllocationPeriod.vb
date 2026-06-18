''' <summary>
''' Nexus object
''' </summary>
''' <remarks></remarks>
<Serializable()> Public Class AllocationPeriod

    Private sPeriodID As Integer

    Private sPeriodName As String

    Private sYearName As String


    Public Overridable Function Print() As String

        Dim sbPrint As New Text.StringBuilder

        sbPrint.AppendLine("PeriodID : " & sPeriodID & "<br />")
        sbPrint.AppendLine("PeriodName : " & sPeriodName & "<br />")
        sbPrint.AppendLine("YearName : " & sYearName & "<br />")
        sbPrint.AppendLine("<br />")

        Return sbPrint.ToString

    End Function


    '''<remarks/>
    Public Property PeriodID() As Integer
        Get
            Return sPeriodID
        End Get
        Set(ByVal value As Integer)
            sPeriodID = value
        End Set

    End Property

    '''<remarks/>
    Public Property PeriodName() As String
        Get
            Return sPeriodName
        End Get
        Set(ByVal value As String)
            sPeriodName = value
        End Set
    End Property

    '''<remarks/>
    Public Property YearName() As String
        Get
            Return sYearName
        End Get
        Set(ByVal value As String)
            sYearName = value
        End Set
    End Property


End Class


''' <summary>
''' Collection of Allocation Period objects
''' </summary>
<Serializable()> Public Class PeriodCollection : Inherits CollectionBase

    ''' <summary>
    ''' Debug interface
    ''' </summary>
    ''' <returns>A HTML string containing the contents of the object</returns>
    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()

        For Each oAllocationPeriod As AllocationPeriod In List
            sbPrint.AppendLine(oAllocationPeriod.Print())
            sbPrint.AppendLine("<br />")
        Next

        Return sbPrint.ToString()

    End Function

    ''' <summary>
    ''' Add a Allocation Period object to the collection
    ''' </summary>
    ''' <param name="v_oAllocationPeriod">The Allocation Period object to be added</param>
    ''' <returns></returns>
    Public Function Add(ByVal v_oAllocationPeriod As AllocationPeriod) As Integer
        Return List.Add(v_oAllocationPeriod)
    End Function

    ''' <summary>
    ''' Remove an Allocation Period object from the collection
    ''' </summary>
    ''' <param name="v_oAllocationPeriod">The Allocation Period object to be removed</param>
    Public Sub Remove(ByVal v_oAllocationPeriod As AllocationPeriod)
        List.Remove(v_oAllocationPeriod)
    End Sub


    Default Public Property Item(ByVal i As Integer) As AllocationPeriod
        Get
            Return List(i)
        End Get
        Set(ByVal value As AllocationPeriod)
            List(i) = value
        End Set
    End Property

End Class