Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Activex
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports System.Windows.Forms.Control
Imports SharedFiles
Imports System.Collections.Generic
Imports System.Text
Imports System.Threading
Imports System.Linq


<System.Runtime.InteropServices.ProgId("ObjectManager_NET.ObjectManager")> _
Public NotInheritable Class ObjectManager
    Inherits Artinsoft.VB6.Activex.ComponentClassHelper

    Implements IDisposable
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer

    Private Const ACClass As String = "ObjectManager"

    Private m_vObjectParam As Object
    Private m_iCountryID As Integer
    Private m_lPartyCnt As Integer
    Private m_bStatelessClientManager As Boolean
    Private m_sServerPrinter As String = ""
    Private m_iIsPrinterChangeable As Integer
    Private m_oPMMessage As iPMMessage.PMMessage
    Private m_oLogonManager As iLogonManager.LogonManager
    Private m_oClientManager As Object
    Private m_sUserConfigXMLDataset As String = ""
    Private m_sPurePath As String = ""
    'Private Declare Function WTSGetActiveConsoleSessionId Lib "Kernel32.dll" Alias "WTSGetActiveConsoleSessionId" () As Int32

    ' PUBLIC Property Procedures (Begin)
    Const portnumberconstant = 65535

    Public ReadOnly Property UserName() As String
        Get
            Return m_sUsername
        End Get
    End Property

    Public ReadOnly Property Password() As String
        Get
            Return m_sPassword
        End Get
    End Property

    Public ReadOnly Property UserID() As Integer
        Get
            Return m_iUserID
        End Get
    End Property

    Public ReadOnly Property LanguageID() As Integer
        Get
            Return m_iLanguageID
        End Get
    End Property

    Public ReadOnly Property SourceID() As Integer
        Get
            Return m_iSourceID
        End Get
    End Property

    Public ReadOnly Property CountryID() As Integer
        Get
            Return m_iCountryID
        End Get
    End Property

    Public ReadOnly Property CurrencyID() As Integer
        Get
            Return m_iCurrencyID
        End Get
    End Property

    Public ReadOnly Property LogLevel() As Integer
        Get
            Return m_iLogLevel
        End Get
    End Property

    Public ReadOnly Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
    End Property

    Public ReadOnly Property GenericConnectionStatus() As Boolean
        Get
            ' Do we have Logon Manager
            If Not (m_oLogonManager Is Nothing) Then
                ' Yes, So return the status from Logon Manager
                Return m_oLogonManager.LoggedOnToPMB
            Else
                ' No Logon Manager, so we cannot possibly be logged on to Broking
                Return False
            End If
        End Get
    End Property

    Public ReadOnly Property LoggedOnLocally() As Boolean
        Get
            ' Do we have Logon Manager
            If Not (m_oLogonManager Is Nothing) Then
                ' Yes, So return the status from Client Manager
                Return m_oLogonManager.LoggedOnLocally
            Else
                ' No Client Manager, so we cannot possibly be logged on to Broking
                Return False
            End If
        End Get
    End Property

    Public Property UserConfigXMLDataSet() As String
        Get
            Return m_sUserConfigXMLDataset
        End Get
        Set(ByVal Value As String)
            m_sUserConfigXMLDataset = Value
        End Set
    End Property

    Public Function Initialise(ByRef sCallingAppName As String) As Integer
        Dim result As Integer = 0
        Dim lErrorValue As gPMConstants.PMEReturnCode
        Dim sDummyName As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Calling Application Name
            m_sCallingAppName = sCallingAppName

            ' Check if we have an instance of the
            ' client manager.
            If m_oClientManager Is Nothing Then
                ' Get instance of the client manager.
                lErrorValue = CType(GetClientManager(m_oClientManager), gPMConstants.PMEReturnCode)
                result = lErrorValue
                ' Check for errors.
                If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue AndAlso lErrorValue <> gPMConstants.PMEReturnCode.PMMAlreadyInUse Then
                    Dim iInstances As Integer = 0
                    Dim iSessionID As Integer = Process.GetCurrentProcess.SessionId

                    'Dim myprocess() As Process = Process.GetProcessesByName("iLogonStatusManager")

                    'If (myprocess.Length >= 1) Then
                    '    For index As Integer = 0 To myprocess.Length - 1
                    '        If myprocess(index).SessionId = iSessionID Then
                    '            iInstances += 1
                    '            Exit For
                    '        End If
                    '    Next
                    'End If
                    'If iInstances < 1 Then
                    '    Dim myprocess2() As Process = Process.GetProcessesByName("Ilogonserver")
                    '    If (myprocess2.Length >= 1) Then
                    '        For index As Integer = 0 To myprocess2.Length - 1
                    '            If myprocess2(index).SessionId = iSessionID Then
                    '                Process.GetProcessesByName("Ilogonserver")(index).Kill()
                    '            End If
                    '        Next
                    '    End If
                    'End If
                    Dim myprocess() As Process = Process.GetProcessesByName("iLogonStatusManager")
                    iInstances = myprocess.Where(Function(x) x.SessionId = iSessionID).Count()

                    If iInstances < 1 Then
                        Dim myprocessSrv() As Process = Process.GetProcessesByName("Ilogonserver")
                        myprocessSrv.Where(Function(x) x.SessionId = iSessionID).SingleOrDefault().Kill()
                        myprocessSrv = Nothing

                    End If
                    myprocess = Nothing

                End If
                If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue And lErrorValue <> gPMConstants.PMEReturnCode.PMCancel And lErrorValue <> gPMConstants.PMEReturnCode.PMMAlreadyInUse Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Display error to screen.
                    gPMFunctions.LogMessageToFile(sUsername:="ObjectManager", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get Client Manager", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                    Return result
                End If

                'if return from GetClientManager was pmcancel
                If lErrorValue = gPMConstants.PMEReturnCode.PMCancel Then
                    'call terminate method
                    Dispose()

                    Return gPMConstants.PMEReturnCode.PMCancel

                End If

            End If
            If lErrorValue <> gPMConstants.PMEReturnCode.PMMAlreadyInUse Then
                If m_bStatelessClientManager Then
                    lErrorValue = m_oLogonManager.GetPropertyValues(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iCountryID:=m_iCountryID, iLanguageId:=m_iLanguageID, iLogLevel:=m_iLogLevel, iCurrencyID:=m_iCurrencyID, lPartyCnt:=m_lPartyCnt, sCallingAppName:="", sServerPrinter:=m_sServerPrinter, iIsPrinterChangeable:=m_iIsPrinterChangeable)
                    If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Error getting Users state data from LogonManager." & Strings.Chr(10).ToString() & _
                                        "Contact you system Administrator to resolve the problem." & Strings.Chr(10).ToString(), "E0023 - Unable to Get State From LogonManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return lErrorValue
                    End If
                Else
                    ' Statefull ClientManager - Get the Values from there
                    lErrorValue = m_oClientManager.GetPropertyValues(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iCountryID:=m_iCountryID, iLanguageId:=m_iLanguageID, iLogLevel:=m_iLogLevel, iCurrencyID:=m_iCurrencyID, lPartyCnt:=m_lPartyCnt, sCallingAppName:=sDummyName, sUserConfigXMLDataSet:=m_sUserConfigXMLDataset)
                    If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Error getting Users state data from ClientManager." & Strings.Chr(10).ToString() & _
                                            "Contact you system Administrator to resolve the problem." & Strings.Chr(10).ToString(), "E0024 - Unable to Get State From ClientManager", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Return lErrorValue
                    End If
                End If
            End If
            If m_bStatelessClientManager Then
                m_oClientManager = Nothing
            End If

            Return result
        Catch excep As System.Net.Sockets.SocketException
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Connection Lost", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", excep:=excep)
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise object", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)
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
            Me.disposedValue = True
            If disposing Then
                If m_oPMMessage IsNot Nothing Then
                    m_oPMMessage.Dispose()
                    m_oPMMessage = Nothing
                End If
                If Not (m_oClientManager Is Nothing) Then
                    m_oClientManager = Nothing
                End If
                If Not (m_oLogonManager Is Nothing) Then
                    m_oLogonManager.AppReferenceCount -= 1
                End If
                Dim iInstances As Integer = 0
                Dim iSessionID As Integer = Process.GetCurrentProcess.SessionId
                Dim myprocess() As Process = Process.GetProcessesByName("iLogonStatusManager")
                iInstances = myprocess.Where(Function(x) x.SessionId = iSessionID).Count()

                If iInstances < 1 Then
                    Dim myprocessPMWrkManager() As Process = Process.GetProcessesByName("PMWorkManager")
                    If myprocessPMWrkManager.Length > 0 Then
                        myprocessPMWrkManager.Where(Function(x) x.SessionId = iSessionID).SingleOrDefault().Kill()
                    End If
                    myprocessPMWrkManager = Nothing
                    Dim myprocessSrv() As Process = Process.GetProcessesByName("Ilogonserver")
                    If myprocessSrv.Length > 0 Then
                        myprocessSrv.Where(Function(x) x.SessionId = iSessionID).SingleOrDefault().Kill()
                    End If
                    myprocessSrv = Nothing
                    
                End If
                myprocess = Nothing
                If Not (m_oLogonManager Is Nothing) Then
                    m_oLogonManager = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    Public Function InitialiseWithUserState(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iCountryID As Integer, ByRef iLanguageId As Integer, ByRef iLogLevel As Integer, ByRef iCurrencyID As Integer, ByRef lPartyCnt As Integer, ByRef sCallingAppName As String, Optional ByRef sServerPrinter As String = "", Optional ByRef iIsPrinterChangeable As Integer = 0) As Integer
        Dim result As Integer = 0

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword

            ' Set the User ID
            m_iUserID = iUserID

            ' Set the other User specific Values
            m_iSourceID = iSourceID
            m_iCountryID = iCountryID
            m_iLanguageID = iLanguageId
            m_iLogLevel = iLogLevel
            m_iCurrencyID = iCurrencyID
            m_lPartyCnt = lPartyCnt
            m_sCallingAppName = sCallingAppName
            m_sServerPrinter = sServerPrinter
            m_iIsPrinterChangeable = iIsPrinterChangeable

            ' This method can ONLY be used for with the Stateless ClientManager
            m_bStatelessClientManager = True
            m_oClientManager = Nothing

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Return the SystemValues", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialiseWithUserState", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetInstance
    '
    ' Description: Gets an instance of the class name passed.
    '
    ' ***************************************************************** '
    Public Function GetInstance(ByRef oObject As Object, ByRef sClassName As String, Optional ByRef vInstanceManager As Object = "") As Integer
        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim lErrorValue As Integer


        Try
            If Information.IsNothing(vInstanceManager) Then
                oObject = CreateLateBoundObject(sClassName)
            Else
                ' Where do we want to get the Object from
                Select Case vInstanceManager.ToUpper
                    ' Local Interface Object
                    Case gPMConstants.PMGetLocalInterface
                        ' Create Localaly
                        oObject = CreateLateBoundObject(sClassName)
                        ' Initialise
                        lErrorValue = oObject.Initialise()
                        ' Check for errors.
                        If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' Log Error.
                            LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Initialise LOCAL Interface Object : " & sClassName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetInstance")
                        End If
                        ' Local Business
                    Case gPMConstants.PMGetLocalBusiness
                        ' Create Locally
                        oObject = CreateLateBoundObject(sClassName)
                        ' Initialise
                        lErrorValue = oObject.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageId:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)
                        ' Check for errors.
                        If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' Log Error.
                            LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Initialise LOCAL Business Object : " & sClassName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetInstance")
                        End If
                        ' Remote Business Via Client Manager
                    Case gPMConstants.PMGetViaClientManager
                        ' RFC010202 - Are we using the Stateless ClientManager
                        If m_bStatelessClientManager Then
                            ' Yes, so Recreate ClientManager
                            'lErrorValue = RecreateClientManager(m_oClientManager)
                        End If
                        ' Get Via Client Manager
                        lErrorValue = m_oClientManager.GetInstance(oObject, sClassName, m_sCallingAppName)

                        ' Check for errors.
                        If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' Log Error.
                            LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get " & sClassName & "from client manager", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInstance")
                        End If

                        ' RFC010202 - Are we using the Stateless ClientManager
                        If m_bStatelessClientManager Then
                            ' RFC010202 - Release LogonManager reference to ClientManager
                            '             so that we are not holding resources on the Server.
                            m_oClientManager = Nothing
                        End If

                        ' Anything else
                    Case Else

                        ' RFC010202 - Are we using the Stateless ClientManager
                        If m_bStatelessClientManager Then
                            ' Yes, so Recreate ClientManager
                            'lErrorValue = RecreateClientManager(m_oClientManager)
                        End If

                        ' Get Via ClientManager (To maintain backward compatibility)
                        lErrorValue = m_oClientManager.GetInstance(oObject, sClassName, m_sCallingAppName)

                        ' Check for errors.
                        If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' Log Error.
                            LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get object instance from client manager : " & sClassName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetInstance")
                        End If

                        ' RFC010202 - Are we using the Stateless ClientManager
                        If m_bStatelessClientManager Then
                            ' RFC010202 - Release LogonManager reference to ClientManager
                            '             so that we are not holding resources on the Server.
                            m_oClientManager = Nothing
                        End If
                End Select
            End If

            Return result
        Catch excep As System.Net.Sockets.SocketException
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Connection Lost", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", excep:=excep)
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get object instance : " & sClassName, vApp:=ACApp, vClass:=ACClass, vMethod:="GetInstance", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try


    End Function

    ' ***************************************************************** '
    ' Name: AddMessage
    '
    ' Description: Wrapper method to the LogMessagePopup function.
    '
    ' ***************************************************************** '
    Public Sub AddMessage(ByRef iType As Integer, ByRef sMsg As String, Optional ByRef vApp As Object = Nothing, Optional ByRef vClass As Object = Nothing, Optional ByRef vMethod As Object = Nothing, Optional ByRef vErrNo As Object = Nothing, Optional ByRef vErrDesc As Object = Nothing)
        Dim lReturn As gPMConstants.PMEReturnCode

        Try
            m_oPMMessage = New iPMMessage.PMMessage()
            lReturn = m_oPMMessage.Initialise(m_sUsername, m_sPassword, m_oClientManager)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Set the PMMessage object to nothing
                m_oPMMessage = Nothing
            End If
            If m_oPMMessage Is Nothing Then
                gPMFunctions.LogMessagePopup(iType:=iType, sMsg:=sMsg, vApp:=CStr(vApp), vClass:=CStr(vClass), vMethod:=CStr(vMethod), excep:=New Exception(vErrDesc))
            Else
                LogMessage(m_sUsername, iType:=iType, sMsg:=sMsg, vApp:=vApp, vClass:=vClass, vMethod:=vMethod, vErrNo:=vErrNo, vErrDesc:=vErrDesc)
            End If

            m_oPMMessage = Nothing
        Catch excep As System.Exception
            gPMFunctions.LogMessagePopup(iType:=iType, sMsg:=sMsg, vApp:=CStr(vApp), vClass:=CStr(vClass), vMethod:=CStr(vMethod), excep:=excep)
            m_oPMMessage = Nothing
            Exit Sub
        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: CheckClientInstall
    '
    ' Description: Checks to see if the Installed Client Version of the
    '              PMProduct needs updating.
    ' ***************************************************************** '
    Public Function CheckClientInstall(ByVal v_lPMEProductFamily As Integer) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oLogonManager Is Nothing Then
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot check Client Version. Initialise Method NOT called.", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckClientInstall")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return m_oLogonManager.CheckClientInstall(v_lPMEProductFamily)
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckClientInstallFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckClientInstall", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

    Private Function GetClientManager(ByRef oClientManager As Object) As Integer
        Dim result As Integer = 0
        Dim lErrorValue As gPMConstants.PMEReturnCode

         Const portnumberconstant = 65535
        result = gPMConstants.PMEReturnCode.PMTrue
        Try
            ' Create a public instance of the logon manager.
            m_oLogonManager = CType(Activator.GetObject(GetType(iLogonManager.LogonManager), "tcp://localhost:" & (portnumberconstant - Process.GetCurrentProcess.SessionId) & "/SSP"), iLogonManager.LogonManager)

            If m_oLogonManager Is Nothing Then
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Logon manager", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientManager", excep:=New Exception(Information.Err().Description))
            End If
            If m_oLogonManager.LogonInProgress Then
                ' prevents other apps starting until branch selection
                ' dialogue has completed
                lErrorValue = gPMConstants.PMEReturnCode.PMMAlreadyInUse
                result = lErrorValue
            Else
                ' Call the initialise method and gain an instance of
                ' the client manager.
                If m_sCallingAppName = "iFieldManager" Then
                    oClientManager = New bClientManager.ClientManager
                    lErrorValue = CType(oClientManager.Initialise(iSourceID:=CShort(m_oLogonManager.SourceID), iCountryID:=CShort(m_oLogonManager.CountryID), iLanguageId:=CShort(m_oLogonManager.LanguageID), iLogLevel:=CShort(m_oLogonManager.LogLevel), iCurrencyID:=CShort(m_oLogonManager.CurrencyID), sCallingAppName:=m_sCallingAppName), gPMConstants.PMEReturnCode)
                Else
                    lErrorValue = m_oLogonManager.Initialise(oClientManager)
                End If
                'oClientManager = m_oLogonManager.m_obclientManager
                If Not oClientManager Is Nothing AndAlso Not m_oLogonManager Is Nothing Then
                    'm_iSourceID = m_oLogonManager.SourceID
                    'm_iCountryID = m_oLogonManager.CountryID
                    'm_iLanguageID = m_oLogonManager.LanguageID
                    'm_iLogLevel = m_oLogonManager.LogLevel
                    'm_iCurrencyID = m_oLogonManager.CurrencyID
                    'lErrorValue = RecreateClientManager(oClientManager)
                    oClientManager.UserName = m_oLogonManager.UserName
                    oClientManager.Userid = m_oLogonManager.Userid
                    oClientManager.CurrencyID = m_oLogonManager.CurrencyID
                    oClientManager.LogLevel = m_oLogonManager.LogLevel
                    oClientManager.CountryID = m_oLogonManager.CountryID
                    oClientManager.SourceID = m_oLogonManager.SourceID
                    'MessageBox.Show("object" & oClientManager.UserName)
                    m_oLogonManager.AppReferenceCount += 1
                End If
            End If
            If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue AndAlso lErrorValue <> gPMConstants.PMEReturnCode.PMCancel AndAlso lErrorValue <> gPMConstants.PMEReturnCode.PMMAlreadyInUse Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get instance of the client manager", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientManager")

                m_oLogonManager = Nothing
                Return result
            End If

            'check to see if LogonManager.Initialise returned pmcancel 
            If lErrorValue = gPMConstants.PMEReturnCode.PMCancel Then
                result = gPMConstants.PMEReturnCode.PMCancel
                m_oLogonManager.Dispose()
                m_oLogonManager = Nothing
                Return result
            End If

            Try
                m_bStatelessClientManager = m_oLogonManager.StatelessClientManager

            Catch
            End Try



            Return result
        Catch excep As System.Net.Sockets.SocketException
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Connection Lost", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", excep:=excep)
        End Try
Err_GetClientManager:

        ' Error Section.
        result = gPMConstants.PMEReturnCode.PMError

        ' Set return object to nothing
        oClientManager = Nothing

        ' Log Error.
        gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get client manager", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientManager", excep:=New Exception(Information.Err().Description))

        Return result


    End Function

    ' ***************************************************************** '
    ' Name: LogMessage
    '
    ' Description: Wrapper function to the log message method of the
    '              PMMessage object.
    '
    ' ***************************************************************** '
    Private Sub LogMessage(ByRef sUsername As String, ByRef iType As Integer, ByRef sMsg As String, Optional ByRef vApp As Object = Nothing, Optional ByRef vClass As Object = Nothing, Optional ByRef vMethod As Object = Nothing, Optional ByRef vErrNo As Object = Nothing, Optional ByRef vErrDesc As Object = Nothing)
        Try
            gPMFunctions.LogMessageToFile(sUsername:=sUsername, iType:=iType, sMsg:=sMsg, vApp:=CStr(vApp), vClass:=CStr(vClass), vMethod:=CStr(vMethod), excep:=New Exception(CStr(vErrDesc)))
        Catch ex As Exception
            gPMFunctions.LogMessagePopup(iType:=iType, sMsg:=sMsg, vApp:=CStr(vApp), vClass:=CStr(vClass), vMethod:=CStr(vMethod), excep:=New Exception(CStr(vErrDesc)))
        End Try
    End Sub

    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.
        Dim myprocess() As Process = Process.GetProcessesByName("Ilogonserver")

        If m_sPurePath = "" Then
            gPMFunctions.GetPMRegSetting(gPMConstants.HKEY_LOCAL_MACHINE, 0, gPMConstants.PMERegSettingLevel.pmeRSLBase, "PMDIR", m_sPurePath)
            m_sPurePath &= "\Pure\Application\"
        End If


        'If (Process.GetProcessesByName("Ilogonserver").Length < 1) Then
        If (myprocess.Length < 1) Then
            Process.Start(m_sPurePath & "Ilogonserver.exe")
            Thread.Sleep(1000)
        Else
            Dim iInstances As Integer = 0
            Dim iSessionID As Integer = Process.GetCurrentProcess.SessionId

            For index As Integer = 0 To myprocess.Length - 1
                If myprocess(index).SessionId = iSessionID Then
                    iInstances += 1
                    Exit For
                End If
            Next

            If iInstances < 1 Then
                Process.Start(m_sPurePath & "Ilogonserver.exe")
                Thread.Sleep(1000)
            End If
        End If
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    Public Function SetUserConfigXML(ByVal sUserConfigXMLDataSet As String) As Integer
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue
        Try
            If Not (m_oClientManager Is Nothing) Then
                m_oClientManager.UserConfigXMLDataSet = sUserConfigXMLDataSet
            End If
            Return result
        Catch excep As System.Exception
            LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Set User Config XML Values", vApp:=ACApp, vClass:=ACClass, vMethod:="SetUserConfigXML", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function
End Class
