Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 02/07/1998
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMBAddress"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	'sj 23/07/2002 - start
	'Constants for Future Dated Addresses
	Public Const ACPartyVariantAddressCnt As Integer = 0
	Public Const ACPartyCnt As Integer = 1
	Public Const ACAddressCnt As Integer = 2
	Public Const ACOriginalAddressCnt As Integer = 3
	Public Const ACEffectiveDate As Integer = 4
	Public Const ACDateCreated As Integer = 5
	Public Const ACCommitInd As Integer = 6
	
	Public Const ACOptCurrentAddress As Integer = 0
	Public Const ACOptFutureAddress As Integer = 1
	'sj 23/07/2002 - end
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	
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
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iCountryID As Integer 'RWH(15/09/2000) RSAIB Process 06.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sCountryCode As String = "" 'eck030101
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	

	Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
	
	Public Const ScreenHelpID As Integer = 22
	
    'RWH(09/06/2000)
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oGIS As Object
	
	'SD 01/08/2002 Scalability changes - copied over from GIIFunc.Bas
	Public Enum CtlType
		ctlnone
		ctllabel
		ctlTextbox
		ctlcombobox
		ctlCheckbox
		ctlYesNoCheck
		ctlCommand
		ctluctDropdown
		ctlMSflexGrid
		ctlPictureBox
		ctlSSPanel
		ctlOptionButton
		ctlFrame
		ctlMEB
	End Enum
	
	
	
	Sub Main_Renamed()
		
		Dim s As String = ""
        'Modified by Archana Tokas on 4/20/2010 11:45:12 AM Developer Guide no. 88
        'Dim o As New ClassInterface
        Dim o As New Object
		
		Dim l As DialogResult = CType(o, SSP.S4I.Interfaces.ILocalInterface).Initialise()
		
		o.CallingAppName = "TEST"
		'o.Reference = "Salvo"
		'o.PostCode = "BR3 3P"
		
		l = MessageBox.Show("Yes to Add, No to Edit", Application.ProductName, MessageBoxButtons.YesNo)
		
		If l = System.Windows.Forms.DialogResult.Yes Then
			' o.AddressCnt = 22
			l = o.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd)
			l = o.Start()
			
		Else
			s = Interaction.InputBox("enter address_cnt")
			o.AddressCnt = CInt(s)
			l = o.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
			l = o.Start()
		End If
		
		
		
		'        vNavigate:=PMNavigateDisabled, _
		''        vProcessMode:=PMProcessModeGeneric, _
		''        vTransactionType:=PMTransactionTypeGeneric, _
		''        vEffectiveDate:=Now)
		
		'o.ContactCnt = 42
		'    o.ContactCnt = 94
		
		
		MessageBox.Show("address_cnt = " & o.AddressCnt & Strings.Chr(10).ToString() & o.Address1 & Strings.Chr(10).ToString() & o.Address2 & Strings.Chr(10).ToString() & o.Address3 & Strings.Chr(10).ToString() & o.Address4 & Strings.Chr(10).ToString() & o.PostalCode, Application.ProductName)
		
		o.Dispose()
		
		
	End Sub
    
End Module