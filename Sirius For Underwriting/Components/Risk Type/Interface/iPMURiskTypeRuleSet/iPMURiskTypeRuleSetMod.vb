Option Strict Off
Option Explicit On

Imports System
Imports SharedFiles

Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 05/05/1999
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iPMURiskType"


    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101
    Public Const ACCode As Integer = 102
    Public Const ACDescription As Integer = 103
    Public Const ACEffectiveDate As Integer = 104
    Public Const ACRuleFileText As Integer = 105
    Public Const ACLive As Integer = 106
    Public Const ACRuleFile As Integer = 107

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

    'Fields Position
    Public Const ACFieldPosRiskTypeRuleSetID As Integer = 0
    'Public Const ACFieldPosCaptionID = 1
    Public Const ACFieldPosCode As Integer = 2
    Public Const ACFieldPosDescription As Integer = 3
    'Public Const ACFieldPosIsDeleted = 4
    Public Const ACFieldPosEffectiveDate As Integer = 5
    Public Const ACFieldPosRiskTypeID As Integer = 6
    Public Const ACFieldPosFileName As Integer = 7
    Public Const ACFieldPosLive As Integer = 8
    Public Const ACFieldPosRuleSetTypeID As Integer = 9
    Public Const ACFieldPosDREExecutorURL As Integer = 10
    Public Const ACFieldPosDREDefaultToken As Integer = 11
    Public Const ACFieldPosDREDefault As Integer = 12
    Public Const ACFieldPosDREQuote As Integer = 13
    Public Const ACFieldPosDREValidate As Integer = 14
    Public Const ACFieldPosPostPRE As Integer = 15
    Public Const ACFieldPosDataModelCode As Integer = 16
    Public Const ACFieldPosPrePRE As Integer = 17
    Public Const ACFieldPREVersion As Integer = 18
    Public Const ACFieldPRERulesetEffDate As Integer = 19
    Public Const ACFieldPREChildRulesetEffDate As Integer = 20


    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Public source and language ID's from the
    ' Object Manager.

    <ThreadStatic()> _
 Public g_iSourceID As Integer

    <ThreadStatic()> _
 Public g_iLanguageID As Integer

    ' Public instance of the object manager.

    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager

    ' KB 020801 Required for help text link
    Public Const ScreenHelpID As Integer = 4098
    'UPGRADE_NOTE: (1053) g_sProductFamily was changed from a Constant to a Variable. More Information: http://www.vbtonet.com/ewis/ewi1053.aspx
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions


    Sub Main_Renamed()

    End Sub

End Module