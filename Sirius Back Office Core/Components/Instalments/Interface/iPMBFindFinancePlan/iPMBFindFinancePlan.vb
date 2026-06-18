Option Strict Off
Option Explicit On
Imports System
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
 Public Module MainModule

    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 17/02/1997
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' SP130199 - Remove NavigatorV3 class an put in stub so can be called
    ' iteratively.
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iPMBFindFinancePlan"

    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons
    ' Form
    Public Const ACInterfaceTitle As Integer = 100

    Public Const ACTabTitle1 As Integer = 101

    Public Const ACShortName As Integer = 102
    Public Const ACLongName As Integer = 103

    Public Const ACListTitle1 As Integer = 120
    Public Const ACListTitle2 As Integer = 121
    Public Const ACListTitle3 As Integer = 122
    Public Const ACListTitle4 As Integer = 123
    Public Const ACListTitle5 As Integer = 124
    Public Const ACListTitle6 As Integer = 126
    Public Const ACListTitle7 As Integer = 125
    'DC030106 PN26062 new captions
    Public Const ACListTitle8 As Integer = 127
    Public Const ACListTitle9 As Integer = 128

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203
    Public Const ACNewButton As Integer = 204
    Public Const ACEditButton As Integer = 205
    Public Const ACStatementButton As Integer = 206

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
    'DC030106 PN260562 was ACIPlanReference
    Public Const ACIPlanRefCnt As Integer = 0
    Public Const ACIPlanVersion As Integer = 1
    Public Const ACIPolicyNumber As Integer = 2
    Public Const ACIAccountNo As Integer = 3
    Public Const ACIAmount As Integer = 4
    Public Const ACIFrequency As Integer = 5
    Public Const ACINextPaymentDate As Integer = 6
    Public Const ACIRemainingInstalments As Integer = 7
    Public Const ACIStatus As Integer = 8
    'SJ 26/02/2004 - start
    Public Const ACIAlternateReference As Integer = 9
    'SJ 26/02/2004 - end
    'DC030106 PN26062
    Public Const ACIPlanReference As Integer = 10
    Public Const ACIFinanceProvider As Integer = 11
    Public Const ACIMax As Integer = 14

    ' {* USER DEFINED CODE (End) *}

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the maxiumum search details.
    Public Const ACMaxSearchDetails As Integer = 500

    ' Constant for the miniumum search length.
    'Modified by ECK 11/05/99
    Public Const ACMinSearchLength As Integer = 1

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Public source and language ID's from the
    ' Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iLanguageID As Integer
    'eck220500
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_iUserID As Integer
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_oObjectManager As bObjectManager.ObjectManager

    ' Public instance of the business object.
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_oBusiness As Object

    'developer guide no. 107
    <ThreadStatic()> _
    Public g_oParty As Object

    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

    Public Const ScreenHelpID As Integer = 1
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_bGenericConnectionStatus As Boolean

    '2005 Client Manager Security
    Public Const PMKeyNameFinancePlanEditAuthority As String = "FinancePlanEditAuthority"
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_oUserAuthorities As Object
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_bEditFinancePlanAuthority As Boolean

    Sub Main_Renamed()

        Dim vKeyArray(,) As Object

        Dim oFindFinancePlan As New Interface_Renamed

        Dim lError As Integer = CType(oFindFinancePlan, SSP.S4I.Interfaces.ILocalInterface).Initialise()

        oFindFinancePlan.CallingAppName = "TEST"

        lError = oFindFinancePlan.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:="", vEffectiveDate:=DateTime.Now)

        ReDim vKeyArray(1, 0)


        lError = oFindFinancePlan.Start()


        Dim lPartyCnt As Integer = oFindFinancePlan.PartyCnt


		oFindFinancePlan.Dispose()


    End Sub
    
End Module