<Serializable()> Public Class TaskGroupTasksDetails

    Private bTimeStampField() As Byte

    Private oTaskGroupTasksField As TaskGroupCollection

    Public Sub New()
        oTaskGroupTasksField = New TaskGroupCollection()
    End Sub

    Public Function Print() As String

        Dim sbPrint As New Text.StringBuilder()
        sbPrint.AppendLine("Time Stamp : " & bTimeStampField.ToString() & "<br />")
        sbPrint.AppendLine(oTaskGroupTasksField.Print() & "<br />")

        Return sbPrint.ToString()

    End Function

    Public Property TimeStamp() As Byte()
        Get
            Return Me.bTimeStampField
        End Get
        Set(ByVal value As Byte())
            Me.bTimeStampField = value
        End Set
    End Property

    Public Property TaskGroupTasks() As TaskGroupCollection
        Get
            Return Me.oTaskGroupTasksField
        End Get
        Set(ByVal value As TaskGroupCollection)
            Me.oTaskGroupTasksField = value
        End Set
    End Property
End Class
