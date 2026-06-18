Namespace Controller
    ''' <summary>
    ''' Status update event argument
    ''' </summary>
    Public NotInheritable Class StatusUpdateEventArgs 
        Inherits EventArgs

        ''' <summary>
        ''' Creates a new instance of the StatusUpdateEventArgs class
        ''' </summary>
        ''' <param name="status">Message</param>
        ''' <param name="level">Status Level</param>
        Sub New(status As String, level As StatusLevel)
            Me.Status = status
            Me.Level = level
        End Sub

        ''' <summary>
        ''' Gets or sets the status message
        ''' </summary>
        Public Property Status As String

        ''' <summary>
        ''' Gets or sets the level of this message
        ''' </summary>
        Public Property Level As StatusLevel

    End Class
End Namespace