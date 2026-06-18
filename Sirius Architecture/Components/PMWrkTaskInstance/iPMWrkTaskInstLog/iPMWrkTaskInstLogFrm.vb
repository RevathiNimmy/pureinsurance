Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Text
Imports System.Windows.Forms
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	
	Private Const ACClass As String = "frmInterface"
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_iTask As gPMConstants.PMEComponentAction
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
    Private sortColumn As Integer = -1
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	Private m_lPMWrkTaskInstanceCnt As Integer
	Private m_dtDateCreated As Date
	Private m_sLogText As String = ""
	Private m_lCreatedByID As Integer
	Private m_sCreatedByName As String = ""
	
    Private m_vLogEntriesArray As Object
	
	Private m_lXPos As Integer
	Private m_lYPos As Integer
	Private m_lReturn As gPMConstants.PMEReturnCode
	Private m_bInitialised As Boolean
	
	' PUBLIC Property Procedures (Begin)
	
	Public ReadOnly Property ErrorNumber() As Integer
		Get
			
			' Standard Property.
			
			' Return any error number that might have
			' occurred on the interface.
			Return m_lErrorNumber
			
		End Get
	End Property
	
	Public WriteOnly Property CallingAppName() As String
		Set(ByVal Value As String)
			
			' Standard Property.
			
			' Set the calling application name.
			m_sCallingAppName = Value
			
		End Set
	End Property
	
	Public ReadOnly Property Status() As Integer
		Get
			
			' Standard Property.
			
			' Return the interface exit status.
			Return m_lStatus
			
		End Get
	End Property
	
	Public WriteOnly Property Task() As Integer
		Set(ByVal Value As Integer)
			
			' Standard Property.
			
			m_iTask = Value
			
		End Set
	End Property
	
	Public WriteOnly Property Navigate() As Integer
		Set(ByVal Value As Integer)
			
			' Standard Property.
			
			' Set the navigate flag.
			m_lNavigate = Value
			
		End Set
	End Property
	
	Public WriteOnly Property ProcessMode() As Integer
		Set(ByVal Value As Integer)
			
			' Standard Property.
			
			' Set the process mode.
			m_lProcessMode = Value
			
		End Set
	End Property
	
	Public WriteOnly Property TransactionType() As String
		Set(ByVal Value As String)
			
			' Standard Property.
			
			' Set the type of business.
			m_sTransactionType = Value
			
		End Set
	End Property
	
	Public WriteOnly Property EffectiveDate() As Date
		Set(ByVal Value As Date)
			
			' Standard Property.
			
			' Set the effective date.
			m_dtEffectiveDate = Value
			
		End Set
	End Property
	
	Public WriteOnly Property PMWrkTaskInstanceCnt() As Integer
		Set(ByVal Value As Integer)
			
			m_lPMWrkTaskInstanceCnt = Value
			
		End Set
	End Property
	
	Public Function InitialForm() As Integer
		
		Dim result As Integer = 0
		Try 
			
			
			Return gPMConstants.PMEReturnCode.PMTrue
		
		Catch excep As System.Exception
			
			
			
			'Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the business", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	'****************************************************************************
	'
	' Function Name: Refreshlist
	'
	' Description: Refreshes the listview box with data held in
	'              the business, not the database.
	'****************************************************************************
	
	Public Function RefreshList() As Integer
		
		Dim result As Integer = 0
        Dim oListItem As ListViewItem
		Dim sString As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Clear the items in the listview box
			lvwTaskInstLogs.Items.Clear()
			

			m_lReturn = g_oBusiness.GetTaskInstLogEntries(v_lPMWrkTaskInstanceCnt:=m_lPMWrkTaskInstanceCnt, r_vLogEntriesArray:=m_vLogEntriesArray)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return result
			End If
			
			If Not Information.IsArray(m_vLogEntriesArray) Then
				Return result
			End If
			
			For lRow As Integer = m_vLogEntriesArray.GetLowerBound(1) To m_vLogEntriesArray.GetUpperBound(1)
				
				sString = CStr(m_vLogEntriesArray(ACText, lRow))
				
				m_lReturn = CType(GenHalveApostrophes(sString:=sString), gPMConstants.PMEReturnCode)
				
				m_vLogEntriesArray(ACText, lRow) = sString
				
				m_lReturn = CType(GenCRLF(sString:=sString), gPMConstants.PMEReturnCode)
				
                oListItem = lvwTaskInstLogs.Items.Add("L" & lRow, CStr(m_vLogEntriesArray(ACCreatedByName, lRow)), 0)

				With oListItem
					ListViewHelper.GetListViewSubItem(oListItem, 1).Text = StringsHelper.Format(m_vLogEntriesArray(ACDateCreated, lRow), "general date")
					ListViewHelper.GetListViewSubItem(oListItem, 2).Text = sString
				End With
			Next lRow
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			'Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Refresh task log details ", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	'****************************************************************************
	'
	' Function Name: PopListBox
	'
	' Description: Populates listview box
	'
	'****************************************************************************
	
	Public Function PopListBox() As Integer
		
		Dim result As Integer = 0

		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'    'Clear the items in the listview box
			'    lvwTaskInstLogs.ListItems.Clear
			
			' Refresh the List
			
			Return RefreshList()
		
		Catch excep As System.Exception
			
			
			
			'Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load task details from database", vApp:=ACApp, vClass:=ACClass, vMethod:="PopListBox", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	'********************************************************************
	'
	' Function Name: ViewTask()
	'
	' Description: Views the task
	'********************************************************************
	
	Private Function ViewTask(ByRef oListItem As ListViewItem) As Integer
		
		Dim result As Integer = 0
		Dim oTaskForm As frmTaskLogEntry
		Dim lRow As Integer
		Dim sKey As String = ""
		Dim lStatus As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If oListItem Is Nothing Then
				Return result
			End If
			
			sKey = oListItem.Name
			lRow = CInt(Conversion.Val(sKey.Substring(sKey.Length - (sKey.Length - 1))))
			
			'Getdetails from business
			
			m_lCreatedByID = CInt(m_vLogEntriesArray(ACCreatedById, lRow))
			m_sCreatedByName = CStr(m_vLogEntriesArray(ACCreatedByName, lRow))
			m_dtDateCreated = CDate(m_vLogEntriesArray(ACDateCreated, lRow))
			m_sLogText = CStr(m_vLogEntriesArray(ACText, lRow))
			
			oTaskForm = New frmTaskLogEntry()

			With oTaskForm
				.PMWrkTaskInstanceCnt = m_lPMWrkTaskInstanceCnt
				.CreatedByID = m_lCreatedByID
				.DateCreated = m_dtDateCreated
				.LogText = m_sLogText
				
				.ShowForm(USRViewTask)
				
				lStatus = .Status
				
			End With
			
			oTaskForm.Close()
			oTaskForm = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			'Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Edit Task Details", vApp:=ACApp, vClass:=ACClass, vMethod:="ViewTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	'********************************************************************
	'
	' Function Name: AddTask()
	'
	' Description: Adds new task to the database
	'********************************************************************
	
	Private Function AddTask() As Integer
		
		Dim result As Integer = 0
		Dim oTaskForm As frmTaskLogEntry
        Dim lStatus As Integer
		
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			oTaskForm = New frmTaskLogEntry()

			With oTaskForm
				
				.PMWrkTaskInstanceCnt = m_lPMWrkTaskInstanceCnt
				.CreatedByID = g_oObjectManager.UserID
				.DateCreated = DateTime.Today.AddDays(DateTimeHelper.Time.ToOADate())
				.LogText = ""
				
				.ShowForm(USRAddTask)
				
				lStatus = .Status
				
				If lStatus = gPMConstants.PMEReturnCode.PMOK Then
					
					m_lReturn = CType(RefreshList(), gPMConstants.PMEReturnCode)
				End If
				
			End With
			
			oTaskForm.Close()
			oTaskForm = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			'Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Task", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	'******************************************************************************
	' GenCRLF
	'
	' Take a string and replace vbCRLF with a space
	'
	'******************************************************************************
	Private Function GenCRLF(ByRef sString As String) As Integer
		
		Dim result As Integer = 0
		Dim i As Integer
		Dim sTemp As New StringBuilder
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If sString.Length = 0 Then
				Return result
			End If
			
			sTemp = New StringBuilder("")
			
			Do 
				i = (sString.IndexOf(Strings.Chr(13) & Strings.Chr(10)) + 1)
				
				If i = 0 Then
					sTemp.Append(sString)
					Exit Do
				End If
				
				sTemp.Append(sString.Substring(0, i - 1) & " ")
				sString = sString.Substring(i + 1)
			Loop 
			
			sString = sTemp.ToString()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Run Time Error", vApp:=ACApp, vClass:=ACClass, vMethod:="GenCRLF", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	'******************************************************************************
	' GenHalveApostrophes
	'
	' Take two apostrophes and replace with one
	'
	'******************************************************************************
	Private Function GenHalveApostrophes(ByRef sString As String) As Integer
		
		Dim result As Integer = 0
		Dim i As Integer
		Dim sTemp As New StringBuilder
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If sString.Length = 0 Then
				Return result
			End If
			
			sTemp = New StringBuilder("")
			
			Do 
				i = (sString.IndexOf("''") + 1)
				
				If i = 0 Then
					sTemp.Append(sString)
					Exit Do
				End If
				
				sTemp.Append(sString.Substring(0, i - 1))
				sString = sString.Substring(i)
			Loop 
			
			sString = sTemp.ToString()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error.
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Run Time Error", vApp:=ACApp, vClass:=ACClass, vMethod:="GenHalveApostrophes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click
		
		m_lReturn = CType(AddTask(), gPMConstants.PMEReturnCode)
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			MessageBox.Show("Failed to show Add Task screen", "Task Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
			Exit Sub
		End If
		
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		Me.Hide()
		
	End Sub
	
	Private Sub cmdView_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdView.Click
		
		'Edit selected user using the Editeuser function
        m_lReturn = CType(ViewTask(lvwTaskInstLogs.FocusedItem), gPMConstants.PMEReturnCode)
	End Sub
	
	Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click
		
		MessageBox.Show("There is no help associated with this screen", "Task Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
		
	End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		m_lStatus = gPMConstants.PMEReturnCode.PMOK
		'Hide the form
		Me.Hide()
		
	End Sub
	
	Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			
			With uctPMResizer1
				.SetControlResizeOption("cmdOK", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdCancel", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdHelp", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("tabMainTab", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				
				.SetControlResizeOption("cmdAdd", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdView", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				
				.SetControlResizeOption("lvwTaskInstLogs", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
			End With
			
			lvwTaskInstLogs.Focus()
			
		End If
	End Sub
	
	Private Sub Form_Initialize_Renamed()
		
		' Initialise the error number value.
		m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
		
		'Initialise the form using selected function
		m_lReturn = CType(InitialForm(), gPMConstants.PMEReturnCode)
		
	End Sub
	

	Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		' Check if we have had an error so far.
		' Possibly creating the business object.
		If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
			' We have already encountered an error,
			' so we MUST exit now.
			Exit Sub
		End If
		
		With uctPMResizer1
			.NoResizeByDefault = True
			.FormMinHeight = 6645
			.FormMinWidth = 9405
		End With
		
		'Populate the listbox using the selected function
		m_lReturn = PopListBox()
		
		' Check for errors.
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
			
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			Exit Sub
		End If
		
		cmdAdd.Enabled = Not (m_iTask = gPMConstants.PMEComponentAction.PMView)
		
	End Sub
	
	Private Sub lvwTaskInstLogs_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwTaskInstLogs.ColumnClick
		Dim ColumnHeader As ColumnHeader = lvwTaskInstLogs.Columns(eventArgs.Column)
		
		With lvwTaskInstLogs
            'Comment the code,replace the code for sorting
            'ListViewHelper.SetSortedProperty(lvwTaskInstLogs, False)
            ''If we've already sorted on this one...
            'If ListViewHelper.GetSortKeyProperty(lvwTaskInstLogs) = ColumnHeader.Index + 1 - 1 Then
            '	ListViewHelper.SetSortOrderProperty(lvwTaskInstLogs, 1 - ListViewHelper.GetSortOrderProperty(lvwTaskInstLogs))
            'Else
            '	ListViewHelper.SetSortKeyProperty(lvwTaskInstLogs, ColumnHeader.Index + 1 - 1)
            'End If
            'ListViewHelper.SetSortedProperty(lvwTaskInstLogs, True)



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
		
	End Sub
	
	Private Sub lvwTaskInstLogs_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwTaskInstLogs.DoubleClick
		
		
		' Edit details of user if doubleclicked
		Dim oListItem As ListViewItem = lvwTaskInstLogs.GetItemAt(m_lXPos, m_lYPos)
		
		m_lReturn = CType(ViewTask(oListItem), gPMConstants.PMEReturnCode)
		
	End Sub
	
	Private Sub lvwTaskInstLogs_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwTaskInstLogs.Enter
		
		'when the focus is on the listview box set cmdView button as default
		VB6.SetDefault(cmdView, True)
		
	End Sub
	
	Private Sub lvwTaskInstLogs_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwTaskInstLogs.MouseMove
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		
		m_lXPos = CInt(x)
		m_lYPos = CInt(y)
		
	End Sub
End Class
