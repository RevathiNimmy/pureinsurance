Option Strict Off
Option Explicit On
Imports System
'Developer Guide No. 129
Imports SharedFiles
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 17/02/1997
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "uctTextFilesControl"

    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons


    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101
    Public Const ACTabTitle2 As Integer = 102

    Public Const ACPolicy As Integer = 103
    Public Const ACClaim As Integer = 104

    Public Const ACListTitle1 As Integer = 105
    Public Const ACListTitle2 As Integer = 106
    Public Const ACListTitle3 As Integer = 107

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203
    Public Const ACNewButton As Integer = 204
    Public Const ACEditButton As Integer = 205
    Public Const ACFindNowButton As Integer = 206
    Public Const ACNewSearchButton As Integer = 207

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303

    Public Const ACClearDetailsTitle As Integer = 304
    Public Const ACClearDetails As Integer = 305
    Public Const ACStatusSearching As Integer = 306
    Public Const ACStatusFound As Integer = 307

    ' Menus


    ' Constants for the search data array indexes.
    Public Const ACISlotNumber As Integer = 0
    Public Const ACIDescription As Integer = 1
    Public Const ACIFileNumber As Integer = 2
    'CT 27/11/00 brought back extra data for source id
    Public Const ACISourceId As Integer = 3
    ' {* USER DEFINED CODE (End) *}

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the maxiumum search details.
    Public Const ACMaxSearchDetails As Integer = 500

    ' Constant for the miniumum search length.
    Public Const ACMinSearchLength As Integer = 1

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

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

    ' Public instance of the business object.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oBusiness As Object
    'Public g_oBusiness As bSIRTextFiles.Business

    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

    Public Const ScreenHelpID As Integer = 24

    Sub Main_Renamed()

    End Sub
End Module