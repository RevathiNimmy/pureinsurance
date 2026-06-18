Option Strict Off
Option Explicit On
Imports System
Imports SharedFiles

Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 19/10/1998
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iACTFindBudget"

    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' Constants for the search data array indexes.
    Public Const ACBudgetID As Integer = 0
    Public Const ACBudgetRef As Integer = 1
    Public Const ACPeriodID As Integer = 2
    Public Const ACBudgetDescription As Integer = 3
    Public Const ACPeriodYearName As Integer = 4
    Public Const ACRevisesBudgetID As Integer = 5
    Public Const ACBudgetStatusID As Integer = 6

    ' General Icons


    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101

    Public Const ACListTitle1 As Integer = 102
    Public Const ACListTitle2 As Integer = 103

    Public Const ACReferenceTitle As Integer = 104
    Public Const ACStatusTitle As Integer = 105
    Public Const ACYearTitle As Integer = 106

    ' List headers
    Public Const ACReferenceListTitle As Integer = 107
    Public Const ACYearListTitle As Integer = 108
    Public Const ACRevisesListTitle As Integer = 109
    Public Const ACStatusListTitle As Integer = 110
    Public Const ACDescriptionListTitle As Integer = 111

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

    Public Const ACClearDetailsTitle As Integer = 304
    Public Const ACClearDetails As Integer = 305
    Public Const ACStatusSearching As Integer = 306
    Public Const ACStatusFound As Integer = 307

    ' Menus


    ' Constants for the search data array indexes.


    ' {* USER DEFINED CODE (End) *}

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the maxiumum search details.
    Public Const ACMaxSearchDetails As Integer = 500

    ' Constant for the miniumum search length.
    Public Const ACMinSearchLength As Integer = 3

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Public source and language ID's from the
    ' Object Manager.
    Public g_iSourceID As Integer
    Public g_iLanguageID As Integer

    ' Public instance of the object manager.
    <ThreadStatic()> _
    Public g_oObjectManager As bObjectManager.ObjectManager

    ' Public instance of the business object.
    <ThreadStatic()> _
    Public g_oBusiness As Object

    Public g_iCurrencyID As Integer

    'Product Family Name for Help
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFOrion

    Public Const ScreenHelpID As Integer = 23000

    Sub Main_Renamed()

    End Sub
End Module