Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide no. 129
Imports SharedFiles
Friend Partial Class frmInterface
	Inherits System.Windows.Forms.Form
	' Edit History:
	' DAK070999 - Ensure updates are carried out correctly
	' DAK111099 - Add Task Group Category
	' DAK231199 - Check PrivilegeLevel to see what we can do
	' DAK011299 - More privilege levels
	' DAK211299 - Remove Task Group Category
	' ***************************************************************** '
	
	Private Const ACClass As String = "frmInterface"
	
	' Object parameter members.
	Private m_sCallingAppName As String = ""
	Private m_lStatus As Integer
	Private m_lError As gPMConstants.PMEReturnCode
	
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	
	Private m_iGroup As Integer
	Private m_sGroupName As String = ""
	Private m_sGroupDescription As String = ""
	Private m_dEffectiveDate As Date
	Private m_iIsDeleted As gPMConstants.PMEReturnCode
	Private m_lDisplayIcon As Integer
	'DAK111099
	' TaskGroupCategoryID
	'DAK211299
	'Private m_lTaskGroupCategoryID As Long
	
	Private m_lXPos As Integer
	Private m_lYPos As Integer
	Private m_lReturn As Integer
	Private m_bInitialised As Boolean
	
	' Public instance of the business object.

	Private m_oBusiness As bPMTaskGroup.Business

	'DAK231199
	' PrivilegeLevel
	Private m_iPrivilegeLevel As Integer
	'DAK221299
	' CurrentKey
	Private m_sCurrentKey As String = ""
	
	' PUBLIC Property Procedures (Begin)
	
	Public ReadOnly Property ErrorNumber() As Integer
		Get
			
			' Standard Property.
			
			' Return any error number that might have
			' occurred on the interface.
			Return m_lError
			
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
	
	'DAK111099
	'DAK211299
	'Public Property Get TaskGroupCategoryID() As Long
	'    TaskGroupCategoryID = m_lTaskGroupCategoryID&
	'End Property
	'Public Property Let TaskGroupCategoryID(lTaskGroupCategoryID As Long)
	'    m_lTaskGroupCategoryID& = lTaskGroupCategoryID&
	'End Property
	'DAK231199
	Public Property PrivilegeLevel() As Integer
		Get
			Return m_iPrivilegeLevel
		End Get
		Set(ByVal Value As Integer)
			m_iPrivilegeLevel = Value
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
	
	Private Function TerminateForm() As Integer
		
		Dim result As Integer = 0
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			' Check if we have an instance of the business object.
			If Not (m_oBusiness Is Nothing) Then
				' Terminate the business object

                m_oBusiness.Dispose()
                ' Destroy the instance of the business object
				' from memory.
				m_oBusiness = Nothing
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			'Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the business", vApp:=ACApp, vClass:=ACClass, vMethod:="TerminateForm", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
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
		Dim lIndex, lRow As Integer
		Dim oListItem As ListViewItem
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'DAK221299
			If Me.Visible Then
				lvwGroups.Focus()
			End If
			

			m_oBusiness.CurrentRecord = 0
			
			'Clear the items in the listview box
			lvwGroups.Items.Clear()
			
			'Set row count
			lRow = -1
			
			Do 
				'Get id of first record

				m_lReturn = m_oBusiness.GetNext(vPMTaskGroupID:=m_iGroup, vTaskGroupCode:=m_sGroupName, vTaskGroupDescription:=m_sGroupDescription, vIsDeleted:=m_iIsDeleted, vEffectiveDate:=m_dEffectiveDate, vDisplayIcon:=m_lDisplayIcon)
				'DAK211299
				'            vTaskGroupCategoryID:=m_lTaskGroupCategoryID)
				
				'Exit if you get an invalid response
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Exit Do
				End If
				
				lIndex = (m_iGroup)
				'increment the count by 1
				lRow += 1
				'Populate the listview box

                oListItem = lvwGroups.Items.Insert(lRow, "L" & m_sGroupName.Trim(), m_sGroupName.Trim(), m_lDisplayIcon - 1)
				With oListItem
					ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_sGroupDescription
					If (m_iIsDeleted = gPMConstants.PMEReturnCode.PMTrue) Or (m_dEffectiveDate > DateTime.Now) Then

                        'Developer Guide no. 13
                        .ForeColor = Color.Gray
                    End If
					oListItem.Tag = CStr(lRow)
				End With
				
			Loop 
			
			'DAK221299
			If lvwGroups.Items.Count = 0 Then
				CurrentKey = ""
				Return result
			End If
			
			If CurrentKey = "" Then
				lvwGroups.FocusedItem = lvwGroups.Items.Item(0)
			Else
				lvwGroups.FocusedItem = lvwGroups.Items.Item(CurrentKey)
			End If
			
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
	' Description: Populates listview box
	'
	'****************************************************************************
	
	Public Function PopListBox() As Integer
		
		Dim result As Integer = 0

		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Getdetails from business

			m_lReturn = m_oBusiness.GetDetails()
			
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
			

			m_lReturn = m_oBusiness.Update
			
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				MessageBox.Show("Changes to Group Details Could not be written to Database", "Group Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)

                m_oBusiness.Dispose()
				m_oBusiness = Nothing
				m_lReturn = InitialForm()
				m_lReturn = PopListBox()
				Return gPMConstants.PMEReturnCode.PMFalse
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
	' Function Name: EditGroup()
	'
	' Description: Writes edited details of an existing user to the database
	'********************************************************************
	
	Private Function EditGroup(ByRef oListItem As ListViewItem) As Integer
		
		Dim result As Integer = 0
		Dim oGroupForm As frmGroup
		Dim lRow As Integer
		Dim sKey As String = ""
		Dim lStatus As Integer

		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			If oListItem Is Nothing Then
				Return result
			End If
			
			' If User is Deleted/Marked for Deletion then exit

            'Developer Guide no. 13
            If oListItem.ForeColor = Color.Gray Then
                Return result
            End If

            sKey = oListItem.Name
            'DAK221299
            CurrentKey = sKey
            '    lRow& = Val(Right(sKey$, Len(sKey$) - 1)) + 1
            lRow = oListItem.Index + 1

            'Getdetails from business

            'm_oBusiness.CurrentRecord = lRow - 1


            m_oBusiness.CurrentRecord = Convert.ToString(oListItem.Tag)

            m_lReturn = m_oBusiness.GetNext(vPMTaskGroupId:=m_iGroup, vTaskGroupCode:=m_sGroupName, vTaskGroupDescription:=m_sGroupDescription, vEffectiveDate:=m_dEffectiveDate, vDisplayIcon:=m_lDisplayIcon)
            'DAK211299
            '        vTaskGroupCategoryID:=m_lTaskGroupCategoryID)

            'Exit if you get an invalid response
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            oGroupForm = New frmGroup()

            With oGroupForm

                .GroupID = m_iGroup
                .GroupCode = m_sGroupName
                .GroupDescription = m_sGroupDescription
                .EffectiveDate = m_dEffectiveDate
                .DisplayIcon = m_lDisplayIcon
                'DAK211299
                '        .TaskGroupCategoryID = m_lTaskGroupCategoryID
                'DAK231199
                .PrivilegeLevel = PrivilegeLevel

                'Populate the listbox using the selected function
                m_lReturn = .PopListBox()

                ' Check for errors.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lError = gPMConstants.PMEReturnCode.PMFalse

                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Return result
                End If

                .ShowForm(USREditGroup)

                lStatus = .Status

                If lStatus <> gPMConstants.PMEReturnCode.PMOK Then

                    If Me.Visible Then
                        lvwGroups.Focus()
                    End If

                Else


                    m_lReturn = m_oBusiness.EditUpdate(lRow:=lRow, vPMTaskGroupId:=.GroupID, vTaskGroupCode:=.GroupCode, vTaskGroupDescription:=.GroupDescription, vEffectiveDate:=.EffectiveDate, vDisplayIcon:=.DisplayIcon)
                    'DAK211299
                    '                vTaskGroupCategoryID:=.TaskGroupCategoryID)

                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                        m_lReturn = WriteToDb()

                    End If

                    '            m_lReturn = PopListBox

                End If

            End With

            oGroupForm.Close()

            oGroupForm = Nothing

            Return result

        Catch excep As System.Exception



            'Error Section

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Edit Group Details", vApp:=ACApp, vClass:=ACClass, vMethod:="EditGroup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
	End Function
	
	'********************************************************************
	'
	' Function Name: AddGroup()
	'
	' Description: Adds new user to the database
	'********************************************************************
	
	Private Function AddGroup() As Integer
		
		Dim result As Integer = 0
		Dim oGroupForm As frmGroup
		Dim lStatus As Integer
        Dim sPrinter As String = ""
		
		'MKR 13/10/2004 -- Corrected Spelling mistake (Changed Err to Error)
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			oGroupForm = New frmGroup()


			With oGroupForm
				
				.GroupCode = ""
				.EffectiveDate = DateTime.Now
				.DisplayIcon = 1
				'DAK211299
				'        .TaskGroupCategoryID = 1
				'DAK231199
				.PrivilegeLevel = PrivilegeLevel
				
				.ShowForm(USRAddGroup)
				
				lStatus = .Status
				
				If lStatus = gPMConstants.PMEReturnCode.PMOK Then
					'DAK221299
					CurrentKey = "L" & .GroupCode
				End If
				
				m_lReturn = PopListBox()
				
			End With
			
			oGroupForm.Close()
			oGroupForm = Nothing
			
			'DAK221299
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				Return gPMConstants.PMEReturnCode.PMFalse
			End If
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			'Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Group", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGroup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	'********************************************************************
	'
	' Function Name: DeleteGroup()
	'
	' Description: Deletes group from the database
	'********************************************************************
	
	Private Function DeleteGroup(ByRef oListItem As ListViewItem) As Integer
		
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
			'DAK221299
			CurrentKey = sKey
			'    lRow& = Val(Right(sKey$, Len(sKey$) - 1))
			lRow = oListItem.Index + 1
			
			sName = (lvwGroups.FocusedItem.Text)
			'DAK221299
			'    lID& = (lRow& + 1)
			lId = lRow
			

            'Developer Guide no. 13
            If lvwGroups.FocusedItem.ForeColor = Color.Gray Then

                m_lReturn = m_oBusiness.EditUpdate(lRow:=lId, vIsDeleted:=gPMConstants.PMEReturnCode.PMFalse, vEffectiveDate:=DateTime.Now)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Undelete selected Group")
                    Return result
                End If


                'Developer Guide no. 13
                lvwGroups.FocusedItem.ForeColor = Color.Black
            Else

                m_lReturn = m_oBusiness.EditDelete(lId)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Delete selected Group")
                    Return result
                End If


                'Developer Guide no. 13
                lvwGroups.FocusedItem.ForeColor = Color.Gray

            End If

            ' Refresh List & enable apply.
            lvwGroups.Refresh()
            cmdApply.Enabled = True

            ' Select the Item deleted so that the Delete button
            ' caption can be set correctly
            'Developer Guide no.185
            lvwGroups_ItemClick(lvwGroups.Items.Item(lvwGroups.FocusedItem.Index))

            Return result

        Catch excep As System.Exception
			
			
			
			'Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Delete Group", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteGroup", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
		End Try
	End Function
	
	' ***************************************************************** '
	' Name: CheckGroupName
	'
	' Description:
	'
	'
	' ***************************************************************** '
	Public Function CheckGroupName(ByVal v_m_sGroupName As String) As Integer
		
		Dim result As Integer = 0
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			For lRow As Integer = 1 To lvwGroups.Items.Count
				If v_m_sGroupName.Trim() = lvwGroups.Items.Item(lRow - 1).Text.Trim() Then
					Return gPMConstants.PMEReturnCode.PMFalse
				End If
			Next 
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckGroupNameFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckGroupName", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click

        m_lReturn = AddGroup()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            MessageBox.Show("Failed to show Add Group screen", "Group Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Exit Sub
        End If

    End Sub
	
    Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click

        m_lReturn = WriteToDb()

    End Sub
	
    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        Me.Hide()

    End Sub
	
    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click

        'delete selected user using the DeleteGroup function
        m_lReturn = DeleteGroup(lvwGroups.FocusedItem)

    End Sub
	
    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        'Edit selected user using the EditGroup function
        m_lReturn = EditGroup(lvwGroups.FocusedItem)

        m_lReturn = PopListBox()

    End Sub
	
	'UPGRADE_NOTE: (7001) The following declaration (cmdHelp_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
	'Private Sub cmdHelp_Click()
		'
		'MessageBox.Show("There is no help associated with this screen", "Group Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
		'
	'End Sub
	
    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        'DAK070999
        m_lReturn = WriteToDb()
        '
        '    If (m_lReturn <> PMTrue) Then
        '        Exit Sub
        '    End If
        '
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
				.SetControlResizeOption("cmdApply", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("SSTab1", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				
				.SetControlResizeOption("cmdAdd", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdEdit", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdDelete", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROLeftOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				
				.SetControlResizeOption("lvwGroups", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
			End With
			
			lvwGroups.Focus()
			
		End If
	End Sub
	
	Private Sub Form_Initialize_Renamed()
		
		' Initialise the error number value.
		m_lError = gPMConstants.PMEReturnCode.PMTrue
		
		'Initialise the form using selected function
		m_lReturn = InitialForm()
		
	End Sub
	

	Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
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
			m_lError = gPMConstants.PMEReturnCode.PMFalse
			
			' Set the mouse pointer to normal.
			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
			
			Exit Sub
		End If
        lvwGroups.Items(0).Selected = True
       


	End Sub
	
	Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed
		
		'Terminate the form
		m_lReturn = TerminateForm()
		
	End Sub
	
    Private Sub lvwGroups_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwGroups.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwGroups.Columns(eventArgs.Column)



        If lvwGroups.Sorting = SortOrder.Ascending Then
            lvwGroups.Sorting = SortOrder.Descending
            ListViewHelper.SetSortOrderProperty(lvwGroups, SortOrder.Descending)
            ListViewHelper.SetSortedProperty(lvwGroups, False)

        Else
            ListViewHelper.SetSortOrderProperty(lvwGroups, SortOrder.Ascending)
            lvwGroups.Sorting = SortOrder.Ascending
            ListViewHelper.SetSortedProperty(lvwGroups, True)
        End If
        ListViewHelper.SetSortKeyProperty(lvwGroups, ColumnHeader.Index + 1 - 1)


    End Sub
	
    Private Sub lvwGroups_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwGroups.DoubleClick


        ' Edit details of user if doubleclicked
        Dim oListItem As ListViewItem = lvwGroups.GetItemAt(m_lXPos, m_lYPos)

        m_lReturn = EditGroup(oListItem)

        m_lReturn = PopListBox()

    End Sub
	

    Private Sub lvwGroups_ItemClick(ByVal Item As ListViewItem)

        'DAK231199
        If PrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupFullPrivileges Then
            cmdDelete.Enabled = True
        End If


        'Developer Guide no. 13
        If Item.ForeColor = Color.Gray Then
            cmdDelete.Text = "&Undelete"
            ToolTip1.SetToolTip(cmdDelete, "Undelete Selected User")
            cmdEdit.Enabled = False
        Else
            cmdDelete.Text = "&Delete"
            ToolTip1.SetToolTip(cmdDelete, "Delete Selected User")
            cmdEdit.Enabled = True
        End If

    End Sub

    Private Sub lvwGroups_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwGroups.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide no. 70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        m_lXPos = CInt(x)
        m_lYPos = CInt(y)

    End Sub

    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            SSTab1.SelectedIndex = 0
        End If
    End Sub

    Private Sub lvwGroups_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwGroups.SelectedIndexChanged

        If PrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupFullPrivileges Then
            cmdDelete.Enabled = True
        End If


        'Developer Guide no. 13
        If lvwGroups.SelectedItems.Count > 0 Then
            If lvwGroups.SelectedItems.Item(0).ForeColor = Color.Gray Then
                cmdDelete.Text = "&Undelete"
                ToolTip1.SetToolTip(cmdDelete, "Undelete Selected User")
                cmdEdit.Enabled = False
            Else
                cmdDelete.Text = "&Delete"
                ToolTip1.SetToolTip(cmdDelete, "Delete Selected User")
                cmdEdit.Enabled = True
            End If
        End If
    End Sub

  
    Private Sub cmdDelete_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdDelete.GotFocus
        lvwGroups.TabStop = False
        cmdOK.TabStop = True
        cmdCancel.TabStop = True
    End Sub

    

    Private Sub lvwGroups_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwGroups.GotFocus
        cmdEdit.TabStop = True
        cmdAdd.TabStop = True
        cmdDelete.TabStop = True
        cmdOK.TabStop = False
        cmdCancel.TabStop = False
    End Sub

    Private Sub cmdCancel_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdCancel.GotFocus
        lvwGroups.TabStop = True
        cmdEdit.TabStop = False
        cmdAdd.TabStop = False
        cmdDelete.TabStop = False
    End Sub
End Class
