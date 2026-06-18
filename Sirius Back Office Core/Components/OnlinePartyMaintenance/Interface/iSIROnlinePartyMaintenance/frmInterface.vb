Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
'Developer Guide No.129
Imports SharedFiles
Imports System.Runtime.InteropServices

Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form

    'Declarations

    Private m_lStatus As gPMConstants.PMEReturnCode

    Private m_lError As gPMConstants.PMEReturnCode

    Private m_lFormMode As Integer
    Private m_bCancel As Boolean


    Private m_oBusiness As bSIROnlineParty.Business
    'Private m_oBusiness As bPMUserGroup.Business

    'bSIRFindParty
    Private m_oFindPartyBusiness As Object

    Private m_dtEffectiveDate As Date
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_sCallingAppName As String = ""

    Private m_lXPos As Integer
    Private m_lYPos As Integer
    Private m_lReturn As Integer

    Private m_vSearchData(,) As Object
    Private m_bChangesHaveBeenMade As Boolean
    Private m_bListRefreshRequiredAfterApply As Boolean

    Private Const ACClass As String = "frmClients"
    Private Const ACFindImage As String = "FindImage"

    Const LVM_FIRST As Int32 = &H1000
    Const LVM_SCROLL As Int32 = LVM_FIRST + 20
    Const SBS_HORZ As Integer = 0
    Private _hScrollValue As Integer = 0

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



    ' PUBLIC Property Procedures (End)

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

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    '
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Dim sPartyTypeOther As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            m_oFindPartyBusiness.PartyCnt = 0


            m_lReturn = m_oFindPartyBusiness.SearchByQuery(r_lNumberOfRecords:=ACMaxSearchDetails, r_vResultArray:=m_vSearchData, v_vShortName:=txtShortName.Text, v_vName:=txtLongName.Text, v_vClientType:=cmbType.Text, v_vAddress1:=txtAddress1.Text, v_vPostalCode:=txtPostalCode.Text)

            ' {* USER DEFINED CODE (End) *}

            ' Check the return values.
            Select Case (m_lReturn)
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' Found search details.
                Case gPMConstants.PMEReturnCode.PMNotFound
                    ' No search details found.
                Case Else
                    ' Failed to get details.
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get search details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")

                    Return result
            End Select

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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
            Dim temp_m_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIROnlineParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
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

            ' Get an instance of the business object via
            ' the public object manager.
            Dim temp_m_oFindPartyBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oFindPartyBusiness, "bSIRFindParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oFindPartyBusiness = temp_m_oFindPartyBusiness

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
    ' Check if we have an instance of the business object.
    'If Not (m_oFindPartyBusiness Is Nothing) Then
    ' Terminate the business object

    'm_lReturn = m_oFindPartyBusiness.Terminate()
    '
    ' Check for errors.
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Log Error.
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to terminate the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="Terminate")
    'End If
    '
    ' Destroy the instance of the business object
    ' from memory.
    'm_oFindPartyBusiness = Nothing
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

    '***********************************************************************
    '
    ' Function Name: WriteToDb()
    '
    ' Description: Does the checks and updates the db
    '
    '***********************************************************************

    Private Function WriteToDb() As Boolean

        Dim vChangeArray(,) As Object = Nothing
        Dim iChangedCount As Integer
        Dim bFailures As Boolean
        Dim f As frmShowFailures
        Dim oListItem As ListViewItem
        Try

            ' Read through the No Access list to find clients that have been moved in
            For iLoop As Integer = 1 To lvwNoAccess.Items.Count
                If CStr(m_vSearchData(ACIOnlineAccess, Convert.ToString(lvwNoAccess.Items.Item(iLoop - 1).Tag))) = "True" Then
                    ' This client previously had access, so must have been moved
                    ReDim Preserve vChangeArray(ACICAMax, iChangedCount)

                    vChangeArray(ACICAPartyCnt, iChangedCount) = m_vSearchData(ACIPartyCnt, Convert.ToString(lvwNoAccess.Items.Item(iLoop - 1).Tag))

                    vChangeArray(ACICAPartyShortName, iChangedCount) = m_vSearchData(ACIShortName, Convert.ToString(lvwNoAccess.Items.Item(iLoop - 1).Tag))

                    vChangeArray(ACICAAccessStatus, iChangedCount) = 0
                    iChangedCount += 1
                End If
            Next

            ' Read through the Access list to find clients that have been moved in
            For iLoop As Integer = 1 To lvwAccess.Items.Count
                If CStr(m_vSearchData(ACIOnlineAccess, Convert.ToString(lvwAccess.Items.Item(iLoop - 1).Tag))) <> "True" Then
                    ' This client previously had no access, so must have been moved
                    ReDim Preserve vChangeArray(ACICAMax, iChangedCount)

                    vChangeArray(ACICAPartyCnt, iChangedCount) = m_vSearchData(ACIPartyCnt, Convert.ToString(lvwAccess.Items.Item(iLoop - 1).Tag))

                    vChangeArray(ACICAPartyShortName, iChangedCount) = m_vSearchData(ACIShortName, Convert.ToString(lvwAccess.Items.Item(iLoop - 1).Tag))

                    vChangeArray(ACICAAccessStatus, iChangedCount) = 1
                    iChangedCount += 1
                End If
            Next

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_lReturn = m_oBusiness.UpdateOnlineAccessStatus(r_vChangeArray:=vChangeArray, r_bFailures:=bFailures)
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Changes to Online Access could not be written to Database", "Group Maintenance", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Exit Function
            End If

            ' Check for failures
            If bFailures Then
                If MessageBox.Show("Not all Client's Status were updated successfully. " & _
                                   "Do you wish to review the details?", "Failures", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = System.Windows.Forms.DialogResult.Yes Then
                    ' Display Failures
                    f = New frmShowFailures()
                    For iLoop As Integer = vChangeArray.GetLowerBound(1) To vChangeArray.GetUpperBound(1)

                        If CStr(vChangeArray(ACICAFailureReason, iLoop)) <> "" Then

                            oListItem = f.lvwFailures.Items.Add(CStr(vChangeArray(ACICAPartyShortName, iLoop)))

                            ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(vChangeArray(ACICAFailureReason, iLoop))
                        End If
                    Next
                    f.ShowDialog()
                    f = Nothing
                    oListItem = Nothing
                End If
            Else
                MessageBox.Show("Updated Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If

            m_bChangesHaveBeenMade = False

            Return Not bFailures

        Catch excep As System.Exception



            'Error Section

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to write details to database", vApp:=ACApp, vClass:=ACClass, vMethod:="WriteToDb", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Function

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddAllClients
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function AddAllClients() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iTemp As Integer = 1 To lvwNoAccess.Items.Count
                m_lReturn = AddClient(lvwNoAccess.Items.Item(iTemp - 1))
            Next iTemp

            lvwNoAccess.Items.Clear()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddAllClients Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddAllClients", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddClients
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function AddClients() As Integer

        Dim result As Integer = 0
        Dim iTemp As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iTemp = 1
            Do Until iTemp > lvwNoAccess.Items.Count
                If lvwNoAccess.Items.Item(iTemp - 1).Selected Then
                    m_lReturn = AddClient(lvwNoAccess.Items.Item(iTemp - 1))
                    lvwNoAccess.Items.RemoveAt(iTemp - 1)
                Else
                    iTemp += 1
                End If
            Loop

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddClients Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddClients", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '********************************************************************
    '
    ' Function Name: AddClient()
    '
    ' Description: Adds task to the database
    '********************************************************************

    Private Function AddClient(ByRef oListItem As ListViewItem) As Integer

        Dim result As Integer = 0
        Dim oNewListItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If oListItem Is Nothing Then
                Return result
            End If


            'Developer Guide No.49
            oNewListItem = lvwAccess.Items.Add(oListItem.Text, "FindImage")

            oNewListItem.Tag = Convert.ToString(oListItem.Tag)
            For i As Integer = 1 To 4
                ListViewHelper.GetListViewSubItem(oNewListItem, i).Text = ListViewHelper.GetListViewSubItem(oListItem, i).Text
            Next

            oNewListItem = Nothing
            m_bChangesHaveBeenMade = True

            Return result

        Catch excep As System.Exception



            'Error Section

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Client", vApp:=ACApp, vClass:=ACClass, vMethod:="AddClient", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteAllClients
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function DeleteAllClients() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iTemp As Integer = 1 To lvwAccess.Items.Count
                m_lReturn = DeleteClient(lvwAccess.Items.Item(iTemp - 1))
            Next iTemp

            lvwAccess.Items.Clear()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteAllClients Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAllClients", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteClients
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function DeleteClients() As Integer

        Dim result As Integer = 0
        Dim iTemp As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iTemp = 1
            Do Until iTemp > lvwAccess.Items.Count
                If lvwAccess.Items.Item(iTemp - 1).Selected Then
                    'Developer Guide No. (As per VB Code)
                    m_lReturn = DeleteClient(lvwAccess.Items.Item(iTemp - 1))
                    lvwAccess.Items.RemoveAt(iTemp - 1)
                Else
                    iTemp += 1
                End If
            Loop

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteClients Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteClients", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '********************************************************************
    '
    ' Function Name: DeleteClient()
    '
    ' Description: Deletes task from the database
    '********************************************************************

    Private Function DeleteClient(ByRef oListItem As ListViewItem) As Integer

        Dim result As Integer = 0
        Dim oNewListItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If oListItem Is Nothing Then
                Return result
            End If


            'Developer Guide No.49
            oNewListItem = lvwNoAccess.Items.Add(oListItem.Text, "FindImage")

            oNewListItem.Tag = Convert.ToString(oListItem.Tag)
            For i As Integer = 1 To 4
                ListViewHelper.GetListViewSubItem(oNewListItem, i).Text = ListViewHelper.GetListViewSubItem(oListItem, i).Text
            Next

            oNewListItem = Nothing
            m_bChangesHaveBeenMade = True

            Return result

        Catch excep As System.Exception



            'Error Section

            result = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Delete Client", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteClient", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Private Sub cmdAddAllClients_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddAllClients.Click

        m_lReturn = AddAllClients()
        If m_bChangesHaveBeenMade Then
            cmdApply.Enabled = True
        End If

    End Sub

    Private Sub cmdAddClients_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddClients.Click

        m_lReturn = AddClients()
        If m_bChangesHaveBeenMade Then
            cmdApply.Enabled = True
        End If

    End Sub



    Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click

        WriteToDb()

        cmdApply.Enabled = False

        If m_bListRefreshRequiredAfterApply Then
            cmdFind_Click(cmdFind, New EventArgs())
        End If

    End Sub

    Private Sub cmdDeleteAllClients_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteAllClients.Click

        m_lReturn = DeleteAllClients()
        If m_bChangesHaveBeenMade Then
            cmdApply.Enabled = True
        End If

    End Sub

    Private Sub cmdDeleteClients_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDeleteClients.Click

        m_lReturn = DeleteClients()
        If m_bChangesHaveBeenMade Then
            cmdApply.Enabled = True
        End If

    End Sub

    Private Sub cmdExit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdExit.Click

        Dim iResponse As DialogResult
        m_bCancel = False
        ' Check if changes have been made
        If m_bChangesHaveBeenMade Then
            iResponse = MessageBox.Show("Changes have been made to the current set of " & _
                        "Clients. Do you wish to save these changes before " & _
                        "Exiting?", "Unsaved Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
            m_bCancel = True
            Select Case iResponse
                Case System.Windows.Forms.DialogResult.Yes
                    m_bListRefreshRequiredAfterApply = False
                    cmdApply_Click(cmdApply, New EventArgs())
                Case System.Windows.Forms.DialogResult.Cancel
                    Exit Sub
            End Select
        End If

        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        Me.Hide()

    End Sub

    Private Sub cmdFind_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFind.Click

        Dim iResponse As DialogResult

        Try

            ' Check if changes have been made
            If m_bChangesHaveBeenMade Then
                iResponse = MessageBox.Show("Changes have been made to the current set of " & _
                            "Clients. Do you wish to save these changes before " & _
                            "Finding a new set of Clients?", "Unsaved Changes", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question)
                Select Case iResponse
                    Case System.Windows.Forms.DialogResult.Yes
                        m_bListRefreshRequiredAfterApply = False
                        cmdApply_Click(cmdApply, New EventArgs())
                    Case System.Windows.Forms.DialogResult.Cancel
                        Exit Sub
                End Select
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Clear the search details.
            lvwNoAccess.Items.Clear()
            lvwAccess.Items.Clear()

            ' Set the buttons status
            cmdOK.Enabled = False
            cmdApply.Enabled = False
            cmdExit.Enabled = False

            ' Get the interface details from the business object.
            m_lReturn = GetBusiness()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the details.
            End If

            ' Assign the details from the search data storage
            ' to the interface.
            m_lReturn = DataToInterface()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to assign the details.
            End If

            If Information.IsArray(m_vSearchData) Then
                VB6.SetDefault(cmdFind, False)
                VB6.SetDefault(cmdOK, True)
            End If

            ' Set the focus.
            lvwNoAccess.Focus()

            ' Set the buttons status
            cmdOK.Enabled = True
            cmdExit.Enabled = True

            m_bListRefreshRequiredAfterApply = True
            m_bChangesHaveBeenMade = False

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception




            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Find command button", vApp:=ACApp, vClass:=ACClass, vMethod:="CmdFind_Click", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

        End Try

    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        'Set status to PMOK
        m_lStatus = gPMConstants.PMEReturnCode.PMOK

        If m_bChangesHaveBeenMade Then
            If Not WriteToDb() Then
                ' There was failure, so refresh lists and don't exit
                cmdFind_Click(cmdFind, New EventArgs())
                Exit Sub
            End If
        End If

        'hide this form
        Me.Hide()

    End Sub

    Private Sub frmInterface_Activated(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Activated
        If Not (ActivateHelper.myActiveForm Is eventSender) Then
            ActivateHelper.myActiveForm = eventSender

            With uctPMResizer1
                .SetControlResizeOption("cmdOK", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                .SetControlResizeOption("cmdCancel", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                .SetControlResizeOption("cmdHelp", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROPositionOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                .SetControlResizeOption("tabclients", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROSizeOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

                .SetControlResizeOption("lvwNoAccess", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROHeightOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)
                .SetControlResizeOption("lvwAccess", PMResizerControl.uctPMResizer.PMEControlResizeOptions.pmeCROHeightOnly, PMResizerControl.uctPMResizer.PMEControlResizeTypes.pmeCRTRelativeToBottomRight)

            End With
        End If
    End Sub

    Private Sub Form_Initialize_Renamed()

        ' Add the Hook
        iPMFunc.ShowFormInTaskBar_Attach()

        ' Initialise the error number value.
        m_lError = gPMConstants.PMEReturnCode.PMTrue

        'Initialise the form using selected function
        m_lReturn = InitialForm()

    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        ' Remove the Hook
        iPMFunc.ShowFormInTaskBar_Detach()

        ' Check if we have had an error so far.
        ' Possibly creating the business object.
        If m_lError = gPMConstants.PMEReturnCode.PMFalse Then
            ' We have already encountered an error,
            ' so we MUST exit now.
            Exit Sub
        End If

        ' Populate the client type combo
        cmbType.Items.Clear()
        cmbType.Items.Insert(0, "<ALL>")
        cmbType.Items.Insert(1, PMBConst.PMBPartyTypePersonalClientText)
        cmbType.Items.Insert(2, PMBConst.PMBPartyTypeCorporateClientText)
        cmbType.Items.Insert(3, PMBConst.PMBPartyTypeGroupClientText)

        With uctPMResizer1
            .NoResizeByDefault = True
            .FormMinHeight = 6645
            .FormMinWidth = 9405
        End With

    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        If m_bCancel = False Then
            cmdExit_Click(cmdExit, New EventArgs())
            Cancel = Not (m_lStatus = gPMConstants.PMEReturnCode.PMCancel)
        Else
            Cancel = gPMConstants.PMEReturnCode.PMCancel
        End If

        eventArgs.Cancel = Cancel <> 0

    End Sub

    Private Sub lvwNoAccess_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwNoAccess.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwNoAccess.Columns(eventArgs.Column)
        _hScrollValue = GetScrollPos(lvwNoAccess.Handle, SBS_HORZ)

        With lvwNoAccess

            ListViewHelper.SetSortedProperty(lvwNoAccess, False)
            ListViewHelper.SetSortKeyProperty(lvwNoAccess, ColumnHeader.Index + 1 - 1)
            ListViewHelper.SetSortedProperty(lvwNoAccess, True)

        End With
        LockWindowUpdate(lvwNoAccess.Handle)
        Dim dx As Integer = _hScrollValue - GetScrollPos(lvwNoAccess.Handle, SBS_HORZ)
        Dim b As Boolean = SendMessage(lvwNoAccess.Handle, LVM_SCROLL, dx, 0)
        LockWindowUpdate(IntPtr.Zero)
    End Sub

    <DllImport("user32.dll")> _
      Private Shared Function GetScrollPos(ByVal hWnd As System.IntPtr, ByVal nBar As Integer) As Integer

    End Function
    <DllImport("user32.dll")> _
    Private Shared Function SendMessage(ByVal hWnd As System.IntPtr, ByVal Msg As UInteger, ByVal wParam As Integer, ByVal lParam As Integer) As Boolean

    End Function
    <DllImport("user32.dll")> _
    Private Shared Function LockWindowUpdate(ByVal Handle As IntPtr) As Boolean

    End Function
   
    Private Sub lvwNoAccess_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwNoAccess.DoubleClick


        ' Edit details of user if doubleclicked
        Dim oListItem As ListViewItem = lvwNoAccess.GetItemAt(m_lXPos, m_lYPos)

        cmdAddClients_Click(cmdAddClients, New EventArgs())

    End Sub

    Private Sub lvwNoAccess_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwNoAccess.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No.70
        'start
        Dim X As Single = eventArgs.X
        Dim Y As Single = eventArgs.Y
        'end

        m_lXPos = CInt(X)
        m_lYPos = CInt(Y)

    End Sub

    Private Sub lvwAccess_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwAccess.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwAccess.Columns(eventArgs.Column)
        _hScrollValue = GetScrollPos(lvwAccess.Handle, SBS_HORZ)
        With lvwAccess

            ListViewHelper.SetSortedProperty(lvwAccess, False)
            ListViewHelper.SetSortKeyProperty(lvwAccess, ColumnHeader.Index + 1 - 1)
            ListViewHelper.SetSortedProperty(lvwAccess, True)

        End With
        LockWindowUpdate(lvwAccess.Handle)
        Dim dx As Integer = _hScrollValue - GetScrollPos(lvwAccess.Handle, SBS_HORZ)
        Dim b As Boolean = SendMessage(lvwAccess.Handle, LVM_SCROLL, dx, 0)
        LockWindowUpdate(IntPtr.Zero)
    End Sub

    Private Sub lvwAccess_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAccess.DoubleClick


        ' Edit details of user if doubleclicked
        Dim oListItem As ListViewItem = lvwNoAccess.GetItemAt(m_lXPos, m_lYPos)

        cmdDeleteClients_Click(cmdDeleteClients, New EventArgs())

    End Sub

    Private Sub lvwAccess_MouseMove(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwAccess.MouseMove
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'Developer Guide No.70
        'start
        Dim X As Single = eventArgs.X
        Dim Y As Single = eventArgs.Y
        'end

        m_lXPos = CInt(X)
        m_lYPos = CInt(Y)

    End Sub

    ' ***************************************************************** '
    ' Name: DataToInterface
    '
    ' Description: Updates all interface details from the search data.
    '              storage.
    '
    ' ***************************************************************** '
    Public Function DataToInterface() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Dim lAccessRowCount As gPMConstants.PMEFormatStyle
        Dim lNoAccessRowCount As gPMConstants.PMEFormatStyle

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the interface details.

            ' Check that search details are valid before
            ' continuing.
            If Not Information.IsArray(m_vSearchData) Then
                Return result
            End If

            ' Assign the details to the interface.
            For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)

                ' Decide which list to add client to
                If CStr(m_vSearchData(ACIOnlineAccess, lRow)) <> "True" Then

                    'Developer Guide No.49
                    oListItem = lvwNoAccess.Items.Add(CStr(m_vSearchData(ACIShortName, lRow)).Trim(), "FindImage")
                    lNoAccessRowCount = CType(lNoAccessRowCount + 1, gPMConstants.PMEFormatStyle)
                Else

                    'Developer Guide No.49
                    oListItem = lvwAccess.Items.Add(CStr(m_vSearchData(ACIShortName, lRow)).Trim(), "FindImage")
                    lAccessRowCount = CType(lAccessRowCount + 1, gPMConstants.PMEFormatStyle)
                End If

                ' Assign details to other the columns
                ' Column 2 Long Name
                ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnName - 1).Text = CStr(m_vSearchData(ACILongName, lRow)).Trim()

                ' Column 3 Address Line 1
                ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnAddressLine1 - 1).Text = CStr(m_vSearchData(ACIAddress1, lRow)).Trim()

                'sj 14/06/2002 - start
                ' Column 4 Address Line 2
                ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnAddressLine2 - 1).Text = CStr(m_vSearchData(ACIAddress2, lRow)).Trim()
                'sj 14/06/2002 - end

                ' Column 5 Post Code
                ListViewHelper.GetListViewSubItem(oListItem, g_kLvwColumnPostCode - 1).Text = CStr(m_vSearchData(ACIPostalCode, lRow)).Trim()


                ' Set the tag property with the index of
                ' the search data storage.
                oListItem.Tag = CStr(lRow)

                ' Refresh the first X amount of rows, to
                ' allow the user to see the results instantly.
                If lAccessRowCount = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                    ' Select the first item.
                    lvwAccess.Items.Item(0).Selected = True

                    ' Refresh the initial results.
                    lvwAccess.Refresh()
                End If
                If lNoAccessRowCount = gPMConstants.PMEFormatStyle.PMListRefreshValue Then
                    ' Select the first item.
                    lvwNoAccess.Items.Item(0).Selected = True

                    ' Refresh the initial results.
                    lvwNoAccess.Refresh()
                End If
                'End If
            Next lRow

            ' Select the first item.
            If lvwAccess.Items.Count > 0 Then
                lvwAccess.Items.Item(0).Selected = True

                '        m_lPartyCnt = CLng(m_vSearchData(ACIPartyCnt, lvwSearchDetails.SelectedItem.Tag))

            End If
            If lvwNoAccess.Items.Count > 0 Then
                lvwNoAccess.Items.Item(0).Selected = True

                '        m_lPartyCnt = CLng(m_vSearchData(ACIPartyCnt, lvwSearchDetails.SelectedItem.Tag))

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the search data storage", vApp:=ACApp, vClass:=ACClass, vMethod:="DataToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            tabClients.SelectedIndex = 0
        End If
    End Sub
End Class
