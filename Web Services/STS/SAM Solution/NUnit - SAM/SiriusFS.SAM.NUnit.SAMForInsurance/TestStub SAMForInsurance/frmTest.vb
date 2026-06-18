Public Class frmTest

#Region "Private Variables"

    Private m_sProcessName As String
    Private m_dtProcessStarted As Date

#End Region

#Region "Private Methods"

    Private Sub BeginProcess(ByVal ctl As Control)

        m_sProcessName = ctl.Parent.Text & " " & ctl.Text
        m_dtProcessStarted = Date.Now

        tsslAction.Text = "Test " & m_sProcessName & " started..."
        tsslAction.Invalidate()

        Cursor = Cursors.WaitCursor

    End Sub

    Private Sub EndProcess()

        Cursor = Cursors.Default

        tsslAction.Text = "Test " & m_sProcessName & " finished (" & (Date.Now - m_dtProcessStarted).ToString() & ")"

    End Sub

#End Region

#Region "Event Handlers"

    Private Sub cmdAA_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAA_Success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.AddAddress
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmd_APSuccessPC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_APSuccessPC.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.AddParty
            o.Success_PC()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdAP_InvalidBranchCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAP_InvalidBranchCode.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.AddParty
            o.InvalidData_BranchCode()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdAP_NoCorrAddr_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAP_NoCorrAddr.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.AddParty
            o.STSBusiness_NoCorrespondenceAddr()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdAQ_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAQ_Success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.AddQuote
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdAR_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAR_Success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.AddRisk
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdAR_InvalidDM_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAR_InvalidDM.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.AddRisk
            o.InvalidData_DataModelCode()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdUR_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUR_Success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.UpdateRisk
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdUR_MissingBranch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUR_MissingBranch.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.UpdateRisk
            o.InvalidData_Missing_BranchCode()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdCP_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCP_Success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.ChangePassword
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGA_Success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetAddress
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdFP_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFP_Success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.FindParty
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdGAPV_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGAPV_Success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetAllPolicyVersions
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdGtPty_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGtPty_Success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetParty
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdGtPty_MsngParty_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGtPty_MsngParty.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetParty
            o.InvalidData_Missing_PartyKey()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdGtPty_InvBranch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGtPty_InvBranch.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetParty
            o.InvalidData_BranchCode()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetPartySummary
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdGtRsk_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGtRsk_Success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetRisk
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdRDRA_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRDRA_Success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.RunDefaultRulesAdd
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdUQ_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUQ_Success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.UpdateQuote
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdUPty_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUPty_Success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.UpdateParty
            o.Success_PC()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdUPty_SuccessCC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUPty_SuccessCC.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.UpdateParty
            o.Success_CC()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdUPty_InvSubBranch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUPty_InvSubBranch.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.UpdateParty
            o.InvalidData_SubBranchCode()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdGD_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGD_Success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GenerateDocument
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub btmBindQuote_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btmBindQuote_Success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.BindQuote
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdGDSS_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGDSS_Success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetDatasetSchema
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdGRBP_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGRBP_Success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetRiskByProduct
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGPBA_Success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetProductByAgent
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmd_DRSuccess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_DRSuccess.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.DeleteRisk
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdAA_BOFailed_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAA_BOFailed.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.AddAddress
            o.STSRule_BOFailed()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetHeaderAndSummariesByKey
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdGL_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGL_Success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetList
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdDR_InvalidInsFolderCnt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDR_InvalidInsFolderCnt.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.DeleteRisk
            o.STSBusiness_InsFileCnt()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdAP_InvalidAgentKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAP_InvalidAgentKey.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.AddParty
            o.STSBusiness_DOBInFuture()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmd_APSuccessCC_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmd_APSuccessCC.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.AddParty
            o.Success_CC()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdGRD_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGRD_Success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetRatingDetails
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdGDSD_success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGDSD_success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetDatasetDefinition
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdGCBB_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGCBB_Success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetCurrenciesByBranch
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub btnForgottenPasswordSuccess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnForgottenPasswordSuccess.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.ForgottenPassword
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub btnForgottenPasswordInvalidBranch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnForgottenPasswordInvalidBranch.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.ForgottenPassword
            o.InvalidData_BranchCode()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub btnForgottenPasswordMissingUserName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnForgottenPasswordMissingUserName.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.ForgottenPassword
            o.InvalidData_Missing_UserName()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub btnForgottenPasswordMissingBranch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnForgottenPasswordMissingBranch.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.ForgottenPassword
            o.InvalidData_Missing_BranchCode()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdFC_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFC_Success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.FindClaim
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdFC_InvalidLossDateFrom_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFC_InvalidLossDateFrom.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.FindClaim
            o.InvalidData_LossDateFrom()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdFC_InvalidLossDateTo_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFC_InvalidLossDateTo.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.FindClaim
            o.InvalidData_LossDateTo()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdFC_InvalidLossDateDiff_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdFC_InvalidLossDateDiff.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.FindClaim
            o.STSBusinessRule_InvalidLossDateDifference()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub btnOpenClaimSuccess_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnOpenClaimSuccess.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.OpenClaim
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    'Private Sub cmdClaimMTA_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClaimMTA_Success.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.ClaimMTA
    '    o.Success()
    'End Sub

    'Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimreceiptTaxes
    '    o.Success()
    'End Sub

    'Private Sub btnmissingBranchCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnmissingBranchCode.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.Claimreceipt
    '    o.Missing_BranchCode()
    'End Sub

    'Private Sub btnmissingBaseClaimKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnmissingBaseClaimKey.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.Claimreceipt
    '    o.Missing_ClaimKey()
    'End Sub

    'Private Sub btnmissingBaseClaimPeril_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnmissingBaseClaimPeril.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.Claimreceipt
    '    o.Missing_ClaimPerilKey()
    'End Sub

    'Private Sub btnInvalidTaxGroupCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInvalidTaxGroupCode.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.Claimreceipt
    '    o.Success()
    'End Sub

    'Private Sub btnInvalidBranchCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInvalidBranchCode.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.Claimreceipt
    '    o.InvalidData_BranchCode()
    'End Sub

    'Private Sub btnInvalidPercentage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInvalidPercentage.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.Claimreceipt
    '    o.InvalidData_Percentages()
    'End Sub

    'Private Sub btnInvalidPartyKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInvalidPartyKey.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.Claimreceipt
    '    o.InvalidData_partyKey()
    'End Sub

    'Private Sub btnAllNonMandatoryFieldsMissing_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAllNonMandatoryFieldsMissing.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.Claimreceipt
    '    o.Missing_AllNonMandatoryFields()
    'End Sub

    'Private Sub btnMissingCurrencyCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMissingCurrencyCode.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.Claimreceipt
    '    o.Missing_CurrencyCode()
    'End Sub

    'Private Sub btnMissingpartyKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMissingpartyKey.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.Claimreceipt
    '    o.Missing_PartyKey()
    'End Sub

    'Private Sub btnNoRecovery_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNoRecovery.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.Claimreceipt
    '    o.MissingData_NoRecovery()
    'End Sub

    'Private Sub btnInvalidBaseClaimKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInvalidBaseClaimKey.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.Claimreceipt
    '    o.InvalidData_BaseClaimKey()
    'End Sub

    'Private Sub btnInvalidBaseClaimPerilKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInvalidBaseClaimPerilKey.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.Claimreceipt
    '    o.InvalidData_BaseClaimPerilKey()
    'End Sub

    'Private Sub btnInvalidBaseRecoveryKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInvalidBaseReserveKey.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.Claimreceipt
    '    o.InvalidData_BaseRecoveryKey()
    'End Sub

    'Private Sub btnInvalidCurrencyCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.Claimreceipt
    '    o.InvalidData_CurrencyCode()
    'End Sub

    'Private Sub btnBranchCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBranchCode.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimreceiptTaxes
    '    o.Missing_BranchCode()
    'End Sub

    'Private Sub btnIPercentage_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIPercentage.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimreceiptTaxes
    '    o.InvalidData_Percentages()
    'End Sub

    'Private Sub btnIpartyKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIpartyKey.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimreceiptTaxes
    '    o.InvalidData_partyKey()
    'End Sub

    'Private Sub btnIcurrencyCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIcurrencyCode.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimreceiptTaxes
    '    o.InvalidData_CurrencyCode()
    'End Sub

    'Private Sub btnIbranchCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIbranchCode.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimreceiptTaxes
    '    o.InvalidData_BranchCode()
    'End Sub

    'Private Sub btnIbaseRecoveryKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIbaseRecoveryKey.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimreceiptTaxes
    '    o.InvalidData_BaseRecoveryKey()
    'End Sub


    'Private Sub btnIbaseclaimPerilKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIbaseclaimPerilKey.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimreceiptTaxes
    '    o.InvalidData_BaseClaimPerilKey()
    'End Sub

    'Private Sub btnIbaseClaimKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnIbaseClaimKey.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimreceiptTaxes
    '    o.InvalidData_BaseClaimKey()
    'End Sub

    'Private Sub btnNoRec_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNoRec.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimreceiptTaxes
    '    o.MissingData_NoRecovery()
    'End Sub

    'Private Sub btnPartyKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPartyKey.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimreceiptTaxes
    '    o.Missing_PartyKey()
    'End Sub

    'Private Sub btnCurrencyCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCurrencyCode.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimreceiptTaxes
    '    o.Missing_CurrencyCode()
    'End Sub

    'Private Sub btnAllNonMendatoryField_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAllNonMendatoryField.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimreceiptTaxes
    '    o.Missing_AllNonMandatoryFields()
    'End Sub

    'Private Sub btnBaseRecoveryKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBaseRecoveryKey.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimreceiptTaxes
    '    o.InvalidData_BaseRecoveryKey()
    'End Sub

    'Private Sub btnMissingBaseRecoveryKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMissingBaseRecoveryKey.Click

    'End Sub

    'Private Sub btnTaxGroupCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTaxGroupCode.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimreceiptTaxes
    '    o.Massing_TaxGroupCode()
    'End Sub

    'Private Sub btnbaseClaimPeril_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnbaseClaimPeril.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimreceiptTaxes
    '    o.Missing_ClaimPerilKey()
    'End Sub

    'Private Sub btnBaseclaimKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBaseclaimKey.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimreceiptTaxes
    '    o.Missing_ClaimKey()
    'End Sub

    'Private Sub btnMissingBranch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMissingBranch.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GenerateClaimsDocuments
    '    o.MissingData_BranchCode()
    'End Sub

    'Private Sub btnMissingClaimKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMissingClaimKey.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GenerateClaimsDocuments
    '    o.MissingData_ClaimKey()
    'End Sub

    'Private Sub btnMissingTransactionType_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMissingTransactionType.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GenerateClaimsDocuments
    '    o.MissingData_TransactionType()
    'End Sub

    'Private Sub btnInvalidBranch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInvalidBranch.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GenerateClaimsDocuments
    '    o.InvalidData_BranchCode()
    'End Sub

    'Private Sub btnInvalidClaimKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInvalidClaimKey.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GenerateClaimsDocuments
    '    o.InvalidData_ClaimKey()
    'End Sub

    'Private Sub btnInvalidTransactionType_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnInvalidTransactionType.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GenerateClaimsDocuments
    '    o.InvalidData_TransactionType()
    'End Sub

    'Private Sub btnGenerateClaimDocument_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGenerateClaimDocument.Click
    '    Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GenerateClaimsDocuments
    '    o.Success()
    'End Sub

    '-------------- FOR GET CLAIM DETAILS PROCESS -----------------------------
    ' SUCCESS CASE
    Private Sub cmdgetClaimDet_success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdgetClaimDet_success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimDetails
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdGetClaimsMissingBranch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGetClaimsMissingBranch.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimDetails
            o.Missing_BranchCode()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdgetClaimMissingClaimId_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdgetClaimMissingClaimId.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimDetails
            o.Missing_ClaimKey()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    '-------------- END OF GET CLAIM DETAILS PROCESS -----------------------------


    '-------------- FOR PAY CLAIM PROCESS -----------------------------
    ' SUCCESS CASE
    Private Sub cmdPayClaimSuccess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPayClaimSuccess.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.PayClaim
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdPayClaimInvalidBranchCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.PayClaim
            o.InvalidData_BranchCode()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdPayClaimMissingBranchCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.PayClaim
            o.Missing_BranchCode()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdPayClaimMissingBaseClaimKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.PayClaim
            o.Missing_ClaimKey()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdPayClaimMissingBaseClaimPerilKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.PayClaim
            o.Missing_ClaimPerilKey()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdPayClaimMissingPartyKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.PayClaim
            o.Missing_PartyKey()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdPayClaimMissingCurrencyCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.PayClaim
            o.Missing_CurrencyCode()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdPayClaimMissingTaxGroupCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.PayClaim
            o.Missing_TaxGroupCode()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    ' TODO: The button is missing but the unit test still exists?
    'Private Sub cmdPayClaimMissingReserves_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPayClaimMissingReserves.Click
    '    Try
    '        BeginProcess(DirectCast(sender, Control))
    '        Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.PayClaim
    '        o.MissingData_NoReserves()
    '    Catch ex As Exception
    '        Handler.InterfaceLayerTop(ex)
    '    Finally
    '        EndProcess()
    '    End Try
    'End Sub

    Private Sub cmdPayClaimInvalidClaimKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.PayClaim
            o.InvalidData_BaseClaimKey()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdPayClaimInvalidClaimPerilKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.PayClaim
            o.InvalidData_BaseClaimPerilKey()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdPayClaimInvalidPartyKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.PayClaim
            o.InvalidData_PartyKey()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdPayClaimInvalidCurrencyCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.PayClaim
            o.InvalidData_BranchCode()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdPayClaimInvalidTaxGroupCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.PayClaim
            o.InvalidData_CurrencyCode()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdPayClaimInvalidPercentages_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.PayClaim
            o.InvalidData_Percentages()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub
    '-------------- END OF PAY CLAIM PROCESS -----------------------------

    '-------------- FOR GET PAYMENT TAXES PROCESS -----------------------------
    ' SUCCESS CASE
    Private Sub cmdPaymentTaxesSuccess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPaymentTaxesSuccess.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimPaymentTaxes
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdPaymentTaxesInvalidBranchCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimPaymentTaxes
            o.InvalidData_BranchCode()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdPaymentTaxesMissingBranchCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimPaymentTaxes
            o.Missing_BranchCode()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdPaymentTaxesMissingBaseClaimKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimPaymentTaxes
            o.Missing_ClaimKey()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdPaymentTaxesMissingBaseClaimPerilKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimPaymentTaxes
            o.Missing_ClaimPerilKey()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdPaymentTaxesMissingPartyKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimPaymentTaxes
            o.Missing_PartyKey()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdPaymentTaxesMissingCurrencyCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimPaymentTaxes
            o.Missing_CurrencyCode()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    ' TODO: The button is missing but the unit test still exists?
    'Private Sub cmdPaymentTaxesMissingReserves_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdPaymentTaxesMissingReserves.Click
    '    Try
    '        BeginProcess(DirectCast(sender, Control))
    '        Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimPaymentTaxes
    '        o.MissingData_NoReserves()
    '    Catch ex As Exception
    '        Handler.InterfaceLayerTop(ex)
    '    Finally
    '        EndProcess()
    '    End Try
    'End Sub

    Private Sub cmdPaymentTaxesInvalidClaimKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimPaymentTaxes
            o.InvalidData_BaseClaimKey()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdPaymentTaxesInvalidClaimPerilKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimPaymentTaxes
            o.InvalidData_BaseClaimPerilKey()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdPaymentTaxesInvalidPartyKey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimPaymentTaxes
            o.InvalidData_PartyKey()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdPaymentTaxesInvalidCurrencyCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimPaymentTaxes
            o.InvalidData_BranchCode()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdPaymentTaxesInvalidTaxGroupCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimPaymentTaxes
            o.InvalidData_CurrencyCode()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdPaymentTaxesInvalidPercentages_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimPaymentTaxes
            o.InvalidData_Percentages()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub
    ''-------------- END OF GET PAYMENT TAXES PROCESS -----------------------------
    '' SUCCESS CASE
    'Private Sub cmdClaimMTA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdClaimMTA.Click
    '    Try
    '        BeginProcess(DirectCast(sender, Control))
    '        Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.ClaimMTA
    '        o.Success()
    '    Catch ex As Exception
    '        Handler.InterfaceLayerTop(ex)
    '    Finally
    '        EndProcess()
    '    End Try
    'End Sub

    '-------------- FOR UPDATE PARTY OTHER PROCESS -----------------------------
    ' SUCCESS CASE
    Private Sub cmdUpdatepartySuccess_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUpdatepartySuccess.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.UpdateParty
            o.Success_Other()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub


    Private Sub cmdUpdatePartyAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.UpdateParty
            o.AddConvictionAccident()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdUpdatePartyMissingNonMandatory_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.UpdateParty
            o.AllMandatoryData()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdUpdatePartyAddUpdate_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.UpdateParty
            o.UpdateConvictionAccident()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdUpdatePartyAddressNoContacts_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.UpdateParty
            o.MultiAddWithNoCotacts()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub btnMaintainClaim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMaintainClaim.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.MaintainClaim
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdAA_InvalidUserName_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAA_InvalidUserName.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.AddAddress
            o.WSESecurity_InvalidUserName()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdAA_InvalidPassword_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAA_InvalidPassword.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.AddAddress
            o.WSESecurity_InvalidPassword()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdAA_InvalidTaskCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAA_InvalidTaskCode.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.AddAddress
            o.WSESecurity_InvalidTaskCode()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdAA_MissingSecurity_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAA_MissingSecurity.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.AddAddress
            o.WSESecurity_MissingSecurity()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub btnSuccessOther_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSuccessOther.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.AddParty
            o.Success_Other()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub btnAddClaimRisk_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddClaimRisk_Success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.AddClaimRisk
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub btnGetClaimRiskReadOnly_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetClaimRiskReadOnly_Success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimRiskReadOnly
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub btnGetClaimRisk_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetClaimRisk_Success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimRisk
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub btnGetAccountBalance_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetAccountBalance_Success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetAccountBalance
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    ' TODO: Richard Taylor - SiriusFS.SAM.Nunit.SAMForInsurance.UpdateClaimRisk does not yet exist.
    'Private Sub btnUpdateClaimRisk_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUpdateClaimRisk_Success.Click
    '    Try
    '        BeginProcess(DirectCast(sender, Control))
    '        Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.UpdateClaimRisk
    '        o.Success()
    '    Catch ex As Exception
    '        Handler.InterfaceLayerTop(ex)
    '    Finally
    '        EndProcess()
    '    End Try
    'End Sub

#End Region

    Private Sub btnGetClaimRiskLinks_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetClaimRiskLinks.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimRiskLinks
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub btnGetUserDetails_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetUserDetails_Success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetUserDetails
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub btnAddAgentReceipt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnAddAgentReceipt.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.AddAgentReceipt
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub btnPostDocument_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPostDocument.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.PostDocument
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub btnFindInsuranceFileForClaims_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFindInsuranceFileForClaims.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.FindInsuranceFileForClaims
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdCDI_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCDI_Success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.ClientDataImport
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdCDI_MissingBranchCode_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCDI_MissingBranchCode.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.ClientDataImport
            o.MissingDataBranchCode()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    'Private Sub cmdClaimMTA_Success_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmdClaimMTA_Success.Click
    '    Try
    '        BeginProcess(DirectCast(sender, Control))
    '        Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.ClaimMTA
    '        o.Success()
    '    Catch ex As Exception
    '        Handler.InterfaceLayerTop(ex)
    '    Finally
    '        EndProcess()
    '    End Try
    'End Sub

    'Private Sub cmdSaveRisk_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdSaveRisk_Success.Click
    '    Try
    '        BeginProcess(DirectCast(sender, Control))
    '        Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.SaveRisk
    '        o.Success()
    '    Catch ex As Exception
    '        Handler.InterfaceLayerTop(ex)
    '    Finally
    '        EndProcess()
    '    End Try
    'End Sub


    Private Sub btnFullOpenClaim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFullOpenClaim.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.FullOpenClaim
            o.FullOpenClaimTest(0, 0, String.Empty)
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub


    Private Sub btnFullMaintainClaim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFullMaintainClaim.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.FullOpenClaim
            o.FullMaintainClaimTest()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try

    End Sub

    Private Sub btnPayClaim_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPayClaim.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.FullOpenClaim
            o.FullPayClaimTest()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try

    End Sub

    Private Sub btnGetClaimPaymentTaxGroups_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetClaimPaymentTaxGroups.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimPaymentTaxGroupDetails
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub btnGetClaimReceiptTaxGroups_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetClaimReceiptTaxGroups.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimReceiptTaxGroupDetails
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub btnClaimReceipt_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnClaimReceipt.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.Claimreceipt
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try

    End Sub

    Private Sub Button5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button5.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetClaimreceiptTaxes
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    'Private Sub cmdAddMTAQuote_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddMTAQuote_Success.Click
    '    Try
    '        BeginProcess(DirectCast(sender, Control))
    '        Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.AddMTAQuote
    '        o.Success()
    '    Catch ex As Exception
    '        Handler.InterfaceLayerTop(ex)
    '    Finally
    '        EndProcess()
    '    End Try
    'End Sub

    Private Sub cmdUR_SuccessWithMTA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUR_SuccessWithMTA.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.UpdateRisk
            o.SuccessWithMTA()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdAddMTAQuote_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdAddMTAQuote_Success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.AddMTAQuote
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdGetOptionSetting_Success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGetOptionSetting_Success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetOptionSetting
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdGetInstalmentQuotes_success_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGetInstalmentQuotes_success.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetInstalmentQuotes
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub	  



    Private Sub btnFullMTA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFullMTA.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.MTA
            o.TestMTA()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub btnMTAViaPayNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMTAViaPayNow.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.MTA
            o.TestMTAPayNow()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub btnMTCViaPayNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMTCViaPayNow.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.MTA
            o.TestMTCPayNow()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub btnFullNBViaInvoice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFullNBViaInvoice.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.MTA
            o.TestNewBusinessPayByInvoiceDirect()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub btnGetHeaderAndSummaryByRef_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnGetHeaderAndSummaryByRef.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetHeaderAndSummariesByRef
            o.Success()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub btnNewBusinessViaPayNow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNewBusinessViaPayNow.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.MTA
            o.TestNewBusinessPayNow()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub btnNBViaInstalments_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNBViaInstalments.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.MTA
            o.TestNewBusinessPayByInstalments()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub Button6_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMTAViaInstalments.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.MTA
            o.TestMTAWithInstalments()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub btnBindQuote_SuccessWithInstalments_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnBindQuote_SuccessWithInstalments.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.BindQuote
            o.Success_Instalments()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdUR_SuccessMTAInstalments_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUR_SuccessMTAInstalments.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.UpdateRisk
            o.SuccessWithMTAAndInstalments()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub cmdGetInstalmentQuotes_successWithMTA_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdGetInstalmentQuotes_successWithMTA.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.GetInstalmentQuotes
            o.SuccessWithMTA()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub btnMTCViaInvoice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMTCViaInvoice.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.MTA
            o.TestMTCPayByInvoice()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub btnTestMTCViainstalments_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTestMTCViainstalments.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.MTA
            o.TestMTCPayByInstalments()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub gbFullNBViaInvoice_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles gbFullNBViaInvoice.Enter

    End Sub

    Private Sub btnNBTMPViaInvoice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNBTMPViaInvoice.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.MTA
            o.TestNewBusinessPayByInvoiceDirectTMP()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub btnMTATMPViaInvoice_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnMTATMPViaInvoice.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.MTA
            o.TestMTATMPWithInvoice()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub btnNBTMPViaInstalments_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNBTMPViaInstalments.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.MTA
            o.TestNewBusinessTMPPayByInstalments()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub btnNBViaInvoicePlusOneMonthCover_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNBViaInvoicePlusOneMonthCover.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.MTA
            o.TestMTAWithPolicyExtendedBy1Month()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub

    Private Sub btnTestNBUpdateQuoteAndGetPolicyDetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnTestNBUpdateQuoteAndGetPolicyDetails.Click
        Try
            BeginProcess(DirectCast(sender, Control))
            Dim o As New SiriusFS.SAM.Nunit.SAMForInsurance.MTA
            o.TestUpdateQuoteAndGetPolicyDetails()
        Catch ex As Exception
            Handler.InterfaceLayerTop(ex)
        Finally
            EndProcess()
        End Try
    End Sub
End Class


