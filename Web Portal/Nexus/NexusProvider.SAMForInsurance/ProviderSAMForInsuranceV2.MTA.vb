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
Partial Public Class ProviderSAMForInsuranceV2

    Public Overrides Sub AddMtaQuote(ByRef r_oMta As MTA,
                            Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oAddMtaQuoteRequest As AddMtaQuoteRequestType
            Dim oAddMtaQuoteResponse As AddMtaQuoteResponseType
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oAddMtaQuoteRequest = New AddMtaQuoteRequestType
                oAddMtaQuoteResponse = New AddMtaQuoteResponseType
                sbLogMessage = New StringBuilder


                With oAddMtaQuoteRequest
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

                    If r_oMta.AccountHandlerCnt > 0 Then
                        .AccountHandlerCnt = r_oMta.AccountHandlerCnt
                        .AccountHandlerCntSpecified = True
                    Else
                        .AccountHandlerCntSpecified = False

                    End If
                    If r_oMta.InsuranceFileKey > 0 Then
                        .InsuranceFileKey = r_oMta.InsuranceFileKey
                    Else
                        Throw New ArgumentNullException("MtaQuote.InsuranceFileKey")
                    End If

                    .TypeOfMta = r_oMta.TypeOfMta
                    .MtaReason = r_oMta.MtaReason

                    If r_oMta.MtaEffectiveDate = Date.MinValue Then
                        Throw New ArgumentNullException("MtaQuote.EffectiveDate")
                    Else
                        .EffectiveDate = r_oMta.MtaEffectiveDate
                    End If

                    If r_oMta.MtaExpiryDate = Date.MinValue Then
                        Throw New ArgumentNullException("MtaQuote.ExpiryDate")
                    Else
                        .ExpiryDate = r_oMta.MtaExpiryDate
                    End If

                    If r_oMta.QuoteExpiryDate = Date.MinValue Then
                        Throw New ArgumentNullException("MtaQuote.QuoteExpiryDate")
                    Else
                        .QuoteExpiryDate = r_oMta.QuoteExpiryDate
                        .QuoteExpiryDateSpecified = True
                    End If

                    .InsuredName = r_oMta.InsuredName
                    .PolicyKey = r_oMta.PolicyKey
                    .Regarding = r_oMta.Regarding
                    .AlternateReference = r_oMta.AlternateReference
                    .PolicyStatusCode = r_oMta.PolicyStatusCode
                    .AnalysisCode = r_oMta.AnalysisCode
                    .BusinessTypeCode = r_oMta.BusinessTypeCode

                    If r_oMta.IssueDate <> Date.MinValue Then
                        .IssueDateSpecified = True
                        .IssueDate = r_oMta.IssueDate
                    Else
                        .IssueDateSpecified = False
                    End If

                    If r_oMta.ProposalDate <> Date.MinValue Then
                        .ProposalDateSpecified = True
                        .ProposalDate = r_oMta.ProposalDate
                    Else
                        .ProposalDateSpecified = False
                    End If

                    .FrequencyCode = r_oMta.FrequencyCode

                    If r_oMta.LapseCancelDate <> Date.MinValue Then
                        .LapseCancelDateSpecified = True
                        .LapseCancelDate = r_oMta.LapseCancelDate
                    Else
                        .LapseCancelDateSpecified = False
                    End If

                    If r_oMta.LTUExpiryDate <> Date.MinValue Then
                        .LTUExpiryDateSpecified = True
                        .LTUExpiryDate = r_oMta.LTUExpiryDate
                    Else
                        .LTUExpiryDateSpecified = False
                    End If

                    .StopReasonCode = r_oMta.StopReasonCode
                    .RenewalMethodCode = r_oMta.RenewalMethodCode
                    .LapseCancelReasonCode = r_oMta.LapseCancelReasonCode

                    If r_oMta.LapseCancelDate <> Date.MinValue Then
                        .LapseCancelDateSpecified = True
                        .LapseCancelDate = r_oMta.LapseCancelDate
                    Else
                        .LapseCancelDateSpecified = False
                    End If

                    .ReferredAtRenewal = r_oMta.ReferredAtRenewal
                    .ReferredAtRenewalSpecified = r_oMta.ReferredAtRenewal
                    .ReferredOnMTA = r_oMta.ReferredOnMTA
                    .ReferredOnMTASpecified = r_oMta.ReferredOnMTA
                    .IsReinstatement = r_oMta.IsReinstatement

                    If String.IsNullOrEmpty(r_oMta.TranactionType) = False Then
                        If r_oMta.TranactionType.Trim.ToUpper = "MTA" Then
                            .TransactionType = TransactionType.MTA
                        ElseIf r_oMta.TranactionType.Trim.ToUpper = "MTC" Then
                            .TransactionType = TransactionType.MTC
                        ElseIf r_oMta.TranactionType.Trim.ToUpper = "MTR" Then
                            .TransactionType = TransactionType.MTR
                        End If
                    End If

                    If r_oMta.RenewalDayNo > 0 Then
                        .RenewalDayNo = r_oMta.RenewalDayNo
                        .RenewalDayNoSpecified = True
                    Else
                        .RenewalDayNoSpecified = False
                    End If


                End With


                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oAddMtaQuoteResponse = oSAM.AddMtaQuote(oAddMtaQuoteRequest)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.




                With oAddMtaQuoteResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        r_oMta.InsuranceFileKey = .InsuranceFileKey
                        r_oMta.QuoteExpiryDate = .QuoteExpiryDate
                        r_oMta.QuoteTimeStamp = .QuoteTimeStamp
                        r_oMta.CanBeAddedToPFPlan = .CanBeAddedToPfPlan
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("AddMtaQuote executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("r_oMta = " & r_oMta.Print.Replace("<br />", vbCrLf))

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
                oAddMtaQuoteRequest = Nothing
                oAddMtaQuoteResponse = Nothing
            End Try


        End SyncLock
    End Sub

    Public Overrides Sub UpdateStandardPolicyWordings(ByRef oQuote As NexusProvider.Quote,
                                                      Optional ByVal v_sBranchCode As String = Nothing)

        Dim oSAM As PureServiceClient 'SAMForInsuranceV2's Object
        Dim oUpdateStandardPolicyWordingsRequest As UpdateStandardPolicyWordingsRequestType    ' Request Type
        Dim oUpdateStandardPolicyWordingsResponse As UpdateStandardPolicyWordingsResponseType    ' Response Type
        Dim oSW As BaseUpdateStandardPolicyWordingsRequestTypePolicyStandardWordingsRow
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

                If oQuote.InsuranceFileKey = 0 Then
                    Throw New ArgumentNullException("InsuranceFileKey")
                Else
                    .InsuranceFileKey = oQuote.InsuranceFileKey
                End If
                .TimeStamp = oQuote.TimeStamp

                .PolicyStandardWordings = New List(Of BaseUpdateStandardPolicyWordingsRequestTypePolicyStandardWordingsRow)
                For iCount As Integer = 0 To oQuote.StandardPolicyWordings.Count - 1
                    oSW = New BaseUpdateStandardPolicyWordingsRequestTypePolicyStandardWordingsRow
                    oSW.Code = oQuote.StandardPolicyWordings(iCount).StandardPolicyWordingCode
                    oSW.DocumentTemplateKey = oQuote.StandardPolicyWordings(iCount).StandardPolicyWordingID
                    .PolicyStandardWordings.Add(oSW)
                Next



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
                    oQuote.TimeStamp = .TimeStamp
                End If
            End With

            If Logger.IsLoggingEnabled Then
                sbLogMessage.AppendLine("WorkManager executed ok" & vbCrLf)
                sbLogMessage.AppendLine("Input:" & oQuote.Print() & vbCrLf)
                If Not IsNothing(v_sBranchCode) Then
                    sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                Else
                    sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                End If

                sbLogMessage.AppendLine("Returned " & oQuote.Print() & "results" & vbCrLf)

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

    End Sub

#Region "WPR33-Backdated MTA"
    ''' <summary>
    ''' If MTAEffectiveDate is out of synch then this method need to be called.
    ''' This will replace the backdated versions and will create a new version.
    ''' This method will return backdated versions as collection in response.
    ''' If any validation becomes fail in SAM then validation message will be returned in FailureReason
    ''' property and BackDatedVersions collection will be nothing.
    ''' </summary>
    ''' <param name="oMta">MTA Details</param>
    ''' <param name="sFailureReason">By Ref parameter so value for this parameter will be return if any failure occurs</param>
    ''' <param name="v_sBranchCode">branch code</param>
    ''' <returns>backdated versions</returns>
    ''' <remarks></remarks>
    Public Overrides Function AddBackdatedMtaQuote(ByVal oMta As MTA,
                                    ByRef sFailureReason As String,
                                    Optional ByVal v_sBranchCode As String = Nothing) As PolicyCollection
        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oAddBackdatedMtaQuoteRequest As AddBackDatedMTAQuoteRequestType
            Dim oAddBackdatedMtaQuoteResponse As AddBackDatedMTAQuoteResponseType
            Dim oPolicyVersion As Policy
            Dim oPolicyVersions As PolicyCollection
            Dim oBackdatedTransactionRow As BaseAddBackDatedMTAQuoteResponseTypeRow
            Dim PolicyRisk As Risk
            Dim PolicyRiskCollection As RiskCollection
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oAddBackdatedMtaQuoteRequest = New AddBackDatedMTAQuoteRequestType
                oAddBackdatedMtaQuoteResponse = New AddBackDatedMTAQuoteResponseType
                oPolicyVersions = New PolicyCollection
                sbLogMessage = New StringBuilder

                'oSAM.Timeout = 300000
                With oAddBackdatedMtaQuoteRequest
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
                    'Party key for the selected party
                    .PartyCnt = oMta.PartyKey
                    'Insurance Folder key for selected quote
                    .InsuranceFolderKey = oMta.InsuranceFolderKey
                    'InsuranceFileKey for selected quote
                    If oMta.InsuranceFileKey > 0 Then
                        .InsuranceFileKey = oMta.InsuranceFileKey
                    Else
                        Throw New ArgumentNullException("MtaQuote.InsuranceFileKey")
                    End If

                    'MTA Effective date
                    If oMta.MtaEffectiveDate = Date.MinValue Then
                        Throw New ArgumentNullException("MtaQuote.EffectiveDate")
                    Else
                        .EffectiveDate = oMta.MtaEffectiveDate
                    End If

                    If Not String.IsNullOrEmpty(oMta.TranactionType) Then
                        If oMta.TranactionType.Trim.ToUpper = "MTA" Then
                            .TransactionType = TransactionType.MTA
                        ElseIf oMta.TranactionType.Trim.ToUpper = "MTC" Then
                            .TransactionType = TransactionType.MTC
                        ElseIf oMta.TranactionType.Trim.ToUpper = "MTR" Then
                            .TransactionType = TransactionType.MTR
                        End If
                    End If
                    oAddBackdatedMtaQuoteRequest.IsInteractive = oMta.IsInteractive
                End With


                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oAddBackdatedMtaQuoteResponse = oSAM.AddBackDatedMTAQuote(oAddBackdatedMtaQuoteRequest)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.




                With oAddBackdatedMtaQuoteResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        'Create a collection of PolicyCollection type from backdated versions retrieved from the response





                        If oAddBackdatedMtaQuoteResponse.BackdatedTransactions IsNot Nothing Then
                            For iCt As Integer = 0 To oAddBackdatedMtaQuoteResponse.BackdatedTransactions.Count - 1
                                oBackdatedTransactionRow = oAddBackdatedMtaQuoteResponse.BackdatedTransactions(iCt)

                                If iCt > 0 Then
                                    If (oBackdatedTransactionRow.InsuranceFileCnt = oPolicyVersions(oPolicyVersions.Count - 1).InsuranceFileKey) _
                                        And oAddBackdatedMtaQuoteRequest.TransactionType <> TransactionType.MTC Then

                                        PolicyRisk = New Risk
                                        PolicyRisk.InsuranceFileKey = oBackdatedTransactionRow.InsuranceFileCnt
                                        PolicyRisk.RiskKey = oBackdatedTransactionRow.RiskCnt
                                        PolicyRisk.Description = oBackdatedTransactionRow.RiskTypeDescription
                                        PolicyRisk.RiskTypeCode = oBackdatedTransactionRow.RiskDescription
                                        PolicyRisk.StatusCode = oBackdatedTransactionRow.QuoteStatus
                                        'We need to show premium for individual risk also
                                        PolicyRisk.Premium = oBackdatedTransactionRow.MTAPremium
                                        PolicyRisk.StatusDescription = oBackdatedTransactionRow.QuoteStatus
                                        oPolicyVersions(oPolicyVersions.Count - 1).Premium = oPolicyVersions(oPolicyVersions.Count - 1).Premium + oBackdatedTransactionRow.MTAPremium
                                        oPolicyVersions(oPolicyVersions.Count - 1).Risks.Add(PolicyRisk)
                                    Else
                                        PolicyRiskCollection = New RiskCollection
                                        oPolicyVersion = New Policy()
                                        oPolicyVersion.InsuranceFileKey = oBackdatedTransactionRow.InsuranceFileCnt
                                        oPolicyVersion.PolicyVersion = oBackdatedTransactionRow.PolicyVersion
                                        oPolicyVersion.PolicyTypeDescription = oBackdatedTransactionRow.PolicyType
                                        oPolicyVersion.CoverStartDate = oBackdatedTransactionRow.CoverStartDate
                                        oPolicyVersion.CoverEndDate = oBackdatedTransactionRow.CoverEndDate
                                        oPolicyVersion.Premium = oBackdatedTransactionRow.MTAPremium
                                        oPolicyVersion.PolicyStatus = oBackdatedTransactionRow.Status
                                        oPolicyVersion.CurrencyCode = oBackdatedTransactionRow.CurrencyCode


                                        PolicyRisk = New Risk
                                        PolicyRisk.InsuranceFileKey = oBackdatedTransactionRow.InsuranceFileCnt
                                        PolicyRisk.RiskKey = oBackdatedTransactionRow.RiskCnt
                                        PolicyRisk.Description = oBackdatedTransactionRow.RiskTypeDescription
                                        PolicyRisk.RiskTypeCode = oBackdatedTransactionRow.RiskDescription
                                        PolicyRisk.StatusCode = oBackdatedTransactionRow.QuoteStatus
                                        'We need to show premium for individual risk also
                                        PolicyRisk.Premium = oBackdatedTransactionRow.MTAPremium
                                        PolicyRisk.StatusDescription = oBackdatedTransactionRow.QuoteStatus
                                        PolicyRiskCollection.Add(PolicyRisk)

                                        oPolicyVersion.Risks = PolicyRiskCollection
                                        oPolicyVersions.Add(oPolicyVersion)
                                    End If
                                Else
                                    'For first version, no risk detail is required as always will be non-editable
                                    PolicyRiskCollection = New RiskCollection
                                    oPolicyVersion = New Policy()
                                    oPolicyVersion.InsuranceFileKey = oBackdatedTransactionRow.InsuranceFileCnt
                                    oPolicyVersion.PolicyVersion = oBackdatedTransactionRow.PolicyVersion
                                    oPolicyVersion.PolicyTypeDescription = oBackdatedTransactionRow.PolicyType
                                    oPolicyVersion.CoverStartDate = oBackdatedTransactionRow.CoverStartDate
                                    oPolicyVersion.CoverEndDate = oBackdatedTransactionRow.CoverEndDate
                                    oPolicyVersion.Premium = oBackdatedTransactionRow.MTAPremium
                                    oPolicyVersion.PolicyStatus = oBackdatedTransactionRow.Status
                                    oPolicyVersion.CurrencyCode = oBackdatedTransactionRow.CurrencyCode

                                    PolicyRisk = New Risk
                                    PolicyRisk.InsuranceFileKey = oBackdatedTransactionRow.InsuranceFileCnt
                                    PolicyRisk.RiskKey = oBackdatedTransactionRow.RiskCnt
                                    PolicyRisk.Description = oBackdatedTransactionRow.RiskTypeDescription
                                    PolicyRisk.RiskTypeCode = oBackdatedTransactionRow.RiskDescription
                                    PolicyRisk.StatusCode = oBackdatedTransactionRow.QuoteStatus
                                    'We need to show premium for individual risk also
                                    PolicyRisk.Premium = oBackdatedTransactionRow.MTAPremium
                                    PolicyRisk.StatusDescription = oBackdatedTransactionRow.QuoteStatus
                                    PolicyRiskCollection.Add(PolicyRisk)

                                    oPolicyVersion.Risks = PolicyRiskCollection
                                    oPolicyVersions.Add(oPolicyVersion)
                                End If
                            Next
                        End If
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("AddBackdatedMtaQuote executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("r_oMta = " & oMta.Print.Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("sFailureReason = " & sFailureReason & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oAddBackdatedMtaQuoteRequest = Nothing
                oAddBackdatedMtaQuoteResponse = Nothing
            End Try

            Return oPolicyVersions

        End SyncLock
    End Function
#End Region

#Region "WPR75-Backdated MTA Rework"
    ''' <summary>
    ''' For getting backdated risk versions
    ''' </summary>
    ''' <param name="v_iInsuranceFileKey">Base insurance file key</param>
    ''' <param name="v_sBranchCode">Branch Code</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetBackdatedMTARiskVersions(ByVal v_iInsuranceFileKey As Integer,
                                        Optional ByVal v_sBranchCode As String = Nothing) As PolicyCollection
        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oGetBackdatedMTARiskVersionsRequest As GetHeaderAndSummariesByKeyRequestType
            Dim oGetBackdatedMTARiskVersionsResponse As AddBackDatedMTAQuoteResponseType
            Dim oPolicyVersion As Policy
            Dim oPolicyVersions As PolicyCollection
            Dim oBackdatedTransactionRow As BaseAddBackDatedMTAQuoteResponseTypeRow
            Dim PolicyRisk As Risk
            Dim PolicyRiskCollection As RiskCollection
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetBackdatedMTARiskVersionsRequest = New GetHeaderAndSummariesByKeyRequestType
                oGetBackdatedMTARiskVersionsResponse = New AddBackDatedMTAQuoteResponseType
                oPolicyVersions = New PolicyCollection
                sbLogMessage = New StringBuilder


                With oGetBackdatedMTARiskVersionsRequest
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

                    'InsuranceFileKey for selected quote
                    If v_iInsuranceFileKey > 0 Then
                        .InsuranceFileKey = v_iInsuranceFileKey
                    Else
                        Throw New ArgumentNullException("v_iInsuranceFileKey")
                    End If

                End With


                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oGetBackdatedMTARiskVersionsResponse = oSAM.GetBackdatedMTARiskVersions(oGetBackdatedMTARiskVersionsRequest)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.




                With oGetBackdatedMTARiskVersionsResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        'Create a collection of PolicyCollection type from backdated versions retrieved from the response




                        If oGetBackdatedMTARiskVersionsResponse.BackdatedTransactions IsNot Nothing Then
                            For iCt As Integer = 0 To oGetBackdatedMTARiskVersionsResponse.BackdatedTransactions.Count - 1
                                oBackdatedTransactionRow = oGetBackdatedMTARiskVersionsResponse.BackdatedTransactions(iCt)
                                If iCt > 0 Then
                                    If oBackdatedTransactionRow.InsuranceFileCnt = oPolicyVersions(oPolicyVersions.Count - 1).InsuranceFileKey Then
                                        PolicyRisk = New Risk
                                        PolicyRisk.InsuranceFileKey = oBackdatedTransactionRow.InsuranceFileCnt
                                        PolicyRisk.RiskKey = oBackdatedTransactionRow.RiskCnt
                                        PolicyRisk.Description = oBackdatedTransactionRow.RiskTypeDescription
                                        PolicyRisk.RiskTypeCode = oBackdatedTransactionRow.RiskDescription
                                        PolicyRisk.StatusCode = oBackdatedTransactionRow.QuoteStatus
                                        'We need to show premium for individual risk also
                                        PolicyRisk.Premium = oBackdatedTransactionRow.MTAPremium
                                        PolicyRisk.StatusDescription = oBackdatedTransactionRow.QuoteStatus
                                        oPolicyVersions(oPolicyVersions.Count - 1).Premium = oPolicyVersions(oPolicyVersions.Count - 1).Premium + oBackdatedTransactionRow.MTAPremium
                                        oPolicyVersions(oPolicyVersions.Count - 1).Risks.Add(PolicyRisk)
                                    Else
                                        PolicyRiskCollection = New RiskCollection
                                        oPolicyVersion = New Policy()
                                        oPolicyVersion.InsuranceFileKey = oBackdatedTransactionRow.InsuranceFileCnt
                                        oPolicyVersion.PolicyVersion = oBackdatedTransactionRow.PolicyVersion
                                        oPolicyVersion.PolicyTypeDescription = oBackdatedTransactionRow.PolicyType
                                        oPolicyVersion.CoverStartDate = oBackdatedTransactionRow.CoverStartDate
                                        oPolicyVersion.CoverEndDate = oBackdatedTransactionRow.CoverEndDate
                                        oPolicyVersion.Premium = oBackdatedTransactionRow.MTAPremium
                                        oPolicyVersion.PolicyStatus = oBackdatedTransactionRow.Status
                                        oPolicyVersion.CurrencyCode = oBackdatedTransactionRow.CurrencyCode

                                        PolicyRisk = New Risk
                                        PolicyRisk.InsuranceFileKey = oBackdatedTransactionRow.InsuranceFileCnt
                                        PolicyRisk.RiskKey = oBackdatedTransactionRow.RiskCnt
                                        PolicyRisk.Description = oBackdatedTransactionRow.RiskTypeDescription
                                        PolicyRisk.RiskTypeCode = oBackdatedTransactionRow.RiskDescription
                                        PolicyRisk.StatusCode = oBackdatedTransactionRow.QuoteStatus
                                        'We need to show premium for individual risk also
                                        PolicyRisk.Premium = oBackdatedTransactionRow.MTAPremium
                                        PolicyRisk.StatusDescription = oBackdatedTransactionRow.QuoteStatus
                                        PolicyRiskCollection.Add(PolicyRisk)

                                        oPolicyVersion.Risks = PolicyRiskCollection
                                        oPolicyVersions.Add(oPolicyVersion)
                                    End If
                                Else
                                    'For first version, no risk detail is required as always will be non-editable
                                    PolicyRiskCollection = New RiskCollection
                                    oPolicyVersion = New Policy()
                                    oPolicyVersion.InsuranceFileKey = oBackdatedTransactionRow.InsuranceFileCnt
                                    oPolicyVersion.PolicyVersion = oBackdatedTransactionRow.PolicyVersion
                                    oPolicyVersion.PolicyTypeDescription = oBackdatedTransactionRow.PolicyType
                                    oPolicyVersion.CoverStartDate = oBackdatedTransactionRow.CoverStartDate
                                    oPolicyVersion.CoverEndDate = oBackdatedTransactionRow.CoverEndDate
                                    oPolicyVersion.Premium = oBackdatedTransactionRow.MTAPremium
                                    oPolicyVersion.PolicyStatus = oBackdatedTransactionRow.Status
                                    oPolicyVersion.CurrencyCode = oBackdatedTransactionRow.CurrencyCode

                                    PolicyRisk = New Risk
                                    PolicyRisk.InsuranceFileKey = oBackdatedTransactionRow.InsuranceFileCnt
                                    PolicyRisk.RiskKey = oBackdatedTransactionRow.RiskCnt
                                    PolicyRisk.Description = oBackdatedTransactionRow.RiskTypeDescription
                                    PolicyRisk.RiskTypeCode = oBackdatedTransactionRow.RiskDescription
                                    PolicyRisk.StatusCode = oBackdatedTransactionRow.QuoteStatus
                                    'We need to show premium for individual risk also
                                    PolicyRisk.Premium = oBackdatedTransactionRow.MTAPremium
                                    PolicyRisk.StatusDescription = oBackdatedTransactionRow.QuoteStatus
                                    PolicyRiskCollection.Add(PolicyRisk)

                                    oPolicyVersion.Risks = PolicyRiskCollection
                                    oPolicyVersions.Add(oPolicyVersion)
                                End If
                            Next
                        End If
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetBackdatedMTARiskVersions executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iInsuranceFileKey = " & v_iInsuranceFileKey.ToString() & vbCrLf)

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
                oGetBackdatedMTARiskVersionsRequest = Nothing
                oGetBackdatedMTARiskVersionsResponse = Nothing
            End Try
            Return oPolicyVersions

        End SyncLock
    End Function


#End Region
#Region "CancelMTAQuote"
    ''' <summary>
    ''' It is a overridable webmethod function that is used to call the SAM method
    ''' </summary>
    ''' <param name="nInsuranceFileKey"></param>
    ''' <param name="sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function CancelMTAQuote(ByVal nInsuranceFileKey As Integer, ByVal v_bTimeStamp As Byte(), Optional ByVal sBranchCode As String = Nothing)
        SyncLock oLock

            Dim oSAM As SAMForInsuranceV2 = InitializeWebMethod()
            Dim oCancelMTAQuoteRequest As New CancelMTAQuoteRequestType
            Dim oCancelMTAQuoteResponse As New CancelMTAQuoteResponseType

            With oCancelMTAQuoteRequest
                If String.IsNullOrEmpty(sBranchCode) Then
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
                    .BranchCode = sBranchCode
                End If

                If nInsuranceFileKey > 0 Then
                    .InsuranceFileKey = nInsuranceFileKey
                Else
                    Throw New ArgumentException("InsuranceFileKey Missing")
                End If
                .QuoteTimeStamp = v_bTimeStamp
            End With

            Try
                Using trace As New Tracer(Category.Trace)
                    oCancelMTAQuoteResponse = oSAM.CancelMTAQuote(oCancelMTAQuoteRequest)
                End Using
            Finally
                oSAM.Dispose()
            End Try

            With oCancelMTAQuoteResponse
                If .Errors IsNot Nothing Then
                    'Process the error object if errors, and throw as a single exception
                    Throw New NexusException(.Errors)
                End If
            End With

            Dim sbLogMessage As New StringBuilder
            sbLogMessage.AppendLine("CancelMTAQuote executed ok" & vbCrLf)
            '' we don't have input here, we have the input on Credentials Method
            sbLogMessage.AppendLine("Input : " & vbCrLf)

            If Not IsNothing(sBranchCode) Then
                sbLogMessage.AppendLine("sBranchCode = " & sBranchCode.ToString & vbCrLf)
            Else
                sbLogMessage.AppendLine("sBranchCode = nothing" & vbCrLf)
            End If

            sbLogMessage.AppendLine("nInsuranceFileKey = " & nInsuranceFileKey.ToString & vbCrLf)

            sbLogMessage.AppendLine("Output : " & vbCrLf)

            Dim logEntry As New LogEntry()

            logEntry.Categories.Clear()
            logEntry.Categories.Add(Category.General)
            logEntry.Priority = Priority.Normal
            logEntry.Severity = TraceEventType.Verbose
            logEntry.Message = sbLogMessage.ToString
            Logger.Write(logEntry)

        End SyncLock
    End Function
#End Region

End Class
