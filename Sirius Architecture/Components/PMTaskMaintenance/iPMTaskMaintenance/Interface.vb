Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Windows.Forms
'Developer Guide no. 129
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	' Edit History:
	' DAK200999 - Allow selection of task icon.
	' DAK061099 - New columns on Task table
	' DAK231199 - Check PrivilegeLevel to see what we can do
	' DAK011299 - More privilege levels
	' DAK211299 - Add Task Category
	' DAK221299 - More changes for task category
	' ***************************************************************** '
	
	Private Const ACClass As String = "frmInterface"
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lErrorNumber As Integer
	
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	Private m_lTask As Integer
	Private m_lCaptionId As Integer
	Private m_sTaskCode As String = ""
	Private m_sDescription As String = ""
	Private m_iIsDeleted As gPMConstants.PMEReturnCode
	Private m_dEffectiveDate As Date
	Private m_iIsSystemTask As CheckState
	Private m_iTypeOfTask As Integer
	Private m_lPMNavProcessId As Integer
	Private m_sComponentObjectName As String = ""
	Private m_sComponentClassName As String = ""
	Private m_lAutoDeleteAfterNumDays As Integer
	'DAK200999
	Private m_lDisplayIcon As Integer
	'DAK061099
	Private m_iIsViewOnlyTask As CheckState
	Private m_sLinkedObjectName As String = ""
	Private m_sLinkedClassName As String = ""
	Private m_sLinkedCaption As String = ""
	Private m_iIsAvailableTask As CheckState
	
	Private m_lXPos As Integer
	Private m_lYPos As Integer
	Private m_lReturn As Integer
	Private m_vUserArray As Object
	Private m_iNextUserID As Integer
	Private m_bInitialised As Boolean
	
	'DAK231199
	' PrivilegeLevel
	Private m_iPrivilegeLevel As Integer
	'DAK221299
	' TaskCategoryID
	Private m_lTaskCategoryID As Integer
	'DAK221299
	' CurrentKey
	Private m_sCurrentKey As String = ""
	
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
	
	'DAK231199
	Public Property PrivilegeLevel() As Integer
		Get
			Return m_iPrivilegeLevel
		End Get
		Set(ByVal Value As Integer)
			m_iPrivilegeLevel = Value
		End Set
	End Property
	
	'DAK221299
	Public Property TaskCategoryID() As Integer
		Get
			Return m_lTaskCategoryID
		End Get
		Set(ByVal Value As Integer)
			m_lTaskCategoryID = Value
		End Set
	End Property
	
	Public Property CurrentKey() As String
		Get
			Return m_sCurrentKey.Trim()
		End Get
		Set(ByVal Value As String)
			m_sCurrentKey = Value.Trim()
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
	' DAK200999 - Allow selection of task icon.
	'****************************************************************************
	
	Public Function RefreshList() As Integer
		
		Dim result As Integer = 0
		Dim lIndex, lRow As Integer
		Dim oListItem As ListViewItem
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If Me.Visible Then
				lvwTasks.Focus()
			End If
			

			g_oBusiness.CurrentRecord = 0
			
			'Clear the items in the listview box
			lvwTasks.Items.Clear()
			
			'Set row count
			lRow = -1
			
			Do 
				'Get id of first record
				'DAK200999
				'DAK061099
				'DAK211299

				m_lReturn = g_oBusiness.GetNext(vTaskId:=m_lTask, vCaptionId:=m_lCaptionId, vCode:=m_sTaskCode, vDescription:=m_sDescription, vIsDeleted:=m_iIsDeleted, vEffectiveDate:=m_dEffectiveDate, vIsSystemTask:=m_iIsSystemTask, vTypeOfTask:=m_iTypeOfTask, vPMNavProcessId:=m_lPMNavProcessId, vComponentObjectName:=m_sComponentObjectName, vComponentClassName:=m_sComponentClassName, vAutoDeleteAfterNumDays:=m_lAutoDeleteAfterNumDays, vDisplayIcon:=m_lDisplayIcon, vIsViewOnlyTask:=m_iIsViewOnlyTask, vLinkedObjectName:=m_sLinkedObjectName, vLinkedClassName:=m_sLinkedClassName, vLinkedCaption:=m_sLinkedCaption, vIsAvailableTask:=m_iIsAvailableTask, vTaskCategoryID:=m_lTaskCategoryID)
				
				'Exit if you get an invalid response
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Exit Do
				End If
				
				lIndex = (m_lTask)
				'increment the count by 1
				lRow += 1
				
				If m_lDisplayIcon > imgTask.Images.Count Then
					m_lDisplayIcon = 1
				End If
				
				'Populate the listview box
				'DAK221299

				oListItem = lvwTasks.Items.Insert(lRow, "L" & m_sTaskCode.Trim(), m_sTaskCode.Trim(), "")
				
				With oListItem
					ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_sDescription
					ListViewHelper.GetListViewSubItem(oListItem, 2).Text = StringsHelper.Format(m_dEffectiveDate, "general date")
					If (m_iIsDeleted = gPMConstants.PMEReturnCode.PMTrue) Or (m_dEffectiveDate > DateTime.Now) Then

                        'Developer Guide no.13
                        .ForeColor = Color.Gray
					End If
				End With
				
			Loop 
			
			'DAK221299
			If lvwTasks.Items.Count = 0 Then
				CurrentKey = ""
				Return result
			End If
			
			If CurrentKey = "" Then
				lvwTasks.FocusedItem = lvwTasks.Items.Item(0)
			Else
				lvwTasks.FocusedItem = lvwTasks.Items.Item(CurrentKey)
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			'Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Refresh user details ", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
			'    lvwTasks.ListItems.Clear
			
			'Getdetails from business

			m_lReturn = g_oBusiness.GetDetails()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return result
			End If
			
			' Refresh the List
			
			Return RefreshList()
		
		Catch excep As System.Exception
			
			
			
			'Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load task details from database", vApp:=ACApp, vClass:=ACClass, vMethod:="PopListBox", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click
		
		m_lReturn = AddTask()
		
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			MessageBox.Show("Failed to show Add Task screen", "Task Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
			Exit Sub
		End If
		
	End Sub
	
	Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click
		
		m_lReturn = WriteToDb()
		
	End Sub
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		Dim iRetval As DialogResult
		

		m_lReturn = g_oBusiness.Cancel
		
		If (m_lReturn = gPMConstants.PMEReturnCode.PMDataChanged) Or (cmdApply.Enabled) Then
			iRetval = MessageBox.Show("Do you want to abandon changes?", "Task Maintenance", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
			If iRetval = System.Windows.Forms.DialogResult.No Then
				Exit Sub
			End If
		End If
		
		Me.Hide()
		
	End Sub
	
	Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click
		
        'delete selected task using the DeleteTask function
        m_lReturn = DeleteTask(lvwTasks.FocusedItem)
		
	End Sub
	
	Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
		
        'Edit selected user using the Editeuser function
        m_lReturn = EditTask(lvwTasks.FocusedItem)
		
		m_lReturn = RefreshList()
		
	End Sub
	
	'UPGRADE_NOTE: (7001) The following declaration (cmdHelp_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Sub cmdHelp_Click()
		'
		'MessageBox.Show("There is no help associated with this screen", "Task Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
		'
	'End Sub
	
	Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
		
		m_lStatus = gPMConstants.PMEReturnCode.PMOK
		'Update the database
		m_lReturn = WriteToDb()
		'Hide the form
		Me.Hide()
		
	End Sub
	
	Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			
			With uctPMResizer1
				.SetControlResizeOption("cmdOK", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdCancel", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdApply", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("tabTask", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				
				.SetControlResizeOption("cmdAdd", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdEdit", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdDelete", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				
				.SetControlResizeOption("lvwTasks", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
			End With
			
			lvwTasks.Focus()
			
		End If
	End Sub
	
	Private Sub Form_Initialize_Renamed()
		
		' Initialise the error number value.
		m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue
		
		'Initialise the form using selected function
		m_lReturn = InitialForm()
		
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

		'DAK200999 - load images into image list
		If albImageStore.SmallImageList.Images.Count > 1 Then
			imgTask.Images.Clear()
			imgTask.ImageSize = New Size(imgTask.ImageSize.Width, 16)
			imgTask.ImageSize = New Size(16, imgTask.ImageSize.Height)
			For Each oIcon As Listbar.SSImage In albImageStore.SmallImageList.Images

				imgTask.Images.Add(oIcon.Key, CType(oIcon.Picture, Image))
			Next oIcon
		End If

		'Populate the listbox using the selected function
		m_lReturn = PopListBox()
		
		'DAK231199
		If PrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupFullPrivileges Then
			cmdAdd.Enabled = True
			cmdDelete.Enabled = True
		Else
			cmdAdd.Enabled = False
			cmdDelete.Enabled = False
		End If
		
		'DAK011299
		If PrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupViewOnly Then
			cmdEdit.Text = "&View"
		Else
			cmdEdit.Text = "&Edit"
		End If
		
		' Check for errors.
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
			
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			Exit Sub
		End If
		
	End Sub
	
	Private Sub lvwTasks_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwTasks.ColumnClick
		Dim ColumnHeader As ColumnHeader = lvwTasks.Columns(eventArgs.Column)
		
		With lvwTasks
			
			ListViewHelper.SetSortedProperty(lvwTasks, False)
			ListViewHelper.SetSortKeyProperty(lvwTasks, ColumnHeader.Index + 1 - 1)
			ListViewHelper.SetSortedProperty(lvwTasks, True)
			
		End With
		
	End Sub
	
	Private Sub lvwTasks_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwTasks.DoubleClick
		
		
		' Edit details of user if doubleclicked
		Dim oListItem As ListViewItem = lvwTasks.GetItemAt(m_lXPos, m_lYPos)
		
		m_lReturn = EditTask(oListItem)
		
		m_lReturn = RefreshList()
		
	End Sub
	
	Private Sub lvwTasks_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwTasks.Enter
		
		'when the focus is on the listview box set cmdedit button as default
		VB6.SetDefault(cmdEdit, True)
		
	End Sub
	
	Private Sub lvwTasks_ItemClick(ByVal Item As ListViewItem)
		
		'DAK231199
		If PrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupFullPrivileges Then
			cmdDelete.Enabled = True
		End If
		

        'Developer guide no. 13
        If Item.ForeColor = Color.Gray Then
            cmdDelete.Text = "&Undelete"
            ToolTip1.SetToolTip(cmdDelete, "Undelete Selected Task")
            cmdEdit.Enabled = False
        Else
            cmdDelete.Text = "&Delete"
            ToolTip1.SetToolTip(cmdDelete, "Delete Selected Task")
            cmdEdit.Enabled = True
        End If
		
	End Sub
	
	Private Sub lvwTasks_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwTasks.MouseMove
		Dim Button As Integer = CInt(eventArgs.Button)
		Dim Shift As Integer = Control.ModifierKeys \ &H10000
		Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
		Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
		
		m_lXPos = CInt(x)
		m_lYPos = CInt(y)
		
	End Sub
	
	'***********************************************************************
	'
	' Function Name: WriteToDb()
	'
	' Description: Does the checks and updates the db
	'
	'***********************************************************************
	
	Private Function WriteToDb() As gPMConstants.PMEReturnCode
		
		Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

			m_lReturn = g_oBusiness.Update
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("Changes to Task Details Could not be written to Database", "Task Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)

                g_oBusiness.Dispose()
				m_lReturn = InitialForm()
				m_lReturn = PopListBox()
				Return result
			End If
			
			m_lReturn = PopListBox()
			cmdApply.Enabled = False
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			'Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to write details to database", vApp:=ACApp, vClass:=ACClass, vMethod:="WriteToDb", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	
	'********************************************************************
	'
	' Function Name: EditTask()
	'
	' Description: Writes edited details of an existing user to the database
	'********************************************************************
	
	Private Function EditTask(ByRef oListItem As ListViewItem) As Integer
		
		Dim result As Integer = 0
		Dim oTaskForm As frmTask
		Dim lRow As Integer
        Dim sKey As String
		Dim lStatus As Integer
        Dim sPrinter As String = ""
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If oListItem Is Nothing Then
				Return result
			End If
			
			' If User is Deleted/Marked for Deletion then exit

            If oListItem.ForeColor = Color.Gray Then
                Return result
            End If

            sKey = oListItem.Name
            'DAK221299
            CurrentKey = sKey
            '    lRow& = Val(Right(sKey$, Len(sKey$) - 1)) + 1
            lRow = oListItem.Index + 1

            'Getdetails from business


            g_oBusiness.CurrentRecord = lRow - 1

            'DAK200999 - tasks can now have a variety of icons
            '    If (lvwTasks.SelectedItem.SmallIcon <> "task") Then
            '        MsgBox "Cannot edit the details for the selected task", vbInformation + vbOKOnly, "Task Maintenance"
            '        Exit Function
            '    End If

            'DAK211299

            m_lReturn = g_oBusiness.GetNext(vTaskId:=m_lTask, vCaptionId:=m_lCaptionId, vCode:=m_sTaskCode, vDescription:=m_sDescription, vIsDeleted:=m_iIsDeleted, vEffectiveDate:=m_dEffectiveDate, vIsSystemTask:=m_iIsSystemTask, vTypeOfTask:=m_iTypeOfTask, vPMNavProcessId:=m_lPMNavProcessId, vComponentObjectName:=m_sComponentObjectName, vComponentClassName:=m_sComponentClassName, vAutoDeleteAfterNumDays:=m_lAutoDeleteAfterNumDays, vDisplayIcon:=m_lDisplayIcon, vIsViewOnlyTask:=m_iIsViewOnlyTask, vLinkedObjectName:=m_sLinkedObjectName, vLinkedClassName:=m_sLinkedClassName, vLinkedCaption:=m_sLinkedCaption, vIsAvailableTask:=m_iIsAvailableTask, vTaskCategoryID:=m_lTaskCategoryID)

            'Exit if you get an invalid response
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            oTaskForm = New frmTask()


            With oTaskForm
                .TaskID = m_lTask
                .CaptionID = m_lCaptionId
                .TaskCode = m_sTaskCode
                .Description = m_sDescription
                .IsDeleted = m_iIsDeleted
                .EffectiveDate = m_dEffectiveDate
                .IsSystemTask = m_iIsSystemTask
                .TypeOfTask = m_iTypeOfTask
                .PMNavProcessId = m_lPMNavProcessId
                .ComponentObjectName = m_sComponentObjectName
                .ComponentClassName = m_sComponentClassName
                .AutoDeleteAfterNumDays = m_lAutoDeleteAfterNumDays
                'DAK200999
                .DisplayIcon = m_lDisplayIcon
                'DAK061099
                .IsViewOnlyTask = m_iIsViewOnlyTask
                .LinkedObjectName = m_sLinkedObjectName
                .LinkedClassName = m_sLinkedClassName
                .LinkedCaption = m_sLinkedCaption
                .IsAvailableTask = m_iIsAvailableTask
                'DAK231199
                .PrivilegeLevel = PrivilegeLevel
                'DAK211299
                .TaskCategoryID = m_lTaskCategoryID
                .ShowForm(USREditTask)

                lStatus = .Status

                If lStatus <> gPMConstants.PMEReturnCode.PMOK Then

                    If Me.Visible Then
                        lvwTasks.Focus()
                    End If

                Else
                    'DAK200999
                    'DAK061099
                    'DAK211299

                    m_lReturn = g_oBusiness.EditUpdate(lRow:=lRow, vTaskId:=.TaskID, vCaptionId:=.CaptionID, vCode:=.TaskCode, vDescription:=.Description, vIsDeleted:=.IsDeleted, vEffectiveDate:=.EffectiveDate, vIsSystemTask:=.IsSystemTask, vTypeOfTask:=.TypeOfTask, vPMNavProcessId:=.PMNavProcessId, vComponentObjectName:=.ComponentObjectName, vComponentClassName:=.ComponentClassName, vAutoDeleteAfterNumDays:=.AutoDeleteAfterNumDays, vDisplayIcon:=.DisplayIcon, vIsViewOnlyTask:=.IsViewOnlyTask, vLinkedObjectName:=.LinkedObjectName, vLinkedClassName:=.LinkedClassName, vLinkedCaption:=.LinkedCaption, vIsAvailableTask:=.IsAvailableTask, vTaskCategoryID:=.TaskCategoryID)

                    'DAK221299
                    '            m_lReturn = g_oBusiness.Update()
                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                        m_lReturn = RefreshList()

                    End If

                End If

            End With

            oTaskForm.Close()
            oTaskForm = Nothing

            'DAK221299
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            'Error Section

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Edit Task Details", vApp:=ACApp, vClass:=ACClass, vMethod:="EditTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
		Dim lStatus As Integer
        Dim lRow As Integer
		Dim sPrinter As String = ""
		
		Select Case Information.Err().Number
			Case Is < 0
				Conversion.ErrorToString(5)
			Case 1
				GoTo err_AddTask
		End Select
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		Dim oTaskForm As New frmTask

		With oTaskForm
			
			.TaskCode = ""
			.EffectiveDate = DateTime.Now
			.PMNavProcessId = 0
			.TypeOfTask = 0
			'DAK231199
			.PrivilegeLevel = PrivilegeLevel
			'DAK211299
			.TaskCategoryID = TaskCategoryID
			
			.ShowForm(USRAddTask)
			
			lStatus = .Status
			
			lRow = lvwTasks.Items.Count + 1
			
			If lStatus <> gPMConstants.PMEReturnCode.PMOK Then
				
				If Me.Visible Then
					lvwTasks.Focus()
				End If
				
			Else
				'DAK200999
				'DAK061099
				'DAK211299

				m_lReturn = g_oBusiness.EditAdd(lRow:=lRow, vTaskId:=.TaskID, vCaptionId:=.CaptionID, vCode:=.TaskCode, vDescription:=.Description, vIsDeleted:=.IsDeleted, vEffectiveDate:=.EffectiveDate, vIsSystemTask:=.IsSystemTask, vTypeOfTask:=.TypeOfTask, vPMNavProcessId:=.PMNavProcessId, vComponentObjectName:=.ComponentObjectName, vComponentClassName:=.ComponentClassName, vAutoDeleteAfterNumDays:=.AutoDeleteAfterNumDays, vDisplayIcon:=.DisplayIcon, vIsViewOnlyTask:=.IsViewOnlyTask, vLinkedObjectName:=.LinkedObjectName, vLinkedClassName:=.LinkedClassName, vLinkedCaption:=.LinkedCaption, vIsAvailableTask:=.IsAvailableTask, vTaskCategoryID:=.TaskCategoryID)
				
				'DAK221299
				If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
					
					CurrentKey = "L" & .TaskCode
					
					m_lReturn = RefreshList()
					
				End If
				
			End If
			
		End With
		
		oTaskForm.Close()
		oTaskForm = Nothing
		
		'DAK221299
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			Return gPMConstants.PMEReturnCode.PMFalse
		End If
		
		Return result
		
err_AddTask: 
		
		'Error Section
		
		result = gPMConstants.PMEReturnCode.PMError
		
		iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Task", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTask", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
		
		Return result
		
	End Function
	
	'********************************************************************
	'
	' Function Name: DeleteTask()
	'
	' Description: Deletes task from the database
	'********************************************************************
	
	Private Function DeleteTask(ByRef oListItem As ListViewItem) As Integer
		
		Dim result As Integer = 0
		Dim lRow As Integer
        Dim sKey As String
        Dim sName As String = ""
		Dim lID As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If oListItem Is Nothing Then
				Return result
			End If
			
			sKey = oListItem.Name
			'DAK221299
			CurrentKey = sKey
			'    lRow& = Val(Right(sKey$, Len(sKey$) - 1)) + 1
			lRow = oListItem.Index + 1
			
			sName = (lvwTasks.FocusedItem.Text)
			'DAK221299
			'    lID& = (lRow& + 1)
			lID = lRow
			

            'Developer guide no. 13
            If lvwTasks.FocusedItem.ForeColor = Color.Gray Then

                m_lReturn = g_oBusiness.EditUpdate(lRow:=lID, vIsDeleted:=gPMConstants.PMEReturnCode.PMFalse, vEffectiveDate:=DateTime.Now)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Undelete selected Task")
                    Return result
                End If


                'Developer guide no. 13
                lvwTasks.FocusedItem.ForeColor = Color.Black
            Else

                m_lReturn = g_oBusiness.EditDelete(lID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Delete selected Task")
                    Return result
                End If


                'Developer guide no. 13
                lvwTasks.FocusedItem.ForeColor = Color.Gray
            End If

            ' Refresh List & enable apply.
            lvwTasks.Refresh()
            cmdApply.Enabled = True

            ' Select the Item deleted so that the Delete button
            ' caption can be set correctly
           
            lvwTasks_ItemClick(lvwTasks.Items.Item(lvwTasks.FocusedItem.Index))

            Return result

        Catch excep As System.Exception



            'Error Section

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Delete Task", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
	End Function
	
	' ***************************************************************** '
	' Name: CheckTaskname
	'
	' Description:
	'
	'
	' ***************************************************************** '
	Public Function CheckTaskname(ByVal v_sTaskname As String) As Integer
		
		Dim result As Integer = 0
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			For lRow As Integer = 1 To lvwTasks.Items.Count
				If v_sTaskname.Trim() = lvwTasks.Items.Item(lRow - 1).Text.Trim() Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			Next 
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckTaskname Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckTaskName", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
		MemoryHelper.ReleaseMemory()
	End Sub


    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabTask.SelectedIndex = 0
        End If
    End Sub
End Class
