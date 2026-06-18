Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
Imports VB6 = Microsoft.VisualBasic.Compatibility.VB6
Public Module ListView6Func
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
	'           30/07/03    PWF  Removed unused declarations
	'                            BatchStart + BatchEnd notes: Don't use
	'                            Added ListViewMoveItem
	'           20/11/03    PWF  General overhaul of functions
	' *************************************************************************
	
	
	' Prototypes
	Private Declare Function LockWindowUpdate Lib "user32" (ByVal hwndLock As Integer) As Integer
	
	' Constants
	
	' Enumerators
	Public Enum SIRListViewMoveItemEnum
		sirLVMIMoveNext
		sirLVMIMovePrevious
		sirLVMIMoveFirst
		sirLVMIMoveLast
		sirLVMIMoveToIndex
		sirLVMIMoveBySteps
	End Enum
	
	
	
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
	Public Function ListViewAutoSize(ByVal lvwList As ListView, Optional ByVal bSizeHeaders As Boolean = True, Optional ByVal bResizeLastColumn As Boolean = True, Optional ByVal oParentForm As Form = Nothing, Optional ByVal lMaxWidth As Integer = 0) As Integer
		Dim result As Integer = 0
        Dim ACClass As Object = Nothing
		
		Dim lColumns, lWidth As Integer
        Dim vArray() As Object
		
        'Dim oFind As Form
        Dim oFind As Object
		
        Const CELL_PADDING As Single = 19 '240
        Const ICON_PADDING As Single = 19 '240
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' If it wasn't passed get the parent form
			If oParentForm Is Nothing Then
				' It may be above a usercontrol somewhere so hunt upwards
				oFind = lvwList.Parent
				' If the parent isn't a form get the parent's parent
				Do While Not (TypeOf oFind Is Form)

					oFind = oFind.FindForm()
				Loop 
				oParentForm = oFind
			End If
			
			' Get column count
			lColumns = lvwList.Columns.Count
			
			' Make an array to store the widths in
			ReDim vArray(lColumns - 1)
			
			' Initialise the array
			If bSizeHeaders Then
				' With the header columns
				For	Each oHeader As ColumnHeader In lvwList.Columns

                    ' developer guide no. 147
                    
                    vArray(oHeader.Index) = oParentForm.CreateGraphics().MeasureString(oHeader.Text, oParentForm.Font).Width 
                    
				Next oHeader
			Else
				' Set defaults
				For lCount As Integer = 1 To lColumns

                    vArray(lCount) = -1
				Next lCount
			End If
			
			' Go across each header and find the biggest item
            For Each oListItem As ListViewItem In lvwList.Items
                ' Do the first column
                lWidth = CInt(oParentForm.CreateGraphics().MeasureString(oListItem.Text, oParentForm.Font).Width)

                ' developer guide no. 147
                'If lWidth > CDbl(vArray(1)) Then
                If lWidth > CDbl(vArray(0)) Then

                    
                    vArray(0) = lWidth
                End If

                ' And the sub columns
                ' developer guide no. 147
                'For lCount As Integer = 2 To lColumns
                For lCount As Integer = 1 To lColumns - 1
                    lWidth = CInt(oParentForm.CreateGraphics().MeasureString(ListViewHelper.GetListViewSubItem(oListItem, lCount).Text, oParentForm.Font).Width)

                    If lWidth > CDbl(vArray(lCount)) Then

                        vArray(lCount) = lWidth
                    End If
                Next lCount
            Next oListItem

            ' Add a little extra for the icon !


            ' developer guide no. 147
            
            vArray(0) = CDbl(vArray(0)) + ICON_PADDING

            ' Now set the column header widths
            For Each oHeader As ColumnHeader In lvwList.Columns
                If (oHeader.Index + 1 <> lColumns) Or bResizeLastColumn Then

                    ' developer guide no. 147
                    'lWidth = CInt(CDbl(vArray(oHeader.Index + 1)) + CELL_PADDING)
                    lWidth = CInt(CDbl(vArray(oHeader.Index)) + CELL_PADDING)

                    ' Check for max width
                    If (lWidth < lMaxWidth) Or (lMaxWidth = 0) Then
                        oHeader.Width = CInt(lWidth)
                    Else
                        oHeader.Width = CInt(lMaxWidth)
                    End If
                End If
            Next oHeader

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
	' ***************************************************************** '
	Public Function ListViewSortByDate(ByVal v_oListView As ListView, ByVal v_iSourceColumn As Integer, ByVal v_iDirection As SortOrder, Optional ByVal v_bMarkSortedColumn As Boolean = False) As Integer
		Dim result As Integer = 0
        Dim ACClass As Object = Nothing
		
		Dim lSortColumn As Integer
		
		Const ACLVTag As String = "SORT_DATE_HIDDEN"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Add a dummy sort column and get the index of this new column
			' -1 because it's a sub item
			v_oListView.Columns.Add(ACLVTag, ACLVTag, CInt(VB6.TwipsToPixelsX(0)))
			lSortColumn = v_oListView.Columns.Count - 1
			
			' Not sorted yet
			ListViewHelper.SetSortedProperty(v_oListView, False)
			
			' Add the items
			For	Each oListItem As ListViewItem In v_oListView.Items
				' Process column 0 from the text property else use subitems
				If v_iSourceColumn Then
					Dim TempDate As Date
					ListViewHelper.GetListViewSubItem(oListItem, lSortColumn).Text = IIf(DateTime.TryParse(ListViewHelper.GetListViewSubItem(oListItem, v_iSourceColumn).Text, TempDate), TempDate.ToString("yyyyMMddHHMMss"), ListViewHelper.GetListViewSubItem(oListItem, v_iSourceColumn).Text)
                Else
                    If IsDate(oListItem.Text) Then
                        ListViewHelper.GetListViewSubItem(oListItem, lSortColumn).Text = CDate(oListItem.Text).ToString("yyyyMMddHHMMss")
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, lSortColumn).Text = (oListItem.Text).ToString
                    End If
                    End If
			Next oListItem
			
			' Set sort column and direction and sort
			ListViewHelper.SetSortOrderProperty(v_oListView, v_iDirection)
			ListViewHelper.SetSortKeyProperty(v_oListView, lSortColumn)
			ListViewHelper.SetSortedProperty(v_oListView, True)
			
			' Remove the column now
			v_oListView.Columns.RemoveAt(lSortColumn)
			
			' Set to original for asc/desc analysis?
			If v_bMarkSortedColumn Then
				' Note: We must remove the sorted flag first of this will botch everything!
				ListViewHelper.SetSortedProperty(v_oListView, False)
				ListViewHelper.SetSortKeyProperty(v_oListView, v_iSourceColumn)
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ListViewSortByDate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ListViewSortByDate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: ListViewSortByCheck
	'
	' Description: Sorts the list view based on the check box.
	'
	' Created: PW311002
	'
	' ***************************************************************** '
	Public Function ListViewSortByCheck(ByVal v_oListView As ListView, ByVal v_iDirection As SortOrder) As Integer
		Dim result As Integer = 0
        Dim ACClass As Object = Nothing
		
		Dim lSortColumn As Integer
		
		Const ACLVTag As String = "SORT_VALUE_HIDDEN"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Add a dummy sort column and get the index of this new column
			' -1 because it's a sub item
			v_oListView.Columns.Add(ACLVTag, ACLVTag, CInt(VB6.TwipsToPixelsX(0)))
			lSortColumn = v_oListView.Columns.Count - 1
			
			' Not sorted yet
			ListViewHelper.SetSortedProperty(v_oListView, False)
			
			' Add the items
			For	Each oListItem As ListViewItem In v_oListView.Items
				' Store value base on checked status
				If oListItem.Checked Then
					ListViewHelper.GetListViewSubItem(oListItem, lSortColumn).Text = "1"
				Else
					ListViewHelper.GetListViewSubItem(oListItem, lSortColumn).Text = "2"
				End If
			Next oListItem
			
			' Set sort column and direction and sort
			ListViewHelper.SetSortOrderProperty(v_oListView, v_iDirection)
			ListViewHelper.SetSortKeyProperty(v_oListView, lSortColumn)
			ListViewHelper.SetSortedProperty(v_oListView, True)
			
			' Remove the column now
			v_oListView.Columns.RemoveAt(lSortColumn)
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ListViewSortByCheck Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ListViewSortByCheck", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: ListViewSortByValue
	'
	' Description: Sorts the list view based on the column passed, and
	'              the order given.
	' ***************************************************************** '
	Public Function ListViewSortByValue(ByVal v_oListView As ListView, ByVal v_iSourceColumn As Integer, ByVal v_iDirection As SortOrder, Optional ByVal v_bMarkSortedColumn As Boolean = False, Optional ByVal v_bIsCurrency As Boolean = False) As Integer
		Dim result As Integer = 0
        Dim ACClass As Object = Nothing
		
		Dim lSortColumn As Integer
		Dim sValue As String = ""
		Dim dValue As Double
		
		Const ACLVTag As String = "SORT_VALUE_HIDDEN"
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Add a dummy sort column and get the index of this new column
			' -1 because it's a sub item
			v_oListView.Columns.Add(ACLVTag, ACLVTag, CInt(VB6.TwipsToPixelsX(0)))
			lSortColumn = v_oListView.Columns.Count - 1
			
			' Not sorted yet
			ListViewHelper.SetSortedProperty(v_oListView, False)
			
			' Add the items
			For	Each oListItem As ListViewItem In v_oListView.Items
				' Process column 0 from the text property else use subitems
				If v_iSourceColumn Then
					sValue = ListViewHelper.GetListViewSubItem(oListItem, v_iSourceColumn).Text
				Else
					sValue = oListItem.Text
				End If
				
				'AK 161101 - try to extract the value otherwise (for right aligned columns)
				Dim dbNumericTemp As Double
				If Double.TryParse(sValue, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    dValue = ToSafeDouble(sValue) + 100000000000.0#
				Else
					' Peter Finney 19/11/2003 - If currency do a bit of hacking
					If v_bIsCurrency Then
						' Remove know problem formatting
						sValue = sValue.Replace(",", "")
                        sValue = sValue.Replace("£", "")
                        sValue = sValue.Replace("$", "")
                        sValue = sValue.Replace("€", "")
                        ' Also remove percentages, option would be better..v_bIsFormattedNumber
						sValue = sValue.Replace("%", "")
						
						' Strip value
						dValue = Conversion.Val(sValue) + 100000000000#
					Else
						dValue = 0
					End If
				End If
				
				' Extend our value as the list view will only sort as strings
				' Extended further to cope with currency (14.) and doubles (.12 for now)
				ListViewHelper.GetListViewSubItem(oListItem, lSortColumn).Text = StringsHelper.Format(dValue, "000000000000.0000")
			Next oListItem
			
			' Set sort column and direction and sort
			ListViewHelper.SetSortOrderProperty(v_oListView, v_iDirection)
			ListViewHelper.SetSortKeyProperty(v_oListView, lSortColumn)
			ListViewHelper.SetSortedProperty(v_oListView, True)
			
			' Remove the column now
			v_oListView.Columns.RemoveAt(lSortColumn)
			
			' Set to original for asc/desc analysis?
			If v_bMarkSortedColumn Then
				' Note: We must remove the sorted flag first of this will botch everything!
				ListViewHelper.SetSortedProperty(v_oListView, False)
				ListViewHelper.SetSortKeyProperty(v_oListView, v_iSourceColumn)
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ListViewSortByValue Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ListViewSortByValue", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	' ***************************************************************** '
	' Name: ListViewSortByStringValue
	'
	' Description: Sorts the list view based on the column passed, and
	'              the order given.
	' ***************************************************************** '
	Public Function ListViewSortByStringValue(ByVal v_oListView As ListView, ByVal v_iSourceColumn As Integer, ByVal v_iDirection As SortOrder, Optional ByVal v_bMarkSortedColumn As Boolean = False) As Integer
		
		' Used to take a numeric value from the start of the column.
		' Now redundent, pass to ListViewSortByValue which will do
		' this automatically where the column is now a direct numeric.
		Return ListViewSortByValue(v_oListView, v_iSourceColumn, v_iDirection, v_bMarkSortedColumn)
		
	End Function
	
	
	' ***************************************************************** '
	' Name: ListViewBatchStart
	'
	' Description: Use when you start a batch of ListItem.Add's.
	'              This will disable the listview from being updated.
	'
	' History: 24/08/1999 CTAF - Created.
	'
	' Notes: Alternatively use....ListView.Enabled = False
	' ***************************************************************** '
	Public Function ListViewBatchStart(ByRef lvwList As ListView) As Integer
		Dim result As Integer = 0
        Dim ACClass As Object = Nothing
		
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
	' Name: ListViewBatchEnd
	'
	' Description: As above, but called when ending the batch.
	'
	' History: 24/08/1999 CTAF - Created.
	'
	' Notes: Alternatively use....ListView.Enabled = True
	' ***************************************************************** '
	Public Function ListViewBatchEnd() As Integer
		Dim result As Integer = 0
        Dim ACClass As Object = Nothing
		
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
	
	
	' ***************************************************************** '
	' Move a ListItem
	'
	' History:
	'   30/07/2003 Peter Finney - Created.
	'
	' ***************************************************************** '
	Public Function ListViewMoveItem(ByVal ListView As ListView, ByVal ListItem As ListViewItem, ByVal Move As SIRListViewMoveItemEnum, Optional ByVal IndexOrSteps As Integer = 0, Optional ByRef NewListItem As ListViewItem = Nothing) As Integer
		Dim result As Integer = 0
        Dim ACClass As Object = Nothing
		
		Dim lNewIndex, lOldIndex As Integer
		Dim oNewItem As ListViewItem
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		Try 
			
			' Remove the existing item from the list
			' Note: This must be done first as we cannot have duplicate keys!!
			lOldIndex = ListItem.Index + 1
			ListView.Items.RemoveAt(lOldIndex - 1)
			
			' Work out our new location
			Select Case Move
				Case SIRListViewMoveItemEnum.sirLVMIMoveNext
					lNewIndex = lOldIndex + 1
				Case SIRListViewMoveItemEnum.sirLVMIMovePrevious
					lNewIndex = lOldIndex - 1
				Case SIRListViewMoveItemEnum.sirLVMIMoveFirst
					lNewIndex = 1
				Case SIRListViewMoveItemEnum.sirLVMIMoveLast
					lNewIndex = ListView.Items.Count + 1
				Case SIRListViewMoveItemEnum.sirLVMIMoveToIndex
					lNewIndex = IndexOrSteps
				Case SIRListViewMoveItemEnum.sirLVMIMoveBySteps
					lNewIndex = lOldIndex + IndexOrSteps
				Case Else
					' Invalid, put it back in the same place
					lNewIndex = lOldIndex
			End Select
			
			' Check bounds
			If lNewIndex < 1 Then
				lNewIndex = 1
			End If
			If lNewIndex > ListView.Items.Count + 1 Then
				lNewIndex = ListView.Items.Count + 1
			End If
			
			' Add the item back in


            'Modified by Deepak Sharma on 4/20/2010 5:17:31 PM refer developer guide no. 126(Guide)
            'oNewItem = ListView.Items.Insert(lNewIndex - 1, ListItem.Name, ListItem.Text, ListItem.Icon)
            oNewItem = ListView.Items.Insert(lNewIndex - 1, ListItem.Name, ListItem.Text, "")
			
			' Add all sum items
			For lCount As Integer = 1 To ListItem.SubItems.Count
                ListViewHelper.GetListViewSubItem(oNewItem, lCount - 1).Text = ListItem.SubItems.Item(lCount - 1).Text
			Next lCount
			
			' Return the new item
			NewListItem = oNewItem
			oNewItem = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ListViewMoveItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ListViewMoveItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return gPMConstants.PMEReturnCode.PMError
			
		End Try
	End Function
End Module
