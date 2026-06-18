Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'Modified by Archana Tokas on 5/11/2010 12:43:37 PM refer developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Form_NET.Form")> _
Public NotInheritable Class Form
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 23/04/1998
    '
    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a BankName.
    '
    ' Edit History:
    ' BB171297 Add BeginTrans CommitTrans to DirectAdd
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

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    ' Task
    Private m_iTask As Integer
    ' Navigate
    Private m_lNavigate As Integer
    ' Process Mode
    Private m_lProcessMode As Integer
    ' Type of Business
    Private m_sTransactionType As String = ""
    ' Effective
    Private m_dtEffectiveDate As Date

    Private m_iBusinessType As Integer

    ' Primary Keys to work with
    ' Insurance File ID
    Private m_lInsuranceFileID As Integer
    ' Risk ID
    Private m_lRiskID As Integer


    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)


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
    Public WriteOnly Property BusinessType() As Integer
        Set(ByVal Value As Integer)

            m_iBusinessType = Value

        End Set
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


    ' PUBLIC Property Procedures (End)


    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise




        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
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

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Set the ProcessMode to Generic
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric

            ' Set the Type Of Business to New Business
            'm_sTransactionType = PMTypeOfBusinessNB

            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now

            ' Default RiskID to 0 (So we will work at Ins File Level by default)
            m_lRiskID = 0


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            If disposing Then
            End If
        End If
        Me.disposedValue = True
    End Sub

    ' ***************************************************************** '
    ' Name: GetRegSettings
    '
    ' Description: Get a default document path from the server
    '
    '
    ' ***************************************************************** '
    Public Function GetRegSettings(ByRef sResult As String, ByRef sAppName As String, ByRef sSection As String, ByRef sKey As String, Optional ByRef vDefault As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    m_lReturn = PMFunc.GetRegSettings( _
            ''        sResult:=sResult, _
            ''        sAppName:=sAppName, _
            ''        sSection:=sSection, _
            ''        sKey:=sKey, _
            ''        vDefault:=vDefault)

            '(sDocumentPath, "Voyager", "Paths", "DocumentProduction", "C:\Program Files\PM\Voyager\Documents\")
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRegSettings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRegSettings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'sj 06/07/98 - start
    ' ***************************************************************** '
    ' Name: GetRegSettings
    '
    ' Description: Get a default document path from the server
    '
    '
    ' ***************************************************************** '
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
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetServerPMRegSetting Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetServerPMRegSetting", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetRegSettings
    '
    ' Description: Get a default document path from the server
    '
    '
    ' ***************************************************************** '
    Public Function SetServerSettings(ByRef r_sServerListFilePath As String, ByRef r_sServerListVersion As String, ByRef r_sServerListPrefVersion As String, ByRef r_bServerListFileCompressed As Boolean, ByRef r_sAppPath As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
            Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
            Dim eProductFamily As gPMConstants.PMEProductFamily
            Dim sTemp As String = ""

            Dim sSettingName As String = ""

            eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
            eProductFamily = gPMConstants.PMEProductFamily.pmePFGemini
            eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLServer

            ' Server List Version
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="ServerListVersion", r_sSettingValue:=sTemp, v_sSubKey:=GEMRegKeyListManagement), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or sTemp = "" Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_sServerListVersion = sTemp.Trim()
            r_sServerListPrefVersion = sTemp.Trim()


            sSettingName = MainModule.GEMServerListFileCompressed


            ' Server List File Compressed
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=sSettingName, r_sSettingValue:=sTemp, v_sSubKey:=GEMRegKeyListManagement), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or sTemp = "" Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_bServerListFileCompressed = sTemp.Trim() = "Y"

            sSettingName = MainModule.GEMServerListFilePath

            ' Server List File Path
            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=sSettingName, r_sSettingValue:=sTemp, v_sSubKey:=GEMRegKeyListManagement), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or sTemp = "" Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_sServerListFilePath = sTemp.Trim()

            sSettingName = MainModule.GEMServerPolarisFilePath

            m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=sSettingName, r_sSettingValue:=sTemp, v_sSubKey:=GEMRegKeyListManagement), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or sTemp = "" Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_sAppPath = sTemp.Trim()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetServerSettings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetServerSettings", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function




    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)


    ' PRIVATE Methods (End)


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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class
