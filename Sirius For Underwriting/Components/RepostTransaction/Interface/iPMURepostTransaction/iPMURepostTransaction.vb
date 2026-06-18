Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'Modified by Sumeet Singh on 5/25/2010 11:03:38 AM refer developer guide no. 129
Imports SharedFiles
Public Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 06/09/2000
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '


    Public Const ACApp As String = "iPMURepostTransaction"

    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    Private Const ACClass As String = "MainModule"
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iLanguageID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iUserID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_sUserName As String = ""

    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_oObjectManager As bObjectManager.ObjectManager

    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions


    Public Sub main()


        Dim oInterface As New Interface_Renamed
        'developer guide no. 10
        Dim lReturn As gPMConstants.PMEReturnCode = CType(oInterface.Initialise(), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise iPMURepostTransaction", vApp:=ACApp, vClass:=ACClass, vMethod:="sub main", excep:=New Exception(Information.Err().Description))

            Environment.Exit(0)
        End If

        lReturn = CType(oInterface.Start(), gPMConstants.PMEReturnCode)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start iPMURepostTransaction", vApp:=ACApp, vClass:=ACClass, vMethod:="sub main", excep:=New Exception(Information.Err().Description))

            Environment.Exit(0)
        End If

        oInterface.Dispose()

        oInterface = Nothing

    End Sub
End Module