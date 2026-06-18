Imports System.Globalization.CultureInfo

Public Module General

    Public Function FormatDateWithNumberOfDaysAgo(ByVal v_Date As DateTime) As String

        Dim lTimeDiff As Long = DateDiff(DateInterval.Day, v_Date, Now)
        Return IIf(lTimeDiff > 0, IIf(lTimeDiff > 1, lTimeDiff & " Days ago", _
            lTimeDiff & " Day ago"), "Today") & " ( " & Format(v_Date, _
            "dd-MMM-yyyy HH:mm:ss") & " )"

    End Function

End Module
