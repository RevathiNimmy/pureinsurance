Option Strict Off
Option Explicit On
Imports System
'Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 09/06/1999
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions to identify which application this is.
    Public Const ACApp As String = "iPMUPerilTypeUsage"

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "MainModule"


    ' Public interface constants used when retrieving data from the resource file.

    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101
    Public Const ACTabTitle2 As Integer = 102

    Public Const ACCTaxGroup As Integer = 111
    Public Const ACHTaxband As Integer = 113
    Public Const ACHSequence As Integer = 114
    Public Const ACCTaxBand As Integer = 115
    Public Const ACCSequence As Integer = 116

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303

    ' Menus

    ' Constants for the search data array indexes.
    Public Const ACPTaxGroupID As Integer = 0
    Public Const ACPTaxBandID As Integer = 1
    Public Const ACPSequence As Integer = 2
    Public Const ACPDescription As Integer = 3
    Public Const ACPAllocSequence As Integer = 4
    Public Const ACPAllocRule As Integer = 5

    Public Const ACPMaxArray As Integer = 5

    ' Public source and language ID's from the Object Manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer

    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oGIS As Object

    ' KB 130801 required for help text, screenhelpid will need to be amended
    ' when text available
    Public Const ScreenHelpID As Integer = 4001
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions


    Sub Main_Renamed()

    End Sub
End Module