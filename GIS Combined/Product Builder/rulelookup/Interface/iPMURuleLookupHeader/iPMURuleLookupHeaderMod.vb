Option Strict Off
Option Explicit On
Imports System
'Developer Guide No. 129
Imports SharedFiles
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 12/07/2000
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iPMURuleLookupHeader"


    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons


    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101
    Public Const ACTabTitle2 As Integer = 102
    Public Const ACIName As Integer = 103
    Public Const ACIEffectiveDate As Integer = 104
    Public Const ACILive As Integer = 105
    Public Const ACIDefinition As Integer = 106
    Public Const ACIValidConstant As Integer = 107
    Public Const ACIDefault As Integer = 108

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
    Public Const ACStatusSearching As Integer = 306
    Public Const ACStatusFound As Integer = 307

    ' Menus


    ' {* USER DEFINED CODE (End) *}

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Constants for the search data array indexes.
    Public Const ACFInsurerPanelMemberKey As Integer = 0
    Public Const ACFSchemeNumber As Integer = 1
    Public Const ACFLookupKey As Integer = 2
    Public Const ACFLookupName As Integer = 3
    Public Const ACFEffectiveDate As Integer = 4
    Public Const ACFStatus As Integer = 6
    Public Const ACFDefinition As Integer = 7
    Public Const ACFValidConstant As Integer = 8
    Public Const ACFDefaultValue As Integer = 9

    ' Public source and language ID's from the
    ' Object Manager.
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

    ' KB 160801 required for help text, screenhelpid will need to be amended
    ' when text available
    Public Const ScreenHelpID As Integer = 4001

    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions



    Sub Main_Renamed()

    End Sub
End Module