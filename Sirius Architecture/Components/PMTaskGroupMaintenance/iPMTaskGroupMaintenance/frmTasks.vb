Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide no. 129
Imports SharedFiles
Friend Partial Class frmTasks
	Inherits System.Windows.Forms.Form
	' Edit History:
	' DAK200999 - Display task icons
	' ***************************************************************** '
	
	'Declarations
	
	Private m_lStatus As Integer
	
	Private m_lError As gPMConstants.PMEReturnCode
	
	Private m_lFormMode As Integer
	

	Private m_oBusiness As bPMTaskGroup.Business

	Private m_lMasterGroupId As Integer
	Private m_sMasterGroupCode As String = ""
	Private m_sMasterGroupDescription As String = ""
	Private m_dMasterEffectiveDate As Date
	
	Private m_lTaskGroupID As Integer
	Private m_sTaskGroupCode As String = ""
	Private m_sTaskGroupDescription As String = ""
	Private m_dEffectiveDate As Date
	Private m_iIsDeleted As gPMConstants.PMEReturnCode
	Private m_iIncluded As Integer
	
	Private m_lXPos As Integer
	Private m_lYPos As Integer
	Private m_lReturn As Integer
	Private m_bInitialised As Boolean
	
	'DAK200999
	Private m_lTaskIcon As Integer
	
	Private Const ACClass As String = "frmTasks"
	
	
	Public Property GroupID() As Integer
		Get
			
			Return m_lMasterGroupId
			
		End Get
		Set(ByVal Value As Integer)
			
			m_lMasterGroupId = Value
			
		End Set
	End Property
	
	
	Public Property GroupCode() As String
		Get
			
			Return m_sMasterGroupCode
			
		End Get
		Set(ByVal Value As String)
			
			m_sMasterGroupCode = Value
			
		End Set
	End Property
	
	
	Public Property GroupDescription() As String
		Get
			
			Return m_sMasterGroupDescription
			
		End Get
		Set(ByVal Value As String)
			
			m_sMasterGroupDescription = Value
			
		End Set
	End Property
	
	
	Public Property EffectiveDate() As Date
		Get
			
			Return m_dMasterEffectiveDate
			
		End Get
		Set(ByVal Value As Date)
			
			m_dMasterEffectiveDate = Value
			
		End Set
	End Property
	
	Public ReadOnly Property Status() As Integer
		Get
			
			Return m_lStatus
			
		End Get
	End Property
	
	
	'UPGRADE_NOTE: (7001) The following declaration (get FormMode) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function FormMode() As Integer
		'
		'Return m_lFormMode
		'
	'End Function
	'UPGRADE_NOTE: (7001) The following declaration (let FormMode) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Sub FormMode(ByVal Value As Integer)
		'
		'm_lFormMode = Value
		'
	'End Sub
	
	'*************************************************************
	'
	' Function Name:ShowForm()
	'
	' Description: Shows form details which correspond with what
	'              the user has selected from the previous form
	'*************************************************************
	
	Public Function ShowForm() As Integer
		
		Dim result As Integer = 0

		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			Me.Text = "Edit Tasks of " & m_sMasterGroupCode.Trim()
			
			'Show the form
			Me.ShowDialog()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			'Error Section
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to show Group Form", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	Private Function InitialForm() As Integer
		
		Dim result As Integer = 0
		Dim sTitle, sMessage As String
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Get an instance of the business object via
			' the public object manager.
			Dim temp_m_oBusiness As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bPMTaskGroup.Business", vInstanceManager:="ClientManager")
			m_oBusiness = temp_m_oBusiness
			
			' Check for errors.
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				' Failed to get an instance of the business object.
				result = gPMConstants.PMEReturnCode.PMFalse
				
				' Display error stating the problem.
				
				' Get description from the resource file.
				sTitle = ACBusinessFailTitleText
				sMessage = ACBusinessFailText
				
				' Display message.
				MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
				
				Return result
			End If
			Return result
		
		Catch excep As System.Exception
			
			
			
			'Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the business", vApp:=ACApp, vClass:=ACClass, vMethod:="InitialForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	'UPGRADE_NOTE: (7001) The following declaration (TerminateForm) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Function TerminateForm() As Integer
		'
		'Dim result As Integer = 0
		'Try 
			'
			'result = gPMConstants.PMEReturnCode.PMTrue
			'
			' Check if we have an instance of the business object.
			'If Not (m_oBusiness Is Nothing) Then
				' Terminate the business object

				'm_lReturn = m_oBusiness.Terminate()
				'
				' Check for errors.
				'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					' Log Error.
					'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to terminate the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Terminate")
				'End If
				'
				' Destroy the instance of the business object
				' from memory.
				'm_oBusiness = Nothing
			'End If
			'
			'Return result
		'
		'Catch excep As System.Exception
			'
			'
			'
			'Error Section
			'
			'result = gPMConstants.PMEReturnCode.PMError
			'
			'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the business", vApp:=ACApp, vClass:=ACClass, vMethod:="TerminateForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
			'
			'Return result
			'
		'End Try
	'End Function
	
	'****************************************************************************
	'
	' Function Name: RefreshList
	'
	' Description: Refreshes the listview boxes with data held in
	'              the business, not the database.
	'****************************************************************************
	
	Private Function RefreshList() As Integer
		
		Dim result As Integer = 0
		Dim lIndex, lRow As Integer
		Dim oListItem As ListViewItem
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

			m_oBusiness.CurrentRecord = 0
			
			'Clear the items in the listview boxes
			lvwAll.Items.Clear()
			lvwContents.Items.Clear()
			
			'Set row count
			lRow = -1
			
			Do 
				'Get id of first record

				m_lReturn = m_oBusiness.GetNext(vPMTaskGroupID:=m_lTaskGroupID, vTaskGroupCode:=m_sTaskGroupCode, vTaskGroupDescription:=m_sTaskGroupDescription, vIsDeleted:=m_iIsDeleted, vEffectiveDate:=m_dEffectiveDate, vDisplayIcon:=m_lTaskIcon, vIncluded:=m_iIncluded)
				
				'Exit if you get an invalid response
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Exit Do
				End If
				
				lIndex = (m_lTaskGroupID)
				'increment the count by 1
				lRow += 1
				
				If m_lTaskIcon > imgGroup.Images.Count Then
					m_lTaskIcon = 1
				End If
				
				'Populate the listview box
				'DAK200999

                oListItem = lvwAll.Items.Add("L" & lRow, m_sTaskGroupCode.Trim(), m_lTaskIcon - 1)
				With oListItem
					ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_sTaskGroupDescription
					If (m_iIsDeleted = gPMConstants.PMEReturnCode.PMTrue) Or (m_dEffectiveDate > DateTime.Now) Then

                        'Developer Guide no. 13
                        .ForeColor = Color.Gray
					End If
				End With
				
				oListItem = Nothing
				
				If m_iIncluded = 1 Then
					'DAK200999

                    oListItem = lvwContents.Items.Add("L" & lRow, m_sTaskGroupCode.Trim(), m_lTaskIcon - 1)
					With oListItem
						ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_sTaskGroupDescription
						If (m_iIsDeleted = gPMConstants.PMEReturnCode.PMTrue) Or (m_dEffectiveDate > DateTime.Now) Then

                            'Developer Guideno.13
                            .ForeColor = Color.Gray
						End If
					End With
					oListItem = Nothing
				End If
			Loop 

			Return result
		
		Catch excep As System.Exception
			
			
			
			'Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Refresh group details ", vApp:=ACApp, vClass:=ACClass, vMethod:="RefreshList", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	'****************************************************************************
	'
	' Function Name: PopListBox
	'
	' Description: Populates listview boxes
	'
	'****************************************************************************
	
	Public Function PopListBox() As Integer
		
		Dim result As Integer = 0

		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Getdetails from business

			m_lReturn = m_oBusiness.GetTasks(lGroupID:=m_lMasterGroupId)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return result
			End If
			
			' Refresh the List
			
			Return RefreshList()
		
		Catch excep As System.Exception
			
			
			
			'Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load group details from database", vApp:=ACApp, vClass:=ACClass, vMethod:="PopListBox", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	'***********************************************************************
	'
	' Function Name: WriteToDb()
	'
	' Description: Does the checks and updates the db
	'
	'***********************************************************************
	
	Private Function WriteToDb() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			

			m_lReturn = m_oBusiness.UpdateTasks(lGroupID:=m_lMasterGroupId)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("Changes to Group Task Details Could not be written to Database", "Group Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)

                m_oBusiness.Dispose()
				m_oBusiness = Nothing
				m_lReturn = InitialForm()
				m_lReturn = PopListBox()
				Return result
			End If
			
			'm_lReturn = PopListBox()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			'Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to write details to database", vApp:=ACApp, vClass:=ACClass, vMethod:="WriteToDb", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: FormCustomResize
	'
	' Description:
	'
	'
	' ***************************************************************** '
	Public Sub FormCustomResize()
		
		Try 
			
			'We've resized the form, so we need to make sure that the buttons and list views
			'stay where they are relative to the form.
			
			'    If (Me.WindowState = vbMinimized) Then
			'        Exit Sub
			'    End If
			
			cmdAddTasks.Left = (tabTask.Width - cmdAddTasks.Width) / 2
			cmdAddAllTasks.Left = (tabTask.Width - cmdAddAllTasks.Width) / 2
			cmdDeleteTasks.Left = (tabTask.Width - cmdDeleteTasks.Width) / 2
			cmdDeleteAllTasks.Left = (tabTask.Width - cmdDeleteAllTasks.Width) / 2
			
			lvwAll.Width = VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(tabTask.Width) - 1785) / 2)
            lvwContents.Width = lvwAll.Width
			lvwContents.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(tabTask.Width) - VB6.PixelsToTwipsX(lvwContents.Width) - 240)
			
			'    If (Me.WindowState = vbMaximized) Then
			'        Exit Sub
			'    End If
			
			'Me.Top = (Screen.Height - Me.Height) / 2
			'Me.Left = (Screen.Width - Me.Width) / 2
		
		Catch excep As System.Exception
			
			
			
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormCustomResizeFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="FormCustomResize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Exit Sub
			
		End Try
		
	End Sub
	
	' ***************************************************************** '
	' Name: NewTask
	'
	' Description:
	'
	'
	' ***************************************************************** '
	Private Function NewTask() As Integer
		
		Dim result As Integer = 0
		Dim oObject As iPMTaskMaintenance.Interface_Renamed
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			oObject = New iPMTaskMaintenance.Interface_Renamed()
			
			m_lReturn = CType(oObject, SSP.S4I.Interfaces.ILocalInterface).Initialise()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				oObject = Nothing
				Return result
			End If
			
			m_lReturn = oObject.NewTask()
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				oObject = Nothing
				Return result
			End If
			
            oObject.Dispose()
			

			oObject = Nothing
			
			m_lReturn = PopListBox()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewTask Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: AddAllTasks
	'
	' Description:
	'
	'
	' ***************************************************************** '
	Private Function AddAllTasks() As Integer
		
		Dim result As Integer = 0
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
            For iTemp As Integer = 1 To lvwAll.Items.Count
                m_lReturn = AddTask(lvwAll.Items.Item(iTemp - 1))
            Next iTemp
			
			m_lReturn = RefreshList()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAllTasks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddAllTasks", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: AddTasks
	'
	' Description:
	'
	'
	' ***************************************************************** '
	Private Function AddTasks() As Integer
		
		Dim result As Integer = 0
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			For iTemp As Integer = 1 To lvwAll.Items.Count
				If lvwAll.Items.Item(iTemp - 1).Selected Then
                    m_lReturn = AddTask(lvwAll.Items.Item(iTemp - 1))
				End If
			Next iTemp
			
			m_lReturn = RefreshList()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTasks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTasks", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	'********************************************************************
	'
	' Function Name: AddTask()
	'
	' Description: Adds task to the database
	'********************************************************************
	
	Private Function AddTask(ByRef oListItem As ListViewItem) As Integer
		
		Dim result As Integer = 0
		Dim lRow As Integer
        Dim sKey As String
        Dim sName As String = ""
		Dim lId As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If oListItem Is Nothing Then
				Return result
			End If
			
			sKey = oListItem.Name
			lRow = CInt(Conversion.Val(sKey.Substring(sKey.Length - (sKey.Length - 1))))
			
			'    sName$ = (lvwGroups.SelectedItem)
			lId = (lRow + 1)
			

			m_lReturn = m_oBusiness.EditInclude(lId)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to ignore selected Task")
				Return result
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			'Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Task", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: DeleteAllTasks
	'
	' Description:
	'
	'
	' ***************************************************************** '
	Private Function DeleteAllTasks() As Integer
		
		Dim result As Integer = 0
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
            For iTemp As Integer = 1 To lvwContents.Items.Count
                m_lReturn = DeleteTask(lvwContents.Items.Item(iTemp - 1))
            Next iTemp
			
			m_lReturn = RefreshList()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteAllTasks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAllTasks", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: DeleteTasks
	'
	' Description:
	'
	'
	' ***************************************************************** '
	Private Function DeleteTasks() As Integer
		
		Dim result As Integer = 0
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			For iTemp As Integer = 1 To lvwContents.Items.Count
                If lvwContents.Items.Item(iTemp - 1).Selected Then
                    m_lReturn = DeleteTask(lvwContents.Items.Item(iTemp - 1))
                End If
			Next iTemp
			
			m_lReturn = RefreshList()
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteTasks Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteTasks", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
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
		Dim lId As Integer
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If oListItem Is Nothing Then
				Return result
			End If
			
			sKey = oListItem.Name
			lRow = CInt(Conversion.Val(sKey.Substring(sKey.Length - (sKey.Length - 1))))
			
			'    sName$ = (lvwGroups.SelectedItem)
			lId = (lRow + 1)
			

			m_lReturn = m_oBusiness.EditIgnore(lId)
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to ignore selected Task")
				Return result
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			'Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Delete Task", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteTask", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
	
    Private Sub cmdAddAllTasks_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddAllTasks.Click

        m_lReturn = AddAllTasks()

    End Sub
	
    Private Sub cmdAddTasks_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddTasks.Click

        m_lReturn = AddTasks()

    End Sub
	
    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        Me.Hide()

    End Sub
	
    Private Sub cmdDeleteAllTasks_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteAllTasks.Click

        m_lReturn = DeleteAllTasks()

    End Sub
	
    Private Sub cmdDeleteTasks_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteTasks.Click

        m_lReturn = DeleteTasks()

    End Sub
	
    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)

        MessageBox.Show("There is no help associated with this screen", "Group Maintenance - Tasks", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub
	
    Private Sub cmdNewTask_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)

        m_lReturn = NewTask()

    End Sub
	
    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        'Set status to PMOK
        m_lStatus = gPMConstants.PMEReturnCode.PMOK

        m_lReturn = WriteToDb()

        'hide this form
        Me.Hide()

    End Sub
	
	Private Sub frmTasks_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			
			With uctPMResizer1
				.SetControlResizeOption("cmdOK", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdCancel", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdHelp", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("tabTask", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				
				.SetControlResizeOption("lvwAll", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROHeightOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("lvwContents", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROHeightOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				
				.SetControlResizeOption("cmdNewTask", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROTopOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
			End With
			
		End If
	End Sub
	
	Private Sub Form_Initialize_Renamed()
		
		' Initialise the error number value.
		m_lError = gPMConstants.PMEReturnCode.PMTrue
		
		'Initialise the form using selected function
		m_lReturn = InitialForm()
		
	End Sub
	

	Private Sub frmTasks_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

		' Check if we have had an error so far.
		' Possibly creating the business object.
		If m_lError = gPMConstants.PMEReturnCode.PMFalse Then
			' We have already encountered an error,
			' so we MUST exit now.
			Exit Sub
		End If
		
		With uctPMResizer1
			.NoResizeByDefault = True
			.FormMinHeight = 6645
			.FormMinWidth = 9405
		End With
		
		'DAK200999
		'    If frmGroup.albTaskImages.IconsSmall.Count > 0 Then
		'        imgGroup.ListImages.Clear
		'        imgGroup.ImageHeight = 16
		'        imgGroup.ImageWidth = 16
		'        For Each oImage In frmGroup.albTaskImages.IconsSmall
		'            imgGroup.ListImages.Add oImage.Index, _
		''                                    oImage.Key, _
		''                                    oImage.Picture
		'        Next oImage
		'    End If
		
		
	End Sub
	
    Private Sub lvwAll_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs)
        Dim ColumnHeader As ColumnHeader = lvwAll.Columns(eventArgs.Column)

        With lvwAll

            ListViewHelper.SetSortedProperty(lvwAll, False)
            ListViewHelper.SetSortKeyProperty(lvwAll, ColumnHeader.Index + 1 - 1)
            ListViewHelper.SetSortedProperty(lvwAll, True)

        End With

    End Sub
	
    Private Sub lvwAll_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs)


        ' Edit details of user if doubleclicked
        Dim oListItem As ListViewItem = lvwAll.GetItemAt(m_lXPos, m_lYPos)

        cmdAddTasks_Click(cmdAddTasks, New EventArgs())

    End Sub
	
    Private Sub lvwAll_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs)
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide no. 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        m_lXPos = CInt(x)
        m_lYPos = CInt(y)

    End Sub
	
    Private Sub lvwContents_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs)
        Dim ColumnHeader As ColumnHeader = lvwContents.Columns(eventArgs.Column)

        With lvwAll

            ListViewHelper.SetSortedProperty(lvwAll, False)
            ListViewHelper.SetSortKeyProperty(lvwAll, ColumnHeader.Index + 1 - 1)
            ListViewHelper.SetSortedProperty(lvwAll, True)

        End With

    End Sub
	
    Private Sub lvwContents_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs)


        ' Edit details of user if doubleclicked
        Dim oListItem As ListViewItem = lvwAll.GetItemAt(m_lXPos, m_lYPos)

        cmdDeleteTasks_Click(cmdDeleteTasks, New EventArgs())

    End Sub
	
    Private Sub lvwContents_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs)
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide no. 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        m_lXPos = CInt(x)
        m_lYPos = CInt(y)

    End Sub

    Private Sub frmTasks_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabTask.SelectedIndex = 0
        End If
    End Sub

  
    Private Sub frmTasks_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        ' MessageBox.Show("Test")
        Try

            'We've resized the form, so we need to make sure that the buttons and list views
            'stay where they are relative to the form.

            '    If (Me.WindowState = vbMinimized) Then
            '        Exit Sub
            '    End If
            Panel4.Width += cmdAddAllTasks.Left + 170
            Panel5.Width = Panel4.Width
            'cmdAddTasks.Left = (tabTask.Width - cmdAddTasks.Width) / 2
            'cmdAddAllTasks.Left = (tabTask.Width - cmdAddAllTasks.Width) / 2
            'cmdDeleteTasks.Left = (tabTask.Width - cmdDeleteTasks.Width) / 2
            'cmdDeleteAllTasks.Left = (tabTask.Width - cmdDeleteAllTasks.Width) / 2

            lvwAll.Width = VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(tabTask.Width) - 1785) / 2)
            lvwContents.Width = lvwAll.Width
            '  lvwContents.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(tabTask.Width) - VB6.PixelsToTwipsX(lvwContents.Width) - 240)

            'If (Me.WindowState = vbMaximized) Then
            '    Exit Sub
            'End If

            'Me.Top = (Screen.Height - Me.Height) / 2
            'Me.Left = (Screen.Width - Me.Width) / 2

        Catch excep As System.Exception




            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormCustomResizeFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="FormCustomResize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub
End Class
