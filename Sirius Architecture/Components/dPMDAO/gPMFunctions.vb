Option Strict Off
Option Explicit On
Imports System.Globalization
Imports System.IO
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Text.RegularExpressions
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports Microsoft.Practices.EnterpriseLibrary.Logging.Filters
Imports Microsoft.Practices.EnterpriseLibrary.Logging.Formatters
Imports Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners

<System.Runtime.InteropServices.ProgId("gPMFunctions_NET.gPMFunctions")>
Public Module PMFunctions
    Private m_lReturn As PMConstants.PMEReturnCode
    Private Const NullToStringDefault As String = ""
    <ThreadStatic()>
    Private vProductOptions(,) As Object

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

            If Not (Convert.IsDBNull(Expression) Or Expression Is Nothing) Then
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
            If Not (Convert.IsDBNull(Expression) OrElse Expression Is Nothing) Then
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
            If Not (Convert.IsDBNull(Expression) OrElse Expression Is Nothing) Then
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
            If Not (Convert.IsDBNull(Expression) OrElse (Expression) Is Nothing) Then
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
            If Not (Convert.IsDBNull(Expression) OrElse Expression Is Nothing) Then
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
            If Not (Convert.IsDBNull(Expression) OrElse Expression Is Nothing) Then
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

            If Not (Convert.IsDBNull(Expression) OrElse Expression Is Nothing) Then
                Return CStr(Expression)
            Else
                Return Default_Renamed
            End If

        Catch
            Return Default_Renamed
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: GetUnderwritingBranchDetails
    '
    ' Description:
    '
    ' History: 25/02/2004 SJ - Created.
    '

    'Public Function NullToString(ByVal Expression As Object) As String

    '    ' Check for null, else convert to string

    '    If Convert.IsDBNull(Expression) Or Expression Is Nothing Then
    '        Return NullToStringDefault
    '    Else

    '        Return CStr(Expression)
    '    End If

    'End Function

    Public Sub RaiseError(ByVal v_sSource As String, ByVal v_sDescription As String)
        RaiseError(v_sSource, v_sDescription, -1)
    End Sub

    Public Sub RaiseError(ByVal v_sSource As String, ByVal v_sDescription As String, ByRef LogLevel As PMConstants.PMELogLevel)
        Throw New System.Exception(v_sSource + ", " + v_sDescription)
    End Sub

    Private Function GetUnderwritingBranchInd(ByVal v_oDatabase As dPMDAO.Database, ByVal v_iSourceID As Integer, ByVal v_sUsername As String, ByRef r_bIsUnderwritingBranch As Boolean) As Integer

        Dim result As Integer = 0


        result = PMConstants.PMEReturnCode.PMTrue

        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        r_bIsUnderwritingBranch = False

        sSQL = "SELECT underwriting_branch_ind FROM source WHERE source_id = " & v_iSourceID
        m_lReturn = v_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetUnderwritingBranchInd", bStoredProcedure:=False, vResultArray:=vResultArray, lNumberRecords:=PMConstants.PMAllRecords)
        If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
            Return PMConstants.PMEReturnCode.PMFalse
        End If

        If (vResultArray.GetType().IsArray) Then

            If (CStr(vResultArray(0, 0))) = 1 Then
                r_bIsUnderwritingBranch = True
            End If
        End If

        Return result

    End Function

    Public Function CheckDatabase(ByVal v_sUsername As String, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_lPMProductFamily As Integer, ByRef r_bNewInstanceCreated As Boolean, ByRef r_oCheckedDatabase As Object, Optional ByVal v_vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lReturn As PMConstants.PMEReturnCode
        Dim bNewInstanceRequired As Boolean
        Dim sPosInFunction As String = ""

        result = PMConstants.PMEReturnCode.PMTrue
        r_bNewInstanceCreated = False
        bNewInstanceRequired = False

        ' Have we a Database Parameter
        If (v_vDatabase Is Nothing) Then
            bNewInstanceRequired = True
        End If

        If Not bNewInstanceRequired Then
            ' Have we a valid Object Reference?
            'If Not Information.IsReference(v_vDatabase) Then
            '    bNewInstanceRequired = True
            'End If
        End If

        ' Do we need to create a new Instance
        If bNewInstanceRequired Then
            r_bNewInstanceCreated = True
            lReturn = CType(NewDatabase(v_sUsername:=v_sUsername, v_iSourceID:=v_iSourceID, v_iLanguageID:=v_iLanguageID, v_lPMProductFamily:=v_lPMProductFamily, r_oDatabase:=r_oCheckedDatabase), PMConstants.PMEReturnCode)
            If lReturn <> PMConstants.PMEReturnCode.PMTrue Then
                result = PMConstants.PMEReturnCode.PMFalse
                r_oCheckedDatabase = Nothing
                Return result
            End If
        Else

            r_bNewInstanceCreated = False
            ' Return the checked Instance
            r_oCheckedDatabase = v_vDatabase
        End If
        Return result
Err_CheckDatabase:
        ' Error Section.
        result = PMConstants.PMEReturnCode.PMError
        ' Log Error.
        Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
        oDict.Add("v_iSourceID", v_iSourceID)
        oDict.Add("v_iLanguageID", v_iLanguageID)
        PMFunctions.LogMessageToFile(sUsername:=v_sUsername, iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check Database for DSN - ", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckDatabase", excep:=New Exception(Information.Err().Description & " Position = " & sPosInFunction), oDicParms:=oDict)
        Return result
    End Function



    Public Function NewDatabase(ByVal v_sUsername As String, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_lPMProductFamily As Integer, ByRef r_oDatabase As dPMDAO.Database) As Integer
        Dim result As Integer = 0
        Dim sDSN As String = ""

        Try
            result = PMConstants.PMEReturnCode.PMTrue
            ' Create a New instance of PMDAO
            r_oDatabase = New dPMDAO.Database

            Return r_oDatabase.OpenDatabase(sSiriusUsername:=v_sUsername, iSourceID:=v_iSourceID, iLanguageID:=v_iLanguageID, sCallingAppName:=ACApp)
        Catch
        End Try
        result = PMConstants.PMEReturnCode.PMError
        r_oDatabase = Nothing
        ' Log Error.
        Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
        oDict.Add("v_iSourceID", v_iSourceID)
        oDict.Add("v_iLanguageID", v_iLanguageID)
        PMFunctions.LogMessageToFile(sUsername:=v_sUsername, iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create a new instance of PMDAO for DSN - " & sDSN, vApp:=ACApp, vClass:=ACClass, vMethod:="NewDatabase", excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
        Return result
    End Function

    Private Function findValueInArray(ByVal v_vOptionNumber As PMConstants.SIRHiddenOptions, ByVal v_vBranch As Integer, ByRef r_vUnderwriting As String, ByVal v_bValue As Boolean) As Integer

        Dim result As Integer = 0
        Dim i As Integer



        result = PMConstants.PMEReturnCode.PMTrue
        r_vUnderwriting = ""

        '   Find the option Number within the array
        If Not (vProductOptions Is Nothing) Then
            For i = 0 To vProductOptions.GetUpperBound(1)

                If CInt(vProductOptions(0, i)) = v_vOptionNumber Then

                    If CInt(vProductOptions(1, i)) = v_vBranch Then
                        Exit For
                    End If
                End If
            Next i

            '   If it was not found then find the option for the Head Office value

            If i > vProductOptions.GetUpperBound(1) Then
                v_vBranch = PMConstants.SIRBCHHeadOffice

                For i = 0 To vProductOptions.GetUpperBound(1)

                    If CInt(vProductOptions(0, i)) = v_vOptionNumber Then

                        If CInt(vProductOptions(1, i)) = v_vBranch Then
                            Exit For
                        End If
                    End If
                Next i
            End If

            '   If found then get the value or UnderwritingType (only used by getUnderwritingType)

            Dim tempString As String = ""
            If i <= vProductOptions.GetUpperBound(1) Then
                If v_bValue Then

                    tempString = CStr(vProductOptions(2, i))
                Else

                    tempString = CStr(vProductOptions(3, i))
                End If

                If Not (Convert.IsDBNull(tempString) Or (tempString Is Nothing)) Then
                    r_vUnderwriting = tempString.Trim()
                End If
            End If
        End If
        Return result

    End Function



    Public Function GetPMRegSetting(ByVal v_lPMERegSettingRoot As Integer, ByVal v_lPMEProductFamily As Integer, ByVal v_lPMERegSettingLevel As Integer, ByVal v_sSettingName As String, ByRef r_sSettingValue As String, Optional ByVal v_sSubKey As String = "") As Integer
        Dim result As Integer = 0
        Dim sKeyString As String = ""
        Dim lRoot As Integer
        Dim vSettingValue As String = ""
        Dim UserName As String = ""
        Dim MachineName As String = ""
        Try

            result = PMConstants.PMEReturnCode.PMTrue

            ' Current User OR Local Machine
            If v_lPMERegSettingRoot = PMConstants.PMERegSettingRoot.pmeRSRCurrentUser Then
                UserName = Environment.UserName
                lRoot = PMConstants.HKEY_CURRENT_USER
            Else
                lRoot = PMConstants.HKEY_LOCAL_MACHINE
                MachineName = Environment.MachineName
            End If

            ' Build up the key String
            sKeyString = BuildKeyString(v_ePMEProductFamily:=v_lPMEProductFamily, v_ePMERegSettingLevel:=v_lPMERegSettingLevel, v_sSubKey:=v_sSubKey)

            ' Do we have a key string
            If sKeyString.Trim() = "" Then
                Return PMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Value
            Dim oDataBase As New dPMDAO.Database
            oDataBase.GetValues(sKeyString, v_sSettingName, vSettingValue, UserName, MachineName)

            ' If the Setting Value Is Null, Empty or Nothing


            If String.IsNullOrEmpty(vSettingValue) Then
                ' Return an Empty String
                r_sSettingValue = ""
            Else
                ' Otherwise, Return the Setting Value
                r_sSettingValue = vSettingValue.Trim()
            End If

            Return result

        Catch




            Return PMConstants.PMEReturnCode.PMError
        End Try

    End Function

    Public Function BuildKeyString(ByVal v_ePMEProductFamily As PMConstants.PMEProductFamily, ByVal v_ePMERegSettingLevel As PMConstants.PMERegSettingLevel, Optional ByVal v_sSubKey As String = "") As String

        Dim sKeyString As String = ""

        Try

            ' Build up the key String

            ' Start with Root
            sKeyString = PMConstants.ACRegRoot

            ' Add PM Product
            Select Case v_ePMEProductFamily
                Case PMConstants.PMEProductFamily.pmePFSiriusArchitecture
                    sKeyString = sKeyString & PMConstants.ACRegSiriusArchitecture
                Case PMConstants.PMEProductFamily.pmePFSiriusUnderwriting
                    sKeyString = sKeyString & PMConstants.ACRegSiriusUnderwriting
                Case PMConstants.PMEProductFamily.pmePFOrion
                    sKeyString = sKeyString & PMConstants.ACRegOrion
                Case PMConstants.PMEProductFamily.pmePFGemini
                    sKeyString = sKeyString & PMConstants.ACRegGemini
                Case PMConstants.PMEProductFamily.pmePFVoyager
                    sKeyString = sKeyString & PMConstants.ACRegVoyager
                Case PMConstants.PMEProductFamily.pmePFMercury
                    sKeyString = sKeyString & PMConstants.ACRegMercury
                Case PMConstants.PMEProductFamily.pmePFDocumaster
                    sKeyString = sKeyString & PMConstants.ACRegDocumaster
                Case PMConstants.PMEProductFamily.pmePFSiriusBroking
                    sKeyString = sKeyString & PMConstants.ACRegSiriusBroking
                Case PMConstants.PMEProductFamily.pmePFSiriusSolutions
                    sKeyString = sKeyString & PMConstants.ACRegSiriusSolutions
                Case PMConstants.PMEProductFamily.pmePFNirvana
                    sKeyString = sKeyString & PMConstants.ACRegNirvana
                Case PMConstants.PMEProductFamily.pmePFGeminiII
                    sKeyString = sKeyString & PMConstants.ACRegGeminiII
                Case PMConstants.PMEProductFamily.pmePFClaims
                    sKeyString = sKeyString & PMConstants.ACRegClaims
                Case PMConstants.PMEProductFamily.pmePFStargate
                    sKeyString = sKeyString & PMConstants.ACRegStargate
                Case PMConstants.PMEProductFamily.pmePFSwift
                    sKeyString = sKeyString & PMConstants.ACRegSwift

            End Select

            ' Add Level
            Select Case v_ePMERegSettingLevel
                Case PMConstants.PMERegSettingLevel.pmeRSLClient
                    sKeyString = sKeyString & PMConstants.ACRegClient
                Case PMConstants.PMERegSettingLevel.pmeRSLServer
                    sKeyString = sKeyString & PMConstants.ACRegServer
                Case PMConstants.PMERegSettingLevel.pmeRSLCommon
                    sKeyString = sKeyString & PMConstants.ACRegCommon
                Case PMConstants.PMERegSettingLevel.pmeRSLSetup
                    sKeyString = sKeyString & PMConstants.ACRegSetup
                Case PMConstants.PMERegSettingLevel.pmeRSLBase
                    sKeyString = sKeyString
                Case Else
                    sKeyString = sKeyString & PMConstants.ACRegCommon
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



    Function QueryValueEx(ByVal lhKey As Integer, ByVal szValueName As String, ByRef vValue As String) As Integer

        Dim cch, lrc, lType, lValue As Integer
        Dim sValue As String = ""

        Try

            ' Determine the size and type of data to be read
            lrc = RegQueryValueExNULL(lhKey, szValueName, 0, lType, 0, cch)
            If lrc <> PMConstants.ERROR_NONE Then Return 1

            Select Case lType
                ' For strings
                Case PMConstants.REG_SZ
                    sValue = New String(Strings.ChrW(0), cch)
                    lrc = RegQueryValueExString(lhKey, szValueName, 0, lType, sValue, cch)

                    If lrc = PMConstants.ERROR_NONE Then
                        vValue = sValue.Substring(0, cch - 1)
                    Else
                        vValue = String.Empty
                    End If

                    ' For DWORDS
                Case PMConstants.REG_DWORD
                    lrc = RegQueryValueExLong(lhKey, szValueName, 0, lType, lValue, cch)

                    If lrc = PMConstants.ERROR_NONE Then vValue = CStr(lValue)

                Case Else
                    'all other data types not supported
                    lrc = -1
            End Select
            Return 1
        Catch

            Return 0
        End Try

    End Function
    ' Registry services
    Declare Function RegCloseKey Lib "advapi32.dll" (ByVal hKey As Integer) As Integer

    Declare Function RegCreateKeyEx Lib "advapi32.dll" Alias "RegCreateKeyExA" (ByVal hKey As Integer, ByVal lpSubKey As String, ByVal Reserved As Integer, ByVal lpClass As String, ByVal dwOptions As Integer, ByVal samDesired As Integer, ByVal lpSecurityAttributes As Integer, ByRef phkResult As Integer, ByRef lpdwDisposition As Integer) As Integer

    Declare Function RegOpenKeyEx Lib "advapi32.dll" Alias "RegOpenKeyExA" (ByVal hKey As Integer, ByVal lpSubKey As String, ByVal ulOptions As Integer, ByVal samDesired As Integer, ByRef phkResult As Integer) As Integer

    Declare Function RegQueryValueExString Lib "advapi32.dll" Alias "RegQueryValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByRef lpType As Integer, ByVal lpData As String, ByRef lpcbData As Integer) As Integer

    Declare Function RegQueryValueExLong Lib "advapi32.dll" Alias "RegQueryValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByRef lpType As Integer, ByRef lpData As Integer, ByRef lpcbData As Integer) As Integer

    Declare Function RegQueryValueExNULL Lib "advapi32.dll" Alias "RegQueryValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByRef lpType As Integer, ByVal lpData As Integer, ByRef lpcbData As Integer) As Integer

    Declare Function RegSetValueExString Lib "advapi32.dll" Alias "RegSetValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal Reserved As Integer, ByVal dwType As Integer, ByVal lpValue As String, ByVal cbData As Integer) As Integer

    Declare Function RegSetValueExLong Lib "advapi32.dll" Alias "RegSetValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal Reserved As Integer, ByVal dwType As Integer, ByRef lpValue As Integer, ByVal cbData As Integer) As Integer

    Public Function ArchitectureInDebug() As Boolean

        Dim result As Boolean = False
        Dim sInDebug As String = ""

        Try


            m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=PMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=PMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=PMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=PMConstants.PMRegKeyArchitectureInDebug, r_sSettingValue:=sInDebug)

            ' Check for errors.
            If (m_lReturn <> PMConstants.PMEReturnCode.PMTrue) Or (sInDebug.Trim() = "") Then
                ' Return False if Errors
                Return False
            End If

            ' Return result YES, Y or PMTrue are Yes

            Select Case sInDebug.Trim().ToUpper()
                Case "YES", "Y", CStr(PMConstants.PMEReturnCode.PMTrue)
                    Return True
                Case Else
                    Return False
            End Select

        Catch
        End Try



        Return False

    End Function

    'Public Function GetSystemName(ByRef sSystemName As String) As Integer
    '    Dim result As Integer = 0
    '    Dim sBuffer As New StringsHelper.FixedLengthString(255)
    '    Dim lBufferSize, lSessionID As Integer

    '    Try

    '        result = PMConstants.PMEReturnCode.PMTrue

    '        lBufferSize = 255

    '        ' API Call to get computer name
    '        m_lReturn = GetComputerName(sBuffer.Value, lBufferSize)

    '        ' Check return code
    '        If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
    '            ' Return error
    '            sSystemName = ""
    '            result = PMConstants.PMEReturnCode.PMFalse
    '        Else
    '            ' Set System Name Parameter
    '            sSystemName = sBuffer.Value.Substring(0, Math.Min(sBuffer.Value.Length, lBufferSize))
    '        End If

    '        m_lReturn = GetWTSSessionID(lSessionID)

    '        If m_lReturn <> PMConstants.PMEReturnCode.PMTrue Then
    '            Return result
    '        End If

    '        sSystemName = sSystemName & "_sid" & CStr(lSessionID)

    '        Return result

    '    Catch excep As System.Exception



    '        ' Error Section.
    '        result = PMConstants.PMELogLevel.PMLogOnError
    '        sSystemName = ""

    '        ' Log Error.
    '        LogMessagePopup(iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get computer name", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSystemName", excep:=excep)

    '        Return result

    '    End Try
    'End Function


    ' ***************************************************************** '
    ' Name: GetSystemNameNoSID
    '
    ' Description: Gets the system (computer) name without the WTS SID
    '
    ' ***************************************************************** '


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
            'While (sSearchText.IndexOf("*"c) >= 0)
            '    Mid(sSearchText, sSearchText.IndexOf("*"c) + 1, 1) = "%"

            'End While
            sSearchText = sSearchText.Replace("*", "%")

            ' Add implied wildcard to end
            If Not sSearchText.EndsWith("%") Then
                sSearchText = sSearchText & "%"
            End If

            r_sTextString = sSearchText


            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = PMConstants.PMELogLevel.PMLogOnError

            ' Log Error.
            'LogMessagePopup(iType:=PMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Convert Wild Cards For SQL", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertWildCardsForSQL", excep:=excep)

            Return result

        End Try
    End Function

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

    Public Function GetWTSSessionID(ByRef lSessionID As Integer) As Integer

        Dim result As Integer = 0
        Dim lLength, lpBuffer As Integer

        Const WTS_CURRENT_SERVER_HANDLE As Integer = 0
        Const WTS_CURRENT_SESSION_HANDLE As Integer = -1

        Const RETURN_FAIL As Integer = 0

        Try

            result = PMConstants.PMEReturnCode.PMFalse

            m_lReturn = WTSQuerySessionInformation(WTS_CURRENT_SERVER_HANDLE, WTS_CURRENT_SESSION_HANDLE, WTS_INFO_CLASS.WTSSessionID, lpBuffer, lLength)

            If m_lReturn = RETURN_FAIL Then
                lSessionID = 0
                Return result
            Else
                Dim handle As GCHandle = GCHandle.Alloc(lSessionID, GCHandleType.Pinned)
                Try
                    Dim tmpPtr As IntPtr = handle.AddrOfPinnedObject()
                    CopyMemoryFromAddress(tmpPtr, lpBuffer, lLength)
                    lSessionID = Marshal.ReadInt32(tmpPtr)
                Finally
                    handle.Free()
                End Try
                WTSFreeMemory(lpBuffer)
            End If


            Return PMConstants.PMEReturnCode.PMTrue

        Catch



            Return PMConstants.PMEReturnCode.PMError
        End Try

    End Function

    Private Declare Function WTSQuerySessionInformation Lib "wtsapi32.dll" Alias "WTSQuerySessionInformationA" (ByVal hServer As Integer, ByVal hSessionId As Integer, ByVal lWSI As WTS_INFO_CLASS, ByRef lptBuffer As Integer, ByRef lBytes As Integer) As Integer

    Private Declare Sub WTSFreeMemory Lib "wtsapi32.dll" (ByVal pMemory As Integer)

    Private Declare Sub CopyMemoryFromAddress Lib "kernel32" Alias "RtlMoveMemory" (ByVal lpDestination As Integer, ByVal lplpSource As Integer, ByVal length As Integer)
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
            Return 0
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
            Return 0
            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try
    End Function
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
    Private Const NullToBooleanDefault As Boolean = False
    Private Const NullToDateDefault As Date = #12/29/1899#
    Private Const NullToDecimalDefault As Integer = 0
    Private Const NullToLongDefault As Integer = 0
    'Private Const NullToStringDefault As String = ""
    Private Const NullToIntegerDefault As Integer = 0
    Private Const NullToDoubleDefault As Double = 0
    Private Const NullToCurrencyDefault As Decimal = 0

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

        If Convert.IsDBNull(Expression) Or (Expression Is Nothing) Then
            Return NullToBooleanDefault
        Else

            Return CBool(Expression)
        End If

    End Function

    Public Function NullToDate(ByVal Expression As Object) As Date

        ' Check for null, else convert to date

        If Convert.IsDBNull(Expression) Or (Expression Is Nothing) Then
            Return NullToDateDefault
        Else

            Return CDate(Expression)
        End If

    End Function

    Public Function NullToDecimal(ByVal Expression As Object) As Decimal

        ' Check for null, else convert to decimal

        If Convert.IsDBNull(Expression) Or (Expression Is Nothing) Then
            ' There is no native decimal type so we must convert
            ' the default as well
            Return NullToDecimalDefault
        Else

            Return CDec(Expression)
        End If

    End Function

    Public Function NullToLong(ByVal Expression As Object) As Integer

        ' Check for null, else convert to long

        If Convert.IsDBNull(Expression) Or (Expression Is Nothing) Then
            Return NullToLongDefault
        Else

            Return CInt(Expression)
        End If

    End Function

    Public Function NullToString(ByVal Expression As Object) As String

        ' Check for null, else convert to string

        If Convert.IsDBNull(Expression) Or (Expression Is Nothing) Then
            Return NullToStringDefault
        Else

            Return CStr(Expression)
        End If

    End Function

    Public Function NullToInteger(ByVal Expression As Object) As Integer


        If Convert.IsDBNull(Expression) Or (Expression Is Nothing) Then
            Return NullToIntegerDefault
        Else

            Return CInt(Expression)
        End If

    End Function

    Public Function NullToDouble(ByVal Expression As Object) As Double

        If Convert.IsDBNull(Expression) Or (Expression Is Nothing) Then
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


        If Convert.IsDBNull(Expression) Or (Expression Is Nothing) Then
            Return NullToCurrencyDefault
        Else

            Return CDec(Expression)
        End If

    End Function



    Public Sub LogMessagePopup(iType As PMELogLevel, sMsg As String, vApp As String, vClass As String, vMethod As String, excep As Exception)
        Throw New NotImplementedException()
    End Sub

    Private Const kUSLangId As Integer = 2
    Private Const kUKLangId As Integer = 1

    Public Function SetPMRegSetting(ByVal v_lPMERegSettingRoot As Integer, ByVal v_lPMEProductFamily As Integer, ByVal v_lPMERegSettingLevel As Integer, ByVal v_sSettingName As String, ByVal v_sSettingValue As String, Optional ByVal v_sSubKey As String = "") As Integer

        Dim result As Integer = 0
        Dim sKeyString As String = ""
        Dim lRoot As Integer
        Dim UserName As String = ""
        Dim MachineName As String = ""
        Try

            result = PMConstants.PMEReturnCode.PMTrue

            ' Current User OR Local Machine
            If v_lPMERegSettingRoot = PMConstants.PMERegSettingRoot.pmeRSRCurrentUser Then
                UserName = Environment.UserName
                lRoot = PMConstants.HKEY_CURRENT_USER
            Else
                lRoot = PMConstants.HKEY_LOCAL_MACHINE
                MachineName = Environment.MachineName
            End If

            ' Build up the key String
            sKeyString = BuildKeyString(v_ePMEProductFamily:=v_lPMEProductFamily, v_ePMERegSettingLevel:=v_lPMERegSettingLevel, v_sSubKey:=v_sSubKey)

            ' Do we have a key string
            If sKeyString.Trim() = "" Then
                Return PMConstants.PMEReturnCode.PMFalse
            End If

            ' Save the Value
            Dim oDataBase As New dPMDAO.Database
            oDataBase.SetValues(sKeyString, v_sSettingName, v_sSettingValue, UserName, MachineName)

            Return result

        Catch




            Return PMConstants.PMEReturnCode.PMError
        End Try

    End Function


    Sub CreateNewKey(ByRef lPredefinedKey As Integer, ByRef sNewKeyName As String)

        Dim hNewKey As Integer 'handle to the new key
        Dim lRetVal As Integer 'result of the RegCreateKeyEx function

        lRetVal = RegCreateKeyEx(lPredefinedKey, sNewKeyName, 0, Nothing, PMConstants.REG_OPTION_NON_VOLATILE, PMConstants.EL_KEY_ALL_ACCESS, 0, hNewKey, lRetVal)

        RegCloseKey(hNewKey)

    End Sub

    Sub SetKeyValue(ByRef lPredefinedKey As Integer, ByRef sKeyName As String, ByRef sValueName As String, ByRef vValueSetting As String, ByRef lValueType As Integer)

        Dim hKey As Integer 'handle of open key

        'open the specified key
        m_lReturn = RegOpenKeyEx(lPredefinedKey, sKeyName, 0, PMConstants.EL_KEY_ALL_ACCESS, hKey)

        m_lReturn = SetValueEx(hKey, sValueName, lValueType, vValueSetting)

        RegCloseKey(hKey)

    End Sub
    Function SetValueEx(ByVal hKey As Integer, ByRef sValueName As String, ByRef lType As Integer, ByRef vValue As String) As Integer

        Dim result As Integer = 0
        Dim lValue As Integer
        Dim sValue As String = ""

        Select Case lType
            Case PMConstants.REG_SZ
                sValue = vValue & Strings.ChrW(0).ToString()
                result = RegSetValueExString(hKey, sValueName, 0, lType, sValue, sValue.Length)
            Case PMConstants.REG_DWORD
                lValue = CInt(vValue)
                result = RegSetValueExLong(hKey, sValueName, 0, lType, lValue, 4)
        End Select

        Return result
    End Function
    Function QueryKeyValue(ByRef lPredefinedKey As Integer, ByRef sKeyName As String, ByRef sValueName As String) As String

        Dim result As String = String.Empty
        Dim hKey As Integer 'handle of opened key
        Dim vValue As String = "" 'setting of queried value

        m_lReturn = RegOpenKeyEx(lPredefinedKey, sKeyName, 0, PMConstants.REG_KEY_READ, hKey)

        m_lReturn = QueryValueEx(hKey, sValueName, vValue)

        If sValueName IsNot Nothing And sValueName <> "" And String.IsNullOrEmpty(vValue) Then
            vValue = gPMRegistryFunctionsWOW6432.GetRegKey64(gPMRegConstants.HKEY_LOCAL_MACHINE, sKeyName, sValueName)
            If vValue Is Nothing Or vValue = "" Then
                vValue = gPMRegistryFunctionsWOW6432.GetRegKey32(gPMRegConstants.HKEY_LOCAL_MACHINE, sKeyName, sValueName)
            End If

            Dim replaceregkey As String = sKeyName.Replace("SOFTWARE\Pure", "SOFTWARE\WOW6432Node\Pure")
            Dim valuetest64 As String = gPMRegistryFunctionsWOW6432.GetRegKey32(gPMRegConstants.HKEY_LOCAL_MACHINE, replaceregkey, sValueName)
            PMFunctions.LogMessageToFile(sUsername:="", iType:=PMConstants.PMELogLevel.PMLogInfo, sMsg:=vValue + "-" + replaceregkey + "-" + valuetest64 + "-" + sValueName, vApp:=ACApp, vClass:=ACClass, vMethod:="ChangeQuerykeyvalue")

        End If

        PMFunctions.LogMessageToFile(sUsername:="", iType:=PMConstants.PMELogLevel.PMLogInfo, sMsg:=sKeyName + "-" + sValueName + "-" + vValue, vApp:=ACApp, vClass:=ACClass, vMethod:="Querykeyvalue")

        result = vValue
        RegCloseKey(hKey)

        Return result
    End Function

    Function KeyExists(ByRef lPredefinedKey As Integer, ByRef sKeyName As String) As Boolean

        Dim result As Boolean = False
        Dim hKey As Integer 'handle of opened key



        m_lReturn = RegOpenKeyEx(lPredefinedKey, sKeyName, 0, PMConstants.REG_KEY_READ, hKey)

        result = m_lReturn = PMConstants.ERROR_NONE

        RegCloseKey(hKey)
        Return result
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

    Public Function EventLogWrite(ByVal iType As Integer, ByVal sMsg As String, ByRef oDicParms As Dictionary(Of String, Object), ByVal sErrUniqueId As String, ByVal excep As Exception, Optional ByRef vUsername As String = "", Optional ByRef vCallingApp As String = "", Optional ByRef vApp As String = "", Optional ByRef vClass As String = "", Optional ByRef vMethod As String = "", Optional ByRef vErrNo As String = "", Optional ByRef vErrDesc As String = "", Optional ByRef vBinaryData As String = "", Optional ByRef vServerName As String = "", Optional ByRef vTraceEventType As TraceEventType = TraceEventType.Error) As Integer
        Dim slogmessage As String = ""
        Dim ifilenumber As Integer
        Dim slogfile = "", smsglogging = ""
        Dim suserloglevel As String = ""
        Dim iuserloglevel As PMConstants.PMELogLevel
        Try

            If vUsername.Trim() = "" OrElse sErrUniqueId = "Incorrect_Connection_String" Then
                iuserloglevel = PMConstants.PMELogLevel.PMLogOnError
            Else
                ' hkey_current_user\software\pm\siriusarchitecture\common
                m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=PMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=PMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=PMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=PMConstants.PMRegKeyLogLevel, r_sSettingValue:=suserloglevel)
                Dim dbnumerictemp As Double
                If Double.TryParse(suserloglevel, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbnumerictemp) Then
                    iuserloglevel = CType(CInt(suserloglevel), PMConstants.PMELogLevel)
                Else
                    iuserloglevel = PMConstants.PMELogLevel.PMLogOnError
                End If
            End If

            ' if its high enough priority, log it.
            If (iType <= iuserloglevel) Or (iType <= PMConstants.PMELogLevel.PMLogOnError) Then
                If sErrUniqueId.Trim = "" Then
                    sErrUniqueId = GenerateUniqueSSPExceptionRef(PMConstants.ERROR_NO_LENGTH)
                End If
                ' rdc 30082002 messages to event log or text files?
                If sErrUniqueId = "Incorrect_Connection_String" Then
                    smsglogging = "1"
                Else
                    m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=PMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=PMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=PMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=PMConstants.PMRegKeyUseEventLog, r_sSettingValue:=smsglogging)
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        smsglogging = "1"
                    End If
                End If
                If smsglogging = "1" Then
                        ' we're writing to the o/s application event log
                        sMsg = sMsg & ChrW(13) & ChrW(10) & ChrW(13) & ChrW(10)
                        If sErrUniqueId <> "" Then sMsg = sMsg & sErrUniqueId & Strings.ChrW(13) & Strings.ChrW(10)
                        If vUsername <> "" Then sMsg = sMsg & "Username:" & vUsername & Strings.ChrW(13) & Strings.ChrW(10)
                        If vCallingApp <> "" Then sMsg = sMsg & "Calling app:" & vCallingApp & Strings.ChrW(13) & Strings.ChrW(10)
                        If vApp <> "" Then sMsg = sMsg & "App:" & vApp & Strings.ChrW(13) & Strings.ChrW(10)
                        If vClass <> "" Then sMsg = sMsg & "Class:" & vClass & Strings.ChrW(13) & Strings.ChrW(10)
                        If vMethod <> "" Then sMsg = sMsg & "Method:" & vMethod & Strings.ChrW(13) & Strings.ChrW(10)
                        If vErrNo <> "" Then sMsg = sMsg & "VB Error:" & vErrNo & Strings.ChrW(13) & Strings.ChrW(10)
                        If vErrDesc <> "" Then sMsg = sMsg & "VB Description:" & vErrDesc & Strings.ChrW(13) & Strings.ChrW(10)

                        sMsg = sMsg & ChrW(13) & ChrW(10)
                        If excep IsNot Nothing AndAlso excep.Message IsNot Nothing Then
                            sMsg = sMsg & excep.Message
                            sMsg = sMsg & ChrW(13) & ChrW(10)
                        End If
                        If excep IsNot Nothing AndAlso excep.StackTrace IsNot Nothing Then
                            sMsg = sMsg & excep.StackTrace
                            sMsg = sMsg & ChrW(13) & ChrW(10)
                        End If

                        ' in EntLib4
                        CreateManualWriter()

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
                        Return PMConstants.PMEReturnCode.PMTrue
                        Exit Function
                    End If

                m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=PMConstants.PMERegSettingRoot.pmeRSRCurrentUser, v_lPMEProductFamily:=PMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=PMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=PMConstants.PMRegKeyLogFile, r_sSettingValue:=slogfile)
                ' if there is no user specific log file
                If slogfile.Trim() = "" Then
                    m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=PMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=PMConstants.PMEProductFamily.pmePFSiriusArchitecture, v_lPMERegSettingLevel:=PMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=PMConstants.PMRegKeyLogFile, r_sSettingValue:=slogfile)
                End If

                ' check for errors.
                If (m_lReturn <> PMConstants.PMEReturnCode.PMTrue) Or (slogfile.Trim() = "") Then
                        ' if there was no log file in the registry,
                        ' use the default one.
                        slogfile = PMConstants.PMDefaultLogFile
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
                        Case PMConstants.PMELogLevel.PMLogDebug1
                            slogmessage = slogmessage & "pmlogdebug1"
                        Case PMConstants.PMELogLevel.PMLogDebug2
                            slogmessage = slogmessage & "pmlogdebug2"
                        Case PMConstants.PMELogLevel.PMLogDebug3
                            slogmessage = slogmessage & "pmlogdebug3"
                        Case PMConstants.PMELogLevel.PMLogDebug4
                            slogmessage = slogmessage & "pmlogdebug4"
                        Case PMConstants.PMELogLevel.PMLogError
                            slogmessage = slogmessage & "pmlogerror"
                        Case PMConstants.PMELogLevel.PMLogFatal
                            slogmessage = slogmessage & "pmlogfatal"
                        Case PMConstants.PMELogLevel.PMLogFeedback
                            slogmessage = slogmessage & "pmlogfeedback"
                        Case PMConstants.PMELogLevel.PMLogInfo
                            slogmessage = slogmessage & "pmloginfo"
                        Case PMConstants.PMELogLevel.PMLogOnError
                            slogmessage = slogmessage & "pmlogonerror"
                        Case PMConstants.PMELogLevel.PMLogWarning
                            slogmessage = slogmessage & "pmlogwarning"
                        Case Else
                            slogmessage = slogmessage & "unknown : " & CStr(iType)
                    End Select

                    ' add unique  error number if we have them
                    slogmessage = slogmessage & Environment.NewLine & sErrUniqueId & Strings.ChrW(13) & Strings.ChrW(10)

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
                Return PMConstants.PMEReturnCode.PMTrue
        Catch
            Return PMConstants.PMEReturnCode.PMError
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
        m_LogWriter = New LogWriterImpl(filters, traceSources, mainLogSource, mainLogSource, mainLogSource, GeneralCategory, True, True)
    End Sub

    Public Function GenerateUniqueSSPExceptionRef(ByRef iLength As Integer) As String
        Dim sResult As New StringBuilder
        sResult.Append(PMConstants.ERROR_LABEL)

        Try
            Dim rdm As New Random()
            Dim allowChrs() As Char = "ABCDEFGHIJKLOMNOPQRSTUVWXYZ0123456789".ToCharArray()

            For i As Integer = 0 To iLength - 1
                sResult.Append(allowChrs(rdm.Next(0, allowChrs.Length)))
            Next
        Catch
            sResult = New StringBuilder
            sResult.Append(PMConstants.ERROR_LABEL)
            sResult.Append(New String("9", iLength))
        End Try

        sResult.Append(" - ")
        sResult.Append(DateTime.Now())

        Return sResult.ToString()
    End Function

End Module
