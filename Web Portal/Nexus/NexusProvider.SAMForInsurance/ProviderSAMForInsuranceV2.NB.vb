Imports NexusProvider.SAMForInsurance.PureService
Imports System.ServiceModel
Imports System.Configuration.Provider
Imports System.Web
Imports System.Web.HttpContext
Imports System.Web.Configuration.WebConfigurationManager
Imports Microsoft.Web.Services3.Security.Tokens
Imports SiriusFS.SAM.Client.Security
Imports System.Xml.Serialization
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports System.Diagnostics
Imports System.Text
Imports System.Xml
Imports System.IO
Imports Ionic.Zip
Partial Public Class ProviderSAMForInsuranceV2

    ''' <summary>
    ''' This is an existing method. To amend a policy before renewal, call UpdateQuoteV2 method. 
    ''' No change has been made to request and response structure.  
    ''' </summary>
    ''' <param name="r_oQuote"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub UpdateQuote(ByRef r_oQuote As Quote,
                                    Optional ByVal v_sBranchCode As String = Nothing)


        SyncLock oLock
            Dim oSAM As PureServiceClient
            Dim oUpdateQuoteRequest As UpdateQuoteRequestType
            Dim oUpdateQuoteResponse As UpdateQuoteResponseType
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oUpdateQuoteRequest = New UpdateQuoteRequestType
                oUpdateQuoteResponse = New UpdateQuoteResponseType
                sbLogMessage = New StringBuilder


                With oUpdateQuoteRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If

                    .CoverEndDate = r_oQuote.CoverEndDate
                    .CoverStartDate = r_oQuote.CoverStartDate
                    .Description = r_oQuote.Description
                    .InsuranceFileKey = r_oQuote.InsuranceFileKey
                    .InsuranceFolderKey = r_oQuote.InsuranceFolderKey
                    '.InsuredParties = r_oQuote.InsuredParties
                    .QuoteTimeStamp = r_oQuote.TimeStamp
                    .AnalysisCode = r_oQuote.AnalysisCode
                    If r_oQuote.ConsolidatedLeadAgentCommission > 0 Then
                        .ConsolidatedLeadAgentCommission = r_oQuote.ConsolidatedLeadAgentCommission
                        .ConsolidatedLeadAgentCommissionSpecified = True
                    Else
                        .ConsolidatedLeadAgentCommissionSpecified = False
                    End If

                    If r_oQuote.ConsolidatedSubAgentCommission > 0 Then
                        .ConsolidatedSubAgentCommission = r_oQuote.ConsolidatedSubAgentCommission
                        .ConsolidatedSubAgentCommissionSpecified = True
                    Else
                        .ConsolidatedSubAgentCommissionSpecified = False


                    End If

                    .ConsolidatedSubAgentCommission = r_oQuote.ConsolidatedSubAgentCommission
                    .CurrencyCode = r_oQuote.CurrencyCode
                    .CoverNoteBookNumber = r_oQuote.CoverNoteBookNumber
                    If r_oQuote.CoverNoteSheetNumber > 0 Then
                        .CoverNoteSheetNumber = r_oQuote.CoverNoteSheetNumber
                        .CoverNoteSheetNumberSpecified = True
                    Else
                        .CoverNoteSheetNumberSpecified = False
                    End If
                    .AlternativeRef = r_oQuote.AlternativeRef
                End With


                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oUpdateQuoteResponse = oSAM.UpdateQuote(oUpdateQuoteRequest)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.




                With oUpdateQuoteResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        r_oQuote.TimeStamp = .QuoteTimeStamp
                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("UpdateQuote executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("r_oQuote = " & r_oQuote.Print.Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If
                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oUpdateQuoteRequest = Nothing
                oUpdateQuoteResponse = Nothing
            End Try


        End SyncLock

    End Sub
#Region "CATLIN - 1.13.5 Anonymous Quote"
    ''' <summary>
    ''' To Transfer Anonymous quote to real party
    ''' </summary>
    ''' <param name="v_iPartyFrom"></param>
    ''' <param name="v_iPartyTo"></param>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub TransferQuoteToRealParty(ByVal v_iPartyFrom As Integer, ByVal v_iPartyTo As Integer,
        ByVal v_iInsuranceFileKey As Integer, Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oTransferQuoteRequest As TransferQuoteRequestType
            Dim oTransferQuoteResponse As TransferQuoteResponseType
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oTransferQuoteRequest = New TransferQuoteRequestType
                oTransferQuoteResponse = New TransferQuoteResponseType
                sbLogMessage = New StringBuilder



                With oTransferQuoteRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If

                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    .PartyToKey = v_iPartyTo
                    .PartyFromKey = v_iPartyFrom

                    If v_iInsuranceFileKey > 0 Then
                        .InsuranceFileKey = v_iInsuranceFileKey
                    Else
                        Throw New ArgumentNullException("InsuranceFileKey")
                    End If

                End With


                oTransferQuoteResponse = oSAM.TransferQuote(oTransferQuoteRequest)




                With oTransferQuoteResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)

                    Else

                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("TransferQuote executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Output:" & vbCrLf)
                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oTransferQuoteRequest = Nothing
                oTransferQuoteResponse = Nothing
            End Try

        End SyncLock
    End Sub
#End Region


    ''' <summary>
    ''' This is an existing method. To amend a single risk, call UpdateRisk method with TransactionType 
    ''' field of request set to�REN�. No change has been made to request and response structure.  
    ''' </summary>
    ''' <param name="r_oQuote"></param>
    ''' <param name="v_iQuoteRiskIndex"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="v_sSubBranchCode"></param>
    ''' <param name="v_sTransactionType"></param>
    ''' <remarks></remarks>
    Public Overrides Sub UpdateRisk(ByRef r_oQuote As Quote,
                                  ByVal v_iQuoteRiskIndex As Integer,
                                  Optional ByVal v_sBranchCode As String = Nothing,
                                  Optional ByVal v_sSubBranchCode As String = Nothing,
                                  Optional ByVal v_sTransactionType As String = Nothing)

        SyncLock oLock
            Dim oSAM As PureServiceClient = Nothing
            Dim oUpdateRiskRequest As UpdateRiskRequestType
            Dim oUpdateRiskResponse As UpdateRiskResponseType
            Dim oPolicyFee As Fee
            Dim oPolicyTax As Tax
            Dim oRiskFee As Fee
            Dim oRiskTax As Tax
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oUpdateRiskRequest = New UpdateRiskRequestType
                oUpdateRiskResponse = New UpdateRiskResponseType
                sbLogMessage = New StringBuilder

                If r_oQuote.Risks Is Nothing Then
                    Throw New ArgumentNullException("Quote.Risk", "Quote must contain one Risk to call Update Risk")
                ElseIf r_oQuote.Risks.Count < 1 Then
                    Throw New ArgumentNullException("Quote.Risk", "Quote must contain one Risk to call Update Risk")
                ElseIf v_iQuoteRiskIndex > r_oQuote.Risks.Count - 1 Then
                    Throw New ArgumentOutOfRangeException("QuoteRiskIndex")
                Else

                    With oUpdateRiskRequest
                        .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                        'if the passed parameter v_sBranchCode is empty
                        If String.IsNullOrEmpty(v_sBranchCode) Then
                            'if the branch code is NOT in session 
                            If String.IsNullOrEmpty(sBranchCode) Then
                                'Use the default branch code
                                .BranchCode = sDefaultBranchCode
                            Else
                                'Use the branch code in session 
                                .BranchCode = sBranchCode
                            End If
                        Else
                            'use the passed parameter v_sBranchCode
                            .BranchCode = v_sBranchCode
                        End If

                        .SubBranchCode = r_oQuote.SubBranchCode
                        .InsuranceFolderKey = r_oQuote.InsuranceFolderKey
                        .InsuranceFileKey = r_oQuote.InsuranceFileKey
                        .RiskKey = r_oQuote.Risks(v_iQuoteRiskIndex).Key
                        .ScreenCode = r_oQuote.Risks(v_iQuoteRiskIndex).ScreenCode
                        .XMLDataSet = r_oQuote.Risks(v_iQuoteRiskIndex).XMLDataset
                        .RiskDescription = r_oQuote.Risks(v_iQuoteRiskIndex).Description '.RiskDescription
                        .QuoteTimeStamp = r_oQuote.TimeStamp
                        .TransactionType = v_sTransactionType

                    End With

                    'add trace to allow us to debug slow SAM calls
                    Using trace As New Tracer(Category.Trace)
                        oUpdateRiskResponse = oSAM.UpdateRisk(oUpdateRiskRequest)
                    End Using

                    'NO catches on the try as we want to cascade all exceptions back up the stack for handling.
                    With oUpdateRiskResponse

                        If .Errors IsNot Nothing Then
                            'for Refer/Decline reason we need to update the XML also
                            r_oQuote.Risks(v_iQuoteRiskIndex).XMLDataset = oUpdateRiskResponse.XMLDataSet
                            r_oQuote.TimeStamp = .QuoteTimeStamp
                            'Process the error object if errors, and throw as a single exception
                            Throw New NexusException(.Errors)
                        Else

                            r_oQuote.Risks(v_iQuoteRiskIndex).PremiumDueNet = .PremiumDueNet
                            r_oQuote.Risks(v_iQuoteRiskIndex).PremiumDueTax = .PremiumDueTax
                            r_oQuote.Risks(v_iQuoteRiskIndex).PremiumDueGross = .PremiumDueGross
                            r_oQuote.Risks(v_iQuoteRiskIndex).TotalAnnualTax = .TotalAnnualTax
                            r_oQuote.Risks(v_iQuoteRiskIndex).CommissionAmount = .CommissionAmount

                            r_oQuote.Risks(v_iQuoteRiskIndex).XMLDataset = oUpdateRiskResponse.XMLDataSet

                            r_oQuote.TimeStamp = .QuoteTimeStamp
                            r_oQuote.Risks(v_iQuoteRiskIndex).PolicyLevelTax = .PolicyLevelTax
                            r_oQuote.Risks(v_iQuoteRiskIndex).PolicyLevelFees = .PolicyLevelFees
                            r_oQuote.Risks(v_iQuoteRiskIndex).ProRata = .ProRata
                            r_oQuote.Risks(v_iQuoteRiskIndex).ProRataRate = .ProRataRate
                            r_oQuote.Risks(v_iQuoteRiskIndex).ProRataMessage = .ProRataMessage

                            If .PolicyLevelTaxesAndFees.Fees IsNot Nothing Then
                                For Each v_oPolicyFees As BaseFeesType In .PolicyLevelTaxesAndFees.Fees
                                    oPolicyFee = New Fee
                                    oPolicyFee.FeeAmount = v_oPolicyFees.Amount
                                    oPolicyFee.Description = v_oPolicyFees.Description
                                    r_oQuote.Risks(v_iQuoteRiskIndex).PolicyFees.Add(oPolicyFee)

                                Next
                            End If

                            If .PolicyLevelTaxesAndFees.Taxes IsNot Nothing Then
                                For Each v_oPolicyTaxes As BaseTaxesType In .PolicyLevelTaxesAndFees.Taxes
                                    oPolicyTax = New Tax
                                    oPolicyTax.TaxAmount = v_oPolicyTaxes.Amount
                                    oPolicyTax.Description = v_oPolicyTaxes.Description

                                    r_oQuote.Risks(v_iQuoteRiskIndex).PolicyTaxes.Add(oPolicyTax)

                                Next
                            End If

                            If .RiskLevelTaxesAndFees.Fees IsNot Nothing Then
                                For Each v_oRiskFees As BaseFeesType In .RiskLevelTaxesAndFees.Fees
                                    oRiskFee = New Fee
                                    oRiskFee.FeeAmount = v_oRiskFees.Amount
                                    oRiskFee.Description = v_oRiskFees.Description

                                    r_oQuote.Risks(v_iQuoteRiskIndex).RiskFees.Add(oRiskFee)
                                Next
                            End If
                            If .RiskLevelTaxesAndFees.Taxes IsNot Nothing Then
                                For Each v_oRiskTaxes As BaseTaxesType In .RiskLevelTaxesAndFees.Taxes
                                    oRiskTax = New Tax
                                    oRiskTax.TaxAmount = v_oRiskTaxes.Amount
                                    oRiskTax.Description = v_oRiskTaxes.Description

                                    r_oQuote.Risks(v_iQuoteRiskIndex).RiskTaxes.Add(oRiskTax)
                                Next
                            End If
                        End If
                    End With
                End If

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("UpdateRisk executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("r_oQuote = " & r_oQuote.Print.Replace("<br />", vbCrLf))
                    sbLogMessage.AppendLine("v_iQuoteRiskIndex = " & v_iQuoteRiskIndex.ToString)


                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If Not IsNothing(v_sSubBranchCode) Then
                        sbLogMessage.AppendLine("v_sSubBranchCode = " & v_sSubBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sSubBranchCode = nothing" & vbCrLf)
                    End If

                    If Not IsNothing(v_sTransactionType) Then
                        sbLogMessage.AppendLine("v_sTransactionType = " & v_sTransactionType.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sTransactionType = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oUpdateRiskRequest = Nothing
                oUpdateRiskRequest = Nothing
                oUpdateRiskResponse = Nothing
                oPolicyFee = Nothing
                oPolicyTax = Nothing
                oRiskFee = Nothing
                oRiskTax = Nothing
            End Try

        End SyncLock

    End Sub


    ''' <summary>
    ''' This method is called for updating the selected quote.
    ''' </summary>
    ''' <param name="r_oQuoteV2"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="v_sSubBranchCode"></param>
    ''' <param name="v_iAgentKey"></param>
    ''' <remarks></remarks>
    Public Overrides Sub UpdateQuotev2(ByRef r_oQuoteV2 As Quote,
                                  Optional ByVal v_sBranchCode As String = Nothing,
                                  Optional ByVal v_sSubBranchCode As String = Nothing,
                                  Optional ByVal v_iAgentKey As Integer = 0)


        SyncLock oLock

            Dim oSAM As PureServiceClient = Nothing
            Dim oUpdateQuoteV2Request As UpdateQuoteV2RequestType
            Dim oUpdateQuoteV2Response As UpdateQuoteV2ResponseType
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oUpdateQuoteV2Request = New UpdateQuoteV2RequestType
                oUpdateQuoteV2Response = New UpdateQuoteV2ResponseType
                sbLogMessage = New StringBuilder


                With oUpdateQuoteV2Request
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)


                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        If String.IsNullOrEmpty(r_oQuoteV2.BranchCode) Then
                            'if the branch code is NOT in session 
                            If String.IsNullOrEmpty(sBranchCode) Then
                                'Use the default branch code
                                .BranchCode = sDefaultBranchCode

                            Else
                                'Use the branch code in session 
                                .BranchCode = sBranchCode
                            End If
                        Else
                            .BranchCode = r_oQuoteV2.BranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .InsuranceFolderKey = r_oQuoteV2.InsuranceFolderKey
                    .InsuranceFileKey = r_oQuoteV2.InsuranceFileKey
                    .Timestamp = r_oQuoteV2.TimeStamp
                    .PartyKey = r_oQuoteV2.PartyKey
                    .SubBranchCode = r_oQuoteV2.SubBranchCode
                    If r_oQuoteV2.ConsolidatedLeadAgentCommission > 0 Then
                        .ConsolidatedLeadAgentCommission = r_oQuoteV2.ConsolidatedLeadAgentCommission
                        .ConsolidatedLeadAgentCommissionSpecified = True
                    Else
                        .ConsolidatedLeadAgentCommissionSpecified = False
                    End If
                    'Code meant to override MarkedQuoteForCollection in the Request
                    If r_oQuoteV2.MarkedQuoteForCollection = False Then
                        .MarkedForCollection = False
                    Else
                        .MarkedForCollection = True
                    End If
                    'This field will always be true
                    .MarkedForCollectionSpecified = True

                    If IsDate(r_oQuoteV2.MarkedDateforCollection) Then
                        .MarkedDateSpecified = True
                        If r_oQuoteV2.MarkedDateforCollection <> Date.MinValue Then
                            .MarkedDate = r_oQuoteV2.MarkedDateforCollection
                        End If
                    Else
                        .MarkedDateSpecified = False
                    End If
                    'end

                    If r_oQuoteV2.ConsolidatedSubAgentCommission > 0 Then
                        .ConsolidatedSubAgentCommission = r_oQuoteV2.ConsolidatedSubAgentCommission
                        .ConsolidatedSubAgentCommissionSpecified = True
                    Else
                        .ConsolidatedSubAgentCommissionSpecified = False
                    End If

                    If r_oQuoteV2.Risks.Count > 0 Then
                        If r_oQuoteV2.Risks(0).PremiumDueGross = 0 Then
                            If r_oQuoteV2.Risks(0).StatusCode IsNot Nothing Then

                            ElseIf r_oQuoteV2.CoverNoteSheetNumberSpecified = False And r_oQuoteV2.Risks(0).StatusCode Is Nothing Then
                                If r_oQuoteV2.CoverNoteBookNumber IsNot Nothing Then
                                    If r_oQuoteV2.CoverNoteBookNumber.Trim.Length <> 0 Then
                                        .CoverNoteBookNumber = r_oQuoteV2.CoverNoteBookNumber
                                    End If
                                End If
                                If r_oQuoteV2.CoverNoteSheetNumber <> 0 Then
                                    .CoverNoteSheetNumber = r_oQuoteV2.CoverNoteSheetNumber
                                    .CoverNoteSheetNumberSpecified = True
                                    r_oQuoteV2.CoverNoteSheetNumberSpecified = True
                                Else
                                    .CoverNoteSheetNumberSpecified = False
                                End If
                            End If
                        End If
                    End If

                    .BusinessTypeCode = r_oQuoteV2.BusinessTypeCode
                    .QuoteExpiryDate = r_oQuoteV2.QuoteExpiryDate
                    .HandlerCode = r_oQuoteV2.HandlerCode
                    .Regarding = r_oQuoteV2.Regarding
                    .PolicyStatusCode = r_oQuoteV2.PolicyStatusCode
                    .InceptionDate = r_oQuoteV2.InceptionDate
                    .RenewalDate = r_oQuoteV2.RenewalDate
                    .InceptionTPI = r_oQuoteV2.InceptionTPI
                    .IssuedDate = r_oQuoteV2.IssuedDate
                    If r_oQuoteV2.UnderwritingYearId <> 0 Then
                        .UnderwritingYearId = r_oQuoteV2.UnderwritingYearId
                        .UnderwritingYearIdSpecified = True
                    Else
                        .UnderwritingYearIdSpecified = False
                    End If

                    If r_oQuoteV2.ProposalDate <> Date.MinValue Then
                        .ProposalDate = r_oQuoteV2.ProposalDate
                        .ProposalDateSpecified = True
                    Else
                        .ProposalDateSpecified = False
                    End If

                    .FrequencyCode = r_oQuoteV2.FrequencyCode
                    .RenewalMethodCode = r_oQuoteV2.RenewalMethodCode
                    .LapseCancelReasonCode = r_oQuoteV2.LapseCancelReasonCode
                    If r_oQuoteV2.LTUExpiryDate <> Date.MinValue Then
                        .LTUExpiryDate = r_oQuoteV2.LTUExpiryDate
                        .LTUExpiryDateSpecified = True
                    Else
                        .LTUExpiryDateSpecified = False
                    End If

                    .StopReasonCode = r_oQuoteV2.StopReasonCode
                    If r_oQuoteV2.LapseCancelDate <> Date.MinValue Then
                        .LapseCancelDate = r_oQuoteV2.LapseCancelDate
                        .LapseCancelDateSpecified = True
                    Else
                        .LapseCancelDateSpecified = False
                    End If
                    If r_oQuoteV2.ReferredAtRenewal = True Then
                        .ReferredAtRenewal = r_oQuoteV2.ReferredAtRenewal
                        .ReferredAtRenewalSpecified = True
                    Else
                        .ReferredAtRenewalSpecified = False

                    End If

                    If r_oQuoteV2.ReferredAtMTA = True Then
                        .ReferredAtMTA = r_oQuoteV2.ReferredAtMTA
                        .ReferredAtMTASpecified = True
                    Else
                        .ReferredAtMTASpecified = False
                    End If
                    .PaymentMethod = r_oQuoteV2.PaymentMethod
                    .CoverStartDate = r_oQuoteV2.CoverStartDate
                    .CoverEndDate = r_oQuoteV2.CoverEndDate
                    .CurrencyCode = r_oQuoteV2.CurrencyCode
                    .ProductCode = r_oQuoteV2.ProductCode
                    .QuoteRef = r_oQuoteV2.InsuranceFileRef
                    .InsuredName = r_oQuoteV2.InsuredName
                    .AlternativeRef = r_oQuoteV2.AlternativeRef
                    .AnalysisCode = r_oQuoteV2.AnalysisCode

                    If v_iAgentKey > 0 Then
                        .AgentKey = v_iAgentKey
                        .AgentKeySpecified = True
                    Else
                        If r_oQuoteV2.Agent IsNot Nothing Then
                            If r_oQuoteV2.Agent.Trim.Length <> 0 And r_oQuoteV2.Agent.Trim <> "0" Then
                                .AgentKey = r_oQuoteV2.Agent
                                .AgentKeySpecified = True
                            Else
                                .AgentKeySpecified = False
                            End If
                        Else
                            .AgentKeySpecified = False
                        End If
                    End If

                    If r_oQuoteV2.RenewalDayNo > 0 Then
                        .RenewalDayNo = r_oQuoteV2.RenewalDayNo
                        .RenewalDayNoSpecified = True
                    Else
                        .RenewalDayNoSpecified = False
                    End If

                    If r_oQuoteV2.ConsolidatedLeadAgentCommission = True Then
                        .ConsolidatedLeadAgentCommission = r_oQuoteV2.ConsolidatedLeadAgentCommission
                        .ConsolidatedLeadAgentCommissionSpecified = True
                    Else
                        .ConsolidatedLeadAgentCommissionSpecified = False
                    End If

                    If r_oQuoteV2.ConsolidatedSubAgentCommission = True Then
                        .ConsolidatedSubAgentCommission = r_oQuoteV2.ConsolidatedSubAgentCommission
                        .ConsolidatedSubAgentCommissionSpecified = True
                    Else
                        .ConsolidatedSubAgentCommissionSpecified = False
                    End If

                    If r_oQuoteV2.ContactUserKey <> Nothing Then
                        If r_oQuoteV2.ContactUserKey <> 0 Then
                            .ContactuserKey = r_oQuoteV2.ContactUserKey
                            .ContactuserKeySpecified = True
                        Else
                            .ContactuserKeySpecified = False
                        End If
                    Else
                        .ContactuserKeySpecified = False
                    End If
                    'Else
                    '    .ContactuserKeySpecified = False
                    'End If
                    .CoInsurancePlacement = r_oQuoteV2.CoinsurancePlacement
                End With

                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oUpdateQuoteV2Response = oSAM.UpdateQuoteV2(oUpdateQuoteV2Request)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling
                With oUpdateQuoteV2Response

                    If .Errors IsNot Nothing Then
                        r_oQuoteV2.TimeStamp = .TimeStamp

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        r_oQuoteV2.TimeStamp = .TimeStamp
                        r_oQuoteV2.InsuranceFileRef = .InsuranceFileRef
                        r_oQuoteV2.BaseInsuranceFolderKey = .BaseInsuranceFolderKey
                        r_oQuoteV2.QuoteStatusKey = .QuoteStatusKey
                        r_oQuoteV2.QuoteVersion = .QuoteVersion
                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("UpdateQuoteV2 executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("r_oQuote = " & r_oQuoteV2.Print.Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If Not v_iAgentKey = 0 Then
                        sbLogMessage.AppendLine("v_iAgentKey = " & v_iAgentKey.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_iAgentKey = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oUpdateQuoteV2Request = Nothing
                oUpdateQuoteV2Response = Nothing
            End Try


        End SyncLock

    End Sub

    ''' <summary>
    ''' This Method is used to Update the Selected Risk.
    ''' </summary>
    ''' <param name="r_oQuote"></param>
    ''' <param name="v_iQuoteRiskIndex"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function UpdateRiskSelection(ByRef r_oQuote As Quote,
                                ByVal v_iQuoteRiskIndex As Integer,
                                Optional ByVal v_sBranchCode As String = Nothing) As Quote

        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oUpdateRiskSelectionRequest As UpdateRiskSelectionRequestType
            Dim oUpdateRiskSelectionResponse As UpdateRiskSelectionResponseType
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oUpdateRiskSelectionRequest = New UpdateRiskSelectionRequestType
                oUpdateRiskSelectionResponse = New UpdateRiskSelectionResponseType
                sbLogMessage = New StringBuilder


                With oUpdateRiskSelectionRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    .InsuranceFileKey = r_oQuote.InsuranceFileKey
                    .InsuranceFolderKey = r_oQuote.InsuranceFolderKey
                    .RiskKey = r_oQuote.Risks(v_iQuoteRiskIndex).Key
                    .TransactionType = r_oQuote.TransactionType
                    .IsSelected = r_oQuote.IsSelected
                    .TimeStamp = r_oQuote.TimeStamp

                End With


                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oUpdateRiskSelectionResponse = oSAM.UpdateRiskSelection(oUpdateRiskSelectionRequest)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.




                With oUpdateRiskSelectionResponse

                    If .Errors IsNot Nothing Then
                        r_oQuote.TimeStamp = .TimeStamp
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        r_oQuote.TimeStamp = .TimeStamp
                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("UpdateRiskSelection executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("r_oQuote = " & r_oQuote.Print.Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oUpdateRiskSelectionRequest = Nothing
                oUpdateRiskSelectionResponse = Nothing
            End Try


            Return r_oQuote

        End SyncLock

    End Function


    ''' <summary>
    ''' GetQuotesMarkedForCollection returns the Quote Collection
    ''' </summary>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="v_iPartyKey"></param>
    ''' <param name="v_iInsuranceFilekey"></param>
    ''' <param name="v_iAgentKey"></param>
    ''' <param name="v_oProduct"></param>
    ''' <param name="v_dSearchDateFrom"></param>
    ''' <param name="v_dSearchDateTo"></param>
    ''' <param name="v_bDirectBusinessonly"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetQuotesMarkedForCollection(Optional ByVal v_sBranchCode As String = Nothing,
                                         Optional ByVal v_iPartyKey As Integer = 0,
                                         Optional ByVal v_iInsuranceFilekey As Integer = 0,
                                         Optional ByVal v_iAgentKey As Integer = 0,
                                         Optional ByVal v_oProductCollection As ProductCollection = Nothing,
                                         Optional ByVal v_dSearchDateFrom As Date = Nothing,
                                         Optional ByVal v_dSearchDateTo As Date = Nothing,
                                         Optional ByVal v_bDirectBusinessonly As Boolean = False) As QuoteCollection

        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oGetQuotesMarkedForCollectionRequest As GetQuotesMarkedForCollectionRequestType
            Dim oGetQuotesMarkedForCollectionResponse As GetQuotesMarkedForCollectionResponseType
            Dim oProductsRequest As BaseGetQuotesMarkedForCollectionRequestTypeProducts
            Dim i As Integer = 0
            Dim oListOfQuotes As QuoteCollection = New QuoteCollection()
            Dim oQuote As Quote
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetQuotesMarkedForCollectionRequest = New GetQuotesMarkedForCollectionRequestType
                oGetQuotesMarkedForCollectionResponse = New GetQuotesMarkedForCollectionResponseType
                sbLogMessage = New StringBuilder


                With oGetQuotesMarkedForCollectionRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If

                    If v_iAgentKey > 0 Then
                        .AgentKey = v_iAgentKey
                        .AgentKeySpecified = True
                    Else
                        .AgentKeySpecified = False
                    End If

                    If v_iPartyKey > 0 Then
                        .PartyKey = v_iPartyKey
                        .PartyKeySpecified = True
                    Else
                        .PartyKeySpecified = False
                    End If

                    If v_iInsuranceFilekey > 0 Then
                        .InsuranceFileKey = v_iInsuranceFilekey
                        .InsuranceFileKeySpecified = True
                    Else
                        .InsuranceFileKeySpecified = False
                    End If

                    If v_bDirectBusinessonly = True Then
                        .DirectBusinessOnly = v_bDirectBusinessonly
                        .DirectBusinessOnlySpecified = True
                    Else
                        .DirectBusinessOnlySpecified = False
                    End If

                    'Loop through the collection to populate the Product array


                    If v_oProductCollection IsNot Nothing And v_oProductCollection.Count <> 0 Then

                        .Products = New List(Of BaseGetQuotesMarkedForCollectionRequestTypeProducts)
                        For i = 0 To v_oProductCollection.Count - 1
                            oProductsRequest = New BaseGetQuotesMarkedForCollectionRequestTypeProducts
                            oProductsRequest.ProductCode = v_oProductCollection(i).ProductCode
                            .Products.Add(oProductsRequest)
                        Next
                    End If

                    If v_dSearchDateFrom = Date.MinValue Then
                        .SearchDateFromSpecified = False
                    Else
                        .SearchDateFrom = v_dSearchDateFrom
                        .SearchDateFromSpecified = True
                    End If

                    If v_dSearchDateTo = Date.MinValue Then
                        .SearchDateToSpecified = False
                    Else
                        .SearchDateTo = v_dSearchDateTo
                        .SearchDateToSpecified = True
                    End If


                End With


                Using trace As New Tracer(Category.Trace)
                    oGetQuotesMarkedForCollectionResponse = oSAM.GetQuotesMarkedForCollection(oGetQuotesMarkedForCollectionRequest)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                With oGetQuotesMarkedForCollectionResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else

                        If oGetQuotesMarkedForCollectionResponse.MarkedQuotes IsNot Nothing Then

                            For Each oGetQuotesMarkedResponse As BaseGetQuotesMarkedForCollectionResponseTypeRow In .MarkedQuotes
                                oQuote = New Quote
                                oQuote.InsuranceFileKey = oGetQuotesMarkedResponse.InsuranceFileKey
                                oQuote.BranchCode = oGetQuotesMarkedResponse.BranchCode
                                oQuote.AgentCode = oGetQuotesMarkedResponse.AgentTypeCode
                                oQuote.Agent = oGetQuotesMarkedResponse.AgentName
                                oQuote.CurrencyCode = oGetQuotesMarkedResponse.CurrencyCode
                                oQuote.InsuranceFileRef = oGetQuotesMarkedResponse.InsuranceFileRef
                                oQuote.PartyKey = oGetQuotesMarkedResponse.PartyKey
                                oQuote.GrossTotal = oGetQuotesMarkedResponse.Premium
                                oQuote.ProductCode = oGetQuotesMarkedResponse.ProductCode
                                oQuote.PartyKey = oGetQuotesMarkedResponse.PartyKey
                                oQuote.PartyName = oGetQuotesMarkedResponse.PartyName
                                oQuote.TotalCommissionLeadAgent = oGetQuotesMarkedResponse.AgentCommission
                                oQuote.InsuranceFileTypeCode = oGetQuotesMarkedResponse.InsuranceFileTypeCode
                                oQuote.CurrencyCode = oGetQuotesMarkedResponse.CurrencyCode
                                oListOfQuotes.Add(oQuote)

                                If Logger.IsLoggingEnabled Then
                                    sbLogMessage.AppendLine("GetQuotesMarkedForCollection executed ok" & vbCrLf)
                                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                                    sbLogMessage.AppendLine("r_oQuote = " & oQuote.Print.Replace("<br />", vbCrLf))


                                    If Not v_iAgentKey = 0 Then
                                        sbLogMessage.AppendLine("v_iAgentKey = " & v_iAgentKey.ToString & vbCrLf)
                                    Else
                                        sbLogMessage.AppendLine("v_iAgentKey = nothing" & vbCrLf)
                                    End If

                                    sbLogMessage.AppendLine("Returned " & oQuote.Risks.Count & " results" & vbCrLf)

                                    LogMessageEntry(sbLogMessage)
                                End If

                            Next

                        End If
                    End If

                End With
                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetQuotesMarkedForCollectionRequest = Nothing
                oGetQuotesMarkedForCollectionResponse = Nothing
            End Try
            Return oListOfQuotes
        End SyncLock
    End Function
    ''' <summary>
    ''' This method is called for adding new quote.
    ''' </summary>
    ''' <param name="r_oQuote"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="v_sSubBranchCode"></param>
    ''' <param name="v_iAgentKey"></param>
    ''' <remarks></remarks>
    Public Overrides Sub AddQuote(ByRef r_oQuote As Quote,
                                        Optional ByVal v_sBranchCode As String = Nothing,
                                        Optional ByVal v_sSubBranchCode As String = Nothing,
                                        Optional ByVal v_iAgentKey As Integer = 0)

        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oAddQuoteRequest As AddQuoteRequestType
            Dim oAddQuoteResponse As AddQuoteResponseType
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oAddQuoteRequest = New AddQuoteRequestType
                oAddQuoteResponse = New AddQuoteResponseType
                sbLogMessage = New StringBuilder


                With oAddQuoteRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                        .SubBranchCode = v_sSubBranchCode

                    End If

                    If r_oQuote.CoverStartDate = DateTime.MinValue Then
                        Throw New ArgumentNullException("Quote.Risk.CoverStartDate")
                    Else
                        .CoverStartDate = r_oQuote.CoverStartDate
                    End If

                    If r_oQuote.CoverEndDate = DateTime.MinValue Then
                        Throw New ArgumentNullException("Quote.Risk.CoverEndDate")
                    Else
                        .CoverEndDate = r_oQuote.CoverEndDate
                    End If

                    .Description = r_oQuote.Description
                    .InsuredName = r_oQuote.InsuredName
                    '.InsuredParties = r_oQuote.InsuredParties

                    If r_oQuote.PartyKey > 0 Then
                        .PartyKey = r_oQuote.PartyKey
                    Else
                        Throw New ArgumentNullException("Quote.PartyKey")
                    End If

                    .QuoteRef = r_oQuote.Reference

                    If v_iAgentKey > 0 Then
                        .AgentKey = v_iAgentKey
                        .AgentKeySpecified = True
                    Else
                        .AgentKeySpecified = False
                    End If

                    .SubBranchCode = v_sSubBranchCode

                    .AnalysisCode = r_oQuote.AnalysisCode
                    If r_oQuote.ConsolidatedLeadAgentCommission > 0 Then
                        .ConsolidatedLeadAgentCommission = r_oQuote.ConsolidatedLeadAgentCommission
                        .ConsolidatedLeadAgentCommissionSpecified = True
                    Else
                        .ConsolidatedLeadAgentCommissionSpecified = False
                    End If
                    If r_oQuote.ConsolidatedSubAgentCommission > 0 Then
                        .ConsolidatedSubAgentCommission = r_oQuote.ConsolidatedSubAgentCommission
                        .ConsolidatedSubAgentCommissionSpecified = True
                    Else
                        .ConsolidatedSubAgentCommissionSpecified = False
                    End If

                    .CurrencyCode = r_oQuote.CurrencyCode
                    .CoverNoteBookNumber = r_oQuote.CoverNoteBookNumber 'addition to existing Function
                    .CoverNoteSheetNumber = r_oQuote.CoverNoteSheetNumber 'addition to existing Function
                    .AlternativeRef = r_oQuote.AlternativeRef

                    If String.IsNullOrEmpty(r_oQuote.ProductCode) Then
                        Throw New ArgumentNullException("Quote.ProductCode")
                    Else
                        .ProductCode = r_oQuote.ProductCode
                    End If

                End With


                Using trace As New Tracer(Category.Trace)
                    oAddQuoteResponse = oSAM.AddQuote(oAddQuoteRequest)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.




                With oAddQuoteResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else

                        r_oQuote.InsuranceFileKey = .InsuranceFileKey
                        r_oQuote.InsuranceFileRef = .InsuranceFileRef
                        r_oQuote.InsuranceFolderKey = .InsuranceFolderKey
                        r_oQuote.ExpiryDate = .QuoteExpiryDate
                        r_oQuote.TimeStamp = .QuoteTimeStamp


                        For i As Integer = 0 To r_oQuote.Risks.Count - 1
                            AddRisk(r_oQuote, i, v_sBranchCode, v_sSubBranchCode)
                        Next

                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("AddQuote executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("r_oQuote = " & r_oQuote.Print.Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If Not IsNothing(v_sSubBranchCode) Then
                        sbLogMessage.AppendLine("v_sSubBranchCode = " & v_sSubBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sSubBranchCode = nothing" & vbCrLf)
                    End If

                    If Not v_iAgentKey = 0 Then
                        sbLogMessage.AppendLine("v_iAgentKey = " & v_iAgentKey.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_iAgentKey = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & r_oQuote.Risks.Count & " results" & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oAddQuoteRequest = Nothing
                oAddQuoteResponse = Nothing
            End Try


        End SyncLock

    End Sub

    ''' <summary>
    ''' To add new risk.
    ''' </summary>
    ''' <param name="r_oQuote"></param>
    ''' <param name="v_iQuoteRiskIndex"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="v_sSubBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub AddRisk(ByRef r_oQuote As Quote,
                                ByVal v_iQuoteRiskIndex As Integer,
                               Optional ByVal v_sBranchCode As String = Nothing,
                                Optional ByVal v_sSubBranchCode As String = Nothing)

        SyncLock oLock
            Dim oSAM As PureServiceClient = Nothing
            Dim oAddRiskRequest As AddRiskRequestType
            Dim oAddRiskResponse As AddRiskResponseType
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oAddRiskRequest = New AddRiskRequestType
                oAddRiskResponse = New AddRiskResponseType
                sbLogMessage = New StringBuilder
                If r_oQuote.Risks Is Nothing Then
                    Throw New ArgumentNullException("Quote.Risk", "Quote must contain one Risk to call Add Risk")
                ElseIf r_oQuote.Risks.Count < 1 Then
                    Throw New ArgumentNullException("Quote.Risk", "Quote must contain one Risk to call Add Risk")
                ElseIf v_iQuoteRiskIndex > r_oQuote.Risks.Count - 1 Then
                    Throw New ArgumentOutOfRangeException("QuoteRiskIndex")
                Else

                    With oAddRiskRequest
                        .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                        'if the passed parameter v_sBranchCode is empty
                        If String.IsNullOrEmpty(v_sBranchCode) Then
                            'if the branch code is NOT in session 
                            If String.IsNullOrEmpty(sBranchCode) Then
                                'Use the default branch code
                                .BranchCode = sDefaultBranchCode
                            Else
                                'Use the branch code in session 
                                .BranchCode = sBranchCode

                            End If
                        Else
                            'use the passed parameter v_sBranchCode
                            .BranchCode = v_sBranchCode
                            .SubBranchCode = v_sSubBranchCode
                        End If

                        If r_oQuote.InsuranceFileKey > 0 Then
                            .InsuranceFileKey = r_oQuote.InsuranceFileKey
                        Else
                            Throw New ArgumentNullException("Quote.InsuranceFileKey")
                        End If

                        If r_oQuote.InsuranceFolderKey > 0 Then
                            .InsuranceFolderKey = r_oQuote.InsuranceFolderKey
                        Else
                            Throw New ArgumentNullException("Quote.InsuranceFolderKey")
                        End If

                        If String.IsNullOrEmpty(r_oQuote.ProductCode) Then
                            Throw New ArgumentNullException("Quote.ProductCode")
                        Else
                            .ProductCode = r_oQuote.ProductCode
                        End If

                        If r_oQuote.TimeStamp Is Nothing Then
                            Throw New ArgumentNullException("Quote.TimeStamp")
                        Else
                            .QuoteTimeStamp = r_oQuote.TimeStamp

                        End If

                        With r_oQuote.Risks(v_iQuoteRiskIndex)

                            If String.IsNullOrEmpty(.DataModelCode) Then
                                Throw New ArgumentNullException("Quote.Risk.DataModelCode")
                            Else
                                oAddRiskRequest.DataModelCode = .DataModelCode
                            End If

                            oAddRiskRequest.RiskDescription = .Description
                            oAddRiskRequest.RiskTypeCode = .RiskCode
                            oAddRiskRequest.ScreenCode = .ScreenCode
                            oAddRiskRequest.XMLDataSet = .XMLDataset

                        End With

                        .RunDefaultRules = True

                        .SubBranchCode = v_sSubBranchCode

                    End With


                    Using trace As New Tracer(Category.Trace)
                        oAddRiskResponse = oSAM.AddRisk(oAddRiskRequest)
                    End Using

                    'NO catches on the try as we want to cascade all exceptions back up the stack for handling.
                    With oAddRiskResponse

                        If .Errors IsNot Nothing Then

                            'Process the error object if errors, and throw as a single exception
                            Throw New NexusException(.Errors)
                        Else

                            r_oQuote.TimeStamp = .QuoteTimeStamp
                            r_oQuote.Risks(v_iQuoteRiskIndex).FolderKey = .RiskFolderKey
                            r_oQuote.Risks(v_iQuoteRiskIndex).Key = .RiskKey
                            r_oQuote.Risks(v_iQuoteRiskIndex).XMLDataset = .XMLDataSet

                        End If

                    End With

                End If

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("AddRisk executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("r_oQuote = " & r_oQuote.Print.Replace("<br />", vbCrLf))
                    sbLogMessage.AppendLine("v_iQuoteRiskIndex = " & v_iQuoteRiskIndex.ToString)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If Not IsNothing(v_sSubBranchCode) Then
                        sbLogMessage.AppendLine("v_sSubBranchCode = " & v_sSubBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sSubBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oAddRiskRequest = Nothing
                oAddRiskResponse = Nothing
            End Try


        End SyncLock

    End Sub

    ''' <summary>
    ''' This method is used to make the policy live.
    ''' Pass the pay now details in the collection when pre payment is disabled.
    ''' </summary>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="v_oPayment"></param>
    ''' <param name="v_bAcceptRenewal"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="v_sTransactionType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function BindQuote(ByVal v_iInsuranceFileKey As Integer,
                                            ByVal v_oPayment As Payment,
                                            ByVal v_bTimeStamp As Byte(),
                                            Optional ByVal v_bAcceptRenewal As Boolean = False,
                                            Optional ByVal v_sBranchCode As String = Nothing,
                                            Optional ByVal v_sTransactionType As String = Nothing,
                                            Optional ByVal v_bIsBackDatedMTA As Boolean = False, Optional ByVal v_bWritePolicy As Boolean = False,
                                            Optional ByVal v_sOverriddenPolicyNumber As String = Nothing,
                                            Optional ByVal v_bPayNegativePremiumMTABalance As Boolean = False) As PolicySummary
        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oBindQuoteRequest As BindQuoteRequestType
            Dim oBindQuoteResponse As BindQuoteResponseType
            Dim oBankGuarantee As BaseBankGuaranteePaymentType
            Dim oBank As BaseBankReceiptType
            Dim oCreditTransaction As BaseBindQuoteRequestTypeCreditTransactionsRow
            'Dim i As Integer
            Dim oCashDeposit As BaseSelectedCashDepositType
            Dim oPolicy As PolicySummary
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oBindQuoteRequest = New BindQuoteRequestType
                oBindQuoteResponse = New BindQuoteResponseType
                oBankGuarantee = New BaseBankGuaranteePaymentType
                oBank = New BaseBankReceiptType
                oCashDeposit = New BaseSelectedCashDepositType
                sbLogMessage = New StringBuilder


                With oBindQuoteRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If

                    If v_iInsuranceFileKey > 0 Then
                        .InsuranceFileKey = v_iInsuranceFileKey
                    Else
                        Throw New ArgumentNullException("InsuranceFileKey")
                    End If

                    .AcceptRenewal = v_bAcceptRenewal
                    .AcceptRenewalSpecified = v_bAcceptRenewal
                    '6389 - MTA Refund Process on Instalments
                    .PayNegativePremiumMTABalance = v_bPayNegativePremiumMTABalance
                    .PayNegativePremiumMTABalanceSpecified = v_bPayNegativePremiumMTABalance

                    If v_oPayment IsNot Nothing Then
                        .PaymentMethod = v_oPayment.PaymentMethod
                        .PaymentMethodSpecified = True

                        .IsBackdatedMTA = v_bIsBackDatedMTA

                        If v_oPayment.BankGuaranteeDetails IsNot Nothing Then
                            If v_oPayment.BankGuaranteeDetails.BGKey > 0 Then

                                oBankGuarantee.BGKey = v_oPayment.BankGuaranteeDetails.BGKey
                                .BankGuaranteeDetails = oBankGuarantee
                            End If
                        End If

                        If v_oPayment.PaymentMethod = PaymentTypes.None Then

                            .SelectedInstalmentQuote = New BaseSelectedInstalmentQuoteType()

                            With .SelectedInstalmentQuote

                                'None, means installment plan and the following values are only used on installment plans

                                .AmountPaid = v_oPayment.AmountPaid
                                .AmountToFinance = v_oPayment.AmountToFinance

                                'Need to set these propertis if selected instalment quote is for Direct Debit 
                                'or Party Bank Key is supplied by Instalments.aspx
                                'If v_oPayment.PartyBankKey > 0 Then
                                .PartyBankKey = v_oPayment.PartyBankKey
                                .BankAccountName = v_oPayment.BankAccountName
                                .BankAccountNo = v_oPayment.BankAccountNo
                                .BankAreaCode = v_oPayment.BankAreaCode
                                .BankBranch = v_oPayment.BankBranch
                                .BankExtn = v_oPayment.BankExtn
                                .BankFax = v_oPayment.BankFax
                                .BankFaxCode = v_oPayment.BankFaxCode
                                .BankName = v_oPayment.BankName
                                .BankPhone = v_oPayment.BankPhone
                                .BankSortCode = v_oPayment.BankSortCode

                                If v_oPayment.BankAddress IsNot Nothing Then
                                    .BankAddress = New BaseAddressType

                                    .BankAddress.AddressTypeCode = v_oPayment.BankAddress.AddressType
                                    .BankAddress.AddressLine1 = v_oPayment.BankAddress.Address1
                                    .BankAddress.AddressLine2 = v_oPayment.BankAddress.Address2
                                    .BankAddress.AddressLine3 = v_oPayment.BankAddress.Address3
                                    .BankAddress.AddressLine4 = v_oPayment.BankAddress.Address4
                                    .BankAddress.PostCode = v_oPayment.BankAddress.PostCode
                                    .BankAddress.CountryCode = v_oPayment.BankAddress.CountryCode

                                End If
                                'End If

                                If v_oPayment.CreditCard IsNot Nothing Then
                                    .CreditCard = New BaseCreditCardType

                                    .CreditCard.Number = v_oPayment.CreditCard.Number
                                    .CreditCard.ExpiryDate = v_oPayment.CreditCard.ExpiryDate
                                    .CreditCard.StartDate = v_oPayment.CreditCard.StartDate
                                    .CreditCard.NameOnCreditCard = v_oPayment.CreditCard.NameOnCreditCard
                                    .CreditCard.TypeCode = v_oPayment.CreditCard.TypeCode
                                    .CreditCard.Issue = v_oPayment.CreditCard.Issue
                                    .CreditCard.Pin = v_oPayment.CreditCard.Pin
                                    If v_oPayment.CreditCard.CreditCardType_CardHolder IsNot Nothing Then
                                        .CreditCard.CardHolder.Name = v_oPayment.CreditCard.CreditCardType_CardHolder.Name
                                    End If
                                    .CreditCard.AuthCode = v_oPayment.CreditCard.AuthCode
                                    .CreditCard.PartyBankKey = v_oPayment.CreditCard.PartyBankKey
                                    'If v_oPayment.PartyBankKey = 0  that means we are currently on CC Plan and in MTA
                                ElseIf Current.Session(Nexus.Constants.CNFinancePlan) IsNot Nothing AndAlso v_oPayment.PartyBankKey = 0 AndAlso
                                (v_oPayment.InstallmentType = InstalmentType.AddAndSpread OrElse v_oPayment.InstallmentType = InstalmentType.AddToNewPlan) Then
                                    Dim oFinancePlan As NexusProvider.FinancePlan = Current.Session(Nexus.Constants.CNFinancePlan)
                                    If oFinancePlan.CreditCardDetails IsNot Nothing AndAlso oFinancePlan.CreditCardDetails.PartyBankKey > 0 Then
                                        .CreditCard = New BaseCreditCardType
                                        .CreditCard.PartyBankKey = oFinancePlan.PartyBankKey
                                    End If
                                End If

                                .EndDate = v_oPayment.EndDate
                                .MonthDay = v_oPayment.MonthDay
                                .OverrideInterestRate = v_oPayment.OverrideInterestRate
                                .OverrideRate = v_oPayment.OverrideRate
                                .PaymentProtection = v_oPayment.PaymentProtection
                                .PreferredDate = v_oPayment.PreferredDate
                                .QuoteDate = v_oPayment.QuoteDate
                                .SelectedSchemeNo = v_oPayment.SelectedSchemeNo
                                .SelectedSchemeVersion = v_oPayment.SelectedSchemeVersion
                                .StartDate = v_oPayment.StartDate
                                .WeekDay = v_oPayment.WeekDay
                                .PFRF_ID = v_oPayment.Pref_ID '.PFRF_ID

                                If v_oPayment.InstallmentTypeSpecified = True Then
                                    Select Case v_oPayment.InstallmentType
                                        Case "0"
                                            oBindQuoteRequest.InstalmentType = InstalmentType.AddAndSpread
                                            oBindQuoteRequest.InstalmentTypeSpecified = True
                                        Case "1"
                                            oBindQuoteRequest.InstalmentType = InstalmentType.AddToNext
                                            oBindQuoteRequest.InstalmentTypeSpecified = True
                                        Case "2"
                                            oBindQuoteRequest.InstalmentType = InstalmentType.AddToNewPlan
                                            oBindQuoteRequest.InstalmentTypeSpecified = True
                                    End Select
                                End If

                            End With
                        End If


                        'New2
                        If v_oPayment.PayNowDetails IsNot Nothing Then

                            .PayNowDetails = New BaseReceiptType()
                            .PayNowDetails.BankAccountName = v_oPayment.PayNowDetails.BankAccountName
                            .PayNowDetails.CurrencyCode = v_oPayment.PayNowDetails.CurrencyCode
                            .PayNowDetails.ReceiptTypeCode = v_oPayment.PayNowDetails.ReceiptTypeCode
                            .PayNowDetails.MediaTypeCode = v_oPayment.PayNowDetails.MediaTypeCode
                            .PayNowDetails.TransactionDate = v_oPayment.PayNowDetails.TransactionDate
                            .PayNowDetails.Amount = v_oPayment.PayNowDetails.Amount
                            .PayNowDetails.CashListRef = v_oPayment.PayNowDetails.CashListRef
                            .PayNowDetails.MediaTypeIssuerCode = v_oPayment.PayNowDetails.MediaTypeIssuerCode
                            .PayNowDetails.OurReference = v_oPayment.PayNowDetails.OurReference
                            .PayNowDetails.TheirReference = v_oPayment.PayNowDetails.TheirReference
                            .PayNowDetails.ContactName = v_oPayment.PayNowDetails.ContactName
                            .PayNowDetails.Address1 = v_oPayment.PayNowDetails.Address1
                            .PayNowDetails.Address2 = v_oPayment.PayNowDetails.Address2
                            .PayNowDetails.Address3 = v_oPayment.PayNowDetails.Address3
                            .PayNowDetails.Address4 = v_oPayment.PayNowDetails.Address4
                            .PayNowDetails.PostalCode = v_oPayment.PayNowDetails.PostalCode
                            .PayNowDetails.CountryCode = v_oPayment.PayNowDetails.CountryCode
                            .PayNowDetails.ChequeName = v_oPayment.PayNowDetails.ChequeName
                            If v_oPayment.PayNowDetails.ChequeDateSpecified Then
                                .PayNowDetails.ChequeDate = v_oPayment.PayNowDetails.ChequeDate
                                .PayNowDetails.ChequeDateSpecified = v_oPayment.PayNowDetails.ChequeDateSpecified
                                .PayNowDetails.MediaReference = v_oPayment.PayNowDetails.InstrumentNumber
                            Else
                                .PayNowDetails.MediaReference = v_oPayment.PayNowDetails.MediaReference
                            End If

                            .PayNowDetails.CollectionDate = v_oPayment.PayNowDetails.CollectionDate
                            .PayNowDetails.CollectionDateSpecified = v_oPayment.PayNowDetails.CollectionDateSpecified
                            .PayNowDetails.Comments = v_oPayment.PayNowDetails.Comments

                            If v_oPayment.PayNowDetails.DraweeBankBranch IsNot Nothing Then

                                oBank.BankBranch = v_oPayment.PayNowDetails.DraweeBankBranch
                                oBank.BankLocation = v_oPayment.PayNowDetails.DraweeBankLocation
                                oBank.BankCode = v_oPayment.PayNowDetails.DraweeBankName
                                oBank.ChequeClearingTypeCode = v_oPayment.PayNowDetails.ChequeClearingType
                                oBank.ChequeTypeCode = v_oPayment.PayNowDetails.ChequeType
                                oBank.ChequeDate = v_oPayment.PayNowDetails.ChequeDate
                                oBank.PayerName = v_oPayment.PayNowDetails.ChequeName

                                .PayNowDetails.Bank = oBank
                            End If

                            .PayNowDetails.CCName = v_oPayment.PayNowDetails.CCName
                            .PayNowDetails.CCNumber = v_oPayment.PayNowDetails.CCNumber
                            .PayNowDetails.CCExpiryDate = v_oPayment.PayNowDetails.CCExpiryDate
                            .PayNowDetails.CCStartDate = v_oPayment.PayNowDetails.CCStartDate
                            .PayNowDetails.CCIssue = v_oPayment.PayNowDetails.CCIssue
                            .PayNowDetails.CCPin = v_oPayment.PayNowDetails.CCPin
                            .PayNowDetails.CCAuthCode = v_oPayment.PayNowDetails.CCAuthCode
                            .PayNowDetails.CCManualAuthCode = v_oPayment.PayNowDetails.CCManualAuthCode
                            .PayNowDetails.CCTransactionCode = v_oPayment.PayNowDetails.CCTransactionCode
                            .PayNowDetails.CCCustomer = v_oPayment.PayNowDetails.CCCustomer
                            .PayNowDetails.CCTypeCode = v_oPayment.PayNowDetails.CCTypeOfCard
                            .PayNowDetails.CCCashListItemBankCode = v_oPayment.PayNowDetails.CCIssueBank
                            .PayNowDetails.CCTransactionSlipNumber = v_oPayment.PayNowDetails.CCSlipNumber

                        End If

                        .TransactionType = v_sTransactionType 'new

                        If v_oPayment.PayTrueMonthlyPolicyMTAPremiumOnRenewal = True Then
                            .PayTrueMonthlyPolicyMTAPremiumOnRenewal = v_oPayment.PayTrueMonthlyPolicyMTAPremiumOnRenewal 'new
                            .PayTrueMonthlyPolicyMTAPremiumOnRenewalSpecified = True
                        Else
                            .PayTrueMonthlyPolicyMTAPremiumOnRenewalSpecified = False
                        End If

                        If v_oPayment.CoverStartDate <> Date.MinValue Then
                            .CoverStartDate = v_oPayment.CoverStartDate 'new
                            .CoverStartDateSpecified = True
                        Else
                            .CoverStartDateSpecified = False
                        End If

                        If v_oPayment.DebitAgainst = Nothing Then
                            .DebitAgainstSpecified = False
                        Else
                            .DebitAgainst = v_oPayment.DebitAgainst 'new
                            .DebitAgainstSpecified = True
                        End If

                        If v_oPayment.CreditTransaction.Count > 0 Then

                            .CreditTransactions = New List(Of BaseBindQuoteRequestTypeCreditTransactionsRow)
                            For i As Integer = 0 To v_oPayment.CreditTransaction.Count - 1
                                oCreditTransaction = New BaseBindQuoteRequestTypeCreditTransactionsRow
                                oCreditTransaction.AccountKey = v_oPayment.CreditTransaction(i).AccountKey
                                oCreditTransaction.TransDetailKey = v_oPayment.CreditTransaction(i).TransDetailKey
                                oCreditTransaction.Amount = v_oPayment.CreditTransaction(i).Amount
                                oCreditTransaction.CollectionDate = v_oPayment.CreditTransaction(i).CollectionDate
                                .CreditTransactions.Add(oCreditTransaction)
                            Next
                        End If

                        If v_oPayment.SelectedCashDeposit IsNot Nothing Then
                            If v_oPayment.SelectedCashDeposit.CashDepositRef IsNot Nothing Then

                                oCashDeposit.CashDepositRef = v_oPayment.SelectedCashDeposit.CashDepositRef

                                .SelectedCashDeposit = oCashDeposit
                            End If
                        End If

                        'New option added 
                        If v_oPayment IsNot Nothing AndAlso String.IsNullOrEmpty(v_oPayment.DebitAgainstAccount) = False Then
                            Select Case v_oPayment.DebitAgainstAccount.Trim.ToUpper
                                Case "AGENT"
                                    .DebitAgainstAccount = DebitAgainstAccountType.Agent
                                    .DebitAgainstAccountSpecified = True
                                Case "CLIENT"
                                    .DebitAgainstAccount = DebitAgainstAccountType.Client
                                    .DebitAgainstAccountSpecified = True
                            End Select
                        End If
                    End If
                    .QuoteTimeStamp = v_bTimeStamp
                    .WritePolicy = v_bWritePolicy
                    .OverriddenPolicyNumber = v_sOverriddenPolicyNumber
                    .TransactionType = v_sTransactionType 'new
                    If System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork").Portals.Portal(HttpContext.Current.Cache("PortalID")).EnableMasterClientAssociate Then
                        .EnableMasterClientAssociate = True
                    Else
                        .EnableMasterClientAssociate = False
                    End If

                End With


                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oBindQuoteResponse = oSAM.BindQuote(oBindQuoteRequest)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                If v_oPayment.PayNowPaymentDetails IsNot Nothing Then

                    .PayNowPaymentDetails = New BasePaymentType()
                    .PayNowPaymentDetails.InsuranceFileRef = v_oPayment.PayNowPaymentDetails.InsuranceFileRef
                    .PayNowPaymentDetails.CashListKey = v_oPayment.PayNowPaymentDetails.CashListKey
                    .PayNowPaymentDetails.CashListItemKey = v_oPayment.PayNowPaymentDetails.CashListItemKey
                    .PayNowPaymentDetails.TransDetailKey = v_oPayment.PayNowPaymentDetails.TransDetailKey
                    .PayNowPaymentDetails.PaymentAccountID = v_oPayment.PayNowPaymentDetails.PaymentAccountID
                    .PayNowPaymentDetails.PaymentTypeCode = v_oPayment.PayNowPaymentDetails.PaymentTypeCode
                    .PayNowPaymentDetails.MediaTypeCode = v_oPayment.PayNowPaymentDetails.MediaTypeCode
                    .PayNowPaymentDetails.MediaReference = v_oPayment.PayNowPaymentDetails.MediaReference
                    .PayNowPaymentDetails.OurReference = v_oPayment.PayNowPaymentDetails.OurReference
                    .PayNowPaymentDetails.TheirReference = v_oPayment.PayNowPaymentDetails.TheirReference

                End If

                .TransactionType = v_sTransactionType 'new





                With oBindQuoteResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        oPolicy = New PolicySummary(.Policy.PolicyRef)
                        oPolicy.CommissionAmount = .Policy.CommissionAmount
                        oPolicy.PremiumDueGross = .Policy.PremiumDueGross
                        oPolicy.PremiumDueNet = .Policy.PremiumDueNet
                        oPolicy.PremiumDueTax = .Policy.PremiumDueTax
                        oPolicy.TotalAnnualTax = .Policy.TotalAnnualTax
                        oPolicy.InstalmentPlanRef = .Policy.AutoGeneratedPlanRef
                        oPolicy.InstdepositTransDetailsId = .Policy.DepositTransDetailID
                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("BindQuote executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iInsuranceFileKey = " & v_iInsuranceFileKey.ToString)
                    If v_oPayment IsNot Nothing Then
                        sbLogMessage.AppendLine("v_oPayment = " & v_oPayment.ToString)
                    End If

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If Not IsNothing(v_sTransactionType) Then
                        sbLogMessage.AppendLine("v_sTransactionType = " & v_sTransactionType.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sTransactionType = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output:" & vbCrLf & oPolicy.Print.Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oBindQuoteRequest = Nothing
                oBindQuoteResponse = Nothing
            End Try


            Return oPolicy

        End SyncLock

    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_iRiskKey"></param>
    ''' <param name="v_iQuoteRiskIndex"></param>
    ''' <param name="r_oQuote"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub GetRisk(ByVal v_iRiskKey As Integer,
                            ByVal v_iQuoteRiskIndex As Integer,
                            ByRef r_oQuote As Quote,
                            Optional ByVal v_sBranchCode As String = Nothing, Optional ByVal v_bIgnoreLocking As Boolean = False)

        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oGetRiskRequest As GetRiskRequestType
            Dim oGetRiskResponse As GetRiskResponseType
            Dim oPolicyFee As Fee
            Dim oPolicyTax As Tax
            Dim oRiskFee As Fee
            Dim oRiskTax As Tax
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetRiskRequest = New GetRiskRequestType
                oGetRiskResponse = New GetRiskResponseType
                sbLogMessage = New StringBuilder


                With oGetRiskRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If

                    If r_oQuote.InsuranceFolderKey > 0 Then
                        .InsuranceFolderKey = r_oQuote.InsuranceFolderKey
                    Else
                        Throw New ArgumentNullException("GetRisk.InsuranceFolderKey")
                    End If

                    If r_oQuote.InsuranceFileKey > 0 Then
                        .InsuranceFileKey = r_oQuote.InsuranceFileKey
                    Else
                        Throw New ArgumentNullException("GetRisk.InsuranceFileKey")
                    End If

                    If v_iRiskKey > 0 Then
                        .RiskKey = v_iRiskKey
                    Else
                        Throw New ArgumentNullException("GetRisk.RiskKey")
                    End If

                    .QuoteTimeStamp = r_oQuote.TimeStamp
                    If Current.Session(Nexus.Constants.CNMode) = Nexus.Constants.Mode.View Then
                        .IgnoreLocking = True
                    Else
                        .IgnoreLocking = v_bIgnoreLocking
                    End If

                End With


                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oGetRiskResponse = oSAM.GetRisk(oGetRiskRequest)
                End Using



                With oGetRiskResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else

                        r_oQuote.Risks(v_iQuoteRiskIndex).PremiumDueNet = .PremiumDueNet
                        r_oQuote.Risks(v_iQuoteRiskIndex).PremiumDueTax = .PremiumDueTax
                        r_oQuote.Risks(v_iQuoteRiskIndex).PremiumDueGross = .PremiumDueGross
                        r_oQuote.Risks(v_iQuoteRiskIndex).TotalAnnualTax = .TotalAnnualTax
                        r_oQuote.Risks(v_iQuoteRiskIndex).CommissionAmount = .CommissionAmount

                        r_oQuote.Risks(v_iQuoteRiskIndex).XMLDataset = .XMLDataSet

                        r_oQuote.TimeStamp = .QuoteTimeStamp
                        r_oQuote.PolicyLevelTax = .PolicyLevelTax
                        r_oQuote.ProRataRate = .proRataRate
                        If .PolicyLevelTaxesAndFees.Fees IsNot Nothing Then


                            For Each v_oPolicyFees As BaseFeesType In .PolicyLevelTaxesAndFees.Fees
                                oPolicyFee = New Fee
                                oPolicyFee.FeeAmount = v_oPolicyFees.Amount
                                oPolicyFee.Description = v_oPolicyFees.Description

                                r_oQuote.PolicyFees.Add(oPolicyFee)

                            Next
                        End If

                        If .PolicyLevelTaxesAndFees.Taxes IsNot Nothing Then


                            For Each v_oPolicyTaxes As BaseTaxesType In .PolicyLevelTaxesAndFees.Taxes
                                oPolicyTax = New Tax
                                oPolicyTax.TaxAmount = v_oPolicyTaxes.Amount
                                oPolicyTax.Description = v_oPolicyTaxes.Description

                                r_oQuote.PolicyTaxes.Add(oPolicyTax)

                            Next
                        End If
                        If .RiskLevelTaxesAndFees.Fees IsNot Nothing Then


                            For Each v_oRiskFees As BaseFeesType In .RiskLevelTaxesAndFees.Fees
                                oRiskFee = New Fee
                                oRiskFee.FeeAmount = v_oRiskFees.Amount
                                oRiskFee.Description = v_oRiskFees.Description

                                r_oQuote.RiskFees.Add(oRiskFee)
                            Next
                        End If

                        If .RiskLevelTaxesAndFees.Taxes IsNot Nothing Then


                            For Each v_oRiskTaxes As BaseTaxesType In .RiskLevelTaxesAndFees.Taxes
                                oRiskTax = New Tax
                                oRiskTax.TaxAmount = v_oRiskTaxes.Amount
                                oRiskTax.Description = v_oRiskTaxes.Description

                                r_oQuote.RiskTaxes.Add(oRiskTax)
                            Next
                        End If
                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetRisk executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("r_oQuote = " & r_oQuote.Print.Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If
                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetRiskRequest = Nothing
                oGetRiskResponse = Nothing
            End Try


        End SyncLock

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_sProductCode"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetRiskByProduct(Optional ByVal v_sProductCode As String = Nothing,
                                                Optional ByVal v_sBranchCode As String = Nothing) As RiskCollection
        SyncLock oLock
            Dim oSAM As PureServiceClient 'SAMForInsuranceV2's Object
            Dim oGetRiskByProductRequest As GetRiskByProductRequestType ' Request Type
            Dim oGetRiskByProductResponse As GetRiskByProductResponseType ' Response Type
            Dim oRisks As RiskCollection = Nothing
            Dim oNewRisk As Risk 'Object of Risk Class
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetRiskByProductRequest = New GetRiskByProductRequestType
                oGetRiskByProductResponse = New GetRiskByProductResponseType
                sbLogMessage = New StringBuilder


                With oGetRiskByProductRequest 'with Request Type
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'Checking the BranchCode
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If
                    .ProductCode = v_sProductCode
                End With

                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oGetRiskByProductResponse = oSAM.GetRiskByProduct(oGetRiskByProductRequest)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                ' Disposing the SAM's object

                'Risk Collection Object and their Initialization

                oRisks = New RiskCollection

                With oGetRiskByProductResponse 'With Response Type
                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else

                        For Each oRisk As BaseGetRiskByProductResponseTypeRow In .Risks 'Fetching from the  Risk Response Collection
                            oNewRisk = New Risk()
                            With oNewRisk
                                .Description = oRisk.Description
                                .Key = oRisk.RiskTypeKey 'Risk Key
                                .DataModelCode = oRisk.DataModelCode
                                .RiskTypeCode = oRisk.RiskTypeCode
                                .ScreenCode = oRisk.ScreenCode
                                .ScreenKey = oRisk.ScreenKey
                            End With
                            oRisks.Add(oNewRisk) 'Adding into the Risk Collection
                        Next
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetRiskByProduct executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    If Not IsNothing(v_sProductCode) Then
                        sbLogMessage.AppendLine("v_sProductCode = " & v_sProductCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sProductCode = nothing" & vbCrLf)
                    End If

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & oRisks.Count.ToString & " results" & vbCrLf)
                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)
                'FaultErrorHandler(ex) ' handling fault error messages 
            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetRiskByProductRequest = Nothing
                oGetRiskByProductResponse = Nothing
            End Try


            Return oRisks 'Returning Risk Collection
        End SyncLock
    End Function

    ''' <summary>
    ''' This method is called for adding new quote.
    ''' </summary>
    ''' <param name="r_oQuoteV2"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="v_sSubBranchCode"></param>
    ''' <param name="v_iAgentKey"></param>
    ''' <remarks></remarks>
    Public Overrides Sub AddQuoteV2(ByRef r_oQuoteV2 As Quote,
                                Optional ByVal v_sBranchCode As String = Nothing,
                                Optional ByVal v_sSubBranchCode As String = Nothing,
                                Optional ByVal v_iAgentKey As Integer = 0)


        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oAddQuoteV2Request As AddQuoteV2RequestType
            Dim oAddQuoteV2Response As AddQuoteV2ResponseType
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oAddQuoteV2Request = New AddQuoteV2RequestType
                oAddQuoteV2Response = New AddQuoteV2ResponseType
                sbLogMessage = New StringBuilder


                With oAddQuoteV2Request
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode

                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    If r_oQuoteV2.PartyKey > 0 Then
                        .PartyKey = r_oQuoteV2.PartyKey
                    Else
                        Throw New ArgumentNullException("Quote.PartyKey")
                    End If

                    .CoverStartDate = r_oQuoteV2.CoverStartDate
                    .CoverEndDate = r_oQuoteV2.CoverEndDate
                    .ProductCode = r_oQuoteV2.ProductCode
                    .CurrencyCode = r_oQuoteV2.CurrencyCode
                    If r_oQuoteV2.AccountHandlerCnt > 0 Then
                        .AccountHandlerCnt = r_oQuoteV2.AccountHandlerCnt
                        .AccountHandlerCntSpecified = True
                    Else
                        .AccountHandlerCntSpecified = False
                    End If

                    If r_oQuoteV2.Agent IsNot Nothing Then
                        If r_oQuoteV2.Agent.Trim.Length <> 0 Then
                            .AgentKey = r_oQuoteV2.Agent
                            .AgentKeySpecified = True
                        Else
                            .AgentKeySpecified = False
                        End If
                    Else
                        .AgentKeySpecified = False
                    End If
                    .AlternativeRef = r_oQuoteV2.AlternativeRef
                    .AnalysisCode = r_oQuoteV2.AnalysisCode
                    .SubBranchCode = r_oQuoteV2.SubBranchCode
                    If r_oQuoteV2.ConsolidatedLeadAgentCommission > 0 Then
                        .ConsolidatedLeadAgentCommission = r_oQuoteV2.ConsolidatedLeadAgentCommission
                        .ConsolidatedLeadAgentCommissionSpecified = True
                    Else
                        .ConsolidatedLeadAgentCommissionSpecified = False
                    End If
                    If r_oQuoteV2.ConsolidatedSubAgentCommission > 0 Then
                        .ConsolidatedSubAgentCommission = r_oQuoteV2.ConsolidatedSubAgentCommission
                        .ConsolidatedSubAgentCommissionSpecified = True
                    Else
                        .ConsolidatedSubAgentCommissionSpecified = False
                    End If
                    .CoverNoteBookNumber = r_oQuoteV2.CoverNoteBookNumber
                    If r_oQuoteV2.CoverNoteSheetNumber > 0 Then
                        .CoverNoteSheetNumber = r_oQuoteV2.CoverNoteSheetNumber
                        .CoverNoteSheetNumberSpecified = True
                    Else
                        .CoverNoteSheetNumberSpecified = False
                    End If

                    .Description = r_oQuoteV2.Description
                    .FrequencyCode = r_oQuoteV2.FrequencyCode
                    .BusinessTypeCode = r_oQuoteV2.BusinessTypeCode
                    .HandlerCode = r_oQuoteV2.HandlerCode
                    .Regarding = r_oQuoteV2.Regarding
                    .PolicyStatusCode = r_oQuoteV2.PolicyStatusCode
                    .InceptionDate = r_oQuoteV2.InceptionDate
                    .InsuredName = r_oQuoteV2.InsuredName
                    .RenewalDate = r_oQuoteV2.RenewalDate
                    .InceptionTPI = r_oQuoteV2.InceptionTPI
                    .QuoteExpiryDate = r_oQuoteV2.QuoteExpiryDate
                    If r_oQuoteV2.IssuedDate <> Date.MinValue Then
                        .IssuedDate = r_oQuoteV2.IssuedDate
                        .IssuedDateSpecified = True
                    Else
                        .IssuedDateSpecified = False

                    End If

                    .PartyKey = r_oQuoteV2.PartyKey
                    .ProductCode = r_oQuoteV2.ProductCode

                    If r_oQuoteV2.ProposalDate <> Date.MinValue Then
                        .ProposalDate = r_oQuoteV2.ProposalDate
                        .ProposalDateSpecified = True
                    Else
                        .ProposalDateSpecified = False
                    End If

                    .RenewalMethodCode = r_oQuoteV2.RenewalMethodCode

                    If r_oQuoteV2.LapseCancelDate <> Date.MinValue Then
                        .LapseCancelDate = r_oQuoteV2.LapseCancelDate
                        .LapseCancelDateSpecified = True
                        .LapseCancelReasonCode = r_oQuoteV2.LapseCancelReasonCode
                    Else
                        .LapseCancelDateSpecified = False
                    End If

                    If r_oQuoteV2.LTUExpiryDate <> Date.MinValue Then
                        .LTUExpiryDate = r_oQuoteV2.LTUExpiryDate
                        .LTUExpiryDateSpecified = True
                    Else
                        .LTUExpiryDateSpecified = False
                    End If

                    .StopReasonCode = r_oQuoteV2.StopReasonCode
                    If r_oQuoteV2.ReferredAtRenewal > 0 Then
                        .ReferredAtRenewal = r_oQuoteV2.ReferredAtRenewal
                        .ReferredAtRenewalSpecified = True
                    Else
                        .ReferredAtRenewalSpecified = False
                    End If


                    If r_oQuoteV2.ReferredAtMTA > 0 Then
                        .ReferredAtMTA = r_oQuoteV2.ReferredAtMTA
                        .ReferredAtMTASpecified = True
                    Else
                        .ReferredAtMTASpecified = False
                    End If
                    If r_oQuoteV2.RenewalCount > 0 Then
                        .RenewalCount = r_oQuoteV2.RenewalCount
                        .RenewalCountSpecified = True
                    Else
                        .RenewalCountSpecified = False
                    End If
                    .Regarding = r_oQuoteV2.Regarding
                    .RenewalMethodCode = r_oQuoteV2.RenewalMethodCode

                    If r_oQuoteV2.RenewalDayNo > 0 Then
                        .RenewalDayNo = r_oQuoteV2.RenewalDayNo
                        .RenewalDayNoSpecified = True
                    Else
                        .RenewalDayNoSpecified = False
                    End If

                    If r_oQuoteV2.ConsolidatedLeadAgentCommission = True Then
                        .ConsolidatedLeadAgentCommission = r_oQuoteV2.ConsolidatedLeadAgentCommission
                        .ConsolidatedLeadAgentCommissionSpecified = True
                    Else
                        .ConsolidatedLeadAgentCommissionSpecified = False
                    End If

                    If r_oQuoteV2.ConsolidatedSubAgentCommission = True Then
                        .ConsolidatedSubAgentCommission = r_oQuoteV2.ConsolidatedSubAgentCommission
                        .ConsolidatedSubAgentCommissionSpecified = True
                    Else
                        .ConsolidatedSubAgentCommissionSpecified = False
                    End If


                End With

                Using trace As New Tracer(Category.Trace)
                    oAddQuoteV2Response = oSAM.AddQuoteV2(oAddQuoteV2Request)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.



                With oAddQuoteV2Response

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        r_oQuoteV2.InsuranceFileKey = .InsuranceFileKey
                        r_oQuoteV2.InsuranceFileRef = .InsuranceFileRef
                        r_oQuoteV2.InsuranceFolderKey = .InsuranceFolderKey
                        r_oQuoteV2.ExpiryDate = .QuoteExpiryDate
                        r_oQuoteV2.TimeStamp = .QuoteTimeStamp

                        If r_oQuoteV2.Risks IsNot Nothing Then
                            For i As Integer = 0 To r_oQuoteV2.Risks.Count - 1
                                If r_oQuoteV2.Risks(i).IsMandatoryRisk = False Then
                                    AddRisk(r_oQuoteV2, i, v_sBranchCode, v_sSubBranchCode)
                                    If .IsMandatoryRisk = True AndAlso String.IsNullOrEmpty(.XMLDataSet) = False Then
                                        Dim oRisk As New NexusProvider.Risk(r_oQuoteV2.Risks(i).ScreenCode, r_oQuoteV2.Risks(i).Description)
                                        oRisk.Key = .RiskKey
                                        oRisk.FolderKey = .RiskFolderKey
                                        oRisk.XMLDataset = .XMLDataSet
                                        r_oQuoteV2.Risks.Add(oRisk)
                                    End If
                                ElseIf r_oQuoteV2.Risks(i).IsMandatoryRisk = True Then
                                    If .IsMandatoryRisk = True AndAlso String.IsNullOrEmpty(.XMLDataSet) = False Then
                                        r_oQuoteV2.Risks(i).Key = .RiskKey
                                        r_oQuoteV2.Risks(i).FolderKey = .RiskFolderKey
                                        r_oQuoteV2.Risks(i).XMLDataset = .XMLDataSet
                                    End If
                                End If
                            Next
                        End If
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("AddQuote executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("r_oQuote = " & r_oQuoteV2.Print.Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If Not IsNothing(v_sSubBranchCode) Then
                        sbLogMessage.AppendLine("v_sSubBranchCode = " & v_sSubBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sSubBranchCode = nothing" & vbCrLf)
                    End If

                    If Not v_iAgentKey = 0 Then
                        sbLogMessage.AppendLine("v_iAgentKey = " & v_iAgentKey.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_iAgentKey = nothing" & vbCrLf)
                    End If
                    .CoInsurancePlacement = r_oQuoteV2.CoinsurancePlacement

                    sbLogMessage.AppendLine("Returned " & r_oQuoteV2.Risks.Count.ToString & " results" & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oAddQuoteV2Request = Nothing
                oAddQuoteV2Response = Nothing
            End Try

        End SyncLock
    End Sub

    ''' <summary>
    ''' To get unpaid premium for the selected insurance file.
    ''' </summary>
    ''' <param name="v_sInsuranceRef"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function CheckUnpaidPremium(ByVal v_sInsuranceRef As String,
                                                  Optional ByVal v_sBranchCode As String = Nothing) As TransactionCollection
        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oCheckUnpaidPremiumRequest As CheckUnpaidPremiumRequestType
            Dim oCheckUnpaidPremiumResponse As CheckUnpaidPremiumResponseType
            Dim otransactionsCollection As TransactionCollection
            Dim oTransactions As Transaction
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oCheckUnpaidPremiumRequest = New CheckUnpaidPremiumRequestType
                oCheckUnpaidPremiumResponse = New CheckUnpaidPremiumResponseType
                otransactionsCollection = New TransactionCollection
                oTransactions = New Transaction
                sbLogMessage = New StringBuilder


                With oCheckUnpaidPremiumRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    .InsuranceRef = v_sInsuranceRef
                End With


                Using trace As New Tracer(Category.Trace)
                    oCheckUnpaidPremiumResponse = oSAM.CheckUnpaidPremium(oCheckUnpaidPremiumRequest)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                With oCheckUnpaidPremiumResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If

                    For Each oBaseCheckUnpaidPremiumResponseTypeRow As BaseCheckUnpaidPremiumResponseTypeRow In .Transactions

                        oTransactions.Amount = oBaseCheckUnpaidPremiumResponseTypeRow.amount
                        oTransactions.ShortCode = oBaseCheckUnpaidPremiumResponseTypeRow.short_code
                        oTransactions.DocRef = oBaseCheckUnpaidPremiumResponseTypeRow.document_ref
                        oTransactions.DocumentDate = oBaseCheckUnpaidPremiumResponseTypeRow.document_date
                        oTransactions.OSAmount = oBaseCheckUnpaidPremiumResponseTypeRow.outstanding
                        oTransactions.Branchcode = oBaseCheckUnpaidPremiumResponseTypeRow.BranchCode
                        oTransactions.BranchDescription = oBaseCheckUnpaidPremiumResponseTypeRow.BranchDescription
                        oTransactions.DocumentType = oBaseCheckUnpaidPremiumResponseTypeRow.document_type

                        otransactionsCollection.Add(oTransactions)
                    Next
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("CheckUnpaidPremium executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input :" & vbCrLf)
                    sbLogMessage.AppendLine("v_sInsuranceRef = " & v_sInsuranceRef.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oCheckUnpaidPremiumRequest = Nothing
                oCheckUnpaidPremiumResponse = Nothing
            End Try


            Return otransactionsCollection

        End SyncLock
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_oQoute"></param>
    ''' <param name="v_iRiskIndex"></param>
    ''' <param name="v_sCopyType"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub CopyRisk(ByRef r_oQoute As Quote,
                                  ByVal v_iRiskIndex As Integer,
                                  ByVal v_sCopyType As CopyRiskTypes,
                                  Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oCopyRiskRequest As CopyRiskRequestType
            Dim oCopyRiskResponse As CopyRiskResponseType
            Dim oRisk As New NexusProvider.Risk(r_oQoute.Risks(v_iRiskIndex).ScreenCode, r_oQoute.Risks(v_iRiskIndex).Description)
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oCopyRiskRequest = New CopyRiskRequestType
                oCopyRiskResponse = New CopyRiskResponseType
                sbLogMessage = New StringBuilder


                With oCopyRiskRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    .InsuranceFileKey = r_oQoute.InsuranceFileKey
                    .InsuranceFolderKey = r_oQoute.InsuranceFolderKey
                    .RiskKey = r_oQoute.Risks(v_iRiskIndex).Key
                    .RiskNumber = r_oQoute.Risks(v_iRiskIndex).RiskNumber

                    Select Case v_sCopyType
                        Case CopyRiskType.Comparative
                            .CopyType = CopyRiskType.Comparative
                        Case CopyRiskType.Duplicate
                            .CopyType = CopyRiskType.Duplicate
                    End Select
                End With

                Using trace As New Tracer(Category.Trace)
                    oCopyRiskResponse = oSAM.CopyRisk(oCopyRiskRequest)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.



                With oCopyRiskResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If


                    oRisk.DataModelCode = r_oQoute.Risks(v_iRiskIndex).DataModelCode
                    If r_oQoute.Risks(v_iRiskIndex).RiskCode Is Nothing Then
                        oRisk.RiskCode = r_oQoute.Risks(v_iRiskIndex).RiskTypeCode
                        oRisk.RiskTypeCode = r_oQoute.Risks(v_iRiskIndex).RiskTypeCode
                    Else
                        oRisk.RiskCode = r_oQoute.Risks(v_iRiskIndex).RiskCode
                        oRisk.RiskTypeCode = r_oQoute.Risks(v_iRiskIndex).RiskCode
                    End If
                    r_oQoute.Risks.Add(oRisk)
                    r_oQoute.Risks(r_oQoute.Risks.Count - 1).Key = .RiskKey
                    r_oQoute.TimeStamp = .QuoteTimeStamp

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetBankGuarantee executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input :" & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & r_oQoute.Print.Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oCopyRiskRequest = Nothing
                oCopyRiskResponse = Nothing
            End Try

        End SyncLock

    End Sub

    ''' <summary>
    ''' To delete the risk.
    ''' </summary>
    ''' <param name="r_oQuote"></param>
    ''' <param name="v_iRiskIndex"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub DeleteRisk(ByRef r_oQuote As Quote,
                                    ByVal v_iRiskIndex As Integer,
                                    Optional ByVal v_sBranchCode As String = Nothing,
                                    Optional ByVal v_sTransactionType As String = Nothing,
                                    Optional ByVal v_iRiskKey As Integer = 0)
        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oDeleteRiskkRequest As DeleteRiskRequestType
            Dim oDeleteRiskResponse As DeleteRiskResponseType
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oDeleteRiskkRequest = New DeleteRiskRequestType
                oDeleteRiskResponse = New DeleteRiskResponseType
                sbLogMessage = New StringBuilder


                With oDeleteRiskkRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    .InsuranceFileKey = r_oQuote.InsuranceFileKey
                    .InsuranceFolderKey = r_oQuote.InsuranceFolderKey
                    .RiskKey = r_oQuote.Risks(v_iRiskIndex).Key
                    .QuoteTimeStamp = r_oQuote.TimeStamp
                    If v_iRiskKey <> 0 Then
                        .RiskKey = v_iRiskKey
                    Else
                        .RiskKey = r_oQuote.Risks(v_iRiskIndex).Key
                    End If
                    .QuoteTimeStamp = r_oQuote.TimeStamp
                    Select Case v_sTransactionType
                        Case "NB"
                            .TransactionType = TransactionType.NB
                        Case "MTA"
                            .TransactionType = TransactionType.MTA
                        Case "MTC"
                            .TransactionType = TransactionType.MTC
                        Case "MTR"
                            .TransactionType = TransactionType.MTR
                        Case "REN"
                            .TransactionType = TransactionType.REN
                    End Select
                    .OrignalRiskKey = r_oQuote.Risks(v_iRiskIndex).OriginalRiskKey
                End With

                Using trace As New Tracer(Category.Trace)
                    oDeleteRiskResponse = oSAM.DeleteRisk(oDeleteRiskkRequest)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.



                With oDeleteRiskResponse
                        If .Errors IsNot Nothing Then
                            'Process the error object if errors, and throw as a single exception
                            Throw New NexusException(.Errors)
                        End If

                    r_oQuote.TimeStamp = .QuoteTimeStamp
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetBankGuarantee executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input :" & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & r_oQuote.Print.Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                    Throw
                Finally
                    oSAM.Close()
                    oDeleteRiskkRequest = Nothing
                    oDeleteRiskResponse = Nothing
                End Try

        End SyncLock

    End Sub

    ''' <summary>
    ''' This method is used to get the coinsurance values for the insurance file.
    ''' This method is called if the policy is a coinsurance policy.
    ''' </summary>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetCoInsuranceValues(ByVal v_iInsuranceFileKey As Integer,
                                                   Optional ByVal v_sBranchCode As String = Nothing) As CoinsuranceDefaults
        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oGetCoinsuranceValuesRequest As GetCoinsuranceValuesRequestType
            Dim oGetCoinsuranceValuesResponse As GetCoinsuranceValuesResponseType
            Dim oCoInsurersDefault As CoinsuranceDefaults
            Dim oCoInsurers As CoInsurers
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetCoinsuranceValuesRequest = New GetCoinsuranceValuesRequestType
                oGetCoinsuranceValuesResponse = New GetCoinsuranceValuesResponseType
                oCoInsurersDefault = New CoinsuranceDefaults
                sbLogMessage = New StringBuilder


                With oGetCoinsuranceValuesRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode

                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    .InsuranceFileKey = v_iInsuranceFileKey
                End With

                Using trace As New Tracer(Category.Trace)
                    oGetCoinsuranceValuesResponse = oSAM.GetCoinsuranceValues(oGetCoinsuranceValuesRequest)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.






                With oGetCoinsuranceValuesResponse

                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If

                    oCoInsurersDefault.IsRecovered = .IsRecovered
                    oCoInsurersDefault.IsSurcharged = .IsSurcharged
                    oCoInsurersDefault.CoinsuranceDefaultId = .DefaultId

                    If .CoInsurers IsNot Nothing Then
                        For Each oCoInsurersRow As BaseGetCoinsuranceValuesResponseTypeRow In .CoInsurers
                            oCoInsurers = New CoInsurers

                            oCoInsurers.ArrangementRef = oCoInsurersRow.ArrangementRef
                            oCoInsurers.CoInsurerKey = oCoInsurersRow.CoInsurerKey
                            oCoInsurers.CoInsurer = oCoInsurersRow.CoInsurer
                            oCoInsurers.SharePerc = oCoInsurersRow.SharePerc
                            oCoInsurers.CommissionPerc = oCoInsurersRow.CommissionPerc

                            oCoInsurersDefault.CoInsurer.Add(oCoInsurers)
                        Next
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetCoInsuranceValues executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input :" & vbCrLf)
                    'sbLogMessage.AppendLine("v_iKey = " & v_iKey.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    'sbLogMessage.AppendLine("Returned " & oCoInsurersDefault.Print.Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetCoinsuranceValuesRequest = Nothing
                oGetCoinsuranceValuesResponse = Nothing
            End Try


            Return oCoInsurersDefault
        End SyncLock
    End Function

    ''' <summary>
    ''' To get the coinsurance defaults. This method is called if the policy is a coinsurance policy.
    ''' </summary>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetCoinsuranceDefaults(Optional ByVal v_sBranchCode As String = Nothing) As CoinsuranceDefaultsCollection
        SyncLock oLock
            Dim oSAM As PureServiceClient
            Dim oGetCoinsuranceDefaultsRequest As GetCoinsuranceDefaultsRequestType
            Dim oGetCoinsuranceDefaultsResponse As GetCoinsuranceDefaultsResponseType
            Dim oCoInsuranceDefaultCollection As CoinsuranceDefaultsCollection
            Dim oCoInsuranceDefault As CoinsuranceDefaults
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetCoinsuranceDefaultsRequest = New GetCoinsuranceDefaultsRequestType
                oGetCoinsuranceDefaultsResponse = New GetCoinsuranceDefaultsResponseType
                oCoInsuranceDefaultCollection = New CoinsuranceDefaultsCollection
                sbLogMessage = New StringBuilder


                With oGetCoinsuranceDefaultsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If

                End With


                Using trace As New Tracer(Category.Trace)
                    oGetCoinsuranceDefaultsResponse = oSAM.GetCoinsuranceDefaults(oGetCoinsuranceDefaultsRequest)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.





                With oGetCoinsuranceDefaultsResponse

                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If



                    For Each oCoInsurersDefaultRow As BaseGetCoinsuranceDefaultsResponseTypeRow In .Defaults

                        oCoInsuranceDefault = New CoinsuranceDefaults
                        oCoInsuranceDefault.Code = oCoInsurersDefaultRow.Code
                        oCoInsuranceDefault.CoinsuranceDefault = oCoInsurersDefaultRow.CoinsuranceDefault
                        oCoInsuranceDefault.CoinsuranceDefaultId = oCoInsurersDefaultRow.CoinsuranceDefaultId
                        oCoInsuranceDefault.IsRecovered = oCoInsurersDefaultRow.IsRecovered
                        oCoInsuranceDefault.IsSurcharged = oCoInsurersDefaultRow.IsSurcharged

                        oCoInsuranceDefaultCollection.Add(oCoInsuranceDefault)
                    Next

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetCoInsuranceValues executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input :" & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetCoinsuranceDefaultsRequest = Nothing
                oGetCoinsuranceDefaultsResponse = Nothing
            End Try


            Return oCoInsuranceDefaultCollection
        End SyncLock
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_oRisk"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub SaveRisk(ByRef r_oQuote As Quote, ByVal v_iQuoteRiskIndex As Integer,
                                            Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oSaveRiskRequest As SaveRiskRequestType
            Dim oSaveRiskResponse As SaveRiskResponseType
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oSaveRiskRequest = New SaveRiskRequestType
                oSaveRiskResponse = New SaveRiskResponseType
                sbLogMessage = New StringBuilder


                With oSaveRiskRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    If r_oQuote.InsuranceFileKey > 0 Then
                        .InsuranceFileKey = r_oQuote.InsuranceFileKey
                    Else
                        Throw New ArgumentException("SaveRisk.InsuranceFileKey")
                    End If

                    If r_oQuote.InsuranceFolderKey > 0 Then
                        .InsuranceFolderKey = r_oQuote.InsuranceFolderKey
                    Else
                        Throw New ArgumentException("SaveRisk.InsuranceFolderKey")
                    End If

                    If r_oQuote.Risks(v_iQuoteRiskIndex).Key > 0 Then
                        .RiskKey = r_oQuote.Risks(v_iQuoteRiskIndex).Key
                    Else
                        Throw New ArgumentException("SaveRisk.RiskKey")
                    End If

                    .XMLDataSet = r_oQuote.Risks(v_iQuoteRiskIndex).XMLDataset

                    .QuoteTimeStamp = r_oQuote.TimeStamp
                End With


                Using trace As New Tracer(Category.Trace)
                    oSaveRiskResponse = oSAM.SaveRisk(oSaveRiskRequest)
                End Using




                With oSaveRiskResponse

                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If

                    r_oQuote.Risks(v_iQuoteRiskIndex).XMLDataset = .XMLDataSet
                    r_oQuote.TimeStamp = .QuoteTimeStamp

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("SaveRisk Executed ok" & vbCrLf)

                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    sbLogMessage.AppendLine("r_oQuote = " & r_oQuote.Print.Replace("<br />", vbCrLf))

                    If IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("Branch Code : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Branch Code : " & v_sBranchCode.ToString & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output : " & vbCrLf)

                    If r_oQuote.Risks(v_iQuoteRiskIndex).XMLDataset IsNot Nothing Then
                        sbLogMessage.AppendLine("XMLDataSet : " & r_oQuote.Risks(v_iQuoteRiskIndex).XMLDataset & vbCrLf)
                    End If

                    If r_oQuote.Risks(v_iQuoteRiskIndex).QuoteTimeStamp IsNot Nothing Then
                        sbLogMessage.AppendLine("QuoteTimeStamp : " & r_oQuote.Risks(v_iQuoteRiskIndex).QuoteTimeStamp.ToString & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oSaveRiskRequest = Nothing
                oSaveRiskResponse = Nothing
            End Try


        End SyncLock
    End Sub

    ''' <summary>
    ''' To get all the standard policy wordings for the selected policy file.
    ''' </summary>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetStandardPolicyWordings(ByVal v_iInsuranceFileKey As Integer,
                                                       Optional ByVal v_bGetFreshPolicyStandardWording As Boolean = False,
                                                       Optional ByVal v_sBranchCode As String = Nothing) As DocumentTemplateCollection

        SyncLock oLock

            Dim oSAM As PureServiceClient = Nothing
            Dim oGetStandardPolicyWordingsRequest As GetStandardPolicyWordingsRequestType
            Dim oGetStandardPolicyWordingsResponse As GetStandardPolicyWordingsResponseType
            Dim oDocumentTemplatesCollection As DocumentTemplateCollection = Nothing
            Dim oDocumentTemplates As DocumentTemplate
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetStandardPolicyWordingsRequest = New GetStandardPolicyWordingsRequestType
                oGetStandardPolicyWordingsResponse = New GetStandardPolicyWordingsResponseType
                oDocumentTemplatesCollection = New DocumentTemplateCollection
                sbLogMessage = New StringBuilder


                With oGetStandardPolicyWordingsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode

                    End If

                    If v_iInsuranceFileKey > 0 Then
                        .InsuranceFileKey = v_iInsuranceFileKey
                    Else
                        Throw New ArgumentException("GetStandardPolicyWordings.InsuranceFileKey")
                    End If
                    .GetFreshPolicyStandardWording = v_bGetFreshPolicyStandardWording
                End With


                Using trace As New Tracer(Category.Trace)
                    oGetStandardPolicyWordingsResponse = oSAM.GetStandardPolicyWordings(oGetStandardPolicyWordingsRequest)
                End Using

                If oGetStandardPolicyWordingsResponse IsNot Nothing Then
                    With oGetStandardPolicyWordingsResponse
                        If .DocumentTemplates IsNot Nothing Then
                            For Each oDocumentTemplatesTypeRow As BaseGetStandardPolicyWordingsResponseTypeRow In .DocumentTemplates
                                oDocumentTemplates = New DocumentTemplate
                                With oDocumentTemplates
                                    .DocumentTemplateId = oDocumentTemplatesTypeRow.DocumentTemplateId
                                    .Code = oDocumentTemplatesTypeRow.Code
                                    .Description = oDocumentTemplatesTypeRow.Description
                                End With

                                oDocumentTemplatesCollection.Add(oDocumentTemplates)
                            Next
                        End If
                    End With
                End If

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetStandardPolicyWordings executed ok" & vbCrLf)

                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    If IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("Branch Code : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Branch Code : " & v_sBranchCode.ToString & vbCrLf)
                    End If

                    If IsNothing(v_iInsuranceFileKey) Then
                        sbLogMessage.AppendLine("Insurance File Key : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Insurance File Key : " & v_iInsuranceFileKey.ToString & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output : " & vbCrLf)
                    sbLogMessage.AppendLine(oDocumentTemplatesCollection.Print().Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetStandardPolicyWordingsRequest = Nothing
                oGetStandardPolicyWordingsResponse = Nothing
            End Try

            Return oDocumentTemplatesCollection
        End SyncLock
    End Function

    ''' <summary>
    ''' To update the standard policy wordings associated with the quote.
    ''' Addition / Deletion of the standard policy wording is done by the same method.
    ''' </summary>
    ''' <param name="v_oDocumentTemplate"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function UpdateStandardPolicyWordings(ByRef v_oDocumentTemplate As DocumentTemplate,
                                                  Optional ByVal v_sBranchCode As String = Nothing) As DocumentTemplate

        SyncLock oLock


            Dim oSAM As PureServiceClient 'SAMForInsuranceV2's Object
            Dim oUpdateStandardPolicyWordingsRequest As UpdateStandardPolicyWordingsRequestType    ' Request Type
            Dim oUpdateStandardPolicyWordingsResponse As UpdateStandardPolicyWordingsResponseType    ' Response Type
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oUpdateStandardPolicyWordingsRequest = New UpdateStandardPolicyWordingsRequestType
                oUpdateStandardPolicyWordingsResponse = New UpdateStandardPolicyWordingsResponseType
                sbLogMessage = New StringBuilder


                With oUpdateStandardPolicyWordingsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty 
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    'WorkManager Request
                    If v_oDocumentTemplate.InsuranceFileKey = String.Empty Then
                        Throw New ArgumentNullException("WorkManager")
                    Else

                        .InsuranceFileKey = v_oDocumentTemplate.InsuranceFileKey
                        .TimeStamp = v_oDocumentTemplate.TimeStamp
                        .PolicyStandardWordings = New List(Of BaseUpdateStandardPolicyWordingsRequestTypePolicyStandardWordingsRow)
                        For Each oPolicies As BaseUpdateStandardPolicyWordingsRequestTypePolicyStandardWordingsRow In .PolicyStandardWordings
                            oPolicies.Code = v_oDocumentTemplate.Code
                        Next
                    End If
                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oUpdateStandardPolicyWordingsResponse = oSAM.UpdateStandardPolicyWordings(oUpdateStandardPolicyWordingsRequest)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                ' Disposing the SAM's object


                With oUpdateStandardPolicyWordingsResponse  'With Response Type
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        'WorkManager Response
                        'Fetching from the  WorkManager Response Collection 

                        v_oDocumentTemplate.TimeStamp = .TimeStamp
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("WorkManager executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & v_oDocumentTemplate.Print() & vbCrLf)
                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & v_oDocumentTemplate.Print() & "results" & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oUpdateStandardPolicyWordingsRequest = Nothing
                oUpdateStandardPolicyWordingsResponse = Nothing
            End Try


            Return v_oDocumentTemplate
        End SyncLock
    End Function
    Public Overrides Function GetProductRiskOptionValue(ByVal ActionType As NexusProvider.ProductConfigActionType,
                                                        ByVal ProductRiskOption As NexusProvider.ProductRiskOptions,
                                                        ByVal RiskTypeOption As NexusProvider.RiskTypeOptions,
                                                        ByVal ProductCode As String,
                                                        ByVal RiskTypeCode As String,
                                                        Optional ByVal v_sBranchCode As String = Nothing) As String
        SyncLock oLock
            Dim sProductRiskOptionValue As String
            Dim oSAM As PureServiceClient 'SAMForInsuranceV2's Object
            Dim oProductRiskOptionValueRequest As ProductRiskOptionValueRequestType    ' Request Type
            Dim oProductRiskOptionValueResponse As ProductRiskOptionValueResponseType    ' Response Type
            Try
                oSAM = InitializeServiceMethod()
                oProductRiskOptionValueRequest = New ProductRiskOptionValueRequestType
                oProductRiskOptionValueResponse = New ProductRiskOptionValueResponseType

                If Current.Cache("ProductOption_" & ActionType.ToString & "_" & ProductCode & "_" & ProductRiskOption & "_" & RiskTypeOption & "_" & RiskTypeCode) Is Nothing Then





                    With oProductRiskOptionValueRequest
                        .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                        'if the passed parameter v_sBranchCode is empty 
                        If String.IsNullOrEmpty(v_sBranchCode) Then
                            'if the branch code is NOT in session 
                            If String.IsNullOrEmpty(sBranchCode) Then
                                'Use the default branch code
                                .BranchCode = sDefaultBranchCode
                            Else
                                'Use the branch code in session 
                                .BranchCode = sBranchCode

                            End If

                        Else
                            'use the passed parameter v_sBranchCode
                            .BranchCode = v_sBranchCode
                        End If
                        Select Case ActionType
                            Case NexusProvider.ProductConfigActionType.ProductRiskMaintenance
                                .ProducRiskOption = ProductRiskOption
                                .ProducRiskOptionSpecified = True
                                .RiskTypeOptionSpecified = False
                            Case NexusProvider.ProductConfigActionType.RiskTypeMaintenance
                                .RiskTypeOption = RiskTypeOption
                                .RiskTypeOptionSpecified = True
                                .ProducRiskOptionSpecified = False
                        End Select
                        .ActionType = ActionType
                        .ProductCode = ProductCode
                        .RiskTypeCode = RiskTypeCode

                    End With


                    'Calling the SAM Method with Request Type
                    'add trace to allow us to debug slow SAM calls
                    Using trace As New Tracer(Category.Trace)
                        oProductRiskOptionValueResponse = oSAM.GetProductRiskOptionValue(oProductRiskOptionValueRequest)
                    End Using

                    'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                    ' Disposing the SAM's object


                    With oProductRiskOptionValueResponse
                        If .Errors IsNot Nothing Then
                            'Process the error object if errors, and throw as a single exception
                            Throw New NexusException(.Errors)
                        Else
                            sProductRiskOptionValue = .ProductRiskOptionValue
                            'Put the value in cache in order to read from cache to reduce the sam call
                            Current.Cache.Insert("ProductOption_" & ActionType.ToString & "_" & ProductCode & "_" & ProductRiskOption & "_" & RiskTypeOption & "_" & RiskTypeCode, sProductRiskOptionValue,
                                                          Nothing, Now.AddHours(iCacheLengthInHours), TimeSpan.Zero)
                        End If
                    End With

                Else
                    sProductRiskOptionValue = CType(Current.Cache("ProductOption_" & ActionType.ToString & "_" & ProductCode & "_" & ProductRiskOption & "_" & RiskTypeOption & "_" & RiskTypeCode), String)
                End If
                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oProductRiskOptionValueRequest = Nothing
                oProductRiskOptionValueResponse = Nothing
            End Try
            Return sProductRiskOptionValue
        End SyncLock
    End Function
    Public Overrides Function GetProductRiskEvents(ByVal v_iInsuranceFileKey As Integer,
                                                        ByVal v_sProductCode As String,
                                                        ByVal v_sEventType As String,
                                                        Optional ByVal v_sBranchCode As String = Nothing) As LookupListCollection
        SyncLock oLock

            Dim oSAM As PureServiceClient = Nothing
            Dim oBaseGetProductRiskEventsRequest As GetProductRiskEventsRequestType   ' Request Type
            Dim oBaseGetProductRiskEventsResponse As GetProductRiskEventsResponseType    ' Response Type
            Dim ollCollection As NexusProvider.LookupListCollection = Nothing
            Dim olllist As LookupListItem
            Try
                oSAM = InitializeServiceMethod()
                oBaseGetProductRiskEventsRequest = New GetProductRiskEventsRequestType
                oBaseGetProductRiskEventsResponse = New GetProductRiskEventsResponseType
                ollCollection = New NexusProvider.LookupListCollection

                With oBaseGetProductRiskEventsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty 
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If

                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    If v_iInsuranceFileKey > 0 Then
                        .InsuranceFileKey = v_iInsuranceFileKey
                        .InsuranceFileKeySpecified = True
                    Else
                        .InsuranceFileKeySpecified = False
                    End If

                    If v_sEventType.Trim.ToUpper = "MTA" Then
                        .EventType = ProductEventActionType.MTAEvent
                    ElseIf v_sEventType.Trim.ToUpper = "CLAIM" Then
                        .EventType = ProductEventActionType.ClaimEvent
                    End If

                    .ProductCode = v_sProductCode
                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oBaseGetProductRiskEventsResponse = oSAM.GetProductRiskEvents(oBaseGetProductRiskEventsRequest)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.
                With oBaseGetProductRiskEventsResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        If .Events IsNot Nothing AndAlso .Events.Count > 0 Then
                            For Each oEvent As BaseGetProductRiskEventsResponseTypeRow In .Events
                                olllist = New LookupListItem
                                olllist.Code = oEvent.EventCode
                                olllist.Description = oEvent.EventDescription
                                olllist.Key = oEvent.EventKey
                                ollCollection.Add(olllist)
                            Next

                        End If

                    End If
                End With
                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oBaseGetProductRiskEventsRequest = Nothing
                oBaseGetProductRiskEventsResponse = Nothing
            End Try
            Return ollCollection
        End SyncLock
    End Function
    'Begin - WPR 64 - Commission Maintenance

    ''' <summary>
    ''' Method to get agent commission details
    ''' </summary>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetAgentCommission(ByVal v_iInsuranceFileKey As Integer,
                                                            Optional ByVal v_sBranchCode As String = Nothing) As EditAgentCommission
        SyncLock oLock
            Dim oSAM As PureServiceClient
            Dim oGetAgentCommissionRequestType As GetAgentCommissionRequestType   ' Request Type
            Dim oGetAgentCommissionResponseType As GetAgentCommissionResponseType    ' Response Type
            Dim oEditAgentCommission As EditAgentCommission
            Dim oAgentCommission As AgentCommission
            Try
                oSAM = InitializeServiceMethod()
                oGetAgentCommissionRequestType = New GetAgentCommissionRequestType
                oGetAgentCommissionResponseType = New GetAgentCommissionResponseType
                oEditAgentCommission = New EditAgentCommission
                oAgentCommission = New AgentCommission


                With oGetAgentCommissionRequestType
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If

                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    If v_iInsuranceFileKey > 0 Then
                        .InsuranceFileKey = v_iInsuranceFileKey
                    Else
                        Throw New ArgumentNullException("Agent Commission")
                    End If
                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oGetAgentCommissionResponseType = oSAM.GetAgentCommission(oGetAgentCommissionRequestType)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                ' Disposing the SAM's object




                With oGetAgentCommissionResponseType
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else

                        oEditAgentCommission.LeadAgentNet = .LeadAgentNet
                        oEditAgentCommission.LeadAgentTotalCommission = .LeadAgentTotalCommission
                        oEditAgentCommission.LeadAgentTotalTax = .LeadAgentTotalTax
                        oEditAgentCommission.SubAgentNet = .SubAgentNet
                        oEditAgentCommission.SubAgentTotalCommission = .SubAgentTotalCommission
                        oEditAgentCommission.SubAgentTotalTax = .SubAgentTotalTax

                        If .AgentCommission IsNot Nothing Then
                            For Each AgentCommission As BaseAgentCommissionResponseTypeRow In .AgentCommission

                                oAgentCommission.Agent = AgentCommission.Agent
                                oAgentCommission.AgentType = AgentCommission.AgentType
                                oAgentCommission.CommissionBand = AgentCommission.CommissionBand
                                oAgentCommission.CommissionRate = AgentCommission.CommissionRate
                                oAgentCommission.CommissionValue = AgentCommission.CommissionValue
                                oAgentCommission.IsLeadAgent = AgentCommission.IsLeadAgent
                                oAgentCommission.IsValue = AgentCommission.IsValue
                                oAgentCommission.MaximumRate = AgentCommission.MaximumRate
                                oAgentCommission.Premium = AgentCommission.Premium
                                oAgentCommission.RiskType = AgentCommission.RiskType
                                oAgentCommission.TaxGroup = AgentCommission.TaxGroup
                                oAgentCommission.TaxValue = AgentCommission.TaxValue
                                oAgentCommission.TaxGroupDescription = AgentCommission.TaxGroupDescription

                                oEditAgentCommission.AgentCommission.Add(oAgentCommission)
                            Next
                        End If
                    End If
                End With
                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetAgentCommissionRequestType = Nothing
                oGetAgentCommissionResponseType = Nothing
            End Try

            Return oEditAgentCommission
        End SyncLock
    End Function

    ''' <summary>
    ''' Method the update agent commission
    ''' </summary>
    ''' <param name="oUpdateAgentCommission"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function UpdateAgentCommission(ByVal oUpdateAgentCommission As EditAgentCommission,
                                          Optional ByVal v_sBranchCode As String = Nothing) As EditAgentCommission
        SyncLock oLock
            Dim oSAM As PureServiceClient
            Dim oUpdateAgentCommissionRequestType As UpdateAgentCommissionRequestType   ' Request Type
            Dim oUpdateAgentCommissionResponseType As UpdateAgentCommissionResponseType    ' Response Type
            Dim oEditAgentCommission As EditAgentCommission
            Try
                oSAM = InitializeServiceMethod()
                oUpdateAgentCommissionRequestType = New UpdateAgentCommissionRequestType
                oUpdateAgentCommissionResponseType = New UpdateAgentCommissionResponseType
                oEditAgentCommission = New EditAgentCommission


                With oUpdateAgentCommissionRequestType
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If

                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    If oUpdateAgentCommission IsNot Nothing AndAlso oUpdateAgentCommission.InsuranceFileKey > 0 Then
                        .InsuranceFileKey = oUpdateAgentCommission.InsuranceFileKey
                    Else
                        Throw New ArgumentNullException("Agent Commission")
                    End If
                    Dim oAgentCommission As BaseUpdateAgentCommissionRequestTypeRow
                    .AgentCommission = New List(Of BaseUpdateAgentCommissionRequestTypeRow)
                    For i As Integer = 0 To oUpdateAgentCommission.AgentCommission.Count - 1
                        oAgentCommission = New BaseUpdateAgentCommissionRequestTypeRow
                        oAgentCommission.Agent = oUpdateAgentCommission.AgentCommission(i).Agent
                        oAgentCommission.AgentType = oUpdateAgentCommission.AgentCommission(i).AgentType
                        oAgentCommission.CommissionBand = oUpdateAgentCommission.AgentCommission(i).CommissionBand
                        oAgentCommission.CommissionRate = oUpdateAgentCommission.AgentCommission(i).CommissionRate
                        oAgentCommission.CommissionValue = oUpdateAgentCommission.AgentCommission(i).CommissionValue
                        oAgentCommission.IsLeadAgent = oUpdateAgentCommission.AgentCommission(i).IsLeadAgent
                        oAgentCommission.IsAmended = oUpdateAgentCommission.AgentCommission(i).IsAmended
                        oAgentCommission.Premium = oUpdateAgentCommission.AgentCommission(i).Premium
                        oAgentCommission.RiskType = oUpdateAgentCommission.AgentCommission(i).RiskType
                        oAgentCommission.TaxGroupCode = oUpdateAgentCommission.AgentCommission(i).TaxGroup
                        oAgentCommission.AmendedTaxValue = oUpdateAgentCommission.AgentCommission(i).TaxValue
                        oAgentCommission.IsTaxAmended = oUpdateAgentCommission.AgentCommission(i).IsTaxAmended
                        oAgentCommission.OverRideReason = oUpdateAgentCommission.AgentCommission(i).OverRideReason
                        oAgentCommission.CalculatedCommissionValue = oUpdateAgentCommission.AgentCommission(i).CalculatedCommissionValue
                        oAgentCommission.IsValue = oUpdateAgentCommission.AgentCommission(i).IsValue
                        oAgentCommission.MaximumRate = oUpdateAgentCommission.AgentCommission(i).MaximumRate

                        If oUpdateAgentCommission.AgentCommission(i).CalculatedCommissionValue > 0.0 Then
                            oAgentCommission.CalculatedCommissionValueSpecified = True
                        Else
                            oAgentCommission.CalculatedCommissionValueSpecified = False
                        End If
                        .AgentCommission.Add(oAgentCommission)
                    Next

                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oUpdateAgentCommissionResponseType = oSAM.UpdateAgentCommission(oUpdateAgentCommissionRequestType)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                ' Disposing the SAM's object




                With oUpdateAgentCommissionResponseType
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        oEditAgentCommission.LeadAgentNet = .LeadAgentNet
                        oEditAgentCommission.LeadAgentTotalCommission = .LeadAgentTotalCommission
                        oEditAgentCommission.LeadAgentTotalTax = .LeadAgentTotalTax
                        oEditAgentCommission.SubAgentNet = .SubAgentNet
                        oEditAgentCommission.SubAgentTotalCommission = .SubAgentTotalCommission
                        oEditAgentCommission.SubAgentTotalTax = .SubAgentTotalTax
                        For Each AgentCommission As BaseAgentCommissionResponseTypeRow In .AgentCommission
                            Dim oAgentCommission As New AgentCommission
                            oAgentCommission.Agent = AgentCommission.Agent
                            oAgentCommission.AgentType = AgentCommission.AgentType
                            oAgentCommission.CommissionBand = AgentCommission.CommissionBand
                            oAgentCommission.CommissionRate = AgentCommission.CommissionRate
                            oAgentCommission.CommissionValue = AgentCommission.CommissionValue
                            oAgentCommission.IsLeadAgent = AgentCommission.IsLeadAgent
                            oAgentCommission.IsValue = AgentCommission.IsValue
                            oAgentCommission.MaximumRate = AgentCommission.MaximumRate
                            oAgentCommission.Premium = AgentCommission.Premium
                            oAgentCommission.RiskType = AgentCommission.RiskType
                            oAgentCommission.TaxGroup = AgentCommission.TaxGroup
                            oAgentCommission.TaxValue = AgentCommission.TaxValue
                            oAgentCommission.TaxGroupDescription = AgentCommission.TaxGroupDescription

                            oEditAgentCommission.AgentCommission.Add(oAgentCommission)
                        Next
                    End If
                End With
                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oUpdateAgentCommissionRequestType = Nothing
                oUpdateAgentCommissionResponseType = Nothing
            End Try
            Return oEditAgentCommission
        End SyncLock
    End Function

    'End - WPR 64 - Commission Maintenance
    ''' <summary>
    ''' Method the get agent commission tax
    ''' </summary>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="v_dAgentCommissionAmount"></param>
    ''' <param name="v_sCurrencyCode"></param>
    ''' <param name="v_sTaxGroupCode"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetAgentCommissionTax(ByVal v_iInsuranceFileKey As Integer,
                                                 ByVal v_dAgentCommissionAmount As Double,
                                                 ByVal v_sCurrencyCode As String,
                                                 ByVal v_sTaxGroupCode As String,
                                                 Optional ByVal v_sBranchCode As String = Nothing) As TaxForClaims
        SyncLock oLock
            Dim oSAM As PureServiceClient = Nothing
            Dim oGetAgentCommissionTaxRequestType As GetAgentCommissionTaxRequestType   ' Request Type
            Dim oGetAgentCommissionTaxResponseType As GetAgentCommissionTaxResponseType    ' Response Type
            Dim oTaxForClaims As TaxForClaims = Nothing
            Try
                oSAM = InitializeServiceMethod()
                oGetAgentCommissionTaxRequestType = New GetAgentCommissionTaxRequestType
                oGetAgentCommissionTaxResponseType = New GetAgentCommissionTaxResponseType
                oTaxForClaims = New TaxForClaims


                With oGetAgentCommissionTaxRequestType
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If

                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    If v_iInsuranceFileKey > 0 Then
                        .InsuranceFileKey = v_iInsuranceFileKey
                    Else
                        Throw New ArgumentNullException("Insurance File Key is Missing")
                    End If
                    .AgentCommissionAmount = v_dAgentCommissionAmount
                    .CurrencyCode = v_sCurrencyCode
                    .TaxGroupCode = v_sTaxGroupCode

                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oGetAgentCommissionTaxResponseType = oSAM.GetAgentCommissionTax(oGetAgentCommissionTaxRequestType)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                With oGetAgentCommissionTaxResponseType
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        oTaxForClaims.TaxBaseAmount = .TaxBaseAmount
                        oTaxForClaims.TaxCurrencyAmount = .TaxCurrencyAmount
                    End If

                End With
                ' Disposing the SAM's object

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetAgentCommissionTaxRequestType = Nothing
                oGetAgentCommissionTaxResponseType = Nothing
            End Try

            Return oTaxForClaims
        End SyncLock
    End Function
    ''' <summary>
    ''' Fetches report via SAM and stores it to the specified location
    ''' </summary>
    ''' <param name="v_sReportName">Name of the Report to be generated</param>
    ''' <param name="v_oDocumentFormatType">Type of document format requested to receive</param>
    ''' <param name="v_oParameters">Collection of parameters</param>
    ''' <param name="v_sDocumentExtractionDirectory">Name of the directory where received file need to be extract</param>
    ''' <param name="v_sBranchCode">Optional Parameter BranchCode</param>
    Public Overrides Function GetReport(ByVal v_sReportName As String,
                                   ByVal v_oDocumentFormatType As NexusProvider.DocumentFormatType,
                                   ByVal v_oParameters As ParametersCollection,
                                   ByVal v_sDocumentExtractionDirectory As String,
                                                      Optional ByVal v_sBranchCode As String = Nothing) As String
        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oGetReportRequest As GetReportRequestType
            Dim oGetReportResponse As GetReportResponseType
            Dim sFileName As String = String.Empty
            Dim oParameters As BaseGetReportRequestTypeParameters
            Dim sGUID As String = Guid.NewGuid.ToString
            Dim file As System.IO.FileInfo = Nothing
            Dim fsOutputFile As IO.FileStream = IO.File.OpenWrite(v_sDocumentExtractionDirectory & "\" & sGUID & ".zip")
            Dim zipToRead As String = v_sDocumentExtractionDirectory & "\" & sGUID & ".zip"
            Dim extractDir As String = v_sDocumentExtractionDirectory
            Dim sourceDir As DirectoryInfo = New DirectoryInfo(v_sDocumentExtractionDirectory)
            Dim sFileToCopy As String = Nothing
            Dim sFileTargetLocation As String = Nothing
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetReportRequest = New GetReportRequestType
                oGetReportResponse = New GetReportResponseType
                sbLogMessage = New StringBuilder


                With oGetReportRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .ReportName = v_sReportName

                    If String.IsNullOrEmpty(v_sDocumentExtractionDirectory) Then
                        'DocumentExtractionDirectory is mandatory to pass
                        Throw New ArgumentNullException("DocumentExtractionDirectory")
                    Else
                        'create directory with the same name if not exist
                        If Not IO.Directory.Exists(v_sDocumentExtractionDirectory) Then
                            IO.Directory.CreateDirectory(v_sDocumentExtractionDirectory)
                        End If
                    End If

                    .FormatType = v_oDocumentFormatType

                    'run the loop for parameters and make the request ready

                    .Parameters = New List(Of BaseGetReportRequestTypeParameters)
                    For iCount As Integer = 0 To v_oParameters.Count - 1
                        oParameters = New BaseGetReportRequestTypeParameters

                        oParameters.ParamName = v_oParameters(iCount).ParamNameField
                        oParameters.ParamValue = v_oParameters(iCount).ParamValueField
                        .Parameters.Add(oParameters)
                    Next
                End With



                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oGetReportResponse = oSAM.GetReport(oGetReportRequest)
                End Using






                With oGetReportResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else




                        'Create destination directory on the web server, if it doesn't already exist
                        IO.Directory.CreateDirectory(v_sDocumentExtractionDirectory)
                        'Create a unique zip file from the byte array retrieved from the Web Service

                        fsOutputFile.Write(.ReportDocument, 0, .ReportDocument.Length)
                        fsOutputFile.Close()

                        ''Invoke the unzip method
                        'duz1.ActionDZ = CDUnZipNET.DUZACTION.UNZIP_EXTRACT


                        Using zip As ZipFile = ZipFile.Read(zipToRead)
                            ' When extraction would overwrite an existing file, overwrite the file silently.The overwrite will happen even if the target file is marked as read-only.
                            zip.ExtractAll(extractDir, Ionic.Zip.ExtractExistingFileAction.OverwriteSilently)
                        End Using
                        'Remove zip file, as it been unzipped
                        IO.File.Delete(v_sDocumentExtractionDirectory & "\" & sGUID & ".zip")


                        'name of the source directory


                        'for copy the unzip file




                        'find the name of the file which has been extracted, we need to return this
                        For Each file In sourceDir.GetFiles()
                            If file.Extension = ".rpt" Then
                                'set sFileName to the name of the rpt file
                                sFileName = file.Name
                                Exit For
                            End If
                        Next
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetReport executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    sbLogMessage.AppendLine("v_sReportName = " & v_sReportName.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_oDocumentFormatType = " & v_oDocumentFormatType.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_oParameters = " & v_oParameters.Count & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetReportRequest = Nothing
                oGetReportResponse = Nothing
            End Try


            Return sFileName
        End SyncLock
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub CopyQuote(ByRef v_iInsuranceFileKey As Integer,
                              Optional ByRef v_iInsuranceFolderKey As Integer = 0,
                              Optional ByVal v_sBranchCode As String = Nothing,
                              Optional ByVal v_bCalledFromClonePolicy As Boolean = False,
                              Optional ByVal v_bIsQuoteVersioning As Boolean = False)

        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oCopyQuoteRequest As CopyQuoteRequestType
            Dim oCopyQuoteResponse As CopyQuoteResponseType
            Dim oQuote As NexusProvider.Quote
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oCopyQuoteRequest = New CopyQuoteRequestType
                oCopyQuoteResponse = New CopyQuoteResponseType
                oQuote = New NexusProvider.Quote
                sbLogMessage = New StringBuilder


                With oCopyQuoteRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .InsuranceFileKey = v_iInsuranceFileKey

                End With



                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oCopyQuoteResponse = oSAM.CopyQuote(oCopyQuoteRequest)
                End Using







                With oCopyQuoteResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)

                    Else

                        'SAM is returning only InsuranceFile, InsuranceFolder, InsuranceRef of newly created quote
                        'currently, we are not using these values in Nexus layer but storing it in quote object for any future use
                        v_iInsuranceFileKey = .InsuranceFileKey
                        v_iInsuranceFolderKey = .InsuranceFolderKey

                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("CopyQuote executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input :" & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("v_iInsuranceFile = " & v_iInsuranceFileKey.ToString & vbCrLf)

                    sbLogMessage.AppendLine("Output:" & vbCrLf)

                    sbLogMessage.AppendLine("Returned " & oQuote.Print.Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oCopyQuoteRequest = Nothing
                oCopyQuoteResponse = Nothing
            End Try


        End SyncLock

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_oQuote"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub UpdateQuoteStatus(ByRef r_oQuote As Quote, Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oUpdateQuoteStatusRequest As UpdateQuoteStatusRequestType
            Dim oUpdateQuoteStatusResponse As UpdateQuoteStatusResponseType
            Dim oQuote As NexusProvider.Quote
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oUpdateQuoteStatusRequest = New UpdateQuoteStatusRequestType
                oUpdateQuoteStatusResponse = New UpdateQuoteStatusResponseType
                oQuote = New NexusProvider.Quote
                sbLogMessage = New StringBuilder


                With oUpdateQuoteStatusRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .InsuranceFileKey = r_oQuote.InsuranceFileKey
                    .QuoteStatusKey = r_oQuote.QuoteStatusKey
                End With



                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oUpdateQuoteStatusResponse = oSAM.UpdateQuoteStatus(oUpdateQuoteStatusRequest)
                End Using







                With oUpdateQuoteStatusResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)

                    Else

                        'SAM is returning only InsuranceFile, InsuranceFolder, InsuranceRef of newly created quote
                        'currently, we are not using these values in Nexus layer but storing it in quote object for any future use
                        r_oQuote.InsuranceFileKey = .InsuranceFileKey
                        r_oQuote.QuoteStatusKey = .QuoteStatusKey

                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("CopyQuote executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input :" & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("v_iInsuranceFile = " & r_oQuote.InsuranceFileKey.ToString & vbCrLf)

                    sbLogMessage.AppendLine("Output:" & vbCrLf)

                    sbLogMessage.AppendLine("Returned " & oQuote.Print.Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oUpdateQuoteStatusRequest = Nothing
                oUpdateQuoteStatusResponse = Nothing
            End Try


        End SyncLock
    End Sub



    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_oQuote"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub DeletePolicy(ByRef r_oQuote As Quote, Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oDeletePolicyRequest As DeletePolicyRequestType
            Dim oDeletePolicyResponse As DeletePolicyResponseType
            Dim oQuote As NexusProvider.Quote
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oDeletePolicyRequest = New DeletePolicyRequestType
                oDeletePolicyResponse = New DeletePolicyResponseType
                oQuote = New NexusProvider.Quote
                sbLogMessage = New StringBuilder


                With oDeletePolicyRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)

                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .InsuranceFileKey = r_oQuote.InsuranceFileKey

                End With



                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oDeletePolicyResponse = oSAM.DeletePolicy(oDeletePolicyRequest)
                End Using

                With oDeletePolicyResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)

                    Else

                        'SAM is returning only InsuranceFile, InsuranceFolder, InsuranceRef of newly created quote
                        'currently, we are not using these values in Nexus layer but storing it in quote object for any future use
                        'r_oQuote.InsuranceFileKey = .InsuranceFilekey


                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("CopyQuote executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input :" & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("v_iInsuranceFile = " & r_oQuote.InsuranceFileKey.ToString & vbCrLf)

                    sbLogMessage.AppendLine("Output:" & vbCrLf)

                    sbLogMessage.AppendLine("Returned " & oQuote.Print.Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oDeletePolicyRequest = Nothing
                oDeletePolicyResponse = Nothing
            End Try


        End SyncLock
    End Sub

    'Begin - WPR 15  
    ''' <summary>
    ''' Method to get All tax
    ''' </summary>
    ''' <param name="i_InsuranceFileKey"></param>  
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetTaxes(ByVal i_InsuranceFileKey As Integer,
                                       Optional ByVal v_sBranchCode As String = Nothing) As AllTaxes
        SyncLock oLock

            Dim oSAM As PureServiceClient = Nothing
            Dim oGetTaxesRequestType As GetTaxesRequestType
            Dim oGetTaxesResponseType As GetTaxesResponseType
            Dim oCollAllTaxes As AllTaxes = Nothing
            Dim oAllTax As AllTaxes
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetTaxesRequestType = New GetTaxesRequestType
                oGetTaxesResponseType = New GetTaxesResponseType
                oCollAllTaxes = New AllTaxes
                sbLogMessage = New StringBuilder


                With oGetTaxesRequestType
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode

                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .InsuranceFileKey = i_InsuranceFileKey

                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oGetTaxesResponseType = oSAM.GetTaxes(oGetTaxesRequestType)
                End Using
                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.
                'oCollAllTaxes = New AllTaxesCollection

                With oGetTaxesResponseType
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else

                        If .Row IsNot Nothing AndAlso .Row.Count > 0 Then

                            For Each oGetAllTaxesRow As BaseGetTaxesResponseTypeRow In .Row
                                oAllTax = New AllTaxes
                                oAllTax.TaxCalculationKey = oGetAllTaxesRow.TaxCalculationKey
                                oAllTax.RiskKey = oGetAllTaxesRow.RiskKey
                                oAllTax.TaxBandKey = oGetAllTaxesRow.TaxBandKey
                                oAllTax.Premium = oGetAllTaxesRow.Premium
                                oAllTax.IsValue = oGetAllTaxesRow.IsValue
                                oAllTax.TaxPercentage = oGetAllTaxesRow.TaxPercentage
                                oAllTax.TaxValue = oGetAllTaxesRow.TaxValue
                                oAllTax.IsManuallyChanged = oGetAllTaxesRow.IsManuallyChanged
                                oAllTax.CalculationBasis = oGetAllTaxesRow.CalculationBasis
                                oAllTax.BasisValue = oGetAllTaxesRow.BasisValue
                                oAllTax.SumInsured = oGetAllTaxesRow.SumInsured
                                oAllTax.SumInsuredRounded = oGetAllTaxesRow.SumInsuredRounded
                                oAllTax.CurrencyKey = oGetAllTaxesRow.CurrencyKey
                                oAllTax.AllowTaxCredit = oGetAllTaxesRow.AllowTaxCredit
                                oAllTax.OriginalSumInsured = oGetAllTaxesRow.OriginalSumInsured
                                oAllTax.CountryKey = oGetAllTaxesRow.CountryKey
                                oAllTax.StateKey = oGetAllTaxesRow.StateKey
                                oAllTax.ClassOfBusinessKey = oGetAllTaxesRow.ClassOfBusinessKey
                                oAllTax.TaxGroupKey = oGetAllTaxesRow.TaxGroupKey
                                oAllTax.PolicyFeeUKey = oGetAllTaxesRow.PolicyFeeUKey
                                oAllTax.AgentCommissionKey = oGetAllTaxesRow.AgentCommissionKey
                                oAllTax.RIPartyKey = oGetAllTaxesRow.RIPartyKey
                                oAllTax.RIArrangementLineKey = oGetAllTaxesRow.RIArrangementLineKey
                                oAllTax.InsuranceSectionKey = oGetAllTaxesRow.InsuranceSectionKey
                                oAllTax.PolicyFeeKey = oGetAllTaxesRow.PolicyFeeKey
                                oAllTax.PolicyAgentsKey = oGetAllTaxesRow.PolicyAgentsKey
                                oAllTax.InsurerPartyKey = oGetAllTaxesRow.InsurerPartyKey
                                oAllTax.ClaimPerilKey = oGetAllTaxesRow.ClaimPerilKey
                                oAllTax.ClaimPaymentKey = oGetAllTaxesRow.ClaimPaymentKey
                                oAllTax.ClaimReceiptKey = oGetAllTaxesRow.ClaimReceiptKey
                                oAllTax.ClaimPaymentItemKey = oGetAllTaxesRow.ClaimPaymentItemKey
                                oAllTax.IsNotAppliedToClient = oGetAllTaxesRow.IsNotAppliedToClient
                                oAllTax.IncludeTaxInInstalments = oGetAllTaxesRow.IncludeTaxInInstalments
                                oAllTax.SpreadTaxAcrossInstalments = oGetAllTaxesRow.SpreadTaxAcrossInstalments
                                oAllTax.BaseTaxCalculationKey = oGetAllTaxesRow.BaseTaxCalculationKey
                                oAllTax.VersionKey = oGetAllTaxesRow.VersionKey
                                oAllTax.PfPremFinanceKey = oGetAllTaxesRow.PfPremFinanceKey
                                oAllTax.PfPremFinanceVersion = oGetAllTaxesRow.PfPremFinanceVersion
                                oAllTax.PolicyCoinsurersSectionKey = oGetAllTaxesRow.PolicyCoinsurersSectionKey
                                oAllTax.IsCommissionTax = oGetAllTaxesRow.IsCommissionTax
                                oAllTax.ApplyTaxBy = oGetAllTaxesRow.ApplyTaxBy
                                oAllTax.TaxBandRateKey = oGetAllTaxesRow.TaxBandRateKey
                                oAllTax.IsSuspended = oGetAllTaxesRow.IsSuspended
                                oAllTax.TransType = oGetAllTaxesRow.TransType
                                oAllTax.TaxBandCode = oGetAllTaxesRow.TaxBandCode
                                oAllTax.TaxBandDescription = oGetAllTaxesRow.TaxBandDescription
                                oAllTax.TaxGroupCode = oGetAllTaxesRow.TaxGroupCode
                                oAllTax.TaxGroupDescription = oGetAllTaxesRow.TaxGroupDescription
                                oAllTax.TaxBandRateDescription = oGetAllTaxesRow.TaxBandRateDescription
                                oCollAllTaxes.AllTaxes.Add(oAllTax)
                            Next
                        End If
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetTaxes executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine(i_InsuranceFileKey.ToString().Replace("<br />", vbCrLf))

                    sbLogMessage.AppendLine("Output: " & vbCrLf)
                    sbLogMessage.AppendLine(oCollAllTaxes.AllTaxes.Print.Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetTaxesRequestType = Nothing
                oGetTaxesResponseType = Nothing
            End Try

            Return oCollAllTaxes
        End SyncLock
    End Function

    ''' <summary>
    ''' Method to update Taxes
    ''' </summary>
    ''' <param name="v_oAllTaxesCollection"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub UpdateTaxes(ByVal v_oAllTaxesCollection As AllTaxesCollection,
                                          Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock
            Dim oSAM As PureServiceClient
            Dim oUpdateTaxesRequestType As UpdateTaxesRequestType   ' Request Type
            Dim oUpdateTaxesResponseType As UpdateTaxesResponseType    ' Response Type
            Dim oAllTaxesCollection As BaseUpdateTaxesRequestTypeRow
            Try
                oSAM = InitializeServiceMethod()
                oUpdateTaxesRequestType = New UpdateTaxesRequestType
                oUpdateTaxesResponseType = New UpdateTaxesResponseType


                With oUpdateTaxesRequestType
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If

                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    .Row = New List(Of BaseUpdateTaxesRequestTypeRow)
                    For i As Integer = 0 To v_oAllTaxesCollection.Count - 1
                        oAllTaxesCollection = New BaseUpdateTaxesRequestTypeRow
                        oAllTaxesCollection.TaxCalculationKey = v_oAllTaxesCollection.Item(i).TaxCalculationKey
                        oAllTaxesCollection.TaxValue = v_oAllTaxesCollection.Item(i).TaxValue
                        oAllTaxesCollection.IsValue = v_oAllTaxesCollection.Item(i).IsValue
                        oAllTaxesCollection.TaxPercentage = v_oAllTaxesCollection.Item(i).TaxPercentage
                        oAllTaxesCollection.IsEdited = v_oAllTaxesCollection.Item(i).IsEdit
                        .Row.Add(oAllTaxesCollection)
                    Next

                End With


                Using trace As New Tracer(Category.Trace)
                    oUpdateTaxesResponseType = oSAM.UpdateTaxes(oUpdateTaxesRequestType)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.
                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oUpdateTaxesRequestType = Nothing
                oUpdateTaxesResponseType = Nothing
            End Try


        End SyncLock
    End Sub
    ''' <summary>
    ''' Get the Rating Sections Based on RiskType
    ''' </summary>
    ''' <param name="RiskTypeCode"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetRatingSectionByRiskType(ByVal RiskTypeCode As String, Optional ByVal v_sBranchCode As String = Nothing) As RatingSectionTypesCollection
        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oGetRatingSectionByRiskTypeRequest As GetRatingSectionByRiskTypeRequestType
            Dim oGetRatingSectionByRiskTypeResponse As GetRatingSectionByRiskTypeResponseType
            Dim oRatingSectionTypesCollection As RatingSectionTypesCollection = Nothing
            Dim oRatingSectionTypes As RatingSectionTypes = Nothing
            Dim oQuote As NexusProvider.Quote
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetRatingSectionByRiskTypeRequest = New GetRatingSectionByRiskTypeRequestType
                oGetRatingSectionByRiskTypeResponse = New GetRatingSectionByRiskTypeResponseType
                oQuote = New NexusProvider.Quote
                sbLogMessage = New StringBuilder



                With oGetRatingSectionByRiskTypeRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    'if the passed parameter v_sBranchCode is empty
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode

                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If
                    .RiskTypeCode = RiskTypeCode

                End With


                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oGetRatingSectionByRiskTypeResponse = oSAM.GetRatingSectionByRiskType(oGetRatingSectionByRiskTypeRequest)
                End Using

                oRatingSectionTypesCollection = New RatingSectionTypesCollection

                With oGetRatingSectionByRiskTypeResponse
                    For i As Integer = 0 To oGetRatingSectionByRiskTypeResponse.RatingSections.Count - 1
                        oRatingSectionTypes = New RatingSectionTypes
                        oRatingSectionTypes.RatingSectionTypeId = .RatingSections(i).RatingSectionId
                        oRatingSectionTypes.RatingSectionTypeCode = .RatingSections(i).RatingSectionCode
                        oRatingSectionTypes.Description = .RatingSections(i).Description
                        oRatingSectionTypesCollection.Add(oRatingSectionTypes)
                    Next

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)

                    Else

                        'SAM is returning only InsuranceFile, InsuranceFolder, InsuranceRef of newly created quote
                        'currently, we are not using these values in Nexus layer but storing it in quote object for any future use
                        'r_oQuote.InsuranceFileKey = .InsuranceFilekey

                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetRatingSectionByRiskType executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input :" & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("RiskType = " & RiskTypeCode & vbCrLf)

                    sbLogMessage.AppendLine("Output:" & vbCrLf)

                    sbLogMessage.AppendLine("Returned " & oQuote.Print.Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetRatingSectionByRiskTypeRequest = Nothing
                oGetRatingSectionByRiskTypeResponse = Nothing
            End Try

            Return oRatingSectionTypesCollection

        End SyncLock

    End Function

    Public Overrides Function GetFinancePlanDetails(ByVal v_iInsuranceFileKey As Integer,
                                                    Optional ByVal v_sBranchCode As String = Nothing) As FinancePlan
        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oGetFinancePlanDetailsRequest As GetFinancePlanDetailsRequestType
            Dim oGetFinancePlanDetailsResponse As GetFinancePlanDetailsResponseType
            Dim oFinancePlan As FinancePlan
            Dim oFinanceBankDetail As Bank
            Dim oInstalmentsCollection As InstalmentsCollection
            Dim oInstalment As Instalment = Nothing
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetFinancePlanDetailsRequest = New GetFinancePlanDetailsRequestType
                oGetFinancePlanDetailsResponse = New GetFinancePlanDetailsResponseType
                oFinancePlan = New FinancePlan
                oFinanceBankDetail = New Bank
                oInstalmentsCollection = New InstalmentsCollection
                sbLogMessage = New StringBuilder


                With oGetFinancePlanDetailsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then
                        'if the branch code is NOT in session 
                        If String.IsNullOrEmpty(sBranchCode) Then
                            'Use the default branch code
                            .BranchCode = sDefaultBranchCode
                        Else
                            'Use the branch code in session 
                            .BranchCode = sBranchCode
                        End If
                    Else
                        'use the passed parameter v_sBranchCode
                        .BranchCode = v_sBranchCode
                    End If

                    If v_iInsuranceFileKey > 0 Then
                        .InsuranceFileKey = v_iInsuranceFileKey
                    Else
                        Throw New ArgumentException("InstalmentQuotes.InsuranceFileKey")
                    End If
                End With
                Using trace As New Tracer(Category.Trace)
                    oGetFinancePlanDetailsResponse = oSAM.GetFinancePlanDetails(oGetFinancePlanDetailsRequest)
                End Using

                With oGetFinancePlanDetailsResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                End If

                'TODO()
                'Process the error object if errors, and throw as a single exception    
                'Populate the FinancePlan from GetFinancePlanDetails
                oFinancePlan.FinanceAmount = oGetFinancePlanDetailsResponse.FinancedAmount
                oFinancePlan.FinanceAmount = oGetFinancePlanDetailsResponse.FinancedAmount
                oFinancePlan.FirstInstalmentAmount = oGetFinancePlanDetailsResponse.FirstInstalmentAmount
                oFinancePlan.FirstInstalmentDate = oGetFinancePlanDetailsResponse.FirstInstalmentDate
                oFinancePlan.OtherInstalmentAmount = oGetFinancePlanDetailsResponse.OtherInstalmentAmount
                oFinancePlan.NextInstalmentDate = oGetFinancePlanDetailsResponse.NextInstalmentDate
                oFinancePlan.LastInstalmentDate = oGetFinancePlanDetailsResponse.LastInstalmentDate
                oFinancePlan.ProtectionCharge = oGetFinancePlanDetailsResponse.ProtectionCharge
                oFinancePlan.InterestRate = oGetFinancePlanDetailsResponse.InterestRate
                oFinancePlan.InterestAmount = oGetFinancePlanDetailsResponse.InterestAmount
                oFinancePlan.AdminCharge = oGetFinancePlanDetailsResponse.AdminCharge
                oFinancePlan.APRRate = oGetFinancePlanDetailsResponse.APRRate
                oFinancePlan.Deposit = oGetFinancePlanDetailsResponse.Deposit
                oFinancePlan.TaxAmount = oGetFinancePlanDetailsResponse.Taxes
                oFinancePlan.StatusDescription = oGetFinancePlanDetailsResponse.StatusDescription
                oFinancePlan.TotalInstalmentAmount = oGetFinancePlanDetailsResponse.TotalAmount
                oFinancePlan.NoOfInstalments = oGetFinancePlanDetailsResponse.NoOfInstalments
                'oFinancePlan.InstalmentDetails = oGetFinancePlanDetailsResponse.Instalments
                oFinanceBankDetail.AccountType = oGetFinancePlanDetailsResponse.BankAccountType
                oFinanceBankDetail.BankName = oGetFinancePlanDetailsResponse.BankName
                oFinanceBankDetail.BankAddress = oGetFinancePlanDetailsResponse.BankAddress1
                oFinanceBankDetail.BankBranch = oGetFinancePlanDetailsResponse.BankBranchName
                oFinanceBankDetail.BranchCode = oGetFinancePlanDetailsResponse.BankBranchCode
                oFinanceBankDetail.AccountHolderName = oGetFinancePlanDetailsResponse.BankAccountName
                oFinanceBankDetail.AccountNumber = oGetFinancePlanDetailsResponse.BankAccountNumber
                oFinancePlan.Frequency = oGetFinancePlanDetailsResponse.Frequency
                oFinancePlan.PaymentMethod = oGetFinancePlanDetailsResponse.PaymentMethod
                oFinancePlan.SchemeName = oGetFinancePlanDetailsResponse.SchemeName
                oFinancePlan.BankDetails = oFinanceBankDetail

                If oGetFinancePlanDetailsResponse.Instalments IsNot Nothing Then
                    For Each oBaseInstalment As BaseGetFinancePlanDetailsResponseTypeRow In oGetFinancePlanDetailsResponse.Instalments
                        oInstalment = New Instalment()
                        .PaymentDate = oBaseInstalment.PaymentDate.Date
                        With oInstalment
                            .InstalmentNumber = oBaseInstalment.InstalmentNumber
                            .Amount = oBaseInstalment.Amount
                            .PaymentDate = oBaseInstalment.PaymentDate.Date
                            '.PaymentDateStatus = oBaseInstalment.PaymentDateSpecified
                            .DueDate = oBaseInstalment.DueDate
                            .Status = oBaseInstalment.Status
                            .Reason = oBaseInstalment.Reason
                        End With
                        oInstalmentsCollection.Add(oInstalment)
                    Next
                End If
                End With
                oFinancePlan.InstalmentDetails = oInstalmentsCollection

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetFinancePlanDetails executed ok" & vbCrLf)
                    '' we don't have input here, we have the input on Credentials Method
                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("v_iInsuranceFileKey = " & v_iInsuranceFileKey.ToString & vbCrLf)

                    sbLogMessage.AppendLine("Output : " & vbCrLf)
                    sbLogMessage.AppendLine(oInstalmentsCollection.Print().Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetFinancePlanDetailsRequest = Nothing
                oGetFinancePlanDetailsResponse = Nothing
            End Try
            Return oFinancePlan
        End SyncLock
    End Function


End Class
