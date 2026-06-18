Option Strict Off
Option Explicit On
Imports System
'Developer Guide No. 129
Imports SharedFiles
Module MainModule

    Public Const ACApp As String = "iPMURisk"
    Private Const ACClass As String = "MainModule"

    Public Const ScreenHelpID As Integer = 4

    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

    ' CTAF 300800
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager


    ' windows helper function
    Public Declare Function BringWindowToTop Lib "user32" (ByVal hwnd As Integer) As Integer

    'Start (Sriram P) Tech Spec - WR8 - Navigator DME Link.doc section(4.6.1)
    Public Const ACTransactionType As String = "C_CO"
    'End (Sriram P) Tech Spec - WR8 - Navigator DME Link.doc section(4.6.1)
End Module