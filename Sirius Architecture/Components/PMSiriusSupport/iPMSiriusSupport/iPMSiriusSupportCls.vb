Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("PMSiriusSupport_NET.PMSiriusSupport")> _
Public NotInheritable Class PMSiriusSupport

    Implements IDisposable
    'Implements SSP.S4I.Interfaces.ILocalInterface
    ' ***************************************************************** '
    ' Class Name: PMSiriusSupport
    '
    ' Date: 12/07/2000
    '
    ' Description: Launches PM Sirius Support Web Page
    '
    ' Edit History:
    ' ***************************************************************** '

    Private Const ACClass As String = "PMSiriusSupport"

    Private m_lReturn As Integer

    Private WithEvents m_oIE As WebBrowser
    ' DocumentComplete
    Private m_bDocumentComplete As Boolean
    ' PMSiriusSupportURL
    Private m_sPMSiriusSupportURL As String = ""

    Public Property DocumentComplete() As Boolean
        Get
            Return m_bDocumentComplete
        End Get
        Set(ByVal Value As Boolean)
            m_bDocumentComplete = Value
        End Set
    End Property

    Public Property PMSiriusSupportURL() As String
        Get
            Return m_sPMSiriusSupportURL.Trim()
        End Get
        Set(ByVal Value As String)
            m_sPMSiriusSupportURL = Value.Trim()
        End Set
    End Property

    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description: Standard function to initialise the object
    '
    ' History: 12/07/2000 DAK - Created.
    '
    ' ***************************************************************** '

    Public Function Initialise(Optional ByVal sUsername As String = "", Optional ByVal sPassword As String = "", Optional ByVal iUserID As Integer = 0, Optional ByVal iSourceID As Integer = 0, Optional ByVal iLanguageID As Integer = 0, Optional ByVal iCurrencyID As Integer = 0, Optional ByVal iLogLevel As Integer = 0, Optional ByVal sCallingAppName As String = "", Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long 'Implements SSP.S4I.Interfaces.ILocalInterface.Initialise

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not False Then
                g_sUsername = sUsername
            Else
                g_sUsername = "sirius"
            End If

            If Not False Then
                g_sPassword = sPassword
            Else
                g_sPassword = "sirius"
            End If

            If Not False Then
                g_iUserID = iUserID
            Else
                g_iUserID = 1
            End If

            If Not False Then
                g_iSourceID = iSourceID
            Else
                g_iSourceID = 1
            End If

            If Not False Then
                g_iLanguageID = iLanguageID
            Else
                g_iLanguageID = 1
            End If

            If Not False Then
                g_iCurrencyID = iCurrencyID
            Else
                iCurrencyID = 1
            End If

            If Not False Then
                g_iLogLevel = iLogLevel
            Else
                iLogLevel = 4
            End If

            If Not False Then
                g_sCallingAppName = sCallingAppName
            Else
                sCallingAppName = ""
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description: Standard function to terminate the object
    '
    ' History: 12/07/2000 DAK - Created.
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
    '
    ' Name: PMSiriusSupport
    '
    ' Description:
    '
    ' History: 11/07/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function PMSiriusSupport() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            DocumentComplete = False

            m_oIE = New WebBrowser()


            'm_oIE.Navigate(New Uri(PMSiriusSupportURL))
            'System.Diagnostics.Process.Start("iexplore", PMSiriusSupportURL)
            'Do While Not DocumentComplete
            '    Application.DoEvents()
            'Loop
            'added code 
            System.Diagnostics.Process.Start("iexplore", PMSiriusSupportURL)
            m_oIE.Visible = True

            m_oIE = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PMSiriusSupport Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PMSiriusSupport", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Class_Initialize
    '
    ' Description: Standard class initialize
    '
    ' History: 12/07/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try


    End Sub

    ' ***************************************************************** '
    '
    ' Name: Class_Terminate
    '
    ' Description:
    '
    ' History: 12/07/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    Private Sub m_oIE_DocumentComplete(ByVal eventSender As Object, ByVal eventArgs As Windows.Forms.WebBrowserDocumentCompletedEventArgs) Handles m_oIE.DocumentCompleted
        DocumentComplete = True
    End Sub
End Class
