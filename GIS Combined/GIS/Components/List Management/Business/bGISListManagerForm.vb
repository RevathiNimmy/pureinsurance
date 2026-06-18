Option Strict Off
Option Explicit On
'developer guide no. 129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Form_NET.Form")>
Public NotInheritable Class Form
    Implements IDisposable

    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer

    Private Const ACClass As String = "Form"
    Private m_bCloseDatabase As Boolean

    Private m_lCurrentRecord As Integer

    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    ' Task
    Private m_iTask As Integer
    ' Navigate
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_lInsuranceFileID As Integer
    ' Risk ID
    Private m_lRiskID As Integer

    Private m_bDataModelLevel As Boolean
    Private m_oLookup As BPMLOOKUP.Business

    Public ReadOnly Property Task() As Integer
        Get
            Return m_iTask
        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get
            Return m_lNavigate
        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get
            Return m_lProcessMode
        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get
            Return m_sTransactionType
        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get
            Return m_dtEffectiveDate
        End Get
    End Property

    Public Property SourceID() As Integer
        Get
            Return m_iSourceID
        End Get
        Set(ByVal Value As Integer)
            m_iSourceID = Value
        End Set
    End Property

    Public ReadOnly Property DataModelLevel() As Boolean
        Get
            Return m_bDataModelLevel
        End Get
    End Property

    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_dtEffectiveDate = DateTime.Now
            m_lRiskID = 0

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
            End If
        End If
        Me.disposedValue = True
    End Sub


    Public Function GetRegSettings(ByRef sResult As String, ByRef sAppName As String, ByRef sSection As String, ByRef sKey As String, Optional ByRef vDefault As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRegSettings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRegSettings", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function GetServerPMRegSetting(ByVal v_lPMERegSettingRoot As Integer, ByVal v_lPMEProductFamily As Integer, ByVal v_lPMERegSettingLevel As Integer, ByVal v_sSettingName As String, ByRef r_sSettingValue As String, Optional ByVal v_sSubKey As String = "") As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=v_lPMERegSettingRoot, v_lPMEProductFamily:=v_lPMEProductFamily, v_lPMERegSettingLevel:=v_lPMERegSettingLevel, v_sSettingName:=v_sSettingName, r_sSettingValue:=r_sSettingValue), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetServerPMRegSetting Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetServerPMRegSetting", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function GetServerSettings(ByRef r_sServerListFilePath As String, ByRef r_sServerListVersion As String, ByRef r_sServerListPrefVersion As String, ByRef r_bServerListFileCompressed As Boolean, ByVal v_sGISDataModelCode As String, Optional ByVal v_sSellerCode As String = "") As Integer
        Dim result As Integer = 0
        Dim sSubKey As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
            Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
            Dim eProductFamily As gPMConstants.PMEProductFamily
            Dim sTemp As String = ""

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLServer

            ' Look in Data Model Specific Sub Key First
            sSubKey = GISSharedConstants.GISRegSubKey & "\" & v_sGISDataModelCode.Trim() & "\" & GEMRegKeyListManagement

            ' Server List Version
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=GEMServerListVersion, r_sSettingValue:=sTemp, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sTemp <> "" Then
                m_bDataModelLevel = True

                r_sServerListVersion = sTemp.Trim()

                eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLServer

                ' Server List Pref Version
                m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=GEMServerListPrefVersion, r_sSettingValue:=sTemp, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                r_sServerListPrefVersion = sTemp.Trim()

                ' Server List File Compressed
                m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=GEMServerListFileCompressed, r_sSettingValue:=sTemp, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                r_bServerListFileCompressed = sTemp.Trim() = "Y"

                ' Server List File Path
                m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=GEMServerListFilePath, r_sSettingValue:=sTemp, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                r_sServerListFilePath = sTemp.Trim()
            Else
                ' Server List Version
                m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=PMEProductFamily.pmePFGemini, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=GEMServerListVersion, r_sSettingValue:=sTemp, v_sSubKey:=GEMRegKeyListManagement), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                r_sServerListVersion = sTemp.Trim()

                eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLServer

                ' Server List Pref Version
                m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=PMEProductFamily.pmePFGemini, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=GEMServerListPrefVersion, r_sSettingValue:=sTemp, v_sSubKey:=GEMRegKeyListManagement), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                r_sServerListPrefVersion = sTemp.Trim()

                ' Server List File Compressed
                m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=PMEProductFamily.pmePFGemini, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=GEMServerListFileCompressed, r_sSettingValue:=sTemp, v_sSubKey:=GEMRegKeyListManagement), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                r_bServerListFileCompressed = sTemp.Trim() = "Y"

                ' Server List File Path
                m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=PMEProductFamily.pmePFGemini, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=GEMServerListFilePath, r_sSettingValue:=sTemp, v_sSubKey:=GEMRegKeyListManagement), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                r_sServerListFilePath = sTemp.Trim()
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetServerSettings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetServerSettings", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetServerSettings
    '
    ' Description: Set them again
    '
    '
    ' ***************************************************************** '
    Public Function SetServerSettings(ByRef r_sServerListFilePath As String, ByRef r_sServerListVersion As String, ByRef r_sServerListPrefVersion As String, ByRef r_bServerListFileCompressed As Boolean, ByVal v_sGISDataModelCode As String, Optional ByVal v_sSellerCode As String = "") As Integer
        Dim result As Integer = 0
        Dim sSubKey As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
            Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
            Dim eProductFamily As gPMConstants.PMEProductFamily
            Dim sTemp As String = ""

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLServer

            ' Look in Data Model Specific Sub Key First
            If v_sGISDataModelCode <> "" Then
                sSubKey = GISSharedConstants.GISRegSubKey & "\" & v_sGISDataModelCode.Trim() & "\" & GEMRegKeyListManagement
            Else
                sSubKey = GEMRegKeyListManagement
            End If

            ' Server List Version
            m_lReturn = CType(gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=GEMServerListVersion, v_sSettingValue:=r_sServerListVersion, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLServer

            ' Server List Pref Version
            m_lReturn = CType(gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=GEMServerListPrefVersion, v_sSettingValue:=r_sServerListPrefVersion, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Server List File Compressed
            If r_bServerListFileCompressed Then
                sTemp = "Y"
            Else
                sTemp = "N"
            End If

            m_lReturn = CType(gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=GEMServerListFileCompressed, v_sSettingValue:=sTemp, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Server List File Path
            m_lReturn = CType(gPMFunctions.SetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=GEMServerListFilePath, v_sSettingValue:=r_sServerListFilePath, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetServerSettings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetServerSettings", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.
            If Not Informations.IsNothing(vTask) Then
                m_iTask = CInt(vTask)
            End If

            If Not Informations.IsNothing(vNavigate) Then
                m_lNavigate = CInt(vNavigate)
            End If

            If Not Informations.IsNothing(vProcessMode) Then
                m_lProcessMode = CInt(vProcessMode)
            End If

            If Not Informations.IsNothing(vTransactionType) Then
                m_sTransactionType = CStr(vTransactionType)
            End If

            If Not Informations.IsNothing(vEffectiveDate) Then
                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result
        Catch excep As System.Exception
            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
