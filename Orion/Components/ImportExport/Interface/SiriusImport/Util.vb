Public NotInheritable Class Util 

    Public Shared Function ToSafeInt(ByVal value As Object, ByVal defaultValue As Integer) As Integer

        Dim retVal As Integer = 0
        Try
            retVal = CInt(value)
        Catch ex As Exception
            retVal = defaultValue
        End Try

        Return retVal

    End Function

    Public Shared Function ToSafeByte(ByVal value As Object, ByVal defaultValue As Byte) As Byte

        Dim retVal As Byte = 0
        Try
            retVal = CByte(value)
        Catch ex As Exception
            retVal = defaultValue
        End Try

        Return retVal

    End Function

    Public Shared Function ToSafeDecimal(ByVal value As Object, ByVal defaultValue As Decimal) As Decimal

        Dim retVal As Decimal = 0
        Try
            retVal = CDec(value)
        Catch ex As Exception
            retVal = defaultValue
        End Try

        Return retVal

    End Function

    Public Shared Function ToSafeBoolean(ByVal value As Object, ByVal defaultValue As Boolean) As Boolean

        Dim retVal As Boolean = False

        Try
            retVal = CBool(value)
        Catch ex As Exception
            retVal = defaultValue
        End Try

        Return retVal

    End Function


    Public Shared Function ToSafeDate(ByVal value As Object, ByVal defaultValue As Date) As Date

        Dim retVal As Date

        Try
            retVal = CDate(value)
        Catch ex As Exception
            retVal = defaultValue
        End Try

        Return retVal

    End Function


End Class
