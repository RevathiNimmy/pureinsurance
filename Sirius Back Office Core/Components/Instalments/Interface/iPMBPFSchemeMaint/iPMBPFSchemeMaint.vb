Option Strict Off
Option Explicit On
Imports System
Imports System.Windows.Forms
'refer Developer Guide No. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 20/10/2000
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History: TF201000
    ' ***************************************************************** '


    Public Declare Function SetWindowPos Lib "user32" (ByVal hwnd As Integer, ByVal hWndInsertAfter As Integer, ByVal x As Integer, ByVal y As Integer, ByVal cx As Integer, ByVal cy As Integer, ByVal wFlags As Integer) As Integer
    Public Const HWND_TOPMOST As Integer = -1
    Public Const SWP_NOMOVE As Integer = &H2S
    Public Const SWP_NOSIZE As Integer = &H1S

    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iPMBPFSchemeMaint"
    'ECK 18/5/99
    Public Const ThisApp As String = "Premium Finance" ' Registry App constant.
    Public Const ThisKey As String = "Recent Files" ' Registry Key constant.

    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons

    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101

    Public Const ACSchemeNo As Integer = 102
    Public Const ACSchemeName As Integer = 103
    Public Const ACPartyCode As Integer = 104
    Public Const ACPartyName As Integer = 105

    Public Const ACListTitle1 As Integer = 120
    Public Const ACListTitle2 As Integer = 121
    Public Const ACListTitle3 As Integer = 122
    Public Const ACListTitle4 As Integer = 123
    Public Const ACListTitle5 As Integer = 124

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203

    Public Const ACFindNowButton As Integer = 204
    Public Const ACNewSearchButton As Integer = 205
    Public Const ACNewButton As Integer = 206
    Public Const ACEditButton As Integer = 207

    Public Const ACDeleteButton As Integer = 208
    Public Const ACUndeleteButton As Integer = 209

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
    Public Const ACICompanyNo As Integer = 0
    Public Const ACISchemeName As Integer = 1
    Public Const ACISchemeNo As Integer = 2
    Public Const ACISchemeVersion As Integer = 3
    Public Const ACIPartyCode As Integer = 4
    Public Const ACIPartyName As Integer = 5
    Public Const ACIPartyCnt As Integer = 6
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
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'eck220500
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iUserID As Integer
    ' Public instance of the object manager.
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager

    ' Public instance of the business object.

    'Developer Guide No. 107
    <ThreadStatic()> _
    Public g_oBusiness As Object
    'eck160500
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_oPMUser As Object

    Public Const ScreenHelpID As Integer = 1

    Sub Main_Renamed()

        Dim lError As Integer
        Dim vKeyArray(,) As Object
        Dim lInvariantKey As Integer
        Dim sClientCode As String = String.Empty
        Dim sClientName As String = String.Empty
        Dim sAddress1 As String = String.Empty
        Dim sAddress2 As String = String.Empty
        Dim sAddress3 As String = String.Empty
        Dim sAddress4 As String = String.Empty
        Dim sPostCode As String = String.Empty
        Dim sPortfolio As String = String.Empty
        Dim sCustomerId As String = String.Empty
        Dim lPartyCnt As Integer

        'Developer Guide No.108
        Dim oPFSchemeMaint As New Interface_Renamed

        With oPFSchemeMaint

            lError = .Initialise()

            .CallingAppName = "TEST"

            lError = .SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:="", vEffectiveDate:=DateTime.Now)

            ReDim vKeyArray(1, 0)

            '.SpecialParty = PMBPartyTypeConsultant

            lError = .Start()

            If .Status = gPMConstants.PMEReturnCode.PMOK Then

                lError = .GetKeys(vKeyArray)



                MessageBox.Show("SELECTED: " & CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0)) & ", " & _
                                CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2)), Application.ProductName)

            End If

            lPartyCnt = .PartyCnt

            lError = .GetDetailsFromPMBArray(v_lInvariantKey:=lInvariantKey, r_sClientCode:=sClientCode, r_sClientName:=sClientName, r_sAddress1:=sAddress1, r_sAddress2:=sAddress2, r_sAddress3:=sAddress3, r_sAddress4:=sAddress4, r_sPostCode:=sPostCode, r_sPortfolio:=sPortfolio, r_sCustomerId:=sCustomerId)

            .Dispose()

            oPFSchemeMaint = Nothing

        End With

    End Sub
End Module