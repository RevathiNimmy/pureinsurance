Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Drawing
Imports System.Globalization
Imports System.Text
Imports System.IO

<System.Runtime.InteropServices.ProgId("GeneralFunc_NET.GeneralFunc")> _
 Public Module GeneralFunc
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    '
    ' Application general functions module. Contains all of the global
    ' functions that might be useful when writing an application.
    '
    ' ***************************************************************** '


    ' Constant for the methods to identify
    ' which class this is.
    Private Const ACClass As String = "GeneralFunc"

    Private Declare Function GetComputerName Lib "kernel32" Alias "GetComputerNameA" (ByVal lpBuffer As String, ByRef nSize As Integer) As Integer

    ' ***************************************************************** '
    ' Name: PMRoundUp
    '
    ' Description: Takes a Double value :
    '              1. Rounds up by adding a rounding factor derived from
    '                 the number of decimal places requested.
    '              2. Truncates to the number of decimal places specified.
    '
    ' ***************************************************************** '
    Public Function PMRoundUp(ByRef dValueIn As Double, ByRef iNumOfPlaces As Integer) As Double

        Dim dRoundFactor As Double
        Const dPointFourNine As Double = 0.49

        Try

            ' Is Number of Decimal Places Requested Invalid
            If iNumOfPlaces < 0 Then
                iNumOfPlaces = 0
            End If

            ' Is Number of Decimal Places Requested Invalid
            If iNumOfPlaces > 8 Then
                iNumOfPlaces = 8
            End If

            ' Derive the Rounding Factor
            dRoundFactor = dPointFourNine / (10 ^ iNumOfPlaces)

            ' Add the rounding factor
            dValueIn += dRoundFactor

            ' Truncate to the number of decimal places required

            Return PMTruncate(dValueIn:=dValueIn, iNumOfPlaces:=iNumOfPlaces)

        Catch



            ' Error Section.

            ' Return the original value.

            Return dValueIn
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: PMStartOfWeek
    '
    ' Description: Returns the start of the week date from the given
    '              date.
    '
    ' ***************************************************************** '
    Public Function PMStartOfWeek(ByRef dtDate As Date) As Date

        Dim iInterval As Integer
        Dim dtSOWDate As Date

        Try

            ' Check what the current day is.
            If StringsHelper.Format(dtDate, "w") = FirstDayOfWeek.Sunday Then
                ' Set the interval value.
                iInterval = -6
            Else
                ' Set the interval value.
                iInterval = (CDbl(StringsHelper.Format(dtDate, "w")) - 2) * -1
            End If

            ' Calculate the start of the week.
            dtSOWDate = dtDate.AddDays(iInterval)

            ' Return the new date.

            Return dtSOWDate

        Catch



            ' Error Section.


            Return dtDate
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: PMTruncate
    '
    ' Description: Takes a Double value and Truncates to the
    '              number of decimal places specified.
    '
    ' ***************************************************************** '
    Public Function PMTruncate(ByRef dValueIn As Double, ByRef iNumOfPlaces As Integer) As Double

        Dim lMultiplier As Integer

        Try

            ' Is Number of Decimal Places Requested Invalid
            If iNumOfPlaces < 0 Then
                iNumOfPlaces = 0
            End If

            ' Is Number of Decimal Places Requested Invalid
            If iNumOfPlaces > 8 Then
                iNumOfPlaces = 8
            End If

            ' Derive the multiplier based on the number of decimal places
            lMultiplier = 10 ^ iNumOfPlaces

            ' PMTruncate the Value

            Return Math.Floor(dValueIn * lMultiplier) / lMultiplier

        Catch



            ' Error Section.

            ' Return the original value.

            Return dValueIn
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: FormatField
    '
    ' Description: Formats a field to the type specified.
    '
    ' ***************************************************************** '
    Public Function FormatField(ByRef iFormatType As Integer, ByRef vFieldValue As String) As String

        Dim sControlResult As String = ""

        Try

            ' Check for a null value

            If Convert.IsDBNull(vFieldValue) Or IsNothing(vFieldValue) Then
                vFieldValue = ""
            End If

            ' Determine which field type it is.
            Select Case (iFormatType)
                Case GeneralConst.PMFormatString
                    ' Format value to a string.
                    sControlResult = vFieldValue.Trim()

                Case GeneralConst.PMFormatStringCase
                    ' Format value to a string with proper case.
                    sControlResult = Strings.StrConv(vFieldValue, VbStrConv.ProperCase).Trim()

                Case GeneralConst.PMFormatStringUpper
                    ' Format value to a string with uppercase.
                    sControlResult = vFieldValue.Trim().ToUpper()

                Case GeneralConst.PMFormatDateShort
                    ' Format value to a short date
                    If Not Information.IsDate(vFieldValue) Then
                        sControlResult = ""
                    Else
                        sControlResult = DateTime.Parse(vFieldValue).ToString("d").Trim()
                    End If

                Case GeneralConst.PMFormatDateMedium
                    ' Format value to a medium date
                    If Not Information.IsDate(vFieldValue) Then
                        sControlResult = ""
                    Else
                        sControlResult = StringsHelper.Format(vFieldValue, "medium date").Trim()
                    End If

                Case GeneralConst.PMFormatDateLong
                    ' Format value to a long date
                    If Not Information.IsDate(vFieldValue) Then
                        sControlResult = ""
                    Else
                        sControlResult = DateTime.Parse(vFieldValue).ToString("D").Trim()
                    End If

                Case GeneralConst.PMFormatTimeShort
                    ' Format value to a short time
                    sControlResult = DateTime.Parse(vFieldValue).ToString("t").Trim()

                Case GeneralConst.PMFormatTimeMedium
                    ' Format value to a medium time
                    sControlResult = StringsHelper.Format(vFieldValue, "medium time").Trim()

                Case GeneralConst.PMFormatTimeLong
                    ' Format value to a long time
                    sControlResult = DateTime.Parse(vFieldValue).ToString("T").Trim()

                Case GeneralConst.PMFormatDateTimeShort
                    ' Format value to a short date and time
                    sControlResult = DateTime.Parse(vFieldValue).ToString("d").Trim() & _
                                     " " & DateTime.Parse(vFieldValue).ToString("t").Trim()

                Case GeneralConst.PMFormatDateTimeMedium
                    ' Format value to a medium date and time
                    sControlResult = StringsHelper.Format(vFieldValue, "medium date").Trim() & _
                                     " " & StringsHelper.Format(vFieldValue, "medium time").Trim()

                Case GeneralConst.PMFormatDateTimeLong
                    ' Format value to a long date and time
                    sControlResult = DateTime.Parse(vFieldValue).ToString("D").Trim() & _
                                     " " & DateTime.Parse(vFieldValue).ToString("T").Trim()

                Case GeneralConst.PMFormatCurrency
                    ' Format value to a currency
                    Dim dbNumericTemp As Double
                    If Not Double.TryParse(vFieldValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                        sControlResult = ""
                    Else
                        sControlResult = StringsHelper.Format(vFieldValue, "standard").Trim()
                    End If

                Case GeneralConst.PMFormatInteger
                    ' Format value to a currency
                    '            If (IsNumeric(vFieldValue) = False) Then
                    '                sControlResult$ = ""
                    '            Else
                    sControlResult = StringsHelper.Format(vFieldValue, "General Number").Trim()
                    'CInt(vFieldValue)
                    '            End If

                Case GeneralConst.PMFormatBoolean
                    ' Format value to a boolean
                    If CInt(vFieldValue) = 0 Then
                        sControlResult = CStr(False)
                    Else
                        sControlResult = CStr(True)
                    End If

                Case GeneralConst.PMFormatPercent
                    ' Format value to a currency
                    Dim dbNumericTemp2 As Double
                    If Not Double.TryParse(vFieldValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                        sControlResult = ""
                    Else
                        sControlResult = StringsHelper.Format(vFieldValue, "standard").Trim() & "%"
                    End If
            End Select

            ' Return the field value with the new
            ' formatted value.

            Return sControlResult

        Catch



            ' Error Section.

            ' Return the original value.

            Return vFieldValue
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: UnFormatField
    '
    ' Description: Unformats a field to the type specified.
    '
    ' ***************************************************************** '
    Public Function UnFormatField(ByRef iFormatTypeIn As Integer, ByRef iDataTypeOut As Integer, ByRef vFieldValue As Object) As Object

        Dim vControlResult As Object

        Try

            ' Check for a null value

            If Convert.IsDBNull(vFieldValue) Or IsNothing(vFieldValue) Then

                vFieldValue = ""
            End If

            ' Check the format of the in value.
            Select Case (iFormatTypeIn)
                Case GeneralConst.PMFormatString, GeneralConst.PMFormatStringCase, GeneralConst.PMFormatStringUpper
                    If iDataTypeOut <> GeneralConst.PMString Then

                        If CStr(vFieldValue).Trim() = "" Then

                            vFieldValue = "0"
                        End If
                    End If

                Case GeneralConst.PMFormatPercent

                    If CStr(vFieldValue).Substring(Strings.Len(CStr(vFieldValue)) - 1) = "%" Then


                        vFieldValue = CStr(vFieldValue).Substring(0, Math.Min(CStr(vFieldValue).Length, Strings.Len(CStr(vFieldValue)) - 1))
                    End If


                    Dim dbNumericTemp As Double
                    If Not Double.TryParse(CStr(vFieldValue), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                        vFieldValue = 0
                    End If

                Case GeneralConst.PMFormatDateShort, GeneralConst.PMFormatDateMedium, GeneralConst.PMFormatDateLong, GeneralConst.PMFormatTimeShort, GeneralConst.PMFormatTimeMedium, GeneralConst.PMFormatTimeLong

                    If CStr(vFieldValue) = "" Or Not Information.IsDate(vFieldValue) Then

                        vFieldValue = #12/29/1899#
                    End If

                Case GeneralConst.PMFormatCurrency, GeneralConst.PMFormatInteger, GeneralConst.PMFormatLong, GeneralConst.PMFormatDouble, GeneralConst.PMFormatBoolean

                    Dim dbNumericTemp2 As Double
                    If CStr(vFieldValue) = "" Or Not Double.TryParse(CStr(vFieldValue), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                        vFieldValue = 0
                    End If
            End Select

            ' Determine which field type it is.
            Select Case (iDataTypeOut)
                Case GeneralConst.PMString

                    Dim dbNumericTemp3 As Double
                    If (Double.TryParse(CStr(vFieldValue), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3)) Or (Information.IsDate(vFieldValue)) Then
                        ' Format value to a string.


                        vControlResult = CStr(vFieldValue).Trim()
                    Else


                        vControlResult = vFieldValue
                    End If

                Case GeneralConst.PMDate
                    ' Format value to a short date


                    vControlResult = CDate(vFieldValue)

                Case GeneralConst.PMCurrency


                    vControlResult = CDec(vFieldValue)

                Case GeneralConst.PMInteger, GeneralConst.PMLong


                    vControlResult = CInt(vFieldValue)

                Case GeneralConst.PMDouble


                    vControlResult = CDbl(vFieldValue)

                Case Else


                    vControlResult = vFieldValue
            End Select

            ' Return the field value with the new
            ' formatted value.

            Return vControlResult

        Catch



            ' Error Section.

            ' Return the original value.

            Return vFieldValue
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: LogMessageToFile
    '
    ' Description: Logs a message into Log File defined in the registry.
    '
    ' ***************************************************************** '

    'IJR 2003-02-20 Start
    'Public Sub LogMessageToFile(sUsername As String, iType As Integer, sMsg As String, Optional vApp, Optional vClass, Optional vMethod, Optional vErrNo, Optional vErrDesc)
    '
    'Dim sLogMessage As String
    'Dim iFileNumber As Integer
    'Dim sLogFile As String
    'Dim lErrorValue As Long
    '
    '    On Error GoTo Err_LogMessageToFile
    '
    '    ' Get the log file name from the registry
    '    lErrorValue& = GetRegSettings( _
    ''        sResult:=sLogFile, _
    ''        sAppName:=PMRegAppName, _
    ''        sSection:=PMRegSecSystem, _
    ''        sKey:=PMRegKeyLogFile)
    '
    '    ' Check for errors.
    '    If (lErrorValue& <> PMTrue) Then
    '
    '        ' Display registry error on screen.
    '        LogMessagePopup _
    ''            iType:=PMLogError, _
    ''            sMsg:="Failed to get the Log File Name from the registry", _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="LogMessageToFile"
    '
    '        ' Display the Error Message we were trying to log
    '        LogMessagePopup _
    ''            iType:=iType, _
    ''            sMsg:=sMsg, _
    ''            vApp:=vApp, _
    ''            vClass:=vClass, _
    ''            vMethod:=vMethod, _
    ''            vErrNo:=vErrNo, _
    ''            vErrDesc:=vErrDesc
    '
    '        Exit Sub
    '
    '    End If
    '
    '    ' Get the next free file number
    '    iFileNumber% = FreeFile
    '
    '    ' Open the log file for Append
    '    Open sLogFile$ For Append As #iFileNumber%
    '
    '    ' Set the log message to the current date/time
    '    sLogMessage$ = Now
    '
    '    ' Append the Message Type
    '    sLogMessage$ = sLogMessage$ & " " & iType%
    '
    '    ' Append the Username
    '    sLogMessage$ = sLogMessage$ & " " & sUsername
    '
    '    ' Add on the optional parameters if they are present
    '
    '    If Not (IsMissing(vApp)) Then
    '        sLogMessage$ = sLogMessage$ & " " & vApp
    '    End If
    '
    '    If Not (IsMissing(vClass)) Then
    '        sLogMessage$ = sLogMessage$ & " " & vClass
    '    End If
    '
    '    If Not (IsMissing(vMethod)) Then
    '        sLogMessage$ = sLogMessage$ & "." & vMethod
    '    End If
    '
    '    ' Add the message to the end
    '    sLogMessage$ = sLogMessage$ & " " & sMsg$
    '
    '    ' Add VB Error number and description if we have them
    '
    '    If Not (IsMissing(vErrNo)) Then
    '        sLogMessage$ = sLogMessage$ & vErrNo
    '    End If
    '
    '    If Not (IsMissing(vErrDesc)) Then
    '        sLogMessage$ = sLogMessage$ & " " & vErrDesc
    '    End If
    '
    '    ' Print the Log Message to the Log File
    '    Print #iFileNumber, sLogMessage$
    '
    '    ' Close the log file
    '    Close #iFileNumber%
    '
    '    Exit Sub
    '
    'Err_LogMessageToFile:
    '
    '    ' Log This Error.
    '    LogMessagePopup _
    ''        iType:=PMLogFatal, _
    ''        sMsg:="Failed to log the following error in Log File : " & sLogFile$, _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="LogMessageToFile", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    ' We have failed to Log the message in the log file
    '    ' so display the error we were trying to log.
    '
    '    ' Log Error.
    '    LogMessagePopup _
    ''        iType:=iType, _
    ''        sMsg:=sMsg, _
    ''        vApp:=vApp, _
    ''        vClass:=vClass, _
    ''        vMethod:=vMethod, _
    ''        vErrNo:=vErrNo, _
    ''        vErrDesc:=vErrDesc
    '
    '    Exit Sub
    '

    'IJR 2003-02-20 End

    ' ***************************************************************** '
    ' Name: LogMessagePopup
    '
    ' Description: Logs a message into the PM Log File.
    '
    ' ***************************************************************** '

    'IJR 2003-02-20 Start
    'This function is in gPMFunctions.bas
    'Public Sub LogMessagePopup(iType As Integer, sMsg As String, Optional vApp, Optional vClass, Optional vMethod, Optional vErrNo, Optional vErrDesc)
    '
    'Dim sLogMessage As String
    'Dim iMsgboxParms As Integer
    'Dim iTypeOfMessage As Integer
    'Dim sMessageTypeText As String
    '
    '    On Error GoTo Err_LogMessagePopup
    '
    '    ' Set Message Type and description
    '
    '    Select Case iType%
    '      Case PMLogFatal
    '        iTypeOfMessage = vbCritical
    '        sMessageTypeText = PMFatalText
    '      Case PMLogError
    '        iTypeOfMessage = vbCritical
    '        sMessageTypeText = PMErrorText
    '      Case PMLogWarning
    '        iTypeOfMessage = vbExclamation
    '        sMessageTypeText = PMWarningText
    '      Case PMLogInfo
    '        iTypeOfMessage = vbExclamation
    '        sMessageTypeText = PMInfoText
    '      Case PMLogOnError
    '        iTypeOfMessage = vbExclamation
    '        sMessageTypeText = PMOnErrorText
    '      Case PMLogDebug1
    '        iTypeOfMessage = vbInformation
    '        sMessageTypeText = PMDebug1Text
    '      Case PMLogDebug2
    '        iTypeOfMessage = vbInformation
    '        sMessageTypeText = PMDebug2Text
    '      Case PMLogDebug3
    '        iTypeOfMessage = vbInformation
    '        sMessageTypeText = PMDebug3Text
    '      Case Else
    '        iTypeOfMessage = vbInformation
    '        sMessageTypeText = PMDebug4Text
    '    End Select
    '
    '    ' Msgbox Parameters are
    '    ' OK Button Only, Type of Message, Button1 is defualt, Application Modal
    '    iMsgboxParms = vbOKOnly + iTypeOfMessage + vbDefaultButton1 + vbApplicationModal
    '
    '    ' Current date/time
    '    sLogMessage = "Date/Time: " & Now & Chr$(13) & Chr$(10)
    '
    '    ' Display optional parameters if they are present
    '    If Not (IsMissing(vApp)) Then
    '        sLogMessage = sLogMessage & "Application: " & vApp & Chr$(13) & Chr$(10)
    '    End If
    '
    '    If Not (IsMissing(vClass)) Then
    '        sLogMessage = sLogMessage & "Class: " & vClass & Chr$(13) & Chr$(10)
    '    End If
    '
    '    If Not (IsMissing(vMethod)) Then
    '        sLogMessage = sLogMessage & "Method: " & vMethod & Chr$(13) & Chr$(10)
    '    End If
    '
    '    ' Add the message to the end
    '    sLogMessage = sLogMessage & "Message Text: " & sMsg & Chr$(13) & Chr$(10)
    '
    '    ' Add VB Error number and description if we have them
    '
    '    If Not (IsMissing(vErrNo)) Then
    '        sLogMessage = sLogMessage & "VB Error Number: " & vErrNo & Chr$(13) & Chr$(10)
    '    End If
    '
    '    If Not (IsMissing(vErrDesc)) Then
    '        sLogMessage = sLogMessage & "VB Error Text: " & vErrDesc
    '    End If
    '
    '    ' Display the Log Message on the screen
    '    MsgBox sLogMessage, iMsgboxParms, sMessageTypeText & " Message"
    '
    '    Exit Sub
    '
    'Err_LogMessagePopup:
    '
    '    ' Error Section.
    '    Exit Sub
    '

    'IJR 2003-02-20 End

    ' ***************************************************************** '
    ' Name: GetRegSettings
    '
    ' Description: Get settings from the registry.
    '
    ' ***************************************************************** '
    Public Function GetRegSettings(ByRef sResult As String, ByRef sAppName As String, ByRef sSection As String, ByRef sKey As String, Optional ByRef vDefault As String = "") As Integer

        Dim result As Integer = 0
        Try

            result = GeneralConst.PMTrue

            ' Check if we have the optional parameter.

            If Information.IsNothing(vDefault) Then
                ' Get setting from the registry not
                ' using the optional parameter.
                sResult = Interaction.GetSetting(sAppName, sSection, sKey, )
            Else
                ' Get setting from the registry
                ' using the optional parameter.
                sResult = Interaction.GetSetting(sAppName, sSection, sKey, vDefault)
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = GeneralConst.PMError

            ' Log Error.
            iGeneralFunc.LogMessage(iType:=GeneralConst.PMLogOnError, sMsg:="Failed to get the registry settings", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRegSettings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetRegAllSettings
    '
    ' Description: Get all section settings from the registry.
    '
    ' ***************************************************************** '
    Public Function GetRegAllSettings(ByRef vResult(,) As Object, ByRef sAppName As String, ByRef sSection As String) As Integer

        Dim result As Integer = 0
        Try

            result = GeneralConst.PMTrue

            ' Get section setting from the registry.
            vResult = Interaction.GetAllSettings(sAppName, sSection)

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = GeneralConst.PMError

            ' Log Error.
            iGeneralFunc.LogMessage(iType:=GeneralConst.PMLogOnError, sMsg:="Failed to get all of the registry settings", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRegAllSettings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SaveRegSettings
    '
    ' Description: Save settings to the registry.
    '
    ' ***************************************************************** '
    Public Function SaveRegSettings(ByRef sSetting As String, ByRef sAppName As String, ByRef sSection As String, ByRef sKey As String) As Integer

        Dim result As Integer = 0
        Try

            result = GeneralConst.PMTrue

            ' Save setting to the registry.
            Interaction.SaveSetting(sAppName, sSection, sKey, sSetting)

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = GeneralConst.PMError

            ' Log Error.
            iGeneralFunc.LogMessage(iType:=GeneralConst.PMLogOnError, sMsg:="Failed to save the registry settings", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveRegSettings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteRegSettings
    '
    ' Description: Delete settings from the registry.
    '
    ' ***************************************************************** '
    Public Function DeleteRegSettings(ByRef sAppName As String, ByRef sSection As String, ByRef sKey As String) As Integer

        Dim result As Integer = 0
        Try

            result = GeneralConst.PMTrue

            ' Delete setting from the registry.
            Interaction.DeleteSetting(sAppName, sSection, sKey)

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = GeneralConst.PMError

            ' Log Error.
            iGeneralFunc.LogMessage(iType:=GeneralConst.PMLogOnError, sMsg:="Failed to delete the registry settings", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRegSettings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Encrypt
    '
    ' Description: Encrypts string passed and returns the result.
    '
    ' ***************************************************************** '
    Public Function Encrypt(ByRef sPassword As String, ByRef sEncryptedPassword As String) As Integer

        Dim result As Integer = 0
        Dim sAString As String = ""
        Dim sBString As New StringBuilder
        Dim iCntr As Integer
        Dim sChar1 As New FixedLengthString(1)
        Dim sChar2 As New FixedLengthString(1)
        Dim iSn As Integer
        Dim sCodeString As String = ""
        Dim iClen As Integer

        Try

            result = GeneralConst.PMTrue

            ' Encrypts the supplied string returning the encrypted
            ' result. Encrypted string will always be 2 characters
            ' longer than original (leave space!)
            '
            ' Encrypted string contains only ASCII characters in
            ' range 32-126

            sCodeString = "aPCXADneGgH7khIJpjKtBMzmQLrRcqSEsbUv6yuVFW9xYZ2T3fd4w5N8"
            iClen = sCodeString.Length

            sAString = sPassword
            iCntr = sAString.Length

            If iCntr < 1 Then
                result = GeneralConst.PMFalse

                sEncryptedPassword = ""

                Return result
            End If

            sChar1.Value = sCodeString.Substring((Strings.Asc(sAString.Substring(0, 1)(0)) + iCntr) Mod iClen, 1)
            sChar2.Value = sCodeString.Substring(Strings.Asc(sAString.Substring(sAString.Length - 1)(0)) Mod iClen, 1)
            iSn = ((Strings.Asc(sChar1.Value(0)) + Strings.Asc(sChar2.Value(0))) Mod iClen) + 1
            sBString = New StringBuilder(sChar2.Value)

            For iCntr2 As Integer = 1 To iCntr
                sBString.Append(sCodeString.Substring((Strings.Asc(sAString.Substring(iCntr2 - 1, 1)(0)) + iSn + iCntr2) Mod iClen, 1))
            Next iCntr2

            sBString.Append(sChar1.Value)

            ' Return the result.
            sEncryptedPassword = sBString.ToString().Trim()

            Return result

        Catch excep As System.Exception



            ' Error Section

            result = GeneralConst.PMError

            sEncryptedPassword = ""

            ' Log Error.
            iGeneralFunc.LogMessage(iType:=GeneralConst.PMLogOnError, sMsg:="Failed to encrypt the string", vApp:=ACApp, vClass:=ACClass, vMethod:="Encrypt", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetResData
    '
    ' Description: Gets a data value from the resource file using the
    '              ID passed.
    '
    ' ***************************************************************** '
    Public Function GetResData(ByRef iLangID As Integer, ByRef lID As Integer, ByRef iDataType As Integer) As Object

        Dim result As Object = Nothing
        Dim lLangID As Integer

        Try

            ' Get data value from the resource file using
            ' the data type to determine what type of value
            ' to retrieve.

            ' Calculate language offset from the language
            ' ID passed.
            lLangID = (iLangID * GeneralConst.PMLangOffSetValue) + lID

            Select Case (iDataType)
                Case GeneralConst.PMResString
                    ' Get string value.
                    result = My.Resources.ResourceManager.GetString("str" + CStr(lLangID))

                Case GeneralConst.PMResBitmap
                    ' Get bitmap value.
                    result = My.Resources.ResourceManager.GetObject("bmp" + CStr(lLangID))

                Case GeneralConst.PMResIcon
                    ' Get Icon value.
                    result = My.Resources.ResourceManager.GetObject("ico" + CStr(lLangID))

                Case GeneralConst.PMResCursor
                    ' Get cursor value.
                    result = My.Resources.ResourceManager.GetObject("cur" + CStr(lLangID))
            End Select

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = ""

            ' Log Error.
            iGeneralFunc.LogMessage(iType:=GeneralConst.PMLogOnError, sMsg:="Failed to get data from the resource file", vApp:=ACApp, vClass:=ACClass, vMethod:="GetResData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSystemName
    '
    ' Description: Gets the system (computer) name.
    '
    ' ***************************************************************** '
    Public Function GetSystemName(ByRef sSystemName As String) As Integer

        Dim result As Integer = 0
        Dim lResult As Integer
        Dim sBuffer As New FixedLengthString(255)
        Dim lBufferSize As Integer

        Try

            result = GeneralConst.PMTrue

            lBufferSize = 255

            ' API Call to get computer name
            lResult = GetComputerName(sBuffer.Value, lBufferSize)

            ' Check return code
            If lResult <> 1 Then
                ' Return error
                sSystemName = ""
                result = GeneralConst.PMFalse
            Else
                ' Set System Name Parameter
                sSystemName = sBuffer.Value.Substring(0, Math.Min(sBuffer.Value.Length, lBufferSize))
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = GeneralConst.PMLogOnError
            sSystemName = ""

            ' Log Error.
            iGeneralFunc.LogMessage(iType:=GeneralConst.PMLogOnError, sMsg:="Failed to get computer name", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSystemName", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FindVarField
    '
    ' Description: Finds the Position of a Variable Data Field within
    '              a Variable Data Block
    '
    ' ***************************************************************** '
    Public Function FindVarField(ByRef sRecordName As String, ByRef sFieldName As String, ByRef vVarDataBlock(,) As Object, ByRef lPosition As Integer) As Integer

        Dim result As Integer = 0

        Try

            result = GeneralConst.PMTrue

            ' Check that Var Data Block is an Array
            If Not Information.IsArray(vVarDataBlock) Then
                Return GeneralConst.PMFalse
            End If

            ' Check that there is a record & field name to search for
            If (sFieldName.Trim() = "") Or (sRecordName.Trim() = "") Then
                Return GeneralConst.PMFalse
            End If

            ' Search for Field in Array using RecordName & FieldName
            ' Convert both to Upper to avoid any case mistakes.
            For l_Row As Integer = vVarDataBlock.GetLowerBound(1) To vVarDataBlock.GetUpperBound(1)

                If (sRecordName.Trim() & sFieldName.Trim()).ToUpper() = (CStr(vVarDataBlock(GeneralConst.PMVarRecordName, l_Row)).Trim() & CStr(vVarDataBlock(GeneralConst.PMVarFieldName, l_Row)).Trim()).ToUpper() Then
                    lPosition = l_Row
                    Return result
                End If
            Next l_Row

            ' Field Not Found so return error

            Return GeneralConst.PMFalse

        Catch excep As System.Exception



            ' Error Section.
            result = GeneralConst.PMLogOnError

            ' Log Error.
            iGeneralFunc.LogMessage(iType:=GeneralConst.PMLogOnError, sMsg:="Failed to Find " & sFieldName & " in variable data block.", vApp:=ACApp, vClass:=ACClass, vMethod:="FindVarData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetCommandLine
    '
    ' Description: Gets the command line passed and splits it down
    '              into the seporate arguments.
    '
    ' ***************************************************************** '
    Public Function GetCommandLine(ByRef vArgArray() As Object, Optional ByRef vMaxArgs As Object = Nothing) As Integer

        'Declare variables.
        Dim result As Integer = 0
        Dim sChar, sCmdLine As String
        Dim iCmdLineLen As Integer
        Dim bInArg As Boolean
        Dim iMaxArgs, iNumArgs As Integer

        Try

            result = GeneralConst.PMTrue

            'See if MaxArgs was provided.

            Dim dbNumericTemp As Double

            If (Not Information.IsNothing(vMaxArgs)) And (Double.TryParse(CStr(vMaxArgs), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then

                iMaxArgs = CInt(vMaxArgs)
            Else
                iMaxArgs = 100
            End If

            'Make array of the correct size.
            ReDim vArgArray(iMaxArgs)

            ' Initialise
            iNumArgs = 0
            bInArg = False

            'Get command line arguments.
            sCmdLine = Interaction.Command()

            ' Get the length of the command line
            iCmdLineLen = sCmdLine.Length

            'Go thru command line one character at a time.
            For iSub As Integer = 1 To iCmdLineLen

                sChar = Mid(sCmdLine, iSub, 1)

                'Test for space or tab.
                If (sChar <> " ") And (sChar <> Strings.Chr(9)) Then

                    'Neither space nor tab.

                    'Test if already in argument.
                    If Not bInArg Then

                        'New argument begins.

                        'Test for too many arguments.
                        If iNumArgs >= iMaxArgs Then
                            Exit For
                        End If

                        iNumArgs += 1
                        bInArg = True

                    End If

                    'Concatenate character to current argument.


                    vArgArray(iNumArgs - 1) = CStr(vArgArray(iNumArgs - 1)) & sChar

                Else

                    'Found a space or tab.
                    'Set bInArg flag to False.
                    bInArg = False

                End If

            Next iSub

            'Resize array just enough to hold arguments.
            If iNumArgs > 0 Then
                ReDim Preserve vArgArray(iNumArgs - 1)
            Else

                vArgArray = Nothing
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = GeneralConst.PMLogOnError

            ' Log Error.
            iGeneralFunc.LogMessage(iType:=GeneralConst.PMLogOnError, sMsg:="Failed to Process the Command Line :- " & sCmdLine, vApp:=ACApp, vClass:=ACClass, vMethod:="GetCommandLine", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function
    'Modified by Deepak Sharma on 4/20/2010 2:07:21 PM refer developer guide no. 29(No Solution)
    'Shared Sub New()
    '	MainModule.JustForInvokeMain()

    Public Sub LogUsageMessage(ByVal productName As String)
        'Dim fileName As String = "ProjectUsageLog_" + System.DateTime.Now.ToString("ddMMMyyyy") + ".txt"
        'Dim filePath As String = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "Logs")

        'If Not Directory.Exists(filePath) Then
        '    Directory.CreateDirectory(filePath)
        'End If

        'fileName = Path.Combine(filePath, fileName)

        'If Not File.Exists(fileName) Then
        '    Dim fs As FileStream = File.Create(fileName)
        '    fs.Close()
        '    fs.Dispose()
        'End If

        'Using logWriter As StreamWriter = File.AppendText(fileName)
        '    logWriter.WriteLine("Project: " + productName + " is used.")
        'End Using
    End Sub

End Module
