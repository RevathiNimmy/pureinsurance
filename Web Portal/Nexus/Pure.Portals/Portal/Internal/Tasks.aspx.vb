Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Utils
Imports CMS.Library

Partial Class Portal_Tasks
    Inherits Frontend.clsCMSPage


    Protected Overrides Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Nexus.UserCanDoTask("Claim") Then
            liClaimSearch.Visible = False
        End If

        If Not Nexus.UserCanDoTask("AuthorisePayment") Then
            liAuthorisePayments.Visible = False
        End If

        If Not Nexus.UserCanDoTask("OpenClaim") Then
            liClaimIntimation.Visible = False
        End If

        If Not Nexus.UserCanDoTask("ClaimPaymentAuthorisation") Then
            liAuthoriseclaimpayments.Visible = False
        End If

        If Not Nexus.UserCanDoTask("InsurerPayment") Then
            liInsurerPayment.Visible = False
        End If

        If Not Nexus.UserCanDoTask("SearchTransaction") Then
            liSearchTransactions.Visible = False
        End If

        If Not Nexus.UserCanDoTask("RenewalSelection") Then
            liRenewalSelection.Visible = False
        End If

        If Not Nexus.UserCanDoTask("CashChequePayment") Then
            liCashChequePayment.Visible = False
        End If

        If Not Nexus.UserCanDoTask("CashChequeReceipt") Then
            liCashChequeReceipts.Visible = False
        End If

        If Not Nexus.UserCanDoTask("ClaimPaymentProcessing") Then
            liClaimPaymentProcessing.Visible = False
        End If

        If Not Nexus.UserCanDoTask("CoverNoteManagement") Then
            liCoverNoteManagement.Visible = False
        End If

        If Not Nexus.UserCanDoTask("BankGuarantee") Then
            liBankGuarantee.Visible = False
        End If

        If Not Nexus.UserCanDoTask("RenewalManager") Then
            liRenewalManager.Visible = False
        End If

        'If Not UserCanDoTask("BrokerView") Then
        '    liBrokerView.Visible = False
        'End If
    End Sub
End Class
