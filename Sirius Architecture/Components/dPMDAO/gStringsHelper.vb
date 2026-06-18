Imports System.Linq.Expressions

Friend Class StringsHelper
    Friend Shared Function ToDoubleSafe(sInProc As String, Optional ByRef Default_Renamed As Double = 0) As Integer
        Dim result As Double
        Try
            If sInProc Is Nothing OrElse (Not (Convert.IsDBNull(sInProc))) Then
                If Double.TryParse(sInProc, result) Then
                    Return result
                Else
                    Return Default_Renamed
                End If
            Else
                Return Default_Renamed
            End If
        Catch
            Return Default_Renamed
        End Try

    End Function
    Friend Shared Function Format(sInProc As String) As String
        Throw New NotImplementedException()
    End Function
End Class
