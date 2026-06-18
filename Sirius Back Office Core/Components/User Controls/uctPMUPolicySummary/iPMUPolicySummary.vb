Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
'Modified by Archana Tokas on 5/3/2010 10:30:49 AM refer developer guide no. 129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 23/12/1997
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "uctPolicyControl"
	
	Public Const FeeImage As String = "PolicyImage"
	Public Const NarrativeImage As String = "PolicyImage"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACPolicyNo As Integer = 102
	Public Const ACLeadAgent As Integer = 103
	Public Const ACProduct As Integer = 104
	Public Const ACAccountHandler As Integer = 105
	Public Const ACCoverFrom As Integer = 106
	Public Const ACStatus As Integer = 107
	Public Const ACCurrency As Integer = 108
	Public Const ACPremiumPayable As Integer = 109
	Public Const ACCoverTo As Integer = 110
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	Public Const ACAddButton As Integer = 204
	Public Const ACEditButton As Integer = 205
	Public Const ACDeleteButton As Integer = 206
	Public Const ACPrevious As Integer = 207
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACRefExists As Integer = 304
	
	' Menus
	
	' {* USER DEFINED CODE (End) *}
	
	' Used by Voyager
	' Message Texts
	Public Const ACCancelDetailsTitleText As String = "Cancel Details"
	Public Const ACCancelDetailsText As String = "Cancelling will lose any changes" & Strings.Chr(13) & Strings.Chr(10) & "Do you want to cancel?"
	
	Public Const ACSaveDetailsTitleText As String = "Save Details"
	Public Const ACSaveDetailsText As String = "Details have changed" & Strings.Chr(13) & Strings.Chr(10) & "Do you want to save?"
	
	Public Const ACBusinessFailTitleText As String = "Business Object"
	Public Const ACBusinessFailText As String = "Unable to gain access to the business object" & Strings.Chr(13) & Strings.Chr(10) & "Please try later"
	
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
	
	Public Const ScreenHelpID As Integer = 4
	'UPGRADE_NOTE: (1053) g_sProductFamily was changed from a Constant to a Variable. More Information: http://www.vbtonet.com/ewis/ewi1053.aspx
	Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
    'Global reference to ListManager
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oListManager As Object
	
	'Start (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.1.3)
	Public Const kUSLangId As Integer = 2
	Public Const kUKLangId As Integer = 1
	'End (Girija chokkalingam) - (Tech Spec - S4IRD001 - US Localisation.doc) - (5.1.3)
	
	Sub Main_Renamed()
		
	End Sub
End Module