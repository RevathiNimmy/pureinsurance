Option Strict Off
Option Explicit On

Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmCloneRIBatchProcess
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmCloneRIBatchProcess
    '
    ' Date: 14/03/2011
    '
    ' Description:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmCloneRIBatchProcess"

    ' Object parameter members.
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    ' ClonedRIPolicyField
    Private Enum CloneRIBatchProcess
        edInsuranceFileCnt = 0
        edInsuranceRef = 1
        edClientCode = 4
        edClientName = 5
    End Enum

    ' ClonedRIClaimsField
    Private Enum CloneClaimRIBatchProcess
        edCLMInsuranceRef = 0
        edCLMShortCode = 1
        edCLMClientName = 2
        edCLMClaimClonedRIUsageID = 3
        edCLMOldInsuranceFileCnt = 4
        edCLMNewInsuranceFileCnt = 5
        edCLMOldRiskCnt = 6
        edCLMNewRiskCnt = 7
        edCLMStatus = 8
        edCLMClaimId = 9
    End Enum

    'Private m_vDefRIRiskData As Variant
    Private m_vClonedRIPoliciesData(,) As Object
    Private m_vClonedRIClaimsData(,) As Object

    ' Stores the return value for the a
    ' function call.
    Private m_lReturn As Integer
    Private m_lItemsFound As Integer
    Private m_lClaimItemsFound As Integer

    Private m_lProgressValue As Integer
    Private m_sStatusBarText As String
    Private m_oBusiness As Object
    Private m_bIsRI2007Enabled As Boolean


    Public Property Status() As Integer
        Get

            ' Return the interface exit status.
            Status = m_lStatus

        End Get
        Set(ByVal Value As Integer)

            ' set the interface exit status.
            m_lStatus = Value

        End Set
    End Property

    Private Sub chkClaims_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkClaims.CheckStateChanged
        EnableDisableStart()
    End Sub

    Private Sub chkPolicies_CheckStateChanged(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles chkPolicies.CheckStateChanged
        EnableDisableStart()
    End Sub

    Private Sub cmdCancel_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdCancel.Click

        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        Me.Hide()

    End Sub

    Private Sub EnableDisableStart()

        If chkPolicies.CheckState = System.Windows.Forms.CheckState.Unchecked And chkClaims.CheckState = System.Windows.Forms.CheckState.Unchecked Then
            cmdStart.Enabled = False
        ElseIf chkPolicies.CheckState = System.Windows.Forms.CheckState.Checked And m_lItemsFound > 0 Then
            cmdStart.Enabled = True
        ElseIf chkClaims.CheckState = System.Windows.Forms.CheckState.Checked And m_lClaimItemsFound > 0 Then
            cmdStart.Enabled = True
        End If

    End Sub

    Private Sub cmdStart_Click(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles cmdStart.Click
        Const kMethodName As String = "cmdStart_Click"
        Dim result As Integer

        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            If chkPolicies.CheckState = System.Windows.Forms.CheckState.Checked And m_lItemsFound > 0 Then
                m_lReturn = ProcessClonedRIPolicies()
            End If

            If chkClaims.CheckState = System.Windows.Forms.CheckState.Checked And m_lClaimItemsFound > 0 Then
                m_lReturn = ProcessClonedRIClaims()
            End If

            ' if an error's occurred, it should have been handled already

            ' set up the interface again
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            cmdStart.Enabled = False
            cmdCancel.Text = "&Close"
            cmdCancel.Enabled = True

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=excep)

        End Try

    End Sub

    Private Sub Form_Initialize_Renamed()
        Const kMethodName As String = "Form_Initialize"

        Dim sMessage As String
        Dim sTitle As String
        Dim vValue As Object

        Try

            ShowFormInTaskBar_Attach()

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Create business  object
            m_lReturn = g_oObjectManager.GetInstance(oObject:=m_oBusiness, sClassName:="bSIRCloneRIBatchProcess.Business", vInstanceManager:=PMGetViaClientManager)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Display error stating the problem.
                sTitle = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString)
                sMessage = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString)
                MsgBox(sMessage, MsgBoxStyle.Critical, sTitle)

                Exit Sub
            End If

            ' Check the product option
            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, v_vBranch:=g_iSourceID, r_vUnderwriting:=vValue)

            If vValue = "1" Then
                m_bIsRI2007Enabled = True
            Else
                m_bIsRI2007Enabled = False
            End If

            m_oBusiness.RI2007Enabled = m_bIsRI2007Enabled

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            Exit Sub


        Catch ex As System.Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)
        End Try

    End Sub

    Private Sub frmCloneRIBatchProcess_Load(ByVal eventSender As System.Object, ByVal eventArgs As System.EventArgs) Handles MyBase.Load

        Const kMethodName As String = "Form_Load"
        Try


            ShowFormInTaskBar_Detach()

            m_lReturn = SetInterfaceDefaults()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="An error has occurred setting interface defaults", vApp:=ACApp, vClass:=ACClass, vMethod:="form_load", vErrNo:=Err.Number, vErrDesc:=Err.Description)
            End If


            m_lReturn = ShowClonedRIPolicies()

        Catch ex As System.Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: ShowClonedRIPolicies
    '
    ' Description:
    ' ***************************************************************** '
    Private Function ShowClonedRIPolicies() As Integer
        Const kMethodName As String = "ShowClonedRIPolicies"
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Display a searching message.
            DisplayStatusSearching()

            m_lReturn = GetBusiness()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="An error has occurred whilst obtaining the Cloned reinsurance policy details", vApp:=ACApp, vClass:=ACClass, vMethod:="ShowClonedRIPolicies", vErrNo:=Err.Number, vErrDesc:=Err.Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Function

            End If

            'EnableDisableButtons
            EnableDisableStart()

            'Display a searching message.
            DisplayStatusFound()

            'Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            ' Check for errors
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Failed to get details.
                ShowClonedRIPolicies = gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch ex As System.Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=ShowClonedRIPolicies, excep:=ex)
            Return result
        End Try
        Return result
    End Function

    Public Function ProcessClonedRIPolicies() As Integer
        Const kMethodName As String = "ProcessClonedRIPolicies"

        Dim lLoopy As Integer
        Dim lCurrInsFileCnt As Integer
        Dim lFailureCount As Integer

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'disable the buttons
            cmdStart.Enabled = False
            cmdCancel.Enabled = False

            lFailureCount = 0

            For lLoopy = 0 To (m_lItemsFound - 1)

                ' update the interface
                txtPolicyNumber.Text = Trim(NullToString(m_vClonedRIPoliciesData(CloneRIBatchProcess.edInsuranceRef, lLoopy)))
                txtClientCode.Text = Trim(NullToString(m_vClonedRIPoliciesData(CloneRIBatchProcess.edClientCode, lLoopy)))
                txtClientName.Text = Trim(NullToString(m_vClonedRIPoliciesData(CloneRIBatchProcess.edClientName, lLoopy)))

                lCurrInsFileCnt = NullToLong(m_vClonedRIPoliciesData(CloneRIBatchProcess.edInsuranceFileCnt, lLoopy))

                _stbStatus_Panel1.Text = "processing Policy..."

                m_lReturn = m_oBusiness.ProcessSingleClonedRIPolicy(v_lInsuranceFileCnt:=lCurrInsFileCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    m_lReturn = m_oBusiness.InsertInsFileClonedRIUsage(v_lInsFileCnt:=lCurrInsFileCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "RI Policy Clone Failed", gPMConstants.PMELogLevel.PMLogError)
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        ProcessClonedRIPolicies = gPMConstants.PMEReturnCode.PMFalse
                        Exit Function
                    End If

                    lFailureCount = lFailureCount + 1

                End If

            Next lLoopy

            m_lReturn = GetBusiness()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Get RI Cloned Policy Failed", gPMConstants.PMELogLevel.PMLogError)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Function
            End If

            ' we're all done, but let the user know how many risks need to be done manually, if any
            If lFailureCount > 0 Then
                _stbStatus_Panel1.Text = "Manual processing of " & lFailureCount & " Policies required."

                MsgBox("Processing of risks with Cloned Reinsurance is complete." & vbCrLf & vbCrLf & "To manually process the " & lFailureCount & " Policies that could not be done automatically, " & vbCrLf & "run the 'Cloned Reinsurance Amendment' task.", MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, "Processing Complete")
            Else

                _stbStatus_Panel1.Text = "Processing complete."

                MsgBox("Processing of risks with cloned reinsurance is complete.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "Processing Complete")
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            ProcessClonedRIPolicies = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As System.Exception
            ProcessClonedRIPolicies = gPMConstants.PMEReturnCode.PMError
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=ProcessClonedRIPolicies, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        End Try

    End Function

    Public Function ProcessClonedRIClaims() As Integer
        Const kMethodName As String = "ProcessClonedRIClaims"

        Dim lLoopy As Integer
        Dim lNewInsFileCnt As Integer
        Dim lNewRiskCnt As Integer
        Dim lOldInsFileCnt As Integer
        Dim lOldRiskCnt As Integer
        Dim lBaseClaimId As Integer

        Try
            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            'disable the buttons
            cmdStart.Enabled = False
            cmdCancel.Enabled = False

            For lLoopy = 0 To (m_lClaimItemsFound - 1)

                ' update the interface
                txtPolicyNumber.Text = Trim(NullToString(m_vClonedRIClaimsData(CloneClaimRIBatchProcess.edCLMInsuranceRef, lLoopy)))
                txtClientCode.Text = Trim(NullToString(m_vClonedRIClaimsData(CloneClaimRIBatchProcess.edCLMShortCode, lLoopy)))
                txtClientName.Text = Trim(NullToString(m_vClonedRIClaimsData(CloneClaimRIBatchProcess.edCLMClientName, lLoopy)))

                lNewInsFileCnt = NullToLong(m_vClonedRIClaimsData(CloneClaimRIBatchProcess.edCLMNewInsuranceFileCnt, lLoopy))
                lNewRiskCnt = NullToLong(m_vClonedRIClaimsData(CloneClaimRIBatchProcess.edCLMNewRiskCnt, lLoopy))
                lOldInsFileCnt = NullToLong(m_vClonedRIClaimsData(CloneClaimRIBatchProcess.edCLMOldInsuranceFileCnt, lLoopy))
                lOldRiskCnt = NullToLong(m_vClonedRIClaimsData(CloneClaimRIBatchProcess.edCLMOldRiskCnt, lLoopy))
                lBaseClaimId = NullToLong(m_vClonedRIClaimsData(CloneClaimRIBatchProcess.edCLMClaimId, lLoopy))
                _stbStatus_Panel1.Text = "processing Claim..."

                m_lReturn = m_oBusiness.ProcessSingleClonedRIClaim(v_lNewInsuranceFileCnt:=lNewInsFileCnt, v_lNewRiskCnt:=lNewRiskCnt, v_lOldInsuranceFileCnt:=lOldInsFileCnt, v_lOldRiskCnt:=lOldRiskCnt, v_bIsRI2007Enabled:=m_bIsRI2007Enabled, v_lBaseClaimId:=lBaseClaimId)
                ' Do error handling

            Next lLoopy

            m_lReturn = GetBusiness()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="An error has occurred whilst obtaining the deferred reinsurance policy details", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessClonedRIClaims", vErrNo:=Err.Number, vErrDesc:=Err.Description)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Function
            End If

            ' we're all done, but let the user know how many risks need to be done manually, if any
            If m_lItemsFound Then

            Else

                _stbStatus_Panel1.Text = "Processing complete."

                MsgBox("Processing of claims with cloned reinsurance is complete.", MsgBoxStyle.OkOnly + MsgBoxStyle.Information, "Processing Complete")
            End If

            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            ProcessClonedRIClaims = gPMConstants.PMEReturnCode.PMTrue
        Catch ex As System.Exception
            ProcessClonedRIClaims = gPMConstants.PMEReturnCode.PMError
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=ProcessClonedRIClaims, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:GetBusiness
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function GetBusiness() As Object
        Const kMethodName As String = "GetBusiness"

        Try

            GetBusiness = gPMConstants.PMEReturnCode.PMTrue

            ' For Policy
            m_lReturn = m_oBusiness.GetClonedRIPolicies(r_vClonedRIPoliciesArray:=m_vClonedRIPoliciesData)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                GetBusiness = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' update the module level variable that holds the number of risks we're dealing with
            If IsArray(m_vClonedRIPoliciesData) Then
                m_lItemsFound = UBound(m_vClonedRIPoliciesData, 2) + 1
            Else
                m_lItemsFound = 0
            End If

            ' Do for claim as well
            m_lReturn = m_oBusiness.GetClonedRIClaims(r_vClonedRIClaimsArray:=m_vClonedRIClaimsData)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                GetBusiness = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If IsArray(m_vClonedRIClaimsData) Then
                m_lClaimItemsFound = UBound(m_vClonedRIClaimsData, 2) + 1
            Else
                m_lClaimItemsFound = 0
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally


        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetInterfaceDefaults
    '
    ' Description: Sets all of the interface default values.
    '
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer
        Const kMethodName As String = "SetInterfaceDefaults"
        Try

            SetInterfaceDefaults = gPMConstants.PMEReturnCode.PMTrue

            txtPolicyNumber.Text = ""
            txtClientCode.Text = ""
            txtClientName.Text = ""

            ' disable the start button till there's something to do
            cmdStart.Enabled = False

        Catch ex As System.Exception
            SetInterfaceDefaults = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=SetInterfaceDefaults, excep:=ex)

        End Try

    End Function
    ' ***************************************************************** '
    ' Name: DisplayStatusSearching
    '
    ' Description: Display the status searching message.
    '
    ' ***************************************************************** '
    Private Sub DisplayStatusSearching()
        Const kMethodName As String = "DisplayStatusSearching"
        Static sMessage As String
        Dim lReturn As Integer

        Try


            ' Get message text if not already present.
            If (sMessage = "") Then
                sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusSearching, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Display the status message.
            _stbStatus_Panel1.Text = sMessage

        Catch ex As System.Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name: DisplayStatusFound
    '
    ' Description: Display the status found message.
    '
    ' ***************************************************************** '
    Private Sub DisplayStatusFound()
        Const kMethodName As String = "DisplayStatusFound"
        Static sMessagePolicy As String
        Static sMessageClaim As String
        Dim lReturn As Integer

        Try

            ' Get message text if not already present.
            If (sMessagePolicy = "") Then
                sMessagePolicy = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFound, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If
            If (sMessageClaim = "") Then
                sMessageClaim = CStr(iPMFunc.GetResData(g_iLanguageID, ACStatusFoundClaim, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If
            If sMessageClaim = "" Then
                sMessageClaim = " Claims found"
            ElseIf sMessagePolicy = "" Then
                sMessagePolicy = " Policies found"
            End If
            ' Display the status message.
            _stbStatus_Panel1.Text = m_lItemsFound & " " & sMessagePolicy & " and " & m_lClaimItemsFound & " " & sMessageClaim

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)

        Finally


        End Try
    End Sub

    Private Sub frmCloneRIBatchProcess_FormClosed(ByVal eventSender As System.Object, ByVal eventArgs As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed

        If Not (m_oBusiness Is Nothing) Then

            ' Terminate the business object
            m_oBusiness.Dispose()
            ' Destroy the instance of the renewals business object
            ' from memory.
            m_oBusiness = Nothing

        End If

    End Sub

    ' ***************************************************************** '
    '
    ' Name: DisplayMessage
    '
    ' Description: Displays message to report failure of child process.
    '
    ' ***************************************************************** '
    Private Function DisplayMessage(ByVal v_sComponentName As String) As Integer
        Const kMethodName As String = "DisplayMessage"
        Dim sTitle As String
        Dim sMessage As String
        Dim lReturn As Integer


        Try

            DisplayMessage = gPMConstants.PMEReturnCode.PMTrue

            ' Get description from the resource file.
            sTitle = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAmendTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString)

            sMessage = iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACAmendProcessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString)

            ' Display message.
            MsgBox(sMessage & v_sComponentName, MsgBoxStyle.OkOnly + MsgBoxStyle.Exclamation, sTitle)


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=DisplayMessage, excep:=ex)

        Finally



        End Try
    End Function


End Class