''' <summary>
''' This class is used for passing config data to a thread.
''' </summary>
Public NotInheritable Class BackgroundJobInfo 

    Implements ICloneable

#Region "Public Fields"

    Public Source As String = ""
    Public MessageBatchSize As Integer = 0
    Public RetryDelay As TimeSpan = TimeSpan.Zero
    Public PollingDelay As TimeSpan = TimeSpan.Zero
    Public Database As Database = Nothing

#End Region

#Region "ICloneable Methods"

    Public Function Clone() As BackgroundJobInfo

        Dim copy As New BackgroundJobInfo

        copy.Source = Me.Source
        copy.MessageBatchSize = Me.MessageBatchSize
        copy.RetryDelay = Me.RetryDelay
        copy.PollingDelay = Me.PollingDelay
        copy.Database = New Database

        Return copy

    End Function

    Private Function ICloneable_Clone() As Object Implements ICloneable.Clone

        Return Clone()

    End Function

#End Region

End Class

