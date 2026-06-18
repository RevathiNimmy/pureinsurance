Imports System.Web.HttpContext
Imports Nexus.Utils
Imports System.Data
Imports Nexus.Constants.Session
Imports Nexus.Constants.Constant


Namespace Nexus
    Partial Class SummaryCoverCntrl
        Inherits System.Web.UI.UserControl

#Region "Page Constants"

#End Region

        Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            If (Session(CNQuoteMode) = QuoteMode.FullQuote) Then
                liScheduledocument.Visible = True
                liCertificate.Visible = True
                liDebitNote.Visible = True
            ElseIf (Session(CNQuoteMode) = QuoteMode.MTAQuote) Then
                'liMTAScheduledocument.Visible = True
            End If

            If Session(CNRenewal) IsNot Nothing Then
                liScheduledocument.Visible = True
                liCertificate.Visible = True
                liDebitNote.Visible = True
            End If

            If Session(CNProduceDocument) IsNot Nothing Then
                If Session(CNProduceDocument) = True And Session(CNReceiptMode) IsNot Nothing Then
                    liReceiptdocument.Visible = True
                ElseIf Session(CNProduceDocument) = True And Session(CNPaymentMode) IsNot Nothing Then
                    liReceiptdocument.Visible = False
                End If
            End If

            If Session(CNMTAType) = MTAType.CANCELLATION Then
                liMTCScheduledocument.Visible = True
            End If
            'Dim oPolicySummary As NexusProvider.PolicySummary
            'Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            'Dim oCashListReceipt As New NexusProvider.CashListReceipt
            'Dim oResults As NexusProvider.CashListReceipts
            'oPolicySummary = Session.Item(CNPolicy_Summary)
            'oCashListReceipt.InsuranceRef = oPolicySummary.Reference
            'oResults = oWebservice.FindCashListReceipts(oCashListReceipt)

            'If oResults IsNot Nothing AndAlso oResults.Count > 0 Then
            '    If oResults(0).MediaTypeCode.Trim.ToUpper = "CQ" Then
            '        liReceiptdocument.Visible = True
            '    Else
            '        liReceiptdocument.Visible = False
            '    End If
            'End If
        End Sub
    End Class
End Namespace