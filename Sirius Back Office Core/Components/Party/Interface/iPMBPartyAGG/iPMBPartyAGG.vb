Option Strict Off
Option Explicit On
Imports System
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 08/07/02
	'
	' Description: Main module containing public variable/constants.
	'              Created from iPMBPartyAG.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMBPartyAGG"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACTabTitle2 As Integer = 102
	Public Const ACTabTitle3 As Integer = 103
	Public Const ACReference As Integer = 104
	Public Const ACPostcode As Integer = 105
	Public Const ACName As Integer = 106
	Public Const ACIsBranch As Integer = 107
	Public Const ACAgencyAgreement As Integer = 110
	Public Const ACAgencyNextReview As Integer = 111
	Public Const ACSource As Integer = 112
	Public Const ACIsHeadOffice As Integer = 113
	Public Const ACfraAppointment As Integer = 114
	Public Const ACHeadOffice As Integer = 115
	
	' TF031298
	Public Const ACFinancial As Integer = 150
	Public Const ACNotes As Integer = 153
	Public Const ACLetter As Integer = 154
	
	'RWH(24/07/2000) RSAIB Process 004
	' Form Constants for Address ListView
	Public Const ACAddressListPostCode As Integer = 155
	Public Const ACAddressListUsage As Integer = 156
	Public Const ACAddressListLine1 As Integer = 157
	Public Const ACAddressListLine2 As Integer = 158
	Public Const ACAddressListLine3 As Integer = 159
	Public Const ACAddressListLine4 As Integer = 160
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	Public Const ACAddButton As Integer = 204
	Public Const ACEditButton As Integer = 205
	Public Const ACDeleteButton As Integer = 206
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACHeadOfficeMissing As Integer = 305
	Public Const ACRefExists As Integer = 306
	' Menus
	
	
	' {* USER DEFINED CODE (End) *}
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	Public Const ACIADDRESS As String = "ADDRESS"
	
	' Public source and language ID's from the
    ' Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iCurrencyID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	

	Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
	
	Public Const ScreenHelpID As Integer = 7
	
	Sub Main_Renamed()
		
		'Dim o As iPMBPartyAG.Interface
		'Dim lReturn As Long
		'Dim vKeyArray As Variant
		'Dim s As String
		'
		'    Set o = New Interface
		'
		'    ReDim vKeyArray(1, 0)
		'
		'    s = MsgBox("yes to add, on to edit", vbYesNo)
		'
		'    If s = vbYes Then
		'
		'        'New ***************************************
		'        lReturn = o.Initialise()
		'
		'        o.CallingAppName = "TEST"
		'
		'        lReturn = o.SetProcessModes( _
		''            vTask:=PMAdd)
		'
		'        'New ***************************************
		'
		'    Else
		'
		'    'Edit ***************************************
		'        s = InputBox("Enter Party_cnt ", , 554)
		'        vKeyArray(0, 0) = PMKeyNamePartyCnt
		'                vKeyArray(1, 0) = CLng(s)
		'
		'        lReturn = o.SetKeys(vKeyArray)
		'        lReturn = o.Initialise()
		'
		'        o.CallingAppName = "TEST"
		'
		'        lReturn = o.SetProcessModes( _
		''            vTask:=PMEdit)
		'
		'    'Edit ***************************************
		'
		'    End If
		'
		'    lReturn = o.Start()
		'
		'    lReturn = o.GetKeys(vKeyArray)
		'
		'    'MsgBox "Are we done?..." & o.StepStatus
		'
		''    If o.Status = PMOK Then
		''        ' must change to InsuranceFileCnt! - JW
		''        MsgBox "SELECTED: " & o.PartyCnt & ", " & _
		'''        o.PartyCnt
		''    End If
		'
		'    lReturn = o.Terminate()
		'
		'    Set o = Nothing
		
    End Sub

End Module