Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Public Module MainModule
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
    Public Const ACApp As String = "iPMUCoverNote"

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Username.
    Public g_sUsername As New FixedLengthString(12)
    'Developer Guide No. 107
    <ThreadStatic()> _
    Public g_iUserID As Integer
    Public g_sPassword As New FixedLengthString(30)
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_sCallingAppName As String = ""
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iCompanyID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iCurrencyID As Integer
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iLogLevel As Integer

    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons

    ' Constants for ListView Columns
    Public Const ACIColSheetId As Integer = 0
    Public Const ACIColSheetNumber As Integer = 1
    Public Const ACIColCustomerName As Integer = 2
    Public Const ACIColStatus As Integer = 3
    Public Const ACIColPolicyNumber As Integer = 4
    Public Const ACIColBranch As Integer = 5
    Public Const ACIColAgent As Integer = 6
    Public Const ACIColDateImported As Integer = 7

    ' Constants for get sheets array
    Public Const ACListISheetId As Integer = 0
    Public Const ACListISheetNumber As Integer = 1
    Public Const ACListICustomerName As Integer = 2
    Public Const ACListIStatusId As Integer = 3
    Public Const ACListIStatusDescription As Integer = 4
    Public Const ACListIPolicyNumber As Integer = 5
    Public Const ACListIBranch As Integer = 6
    Public Const ACListIAgent As Integer = 7
    Public Const ACListIDateImported As Integer = 8

    ' Constants for select sheet array
    Public Const ACISheetId As Integer = 0
    Public Const ACISheetNumber As Integer = 1
    Public Const ACIPolicyId As Integer = 2
    Public Const ACIPolicyRef As Integer = 3
    Public Const ACIAssignedDate As Integer = 4
    Public Const ACIStatusId As Integer = 5
    Public Const ACIStatusCode As Integer = 6
    Public Const ACIStatusDescription As Integer = 7
    Public Const ACIComments As Integer = 8


    ' Form
    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle As Integer = 101

    'Labels
    Public Const ACBookNumber As Integer = 110
    Public Const ACStartNumber As Integer = 111
    Public Const ACEndNumber As Integer = 112
    Public Const ACAgent As Integer = 113
    Public Const ACEffectiveDate As Integer = 114
    Public Const ACBranch As Integer = 115
    Public Const ACBookStatus As Integer = 116
    Public Const ACCreatedDate As Integer = 117
    Public Const ACProducts As Integer = 118
    Public Const ACCoverNoteSheets As Integer = 119

    ' List View Headers
    Public Const ACColSheetId As Integer = 120
    Public Const ACColSheetNumber As Integer = 121
    Public Const ACColCustomerName As Integer = 122
    Public Const ACColStatus As Integer = 123
    Public Const ACColPolicyNumber As Integer = 124
    Public Const ACColBranch As Integer = 125
    Public Const ACColAgent As Integer = 126
    Public Const ACColDateImported As Integer = 127

    ' Buttons
    Public Const ACCloseButton As Integer = 200
    Public Const ACAddButton As Integer = 201
    Public Const ACEditButton As Integer = 202
    Public Const ACViewButton As Integer = 203
    Public Const ACDeleteButton As Integer = 204
    Public Const ACOkButton As Integer = 205
    Public Const ACApplyButton As Integer = 206
    Public Const ACCancelButton As Integer = 207

    'Sheet status code
    Public Const ACSTATUSNOTISSUED As String = "NOTISS"
    Public Const ACSTATUSISSUED As String = "ISSUED"
    Public Const ACSTATUSCANCELLED As String = "CANCEL"
    Public Const ACSTATUSMISSING As String = "MISSING"

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

    ' Menus

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
    'Developer Guide No. 107
    <ThreadStatic()> _
 Public g_iSourceID As Integer
    Public g_iLanguageID As Integer

    'Private instance of the object manager.
    'Developer Guide No. 107
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

        Const kMethodName As String = "Class_Terminate"
        Try



        lColumnHeaderIndex = ColumnHeader.Index + 1 - 1

        With ListView

            '        Select Case lColumnHeaderIndex
            '        Case ACListIBalance
            '            If lColumnHeaderIndex = .SortKey Then
            '                lSortOrder = (.SortOrder + 1) Mod 2
            '            Else
            '                lSortOrder = 0
            '            End If
            '
            '            m_lReturn = ListViewSortByStringValue( _
            ''                            v_oListView:=ListView, _
            ''                            v_iSourceColumn:=lColumnHeaderIndex, _
            ''                            v_iDirection:=lSortOrder)
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
        End With

        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally

  
        End Try
    End Sub
End Module
