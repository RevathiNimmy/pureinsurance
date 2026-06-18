Option Strict Off
Option Explicit On
Imports System
'developer guide no.129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
 Public Module MainModule

    Public Const ACApp As String = "iPMUPolicy"
    Private Const ACClass As String = "MainModule"

    ' KB 010801 Correct ScreenContextId
    Public Const ScreenHelpID As Integer = 4004
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

    ' CTAF 300800
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_oObjectManager As bObjectManager.ObjectManager
End Module