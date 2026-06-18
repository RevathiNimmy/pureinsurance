Option Strict Off
Option Explicit On

Module gPMFunctions

    ' ***************************************************************** '
    ' Module Name: PMFunctions
    '
    ' Date: 20th January 1998
    '
    ' Description: This class maps ALL of the constants currently
    '              provided by PMFunc.bas with the following exceptions:
    '
    '              PMRoundUp    - Replaced by PMRoundUpValueCurrency &
    '                             PMRoundUpValueVDecimal
    '              PMTruncate   - Replaced by PMTruncateCurrency &
    '                             PMTruncatevDecimal
    '              FindVarField - This is Sirius specific.
    '                             Look in gSirLibraries for this method.
    '              Encrypt      - Not implemented here for security
    '                             reasons.
    '                             Implemented in iPMFunc & bPMFunc.
    '              GetCommandLine Not implemented here as it needs to
    '                             be at source level for it to work.
    '                             It will be implemented with Encrypt.
    '                             Implemented in iPMFunc & bPMFunc.
    '              GetResData     Moved to iPMFunc as it needs to be
    '                             at source level.
    '
    ' Edit History: 20/01/98    Original created                     RFC
    '               27/02/98    GetResData moved to iPMFunc          RFC
    '               04/03/98    Added Money formatting for Voyager   SP
    '               20/04/98    LogMessageToFile amended to use      RFC
    '               20/04/98    default lof file if non found in reg.RFC
    '               20/04/98    ArchitectureInDebug method added.    RFC
    '               08/06/98    GetPMRegSetting and SetPMRegSetting. RFC
    '               08/06/98    ArchitectureInDebug & LogMessageToFilRFC
    '               08/06/98    amended to use new Registry methods. RFC
    '               19/06/98    Format Month Long and Short added.   RFC
    '               25/11/98    Added SiriusSolutions & Nirvana      RFC
    '                           Registry Constants.                  RFC
    'RFC060799 - Added GeminiII Product Family, DSN etc etc
    'RDC12062002 - Changed to BAS module
    ' ***************************************************************** '

    ' Constant for the methods to identify
    ' which class this is.
    Private Const ACClass As String = "gPMFunctions"
    ' RDC 12062002
    Private Const ACApp As String = "gPMFunctions"

    Private Declare Function GetComputerName Lib "kernel32" Alias "GetComputerNameA" (ByVal lpBuffer As String, ByRef nSize As Integer) As Integer

    ' RDC 12072002 Windows versioning resources
    Private Structure OSVERSIONINFO
        Dim dwOSVersionInfoSize As Integer
        Dim dwMajorVersion As Integer
        Dim dwMinorVersion As Integer
        Dim dwBuildNumber As Integer
        Dim dwPlatformId As Integer
        <VBFixedString(128), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=128)> Public szCSDVersion As String
    End Structure

    'UPGRADE_WARNING: Structure OSVERSIONINFO may require marshalling attributes to be passed as an argument in this Declare statement. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1050"'
    Private Declare Function GetVersionEx Lib "kernel32" Alias "GetVersionExA" (ByRef lpVersionInformation As OSVERSIONINFO) As Integer

    ' RDC 12072002 NT account information resources
    Private Enum EXTENDED_NAME_FORMAT
        NameUnknown = 0
        NameFullyQualifiedDN = 1
        NameSamCompatible = 2
        NameDisplay = 3
        NameUniqueId = 6
        NameCanonical = 7
        NameUserPrincipal = 8
        NameCanonicalEx = 9
        NameServicePrincipal = 10
    End Enum

    'UPGRADE_WARNING: Structure EXTENDED_NAME_FORMAT may require marshalling attributes to be passed as an argument in this Declare statement. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1050"'
    Private Declare Function GetUserNameEx Lib "secur32.dll" Alias "GetUserNameExA" (ByVal lpType As EXTENDED_NAME_FORMAT, ByVal lpName As String, ByRef lpwLength As Integer) As Integer

    ' Registry services
    Declare Function RegCloseKey Lib "advapi32.dll" (ByVal hKey As Integer) As Integer

    Declare Function RegCreateKeyEx Lib "advapi32.dll" Alias "RegCreateKeyExA" (ByVal hKey As Integer, ByVal lpSubKey As String, ByVal Reserved As Integer, ByVal lpClass As String, ByVal dwOptions As Integer, ByVal samDesired As Integer, ByVal lpSecurityAttributes As Integer, ByRef phkResult As Integer, ByRef lpdwDisposition As Integer) As Integer

    Declare Function RegOpenKeyEx Lib "advapi32.dll" Alias "RegOpenKeyExA" (ByVal hKey As Integer, ByVal lpSubKey As String, ByVal ulOptions As Integer, ByVal samDesired As Integer, ByRef phkResult As Integer) As Integer

    Declare Function RegQueryValueExString Lib "advapi32.dll" Alias "RegQueryValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByRef lpType As Integer, ByVal lpData As String, ByRef lpcbData As Integer) As Integer

    Declare Function RegQueryValueExLong Lib "advapi32.dll" Alias "RegQueryValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByRef lpType As Integer, ByRef lpData As Integer, ByRef lpcbData As Integer) As Integer

    Declare Function RegQueryValueExNULL Lib "advapi32.dll" Alias "RegQueryValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByRef lpType As Integer, ByVal lpData As Integer, ByRef lpcbData As Integer) As Integer

    Declare Function RegSetValueExString Lib "advapi32.dll" Alias "RegSetValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal Reserved As Integer, ByVal dwType As Integer, ByVal lpValue As String, ByVal cbData As Integer) As Integer

    Declare Function RegSetValueExLong Lib "advapi32.dll" Alias "RegSetValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal Reserved As Integer, ByVal dwType As Integer, ByRef lpValue As Integer, ByVal cbData As Integer) As Integer

    Structure FILETIME
        Dim dwLowDateTime As Integer
        Dim dwHighDateTime As Integer
    End Structure

    'UPGRADE_WARNING: Structure FILETIME may require marshalling attributes to be passed as an argument in this Declare statement. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1050"'
    Declare Function RegEnumKeyEx Lib "advapi32.dll" Alias "RegEnumKeyExA" (ByVal hKey As Integer, ByVal dwIndex As Integer, ByVal lpName As String, ByRef lpcbName As Integer, ByRef lpReserved As Integer, ByVal lpClass As String, ByRef lpcbClass As Integer, ByRef lpftLastWriteTime As FILETIME) As Integer

    Declare Function RegEnumKey Lib "advapi32.dll" Alias "RegEnumKeyA" (ByVal hKey As Integer, ByVal dwIndex As Integer, ByVal lpName As String, ByVal cbName As Integer) As Integer

    Declare Function RegDeleteKey Lib "advapi32.dll" Alias "RegDeleteKeyA" (ByVal hKey As Integer, ByVal lpSubKey As String) As Integer

    Declare Function RegDeleteValue Lib "advapi32.dll" Alias "RegDeleteValueA" (ByVal hKey As Integer, ByVal lpValueName As String) As Integer

    ' ################################################################################################### END

    ' RDC 11072002
    Public Declare Function WNetGetUser Lib "mpr.dll" Alias "WNetGetUserA" (ByVal lpName As String, ByVal lpUserName As String, ByRef LPNLENGTH As Integer) As Integer

    ' RDC 19072002 enum and declares for Windows Terminal Services functions
    Private Enum WTS_INFO_CLASS
        WTSInitialProgram
        WTSApplicationName
        WTSWorkingDirectory
        WTSOEMId
        WTSSessionID
        WTSUserName
        WTSWinStationName
        WTSDomainName
        WTSConnectState
        WTSClientBuildNumber
        WTSClientName
        WTSClientDirectory
        WTSClientProductId
        WTSClientHardwareId
        WTSClientAddress
        WTSClientDisplay
        WTSClientProtocolType
    End Enum

    'UPGRADE_WARNING: Structure WTS_INFO_CLASS may require marshalling attributes to be passed as an argument in this Declare statement. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1050"'
    Private Declare Function WTSQuerySessionInformation Lib "wtsapi32.dll" Alias "WTSQuerySessionInformationA" (ByVal hServer As Integer, ByVal hSessionId As Integer, ByVal lWSI As WTS_INFO_CLASS, ByRef lptBuffer As Integer, ByRef lBytes As Integer) As Integer

    Private Declare Sub WTSFreeMemory Lib "wtsapi32.dll" (ByVal pMemory As Integer)

    '	'UPGRADE_ISSUE: Declaring a parameter 'As Any' is not supported. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1016"''
    'Private Declare Sub CopyMemoryFromAddress Lib "kernel32"  Alias "RtlMoveMemory"(ByRef lpDestination As Any, ByVal lplpSource As Integer, ByVal length As Integer)


    ' RDC 12062002 classes changed to BAS modules

    ' ***************************************************************** '
    ' Name: PMStartOfWeek
    '
    ' Description: Returns the start of the week date from the given
    '              date.
    '
    ' ***************************************************************** '
    Public Function PMStartOfWeek(ByVal dtDate As Date) As Date

        Dim iInterval As Short
        Dim dtSOWDate As Date

        Try

        ' Check what the current day is.
        If (Format(dtDate, "w") = CStr(FirstDayOfWeek.Sunday)) Then
            ' Set the interval value.
            iInterval = -6
        Else
            ' Set the interval value.
            iInterval = (CDbl(Format(dtDate, "w")) - 2) * -1
        End If

        ' Calculate the start of the week.
        dtSOWDate = DateAdd(Microsoft.VisualBasic.DateInterval.Day, iInterval, dtDate)

        ' Return the new date.
        PMStartOfWeek = dtSOWDate

        Exit Function

        Catch ex As Exception

        ' Error Section.

        PMStartOfWeek = dtDate

        Exit Function

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: UnFormatField
    '
    ' Description: Unformats a field to the type specified.
    '
    ' ***************************************************************** '
    Public Function UnFormatField(ByRef iFormatTypeIn As Short, ByRef iDataTypeOut As Short, ByRef vFieldValue As Object) As Object

        Dim vControlResult As Object

        Try

        ' Check for a null value
        'UPGRADE_WARNING: Use of Null/IsNull() detected. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1049"'
        If (IsDBNull(vFieldValue) = True) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object vFieldValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            vFieldValue = ""
        End If

        ' Check the format of the in value.
        Select Case (iFormatTypeIn)
            Case gPMConstants.PMEFormatStyle.PMFormatString, gPMConstants.PMEFormatStyle.PMFormatStringCase, gPMConstants.PMEFormatStyle.PMFormatStringUpper, gPMConstants.PMEFormatStyle.PMFormatStringMultiLine
                If (iDataTypeOut <> gPMConstants.PMEDataType.PMString) Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object vFieldValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    If (Trim(vFieldValue) = "") Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object vFieldValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                        vFieldValue = "0"
                    End If
                End If

            Case gPMConstants.PMEFormatStyle.PMFormatPercent
                'UPGRADE_WARNING: Couldn't resolve default property of object vFieldValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                If (Mid(vFieldValue, Len(vFieldValue), 1) = "%") Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object vFieldValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    vFieldValue = Mid(vFieldValue, 1, Len(vFieldValue) - 1)
                End If

                If (IsNumeric(vFieldValue) = False) Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object vFieldValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    vFieldValue = 0
                End If

                'DAK230600 - allow for American style long date formats
                '        Case PMFormatDateShort, PMFormatDateMedium, _
                'PMFormatDateLong, PMFormatTimeShort, _
                'PMFormatTimeMedium, PMFormatTimeLong
            Case gPMConstants.PMEFormatStyle.PMFormatDateShort, gPMConstants.PMEFormatStyle.PMFormatDateMedium, gPMConstants.PMEFormatStyle.PMFormatTimeShort, gPMConstants.PMEFormatStyle.PMFormatTimeMedium, gPMConstants.PMEFormatStyle.PMFormatTimeLong, gPMConstants.PMEFormatStyle.PMFormatDateTimeShort, gPMConstants.PMEFormatStyle.PMFormatDateTimeMedium
                'UPGRADE_WARNING: Couldn't resolve default property of object vFieldValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                If (vFieldValue = "" Or IsDate(vFieldValue) = False) Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object vFieldValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    vFieldValue = System.DateTime.FromOADate(-1)
                End If
                'SJ 04/03/98 Added money types
            Case gPMConstants.PMEFormatStyle.PMFormatCurrency, gPMConstants.PMEFormatStyle.PMFormatInteger, gPMConstants.PMEFormatStyle.PMFormatLong, gPMConstants.PMEFormatStyle.PMFormatDouble, gPMConstants.PMEFormatStyle.PMFormatBoolean, gPMConstants.PMEFormatStyle.PMFormatDecimal, gPMConstants.PMEFormatStyle.PMFormatMoney, gPMConstants.PMEFormatStyle.PMFormatWholeMoney
                'UPGRADE_WARNING: Couldn't resolve default property of object vFieldValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                If (vFieldValue = "" Or IsNumeric(vFieldValue) = False) Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object vFieldValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    vFieldValue = 0
                End If
                'DAK230600 - allow for american style date formats
            Case gPMConstants.PMEFormatStyle.PMFormatDateLong, gPMConstants.PMEFormatStyle.PMFormatDateTimeLong
                'UPGRADE_WARNING: Couldn't resolve default property of object vFieldValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                If vFieldValue = "" Then
                    'UPGRADE_WARNING: Couldn't resolve default property of object vFieldValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    vFieldValue = System.DateTime.FromOADate(-1)
                ElseIf IsDate(vFieldValue) = False Then
                    ' Remove everything up to and including the 1st space
                    If TrimDate(vFieldValue) <> gPMConstants.PMEReturnCode.PMTrue Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object vFieldValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                        vFieldValue = System.DateTime.FromOADate(-1)
                    ElseIf IsDate(vFieldValue) = False Then
                        'UPGRADE_WARNING: Couldn't resolve default property of object vFieldValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                        vFieldValue = System.DateTime.FromOADate(-1)
                    End If
                End If
                'DAK230600
            Case Else
                ' Do nothing
        End Select

        ' Determine which field type it is.
        Select Case (iDataTypeOut)
            Case gPMConstants.PMEDataType.PMString
                If (IsNumeric(vFieldValue) = True) Or (IsDate(vFieldValue) = True) Then
                    ' Format value to a string.
                    'UPGRADE_WARNING: Couldn't resolve default property of object vFieldValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object vControlResult. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    vControlResult = Trim(CStr(vFieldValue))
                Else
                    'UPGRADE_WARNING: Couldn't resolve default property of object vFieldValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    'UPGRADE_WARNING: Couldn't resolve default property of object vControlResult. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                    vControlResult = vFieldValue
                End If

            Case gPMConstants.PMEDataType.PMDate
                ' Format value to a short date
                'UPGRADE_WARNING: Couldn't resolve default property of object vFieldValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                'UPGRADE_WARNING: Couldn't resolve default property of object vControlResult. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                vControlResult = CDate(vFieldValue)

            Case gPMConstants.PMEDataType.PMCurrency
                'UPGRADE_WARNING: Couldn't resolve default property of object vFieldValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                'UPGRADE_WARNING: Couldn't resolve default property of object vControlResult. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                vControlResult = CDec(vFieldValue)

            Case gPMConstants.PMEDataType.PMInteger
                'UPGRADE_WARNING: Couldn't resolve default property of object vFieldValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                'UPGRADE_WARNING: Couldn't resolve default property of object vControlResult. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                vControlResult = CShort(vFieldValue)

            Case gPMConstants.PMEDataType.PMLong
                'UPGRADE_WARNING: Couldn't resolve default property of object vFieldValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                'UPGRADE_WARNING: Couldn't resolve default property of object vControlResult. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                vControlResult = CInt(vFieldValue)

            Case gPMConstants.PMEDataType.PMDouble
                'UPGRADE_WARNING: Couldn't resolve default property of object vFieldValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                'UPGRADE_WARNING: Couldn't resolve default property of object vControlResult. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                vControlResult = CDbl(vFieldValue)

            Case gPMConstants.PMEFormatStyle.PMFormatDecimal
                'UPGRADE_WARNING: Couldn't resolve default property of object vFieldValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                'UPGRADE_WARNING: Couldn't resolve default property of object vControlResult. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                vControlResult = CDec(vFieldValue)

            Case Else
                'UPGRADE_WARNING: Couldn't resolve default property of object vFieldValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                'UPGRADE_WARNING: Couldn't resolve default property of object vControlResult. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                vControlResult = vFieldValue
        End Select

        ' Return the field value with the new
        ' formatted value.
        'UPGRADE_WARNING: Couldn't resolve default property of object vControlResult. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
        'UPGRADE_WARNING: Couldn't resolve default property of object UnFormatField. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
        UnFormatField = vControlResult

        Exit Function

        Catch ex As Exception

        ' Error Section.

        ' Return the original value.
        'UPGRADE_WARNING: Couldn't resolve default property of object vFieldValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
        'UPGRADE_WARNING: Couldn't resolve default property of object UnFormatField. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
        UnFormatField = vFieldValue

        Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LogMessageToFile
    '
    ' Description: Logs a message into Log File defined in the registry.
    '
    ' ***************************************************************** '
    Public Sub LogMessageToFile(ByRef sUsername As String, ByRef iType As Short, ByRef sMsg As String, Optional ByRef vApp As Object = Nothing, Optional ByRef vClass As Object = Nothing, Optional ByRef vMethod As Object = Nothing, Optional ByRef vErrNo As Object = Nothing, Optional ByRef vErrDesc As Object = Nothing)

        Dim sLogMessage As String
        Dim iFileNumber As Short
        Dim sLogFile As String = String.Empty
        Dim lErrorValue As Integer
        'DAK120100
        Dim lSub As Integer
        Dim sPrefix As String
        Dim sSuffix As String


        Try

        '    ' Get the log file name from the registry
        '    lErrorValue& = GetRegSettings( _
        ''        sResult:=sLogFile, _
        ''        sAppName:=m_oConst.PMRegAppName, _
        ''        sSection:=m_oConst.PMRegSecSystem, _
        ''        sKey:=m_oConst.PMRegKeyLogFile)

        ' Get User Specific Log File if there is one
        ' Get the Log File Name setting from
        ' HKEY_CURRENT_USER\software\PM\SiriusArchitecture\Common
        ' RDC 12062002 classes changed to BAS modules
        '    lErrorValue& = GetPMRegSetting( _
        'v_lPMERegSettingRoot:=pmeRSRCurrentUser, _
        'v_lPMEProductFamily:=pmePFSiriusArchitecture, _
        'v_lPMERegSettingLevel:=pmeRSLCommon, _
        'v_sSettingName:=m_oConst.PMRegKeyLogFile, _
        'r_sSettingValue:=sLogFile)
        lErrorValue = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=PMRegKeyLogFile, r_sSettingValue:=sLogFile)

        ' If there is no user specific log file
        If (Trim(sLogFile) = "") Then
            ' Get Machine Default
            ' Get the Log File Name setting from
            ' HKEY_LOCAL_MACHINE\software\PM\SiriusArchitecture\Common
            ' RDC 12062002 classes changed to BAS modules
            '        lErrorValue& = GetPMRegSetting( _
            'v_lPMERegSettingRoot:=pmeRSRLocalMachine, _
            'v_lPMEProductFamily:=pmePFSiriusArchitecture, _
            'v_lPMERegSettingLevel:=pmeRSLCommon, _
            'v_sSettingName:=m_oConst.PMRegKeyLogFile, _
            'r_sSettingValue:=sLogFile)
            lErrorValue = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=PMRegKeyLogFile, r_sSettingValue:=sLogFile)

            'DAK120100
            '        If (Trim$(sLogFile$) <> "") Then
            '        ' Insert the username
            '            lErrorValue& = InsertUserName(sLogFile, sUsername)
            '            If lErrorValue& <> PMTrue Then
            '                GoTo Err_LogMessageToFile
            '            End If
            '        End If

        End If

        ' Check for errors.
        If ((lErrorValue <> gPMConstants.PMEReturnCode.PMTrue) Or (Trim(sLogFile) = "")) Then
            ' If there was no Log File in the Registry,
            ' use the default one.
            ' RDC 12062002 classes changed to BAS modules
            '        sLogFile = m_oConst.PMDefaultLogFile
            sLogFile = PMDefaultLogFile
            'DAK120100
            ' Insert the username
            '        lErrorValue& = InsertUserName(sLogFile, sUsername)
            '        If lErrorValue& <> PMTrue Then
            '            GoTo Err_LogMessageToFile
            '        End If

        End If

        ' Get the next free file number
        iFileNumber = FreeFile()

        ' Open the log file for Append
        FileOpen(iFileNumber, sLogFile, OpenMode.Append)

        ' Set the log message to the current date/time
        sLogMessage = CStr(Now)

        ' Append the Message Type
        sLogMessage = sLogMessage & " " & iType

        ' Append the Username
        sLogMessage = sLogMessage & " " & sUsername

        ' Add on the optional parameters if they are present

        'UPGRADE_NOTE: IsMissing() was changed to IsNothing(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1021"'
        If Not (IsNothing(vApp)) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object vApp. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            sLogMessage = sLogMessage & " " & vApp
        End If

        'UPGRADE_NOTE: IsMissing() was changed to IsNothing(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1021"'
        If Not (IsNothing(vClass)) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object vClass. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            sLogMessage = sLogMessage & " " & vClass
        End If

        'UPGRADE_NOTE: IsMissing() was changed to IsNothing(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1021"'
        If Not (IsNothing(vMethod)) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object vMethod. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            sLogMessage = sLogMessage & "." & vMethod
        End If

        ' Add the message to the end
        sLogMessage = sLogMessage & " " & sMsg

        ' Add VB Error number and description if we have them

        'UPGRADE_NOTE: IsMissing() was changed to IsNothing(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1021"'
        If Not (IsNothing(vErrNo)) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object vErrNo. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            sLogMessage = sLogMessage & vErrNo
        End If

        'UPGRADE_NOTE: IsMissing() was changed to IsNothing(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1021"'
        If Not (IsNothing(vErrDesc)) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object vErrDesc. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            sLogMessage = sLogMessage & " " & vErrDesc
        End If

        ' Print the Log Message to the Log File
        PrintLine(iFileNumber, sLogMessage)

        ' Close the log file
        FileClose(iFileNumber)

        Exit Sub

        Catch ex As Exception

        ' Log This Error.
        LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogFatal, sMsg:="Failed to log the following error in Log File : " & sLogFile, vApp:=ACApp, vClass:=ACClass, vMethod:="LogMessageToFile", excep:=ex)

        ' We have failed to Log the message in the log file
        ' so display the error we were trying to log.

        ' Log Error.
        LogMessagePopup(iType:=iType, sMsg:=sMsg, vApp:=vApp, vClass:=vClass, vMethod:=vMethod, excep:=ex)

        Exit Sub

        End Try
    End Sub

    ' ***************************************************************** '
    '
    ' Name: InsertUserName
    '
    ' Description:
    '
    ' History: 13/01/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function InsertUserName(ByRef r_sLogFile As String, ByVal v_sUserName As String) As Integer

        Dim sLogFile As String
        Dim lSub As Integer
        Dim sPrefix As String
        Dim sSuffix As String


        Try

        InsertUserName = gPMConstants.PMEReturnCode.PMTrue

        sLogFile = r_sLogFile

        For lSub = Len(sLogFile) To 1 Step -1
            If Mid(sLogFile, lSub, 1) = "\" Then
                Exit For
            End If
        Next

        If lSub > 1 Then
            sPrefix = Left(sLogFile, lSub)
            sSuffix = Right(sLogFile, Len(sLogFile) - lSub)
            sLogFile = v_sUserName & "_" & sSuffix
            For lSub = 1 To Len(sLogFile)
                If Mid(sLogFile, lSub, 1) = " " Then
                    Mid(sLogFile, lSub, 1) = "_"
                End If
            Next
            sLogFile = sPrefix & sLogFile
        End If

        r_sLogFile = sLogFile

        Exit Function

        Catch ex As Exception

        InsertUserName = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InsertUserName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InsertUserName", excep:=ex)

        Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LogMessagePopup
    '
    ' Description: Logs a message into the PM Log File.
    '
    ' ***************************************************************** '
    Public Sub LogMessagePopup(ByRef iType As Short, ByRef sMsg As String, ByRef vApp As Object, ByRef vClass As Object, ByRef vMethod As Object, ByRef excep As Exception )

        Dim sLogMessage As String
        Dim iMsgboxParms As Short
        Dim iTypeOfMessage As Short
        Dim sMessageTypeText As String

        Try

        ' Set Message Type and description

        ' RDC 12062002 classes changed to BAS modules
        '    Select Case iType%
        '      Case PMLogFatal
        '        iTypeOfMessage = vbCritical
        '         sMessageTypeText = m_oConst.PMFatalText
        '        sMessageTypeText = m_oConst.PMFatalText
        '      Case PMLogError
        '        iTypeOfMessage = vbCritical
        '        sMessageTypeText = m_oConst.PMErrorText
        '      Case PMLogWarning
        '        iTypeOfMessage = vbExclamation
        '        sMessageTypeText = m_oConst.PMWarningText
        '      Case PMLogInfo
        '        iTypeOfMessage = vbExclamation
        '        sMessageTypeText = m_oConst.PMInfoText
        '      Case PMLogOnError
        '        iTypeOfMessage = vbExclamation
        '        sMessageTypeText = m_oConst.PMOnErrorText
        '      Case PMLogDebug1
        '        iTypeOfMessage = vbInformation
        '        sMessageTypeText = m_oConst.PMDebug1Text
        '      Case PMLogDebug2
        '        iTypeOfMessage = vbInformation
        '        sMessageTypeText = m_oConst.PMDebug2Text
        '      Case PMLogDebug3
        '        iTypeOfMessage = vbInformation
        '        sMessageTypeText = m_oConst.PMDebug3Text
        '      Case Else
        '        iTypeOfMessage = vbInformation
        '        sMessageTypeText = m_oConst.PMDebug4Text
        '    End Select

        Select Case iType
            Case gPMConstants.PMELogLevel.PMLogFatal
                iTypeOfMessage = MsgBoxStyle.Critical
                sMessageTypeText = PMFatalText
                sMessageTypeText = PMFatalText
            Case gPMConstants.PMELogLevel.PMLogError
                iTypeOfMessage = MsgBoxStyle.Critical
                sMessageTypeText = PMErrorText
            Case gPMConstants.PMELogLevel.PMLogWarning
                iTypeOfMessage = MsgBoxStyle.Exclamation
                sMessageTypeText = PMWarningText
            Case gPMConstants.PMELogLevel.PMLogInfo
                iTypeOfMessage = MsgBoxStyle.Exclamation
                sMessageTypeText = PMInfoText
            Case gPMConstants.PMELogLevel.PMLogOnError
                iTypeOfMessage = MsgBoxStyle.Exclamation
                sMessageTypeText = PMOnErrorText
            Case gPMConstants.PMELogLevel.PMLogDebug1
                iTypeOfMessage = MsgBoxStyle.Information
                sMessageTypeText = PMDebug1Text
            Case gPMConstants.PMELogLevel.PMLogDebug2
                iTypeOfMessage = MsgBoxStyle.Information
                sMessageTypeText = PMDebug2Text
            Case gPMConstants.PMELogLevel.PMLogDebug3
                iTypeOfMessage = MsgBoxStyle.Information
                sMessageTypeText = PMDebug3Text
            Case Else
                iTypeOfMessage = MsgBoxStyle.Information
                sMessageTypeText = PMDebug4Text
        End Select

        ' Msgbox Parameters are
        ' OK Button Only, Type of Message, Button1 is defualt, Application Modal
        iMsgboxParms = MsgBoxStyle.OKOnly + iTypeOfMessage + MsgBoxStyle.DefaultButton1 + MsgBoxStyle.ApplicationModal

        ' Current date/time
        sLogMessage = "Date/Time: " & Now & Chr(13) & Chr(10)

        ' Display optional parameters if they are present
        'UPGRADE_NOTE: IsMissing() was changed to IsNothing(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1021"'
        If Not (IsNothing(vApp)) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object vApp. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            sLogMessage = sLogMessage & "Application: " & vApp & Chr(13) & Chr(10)
        End If

        'UPGRADE_NOTE: IsMissing() was changed to IsNothing(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1021"'
        If Not (IsNothing(vClass)) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object vClass. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            sLogMessage = sLogMessage & "Class: " & vClass & Chr(13) & Chr(10)
        End If

        'UPGRADE_NOTE: IsMissing() was changed to IsNothing(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1021"'
        If Not (IsNothing(vMethod)) Then
            'UPGRADE_WARNING: Couldn't resolve default property of object vMethod. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            sLogMessage = sLogMessage & "Method: " & vMethod & Chr(13) & Chr(10)
        End If

        ' Add the message to the end
        sLogMessage = sLogMessage & "Message Text: " & sMsg & Chr(13) & Chr(10)

        If excep IsNot Nothing AndAlso excep.Message IsNot Nothing Then
            sLogMessage = sLogMessage & Environment.NewLine & "Ex Message : " & excep.Message
        End If

        ' Display the Log Message on the screen
        MsgBox(sLogMessage, iMsgboxParms, sMessageTypeText & " Message")

        Exit Sub

        Catch ex As Exception

        ' Error Section.
        Exit Sub

        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: GetRegSettings
    '
    ' Description: Get settings from the registry.
    '
    ' ***************************************************************** '
    Public Function GetRegSettings(ByRef sResult As String, ByRef sAppName As String, ByRef sSection As String, ByRef sKey As String, Optional ByRef vDefault As Object = Nothing) As Integer

        Try

        GetRegSettings = gPMConstants.PMEReturnCode.PMTrue

        ' Check if we have the optional parameter.
        'UPGRADE_NOTE: IsMissing() was changed to IsNothing(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1021"'
        If (IsNothing(vDefault) = True) Then
            ' Get setting from the registry not
            ' using the optional parameter.
            sResult = GetSetting(sAppName, sSection, sKey)
        Else
            ' Get setting from the registry
            ' using the optional parameter.
            'UPGRADE_WARNING: Couldn't resolve default property of object vDefault. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
            sResult = GetSetting(sAppName, sSection, sKey, vDefault)
        End If

        Exit Function

        Catch ex As Exception

        ' Error Section.

        GetRegSettings = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the registry settings", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRegSettings", excep:=ex)

        Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetRegAllSettings
    '
    ' Description: Get all section settings from the registry.
    '
    ' ***************************************************************** '
    Public Function GetRegAllSettings(ByRef vResult As Object, ByRef sAppName As String, ByRef sSection As String) As Integer

        Try

        GetRegAllSettings = gPMConstants.PMEReturnCode.PMTrue

        ' Get section setting from the registry.
        'UPGRADE_WARNING: Couldn't resolve default property of object vResult. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
        vResult = GetAllSettings(sAppName, sSection)

        Exit Function

        Catch ex As Exception

        ' Error Section.

        GetRegAllSettings = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the registry settings", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRegAllSettings", excep:=ex)

        Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SaveRegSettings
    '
    ' Description: Save settings to the registry.
    '
    ' ***************************************************************** '
    Public Function SaveRegSettings(ByRef sSetting As String, ByRef sAppName As String, ByRef sSection As String, ByRef sKey As String) As Integer

        Try

        SaveRegSettings = gPMConstants.PMEReturnCode.PMTrue

        ' Save setting to the registry.
        SaveSetting(sAppName, sSection, sKey, sSetting)

        Exit Function

        Catch ex As Exception

        ' Error Section.

        SaveRegSettings = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save the registry settings", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveRegSettings", excep:=ex)

        Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteRegSettings
    '
    ' Description: Delete settings from the registry.
    '
    ' ***************************************************************** '
    Public Function DeleteRegSettings(ByRef sAppName As String, ByRef sSection As String, ByRef sKey As String) As Integer

        Try

        DeleteRegSettings = gPMConstants.PMEReturnCode.PMTrue

        ' Delete setting from the registry.
        DeleteSetting(sAppName, sSection, sKey)

        Exit Function

        Catch ex As Exception

        ' Error Section.

        DeleteRegSettings = gPMConstants.PMEReturnCode.PMError

        ' Log Error.
        LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete the registry settings", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRegSettings", excep:=ex)

        Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ConvertWildCardsForSQL (Public)
    '
    ' Description: Converts '*' wildcards to '%' for SQL
    '
    ' Edit History TF311097 Created
    ' ***************************************************************** '
    Public Function ConvertWildCardsForSQL(ByRef r_sTextString As String) As Integer

        Dim sSearchText As String

        Try

        sSearchText = r_sTextString

        ' Replace * with % wildcards
        While ((InStr(sSearchText, "*") > 0) = True)
            Mid(sSearchText, InStr(sSearchText, "*"), 1) = "%"
        End While

        ' Add implied wildcard to end
        If Right(sSearchText, 1) <> "%" Then
            sSearchText = sSearchText & "%"
        End If

        r_sTextString = sSearchText


        Exit Function

        Catch ex As Exception

        ' Error Section.
        ConvertWildCardsForSQL = gPMConstants.PMELogLevel.PMLogOnError

        ' Log Error.
        LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Convert Wild Cards For SQL", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertWildCardsForSQL", excep:=ex)

        Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ArchitectureInDebug
    '
    ' Description: Returns True if the Architecure is runnind in
    '              Debug Mode, False otherwise.
    '
    ' ***************************************************************** '
    Public Function ArchitectureInDebug() As Boolean

        Dim sInDebug As String = String.Empty
        Dim lErrorValue As Integer

        Try

        ArchitectureInDebug = False

        '    lErrorValue& = GetRegSettings( _
        ''        sResult:=sInDebug, _
        ''        sAppName:=m_oConst.PMRegAppName, _
        ''        sSection:=m_oConst.PMRegSecSystem, _
        ''        sKey:=m_oConst.PMRegKeyArchitectureInDebug)

        ' Get the ArchitectureInDebug setting from
        ' HKEY_LOCAL_MACHINE\software\PM\SiriusArchitecture\Common
        ' RDC 12062002 classes changed to BAS modules
        '    lErrorValue& = GetPMRegSetting( _
        'v_lPMERegSettingRoot:=pmeRSRLocalMachine, _
        'v_lPMEProductFamily:=pmePFSiriusArchitecture, _
        'v_lPMERegSettingLevel:=pmeRSLCommon, _
        'v_sSettingName:=m_oConst.PMRegKeyArchitectureInDebug, _
        'r_sSettingValue:=sInDebug)

        lErrorValue = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=PMRegKeyArchitectureInDebug, r_sSettingValue:=sInDebug)

        ' Check for errors.
        If ((lErrorValue <> gPMConstants.PMEReturnCode.PMTrue) Or (Trim(sInDebug) = "")) Then
            ' Return False if Errors
            ArchitectureInDebug = False
            Exit Function
        End If

        ' Return result YES, Y or PMTrue are Yes
        Select Case UCase(Trim(sInDebug))
            Case "YES"
                ArchitectureInDebug = True
            Case "Y"
                ArchitectureInDebug = True
            Case CStr(gPMConstants.PMEReturnCode.PMTrue)
                ArchitectureInDebug = True
            Case Else
                ArchitectureInDebug = False
        End Select

        Exit Function

        Catch ex As Exception

        ArchitectureInDebug = False
        Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetPMRegSetting
    '
    ' Description: Sets the value for the specified setting in the
    '              Registry at the specified location.
    '
    ' RegSettingRoot  = Local Machine Or Current User
    ' Software\PM     = Fixed location
    ' ProductFamily   = SiriusArchitecture Or Gemini Or Mercury etc
    ' RegSettinglevel = Client OR Server Or Common
    '
    ' e.g. HKEY_CURRENT_USER\software\PM\SiriusArchitecture\Client
    ' ***************************************************************** '
    Public Function SetPMRegSetting(ByVal v_lPMERegSettingRoot As Integer, ByVal v_lPMEProductFamily As Integer, ByVal v_lPMERegSettingLevel As Integer, ByVal v_sSettingName As String, ByVal v_sSettingValue As String, Optional ByVal v_sSubKey As String = "") As Integer

        Dim bKeyExists As Boolean
        Dim sKeyString As String
        Dim lRoot As Integer

        Try

        SetPMRegSetting = gPMConstants.PMEReturnCode.PMTrue

        ' Current User OR Local Machine
        If (v_lPMERegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser) Then
            lRoot = HKEY_CURRENT_USER
        Else
            lRoot = HKEY_LOCAL_MACHINE
        End If

        ' Build up the key String
        sKeyString = BuildKeyString(v_ePMEProductFamily:=v_lPMEProductFamily, v_ePMERegSettingLevel:=v_lPMERegSettingLevel, v_sSubKey:=v_sSubKey)

        ' Do we have a key string
        If (Trim(sKeyString) = "") Then
            SetPMRegSetting = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        ' Does this key exist
        bKeyExists = KeyExists(lRoot, sKeyString)
        If (bKeyExists = False) Then
            ' No it doesn't exist, so create it
            Call CreateNewKey(lRoot, sKeyString)
        End If

        ' Save the Value
        Call SetKeyValue(lRoot, sKeyString, v_sSettingName, v_sSettingValue, REG_SZ)

        Exit Function

        Catch ex As Exception

        SetPMRegSetting = gPMConstants.PMEReturnCode.PMError

        Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPMRegSetting
    '
    ' Description: Gets the value for the specified setting in the
    '              Registry at the specified location.
    '
    ' RegSettingRoot  = Local Machine Or Current User
    ' Software\PM     = Fixed location
    ' ProductFamily   = SiriusArchitecture Or Gemini Or Mercury etc
    ' RegSettinglevel = Client OR Server Or Common
    '
    ' e.g. HKEY_CURRENT_USER\software\PM\SiriusArchitecture\Client
    '
    ' ***************************************************************** '
    Public Function GetPMRegSetting(ByVal v_lPMERegSettingRoot As Integer, ByVal v_lPMEProductFamily As Integer, ByVal v_lPMERegSettingLevel As Integer, ByVal v_sSettingName As String, ByRef r_sSettingValue As String, Optional ByVal v_sSubKey As String = "") As Integer 
    
        Dim bKeyExists As Boolean        
        Dim sKeyString As String
        Dim lRoot As Integer
        Dim vSettingValue As Object

        Try

        GetPMRegSetting = gPMConstants.PMEReturnCode.PMTrue

        ' Current User OR Local Machine
        If (v_lPMERegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser) Then
            lRoot = HKEY_CURRENT_USER
        Else
            lRoot = HKEY_LOCAL_MACHINE
        End If

        ' Build up the key String
        sKeyString = BuildKeyString(v_ePMEProductFamily:=v_lPMEProductFamily, v_ePMERegSettingLevel:=v_lPMERegSettingLevel, v_sSubKey:=v_sSubKey)

        ' Do we have a key string
        If (Trim(sKeyString) = "") Then
            GetPMRegSetting = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        ' Get the Value
        'UPGRADE_WARNING: Couldn't resolve default property of object QueryKeyValue(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
        'UPGRADE_WARNING: Couldn't resolve default property of object vSettingValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
        vSettingValue = QueryKeyValue(lRoot, sKeyString, v_sSettingName)


            'UPGRADE_WARNING: IsEmpty was upgraded to IsNothing and has a new behavior. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1041"'
            If (IsNothing(vSettingValue) = True) Then
                ' Return an Empty String
                r_sSettingValue = ""
            Else
                ' Otherwise, Return the Setting Value
                'UPGRADE_WARNING: Couldn't resolve default property of object vSettingValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
                r_sSettingValue = Trim(CStr(vSettingValue))
            End If

            Exit Function

        Catch ex As Exception

        GetPMRegSetting = gPMConstants.PMEReturnCode.PMError

        Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: FlipArray
    '
    ' Description: Swap columns and rows in a 2 dimensional array
    '
    ' History: 11/05/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function FlipArray(ByRef r_vArray(,) As Object) As Integer

        Dim vArray(,) As Object
        Dim lRow As Integer
        Dim lColumn As Integer
        Dim lDimension As Integer

        On Error Resume Next

        FlipArray = gPMConstants.PMEReturnCode.PMTrue

        ' Check if parameter realy is an array
        If IsArray(r_vArray) = False Then
            FlipArray = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        ' Ensure array has at least 2 dimensions
        Err.Clear()
        lDimension = UBound(r_vArray, 2)
        If Err.Number <> 0 Then
            FlipArray = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        ' Ensure array has no more than 2 dimensions
        lDimension = UBound(r_vArray, 3)
        If Err.Number = 0 Then
            FlipArray = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        On Error GoTo Err_FlipArray
        ' Dimension the output array
        ReDim vArray(UBound(r_vArray, 2), UBound(r_vArray, 1))

        ' Flip the array
        For lRow = LBound(r_vArray, 2) To UBound(r_vArray, 2)
            For lColumn = LBound(r_vArray, 1) To UBound(r_vArray, 1)
                
                vArray(lRow, lColumn) = r_vArray(lColumn, lRow)
            Next lColumn
        Next lRow

        ' Return the flipped array


        r_vArray = vArray

        Exit Function

Err_FlipArray:

        FlipArray = gPMConstants.PMEReturnCode.PMError

        Exit Function

    End Function

    ' ***************************************************************** '
    ' Private Methods
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Name: BuildKeyString (Private)
    '
    ' Description: Builds up the Key String for the Reg Setting
    ' ***************************************************************** '
    Private Function BuildKeyString(ByVal v_ePMEProductFamily As gPMConstants.PMEProductFamily, ByVal v_ePMERegSettingLevel As gPMConstants.PMERegSettingLevel, Optional ByVal v_sSubKey As String = "") As String

        Dim sKeyString As String

        Try
            ' Build up the key String

            ' Start with Root
            sKeyString = ACRegRoot

            ' Add PM Product
            Select Case v_ePMEProductFamily
                Case gPMConstants.PMEProductFamily.pmePFSiriusArchitecture
                    sKeyString = sKeyString & ACRegSiriusArchitecture
                Case gPMConstants.PMEProductFamily.pmePFSiriusUnderwriting
                    sKeyString = sKeyString & ACRegSiriusUnderwriting
                Case gPMConstants.PMEProductFamily.pmePFOrion
                    sKeyString = sKeyString & ACRegOrion
                Case gPMConstants.PMEProductFamily.pmePFGemini
                    sKeyString = sKeyString & ACRegGemini
                Case gPMConstants.PMEProductFamily.pmePFVoyager
                    sKeyString = sKeyString & ACRegVoyager
                Case gPMConstants.PMEProductFamily.pmePFMercury
                    sKeyString = sKeyString & ACRegMercury
                Case gPMConstants.PMEProductFamily.pmePFDocumaster
                    sKeyString = sKeyString & ACRegDocumaster
                Case gPMConstants.PMEProductFamily.pmePFSiriusBroking
                    sKeyString = sKeyString & ACRegSiriusBroking
                    'RFC251198 - Added SiriusSolutions & Nirvana Registry Constants
                Case gPMConstants.PMEProductFamily.pmePFSiriusSolutions
                    sKeyString = sKeyString & ACRegSiriusSolutions
                Case gPMConstants.PMEProductFamily.pmePFNirvana
                    sKeyString = sKeyString & ACRegNirvana
                    'RFC060799 - Added GeminiII Product Family, DSN etc etc
                Case gPMConstants.PMEProductFamily.pmePFGeminiII
                    sKeyString = sKeyString & ACRegGeminiII
                    ' RDC 07082000 - new product family: Claims
                Case gPMConstants.PMEProductFamily.pmePFClaims
                    sKeyString = sKeyString & ACRegClaims
            End Select

            ' Add Level
            Select Case v_ePMERegSettingLevel
                Case gPMConstants.PMERegSettingLevel.pmeRSLClient
                    sKeyString = sKeyString & ACRegClient
                Case gPMConstants.PMERegSettingLevel.pmeRSLServer
                    sKeyString = sKeyString & ACRegServer
                Case gPMConstants.PMERegSettingLevel.pmeRSLCommon
                    sKeyString = sKeyString & ACRegCommon
                Case gPMConstants.PMERegSettingLevel.pmeRSLSetup
                    sKeyString = sKeyString & ACRegSetup
                Case Else
                    sKeyString = sKeyString & ACRegCommon
            End Select

            ' Has a Sub key been supplied
            If (v_sSubKey <> "") Then

                ' Yes we have a sub key

                ' Add a separator if the Start of the sub key does not have one
                If (Mid(v_sSubKey, 1, 1) <> "\") Then
                    sKeyString = sKeyString & "\"
                End If

                ' Add the Sub Key
                sKeyString = sKeyString & v_sSubKey

                ' Remove a Trailing separator if there is one
                If (Mid(sKeyString, Len(sKeyString), 1) = "\") Then
                    sKeyString = Mid(sKeyString, 1, Len(sKeyString) - 1)
                End If

            End If

            ' Return the string
            BuildKeyString = sKeyString

        Catch ex As Exception
            BuildKeyString = ""
        End Try

    End Function

    ' ***************************************************************** '
    '
    ' Name: TrimDate
    '
    ' Description: Assuming normal US format long date, drop day from
    '              start of input
    '
    ' History: 23/06/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Private Function TrimDate(ByRef r_vDate As Object) As Integer

        Dim lSub As Integer
        Dim sDate As String

        Try
            TrimDate = gPMConstants.PMEReturnCode.PMTrue

            sDate = CStr(r_vDate)
            For lSub = 1 To Len(sDate)
                If Mid(sDate, lSub, 1) = " " Then
                    lSub = lSub + 1
                    Exit For
                End If
            Next lSub

            If lSub < Len(sDate) Then
                r_vDate = Mid(sDate, lSub, Len(sDate) - lSub)
            End If

        Catch ex As Exception
            TrimDate = gPMConstants.PMEReturnCode.PMError
        End Try

    End Function


    ' ***************************************************************** '
    ' Name: FormatName
    '
    ' Description: format string into upper/lower-case, for real names
    '
    ' ***************************************************************** '
    Private Function FormatName(ByVal vString As Object) As Object

        Dim iLoop As Short
        Dim sLast As String
        Dim sChar As String
        Dim sMac As String
        Dim sSearch As String
        Dim vTemp As Object

        Try

            FormatName = ""

            ' significant characters
            sSearch = " -'(."

            ' copy of input


            vTemp = Trim(vString)

            ' first char is always upper case

            Mid(vTemp, 1, 1) = UCase(Mid(vTemp, 1, 1))

            ' other characters
            For iLoop = 2 To Len(vTemp)

                ' previous and current characters

                sLast = Mid(vTemp, iLoop - 1, 1)

                sChar = Mid(vTemp, iLoop, 1)

                ' previous char in search string?
                If InStr(1, sSearch, sLast) > 0 Then
                    sChar = UCase(sChar)
                End If

                ' check for Mc
                If iLoop > 3 Then

                    sMac = UCase(Mid(vTemp, iLoop - 3, 3))
                    ' any of the significant characters + Mc?
                    If InStr(1, sSearch, Left(sMac, 1)) > 0 And UCase(Mid(sMac, 2, 2)) = "MC" Then
                        sChar = UCase(sChar)
                    End If
                End If

                ' check for Mac
                If iLoop > 4 Then
                    sMac = UCase(Mid(vTemp, iLoop - 4, 4))
                    ' any of the significant characters + Mac?
                    If InStr(1, sSearch, Left(sMac, 1)) > 0 And UCase(Mid(sMac, 2, 3)) = "MAC" Then
                        sChar = UCase(sChar)
                    End If
                End If

                ' replace character

                Mid(vTemp, iLoop, 1) = sChar

            Next

            ' finally ...

            ' Mc at start of string?

            If UCase(Left(vTemp, 2)) = "MC" Then

                Mid(vTemp, 3, 1) = UCase(Mid(vTemp, 3, 1))
            End If

            ' Mac at start of string?

            If UCase(Left(vTemp, 3)) = "MAC" Then

                Mid(vTemp, 4, 1) = UCase(Mid(vTemp, 4, 1))
            End If

            FormatName = vTemp

        Catch ex As Exception
            FormatName = ""
        End Try

    End Function


    ' ***************************************************************** '
    '
    ' Name: replaceVbcrWithVbcrlf
    '
    ' Description: As name. Normal VB replace does not work for this
    '
    ' History: 17/08/2001 CLG - Created.
    '
    ' ***************************************************************** '
    Private Sub replaceVbcrWithVbcrlf(ByRef sString As String)
        Try

            Dim iLoop As Short

            'if already converted then do nothing
            If InStr(sString, vbCrLf) Then Exit Sub

            iLoop = InStr(sString, vbLf)

            Do While iLoop > 0
                sString = Left(sString, iLoop - 1) & vbCrLf & Mid(sString, iLoop + 1)
                iLoop = InStr(iLoop + 2, sString, vbLf)
            Loop

        Catch ex As Exception

        End Try

    End Sub

    ' RDC 13062002 declarations moved from RegistryFunctions.bas ######################################## START
    Function SetValueEx(ByVal hKey As Integer, ByRef sValueName As String, ByRef lType As Integer, ByRef vValue As Object) As Integer

        Dim lValue As Integer
        Dim sValue As String

        Select Case lType
            Case REG_SZ

                sValue = vValue & Chr(0)
                SetValueEx = RegSetValueExString(hKey, sValueName, 0, lType, sValue, Len(sValue))
            Case REG_DWORD
                lValue = vValue
                SetValueEx = RegSetValueExLong(hKey, sValueName, 0, lType, lValue, 4)
        End Select

    End Function

    Function QueryValueEx(ByVal lhKey As Integer, ByVal szValueName As String, ByRef vValue As Object) As Integer

        Dim cch As Integer
        Dim lrc As Integer
        Dim lType As Integer
        Dim lValue As Integer
        Dim sValue As String

        On Error GoTo QueryValueExError

        ' Determine the size and type of data to be read
        lrc = RegQueryValueExNULL(lhKey, szValueName, 0, lType, 0, cch)
        If lrc <> ERROR_NONE Then Error (5)

        Select Case lType
            ' For strings
        Case REG_SZ
                sValue = New String(Chr(0), cch)
                lrc = RegQueryValueExString(lhKey, szValueName, 0, lType, sValue, cch)

                If lrc = ERROR_NONE Then

                    vValue = Left(sValue, cch - 1)
                Else

                    vValue = Nothing
                End If

                ' For DWORDS
            Case REG_DWORD
                lrc = RegQueryValueExLong(lhKey, szValueName, 0, lType, lValue, cch)


                If lrc = ERROR_NONE Then vValue = lValue

            Case Else
                'all other data types not supported
                lrc = -1
        End Select

QueryValueExExit:
        QueryValueEx = lrc
        Exit Function
QueryValueExError:
        Resume QueryValueExExit
    End Function

    Sub CreateNewKey(ByRef lPredefinedKey As Integer, ByRef sNewKeyName As String)

        Dim hNewKey As Integer 'handle to the new key
        Dim lRetVal As Integer 'result of the RegCreateKeyEx function

        lRetVal = RegCreateKeyEx(lPredefinedKey, sNewKeyName, 0, vbNullString, REG_OPTION_NON_VOLATILE, KEY_ALL_ACCESS, 0, hNewKey, lRetVal)
        RegCloseKey(hNewKey)
    End Sub

    Sub SetKeyValue(ByRef lPredefinedKey As Integer, ByRef sKeyName As String, ByRef sValueName As String, ByRef vValueSetting As Object, ByRef lValueType As Integer)

        Dim lRetVal As Integer 'result of the SetValueEx function
        Dim hKey As Integer 'handle of open key

        'open the specified key
        lRetVal = RegOpenKeyEx(lPredefinedKey, sKeyName, 0, KEY_ALL_ACCESS, hKey)
        lRetVal = SetValueEx(hKey, sValueName, lValueType, vValueSetting)
        RegCloseKey(hKey)
    End Sub

    Function QueryKeyValue(ByRef lPredefinedKey As Integer, ByRef sKeyName As String, ByRef sValueName As String) As Object

        Dim lRetVal As Integer 'result of the API functions
        Dim hKey As Integer 'handle of opened key
        Dim vValue As Object = Nothing 'setting of queried value

        lRetVal = RegOpenKeyEx(lPredefinedKey, sKeyName, 0, KEY_ALL_ACCESS, hKey)
        lRetVal = QueryValueEx(hKey, sValueName, vValue)
        'UPGRADE_WARNING: Couldn't resolve default property of object vValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
        'UPGRADE_WARNING: Couldn't resolve default property of object QueryKeyValue. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
        QueryKeyValue = vValue
        RegCloseKey(hKey)
    End Function

    Function KeyExists(ByRef lPredefinedKey As Integer, ByRef sKeyName As String) As Boolean

        Dim lRetVal As Integer 'result of the API functions
        Dim hKey As Integer 'handle of opened key

        KeyExists = False
        lRetVal = RegOpenKeyEx(lPredefinedKey, sKeyName, 0, KEY_ALL_ACCESS, hKey)
        If lRetVal = ERROR_NONE Then
            KeyExists = True
        Else
            KeyExists = False
        End If
        RegCloseKey(hKey)
    End Function

    Function DeleteKey(ByRef lPredefinedKey As Integer, ByRef sKeyName As String) As Integer

        Dim lRetVal As Integer

        lRetVal = RegDeleteKey(lPredefinedKey, sKeyName)
        If lRetVal = ERROR_NONE Then
            DeleteKey = True
        Else
            DeleteKey = False
        End If

    End Function

    Function DeleteKeyValue(ByRef lPredefinedKey As Integer, ByRef sKeyName As String, ByRef sValueName As String) As Integer

        Dim lRetVal As Integer
        Dim hKey As Integer

        lRetVal = RegOpenKeyEx(lPredefinedKey, sKeyName, 0, KEY_ALL_ACCESS, hKey)

        lRetVal = RegDeleteValue(hKey, sValueName)
        If lRetVal = ERROR_NONE Then
            DeleteKeyValue = True
        Else
            DeleteKeyValue = False
        End If

    End Function
    ' ################################################################################################### START

    ' RDC 11072002
    ' ***************************************************************** '
    ' Name: GetNTUserName
    '
    ' Description: get the NT account name of the currently logged on user
    '
    ' ***************************************************************** '
    Public Function GetNTUsername(ByRef sNTUsername As String) As Integer

        Dim lStatus As Short
        Dim sName As String = String.Empty
        Dim sUsername As String = String.Empty

        Const BUFFER_LENGTH As Short = 255
        Const STATUS_NOERROR As Short = 0

        Try

        GetNTUsername = gPMConstants.PMEReturnCode.PMFalse

        sNTUsername = ""

        sUsername = Space(BUFFER_LENGTH + 1)

        lStatus = WNetGetUser(sName, sUsername, BUFFER_LENGTH)

        If lStatus <> STATUS_NOERROR Then
            Exit Function
        End If

        sNTUsername = Left(sUsername, InStr(sUsername, Chr(0)) - 1)

        GetNTUsername = gPMConstants.PMEReturnCode.PMTrue

        Exit Function

        Catch ex As Exception

        GetNTUsername = gPMConstants.PMEReturnCode.PMError

        End Try
    End Function

    ' RDC 12072002
    ' ***************************************************************** '
    ' Name: GetNTUserNameEx
    '
    ' Description:  get the fully qualified NT account name
    '               of the currently logged on user
    '
    ' ***************************************************************** '
    Public Function GetNTUsernameEx(ByRef sUsername As Object) As Integer

        Dim lLen As Integer
        Dim lRet As Integer
        Dim sName As String

        Const BUFFER_LENGTH As Short = 255
        Const RETURN_ERROR As Short = 0

        Try

        GetNTUsernameEx = gPMConstants.PMEReturnCode.PMFalse

        sName = Space(BUFFER_LENGTH + 1)
        lLen = Len(sName)

        lRet = GetUserNameEx(EXTENDED_NAME_FORMAT.NameSamCompatible, sName, lLen)

        If lRet = RETURN_ERROR Then
            Exit Function
        End If

        'UPGRADE_WARNING: Couldn't resolve default property of object sUsername. Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
        sUsername = Left(sName, lLen - 1)

        GetNTUsernameEx = gPMConstants.PMEReturnCode.PMTrue

        Exit Function

        Catch ex As Exception

        GetNTUsernameEx = gPMConstants.PMEReturnCode.PMError

        End Try
    End Function

    ' RDC 12072002
    ' ***************************************************************** '
    ' Name: GetSystemInfo
    '
    ' Description:  get system version info
    '
    ' ***************************************************************** '
    Public Function GetSystemInfo(ByRef vSysInfo As Object) As Integer

        Dim lRet As Integer
        Dim OSInfo As OSVERSIONINFO = Nothing
        Dim PId As String = String.Empty
        Dim sInfo As String

        Try

        GetSystemInfo = gPMConstants.PMEReturnCode.PMFalse

        OSInfo.dwOSVersionInfoSize = Len(OSInfo)

        'Get the Windows version
        lRet = GetVersionEx(OSInfo)

        'Check for errors
        If lRet = 0 Then
            Exit Function
        End If

        ReDim vSysInfo(1, 9)

        'UPGRADE_WARNING: Couldn't resolve default property of object vSysInfo(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
        vSysInfo(0, 0) = "PlatformName"
        'UPGRADE_WARNING: Couldn't resolve default property of object vSysInfo(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
        vSysInfo(1, 0) = "MajorVersion"
        'UPGRADE_WARNING: Couldn't resolve default property of object vSysInfo(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
        vSysInfo(2, 0) = "MinorVersion"
        'UPGRADE_WARNING: Couldn't resolve default property of object vSysInfo(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
        vSysInfo(3, 0) = "BuildNumber"

        Select Case OSInfo.dwPlatformId
            Case 0
                PId = "Windows 32s"
            Case 1
                PId = "Windows 95/98"
            Case 2
                PId = "Windows NT"
        End Select

        'UPGRADE_WARNING: Couldn't resolve default property of object vSysInfo(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
        vSysInfo(0, 1) = PId
        'UPGRADE_WARNING: Couldn't resolve default property of object vSysInfo(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
        vSysInfo(1, 1) = OSInfo.dwMajorVersion
        'UPGRADE_WARNING: Couldn't resolve default property of object vSysInfo(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
        vSysInfo(2, 1) = OSInfo.dwMinorVersion
        'UPGRADE_WARNING: Couldn't resolve default property of object vSysInfo(). Click for more: 'ms-help://MS.VSCC/commoner/redir/redirect.htm?keyword="vbup1037"'
        vSysInfo(3, 1) = OSInfo.dwBuildNumber

        GetSystemInfo = gPMConstants.PMEReturnCode.PMTrue

        Exit Function

        Catch ex As Exception

        GetSystemInfo = gPMConstants.PMEReturnCode.PMError

        End Try
    End Function
End Module