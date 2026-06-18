Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
 Public Module MainModule

    Public Const ACApp As String = "iPMUListPolicy"
    Private Const ACClass As String = "MainModule"

    'ED 19072002
    'Developer Guide No. 107
    <ThreadStatic()> _
    Public g_oObjectManager As bObjectManager.ObjectManager
    'Developer Guide No. 107
    <ThreadStatic()> _
    Public g_iUserID As Integer '   RAM20030123 : Added this variable
    Public g_sUsername As New FixedLengthString(12)
    'Developer Guide No. 107
    <ThreadStatic()> _
    Public g_iSourceID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
    Public g_iLanguageID As Integer
    'ED 19072002 (End)

    Public Const ScreenHelpID As Integer = 4
    'UPGRADE_NOTE: (1053) g_sProductFamily was changed from a Constant to a Variable. More Information: http://www.vbtonet.com/ewis/ewi1053.aspx
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

    ''sj 02/01/2003 - start
    ''PS104
    'Public Const ACRunModeNewMtaCancel = 1
    'Public Const ACRunModeNewMtaReinstate = 2
    'Public Const ACRunModeMtaLinkCancel = 3
    'Public Const ACRunModeMtaLinkReinstate = 4
    'Public Const ACRunModeMtaLinkMTA = 5
    'Public Const ACRunModeNewMTAReinstateRisk = 6
    ''sj 02/01/2003 - end
End Module