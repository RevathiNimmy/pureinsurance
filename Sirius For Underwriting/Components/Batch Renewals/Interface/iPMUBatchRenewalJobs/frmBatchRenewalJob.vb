Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Text
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmBatchRenewalJob
    Inherits System.Windows.Forms.Form

    ' Constant for the functions to identify
    Private Const ACClass As String = "frmBatchRenewalJob"

    ' PRIVATE Events (Begin)

    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields
    Private m_lReturn As Integer
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lStatus As Integer
    Private m_lErrorNumber As gPMConstants.PMEReturnCode
    Private m_oGeneral As General
    Private m_oBusiness As Object

    Private m_oPMUser As bPMUser.Business
    Private m_iBatchRenewalJobId As Integer
    Private m_vLeadAgentCnt As Object
    Private m_vGetAssociatedSubAgent(,) As Object
    Private m_iAgentAllowedCommission As Integer
    Private m_sTransactionType As String = ""
    Private m_iLine As Integer
    Private m_lAgentId As Integer
    Private m_vSearchData(,) As Object

    ' PRIVATE Events (End)

    Public Event SubAgentChange() 'Event fired when subagent added or  deleted

    ' PUBLIC Property (Begin)
    Public WriteOnly Property Task() As Integer
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

    Public WriteOnly Property BatchRenewalJobId() As Integer
        Set(ByVal Value As Integer)
            m_iBatchRenewalJobId = Value
        End Set
    End Property

    ' PUBLIC Property (End)

    ' PRIVATE Property (Begin)
    Private ReadOnly Property Code() As String
        Get
            Return txtJobCode.Text.Trim()
        End Get
    End Property

    Private ReadOnly Property Description() As String
        Get
            Return txtDescription.Text.Trim()
        End Get
    End Property

    Private ReadOnly Property SAMServer() As String
        Get
            Return txtSAMServer.Text.Trim()
        End Get
    End Property

    Private ReadOnly Property DaysBeforeRenewalDate() As Integer
        Get
            Return gPMFunctions.ToSafeInteger(txtDaysBeforeRenewalDate.Text)
        End Get
    End Property

    Private ReadOnly Property IsActive() As Integer
        Get
            Return gPMFunctions.ToSafeInteger(CStr(chkIsActive.CheckState))
        End Get
    End Property

    Private ReadOnly Property BatchRenewalJobTypeId() As Integer
        Get
            Return gPMFunctions.ToSafeInteger(CStr(cboJobType.ItemId))
        End Get
    End Property

    Private ReadOnly Property RenewalDocsDestination() As Integer
        Get
            If optNotRequired.Checked Then
                Return 0
            ElseIf optSpool.Checked Then
                Return 2
            Else
                'Print Default
                Return 1
            End If
        End Get
    End Property

    Private ReadOnly Property ReportSortOrder() As Integer
        Get
            If optPolicyNumber.Checked Then
                Return 2
            Else
                'Client default
                Return 1
            End If
        End Get
    End Property

    Private ReadOnly Property AllAgents() As Integer
        Get
            If optSelectedAgents.Checked Then
                Return 0
            Else
                'All Agents default
                Return 1
            End If
        End Get
    End Property

    Private ReadOnly Property IncludeDirectPolicies() As Integer
        Get
            Return gPMFunctions.ToSafeInteger(CStr(chkIncludeDirectPolicies.CheckState))
        End Get
    End Property

    ' PRIVATE Property (End)

    Public Function DataToInterface() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DataToInterface"
        Dim vBatch_Renewal_Job_Id As Object
        Dim dtCreated As Date
        Dim sJobCode, sDescription As String
        Dim iStatus As Integer
        Dim sJobType, sUsername As String
        Dim oListItem As ListViewItem
        Dim lRow As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
            optAllAgents.Checked = True
            cmdAddProduct.Enabled = False
            cmdRemoveProduct.Enabled = False
        End If

        If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then

            m_lReturn = GetBusiness(m_iBatchRenewalJobId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to call bSIRBatchRenewalJobs.GetJobCode")
            End If

        End If

        GoTo Finally_Renamed



        ' Log Error.
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value)

Finally_Renamed:
        Return result
        Resume
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SetFieldValidation
    '
    ' Description: Sets the rules for validating fields.
    '
    ' ***************************************************************** '
    Public Function SetFieldValidation() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "SetFieldValidation"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtDescription, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboJobType, lFieldType:=gPMConstants.PMEDataType.PMLookup, lFormat:=gPMConstants.PMEFormatStyle.PMFormatBoolean, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSAMServer, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If




        Catch ex As Exception

            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: InterfaceToBusiness
    '
    ' Description: Updates all business members from the interface
    '              details.
    '
    ' ***************************************************************** '
    Public Function InterfaceToBusiness() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "InterfaceToBusiness"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vResultArray(,) As Object
            Dim sCode, sDescription, sSAMServer As String
            Dim iDaysBeforeRenewalDate, iIsActive, iBatchRenewalJobTypeId, iRenewalDocsDestination, iReportSortOrder, iAllAgents, iIsIncludeDirectPolicies As Integer
            Dim bRunExtendedRule As Boolean

            'Filling From Private Property
            sCode = Code
            sDescription = Description
            sSAMServer = SAMServer
            iDaysBeforeRenewalDate = DaysBeforeRenewalDate
            iIsActive = IsActive
            iBatchRenewalJobTypeId = BatchRenewalJobTypeId
            iRenewalDocsDestination = RenewalDocsDestination
            iReportSortOrder = ReportSortOrder
            iAllAgents = AllAgents
            iIsIncludeDirectPolicies = IncludeDirectPolicies
            bRunExtendedRule = chkRunExtendedRule.Checked

            ' Check the task.
            Select Case (m_iTask)
                Case gPMConstants.PMEComponentAction.PMAdd

                    m_lReturn = g_oBusiness.DirectAdd(v_vCode:=sCode, v_vDescription:=sDescription, v_vSAMServer:=sSAMServer, v_vDaysBeforeRenewalDate:=iDaysBeforeRenewalDate, v_vIsActive:=iIsActive, v_vBatchRenewalJobTypeId:=iBatchRenewalJobTypeId, v_vRenewalDocsDestination:=iRenewalDocsDestination, v_vReportSortOrder:=iReportSortOrder, v_vAllAgents:=iAllAgents, v_vPMUserId:=g_iUserID, v_vIncludeDirectPolicies:=iIsIncludeDirectPolicies, r_vResultArray:=vResultArray, v_bRunExtendedRule:=bRunExtendedRule)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Failed to save Batch Renewal Job")
                    End If

                    If Information.IsArray(vResultArray) Then

                        m_iBatchRenewalJobId = CInt(vResultArray(0, 0))
                        'Save the pick product lists
                        'Put in the picklist PK values

                        uctPickListProducts.ForeignKeys.Item("BatchRenewalJobId").Value = m_iBatchRenewalJobId
                        m_lReturn = uctPickListProducts.Save()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "Failed to save Product Pick List")
                        End If

                        'Save the pick branch lists
                        'Put in the picklist PK values

                        uctPickListBranches.ForeignKeys.Item("BatchRenewalJobId").Value = m_iBatchRenewalJobId
                        m_lReturn = uctPickListBranches.Save()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "Failed to save Branch Pick List")
                        End If

                        m_lReturn = SetAgentList(m_iBatchRenewalJobId)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "Failed to save Agents")
                        End If

                    End If

                Case gPMConstants.PMEComponentAction.PMEdit


                    m_lReturn = g_oBusiness.DirectUpdate(v_vBatchRenewalJobId:=m_iBatchRenewalJobId, v_vCode:=sCode, v_vDescription:=sDescription, v_vSAMServer:=sSAMServer, v_vDaysBeforeRenewalDate:=iDaysBeforeRenewalDate, v_vIsActive:=iIsActive, v_vBatchRenewalJobTypeId:=iBatchRenewalJobTypeId, v_vRenewalDocsDestination:=iRenewalDocsDestination, v_vReportSortOrder:=iReportSortOrder, v_vAllAgents:=iAllAgents, v_vPMUserId:=g_iUserID, v_vIncludeDirectPolicies:=iIsIncludeDirectPolicies, v_bRunExtendedRule:=bRunExtendedRule)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Failed to update Batch Renewal Job")
                        Return result
                    End If

                    If m_iBatchRenewalJobId >= 0 Then
                        'Save the pick product lists
                        'Put in the picklist PK values

                        uctPickListProducts.ForeignKeys.Item("BatchRenewalJobId").Value = m_iBatchRenewalJobId
                        m_lReturn = uctPickListProducts.Save()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "Failed to save Product Pick List")
                        End If

                        'Save the pick branch lists
                        'Put in the picklist PK values

                        uctPickListBranches.ForeignKeys.Item("BatchRenewalJobId").Value = m_iBatchRenewalJobId
                        m_lReturn = uctPickListBranches.Save()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "Failed to save Branch Pick List")
                        End If

                        m_lReturn = SetAgentList(m_iBatchRenewalJobId)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "Failed to save Agents")
                        End If
                    End If
            End Select

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(kMethodName, "Failed to assign the interface details to business object")
            End If


        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    Private Sub cboJobType_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboJobType.Click

        If cboJobType.ItemId > 0 And (cboJobType.ItemId = kAcceptance Or cboJobType.ItemId = kInvitation) Then
            lblDaysBeforeRenewalDate.Enabled = True
            txtDaysBeforeRenewalDate.Enabled = True
        Else
            lblDaysBeforeRenewalDate.Enabled = False
            txtDaysBeforeRenewalDate.Text = "0"
            txtDaysBeforeRenewalDate.Enabled = False
        End If

        If cboJobType.ItemId > 0 Then 'And (cboJobType.ItemId = kSelection Or cboJobType.ItemId = kAcceptance) Then
            optNotRequired.Enabled = True
            optSpool.Enabled = True
        Else
            optNotRequired.Checked = False
            optNotRequired.Enabled = False
            optSpool.Enabled = False
            optSpool.Checked = False
        End If

        If cboJobType.ItemId > 0 And cboJobType.ItemId = kSelection Then
            fraSortOrder.Enabled = True
            optClient.Enabled = True
            optPolicyNumber.Enabled = True
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                optClient.Checked = True
            End If
            optSpool.Left = optPrint.Left
            optNotRequired.Top = VB6.TwipsToPixelsY(390)
            optNotRequired.Left = VB6.TwipsToPixelsX(3435)
            optNotRequired.BringToFront()
        Else
            optSpool.Left = optPrint.Left + VB6.TwipsToPixelsX(3015)
            optClient.Checked = False
            optPolicyNumber.Checked = False
            fraSortOrder.Enabled = False
            optClient.Enabled = False
            optPolicyNumber.Enabled = False
            optNotRequired.Top = VB6.TwipsToPixelsY(390)
            optNotRequired.Left = VB6.TwipsToPixelsX(6600)
        End If

        If cboJobType.ItemId > 0 And m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
            GetJobCode(cboJobType.ItemCaption)
        End If

        If cboJobType.ItemId > 0 And cboJobType.ItemId = kSelection Then
            fraRenewalReportDocuments.Text = "Renewal Reports"
        ElseIf cboJobType.ItemId > 0 And (cboJobType.ItemId = kInvitation Or cboJobType.ItemId = kAcceptance) Then
            fraRenewalReportDocuments.Text = "Renewal Documents"
        Else
            fraRenewalReportDocuments.Text = "Renewal Reports/Documents"
        End If
        If cboJobType.ItemId > 0 And cboJobType.ItemId = kAcceptance Then
            chkRunExtendedRule.Visible = True
        Else
            chkRunExtendedRule.Visible = False
        End If

    End Sub

    Private Function GetJobCode(ByVal v_sJobType As String) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetJobCode"
        Try
            Dim m_vSearchData As Object



            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = g_oBusiness.GetJobCode(v_vBatch_Renewal_Job_Type:=v_sJobType, r_vResultArray:=m_vSearchData)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to call bSIRBatchRenewalJobs.GetJobCode")
            End If

            If Information.IsArray(m_vSearchData) Then
                If cboJobType.ItemId = kInvitation Then

                    txtJobCode.Text = "RI" & (CStr(gPMFunctions.ToSafeLong(CStr(m_vSearchData(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, ACArJobCode))) + 1))
                ElseIf cboJobType.ItemId = kSelection Then

                    txtJobCode.Text = "RS" & (CStr(gPMFunctions.ToSafeLong(CStr(m_vSearchData(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, ACArJobCode))) + 1))
                ElseIf cboJobType.ItemId = kAcceptance Then

                    txtJobCode.Text = "RA" & (CStr(gPMFunctions.ToSafeLong(CStr(m_vSearchData(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, ACArJobCode))) + 1))
                End If
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally





        End Try
        Return result
    End Function

    Private Sub cmdAddProduct_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddProduct.Click

        Try
            Dim vCnt, vName, vShortName, vResolvedName As Object
            Dim dDefaultDate As Date
            Dim vDateCancelled As Object
            Dim dTransactionDate As Date
            Dim iLen, iIndex As Integer


            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_lReturn = SelectParty(vPartyCnt:=vCnt, vName:=vName, vShortName:=vShortName, vResolvedName:=vResolvedName, vSpecialParty:="AG", bSuppressSubAgents:=True, vDateCancelled:=vDateCancelled)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If



        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=gPMConstants.PMErrorText, vApp:=ACApp, vClass:=ACClass, vMethod:="cmdAgentCode_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        End Try
    End Sub

    Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click
        ' Click event of the OK button.

        Try


            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If
            'Validate some  stuff double check
            m_lReturn = ValidateOK()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                cmdApply.Enabled = False
                cmdCalculate.Enabled = True
                m_iTask = gPMConstants.PMEComponentAction.PMEdit
            End If



        Catch ex As Exception

            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to call cmdApply_Click", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdApply_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
    End Sub

    Private Sub cmdCalculate_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCalculate.Click

        Try
            Dim iMsgResult As DialogResult
            Dim sMessage, sTitle As String



            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACMultiSelectCalculate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Information)

                ' Check message result.
                If iMsgResult = System.Windows.Forms.DialogResult.Yes Then
                    cmdApply_Click(cmdApply, New EventArgs())
                End If

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

                m_lReturn = GetRenewalConfigurationResults(cboJobType.ItemId)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("cmdCalculate_Click", "Failed to call bSIRBatchRenewalJobs.GetRenewalConfigurationResults")
                    Exit Sub
                End If
            End If



        Catch ex As Exception

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Calculate", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCalculate_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        End Try
    End Sub

    Private Function GetRenewalConfigurationResults(ByVal v_iBatchRenewalJobID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetRenewalConfigurationResults"
        Dim vSearchData(,) As Object
        Dim sResultMessage As New StringBuilder

        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            sResultMessage = New StringBuilder(Strings.Chr(13) & Strings.Chr(10))
            sResultMessage.Append("Checking Configuration Results " & Strings.Chr(13) & Strings.Chr(10))
            sResultMessage.Append(" Please Wait...")

            lblCalculate.TextAlign = ContentAlignment.TopCenter
            lblCalculate.Text = sResultMessage.ToString()



            m_lReturn = g_oBusiness.GetRenewalConfigurationResults(v_sBatchRenewalJobCode:=Code, v_iBatchRenewalJobID:=v_iBatchRenewalJobID, r_vResultArray:=vSearchData)

            sResultMessage = New StringBuilder("")

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to call bSIRBatchRenewalJobs.GetRenewalConfigurationResults")
            End If

            If Not Information.IsArray(vSearchData) Then 'Default Messages
                If (v_iBatchRenewalJobID = kSelection) Or (v_iBatchRenewalJobID = kInvitation) Then
                    sResultMessage = New StringBuilder(Strings.Chr(13) & Strings.Chr(10))
                    sResultMessage.Append(Strings.Chr(13) & Strings.Chr(10))
                    sResultMessage.Append(Strings.Chr(13) & Strings.Chr(10))
                    sResultMessage.Append(New String(" "c, 6) & "0 ")
                    sResultMessage.Append(New String(" "c, 1) & "policies will be selected for Renewal based on the current configuration" & Strings.Chr(13) & Strings.Chr(10))
                    sResultMessage.Append(New String(" "c, 6) & "at the " & DateTime.Now.ToString("dd MMMM yyyy") & ".")
                End If

                If v_iBatchRenewalJobID = kAcceptance Then
                    sResultMessage.Append(Strings.Chr(13) & Strings.Chr(10))
                    sResultMessage.Append(New String(" "c, 6) & "Total Policies for Acceptance: " & "0")
                End If

                lblCalculate.Text = sResultMessage.ToString()
                lblCalculate.TextAlign = ContentAlignment.TopLeft
                Return result
            End If

            If (v_iBatchRenewalJobID = kSelection) Or (v_iBatchRenewalJobID = kInvitation) Then
                sResultMessage = New StringBuilder(Strings.Chr(13) & Strings.Chr(10))
                sResultMessage.Append(Strings.Chr(13) & Strings.Chr(10))
                sResultMessage.Append(Strings.Chr(13) & Strings.Chr(10))

                sResultMessage.Append(New String(" "c, 6) & (CStr(gPMFunctions.ToSafeLong(CStr(vSearchData(0, 0)).Trim()))))
                sResultMessage.Append(New String(" "c, 1) & "policies will be selected for Renewal based on the current configuration" & Strings.Chr(13) & Strings.Chr(10))
                sResultMessage.Append(New String(" "c, 6) & "at the " & DateTime.Now.ToString("dd MMMM yyyy") & ".")
                lblCalculate.Text = sResultMessage.ToString()
            End If

            If v_iBatchRenewalJobID = kAcceptance Then


                For iCnt As Integer = vSearchData.GetLowerBound(1) To vSearchData.GetUpperBound(1)

                    Select Case gPMFunctions.ToSafeInteger(CStr(vSearchData(1, iCnt)))
                        Case kRenewalWithNoException ' Where no exception
                            sResultMessage.Append(Strings.Chr(13) & Strings.Chr(10))

                            sResultMessage.Append(New String(" "c, 6) & "Total Policies for Acceptance: " & CStr(gPMFunctions.ToSafeLong(CStr(vSearchData(0, iCnt)).Trim())))
                        Case kRenewalExceptionPOLNUM
                            sResultMessage.Append(Strings.Chr(13) & Strings.Chr(10))

                            sResultMessage.Append(New String(" "c, 6) & "Total Policies on a Product with a Policy Number Change: " & CStr(gPMFunctions.ToSafeLong(CStr(vSearchData(0, iCnt)).Trim())))
                        Case kRenewalExceptionINSTAL
                            sResultMessage.Append(Strings.Chr(13) & Strings.Chr(10))

                            sResultMessage.Append(New String(" "c, 6) & "Total Policies without a finance plan whereas the original version had a Finance Plan: " & CStr(gPMFunctions.ToSafeLong(CStr(vSearchData(0, iCnt)).Trim())))
                        Case kRenewalExceptionPERPAY
                            sResultMessage.Append(Strings.Chr(13) & Strings.Chr(10))

                            sResultMessage.Append(New String(" "c, 6) & "Total Policies required Prepayment: " & CStr(gPMFunctions.ToSafeLong(CStr(vSearchData(0, iCnt)).Trim())))
                        Case kRenewalExceptionTEMPLATE
                            sResultMessage.Append(Strings.Chr(13) & Strings.Chr(10))

                            sResultMessage.Append(New String(" "c, 6) & "Total Policies having no renewal document template configured: " & CStr(gPMFunctions.ToSafeLong(CStr(vSearchData(0, iCnt)).Trim())))
                        Case kRenewalExceptionOTHER
                            sResultMessage.Append(Strings.Chr(13) & Strings.Chr(10))

                            sResultMessage.Append(New String(" "c, 6) & "Total Policies having other exceptions: " & CStr(gPMFunctions.ToSafeLong(CStr(vSearchData(0, iCnt)).Trim())))
                    End Select
                Next
            End If

            lblCalculate.TextAlign = ContentAlignment.TopLeft
            lblCalculate.Text = sResultMessage.ToString()



        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            result = gPMConstants.PMEReturnCode.PMFalse

        Finally



            vSearchData = Nothing




        End Try
        Return result
    End Function

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        ' Click event of the Cancel button.
        Try



            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If



        Catch ex As Exception

            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Close Interface", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdCancel_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
    End Sub

    Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click
        ' Click event of the OK button.

        Try


            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Check mandatory controls have been entered into.
            m_lReturn = m_oFormFields.CheckMandatoryControls()

            ' Check for errors
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If
            'Validate some  stuff double check
            m_lReturn = ValidateOK()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Process the next set of actions depending
            ' upon the interface task etc.
            m_lReturn = m_oGeneral.ProcessCommand()

            ' Check the return value.
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                ' Everything OK, so we can hide the interface.
                Me.Hide()
            End If



        Catch ex As Exception

            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to call cmdOk_Click", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdOk_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
    End Sub

    Private Sub cmdRemoveProduct_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdRemoveProduct.Click

        Dim iRow As Integer
        Dim lAddressCnt As Integer
        Dim iMsgResult As DialogResult
        Dim sMessage, sTitle As String

        Try



            'Set row to be deleted - if a valid one selected
            If lvwAgents.Items.Count < 1 Then
                Exit Sub
            End If


            sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDeleteAgent, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)

            ' Check message result.
            If iMsgResult = System.Windows.Forms.DialogResult.Yes Then
                lvwAgents.Items.RemoveAt(m_iLine - 1)
                cmdRemoveProduct.Enabled = False
            End If



        Catch ex As Exception

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="cmdDeleteAd_ClickFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="cmdDeleteAd_Click", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally


        End Try
    End Sub

    Private Sub Form_Initialize_Renamed()

        ' Forms initialise event.

        Try



            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()

            ' Create an instance of the general interface object.
            m_oGeneral = New iPMUBatchRenewalJobs.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)

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

                ' Raise Error.
                gPMFunctions.RaiseError("Form_Initialize", "Failed to get PMUser")
                Exit Sub

            End If

            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        Catch ex As Exception

            ' Error Section

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialize interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initilize", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
    End Sub


    Private Sub frmBatchRenewalJob_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Dim Key As uctPickList.PickListKey
        ' Forms load event.

        Try



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

            ' Validate fields using Forms Control
            m_lReturn = SetFieldValidation()

            ' Set the interface default values.
            m_lReturn = SetInterfaceDefaults()

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            m_lReturn = PopulateJobTypeCombo()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            'For Product Pick List(Start)
            Key = New uctPickList.PickListKey()
            Key.KeyName = "BatchRenewalJobId"
            Key.ValueType = gPMConstants.PMEDataType.PMInteger
            uctPickListProducts.ForeignKeys.Add(Key, Key:="BatchRenewalJobId")

            uctPickListProducts.ForeignKeys.Item("BatchRenewalJobId").Value = m_iBatchRenewalJobId
            'Developer Guide No. 68
            m_lReturn = uctPickListProducts.Load_Renamed()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to load list of Products", "Batch Renewal Job Configuration", MessageBoxButtons.OK, MessageBoxIcon.Information)
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If
            'Product Pick List(End)

            'For Branch Pick List(Start)
            Key.KeyName = "BatchRenewalJobId"
            Key.ValueType = gPMConstants.PMEDataType.PMInteger
            uctPickListBranches.ForeignKeys.Add(Key, Key:="BatchRenewalJobId")

            uctPickListBranches.ForeignKeys.Item("BatchRenewalJobId").Value = m_iBatchRenewalJobId
            'Developer Guide No. 68
            m_lReturn = uctPickListBranches.Load_Renamed()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                MessageBox.Show("Failed to load list of Sources", "Batch Renewal Job Configuration", MessageBoxButtons.OK, MessageBoxIcon.Information)
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub
            End If
            'Branch Pick List(End)

            m_lReturn = DataToInterface()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the interface details.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Set the mouse pointer to normal.
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Exit Sub
            End If

            'Set Focus to GeneralTab
            SSTabHelper.SetSelectedIndex(SSBatchRenewalJobs, ACTabGeneral)

            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                lblDaysBeforeRenewalDate.Enabled = False
                txtDaysBeforeRenewalDate.Enabled = False
                txtJobCode.Text = ""
                cmdCalculate.Enabled = False
                lvwAgents.Enabled = False
                lvwAgents.BackColor = Color.Silver
                chkIncludeDirectPolicies.CheckState = CheckState.Checked
            End If
            If m_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                cboJobType.Enabled = False
                cmdCalculate.Enabled = True
            End If
            lvwAgents_Click(lvwAgents, New EventArgs())

            '    If cboJobType.ItemId > 0 Then
            '        If cboJobType.ItemId = kAcceptance Or cboJobType.ItemId = kInvitation Then
            Me.optPrint.Checked = False
            Me.optPrint.Enabled = False
            '        End If
            '    End If
            'Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            txtDescription.Focus()
            txtDescription.Select()


        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

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
            m_lReturn = DisplayCaptions()

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

            lvwAgents.Columns.Clear()
            lvwAgents.BorderStyle = BorderStyle.Fixed3D
            lvwAgents.FullRowSelect = True
            lvwAgents.View = View.Details
            lvwAgents.LabelEdit = False
            lvwAgents.HideSelection = False

            'Party Cnt

            sResValue = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPartyCnt, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwAgents.Columns.Add(sResValue, CInt(VB6.TwipsToPixelsX(0)))

            'Agent Code

            sResValue = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAgentCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwAgents.Columns.Add(sResValue, CInt(VB6.TwipsToPixelsX(1400)))

            'Agent Name

            sResValue = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAgentName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwAgents.Columns.Add(sResValue, CInt(VB6.TwipsToPixelsX(1800)))

            'Address Line 1

            sResValue = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressLine1, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwAgents.Columns.Add(sResValue, CInt(VB6.TwipsToPixelsX(2500)))

            'Address Line 2

            sResValue = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAddressLine2, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwAgents.Columns.Add(sResValue, CInt(VB6.TwipsToPixelsX(2500)))

            'PostCode

            sResValue = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACPostCode, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            lvwAgents.Columns.Add(sResValue, CInt(VB6.TwipsToPixelsX(1400)))

            uctPickListProducts.AvailableCaption = "Available"
            uctPickListBranches.AvailableCaption = "Available"



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally






        End Try
        Return result
    End Function

    Private Function ValidateOK() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "ValidateOK"
        Dim vValue As Object
        Dim sMessage As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If txtJobCode.Text.Trim() = "" Or cboJobType.ItemId = 0 Then
                MessageBox.Show("Please Select Job Type", "Batch Renewal Job Configuration", MessageBoxButtons.OK, MessageBoxIcon.Information)
                result = gPMConstants.PMEReturnCode.PMFalse
                SSTabHelper.SetSelectedIndex(SSBatchRenewalJobs, 0)
                Return result
            End If

            If txtJobCode.Text.Trim().Length > 6 Then
                MessageBox.Show("Job Code limit reached (1-9999)", "Batch Renewal Job Configuration", MessageBoxButtons.OK, MessageBoxIcon.Information)
                result = gPMConstants.PMEReturnCode.PMFalse
                SSTabHelper.SetSelectedIndex(SSBatchRenewalJobs, 0)
                Return result
            End If

            If Conversion.Val(txtDaysBeforeRenewalDate.Text) = StringsHelper.ToDoubleSafe("0") And (cboJobType.ItemId = kAcceptance Or cboJobType.ItemId = kInvitation) Then
                txtDaysBeforeRenewalDate.Text = "0"
            End If

            If Conversion.Val(txtDaysBeforeRenewalDate.Text) > 365 And (cboJobType.ItemId = kAcceptance Or cboJobType.ItemId = kInvitation) Then
                MessageBox.Show("Please Enter Days Before Renewal Date Range 0-365", "Batch Renewal Job Configuration", MessageBoxButtons.OK, MessageBoxIcon.Information)
                result = gPMConstants.PMEReturnCode.PMFalse
                SSTabHelper.SetSelectedIndex(SSBatchRenewalJobs, 0)
                Return result
            End If

            'If chkIsActive.CheckState = CheckState.Checked And txtJobCode.Text.Trim() <> "" Then
            '    m_lReturn = CheckTwoActiveConfigurations(txtJobCode.Text.Trim().Substring(0, 2))
            '    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '        'PN: 46528
            '        sMessage = "An active job already exist for Job Type - '" & cboJobType.ItemCaption & "'."
            '        MessageBox.Show(sMessage, "Batch Renewal Job Configuration", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            '        result = gPMConstants.PMEReturnCode.PMFalse
            '        SSTabHelper.SetSelectedIndex(SSBatchRenewalJobs, 0)
            '        chkIsActive.Focus()
            '        Return result
            '    End If
            'End If

            If uctPickListProducts.SelectedItems < 1 Then
                MessageBox.Show("Please select atleast one product.", "No Product Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
                result = gPMConstants.PMEReturnCode.PMFalse
                SSTabHelper.SetSelectedIndex(SSBatchRenewalJobs, 1)
                Return result
            End If

            If optSelectedAgents.Checked And lvwAgents.Items.Count < 1 Then
                MessageBox.Show("Please select atleast one agent.", "No Agent Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
                result = gPMConstants.PMEReturnCode.PMFalse
                SSTabHelper.SetSelectedIndex(SSBatchRenewalJobs, 1)
                Return result
            End If

            If uctPickListBranches.SelectedItems < 1 Then
                MessageBox.Show("Please select atleast one branch.", "No Branch Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
                result = gPMConstants.PMEReturnCode.PMFalse
                SSTabHelper.SetSelectedIndex(SSBatchRenewalJobs, 2)
                Return result
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally






        End Try
        Return result
    End Function

    Private Function PopulateJobTypeCombo() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "PopulateJobTypeCombo"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            cboJobType.TableName = "Batch_Renewal_Job_Type"
            cboJobType.WhereClause = "is_deleted<>1"
            cboJobType.RefreshList()
            cboJobType.FirstItem = "(Select Job Type)"

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally





        End Try
        Return result
    End Function

    Private Sub frmBatchRenewalJob_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        ' Forms query unload event.

        Try



            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.
            '    If (UnloadMode <> vbFormCode) Then
            '        ' Process the next set of actions depending
            '        ' upon the interface task etc.
            '        m_lReturn& = m_oGeneral.ProcessCommand()
            '
            '        ' Check the return value.
            '        If (m_lReturn& <> PMTrue) Then
            '            ' Do not procced with the interface termination.
            '            Cancel = 1
            '
            '            ' Set the mouse pointer to normal.
            '            SetMousePointer PMMouseNormal
            '
            '            GoTo Finally
            '        End If
            '    End If

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

            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to terminate the interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_QueryUnload", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

        Finally



        End Try
    End Sub

    Private Sub lvwAgents_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwAgents.Click
        If Not (lvwAgents.FocusedItem Is Nothing) And optSelectedAgents.Checked Then

            m_lAgentId = Convert.ToString(lvwAgents.FocusedItem.Tag)
            m_iLine = lvwAgents.FocusedItem.Index + 1
            cmdRemoveProduct.Enabled = True
        Else
            cmdRemoveProduct.Enabled = False
        End If
    End Sub

    Private isInitializingComponent As Boolean
    Private Sub optAllAgents_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optAllAgents.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            If optAllAgents.Checked Then
                cmdAddProduct.Enabled = False
                cmdRemoveProduct.Enabled = False
                lvwAgents.Items.Clear()
                lvwAgents.Enabled = False
                lvwAgents.BackColor = Color.Silver
            End If
        End If
    End Sub

    Private Sub optSelectedAgents_CheckedChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles optSelectedAgents.CheckedChanged
        If eventSender.Checked Then
            If isInitializingComponent Then
                Exit Sub
            End If
            If optSelectedAgents.Checked Then
                cmdAddProduct.Enabled = True
                cmdRemoveProduct.Enabled = False
                lvwAgents.Enabled = True
                lvwAgents.BackColor = SystemColors.Window
            End If
        End If
    End Sub

    Private Sub txtDaysBeforeRenewalDate_KeyPress(ByVal eventSender As Object, ByVal eventArgs As KeyPressEventArgs) Handles txtDaysBeforeRenewalDate.KeyPress
        Dim KeyAscii As Integer = Strings.Asc(eventArgs.KeyChar)
        If (KeyAscii >= 48 And KeyAscii <= 57) Or KeyAscii = 8 Then
            'Fill Days
        Else
            KeyAscii = 0
        End If
        If KeyAscii = 0 Then
            eventArgs.Handled = True
        End If
        eventArgs.KeyChar = Convert.ToChar(KeyAscii)
    End Sub

    ' ***************************************************************** '
    ' Name: SelectParty
    '
    ' Description: Call Find Party component to choose a party
    '
    ' ***************************************************************** '
    Private Function SelectParty(ByRef vPartyCnt As Object, ByRef vShortName As Object, Optional ByRef vName As Object = Nothing, Optional ByRef vSpecialParty As String = "", Optional ByRef vResolvedName As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef bSuppressSubAgents As Boolean = False, Optional ByRef vDateCancelled As Object = Nothing) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetFieldValidation"
        Try


            'Developer Guide No. 108
            Dim oFindParty As iPMBFindParty.Interface_Renamed
            Dim vKeyArray(,) As Object
            Dim lCount, lLower, lUpper As Integer
            Dim oListItem As ListViewItem
            Dim bCheckDuplicate As Boolean
            Dim lPartyCnt As Integer
            Dim iMsgResult As DialogResult
            Dim sMessage, sTitle As String

            result = gPMConstants.PMEReturnCode.PMTrue
            'Developer Guide No. 108
            oFindParty = New iPMBFindParty.Interface_Renamed()

            m_lErrorNumber = CType(CType(oFindParty, SSP.S4I.Interfaces.ILocalInterface).Initialise(), gPMConstants.PMEReturnCode)

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oFindParty.CallingAppName = ACApp


            oFindParty.BranchID = g_iSourceID

            m_lErrorNumber = CType(oFindParty.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:=m_sTransactionType, vEffectiveDate:=DateTime.Now), gPMConstants.PMEReturnCode)

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            If (Not Information.IsNothing(vSpecialParty)) And (Not String.IsNullOrEmpty(vSpecialParty)) Then

                ReDim vKeyArray(1, 0)

                vKeyArray(0, 0) = "special_party"

                vKeyArray(1, 0) = vSpecialParty

                m_lErrorNumber = CType(oFindParty.SetKeys(vKeyArray), gPMConstants.PMEReturnCode)

                If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                    oFindParty = Nothing
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If (vSpecialParty = "AG") Or (vSpecialParty = "UB") Or (vSpecialParty = "AH") Then
                    oFindParty.NotEditable = 1
                End If

                oFindParty.SuppressSubAgents = bSuppressSubAgents
            End If

            m_lErrorNumber = CType(oFindParty.Start(), gPMConstants.PMEReturnCode)

            If m_lErrorNumber <> gPMConstants.PMEReturnCode.PMTrue Then
                oFindParty.Dispose()
                oFindParty = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then

                m_lReturn = oFindParty.GetKeys(vKeyArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Information.IsArray(vKeyArray) Then
                    'Check Duplicate
                    bCheckDuplicate = False

                    lPartyCnt = gPMFunctions.ToSafeLong(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, ACArPartyCnt)))
                    For lRow As Integer = 1 To lvwAgents.Items.Count
                        If lPartyCnt = gPMFunctions.ToSafeLong(lvwAgents.Items.Item(lRow - 1).Text) Then
                            bCheckDuplicate = True
                            Exit For
                        End If
                    Next

                    If Not bCheckDuplicate Then
                        ' PartyCnt

                        oListItem = lvwAgents.Items.Add(CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, ACArPartyCnt)))
                        ' Agent Code

                        ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, ACArAgentCode)).Trim()
                        ' Agent Name

                        ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, ACArAgentName)).Trim()
                        ' Address line 1

                        ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, ACArAddressLine1)).Trim()
                        ' Address line 2

                        ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, ACArAddressLine2)).Trim()
                        ' Post Code

                        ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, ACArPostCode)).Trim()
                        oListItem.Tag = CStr(1)
                    Else


                        sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


                        sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACDuplicateMsg, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                        iMsgResult = MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Information)

                    End If
                End If

            Else
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oFindParty.Dispose()

            oFindParty = Nothing



        Catch ex As Exception

            ' Error Section.

            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: AddAssociatedSubAgent
    '
    ' Description: Adds associated sub agents to the list view if
    ' raise commission transactions against this associated agent
    ' is checked in the iPMBAssociates.
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (AddAssociatedSubAgent) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function AddAssociatedSubAgent(ByVal v_lLeadAgentCnt As Integer) As Integer
    'Dim result As Integer = 0
    'Dim bSIRFindParty As Object
    '
    'Const kMethodName As String = "AddAssociatedSubAgent"
    '

    'Dim oFindParty As bSIRFindParty.Business
    'Dim vSubAgentCnt As Integer
    'Dim oListItem As ListViewItem
    '
    'On Error GoTo Catch_Renamed
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'Get business object of the component bSIRFindParty.
    'Dim temp_oFindParty As Object
    'm_lReturn = g_oObjectManager.GetInstance(temp_oFindParty, "bSIRFindParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
    'oFindParty = temp_oFindParty
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Failed to process the interface.
    'result = gPMConstants.PMEReturnCode.PMFalse
    'iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get business object", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    'GoTo Finally_Renamed
    'End If
    '
    'If Information.IsArray(m_vGetAssociatedSubAgent) Then
    'm_vGetAssociatedSubAgent = Nothing
    'End If
    '
    ' Get associated sub agents to lead agent having "raise commission transaction
    ' against this associated agent" check box active in iPMBAssociates.

    'm_lReturn = oFindParty.GetAssociatedSubAgent(v_lLeadPartyCnt:=v_lLeadAgentCnt, r_vGetAssociatedSubAgent:=m_vGetAssociatedSubAgent)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    ' Failed to process the interface.
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    ' Log Error.
    'gPMFunctions.RaiseError(kMethodName, "Failed to get associated sub-agents", gPMConstants.PMELogLevel.PMLogError)
    'End If
    '
    ' Add Associated SubAgents to list view.
    'If Information.IsArray(m_vGetAssociatedSubAgent) Then
    'For 'lRow As Integer = m_vGetAssociatedSubAgent.GetLowerBound(1) To m_vGetAssociatedSubAgent.GetUpperBound(1)
    '
    ' add to list view if not already in the list
    'If IsInListView(gPMFunctions.ToSafeLong(CStr(m_vGetAssociatedSubAgent(kAssociatedSubAgentCnt, lRow))), lvwAgents) = gPMConstants.PMEReturnCode.PMFalse Then
    '
    ' Sub Agent short Name

    'oListItem = lvwAgents.Items.Add(gPMFunctions.ToSafeString(CStr(m_vGetAssociatedSubAgent(kAssociatedSubAgentShortName, lRow)).Trim()), "")
    '
    ' Sub Agent Name
    'If gPMFunctions.ToSafeString(CStr(m_vGetAssociatedSubAgent(kAssociatedSubAgentResolvedName, lRow))) <> "" Then
    'ListViewHelper.GetListViewSubItem(oListItem, 1).Text = gPMFunctions.ToSafeString(CStr(m_vGetAssociatedSubAgent(kAssociatedSubAgentResolvedName, lRow)))
    'Else
    'ListViewHelper.GetListViewSubItem(oListItem, 1).Text = gPMFunctions.ToSafeString(CStr(m_vGetAssociatedSubAgent(kAssociatedSubAgentName, lRow)))
    'End If
    '
    'Sub Agent party_cnt
    'oListItem.Tag = CStr(gPMFunctions.ToSafeLong(CStr(m_vGetAssociatedSubAgent(kAssociatedSubAgentCnt, lRow))))
    '
    ' refreshes the list
    'lvwAgents.Refresh()
    '
    'End If
    'Next 
    'RaiseEvent SubAgentChange()
    'End If
    '
    'GoTo Finally_Renamed
    '
    'Catch_Renamed: '
    ' DO Not Call any functions before here or the error will be lost
    'iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse)
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    'Finally_Renamed: '
    '
    ' Terminate the business object
    'If Not (oFindParty Is Nothing) Then

    'm_lReturn = oFindParty.Terminate()
    'oFindParty = Nothing
    'End If
    '
    'Return result
    'End Function

    Private Function SetAgentList(ByVal iBatchRenewalJobId As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "SetAgentList"
        Dim vAgentList As Object

        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            If lvwAgents.Items.Count > 0 And optSelectedAgents.Checked Then
                ReDim vAgentList(lvwAgents.Items.Count - 1)
                For lRow As Integer = 1 To lvwAgents.Items.Count

                    vAgentList(lRow - 1) = lvwAgents.Items.Item(lRow - 1).Text
                Next
            End If


            m_lReturn = g_oBusiness.AddAgent(iBatchRenewalJobId:=iBatchRenewalJobId, vKeys:=vAgentList)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If



        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally

            ' Terminate the business object





        End Try
        Return result
    End Function

    Private Function GetBusiness(ByVal v_vBatch_Renewal_Job_Id As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetBusiness"
        Dim vAgentArray(,) As Object
        Dim oListItem As ListViewItem


        result = gPMConstants.PMEReturnCode.PMTrue

        m_vSearchData = Nothing


        m_lReturn = g_oBusiness.GetBusiness(v_vBatch_Renewal_Job_Id:=v_vBatch_Renewal_Job_Id, r_vResultArray:=m_vSearchData, r_vResultAgentArray:=vAgentArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "Failed to call bSIRBatchRenewalJobs.GetBusiness")
        End If

        If Information.IsArray(m_vSearchData) Then
            txtJobCode.Text = CStr(m_vSearchData(ACBatchRenewalCode, 0)).Trim()
            txtDescription.Text = CStr(m_vSearchData(ACBatchRenewalDescription, 0)).Trim()
            txtSAMServer.Text = CStr(m_vSearchData(ACBatchRenewalSAMServer, 0)).Trim()
            txtDaysBeforeRenewalDate.Text = CStr(m_vSearchData(ACBatchRenewalDaysBeforeRenewalDate, 0)).Trim()
            cboJobType.ItemId = gPMFunctions.ToSafeInteger(CStr(m_vSearchData(ACBatchRenewalJobTypeId, 0)))
            If gPMFunctions.ToSafeInteger(CStr(m_vSearchData(ACBatchRenewalDocsDescription, 0))) = 0 Then
                optNotRequired.Checked = True
            ElseIf gPMFunctions.ToSafeInteger(CStr(m_vSearchData(ACBatchRenewalDocsDescription, 0))) = 2 Then
                optSpool.Checked = True
            Else
                'default print
                optPrint.Checked = True
            End If
            If gPMFunctions.ToSafeInteger(CStr(m_vSearchData(ACBatchRenewalReportSortOrder, 0))) = 2 Then
                optPolicyNumber.Checked = True
            Else
                'default client
                optClient.Checked = True
            End If
            If gPMFunctions.ToSafeInteger(CStr(m_vSearchData(ACBatchRenewalIsActive, 0))) = 1 Then
                chkIsActive.CheckState = CheckState.Checked
            Else
                'default suspend
                chkIsActive.CheckState = CheckState.Unchecked
            End If
            If gPMFunctions.ToSafeInteger(CStr(m_vSearchData(ACBatchRenewalAllAgents, 0))) = 0 Then
                optSelectedAgents.Checked = True
                lvwAgents.Enabled = True
                lvwAgents.BackColor = SystemColors.Window
                cmdAddProduct.Enabled = True
                cmdRemoveProduct.Enabled = True
            Else
                'default All Agents
                optAllAgents.Checked = True
                lvwAgents.Enabled = False
                lvwAgents.BackColor = Color.Silver
                cmdAddProduct.Enabled = False
                cmdRemoveProduct.Enabled = False
            End If
            If gPMFunctions.ToSafeInteger(CStr(m_vSearchData(ACBatchRenewalJobIncludeDirectPolicies, 0))) = 1 Then
                chkIncludeDirectPolicies.CheckState = CheckState.Checked
            Else
                chkIncludeDirectPolicies.CheckState = CheckState.Unchecked
            End If
            If gPMFunctions.ToSafeInteger(CStr(m_vSearchData(ACBatchRenewalJobRunExtendedRule, 0))) = 1 Then
                chkRunExtendedRule.CheckState = CheckState.Checked
            Else
                chkRunExtendedRule.CheckState = CheckState.Unchecked
            End If

            m_lReturn = GetRenewalConfigurationResults(cboJobType.ItemId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("cmdCalculate_Click", "Failed to call bSIRBatchRenewalJobs.GetRenewalConfigurationResults")
            End If

        End If

        If cboJobType.ItemId > 0 And cboJobType.ItemId = kSelection Then
            fraRenewalReportDocuments.Text = "Renewal Reports"
            optNotRequired.Top = VB6.TwipsToPixelsY(390)
            optNotRequired.Left = VB6.TwipsToPixelsX(3435)
            optNotRequired.BringToFront()
        ElseIf cboJobType.ItemId > 0 And (cboJobType.ItemId = kInvitation Or cboJobType.ItemId = kAcceptance) Then
            fraRenewalReportDocuments.Text = "Renewal Documents"
            optNotRequired.Top = VB6.TwipsToPixelsY(390)
            optNotRequired.Left = VB6.TwipsToPixelsX(6600)
        Else
            fraRenewalReportDocuments.Text = "Renewal Reports/Documents"
            optNotRequired.Top = VB6.TwipsToPixelsY(390)
            optNotRequired.Left = VB6.TwipsToPixelsX(6600)
        End If

        If cboJobType.ItemId > 0 And cboJobType.ItemId = kAcceptance Then
            chkRunExtendedRule.Visible = True
        Else
            chkRunExtendedRule.Visible = False
        End If

        lvwAgents.Items.Clear()
        If Information.IsArray(vAgentArray) Then

            For lRow As Integer = vAgentArray.GetLowerBound(1) To vAgentArray.GetUpperBound(1)
                ' PartyCnt

                oListItem = lvwAgents.Items.Add(CStr(vAgentArray(ACArPartyCnt, lRow)))
                ' Agent Code

                ListViewHelper.GetListViewSubItem(oListItem, 1).Text = CStr(vAgentArray(ACArAgentCode, lRow)).Trim()
                ' Agent Name

                ListViewHelper.GetListViewSubItem(oListItem, 2).Text = CStr(vAgentArray(ACArAgentName, lRow)).Trim()
                ' Address line 1

                ListViewHelper.GetListViewSubItem(oListItem, 3).Text = CStr(vAgentArray(ACArAgAddressLine1, lRow)).Trim()
                ' Address line 2

                ListViewHelper.GetListViewSubItem(oListItem, 4).Text = CStr(vAgentArray(ACArAgAddressLine2, lRow)).Trim()
                ' Post Code

                ListViewHelper.GetListViewSubItem(oListItem, 5).Text = CStr(vAgentArray(ACArAgPostCode, lRow)).Trim()
                oListItem.Tag = CStr(1)
            Next lRow
        End If

        GoTo Finally_Renamed



        ' Log Error.
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value)

Finally_Renamed:
        Return result
        Resume
        Return result
    End Function

    Private Function CheckTwoActiveConfigurations(ByVal v_sBatchRenewalJobCode As String) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "CheckTwoActiveConfigurations"
        Dim vActiveStatusArray(,) As Object
        Dim iJobId As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = g_oBusiness.CheckTwoActiveConfigurations(v_sBatchRenewalJobCode:=v_sBatchRenewalJobCode, r_vResultArray:=vActiveStatusArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to call bSIRBatchRenewalJobs.CheckTwoActiveConfigurations")
            End If

            If Information.IsArray(vActiveStatusArray) Then 'Active state already exists

                iJobId = gPMFunctions.ToSafeInteger(CStr(vActiveStatusArray(0, 0)))
                If iJobId <> m_iBatchRenewalJobId Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If



        Catch ex As Exception

            ' Log Error.
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=g_sUsername.Value, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    Private Sub frmBatchRenewalJob_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        'Developer Guide No 293
        If e.Alt And e.KeyCode = Keys.D1 Then
            SSBatchRenewalJobs.SelectedIndex = 0
        End If
        If e.Alt And e.KeyCode = Keys.D2 Then
            SSBatchRenewalJobs.SelectedIndex = 1
        End If
        If e.Alt And e.KeyCode = Keys.D3 Then
            SSBatchRenewalJobs.SelectedIndex = 2
        End If
    End Sub
End Class
