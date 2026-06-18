'Add the GetResData in the program
Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
' developer guide no. 129 (guide)
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
	Public Const ACApp As String = "iACTFindBank"
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' Username.
	Public g_sUsername As New FixedLengthString(12)
	Public g_iUserID As Integer
	Public g_sPassword As New FixedLengthString(30)
	Public g_sCallingAppName As String = ""
	Public g_iCompanyID As Integer
	Public g_iCurrencyID As Integer
	Public g_iLogLevel As Integer
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	
	' Form
	Public Const ACInterfaceTitle As Integer = 100
	Public Const ACTabTitle As Integer = 100
	Public Const ACTabTitleCount As Integer = 1
	
	' ColumnHeaders
	
	Public Const ACColShortCode As Integer = 210
	Public Const ACColBranchId As Integer = 212
	Public Const ACColBranchName As Integer = 213
	Public Const ACColStatus As Integer = 213
	Public Const ACColHeadOffice As Integer = 214
	Public Const ACColAddress1 As Integer = 215
	
	' Labels
	Public Const ACShortCode As Integer = 102
	Public Const ACAccountName As Integer = 103
	
	
	' Buttons
	Public Const ACOKButton As Integer = 200
	Public Const ACCancelButton As Integer = 201
	Public Const ACHelpButton As Integer = 202
	Public Const ACNavigateButton As Integer = 203
	Public Const ACNewButton As Integer = 204
	Public Const ACEditButton As Integer = 205
	Public Const ACFindNowButton As Integer = 206
	Public Const ACNewSearchButton As Integer = 207
	
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
	Public Const ACLLedgerType As Integer = 0
	Public Const ACLAccountType As Integer = 1
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
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	
	'TF100802 - Move these to class level instances
	'particularly as this component is called from iBank on same thread
	' Public instance of the object manager.
	'Public g_oObjectManager As bObjectManager.ObjectManager
	' Public instance of the business object.
	'Public g_oBusiness As Object
	
	' Stores the return value for a function call.
	Private m_lReturn As Integer
	
	'Product Family Name for Help
    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFOrion
	
	Public Const ScreenHelpID As Integer = 15000
	
	
	Sub Main_Renamed()
		
	End Sub
	
	' Global application functions
	
	' ***************************************************************** '
	' Name: OnColumnClick
	'
	' Description: Called by a form's listview column click event
	'
	' ***************************************************************** '
	Public Sub OnColumnClick(ByVal ListView As ListView, ByVal ColumnHeader As ColumnHeader)

        'Try 

        'With ListView
        '	' If current sort column header is
        '	' pressed.
        '	If ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(ListView) Then
        '		' Set sort order opposite of
        '		' current direction.
        '		ListViewHelper.SetSortOrderProperty(ListView, (ListViewHelper.GetSortOrderProperty(ListView) + 1) Mod 2)
        '	Else
        '		' Sort by this column (ascending).
        '		ListViewHelper.SetSortedProperty(ListView, False)

        '		' Turn off sorting so that the list
        '		' is not sorted twice
        '                 ListViewHelper.SetSortOrderProperty(ListView, SortOrder.Ascending)
        '		ListViewHelper.SetSortKeyProperty(ListView, ColumnHeader.Index + 1 - 1)
        '		ListViewHelper.SetSortedProperty(ListView, True)
        '	End If
        'End With




        '          With ListView
        '              If ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(ListView) Then
        '                  ListViewHelper.SetSortedProperty(ListView, False)
        '                  If ListViewHelper.GetSortOrderProperty(ListView) = SortOrder.Ascending Then
        '                      ListViewHelper.SetSortOrderProperty(ListView, SortOrder.Descending)
        '                  Else
        '                      ListViewHelper.SetSortOrderProperty(ListView, SortOrder.Ascending)

        '                  End If
        '                  ListViewHelper.SetSortKeyProperty(ListView, ColumnHeader.Index + 1 - 1)

        '              Else
        '                  ListViewHelper.SetSortedProperty(ListView, False)
        '                  If ListViewHelper.GetSortOrderProperty(ListView) = SortOrder.Ascending Then
        '                      ListViewHelper.SetSortOrderProperty(ListView, SortOrder.Descending)
        '                  Else
        '                      ListViewHelper.SetSortOrderProperty(ListView, SortOrder.Ascending)

        '                  End If
        '                  ListViewHelper.SetSortKeyProperty(ListView, ColumnHeader.Index + 1 - 1)
        '              End If

        '          End With

        'Catch excep As System.Exception



        '	' Error Section

        '	' Log Error.
        '	iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="OnColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

        Exit Sub

        'End Try
		
    End Sub
   
End Module