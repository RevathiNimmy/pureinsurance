Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
Module MainModule
	' ***************************************************************** '
	' Module Name: MainModule
	'
	' Date: 25/09/2000
	'
	' Description: Main module containing public variable/constants.
	'
	' Edit History:
	' ***************************************************************** '
	
	
	' Main public constant for all functions
	' to identify which application this is.
	Public Const ACApp As String = "iPMUDeferredRIManual"
	
	' Public interface constants used when
	' retrieving data from the resource file.
	
	' {* USER DEFINED CODE (Begin) *}
	
	' General Icons
	' Form
	' Buttons
	
	' Messages
	Public Const ACInvalidStatusTitle As Integer = 300
	Public Const ACInvalidStatus As Integer = 301
	Public Const ACBusinessFailTitle As Integer = 302
	Public Const ACBusinessFail As Integer = 303
	Public Const ACStatusSearching As Integer = 304
	Public Const ACStatusFound As Integer = 305
	Public Const ACRerateNow As Integer = 306
	Public Const ACReprintNow As Integer = 307
	Public Const ACAmendQuery As Integer = 308
	Public Const ACValidationTitle As Integer = 309
	Public Const ACMandatoryStartDate As Integer = 310
	Public Const ACMandatoryExpiryDate As Integer = 311
	Public Const ACMandatoryStartGreaterThanExpiry As Integer = 312
	Public Const ACLapseTitle As Integer = 313
	Public Const ACLapseComplete As Integer = 314
	Public Const ACAmendTitle As Integer = 315
	Public Const ACAmendProcessLaunchFail As Integer = 316
	Public Const ACAmendSetKeysFail As Integer = 317
	Public Const ACAmendSetProcessModesFail As Integer = 318
	Public Const ACAmendProcessFail As Integer = 319
	
	Public Const ACDateColumn As Integer = 2
	
	' Public contants used for the start
	' and end control indexes.
	Public Const ACControlStart As Integer = 0
	Public Const ACControlEnd As Integer = 1
	
	' Constant for the functions to identify
	' which class this is.
	Private Const ACClass As String = "MainModule"
	
	' AMB 05-Sep-03: 1.8.6 Deferred Reinsurance development - deferred RI listview columns
	Public Enum enumDeferredRIListView
		elBranchDesc = 0
		elDefRIStatusDesc = 1
		elInsuranceRef = 2
		elPartyShortName = 3
		elPartyName = 4
		elProductDesc = 5
	End Enum
	
	' AMB 05-Sep-03: 1.8.6 Deferred Reinsurance development - deferred RI data array positions
	Public Enum enumDeferredRIData
		edInsFileDeferredRIUsageID = 0
		edInsFileCnt = 1
		edInsuranceRef = 2
		edBranchID = 3
		edBranchDesc = 4
		edDefRIStatusID = 5
		edDefRIStatusDesc = 6
		edInsuredCnt = 7
		edPartyShortName = 8
		edPartyName = 9
		edProductID = 10
		edProductDesc = 11
		edInsFolderCnt = 12
	End Enum
	
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
 Public g_lInsuranceFileCnt As Integer
	
    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	' Public instance of the renewal business object.
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oDeferredRI As bSIRDeferredRIAuto.Business
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oAutoDeferredRI As bSIRDeferredRIAuto.Business
	
	' AMB 08-Sep-03: 1.8.6 Deferred Reinsurance development
	Public Const ksDeferredRIStatusAmend As Integer = 1
	Public Const ksDeferredRIStatusUpdate As Integer = 2
	' ***************************************************************** '
	' Name: ListViewSortByDate
	'
	' Description: Sorts the list view based on the column passed, and
	'              the order given.
	'
	' Note : This hasn't been tested on the first column. I suspect
	'        changes might need to be made if sorting on the first
	'        column is needed (CF 060899).
	'
	' ***************************************************************** '
	Public Function ListViewSortByDate(ByVal v_oListView As ListView, ByVal v_iSourceColumn As Integer, ByVal v_iDirection As SortOrder) As Integer
		
		Dim result As Integer = 0
		Dim sDate As String = ""
		Dim iIndex As Integer
		Const ACLVTag As String = "SORT_DATE_HIDDEN"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Add the column
			v_oListView.Columns.Add(ACLVTag, "Shhh Im hidden", CInt(VB6.TwipsToPixelsX(0)))
			
			' Get the index of this new column, -1 because it's a sub item
			iIndex = v_oListView.Columns.Count - 1
			
			' Not sorted yet
			ListViewHelper.SetSortedProperty(v_oListView, False)
			
			' Add the items
			For iLoop1 As Integer = 1 To v_oListView.Items.Count
				
				If v_iSourceColumn = 0 Then
					sDate = CDate(v_oListView.Items.Item(iLoop1 - 1).Text).ToString("yyyyMMddHHMMss")
				Else
					Dim TempDate As Date
					sDate = IIf(DateTime.TryParse(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(iLoop1 - 1), v_iSourceColumn).Text, TempDate), TempDate.ToString("yyyyMMddHHMMss"), ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(iLoop1 - 1), v_iSourceColumn).Text)
				End If
				ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(iLoop1 - 1), iIndex).Text = sDate
				
			Next iLoop1
			
			' Sort now
			ListViewHelper.SetSortOrderProperty(v_oListView, v_iDirection)
			
			' Set the sort key
			ListViewHelper.SetSortKeyProperty(v_oListView, iIndex)
			
			ListViewHelper.SetSortedProperty(v_oListView, True)
			
			' Remove the column now
			v_oListView.Columns.RemoveAt(iIndex)
			
			' Reset the sort key
			'eck 010800 This resorts listview so remove it.
			'    v_oListView.SortKey = v_iSourceColumn%
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ListViewSortByDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ListViewSortByDate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Sub Main_Renamed()
		
	End Sub
End Module