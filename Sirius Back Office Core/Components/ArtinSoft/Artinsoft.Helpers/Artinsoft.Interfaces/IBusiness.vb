Public Interface IBusiness
    Function Initialise(ByVal sUsername As String, _
        ByVal sPassword As String, _
        ByVal iUserID As Integer, _
        ByVal iSourceID As Integer, _
        ByVal iLanguageID As Integer, _
        ByVal iCurrencyID As Integer, _
        ByVal iLogLevel As Integer, _
        ByVal sCallingAppName As String, _
        Optional ByVal bStandAlone _
        As Boolean = False, _
        Optional ByVal vDatabase _
        As Object = Nothing) As Long
End Interface
