Imports System.Globalization
Public NotInheritable Class Informations
    Public Shared ReadOnly Property Erl As String
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public Shared Function IsNothing(ByVal Expression As Object) As Boolean

        Try
            If (Expression Is Nothing) Then
                Return True
            Else
                Return False
            End If
        Catch
            Return False
        End Try


    End Function
    Public Shared Function IsReference(Expression As Object) As Boolean
        If TypeOf (Expression) Is ValueType Then
            Return False
        End If
        Return True
    End Function

    Public Shared Function IsDBNull(ByVal Expression As Object) As Boolean

        Try
            If (Expression Is Nothing OrElse Expression Is System.DBNull.Value) Then
                Return True
            Else
                Return False
            End If
        Catch
            Return False
        End Try


    End Function
    Public Shared Function IsDate(ByVal Expression As Object) As Boolean

        If Expression Is Nothing Then
            Return False
        End If

        If TypeOf (Expression) Is DateTime Then
            Return True
        End If

        Dim text As String = Convert.ToString(Expression)
        Dim Result As DateTime = DateTime.MinValue
        If (text IsNot Nothing AndAlso text.Length > 0) Then
            Return DateTime.TryParse(text, Result)
        End If

        Return False

    End Function
    Public Shared Function IsArray(ByVal Expression As Object()) As Boolean

        Try

            If (Expression Is Nothing OrElse Expression.Length = 0 OrElse Not Expression.GetType().IsArray) Then
                Return False
            Else
                Return True
            End If
        Catch
            Return False
        End Try

    End Function
    Public Shared Function IsArray(ByVal Expression As Array) As Boolean

        Try

            If (Expression Is Nothing OrElse Expression.Length = 0 OrElse Not Expression.GetType().IsArray) Then
                Return False
            Else
                Return True
            End If
        Catch
            Return False
        End Try

    End Function

    Public Shared Function IsArray(ByVal Expression As Object(,)) As Boolean

        Try

            If (Expression Is Nothing OrElse Expression.Length = 0 OrElse Not Expression.GetType().IsArray) Then
                Return False
            Else
                Return True
            End If
        Catch
            Return False
        End Try

    End Function

    Public Shared Function IsArray(ByVal Expression As Object) As Boolean

        Try

            If (Expression Is Nothing OrElse Expression.Length = 0 OrElse Not Expression.GetType().IsArray) Then
                Return False
            Else
                Return True
            End If
        Catch
            Return False
        End Try

    End Function
    Public Shared Function VarType(varName As Object) As VariantType
        Return [Enum].Parse(GetType(VariantType), varName)
    End Function

    Public Shared Function IsNumeric(input As String) As Boolean
        ' Try to parse the string to double
        Dim result As Double
        Return Double.TryParse(input, result)
    End Function


    Public Shared Function Err() As ErrObject
        Return New ErrObject()
    End Function

    Public Shared Function inStr(startIndex As Integer, inputString As String, findstring As String) As Integer
        Dim position As Integer = inputString.IndexOf(findstring, startIndex - 1) + 1
        Return position
    End Function
    Public Shared Function inStr(inputString As String, findstring As String) As Integer
        Dim position As Integer = inputString.IndexOf(findstring) + 1
        Return position
    End Function

    Public Shared Function Left(str As String, len As Integer) As String
        Dim leftstring As String = ""
        If Not String.IsNullOrEmpty(str) Then
            leftstring = str.Substring(0, len)
        End If
        Return leftstring
    End Function

    Public Shared Function Mid(inputString As String, startIndex As Integer, length As Integer) As String
        ' Adjust startIndex to 0-based index
        startIndex -= 1

        ' Ensure startIndex is within the bounds of the string
        If startIndex < 0 Then startIndex = 0
        If startIndex > inputString.Length - 1 Then startIndex = inputString.Length - 1

        ' Ensure length is within the bounds of the string
        If length < 0 Then length = 0
        If startIndex + length > inputString.Length Then length = inputString.Length - startIndex

        ' Extract the substring using String.Substring
        Return inputString.Substring(startIndex, length)
    End Function
    Public Shared Function DateDiff(Datetype As String, start As DateTime, [end] As DateTime, firstDayOfWeek As DayOfWeek, firstWeekOfYear As FirstWeekOfYear) As Integer

        Select Case Datetype.Trim().ToUpper()
            Case "D"
                ' Adjust the start date based on the first day of the week
                'Dim adjustedStartDate As DateTime = AdjustStartDate(start, firstDayOfWeek)
                ' Calculate the difference in days
                Dim daysDifference As Integer = CInt(([end] - start).TotalDays)
                daysDifference += AdjustForFirstWeekOfYear(start, firstDayOfWeek, firstWeekOfYear)
                Return daysDifference
            Case "WW"
                Return DateDiffWeeks(CDate(start), CDate([end]), firstDayOfWeek, firstWeekOfYear)
            Case "M"
                Return DateDiffMonths(CDate(start), CDate([end]), firstDayOfWeek, firstWeekOfYear)
            Case "Y"
                Return DateDiffYears(CDate(start), CDate([end]), firstDayOfWeek, firstWeekOfYear)

        End Select
        ' Adjust for the first week of the year
        Return 0

    End Function
    Public Shared Function WCFDateDiff(Datetype As Integer, start As DateTime, [end] As DateTime, firstDayOfWeek As FirstDayOfWeek, firstWeekOfYear As FirstWeekOfYear) As Integer

        Select Case Datetype
            Case 4
                ' Adjust the start date based on the first day of the week
                Dim adjustedStartDate As DateTime = AdjustStartDate(start, firstDayOfWeek)
                ' Calculate the difference in days
                Dim daysDifference As Integer = CInt(([end] - adjustedStartDate).TotalDays)
                daysDifference += AdjustForFirstWeekOfYear(start, firstDayOfWeek, firstWeekOfYear)
                Return daysDifference
            Case 6
                Return DateDiffWeeks(CDate(start), CDate([end]), firstDayOfWeek, firstWeekOfYear)
            Case 2
                Return DateDiffMonths(CDate(start), CDate([end]), firstDayOfWeek, firstWeekOfYear)
            Case 0
                Return DateDiffYears(CDate(start), CDate([end]), firstDayOfWeek, firstWeekOfYear)

        End Select
        ' Adjust for the first week of the year
        Return 0

    End Function
    Public Shared Function DateDiffYears(start As DateTime, [end] As DateTime, firstDayOfWeek As DayOfWeek, firstWeekOfYear As FirstWeekOfYear) As Integer
        ' Calculate the difference in years
        Dim yearsDifference As Integer = [end].Year - start.Year

        ' Adjust for the first day of the week
        'Dim adjustedStartDate As DateTime = AdjustStartDate(start, firstDayOfWeek)

        ' Check if the start date is later in the year of the end date
        'If adjustedStartDate.DayOfYear > [end].DayOfYear Then
        '    yearsDifference -= 1
        'End If

        ' Adjust for the first week of the year
        yearsDifference += AdjustForFirstWeekOfYear(start, firstDayOfWeek, firstWeekOfYear)

        Return yearsDifference
    End Function
    Public Shared Function AdjustStartDate(startDate As DateTime, firstDayOfWeek As DayOfWeek) As DateTime
        ' Find the day of the week of the start date
        Dim startDayOfWeek As DayOfWeek = startDate.DayOfWeek

        ' Calculate the difference in days to adjust the start date
        Dim daysToAdjust As Integer = (startDayOfWeek - firstDayOfWeek + 7) Mod 7

        ' Adjust the start date
        Return startDate.AddDays(-daysToAdjust)
    End Function

    Public Shared Function AdjustForFirstWeekOfYear(startDate As DateTime, firstDayOfWeek As DayOfWeek, firstWeekOfYear As FirstWeekOfYear) As Integer
        ' Calculate the difference in days to adjust for the first week of the year
        Dim daysToAdjust As Integer = 0

        If firstWeekOfYear = FirstWeekOfYear.FirstFourDays Then
            ' Find the day of the week of January 1st of the start year
            Dim january1DayOfWeek As DayOfWeek = New DateTime(startDate.Year, 1, 1).DayOfWeek

            ' Calculate the difference in days to adjust for the first four days of the week
            daysToAdjust = (CInt(january1DayOfWeek) - CInt(firstDayOfWeek) + 7) Mod 7
        End If

        ' Adjust the difference in days
        Return daysToAdjust
    End Function

    Public Shared Function DateDiffWeeks(start As DateTime, [end] As DateTime, firstDayOfWeek As DayOfWeek, firstWeekOfYear As FirstWeekOfYear) As Integer
        ' Adjust the start date based on the first day of the week
        Dim adjustedStartDate As DateTime = AdjustStartDate(start, firstDayOfWeek)
        Dim adjustedEndDate As DateTime = AdjustStartDate([end], firstDayOfWeek)

        ' Calculate the difference in weeks
        Dim weeksDifference As Integer = CInt(adjustedEndDate.Subtract(adjustedStartDate).TotalDays / 7)

        ' Adjust for the first week of the year
        weeksDifference += AdjustForFirstWeekOfYear(start, firstDayOfWeek, firstWeekOfYear)

        Return weeksDifference
    End Function


    Public Shared Function DateDiffMonths(start As DateTime, [end] As DateTime, firstDayOfWeek As DayOfWeek, firstWeekOfYear As FirstWeekOfYear) As Integer
        ' Calculate the difference in months
        Dim monthsDifference As Integer = (([end].Year - start.Year) * 12) + [end].Month - start.Month

        ' Adjust for the first day of the week
        'Dim adjustedStartDate As DateTime = AdjustStartDate(start, firstDayOfWeek)

        ' Check if the start date is later in the month of the end date
        'If adjustedStartDate.Day > [end].Day Then
        '    monthsDifference -= 1
        'End If

        ' Adjust for the first week of the year
        monthsDifference += AdjustForFirstWeekOfYear(start, firstDayOfWeek, firstWeekOfYear)

        Return monthsDifference
    End Function
    Public Shared Function DateAdd(interval As String, amount As Integer, dt As DateTime) As DateTime
        Select Case interval.ToLower()
         Case "d", "w"
                ' Add days
                Return dt.AddDays(amount)
            Case "m"
                ' Add months
                Return dt.AddMonths(amount)
            Case "y"
                ' Add years
                Return dt.AddYears(amount)
         Case "ww"
                Return dt.AddDays(7 * amount)                     
            Case Else
                ' Handle other intervals if needed
                Throw New ArgumentException("Invalid interval")
        End Select
    End Function
    Public Shared Function DateAdd(interval As Integer, amount As Integer, dt As DateTime) As DateTime
        Select Case interval
            Case 4
                ' Add days
                Return dt.AddDays(amount)
            Case 2
                ' Add months
                Return dt.AddMonths(amount)
            Case 0
                ' Add years
                Return dt.AddYears(amount)
            Case Else
                ' Handle other intervals if needed
                Throw New ArgumentException("Invalid interval")
        End Select
    End Function
    Public Shared Function DateDiff(interval As String, dtStartDate As DateTime, dtEndDate As DateTime) As Integer
        Select Case interval.ToLower()
            Case "d"
                ' Calculate the difference in days
                Return CInt(dtEndDate.Subtract(dtStartDate).TotalDays)
            Case "m"
                ' Calculate the difference in months
                Return Math.Abs((dtEndDate.Year - dtStartDate.Year) * 12 + dtEndDate.Month - dtStartDate.Month)
            Case "y"
                ' Calculate the difference in years
                Return Math.Abs(dtEndDate.Year - dtStartDate.Year)
            Case Else
                ' Handle other intervals if needed
                Throw New ArgumentException("Invalid interval")
        End Select
    End Function
    Public Shared Function DateDiff(interval As Integer, dtStartDate As DateTime, dtEndDate As DateTime) As Integer
        Select Case interval
            Case 4
                ' Calculate the difference in days
                Return CInt(dtEndDate.Subtract(dtStartDate).TotalDays)
            Case 2
                ' Calculate the difference in months
                Return Math.Abs((dtEndDate.Year - dtStartDate.Year) * 12 + dtEndDate.Month - dtStartDate.Month)
            Case 0
                ' Calculate the difference in years
                Return Math.Abs(dtEndDate.Year - dtStartDate.Year)
            Case Else
                ' Handle other intervals if needed
                Throw New ArgumentException("Invalid interval")
        End Select
    End Function


    Public Shared Function DateSerial(vYear As Integer, vMonth As Integer, vDays As Integer) As DateTime

        Dim dSerialDate As DateTime = New DateTime(vYear, vMonth, vDays)
        Return dSerialDate
    End Function

    Public Shared Function BlankToNull(ByVal Expression As Object) As Object

        Try



            If (CStr(Expression)).Length = 0 Then

                Return DBNull.Value
            Else
                Return Expression
            End If

        Catch
        End Try

        Return DBNull.Value

    End Function

    Public Shared Function FormatDateTime(dateTime As DateTime) As String
        Dim text As String = "M/d/yyyy h:mm:ss tt"
        Dim MinValue As DateTime = New DateTime(1899, 12, 30)

        If dateTime.Date = MinValue Then
            text = "h:mm:ss tt"
        ElseIf dateTime.TimeOfDay.Ticks = 0 Then
            text = "M/d/yyyy"
        End If

        Return dateTime.ToString(text)
    End Function
    Public Shared Function ProperCase(ByVal value As String) As String
        If String.IsNullOrEmpty(value) Then Return value

        Dim textInfo As TextInfo = CultureInfo.CurrentCulture.TextInfo
        Return textInfo.ToTitleCase(value.ToLower())
    End Function
    Public Shared Function DatePart(ByVal type As String, ByVal dt As DateTime, ByVal firstDayOfWeek As DayOfWeek, ByVal firstWeekOfYear As FirstWeekOfYear) As Integer
     ' Find the day of the month
     Dim iRetVal As Integer = 0

        If type.ToUpper = "D" Then
         iRetVal = dt.Day
     End If

     Return iRetVal
    End Function

    Public Shared Function Weekday(ByVal dt As DateTime, ByVal firstDayOfWeek As DayOfWeek) As Integer
        ' Calculate the offset to the specified first day of the week
        Dim offset As Integer = (dt.DayOfWeek - firstDayOfWeek + 7) Mod 7

        ' Add 1 to make Sunday the 1st day of the week, Monday the 2nd, and so on
        Return offset + 1
    End Function

End Class
Public NotInheritable Class ErrObject
    Public Property Number()
    Public Property Description()
    Public Property Clear()

    Public Property Source()
    Public ReadOnly Property LastDllError As Integer
        Get
            Throw New NotImplementedException()
        End Get
    End Property

    Public Property HelpFile As Integer
    Public Property HelpContext As Integer
End Class
Public Class DateTimeHelper
    Public Shared ReadOnly Property [Time] As DateTime
        Get
            Return DateTime.Now.AddDays(0.0 - DateTime.Now.Date.ToOADate())
        End Get
    End Property
End Class
