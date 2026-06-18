Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
Imports System.Xml

Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Interface_Renamed_NET.Interface_Renamed")> _
Public NotInheritable Class Interface_Renamed

    Implements IDisposable
    Private Const ACClass As String = "Interface"

    ' Return value
    Private m_lReturn As Integer

    Private m_sCallingAppName As String = ""
    Private m_lPMAuthorityLevel As Integer
    Private m_lStatus As Integer

    ' Process modes
    Private m_vTask As Object
    Private m_vNavigate As Object
    Private m_vProcessMode As Object
    Private m_vTransactionType As Object
    Private m_vEffectiveDate As Object

    ' Restart step
    Private m_lRestartStep As Integer

    ' Key array
    Private m_vKeyArray(,) As Object

    ' XMLFileName
    Private m_sXMLFileName As String = ""

    ' PUBLIC Events (Begin)
    Public Event SetProcessStatus(ByVal v_bProcessComplete As Boolean)
    Public Event NavigatorClose()

    Private WithEvents m_fNavForm As frmMain

    ' SET 27/01/2004
    Private m_lPMWrkTaskInstanceCnt As Integer

    Private bIsChildNavigatorON As Boolean = False
    Public Property IsChildNavigatorON() As Boolean
        Get
            Return bIsChildNavigatorON
        End Get
        Set(ByVal value As Boolean)
            bIsChildNavigatorON = value
        End Set
    End Property
    ' PUBLIC Events (End)

    Public Property XMLFileName() As String
        Get
            Return m_sXMLFileName
        End Get
        Set(ByVal Value As String)
            m_sXMLFileName = Value
        End Set
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property
    Friend Property arrayvalue() As Object
        Get
            Return m_vKeyArray
        End Get
        Set(ByVal value As Object)
            m_vKeyArray = value
        End Set
    End Property
    Public Property CallingAppName() As String
        Get
            Return m_sCallingAppName
        End Get
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property

    Public Property NavigatorV3_PMAuthorityLevel() As Integer
        Get
            Return m_lPMAuthorityLevel
        End Get
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    Public Property Status() As Integer
        Get
            Return m_fNavForm.Status
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property

    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description: Standard Initialise function.
    '
    ' History: 23/05/2000 CTAF - Created.
    '
    '          10/04/2003 Kevin Renshaw (CMG) Create New form when called by WorkManager
    ' ***************************************************************** '
    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_fNavForm = New frmMain()
            objfrmDebug = New frmDebug
            ' Pass myself through to the form
            m_fNavForm.ParentClass = Me

            Return result

        Catch excep As System.Exception
            'Debugger.Break()()
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description: Standard Terminate function.
    '
    ' History: 23/05/2000 CTAF - Created.
    '
    '          10/04/2003 Kevin Renshaw (CMG) Unload form when class terminated
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                If Not (m_fNavForm Is Nothing) Then
                    m_fNavForm.ParentClass = Nothing
                    m_fNavForm.Hide()
                    m_fNavForm = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    '
    ' Name: Start
    '
    ' Description: Standard program entry point
    '
    ' History: 23/05/2000 CTAF - Created.
    '
    '          10/04/2003 Kevin Renshaw (CMG) Forms being opened modally - Changed the
    '                     way the form is created
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_fNavForm.IsChildNavigatorON = bIsChildNavigatorON

            ' Pass in the name of the xml file
            m_fNavForm.XMLFileName = m_sXMLFileName

            ' Check for errors
            If m_fNavForm.Error_Renamed <> gPMConstants.PMEReturnCode.PMFalse Then

                ' Pass in any start key (a value of 0 is also good)
                m_fNavForm.RestartStep = m_lRestartStep

                ' SET 27/01/2004 - Pass the PMWrkTaskInstanceCnt
                m_fNavForm.TaskInstanceCnt = m_lPMWrkTaskInstanceCnt

                ' Pass any keys in
                If Information.IsArray(m_vKeyArray) Then
                    m_lReturn = m_fNavForm.SetKeys(vKeyArray:=m_vKeyArray)
                End If

                m_fNavForm.tmrStart.Enabled = True
                '#If APPDEBUG = 1 Then
#If DEBUG Then

                ' Show the debug form
                m_fNavForm.Show()

                Do While (m_fNavForm.Ended = False)
                    Application.DoEvents()
                Loop

                ' Remove debug
                'Unload frmDebug
                'frmDebug.Hide()
                objfrmDebug.Close()

#Else
                m_fNavForm.Show() 'vbModal
#End If

                ' Get the status
                Status = m_fNavForm.Status
            End If

            Return result

        Catch exDisposed As ObjectDisposedException

            result = gPMConstants.PMEReturnCode.PMTrue
            Return result

        Catch excep As System.Exception

            'Debugger.Break()()

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetKeys
    '
    ' Description:
    '
    ' History: 23/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim vTempArray(,) As Object
        Dim iIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Information.IsArray(vKeyArray) Then
                Return result
            End If

            vTempArray = Nothing

            ' Fish the restart_step key out
            For iLoop1 As Integer = 0 To vKeyArray.GetUpperBound(1)
                If CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop1)) = "restart_step" Then

                    m_lRestartStep = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                ElseIf (CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop1)) = PMNavKeyConst.PMKeyNameTaskInstanceCnt) Then
                    ' SET 27/01/2004 - save the PMWrkTaskInstanceCnt

                    m_lPMWrkTaskInstanceCnt = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1))
                Else
                    ' Stash the key
                    If Not Information.IsArray(vTempArray) Then
                        iIndex = 0
                        ReDim vTempArray(1, iIndex)
                    Else

                        iIndex = vTempArray.GetUpperBound(1) + 1
                        ReDim Preserve vTempArray(1, iIndex)
                    End If
                    vTempArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iIndex) = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iLoop1)
                    ' CTAF 300502 - Pass objects as keys (eg. GIS)
                    If Information.IsReference(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)) Then
                        vTempArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iIndex) = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)
                    Else
                        vTempArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iIndex) = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iLoop1)
                    End If
                End If

            Next iLoop1

            ' Assign the array
            If Information.IsArray(vTempArray) Then

                m_vKeyArray = vTempArray
            End If

            Return result

        Catch excep As System.Exception
            'Debugger.Break()()
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetKeys
    '
    ' Description:
    '
    ' History: 23/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Pass any keys in
            If Not (m_fNavForm Is Nothing) Then
                m_lReturn = m_fNavForm.GetKeys(vKeyArray:=m_vKeyArray)

                If Information.IsArray(m_vKeyArray) Then
                    vKeyArray = VB6.CopyArray(m_vKeyArray)
                End If
            End If

            Return result

        Catch excep As System.Exception
            'Debugger.Break()()
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetSummary
    '
    ' Description:
    '
    ' History: 23/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer

        Dim result As Integer = 0
        Try
            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception
            'Debugger.Break()()
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummary Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetProcessModes
    '
    ' Description:
    '
    ' History: 23/05/2000 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            If Not Information.IsNothing(vTask) Then
                m_vTask = vTask
            End If
            If Not Information.IsNothing(vNavigate) Then
                m_vNavigate = vNavigate
            End If
            If Not Information.IsNothing(vProcessMode) Then
                m_vProcessMode = vProcessMode
            End If
            If Not Information.IsNothing(vTransactionType) Then
                m_vTransactionType = vTransactionType
            End If
            If Not Information.IsNothing(vEffectiveDate) Then
                m_vEffectiveDate = vEffectiveDate
            End If

            Return result

        Catch excep As System.Exception
            'Debugger.Break()()
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("vEffectiveDate", vEffectiveDate)
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", excep:=excep, oDicParms:=oDict)

            Return result

        End Try
    End Function

    Public Sub New()
        MyBase.New()

        ' Class Initialise Event.

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface entry class", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    '
    ' Name: SetProcessStatus
    '
    ' Description: Called from the form, raises an event
    '
    ' History: 14/10/2002 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function SetProcessStatus_Renamed(ByVal v_bProcessComplete As Boolean) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            RaiseEvent SetProcessStatus(v_bProcessComplete)

            Return result

        Catch excep As System.Exception
            'Debugger.Break()()
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessStatus", excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: NavigatorClose
    '
    ' Description: Called from the form, raises an event
    '
    ' History: 11/10/2002 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function NavigatorClose_Renamed() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            RaiseEvent NavigatorClose()

            Return result

        Catch excep As System.Exception
            'Debugger.Break()()
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NavigatorClose Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NavigatorClose", excep:=excep)

            Return result

        End Try
    End Function
End Class
