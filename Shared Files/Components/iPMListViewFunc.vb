Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Globalization
Imports System.Runtime.InteropServices
Imports System.Windows.Forms
Imports VB6 = Microsoft.VisualBasic.Compatibility.VB6
Imports SharedFiles

Public Module ListViewFunc
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
	'           04/10/01    DRD  Added Checkbox functionality
	'           07/11/03    CLG  Removed refernces ComctlLib so it can be used by MSComctlLib
	'
	' *************************************************************************
	
	
	Private Const ACClass As String = "ListViewFunc"
	
	' Prototypes
    <DllImport("user32.dll")> _
    Private Function GetScrollPos(ByVal hWnd As System.IntPtr, ByVal nBar As Integer) As Integer

    End Function
	<DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Auto)>
	Private Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr

	End Function
	<DllImport("user32.dll")> _
    Private Function LockWindowUpdate(ByVal hwndLock As IntPtr) As Boolean

    End Function
	
	' Constants
	Private Const LVM_FIRST As Integer = &H1000s
	Private Const LVM_SETCOLUMNWIDTH As Integer = (LVM_FIRST + 30)
	Private Const LVSCW_AUTOSIZE As Integer = -1
	Private Const LVSCW_AUTOSIZE_USEHEADER As Integer = -2
	
	
	Private Const LVM_SETITEMSTATE As Integer = (LVM_FIRST + 43)
	Private Const LVM_GETITEMSTATE As Integer = (LVM_FIRST + 44)
	Private Const LVIS_STATEIMAGEMASK As Integer = &HF000S

	Private Const LVM_SETEXTENDEDLISTVIEWSTYLE As Long = LVM_FIRST + 54
	Private Const LVM_GETEXTENDEDLISTVIEWSTYLE As Integer = LVM_FIRST + 55
	
	Private Const LVS_EX_GRIDLINES As Integer = &H1s
	Private Const LVS_EX_CHECKBOXES As Integer = &H4s
	Private Const LVS_EX_TRACKSELECT As Integer = &H8s
	Private Const LVS_EX_FULLROWSELECT As Integer = &H20s
	Private Const LVS_EX_ONECLICKACTIVATE As Integer = &H40s
	Private Const LVS_EX_TWOCLICKACTIVATE As Integer = &H80s
	Private Const LVS_EX_INFOTIP As Integer = &H400s
	Private Const LVS_EX_UNDERLINEHOT As Integer = &H800s
	Private Const LVS_EX_UNDERLINECOLD As Integer = &H1000s
	
	Private Const BITSPIXEL As Integer = 12
	
	Public Function GetCheck(ByRef hwnd As Integer, ByRef lItemIndex As Integer) As Boolean
		
		Dim handle As GCHandle = GCHandle.Alloc(LVIS_STATEIMAGEMASK, GCHandleType.Pinned)
		Dim r As Integer = 0
		Try 
			Dim tmpPtr As IntPtr = handle.AddrOfPinnedObject()
			r = SendMessage(hwnd, LVM_GETITEMSTATE, lItemIndex, tmpPtr)
		Finally 
			handle.Free()
		End Try
		
		Return r And &H2000
	End Function
	
	' ************************************************************************************
	'
	' Function: SetExtraListViewProperties
	'
	' Note: Only RowSelect and GridLines will work WITHOUT Internet Explorer 4 installed,
	'       or the equivalent version of the common controls libraries.
	'
	' ************************************************************************************
	Public Function SetExtraListViewProperties(ByVal v_hWndList As Integer, Optional ByVal v_vShowRowSelect As Boolean = False, Optional ByVal v_vShowGridLines As Boolean = False, Optional ByVal v_vTrackSelect As Boolean = False, Optional ByVal v_vOneClickActivate As Boolean = False, Optional ByVal v_vTwoClickActivate As Boolean = False, Optional ByVal v_vInfoTip As Boolean = False, Optional ByVal v_vUnderLineHot As Boolean = False, Optional ByVal v_vUnderLineCold As Boolean = False, Optional ByVal v_vCheckBoxes As Boolean = False) As Integer
		
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
			

			If Information.IsNothing(v_vCheckBoxes) Then
				v_vCheckBoxes = False
			Else
				lNewStyle = lNewStyle And (Not LVS_EX_CHECKBOXES)
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
			
			' Checkboxes
			If v_vCheckBoxes Then
				lNewStyle = lNewStyle Or LVS_EX_CHECKBOXES
				lMask = lMask Or LVS_EX_CHECKBOXES
			End If
			
			' Set the extended properties of the list view
			Dim handle2 As GCHandle = GCHandle.Alloc(lNewStyle, GCHandleType.Pinned)
			Try 
				Dim tmpPtr2 As IntPtr = handle2.AddrOfPinnedObject()
				lReturn = SendMessage(v_hWndList, LVM_SETEXTENDEDLISTVIEWSTYLE, lMask, tmpPtr2)
				lNewStyle = Marshal.ReadInt64(tmpPtr2)
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
		Dim iOffset As Integer
		Dim lUpper As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Make an array to store the widths in
			ReDim vArray(lvwList.Columns.Count - 1)
			
			' Initialise the array

			lUpper = vArray.GetUpperBound(0)
			For lLoop1 As Integer = 1 To lUpper
				If bSizeHeaders Then

                    'vArray(lvwList.Parent.Font).Width)), lLoop1) = CInt(VB6.PixelsToTwipsX(lvwList.Parent.CreateGraphics().MeasureString(lvwList.Columns.Item(lLoop1 - 1).Text
                    vArray(lLoop1) = CInt(VB6.PixelsToTwipsX(lvwList.Parent.CreateGraphics().MeasureString(lvwList.Columns.Item(lLoop1 - 1).Text, lvwList.Parent.Font).Width))
				Else

                    vArray(lLoop1) = -1
				End If
			Next lLoop1
			
			' Go across each header and find the biggest item
			For lLoop1 As Integer = 1 To lvwList.Items.Count
				
				' Do the first column
				lWidth = CInt(VB6.PixelsToTwipsX(lvwList.Parent.CreateGraphics().MeasureString(lvwList.Items.Item(lLoop1 - 1).Text, lvwList.Parent.Font).Width))

                If lWidth > CDbl(vArray(1)) Then

                    vArray(1) = lWidth
                End If

            Next lLoop1

            ' Add a little extra for the icon !


            vArray(1) = CDbl(vArray(1)) + 40

            ' Now do the subitems (other columns)

            For lLoop1 As Integer = 1 To vArray.GetUpperBound(0) - 1

                For lLoop2 As Integer = 1 To lvwList.Items.Count
                    lWidth = CInt(VB6.PixelsToTwipsX(lvwList.Parent.CreateGraphics().MeasureString(ListViewHelper.GetListViewSubItem(lvwList.Items.Item(lLoop2 - 1), lLoop1).Text, lvwList.Parent.Font).Width))

                    If lWidth > CDbl(vArray(lLoop1 + 1)) Then

                        vArray(lLoop1 + 1) = lWidth
                    End If
                Next lLoop2

            Next lLoop1

            ' Dont do the last one if not wanted
            If bResizeLastColumn Then
                iOffset = 0
            Else
                iOffset = 1
            End If

            ' Now set the column header widths

            For lLoop1 As Integer = 1 To vArray.GetUpperBound(0) - iOffset


                lvwList.Columns.Item(lLoop1 - 1).Width = CInt(VB6.TwipsToPixelsX(CDbl(vArray(lLoop1))))
            Next lLoop1
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ListViewAutoSize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ListViewAutoSize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: ListView6AutoSize
	'
	' Description: Resizes the column widths of a v6 list view so that
	'              all information is visible. Far simpler than the v5
	'              version as the sizing is built into the control.
	'
	' ***************************************************************** '
	Public Sub ListView6Autosize(ByRef lvwList As ListView, Optional ByRef bSizeHeaders As Boolean = False)
		
		


		
		
		'/* Size each column based on the maximum of
		'/* EITHER the columnheader text width, or,
		'/* if the items below it are wider, the
		'/* widest list item in the column
		For col2adjust As Integer = 0 To lvwList.Columns.Count - 1
			If bSizeHeaders Then
                SendMessage(lvwList.Handle, LVM_SETCOLUMNWIDTH, col2adjust, LVSCW_AUTOSIZE_USEHEADER)
			Else
                SendMessage(lvwList.Handle, LVM_SETCOLUMNWIDTH, col2adjust, LVSCW_AUTOSIZE)
			End If
			
		Next col2adjust
		
		GoTo Finally_Renamed
		
		'----------------------------------------------------------------------------------------
		'Only for Debugging, the code will never execute this line
		'----------------------------------------------------------------------------------------


		
Catch_Renamed: 
		Select Case Information.Err().Number
			Case Else
				' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ListView6Autosize", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				
				GoTo Finally_Renamed
		End Select
		
Finally_Renamed: 
		Exit Sub
		
		
	End Sub
	
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
		Dim lLoop2 As Integer
		Dim iIndex As Integer
		Const ACLVTag As String = "SORT_DATE_HIDDEN"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'KB PN 4360
			'Check if we already have the extra sort column
			lLoop2 = v_oListView.Columns.Count
			If v_oListView.Columns.Item(lLoop2 - 1).Name = ACLVTag Then
				'do nothing as no need to add it
			Else
				' Add the column
				v_oListView.Columns.Add(ACLVTag, "Internal Sort", CInt(VB6.TwipsToPixelsX(0)))
			End If
			
			' Get the index of this new column, -1 because it's a sub item
			iIndex = v_oListView.Columns.Count - 1
			
			' Not sorted yet
			ListViewHelper.SetSortedProperty(v_oListView, False)
			
			' Add the items
            For lLoop1 As Integer = 1 To v_oListView.Items.Count
                If v_iSourceColumn = 0 Then

                    If (IsDate(v_oListView.Items.Item(lLoop1 - 1).Text.Trim)) Then
                        sDate = CDate(v_oListView.Items.Item(lLoop1 - 1).Text).ToString("yyyyMMddHHMMss")
                    Else
                        sDate = v_oListView.Items.Item(lLoop1 - 1).Text.Trim
                    End If
                Else
                    If Information.IsDate(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text) Then
                        Dim TempDate As Date
                        sDate = IIf(DateTime.TryParse(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text, TempDate), TempDate.ToString("yyyyMMddHHMMss"), ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text)
                    Else
                        sDate = ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text
                    End If
                End If
                ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), iIndex).Text = sDate

            Next lLoop1
			
			' Sort now
			ListViewHelper.SetSortOrderProperty(v_oListView, v_iDirection)
			
			' Set the sort key
			ListViewHelper.SetSortKeyProperty(v_oListView, iIndex)
			
			ListViewHelper.SetSortedProperty(v_oListView, True)
			
			' Remove the column now
			'KB PN 4360
			'Removing the column seems to prevent the sort from working correctly
			'so leave it and it we sort again we will check for it before trying to add it
			' v_oListView.ColumnHeaders.Remove iIndex% + 1
			
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
			'PSL Issue 6479 10/01/2003 should be width 0
			v_oListView.Columns.Add(ACLVTag, "Internal Sort", CInt(VB6.TwipsToPixelsX(0)))
			
			' Get the index of this new column, -1 because it's a sub item
			iIndex = v_oListView.Columns.Count - 1
			
			' Not sorted yet
			ListViewHelper.SetSortedProperty(v_oListView, False)
			
			' Add the items
			For lLoop1 As Integer = 1 To v_oListView.Items.Count
				
				sValue = ""
				' Alix - Check if item or sum-item
				If v_iSourceColumn = 0 Then
					Dim dbNumericTemp As Double
					If Double.TryParse(v_oListView.Items.Item(lLoop1 - 1).Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
						' Alix - We are looking at a numeric value
						cValue = CDec(v_oListView.Items.Item(lLoop1 - 1).Text) + 1000000000
					Else
						' Alix - This is not a numeric value, we do a normal sort
						' This can happen when a field is a lookup field. It is defined as
						' a integer (the ID of the lookup), but what is displayed is
						' actually the description of the item.
						sValue = v_oListView.Items.Item(lLoop1 - 1).Text
					End If
				Else
					Dim dbNumericTemp2 As Double
					If Double.TryParse(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
						cValue = CDec(ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text) + 1000000000
					Else
						sValue = ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text
					End If
				End If
				If sValue.Trim() = "" Then
					' Alix - If it WAS a numeric value, we format it for the sorting to work
					sValue = StringsHelper.Format(cValue, "0000000000.00")
				End If
				ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), iIndex).Text = sValue
                cValue = 0
			Next lLoop1
			
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
		Const ACLVTag As String = "SORT_VALUE_HIDDEN"
		
		Dim cValue As Decimal
		Dim sValue As String = ""
		Dim iIndex As Integer
		Dim bNegative As Boolean
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Add the column
			'PSL 02/10/2003 Should be zero width as well
			v_oListView.Columns.Add(ACLVTag, "Internal Sort", CInt(VB6.TwipsToPixelsX(0)))
			
			' Get the index of this new column, -1 because it's a sub item
			iIndex = v_oListView.Columns.Count - 1
			
			' Not sorted yet
			ListViewHelper.SetSortedProperty(v_oListView, False)
			
			' Add the items
			For lLoop1 As Integer = 1 To v_oListView.Items.Count
				
				If v_iSourceColumn = 0 Then
					sValue = StringsHelper.Format(v_oListView.Items.Item(lLoop1 - 1).Text, "#,##0.00")
				Else
					
					'PSL 05/08/2003 Issue 5830
					'Changed various bits, so negative numbers, and various currency formats work
					If ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.IndexOf(" "c) + 1 Then
						sValue = ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.Substring(0, ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text.IndexOf(" "c) + 1)
					Else
						sValue = ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text
					End If
					sValue.TrimEnd()
					
					If sValue.StartsWith("-") Then
						sValue = Mid(sValue, 2, sValue.Length - 1)
						bNegative = True
					Else
						bNegative = False
					End If
					If sValue.Substring(0, 1) < "0" Or sValue.Substring(0, 1) > "9" Then
						sValue = sValue.Substring(sValue.Length - (sValue.Length - 1))
					End If
					If bNegative Then
						cValue = 1000000000 - CDec(sValue)
					Else
						cValue = CDec(sValue) + 1000000000
					End If
					sValue = StringsHelper.Format(cValue, "0000000000.00")
					
				End If
				ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), iIndex).Text = sValue
				
			Next lLoop1
			
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
	
	'DJM 21/01/2004 : Add function to just do a standard sort.
	'RAM20040123 : Removed the ComctlLib.ListSortOrderConstants
	'               Since, if the component have only VB6 Common Control,
	'               this will fail, since ComctlLib is VB5 Common Control
	Public Function ListViewSort(ByVal v_oListView As ListView, ByVal v_iSourceColumn As Integer, ByVal v_iDirection As Integer) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Set table to not sorted
			ListViewHelper.SetSortedProperty(v_oListView, False)
			
			'Set the sort order
			ListViewHelper.SetSortOrderProperty(v_oListView, v_iDirection)
			
			'Set the sort key
			ListViewHelper.SetSortKeyProperty(v_oListView, v_iSourceColumn)
			
			'Sort table
			ListViewHelper.SetSortedProperty(v_oListView, True)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ListViewSort Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ListViewSort", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name       : ListViewSortByCurrencyValue
	' Description: Sorts the list view based on the Currency column passed, and
	'              the order given.
	' Author     : Ram Chandrabose
	' Created on : 2004/01/23
	' Note       : This function will ignore the -ive sign. i.e both -100 and 100 are
	'               ordered together
	' ***************************************************************** '
	Public Function ListViewSortByCurrencyValue(ByVal v_oListView As ListView, ByVal v_iSourceColumn As Integer, ByVal v_iDirection As Integer) As Integer
		Dim result As Integer = 0
		Dim cValue As Decimal
		Dim sValue As String = ""
		Dim iIndex As Integer
		Dim lLoopCount As Integer
		Const ACLVTag As String = "SORT_VALUE_HIDDEN"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Add a new temporary column
			v_oListView.Columns.Add(ACLVTag, "Internal Sort", CInt(VB6.TwipsToPixelsX(0)))
			
			' Get the index of this new column, -1 because it's a sub item
			iIndex = v_oListView.Columns.Count - 1
			
			' Not sorted yet
			ListViewHelper.SetSortedProperty(v_oListView, False)
			
			' Add the items
			lLoopCount = v_oListView.Items.Count
			
			For lLoop1 As Integer = 1 To lLoopCount
				If v_iSourceColumn = 0 Then
					' Get the value from the listview
					sValue = v_oListView.Items.Item(lLoop1 - 1).Text
				Else
					' Get the value from the listview
					sValue = ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), v_iSourceColumn).Text
				End If
				
				' Convert to currency value
				cValue = gPMFunctions.ConvertCurrencyStringToValue(sValue)
				
				'Note : The value is ordered on the basis, that the no of digits is less than 15
				sValue = StringsHelper.Format(cValue, "000000000000000.00")
				
				' set the new value to the new colomn
				ListViewHelper.GetListViewSubItem(v_oListView.Items.Item(lLoop1 - 1), iIndex).Text = sValue
			Next lLoop1
			
			' Sort now
			ListViewHelper.SetSortOrderProperty(v_oListView, v_iDirection)
			
			' Set the sort key
			ListViewHelper.SetSortKeyProperty(v_oListView, iIndex)
			
			ListViewHelper.SetSortedProperty(v_oListView, True)
			
			' Remove the column now
			v_oListView.Columns.RemoveAt(iIndex)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ListViewSortByCurrencyValue Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ListViewSortByCurrencyValue", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Public Sub SetListViewLedger(ByRef lvwList As Object, ByRef iSizingType As Integer)
		Dim BF As Object
		' ---------------------------------------------------------------------------
		' PROCEDURE NAME: SetListViewLedger
		' PURPOSE: Sets a list view with alternating row colours for easier readability
		'          WARNING:
		'          The parent form must have a PictureBox control called "RowPicture"
		'
		'          The SizingType parameter is:
		'               1 - size for text only in list
		'               2 - size for checkbox in list
		'               3 - size for icon in list
		'
		' AUTHOR: Danny Davis
		' DATE: 30 September 2004, 16:35:00
		' CHANGES:
		' ---------------------------------------------------------------------------
		
		
		Try
		
		
		Dim iBarHeight As Integer '/* height of 1 line in the listview
		Dim lBarWidth As Integer '/* width of listview
		Dim diff As Integer '/* used in calculations of row height
		Dim twipsy As Integer '/* variable holding Screen.TwipsPerPicture1elY
		Dim nSizingType As Integer
		Dim pic As PictureBox
		Dim lBar1Color As Color
		Dim lBar2Color, hdc As Integer
		
		iBarHeight = 0
		lBarWidth = 0
		diff = 0
		
		lBar1Color = Color.White
		lBar2Color = &H80000018 'Tooltip colour
		
		twipsy = CInt(VB6.TwipsPerPixelY())

		pic = lvwList.Parent.RowPicture
		

		If lvwList.View = View.Details Then
			
			'/* set up the listview properties
			With lvwList

                '.Picture = Nothing '/* clear picture
                .BackgroundImage = Nothing '/* clear picture

				.Refresh()

				.Visible = 1

                '.PictureAlignment = 5 'lvwTile = 5
                .Alignment = 5 'lvwTile = 5

				lBarWidth = CInt(.Width * 2)
			End With ' lvwList
			
			'/* set up the picture box properties
			With pic

                'Modified by Deepak Sharma on 4/20/2010 4:34:38 PM refer developer guide no. 31 (No Solutions)
                '.AutoRedraw = False '/* clear/reset picture
                'ReflectionHelper.SetMember(pic, "AutoRedraw", False)
                'TODOLIST
                .AutoSize = False '/* clear/reset picture
                .Image = Nothing
				.BackColor = Color.White
				.Height = VB6.TwipsToPixelsY(1)

                'Modified by Deepak Sharma on 4/20/2010 4:34:38 PM refer developer guide no. 31 (No Solutions)
                '.AutoRedraw = True '/* assure image draws
                'ReflectionHelper.SetMember(pic, "AutoRedraw", True)
                'TODOLIST
                .AutoSize = True '/* assure image draws
				.BorderStyle = FormBorderStyle.None '/* other attributes


                'Modified by Deepak Sharma on 4/20/2010 4:34:38 PM refer developer guide no. 32 (No Solutions)
                '.ScaleMode = vbTwips

				.Top = lvwList.Parent.Top - VB6.TwipsToPixelsY(10000) '/* move it way off screen
				.Width = VB6.TwipsToPixelsX(Screen.PrimaryScreen.Bounds.Width)
				.Visible = False

                'TODOLIST
                '.Font = VB6.FontChangeName(.Font, lvwList.Font) '/* assure pic font matched listview font
                .Font = VB6.FontChangeName(.Font, lvwList.Font.Name) '/* assure pic font matched listview font
				
				'/* match picture box font properties
				'/* with those of listview
				With .Font

                    pic.Font = VB6.FontChangeBold(pic.Font, lvwList.Font.Bold)

                    'TODOLIST
                    'pic.Font = VB6.FontChangeGdiCharSet(pic.Font, lvwList.Font.Charset)
                    pic.Font = VB6.FontChangeGdiCharSet(pic.Font, lvwList.Font.GdiCharSet)

					pic.Font = VB6.FontChangeItalic(pic.Font, lvwList.Font.Italic)

					pic.Font = VB6.FontChangeName(pic.Font, lvwList.Font.Name)

                    'TODOLIST
                    'pic.Font = VB6.FontChangeStrikeout(pic.Font, lvwList.Font.Strikethrough)
                    pic.Font = VB6.FontChangeStrikeout(pic.Font, lvwList.Font.Strikeout)

					pic.Font = VB6.FontChangeUnderline(pic.Font, lvwList.Font.Underline)


                                        'Modified by Deepak Sharma on 4/20/2010 5:08:30 PM refer developer guide no. 34(No Solutions)
					'.Weight = lvwList.Font.Weight

					pic.Font = VB6.FontChangeSize(pic.Font, lvwList.Font.Size)
				End With 'pic.Font
				
				'/* here we calculate the height of each
				'/* bar in the listview. Several things
				'/*  can affect this height - the use
				'/* of item icons, the size of those icons,
				'/* the use of checkboxes and so on through
				'/* all the permutations.
				'/*
				'/* Shown here is code sufficient to calculate
				'/* this height based on three combinations of
				'/*  data, state icons, and imagelist icons:
				'/*
				'/* 1. text only
				'/* 2. text with checkboxes
				'/* 3. text with icons
				
				'/* used by all sizing routines

                'Modified by Deepak Sharma on 4/20/2010 5:19:26 PM refer developer guide no. 127(Guide)
				'iBarHeight = CInt(.TextHeight("W"))
				
				Select Case iSizingType
					Case 1
						'/* 1. text only
						iBarHeight += twipsy
						
					Case 2
						'/* 2. text with checkboxes: add to textheight the
						'/*    difference between 18 Pixels and iBarHeight
						'/*    all calculated initially in Pixels,
						'/*    then converted to twips
						If (iBarHeight \ twipsy) > 18 Then
							iBarHeight += twipsy
						Else
							diff = 18 - (iBarHeight \ twipsy)
							iBarHeight = iBarHeight + (diff * twipsy) + (twipsy * 1)
						End If
						
					Case 3
						'/* 3. text with icons: add to textheight the
						'/*    difference between textheight and image
						'/*    height, all calculated initially in Pixels,
						'/*    then converted to twips. Handles 16x16 icons
						diff = 16 - (iBarHeight \ twipsy)
						iBarHeight = iBarHeight + (diff * twipsy) + (twipsy * 1)
						
				End Select
				
				'/* since we need two-tone bars, the
				'/* picturebox needs to be twice as high
				.Height = VB6.TwipsToPixelsY(iBarHeight * 2)
				.Width = VB6.TwipsToPixelsX(lBarWidth)
				
				'/* paint the two bars of color and refresh
				'/* Note: The line method does not support
				'/* With/End With blocks



                'Modified by Deepak Sharma on 4/20/2010 4:53:38 PM refer developer guide no. 33(No Solutions)
                'pic.Line(CSng((0, 0) - (lBarWidth, iBarHeight)), ColorTranslator.ToOle(lBar1Color), CSng(BF))



                'Modified by Deepak Sharma on 4/20/2010 4:53:38 PM refer developer guide no. 33(No Solutions)
                'pic.Line(CSng((0, iBarHeight) - (lBarWidth, iBarHeight * 2)), lBar2Color, CSng(BF))

                .SizeMode = PictureBoxSizeMode.AutoSize
				.Refresh()
				
			End With 'pic
			
			'/* set the lvwList picture to the
			'/* pic image
			

			lvwList.Refresh()


            'TODOLIST
            'lvwList.Picture = pic.Image
            lvwList.BackgroundImage = pic.Image
			
		Else
			

			lvwList.Picture = Nothing
			
		End If 'lvwList.View = lvwReport
		
		
		
		'----------------------------------------------------------------------------------------
		'Only for Debugging, the code will never execute this line
		'----------------------------------------------------------------------------------------
		  
		
		Catch ex As Exception
		Select Case Information.Err().Number
			Case Else
				' Log Error.
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="SetListViewLedger", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)
				
		End Select
		
		Finally
		
		
		
		End Try
    End Sub

    Private sortColumn As Integer = -1
    Public Sub SortListView(ByRef lvControl As ListView, ByVal eventArgs As ColumnClickEventArgs)
        Try
            Dim iEmptyColCtr As Integer = 0
            With lvControl

                For Each lvItem As ListViewItem In lvControl.Items
                    'If lvItem.SubItems.Count <= lvControl.Columns.Count Then
                    '    Exit Sub

                    Try
                        If lvItem.SubItems(eventArgs.Column).Text.Trim.Equals(String.Empty) Then
                            iEmptyColCtr += 1
                        End If
                    Catch ex As ArgumentOutOfRangeException
                        iEmptyColCtr += 1
                        Continue For
                    End Try
                Next

                If iEmptyColCtr = lvControl.Items.Count Then
                    Exit Sub
                End If

                If eventArgs.Column <> sortColumn Then
                    sortColumn = eventArgs.Column
                    .Sorting = SortOrder.Ascending
                Else
                    If .Sorting = SortOrder.Ascending Then
                        .Sorting = SortOrder.Descending
                    Else
                        .Sorting = SortOrder.Ascending
                    End If

                End If
                .Sort()
                .ListViewItemSorter = New ListViewItemComparer(eventArgs.Column, .Sorting)
            End With
        Catch ex As Exception
            gPMFunctions.LogMessageToFile(sUserName:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SortListView Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyVersionArrayIndex", excep:=ex)
        End Try
    End Sub
End Module
