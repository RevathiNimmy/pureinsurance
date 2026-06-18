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
    Public Const ACApp As String = "iPMURIBandVersion"

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "MainModule"


    ' Public interface constants used when retrieving data from the resource file.

    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101
    Public Const ACTabTitle2 As Integer = 102

    Public Const ACCRIBand As Integer = 111
    Public Const ACHDateForTreaty As Integer = 112
    Public Const ACHXolTreaty As Integer = 113
    Public Const ACHProportionalRICal As Integer = 114
    Public Const ACHUseAnniversary As Integer = 115
    Public Const ACHEffectiveDate As Integer = 116

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
    Public Const ACPRIBandId As Integer = 0
    Public Const ACPCode As Integer = 1
    Public Const ACPDescription As Integer = 2
    Public Const ACPCaption As Integer = 3
    Public Const ACPEffectiveDate As Integer = 4
    Public Const ACPDateForTreaty As Integer = 5
    Public Const ACPXOLTreaty As Integer = 6
    Public Const ACPProportionalRICal As Integer = 7
    Public Const ACPUseAnniversaryDate As Integer = 8


    Public Const ACPDateForTreatyID As Integer = 9
    Public Const ACPXOLID As Integer = 10
    Public Const ACPPRICALID As Integer = 11

    Public Const ACPBandVersionMaxArray As Integer = 11

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

End Module