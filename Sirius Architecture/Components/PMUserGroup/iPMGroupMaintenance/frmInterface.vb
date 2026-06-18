Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no.129
Imports SharedFiles

Partial Public Class frmInterface
    Inherits System.Windows.Forms.Form
    ' Edit History:
    ' DAK070999 - Ensure updates are carried out correctly
    ' DAK221199 - Check PrivilegeLevel to see what we can do
    ' DAK011299 - More privilege levels
    ' DAK221299 - Ensure selected item remains seleceted after amendments
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

    Private m_sUserOrGroup As String = ""
    Private m_iGroup As Integer
    Private m_sGroupName As String = ""
    Private m_sGroupDescription As String = ""
    Private m_dEffectiveDate As Date
    Private m_iIsDeleted As gPMConstants.PMEReturnCode
    Private m_iIsSysAdminGroup As Integer

    Private m_lHowMany As Integer

    Private m_lXPos As Integer
    Private m_lYPos As Integer
    Private m_lReturn As Integer
    Private m_bInitialised As Boolean

    ' Public instance of the business object.

    Private m_oBusiness As Object


    'DAK221199
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

                m_lReturn = m_oBusiness.GetNext(vUserOrGroup:=m_sUserOrGroup, vPMUserGroupId:=m_iGroup, vUserGroupCode:=m_sGroupName, vUserGroupDescription:=m_sGroupDescription, vIsDeleted:=m_iIsDeleted, vEffectiveDate:=m_dEffectiveDate, vIsSysAdminGroup:=m_iIsSysAdminGroup)

                'Exit if you get an invalid response
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Do
                End If

                lIndex = (m_iGroup)
                'increment the count by 1
                lRow += 1
                'Populate the listview box
                'DAK221299

                'developer guide no.49
                oListItem = lvwGroups.Items.Insert(lRow, "L" & m_sGroupName.Trim(), m_sGroupName.Trim(), "group")
                With oListItem
                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_sGroupDescription
                    If (m_iIsDeleted = gPMConstants.PMEReturnCode.PMTrue) Or (m_dEffectiveDate > DateTime.Now) Then

                        'developer guide no.13
                        .ForeColor = Color.Gray
                    End If
                    If m_iIsSysAdminGroup = gPMConstants.PMEReturnCode.PMTrue Then
                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = "System Administrator"
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


            m_lReturn = m_oBusiness.GetSystemAdministrators(lHowMany:=m_lHowMany)

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
            'DAK221299
            CurrentKey = sKey
            '    lRow& = Val(Right(sKey$, Len(sKey$) - 1)) + 1
            lRow = oListItem.Index + 1

            'Getdetails from business
            'm_oBusiness.CurrentRecord = lRow - 1


            m_oBusiness.CurrentRecord = Convert.ToString(oListItem.Tag)



            'developer guide no.49
            If lvwGroups.FocusedItem.ImageKey <> "group" Then
                MessageBox.Show("Cannot edit the details for the selected user", "Group Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return result
            End If


            m_lReturn = m_oBusiness.GetNext(vUserOrGroup:=m_sUserOrGroup, vPMUserGroupId:=m_iGroup, vUserGroupCode:=m_sGroupName, vUserGroupDescription:=m_sGroupDescription, vEffectiveDate:=m_dEffectiveDate, vIsSysAdminGroup:=m_iIsSysAdminGroup)

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
                .IsSysAdminGroup = m_iIsSysAdminGroup
                'DAK221199
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

                If lStatus = gPMConstants.PMEReturnCode.PMOK Then

                    m_lReturn = m_oBusiness.EditUpdate(lRow:=lRow, vPMUserGroupId:=.GroupID, vUserGroupCode:=.GroupCode, vUserGroupDescription:=.GroupDescription, vEffectiveDate:=.EffectiveDate, vIsSysAdminGroup:=.IsSysAdminGroup)

                    m_lReturn = WriteToDb()

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
        Dim lStatus As Integer
        Dim lRow As Integer
        Dim sPrinter As String = ""

        Select Case Information.Err().Number
            Case Is < 0
                Conversion.ErrorToString(5)
            Case 1
                GoTo err_AddGroup
        End Select

        result = gPMConstants.PMEReturnCode.PMTrue

        Dim oGroupForm As New frmGroup

        With oGroupForm

            .GroupCode = ""
            .EffectiveDate = DateTime.Now
            'DAK221199
            .PrivilegeLevel = PrivilegeLevel

            .ShowForm(USRAddGroup)

            lStatus = .Status

            lRow = lvwGroups.Items.Count + 1

            If lStatus = gPMConstants.PMEReturnCode.PMOK Then

                '            m_lReturn = m_oBusiness.EditAdd( _
                ''                lRow:=lRow, _
                ''                vUserOrGroup:="group", _
                ''                vPMUserGroupID:=.GroupID, _
                ''                vUserGroupCode:=.GroupCode, _
                ''                vUserGroupDescription:=.GroupDescription, _
                ''                vIsDeleted:=0, _
                ''                vEffectiveDate:=.EffectiveDate)
                '
                '            If (m_lReturn <> PMTrue) Then
                '                Exit Function
                '            End If
                '
                '            m_lReturn = RefreshList()

                'DAK221299
                CurrentKey = "L" & .GroupCode
                m_lReturn = PopListBox()
            End If

        End With

        oGroupForm.Close()
        oGroupForm = Nothing

        Return result

err_AddGroup:

        'Error Section

        result = gPMConstants.PMEReturnCode.PMError

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Group", vApp:=ACApp, vClass:=ACClass, vMethod:="AddGroup", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

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
        Dim lID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If oListItem Is Nothing Then
                Return result
            End If

            sKey = oListItem.Name
            CurrentKey = sKey
            lRow = CInt(oListItem.Tag) + 1

            sName = (lvwGroups.FocusedItem.Text)
            lID = lRow
            If lvwGroups.FocusedItem.ForeColor = Color.Gray Then
                m_lReturn = m_oBusiness.EditUpdate(lRow:=lID, vIsDeleted:=gPMConstants.PMEReturnCode.PMFalse, vEffectiveDate:=DateTime.Now)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Undelete selected Group")
                    Return result
                End If


                'developer guide no.13
                lvwGroups.FocusedItem.ForeColor = Color.Black
                'developer guide no.52
                If lvwGroups.FocusedItem.SubItems(1).Text = "System Administrator" Then
                    m_lHowMany += 1
                End If
            Else

                m_lReturn = m_oBusiness.EditDelete(lID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Delete selected Group")
                    Return result
                End If


                'developer guide no.13
                lvwGroups.FocusedItem.ForeColor = Color.Gray

                'developer guide no.52
                If lvwGroups.FocusedItem.SubItems(1).Text = "System Administrator" Then
                    m_lHowMany -= 1
                End If
            End If

            ' Refresh List & enable apply.
            lvwGroups.Refresh()
            cmdApply.Enabled = True

            ' Select the Item deleted so that the Delete button
            ' caption can be set correctly
            'developer guide no.185
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

                .SetControlResizeOption("tabGroups", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

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

        iPMFunc.ShowFormInTaskBar_Attach()

        'Initialise the form using selected function
        m_lReturn = InitialForm()

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        iPMFunc.ShowFormInTaskBar_Detach()

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

        'DAK221199
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

    End Sub

    Private Sub frmInterface_Closed(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Closed

        'Terminate the form
        m_lReturn = TerminateForm()

    End Sub

    Private Sub lvwGroups_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwGroups.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwGroups.Columns(eventArgs.Column)


        If ColumnHeader.Index = 2 Then
            m_lReturn = ListView6Func.ListViewSortByStringValue(lvwGroups, ColumnHeader.Index, SortOrder.Ascending)
        End If

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

    End Sub

    Private Sub lvwGroups_ItemClick(ByVal Item As ListViewItem)

        'DAK221199
        If PrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupFullPrivileges Then
            cmdDelete.Enabled = True
        End If


        'developer guide no.13
        If lvwGroups.FocusedItem.ForeColor = Color.Gray Then
            cmdDelete.Text = "&Undelete"
            ToolTip1.SetToolTip(cmdDelete, "Undelete Selected User")
            cmdEdit.Enabled = False
        Else
            cmdDelete.Text = "&Delete"
            ToolTip1.SetToolTip(cmdDelete, "Delete Selected User")
            cmdEdit.Enabled = True
            If ListViewHelper.GetListViewSubItem(lvwGroups.FocusedItem, 2).Text = "System Administrator" Then
                If m_lHowMany = 1 Then
                    cmdDelete.Enabled = False
                End If
            End If
        End If

    End Sub

    Private Sub lvwGroups_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwGroups.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no.40
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        m_lXPos = CInt(x)
        m_lYPos = CInt(y)

    End Sub

    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabGroups.SelectedIndex = 0
        End If
    End Sub


    Private Sub lvwGroups_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwGroups.SelectedIndexChanged
        'DAK221199
        If PrivilegeLevel = gPMConstants.PMELookupEditPrivlegeLevel.PMLookupFullPrivileges Then
            cmdDelete.Enabled = True
        End If

        If Not (lvwGroups.FocusedItem Is Nothing) Then
            'developer guide no.13
            If lvwGroups.FocusedItem.ForeColor = Color.Gray Then
                cmdDelete.Text = "&Undelete"
                ToolTip1.SetToolTip(cmdDelete, "Undelete Selected User")
                cmdEdit.Enabled = False
            Else
                cmdDelete.Text = "&Delete"
                ToolTip1.SetToolTip(cmdDelete, "Delete Selected User")
                cmdEdit.Enabled = True
                If ListViewHelper.GetListViewSubItem(lvwGroups.FocusedItem, 2).Text = "System Administrator" Then
                    If m_lHowMany = 1 Then
                        cmdDelete.Enabled = False
                    End If
                End If
            End If
        End If
    End Sub
End Class
