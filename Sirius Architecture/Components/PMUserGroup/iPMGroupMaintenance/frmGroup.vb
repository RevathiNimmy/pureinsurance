Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles
Friend Partial Class frmGroup
	Inherits System.Windows.Forms.Form
	' Edit History:
	' DAK070999 - Ensure updates are carried out correctly
	' DAK221199 - Check PrivilegeLevel to see what we can do
	' DAK011299 - View only privilege added
	' DAK221299 - Ensure selected item remains seleceted after amendments
	' ***************************************************************** '

    'Declarations
	
	Private m_lStatus As Integer
	
	Private m_lError As gPMConstants.PMEReturnCode
	
	Private m_lFormMode As Integer
	

    Private m_oBusiness As Object

	Private m_sMasterUserOrGroup As String = ""
	Private m_lMasterGroupId As Integer
	Private m_sMasterGroupCode As String = ""
	Private m_sMasterGroupDescription As String = ""
	Private m_dMasterEffectiveDate As Date
	Private m_iMasterIsSysAdminGroup As Integer
	
	Private m_sUserOrGroup As String = ""
	Private m_lGroupID As Integer
	Private m_sGroupCode As String = ""
	Private m_sGroupDescription As String = ""
	Private m_dEffectiveDate As Date
	Private m_iIsDeleted As gPMConstants.PMEReturnCode
	Private m_iIsSysAdminGroup As Integer
	Private m_iIsSupervisor As gPMConstants.PMEReturnCode
	
	Private m_lHowMany As Integer
	
	Private m_lXPos As Integer
	Private m_lYPos As Integer
	Private m_lReturn As Integer
	Private m_bInitialised As Boolean
	'DAK221199
	' PrivilegeLevel
	Private m_iPrivilegeLevel As gPMConstants.PMELookupEditPrivlegeLevel
	'DAK221299
	' CurrentKey
	Private m_sCurrentKey As String = ""
	
	Private Const ACClass As String = "frmGroup"
	
	
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
	
	
	Public Property IsSysAdminGroup() As Integer
		Get
			
			Return m_iMasterIsSysAdminGroup
			
		End Get
		Set(ByVal Value As Integer)
			
			m_iMasterIsSysAdminGroup = Value
			
		End Set
	End Property
	
	Public ReadOnly Property Status() As Integer
		Get
			
			Return m_lStatus
			
		End Get
	End Property
	
	
	Private Property FormMode() As Integer
		Get
			
			Return m_lFormMode
			
		End Get
		Set(ByVal Value As Integer)
			
			m_lFormMode = Value
			
		End Set
	End Property
	
	'DAK221199
	Public Property PrivilegeLevel() As Integer
		Get
			Return m_iPrivilegeLevel
		End Get
		Set(ByVal Value As Integer)
			m_iPrivilegeLevel = Value
		End Set
	End Property
	
	'DAK221299
	Public Property CurrentKey() As String
		Get
			Return m_sCurrentKey.Trim()
		End Get
		Set(ByVal Value As String)
			m_sCurrentKey = Value.Trim()
		End Set
	End Property
	
	'*************************************************************
	'
	' Function Name:ShowForm()
	'
	' Description: Shows form details which correspond with what
	'              the user has selected from the previous form
	'*************************************************************
	
	Public Function ShowForm(ByRef lEditMode As Integer) As Integer
		
		Dim result As Integer = 0
			
		Try 
			
			FormMode = lEditMode
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			txtGroupName.Text = m_sMasterGroupCode.Trim()
			txtGroupDescription.Text = m_sMasterGroupDescription.Trim()

            'developer guide no.26
            lblpEffectiveDate.Text = StringsHelper.Format(m_dMasterEffectiveDate, "general date")
			chkIsSysAdminGroup.CheckState = m_iMasterIsSysAdminGroup
			
			'DAK221199
			If PrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAmendCaptions Then
				
				chkIsSysAdminGroup.Enabled = False
				lvwGroups.Enabled = False
				cmdMembers.Enabled = False
				cmdTasks.Enabled = False
				
				'DAK011299
			ElseIf PrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupViewOnly Then 
				
				txtGroupDescription.Enabled = False
				chkIsSysAdminGroup.Enabled = False
				lvwGroups.Enabled = False
				cmdMembers.Enabled = False
				cmdTasks.Enabled = False
				
			Else
				
				If (m_lHowMany > 1) Or (m_iMasterIsSysAdminGroup = 0) Then
					chkIsSysAdminGroup.Enabled = True
					ToolTip1.SetToolTip(chkIsSysAdminGroup, "Is this Group a System Administrator")
				Else
					chkIsSysAdminGroup.Enabled = False
					ToolTip1.SetToolTip(chkIsSysAdminGroup, "This Group is the only System Administrator - cannot delete")
				End If
				lvwGroups.Enabled = True
				cmdMembers.Enabled = True
				cmdTasks.Enabled = True
				
			End If
			
			Select Case lEditMode
				Case USRAddGroup
					'Give some names
					Me.Text = "Add User Group"
					
					pnlEffectiveDate.BackColor = SystemColors.Control
					
					cmdMembers.Enabled = False
					cmdTasks.Enabled = False
					
					ToolTip1.SetToolTip(cmdOK, "Accepts entry of new user group")
					ToolTip1.SetToolTip(cmdCancel, "Cancels entry of new user group")
					
					cmdOK.Text = "&Apply"
					
				Case USREditGroup
					'Give some names
					Me.Text = m_sMasterGroupCode.Trim() & " Properties"
					
					'Set some defaults
					txtGroupName.BackColor = SystemColors.Control
					pnlEffectiveDate.BackColor = SystemColors.Control
					
					txtGroupName.Enabled = False
					pnlEffectiveDate.Enabled = False
					
					ToolTip1.SetToolTip(cmdOK, "Accept Changes and return to previous screen")
					ToolTip1.SetToolTip(cmdCancel, "Cancel Changes and return to previous screen")
					
					cmdOK.Text = "&OK"
					
			End Select
			
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
			m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bPMUserGroup.Business", vInstanceManager:="ClientManager")
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
	' Function Name: Refreshlist
	'
	' Description: Refreshes the listview box with data held in
	'              the business, not the database.
	'****************************************************************************
	
	Private Function RefreshList() As Integer
		
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

				m_lReturn = m_oBusiness.GetNext(vUserOrGroup:=m_sUserOrGroup, vPMUserGroupID:=m_lGroupID, vUserGroupCode:=m_sGroupCode, vUserGroupDescription:=m_sGroupDescription, vIsDeleted:=m_iIsDeleted, vEffectiveDate:=m_dEffectiveDate, vIsSysAdminGroup:=m_iIsSysAdminGroup, vIsSupervisor:=m_iIsSupervisor)
				
				'Exit if you get an invalid response
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					Exit Do
				End If
				
				lIndex = (m_lGroupID)
				'increment the count by 1
				lRow += 1
				'Populate the listview box
				'DAK221299
				' RDC 09052003 add lRow to key to make unique (ie. if user and user group have same name)

                'developer guide no.49
                oListItem = lvwGroups.Items.Insert(lRow, "L" & m_sGroupCode.Trim() & CStr(lRow), m_sGroupCode.Trim(), m_sUserOrGroup)
				With oListItem
					ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_sGroupDescription
					
					If (m_iIsDeleted = gPMConstants.PMEReturnCode.PMTrue) Or (m_dEffectiveDate > DateTime.Now) Then

                        'developer guide no.13
                        .ForeColor = Color.Gray
					End If
					
					If m_iIsSysAdminGroup = gPMConstants.PMEReturnCode.PMTrue Then
						ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "System Administrator"
					Else
						ListViewHelper.GetListViewSubItem(oListItem, 2).Text = ""
					End If
					
					If m_iIsSupervisor = gPMConstants.PMEReturnCode.PMTrue Then

                        'developer guide no.49
                        oListItem.ImageKey = "supervisor"
						ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "Supervisor"
					End If
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

            m_lReturn = m_oBusiness.GetDetails(GroupID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'DAK221299
                Return RefreshList()
            End If


            m_lReturn = m_oBusiness.GetSystemAdministrators(lHowMany:=m_lHowMany)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'DAK221299
                Return RefreshList()
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
				Return result
			End If
			
			m_lReturn = PopListBox()
			
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

            'developer guide no.13
            If oListItem.ForeColor = Color.Gray Then
                Return result
            End If

            sKey = oListItem.Name

            CurrentKey = sKey
            lRow = oListItem.Index + 1

            'Getdetails from business

            m_oBusiness.CurrentRecord = lRow - 1

            If lvwGroups.FocusedItem.ImageKey <> "group" Then
                MessageBox.Show("Cannot edit the details for the selected group", "Group Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return result
            End If

            m_lReturn = m_oBusiness.GetNext(vPMUserGroupId:=m_lGroupID, vUserGroupCode:=m_sGroupCode, vUserGroupDescription:=m_sGroupDescription, vEffectiveDate:=m_dEffectiveDate)

            'Exit if you get an invalid response
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            oGroupForm = New frmGroup()

            With oGroupForm

                .GroupID = m_lGroupID
                .GroupCode = m_sGroupCode
                .GroupDescription = m_sGroupDescription
                .EffectiveDate = m_dEffectiveDate
                .IsSysAdminGroup = m_iIsSysAdminGroup

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

                If lStatus = gPMConstants.PMEReturnCode.PMOK Then

                    m_lReturn = m_oBusiness.EditUpdate(lRow:=lRow, vPMUserGroupId:=.GroupID, vUserGroupCode:=.GroupCode, vUserGroupDescription:=.GroupDescription, vEffectiveDate:=.EffectiveDate, vIsSysAdminGroup:=.IsSysAdminGroup)

                    m_lReturn = WriteToDb()

                End If

            End With

            oGroupForm.Close()

            oGroupForm = Nothing

            'Let's repopulate the box, as things may have changed
            m_lReturn = PopListBox()

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
	' Function Name: EditMembership()
	'
	' Description: Calls the screen that allows maintainence of group membership
	'********************************************************************
	
	Private Function EditMembership() As Integer
		
		Dim result As Integer = 0
		Dim oMembersForm As frmMembers
		Dim lStatus As gPMConstants.PMEReturnCode
		Dim iPrivilegeLevel As gPMConstants.PMELookupEditPrivlegeLevel
		
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			oMembersForm = New frmMembers()

			With oMembersForm
				
				.GroupID = m_lMasterGroupId
				.GroupCode = m_sMasterGroupCode
				.GroupDescription = m_sMasterGroupDescription
				.EffectiveDate = m_dMasterEffectiveDate
				.IsSysAdminGroup = m_iMasterIsSysAdminGroup
				'DAK011299
				m_lReturn = GetPrivileges(iPrivilegeLevel)
				.cmdNewUser.Enabled = Not (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or iPrivilegeLevel <> gPMConstants.PMELookupEditPrivlegeLevel.PMLookupFullPrivileges)
				
				'Populate the listboxes using the selected function
				m_lReturn = .PopListBox()
				
				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					m_lError = gPMConstants.PMEReturnCode.PMFalse
					
					' Set the mouse pointer to normal.
					iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
					
					Return result
				End If
				
				.ShowForm()
				
				lStatus = .Status
				
				If lStatus = gPMConstants.PMEReturnCode.PMOK Then
					m_lReturn = PopListBox()
				End If
				
			End With
			
			oMembersForm.Close()
			
			oMembersForm = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			'Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Edit Group Membership Details", vApp:=ACApp, vClass:=ACClass, vMethod:="EditMembership", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	'********************************************************************
	'
	' Function Name: EditTasks()
	'
	' Description: Calls the screen that allows maintainence of tasks
	'********************************************************************
	
	Private Function EditTasks() As Integer
		
		Dim result As Integer = 0
		Dim oTasksForm As frmTasks
		Dim lStatus As gPMConstants.PMEReturnCode
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			oTasksForm = New frmTasks()

			
			With oTasksForm
				
				.GroupID = m_lMasterGroupId
				.GroupCode = m_sMasterGroupCode
				.GroupDescription = m_sMasterGroupDescription
				.EffectiveDate = m_dMasterEffectiveDate
				.IsSysAdminGroup = m_iMasterIsSysAdminGroup
				
				'Populate the listboxes using the selected function
				m_lReturn = .PopListBox()
				
				' Check for errors.
				If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
					m_lError = gPMConstants.PMEReturnCode.PMFalse
					
					' Set the mouse pointer to normal.
					iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
					
					Return result
				End If
				
				.ShowForm()
				
				lStatus = .Status
				
				If lStatus = gPMConstants.PMEReturnCode.PMOK Then
					
				End If
				
			End With
			
			oTasksForm.Close()
			
			oTasksForm = Nothing
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			'Error Section
			
			result = gPMConstants.PMEReturnCode.PMError
			
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Edit Group Task Details", vApp:=ACApp, vClass:=ACClass, vMethod:="EditTasks", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
	' ***************************************************************** '
	'
	' Name: GetPrivileges
	'
	' Description:
	'
	' History: 01/12/1999 DAK - Created.
	'
	' ***************************************************************** '
	Public Function GetPrivileges(ByRef r_iPrivilegeLevel As Integer) As Integer
		Dim result As Integer = 0
	    Dim oBusiness As Object
		Dim bIsAdministrator As Boolean
		Dim vSupervisedGroups As Object
		
		
		Try 
			
			result = gPMConstants.PMEReturnCode.PMTrue
			
			'Get the Business object
			Dim temp_oBusiness As Object
			m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bPMUser.Business", vInstanceManager:="ClientManager")
			oBusiness = temp_oBusiness
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				result = gPMConstants.PMEReturnCode.PMFalse

                oBusiness.Dispose()
				oBusiness = Nothing
				
				gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get bPMUser.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPrivileges")
				
				Return result
			End If
			

			m_lReturn = oBusiness.GetPrivilegeLevel(r_iPrivilegeLevel:=r_iPrivilegeLevel)
			If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
				
				result = gPMConstants.PMEReturnCode.PMFalse

                oBusiness.Dispose()
				oBusiness = Nothing
				
				Return result
			End If
			
			If r_iPrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupFullPrivileges Or m_iPrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAmendCaptions Or m_iPrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupViewOnly Or m_iPrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupNoEdit Then
				

                oBusiness.Dispose()
				oBusiness = Nothing
				Return result
				
			End If
			

			m_lReturn = oBusiness.GetUserAuthority(r_bIsAdministrator:=bIsAdministrator, r_vSupervisedGroups:=vSupervisedGroups)

            oBusiness.Dispose()
            oBusiness = Nothing
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

			If Not bIsAdministrator Then
				
				
				Select Case m_iPrivilegeLevel
					Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminCaptionsUserView, gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminFullUserView
						
						m_iPrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupViewOnly
						
					Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminFullUserCaptions
						
						m_iPrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAmendCaptions
						
					Case Else
						
						m_iPrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupNoEdit
						
				End Select
				
			Else
				
				
				Select Case m_iPrivilegeLevel
					Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminViewUserNone
						
						m_iPrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupViewOnly
						
					Case gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminCaptionsUserView, gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAdminCaptionsUserNone
						
						m_iPrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupAmendCaptions
						
					Case Else
						
						m_iPrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupFullPrivileges
						
				End Select
				
			End If
			
			
			Return result
		
		Catch excep As System.Exception
			
			
			
			result = gPMConstants.PMEReturnCode.PMError
			
			' Log Error Message
			iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPrivileges Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPrivileges", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
			
			Return result
			
		End Try
	End Function
	
    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        Me.Hide()

    End Sub
	
    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        MessageBox.Show("There is no help associated with this screen", "Group Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
        'DAK221299
        If Me.Visible Then
            lvwGroups.Focus()
        End If

    End Sub
	
    Private Sub cmdMembers_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdMembers.Click

        m_lReturn = EditMembership()
        'DAK221299
        If Me.Visible Then
            lvwGroups.Focus()
        End If

    End Sub
	
    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim lReturn As gPMConstants.PMEReturnCode

        m_sMasterGroupCode = txtGroupName.Text.Trim()
        m_sMasterGroupDescription = txtGroupDescription.Text.Trim()

        m_dMasterEffectiveDate = CDate(lblpEffectiveDate.Text)
        m_iMasterIsSysAdminGroup = chkIsSysAdminGroup.CheckState

        ' Check to see if a Group name has been entered
        If m_sMasterGroupCode = "" Then

            MessageBox.Show("Group name not entered.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            SSTabHelper.SetSelectedIndex(tabGroup, 0)
            txtGroupName.Focus()
            Exit Sub

        End If

        If FormMode = USRAddGroup Then
            ' Check for Duplicate GroupCodes
            lReturn = CType(m_ofrmInterface.CheckGroupName(m_sMasterGroupCode), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Group name " & m_sGroupCode & _
                                " already exists." & Strings.Chr(13) & Strings.Chr(10) & "Please Choose another Group name.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                txtGroupName.Focus()
                txtGroupName.Text = ""
                Exit Sub
            End If


            m_lReturn = m_oBusiness.EditAdd(lRow:=1, vUserOrGroup:="group", vPMUserGroupId:=1, vUserGroupCode:=m_sMasterGroupCode, vUserGroupDescription:=m_sMasterGroupDescription, vIsDeleted:=0, vEffectiveDate:=m_dMasterEffectiveDate, vIsSysAdminGroup:=m_iMasterIsSysAdminGroup)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            m_lReturn = m_oBusiness.Update()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'Get the ID...

            m_oBusiness.CurrentRecord = 0
            m_lReturn = m_oBusiness.GetNext(vPMUserGroupId:=m_lMasterGroupId)

            'Make it edit, not read
            FormMode = USREditGroup

            'Turn the buttons back on
            cmdMembers.Enabled = True
            cmdTasks.Enabled = True

            ToolTip1.SetToolTip(cmdOK, "Accept Changes and return to previous screen")
            ToolTip1.SetToolTip(cmdCancel, "Cancel Changes and return to previous screen")

            cmdOK.Text = "&OK"

            txtGroupName.Enabled = False
            pnlEffectiveDate.Enabled = False

            Exit Sub
        Else

            'Set status to PMOK
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            'Enable the apply button
            'frmInterface.cmdApply.Enabled = True

            'hide this form
            Me.Hide()

        End If

    End Sub
	
    Private Sub cmdTasks_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdTasks.Click

        m_lReturn = EditTasks()
        'DAK221299
        If Me.Visible Then
            lvwGroups.Focus()
        End If

    End Sub
	
	Private Sub frmGroup_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
		If Not (ActivateHelper.myActiveForm Is eventSender) Then
			ActivateHelper.myActiveForm = eventSender
			
			With uctPMResizer1
				.SetControlResizeOption("cmdOK", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdCancel", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdHelp", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				
				.SetControlResizeOption("tabGroup", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				
				.SetControlResizeOption("cmdMembers", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROTopOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				.SetControlResizeOption("cmdTasks", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				
				.SetControlResizeOption("lvwGroups", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
				
			End With
			
		End If
	End Sub
	
	Private Sub Form_Initialize_Renamed()
		
		' Initialise the error number value.
		m_lError = gPMConstants.PMEReturnCode.PMTrue
		
		'Initialise the form using selected function
		m_lReturn = InitialForm()
		
	End Sub
	

	Private Sub frmGroup_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
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
		
	End Sub
	
    Private Sub lvwGroups_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwGroups.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwGroups.Columns(eventArgs.Column)

        With lvwGroups

            ListViewHelper.SetSortedProperty(lvwGroups, False)
            ListViewHelper.SetSortKeyProperty(lvwGroups, ColumnHeader.Index + 1 - 1)
            ListViewHelper.SetSortedProperty(lvwGroups, True)

        End With

    End Sub
	
    Private Sub lvwGroups_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwGroups.DoubleClick


        ' Edit details of user if doubleclicked
        Dim oListItem As ListViewItem = lvwGroups.GetItemAt(m_lXPos, m_lYPos)

        m_lReturn = EditGroup(oListItem)

    End Sub
	
    Private Sub lvwGroups_ItemClick(ByVal Item As ListViewItem)

        'developer guide no.13
        If Item.ForeColor = Color.Gray Then
            '        cmdDelete.Caption = "&Undelete"
            '        cmdDelete.ToolTipText = "Undelete Selected User"
            '        cmdEdit.Enabled = False
            cmdMembers.Enabled = False
        Else
            '        cmdDelete.Caption = "&Delete"
            '        cmdDelete.ToolTipText = "Delete Selected User"
            '        cmdEdit.Enabled = True
            cmdMembers.Enabled = True
        End If
    End Sub
	
    Private Sub lvwGroups_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwGroups.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no.70
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        m_lXPos = CInt(x)
        m_lYPos = CInt(y)

    End Sub

    Private Sub frmGroup_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabGroup.SelectedIndex = 0
        End If
    End Sub

   
End Class
