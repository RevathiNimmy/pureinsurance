Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 06 May 1997
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMUFindCoverNote"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Username.
    Public g_sUsername As New FixedLengthString(12)
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iUserID As Integer
    Public g_sPassword As New FixedLengthString(30)
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_sCallingAppName As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iCompanyID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iCurrencyID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLogLevel As Integer
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	' Constants for SubItems of ListView (ColumnHeader indicies are these + 1)
	Public Const ACListIBookId As Integer = 0
	Public Const ACListIBookNumber As Integer = 1
	Public Const ACListIStartNumber As Integer = 2
	Public Const ACListIEndNumber As Integer = 3
	Public Const ACListIAgent As Integer = 4
	Public Const ACListIStatus As Integer = 5
	Public Const ACListIBranch As Integer = 6
	Public Const ACListIDateUpdated As Integer = 7
	Public Const ACListICreatedDate As Integer = 8
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACFrameTitle As Integer = 101
	
	'Labels
	Public Const ACBookNumber As Integer = 110
	Public Const ACStartNumber As Integer = 111
	Public Const ACEndNumber As Integer = 112
	Public Const ACAgent As Integer = 113
	Public Const ACLastUpdate As Integer = 114
	Public Const ACBranch As Integer = 115
	Public Const ACPolicyNumber As Integer = 116
	Public Const ACCoverNoteStatus As Integer = 117
	Public Const ACAssignedDate As Integer = 118
	
	' List View Headers
	Public Const ACColBookId As Integer = 120
	Public Const ACColBookNumber As Integer = 121
	Public Const ACColStartNumber As Integer = 122
	Public Const ACColEndNumber As Integer = 123
	Public Const ACColAgent As Integer = 124
	Public Const ACColStatus As Integer = 125
	Public Const ACColBranch As Integer = 126
	Public Const ACColDateUpdated As Integer = 127
	Public Const ACColCreatedDate As Integer = 128
	
	'cmg/pb Control removed bug 2253 Public Const ACDepartment = 129
	Public Const ACSpare As Integer = 130
	Public Const ACShowDeleted As Integer = 132
	Public Const ACShowBalance As Integer = 136
	
	' Buttons
	Public Const ACCloseButton As Integer = 200
	Public Const ACNewButton As Integer = 201
	Public Const ACEditButton As Integer = 202
	Public Const ACViewButton As Integer = 203
	Public Const ACFindNowButton As Integer = 204
	Public Const ACNewSearchButton As Integer = 205
	
	' Messages
	Public Const ACCancelDetailsTitle As Integer = 300
	Public Const ACCancelDetails As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	
	Public Const ACClearDetailsTitle As Integer = 304
	Public Const ACClearDetails As Integer = 305
	Public Const ACStatusSearching As Integer = 306
	Public Const ACStatusFound As Integer = 307
	Public Const ACAllTypes As Integer = 308
	
	' Constants for the search data array indexes.
	Public Const ACIBookId As Integer = 0
	Public Const ACIBookNumber As Integer = 1
	Public Const ACIStartNumber As Integer = 2
	Public Const ACIEndNumber As Integer = 3
	Public Const ACIAgentId As Integer = 4
	Public Const ACIAgentName As Integer = 5
	Public Const ACIStatusId As Integer = 6
	Public Const ACIStatusDescription As Integer = 7
	Public Const ACIBranchId As Integer = 8
	Public Const ACIBranchName As Integer = 9
	Public Const ACIDateUpdated As Integer = 10
	Public Const ACICreatedDate As Integer = 11
	Public Const ACIEffectiveDate As Integer = 12
	
	' Constants for list view columns
	Public Const ACIColBookId As Integer = 0
	Public Const ACIColBookNumber As Integer = 1
	Public Const ACIColStartNumber As Integer = 2
	Public Const ACIColEndNumber As Integer = 3
	Public Const ACIColAgentName As Integer = 4
	Public Const ACIColStatus As Integer = 5
	Public Const ACIColBranch As Integer = 6
	Public Const ACIColDateUpdated As Integer = 7
	Public Const ACIColCreatedDate As Integer = 8
	
	
	' Constants for the lookup table array indexes.
	Public Const ACLSourceType As Integer = 0
	Public Const ACLBookType As Integer = 1
	Public Const ACLMax As Integer = 1
	
	' {* USER DEFINED CODE (End) *}
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the maxiumum search details.
	Public Const ACMaxSearchDetails As Integer = 500
	
	' Constant for the miniumum search length.
	Public Const ACMinSearchLength As Integer = 3
	
	' Public source and language ID's from the
    ' Object Manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_iLanguageID As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	' Stores the return value for a function call.
	Private m_lReturn As Integer
	
	'Product Family Name for Help
	'UPGRADE_NOTE: (1053) g_sProductFamily was changed from a Constant to a Variable. More Information: http://www.vbtonet.com/ewis/ewi1053.aspx
	Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusUnderwriting
	
	Sub Main_Renamed()
		
	End Sub
	
	' ***************************************************************** '
	' Name: OnColumnClick
	'
	' Description: Called by a form's listview column click event
	'
	' ***************************************************************** '
	Public Sub OnColumnClick(ByVal ListView As ListView, ByVal ColumnHeader As ColumnHeader)
		
		Dim lColumnHeaderIndex, lSortOrder As Integer
		
		Const kMethodName As String = "OnColumnClick"
		Try
		
		
		
		lColumnHeaderIndex = ColumnHeader.Index + 1 - 1
		
		'    With ListView
		
		'        Select Case lColumnHeaderIndex
		'        Case ACListIBalance
		'            If lColumnHeaderIndex = .SortKey Then
		'                lSortOrder = (.SortOrder + 1) Mod 2
		'            Else
		'                lSortOrder = 0
		'            End If
		'
		'            m_lReturn = ListViewSortByStringValue( _
		'v_oListView:=ListView, _
		'v_iSourceColumn:=lColumnHeaderIndex, _
		'v_iDirection:=lSortOrder)
		'
		'            .Sorted = False
		'            .SortKey = lColumnHeaderIndex
		'        Case Else
		'
		'            If lColumnHeaderIndex = .SortKey Then
		'            ' If current sort column header is
		'            ' pressed.
		'                ' Set sort order opposite of
		'                ' current direction.
		'                .SortOrder = (.SortOrder + 1) Mod 2
		'            Else
		'                ' Sort by this column (ascending).
		'                .Sorted = False
		'
		'                ' Turn off sorting so that the list
		'                ' is not sorted twice
		'                .SortOrder = 0
		'                .SortKey = lColumnHeaderIndex
		'                .Sorted = True
		'            End If
		'        End Select
		'    End With
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)
		
		Finally
		

		
		End Try
	End Sub
End Module
