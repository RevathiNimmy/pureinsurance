Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
'Developer Guide No: 129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 30/09/2000
	'
	' Description: Main module containing public variable/constants.
	'
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iCLMDefnFlds"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public constants used when populating the Combo box
	' or adding the reserve type for a peril type to the list view
	Public Const ACColReseverTypeID As Integer = 0
	Public Const ACColReseverTypeName As Integer = 1
	Public Const ACColReserveTypeDescription As Integer = 2
	Public Const ACColIncludeInTotal As Integer = 3
	Public Const ACColMainReserve As Integer = 4
	
	' Public constants used for populating the list view.
	Public Const ACColPrlRsvrTypeID As Integer = 0
	Public Const ACColPrlRsvrTypeName As Integer = 1
	Public Const ACColPrlRsvrTypeDescription As Integer = 2
	Public Const ACColPrlRsvrTypeIncludeInTotal As Integer = 3
	Public Const ACColPrlRsvrTypeMainReserve As Integer = 4
	
	Public Const ACAdd As Integer = 0 'Constant for ADD button index
	Public Const ACModify As Integer = 1 'Constant for MODIFY button index
	Public Const ACDelete As Integer = 2 'Constant for DELETE button index
	
	' General Icons
	' RESOURCE FILE CONSTANTS
	' Form
	Public Const ACInterfaceTitle As Integer = 100 'Data Definition
	Public Const ACTabTitle1 As Integer = 101 '&1-General
	
	'Labels
	Public Const AClblCaption As Integer = 104 'Caption :
	Public Const AClblDesc As Integer = 105 'Description :
	Public Const AClblMandatory As Integer = 106 'Mandatory ?
	Public Const AClblType As Integer = 107 'Type :
	Public Const AClblDispOrd As Integer = 108 'Display Order :
	Public Const AClblParty As Integer = 109 'Party :
	Public Const AClblLookup As Integer = 110 'Lookup :
	Public Const AClblReadOnly As Integer = 111 'Read-only ?
	
	'List-View Column Headers
	Public Const ACReserveName As Integer = 116 'Caption
	Public Const ACReserveDesc As Integer = 117 'Description
	Public Const ACReserveIncludeInTotal As Integer = 118 'Mandatory
	Public Const ACReserveMainReserve As Integer = 309 'MainReserve
	' Messages for Main reserve
	Public Const ACMainReserveNotSet As Integer = 310 'Main reserve is not set for this peril type.
	Public Const ACMainReserveMoreThanOne As Integer = 311 'There are more than one main reserves for this peril type.
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACAddButton As Integer = 203 '&Add
	Public Const ACModButton As Integer = 204 '&Modify
	Public Const ACDelButton As Integer = 205 '&Delete
	
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACClearDetailsTitle As Integer = 304
	Public Const ACClearDetails As Integer = 305
	
	Public Const ACReserveTypeAlreadyExists As Integer = 306
	Public Const ACInvalidAction As Integer = 307
	Public Const ACCannotDeleteReserveType As Integer = 308
	Public Const AClblMainReserve As Integer = 309
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	' Public instance of the business object.
    'Public g_oBackofficelink As bBackOfficeLink.bBOLink
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oBackofficelink As Object
	' Public instance of the business object.
	
	
	'holds the Risk/Peril Data Definition ID globally
	
	Sub Main_Renamed()
		
    End Sub

    
End Module