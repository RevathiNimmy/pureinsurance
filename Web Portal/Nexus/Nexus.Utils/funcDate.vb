
Public Module funcDate

    Function IsValidDate(ByVal pDay As Int16, ByVal pMonth As String, ByVal pYear As Integer) As Boolean

        'Checks that the day passed is valid for the month/year e.g 30 Feb is invalid (false)
        '? Who wrote this ?

        If pDay > 29 And pMonth = "February" Then
            Return False
        ElseIf pDay = 29 And pMonth = "February" And Not Date.IsLeapYear(pYear) Then
            Return False
        ElseIf pDay = 31 And (pMonth = "April" Or pMonth = "June" Or pMonth = "September" Or pMonth = "November") Then
            Return False
        Else
            Return True
        End If

    End Function

    Function GetDateWithSuffix(ByVal pDay) As String

        'Return the suffix of a date e.g '21st' for 21
        'DH - 01/09/05

        Select Case pDay
            Case 1, 21, 31
                Return pDay & "st"
            Case 2, 22
                Return pDay & "nd"
            Case 3, 23
                Return pDay & "rd"
            Case Else
                Return pDay & "th"
        End Select

    End Function

    Function ConvertMonthToNumeric(ByVal name As String) As Integer

        'returns the integer value of a month passed as a string e.g 'February' = 2
        'DH - 01/09/05

        If IsDate("01 " & name) Then
            Return DateTime.Parse("01 " & name).Month
        Else
            Return 0
        End If

    End Function

    Function ConvertDaytoDayOfWeek(ByVal pDayOfWeek As String) As DayOfWeek

        'return the DayOfWeek enumerator for a string representation of a day
        'DH - 01/09/05

        Dim dwTmp As New DayOfWeek

        Select Case LCase(pDayOfWeek)
            Case "monday" Or "mon"
                dwTmp = DayOfWeek.Monday
            Case "tuesday" Or "tue"
                dwTmp = DayOfWeek.Tuesday
            Case "wednesday" Or "wed"
                dwTmp = DayOfWeek.Wednesday
            Case "thursday" Or "thur"
                dwTmp = DayOfWeek.Thursday
            Case "friday" Or "fri"
                dwTmp = DayOfWeek.Friday
            Case "saturday" Or "sat"
                dwTmp = DayOfWeek.Saturday
            Case "sunday" Or "sun"
                dwTmp = DayOfWeek.Sunday
            Case Else
                dwTmp = Nothing
        End Select

        Return dwTmp

    End Function

    Function GetWeekNumber(ByVal vDate As Date) As Integer

        'Return the week of the year of the date passed as an integer
        '? Who wrote this ?

        Return Math.Ceiling((vDate.Date.Subtract(New Date(Date.Today.Year, 1, 1)).Days + 1) / 7)

    End Function

End Module

