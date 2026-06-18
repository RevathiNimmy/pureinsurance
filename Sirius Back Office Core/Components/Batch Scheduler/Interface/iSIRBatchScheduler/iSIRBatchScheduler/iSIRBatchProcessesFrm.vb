Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Imports PMLookupControl.cboPMLookup

Partial Friend Class frmSelectBatchProcess
    Inherits System.Windows.Forms.Form

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmSelectBatchProcess"

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iSIRBatchScheduler.General

    Private m_iBatchProcessId As Integer
    Private m_sBatchProcessName As String
    Private m_lReturn As Integer

    Private m_vBatchScheduler(,) As Object
    Private m_bIsChanged As Boolean
    Private m_sFrequency As String = ""

    Private m_vParameters As Object

    Private m_iReportId As Integer
    Private Const vbFormCode As Integer = 0
    Private m_bIsWrongFrequency As Boolean = False
    Public Status As gPMConstants.PMEReturnCode
    Private m_vBatchProcesses As Object(,)

    Dim oSirBatchListBusiness As bSIRBatchScheduler.Business
    ' Declare an instance of the Business object.
    Public Business As Object
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


    ' Declare an instance of the FormControl object
    ' Private m_oFormFields As iPMFormControl.FormFields

    ' ***************************************************************** '
    '                         PUBLIC METHODS
    ' ***************************************************************** '
    Public Function Clear() As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Const kMethodName As String = "Clear"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue



        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function



    Private Function Validate_Renamed() As Integer

        Dim result As Integer = 0
        Dim bOverlapLimit, bDuplicateRILine As Boolean
        Dim lCount1, lCount As Integer
        Dim bIsRetainedReinsurer As Boolean
        Dim iCountRetained As Integer

        Dim lReturn As Integer
        Const kMethodName As String = "Validate"


        Try

            ' Default to false, only set true if we get to the end
            result = gPMConstants.PMEReturnCode.PMFalse

            'Mean while nothing to validate

            ' All validation passed return True
            result = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function


    Private Sub frmSelectBatchProcess_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vValue As Object
        Const kMethodName As String = "frmSelectBatchProcess_Load"



        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Set the interface default values.
            lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("SetInterfaceDefaults", "Failed to set interface default values")
            End If



        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
    End Sub

    Private Sub frmSelectBatchProcess_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Dim lReturn As Integer
        Const kMethodName As String = "Form_QueryUnload"


        Try

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.

            If UnloadMode <> vbFormCode Then
                ' Check the user wants to close
                'If MessageBox.Show("Cancelling will lose all of your current details." &
                '               Strings.Chr(13) & Strings.Chr(10) & "Do you really wish to cancel?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = System.Windows.Forms.DialogResult.No Then
                '    ' Do not procced with the interface termination.
                '    eventArgs.Cancel = 1
                'Else
                Status = gPMConstants.PMEReturnCode.PMCancel
                'End If
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally

            eventArgs.Cancel = Cancel <> 0
        End Try
    End Sub

    '8.5
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Const kMethodName As String = "SetInterfaceDefaults"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)


            ' Display all language specific captions.
            m_lReturn = DisplayCaptions()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Dim r_vBatchArray As Object(,)
            Dim temp_oSIBatchProcesses As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oSIBatchProcesses, "bSIRBatchScheduler.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oSirBatchListBusiness = temp_oSIBatchProcesses

            cboBatchProcessList.Items.Clear()
            'This method return report code corresponding to reportID
            lReturn = oSirBatchListBusiness.GetBatchProcesses(r_vBatchProcesses:=m_vBatchProcesses)
            'check report code not nothing
            cboBatchProcessList.Items.Add(New Microsoft.VisualBasic.Compatibility.VB6.ListBoxItem("Please Select Process", 0))
            If Information.IsArray(m_vBatchProcesses) Then
                For lCount As Integer = m_vBatchProcesses.GetLowerBound(1) To m_vBatchProcesses.GetUpperBound(1)

                    cboBatchProcessList.Items.Add(New Microsoft.VisualBasic.Compatibility.VB6.ListBoxItem(Trim(m_vBatchProcesses(1, lCount)), CInt(m_vBatchProcesses(0, lCount))))


                Next
            End If
            cboBatchProcessList.SelectedIndex = 0
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

            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kInterfaceCaptionDetail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Button

            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



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

    ' ***************************************************************** '
    ' Name: BusinessToInterface
    '
    ' Description: Updates all interface details from the business object.
    ' ***************************************************************** '
    Private Function BusinessToInterface() As Integer

        Dim result As Integer = 0
        Dim oListItem As ListViewItem
        Dim oListSubItem As ListViewItem.ListViewSubItem
        Dim sFrequency As String = ""
        Dim sSeprateBy As String = ""

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "BusinessToInterface"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Information.IsArray(m_vBatchScheduler) Then
                For lCount As Integer = m_vBatchScheduler.GetLowerBound(1) To m_vBatchScheduler.GetUpperBound(1)
                    'txtReportName.Text = CStr(m_vReportScheduler(MainModule.ReportSchedulerEnum.DBMDescription, lCount))
                    sFrequency = CStr(m_vBatchScheduler(MainModule.BatchSchedulerEnum.DBMFrequencyDescription, lCount))

                Next
            End If



        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally




        End Try
        Return result
    End Function



    Private Function GetSetValues() As Integer

        Dim nResult As Integer = 0
        Dim nEndDateIndex As Integer = 0
        Dim nStartDateIndex As Integer = 0
        Dim bReportContainStartDate As Boolean = False
        Dim bReportContainEndDate As Boolean = False
        Const kMethodName As String = "GetSetValues"

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue
            'Frequency = gPMFunctions.ToSafeString(cboBatchProcessList.ItemCaption(cboBatchProcessList.ItemId)).Trim()


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
        Finally

        End Try
        Return nResult
    End Function

    ' ***************************************************************** '
    '                             EVENTS
    ' ***************************************************************** '

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        ' Dim dShare As Double
        'Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "cmdOK_Click"
        Dim dSelectedProcessId As Integer

        Try

            ' Validate data
            If Validate_Renamed() = gPMConstants.PMEReturnCode.PMTrue Then
                ' Set status to OK and close
                Dim r_vBatchArray As Object(,)


                If cboBatchProcessList.SelectedIndex = -1 Then
                    MessageBox.Show("Invalid Process Selection ", "Process Selection", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    cboBatchProcessList.Select()
                Else

                    dSelectedProcessId = gPMFunctions.ToSafeInteger(Microsoft.VisualBasic.Compatibility.VB6.GetItemData(cboBatchProcessList, cboBatchProcessList.SelectedIndex))
                    m_iBatchProcessId = dSelectedProcessId
                    If dSelectedProcessId = 0 Then
                        MessageBox.Show("Invalid Process Selection ", "Process Selection", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        cboBatchProcessList.Select()
                    Else

                        DisplayForm(dSelectedProcessId)

                    End If

                End If


                'Edit
                'lReturn = CType(GetSetValues(), gPMConstants.PMEReturnCode)

                'If m_bIsWrongFrequency = False Then
                'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' gPMFunctions.RaiseError("cmd_ok", "Failed to Get Set Values")
                'End If

                'lReturn = CType(UpdateReportScheduler(), gPMConstants.PMEReturnCode)
                'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '    gPMFunctions.RaiseError("cmd_ok", "Failed to Update Report Scheduler Details")
                'End If

                ' Status = gPMConstants.PMEReturnCode.PMOK
                Me.Hide()
                '  End If
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally


        End Try
    End Sub
    Private Function DisplayForm(ByVal dSelectedProcessId As Integer) As Integer
        Dim temp_oSIBatchProcesses As Object
        Dim lReturn As gPMConstants.PMEReturnCode

        Dim component As String
        Dim description As String = ""
        m_lReturn = g_oObjectManager.GetInstance(temp_oSIBatchProcesses, "bSIRBatchScheduler.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        oSirBatchListBusiness = temp_oSIBatchProcesses

        'This method return report code corresponding to reportID
        lReturn = oSirBatchListBusiness.GetBatchProcesses(r_vBatchProcesses:=m_vBatchProcesses, batchProcessId:=dSelectedProcessId)
        If Information.IsArray(m_vBatchProcesses) Then
            For lCount As Integer = m_vBatchProcesses.GetLowerBound(1) To m_vBatchProcesses.GetUpperBound(1)
                'txtReportName.Text = CStr(m_vReportScheduler(MainModule.ReportSchedulerEnum.DBMDescription, lCount))
                component = (m_vBatchProcesses(3, lCount) & "." & m_vBatchProcesses(2, lCount))
                description = (m_vBatchProcesses(1, lCount))

            Next
            Dim temp_obatchprocess As Object = Nothing

            Select Case (description)
                Case "Batch Renewal"
                    m_sBatchProcessName = "Batch Renewal"
                    Dim iPMUBatchRenewalJobs As iPMUBatchRenewalJobs.Interface_Renamed = New iPMUBatchRenewalJobs.Interface_Renamed
                    m_lReturn = g_oObjectManager.GetInstance(temp_obatchprocess, sClassName:="iPMUBatchRenewalJobs.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                    iPMUBatchRenewalJobs = temp_obatchprocess
                    iPMUBatchRenewalJobs.AttachToScheduler = gPMConstants.PMEReturnCode.PMTrue
                    iPMUBatchRenewalJobs.BatchProcessId = m_iBatchProcessId
                    iPMUBatchRenewalJobs.BatchProcessName = m_sBatchProcessName
                    iPMUBatchRenewalJobs.Task = gPMConstants.PMEComponentAction.PMAdd
                    'Microsoft.VisualBasic.Compatibility.VB6.ShowForm(iPMUBatchRenewalJobs, 1)
                    temp_obatchprocess.Start()
                Case "Import", "Export"
                    Dim iACTImportExport As iACTImportExport.Interface_Renamed = New iACTImportExport.Interface_Renamed
                    m_lReturn = g_oObjectManager.GetInstance(temp_obatchprocess, sClassName:="iACTImportExport.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                    iACTImportExport = temp_obatchprocess
                    iACTImportExport.AttachToScheduler = gPMConstants.PMEReturnCode.PMTrue
                    iACTImportExport.BatchProcessId = m_iBatchProcessId
                    iACTImportExport.BatchProcessName = description
                    iACTImportExport.Task = gPMConstants.PMEComponentAction.PMAdd
                    temp_obatchprocess.Start()
                Case "Credit Control"
                    Dim iACTCreditControlProcessing As iACTCreditControlProcessing.Interface_Renamed = New iACTCreditControlProcessing.Interface_Renamed
                    m_lReturn = g_oObjectManager.GetInstance(temp_obatchprocess, sClassName:="iACTCreditControlProcessing.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                    iACTCreditControlProcessing = temp_obatchprocess
                    iACTCreditControlProcessing.AttachToScheduler = gPMConstants.PMEReturnCode.PMTrue
                    iACTCreditControlProcessing.BatchProcessId = m_iBatchProcessId
                    iACTCreditControlProcessing.Task = gPMConstants.PMEComponentAction.PMAdd
                    temp_obatchprocess.Start()
                Case "Chase Cycle"
                    Dim iPMUChaseCycleProcessing As iPMUChaseCycleProcessing.Interface_Renamed = New iPMUChaseCycleProcessing.Interface_Renamed
                    m_lReturn = g_oObjectManager.GetInstance(temp_obatchprocess, sClassName:="iPMUChaseCycleProcessing.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                    iPMUChaseCycleProcessing = temp_obatchprocess
                    iPMUChaseCycleProcessing.AttachToScheduler = gPMConstants.PMEReturnCode.PMTrue
                    iPMUChaseCycleProcessing.BatchProcessId = m_iBatchProcessId
                    iPMUChaseCycleProcessing.Task = gPMConstants.PMEComponentAction.PMAdd
                    temp_obatchprocess.Start()

                Case "Period End"
                    Dim iACTPeriodEnd As iACTPeriodEnd.Interface_Renamed = New iACTPeriodEnd.Interface_Renamed
                    m_lReturn = g_oObjectManager.GetInstance(temp_obatchprocess, sClassName:="iACTPeriodEnd.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
                    iACTPeriodEnd = temp_obatchprocess
                    iACTPeriodEnd.AttachToScheduler = gPMConstants.PMEReturnCode.PMTrue
                    iACTPeriodEnd.BatchProcessId = m_iBatchProcessId
                    iACTPeriodEnd.BatchProcessName = description
                    iACTPeriodEnd.Task = gPMConstants.PMEComponentAction.PMAdd
                    temp_obatchprocess.Start()

            End Select

        End If
    End Function
    Private Sub frmSelectBatchProcess_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        If eventArgs.KeyCode = Keys.Tab Then
            'If Shift And ShiftConstants.CtrlMask Then
            '    If Shift And ShiftConstants.ShiftMask Then

            '    Else

            '    End If
            'End If
        End If
    End Sub

    Private Sub cboBatchProcessList_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles cboBatchProcessList.KeyPress

        Dim cbx As ComboBox = sender
        e.Handled = cbx.FindString(cbx.Text & e.KeyChar, 1) = -1 And Asc(e.KeyChar) <> 8

    End Sub


    Private Sub cmdOK_ContextMenuStripChanged(sender As Object, e As EventArgs) Handles cmdOK.ContextMenuStripChanged

    End Sub
End Class

'This class is used for generatig separate reports for all Agent and SubAgent.
