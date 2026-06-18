Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Friend Partial Class frmMain
	Inherits System.Windows.Forms.Form
	'******************************************************************************
	'
	' Name: frmMain
	'
	' Edit History:
	'
	' DAK081299 - Refresh list on change of Number of Records
	'******************************************************************************
	
	' Class name
	Private Const ACClass As String = "frmMain"
	
	' Return value
	Private m_lReturn As gPMConstants.PMEReturnCode
	
	' Reference to the business object
	Private m_oBusiness As Object
	
	' Message array
	Private m_vResultArray( ,  ) As Object
	
	' Reference of object manager
	Private g_oObjectManager As bObjectManager.ObjectManager
	
	Private Sub cboFilter_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboFilter.SelectedIndexChanged
		
		' Refresh the list
		cmdRefresh_Click(cmdRefresh, New EventArgs())
		
	End Sub
	
	'DAK081299
	Private Sub cboNumberOfRecords_SelectionChangeCommitted(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboNumberOfRecords.SelectionChangeCommitted
		
		' Refresh the list
		cmdRefresh_Click(cmdRefresh, New EventArgs())
		
	End Sub
	
	Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click
		
		' Confirm that the user wants to delete the items in the list
		m_lReturn = CType(MessageBox.Show("Are you sure that you wish to delete all of the displayed messages?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question), gPMConstants.PMEReturnCode)
		If m_lReturn = System.Windows.Forms.DialogResult.Yes Then
			
			' Busy mouse pointer
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
			
			' Delete the items
			m_lReturn = CType(DeleteItems(), gPMConstants.PMEReturnCode)
			
			' Back to normal mouse pointer
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
		End If
		
	End Sub
	
	' ****************************************************************************** '
	' Name: DeleteItems
	'
	' Description: Deletes the currently displayed items from the pmmessage table
	'
	' ****************************************************************************** '
	Private Function DeleteItems() As Integer
		
		Dim result As Integer = 0
		Dim lFilterType As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get the filter value
			m_lReturn = CType(GetFilterType(r_lFilterValue:=lFilterType), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Call the business
			'DAK081299
			'    m_lReturn& = m_oBusiness.DeleteByType(v_lMessageType:=lFilterType)

			m_lReturn = m_oBusiness.DeleteMessages(v_vMessageArray:=m_vResultArray)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Log Error Message
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to delete items from pmmessage.", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteItems", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
				
				Return result
			End If
			
			' Refresh the list
			m_lReturn = CType(RefreshList(), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
            result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteItemsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteItems", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		Environment.Exit(0)
		
	End Sub
	
	Private Sub cmdRefresh_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRefresh.Click
		
		' Busy mouse pointer
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
		
		' Refresh the list
		m_lReturn = CType(RefreshList(), gPMConstants.PMEReturnCode)
		
		' Back to normal mouse pointer
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
	End Sub
	
	Private Sub frmMain_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			
			With uctPMResizer1
				
				.SetControlResizeOption("cmdOK", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdCancel", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdHelp", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				
				.SetControlResizeOption("cmdRefresh", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdDelete", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				
				' CF220199 - Fixed icon
				.SetControlResizeOption("imgIcon", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				
				.SetControlResizeOption("tabMain", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("lvwMessages", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				
				.FormMinHeight = 4000
				.FormMinWidth = 8865
				
			End With
			
		End If
	End Sub
	
	Private Sub Form_Initialize_Renamed()
		
        Try

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get an instance of object manager

            g_oObjectManager = New bObjectManager.ObjectManager()

            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn = gPMConstants.PMEReturnCode.PMCancel Then
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Environment.Exit(0)
                Else
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create an instance of bObjectManager.ObjectManager", vApp:=ACApp, vClass:=ACClass, vMethod:="Form.Initialize", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Exit Sub
                End If
            End If

            ' Get an instance of the message business object
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bPMMessage.FormAdmin", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create an instance of bPMMessage.FormAdmin", vApp:=ACApp, vClass:=ACClass, vMethod:="Form.Initialize", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Exit Sub
            End If

            m_lReturn = CType(SetExtraListViewProperties(v_hWndList:=lvwMessages.Handle.ToInt32(), v_vShowRowSelect:=True), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Do nothing as this is hardly important
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise form.", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
		
	End Sub
	

	Private Sub frmMain_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		' Initialise combo box
		With cboFilter
			.Items.Clear()
			.Items.Add(FilterAll)
			.Items.Add(gPMConstants.PMDebug1Text)
			.Items.Add(gPMConstants.PMDebug2Text)
			.Items.Add(gPMConstants.PMDebug3Text)
			.Items.Add(gPMConstants.PMDebug4Text)
			.Items.Add(gPMConstants.PMErrorText)
			.Items.Add(gPMConstants.PMFatalText)
			.Items.Add(gPMConstants.PMInfoText)
			.Items.Add(gPMConstants.PMOnErrorText)
			.Items.Add(gPMConstants.PMWarningText)
			.SelectedIndex = 0
		End With
		
		With cboNumberOfRecords
			.Items.Clear()
			.Items.Add(PMRecordsAll)
			.Items.Add(PMRecords500)
			.Items.Add(PMRecords400)
			.Items.Add(PMRecords300)
			.Items.Add(PMRecords200)
			.Items.Add(PMRecords100)
			.Items.Add(PMRecords10)
			.SelectedIndex = 0
		End With
		
		' CF090299 - Check if there are some items in the list
		If lvwMessages.Items.Count > 0 Then
			lvwMessages.FocusedItem = lvwMessages.Items.Item(0)
			
			' CF230899 - Changed to use this function instead
			' Default to sort by date
            'developer guide no. 170
            m_lReturn = CType(SharedFiles.ListViewFunc.ListViewSortByDate(v_oListView:=lvwMessages, v_iSourceColumn:=ACColumnDate, v_iDirection:=((ListViewHelper.GetSortOrderProperty(lvwMessages) + 1) Mod 2)), gPMConstants.PMEReturnCode)
			
		End If
		
		uctPMResizer1.NoResizeByDefault = True
		
	End Sub
	
	' ***************************************************************** '
	' Name: GetFilterType
	'
	' Description: Gets the enum of the text in the filter combo box
	'
	'
	' ***************************************************************** '
	Private Function GetFilterType(ByRef r_lFilterValue As gPMConstants.PMELogLevel) As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			Select Case cboFilter.Text
				Case FilterAll
					r_lFilterValue = AllMessages
				Case gPMConstants.PMDebug1Text
					r_lFilterValue = gPMConstants.PMELogLevel.PMLogDebug1
				Case gPMConstants.PMDebug2Text
					r_lFilterValue = gPMConstants.PMELogLevel.PMLogDebug2
				Case gPMConstants.PMDebug3Text
					r_lFilterValue = gPMConstants.PMELogLevel.PMLogDebug3
				Case gPMConstants.PMDebug4Text
					r_lFilterValue = gPMConstants.PMELogLevel.PMLogDebug4
				Case gPMConstants.PMErrorText
					r_lFilterValue = gPMConstants.PMELogLevel.PMLogError
				Case gPMConstants.PMFatalText
					r_lFilterValue = gPMConstants.PMELogLevel.PMLogFatal
				Case gPMConstants.PMInfoText
					r_lFilterValue = gPMConstants.PMELogLevel.PMLogInfo
				Case gPMConstants.PMOnErrorText
					r_lFilterValue = gPMConstants.PMELogLevel.PMLogOnError
				Case gPMConstants.PMWarningText
					r_lFilterValue = gPMConstants.PMELogLevel.PMLogWarning
				Case Else
					result = gPMConstants.PMEReturnCode.PMFalse
			End Select
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFilterTypeFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFilterType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Function RefreshList() As Integer
		
		Dim result As Integer = 0
		Dim lFilterValue As Integer
		'Dim vResultArray As Variant
		Dim lItem As ListViewItem
		Dim sKey As String = ""
		Dim vErrorTypes(9) As Object
		Dim sErrorType As String = ""
		
		Dim lRecords As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get the filter value
			m_lReturn = CType(GetFilterType(r_lFilterValue:=lFilterValue), gPMConstants.PMEReturnCode)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' Clear the list view
			lvwMessages.Items.Clear()
			
			' Disable the delete key
			cmdDelete.Enabled = False
			
			' Get the amount of records to return
			' CF 021299 - Check to see if a valid number
			Select Case cboNumberOfRecords.Text.Trim()
				Case PMRecordsAll, ""
					lRecords = gPMConstants.PMAllRecords
				Case Else
					Dim dbNumericTemp As Double
					If Double.TryParse(cboNumberOfRecords.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
						lRecords = CInt(cboNumberOfRecords.Text)
					Else
						lRecords = gPMConstants.PMAllRecords
						cboNumberOfRecords.Text = PMRecordsAll
					End If
			End Select
			
			' Call the business and get the messages
			' CF230899 - Added NumberOfRecords parameter

			m_lReturn = m_oBusiness.SelectMessages(r_vMessages:=m_vResultArray, v_lMessageType:=lFilterValue, v_lNumberOfRecords:=lRecords)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			' CF090299
			If Not Information.IsArray(m_vResultArray) Then
				Return gPMConstants.PMEReturnCode.PMTrue
			End If
			
			' CF021299
			' Re-enable the delete key
			cmdDelete.Enabled = True
			

			vErrorTypes(1) = gPMConstants.PMFatalText

			vErrorTypes(2) = gPMConstants.PMErrorText

			vErrorTypes(3) = gPMConstants.PMWarningText

			vErrorTypes(4) = gPMConstants.PMInfoText

			vErrorTypes(5) = gPMConstants.PMOnErrorText

			vErrorTypes(6) = gPMConstants.PMDebug1Text

			vErrorTypes(7) = gPMConstants.PMDebug2Text

			vErrorTypes(8) = gPMConstants.PMDebug3Text

			vErrorTypes(9) = gPMConstants.PMDebug4Text
			
			For iLoop1 As Integer = 0 To m_vResultArray.GetUpperBound(1)
				
				sKey = "L" & iLoop1
				

				sErrorType = CStr(vErrorTypes(CInt(m_vResultArray(MAMessageType, iLoop1))))
				
				lItem = lvwMessages.Items.Insert(CInt(CStr(iLoop1 + 1)) - 1, sKey, sErrorType, "")
				
				' Add the data
				ListViewHelper.GetListViewSubItem(lItem, 1).Text = CStr(m_vResultArray(MALogDate, iLoop1))
				ListViewHelper.GetListViewSubItem(lItem, 2).Text = CStr(m_vResultArray(MAUserName, iLoop1))
				ListViewHelper.GetListViewSubItem(lItem, 3).Text = CStr(m_vResultArray(MAText, iLoop1))
				ListViewHelper.GetListViewSubItem(lItem, 4).Text = CStr(m_vResultArray(MAErrNumber, iLoop1))
				ListViewHelper.GetListViewSubItem(lItem, 5).Text = CStr(m_vResultArray(MAErrDescription, iLoop1))
				ListViewHelper.GetListViewSubItem(lItem, 6).Text = CStr(m_vResultArray(MACallingAppName, iLoop1))
				ListViewHelper.GetListViewSubItem(lItem, 7).Text = CStr(m_vResultArray(MAAppName, iLoop1))
				ListViewHelper.GetListViewSubItem(lItem, 8).Text = CStr(m_vResultArray(MAClassName, iLoop1))
				ListViewHelper.GetListViewSubItem(lItem, 9).Text = CStr(m_vResultArray(MAMethodName, iLoop1))
				
			Next iLoop1
			
			' CF 021299
			lvwMessages.FocusedItem = lvwMessages.Items.Item(0)
			
			' 230899 - Autosize the list
            'developer guide no. 170
            m_lReturn = CType(SharedFiles.ListViewFunc.ListViewAutoSize(lvwList:=lvwMessages), gPMConstants.PMEReturnCode)
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMFalse
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to refresh list.", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
	
	Private Sub lvwMessages_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwMessages.ColumnClick
		Dim ColumnHeader As ColumnHeader = lvwMessages.Columns(eventArgs.Column)
		
		' if the user's clicked the date column, then sort by the hidden column instead
		If ColumnHeader.Name = "Date" Then
			
			' CF230899 - Changed to use this function instead
            'developer guide no. 170
            m_lReturn = CType(SharedFiles.ListViewFunc.ListViewSortByDate(v_oListView:=lvwMessages, v_iSourceColumn:=ACColumnDate, v_iDirection:=((ListViewHelper.GetSortOrderProperty(lvwMessages) + 1) Mod 2)), gPMConstants.PMEReturnCode)
			
		Else
			
			' set the sort order
			If ListViewHelper.GetSortOrderProperty(lvwMessages) = SortOrder.Ascending Then
				ListViewHelper.SetSortOrderProperty(lvwMessages, SortOrder.Descending)
			Else
				ListViewHelper.SetSortOrderProperty(lvwMessages, SortOrder.Ascending)
			End If
			
		End If
		
		' set it to sorted
		ListViewHelper.SetSortedProperty(lvwMessages, True)
		
	End Sub
End Class
