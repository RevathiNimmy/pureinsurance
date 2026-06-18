Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129 (guide)
Imports SharedFiles
Partial Friend Class frmInstalment
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInstalment
    '
    ' Date: 26/09/2001
    '
    ' Description: Instalment interface.
    '
    ' Edit History:
    '   PF260901 - Created (again)
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmInstalment"

    Private m_lInstalment As Integer
    Private m_vInstalmentArray(,) As Object
    Public m_vFinancePlanInstalmentArray As Object = Nothing
    Private m_vStatusArray As Object
    Private m_vTransArray As Object

    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    Private m_oBusiness As bSIRPremiumFinance.Business
    Private m_vInstalmentHistory(,) As Object

    Private m_lPFInstalmentId As Integer
    Private m_iTask As gPMConstants.PMEComponentAction

    Private m_nInstalmentStatus As Integer

    Private m_iInstalmentFinanceVersion As Integer
    Private m_iInstalmentNumber As Integer
    Private m_dtOriginalDueDate As Date
    Private m_iInstalmentDueDate As Integer
    Private m_IsDueDateEditable As Boolean = False
    Private m_dtCoverExpiryDate As Date
    Private m_dtNextInstalmentDuedate As Date
    Private m_sPlanAutoGenPlanRef As String
    Private m_iPremFinancialcnt As Integer

    Public WriteOnly Property Business() As bSIRPremiumFinance.Business
        Set(ByVal Value As bSIRPremiumFinance.Business)
            m_oBusiness = Value
        End Set
    End Property

    Public WriteOnly Property Instalment() As Integer
        Set(ByVal Value As Integer)
            m_lInstalment = Value
        End Set
    End Property

    Public Property InstalmentArray() As Object
        Get
            Return VB6.CopyArray(m_vInstalmentArray)
        End Get
        Set(ByVal Value As Object)
            m_vInstalmentArray = Value
        End Set
    End Property
    Public WriteOnly Property Task() As Integer
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property

    Public WriteOnly Property IsDueDateEditable() As Boolean
        Set(ByVal Value As Boolean)
            m_IsDueDateEditable = Value
        End Set
    End Property

    Public WriteOnly Property CoverExpiryDate() As Date
        Set(ByVal Value As Date)
            m_dtCoverExpiryDate = Value
        End Set
    End Property

    Public WriteOnly Property NextInstalmentDuedate() As Date
        Set(ByVal Value As Date)
            m_dtNextInstalmentDuedate = Value
        End Set
    End Property

    Public WriteOnly Property PlanAutoGenPlanRef() As Integer
        Set(ByVal Value As Integer)
            m_sPlanAutoGenPlanRef = Value
        End Set
    End Property

    Private Sub SetControls()

        With m_oFormFields
            m_lReturn = .AddNewFormField(ctlControl:=txtBatchNo, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            m_lReturn = .AddNewFormField(ctlControl:=txtInstalmentNo, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            m_lReturn = .AddNewFormField(ctlControl:=txtAmount, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            m_lReturn = .AddNewFormField(ctlControl:=txtFee, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            '    m_lReturn = .AddNewFormField(ctlControl:=txtDueDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateShort, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            m_lReturn = .AddNewFormField(ctlControl:=txtPaidDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateShort, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            m_lReturn = .AddNewFormField(ctlControl:=txtPostedDate, lFormat:=gPMConstants.PMEFormatStyle.PMFormatDateShort, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            m_lReturn = .AddNewFormField(ctlControl:=txtTransactionCode, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            m_lReturn = .AddNewFormField(ctlControl:=txtStatus, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

            m_lReturn = .AddNewFormField(ctlControl:=txtReason, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lFieldType:=gPMConstants.PMEDataType.PMString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)

        End With
    End Sub

    ''' <summary>
    ''' SetControlsValues
    ''' </summary>    
    ''' <remarks></remarks>
    Private Sub SetControlsValues()

        With m_oFormFields

            m_lReturn = .FormatControl(ctlControl:=txtBatchNo, vControlValue:=m_vInstalmentArray(bSIRPremFinConst.PFInstBatchNumber, m_lInstalment))

            m_lReturn = .FormatControl(ctlControl:=txtInstalmentNo, vControlValue:=m_vInstalmentArray(bSIRPremFinConst.PFInstInstalmentNumber, m_lInstalment))
            m_iInstalmentNumber = m_vInstalmentArray(bSIRPremFinConst.PFInstInstalmentNumber, m_lInstalment)
            m_iPremFinancialcnt = m_vInstalmentArray(bSIRPremFinConst.PFInstFinanceCnt, m_lInstalment)
            m_iInstalmentFinanceVersion = m_vInstalmentArray(bSIRPremFinConst.PFInstFinanceVersion, m_lInstalment)

            m_lReturn = .FormatControl(ctlControl:=txtAmount, vControlValue:=m_vInstalmentArray(bSIRPremFinConst.PFInstAmount, m_lInstalment))

            m_lReturn = .FormatControl(ctlControl:=txtFee, vControlValue:=m_vInstalmentArray(bSIRPremFinConst.PFInstFee, m_lInstalment))

            ' m_lReturn = .FormatControl(ctlControl:=txtDueDate, vControlValue:=m_vInstalmentArray(bSIRPremFinConst.PFInstDueDate, m_lInstalment))
            m_dtOriginalDueDate = m_vInstalmentArray(bSIRPremFinConst.PFInstDueDate, m_lInstalment)
            m_iInstalmentFinanceVersion = m_vInstalmentArray(bSIRPremFinConst.PFInstFinanceVersion, m_lInstalment)

            m_lReturn = .FormatControl(ctlControl:=txtPaidDate, vControlValue:=m_vInstalmentArray(bSIRPremFinConst.PFInstPostedDate, m_lInstalment))

            m_lReturn = .FormatControl(ctlControl:=txtPaidDate, vControlValue:=m_vInstalmentArray(bSIRPremFinConst.PFInstPostedDate, m_lInstalment))

            m_lReturn = .FormatControl(ctlControl:=txtPostedDate, vControlValue:=m_vInstalmentArray(bSIRPremFinConst.PFInstPostedDate, m_lInstalment))

            m_lReturn = .FormatControl(ctlControl:=txtTransactionCode, vControlValue:=m_vInstalmentArray(bSIRPremFinConst.PFInstTransactionDescription, m_lInstalment))
            cboPMLookuppfinstalmentsresult.Visible = True
            txtReason.Visible = False
            If gPMFunctions.ToSafeInteger(m_vInstalmentArray(bSIRPremFinConst.PFInstResultID, m_lInstalment), 0) <> 0 Then
                cboPMLookuppfinstalmentsresult.ItemId = CInt(m_vInstalmentArray(bSIRPremFinConst.PFInstResultID, m_lInstalment))
            Else
                cboPMLookuppfinstalmentsresult.Visible = False
                cboPMLookuppfinstalmentsresult.ListIndex = -1
                txtReason.Visible = True
                m_lReturn = .FormatControl(ctlControl:=txtReason, vControlValue:=m_vInstalmentArray(bSIRPremFinConst.PFInstStatusDescription, m_lInstalment))

            End If

            If gPMFunctions.ToSafeInteger(m_vInstalmentArray(bSIRPremFinConst.PFInstStatus, m_lInstalment), 0) <> 0 Then
                cboPMLookuppfinstalmentsStatus.ItemId = CInt(m_vInstalmentArray(bSIRPremFinConst.PFInstStatus, m_lInstalment))
            End If

            m_lReturn = .FormatControl(ctlControl:=txtStatus, vControlValue:=m_vInstalmentArray(bSIRPremFinConst.PFInstStatusCode, m_lInstalment))

            If gPMFunctions.ToSafeInteger(m_vInstalmentArray(bSIRPremFinConst.kPFInstWriteOffReasonID, m_lInstalment), 0) <> 0 Then
                cboWriteOffReasonID.ItemId = m_vInstalmentArray(bSIRPremFinConst.kPFInstWriteOffReasonID, m_lInstalment)
            Else
                cboWriteOffReasonID.ListIndex = -1
            End If

            cboWriteOffReasonID.Enabled = False
            FillInstalmentDueDate()
        End With

        m_lPFInstalmentId = gPMFunctions.ToSafeLong(m_vInstalmentArray(bSIRPremFinConst.PFInstId, m_lInstalment), 0)

    End Sub

    'UPGRADE_NOTE: (7001) The following declaration (cmdCancel_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cmdCancel_Click()
    'Me.Hide()
    'End Sub

    Private Sub cmdNavigate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdNavigate.Click
        Dim lReturn As gPMConstants.PMEReturnCode = ProcessSaveChanges()
        If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            Me.Hide()
        End If
    End Sub

    ' ***************************************************************** '
    ' Name: ProcessSaveChanges
    '
    ' Parameters: n/a
    '
    ' Description: Update Instalment Status if Required
    '
    ' History:
    '           Created : MEvans : 09-05-2007 : Instalment_Import Changes
    ' ***************************************************************** '
    Public Function ProcessSaveChanges() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessSaveChanges"
        Const k_sFUNCTION_NAME As String = "Plan Maintenance"
        Dim lOriginalStatus, lNewStatus, lInstalmentId As Integer
        Dim dtNewDueDate As Date
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim policyno As String
        Dim currency As String
        Dim VInsuranneFileArray As Object
        Dim nInsuranceFilecnt As Integer
        Dim nInsuranceFoldercnt As Integer
        Dim nInsurancePartycnt As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' only save if the instalment status is enabled
            ' as this is the only field that can actually change
            ' and if its not enabled the values it stores are irrelevant
            If Not cboPMLookuppfinstalmentsStatus.Visible Then
                Return result
            End If

            ' determine if any changes have actually taken place
            lOriginalStatus = CInt(m_vInstalmentArray(bSIRPremFinConst.PFInstStatus, m_lInstalment))
            lNewStatus = cboPMLookuppfinstalmentsStatus.ItemId
            lInstalmentId = CInt(m_vInstalmentArray(bSIRPremFinConst.PFInstId, m_lInstalment))

            If lOriginalStatus <> lNewStatus Then

                ' update the changes back to the array
                m_vInstalmentArray(bSIRPremFinConst.PFInstStatus, m_lInstalment) = lNewStatus

                ' update the changes back to the database

                lReturn = m_oBusiness.UpdateInstalmentStatus(lInstalmentId, lNewStatus)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to update the status of the instalment. View error log for further details.", "Update Instalment Status Failed", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If

            End If
            dtNewDueDate = CDate(CboDueDate.SelectedItem)

            If Convert.ToDateTime(dtNewDueDate).ToString("yyyy/MM/dd") < DateTime.Now.ToString("yyyy/MM/dd") Then
                MessageBox.Show("New Due Date Is  Not valid. Must be a Future Date. Please enter a valid date.", "Update Instalment Duedate Failed", MessageBoxButtons.OK, MessageBoxIcon.Information)
                result = gPMConstants.PMEReturnCode.PMFalse
            End If


            result = m_oBusiness.GetInsuranceFileDetailsFromInstalmentPlan(m_iPremFinancialcnt, m_iInstalmentFinanceVersion, VInsuranneFileArray)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to GetInsuranceFileDetailsFromInstalmentPlan. View error log for further details.", "Get InsuranceFileDetails From InstalmentPlan", MessageBoxButtons.OK, MessageBoxIcon.Information)
                result = gPMConstants.PMEReturnCode.PMFalse
            End If


            If Information.IsArray(VInsuranneFileArray) Then
                nInsuranceFilecnt = VInsuranneFileArray(0, 0)
                nInsuranceFoldercnt = VInsuranneFileArray(1, 0)
                nInsurancePartycnt = VInsuranneFileArray(2, 0)
                policyno = VInsuranneFileArray(3, 0)
                currency = VInsuranneFileArray(4, 0)
            End If

            If m_dtOriginalDueDate <> dtNewDueDate Then

                m_vInstalmentArray(bSIRPremFinConst.PFInstDueDate, m_lInstalment) = dtNewDueDate

                lReturn = m_oBusiness.UpdateInstalmentDueDate(m_iPremFinancialcnt, m_iInstalmentFinanceVersion, m_iInstalmentNumber, dtNewDueDate)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    MessageBox.Show("Failed to update the Duedate of the instalment. View error log for further details.", "Update Instalment Duedate Failed", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    result = gPMConstants.PMEReturnCode.PMFalse
                Else
                    result = m_oBusiness.CreateInstalmentEvent(m_sPlanAutoGenPlanRef, m_iInstalmentNumber, m_dtOriginalDueDate, dtNewDueDate, nInsuranceFilecnt, nInsuranceFoldercnt, nInsurancePartycnt)
                    If result <> gPMConstants.PMEReturnCode.PMTrue Then
                        MessageBox.Show("Failed to create event of the instalment Duedate update. View error log for further details.", "Create EventLog Failed", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            If lNewStatus = 7 Then
                m_lReturn = g_oBusiness.GetSingleFinancePlan(m_iPremFinancialcnt, m_iInstalmentFinanceVersion, m_vFinancePlanInstalmentArray)

                m_lReturn = CreateWorkManagerTask(1, "PFMAINT", CStr(m_vFinancePlanInstalmentArray(bSIRPremFinConst.k_PFPlanClientName, 0)), Date.Now(), 1, String.Format("Policy No. {0}/Finance Plan {1}: Instalment No {2} Due date {3} –Amount({4} {5}) is on hold", policyno, CStr(m_vFinancePlanInstalmentArray(0, 0)),
                                                          m_iInstalmentNumber, m_dtOriginalDueDate.ToString("d-MM-yyyy"), currency, CStr(m_vFinancePlanInstalmentArray(bSIRPremFinConst.k_PFPlanNetAmount, 0))), gPMConstants.PMEWrkManTaskStatus.pmeWMTSNew, 0, 0, "", g_iUserId, , 1)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to Create a Work " & "Manager Task", ACApp, ACClass, k_sFUNCTION_NAME)
                    Return result
                End If
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            result = gPMConstants.PMEReturnCode.PMFalse

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Private Function CreateWorkManagerTask(ByVal v_lPMWrkTaskGroupID As Integer, ByVal v_sPMWrkTaskCode As String, ByVal v_sCustomer As String, ByVal v_dtTaskDueDate As Date, ByVal v_lPMUserGroupID As Integer, ByVal v_sDescription As String, ByVal v_iTaskStatus As Integer, ByVal v_iIsUrgent As Integer, ByRef r_lPMWrkTaskInstanceCnt As Integer, Optional ByRef v_sWorkflowInformation As String = "", Optional ByVal v_iUserID As Integer = 0, Optional ByVal v_vKeyArray(,) As Object = Nothing, Optional ByVal v_iIsVisible As Integer = gPMConstants.PMEReturnCode.PMTrue) As Integer

        Dim result As Integer = 0
        Dim obPMWrkTaskInstance As bPMWrkTaskInstance.TaskControl

        Try

            'TR -  Initialise Required Variables
            result = gPMConstants.PMEReturnCode.PMTrue

            'Create the business Object
            obPMWrkTaskInstance = New bPMWrkTaskInstance.TaskControl()

            'Initialise with the Sirius user and password
            m_lReturn = CType(obPMWrkTaskInstance, SSP.S4I.Interfaces.IBusiness).Initialise(sUsername:=g_oObjectManager.UserName, sPassword:=g_oObjectManager.Password, iUserID:=g_oObjectManager.UserID, iSourceID:=g_oObjectManager.SourceID, iLanguageID:=g_oObjectManager.LanguageID, iCurrencyID:=g_oObjectManager.CurrencyID, iLogLevel:=g_oObjectManager.LogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to Initialise " & "workmanager Task Business Object", ACApp, ACClass, "CreateWorkManagerTask")
                Return result
            End If

            'TR - Create the WorkManager Task - just pass through the parameters
            'Create Using Code not the bloody ID which changes!
            m_lReturn = obPMWrkTaskInstance.CreateNewByCode(v_lPMWrkTaskGroupID, v_sPMWrkTaskCode, v_sCustomer, v_dtTaskDueDate, v_lPMUserGroupID, v_sDescription, v_iTaskStatus, v_iIsUrgent, r_lPMWrkTaskInstanceCnt, v_sWorkflowInformation, v_iUserID, v_vKeyArray, v_iIsVisible)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to Create Work Manager " & "Task.", ACApp, ACClass, "CreateWorkManagerTask")
                Return result
            End If

            obPMWrkTaskInstance = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to Create workmanager " & "Task.", ACApp, ACClass, "CreateWorkManagerTask", Information.Err().Number, excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    Private Sub frmInstalment_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        cboPMLookuppfinstalmentsresult.Enabled = True

        m_oFormFields = New iPMFormControl.FormFields()

        SetControls()
        'Developer Guide No 220
        cboPMLookuppfinstalmentsStatus.FirstItem = ""
        cboPMLookuppfinstalmentsresult.FirstItem = ""

        SetControlsValues()

        GetInstalmentHistory()

        SetupInstalmentHistoryListView()

        PopulateInstalmentHistoryListView()

        ' get the current instalment status
        Dim lInstalmentStatus As Integer = CInt(m_vInstalmentArray(bSIRPremFinConst.PFInstStatus, m_lInstalment))

        ' if the status is
        ' New, Retrying, Hold, Failed or Write-Off
        ' allow the user to manually change this value within the above range

        Select Case lInstalmentStatus
            Case 1, 5, 6, 7, 8

                txtStatus.Visible = False
                cboPMLookuppfinstalmentsStatus.Visible = True

                ' if its not only allow viewing of the data
            Case Else

                txtStatus.Visible = True
                txtStatus.Enabled = False
                cboPMLookuppfinstalmentsStatus.Visible = False

        End Select

        cboPMLookuppfinstalmentsresult.Enabled = False

        If m_iTask = gPMConstants.PMEComponentAction.PMView Then
            cboPMLookuppfinstalmentsStatus.Enabled = False
        End If
        ' PN72529
        m_lReturn = SetInstalmentStatusFromUserAuthority()
        If m_nInstalmentStatus = 1 And m_iTask <> gPMConstants.PMEComponentAction.PMView Then
            cboPMLookuppfinstalmentsStatus.Enabled = True
        Else
            cboPMLookuppfinstalmentsStatus.Enabled = False
        End If

        Dim result As Integer = 0
        result = SetInstalmentDueDateFromUserAuthority()
        If m_iInstalmentDueDate = 1 AndAlso m_IsDueDateEditable = True Then
            CboDueDate.Enabled = True
        Else
            CboDueDate.Enabled = False
        End If
    End Sub

    ' ***************************************************************** '
    ' Name: PopulateInstalmentHistoryListView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function PopulateInstalmentHistoryListView() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateInstalmentHistoryListView"

        Dim lReturn, llBound, lUBound As Integer

        Dim sPostedDate, sPFInstalmentStatusDescription, sPFInstalmentResultDescription, sPFInstalmentResultCode As String
        Dim oListItem As ListViewItem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lvwInstalmentEvents.Items.Clear()

            If Information.IsArray(m_vInstalmentHistory) Then

                llBound = m_vInstalmentHistory.GetLowerBound(1)
                lUBound = m_vInstalmentHistory.GetUpperBound(1)

                For lHistoryItemIndex As Integer = llBound To lUBound

                    sPostedDate = CStr(m_vInstalmentHistory(0, lHistoryItemIndex))
                    sPFInstalmentStatusDescription = CStr(m_vInstalmentHistory(1, lHistoryItemIndex))
                    sPFInstalmentResultDescription = CStr(m_vInstalmentHistory(2, lHistoryItemIndex))
                    sPFInstalmentResultCode = CStr(m_vInstalmentHistory(3, lHistoryItemIndex))

                    oListItem = lvwInstalmentEvents.Items.Add("")

                    oListItem.Text = sPostedDate
                    ListViewHelper.GetListViewSubItem(oListItem, 1).Text = sPFInstalmentStatusDescription
                    ListViewHelper.GetListViewSubItem(oListItem, 2).Text = sPFInstalmentResultCode
                    ListViewHelper.GetListViewSubItem(oListItem, 3).Text = sPFInstalmentResultDescription

                Next

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sUsername:=g_sUserName, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetupInstalmentHistoryListView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function SetupInstalmentHistoryListView() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupInstalmentHistoryListView"

        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lvwInstalmentEvents.Columns.Clear()
            lvwInstalmentEvents.Columns.Insert(0, "", "Paid Date", CInt(VB6.TwipsToPixelsX(1500)))
            lvwInstalmentEvents.Columns.Insert(1, "", "Status", CInt(VB6.TwipsToPixelsX(1500)))
            lvwInstalmentEvents.Columns.Insert(2, "", "Reason", CInt(VB6.TwipsToPixelsX(1500)))
            lvwInstalmentEvents.Columns.Insert(3, "", "Description", CInt(VB6.TwipsToPixelsX(1500)))


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sUsername:=g_sUserName, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetInstalmentHistory
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 11-10-2007 : ARUDDS Phase II
    ' ***************************************************************** '
    Public Function GetInstalmentHistory() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetInstalmentHistory"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = m_oBusiness.GetInstalmentHistory(m_lPFInstalmentId, m_vInstalmentHistory)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInstalmentHistory Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sUsername:=g_sUserName, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function
    ''' <summary>
    ''' This Method is used to authenticate the permission for instalment status.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetInstalmentStatusFromUserAuthority() As Long
        Dim oUserAuthorities As Object = Nothing
        Dim nReturn As Integer
        Const kMethodName As String = "SetInstalmentStatusFromUserAuthority"
        Try

            SetInstalmentStatusFromUserAuthority = gPMConstants.PMEReturnCode.PMTrue

            nReturn = g_oObjectManager.GetInstance( _
                    oObject:=oUserAuthorities, _
                    sClassName:="bACTUserAuthorities.Business", _
                    vInstanceManager:=PMGetViaClientManager)

            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to get instance of bACTUserAuthorities.Business", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = oUserAuthorities.GetValueFromTable(v_sTableName:="User_Authorities", _
                                                               v_vReturnColumn:="can_update_instalment_status", _
                                                               v_sKeyColumn:="user_id", _
                                                               v_sKeyValue:=g_oObjectManager.UserID, _
                                                               v_iDataType:=gPMConstants.PMEDataType.PMLong, _
                                                               r_vResult:=m_nInstalmentStatus)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to Execute GetValueFromTable", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Terminate the object
            nReturn = oUserAuthorities.Terminate()
            oUserAuthorities = Nothing

        Catch Ex As Exception
            nReturn = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to call " & kMethodName & "" _
                               , ACApp, ACClass, "SetInstalmentStatusFromUserAuthority", Information.Err().Number, Ex.Message)
            Return nReturn
        End Try

        Return nReturn
    End Function
    ''' <summary>
    ''' CR - 2573
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetInstalmentDueDateFromUserAuthority() As Long
        Dim oUserAuthorities As Object = Nothing
        Dim nReturn As Integer
        Const kMethodName As String = "SetInstalmentDueDateFromUserAuthority"
        Try

            SetInstalmentDueDateFromUserAuthority = gPMConstants.PMEReturnCode.PMTrue

            nReturn = g_oObjectManager.GetInstance( _
                    oObject:=oUserAuthorities, _
                    sClassName:="bACTUserAuthorities.Business", _
                    vInstanceManager:=PMGetViaClientManager)

            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to get instance of bACTUserAuthorities.Business", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = oUserAuthorities.GetValueFromTable(v_sTableName:="User_Authorities", _
                                                               v_vReturnColumn:="can_edit_instalment_date", _
                                                               v_sKeyColumn:="user_id", _
                                                               v_sKeyValue:=g_oObjectManager.UserID, _
                                                               v_iDataType:=gPMConstants.PMEDataType.PMLong, _
                                                               r_vResult:=m_iInstalmentDueDate)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to Execute GetValueFromTable", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Terminate the object
            nReturn = oUserAuthorities.Terminate()
            oUserAuthorities = Nothing

        Catch Ex As Exception
            nReturn = gPMConstants.PMEReturnCode.PMError
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "Failed to call " & kMethodName & "" _
                               , ACApp, ACClass, "SetInstalmentDueDateFromUserAuthority", Information.Err().Number, Ex.Message)
            Return nReturn
        End Try
        Return nReturn
    End Function

    Private Sub FillInstalmentDueDate()
        CboDueDate.Items.Clear()
        Dim iDaysDelay As Integer = 0
        Dim iStartLimit As Integer = 0
        Dim oUserAuthorities As Object = Nothing
        Dim nReturn As Integer
        Dim nInstalmentDayes As Integer

        nReturn = g_oObjectManager.GetInstance( _
                   oObject:=oUserAuthorities, _
                   sClassName:="bACTUserAuthorities.Business", _
                   vInstanceManager:=PMGetViaClientManager)

        If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            RaiseError("FillDueDate", "Failed to get instance of bACTUserAuthorities.Business", gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = oUserAuthorities.GetValueFromTable(v_sTableName:="User_Authorities", _
                                                           v_vReturnColumn:="edit_instalment_by_no_of_days", _
                                                           v_sKeyColumn:="user_id", _
                                                           v_sKeyValue:=g_oObjectManager.UserID, _
                                                           v_iDataType:=gPMConstants.PMEDataType.PMLong, _
                                                           r_vResult:=nInstalmentDayes)

        If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError("FillDueDate", "Failed to Execute GetValueFromTable", gPMConstants.PMELogLevel.PMLogError)
        End If

        'Terminate the object
        nReturn = oUserAuthorities.Terminate()
        oUserAuthorities = Nothing

        CboDueDate.Items.Add(m_dtOriginalDueDate.ToString("d"))
        CboDueDate.SelectedIndex = 0
        ' m_vInstalmentArray(bSIRPremFinConst.PFInstDueDate, m_lInstalment)
        For iCount As Integer = 1 To ToSafeInteger(nInstalmentDayes)
            Dim dtNextDuedate As Date = Date.MinValue
            dtNextDuedate = m_dtOriginalDueDate.AddDays(iCount).ToString("d")
            If dtNextDuedate < m_dtCoverExpiryDate AndAlso (dtNextDuedate <= m_dtNextInstalmentDuedate Or m_dtNextInstalmentDuedate.Equals(Date.MinValue)) Then
                CboDueDate.Items.Add(dtNextDuedate)
            End If
        Next iCount
        CboDueDate.SelectedIndex = 0
    End Sub



End Class