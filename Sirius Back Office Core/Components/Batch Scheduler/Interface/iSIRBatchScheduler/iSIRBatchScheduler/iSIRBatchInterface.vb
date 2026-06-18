Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Imports Microsoft.Win32.TaskScheduler
Imports System.IO

Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 25/07/2021
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmInterface"

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iSIRBatchScheduler.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Stores the return value for the a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Batch Scheduler array
    Private m_vBatchScheduler(,) As Object
    Private m_vBatchProcessParameters(,) As Object

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As gPMConstants.PMEReturnCode
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As gPMConstants.PMENavigateButtonStatus
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private Const vbFormCode As Integer = 0
    Public frmSelectBatchProcess As frmSelectBatchProcess
    Dim m_sBatchSchedulerId As Integer
    Dim m_sProcessSelected As String
    Private m_sbatchDirPath As String

    ' ***************************************************************** '
    '                         PUBLIC PROPERTIES
    ' ***************************************************************** '
    Public ReadOnly Property ErrorNumber() As Integer
        Get
            ' Return any error number that might have occurred on the interface.
            Return m_lErrorNumber
        End Get
    End Property
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            ' Set the calling application name.
            m_sCallingAppName = Value
        End Set
    End Property
    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)
            m_lNavigate = Value
        End Set
    End Property
    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)
            m_lProcessMode = Value
        End Set
    End Property
    Public ReadOnly Property Status() As Integer
        Get
            ' Return the interface exit status.
            Return m_lStatus
        End Get
    End Property
    Public Property Task() As Integer
        Get
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property

    ' ***************************************************************** '
    '                          PUBLIC METHODS
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Name: GetBusiness
    '
    ' Description: Retrieves the details from the business object.
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetBusiness"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            lReturn = m_oBusiness.GetScheduledBatchProcesses(r_vScheduledProcesses:=m_vBatchScheduler)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.GetScheduledBatchProcesses", "Unable to get scheduler")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    Public Function GetProcessParameters() As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetProcessParameters"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            lReturn = m_oBusiness.GetProcessParameters(r_vProcessesParameter:=m_vBatchProcessParameters, v_lBatchSchedulerID:=m_sBatchSchedulerId)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.GetProcessParameters", "Unable to get parameters")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business object.
    ' ***************************************************************** '
    Public Function BusinessToInterface(Optional ByVal v_lIndex As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Dim oListSubItem As ListViewItem.ListViewSubItem

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "BusinessToInterface"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the list before we start
            lvwBatchScheduler.Items.Clear()

            ' Check for items (we may not have any yet)
            If Information.IsArray(m_vBatchScheduler) Then
                ' Process all treaties
                For lCount As Integer = m_vBatchScheduler.GetLowerBound(1) To m_vBatchScheduler.GetUpperBound(1)
                    oListItem = lvwBatchScheduler.Items.Add(CStr(m_vBatchScheduler(MainModule.BatchSchedulerEnum.DBMProcess, lCount)))
                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vBatchScheduler(MainModule.BatchSchedulerEnum.DBMDescription, lCount))
                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vBatchScheduler(MainModule.BatchSchedulerEnum.DBMFrequencyDescription, lCount))
                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vBatchScheduler(MainModule.BatchSchedulerEnum.DBMStatus, lCount))
                    ' Store array index so we can find the original record     
                    oListItem.Tag = CStr(lCount)

                    ' Check for selected item
                    If lCount = v_lIndex Then

                        'lvwReportScheduler_ItemClick(lvwReportScheduler, New EventArgs())
                        If lvwBatchScheduler.SelectedItems.Count > 0 Then
                            lvwBatchScheduler_ItemClick(lvwBatchScheduler.SelectedItems.Item(0))
                        End If
                    End If
                Next lCount
            End If

            ' Ignore errors this is only a cosmetic nicety
            'TODO:to be checked at runtime
            'lReturn = CType(ListViewAutoSize(lvwReportScheduler, True, True, Me), gPMConstants.PMEReturnCode)
            lReturn = CType(ListViewFunc.ListViewAutoSize(lvwBatchScheduler, True, True), gPMConstants.PMEReturnCode)

            ' Refresh sort order
            SortList(ListViewHelper.GetSortKeyProperty(lvwBatchScheduler), True)

            If lvwBatchScheduler.Items.Count > 0 Then
                If lvwBatchScheduler.SelectedItems.Count > 0 Then
                    cmdView.Enabled = True
                    cmdDelete.Enabled = True
                    cmdEdit.Enabled = True
                Else
                    cmdView.Enabled = False
                    cmdDelete.Enabled = False
                    cmdEdit.Enabled = False
                End If

            Else
                cmdView.Enabled = False
                cmdDelete.Enabled = False
                cmdEdit.Enabled = False
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '                         PRIVATE METHODS
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Const kMethodName As String = "SetInterfaceDefaults"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            cmdNew.Enabled = (Task <> gPMConstants.PMEComponentAction.PMView)
            cmdView.Enabled = False
            cmdDelete.Enabled = False
            cmdOK.Enabled = True


            ' Display all language specific captions.
            m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally

        End Try
        Return result
    End Function

    Private Function DisplayCaptions() As Integer
        Dim Catch_Renamed As Boolean = False

        Dim result As Integer = 0
        Const kMethodName As String = "DisplayCaptions"


        Try
            Catch_Renamed = True

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.

            'Form Caption

            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kInterfaceCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Button

            cmdView.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kViewButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdNew.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kNewButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'List View


            lvwBatchScheduler.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kListTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwBatchScheduler.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kListTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))




            lvwBatchScheduler.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kListTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBatchScheduler.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kListTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            Return result

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If

            Exit Function
            If Catch_Renamed Then

                ' DO Not Call any functions before here or the error will be lost
                iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=excep)
            End If

        End Try
    End Function

    Private Function SortList(ByVal lColumnIndex As Integer, Optional ByVal bReSort As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Const kMethodName As String = "SortList"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' We may just be refreshing after a item edit or addition
            If Not bReSort Then
                ' Reverse sort order if column hasn't changed
                If ListViewHelper.GetSortKeyProperty(lvwBatchScheduler) = lColumnIndex Then
                    ListViewHelper.SetSortOrderProperty(lvwBatchScheduler, IIf(ListViewHelper.GetSortOrderProperty(lvwBatchScheduler) = SortOrder.Ascending, SortOrder.Descending, SortOrder.Ascending))
                Else
                    ListViewHelper.SetSortOrderProperty(lvwBatchScheduler, SortOrder.Ascending)
                End If
            End If

            ' Sort based on contents
            Select Case lColumnIndex
                Case 2, 3 ' Date
                    'ListViewSortByValue(lvwReportScheduler, lColumnIndex, ListViewHelper.GetSortOrderProperty(lvwReportScheduler), True)
                    ListViewFunc.ListViewSortByValue(lvwBatchScheduler, lColumnIndex, ListViewHelper.GetSortOrderProperty(lvwBatchScheduler))
                Case Else
                    ListViewHelper.SetSortKeyProperty(lvwBatchScheduler, lColumnIndex)
                    ListViewHelper.SetSortedProperty(lvwBatchScheduler, True)
            End Select


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally




        End Try
        Return result
    End Function

    Private Function GetBatchFilePath() As Integer
        If m_sbatchDirPath = "" Then
            gPMFunctions.GetPMRegSetting(gPMConstants.HKEY_LOCAL_MACHINE, 0, gPMConstants.PMERegSettingLevel.pmeRSLBase, "PMDIR", m_sbatchDirPath)
            m_sbatchDirPath &= "Pure\PureBatchFile"

        End If

        Return gPMConstants.PMEReturnCode.PMTrue
    End Function

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click

        Dim oListItem As ListViewItem
        Dim lIndex As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim iMsgResult As DialogResult
        Const kMethodName As String = "cmdDelete_Click"

        Dim m_sProcessDescription As String
        Dim m_sBatchFileName As String = String.Empty
        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check for active item
            If lvwBatchScheduler.FocusedItem Is Nothing Then
                cmdDelete.Enabled = False
                cmdView.Enabled = False
            Else
                ' Get index of selected item
                oListItem = lvwBatchScheduler.FocusedItem
                lIndex = gPMFunctions.ToSafeLong(Convert.ToString(oListItem.Tag))

                ' Delete report scheduler detail

                iMsgResult = MessageBox.Show("If you delete, this task will be removed from the scheduler. Do you wish to proceed? ", "Scheduler", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                ' Check message result.
                If iMsgResult = System.Windows.Forms.DialogResult.Yes Then

                    If Information.IsArray(m_vBatchScheduler) Then
                        ' Process all treaties
                        For lCount As Integer = m_vBatchScheduler.GetLowerBound(1) To m_vBatchScheduler.GetUpperBound(1)
                            If lCount = lIndex Then
                                m_sBatchSchedulerId = CStr(m_vBatchScheduler(MainModule.BatchSchedulerEnum.DBMBatchSchedulerID, lCount))
                                m_sProcessDescription = CStr(m_vBatchScheduler(MainModule.BatchSchedulerEnum.DBMProcessDescription, lCount))
                                m_sBatchFileName = CStr(m_vBatchScheduler(MainModule.BatchSchedulerEnum.DBMBatchFileName, lCount))
                            End If
                        Next
                    End If

                    Using tService As New TaskService()
                        'Dim task As Task = tService.FindTask(m_sProcessDescription.Trim())
                        'If task IsNot Nothing Then
                        '    tService.RootFolder.DeleteTask(m_sProcessDescription.Trim())
                        'End If
                        Dim taskFolder As TaskFolder = tService.GetFolder("PureTaskFolder")
                        If m_sProcessDescription IsNot Nothing Then

                            Dim currentTask As String = "" 'tService.GetFolder("PureTaskFolder") 'FindTask(m_sCurrentProcessDescription)
                            For Each task As Task In taskFolder.Tasks
                                currentTask = task.Name
                                If currentTask = m_sProcessDescription Then
                                    taskFolder.DeleteTask(m_sProcessDescription.Trim())
                                End If

                            Next

                        End If
                    End Using

                    lReturn = m_oBusiness.DeleteBatchProcessSchedulerDetail(v_iBatchSchedulerId:=gPMFunctions.ToSafeInteger(m_sBatchSchedulerId))


                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("m_oBusiness.DeleteBatchProcessSchedulerDetail", "Unable to delete report scheduler detail")
                    End If
                    lReturn = GetBatchFilePath()
                    'm_sbatchDirPath
                    If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        If File.Exists(m_sbatchDirPath & "\" & m_sBatchFileName & ".bat") Then

                            File.Delete(m_sbatchDirPath & "\" & m_sBatchFileName & ".bat")
                        End If
                    End If
                    lReturn = CType(GetBusiness(), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("GetBusiness", "Unable to Get scheduler Details")
                        End If

                        lReturn = CType(BusinessToInterface(lIndex), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("BusinessToInterface", "Unable to refresh interface")
                        End If

                    End If

                End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally
            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)


        End Try
    End Sub

    Private Sub cmdNew_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNew.Click
        Dim lReturn As Integer
        Const kMethodName As String = "cmdNew_Click"
        Try
            m_iTask = gPMConstants.PMEComponentAction.PMAdd
            frmSelectBatchProcess = New frmSelectBatchProcess()

            frmSelectBatchProcess.ShowDialog()
            'VB6.ShowForm(frmSelectBatchProcess, 1)

            ' Refresh list
            lReturn = GetBusiness()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetBusiness", "Unable to Get report scheduler Details")
            End If

            lReturn = BusinessToInterface()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("BusinessToInterface()", "Unable to refresh report scheduler")
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally
            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)


            '    oiPMBReportPrint.Dispose()
            '    oiPMBReportPrint = Nothing


        End Try
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        Try



            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel


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

    Private Sub cmdView_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdView.Click
        ' Dim oForm As frmViewSchedulerDetails
        Dim result As Integer
        Dim oListItem As ListViewItem

        Dim lIndex As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "cmdView_Click"
        m_iTask = gPMConstants.PMEComponentAction.PMView
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            EditViewClick(kMethodName, m_iTask)
        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally
            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)


        End Try
    End Sub

    Private Sub Form_Initialize_Renamed()

        Dim lReturn As Integer
        Const kMethodName As String = "Form_Initialize"


        Try

            ' Show form in task bar
            iPMFunc.ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Get an instance of the business object via the public object manager.
            Dim temp_m_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRBatchScheduler.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "Unable to get instance of business object")
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iSIRBatchScheduler.General()

            ' Call the initialise method passing this interface and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oGeneral.Initialise", "Unable to initialise General object")
            End If


            m_lStatus = gPMConstants.PMEReturnCode.PMCancel


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

            ' Set error code
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

        Finally

            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
    End Sub


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "Form_Load"


        Try

            ' Show form in task bar
            iPMFunc.ShowFormInTaskBar_Detach()

            ' Check if we have had an error so far. Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error, so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the process modes for the busines object.

            lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.SetProcessModes", "Failed to set the process modes for the business object")
            End If

            ' Set the interface default values.
            lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("SetInterfaceDefaults", "Failed to set interface default values")
            End If

            ' Gets the interface details to be displayed.
            lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oGeneral.GetInterfaceDetails", "Failed to get interface details")
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

            ' Set error code
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

        Finally
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Dim lReturn As Integer
        Const kMethodName As String = "Form_QueryUnload"


        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Process the next set of actions depending upon the interface task etc.
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                lReturn = m_oGeneral.ProcessCommand()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.Cancel = True
                    Exit Sub
                End If
            End If

            ' Terminate the general object.
            m_oGeneral.Dispose()
            m_oGeneral = Nothing

            ' Terminate the business object

            m_oBusiness.Dispose()
            m_oBusiness = Nothing


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally
            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

            eventArgs.Cancel = Cancel <> 0
        End Try
    End Sub

    Private Sub lvwBatchScheduler_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwBatchScheduler.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwBatchScheduler.Columns(eventArgs.Column)
        SortList(ColumnHeader.Index + 1 - 1)
    End Sub

    Private Sub lvwBatchScheduler_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwBatchScheduler.DoubleClick
        cmdView_Click(cmdView, New EventArgs())
    End Sub

    Private Sub lvwBatchScheduler_ItemClick(ByVal Item As ListViewItem)

        Dim lIndex, lReturn As Integer
        Const kMethodName As String = "lvwBatchScheduler_ItemClick"


        Try

            ' Get index of selected item
            lIndex = gPMFunctions.ToSafeLong(Convert.ToString(Item.Tag))
        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally
            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)

        End Try
    End Sub

    Private Sub lvwBatchScheduler_MouseHover(sender As Object, e As EventArgs) Handles lvwBatchScheduler.MouseHover

    End Sub

    Private Function EditViewClick(ByVal m_methodName As String, ByVal m_itask As PMEComponentAction) As Integer
        Dim result As Integer
        Dim oListItem As ListViewItem

        Dim lIndex As Integer
        Dim lReturn As gPMConstants.PMEReturnCode

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            ' Check for active item
            If lvwBatchScheduler.FocusedItem Is Nothing Then
                cmdView.Enabled = False
                cmdDelete.Enabled = False
                Exit Function
            End If

            ' Get index of selected item
            oListItem = lvwBatchScheduler.FocusedItem
            lIndex = gPMFunctions.ToSafeLong(Convert.ToString(oListItem.Tag))

            If Information.IsArray(m_vBatchScheduler) Then
                ' Process all treaties
                For lCount As Integer = m_vBatchScheduler.GetLowerBound(1) To m_vBatchScheduler.GetUpperBound(1)
                    If lCount = lIndex Then
                        m_sBatchSchedulerId = CStr(m_vBatchScheduler(MainModule.BatchSchedulerEnum.DBMBatchSchedulerID, lCount))
                        m_sProcessSelected = CStr(m_vBatchScheduler(MainModule.BatchSchedulerEnum.DBMProcessSelected, lCount))

                    End If
                Next
            End If

            result = GetProcessParameters()


            Dim temp_obatchprocess As Object = Nothing

            Select Case (m_sProcessSelected)
                Case "Batch Renewal"
                    Dim m_iBatchRenewalJobId As Integer
                    If result = gPMConstants.PMEReturnCode.PMTrue Then
                        If Information.IsArray(m_vBatchProcessParameters) Then
                            m_iBatchRenewalJobId = CStr(m_vBatchProcessParameters(1, 0))
                        End If
                    End If

                    Dim iPMUBatchRenewalJobs As iPMUBatchRenewalJobs.Interface_Renamed = New iPMUBatchRenewalJobs.Interface_Renamed
                    m_lReturn = g_oObjectManager.GetInstance(temp_obatchprocess, sClassName:="iPMUBatchRenewalJobs.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                    iPMUBatchRenewalJobs = temp_obatchprocess
                    iPMUBatchRenewalJobs.AttachToScheduler = gPMConstants.PMEReturnCode.PMTrue
                    iPMUBatchRenewalJobs.BatchSchedulerId = m_sBatchSchedulerId
                    iPMUBatchRenewalJobs.BatchRenewalJobId = m_iBatchRenewalJobId
                    iPMUBatchRenewalJobs.Task = m_itask
                    'Microsoft.VisualBasic.Compatibility.VB6.ShowForm(iPMUBatchRenewalJobs, 1)
                    temp_obatchprocess.Start()
                Case "Import", "Export"
                    Dim iACTImportExport As iACTImportExport.Interface_Renamed = New iACTImportExport.Interface_Renamed
                    m_lReturn = g_oObjectManager.GetInstance(temp_obatchprocess, sClassName:="iACTImportExport.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                    iACTImportExport = temp_obatchprocess
                    iACTImportExport.AttachToScheduler = gPMConstants.PMEReturnCode.PMTrue
                    iACTImportExport.BatchSchedulerId = m_sBatchSchedulerId
                    iACTImportExport.BatchParameters = m_vBatchProcessParameters
                    iACTImportExport.BatchProcessName = m_sProcessSelected
                    iACTImportExport.Task = m_itask
                    temp_obatchprocess.Start()
                Case "Credit Control"
                    Dim iACTCreditControlProcessing As iACTCreditControlProcessing.Interface_Renamed = New iACTCreditControlProcessing.Interface_Renamed
                    m_lReturn = g_oObjectManager.GetInstance(temp_obatchprocess, sClassName:="iACTCreditControlProcessing.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                    iACTCreditControlProcessing = temp_obatchprocess
                    iACTCreditControlProcessing.AttachToScheduler = gPMConstants.PMEReturnCode.PMTrue
                    iACTCreditControlProcessing.BatchSchedulerId = m_sBatchSchedulerId
                    iACTCreditControlProcessing.BatchParameters = m_vBatchProcessParameters
                    iACTCreditControlProcessing.Task = m_itask
                    temp_obatchprocess.Start()
                Case "Chase Cycle"
                    Dim iPMUChaseCycleProcessing As iPMUChaseCycleProcessing.Interface_Renamed = New iPMUChaseCycleProcessing.Interface_Renamed
                    m_lReturn = g_oObjectManager.GetInstance(temp_obatchprocess, sClassName:="iPMUChaseCycleProcessing.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                    iPMUChaseCycleProcessing = temp_obatchprocess
                    iPMUChaseCycleProcessing.AttachToScheduler = gPMConstants.PMEReturnCode.PMTrue
                    iPMUChaseCycleProcessing.BatchSchedulerId = m_sBatchSchedulerId
                    iPMUChaseCycleProcessing.BatchParameters = m_vBatchProcessParameters
                    iPMUChaseCycleProcessing.Task = m_itask
                    temp_obatchprocess.Start()
                Case "Period End"
                    Dim iACTPeriodEnd As iACTPeriodEnd.Interface_Renamed = New iACTPeriodEnd.Interface_Renamed
                    m_lReturn = g_oObjectManager.GetInstance(temp_obatchprocess, sClassName:="iACTPeriodEnd.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                    iACTPeriodEnd = temp_obatchprocess
                    iACTPeriodEnd.AttachToScheduler = gPMConstants.PMEReturnCode.PMTrue
                    iACTPeriodEnd.BatchSchedulerId = m_sBatchSchedulerId
                    iACTPeriodEnd.BatchParameters = m_vBatchProcessParameters
                    iACTPeriodEnd.Task = m_itask
                    temp_obatchprocess.Start()
            End Select

            ' Refresh list
            lReturn = GetBusiness()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetBusiness", "Unable to Get report scheduler Details")
            End If

            lReturn = BusinessToInterface()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("BusinessToInterface()", "Unable to refresh report scheduler")
            End If





        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=m_methodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally
            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)


        End Try

    End Function
    Private Sub cmdEdit_Click(sender As Object, e As EventArgs) Handles cmdEdit.Click
        Const kMethodName As String = "cmdEdit_Click"
        m_iTask = gPMConstants.PMEComponentAction.PMEdit
        EditViewClick(kMethodName, m_iTask)

    End Sub

    Private Sub lvwBatchScheduler_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lvwBatchScheduler.SelectedIndexChanged

        If lvwBatchScheduler.SelectedItems.Count > 0 Then

            cmdView.Enabled = True
            cmdDelete.Enabled = True
            cmdEdit.Enabled = True
        Else
            cmdView.Enabled = False
            cmdDelete.Enabled = False
            cmdEdit.Enabled = False

        End If

    End Sub
End Class
