Option Strict Off
Option Explicit On
Imports System.Globalization
Imports SSP.Shared

Module VersionHandler
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Module Name: VersionHandler
    '
    ' Date:  25/03/1999
    '
    ' Description: VersionHandler.
    '
    ' Edit History:
    ' ***************************************************************** '


    Const ACClass As String = "VersionHandler"

    '***********************************************
    ' TO DO: GLOBAL VARIABLE IN BAS MODULE
    'Public lID As Long
    '***********************************************


    ' ***************************************************************** '
    ' Name: CalcBuildNumber
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function CalcBuildNumber(ByVal v_sVersion As String, ByRef r_sBuildNumber As String) As Integer

        Dim result As Integer = 0
        Dim sMajor, sServiceRelease, sBuild, sPatch As String
        Dim vSplit As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sMajor = "0"
            sServiceRelease = "0"
            sBuild = "0"
            sPatch = "0"
            r_sBuildNumber = "0"

            ' RDC 15052001 use VB6 Split function to get parameters
            If (v_sVersion.IndexOf("."c) + 1) = 0 Then
                ' can't split version string if no dots
                Return result
            End If


            'vSplit = Strings.Split(v_sVersion, ".", -1, CompareMethod.Text)
            vSplit = v_sVersion.Split("."c)


            sMajor = CStr(vSplit(0))

            sServiceRelease = CStr(vSplit(1))

            ' set revision if version string has 3rd parm

            If vSplit.GetUpperBound(0) >= 2 Then

                sBuild = CStr(vSplit(2))
            End If

            If vSplit.GetUpperBound(0) >= 3 Then

                sPatch = CStr(vSplit(3))
            End If

            ' Calculate the Build Number
            Dim dbNumericTemp4 As Double
            Dim dbNumericTemp3 As Double
            Dim dbNumericTemp2 As Double
            Dim dbNumericTemp As Double
            If (Double.TryParse(sMajor, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) And (Double.TryParse(sServiceRelease, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2)) And (Double.TryParse(sBuild, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3)) And (Double.TryParse(sPatch, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4)) Then
                r_sBuildNumber = StringsHelper.Format(sMajor, "00000") & StringsHelper.Format(sServiceRelease, "00000") & StringsHelper.Format(sBuild, "00000") & StringsHelper.Format(sPatch, "00000")
            Else
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CalcBuildNumberFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CalcBuildNumber", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Module
