Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms

Imports SharedFiles
Partial Friend Class frmMembers
    Inherits System.Windows.Forms.Form

   

    'PSL 10/10/2002 Issue 1006 Added Authority column to lvwAll

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
    Private m_iIncluded As Integer
    Private m_iIsSupervisor As gPMConstants.PMEReturnCode

    Private m_lXPos As Integer
    Private m_lYPos As Integer
    Private m_lReturn As Integer
    Private m_bInitialised As Boolean

    Private Const ACClass As String = "frmMembers"
    Private m_oListItem As ListViewItem

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

            Me.Text = "Edit Members of " & m_sMasterGroupCode.Trim()

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

                m_lReturn = m_oBusiness.GetNext(vUserOrGroup:=m_sUserOrGroup, vPMUserGroupID:=m_lGroupID, vUserGroupCode:=m_sGroupCode, vUserGroupDescription:=m_sGroupDescription, vIsDeleted:=m_iIsDeleted, vEffectiveDate:=m_dEffectiveDate, vIncluded:=m_iIncluded, vIsSupervisor:=m_iIsSupervisor)

                'Exit if you get an invalid response
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Do
                End If

                lIndex = (m_lGroupID)
                'increment the count by 1
                lRow += 1
                'Populate the listview box


                oListItem = lvwAll.Items.Add("L" & lRow, m_sGroupCode.Trim(), m_sUserOrGroup)
                With oListItem
                    If m_sUserOrGroup = "group" Then
                        'CHECK_PSL
                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_sGroupDescription
                    End If
                    If (m_iIsDeleted = gPMConstants.PMEReturnCode.PMTrue) Or (m_dEffectiveDate > DateTime.Now) Then


                        .ForeColor = Color.Gray
                    End If
                End With

                oListItem = Nothing

                If m_iIncluded = 1 Then

                    oListItem = lvwContents.Items.Add("L" & lRow, m_sGroupCode.Trim(), m_sUserOrGroup)
                    With oListItem
                        If m_sUserOrGroup = "group" Then
                            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = m_sGroupDescription
                        End If

                        If (m_iIsDeleted = gPMConstants.PMEReturnCode.PMTrue) Or (m_dEffectiveDate > DateTime.Now) Then
                            .ForeColor = Color.Gray
                        End If

                        If m_iIsSupervisor = gPMConstants.PMEReturnCode.PMTrue Then
                            oListItem.ImageKey = "supervisor"
                            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = "Supervisor"
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

            m_lReturn = m_oBusiness.GetAllUsersAndGroups(lgroupid:=m_lMasterGroupId)

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


            m_lReturn = m_oBusiness.UpdateMemberships(lgroupid:=m_lMasterGroupId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Changes to Group Membership Details Could not be written to Database", "Group Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)

                m_oBusiness.Dispose()
                m_oBusiness = Nothing
                m_lReturn = InitialForm()
                m_lReturn = PopListBox()
                Return result
            End If

            ' m_lReturn = PopListBox()

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

            cmdAddMembers.Left = (tabMembers.Width - cmdAddMembers.Width) / 2
            cmdAddAllMembers.Left = (tabMembers.Width - cmdAddAllMembers.Width) / 2
            cmdDeleteMembers.Left = (tabMembers.Width - cmdDeleteMembers.Width) / 2
            cmdDeleteAllMembers.Left = (tabMembers.Width - cmdDeleteAllMembers.Width) / 2

            lvwAll.Width = VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(tabMembers.Width) - 1785) / 2)

            lvwContents.Width = lvwAll.Width
            lvwContents.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(tabMembers.Width) - VB6.PixelsToTwipsX(lvwContents.Width) - 240)

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
    ' Name: NewUser
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function NewUser() As Integer

        Dim result As Integer = 0
        Dim oObject As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oObject = CreateLateBoundObject("iPMUserMaintenance.Interface_Renamed")

            m_lReturn = oObject.Initialise()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                oObject = Nothing
                Return result
            End If

            'DC201003 PN7424 -change from NewUser to Start
            'm_lReturn = oObject.NewUser
            m_lReturn = oObject.Start()

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
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewUser Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewUser", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddAllMembers
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function AddAllMembers() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iTemp As Integer = 1 To lvwAll.Items.Count

                m_lReturn = AddMember(lvwAll.Items.Item(iTemp - 1))
            Next iTemp

            m_lReturn = RefreshList()

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAllMembers Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddAllMembers", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddMembers
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function AddMembers() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iTemp As Integer = 1 To lvwAll.Items.Count
                If lvwAll.Items.Item(iTemp - 1).Selected Then

                    m_lReturn = AddMember(lvwAll.Items.Item(iTemp - 1))
                End If
            Next iTemp

            m_lReturn = RefreshList()

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddMembers Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddMembers", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '********************************************************************
    '
    ' Function Name: AddMember()
    '
    ' Description: Adds member to the database
    '********************************************************************

    Private Function AddMember(ByRef oListItem As ListViewItem) As Integer

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
            lRow = CInt(Conversion.Val(sKey.Substring(sKey.Length - (sKey.Length - 1))))

            '    sName$ = (lvwGroups.SelectedItem)
            lID = (lRow + 1)


            m_lReturn = m_oBusiness.EditInclude(lID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add selected Member")
                Return result
            End If

            Return result

        Catch excep As System.Exception
            'Error Section

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Member", vApp:=ACApp, vClass:=ACClass, vMethod:="AddMember", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteAllMembers
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function DeleteAllMembers() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iTemp As Integer = 1 To lvwContents.Items.Count

                m_lReturn = DeleteMember(lvwContents.Items.Item(iTemp - 1))
            Next iTemp

            m_lReturn = RefreshList()

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteAllMembers Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAllMembers", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteMembers
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function DeleteMembers() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iTemp As Integer = 1 To lvwContents.Items.Count
                If lvwContents.Items.Item(iTemp - 1).Selected Then

                    m_lReturn = DeleteMember(lvwContents.Items.Item(iTemp - 1))
                End If
            Next iTemp

            m_lReturn = RefreshList()

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteMembers Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteMembers", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '********************************************************************
    '
    ' Function Name: DeleteMember()
    '
    ' Description: Deletes member from the database
    '********************************************************************

    Private Function DeleteMember(ByRef oListItem As ListViewItem) As Integer

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
            lRow = CInt(Conversion.Val(sKey.Substring(sKey.Length - (sKey.Length - 1))))

            '    sName$ = (lvwGroups.SelectedItem)
            lID = (lRow + 1)


            m_lReturn = m_oBusiness.EditIgnore(lID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to delete selected Member")
                Return result
            End If

            Return result

        Catch excep As System.Exception
            'Error Section

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Delete Member", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteMember", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '********************************************************************
    '
    ' Function Name: ToggleSupervisor()
    '
    ' Description: Toggles supervisor flag on the database
    '********************************************************************

    Private Function ToggleSupervisor(ByRef oListItem As ListViewItem) As Integer

        Dim result As Integer = 0
        Dim lRow As Integer
        Dim sKey As String
        Dim sName As String = ""
        Dim lID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            mnuSuper.Checked = Not mnuSuper.Checked
            If oListItem Is Nothing Then
                Return result
            End If

            sKey = oListItem.Name
            lRow = CInt(Conversion.Val(sKey.Substring(sKey.Length - (sKey.Length - 1))))

            '    sName$ = (lvwGroups.SelectedItem)
            lID = (lRow + 1)


            m_lReturn = m_oBusiness.EditToggleSupervisor(lID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to Toggle Supervisor")
                Return result
            End If
            If mnuSuper.Checked Then
                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = "supervisor"
                oListItem.ImageKey = "supervisor"
            Else
                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = ""
                oListItem.ImageKey = "user"
            End If
            Return result

        Catch excep As System.Exception
            'Error Section

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Toggle Supervisor", vApp:=ACApp, vClass:=ACClass, vMethod:="ToggleSupervisor", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cmdAddAllMembers_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddAllMembers.Click

        m_lReturn = AddAllMembers()

    End Sub

    Private Sub cmdAddMembers_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddMembers.Click

        m_lReturn = AddMembers()

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        Me.Hide()

    End Sub

    Private Sub cmdHelp_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdHelp.Click

        MessageBox.Show("There is no help associated with this screen", "Group Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)

    End Sub

    Private Sub cmdDeleteAllMembers_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteAllMembers.Click

        m_lReturn = DeleteAllMembers()

    End Sub

    Private Sub cmdDeleteMembers_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteMembers.Click

        m_lReturn = DeleteMembers()

    End Sub

    Private Sub cmdNewUser_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNewUser.Click

        m_lReturn = NewUser()

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        'Set status to PMOK
        m_lStatus = gPMConstants.PMEReturnCode.PMOK

        m_lReturn = WriteToDb()

        'hide this form
        Me.Hide()

    End Sub

    Private Sub frmMembers_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            With uctPMResizer1
                .SetControlResizeOption("cmdOK", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                .SetControlResizeOption("cmdCancel", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                .SetControlResizeOption("cmdHelp", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                .SetControlResizeOption("tabMembers", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                .SetControlResizeOption("cmdNewUser", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROTopOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                .SetControlResizeOption("lvwAll", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROHeightOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                .SetControlResizeOption("lvwContents", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROHeightOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

            End With

        End If
    End Sub

    Private Sub Form_Initialize_Renamed()

        ' Initialise the error number value.
        m_lError = gPMConstants.PMEReturnCode.PMTrue

        'Initialise the form using selected function
        m_lReturn = InitialForm()

    End Sub


    Private Sub frmMembers_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

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

    Private Sub lvwAll_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwAll.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwAll.Columns(eventArgs.Column)

        With lvwAll

            ListViewHelper.SetSortedProperty(lvwAll, False)
            ListViewHelper.SetSortKeyProperty(lvwAll, ColumnHeader.Index + 1 - 1)
            ListViewHelper.SetSortedProperty(lvwAll, True)

        End With

    End Sub

    Private Sub lvwAll_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAll.DoubleClick


        ' Edit details of user if doubleclicked
        Dim oListItem As ListViewItem = lvwAll.GetItemAt(m_lXPos, m_lYPos)

        cmdAddMembers_Click(cmdAddMembers, New EventArgs())

    End Sub

    Private Sub lvwAll_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwAll.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000

        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y

        m_lXPos = CInt(x)
        m_lYPos = CInt(y)

    End Sub

    Private Sub lvwContents_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwContents.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwContents.Columns(eventArgs.Column)

        With lvwAll

            ListViewHelper.SetSortedProperty(lvwAll, False)
            ListViewHelper.SetSortKeyProperty(lvwAll, ColumnHeader.Index + 1 - 1)
            ListViewHelper.SetSortedProperty(lvwAll, True)

        End With

    End Sub

    Private Sub lvwContents_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwContents.DoubleClick


        ' Edit details of user if doubleclicked
        Dim oListItem As ListViewItem = lvwAll.GetItemAt(m_lXPos, m_lYPos)

        cmdDeleteMembers_Click(cmdDeleteMembers, New EventArgs())

    End Sub

    Private Sub lvwContents_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwContents.MouseDown

        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        Try

            If eventArgs.Button <> Windows.Forms.MouseButtons.Right Then
                Exit Sub
            End If
            Dim oListItem As ListViewItem = lvwContents.GetItemAt(x, y)
           
            If Not oListItem.ImageKey Is Nothing Then
                If oListItem.ImageKey = "group" Then
                    Exit Sub
                End If
            End If

            mnuSuper.Checked = (oListItem.ImageKey = "supervisor")
            Ctx_mnuSupervisor.Show(Me, PointToClient(Cursor.Position).X, PointToClient(Cursor.Position).Y)

            If mnuSuper.Checked Then
                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = "supervisor"
                oListItem.ImageKey = "supervisor"
            Else
                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = ""
                oListItem.ImageKey = "user"
            End If

            m_oListItem = oListItem
        Catch ex As Exception

        End Try
    End Sub

    Private Sub lvwContents_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs)
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000

        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y


        m_lXPos = CInt(x)
        m_lYPos = CInt(y)

    End Sub

    Public Sub mnuSuper_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles mnuSuper.Click

        mnuSuper.Checked = Not mnuSuper.Checked

    End Sub


    Private Sub frmMembers_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown

        If e.Alt And e.KeyCode = Keys.D1 Then
            tabMembers.SelectedIndex = 0
        End If
    End Sub

    Private Sub Panel5_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel5.Paint

    End Sub

    Private Sub frmMembers_Resize(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Resize
        Try
            Panel4.Width += cmdAddAllMembers.Left + 200
            Panel5.Width = Panel4.Width
            lvwAll.Width = VB6.TwipsToPixelsX((VB6.PixelsToTwipsX(tabMembers.Width) - 1785) / 2)
            lvwContents.Width = lvwAll.Width
        Catch excep As System.Exception
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormCustomResizeFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="FormCustomResize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try
    End Sub

    Private Sub Ctx_mnuSupervisor_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Ctx_mnuSupervisor.Click
        m_lReturn = ToggleSupervisor(m_oListItem)
    End Sub
End Class
