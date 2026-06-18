Imports Sirius.BatchRenewal.SamConfiguration

Namespace Controller
    Friend NotInheritable Class Result 

        Private ReadOnly mStartTime As DateTime

        Private ReadOnly mSamInstance As InstanceElement

        Private ReadOnly mJobDetail As JobDetail

        Public Sub New(samInstance As InstanceElement, jobDetail As JobDetail)
            mSamInstance = samInstance
            mJobDetail = jobDetail
            mStartTime = DateTime.Now
        End Sub

        ''' <summary>
        ''' Gets or sets insurance folder that was proccesed
        ''' </summary>
        Public Property InsuranceFolderCnt As Integer

        ''' <summary>
        ''' Gets or sets the result message
        ''' </summary>
        Public Property Message As String

        ''' <summary>
        ''' Gets or sets the end time for the call
        ''' </summary>
        Public Property EndTime As DateTime?

        ''' <summary>
        ''' Gets the start time
        ''' </summary>
        Public ReadOnly Property StartTime As DateTime
            Get
                Return mStartTime
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
        ''' Gets the Sam Instance that the call was executed on
        ''' </summary>
        Public ReadOnly Property Address() As String
            Get
                Return mSamInstance.Address
            End Get
        End Property


        ''' <summary>
        ''' Gets the Sam Instance that the call was executed on
        ''' </summary>
        Public ReadOnly Property InstanceId() As String
            Get
                Return mSamInstance.InstanceId
            End Get
        End Property

        ''' <summary>
        ''' Gets the job details
        ''' </summary>
        Public ReadOnly Property JobDetail As JobDetail
            Get
                Return mJobDetail
            End Get
        End Property
        
    End Class
End Namespace
