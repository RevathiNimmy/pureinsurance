Option Strict Off
Option Explicit On
Imports System.Globalization
Imports System.IO
Imports System.Reflection
Imports System.Runtime.InteropServices
Imports System.Text
Imports System.Text.RegularExpressions
Imports dPMDAO
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports Microsoft.Practices.EnterpriseLibrary.Logging.Filters
Imports Microsoft.Practices.EnterpriseLibrary.Logging.Formatters
Imports Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners

<System.Runtime.InteropServices.ProgId("gPMFunctions_NET.gPMFunctions")>
Public Module gPMFunctions
    Private m_lReturn As gPMConstants.PMEReturnCode
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

    Public Sub RaiseError(ByVal v_sSource As String, ByVal v_sDescription As String, ByRef LogLevel As gPMConstants.PMELogLevel)
        Throw New System.Exception(v_sSource + ", " + v_sDescription)
    End Sub

    Private Function GetUnderwritingBranchInd(ByVal v_oDatabase As dPMDAO.Database, ByVal v_iSourceID As Integer, ByVal v_sUsername As String, ByRef r_bIsUnderwritingBranch As Boolean) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        r_bIsUnderwritingBranch = False

        sSQL = "SELECT underwriting_branch_ind FROM source WHERE source_id = " & v_iSourceID
        m_lReturn = v_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetUnderwritingBranchInd", bStoredProcedure:=False, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
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
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim bNewInstanceRequired As Boolean
        Dim sPosInFunction As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue
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
            lReturn = CType(NewDatabase(v_sUsername:=v_sUsername, v_iSourceID:=v_iSourceID, v_iLanguageID:=v_iLanguageID, v_lPMProductFamily:=v_lPMProductFamily, r_oDatabase:=r_oCheckedDatabase), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
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
        result = gPMConstants.PMEReturnCode.PMError
        ' Log Error.
        Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
        oDict.Add("v_iSourceID", v_iSourceID)
        oDict.Add("v_iLanguageID", v_iLanguageID)
        gPMFunctions.LogMessageToFile(sUsername:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check Database for DSN - ", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckDatabase", excep:=New Exception(Information.Err().Description & " Position = " & sPosInFunction), oDicParms:=oDict)
        Return result
    End Function



    Public Function NewDatabase(ByVal v_sUsername As String, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_lPMProductFamily As Integer, ByRef r_oDatabase As dPMDAO.Database) As Integer
        Dim result As Integer = 0
        Dim sDSN As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            ' Create a New instance of PMDAO
            r_oDatabase = New dPMDAO.Database

            Return r_oDatabase.OpenDatabase(sSiriusUsername:=v_sUsername, iSourceID:=v_iSourceID, iLanguageID:=v_iLanguageID, sCallingAppName:=ACApp)
        Catch
        End Try
        result = gPMConstants.PMEReturnCode.PMError
        r_oDatabase = Nothing
        ' Log Error.
        Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
        oDict.Add("v_iSourceID", v_iSourceID)
        oDict.Add("v_iLanguageID", v_iLanguageID)
        gPMFunctions.LogMessageToFile(sUsername:=v_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create a new instance of PMDAO for DSN - " & sDSN, vApp:=ACApp, vClass:=ACClass, vMethod:="NewDatabase", excep:=New Exception(Information.Err().Description), oDicParms:=oDict)
        Return result
    End Function

    Private Function findValueInArray(ByVal v_vOptionNumber As gPMConstants.SIRHiddenOptions, ByVal v_vBranch As Integer, ByRef r_vUnderwriting As String, ByVal v_bValue As Boolean) As Integer

        Dim result As Integer = 0
        Dim i As Integer



        result = gPMConstants.PMEReturnCode.PMTrue
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
                v_vBranch = gPMConstants.SIRBCHHeadOffice

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

    Public Function CreateLateBoundObject(ByVal ClassName As String) As Object
        Dim sPurePath As String

        Try
            sPurePath = New Uri(Assembly.GetExecutingAssembly().CodeBase).LocalPath
            sPurePath = Path.GetDirectoryName(sPurePath)

            Dim libraryPath As String
            Dim DLLAssembly As [Assembly]

            libraryPath = Path.Combine(sPurePath, ClassName.Substring(0, ClassName.IndexOf(".")) & ".dll")
            If IO.File.Exists(libraryPath) Then
                DLLAssembly = [Assembly].LoadFrom(libraryPath)
                Return DLLAssembly.CreateInstance(ClassName, True)
            End If

            libraryPath = Path.Combine(sPurePath, ClassName.Substring(0, ClassName.IndexOf(".")) & ".exe")
            If IO.File.Exists(libraryPath) Then
                DLLAssembly = [Assembly].LoadFrom(libraryPath)
                Return DLLAssembly.CreateInstance(ClassName, True)
            End If

            Throw New IO.FileNotFoundException("Cannot find " & libraryPath & " in " & sPurePath & " as exe or dll")
        Catch excep As Exception
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CreateLateBoundObject Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateLateBoundObject", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try
        Return Nothing
    End Function

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

            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

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
                    sValue = New String(Strings.ChrW(0), cch)
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
    ' Registry services
    Declare Function RegCloseKey Lib "advapi32.dll" (ByVal hKey As Integer) As Integer

    Declare Function RegCreateKeyEx Lib "advapi32.dll" Alias "RegCreateKeyExA" (ByVal hKey As Integer, ByVal lpSubKey As String, ByVal Reserved As Integer, ByVal lpClass As String, ByVal dwOptions As Integer, ByVal samDesired As Integer, ByVal lpSecurityAttributes As Integer, ByRef phkResult As Integer, ByRef lpdwDisposition As Integer) As Integer

    Declare Function RegOpenKeyEx Lib "advapi32.dll" Alias "RegOpenKeyExA" (ByVal hKey As Integer, ByVal lpSubKey As String, ByVal ulOptions As Integer, ByVal samDesired As Integer, ByRef phkResult As Integer) As Integer

    Declare Function RegQueryValueExString Lib "advapi32.dll" Alias "RegQueryValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByRef lpType As Integer, ByVal lpData As String, ByRef lpcbData As Integer) As Integer

    Declare Function RegQueryValueExLong Lib "advapi32.dll" Alias "RegQueryValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByRef lpType As Integer, ByRef lpData As Integer, ByRef lpcbData As Integer) As Integer

    Declare Function RegQueryValueExNULL Lib "advapi32.dll" Alias "RegQueryValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal lpReserved As Integer, ByRef lpType As Integer, ByVal lpData As Integer, ByRef lpcbData As Integer) As Integer

    Declare Function RegSetValueExString Lib "advapi32.dll" Alias "RegSetValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal Reserved As Integer, ByVal dwType As Integer, ByVal lpValue As String, ByVal cbData As Integer) As Integer

    Declare Function RegSetValueExLong Lib "advapi32.dll" Alias "RegSetValueExA" (ByVal hKey As Integer, ByVal lpValueName As String, ByVal Reserved As Integer, ByVal dwType As Integer, ByRef lpValue As Integer, ByVal cbData As Integer) As Integer
    Declare Function RegEnumKey Lib "advapi32.dll" Alias "RegEnumKeyA" (ByVal hKey As Integer, ByVal dwIndex As Integer, ByVal lpName As String, ByVal cbName As Integer) As Integer

    Declare Function RegDeleteKey Lib "advapi32.dll" Alias "RegDeleteKeyA" (ByVal hKey As Integer, ByVal lpSubKey As String) As Integer

    Declare Function RegDeleteValue Lib "advapi32.dll" Alias "RegDeleteValueA" (ByVal hKey As Integer, ByVal lpValueName As String) As Integer

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

    Public Function GetSystemName(ByRef sSystemName As String) As Integer
        Dim result As Integer = 0
        Dim sBuffer As New StringsHelper.FixedLengthString(255)
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
            iPos = If(sSystemName = "" And "_sid" = "", 0, (sSystemName.LastIndexOf("_sid") + 1))

            If iPos > 0 Then
                ' get rid of SID
                sSystemName = sSystemName.Substring(0, iPos - 1)
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            'LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get computer name (No SID)", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSystemNameNoSID", excep:=excep)

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
            result = gPMConstants.PMELogLevel.PMLogOnError

            ' Log Error.
            'LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Convert Wild Cards For SQL", vApp:=ACApp, vClass:=ACClass, vMethod:="ConvertWildCardsForSQL", excep:=excep)

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

            result = gPMConstants.PMEReturnCode.PMFalse

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


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch



            Return gPMConstants.PMEReturnCode.PMError
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

    Public Function ValidWildcardSearch(ByVal v_bDisableWildcardSearchOption As Boolean, ByVal v_bEnablePartialWildcardSearchOption As Boolean, ByRef r_sFieldValue As String, Optional ByRef r_sErrorMessage As String = "") As Boolean

        Dim result As Boolean = False
        Const kMethodName As String = "ValidWildcardSearch"

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

    Public Sub LogMessagePopup(iType As PMELogLevel, sMsg As String, vApp As String, vClass As String, vMethod As String, excep As Exception)
        Throw New NotImplementedException()
    End Sub

    Private Const kUSLangId As Integer = 2
    Private Const kUKLangId As Integer = 1
    Public Function GetUserIsAmerican(ByRef r_bIsAmerican As Boolean) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetUserIsAmerican"

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
        Const kMethodName As String = "GetUserIsAmericanLanguageID"

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
            Dim oDatabase = New dPMDAO.Database()
            oDatabase.SetValues(sKeyString, v_sSettingName, v_sSettingValue, UserName, MachineName)

            Return result

        Catch




            Return gPMConstants.PMEReturnCode.PMError
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
    Function SetValueEx(ByVal hKey As Integer, ByRef sValueName As String, ByRef lType As Integer, ByRef vValue As String) As Integer

        Dim result As Integer = 0
        Dim lValue As Integer
        Dim sValue As String = ""

        Select Case lType
            Case gPMConstants.REG_SZ
                sValue = vValue & Strings.ChrW(0).ToString()
                result = RegSetValueExString(hKey, sValueName, 0, lType, sValue, sValue.Length)
            Case gPMConstants.REG_DWORD
                lValue = CInt(vValue)
                result = RegSetValueExLong(hKey, sValueName, 0, lType, lValue, 4)
        End Select

        Return result
    End Function
    Function QueryKeyValue(ByRef lPredefinedKey As Integer, ByRef sKeyName As String, ByRef sValueName As String) As String

        Dim result As String = String.Empty
        Dim hKey As Integer 'handle of opened key
        Dim vValue As String = "" 'setting of queried value

        m_lReturn = RegOpenKeyEx(lPredefinedKey, sKeyName, 0, gPMConstants.REG_KEY_READ, hKey)

        m_lReturn = QueryValueEx(hKey, sValueName, vValue)
        If sValueName IsNot Nothing And sValueName <> "" And String.IsNullOrEmpty(vValue) Then
            ' Dim vValue64 As String = localKey.GetValue(sValueName)
            'value64 = localKey.GetRegKey64("RegisteredOrganization")
            vValue = gPMRegistryFunctionsWOW6432.GetRegKey64(gPMRegConstants.HKEY_LOCAL_MACHINE, sKeyName, sValueName)
            If vValue Is Nothing Or vValue = "" Then
                vValue = gPMRegistryFunctionsWOW6432.GetRegKey32(gPMRegConstants.HKEY_LOCAL_MACHINE, sKeyName, sValueName)
            End If

            Dim replaceregkey As String = sKeyName.Replace("SOFTWARE\Pure", "SOFTWARE\WOW6432Node\Pure")
            Dim valuetest64 As String = gPMRegistryFunctionsWOW6432.GetRegKey32(gPMRegConstants.HKEY_LOCAL_MACHINE, replaceregkey, sValueName)
            PMFunctions.LogMessageToFile(sUsername:="", iType:=PMConstants.PMELogLevel.PMLogInfo, sMsg:=vValue + "-" + replaceregkey + "-" + valuetest64 + "-" + sValueName, vApp:=ACApp, vClass:=ACClass, vMethod:="ChangeQuerykeyvalue")
            ' byteValue32 = localKey.GetRegKey32AsByteArray("DigitalProductId"); // Key doesn't exist by default in 32 bit
            ' vValue = value64
        End If

        PMFunctions.LogMessageToFile(sUsername:="", iType:=PMConstants.PMELogLevel.PMLogInfo, sMsg:=sKeyName + "-" + sValueName + "-" + vValue, vApp:=ACApp, vClass:=ACClass, vMethod:="Querykeyvalue")

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
        Dim slogfile, smsglogging, suserloglevel As String
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

                    'If Not EventLog.SourceExists(EVENT_LOG_APP_NAME, ".") Then
                    '    Dim creationData As New EventSourceCreationData(EVENT_LOG_APP_NAME, EVENT_LOG_FILE_NAME)
                    '    creationData.MachineName = "."
                    '    EventLog.CreateEventSource(creationData)
                    'End If
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
        m_LogWriter = New LogWriterImpl(filters, traceSources, mainLogSource, mainLogSource, mainLogSource, GeneralCategory, True, True)
    End Sub

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
        Dim sSpacePaddedNumber As New StringsHelper.FixedLengthString(10) 'Holds a 10 char string padded with spaces and numeric right aligned
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
    'Public Function Len(sValue As String) As Integer
    '    Return ToSafeString(sValue).Length
    'End Function
    Public Function FormatField(ByVal iFormatType As Integer, ByVal vFieldValue As String, Optional ByVal vDecimalPlaces As Integer = -1) As String
        Dim sControlResult As String

        Try

            ' Check for a null value

            If Convert.IsDBNull(vFieldValue) Or Information.IsNothing(vFieldValue) Then
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
                        sControlResult = String.Format(vFieldValue, "medium date").Trim()
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
                        sControlResult = String.Format(vFieldValue, "medium time").Trim()
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
                        sControlResult = String.Format(vFieldValue, "medium date").Trim() &
                                         " " & String.Format(vFieldValue, "medium time").Trim()
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
                        sControlResult = (If(DateTime.TryParse(vFieldValue, TempDate), TempDate.ToString("yyyy"), vFieldValue)).Trim()
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatMonthOnlyLong
                    ' Format value to a Month in Long format eg January
                    If Not Information.IsDate(vFieldValue) Then
                        sControlResult = ""
                    Else
                        Dim TempDate2 As Date
                        sControlResult = (If(DateTime.TryParse(vFieldValue, TempDate2), TempDate2.ToString("MMMM"), vFieldValue)).Trim()
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatMonthOnlyMedium
                    ' Format value to a Month in Medium format eg Jan
                    If Not Information.IsDate(vFieldValue) Then
                        sControlResult = ""
                    Else
                        Dim TempDate3 As Date
                        sControlResult = (If(DateTime.TryParse(vFieldValue, TempDate3), TempDate3.ToString("MMM"), vFieldValue)).Trim()
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatMonthOnlyShort
                    ' Format value to a Month in Short format eg 01
                    If Not Information.IsDate(vFieldValue) Then
                        sControlResult = ""
                    Else
                        Dim TempDate4 As Date
                        sControlResult = (If(DateTime.TryParse(vFieldValue, TempDate4), TempDate4.ToString("MM"), vFieldValue)).Trim()
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatDayOnlyLong
                    ' Format value to a Day in Long format eg Friday
                    If Not Information.IsDate(vFieldValue) Then
                        sControlResult = ""
                    Else
                        Dim TempDate5 As Date
                        sControlResult = (If(DateTime.TryParse(vFieldValue, TempDate5), TempDate5.ToString("dddd"), vFieldValue)).Trim()
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatDayOnlyMedium
                    ' Format value to a Day in Medium format eg Fri
                    If Not Information.IsDate(vFieldValue) Then
                        sControlResult = ""
                    Else
                        Dim TempDate6 As Date
                        sControlResult = (If(DateTime.TryParse(vFieldValue, TempDate6), TempDate6.ToString("ddd"), vFieldValue)).Trim()
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatDayOnlyShort
                    ' Format value to a Day in Short format eg 01
                    If Not Information.IsDate(vFieldValue) Then
                        sControlResult = ""
                    Else
                        Dim TempDate7 As Date
                        sControlResult = (If(DateTime.TryParse(vFieldValue, TempDate7), TempDate7.ToString("dd"), vFieldValue)).Trim()
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatDecimal
                    ' Format value to a currency
                    Dim dbNumericTemp As Double
                    If Not Double.TryParse(vFieldValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                        sControlResult = ""
                    Else
                        sControlResult = FormatNumber(ToSafeCurrency(vFieldValue))
                    End If
                Case gPMConstants.PMEFormatStyle.PMFormatCurrency
                    ' Format value to a currency
                    Dim dbNumericTemp As Double
                    If Not Double.TryParse(vFieldValue, NumberStyles.Currency, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                        sControlResult = ""
                    Else
                        sControlResult = FormatNumber(ToSafeCurrency(vFieldValue))
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatMoney
                    ' Format value to decimal pounds sterling
                    Dim dbNumericTemp2 As Double
                    If Not Double.TryParse(vFieldValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                        sControlResult = ""
                    Else
                        sControlResult = FormatNumber(ToSafeCurrency(vFieldValue))
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatWholeMoney
                    ' Format value to a whole pounds sterling
                    Dim dbNumericTemp3 As Double
                    If Not Double.TryParse(vFieldValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                        sControlResult = ""
                    Else
                        vFieldValue = CStr(gPMMaths.PMTruncateCurrency(v_vWholeValue:=CDec(vFieldValue), v_eNumberOfDP:=gPMConstants.PMECurrencyNoOfDP.pmeCurDPZero))

                        sControlResult = String.Format(vFieldValue, "#,###").Trim()
                    End If

                    ' PWF - Long was missing, quick fix.
                Case gPMConstants.PMEFormatStyle.PMFormatInteger, gPMConstants.PMEFormatStyle.PMFormatLong
                    ' Format value to an integer
                    Dim dbNumericTemp4 As Double
                    If Double.TryParse(vFieldValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                        sControlResult = String.Format(vFieldValue, "General Number").Trim()
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
                                    sControlResult = FormatNumber(ToSafeDouble(vFieldValue), 0) & "%"
                                Case Is > 0
                                    sControlResult = FormatNumber(ToSafeDouble(vFieldValue), vDecimalPlaces) & "%"
                                Case Is < -1
                                    ' Enforce minimum of 2 fixed digits
                                    sControlResult = FormatNumber(ToSafeDouble(vFieldValue), 2) & "%"

                                Case Else ' i.e. -1
                                    ' Stick to the original 2
                                    sControlResult = FormatNumber(ToSafeDouble(vFieldValue)) & "%"
                            End Select
                        Else
                            ' Stick to the original 2
                            sControlResult = FormatNumber(vFieldValue) & "%"
                        End If
                    End If

                Case gPMConstants.PMEFormatStyle.PMFormatPercentFourDecimal
                    ' Format value to a percentage
                    Dim dbNumericTemp7 As Double
                    If Not Double.TryParse(vFieldValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp7) Then
                        sControlResult = ""
                    Else
                        sControlResult = String.Format(vFieldValue, "##0.0000").Trim() & "%"
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
            vTemp.Replace(vTemp.Substring(1, 1), vTemp.Substring(1, 1).ToUpper())

            ' other characters
            For lLoop As Integer = 2 To vTemp.Length

                ' previous and current characters
                sLast = vTemp.Substring(lLoop - 1, 1)
                sChar = vTemp.Substring(lLoop, 1)

                ' previous char in search string?
                If sSearch.IndexOf(sLast) >= 0 Then
                    If sLast = "'" And lLoop > 3 Then
                        If sSearch.IndexOf(vTemp.Substring(lLoop - 3, 1)) >= 0 Then
                            sChar = sChar.ToUpper()
                        End If
                    Else
                        sChar = sChar.ToUpper()
                    End If
                End If

                ' replace character
                vTemp.Replace(vTemp.Substring(lLoop, 1), sChar)

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
        If (sString.IndexOf(Strings.ChrW(13) & Strings.ChrW(10)) + 1) Then Exit Sub

        iLoop = (sString.IndexOf(Constants.vbLf) + 1)

        Do While iLoop > 0
            sString = sString.Substring(0, iLoop - 1) & Strings.ChrW(13) & Strings.ChrW(10) & sString.Substring(iLoop + 1)
            iLoop = sString.IndexOf(Constants.vbLf.ToString, iLoop + 2)
        Loop

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".replaceVbcrWithVbcrlf")



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
    'Public Function Right(str As String, v As Integer) As String
    '    Return str.Substring(str.Length - v, v)
    'End Function
    'Public Function Trim(ByRef sValue As String) As String
    '    Return ToSafeString(sValue).Trim
    'End Function

    'Public Function Mid(ByRef sValue As String, nStartIndex As Integer, ByVal nLength As Integer) As String
    '    nLength = Math.Min(nLength, sValue.Length)
    '    Return ToSafeString(sValue).Substring(nStartIndex - 1, nLength)
    'End Function
    'Public Function Mid(ByRef sValue As String, nStartIndex As Integer) As String
    '    Return ToSafeString(sValue).Substring(nStartIndex - 1)
    'End Function
    'Public Function UBound(ByRef arrayObject() As Object, Optional ByRef nIndex As Integer = 1) As Integer
    '    Return arrayObject.GetUpperBound(nIndex)
    'End Function
    Public Function WCFUBound(ByRef arrayObject As String(), Optional ByRef nIndex As Integer = 1) As Integer
        Return arrayObject.GetUpperBound(nIndex)
    End Function
    'Public Function LBound(ByRef arrayObject As Object(), Optional ByRef nIndex As Integer = 1) As Integer
    '    Return arrayObject.GetLowerBound(nIndex)
    'End Function
    Public Function WCFLBound(ByRef arrayObject As Object(,), Optional ByRef nIndex As Integer = 1) As Integer
        Return arrayObject.GetLowerBound(nIndex)
    End Function
    Public Function WCFUBound(ByRef arrayObject As Object(,), Optional ByRef nIndex As Integer = 1) As Integer
        Return arrayObject.GetUpperBound(nIndex)
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
                    If Not regex.IsMatch(sPasswordString.Trim, sRegEx) Then
                        bIsstrongPassword = False
                        Return gPMConstants.PMEReturnCode.PMTrue
                    Else
                        bIsstrongPassword = True
                        Return gPMConstants.PMEReturnCode.PMTrue
                    End If
                End If
            Else
                If Len(sPasswordString.Trim) < 4 Then
                    'MessageBox.Show("Passwords must consist of four or more characters." & Strings.Chr(10).ToString() &
                    '                "Please choose another Password.", "E0105 - Incorrect Password Length", MessageBoxButtons.OK, MessageBoxIcon.Information)
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
            'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Is Strong Password Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsStrongPassword", excep:=ex, oDicParms:=oDict)

            Return m_lReturn
        End Try
        Return m_lReturn
    End Function

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
                    'MessageBox.Show("gPMFUnctions.ConvertCurrencyStringToValue" &
                    '                "Version: " & CStr(My.Application.Info.Version.Major) & "." & CStr(My.Application.Info.Version.Minor) & "." & CStr(My.Application.Info.Version.Revision) &
                    '                " At line: " & CStr(Information.Erl()) & "|" & Information.Err().Source & Strings.Chr(13) & Strings.Chr(10) & Strings.Chr(13) & Strings.Chr(10) &
                    '                Information.Err().Number & ":" & Information.Err().Description, Application.ProductName)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
            End Select

        Finally


        End Try
        Return result
    End Function
    Public Function FormatDocumentRef(ByVal RangeCode As String, ByVal lNumber As Integer) As String
        Try
            'Format the number
            Return RangeCode & StringsHelper.Format(lNumber, "0000000000")
        Catch
        End Try
        Return ""
    End Function
    Public Function GetDocumentLibrary(ByVal m_oDatabase As Object, ByVal nPartyCnt As Integer, ByVal PartyShortName As String) As String
        'Dim m_oDatabase As dPMDAO.Database
        Dim m_nReturn As Integer
        Dim sDocumentLibrary As String

        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("PartyCnt", ToSafeInteger(nPartyCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            .Parameters.Add("PartyShortName", ToSafeString(PartyShortName), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
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

    Private Declare Function MSPeelerMain Lib "msfilter.dll" (ByVal sHtmlFile As String, ByVal sCmdOptions As String) As Short


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
            .LoadHTMLFile(sInputFileName.ToString)
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
        tmp = tmp.Replace("&quot;", Strings.ChrW(34).ToString())
        tmp = tmp.Replace("&lt;", Strings.ChrW(60).ToString())
        tmp = tmp.Replace("&gt;", Strings.ChrW(62).ToString())
        tmp = tmp.Replace("&amp;", Strings.ChrW(38).ToString())
        tmp = tmp.Replace("&nbsp;", Strings.ChrW(32).ToString())
        For i As Integer = 1 To 255
            tmp = tmp.Replace("&#" & i & ";", Strings.ChrW(i).ToString())
        Next
        Return tmp
    End Function

    Public Function Month(ByVal dtDate As Date) As Integer
        Return dtDate.Month
    End Function
    Public Function Day(ByVal dtDate As Date) As Integer
        Return dtDate.Day
    End Function
    Public Function Year(ByVal dtDate As Date) As Integer
        Return dtDate.Year
    End Function
    Public Function Split(ByVal sString As String, ByVal indicator As String) As String()
        Return sString.Split(indicator)
    End Function
    Public Function Environ(ByVal Expression As String) As String

        Expression = ToSafeString(Expression).Trim
        If (Expression.Length = 0) Then
            Throw New ArgumentException("Argument_InvalidValue1", "Expression")
        End If

        Return Environment.GetEnvironmentVariable(Expression)
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
                iPos = sPath.IndexOf("\", iPos)

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



            Throw New System.Exception(Informations.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message + ", " + Informations.Err().HelpFile + ", " + Informations.Err().HelpContext)

        End Try
    End Function

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