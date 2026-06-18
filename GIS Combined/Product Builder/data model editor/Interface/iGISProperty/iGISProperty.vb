Option Strict Off
Option Explicit On
Imports System
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 05/05/1999
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' RKS 27/04/2005 354-Standard Wording Control Enchancements
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iGISProperty"
	
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle1 As Integer = 101
	Public Const ACCaptionPropertyName As Integer = 102
	Public Const ACCaptionColumnName As Integer = 103
	Public Const ACCaptionDataType As Integer = 104
	Public Const ACCaptionPolarisProperty As Integer = 105
	Public Const ACCaptionIsInputProperty As Integer = 106
	Public Const ACCaptionIsIdentifier As Integer = 107
	Public Const ACCaptionIsPrimaryKey As Integer = 108
	Public Const ACCaptionIsDeleted As Integer = 109
	Public Const ACCaptionIsSearchProperty As Integer = 110
	Public Const ACCaptionNormal As Integer = 111
	Public Const ACCaptionGISList As Integer = 112
	Public Const ACCaptionPMLookup As Integer = 113
	Public Const ACCaptionParty As Integer = 114
	Public Const ACCaptionSumInsured As Integer = 115
	Public Const ACCaptionUserDefined As Integer = 116
	Public Const ACCaptionStandardWording As Integer = 117
	Public Const ACCaptionProduct As Integer = 118
	Public Const ACCaptionGISListId As Integer = 119
	Public Const ACCaptionLookupTableName As Integer = 120
	Public Const ACCaptionPartyType As Integer = 121
	Public Const ACCaptionSumInsuredType As Integer = 122
	Public Const ACCaptionUserDefinedTable As Integer = 123
	Public Const ACCaptionProductId As Integer = 124
	Public Const ACCaptionIndexLinkingId As Integer = 125
	Public Const ACCaptionSwiftSpecial As Integer = 126
	Public Const ACCaptionSwiftCC As Integer = 127
	Public Const ACCaptionSwiftClientSelector As Integer = 128
	Public Const ACCaptionSwiftAddressSelector As Integer = 129
	Public Const ACTabTitle2 As Integer = 130
	Public Const ACCaptionListType As Integer = 131
	Public Const ACCaptionSwiftSpecails As Integer = 132
	Public Const ACCaptionSwiftNotes As Integer = 133
	Public Const ACCaptionSwiftAddress As Integer = 134
	Public Const ACCaptionComboLookup As Integer = 135
	Public Const ACCaptionComboLookupTableName As Integer = 136
	Public Const ACCaptionCommonCodeType As Integer = 137
	Public Const ACCaptionSwiftListViewType As Integer = 138
	Public Const ACCaptionDocumentFilter As Integer = 139
	Public Const ACCaptionCaseHeader As Integer = 140
	Public Const ACCaptionCaseClaimLinks As Integer = 141
	
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
	
	
	'Array constants for the GIS Property
	Public Const ACPGISPropertyId As Integer = 0
	Public Const ACPGISObjectId As Integer = 1
	Public Const ACPPropertyName As Integer = 2
	Public Const ACPColumnName As Integer = 3
	Public Const ACPDataType As Integer = 4
	Public Const ACPIsInputProperty As Integer = 5
	Public Const ACPIsIdentifyingProperty As Integer = 6
	Public Const ACPIsPrimaryKey As Integer = 7
	Public Const ACPGISListId As Integer = 8
	Public Const ACPPolarisPropertyId As Integer = 9
	Public Const ACPIsDeleted As Integer = 10
	Public Const ACPIsSearchProperty As Integer = 11
	Public Const ACPPMLookupTableName As Integer = 12
	Public Const ACPPartyTypeId As Integer = 13
	Public Const ACPPMUSumInsuredType As Integer = 14
	Public Const ACPPMUStdWordingType As Integer = 15
	Public Const ACPGISUserDefHeaderId As Integer = 16
	Public Const ACPPMUProductId As Integer = 17
	Public Const ACPIndexLinking As Integer = 18
	
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
 Public g_sUsername As String = ""
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	
	Sub Main_Renamed()
		
	End Sub
End Module