Option Strict Off
Option Explicit On
Module MainModule
    ' ***************************************************************** '
    ' DO NOT USE GLOBAL OR PUBLIC VARIABLES IN BAS MODULES,
    ' THEY WILL GET CORRUPTED WHEN RUNNING UNDER COM+, FOR MORE INFO SEE
    ' http://support.microsoft.com/default.aspx?scid=kb;en-us;815053
    ' ***************************************************************** '
    '

    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 18th June 1998
    '
    ' Description: Main Module.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main global constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "bPMPropertyManager"

    ' Username and Password
    ' Country ID
    '***********************************************
    ' TO DO: GLOBAL VARIABLE IN BAS MODULE
    'Public g_iCountryID As Integer
    '***********************************************
    ' Log Level
    ' Currency
    ' Calling App Name
    ' Party Count for this User
    '***********************************************
    ' TO DO: GLOBAL VARIABLE IN BAS MODULE
    'Public g_lPartyCnt As Long
    '***********************************************

    ' Groups of Properties
    ' This component will NOT be running under COM+ and is therefore a special case.
    ' It needs this as a global variable and the project to have a thread pool of 1
    ' so that every instance of the Business class can see the same value here.
    ' If the thread pool is increased from 1, then each thread will have its own value for this
    ' variable and this will then cease to work.
    ' Remember global variables are stored on Thread Local Storage.
    Public g_oGroups As bPMPropertyManager.Groups
    '***********************************************

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"


    Public Sub Main()

    End Sub

    Sub New()
        Main()
    End Sub
    Sub JustForInvokeMain()
    End Sub
End Module