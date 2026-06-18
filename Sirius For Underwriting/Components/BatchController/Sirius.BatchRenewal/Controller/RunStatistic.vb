Namespace Controller
    Public NotInheritable Class RunStatistic 

        ''' <summary>
        ''' Gets or sets the end point address
        ''' </summary>
        Public Property Address As String

        ''' <summary>
        ''' Gets or sets the number of transaction processed
        ''' </summary>
        Public Property Processed As Integer

        ''' <summary>
        ''' Gets or sets the number of transactions in progress
        ''' </summary>
        Public Property InProgress As Integer

        ''' <summary>
        ''' Gets or sets the Min call duration
        ''' </summary>
        Public Property MinDuration As Double

        ''' <summary>
        ''' Gets or sets the max duration
        ''' </summary>
        Public Property MaxDuration As Double

        ''' <summary>
        ''' Gets or sets the average duration
        ''' </summary>
        Public Property AverageDuration As Double

    End Class
End Namespace