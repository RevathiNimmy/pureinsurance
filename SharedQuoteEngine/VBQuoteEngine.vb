Option Strict Off
Option Explicit On
Imports SharedFiles
Public Class VBQuoteEngine
    Public Function ExecuteVBScript(vbscriptCodeFromStringBuilder As String, methodName As String, ByRef oDataSet As cGISDataSetControl.Application, oExtras As bGISPMUExtras.Business,
                                    m_sTransactionType As String, vArray As Object, v_bIsBackdatedMTA As Boolean) As Integer

        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue

        Dim oScriptControl As Object = Activator.CreateInstance(Type.GetTypeFromProgID("MSScriptControl.ScriptControl"))
        Try
            If oScriptControl Is Nothing Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If
            oScriptControl.Language = "VBScript"
            oScriptControl.Timeout = 30000
            'set the global objects directly rather than using script
            ' RAW 10/07/2003 : remove global parameter forcing scripts to explicitly qualify calls to objects
            oScriptControl.AddObject("Engine", oDataSet.Risk, False)
            oScriptControl.AddObject("DataSet", oDataSet, False)
            oScriptControl.AddObject("Extras", oExtras, False)
            oScriptControl.AddCode(vbscriptCodeFromStringBuilder.ToString)
            oScriptControl.Run("SetDefaultValue", m_sTransactionType, vArray, v_bIsBackdatedMTA)
            oScriptControl.Run(methodName)

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError
            oScriptControl = Nothing
        End Try
        oScriptControl = Nothing
        Return result
    End Function

    Public Function RunMediaTypeValidation(vbscriptCodeFromStringBuilder As String, methodName As String, oSharedStorage As Object) As Integer

        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue

        Dim oScriptControl As Object = Activator.CreateInstance(Type.GetTypeFromProgID("MSScriptControl.ScriptControl"))
        Try
            If oScriptControl Is Nothing Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If
            oScriptControl.Language = "VBScript"
            oScriptControl.Timeout = 30000
            'set the global objects directly rather than using script
            ' RAW 10/07/2003 : remove global parameter forcing scripts to explicitly qualify calls to objects
            oScriptControl.AddObject("oSharedStorage", oSharedStorage, False)
            oScriptControl.AddCode(vbscriptCodeFromStringBuilder.ToString)

            oScriptControl.Run(methodName)

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError
            oScriptControl = Nothing
        End Try
        oScriptControl = Nothing
        Return result
    End Function

    Public Function ExecuteRenewalRuleScript(vbscriptCodeFromStringBuilder As String, methodName As String, oSharedStorage As Object, oExtras As bGISPMUExtras.Business) As Integer

        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue

        Dim oScriptControl As Object = Activator.CreateInstance(Type.GetTypeFromProgID("MSScriptControl.ScriptControl"))
        Try
            If oScriptControl Is Nothing Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If
            oScriptControl.Language = "VBScript"
            oScriptControl.Timeout = 30000
            'set the global objects directly rather than using script
            ' RAW 10/07/2003 : remove global parameter forcing scripts to explicitly qualify calls to objects
            oScriptControl.AddObject("oSharedStorage", oSharedStorage, False)
            oScriptControl.AddObject("Extras", oExtras, False)
            oScriptControl.AddCode(vbscriptCodeFromStringBuilder.ToString)

            oScriptControl.Run(methodName)

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError
            oScriptControl = Nothing
        End Try
        oScriptControl = Nothing
        Return result
    End Function

     Public Function ExecuteAuthorityRule(vbscriptCodeFromStringBuilder As String, methodName As String, authorityleveldata As Object, oExtras As bGISPMUExtras.Business) As Integer
        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim oScriptControl As Object = Activator.CreateInstance(Type.GetTypeFromProgID("MSScriptControl.ScriptControl"))
        Try
            If oScriptControl Is Nothing Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If
            oScriptControl.Language = "VBScript"
            oScriptControl.Timeout = 30000
            'set the global objects directly rather than using script
            ' RAW 10/07/2003 : remove global parameter forcing scripts to explicitly qualify calls to objects
            oScriptControl.AddObject("authorityleveldata", authorityleveldata, False)
            oScriptControl.AddObject("Extras", oExtras, False)
            oScriptControl.AddCode(vbscriptCodeFromStringBuilder.ToString)
            oScriptControl.Run(methodName)
        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError
            oScriptControl = Nothing
        End Try
        oScriptControl = Nothing
        Return result
    End Function


End Class