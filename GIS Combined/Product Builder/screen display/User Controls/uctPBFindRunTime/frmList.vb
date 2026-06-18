Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmList
	Inherits System.Windows.Forms.Form
	
	Private m_vDataArray( ,  ) As Object
	
    'developer guide no.33
    Public Property DataArray() As Object(,)
        Get
            Return VB6.CopyArray(m_vDataArray)
        End Get
        Set(ByVal Value As Object(,))  'developer guide no.33
            m_vDataArray = Value
        End Set
    End Property
	
	' ***************************************************************** '
	' Name: AutosizeListView
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : Date : Process ID
	' ***************************************************************** '
	Public Function AutosizeListView() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "AutosizeListView"
		
		Const CELL_PADDING As Single = 240
		Const ICON_PADDING As Single = 240
		
		Dim lReturn, lColumns, lWidth As Integer
        Dim vArray() As Object
		Dim lMaxWidth As Integer
		
		Dim oFind As Object
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		' Get column count
		lColumns = lvwList.Columns.Count
		
		' Make an array to store the widths in
		ReDim vArray(lColumns - 1)
		
		' ensure that the minimum width is at least that of the header
		For	Each oHeader As ColumnHeader In lvwList.Columns

            vArray(oHeader.Index + 1) = VB6.PixelsToTwipsX(Me.CreateGraphics().MeasureString(oHeader.Text, Me.Font).Width)
		Next oHeader
		
		' if any items require a larger width than that
		' of the header use the width of the item instead
		For	Each oListItem As ListViewItem In lvwList.Items
			
			' Do the first column
			lWidth = CInt(VB6.PixelsToTwipsX(Me.CreateGraphics().MeasureString(oListItem.Text, Me.Font).Width))

            If lWidth > CDbl(vArray(1)) Then

                vArray(1) = lWidth
            End If

            ' And the sub columns
            For lCount As Integer = 2 To lColumns
                lWidth = CInt(VB6.PixelsToTwipsX(Me.CreateGraphics().MeasureString(ListViewHelper.GetListViewSubItem(oListItem, lCount - 1).Text, Me.Font).Width))

                If lWidth > CDbl(vArray(lCount)) Then

                    vArray(lCount) = lWidth
                End If
            Next lCount

        Next oListItem

        ' Add a little extra for the icon if there is one


        vArray(1) = CDbl(vArray(1)) + ICON_PADDING

        ' Now set the column header widths
        For Each oHeader As ColumnHeader In lvwList.Columns

            lWidth = CInt(CDbl(vArray(oHeader.Index + 1)) + CELL_PADDING)

            oHeader.Width = CInt(VB6.TwipsToPixelsX(lWidth))

        Next oHeader
		
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		
'		Return result
'		Resume 
'		Return result
		End Try
		Return result
	End Function
	
	
	Private Sub btnCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles btnCancel.Click
		
		m_bFoundValues = False
		
		Me.Close()
		
	End Sub
	
	Private Sub btnOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles btnOK.Click
		Try 
			
			If lvwList.FocusedItem Is Nothing Then Exit Sub
			
			'transfer chosen item to array
			For i As Integer = 0 To m_vDataArray.GetUpperBound(ACControl - 1)
				With lvwList.FocusedItem
					If i = 0 Then
						m_vDataArray(kMappingFoundValue, i) = .Text
					Else
						m_vDataArray(kMappingFoundValue, i) = ListViewHelper.GetListViewSubItem(lvwList.FocusedItem, i).Text
					End If
				End With
			Next i
			
			m_bFoundValues = True
			
			Me.Close()
		
		Catch excep As System.Exception
			
			
			TempFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process OK button", vApp:=ACAPP, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			
		End Try
		
	End Sub
	

	Private Sub frmList_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		m_bFoundValues = False
        If lvwList.Items.Count > 0 Then
            Dim iPtr As IntPtr
            lvwList.Items(0).Focused = True
            lvwList.Items(0).Selected = True
            iPtr = lvwList.Handle
            lvwList.Select()
            ' iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            'Threading.Thread.Sleep(10000)
            'iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        End If
	End Sub
	
	'Start (Prakash Varghese) - (Tech Spec - TRAC 3867 Wording Code Display.docx) - (6.1.2)
	'Added sorting facility based on column headers
	Private Sub lvwList_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwList.ColumnClick
        'Dim ColumnHeader As ColumnHeader = lvwList.Columns(eventArgs.Column)
        '' Column click event for the search details

        'Try 

        '	With lvwList
        '		ListViewHelper.SetSortedProperty(lvwList, False)
        '		' If current sort column header is pressed.
        '		If ColumnHeader.Index + 1 - 1 = ListViewHelper.GetSortKeyProperty(lvwList) Then
        '			' Set sort order opposite of current direction.
        '			ListViewHelper.SetSortOrderProperty(lvwList, IIf(ListViewHelper.GetSortOrderProperty(lvwList) = SortOrder.Ascending, SortOrder.Descending, SortOrder.Ascending))
        '		Else
        '			ListViewHelper.SetSortOrderProperty(lvwList, SortOrder.Ascending)
        '			ListViewHelper.SetSortKeyProperty(lvwList, ColumnHeader.Index + 1 - 1)
        '		End If
        '		ListViewHelper.SetSortedProperty(lvwList, True)
        '	End With

        'Catch excep As System.Exception
        '          ' Log Error.
        '          TempFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACAPP, vClass:=ACClass, vMethod:="lvwList_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '          Exit Sub
        'End Try

        ListViewFunc.SortListView(lvwList, eventArgs)

	End Sub
	'End (Prakash Varghese) - (Tech Spec - TRAC 3867 Wording Code Display.docx) - (6.1.2)
	
	Private Sub lvwList_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwList.DoubleClick
        btnOK_Click(btnOK, New EventArgs())
    End Sub
	
	Private Sub lvwList_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles lvwList.KeyPress
		Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
		
		'if enter is pressed
		If KeyAscii = 13 Then
			btnOK_Click(btnOK, New EventArgs())
		End If
		
		If KeyAscii = 0 Then
			eventArgs.Handled = True
		End If
		eventArgs.KeyChar = Convert.ToChar(KeyAscii)
	End Sub
	
	' ***************************************************************** '
	' Name: SizeColumn
	'
	' Parameters: n/a
	'
	' Description:
	'
	' History:
	'           Created : MEvans : Date : Process ID
	' ***************************************************************** '
	Public Function SizeColumn(ByVal v_lColumn As Integer, ByVal v_sWidth As String) As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "SizeColumn"
		
		Const CELL_PADDING As Single = 240
		Const ICON_PADDING As Single = 240
		
		
		Dim lReturn, lWidth, lMaxWidth As Integer
		
		
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		If v_sWidth = "AUTO" Then
			
			' ensure that the minimum width is at least that of the header
			lMaxWidth = CInt(VB6.PixelsToTwipsX(Me.CreateGraphics().MeasureString(lvwList.Columns.Item(v_lColumn - 1).Text, Me.Font).Width))
			
			' if any items require a larger width than that
			' of the header use the width of the item instead
			For	Each oListItem As ListViewItem In lvwList.Items
				
				If v_lColumn = 1 Then
					lWidth = CInt(VB6.PixelsToTwipsX(Me.CreateGraphics().MeasureString(oListItem.Text, Me.Font).Width))
					If lWidth > lMaxWidth Then
						lMaxWidth = lWidth
					End If
				Else
					lWidth = CInt(VB6.PixelsToTwipsX(Me.CreateGraphics().MeasureString(ListViewHelper.GetListViewSubItem(oListItem, v_lColumn - 1).Text, Me.Font).Width))
					If lWidth > lMaxWidth Then
						lMaxWidth = lWidth
					End If
				End If
				
			Next oListItem
			
			' Add a little extra for the icon if there is one
			If v_lColumn = 0 Then
				lMaxWidth = CInt(lMaxWidth + ICON_PADDING)
			Else
				lMaxWidth = CInt(lMaxWidth + CELL_PADDING)
			End If
			
		Else
			lMaxWidth = gPMFunctions.ToSafeLong(v_sWidth)
		End If
		
		lvwList.Columns.Item(v_lColumn - 1).Width = CInt(VB6.TwipsToPixelsX(lMaxWidth))
		
		
		Catch ex As Exception
		
		' DO Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		
'		Return result
'		Resume 
'		Return result
		End Try
		Return result
	End Function
End Class
