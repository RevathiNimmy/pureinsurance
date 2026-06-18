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

    Public Overrides Sub DeleteRenewal(ByVal v_oQuote As Quote,
                                       Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock

            Dim oSAM As PureServiceClient 'SAMForInsuranceV2's Object
            Dim oDeleteRenewalRequest As DeleteRenewalRequestType 'Request Type
            Dim oDeleteRenewalResponse As DeleteRenewalResponseType 'Response Type
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oDeleteRenewalRequest = New DeleteRenewalRequestType
                oDeleteRenewalResponse = New DeleteRenewalResponseType
                sbLogMessage = New StringBuilder


                With oDeleteRenewalRequest
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


                    If v_oQuote.InsuranceFileKey > 0 Then
                        .InsuranceFileKey = v_oQuote.InsuranceFileKey
                    Else
                        Throw New ArgumentNullException("Quote.InsuranceFileKey")
                    End If

                    If v_oQuote.TimeStamp Is Nothing Then
                        Throw New ArgumentNullException("Quote.TimeStamp")
                    Else
                        .QuoteTimeStamp = v_oQuote.TimeStamp
                    End If

                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oDeleteRenewalResponse = oSAM.DeleteRenewal(oDeleteRenewalRequest)
                End Using


                ' Disposing the SAM's object

                If oDeleteRenewalResponse IsNot Nothing Then
                    With oDeleteRenewalResponse
                        If .Errors IsNot Nothing Then
                            'Process the error object if errors, and throw as a single exception
                            Throw New NexusException(.Errors)
                        End If
                    End With
                End If

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("DeleteRenewal executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_oQuote = " & v_oQuote.Print.Replace("<br />", vbCrLf))

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
                oDeleteRenewalRequest = Nothing
                oDeleteRenewalResponse = Nothing
            End Try


        End SyncLock
    End Sub

    Public Overrides Sub LapseRenewal(ByRef r_oQuote As Quote,
                                    ByVal v_sLapseReasonCode As String,
                                    Optional ByVal v_sBranchCode As String = Nothing)


        SyncLock oLock

            Dim oSAM As PureServiceClient 'SAMForInsuranceV2's Object
            Dim oLapseRenewalRequest As LapseRenewalRequestType 'Request Type
            Dim oLapseRenewalResponse As LapseRenewalResponseType 'Response Type
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oLapseRenewalRequest = New LapseRenewalRequestType
                oLapseRenewalResponse = New LapseRenewalResponseType
                sbLogMessage = New StringBuilder


                With oLapseRenewalRequest
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

                    If r_oQuote.InsuranceFileKey > 0 Then
                        .InsuranceFileKey = r_oQuote.InsuranceFileKey
                    Else
                        Throw New ArgumentNullException("Quote.InsuranceFileKey")
                    End If

                    If r_oQuote.TimeStamp Is Nothing Then
                        Throw New ArgumentNullException("Quote.TimeStamp")
                    Else
                        .QuoteTimeStamp = r_oQuote.TimeStamp
                    End If

                    If String.IsNullOrEmpty(v_sLapseReasonCode) Then
                        Throw New ArgumentNullException("v_sLapseReasonCode")
                    Else
                        .LapseReasonCode = v_sLapseReasonCode
                    End If

                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oLapseRenewalResponse = oSAM.LapseRenewal(oLapseRenewalRequest)
                End Using

                ' Disposing the SAM's object


                With oLapseRenewalResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        r_oQuote.TimeStamp = .QuoteTimeStamp
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("LapseRenewal executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("r_oQuote = " & r_oQuote.Print.Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oLapseRenewalRequest = Nothing
                oLapseRenewalResponse = Nothing
            End Try


        End SyncLock

    End Sub

    Public Overrides Sub UpdateRenewalStatus(ByRef r_oQuote As Quote,
                                            ByVal v_sRenewalStatusCode As String,
                                            Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock

            Dim oSAM As PureServiceClient 'SAMForInsuranceV2's Object
            Dim oUpdateRenewalStatusRequest As UpdateRenewalStatusRequestType 'Request Type
            Dim oUpdateRenewalStatusResponse As UpdateRenewalStatusResponseType 'Response Type
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oUpdateRenewalStatusRequest = New UpdateRenewalStatusRequestType
                oUpdateRenewalStatusResponse = New UpdateRenewalStatusResponseType
                sbLogMessage = New StringBuilder


                With oUpdateRenewalStatusRequest
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

                    If r_oQuote.InsuranceFileKey > 0 Then
                        .InsuranceFileKey = r_oQuote.InsuranceFileKey
                    Else
                        Throw New ArgumentNullException("Quote.InsuranceFileKey")
                    End If

                    If r_oQuote.TimeStamp Is Nothing Then
                        Throw New ArgumentNullException("Quote.TimeStamp")
                    Else
                        .QuoteTimeStamp = r_oQuote.TimeStamp
                    End If

                    If String.IsNullOrEmpty(v_sRenewalStatusCode) Then
                        Throw New ArgumentNullException("v_sRenewalStatusCode")
                    Else
                        .RenewalStatusCode = v_sRenewalStatusCode
                    End If

                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oUpdateRenewalStatusResponse = oSAM.UpdateRenewalStatus(oUpdateRenewalStatusRequest)
                End Using

                ' Disposing the SAM's object


                With oUpdateRenewalStatusResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        r_oQuote.TimeStamp = .QuoteTimeStamp
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("UpdateRenewalStatus executed ok" & vbCrLf)
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
                oUpdateRenewalStatusRequest = Nothing
                oUpdateRenewalStatusResponse = Nothing
            End Try


        End SyncLock
    End Sub

    Public Overrides Function GenerateInvite(ByVal v_oDocumentType As DocumentType,
                                            ByVal v_bSpoolDocumentOnly As Boolean,
                                            ByRef r_oQuote As Quote,
                                            ByVal v_sDocumentExtractionPath As String,
                                            Optional ByVal v_sBranchCode As String = Nothing) As String

        SyncLock oLock

            Dim oSAM As PureServiceClient 'SAMForInsuranceV2's Object
            Dim oGenerateInviteRequest As GenerateInviteRequestType 'Request Type
            Dim oGenerateInviteResponse As GenerateInviteResponseType 'Response Type
            Dim sDirectoryName As String = ""

            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGenerateInviteRequest = New GenerateInviteRequestType
                oGenerateInviteResponse = New GenerateInviteResponseType
                sbLogMessage = New StringBuilder

                If (v_sDocumentExtractionPath.Contains(".")) Then
                    sDirectoryName = Left(v_sDocumentExtractionPath, v_sDocumentExtractionPath.LastIndexOf("\"))
                Else
                    sDirectoryName = v_sDocumentExtractionPath
                End If


                With oGenerateInviteRequest
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

                    'Checking the Insurance File Key
                    If r_oQuote.InsuranceFileKey > 0 Then
                        .InsuranceFileKey = r_oQuote.InsuranceFileKey
                    Else
                        Throw New ArgumentNullException("Quote.InsuranceFileKey")
                    End If

                    'Checking the DocumentExtractionDirectory
                    If String.IsNullOrEmpty(v_sDocumentExtractionPath) Then
                        Throw New ArgumentNullException("DocumentExtractionDirectory")
                    Else
                        If Not IO.Directory.Exists(sDirectoryName) Then
                            'create the directory that the file will be written to
                            IO.Directory.CreateDirectory(sDirectoryName)
                        End If
                    End If

                    'Checking the DocumentType
                    Select Case v_oDocumentType
                        Case DocumentType.None
                            Throw New ArgumentException("Can not be DocumentType.None", "DocumentType")
                        Case DocumentType.HTML
                            'changes as of Pure 2.01 mean that
                            Throw New ArgumentException("Can not be DocumentType.HTML, no longer supported", "DocumentType")
                        Case DocumentType.PDF
                            .OutputAsPDF = True
                            .OutputAsHTML = False
                        Case DocumentType.DOCX
                            .OutputAsPDF = False
                            .OutputAsHTML = False
                    End Select

                    'Checking the QuoteTimeStamp 
                    If r_oQuote.TimeStamp Is Nothing Then
                        Throw New ArgumentNullException("Quote.TimeStamp")
                    Else
                        .QuoteTimeStamp = r_oQuote.TimeStamp
                    End If

                    If v_bSpoolDocumentOnly > 0 Then
                        .SpoolDocumentOnly = v_bSpoolDocumentOnly
                        .SpoolDocumentOnlySpecified = True
                    Else
                        .SpoolDocumentOnlySpecified = False
                    End If
                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oGenerateInviteResponse = oSAM.GenerateInvite(oGenerateInviteRequest)
                End Using

                ' Disposing the SAM's object


                With oGenerateInviteResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        'in case renewal document is configured in BO write the file, returned as a byte array in SpooledZipFile to disk
                        If .SpooledZipFile IsNot Nothing Then
                            Dim fsOutputFile As IO.FileStream = IO.File.OpenWrite(v_sDocumentExtractionPath)
                            fsOutputFile.Write(.SpooledZipFile, 0, .SpooledZipFile.Length)
                            fsOutputFile.Close()
                        End If
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GenerateInvite executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("r_oQuote.InsuranceFileKey = " & r_oQuote.InsuranceFileKey.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_oDocumentType = " & v_oDocumentType.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_sDocumentExtractionDirectory = " & v_sDocumentExtractionPath.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & v_sDocumentExtractionPath.ToString & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGenerateInviteRequest = Nothing
                oGenerateInviteResponse = Nothing
                sDirectoryName = Nothing
            End Try



            Return v_sDocumentExtractionPath 'todo - should be a sub, we're just passing back an input

        End SyncLock

    End Function

    Public Overrides Function GetPoliciesInRenewal(ByVal v_iPartyKey As Integer,
                                                ByVal v_iAgentKey As Integer,
                                                ByVal v_sProductCode As String,
                                                ByVal v_dtRenewalDate As DateTime,
                                                ByVal v_bForAccept As Boolean,
                                                ByVal v_bDirectOnly As Boolean,
                                                Optional ByVal v_sBranchCode As String = Nothing,
                                                Optional ByVal v_sInsuranceRef As String = Nothing,
                                                Optional ByVal v_bShowUserOnly As Boolean = False) As PolicyCollection

        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oGetPoliciesInRenewalRequest As GetPoliciesInRenewalRequestType
            Dim oGetPoliciesInRenewalResponse As GetPoliciesInRenewalResponseType
            Dim oPolicies As PolicyCollection = Nothing
            Dim oNewPolicy As Policy
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetPoliciesInRenewalRequest = New GetPoliciesInRenewalRequestType
                oGetPoliciesInRenewalResponse = New GetPoliciesInRenewalResponseType
                sbLogMessage = New StringBuilder


                With oGetPoliciesInRenewalRequest
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

                    If v_iPartyKey > 0 Then
                        .PartyKey = v_iPartyKey
                        .PartyKeySpecified = True
                    Else
                        .PartyKeySpecified = False

                    End If


                    If v_iAgentKey > 0 Then
                        .AgentKey = v_iAgentKey
                        .AgentKeySpecified = True
                    Else
                        .AgentKeySpecified = False

                    End If

                    If v_bDirectOnly = True Then
                        .DirectOnly = v_bDirectOnly
                        .DirectOnlySpecified = True
                    Else
                        .DirectOnlySpecified = False

                    End If

                    If v_bForAccept = True Then
                        .ForAccept = v_bForAccept
                        .ForAcceptSpecified = True
                    Else
                        .ForAcceptSpecified = False

                    End If

                    'if the passed parameter v_sProductCode is empty
                    'If String.IsNullOrEmpty(v_sProductCode) Then
                    '    Throw New ArgumentNullException("v_sProductCode")
                    'Else
                    .ProductCode = v_sProductCode
                    'End If

                    If v_dtRenewalDate <> Date.MinValue Then
                        .RenewalDateSpecified = True
                        .RenewalDate = v_dtRenewalDate
                    Else
                        .RenewalDateSpecified = False
                    End If

                    If (v_sInsuranceRef IsNot Nothing) Then


                        If (v_sInsuranceRef.Trim() = "") Then

                            '.InsuranceRefSpecified = False

                        Else

                            '.InsuranceRefSpecified = True
                            .InsuranceRef = v_sInsuranceRef
                        End If
                    End If

                    If (v_bShowUserOnly = True) Then
                        .SearchType = ContactUserSearchType.LoggedInUserOnly
                        .SearchTypeSpecified = True
                    Else
                        .SearchType = ContactUserSearchType.All
                        .SearchTypeSpecified = False
                    End If

                    If System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork").Portals.Portal(HttpContext.Current.Cache("PortalID")).EnableMasterClientAssociate Then
                        .RetrieveAssociates = True
                    Else
                        .RetrieveAssociates = False
                    End If
                End With


                Using trace As New Tracer(Category.Trace)
                    oGetPoliciesInRenewalResponse = oSAM.GetPoliciesInRenewal(oGetPoliciesInRenewalRequest)
                End Using

                With oGetPoliciesInRenewalResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        If .Policies IsNot Nothing AndAlso .Policies.Count > 0 Then

                            oPolicies = New PolicyCollection


                            For Each oPolicy As BaseGetPoliciesInRenewalResponseTypeRow In .Policies
                                oNewPolicy = New Policy(oPolicy.InsuranceFileRef)

                                With oNewPolicy

                                    .RenewalStatusKey = oPolicy.RenewalStatusKey
                                    .PartyKey = oPolicy.PartyKey
                                    .BranchCode = oPolicy.BranchCode
                                    .PartyName = oPolicy.PartyName
                                    .InsuranceFileRef = oPolicy.InsuranceFileRef
                                    .InsuranceFileKey = oPolicy.InsuranceFileKey
                                    .InsuranceFolderKey = oPolicy.InsuranceFolderKey
                                    .InsuranceFileStatusDescription = oPolicy.InsuranceFileStatusDescription
                                    .InsuranceFileTypeDescription = oPolicy.InsuranceFileTypeDescription
                                    .RenewalStatusTypeCode = oPolicy.RenewalStatusTypeCode
                                    .RenewalStatusTypeDescription = oPolicy.RenewalStatusTypeDescription
                                    .CoverStartDate = oPolicy.CoverStartDate
                                    .CoverEndDate = oPolicy.CoverEndDate
                                    .RenewalDate = oPolicy.RenewalDate
                                    .RenewalPremium = oPolicy.RenewalPremium
                                    .ProductCode = oPolicy.ProductCode
                                    .ProductDescription = oPolicy.ProductDescription
                                    .LeadAgentKey = oPolicy.LeadAgentKey
                                    .LeadAgent = oPolicy.LeadAgent
                                    .AccHandler = oPolicy.AccHandler
                                    .ClaimIndicator = oPolicy.ClaimIndicator
                                    .IsClosed = oPolicy.IsClosed
                                    .IsTrueMonthlyPolicy = oPolicy.IsTrueMonthlyPolicy
                                    .AnniversaryCopy = oPolicy.AnniversaryCopy
                                    .AssociatedClients = oPolicy.AssociatedClients


                                End With

                                oPolicies.Add(oNewPolicy)

                            Next

                        End If

                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetPoliciesInRenewal executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iPartyKey = " & v_iPartyKey.ToString & vbCrLf)
                    'sbLogMessage.AppendLine("v_sProductCode = " & v_sProductCode.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_dtRenewalDate = " & v_dtRenewalDate.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_bForAccept = " & v_bForAccept.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_bDirectOnly = " & v_bDirectOnly.ToString & vbCrLf)

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
                    If oGetPoliciesInRenewalResponse.Policies IsNot Nothing Then
                        sbLogMessage.AppendLine("Returned " & oPolicies.Print.ToString & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetPoliciesInRenewalRequest = Nothing
                oGetPoliciesInRenewalResponse = Nothing
            End Try


            Return oPolicies

        End SyncLock

    End Function

    Public Overrides Function FindPolicy(ByVal v_sInsuranceRef As String,
                                     ByVal v_sRiskIndex As String,
                                     ByVal v_sClientShortName As String,
                                     ByVal v_oQuoteType As InsuranceFileTypes,
                                     ByVal v_bShowLapsedOnly As Boolean,
                                     Optional ByVal v_iMaxRowsToFetch As Integer = 0,
                                     Optional ByVal v_sBranchCode As String = Nothing,
                                     Optional ByVal v_bShowCancelledForEvents As Boolean = False) As InsuranceFileDetailsCollection

        SyncLock oLock

            Dim oSAM As PureServiceClient 'SAMForInsuranceV2's Object
            Dim oFindPolicyRequest As FindPolicyRequestType 'Request Type
            Dim oFindPolicyResponse As FindPolicyResponseType 'Response Type
            Dim oInsuranceFileDetailsCollection As InsuranceFileDetailsCollection = Nothing
            Dim oNewInsuranceFileDetails As InsuranceFileDetails
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oFindPolicyRequest = New FindPolicyRequestType
                oFindPolicyResponse = New FindPolicyResponseType
                sbLogMessage = New StringBuilder


                With oFindPolicyRequest
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
                    .ClientShortName = v_sClientShortName
                    .InsuranceRef = v_sInsuranceRef
                    .RiskIndex = v_sRiskIndex

                    'Checking the Quote Type
                    Select Case v_oQuoteType
                        Case InsuranceFileType.MTAQUOTE
                            .QuoteType = InsuranceFileType.MTAQUOTE
                            .QuoteTypeSpecified = True
                        Case InsuranceFileType.POLICY
                            .QuoteType = InsuranceFileType.POLICY
                            .QuoteTypeSpecified = True
                        Case InsuranceFileType.QUOTE
                            .QuoteType = InsuranceFileType.QUOTE
                            .QuoteTypeSpecified = True
                        Case InsuranceFileType.RENEWAL
                            .QuoteType = InsuranceFileType.RENEWAL
                            .QuoteTypeSpecified = True
                        Case Else
                            .QuoteTypeSpecified = False
                    End Select

                    'Checking the v_bShowLapsedOnly
                    If v_bShowLapsedOnly = True Then
                        .ShowLapsedOnly = v_bShowLapsedOnly
                        .ShowLapsedOnlySpecified = v_bShowLapsedOnly

                    Else
                        .ShowLapsedOnly = v_bShowLapsedOnly
                        .ShowLapsedOnlySpecified = v_bShowLapsedOnly
                    End If

                    If v_iMaxRowsToFetch > 0 Then
                        .MaxRowsToFetch = v_iMaxRowsToFetch
                        .MaxRowsToFetchSpecified = True
                    Else
                        .MaxRowsToFetchSpecified = False
                    End If
                    .ShowCancelledForEvents = v_bShowCancelledForEvents

                    If System.Web.Configuration.WebConfigurationManager.GetSection("NexusFrameWork").Portals.Portal(HttpContext.Current.Cache("PortalID")).EnableMasterClientAssociate Then
                        .RetrieveAssociates = True
                    Else
                        .RetrieveAssociates = False
                    End If
                End With


                Using trace As New Tracer(Category.Trace)
                    oFindPolicyResponse = oSAM.FindPolicy(oFindPolicyRequest)
                End Using

                With oFindPolicyResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        If .InsuranceFileDetails IsNot Nothing AndAlso .InsuranceFileDetails.Count > 0 Then

                            oInsuranceFileDetailsCollection = New InsuranceFileDetailsCollection



                            For Each oInsuranceFileDetails As BaseFindPolicyResponseTypeRow In .InsuranceFileDetails

                                oNewInsuranceFileDetails = New InsuranceFileDetails

                                With oNewInsuranceFileDetails

                                    .ClientName = oInsuranceFileDetails.ClientName
                                    .ClientShortName = oInsuranceFileDetails.ClientShortName
                                    .InsuranceFileKey = oInsuranceFileDetails.InsuranceFileKey
                                    .InsuranceFileType = oInsuranceFileDetails.InsuranceFileType
                                    .InsuranceFolderKey = oInsuranceFileDetails.InsuranceFolderKey
                                    .InsuranceRef = oInsuranceFileDetails.InsuranceRef
                                    .LastModifiedDate = oInsuranceFileDetails.LastModifiedDate
                                    .PartyKey = oInsuranceFileDetails.PartyKey
                                    .ProductCode = oInsuranceFileDetails.ProductCode
                                    .ProductDescription = oInsuranceFileDetails.ProductDescription
                                    .RiskIndex = oInsuranceFileDetails.RiskIndex
                                    .Status = oInsuranceFileDetails.Status
                                    .Value = oInsuranceFileDetails.Value
                                    .AssociatedClients = oInsuranceFileDetails.AssociatedClients
                                End With

                                oInsuranceFileDetailsCollection.Add(oNewInsuranceFileDetails)

                            Next

                        End If

                    End If


                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("FindPolicy executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_sInsuranceRef = " & v_sInsuranceRef & vbCrLf)
                    sbLogMessage.AppendLine("v_sRiskIndex  = " & v_sRiskIndex & vbCrLf)
                    sbLogMessage.AppendLine("v_sClientShortName  = " & v_sClientShortName & vbCrLf)
                    sbLogMessage.AppendLine("v_oQuoteType  = " & v_oQuoteType.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_bShowLapsedOnly  = " & v_bShowLapsedOnly.ToString & vbCrLf)

                    If oInsuranceFileDetailsCollection IsNot Nothing Then
                        sbLogMessage.AppendLine("Returned " & oInsuranceFileDetailsCollection.Print.ToString & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oFindPolicyRequest = Nothing
                oFindPolicyResponse = Nothing
            End Try


            Return oInsuranceFileDetailsCollection

        End SyncLock

    End Function

    Public Overrides Function GetAgentDetailsForPolicy(ByVal v_iInsuranceFileKey As Integer,
                                            Optional ByVal v_sBranchCode As String = Nothing) As AgentDetailsForPolicy

        SyncLock oLock

            Dim oSAM As PureServiceClient 'SAMForInsuranceV2's Object
            Dim oGetAgentDetailsForPolicyRequest As GetAgentDetailsForPolicyRequestType 'Request Type
            Dim oGetAgentDetailsForPolicyResponse As GetAgentDetailsForPolicyResponseType 'Response Type
            Dim oAgentDetailsForPolicy As AgentDetailsForPolicy = Nothing
            Dim oAgentContact As NexusProvider.Contact
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetAgentDetailsForPolicyRequest = New GetAgentDetailsForPolicyRequestType
                oGetAgentDetailsForPolicyResponse = New GetAgentDetailsForPolicyResponseType
                oAgentContact = New NexusProvider.Contact
                sbLogMessage = New StringBuilder


                With oGetAgentDetailsForPolicyRequest
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

                    'if the passed parameter v_iInsuranceFileKey is empty
                    If v_iInsuranceFileKey > 0 Then
                        .InsuranceFileKey = v_iInsuranceFileKey
                    Else
                        Throw New ArgumentNullException("v_iInsuranceFileKey")
                    End If

                End With


                Using trace As New Tracer(Category.Trace)
                    oGetAgentDetailsForPolicyResponse = oSAM.GetAgentDetailsForPolicy(oGetAgentDetailsForPolicyRequest)
                End Using

                With oGetAgentDetailsForPolicyResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else

                        oAgentDetailsForPolicy = New AgentDetailsForPolicy()

                        oAgentDetailsForPolicy.Name = .Name
                        oAgentDetailsForPolicy.Shortname = .Shortname
                        oAgentDetailsForPolicy.Address1 = .Address1
                        oAgentDetailsForPolicy.Address2 = .Address2
                        oAgentDetailsForPolicy.Address3 = .Address3
                        oAgentDetailsForPolicy.Address4 = .Address4
                        oAgentDetailsForPolicy.PostalCode = .PostalCode
                        oAgentDetailsForPolicy.AreaCode = .AreaCode
                        oAgentDetailsForPolicy.Number = .Number

                        If Trim(.Extension).Length > 0 Then
                            oAgentDetailsForPolicy.Extension = CInt(.Extension)
                        End If

                        oAgentDetailsForPolicy.ContactTypeKey = .ContactTypeKey
                        oAgentDetailsForPolicy.Code = .Code
                        oAgentDetailsForPolicy.CountryKey = .CountryKey
                        oAgentDetailsForPolicy.Description = .Description
                        oAgentDetailsForPolicy.AddressUsageTypeKey = .AddressUsageTypeKey
                        oAgentDetailsForPolicy.AddressKey = .AddressKey

                        If .Contacts IsNot Nothing AndAlso .Contacts.Count > 0 Then
                            For Each oContact As BaseContactType In .Contacts


                                oAgentContact.AreaCode = oContact.AreaCode
                                oAgentContact.ContactDetailType = oContact.ContactDetail.ItemElementName
                                oAgentContact.Number = oContact.ContactDetail.Item
                                oAgentContact.ContactType = oContact.ContactTypeCode
                                oAgentContact.Description = oContact.Description
                                oAgentContact.Extension = oContact.Extension

                                oAgentDetailsForPolicy.Contacts.Add(oAgentContact)
                            Next
                        End If

                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetAgentDetailsForPolicy executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iInsuranceFileKey = " & v_iInsuranceFileKey.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & oAgentDetailsForPolicy.Print.ToString & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetAgentDetailsForPolicyRequest = Nothing
                oGetAgentDetailsForPolicyResponse = Nothing
            End Try


            Return oAgentDetailsForPolicy

        End SyncLock

    End Function

    Public Overrides Function GetAllocationDetails(ByVal v_iTransDetailKey As Integer,
                                            Optional ByVal v_sBranchCode As String = Nothing) As AllocationDetailsCollections

        SyncLock oLock

            Dim oSAM As PureServiceClient 'SAMForInsuranceV2's Object
            Dim oGetAllocationDetailsRequest As GetAllocationDetailsRequestType 'Request Type
            Dim oGetAllocationDetailsResponse As GetAllocationDetailsResponseType 'Response Type
            Dim oAllocationDetailsCollection As AllocationDetailsCollections = Nothing
            Dim oNewAllocationDetails As AllocationDetails
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetAllocationDetailsRequest = New GetAllocationDetailsRequestType
                oGetAllocationDetailsResponse = New GetAllocationDetailsResponseType
                sbLogMessage = New StringBuilder

                With oGetAllocationDetailsRequest
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

                    'if the passed parameter v_iTransDetailKey is empty
                    If v_iTransDetailKey > 0 Then
                        .TransDetailKey = v_iTransDetailKey
                    Else
                        Throw New ArgumentNullException("v_iTransDetailKey")
                    End If
                End With

                Using trace As New Tracer(Category.Trace)
                    oGetAllocationDetailsResponse = oSAM.GetAllocationDetails(oGetAllocationDetailsRequest)
                End Using

                With oGetAllocationDetailsResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        If .Row IsNot Nothing AndAlso .Row.Count > 0 Then

                            oAllocationDetailsCollection = New AllocationDetailsCollections
                            For Each oAllocationDetails As BaseGetAllocationDetailsResponseTypeRow In .Row
                                oNewAllocationDetails = New AllocationDetails

                                With oNewAllocationDetails
                                    .DocRef = oAllocationDetails.DocRef
                                    .TransDate = oAllocationDetails.TransDate
                                    .AllocatedDate = oAllocationDetails.AllocatedDate
                                    .AllocatedAmount = oAllocationDetails.AllocatedAmount
                                    .OriginalAmount = oAllocationDetails.OriginalAmount
                                    .WriteOffAmount = oAllocationDetails.WriteOffAmount
                                    .DocType = oAllocationDetails.DocType
                                    .InsuranceRef = oAllocationDetails.InsuranceRef
                                    .Account = oAllocationDetails.Account
                                    .User = oAllocationDetails.User
                                    .AllocationKey = oAllocationDetails.AllocationKey
                                    .TransdetailKey = oAllocationDetails.TransDetailKey
                                End With

                                oAllocationDetailsCollection.Add(oNewAllocationDetails)

                            Next
                        End If
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetAllocationDetails executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iTransDetailKey = " & v_iTransDetailKey.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If
                    If Not IsNothing(oAllocationDetailsCollection) Then
                        sbLogMessage.AppendLine("Returned " & oAllocationDetailsCollection.Print.ToString & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetAllocationDetailsRequest = Nothing
                oGetAllocationDetailsResponse = Nothing
            End Try

            Return oAllocationDetailsCollection

        End SyncLock

    End Function

    Public Overrides Function GetPoliciesForRenewalSelection(ByVal v_sProductCode As String,
                                                ByVal v_dtCompareDate As Date,
                                                ByVal v_dtStartDate As Date,
                                                Optional ByVal v_sBranchCode As String = Nothing) As PolicyCollection

        SyncLock oLock

            Dim oSAM As PureServiceClient 'SAMForInsuranceV2's Object
            Dim oGetPoliciesForRenewalSelectionRequest As GetPoliciesForRenewalSelectionRequestType 'Request Type
            Dim oGetPoliciesForRenewalSelectionResponse As GetPoliciesForRenewalSelectionResponseType 'Response Type
            Dim oGetPoliciesForRenewalSelectionCollections As PolicyCollection = Nothing
            Dim oNewGetPoliciesForRenewalSelection As Policy
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetPoliciesForRenewalSelectionRequest = New GetPoliciesForRenewalSelectionRequestType
                oGetPoliciesForRenewalSelectionResponse = New GetPoliciesForRenewalSelectionResponseType
                sbLogMessage = New StringBuilder


                With oGetPoliciesForRenewalSelectionRequest
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

                    'if the passed parameter v_sProductCode is empty
                    If String.IsNullOrEmpty(v_sProductCode) Then
                        Throw New ArgumentNullException("v_sProductCode")
                    Else
                        .ProductCode = v_sProductCode
                    End If

                    .CompareDate = v_dtCompareDate

                    If v_dtStartDate <> Date.MinValue Then
                        .StartDateSpecified = True
                        .StartDate = v_dtStartDate
                    Else
                        .StartDateSpecified = False
                    End If

                End With


                Using trace As New Tracer(Category.Trace)
                    oGetPoliciesForRenewalSelectionResponse = oSAM.GetPoliciesForRenewalSelection(oGetPoliciesForRenewalSelectionRequest)
                End Using

                With oGetPoliciesForRenewalSelectionResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        If .Policies IsNot Nothing AndAlso .Policies.Count > 0 Then

                            oGetPoliciesForRenewalSelectionCollections = New PolicyCollection()



                            For Each oGetPoliciesForRenewalSelection As BaseGetPoliciesForRenewalSelectionResponseTypeRow In .Policies

                                oNewGetPoliciesForRenewalSelection = New Policy(oGetPoliciesForRenewalSelection.InsuranceFileRef)

                                With oNewGetPoliciesForRenewalSelection

                                    .InsuranceFileKey = oGetPoliciesForRenewalSelection.InsuranceFileKey
                                    .InsuranceFolderKey = oGetPoliciesForRenewalSelection.InsuranceFolderKey
                                    .PartyKey = oGetPoliciesForRenewalSelection.PartyKey
                                    .ProductKey = oGetPoliciesForRenewalSelection.ProductKey
                                    .LeadAgentKey = oGetPoliciesForRenewalSelection.LeadAgentKey
                                    .InsuranceFileRef = oGetPoliciesForRenewalSelection.InsuranceFileRef
                                    .CoverStartDate = oGetPoliciesForRenewalSelection.CoverStartDate
                                    .CoverEndDate = oGetPoliciesForRenewalSelection.CoverEndDate
                                    .ClientCode = oGetPoliciesForRenewalSelection.ClientCode
                                    .Client = oGetPoliciesForRenewalSelection.Client
                                    .LeadAgent = oGetPoliciesForRenewalSelection.LeadAgent
                                    .IsAutoRenewable = oGetPoliciesForRenewalSelection.IsAutoRenewable
                                    .ProductDescription = oGetPoliciesForRenewalSelection.ProductDescription
                                    .RenewalDate = oGetPoliciesForRenewalSelection.RenewalDate
                                    .IsClosed = oGetPoliciesForRenewalSelection.IsClosed
                                    .IsInTransferMode = oGetPoliciesForRenewalSelection.IsInTransferMode
                                    .IsTrueMonthlyPolicy = oGetPoliciesForRenewalSelection.IsTrueMonthlyPolicy
                                    .AnniversaryCopy = oGetPoliciesForRenewalSelection.AnniversaryCopy
                                    .RenewalCount = oGetPoliciesForRenewalSelection.RenewalCount

                                End With

                                oGetPoliciesForRenewalSelectionCollections.Add(oNewGetPoliciesForRenewalSelection)

                            Next

                        End If

                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetPoliciesForRenewalSelection executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_sProductCode = " & v_sProductCode.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_dtCompareDate = " & v_dtCompareDate.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_dtStartDate = " & v_dtStartDate.ToString & vbCrLf)


                    sbLogMessage.AppendLine("Returned " & oGetPoliciesForRenewalSelectionCollections.Print.ToString & vbCrLf)

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
                oGetPoliciesForRenewalSelectionRequest = Nothing
                oGetPoliciesForRenewalSelectionResponse = Nothing
            End Try


            Return oGetPoliciesForRenewalSelectionCollections

        End SyncLock

    End Function

    Public Overrides Function GetRenewalStatus(ByVal v_iInsuranceFileKey As Integer,
                                                Optional ByVal v_sBranchCode As String = Nothing) As RenewalStatus

        SyncLock oLock

            Dim oSAM As PureServiceClient 'SAMForInsuranceV2's Object
            Dim oGetRenewalStatusRequest As GetRenewalStatusRequestType 'Request Type
            Dim oGetRenewalStatusResponse As GetRenewalStatusResponseType 'Response Type
            Dim oRenewalStatus As RenewalStatus = Nothing
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetRenewalStatusRequest = New GetRenewalStatusRequestType
                oGetRenewalStatusResponse = New GetRenewalStatusResponseType
                sbLogMessage = New StringBuilder


                With oGetRenewalStatusRequest
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

                    'if the passed parameter v_iInsuranceFileKey is empty
                    If v_iInsuranceFileKey > 0 Then
                        .InsuranceFileKey = v_iInsuranceFileKey
                    Else
                        Throw New ArgumentNullException("v_iInsuranceFileKey")
                    End If

                End With


                Using trace As New Tracer(Category.Trace)
                    oGetRenewalStatusResponse = oSAM.GetRenewalStatus(oGetRenewalStatusRequest)
                End Using

                With oGetRenewalStatusResponse

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else

                        oRenewalStatus = New RenewalStatus()

                        oRenewalStatus.RenewalStatusTypeCode = .RenewalStatusTypeCode
                        oRenewalStatus.RenewalStatusTypeDescription = .RenewalStatusTypeDescription
                        oRenewalStatus.RenewalStatusKey = .RenewalStatusKey
                        oRenewalStatus.InsuranceHolderKey = .InsuranceHolderKey
                        oRenewalStatus.LeadAgentKey = .LeadAgentKey
                        oRenewalStatus.DateCreated = .DateCreated
                        oRenewalStatus.CriticalDate = .CriticalDate
                        oRenewalStatus.IsInvitePrinted = .IsInvitePrinted
                        oRenewalStatus.OriginalInsuranceFileKey = .OriginalInsuranceFileKey
                        oRenewalStatus.DateInvitePrinted = .DateInvitePrinted
                        oRenewalStatus.RenewalExceptionNotes = .RenewalExceptionNotes
                        oRenewalStatus.EmailSent = .EmailSent
                        oRenewalStatus.EmailSentDate = .EmailSentDate
                        oRenewalStatus.ProductCode = .ProductCode
                        oRenewalStatus.RenewalExceptionReasonCode = .RenewalExceptionReasonCode
                        oRenewalStatus.RenewalExceptionReasonDescription = .RenewalExceptionReasonDescription

                    End If

                End With
                If Logger.IsLoggingEnabled Then
                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("GetRenewalStatus executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iInsuranceFileKey = " & v_iInsuranceFileKey.ToString & vbCrLf)
                    sbLogMessage.AppendLine("Returned " & oRenewalStatus.Print.ToString & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetRenewalStatusRequest = Nothing
                oGetRenewalStatusResponse = Nothing
            End Try


            Return oRenewalStatus

        End SyncLock

    End Function

    Public Overrides Sub RunRenewalAccept(ByVal v_iInsuranceFileKey As Integer,
                                            ByVal v_iBatchRenewalJobKey As Integer,
                                            ByVal v_iRecordsCount As Integer,
                                            ByVal v_sGUID As String,
                                            Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock

            Dim oSAM As PureServiceClient 'SAMForInsuranceV2's Object
            Dim oRunRenewalAcceptRequest As RunRenewalAcceptRequestType 'Request Type
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oRunRenewalAcceptRequest = New RunRenewalAcceptRequestType
                sbLogMessage = New StringBuilder


                With oRunRenewalAcceptRequest
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
                        Throw New ArgumentNullException("v_iInsuranceFileKey")
                    End If

                    If v_iBatchRenewalJobKey > 0 Then
                        .BatchRenewalJobKey = v_iBatchRenewalJobKey
                    Else
                        Throw New ArgumentNullException("v_iBatchRenewalJobKey")
                    End If

                    If v_iRecordsCount > 0 Then
                        .RecordsCount = v_iRecordsCount
                    Else
                        Throw New ArgumentNullException("v_iRecordsCount")
                    End If

                    If String.IsNullOrEmpty(v_sGUID) Then
                        Throw New ArgumentNullException("v_sGUID")
                    Else
                        .GUID = v_sGUID
                    End If

                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oSAM.RunRenewalAccept(oRunRenewalAcceptRequest)
                End Using


                ' Disposing the SAM's object

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("RunRenewalAccept executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iInsuranceFileKey = " & v_iInsuranceFileKey.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_iBatchRenewalJobKey = " & v_iBatchRenewalJobKey.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_iRecordsCount = " & v_iRecordsCount.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_sGUID = " & v_sGUID.ToString & vbCrLf)


                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)


            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oRunRenewalAcceptRequest = Nothing
            End Try


        End SyncLock

    End Sub

    Public Overrides Sub RunRenewalSelection(ByVal v_iInsuranceFileKey As Integer,
                                            ByVal v_iBatchRenewalJobKey As Integer,
                                            ByVal v_iRecordsCount As Integer,
                                            ByVal v_sGUID As String,
                                            Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock

            Dim oSAM As PureServiceClient 'SAMForInsuranceV2's Object
            Dim oRunRenewalSelectionRequest As RunRenewalSelectionRequestType 'Request Type
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oRunRenewalSelectionRequest = New RunRenewalSelectionRequestType
                sbLogMessage = New StringBuilder


                With oRunRenewalSelectionRequest
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
                        Throw New ArgumentNullException("v_iInsuranceFileKey")
                    End If

                    If v_iBatchRenewalJobKey > 0 Then
                        .BatchRenewalJobKey = v_iBatchRenewalJobKey
                    Else
                        Throw New ArgumentNullException("v_iBatchRenewalJobKey")
                    End If

                    If v_iRecordsCount > 0 Then
                        .RecordsCount = v_iRecordsCount
                    Else
                        Throw New ArgumentNullException("v_iRecordsCount")
                    End If

                    If String.IsNullOrEmpty(v_sGUID) Then
                        Throw New ArgumentNullException("v_sGUID")
                    Else
                        .GUID = v_sGUID
                    End If

                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oSAM.RunRenewalSelection(oRunRenewalSelectionRequest)
                End Using


                ' Disposing the SAM's object
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("RunRenewalAccept executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iInsuranceFileKey = " & v_iInsuranceFileKey.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_iBatchRenewalJobKey = " & v_iBatchRenewalJobKey.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_iRecordsCount = " & v_iRecordsCount.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_sGUID = " & v_sGUID.ToString & vbCrLf)


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
                oRunRenewalSelectionRequest = Nothing
            End Try


        End SyncLock

    End Sub

    Public Overrides Sub RunRenewalInvite(ByVal v_iInsuranceFileKey As Integer,
                                        ByVal v_iBatchRenewalJobKey As Integer,
                                        ByVal v_iRecordsCount As Integer,
                                        ByVal v_sGUID As String,
                                        Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock

            Dim oSAM As PureServiceClient 'SAMForInsuranceV2's Object
            Dim oRunRenewalInviteRequest As RunRenewalInviteRequestType 'Request Type
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oRunRenewalInviteRequest = New RunRenewalInviteRequestType
                sbLogMessage = New StringBuilder


                With oRunRenewalInviteRequest
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
                        Throw New ArgumentNullException("v_iInsuranceFileKey")
                    End If

                    If v_iBatchRenewalJobKey > 0 Then
                        .BatchRenewalJobKey = v_iBatchRenewalJobKey
                    Else
                        Throw New ArgumentNullException("v_iBatchRenewalJobKey")
                    End If

                    If v_iRecordsCount > 0 Then
                        .RecordsCount = v_iRecordsCount
                    Else
                        Throw New ArgumentNullException("v_iRecordsCount")
                    End If

                    If String.IsNullOrEmpty(v_sGUID) Then
                        Throw New ArgumentNullException("v_sGUID")
                    Else
                        .GUID = v_sGUID
                    End If

                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oSAM.RunRenewalInvitation(oRunRenewalInviteRequest)
                End Using


                ' Disposing the SAM's object

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("RunRenewalInvite executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iInsuranceFileKey = " & v_iInsuranceFileKey.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_iBatchRenewalJobKey = " & v_iBatchRenewalJobKey.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_iRecordsCount = " & v_iRecordsCount.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_sGUID = " & v_sGUID.ToString & vbCrLf)


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
                oRunRenewalInviteRequest = Nothing
            End Try


        End SyncLock


    End Sub

    Public Overrides Sub RunRenewalSelectionByPolicy(ByRef r_oQuote As Quote,
                                                    Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock

            Dim oSAM As PureServiceClient 'SAMForInsuranceV2's Object
            Dim oRunRenewalSelectionByPolicyRequest As RunRenewalSelectionByPolicyRequestType 'Request Type
            Dim oRunRenewalSelectionByPolicyResponse As RunRenewalSelectionByPolicyResponseType 'Response Type
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oRunRenewalSelectionByPolicyRequest = New RunRenewalSelectionByPolicyRequestType
                oRunRenewalSelectionByPolicyResponse = New RunRenewalSelectionByPolicyResponseType
                sbLogMessage = New StringBuilder


                With oRunRenewalSelectionByPolicyRequest
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

                    If r_oQuote.InsuranceFileKey > 0 Then
                        .InsuranceFileKey = r_oQuote.InsuranceFileKey
                    Else
                        Throw New ArgumentNullException("Quote.InsuranceFileKey")
                    End If

                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oRunRenewalSelectionByPolicyResponse = oSAM.RunRenewalSelectionByPolicy(oRunRenewalSelectionByPolicyRequest)
                End Using

                ' Disposing the SAM's object


                With oRunRenewalSelectionByPolicyResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        r_oQuote.InsuranceFileKey = .RenewalInsuranceFileKey
                        r_oQuote.TimeStamp = .QuoteTimeStamp
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("RunRenewalSelectionByPolicy executed ok" & vbCrLf)
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
                oRunRenewalSelectionByPolicyRequest = Nothing
                oRunRenewalSelectionByPolicyResponse = Nothing
            End Try


        End SyncLock

    End Sub

    Public Overrides Function RunValidationRules(ByVal v_sScreenCode As String,
                                            ByRef v_sXMLDataSet As String,
                                            Optional ByVal v_iClaimKey As Integer = 0,
                                            Optional ByVal v_iClaimPerilKey As Integer = 0,
                                            Optional ByVal v_sBranchCode As String = Nothing,
                                            Optional ByVal v_sTransactionType As String = Nothing,
                                            Optional ByVal v_bSkipDaveToDB As Boolean = True) As String

        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oRunValidationRulesRequest As RunValidationRulesRequestType
            Dim oRunValidationRulesResponse As RunValidationRulesResponseType
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oRunValidationRulesRequest = New RunValidationRulesRequestType
                oRunValidationRulesResponse = New RunValidationRulesResponseType
                sbLogMessage = New StringBuilder


                With oRunValidationRulesRequest
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

                    'if the passed parameter v_iClaimKey is empty
                    If v_iClaimKey > 0 Then
                        .ClaimKey = v_iClaimKey
                        .ClaimKeySpecified = True
                    Else
                        .ClaimKeySpecified = False
                    End If

                    'if the passed parameter v_iClaimPerilKey is empty
                    If v_iClaimPerilKey > 0 Then
                        .ClaimPerilKey = v_iClaimPerilKey
                        .ClaimPerilKeySpecified = True
                    Else
                        .ClaimPerilKeySpecified = False
                    End If

                    'if the passed parameter v_sScreenCode is empty
                    If String.IsNullOrEmpty(v_sScreenCode) Then
                        Throw New ArgumentNullException("v_sScreenCode")
                    Else
                        .ScreenCode = v_sScreenCode
                    End If

                    'if the passed parameter v_sXMLDataSet is empty
                    If String.IsNullOrEmpty(v_sXMLDataSet) Then
                        Throw New ArgumentNullException("v_sXMLDataSet")
                    Else
                        .XMLDataSet = v_sXMLDataSet
                    End If
                    .TransactionType = v_sTransactionType

                    'as per nexus requirement we  need not to save the data to v_bSkipDaveToDB must be true
                    .SkipSaveToDB = v_bSkipDaveToDB

                End With


                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oRunValidationRulesResponse = oSAM.RunValidationRules(oRunValidationRulesRequest)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                With oRunValidationRulesResponse

                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        v_sXMLDataSet = .XMLDataset
                        RunValidationRules = .XMLDataset
                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("RunValidationRules executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iClaimKey =" & v_iClaimKey.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_iClaimPerilKey =" & v_iClaimPerilKey.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_sScreenCode = " & v_sScreenCode.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_sXMLDataset = " & v_sXMLDataSet.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_bSkipDaveToDB = " & v_bSkipDaveToDB.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & RunValidationRules.ToString & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oRunValidationRulesRequest = Nothing
                oRunValidationRulesResponse = Nothing
            End Try


        End SyncLock

    End Function

    Public Overrides Sub UpdateCoInsuranceValues(ByVal v_iInsuranceFileKey As Integer,
                                                ByVal v_bIsRecovered As Boolean,
                                                ByVal v_bIsSurcharged As Boolean,
                                                ByRef v_bTimeStampField() As Byte,
                                                ByVal v_oCoInsurers As CoInsurersCollections,
                                                Optional ByVal v_iDefaultId As Integer = Nothing,
                                                Optional ByVal v_sBranchCode As String = Nothing)


        SyncLock oLock

            Dim oSAM As PureServiceClient
            Dim oUpdateCoInsuranceValuesRequest As UpdateCoinsuranceValuesRequestType
            Dim oUpdateCoInsuranceValuesResponse As UpdateCoinsuranceValuesResponseType
            Dim oNewCoInsurers As BaseUpdateCoinsuranceValuesRequestTypeRow
            Dim i As Integer
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oUpdateCoInsuranceValuesRequest = New UpdateCoinsuranceValuesRequestType
                oUpdateCoInsuranceValuesResponse = New UpdateCoinsuranceValuesResponseType
                sbLogMessage = New StringBuilder


                With oUpdateCoInsuranceValuesRequest
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
                    .DefaultId = v_iDefaultId
                    .InsuranceFileKey = v_iInsuranceFileKey
                    .IsRecovered = v_bIsRecovered
                    .IsSurcharged = v_bIsSurcharged
                    .TimeStamp = v_bTimeStampField



                    .CoInsurers = New List(Of BaseUpdateCoinsuranceValuesRequestTypeRow)
                    For i = 0 To (v_oCoInsurers.Count - 1)

                        oNewCoInsurers = New BaseUpdateCoinsuranceValuesRequestTypeRow
                        oNewCoInsurers.CoInsurerKey = v_oCoInsurers(i).CoInsurerKey
                        oNewCoInsurers.ArrangementRef = v_oCoInsurers(i).ArrangementRef
                        oNewCoInsurers.SharePerc = v_oCoInsurers(i).SharePerc
                        oNewCoInsurers.CommissionPerc = v_oCoInsurers(i).CommissionPerc

                        .CoInsurers.Add(oNewCoInsurers)
                    Next


                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oUpdateCoInsuranceValuesResponse = oSAM.UpdateCoinsuranceValues(oUpdateCoInsuranceValuesRequest)
                End Using

                ' Disposing the SAM's object


                With oUpdateCoInsuranceValuesResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        v_bTimeStampField = .TimeStamp
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("UpdateCoInsuranceValues executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iInsuranceFileKey = " & v_iInsuranceFileKey.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_bIsRecovered = " & v_bIsRecovered.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_bIsSurcharged = " & v_bIsSurcharged.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_iDefaultId = " & v_iDefaultId.ToString & vbCrLf)
                    sbLogMessage.AppendLine("v_oCoInsurers = " & v_oCoInsurers.Count & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oUpdateCoInsuranceValuesRequest = Nothing
                oUpdateCoInsuranceValuesResponse = Nothing
            End Try


        End SyncLock
    End Sub

    Public Overrides Function GetRiskReinsuranceArrangementLines(ByVal v_iArrangementId As Integer,
                                                                        Optional ByVal v_sBranchCode As String = Nothing) As ArrangementLinesTypeCollection
        SyncLock oLock
            Dim oSAM As PureServiceClient
            Dim oGetRiskReinsuranceArrangementLinesRequest As GetRiskReinsuranceArrangementLinesRequestType
            Dim oGetRiskReinsuranceArrangementLinesResponse As GetRiskReinsuranceArrangementLinesResponseType
            Dim oArrangementLines As ArrangementLinesType
            Dim oArrangementLinesCollection As ArrangementLinesTypeCollection
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetRiskReinsuranceArrangementLinesRequest = New GetRiskReinsuranceArrangementLinesRequestType
                oGetRiskReinsuranceArrangementLinesResponse = New GetRiskReinsuranceArrangementLinesResponseType
                oArrangementLinesCollection = New ArrangementLinesTypeCollection
                sbLogMessage = New StringBuilder


                With oGetRiskReinsuranceArrangementLinesRequest
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
                        .ArrangementId = v_iArrangementId
                    End If

                    If v_iArrangementId > 0 Then
                        .ArrangementId = v_iArrangementId
                    Else
                        Throw New ArgumentNullException("GetRiskReinsuranceArrangementLines.ArrangementId")
                    End If
                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oGetRiskReinsuranceArrangementLinesResponse = oSAM.GetRiskReinsuranceArrangementLines(oGetRiskReinsuranceArrangementLinesRequest)
                End Using

                ' Disposing the SAM's object

                With oGetRiskReinsuranceArrangementLinesResponse

                    If .ArrangementLines IsNot Nothing AndAlso .ArrangementLines.Count > 0 Then
                        For Each oArrangementLinesTypeRow As BaseGetRiskReinsuranceArrangementLinesResponseTypeRow In .ArrangementLines

                            oArrangementLines = New ArrangementLinesType

                            With oArrangementLines
                                .Name = oArrangementLinesTypeRow.Name
                                .DefaultPerc = oArrangementLinesTypeRow.DefaultPerc
                                .ThisPerc = oArrangementLinesTypeRow.ThisPerc
                                .SumInsured = oArrangementLinesTypeRow.SumInsured
                                .PremiumValue = oArrangementLinesTypeRow.Premium
                                .Tax = oArrangementLinesTypeRow.Tax
                                .CommissionPerc = oArrangementLinesTypeRow.CommissionPerc
                                .CommissionValue = oArrangementLinesTypeRow.Commission
                                .CommissionTax = oArrangementLinesTypeRow.CommissionTax
                                .AgreementCode = oArrangementLinesTypeRow.Agreement

                            End With

                            oArrangementLinesCollection.Add(oArrangementLines)
                            oArrangementLines = Nothing

                        Next
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetRiskReinsuranceArrangementLines" & vbCrLf)

                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    If IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("Branch Code : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Branch Code : " & v_sBranchCode & vbCrLf)
                    End If

                    If IsNothing(v_iArrangementId) Then
                        sbLogMessage.AppendLine("ArrangementId : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("ArrangementId : " & v_iArrangementId & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output : " & vbCrLf)
                    sbLogMessage.AppendLine(oArrangementLinesCollection.Print().Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetRiskReinsuranceArrangementLinesRequest = Nothing
                oGetRiskReinsuranceArrangementLinesResponse = Nothing
            End Try


            Return oArrangementLinesCollection
        End SyncLock
    End Function

    Public Overrides Function GetRiskReinsuranceArrangements(ByVal v_iRiskKey As Integer,
                                                                    Optional ByVal v_sBranchCode As String = Nothing) As ArrangementsTypeCollection
        SyncLock oLock
            Dim oSAM As PureServiceClient
            Dim oGetRiskReinsuranceArrangementsRequest As GetRiskReinsuranceArrangementsRequestType
            Dim oGetRiskReinsuranceArrangementsResponse As GetRiskReinsuranceArrangementsResponseType
            Dim oArrangements As ArrangementsType
            Dim oArrangementsCollection As ArrangementsTypeCollection
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetRiskReinsuranceArrangementsRequest = New GetRiskReinsuranceArrangementsRequestType
                oGetRiskReinsuranceArrangementsResponse = New GetRiskReinsuranceArrangementsResponseType
                oArrangementsCollection = New ArrangementsTypeCollection
                sbLogMessage = New StringBuilder


                With oGetRiskReinsuranceArrangementsRequest
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
                        .RiskKey = v_iRiskKey
                    End If

                    If v_iRiskKey > 0 Then
                        .RiskKey = v_iRiskKey
                    Else
                        Throw New ArgumentNullException("GetRiskReinsuranceArrangements.RiskKey")
                    End If
                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oGetRiskReinsuranceArrangementsResponse = oSAM.GetRiskReinsuranceArrangements(oGetRiskReinsuranceArrangementsRequest)
                End Using

                ' Disposing the SAM's object
                With oGetRiskReinsuranceArrangementsResponse
                    If .Arrangements IsNot Nothing AndAlso .Arrangements.Count > 0 Then
                        For Each oArrangementsTypeRow As BaseGetRiskReinsuranceArrangementsResponseTypeRow In .Arrangements

                            oArrangements = New ArrangementsType

                            With oArrangements
                                .ArrangementId = oArrangementsTypeRow.ArrangementId
                                .BandId = oArrangementsTypeRow.BandId
                                .ModelId = oArrangementsTypeRow.ModelId
                                .SumInsured = oArrangementsTypeRow.SumInsured
                                .Premium = oArrangementsTypeRow.Premium
                                .IsOriginal = oArrangementsTypeRow.IsOriginal
                                .IsModified = oArrangementsTypeRow.IsModified
                                .FACPremiumType = oArrangementsTypeRow.FACPremiumType
                                .RIModelCode = oArrangementsTypeRow.RIModelCode
                                .IsExtendedLimitapplied = oArrangementsTypeRow.IsExtendedLimitApplied
                                .ExtendedLimitAmount = oArrangementsTypeRow.ExtendedLimitAmount
                            End With
                            oArrangementsCollection.Add(oArrangements)
                            oArrangements = Nothing
                        Next
                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetRiskReinsuranceArrangements" & vbCrLf)
                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    If IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("Branch Code : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Branch Code : " & v_sBranchCode & vbCrLf)
                    End If

                    If IsNothing(v_iRiskKey) Then
                        sbLogMessage.AppendLine("RiskKey : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("RiskKey : " & v_iRiskKey & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output: " & vbCrLf)
                    'sbLogMessage.AppendLine(oArrangementsCollection.Print().Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetRiskReinsuranceArrangementsRequest = Nothing
                oGetRiskReinsuranceArrangementsResponse = Nothing
            End Try


            Return oArrangementsCollection
        End SyncLock
    End Function

    Public Overrides Function GetRiskReinsuranceBands(ByVal v_iRiskKey As Integer,
                                                            Optional ByVal v_sBranchCode As String = Nothing) As ReinsuranceBandsCollection
        SyncLock oLock
            Dim oSAM As PureServiceClient
            Dim oGetRiskReinsuranceBandsRequest As GetRiskReinsuranceBandsRequestType
            Dim oGetRiskReinsuranceBandsResponse As GetRiskReinsuranceBandsResponseType
            Dim oReinsuranceBandsCollection As ReinsuranceBandsCollection
            Dim oReinsuranceBands As ReinsuranceBands
            Dim sbLogMessage As StringBuilder

            Try
                oSAM = InitializeServiceMethod()
                oGetRiskReinsuranceBandsRequest = New GetRiskReinsuranceBandsRequestType
                oGetRiskReinsuranceBandsResponse = New GetRiskReinsuranceBandsResponseType
                oReinsuranceBandsCollection = New ReinsuranceBandsCollection
                sbLogMessage = New StringBuilder


                With oGetRiskReinsuranceBandsRequest
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
                        .RiskKey = v_iRiskKey
                    End If

                    If v_iRiskKey > 0 Then
                        .RiskKey = v_iRiskKey
                    Else
                        Throw New ArgumentNullException("GetRiskReinsuranceArrangements.RiskKey")
                    End If
                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    oGetRiskReinsuranceBandsResponse = oSAM.GetRiskReinsuranceBands(oGetRiskReinsuranceBandsRequest)
                End Using


                ' Disposing the SAM's object



                With oGetRiskReinsuranceBandsResponse


                    If .ReinsuranceBands IsNot Nothing AndAlso .ReinsuranceBands.Count > 0 Then
                        For Each oReinsuranceTypeRow As BaseGetRiskReinsuranceBandsResponseTypeRow In .ReinsuranceBands
                            oReinsuranceBands = New ReinsuranceBands
                            oReinsuranceBands.Band = oReinsuranceTypeRow.Band
                            oReinsuranceBands.BandKey = oReinsuranceTypeRow.BandKey

                            oReinsuranceBandsCollection.Add(oReinsuranceBands)
                        Next
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetRiskReinsuranceBands" & vbCrLf)
                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    If IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("Branch Code : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Branch Code : " & v_sBranchCode & vbCrLf)
                    End If

                    If IsNothing(v_iRiskKey) Then
                        sbLogMessage.AppendLine("RiskKey : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("RiskKey : " & v_iRiskKey & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output : " & vbCrLf)
                    sbLogMessage.AppendLine(oReinsuranceBandsCollection.Print().Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oSAM.Close()
                oGetRiskReinsuranceBandsRequest = Nothing
                oGetRiskReinsuranceBandsResponse = Nothing
            End Try


            Return oReinsuranceBandsCollection
        End SyncLock
    End Function

End Class
