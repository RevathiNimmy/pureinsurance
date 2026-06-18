Option Strict Off
Option Explicit On
Imports System
'Developer Guide no 129
Imports SharedFiles
Module MainModule
    'Modified as include the GetResData in this project 
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 11th July 1997
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iACTPeriod"

    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons


    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101

    Public Const ACListTitle1 As Integer = 102

    Public Const ACMainTabTitle0 As Integer = 111

    Public Const ACComboMonth As Integer = 150
    Public Const ACComboFourWeeks As Integer = 151
    Public Const ACComboQuarter As Integer = 152

    Public Const ACPeriodPrefixDefault As Integer = 160
    'DD 02/08/2002: For Multi-Branch
    Public Const ACSubBranchLabel As Integer = 161

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203
    Public Const ACAddButton As Integer = 204
    Public Const ACRemoveButton As Integer = 205
    Public Const ACEditButton As Integer = 206

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303
    Public Const ACPeriodNotUnique As Integer = 304
    Public Const ACPeriodNotUniqueTitle As Integer = 305

    ' Menus

    'BB Constants for Period Length Combo
    Public Const ACMonth As Integer = 1
    Public Const ACFourWeeks As Integer = 2
    Public Const ACQuarter As Integer = 3

    'BB Constants for the List data array subscripts.
    Public Const ACSubYearID As Integer = 0
    Public Const ACSubYearName As Integer = 1

    'BB Constants for the Add period and Grid period data array subscripts.
    ' Both arrays use same constants for Name and End Date ie slots 0 and 1
    Public Const ACSubPeriodName As Integer = 0
    Public Const ACSubPeriodEndDate As Integer = 1
    ' The add period array has YearName in slot 2
    Public Const ACSubPeriodYearName As Integer = 2
    ' The grid period array has the ID in slot 2 instead
    Public Const ACSubPeriodID As Integer = 2

    ' {* USER DEFINED CODE (End) *}

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Public source and language ID's from the
    ' Object Manager.
    Public g_iSourceID As Integer
    Public g_iLanguageID As Integer

    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_oObjectManager As bObjectManager.ObjectManager
    ' Public grid data array
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_vGridData(,) As Object

    'Product Family Name for Help

    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFOrion

    Public Const ScreenHelpID1 As Integer = 18000
    Public Const ScreenHelpID2 As Integer = 28000


    Public Sub Main()

    End Sub

    Sub New()
        Main()
    End Sub
    Sub JustForInvokeMain()
    End Sub
End Module