Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 19/11/2009
    '
    ' Description: Main interface.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmInterface"

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iSIRReportScheduler.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Stores the return value for the a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Report Scheduler array
    Private m_vReportScheduler(,) As Object

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As gPMConstants.PMEReturnCode
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As gPMConstants.PMENavigateButtonStatus
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private Const vbFormCode As Integer = 0
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

            lReturn = m_oBusiness.GetScheduledReports(r_vScheduledReports:=m_vReportScheduler)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.GetScheduledReports", "Unable to get report scheduler")
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
            lvwReportScheduler.Items.Clear()

            ' Check for items (we may not have any yet)
            If Information.IsArray(m_vReportScheduler) Then
                ' Process all treaties
                For lCount As Integer = m_vReportScheduler.GetLowerBound(1) To m_vReportScheduler.GetUpperBound(1)
                    oListItem = lvwReportScheduler.Items.Add("M" & CStr(m_vReportScheduler(MainModule.ReportSchedulerEnum.DBMReportSchedulerID, lCount)), CStr(m_vReportScheduler(MainModule.ReportSchedulerEnum.DBMReportSchedulerID, lCount)).Trim(), "")
                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vReportScheduler(MainModule.ReportSchedulerEnum.DBMDescription, lCount))
                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vReportScheduler(MainModule.ReportSchedulerEnum.DBMFrequency, lCount))
                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vReportScheduler(MainModule.ReportSchedulerEnum.DBMReportID, lCount))
                    ' Store array index so we can find the original record
                    oListItem.Tag = CStr(lCount)

                    ' Check for selected item
                    If lCount = v_lIndex Then
                        ' If we are refreshing reselect the original item
                        lvwReportScheduler.FocusedItem = oListItem

                        lvwReportScheduler.FocusedItem.EnsureVisible()
                        If lvwReportScheduler.Visible Then
                            lvwReportScheduler.Focus()
                        End If

                        ' Click the item to refresh buttons
                        'developer guide no.184
                        'lvwReportScheduler_ItemClick(lvwReportScheduler, New EventArgs())
                        If lvwReportScheduler.SelectedItems.Count > 0 Then
                            lvwReportScheduler_ItemClick(lvwReportScheduler.SelectedItems.Item(0))
                        End If
                    End If
                Next lCount
            End If

            ' Ignore errors this is only a cosmetic nicety
            'TODO:to be checked at runtime
            'lReturn = CType(ListViewAutoSize(lvwReportScheduler, True, True, Me), gPMConstants.PMEReturnCode)
            lReturn = CType(ListViewFunc.ListViewAutoSize(lvwReportScheduler, True, True), gPMConstants.PMEReturnCode)

            ' Refresh sort order
            SortList(ListViewHelper.GetSortKeyProperty(lvwReportScheduler), True)

            If lvwReportScheduler.Items.Count > 0 Then
                cmdView.Enabled = True
                cmdDelete.Enabled = True
            Else
                cmdView.Enabled = False
                cmdDelete.Enabled = False
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

            With lvwReportScheduler
                .Columns.Clear()
                .Columns.Insert(0, "", "Scheduler Id", CInt(VB6.TwipsToPixelsX(0)), HorizontalAlignment.Left, -1)
                .Columns.Insert(1, "", "Report Name", CInt(VB6.TwipsToPixelsX(10000)), HorizontalAlignment.Left, -1)
                .Columns.Insert(2, "", "Frequency", CInt(VB6.TwipsToPixelsX(5000)), HorizontalAlignment.Left, -1)
                .Columns.Insert(3, "", "ReportID", CInt(VB6.TwipsToPixelsX(0)), HorizontalAlignment.Left, -1)
            End With

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

            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceCaption, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Button

            cmdView.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACViewButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdNew.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACNewButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdHelp.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACHelpButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'List View


            lvwReportScheduler.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwReportScheduler.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))




            lvwReportScheduler.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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
                If ListViewHelper.GetSortKeyProperty(lvwReportScheduler) = lColumnIndex Then
                    ListViewHelper.SetSortOrderProperty(lvwReportScheduler, IIf(ListViewHelper.GetSortOrderProperty(lvwReportScheduler) = SortOrder.Ascending, SortOrder.Descending, SortOrder.Ascending))
                Else
                    ListViewHelper.SetSortOrderProperty(lvwReportScheduler, SortOrder.Ascending)
                End If
            End If

            ' Sort based on contents
            Select Case lColumnIndex
                Case 2, 3 ' Date
                    'ListViewSortByValue(lvwReportScheduler, lColumnIndex, ListViewHelper.GetSortOrderProperty(lvwReportScheduler), True)
                    ListViewFunc.ListViewSortByValue(lvwReportScheduler, lColumnIndex, ListViewHelper.GetSortOrderProperty(lvwReportScheduler))
                Case Else
                    ListViewHelper.SetSortKeyProperty(lvwReportScheduler, lColumnIndex)
                    ListViewHelper.SetSortedProperty(lvwReportScheduler, True)
            End Select


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally




        End Try
        Return result
    End Function


    ' ***************************************************************** '
    '                             EVENTS
    ' ***************************************************************** '

    'UPGRADE_NOTE: (7001) The following declaration (chkHideDeleted_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub chkHideDeleted_Click()
    ' Refresh the list
    'If lvwReportScheduler.FocusedItem Is Nothing Then
    'm_lReturn = BusinessToInterface()
    'Else
    'm_lReturn = CType(BusinessToInterface(gPMFunctions.ToSafeLong(Convert.ToString(lvwReportScheduler.FocusedItem.Tag))), gPMConstants.PMEReturnCode)
    'End If
    'End Sub

    'UPGRADE_NOTE: (7001) The following declaration (chkHideExpired_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub chkHideExpired_Click()
    ' Refresh the list
    'If lvwReportScheduler.FocusedItem Is Nothing Then
    'm_lReturn = BusinessToInterface()
    'Else
    'm_lReturn = CType(BusinessToInterface(gPMFunctions.ToSafeLong(Convert.ToString(lvwReportScheduler.FocusedItem.Tag))), gPMConstants.PMEReturnCode)
    'End If
    'End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click

        Dim oListItem As ListViewItem
        Dim lIndex As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim iMsgResult As DialogResult
        Const kMethodName As String = "cmdDelete_Click"


        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check for active item
            If lvwReportScheduler.FocusedItem Is Nothing Then
                cmdDelete.Enabled = False
                cmdView.Enabled = False
            Else
                ' Get index of selected item
                oListItem = lvwReportScheduler.FocusedItem
                lIndex = gPMFunctions.ToSafeLong(Convert.ToString(oListItem.Tag))

                ' Delete report scheduler detail

                iMsgResult = MessageBox.Show("Delete Report Scheduler detail" & Strings.Chr(13) & Strings.Chr(10) & " Do you wish to continue? ", "Report Scheduler", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
                ' Check message result.
                If iMsgResult = System.Windows.Forms.DialogResult.Yes Then


                    lReturn = m_oBusiness.DeleteReportSchedulerDetail(v_iReportSchedulerId:=gPMFunctions.ToSafeInteger(lvwReportScheduler.FocusedItem.Text))
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("m_oBusiness.DeleteReportSchedulerDetail", "Unable to delete report scheduler detail")
                    End If

                    lReturn = CType(GetBusiness(), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("GetBusiness", "Unable to Get report scheduler Details")
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
        Dim iPMBReportPrint As Object

        'Dim oForm As frmReportSchedulerDetail

        Dim oiPMBReportPrint As iPMBReportPrint.Interface_Renamed
        Dim lReturn As Integer
        Const kMethodName As String = "cmdNew_Click"


        Try

            ' Create iPMBReport object
            Dim temp_oiPMBReportPrint As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_oiPMBReportPrint, sClassName:="iPMBReportPrint.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oiPMBReportPrint = temp_oiPMBReportPrint

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "oiPMBReportPrint.Interface Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            oiPMBReportPrint.AttachToScheduler = True


            m_lReturn = oiPMBReportPrint.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "oiPMBReportPrint.Start Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

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


            oiPMBReportPrint.Dispose()
            oiPMBReportPrint = Nothing


        End Try
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click
        Dim lReturn As Integer
        Const kMethodName As String = "cmdClose_Click"


        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending upon the interface task etc.
            m_lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)
        Finally


        End Try
    End Sub

    Private Sub cmdView_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdView.Click
        Dim oForm As frmReportSchedulerDetail

        Dim oListItem As ListViewItem

        Dim lIndex As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "cmdEdit_Click"


        Try

            ' Check for active item
            If lvwReportScheduler.FocusedItem Is Nothing Then
                cmdView.Enabled = False
                cmdDelete.Enabled = False
                Exit Sub
            End If

            ' Get index of selected item
            oListItem = lvwReportScheduler.FocusedItem
            lIndex = gPMFunctions.ToSafeLong(Convert.ToString(oListItem.Tag))

            oForm = New frmReportSchedulerDetail()
            oForm.Business = m_oBusiness
            oForm.ReportSchedulerId = CInt(lvwReportScheduler.FocusedItem.Text)
            oForm.ReportId = CInt(oListItem.SubItems(3).Text)
            oForm.ShowDialog()

            If oForm.Status = gPMConstants.PMEReturnCode.PMOK Then
                ' Refresh list
                lReturn = CType(GetBusiness(), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("GetBusiness", "Unable to Get report scheduler Details")
                End If

                lReturn = CType(BusinessToInterface(lIndex), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("BusinessToInterface(lIndex)", "Unable to refresh report scheduler")
                End If
            End If

            If oForm.Status = gPMConstants.PMEReturnCode.PMCancel Then
                ' Do Nothing
            End If


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
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRReportScheduler.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "Unable to get instance of business object")
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iSIRReportScheduler.General()

            ' Call the initialise method passing this interface and the business object as parameters.
            m_lReturn = CType(m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oGeneral.Initialise", "Unable to initialise General object")
            End If

            ' Set the interface status to cancelled. This is done so that any
            ' interface termination will be noted as cancelled except in the
            ' event of accepting the interface.
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
                    eventArgs.cancel = True
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

    Private Sub lvwReportScheduler_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwReportScheduler.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwReportScheduler.Columns(eventArgs.Column)
        SortList(ColumnHeader.Index + 1 - 1)
    End Sub

    Private Sub lvwReportScheduler_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwReportScheduler.DoubleClick
        cmdView_Click(cmdView, New EventArgs())
    End Sub

    Private Sub lvwReportScheduler_ItemClick(ByVal Item As ListViewItem)

        Dim lIndex, lReturn As Integer
        Const kMethodName As String = "lvwReportScheduler_ItemClick"


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
End Class
