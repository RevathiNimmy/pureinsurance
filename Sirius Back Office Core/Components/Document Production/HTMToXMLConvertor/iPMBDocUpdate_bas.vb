Option Strict Off
Option Explicit On
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 17/02/1997
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMUDocConversion"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	
	' Form
	Public Const ACInterfaceTitle As Short = 100
	Public Const ACTabTitle1 As Short = 101
	
	Public Const ACCode As Short = 102
	Public Const ACType As Short = 103
	
	Public Const ACListTitle1 As Short = 104
	Public Const ACListTitle2 As Short = 105
	Public Const ACListTitle3 As Short = 106
	Public Const ACListTitle4 As Short = 107
	
	' Buttons
	Public Const ACOKButton As Short = 200
	Public Const ACCancelButton As Short = 201
	Public Const ACHelpButton As Short = 202
	Public Const ACNavigateButton As Short = 203
	Public Const ACNewButton As Short = 204
	Public Const ACEditButton As Short = 205
	Public Const ACFindNowButton As Short = 206
	Public Const ACNewSearchButton As Short = 207
	Public Const ACDeleteButton As Short = 208
	
	' Messages
	Public Const ACCancelDetailsTitle As Short = 300
	Public Const ACCancelDetails As Short = 301
	Public Const ACBusinessFailTitle As Short = 302
	Public Const ACBusinessFail As Short = 303
	
	Public Const ACClearDetailsTitle As Short = 304
	Public Const ACClearDetails As Short = 305
	Public Const ACStatusSearching As Short = 306
	Public Const ACStatusFound As Short = 307
	
	Public Const ACLookupFailTitle As Short = 308
	Public Const ACLookupFail As Short = 309
	
	' Menus
	
	
	
	' {* USER DEFINED CODE (End) *}
	
	Public Const DbTag As String = "DB"
	Public Const TableTag As String = "TBL"
	Public Const FieldTag As String = "FLD"
	Public Const LoopTag As String = "LOOP"
	Public Const EndLoopTag As String = "ENDLOOP"
	Public Const FileTag As String = "FILE"
	Public Const QuestionTag As String = "KEY0"
	Public Const Separator As String = "_"
	Public Const ClauseTag As String = "CL" 'RWH(17/08/2000) RSAIB Process 12.
	
	Public Const lCLAUSE_TYPE_ID As Short = 7 'RWH(18/08/2000) RSAIB Process 12.
	
	'Bookmark array values
	Public Const BookmarkCode As Short = 0
	Public Const BookmarkName As Short = 1
	Public Const BookmarkValue As Short = 2
	Public Const BookmarkType As Short = 3
	Public Const BookmarkInstance1 As Short = 4
	Public Const BookmarkInstance2 As Short = 5
	Public Const BookmarkInstance3 As Short = 6
	
	'RWH(31/07/2000)
	'Field array values
	Public Const FieldCode As Short = 0
	Public Const FieldName As Short = 1
	Public Const FieldValue As Short = 2
	Public Const FieldType As Short = 3
	Public Const FieldInstance1 As Short = 4
	Public Const FieldInstance2 As Short = 5
	Public Const FieldInstance3 As Short = 6
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Short = 0
	Public Const ACControlEnd As Short = 1
	
	' Constant for the maxiumum search details.
	Public Const ACMaxSearchDetails As Short = 500
	
	' Constant for the miniumum search length.
	Public Const ACMinSearchLength As Short = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Short
	Public g_iLanguageID As Short
	Public g_sUserName As String
	
	' Public instance of the object manager.
	Public g_oObjectManager As bObjectManager.ObjectManager
	
	' Public instance of the business object.
	'Public g_oBusiness As Object
	Public g_oDataBase As Object
	Public g_oBusiness As Object
	
	'UPGRADE_NOTE: g_sProductFamily was changed from a Constant to a Variable. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="C54B49D7-5804-4D48-834B-B3D81E4C2F13"'
	Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
	
	Public g_oZipper As New bPMZipper.Business
	
	Public Const ScreenHelpID As Short = 1
	
	Public Const lLETTER_TYPE_ID As Short = 5
	Public Const lSUBDOC_TYPE_ID As Short = 9
	
	
	'UPGRADE_WARNING: Application will terminate when Sub Main() finishes. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="E08DDC71-66BA-424F-A612-80AF11498FF8"'
	 Public Sub main()
		Dim oInterface As Object
		oInterface = New Interface_Renamed
		
		'UPGRADE_WARNING: Couldn't resolve default property of object oInterface.Initialise. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		oInterface.Initialise()
		
		'UPGRADE_WARNING: Couldn't resolve default property of object oInterface.Start. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		oInterface.Start()
		
		'UPGRADE_WARNING: Couldn't resolve default property of object oInterface.Terminate. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
		oInterface.Dispose()
		'UPGRADE_NOTE: Object oInterface may not be destroyed until it is garbage collected. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6E35BFF6-CD74-4B09-9689-3E1A43DF8969"'
		oInterface = Nothing
	End Sub
End Module