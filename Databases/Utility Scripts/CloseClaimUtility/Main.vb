Option Strict Off
Option Explicit On
Imports VB = Microsoft.VisualBasic
Imports SharedFiles
Imports Microsoft.VisualBasic.Compatibility.VB6

Module MainModule

    Public Const ACApp As String = "CloseClaims"
    Public Const ACClass As String = "CloseClaims"
    Public Const kSelectAllItem As String = "<--ALL-->"
    ' constants for user name and password - apparently, these should be available on all systems...
    Public Const m_sUserName As String = "sirius"
    Public Const m_sPassword As String = ""

    Public g_sUserName As String


    <ThreadStatic()> _
    Public g_bBalanceAndCloseClaim As Boolean

    Public Sub Main()
        On Error GoTo Err_Main

        Dim ofrmMain As frmInterface

        ofrmMain = New frmInterface

        ofrmMain.ShowDialog()
        'Call ofrmMain.RegenerateAllModels()

        ofrmMain.Hide()

        'UPGRADE_NOTE: Object ofrmMain may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v90/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
        ofrmMain = Nothing

        End

        Exit Sub

Err_Main:

        ' Log Error.
        SharedFiles.bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to run datamodel rebuild process", vApp:=ACApp, vClass:=ACClass, vMethod:="Main", vErrNo:=Err.Number, vErrDesc:=Err.Description)

        Exit Sub

    End Sub
End Module