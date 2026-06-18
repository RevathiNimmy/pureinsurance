Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.IO
'Modified by Vijay Pal on 5/20/2010 10:34:19 AM refer developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Form_NET.Form")> _
Public NotInheritable Class Form
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 19/08/1998
    '
    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a Policy.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 28/11/2003
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Form"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)
    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_lRLDFRecord As Integer
    Private m_lPropertyArrayCnt As Integer
    Private m_vPropertyArray(,) As Object
    Private m_iRLDFFile As Integer

    Private m_sServerListFilePath As String = ""
    Private m_sServerListFilePathDat As String = ""
    Private m_sServerListFilePathIdx As String = ""
    Private m_sSaveServerListFilePathDat As String = ""
    Private m_sSaveServerListFilePathDatIdx As String = ""
    Private m_sServerListPrefVersion As String = ""
    Private m_sServerListVersion As String = ""
    Private m_sServerPolarisFilePath As String = ""
    Private m_bServerListFileCompressed As Boolean
    Private m_iBusinessType As Integer
    Private m_sCoverDatFilePath As String = ""

    'sj 19/09/2000 - start
    Private m_sGeminiListFilePath As String = ""
    Private m_sGeminiListFilePathDat As String = ""
    Private m_sGeminiListFilePathIdx As String = ""
    Private m_sGeminiPolarisFilePath As String = ""
    Private m_bUpdateUserLists As Boolean
    Private m_sGeminiBusinessCode As String = ""

    'sj 19/09/2000 - end

    Private m_cCustomIndex As Collection

    Private m_oLists As bGEMLists.Form

    Private m_oListCustom As bGEMListCustom.Form
    Private m_oListUser As bGEMListUser.Form
    Private m_oComponentManager As Object
    Private m_oPolCall As Object
    Private m_oZipper As bPMZipper.Business

    Private Const VehicleCoverPropertyID As Integer = 9502722
    Private Const SchemeCoverPropertyID As Integer = 589826
    Private Const PremisesCoverPropertyID As Integer = 3473411

    'sj 19/09/2000 - start
    ' CalledFromGeminiII
    Private m_bCalledFromGeminiII As Boolean

    Public WriteOnly Property CalledFromGeminiII() As Boolean
        Set(ByVal Value As Boolean)
            m_bCalledFromGeminiII = Value
        End Set
    End Property

    Public WriteOnly Property BusinessType() As Integer
        Set(ByVal Value As Integer)

            m_iBusinessType = Value

        End Set
    End Property
    'sj 19/09/2000 - end


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer




        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUserName
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            ' Initialisation Code.

            ' Set Username and Password

            ' Set UserID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level


            ' Lists
            m_oLists = New bGEMLists.Form()

            If m_oLists Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oLists.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                m_oLists = Nothing
                Return result
            End If

            ' List custom
            m_oListCustom = New bGEMListCustom.Form

            If m_oListCustom Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oListCustom.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                m_oListCustom = Nothing
                Return result
            End If

            ' List User
            m_oListUser = New bGEMListUser.Form()

            If m_oListUser Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oListUser.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                m_oListUser = Nothing
                Return result
            End If


            ' File compresser
            m_oZipper = New bPMZipper.Business()

            If m_oZipper Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If m_oLists IsNot Nothing Then
                    m_oLists.Dispose()
                    m_oLists = Nothing
                End If
                If m_oListCustom IsNot Nothing Then
                    m_oListCustom.Dispose()
                    m_oListCustom = Nothing
                End If
                If m_oListUser IsNot Nothing Then
                    m_oListUser.Dispose()
                    m_oListUser = Nothing
                End If

                If Not (m_oZipper Is Nothing) Then
                    'm_lReturn = m_oListUser.Terminate
                    m_oZipper = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub

    'sj 19/09/2000 - start
    ' ***************************************************************** '
    '
    ' Name: ListUpdateToGemini
    '
    ' Description:
    '
    ' History: 19/09/2000 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function ListUpdateToGemini(ByRef r_sServerListFilePath As String, ByRef r_sServerPolarisFilePath As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim iStart, iPos As Integer
            Dim sServerListFilePath As String = ""

            '******************************************************************
            ' Get all the registry settings we need
            '******************************************************************
            m_lReturn = CType(GetRegistrySettings(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = m_lReturn

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Error getting registry settings", vApp:=ACApp, vClass:=ACClass, vMethod:="ListUpdateToGemini", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            iStart = m_sServerListFilePath.Length - 1

            For i As Integer = iStart To 0 Step -1
                If Mid(m_sServerListFilePath, i, 1) = "\" Then
                    iPos = i
                    i = 0
                End If
            Next i

            sServerListFilePath = m_sServerListFilePath.Substring(0, iPos - 1)

            ' Remove the file name part of the path

            r_sServerListFilePath = sServerListFilePath
            r_sServerPolarisFilePath = m_sServerPolarisFilePath

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ListUpdateToGemini Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ListUpdateToGemini", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GeminiToListUpdate
    '
    ' Description:
    '
    ' History: 19/09/2000 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function GeminiToListUpdate(ByVal v_sServerListFilePath As String, ByVal v_sServerPolarisFilePath As String, ByVal v_sBusinessTypeCode As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sServerListFilePath As String = ""

            sServerListFilePath = v_sServerListFilePath
            m_sGeminiPolarisFilePath = v_sServerPolarisFilePath
            m_sGeminiBusinessCode = v_sBusinessTypeCode

            m_sGeminiListFilePath = sServerListFilePath & "\" & v_sBusinessTypeCode & "List"

            m_sGeminiListFilePathDat = sServerListFilePath & "\" & v_sBusinessTypeCode & "List.dat"

            m_sGeminiListFilePathIdx = sServerListFilePath & "\" & v_sBusinessTypeCode & "List.idx"


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GeminiToListUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GeminiToListUpdate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'sj 19/09/2000 - end

    ' ***************************************************************** '
    ' Name: ListUpdateProcess (Public)
    '
    ' Description: Updates the RLDF on the server by merging
    '              information from polaris, user lists and custom
    '              user modifications
    ' ***************************************************************** '
    Public Function ListUpdateProcess() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sSequenceNumber As String = ""

            'sj 19/09/2000 - start
            m_bUpdateUserLists = True
            'sj 19/09/2000 - end

            '******************************************************************
            ' Get all the registry settings we need
            '******************************************************************
            m_lReturn = CType(GetRegistrySettings(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = m_lReturn

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Error getting registry settings", vApp:=ACApp, vClass:=ACClass, vMethod:="ListUpdateProcess", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                Return result
            End If

            'sj 19/09/2000 - start
            If m_bCalledFromGeminiII Then
                m_sServerPolarisFilePath = m_sGeminiPolarisFilePath
                m_sServerListFilePath = m_sGeminiListFilePath
                m_sServerListFilePathDat = m_sGeminiListFilePathDat
                m_sServerListFilePathIdx = m_sGeminiListFilePathIdx
                If m_sGeminiBusinessCode = "GIIH" Then
                    m_bUpdateUserLists = False
                End If
            End If
            'sj 19/09/2000 - end

            '******************************************************************
            ' Construct the full path name of the index file
            '******************************************************************
            '    m_lReturn = BuildIndexFileName()
            '    If m_lReturn <> PMTrue Then
            '
            '        ListUpdateProcess = m_lReturn
            '
            '        LogMessage m_sUsername, _
            ''            iType:=PMError, _
            ''            sMsg:="Error constructing index filename", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="ListUpdateProcess", _
            ''            vErrNo:=Err.Number, _
            ''            vErrDesc:=Err.Description
            '
            '        Exit Function
            '    End If

            '******************************************************************
            ' Rename the existing RLDF
            '******************************************************************
            If FileSystem.Dir(m_sServerListFilePathDat, FileAttribute.Normal) <> "" Then

                m_lReturn = CType(ArchiveRLDF(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = m_lReturn

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Error archiving RLDF", vApp:=ACApp, vClass:=ACClass, vMethod:="ListUpdateProcess", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result

                End If
            End If

            '******************************************************************
            ' Update the RLDF
            '******************************************************************
            m_lReturn = CType(UpdateRLDFList(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = m_lReturn

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Error updating list", vApp:=ACApp, vClass:=ACClass, vMethod:="ListUpdateProcess", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            End If

            '******************************************************************
            ' IF there was an error restore the original RLDF
            '******************************************************************
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RestoreFiles(), gPMConstants.PMEReturnCode)
            End If

            '******************************************************************
            ' Update the registry
            '******************************************************************
            'sj 19/9/2000 - start
            If Not m_bCalledFromGeminiII Then
                'sj 19/9/2000 - end

                m_lReturn = CType(UpdateRegistrySettings(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = m_lReturn

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Error registry settings", vApp:=ACApp, vClass:=ACClass, vMethod:="ListUpdateProcess", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    m_lReturn = CType(RestoreFiles(), gPMConstants.PMEReturnCode)

                    Return result

                End If
            End If

            '******************************************************************
            ' Compress the data files if necessary
            '******************************************************************
            If m_bServerListFileCompressed Then

                m_lReturn = CType(CompressFiles(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = m_lReturn

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Error compressing files", vApp:=ACApp, vClass:=ACClass, vMethod:="ListUpdateProcess", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result

                    m_lReturn = CType(RestoreFiles(), gPMConstants.PMEReturnCode)

                End If
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="ListUpdateProcess Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ListUpdateProcess", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetRegistrySettings (Private)
    '
    ' Description: Archives a copy of the RLDF

    ' ***************************************************************** '
    Private Function GetRegistrySettings() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily
        Dim sTemp As String = ""

        eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
        eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLServer
        eProductFamily = gPMConstants.PMEProductFamily.pmePFGemini

        'sj Pick up the polaris version from Polaris region of the registry
        eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLCommon
        ' Server List Version

        'MN160799 - Check for businesstype
        Select Case m_iBusinessType
            Case GemBusinessTypeMV

                m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=GEMCommonHKJPolarisAppVer, r_sSettingValue:=sTemp, v_sSubKey:=GEMRegKeyListPolaris), gPMConstants.PMEReturnCode)

            Case Else
                m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=GEMCommonPolarisAppVer, r_sSettingValue:=sTemp, v_sSubKey:=GEMRegKeyListPolaris), gPMConstants.PMEReturnCode)

        End Select
        '    m_lReturn = GetPMRegSetting( _
        ''        v_lPMERegSettingRoot:=eRegSettingRoot, _
        ''        v_lPMEProductFamily:=eProductFamily, _
        ''        v_lPMERegSettingLevel:=eRegSettingLevel, _
        ''        v_sSettingName:=GEMServerListVersion, _
        ''        r_sSettingValue:=sTemp, _
        ''        v_sSubKey:=GEMRegKeyListManagement)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or sTemp = "" Then

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:="GetRegistrySettings Failed - Polaris App Version", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRegistrySettings")

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_sServerListVersion = sTemp.Trim()
        eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLServer
        ' Server List Pref Version

        Select Case m_iBusinessType
            Case GemBusinessTypeMV

                m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=GEMServerHKJListPrefVersion, r_sSettingValue:=sTemp, v_sSubKey:=GEMRegKeyListManagement), gPMConstants.PMEReturnCode)

            Case Else

                m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=GEMServerListPrefVersion, r_sSettingValue:=sTemp, v_sSubKey:=GEMRegKeyListManagement), gPMConstants.PMEReturnCode)

        End Select

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or sTemp = "" Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetRegistrySettings Failed - Server List Pref Version", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRegistrySettings")

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_sServerListPrefVersion = sTemp.Trim()


        Select Case m_iBusinessType
            Case GemBusinessTypeMV

                ' Server List File Compressed
                m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=GEMServerHKJListFileCompressed, r_sSettingValue:=sTemp, v_sSubKey:=GEMRegKeyListManagement), gPMConstants.PMEReturnCode)

            Case Else

                ' Server List File Compressed
                m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=GEMServerListFileCompressed, r_sSettingValue:=sTemp, v_sSubKey:=GEMRegKeyListManagement), gPMConstants.PMEReturnCode)

        End Select

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or sTemp = "" Then

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetRegistrySettings Failed - Server List File Compressed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRegistrySettings")

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_bServerListFileCompressed = sTemp.Trim() = "Y"

        'get CoverDatFilePath
        m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=GEMCoverDatFilePath, r_sSettingValue:=sTemp, v_sSubKey:=GEMRegKeyListManagement), gPMConstants.PMEReturnCode)

        m_sCoverDatFilePath = sTemp.Trim()


        Select Case m_iBusinessType
            Case GemBusinessTypeMV

                ' Server List File Path
                m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=GEMServerHKJListFilePath, r_sSettingValue:=sTemp, v_sSubKey:=GEMRegKeyListManagement), gPMConstants.PMEReturnCode)

            Case Else

                ' Server List File Path
                m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=GEMServerListFilePath, r_sSettingValue:=sTemp, v_sSubKey:=GEMRegKeyListManagement), gPMConstants.PMEReturnCode)

        End Select

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or sTemp = "" Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetRegistrySettings Failed - Server List File Path", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRegistrySettings")

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_sServerListFilePath = sTemp.Trim()

        m_sServerListFilePathIdx = m_sServerListFilePath & ".idx"
        m_sServerListFilePathDat = m_sServerListFilePath & ".dat"



        Select Case m_iBusinessType
            Case GemBusinessTypeMV

                ' Server List Input File Path
                m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=GEMServerHKJPolarisFilePath, r_sSettingValue:=sTemp, v_sSubKey:=GEMRegKeyListManagement), gPMConstants.PMEReturnCode)


            Case Else

                ' Server List Input File Path
                m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=GEMServerPolarisFilePath, r_sSettingValue:=sTemp, v_sSubKey:=GEMRegKeyListManagement), gPMConstants.PMEReturnCode)

        End Select

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or sTemp = "" Then

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetRegistrySettings Failed - Server Polaris File Path", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRegistrySettings")


            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_sServerPolarisFilePath = sTemp.Trim()

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateRegistrySettings (Private)
    '
    ' Description: Updates the customer preference registry setting

    ' ***************************************************************** '
    Private Function UpdateRegistrySettings() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily
        Dim sTemp As String = ""
        Dim iVersion As Integer

        eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
        eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLServer
        eProductFamily = gPMConstants.PMEProductFamily.pmePFGemini

        ' Server List Version
        '    m_lReturn = GetPMRegSetting( _
        ''        v_lPMERegSettingRoot:=eRegSettingRoot, _
        ''        v_lPMEProductFamily:=eProductFamily, _
        ''        v_lPMERegsettinglevel:=eRegSettingLevel, _
        ''        v_sSettingName:=GEMServerListVersion, _
        ''        r_sSettingValue:=sTemp, _
        ''        v_sSubKey:=GEMRegKeyListManagement)
        '
        '    If m_lReturn <> PMTrue _
        ''    Or sTemp = "" Then
        '        UpdateRegistrySettings = PMFalse
        '        Exit Function
        '    End If

        '    m_sServerListVersion = Trim(sTemp)

        'Increment the version number
        iVersion = CInt(m_sServerListPrefVersion)
        iVersion += 1
        sTemp = CStr(iVersion)

        ' Server List Pref Version
        m_lReturn = CType(gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=GEMServerListPrefVersion, v_sSettingValue:=sTemp, v_sSubKey:=GEMRegKeyListManagement), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or sTemp = "" Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_sServerListPrefVersion = sTemp.Trim()

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CompressFiles (Private)
    '
    ' Description: Updates the customer preference registry setting

    ' ***************************************************************** '
    Private Function CompressFiles() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim bReturn As Boolean
        Dim sFileIn, sFileOut As String

        ' First do the ".dat" file

        sFileIn = m_sServerListFilePathDat
        sFileOut = m_sServerListFilePathDat & ".Z"

        bReturn = m_oZipper.ZipFile(sFileIn, sFileOut)
        If Not bReturn Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        File.Delete(sFileIn)
        FileSystem.Rename(sFileOut, sFileIn)

        ' Now do the ".idx" file

        sFileIn = m_sServerListFilePathIdx
        sFileOut = m_sServerListFilePathIdx & ".Z"

        bReturn = m_oZipper.ZipFile(sFileIn, sFileOut)
        If Not bReturn Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        File.Delete(sFileIn)
        FileSystem.Rename(sFileOut, sFileIn)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: RestoreFiles (Private)
    '
    ' Description: Restore the existing RLDF if there is an error

    ' ***************************************************************** '
    Private Function RestoreFiles() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        File.Delete(m_sServerListFilePathDat)
        File.Delete(m_sServerListFilePathIdx)
        FileSystem.Rename(m_sSaveServerListFilePathDat, m_sServerListFilePathDat)
        FileSystem.Rename(m_sSaveServerListFilePathDatIdx, m_sServerListFilePathIdx)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: BuildIndexFileName (Private)
    '
    ' Description: Build the full path of the index file

    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (BuildIndexFileName) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function BuildIndexFileName() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'Dim iLen, i, iDotPos As Integer
    'Dim sTemp, sExt As String
    '
    'iLen = m_sServerListFilePath.Length
    '
    'i = (m_sServerListFilePath.IndexOf("."c) + 1)
    'While i > 0
    'iDotPos = i
    'i = Strings.InStr(i + 1, m_sServerListFilePath, ".")
    'End While
    'If iDotPos > 1 Then
    'sTemp = m_sServerListFilePath.Substring(0, iDotPos - 1)
    'sExt = m_sServerListFilePath.Substring(m_sServerListFilePath.Length - (iLen - iDotPos))
    'Else
    'sTemp = m_sServerListFilePath
    'sExt = ""
    'End If
    '
    'm_sServerListFilePathIdx = sTemp & ".idx"
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="BuildIndexFileName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BuildIndexFileName", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: ArchiveRLDF (Private)
    '
    ' Description: Archives a copy of the RLDF

    ' ***************************************************************** '
    Private Function ArchiveRLDF() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim sSequenceNumber As String = ""
        '    Dim iDotPos As Integer
        '    Dim sTemp As String
        '    Dim iLen As Integer
        '    Dim sExt As String
        '    Dim i As Integer

        '******************************************************************
        ' sequence number is built from the date and time
        '******************************************************************
        sSequenceNumber = DateTime.Today.ToString("yyyyMMdd") & DateTimeHelper.Time.ToString("HHmmss")

        '******************************************************************
        ' Split the file name into name and extention
        '******************************************************************
        '    i = InStr(1, m_sServerListFilePath, ".")
        '    iLen = Len(m_sServerListFilePath)
        '
        '    While i > 0
        '        iDotPos = i
        '        i = InStr(i + 1, m_sServerListFilePath, ".")
        '    Wend
        '    If iDotPos > 1 Then
        '        sTemp = Left(m_sServerListFilePath, iDotPos - 1)
        '        sExt = Right(m_sServerListFilePath, iLen - iDotPos)
        '    Else
        '        sTemp = m_sServerListFilePath
        '        sExt = ""
        '    End If
        '
        '
        '    m_sSaveServerListFilePathDat = _
        ''        sTemp & sSequenceNumber & "." & sExt
        '    m_sSaveServerListFilePathDatIdx = _
        ''        sTemp & sSequenceNumber & ".idx"

        m_sSaveServerListFilePathDat = m_sServerListFilePath & sSequenceNumber & ".dat"
        m_sSaveServerListFilePathDatIdx = m_sServerListFilePath & sSequenceNumber & ".idx"
        '******************************************************************
        ' Rename the existing RLDF
        '******************************************************************
        FileSystem.Rename(m_sServerListFilePathDat, m_sSaveServerListFilePathDat)
        FileSystem.Rename(m_sServerListFilePathIdx, m_sSaveServerListFilePathDatIdx)
        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateRLDFList (Public)
    '
    ' Description: Archives a copy of the RLDF

    ' ***************************************************************** '
    Private Function UpdateRLDFList() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim iInputFile As Integer
        Dim vListUserArray(,) As Object = Nothing
        Dim vListsArray(,) As Object = Nothing


        m_iRLDFFile = FileSystem.FreeFile()
        iInputFile = FileSystem.FreeFile()


        '******************************************************************
        ' Open the RLDF
        '******************************************************************
        FileSystem.FileOpen(m_iRLDFFile, m_sServerListFilePathDat, OpenMode.Random)

        '******************************************************************
        ' If the polaris data dump exists build RLDF
        '******************************************************************
        If FileSystem.Dir(m_sServerPolarisFilePath, FileAttribute.Normal) <> "" Then

            m_lReturn = CType(BuildFileFromPolaris(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        '******************************************************************
        ' Do we have any user defined lists
        '******************************************************************
        If m_bUpdateUserLists Then


            m_lReturn = CType(GetUserDefinedList(r_vListsArray:=vListsArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound And Information.IsArray(vListsArray) Then


                For i As Integer = 0 To vListsArray.GetUpperBound(1)



                    m_lReturn = CType(GetDetailsFromListUser(v_lListId:=CInt(vListsArray(ACLists_ListID, i)), r_vListUserArray:=vListUserArray), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then



                        m_lReturn = CType(BuildFileFromListUser(v_sPropertyId:=CStr(vListsArray(ACLists_PropertyID, i)), v_vListUserArray:=vListUserArray), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    End If

                Next i

            End If

        End If

        '******************************************************************
        ' Close the RLDF
        '******************************************************************
        FileSystem.FileClose(m_iRLDFFile)

        '******************************************************************
        ' Create the RLDF index (.idx)
        '******************************************************************
        If Information.IsArray(m_vPropertyArray) Then

            m_lReturn = CType(CreateRLDFIndex(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: BuildFileFromPolaris (Public)
    '
    ' Description: Archives a copy of the RLDF

    ' ***************************************************************** '
    Private Function BuildFileFromPolaris() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim iInputFile As Integer
        Dim sInputRecord As String = ""
        Dim bValidZipFile As Boolean
        Dim sFileIn, sFileOut As String
        Dim bReturn As Boolean
        Dim sFileLength As String 'DN 14/09/00
        '    Dim sWinSysPath As String
        '    Dim lWinSysPath As Long

        ReDim m_vPropertyArray(2, 0)
        m_lRLDFRecord = 1

        iInputFile = FileSystem.FreeFile()

        '******************************************************************
        ' Is the input file compressed
        '******************************************************************
        '    sWinSysPath = Space(100)
        '    lWinSysPath = 100
        '
        '    m_lReturn& = GetSystemDirectory(sWinSysPath$, lWinSysPath&)
        '
        '    sWinSysPath$ = Left$(sWinSysPath$, m_lReturn&)

        sFileIn = m_sServerPolarisFilePath
        bReturn = m_oZipper.ValidZIPFile(sFileIn, bValidZipFile)
        If Not bReturn Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '******************************************************************
        ' If the input file is compressed then uncompress it
        '******************************************************************
        If bValidZipFile Then

            'DN 14/09/00 - Find Length of data file
            sFileLength = CStr(FileSystem.Dir(m_sServerPolarisFilePath, FileAttribute.Normal).Length)

            sFileIn = m_sServerPolarisFilePath
            'DN 14/09/00 - Changed from using WinSys folder as can have permission issues
            'sFileOut = sWinSysPath & "\temp.dat"
            sFileOut = m_sServerPolarisFilePath.Substring(0, CInt(m_sServerPolarisFilePath.Length - CDbl(sFileLength))) & "temp.dat"

            bReturn = m_oZipper.UnZipFile(sFileIn, sFileOut)
            If Not bReturn Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            File.Delete(sFileIn)
            FileSystem.Rename(sFileOut, sFileIn)
        End If

        '******************************************************************
        ' Open the input file
        '******************************************************************
        FileSystem.FileOpen(iInputFile, m_sServerPolarisFilePath, OpenMode.Input)

        '******************************************************************
        ' The first three lines are rubbish
        '******************************************************************

        '    Line Input #iInputFile, sInputRecord
        '    Line Input #iInputFile, sInputRecord
        '    Line Input #iInputFile, sInputRecord
        '    Line Input #iInputFile, sInputRecord
        '    Line Input #iInputFile, sInputRecord
        '    Line Input #iInputFile, sInputRecord

        While Not FileSystem.EOF(iInputFile)

            sInputRecord = FileSystem.LineInput(iInputFile)

            If sInputRecord <> "" Then

                m_lReturn = CType(ProcessInputRecord(v_sInputRecord:=sInputRecord), gPMConstants.PMEReturnCode)

            End If

        End While

        '(IB)030999 - sort properties if required
        'also prepare cover.dat & coverbreak.dat
        Dim lBegin, lEnd As Integer
        Dim bChanged As Boolean
        Dim RecordA As MainModule.RLDFDetailRecord = MainModule.RLDFDetailRecord.CreateInstance()
        Dim RecordB As MainModule.RLDFDetailRecord = MainModule.RLDFDetailRecord.CreateInstance()
        Dim lCoverFileHandle As Integer
        Dim sRecordABICode, sRecordDescription As String
        Dim CoverArray(,) As Object = Nothing


        'for each property
        For lPropertyIndex As Integer = 0 To m_lPropertyArrayCnt - 1
            'does this property contain custom entries?
            If CBool(m_vPropertyArray(ACPACustom, lPropertyIndex)) Then
                'yes - ok, better sort list then so... work out boundaries of data
                lBegin = CInt(m_vPropertyArray(ACPARecordIndex, lPropertyIndex))
                If lPropertyIndex < (m_lPropertyArrayCnt - 1) Then
                    lEnd = CInt(CDbl(m_vPropertyArray(ACPARecordIndex, lPropertyIndex + 1)) - 1)
                Else
                    lEnd = m_lRLDFRecord
                End If
                'now sort!
                bChanged = True
                Do While bChanged
                    bChanged = False
                    'for each record
                    For lRecord As Integer = lBegin To lEnd - 1
                        'get recs,compare and swap write if out of sequence!

                        FileSystem.FileGet(m_iRLDFFile, RecordA, lRecord)

                        FileSystem.FileGet(m_iRLDFFile, RecordB, lRecord + 1)
                        If RecordA.Description.Value > RecordB.Description.Value Then
                            bChanged = True

                            FileSystem.FilePutObject(m_iRLDFFile, RecordA, lRecord + 1)

                            FileSystem.FilePutObject(m_iRLDFFile, RecordB, lRecord)
                        End If
                    Next lRecord
                Loop
            End If
            'is this a cover list?
            If m_sCoverDatFilePath <> "" And (CStr(m_vPropertyArray(ACPAPropertyId, lPropertyIndex)).IndexOf("rec") + 1) = 0 Then
                If CDbl(m_vPropertyArray(ACPAPropertyId, lPropertyIndex)) = SchemeCoverPropertyID Or CDbl(m_vPropertyArray(ACPAPropertyId, lPropertyIndex)) = VehicleCoverPropertyID Or CDbl(m_vPropertyArray(ACPAPropertyId, lPropertyIndex)) = PremisesCoverPropertyID Then
                    'dump list
                    lBegin = CInt(m_vPropertyArray(ACPARecordIndex, lPropertyIndex))
                    If lPropertyIndex < (m_lPropertyArrayCnt - 1) Then
                        lEnd = CInt(CDbl(m_vPropertyArray(ACPARecordIndex, lPropertyIndex + 1)) - 1)
                    Else
                        lEnd = m_lRLDFRecord
                    End If
                    For lRecord As Integer = lBegin To lEnd
                        'get recs

                        FileSystem.FileGet(m_iRLDFFile, RecordA, lRecord)
                        'print recs to cover.dat
                        If Strings.Asc(RecordA.ABICode.Value.Substring(0, 1)(0)) <> 0 Then
                            sRecordABICode = RecordA.ABICode.Value.Trim()
                            sRecordDescription = RecordA.Description.Value.Trim()
                            If sRecordABICode.Trim() <> "" Then
                                If Not Information.IsArray(CoverArray) Then
                                    ReDim CoverArray(1, 1)
                                Else

                                    ReDim Preserve CoverArray(1, CoverArray.GetUpperBound(1) + 1)
                                End If


                                CoverArray(1, CoverArray.GetUpperBound(1)) = sRecordABICode


                                CoverArray(2, CoverArray.GetUpperBound(1)) = sRecordDescription
                            End If
                        End If
                    Next lRecord
                End If
            End If
        Next lPropertyIndex
        FileSystem.FileClose(iInputFile)

        lCoverFileHandle = FileSystem.FreeFile()

        Dim bDumpedAny As Boolean
        If m_sCoverDatFilePath <> "" Then
            'create empty coverbreak.dat file
            FileSystem.FileOpen(lCoverFileHandle, m_sCoverDatFilePath & "coverbreak.dat", OpenMode.Output)
            FileSystem.FileClose(lCoverFileHandle)
            'create cover.dat file
            FileSystem.FileOpen(lCoverFileHandle, m_sCoverDatFilePath & "cover.dat", OpenMode.Output)
            'sort cover array
            bChanged = True
            Do While bChanged
                bChanged = False

                For iCoverPointer As Integer = 1 To CoverArray.GetUpperBound(1) - 1



                    If CoverArray(2, iCoverPointer) > CoverArray(2, iCoverPointer + 1) Then


                        CoverArray(1, 0) = CoverArray(1, iCoverPointer + 1)


                        CoverArray(2, 0) = CoverArray(2, iCoverPointer + 1)


                        CoverArray(1, iCoverPointer + 1) = CoverArray(1, iCoverPointer)


                        CoverArray(2, iCoverPointer + 1) = CoverArray(2, iCoverPointer)


                        CoverArray(1, iCoverPointer) = CoverArray(1, 0)


                        CoverArray(2, iCoverPointer) = CoverArray(2, 0)
                        bChanged = True
                    End If
                Next iCoverPointer
            Loop
            'now filter out duplicate cover types!

            For iCoverPointer As Integer = 1 To CoverArray.GetUpperBound(1) - 1


                If CoverArray(1, iCoverPointer).Equals(CoverArray(1, iCoverPointer + 1)) Then
                    CoverArray(1, iCoverPointer) = ""
                End If
            Next iCoverPointer
            'now dump cover array to cover.dat file
            bDumpedAny = False

            For iCoverPointer As Integer = 1 To CoverArray.GetUpperBound(1)

                If CStr(CoverArray(1, iCoverPointer)) <> "" Then
                    If bDumpedAny Then FileSystem.PrintLine(lCoverFileHandle)
                    FileSystem.Print(lCoverFileHandle, CoverArray(1, iCoverPointer) & ",")
                    FileSystem.Print(lCoverFileHandle, CoverArray(2, iCoverPointer))
                    bDumpedAny = True
                End If
            Next iCoverPointer
            FileSystem.FileClose(lCoverFileHandle)
        End If
        FileSystem.FileClose(lCoverFileHandle)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ProcessInputRecord (Public)
    '
    ' Description: Archives a copy of the RLDF

    ' ***************************************************************** '
    Private Function ProcessInputRecord(ByRef v_sInputRecord As String) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim uHeaderRecord As MainModule.RLDFHeaderRecord = MainModule.RLDFHeaderRecord.CreateInstance()
        Dim uDetailrecord As MainModule.RLDFDetailRecord = MainModule.RLDFDetailRecord.CreateInstance()

        Dim vListCustomArray As Object = Nothing
        Dim sDescription, sABICode, sPropertyId As String

        Static sSavePropertyId As String = ""
        Static bCheckForCustomList As Boolean

        'sj 19/09/2000 - start
        'Chnages for new format file for windows product writer
        'sPropertyId = Trim(Mid(v_sInputRecord, 1, 11))
        'sABICode = Trim(Mid(v_sInputRecord, 84))
        'sDescription = Trim(Mid(v_sInputRecord, 12, 83))

        sPropertyId = Mid(v_sInputRecord, 1, 11).Trim()
        sABICode = Mid(v_sInputRecord, 82, 10).Trim()
        sDescription = Mid(v_sInputRecord, 12, 70).Trim()
        'sj 19/09/2000- end

        If sSavePropertyId = "" Or sSavePropertyId <> sPropertyId Then

            ReDim Preserve m_vPropertyArray(2, m_lPropertyArrayCnt)
            m_vPropertyArray(MainModule.ACPAPropertyId, m_lPropertyArrayCnt) = CDbl(sPropertyId)
            m_vPropertyArray(MainModule.ACPARecordIndex, m_lPropertyArrayCnt) = m_lRLDFRecord
            m_vPropertyArray(MainModule.ACPACustom, m_lPropertyArrayCnt) = False
            m_lPropertyArrayCnt += 1

            uHeaderRecord.PropertyId.Value = sPropertyId


            FileSystem.FilePutObject(m_iRLDFFile, uHeaderRecord, m_lRLDFRecord)
            m_lRLDFRecord += 1

            '        If sPropertyId = "196650" Then
            '            MsgBox sAbiCode
            '        End If


            m_lReturn = CType(GetListCustom(v_sPropertyId:=sPropertyId, r_vListCustomArray:=vListCustomArray), gPMConstants.PMEReturnCode)

            bCheckForCustomList = Not (m_lReturn = gPMConstants.PMEReturnCode.PMNotFound)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            sSavePropertyId = sPropertyId

        End If
        '    If sPropertyId = "196646" Then
        '        MsgBox sAbiCode
        '    End If

        m_lReturn = gPMConstants.PMEReturnCode.PMFalse
        If bCheckForCustomList Then

            'We have a custom list for this polaris property id

            m_lReturn = CType(CheckCustomIndex(r_vListCustomArray:=vListCustomArray, v_sABICode:=sABICode), gPMConstants.PMEReturnCode)

        End If

        uDetailrecord.ABICode.Value = sABICode
        uDetailrecord.PropertyId.Value = sPropertyId

        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then


            If CStr(vListCustomArray(ACListCustom_Command)) = "C" Then


                'developer guide no. 236
                'uDetailrecord.Description.Value = CType(vListCustomArray(ACListCustom_Text), FixedLengthString)
                uDetailrecord.Description.Value = vListCustomArray(ACListCustom_Text)


                FileSystem.FilePutObject(m_iRLDFFile, uDetailrecord, m_lRLDFRecord)
                m_lRLDFRecord += 1
                m_vPropertyArray(ACPACustom, m_lPropertyArrayCnt - 1) = True

            End If
        Else

            uDetailrecord.Description.Value = sDescription


            FileSystem.FilePutObject(m_iRLDFFile, uDetailrecord, m_lRLDFRecord)
            m_lRLDFRecord += 1

        End If

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: GetListCustom (Public)
    '
    ' Description: Gets any customised objects from the database

    ' ***************************************************************** '
    'Modified by Vijay Pal on 5/20/2010 10:36:58 AM refer developer guide no. 33
    'Private Function GetListCustom(ByVal v_sPropertyId As String, ByRef r_vListCustomArray( ,  ) As String) As Integer
    Private Function GetListCustom(ByVal v_sPropertyId As String, ByRef r_vListCustomArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim i As Integer
        Dim sABICode As String = ""
        Dim vFieldArray As Object = Nothing

        ReDim r_vListCustomArray(ACListCustomFieldArraySize, 0)

        m_cCustomIndex = New Collection()

        m_lReturn = m_oListCustom.GetDetails(v_vPropertyID:=v_sPropertyId)

        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        i = 0

        While m_lReturn <> gPMConstants.PMEReturnCode.PMEOF

            m_lReturn = m_oListCustom.GetNext(r_vFieldArray:=vFieldArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = gPMConstants.PMEReturnCode.PMEOF
            Else
                ReDim Preserve r_vListCustomArray(ACListCustomFieldArraySize, i)



                r_vListCustomArray(ACListCustom_ListCustomID, i) = vFieldArray(ACListCustom_ListCustomID)


                r_vListCustomArray(ACListCustom_PositionID, i) = vFieldArray(ACListCustom_PositionID)


                r_vListCustomArray(ACListCustom_ValueID, i) = vFieldArray(ACListCustom_ValueID)


                r_vListCustomArray(ACListCustom_Text, i) = vFieldArray(ACListCustom_Text)


                r_vListCustomArray(ACListCustom_AbiCode, i) = vFieldArray(ACListCustom_AbiCode)


                r_vListCustomArray(ACListCustom_Command, i) = vFieldArray(ACListCustom_Command)


                r_vListCustomArray(ACListCustom_PropertyID, i) = vFieldArray(ACListCustom_PropertyID)


                sABICode = CStr(vFieldArray(ACListCustom_AbiCode)).Trim()

                m_cCustomIndex.Add(vFieldArray, sABICode)
            End If

        End While

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetUserDefinedList (Public)
    '
    ' Description: Gets all the user defined lists
    '
    ' ***************************************************************** '
    Private Function GetUserDefinedList(ByRef r_vListsArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim vListArray() As Object = Nothing
        Dim i As Integer

        m_lReturn = m_oLists.GetDetails()

        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ReDim r_vListsArray(ACListsFieldArraySize, 0)
        i = 0

        While m_lReturn <> gPMConstants.PMEReturnCode.PMEOF

            m_lReturn = m_oLists.GetNext(r_vFieldArray:=vListArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = gPMConstants.PMEReturnCode.PMEOF
            Else

                ReDim Preserve r_vListsArray(ACListsFieldArraySize, i)


                r_vListsArray(ACLists_ListID, i) = vListArray(ACLists_ListID)


                r_vListsArray(ACLists_PropertyID, i) = vListArray(ACLists_PropertyID)


                r_vListsArray(ACLists_Description, i) = vListArray(ACLists_Description)
                i += 1

            End If

        End While


        Return result

    End Function
    ' ***************************************************************** '
    ' Name: GetDetailsFromListUser (Public)
    '
    ' Description: Gets the details for one user defined list
    '
    ' ***************************************************************** '
    Private Function GetDetailsFromListUser(ByVal v_lListId As Integer, ByRef r_vListUserArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim vListUserArray() As Object = Nothing
        Dim i As Integer

        i = 0

        m_lReturn = m_oListUser.GetDetails(v_lListId)

        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ReDim r_vListUserArray(ACListUserFieldArraySize, 0)

        While m_lReturn <> gPMConstants.PMEReturnCode.PMEOF

            m_lReturn = m_oListUser.GetNext(r_vFieldArray:=vListUserArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = gPMConstants.PMEReturnCode.PMEOF
            Else

                ReDim Preserve r_vListUserArray(ACListUserFieldArraySize, i)


                r_vListUserArray(ACListUser_ListID, i) = vListUserArray(ACListUser_ListID)


                r_vListUserArray(ACListUser_ListUserID, i) = vListUserArray(ACListUser_ListUserID)


                r_vListUserArray(ACListUser_Text, i) = vListUserArray(ACListUser_Text)


                r_vListUserArray(ACListUser_AbiCode, i) = vListUserArray(ACListUser_AbiCode)
                i += 1

            End If

        End While


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMEOF Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Information.IsArray(r_vListUserArray) Then
            result = gPMConstants.PMEReturnCode.PMNotFound
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: BuildFileFromListUser (Public)
    '
    ' Description: Builds the RLDF from a given user list
    '
    ' ***************************************************************** '
    Private Function BuildFileFromListUser(ByVal v_sPropertyId As String, ByVal v_vListUserArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim uHeaderRecord As MainModule.RLDFHeaderRecord = MainModule.RLDFHeaderRecord.CreateInstance()
        Dim uDetailrecord As MainModule.RLDFDetailRecord = MainModule.RLDFDetailRecord.CreateInstance()
        Dim sDescription, sABICode As String

        '*****************************************************************
        ' Update the property array
        '*****************************************************************
        ReDim Preserve m_vPropertyArray(2, m_lPropertyArrayCnt)
        m_vPropertyArray(ACPAPropertyId, m_lPropertyArrayCnt) = v_sPropertyId
        m_vPropertyArray(ACPARecordIndex, m_lPropertyArrayCnt) = m_lRLDFRecord
        m_lPropertyArrayCnt += 1

        '*****************************************************************
        ' Write the header to the RLDF
        '*****************************************************************
        uHeaderRecord.PropertyId.Value = v_sPropertyId


        FileSystem.FilePutObject(m_iRLDFFile, uHeaderRecord, m_lRLDFRecord)
        m_lRLDFRecord += 1

        '*****************************************************************
        ' Now loop arround the array writing the details to the RLDF
        '*****************************************************************
        For i As Integer = 0 To v_vListUserArray.GetUpperBound(1)


            sABICode = CStr(v_vListUserArray(ACListUser_AbiCode, i))

            sDescription = CStr(v_vListUserArray(ACListUser_Text, i))

            uDetailrecord.ABICode.Value = sABICode
            uDetailrecord.PropertyId.Value = v_sPropertyId
            uDetailrecord.Description.Value = sDescription


            FileSystem.FilePutObject(m_iRLDFFile, uDetailrecord, m_lRLDFRecord)
            m_lRLDFRecord += 1

        Next i

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CheckCustomIndex (Public)
    '
    ' Description: Creates the index for the RLDF

    ' ***************************************************************** '
    Private Function CheckCustomIndex(ByRef r_vListCustomArray() As Object, ByVal v_sABICode As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            r_vListCustomArray = m_cCustomIndex(v_sABICode)

            Return result

        Catch
            Return gPMConstants.PMEReturnCode.PMError
        End Try

    End Function

    ' ***************************************************************** '
    ' Name: CreateRLDFIndex (Public)
    '
    ' Description: Creates the index for the RLDF

    ' ***************************************************************** '
    Private Function CreateRLDFIndex() As Integer

        Dim result As Integer = 0
        Dim iIndexFile As Integer


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim sPropertyId As String = ""
        Dim lRecordNumber As Integer
        Dim uindexrecord As MainModule.RLDFIndexRecord = MainModule.RLDFIndexRecord.CreateInstance()
        Dim i As Integer

        i = 0

        iIndexFile = FileSystem.FreeFile()

        '******************************************************************
        ' Open the index file
        '******************************************************************
        FileSystem.FileOpen(iIndexFile, m_sServerListFilePathIdx, OpenMode.Random)

        '******************************************************************
        ' Loop arround the arrray writing records
        '******************************************************************
        For i = 0 To m_vPropertyArray.GetUpperBound(1)

            sPropertyId = CStr(m_vPropertyArray(ACPAPropertyId, i))
            lRecordNumber = CInt(m_vPropertyArray(ACPARecordIndex, i))

            uindexrecord.PropertyId.Value = sPropertyId
            uindexrecord.RecordNumber = lRecordNumber


            FileSystem.FilePutObject(iIndexFile, uindexrecord, i + 1)

        Next i

        '******************************************************************
        ' close the index file
        '******************************************************************
        FileSystem.FileClose(iIndexFile)

        Return result

    End Function
    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
