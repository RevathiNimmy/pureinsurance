Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
'Modified by Sudhanshu Behera on 5/19/2010 3:21:44 PM refer developer guide no. 129 (guide)
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Fish_NET.Fish")> _
Public NotInheritable Class Fish

    Implements IDisposable
    Private Const ACClass As String = "Fish"

    Private m_lReturn As Integer

    ' Path to fish.exe in the registry
    Private Const ACFishPath As String = "SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths\Fish.EXE"

    'TF011102 - Policy Ref no longer inc.
    'Private Const ACFishParameters As String = _
    '" /F ""Client|{clientid}|{policyid}"""
    Private Const ACFishParameters As String = " /F ""Client|{clientid}"""
    Private m_sFishPath As String = ""

    ' PolicyID
    Private m_vPolicyID As Object
    ' ClientID
    Private m_vClientID As String = ""

    Public Property PolicyID() As Object
        Get
            Return m_vPolicyID
        End Get
        Set(ByVal Value As Object)


            m_vPolicyID = Value
        End Set
    End Property

    Public Property ClientID() As String
        Get
            Return m_vClientID
        End Get
        Set(ByVal Value As String)

            m_vClientID = CStr(Value)
        End Set
    End Property

    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description:
    '
    ' History: 23/04/2002 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function Initialise() As Integer


        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Start
    '
    ' Description:
    '
    ' History: 23/04/2002 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Dim sCommandLine, sParameters As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Read the registry
            m_sFishPath = ADVReg.ReadRegistry(Group:=gPMConstants.HKEY_LOCAL_MACHINE, Section:=ACFishPath, Key:="")
            If m_sFishPath = "Not Found" Then
                ' Just default to something
                m_sFishPath = "C:\Program Files\Fish\Fish.exe"
            End If

            ' Work out the parameters
            sParameters = ACFishParameters

            ' Warning: VB6 only code...
            If m_vClientID <> "" Then
                sParameters = sParameters.Replace("{clientid}", m_vClientID)
            Else
                sParameters = sParameters.Replace("{clientid}", " ")
            End If

            'TF011102 - No longer req.
            '    If (m_vPolicyID <> "") Then
            '        sParameters = Replace(sParameters, "{policyid}", CStr(m_vPolicyID))
            '    Else
            '        sParameters = Replace(sParameters, "{policyid}", " ")
            '    End If

            ' Construct the path
            sCommandLine = m_sFishPath & " " & sParameters

            ' Launch and dont wait for it to finish

            Dim startInfo As ProcessStartInfo = New ProcessStartInfo(sCommandLine)
            startInfo.WindowStyle = ProcessWindowStyle.Normal
            Process.Start(startInfo)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description:
    '
    ' History: 23/04/2002 CTAF - Created.
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

End Class
