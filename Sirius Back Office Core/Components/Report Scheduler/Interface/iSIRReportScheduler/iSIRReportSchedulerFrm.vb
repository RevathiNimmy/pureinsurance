Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Imports PMLookupControl.cboPMLookup

Partial Friend Class frmReportSchedulerDetail
    Inherits System.Windows.Forms.Form

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmReportSchedulerDetail"

    ' Declare an instance of the general interface object.
    Private m_oGeneral As iSIRReportScheduler.General

    Private m_iReportSchedulerId As Integer
    Private m_lReturn As Integer
    Private m_vReportSchedulerDetail As Object
    Private m_vReportScheduler(,) As Object
    Private m_bIsChanged As Boolean
    Private m_sFrequency As String = ""
    Private m_iExportToPDF As Integer
    Private m_iArchieveToPDF As Integer
    Private m_iExportToCSV As Integer
    Private m_vParameters As Object
    Private m_sSeprateBy As String = ""
    Private m_iReportId As Integer
    Private Const vbFormCode As Integer = 0
    Private m_bIsWrongFrequency As Boolean = False
    Public Status As gPMConstants.PMEReturnCode
   


    ' Declare an instance of the Business object.
    Public Business As Object

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' ***************************************************************** '
    '                        PUBLIC PROPERTIES
    ' ***************************************************************** '

    Public Property ReportSchedulerId() As Integer
        Get
            Return m_iReportSchedulerId
        End Get
        Set(ByVal Value As Integer)
            m_iReportSchedulerId = Value
        End Set
    End Property

    Public Property ReportId() As Integer
        Get
            Return m_iReportId
        End Get
        Set(ByVal Value As Integer)
            m_iReportId = Value
        End Set
    End Property
    ' ***************************************************************** '
    '                        PRIVATE PROPERTIES
    ' ***************************************************************** '

    Private Property Frequency() As String
        Get
            Return m_sFrequency
        End Get
        Set(ByVal Value As String)
            m_sFrequency = Value.Trim()
        End Set
    End Property

    Private Property ExportToPDF() As Integer
        Get
            Return m_iExportToPDF
        End Get
        Set(ByVal Value As Integer)
            m_iExportToPDF = Value
        End Set
    End Property

    Private Property ArchieveToPDF() As Integer
        Get
            Return m_iArchieveToPDF
        End Get
        Set(ByVal Value As Integer)
            m_iArchieveToPDF = Value
        End Set
    End Property

    Private Property ExportToCSV() As Integer
        Get
            Return m_iExportToCSV
        End Get
        Set(ByVal Value As Integer)
            m_iExportToCSV = Value
        End Set
    End Property

    Private Property Parameters() As Object
        Get
            Return m_vParameters
        End Get
        Set(ByVal Value As Object)


            m_vParameters = Value
        End Set
    End Property

    Private Property SeprateBy() As String
        Get
            Return m_sSeprateBy
        End Get
        Set(ByVal Value As String)
            m_sSeprateBy = Value.Trim()
        End Set
    End Property
    ' ***************************************************************** '
    '                         PUBLIC METHODS
    ' ***************************************************************** '
    Public Function Clear() As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Const kMethodName As String = "Clear"


        Try

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear controls

        m_vReportSchedulerDetail = ""

        ' Clear list view and adjust buttons
        lvwReportSchedulerDetail.Items.Clear()

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

    ' ***************************************************************** '
    '                         PRIVATE METHODS
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetReportSchedulerDetail) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetReportSchedulerDetail() As Integer
    '
    'Dim result As Integer = 0
    'Dim lReturn As gPMConstants.PMEReturnCode
    'Const kMethodName As String = "GetReportSchedulerDetail"
    '
    '
    'On Error GoTo Catch_Renamed
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Get report scheduler details from the business object.

    'lReturn = Business.GetReportSchedulerDetail(v_iReportSchedulerId:=m_iReportSchedulerId, r_vReportSchedulerDetail:=m_vReportSchedulerDetail)
    'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'gPMFunctions.RaiseError("m_oBusiness.GetRIModelLines", "Unable to retrieve ri model lines")
    'End If
    '
    'GoTo Finally_Renamed
    'Catch_Renamed: '
    '
    ' DO Not Call any functions before here or the error will be lost
    'iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
    '
    'Finally_Renamed: '
    '
    'Return result
    'Resume 
    'Return result
    'End Function

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


    Private Sub frmReportSchedulerDetail_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vValue As Object
        Const kMethodName As String = "Form_Load"



        Try

        ' Set the mouse pointer to busy.
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        ' Set the interface default values.
        lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("SetInterfaceDefaults", "Failed to set interface default values")
        End If

        lReturn = CType(GetBusiness(), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("GetBusiness", "Failed to get interface details")
        End If

        lReturn = CType(BusinessToInterface(), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("BusinessToInterface", "Failed to get interface details")
        End If


        Catch ex As Exception
        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally
        ' Set the mouse pointer to normal.
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

     
        End Try
    End Sub

    Private Sub frmReportSchedulerDetail_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Dim lReturn As Integer
        Const kMethodName As String = "Form_QueryUnload"


        Try

        ' Check if the interface has been terminated by means
        ' other than pressing the command buttons.

        If UnloadMode <> vbFormCode Then
            ' Check the user wants to close
            If MessageBox.Show("Cancelling will lose all of your current details." & _
                               Strings.Chr(13) & Strings.Chr(10) & "Do you really wish to cancel?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = System.Windows.Forms.DialogResult.No Then
                ' Do not procced with the interface termination.
                eventArgs.Cancel = 1
            Else
                Status = gPMConstants.PMEReturnCode.PMCancel
            End If
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

        With lvwReportSchedulerDetail
            .Columns.Clear()
            .Columns.Insert(0, "", "Automatic", CInt(VB6.TwipsToPixelsX(100)), HorizontalAlignment.Left, -1)
            .Columns.Insert(1, "", "", CInt(VB6.TwipsToPixelsX(2000)), HorizontalAlignment.Left, -1) 'Parameter ID
            .Columns.Insert(2, "", "Name", CInt(VB6.TwipsToPixelsX(2000)), HorizontalAlignment.Left, -1)
            .Columns.Insert(3, "", "Type", CInt(VB6.TwipsToPixelsX(2000)), HorizontalAlignment.Left, -1)
            .Columns.Insert(4, "", "Value", CInt(VB6.TwipsToPixelsX(2000)), HorizontalAlignment.Left, -1)
            .CheckBoxes = True
            .AllowColumnReorder = True
            .Columns.Item(0).DisplayIndex = .Columns.Item(4).DisplayIndex
        End With

        ' Display all language specific captions.
        m_lReturn = DisplayCaptions()

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

            cboFrequency.FirstItem = "(None)"

            Dim r_vReportCode As Object(,)
        'This method return report code corresponding to reportID
        lReturn = Business.GetCodeFromReportID(ReportId:=ReportId, r_vReportCode:=r_vReportCode)
        'check report code not nothing
        If r_vReportCode IsNot Nothing Then
            'Here compare repord code is corresponding to Agent or SubAgent code.
            If (r_vReportCode(0, 0) = ACAgentReportCode Or r_vReportCode(0, 0) = ACSubAgentReportCode) Then
                'If codes found then set visible true seprate by label and combo box 
                lblSeprateBy.Visible = True
                cboSeprateBy.Visible = True
                Dim SeprateByLists = New Collections.Generic.List(Of SeprateByItem)
                If r_vReportCode(0, 0) = ACAgentReportCode Then
                    'populate seprate by combo
                    SeprateByLists.Add(New SeprateByItem(ACAgentShortCode, ACAgentShortDesc))
                ElseIf r_vReportCode(0, 0) = ACSubAgentReportCode Then
                    'populate seprate by combo
                    SeprateByLists.Add(New SeprateByItem(ACSubAgentShortCode, ACSubAgentShortDesc))
                End If
                cboSeprateBy.DataSource = SeprateByLists
                cboSeprateBy.DisplayMember = "Description"
                cboSeprateBy.ValueMember = "Code"
                cboSeprateBy.SelectedIndex = -1
            End If
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

            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceCaptionDetail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'Button

            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            'List View


            lvwReportSchedulerDetail.Columns.Item(0).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListDetailTitle1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwReportSchedulerDetail.Columns.Item(1).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListDetailTitle2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwReportSchedulerDetail.Columns.Item(2).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListDetailTitle3, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwReportSchedulerDetail.Columns.Item(3).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListDetailTitle4, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



            lvwReportSchedulerDetail.Columns.Item(4).Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACListDetailTitle5, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

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

        If Information.IsArray(m_vReportScheduler) Then
            For lCount As Integer = m_vReportScheduler.GetLowerBound(1) To m_vReportScheduler.GetUpperBound(1)
                txtReportName.Text = CStr(m_vReportScheduler(MainModule.ReportSchedulerEnum.DBMDescription, lCount))
                sFrequency = CStr(m_vReportScheduler(MainModule.ReportSchedulerEnum.DBMFrequency, lCount))
                chkOutputAsPDF.CheckState = IIf(CBool(m_vReportScheduler(MainModule.ReportSchedulerEnum.DBMExportToPDF, lCount)), CheckState.Checked, CheckState.Unchecked)
                chkOutputAsCSV.CheckState = IIf(CBool(m_vReportScheduler(MainModule.ReportSchedulerEnum.DBMExportToCSV, lCount)), CheckState.Checked, CheckState.Unchecked)
                chkArchieveAsPDF.CheckState = IIf(CBool(m_vReportScheduler(MainModule.ReportSchedulerEnum.DBMArchieveToPDF, lCount)), CheckState.Checked, CheckState.Unchecked)
                sSeprateBy = CStr(m_vReportScheduler(MainModule.ReportSchedulerEnum.DBMSeprateBy, lCount))
            Next
        End If
        cboSeprateBy.SelectedValue = sSeprateBy

        cboFrequency.RefreshList()

            Select Case sFrequency.ToUpper()
                Case "(None)"
                    cboFrequency.ListIndex = 0
                Case "ANNUALLY"
                    cboFrequency.ListIndex = 1
                Case "DAILY"
                    cboFrequency.ListIndex = 2
                Case "MONTHLY"
                    cboFrequency.ListIndex = 3
            End Select

        ' Clear the list before we start
        lvwReportSchedulerDetail.Items.Clear()

        ' Check for items (we may not have any yet)
        If Information.IsArray(m_vReportSchedulerDetail) Then
            ' Process all treaties

            For lCount As Integer = m_vReportSchedulerDetail.GetLowerBound(1) To m_vReportSchedulerDetail.GetUpperBound(1)

                If CStr(m_vReportSchedulerDetail(MainModule.ReportSchedulerDetailEnum.DBMParameterName, lCount)).Trim().ToUpper() <> "OPERATOR" Then
                    'Set oListItem = lvwReportSchedulerDetail.ListItems.Add(, "M" & m_vReportSchedulerDetail(DBMIsAutomatic, lCount), Trim$(m_vReportSchedulerDetail(DBMIsAutomatic, lCount)))
                    oListItem = lvwReportSchedulerDetail.Items.Add("")

                    oListItem.Checked = IIf(CBool(m_vReportSchedulerDetail(MainModule.ReportSchedulerDetailEnum.DBMIsAutomatic, lCount)), CheckState.Checked, CheckState.Unchecked)

                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(m_vReportSchedulerDetail(MainModule.ReportSchedulerDetailEnum.DBMReportSchedulerParameterID, lCount))

                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(m_vReportSchedulerDetail(MainModule.ReportSchedulerDetailEnum.DBMParameterName, lCount))

                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(m_vReportSchedulerDetail(MainModule.ReportSchedulerDetailEnum.DBMDataType, lCount))

                    ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(m_vReportSchedulerDetail(MainModule.ReportSchedulerDetailEnum.DBMDefaultValue, lCount))


                    ' Store array index so we can find the original record
                    'oListItem.Tag = lCount

                End If
            Next lCount
        End If

        ' Ignore errors this is only a cosmetic nicety
        'to be checked at runtime
        'lReturn = CType(ListViewAutoSize(lvwReportSchedulerDetail, True, True, Me), gPMConstants.PMEReturnCode)
        lReturn = CType(ListViewFunc.ListViewAutoSize(lvwReportSchedulerDetail, True, True), gPMConstants.PMEReturnCode)
        ' Refresh sort order
        'SortList lvwReportSchedulerDetail.SortKey, True
        For i As Integer = 0 To lvwReportSchedulerDetail.Columns.Count - 1

            If lvwReportSchedulerDetail.Columns(i).Text = "Type" Or lvwReportSchedulerDetail.Columns(i).Text = "ID" Then
                lvwReportSchedulerDetail.Columns(i).Width = 0
            End If

        Next

        Catch ex As Exception
        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally




        End Try
        Return result
    End Function

    Private Function GetBusiness() As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetBusiness"


        Try

        result = gPMConstants.PMEReturnCode.PMTrue

        If gPMFunctions.ToSafeInteger(CStr(ReportSchedulerId)) > 0 Then
            ' Get the details from the business object.

            lReturn = Business.GetScheduledReports(r_vScheduledReports:=m_vReportScheduler, v_lReportSchedulerID:=gPMFunctions.ToSafeInteger(CStr(ReportSchedulerId)))
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.GetScheduledReports", "Unable to get report scheduler")
            End If

            ' Get the details from the business object.

            lReturn = Business.GetReportSchedulerDetail(v_iReportSchedulerId:=gPMFunctions.ToSafeInteger(CStr(ReportSchedulerId)), r_vReportSchedulerDetail:=m_vReportSchedulerDetail)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.GetReportSchedulerDetail", "Unable to get report scheduler detail")
            End If

        End If


        Catch ex As Exception
        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally




        End Try
        Return result
    End Function

    Private Sub lvwReportSchedulerDetail_ItemClick(ByVal Item As ListViewItem)
        m_bIsChanged = True
    End Sub

    Private Function UpdateReportScheduler() As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Dim v_sFrequency As String = ""
        Dim v_iExportToPDF, v_iArchieveToPDF, v_iExportToCSV As Integer
        Dim v_vparameters As Object
        Dim v_iReportSchedulerId As Integer
        Dim v_sSeprateBy As String = ""

        Const kMethodName As String = "UpdateReportScheduler"


        Try

        result = gPMConstants.PMEReturnCode.PMTrue

        v_sFrequency = Frequency
        v_iExportToPDF = ExportToPDF
        v_iArchieveToPDF = ArchieveToPDF
        v_iExportToCSV = ExportToCSV


        v_vparameters = Parameters
        v_iReportSchedulerId = ReportSchedulerId
        v_sSeprateBy = SeprateBy
        ' Get the details from the business object.

        lReturn = Business.UpdateReportScheduler(v_iReportSchedulerId:=v_iReportSchedulerId, v_vparameters:=v_vparameters, v_sFrequency:=v_sFrequency, v_iExportToPDF:=v_iExportToPDF, v_iArchieveToPDF:=v_iArchieveToPDF, v_iExportToCSV:=v_iExportToCSV, v_sSeprateBy:=v_sSeprateBy)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oBusiness.UpdateReportScheduler", "Unable to get report scheduler")
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
            Frequency = gPMFunctions.ToSafeString(cboFrequency.ItemCaption(cboFrequency.ItemId)).Trim()
            ExportToPDF = IIf(chkOutputAsPDF.CheckState, 1, 0)
            ArchieveToPDF = IIf(chkArchieveAsPDF.CheckState, 1, 0)
            ExportToCSV = IIf(chkOutputAsCSV.CheckState, 1, 0)
            If cboSeprateBy.SelectedValue IsNot Nothing Then
                SeprateBy = gPMFunctions.ToSafeString(cboSeprateBy.SelectedValue.Trim())
            End If

            m_vParameters = ""

            If lvwReportSchedulerDetail.Items.Count > 0 Then
                ReDim m_vParameters(lvwReportSchedulerDetail.Items.Count - 1, 1)
            End If


            For lRow As Integer = 1 To lvwReportSchedulerDetail.Items.Count


                m_vParameters.SetValue(gPMFunctions.ToSafeInteger(ListViewHelper.GetListViewSubItem(lvwReportSchedulerDetail.Items.Item(lRow - 1), 1).Text), lRow - 1, 0) 'Parameter ID
                If lvwReportSchedulerDetail.Items.Item(lRow - 1).Checked Then

                    m_vParameters(lRow - 1, 1) = 1
                Else

                    m_vParameters(lRow - 1, 1) = 0
                End If
                If (ListViewHelper.GetListViewSubItem(lvwReportSchedulerDetail.Items.Item(lRow - 1), 2).Text).ToLower = "end_date" Then
                    nEndDateIndex = lRow - 1
                    bReportContainEndDate = True

                End If
                If (ListViewHelper.GetListViewSubItem(lvwReportSchedulerDetail.Items.Item(lRow - 1), 2).Text).ToLower = "start_date" Then
                    nStartDateIndex = lRow - 1
                    bReportContainStartDate = True
                End If
            Next

            If (bReportContainEndDate AndAlso bReportContainStartDate) Then
                If lvwReportSchedulerDetail.Items.Item(nStartDateIndex).Checked And lvwReportSchedulerDetail.Items.Item(nEndDateIndex).Checked And Frequency.ToUpper() = "(NONE)" Then
                    MessageBox.Show("Select frequency other than None." & Strings.Chr(10).ToString(), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    m_bIsWrongFrequency = True
                    Exit Function
                Else
                    m_bIsWrongFrequency = False
                End If
                If lvwReportSchedulerDetail.Items.Item(nStartDateIndex).Checked And lvwReportSchedulerDetail.Items.Item(nEndDateIndex).Checked = False Then
                    MessageBox.Show("End date is not selected. End date will be selected automatically" & Strings.Chr(10).ToString(), "Warning Message", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    lvwReportSchedulerDetail.Items.Item(nEndDateIndex).Checked = True
                    cmdOK_Click(Nothing, Nothing)
                End If
            End If


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

    Private Sub chkOutputAsCSV_CheckStateChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles chkOutputAsCSV.CheckStateChanged
        If (chkOutputAsCSV.Checked AndAlso Not chkOutputAsPDF.Checked AndAlso Not chkArchieveAsPDF.Checked) Then
            chkOutputAsPDF.Checked = False

        ElseIf (chkOutputAsPDF.Checked AndAlso chkOutputAsCSV.Checked AndAlso chkArchieveAsPDF.Checked) Then
            chkOutputAsPDF.Checked = False
        ElseIf (chkOutputAsCSV.Checked AndAlso chkOutputAsPDF.Checked AndAlso Not chkArchieveAsPDF.Checked) Then
            chkOutputAsPDF.Checked = False
        ElseIf (Not chkOutputAsCSV.Checked AndAlso Not chkOutputAsPDF.Checked AndAlso chkArchieveAsPDF.Checked) Then

            chkArchieveAsPDF.Checked = False
        ElseIf (Not chkOutputAsCSV.Checked AndAlso chkArchieveAsPDF.Checked) Then
            chkArchieveAsPDF.Checked = True
        Else
            chkArchieveAsPDF.Checked = False
        End If
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        ' Check the user wants to close
        If MessageBox.Show("Cancelling will lose all of your current details." & _
                           Strings.Chr(13) & Strings.Chr(10) & "Do you really wish to cancel?", Text, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) = System.Windows.Forms.DialogResult.Yes Then
            ' Set status to cancel and close
            Status = gPMConstants.PMEReturnCode.PMCancel
            Me.Hide()
        End If
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim dShare As Double
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "cmdOK_Click"


        Try

        ' Validate data
        If Validate_Renamed() = gPMConstants.PMEReturnCode.PMTrue Then
            ' Set status to OK and close

            'Edit
                lReturn = CType(GetSetValues(), gPMConstants.PMEReturnCode)

                If m_bIsWrongFrequency = False Then
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("cmd_ok", "Failed to Get Set Values")
                    End If

                    lReturn = CType(UpdateReportScheduler(), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("cmd_ok", "Failed to Update Report Scheduler Details")
                    End If

                    Status = gPMConstants.PMEReturnCode.PMOK
                    Me.Hide()
                End If
        End If

        Catch ex As Exception
        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally


        End Try
    End Sub
    Private Sub frmReportSchedulerDetail_KeyDown(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles MyBase.KeyDown
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        If eventArgs.KeyCode = Keys.Tab Then
            If Shift And ShiftConstants.CtrlMask Then
                If Shift And ShiftConstants.ShiftMask Then

                Else

                End If
            End If
        End If
    End Sub

    Private Sub chkOutputAsPDF_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkOutputAsPDF.CheckedChanged
        If (chkOutputAsPDF.Checked AndAlso Not chkOutputAsCSV.Checked AndAlso Not chkArchieveAsPDF.Checked) Then
            chkOutputAsCSV.Checked = False

        ElseIf (chkOutputAsPDF.Checked AndAlso chkOutputAsCSV.Checked AndAlso chkArchieveAsPDF.Checked) Then
            chkOutputAsCSV.Checked = False
        ElseIf (chkOutputAsCSV.Checked AndAlso chkOutputAsPDF.Checked AndAlso Not chkArchieveAsPDF.Checked) Then
            chkOutputAsCSV.Checked = False
        ElseIf (Not chkOutputAsCSV.Checked AndAlso Not chkOutputAsPDF.Checked AndAlso chkArchieveAsPDF.Checked) Then

            chkArchieveAsPDF.Checked = False
        ElseIf (Not chkOutputAsPDF.Checked AndAlso chkArchieveAsPDF.Checked) Then
            chkArchieveAsPDF.Checked = True
        Else
            chkArchieveAsPDF.Checked = False
        End If


    End Sub
    Private Sub chkArchive_ChekedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkArchieveAsPDF.CheckedChanged
        If ((chkOutputAsCSV.Checked Or chkOutputAsPDF.Checked) AndAlso chkArchieveAsPDF.Checked) Then
            chkArchieveAsPDF.Checked = True
        ElseIf ((chkOutputAsCSV.Checked Or chkOutputAsPDF.Checked) AndAlso Not chkArchieveAsPDF.Checked) Then
            chkArchieveAsPDF.Checked = False
        Else
            chkArchieveAsPDF.Checked = False
        End If

    End Sub
End Class

'This class is used for generatig separate reports for all Agent and SubAgent.
Public Class SeprateByItem

    Public Sub New(ByVal Code As String, ByVal Description As String)
        m_Code = Code
        m_Description = Description
    End Sub

    Private m_Code As String
    Public Property Code() As String
        Get
            Return m_Code
        End Get
        Set(ByVal value As String)
            m_Code = value
        End Set
    End Property

    Private m_Description As String
    Public Property Description() As String
        Get
            Return m_Description
        End Get
        Set(ByVal value As String)
            m_Description = value
        End Set
    End Property
End Class
