
Option Strict Off
Option Explicit On

Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles

Partial Friend Class frmMIDRules
    Inherits System.Windows.Forms.Form

    Public Const m_nFormCode As Integer = 0

    ' Status members
    Private Property m_sProcessStatus() As String

#Region "Public Variables"

    Public m_aoMIDRulesArray(,) As Object

#End Region

#Region "Public Property"

    Public ReadOnly Property ErrorNumber() As Integer
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_nErrorNumber

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
            Return m_nStatus

        End Get
    End Property

    Public Property Task() As Integer
        Get

            ' Return the objects task.
            Return m_nTask

        End Get
        Set(ByVal Value As Integer)

            ' Standard Property.
            ' Set the objects task.
            m_nTask = Value

        End Set
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the navigate flag.
            m_nNavigate = Value

        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)

            ' Standard Property.

            ' Set the process mode.
            m_nProcessMode = Value

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

#End Region

#Region "Private Variables"

    Private m_sCallingAppName As String = ""
    Private m_nStatus As Integer
    Private m_nErrorNumber As gPMConstants.PMEReturnCode
    Private m_nTask As gPMConstants.PMEComponentAction
    Private m_nNavigate As Integer
    Private m_nProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iSIRMIDMaintenance.General
    ' Declare an instance of the Business object.
    Private m_oBusiness As Object
    ' Form control
    Private m_oFormfields As Object

    ' Variables to store the lookup values/details.
    Private m_aoLookupValues(,) As Object
    Private m_aoLookupDetails(,) As Object
    Private m_aoTaskGroupUserGroups(,) As Object
    Private m_aoTaskGroupTask(,) As Object
    Private m_aoLookupTables(,) As Object
    Private m_aoValidInsuranceFileStatuses(,) As Object

    Private m_nReturn As Integer
    'Private m_oListData As Object
    'Private m_nSelectedRow As Integer


    Private m_nSortKey As Integer

#End Region

#Region "Private Constants"

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmMIDRules"

    Private Const MIDRuleID As Integer = 0
    Private Const MIDRuleCode As Integer = 1
    Private Const MIDRuleDesc As Integer = 2
    Private Const MIDRuleEffectiveDate As Integer = 3
    Private Const MIDRuleStartDate As Integer = 4
    Private Const MIDRuleExpiryDate As Integer = 5
    Private Const MIDRuleType As Integer = 6
    Private Const MIDRuleSupplierTypeID As Integer = 7
    Private Const MIDRuleSupplierID As Integer = 8
    Private Const MIDRuleInsurerID As Integer = 9
    Private Const MIDDelegatedAuthorityID As Integer = 10
    Private Const MIDRuleSiteNumber As Integer = 11
    Private Const MIDRuleUserGroupID As Integer = 12
    Private Const MIDRuleTaskGroupID As Integer = 13
    Private Const MIDRuleFileName As Integer = 14
    Private Const MIDRuleTestIndicator As Integer = 15
    Private Const MIDRuleFileSeqNumStart As Integer = 16
    Private Const MIDRuleCurrentFileSeqNum As Integer = 17
    Private Const MIDRuleSourceId As Integer = 18
    Private Const MIDRuleSupplierTypeCode As Integer = 19
    Private Const MIDRuleTaskGroupCode As Integer = 20
    Private Const MIDRuleUserGroupCode As Integer = 21
    Private Const MIDisDeleted As Integer = 22
    Private Const MIDDABranchDesc As Integer = 23

    Private Const ACYesChar As String = "Y"

#End Region

#Region "Public Functions"

    ''' <summary>
    ''' Set the Process
    ''' </summary>
    ''' <param name="sProcessStatus"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetStatus(ByRef sProcessStatus As String) As Integer
        Dim nResult As PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue
        Try
            ' Assign the current Status settings.
            m_sProcessStatus = sProcessStatus
        Catch Excep As Exception
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Information.Err().Number, vErrDesc:=Excep.Message, excep:=Excep)
            nResult = gPMConstants.PMEReturnCode.PMError
        End Try

        Return nResult
    End Function

    ''' <summary>
    ''' Gets valid Source ID's  and if nessessary displays selection
    ''' </summary>
    ''' <param name="m_iCompanyID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCompany(ByRef m_iCompanyID As Integer) As Integer
        Dim nResult As PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue
        Try
            Dim oBranch As iPMBBranch.Interface_Renamed
            Dim oTemp_oBranch As Object
            nResult = g_oObjectManager.GetInstance(oTemp_oBranch, sClassName:="iPMBBranch.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If
            oBranch = oTemp_oBranch
            nResult = oBranch.GetSource(iSourceID:=m_iCompanyID)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            oBranch.Dispose()
            oBranch = Nothing
        Catch Excep As Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Company", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCompany", excep:=Excep)
            nResult = gPMConstants.PMEReturnCode.PMError
        End Try

        Return nResult
    End Function

    ''' <summary>
    '''  Retrieves the details from the business object.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetBusiness() As Integer

        Dim nResult As PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue
        Try
            nResult = m_oBusiness.GetMIDRules(nSourceID:=cboPMLookupSource.SelectedValue, r_aoResultArray:=m_aoMIDRulesArray)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue AndAlso m_nReturn <> PMEReturnCode.PMNotFound Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness")
                Return nResult
            End If
        Catch Excep As Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBusiness", vErrNo:=Information.Err().Number, vErrDesc:=Excep.Message, excep:=Excep)
            nResult = gPMConstants.PMEReturnCode.PMError
        End Try
        Return nResult
    End Function

    ''' <summary>
    ''' Updates all interface details from the business object
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function BusinessToInterface() As Integer
        Dim nResult As PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue
        Try
            'Poulate the listview with the results
            PopulateList()
        Catch Excep As Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details from the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="BusinessToInterface", vErrNo:=Information.Err().Number, vErrDesc:=Excep.Message, excep:=Excep)
            nResult = gPMConstants.PMEReturnCode.PMError
        End Try
        Return nResult
    End Function

    ''' <summary>
    ''' Updates all business members from the interface details
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function InterfaceToBusiness() As Integer
        Dim nResult As PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue
        Try
            ' Check the task.
            Select Case (m_nTask)
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Inform the business object with a new data item.
                    ' m_lReturn& = m_oBusiness.DirectAdd()
                Case gPMConstants.PMEComponentAction.PMEdit
                    ' Inform the business object with an updated data item.
                    ' m_lReturn& = m_oBusiness.EditUpdate( ))
            End Select
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to assign the interface details to business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness")
            End If
        Catch Excep As Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the business object", vApp:=ACApp, vClass:=ACClass, vMethod:="InterfaceToBusiness", vErrNo:=Information.Err().Number, vErrDesc:=Excep.Message, excep:=Excep)
            nResult = gPMConstants.PMEReturnCode.PMError
        End Try
        Return nResult
    End Function

#End Region

#Region "Events"

    ''' <summary>
    ''' Add Rules.Click event of the Add Button.
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click
        Try
            m_nReturn = ValidateBranch()
            If m_nReturn <> PMEReturnCode.PMTrue Then
                Exit Sub
            End If
            'Process the Add request
            m_nReturn = DetailsFormProcessWrapper(r_iTaskType:=PMEComponentAction.PMAdd, nMIDRuleId:=0)
            If m_nReturn <> PMEReturnCode.PMTrue Then
                Exit Sub
            End If
            GetBusiness()
            PopulateList()

        Catch Excep As Exception

            ' Error Section.
            m_nErrorNumber = Information.Err().Number
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Add button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAdd_Click", excep:=Excep)
            Exit Sub
        End Try

    End Sub

    ''' <summary>
    ''' Click event of the Close button
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cmdClose_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClose.Click
        Try
            ' Set the interface status.
            m_nStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_nReturn = m_oGeneral.ProcessCommand(r_bChangesMade:=False)
            If m_nReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If
        Catch Excep As Exception
            ' Error Section.
            m_nReturn = gPMConstants.PMEReturnCode.PMError
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Close command button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdClose_Click", vErrNo:=Information.Err().Number, vErrDesc:=Excep.Message, excep:=Excep)
            Exit Sub
        End Try

    End Sub

    ''' <summary>
    ''' Delete/Undelete Rules.Click event of the Delete Button.
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click
        Dim oListItem As ListViewItem
        Dim nMIDRuleID As Integer
        Dim nIsDeleted As Integer
        Try
            'Only process if there is a selection made
            oListItem = lvwMIDRules.FocusedItem
            If oListItem Is Nothing Then
                'Display standard message
                DisplayMessage(r_nTitleId:=ACNoSelectionTitle, r_nMessageId:=ACNoSelectionDetails, r_nOptions:=MsgBoxStyle.Exclamation)
                Exit Sub
            End If
            nMIDRuleID = Convert.ToInt32(oListItem.SubItems(5).Text)
            nIsDeleted = Convert.ToInt32(oListItem.SubItems(6).Text)
            If nIsDeleted = 0 Then
                'Ensure that delete should proceed
                m_nReturn = DisplayMessage(r_nTitleId:=ACConfirmDeleteTitle, r_nMessageId:=ACConfirmDeleteDetails, r_nOptions:=MsgBoxStyle.YesNo + MsgBoxStyle.Question)
                If m_nReturn = System.Windows.Forms.DialogResult.No Then
                    Exit Sub
                End If
                m_nReturn = m_oBusiness.DeleteMIDRule(nMIDRuleId:=nMIDRuleID)
            Else
                m_nReturn = ConfirmSingularityOfActiveRule()
                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Dim sMessage As String
                    sMessage = String.Format("Active date range for this rule ovarlapes with another active rule for this branch. Hence this rule can not be Un-deleted.")
                    MessageBox.Show(sMessage, "Invalid Un-Delete.", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If
                m_nReturn = m_oBusiness.UnDeleteMIDRule(nMIDRuleId:=nMIDRuleID)
            End If
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                DisplayMessage(r_nTitleId:=ACRuleUsedTitle, r_nMessageId:=ACRuleUsedDetails, r_nOptions:=MsgBoxStyle.Exclamation)
                Exit Sub
            End If
            GetBusiness()
            PopulateList()
        Catch Excep As Exception
            m_nErrorNumber = Information.Err().Number
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Delete button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDelete_Click", excep:=Excep)
        End Try

    End Sub

    ''' <summary>
    ''' Edits existing Rule.Click event of the Delete Button.
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
        Dim nMIDRuleId As Integer
        Dim oListItem As ListViewItem
        Try
            'Only process if there is a selection made
            oListItem = lvwMIDRules.FocusedItem
            If oListItem Is Nothing Then
                'Display standard message
                DisplayMessage(r_nTitleId:=ACNoSelectionTitle, r_nMessageId:=ACNoSelectionDetails, r_nOptions:=MsgBoxStyle.Exclamation)
                Exit Sub
            End If
            nMIDRuleId = ToSafeInteger(oListItem.SubItems(5).Text)
            'Process the edit request
            m_nReturn = DetailsFormProcessWrapper(r_iTaskType:=gPMConstants.PMEComponentAction.PMEdit, nMIDRuleId:=nMIDRuleId)
            If m_nReturn <> PMEReturnCode.PMTrue Then
                Exit Sub
            End If
            GetBusiness()
            PopulateList()
        Catch Excep As Exception
            ' Error Section.
            m_nErrorNumber = Information.Err().Number
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the Edit button", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdEdit_Click", excep:=Excep)
        End Try

    End Sub

    ''' <summary>
    ''' Click Event of the MID rules listview
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lvwMIDRules_Click(sender As Object, e As EventArgs) Handles lvwMIDRules.Click
        Try
            Dim oListItem As ListViewItem = New ListViewItem
            'Only process if there is a selection made
            If lvwMIDRules.SelectedItems.Count = 1 Then
                oListItem = lvwMIDRules.FocusedItem
                Dim nIsDeleted As Integer = Conversion.Val(oListItem.SubItems(6).Text)
                If nIsDeleted = 0 Then
                    cmdDelete.Text = "Delete"
                    cmdEdit.Enabled = True
                Else
                    cmdDelete.Text = "Un-Delete"
                    cmdEdit.Enabled = False
                End If
                cmdDelete.Enabled = True
            Else
                cmdEdit.Enabled = False
                cmdDelete.Enabled = False
            End If
        Catch Excep As Exception
            ' Error Section.
            m_nErrorNumber = Information.Err().Number
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the list view click", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwMIDRules_Click", excep:=Excep)
        End Try
    End Sub

    ''' <summary>
    ''' Column click event for the search details
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lvwMIDRules_ColumnClick(sender As Object, e As ColumnClickEventArgs) Handles lvwMIDRules.ColumnClick
        Dim nReturn As PMEReturnCode = PMEReturnCode.PMTrue
        Dim ColumnHeader As ColumnHeader = lvwMIDRules.Columns(e.Column)
        Dim nOrder As SortOrder
        Dim nDirection As Integer
        Const kEffectiveDateColumn As Integer = 2
        If ListViewHelper.GetSortOrderProperty(lvwMIDRules) = SortOrder.Descending Then
            nOrder = SortOrder.Ascending
        Else
            nOrder = SortOrder.Descending
        End If
        ListViewHelper.SetSortedProperty(lvwMIDRules, True)

        Try
            With lvwMIDRules
                If ColumnHeader.Index + 1 - 1 = kEffectiveDateColumn Then

                    If m_nSortKey <> kEffectiveDateColumn Then
                        m_nSortKey = kEffectiveDateColumn
                        nDirection = nOrder
                    Else
                        nDirection = nOrder
                    End If
                    nReturn = CType(ListViewFunc.ListViewSortByDate(v_oListView:=lvwMIDRules, v_iSourceColumn:=kEffectiveDateColumn, v_iDirection:=nDirection), gPMConstants.PMEReturnCode)
                    ' If current sort column header is pressed.
                ElseIf (ColumnHeader.Index + 1 - 1 = m_nSortKey) Then
                    ' Set sort order opposite of current direction.
                    nDirection = nOrder
                    ListViewHelper.SetSortKeyProperty(lvwMIDRules, m_nSortKey)
                    ListViewHelper.SetSortOrderProperty(lvwMIDRules, nDirection)
                Else
                    ' Sort by this column (ascending).
                    ListViewHelper.SetSortedProperty(lvwMIDRules, False)

                    ' Turn off sorting so that the list is not sorted twice
                    ListViewHelper.SetSortOrderProperty(lvwMIDRules, nOrder)
                    ListViewHelper.SetSortKeyProperty(lvwMIDRules, ColumnHeader.Index + 1 - 1)
                    ListViewHelper.SetSortedProperty(lvwMIDRules, True)
                    nDirection = ListViewHelper.GetSortOrderProperty(lvwMIDRules)
                    m_nSortKey = ListViewHelper.GetSortKeyProperty(lvwMIDRules)
                End If
            End With

        Catch excep As System.Exception
            ' Error Section
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwProduct_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try
    End Sub

    Private Sub lvwMIDRules_ColumnWidthChanged(sender As Object, e As ColumnWidthChangedEventArgs) Handles lvwMIDRules.ColumnWidthChanged
        If (e.ColumnIndex > 4 AndAlso lvwMIDRules.Columns(e.ColumnIndex).Width > 0) Then
            lvwMIDRules.Columns(e.ColumnIndex).Width = 0
        End If
    End Sub


    ''' <summary>
    ''' Double click Event of the MID rules listview
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub lvwMIDRules_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwMIDRules.DoubleClick
        If cmdEdit.Enabled Then
            cmdEdit_Click(sender, e)
        End If
    End Sub

    ''' <summary>
    ''' Mouse Down Event of the MID rules listview
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub lvwMIDRules_MouseDown(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwMIDRules.MouseDown
        Try
            Dim x As Single = eventArgs.X
            Dim Y As Single = eventArgs.Y

            If Me.lvwMIDRules.GetItemAt(x, Y) Is Nothing Then
                cmdAdd.Enabled = True
                cmdEdit.Enabled = False
                cmdDelete.Enabled = False
            Else
                cmdEdit.Enabled = True
                cmdDelete.Enabled = True
            End If
        Catch Excep As Exception
            ' Error Section.
            m_nErrorNumber = Information.Err().Number
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process the list view mouse event", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwMIDRules_MouseDown", excep:=Excep)
        End Try
    End Sub

    ''' <summary>
    ''' Form load event
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        Try
            iPMFunc.ShowFormInTaskBar_Detach()
            ListViewHelper.SetSortedProperty(lvwMIDRules, True)

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_nErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                Exit Sub
            End If
            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            m_nReturn = iPMForms.SetFieldValidation(r_frmSource:=Me, r_oFormfields:=m_oFormfields)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Set the branch list
            m_nReturn = GetSourceCombo()
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_nErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            ' Set the interface default values.
            m_nReturn = SetInterfaceDefaults()
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_nErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            ' Gets the interface details to be displayed.
            m_nReturn = m_oGeneral.GetInterfaceDetails()
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_nErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            lvwMIDRules.FullRowSelect = True
            lvwMIDRules.Select()

            m_nSortKey = -1
        Catch Excep As Exception
            ' Error Section
            m_nErrorNumber = Information.Err().Number
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", excep:=Excep)
            Exit Sub
        End Try
    End Sub

    ''' <summary>
    ''' Form CLosing event handling
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing

        Dim nCancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim nUnloadMode As Integer = CInt(eventArgs.CloseReason)
        Try
            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_nStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.
            If nUnloadMode <> m_nFormCode Then
                'Process the next set of actions depending
                'upon the interface task etc.
                m_nReturn = m_oGeneral.ProcessCommand(r_bChangesMade:=False)
                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Do not procced with the interface termination.
                    eventArgs.Cancel = True
                    nCancel = 1
                    'Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()
            ' Destroy the instance of the general object from memory.
            m_oGeneral = Nothing

            ' Terminate the business object
            m_oBusiness.Dispose()
            ' Destroy the instance of the business object from memory.
            m_oBusiness = Nothing

            m_oFormfields.Dispose()
            m_oFormfields = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)
        Catch Excep As Exception
            ' Error Section.
            m_nErrorNumber = Information.Err().Number
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=Excep.Message, excep:=Excep)
            Exit Sub
            eventArgs.Cancel = nCancel <> 0
        End Try
    End Sub

    ''' <summary>
    ''' Event on Branch Selection Change
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub cboPMLookupSource_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPMLookupSource.SelectedIndexChanged
        'Populate the list
        PopulateList()

    End Sub

    ''' <summary>
    ''' Form Keydown event
    ''' </summary>
    ''' <param name="eventSender"></param>
    ''' <param name="eventArgs"></param>
    ''' <remarks></remarks>
    Private Sub frmInterface_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        If eventArgs.Alt And eventArgs.KeyCode = Keys.D1 Then
            tabMainTab.SelectedIndex = 0
        End If
    End Sub

#End Region

#Region "Private Functions"

    ''' <summary>
    ''' Sets all of the interface default values.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetInterfaceDefaults() As Integer

        Dim nResult As PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue
        Try
            ' Center the interface.
            iPMFunc.CenterForm(Me)
            ' Display all language specific captions.
            nResult = iPMForms.DisplayCaptions(Me, My.Resources.ResourceManager)

            ' Check for errors.
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Added call to SetExtraListViewProperties to select entire row
            'm_lReturn = SetExtraListViewProperties(v_hWndList:=Me.lvwMIDRules.Handle.ToInt32(), v_vShowRowSelect:=True)
            lvwMIDRules.FullRowSelect = True
            lvwMIDRules.MultiSelect = False
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False
            Return nResult

        Catch Excep As Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="SetInterfaceDefaults", vErrNo:=Information.Err().Number, vErrDesc:=Excep.Message, excep:=Excep)
            nResult = gPMConstants.PMEReturnCode.PMError
            Return nResult
        End Try

    End Function

    ''' <summary>
    ''' Customised Form Initialize event
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Form_Initialize_Renamed()
        Dim sMessage, sTitle As String
        Try
            iPMFunc.ShowFormInTaskBar_Attach()
            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_nErrorNumber = gPMConstants.PMEReturnCode.PMTrue
            m_nReturn = g_oObjectManager.GetInstance(m_oBusiness, "bSIRMIDMaintenance.Business", vInstanceManager:="ClientManager")
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_nErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.
                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_nLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_nLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If
            '  '' Create an instance of the general interface object.
            m_oGeneral = New iSIRMIDMaintenance.General()

            '' Call the initialise method passing this interface
            m_nReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_nErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If
            ' Set the interface status to cancelled. This is done
            ' so that any interface termination will be noted
            ' as cancelled except in the event of accepting the interface.
            m_nStatus = gPMConstants.PMEReturnCode.PMCancel
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        Catch Excep As Exception
            ' Error Section
            m_nErrorNumber = Information.Err().Number
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=Excep.Message, excep:=Excep)
        End Try
    End Sub

    ''' <summary>
    ''' Populate Rules list view
    ''' </summary>
    ''' <param name="v_nSourceID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PopulateList(Optional ByVal v_nSourceID As Integer = 0) As Integer
        Dim nResult As PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue
        Dim iLower, iUpper As Integer
        Dim lSourceID As Integer
        Try
            If v_nSourceID > 0 Then
                lSourceID = v_nSourceID
            Else
                'Check if a source has yet been selected
                If cboPMLookupSource.SelectedIndex >= 0 Then
                    lSourceID = cboPMLookupSource.SelectedValue
                Else
                    lSourceID = 0
                End If
            End If

            'Clear the existing items
            lvwMIDRules.Items.Clear()
            'Get array limits
            If gArrays.GetArrayBounds(r_vArray:=m_aoMIDRulesArray, r_lDimension:=gArrays.klRowDimension, r_lLower:=iLower, r_lUpper:=iUpper) Then
                'Turn off listview updating
                ' refer developer guide no. 170 (Latest solutions)
                ListViewFunc.ListViewBatchStart(lvwMIDRules)

                With lvwMIDRules.Items
                    'Loop through the results array and populate the listview
                    For lRow As Integer = iLower To iUpper

                        'Check that the source id of the row is a match
                        If CInt(m_aoMIDRulesArray(MIDRuleSourceId, lRow)) = lSourceID Then

                            'ADD a new listitem to the listview
                            AddOrEditListViewItem(r_oListItem:=Nothing, nRuleID:=CInt(m_aoMIDRulesArray(MIDRuleID, lRow)), _
                                                  sCode:=CStr(m_aoMIDRulesArray(MIDRuleCode, lRow)), _
                                                  sDescription:=CStr(m_aoMIDRulesArray(MIDRuleDesc, lRow)), _
                                                  dtEffectiveDate:=CDate(m_aoMIDRulesArray(MIDRuleEffectiveDate, lRow)), _
                                                  sMIDType:=CStr(m_aoMIDRulesArray(MIDRuleType, lRow)), _
                                                  sSupplierType:=CStr(m_aoMIDRulesArray(MIDRuleSupplierTypeCode, lRow)), _
                                                  nIsDeleted:=IIf(ToSafeBoolean(m_aoMIDRulesArray(MIDisDeleted, lRow)), 1, 0))
                        End If
                        Me.lvwMIDRules.Refresh()
                    Next lRow
                End With
                'Turn on listview updating
                ListViewFunc.ListViewBatchEnd()
                lvwMIDRules.FocusedItem = Nothing
                cmdEdit.Enabled = False
                cmdDelete.Enabled = False
            End If
        Catch Excep As Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to populate the list", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateList", excep:=Excep)
            'Turn on listview updating
            ListViewFunc.ListViewBatchEnd()
            m_nErrorNumber = Information.Err().Number
            nResult = gPMConstants.PMEReturnCode.PMError
        End Try
        Return nResult
    End Function

    ''' <summary>
    ''' Adds or edit and item to the rules listview 
    ''' </summary>
    ''' <param name="r_oListItem"></param>
    ''' <param name="nRuleID"></param>
    ''' <param name="sCode"></param>
    ''' <param name="sDescription"></param>
    ''' <param name="dtEffectiveDate"></param>
    ''' <param name="sMIDType"></param>
    ''' <param name="sSupplierType"></param>
    ''' <param name="nIsDeleted"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AddOrEditListViewItem(ByRef r_oListItem As ListViewItem, ByVal nRuleID As Integer, ByVal sCode As String, _
                                           ByVal sDescription As String, ByVal dtEffectiveDate As DateTime, _
                                           ByVal sMIDType As String, ByVal sSupplierType As String, ByVal nIsDeleted As Integer) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue

        'If a reference to a listitem is not passed we need to create a new one
        If r_oListItem Is Nothing Then
            'Add the new item
            r_oListItem = lvwMIDRules.Items.Add(sCode)
        Else
            'Edit the existing item
            r_oListItem.Text = sCode
        End If

        'Populate the subitems
        With r_oListItem
            ' Save the key so that it is easier to delete an item
            .Name = "Key" & nRuleID

            'LVI.SubItems.Add(comments.Tables(0).Rows(i).Item(2))    ' maybe .ToString?
            'LVI.SubItems.Add(comments.Tables(0).Rows(i).Item(3))
            r_oListItem.SubItems.Add(1).Text = sDescription
            r_oListItem.SubItems.Add(1).Text = CStr(dtEffectiveDate)
            r_oListItem.SubItems.Add(1).Text = sMIDType
            ' r_oListItem.SubItems.Add(1).Text = CStr(nRuleID)
            r_oListItem.SubItems.Add(1).Text = sSupplierType
            r_oListItem.SubItems.Add(1).Text = nRuleID
            r_oListItem.SubItems.Add(1).Text = nIsDeleted
            If nIsDeleted = 1 Then
                r_oListItem.ForeColor = Color.Gray
            End If
        End With

        Return nResult
    End Function

    ''' <summary>
    ''' Display details form and Add, Edit or View
    ''' </summary>
    ''' <param name="r_iTaskType"></param>
    ''' <param name="nMIDRuleId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DetailsFormProcess(ByRef r_iTaskType As Integer, nMIDRuleId As Integer) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue

        ofrmDetails = New frmMIDRuleConfiguration()

        ' Pass standard details to form properties
        With ofrmDetails

            .LookupTables = m_aoLookupTables
            .LookupDetails = m_aoLookupDetails
            .TaskGroupTasks = m_aoTaskGroupTask
            .TaskGroupUsers = m_aoTaskGroupUserGroups
            .CallingAppName = m_sCallingAppName
            .Task = r_iTaskType
            .Navigate = m_nNavigate
            .ProcessMode = m_nProcessMode
            .m_aoRulesArray = m_aoMIDRulesArray
            If r_iTaskType = PMEComponentAction.PMEdit Then
                For iRow As Integer = 0 To UBound(m_aoMIDRulesArray, 2)
                    If m_aoMIDRulesArray(MIDRuleID, iRow) = nMIDRuleId Then
                        .MIDRuleID = ToSafeString(m_aoMIDRulesArray(MIDRuleID, iRow))
                        .MIDRuleCode = ToSafeString(m_aoMIDRulesArray(MIDRuleCode, iRow))
                        .MIDRuleDescription = ToSafeString(m_aoMIDRulesArray(MIDRuleDesc, iRow))
                        .MIDRuleEffectiveDate = ToSafeDate(m_aoMIDRulesArray(MIDRuleEffectiveDate, iRow))
                        .MIDRuleExpiryDate = ToSafeDate(m_aoMIDRulesArray(MIDRuleExpiryDate, iRow))
                        .MIDRuleStartDate = ToSafeDate(m_aoMIDRulesArray(MIDRuleStartDate, iRow))
                        .MIDRuleMIDType = ToSafeString(m_aoMIDRulesArray(MIDRuleType, iRow))
                        .MIDRuleSupplierTypeID = ToSafeInteger(m_aoMIDRulesArray(MIDRuleSupplierTypeID, iRow))
                        .MIDRuleInsurerID = ToSafeInteger(m_aoMIDRulesArray(MIDRuleInsurerID, iRow))
                        .MIDRuleSupplierID = ToSafeInteger(m_aoMIDRulesArray(MIDRuleSupplierID, iRow))
                        .MIDRuleDelegateAuthorityID = ToSafeInteger(m_aoMIDRulesArray(MIDDelegatedAuthorityID, iRow))
                        .MIDRuleSitenumber = ToSafeInteger(m_aoMIDRulesArray(MIDRuleSiteNumber, iRow))
                        .MIDRuleTaskGroupId = ToSafeInteger(m_aoMIDRulesArray(MIDRuleTaskGroupID, iRow))
                        .MIDRuleUserGroupId = ToSafeInteger(m_aoMIDRulesArray(MIDRuleUserGroupID, iRow))
                        .MIDRuleSitenumber = ToSafeInteger(m_aoMIDRulesArray(MIDRuleSiteNumber, iRow))
                        .MIDRuleFileName = ToSafeString(m_aoMIDRulesArray(MIDRuleFileName, iRow))
                        .MIDRuleTestIndicator = ToSafeBoolean(m_aoMIDRulesArray(MIDRuleTestIndicator, iRow))
                        .MIDRuleFileSequenceNumberStart = ToSafeString(m_aoMIDRulesArray(MIDRuleFileSeqNumStart, iRow))
                        .MIDRuleCurrenctFileSequenceNumber = ToSafeString(m_aoMIDRulesArray(MIDRuleCurrentFileSeqNum, iRow))
                        .MIDRuleSupplierTypeCode = ToSafeString(m_aoMIDRulesArray(MIDRuleSupplierTypeCode, iRow))
                        .MIDRuleTaskGroupCode = ToSafeString(m_aoMIDRulesArray(MIDRuleTaskGroupCode, iRow))
                        .MIDRuleUserGroupCode = ToSafeString(m_aoMIDRulesArray(MIDRuleUserGroupCode, iRow))
                    End If
                Next
            End If
            ' Check the task.
            Select Case (r_iTaskType)
                Case gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMAdd
                    If cboPMLookupSource.SelectedIndex >= 0 Then
                        .DefaultSourceID = cboPMLookupSource.SelectedValue
                    End If
            End Select
        End With

        'Show the form
        ofrmDetails.ShowDialog()
        Return nResult
    End Function

    ''' <summary>
    ''' Close the Details form
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UnloadfrmDetails() As Integer
        Dim nResult As PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue
        Try
            ofrmDetails.Close()
            ofrmDetails = Nothing
        Catch Excep As Exception
            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="UnloadfrmDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=Excep)
            nResult = gPMConstants.PMEReturnCode.PMError
            m_nErrorNumber = Information.Err().Number
        End Try
        Return nResult
    End Function

    ''' <summary>
    ''' Process details form afetr and add or update
    ''' </summary>
    ''' <param name="r_iTaskType"></param>
    ''' <param name="nMIDRuleId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DetailsFormProcessWrapper(ByRef r_iTaskType As Integer, nMIDRuleId As Integer) As Integer
        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Try
            ' Show the Details form in Add mode
            nResult = DetailsFormProcess(r_iTaskType, nMIDRuleId)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                m_nErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If
            'Refresh the list if changes written to db
            If ofrmDetails.Status <> gPMConstants.PMEReturnCode.PMCancel Or ofrmDetails.Applied Then

                'Re-query to get updated details
                nResult = GetBusiness()
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_nErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    Return nResult
                End If

                'Populate the list
                nResult = PopulateList(cboPMLookupSource.SelectedValue)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_nErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                    Return nResult
                End If
            End If
            UnloadfrmDetails()
        Catch Excep As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="DetailsFormProcessWrapper", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=Excep)
            m_nErrorNumber = Information.Err().Number
            nResult = gPMConstants.PMEReturnCode.PMError
        End Try
        Return nResult
    End Function

    ''' <summary>
    ''' Get the list of branches
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetSourceCombo() As Integer
        Dim nResult As PMEReturnCode = PMEReturnCode.PMTrue
        Try
            Dim oPMUser As bPMUser.Business
            Dim aoSourceArray(,) As Object = Nothing
            Dim oTemp_oUser As Object

            nResult = g_oObjectManager.GetInstance(oTemp_oUser, "bPMUser.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMUser", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSourceCombo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return nResult
            End If
            oPMUser = oTemp_oUser

            'Only populate combo with addresses the user is authorised to access.
            nResult = oPMUser.GetUserSources(r_vSourceArray:=aoSourceArray, v_vUserID:=g_oObjectManager.UserID, v_bIncludeDeletedSources:=False)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get authorised brances", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSourceCombo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Return nResult
            End If
            'Clear combo.
            cboPMLookupSource.Items.Clear()
            Dim table As New DataTable

            ' Create four typed columns in the DataTable.
            table.Columns.Add("Code", GetType(String))
            table.Columns.Add("SourceID", GetType(Integer))
            Dim drBlank As DataRow = table.NewRow
            drBlank("Code") = String.Empty
            drBlank("SourceID") = -1
            table.Rows.Add(drBlank)

            'Populate branch combo
            If Information.IsArray(aoSourceArray) Then
                For i As Integer = 0 To aoSourceArray.GetUpperBound(1)
                    Dim dr As DataRow = table.NewRow
                    dr("Code") = aoSourceArray(2, i).ToString.Trim
                    dr("SourceID") = ToSafeInteger(aoSourceArray(0, i))
                    table.Rows.Add(dr)
                Next i
                cboPMLookupSource.DisplayMember = "Code"
                cboPMLookupSource.ValueMember = "SourceID"
            End If
            cboPMLookupSource.DataSource = table
            oPMUser.Dispose()
            oPMUser = Nothing
        Catch Excep As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSourceComboFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSourceCombo", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=Excep)
            m_nErrorNumber = Information.Err().Number
            nResult = gPMConstants.PMEReturnCode.PMError
        End Try
        Return nResult
    End Function

    ''' <summary>
    ''' Validate branch selection
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ValidateBranch() As Integer
        Dim nResult As PMEReturnCode = PMEReturnCode.PMTrue
        Try
            If cboPMLookupSource.SelectedIndex <= 0 Then
                MessageBox.Show("Please select Branch" & Strings.Chr(9), "Invalid branch", MessageBoxButtons.OK, MessageBoxIcon.Information)
                If cboPMLookupSource.Enabled Then
                    cboPMLookupSource.Focus()
                End If
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If
        Catch Excep As Exception
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateBranchFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateBranch", vErrNo:=Information.Err().Number, vErrDesc:=Excep.Message, excep:=Excep)
            m_nErrorNumber = Information.Err().Number
            nResult = gPMConstants.PMEReturnCode.PMError
        End Try
        Return nResult
    End Function

    ''' <summary>
    ''' Checks that date range for new rule being added does not overlap with any existing rule
    ''' </summary>
    ''' <param name="sMIDRuleType"></param>
    ''' <param name="dtStartDate"></param>
    ''' <param name="dtExpiryDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ConfirmSingularityOfActiveRule()
        Dim nResult As PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue
        Dim nLower, nUpper, nIsDeleted, nMIDRuleId, nMIDRuleSourceId As Integer
        Dim sMIDRuleType As String
        Dim dtStartDate, dtExpiryDate As Date
        Const sFunctionName As String = "ConfirmSingularityOfActiveRule"

        'Only process if there is a selection made
        If lvwMIDRules.SelectedItems.Count = 1 Then
            Dim oListItem As ListViewItem = New ListViewItem
            oListItem = lvwMIDRules.FocusedItem
            nMIDRuleId = Convert.ToInt32(oListItem.SubItems(5).Text)
            nIsDeleted = Convert.ToInt32(oListItem.SubItems(6).Text)
            nMIDRuleSourceId = Convert.ToInt32(cboPMLookupSource.SelectedValue)
            For iRow As Integer = 0 To UBound(m_aoMIDRulesArray, 2)
                If m_aoMIDRulesArray(MIDRuleID, iRow) = nMIDRuleId Then
                    sMIDRuleType = ToSafeString(m_aoMIDRulesArray(MIDRuleType, iRow))
                    dtStartDate = ToSafeDate(m_aoMIDRulesArray(MIDRuleStartDate, iRow))
                    dtExpiryDate = ToSafeDate(m_aoMIDRulesArray(MIDRuleExpiryDate, iRow))
                End If
            Next
            Try
                'Get array limits
                If gArrays.GetArrayBounds(r_vArray:=m_aoMIDRulesArray, r_lDimension:=gArrays.klRowDimension, r_lLower:=nLower, r_lUpper:=nUpper) Then
                    For lRow As Integer = nLower To nUpper
                        If Not nMIDRuleId = CInt(m_aoMIDRulesArray(MIDRuleID, lRow)) Then
                            If nMIDRuleSourceId = CInt(m_aoMIDRulesArray(MIDRuleSourceId, lRow)) AndAlso _
                                sMIDRuleType = ToSafeString(m_aoMIDRulesArray(MIDRuleType, lRow)) AndAlso _
                                Not ToSafeBoolean(m_aoMIDRulesArray(MIDisDeleted, lRow)) Then

                                'for selected branch check that there is no datae range over lap
                                If (dtStartDate >= ToSafeDate(m_aoMIDRulesArray(MIDRuleStartDate, lRow)) And String.IsNullOrEmpty(m_aoMIDRulesArray(MIDRuleExpiryDate, lRow))) Or _
                                     (dtStartDate >= ToSafeDate(m_aoMIDRulesArray(MIDRuleStartDate, lRow)) And dtStartDate <= ToSafeDate(m_aoMIDRulesArray(MIDRuleExpiryDate, lRow))) Or _
                                      (dtExpiryDate >= ToSafeDate(m_aoMIDRulesArray(MIDRuleStartDate, lRow)) And dtExpiryDate <= ToSafeDate(m_aoMIDRulesArray(MIDRuleExpiryDate, lRow))) _
                                      Then
                                    nResult = gPMConstants.PMEReturnCode.PMFalse
                                    Return nResult
                                End If
                            End If
                        End If
                    Next lRow
                End If
            Catch Excep As Exception
                ' Log Error.
                gPMFunctions.LogMessageToFile(sUsername:=g_oObjectManager.UserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=Excep)
                m_nErrorNumber = Information.Err().Number
                nResult = gPMConstants.PMEReturnCode.PMError
            End Try
        End If
        Return nResult
    End Function


#End Region

End Class
