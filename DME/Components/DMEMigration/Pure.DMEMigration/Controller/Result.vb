Imports Pure.DMEMigration.Configuration

Namespace Controller
    ''' <summary>
    ''' add results for each doc processed
    ''' </summary>
    Friend NotInheritable Class Result

        Private ReadOnly m_oInstance As InstanceElement

        Private ReadOnly m_oJobDetail As JobDetail

        Private ReadOnly m_dStartTime As DateTime

        Public Sub New(Instance As InstanceElement, jobDetail As JobDetail)
            m_oInstance = Instance
            m_oJobDetail = jobDetail
            m_dStartTime = DateTime.Now
        End Sub

        ' ''' <summary>
        ' ''' Gets or sets doc number that was proccesed
        ' ''' </summary>
        'Public Property DocNum As Integer

        ''' <summary>
        ''' Gets or sets processed status
        ''' </summary>
        Public Property isFailed As Boolean

        ''' <summary>
        ''' Gets or sets the result message
        ''' </summary>
        Public Property Message As String

        ''' <summary>
        ''' Gets or sets the end time for the call
        ''' </summary>
        Public Property EndTime As DateTime?

        ''' <summary>
        ''' Gets the duration of the call
        ''' </summary>
        Public ReadOnly Property StartTime As DateTime
            Get
                Return m_dStartTime
            End Get
        End Property

        ''' <summary>
        ''' Gets the duration of the call
        ''' </summary>
        Public ReadOnly Property Duration As TimeSpan
            Get
                Return EndTime.Value - StartTime
            End Get
        End Property

        ''' <summary>
        ''' Gets the Batch Instance that the call was executed on. A possible future expansion for multiple instance in queue
        ''' </summary>
        Public ReadOnly Property InstanceId() As String
            Get
                Return m_oInstance.InstanceId
            End Get
        End Property

        ''' <summary>
        ''' Gets the job details
        ''' </summary>
        Public ReadOnly Property JobDetail As JobDetail
            Get
                Return m_oJobDetail
            End Get
        End Property

    End Class
End Namespace