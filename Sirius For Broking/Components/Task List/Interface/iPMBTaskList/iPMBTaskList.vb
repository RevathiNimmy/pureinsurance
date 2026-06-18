Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Module MainModule
	
	Public Const ACListTaskTypeAll As String = "All"
	Public Const ACListTaskTypeNew As String = "New"
	Public Const ACListTaskTypeInProgress As String = "In Progress"
	Public Const ACListTaskTypeComplete As String = "Complete"
	Public Const ACListTaskTypeInComplete As String = "InComplete"
	Public Const ACListTaskTypeAllButComplete As String = "(Not Complete)"
	
	Public Const ACListShowSystemOnly As String = "System"
	Public Const ACListShowSystemUser As String = "User"
	Public Const ACListShowSystemAll As String = "(All)"
	
	Public Const ACTaskTypeDescSingle As String = "Non-Navigator Function"
	Public Const ACTaskTypeDescNavigator As String = "Navigator Process"
	Public Const ACTaskTypeDescMemo As String = "Memo"
	Public Const ACTaskTypeDescSystem As String = "System Task"
	
	Public Const ACTaskStatusDescNew As String = "New"
	Public Const ACTaskStatusDescInProgress As String = "In Progress"
	Public Const ACTaskStatusDescInComplete As String = "InComplete"
	Public Const ACTaskStatusDescComplete As String = "Complete"
	
	' Date Range Combo Constants
	Public Const ACDateRangeDescAll As String = "(All Dates)"
	Public Const ACDateRangeDescToday As String = "Today"
	Public Const ACDateRangeDescNext1 As String = "Tomorrow"
	Public Const ACDateRangeDescNext2 As String = "Next 2 Days"
	Public Const ACDateRangeDescNext3 As String = "Next 3 Days"
	Public Const ACDateRangeDescNext4 As String = "Next 4 Days"
	Public Const ACDateRangeDescNext5 As String = "Next 5 Days"
	Public Const ACDateRangeDescNext6 As String = "Next 6 Days"
	Public Const ACDateRangeDescNext7 As String = "Next 7 Days"
	Public Const ACDateRangeDescNext14 As String = "Next 14 Days"
	Public Const ACDateRangeDescNext28 As String = "Next 28 Days"
	
	Public Const ACDateRangeIndexAll As Integer = 0
	Public Const ACDateRangeIndexToday As Integer = 1
	Public Const ACDateRangeIndexNext1 As Integer = 2
	Public Const ACDateRangeIndexNext2 As Integer = 3
	Public Const ACDateRangeIndexNext3 As Integer = 4
	Public Const ACDateRangeIndexNext4 As Integer = 5
	Public Const ACDateRangeIndexNext5 As Integer = 6
	Public Const ACDateRangeIndexNext6 As Integer = 7
	Public Const ACDateRangeIndexNext7 As Integer = 8
	Public Const ACDateRangeIndexNext14 As Integer = 9
	Public Const ACDateRangeIndexNext28 As Integer = 10
	
	Public Const ACUserGroupAllGroups As String = "All Groups"
	Public Const ACUserGroupYourGroups As String = "Your Groups"
	
	Public Const ACSTUrgentCol As Integer = 0
	Public Const ACSTStatusCol As Integer = 1
	Public Const ACSTDueDateCol As Integer = 2
	Public Const ACSTDescriptionCol As Integer = 3
	Public Const ACSTCustomerCol As Integer = 4
	Public Const ACSTTaskTypeCol As Integer = 5
	Public Const ACSTUserGroupCol As Integer = 6
	Public Const ACSTUserCol As Integer = 7
	Public Const ACSTDueDateSortableCol As Integer = 8
	
	' Constant for the functions to identify
	' which class this is.
	'Public Const ACApp = "iPMBTaskList"
	Public Const ACApp As String = "TaskList"
	
	Private Const ACClass As String = "MainModule"
	
	'
	' Public source and language ID's from the
	' Object Manager.
	Public g_iSourceID As Integer
	Public g_iLanguageID As Integer
	Public g_sUsername As String = ""
	
    ' Public instance of the object manager.'developer guide no. 107
    'developer guide no. 107
    <ThreadStatic()> _
 Public g_oObjectManager As bObjectManager.ObjectManager
	
	' *************************************************************************
	'
	' Title:    Extended list view functions
	'
	' History:  22/02/99    CTAF Created
	'           20/05/99    CTAF Added ListViewAutoSize
	'           09/07/99    CTAF Added ListViewSortByDate
	'           06/08/99    CTAF Added comments on above two functions.
	'                       CTAF Changed SendMessage declaration to Private.
	'           09/08/99    CTAF Fixed bug in sortbydate function that stopped
	'                            module from compiling. Strange no one noticed :)
	'           24/08/99    CTAF Added BatchStart + BatchEnd functions
	'
	' *************************************************************************
	
	
	' Prototypes
	Private Declare Function SendMessage Lib "user32"  Alias "SendMessageA"(ByVal hwnd As Integer, ByVal wMsg As Integer, ByVal wParam As Integer, ByVal lParam As Integer) As Integer
	Private Declare Function LockWindowUpdate Lib "user32" (ByVal hwndLock As Integer) As Integer
	
	' Constants
	Private Const LVM_FIRST As Integer = &H1000s
	
	Private Const LVM_SETEXTENDEDLISTVIEWSTYLE As Integer = LVM_FIRST + 54
	Private Const LVM_GETEXTENDEDLISTVIEWSTYLE As Integer = LVM_FIRST + 55
	
	Private Const LVS_EX_GRIDLINES As Integer = &H1s
	Private Const LVS_EX_TRACKSELECT As Integer = &H8s
	Private Const LVS_EX_FULLROWSELECT As Integer = &H20s
	Private Const LVS_EX_ONECLICKACTIVATE As Integer = &H40s
	Private Const LVS_EX_TWOCLICKACTIVATE As Integer = &H80s
	Private Const LVS_EX_INFOTIP As Integer = &H400s
	Private Const LVS_EX_UNDERLINEHOT As Integer = &H800s
	Private Const LVS_EX_UNDERLINECOLD As Integer = &H1000s
	
	' ************************************************************************************
	'
	' Function: SetExtraListViewProperties
	'
	' Note: Only RowSelect and GridLines will work WITHOUT Internet Explorer 4 installed,
	'       or the equivalent version of the common controls libraries.
	'
	' ************************************************************************************
	Public Function SetExtraListViewProperties(ByVal v_hWndList As Integer, Optional ByVal v_vShowRowSelect As Boolean = False, Optional ByVal v_vShowGridLines As Boolean = False, Optional ByVal v_vTrackSelect As Boolean = False, Optional ByVal v_vOneClickActivate As Boolean = False, Optional ByVal v_vTwoClickActivate As Boolean = False, Optional ByVal v_vInfoTip As Boolean = False, Optional ByVal v_vUnderLineHot As Boolean = False, Optional ByVal v_vUnderLineCold As Boolean = False) As Integer
		
		Dim result As Integer = 0
		Dim lMask, lNewStyle, lReturn As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Mask tells the control what properties we want to set
			lMask = 0
			
			' Get the current style of the list view
			Dim handle As GCHandle = GCHandle.Alloc(lMask, GCHandleType.Pinned)
			Try 
				Dim tmpPtr As IntPtr = handle.AddrOfPinnedObject()
				lNewStyle = SendMessage(v_hWndList, CInt(LVM_GETEXTENDEDLISTVIEWSTYLE), 0, tmpPtr)
				lMask = Marshal.ReadInt32(tmpPtr)
			Finally 
				handle.Free()
			End Try
			
			' Default any missing parameters

			If Information.IsNothing(v_vShowRowSelect) Then
				v_vShowRowSelect = False
			Else
				' If not missing then we want to fiddle with this property
				lNewStyle = lNewStyle And (Not LVS_EX_FULLROWSELECT)
			End If
			

			If Information.IsNothing(v_vShowGridLines) Then
				v_vShowGridLines = False
			Else
				lNewStyle = lNewStyle And (Not LVS_EX_GRIDLINES)
			End If
			

			If Information.IsNothing(v_vTrackSelect) Then
				v_vTrackSelect = False
			Else
				lNewStyle = lNewStyle And (Not LVS_EX_TRACKSELECT)
			End If
			

			If Information.IsNothing(v_vOneClickActivate) Then
				v_vOneClickActivate = False
			Else
				lNewStyle = lNewStyle And (Not LVS_EX_ONECLICKACTIVATE)
			End If
			

			If Information.IsNothing(v_vTwoClickActivate) Then
				v_vTwoClickActivate = False
			Else
				lNewStyle = lNewStyle And (Not LVS_EX_TWOCLICKACTIVATE)
			End If
			

			If Information.IsNothing(v_vInfoTip) Then
				v_vInfoTip = False
			Else
				lNewStyle = lNewStyle And (Not LVS_EX_INFOTIP)
			End If
			

			If Information.IsNothing(v_vUnderLineHot) Then
				v_vUnderLineHot = False
			Else
				lNewStyle = lNewStyle And (Not LVS_EX_UNDERLINEHOT)
			End If
			

			If Information.IsNothing(v_vUnderLineCold) Then
				v_vUnderLineCold = False
			Else
				lNewStyle = lNewStyle And (Not LVS_EX_UNDERLINECOLD)
			End If
			
			
			' Full row select ?
			If v_vShowRowSelect Then
				lNewStyle = lNewStyle Or LVS_EX_FULLROWSELECT
				lMask = lMask Or LVS_EX_FULLROWSELECT
			End If
			
			' Grid Lines ?
			If v_vShowGridLines Then
				lNewStyle = lNewStyle Or LVS_EX_GRIDLINES
				lMask = lMask Or LVS_EX_GRIDLINES
			End If
			
			If v_vTrackSelect Then
				lNewStyle = lNewStyle Or LVS_EX_TRACKSELECT
				lMask = lMask Or LVS_EX_TRACKSELECT
			End If
			
			If v_vOneClickActivate Then
				lNewStyle = lNewStyle Or LVS_EX_ONECLICKACTIVATE
				lMask = lMask Or LVS_EX_ONECLICKACTIVATE
			End If
			
			If v_vTwoClickActivate Then
				lNewStyle = lNewStyle Or LVS_EX_TWOCLICKACTIVATE
				lMask = lMask Or LVS_EX_TWOCLICKACTIVATE
			End If
			
			'LVS_EX_INFOTIP
			If v_vInfoTip Then
				lNewStyle = lNewStyle Or LVS_EX_INFOTIP
				lMask = lMask Or LVS_EX_INFOTIP
			End If
			
			'LVS_EX_UNDERLINEHOT
			If v_vUnderLineHot Then
				lNewStyle = lNewStyle Or LVS_EX_UNDERLINEHOT
				lMask = lMask Or LVS_EX_UNDERLINEHOT
			End If
			
			'LVS_EX_UNDERLINECOLD
			If v_vUnderLineCold Then
				lNewStyle = lNewStyle Or LVS_EX_UNDERLINECOLD
				lMask = lMask Or LVS_EX_UNDERLINECOLD
			End If
			
			' Set the extended properties of the list view
			Dim handle2 As GCHandle = GCHandle.Alloc(lNewStyle, GCHandleType.Pinned)
			Try 
				Dim tmpPtr2 As IntPtr = handle2.AddrOfPinnedObject()
				lReturn = SendMessage(v_hWndList, CInt(LVM_SETEXTENDEDLISTVIEWSTYLE), lMask, tmpPtr2)
				lNewStyle = Marshal.ReadInt32(tmpPtr2)
			Finally 
				handle2.Free()
			End Try
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetExtraListViewProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetExtraListViewProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: ListViewAutoSize
	'
	' Description: Resizes the column widths of a list view so that
	'              all information is visible.
	'
	'              If [bSizeHeaders] is true then it will also include
	'              the column headers in the sizing.
	'
	'              You might not want to resize the last column if you
	'              have a hidden date in it.
	'
	'              Note 1: Doesn't work on user controls properly...yet.
	'
	'              Note 2: It uses the control's parent's font to calculate
	'                      font width. So, if your control has the Verdanna
	'                      font, for example, then the form must have that
	'                      font too otherwise the sizes will be slightly out.
	'
	' ***************************************************************** '
	Public Function ListViewAutoSize(ByRef lvwList As ListView, Optional ByRef bSizeHeaders As Boolean = True, Optional ByRef bResizeLastColumn As Boolean = True) As Integer
		
		Dim result As Integer = 0
        Dim vArray() As Object
		Dim lWidth As Integer
		Dim iOffset, iUpper As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Make an array to store the widths in
			ReDim vArray(lvwList.Columns.Count - 1)
			
			' Initialise the array

			iUpper = vArray.GetUpperBound(0)
			For iLoop1 As Integer = 1 To iUpper
				If bSizeHeaders Then

                    vArray(iLoop1) = CInt(VB6.PixelsToTwipsX(lvwList.Parent.CreateGraphics().MeasureString(lvwList.Columns.Item(iLoop1 - 1).Text, lvwList.Parent.Font).Width))
				Else

					vArray(iLoop1) = -1
				End If
			Next iLoop1
			
			' Go across each header and find the biggest item
			For iLoop1 As Integer = 1 To lvwList.Items.Count
				
				' Do the first column
				lWidth = CInt(VB6.PixelsToTwipsX(lvwList.Parent.CreateGraphics().MeasureString(lvwList.Items.Item(iLoop1 - 1).Text, lvwList.Parent.Font).Width))

                If lWidth > CDbl(vArray(1)) Then

                    vArray(1) = lWidth
                End If

            Next iLoop1

            ' Add a little extra for the icon !


            vArray(1) = CDbl(vArray(1)) + 40

            ' Now do the subitems (other columns)

            For iLoop1 As Integer = 1 To vArray.GetUpperBound(0) - 1

                For iLoop2 As Integer = 1 To lvwList.Items.Count
                    lWidth = CInt(VB6.PixelsToTwipsX(lvwList.Parent.CreateGraphics().MeasureString(ListViewHelper.GetListViewSubItem(lvwList.Items.Item(iLoop2 - 1), iLoop1).Text, lvwList.Parent.Font).Width))

                    If lWidth > CDbl(vArray(iLoop1 + 1)) Then

                        vArray(iLoop1 + 1) = lWidth
                    End If
                Next iLoop2

            Next iLoop1

            ' Dont do the last one if not wanted
            If bResizeLastColumn Then
                iOffset = 0
            Else
                iOffset = 1
            End If

            ' Now set the column header widths

            For iLoop1 As Integer = 1 To vArray.GetUpperBound(0) - iOffset


                lvwList.Columns.Item(iLoop1 - 1).Width = CInt(VB6.TwipsToPixelsX(CDbl(vArray(iLoop1))))
            Next iLoop1
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ListViewAutoSize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ListViewAutoSize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
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
					sDate = ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(iLoop1 - 1), v_iSourceColumn).Text
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
	'eck010800
	' ***************************************************************** '
	' Name: ListViewSortByValue
	'
	' Description: Sorts the list view based on the column passed, and
	'              the order given.
	'
	' Note : This hasn't been tested on the first column. I suspect
	'        changes might need to be made if sorting on the first
	'        column is needed (CF 060899).
	'
	' ***************************************************************** '
	Public Function ListViewSortByValue(ByVal v_oListView As ListView, ByVal v_iSourceColumn As Integer, ByVal v_iDirection As SortOrder) As Integer
		Dim result As Integer = 0
		Dim cValue As Decimal
		Dim sValue As String = ""
		Dim iIndex As Integer
		Const ACLVTag As String = "SORT_VALUE_HIDDEN"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Add the column
			v_oListView.Columns.Add(ACLVTag, "Shhh Im hidden", CInt(VB6.TwipsToPixelsX(1000)))
			
			' Get the index of this new column, -1 because it's a sub item
			iIndex = v_oListView.Columns.Count - 1
			
			' Not sorted yet
			ListViewHelper.SetSortedProperty(v_oListView, False)
			
			' Add the items
			For iLoop1 As Integer = 1 To v_oListView.Items.Count
				
				If v_iSourceColumn = 0 Then
					sValue = StringsHelper.Format(v_oListView.Items.Item(iLoop1 - 1).Text, "#,##0.00")
				Else
					cValue = CDec(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(iLoop1 - 1), v_iSourceColumn).Text.Substring(0, ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(iLoop1 - 1), v_iSourceColumn).Text.IndexOf(" "c) + 1)) + 1000000000
					sValue = StringsHelper.Format(cValue, "0000000000.00")
				End If
				ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(iLoop1 - 1), iIndex).Text = sValue
				
			Next iLoop1
			
			' Sort now
			ListViewHelper.SetSortOrderProperty(v_oListView, v_iDirection)
			
			' Set the sort key
			ListViewHelper.SetSortKeyProperty(v_oListView, iIndex)
			
			ListViewHelper.SetSortedProperty(v_oListView, True)
			
			' Remove the column now
			v_oListView.Columns.RemoveAt(iIndex)
			
			' Reset the sort key
			'eck 010800
			'    v_oListView.SortKey = v_iSourceColumn%
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ListViewSortByValue Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ListViewSortByValue", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	'eck010900
	' ***************************************************************** '
	' Name: ListViewSortByStringValue
	'
	' Description: Sorts the list view based on the column passed, and
	'              the order given.
	'
	' Note : This hasn't been tested on the first column. I suspect
	'        changes might need to be made if sorting on the first
	'        column is needed (CF 060899).
	'
	' ***************************************************************** '
	Public Function ListViewSortByStringValue(ByVal v_oListView As ListView, ByVal v_iSourceColumn As Integer, ByVal v_iDirection As SortOrder) As Integer
		Dim result As Integer = 0
		Dim cValue As Decimal
		Dim sValue As String = ""
		Dim iIndex As Integer
		Const ACLVTag As String = "SORT_VALUE_HIDDEN"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Add the column
			v_oListView.Columns.Add(ACLVTag, "Shhh Im hidden", CInt(VB6.TwipsToPixelsX(1000)))
			
			' Get the index of this new column, -1 because it's a sub item
			iIndex = v_oListView.Columns.Count - 1
			
			' Not sorted yet
			ListViewHelper.SetSortedProperty(v_oListView, False)
			
			' Add the items
			For iLoop1 As Integer = 1 To v_oListView.Items.Count
				
				If v_iSourceColumn = 0 Then
					sValue = StringsHelper.Format(v_oListView.Items.Item(iLoop1 - 1).Text, "#,##0.00")
				Else
					sValue = ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(iLoop1 - 1), v_iSourceColumn).Text.Substring(0, ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(iLoop1 - 1), v_iSourceColumn).Text.IndexOf(" "c) + 1)
					sValue.TrimEnd()
					If sValue.StartsWith("-") Then
						sValue = Mid(sValue, 2, sValue.Length - 1)
					End If
					cValue = CDec(sValue) + 1000000000
					sValue = StringsHelper.Format(cValue, "0000000000.00")
					
				End If
				ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(iLoop1 - 1), iIndex).Text = sValue
				
			Next iLoop1
			
			' Sort now
			ListViewHelper.SetSortOrderProperty(v_oListView, v_iDirection)
			
			' Set the sort key
			ListViewHelper.SetSortKeyProperty(v_oListView, iIndex)
			
			ListViewHelper.SetSortedProperty(v_oListView, True)
			
			' Remove the column now
			v_oListView.Columns.RemoveAt(iIndex)
			
			' Reset the sort key
			'eck 010800
			'    v_oListView.SortKey = v_iSourceColumn%
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ListViewSortByStringValue Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ListViewSortByStringValue", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	' ***************************************************************** '
	'
	' Name: ListViewBatchStart
	'
	' Description: Use when you start a batch of ListItem.Add's.
	'              This will disable the listview from being updated.
	'
	' History: 24/08/1999 CTAF - Created.
	'
	' ***************************************************************** '
	Public Function ListViewBatchStart(ByRef lvwList As ListView) As Integer
		
		Dim result As Integer = 0
		Dim lReturn As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Call the API Function
			lReturn = LockWindowUpdate(hwndLock:=lvwList.Handle.ToInt32())
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ListViewBatchStart Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ListViewBatchStart", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: ListViewBatchEnd
	'
	' Description: As above, but called when ending the batch.
	'
	' History: 24/08/1999 CTAF - Created.
	'
	' ***************************************************************** '
	Public Function ListViewBatchEnd() As Integer
		
		Dim result As Integer = 0
		Dim lReturn As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			lReturn = LockWindowUpdate(hwndLock:=0)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ListViewBatchEnd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ListViewBatchEnd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	

	Public Sub Main()
		
	End Sub
    Sub New()
        Main()
    End Sub
	Sub JustForInvokeMain()
	End Sub
End Module
