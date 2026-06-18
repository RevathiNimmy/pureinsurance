Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Diagnostics
Imports System.Globalization
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmStep
    Inherits System.Windows.Forms.Form

    '====================================================================
    '   Class/Module: frmStep
    '   Description :
    '
    '====================================================================
    '   Maintenance History
    '
    '    04/03/2013       Created.
    '
    '====================================================================

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "frmStep"

    ' PUBLIC Data Members (Begin)
    'Now OK to use PUBLIC variable instead of Property (as long as no validation, read only, etc)
    Public Status As Integer
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)
    ' Stores the return value for the a function call.

    ' Form control
    Private m_oFormfields As Object

    Private m_lReturn As Integer
    Private m_vDocTemplateList As Object

    Private m_lStepID As Integer

    Private m_vTaskGroupUserGroups(,) As Object
    Private m_vTaskGroupTask(,) As Object
    Private m_vLookupDetails(,) As Object
    Private m_vLookupTables(,) As Object
    Private m_sPrevTaskGroup As String = ""

    Private m_lTaskGroupId As Integer
    Private m_lTaskId As Integer
    Private m_lUserGroupId As Integer
    Private m_sStepDescription As String = ""
    Private m_sAgencyOrUnderwriting As String = ""

    Public Property TaskId() As Integer
        Get
            Return m_lTaskId
        End Get
        Set(ByVal Value As Integer)
            m_lTaskId = Value
        End Set
    End Property
    Public Property TaskGroupId() As Integer
        Get
            Return m_lTaskGroupId
        End Get
        Set(ByVal Value As Integer)
            m_lTaskGroupId = Value
        End Set
    End Property

    Public Property UserGroupId() As Integer
        Get
            Return m_lUserGroupId
        End Get
        Set(ByVal Value As Integer)
            m_lUserGroupId = Value
        End Set
    End Property
    Public Property StepDescription() As String
        Get
            Return m_sStepDescription
        End Get
        Set(ByVal Value As String)
            m_sStepDescription = Value
        End Set
    End Property

    Public WriteOnly Property LookupTables() As Object(,)
        Set(ByVal Value As Object(,))
            m_vLookupTables = Value
        End Set
    End Property

    Public WriteOnly Property LookupDetails() As Object(,)
        Set(ByVal Value As Object(,))
            m_vLookupDetails = Value
        End Set
    End Property

    Public WriteOnly Property TaskGroupTasks() As Object(,)
        Set(ByVal Value As Object(,))
            m_vTaskGroupTask = Value
        End Set
    End Property

    Public WriteOnly Property TaskGroupUsers() As Object(,)
        Set(ByVal Value As Object(,))
            m_vTaskGroupUserGroups = Value
        End Set
    End Property

    ' PRIVATE Data Members (End)


    Public Property StepID() As Integer
        Get
            Return m_lStepID
        End Get
        Set(ByVal Value As Integer)
            m_lStepID = Value
        End Set
    End Property


    Public WriteOnly Property DocumentList() As Object(,)

        Set(ByVal Value As Object(,))

            'automatocally set document list array with first item being blank
            Dim vNewArray As Object

            'what if no documents yet created
            If Not Information.IsArray(Value) Then

                m_vDocTemplateList = 0
                Exit Property
            End If

            'Parse the array and get values out into our modular array. The combo listindex will
            'match the elements of our array
            ReDim vNewArray(2, (Value.GetUpperBound(1) + 1))
            'Add first item manually

            vNewArray(0, 0) = 0

            vNewArray(1, 0) = 0

            vNewArray(2, 0) = "(None)"

            For lLoop As Integer = 0 To Value.GetUpperBound(1)

                vNewArray(0, lLoop + 1) = lLoop + 1


                vNewArray(1, lLoop + 1) = CInt(Value(0, lLoop))


                vNewArray(2, lLoop + 1) = Value(1, lLoop)
            Next lLoop



            m_vDocTemplateList = vNewArray

        End Set
    End Property


    Public Property StepNumber() As Object
        Get
            If txtStep.Text.Trim() = "" Then

                Return Nothing
            Else
                Return CInt(txtStep.Text)
            End If
        End Get
        'Set(ByVal Value As String)
        Set(ByVal Value As Object)


            If IsNothing(Value) OrElse Convert.IsDBNull(Value) OrElse CStr(Value).Trim() = "" Then
                txtStep.Text = ""
            Else

                txtStep.Text = Value
            End If
        End Set
    End Property


    Public Property NextStep() As Object
        Get
            If txtNextStep.Text.Trim() = "" Then

                Return Nothing
            Else
                Return CInt(txtNextStep.Text)
            End If
        End Get
        'Set(ByVal Value As String)
        Set(ByVal Value As Object)

            If Convert.IsDBNull(Value) Or IsNothing(Value) Then
                txtNextStep.Text = ""
            Else
                txtNextStep.Text = Value
            End If
        End Set

    End Property


    'Public Property PreviousStep() As String
    Public Property PreviousStep() As Object
        Get
            If txtPreviousStep.Text.Trim() = "" Then

                Return Nothing
            Else
                'Return CStr(CInt(txtPreviousStep.Text))
                Return CInt(txtPreviousStep.Text)
            End If
        End Get
        'Set(ByVal Value As String)
        Set(ByVal Value As Object)

            If Convert.IsDBNull(Value) Or IsNothing(Value) Then
                txtPreviousStep.Text = ""
            Else
                txtPreviousStep.Text = Value
            End If
        End Set
    End Property



    Public Property ElapsedDays() As Object
        Get
            If txtElapsedDays.Text.Trim() = "" Then

                Return Nothing
            Else
                Return CInt(txtElapsedDays.Text)
            End If
        End Get
        'Set(ByVal Value As String)
        Set(ByVal Value As Object)

            If Convert.IsDBNull(Value) Or IsNothing(Value) Then
                txtElapsedDays.Text = ""
            Else
                txtElapsedDays.Text = Value
            End If
        End Set
    End Property


    'Public Property CheckAutoCancelRules() As String
    Public Property CheckAutoCancelRules() As Object
        Get
            'Return CStr(chkCheckAutoCancel.CheckState)
            Return chkCheckAutoCancel.CheckState
        End Get
        'Set(ByVal Value As String)
        Set(ByVal Value As Object)

            If IsNothing(Value) OrElse Convert.IsDBNull(Value) OrElse CStr(Value).Trim() = "" Then
                chkCheckAutoCancel.CheckState = CheckState.Unchecked
            Else

                chkCheckAutoCancel.CheckState = Value
            End If
        End Set
    End Property


    'Public Property RunAutoCancelRules() As String
    Public Property RunAutoCancelRules() As Object
        Get
            'Return CStr(chkRunAutoCancel.CheckState)
            Return chkRunAutoCancel.CheckState
        End Get
        'Set(ByVal Value As String)
        Set(ByVal Value As Object)


            'If Convert.IsDBNull(Value) Or IsNothing(Value) Or CStr(Value).Trim() = "" Then
            If IsNothing(Value) OrElse Convert.IsDBNull(Value) OrElse CStr(Value).Trim() = "" Then
                chkRunAutoCancel.CheckState = CheckState.Unchecked
            Else

                'chkRunAutoCancel.CheckState = CInt(Value)
                chkRunAutoCancel.CheckState = Value
            End If
        End Set
    End Property

    Public Property ClientLetterId() As Object
        Get
            Dim lId As Integer

            If cboClientLetter.SelectedIndex < 0 Then
                lId = 0
            Else
                lId = m_vDocTemplateList(1, cboClientLetter.SelectedIndex)
            End If

            If lId = 0 Then

                Return Nothing
            Else
                Return lId
            End If
        End Get
        'Set(ByVal Value As Integer)
        Set(ByVal Value As Object)
            Dim lListIndex As Integer

            If IsNothing(Value) OrElse Convert.IsDBNull(Value) OrElse CStr(Value).Trim() = "" Then
                cboClientLetter.SelectedIndex = -1
            Else
                m_lReturn = GetComboDocuMatch(v_lDocID:=Value, r_lListindex:=lListIndex)
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    If cboClientLetter.Items.Count > 0 Then
                        cboClientLetter.SelectedIndex = lListIndex
                    End If
                Else
                    cboClientLetter.SelectedIndex = -1
                End If
            End If

        End Set
    End Property



    '
    Private Function IsOptionalNum(ByVal v_vValue As TextBox, ByVal v_sLabel As String) As Boolean

        Dim result As Boolean = False
        Const sMsgBoxTitle As String = "Chase Cycle Rule Item"
        Try


            result = True

            Dim dbNumericTemp As Double
            If Not Double.TryParse(v_vValue.Text, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) And v_vValue.Text <> "" Then
                MessageBox.Show(v_sLabel & " must be numeric.", sMsgBoxTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End If

            Return result

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to validate " & sMsgBoxTitle & " '" & v_sLabel & "'", vApp:=ACApp, vClass:=ACClass, vMethod:="IsOptionalNum", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub cboTaskGroup_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboTaskGroup.SelectedIndexChanged
        SetupTaskCombos()

        ' store the currenct task group
        m_sPrevTaskGroup = cboTaskGroup.Text

    End Sub


    Private Sub chkCheckAutoCancel_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkCheckAutoCancel.CheckStateChanged
        If chkCheckAutoCancel.CheckState = CheckState.Checked Then

        ElseIf chkRunAutoCancel.CheckState = CheckState.Unchecked Then
            cboClientLetter.Enabled = True
        End If

    End Sub

    Private Sub chkRunAutoCancel_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkRunAutoCancel.CheckStateChanged
        If chkRunAutoCancel.CheckState = CheckState.Checked Then
            'cboClientLetter.SelectedIndex = 0
            'cboClientLetter.Enabled = False

        ElseIf chkRunAutoCancel.CheckState = CheckState.Unchecked Then
            cboClientLetter.Enabled = True
        End If

    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.

        Try

            ' Set the interface status.
            Status = gPMConstants.PMEReturnCode.PMCancel
            Me.Hide()

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Cancel command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try


    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Click event of the OK button.
        Dim lStepVal As Integer
        Try

            'Check mandatory controls have been entered into.

            m_lReturn = m_oFormfields.CheckMandatoryControls()
            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'Do some basic datatype validation (this should be automated in future)

            ' Validate fields
            If Not IsOptionalNum(txtStep, "Step") Then Exit Sub
            If Not IsOptionalNum(txtNextStep, "Next Step") Then Exit Sub
            If Not IsOptionalNum(txtPreviousStep, "Previous Step") Then Exit Sub

            'start
            lStepVal = gPMFunctions.ToSafeLong(txtStep.Text)
            If gPMFunctions.ToSafeLong(txtNextStep.Text) = lStepVal And gPMFunctions.ToSafeLong(txtPreviousStep.Text) = lStepVal Then
                'end
                MessageBox.Show("Step's order can't be same", "Chase Cycle Rule Item", MessageBoxButtons.OK, MessageBoxIcon.Error)
                txtStep.Focus()
                Exit Sub
            End If
            If Not IsOptionalNum(txtElapsedDays, "Elapsed Days ") Then Exit Sub


            'Make sure step number not duplicate
            If Not IsValidStepNumber(CInt(txtStep.Text.Trim())) Then
                DisplayMessage(r_lTitleId:=ACInvalidStepNumberTitle, r_lMessageId:=ACInvalidStepNumberDetails, r_lOptions:=MsgBoxStyle.Exclamation)
                Exit Sub
            End If

            'Make sure if TaskGroup is selected then Task and UserGroup can not be blanked
            If cboTaskGroup.SelectedIndex > 0 Then
                If cboTask.SelectedIndex = -1 Then
                    MessageBox.Show("A 'Task Group' has been selected, please choose a 'Task'", "Chase Cycle Rule Item", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    cboTask.Focus()
                    Exit Sub
                End If
            End If
            If cboTaskGroup.SelectedIndex > 0 Then
                If cboUserGroup.SelectedIndex = -1 Then
                    MessageBox.Show("A 'Task Group' has been selected, please choose a 'User Group'", "Chase Cycle Rule Item", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    cboUserGroup.Focus()
                    Exit Sub
                End If
            End If

            If cboTask.SelectedIndex >= 0 Then
                m_lTaskId = VB6.GetItemData(cboTask, cboTask.SelectedIndex)
            End If

            If cboUserGroup.SelectedIndex >= 0 Then
                m_lUserGroupId = VB6.GetItemData(cboUserGroup, cboUserGroup.SelectedIndex)
            End If

            If cboTaskGroup.SelectedIndex >= 0 Then
                m_lTaskGroupId = VB6.GetItemData(cboTaskGroup, cboTaskGroup.SelectedIndex)
            End If

            m_sStepDescription = txtStepDescription.Text

            ' Set the interface status.
            Status = gPMConstants.PMEReturnCode.PMOK

            Me.Hide()

        Catch excep As System.Exception


            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the OK command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOK_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub


        End Try


    End Sub


    Private Sub frmStep_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            Try

                m_lReturn = SetInterfaceDefaults()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Exit Sub
                End If

                Exit Sub

            Catch excep As System.Exception

                ' Error Section.

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to activate the form", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Activate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

                Exit Sub

            End Try
        End If
    End Sub

    Private Sub Form_Initialize_Renamed()

        Try
            cboClientLetter.SelectedIndex = -1
            Status = gPMConstants.PMEReturnCode.PMCancel

        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the form", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try


    End Sub


    Private Sub frmStep_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        Try

            m_lReturn = iPMFunc.getUnderwritingOrAgency(m_sAgencyOrUnderwriting)

            ' Display all language specific captions.
            m_lReturn = iPMForms.DisplayCaptions(Me, My.Resources.ResourceManager)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            'Set formfields object

            m_lReturn = iPMForms.SetFieldValidation(r_frmSource:=Me, r_oFormfields:=m_oFormfields)


            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load the form", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Private Sub frmStep_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)


        m_oFormfields.Dispose()
        m_oFormfields = Nothing

        eventArgs.Cancel = Cancel <> 0

    End Sub

    Public Function ShowDocumentLists() As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: ShowDocumentLists
        ' PURPOSE: Get list of Document_Template records
        ' CHANGES:
        ' ---------------------------------------------------------------------------
        Dim result As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'Populate the dropdowns
            cboClientLetter.Items.Clear()

            For lLoop As Integer = 0 To m_vDocTemplateList.GetUpperBound(1)

                cboClientLetter.Items.Add(CStr(m_vDocTemplateList(2, lLoop)))

                VB6.SetItemData(cboClientLetter, cboClientLetter.Items.Count - 1, CInt(m_vDocTemplateList(1, lLoop)))

            Next lLoop
        Catch excep As System.Exception


            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowDocumentLists", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=excep)

            result = gPMConstants.PMEReturnCode.PMFalse


            Exit Function

        End Try

        Return result

    End Function


    Private Function IsValidStepNumber(ByVal v_iStepNumber As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: IsValidStepNumber
        ' PURPOSE: Ensure that a Step Number is not duplicated
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0


        Try

            result = True

            'If no items in listview - then no conflict
            If ofrmDetails.lvwSteps.Items.Count = 0 Then
                Return result
            End If

            'If the StepID is missing, then it is a new step being added
            If m_lStepID = 0 And Me.Status <> gPMConstants.PMEComponentAction.PMEdit Then

                'Loop through the listview
                For Each oListItem As ListViewItem In ofrmDetails.lvwSteps.Items

                    'Test for a match on step number only
                    With oListItem

                        If Conversion.Val(.Text) = v_iStepNumber Then

                            'If adding an item - error


                            'if editing an item - no error


                            'There is a match so an entry already exists
                            'therefore this selection is invalid
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    End With

                Next oListItem

            Else
                'there should only be one row, which is the currently selected item
                'Loop through the listview

                For Each oListItem As ListViewItem In ofrmDetails.lvwSteps.Items
                    'Test for a match on UserGroupId and CompanyId
                    With oListItem
                        If Conversion.Val(.Text) = v_iStepNumber And Conversion.Val(ListViewHelper.GetListViewSubItem(oListItem, 4).Text) <> m_lStepID Then

                            'There is a match so an entry already exists
                            'thereofre this selection is invalid
                            Return gPMConstants.PMEReturnCode.PMFalse

                        End If
                    End With
                Next oListItem

            End If
        Catch excep As System.Exception


            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="IsValidStepNumber", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=excep)

            result = gPMConstants.PMEReturnCode.PMFalse

            Exit Function

        End Try
        Return result

    End Function


    Private Function GetComboDocuMatch(ByVal v_lDocID As Integer, ByRef r_lListindex As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetComboDocuMatch
        ' PURPOSE: Return the position of a match for a document ID from
        '          m_vDocTemplateList array
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'If no items in listview - then nothing to do
            If Not Information.IsArray(m_vDocTemplateList) Then
                r_lListindex = 0
                Return result
            End If

            'Go through the array and find a match for the supplied ID

            For lLoop As Integer = 0 To m_vDocTemplateList.GetUpperBound(1)

                If CInt(m_vDocTemplateList(1, lLoop)) = v_lDocID Then

                    r_lListindex = CInt(m_vDocTemplateList(0, lLoop))
                    Return result
                End If
            Next lLoop

            'We should have a match by now - this code should not execute

            result = False
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to find supplied document Id in dropdown of available documents", vApp:=ACApp, vClass:=ACClass, vMethod:="GetComboDocumentMatch")
        Catch excep As System.Exception

            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetComboDocuMatch", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=excep)

            result = gPMConstants.PMEReturnCode.PMFalse

            Exit Function

        End Try
        Return result


    End Function

    ' ***************************************************************** '
    ' Name: PopulateTaskCbo
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           
    ' ***************************************************************** '
    Private Function PopulateTaskCbo(ByVal v_lTaskGroupId As Integer) As Integer


        Dim result As Integer = 0
        Const sFunctionName As String = "PopulateTaskCbo"

        Dim llBound, lUBound, lTaskId, lTaskGroupId As Integer
        Dim sTaskDescription As String = ""
        Dim lIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' initialisation
            cboTask.Items.Clear()

            lIndex = 0

            ' if we have a selected task group
            If cboTaskGroup.Text <> "" Then

                ' if we have an array of task group tasks
                If Information.IsArray(m_vTaskGroupTask) Then

                    ' get array boundaries
                    llBound = m_vTaskGroupTask.GetLowerBound(1)
                    lUBound = m_vTaskGroupTask.GetUpperBound(1)

                    ' for each item in the array
                    For lItem As Integer = llBound To lUBound

                        ' get item details
                        lTaskGroupId = CInt(m_vTaskGroupTask(ACTaskGroupTaskGroupId, lItem))
                        lTaskId = CInt(m_vTaskGroupTask(ACTaskgroupTaskId, lItem))
                        sTaskDescription = CStr(m_vTaskGroupTask(ACTaskGroupTaskDescription, lItem))

                        ' if task group matches the selected task group
                        If lTaskGroupId = v_lTaskGroupId Then

                            ' add task to combo
                            cboTask.Items.Insert(lIndex, sTaskDescription)
                            VB6.SetItemData(cboTask, lIndex, lTaskId)

                            lIndex += 1

                        End If

                    Next lItem

                End If

            End If

            cboTask.Enabled = Not (lIndex = 0)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lTaskGroupId", v_lTaskGroupId)
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '*******************************

            Return result

        End Try

    End Function


    ' ***************************************************************** '
    ' Name: PopulateUserGroupscbo
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created :  01-03-2013 : workflow
    ' ***************************************************************** '
    Private Function PopulateUserGroupscbo(ByVal v_lTaskGroupId As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "PopulateUserGroupscbo"

        Dim lIndex, llBound, lUBound, lTaskGroupId, lUserGroupId As Integer
        Dim sUserGroup As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cboUserGroup.Items.Clear()

            lIndex = 0

            ' if we have a selected task group
            If cboTaskGroup.Text <> "" Then

                ' if we have an array
                If Information.IsArray(m_vTaskGroupUserGroups) Then

                    ' get array boundaries
                    llBound = m_vTaskGroupUserGroups.GetLowerBound(1)
                    lUBound = m_vTaskGroupUserGroups.GetUpperBound(1)

                    ' for each item in the array
                    For lItem As Integer = llBound To lUBound

                        ' get item details
                        lTaskGroupId = CInt(m_vTaskGroupUserGroups(ACTaskGroupUserGroup_TaskGroupId, lItem))
                        lUserGroupId = CInt(m_vTaskGroupUserGroups(ACTaskGroupUserGroup_UserGroupId, lItem))
                        sUserGroup = CStr(m_vTaskGroupUserGroups(ACTaskGroupUserGroup_UserGroupDescription, lItem))

                        ' if task group matches the selected task group
                        If lTaskGroupId = v_lTaskGroupId Then

                            ' add user group to combo
                            cboUserGroup.Items.Insert(lIndex, sUserGroup)
                            VB6.SetItemData(cboUserGroup, lIndex, lUserGroupId)
                            lIndex += 1

                        End If

                    Next lItem

                End If

            End If

            cboUserGroup.Enabled = Not (lIndex = 0)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lTaskGroupId", v_lTaskGroupId)
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '*******************************
            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: GetLookupDetails
    '
    ' Description: Gets all of the lookup details using the lookup
    '              values, then assigns them to the control passed.
    '               Selects the specified row
    ' ***************************************************************** '

    Private Function GetLookupDetails(ByVal v_sLookupTable As String, ByRef r_octlLookup As ComboBox, Optional ByVal v_lSelectedItemId As Integer = 0, Optional ByVal v_bAddBlankEntry As Boolean = False) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetLookupDetails"

        ' Lookup value contants.
        Const ACValueTableName As Integer = 0
        '======Done on 27/01/2014 by samar=======
        ' Const ACValueID As Integer = 1
        '=============end========================
        Const ACValueStartPos As Integer = 2
        Const ACValueNumber As Integer = 3

        Dim lRow As Integer
        Dim bFoundMatch As Boolean
        Dim lItemIndex, lItemFoundIndex, lIndex As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            r_octlLookup.Items.Clear()

            bFoundMatch = False

            For lRow = m_vLookupTables.GetLowerBound(1) To m_vLookupTables.GetUpperBound(1)
                ' Check for a match of the table name.
                If CStr(m_vLookupTables(ACValueTableName, lRow)).Trim() = v_sLookupTable.Trim() Then
                    ' Found a match
                    bFoundMatch = True
                    Exit For
                End If
            Next lRow

            ' Check if there has been a table match.
            If Not bFoundMatch Then

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get details for the table, " & v_sLookupTable, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupDetails")

                Return result
            End If

            ' Using the lookup values, populate the control with
            ' the details from the lookup details array.
            lItemIndex = -1
            lItemFoundIndex = -1

            If v_bAddBlankEntry Then
                r_octlLookup.Items.Insert(0, "")
                lIndex = 1
            Else
                lIndex = 0
            End If

            For lCntr As Integer = CInt(m_vLookupTables(ACValueStartPos, lRow)) To CInt((CDbl(m_vLookupTables(ACValueStartPos, lRow)) + CDbl(m_vLookupTables(ACValueNumber, lRow))) - 1)

                r_octlLookup.Items.Add(CStr(m_vLookupDetails(ACDetailDesc, lCntr)))
                VB6.SetItemData(r_octlLookup, lIndex, CInt(m_vLookupDetails(ACDetailKey, lCntr)))


                If CDbl(m_vLookupDetails(ACDetailKey, lCntr)) = v_lSelectedItemId Then
                    lItemFoundIndex = lItemIndex
                End If

                Debug.WriteLine(VB6.GetItemString(r_octlLookup, lIndex) & ":" & CStr(lIndex))

                lIndex += 1
                lItemIndex += 1

            Next lCntr

            ' set the item we want to display in the list
            If lItemFoundIndex <> -1 Then
                r_octlLookup.SelectedIndex = lItemFoundIndex
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lSelectedItemId", v_lSelectedItemId)
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '*******************************
            Return result


            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: PopulateLookups
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created :  01/02/2013 : workflow
    ' ***************************************************************** '
    Private Function PopulateLookups() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "PopulateLookups"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Populate Lookup Values

            ' Task Group
            m_lReturn = GetLookupDetails(v_sLookupTable:=ACLookupTablePMWrkTaskGroup, r_octlLookup:=cboTaskGroup, v_bAddBlankEntry:=True)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupItem
    '
    ' Parameters: n/a
    '
    ' Description: Returns the code for a specifed item description
    '                  in a specified lookup table..
    '
    ' History:
    '           Created :   01/01/2013 
    ' ***************************************************************** '
    Private Function GetLookupItem(ByVal v_sLookupTable As String, ByVal r_sItemDesc As String, ByRef r_sItemCode As String, ByRef r_lItemId As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "GetLookupItem"

        Dim lRow As Integer
        Dim bFoundMatch As Boolean
        Dim sCode As String = ""

        Dim llBound, lUBound As Integer
        Dim v_vLookupItem As String = ""
        Dim lLookupItem As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Lookup value contants.
            Const ACValueTableName As Integer = 0
            'Const ACValueID As Integer = 1
            Const ACValueStartPos As Integer = 2
            Const ACValueNumber As Integer = 3

            Const ACDetailKey As Integer = 0
            Const ACDetailDesc As Integer = 1
            Const ACDetailCode As Integer = 2

            ' Initilisation
            bFoundMatch = False

            For lRow = m_vLookupTables.GetLowerBound(1) To m_vLookupTables.GetUpperBound(1)

                ' Check for a match of the table name.
                If CStr(m_vLookupTables(ACValueTableName, lRow)).Trim() = v_sLookupTable.Trim() Then
                    bFoundMatch = True
                    Exit For
                End If

            Next lRow

            If bFoundMatch Then
                ' get array boundaries for specified table
                llBound = CInt(m_vLookupTables(ACValueStartPos, lRow))

                lUBound = CInt((CDbl(m_vLookupTables(ACValueStartPos, lRow)) + CDbl(m_vLookupTables(ACValueNumber, lRow))) - 1)


                ' set lookup properties
                If r_lItemId <> 0 Then
                    v_vLookupItem = CStr(r_lItemId)
                    lLookupItem = 0

                ElseIf r_sItemDesc <> "" Then
                    v_vLookupItem = r_sItemDesc
                    lLookupItem = 1

                ElseIf r_sItemCode <> "" Then
                    v_vLookupItem = r_sItemCode
                    lLookupItem = 2
                End If

                ' loop around the available items for the specified table
                For lCntr As Integer = llBound To lUBound

                    ' get the code for the specified lookup items key
                    If CStr(m_vLookupDetails(lLookupItem, lCntr)).Trim() = v_vLookupItem Then

                        ' return the requested code, id, description
                        r_sItemDesc = CStr(m_vLookupDetails(ACDetailDesc, lCntr)).Trim()
                        r_sItemCode = CStr(m_vLookupDetails(ACDetailCode, lCntr)).Trim()
                        r_lItemId = CInt(CStr(m_vLookupDetails(ACDetailKey, lCntr)).Trim())

                        Exit For
                    End If

                Next lCntr

            End If

            ' if we dont find the code then log an error
            If r_sItemCode = "" Then

                result = gPMConstants.PMEReturnCode.PMFalse

                '******************************
                ' Log Error.
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("r_sItemCode", r_sItemCode)
                oDict.Add("r_lItemId", r_lItemId)
                gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to find code for lookuptable:" & v_sLookupTable & "and lookup Item:" & v_vLookupItem, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)
                '*******************************

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("r_sItemCode", r_sItemCode)
            oDict.Add("r_lItemId", r_lItemId)
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '*******************************

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetupTaskCombos
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : 01/01/2013 : workflow
    ' ***************************************************************** '
    Private Function SetupTaskCombos() As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "SetupTaskCombos"

        Dim lTaskGroupId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' get task group id
            If cboTaskGroup.Text <> "" Then
                m_lReturn = GetLookupItem(v_sLookupTable:=ACLookupTablePMWrkTaskGroup, r_sItemDesc:=cboTaskGroup.Text, r_sItemCode:="", r_lItemId:=lTaskGroupId)
            End If

            ' if a new item has been selected or onload no item has yet been selected
            If m_sPrevTaskGroup <> cboTaskGroup.Text Or m_sPrevTaskGroup = "" Then

                If cboTaskGroup.Text <> "" Then

                    ' populate task
                    If PopulateTaskCbo(v_lTaskGroupId:=lTaskGroupId) <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' populate user groups
                    If PopulateUserGroupscbo(v_lTaskGroupId:=lTaskGroupId) <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If

                Else

                    ' clear down any selected task and user group

                    ' user group clear
                    cboUserGroup.Items.Clear()
                    cboUserGroup.Enabled = False

                    ' task clear
                    cboTask.Items.Clear()
                    cboTask.Enabled = False

                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep)
            '*******************************

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created :01/03/2013 : Chase Cycle RetroFit
    ' ***************************************************************** '
    Public Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetInterfaceDefaults"

        Dim lReturn As gPMConstants.PMEReturnCode
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            If Status = gPMConstants.PMEComponentAction.PMAdd Then

                m_sPrevTaskGroup = ""

                ' set interface defaults..
                txtStepDescription.Text = ""

                ' populate lookup combos
                lReturn = PopulateLookups()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "PopulateLookups Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                lReturn = SetupTaskCombos()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SetupTaskCombos Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddNonInsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Else
                Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditNonInsTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                ' populate lookup combos
                m_lReturn = PopulateLookups()

                ' set interface defaults..
                txtStepDescription.Text = m_sStepDescription

                ' populate task lookup cbo's
                lReturn = CType(SelectcboItem(cboTaskGroup, m_lTaskGroupId), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SelectcboItem Failed to populate cboTaskGroup", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' verify that any saved data has been correctly transposed to the screen
                ' if not raise a message box stating that the individual item is no longer
                ' available....
                If m_lTaskGroupId And cboTaskGroup.SelectedIndex = -1 Then
                    MessageBox.Show("The Task Group originally specified for the step is no longer available. TaskGroupId=" & m_lTaskGroupId & ". Either it has been set to deleted is it is no longer effective.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    m_lTaskGroupId = 0
                Else
                    If m_lTaskGroupId <> VB6.GetItemData(cboTaskGroup, cboTaskGroup.SelectedIndex) Then
                        MessageBox.Show("The Task Group originally specified for the step is no longer available. Either it has been set to deleted is it is no longer effective.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                        m_lTaskGroupId = 0
                    End If
                End If
            End If
            If m_lTaskGroupId <> 0 Then

                lReturn = CType(SelectcboItem(cboTask, m_lTaskId), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SelectcboItem Failed to populate cboTask", gPMConstants.PMELogLevel.PMLogError)
                End If

                lReturn = CType(SelectcboItem(cboUserGroup, m_lUserGroupId), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SelectcboItem Failed to populate cboUserGroup", gPMConstants.PMELogLevel.PMLogError)
                End If

                If cboTask.SelectedIndex = -1 And m_lTaskId > 0 Then
                    MessageBox.Show("The Task originally specified for the step is no longer available. TaskId=" & m_lTaskId & ". Either it has been set to deleted is it is no longer effective.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                Else
                    If m_lTaskId <> VB6.GetItemData(cboTask, cboTask.SelectedIndex) Then
                        MessageBox.Show("The Task originally specified for the step is no longer available. TaskId=" & m_lTaskId & ". Either it has been set to deleted is it is no longer effective.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If

                If cboUserGroup.SelectedIndex = -1 And m_lUserGroupId > 0 Then
                    MessageBox.Show("The User Group originally specified for the step is no longer available. UserGroupId=" & m_lUserGroupId & ". Either it has been set to deleted is it is no longer effective.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                ElseIf cboUserGroup.SelectedIndex >= 0 Then
                    If m_lUserGroupId <> VB6.GetItemData(cboUserGroup, cboUserGroup.SelectedIndex) Then
                        MessageBox.Show("The User Group originally specified for the step is no longer available. UserGroupId=" & m_lUserGroupId & ". Either it has been set to deleted is it is no longer effective.", Application.ProductName, MessageBoxButtons.OK, MessageBoxIcon.Information)
                    End If
                End If

            End If

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=excep)
            '*******************************

            Return result
        End Try
        Return result
    End Function



    ' ***************************************************************** '
    ' Name: SelectcboItem
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created  : 01/03/2013
    ' ***************************************************************** '
    Private Function SelectcboItem(ByRef r_oCbo As ComboBox, ByVal v_lSelectedId As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "SelectcboItem"

        Dim bItemNotFound As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bItemNotFound = True

            ' if the item id is valid
            If v_lSelectedId <> -1 Then

                ' for each item in the list
                For lItem As Integer = 0 To r_oCbo.Items.Count - 1
                    ' search the item data array for a match
                    If VB6.GetItemData(r_oCbo, lItem) = v_lSelectedId Then

                        ' found a match - select the item
                        r_oCbo.SelectedIndex = lItem
                        bItemNotFound = False
                        Exit For
                    End If

                Next lItem

            End If

            If bItemNotFound Then

                ' log that we havent found the specified item
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lSelectedId", v_lSelectedId)
                gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to find item with id:" & CStr(v_lSelectedId) & " in :" & r_oCbo.Name, vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, oDicParms:=oDict)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lSelectedId", v_lSelectedId)
            gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '*******************************

            Return result


            Return result
        End Try
    End Function


End Class
