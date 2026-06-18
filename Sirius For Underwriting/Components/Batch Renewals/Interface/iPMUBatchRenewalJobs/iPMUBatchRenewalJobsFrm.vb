Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 10 June 2008
    '
    ' Description: Main interface.
    '
    ' Edit History: Gurucharan Gulati
    ' ***************************************************************** '


    ' Constant for the functions to identify
    ' PUBLIC Events (Begin)
    Public m_vSearchData(,) As Object

    ' PRIVATE Events (End)

    Private Const ACClass As String = "frmInterface"

    ' PRIVATE Events (Begin)
    Private m_oGeneral As iPMUBatchRenewalJobs.General
    Private m_oBusiness As Object

    Private m_oPMUser As bPMUser.Business
    Private m_lReturn As gPMConstants.PMEReturnCode
    Private m_sCallingAppName As String = ""
    Private m_lErrorNumber As gPMConstants.PMEReturnCode
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lStatus As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_iBatchRenewalJobId As Integer
    Private Const vbFormCode As Integer = 0
    Private m_bAttachToScheduler As Boolean
    Private m_jobCode As String
    Private m_jobDescription As String
    Private m_jobType As String
    Private m_iStatus As Integer
    Private m_userName As String
    Private m_iBatchProcessId As Integer
    Private m_sbatchStatus As String
    Private m_sBatchProcessName As String
    Private m_sbatchContentDetails As String
    Dim m_dtParameters As DataTable = Nothing
    Private m_ibatchSchedulerId As Integer




    ' PRIVATE Events (End)

    ' PUBLIC Property (Begin)
    Public ReadOnly Property ErrorNumber() As Integer
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_lErrorNumber
        End Get
    End Property
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            ' Set the calling application name.
            m_sCallingAppName = Value
        End Set
    End Property
    Public Property Task() As Integer
        Get
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property
    Public ReadOnly Property Status() As Integer
        Get
            ' Return the interface exit status.
            Return m_lStatus
        End Get
    End Property
    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)
            ' Set the navigate flag.
            m_lNavigate = Value
        End Set
    End Property
    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)
            ' Set the process mode.
            m_lProcessMode = Value
        End Set
    End Property
    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            ' Set the type of business.
            m_sTransactionType = Value
        End Set
    End Property
    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)
            ' Set the effective date.
            m_dtEffectiveDate = Value
        End Set
    End Property
    ' PUBLIC Property (End)
    Public Property AttachToScheduler() As Boolean
        Get
            Return m_bAttachToScheduler
        End Get
        Set(ByVal Value As Boolean)
            m_bAttachToScheduler = Value
        End Set
    End Property
    Public Property BatchStatus() As Boolean
        Get
            Return m_sbatchStatus
        End Get
        Set(ByVal Value As Boolean)
            m_sbatchStatus = Value
        End Set
    End Property
    Public Property JobCode() As String
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_jobCode
        End Get
        Set(ByVal Value As String)
            m_jobCode = Value
        End Set
    End Property
    Public ReadOnly Property JobDescription() As Integer
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_jobDescription
        End Get
    End Property
    Public ReadOnly Property JobType() As Integer
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_jobType
        End Get
    End Property
    Public ReadOnly Property JobStatus() As Integer
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_iStatus
        End Get
    End Property
    Public ReadOnly Property UserName() As Integer
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_userName
        End Get
    End Property
    Public Property BatchProcessId() As Integer
        Get
            Return m_iBatchProcessId
        End Get
        Set(ByVal Value As Integer)
            m_iBatchProcessId = Value
        End Set
    End Property
    Public Property BatchProcessName() As String
        Get
            Return m_sBatchProcessName
        End Get
        Set(ByVal Value As String)
            m_sBatchProcessName = Value
        End Set
    End Property
    Public Property ProcessParameters() As DataTable
        Get
            Return m_dtParameters
        End Get
        Set(ByVal Value As DataTable)
            m_dtParameters = Value
        End Set
    End Property
    Public Property BatchFileContentDetails() As String
        Get
            ' Return any error number that might have
            ' occurred on the interface.
            Return m_sbatchContentDetails
        End Get
        Set(value As String)
            m_sbatchContentDetails = value
        End Set
    End Property
    Public Property BatchSchedulerId() As Integer
        Get
            Return m_ibatchSchedulerId
        End Get
        Set(ByVal Value As Integer)
            m_ibatchSchedulerId = Value
        End Set
    End Property
    Public Property BatchRenewalJobId() As Integer
        Get
            Return m_iBatchRenewalJobId
        End Get
        Set(ByVal Value As Integer)
            m_iBatchRenewalJobId = Value
        End Set
    End Property


    Public Function DataToInterface() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DataToInterface"
        Dim vBatch_Renewal_Job_Id As Object
        Dim dtCreated As Date
        Dim sJobCode, sDescription As String
        Dim iStatus As Integer
        Dim sJobType, sUsername As String
        Dim oListItem As ListViewItem
        Dim iSelJobCode As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = CType(GetRenewalJobs(v_vBatch_Renewal_Job_Id:=DBNull.Value), gPMConstants.PMEReturnCode)

        ' Clear the search details.
        lvwSearchDetails.Items.Clear()

        If (Not Information.IsArray(m_vSearchData)) Or (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            result = gPMConstants.PMEReturnCode.PMTrue
            cmdEdit.Enabled = False
            cmdDelete.Enabled = False
            cmdSuspend.Text = "Ac&tivate"
            GoTo Finally_Renamed
        End If

        If AttachToScheduler Then
            cmdSchedule.Visible = True
            ' cmdSchedule.Enabled = False
            cmdAdd.Visible = False
            cmdEdit.Visible = False
            cmdDelete.Visible = False
            cmdSuspend.Visible = False
        Else
            cmdSchedule.Visible = False
            cmdAdd.Visible = True
            cmdEdit.Visible = True
            cmdDelete.Visible = True
            cmdSuspend.Visible = True
        End If

        For lRow As Integer = m_vSearchData.GetLowerBound(1) To m_vSearchData.GetUpperBound(1)

            ' Assign details to other the columns
            oListItem = lvwSearchDetails.Items.Add(CStr(m_vSearchData(ACBatchRenewalJobId, lRow)).Trim())

            If Information.IsDate(m_vSearchData(ACBatchRenewalDateCreated, lRow)) Then
                dtCreated = CDate(m_vSearchData(ACBatchRenewalDateCreated, lRow))
                ListViewHelper.GetListViewSubItem(oListItem, kBatchRenewalColHIndexDateCreated).Text = dtCreated.ToString("dd MMM yyyy")
            Else
                ListViewHelper.GetListViewSubItem(oListItem, kBatchRenewalColHIndexDateCreated).Text = ""
            End If

            sJobCode = CStr(m_vSearchData(ACBatchRenewalCode, lRow))
            ListViewHelper.GetListViewSubItem(oListItem, kBatchRenewalColHIndexJobCode).Text = sJobCode.Trim()

            sDescription = CStr(m_vSearchData(ACBatchRenewalDescription, lRow))
            ListViewHelper.GetListViewSubItem(oListItem, kBatchRenewalColHIndexDesription).Text = sDescription.Trim()

            iStatus = gPMFunctions.ToSafeInteger(CStr(m_vSearchData(ACBatchRenewalIsActive, lRow)))
            If iStatus = 1 Then
                ListViewHelper.GetListViewSubItem(oListItem, kBatchRenewalColHIndexStatus).Text = "Active"
            Else
                ListViewHelper.GetListViewSubItem(oListItem, kBatchRenewalColHIndexStatus).Text = "Suspended"
            End If

            sJobType = CStr(m_vSearchData(ACBatchRenewalJobDescription, lRow))
            ListViewHelper.GetListViewSubItem(oListItem, kBatchRenewalColHIndexJobType).Text = sJobType.Trim()

            sUsername = CStr(m_vSearchData(ACBatchRenewalUserName, lRow))
            ListViewHelper.GetListViewSubItem(oListItem, kBatchRenewalColHIndexUser).Text = sUsername.Trim()
            If gPMFunctions.ToSafeInteger(m_vSearchData(ACBatchRenewalJobId, lRow)) = BatchRenewalJobId Then
                iSelJobCode = lRow
            End If
            oListItem.Tag = CStr(lRow)
        Next lRow

        If AttachToScheduler = True Then
            lvwSearchDetails.FullRowSelect = True
            lvwSearchDetails.Items.Item(iSelJobCode).Selected = True
            ' lvwSearchDetails.Items.Item(iSelJobCode).ForeColor = System.Drawing.SystemColors.WindowFrame
            lvwSearchDetails.Items.Item(iSelJobCode).Focused = True


        Else

            lvwSearchDetails.Items.Item(0).Selected = True
        End If


        ' Select the first item.


        If lvwSearchDetails.Items.Count > 0 Then
            cmdEdit.Enabled = True
            cmdDelete.Enabled = True
        End If
        EnableDisableInterfaceButton()
        GoTo Finally_Renamed



        ' Log Error.
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value)

Finally_Renamed:
        Return result
        Resume
        Return result
    End Function

    Private Sub cmdAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAdd.Click
        m_iTask = gPMConstants.PMEComponentAction.PMADD

        m_lReturn = ProcessCommand()
    End Sub

    Private Sub cmdClose_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClose.Click
        ' Click event of the Close button.
        Try



            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.pmcancel

            '    ' Process the next set of actions depending
            '    ' upon the interface task etc.
            '    m_lReturn = m_oGeneral.ProcessCommand()
            '
            '    ' Check the return value.
            '    If (m_lReturn = PMTrue) Then
            ' Everything OK, so we can hide the interface.
            Me.Hide()
            '    End If



        Catch ex As Exception

            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Close Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdClose_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click

        Try


            m_iTask = gPMConstants.PMEComponentAction.PMDelete
            m_lReturn = ProcessCommand()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If



        Catch ex As Exception

            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Delete Batch Renewal Job", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDelete_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click

        m_iTask = gPMConstants.PMEComponentAction.PMEdit
        m_lReturn = ProcessCommand()

    End Sub

    Private Sub cmdSuspend_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdSuspend.Click

        Dim iMsgResult As Integer
        Dim sMessage, sTitle As String



        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        Dim iIsActive As Integer = 1

        If Me.cmdSuspend.Text.ToUpper() = "&SUSPEND" Then
            iIsActive = 0
        End If

        m_lReturn = CType(SuspendJobs(iIsActive), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("cmdSuspend_Click", "Failed to Suspend status Batch Renewal")
            GoTo Finally_Renamed
        End If

        GoTo Finally_Renamed



        ' Error Section

        m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to do Suspend state", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdSuspend_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

Finally_Renamed:

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        Exit Sub
        Resume

    End Sub

    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.

        Try



            'Show In TaskBar
            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMUBatchRenewalJobs.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            Dim temp_m_oPMUser As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oPMUser, "bPMUser.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oPMUser = temp_m_oPMUser

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError("Form_Initialize", "Failed to get PMUser")
                Exit Sub
            End If

            m_lStatus = gPMConstants.PMEReturnCode.pmcancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        Catch ex As Exception

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialize interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initilize", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
    End Sub


    '	Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

    '		' Forms load event.

    '		On Error GoTo Catch_Renamed



    '		'Detach From TaskBar
    '		iPMFunc.ShowFormInTaskBar_Detach()

    '		' Check if we have had an error so far.
    '		' Possibly creating the business object.
    '		If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
    '			' We have already encountered an error,
    '			' so we MUST exit now.
    '			GoTo Finally_Renamed
    '		End If

    '		' Set the mouse pointer to busy.
    '		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

    '		' Check for errors.
    '		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '			' Failed to process the interface.
    '			m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

    '			' Raise Error
    '			gPMFunctions.RaiseError("Form_Load", "Failed to set the status for the business object")
    '			GoTo Finally_Renamed
    '		End If

    '		' Set the interface default values.
    '		m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

    '		' Check for errors.
    '		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '			m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

    '			' Set the mouse pointer to normal.
    '			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

    '			GoTo Finally_Renamed
    '		End If

    '		' Check for errors.
    '		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '			' Failed to get the interface details.
    '			m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

    '			' Set the mouse pointer to normal.
    '			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

    '			GoTo Finally_Renamed
    '		End If

    '		m_lReturn = DataToInterface()
    '		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '			' Failed to get the interface details.
    '			m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

    '			' Set the mouse pointer to normal.
    '			iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

    '			GoTo Finally_Renamed
    '		End If

    '		' Set the mouse pointer to normal.
    '		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

    '		GoTo Finally_Renamed

    'Catch_Renamed: 

    '		iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

    'Finally_Renamed: 
    '		Exit Sub
    '		Resume 

    '    End Sub
    Public Sub frmInterfaceLoad()

        ' Forms load event.

        Try



            'Detach From TaskBar
            iPMFunc.ShowFormInTaskBar_Detach()

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error,
                ' so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Raise Error
                gPMFunctions.RaiseError("Form_Load", "Failed to set the status for the business object")
                Exit Sub
            End If

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            m_lReturn = DataToInterface()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        Catch ex As Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally


        End Try
    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try



            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            'Developer Guide No.7
            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending
                ' upon the interface task etc.
                m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)

                ' Check the return value.
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.Cancel = True
                    ' Set the mouse pointer to normal.
                    iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                    Exit Sub
                End If
            End If

            If Not (m_oPMUser Is Nothing) Then


                m_oPMUser.Dispose()
                m_oPMUser = Nothing
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()

            ' Destroy the instance of the general object
            ' from memory.
            m_oGeneral = Nothing

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)



        Catch ex As Exception

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally



        End Try
    End Sub

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    ' Description: Sets all of the interface default values.
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetInterfaceDefaults"

        Dim iTemp As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If




        Catch ex As Exception

            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally




        End Try
        Return result
    End Function

    Private Sub lvwSearchDetails_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.Click

        EnableDisableInterfaceButton()

    End Sub

    Private Sub lvwSearchDetails_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwSearchDetails.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwSearchDetails.Columns(eventArgs.Column)
        ' Column click event for the search details

        Try


            With lvwSearchDetails

                ListViewHelper.SetSortOrderProperty(lvwSearchDetails, (ListViewHelper.GetSortOrderProperty(lvwSearchDetails) + 1) Mod 2)
                ListViewHelper.SetSortKeyProperty(lvwSearchDetails, ColumnHeader.Index + 1 - 1)
                ListViewHelper.SetSortedProperty(lvwSearchDetails, True)

            End With



        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to sort the column", vApp:=ACApp, vClass:=ACClass, vMethod:="lvwSearchDetails_ColumnClick", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally


        End Try
    End Sub

    Private Sub lvwSearchDetails_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSearchDetails.DoubleClick

        cmdEdit_Click(cmdEdit, New EventArgs())

    End Sub

    Private Sub EnableDisableInterfaceButton()
        Dim bFlgSuspended, bFlgActive As Boolean
        Me.cmdSuspend.Enabled = True
        If AttachToScheduler = True Then
            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                cmdSchedule.Text = "View &Schedule"
                cmdSchedule.Width = 87
                If lvwSearchDetails.FocusedItem IsNot Nothing Then
                    If lvwSearchDetails.FocusedItem.Selected Then
                        If m_iBatchRenewalJobId <> gPMFunctions.ToSafeInteger(lvwSearchDetails.SelectedItems(0).Text) Then

                            cmdSchedule.Enabled = False
                        Else
                            cmdSchedule.Enabled = True
                        End If
                    End If
                End If
            Else
                cmdSchedule.Enabled = True
            End If



        End If
        If lvwSearchDetails.Items.Count > 0 Then
            If Not lvwSearchDetails.FocusedItem Is Nothing AndAlso lvwSearchDetails.FocusedItem.Text <> "" Then
                For lRow As Integer = 1 To lvwSearchDetails.Items.Count
                    If lvwSearchDetails.Items.Item(lRow - 1).Selected And ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(lRow - 1), kBatchRenewalColHIndexStatus).Text.ToUpper() = "SUSPENDED" Then
                        bFlgSuspended = True
                    ElseIf lvwSearchDetails.Items.Item(lRow - 1).Selected And ListViewHelper.GetListViewSubItem(lvwSearchDetails.Items.Item(lRow - 1), kBatchRenewalColHIndexStatus).Text.ToUpper() <> "SUSPENDED" Then
                        bFlgActive = True
                    End If

                    If bFlgSuspended And bFlgActive Then
                        Me.cmdSuspend.Enabled = False
                        Exit For
                    End If
                Next

                If Me.cmdSuspend.Enabled Then
                    If bFlgSuspended Then
                        cmdSuspend.Text = "Ac&tivate"
                    End If

                    If bFlgActive Then
                        cmdSuspend.Text = "&Suspend"
                    End If
                End If
                cmdDelete.Enabled = True
            End If
        Else
            cmdDelete.Enabled = False
            cmdSuspend.Enabled = False
        End If
    End Sub

    ' ***************************************************************** '
    ' Name: ProcessCommand
    '
    ' Description: Determines which action to take on the details
    '              depending upon the task and interface state.
    '
    ' ***************************************************************** '
    Private Function ProcessCommand() As Integer

        Dim result As Integer = 0
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String
        Dim iCnt As Integer

        Const kMethodName As String = "ProcessCommand"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Check if form has been cancelled, if so, prompt
        ' if you wish to lose details.

        Select Case m_iTask
            Case gPMConstants.PMEComponentAction.PMADD
                m_lReturn = AddNewJobRenewalJob()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed to Add New Job", gPMConstants.PMELogLevel.PMLogError)
                End If

            Case gPMConstants.PMEComponentAction.PMEdit

                iCnt = 0
                If lvwSearchDetails.Items.Count > 0 Then
                    For lRow As Integer = 1 To lvwSearchDetails.Items.Count
                        If lvwSearchDetails.Items.Item(lRow - 1).Selected Then
                            iCnt += 1
                        End If
                    Next
                End If

                If iCnt > 1 Then

                    sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                    sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMultiSelect, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    GoTo Finally_Renamed
                End If

                m_lReturn = AddNewJobRenewalJob()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed to Edit Job", gPMConstants.PMELogLevel.PMLogError)
                End If

            Case gPMConstants.PMEComponentAction.PMDelete

                iCnt = 0
                If lvwSearchDetails.Items.Count > 0 Then
                    For lRow As Integer = 1 To lvwSearchDetails.Items.Count
                        If lvwSearchDetails.Items.Item(lRow - 1).Selected Then
                            iCnt += 1
                        End If
                    Next
                End If

                If iCnt > 1 Then

                    sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                    sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMultiSelectDelete, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                    iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    GoTo Finally_Renamed
                End If

                m_lReturn = DeleteJobRenewalJob()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed.", gPMConstants.PMELogLevel.PMLogError)
                End If

        End Select

        m_lReturn = DataToInterface()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            GoTo Finally_Renamed
        End If

        lvwSearchDetails.Focus()

        GoTo Finally_Renamed



        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

Finally_Renamed:

        Return result

        Resume
        Return result
    End Function

    Private Function AddNewJobRenewalJob() As Integer

        Dim result As Integer = 0
        Dim lStatus As Integer
        Dim iProcessTypeID As Integer

        Const kMethodName As String = "AddNewJobRenewalJob"


        result = gPMConstants.PMEReturnCode.PMTrue
        Dim ofrmBatchRenewalJob As New frmBatchRenewalJob
        If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
            If lvwSearchDetails.Items.Count = 0 Then
                GoTo Finally_Renamed
            End If
            'm_iBatchRenewalJobId = gPMFunctions.ToSafeInteger(lvwSearchDetails.FocusedItem.Text)
            m_iBatchRenewalJobId = gPMFunctions.ToSafeInteger(lvwSearchDetails.SelectedItems(0).Text)

        Else
            'On Add
            m_iBatchRenewalJobId = -1
        End If

        With ofrmBatchRenewalJob
            .Task = m_iTask
            .BatchRenewalJobId = m_iBatchRenewalJobId
            .ShowDialog()
            lStatus = .Status
        End With

        GoTo Finally_Renamed



        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

Finally_Renamed:
        Return result

        Resume

        Return result
    End Function

    Private Function DeleteJobRenewalJob() As Integer
        Dim result As Integer = 0
        Dim lDocLinkId As Integer

        Const kMethodName As String = "DeleteJobRenewalJob"



        result = gPMConstants.PMEReturnCode.PMTrue

        If lvwSearchDetails.Items.Count = 0 Then
            Return result
        End If

        Dim iBatchRenewalJobId As Integer = CInt(lvwSearchDetails.SelectedItems.Item(0).Text)


        Dim sTitle As String = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


        Dim sMessage As String = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteDetails, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

        Dim iMsgResult As DialogResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

        ' Check message result.
        If iMsgResult = System.Windows.Forms.DialogResult.Yes Then

            m_lReturn = g_oBusiness.DirectDelete(iBatchRenewalJobId:=iBatchRenewalJobId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to call Delete Record method DirectDelete")
            End If

            m_lReturn = DataToInterface()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GoTo Finally_Renamed
            End If

        End If

        GoTo Finally_Renamed



        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

Finally_Renamed:

        Return result

        Resume

        Return result
    End Function

    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DisplayCaptions"
        Dim sResValue As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.


            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Me.Text = "" Then
                ' Failed to get data from the resource file.
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error.
                iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

                Return result
            End If


            cmdAdd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdSuspend.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSuspendButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdClose.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCloseButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(SSTab1, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACTabTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Clear()
            lvwSearchDetails.BorderStyle = BorderStyle.Fixed3D
            lvwSearchDetails.FullRowSelect = True
            lvwSearchDetails.View = View.Details
            lvwSearchDetails.LabelEdit = False
            lvwSearchDetails.HideSelection = False

            'JobId

            sResValue = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSearchJobId, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Add(sResValue, CInt(VB6.TwipsToPixelsX(0)))

            'Created

            sResValue = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSearchCreated, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Add(sResValue, CInt(VB6.TwipsToPixelsX(1400)))

            'Job Code

            sResValue = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSearchJobCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Add(sResValue, CInt(VB6.TwipsToPixelsX(1400)))

            'Description

            sResValue = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSearchDescription, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Add(sResValue, CInt(VB6.TwipsToPixelsX(2700)))

            'Status

            sResValue = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSearchStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Add(sResValue, CInt(VB6.TwipsToPixelsX(1300)))

            'Job Type

            sResValue = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSearchJobType, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Add(sResValue, CInt(VB6.TwipsToPixelsX(1400)))

            'User

            sResValue = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACSearchUser, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwSearchDetails.Columns.Add(sResValue, CInt(VB6.TwipsToPixelsX(1500)))



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally






        End Try
        Return result
    End Function

    Private Function GetRenewalJobs(Optional ByRef v_vBatch_Renewal_Job_Id As Object = Nothing) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetRenewalJobs"


        result = gPMConstants.PMEReturnCode.PMTrue

        m_vSearchData = Nothing


        m_lReturn = g_oBusiness.GetRenewalJobs(v_vBatch_Renewal_Job_Id:=v_vBatch_Renewal_Job_Id, r_vResultArray:=m_vSearchData)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Failed to call bSIRBatchRenewalJobs.GetRenewalJobs")
        End If

        GoTo Finally_Renamed



        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value)

Finally_Renamed:
        Return result
        Resume
        Return result
    End Function

    Private Function SuspendJobs(ByVal v_iIsActive As Integer) As Integer

        Dim result As Integer = 0
        Dim lDocLinkId As Integer
        Dim iBatchRenewalJobId As Integer

        Const kMethodName As String = "SuspendJobs"



        result = gPMConstants.PMEReturnCode.PMTrue

        If lvwSearchDetails.Items.Count = 0 Then
            Return result
        End If


        Dim sTitle As String = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


        Dim sMessage As String = "Do you wish to suspend selected Job(s)?"
        If v_iIsActive = 1 Then
            sMessage = "Do you wish to activate selected Job(s)?"
        End If
        Dim iMsgResult As DialogResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

        ' Check message result.
        If iMsgResult = System.Windows.Forms.DialogResult.Yes Then
            For lRow As Integer = 1 To lvwSearchDetails.Items.Count
                If lvwSearchDetails.Items.Item(lRow - 1).Selected Then

                    iBatchRenewalJobId = gPMFunctions.ToSafeInteger(lvwSearchDetails.Items.Item(lRow - 1).Text)

                    m_lReturn = g_oBusiness.SuspendJobs(v_iBatchRenewalJobID:=iBatchRenewalJobId, v_iIsActive:=v_iIsActive)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Failed to Suspend Jobs : method SuspendJobs")
                    End If
                End If
            Next
            m_lReturn = DataToInterface()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GoTo Finally_Renamed
            End If
        End If

        GoTo Finally_Renamed



        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

Finally_Renamed:

        Return result

        Resume

        Return result
    End Function

    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            SSTab1.SelectedIndex = 0
        End If
    End Sub



    Private Sub CreateProcessParameter()

        m_dtParameters = New DataTable("Scheduler")


        m_dtParameters.Columns.AddRange(New DataColumn(4) {
                                                    New DataColumn("Id", System.Type.GetType("System.String")),
                                                   New DataColumn("ParameterName", System.Type.GetType("System.String")),
                                                   New DataColumn("DefaultValue", System.Type.GetType("System.String")),
                                                   New DataColumn("DataType", System.Type.GetType("System.String")),
                                                   New DataColumn("CurrentValue", System.Type.GetType("System.String"))})


    End Sub



    Private Sub cmdSchedule_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cmdSchedule.Click
        Dim temp_ofrequency As Object = Nothing

        Dim iCnt As Integer = 0
        If lvwSearchDetails.Items.Count > 0 Then
            For lRow As Integer = 0 To lvwSearchDetails.Items.Count - 1
                If lvwSearchDetails.Items.Item(lRow).Selected Then

                    m_iBatchRenewalJobId = gPMFunctions.ToSafeInteger(lvwSearchDetails.SelectedItems(0).Text)
                    m_jobCode = lvwSearchDetails.Items.Item(lRow).SubItems(2).Text
                    m_jobDescription = lvwSearchDetails.Items.Item(lRow).SubItems(3).Text
                    m_jobType = lvwSearchDetails.Items.Item(lRow).SubItems(5).Text
                    m_userName = lvwSearchDetails.Items.Item(lRow).SubItems(6).Text
                    m_sbatchStatus = lvwSearchDetails.Items.Item(lRow).SubItems(4).Text
                    iCnt += 1
                    Exit For
                    'Else
                    '   m_lReturn = MessageBox.Show("Select Job to Schedule", "Select Something", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    '  Exit Sub
                End If
            Next
        End If


        If iCnt > 0 Then
            If Task <> gPMConstants.PMEComponentAction.PMView Then
                CreateProcessParameter()
                m_dtParameters.LoadDataRow(New String(4) {String.Empty, "Batch_Renewal_Job_Id", m_iBatchRenewalJobId, "Integer", m_iBatchRenewalJobId.ToString()}, True)
                m_dtParameters.LoadDataRow(New String(4) {String.Empty, "JobCode", m_jobCode, "String", m_jobCode.ToString()}, True)
                m_dtParameters.LoadDataRow(New String(4) {String.Empty, "JobDescription", m_jobDescription, "String", m_jobDescription.ToString()}, True)
                m_dtParameters.LoadDataRow(New String(4) {String.Empty, "JobType", m_jobType, "String", m_jobType.ToString()}, True)
                m_dtParameters.LoadDataRow(New String(4) {String.Empty, "UserName", m_userName, "String", m_userName.ToString()}, True)
            End If
            m_sbatchContentDetails = Application.StartupPath() & "\" & "BatchRenewalWinController.exe -J:" & JobCode & " -S:false sirius Password"

            Dim iSIRFrequencyScheduler As iSIRFrequencyScheduler.Interface_Renamed = New iSIRFrequencyScheduler.Interface_Renamed
                m_lReturn = g_oObjectManager.GetInstance(temp_ofrequency, sClassName:="iSIRFrequencyScheduler.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                iSIRFrequencyScheduler = temp_ofrequency
                iSIRFrequencyScheduler.BatchProcessId = m_iBatchProcessId

                iSIRFrequencyScheduler.JobCode = m_jobCode
                iSIRFrequencyScheduler.JobStatus = m_sbatchStatus
                iSIRFrequencyScheduler.JobDescription = m_jobDescription
                iSIRFrequencyScheduler.JobType = m_jobType
                iSIRFrequencyScheduler.UserName = m_userName
                iSIRFrequencyScheduler.Process = "Renewal" & " " & m_jobType
            iSIRFrequencyScheduler.ProcessDescription = m_jobCode + "_" + m_jobDescription + "_" + Now.ToString("yyyyMMddhhmm")
            iSIRFrequencyScheduler.BatchFileName = "Renewal" & " " & m_jobType & "_" & Now.ToString("yyyyMMddhhmm")
            iSIRFrequencyScheduler.BatchFileContentDetails = m_sbatchContentDetails   'm_jobCode & ".log"
            iSIRFrequencyScheduler.BatchProcessName = m_sBatchProcessName
            iSIRFrequencyScheduler.ProcessParameters = m_dtParameters
            iSIRFrequencyScheduler.BatchSchedulerId = m_ibatchSchedulerId
            iSIRFrequencyScheduler.Task = m_iTask
            'iSIRFrequencyScheduler.Start()
            'Microsoft.VisualBasic.Compatibility.VB6.ShowForm(iPMUBatchRenewalJobs, 1)
            temp_ofrequency.Start()
                ' frmFrequency.ShowDialog()
                Me.Hide()

            End If


        'Dim frmFrequencyScheduler As iSIRFrequencyScheduler.Interface_Renamed = New iSIRFrequencyScheduler.Interface_Renamed

        'frmFrequencyScheduler.Start()
        'VB6.ShowForm(frmFrequencyScheduler, 1)
    End Sub

    Private Sub cmdSchedule_ClientSizeChanged(sender As Object, e As EventArgs) Handles cmdSchedule.ClientSizeChanged

    End Sub


    Private Sub lvwSearchDetails_ItemClick(ByVal Item As ListViewItem)
        If m_iTask = gPMConstants.PMEComponentAction.PMView Then

            If m_iBatchRenewalJobId <> gPMFunctions.ToSafeInteger(lvwSearchDetails.SelectedItems(0).Text) Then

                cmdSchedule.Enabled = False
            Else
                cmdSchedule.Enabled = True
            End If
        End If

    End Sub

    Private Sub lvwSearchDetails_ItemSelectionChanged(sender As Object, e As ListViewItemSelectionChangedEventArgs) Handles lvwSearchDetails.ItemSelectionChanged
        If lvwSearchDetails.FocusedItem IsNot Nothing Then
            If lvwSearchDetails.FocusedItem.Selected Then
                lvwSearchDetails_ItemClick(lvwSearchDetails.SelectedItems.Item(0))
            End If
        End If
    End Sub
End Class