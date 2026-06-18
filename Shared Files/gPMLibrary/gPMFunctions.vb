Option Strict Off
Option Explicit On
Imports System.DirectoryServices
Imports System.DirectoryServices.ActiveDirectory
Imports System.Globalization
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Windows.Forms
Imports Artinsoft.VB6.Utils
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports Microsoft.Practices.EnterpriseLibrary.Logging.Filters
Imports Microsoft.Practices.EnterpriseLibrary.Logging.Formatters
Imports Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
Imports Microsoft.VisualBasic.Compatibility.VB6

<System.Runtime.InteropServices.ProgId("gPMFunctions_NET.gPMFunctions")>
Public Module gPMFunctions

    Private m_lReturn As Integer
    Private m_lErrNum As Integer
    Private m_sErrSource As String = ""
    Private m_sErrDesc As String = ""
    Private m_lEvtLogHandle As Integer
    Private m_lCategory As Integer
    Private m_lNumMsgs As Integer
    Private m_lLenRawData As Integer
    Private m_typMessages As gPMConstants.FmtMsgArrayType = gPMConstants.FmtMsgArrayType.CreateInstance()
    Private m_abytDataBuffer() As Byte

    Private Const ACClass As String = "gPMFunctions"
    Private Const ACApp As String = "gPMFunctions"

    Public Property LogWriter() As LogWriter
        Get
            Return m_LogWriter
        End Get
        Private Set(value As LogWriter)
            m_LogWriter = value
        End Set
    End Property
    Private m_LogWriter As LogWriter

    Private Declare Function GetComputerName Lib "kernel32" Alias "GetComputerNameA" (ByVal lpBuffer As String, ByRef nSize As Integer) As Integer

    'Required to Map Window Handles from Container Objects to Embedded Objects
    'WIN32 API Calls from User32 Library
    Public Declare Function GetParent Lib "user32" (ByVal hwnd As Integer) As Integer
    Public Declare Function SetParent Lib "user32" (ByVal hWndChild As Integer, ByVal hWndNewParent As Integer) As Integer

    'Required to Overcome Any HardCoded Paths
    'WIN32API Calls from Kernel32 Library For Getting Path to System on Deployed Machine
    Public Declare Function GetSystemDirectory Lib "kernel32" Alias "GetSystemDirectoryA" (ByVal lpBuffer As String, ByVal nSize As Integer) As Integer
    Public Declare Function GetWindowsDirectory Lib "kernel32" Alias "GetWindowsDirectoryA" (ByVal lpBuffer As String, ByVal nSize As Integer) As Integer
    Public Declare Function GetUserDefaultLangID Lib "kernel32" () As Integer

    'Windows versioning resources
    <StructLayout(LayoutKind.Sequential, CharSet:=CharSet.Auto)> _
                                                                 _
    Private Structure OSVERSIONINFO
        Dim dwOSVersionInfoSize As Integer
        Dim dwMajorVersion As Integer
        Dim dwMinorVersion As Integer
        Dim dwBuildNumber As Integer
        Dim dwPlatformId As Integer
        <VBFixedString(128), System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.ByValTStr, SizeConst:=128)>
        Public szCSDVersion As FixedLengthString
        Public Shared Function CreateInstance() As OSVERSIONINFO
            Dim result As New OSVERSIONINFO
            result.szCSDVersion = New FixedLengthString(128)
            Return result
        End Function
    End Structure


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
        Public Function CompareFileTime(ByRef ft As FILETIME) As Integer
            Dim result As Integer = dwHighDateTime.CompareTo(ft.dwHighDateTime)

            If result = 0 Then
                result = dwLowDateTime.CompareTo(ft.dwLowDateTime)
            End If

            Return result
        End Function
    End Structure


    Declare Function RegEnumKeyEx Lib "advapi32.dll" Alias "RegEnumKeyExA" (ByVal hKey As Integer, ByVal dwIndex As Integer, ByVal lpName As String, ByRef lpcbName As Integer, ByRef lpReserved As Integer, ByVal lpClass As String, ByRef lpcbClass As Integer, ByRef lpftLastWriteTime As FILETIME) As Integer

    Declare Function RegEnumKey Lib "advapi32.dll" Alias "RegEnumKeyA" (ByVal hKey As Integer, ByVal dwIndex As Integer, ByVal lpName As String, ByVal cbName As Integer) As Integer

    Declare Function RegDeleteKey Lib "advapi32.dll" Alias "RegDeleteKeyA" (ByVal hKey As Integer, ByVal lpSubKey As String) As Integer

    Declare Function RegDeleteValue Lib "advapi32.dll" Alias "RegDeleteValueA" (ByVal hKey As Integer, ByVal lpValueName As String) As Integer

    Private Declare Function CloseEventLog Lib "advapi32" (ByVal hEventLog As Integer) As Integer

    Private Declare Function RegisterEventSource Lib "advapi32" Alias "RegisterEventSourceA" (ByVal lpUNCServerName As String, ByVal lpSourceName As String) As Integer

    Private Declare Function DeregisterEventSource Lib "advapi32" (ByVal hEventLog As Integer) As Integer

    Private Declare Function ReportEvent Lib "advapi32" Alias "ReportEventA" (ByVal hEventLog As Integer, ByVal wType As Integer, ByVal wCategory As Integer, ByVal dwEventID As Integer, ByVal lpUserSid As Integer, ByVal wNumStrings As Integer, ByVal dwDataSize As Integer, ByVal lpStrings As Integer, ByVal lpRawData As Integer) As Integer

    Private Declare Function IsValidSid Lib "advapi32.dll" (ByVal PSID As Integer) As Integer

    Private Declare Function GetLengthSid Lib "advapi32.dll" (ByVal PSID As Integer) As Integer

    Private Declare Sub CopyMem Lib "kernel32" Alias "RtlMoveMemory" (ByVal pTo As Integer, ByVal uFrom As Integer, ByVal lSize As Integer)

    Private Declare Function HeapAlloc Lib "kernel32" (ByVal hHeap As Integer, ByVal dwFlags As Integer, ByVal dwBytes As Integer) As Integer

    Private Declare Function HeapFree Lib "kernel32" (ByVal hHeap As Integer, ByVal dwFlags As Integer, ByVal lpMem As Integer) As Integer

    Private Declare Function GetProcessHeap Lib "kernel32" () As Integer

    Private Declare Function LoadLibraryEx Lib "kernel32" Alias "LoadLibraryExA" (ByVal lpLibFileName As String, ByVal hFile As Integer, ByVal dwFlags As Integer) As Integer

    Private Declare Function FreeLibrary Lib "kernel32" (ByVal hLibModule As Integer) As Integer

    Private Declare Function CloseHandle Lib "kernel32" (ByVal hObject As Integer) As Integer

    Private Declare Function OpenProcessToken Lib "advapi32" (ByVal hProcess As Integer, ByVal dwDesiredAccess As Integer, ByRef hToken As Integer) As Integer

    Private Declare Function GetCurrentProcess Lib "kernel32" () As Integer


    Private Declare Function GetTokenInformation Lib "advapi32.dll" (ByVal hToken As Integer, ByVal eTokenInformationClass As gPMConstants.TOKEN_INFORMATION_CLASS, ByVal uTokenInformation As Integer, ByVal nTokenInformationLength As Integer, ByRef nReturnLength As Integer) As Integer

    Private Declare Function FormatMessage Lib "kernel32" Alias "FormatMessageA" (ByVal dwFlags As Integer, ByVal lpSource As Integer, ByVal dwMessageId As Integer, ByVal dwLanguageId As Integer, ByVal lpBuffer As String, ByVal nSize As Integer, ByRef Arguments As Integer) As Integer

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

    <DllImport("Wtsapi32.dll", SetLastError:=True)>
    Private Function WTSQuerySessionInformation(
       ByVal hServer As IntPtr,
       ByVal sessionId As Integer,
       ByVal infoClass As WTS_INFO_CLASS,
       ByRef ppBuffer As IntPtr,
       ByRef pBytesReturned As Integer) As Boolean
    End Function

    <DllImport("Wtsapi32.dll")>
    Private Sub WTSFreeMemory(ByVal pMemory As IntPtr)
    End Sub

    ' 24/07/2002 PWF - NullToXXX Function Default Constants
    '
    ' These allow us to quickly enforce correct type without
    ' conversion. Note: Decimal is an exception but included
    ' for completeness.
    Private Const NullToBooleanDefault As Boolean = False
    Private Const NullToDateDefault As Date = #12/29/1899#
    Private Const NullToDecimalDefault As Integer = 0
    Private Const NullToLongDefault As Integer = 0
    Private Const NullToStringDefault As String = ""
    Private Const NullToIntegerDefault As Integer = 0
    Private Const NullToDoubleDefault As Double = 0
    Private Const NullToCurrencyDefault As Decimal = 0

    'Changes for Windows unified login.
    '----------------------------------
    Private Structure USER_INFO_0
        Dim usri0_name As Integer
    End Structure

    ' function provides information about all user
    ' accounts on a server
    Private Declare Function NetUserEnum Lib "netapi32.DLL" (ByVal lServerName As Integer, ByVal lLevel As Integer, ByVal lFilter As Integer, ByRef lBufferpointer As Integer, ByVal lMaxLength As Integer, ByRef lEntriesRead As Integer, ByRef lTotalEntries As Integer, ByRef lResumeHandle As Integer) As Integer

    ' function frees the memory that the NetApiBufferAllocate
    ' function allocates.
    Private Declare Function NetApiBufferFree Lib "netapi32.DLL" (ByVal lBuffer As Integer) As Integer

    ' Retrieves the length of the specified wide string.
    Private Declare Function lstrlenW Lib "kernel32" (ByVal lpString As Integer) As Integer

    Private Const FILTER_NORMAL_ACCOUNT As Integer = &H2
    Private Const MAX_PREFERRED_LENGTH As Integer = -1
    Private Const NERR_SUCCESS As Integer = 0
    Private Const ERROR_MORE_DATA As Integer = 234

    Private Const kUSLangId As Integer = 2
    Private Const kUKLangId As Integer = 1

    'PN23693
    'Object for LDAP users
    Private m_objLDAPItem As Object
    Private m_objLDAPItem1 As Object

    Private Declare Function MSPeelerMain Lib "msfilter.dll" (ByVal sHtmlFile As String, ByVal sCmdOptions As String) As Short

    Public Function GetUsersFromLDAP(ByRef sAD_OU_Path As String, ByRef r_vAvailableUsers() As Object) As Integer
        Try

            GetUsersFromLDAP = gPMConstants.PMEReturnCode.PMTrue

            ReDim r_vAvailableUsers(0)
            'Example:sAD_OU_Path = "ou=General Users,ou=Birmingham Office,dc=siriusfs,dc=com"

            Dim ldapPath As String = "LDAP://" & sAD_OU_Path
            Dim searchRoot As New DirectoryEntry(ldapPath)

            ' Set up a DirectorySearcher to find user objects in the directory
            Dim searcher As New DirectorySearcher(searchRoot)
            searcher.Filter = "(&(objectClass=user)(objectCategory=person))"
            searcher.PropertiesToLoad.Add("samaccountname") ' Load the usernames (sAMAccountName)

            ' Get search results
            Dim searchResults As SearchResultCollection = searcher.FindAll()

            For Each result As SearchResult In searchResults
                ' Fetch the username (sAMAccountName)
                If result.Properties.Contains("samaccountname") Then
                    Dim username As String = result.Properties("samaccountname")(0).ToString()

                    If r_vAvailableUsers(0) IsNot Nothing Then
                        ReDim Preserve r_vAvailableUsers(r_vAvailableUsers.GetUpperBound(0) + 1)
                    End If

                    r_vAvailableUsers(r_vAvailableUsers.GetUpperBound(0)) = username
                End If
            Next

            Return GetUsersFromLDAP

        Catch excep As System.Exception
            GetUsersFromLDAP = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get available Users from LDAP query.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUsersFromLDAP", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return GetUsersFromLDAP
        End Try
    End Function



    Private Function UserProperty(ByRef strProperty As String) As String
        Dim result As String = String.Empty
        Try
            result = m_objLDAPItem.Get(strProperty)
        Catch
            result = "NA"
        End Try
        Return result
    End Function

    Private Function UserProperty1(ByRef strProperty As String) As String
        Dim result As String = String.Empty
        Try
            result = m_objLDAPItem1.Get(strProperty)
        Catch
            result = "NA"
        End Try
        Return result
    End Function

    Public Function DeleteFolderAll(ByVal sFolder As String) As Integer
        Dim result As Integer = 0
        Dim sSubFolder As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Delete all non-folder files
            File.Delete(sFolder & "\" & "*.*")

            ' Loop round the sub folders
            sSubFolder = FileSystem.Dir(sFolder & "\*.*", FileAttribute.Directory)
            Do While sSubFolder <> ""
                If sSubFolder <> "." And sSubFolder <> ".." Then
                    ' Empty and remove the folder
                    m_lReturn = DeleteFolderAll(sFolder & "\" & sSubFolder)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return m_lReturn
                    End If
                    ' Get the next sub-folder (need to start again if we've called
                    ' ourself recursively, 'cos the Dir command will have been
                    ' hijacked
                    sSubFolder = FileSystem.Dir(sFolder & "\*.*", FileAttribute.Directory)
                Else
                    ' Get the next sub-folder
                    'Modified by Deepak Sharma on 4/20/2010 12:55:16 PM refer developer guide no. 19 (Guide)
                    'sSubFolder = FileSystem.Dir( , FileAttribute.Directory)
                    sSubFolder = FileSystem.Dir(FileAttribute.Directory)
                End If
            Loop

            ' Remove the folder
            Directory.Delete(sFolder)

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete the folder", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteFolderAll", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PMStartOfWeek
    '
    ' Description: Returns the start of the week date from the given
    '              date.
    '
    ' ***************************************************************** '
    Public Function PMStartOfWeek(ByVal dtDate As Date) As Date
        Try
            ' PWF 10/10/2002: Simplified
            Return dtDate.AddDays(-(DateAndTime.Weekday(dtDate, FirstDayOfWeek.Monday) - 1))
        Catch
            Return dtDate
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FormatField
    '
    ' Description: Formats a field to the type specified.
    '
    ' History:
    '   Peter Finney 13/06/2003 - Added optional decimal places. In this
    '       instance it is specifically for PMFormatPercent!
    '       See local "Case" comments for usage
    ' ***************************************************************** '
    'Public Function FormatField(ByVal iFormatType As Integer, ByVal vFieldValue As String, Optional ByVal vDecimalPlaces As Integer = 0) As String
    Public Function FormatField(ByVal iFormatType As Integer, ByVal vFieldValue As String, Optional ByVal vDecimalPlaces As Integer = -1) As String
        Dim sControlResult As String

        Try

            ' Check for a null value

            If Convert.IsDBNull(vFieldValue) Or IsNothing(vFieldValue) Then
                vFieldValue = ""
            End If

            ' Determine which field type it is.
            Select Case iFormatType
                Case gPMConstants.PMEFormatStyle.PMFormatString
                    ' Format value to a string.
                    sControlResult = vFieldValue.Trim()

                Case gPMConstants.PMEFormatStyle.PMFormatStringMultiLine
                    ' Format value to a string.
                    sControlResult = vFieldValue.Trim()
                    ReplaceVbCrWithVbCrLf(sControlResult)

                Case gPMConstants.PMEFormatStyle.PMFormatStringCase
                    ' Format value to a string with proper case.
                    sControlResult = FormatName(vFieldValue)

                Case gPMConstants.PMEFormatStyle.PMFormatStringUpper
                    ' Format value to a string with uppercase.
                    sControlResult = vFieldValue.Trim().ToUpper()

                Case gPMConstants.PMEFormatStyle.PMFormatDateShort
                    ' Format value to a short date
                    If Not Information.IsDate(vFieldValue) Then
                        sControlResult = ""
                    Else
                        sControlResult = DateTime.Parse(vFieldValue).ToString("d").Trim()
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatDateMedium
                    ' Format value to a medium date
                    If Not Information.IsDate(vFieldValue) Then
                        sControlResult = ""
                    Else
                        sControlResult = StringsHelper.Format(vFieldValue, "medium date").Trim()
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatDateLong
                    ' Format value to a long date
                    If Not Information.IsDate(vFieldValue) Then
                        sControlResult = ""
                    Else
                        sControlResult = DateTime.Parse(vFieldValue).ToString("D").Trim()
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatTimeShort
                    ' Format value to a short time
                    If Not Information.IsDate(vFieldValue) Then
                        sControlResult = ""
                    Else
                        sControlResult = DateTime.Parse(vFieldValue).ToString("t").Trim()
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatTimeMedium
                    ' Format value to a medium time
                    If Not Information.IsDate(vFieldValue) Then
                        sControlResult = ""
                    Else
                        sControlResult = StringsHelper.Format(vFieldValue, "medium time").Trim()
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatTimeLong
                    ' Format value to a long time
                    If Not Information.IsDate(vFieldValue) Then
                        sControlResult = ""
                    Else
                        sControlResult = DateTime.Parse(vFieldValue).ToString("T").Trim()
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatDateTimeShort
                    ' Format value to a short date and time
                    If Not Information.IsDate(vFieldValue) Then
                        sControlResult = ""
                    Else
                        sControlResult = DateTime.Parse(vFieldValue).ToString("d").Trim() &
                                         " " & DateTime.Parse(vFieldValue).ToString("t").Trim()
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatDateTimeMedium
                    ' Format value to a medium date and time
                    If Not Information.IsDate(vFieldValue) Then
                        sControlResult = ""
                    Else
                        sControlResult = StringsHelper.Format(vFieldValue, "medium date").Trim() &
                                         " " & StringsHelper.Format(vFieldValue, "medium time").Trim()
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatDateTimeLong
                    ' Format value to a long date and time
                    If Not Information.IsDate(vFieldValue) Then
                        sControlResult = ""
                    Else
                        sControlResult = DateTime.Parse(vFieldValue).ToString("D").Trim() &
                                         " " & DateTime.Parse(vFieldValue).ToString("T").Trim()
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatDateYearOnly
                    ' Format value to a year only date
                    If Not Information.IsDate(vFieldValue) Then
                        sControlResult = ""
                    Else
                        Dim TempDate As Date
                        sControlResult = (IIf(DateTime.TryParse(vFieldValue, TempDate), TempDate.ToString("yyyy"), vFieldValue)).Trim()
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatMonthOnlyLong
                    ' Format value to a Month in Long format eg January
                    If Not Information.IsDate(vFieldValue) Then
                        sControlResult = ""
                    Else
                        Dim TempDate2 As Date
                        sControlResult = (IIf(DateTime.TryParse(vFieldValue, TempDate2), TempDate2.ToString("MMMM"), vFieldValue)).Trim()
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatMonthOnlyMedium
                    ' Format value to a Month in Medium format eg Jan
                    If Not Information.IsDate(vFieldValue) Then
                        sControlResult = ""
                    Else
                        Dim TempDate3 As Date
                        sControlResult = (IIf(DateTime.TryParse(vFieldValue, TempDate3), TempDate3.ToString("MMM"), vFieldValue)).Trim()
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatMonthOnlyShort
                    ' Format value to a Month in Short format eg 01
                    If Not Information.IsDate(vFieldValue) Then
                        sControlResult = ""
                    Else
                        Dim TempDate4 As Date
                        sControlResult = (IIf(DateTime.TryParse(vFieldValue, TempDate4), TempDate4.ToString("MM"), vFieldValue)).Trim()
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatDayOnlyLong
                    ' Format value to a Day in Long format eg Friday
                    If Not Information.IsDate(vFieldValue) Then
                        sControlResult = ""
                    Else
                        Dim TempDate5 As Date
                        sControlResult = (IIf(DateTime.TryParse(vFieldValue, TempDate5), TempDate5.ToString("dddd"), vFieldValue)).Trim()
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatDayOnlyMedium
                    ' Format value to a Day in Medium format eg Fri
                    If Not Information.IsDate(vFieldValue) Then
                        sControlResult = ""
                    Else
                        Dim TempDate6 As Date
                        sControlResult = (IIf(DateTime.TryParse(vFieldValue, TempDate6), TempDate6.ToString("ddd"), vFieldValue)).Trim()
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatDayOnlyShort
                    ' Format value to a Day in Short format eg 01
                    If Not Information.IsDate(vFieldValue) Then
                        sControlResult = ""
                    Else
                        Dim TempDate7 As Date
                        sControlResult = (IIf(DateTime.TryParse(vFieldValue, TempDate7), TempDate7.ToString("dd"), vFieldValue)).Trim()
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatDecimal
                    ' Format value to a currency
                    Dim dbNumericTemp As Double
                    If Not Double.TryParse(vFieldValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                        sControlResult = ""
                    Else
                        sControlResult = StringsHelper.Format(vFieldValue, "standard").Trim()
                    End If
                Case gPMConstants.PMEFormatStyle.PMFormatCurrency
                    ' Format value to a currency
                    Dim dbNumericTemp As Double
                    If Not Double.TryParse(vFieldValue, NumberStyles.Currency, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                        sControlResult = ""
                    Else
                        sControlResult = StringsHelper.Format(vFieldValue, "standard").Trim()
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatMoney
                    ' Format value to decimal pounds sterling
                    Dim dbNumericTemp2 As Double
                    If Not Double.TryParse(vFieldValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                        sControlResult = ""
                    Else
                        sControlResult = StringsHelper.Format(vFieldValue, "#,##0.00").Trim()
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatWholeMoney
                    ' Format value to a whole pounds sterling
                    Dim dbNumericTemp3 As Double
                    If Not Double.TryParse(vFieldValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                        sControlResult = ""
                    Else
                        vFieldValue = CStr(gPMMaths.PMTruncateCurrency(v_vWholeValue:=CDec(vFieldValue), v_eNumberOfDP:=gPMConstants.PMECurrencyNoOfDP.pmeCurDPZero))

                        sControlResult = StringsHelper.Format(vFieldValue, "#,###").Trim()
                    End If

                    ' PWF - Long was missing, quick fix.
                Case gPMConstants.PMEFormatStyle.PMFormatInteger, gPMConstants.PMEFormatStyle.PMFormatLong
                    ' Format value to an integer
                    Dim dbNumericTemp4 As Double
                    If Double.TryParse(vFieldValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                        sControlResult = StringsHelper.Format(vFieldValue, "General Number").Trim()
                    Else
                        sControlResult = ""
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatBoolean
                    If CInt(vFieldValue) = 0 Then
                        sControlResult = CStr(False)
                    Else
                        sControlResult = CStr(True)
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatPercent
                    ' Format value to a currency
                    Dim dbNumericTemp6 As Double
                    If Not Double.TryParse(vFieldValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) Then
                        sControlResult = ""
                    Else
                        ' ****************************************************************
                        ' Usage:
                        '
                        '   vDecimalPlaces = 0
                        '       Format to whole percentage
                        '   vDecimalPlaces > 0
                        '       Format to an exact number of decimal places
                        '   vDecimalPlaces < -1
                        '       Format "up to" specified places (minimum of 2)
                        '   vDecimalPlaces = Not passed, Invalid or -1
                        '       Default to standard 2 decimal places
                        ' ****************************************************************

                        ' Check decimal places
                        Dim dbNumericTemp5 As Double

                        If (Not Information.IsNothing(vDecimalPlaces)) And Double.TryParse(CStr(vDecimalPlaces), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                            Select Case vDecimalPlaces
                                Case 0
                                    sControlResult = StringsHelper.Format(vFieldValue, "#,##0").Trim() & "%"
                                Case Is > 0
                                    sControlResult = StringsHelper.Format(vFieldValue, "#,##0." & New String("0", vDecimalPlaces)).Trim() & "%"
                                Case Is < -1
                                    ' Enforce minimum of 2 fixed digits
                                    sControlResult = StringsHelper.Format(vFieldValue, "#,##0.00" & New String("#", -2 - vDecimalPlaces)).Trim() & "%"

                                Case Else ' i.e. -1
                                    ' Stick to the original 2
                                    sControlResult = StringsHelper.Format(vFieldValue, "#,##0.00").Trim() & "%"
                            End Select
                        Else
                            ' Stick to the original 2
                            sControlResult = StringsHelper.Format(vFieldValue, "#,##0.00").Trim() & "%"
                        End If
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatPercentFourDecimal
                    ' Format value to a percentage
                    Dim dbNumericTemp7 As Double
                    If Not Double.TryParse(vFieldValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp7) Then
                        sControlResult = ""
                    Else
                        sControlResult = StringsHelper.Format(vFieldValue, "##0.0000").Trim() & "%"
                    End If
            End Select


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
    Public Function UnFormatField(ByRef iFormatTypeIn As Integer, ByRef iDataTypeOut As Integer, ByVal vFieldValue As Object) As Object

        Dim vControlResult As Object

        Try

            ' Check for a null value

            If Convert.IsDBNull(vFieldValue) Or IsNothing(vFieldValue) Then

                vFieldValue = ""
            End If

            ' Check the format of the in value.
            Select Case (iFormatTypeIn)
                Case gPMConstants.PMEFormatStyle.PMFormatString, gPMConstants.PMEFormatStyle.PMFormatStringCase, gPMConstants.PMEFormatStyle.PMFormatStringUpper, gPMConstants.PMEFormatStyle.PMFormatStringMultiLine
                    If iDataTypeOut <> gPMConstants.PMEDataType.PMString Then

                        If CStr(vFieldValue).Trim() = "" Then

                            vFieldValue = "0"
                        End If
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatPercent, gPMConstants.PMEFormatStyle.PMFormatPercentFourDecimal

                    If CStr(vFieldValue).Substring(Strings.Len(CStr(vFieldValue)) - 1) = "%" Then


                        vFieldValue = CStr(vFieldValue).Substring(0, Math.Min(CStr(vFieldValue).Length, Strings.Len(CStr(vFieldValue)) - 1))
                    End If


                    Dim dbNumericTemp As Double
                    If Not Double.TryParse(CStr(vFieldValue), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                        vFieldValue = 0
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatDateLong, gPMConstants.PMEFormatStyle.PMFormatDateMedium, gPMConstants.PMEFormatStyle.PMFormatDateShort, gPMConstants.PMEFormatStyle.PMFormatDateTimeLong, gPMConstants.PMEFormatStyle.PMFormatDateTimeMedium, gPMConstants.PMEFormatStyle.PMFormatDateTimeShort

                    Dim nTempFieldValue As String = String.Empty
                    'Replace the "." with the "/" to make a valid date
                    vFieldValue = CStr(vFieldValue).Replace("."c, "/"c)

                    If Not Information.IsDate(vFieldValue) Then
                        nTempFieldValue = ""
                    Else
                        nTempFieldValue = DateTime.Parse(vFieldValue).ToString("D").Trim()
                    End If

                    ' If we have a value but it is not a valid date it could be due to the day name
                    If (Strings.Len(CStr(nTempFieldValue)) > 0) And (Not Information.IsDate(nTempFieldValue)) Then
                        ' Strip the first word (assuming for now it's the day name)
                        nTempFieldValue = CStr(nTempFieldValue).Substring(CStr(nTempFieldValue).IndexOf(" "c) + 1)
                    End If

                    ' Recheck
                    If Not Information.IsDate(nTempFieldValue) Then
                        nTempFieldValue = #12/29/1899#
                    ElseIf CDate(nTempFieldValue) < gPMConstants.PMSystemLowDate Or CDate(nTempFieldValue) > gPMConstants.PMSystemHighDate Then
                        nTempFieldValue = #12/29/1899#
                    End If

                    vFieldValue = nTempFieldValue

                Case gPMConstants.PMEFormatStyle.PMFormatTimeLong, gPMConstants.PMEFormatStyle.PMFormatTimeMedium, gPMConstants.PMEFormatStyle.PMFormatTimeShort
                    ' Time can be formated with '.' convert to ':' to allow safer processing


                    vFieldValue = CStr(vFieldValue).Replace("."c, ":"c)

                    ' Recheck
                    If Not Information.IsDate(vFieldValue) Then

                        vFieldValue = #12/29/1899#
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatCurrency, gPMConstants.PMEFormatStyle.PMFormatInteger, gPMConstants.PMEFormatStyle.PMFormatLong, gPMConstants.PMEFormatStyle.PMFormatDouble, gPMConstants.PMEFormatStyle.PMFormatBoolean, gPMConstants.PMEFormatStyle.PMFormatDecimal, gPMConstants.PMEFormatStyle.PMFormatMoney, gPMConstants.PMEFormatStyle.PMFormatWholeMoney


                    Dim dbNumericTemp2 As Double
                    If CStr(vFieldValue) = "" Or Not Double.TryParse(CStr(vFieldValue), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                        vFieldValue = 0
                    End If

                Case Else
                    ' Do nothing
            End Select

            ' Determine which field type it is.
            Select Case (iDataTypeOut)
                Case gPMConstants.PMEDataType.PMString

                    Dim dbNumericTemp3 As Double
                    If (Double.TryParse(CStr(vFieldValue), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3)) Or (Information.IsDate(vFieldValue)) Then
                        ' Format value to a string.


                        vControlResult = CStr(vFieldValue).Trim()
                    Else


                        vControlResult = vFieldValue
                    End If

                Case gPMConstants.PMEDataType.PMDate
                    ' Format value to a short date


                    vControlResult = CDate(vFieldValue)

                Case gPMConstants.PMEDataType.PMCurrency


                    vControlResult = CDec(vFieldValue)

                Case gPMConstants.PMEDataType.PMInteger, gPMConstants.PMEDataType.PMLong


                    vControlResult = CInt(vFieldValue)

                Case gPMConstants.PMEDataType.PMDouble


                    vControlResult = CDbl(vFieldValue)

                Case gPMConstants.PMEFormatStyle.PMFormatDecimal


                    vControlResult = CDec(vFieldValue)

                Case Else


                    vControlResult = vFieldValue
            End Select

            Return vControlResult
        Catch
            Return vFieldValue
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: LogMessageToFile
    '
    ' Description: Logs a message into Log File defined in the registry.
    '
    ' History: RDC 29072002 All messaging now goes to the o/s event log.
    '                       If event log messaging fails, DO NOT use popup!
    ' ***************************************************************** '
    Public Sub LogMessageToFile(ByRef sUsername As String, ByRef iType As Integer, ByRef sMsg As String)
        LogMessageToFile(sUsername, iType, sMsg, "", "", "", Nothing, "", Nothing)
    End Sub
    Public Sub LogMessageToFile(ByRef sUsername As String, ByRef iType As Integer, ByRef sMsg As String, ByRef vApp As String)
        LogMessageToFile(sUsername, iType, sMsg, vApp, "", "", Nothing, "", Nothing)
    End Sub
    Public Sub LogMessageToFile(ByRef sUsername As String, ByRef iType As Integer, ByRef sMsg As String, ByRef vApp As String, ByRef vClass As String)
        LogMessageToFile(sUsername, iType, sMsg, vApp, vClass, "", Nothing, "", Nothing)
    End Sub
    Public Sub LogMessageToFile(ByRef sUsername As String, ByRef iType As Integer, ByRef sMsg As String, ByRef vApp As String, ByRef vClass As String, ByRef vMethod As String)
        LogMessageToFile(sUsername, iType, sMsg, vApp, vClass, vMethod, Nothing, "", Nothing)
    End Sub
    Public Sub LogMessageToFile(ByRef sUsername As String, ByRef iType As Integer, ByRef sMsg As String, ByRef vApp As String, ByRef vClass As String, ByRef vMethod As String, ByVal sErrUniqueId As String)
        LogMessageToFile(sUsername, iType, sMsg, vApp, vClass, vMethod, Nothing, sErrUniqueId, Nothing)
    End Sub
    Public Sub LogMessageToFile(ByRef sUsername As String, ByRef iType As Integer, ByRef sMsg As String, ByRef vApp As String, ByRef vClass As String, ByRef vMethod As String, ByRef excep As Exception)
        LogMessageToFile(sUsername, iType, sMsg, vApp, vClass, vMethod, excep, "", Nothing)
    End Sub
    Public Sub LogMessageToFile(ByRef sUsername As String, ByRef iType As Integer, ByRef sMsg As String, ByRef vApp As String, ByRef vClass As String, ByRef vMethod As String, ByRef excep As Exception, ByVal sErrUniqueId As String)
        LogMessageToFile(sUsername, iType, sMsg, vApp, vClass, vMethod, excep, sErrUniqueId, Nothing)
    End Sub


    Public Sub LogMessageToFile(ByRef sUsername As String, ByRef iType As Integer, ByRef sMsg As String, ByRef oDicParms As Dictionary(Of String, Object))
        LogMessageToFile(sUsername, iType, sMsg, "", "", "", Nothing, "", oDicParms)
    End Sub
    Public Sub LogMessageToFile(ByRef sUsername As String, ByRef iType As Integer, ByRef sMsg As String, ByRef vApp As String, ByRef oDicParms As Dictionary(Of String, Object))
        LogMessageToFile(sUsername, iType, sMsg, vApp, "", "", Nothing, "", oDicParms)
    End Sub
    Public Sub LogMessageToFile(ByRef sUsername As String, ByRef iType As Integer, ByRef sMsg As String, ByRef vApp As String, ByRef vClass As String, ByRef oDicParms As Dictionary(Of String, Object))
        LogMessageToFile(sUsername, iType, sMsg, vApp, vClass, "", Nothing, "", oDicParms)
    End Sub
    Public Sub LogMessageToFile(ByRef sUsername As String, ByRef iType As Integer, ByRef sMsg As String, ByRef vApp As String, ByRef vClass As String, ByRef vMethod As String, ByRef oDicParms As Dictionary(Of String, Object))
        LogMessageToFile(sUsername, iType, sMsg, vApp, vClass, vMethod, Nothing, "", oDicParms)
    End Sub
    Public Sub LogMessageToFile(ByRef sUsername As String, ByRef iType As Integer, ByRef sMsg As String, ByRef vApp As String, ByRef vClass As String, ByRef vMethod As String, ByVal sErrUniqueId As String, ByRef oDicParms As Dictionary(Of String, Object))
        LogMessageToFile(sUsername, iType, sMsg, vApp, vClass, vMethod, Nothing, sErrUniqueId, oDicParms)
    End Sub
    Public Sub LogMessageToFile(ByRef sUsername As String, ByRef iType As Integer, ByRef sMsg As String, ByRef vApp As String, ByRef vClass As String, ByRef vMethod As String, ByRef excep As Exception, ByRef oDicParms As Dictionary(Of String, Object))
        LogMessageToFile(sUsername, iType, sMsg, vApp, vClass, vMethod, excep, "", oDicParms)
    End Sub

    Public Sub LogMessageToFile(ByRef sUsername As String, ByRef iType As Integer, ByRef sMsg As String, ByRef vApp As String, ByRef vClass As String, ByRef vMethod As String, ByRef excep As Exception, ByVal sErrUniqueId As String, ByRef oDicParms As Dictionary(Of String, Object))
        Try
            m_lReturn = EventLogWrite(iType:=iType, sMsg:=sMsg, oDicParms:=oDicParms, sErrUniqueId:=sErrUniqueId, excep:=excep, vUsername:=sUsername, vApp:=vApp, vClass:=vClass, vMethod:=vMethod)
        Catch
        End Try
    End Sub
    'NIIT: To Do : New method added as per proactive list for exception handling
    Public Sub LogExcepMessageToFile(ByRef sUsername As String, ByRef iType As Integer, ByRef sMsg As String, Optional ByRef vApp As String = "", Optional ByRef vClass As String = "", Optional ByRef vMethod As String = "", Optional ByRef vErrNo As String = "", Optional ByRef vErrDesc As String = "")
        LogMessageToFile(sUsername, iType, sMsg, vApp, vClass, vMethod, New Exception(vErrDesc))
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
    Public Function InsertUserName(ByRef r_sLogFile As String, ByVal v_sUsername As String) As Integer

        Dim result As Integer = 0
        Dim sLogFile As String = ""
        Dim lSub As Integer
        Dim sPrefix, sSuffix As String


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sLogFile = r_sLogFile

            For lSub = sLogFile.Length To 1 Step -1
                If sLogFile.Substring(lSub - 1, 1) = "\" Then
                    Exit For
                End If
            Next

            If lSub > 1 Then
                sPrefix = sLogFile.Substring(0, lSub)
                sSuffix = sLogFile.Substring(sLogFile.Length - (sLogFile.Length - lSub))
                sLogFile = v_sUsername & "_" & sSuffix
                For lSub = 1 To sLogFile.Length
                    If sLogFile.Substring(lSub - 1, 1) = " " Then
                        Mid(sLogFile, lSub, 1) = "_"
                    End If
                Next
                sLogFile = sPrefix & sLogFile
            End If

            r_sLogFile = sLogFile

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InsertUserName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InsertUserName", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RaiseError
    '
    ' Description: Call this to raise an error and jucp to the error handler.
    '              It uses a default LogLevel of PMLogDebug1 so the error
    '              message may not be output to the Log File depending on the
    '              user's log level setting.
    '              If you are raising an error that has not been raised elsewhere,
    '              ensure that it is output to the log file by raising the level
    '              of the error to PMLogError.
    '  Source      The source of the error, with the aim of helping you quickly
    '              find its location in the code if you do not have the line number information.
    '              This is not the function that you are currently in, but something like the:
    '                   Object.Method being called
    '                   Variable being checked
    '                   Reference to the section of code within the method
    '
    '  Description This should give as much detail about the error as possible and include
    '              the value of any relevant variables if possible.
    ' ***************************************************************** '

    Public Sub RaiseError(ByVal v_sSource As String, ByVal v_sDescription As String)
        RaiseError(v_sSource, v_sDescription, -1)
    End Sub

    Public Sub RaiseError(ByVal v_sSource As String, ByVal v_sDescription As String, ByRef LogLevel As gPMConstants.PMELogLevel)
        Throw New System.Exception(v_sSource + ", " + v_sDescription)
    End Sub



    ' ***************************************************************** '
    ' Name: LogMessagePopup
    '
    ' Description: Logs a message into the PM Log File.
    '
    ' History: RDC 29072002 tidyup during messaging review
    ' ***************************************************************** '
    Public Sub LogMessagePopup(ByVal iType As Integer, ByVal sMsg As String)
        LogMessagePopup(iType, sMsg, "", "", "", Nothing, Nothing)
    End Sub

    Public Sub LogMessagePopup(ByVal iType As Integer, ByVal sMsg As String, ByRef vApp As String)
        LogMessagePopup(iType, sMsg, vApp, "", "", Nothing, Nothing)
    End Sub

    Public Sub LogMessagePopup(ByVal iType As Integer, ByVal sMsg As String, ByRef vApp As String, ByRef vClass As String)
        LogMessagePopup(iType, sMsg, vApp, vClass, "", Nothing, Nothing)
    End Sub

    Public Sub LogMessagePopup(ByVal iType As Integer, ByVal sMsg As String, ByRef vApp As String, ByRef vClass As String, ByRef vMethod As String)
        LogMessagePopup(iType, sMsg, vApp, vClass, vMethod, Nothing, Nothing)
    End Sub

    Public Sub LogMessagePopup(ByVal iType As Integer, ByVal sMsg As String, ByRef vApp As String, ByRef vClass As String, ByRef vMethod As String, ByRef excep As Exception)
        LogMessagePopup(iType, sMsg, vApp, vClass, vMethod, excep, Nothing)
    End Sub

    Public Sub LogMessagePopup(ByVal iType As Integer, ByVal sMsg As String, ByRef vApp As String, ByRef vClass As String, ByRef vMethod As String, ByRef oDicParms As Dictionary(Of String, Object))
        LogMessagePopup(iType, sMsg, vApp, vClass, vMethod, Nothing, oDicParms)
    End Sub

    Public Sub LogMessagePopup(ByVal iType As Integer, ByVal sMsg As String, ByRef vApp As String, ByRef vClass As String, ByRef vMethod As String, ByRef excep As Exception, ByRef oDicParms As Dictionary(Of String, Object))

        Dim sLogMessage As String = ""
        Dim iMsgboxParms As MsgBoxStyle
        Dim iTypeOfMessage As MsgBoxStyle
        Dim sMessageTypeText As String

        Try
            Dim sErrUniqueId As String = GenerateUniqueSSPExceptionRef(gPMConstants.ERROR_NO_LENGTH)
            ' RDC 30082002 messages to event log or text files?

            ' we're writing to the o/s application event log
            m_lReturn = EventLogWrite(iType:=iType, sMsg:=sMsg, oDicParms:=oDicParms, sErrUniqueId:=sErrUniqueId, excep:=excep, vApp:=vApp, vClass:=vClass, vMethod:=vMethod)


            ' Set Message Type and description
            Select Case iType
                Case gPMConstants.PMELogLevel.PMLogFatal
                    iTypeOfMessage = MsgBoxStyle.Critical
                    sMessageTypeText = gPMConstants.PMFatalText
                    sMessageTypeText = gPMConstants.PMFatalText
                Case gPMConstants.PMELogLevel.PMLogError
                    iTypeOfMessage = MsgBoxStyle.Critical
                    sMessageTypeText = gPMConstants.PMErrorText
                Case gPMConstants.PMELogLevel.PMLogWarning
                    iTypeOfMessage = MsgBoxStyle.Exclamation
                    sMessageTypeText = gPMConstants.PMWarningText
                Case gPMConstants.PMELogLevel.PMLogInfo
                    iTypeOfMessage = MsgBoxStyle.Exclamation
                    sMessageTypeText = gPMConstants.PMInfoText
                Case gPMConstants.PMELogLevel.PMLogOnError
                    iTypeOfMessage = MsgBoxStyle.Exclamation
                    sMessageTypeText = gPMConstants.PMOnErrorText
                Case gPMConstants.PMELogLevel.PMLogDebug1
                    iTypeOfMessage = MsgBoxStyle.Information
                    sMessageTypeText = gPMConstants.PMDebug1Text
                Case gPMConstants.PMELogLevel.PMLogDebug2
                    iTypeOfMessage = MsgBoxStyle.Information
                    sMessageTypeText = gPMConstants.PMDebug2Text
                Case gPMConstants.PMELogLevel.PMLogDebug3
                    iTypeOfMessage = MsgBoxStyle.Information
                    sMessageTypeText = gPMConstants.PMDebug3Text
                Case gPMConstants.PMELogLevel.PMLogFeedback
                    iTypeOfMessage = MsgBoxStyle.Information
                    sMessageTypeText = gPMConstants.PMFeedbackText
                Case Else
                    iTypeOfMessage = MsgBoxStyle.Information
                    sMessageTypeText = gPMConstants.PMDebug4Text
            End Select

            ' Msgbox Parameters are
            ' OK Button Only, Type of Message, Button1 is defualt, Application Modal
            iMsgboxParms = MsgBoxStyle.OkOnly + iTypeOfMessage + MsgBoxStyle.DefaultButton1 + MsgBoxStyle.ApplicationModal

            ' Current date/time
            sLogMessage = "Date/Time: " & DateTimeHelper.ToString(DateTime.Now) & Strings.Chr(13) & Strings.Chr(10)

            ' Add Unique Error number if we have them
            sLogMessage = sLogMessage & sErrUniqueId & Strings.Chr(13) & Strings.Chr(10)

            ' Display optional parameters if they are present
            If Not (False) Then
                If vApp <> "" Then
                    sLogMessage = sLogMessage & "Application: " & vApp & Strings.Chr(13) & Strings.Chr(10)
                End If
            End If

            If Not (False) Then
                If vClass <> "" Then
                    sLogMessage = sLogMessage & "Class: " & vClass & Strings.Chr(13) & Strings.Chr(10)
                End If
            End If

            If Not (False) Then
                If vMethod <> "" Then
                    sLogMessage = sLogMessage & "Method: " & vMethod & Strings.Chr(13) & Strings.Chr(10)
                End If
            End If

            ' Add the message to the end
            sLogMessage = sLogMessage & "Message Text: " & sMsg & Strings.Chr(13) & Strings.Chr(10)

            If excep IsNot Nothing AndAlso excep.Message IsNot Nothing Then
                sLogMessage = sLogMessage & Environment.NewLine & "Ex Message : " & excep.Message
            End If

            ' PWF 01/09/2002 - Feedback messages should just be the message
            If iType = gPMConstants.PMELogLevel.PMLogFeedback Then
                sLogMessage = sMsg
            End If

            ' Display the Log Message on the screen
            Interaction.MsgBox(sLogMessage, iMsgboxParms, sMessageTypeText & " Message")

        Catch
        End Try


        Exit Sub


    End Sub
    'NIIT: To Do : New method added as per proactive list for exception handling
    Public Sub LogExecpMessagePopup(ByVal iType As Integer, ByVal sMsg As String, Optional ByRef vApp As String = "", Optional ByRef vClass As String = "", Optional ByRef vMethod As String = "", Optional ByRef vErrNo As String = "", Optional ByRef vErrDesc As String = "", Optional ByVal oException As Exception = Nothing)

        LogMessagePopup(iType, sMsg, vApp, vClass, vMethod, oException)

    End Sub


    ' ***************************************************************** '
    ' Name: GetRegSettings
    '
    ' Description: Get settings from the registry.
    '
    ' ***************************************************************** '
    Public Function GetRegSettings(ByRef sResult As String, ByRef sAppName As String, ByRef sSection As String, ByRef sKey As String, Optional ByRef vDefault As String = "") As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

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

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the registry settings", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRegSettings", excep:=excep)

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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get section setting from the registry.
            vResult = Interaction.GetAllSettings(sAppName, sSection)

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get all of the registry settings", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRegAllSettings", excep:=excep)

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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Save setting to the registry.
            Interaction.SaveSetting(sAppName, sSection, sKey, sSetting)

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to save the registry settings", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveRegSettings", excep:=excep)

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

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Delete setting from the registry.
            Interaction.DeleteSetting(sAppName, sSection, sKey)

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete the registry settings", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRegSettings", excep:=excep)

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
        Dim sBuffer As New FixedLengthString(255)
        Dim lBufferSize, lSessionID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lBufferSize = 255

            ' API Call to get computer name
            m_lReturn = GetComputerName(sBuffer.Value, lBufferSize)

            ' Check return code
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Return error
                sSystemName = ""
                result = gPMConstants.PMEReturnCode.PMFalse
            Else
                ' Set System Name Parameter
                sSystemName = sBuffer.Value.Substring(0, Math.Min(sBuffer.Value.Length, lBufferSize))
            End If

            m_lReturn = GetWTSSessionID(lSessionID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            sSystemName = sSystemName & "_sid" & CStr(lSessionID)

            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMELogLevel.PMLogOnError
            sSystemName = ""

            ' Log Error.
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get computer name", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSystemName", excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetSystemNameNoSID
    '
    ' Description: Gets the system (computer) name without the WTS SID
    '
    ' ***************************************************************** '
    Public Function GetSystemNameNoSID(ByRef sSystemName As String) As Integer

        Dim result As Integer = 0
        Dim iPos As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            If sSystemName.Trim() = "" Then
                ' SystemName not supplied, so get it
                m_lReturn = GetSystemName(sSystemName)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or sSystemName = "" Then
                    Return result
                End If
            End If

            ' search for SID
            iPos = IIf(sSystemName = "" And "_sid" = "", 0, (sSystemName.LastIndexOf("_sid") + 1))

            If iPos > 0 Then
                ' get rid of SID
                sSystemName = sSystemName.Substring(0, iPos - 1)
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get computer name (No SID)", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSystemNameNoSID", excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: ConvertWildCardsForSQL (Public)
    '
    ' Description: Converts '*' wildcards to '%' for SQL
    '
    ' History TF311097 Created
    ' ***************************************************************** '
    Public Function ConvertWildCardsForSQL(ByRef r_sTextString As String) As Integer

        Dim result As Integer = 0
        Dim sSearchText As String = ""

        Try

            sSearchText = r_sTextString

            ' Replace * with % wildcards
            While (sSearchText.IndexOf("*"c) >= 0)
                Mid(sSearchText, sSearchText.IndexOf("*"c) + 1, 1) = "%"
            End While

            ' Add implied wildcard to end
            If Not sSearchText.EndsWith("%") Then
                sSearchText = sSearchText & "%"
            End If

            r_sTextString = sSearchText


            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMELogLevel.PMLogOnError

            ' Log Error.
            LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Convert Wild Cards For SQL", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertWildCardsForSQL", excep:=excep)

            Return result

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

        Dim result As Boolean = False
        Dim sInDebug As String = ""

        Try


            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=gPMConstants.PMRegKeyArchitectureInDebug, r_sSettingValue:=sInDebug)

            ' Check for errors.
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (sInDebug.Trim() = "") Then
                ' Return False if Errors
                Return False
            End If

            ' Return result YES, Y or PMTrue are Yes

            Select Case sInDebug.Trim().ToUpper()
                Case "YES", "Y", CStr(gPMConstants.PMEReturnCode.PMTrue)
                    Return True
                Case Else
                    Return False
            End Select

        Catch
        End Try



        Return False

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

        Dim result As Integer = 0
        Dim sKeyString As String = ""
        Dim lRoot As Integer
        Dim UserName As String = ""
        Dim MachineName As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Current User OR Local Machine
            If v_lPMERegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser Then
                UserName = Environment.UserName
                lRoot = gPMConstants.HKEY_CURRENT_USER
            Else
                lRoot = gPMConstants.HKEY_LOCAL_MACHINE
                MachineName = Environment.MachineName
            End If

            ' Build up the key String
            sKeyString = BuildKeyString(v_ePMEProductFamily:=v_lPMEProductFamily, v_ePMERegSettingLevel:=v_lPMERegSettingLevel, v_sSubKey:=v_sSubKey)

            ' Do we have a key string
            If sKeyString.Trim() = "" Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Save the Value
            Dim oDataBase As New dPMDAO.Database
            oDataBase.SetValues(sKeyString, v_sSettingName, v_sSettingValue, UserName, MachineName)

            Return result

        Catch
            Return gPMConstants.PMEReturnCode.PMError
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
        Dim result As Integer = 0
        Dim sKeyString As String = ""
        Dim lRoot As Integer
        Dim vSettingValue As String = ""
        Dim UserName As String = ""
        Dim MachineName As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Current User OR Local Machine
            If v_lPMERegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser Then
                UserName = Environment.UserName
                lRoot = gPMConstants.HKEY_CURRENT_USER
            Else
                lRoot = gPMConstants.HKEY_LOCAL_MACHINE
                MachineName = Environment.MachineName
            End If

            ' Build up the key String
            sKeyString = BuildKeyString(v_ePMEProductFamily:=v_lPMEProductFamily, v_ePMERegSettingLevel:=v_lPMERegSettingLevel, v_sSubKey:=v_sSubKey)

            ' Do we have a key string
            If sKeyString.Trim() = "" Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Value
            Dim oDatabase = New dPMDAO.Database()
            m_lReturn = oDatabase.GetValues(sKeyString, v_sSettingName, vSettingValue, UserName, MachineName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If String.IsNullOrEmpty(vSettingValue) Then
                ' Return an Empty String
                r_sSettingValue = ""
            Else
                ' Otherwise, Return the Setting Value
                r_sSettingValue = vSettingValue.Trim()
            End If

            Return result

        Catch




            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' ***************************************************************** '
    '
    ' Name: FlipArray
    '
    ' Description: Swap columns and rows in a 2 dimensional array
    '
    ' History: DAK 11052000 DAK Created.
    '
    ' ***************************************************************** '
    Public Function FlipArray(ByRef r_vArray As Array) As Integer

        Dim result As Integer = 0
        Dim vArray As Array


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if parameter realy is an array
            If Not Information.IsArray(r_vArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Ensure array has at least 2 dimensions
            Information.Err().Clear()
            Dim lDimension As Integer = r_vArray.GetUpperBound(1)
            If Information.Err().Number <> 0 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Ensure array has no more than 2 dimensions
            lDimension = r_vArray.GetUpperBound(2)
            If Information.Err().Number = 0 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Try
                ' Dimension the output array
                vArray = Array.CreateInstance(GetType(Object), New Integer() {r_vArray.GetUpperBound(1) - r_vArray.GetLowerBound(1) + 1, r_vArray.GetUpperBound(0) - r_vArray.GetLowerBound(0) + 1}, New Integer() {r_vArray.GetLowerBound(1), r_vArray.GetLowerBound(0)})

                ' Flip the array
                For lRow As Integer = r_vArray.GetLowerBound(1) To r_vArray.GetUpperBound(1)
                    For lColumn As Integer = r_vArray.GetLowerBound(0) To r_vArray.GetUpperBound(0)

                        vArray(lRow, lColumn) = r_vArray(lColumn, lRow)
                    Next lColumn
                Next lRow

                ' Return the flipped array
                r_vArray = vArray

                Return result

            Catch




                Return gPMConstants.PMEReturnCode.PMError
            End Try

        Catch exc As System.Exception
            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try

    End Function

    ' ***************************************************************** '
    ' Private Methods
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Name: BuildKeyString (Private)
    '
    ' Description: Builds up the Key String for the Reg Setting
    ' ***************************************************************** '
    Public Function BuildKeyString(ByVal v_ePMEProductFamily As gPMConstants.PMEProductFamily, ByVal v_ePMERegSettingLevel As gPMConstants.PMERegSettingLevel, Optional ByVal v_sSubKey As String = "") As String

        Dim sKeyString As String = ""

        Try

            ' Build up the key String

            ' Start with Root
            sKeyString = gPMConstants.ACRegRoot

            ' Add PM Product
            Select Case v_ePMEProductFamily
                Case gPMConstants.PMEProductFamily.pmePFSiriusArchitecture
                    sKeyString = sKeyString & gPMConstants.ACRegSiriusArchitecture
                Case gPMConstants.PMEProductFamily.pmePFSiriusUnderwriting
                    sKeyString = sKeyString & gPMConstants.ACRegSiriusUnderwriting
                Case gPMConstants.PMEProductFamily.pmePFOrion
                    sKeyString = sKeyString & gPMConstants.ACRegOrion
                Case gPMConstants.PMEProductFamily.pmePFGemini
                    sKeyString = sKeyString & gPMConstants.ACRegGemini
                Case gPMConstants.PMEProductFamily.pmePFVoyager
                    sKeyString = sKeyString & gPMConstants.ACRegVoyager
                Case gPMConstants.PMEProductFamily.pmePFMercury
                    sKeyString = sKeyString & gPMConstants.ACRegMercury
                Case gPMConstants.PMEProductFamily.pmePFDocumaster
                    sKeyString = sKeyString & gPMConstants.ACRegDocumaster
                Case gPMConstants.PMEProductFamily.pmePFSiriusBroking
                    sKeyString = sKeyString & gPMConstants.ACRegSiriusBroking
                Case gPMConstants.PMEProductFamily.pmePFSiriusSolutions
                    sKeyString = sKeyString & gPMConstants.ACRegSiriusSolutions
                Case gPMConstants.PMEProductFamily.pmePFNirvana
                    sKeyString = sKeyString & gPMConstants.ACRegNirvana
                Case gPMConstants.PMEProductFamily.pmePFGeminiII
                    sKeyString = sKeyString & gPMConstants.ACRegGeminiII
                Case gPMConstants.PMEProductFamily.pmePFClaims
                    sKeyString = sKeyString & gPMConstants.ACRegClaims
                Case gPMConstants.PMEProductFamily.pmePFStargate
                    sKeyString = sKeyString & gPMConstants.ACRegStargate
                Case gPMConstants.PMEProductFamily.pmePFSwift
                    sKeyString = sKeyString & gPMConstants.ACRegSwift

            End Select

            ' Add Level
            Select Case v_ePMERegSettingLevel
                Case gPMConstants.PMERegSettingLevel.pmeRSLClient
                    sKeyString = sKeyString & gPMConstants.ACRegClient
                Case gPMConstants.PMERegSettingLevel.pmeRSLServer
                    sKeyString = sKeyString & gPMConstants.ACRegServer
                Case gPMConstants.PMERegSettingLevel.pmeRSLCommon
                    sKeyString = sKeyString & gPMConstants.ACRegCommon
                Case gPMConstants.PMERegSettingLevel.pmeRSLSetup
                    sKeyString = sKeyString & gPMConstants.ACRegSetup
                Case gPMConstants.PMERegSettingLevel.pmeRSLBase
                    sKeyString = sKeyString
                Case Else
                    sKeyString = sKeyString & gPMConstants.ACRegCommon
            End Select

            ' Has a Sub key been supplied
            If v_sSubKey <> "" Then

                ' Yes we have a sub key

                ' Add a separator if the Start of the sub key does not have one
                If v_sSubKey.Substring(0, 1) <> "\" Then
                    sKeyString = sKeyString & "\"
                End If

                ' Add the Sub Key
                sKeyString = sKeyString & v_sSubKey

                ' Remove a Trailing separator if there is one
                If sKeyString.EndsWith("\") Then
                    sKeyString = sKeyString.Substring(0, sKeyString.Length - 1)
                End If

            End If

            ' Return the string

            Return sKeyString

        Catch




            Return ""
        End Try

    End Function

    ' ***************************************************************** '
    '
    ' Name: TrimDate
    '
    ' Description: Assuming normal US format long date, drop day from
    '              start of input
    '
    ' History: DAK 23062000 Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (TrimDate) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function TrimDate(ByRef r_vDate As String) As Integer
    '
    'Dim result As Integer = 0
    'Dim lSub As Integer
    'Dim sDate As String = ""
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'sDate = r_vDate
    'For 'lSub = 1 To sDate.Length
    'If Mid(sDate, lSub, 1) = " " Then
    'lSub += 1
    'Exit For

    'Next lSub
    '
    'If lSub < sDate.Length Then
    'r_vDate = Mid(sDate, lSub, sDate.Length - lSub)

    '
    'Return result
    '
    'Catch 
    '
    '
    '
    '
    'Return gPMConstants.PMEReturnCode.PMError

    '



    ' ***************************************************************** '
    ' Name: FormatName
    '
    ' Description: format string into upper/lower-case, for real names
    '
    ' ***************************************************************** '
    Private Function FormatName(ByVal vString As String) As String

        Dim result As String = String.Empty
        Dim sLast, sChar, sSearch As String
        Dim vTemp As String = ""

        Try

            result = ""

            ' significant characters
            sSearch = " -'(."

            ' copy of input
            vTemp = vString.Trim()

            ' first char is always upper case
            Mid(vTemp, 1, 1) = Mid(vTemp, 1, 1).ToUpper()

            ' other characters
            For lLoop As Integer = 2 To vTemp.Length

                ' previous and current characters
                sLast = Mid(vTemp, lLoop - 1, 1)
                sChar = Mid(vTemp, lLoop, 1)

                ' previous char in search string?
                If sSearch.IndexOf(sLast) >= 0 Then
                    If sLast = "'" And lLoop > 3 Then
                        If sSearch.IndexOf(Mid(vTemp, lLoop - 3, 1)) >= 0 Then
                            sChar = sChar.ToUpper()
                        End If
                    Else
                        sChar = sChar.ToUpper()
                    End If
                End If

                ' replace character
                Mid(vTemp, lLoop, 1) = sChar

            Next


            Return vTemp

        Catch



            Return ""
        End Try

    End Function


    ' ***************************************************************** '
    '
    ' Name: replaceVbcrWithVbcrlf
    '
    ' Description: As name. Normal VB replace does not work for this
    '
    ' History: CLG 17082001 Created.
    '
    ' ***************************************************************** '
    Private Sub ReplaceVbCrWithVbCrLf(ByRef sString As String)

        Dim iLoop As Integer

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".replaceVbcrWithVbcrlf")



        'if already converted then do nothing
        If (sString.IndexOf(Strings.Chr(13) & Strings.Chr(10)) + 1) Then Exit Sub

        iLoop = (sString.IndexOf(Constants.vbLf) + 1)

        Do While iLoop > 0
            sString = sString.Substring(0, iLoop - 1) & Strings.Chr(13) & Strings.Chr(10) & Mid(sString, iLoop + 1)
            iLoop = Strings.InStr(iLoop + 2, sString, Constants.vbLf)
        Loop

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".replaceVbcrWithVbcrlf")



    End Sub

    Function SetValueEx(ByVal hKey As Integer, ByRef sValueName As String, ByRef lType As Integer, ByRef vValue As String) As Integer

        Dim result As Integer = 0
        Dim lValue As Integer
        Dim sValue As String = ""

        Select Case lType
            Case gPMConstants.REG_SZ
                sValue = vValue & Strings.Chr(0).ToString()
                result = RegSetValueExString(hKey, sValueName, 0, lType, sValue, sValue.Length)
            Case gPMConstants.REG_DWORD
                lValue = CInt(vValue)
                result = RegSetValueExLong(hKey, sValueName, 0, lType, lValue, 4)
        End Select

        Return result
    End Function

    Function QueryValueEx(ByVal lhKey As Integer, ByVal szValueName As String, ByRef vValue As String) As Integer

        Dim cch, lrc, lType, lValue As Integer
        Dim sValue As String = ""

        Try

            ' Determine the size and type of data to be read
            lrc = RegQueryValueExNULL(lhKey, szValueName, 0, lType, 0, cch)
            If lrc <> gPMConstants.ERROR_NONE Then Exit Function

            Select Case lType
                ' For strings
                Case gPMConstants.REG_SZ
                    sValue = New String(Strings.Chr(0), cch)
                    lrc = RegQueryValueExString(lhKey, szValueName, 0, lType, sValue, cch)

                    If lrc = gPMConstants.ERROR_NONE Then
                        vValue = sValue.Substring(0, cch - 1)
                    Else
                        vValue = String.Empty
                    End If

                    ' For DWORDS
                Case gPMConstants.REG_DWORD
                    lrc = RegQueryValueExLong(lhKey, szValueName, 0, lType, lValue, cch)

                    If lrc = gPMConstants.ERROR_NONE Then vValue = CStr(lValue)

                Case Else
                    'all other data types not supported
                    lrc = -1
            End Select

        Catch


        End Try

    End Function

    Sub CreateNewKey(ByRef lPredefinedKey As Integer, ByRef sNewKeyName As String)

        Dim hNewKey As Integer 'handle to the new key
        Dim lRetVal As Integer 'result of the RegCreateKeyEx function

        lRetVal = RegCreateKeyEx(lPredefinedKey, sNewKeyName, 0, Nothing, gPMConstants.REG_OPTION_NON_VOLATILE, gPMConstants.EL_KEY_ALL_ACCESS, 0, hNewKey, lRetVal)

        RegCloseKey(hNewKey)

    End Sub

    Sub SetKeyValue(ByRef lPredefinedKey As Integer, ByRef sKeyName As String, ByRef sValueName As String, ByRef vValueSetting As String, ByRef lValueType As Integer)

        Dim hKey As Integer 'handle of open key

        'open the specified key
        m_lReturn = RegOpenKeyEx(lPredefinedKey, sKeyName, 0, gPMConstants.EL_KEY_ALL_ACCESS, hKey)

        m_lReturn = SetValueEx(hKey, sValueName, lValueType, vValueSetting)

        RegCloseKey(hKey)

    End Sub

    Function QueryKeyValue(ByRef lPredefinedKey As Integer, ByRef sKeyName As String, ByRef sValueName As String) As String

        Dim result As String = String.Empty
        Dim hKey As Integer 'handle of opened key
        Dim vValue As String = "" 'setting of queried value

        m_lReturn = RegOpenKeyEx(lPredefinedKey, sKeyName, 0, gPMConstants.REG_KEY_READ, hKey)

        m_lReturn = QueryValueEx(hKey, sValueName, vValue)

        result = vValue
        RegCloseKey(hKey)

        Return result
    End Function

    Function KeyExists(ByRef lPredefinedKey As Integer, ByRef sKeyName As String) As Boolean

        Dim result As Boolean = False
        Dim hKey As Integer 'handle of opened key



        m_lReturn = RegOpenKeyEx(lPredefinedKey, sKeyName, 0, gPMConstants.REG_KEY_READ, hKey)

        result = m_lReturn = gPMConstants.ERROR_NONE

        RegCloseKey(hKey)

        Return result
    End Function

    Function DeleteKey(ByRef lPredefinedKey As Integer, ByRef sKeyName As String) As Integer

        m_lReturn = RegDeleteKey(lPredefinedKey, sKeyName)

        Return m_lReturn = gPMConstants.ERROR_NONE

    End Function

    Function DeleteKeyValue(ByRef lPredefinedKey As Integer, ByRef sKeyName As String, ByRef sValueName As String) As Integer

        Dim hKey As Integer

        m_lReturn = RegOpenKeyEx(lPredefinedKey, sKeyName, 0, gPMConstants.EL_KEY_ALL_ACCESS, hKey)

        m_lReturn = RegDeleteValue(hKey, sValueName)

        Return m_lReturn = gPMConstants.ERROR_NONE

    End Function

    ' ***************************************************************** '
    ' Name: GetNTUserName
    '
    ' Description: get the NT account name of the currently logged on user
    '
    ' History: RDC 11072002 created
    '
    ' ***************************************************************** '
    Public Function GetNTUsername(ByRef sNTUsername As String) As Integer

        Dim result As Integer = 0
        Dim sName = "", sUsername As String

        Const BUFFER_LENGTH As Integer = 255
        Const STATUS_NOERROR As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sNTUsername = ""

            sUsername = New String(" "c, BUFFER_LENGTH + 1)

            m_lReturn = WNetGetUser(sName, sUsername, BUFFER_LENGTH)

            If m_lReturn <> STATUS_NOERROR Then
                Return result
            End If

            sNTUsername = sUsername.Substring(0, sUsername.IndexOf(Strings.Chr(0).ToString()))


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: GetNTUserNameEx
    '
    ' Description:  get the fully qualified NT account name
    '               of the currently logged on user
    '
    ' History: RDC 12072002 created
    '
    ' ***************************************************************** '
    Public Function GetNTUsernameEx(ByRef sUsername As String) As Integer

        Dim result As Integer = 0
        Dim lLen As Integer
        Dim sName As String = ""

        Const BUFFER_LENGTH As Integer = 255
        Const RETURN_ERROR As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            sName = New String(" "c, BUFFER_LENGTH + 1)
            lLen = sName.Length

            m_lReturn = GetUserNameEx(EXTENDED_NAME_FORMAT.NameSamCompatible, sName, lLen)

            If m_lReturn = RETURN_ERROR Then
                Return result
            End If

            ' On Windows2000 and XP this buffer is returned with a null string terminator
            ' but on Windows Server 2003, for some reason it is returned without it.
            ' So we only want to remove the null string terminator if it is there
            If Strings.Asc(sName.Substring(lLen - 1, 1)(0)) <= 0 Then
                sUsername = sName.Substring(0, lLen - 1)
            Else
                sUsername = sName.Substring(0, lLen)
            End If



            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: GetSystemInfo
    '
    ' Description:  get system version info
    '
    ' History: RDC 12072002 created
    '
    ' ***************************************************************** '
    Public Function GetSystemInfo(ByRef vSysInfo(,) As Object) As Integer


        Dim result As Integer = 0
        Dim OSInfo As OSVERSIONINFO = OSVERSIONINFO.CreateInstance()
        Dim PId As String

        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            OSInfo.dwOSVersionInfoSize = Marshal.SizeOf(OSInfo)

            'Get the Windows version
            m_lReturn = GetVersionEx(OSInfo)

            'Check for errors
            If m_lReturn = 0 Then
                Return result
            End If

            ReDim vSysInfo(1, 9)


            vSysInfo(0, 0) = "PlatformName"

            vSysInfo(1, 0) = "MajorVersion"

            vSysInfo(2, 0) = "MinorVersion"

            vSysInfo(3, 0) = "BuildNumber"

            Select Case OSInfo.dwPlatformId
                Case 0
                    PId = "Windows 32s"
                Case 1
                    PId = "Windows 95/98"
                Case 2
                    PId = "Windows NT"
            End Select


            vSysInfo(0, 1) = PId

            vSysInfo(1, 1) = OSInfo.dwMajorVersion

            vSysInfo(2, 1) = OSInfo.dwMinorVersion

            vSysInfo(3, 1) = OSInfo.dwBuildNumber


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    Private ReadOnly WTS_CURRENT_SERVER_HANDLE As IntPtr = IntPtr.Zero
    Public Function GetWTSSessionID(ByRef lSessionID As Integer) As Integer

        Dim pSessionInfo As IntPtr = IntPtr.Zero
        Dim nReturned As Integer = 0

        Const WTS_CURRENT_SESSION_HANDLE As Integer = -1
        Const RETURN_FAIL As Integer = 0
        Dim result As Integer = 0

        Try
            result = WTSQuerySessionInformation(WTS_CURRENT_SERVER_HANDLE, WTS_CURRENT_SESSION_HANDLE, WTS_INFO_CLASS.WTSSessionID, pSessionInfo, nReturned)

            If result = RETURN_FAIL Then
                lSessionID = 0
                Return result
            Else
                lSessionID = Marshal.ReadInt32(pSessionInfo)
                WTSFreeMemory(pSessionInfo)
            End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch
            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: NullToXXX Functions
    '
    ' Date: 24th July 2002
    '
    ' Description: This is a group of functions to check for and default
    '              Null values in the supplied expressions
    '
    ' Author: Peter Finney
    '
    ' Note: There is no error handling in these functions as we want any
    '       type conversion errors to bubble up and be reported from the
    '       point the offending data was supplied.
    '
    ' ***************************************************************** '
    Public Function NullToBoolean(ByVal Expression As Object) As Boolean

        ' Check for null, else convert to boolean

        If Convert.IsDBNull(Expression) Or IsNothing(Expression) Then
            Return NullToBooleanDefault
        Else

            Return CBool(Expression)
        End If

    End Function

    Public Function NullToDate(ByVal Expression As Object) As Date

        ' Check for null, else convert to date

        If Convert.IsDBNull(Expression) Or IsNothing(Expression) Then
            Return NullToDateDefault
        Else

            Return CDate(Expression)
        End If

    End Function

    Public Function NullToDecimal(ByVal Expression As Object) As Decimal

        ' Check for null, else convert to decimal

        If Convert.IsDBNull(Expression) Or IsNothing(Expression) Then
            ' There is no native decimal type so we must convert
            ' the default as well
            Return NullToDecimalDefault
        Else

            Return CDec(Expression)
        End If

    End Function

    Public Function NullToLong(ByVal Expression As Object) As Integer

        ' Check for null, else convert to long

        If Convert.IsDBNull(Expression) Or IsNothing(Expression) Then
            Return NullToLongDefault
        Else

            Return CInt(Expression)
        End If

    End Function

    Public Function NullToString(ByVal Expression As Object) As String

        ' Check for null, else convert to string

        If Convert.IsDBNull(Expression) Or IsNothing(Expression) Then
            Return NullToStringDefault
        Else

            Return CStr(Expression)
        End If

    End Function

    Public Function NullToInteger(ByVal Expression As Object) As Integer


        If Convert.IsDBNull(Expression) Or IsNothing(Expression) Then
            Return NullToIntegerDefault
        Else

            Return CInt(Expression)
        End If

    End Function

    Public Function NullToDouble(ByVal Expression As Object) As Double

        If Convert.IsDBNull(Expression) Or IsNothing(Expression) Then
            Return NullToDoubleDefault
        Else

            Dim dbNumericTemp As Double
            If Double.TryParse(CStr(Expression), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                Return CDbl(Expression)
            Else
                Return NullToDoubleDefault
            End If
        End If

    End Function

    Public Function NullToCurrency(ByVal Expression As Object) As Decimal


        If Convert.IsDBNull(Expression) Or IsNothing(Expression) Then
            Return NullToCurrencyDefault
        Else

            Return CDec(Expression)
        End If

    End Function
    ' ***************************************************************** '
    ' End Of NullToXXX Functions
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Name: EventLogWrite
    '
    ' Description: Write message to o/s event log
    '
    ' History: RDC 29072002
    '
    ' ***************************************************************** '
    Public Function EventLogWrite(ByVal iType As Integer, ByVal sMsg As String, ByRef oDicParms As Dictionary(Of String, Object), ByVal sErrUniqueId As String, ByVal excep As Exception, Optional ByRef vUsername As String = "", Optional ByRef vCallingApp As String = "", Optional ByRef vApp As String = "", Optional ByRef vClass As String = "", Optional ByRef vMethod As String = "", Optional ByRef vErrNo As String = "", Optional ByRef vErrDesc As String = "", Optional ByRef vBinaryData As String = "", Optional ByRef vServerName As String = "", Optional ByRef vTraceEventType As TraceEventType = TraceEventType.Error) As Integer
        Dim slogmessage As String = ""
        Dim ifilenumber As Integer
        Dim slogfile = "", smsglogging = "", suserloglevel As String = ""
        Dim iuserloglevel As gPMConstants.PMELogLevel
        Try

            If vUsername.Trim() = "" Then
                iuserloglevel = gPMConstants.PMELogLevel.PMLogOnError
            Else
                ' hkey_current_user\software\pm\siriusarchitecture\common
                m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=gPMConstants.PMRegKeyLogLevel, r_sSettingValue:=suserloglevel)
                Dim dbnumerictemp As Double
                If Double.TryParse(suserloglevel, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbnumerictemp) Then
                    iuserloglevel = CType(CInt(suserloglevel), gPMConstants.PMELogLevel)
                Else
                    iuserloglevel = gPMConstants.PMELogLevel.PMLogOnError
                End If
            End If

            ' if its high enough priority, log it.
            If (iType <= iuserloglevel) Or (iType <= gPMConstants.PMELogLevel.PMLogOnError) Then
                If sErrUniqueId.Trim = "" Then
                    sErrUniqueId = GenerateUniqueSSPExceptionRef(gPMConstants.ERROR_NO_LENGTH)
                End If
                ' rdc 30082002 messages to event log or text files?
                m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=gPMConstants.PMRegKeyUseEventLog, r_sSettingValue:=smsglogging)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    smsglogging = "1"
                End If
                If smsglogging = "1" Then
                    ' we're writing to the o/s application event log
                    sMsg = sMsg & Chr(13) & Chr(10) & Chr(13) & Chr(10)
                    If sErrUniqueId <> "" Then sMsg = sMsg & sErrUniqueId & Strings.Chr(13) & Strings.Chr(10)
                    If vUsername <> "" Then sMsg = sMsg & "Username:" & vUsername & Strings.Chr(13) & Strings.Chr(10)
                    If vCallingApp <> "" Then sMsg = sMsg & "Calling app:" & vCallingApp & Strings.Chr(13) & Strings.Chr(10)
                    If vApp <> "" Then sMsg = sMsg & "App:" & vApp & Strings.Chr(13) & Strings.Chr(10)
                    If vClass <> "" Then sMsg = sMsg & "Class:" & vClass & Strings.Chr(13) & Strings.Chr(10)
                    If vMethod <> "" Then sMsg = sMsg & "Method:" & vMethod & Strings.Chr(13) & Strings.Chr(10)
                    If vErrNo <> "" Then sMsg = sMsg & "VB Error:" & vErrNo & Strings.Chr(13) & Strings.Chr(10)
                    If vErrDesc <> "" Then sMsg = sMsg & "VB Description:" & vErrDesc & Strings.Chr(13) & Strings.Chr(10)

                    sMsg = sMsg & Chr(13) & Chr(10)
                    If excep IsNot Nothing AndAlso excep.Message IsNot Nothing Then
                        sMsg = sMsg & excep.Message
                        sMsg = sMsg & Chr(13) & Chr(10)
                    End If
                    If excep IsNot Nothing AndAlso excep.StackTrace IsNot Nothing Then
                        sMsg = sMsg & excep.StackTrace
                        sMsg = sMsg & Chr(13) & Chr(10)
                    End If

                    ' in EntLib4
                    CreateManualWriter()

                    If Not EventLog.SourceExists(EVENT_LOG_APP_NAME, ".") Then
                        Dim creationData As New EventSourceCreationData(EVENT_LOG_APP_NAME, EVENT_LOG_FILE_NAME)
                        creationData.MachineName = "."
                        EventLog.CreateEventSource(creationData)
                    End If
                    Dim logEntry As New LogEntry()
                    logEntry.Priority = iType
                    If excep IsNot Nothing OrElse vErrDesc <> "" Then
                        logEntry.Severity = vTraceEventType
                    Else
                        logEntry.Severity = TraceEventType.Verbose
                    End If
                    logEntry.Message = sMsg
                    logEntry.ExtendedProperties = oDicParms
                    m_LogWriter.Write(logEntry)
                    Return gPMConstants.PMEReturnCode.PMTrue
                    Exit Function
                End If

                ' get user specific log file if there is one
                ' get the log file name setting from
                ' hkey_current_user\software\pm\siriusarchitecture\common
                m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=gPMConstants.PMRegKeyLogFile, r_sSettingValue:=slogfile)
                ' if there is no user specific log file
                If slogfile.Trim() = "" Then
                    ' get machine default
                    ' get the log file name setting from
                    ' hkey_local_machine\software\pm\siriusarchitecture\common
                    m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=gPMConstants.PMRegKeyLogFile, r_sSettingValue:=slogfile)
                End If

                ' check for errors.
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (slogfile.Trim() = "") Then
                    ' if there was no log file in the registry,
                    ' use the default one.
                    slogfile = gPMConstants.PMDefaultLogFile
                End If

                ' get the next free file number
                ifilenumber = FileSystem.FreeFile()

                ' open the log file for append
                FileSystem.FileOpen(ifilenumber, slogfile, OpenMode.Append)

                ' set the log message to the current date/time
                slogmessage = "date / time     : " & DateTime.Now.ToString

                ' append the message type
                slogmessage = slogmessage & Environment.NewLine & "type            : "

                Select Case iType
                    Case gPMConstants.PMELogLevel.PMLogDebug1
                        slogmessage = slogmessage & "pmlogdebug1"
                    Case gPMConstants.PMELogLevel.PMLogDebug2
                        slogmessage = slogmessage & "pmlogdebug2"
                    Case gPMConstants.PMELogLevel.PMLogDebug3
                        slogmessage = slogmessage & "pmlogdebug3"
                    Case gPMConstants.PMELogLevel.PMLogDebug4
                        slogmessage = slogmessage & "pmlogdebug4"
                    Case gPMConstants.PMELogLevel.PMLogError
                        slogmessage = slogmessage & "pmlogerror"
                    Case gPMConstants.PMELogLevel.PMLogFatal
                        slogmessage = slogmessage & "pmlogfatal"
                    Case gPMConstants.PMELogLevel.PMLogFeedback
                        slogmessage = slogmessage & "pmlogfeedback"
                    Case gPMConstants.PMELogLevel.PMLogInfo
                        slogmessage = slogmessage & "pmloginfo"
                    Case gPMConstants.PMELogLevel.PMLogOnError
                        slogmessage = slogmessage & "pmlogonerror"
                    Case gPMConstants.PMELogLevel.PMLogWarning
                        slogmessage = slogmessage & "pmlogwarning"
                    Case Else
                        slogmessage = slogmessage & "unknown : " & CStr(iType)
                End Select

                ' add unique  error number if we have them
                slogmessage = slogmessage & Environment.NewLine & sErrUniqueId & Strings.Chr(13) & Strings.Chr(10)

                ' append the username
                slogmessage = slogmessage & Environment.NewLine & "username        : " & vUsername

                ' add on the optional parameters if they are present
                If Not (False) Then
                    slogmessage = slogmessage & Environment.NewLine & "application     : " & vApp
                End If

                If Not (False) Then
                    slogmessage = slogmessage & Environment.NewLine & "class           : " & vClass
                End If

                If Not (False) Then
                    slogmessage = slogmessage & "." & vMethod
                End If

                ' add the message to the end
                slogmessage = slogmessage & Environment.NewLine & "message         : " & sMsg

                'If Not (False) Then
                '    slogmessage = slogmessage & Environment.NewLine & "err.no          : " & vErrNo
                'End If

                'If Not (False) Then
                '    slogmessage = slogmessage & Environment.NewLine & "err.description : " & vErrDesc
                'End If

                If excep IsNot Nothing Then
                    slogmessage = slogmessage & Environment.NewLine & "ex message : " & excep.Message
                End If

                If excep IsNot Nothing Then
                    slogmessage = slogmessage & Environment.NewLine & "stacktrace : " & excep.StackTrace
                End If

                slogmessage = slogmessage & Environment.NewLine & New String("*", 80)

                ' print the log message to the log file
                FileSystem.PrintLine(ifilenumber, slogmessage)

                ' close the log file
                FileSystem.FileClose(ifilenumber)
            End If
            Return gPMConstants.PMEReturnCode.PMTrue
        Catch
            Return gPMConstants.PMEReturnCode.PMError
        End Try
    End Function

    Public Sub CreateManualWriter()
        Dim GeneralCategory As String = "General"
        Dim ErrorCategory As String = "Errors"
        Dim formatter = New TextFormatter("Timestamp: {timestamp}{newline}" + "Category: {category}{newline}" + "Message: {message}{newline}" + "Extended Properties: {dictionary({key} - {value}{newline})}")

        ' Log messages to event log 
        Dim logListener = New FormattedEventLogTraceListener(EVENT_LOG_APP_NAME, EVENT_LOG_FILE_NAME, ".", formatter)

        'this source has our listener
        Dim mainLogSource = New LogSource(GeneralCategory, SourceLevels.All)
        mainLogSource.Listeners.Add(logListener)

        ' Don't log to this source
        Dim emptyLogSource = New LogSource("Empty")

        ' "Error" category goes to main log source
        Dim traceSources = New Dictionary(Of String, LogSource)() From {
            {GeneralCategory, mainLogSource}
        }

        ' filter "Error" category
        Dim categoryFilter = New CategoryFilter("All", New List(Of String)() From {
            GeneralCategory
        }, CategoryFilterMode.DenyAllExceptAllowed)
        Dim filters = New List(Of ILogFilter)() From {
            categoryFilter
        }

        ' in EntLib5 can't use LogWriter (it's abstract) or LogWriterFactory (which uses IServiceLocator)
        'The collection of filters to use when processing an entry
        'The trace sources to dispatch entries to
        'The special LogSource to which all log entries should be logged.
        'The special LogSource to which log entries with at least one non-matching category should be logged
        'The special LogSource to which internal errors must be logged
        'The default category to set when entry categories list of a log entry is empty
        'The tracing status
        'true if warnings should be logged when a non-matching category is found
        m_LogWriter = New LogWriterImpl(filters, traceSources, mainLogSource, mainLogSource, mainLogSource, GeneralCategory,
            True, True)
    End Sub

    ' ***************************************************************** '
    ' Name: WriteEventLog
    '
    ' Description: Write message to o/s event log
    '
    ' History: RDC 29072002
    '
    ' ***************************************************************** '
    Private Function WriteEventLog(ByVal sServName As String, ByVal sAppName As String, ByVal eLogType As gPMConstants.enmLogType, ByVal lEventID As Integer, ByVal sEventLogName As String, ByRef sEventText() As String) As Integer

        Dim result As Integer = 0
        Dim lRtn, lRegEvntLogHwnd, lUserSID, lNumMessages As Integer
        Dim sServerName As String

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Add the leading '\\' to remote server names
            If sServName.Trim() = "" Then
                sServerName = Nothing
            Else
                If Not sServName.Trim().StartsWith("\\") Then
                    sServerName = "\\" & sServName
                Else
                    sServerName = sServName
                End If
            End If

            ' Get the number of messages
            lNumMessages = TranslateArray(sEventText, m_typMessages)

            ' Register the event log
            lRegEvntLogHwnd = RegisterEventSource(lpUNCServerName:=sServerName, lpSourceName:=sAppName)

            If lRegEvntLogHwnd = 0 Then
                lRtn = Information.Err().LastDllError

                m_lErrNum = (gPMConstants.ERR_REG_EVENT_SOURCE + Constants.vbObjectError)
                m_sErrSource = gPMConstants.REG_SOURCENAME & "WriteEventLog"
                m_sErrDesc = "Could not RegisterEventSource for " &
                             sAppName & ".  The error was: " &
                             ReturnAPIErrString(lRtn)

                Return result
            End If

            ' Get the user of the current process's SID and use
            ' that to insert this user's name in the log item
            lUserSID = GetSID()

            ' Write to the application event log
            Dim handle As GCHandle = GCHandle.Alloc(m_abytDataBuffer, GCHandleType.Pinned)
            Dim handle2 As GCHandle = GCHandle.Alloc(m_typMessages, GCHandleType.Pinned)
            Try
                Dim tmpPtr2 As IntPtr = handle2.AddrOfPinnedObject()

                Dim tmpPtr As IntPtr = New IntPtr(handle.AddrOfPinnedObject().ToInt32() + Marshal.SizeOf(m_abytDataBuffer(0)) * 0)
                lRtn = ReportEvent(hEventLog:=lRegEvntLogHwnd, wType:=eLogType, wCategory:=m_lCategory, dwEventID:=lEventID, lpUserSid:=lUserSID, wNumStrings:=lNumMessages, dwDataSize:=m_lLenRawData, lpStrings:=tmpPtr2, lpRawData:=tmpPtr)
            Finally
                handle.Free()
                handle2.Free()
            End Try

            ' Unlock and deallocate the memory used by SID
            DeallocateSID(lUserSID:=lUserSID)

            ' Check for errors
            If Not lRtn Then
                lRtn = Information.Err().LastDllError

                Return result
            End If

            ' Cleanup -- DeRegister the event log
            lRtn = DeregisterEventSource(lRegEvntLogHwnd)

            If Not lRtn Then
                lRtn = Information.Err().LastDllError

                Return result
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: TranslateArray
    '
    ' Description: Event logging support
    '
    ' History: RDC 29072002
    '
    ' ***************************************************************** '
    Private Function TranslateArray(ByRef args() As String, ByRef udt As gPMConstants.FmtMsgArrayType) As Integer

        Dim lCounter As Integer


        Dim lLower As Integer = 0
        Dim lUpper As Integer = 0
        Try

            lLower = args.GetLowerBound(0)
            lUpper = args.GetUpperBound(0)

        Catch exc As System.Exception
            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try


        If lLower < 0 Then
            Exit Function
        ElseIf lLower > lUpper Then
            Exit Function
        End If

        For lLoop As Integer = lLower To lUpper
            lCounter += 1


            Select Case lCounter
                Case 1 : udt.s1 = args(lLoop)
                Case 2 : udt.s2 = args(lLoop)
                Case 3 : udt.s3 = args(lLoop)
                Case 4 : udt.s4 = args(lLoop)
                Case 5 : udt.s5 = args(lLoop)
                Case 6 : udt.s6 = args(lLoop)
                Case 7 : udt.s7 = args(lLoop)
                Case 8 : udt.s8 = args(lLoop)
                Case 9 : udt.s9 = args(lLoop)
                Case 10 : udt.s10 = args(lLoop)
                Case 11 : udt.s11 = args(lLoop)
                Case 12 : udt.s12 = args(lLoop)
                Case 13 : udt.s13 = args(lLoop)
                Case 14 : udt.s14 = args(lLoop)
                Case 15 : udt.s15 = args(lLoop)
                Case 16 : udt.s16 = args(lLoop)
                Case 17 : udt.s17 = args(lLoop)
                Case 18 : udt.s18 = args(lLoop)
                Case 19 : udt.s19 = args(lLoop)
                Case 20 : udt.s20 = args(lLoop)
                Case 21 : udt.s21 = args(lLoop)
                Case 22 : udt.s22 = args(lLoop)
                Case 23 : udt.s23 = args(lLoop)
                Case 24 : udt.s24 = args(lLoop)
                Case 25 : udt.s25 = args(lLoop)
                Case 26 : udt.s26 = args(lLoop)
                Case 27 : udt.s27 = args(lLoop)
                Case 28 : udt.s28 = args(lLoop)
                Case 29 : udt.s29 = args(lLoop)
                Case 30 : udt.s30 = args(lLoop)
                Case 31 : udt.s31 = args(lLoop)
                Case 32 : udt.s32 = args(lLoop)
                Case 33 : udt.s33 = args(lLoop)
                Case 34 : udt.s34 = args(lLoop)
                Case 35 : udt.s35 = args(lLoop)
                Case 36 : udt.s36 = args(lLoop)
                Case 37 : udt.s37 = args(lLoop)
                Case 38 : udt.s38 = args(lLoop)
                Case 39 : udt.s39 = args(lLoop)
                Case 40 : udt.s40 = args(lLoop)
                Case 41 : udt.s41 = args(lLoop)
                Case 42 : udt.s42 = args(lLoop)
                Case 43 : udt.s43 = args(lLoop)
                Case 44 : udt.s44 = args(lLoop)
                Case 45 : udt.s45 = args(lLoop)
                Case 46 : udt.s46 = args(lLoop)
                Case 47 : udt.s47 = args(lLoop)
                Case 48 : udt.s48 = args(lLoop)
                Case 49 : udt.s49 = args(lLoop)
                Case 50 : udt.s50 = args(lLoop)
                Case 51 : udt.s51 = args(lLoop)
                Case 52 : udt.s52 = args(lLoop)
                Case 53 : udt.s53 = args(lLoop)
                Case 54 : udt.s54 = args(lLoop)
                Case 55 : udt.s55 = args(lLoop)
                Case 56 : udt.s56 = args(lLoop)
                Case 57 : udt.s57 = args(lLoop)
                Case 58 : udt.s58 = args(lLoop)
                Case 59 : udt.s59 = args(lLoop)
                Case 60 : udt.s60 = args(lLoop)
                Case 61 : udt.s61 = args(lLoop)
                Case 62 : udt.s62 = args(lLoop)
                Case 63 : udt.s63 = args(lLoop)
                Case 64 : udt.s64 = args(lLoop)
                Case 65 : udt.s65 = args(lLoop)
                Case 66 : udt.s66 = args(lLoop)
                Case 67 : udt.s67 = args(lLoop)
                Case 68 : udt.s68 = args(lLoop)
                Case 69 : udt.s69 = args(lLoop)
                Case 70 : udt.s70 = args(lLoop)
                Case 71 : udt.s71 = args(lLoop)
                Case 72 : udt.s72 = args(lLoop)
                Case 73 : udt.s73 = args(lLoop)
                Case 74 : udt.s74 = args(lLoop)
                Case 75 : udt.s75 = args(lLoop)
                Case 76 : udt.s76 = args(lLoop)
                Case 77 : udt.s77 = args(lLoop)
                Case 78 : udt.s78 = args(lLoop)
                Case 79 : udt.s79 = args(lLoop)
                Case 80 : udt.s80 = args(lLoop)
                Case 81 : udt.s81 = args(lLoop)
                Case 82 : udt.s82 = args(lLoop)
                Case 83 : udt.s83 = args(lLoop)
                Case 84 : udt.s84 = args(lLoop)
                Case 85 : udt.s85 = args(lLoop)
                Case 86 : udt.s86 = args(lLoop)
                Case 87 : udt.s87 = args(lLoop)
                Case 88 : udt.s88 = args(lLoop)
                Case 89 : udt.s89 = args(lLoop)
                Case 90 : udt.s90 = args(lLoop)
                Case 91 : udt.s91 = args(lLoop)
                Case 92 : udt.s92 = args(lLoop)
                Case 93 : udt.s93 = args(lLoop)
                Case 94 : udt.s94 = args(lLoop)
                Case 95 : udt.s95 = args(lLoop)
                Case 96 : udt.s96 = args(lLoop)
                Case 97 : udt.s97 = args(lLoop)
                Case 98 : udt.s98 = args(lLoop)
                Case 99 : udt.s99 = args(lLoop)
            End Select

        Next

        ' Set the return value
        Return lCounter

    End Function

    ' ***************************************************************** '
    ' Name: ReturnAPIErrString
    '
    ' Description: Event logging support
    '
    ' History: RDC 29072002
    '
    ' ***************************************************************** '
    Private Function ReturnAPIErrString(ByRef ErrorCode As Integer) As String

        Dim sBuffer As String = ""
        Dim lHwndModule, lFlags As Integer


        Try

            ' Separate handling for network errors netmsg.dll
            If ErrorCode >= gPMConstants.NERR_BASE And ErrorCode <= gPMConstants.MAX_NERR Then

                lHwndModule = LoadLibraryEx(lpLibFileName:="netmsg.dll", hFile:=0, dwFlags:=gPMConstants.LOAD_LIBRARY_AS_DATAFILE)

                If lHwndModule <> 0 Then

                    lFlags = gPMConstants.FORMAT_MESSAGE_FROM_SYSTEM Or gPMConstants.FORMAT_MESSAGE_IGNORE_INSERTS Or gPMConstants.FORMAT_MESSAGE_FROM_HMODULE

                    ' Allocate the string, then get the system to tell us the error
                    ' message associated with this error number
                    sBuffer = New String(Strings.Chr(0), 256)

                    Dim handle As GCHandle = GCHandle.Alloc(lHwndModule, GCHandleType.Pinned)
                    Try
                        Dim tmpPtr As IntPtr = handle.AddrOfPinnedObject()
                        FormatMessage(dwFlags:=lFlags, lpSource:=tmpPtr, dwMessageId:=ErrorCode, dwLanguageId:=0, lpBuffer:=sBuffer, nSize:=sBuffer.Length, Arguments:=0)
                        lHwndModule = Marshal.ReadInt32(tmpPtr)
                    Finally
                        handle.Free()
                    End Try

                    ' Strip the last null, then the last CrLf pair if it exists
                    sBuffer = sBuffer.Substring(0, sBuffer.IndexOf(Strings.Chr(0)))

                    If sBuffer.Substring(sBuffer.Length - 2) = Strings.Chr(13).ToString() & Strings.Chr(10).ToString() Then
                        sBuffer = sBuffer.Substring(0, sBuffer.Length - 2)
                    End If

                    FreeLibrary(hLibModule:=lHwndModule)
                End If

                ' Separate handling for Wininet error Wininet.dll
            ElseIf ErrorCode >= gPMConstants.INTERNET_ERROR_BASE And ErrorCode <= gPMConstants.INTERNET_ERROR_LAST Then

                ' Load the library
                lHwndModule = LoadLibraryEx(lpLibFileName:="Wininet.dll", hFile:=0, dwFlags:=gPMConstants.LOAD_LIBRARY_AS_DATAFILE)

                If lHwndModule <> 0 Then

                    lFlags = gPMConstants.FORMAT_MESSAGE_FROM_SYSTEM Or gPMConstants.FORMAT_MESSAGE_IGNORE_INSERTS Or gPMConstants.FORMAT_MESSAGE_FROM_HMODULE

                    ' Allocate the string, then get the  system to tell us the error
                    ' message associated with this error number
                    sBuffer = New String(Strings.Chr(0), 256)

                    Dim handle2 As GCHandle = GCHandle.Alloc(lHwndModule, GCHandleType.Pinned)
                    Try
                        Dim tmpPtr2 As IntPtr = handle2.AddrOfPinnedObject()
                        FormatMessage(dwFlags:=lFlags, lpSource:=tmpPtr2, dwMessageId:=ErrorCode, dwLanguageId:=0, lpBuffer:=sBuffer, nSize:=sBuffer.Length, Arguments:=0)
                        lHwndModule = Marshal.ReadInt32(tmpPtr2)
                    Finally
                        handle2.Free()
                    End Try

                    ' Strip the last null, then the last CrLf pair if it exists
                    sBuffer = sBuffer.Substring(0, sBuffer.IndexOf(Strings.Chr(0)))
                    If sBuffer.Substring(sBuffer.Length - 2) = Strings.Chr(13).ToString() & Strings.Chr(10).ToString() Then
                        sBuffer = sBuffer.Substring(0, sBuffer.Length - 2)
                    End If

                    FreeLibrary(hLibModule:=lHwndModule)
                End If

                ' Wasn't Wininet or NetMsg, so do the standard API error look-up
            Else
                ' Allocate the string, then get the system to tell us the error message
                ' associated with this error number
                sBuffer = New String(Strings.Chr(0), 256)
                lFlags = gPMConstants.FORMAT_MESSAGE_FROM_SYSTEM Or gPMConstants.FORMAT_MESSAGE_IGNORE_INSERTS

                Dim handle3 As GCHandle = GCHandle.Alloc(0, GCHandleType.Pinned)
                Try
                    Dim tmpPtr3 As IntPtr = handle3.AddrOfPinnedObject()
                    FormatMessage(dwFlags:=lFlags, lpSource:=tmpPtr3, dwMessageId:=ErrorCode, dwLanguageId:=0, lpBuffer:=sBuffer, nSize:=sBuffer.Length, Arguments:=0)
                Finally
                    handle3.Free()
                End Try

                ' Strip the last null, then the last CrLf pair if it exists
                sBuffer = sBuffer.Substring(0, sBuffer.IndexOf(Strings.Chr(0)))

                If sBuffer.Substring(sBuffer.Length - 2) = Strings.Chr(13).ToString() & Strings.Chr(10).ToString() Then
                    sBuffer = sBuffer.Substring(0, sBuffer.Length - 2)
                End If
            End If

            ' Set the return value
            Return sBuffer

        Catch exc As System.Exception
            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: GetSID
    '
    ' Description: Event logging support
    '
    ' History: RDC 29072002
    '
    ' ***************************************************************** '
    Private Function GetSID() As Integer

        Dim result As Integer = 0
        Const FIRST_ATTEMPT As Integer = 16

        Dim lToken As Integer
        Dim atypTokenInfo() As gPMConstants.TOKEN_USER = Nothing
        Dim lLength, lRtn, lPtrSid, lLenSid, lPtrHeap As Integer

        ' Default to failure

        If OpenProcessToken(GetCurrentProcess(), gPMConstants.TOKEN_QUERY, lToken) <> 0 Then
            If lToken <> 0 Then
                ReDim atypTokenInfo(FIRST_ATTEMPT - 1)

                Dim handle As GCHandle = GCHandle.Alloc(atypTokenInfo(1), GCHandleType.Pinned)
                Try
                    Dim tmpPtr As IntPtr = handle.AddrOfPinnedObject()


                    lRtn = GetTokenInformation(hToken:=lToken, eTokenInformationClass:=gPMConstants.TOKEN_INFORMATION_CLASS.TokenUser, uTokenInformation:=tmpPtr, nTokenInformationLength:=Marshal.SizeOf(atypTokenInfo(1)) * FIRST_ATTEMPT, nReturnLength:=lLength)
                Finally
                    handle.Free()
                End Try

                If lRtn <> 0 Then
                    If atypTokenInfo(1).uSid.PSID <> 0 Then
                        ' Get the pointer
                        lPtrSid = atypTokenInfo(1).uSid.PSID
                        Dim handle2 As GCHandle = GCHandle.Alloc(lPtrSid, GCHandleType.Pinned)
                        Try
                            Dim tmpPtr2 As IntPtr = handle2.AddrOfPinnedObject()
                            lLenSid = GetLengthSid(tmpPtr2)
                            lPtrSid = Marshal.ReadInt32(tmpPtr2)
                        Finally
                            handle2.Free()
                        End Try

                        If IsValidSid(lPtrSid) <> 0 Then
                            lPtrHeap = HeapAlloc(hHeap:=GetProcessHeap(), dwFlags:=gPMConstants.HEAP_GENERATE_EXCEPTIONS Or gPMConstants.HEAP_ZERO_MEMORY, dwBytes:=lLenSid)

                            If lPtrHeap <> 0 Then
                                Dim handle3 As GCHandle = GCHandle.Alloc(lPtrSid, GCHandleType.Pinned)
                                Dim handle4 As GCHandle = GCHandle.Alloc(lPtrHeap, GCHandleType.Pinned)
                                Try
                                    Dim tmpPtr4 As IntPtr = handle4.AddrOfPinnedObject()
                                    Dim tmpPtr3 As IntPtr = handle3.AddrOfPinnedObject()

                                    CopyMem(pTo:=tmpPtr4, uFrom:=tmpPtr3, lSize:=Marshal.SizeOf(lPtrSid))
                                    lPtrHeap = Marshal.ReadInt32(tmpPtr4)
                                    lPtrSid = Marshal.ReadInt32(tmpPtr3)
                                Finally
                                    handle3.Free()
                                    handle4.Free()
                                End Try
                                result = lPtrHeap
                            End If ' lPtrHeap <> 0
                        End If ' IsValidSid(lPtrSid) <> 0
                    End If ' atypTokenInfo(1).uSid.PSID <> 0
                End If ' lRtn <> 0
            End If ' lToken <> 0
        End If ' OpenProcessToken(...) <> 0

        If lToken <> 0 Then
            CloseHandle(lToken)
        End If

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: DeallocateSID
    '
    ' Description: Event logging support
    '
    ' History: RDC 29072002
    '
    ' ***************************************************************** '
    Private Sub DeallocateSID(ByVal lUserSID As Integer)

        ' Unload the memory used to store the SID
        Dim handle As GCHandle = GCHandle.Alloc(lUserSID, GCHandleType.Pinned)
        Try
            Dim tmpPtr As IntPtr = handle.AddrOfPinnedObject()
            m_lReturn = HeapFree(hHeap:=GetProcessHeap(), dwFlags:=gPMConstants.HEAP_GENERATE_EXCEPTIONS, lpMem:=tmpPtr)
            lUserSID = Marshal.ReadInt32(tmpPtr)
        Finally
            handle.Free()
        End Try

        If m_lReturn <> 0 Then
            Debug.WriteLine(ReturnAPIErrString(Information.Err().LastDllError))
        End If

    End Sub

    ' ***************************************************************** '
    ' Name: GetSiriusVersion
    '
    ' Description: returns Sirius version, service release, Sirius type (B/U)
    '              and (optionally) installation date from registry. If
    '              Sirius is not installed, this function fetches the
    '              version info for Sirius Architecture.
    '
    ' History: RDC 25092002 created
    '
    ' ***************************************************************** '
    Public Function GetSiriusVersion(ByRef sVersion As String, ByRef sRelease As String, ByRef sSiriusType As String, Optional ByRef sDate As String = "") As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' get version number
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLSetup, v_sSettingName:=gPMConstants.ACRegSiriusSetupVersion, r_sSettingValue:=sVersion)
            If sVersion = "" Then
                m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLSetup, v_sSettingName:=gPMConstants.ACRegArchitectureSetupVersion, r_sSettingValue:=sVersion)
                sVersion = sVersion.Substring(0, sVersion.LastIndexOf("."c))
            End If

            ' get service release
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLSetup, v_sSettingName:=gPMConstants.ACRegSiriusSetupRelease, r_sSettingValue:=sRelease)

            ' get type (Broking/Underwriting)
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLSetup, v_sSettingName:=gPMConstants.ACRegSiriusSetupSiriusType, r_sSettingValue:=sSiriusType)

            ' get install date
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLSetup, v_sSettingName:=gPMConstants.ACRegSiriusSetupInstallDate, r_sSettingValue:=sDate)

            If sVersion <> "" Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            ' Sirius not installed, so get SA info

            ' get version number
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLSetup, v_sSettingName:=gPMConstants.ACRegArchitectureSetupVersion, r_sSettingValue:=sVersion)

            ' get service release
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLSetup, v_sSettingName:=gPMConstants.ACRegArchitectureSetupRelease, r_sSettingValue:=sRelease)

            ' type is not specified in SA
            sSiriusType = ""

            ' get install date
            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLSetup, v_sSettingName:=gPMConstants.ACRegArchitectureSetupInstallDate, r_sSettingValue:=sDate)


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function


    ' ***************************************************************** '
    ' Name: ReformatText
    '
    ' Description: Reformat mixed case, underscored text
    '
    ' Summary:
    '   Convert underscores to spaces
    '   Insert spaces where capitals occur mid word
    '   Retain case for all capitalised words
    '   Convert initials to captials
    ' ***************************************************************** '
    Public Function ReformatText(ByVal Expression As String) As String

        Dim nLen As Integer
        Dim sChar As String = ""
        Dim bLower, bSpace As Boolean


        Try

            nLen = Expression.Length
            bSpace = True

            ' Replace underscores with spaces
            Expression = Expression.Replace("_"c, " "c)

            ' Capitalise first letters and insert spaces where necessary
            Dim counter As Integer
            counter = nLen
            For nPos As Integer = 1 To counter
                sChar = Mid(Expression, nPos, 1)

                If bSpace Then
                    bSpace = False
                    bLower = False
                    Mid(Expression, nPos, 1) = sChar.ToUpper()
                Else
                    Select Case sChar
                        Case " "
                            bLower = False
                            bSpace = True

                        Case sChar.ToLower()
                            bLower = True

                        Case sChar.ToUpper()
                            ' Insert a space
                            If bLower Then
                                bLower = False
                                Expression = Expression.Substring(0, nPos - 1) & " " & Mid(Expression, nPos)
                                nLen += 1
                            End If
                    End Select
                End If
            Next
            Return Expression

        Catch ex As Exception

            ' We don't need to handle the error just return the string

        Finally

            ' Return the formatted string


        End Try
        Return Expression
    End Function


    Public Function FileExists(ByVal sFile As String) As Boolean

        Dim result As Boolean = False
        Dim bFolder, bVolumeLabel, bAlias As Boolean


        Try
            Dim nAttributes As FileAttribute = FileSystem.GetAttr(sFile)
            If Information.Err().Number = 0 Then
                ' Return True only if it's actually a file.
                bFolder = (nAttributes And FileAttribute.Directory) <> 0

                bVolumeLabel = (nAttributes And vbVolume) <> 0

                'Modified by Deepak Sharma on 4/20/2010 5:31:18 PM refer developer guide no. 4 (No Solutions)
                'bAlias = (nAttributes And vbAlias) <> 0
                result = Not bFolder And Not bVolumeLabel And Not bAlias
            Else
                result = False
            End If
            Information.Err().Clear()

            Return result

        Catch exc As System.Exception
            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try
    End Function


    Public Function FolderExists(ByVal sFolder As String) As Boolean
        Dim result As Boolean = False

        Try
            Dim nAttributes As FileAttribute = FileSystem.GetAttr(sFolder)
            If Information.Err().Number = 0 Then
                ' Return True only if it's actually a folder
                result = (nAttributes And FileAttribute.Directory) <> 0
            Else
                result = False
            End If
            Information.Err().Clear()

            Return result

        Catch exc As System.Exception
            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: BuildFilePath
    '
    ' Description: Build a valid file path from the passed items.
    '
    ' Summary:
    '   Adds or removes leading and trailing backslash '\' as appropriate
    '   Does not change leading and trailing backslash for overall path
    ' ***************************************************************** '

    Public Function BuildFilePath(ByVal ParamArray Paths() As Object) As String

        Dim result As String = String.Empty
        Dim lLower, lUpper As Integer
        Dim sNextPath As String = ""
        Dim sFullPath As New StringBuilder


        Try

            lLower = Paths.GetLowerBound(0)
            lUpper = Paths.GetUpperBound(0)

            ' Set the base

            sFullPath = New StringBuilder(CStr(Paths(lLower)))
            lLower += 1

            ' Walk all paths
            For lCount As Integer = lLower To lUpper
                ' Assign next path and check length

                sNextPath = CStr(Paths(lCount)).Trim()
                If sNextPath.Length Then
                    ' Trim trailing slashes from full path
                    Do While sFullPath.ToString().EndsWith("\")
                        sFullPath = New StringBuilder(sFullPath.ToString().Substring(0, sFullPath.ToString().Length - 1))
                    Loop

                    ' Trim leading slashes from new path
                    Do While sNextPath.StartsWith("\")
                        sNextPath = sNextPath.Substring(1)
                    Loop

                    ' Append to path
                    sFullPath.Append("\" & sNextPath)
                End If
            Next lCount

            ' Return the built path
            result = sFullPath.ToString()
            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Throw New System.Exception(Information.Err().Number.ToString() + ", " + Information.Err().Source + ", " + Information.Err().Description + ", " + Information.Err().HelpFile + ", " + Information.Err().HelpContext)

        Finally

        End Try
        Return result
    End Function


    ' ***************************************************************** '
    '
    ' Name: BuildDirectory
    '
    ' Description:  Builds a directory tree from a path.
    '
    ' History: 14/08/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function BuildDirectory(ByRef sPath As String) As Integer

        Dim sDir As String = ""
        Dim iEnd, iPos As Integer

        Try

            iEnd = sPath.Length
            iPos = 1

            Do While iPos < iEnd
                iPos = Strings.InStr(iPos, sPath, "\")
                If iPos > 0 Then
                    sDir = sPath.Substring(0, iPos - 1)
                    If Not Directory.Exists(sDir) Then
                        Directory.CreateDirectory(sDir)
                    End If
                    iPos += 1
                Else
                    Exit Do
                End If
            Loop


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            Throw New System.Exception(Information.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message + ", " + Information.Err().HelpFile + ", " + Information.Err().HelpContext)

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetWinDir
    '
    ' Description:  gets path to win dir
    '
    ' History: RKC 04112002 created
    '
    ' ***************************************************************** '
    Public Function GetWinDir() As String

        Dim sBuffer As String = New String(" "c, 1024)

        Dim z As Integer = GetWindowsDirectory(sBuffer, sBuffer.Length - 1)
        Return sBuffer.Substring(0, z)

    End Function

    ' ***************************************************************** '
    ' Name: GetSysDir
    '
    ' Description:  gets path to system dir
    '
    ' History: RKC 04112002 created
    '
    ' ***************************************************************** '
    Public Function GetSysDir() As String

        Dim sBuffer As String = New String(" "c, 1024)

        Dim z As Integer = GetSystemDirectory(sBuffer, sBuffer.Length - 1)
        Return sBuffer.Substring(0, z)

    End Function




    Public Function ConvertCurrencyStringToValue(ByVal sCurrency As String) As Decimal
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: ConvertCurrencyStringToValue
        ' PURPOSE: Removes all non-numeric characters from a string for easy conversion
        ' to a value. This will handle formats like "1000 GBP" or "�100" or "$100.00"
        ' AUTHOR: Danny Davis
        ' DATE: 01/05/2003, 15:17
        ' RETURNS: Value of the Currency String
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Decimal = 0

        Try

            Dim sStrippedString As String = ""

            For i As Integer = 1 To sCurrency.Length
                Dim dbNumericTemp As Double
                If Double.TryParse(Mid(sCurrency, i, 1), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Or Mid(sCurrency, i, 1) = "." Or Mid(sCurrency, i, 1) = "-" Then
                    'Remove all but numerics, commas or periods
                    sStrippedString = sStrippedString & Mid(sCurrency, i, 1)
                End If
            Next i

            'Convert the final value
            result = Conversion.Val(sStrippedString)


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------
            Return result

        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    MessageBox.Show("gPMFUnctions.ConvertCurrencyStringToValue" &
                                    "Version: " & CStr(My.Application.Info.Version.Major) & "." & CStr(My.Application.Info.Version.Minor) & "." & CStr(My.Application.Info.Version.Revision) &
                                    " At line: " & CStr(Information.Erl()) & "|" & Information.Err().Source & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) &
                                    Information.Err().Number & ":" & Information.Err().Description, Application.ProductName)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
            End Select

        Finally


        End Try
        Return result
    End Function


    ' RAW 10/06/2003 : added
    Public Function IsArrayEmpty(ByVal v_vArray As Object) As Boolean
        Dim result As Boolean = False
        Dim i As Integer


        Try
            Information.Err().Clear()

            result = True

            If Information.IsArray(v_vArray) Then

                i = v_vArray.GetUpperBound(0)
                If Information.Err().Number = 0 Then
                    result = False
                    Return result
                End If
            End If




            Return result
        Catch exc As System.Exception
            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        Finally
            Information.Err().Clear()
        End Try
    End Function

    Public Function ShellSort1DArray(ByRef r_vArray() As Object) As Integer

        Dim result As Integer = 0
        Dim lLower, lUpper, lTotal, lIncrement, lLoop2 As Integer
        Dim vData As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lUpper = r_vArray.GetUpperBound(0)
            lLower = r_vArray.GetLowerBound(0)

            ' compute largest increment
            lTotal = lUpper - lLower + 1
            lIncrement = 1
            If lTotal < 14 Then
                lIncrement = 1
            Else
                Do While lIncrement < lTotal
                    lIncrement = 3 * lIncrement + 1
                Loop
                lIncrement = lIncrement \ 3
                lIncrement = lIncrement \ 3
            End If

            Do While lIncrement > 0
                ' sort by insertion in increments of lIncrement
                For lLoop1 As Integer = lLower + lIncrement To lUpper


                    vData = r_vArray(lLoop1)
                    For lLoop2 = lLoop1 - lIncrement To lLower Step -lIncrement



                        If r_vArray(lLoop2) <= vData Then Exit For


                        r_vArray(lLoop2 + lIncrement) = r_vArray(lLoop2)
                    Next


                    r_vArray(lLoop2 + lIncrement) = vData
                Next
                lIncrement = lIncrement \ 3
            Loop

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShellSort1DArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShellSort1DArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function ShellSort2DArray(ByRef r_vArray(,) As Object, ByVal v_iSortColumn As Integer, Optional ByVal v_sSortDirection As String = "ASCENDING") As Integer
        ' ************************************************************************ '
        ' Name: ShellSort
        '
        ' Description: Execute SHELL sort on the selected index of the 2D array that is provided.
        '
        ' Parameters:
        '
        ' r_vArray         = Variant 2D array to sort (column, row dimension positioning)
        ' v_iSortColumn    = Holds the index of the column to sort
        ' v_sSortDirection = Specifies the direction (ASCENDING or DESCENDING) to sort
        '
        ' History: 08/02/2000 CJB - Created.
        '
        ' ************************************************************************ '

        Dim result As Integer = 0
        Dim iNoOfColumns As Integer 'Total number of columns in the array
        Dim iNoOfRows As Integer 'Total number of rows in the array
        Dim iFirstRowNo As Integer 'Index of 1st row number
        Dim iLastRowNo As Integer 'Index of last row number
        'Holds current column currently processing
        Dim iCurrentRow As Integer 'Holds current row currently processing
        Dim iDistance As Integer 'Value used in sorting
        Dim iNextRow As Integer 'Holds next row to process
        Dim vTempStorage As Object 'Holds array element while swapping around
        Dim sSpacePaddedNumber As New FixedLengthString(10) 'Holds a 10 char string padded with spaces and numeric right aligned
        Dim sDataValue1 As String = "" 'Holds value of string to compare with sDataValue2
        Dim sDataValue2 As String = "" 'Holds value of string to compare with sDataValue1
        Dim iStringLength As Integer 'Holds length of string - used in padding routine

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Find number of columns in the array
            iNoOfColumns = r_vArray.GetUpperBound(0)

            'Save the first row number
            iFirstRowNo = r_vArray.GetLowerBound(1)

            'Save the last row number
            iLastRowNo = r_vArray.GetUpperBound(1)

            'Find no. of rows to traverse
            iNoOfRows = iLastRowNo - iFirstRowNo + 1
            iDistance = 1

            While (iDistance <= iNoOfRows)
                iDistance = 2 * iDistance
            End While

            iDistance = CInt((iDistance / 2) - 1)

            While (iDistance > 0)
                iNextRow = iFirstRowNo + iDistance

                'While there are rows to process
                While (iNextRow <= iLastRowNo)
                    iCurrentRow = iNextRow
                    Do
                        If iCurrentRow >= (iFirstRowNo + iDistance) Then

                            'Prepare for actual compare of data value

                            sDataValue1 = CStr(r_vArray(v_iSortColumn, iCurrentRow))

                            sDataValue2 = CStr(r_vArray(v_iSortColumn, iCurrentRow - iDistance))

                            'Don't bother with pad routine (to ensure proper numeric sorting)if both the same !
                            If sDataValue1 <> sDataValue2 Then

                                'If first value to compare is numeric, put in right aligned space padded variable
                                'to ensure that it sorts correctly e.g. 1, 2, 10, 20 not 1, 10, 2, 20
                                Dim dbNumericTemp As Double
                                If Double.TryParse(sDataValue1, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                                    sSpacePaddedNumber.Value = "          "
                                    iStringLength = sDataValue1.Length
                                    If iStringLength < 10 Then
                                        Mid(sSpacePaddedNumber.Value, 10 - iStringLength, iStringLength) = sDataValue1
                                        sDataValue1 = sSpacePaddedNumber.Value
                                    End If
                                End If

                                'If second value is numeric do same as first value
                                Dim dbNumericTemp2 As Double
                                If Double.TryParse(sDataValue2, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                                    sSpacePaddedNumber.Value = "          "
                                    iStringLength = sDataValue2.Length
                                    If iStringLength < 10 Then 'KN(CMG) 12/03/03 PN 2929
                                        Mid(sSpacePaddedNumber.Value, 10 - iStringLength, iStringLength) = sDataValue2
                                        sDataValue2 = sSpacePaddedNumber.Value
                                    End If
                                    sSpacePaddedNumber.Value = "          "
                                End If
                            End If


                            'Ascending sort
                            If v_sSortDirection = "ASCENDING" Then

                                'Do the comparison of data values - if unsorted then swap the two rows around
                                If sDataValue1 < sDataValue2 Then

                                    For iCurrentColumn As Integer = 0 To iNoOfColumns

                                        vTempStorage = r_vArray(iCurrentColumn, iCurrentRow)


                                        r_vArray(iCurrentColumn, iCurrentRow) = r_vArray(iCurrentColumn, iCurrentRow - iDistance)

                                        r_vArray(iCurrentColumn, iCurrentRow - iDistance) = vTempStorage
                                    Next
                                    iCurrentRow -= iDistance
                                Else
                                    Exit Do
                                End If
                            Else

                                'Descending sort
                                If v_sSortDirection = "DESCENDING" Then

                                    'Actual compare of data value - if unsorted then swap the two rows around
                                    If sDataValue1 > sDataValue2 Then
                                        For iCurrentColumn As Integer = 0 To iNoOfColumns

                                            vTempStorage = r_vArray(iCurrentColumn, iCurrentRow)


                                            r_vArray(iCurrentColumn, iCurrentRow) = r_vArray(iCurrentColumn, iCurrentRow - iDistance)

                                            r_vArray(iCurrentColumn, iCurrentRow - iDistance) = vTempStorage
                                        Next
                                        iCurrentRow -= iDistance
                                    Else
                                        Exit Do
                                    End If
                                End If
                            End If
                        Else
                            Exit Do
                        End If
                    Loop
                    iNextRow += 1
                End While
                iDistance = CInt((iDistance - 1) / 2)
            End While

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ShellSort2DArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ShellSort2DArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' *****************************************************************
    ' Returns true if the given expression exists in an array.
    '
    ' Note:
    '   Array must be ordered for search to function correctly, use
    '     ShellSort or ShellSortDistinct in iPMFunc.bas
    '   Search is v.efficient. An array of 4 billion items will only
    '     take 32 iterations to find!!
    ' *****************************************************************
    Public Function BinarySearch(ByVal v_vExpression As Object, ByVal v_vArray() As Object, Optional ByRef r_lIndex As Decimal = 0) As Boolean

        Dim result As Boolean = False
        Dim lLower, lUpper, lCurrent As Integer

        Try

            ' Get bounds
            lLower = v_vArray.GetLowerBound(0)
            lUpper = v_vArray.GetUpperBound(0)

            Do
                ' Get split
                lCurrent = (lUpper + lLower) \ 2

                ' Have we found our value?
                Select Case v_vArray(lCurrent)
                    Case v_vExpression
                        ' Hit
                        result = True
                        r_lIndex = lCurrent
                        Exit Do

                    Case Is < v_vExpression
                        ' Not far enough yet
                        lLower = lCurrent + 1

                    Case Else
                        ' Too far
                        lUpper = lCurrent - 1
                End Select

                ' Keep going until we've run out
            Loop While lLower <= lUpper

            Return result

        Catch excep As System.Exception

            Throw New System.Exception(Information.Err().Number.ToString() + ", BinarySearch, " + excep.Message + ", " + Information.Err().HelpFile + ", " + Information.Err().HelpContext)



            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: ToSafeXXX Functions
    '
    ' Date: 18th February 2004
    '
    ' Description: This is a group of functions to guarantee conversion
    '              to a specified datatype
    '
    ' Author: Peter Finney
    '
    ' Note: Any errors in these functions will always be masked and
    '       return an optional default value. If none is supplied the
    '       default for that datatype will be used.
    ' ***************************************************************** '
    Public Function ToSafeBoolean(ByVal Expression As Object, Optional ByRef Default_Renamed As Boolean = False) As Boolean
        Dim result As Boolean = False
        Try

            If Not (Convert.IsDBNull(Expression) Or IsNothing(Expression)) Then
                result = CBool(Expression)
            End If
            Return result
        Catch
            Return Default_Renamed
        End Try
    End Function

    Public Function ToSafeCurrency(ByVal Expression As Object, Optional ByRef Default_Renamed As Decimal = 0) As Decimal

        Dim result As Decimal
        Try
            If Not (Convert.IsDBNull(Expression) OrElse IsNothing(Expression)) Then
                If Decimal.TryParse(Expression, result) Then
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

    Public Function ToSafeDate(ByVal Expression As Object, Optional ByRef Default_Renamed As Date = #12/30/1899#) As Date

        Dim result As Date
        Try
            If Date.TryParse(Expression, result) Then
                Return result
            Else
                Return Default_Renamed
            End If
        Catch
            Return Default_Renamed
        End Try

    End Function

    Public Function ToSafeDecimal(ByVal Expression As Object, Optional ByRef Default_Renamed As Object = Nothing) As Decimal

        Dim result As Decimal
        Try
            If Not (Convert.IsDBNull(Expression) OrElse IsNothing(Expression)) Then
                If Decimal.TryParse(Expression, result) Then
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

    Public Function ToSafeDouble(ByVal Expression As Object, Optional ByRef Default_Renamed As Double = 0) As Double
        Dim result As Double
        Try
            If Not (Convert.IsDBNull(Expression) OrElse IsNothing(Expression)) Then
                If Double.TryParse(Expression, result) Then
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

    Public Function ToSafeInteger(ByVal Expression As Object, Optional ByRef Default_Renamed As Integer = 0) As Integer
        Dim result As Integer
        Try
            If Not (Convert.IsDBNull(Expression) OrElse IsNothing(Expression)) Then
                If Integer.TryParse(Expression, result) Then
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

    Public Function ToSafeLong(ByVal Expression As Object, Optional ByRef Default_Renamed As Integer = 0) As Integer

        Dim result As Integer
        Try
            If Not (Convert.IsDBNull(Expression) OrElse IsNothing(Expression)) Then
                If Integer.TryParse(Expression, result) Then
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

    Public Function ToSafeString(ByVal Expression As Object, Optional ByRef Default_Renamed As String = "") As String

        Try

            If Not (Convert.IsDBNull(Expression) OrElse IsNothing(Expression)) Then
                Return CStr(Expression)
            Else
                Return Default_Renamed
            End If

        Catch
            Return Default_Renamed
        End Try

    End Function
    ' ***************************************************************** '
    ' End Of ToSafeXXX Functions
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Name: GetDomainUsers
    '
    ' Description:  Enumerates all Global accounts on a NT Domain
    ' ***************************************************************** '

    Public Function GetDomainUsers(ByRef r_vUserNames() As Object, ByVal r_sServerName As String) As Integer
        Dim result As Integer = 0

        Try

            r_vUserNames = Nothing
            ReDim r_vUserNames(0)
            If r_sServerName.Length Then

                Dim searchRoot As String = "WinNT://" & r_sServerName

                Dim Computer As New DirectoryEntry(searchRoot)
                Dim searcher As New DirectorySearcher(searchRoot)
                searcher.Filter = "(&(objectClass=user)(objectCategory=person))"
                searcher.PropertiesToLoad.Add("samaccountname") ' Load the usernames (sAMAccountName)

                ' Get search results
                Dim searchResults As SearchResultCollection = searcher.FindAll()

                For Each searchresult As SearchResult In searchResults
                    ' Fetch the username (sAMAccountName)
                    If searchresult.Properties.Contains("samaccountname") Then
                        Dim username As String = searchresult.Properties("samaccountname")(0).ToString()

                        If r_vUserNames(0) IsNot Nothing Then
                            ReDim Preserve r_vUserNames(r_vUserNames.GetUpperBound(0) + 1)
                        End If

                        r_vUserNames(r_vUserNames.GetUpperBound(0)) = username
                    End If
                Next

            End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get domain users.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDomainUsers", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAvailableNTDomains
    '
    ' Description: Get all available NT Domains
    '
    ' ***************************************************************** '
    Public Function GetAvailableNTDomains(ByRef r_vAvailableDomains() As Object) As Integer

        Dim result As Integer = 0
        Dim forest As Forest
        Dim sOptionValue As String = ""
        Dim nOptionValue As Integer = 5238
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            result = iPMFunc.GetSystemOption(v_iOptionNumber:=nOptionValue, r_sOptionValue:=sOptionValue)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            forest = forest.GetCurrentForest
            ReDim r_vAvailableDomains(0)

            For Each domain As Domain In forest.Domains

                If Convert.ToString(r_vAvailableDomains(0)).Trim() <> "" Then
                    ReDim Preserve r_vAvailableDomains(r_vAvailableDomains.GetUpperBound(0) + 1)
                End If
                Dim sDomainName As String = ""
                sDomainName = domain.Name
                If sOptionValue = "1" AndAlso Not IsNothing(GetNetbiosDomainName(sDomainName)) Then
                    r_vAvailableDomains(r_vAvailableDomains.GetUpperBound(0)) = GetNetbiosDomainName(sDomainName)
                Else
                    sDomainName = domain.GetDirectoryEntry.Name
                    If sDomainName.Contains("DC=") Then
                        'we need to remove the DC= part
                        sDomainName = sDomainName.Replace("DC=", "")
                    End If
                    r_vAvailableDomains(r_vAvailableDomains.GetUpperBound(0)) = sDomainName
                End If

            Next domain


            Return result

        Catch adoexcep As ActiveDirectoryOperationException

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get available NT domains. Please check that the Pure application server is joined to a domain.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAvailableNTDomains", vErrNo:=Information.Err().Number, vErrDesc:=adoexcep.Message, excep:=adoexcep)
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get available NT domains.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAvailableNTDomains", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function

    Private Function GetNetbiosDomainName(dnsDomainName As String) As String

        Dim rootDSE As DirectoryEntry = New DirectoryEntry(String.Format("LDAP://{0}/RootDSE", dnsDomainName))

        Dim configurationNamingContext As String = rootDSE.Properties.Item("configurationNamingContext").Item(0).ToString()

        Dim searchRoot As DirectoryEntry = New DirectoryEntry("LDAP://cn=Partitions," + configurationNamingContext)

        Dim searcher As DirectorySearcher = New DirectorySearcher(searchRoot)
        searcher.SearchScope = SearchScope.OneLevel
        searcher.PropertiesToLoad.Add("netbiosname")
        searcher.Filter = String.Format("(&(objectcategory=Crossref)(dnsRoot={0})(netBIOSName=*))", dnsDomainName)

        Dim result As SearchResult = searcher.FindOne()

        If result IsNot Nothing Then
            Return result.Properties.Item("netbiosname").Item(0).ToString()
        End If

        Return Nothing

    End Function


    ' ***************************************************************** '
    ' Name: BlankTo Functions
    '
    ' Date: 19th May 2005
    '
    ' Description: This is a group of functions to help with converting
    '              blank strings to suitable DB parameter values.
    '
    ' Author: Danny Davis
    '
    ' Note: Strong typing in dPMDAO means that passing a blank string
    '       parameter through to, say, an integer will give an error.
    ' ***************************************************************** '
    Public Function BlankToNull(ByVal Expression As Object) As Object

        Try



            If Strings.Len(CStr(Expression)) = 0 Then

                Return DBNull.Value
            Else
                Return Expression
            End If

        Catch
        End Try



        Return DBNull.Value

    End Function

    Public Function BlankToZero(ByVal Expression As Byte) As Byte

        Try



            If Marshal.SizeOf(Expression) = 0 Then
                Return 0
            Else
                Return Expression
            End If

        Catch
        End Try


        Return 0

    End Function

    ' ***************************************************************** '
    ' End: BlankTo Functions
    ' ***************************************************************** '

    Public Function ConvertHTMLToPDF(ByVal sInputFileName As String, ByRef r_sOutputFilename As String, Optional ByRef bIsDigitalSignature As Boolean = False) As Integer
        Dim result As Integer = 0
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: ConvertHTMLToPDF
        ' PURPOSE: Converts the input htm file into a PDF document in the same directory
        ' AUTHOR: Danny Davis
        ' DATE: 27 January 2006, 15:01:21
        ' RETURNS: PMTrue for success
        '
        ' CHANGES: Added Digital Signature to PDF File
        ' AUTHOR: GAURAV ARORA
        ' DATE: 29 June 2006, 11:55:07
        ' ---------------------------------------------------------------------------

        Dim iPos As Integer
        Dim oPDF As Object 'PDFCreatorPilot2.piPDFDocument
        Dim oHTML As Object 'HTML2PDFAddOn.HTML2PDF
        Dim oFSO As Object

        ' Variables declared for digital signature
        Dim sPMPath As String = "" 'Holds PM Path
        Dim sLogoFilePath As String = "" 'Holds Logo File Path
        Dim sReason, sLocation, sInFile As String 'File to be signed
        Dim bSignTime As Boolean 'Flag for signing time
        Dim iStoreType As Integer 'Input for store type - 0 for listing certificates in IE, 1 for listing certificates in Netscape, 2 for listing certificates in both, 3 for using PFX/P12 files.
        Dim sIssueTo As String = "" 'CN for signer certificate filtering
        Dim sIssueBy As String = "" 'CN for issued by certificate filtering
        Dim sPFXFilePath As String = "" 'Holds Certificate file name
        Dim sPwd As String = "" 'Certificate file password
        Dim bSilentMode As Boolean 'Flag for silent mode
        Dim hAppWnd As Integer 'handle to invoking application
        Dim sOutFile As String = "" 'Path to save the resultant signed file
        Dim iServerCode As Integer
        Dim iSignatureType, iSignatureFormat, iSigPlacement, iPagePlacement As Integer
        Dim sAdditionalErrorInfo As String = ""






        result = gPMConstants.PMEReturnCode.PMTrue
        iPos = IIf(sInputFileName = "" And "." = "", 0, (sInputFileName.LastIndexOf(".") + 1))

        If iPos = 0 Then
            Return result
        End If

        r_sOutputFilename = sInputFileName.Substring(0, iPos) & "pdf"

        'delete the output file if it exists
        oFSO = New Object()

        If File.Exists(r_sOutputFilename) Then
            File.Delete(r_sOutputFilename)
        End If

        oFSO = Nothing

        ' Do not catch errors and let the execution of code
        ' if MSPeelerMain is not installed
        Try

            'If the filter is installed, remove the extra XML.
            MSPeelerMain(sInputFileName, "-tfrb")

        Catch
        End Try

        ' Reset the error handler



        ' PDF Engine
        oPDF = CreateObject("PDFCreatorPilot3Lib.PDFDocument3")

        With oPDF
            .StartEngine(gPMConstants.PDF_CREATOR_PILOT_EMAIL, gPMConstants.PDF_CREATOR_PILOT_PWORD)
            .FileName = r_sOutputFilename
            .BeginDoc()
        End With

        ' HTML add-in
        oHTML = CreateObject("HTML2PDFAddOn.HTML2PDF2")


        With oHTML
            .StartHTMLEngine(gPMConstants.PDF_HTML2PDF_USER, gPMConstants.PDF_HTML2PDF_PWORD)
            .ConnectToPDFLibrary(oPDF)
            .LoadHTMLFile(sInputFileName)
            .AutoAdjustContentWidth = True
            .MarginBottom = 150 'PN 38132 (RC)
            .MarginLeft = 50 'PN 38132 (RC)
            .MarginRight = 100 'PN 38132 (RC)
            .MarginTop = 50 'PN 38132 (RC)
            .MinimalWidth = 400 'PN 38132 (RC)
            .ConvertAll()
            .DisconnectFromPDFLibrary()
        End With

        oHTML = Nothing

        oPDF.EndDoc()

        oPDF = Nothing

        ' Code for Digital Signature
        If bIsDigitalSignature Then
            ' Get the path of PM Folder
            GetPMRegSetting(gPMConstants.HKEY_LOCAL_MACHINE, 0, gPMConstants.PMERegSettingLevel.pmeRSLBase, "PMDIR", sPMPath)

            ' Assign values to the varibales to be supplied as parameters
            sPFXFilePath = sPMPath & "\PM\Sirius Architecture\Common\PDF\pdfsignature.pfx"
            sLogoFilePath = sPMPath & "\PM\Sirius Architecture\Common\PDF\pdfsignedgraphic.txt"
            sReason = ""
            sLocation = ""
            sInFile = r_sOutputFilename
            sOutFile = r_sOutputFilename
            bSignTime = False
            iStoreType = 3
            sIssueTo = ""
            sIssueBy = ""
            sPwd = "password"
            bSilentMode = True
            hAppWnd = 0
            iServerCode = 124 '124
            iSignatureType = 1
            iSignatureFormat = 3
            iSigPlacement = 3
            iPagePlacement = 1
            sAdditionalErrorInfo = "Create Object"
            'Modified by Deepak Sharma on 4/20/2010 12:44:27 PM refer developer guide no. 100 (guide)
            'Create the DeskSeal signing control object

            'oDeskSign = New ReflectionHelper.GetMember(DeskSign, "Sign").1()

            'With oDeskSign
            '	'Set signing parameters
            '	sAdditionalErrorInfo = "SetSigningParametersEx"

            '	ReflectionHelper.Invoke(oDeskSign, "SetSigningParametersEx", New Object(){sReason, sLocation, "", ""})

            '	'Set signatureappearance parameter
            '	sAdditionalErrorInfo = "SetSigAppearanceParam"

            '	ReflectionHelper.Invoke(oDeskSign, "SetSigAppearanceParam", New Object(){iSigPlacement, iPagePlacement, sLogoFilePath})

            '	'Sign the file
            '	sAdditionalErrorInfo = "SignPDFData"

            '	ReflectionHelper.Invoke(oDeskSign, "SignPDFData", New Object(){sInFile, sOutFile, bSignTime, iStoreType, "", "", sPFXFilePath, sPwd, bSilentMode, 0, iServerCode, iSignatureType, iSignatureFormat, ""})

            '	'Check if signing has succeeded

            '	If ReflectionHelper.GetMember(oDeskSign, "GetOperationStatusCode") <> 0 Then


            '		sAdditionalErrorInfo = "GetOperationStatusCode=" & ToSafeString(ReflectionHelper.GetMember(oDeskSign, "GetOperationStatusCode")) & " - " & ToSafeString(ReflectionHelper.GetMember(oDeskSign, "GetOperationStatusString"))
            '		oDeskSign = Nothing
            '		GoTo Catch_Renamed
            '	End If

            'oDeskSign = Nothing
        End If

        GoTo Finally_Renamed

Catch_Renamed:
        If Information.Err().Number = 53 Then
            ' File not found would be if the DLL could not be found, which
            ' would mean that the Filter is not installed on this computer.
            Information.Err().Clear()


        Else
            result = gPMConstants.PMEReturnCode.PMFalse
            oHTML = Nothing
            oPDF.EndDoc()
            oPDF = Nothing

            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ConvertHTMLToPDF failed. Additional Info: " & sAdditionalErrorInfo, vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertHTMLToPDF", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        End If

Finally_Renamed:
        Return result
        ' This is for debugging only
    End Function


    ' ***************************************************************** '
    ' Name: ZeroToNull Function
    '
    ' Date: 24th March 2006
    '
    ' Description: Convert a zero to a null value.
    '
    ' Author: Danny Davis
    '
    ' Note: This is useful for .
    ' ***************************************************************** '
    Public Function ZeroToNull(ByVal Expression As String) As String

        Try


            If Conversion.Val(Expression) = 0 Then

                Return Nothing
            Else
                Return Expression
            End If

        Catch
        End Try



        Return Nothing

    End Function

    Public Function ValidWildcardSearch(ByVal v_bDisableWildcardSearchOption As Boolean, ByVal v_bEnablePartialWildcardSearchOption As Boolean, ByRef r_sFieldValue As String, Optional ByRef r_sErrorMessage As String = "") As Boolean

        Dim result As Boolean = False


        Try



            result = True

            'If Niether option not set then valid
            If Not v_bDisableWildcardSearchOption And Not v_bEnablePartialWildcardSearchOption Then
                Return result
            End If

            'If no value then valid
            If r_sFieldValue.Trim() = "" Then
                Return result
            End If

            If v_bEnablePartialWildcardSearchOption Then
                'Allow wildcards but not the first character
                If r_sFieldValue.TrimStart().StartsWith("%") Then
                    result = False
                    r_sErrorMessage = "Wildcard searches cannot begin with %"
                    Return result
                End If
            ElseIf v_bDisableWildcardSearchOption Then
                'Cannot Contain %
                If r_sFieldValue.IndexOf("%"c) >= 0 Then
                    result = False
                    r_sErrorMessage = "Wildcard search not enabled. Please remove all % characters."
                    Return result
                End If
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sErrorMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ValidWildcardSearch", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
        Finally


        End Try
        Return result
    End Function

    Public Function GetUserIsAmerican(ByRef r_bIsAmerican As Boolean) As Integer
        Dim result As Integer = 0


        Try


            Dim lLCID As Integer
            result = gPMConstants.PMEReturnCode.PMTrue
            lLCID = GetUserDefaultLangID()

            'lLCID = 1033  English - United States
            'lLCID = 4105  English - Canada
            'lLCID = 9225  English - Caribbean
            'lLCID = 8201  English - Jamaica
            'lLCID = 11273 English - Trinidad

            r_bIsAmerican = (lLCID = 1033 Or lLCID = 4105 Or lLCID = 9225 Or lLCID = 8201 Or lLCID = 11273)

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserIsAmerican Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserIsAmerican", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
        Finally


        End Try
        Return result
    End Function


    Public Function GetUserIsAmericanLanguageID(ByRef r_iLanguageID As Integer) As Integer

        Dim result As Integer = 0


        Try


            Dim bIsAmerican As Boolean

            result = GetUserIsAmerican(bIsAmerican)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If bIsAmerican Then
                r_iLanguageID = kUSLangId
            Else
                r_iLanguageID = kUKLangId
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserIsAmericanLanguageID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserIsAmericanLanguageID", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)


            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
        Return result
    End Function

    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: GetUsersFromLDAPSubdirectory
    ' PURPOSE: Recursive Funtion to find out users in Subdirectory for PN52194
    ' DATE: 15 Dec 2008
    ' RETURNS: PMTrue for success
    ' ---------------------------------------------------------------------------
    Private Function GetUsersFromLDAPSubdirectory(ByVal objOrgUnitSub As Object, ByRef r_vAvailableUsers() As Object, ByVal strRoot As String, ByVal strParent As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetUsersFromLDAPSubdirectory"

        Dim strRootSub, strParentSub As String
        Dim objouSub As Object
        Dim objOrgUnitSub1 As Object
        Dim strUserName As String = ""
        Dim bEnterSub As Boolean




        result = gPMConstants.PMEReturnCode.PMTrue


        If m_objLDAPItem1.Class = "organizationalUnit" Then

            strRootSub = m_objLDAPItem1.Name & "," & strRoot
            strParentSub = strParent
            If strParentSub <> "" Then
                objouSub = Marshal.BindToMoniker("LDAP://" & strRootSub & "," & strParentSub)

                objOrgUnitSub1 = Marshal.BindToMoniker(objouSub.ADsPath)
                For Each m_objLDAPItem2 As Object In objOrgUnitSub1
                    m_objLDAPItem1 = m_objLDAPItem2
                    bEnterSub = False

                    If m_objLDAPItem1.Class = "organizationalUnit" Then
                        bEnterSub = True
                        m_lReturn = GetUsersFromLDAPSubdirectory(objOrgUnitSub:=objOrgUnitSub1, r_vAvailableUsers:=r_vAvailableUsers, strRoot:=strRootSub, strParent:=strParentSub)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            RaiseError(kMethodName, "Failed to get Users from Organisational Unit")
                            Return result
                        End If
                    End If

                    If Not bEnterSub Then

                        If m_objLDAPItem1.Class = "user" Then

                            If CStr(r_vAvailableUsers(0)).Trim() <> "" Then
                                ReDim Preserve r_vAvailableUsers(r_vAvailableUsers.GetUpperBound(0) + 1)
                            End If
                            strUserName = UserProperty1("sAMAccountName")

                            r_vAvailableUsers(r_vAvailableUsers.GetUpperBound(0)) = strUserName
                        End If
                    End If
                Next m_objLDAPItem2
            End If
        End If



        Return result
    End Function

    Public Function ConvertHTMLToTxt(ByVal sInputFileName As String, ByRef r_sOutputFilename As String) As Integer
        Dim result As Integer = 0
        Dim iPos As Integer
        Dim oFSO As New Object
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iPos = IIf(sInputFileName = "" And "." = "", 0, (sInputFileName.LastIndexOf(".") + 1))

            If iPos = 0 Then
                Return result
            End If

            r_sOutputFilename = sInputFileName.Substring(0, iPos) & "txt"



            If File.Exists(r_sOutputFilename) Then
                File.Delete(r_sOutputFilename)
            End If

            'Read file to string
            Dim ts As FileStream
            Dim sContents As String = ""
            ts = New FileStream(sInputFileName, FileMode.Open, FileAccess.Read)
            'Do Until ts.AtEndOfStream
            'sContents = sContents & ts.ReadLine()
            'Loop 
            'Akash: Use StreamReader object to read the string from file
            Dim sreader As New StreamReader(ts)
            sContents = sreader.ReadToEnd()

            ts.Close()

            'Remove Comments
            Dim sHTML As String = ""
            Dim oRegExp As Regex
            'Akash: Need to discuss about regular expression
            oRegExp = New Regex("<!--.*?-->", RegexOptions.IgnoreCase Or RegexOptions.Multiline)
            sHTML = oRegExp.Replace(sContents, "")

            'Remove HTML Tags
            Dim sContents1 As String = ""
            sContents1 = sHTML
            oRegExp = New Regex("<[^>]*>", RegexOptions.IgnoreCase Or RegexOptions.Multiline)
            sHTML = oRegExp.Replace(sContents1, "")

            'Set &gt; and &lt; back
            sContents = HTMLDecode(sHTML)

            'Save File
            Dim hFile As Integer
            hFile = FileSystem.FreeFile()
            FileSystem.FileOpen(hFile, r_sOutputFilename, OpenMode.Output)
            FileSystem.PrintLine(hFile, sContents)
            FileSystem.FileClose(hFile)

            Return result
        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            oFSO = Nothing

            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ConvertHTMLToTxt failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertHTMLToTxt", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally
            '        

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    Private Function HTMLDecode(ByVal encodedstring As String) As String
        Dim tmp As String = encodedstring
        tmp = tmp.Replace("&quot;", Strings.Chr(34).ToString())
        tmp = tmp.Replace("&lt;", Strings.Chr(60).ToString())
        tmp = tmp.Replace("&gt;", Strings.Chr(62).ToString())
        tmp = tmp.Replace("&amp;", Strings.Chr(38).ToString())
        tmp = tmp.Replace("&nbsp;", Strings.Chr(32).ToString())
        For i As Integer = 1 To 255
            tmp = tmp.Replace("&#" & i & ";", Strings.Chr(i).ToString())
        Next
        Return tmp
    End Function


    ''''Issue Report (ref: 62185) : 22 Oct 2009
    Public Function ValidateNumeric(ByRef iKeyAscii As Integer, ByRef sValue As String, Optional ByRef bIsInteger As Boolean = False) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "ValidateNumeric"
        Try
            Select Case iKeyAscii
                Case 8, 26, 48 To 57 'Allow Backspace and numbers
                    If iKeyAscii = 48 And sValue.Trim() = "0" Then ''''For avaiding more than one zero
                        result = 0
                    Else
                        result = iKeyAscii
                    End If
                Case Else
                    If iKeyAscii = 46 And Not bIsInteger Then
                        If sValue.IndexOf("."c) >= 0 Then
                            result = 0
                        Else
                            result = iKeyAscii
                        End If

                    Else
                        result = 0
                    End If
            End Select
            Return result
        Catch ex As Exception
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateNumeric Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
        Finally

        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: FormatDocumentRef
    '
    ' Description: Format doc ref with 12 places of postfix
    '
    ' ***************************************************************** '
    Public Function FormatDocumentRef(ByVal RangeCode As String, ByVal lNumber As Integer) As String
        Try
            'Format the number
            Return RangeCode & StringsHelper.Format(lNumber, "0000000000")
        Catch
        End Try
        Return ""
    End Function
    Public Function IsStrongPassword(ByVal v_sUsername As String, ByVal v_iUserID As Integer, ByVal v_iMainSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_iLogLevel As Integer, ByVal v_sCallingAppName As String, ByVal sPasswordString As String, ByRef bIsstrongPassword As Boolean, Optional ByVal v_iSourceID As Integer = 0) As Integer
        Dim sRegEx As String = ""

        Dim iOptionNumber As Integer = 5101
        Try
            m_lReturn = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(bPMFunc.GetSystemOption(v_sUsername:=v_sUsername, v_sPassword:=sPasswordString, v_iUserID:=v_iUserID, v_iMainSourceID:=v_iMainSourceID, v_iLanguageID:=v_iLanguageID, v_iCurrencyID:=v_iCurrencyID, v_iLogLevel:=v_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=iOptionNumber, r_sOptionValue:=sRegEx, v_iSourceID:=v_iSourceID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sRegEx <> "" Then
                If Not String.IsNullOrEmpty(sPasswordString.Trim) Then
                    'MessageBox.Show("Password is not valid")
                    If Not Regex.IsMatch(sPasswordString.Trim, sRegEx) Then
                        bIsstrongPassword = False
                        Return gPMConstants.PMEReturnCode.PMTrue
                    Else
                        bIsstrongPassword = True
                        Return gPMConstants.PMEReturnCode.PMTrue
                    End If
                End If
            Else
                If Strings.Len(sPasswordString.Trim) < 4 Then
                    MessageBox.Show("Passwords must consist of four or more characters." & Strings.Chr(10).ToString() &
                                    "Please choose another Password.", "E0105 - Incorrect Password Length", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                bIsstrongPassword = True
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

        Catch ex As Exception
            m_lReturn = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_iUserID", v_iUserID)
            oDict.Add("v_iMainSourceID", v_iMainSourceID)
            oDict.Add("v_iLanguageID", v_iLanguageID)
            oDict.Add("v_iCurrencyID", v_iCurrencyID)
            oDict.Add("v_iSourceID", v_iSourceID)
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Is Strong Password Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsStrongPassword", excep:=ex, oDicParms:=oDict)

            Return m_lReturn
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: IsValidEmail
    '
    ' Description: Checking Email regex
    '
    ' ***************************************************************** '

    Public Function IsValidEmail(ByVal v_sEmail As String) As Integer
        Dim sResult As Integer = 0
        Dim sOptionValue As String = ""
        Dim nOptionValue As Integer = 5245
        Try

            sResult = gPMConstants.PMEReturnCode.PMTrue

            sResult = iPMFunc.GetSystemOption(v_iOptionNumber:=nOptionValue, r_sOptionValue:=sOptionValue)

            If sResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sOptionValue <> "" Then
                If Not Regex.IsMatch(v_sEmail, sOptionValue) Then
                    sResult = gPMConstants.PMEReturnCode.PMFalse
                Else
                    sResult = gPMConstants.PMEReturnCode.PMTrue
                End If
            End If

            Return sResult

        Catch excep As System.Exception

            sResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Is Valid Email Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsValidEmail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return sResult

        End Try


    End Function

    Public Function GenerateUniqueSSPExceptionRef(ByRef iLength As Integer) As String
        Dim sResult As New StringBuilder
        sResult.Append(gPMConstants.ERROR_LABEL)

        Try
            Dim rdm As New Random()
            Dim allowChrs() As Char = "ABCDEFGHIJKLOMNOPQRSTUVWXYZ0123456789".ToCharArray()

            For i As Integer = 0 To iLength - 1
                sResult.Append(allowChrs(rdm.Next(0, allowChrs.Length)))
            Next
        Catch
            sResult = New StringBuilder
            sResult.Append(gPMConstants.ERROR_LABEL)
            sResult.Append(New String("9", iLength))
        End Try

        sResult.Append(" - ")
        sResult.Append(DateTime.Now())

        Return sResult.ToString()
    End Function
#Region "Suppress Decimals"
    ''' <summary>
    ''' It's a common function used to dis-allow the Decimals
    ''' </summary>
    ''' <param name="oTEXT"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Public Sub NumPress(ByRef oTEXT As System.Windows.Forms.TextBox, ByVal e As System.Windows.Forms.KeyPressEventArgs)

        On Error Resume Next
        Dim stempString As String
        '
        Dim nLeftPlace As Integer = 10
        Dim nRightPlace As Integer = 0

        If nRightPlace = 0 Then stempString = "0123456789-" & oTEXT.Tag Else stempString = "0123456789.-" & oTEXT.Tag
        If Asc(e.KeyChar) > 26 Then
            If InStr(stempString, e.KeyChar) = 0 Then e.Handled = True

            If InStr(oTEXT.Text, ".") <> 0 Then
                If Asc(e.KeyChar) = 46 Then e.Handled = True
                If InStr(oTEXT.Text, "-") <> 0 Then
                    If InStr(oTEXT.Text, ".") - 1 > nLeftPlace And oTEXT.SelectionStart < InStr(oTEXT.Text, ".") Then
                        e.Handled = True
                    ElseIf Len(oTEXT.Text) >= InStr(oTEXT.Text, ".") + nRightPlace And oTEXT.SelectionStart >= InStr(oTEXT.Text, ".") Then
                        e.Handled = True
                    End If
                Else
                    If InStr(oTEXT.Text, ".") > nLeftPlace And oTEXT.SelectionStart < InStr(oTEXT.Text, ".") Then
                        e.Handled = True
                    ElseIf Len(oTEXT.Text) >= InStr(oTEXT.Text, ".") + nRightPlace And oTEXT.SelectionStart >= InStr(oTEXT.Text, ".") Then
                        e.Handled = True
                    End If
                End If
            Else
                If Asc(e.KeyChar) = 46 Then Exit Sub
                If InStr(oTEXT.Text, "-") <> 0 Then
                    If Len(oTEXT.Text) - 1 >= nLeftPlace Then e.Handled = True
                Else
                    If Len(oTEXT.Text) >= nLeftPlace And Asc(e.KeyChar) <> 45 Then e.Handled = True
                End If
            End If
        ElseIf Asc(e.KeyChar) = 8 And InStr(oTEXT.Text, "-") <> 0 And Mid(oTEXT.Text, oTEXT.SelectionStart, 1) = "." And Mid(oTEXT.Text, oTEXT.SelectionStart + 1, 1) <> "" And Len(oTEXT.Text) - 1 - nRightPlace >= nLeftPlace Then
            e.Handled = True
        ElseIf Asc(e.KeyChar) = 8 And InStr(oTEXT.Text, "-") = 0 And Mid(oTEXT.Text, oTEXT.SelectionStart, 1) = "." And Mid(oTEXT.Text, oTEXT.SelectionStart + 1, 1) <> "" And Len(oTEXT.Text) - nRightPlace >= nLeftPlace Then
            e.Handled = True
        End If
    End Sub
    ''' <summary>
    ''' It's a pure custom rounding function to round the values based on parameter.
    ''' </summary>
    ''' <param name="crInputValue"> Decimal value to be Round</param>
    ''' <param name="nRoundUpto"> Secimal places</param>
    ''' <param name="bIsRoundWholeNumber">is round upto whole numbers</param>
    ''' <returns></returns>
    Public Function ToSafeRound(ByVal crInputValue As Decimal,
                                       ByVal nRoundUpto As Integer,
                                       ByVal bIsRoundWholeNumber As Boolean) As Decimal
        Dim crTempValue As Decimal
        Try
            If crInputValue <> 0 Then
                If bIsRoundWholeNumber Then
                    crTempValue = Math.Round(crInputValue, 0, MidpointRounding.AwayFromZero)
                Else
                    crTempValue = Math.Round(crInputValue, nRoundUpto)
                End If
            Else
                crInputValue = crInputValue
            End If

        Catch ex As Exception
            crTempValue = crInputValue
        End Try
        Return crTempValue
    End Function

    Public Function GetDocumentLibrary(ByVal m_oDatabase As Object, ByVal nPartyCnt As Integer, ByVal PartyShortName As String) As String
        'Dim m_oDatabase As dPMDAO.Database
        Dim m_nReturn As Integer
        Dim sDocumentLibrary As String

        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("PartyCnt", nPartyCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            .Parameters.Add("PartyShortName", PartyShortName, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            m_nReturn = .SQLSelect("spu_SIR_Get_Party_Document_Library", "Get_Party_Document_Library", True)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_nReturn = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the Data", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocumentLibrary", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return Convert.ToString(m_nReturn)
            End If
        End With

        sDocumentLibrary = Convert.ToString(m_oDatabase.Records.Item(0).Fields("DocumentLibrary"))
        Return sDocumentLibrary
    End Function
#End Region
    Public Function GetUniqueID() As String
        Dim validchars As String = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890"

        Dim sb As New StringBuilder()
        Dim rand As New Random()
        For i As Integer = 1 To 10
            Dim idx As Integer = rand.Next(0, validchars.Length)
            Dim randomChar As Char = validchars(idx)
            sb.Append(randomChar)
        Next i

        Return sb.ToString()

    End Function
End Module