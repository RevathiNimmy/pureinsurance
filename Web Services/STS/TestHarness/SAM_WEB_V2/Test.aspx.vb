Imports Microsoft.Web.Services3.Security.Tokens
Imports SAMForInsuranceV2

Partial Class Test
    Inherits System.Web.UI.Page

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim UserToken As UsernameToken = GetUserToken("sirius", "sirius")
        Dim oSAM As New SAMForInsuranceV2
        oSAM.SetClientCredential(UserToken)
        oSAM.SetPolicy("SamClientPolicy")

        'Dim oRequest2 As New GetHeaderAndAgentCommissionByKeyRequestType
        'Dim oResponse2 As New GetHeaderAndAgentCommissionByKeyResponseType
        'oRequest2.InsuranceFileKey = 4573
        'oRequest2.BranchCode = "Headoff"
        'oResponse2 = oSAM.GetHeaderAndAgentCommissionByKey(oRequest2)
        'Response.Write("aa")

        'Dim oRequest As New GetAgentCommissionRequestType
        'Dim oResponse As New GetAgentCommissionResponseType
        'oRequest.InsuranceFileKey = 4573
        'oRequest.BranchCode = "Headoff"
        'oResponse = oSAM.GetAgentCommission(oRequest)

        'Dim oRequest1 As New UpdateAgentCommissionRequestType
        'Dim oResponse1 As New UpdateAgentCommissionResponseType
        'Dim oAgentCommission As BaseUpdateAgentCommissionRequestTypeRow()
        'Dim oitem As BaseAgentCommissionResponseTypeRow
        'oRequest1.InsuranceFileKey = 4573
        'oRequest1.BranchCode = "Headoff"
        'Dim i As Integer
        'For Each oitem In oResponse.AgentCommission
        '    ReDim Preserve oAgentCommission(i)
        '    oAgentCommission(i) = New BaseUpdateAgentCommissionRequestTypeRow
        '    oAgentCommission(i).Agent = oitem.Agent
        '    oAgentCommission(i).AgentType = oitem.AgentType
        '    oAgentCommission(i).CommissionBand = oitem.CommissionBand
        '    If oitem.IsValue = False Then
        '        oAgentCommission(i).CommissionRate = 15
        '        oAgentCommission(i).CommissionValue = 15 * oitem.Premium / 100
        '        oAgentCommission(i).CalculatedCommissionValue = oAgentCommission(i).CommissionValue
        '        oAgentCommission(i).CalculatedCommissionValueSpecified = True
        '        oAgentCommission(i).IsAmended = True
        '        oAgentCommission(i).OverRideReason = "hello"
        '    Else
        '        oAgentCommission(i).CommissionRate = 0
        '        oAgentCommission(i).CommissionValue = 0
        '        oAgentCommission(i).CalculatedCommissionValue = 0
        '        oAgentCommission(i).CalculatedCommissionValueSpecified = True
        '    End If

        '    oAgentCommission(i).IsLeadAgent = oitem.IsLeadAgent
        '    oAgentCommission(i).Premium = oitem.Premium
        '    oAgentCommission(i).RiskType = oitem.RiskType
        '    oAgentCommission(i).TaxGroupCode = oitem.TaxGroup
        '    oAgentCommission(i).IsValue = oitem.IsValue
        '    oAgentCommission(i).MaximumRate = oitem.MaximumRate

        '    i = i + 1
        'Next
        'oRequest1.AgentCommission = oAgentCommission
        'oResponse1 = oSAM.UpdateAgentCommission(oRequest1)

      
        'Dim oRequest As New GetRiskRequestType
        'Dim oResponse As New GetRiskResponseType

        'oRequest.BranchCode = "Headoff"
        'oRequest.InsuranceFileKey = oResponse1.InsuranceFileKey
        'oRequest.InsuranceFolderKey = oResponse1.InsuranceFolderKey
        'oRequest.RiskKey = 5133
        'oRequest.QuoteTimeStamp = oResponse1.QuoteTimeStamp
        'oResponse = oSAM.GetRisk(oRequest)

        'Dim oRequest2 As New RunDefaultRulesEditRequestType
        'Dim oResponse2 As New RunDefaultRulesEditResponseType
        'oRequest2.BranchCode = "Headoff"
        'oRequest2.ScreenCode = "MOTOR"
        'oRequest2.XMLDataSet = oResponse.XMLDataSet
        'oResponse2 = oSAM.RunDefaultRulesEdit(oRequest2)

        'Dim oRequest As New BindQuoteRequestType
        'Dim oResponse As New BindQuoteResponseType
        'oRequest.BranchCode = "Headoff"
        'oRequest.InsuranceFileKey = 4567
        'oRequest.PaymentMethod = PaymentMethodType.AgentOverdraft
        'oRequest.PaymentMethodSpecified = True
        'oRequest.DebitAgainst = DebitAgainstType.DebitAgainstOverDraft
        'oRequest.DebitAgainstSpecified = True
        'oRequest.TransactionType = "NB"
        'oRequest.QuoteTimeStamp = oResponse1.QuoteTimeStamp
        'oResponse = oSAM.BindQuote(oRequest)
        'Response.Write("aa")

        Dim oRequest As New FindCashListReceiptsRequestType
        Dim oResponse As New FindCashListReceiptsResponseType

        oRequest.BranchCode = "Headoff"
        oRequest.CollectionDateFrom = "1-Jan-2009"
        oRequest.CollectionDateFromSpecified = True
        oRequest.CollectionDateTo = "5-sep-2009"
        oRequest.CollectionDateToSpecified = True
        oRequest.DocumentRef = "SRP00000261"

        oResponse = oSAM.FindCashListReceipts(oRequest)
       
        'Dim oRequest1 As New UpdateReceiptMediaTypeStatusRequestType
        'Dim oResponse1 As New UpdateReceiptMediaTypeStatusResponseType
        'Dim oCashListItems As BaseUpdateReceiptMediaTypeStatusRequestTypeRow()
        'oRequest1.BranchCode = "Headoff"
        'ReDim oCashListItems(0)
        'oCashListItems(0) = New BaseUpdateReceiptMediaTypeStatusRequestTypeRow
        'oCashListItems(0).CashListItemKey = oResponse.CashListItems(0).CashListItemKey
        'oCashListItems(0).Comments = oResponse.CashListItems(0).CashListItemKey
        'oCashListItems(0).DocumentRef = oResponse.CashListItems(0).CashListItemKey
        'oCashListItems(0).InsuranceFileKey = oResponse.CashListItems(0).CashListItemKey
        'oCashListItems(0).MediaTypeCode = "CQ"
        'oCashListItems(0).MediaTypeStatusCode = "SRPC"
        'oCashListItems(0).ModifiedDate = Today
        'oRequest1.CashListItems = oCashListItems

        'Dim oRequest As New GetTaskGroupTasksRequestType
        'Dim oResponse As New GetTaskGroupTasksResponseType

        'oRequest.BranchCode = "Headoff"
        'oRequest.TaskGroupKey = 10
        'oRequest.EffectiveDate = Now


        'oResponse = oSAM.GetTaskGroupTasks(oRequest)
        'Response.Write("aa")
    End Sub
End Class
