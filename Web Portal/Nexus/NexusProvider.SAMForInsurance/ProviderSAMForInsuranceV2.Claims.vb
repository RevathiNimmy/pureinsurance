Imports System.ServiceModel
Imports System.Configuration.Provider
Imports System.Web
Imports System.Web.HttpContext
Imports System.Web.Configuration.WebConfigurationManager
Imports SiriusFS.SAM.Client.Security
Imports System.Xml.Serialization
Imports Microsoft.Practices.EnterpriseLibrary.Logging
Imports System.Diagnostics
Imports System.Text
Imports System.Xml
Imports NexusProvider
Imports Nexus.Constants
Imports Nexus.Constants.Session
Imports System.Xml.XmlReader
Imports System.IO
Imports Nexus.Utils
Imports Nexus.Constants.Constant
Imports System.Resources
Imports System.Linq
Imports SSP.PureInsuranceRestAPIHandler
Imports SSP.PureInsuranceRestAPIHandler.BaseClasses






Partial Public Class ProviderSAMForInsuranceV2
#Region "AddCashClaimLink"
    ''' <summary>
    ''' To AddCashClaimLink.
    ''' </summary>
    ''' <param name="v_iClaimPaymentKey"></param>
    ''' <param name="v_iCashListItemKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub AddCashClaimLink(ByVal v_iClaimPaymentKey As Integer,
                                               ByVal v_iCashListItemKey As Integer,
                                                        Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock

            ' Dim oSAM As PureClaimServiceClient 'SAMForInsuranceV2's Object
            Dim oAddCashClaimLinkRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.AddCashClaimLinkCommand = Nothing
            Dim oAddCashClaimLinkResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.AddCashClaimLinkCommandResponse = Nothing 'Response Type
            Dim sbLogMessage As StringBuilder = Nothing
            Try
                'oSAM = InitializeClaimServiceMethod()
                oAddCashClaimLinkRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.AddCashClaimLinkCommand
                oAddCashClaimLinkResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.AddCashClaimLinkCommandResponse
                With oAddCashClaimLinkRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    ' .WCFSecurityToken = SecurityToken()

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
                    .ClaimPaymentKey = v_iClaimPaymentKey
                    .CashListItemKey = v_iCashListItemKey
                End With
                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    ' oAddCashClaimLinkResponse = oSAM.AddCashClaimLink(oAddCashClaimLinkRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.AddCashClaimLink, oAddCashClaimLinkRequest)
                    oAddCashClaimLinkResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.AddCashClaimLinkCommandResponse)(result)

                End Using

                With oAddCashClaimLinkResponse

                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage = New StringBuilder
                    sbLogMessage.AppendLine("AddCashClaimLink executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)

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
                'oSAM.Close()
                oAddCashClaimLinkRequest = Nothing
                oAddCashClaimLinkResponse = Nothing
                sbLogMessage = Nothing
            End Try
        End SyncLock
    End Sub
#End Region


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_iBaseClaimKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function AddClaimRisk(ByVal v_iBaseClaimKey As Integer,
                                           ByVal v_bTimeStamp As Byte(),
                                                        Optional ByVal v_sBranchCode As String = Nothing) As ClaimRisk
        SyncLock oLock
            Dim oAddClaimRiskRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.AddClaimRiskCommand
            Dim oAddClaimRiskResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.AddClaimRiskCommandResponse
            Dim oClaimRisk As ClaimRisk
            Dim sbLogMessage As StringBuilder

            Try
                oAddClaimRiskRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.AddClaimRiskCommand
                oAddClaimRiskResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.AddClaimRiskCommandResponse
                sbLogMessage = New StringBuilder


                With oAddClaimRiskRequest
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
                    If v_iBaseClaimKey > 0 Then
                        .BaseClaimKey = v_iBaseClaimKey
                    Else
                        Throw New ArgumentException("ClaimKey")
                    End If

                    .ApiTimseStamp = v_bTimeStamp

                End With



                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Post(ApiMethods.AddClaimRisk, oAddClaimRiskRequest)
                    oAddClaimRiskResponse = ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.AddClaimRiskCommandResponse)(result)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                ' Disposing the SAM's object
                With oAddClaimRiskResponse
                    If .AddClaimRiskResponse.Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.AddClaimRiskResponse.Errors)
                    Else
                        oClaimRisk = New ClaimRisk
                        oClaimRisk.TimeStamp = .AddClaimRiskResponse.ApiTimeStamp
                        oClaimRisk.XMLDataSet = .AddClaimRiskResponse.XMLDataSet
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetClaimRisk executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iBaseClaimKey = " & v_iBaseClaimKey.ToString() & vbCrLf)

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
                oAddClaimRiskRequest = Nothing
                oAddClaimRiskResponse = Nothing
            End Try


            Return oClaimRisk
        End SyncLock
    End Function



    ''' <summary>
    '''  Method Used During Claim Authorization and Decline
    ''' </summary>
    ''' <param name="v_iClaimPaymentKey"></param>
    ''' <param name="v_sComments"></param>
    ''' <param name="v_bDeclined"></param>
    ''' <param name="v_bTimeStamp"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="Request"></param>
    ''' <param name="v_iAccountKey"></param>
    ''' <param name="v_dtPaymentDate"></param>
    ''' <param name="v_dtPaymentDateTo"></param>
    ''' <param name="v_sSourceIds"></param>
    ''' <param name="r_bIsUpdated"></param>
    ''' <remarks></remarks>
    Public Overrides Sub AuthoriseClaimPayment(ByVal v_nClaimPaymentKey As Integer,
                                                ByVal v_sComments As String,
                                                ByVal v_bDeclined As Boolean,
                                                ByRef v_bTimeStamp() As Byte,
                                                Optional ByVal v_sBranchCode As String = Nothing,
                                                Optional ByVal v_PaymentCashList As NexusProvider.PaymentCashListItemType = Nothing,
                                                Optional ByVal v_nAccountKey As Integer = 0,
                                                Optional ByVal v_dtPaymentDate As DateTime = Nothing,
                                                Optional ByVal v_dtPaymentDateTo As DateTime = Nothing,
                                                Optional ByVal v_sSourceIds As String = "",
                                                Optional ByRef r_bIsUpdated As Boolean = False,
                                                Optional ByRef r_sFailureReason As String = "",
                                                Optional ByVal bExclusiveLock As Boolean = False)
        SyncLock oLock

            ' Dim oSAM As PureClaimServiceClient
            Dim oAuthoriseClaimPaymentRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.AuthoriseClaimPaymentCommand
            Dim oAuthoriseClaimPaymentResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.AuthoriseClaimPaymentCommandResponse
            Try
                'oSAM = InitializeClaimServiceMethod()
                oAuthoriseClaimPaymentRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.AuthoriseClaimPaymentCommand
                oAuthoriseClaimPaymentResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.AuthoriseClaimPaymentCommandResponse


                With oAuthoriseClaimPaymentRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    '.WCFSecurityToken = SecurityToken()
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
                    If v_nClaimPaymentKey > 0 Then
                        .ClaimPaymentKey = v_nClaimPaymentKey
                    Else
                        Throw New ArgumentException("ClaimPaymentKey")
                    End If

                    .Comments = v_sComments
                    .Declined = v_bDeclined
                    .LoginUserName = Current.Session(CNLoginName)
                    .ApiTimeStamp = Current.Session(CNClaimCallsTimeStamp)
                    'Set SessionID in every call
                    .SessionValue = HttpContext.Current.Session.SessionID.ToString()

                    'Set ExclusinLock Value
                    .ExclusiveLock = bExclusiveLock
                    If v_PaymentCashList IsNot Nothing Then
                        .PaymentCashList = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BasePaymentCashListType()
                        'CreatePaymentWithCashListItems Request
                        .PaymentCashList.BankAccountCode = v_PaymentCashList.CoreCashList.BankAccountCode
                        .PaymentCashList.BankAccountName = v_PaymentCashList.CoreCashList.BankAccountName
                        .PaymentCashList.CurrencyCode = v_PaymentCashList.CoreCashList.CurrencyCode
                        .PaymentCashList.ListDate = v_PaymentCashList.CoreCashList.ListDate
                        .PaymentCashList.Reference = v_PaymentCashList.CoreCashList.Reference
                        .PaymentCashList.StatusCode = v_PaymentCashList.CoreCashList.StatusCode
                        .PaymentCashList.TypeCode = v_PaymentCashList.CoreCashList.TypeCode
                        .PaymentCashList.CashListKey = v_PaymentCashList.CoreCashList.CashListKey
                        .PaymentCashList.BankAccountKey = v_PaymentCashList.CoreCashList.BankAccountKey

                        Dim oPayment As SSP.PureInsuranceRestAPIHandler.BaseClasses.BasePaymentCashListItemType
                        For index As Integer = 0 To v_PaymentCashList.PaymentItems.Count - 1
                            oPayment = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BasePaymentCashListItemType
                            oPayment.StatusCode = v_PaymentCashList.PaymentItems(index).StatusCode
                            oPayment.TypeCode = v_PaymentCashList.PaymentItems(index).TypeCode
                            oPayment.TransactionDate = v_PaymentCashList.PaymentItems(index).TransactionDate
                            oPayment.MediaTypeCode = v_PaymentCashList.PaymentItems(index).MediaTypeCode
                            oPayment.OurReference = v_PaymentCashList.PaymentItems(index).OurReference
                            oPayment.TheirReference = v_PaymentCashList.PaymentItems(index).TheirReference
                            oPayment.AccountShortCode = v_PaymentCashList.PaymentItems(index).AccountShortCode
                            oPayment.IsProduceDocument = v_PaymentCashList.PaymentItems(index).IsProduceDocument
                            oPayment.BankReference = v_PaymentCashList.PaymentItems(index).BankReference
                            oPayment.ContactName = v_PaymentCashList.PaymentItems(index).ContactName
                            oPayment.FurtherDetails = v_PaymentCashList.PaymentItems(index).FurtherDetails
                            oPayment.SkipPosting = v_PaymentCashList.PaymentItems(index).SkipPosting
                            oPayment.TaxBandCode = v_PaymentCashList.PaymentItems(index).TaxBandCode
                            If v_PaymentCashList.PaymentItems(index).TaxAmount <> Decimal.MinValue Then
                                oPayment.TaxAmount = v_PaymentCashList.PaymentItems(index).TaxAmount
                                oPayment.TaxAmountSpecified = True
                            Else
                                oPayment.TaxAmountSpecified = False
                            End If
                            oPayment.AllocationStatusCode = v_PaymentCashList.PaymentItems(index).AllocationStatusCode
                            oPayment.Amount = v_PaymentCashList.PaymentItems(index).Amount
                            oPayment.MediaReference = v_PaymentCashList.MediaReference
                            If v_PaymentCashList.PaymentItems(index).Bank.AccountCode.Trim.Length <> 0 Then
                                oPayment.Bank = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseBankPaymentType
                                oPayment.Bank.AccountCode = v_PaymentCashList.PaymentItems(index).Bank.AccountCode
                                oPayment.Bank.BranchCode = v_PaymentCashList.PaymentItems(index).Bank.BranchCode
                                oPayment.Bank.ExpiryDate = v_PaymentCashList.PaymentItems(index).Bank.ExpiryDate
                                If oPayment.Bank.ExpiryDate <> DateTime.MinValue Then
                                    oPayment.Bank.ExpiryDateSpecified = True
                                Else
                                    oPayment.Bank.ExpiryDateSpecified = False
                                End If
                                oPayment.Bank.PayeeName = v_PaymentCashList.PaymentItems(index).Bank.PayeeName
                                oPayment.Bank.Reference1 = v_PaymentCashList.PaymentItems(index).Bank.Reference1
                                oPayment.Bank.Reference2 = v_PaymentCashList.PaymentItems(index).Bank.Reference2
                                oPayment.Bank.PartyBankKey = v_PaymentCashList.PaymentItems(index).Bank.PartyBankKey
                                oPayment.Bank.BIC = v_PaymentCashList.PaymentItems(index).Bank.BIC
                                oPayment.Bank.IBAN = v_PaymentCashList.PaymentItems(index).Bank.IBAN
                            End If
                            If Trim(v_PaymentCashList.ContactAddress.Address1) <> String.Empty Then
                                oPayment.ContactAddress = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseSimpleAddressType
                                oPayment.ContactAddress.AddressLine1 = v_PaymentCashList.ContactAddress.Address1
                                oPayment.ContactAddress.AddressLine2 = v_PaymentCashList.ContactAddress.Address2
                                oPayment.ContactAddress.AddressLine3 = v_PaymentCashList.ContactAddress.Address3
                                oPayment.ContactAddress.AddressLine4 = v_PaymentCashList.ContactAddress.Address4
                                oPayment.ContactAddress.CountryCode = v_PaymentCashList.ContactAddress.CountryCode
                                oPayment.ContactAddress.PostCode = v_PaymentCashList.ContactAddress.PostCode
                            End If
                            If Trim(v_PaymentCashList.CreditCard.Address1) <> String.Empty Then
                                oPayment.CreditCard = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseCreditCardType
                                oPayment.CreditCard.Number = v_PaymentCashList.CreditCard.Name
                                oPayment.CreditCard.NameOnCreditCard = v_PaymentCashList.CreditCard.NameOnCreditCard
                                oPayment.CreditCard.ExpiryDate = v_PaymentCashList.CreditCard.ExpiryDate
                                oPayment.CreditCard.StartDate = v_PaymentCashList.CreditCard.StartDate
                                oPayment.CreditCard.Issue = v_PaymentCashList.CreditCard.Issue
                            End If

                            If v_PaymentCashList.PaymentItems(index).CashListItemKey > 0 Then
                                oPayment.CashListItemKey = v_PaymentCashList.PaymentItems(index).CashListItemKey
                                oPayment.CashListItemKeySpecified = True
                            End If

                        Next
                        .PaymentCashList.PaymentItem = New List(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BasePaymentCashListItemType)
                        .PaymentCashList.PaymentItem.Add(oPayment)

                        'GetUnallocatedClaimPaymentRequest
                        If v_nAccountKey > 0 Then
                            .AccountKey = v_nAccountKey
                            .AccountKeySpecified = True
                        Else
                            .AccountKeySpecified = False
                        End If


                        If v_dtPaymentDate <> DateTime.MinValue Then
                            .PaymentDateSpecified = True
                            .PaymentDate = v_dtPaymentDate
                        Else
                            .PaymentDateSpecified = False
                        End If

                        If v_dtPaymentDateTo <> DateTime.MinValue Then
                            .PaymentDateToSpecified = True
                            .PaymentDateTo = v_dtPaymentDateTo
                        Else
                            .PaymentDateToSpecified = False
                        End If

                        'GetAccountDetailsRequest
                        .SourceArray = v_sSourceIds
                        oPayment = Nothing
                    End If

                End With

                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    'oAuthoriseClaimPaymentResponse = oSAM.AuthoriseClaimPayment(oAuthoriseClaimPaymentRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.AuthoriseClaimPayment, oAuthoriseClaimPaymentRequest)
                    oAuthoriseClaimPaymentResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.AuthoriseClaimPaymentCommandResponse)(result)

                End Using

                With oAuthoriseClaimPaymentResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    ElseIf Not String.IsNullOrEmpty(.ErrorMessage) Then
                        r_sFailureReason = .ErrorMessage
                    End If

                    r_bIsUpdated = .AllocationStatus

                End With
                ' Disposing the SAM's object
                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                'oSAM.Close()
                oAuthoriseClaimPaymentRequest = Nothing
                oAuthoriseClaimPaymentResponse = Nothing
            End Try

        End SyncLock
    End Sub


    Public Overrides Function BindClaim(ByVal v_oClaimOpen As Claim,
                                            ByVal v_bTimeStamp As Byte(),
                                            ByVal v_iProcessType As Integer,
                                            ByVal v_oClaimPayment As ClaimPayment,
                                            Optional ByVal v_sBranchCode As String = Nothing,
                                            Optional ByVal bSkipSaveTransaction As Boolean = False, Optional ByVal ClaimPaymentCollection As NexusProvider.ClaimPaymentCollection = Nothing) As ClaimResponse
        SyncLock oLock

            Dim oBindClaimRequest As BaseClasses.BindClaimCommand
            Dim oBindClaimResponse As BaseClasses.BindClaimCommandResponse    ' Response Type
            Dim iPeril As Integer = 0
            Dim iRecovery As Integer = 0
            Dim iReserve As Integer = 0
            Dim i As Integer = 0
            Dim oPaymentItem As BaseClasses.BasePaymentCashListItemType
            Dim oClaimPaymentItemType As BaseClasses.BaseClaimPaymentItemType
            Dim oClaimResponse As ClaimResponse
            Try
                oBindClaimRequest = New BaseClasses.BindClaimCommand
                oBindClaimResponse = New BaseClasses.BindClaimCommandResponse
                oClaimResponse = New ClaimResponse
                Dim insuranceFileKey As Integer = 0
                With oBindClaimRequest
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
                    .ClaimKey = v_oClaimOpen.ClaimKey
                    .CloseClaimOnZeroReserveRecoveryBalance = v_oClaimOpen.CloseClaimOnZeroReserveRecoveryBalance
                    .CloseClaimOnFinalPayment = v_oClaimOpen.CloseClaimOnFinalPayment
                    .ApiTimeStamp = v_bTimeStamp
                    .BaseCaseKey = v_oClaimOpen.BaseCaseKey
                    .InsuranceFileCNT = v_oClaimOpen.InsuranceFileKey
                    '1 for Open Claim
                    '2 for Maintain Claim
                    '3 for Payment Claim
                    .ProcessType = v_iProcessType
                    insuranceFileKey = v_oClaimOpen.InsuranceFileKey
                    .CoinsuranceTreatmentCode = v_oClaimOpen.CoinsuranceTreatmentCode
                    'Payment Data
                    If v_oClaimPayment IsNot Nothing Then

                        .ClaimPayment = New BaseClasses.BaseClaimPaymentType()
                        .ClaimPayment.BaseClaimKey = v_oClaimPayment.BaseClaimKey
                        .ClaimPayment.BaseClaimPerilKey = v_oClaimPayment.BaseClaimPerilKey
                        .ClaimPayment.ClaimVersionDescription = v_oClaimPayment.ClaimVersionDescription
                        .ClaimPayment.CloseClaimOnZeroReserveRecoveryBalance = v_oClaimPayment.CloseClaimOnZeroReserveRecoveryBalance
                        .ClaimPayment.CurrencyCode = v_oClaimPayment.CurrencyCode
                        .ClaimPayment.PartyKey = v_oClaimPayment.PartyKey
                        .ClaimPayment.ClientKey = v_oClaimPayment.ClientKey
                        .ClaimPayment.PaymentPartyType = v_oClaimPayment.PaymentPartyType
                        .ClaimPayment.CloseClaimOnFinalPayment = v_oClaimPayment.CloseClaimOnFinalPayment
                        .ClaimPayment.OurRef = v_oClaimPayment.OurRef
                        .ClaimPayment.InsuranceFileCNT = insuranceFileKey
                        .ApiTimeStamp = v_bTimeStamp
                        Dim j As Integer = 0
                        Dim oClaimPaymentTaxItems(v_oClaimPayment.ClaimPaymentTaxItems.Count - 1) As BaseClasses.BaseClaimPaymentTaxItemType
                        For index As Integer = 0 To v_oClaimPayment.ClaimPaymentTaxItems.Count - 1
                            'If v_oClaimPayment.ClaimPaymentItem(index).PaymentAmount <> 0.0 Then
                            oClaimPaymentTaxItems(j) = New BaseClasses.BaseClaimPaymentTaxItemType()
                            oClaimPaymentTaxItems(j).Amount = v_oClaimPayment.ClaimPaymentTaxItems(index).Amount
                            oClaimPaymentTaxItems(j).Percentage = v_oClaimPayment.ClaimPaymentTaxItems(index).Percentage
                            oClaimPaymentTaxItems(j).ReserveType = v_oClaimPayment.ClaimPaymentTaxItems(index).ReserveType
                            oClaimPaymentTaxItems(j).TaxBandCode = v_oClaimPayment.ClaimPaymentTaxItems(index).TaxBandCode
                            oClaimPaymentTaxItems(j).TaxGroupCode = v_oClaimPayment.ClaimPaymentTaxItems(index).TaxGroupCode
                            oClaimPaymentTaxItems(j).ClassOfBusinessID = v_oClaimPayment.ClaimPaymentTaxItems(index).ClassOfBusinessID
                            oClaimPaymentTaxItems(j).IsManuallyChanges = v_oClaimPayment.ClaimPaymentTaxItems(index).IsManuallyChanges
                            oClaimPaymentTaxItems(j).Sequence = v_oClaimPayment.ClaimPaymentTaxItems(index).Sequence
                            oClaimPaymentTaxItems(j).TaxBandId = v_oClaimPayment.ClaimPaymentTaxItems(index).TaxBandId
                            oClaimPaymentTaxItems(j).TaxGroupId = v_oClaimPayment.ClaimPaymentTaxItems(index).TaxGroupId
                            j += 1
                            'End If
                        Next

                        .ClaimPayment.ClaimPaymentTaxItems = oClaimPaymentTaxItems.ToList

                        If v_oClaimPayment.PaymentAdvancedTaxDetails.InsuranceTaxNumber <> String.Empty Then
                            .ClaimPayment.AdvancedTaxDetails = New BaseClasses.BaseClaimPaymentAdvancedTaxDetailsType()
                            .ClaimPayment.AdvancedTaxDetails.InsuranceTaxNumber = v_oClaimPayment.PaymentAdvancedTaxDetails.InsuranceTaxNumber
                            .ClaimPayment.AdvancedTaxDetails.InsuredDomiciled = v_oClaimPayment.PaymentAdvancedTaxDetails.InsuredDomiciled
                            .ClaimPayment.AdvancedTaxDetails.InsuredPercentage = v_oClaimPayment.PaymentAdvancedTaxDetails.InsuredPercentage
                            .ClaimPayment.AdvancedTaxDetails.IsSettlement = v_oClaimPayment.PaymentAdvancedTaxDetails.IsSettlement
                            .ClaimPayment.AdvancedTaxDetails.IsTaxExempt = v_oClaimPayment.PaymentAdvancedTaxDetails.IsTaxExempt
                            .ClaimPayment.AdvancedTaxDetails.IsWHTExempt = v_oClaimPayment.PaymentAdvancedTaxDetails.IsWHTExempt
                            .ClaimPayment.AdvancedTaxDetails.PayeeDomiciled = v_oClaimPayment.PaymentAdvancedTaxDetails.PayeeDomiciled
                            .ClaimPayment.AdvancedTaxDetails.PayeePercentage = v_oClaimPayment.PaymentAdvancedTaxDetails.PayeePercentage
                            .ClaimPayment.AdvancedTaxDetails.PayeeTaxNumber = v_oClaimPayment.PaymentAdvancedTaxDetails.PayeeTaxNumber
                            .ClaimPayment.AdvancedTaxDetails.SafeHarbourCode = v_oClaimPayment.PaymentAdvancedTaxDetails.SafeHarbourCode
                            .ClaimPayment.AdvancedTaxDetails.SafeHarbourPercentage = v_oClaimPayment.PaymentAdvancedTaxDetails.SafeHarbourPercentage
                            .ClaimPayment.AdvancedTaxDetails.PaymentTo = v_oClaimPayment.PaymentAdvancedTaxDetails.PaymentTo
                            .ClaimPayment.AdvancedTaxDetails.PayeeName = v_oClaimPayment.PaymentAdvancedTaxDetails.PayeeName
                        End If

                        If v_oClaimPayment.CashList.BankAccountCode <> String.Empty Then
                            .ClaimPayment.CashList = New BaseClasses.BasePaymentCashListType()
                            .ClaimPayment.CashList.BankAccountCode = v_oClaimPayment.CashList.BankAccountCode
                            .ClaimPayment.CashList.CurrencyCode = v_oClaimPayment.CashList.CurrencyCode
                            .ClaimPayment.CashList.ListDate = v_oClaimPayment.CashList.ListDate
                            .ClaimPayment.CashList.Reference = v_oClaimPayment.CashList.Reference
                            .ClaimPayment.CashList.StatusCode = v_oClaimPayment.CashList.StatusCode
                            .ClaimPayment.CashList.TypeCode = v_oClaimPayment.CashList.TypeCode
                            .ClaimPayment.CashList.SubBranchCode = v_oClaimPayment.CashList.SubbranchCode
                            .ClaimPayment.CashList.BranchCode = sBranchCode
                            .ClaimPayment.CashList.PaymentItem = New List(Of BaseClasses.BasePaymentCashListItemType)

                            For index As Integer = 0 To v_oClaimPayment.CashList.PaymentCashListItemType.Count - 1
                                With v_oClaimPayment.CashList.PaymentCashListItemType(index)
                                    oPaymentItem = New BaseClasses.BasePaymentCashListItemType()
                                    oPaymentItem.AccountShortCode = .AccountShortCode
                                    oPaymentItem.AllocationStatusCode = .AllocationStatusCode
                                    oPaymentItem.Amount = .Amount

                                    If .BankPaymentType.AccountCode.Trim.Length <> 0 Then
                                        oPaymentItem.Bank = New BaseClasses.BaseBankPaymentType
                                        oPaymentItem.Bank.AccountCode = .BankPaymentType.AccountCode
                                        oPaymentItem.Bank.BranchCode = .BankPaymentType.BranchCode
                                        oPaymentItem.Bank.ExpiryDate = .BankPaymentType.ExpiryDate
                                        If .BankPaymentType.ExpiryDate <> DateTime.MinValue Then
                                            oPaymentItem.Bank.ExpiryDateSpecified = True
                                        Else
                                            oPaymentItem.Bank.ExpiryDateSpecified = False
                                        End If
                                        oPaymentItem.Bank.PayeeName = .BankPaymentType.PayeeName
                                        oPaymentItem.Bank.Reference1 = .BankPaymentType.Reference1
                                        oPaymentItem.Bank.Reference2 = .BankPaymentType.Reference2
                                        oPaymentItem.BankReference = .BankReference
                                        oPaymentItem.Bank.BIC = .BankPaymentType.BIC
                                        oPaymentItem.Bank.IBAN = .BankPaymentType.IBAN
                                    End If

                                    If Trim(.ContactAddress.Address1) <> String.Empty Then
                                        oPaymentItem.ContactAddress = New BaseClasses.BaseSimpleAddressType
                                        oPaymentItem.ContactAddress.AddressLine1 = .ContactAddress.Address1
                                        oPaymentItem.ContactAddress.AddressLine2 = .ContactAddress.Address2
                                        oPaymentItem.ContactAddress.AddressLine3 = .ContactAddress.Address3
                                        oPaymentItem.ContactAddress.AddressLine4 = .ContactAddress.Address4
                                        oPaymentItem.ContactAddress.CountryCode = .ContactAddress.CountryCode
                                        oPaymentItem.ContactAddress.PostCode = .ContactAddress.PostCode
                                    End If
                                    oPaymentItem.ContactName = .ContactName
                                    oPaymentItem.FurtherDetails = .FurtherDetails
                                    oPaymentItem.MediaReference = .MediaReference
                                    oPaymentItem.MediaTypeCode = .MediaTypeCode
                                    oPaymentItem.OurReference = .OurReference
                                    oPaymentItem.StatusCode = .StatusCode
                                    oPaymentItem.TheirReference = .TheirReference
                                    oPaymentItem.TypeCode = .TypeCode
                                    oPaymentItem.TransactionDate = .TransactionDate
                                    oPaymentItem.ChequeDate = .ChequeDate
                                End With
                                .ClaimPayment.CashList.PaymentItem.Add(oPaymentItem)
                            Next

                        End If

                        .ClaimPayment.ClaimPaymentItem = New List(Of BaseClasses.BaseClaimPaymentItemType)
                        For index As Integer = 0 To v_oClaimPayment.ClaimPaymentItem.Count - 1
                            'If v_oClaimPayment.ClaimPaymentItem(index).PaymentAmount <> 0.0 Then
                            oClaimPaymentItemType = New BaseClasses.BaseClaimPaymentItemType()
                            oClaimPaymentItemType.PaymentAmount = v_oClaimPayment.ClaimPaymentItem(index).PaymentAmount
                            oClaimPaymentItemType.BaseReserveKey = v_oClaimPayment.ClaimPaymentItem(index).BaseReserveKey
                            oClaimPaymentItemType.ReverseExcess = v_oClaimPayment.ClaimPaymentItem(index).ReverseExcess
                            oClaimPaymentItemType.TaxGroupCode = v_oClaimPayment.ClaimPaymentItem(index).TaxGroupCode
                            oClaimPaymentItemType.IsTaxOverridden = v_oClaimPayment.ClaimPaymentItem(index).IsTaxOverridden
                            oClaimPaymentItemType.OverriddedTaxAmount = v_oClaimPayment.ClaimPaymentItem(index).OverriddedTaxAmount
                            i += 1
                            'End If
                            .ClaimPayment.ClaimPaymentItem.Add(oClaimPaymentItemType)
                        Next

                        .ClaimPayment.Payee = New BaseClasses.BaseClaimPayeeType()
                        If Trim(v_oClaimPayment.Payee.Address.Address1) <> String.Empty Then
                            .ClaimPayment.Payee.Address = New BaseClasses.BaseAddressType()
                            .ClaimPayment.Payee.Address.AddressLine1 = v_oClaimPayment.Payee.Address.Address1
                            .ClaimPayment.Payee.Address.AddressLine2 = v_oClaimPayment.Payee.Address.Address2
                            .ClaimPayment.Payee.Address.AddressLine3 = v_oClaimPayment.Payee.Address.Address3
                            .ClaimPayment.Payee.Address.AddressLine4 = v_oClaimPayment.Payee.Address.Address4
                            .ClaimPayment.Payee.Address.AddressTypeCode = v_oClaimPayment.Payee.Address.AddressType
                            .ClaimPayment.Payee.Address.CountryCode = v_oClaimPayment.Payee.Address.CountryCode
                            If v_oClaimPayment.Payee.Address.PostCode IsNot Nothing Then
                                .ClaimPayment.Payee.Address.PostCode = v_oClaimPayment.Payee.Address.PostCode
                            End If
                        End If
                        .ClaimPayment.Payee.BankCode = v_oClaimPayment.Payee.BankCode
                        .ClaimPayment.Payee.BankName = v_oClaimPayment.Payee.BankName
                        .ClaimPayment.Payee.BankNumber = v_oClaimPayment.Payee.BankNumber
                        .ClaimPayment.Payee.Comments = v_oClaimPayment.Payee.Comments
                        .ClaimPayment.Payee.MediaReference = v_oClaimPayment.Payee.MediaReference
                        .ClaimPayment.Payee.MediaTypeCode = v_oClaimPayment.Payee.MediaTypeCode
                        .ClaimPayment.Payee.Name = v_oClaimPayment.Payee.Name
                        .ClaimPayment.Payee.TheirReference = v_oClaimPayment.Payee.TheirReference
                        .ClaimPayment.Payee.PartyBankKey = v_oClaimPayment.Payee.PartyBankKey
                    End If

                    'Payment Collection Data
                    If ClaimPaymentCollection IsNot Nothing Then
                        oBindClaimRequest.ClaimPerilPayment = New List(Of BaseClasses.BaseClaimPaymentType)
                        For Each ClaimPaymentItem As ClaimPayment In ClaimPaymentCollection
                            Dim oClaimPayment As New BaseClasses.BaseClaimPaymentType()
                            oClaimPayment = New BaseClasses.BaseClaimPaymentType()
                            oClaimPayment.BaseClaimKey = ClaimPaymentItem.BaseClaimKey
                            oClaimPayment.BaseClaimPerilKey = ClaimPaymentItem.BaseClaimPerilKey
                            oClaimPayment.ClaimVersionDescription = ClaimPaymentItem.ClaimVersionDescription
                            oClaimPayment.CloseClaimOnZeroReserveRecoveryBalance = ClaimPaymentItem.CloseClaimOnZeroReserveRecoveryBalance
                            oClaimPayment.CurrencyCode = ClaimPaymentItem.CurrencyCode
                            oClaimPayment.PartyKey = ClaimPaymentItem.PartyKey
                            oClaimPayment.ClientKey = ClaimPaymentItem.ClientKey
                            oClaimPayment.PaymentPartyType = ClaimPaymentItem.PaymentPartyType
                            oClaimPayment.OurRef = ClaimPaymentItem.OurRef
                            .ApiTimeStamp = v_bTimeStamp
                            Dim j As Integer = 0
                            Dim oClaimPaymentTaxItems(ClaimPaymentItem.ClaimPaymentTaxItems.Count - 1) As BaseClasses.BaseClaimPaymentTaxItemType
                            For index As Integer = 0 To ClaimPaymentItem.ClaimPaymentTaxItems.Count - 1
                                'If v_oClaimPayment.ClaimPaymentItem(index).PaymentAmount <> 0.0 Then
                                oClaimPaymentTaxItems(j) = New BaseClasses.BaseClaimPaymentTaxItemType()
                                oClaimPaymentTaxItems(j).Amount = ClaimPaymentItem.ClaimPaymentTaxItems(index).Amount
                                oClaimPaymentTaxItems(j).Percentage = ClaimPaymentItem.ClaimPaymentTaxItems(index).Percentage
                                oClaimPaymentTaxItems(j).ReserveType = ClaimPaymentItem.ClaimPaymentTaxItems(index).ReserveType
                                oClaimPaymentTaxItems(j).TaxBandCode = ClaimPaymentItem.ClaimPaymentTaxItems(index).TaxBandCode
                                oClaimPaymentTaxItems(j).TaxGroupCode = ClaimPaymentItem.ClaimPaymentTaxItems(index).TaxGroupCode
                                oClaimPaymentTaxItems(j).ClassOfBusinessID = ClaimPaymentItem.ClaimPaymentTaxItems(index).ClassOfBusinessID
                                oClaimPaymentTaxItems(j).IsManuallyChanges = ClaimPaymentItem.ClaimPaymentTaxItems(index).IsManuallyChanges
                                oClaimPaymentTaxItems(j).Sequence = ClaimPaymentItem.ClaimPaymentTaxItems(index).Sequence
                                oClaimPaymentTaxItems(j).TaxBandId = ClaimPaymentItem.ClaimPaymentTaxItems(index).TaxBandId
                                oClaimPaymentTaxItems(j).TaxGroupId = ClaimPaymentItem.ClaimPaymentTaxItems(index).TaxGroupId
                                j += 1
                                'End If
                            Next

                            oClaimPayment.ClaimPaymentTaxItems = oClaimPaymentTaxItems.ToList

                            If ClaimPaymentItem.PaymentAdvancedTaxDetails.InsuranceTaxNumber <> String.Empty Then
                                oClaimPayment.AdvancedTaxDetails = New BaseClasses.BaseClaimPaymentAdvancedTaxDetailsType()
                                oClaimPayment.AdvancedTaxDetails.InsuranceTaxNumber = ClaimPaymentItem.PaymentAdvancedTaxDetails.InsuranceTaxNumber
                                oClaimPayment.AdvancedTaxDetails.InsuredDomiciled = ClaimPaymentItem.PaymentAdvancedTaxDetails.InsuredDomiciled
                                oClaimPayment.AdvancedTaxDetails.InsuredPercentage = ClaimPaymentItem.PaymentAdvancedTaxDetails.InsuredPercentage
                                oClaimPayment.AdvancedTaxDetails.IsSettlement = ClaimPaymentItem.PaymentAdvancedTaxDetails.IsSettlement
                                oClaimPayment.AdvancedTaxDetails.IsTaxExempt = ClaimPaymentItem.PaymentAdvancedTaxDetails.IsTaxExempt
                                oClaimPayment.AdvancedTaxDetails.IsWHTExempt = ClaimPaymentItem.PaymentAdvancedTaxDetails.IsWHTExempt
                                oClaimPayment.AdvancedTaxDetails.PayeeDomiciled = ClaimPaymentItem.PaymentAdvancedTaxDetails.PayeeDomiciled
                                oClaimPayment.AdvancedTaxDetails.PayeePercentage = ClaimPaymentItem.PaymentAdvancedTaxDetails.PayeePercentage
                                oClaimPayment.AdvancedTaxDetails.PayeeTaxNumber = ClaimPaymentItem.PaymentAdvancedTaxDetails.PayeeTaxNumber
                                oClaimPayment.AdvancedTaxDetails.SafeHarbourCode = ClaimPaymentItem.PaymentAdvancedTaxDetails.SafeHarbourCode
                                oClaimPayment.AdvancedTaxDetails.SafeHarbourPercentage = ClaimPaymentItem.PaymentAdvancedTaxDetails.SafeHarbourPercentage
                                oClaimPayment.AdvancedTaxDetails.PaymentTo = ClaimPaymentItem.PaymentAdvancedTaxDetails.PaymentTo
                                oClaimPayment.AdvancedTaxDetails.PayeeName = ClaimPaymentItem.PaymentAdvancedTaxDetails.PayeeName
                            End If

                            If ClaimPaymentItem.CashList.BankAccountCode <> String.Empty Then
                                oClaimPayment.CashList = New BaseClasses.BasePaymentCashListType()
                                oClaimPayment.CashList.BankAccountCode = ClaimPaymentItem.CashList.BankAccountCode
                                oClaimPayment.CashList.CurrencyCode = ClaimPaymentItem.CashList.CurrencyCode
                                oClaimPayment.CashList.ListDate = ClaimPaymentItem.CashList.ListDate
                                oClaimPayment.CashList.Reference = ClaimPaymentItem.CashList.Reference
                                oClaimPayment.CashList.StatusCode = ClaimPaymentItem.CashList.StatusCode
                                oClaimPayment.CashList.TypeCode = ClaimPaymentItem.CashList.TypeCode
                                oClaimPayment.CashList.BranchCode = sBranchCode
                                oClaimPayment.CashList.PaymentItem = New List(Of BaseClasses.BasePaymentCashListItemType)

                                For index As Integer = 0 To ClaimPaymentItem.CashList.PaymentCashListItemType.Count - 1
                                    With ClaimPaymentItem.CashList.PaymentCashListItemType(index)
                                        oPaymentItem = New BaseClasses.BasePaymentCashListItemType()
                                        oPaymentItem.AccountShortCode = .AccountShortCode
                                        oPaymentItem.AllocationStatusCode = .AllocationStatusCode
                                        oPaymentItem.Amount = .Amount

                                        If .BankPaymentType.AccountCode.Trim.Length <> 0 Then
                                            oPaymentItem.Bank = New BaseClasses.BaseBankPaymentType
                                            oPaymentItem.Bank.AccountCode = .BankPaymentType.AccountCode
                                            oPaymentItem.Bank.BranchCode = .BankPaymentType.BranchCode
                                            oPaymentItem.Bank.ExpiryDate = .BankPaymentType.ExpiryDate
                                            If .BankPaymentType.ExpiryDate <> DateTime.MinValue Then
                                                oPaymentItem.Bank.ExpiryDateSpecified = True
                                            Else
                                                oPaymentItem.Bank.ExpiryDateSpecified = False
                                            End If
                                            oPaymentItem.Bank.PayeeName = .BankPaymentType.PayeeName
                                            oPaymentItem.Bank.Reference1 = .BankPaymentType.Reference1
                                            oPaymentItem.Bank.Reference2 = .BankPaymentType.Reference2
                                            oPaymentItem.BankReference = .BankReference
                                            oPaymentItem.Bank.BIC = .BankPaymentType.BIC
                                            oPaymentItem.Bank.IBAN = .BankPaymentType.IBAN
                                        End If

                                        If Trim(.ContactAddress.Address1) <> String.Empty Then
                                            oPaymentItem.ContactAddress = New BaseClasses.BaseSimpleAddressType
                                            oPaymentItem.ContactAddress.AddressLine1 = .ContactAddress.Address1
                                            oPaymentItem.ContactAddress.AddressLine2 = .ContactAddress.Address2
                                            oPaymentItem.ContactAddress.AddressLine3 = .ContactAddress.Address3
                                            oPaymentItem.ContactAddress.AddressLine4 = .ContactAddress.Address4
                                            oPaymentItem.ContactAddress.CountryCode = .ContactAddress.CountryCode
                                            oPaymentItem.ContactAddress.PostCode = .ContactAddress.PostCode
                                        End If
                                        oPaymentItem.ContactName = .ContactName
                                        oPaymentItem.FurtherDetails = .FurtherDetails
                                        oPaymentItem.MediaReference = .MediaReference
                                        oPaymentItem.MediaTypeCode = .MediaTypeCode
                                        oPaymentItem.OurReference = .OurReference
                                        oPaymentItem.StatusCode = .StatusCode
                                        oPaymentItem.TheirReference = .TheirReference
                                        oPaymentItem.TypeCode = .TypeCode
                                        oPaymentItem.TransactionDate = .TransactionDate
                                        oPaymentItem.ChequeDate = .ChequeDate
                                    End With
                                    oClaimPayment.CashList.PaymentItem.Add(oPaymentItem)
                                Next

                            End If

                            oClaimPayment.ClaimPaymentItem = New List(Of BaseClasses.BaseClaimPaymentItemType)
                            For index As Integer = 0 To ClaimPaymentItem.ClaimPaymentItem.Count - 1
                                'If v_oClaimPayment.ClaimPaymentItem(index).PaymentAmount <> 0.0 Then
                                oClaimPaymentItemType = New BaseClasses.BaseClaimPaymentItemType()
                                oClaimPaymentItemType.PaymentAmount = ClaimPaymentItem.ClaimPaymentItem(index).PaymentAmount
                                oClaimPaymentItemType.BaseReserveKey = ClaimPaymentItem.ClaimPaymentItem(index).BaseReserveKey
                                oClaimPaymentItemType.ReverseExcess = ClaimPaymentItem.ClaimPaymentItem(index).ReverseExcess
                                oClaimPaymentItemType.TaxGroupCode = ClaimPaymentItem.ClaimPaymentItem(index).TaxGroupCode
                                i += 1
                                'End If
                                oClaimPayment.ClaimPaymentItem.Add(oClaimPaymentItemType)
                            Next

                            oClaimPayment.Payee = New BaseClasses.BaseClaimPayeeType()
                            If Trim(ClaimPaymentItem.Payee.Address.Address1) <> String.Empty Then
                                oClaimPayment.Payee.Address = New BaseClasses.BaseAddressType()
                                oClaimPayment.Payee.Address.AddressLine1 = ClaimPaymentItem.Payee.Address.Address1
                                oClaimPayment.Payee.Address.AddressLine2 = ClaimPaymentItem.Payee.Address.Address2
                                oClaimPayment.Payee.Address.AddressLine3 = ClaimPaymentItem.Payee.Address.Address3
                                oClaimPayment.Payee.Address.AddressLine4 = ClaimPaymentItem.Payee.Address.Address4
                                oClaimPayment.Payee.Address.AddressTypeCode = ClaimPaymentItem.Payee.Address.AddressType
                                oClaimPayment.Payee.Address.CountryCode = ClaimPaymentItem.Payee.Address.CountryCode
                                If ClaimPaymentItem.Payee.Address.PostCode IsNot Nothing Then
                                    oClaimPayment.Payee.Address.PostCode = ClaimPaymentItem.Payee.Address.PostCode
                                End If
                            End If
                            oClaimPayment.Payee.BankCode = ClaimPaymentItem.Payee.BankCode
                            oClaimPayment.Payee.BankName = ClaimPaymentItem.Payee.BankName
                            oClaimPayment.Payee.BankNumber = ClaimPaymentItem.Payee.BankNumber
                            oClaimPayment.Payee.Comments = ClaimPaymentItem.Payee.Comments
                            oClaimPayment.Payee.MediaReference = ClaimPaymentItem.Payee.MediaReference
                            oClaimPayment.Payee.MediaTypeCode = ClaimPaymentItem.Payee.MediaTypeCode
                            oClaimPayment.Payee.Name = ClaimPaymentItem.Payee.Name
                            oClaimPayment.Payee.TheirReference = ClaimPaymentItem.Payee.TheirReference
                            oClaimPayment.Payee.PartyBankKey = ClaimPaymentItem.Payee.PartyBankKey
                            .ClaimPerilPayment.Add(oClaimPayment)
                        Next
                    End If

                    .IgnoreWarnings = True
                    .SkipSaveTransaction = bSkipSaveTransaction
                    If (HttpContext.Current.Session(CNNoTrans) IsNot Nothing AndAlso HttpContext.Current.Session(CNNoTrans).ToString() = "Claim") Then
                        .NoTrans = True
                    End If
                    .SkipSaveTransaction = bSkipSaveTransaction
                End With

                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Post(ApiMethods.BindClaim, oBindClaimRequest)
                    oBindClaimResponse = ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BindClaimCommandResponse)(result)
                End Using

                With oBindClaimResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        oClaimResponse.ResultingStatus = .ResultingStatus
                        oClaimResponse.PaymentAuthorized = .PaymentAuthorized
                        oClaimResponse.TimeStamp = .ApiTimeStamp
                        If Not IsNothing(.Warnings) Then
                            Dim oWarningCollection As New WarningCollection
                            Dim oWarningItem As New Warnings
                            For Each oWarning As BaseClasses.BaseClaimResponseTypeWarnings In .Warnings
                                oWarningItem.Code = oWarning.Code
                                oWarningItem.Description = oWarning.Description
                                oWarningCollection.Add(oWarningItem)
                            Next
                            oClaimResponse.Warnings = oWarningCollection
                        End If
                    End If
                End With
            Catch ex As Exception
                Throw
            Finally
                oBindClaimRequest = Nothing
                oBindClaimResponse = Nothing
            End Try
            Return oClaimResponse
        End SyncLock
    End Function



    ''' <summary>
    '''  This Method Calculate the UnAllocated Band for Claim
    ''' </summary>
    ''' <param name="oRITable"></param>
    ''' <param name="oXMLDoc"></param>
    ''' <param name="RIBand"></param>
    ''' <remarks></remarks>
    Sub CalculateClaimUnAllocated(ByVal oRITable As DataTable, ByRef oXMLDoc As XmlDocument, ByVal RIBand As XmlElement)
        'Calculate/Retreive Band Total
        Dim dBANDSumInsured, dBANDReserveToDate, dBANDThisReserve, dBANDPaymentToDate, dBANDBalance, dBANDRecoverToDate As Decimal
        Dim oNode As XmlNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & oRITable.TableName & "']/ArrangementRow[@Name='Band Total']")
        If oNode IsNot Nothing Then
            Decimal.TryParse(oNode.Attributes("SumInsured").Value, dBANDSumInsured)
            Decimal.TryParse(oNode.Attributes("ReserveToDate").Value, dBANDReserveToDate)
            Decimal.TryParse(oNode.Attributes("ThisReserve").Value, dBANDThisReserve)
            Decimal.TryParse(oNode.Attributes("PaymentToDate").Value, dBANDPaymentToDate)
            Decimal.TryParse(oNode.Attributes("Balance").Value, dBANDBalance)
            Decimal.TryParse(oNode.Attributes("RecoverToDate").Value, dBANDRecoverToDate)
        End If

        'Calculate/Retreive Allocated Total
        Dim dAllocatedSumInsured, dAllocatedReserveToDate, dAllocatedThisReserve, dAllocatedPaymentToDate, dAllocatedBalance, dAllocatedRecoverToDate As Decimal
        oNode = oXMLDoc.SelectSingleNode("/rows/RIBAND[@Name='" & oRITable.TableName & "']/ArrangementRow[@Name='Allocated']")
        If oNode IsNot Nothing Then
            Decimal.TryParse(oNode.Attributes("SumInsured").Value, dAllocatedSumInsured)
            Decimal.TryParse(oNode.Attributes("ReserveToDate").Value, dAllocatedReserveToDate)
            Decimal.TryParse(oNode.Attributes("ThisReserve").Value, dAllocatedThisReserve)
            Decimal.TryParse(oNode.Attributes("PaymentToDate").Value, dAllocatedPaymentToDate)
            Decimal.TryParse(oNode.Attributes("Balance").Value, dAllocatedBalance)
            Decimal.TryParse(oNode.Attributes("RecoverToDate").Value, dAllocatedRecoverToDate)
        End If

        'Add UnAllocated if there is Any
        Dim dUnAllocatedSumInsured, dUnAllocatedReserveToDate, dUnAllocatedThisReserve, dUnAllocatedPaymentToDate, dUnAllocatedRecoverToDate, dUnAllocatedBalance As Decimal
        dUnAllocatedSumInsured = dBANDSumInsured - dAllocatedSumInsured
        dUnAllocatedReserveToDate = dBANDReserveToDate - dAllocatedReserveToDate
        dUnAllocatedThisReserve = dBANDThisReserve - dAllocatedThisReserve
        dUnAllocatedPaymentToDate = dBANDPaymentToDate - dAllocatedPaymentToDate
        dUnAllocatedRecoverToDate = dBANDRecoverToDate - dAllocatedRecoverToDate
        dUnAllocatedBalance = dBANDBalance - dAllocatedBalance

        'Check for unallocated amounts and update from r type node

        Dim oRNodes As XmlNodeList = oXMLDoc.SelectNodes("/rows/RIBAND[@Name='" & oRITable.TableName & "']/ArrangementRow[@Type='R']")
        If oRNodes IsNot Nothing AndAlso oRNodes.Count > 0 Then
            For Each oRNode As XmlNode In oRNodes
                If oRNode IsNot Nothing Then
                    If dUnAllocatedThisReserve <> 0 Then
                        oRNode.Attributes("ThisReserve").Value = Convert.ToDouble(oRNode.Attributes("ThisReserve").Value) + dUnAllocatedThisReserve
                        dUnAllocatedThisReserve = 0
                    End If
                End If
            Next
        End If

        If dUnAllocatedSumInsured <> 0 Or dUnAllocatedReserveToDate <> 0 Or dUnAllocatedThisReserve <> 0 Or dUnAllocatedPaymentToDate <> 0 _
      Or dUnAllocatedBalance <> 0 Or dUnAllocatedRecoverToDate <> 0 Then
            'Add into the XML
            Dim sArrangementRow As String = "ArrangementRow"
            Dim ArrangementRow As XmlElement = oXMLDoc.CreateElement(sArrangementRow)
            Dim myCol As DataColumn
            Dim sValue As String = ""
            For Each myCol In oRITable.Columns
                If myCol.ColumnName = "SumInsured" Or myCol.ColumnName = "ReserveToDate" _
                Or myCol.ColumnName = "Name" Or myCol.ColumnName = "ThisReserve" _
                Or myCol.ColumnName = "PaymentToDate" Or myCol.ColumnName = "Balance" _
                Or myCol.ColumnName = "RecoverToDate" Then

                    'Name
                    If myCol.ColumnName = "Name" Then
                        ArrangementRow.SetAttribute(myCol.ColumnName, "Unallocated")
                    End If

                    'Sum Insured
                    sValue = ""
                    If myCol.ColumnName = "SumInsured" Then
                        sValue = dUnAllocatedSumInsured
                        ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                    End If

                    'ReserveToDate
                    sValue = ""
                    If myCol.ColumnName = "ReserveToDate" Then
                        sValue = dUnAllocatedReserveToDate
                        ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                    End If

                    'ThisReserve
                    sValue = ""
                    If myCol.ColumnName = "ThisReserve" Then
                        sValue = dUnAllocatedThisReserve
                        ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                    End If

                    'PaymentToDate
                    sValue = ""
                    If myCol.ColumnName = "PaymentToDate" Then
                        sValue = dUnAllocatedPaymentToDate
                        ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                    End If

                    'Balance
                    sValue = ""
                    If myCol.ColumnName = "Balance" Then
                        sValue = dUnAllocatedBalance
                        ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                    End If

                    'RecoverToDate
                    sValue = ""
                    If myCol.ColumnName = "RecoverToDate" Then
                        sValue = dUnAllocatedRecoverToDate
                        ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                    End If
                Else
                    sValue = ""
                    ArrangementRow.SetAttribute(myCol.ColumnName, sValue)
                End If
            Next

            RIBand.InsertAfter(ArrangementRow, RIBand.LastChild)
        End If
    End Sub


    ''' <summary>
    ''' This method calculates the Tax for claims
    ''' </summary>
    ''' <param name="v_oTaxForClaims"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub CalculateTaxForClaims(ByRef v_oTaxForClaims As NexusProvider.TaxForClaims, Optional ByVal v_sBranchCode As String = Nothing)
        'Dim oSAM As PureClaimServiceClient
        Dim oTaxForClaimRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.CalculateTaxForClaimsCommand
        Dim oTaxForClaimResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.CalculateTaxForClaimsCommandResponse
        Dim sbLogMessage As StringBuilder
        Dim sATSOption As Boolean
        Try
            'oSAM = InitializeClaimServiceMethod()
            oTaxForClaimRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.CalculateTaxForClaimsCommand
            oTaxForClaimResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.CalculateTaxForClaimsCommandResponse
            sbLogMessage = New StringBuilder


            With oTaxForClaimRequest
                .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                '.WCFSecurityToken = SecurityToken()
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
                'If v_oTaxForClaims.Amount > 0.0 Then
                '    .Amount = v_oTaxForClaims.Amount
                'Else
                '    Throw New ArgumentNullException("TaxForClaims.Amount")
                'End If
                .Amount = v_oTaxForClaims.Amount

                .CompanyCode = oTaxForClaimRequest.BranchCode
                .CurrencyCode = v_oTaxForClaims.CurrencyCode
                .LossCurrencyCode = v_oTaxForClaims.LossCurrencyCode
                .TaxGroupCode = v_oTaxForClaims.TaxGroupCode
                .PerilId = v_oTaxForClaims.ClaimPerilID
                .TransactionTypeCode = v_oTaxForClaims.TransactionTypeCode
                .PerilId = v_oTaxForClaims.ClaimPerilID
                .ReserveKey = v_oTaxForClaims.ReserveKey
                .ReserveType = v_oTaxForClaims.ReserveType
                .IsSalvageRecovery = v_oTaxForClaims.IsSalvageRecovery
                If v_oTaxForClaims.PaymentAdvancedTaxDetails IsNot Nothing Then
                    .AdvancedTaxDetails = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPaymentAdvancedTaxDetailsType
                    .AdvancedTaxDetails.PaymentTo = v_oTaxForClaims.PaymentAdvancedTaxDetails.PaymentTo
                    .AdvancedTaxDetails.InsuranceTaxNumber = v_oTaxForClaims.PaymentAdvancedTaxDetails.InsuranceTaxNumber
                    .AdvancedTaxDetails.InsuredDomiciled = v_oTaxForClaims.PaymentAdvancedTaxDetails.InsuredDomiciled
                    .AdvancedTaxDetails.InsuredPercentage = v_oTaxForClaims.PaymentAdvancedTaxDetails.InsuredPercentage
                    .AdvancedTaxDetails.IsSettlement = v_oTaxForClaims.PaymentAdvancedTaxDetails.IsSettlement
                    .AdvancedTaxDetails.IsTaxExempt = v_oTaxForClaims.PaymentAdvancedTaxDetails.IsTaxExempt
                    .AdvancedTaxDetails.IsWHTExempt = v_oTaxForClaims.PaymentAdvancedTaxDetails.IsWHTExempt
                    .AdvancedTaxDetails.PayeeDomiciled = v_oTaxForClaims.PaymentAdvancedTaxDetails.PayeeDomiciled
                    .AdvancedTaxDetails.PayeePercentage = v_oTaxForClaims.PaymentAdvancedTaxDetails.PayeePercentage
                    .AdvancedTaxDetails.PayeeTaxNumber = v_oTaxForClaims.PaymentAdvancedTaxDetails.PayeeTaxNumber
                    .AdvancedTaxDetails.SafeHarbourCode = v_oTaxForClaims.PaymentAdvancedTaxDetails.SafeHarbourCode
                    .AdvancedTaxDetails.SafeHarbourPercentage = v_oTaxForClaims.PaymentAdvancedTaxDetails.SafeHarbourPercentage
                    .AdvancedTaxDetails.IsExcess = v_oTaxForClaims.PaymentAdvancedTaxDetails.IsExcess
                    .AdvancedTaxDetails.AdvancedTaxScriptOptionOn = v_oTaxForClaims.PaymentAdvancedTaxDetails.AdvancedTaxScriptOptionOn
                    .AdvancedTaxDetails.PayeeName = v_oTaxForClaims.PaymentAdvancedTaxDetails.PayeeName
                    sATSOption = v_oTaxForClaims.PaymentAdvancedTaxDetails.AdvancedTaxScriptOptionOn
                End If

                If v_oTaxForClaims.ReceiptAdvancedTaxDetails IsNot Nothing Then
                    .ReceiptAdvancedTaxDetails = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimReceiptAdvancedTaxDetailsType

                    If v_oTaxForClaims.ReceiptAdvancedTaxDetails.InsuredDomiciled Then
                        .ReceiptAdvancedTaxDetails.InsuredDomiciled = v_oTaxForClaims.ReceiptAdvancedTaxDetails.InsuredDomiciled
                        .ReceiptAdvancedTaxDetails.InsuredDomiciledSpecified = True
                    End If
                    If v_oTaxForClaims.ReceiptAdvancedTaxDetails.InsuredPercentage <> 0 Then
                        .ReceiptAdvancedTaxDetails.InsuredPercentage = v_oTaxForClaims.ReceiptAdvancedTaxDetails.InsuredPercentage
                        .ReceiptAdvancedTaxDetails.InsuredPercentageSpecified = True
                    End If

                    .ReceiptAdvancedTaxDetails.InsuredTaxNumber = v_oTaxForClaims.ReceiptAdvancedTaxDetails.InsuredTaxNumber
                    .ReceiptAdvancedTaxDetails.IsTaxExempt = v_oTaxForClaims.ReceiptAdvancedTaxDetails.IsTaxExempt

                    If v_oTaxForClaims.ReceiptAdvancedTaxDetails.ReceivableTaxPercentage <> 0 Then
                        .ReceiptAdvancedTaxDetails.ReceivableTaxPercentage = v_oTaxForClaims.ReceiptAdvancedTaxDetails.ReceivableTaxPercentage
                        .ReceiptAdvancedTaxDetails.InsuredPercentageSpecified = True
                    End If

                    .ReceiptAdvancedTaxDetails.PayeeName = v_oTaxForClaims.ReceiptAdvancedTaxDetails.PayeeName
                    .ReceiptAdvancedTaxDetails.AdvancedTaxScriptOptionOn = v_oTaxForClaims.ReceiptAdvancedTaxDetails.AdvancedTaxScriptOptionOn
                    sATSOption = v_oTaxForClaims.ReceiptAdvancedTaxDetails.AdvancedTaxScriptOptionOn
                End If
            End With


            'Calling the SAM Method with Request Type
            'add trace to allow us to debug slow SAM calls
            Using trace As New Tracer(Category.Trace)
                'oTaxForClaimResponse = oSAM.CalculateTaxForClaims(oTaxForClaimRequest)
                SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.CalculateTaxForClaims, oTaxForClaimRequest)
                oTaxForClaimResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.CalculateTaxForClaimsCommandResponse)(result)

            End Using
            'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

            ' Disposing the SAM's object


            With oTaxForClaimResponse
                If .Errors IsNot Nothing Then
                    'Process the error object if errors, and throw as a single exception
                    Throw New NexusException(.Errors)
                Else
                    v_oTaxForClaims.TaxBaseAmount = .TaxBaseAmount
                    v_oTaxForClaims.TaxCurrencyAmount = .TaxCurrencyAmount
                    v_oTaxForClaims.TaxLossAmount = .TaxLossAmount
                    If sATSOption Then
                        If Not v_oTaxForClaims.PaymentAdvancedTaxDetails Is Nothing Then
                            If v_oTaxForClaims.PaymentAdvancedTaxDetails.AdvancedTaxScriptOptionOn OrElse v_oTaxForClaims.ReceiptAdvancedTaxDetails.AdvancedTaxScriptOptionOn Then
                                If .TaxItems IsNot Nothing Then
                                    v_oTaxForClaims.TaxItems = New NexusProvider.ClaimPaymentTaxItemCollection
                                    For Each oClaimPaymentTax As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPaymentTaxItemType In .TaxItems
                                        Dim oTaxItem As New NexusProvider.ClaimPaymentTaxItem
                                        With oClaimPaymentTax
                                            oTaxItem.Amount = .Amount
                                            oTaxItem.Percentage = .Percentage
                                            oTaxItem.ReserveType = .ReserveType
                                            oTaxItem.TaxBandCode = .TaxBandCode
                                            oTaxItem.TaxGroupCode = .TaxGroupCode
                                            oTaxItem.ClassOfBusinessID = .ClassOfBusinessID
                                            oTaxItem.IsManuallyChanges = .IsManuallyChanges
                                            oTaxItem.Sequence = .Sequence
                                            oTaxItem.TaxBandId = .TaxBandId
                                            oTaxItem.TaxGroupId = .TaxGroupId
                                            oTaxItem.ReserveTypeCode = v_oTaxForClaims.ReserveType
                                        End With
                                        v_oTaxForClaims.TaxItems.Add(oTaxItem)
                                    Next
                                End If

                                If .ReceiptTaxItems IsNot Nothing Then
                                    v_oTaxForClaims.ReceiptTaxItem = New NexusProvider.TaxItemTypeCollection
                                    For Each oClaimReceiptTax As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimReceiptTaxItemType In .ReceiptTaxItems
                                        Dim oTaxItem As New NexusProvider.BaseClaimTaxItemType
                                        With oClaimReceiptTax
                                            oTaxItem.Amount = .Amount
                                            oTaxItem.Percentage = .Percentage
                                            oTaxItem.RecoveryType = v_oTaxForClaims.RecoveryType
                                            oTaxItem.TaxBandCode = .TaxBandCode
                                            oTaxItem.TaxGroupCode = .TaxGroupCode
                                            'oTaxItem.ClassOfBusinessID = .ClassOfBusinessID
                                            oTaxItem.IsManuallyChanges = .IsManuallyChanges
                                            oTaxItem.Sequence = .Sequence
                                            oTaxItem.TaxBandId = .TaxBandId
                                            oTaxItem.TaxGroupId = .TaxGroupId
                                            oTaxItem.RecoveryType = v_oTaxForClaims.RecoveryType
                                        End With
                                        v_oTaxForClaims.ReceiptTaxItem.Add(oTaxItem)
                                    Next
                                End If
                            End If
                        End If
                    End If
                End If
            End With
            If Logger.IsLoggingEnabled Then
                sbLogMessage.AppendLine("CalculateTaxForClaims executed ok" & vbCrLf)
                sbLogMessage.AppendLine(v_oTaxForClaims.Print.ToString() & vbCrLf)
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
            'oSAM.Close()
            oTaxForClaimRequest = Nothing
            oTaxForClaimResponse = Nothing
        End Try


    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_oBaseCaseKey"></param>
    ''' <param name="v_oClaimKey"></param>
    ''' <param name="v_oIsLinked"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function CaseLinkUnlink(Optional ByVal v_oBaseCaseKey As Integer = 0,
                                        Optional ByVal v_oClaimKey As Integer = 0,
                                        Optional ByVal v_oIsLinked As Boolean = False,
                                        Optional ByVal v_sBranchCode As String = Nothing) As WarningCollection

        SyncLock oLock
            Dim oCaseLinkUnlinkRequest As BaseClasses.CaseLinkUnlinkCommand = Nothing  ' Request Type
            Dim oCaseLinkUnlinkResponse As BaseClasses.CaseLinkUnlinkCommandResponse = Nothing  ' Response Type
            Dim oWarnings As NexusProvider.WarningCollection = Nothing
            Dim oWarning As NexusProvider.Warnings
            Try
                oCaseLinkUnlinkRequest = New BaseClasses.CaseLinkUnlinkCommand
                oCaseLinkUnlinkResponse = New BaseClasses.CaseLinkUnlinkCommandResponse
                oWarnings = New WarningCollection

                With oCaseLinkUnlinkRequest
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

                    .BaseCaseKey = v_oBaseCaseKey
                    .ClaimKey = v_oClaimKey
                    .IsLinked = v_oIsLinked
                End With
                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Post(ApiMethods.CaseLinkUnlink, oCaseLinkUnlinkRequest)
                    oCaseLinkUnlinkResponse = ApiClient.DeserializeJson(Of BaseClasses.CaseLinkUnlinkCommandResponse)(result)
                End Using
                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                With oCaseLinkUnlinkResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        If oCaseLinkUnlinkResponse.Warnings IsNot Nothing Then
                            For iCt As Integer = 0 To oCaseLinkUnlinkResponse.Warnings.Count - 1
                                oWarning = New NexusProvider.Warnings()
                                oWarning.Code = oCaseLinkUnlinkResponse.Warnings(iCt).Code
                                oWarning.Description = oCaseLinkUnlinkResponse.Warnings(iCt).Description
                                oWarnings.Add(oWarning)
                            Next
                        End If
                    End If
                End With

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oCaseLinkUnlinkRequest = Nothing
                oCaseLinkUnlinkResponse = Nothing
            End Try
            Return oWarnings
        End SyncLock
    End Function

    Public Overrides Sub ClaimReceipt(ByRef r_oClaimReceipt As ClaimReceipt,
                                         Optional ByVal v_sBranchCode As String = Nothing, Optional ByVal ClaimReceiptCollection As NexusProvider.ClaimReceiptCollection = Nothing)
        SyncLock oLock

            ' Dim oSAM As PureClaimServiceClient
            Dim oClaimReceiptRequest As BaseClasses.ClaimReceiptCommand
            Dim oClaimReceiptResponse As BaseClasses.ClaimReceiptCommandResponse
            Dim oClaimReceipt As BaseClasses.BaseClaimReceiptType
            Dim oAdvanceTaxDetails As BaseClasses.BaseClaimReceiptAdvancedTaxDetailsType
            Dim oClaimReceiptItemType As BaseClasses.BaseClaimReceiptItemType
            Dim oPayee As BaseClasses.BaseClaimPayeeType
            Dim oAddress As BaseClasses.BaseAddressType
            Dim oClainReceiptWarning As NexusProvider.ClaimReceiptWarning
            Dim sbLogMessage As StringBuilder
            Dim IsSalvageTPExcludeTax As NexusProvider.OptionTypeSetting
            Try
                'oSAM = InitializeClaimServiceMethod()

                IsSalvageTPExcludeTax = GetOptionSetting(NexusProvider.OptionType.SystemOption, 5067)

                oClaimReceiptRequest = New BaseClasses.ClaimReceiptCommand
                oClaimReceiptResponse = New BaseClasses.ClaimReceiptCommandResponse
                oAdvanceTaxDetails = New BaseClasses.BaseClaimReceiptAdvancedTaxDetailsType
                oPayee = New BaseClasses.BaseClaimPayeeType
                oAddress = New BaseClasses.BaseAddressType
                oClainReceiptWarning = New NexusProvider.ClaimReceiptWarning
                sbLogMessage = New StringBuilder
                With oClaimReceiptRequest
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

                    If r_oClaimReceipt IsNot Nothing Then
                        oClaimReceipt = New BaseClasses.BaseClaimReceiptType
                        .ApiTimeStamp = r_oClaimReceipt.TimeStamp
                        oClaimReceipt.BaseClaimKey = r_oClaimReceipt.BaseClaimKey
                        oClaimReceipt.BaseClaimPerilKey = r_oClaimReceipt.BaseClaimPerilKey
                        oClaimReceipt.ReceiptPartyType = r_oClaimReceipt.ReceiptPartyType
                        oClaimReceipt.PartyKey = r_oClaimReceipt.PartyKey
                        oClaimReceipt.CurrencyCode = r_oClaimReceipt.CurrencyCode
                        oClaimReceipt.ClaimVersionDescription = r_oClaimReceipt.ClaimVersionDescription
                        oClaimReceipt.DoNotCreateClaimVersionOnSalvageReceipt = r_oClaimReceipt.DoNotCreateClaimVersionOnSalvageReceipt


                        If r_oClaimReceipt.AdvancedTaxDetails.InsuredTaxNumber <> String.Empty Then

                            oAdvanceTaxDetails.IsSettlement = r_oClaimReceipt.AdvancedTaxDetails.IsSettlement
                            oAdvanceTaxDetails.IsTaxExempt = r_oClaimReceipt.AdvancedTaxDetails.IsTaxExempt
                            If r_oClaimReceipt.AdvancedTaxDetails.ReceivableTaxPercentage <> 0 Then
                                oAdvanceTaxDetails.ReceivableTaxPercentage = r_oClaimReceipt.AdvancedTaxDetails.ReceivableTaxPercentage
                                oAdvanceTaxDetails.InsuredPercentageSpecified = True
                            End If
                            If r_oClaimReceipt.AdvancedTaxDetails.InsuredDomiciled Then
                                oAdvanceTaxDetails.InsuredDomiciled = r_oClaimReceipt.AdvancedTaxDetails.InsuredDomiciled
                                oAdvanceTaxDetails.InsuredDomiciledSpecified = True
                            End If
                            If r_oClaimReceipt.AdvancedTaxDetails.InsuredPercentage <> 0 Then
                                oAdvanceTaxDetails.InsuredPercentage = r_oClaimReceipt.AdvancedTaxDetails.InsuredPercentage
                                oAdvanceTaxDetails.InsuredPercentageSpecified = True
                            End If
                            oAdvanceTaxDetails.InsuredTaxNumber = r_oClaimReceipt.AdvancedTaxDetails.InsuredTaxNumber

                            oClaimReceipt.AdvancedTaxDetails = oAdvanceTaxDetails
                        End If

                        If r_oClaimReceipt.ReceiptItem IsNot Nothing AndAlso r_oClaimReceipt.ReceiptItem.Count > 0 Then
                            If r_oClaimReceipt.ReceiptItem.Count > 0 Then
                                oClaimReceipt.ReceiptItem = New List(Of BaseClasses.BaseClaimReceiptItemType)
                                For iRecovery As Integer = 0 To r_oClaimReceipt.ReceiptItem.Count - 1
                                    If r_oClaimReceipt.ReceiptItem(iRecovery).ThisReceiptAmount <> 0 Then
                                        oClaimReceiptItemType = New BaseClasses.BaseClaimReceiptItemType
                                        oClaimReceiptItemType.BaseRecoveryKey = r_oClaimReceipt.ReceiptItem(iRecovery).RecoveryKey
                                        oClaimReceiptItemType.ReceiptAmount = r_oClaimReceipt.ReceiptItem(iRecovery).ThisReceiptINCLTaxAmount 'ThisReceiptNetAmount
                                        oClaimReceiptItemType.TaxGroupCode = r_oClaimReceipt.ReceiptItem(iRecovery).TaxCode
                                        oClaimReceiptItemType.IsTaxOverridden = r_oClaimReceipt.ReceiptItem(iRecovery).IsTaxOverridden
                                        oClaimReceiptItemType.OverriddedTaxAmount = r_oClaimReceipt.ReceiptItem(iRecovery).OverriddedTaxAmount
                                        oClaimReceipt.ReceiptItem.Add(oClaimReceiptItemType)
                                    End If

                                Next
                            End If
                        End If

                        oPayee.Name = r_oClaimReceipt.Payee.Name
                        oPayee.BankName = r_oClaimReceipt.Payee.BankName
                        oPayee.BankNumber = r_oClaimReceipt.Payee.BankNumber
                        oPayee.BankCode = r_oClaimReceipt.Payee.BankCode
                        oPayee.MediaTypeCode = r_oClaimReceipt.Payee.MediaTypeCode
                        oPayee.MediaReference = r_oClaimReceipt.Payee.MediaReference
                        oPayee.TheirReference = r_oClaimReceipt.Payee.TheirReference
                        oPayee.Comments = r_oClaimReceipt.Payee.Comments

                        If r_oClaimReceipt.Payee.Address IsNot Nothing Then
                            oAddress.AddressLine1 = r_oClaimReceipt.Payee.Address.Address1
                            oAddress.AddressLine2 = r_oClaimReceipt.Payee.Address.Address2
                            oAddress.AddressLine3 = r_oClaimReceipt.Payee.Address.Address3
                            oAddress.AddressLine4 = r_oClaimReceipt.Payee.Address.Address4
                            oAddress.AddressTypeCode = r_oClaimReceipt.Payee.Address.AddressType
                            oAddress.CountryCode = r_oClaimReceipt.Payee.Address.CountryCode
                            oAddress.PostCode = r_oClaimReceipt.Payee.Address.PostCode
                        End If

                        oClaimReceipt.Payee = oPayee
                        oClaimReceipt.Payee.Address = oAddress
                        oClaimReceipt.IsSalvageRecovery = r_oClaimReceipt.IsSalvageRecovery

                        .ClaimReceipt = oClaimReceipt
                        .CloseClaimOnZeroReserveRecoveryBalance = r_oClaimReceipt.CloseClaimOnZeroReserveRecoveryBalance
                    ElseIf ClaimReceiptCollection IsNot Nothing Then
                        .ApiTimeStamp = ClaimReceiptCollection(0).TimeStamp
                        .ClaimReceiptCollection = New List(Of BaseClasses.BaseClaimReceiptType)
                        For ReceiptItemIndex As Integer = 0 To ClaimReceiptCollection.Count - 1
                            oClaimReceipt = New BaseClasses.BaseClaimReceiptType
                            oClaimReceipt = New BaseClasses.BaseClaimReceiptType
                            oClaimReceipt.BaseClaimKey = ClaimReceiptCollection(ReceiptItemIndex).BaseClaimKey
                            oClaimReceipt.BaseClaimPerilKey = ClaimReceiptCollection(ReceiptItemIndex).BaseClaimPerilKey
                            oClaimReceipt.ReceiptPartyType = ClaimReceiptCollection(ReceiptItemIndex).ReceiptPartyType
                            oClaimReceipt.PartyKey = ClaimReceiptCollection(ReceiptItemIndex).PartyKey
                            oClaimReceipt.CurrencyCode = ClaimReceiptCollection(ReceiptItemIndex).CurrencyCode
                            oClaimReceipt.ClaimVersionDescription = ClaimReceiptCollection(ReceiptItemIndex).ClaimVersionDescription
                            oClaimReceipt.DoNotCreateClaimVersionOnSalvageReceipt = ClaimReceiptCollection(ReceiptItemIndex).DoNotCreateClaimVersionOnSalvageReceipt


                            If ClaimReceiptCollection(ReceiptItemIndex).AdvancedTaxDetails.InsuredTaxNumber <> String.Empty Then

                                oAdvanceTaxDetails.IsSettlement = ClaimReceiptCollection(ReceiptItemIndex).AdvancedTaxDetails.IsSettlement
                                oAdvanceTaxDetails.IsTaxExempt = ClaimReceiptCollection(ReceiptItemIndex).AdvancedTaxDetails.IsTaxExempt
                                If ClaimReceiptCollection(ReceiptItemIndex).AdvancedTaxDetails.ReceivableTaxPercentage <> 0 Then
                                    oAdvanceTaxDetails.ReceivableTaxPercentage = ClaimReceiptCollection(ReceiptItemIndex).AdvancedTaxDetails.ReceivableTaxPercentage
                                    oAdvanceTaxDetails.InsuredPercentageSpecified = True
                                End If
                                If ClaimReceiptCollection(ReceiptItemIndex).AdvancedTaxDetails.InsuredDomiciled Then
                                    oAdvanceTaxDetails.InsuredDomiciled = ClaimReceiptCollection(ReceiptItemIndex).AdvancedTaxDetails.InsuredDomiciled
                                    oAdvanceTaxDetails.InsuredDomiciledSpecified = True
                                End If
                                If ClaimReceiptCollection(ReceiptItemIndex).AdvancedTaxDetails.InsuredPercentage <> 0 Then
                                    oAdvanceTaxDetails.InsuredPercentage = ClaimReceiptCollection(ReceiptItemIndex).AdvancedTaxDetails.InsuredPercentage
                                    oAdvanceTaxDetails.InsuredPercentageSpecified = True
                                End If
                                oAdvanceTaxDetails.InsuredTaxNumber = ClaimReceiptCollection(ReceiptItemIndex).AdvancedTaxDetails.InsuredTaxNumber

                                oClaimReceipt.AdvancedTaxDetails = oAdvanceTaxDetails
                            End If

                            If ClaimReceiptCollection(ReceiptItemIndex).ReceiptItem IsNot Nothing AndAlso ClaimReceiptCollection(ReceiptItemIndex).ReceiptItem.Count > 0 Then
                                If ClaimReceiptCollection(ReceiptItemIndex).ReceiptItem.Count > 0 Then
                                    oClaimReceipt.ReceiptItem = New List(Of BaseClasses.BaseClaimReceiptItemType)
                                    For iRecovery As Integer = 0 To ClaimReceiptCollection(ReceiptItemIndex).ReceiptItem.Count - 1
                                        If ClaimReceiptCollection(ReceiptItemIndex).ReceiptItem(iRecovery).ThisReceiptAmount <> 0 Then
                                            oClaimReceiptItemType = New BaseClasses.BaseClaimReceiptItemType
                                            oClaimReceiptItemType.BaseRecoveryKey = ClaimReceiptCollection(ReceiptItemIndex).ReceiptItem(iRecovery).RecoveryKey

                                            oClaimReceiptItemType.ReceiptAmount = ClaimReceiptCollection(ReceiptItemIndex).ReceiptItem(iRecovery).ThisReceiptAmount

                                            oClaimReceiptItemType.TaxGroupCode = ClaimReceiptCollection(ReceiptItemIndex).ReceiptItem(iRecovery).TaxCode
                                            oClaimReceipt.ReceiptItem.Add(oClaimReceiptItemType)
                                        End If

                                    Next
                                End If
                            End If

                            oPayee.Name = ClaimReceiptCollection(ReceiptItemIndex).Payee.Name
                            oPayee.BankName = ClaimReceiptCollection(ReceiptItemIndex).Payee.BankName
                            oPayee.BankNumber = ClaimReceiptCollection(ReceiptItemIndex).Payee.BankNumber
                            oPayee.BankCode = ClaimReceiptCollection(ReceiptItemIndex).Payee.BankCode
                            oPayee.MediaTypeCode = ClaimReceiptCollection(ReceiptItemIndex).Payee.MediaTypeCode
                            oPayee.MediaReference = ClaimReceiptCollection(ReceiptItemIndex).Payee.MediaReference
                            oPayee.TheirReference = ClaimReceiptCollection(ReceiptItemIndex).Payee.TheirReference
                            oPayee.Comments = ClaimReceiptCollection(ReceiptItemIndex).Payee.Comments

                            If ClaimReceiptCollection(ReceiptItemIndex).Payee.Address IsNot Nothing Then
                                oAddress.AddressLine1 = ClaimReceiptCollection(ReceiptItemIndex).Payee.Address.Address1
                                oAddress.AddressLine2 = ClaimReceiptCollection(ReceiptItemIndex).Payee.Address.Address2
                                oAddress.AddressLine3 = ClaimReceiptCollection(ReceiptItemIndex).Payee.Address.Address3
                                oAddress.AddressLine4 = ClaimReceiptCollection(ReceiptItemIndex).Payee.Address.Address4
                                oAddress.AddressTypeCode = ClaimReceiptCollection(ReceiptItemIndex).Payee.Address.AddressType
                                oAddress.CountryCode = ClaimReceiptCollection(ReceiptItemIndex).Payee.Address.CountryCode
                                oAddress.PostCode = ClaimReceiptCollection(ReceiptItemIndex).Payee.Address.PostCode
                            End If

                            oClaimReceipt.Payee = oPayee
                            oClaimReceipt.Payee.Address = oAddress
                            oClaimReceipt.IsSalvageRecovery = ClaimReceiptCollection(ReceiptItemIndex).IsSalvageRecovery

                            .ClaimReceiptCollection.Add(oClaimReceipt)
                        Next
                        .CloseClaimOnZeroReserveRecoveryBalance = ClaimReceiptCollection(0).CloseClaimOnZeroReserveRecoveryBalance
                    End If
                End With

                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.ClaimReceipt, oClaimReceiptRequest)
                    oClaimReceiptResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of BaseClasses.ClaimReceiptCommandResponse)(result)
                End Using

                ' Disposing the SAM's object
                With oClaimReceiptResponse
                    If r_oClaimReceipt IsNot Nothing Then
                        If .Errors IsNot Nothing Then
                            r_oClaimReceipt.TimeStamp = Nothing
                            'Process the error object if errors, and throw as a single exception
                            Throw New NexusException(.Errors)
                        Else
                            r_oClaimReceipt.ClaimKey = .ClaimKey
                            r_oClaimReceipt.BaseClaimKey = .BaseClaimKey
                            r_oClaimReceipt.ClaimNumber = .ClaimNumber
                            r_oClaimReceipt.Version = .Version
                            r_oClaimReceipt.TimeStamp = .ApiTimeStamp()
                            r_oClaimReceipt.ResultingStatus = .ResultingStatus

                            If .Warnings IsNot Nothing AndAlso .Warnings.Count > 0 Then
                                For Each oClaimRecWarning As BaseClasses.BaseClaimResponseTypeWarnings In .Warnings

                                    oClainReceiptWarning.Code = oClaimRecWarning.Code
                                    oClainReceiptWarning.Description = oClaimRecWarning.Description
                                    r_oClaimReceipt.ClaimReceiptWarning.Add(oClainReceiptWarning)
                                Next

                            End If
                        End If
                    ElseIf ClaimReceiptCollection IsNot Nothing Then
                        If .Errors IsNot Nothing Then
                            ClaimReceiptCollection(0).TimeStamp = Nothing
                            'Process the error object if errors, and throw as a single exception
                            Throw New NexusException(.Errors)
                        Else
                            ClaimReceiptCollection(0).ClaimKey = .ClaimKey
                            ClaimReceiptCollection(0).BaseClaimKey = .BaseClaimKey
                            ClaimReceiptCollection(0).ClaimNumber = .ClaimNumber
                            ClaimReceiptCollection(0).Version = .Version
                            ClaimReceiptCollection(0).TimeStamp = .ApiTimeStamp()
                            ClaimReceiptCollection(0).ResultingStatus = .ResultingStatus

                            If .Warnings IsNot Nothing AndAlso .Warnings.Count > 0 Then
                                For Each oClaimRecWarning As BaseClasses.BaseClaimResponseTypeWarnings In .Warnings

                                    oClainReceiptWarning.Code = oClaimRecWarning.Code
                                    oClainReceiptWarning.Description = oClaimRecWarning.Description
                                    ClaimReceiptCollection(0).ClaimReceiptWarning.Add(oClainReceiptWarning)
                                Next

                            End If
                        End If
                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("FindInsuranceFileForClaims executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_oInsuranceFileForClaims = " & r_oClaimReceipt.Print.Replace("<br />", vbCrLf))

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
                'oSAM.Close()
                oClaimReceiptRequest = Nothing
                oClaimReceiptResponse = Nothing
            End Try

        End SyncLock
    End Sub

    ''' <summary>
    ''' Claim RIArrangementRow_Others calculate the other row from RI Table
    ''' </summary>
    ''' <param name="oRITable"></param>
    ''' <param name="oXMLDoc"></param>
    ''' <param name="RIBand"></param>
    ''' <param name="dsParticipentGridData"></param>
    ''' <remarks></remarks>
    Sub ClaimRIArrangementRow_Others(ByVal oRITable As DataTable, ByRef oXMLDoc As XmlDocument,
                        ByVal RIBand As XmlElement,
                        ByVal dsParticipentGridData As DataSet, ByVal sPlacement As String)
        Dim prefix As String = If(oRITable.TableName.Contains("Current"), "Current", If(oRITable.TableName.Contains("Original"), "Original", ""))
        If String.IsNullOrEmpty(prefix) Then Return

        Dim addedRIArrangementLineIds As New HashSet(Of String)

        For Each myRow As DataRow In oRITable.Rows
            If Not ShouldProcessRowForClaim(myRow, sPlacement, oRITable) Then Continue For
            Dim ArrangementRow As XmlElement = CreateArrangementRow(oXMLDoc, oRITable, myRow)
            AddBrokerParticipants(oXMLDoc, ArrangementRow, myRow, dsParticipentGridData, prefix)
            AddFAXParticipants(oXMLDoc, ArrangementRow, myRow, dsParticipentGridData, prefix)
            RIBand.AppendChild(ArrangementRow)

        Next
    End Sub
    Private Function ShouldProcessRowForClaim(myRow As DataRow, sPlacement As String, oRITable As DataTable) As Boolean
        Dim placement As String = If(IsDBNull(myRow("Placement")), "", myRow("Placement").ToString().Trim().ToUpper())
        If sPlacement.Trim.ToUpper = "GROSS" Then Return placement = "GROSS"
        If String.IsNullOrEmpty(sPlacement.Trim) AndAlso placement = "GROSS" Then Return False

        ' When placement is empty, only add when is_obligatory is false
        If String.IsNullOrEmpty(sPlacement.Trim) Then
            Dim isObligatory As Boolean = Not IsDBNull(myRow("IsObligatory")) AndAlso Convert.ToBoolean(myRow("IsObligatory"))
            If isObligatory Then Return False
        End If

        Return IsDBNull(myRow("Placement")) OrElse (myRow("Placement").Trim.ToUpper <> "FAC XOL" And myRow("Placement").Trim.ToUpper <> "FAC PROP")
    End Function

    ''' <summary>
    ''' Validates no active instalment plan exists for the given CLR recovery transaction.
    ''' Returns True if creation is allowed (no active plan), False if blocked.
    ''' </summary>
    ''' <param name="v_iClrTransactionId">The CLR transaction ID to validate</param>
    ''' <param name="v_sBranchCode">Optional branch code</param>
    ''' <returns>Boolean indicating if instalment plan creation is allowed</returns>
    ''' <remarks></remarks>
    Public Overrides Function ValidateNoExistingInstalmentPlan(ByVal v_iClrTransactionId As Integer,
                                                               Optional ByVal v_sBranchCode As String = Nothing) As Boolean
        SyncLock oLock
            Dim oRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.ValidateRecoveryInstalmentPlanQuery
            Dim oResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.ValidateRecoveryInstalmentPlanQueryResponse
            Dim sbLogMessage As StringBuilder

            Try
                oRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.ValidateRecoveryInstalmentPlanQuery
                oResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.ValidateRecoveryInstalmentPlanQueryResponse
                sbLogMessage = New StringBuilder

                With oRequest
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

                    If v_iClrTransactionId > 0 Then
                        .ClrTransactionId = v_iClrTransactionId
                    Else
                        Throw New ArgumentNullException("ClrTransactionId")
                    End If
                End With

                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.ValidateRecoveryInstalmentPlan, oRequest)
                    oResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.ValidateRecoveryInstalmentPlanQueryResponse)(result)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                ' Disposing the SAM's object

                With oResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("ValidateNoExistingInstalmentPlan executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iClrTransactionId = " & v_iClrTransactionId.ToString() & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output:" & vbCrLf)
                    sbLogMessage.AppendLine("CanCreate = " & oResponse.CanCreate.ToString() & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                Return oResponse.CanCreate

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oRequest = Nothing
                oResponse = Nothing
                sbLogMessage = Nothing
            End Try
        End SyncLock
    End Function
    ''' <summary>
    ''' To copy any existing risk
    ''' </summary>
    ''' <param name="r_oQoute"></param>
    ''' <param name="v_iRiskIndex"></param>
    ''' <param name="v_sCopyType"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub CopyRisk(ByRef r_oQoute As Quote, ByVal v_iRiskNumber As Integer,
                                  ByVal v_iRiskIndex As Integer,
                                  ByVal v_sCopyType As CopyRiskTypes,
                                  Optional ByVal v_sBranchCode As String = Nothing, Optional ByVal v_iRiskKey As Integer = 0)
        SyncLock oLock

            Dim oCopyRiskRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.CopyRiskCommand
            Dim oCopyRiskResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.CopyRiskCommandResponse
            Dim oRisk As New NexusProvider.Risk(r_oQoute.Risks(v_iRiskIndex).ScreenCode, r_oQoute.Risks(v_iRiskIndex).Description)
            Dim sbLogMessage As StringBuilder

            Try
                oCopyRiskRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.CopyRiskCommand
                oCopyRiskResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.CopyRiskCommandResponse
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
                    If v_iRiskKey <> 0 Then
                        .RiskKey = v_iRiskKey
                        .RiskNumber = v_iRiskNumber
                    End If

                    Select Case v_sCopyType
                        Case Enums.CopyRiskType.Comparative
                            .CopyType = Enums.CopyRiskType.Comparative
                        Case Enums.CopyRiskType.Duplicate
                            .CopyType = Enums.CopyRiskType.Duplicate
                    End Select
                End With

                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.CopyRisk, oCopyRiskRequest)
                    oCopyRiskResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.CopyRiskCommandResponse)(result)
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
                oCopyRiskRequest = Nothing
                oCopyRiskResponse = Nothing
            End Try

        End SyncLock

    End Sub

    ''' <summary>
    ''' This method is used to add cash list item against existing payment cash list.
    ''' </summary>
    ''' <param name="v_iCashListKey"></param>
    ''' <param name="v_oPaymentCashListCollection"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub CreatePaymentCashListItem(ByVal v_iCashListKey As Integer,
                                                        ByRef v_oPaymentCashListCollection As PaymentCashListItemTypeCollection,
                                                        Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock
            'deepak
            Dim oCreatePaymentCashListItemRequest As BaseClasses.CreatePaymentCashListItemCommand 'Request Type
            Dim oCreatePaymentCashListItemResponse As BaseClasses.CreatePaymentCashListItemCommandResponse 'Response Type
            Dim oPaymentCashList As BaseClasses.BasePaymentCashListItemType
            Dim sbLogMessage As StringBuilder

            Try
                oCreatePaymentCashListItemRequest = New BaseClasses.CreatePaymentCashListItemCommand
                oCreatePaymentCashListItemResponse = New BaseClasses.CreatePaymentCashListItemCommandResponse
                sbLogMessage = New StringBuilder


                With oCreatePaymentCashListItemRequest
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

                    If v_iCashListKey > 0 Then
                        .CashListKey = v_iCashListKey
                    Else
                        Throw New ArgumentNullException("CreatePaymentCashListItem.CashListKey")
                    End If


                    .PaymentItem = New List(Of BaseClasses.BasePaymentCashListItemType)
                    For iCounter As Integer = 0 To v_oPaymentCashListCollection.Count - 1

                        oPaymentCashList = New BaseClasses.BasePaymentCashListItemType

                        oPaymentCashList.TypeCode = v_oPaymentCashListCollection(iCounter).TypeCode
                        oPaymentCashList.StatusCode = v_oPaymentCashListCollection(iCounter).StatusCode
                        oPaymentCashList.AllocationStatusCode = v_oPaymentCashListCollection(iCounter).AllocationStatusCode

                        Select Case v_oPaymentCashListCollection(iCounter).MediaTypeCode
                            Case "BD", "DD", "PF", "SO"

                                oPaymentCashList.Bank = New BaseClasses.BaseBankPaymentType
                                oPaymentCashList.Bank.PayeeName = v_oPaymentCashListCollection(iCounter).BankPaymentType.PayeeName
                                oPaymentCashList.Bank.AccountCode = v_oPaymentCashListCollection(iCounter).BankPaymentType.AccountCode
                                oPaymentCashList.Bank.BranchCode = v_oPaymentCashListCollection(iCounter).BankPaymentType.BranchCode
                                oPaymentCashList.Bank.ExpiryDate = v_oPaymentCashListCollection(iCounter).BankPaymentType.ExpiryDate
                                oPaymentCashList.Bank.Reference1 = v_oPaymentCashListCollection(iCounter).BankPaymentType.Reference1
                                oPaymentCashList.Bank.Reference2 = v_oPaymentCashListCollection(iCounter).BankPaymentType.Reference2
                                oPaymentCashList.Bank.BIC = v_oPaymentCashListCollection(iCounter).BankPaymentType.BIC
                                oPaymentCashList.Bank.IBAN = v_oPaymentCashListCollection(iCounter).BankPaymentType.IBAN

                            Case "CC"
                                oPaymentCashList.CreditCard = New BaseClasses.BaseCreditCardType
                                oPaymentCashList.CreditCard.Number = v_oPaymentCashListCollection(iCounter).CreditCard.Number
                                oPaymentCashList.CreditCard.ExpiryDate = v_oPaymentCashListCollection(iCounter).CreditCard.ExpiryDate
                                oPaymentCashList.CreditCard.StartDate = v_oPaymentCashListCollection(iCounter).CreditCard.StartDate
                                oPaymentCashList.CreditCard.NameOnCreditCard = v_oPaymentCashListCollection(iCounter).CreditCard.NameOnCreditCard
                                oPaymentCashList.CreditCard.TypeCode = v_oPaymentCashListCollection(iCounter).CreditCard.TypeCode
                                oPaymentCashList.CreditCard.Issue = v_oPaymentCashListCollection(iCounter).CreditCard.Issue
                                oPaymentCashList.CreditCard.Pin = v_oPaymentCashListCollection(iCounter).CreditCard.Pin
                                oPaymentCashList.CreditCard.AuthCode = v_oPaymentCashListCollection(iCounter).CreditCard.AuthCode
                                oPaymentCashList.CreditCard.ManualAuthCode = v_oPaymentCashListCollection(iCounter).CreditCard.ManualAuthCode
                                oPaymentCashList.CreditCard.TransactionCode = v_oPaymentCashListCollection(iCounter).CreditCard.TransactionCode
                                oPaymentCashList.CreditCard.CustomerPresent = v_oPaymentCashListCollection(iCounter).CreditCard.CustomerPresent

                                oPaymentCashList.CreditCard.CardHolder = New BaseClasses.BaseCreditCardTypeCardHolder
                                oPaymentCashList.CreditCard.CardHolder.AddressLine1 = v_oPaymentCashListCollection(iCounter).CreditCard.CardHolder.Address1
                                oPaymentCashList.CreditCard.CardHolder.AddressLine2 = v_oPaymentCashListCollection(iCounter).CreditCard.CardHolder.Address2
                                oPaymentCashList.CreditCard.CardHolder.AddressLine3 = v_oPaymentCashListCollection(iCounter).CreditCard.CardHolder.Address3
                                oPaymentCashList.CreditCard.CardHolder.AddressLine4 = v_oPaymentCashListCollection(iCounter).CreditCard.CardHolder.Address4
                                oPaymentCashList.CreditCard.CardHolder.CountryCode = v_oPaymentCashListCollection(iCounter).CreditCard.CardHolder.CountryCode
                                oPaymentCashList.CreditCard.CardHolder.PostCode = v_oPaymentCashListCollection(iCounter).CreditCard.CardHolder.PostCode

                        End Select

                        oPaymentCashList.IsProduceDocument = v_oPaymentCashListCollection(iCounter).IsProduceDocument
                        oPaymentCashList.BankReference = v_oPaymentCashListCollection(iCounter).BankReference
                        oPaymentCashList.MediaTypeCode = v_oPaymentCashListCollection(iCounter).MediaTypeCode
                        oPaymentCashList.TransactionDate = v_oPaymentCashListCollection(iCounter).TransactionDate
                        oPaymentCashList.AccountShortCode = v_oPaymentCashListCollection(iCounter).AccountShortCode
                        oPaymentCashList.Amount = v_oPaymentCashListCollection(iCounter).Amount
                        oPaymentCashList.AllocationStatusCode = v_oPaymentCashListCollection(iCounter).AllocationStatusCode
                        oPaymentCashList.MediaReference = v_oPaymentCashListCollection(iCounter).MediaReference
                        oPaymentCashList.OurReference = v_oPaymentCashListCollection(iCounter).OurReference
                        oPaymentCashList.TheirReference = v_oPaymentCashListCollection(iCounter).TheirReference
                        oPaymentCashList.ContactName = v_oPaymentCashListCollection(iCounter).ContactName
                        oPaymentCashList.FurtherDetails = v_oPaymentCashListCollection(iCounter).FurtherDetails


                        oPaymentCashList.ContactAddress = New BaseClasses.BaseSimpleAddressType
                        oPaymentCashList.ContactAddress.AddressLine1 = v_oPaymentCashListCollection(iCounter).ContactAddress.Address1
                        oPaymentCashList.ContactAddress.AddressLine2 = v_oPaymentCashListCollection(iCounter).ContactAddress.Address2
                        oPaymentCashList.ContactAddress.AddressLine3 = v_oPaymentCashListCollection(iCounter).ContactAddress.Address3
                        oPaymentCashList.ContactAddress.AddressLine4 = v_oPaymentCashListCollection(iCounter).ContactAddress.Address4
                        oPaymentCashList.ContactAddress.PostCode = v_oPaymentCashListCollection(iCounter).ContactAddress.PostCode
                        oPaymentCashList.ContactAddress.CountryCode = v_oPaymentCashListCollection(iCounter).ContactAddress.CountryCode
                        .PaymentItem.Add(oPaymentCashList)
                    Next

                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.CreatePaymentCashListItem, oCreatePaymentCashListItemRequest)
                    oCreatePaymentCashListItemResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of BaseClasses.CreatePaymentCashListItemCommandResponse)(result)
                End Using


                ' Disposing the SAM's object


                With oCreatePaymentCashListItemResponse

                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If

                    ' v_oPaymentCashListCollection(0).CashListItemKey = .CashListItemKey
                    ' v_oPaymentCashListCollection(0).TransDetailKey = .TransDetailKey
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("CreatePaymentCashListItem executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If
                    sbLogMessage.AppendLine("Output : " & vbCrLf)
                    sbLogMessage.AppendLine(v_oPaymentCashListCollection.Print().Replace("<br />", vbCrLf))
                    LogMessageEntry(sbLogMessage)
                End If

            Catch ex As Exception
                Throw
            Finally
                oCreatePaymentCashListItemRequest = Nothing
                oCreatePaymentCashListItemResponse = Nothing
            End Try

        End SyncLock
    End Sub

    ''' <summary>
    ''' This method is used to add cash list item against existing receipt cash list.
    ''' </summary>
    ''' <param name="r_oReceiptCashList"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function CreateReceiptCashListItem(ByRef r_oReceiptCashList As PaymentCashListItemType,
                                                        Optional ByVal v_sBranchCode As String = Nothing) As PaymentCashListItemType

        SyncLock oLock
            'Dim oSAM As PureClaimServiceClient 'SAMForInsuranceV2's Object
            'Dim oCreateReceiptcashListItemRequest As CreateReceiptCashListItemRequestType 'Request Type
            'Dim oCreateReceiptcashListItemResponse As CreateReceiptCashListItemResponseType 'Response Type
            'Dim r_oReceiptCashListCollection As PaymentCashListItemTypeCollection
            'Dim oReceiptCashList(r_oReceiptCashListCollection.Count) As BaseReceiptCashListItemType
            'Dim r_oCreditCollection As CreditCardCollection
            'Dim oCredit(r_oCreditCollection.Count) As BaseCreditCardType
            'Dim r_oCardHolder As CardHolder
            'Dim r_BankReceipt As BankReceiptType
            'Dim oPolicyCollection As PolicyCollection
            'Dim oReceipt As ReceiptCashList
            'Dim sbLogMessage As StringBuilder

            'Try
            '    oSAM = InitializeClaimServiceMethod()
            '    oCreateReceiptcashListItemRequest = New CreateReceiptCashListItemRequestType
            '    oCreateReceiptcashListItemResponse = New CreateReceiptCashListItemResponseType
            '    sbLogMessage = New StringBuilder


            '    With oCreateReceiptcashListItemRequest
            '        .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
            '        .WCFSecurityToken = SecurityToken()
            '        'if the passed parameter v_sBranchCode is empty
            '        If String.IsNullOrEmpty(v_sBranchCode) Then
            '            'if the branch code is NOT in session 
            '            If String.IsNullOrEmpty(sBranchCode) Then
            '                'Use the default branch code
            '                .BranchCode = sDefaultBranchCode
            '            Else
            '                'Use the branch code in session 
            '                .BranchCode = sBranchCode
            '            End If
            '        Else
            '            'use the passed parameter v_sBranchCode
            '            .BranchCode = v_sBranchCode
            '        End If

            '        .CashListKey = r_oReceiptCashList.CashListKey

            '        r_oReceiptCashListCollection = New PaymentCashListItemTypeCollection

            '.ReceiptCashListItem = New List(Of BaseReceiptCashListItemType)

            '        For iCounter As Integer = 0 To r_oReceiptCashListCollection.Count - 1

            '            .ReceiptCashListItem(0).TypeCode = r_oReceiptCashListCollection(iCounter).TypeCode
            '            .ReceiptCashListItem(0).StatusCode = r_oReceiptCashListCollection(iCounter).StatusCode

            '            'Credit Card

            '            r_oCreditCollection = New CreditCardCollection

            '            For j As Integer = 0 To r_oCreditCollection.Count - 1
            '                .ReceiptCashListItem(j).CreditCard.Number = r_oCreditCollection(j).Number
            '                .ReceiptCashListItem(j).CreditCard.ExpiryDate = r_oCreditCollection(j).ExpiryDate
            '                .ReceiptCashListItem(j).CreditCard.StartDate = r_oCreditCollection(j).StartDate
            '                .ReceiptCashListItem(j).CreditCard.NameOnCreditCard = r_oCreditCollection(j).NameOnCreditCard
            '                .ReceiptCashListItem(j).CreditCard.TypeCode = r_oCreditCollection(j).TypeCode
            '                .ReceiptCashListItem(j).CreditCard.Issue = r_oCreditCollection(j).Issue
            '                .ReceiptCashListItem(j).CreditCard.Pin = r_oCreditCollection(j).Pin
            '                .ReceiptCashListItem(j).CreditCard.AuthCode = r_oCreditCollection(j).AuthCode
            '                .ReceiptCashListItem(j).CreditCard.CustomerPresent = r_oCreditCollection(j).CustomerPresent
            '                .ReceiptCashListItem(j).CreditCard.ManualAuthCode = r_oCreditCollection(j).ManualAuthCode
            '                .ReceiptCashListItem(j).CreditCard.TransactionCode = r_oCreditCollection(j).TransactionCode

            '                'CardHolder

            '                r_oCardHolder = New CardHolder
            '                .ReceiptCashListItem(j).CreditCard.CardHolder.Name = r_oCardHolder.Name
            '                .ReceiptCashListItem(j).CreditCard.CardHolder.AddressLine1 = r_oCardHolder.Address1
            '                .ReceiptCashListItem(j).CreditCard.CardHolder.AddressLine2 = r_oCardHolder.Address2
            '                .ReceiptCashListItem(j).CreditCard.CardHolder.AddressLine3 = r_oCardHolder.Address3
            '                .ReceiptCashListItem(j).CreditCard.CardHolder.AddressLine4 = r_oCardHolder.Address4
            '                .ReceiptCashListItem(j).CreditCard.CardHolder.CountryCode = r_oCardHolder.CountryCode
            '                .ReceiptCashListItem(j).CreditCard.CardHolder.PostCode = r_oCardHolder.PostCode

            '                'BankReceipt

            '                r_BankReceipt = New BankReceiptType
            '                .ReceiptCashListItem(j).Bank.BankCode = r_BankReceipt.BankCode
            '                .ReceiptCashListItem(j).Bank.ChequeDate = r_BankReceipt.ChequeDate
            '                .ReceiptCashListItem(j).Bank.PayerName = r_BankReceipt.PayerName
            '            Next


            '            oPolicyCollection = New PolicyCollection

            'For k As Integer = 0 To oPolicyCollection.Count - 1
            '    .ReceiptCashListItem(k).Policies = New List(Of BaseReceiptCashListItemTypePolicies)
            '    .ReceiptCashListItem(k).Policies(k).InsuranceFileKey = oPolicyCollection(k).InsuranceFileKey
            '    .ReceiptCashListItem(k).Policies(k).DocumentRef = oPolicyCollection(k).DocumentRef
            '    .ReceiptCashListItem(k).Policies(k).WriteOffReasonKey = oPolicyCollection(k).WriteOffReasonKey
            '    .ReceiptCashListItem(k).Policies(k).WriteOffAmount = oPolicyCollection(k).WriteOffAmount
            '    .ReceiptCashListItem(k).Policies(k).IsCurrencyWriteOff = oPolicyCollection(k).IsCurrencyWriteOff
            '    .ReceiptCashListItem(k).Policies(k).AmountTobeAllocated = oPolicyCollection(k).AmountTobeAllocated
            '    .ReceiptCashListItem(k).Policies(k).BGKey = oPolicyCollection(k).BGKey
            'Next

            '        Next
            '    End With


            '    'Calling the SAM Method with Request Type
            '    'add trace to allow us to debug slow SAM calls
            '    Using trace As New Tracer(Category.Trace)
            '        oCreateReceiptcashListItemResponse = oSAM.CreateReceiptCashListItem(oCreateReceiptcashListItemRequest)
            '    End Using


            '    ' Disposing the SAM's object

            '    With oCreateReceiptcashListItemResponse
            '        If .Errors IsNot Nothing Then
            '            'Process the error object if errors, and throw as a single exception CashListItem
            '            Throw New NexusException(.Errors)
            '        End If

            'If .CashListItem IsNot Nothing AndAlso .CashListItem.Count > 0 Then
            '    For Each oReceiptItem As BaseCreateReceiptCashListItemResponseTypeCashListItem In .CashListItem
            '        oReceipt = New ReceiptCashList
            '        oReceipt.CashListKey = oReceiptItem.CashListItemKey
            '        oReceipt.TransDetailsKey = oReceiptItem.TransDetailKey
            '        oReceipt.InsuranceFileKey = oReceiptItem.InsuranceFileKey
            '        oReceipt.AllocationStatus = oReceiptItem.AllocationStatus
            '    Next
            'End If

            '    End With
            '    If Logger.IsLoggingEnabled Then
            '        sbLogMessage.AppendLine("CreateReceiptCashListWithItems executed ok" & vbCrLf)
            '        sbLogMessage.AppendLine("Input:" & vbCrLf)

            '        If Not IsNothing(v_sBranchCode) Then
            '            sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
            '        Else
            '            sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
            '        End If
            '        sbLogMessage.AppendLine("Output : " & vbCrLf)
            '        sbLogMessage.AppendLine(r_oReceiptCashList.Print().Replace("<br />", vbCrLf))

            '        LogMessageEntry(sbLogMessage)
            '    End If

            '    'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

            '    'FaultErrorHandler(ex) ' handling fault error messages 

            'Catch ex As Exception
            '    Throw
            'Finally
            '    oSAM.Close()
            '    oCreateReceiptcashListItemRequest = Nothing
            '    oCreateReceiptcashListItemResponse = Nothing
            'End Try

            'Return r_oReceiptCashList
        End SyncLock
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_oCaseSearchCriteria"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function FindCase(ByVal v_oCaseSearchCriteria As CaseDetails,
                                        Optional ByVal v_sBranchCode As String = Nothing) As CaseCollection
        SyncLock oLock
            Dim oFindCaseRequest As BaseClasses.FindCaseCommand ' Request Type
            Dim oFindCaseResponse As BaseClasses.FindCaseCommandResponse    ' Response Type
            Dim sbLogMessage As StringBuilder
            Dim oListOfCase As CaseCollection = Nothing
            Try
                oFindCaseRequest = New BaseClasses.FindCaseCommand
                oFindCaseResponse = New BaseClasses.FindCaseCommandResponse
                With oFindCaseRequest
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

                    .CaseNumber = v_oCaseSearchCriteria.CaseNumber
                    If v_oCaseSearchCriteria.CaseOpenDate <> Date.MinValue Then
                        .CaseOpenDate = v_oCaseSearchCriteria.CaseOpenDate
                        .CaseOpenDateSpecified = True
                    End If

                    .ClaimNumber = v_oCaseSearchCriteria.ClaimNumber
                    .ProgressStatusCode = v_oCaseSearchCriteria.ProgressStatusCode
                    .RiskIndex = v_oCaseSearchCriteria.RiskIndex
                    .RiskType = v_oCaseSearchCriteria.RiskType

                    If v_oCaseSearchCriteria.MaxRowsToFetch > 0 Then
                        .MaxRowsToFetch = v_oCaseSearchCriteria.MaxRowsToFetch
                        .MaxRowsToFetchSpecified = True
                    End If
                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.FindCase, oFindCaseRequest)
                    oFindCaseResponse = ApiClient.DeserializeJson(Of BaseClasses.FindCaseCommandResponse)(result)
                End Using

                oListOfCase = New CaseCollection

                With oFindCaseResponse
                    Dim oCaseDetails As CaseDetails
                    If Not IsNothing(.CaseDetails) Then
                        For Each oClaimCase As BaseClasses.BaseFindCaseResponseTypeCaseDetailsRow In .CaseDetails
                            oCaseDetails = New CaseDetails

                            oCaseDetails.Analyst = oClaimCase.Analyst
                            oCaseDetails.Assistant = oClaimCase.Assistant
                            oCaseDetails.BaseCaseKey = oClaimCase.BaseCaseKey
                            oCaseDetails.CaseNumber = oClaimCase.CaseNumber
                            oCaseDetails.CaseOpenDate = oClaimCase.CaseOpenDate
                            oCaseDetails.CaseKey = oClaimCase.CaseKey
                            oCaseDetails.CaseProgressDescription = oClaimCase.CaseProgressDescription
                            oCaseDetails.TotalIndemnity = oClaimCase.TotalIndemnity
                            oCaseDetails.TotalExpense = oClaimCase.TotalExpense
                            oCaseDetails.TotalExcess = oClaimCase.TotalExcess
                            oCaseDetails.CurrencyCode = oClaimCase.CurrencyCode
                            oListOfCase.Add(oCaseDetails)
                        Next
                    End If
                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage = New StringBuilder
                    sbLogMessage.AppendLine("FindCase executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_oCaseSearchCriteria = " & v_oCaseSearchCriteria.Print.Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If Not IsNothing(oListOfCase) Then
                        sbLogMessage.AppendLine("Returned " & oListOfCase.Count.ToString & " results" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Returned 0 results" & vbCrLf)
                    End If
                    LogMessageEntry(sbLogMessage)
                End If
            Catch ex As Exception
                Throw
            Finally
                oFindCaseRequest = Nothing
                oFindCaseResponse = Nothing
                sbLogMessage = Nothing
            End Try
            Return oListOfCase
        End SyncLock
    End Function


    Public Overrides Function FindClaim(ByVal v_oClaimSearchCriteria As ClaimSearchCriteria,
                                        Optional ByVal v_sBranchCode As String = Nothing) As ClaimCollection
        SyncLock oLock

            Dim oFindClaimRequest As BaseClasses.FindClaimQuery
            Dim oFindClaimResponse As BaseClasses.FindClaimQueryResponse
            Dim oListOfClaims As ClaimCollection
            Dim oClaim As Claim
            Dim sbLogMessage As StringBuilder

            Try
                oFindClaimRequest = New BaseClasses.FindClaimQuery
                oFindClaimResponse = New BaseClasses.FindClaimQueryResponse
                oListOfClaims = New ClaimCollection
                sbLogMessage = New StringBuilder


                With oFindClaimRequest
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
                    If Not String.IsNullOrEmpty(v_oClaimSearchCriteria.ClaimNumber) Then
                        .ClaimNumber = v_oClaimSearchCriteria.ClaimNumber
                    End If
                    If Not String.IsNullOrEmpty(v_oClaimSearchCriteria.InsuranceFileRef) Then
                        .InsuranceFileRef = v_oClaimSearchCriteria.InsuranceFileRef
                    End If
                    '.InsuranceFileRef = v_oClaimSearchCriteria.InsuranceFileRef
                    If Not String.IsNullOrEmpty(v_oClaimSearchCriteria.ClientShortName) Then
                        .ClientShortName = v_oClaimSearchCriteria.ClientShortName
                    End If
                    If v_oClaimSearchCriteria.LossDateFrom <> Date.MinValue Then
                        .LossDateFrom = v_oClaimSearchCriteria.LossDateFrom
                        .LossDateFromSpecified = True
                    Else
                        .LossDateFromSpecified = False
                    End If
                    If v_oClaimSearchCriteria.LossDateTo <> Date.MinValue Then
                        .LossDateTo = v_oClaimSearchCriteria.LossDateTo
                        .LossDateToSpecified = True
                    Else
                        .LossDateToSpecified = False
                    End If
                    If Not String.IsNullOrEmpty(v_oClaimSearchCriteria.RiskIndex) Then
                        .RiskIndex = v_oClaimSearchCriteria.RiskIndex
                    End If
                    .IncludeClosedClaim = v_oClaimSearchCriteria.IncludeClosedClaim
                    .RiskKey = v_oClaimSearchCriteria.RiskKey
                    If v_oClaimSearchCriteria.CaseNumber <> "" Then
                        .CaseNumber = v_oClaimSearchCriteria.CaseNumber
                        .CaseNumberSpecified = True
                    Else
                        .CaseNumberSpecified = False
                    End If
                    'WPR08
                    If v_oClaimSearchCriteria.TPACode <> "" Then
                        .TPA = v_oClaimSearchCriteria.TPACode
                    End If
                    If v_oClaimSearchCriteria.MaxRowsToFetch > 0 Then
                        .MaxRowsToFetch = v_oClaimSearchCriteria.MaxRowsToFetch
                        .MaxRowsToFetchSpecified = True
                    Else
                        .MaxRowsToFetchSpecified = False
                    End If
                    If v_oClaimSearchCriteria.Description <> "" Then
                        .Description = v_oClaimSearchCriteria.Description
                    End If

                    Dim oAllowPolicyClientAssociations As NexusProvider.OptionTypeSetting = GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.AllowPolicyClientAssociations)
                    If oAllowPolicyClientAssociations.OptionValue = "1" Then
                        .RetrieveAssociates = True
                    Else
                        .RetrieveAssociates = False
                    End If
                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.FindClaim, oFindClaimRequest)
                    oFindClaimResponse = ApiClient.DeserializeJson(Of BaseClasses.FindClaimQueryResponse)(result)
                End Using

                oListOfClaims = New ClaimCollection()

                With oFindClaimResponse
                    If .Claims IsNot Nothing AndAlso .Claims.Count > 0 Then
                        For Each oResponseClaim As BaseClasses.BaseFindClaimResponseTypeRow In .Claims
                            oClaim = New Claim()

                            oClaim.InsuranceFileKey = oResponseClaim.InsuranceFileKey
                            oClaim.ClaimKey = oResponseClaim.ClaimKey
                            oClaim.BaseClaimKey = oResponseClaim.BaseClaimKey
                            oClaim.ClaimDescription = oResponseClaim.ClaimDescription
                            oClaim.ClaimNumber = oResponseClaim.ClaimNumber
                            oClaim.InsuranceRef = oResponseClaim.InsuranceRef
                            oClaim.ClientShortName = oResponseClaim.ClientShortName
                            oClaim.ProductDescription = oResponseClaim.ProductDescription
                            oClaim.LossDateFrom = oResponseClaim.LossDateFrom
                            oClaim.ClientName = oResponseClaim.ClientName
                            oClaim.ClaimStatusID = oResponseClaim.ClaimStatusID
                            oClaim.ClaimHandlerDescription = oResponseClaim.ClaimHandlerDescription
                            oClaim.InsurerClaimNumber = oResponseClaim.InsurerClaimNumber
                            oClaim.ClientClaimNumber = oResponseClaim.ClientClaimNumber
                            oClaim.ClientTelNo = oResponseClaim.ClientTelephoneNumber
                            oClaim.ClientTelNoOff = oResponseClaim.ClientTelephoneNumberOffice
                            oClaim.PrimaryCauseDescription = oResponseClaim.PrimaryCauseDescription
                            oClaim.SecondaryCauseDescription = oResponseClaim.SecondaryCauseDescription
                            oClaim.ProgressStatusDescription = oResponseClaim.ProgressStatusDescription
                            oClaim.Payments = oResponseClaim.Payments
                            oClaim.Reserve = oResponseClaim.Reserve
                            oClaim.CurrencyISOCode = oResponseClaim.CurrencyISOCode
                            oClaim.IsDeleted = oResponseClaim.IsDeleted
                            oClaim.IsAllowedClosedClaims = oResponseClaim.IsAllowedClosedClaims
                            oClaim.InfoOnly = oResponseClaim.InfoOnly
                            oClaim.ReportedDate = oResponseClaim.ReportedDate
                            oClaim.LastModifiedDate = oResponseClaim.LastModifiedDate

                            oClaim.CoverFrom = oResponseClaim.CoverFrom
                            oClaim.CoverTo = oResponseClaim.CoverTo
                            oClaim.LeadAgentName = oResponseClaim.LeadAgentName
                            oClaim.NotificationDate = oResponseClaim.NotificationDate
                            oClaim.CatastropheCode = oResponseClaim.CatastropheCode

                            If String.IsNullOrEmpty(oResponseClaim.CaseNumber) Then
                                oClaim.CaseNumber = ""
                            Else
                                oClaim.CaseNumber = oResponseClaim.CaseNumber
                            End If
                            oClaim.SearchResultsCol1 = oResponseClaim.SearchResultsCol1
                            oClaim.RiskDescription = oResponseClaim.RiskDescription
                            oClaim.ClaimStatus = oResponseClaim.ClaimStatus
                            oClaim.AssociatedClients = oResponseClaim.AssociatedClients
                            oClaim.ClaimRiskField = oResponseClaim.ClaimRiskField
                            oListOfClaims.Add(oClaim)
                        Next
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("FindClaim executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_oClaimSearchCriteria = " & v_oClaimSearchCriteria.Print.Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If Not IsNothing(oListOfClaims) Then
                        sbLogMessage.AppendLine("Returned " & oListOfClaims.Count.ToString & " results" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Returned 0 results" & vbCrLf)
                    End If

                    LogMessageEntry(sbLogMessage)
                End If
            Catch ex As Exception
                Throw
            Finally
                oFindClaimRequest = Nothing
                oFindClaimResponse = Nothing
            End Try


            Return oListOfClaims
        End SyncLock
    End Function

    ''' <summary>
    ''' FindInsuranceFileForClaims
    ''' </summary>
    ''' <param name="v_oInsuranceFileForClaims"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    Public Overrides Function FindInsuranceFileForClaims(ByRef v_oInsuranceFileForClaims As InsuranceFileDetails,
                                         Optional ByVal v_sBranchCode As String = Nothing) As InsuranceFileDetailsCollection

        SyncLock oLock

            Dim oFindInsuranceFileForClaimsRequest As BaseClasses.FindInsuranceFileForClaimsCommand
            Dim oFindInsuranceFileForClaimsResponse As BaseClasses.FindInsuranceFileForClaimsCommandResponse
            Dim oListOfInsuranceFileForClaims As InsuranceFileDetailsCollection
            Dim oInsuranceFileDetails As InsuranceFileDetails
            Dim sbLogMessage As StringBuilder

            Try
                oFindInsuranceFileForClaimsRequest = New BaseClasses.FindInsuranceFileForClaimsCommand
                oFindInsuranceFileForClaimsResponse = New BaseClasses.FindInsuranceFileForClaimsCommandResponse
                oListOfInsuranceFileForClaims = New InsuranceFileDetailsCollection
                sbLogMessage = New StringBuilder



                With oFindInsuranceFileForClaimsRequest
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

                    '.InsuranceFileRef = v_oInsuranceFileForClaims.InsuranceFileRef
                    If v_oInsuranceFileForClaims.SearchDate <> Date.MinValue Then
                        .SearchDate = v_oInsuranceFileForClaims.SearchDate
                    Else
                        Throw New ArgumentException("SearchDate")
                    End If
                    If v_oInsuranceFileForClaims.LossDate <> Date.MinValue Then
                        .LossDate = v_oInsuranceFileForClaims.LossDate
                    Else
                        Throw New ArgumentException("LossDate")
                    End If

                    If Not String.IsNullOrEmpty(v_oInsuranceFileForClaims.InsuranceRef) Then
                        .InsuranceRef = v_oInsuranceFileForClaims.InsuranceRef
                    End If

                    If v_oInsuranceFileForClaims.CoverNoteSheetNumber <> 0 Then
                        .CoverNoteSheetNumber = v_oInsuranceFileForClaims.CoverNoteSheetNumber
                        .CoverNoteSheetNumberSpecified = True
                    Else
                        .CoverNoteSheetNumberSpecified = False
                    End If
                    If Not String.IsNullOrEmpty(v_oInsuranceFileForClaims.RiskIndex) Then
                        .RiskIndex = v_oInsuranceFileForClaims.RiskIndex
                    End If
                    If Not String.IsNullOrEmpty(v_oInsuranceFileForClaims.PostCode) Then
                        .PostCode = v_oInsuranceFileForClaims.PostCode
                    End If

                    If v_oInsuranceFileForClaims.InForceFrom <> Date.MinValue Then
                        .InForceFrom = v_oInsuranceFileForClaims.InForceFrom
                        .InForceFromSpecified = True
                    Else
                        .InForceFromSpecified = False
                    End If
                    If v_oInsuranceFileForClaims.InForceTo <> Date.MinValue Then
                        .InForceTo = v_oInsuranceFileForClaims.InForceTo
                        .InForceToSpecified = True
                    Else
                        .InForceToSpecified = False
                    End If
                    If Not String.IsNullOrEmpty(v_oInsuranceFileForClaims.ClientShortName) Then
                        .ClientShortName = v_oInsuranceFileForClaims.ClientShortName
                    End If

                    If v_oInsuranceFileForClaims.MaxRowsToFetch > 0 Then
                        .MaxRowsToFetch = v_oInsuranceFileForClaims.MaxRowsToFetch
                        .MaxRowsToFetchSpecified = True
                    Else
                        .MaxRowsToFetchSpecified = False
                    End If
                    Dim oAllowPolicyClientAssociations As NexusProvider.OptionTypeSetting = GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.AllowPolicyClientAssociations)
                    If oAllowPolicyClientAssociations.OptionValue = "1" Then
                        .RetrieveAssociates = True
                    Else
                        .RetrieveAssociates = False
                    End If

                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.FindInsuranceFileForClaims, oFindInsuranceFileForClaimsRequest)
                    oFindInsuranceFileForClaimsResponse = ApiClient.DeserializeJson(Of BaseClasses.FindInsuranceFileForClaimsCommandResponse)(result)
                End Using

                ' Disposing the SAM's object

                With oFindInsuranceFileForClaimsResponse
                    If .InsuranceFileDetails IsNot Nothing AndAlso .InsuranceFileDetails.Count > 0 Then
                        For Each oResponseInsuranceFileForClaim As BaseClasses.BaseFindInsuranceFileResponseTypeRow In .InsuranceFileDetails
                            oInsuranceFileDetails = New InsuranceFileDetails
                            oInsuranceFileDetails.InsuranceFileKey = .InsuranceFileKey
                            oInsuranceFileDetails.LeadAgentKey = oResponseInsuranceFileForClaim.LeadAgentKey
                            oInsuranceFileDetails.InsuranceFileKey = oResponseInsuranceFileForClaim.InsuranceFileKey
                            oInsuranceFileDetails.StatusID = oResponseInsuranceFileForClaim.StatusId
                            oInsuranceFileDetails.IsSourceClosed = oResponseInsuranceFileForClaim.IsSourceClosed
                            oInsuranceFileDetails.InsuranceRef = oResponseInsuranceFileForClaim.InsuranceRef
                            oInsuranceFileDetails.ProductCode = oResponseInsuranceFileForClaim.ProductCode
                            oInsuranceFileDetails.RenewalDate = FormatDateTime(oResponseInsuranceFileForClaim.RenewalDate, DateFormat.ShortDate)
                            oInsuranceFileDetails.RiskIndex = oResponseInsuranceFileForClaim.RiskIndex
                            oInsuranceFileDetails.ClientShortName = oResponseInsuranceFileForClaim.ClientShortName
                            oInsuranceFileDetails.ClientName = oResponseInsuranceFileForClaim.ClientName
                            oInsuranceFileDetails.ClientAddressLine1 = oResponseInsuranceFileForClaim.ClientAddressLine1
                            oInsuranceFileDetails.PostCode = oResponseInsuranceFileForClaim.ClientPostCode
                            oInsuranceFileDetails.InsuranceFileStatusCode = oResponseInsuranceFileForClaim.InsuranceFileStatusCode

                            oInsuranceFileDetails.CoverFrom = oResponseInsuranceFileForClaim.CoverFrom
                            oInsuranceFileDetails.CoverTo = oResponseInsuranceFileForClaim.CoverTo
                            oInsuranceFileDetails.LeadAgentName = oResponseInsuranceFileForClaim.LeadAgentName
                            oInsuranceFileDetails.InceptionDate = oResponseInsuranceFileForClaim.InceptionDate
                            oInsuranceFileDetails.InsuranceFileTypeCode = oResponseInsuranceFileForClaim.InsuranceFileTypeCode
                            oInsuranceFileDetails.LapseDate = oResponseInsuranceFileForClaim.LapseDate
                            oInsuranceFileDetails.AssociatedClients = oResponseInsuranceFileForClaim.AssociatedClients
                            oInsuranceFileDetails.AllowedClosedBranchClaims = oResponseInsuranceFileForClaim.AllowedClosedBranchClaims
                            oInsuranceFileDetails.FileCode = oResponseInsuranceFileForClaim.FileCode
                            oInsuranceFileDetails.DOBirth = oResponseInsuranceFileForClaim.DOB
                            oListOfInsuranceFileForClaims.Add(oInsuranceFileDetails)
                        Next
                    End If
                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("FindInsuranceFileForClaims executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_oInsuranceFileForClaims = " & v_oInsuranceFileForClaims.Print.Replace("<br />", vbCrLf))

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    If Not IsNothing(oListOfInsuranceFileForClaims) Then
                        sbLogMessage.AppendLine("Returned " & oListOfInsuranceFileForClaims.Count.ToString & " results" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("Returned 0 results" & vbCrLf)
                    End If
                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oFindInsuranceFileForClaimsRequest = Nothing
                oFindInsuranceFileForClaimsResponse = Nothing
            End Try


            Return oListOfInsuranceFileForClaims
        End SyncLock
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_oClaimsDocument"></param>
    ''' <param name="v_oDocumentType"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GenerateClaimsDocuments(ByVal v_oClaimsDocument As ClaimDocument,
                                         ByVal v_oDocumentType As DocumentType,
                                         Optional ByVal v_sBranchCode As String = Nothing) As ClaimDocumentCollection
        SyncLock oLock

            Dim oClaimsDocumentsRequest As BaseClasses.GenerateClaimsDocumentsCommand
            Dim oClaimsDocumentsResponse As BaseClasses.GenerateClaimsDocumentsCommandResponse
            Dim oListOfClaimsDocument As ClaimDocumentCollection
            Dim oClaimDocument As ClaimDocument
            Dim sbLogMessage As StringBuilder

            Try
                oClaimsDocumentsRequest = New BaseClasses.GenerateClaimsDocumentsCommand
                oClaimsDocumentsResponse = New BaseClasses.GenerateClaimsDocumentsCommandResponse
                sbLogMessage = New StringBuilder


                With oClaimsDocumentsRequest
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

                    If v_oClaimsDocument.ClaimKey > 0 Then
                        .ClaimKey = v_oClaimsDocument.ClaimKey
                    Else
                        Throw New ArgumentException("ClaimKey")
                    End If

                    If v_oClaimsDocument.Mode > 0 Then
                        .Mode = v_oClaimsDocument.Mode
                    Else
                        Throw New ArgumentException("Mode")
                    End If

                    .TransactionType = v_oClaimsDocument.TransactionType
                    .ParameterXML = v_oClaimsDocument.ParameterXML

                    Select Case v_oDocumentType
                        Case DocumentType.None
                            Throw New ArgumentException("Can not be DocumentType.None", "DocumentType")
                        Case DocumentType.HTML
                            .OutputAsHTML = True
                            .OutputAsPDF = False
                        Case DocumentType.PDF
                            .OutputAsHTML = True 'changes made to run the PDF document
                            .OutputAsPDF = True
                    End Select

                End With

                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Post(ApiMethods.GenerateClaimsDocuments, oClaimsDocumentsRequest)
                    oClaimsDocumentsResponse = ApiClient.DeserializeJson(Of BaseClasses.GenerateClaimsDocumentsCommandResponse)(result)
                End Using


                ' Disposing the SAM's object

                oListOfClaimsDocument = New ClaimDocumentCollection()

                With oClaimsDocumentsResponse
                    If .Documents IsNot Nothing AndAlso .Documents.Count > 0 Then
                        For Each oResponseClaimDocument As BaseClasses.BaseGenerateClaimsDocumentsResponseTypeRow In .Documents
                            oClaimDocument = New ClaimDocument()
                            oClaimDocument.DocumentName = oResponseClaimDocument.DocumentName
                            oClaimDocument.DocumentDescription = oResponseClaimDocument.DocumentDescription
                            oListOfClaimsDocument.Add(oClaimDocument)
                        Next
                    End If
                End With

                'Logging
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GenerateClaimsDocuments executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iBaseClaimKey = " & v_oClaimsDocument.ToString() & vbCrLf)

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
                oClaimsDocumentsRequest = Nothing
                oClaimsDocumentsResponse = Nothing
            End Try


            Return oListOfClaimsDocument

        End SyncLock

    End Function

#Region "GetCashClaimLink"
    ''' <summary>
    ''' To GetCashClaimLink.
    ''' </summary>
    ''' <param name="v_iClaimPaymentKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetCashClaimLink(ByVal v_iClaimPaymentKey As Integer,
                                                        Optional ByVal v_sBranchCode As String = Nothing) As CashClaimLink

        SyncLock oLock
            Dim oGetCashClaimLinkRequest As BaseClasses.GetCashClaimLinkQuery = Nothing
            Dim oGetCashClaimLinkResponse As BaseClasses.GetCashClaimLinkQueryResponse = Nothing 'Response Type
            Dim sbLogMessage As StringBuilder = Nothing
            Dim oCashClaimLink As CashClaimLink = Nothing
            Try
                oGetCashClaimLinkRequest = New BaseClasses.GetCashClaimLinkQuery
                oGetCashClaimLinkResponse = New BaseClasses.GetCashClaimLinkQueryResponse

                With oGetCashClaimLinkRequest
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
                    .ClaimPaymentKey = v_iClaimPaymentKey
                End With
                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetCashClaimLink, oGetCashClaimLinkRequest)
                    oGetCashClaimLinkResponse = ApiClient.DeserializeJson(Of BaseClasses.GetCashClaimLinkQueryResponse)(result)
                End Using
                oCashClaimLink = New CashClaimLink

                With oGetCashClaimLinkResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If
                    oCashClaimLink.Amount = .Amount
                    oCashClaimLink.BranchCode = .BranchCode
                    oCashClaimLink.CashListItemKey = .CashListItemKey
                    oCashClaimLink.CashListKey = .CashListKey
                    oCashClaimLink.CurrencyCode = .CurrencyCode
                    oCashClaimLink.MediaTypeCode = .MediaTypeCode
                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage = New StringBuilder
                    sbLogMessage.AppendLine("GetCashClaimLink executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)

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
                oGetCashClaimLinkRequest = Nothing
                oGetCashClaimLinkResponse = Nothing
                sbLogMessage = Nothing
            End Try
            Return oCashClaimLink
        End SyncLock
    End Function
#End Region

#Region "GetTaxTypesAndBands"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_iTaxGroupId"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    Public Overrides Function GetTaxTypesAndBands(ByVal v_iTaxGroupId As Integer,
                                                        Optional ByVal v_sBranchCode As String = Nothing) As TaxTypesAndBandsCollection
        SyncLock oLock
            Dim oGetTaxTypesAndBandsRequest As BaseClasses.GetTaxTypesAndBandsQuery = Nothing
            Dim oGetTaxTypesAndBandsResponse As BaseClasses.GetTaxTypesAndBandsQueryResponse = Nothing
            Dim sbLogMessage As StringBuilder = Nothing
            Dim oTaxTypesAndBands As TaxTypesAndBands = Nothing
            Dim oTaxTypesAndBandsCollection As TaxTypesAndBandsCollection = Nothing

            Try
                oGetTaxTypesAndBandsRequest = New BaseClasses.GetTaxTypesAndBandsQuery
                oGetTaxTypesAndBandsResponse = New BaseClasses.GetTaxTypesAndBandsQueryResponse
                oTaxTypesAndBandsCollection = New TaxTypesAndBandsCollection

                With oGetTaxTypesAndBandsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    If String.IsNullOrEmpty(v_sBranchCode) Then

                        If String.IsNullOrEmpty(sBranchCode) Then
                            .BranchCode = sDefaultBranchCode
                        Else
                            .BranchCode = sBranchCode
                        End If
                    Else
                        .BranchCode = v_sBranchCode
                    End If

                    .TaxGroupId = v_iTaxGroupId
                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetTaxTypesAndBands, oGetTaxTypesAndBandsRequest)
                    oGetTaxTypesAndBandsResponse = ApiClient.DeserializeJson(Of BaseClasses.GetTaxTypesAndBandsQueryResponse)(result)
                End Using


                With oGetTaxTypesAndBandsResponse
                    If .Taxes IsNot Nothing AndAlso .Taxes.Count > 0 Then

                        For Each oTaxTypesAndBandsRow As BaseClasses.BaseGetTaxTypesAndBandsResponseRow In .Taxes()

                            oTaxTypesAndBands = New TaxTypesAndBands()

                            With oTaxTypesAndBands
                                .TaxTypeId = oTaxTypesAndBandsRow.TaxTypeId
                                .TaxTypeDescription = oTaxTypesAndBandsRow.TaxTypeDescription
                                .TaxTypeCode = oTaxTypesAndBandsRow.TaxTypeCode
                                .TaxBandId = oTaxTypesAndBandsRow.TaxBandId
                                .TaxBandDescription = oTaxTypesAndBandsRow.TaxBandDescription
                                .IsValue = oTaxTypesAndBandsRow.IsValue
                                .Rate = oTaxTypesAndBandsRow.Rate
                                .CurrencyId = oTaxTypesAndBandsRow.CurrencyId
                                .Sequence = oTaxTypesAndBandsRow.Sequence
                                .AllowTaxCredit = oTaxTypesAndBandsRow.AllowTaxCredit
                                .TaxBandRateId = oTaxTypesAndBandsRow.TaxBandRateId

                            End With

                            oTaxTypesAndBandsCollection.Add(oTaxTypesAndBands)
                        Next
                    End If
                End With

            Catch ex As Exception
                Throw
            Finally
                oGetTaxTypesAndBandsRequest = Nothing
                oGetTaxTypesAndBandsResponse = Nothing
                sbLogMessage = Nothing
            End Try

            Return oTaxTypesAndBandsCollection

        End SyncLock
    End Function
#End Region

    Public Overrides Function GetClaimCoinsurer(ByVal v_sClaimKey As String,
                                         Optional ByVal v_sBranchCode As String = Nothing) As Claim
        SyncLock oLock
            Dim oGetClaimCoinsurerRequest As BaseClasses.GetClaimCoinsurerQuery
            Dim oGetClaimCoinsurerResponse As BaseClasses.GetClaimCoinsurerQueryResponse
            Dim oClaim As Claim
            Dim oClaimCoInsurer As ClaimCoInsurer
            Try
                oGetClaimCoinsurerRequest = New BaseClasses.GetClaimCoinsurerQuery
                oGetClaimCoinsurerResponse = New BaseClasses.GetClaimCoinsurerQueryResponse



                With oGetClaimCoinsurerRequest
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
                    'use the passed parameter v_sClaimKey
                    If v_sClaimKey > 0 Then
                        .ClaimKey = v_sClaimKey
                    Else
                        Throw New ArgumentException("Claim Key")
                    End If
                End With


                oClaim = New Claim
                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetClaimCoinsurer, oGetClaimCoinsurerRequest)
                    oGetClaimCoinsurerResponse = ApiClient.DeserializeJson(Of BaseClasses.GetClaimCoinsurerQueryResponse)(result)
                End Using

                ' Disposing the SAM's object



                With oGetClaimCoinsurerResponse

                    oClaim.ClaimNumber = .ClaimNumber
                    oClaim.TotalShare = .TotalShare
                    oClaim.TotalCurrentShareValue = .TotalCurrentShareValue

                    If .Coinsurers IsNot Nothing AndAlso .Coinsurers.Count > 0 Then
                        oClaim.ClaimCoInsurer = New ClaimCoInsurerCollection
                        For Each oReponseCoInsurerClaim As BaseClasses.BaseGetClaimCoinsurerResponseTypeRow In .Coinsurers
                            oClaimCoInsurer = New ClaimCoInsurer
                            oClaimCoInsurer.Name = oReponseCoInsurerClaim.Name
                            oClaimCoInsurer.PartyKey = oReponseCoInsurerClaim.PartyKey
                            oClaimCoInsurer.Share = oReponseCoInsurerClaim.Share
                            oClaimCoInsurer.ShareValue = oReponseCoInsurerClaim.ShareValue

                            oClaim.ClaimCoInsurer.Add(oClaimCoInsurer)
                        Next
                    End If
                End With
                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGetClaimCoinsurerRequest = Nothing
                oGetClaimCoinsurerResponse = Nothing
                oClaimCoInsurer = Nothing

            End Try
            Return oClaim

        End SyncLock
    End Function

    ''' <summary>
    ''' GetClaimDetails
    ''' </summary>
    ''' <param name="v_iClaimKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="v_iFetchAllVersionAmounts"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetClaimDetails(ByVal v_iClaimKey As Integer,
                                           Optional ByVal v_sBranchCode As String = Nothing,
                                            Optional ByVal v_iFetchAllVersionAmounts As Integer = 0,
                                         Optional ByVal bExclusiveLock As Boolean = False) As ClaimDetails

        SyncLock oLock
            Dim oGetClaimDetailsRequest As BaseClasses.GetClaimDetailsQuery
            Dim oGetClaimDetailsResponse As BaseClasses.GetClaimDetailsQueryResponse
            Dim oClaim As ClaimDetails
            Dim oClaimPerils As PerilCollection = Nothing
            Dim oPeril As PerilSummary = Nothing
            Dim oContactForInsurer As New Contact()
            Dim oClaimPayments As ClaimPaymentCollection = Nothing
            Dim oClaimPayment As ClaimPayment = Nothing
            Dim oClaimPaymentItems As ClaimPaymentItemCollection = Nothing
            Dim oClaimReceipts As ClaimReceiptCollection = Nothing
            Dim oRecoveries As RecoveryCollection = Nothing
            Dim oReserves As ReserveCollection = Nothing
            Dim oPaymentItem As ClaimPaymentItem = Nothing
            Dim oClaimReceipt As ClaimReceipt = Nothing
            Dim oClaimReceiptItems = New ClaimReceiptItemTypeCollection
            Dim oReceiptItem As ClaimReceiptItemType = Nothing
            Dim oSalvageRecoveries As PerilRecoveryCollection
            Dim oTPRecoveries As PerilRecoveryCollection
            Dim oSalvageRecovery As PerilRecovery = Nothing
            Dim oTPRecovery As PerilRecovery = Nothing
            Dim oReserve As Reserve = Nothing
            Try
                oGetClaimDetailsRequest = New BaseClasses.GetClaimDetailsQuery
                oGetClaimDetailsResponse = New BaseClasses.GetClaimDetailsQueryResponse

                With oGetClaimDetailsRequest
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
                    'use the passed parameter v_sClaimKey
                    If v_iClaimKey > 0 Then
                        .ClaimKey = v_iClaimKey
                    Else
                        Throw New ArgumentException("Claim Key")
                    End If
                    .FetchAllVersionAmounts = v_iFetchAllVersionAmounts
                    'Set SessionID in every call
                    .SessionValue = HttpContext.Current.Session.SessionID.ToString()

                    'Set ExclusinLock Value
                    .ExclusiveLock = bExclusiveLock
                End With

                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetClaimDetails, oGetClaimDetailsRequest)
                    oGetClaimDetailsResponse = ApiClient.DeserializeJson(Of BaseClasses.GetClaimDetailsQueryResponse)(result)
                End Using
                ' Disposing the SAM's object
                With oGetClaimDetailsResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        oClaim = New ClaimDetails()
                        oClaimPerils = New PerilCollection()

                        If .ClaimDetails IsNot Nothing Then
                            oClaim.ClaimKey = .ClaimDetails.ClaimDetails.ClaimKey
                            oClaim.BaseClaimKey = .ClaimDetails.ClaimDetails.BaseClaimKey
                            oClaim.BaseCaseKey = .ClaimDetails.ClaimDetails.BaseCaseKey
                            oClaim.InsuranceFileKey = .ClaimDetails.ClaimDetails.InsuranceFileKey
                            oClaim.InsuranceFolderKey = .ClaimDetails.ClaimDetails.InsuranceFolderKey
                            oClaim.RiskKey = .ClaimDetails.ClaimDetails.RiskKey
                            oClaim.CurrencyCode = .ClaimDetails.ClaimDetails.CurrencyCode
                            oClaim.CurrencyISOCode = .ClaimDetails.ClaimDetails.CurrencyCode
                            oClaim.UnderwritingYearCode = .ClaimDetails.ClaimDetails.UnderwritingYearCode
                            oClaim.GisScreenCode = .ClaimDetails.ClaimDetails.GisScreenCode
                            oClaim.ClaimNumber = .ClaimDetails.ClaimDetails.ClaimNumber
                            oClaim.TPA = .ClaimDetails.ClaimDetails.TPA 'WPR08
                            oClaim.CatastropheCode = .ClaimDetails.ClaimDetails.CatastropheCode
                            oClaim.CoinsuranceTreatmentCode = .ClaimDetails.ClaimDetails.CoinsuranceTreatmentCode
                            oClaim.ClaimNumber = .ClaimDetails.ClaimDetails.ClaimNumber
                            oClaim.ClaimStatus = .ClaimDetails.ClaimDetails.ClaimStatus
                            oClaim.ClaimStatusDate = .ClaimDetails.ClaimDetails.ClaimStatusDate
                            oClaim.ClaimVersion = .ClaimDetails.ClaimDetails.ClaimVersion
                            oClaim.ClaimVersionDescription = .ClaimDetails.ClaimDetails.ClaimVersionDescription
                            oClaim.ClientEmail = .ClaimDetails.ClaimDetails.ClientEmail
                            oClaim.ClientFaxNo = .ClaimDetails.ClaimDetails.ClientFaxNo
                            oClaim.ClientMobileNo = .ClaimDetails.ClaimDetails.ClientMobileNo
                            oClaim.ClientTelNo = .ClaimDetails.ClaimDetails.ClientTelNo
                            oClaim.ClientTelNoOff = .ClaimDetails.ClaimDetails.ClientTelNoOff
                            oClaim.Comments = .ClaimDetails.ClaimDetails.Comments
                            oClaim.Description = .ClaimDetails.ClaimDetails.Description
                            oClaim.HandlerCode = .ClaimDetails.ClaimDetails.HandlerCode
                            oClaim.InfoOnly = .ClaimDetails.ClaimDetails.InfoOnly
                            oClaim.LastModifiedDate = .ClaimDetails.ClaimDetails.LastModifiedDate
                            oClaim.LikelyClaim = .ClaimDetails.ClaimDetails.LikelyClaim
                            oClaim.Location = .ClaimDetails.ClaimDetails.Location
                            oClaim.LossFromDate = .ClaimDetails.ClaimDetails.LossFromDate
                            oClaim.LossToDate = .ClaimDetails.ClaimDetails.LossToDate
                            oClaim.LossToDateSpecified = .ClaimDetails.ClaimDetails.LossToDateSpecified
                            oClaim.PrimaryCauseCode = .ClaimDetails.ClaimDetails.PrimaryCauseCode
                            oClaim.ProgressStatusCode = .ClaimDetails.ClaimDetails.ProgressStatusCode
                            oClaim.ReportedDate = .ClaimDetails.ClaimDetails.ReportedDate
                            oClaim.SecondaryCauseCode = .ClaimDetails.ClaimDetails.SecondaryCauseCode
                            oClaim.TownCode = .ClaimDetails.ClaimDetails.TownCode
                            oClaim.UnderwritingYearCode = .ClaimDetails.ClaimDetails.UnderwritingYearCode
                            oClaim.UserDefFldACode = .ClaimDetails.ClaimDetails.UserDefFldACode
                            oClaim.UserDefFldBCode = .ClaimDetails.ClaimDetails.UserDefFldBCode
                            oClaim.UserDefFldCCode = .ClaimDetails.ClaimDetails.UserDefFldCCode
                            oClaim.UserDefFldDCode = .ClaimDetails.ClaimDetails.UserDefFldDCode
                            oClaim.UserDefFldECode = .ClaimDetails.ClaimDetails.UserDefFldECode
                            oClaim.IsPolicyOutstanding = .ClaimDetails.ClaimDetails.IsPolicyOutstanding

                            oClaim.ClientShortName = .ClaimDetails.ClaimDetails.ClientShortName

                            If Not String.IsNullOrEmpty(.ClaimDetails.ClaimDetails.ClaimVersionDescription) Then
                                Dim sClaimVersionDescriptionWithoutComment As String = Mid(.ClaimDetails.ClaimDetails.ClaimVersionDescription, 1, InStr(.ClaimDetails.ClaimDetails.ClaimVersionDescription, "Comments"))
                                If sClaimVersionDescriptionWithoutComment.Contains("Recovery") Then
                                    oClaim.IsRecovery = True
                                End If
                                Dim sClaimVersionDescription As String = .ClaimDetails.ClaimDetails.ClaimVersionDescription
                                If sClaimVersionDescription.Contains("Clone") AndAlso sClaimVersionDescription.Contains("Recovery") Then
                                    oClaim.IsRecovery = True
                                End If
                            End If
                            oClaim.TimeStamp = .ApiTimeStamp
                            oClaim.Client.Address.Address1 = .ClaimDetails.ClaimDetails.Client.Address.AddressLine1
                            oClaim.Client.Address.Address2 = .ClaimDetails.ClaimDetails.Client.Address.AddressLine2
                            oClaim.Client.Address.Address3 = .ClaimDetails.ClaimDetails.Client.Address.AddressLine3
                            oClaim.Client.Address.Address4 = .ClaimDetails.ClaimDetails.Client.Address.AddressLine4
                            oClaim.Client.Address.AddressType = .ClaimDetails.ClaimDetails.Client.Address.AddressTypeCode
                            oClaim.Client.Address.CountryCode = .ClaimDetails.ClaimDetails.Client.Address.CountryCode
                            oClaim.Client.Address.PostCode = .ClaimDetails.ClaimDetails.Client.Address.PostCode
                            oClaim.Client.PartyClaimNumber = .ClaimDetails.ClaimDetails.Client.PartyClaimNumber
                            oClaim.Client.TaxRegistered = .ClaimDetails.ClaimDetails.Client.TaxRegistered
                            oClaim.Client.TaxRegistrationNumber = .ClaimDetails.ClaimDetails.Client.TaxRegistrationNumber
                            oClaim.Client.PartyKey = .ClaimDetails.ClaimDetails.Client.PartyKey
                            If Not IsNothing(.ClaimDetails.ClaimDetails.Client) Then
                                oClaim.Client.ClientEmail = .ClaimDetails.ClaimDetails.Client.PartyEmail
                                oClaim.Client.ClientFaxNo = .ClaimDetails.ClaimDetails.Client.PartyFaxNo
                                oClaim.Client.ClientMobileNo = .ClaimDetails.ClaimDetails.Client.PartyMobileNo
                                oClaim.Client.ClientTelNo = .ClaimDetails.ClaimDetails.Client.PartyTelNo
                                oClaim.Client.ClientTelNoOff = .ClaimDetails.ClaimDetails.Client.PartyTelNoOff
                            End If
                        End If
                        If .ClaimDetails.ClaimDetails.Client.Contact IsNot Nothing AndAlso .ClaimDetails.ClaimDetails.Client.Contact.Count > 0 Then
                            oClaim.Client.Contact = New ContactCollection
                            If .ClaimDetails.ClaimDetails.Client.Contact IsNot Nothing AndAlso .ClaimDetails.ClaimDetails.Client.Contact.Count > 0 Then
                                For Each oBaseContactType As BaseClasses.BaseContactType In .ClaimDetails.ClaimDetails.Client.Contact
                                    Dim oContact As New Contact()
                                    With oBaseContactType
                                        oContact.AreaCode = .AreaCode
                                        oContact.Description = .Description
                                        oContact.ContactType = .ContactTypeCode
                                        oContact.Extension = .Extension
                                        Select Case .ContactTypeCode
                                            Case Enums.ContactTypeType.EMAIL
                                                oContact.ContactDetailType = ItemChoiceTypes.EmailAddress
                                            Case Else
                                                oContact.ContactDetailType = ItemChoiceTypes.Number
                                        End Select
                                        oContact.Number = .ContactDetail.Item
                                    End With
                                    oClaim.Client.Contact.Add(oContact)
                                Next
                            End If
                        End If
                        'Added for fetching the insurer Details
                        If .ClaimDetails.ClaimDetails.Insurer IsNot Nothing Then
                            If .ClaimDetails.ClaimDetails.Insurer.Address IsNot Nothing Then
                                oClaim.Insurer.Address.Address1 = .ClaimDetails.ClaimDetails.Insurer.Address.AddressLine1
                                oClaim.Insurer.Address.Address2 = .ClaimDetails.ClaimDetails.Insurer.Address.AddressLine2
                                oClaim.Insurer.Address.Address3 = .ClaimDetails.ClaimDetails.Insurer.Address.AddressLine3
                                oClaim.Insurer.Address.Address4 = .ClaimDetails.ClaimDetails.Insurer.Address.AddressLine4
                                oClaim.Insurer.Address.AddressType = .ClaimDetails.ClaimDetails.Insurer.Address.AddressTypeCode
                                oClaim.Insurer.Address.CountryCode = .ClaimDetails.ClaimDetails.Insurer.Address.CountryCode
                                oClaim.Insurer.Address.PostCode = .ClaimDetails.ClaimDetails.Insurer.Address.PostCode
                                oClaim.Insurer.ContactName = .ClaimDetails.ClaimDetails.Insurer.ContactName
                                oClaim.Insurer.PartyClaimNumber = .ClaimDetails.ClaimDetails.Insurer.PartyClaimNumber
                                If .ClaimDetails.ClaimDetails.Insurer.Contact IsNot Nothing AndAlso .ClaimDetails.ClaimDetails.Insurer.Contact.Count > 0 Then
                                    oClaim.Insurer.Contact = New ContactCollection
                                    For Each oBaseContactType As BaseClasses.BaseContactType In .ClaimDetails.ClaimDetails.Insurer.Contact
                                        With oBaseContactType
                                            oContactForInsurer.AreaCode = .AreaCode
                                            oContactForInsurer.Description = .Description
                                            oContactForInsurer.ContactType = .ContactTypeCode
                                            oContactForInsurer.Extension = .Extension
                                            Select Case .ContactTypeCode
                                                Case Enums.ContactTypeType.EMAIL
                                                    oContactForInsurer.ContactDetailType = ItemChoiceTypes.EmailAddress
                                                Case Else
                                                    oContactForInsurer.ContactDetailType = ItemChoiceTypes.Number
                                            End Select
                                            oContactForInsurer.Number = .ContactDetail.Item
                                        End With
                                        oClaim.Insurer.Contact.Add(oContactForInsurer)
                                    Next
                                End If
                            End If
                            'oClaim.Insurer.InsurerName = .ClaimDetails.ClaimDetails.Insurer.InsurerName
                            oClaim.Insurer.InsurerContact = .ClaimDetails.ClaimDetails.Insurer.InsurerContact
                            oClaim.Insurer.InsurerEmail = .ClaimDetails.ClaimDetails.Insurer.InsurerEmail
                            oClaim.Insurer.InsurerFaxNo = .ClaimDetails.ClaimDetails.Insurer.InsurerFaxNo
                            oClaim.Insurer.InsurerTelNo = .ClaimDetails.ClaimDetails.Insurer.InsurerTelNo

                        End If

                        If .ClaimDetails.ClaimPeril IsNot Nothing AndAlso .ClaimDetails.ClaimPeril.Count > 0 Then
                            For Each oBasePeril As BaseClasses.BaseGetClaimPerilDetailsType In .ClaimDetails.ClaimPeril
                                oPeril = New PerilSummary()
                                With oBasePeril
                                    oPeril.BaseClaimPerilKey = .BaseClaimPerilKey
                                    oClaimPayments = New ClaimPaymentCollection

                                    If .ClaimPayments IsNot Nothing AndAlso .ClaimPayments.Count > 0 Then
                                        For Each oBasePayment As BaseClasses.BaseGetClaimPaymentDetailsType In .ClaimPayments
                                            oClaimPayment = New ClaimPayment
                                            With oBasePayment
                                                If Not IsNothing(.AdvancedTaxDetails) Then
                                                    oClaimPayment.PaymentAdvancedTaxDetails.InsuranceTaxNumber = .AdvancedTaxDetails.InsuranceTaxNumber
                                                    oClaimPayment.PaymentAdvancedTaxDetails.InsuredDomiciled = .AdvancedTaxDetails.InsuredDomiciled
                                                    oClaimPayment.PaymentAdvancedTaxDetails.InsuredPercentage = .AdvancedTaxDetails.InsuredPercentage
                                                    oClaimPayment.PaymentAdvancedTaxDetails.IsSettlement = .AdvancedTaxDetails.IsSettlement
                                                    oClaimPayment.PaymentAdvancedTaxDetails.IsTaxExempt = .AdvancedTaxDetails.IsTaxExempt
                                                    oClaimPayment.PaymentAdvancedTaxDetails.IsWHTExempt = .AdvancedTaxDetails.IsWHTExempt
                                                    oClaimPayment.PaymentAdvancedTaxDetails.PayeeDomiciled = .AdvancedTaxDetails.PayeeDomiciled
                                                    oClaimPayment.PaymentAdvancedTaxDetails.PayeePercentage = .AdvancedTaxDetails.PayeePercentage
                                                    oClaimPayment.PaymentAdvancedTaxDetails.PayeeTaxNumber = .AdvancedTaxDetails.PayeeTaxNumber
                                                    oClaimPayment.PaymentAdvancedTaxDetails.SafeHarbourCode = .AdvancedTaxDetails.SafeHarbourCode
                                                    oClaimPayment.PaymentAdvancedTaxDetails.SafeHarbourPercentage = .AdvancedTaxDetails.SafeHarbourPercentage
                                                End If
                                                oClaimPayment.BaseAmount = .BaseAmount
                                                oClaimPayment.BaseClaimPaymentKey = .BaseClaimPaymentKey

                                                If .ClaimPaymentItems IsNot Nothing AndAlso .ClaimPaymentItems.Count > 0 Then
                                                    oClaimPaymentItems = New ClaimPaymentItemCollection
                                                    For Each oBasePaymentItem As BaseClasses.BaseGetClaimPaymentItemDetailsType In .ClaimPaymentItems
                                                        With oBasePaymentItem
                                                            oPaymentItem = New ClaimPaymentItem
                                                            oPaymentItem.BaseClaimPaymentItemKey = .BaseClaimPaymentItemKey
                                                            oPaymentItem.BaseRecoveryKey = .BaseRecoveryKey
                                                            oPaymentItem.BaseReserveKey = .BaseReserveKey
                                                            oPaymentItem.ReserveKey = .ReserveKey
                                                            oPaymentItem.PaymentAdjustment = .PaymentAdjustment
                                                            oPaymentItem.PaymentAmount = .PaymentAmount
                                                            oPaymentItem.TaxAmount = .TaxAmount
                                                            oPaymentItem.TotalTaxAmount = .TotalTaxAmount
                                                            oPaymentItem.ThisRevision = .ThisRevision
                                                            oPaymentItem.LossAmount = .LossAmount
                                                            oPaymentItem.BaseAmount = .BaseAmount
                                                            oPaymentItem.LossTaxAmount = .LossTaxAmount
                                                            oPaymentItem.TaxGroupCode = .TaxGroupCode
                                                        End With
                                                        oClaimPaymentItems.Add(oPaymentItem)
                                                    Next
                                                    oClaimPayment.PaymentItems = oClaimPaymentItems
                                                End If
                                                oClaimPayment.CurrencyCode = .CurrencyCode
                                                oClaimPayment.LossCurrencyCode = .LossCurrencyCode
                                                oClaimPayment.CurrencyDescription = .CurrencyDescription
                                                oClaimPayment.IsReferred = .IsReferred
                                                oClaimPayment.LossAmount = .LossAmount
                                                oClaimPayment.PartyKey = .PartyKey
                                                oClaimPayment.PartyPaidCode = .PartyPaidCode
                                                oClaimPayment.PartyPaidName = .PartyPaidName
                                                oClaimPayment.OurRef = .OurRef
                                                oClaimPayment.IsExGratia = .IsExGratia
                                                oClaimPayment.IsThisPayment = .IsThisPayment
                                                'oClaimPayment.DocumentReference = .DocumentReference
                                                'oClaimPayment.PaymentStatus = .PaymentStatus
                                                'oClaimPayment.TheirReference = .TheirReference
                                                If .Payee IsNot Nothing Then
                                                    If Not IsNothing(.Payee.Address) Then
                                                        oClaimPayment.Payee.Address.Address1 = .Payee.Address.AddressLine1
                                                        oClaimPayment.Payee.Address.Address2 = .Payee.Address.AddressLine2
                                                        oClaimPayment.Payee.Address.Address3 = .Payee.Address.AddressLine3
                                                        oClaimPayment.Payee.Address.Address4 = .Payee.Address.AddressLine4
                                                        oClaimPayment.Payee.Address.AddressType = .Payee.Address.AddressTypeCode
                                                        oClaimPayment.Payee.Address.CountryCode = .Payee.Address.CountryCode
                                                        oClaimPayment.Payee.Address.PostCode = .Payee.Address.PostCode
                                                    End If

                                                    oClaimPayment.Payee.BankCode = .Payee.BankCode
                                                    oClaimPayment.Payee.BankName = .Payee.BankName
                                                    oClaimPayment.Payee.BankNumber = .Payee.BankNumber
                                                    oClaimPayment.Payee.Comments = .Payee.Comments
                                                    oClaimPayment.Payee.MediaReference = .Payee.MediaReference
                                                    oClaimPayment.Payee.MediaTypeDesc = .Payee.MediaTypeDesc
                                                    oClaimPayment.Payee.MediaTypeCode = .Payee.MediaTypeCode
                                                    oClaimPayment.Payee.Name = .Payee.Name
                                                    oClaimPayment.Payee.TheirReference = .Payee.TheirReference
                                                    oClaimPayment.Payee.BIC = .Payee.BIC
                                                    oClaimPayment.Payee.IBAN = .Payee.IBAN
                                                    oClaimPayment.Payee.AccountType = .Payee.AccountType
                                                End If

                                                oClaimPayment.PaymentAmount = .PaymentAmount
                                                oClaimPayment.PaymentDate = .PaymentDate
                                                If .PaymentPartyType IsNot Nothing AndAlso .PaymentPartyType.Count > 0 Then
                                                    Select Case .PaymentPartyType.Trim
                                                        Case NexusProvider.ClaimPaymentPartyTypeType.AGENT.ToString
                                                            oClaimPayment.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.AGENT
                                                        Case NexusProvider.ClaimPaymentPartyTypeType.PARTY.ToString
                                                            oClaimPayment.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.PARTY
                                                        Case NexusProvider.ClaimPaymentPartyTypeType.CLMPAYABLE.ToString
                                                            oClaimPayment.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.CLMPAYABLE
                                                        Case NexusProvider.ClaimPaymentPartyTypeType.CLIENT.ToString
                                                            oClaimPayment.PaymentPartyType = NexusProvider.ClaimPaymentPartyTypeType.CLIENT
                                                    End Select
                                                End If

                                                oClaimPayment.TaxAmount = .TaxAmount
                                                oClaimPayment.UltimatePayee = .UltimatePayee
                                            End With
                                            oClaimPayments.Add(oClaimPayment)
                                        Next
                                        oPeril.ClaimPayment = oClaimPayments
                                    End If
                                    oPeril.ClaimPerilKey = .ClaimPerilKey
                                    oClaimReceipts = New ClaimReceiptCollection

                                    If .ClaimReceipts IsNot Nothing AndAlso .ClaimReceipts.Count > 0 Then
                                        For Each oBaseClaimReceipt As BaseClasses.BaseGetClaimReceiptDetailsType In .ClaimReceipts
                                            oClaimReceipt = New ClaimReceipt
                                            With oBaseClaimReceipt
                                                If .AdvancedTax IsNot Nothing Then
                                                    oClaimReceipt.AdvancedTaxDetails.InsuredDomiciled = .AdvancedTax.InsuredDomiciled
                                                    oClaimReceipt.AdvancedTaxDetails.InsuredPercentage = .AdvancedTax.InsuredPercentage
                                                    oClaimReceipt.AdvancedTaxDetails.InsuredTaxNumber = .AdvancedTax.InsuredTaxNumber
                                                    oClaimReceipt.AdvancedTaxDetails.IsSettlement = .AdvancedTax.IsSettlement
                                                    oClaimReceipt.AdvancedTaxDetails.IsTaxExempt = .AdvancedTax.IsTaxExempt
                                                    oClaimReceipt.AdvancedTaxDetails.ReceivableTaxPercentage = .AdvancedTax.ReceivableTaxPercentage
                                                End If
                                                oClaimReceipt.BaseClaimReceiptKey = .BaseClaimReceiptKey
                                                oClaimReceipt.CurrencyCode = .CurrencyCode
                                                oClaimReceipt.PartyKey = .PartyKey
                                                oClaimReceipt.PartyReceiptCode = .ReceiptPartyCode
                                                oClaimReceipt.ReceiptPartyType = .ReceiptPartyType
                                                oClaimReceipt.ClaimReceiptKey = .ClaimReceiptKey
                                                If .Payee IsNot Nothing Then
                                                    If Not IsNothing(.Payee.Address) Then
                                                        oClaimReceipt.Payee.Address.Address1 = .Payee.Address.AddressLine1
                                                        oClaimReceipt.Payee.Address.Address2 = .Payee.Address.AddressLine2
                                                        oClaimReceipt.Payee.Address.Address3 = .Payee.Address.AddressLine3
                                                        oClaimReceipt.Payee.Address.Address4 = .Payee.Address.AddressLine4
                                                        oClaimReceipt.Payee.Address.AddressType = .Payee.Address.AddressTypeCode
                                                        oClaimReceipt.Payee.Address.CountryCode = .Payee.Address.CountryCode
                                                        oClaimReceipt.Payee.Address.PostCode = .Payee.Address.PostCode
                                                    End If

                                                    oClaimReceipt.Payee.BankCode = .Payee.BankCode
                                                    oClaimReceipt.Payee.BankName = .Payee.BankName
                                                    oClaimReceipt.Payee.BankNumber = .Payee.BankNumber
                                                    oClaimReceipt.Payee.Comments = .Payee.Comments
                                                    oClaimReceipt.Payee.MediaReference = .Payee.MediaReference
                                                    oClaimReceipt.Payee.MediaTypeCode = .Payee.MediaTypeCode
                                                    oClaimReceipt.Payee.Name = .Payee.Name
                                                    oClaimReceipt.Payee.TheirReference = .Payee.TheirReference
                                                End If
                                                oClaimReceipt.ReceiptAmount = .ReceiptAmount
                                                oClaimReceipt.ReceiptDate = .ReceiptDate
                                                oClaimReceipt.ReceiptPartyType = .ReceiptPartyType
                                                oClaimReceipt.TaxAmount = .TaxAmount


                                                If .ReceiptItem IsNot Nothing AndAlso .ReceiptItem.Count > 0 Then
                                                    For Each oBaseReceiptItem As BaseClasses.BaseGetClaimReceiptItemDetailsType In .ReceiptItem
                                                        With oBaseReceiptItem
                                                            oReceiptItem = New ClaimReceiptItemType
                                                            oReceiptItem.BaseClaimReceiptItemKey = .BaseClaimReceiptItemKey
                                                            oReceiptItem.BaseRecoveryKey = .BaseRecoveryKey
                                                            oReceiptItem.BaseReserveKey = .BaseReserveKey
                                                            oReceiptItem.ReceiptAmount = .ReceiptAmount
                                                            oReceiptItem.TaxAmount = .TaxAmount
                                                            oReceiptItem.LossAmount = .ReceiptLossAmount
                                                            oReceiptItem.BaseAmount = .ReceiptBaseAmount
                                                            oReceiptItem.RecoveryTypeCode = .RecoveryTypeCode
                                                            oReceiptItem.ClaimReceiptItemKey = .ClaimReceiptItemKey
                                                            oReceiptItem.TaxGroupCode = .TaxGroupCode
                                                        End With
                                                        oClaimReceiptItems.Add(oReceiptItem)
                                                    Next
                                                    oClaimReceipt.ClaimReceiptItem = oClaimReceiptItems
                                                End If
                                                oClaimReceipts.Add(oClaimReceipt)
                                            End With
                                        Next
                                        oPeril.ClaimReceipt = oClaimReceipts
                                    End If
                                    oPeril.Description = .Description
                                    oPeril.GisScreenCode = .GisScreenCode

                                    oSalvageRecoveries = New PerilRecoveryCollection
                                    oTPRecoveries = New PerilRecoveryCollection
                                    oSalvageRecovery = New PerilRecovery
                                    oTPRecovery = New PerilRecovery

                                    If .Recovery IsNot Nothing AndAlso .Recovery.Count > 0 Then
                                        oSalvageRecoveries = New PerilRecoveryCollection
                                        oTPRecoveries = New PerilRecoveryCollection
                                        For Each oBaseRecovery As BaseClasses.BaseGetClaimRecoveryDetailsType In .Recovery
                                            If oBaseRecovery.IsSalvage = 1 Then
                                                oSalvageRecovery = New PerilRecovery
                                                With oBaseRecovery
                                                    oSalvageRecovery.BaseRecoveryKey = .BaseRecoveryKey
                                                    oSalvageRecovery.CurrencyCode = .CurrencyCode
                                                    oSalvageRecovery.InitialRecovery = .InitialRecovery
                                                    oSalvageRecovery.IsSalvage = .IsSalvage
                                                    oSalvageRecovery.ReceiptedAmount = .ReceiptedAmount
                                                    oSalvageRecovery.ReceiptedTaxAmount = .ReceiptedTaxAmount
                                                    oSalvageRecovery.RevisedRecovery = .RevisedRecovery
                                                    oSalvageRecovery.TypeCode = .TypeCode
                                                    oSalvageRecovery.TotalRecovery = .InitialRecovery + .RevisedRecovery - .ReceiptedAmount
                                                    oSalvageRecovery.IsNew = False
                                                    oSalvageRecovery.CanDelete = .CanDelete
                                                    oSalvageRecovery.ClaimPerilId = .ClaimPerilId
                                                    oSalvageRecovery.RecoveryPartyTypeId = .RecoveryPartyTypeKey
                                                    oSalvageRecovery.RecoveryPartyKey = .RecoveryPartyKey
                                                    oSalvageRecovery.PartyShortName = .PartyShortName
                                                    oSalvageRecovery.RecoveryPartyTypeCode = .RecoveryPartyTypeCode
                                                End With
                                                oSalvageRecoveries.Add(oSalvageRecovery)
                                            Else
                                                oTPRecovery = New PerilRecovery
                                                With oBaseRecovery
                                                    oTPRecovery.BaseRecoveryKey = .BaseRecoveryKey
                                                    oTPRecovery.CurrencyCode = .CurrencyCode
                                                    oTPRecovery.InitialRecovery = .InitialRecovery
                                                    oTPRecovery.IsSalvage = .IsSalvage
                                                    oTPRecovery.ReceiptedAmount = .ReceiptedAmount
                                                    oTPRecovery.ReceiptedTaxAmount = .ReceiptedTaxAmount
                                                    oTPRecovery.RevisedRecovery = .RevisedRecovery
                                                    oTPRecovery.TypeCode = .TypeCode
                                                    oTPRecovery.TotalRecovery = .InitialRecovery + .RevisedRecovery - .ReceiptedAmount
                                                    oTPRecovery.IsNew = False
                                                    oTPRecovery.CanDelete = .CanDelete
                                                    oTPRecovery.ClaimPerilId = .ClaimPerilId
                                                    oTPRecovery.RecoveryPartyTypeId = .RecoveryPartyTypeKey
                                                    oTPRecovery.RecoveryPartyKey = .RecoveryPartyKey
                                                    oTPRecovery.PartyShortName = .PartyShortName
                                                    oTPRecovery.RecoveryPartyTypeCode = .RecoveryPartyTypeCode
                                                End With
                                                oTPRecoveries.Add(oTPRecovery)
                                            End If
                                        Next
                                        oPeril.SalvageRecovery = oSalvageRecoveries
                                        oPeril.TPRecovery = oTPRecoveries
                                    End If

                                    oReserves = New ReserveCollection

                                    If .Reserve IsNot Nothing AndAlso .Reserve.Count > 0 Then
                                        For Each oBaseReserve As BaseClasses.BaseGetClaimReserveDetailsType In .Reserve
                                            oReserve = New Reserve
                                            With oBaseReserve
                                                oReserve.BaseReserveKey = .BaseReserveKey
                                                oReserve.InitialReserve = .InitialReserve
                                                oReserve.PaidAmount = .PaidAmount
                                                oReserve.RevisedReserve = .RevisedReserve
                                                oReserve.SumInsured = .SumInsured
                                                oReserve.TypeCode = .TypeCode
                                                oReserve.IsExcess = .IsExcess
                                                oReserve.IsExpense = .IsExpense
                                                oReserve.TypeDescription = .TypeDescription
                                                oReserve.IsIndemnity = .IsIndemnity
                                                oReserve.GrossReserve = .GrossReserve
                                                oReserve.Tax = .Tax
                                                oReserve.RevisedGrossReserve = .RevisedGrossReserve
                                                oReserve.RevisedTaxReserve = .RevisedTaxReserve
                                                oReserve.PaidToDateTax = .PaidToDateTax
                                                oPeril.PaidAmount += .PaidAmount
                                                oPeril.CurrentReserve += .InitialReserve + .RevisedReserve - .PaidAmount
                                            End With
                                            oReserves.Add(oReserve)

                                        Next
                                        oPeril.Reserve = oReserves
                                    End If
                                    oPeril.RIBand = .RIBand
                                    oPeril.SumInsured = .SumInsured
                                    oPeril.TypeCode = .TypeCode
                                End With
                                oClaimPerils.Add(oPeril)
                            Next
                            oClaim.ClaimPeril = oClaimPerils
                        End If
                    End If

                End With

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGetClaimDetailsRequest = Nothing
                oGetClaimDetailsResponse = Nothing
                oClaimPerils = Nothing
                oPeril = Nothing
                oContactForInsurer = Nothing
                oClaimPayments = Nothing
                oClaimPayment = Nothing
                oClaimPaymentItems = Nothing
                oClaimReceipts = Nothing
                oRecoveries = Nothing
                oReserves = Nothing
                oPaymentItem = Nothing
                oClaimReceipt = Nothing
                oClaimReceiptItems = Nothing
                oReceiptItem = Nothing
                oSalvageRecoveries = Nothing
                oTPRecoveries = Nothing
                oSalvageRecovery = Nothing
                oTPRecovery = Nothing
                oReserve = Nothing
            End Try

            Return oClaim

        End SyncLock
    End Function

    ''' <summary>
    ''' Get Claim Party Details
    ''' </summary>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetClaimPartyDetails(ByVal v_iInsuranceFileKey As Integer,
                                                     Optional ByVal v_sBranchCode As String = Nothing) As InsuranceFileDetails

        Dim oGetClaimPartyDetailsRequest As BaseClasses.GetClaimPartyDetailsQuery
        Dim oGetClaimPartyDetailsResponse As BaseClasses.GetClaimPartyDetailsQueryResponse
        Dim oPartyDetails As NexusProvider.InsuranceFileDetails
        Dim sbLogMessage As StringBuilder

        Try
            oGetClaimPartyDetailsRequest = New BaseClasses.GetClaimPartyDetailsQuery
            oGetClaimPartyDetailsResponse = New BaseClasses.GetClaimPartyDetailsQueryResponse
            sbLogMessage = New StringBuilder


            With oGetClaimPartyDetailsRequest
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
                .InsuranceFileKey = v_iInsuranceFileKey
            End With

            'Calling the SAM Method with Request Type
            'add trace to allow us to debug slow SAM calls
            Using trace As New Tracer(Category.Trace)
                ApiClient._tokenModel = GetApiTokendetails()
                Dim result As String = ApiClient.Get(ApiMethods.GetClaimPartyDetails, oGetClaimPartyDetailsRequest)
                oGetClaimPartyDetailsResponse = ApiClient.DeserializeJson(Of BaseClasses.GetClaimPartyDetailsQueryResponse)(result)
            End Using

            With oGetClaimPartyDetailsResponse
                If .GetClaimPartyDetailsResponse IsNot Nothing AndAlso .GetClaimPartyDetailsResponse.PartyDetails IsNot Nothing AndAlso .GetClaimPartyDetailsResponse.PartyDetails.Count > 0 Then
                    For Each oParty As BaseClasses.BaseGetClaimPartyDetailsResponseTypeRow In .GetClaimPartyDetailsResponse.PartyDetails
                        oPartyDetails = New NexusProvider.InsuranceFileDetails
                        oPartyDetails.ClientAddressLine1 = oParty.Address1
                        oPartyDetails.Address.Address1 = oParty.Address1
                        oPartyDetails.Address.Address2 = oParty.Address2
                        oPartyDetails.Address.Address3 = oParty.Address3
                        oPartyDetails.Address.Address4 = oParty.Address4
                        oPartyDetails.Address.Key = oParty.AddressKey
                        oPartyDetails.Address.CountryKey = oParty.CountryKey
                        oPartyDetails.Address.PostCode = oParty.PostalCode

                        oPartyDetails.ClientName = oParty.ResolvedName
                        oPartyDetails.ClientShortName = oParty.ShortName
                        oPartyDetails.PartyKey = oParty.PartyKey
                        oPartyDetails.PostCode = oParty.PostalCode
                        oPartyDetails.CountryKey = oParty.CountryKey
                        oPartyDetails.Email = oParty.EMail
                        oPartyDetails.Fax = oParty.Fax
                        oPartyDetails.Mobile = oParty.Mobile
                        oPartyDetails.TelHome = oParty.TelHome
                        oPartyDetails.TelOff = oParty.TelOff
                    Next
                End If
            End With

            If Logger.IsLoggingEnabled Then
                sbLogMessage.AppendLine("GetClaimPerilSummary executed ok" & vbCrLf)
                sbLogMessage.AppendLine("Input:" & vbCrLf)
                ' sbLogMessage.AppendLine("r_oPeril = " & r_oPeril.Print.Replace("<br />", vbCrLf))

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
            oGetClaimPartyDetailsRequest = Nothing
            oGetClaimPartyDetailsResponse = Nothing
        End Try


        Return oPartyDetails
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="r_oClaimPaymentTaxes"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetClaimPaymentTaxes(ByVal r_oClaimPaymentTaxes As ClaimPayment,
                                                   Optional ByVal v_sBranchCode As String = Nothing,
                                                    Optional ByVal nGetSavedTaxOfPeril As Integer = 0) As ClaimPayment


        SyncLock oLock
            Dim oGetClaimPaymentTaxesRequest As BaseClasses.GetClaimPaymentTaxesQuery
            Dim oGetClaimPaymentTaxesResponse As BaseClasses.GetClaimPaymentTaxesQueryResponse
            Dim oPaymentItem As BaseClasses.BasePaymentCashListItemType
            Dim oClaimPayment As BaseClasses.BaseClaimPaymentItemType
            Dim oClaimPayments As ClaimPayment
            Dim oTaxItems As ClaimPaymentTaxItem
            Dim oClaimPerilReserve As ClaimPerilReservePaymentType
            Dim sbLogMessage As StringBuilder

            Try
                oGetClaimPaymentTaxesRequest = New BaseClasses.GetClaimPaymentTaxesQuery
                oGetClaimPaymentTaxesResponse = New BaseClasses.GetClaimPaymentTaxesQueryResponse
                oClaimPayments = New ClaimPayment
                sbLogMessage = New StringBuilder


                With oGetClaimPaymentTaxesRequest
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

                    .ApiTimeStamp = r_oClaimPaymentTaxes.TimeStamp
                    .ClaimPayment = New BaseClasses.BaseClaimPaymentType
                    .ClaimPayment.CloseClaimOnZeroReserveRecoveryBalance = r_oClaimPaymentTaxes.CloseClaimOnZeroReserveRecoveryBalance
                    .ClaimPayment.BaseClaimKey = r_oClaimPaymentTaxes.BaseClaimKey
                    .ClaimPayment.BaseClaimPerilKey = r_oClaimPaymentTaxes.BaseClaimPerilKey
                    If r_oClaimPaymentTaxes.PaymentPartyType <> ClaimPaymentPartyTypeType.PARTY Then
                        .ClaimPayment.PartyKey = 0
                    Else
                        .ClaimPayment.PartyKey = r_oClaimPaymentTaxes.PartyKey
                    End If
                    .ClaimPayment.PaymentPartyType = r_oClaimPaymentTaxes.PaymentPartyType
                    .ClaimPayment.CurrencyCode = r_oClaimPaymentTaxes.CurrencyCode
                    .ClaimPayment.ClaimVersionDescription = r_oClaimPaymentTaxes.ClaimVersionDescription
                    .ClaimPayment.ViewMode = r_oClaimPaymentTaxes.ViewMode
                    If r_oClaimPaymentTaxes.PaymentAdvancedTaxDetails.InsuranceTaxNumber <> String.Empty Then
                        .ClaimPayment.AdvancedTaxDetails = New BaseClasses.BaseClaimPaymentAdvancedTaxDetailsType
                        .ClaimPayment.AdvancedTaxDetails.InsuranceTaxNumber = r_oClaimPaymentTaxes.PaymentAdvancedTaxDetails.InsuranceTaxNumber
                        .ClaimPayment.AdvancedTaxDetails.InsuredDomiciled = r_oClaimPaymentTaxes.PaymentAdvancedTaxDetails.InsuredDomiciled
                        .ClaimPayment.AdvancedTaxDetails.InsuredPercentage = r_oClaimPaymentTaxes.PaymentAdvancedTaxDetails.InsuredPercentage
                        .ClaimPayment.AdvancedTaxDetails.IsSettlement = r_oClaimPaymentTaxes.PaymentAdvancedTaxDetails.IsSettlement
                        .ClaimPayment.AdvancedTaxDetails.IsTaxExempt = r_oClaimPaymentTaxes.PaymentAdvancedTaxDetails.IsTaxExempt
                        .ClaimPayment.AdvancedTaxDetails.IsWHTExempt = r_oClaimPaymentTaxes.PaymentAdvancedTaxDetails.IsWHTExempt
                        .ClaimPayment.AdvancedTaxDetails.PayeeDomiciled = r_oClaimPaymentTaxes.PaymentAdvancedTaxDetails.PayeeDomiciled
                        .ClaimPayment.AdvancedTaxDetails.PayeePercentage = r_oClaimPaymentTaxes.PaymentAdvancedTaxDetails.PayeePercentage
                        .ClaimPayment.AdvancedTaxDetails.PayeeTaxNumber = r_oClaimPaymentTaxes.PaymentAdvancedTaxDetails.PayeeTaxNumber
                        .ClaimPayment.AdvancedTaxDetails.SafeHarbourCode = r_oClaimPaymentTaxes.PaymentAdvancedTaxDetails.SafeHarbourCode
                        .ClaimPayment.AdvancedTaxDetails.SafeHarbourPercentage = r_oClaimPaymentTaxes.PaymentAdvancedTaxDetails.SafeHarbourPercentage
                        .ClaimPayment.AdvancedTaxDetails.PaymentTo = r_oClaimPaymentTaxes.PaymentAdvancedTaxDetails.PaymentTo
                        .ClaimPayment.AdvancedTaxDetails.PayeeName = r_oClaimPaymentTaxes.PaymentAdvancedTaxDetails.PayeeName
                    End If
                    'ClaimPayee
                    .ClaimPayment.Payee = New BaseClasses.BaseClaimPayeeType
                    If r_oClaimPaymentTaxes.Payee.Address.Address1 <> String.Empty Then
                        .ClaimPayment.Payee.Address = New BaseClasses.BaseAddressType
                        .ClaimPayment.Payee.Address.AddressLine1 = r_oClaimPaymentTaxes.Payee.Address.Address1
                        .ClaimPayment.Payee.Address.AddressLine2 = r_oClaimPaymentTaxes.Payee.Address.Address2
                        .ClaimPayment.Payee.Address.AddressLine3 = r_oClaimPaymentTaxes.Payee.Address.Address3
                        .ClaimPayment.Payee.Address.AddressLine4 = r_oClaimPaymentTaxes.Payee.Address.Address4
                        .ClaimPayment.Payee.Address.AddressTypeCode = r_oClaimPaymentTaxes.Payee.Address.AddressType
                        .ClaimPayment.Payee.Address.CountryCode = r_oClaimPaymentTaxes.Payee.Address.CountryCode
                        .ClaimPayment.Payee.Address.PostCode = r_oClaimPaymentTaxes.Payee.Address.PostCode
                    End If
                    'CashList
                    If r_oClaimPaymentTaxes.CashList.BankAccountCode <> String.Empty Then
                        .ClaimPayment.CashList = New BaseClasses.BasePaymentCashListType
                        .ClaimPayment.CashList.BankAccountCode = r_oClaimPaymentTaxes.CashList.BankAccountCode
                        .ClaimPayment.CashList.CurrencyCode = r_oClaimPaymentTaxes.CashList.CurrencyCode
                        .ClaimPayment.CashList.ListDate = r_oClaimPaymentTaxes.CashList.ListDate
                        .ClaimPayment.CashList.Reference = r_oClaimPaymentTaxes.CashList.Reference
                        .ClaimPayment.CashList.StatusCode = r_oClaimPaymentTaxes.CashList.StatusCode
                        .ClaimPayment.CashList.TypeCode = r_oClaimPaymentTaxes.CashList.TypeCode
                        .ClaimPayment.CashList.BranchCode = sBranchCode
                        'CashListItem
                        .ClaimPayment.CashList.PaymentItem = New List(Of BaseClasses.BasePaymentCashListItemType)
                        For index As Integer = 0 To r_oClaimPaymentTaxes.CashList.PaymentCashListItemType.Count - 1
                            With r_oClaimPaymentTaxes.CashList.PaymentCashListItemType(index)
                                oPaymentItem = New BaseClasses.BasePaymentCashListItemType()
                                oPaymentItem.AccountShortCode = .AccountShortCode
                                oPaymentItem.AllocationStatusCode = .AllocationStatusCode
                                oPaymentItem.Amount = .Amount
                                oPaymentItem.Bank = New BaseClasses.BaseBankPaymentType
                                oPaymentItem.Bank.AccountCode = .BankPaymentType.AccountCode
                                oPaymentItem.Bank.BranchCode = .BankPaymentType.BranchCode
                                oPaymentItem.Bank.ExpiryDate = .BankPaymentType.ExpiryDate
                                If .BankPaymentType.ExpiryDate <> DateTime.MinValue Then
                                    oPaymentItem.Bank.ExpiryDateSpecified = True
                                Else
                                    oPaymentItem.Bank.ExpiryDateSpecified = False
                                End If
                                oPaymentItem.Bank.PayeeName = .BankPaymentType.PayeeName
                                oPaymentItem.Bank.Reference1 = .BankPaymentType.Reference1
                                oPaymentItem.Bank.Reference2 = .BankPaymentType.Reference2
                                oPaymentItem.BankReference = .BankReference
                                oPaymentItem.Bank.BIC = .BankPaymentType.BIC
                                oPaymentItem.Bank.IBAN = .BankPaymentType.IBAN

                                If Trim(.ContactAddress.Address1) <> String.Empty Then
                                    oPaymentItem.ContactAddress = New BaseClasses.BaseSimpleAddressType
                                    oPaymentItem.ContactAddress.AddressLine1 = .ContactAddress.Address1
                                    oPaymentItem.ContactAddress.AddressLine2 = .ContactAddress.Address2
                                    oPaymentItem.ContactAddress.AddressLine3 = .ContactAddress.Address3
                                    oPaymentItem.ContactAddress.AddressLine4 = .ContactAddress.Address4
                                    oPaymentItem.ContactAddress.CountryCode = .ContactAddress.CountryCode
                                    oPaymentItem.ContactAddress.PostCode = .ContactAddress.PostCode
                                End If
                                oPaymentItem.ContactName = .ContactName
                                oPaymentItem.FurtherDetails = .FurtherDetails
                                oPaymentItem.MediaReference = .MediaReference
                                oPaymentItem.MediaTypeCode = .MediaTypeCode
                                oPaymentItem.OurReference = .OurReference
                                oPaymentItem.StatusCode = .StatusCode
                                oPaymentItem.TheirReference = .TheirReference
                                oPaymentItem.TypeCode = .TypeCode
                                oPaymentItem.TransactionDate = .TransactionDate
                            End With
                            .ClaimPayment.CashList.PaymentItem.Add(oPaymentItem)
                        Next

                    End If

                    'ClaimPaymentItem
                    .ClaimPayment.ClaimPaymentItem = New List(Of BaseClasses.BaseClaimPaymentItemType)
                    For i As Integer = 0 To r_oClaimPaymentTaxes.ClaimPaymentItem.Count - 1
                        ' If r_oClaimPaymentTaxes.ClaimPaymentItem.Count > 0 AndAlso r_oClaimPaymentTaxes.ClaimPaymentItem(i).PaymentAmount <> 0 Then
                        oClaimPayment = New BaseClasses.BaseClaimPaymentItemType
                        oClaimPayment.BaseReserveKey = r_oClaimPaymentTaxes.ClaimPaymentItem(i).BaseReserveKey
                        oClaimPayment.PaymentAmount = r_oClaimPaymentTaxes.ClaimPaymentItem(i).LossPaymentAmount
                        oClaimPayment.ReverseExcess = r_oClaimPaymentTaxes.ClaimPaymentItem(i).ReverseExcess
                        oClaimPayment.TaxGroupCode = r_oClaimPaymentTaxes.ClaimPaymentItem(i).TaxGroupCode
                        oClaimPayment.IsTaxOverridden = r_oClaimPaymentTaxes.ClaimPaymentItem(i).IsTaxOverridden
                        oClaimPayment.OverriddedTaxAmount = r_oClaimPaymentTaxes.ClaimPaymentItem(i).OverriddedTaxAmount
                        .ClaimPayment.ClaimPaymentItem.Add(oClaimPayment)
                        ' End If
                    Next
                    .GetSavedTaxOfPeril = nGetSavedTaxOfPeril
                End With

                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Post(ApiMethods.GetClaimPaymentTaxes, oGetClaimPaymentTaxesRequest)
                    oGetClaimPaymentTaxesResponse = ApiClient.DeserializeJson(Of BaseClasses.GetClaimPaymentTaxesQueryResponse)(result)
                End Using


                ' Disposing the SAM's object


                With oGetClaimPaymentTaxesResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else

                        If .TaxItems IsNot Nothing AndAlso .TaxItems.Count > 0 Then

                            For Each oResponseClaimTaxItems As BaseClasses.BaseClaimPaymentTaxItemType In .TaxItems
                                oTaxItems = New ClaimPaymentTaxItem
                                With oResponseClaimTaxItems
                                    oTaxItems.ReserveType = .ReserveType
                                    oTaxItems.TaxGroupCode = .TaxGroupCode
                                    oTaxItems.TaxBandCode = .TaxBandCode
                                    oTaxItems.Percentage = .Percentage
                                    oTaxItems.Amount = .Amount
                                End With
                                oClaimPayments.ClaimPaymentTaxItems.Add(oTaxItems)
                            Next
                        End If

                        If .Reserves IsNot Nothing AndAlso .Reserves.Count > 0 Then

                            For Each oResponseClaimPerilResType As BaseClasses.BaseClaimPerilReservePaymentType In .Reserves
                                oClaimPerilReserve = New ClaimPerilReservePaymentType
                                With oResponseClaimPerilResType
                                    oClaimPerilReserve.BaseReserveKey = .BaseReserveKey
                                    oClaimPerilReserve.TypeCode = .TypeCode
                                    oClaimPerilReserve.TotalReserve = .TotalReserve
                                    oClaimPerilReserve.PaidToDate = .PaidToDate
                                    oClaimPerilReserve.PaidToDateTax = .PaidToDateTax
                                    oClaimPerilReserve.CurrentReserve = .CurrentReserve
                                    oClaimPerilReserve.ThisPaymentINCLTax = .ThisPaymentINCLTax
                                    oClaimPerilReserve.ThisPaymentTax = .ThisPaymentTax
                                    oClaimPerilReserve.CostToClaim = .CostToClaim
                                End With
                                oClaimPayments.ClaimReserve.Add(oClaimPerilReserve)
                            Next
                        End If

                        If .PaymentItems IsNot Nothing AndAlso .PaymentItems.Count > 0 Then
                            Dim oClaimPaymentItem As ClaimPaymentItem
                            For Each oResponseClaimPayment As BaseClasses.BaseClaimPaymentItemType In .PaymentItems
                                oClaimPaymentItem = New ClaimPaymentItem
                                With oResponseClaimPayment
                                    oClaimPaymentItem.BaseReserveKey = .BaseReserveKey
                                    oClaimPaymentItem.PaymentAmount = .PaymentAmount
                                    oClaimPaymentItem.ReverseExcess = .ReverseExcess
                                    oClaimPaymentItem.TaxGroupCode = .TaxGroupCode
                                End With
                                oClaimPayments.PaymentItems.Add(oClaimPaymentItem)
                            Next
                        End If
                    End If

                End With

                'Logging
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("Generate ClaimsDocuments executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iBaseClaimKey = " & oClaimPayments.ToString() & vbCrLf)

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
                oGetClaimPaymentTaxesRequest = Nothing
                oGetClaimPaymentTaxesResponse = Nothing
            End Try

            Return oClaimPayments
        End SyncLock
    End Function

    Public Overrides Function GetClaimPaymentTaxGroups(ByVal v_iPartyKey As Integer,
                                                        ByVal v_oPartyType As NexusProvider.ClaimReceiptPartyTypeType,
                                                        ByVal v_oAdvancedTax As ClaimReceiptAdvancedTaxDetails,
                                                        Optional ByVal v_sBranchCode As String = Nothing) As TaxCollection

        Dim oGetPaymentTaxgroupsRequest As BaseClasses.GetClaimPaymentTaxGroupsQuery
        Dim oGetPaymentTaxgroupsResponse As BaseClasses.GetClaimPaymentTaxGroupsQueryResponse
        Dim oPaymentTaxGroups As TaxCollection
        Dim oPaymentTaxGroupItem As Tax
        Dim sbLogMessage As StringBuilder

        Try
            oGetPaymentTaxgroupsRequest = New BaseClasses.GetClaimPaymentTaxGroupsQuery
            oGetPaymentTaxgroupsResponse = New BaseClasses.GetClaimPaymentTaxGroupsQueryResponse
            sbLogMessage = New StringBuilder


            With oGetPaymentTaxgroupsRequest
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

                .PartyKey = v_iPartyKey

                Select Case v_oPartyType
                    Case ClaimPaymentPartyTypeType.AGENT
                        .PaymentPartyType = ClaimPaymentPartyTypeType.AGENT
                    Case ClaimPaymentPartyTypeType.CLIENT
                        .PaymentPartyType = ClaimPaymentPartyTypeType.CLIENT
                    Case ClaimPaymentPartyTypeType.CLMPAYABLE
                        .PaymentPartyType = ClaimPaymentPartyTypeType.CLMPAYABLE
                    Case ClaimPaymentPartyTypeType.PARTY
                        .PaymentPartyType = ClaimPaymentPartyTypeType.PARTY
                End Select

                .PaymentPartyType = v_oPartyType

                If v_oAdvancedTax.PayeeDomiciled > 0 Then
                    .AdvancedTax.PayeeDomiciled = v_oAdvancedTax.PayeeDomiciled
                Else
                    Throw New ArgumentException("PayeeDomiciled")
                End If

                If v_oAdvancedTax.PayeeDomiciled > 0 Then
                    .AdvancedTax.PayeePercentage = v_oAdvancedTax.PayeePercentage
                Else
                    Throw New ArgumentException("PayeePercentage")
                End If
                .AdvancedTax.PayeeTaxNumber = v_oAdvancedTax.PayeeTaxNumber
            End With

            'Calling the SAM Method with Request Type
            'add trace to allow us to debug slow SAM calls
            Using trace As New Tracer(Category.Trace)
                ApiClient._tokenModel = GetApiTokendetails()
                Dim result As String = ApiClient.Get(ApiMethods.GetClaimPaymentTaxGroups, oGetPaymentTaxgroupsRequest)
                oGetPaymentTaxgroupsResponse = ApiClient.DeserializeJson(Of BaseClasses.GetClaimPaymentTaxGroupsQueryResponse)(result)

            End Using

            'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

            ' Disposing the SAM's object

            oPaymentTaxGroups = New TaxCollection

            With oGetPaymentTaxgroupsResponse
                If .TaxGroup IsNot Nothing AndAlso .TaxGroup.Count > 0 Then
                    For Each oTaxGroupItemType As BaseClasses.BaseGetClaimPaymentTaxGroupsResponseTypeRow In .TaxGroup
                        oPaymentTaxGroupItem = New Tax
                        oPaymentTaxGroupItem.Code = oTaxGroupItemType.Code
                        oPaymentTaxGroupItem.Description = oTaxGroupItemType.Description
                        oPaymentTaxGroupItem.IsWithHoldingTax = oTaxGroupItemType.IsWithholdingTax
                        oPaymentTaxGroups.Add(oPaymentTaxGroupItem)
                    Next
                End If
            End With


            If Logger.IsLoggingEnabled Then
                sbLogMessage.AppendLine("GetClaimPaymentTaxGroups executed ok" & vbCrLf)
                sbLogMessage.AppendLine("Input:" & vbCrLf)
                sbLogMessage.AppendLine("v_iPartyKey = " & v_iPartyKey.ToString() & vbCrLf)
                sbLogMessage.AppendLine("v_oPartyType = " & v_oPartyType.ToString() & vbCrLf)
                sbLogMessage.AppendLine("v_oAdvancedTax PayeeDomiciled= " & v_oAdvancedTax.PayeeDomiciled & vbCrLf)
                sbLogMessage.AppendLine("v_oAdvancedTax PayeePercentage = " & v_oAdvancedTax.PayeePercentage & vbCrLf)
                sbLogMessage.AppendLine("v_oAdvancedTax PayeeTaxNumber= " & v_oAdvancedTax.PayeeTaxNumber & vbCrLf)
                sbLogMessage.AppendLine("Output:" & vbCrLf)
                'sbLogMessage.AppendLine("GetClaimPaymentTaxGroups" & GetClaimPaymentTaxGroups.Print.Replace("<br />", vbCrLf))

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
            oGetPaymentTaxgroupsRequest = Nothing
            oGetPaymentTaxgroupsResponse = Nothing
        End Try


        Return oPaymentTaxGroups
    End Function

    ''' <summary>
    ''' Gets the summary of Claim peril
    ''' </summary>
    ''' <param name="r_oPeril"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns>Peril Object</returns>
    ''' <remarks></remarks>
    ''' 
    Public Overrides Function GetClaimPerilSummary(ByRef r_oPeril As PerilSummary,
                                                Optional ByVal v_sBranchCode As String = Nothing) As PerilSummary

        Dim oGetClaimPerilSummmaryRequest As BaseClasses.GetClaimPerilSummaryQuery
        Dim oGetClaimPerilSummaryResponse As BaseClasses.GetClaimPerilSummaryQueryResponse
        Dim oPerilSummary As PerilSummary
        Dim oPeril As PerilSummary
        Dim oReserveType As ReserveType
        Dim oSalvageRecovery As PerilRecovery
        Dim oTPRecovery As PerilRecovery
        Dim sbLogMessage As StringBuilder

        Try
            oGetClaimPerilSummmaryRequest = New BaseClasses.GetClaimPerilSummaryQuery
            oGetClaimPerilSummaryResponse = New BaseClasses.GetClaimPerilSummaryQueryResponse
            oPerilSummary = New PerilSummary
            sbLogMessage = New StringBuilder

            With oGetClaimPerilSummmaryRequest
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
                If r_oPeril.ClaimKey > 0 Then
                    .ClaimKey = r_oPeril.ClaimKey
                Else
                    Throw New ArgumentNullException("ClaimKey")
                End If

                .IncludeReserveTypes = r_oPeril.IncludeReserveTypes
                .IncludeSalvageRecovery = r_oPeril.IncludeSalvageRecovery
                .IncludeTotals = r_oPeril.IncludeTotals
                .IncludeTPRecovery = r_oPeril.IncludeTPRecovery
            End With

            'Calling the SAM Method with Request Type
            'add trace to allow us to debug slow SAM calls
            Using trace As New Tracer(Category.Trace)
                ApiClient._tokenModel = GetApiTokendetails()
                Dim result As String = ApiClient.Get(ApiMethods.GetClaimPerilSummary, oGetClaimPerilSummmaryRequest)
                oGetClaimPerilSummaryResponse = ApiClient.DeserializeJson(Of BaseClasses.GetClaimPerilSummaryQueryResponse)(result)
            End Using

            With oGetClaimPerilSummaryResponse
                If .Errors IsNot Nothing Then
                    'Process the error object if errors, and throw as a single exception
                    Throw New NexusException(.Errors)
                Else

                    If .PerilTotals IsNot Nothing AndAlso .PerilTotals.Count > 0 Then
                        For Each oPerilRow As BaseClasses.BaseGetClaimPerilSummaryResponseTypeRow In .PerilTotals
                            oPeril = New PerilSummary
                            oPeril.Average = oPerilRow.Average
                            oPeril.CurrentReserve = oPerilRow.CurrentReserve
                            oPeril.Description = oPerilRow.Description
                            oPeril.InitialReserve = oPerilRow.InitialReserve
                            oPeril.PaidAmount = oPerilRow.PaidAmount
                            oPeril.RevisedReserve = oPerilRow.RevisedReserve
                            oPeril.SumInsured = oPerilRow.SumInsured

                            oPerilSummary.PerilTotals.Add(oPeril)
                        Next
                    End If
                    '.ReserveType

                    If .ReserveType IsNot Nothing AndAlso .ReserveType.Count > 0 Then
                        For Each oReserveTypeRow As BaseClasses.BaseGetClaimPerilSummaryResponseTypeReserveType In .ReserveType
                            oReserveType = New ReserveType
                            oReserveType.Code = oReserveTypeRow.Code
                            oReserveType.Description = oReserveTypeRow.Description
                            If oReserveTypeRow.Perils IsNot Nothing AndAlso oReserveTypeRow.Perils.Count > 0 Then
                                For Each oPerilReserveTypeRow As BaseClasses.BaseGetClaimPerilSummaryResponseTypeReserveTypeRow In oReserveTypeRow.Perils
                                    oPeril = New PerilSummary
                                    oPeril.Average = oPerilReserveTypeRow.Average
                                    oPeril.CurrentReserve = oPerilReserveTypeRow.CurrentReserve
                                    oPeril.Description = oPerilReserveTypeRow.Description
                                    oPeril.InitialReserve = oPerilReserveTypeRow.InitialReserve
                                    oPeril.PaidAmount = oPerilReserveTypeRow.PaidAmount
                                    oPeril.RevisedReserve = oPerilReserveTypeRow.RevisedReserve
                                    oPeril.SumInsured = oPerilReserveTypeRow.SumInsured

                                    oReserveType.Perils.Add(oPeril)
                                Next
                            End If
                            oPerilSummary.ReserveType.Add(oReserveType)
                        Next
                    End If
                    'Salvage recovery

                    If .SalvageRecoveryPerils IsNot Nothing AndAlso .SalvageRecoveryPerils.Count > 0 Then
                        For Each oPerilSRRow As BaseClasses.BaseGetClaimPerilSummaryResponseTypeRow2 In .SalvageRecoveryPerils
                            oSalvageRecovery = New PerilRecovery
                            oSalvageRecovery.CurrentRecovery = oPerilSRRow.CurrentRecovery
                            oSalvageRecovery.Description = oPerilSRRow.Description
                            oSalvageRecovery.InitialRecovery = oPerilSRRow.InitialRecovery
                            oSalvageRecovery.ReceiptedAmount = oPerilSRRow.ReceiptedAmount
                            oSalvageRecovery.RevisedRecovery = oPerilSRRow.RevisedRecovery
                            oPerilSummary.SalvageRecovery.Add(oSalvageRecovery)
                        Next
                    End If
                    'TP recovery
                    If .TPRecoveryPerils IsNot Nothing AndAlso .TPRecoveryPerils.Count > 0 Then
                        For Each oPerilTPRRow As BaseClasses.BaseGetClaimPerilSummaryResponseTypeRow1 In .TPRecoveryPerils

                            oTPRecovery = New PerilRecovery
                            oTPRecovery.CurrentRecovery = oPerilTPRRow.CurrentRecovery
                            oTPRecovery.Description = oPerilTPRRow.Description
                            oTPRecovery.InitialRecovery = oPerilTPRRow.InitialRecovery
                            oTPRecovery.ReceiptedAmount = oPerilTPRRow.ReceiptedAmount
                            oTPRecovery.RevisedRecovery = oPerilTPRRow.RevisedRecovery

                            oPerilSummary.TPRecovery.Add(oTPRecovery)
                        Next
                    End If
                End If
            End With
            If Logger.IsLoggingEnabled Then
                sbLogMessage.AppendLine("GetClaimPerilSummary executed ok" & vbCrLf)
                sbLogMessage.AppendLine("Input:" & vbCrLf)
                sbLogMessage.AppendLine("r_oPeril = " & r_oPeril.Print.Replace("<br />", vbCrLf))

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
            oGetClaimPerilSummmaryRequest = Nothing
            oGetClaimPerilSummaryResponse = Nothing
        End Try

        Return oPerilSummary
    End Function
    ''' <summary>
    ''' Get Claim Receipt taxes
    ''' </summary>
    ''' <param name="r_oClaimReceipt"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="nGetSavedTaxOfPeril"></param>
    Public Overrides Sub GetClaimReceiptTaxes(ByRef r_oClaimReceipt As ClaimReceipt,
                                                       Optional ByVal v_sBranchCode As String = Nothing,
                                                      Optional ByVal nGetSavedTaxOfPeril As Integer = 0)

        Dim oGetClaimReceiptTaxesRequest As BaseClasses.GetClaimReceiptTaxesQuery
        Dim oGetClaimReceiptTaxesResponse As BaseClasses.GetClaimReceiptTaxesQueryResponse
        Dim oClaimReceipt As BaseClasses.BaseClaimReceiptType
        Dim oAdvanceTaxDetails As BaseClasses.BaseClaimReceiptAdvancedTaxDetailsType
        Dim oClaimReceiptItemType As BaseClasses.BaseClaimReceiptItemType
        Dim oPayee As BaseClasses.BaseClaimPayeeType
        Dim oAddress As BaseClasses.BaseAddressType
        Dim oTaxItem As NexusProvider.BaseClaimTaxItemType
        Dim sbLogMessage As StringBuilder

        Try
            oGetClaimReceiptTaxesRequest = New BaseClasses.GetClaimReceiptTaxesQuery
            oGetClaimReceiptTaxesResponse = New BaseClasses.GetClaimReceiptTaxesQueryResponse
            oClaimReceipt = New BaseClasses.BaseClaimReceiptType
            oAdvanceTaxDetails = New BaseClasses.BaseClaimReceiptAdvancedTaxDetailsType
            oPayee = New BaseClasses.BaseClaimPayeeType
            oAddress = New BaseClasses.BaseAddressType
            oTaxItem = New NexusProvider.BaseClaimTaxItemType
            sbLogMessage = New StringBuilder


            With oGetClaimReceiptTaxesRequest
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
                .ApiTimeStamp = r_oClaimReceipt.TimeStamp

                oClaimReceipt.BaseClaimKey = r_oClaimReceipt.BaseClaimKey
                oClaimReceipt.BaseClaimPerilKey = r_oClaimReceipt.BaseClaimPerilKey
                oClaimReceipt.ReceiptPartyType = r_oClaimReceipt.ReceiptPartyType
                oClaimReceipt.PartyKey = r_oClaimReceipt.PartyKey
                oClaimReceipt.CurrencyCode = r_oClaimReceipt.CurrencyCode
                oClaimReceipt.ClaimVersionDescription = r_oClaimReceipt.ClaimVersionDescription
                oClaimReceipt.ClaimKey = r_oClaimReceipt.ClaimKey
                oClaimReceipt.ClaimPerilKey = r_oClaimReceipt.ClaimPerilKey

                If r_oClaimReceipt.AdvancedTaxDetails IsNot Nothing Then


                    oAdvanceTaxDetails.IsSettlement = r_oClaimReceipt.AdvancedTaxDetails.IsSettlement
                    oAdvanceTaxDetails.IsTaxExempt = r_oClaimReceipt.AdvancedTaxDetails.IsTaxExempt
                    oAdvanceTaxDetails.ReceivableTaxPercentage = r_oClaimReceipt.AdvancedTaxDetails.ReceivableTaxPercentage
                    oAdvanceTaxDetails.InsuredDomiciled = r_oClaimReceipt.AdvancedTaxDetails.InsuredDomiciled
                    oAdvanceTaxDetails.InsuredPercentage = r_oClaimReceipt.AdvancedTaxDetails.InsuredPercentage
                    oAdvanceTaxDetails.InsuredTaxNumber = r_oClaimReceipt.AdvancedTaxDetails.InsuredTaxNumber

                    oClaimReceipt.AdvancedTaxDetails = oAdvanceTaxDetails
                End If

                If r_oClaimReceipt.ReceiptItem IsNot Nothing Then
                    If r_oClaimReceipt.ReceiptItem.Count > 0 Then
                        oClaimReceipt.ReceiptItem = New List(Of BaseClasses.BaseClaimReceiptItemType)
                        For iRecovery As Integer = 0 To r_oClaimReceipt.ReceiptItem.Count - 1
                            If r_oClaimReceipt.ReceiptItem(iRecovery).ThisReceiptINCLTaxAmount <> 0 Then
                                oClaimReceiptItemType = New BaseClasses.BaseClaimReceiptItemType
                                oClaimReceiptItemType.BaseRecoveryKey = r_oClaimReceipt.ReceiptItem(iRecovery).RecoveryKey
                                oClaimReceiptItemType.ReceiptAmount = r_oClaimReceipt.ReceiptItem(iRecovery).ThisReceiptAmount ' Keep it same as ClaimReceipt call
                                oClaimReceiptItemType.TaxGroupCode = r_oClaimReceipt.ReceiptItem(iRecovery).TaxCode
                                oClaimReceiptItemType.IsTaxOverridden = r_oClaimReceipt.ReceiptItem(iRecovery).IsTaxOverridden
                                oClaimReceiptItemType.OverriddedTaxAmount = r_oClaimReceipt.ReceiptItem(iRecovery).OverriddedTaxAmount
                                oClaimReceipt.ReceiptItem.Add(oClaimReceiptItemType)

                            End If
                        Next

                    ElseIf r_oClaimReceipt.ClaimReceiptItem IsNot Nothing AndAlso r_oClaimReceipt.ClaimReceiptItem.Count > 0 Then
                        oClaimReceipt.ReceiptItem = New List(Of BaseClasses.BaseClaimReceiptItemType)
                        For iRecovery As Integer = 0 To r_oClaimReceipt.ClaimReceiptItem.Count - 1
                            If (r_oClaimReceipt.ClaimReceiptItem(iRecovery).ReceiptAmount > 0) AndAlso (r_oClaimReceipt.ClaimReceiptItem(iRecovery).ClaimReceiptItemKey = r_oClaimReceipt.ClaimReceiptItem(iRecovery).BaseClaimReceiptItemKey) Then

                                oClaimReceiptItemType = New BaseClasses.BaseClaimReceiptItemType
                                oClaimReceiptItemType.BaseRecoveryKey = r_oClaimReceipt.ClaimReceiptItem(iRecovery).BaseRecoveryKey
                                oClaimReceiptItemType.ReceiptAmount = r_oClaimReceipt.ClaimReceiptItem(iRecovery).ReceiptAmount
                                oClaimReceiptItemType.TaxGroupCode = r_oClaimReceipt.ClaimReceiptItem(iRecovery).TaxGroupCode
                                oClaimReceiptItemType.RecoveryTypeCode = r_oClaimReceipt.ClaimReceiptItem(iRecovery).RecoveryTypeCode
                                oClaimReceipt.ReceiptItem.Add(oClaimReceiptItemType)

                            End If
                        Next

                    End If


                End If
                oPayee.Name = r_oClaimReceipt.Payee.Name
                oPayee.BankName = r_oClaimReceipt.Payee.BankName
                oPayee.BankNumber = r_oClaimReceipt.Payee.BankNumber
                oPayee.BankCode = r_oClaimReceipt.Payee.BankCode
                oPayee.MediaTypeCode = r_oClaimReceipt.Payee.MediaTypeCode
                oPayee.MediaReference = r_oClaimReceipt.Payee.MediaReference
                oPayee.TheirReference = r_oClaimReceipt.Payee.TheirReference
                oPayee.Comments = r_oClaimReceipt.Payee.Comments
                If r_oClaimReceipt.Payee IsNot Nothing AndAlso r_oClaimReceipt.Payee.Address IsNot Nothing Then
                    oAddress.AddressLine1 = r_oClaimReceipt.Payee.Address.Address1
                    oAddress.AddressLine2 = r_oClaimReceipt.Payee.Address.Address2
                    oAddress.AddressLine3 = r_oClaimReceipt.Payee.Address.Address3
                    oAddress.AddressLine4 = r_oClaimReceipt.Payee.Address.Address4
                    oAddress.AddressTypeCode = r_oClaimReceipt.Payee.Address.AddressType
                    oAddress.CountryCode = r_oClaimReceipt.Payee.Address.CountryCode
                    oAddress.PostCode = r_oClaimReceipt.Payee.Address.PostCode

                End If
                oClaimReceipt.Payee = oPayee
                If (oClaimReceipt.Payee IsNot Nothing) Then
                    oClaimReceipt.Payee.Address = oAddress
                End If
                oClaimReceipt.IsSalvageRecovery = r_oClaimReceipt.IsSalvageRecovery

                .ClaimReceipt = oClaimReceipt
                .GetSavedTaxOfPeril = nGetSavedTaxOfPeril
            End With


            'Calling the SAM Method with Request Type
            'add trace to allow us to debug slow SAM calls
            Using trace As New Tracer(Category.Trace)
                SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.GetClaimReceiptTaxes, oGetClaimReceiptTaxesRequest)
                oGetClaimReceiptTaxesResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of BaseClasses.GetClaimReceiptTaxesQueryResponse)(result)
            End Using
            'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

            ' Disposing the SAM's object


            With oGetClaimReceiptTaxesResponse
                If .Errors IsNot Nothing Then
                    'Process the error object if errors, and throw as a single exception
                    Throw New NexusException(.Errors)
                Else
                    If .ReceiptItems IsNot Nothing Then

                    End If

                    If .Recoveries IsNot Nothing Then

                    End If
                    If .TaxItems IsNot Nothing AndAlso .TaxItems.Count > 0 Then

                        For Each oClaimReceiptTaxItem As BaseClasses.BaseClaimReceiptTaxItemType In .TaxItems

                            With oClaimReceiptTaxItem
                                oTaxItem = New BaseClaimTaxItemType
                                oTaxItem.Amount = .Amount
                                oTaxItem.Percentage = .Percentage
                                oTaxItem.RecoveryType = .RecoveryType
                                oTaxItem.TaxBandCode = .TaxBandCode
                                oTaxItem.TaxGroupCode = .TaxGroupCode
                            End With
                            r_oClaimReceipt.TaxItem.Add(oTaxItem)
                        Next
                    End If

                    r_oClaimReceipt.ReceiptToLossExchangeRate = .ReceiptToLossExchangeRate


                End If
            End With

            If Logger.IsLoggingEnabled Then

                sbLogMessage.AppendLine("GetClaimReceiptTaxes executed ok" & vbCrLf)
                sbLogMessage.AppendLine("Input:" & vbCrLf)
                sbLogMessage.AppendLine("r_oClaimReceiptRequestType = " & r_oClaimReceipt.Print.ToString() & vbCrLf)

                sbLogMessage.AppendLine("Output:" & vbCrLf)
                sbLogMessage.AppendLine("oClaimReceipt = " & r_oClaimReceipt.Print.ToString() & vbCrLf)

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
            oGetClaimReceiptTaxesRequest = Nothing
            oGetClaimReceiptTaxesResponse = Nothing
        End Try

    End Sub

    ''' <summary>
    ''' GetClaimReceiptTaxGroups
    ''' </summary>
    ''' <param name="v_sTypeCode"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetClaimReceiptTaxGroups(ByVal v_sTypeCode As String,
                                                Optional ByVal v_sBranchCode As String = Nothing) As TaxCollection
        Dim oGetClaimReceiptTaxGroupRequest As BaseClasses.GetClaimReceiptTaxGroupsQuery
        Dim oGetClaimReceiptTaxGroupResponse As BaseClasses.GetClaimReceiptTaxGroupsQueryResponse
        'Dim oTaxGroups As Tax
        Dim oTaxGroups As TaxCollection
        Dim oTax As Tax
        Dim sbLogMessage As StringBuilder

        Try
            oGetClaimReceiptTaxGroupRequest = New BaseClasses.GetClaimReceiptTaxGroupsQuery
            oGetClaimReceiptTaxGroupResponse = New BaseClasses.GetClaimReceiptTaxGroupsQueryResponse
            sbLogMessage = New StringBuilder

            With oGetClaimReceiptTaxGroupRequest
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

                .TypeCode = v_sTypeCode

            End With

            'Calling the SAM Method with Request Type
            'add trace to allow us to debug slow SAM calls
            Using trace As New Tracer(Category.Trace)
                ApiClient._tokenModel = GetApiTokendetails()
                Dim result As String = ApiClient.Get(ApiMethods.GetClaimReceiptTaxGroups, oGetClaimReceiptTaxGroupRequest)
                oGetClaimReceiptTaxGroupResponse = ApiClient.DeserializeJson(Of BaseClasses.GetClaimReceiptTaxGroupsQueryResponse)(result)
            End Using

            oTaxGroups = New TaxCollection

            With oGetClaimReceiptTaxGroupResponse
                If .TaxGroup IsNot Nothing AndAlso .TaxGroup.Count > 0 Then
                    For Each oTaxReceiptItem As BaseClasses.BaseGetClaimPaymentTaxGroupsResponseTypeRow In .TaxGroup
                        oTax = New Tax
                        oTax.Code = oTaxReceiptItem.Code
                        oTax.Description = oTaxReceiptItem.Description
                        oTax.IsWithHoldingTax = oTaxReceiptItem.IsWithholdingTax
                        oTaxGroups.Add(oTax)
                    Next
                End If
            End With
            If Logger.IsLoggingEnabled Then
                sbLogMessage.AppendLine("GetClaimReceiptTaxGroups executed ok" & vbCrLf)
                sbLogMessage.AppendLine("Input:" & vbCrLf)
                sbLogMessage.AppendLine(v_sTypeCode.Replace("<br />", vbCrLf))

                sbLogMessage.AppendLine("Output: " & vbCrLf)
                sbLogMessage.AppendLine(oTaxGroups.Print.Replace("<br />", vbCrLf))

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
            oGetClaimReceiptTaxGroupRequest = Nothing
            oGetClaimReceiptTaxGroupResponse = Nothing
        End Try

        Return oTaxGroups
    End Function


    ''' <summary>
    '''  This Method Return the DataSet for Claim Reinsurance, If RI2007 is OFF
    ''' </summary>
    ''' <param name="iClaimKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    ''' </summary>
    ''' <param name="iClaimKey"></param>
    ''' <param name="bIsRI2007"></param>
    ''' <param name="sReturnXML"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetClaimReinsurance(ByVal iClaimKey As Integer) As DataSet
        Dim oReinsurarerBandCollection As NexusProvider.ReinsuranceBandsCollection
        Dim oReinsuranceArrangements As NexusProvider.ReinsuranceArrangementLineCollection
        Dim oArrangementLinesTypeCollection As ArrangementLinesTypeCollection = Nothing
        Dim dsArrangementGridData As New DataSet
        Dim dsParticipentGridData As New DataSet
        Dim dtCurParticipent As New DataTable("Current_Broker_Participent")
        Dim oResource As ResXResourceReader
        Dim enResources As IDictionaryEnumerator
        Dim sLblAllocated As String
        Dim sLblFacTotal As String
        Dim sLblTreatyTotal As String
        Dim sLblBandTotal As String
        Dim drArrangementGridRow As DataRow
        Dim dtArrangements As DataTable
        Dim nArrangementId As Integer
        Dim dAllocatedReserveToDate, dAllocatedSumInsured, dAllocatedThisReserve, dAllocatedThisPayment, dAllocatedPaymentToDate, dAllocatedBalance, dAllocatedRecoverToDate, dAllocatedThisPercent As Double
        Dim dSectionTotalReserveToDate, dSectionTotalSumInsured, dSectionTotalThisReserve, dSectionTotalThisPayment, dSectionTotalPaymentToDate, dSectionTotalBalance, dSectionTotalRecoverToDate, dSectionTotalThisPercent As Double

        oResource = New ResXResourceReader(HttpContext.Current.Server.MapPath(AppSettings("WebRoot") & "Controls/App_LocalResources/ClaimReinsurance.ascx.resx"))
        enResources = oResource.GetEnumerator()


        While (enResources.MoveNext)
            If enResources.Key.ToString.Trim = "lbl_Allocated" Then
                sLblAllocated = enResources.Value
            ElseIf enResources.Key.ToString.Trim = "lbl_FacTotal" Then
                sLblFacTotal = enResources.Value
            ElseIf enResources.Key.ToString.Trim = "lbl_TreatyTotal" Then
                sLblTreatyTotal = enResources.Value
            ElseIf enResources.Key.ToString.Trim = "lbl_BandTotal" Then
                sLblBandTotal = enResources.Value
            End If
        End While

        oReinsurarerBandCollection = GetClaimReinsurancebands(iClaimKey)

        For Each oReinsuranceBands As NexusProvider.ReinsuranceBands In oReinsurarerBandCollection
            dtArrangements = New DataTable("Current_" & oReinsuranceBands.BandID.ToString)

            ' Obtaining the value of ArrangementsType for specific riskkey from SAM
            oReinsuranceArrangements = Nothing

            If Current.Session(CNMode) = Mode.ViewClaim Then
                oReinsuranceArrangements = GetClaimReinsuranceArrangements(iClaimKey, 0)
            Else
                '-1 is passed since user has not specified any mode
                oReinsuranceArrangements = GetClaimReinsuranceArrangements(iClaimKey, -1)
            End If


            dAllocatedReserveToDate = 0
            dAllocatedSumInsured = 0
            dAllocatedThisReserve = 0
            dAllocatedPaymentToDate = 0
            dAllocatedBalance = 0
            dAllocatedRecoverToDate = 0
            dAllocatedThisPayment = 0
            dAllocatedThisPercent = 0
            dAllocatedThisPayment = 0

            ' declaring table for adding into the dataset
            dtArrangements.Columns.Add("Placement", GetType(String))
            dtArrangements.Columns.Add("Name", GetType(String))
            dtArrangements.Columns.Add("DefaultPerc", GetType(String))
            dtArrangements.Columns.Add("ThisPerc", GetType(String))
            dtArrangements.Columns.Add("SumInsured", GetType(String))
            dtArrangements.Columns.Add("ReserveToDate", GetType(String))
            dtArrangements.Columns.Add("PaymentToDate", GetType(String))
            dtArrangements.Columns.Add("RecoverToDate", GetType(String))
            dtArrangements.Columns.Add("ThisReserve", GetType(String))
            dtArrangements.Columns.Add("ThisPayment", GetType(String))
            dtArrangements.Columns.Add("Balance", GetType(String))
            dtArrangements.Columns.Add("Agreement", GetType(String))
            If Not dsArrangementGridData.Tables.Contains(dtArrangements.TableName) Then
                dsArrangementGridData.Tables.Add(dtArrangements)
            End If

            ' Obtaining the value of ArrangementLinesType for specific riskkey from SAM
            For Each oArrangementType As NexusProvider.ReinsuranceArrangementLines In oReinsuranceArrangements

                If oArrangementType.BandId = oReinsuranceBands.BandID Then
                    nArrangementId = oArrangementType.ArrangementId

                    If Current.Session(CNMode) = Mode.ViewClaim Then
                        oArrangementLinesTypeCollection = GetClaimReinsuranceArrangementLines(iClaimKey, nArrangementId, 0)
                    Else
                        oArrangementLinesTypeCollection = GetClaimReinsuranceArrangementLines(iClaimKey, nArrangementId, -1)
                    End If

                    'Add rows for Band Totals
                    drArrangementGridRow = dsArrangementGridData.Tables("Current_" & oArrangementType.BandId.ToString).NewRow
                    drArrangementGridRow("Placement") = "GROSS"
                    drArrangementGridRow("Name") = sLblBandTotal
                    drArrangementGridRow("SumInsured") = New Money(CType(oArrangementType.SumInsured, Decimal), Current.Session(CNCurrenyCode)).Formatted
                    drArrangementGridRow("Balance") = New Money(CType(oArrangementType.Balance, Decimal), Current.Session(CNCurrenyCode)).Formatted
                    drArrangementGridRow("PaymentToDate") = New Money(CType(oArrangementType.PaymentToDate, Decimal), Current.Session(CNCurrenyCode)).Formatted
                    drArrangementGridRow("ReserveToDate") = New Money(CType(oArrangementType.ReserveToDate, Decimal), Current.Session(CNCurrenyCode)).Formatted
                    drArrangementGridRow("RecoverToDate") = New Money(CType(oArrangementType.RecoveryToDate, Decimal), Current.Session(CNCurrenyCode)).Formatted
                    drArrangementGridRow("ThisPayment") = New Money(CType(oArrangementType.ThisPayment, Decimal), Current.Session(CNCurrenyCode)).Formatted
                    drArrangementGridRow("ThisReserve") = New Money(CType(oArrangementType.ThisReserve, Decimal), Current.Session(CNCurrenyCode)).Formatted
                    dsArrangementGridData.Tables("Current_" & oArrangementType.BandId.ToString).Rows.Add(drArrangementGridRow)
                    drArrangementGridRow = Nothing

                    'Add rows for non fac lines
                    dSectionTotalSumInsured = 0
                    dSectionTotalReserveToDate = 0
                    dSectionTotalThisReserve = 0
                    dSectionTotalPaymentToDate = 0
                    dSectionTotalBalance = 0
                    dSectionTotalThisPercent = 0
                    dSectionTotalThisPayment = 0

                    Dim oReinsuranceArrangmentNonFacLines = From nonFacLines In oArrangementLinesTypeCollection Where nonFacLines.Type <> "F"
                                                            Select nonFacLines

                    For Each oArrangementLinesType As NexusProvider.ArrangementLinesType In oReinsuranceArrangmentNonFacLines

                        With oArrangementLinesType
                            drArrangementGridRow = dsArrangementGridData.Tables("Current_" & oReinsuranceBands.BandID.ToString).NewRow
                            drArrangementGridRow("Name") = .Name
                            drArrangementGridRow("DefaultPerc") = Format(Math.Round(.DefaultPerc * 100, 2), "0.00")
                            drArrangementGridRow("ThisPerc") = Format(Math.Round(.ThisPerc * 100, 2), "0.00")
                            drArrangementGridRow("SumInsured") = New Money(.SumInsured, Current.Session(CNCurrenyCode)).Formatted
                            drArrangementGridRow("ReserveToDate") = New Money(.ReserveToDate, Current.Session(CNCurrenyCode)).Formatted
                            drArrangementGridRow("RecoverToDate") = New Money(.RecoverToDate, Current.Session(CNCurrenyCode)).Formatted
                            drArrangementGridRow("PaymentToDate") = New Money(.PaymentToDate, Current.Session(CNCurrenyCode)).Formatted
                            drArrangementGridRow("ThisPayment") = New Money(.ThisPayment, Current.Session(CNCurrenyCode)).Formatted
                            drArrangementGridRow("ThisReserve") = New Money(.ThisReserve, Current.Session(CNCurrenyCode)).Formatted
                            drArrangementGridRow("Balance") = New Money(.Balance, Current.Session(CNCurrenyCode)).Formatted
                            drArrangementGridRow("Agreement") = .AgreementCode
                            dsArrangementGridData.Tables("Current_" & oArrangementType.BandId.ToString).Rows.Add(drArrangementGridRow)


                            dAllocatedSumInsured = dAllocatedSumInsured + .SumInsured
                            dAllocatedReserveToDate = dAllocatedReserveToDate + .ReserveToDate
                            dAllocatedThisReserve = dAllocatedThisReserve + .ThisReserve
                            dAllocatedPaymentToDate = dAllocatedPaymentToDate + .PaymentToDate
                            dAllocatedBalance = dAllocatedBalance + .Balance
                            dAllocatedThisPercent = dAllocatedThisPercent + .ThisPerc
                            dAllocatedThisPayment = dAllocatedThisPayment + .ThisPayment

                            dSectionTotalSumInsured = dSectionTotalSumInsured + .SumInsured
                            dSectionTotalReserveToDate = dSectionTotalReserveToDate + .ReserveToDate
                            dSectionTotalThisReserve = dSectionTotalThisReserve + .ThisReserve
                            dSectionTotalPaymentToDate = dSectionTotalPaymentToDate + .PaymentToDate
                            dSectionTotalBalance = dSectionTotalBalance + .Balance
                            dSectionTotalThisPercent = dSectionTotalThisPercent + .ThisPerc
                            dSectionTotalThisPayment = dSectionTotalThisPayment + .ThisPayment
                        End With
                    Next

                    'Totals for non fac lines
                    drArrangementGridRow = dsArrangementGridData.Tables("Current_" & oReinsuranceBands.BandID.ToString).NewRow
                    drArrangementGridRow("Name") = sLblTreatyTotal
                    drArrangementGridRow("ThisPerc") = Format(Math.Round(dSectionTotalThisPercent * 100, 2), "0.00")
                    drArrangementGridRow("SumInsured") = New Money(dSectionTotalSumInsured, Current.Session(CNCurrenyCode)).Formatted
                    drArrangementGridRow("ReserveToDate") = New Money(dSectionTotalReserveToDate, Current.Session(CNCurrenyCode)).Formatted
                    drArrangementGridRow("ThisPayment") = New Money(dSectionTotalThisPayment, Current.Session(CNCurrenyCode)).Formatted
                    drArrangementGridRow("PaymentToDate") = New Money(dSectionTotalPaymentToDate, Current.Session(CNCurrenyCode)).Formatted
                    drArrangementGridRow("ThisReserve") = New Money(dSectionTotalThisReserve, Current.Session(CNCurrenyCode)).Formatted
                    drArrangementGridRow("Balance") = New Money(dSectionTotalBalance, Current.Session(CNCurrenyCode)).Formatted
                    dsArrangementGridData.Tables("Current_" & oArrangementType.BandId.ToString).Rows.Add(drArrangementGridRow)

                    'Add rows for fac lines
                    dSectionTotalSumInsured = 0
                    dSectionTotalReserveToDate = 0
                    dSectionTotalThisReserve = 0
                    dSectionTotalPaymentToDate = 0
                    dSectionTotalBalance = 0
                    dSectionTotalThisPercent = 0
                    dSectionTotalThisPayment = 0

                    Dim oReinsuranceArrangmentFacLines = From nonFacLines In oArrangementLinesTypeCollection Where nonFacLines.Type = "F"
                                                         Select nonFacLines

                    For Each oArrangementLinesType As NexusProvider.ArrangementLinesType In oReinsuranceArrangmentFacLines

                        With oArrangementLinesType
                            drArrangementGridRow = dsArrangementGridData.Tables("Current_" & oReinsuranceBands.BandID.ToString).NewRow
                            drArrangementGridRow("Name") = .Name
                            drArrangementGridRow("DefaultPerc") = Format(Math.Round(.DefaultPerc * 100, 2), "0.00")
                            drArrangementGridRow("ThisPerc") = Format(Math.Round(.ThisPerc * 100, 2), "0.00")
                            drArrangementGridRow("SumInsured") = New Money(.SumInsured, Current.Session(CNCurrenyCode)).Formatted
                            drArrangementGridRow("ReserveToDate") = New Money(.ReserveToDate, Current.Session(CNCurrenyCode)).Formatted
                            drArrangementGridRow("RecoverToDate") = New Money(.RecoverToDate, Current.Session(CNCurrenyCode)).Formatted
                            drArrangementGridRow("PaymentToDate") = New Money(.PaymentToDate, Current.Session(CNCurrenyCode)).Formatted
                            drArrangementGridRow("ThisPayment") = New Money(.ThisPayment, Current.Session(CNCurrenyCode)).Formatted
                            drArrangementGridRow("ThisReserve") = New Money(.ThisReserve, Current.Session(CNCurrenyCode)).Formatted
                            drArrangementGridRow("Balance") = New Money(.Balance, Current.Session(CNCurrenyCode)).Formatted
                            drArrangementGridRow("Agreement") = .AgreementCode
                            dsArrangementGridData.Tables("Current_" & oArrangementType.BandId.ToString).Rows.Add(drArrangementGridRow)

                            dAllocatedSumInsured = dAllocatedSumInsured + CDbl(.SumInsured)
                            dAllocatedReserveToDate = dAllocatedReserveToDate + CDbl(.ReserveToDate)
                            dAllocatedThisReserve = dAllocatedThisReserve + CDbl(.ThisReserve)
                            dAllocatedPaymentToDate = dAllocatedPaymentToDate + CDbl(.PaymentToDate)
                            dAllocatedBalance = dAllocatedBalance + CDbl(.Balance)
                            dAllocatedThisPercent = dAllocatedThisPercent + CDbl(.ThisPerc)
                            dAllocatedThisPayment = dAllocatedThisPayment + .ThisPayment

                            dSectionTotalSumInsured = dSectionTotalSumInsured + CDbl(.SumInsured)
                            dSectionTotalReserveToDate = dSectionTotalReserveToDate + CDbl(.ReserveToDate)
                            dSectionTotalThisReserve = dSectionTotalThisReserve + CDbl(.ThisReserve)
                            dSectionTotalPaymentToDate = dSectionTotalPaymentToDate + CDbl(.PaymentToDate)
                            dSectionTotalBalance = dSectionTotalBalance + CDbl(.Balance)
                            dSectionTotalThisPercent = dSectionTotalThisPercent + .ThisPerc
                            dSectionTotalThisPayment = dSectionTotalThisPayment + .ThisPayment

                        End With
                    Next

                    'Add row for FAC totals
                    drArrangementGridRow = dsArrangementGridData.Tables("Current_" & oReinsuranceBands.BandID.ToString).NewRow
                    drArrangementGridRow("Name") = sLblFacTotal
                    drArrangementGridRow("ThisPerc") = Format(Math.Round(dSectionTotalThisPercent * 100, 2), "0.00")
                    drArrangementGridRow("SumInsured") = New Money(dSectionTotalSumInsured, Current.Session(CNCurrenyCode)).Formatted
                    drArrangementGridRow("ReserveToDate") = New Money(dSectionTotalReserveToDate, Current.Session(CNCurrenyCode)).Formatted
                    drArrangementGridRow("ThisPayment") = New Money(dSectionTotalThisPayment, Current.Session(CNCurrenyCode)).Formatted
                    drArrangementGridRow("PaymentToDate") = New Money(dSectionTotalPaymentToDate, Current.Session(CNCurrenyCode)).Formatted
                    drArrangementGridRow("ThisReserve") = New Money(dSectionTotalThisReserve, Current.Session(CNCurrenyCode)).Formatted
                    drArrangementGridRow("Balance") = New Money(dSectionTotalBalance, Current.Session(CNCurrenyCode)).Formatted
                    dsArrangementGridData.Tables("Current_" & oArrangementType.BandId.ToString).Rows.Add(drArrangementGridRow)
                End If
            Next

            'For Allocated Row
            drArrangementGridRow = dsArrangementGridData.Tables("Current_" & oReinsuranceBands.BandID.ToString).NewRow
            drArrangementGridRow("Name") = sLblAllocated
            drArrangementGridRow("ThisPerc") = Format(Math.Round(dAllocatedThisPercent * 100, 2), "0.00")
            drArrangementGridRow("ReserveToDate") = New Money(CType(dAllocatedReserveToDate, Decimal), Current.Session(CNCurrenyCode)).Formatted
            drArrangementGridRow("SumInsured") = New Money(CType(dAllocatedSumInsured, Decimal), Current.Session(CNCurrenyCode)).Formatted
            drArrangementGridRow("ThisReserve") = New Money(CType(dAllocatedThisReserve, Decimal), Current.Session(CNCurrenyCode)).Formatted
            drArrangementGridRow("PaymentToDate") = New Money(CType(dAllocatedPaymentToDate, Decimal), Current.Session(CNCurrenyCode)).Formatted
            drArrangementGridRow("ThisPayment") = New Money(CType(dAllocatedThisPayment, Decimal), Current.Session(CNCurrenyCode)).Formatted
            drArrangementGridRow("Balance") = New Money(CType(dAllocatedBalance, Decimal), Current.Session(CNCurrenyCode)).Formatted
            drArrangementGridRow("RecoverToDate") = New Money(CType(dAllocatedRecoverToDate, Decimal), Current.Session(CNCurrenyCode)).Formatted
            dsArrangementGridData.Tables("Current_" & oReinsuranceBands.BandID.ToString).Rows.Add(drArrangementGridRow)
            drArrangementGridRow = Nothing
        Next

        Return dsArrangementGridData
    End Function

    ''' <summary>
    ''' This Method Return the XML for Claim Reinsurance, If RI2007 is ON
    ''' </summary>
    ''' <param name="iClaimKey"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetClaimReinsurance2007(ByVal iClaimKey As Integer) As String
        Dim sReturnXML As String = Nothing
        Dim dsArrangementGridData As DataSet
        dsArrangementGridData = ClaimReinsurance2007(iClaimKey, sReturnXML)
        Return sReturnXML
    End Function

    ''' <summary>
    ''' THis will return the XML for claim RI- RI 2007 should be enabled 
    ''' </summary>
    ''' <param name="iClaimKey"></param>
    ''' <param name="sReturnXML"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ClaimReinsurance2007(ByVal iClaimKey As Integer, Optional ByRef sReturnXML As String = Nothing) As DataSet
        Dim oReinsurarerBandCollection As NexusProvider.ReinsuranceBandsCollection
        Dim dsArrangementGridData As New DataSet
        Dim dsParticipentGridData As New DataSet
        Dim dtCurParticipent As New DataTable("Current_Broker_Participent")
        Dim dtCurFAXBrokerParticipent As New DataTable("Current_FAX_Broker_Participent")
        Dim dtCurFAXParticipent As New DataTable("Current_FAX_Participent")


        'FAX Participent Table
        dtCurFAXParticipent.Columns.Add("IsNew", GetType(Boolean))
        dtCurFAXParticipent.Columns.Add("IsEdited", GetType(Boolean))
        dtCurFAXParticipent.Columns.Add("IsDeleted", GetType(Boolean))
        dtCurFAXParticipent.Columns.Add("PartyKey", GetType(Integer))
        dtCurFAXParticipent.Columns.Add("ParticipationPercent", GetType(Double))
        dtCurFAXParticipent.Columns.Add("PartyCode", GetType(String))
        dtCurFAXParticipent.Columns.Add("PartyName", GetType(String))
        dtCurFAXParticipent.Columns.Add("AccountType", GetType(String))
        dtCurFAXParticipent.Columns.Add("AgreementCode", GetType(String))
        dtCurFAXParticipent.Columns.Add("SumInsured", GetType(Double))
        dtCurFAXParticipent.Columns.Add("PaymentToDate", GetType(Double))
        dtCurFAXParticipent.Columns.Add("RecoverToDate", GetType(Double))
        dtCurFAXParticipent.Columns.Add("ReserveToDate", GetType(Double))
        dtCurFAXParticipent.Columns.Add("ThisPayment", GetType(Double))
        dtCurFAXParticipent.Columns.Add("ThisReserve", GetType(Double))
        dtCurFAXParticipent.Columns.Add("ActionType", GetType(RowAction))
        dtCurFAXParticipent.Columns.Add("RIArrangementLineKey", GetType(Integer))
        dtCurFAXParticipent.Columns.Add("RIarrangementKey", GetType(Integer))
        dtCurFAXParticipent.Columns.Add("Grouping", GetType(Integer))
        dsParticipentGridData.Tables.Add(dtCurFAXParticipent)

        'FAX Broker Participent Table
        dtCurFAXBrokerParticipent.Columns.Add("IsNew", GetType(Boolean))
        dtCurFAXBrokerParticipent.Columns.Add("IsEdited", GetType(Boolean))
        dtCurFAXBrokerParticipent.Columns.Add("IsDeleted", GetType(Boolean))
        dtCurFAXBrokerParticipent.Columns.Add("PartyKey", GetType(Integer))
        dtCurFAXBrokerParticipent.Columns.Add("ParticipationPercent", GetType(Double))
        dtCurFAXBrokerParticipent.Columns.Add("RIArrangementLineKey", GetType(Integer))
        dtCurFAXBrokerParticipent.Columns.Add("RIarrangementKey", GetType(Integer))
        dtCurFAXBrokerParticipent.Columns.Add("PartyCode", GetType(String))
        dtCurFAXBrokerParticipent.Columns.Add("PartyName", GetType(String))
        dsParticipentGridData.Tables.Add(dtCurFAXBrokerParticipent)

        'Broker Paricipent Table
        dtCurParticipent.Columns.Add("IsNew", GetType(Boolean))
        dtCurParticipent.Columns.Add("IsEdited", GetType(Boolean))
        dtCurParticipent.Columns.Add("IsDeleted", GetType(Boolean))
        dtCurParticipent.Columns.Add("PartyKey", GetType(Integer))
        dtCurParticipent.Columns.Add("ParticipationPercent", GetType(Double))
        dtCurParticipent.Columns.Add("RIArrangementLineKey", GetType(Integer))
        dtCurParticipent.Columns.Add("RIarrangementKey", GetType(Integer))
        dtCurParticipent.Columns.Add("PartyCode", GetType(String))
        dtCurParticipent.Columns.Add("PartyName", GetType(String))
        dsParticipentGridData.Tables.Add(dtCurParticipent)

        oReinsurarerBandCollection = GetClaimReinsurancebands(iClaimKey)

        For Each oReinsuranceBands As NexusProvider.ReinsuranceBands In oReinsurarerBandCollection
            Dim drArrangementGridRow As DataRow
            Dim dtArrangements As New DataTable("Current_" & oReinsuranceBands.BandID.ToString)
            Dim iArrangementId As Integer
            ' Obtaining the value of ArrangementsType for specific riskkey from SAM
            Dim oReinsuranceArrangementLineCollection As NexusProvider.ReinsuranceArrangementLineCollection = Nothing

            If Current.Session(CNMode) = Mode.ViewClaim Then
                oReinsuranceArrangementLineCollection = GetClaimReinsuranceArrangements(iClaimKey, 0)
            Else
                '-1 is passed since user has not specified any mode
                oReinsuranceArrangementLineCollection = GetClaimReinsuranceArrangements(iClaimKey, -1)
            End If

            Dim dAllocatedReserveToDate, dAllocatedSumInsured, dAllocatedThisReserve, dAllocatedThisPayment, dAllocatedPaymentToDate, dAllocatedBalance As Double
            Dim dAllocatedRecoverToDate As Double

            dAllocatedReserveToDate = 0
            dAllocatedSumInsured = 0
            dAllocatedThisReserve = 0
            dAllocatedPaymentToDate = 0
            dAllocatedBalance = 0
            dAllocatedRecoverToDate = 0
            dAllocatedThisPayment = 0


            ' declaring table for adding into the dataset
            ' declaring table for adding into the dataset
            dtArrangements.Columns.Add("IsNew", GetType(Boolean))
            dtArrangements.Columns.Add("IsEdited", GetType(Boolean))
            dtArrangements.Columns.Add("IsDeleted", GetType(Boolean))
            dtArrangements.Columns.Add("ActionType", GetType(RowAction))
            dtArrangements.Columns.Add("AgreementCode", GetType(String))
            dtArrangements.Columns.Add("CedePremiumOnly", GetType(Boolean))
            dtArrangements.Columns.Add("DefaultSharePercent", GetType(Double))
            dtArrangements.Columns.Add("Grouping", GetType(Integer))
            dtArrangements.Columns.Add("IsDomiciledForTax", GetType(Boolean))
            dtArrangements.Columns.Add("IsRIBroker", GetType(Boolean))
            dtArrangements.Columns.Add("LineLimit", GetType(Double))
            dtArrangements.Columns.Add("LowerLimit", GetType(Double))
            dtArrangements.Columns.Add("NumberOfLines", GetType(Decimal))
            dtArrangements.Columns.Add("PartyKey", GetType(Integer))
            dtArrangements.Columns.Add("Priority", GetType(Integer))
            dtArrangements.Columns.Add("ReinsuranceTypeCode", GetType(String))
            dtArrangements.Columns.Add("Retained", GetType(Double))
            dtArrangements.Columns.Add("RIArrangementKey", GetType(Integer))
            dtArrangements.Columns.Add("RIArrangementLineKey", GetType(Integer))
            dtArrangements.Columns.Add("Placement", GetType(String))
            dtArrangements.Columns.Add("TreatyCode", GetType(String))
            dtArrangements.Columns.Add("Type", GetType(String))
            dtArrangements.Columns.Add("RecoverToDate", GetType(Double))
            dtArrangements.Columns.Add("ThisPayment", GetType(Double))
            dtArrangements.Columns.Add("Name", GetType(String))
            dtArrangements.Columns.Add("DefaultPerc", GetType(Double))
            dtArrangements.Columns.Add("ThisPerc", GetType(Double))
            dtArrangements.Columns.Add("SumInsured", GetType(Double))
            dtArrangements.Columns.Add("ReserveToDate", GetType(Double))
            dtArrangements.Columns.Add("PaymentToDate", GetType(Double))
            dtArrangements.Columns.Add("ThisReserve", GetType(Double))
            dtArrangements.Columns.Add("Balance", GetType(Double))
            dtArrangements.Columns.Add("Agreement", GetType(String))
            dtArrangements.Columns.Add("Incurred", GetType(Double))
            dtArrangements.Columns.Add("IsObligatory", GetType(Boolean))
            If Not dsArrangementGridData.Tables.Contains(dtArrangements.TableName) Then
                dsArrangementGridData.Tables.Add(dtArrangements)
            End If

            ' Obtaining the value of ArrangementLinesType for specific riskkey from SAM
            For Each oArrangementType As NexusProvider.ReinsuranceArrangementLines In oReinsuranceArrangementLineCollection

                If oArrangementType.BandId = oReinsuranceBands.BandID Then
                    iArrangementId = oArrangementType.ArrangementId
                    Dim oArrangementLinesTypeCollection As ArrangementLinesTypeCollection

                    If Current.Session(CNMode) = Mode.ViewClaim Then
                        Dim oClaim As NexusProvider.Claim = Current.Session(CNClaim)
                        If oClaim.IsRecovery = True Then
                            oArrangementLinesTypeCollection = GetClaimRIArrangementLinesRI2007(iClaimKey, iArrangementId, 0, True)
                        Else
                            oArrangementLinesTypeCollection = GetClaimRIArrangementLinesRI2007(iClaimKey, iArrangementId, 0, False)
                        End If
                    ElseIf Current.Session(CNMode) = Mode.TPRecovery Or Current.Session(CNMode) = Mode.SalvageClaim Then
                        '-1 is passed since user has not specified any mode
                        oArrangementLinesTypeCollection = GetClaimRIArrangementLinesRI2007(iClaimKey, iArrangementId, -1, True)
                    Else
                        '-1 is passed since user has not specified any mode
                        oArrangementLinesTypeCollection = GetClaimRIArrangementLinesRI2007(iClaimKey, iArrangementId, -1, False)
                    End If

                    drArrangementGridRow = dsArrangementGridData.Tables("Current_" & oArrangementType.BandId.ToString).NewRow
                    drArrangementGridRow("Placement") = "GROSS"
                    drArrangementGridRow("Name") = "Band Total"
                    drArrangementGridRow("SumInsured") = oArrangementType.SumInsured
                    drArrangementGridRow("Balance") = oArrangementType.Balance
                    drArrangementGridRow("PaymentToDate") = oArrangementType.PaymentToDate
                    drArrangementGridRow("ReserveToDate") = oArrangementType.ReserveToDate
                    drArrangementGridRow("RecoverToDate") = oArrangementType.RecoveryToDate
                    drArrangementGridRow("ThisPayment") = oArrangementType.ThisPayment
                    drArrangementGridRow("ThisReserve") = oArrangementType.ThisReserve

                    dsArrangementGridData.Tables("Current_" & oArrangementType.BandId.ToString).Rows.Add(drArrangementGridRow)
                    drArrangementGridRow = Nothing

                    ' Clear existing rows for this arrangement to prevent duplicates
                    Dim tableName As String = "Current_" & oReinsuranceBands.BandID.ToString
                    If dsArrangementGridData.Tables.Contains(tableName) Then
                        ' Remove existing rows that match this arrangement to prevent duplicates
                        Dim rowsToRemove As New List(Of DataRow)
                        For Each existingRow As DataRow In dsArrangementGridData.Tables(tableName).Rows
                            If Not IsDBNull(existingRow("RIArrangementKey")) AndAlso existingRow("RIArrangementKey").ToString() = iArrangementId.ToString() Then
                                rowsToRemove.Add(existingRow)
                            End If
                        Next
                        For Each rowToRemove As DataRow In rowsToRemove
                            dsArrangementGridData.Tables(tableName).Rows.Remove(rowToRemove)
                        Next
                    End If

                    For Each oArrangementLinesType As NexusProvider.ArrangementLinesType In oArrangementLinesTypeCollection
                        With oArrangementLinesType
                            drArrangementGridRow = dsArrangementGridData.Tables("Current_" & oReinsuranceBands.BandID.ToString).NewRow
                            drArrangementGridRow("IsNew") = False
                            drArrangementGridRow("IsEdited") = False
                            drArrangementGridRow("IsDeleted") = False
                            drArrangementGridRow("ActionType") = .ActionType
                            drArrangementGridRow("AgreementCode") = .AgreementCode
                            drArrangementGridRow("CedePremiumOnly") = .CedePremiumOnly
                            drArrangementGridRow("Grouping") = .Grouping
                            drArrangementGridRow("IsDomiciledForTax") = .IsDomiciledForTax
                            drArrangementGridRow("IsRIBroker") = .IsRIBroker
                            drArrangementGridRow("LineLimit") = .LineLimit
                            drArrangementGridRow("LowerLimit") = .LowerLimit
                            drArrangementGridRow("NumberOfLines") = .NumberOfLines
                            drArrangementGridRow("PartyKey") = .PartyKey
                            drArrangementGridRow("Priority") = .Priority
                            drArrangementGridRow("ReinsuranceTypeCode") = .ReinsuranceTypeCode
                            drArrangementGridRow("Retained") = .Retained
                            drArrangementGridRow("RIArrangementKey") = .RIarrangementKey
                            drArrangementGridRow("RIArrangementLineKey") = .RIArrangementLineKey
                            drArrangementGridRow("Placement") = .RIPlacement
                            drArrangementGridRow("TreatyCode") = .TreatyCode
                            drArrangementGridRow("Type") = .Type
                            drArrangementGridRow("RecoverToDate") = .RecoverToDate
                            drArrangementGridRow("ThisPayment") = .ThisPayment
                            drArrangementGridRow("Name") = .RIName
                            drArrangementGridRow("DefaultPerc") = .DefaultPerc
                            drArrangementGridRow("ThisPerc") = .ThisPerc
                            drArrangementGridRow("SumInsured") = .SumInsured
                            drArrangementGridRow("ReserveToDate") = .ReserveToDate
                            drArrangementGridRow("PaymentToDate") = .PaymentToDate
                            drArrangementGridRow("ThisReserve") = .ThisReserve
                            drArrangementGridRow("Balance") = .Balance
                            drArrangementGridRow("Agreement") = .AgreementCode
                            drArrangementGridRow("Incurred") = .Incurred
                            drArrangementGridRow("IsObligatory") = .IsObligatory

                            dAllocatedSumInsured = dAllocatedSumInsured + .SumInsured
                            dAllocatedReserveToDate = dAllocatedReserveToDate + .ReserveToDate
                            dAllocatedThisReserve = dAllocatedThisReserve + .ThisReserve
                            dAllocatedPaymentToDate = dAllocatedPaymentToDate + .PaymentToDate
                            dAllocatedRecoverToDate = dAllocatedRecoverToDate + .RecoverToDate
                            dAllocatedBalance = dAllocatedBalance + .Balance
                            dAllocatedThisPayment = dAllocatedThisPayment + .ThisPayment

                            ' FAX Participent (FAC XOL)
                            If .FAXParticipants IsNot Nothing AndAlso .FAXParticipants.Count > 0 Then
                                For Each oFAXPaticipent As FAXParticipants In .FAXParticipants
                                    Dim drFAXParticipent As DataRow
                                    With oFAXPaticipent
                                        drFAXParticipent = dsParticipentGridData.Tables("Current_FAX_Participent").NewRow
                                        drFAXParticipent("PartyKey") = .PartyKey
                                        drFAXParticipent("ParticipationPercent") = .ParticipationPercentage
                                        drFAXParticipent("PartyCode") = .PartyCode
                                        drFAXParticipent("PartyName") = .PartyName
                                        drFAXParticipent("AccountType") = .AccountType
                                        drFAXParticipent("AgreementCode") = .AgreementCode
                                        drFAXParticipent("SumInsured") = .SumInsured
                                        drFAXParticipent("PaymentToDate") = .PaymentToDate
                                        drFAXParticipent("RecoverToDate") = .RecoverToDate
                                        drFAXParticipent("ReserveToDate") = .ReserveToDate
                                        drFAXParticipent("ThisPayment") = .ThisPayment
                                        drFAXParticipent("ThisReserve") = .ThisReserve
                                        drFAXParticipent("ActionType") = .ActionType
                                        drFAXParticipent("RIArrangementLineKey") = .RIArrangementLineKey
                                        drFAXParticipent("RIarrangementKey") = oArrangementLinesType.RIarrangementKey
                                        drFAXParticipent("Grouping") = oArrangementLinesType.Grouping

                                        'FAX Broker Participent
                                        If .BrokerParticipants IsNot Nothing AndAlso .BrokerParticipants.Count > 0 Then
                                            For Each oPaticipent As BrokerParticipants In .BrokerParticipants
                                                Dim drParticipent As DataRow
                                                With oPaticipent
                                                    drParticipent = dsParticipentGridData.Tables("Current_FAX_Broker_Participent").NewRow
                                                    drParticipent("PartyCode") = .PartyCode
                                                    drParticipent("PartyName") = .PartyName
                                                    drParticipent("PartyKey") = .PartyKey
                                                    drParticipent("ParticipationPercent") = .ParticipationPercentage
                                                    drParticipent("RIArrangementLineKey") = oFAXPaticipent.RIArrangementLineKey
                                                    drParticipent("RIarrangementKey") = oArrangementLinesType.RIarrangementKey
                                                End With

                                                dsParticipentGridData.Tables("Current_FAX_Broker_Participent").Rows.Add(drParticipent)
                                            Next
                                        End If
                                    End With
                                    dsParticipentGridData.Tables("Current_FAX_Participent").Rows.Add(drFAXParticipent)
                                Next
                            End If

                            'Broker Participent (FAC PROP)
                            If .BrokerParticipants IsNot Nothing AndAlso .BrokerParticipants.Count > 0 Then
                                For Each oPaticipent As BrokerParticipants In .BrokerParticipants
                                    Dim drParticipent As DataRow
                                    With oPaticipent
                                        drParticipent = dsParticipentGridData.Tables("Current_Broker_Participent").NewRow
                                        drParticipent("IsNew") = False
                                        drParticipent("PartyCode") = .PartyCode
                                        drParticipent("PartyName") = .PartyName
                                        drParticipent("PartyKey") = .PartyKey
                                        drParticipent("ParticipationPercent") = .ParticipationPercentage
                                        drParticipent("RIArrangementLineKey") = oArrangementLinesType.RIArrangementLineKey
                                        drParticipent("RIarrangementKey") = oArrangementLinesType.RIarrangementKey
                                    End With

                                    dsParticipentGridData.Tables("Current_Broker_Participent").Rows.Add(drParticipent)
                                Next
                            End If
                        End With

                        ' adding rows to the table
                        dsArrangementGridData.Tables("Current_" & oReinsuranceBands.BandID.ToString).Rows.Add(drArrangementGridRow)
                    Next
                End If
            Next

            Dim oResource As ResXResourceReader
            Dim en As IDictionaryEnumerator

            oResource = New ResXResourceReader(HttpContext.Current.Server.MapPath(AppSettings("WebRoot") & "Controls/App_LocalResources/ClaimReinsurance2007.ascx.resx"))
            en = oResource.GetEnumerator()

            'Allocated
            drArrangementGridRow = dsArrangementGridData.Tables("Current_" & oReinsuranceBands.BandID.ToString).NewRow
            While (en.MoveNext)
                If en.Key.ToString.Trim = "lbl_Allocated" Then
                    drArrangementGridRow("Name") = en.Value
                    Exit While
                End If
            End While

            drArrangementGridRow("ReserveToDate") = dAllocatedReserveToDate
            drArrangementGridRow("SumInsured") = dAllocatedSumInsured
            drArrangementGridRow("ThisReserve") = dAllocatedThisReserve
            drArrangementGridRow("ThisPayment") = dAllocatedThisPayment
            drArrangementGridRow("PaymentToDate") = dAllocatedPaymentToDate
            drArrangementGridRow("Balance") = dAllocatedBalance
            drArrangementGridRow("RecoverToDate") = dAllocatedRecoverToDate
            dsArrangementGridData.Tables("Current_" & oReinsuranceBands.BandID.ToString).Rows.Add(drArrangementGridRow)
            drArrangementGridRow = Nothing
        Next

        'Need to convert it into XML
        Dim oXMLDoc As New XmlDocument

        ' Write down the XML declaration
        Dim xmlDeclaration As XmlDeclaration = oXMLDoc.CreateXmlDeclaration("1.0", "utf-8", Nothing)

        ' Create the root element
        Dim rootNode As XmlElement = oXMLDoc.CreateElement("rows")
        oXMLDoc.InsertBefore(xmlDeclaration, oXMLDoc.DocumentElement)
        oXMLDoc.AppendChild(rootNode)

        For Each oRITable As DataTable In dsArrangementGridData.Tables

            ' Create the root element
            Dim sElement As String = "RIBAND"
            Dim RIBand As XmlElement = oXMLDoc.CreateElement(sElement)
            RIBand.SetAttribute("Name", oRITable.TableName)
            rootNode.AppendChild(RIBand)

            oRITable = GetDistinctRows(oRITable)

            If oRITable.Rows.Count > 0 Then
                'First Gross 
                ClaimRIArrangementRow_Others(oRITable, oXMLDoc, RIBand, dsParticipentGridData, "GROSS")
                'IsObligatory
                If CheckIsObligatory(oRITable) Then
                    'Treaty QSh should be added
                    RIArrangementRow_FAC(oRITable, oXMLDoc, RIBand, dsParticipentGridData, "Treaty QSH")
                End If
                'FAX XOL should be added
                RIArrangementRow_FAC(oRITable, oXMLDoc, RIBand, dsParticipentGridData, "FAC XOL")
                'FAC Prop should be added
                RIArrangementRow_FAC(oRITable, oXMLDoc, RIBand, dsParticipentGridData, "FAC Prop")
                'Calculate Net FAC
                CalculateClaimFACNet(oRITable, oXMLDoc, RIBand)
                'add other treaties
                ClaimRIArrangementRow_Others(oRITable, oXMLDoc, RIBand, dsParticipentGridData, "")
                'Treaty Surplus should be added
                'Treaty XOL should be added
                'RIArrangementRow_FAC(oRITable, oXMLDoc, RIBand, dsParticipentGridData, "Treaty XOL")
                '''Treaty CAT should be added
                'RIArrangementRow_FAC(oRITable, oXMLDoc, RIBand, dsParticipentGridData, "Treaty CAT")
                'Calculate UnAllocated
                CalculateClaimUnAllocated(oRITable, oXMLDoc, RIBand)
            End If
        Next

        sReturnXML = oXMLDoc.OuterXml

        Return dsArrangementGridData
    End Function

    Private Function GetDistinctRows(ByVal sourceTable As DataTable) As DataTable
        Dim distinctTable As DataTable = sourceTable.Clone()
        Dim uniqueRows As New HashSet(Of String)
        Dim allocatedRow As DataRow = Nothing
        For Each row As DataRow In sourceTable.Rows
            Dim rowKey As String = String.Join("|", row.ItemArray.Select(Function(x) If(x Is Nothing, "", x.ToString())))
            If Not uniqueRows.Contains(rowKey) Then
                uniqueRows.Add(rowKey)
                If row("Name") IsNot Nothing AndAlso row("Name").ToString().Contains("Allocated") Then
                    allocatedRow = row
                Else
                    distinctTable.ImportRow(row)
                End If
            End If
        Next
        If allocatedRow IsNot Nothing Then
            distinctTable.ImportRow(allocatedRow)
        End If
        Return distinctTable
    End Function

    ''' <summary>
    ''' To get claim reinsurance arrangement lines
    ''' </summary>
    ''' <param name="v_iClaimID"></param>
    ''' <param name="v_iArrangementID"></param>
    ''' <param name="v_iMode"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetClaimReinsuranceArrangementLines(ByVal v_iClaimID As Integer,
                                                        ByVal v_iArrangementID As Integer,
                                                        ByVal v_iMode As Integer,
                                                        Optional ByVal v_sBranchCode As String = Nothing) As ArrangementLinesTypeCollection
        Dim oGetReinsuranceArrangementLinesRequest As BaseClasses.GetClaimReinsuranceArrangementLinesQuery
        Dim oGetReinsuranceArrangementLinesResponse As BaseClasses.GetClaimReinsuranceArrangementLinesQueryResponse
        Dim oRALinesCollection As ReinsuranceArrangementLineCollection
        Dim oRALinesItemType As ReinsuranceArrangementLines
        Dim sbLogMessage As StringBuilder
        Dim oArrangementLines As ArrangementLinesType
        Dim oArrangementLinesCollection As New ArrangementLinesTypeCollection

        Try
            oGetReinsuranceArrangementLinesRequest = New BaseClasses.GetClaimReinsuranceArrangementLinesQuery
            oGetReinsuranceArrangementLinesResponse = New BaseClasses.GetClaimReinsuranceArrangementLinesQueryResponse
            sbLogMessage = New StringBuilder


            With oGetReinsuranceArrangementLinesRequest
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
                If v_iClaimID > 0 Then
                    .ClaimId = v_iClaimID
                Else
                    Throw New ArgumentException("ClaimID")
                End If
                If v_iArrangementID > 0 Then
                    .ArrangementId = v_iArrangementID
                Else
                    Throw New ArgumentException("ArrangementID")
                End If

                If v_iMode < 0 Then
                    .ModeSpecified = False
                Else
                    .Mode = v_iMode
                    .ModeSpecified = True
                End If
            End With


            'Calling the SAM Method with Request Type
            'add trace to allow us to debug slow SAM calls
            Using trace As New Tracer(Category.Trace)
                ApiClient._tokenModel = GetApiTokendetails()
                Dim result As String = ApiClient.Get(ApiMethods.GetClaimReinsuranceArrangementLines, oGetReinsuranceArrangementLinesRequest)
                oGetReinsuranceArrangementLinesResponse = ApiClient.DeserializeJson(Of BaseClasses.GetClaimReinsuranceArrangementLinesQueryResponse)(result)
            End Using

            'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

            ' Disposing the SAM's object

            oRALinesCollection = New ReinsuranceArrangementLineCollection

            With oGetReinsuranceArrangementLinesResponse
                If .ReinsuranceArrangementLines IsNot Nothing Then
                    For Each oRALinesItem As BaseClasses.BaseGetClaimReinsuranceArrangementLinesResponseTypeRow In .ReinsuranceArrangementLines
                        oArrangementLines = New ArrangementLinesType
                        With oArrangementLines
                            .Type = oRALinesItem.Type
                            .Name = oRALinesItem.Name
                            .DefaultPerc = oRALinesItem.DefaultPerc
                            .ThisPerc = oRALinesItem.ThisPerc
                            .SumInsured = oRALinesItem.SumInsured
                            .ReserveToDate = oRALinesItem.ReserveToDate
                            .ThisReserve = oRALinesItem.ThisReserve
                            .PaymentToDate = oRALinesItem.PaymentToDate
                            .ThisPayment = oRALinesItem.ThisPayment
                            .Balance = oRALinesItem.Balance
                            .AgreementCode = oRALinesItem.Agreement
                            .IsObligatory = oRALinesItem.IsObligatory
                            oArrangementLinesCollection.Add(oArrangementLines)
                        End With
                    Next
                End If
            End With
            If Logger.IsLoggingEnabled Then
                sbLogMessage.AppendLine("GetClaimReinsuranceArrangementLines executed ok" & vbCrLf)
                sbLogMessage.AppendLine("Input:" & vbCrLf)
                sbLogMessage.AppendLine("v_iClaimID = " & v_iClaimID.ToString() & vbCrLf)
                sbLogMessage.AppendLine("v_iArrangementID = " & v_iArrangementID.ToString() & vbCrLf)
                sbLogMessage.AppendLine("v_iMode = " & v_iMode.ToString() & vbCrLf)

                sbLogMessage.AppendLine("Output:" & vbCrLf)
                sbLogMessage.AppendLine("oRALinesCollection" & oRALinesCollection.Print.Replace("<br />", vbCrLf))

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
            oGetReinsuranceArrangementLinesRequest = Nothing
            oGetReinsuranceArrangementLinesResponse = Nothing
        End Try

        Return oArrangementLinesCollection
    End Function

    Public Overrides Function GetClaimReinsuranceArrangements(ByVal v_iClaimID As Integer,
                                                        ByVal v_iMode As Integer,
                                                        Optional ByVal v_sBranchCode As String = Nothing) As ReinsuranceArrangementLineCollection
        Dim oGetReinsuranceArrangementRequest As BaseClasses.GetClaimReinsuranceArrangementsQuery
        Dim oGetReinsuranceArrangementResponse As BaseClasses.GetClaimReinsuranceArrangementsQueryResponse
        Dim oReinsuranceArrangementCollection As ReinsuranceArrangementLineCollection
        Dim oReinsuranceArrangementItemType As ReinsuranceArrangementLines
        Dim sbLogMessage As StringBuilder

        Try
            oGetReinsuranceArrangementRequest = New BaseClasses.GetClaimReinsuranceArrangementsQuery
            oGetReinsuranceArrangementResponse = New BaseClasses.GetClaimReinsuranceArrangementsQueryResponse
            sbLogMessage = New StringBuilder


            With oGetReinsuranceArrangementRequest
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
                If v_iClaimID > 0 Then
                    .ClaimId = v_iClaimID
                Else
                    Throw New ArgumentException("ClaimID")
                End If
                If v_iMode < 0 Then
                    .ModeSpecified = False
                Else
                    .Mode = v_iMode
                    .ModeSpecified = True
                End If
            End With


            'Calling the SAM Method with Request Type
            'add trace to allow us to debug slow SAM calls
            Using trace As New Tracer(Category.Trace)
                ApiClient._tokenModel = GetApiTokendetails()
                Dim result As String = ApiClient.Get(ApiMethods.GetClaimReinsuranceArrangements, oGetReinsuranceArrangementRequest)
                oGetReinsuranceArrangementResponse = ApiClient.DeserializeJson(Of BaseClasses.GetClaimReinsuranceArrangementsQueryResponse)(result)
            End Using

            'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

            ' Disposing the SAM's object



            oReinsuranceArrangementCollection = New ReinsuranceArrangementLineCollection

            With oGetReinsuranceArrangementResponse
                If .ReinsuranceArrangements IsNot Nothing AndAlso .ReinsuranceArrangements.Count > 0 Then
                    For Each oReinsuranceArrangementItem As BaseClasses.BaseGetClaimReinsuranceArrangementsResponseTypeRow In .ReinsuranceArrangements
                        oReinsuranceArrangementItemType = New ReinsuranceArrangementLines

                        With oReinsuranceArrangementItemType
                            .ArrangementId = oReinsuranceArrangementItem.ArrangementId
                            .BandId = oReinsuranceArrangementItem.BandId
                            .SumInsured = oReinsuranceArrangementItem.SumInsured
                            .ReserveToDate = oReinsuranceArrangementItem.ReserveToDate
                            .ThisReserve = oReinsuranceArrangementItem.ThisReserve
                            .PaymentToDate = oReinsuranceArrangementItem.PaymentToDate
                            .ThisPayment = oReinsuranceArrangementItem.ThisPayment
                            .Balance = oReinsuranceArrangementItem.Balance
                            .RecoveryToDate = oReinsuranceArrangementItem.RecoveryToDate
                            oReinsuranceArrangementCollection.Add(oReinsuranceArrangementItemType)
                        End With
                    Next
                End If
            End With

            If Logger.IsLoggingEnabled Then
                sbLogMessage.AppendLine("GetClaimReinsuranceArrangements executed ok" & vbCrLf)
                sbLogMessage.AppendLine("Input:" & vbCrLf)
                sbLogMessage.AppendLine("v_iClaimID = " & v_iClaimID.ToString() & vbCrLf)
                sbLogMessage.AppendLine("v_iMode = " & v_iMode.ToString() & vbCrLf)

                sbLogMessage.AppendLine("Output:" & vbCrLf)
                sbLogMessage.AppendLine("oReinsuranceArrangementCollection" & oReinsuranceArrangementCollection.Print.Replace("<br />", vbCrLf))

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
            oGetReinsuranceArrangementRequest = Nothing
            oGetReinsuranceArrangementResponse = Nothing
        End Try


        Return oReinsuranceArrangementCollection
    End Function

    Public Overrides Function GetClaimReinsurancebands(ByVal v_iClaimID As Integer,
                                                        Optional ByVal v_sBranchCode As String = Nothing) As ReinsuranceBandsCollection
        Dim oGetReinsurancebandsRequest As BaseClasses.GetClaimReinsuranceBandsQuery
        Dim oGetReinsurancebandsResponse As BaseClasses.GetClaimReinsuranceBandsQueryResponse
        Dim oReInsuranceBandsList As ReinsuranceBandsCollection
        Dim oReInsuranceBands As ReinsuranceBands
        Dim sbLogMessage As StringBuilder

        Try
            oGetReinsurancebandsRequest = New BaseClasses.GetClaimReinsuranceBandsQuery
            oGetReinsurancebandsResponse = New BaseClasses.GetClaimReinsuranceBandsQueryResponse
            sbLogMessage = New StringBuilder


            With oGetReinsurancebandsRequest
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
                If v_iClaimID > 0 Then
                    .ClaimId = v_iClaimID
                Else
                    Throw New ArgumentException("ClaimID")
                End If
            End With

            'Calling the SAM Method with Request Type
            'add trace to allow us to debug slow SAM calls
            Using trace As New Tracer(Category.Trace)
                ApiClient._tokenModel = GetApiTokendetails()
                Dim result As String = ApiClient.Get(ApiMethods.GetClaimReinsuranceBands, oGetReinsurancebandsRequest)
                oGetReinsurancebandsResponse = ApiClient.DeserializeJson(Of BaseClasses.GetClaimReinsuranceBandsQueryResponse)(result)
            End Using

            'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

            ' Disposing the SAM's object


            oReInsuranceBandsList = New ReinsuranceBandsCollection



            With oGetReinsurancebandsResponse
                If .ReinsuranceBands IsNot Nothing AndAlso .ReinsuranceBands.Count > 0 Then
                    For Each oReIsuranceBandsItem As BaseClasses.BaseGetClaimReinsuranceBandsResponseTypeRow In .ReinsuranceBands
                        oReInsuranceBands = New ReinsuranceBands
                        oReInsuranceBands.Band = oReIsuranceBandsItem.Band
                        oReInsuranceBands.BandID = oReIsuranceBandsItem.BandId
                        oReInsuranceBandsList.Add(oReInsuranceBands)
                    Next
                End If
            End With

            If Logger.IsLoggingEnabled Then
                sbLogMessage.AppendLine("GetClaimReinsuranceArrangements executed ok" & vbCrLf)
                sbLogMessage.AppendLine("Input:" & vbCrLf)
                sbLogMessage.AppendLine("v_iClaimID = " & v_iClaimID.ToString() & vbCrLf)

                sbLogMessage.AppendLine("Output:" & vbCrLf)
                sbLogMessage.AppendLine("GetClaimReinsurancebands" & oReInsuranceBandsList.Print.Replace("<br />", vbCrLf))

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
            oGetReinsurancebandsRequest = Nothing
            oGetReinsurancebandsResponse = Nothing
        End Try


        Return oReInsuranceBandsList
    End Function

    ''' <summary>
    ''' GetClaimRIArrangementLinesRI2007
    ''' </summary>
    ''' <param name="v_iClaimKey"></param>
    ''' <param name="v_iArrangementKey"></param>
    ''' <param name="v_iMode"></param>
    ''' <param name="v_bIsRecovery"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetClaimRIArrangementLinesRI2007(ByVal v_iClaimKey As Integer,
                                                        ByVal v_iArrangementKey As Integer,
                                                        ByVal v_iMode As Integer,
                                                        ByVal v_bIsRecovery As Boolean,
                                                        Optional ByVal v_sBranchCode As String = Nothing) As ArrangementLinesTypeCollection
        SyncLock oLock

            Dim oGetReinsuranceArrangementLinesRI2007Request As BaseClasses.GetClaimRIArrangementLinesRI2007Query
            Dim oGetReinsuranceArrangementLinesRI2007Response As BaseClasses.GetClaimRIArrangementLinesRI2007QueryResponse
            Dim sbLogMessage As StringBuilder
            Dim oArrangementLinesRI2007 As ArrangementLinesType
            Dim oArrangementLinesRI2007Collection As New ArrangementLinesTypeCollection
            Try
                sbLogMessage = New StringBuilder
                oGetReinsuranceArrangementLinesRI2007Request = New BaseClasses.GetClaimRIArrangementLinesRI2007Query
                oGetReinsuranceArrangementLinesRI2007Response = New BaseClasses.GetClaimRIArrangementLinesRI2007QueryResponse

                With oGetReinsuranceArrangementLinesRI2007Request
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

                    .ArrangementKey = v_iArrangementKey
                    .ClaimKey = v_iClaimKey

                    If v_iMode < 0 Then
                        .ModeSpecified = False
                    Else
                        .Mode = v_iMode
                        .ModeSpecified = True
                    End If

                    .IsRecovery = v_bIsRecovery
                    .IsRecoverySpecified = v_bIsRecovery
                End With

                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetClaimRIArrangementLinesRI2007, oGetReinsuranceArrangementLinesRI2007Request)
                    oGetReinsuranceArrangementLinesRI2007Response = ApiClient.DeserializeJson(Of BaseClasses.GetClaimRIArrangementLinesRI2007QueryResponse)(result)
                End Using

                With oGetReinsuranceArrangementLinesRI2007Response
                    If .RIArrangementLines IsNot Nothing Then

                        For Each oArrangementLinesRI2007TypeRow As BaseClasses.BaseClaimRiskRIArrangementLineType In .RIArrangementLines

                            oArrangementLinesRI2007 = New ArrangementLinesType

                            With oArrangementLinesRI2007TypeRow
                                If .ActionType IsNot Nothing Then
                                    oArrangementLinesRI2007.ActionType = .ActionType
                                End If
                                oArrangementLinesRI2007.AgreementCode = .AgreementCode
                                oArrangementLinesRI2007.CedePremiumOnly = .CedePremiumOnly
                                oArrangementLinesRI2007.DefaultPerc = .DefaultSharePercent
                                oArrangementLinesRI2007.Grouping = .Grouping
                                oArrangementLinesRI2007.IsDomiciledForTax = .IsDomiciledForTax
                                oArrangementLinesRI2007.IsRIBroker = .IsRIBroker
                                oArrangementLinesRI2007.LineLimit = .LineLimit
                                oArrangementLinesRI2007.LowerLimit = .LowerLimit
                                oArrangementLinesRI2007.NumberOfLines = .NumberOfLines
                                oArrangementLinesRI2007.PartyKey = .PartyKey
                                oArrangementLinesRI2007.Priority = .Priority
                                oArrangementLinesRI2007.ReinsuranceTypeCode = .ReinsuranceTypeCode
                                oArrangementLinesRI2007.Retained = .Retained
                                oArrangementLinesRI2007.RIarrangementKey = .RIArrangementKey
                                oArrangementLinesRI2007.RIArrangementLineKey = .RIArrangementLineKey
                                oArrangementLinesRI2007.RIName = .RIName
                                oArrangementLinesRI2007.RIPlacement = .RIPlacement
                                oArrangementLinesRI2007.SumInsured = .SumInsured
                                oArrangementLinesRI2007.ThisPerc = .ThisSharePercent
                                oArrangementLinesRI2007.TreatyCode = .TreatyCode
                                oArrangementLinesRI2007.Type = .Type
                                oArrangementLinesRI2007.Balance = .Balance
                                oArrangementLinesRI2007.PaymentToDate = .PaymentToDate
                                oArrangementLinesRI2007.RecoverToDate = .RecoverToDate
                                oArrangementLinesRI2007.ReserveToDate = .ReserveToDate
                                oArrangementLinesRI2007.ThisPayment = .ThisPayment
                                oArrangementLinesRI2007.ThisReserve = .ThisReserve
                                oArrangementLinesRI2007.Incurred = .Incurred
                                oArrangementLinesRI2007.IsObligatory = .IsObligatory

                                If .BrokerParticipants IsNot Nothing AndAlso .BrokerParticipants.Count > 0 Then
                                    oArrangementLinesRI2007.BrokerParticipants = New NexusProvider.BrokerParticipantsCollection
                                    For Each oBrokerParticipant As BaseClasses.BaseBrokerParticipants In .BrokerParticipants
                                        Dim oBrokerPart As New NexusProvider.BrokerParticipants
                                        With oBrokerParticipant
                                            oBrokerPart.ParticipationPercentage = .ParticipationPercentage
                                            oBrokerPart.PartyKey = .PartyKey
                                            oBrokerPart.PartyCode = .PartyCode
                                            oBrokerPart.PartyName = .PartyName
                                        End With

                                        oArrangementLinesRI2007.BrokerParticipants.Add(oBrokerPart)
                                    Next
                                End If

                                If .FAXParticipants IsNot Nothing AndAlso .FAXParticipants.Count > 0 Then
                                    For Each oFAXPart As BaseClasses.BaseClaimFaxParticipants In .FAXParticipants
                                        Dim oFAXParticipent As New NexusProvider.FAXParticipants
                                        With oFAXPart
                                            oFAXParticipent.AccountType = .AccountType
                                            oFAXParticipent.AgreementCode = .AgreementCode
                                            oFAXParticipent.ParticipationPercentage = .ParticpationPercentage
                                            oFAXParticipent.PartyCode = .PartyCode
                                            oFAXParticipent.PartyKey = .PartyKey
                                            oFAXParticipent.PartyName = .PartyName
                                            oFAXParticipent.RIArrangementLineKey = .RIArrangementLineKey
                                            oFAXParticipent.SumInsured = .SumInsured
                                            oFAXParticipent.ActionType = If(.ActionType, NexusProvider.RowAction.EditRow)
                                            oFAXParticipent.PaymentToDate = .PaymentToDate
                                            oFAXParticipent.RecoverToDate = .RecoverToDate
                                            oFAXParticipent.ReserveToDate = .ReserveToDate
                                            oFAXParticipent.ThisPayment = .ThisPayment
                                            oFAXParticipent.ThisReserve = .ThisReserve

                                            If .BrokerParticipants IsNot Nothing AndAlso .BrokerParticipants.Count > 0 Then
                                                For Each oBrokerParticipant As BaseClasses.BaseBrokerParticipants In .BrokerParticipants
                                                    Dim oBrokerPart As New NexusProvider.BrokerParticipants
                                                    With oBrokerParticipant
                                                        oBrokerPart.ParticipationPercentage = .ParticipationPercentage
                                                        oBrokerPart.PartyKey = .PartyKey
                                                        oBrokerPart.PartyCode = .PartyCode
                                                        oBrokerPart.PartyName = .PartyName
                                                    End With
                                                    oFAXParticipent.BrokerParticipants.Add(oBrokerPart)
                                                Next
                                            End If
                                        End With
                                        oArrangementLinesRI2007.FAXParticipants.Add(oFAXParticipent)
                                    Next
                                End If
                            End With
                            oArrangementLinesRI2007Collection.Add(oArrangementLinesRI2007)
                        Next
                    End If
                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetClaimReinsuranceArrangementLinesRI2007 executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iClaimID = " & v_iClaimKey.ToString() & vbCrLf)
                    sbLogMessage.AppendLine("v_iArrangementID = " & v_iArrangementKey.ToString() & vbCrLf)
                    sbLogMessage.AppendLine("v_iMode = " & v_iMode.ToString() & vbCrLf)

                    sbLogMessage.AppendLine("Output:" & vbCrLf)
                    sbLogMessage.AppendLine("oRALinesRI2007Collection" & oArrangementLinesRI2007Collection.Print.Replace("<br />", vbCrLf))

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
                oGetReinsuranceArrangementLinesRI2007Request = Nothing
                oGetReinsuranceArrangementLinesRI2007Response = Nothing
            End Try

            Return oArrangementLinesRI2007Collection
        End SyncLock
    End Function

    ' for 1.13 Sr1
    Public Overrides Function GetProductClaimsWorkflowOptions(ByVal ProcessType As NexusProvider.ClaimProcessType,
                                                          ByVal ProductCode As String,
                                                          Optional ByVal v_sBranchCode As String = Nothing) As ProductClaimsWorkflowOptionsValue
        SyncLock oLock
            Dim sProductRiskOptionValue As NexusProvider.ProductClaimsWorkflowOptionsValue
            Dim oGetProductClaimsWorkflowOptionsRequest As BaseClasses.GetProductClaimsWorkflowOptionsQuery ' Request Type
            Dim oGetProductClaimsWorkflowOptionsResponse As BaseClasses.GetProductClaimsWorkflowOptionsQueryResponse    ' Response Type
            Try
                oGetProductClaimsWorkflowOptionsRequest = New BaseClasses.GetProductClaimsWorkflowOptionsQuery
                oGetProductClaimsWorkflowOptionsResponse = New BaseClasses.GetProductClaimsWorkflowOptionsQueryResponse

                If Current.Cache("ClaimWorkFlow_" & ProcessType.ToString & "_" & ProductCode) Is Nothing Then

                    With oGetProductClaimsWorkflowOptionsRequest
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
                        Select Case ProcessType
                            Case NexusProvider.ClaimProcessType.ClaimPayment
                                .ClaimProcessType = ClaimProcessType.ClaimPayment
                            Case NexusProvider.ClaimProcessType.MaintainClaim
                                .ClaimProcessType = ClaimProcessType.MaintainClaim
                            Case NexusProvider.ClaimProcessType.None
                                .ClaimProcessType = ClaimProcessType.None
                            Case NexusProvider.ClaimProcessType.OpenClaim
                                .ClaimProcessType = ClaimProcessType.OpenClaim
                        End Select

                        .ProductCode = ProductCode

                    End With



                    'Calling the SAM Method with Request Type
                    'add trace to allow us to debug slow SAM calls
                    Using trace As New Tracer(Category.Trace)
                        ApiClient._tokenModel = GetApiTokendetails()
                        Dim result As String = ApiClient.Get(ApiMethods.GetProductClaimsWorkflowOptions, oGetProductClaimsWorkflowOptionsRequest)
                        oGetProductClaimsWorkflowOptionsResponse = ApiClient.DeserializeJson(Of BaseClasses.GetProductClaimsWorkflowOptionsQueryResponse)(result)
                    End Using


                    'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                    ' Disposing the SAM's object


                    With oGetProductClaimsWorkflowOptionsResponse
                        If .Errors IsNot Nothing Then
                            'Process the error object if errors, and throw as a single exception
                            Throw New NexusException(.Errors)
                        Else
                            sProductRiskOptionValue = New NexusProvider.ProductClaimsWorkflowOptionsValue

                            sProductRiskOptionValue.CashPaymentProcess = .CashPaymentProcess
                            sProductRiskOptionValue.CheckDeferredReinsurances = .CheckDeferredReinsurance
                            sProductRiskOptionValue.CheckUnpaidStatus = .CheckUnpaidStatus
                            sProductRiskOptionValue.ClaimNotificationDocMessage = .ClaimNotificationDocMessage
                            sProductRiskOptionValue.ClaimPaymentDocMessage = .ClaimPaymentDocMessage
                            sProductRiskOptionValue.ClaimPaymentProcess = .ClaimPaymentProcess
                            sProductRiskOptionValue.DescriptionForChangeInPayment = .DescriptionForChangeInPayment
                            sProductRiskOptionValue.DescriptionForChangeInReserve = .DescriptionForChangeInReserve
                            sProductRiskOptionValue.ExternalClaimHandling = .ExternalClaimHandling
                            sProductRiskOptionValue.FastTrackClaims = .FastTrackClaims
                            sProductRiskOptionValue.GenerateClaimNotificationDoc = .GenerateClaimNotificationDoc
                            sProductRiskOptionValue.GenerateClaimPaymentDoc = .GenerateClaimPaymentDoc
                            sProductRiskOptionValue.MakeFurtherPayments = .MakeFurtherPayments
                            sProductRiskOptionValue.ReinsurancePayment = .ReinsurancePayment()
                            sProductRiskOptionValue.ReinsuranceRecovery = .ReinsuranceRecovery
                            sProductRiskOptionValue.SalvageRecovery = .SalvageRecovery
                            sProductRiskOptionValue.ThirdPartyRecovery = .ThirdPartyRecovery
                            'Put it in Cache in order to read it from cache instead of SAM call
                            Current.Cache.Insert("ClaimWorkFlow_" & ProcessType.ToString & "_" & ProductCode, sProductRiskOptionValue,
                                                                              Nothing, Now.AddHours(iCacheLengthInHours), TimeSpan.Zero)
                        End If
                    End With

                Else
                    sProductRiskOptionValue = CType(Current.Cache("ClaimWorkFlow_" & ProcessType.ToString & "_" & ProductCode), ProductClaimsWorkflowOptionsValue)
                End If
                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGetProductClaimsWorkflowOptionsRequest = Nothing
                oGetProductClaimsWorkflowOptionsResponse = Nothing
            End Try
            Return sProductRiskOptionValue
        End SyncLock
    End Function

    ''' <summary>
    ''' GetClaimRisk
    ''' </summary>
    ''' <param name="v_iBaseClaimKey"></param>
    ''' <param name="v_iClaimKey"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetClaimRisk(ByVal v_iBaseClaimKey As Integer,
                                           Optional ByVal v_iClaimKey As Integer = 0,
                                           Optional ByVal v_sBranchCode As String = Nothing) As ClaimRisk
        Dim oGetClaimRiskRequest As BaseClasses.GetClaimRiskQuery
        Dim oGetClaimRiskResponse As BaseClasses.GetClaimRiskQueryResponse
        Dim oClaimRisk As ClaimRisk
        Dim sbLogMessage As StringBuilder

        Try
            oGetClaimRiskRequest = New BaseClasses.GetClaimRiskQuery
            oGetClaimRiskResponse = New BaseClasses.GetClaimRiskQueryResponse
            sbLogMessage = New StringBuilder

            With oGetClaimRiskRequest
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
                .BaseClaimKey = v_iBaseClaimKey
                .ClaimKey = v_iClaimKey
            End With

            'Calling the SAM Method with Request Type
            'add trace to allow us to debug slow SAM calls
            Using trace As New Tracer(Category.Trace)
                ApiClient._tokenModel = GetApiTokendetails()
                Dim result As String = ApiClient.Get(ApiMethods.GetClaimRisk, oGetClaimRiskRequest)
                oGetClaimRiskResponse = ApiClient.DeserializeJson(Of BaseClasses.GetClaimRiskQueryResponse)(result)
            End Using

            With oGetClaimRiskResponse
                If .GetClaimRiskResponse IsNot Nothing Then
                    oClaimRisk = New ClaimRisk
                    oClaimRisk.TimeStamp = .GetClaimRiskResponse.ApiTimeStamp
                    oClaimRisk.XMLDataSet = .GetClaimRiskResponse.XMLDataSet
                End If
            End With
            If Logger.IsLoggingEnabled Then
                sbLogMessage.AppendLine("GetClaimRisk executed ok" & vbCrLf)
                sbLogMessage.AppendLine("Input:" & vbCrLf)
                sbLogMessage.AppendLine("v_iBaseClaimKey = " & v_iBaseClaimKey.ToString() & vbCrLf)

                sbLogMessage.AppendLine("Output:" & vbCrLf)
                sbLogMessage.AppendLine("oClaimRisk = " & oClaimRisk.TimeStamp.ToString() & vbCrLf)
                sbLogMessage.AppendLine("oClaimRisk = " & oClaimRisk.XMLDataSet & vbCrLf)

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
            oGetClaimRiskRequest = Nothing
            oGetClaimRiskResponse = Nothing
        End Try

        Return oClaimRisk
    End Function

    Public Overrides Function GetClaimRiskLinks(ByVal v_iInsuranceFileKey As Integer,
                                                        ByVal v_iRiskKey As Integer,
                                                        Optional ByVal v_sBranchCode As String = Nothing) As ClaimRiskLinkCollection
        Dim oGetClaimRiskLinkRequest As BaseClasses.GetClaimRiskLinksQuery
        Dim oGetClaimRiskLinkResponse As BaseClasses.GetClaimRiskLinksQueryResponse
        Dim oClaimRiskLinks As ClaimRiskLinkCollection
        Dim oClaimRiskLinkType As ClaimRiskLink = Nothing
        Dim oClaimReserveItem As ReserveType
        Dim oClaimRecoveryItem As RecoveryType = Nothing
        Dim sbLogMessage As StringBuilder

        Try
            oGetClaimRiskLinkRequest = New BaseClasses.GetClaimRiskLinksQuery
            oGetClaimRiskLinkResponse = New BaseClasses.GetClaimRiskLinksQueryResponse
            sbLogMessage = New StringBuilder


            With oGetClaimRiskLinkRequest
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
                    Throw New ArgumentException("InsuranceFileKey")
                End If
                If v_iRiskKey > 0 Then
                    .RiskKey = v_iRiskKey
                Else
                    Throw New ArgumentException("RiskKey")
                End If

            End With

            'Calling the SAM Method with Request Type
            'add trace to allow us to debug slow SAM calls
            Using trace As New Tracer(Category.Trace)
                ApiClient._tokenModel = GetApiTokendetails()
                Dim result As String = ApiClient.Get(ApiMethods.GetClaimRiskLinks, oGetClaimRiskLinkRequest)
                oGetClaimRiskLinkResponse = ApiClient.DeserializeJson(Of BaseClasses.GetClaimRiskLinksQueryResponse)(result)
            End Using

            'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

            ' Disposing the SAM's object



            oClaimRiskLinks = New ClaimRiskLinkCollection

            With oGetClaimRiskLinkResponse
                If .PerilType IsNot Nothing AndAlso .PerilType.Count > 0 Then
                    For Each oClaimRiskItem As BaseClasses.BaseGetClaimRiskLinksResponseTypePerilType In .PerilType
                        oClaimRiskLinkType = New ClaimRiskLink
                        oClaimRiskLinkType.Code = oClaimRiskItem.Code
                        oClaimRiskLinkType.Description = oClaimRiskItem.Description
                        oClaimRiskLinkType.SumInsured = oClaimRiskItem.SumInsured

                        With oClaimRiskItem

                            If .ReserveType IsNot Nothing AndAlso .ReserveType.Count > 0 Then
                                For Each oClaimRiskLinkReserveItem As BaseClasses.BaseGetClaimRiskLinksResponseTypePerilTypeReserveType In .ReserveType
                                    oClaimReserveItem = New ReserveType
                                    With oClaimReserveItem
                                        .Code = oClaimRiskLinkReserveItem.Code
                                        .Description = oClaimRiskLinkReserveItem.Description
                                    End With
                                    oClaimRiskLinkType.ReserveItemType.Add(oClaimReserveItem)
                                Next
                            End If



                            If .RecoveryType IsNot Nothing AndAlso .RecoveryType.Count > 0 Then
                                For Each oClaimRiskLinkRecoveryItem As BaseClasses.BaseGetClaimRiskLinksResponseTypePerilTypeRecoveryType In .RecoveryType
                                    oClaimRecoveryItem = New RecoveryType
                                    With oClaimRecoveryItem
                                        .Code = oClaimRiskLinkRecoveryItem.Code
                                        .Description = oClaimRiskLinkRecoveryItem.Description
                                        .IsSalvage = oClaimRiskLinkRecoveryItem.IsSalvage
                                        oClaimRiskLinkType.RecoveryItemType.Add(oClaimRecoveryItem)
                                    End With
                                Next
                            End If
                        End With
                        oClaimRiskLinks.Add(oClaimRiskLinkType)
                    Next
                End If
            End With

            If Logger.IsLoggingEnabled Then
                sbLogMessage.AppendLine("GetClaimRiskLink executed ok" & vbCrLf)
                sbLogMessage.AppendLine("Input:" & vbCrLf)
                sbLogMessage.AppendLine("v_iInsuranceFileKey = " & v_iInsuranceFileKey.ToString() & vbCrLf)
                sbLogMessage.AppendLine("v_iRiskKey = " & v_iRiskKey.ToString() & vbCrLf)

                sbLogMessage.AppendLine("Output:" & vbCrLf)
                sbLogMessage.AppendLine("oClaimRiskLinks = " & oClaimRiskLinks.Print.Replace("<br />", vbCrLf))

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
            oGetClaimRiskLinkRequest = Nothing
            oGetClaimRiskLinkResponse = Nothing
        End Try


        Return oClaimRiskLinks
    End Function

    Public Overrides Function GetClaimRiskReadOnly(ByVal v_iBaseClaimKey As Integer,
                                                     Optional ByVal v_sBranchCode As String = Nothing) As ClaimRisk
        Dim oGetClaimRiskRequest As BaseClasses.GetClaimRiskReadOnlyQuery
        Dim oGetClaimRiskResponse As BaseClasses.GetClaimRiskReadOnlyQueryResponse
        Dim oClaimRisk As ClaimRisk
        Dim sbLogMessage As StringBuilder

        Try
            oGetClaimRiskRequest = New BaseClasses.GetClaimRiskReadOnlyQuery
            oGetClaimRiskResponse = New BaseClasses.GetClaimRiskReadOnlyQueryResponse
            sbLogMessage = New StringBuilder


            With oGetClaimRiskRequest
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
                If v_iBaseClaimKey > 0 Then
                    .BaseClaimKey = v_iBaseClaimKey
                Else
                    Throw New ArgumentException("ClaimKey")
                End If

            End With



            'Calling the SAM Method with Request Type
            'add trace to allow us to debug slow SAM calls
            Using trace As New Tracer(Category.Trace)
                ApiClient._tokenModel = GetApiTokendetails()
                Dim result As String = ApiClient.Get(ApiMethods.GetClaimRiskReadOnly, oGetClaimRiskRequest)
                oGetClaimRiskResponse = ApiClient.DeserializeJson(Of BaseClasses.GetClaimRiskReadOnlyQueryResponse)(result)
            End Using

            'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

            ' Disposing the SAM's object





            With oGetClaimRiskResponse
                oClaimRisk = New ClaimRisk
                'oClaimRisk.TimeStamp = .TimeStamp()
                oClaimRisk.XMLDataSet = .XMLDataSet
            End With

            If Logger.IsLoggingEnabled Then
                sbLogMessage.AppendLine("GetClaimRisk executed ok" & vbCrLf)
                sbLogMessage.AppendLine("Input:" & vbCrLf)
                sbLogMessage.AppendLine("v_iBaseClaimKey = " & v_iBaseClaimKey.ToString() & vbCrLf)

                sbLogMessage.AppendLine("Output:" & vbCrLf)
                sbLogMessage.AppendLine("oClaimRisk = " & oClaimRisk.TimeStamp.ToString() & vbCrLf)
                sbLogMessage.AppendLine("oClaimRisk = " & oClaimRisk.XMLDataSet & vbCrLf)

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
            oGetClaimRiskRequest = Nothing
            oGetClaimRiskResponse = Nothing
        End Try


        Return oClaimRisk
    End Function


    ''' <summary>
    ''' To get recovery coinsurance if the policy was a coinsurance policy.
    ''' </summary>
    ''' <param name="v_iClaimPerilKey"></param>
    ''' <param name="v_bIsSalvage"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetRecoveryCoinsurance(ByVal v_iClaimPerilKey As Integer,
                                                        ByVal v_bIsSalvage As Boolean,
                                                        Optional ByVal v_sBranchCode As String = Nothing) As CoInsurersCollections
        SyncLock oLock

            Dim oGetRecoveryCoinsuranceRequest As BaseClasses.GetRecoveryCoinsuranceQuery 'Request Type
            Dim oGetRecoveryCoinsuranceResponse As BaseClasses.GetRecoveryCoinsuranceQueryResponse 'Response Type
            Dim oCoinsurances As CoInsurers
            Dim oCoinsurancesCollection As CoInsurersCollections
            Dim sbLogMessage As StringBuilder

            Try
                oGetRecoveryCoinsuranceRequest = New BaseClasses.GetRecoveryCoinsuranceQuery
                oGetRecoveryCoinsuranceResponse = New BaseClasses.GetRecoveryCoinsuranceQueryResponse
                oCoinsurancesCollection = New CoInsurersCollections
                sbLogMessage = New StringBuilder

                With oGetRecoveryCoinsuranceRequest
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
                    If v_iClaimPerilKey > 0 Then
                        .ClaimPerilKey = v_iClaimPerilKey
                    Else
                        Throw New ArgumentNullException("GetRecoveryreinsurance.ClaimPerilKey")
                    End If
                    .IsSalvage = v_bIsSalvage
                End With

                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetRecoveryCoinsurance, oGetRecoveryCoinsuranceRequest)
                    oGetRecoveryCoinsuranceResponse = ApiClient.DeserializeJson(Of BaseClasses.GetRecoveryCoinsuranceQueryResponse)(result)
                End Using

                ' Disposing the SAM's object



                With oGetRecoveryCoinsuranceResponse
                    If .Coinsurances IsNot Nothing AndAlso .Coinsurances.Count > 0 Then
                        For Each oCoinsurancesTypeRow As BaseClasses.BaseGetRecoveryCoinsuranceResponseTypeRow In .Coinsurances
                            oCoinsurances = New CoInsurers
                            With oCoinsurances
                                .RecoveryKey = oCoinsurancesTypeRow.RecoveryKey
                                .RecoveryType = oCoinsurancesTypeRow.RecoveryType
                                .PartyKey = oCoinsurancesTypeRow.PartyKey
                                .CoInsurer = oCoinsurancesTypeRow.Coinsurer
                                .SharePercent = oCoinsurancesTypeRow.SharePercent
                                .RecoveryToDate = oCoinsurancesTypeRow.RecoveryToDate
                                .RecoveryTypeCode = oCoinsurancesTypeRow.RecoveryTypeCode
                            End With
                            oCoinsurancesCollection.Add(oCoinsurances)
                        Next
                    End If
                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetRecoveryCoinsurance executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output : " & vbCrLf)
                    sbLogMessage.AppendLine(oCoinsurancesCollection.Print().Replace("<br />", vbCrLf))


                    LogMessageEntry(sbLogMessage)
                End If

            Catch ex As Exception
                Throw
            Finally
                oGetRecoveryCoinsuranceRequest = Nothing
                oGetRecoveryCoinsuranceResponse = Nothing
            End Try


            Return oCoinsurancesCollection
        End SyncLock
    End Function

    ''' <summary>
    ''' To get recovery reinsurance.
    ''' </summary>
    ''' <param name="v_iClaimPerilKey"></param>
    ''' <param name="v_bIsSalvage"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetRecoveryReinsurance(ByVal v_iClaimPerilKey As Integer,
                                                        ByVal v_bIsSalvage As Boolean,
                                                        Optional ByVal v_sBranchCode As String = Nothing) As ReinsurancesCollection

        SyncLock oLock

            Dim oGetRecoveryreinsuranceRequest As BaseClasses.GetRecoveryReinsuranceQuery 'Request Type
            Dim oGetRecoveryreinsuranceResponse As BaseClasses.GetRecoveryReinsuranceQueryResponse 'Response Type
            Dim oReinsurances As Reinsurances
            Dim oReinsurancesCollection As ReinsurancesCollection
            Dim sbLogMessage As StringBuilder

            Try
                oGetRecoveryreinsuranceRequest = New BaseClasses.GetRecoveryReinsuranceQuery
                oGetRecoveryreinsuranceResponse = New BaseClasses.GetRecoveryReinsuranceQueryResponse
                oReinsurancesCollection = New ReinsurancesCollection
                sbLogMessage = New StringBuilder

                With oGetRecoveryreinsuranceRequest
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

                    If v_iClaimPerilKey > 0 Then
                        .ClaimPerilKey = v_iClaimPerilKey
                    Else
                        Throw New ArgumentNullException("GetRecoveryreinsurance.ClaimPerilKey")
                    End If
                    .IsSalvage = v_bIsSalvage

                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetRecoveryReinsurance, oGetRecoveryreinsuranceRequest)
                    oGetRecoveryreinsuranceResponse = ApiClient.DeserializeJson(Of BaseClasses.GetRecoveryReinsuranceQueryResponse)(result)
                End Using

                With oGetRecoveryreinsuranceResponse
                    If oGetRecoveryreinsuranceResponse.Reinsurances IsNot Nothing Then
                        For Each oReinsurancesTypeRow As BaseClasses.BaseGetRecoveryReinsuranceResponseTypeRow In .Reinsurances
                            oReinsurances = New Reinsurances
                            With oReinsurances

                                .RecoveryKey = oReinsurancesTypeRow.RecoveryKey
                                .RecoveryType = oReinsurancesTypeRow.RecoveryType
                                .PartyKey = oReinsurancesTypeRow.PartyKey
                                .Reinsurer = oReinsurancesTypeRow.Reinsurer
                                .SharePercent = oReinsurancesTypeRow.SharePercent
                                .RecoveryToDate = oReinsurancesTypeRow.RecoveryToDate
                                .ThisRecovery = oReinsurancesTypeRow.ThisRecovery
                                .ThisSalvage = oReinsurancesTypeRow.ThisSalvage
                                .Salvage = oReinsurancesTypeRow.Salvage
                                .Recovery = oReinsurancesTypeRow.Recovery

                            End With
                            oReinsurancesCollection.Add(oReinsurances)
                        Next
                    End If
                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetRecoveryReinsurance executed ok" & vbCrLf)

                    sbLogMessage.AppendLine("Input : " & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If


                    If IsNothing(v_iClaimPerilKey) Then
                        sbLogMessage.AppendLine("ClaimPerilKey : nothing" & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("ClaimPerilKey : " & v_iClaimPerilKey & vbCrLf)
                    End If


                    sbLogMessage.AppendLine("Output : " & vbCrLf)
                    sbLogMessage.AppendLine(oReinsurancesCollection.Print().Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGetRecoveryreinsuranceRequest = Nothing
                oGetRecoveryreinsuranceResponse = Nothing
            End Try

            Return oReinsurancesCollection
        End SyncLock
    End Function



    ''' <summary>
    ''' This method is used to retrieve the referred payments.
    ''' </summary>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetReferredPayments(Optional ByVal v_oCashListItem As NexusProvider.CashListItems = Nothing,
                                                 Optional ByVal v_sBranchCode As String = Nothing, Optional ByVal v_sReferredPaymentsBranchCode As String = Nothing) As CashListItemsCollection
        SyncLock oLock

            Dim oGetReferredPaymentsRequest As BaseClasses.GetReferredPaymentsQuery 'Request Type
            Dim oGetReferredPaymentsResponse As BaseClasses.GetReferredPaymentsQueryResponse 'Response Type
            Dim oCashListItems As CashListItems
            Dim oCashListItemsCollections As CashListItemsCollection
            Dim sbLogMessage As StringBuilder

            Try
                oGetReferredPaymentsRequest = New BaseClasses.GetReferredPaymentsQuery
                oGetReferredPaymentsResponse = New BaseClasses.GetReferredPaymentsQueryResponse
                oCashListItemsCollections = New CashListItemsCollection
                sbLogMessage = New StringBuilder

                With oGetReferredPaymentsRequest
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
                    If v_oCashListItem IsNot Nothing Then
                        .ClaimNumber = v_oCashListItem.ClaimNumber
                        If v_oCashListItem.PaymentDate <> Date.MinValue Then
                            .DateOfPayment = v_oCashListItem.PaymentDate
                            '.DateOfPaymentSpecified = True
                        Else
                            '.DateOfPaymentSpecified = False
                        End If
                        If v_oCashListItem.ClientKey > 0 Then
                            .PartyKey = v_oCashListItem.ClientKey
                            .PartyKeySpecified = True
                        Else
                            .PartyKeySpecified = False
                        End If
                        .PolicyNumber = v_oCashListItem.PolicyNumber
                        .UserCode = v_oCashListItem.CreatedBy
                        .CaseNumber = v_oCashListItem.CaseNumber
                        .PayeeName = v_oCashListItem.PayeeName
                    End If

                    .ReferredPaymentsBranchCode = v_sReferredPaymentsBranchCode
                End With

                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = ApiClient.Get(ApiMethods.GetReferredPayments, oGetReferredPaymentsRequest)
                    oGetReferredPaymentsResponse = ApiClient.DeserializeJson(Of BaseClasses.GetReferredPaymentsQueryResponse)(result)
                End Using


                With oGetReferredPaymentsResponse
                    If .CashListItems IsNot Nothing AndAlso .CashListItems.Count > 0 Then
                        For Each oCashListItemTypeRow As BaseClasses.BaseGetReferredPaymentsResponseTypeRow In .CashListItems
                            oCashListItems = New CashListItems
                            With oCashListItems
                                .ClaimPaymentKey = oCashListItemTypeRow.ClaimPaymentKey
                                .ClaimKey = oCashListItemTypeRow.ClaimKey
                                .ClaimNumber = oCashListItemTypeRow.ClaimNumber
                                .PolicyNumber = oCashListItemTypeRow.PolicyNumber
                                .ClientName = oCashListItemTypeRow.ClientName
                                .PaymentAmount = oCashListItemTypeRow.PaymentAmount
                                .PaymentDate = oCashListItemTypeRow.PaymentDate
                                .CreatedBy = oCashListItemTypeRow.CreatedBy
                                .Status = oCashListItemTypeRow.Status
                                .CaseNumber = oCashListItemTypeRow.CaseNumber
                                .PayeeName = oCashListItemTypeRow.PayeeName
                                .PartyType = oCashListItemTypeRow.PayeeType
                                .CurrencyId = oCashListItemTypeRow.CurrencyId
                                .IsReferredForRecommendation = oCashListItemTypeRow.IsReferredForRecommendation
                                .RecommendedBy = oCashListItemTypeRow.RecommendedBy
                                .CurrencyCode = oCashListItemTypeRow.CurrencyCode
                                .PaymentAmountBaseCurrency = oCashListItemTypeRow.PaymentAmountBaseCurrency
                            End With
                            oCashListItemsCollections.Add(oCashListItems)
                        Next
                    End If
                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetReferredPayments executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output : " & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oGetReferredPaymentsRequest = Nothing
                oGetReferredPaymentsResponse = Nothing
            End Try

            Return oCashListItemsCollections
        End SyncLock

    End Function

    ''' <summary>
    ''' This method is used to Get TaxGroups For Claims
    ''' </summary>
    ''' <param name="v_bIs_withholding_tax"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetTaxGroupsForClaims(ByVal v_bIs_withholding_tax As Boolean,
                                  Optional ByVal v_sBranchCode As String = Nothing, Optional ByVal v_sTransactionTypeCode As String = Nothing) As TaxGroupCollection


        SyncLock oLock

            'Dim oSAM As PureClaimServiceClient
            Dim oGetTaxGroupsForClaimsRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetTaxGroupsForClaimsQuery
            Dim oGetTaxGroupsForClaimsResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetTaxGroupsForClaimsQueryResponse
            Dim oTaxGroupCollection As TaxGroupCollection
            Dim oTaxGroup As TaxGroup
            Dim sbLogMessage As StringBuilder

            Try
                'oSAM = InitializeClaimServiceMethod()
                oGetTaxGroupsForClaimsRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetTaxGroupsForClaimsQuery
                oGetTaxGroupsForClaimsResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetTaxGroupsForClaimsQueryResponse
                oTaxGroupCollection = New TaxGroupCollection
                sbLogMessage = New StringBuilder


                With oGetTaxGroupsForClaimsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    '.WCFSecurityToken = SecurityToken()
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

                    .Is_withholding_tax = v_bIs_withholding_tax
                    .TransactionTypeCode = v_sTransactionTypeCode
                End With


                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    'oGetTaxGroupsForClaimsResponse = oSAM.GetTaxGroupsForClaims(oGetTaxGroupsForClaimsRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetTaxGroupsForClaims, oGetTaxGroupsForClaimsRequest)
                    oGetTaxGroupsForClaimsResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetTaxGroupsForClaimsQueryResponse)(result)

                End Using

                With oGetTaxGroupsForClaimsResponse


                    If .TaxGroups IsNot Nothing AndAlso .TaxGroups.Count > 0 Then
                        For Each oTaxGroupRow As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetTaxGroupsForClaimsResponseTypeRow In .TaxGroups
                            oTaxGroup = New TaxGroup()
                            oTaxGroup.AdvanceTaxScript = oTaxGroupRow.AdvanceTaxScript
                            oTaxGroup.Code = oTaxGroupRow.Code
                            oTaxGroup.Description = oTaxGroupRow.Description
                            oTaxGroup.IsWithHoldingTax = oTaxGroupRow.IsWithHoldingTax
                            oTaxGroup.TaxGroupKey = oTaxGroupRow.TaxGroupKey
                            oTaxGroup.IsTaxAmountEditable = oTaxGroupRow.IsTaxAmountEditable
                            oTaxGroupCollection.Add(oTaxGroup)
                        Next
                    End If

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetTaxGroupsForClaims executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    'sbLogMessage.AppendLine("v_iAddressKey = " & v_iAddressKey.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    'sbLogMessage.AppendLine("Returned " & oAddress.Print.Replace("<br />", vbCrLf) & vbCrLf)
                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                ' oSAM.Close()
                oGetTaxGroupsForClaimsRequest = Nothing
                oGetTaxGroupsForClaimsResponse = Nothing
            End Try



            Return oTaxGroupCollection

        End SyncLock
    End Function
    ''' <summary>
    ''' To get the account details for unallocated claim payments
    ''' </summary>
    ''' <param name="v_iAccountKey"></param>
    ''' <param name="v_dtPaymentDate"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetUnallocatedClaimPayment(ByVal v_iAccountKey As Integer,
                                 ByVal v_dtPaymentDate As DateTime,
                                 ByVal v_dtPaymentDateTo As DateTime,
                                 Optional ByVal v_sShortCode As String = Nothing,
                                 Optional ByVal v_sBranchCode As String = Nothing) As UnallocatedClaimPaymentsCollection


        SyncLock oLock

            'Dim oSAM As PureClaimServiceClient
            Dim oGetUnallocatedClaimPaymentsRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUnallocatedClaimPaymentsQuery
            Dim oGetUnallocatedClaimPaymentsResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUnallocatedClaimPaymentsQueryResponse
            Dim oUnallocatedClaimPaymentsCollection As UnallocatedClaimPaymentsCollection
            Dim oUnallocatedClaimPayments As UnallocatedClaimPayments
            Dim sbLogMessage As StringBuilder

            Try
                'oSAM = InitializeClaimServiceMethod()
                oGetUnallocatedClaimPaymentsRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUnallocatedClaimPaymentsQuery
                oGetUnallocatedClaimPaymentsResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUnallocatedClaimPaymentsQueryResponse
                oUnallocatedClaimPaymentsCollection = New UnallocatedClaimPaymentsCollection
                sbLogMessage = New StringBuilder


                With oGetUnallocatedClaimPaymentsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    '.WCFSecurityToken = SecurityToken()
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

                    If v_iAccountKey > 0 Then
                        .AccountKey = v_iAccountKey
                        .AccountKeySpecified = True
                    Else
                        .AccountKeySpecified = False

                    End If

                    If v_dtPaymentDate <> DateTime.MinValue Then
                        .PaymentDateSpecified = True
                        .PaymentDate = v_dtPaymentDate
                    Else
                        .PaymentDateSpecified = False
                    End If

                    If v_dtPaymentDateTo <> DateTime.MinValue Then
                        .PaymentDateToSpecified = True
                        .PaymentDateTo = v_dtPaymentDateTo
                    Else
                        .PaymentDateToSpecified = False
                    End If

                    .ShortCode = v_sShortCode

                End With

                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    'oGetUnallocatedClaimPaymentsResponse = oSAM.GetUnallocatedClaimPayments(oGetUnallocatedClaimPaymentsRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetUnallocatedClaimPayment, oGetUnallocatedClaimPaymentsRequest)
                    oGetUnallocatedClaimPaymentsResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetUnallocatedClaimPaymentsQueryResponse)(result)

                End Using

                With oGetUnallocatedClaimPaymentsResponse



                    If .UnallocatedClaimPayments IsNot Nothing AndAlso .UnallocatedClaimPayments.Count > 0 Then

                        For Each oUnallocatedClaimPaymentsRow As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetUnallocatedClaimPaymentsResponseTypeRow In .UnallocatedClaimPayments
                            oUnallocatedClaimPayments = New UnallocatedClaimPayments()

                            oUnallocatedClaimPayments.DocumentKey = oUnallocatedClaimPaymentsRow.DocumentKey
                            oUnallocatedClaimPayments.DocumentRef = oUnallocatedClaimPaymentsRow.DocumentRef
                            oUnallocatedClaimPayments.CurrencyAmount = oUnallocatedClaimPaymentsRow.CurrencyAmount
                            oUnallocatedClaimPayments.CurrencyKey = oUnallocatedClaimPaymentsRow.CurrencyKey
                            oUnallocatedClaimPayments.Amount = oUnallocatedClaimPaymentsRow.Amount
                            oUnallocatedClaimPayments.AmountCurrencyKey = oUnallocatedClaimPaymentsRow.AmountCurrencyKey
                            oUnallocatedClaimPayments.AccountAmount = oUnallocatedClaimPaymentsRow.AccountAmount
                            oUnallocatedClaimPayments.AccountCurrencyKey = oUnallocatedClaimPaymentsRow.AccountCurrencyKey
                            oUnallocatedClaimPayments.ClaimNumber = oUnallocatedClaimPaymentsRow.ClaimNumber
                            oUnallocatedClaimPayments.DocumentComment = oUnallocatedClaimPaymentsRow.DocumentComment
                            oUnallocatedClaimPayments.CurrencyDescription = oUnallocatedClaimPaymentsRow.CurrencyDescription
                            oUnallocatedClaimPayments.CurrencyFormatString = oUnallocatedClaimPaymentsRow.CurrencyFormatString
                            oUnallocatedClaimPayments.DateOfPayment = oUnallocatedClaimPaymentsRow.DateOfPayment
                            oUnallocatedClaimPayments.PayeeMediaTypeKey = oUnallocatedClaimPaymentsRow.PayeeMediaTypeKey
                            oUnallocatedClaimPayments.BaseCurrencyDescription = oUnallocatedClaimPaymentsRow.BaseCurrencyDescription
                            oUnallocatedClaimPayments.BaseCurrencyFormatString = oUnallocatedClaimPaymentsRow.BaseCurrencyFormatString
                            oUnallocatedClaimPayments.MaxClaimPaymentKey = oUnallocatedClaimPaymentsRow.MaxClaimPaymentKey
                            oUnallocatedClaimPayments.DocumentDate = oUnallocatedClaimPaymentsRow.DocumentDate
                            oUnallocatedClaimPayments.AccountKey = oUnallocatedClaimPaymentsRow.AccountKey
                            oUnallocatedClaimPayments.BaseClaimPaymentKey = oUnallocatedClaimPaymentsRow.BaseClaimPaymentKey
                            oUnallocatedClaimPayments.AccountName = oUnallocatedClaimPaymentsRow.AccountName

                            oUnallocatedClaimPayments.Status = oUnallocatedClaimPaymentsRow.Status
                            oUnallocatedClaimPayments.MediaTypeCode = oUnallocatedClaimPaymentsRow.MediaTypeCode
                            oUnallocatedClaimPayments.CurrencyCode = oUnallocatedClaimPaymentsRow.CurrencyCode
                            oUnallocatedClaimPayments.BankAccountCode = oUnallocatedClaimPaymentsRow.BankAccountCode
                            oUnallocatedClaimPayments.CashListItemKey = oUnallocatedClaimPaymentsRow.CashListItemKey
                            oUnallocatedClaimPayments.AccountCode = oUnallocatedClaimPaymentsRow.AccountCode
                            oUnallocatedClaimPayments.ClaimPaymentBranchCode = oUnallocatedClaimPaymentsRow.ClaimPaymentBranchCode
                            oUnallocatedClaimPayments.PayeeName = oUnallocatedClaimPaymentsRow.PayeeName
                            oUnallocatedClaimPayments.OurRef = oUnallocatedClaimPaymentsRow.OurRef
                            oUnallocatedClaimPayments.ClaimPaymentKey = oUnallocatedClaimPaymentsRow.ClaimPaymentKey
                            oUnallocatedClaimPayments.MediaTypeDesc = oUnallocatedClaimPaymentsRow.MediaTypeDesc
                            oUnallocatedClaimPayments.TheirRef = oUnallocatedClaimPaymentsRow.TheirRef
                            oUnallocatedClaimPayments.PayeeAccountNo = oUnallocatedClaimPaymentsRow.PayeeAccountNo
                            oUnallocatedClaimPayments.PayeeShortCode = oUnallocatedClaimPaymentsRow.PayeeShortCode
                            oUnallocatedClaimPayments.PartyBankId = oUnallocatedClaimPaymentsRow.PartyBankId
                            oUnallocatedClaimPayments.MediaReference = oUnallocatedClaimPaymentsRow.MediaRef
                            oUnallocatedClaimPaymentsCollection.Add(oUnallocatedClaimPayments)
                        Next

                    End If


                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetUnallocatedClaimPayments executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    'sbLogMessage.AppendLine("v_iAddressKey = " & v_iAddressKey.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    'sbLogMessage.AppendLine("Returned " & oAddress.Print.Replace("<br />", vbCrLf) & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                'oSAM.Close()
                oGetUnallocatedClaimPaymentsRequest = Nothing
                oGetUnallocatedClaimPaymentsResponse = Nothing
            End Try
            Return oUnallocatedClaimPaymentsCollection
        End SyncLock

    End Function
    ''' <summary>
    ''' GetValidPrimaryCauses
    ''' </summary>
    ''' <param name="v_iInsuranceFileKey"></param>
    ''' <param name="v_iMode"></param>
    ''' <param name="v_bModeSpecified"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <param name="v_bIncludeDeleted"></param>
    ''' <returns></returns>
    Public Overrides Function GetValidPrimaryCauses(ByVal v_iInsuranceFileKey As Integer,
                                                       Optional ByVal v_iMode As Integer = 0,
                                                       Optional ByVal v_bModeSpecified As Boolean = False,
                                        Optional ByVal v_sBranchCode As String = Nothing,
                                                    Optional ByVal v_bIncludeDeleted As Boolean = False) As PrimaryCausesCollections
        SyncLock oLock

            'Dim oSAM As PureClaimServiceClient
            Dim oGetValidPrimaryCausesRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetValidPrimaryCausesQuery
            Dim oGetValidPrimaryCausesResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetValidPrimaryCausesQueryResponse
            Dim oPrimaryCausesCollection As PrimaryCausesCollections = Nothing
            Dim oNewPrimaryCauses As PrimaryCauses
            Dim sbLogMessage As StringBuilder

            Try
                'oSAM = InitializeClaimServiceMethod()
                oGetValidPrimaryCausesRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetValidPrimaryCausesQuery
                oGetValidPrimaryCausesResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetValidPrimaryCausesQueryResponse
                sbLogMessage = New StringBuilder


                With oGetValidPrimaryCausesRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    '.WCFSecurityToken = SecurityToken()
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
                    If v_bModeSpecified Then
                        .Mode = v_iMode
                        .ModeSpecified = True
                    End If
                    .IncludeDeleted = v_bIncludeDeleted
                End With

                Using trace As New Tracer(Category.Trace)
                    'oGetValidPrimaryCausesResponse = oSAM.GetValidPrimaryCauses(oGetValidPrimaryCausesRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetValidPrimaryCauses, oGetValidPrimaryCausesRequest)
                    oGetValidPrimaryCausesResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetValidPrimaryCausesQueryResponse)(result)

                End Using

                With oGetValidPrimaryCausesResponse

                    If .PrimaryCauses IsNot Nothing AndAlso .PrimaryCauses.Count > 0 Then

                        oPrimaryCausesCollection = New PrimaryCausesCollections
                        If .PrimaryCauses IsNot Nothing AndAlso .PrimaryCauses.Count > 0 Then
                            For Each oPrimaryCauses As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetValidPrimaryCausesResponseTypeRow In .PrimaryCauses

                                oNewPrimaryCauses = New PrimaryCauses

                                With oNewPrimaryCauses

                                    .Code = oPrimaryCauses.code
                                    .Description = oPrimaryCauses.description
                                    .PrimaryCauseId = oPrimaryCauses.primary_cause_id

                                End With

                                oPrimaryCausesCollection.Add(oNewPrimaryCauses)

                            Next
                        End If
                    End If



                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetValidPrimaryCauses executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_iInsuranceFileKey = " & v_iInsuranceFileKey.ToString & vbCrLf)

                    sbLogMessage.AppendLine("Returned " & oPrimaryCausesCollection.Print.ToString & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

            Catch ex As Exception
                Throw
            Finally
                'oSAM.Close()
                oGetValidPrimaryCausesRequest = Nothing
                oGetValidPrimaryCausesResponse = Nothing
            End Try


            Return oPrimaryCausesCollection

        End SyncLock

    End Function
    Public Overrides Function GetVersionsForClaim(ByVal v_sClaimNumber As String,
                                            Optional ByVal v_sBranchCode As String = Nothing) As VersionsCollections

        SyncLock oLock

            'Dim oSAM As PureClaimServiceClient
            Dim oGetVersionsForClaimRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetVersionsForClaimQuery
            Dim oGetVersionsForClaimResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetVersionsForClaimQueryResponse
            Dim oVersionsCollection As VersionsCollections = Nothing
            Dim oNewVersions As Versions
            Dim sbLogMessage As StringBuilder

            Try
                'oSAM = InitializeClaimServiceMethod()
                oGetVersionsForClaimRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetVersionsForClaimQuery
                oGetVersionsForClaimResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetVersionsForClaimQueryResponse
                sbLogMessage = New StringBuilder


                With oGetVersionsForClaimRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    '.WCFSecurityToken = SecurityToken()
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

                    'if the passed parameter v_sClaimNumber is empty
                    If String.IsNullOrEmpty(v_sClaimNumber) Then
                        Throw New ArgumentNullException("v_sClaimNumber")
                    Else
                        .ClaimNumber = v_sClaimNumber
                    End If

                End With


                Using trace As New Tracer(Category.Trace)
                    'oGetVersionsForClaimResponse = oSAM.GetVersionsForClaim(oGetVersionsForClaimRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetVersionsForClaim, oGetVersionsForClaimRequest)
                    oGetVersionsForClaimResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetVersionsForClaimQueryResponse)(result)

                End Using

                With oGetVersionsForClaimResponse


                    If .Versions IsNot Nothing Then

                        oVersionsCollection = New VersionsCollections
                        If .Versions IsNot Nothing AndAlso .Versions.Count > 0 Then
                            For Each oVersions As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetVersionsForClaimResponseTypeRow In .Versions

                                oNewVersions = New Versions
                                With oNewVersions

                                    .ClaimKey = oVersions.ClaimKey
                                    .Version = oVersions.Version
                                    .TransactionDate = oVersions.TransactionDate
                                    .TransactionType = oVersions.TransactionType
                                    .VersionDescription = oVersions.VersionDescription
                                    .TotalIncurred = oVersions.TotalIncurred
                                    .TotalPaid = oVersions.TotalPaid
                                    .ThisRevision = oVersions.ThisRevision
                                    .ThisPayment = oVersions.ThisPayment
                                    .ThisSalvageRecovery = oVersions.ThisSalvageRecovery
                                    .ThisThirdPartyRecovery = oVersions.ThisThirdPartyRecovery
                                    .CurrentReserve = oVersions.CurrentReserve
                                    .PolicyCurrency = oVersions.PolicyCurrency
                                    .LossCurrency = oVersions.LossCurrency
                                    .User = oVersions.User
                                    .ClaimDescription = oVersions.ClaimDescription
                                    .InsuranceRef = oVersions.InsuranceRef
                                    .InsuranceFileKey = oVersions.InsuranceFileKey
                                    .ClaimNumber = oVersions.claim_number
                                    .RiskKey = oVersions.RiskKey
                                    .ClientShortName = oVersions.client_short_name
                                    .LossFromDate = oVersions.loss_from_date
                                    .InsuranceHolderShortName = oVersions.InsuranceHolderShortName
                                    .InsuranceFolderKey = oVersions.InsuranceFolderKey

                                End With

                                oVersionsCollection.Add(oNewVersions)

                            Next
                        End If
                    End If



                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetVersionsForClaim executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_sClaimNumber = " & v_sClaimNumber.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Returned " & oVersionsCollection.Print.ToString & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                'oSAM.Close()
                oGetVersionsForClaimRequest = Nothing
                oGetVersionsForClaimResponse = Nothing
            End Try


            Return oVersionsCollection

        End SyncLock

    End Function

    Public Overrides Function MaintainClaim(ByVal v_oClaimMaintain As ClaimOpen,
                                             ByVal v_bTimeStamp As Byte(),
                                             Optional ByVal v_sBranchCode As String = Nothing) As ClaimResponse
        SyncLock oLock
            'Dim oSAM As PureClaimServiceClient
            Dim oMaintainClaimRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.MaintainClaimCommand
            Dim oMaintainClaimResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.MaintainClaimCommandResponse
            Dim oMaintainClaim As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimMaintainType = Nothing
            Dim iAddress As Integer = 0
            Dim iPeril As Integer = 0
            Dim iRecovery As Integer = 0
            Dim iReserve As Integer = 0
            Dim iClient As Integer = 0
            Dim iInsurer As Integer = 0
            Dim iClaimContact As Integer = 0
            Dim oClaimPeril As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPerilMaintainType
            Dim oContact As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseContactType
            Dim oContactForInsurer As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseContactType
            Dim v_oClaimResponse As New ClaimResponse()
            Dim oWarningCollection As WarningCollection
            Dim oWarn As Warnings
            Dim sbLogMessage As StringBuilder

            Try
                'oSAM = InitializeClaimServiceMethod()
                oMaintainClaimRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.MaintainClaimCommand
                oMaintainClaimResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.MaintainClaimCommandResponse
                oWarningCollection = New WarningCollection
                oWarn = New Warnings
                sbLogMessage = New StringBuilder


                With oMaintainClaimRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    '.WCFSecurityToken = SecurityToken()
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

                    oMaintainClaim = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimMaintainType()
                    oMaintainClaim.CatastropheCode = v_oClaimMaintain.CatastropheCode
                    oMaintainClaim.ClaimPeril = New List(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPerilMaintainType)
                    For iPeril = 0 To v_oClaimMaintain.ClaimPeril.Count - 1
                        oClaimPeril = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPerilMaintainType()
                        oClaimPeril.Description = v_oClaimMaintain.ClaimPeril(iPeril).Description
                        oClaimPeril.BaseClaimPerilKey = v_oClaimMaintain.ClaimPeril(iPeril).BaseClaimPerilKey
                        oClaimPeril.BaseClaimPerilKeySpecified = IIf(v_oClaimMaintain.ClaimPeril(iPeril).BaseClaimPerilKey > 0, True, False)
                        Dim iCount As Integer = (v_oClaimMaintain.ClaimPeril(iPeril).SalvageRecovery.Count) + (v_oClaimMaintain.ClaimPeril(iPeril).TPRecovery.Count)
                        Dim oClaimRecovery As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPerilRecoveryType
                        Dim iRecoveries As Integer = 0
                        Dim oClaimReserve As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPerilReserveType
                        oClaimPeril.Recovery = New List(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPerilRecoveryType)
                        For iRecovery = 0 To v_oClaimMaintain.ClaimPeril(iPeril).SalvageRecovery.Count - 1
                            If v_oClaimMaintain.ClaimPeril(iPeril).SalvageRecovery(iRecovery).CurrentRecovery <> 0 Or v_oClaimMaintain.ClaimPeril(iPeril).SalvageRecovery(iRecovery).IsDeleted = True Or v_oClaimMaintain.ClaimPeril(iPeril).SalvageRecovery(iRecovery).IsNew = True Then
                                oClaimRecovery = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPerilRecoveryType()
                                oClaimRecovery.BaseRecoveryKey = v_oClaimMaintain.ClaimPeril(iPeril).SalvageRecovery(iRecovery).BaseRecoveryKey
                                oClaimRecovery.Initialamount = v_oClaimMaintain.ClaimPeril(iPeril).SalvageRecovery(iRecovery).InitialRecovery
                                oClaimRecovery.IsDeletedRecovery = v_oClaimMaintain.ClaimPeril(iPeril).SalvageRecovery(iRecovery).IsDeleted
                                'oClaimRecovery(iRecovery).RecoveryPartyCode = v_oClaimMaintain.ClaimPeril(iPeril).SalvageRecovery(iRecovery).PartyReceiptCode
                                ' oClaimRecovery.RecoveryPartyTypeCode = v_oClaimMaintain.ClaimPeril(iPeril).SalvageRecovery(iRecovery).ReceiptPartyType
                                oClaimRecovery.RevisionAmount = v_oClaimMaintain.ClaimPeril(iPeril).SalvageRecovery(iRecovery).CurrentRecovery
                                oClaimRecovery.TypeCode = v_oClaimMaintain.ClaimPeril(iPeril).SalvageRecovery(iRecovery).TypeCode
                                oClaimRecovery.RecoveryPartyKey = v_oClaimMaintain.ClaimPeril(iPeril).SalvageRecovery(iRecovery).RecoveryPartyKey
                                oClaimRecovery.RecoveryPartyTypeKey = v_oClaimMaintain.ClaimPeril(iPeril).SalvageRecovery(iRecovery).RecoveryPartyTypeId
                                iRecoveries += 1
                                oClaimPeril.Recovery.Add(oClaimRecovery)
                            End If

                        Next
                        For iRecovery = 0 To v_oClaimMaintain.ClaimPeril(iPeril).TPRecovery.Count - 1
                            If v_oClaimMaintain.ClaimPeril(iPeril).TPRecovery(iRecovery).CurrentRecovery <> 0 Or v_oClaimMaintain.ClaimPeril(iPeril).TPRecovery(iRecovery).IsDeleted = True Or v_oClaimMaintain.ClaimPeril(iPeril).TPRecovery(iRecovery).IsNew = True Then
                                oClaimRecovery = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPerilRecoveryType()
                                oClaimRecovery.BaseRecoveryKey = v_oClaimMaintain.ClaimPeril(iPeril).TPRecovery(iRecovery).BaseRecoveryKey
                                oClaimRecovery.Initialamount = v_oClaimMaintain.ClaimPeril(iPeril).TPRecovery(iRecovery).InitialRecovery
                                oClaimRecovery.IsDeletedRecovery = v_oClaimMaintain.ClaimPeril(iPeril).TPRecovery(iRecovery).IsDeleted
                                ' oClaimRecovery.RecoveryPartyCode = v_oClaimMaintain.ClaimPeril(iPeril).TPRecovery(iRecovery).PartyReceiptCode
                                ' oClaimRecovery.RecoveryPartyTypeCode = v_oClaimMaintain.ClaimPeril(iPeril).TPRecovery(iRecovery).ReceiptPartyType
                                oClaimRecovery.RevisionAmount = v_oClaimMaintain.ClaimPeril(iPeril).TPRecovery(iRecovery).CurrentRecovery
                                oClaimRecovery.TypeCode = v_oClaimMaintain.ClaimPeril(iPeril).TPRecovery(iRecovery).TypeCode
                                oClaimRecovery.RecoveryPartyKey = v_oClaimMaintain.ClaimPeril(iPeril).TPRecovery(iRecovery).RecoveryPartyKey
                                oClaimRecovery.RecoveryPartyTypeKey = v_oClaimMaintain.ClaimPeril(iPeril).TPRecovery(iRecovery).RecoveryPartyTypeId

                                iRecoveries += 1
                                oClaimPeril.Recovery.Add(oClaimRecovery)
                            End If
                        Next


                        oClaimPeril.Reserve = New List(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPerilReserveType)
                        For iReserve = 0 To v_oClaimMaintain.ClaimPeril(iPeril).Reserve.Count - 1
                            If v_oClaimMaintain.ClaimPeril(iPeril).Reserve(iReserve).CurrentReserve <> 0 Then
                                oClaimReserve = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPerilReserveType()
                                oClaimReserve.RevisionAmount = v_oClaimMaintain.ClaimPeril(iPeril).Reserve(iReserve).CurrentReserve
                                oClaimReserve.TypeCode = v_oClaimMaintain.ClaimPeril(iPeril).Reserve(iReserve).TypeCode
                                oClaimPeril.Reserve.Add(oClaimReserve)
                            End If
                        Next
                        oClaimPeril.TypeCode = v_oClaimMaintain.ClaimPeril(iPeril).TypeCode
                        oMaintainClaim.ClaimPeril.Add(oClaimPeril)
                    Next


                    oMaintainClaim.BaseClaimKey = v_oClaimMaintain.BaseClaimKey
                    oMaintainClaim.ClaimStatus = v_oClaimMaintain.ClaimStatus
                    oMaintainClaim.ClaimStatusDate = v_oClaimMaintain.ClaimStatusDate
                    oMaintainClaim.ClaimVersionDescription = v_oClaimMaintain.ClaimVersionDescription

                    oMaintainClaim.ClientEmail = v_oClaimMaintain.ClientEmail
                    oMaintainClaim.ClientFaxNo = v_oClaimMaintain.ClientFaxNo
                    oMaintainClaim.ClientMobileNo = v_oClaimMaintain.ClientMobileNo
                    oMaintainClaim.ClientTelNo = v_oClaimMaintain.ClientTelNo
                    oMaintainClaim.ClientTelNoOff = v_oClaimMaintain.ClientTelNoOff
                    oMaintainClaim.Comments = v_oClaimMaintain.Comments
                    oMaintainClaim.Description = v_oClaimMaintain.Description
                    oMaintainClaim.TPA = v_oClaimMaintain.TPA 'WPR08
                    oMaintainClaim.HandlerCode = v_oClaimMaintain.HandlerCode
                    oMaintainClaim.InfoOnly = v_oClaimMaintain.InfoOnly
                    oMaintainClaim.LastModifiedDate = v_oClaimMaintain.LastModifiedDate
                    oMaintainClaim.LikelyClaim = v_oClaimMaintain.LikelyClaim
                    oMaintainClaim.Location = v_oClaimMaintain.Location
                    oMaintainClaim.LossFromDate = v_oClaimMaintain.LossFromDate
                    oMaintainClaim.LossToDate = v_oClaimMaintain.LossToDate
                    'oMaintainClaim.LossToDateSpecified = v_oClaimMaintain.LossToDateSpecified
                    If oMaintainClaim.LossToDate <> Date.MinValue Then
                        oMaintainClaim.LossToDate = oMaintainClaim.LossToDate
                        oMaintainClaim.LossToDateSpecified = True
                    Else
                        oMaintainClaim.LossToDateSpecified = False
                    End If
                    oMaintainClaim.PrimaryCauseCode = v_oClaimMaintain.PrimaryCauseCode
                    oMaintainClaim.ProgressStatusCode = UCase(v_oClaimMaintain.ProgressStatusCode)
                    oMaintainClaim.ReportedDate = v_oClaimMaintain.ReportedDate
                    oMaintainClaim.SecondaryCauseCode = v_oClaimMaintain.SecondaryCauseCode
                    oMaintainClaim.TownCode = v_oClaimMaintain.TownCode
                    oMaintainClaim.UserDefFldACode = v_oClaimMaintain.UserDefFldACode
                    oMaintainClaim.UserDefFldBCode = v_oClaimMaintain.UserDefFldBCode
                    oMaintainClaim.UserDefFldCCode = v_oClaimMaintain.UserDefFldCCode
                    oMaintainClaim.UserDefFldDCode = v_oClaimMaintain.UserDefFldDCode
                    oMaintainClaim.UserDefFldECode = v_oClaimMaintain.UserDefFldECode
                    oMaintainClaim.CloseClaimOnZeroReserveRecoveryBalance = v_oClaimMaintain.CloseClaimOnZeroReserveRecoveryBalance
                    'oMaintainClaim.ExternalHandler
                    oMaintainClaim.IgnoreWarnings = True
                    If v_oClaimMaintain.Client IsNot Nothing Then
                        oMaintainClaim.Client = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPartyClientType
                        oMaintainClaim.Client.Address = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseAddressType
                        oMaintainClaim.Client.Address.AddressLine1 = v_oClaimMaintain.Client.Address.Address1
                        oMaintainClaim.Client.Address.AddressLine2 = v_oClaimMaintain.Client.Address.Address2
                        oMaintainClaim.Client.Address.AddressLine3 = v_oClaimMaintain.Client.Address.Address3
                        oMaintainClaim.Client.Address.AddressLine4 = v_oClaimMaintain.Client.Address.Address4
                        oMaintainClaim.Client.Address.AddressTypeCode = v_oClaimMaintain.Client.Address.AddressType
                        oMaintainClaim.Client.Address.CountryCode = v_oClaimMaintain.Client.Address.CountryCode
                        oMaintainClaim.Client.Address.PostCode = v_oClaimMaintain.Client.Address.PostCode
                        oMaintainClaim.Client.TaxRegistered = v_oClaimMaintain.Client.TaxRegistered
                        oMaintainClaim.Client.TaxRegistrationNumber = v_oClaimMaintain.Client.TaxRegistrationNumber
                    End If

                    oMaintainClaim.Client.PartyEmail = v_oClaimMaintain.Client.ClientEmail
                    oMaintainClaim.Client.PartyFaxNo = v_oClaimMaintain.Client.ClientFaxNo
                    oMaintainClaim.Client.PartyMobileNo = v_oClaimMaintain.Client.ClientMobileNo
                    oMaintainClaim.Client.PartyTelNo = v_oClaimMaintain.Client.ClientTelNo
                    oMaintainClaim.Client.PartyTelNoOff = v_oClaimMaintain.Client.ClientTelNoOff
                    oMaintainClaim.Client.PartyClaimNumber = v_oClaimMaintain.Client.PartyClaimNumber
                    oMaintainClaim.Client.TaxRegistered = v_oClaimMaintain.Client.TaxRegistered
                    oMaintainClaim.Client.TaxRegistrationNumber = v_oClaimMaintain.Client.TaxRegistrationNumber

                    oMaintainClaim.Client.Contact = New List(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseContactType)
                    For iClient = 0 To v_oClaimMaintain.Client.Contact.Count - 1
                        If Not String.IsNullOrEmpty(v_oClaimMaintain.Client.Contact(iClient).Number) Then
                            oContact = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseContactType
                            With v_oClaimMaintain.Client.Contact
                                oContact.AreaCode = v_oClaimMaintain.Client.Contact(iClient).AreaCode
                                oContact.ContactDetail = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseContactDetailType
                                oContact.ContactDetail.ItemElementName = v_oClaimMaintain.Client.Contact(iClient).ContactDetailType
                                oContact.ContactDetail.Item = v_oClaimMaintain.Client.Contact(iClient).Number
                                oContact.ContactTypeCode = v_oClaimMaintain.Client.Contact(iClient).ContactType
                                oContact.Description = v_oClaimMaintain.Client.Contact(iClient).Description
                                oContact.Extension = v_oClaimMaintain.Client.Contact(iClient).Extension
                            End With
                            oMaintainClaim.Client.Contact.Add(oContact)
                        End If
                    Next

                    'Added for Insure type also ...Begin
                    oMaintainClaim.Insurer = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPartyInsurerType
                    If v_oClaimMaintain.Insurer IsNot Nothing AndAlso v_oClaimMaintain.Insurer.Address IsNot Nothing Then
                        If v_oClaimMaintain.Insurer.Address.Address1 <> String.Empty Then
                            oMaintainClaim.Insurer.Address = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseAddressType
                            oMaintainClaim.Insurer.Address.AddressLine1 = v_oClaimMaintain.Insurer.Address.Address1
                            oMaintainClaim.Insurer.Address.AddressLine2 = v_oClaimMaintain.Insurer.Address.Address2
                            oMaintainClaim.Insurer.Address.AddressLine3 = v_oClaimMaintain.Insurer.Address.Address3
                            oMaintainClaim.Insurer.Address.AddressLine4 = v_oClaimMaintain.Insurer.Address.Address4
                            oMaintainClaim.Insurer.Address.AddressTypeCode = v_oClaimMaintain.Insurer.Address.AddressType
                            oMaintainClaim.Insurer.Address.PostCode = v_oClaimMaintain.Insurer.Address.PostCode
                            If Not String.IsNullOrEmpty(v_oClaimMaintain.Insurer.Address.CountryCode) Then
                                oMaintainClaim.Insurer.Address.CountryCode = v_oClaimMaintain.Insurer.Address.CountryCode
                            End If
                        End If
                        oMaintainClaim.Insurer.PartyClaimNumber = v_oClaimMaintain.Insurer.PartyClaimNumber
                        oMaintainClaim.Insurer.InsurerContact = v_oClaimMaintain.Insurer.InsurerContact
                        oMaintainClaim.Insurer.InsurerEmail = v_oClaimMaintain.Insurer.InsurerEmail
                        oMaintainClaim.Insurer.InsurerFaxNo = v_oClaimMaintain.Insurer.InsurerFaxNo
                        oMaintainClaim.Insurer.InsurerTelNo = v_oClaimMaintain.Insurer.InsurerTelNo

                        oMaintainClaim.Insurer.Contact = New List(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseContactType)
                        For iInsurer = 0 To v_oClaimMaintain.Insurer.Contact.Count - 1
                            If Not String.IsNullOrEmpty(v_oClaimMaintain.Insurer.Contact(iInsurer).Number) Then
                                oContactForInsurer = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseContactType
                                With v_oClaimMaintain.Insurer.Contact
                                    oContactForInsurer.AreaCode = v_oClaimMaintain.Insurer.Contact(iInsurer).AreaCode
                                    oContactForInsurer.ContactDetail = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseContactDetailType
                                    oContactForInsurer.ContactDetail.ItemElementName = v_oClaimMaintain.Insurer.Contact(iInsurer).ContactDetailType
                                    oContactForInsurer.ContactDetail.Item = v_oClaimMaintain.Insurer.Contact(iInsurer).Number
                                    oContactForInsurer.ContactTypeCode = v_oClaimMaintain.Insurer.Contact(iInsurer).ContactType
                                    oContactForInsurer.Description = v_oClaimMaintain.Insurer.Contact(iInsurer).Description
                                    oContactForInsurer.Extension = v_oClaimMaintain.Insurer.Contact(iInsurer).Extension
                                End With
                                oMaintainClaim.Insurer.Contact.Add(oContactForInsurer)
                            End If
                        Next

                        oMaintainClaim.Insurer.ContactName = v_oClaimMaintain.Insurer.ContactName
                    End If
                    'To skip the posting if it is set True
                    oMaintainClaim.ReserveOnly = v_oClaimMaintain.ReserveOnly
                    .Claim = oMaintainClaim
                    .ApiTimeStamp = v_bTimeStamp
                End With



                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    ' oMaintainClaimResponse = oSAM.MaintainClaim(oMaintainClaimRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.MaintainClaim, oMaintainClaimRequest)
                    oMaintainClaimResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.MaintainClaimCommandResponse)(result)

                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                ' Disposing the SAM's object



                With oMaintainClaimResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        v_oClaimResponse.BaseClaimKey = .BaseClaimKey
                        v_oClaimResponse.ClaimKey = .ClaimKey
                        v_oClaimResponse.ClaimNumber = .ClaimNumber
                        v_oClaimResponse.ResultingStatus = .ResultingStatus
                        v_oClaimResponse.TimeStamp = .ApiTimeStamp
                        v_oClaimResponse.Version = .Version
                        If .Warnings IsNot Nothing AndAlso .Warnings.Count > 0 Then

                            For Each oWarning As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimResponseTypeWarnings In .Warnings
                                oWarn.Code = oWarning.Code
                                oWarn.Description = oWarning.Description
                                oWarningCollection.Add(oWarn)
                            Next
                            v_oClaimResponse.Warnings = oWarningCollection
                        End If
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("MaintainClaim executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_btimeStampField = " & v_bTimeStamp.ToString & vbCrLf)
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
                'oSAM.Close()
                oMaintainClaimRequest = Nothing
                oMaintainClaimResponse = Nothing
            End Try

            Return v_oClaimResponse
        End SyncLock

    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_oCaseKey"></param>
    ''' <param name="v_oCaseNumber"></param>
    ''' <param name="v_oCaseOpenDate"></param>
    ''' <param name="v_oAssistant"></param>
    ''' <param name="v_oAnalyst"></param>
    ''' <param name="v_oProgressStatusCode"></param>
    ''' <param name="v_oEventDescription"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function SaveCase(Optional ByVal v_oCaseKey As Integer = 0,
                                        Optional ByVal v_oCaseNumber As String = Nothing,
                                        Optional ByVal v_oCaseOpenDate As Date = Nothing,
                                        Optional ByVal v_oAssistant As String = Nothing,
                                        Optional ByVal v_oAnalyst As String = Nothing,
                                        Optional ByVal v_oProgressStatusCode As String = Nothing,
                                        Optional ByVal v_oEventDescription As String = Nothing,
                                        Optional ByVal v_sBranchCode As String = Nothing) As CaseDetails

        SyncLock oLock
            'Dim oSAM As PureClaimServiceClient 'SAMForInsuranceV2's Object
            Dim oSaveCaseRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.SaveCaseCommand ' Request Type
            Dim oSaveCaseResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.SaveCaseCommandResponse    ' Response Type
            Dim sbLogMessage As StringBuilder
            Dim oSaveCaseDetails As NexusProvider.CaseDetails = Nothing
            Try
                'oSAM = InitializeClaimServiceMethod()
                oSaveCaseRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.SaveCaseCommand
                oSaveCaseResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.SaveCaseCommandResponse

                With oSaveCaseRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    '.WCFSecurityToken = SecurityToken()
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
                    If v_oCaseKey > 0 Then
                        .CaseKey = v_oCaseKey
                        .CaseKeySpecified = True
                    End If
                    .Analyst = v_oAnalyst
                    .Assistant = v_oAssistant

                    If String.IsNullOrEmpty(v_oCaseNumber) Then
                        .CaseNumber = Nothing
                    Else
                        .CaseNumber = v_oCaseNumber
                    End If

                    If v_oCaseOpenDate <> Date.MinValue Then
                        .CaseOpenDate = v_oCaseOpenDate
                    End If
                    .EventDescription = v_oEventDescription
                    .ProgressStatusCode = v_oProgressStatusCode
                End With

                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    'oSaveCaseResponse = oSAM.SaveCase(oSaveCaseRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.SaveCase, oSaveCaseRequest)
                    oSaveCaseResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.SaveCaseCommandResponse)(result)

                End Using


                oSaveCaseDetails = New NexusProvider.CaseDetails
                'Dim oSaveCaseDetailsColl As New NexusProvider.CaseCollection

                With oSaveCaseResponse  'With Response Type
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        oSaveCaseDetails.CaseKey = .CaseKey
                        oSaveCaseDetails.CaseNumber = .CaseNumber
                        oSaveCaseDetails.BaseCaseKey = .BaseCaseKey
                    End If
                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage = New StringBuilder
                    sbLogMessage.AppendLine("SaveCase executed ok" & vbCrLf)
                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)
                'FaultErrorHandler(ex) ' handling fault error messages 
            Catch ex As Exception
                Throw
            Finally
                'oSAM.Close()
                oSaveCaseRequest = Nothing
                oSaveCaseResponse = Nothing
                sbLogMessage = Nothing
            End Try
            Return oSaveCaseDetails
        End SyncLock
    End Function


    Public Overrides Function OpenClaim(ByVal v_oClaimOpen As ClaimOpen,
                                              Optional ByVal v_sBranchCode As String = Nothing) As ClaimResponse
        SyncLock oLock
            'Dim oSAM As PureClaimServiceClient
            Dim oOpenClaimRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.OpenClaimCommand
            Dim oOpenClaimResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.OpenClaimCommandResponse
            Dim oOpenClaim As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimOpenType = Nothing
            Dim iAddress As Integer = 0
            Dim iPeril As Integer = 0
            Dim iRecovery As Integer = 0
            Dim iReserve As Integer = 0
            Dim iClient As Integer = 0
            Dim iInsurer As Integer = 0
            Dim iClaimContact As Integer = 0
            Dim oClaimPeril As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPerilType
            Dim oClaimRecovery As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPerilRecoveryType
            Dim oClaimReserve As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPerilReserveType
            Dim oContact As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseContactType
            Dim oContactForInsurer As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseContactType
            Dim oClaimResponse As ClaimResponse = Nothing
            Dim oWarningCollection As WarningCollection
            Dim oWarn As Warnings
            Dim sbLogMessage As StringBuilder

            Try
                'oSAM = InitializeClaimServiceMethod()
                oOpenClaimRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.OpenClaimCommand
                oOpenClaimResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.OpenClaimCommandResponse
                oWarningCollection = New WarningCollection
                oWarn = New Warnings
                sbLogMessage = New StringBuilder


                With oOpenClaimRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    '.WCFSecurityToken = SecurityToken()
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

                    oOpenClaim = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimOpenType()
                    oOpenClaim.CatastropheCode = v_oClaimOpen.CatastropheCode
                    oOpenClaim.DuplicateClaimOverrideUserName = v_oClaimOpen.DuplicateClaimOverrideUserName
                    oOpenClaim.DuplicateClaimOverrideUserPassword = v_oClaimOpen.DuplicateClaimOverrideUserPassword

                    If v_oClaimOpen.BaseCaseKey > 0 Then
                        oOpenClaim.BaseCaseKey = v_oClaimOpen.BaseCaseKey
                    End If

                    If v_oClaimOpen.ClaimPeril IsNot Nothing Then
                        oOpenClaim.ClaimPeril = New List(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPerilType)
                        For iPeril = 0 To v_oClaimOpen.ClaimPeril.Count - 1
                            oClaimPeril = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPerilType()
                            oClaimPeril.Description = v_oClaimOpen.ClaimPeril(iPeril).Description
                            Dim iCount As Integer = (v_oClaimOpen.ClaimPeril(iPeril).SalvageRecovery.Count) + (v_oClaimOpen.ClaimPeril(iPeril).TPRecovery.Count)
                            Dim iRecoveries As Integer = 0
                            If iCount > 0 Then
                                oClaimPeril.Recovery = New List(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPerilRecoveryType)
                                For iRecovery = 0 To v_oClaimOpen.ClaimPeril(iPeril).SalvageRecovery.Count - 1
                                    oClaimRecovery = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPerilRecoveryType()
                                    oClaimRecovery.RevisionAmount = v_oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iRecovery).RevisionAmount + v_oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iRecovery).InitialRecovery
                                    oClaimRecovery.TypeCode = v_oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iRecovery).TypeCode
                                    oClaimRecovery.RecoveryPartyKey = v_oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iRecovery).RecoveryPartyKey
                                    oClaimRecovery.RecoveryPartyTypeKey = v_oClaimOpen.ClaimPeril(iPeril).SalvageRecovery(iRecovery).RecoveryPartyTypeId
                                    iRecoveries += 1
                                    oClaimPeril.Recovery.Add(oClaimRecovery)
                                Next
                                For iRecovery = 0 To v_oClaimOpen.ClaimPeril(iPeril).TPRecovery.Count - 1
                                    oClaimRecovery = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPerilRecoveryType()
                                    oClaimRecovery.RevisionAmount = v_oClaimOpen.ClaimPeril(iPeril).TPRecovery(iRecovery).RevisionAmount + v_oClaimOpen.ClaimPeril(iPeril).TPRecovery(iRecovery).InitialRecovery
                                    oClaimRecovery.TypeCode = v_oClaimOpen.ClaimPeril(iPeril).TPRecovery(iRecovery).TypeCode
                                    oClaimRecovery.RecoveryPartyKey = v_oClaimOpen.ClaimPeril(iPeril).TPRecovery(iRecovery).RecoveryPartyKey
                                    oClaimRecovery.RecoveryPartyTypeKey = v_oClaimOpen.ClaimPeril(iPeril).TPRecovery(iRecovery).RecoveryPartyTypeId
                                    iRecoveries += 1
                                    oClaimPeril.Recovery.Add(oClaimRecovery)
                                Next

                            End If
                            If v_oClaimOpen.ClaimPeril(iPeril).Reserve.Count > 0 Then
                                oClaimPeril.Reserve = New List(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPerilReserveType)
                                For iReserve = 0 To v_oClaimOpen.ClaimPeril(iPeril).Reserve.Count - 1
                                    oClaimReserve = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPerilReserveType()
                                    'Incase of Open Claim this should always be Initial Reserve
                                    oClaimReserve.RevisionAmount = v_oClaimOpen.ClaimPeril(iPeril).Reserve(iReserve).InitialReserve
                                    oClaimReserve.TypeCode = v_oClaimOpen.ClaimPeril(iPeril).Reserve(iReserve).TypeCode
                                    oClaimPeril.Reserve.Add(oClaimReserve)
                                Next

                            End If
                            oClaimPeril.TypeCode = v_oClaimOpen.ClaimPeril(iPeril).TypeCode
                            oOpenClaim.ClaimPeril.Add(oClaimPeril)
                        Next


                    End If

                    oOpenClaim.ClaimStatus = v_oClaimOpen.ClaimStatus
                    oOpenClaim.ClaimStatusDate = v_oClaimOpen.ClaimStatusDate
                    oOpenClaim.ClaimVersion = v_oClaimOpen.ClaimVersion
                    oOpenClaim.ClaimVersionDescription = v_oClaimOpen.ClaimVersionDescription

                    oOpenClaim.ClientEmail = v_oClaimOpen.ClientEmail
                    oOpenClaim.ClientFaxNo = v_oClaimOpen.ClientFaxNo
                    oOpenClaim.ClientMobileNo = v_oClaimOpen.ClientMobileNo
                    oOpenClaim.ClientTelNo = v_oClaimOpen.ClientTelNo
                    oOpenClaim.ClientTelNoOff = v_oClaimOpen.ClientTelNoOff
                    oOpenClaim.Comments = v_oClaimOpen.Comments
                    oOpenClaim.CurrencyCode = v_oClaimOpen.CurrencyISOCode
                    oOpenClaim.Description = v_oClaimOpen.Description
                    oOpenClaim.HandlerCode = v_oClaimOpen.HandlerCode
                    oOpenClaim.PrimaryCauseCode = v_oClaimOpen.PrimaryCauseCode
                    oOpenClaim.HandlerCode = v_oClaimOpen.HandlerCode
                    oOpenClaim.InfoOnly = v_oClaimOpen.InfoOnly
                    oOpenClaim.LastModifiedDate = v_oClaimOpen.LastModifiedDate
                    oOpenClaim.LikelyClaim = v_oClaimOpen.LikelyClaim
                    oOpenClaim.Location = v_oClaimOpen.Location
                    oOpenClaim.LossFromDate = v_oClaimOpen.LossFromDate
                    oOpenClaim.TPA = v_oClaimOpen.TPA 'WPR08
                    If v_oClaimOpen.LossToDate <> Date.MinValue Then
                        oOpenClaim.LossToDate = v_oClaimOpen.LossToDate
                        oOpenClaim.LossToDateSpecified = True
                    Else
                        oOpenClaim.LossToDateSpecified = False
                    End If

                    oOpenClaim.PrimaryCauseCode = v_oClaimOpen.PrimaryCauseCode
                    oOpenClaim.ProgressStatusCode = UCase(v_oClaimOpen.ProgressStatusCode)
                    oOpenClaim.ReportedDate = v_oClaimOpen.ReportedDate
                    oOpenClaim.SecondaryCauseCode = v_oClaimOpen.SecondaryCauseCode
                    oOpenClaim.TownCode = v_oClaimOpen.TownCode
                    oOpenClaim.UserDefFldACode = v_oClaimOpen.UserDefFldACode
                    oOpenClaim.UserDefFldBCode = v_oClaimOpen.UserDefFldBCode
                    oOpenClaim.UserDefFldCCode = v_oClaimOpen.UserDefFldCCode
                    oOpenClaim.UserDefFldDCode = v_oClaimOpen.UserDefFldDCode
                    oOpenClaim.UserDefFldECode = v_oClaimOpen.UserDefFldECode
                    oOpenClaim.RiskKey = v_oClaimOpen.RiskKey
                    oOpenClaim.InsuranceFileKey = v_oClaimOpen.InsuranceFileKey

                    If v_oClaimOpen.Client IsNot Nothing Then
                        oOpenClaim.Client = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPartyClientType
                        oOpenClaim.Client.Address = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseAddressType
                        If v_oClaimOpen.Client.Address IsNot Nothing Then
                            oOpenClaim.Client.Address.AddressLine1 = v_oClaimOpen.Client.Address.Address1
                            oOpenClaim.Client.Address.AddressLine2 = v_oClaimOpen.Client.Address.Address2
                            oOpenClaim.Client.Address.AddressLine3 = v_oClaimOpen.Client.Address.Address3
                            oOpenClaim.Client.Address.AddressLine4 = v_oClaimOpen.Client.Address.Address4
                            oOpenClaim.Client.Address.AddressTypeCode = v_oClaimOpen.Client.Address.AddressType
                            If Not String.IsNullOrEmpty(v_oClaimOpen.Client.Address.CountryCode) Then
                                oOpenClaim.Client.Address.CountryCode = v_oClaimOpen.Client.Address.CountryCode
                            End If
                        End If
                        oOpenClaim.Client.Address.PostCode = v_oClaimOpen.Client.Address.PostCode
                        oOpenClaim.Client.PartyClaimNumber = v_oClaimOpen.Client.PartyClaimNumber
                        oOpenClaim.Client.TaxRegistered = v_oClaimOpen.Client.TaxRegistered
                        oOpenClaim.Client.TaxRegistrationNumber = v_oClaimOpen.Client.TaxRegistrationNumber

                        oOpenClaim.Client.Contact = New List(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseContactType)
                        For iClient = 0 To v_oClaimOpen.Client.Contact.Count - 1
                            If Not String.IsNullOrEmpty(v_oClaimOpen.Client.Contact(iClient).Number) Then
                                oContact = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseContactType
                                With v_oClaimOpen.Client.Contact
                                    oContact.AreaCode = v_oClaimOpen.Client.Contact(iClient).AreaCode
                                    oContact.ContactDetail = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseContactDetailType
                                    oContact.ContactDetail.ItemElementName = v_oClaimOpen.Client.Contact(iClient).ContactDetailType
                                    oContact.ContactDetail.Item = v_oClaimOpen.Client.Contact(iClient).Number
                                    oContact.ContactTypeCode = v_oClaimOpen.Client.Contact(iClient).ContactType
                                    oContact.Description = v_oClaimOpen.Client.Contact(iClient).Description
                                    oContact.Extension = v_oClaimOpen.Client.Contact(iClient).Extension
                                    oOpenClaim.Client.Contact.Add(oContact)
                                End With
                            End If
                        Next

                    End If

                    'Added for Insure type also ...Begin
                    If v_oClaimOpen.Insurer IsNot Nothing Then
                        oOpenClaim.Insurer = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPartyInsurerType
                        If v_oClaimOpen.Insurer.Address.Address1 <> String.Empty Then
                            oOpenClaim.Insurer.Address = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseAddressType
                            oOpenClaim.Insurer.Address.AddressLine1 = v_oClaimOpen.Insurer.Address.Address1
                            oOpenClaim.Insurer.Address.AddressLine2 = v_oClaimOpen.Insurer.Address.Address2
                            oOpenClaim.Insurer.Address.AddressLine3 = v_oClaimOpen.Insurer.Address.Address3
                            oOpenClaim.Insurer.Address.AddressLine4 = v_oClaimOpen.Insurer.Address.Address4
                            oOpenClaim.Insurer.Address.AddressTypeCode = v_oClaimOpen.Insurer.Address.AddressType
                            If Not String.IsNullOrEmpty(v_oClaimOpen.Insurer.Address.CountryCode) Then
                                oOpenClaim.Insurer.Address.CountryCode = v_oClaimOpen.Insurer.Address.CountryCode
                            End If
                            oOpenClaim.Insurer.Address.PostCode = v_oClaimOpen.Insurer.Address.PostCode
                        End If
                        oOpenClaim.Insurer.PartyClaimNumber = v_oClaimOpen.Insurer.PartyClaimNumber
                        oOpenClaim.Insurer.InsurerContact = v_oClaimOpen.Insurer.InsurerContact
                        oOpenClaim.Insurer.InsurerEmail = v_oClaimOpen.Insurer.InsurerEmail
                        oOpenClaim.Insurer.InsurerFaxNo = v_oClaimOpen.Insurer.InsurerFaxNo
                        oOpenClaim.Insurer.InsurerTelNo = v_oClaimOpen.Insurer.InsurerTelNo
                        oOpenClaim.Insurer.Contact = New List(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseContactType)
                        For iInsurer = 0 To v_oClaimOpen.Insurer.Contact.Count - 1
                            If Not String.IsNullOrEmpty(v_oClaimOpen.Insurer.Contact(iInsurer).Number) Then
                                oContactForInsurer = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseContactType
                                With v_oClaimOpen.Insurer.Contact
                                    oContactForInsurer.AreaCode = v_oClaimOpen.Insurer.Contact(iInsurer).AreaCode
                                    oContactForInsurer.ContactDetail = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseContactDetailType
                                    oContactForInsurer.ContactDetail.ItemElementName = v_oClaimOpen.Insurer.Contact(iInsurer).ContactDetailType
                                    oContactForInsurer.ContactDetail.Item = v_oClaimOpen.Insurer.Contact(iInsurer).Number
                                    oContactForInsurer.ContactTypeCode = v_oClaimOpen.Insurer.Contact(iInsurer).ContactType
                                    oContactForInsurer.Description = v_oClaimOpen.Insurer.Contact(iInsurer).Description
                                    oContactForInsurer.Extension = v_oClaimOpen.Insurer.Contact(iInsurer).Extension
                                    oOpenClaim.Insurer.Contact.Add(oContactForInsurer)
                                End With
                            End If
                        Next
                        oOpenClaim.Insurer.ContactName = v_oClaimOpen.Insurer.ContactName

                    End If
                    'To skip the posting if it is set True
                    oOpenClaim.ReserveOnly = v_oClaimOpen.ReserveOnly
                    'End
                    If v_oClaimOpen.UnderwritingYearCode <> String.Empty Then
                        oOpenClaim.UnderwritingYearCode = v_oClaimOpen.UnderwritingYearCode
                    End If
                    oOpenClaim.IgnoreWarnings = True

                    .Claim = oOpenClaim
                End With


                Using trace As New Tracer(Category.Trace)
                    'oOpenClaimResponse = oSAM.OpenClaim(oOpenClaimRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.OpenClaim, oOpenClaimRequest)
                    oOpenClaimResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.OpenClaimCommandResponse)(result)
                End Using
                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.
                oOpenClaimRequest = Nothing
                oClaimResponse = New ClaimResponse
                With oOpenClaimResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        oClaimResponse.BaseClaimKey = .BaseClaimKey
                        oClaimResponse.ClaimKey = .ClaimKey
                        oClaimResponse.ClaimNumber = .ClaimNumber
                        oClaimResponse.ResultingStatus = .ResultingStatus
                        oClaimResponse.TimeStamp = .ApiTimeStamp
                        oClaimResponse.Version = .Version
                        If .Warnings IsNot Nothing AndAlso .Warnings.Count > 0 Then
                            For Each oWarning As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimResponseTypeWarnings In .Warnings

                                oWarn.Code = oWarning.Code
                                oWarn.Description = oWarning.Description
                                oWarningCollection.Add(oWarn)
                            Next
                            oClaimResponse.Warnings = oWarningCollection
                        End If
                    End If
                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("OpenClaim executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_oClaimOpen = " & v_oClaimOpen.Print.Replace("<br/>", vbCrLf))

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
                'oSAM.Close()
                oOpenClaimRequest = Nothing
                oOpenClaimResponse = Nothing
            End Try

            Return oClaimResponse

        End SyncLock

    End Function
    ''' <summary>
    ''' PayClaim
    ''' </summary>
    ''' <param name="v_oClaimPayment"></param>
    ''' <param name="v_bTimeStamp"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    Public Overrides Function PayClaim(ByVal v_oClaimPayment As ClaimPayment,
                                   ByVal v_bTimeStamp As Byte(),
                                   Optional ByVal v_sBranchCode As String = Nothing, Optional ByVal oClaimPaymentCollection As ClaimPaymentCollection = Nothing) As PayClaimResponse

        SyncLock oLock
            ' Dim oSAM As PureClaimServiceClient
            Dim oPayClaimRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.PayClaimCommand
            Dim oPayClaimResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.PayClaimCommandResponse
            Dim i As Integer = 0
            Dim oPaymentItem As SSP.PureInsuranceRestAPIHandler.BaseClasses.BasePaymentCashListItemType
            Dim oClaimPaymentItemType As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPaymentItemType
            Dim v_oPayClaimResponse As New PayClaimResponse()
            Dim oWarningCollection As New WarningCollection()
            Dim iWarning As Integer = 0
            Dim sbLogMessage As StringBuilder

            Try
                'oSAM = InitializeClaimServiceMethod()
                oPayClaimRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.PayClaimCommand
                oPayClaimResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.PayClaimCommandResponse
                sbLogMessage = New StringBuilder


                With oPayClaimRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    '.WCFSecurityToken = SecurityToken()
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

                    .ClaimPayment = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPaymentType()
                    .ClaimPayment.BaseClaimKey = v_oClaimPayment.BaseClaimKey
                    .ClaimPayment.BaseClaimPerilKey = v_oClaimPayment.BaseClaimPerilKey
                    .ClaimPayment.ClaimVersionDescription = v_oClaimPayment.ClaimVersionDescription
                    .ClaimPayment.CloseClaimOnZeroReserveRecoveryBalance = v_oClaimPayment.CloseClaimOnZeroReserveRecoveryBalance
                    .ClaimPayment.CurrencyCode = v_oClaimPayment.CurrencyCode
                    .ClaimPayment.PartyKey = v_oClaimPayment.PartyKey
                    .ClaimPayment.ClientKey = v_oClaimPayment.ClientKey
                    .ClaimPayment.PaymentPartyType = v_oClaimPayment.PaymentPartyType
                    .ClaimPayment.UltimatePayee = v_oClaimPayment.UltimatePayee
                    .ClaimPayment.IsExGratia = v_oClaimPayment.IsExGratia
                    .ApiTimeStamp = v_bTimeStamp

                    If v_oClaimPayment.PaymentAdvancedTaxDetails.InsuranceTaxNumber <> String.Empty Then
                        .ClaimPayment.AdvancedTaxDetails = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPaymentAdvancedTaxDetailsType()
                        .ClaimPayment.AdvancedTaxDetails.InsuranceTaxNumber = v_oClaimPayment.PaymentAdvancedTaxDetails.InsuranceTaxNumber
                        .ClaimPayment.AdvancedTaxDetails.InsuredDomiciled = v_oClaimPayment.PaymentAdvancedTaxDetails.InsuredDomiciled
                        .ClaimPayment.AdvancedTaxDetails.InsuredPercentage = v_oClaimPayment.PaymentAdvancedTaxDetails.InsuredPercentage
                        .ClaimPayment.AdvancedTaxDetails.IsSettlement = v_oClaimPayment.PaymentAdvancedTaxDetails.IsSettlement
                        .ClaimPayment.AdvancedTaxDetails.IsTaxExempt = v_oClaimPayment.PaymentAdvancedTaxDetails.IsTaxExempt
                        .ClaimPayment.AdvancedTaxDetails.IsWHTExempt = v_oClaimPayment.PaymentAdvancedTaxDetails.IsWHTExempt
                        .ClaimPayment.AdvancedTaxDetails.PayeeDomiciled = v_oClaimPayment.PaymentAdvancedTaxDetails.PayeeDomiciled
                        .ClaimPayment.AdvancedTaxDetails.PayeePercentage = v_oClaimPayment.PaymentAdvancedTaxDetails.PayeePercentage
                        .ClaimPayment.AdvancedTaxDetails.PayeeTaxNumber = v_oClaimPayment.PaymentAdvancedTaxDetails.PayeeTaxNumber
                        .ClaimPayment.AdvancedTaxDetails.SafeHarbourCode = v_oClaimPayment.PaymentAdvancedTaxDetails.SafeHarbourCode
                        .ClaimPayment.AdvancedTaxDetails.SafeHarbourPercentage = v_oClaimPayment.PaymentAdvancedTaxDetails.SafeHarbourPercentage
                    End If

                    If v_oClaimPayment.CashList.BankAccountCode <> String.Empty Then
                        .ClaimPayment.CashList = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BasePaymentCashListType()
                        .ClaimPayment.CashList.BankAccountCode = v_oClaimPayment.CashList.BankAccountCode
                        .ClaimPayment.CashList.CurrencyCode = v_oClaimPayment.CashList.CurrencyCode
                        .ClaimPayment.CashList.ListDate = v_oClaimPayment.CashList.ListDate
                        .ClaimPayment.CashList.Reference = v_oClaimPayment.CashList.Reference
                        .ClaimPayment.CashList.StatusCode = v_oClaimPayment.CashList.StatusCode
                        .ClaimPayment.CashList.TypeCode = v_oClaimPayment.CashList.TypeCode
                        .ClaimPayment.CashList.BranchCode = sBranchCode
                        .ClaimPayment.CashList.PaymentItem = New List(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BasePaymentCashListItemType)
                        For index As Integer = 0 To v_oClaimPayment.CashList.PaymentCashListItemType.Count - 1
                            With v_oClaimPayment.CashList.PaymentCashListItemType(index)
                                oPaymentItem = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BasePaymentCashListItemType()
                                oPaymentItem.AccountShortCode = .AccountShortCode
                                oPaymentItem.AllocationStatusCode = .AllocationStatusCode
                                oPaymentItem.Amount = .Amount

                                If .BankPaymentType.AccountCode.Trim.Length <> 0 Then
                                    oPaymentItem.Bank = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseBankPaymentType
                                    oPaymentItem.Bank.AccountCode = .BankPaymentType.AccountCode
                                    oPaymentItem.Bank.BranchCode = .BankPaymentType.BranchCode
                                    oPaymentItem.Bank.ExpiryDate = .BankPaymentType.ExpiryDate
                                    If .BankPaymentType.ExpiryDate <> DateTime.MinValue Then
                                        oPaymentItem.Bank.ExpiryDateSpecified = True
                                    Else
                                        oPaymentItem.Bank.ExpiryDateSpecified = False
                                    End If
                                    oPaymentItem.Bank.PayeeName = .BankPaymentType.PayeeName
                                    oPaymentItem.Bank.Reference1 = .BankPaymentType.Reference1
                                    oPaymentItem.Bank.Reference2 = .BankPaymentType.Reference2
                                    oPaymentItem.BankReference = .BankReference
                                    oPaymentItem.Bank.BIC = .BankPaymentType.BIC
                                    oPaymentItem.Bank.IBAN = .BankPaymentType.IBAN
                                End If

                                If Trim(.ContactAddress.Address1) <> String.Empty Then
                                    oPaymentItem.ContactAddress = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseSimpleAddressType
                                    oPaymentItem.ContactAddress.AddressLine1 = .ContactAddress.Address1
                                    oPaymentItem.ContactAddress.AddressLine2 = .ContactAddress.Address2
                                    oPaymentItem.ContactAddress.AddressLine3 = .ContactAddress.Address3
                                    oPaymentItem.ContactAddress.AddressLine4 = .ContactAddress.Address4
                                    oPaymentItem.ContactAddress.CountryCode = .ContactAddress.CountryCode
                                    oPaymentItem.ContactAddress.PostCode = .ContactAddress.PostCode
                                End If
                                oPaymentItem.ContactName = .ContactName
                                oPaymentItem.FurtherDetails = .FurtherDetails
                                oPaymentItem.MediaReference = .MediaReference
                                oPaymentItem.MediaTypeCode = .MediaTypeCode
                                oPaymentItem.OurReference = .OurReference
                                oPaymentItem.StatusCode = .StatusCode
                                oPaymentItem.TheirReference = .TheirReference
                                oPaymentItem.TypeCode = .TypeCode
                                oPaymentItem.TransactionDate = .TransactionDate
                            End With
                            .ClaimPayment.CashList.PaymentItem.Add(oPaymentItem)
                        Next

                    End If
                    .ClaimPayment.ClaimPaymentItem = New List(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPaymentItemType)
                    For index As Integer = 0 To v_oClaimPayment.ClaimPaymentItem.Count - 1
                        'If v_oClaimPayment.ClaimPaymentItem(index).PaymentAmount <> 0.0 Then
                        oClaimPaymentItemType = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPaymentItemType()
                        oClaimPaymentItemType.PaymentAmount = v_oClaimPayment.ClaimPaymentItem(index).PaymentAmount
                        oClaimPaymentItemType.BaseReserveKey = v_oClaimPayment.ClaimPaymentItem(index).BaseReserveKey
                        oClaimPaymentItemType.ReverseExcess = v_oClaimPayment.ClaimPaymentItem(index).ReverseExcess
                        oClaimPaymentItemType.TaxGroupCode = v_oClaimPayment.ClaimPaymentItem(index).TaxGroupCode
                        oClaimPaymentItemType.IsTaxOverridden = v_oClaimPayment.ClaimPaymentItem(index).IsTaxOverridden
                        oClaimPaymentItemType.OverriddedTaxAmount = v_oClaimPayment.ClaimPaymentItem(index).OverriddedTaxAmount
                        i += 1
                        .ClaimPayment.ClaimPaymentItem.Add(oClaimPaymentItemType)
                        'End If
                    Next
                    .ClaimPayment.Payee = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPayeeType()
                    If Trim(v_oClaimPayment.Payee.Address.Address1) <> String.Empty Then
                        .ClaimPayment.Payee.Address = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseAddressType()
                        .ClaimPayment.Payee.Address.AddressLine1 = v_oClaimPayment.Payee.Address.Address1
                        .ClaimPayment.Payee.Address.AddressLine2 = v_oClaimPayment.Payee.Address.Address2
                        .ClaimPayment.Payee.Address.AddressLine3 = v_oClaimPayment.Payee.Address.Address3
                        .ClaimPayment.Payee.Address.AddressLine4 = v_oClaimPayment.Payee.Address.Address4
                        .ClaimPayment.Payee.Address.AddressTypeCode = v_oClaimPayment.Payee.Address.AddressType
                        .ClaimPayment.Payee.Address.CountryCode = v_oClaimPayment.Payee.Address.CountryCode
                        If v_oClaimPayment.Payee.Address.PostCode IsNot Nothing Then
                            .ClaimPayment.Payee.Address.PostCode = v_oClaimPayment.Payee.Address.PostCode
                        End If
                    End If
                    .ClaimPayment.Payee.BankCode = v_oClaimPayment.Payee.BankCode
                    .ClaimPayment.Payee.BankName = v_oClaimPayment.Payee.BankName
                    .ClaimPayment.Payee.BankNumber = v_oClaimPayment.Payee.BankNumber
                    .ClaimPayment.Payee.Comments = v_oClaimPayment.Payee.Comments
                    .ClaimPayment.Payee.MediaReference = v_oClaimPayment.Payee.MediaReference
                    .ClaimPayment.Payee.MediaTypeCode = v_oClaimPayment.Payee.MediaTypeCode
                    .ClaimPayment.Payee.Name = v_oClaimPayment.Payee.Name
                    .ClaimPayment.Payee.TheirReference = v_oClaimPayment.Payee.TheirReference
                    .ClaimPayment.Payee.PartyBankKey = v_oClaimPayment.Payee.PartyBankKey
                    'To skip the posting if it is set to True
                    .ClaimPayment.PaymentOnly = v_oClaimPayment.PaymentOnly

                End With

                Using trace As New Tracer(Category.Trace)
                    'oPayClaimResponse = oSAM.PayClaim(oPayClaimRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.PayClaim, oPayClaimRequest)
                    oPayClaimResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.PayClaimCommandResponse)(result)
                End Using
                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                With oPayClaimResponse



                    v_oPayClaimResponse.Warnings = oWarningCollection
                    v_oPayClaimResponse.BaseClaimKey = .BaseClaimKey

                    'If .CashList IsNot Nothing Then
                    '    v_oPayClaimResponse.CashList.CashListKey = .CashList.CashListKey
                    '    v_oPayClaimResponse.CashList.TimeStamp = .cashList.ApiTimeStamp
                    '    v_oPayClaimResponse.CashList.Version = .cashList.Version
                    'End If

                    v_oPayClaimResponse.ClaimKey = .ClaimKey
                    v_oPayClaimResponse.ClaimNumber = .ClaimNumber
                    v_oPayClaimResponse.creditedAccountKey = .CreditedAccountKey
                    v_oPayClaimResponse.creditedDocumentKey = .CreditedDocumentKey
                    v_oPayClaimResponse.creditedTransdetailKey = .CreditedTransdetailKey

                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("PayClaim executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_oClaimPayment = " & v_oClaimPayment.Print.Replace("<br/>", vbCrLf))

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
                'oSAM.Close()
                oPayClaimRequest = Nothing
                oPayClaimResponse = Nothing
                i = Nothing
            End Try

            'Returns PayClaimResponse
            Return v_oPayClaimResponse

        End SyncLock
    End Function
    ''' <summary>
    ''' UpdateClaimReservesOrPayments
    ''' </summary>
    ''' <param name="v_oClaimOpen"></param>
    ''' <param name="v_oClaimPayment"></param>
    ''' <param name="v_bTimeStamp"></param>
    ''' <param name="v_iProcessType"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function UpdateClaimReservesOrPayments(ByVal v_oClaimOpen As Claim,
                                                            ByVal v_oClaimPayment As ClaimPayment,
                                                            ByVal v_bTimeStamp As Byte(),
                                                            ByVal v_iProcessType As Integer,
                                                            Optional ByVal v_sBranchCode As String = Nothing) As ClaimResponse
        SyncLock oLock

            'Dim oSAM As PureClaimServiceClient
            Dim oUpdateClaimReservesOrPaymentsRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateClaimReservesOrPaymentsCommand ' Request Type
            Dim oUpdateClaimReservesOrPaymentsResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateClaimReservesOrPaymentsCommandResponse    ' Response Type
            Dim iPeril As Integer = 0
            Dim iRecovery As Integer = 0
            Dim iReserve As Integer = 0
            Dim iPerilCount, iTotalPerilCount As Integer
            Dim oClaimPeril As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPerilType

            Dim oClaimRecovery As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPerilRecoveryType
            Dim iTotalReserveCount As Integer = 0
            Dim iReserveCount As Integer = 0
            Dim oClaimReserve As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPerilReserveType
            Dim i As Integer = 0
            Dim oPaymentItem As SSP.PureInsuranceRestAPIHandler.BaseClasses.BasePaymentCashListItemType
            Dim oClaimPaymentItemType As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPaymentItemType
            Dim oClaimResponse As ClaimResponse
            Try
                'oSAM = InitializeClaimServiceMethod()
                oUpdateClaimReservesOrPaymentsRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateClaimReservesOrPaymentsCommand
                oUpdateClaimReservesOrPaymentsResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateClaimReservesOrPaymentsCommandResponse
                oClaimResponse = New ClaimResponse


                With oUpdateClaimReservesOrPaymentsRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    '.WCFSecurityToken = SecurityToken()
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

                    'Claim Reserve
                    If v_oClaimOpen IsNot Nothing Then
                        .ClaimKey = v_oClaimOpen.ClaimKey
                        If v_oClaimOpen.ClaimPeril IsNot Nothing Then
                            'Calculate the Number of peril edited

                            iTotalPerilCount = 0
                            For iPerilCount = 0 To v_oClaimOpen.ClaimPeril.Count - 1
                                If v_oClaimOpen.ClaimPeril(iPerilCount).PerilEdited = True Then
                                    iTotalPerilCount += 1
                                End If
                            Next

                            .ClaimPeril = New List(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPerilType)
                            For iPerilCount = 0 To v_oClaimOpen.ClaimPeril.Count - 1
                                If v_oClaimOpen.ClaimPeril(iPerilCount).PerilEdited = True Then
                                    oClaimPeril = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPerilType()
                                    oClaimPeril.Description = v_oClaimOpen.ClaimPeril(iPerilCount).Description
                                    Dim iCount As Integer = (v_oClaimOpen.ClaimPeril(iPerilCount).SalvageRecovery.Count) + (v_oClaimOpen.ClaimPeril(iPerilCount).TPRecovery.Count)
                                    Dim iRecoveries As Integer = 0
                                    'Claim Recovery
                                    If iCount > 0 Then
                                        'Update the claim recovery reserve
                                        oClaimPeril.Recovery = New List(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPerilRecoveryType)
                                        For iRecovery = 0 To v_oClaimOpen.ClaimPeril(iPerilCount).SalvageRecovery.Count - 1
                                            ' If v_oClaimOpen.ClaimPeril(iPerilCount).SalvageRecovery(iRecovery).CurrentRecovery <> 0 OrElse v_oClaimOpen.ClaimPeril(iPerilCount).SalvageRecovery(iRecovery).IsDeleted = True OrElse v_oClaimOpen.ClaimPeril(iPerilCount).SalvageRecovery(iRecovery).IsNew = True Then
                                            oClaimRecovery = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPerilRecoveryType()
                                            oClaimRecovery.BaseRecoveryKey = v_oClaimOpen.ClaimPeril(iPerilCount).SalvageRecovery(iRecovery).BaseRecoveryKey
                                            oClaimRecovery.Initialamount = v_oClaimOpen.ClaimPeril(iPerilCount).SalvageRecovery(iRecovery).InitialRecovery
                                            oClaimRecovery.IsDeletedRecovery = v_oClaimOpen.ClaimPeril(iPerilCount).SalvageRecovery(iRecovery).IsDeleted
                                            oClaimRecovery.RevisionAmount = v_oClaimOpen.ClaimPeril(iPerilCount).SalvageRecovery(iRecovery).RevisionAmount
                                            oClaimRecovery.TypeCode = v_oClaimOpen.ClaimPeril(iPerilCount).SalvageRecovery(iRecovery).TypeCode
                                            oClaimRecovery.RecoveryPartyKey = v_oClaimOpen.ClaimPeril(iPerilCount).SalvageRecovery(iRecovery).RecoveryPartyKey
                                            oClaimRecovery.RecoveryPartyTypeKey = v_oClaimOpen.ClaimPeril(iPerilCount).SalvageRecovery(iRecovery).RecoveryPartyTypeId
                                            oClaimRecovery.RecoveryPartyKeySpecified = v_oClaimOpen.ClaimPeril(iPerilCount).SalvageRecovery(iRecovery).RecoveryPartyKey <> 0
                                            oClaimRecovery.RecoveryPartyTypeKeySpecified = v_oClaimOpen.ClaimPeril(iPerilCount).SalvageRecovery(iRecovery).RecoveryPartyTypeId <> 0
                                            iRecoveries += 1
                                            oClaimPeril.Recovery.Add(oClaimRecovery)
                                            ' End If
                                        Next
                                        For iRecovery = 0 To v_oClaimOpen.ClaimPeril(iPerilCount).TPRecovery.Count - 1
                                            If v_oClaimOpen.ClaimPeril(iPerilCount).TPRecovery(iRecovery).CurrentRecovery <> 0 Or v_oClaimOpen.ClaimPeril(iPerilCount).TPRecovery(iRecovery).IsDeleted = True OrElse v_oClaimOpen.ClaimPeril(iPerilCount).TPRecovery(iRecovery).IsNew Then
                                                oClaimRecovery = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPerilRecoveryType()
                                                oClaimRecovery.BaseRecoveryKey = v_oClaimOpen.ClaimPeril(iPerilCount).TPRecovery(iRecovery).BaseRecoveryKey
                                                oClaimRecovery.Initialamount = v_oClaimOpen.ClaimPeril(iPerilCount).TPRecovery(iRecovery).InitialRecovery
                                                oClaimRecovery.IsDeletedRecovery = v_oClaimOpen.ClaimPeril(iPerilCount).TPRecovery(iRecovery).IsDeleted
                                                oClaimRecovery.RevisionAmount = v_oClaimOpen.ClaimPeril(iPerilCount).TPRecovery(iRecovery).RevisionAmount
                                                oClaimRecovery.TypeCode = v_oClaimOpen.ClaimPeril(iPerilCount).TPRecovery(iRecovery).TypeCode
                                                oClaimRecovery.RecoveryPartyKey = v_oClaimOpen.ClaimPeril(iPerilCount).TPRecovery(iRecovery).RecoveryPartyKey
                                                oClaimRecovery.RecoveryPartyTypeKey = v_oClaimOpen.ClaimPeril(iPerilCount).TPRecovery(iRecovery).RecoveryPartyTypeId
                                                oClaimRecovery.RecoveryPartyKeySpecified = v_oClaimOpen.ClaimPeril(iPerilCount).TPRecovery(iRecovery).RecoveryPartyKey <> 0
                                                oClaimRecovery.RecoveryPartyTypeKeySpecified = v_oClaimOpen.ClaimPeril(iPerilCount).TPRecovery(iRecovery).RecoveryPartyTypeId <> 0
                                                oClaimRecovery.IsNew = v_oClaimOpen.ClaimPeril(iPerilCount).TPRecovery(iRecovery).IsNew
                                                iRecoveries += 1
                                                oClaimPeril.Recovery.Add(oClaimRecovery)
                                            End If
                                        Next

                                    End If

                                    'Updating Claim reserve
                                    For iReserveCount = 0 To v_oClaimOpen.ClaimPeril(iPerilCount).Reserve.Count - 1
                                        If v_oClaimOpen.ClaimPeril(iPerilCount).Reserve(iReserveCount).ReserveEdited = True Then
                                            iTotalReserveCount += 1
                                        End If
                                    Next
                                    If iTotalReserveCount > 0 Then
                                        oClaimPeril.Reserve = New List(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPerilReserveType)
                                        iReserveCount = 0
                                        For iReserve = 0 To v_oClaimOpen.ClaimPeril(iPerilCount).Reserve.Count - 1
                                            If v_oClaimOpen.ClaimPeril(iPerilCount).Reserve(iReserve).ReserveEdited = True Then
                                                oClaimReserve = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPerilReserveType()
                                                'Incase of Open Claim this should always be Initial Reserve
                                                oClaimReserve.RevisionAmount = v_oClaimOpen.ClaimPeril(iPerilCount).Reserve(iReserve).CurrentReserve 'Lalit 
                                                'oClaimReserve.RevisionAmount = v_oClaimOpen.ClaimPeril(iPerilCount).Reserve(iReserve).RevisionAmount
                                                oClaimReserve.TypeCode = v_oClaimOpen.ClaimPeril(iPerilCount).Reserve(iReserve).TypeCode
                                                oClaimReserve.GrossReserve = v_oClaimOpen.ClaimPeril(iPerilCount).Reserve(iReserve).GrossReserve
                                                oClaimReserve.Tax = v_oClaimOpen.ClaimPeril(iPerilCount).Reserve(iReserve).Tax
                                                oClaimReserve.RevisedGrossReserve = v_oClaimOpen.ClaimPeril(iPerilCount).Reserve(iReserve).RevisedGrossReserve
                                                oClaimReserve.RevisedTaxReserve = v_oClaimOpen.ClaimPeril(iPerilCount).Reserve(iReserve).RevisedTaxReserve

                                                iReserveCount += 1
                                                oClaimPeril.Reserve.Add(oClaimReserve)
                                            End If
                                        Next

                                    End If
                                    oClaimPeril.TypeCode = v_oClaimOpen.ClaimPeril(iPerilCount).TypeCode
                                    iPeril += 1
                                    .ClaimPeril.Add(oClaimPeril)
                                End If
                            Next

                        End If
                    End If
                    'Claim Payment
                    If v_oClaimPayment IsNot Nothing Then

                        .ClaimKey = v_oClaimPayment.ClaimKey
                        .ClaimPayment = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPaymentType()
                        .ClaimPayment.BaseClaimKey = v_oClaimPayment.BaseClaimKey
                        .ClaimPayment.BaseClaimPerilKey = v_oClaimPayment.BaseClaimPerilKey
                        .ClaimPayment.ClaimVersionDescription = v_oClaimPayment.ClaimVersionDescription
                        .ClaimPayment.CloseClaimOnZeroReserveRecoveryBalance = v_oClaimPayment.CloseClaimOnZeroReserveRecoveryBalance
                        .ClaimPayment.CurrencyCode = v_oClaimPayment.CurrencyCode
                        If v_oClaimPayment.PaymentPartyType <> ClaimPaymentPartyTypeType.PARTY Then
                            .ClaimPayment.PartyKey = 0
                        Else
                            .ClaimPayment.PartyKey = v_oClaimPayment.PartyKey
                        End If
                        .ClaimPayment.ClientKey = v_oClaimPayment.ClientKey
                        .ClaimPayment.PaymentPartyType = v_oClaimPayment.PaymentPartyType
                        .ClaimPayment.UltimatePayee = v_oClaimPayment.UltimatePayee
                        .ClaimPayment.IsExGratia = v_oClaimPayment.IsExGratia
                        .ClaimPayment.OurRef = v_oClaimPayment.OurRef
                        .ApiTimeStamp = v_bTimeStamp
                        .ClaimPayment.ExgRateReasonId = v_oClaimPayment.ExgRateReasonId
                        .ClaimPayment.SystemToBaseDate = v_oClaimPayment.SystemToBaseDate
                        .ClaimPayment.SystemToBaseXRate = v_oClaimPayment.SystemToBaseXRate
                        .ClaimPayment.CurrencyToBaseDate = v_oClaimPayment.CurrencyToBaseDate
                        .ClaimPayment.CurrencyToBaseXRate = v_oClaimPayment.CurrencyToBaseXRate
                        .ClaimPayment.AccountToBaseDate = v_oClaimPayment.AccountToBaseDate
                        .ClaimPayment.AccountToBaseXRate = v_oClaimPayment.AccountToBaseXRate
                        Dim j As Integer = 0
                        Dim oClaimPaymentTaxItems(v_oClaimPayment.ClaimPaymentTaxItems.Count - 1) As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPaymentTaxItemType
                        For index As Integer = 0 To v_oClaimPayment.ClaimPaymentTaxItems.Count - 1
                            'If v_oClaimPayment.ClaimPaymentItem(index).PaymentAmount <> 0.0 Then
                            oClaimPaymentTaxItems(j) = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPaymentTaxItemType()
                            oClaimPaymentTaxItems(j).Amount = v_oClaimPayment.ClaimPaymentTaxItems(index).Amount
                            oClaimPaymentTaxItems(j).Percentage = v_oClaimPayment.ClaimPaymentTaxItems(index).Percentage
                            oClaimPaymentTaxItems(j).ReserveType = v_oClaimPayment.ClaimPaymentTaxItems(index).ReserveType
                            oClaimPaymentTaxItems(j).TaxBandCode = v_oClaimPayment.ClaimPaymentTaxItems(index).TaxBandCode
                            oClaimPaymentTaxItems(j).TaxGroupCode = v_oClaimPayment.ClaimPaymentTaxItems(index).TaxGroupCode
                            oClaimPaymentTaxItems(j).ClassOfBusinessID = v_oClaimPayment.ClaimPaymentTaxItems(index).ClassOfBusinessID
                            oClaimPaymentTaxItems(j).IsManuallyChanges = v_oClaimPayment.ClaimPaymentTaxItems(index).IsManuallyChanges
                            oClaimPaymentTaxItems(j).Sequence = v_oClaimPayment.ClaimPaymentTaxItems(index).Sequence
                            oClaimPaymentTaxItems(j).TaxBandId = v_oClaimPayment.ClaimPaymentTaxItems(index).TaxBandId
                            oClaimPaymentTaxItems(j).TaxGroupId = v_oClaimPayment.ClaimPaymentTaxItems(index).TaxGroupId
                            j += 1
                            'End If
                        Next

                        'New List(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPaymentItemType)
                        .ClaimPayment.ClaimPaymentTaxItems = oClaimPaymentTaxItems.ToList

                        If v_oClaimPayment.PaymentAdvancedTaxDetails.InsuranceTaxNumber <> String.Empty OrElse v_oClaimPayment.PaymentAdvancedTaxDetails.PayeeTaxNumber <> String.Empty Then
                            .ClaimPayment.AdvancedTaxDetails = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPaymentAdvancedTaxDetailsType()
                            .ClaimPayment.AdvancedTaxDetails.InsuranceTaxNumber = v_oClaimPayment.PaymentAdvancedTaxDetails.InsuranceTaxNumber
                            .ClaimPayment.AdvancedTaxDetails.InsuredDomiciled = v_oClaimPayment.PaymentAdvancedTaxDetails.InsuredDomiciled
                            .ClaimPayment.AdvancedTaxDetails.InsuredPercentage = v_oClaimPayment.PaymentAdvancedTaxDetails.InsuredPercentage
                            .ClaimPayment.AdvancedTaxDetails.IsSettlement = v_oClaimPayment.PaymentAdvancedTaxDetails.IsSettlement
                            .ClaimPayment.AdvancedTaxDetails.IsTaxExempt = v_oClaimPayment.PaymentAdvancedTaxDetails.IsTaxExempt
                            .ClaimPayment.AdvancedTaxDetails.IsWHTExempt = v_oClaimPayment.PaymentAdvancedTaxDetails.IsWHTExempt
                            .ClaimPayment.AdvancedTaxDetails.PayeeDomiciled = v_oClaimPayment.PaymentAdvancedTaxDetails.PayeeDomiciled
                            .ClaimPayment.AdvancedTaxDetails.PayeePercentage = v_oClaimPayment.PaymentAdvancedTaxDetails.PayeePercentage
                            .ClaimPayment.AdvancedTaxDetails.PayeeTaxNumber = v_oClaimPayment.PaymentAdvancedTaxDetails.PayeeTaxNumber
                            .ClaimPayment.AdvancedTaxDetails.SafeHarbourCode = v_oClaimPayment.PaymentAdvancedTaxDetails.SafeHarbourCode
                            .ClaimPayment.AdvancedTaxDetails.SafeHarbourPercentage = v_oClaimPayment.PaymentAdvancedTaxDetails.SafeHarbourPercentage
                            .ClaimPayment.AdvancedTaxDetails.PayeeName = v_oClaimPayment.PaymentAdvancedTaxDetails.PayeeName
                            .ClaimPayment.AdvancedTaxDetails.PaymentTo = v_oClaimPayment.PaymentAdvancedTaxDetails.PaymentTo
                        End If

                        If v_oClaimPayment.CashList.BankAccountCode <> String.Empty Then
                            .ClaimPayment.CashList = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BasePaymentCashListType()
                            .ClaimPayment.CashList.BankAccountCode = v_oClaimPayment.CashList.BankAccountCode
                            .ClaimPayment.CashList.CurrencyCode = v_oClaimPayment.CashList.CurrencyCode
                            .ClaimPayment.CashList.ListDate = v_oClaimPayment.CashList.ListDate
                            .ClaimPayment.CashList.Reference = v_oClaimPayment.CashList.Reference
                            .ClaimPayment.CashList.StatusCode = v_oClaimPayment.CashList.StatusCode
                            .ClaimPayment.CashList.TypeCode = v_oClaimPayment.CashList.TypeCode
                            .ClaimPayment.CashList.BranchCode = sBranchCode
                            .ClaimPayment.CashList.PaymentItem = New List(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BasePaymentCashListItemType)
                            '
                            For index As Integer = 0 To v_oClaimPayment.CashList.PaymentCashListItemType.Count - 1
                                With v_oClaimPayment.CashList.PaymentCashListItemType(index)
                                    oPaymentItem = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BasePaymentCashListItemType()
                                    oPaymentItem.AccountShortCode = .AccountShortCode
                                    oPaymentItem.AllocationStatusCode = .AllocationStatusCode
                                    oPaymentItem.Amount = .Amount

                                    If .BankPaymentType.AccountCode.Trim.Length <> 0 Then
                                        oPaymentItem.Bank = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseBankPaymentType
                                        oPaymentItem.Bank.AccountCode = .BankPaymentType.AccountCode
                                        oPaymentItem.Bank.BranchCode = .BankPaymentType.BranchCode
                                        oPaymentItem.Bank.ExpiryDate = .BankPaymentType.ExpiryDate
                                        If .BankPaymentType.ExpiryDate <> DateTime.MinValue Then
                                            oPaymentItem.Bank.ExpiryDateSpecified = True
                                        Else
                                            oPaymentItem.Bank.ExpiryDateSpecified = False
                                        End If
                                        oPaymentItem.Bank.PayeeName = .BankPaymentType.PayeeName
                                        oPaymentItem.Bank.Reference1 = .BankPaymentType.Reference1
                                        oPaymentItem.Bank.Reference2 = .BankPaymentType.Reference2
                                        oPaymentItem.BankReference = .BankReference
                                        oPaymentItem.Bank.BIC = .BankPaymentType.BIC
                                        oPaymentItem.Bank.IBAN = .BankPaymentType.IBAN
                                    End If

                                    If Trim(.ContactAddress.Address1) <> String.Empty Then
                                        oPaymentItem.ContactAddress = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseSimpleAddressType
                                        oPaymentItem.ContactAddress.AddressLine1 = .ContactAddress.Address1
                                        oPaymentItem.ContactAddress.AddressLine2 = .ContactAddress.Address2
                                        oPaymentItem.ContactAddress.AddressLine3 = .ContactAddress.Address3
                                        oPaymentItem.ContactAddress.AddressLine4 = .ContactAddress.Address4
                                        oPaymentItem.ContactAddress.CountryCode = .ContactAddress.CountryCode
                                        oPaymentItem.ContactAddress.PostCode = .ContactAddress.PostCode
                                    End If
                                    oPaymentItem.ContactName = .ContactName
                                    oPaymentItem.FurtherDetails = .FurtherDetails
                                    oPaymentItem.MediaReference = .MediaReference
                                    oPaymentItem.MediaTypeCode = .MediaTypeCode
                                    oPaymentItem.OurReference = .OurReference
                                    oPaymentItem.StatusCode = .StatusCode
                                    oPaymentItem.TheirReference = .TheirReference
                                    oPaymentItem.TypeCode = .TypeCode
                                    oPaymentItem.TransactionDate = .TransactionDate
                                End With
                                .ClaimPayment.CashList.PaymentItem.Add(oPaymentItem)
                            Next
                        End If

                        .ClaimPayment.ClaimPaymentItem = New List(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPaymentItemType)
                        For index As Integer = 0 To v_oClaimPayment.ClaimPaymentItem.Count - 1
                            'If v_oClaimPayment.ClaimPaymentItem(index).PaymentAmount <> 0.0 Then
                            oClaimPaymentItemType = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPaymentItemType()
                            oClaimPaymentItemType.PaymentAmount = v_oClaimPayment.ClaimPaymentItem(index).PaymentAmount
                            oClaimPaymentItemType.BaseReserveKey = v_oClaimPayment.ClaimPaymentItem(index).BaseReserveKey
                            oClaimPaymentItemType.ReverseExcess = v_oClaimPayment.ClaimPaymentItem(index).ReverseExcess
                            oClaimPaymentItemType.TaxGroupCode = v_oClaimPayment.ClaimPaymentItem(index).TaxGroupCode
                            oClaimPaymentItemType.IsTaxOverridden = v_oClaimPayment.ClaimPaymentItem(index).IsTaxOverridden
                            oClaimPaymentItemType.OverriddedTaxAmount = v_oClaimPayment.ClaimPaymentItem(index).OverriddedTaxAmount
                            i += 1
                            'End If
                            .ClaimPayment.ClaimPaymentItem.Add(oClaimPaymentItemType)
                        Next
                        .ClaimPayment.Payee = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseClaimPayeeType()
                        If Trim(v_oClaimPayment.Payee.Address.Address1) <> String.Empty Then
                            .ClaimPayment.Payee.Address = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseAddressType()
                            .ClaimPayment.Payee.Address.AddressLine1 = v_oClaimPayment.Payee.Address.Address1
                            .ClaimPayment.Payee.Address.AddressLine2 = v_oClaimPayment.Payee.Address.Address2
                            .ClaimPayment.Payee.Address.AddressLine3 = v_oClaimPayment.Payee.Address.Address3
                            .ClaimPayment.Payee.Address.AddressLine4 = v_oClaimPayment.Payee.Address.Address4
                            .ClaimPayment.Payee.Address.AddressTypeCode = v_oClaimPayment.Payee.Address.AddressType
                            .ClaimPayment.Payee.Address.CountryCode = v_oClaimPayment.Payee.Address.CountryCode
                            If v_oClaimPayment.Payee.Address.PostCode IsNot Nothing Then
                                .ClaimPayment.Payee.Address.PostCode = v_oClaimPayment.Payee.Address.PostCode
                            End If
                        End If
                        .ClaimPayment.Payee.BankCode = v_oClaimPayment.Payee.BankCode
                        .ClaimPayment.Payee.BankName = v_oClaimPayment.Payee.BankName
                        .ClaimPayment.Payee.BankNumber = v_oClaimPayment.Payee.BankNumber
                        .ClaimPayment.Payee.Comments = v_oClaimPayment.Payee.Comments
                        .ClaimPayment.Payee.MediaReference = v_oClaimPayment.Payee.MediaReference
                        .ClaimPayment.Payee.MediaTypeCode = v_oClaimPayment.Payee.MediaTypeCode
                        .ClaimPayment.Payee.Name = v_oClaimPayment.Payee.Name
                        .ClaimPayment.Payee.TheirReference = v_oClaimPayment.Payee.TheirReference
                        .ClaimPayment.Payee.PartyBankKey = v_oClaimPayment.Payee.PartyBankKey
                        .ClaimPayment.Payee.BIC = v_oClaimPayment.Payee.BIC
                        .ClaimPayment.Payee.IBAN = v_oClaimPayment.Payee.IBAN
                        .ClaimPayment.Payee.AccountType = v_oClaimPayment.Payee.AccountType
                    End If

                    .ApiTimeStamp = v_bTimeStamp
                    '1 fro Open Claim
                    '2 fro Maintain Claim
                    .ProcessType = v_iProcessType

                End With

                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    'oUpdateClaimReservesOrPaymentsResponse = oSAM.UpdateClaimReservesOrPayments(oUpdateClaimReservesOrPaymentsRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Put(ApiMethods.UpdateClaimReservesOrPayments, oUpdateClaimReservesOrPaymentsRequest)
                    oUpdateClaimReservesOrPaymentsResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateClaimReservesOrPaymentsCommandResponse)(result)

                End Using

                With oUpdateClaimReservesOrPaymentsResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        oClaimResponse.ResultingStatus = .ResultingStatus
                        oClaimResponse.TimeStamp = .ApiTimeStamp
                    End If
                End With
                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                'oSAM.Close()
                oUpdateClaimReservesOrPaymentsRequest = Nothing
                oUpdateClaimReservesOrPaymentsResponse = Nothing
            End Try
            Return oClaimResponse
        End SyncLock
    End Function
    ''' <summary>
    ''' This Method Update the Added/Changed RI Line into Database for Claim
    ''' </summary>
    ''' <param name="v_oRIArrangementLines"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub UpdateClaimRIArrangementLinesRI2007(ByVal v_oRIArrangementLines As ArrangementLinesTypeCollection,
                                  Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock

            Dim oUpdateClaimRIArrangementLinesRI2007Request As BaseClasses.UpdateClaimRIArrangementLinesRI2007Command
            Dim oUpdateClaimRIArrangementLinesRI2007Response As BaseClasses.UpdateClaimRIArrangementLinesRI2007CommandResponse
            Dim sbLogMessage As StringBuilder

            Try
                sbLogMessage = New StringBuilder
                oUpdateClaimRIArrangementLinesRI2007Request = New BaseClasses.UpdateClaimRIArrangementLinesRI2007Command
                oUpdateClaimRIArrangementLinesRI2007Response = New BaseClasses.UpdateClaimRIArrangementLinesRI2007CommandResponse

                With oUpdateClaimRIArrangementLinesRI2007Request
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

                    If v_oRIArrangementLines.Count > 0 Then
                        Dim oRIArrangementLine(v_oRIArrangementLines.Count - 1) As BaseClasses.BaseClaimRiskRIArrangementLineType
                        .ClaimRIArrangementLines = oRIArrangementLine.ToList().ConvertAll(New Converter(Of BaseClasses.BaseClaimRiskRIArrangementLineType, BaseClasses.BaseClaimRiskRIArrangementLineType)(AddressOf ToBaseClaimRiskRIArrangementLineTypeList))
                    End If
                End With

                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Put(ApiMethods.UpdateClaimRIArrangementLinesRI2007, oUpdateClaimRIArrangementLinesRI2007Request)
                    oUpdateClaimRIArrangementLinesRI2007Response = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of BaseClasses.UpdateClaimRIArrangementLinesRI2007CommandResponse)(result)
                End Using

                With oUpdateClaimRIArrangementLinesRI2007Response

                    If .Errors IsNot Nothing Then

                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If
                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("UpdateClaimRIArrangementLinesRI2007 executed ok" & vbCrLf)
                    LogMessageEntry(sbLogMessage)
                End If

                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oUpdateClaimRIArrangementLinesRI2007Request = Nothing
                oUpdateClaimRIArrangementLinesRI2007Response = Nothing
            End Try

        End SyncLock

    End Sub

    Public Overrides Function UpdateClaimRisk(ByVal v_oClaimRisk As ClaimRisk,
                                     Optional ByVal v_sBranchCode As String = Nothing) As Byte()
        SyncLock oLock

            'Dim oSAM As PureClaimServiceClient
            Dim oUpdateClaimRiskRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateClaimRiskRequestType
            Dim oUpdateClaimRiskResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateClaimRiskResponseType
            Dim sbLogMessage As StringBuilder

            Try
                'oSAM = InitializeClaimServiceMethod()
                oUpdateClaimRiskRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateClaimRiskRequestType
                oUpdateClaimRiskResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateClaimRiskResponseType
                sbLogMessage = New StringBuilder

                With oUpdateClaimRiskRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    '.WCFSecurityToken = SecurityToken()
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
                        .BaseClaimKey = v_oClaimRisk.ClaimKey
                        .TimeStamp = v_oClaimRisk.TimeStamp
                        .XMLDataSet = v_oClaimRisk.XMLDataSet

                    End If

                    Using trace As New Tracer(Category.Trace)
                        'oUpdateClaimRiskResponse = oSAM.UpdateClaimRisk(oUpdateClaimRiskRequest)
                        SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                        Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.UpdateClaimRisk, oUpdateClaimRiskRequest)
                        oUpdateClaimRiskResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.UpdateClaimRiskResponseType)(result)
                    End Using

                    'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                End With
                With oUpdateClaimRiskResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else
                        v_oClaimRisk.TimeStamp = .TimeStamp

                    End If

                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("UpdateClaimRisk executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_oClaimRisk = " & v_oClaimRisk.Print.Replace("<br/>", vbCrLf))

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
                'oSAM.Close()
                oUpdateClaimRiskRequest = Nothing
                oUpdateClaimRiskResponse = Nothing
            End Try
            'Returns Timestamp
            Return v_oClaimRisk.TimeStamp

        End SyncLock
    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_oBaseCaseKey"></param>
    ''' <param name="v_oCaseKey"></param>
    ''' <param name="v_oEventDescription"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub CloseCase(Optional ByVal v_oBaseCaseKey As Integer = 0,
                                       Optional ByVal v_oCaseKey As Integer = 0,
                                       Optional ByVal v_oEventDescription As String = Nothing,
                                       Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock

            Dim oCloseCaseRequest As BaseClasses.CloseCaseCommand = Nothing  ' Request Type
            Dim oCloseCaseResponse As BaseClasses.CloseCaseCommandResponse = Nothing  ' Response Type
            Dim sbLogMessage As StringBuilder = Nothing
            Try
                oCloseCaseRequest = New BaseClasses.CloseCaseCommand
                oCloseCaseResponse = New BaseClasses.CloseCaseCommandResponse
                sbLogMessage = New StringBuilder
                With oCloseCaseRequest
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

                    .BaseCaseKey = v_oBaseCaseKey
                    .CaseKey = v_oCaseKey
                    .EventDescription = v_oEventDescription
                End With

                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.CloseCase, oCloseCaseRequest)
                    oCloseCaseResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of BaseClasses.CloseCaseCommandResponse)(result)
                End Using

                With oCloseCaseResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    Else

                    End If
                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage = New StringBuilder
                    sbLogMessage.AppendLine("CloseCase executed ok" & vbCrLf)

                    LogMessageEntry(sbLogMessage)
                End If
            Catch ex As Exception
                Throw
            Finally
                oCloseCaseRequest = Nothing
                oCloseCaseResponse = Nothing
                sbLogMessage = Nothing
            End Try

        End SyncLock
    End Sub


    ''' <summary>
    ''' ToBaseClaimRiskRIArrangementLineTypeList
    ''' </summary>
    ''' <param name="oImpList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ToBaseClaimRiskRIArrangementLineTypeList(ByVal oImpList As BaseClasses.BaseClaimRiskRIArrangementLineType) As BaseClasses.BaseClaimRiskRIArrangementLineType
        Dim oservice As New BaseClasses.BaseClaimRiskRIArrangementLineType
        If oImpList IsNot Nothing Then
            With oImpList
                oservice.ActionType = .ActionType
                oservice.AgreementCode = .AgreementCode

                If .Balance > 0 Then
                    oservice.Balance = .Balance
                    oservice.BalanceSpecified = True
                Else
                    oservice.BalanceSpecified = False
                End If

                oservice.CedePremiumOnly = .CedePremiumOnly
                oservice.DefaultSharePercent = .DefaultSharePercent

                If .Grouping > 0 Then
                    oservice.Grouping = .Grouping
                    oservice.GroupingSpecified = True
                Else
                    oservice.GroupingSpecified = False
                End If

                oservice.IsDomiciledForTax = .IsDomiciledForTax
                oservice.IsDomiciledForTaxSpecified = .IsDomiciledForTax
                oservice.IsRIBroker = .IsRIBroker
                oservice.IsRIBrokerSpecified = .IsRIBroker
                oservice.LineLimit = .LineLimit
                If .LowerLimit > 0 Then
                    oservice.LowerLimit = .LowerLimit
                    oservice.LowerLimitSpecified = True
                Else
                    oservice.LowerLimitSpecified = False
                End If

                oservice.NumberOfLines = .NumberOfLines

                If .PartyKey > 0 Then
                    oservice.PartyKey = .PartyKey
                    oservice.PartyKeySpecified = True
                Else
                    oservice.PartyKeySpecified = False
                End If

                oservice.PaymentToDate = .PaymentToDate
                oservice.Priority = .Priority

                If .RecoverToDate > 0 Then
                    oservice.RecoverToDate = .RecoverToDate
                    oservice.RecoverToDateSpecified = True
                Else
                    oservice.RecoverToDateSpecified = False
                End If

                oservice.ReinsuranceTypeCode = .ReinsuranceTypeCode
                oservice.ReserveToDate = .ReserveToDate

                If .Retained > 0 Then
                    oservice.Retained = .Retained
                    oservice.RetainedSpecified = True
                Else
                    oservice.RetainedSpecified = False
                End If

                oservice.RIArrangementKey = .RIArrangementKey
                oservice.RIArrangementLineKey = .RIArrangementLineKey
                oservice.RIName = .RIName
                oservice.RIPlacement = .RIPlacement
                oservice.SumInsured = .SumInsured
                oservice.ThisPayment = .ThisPayment
                oservice.ThisReserve = .ThisReserve
                oservice.ThisSharePercent = .ThisSharePercent
                oservice.TreatyCode = .TreatyCode
                oservice.Type = .Type
                If .BrokerParticipants.Count > 0 Then
                    'oservice.BrokerParticipants = .BrokerParticipants.ToList().ConvertAll(New Converter(Of BaseBrokerParticipants, BaseBrokerParticipants)(AddressOf ToBrokerParticipantList))
                    oservice.BrokerParticipants = .BrokerParticipants
                    oservice.BrokerParticipants = .BrokerParticipants.ToList().ConvertAll(New Converter(Of BaseClasses.BaseBrokerParticipants, BaseClasses.BaseBrokerParticipants)(AddressOf ToBrokerParticipantList))
                End If

                If .FAXParticipants IsNot Nothing AndAlso .FAXParticipants.Count > 0 Then
                    oservice.FAXParticipants = .FAXParticipants.ToList().ConvertAll(New Converter(Of BaseClasses.BaseClaimFaxParticipants, BaseClasses.BaseClaimFaxParticipants)(AddressOf ToClaimFAXParticipantList))
                End If
            End With
        End If
        Return oservice
    End Function
    ''' <summary>
    ''' ToClaimFAXParticipantList
    ''' </summary>
    ''' <param name="oImpList"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ToClaimFAXParticipantList(ByVal oImpList As BaseClasses.BaseClaimFaxParticipants) As BaseClasses.BaseClaimFaxParticipants
        Dim oservice As New BaseClasses.BaseClaimFaxParticipants
        If oImpList IsNot Nothing Then
            With oImpList
                oservice.AccountType = .AccountType
                oservice.AgreementCode = .AgreementCode
                oservice.ParticpationPercentage = .ParticpationPercentage
                oservice.PartyCode = .PartyCode
                oservice.PartyKey = .PartyKey
                oservice.PartyName = .PartyName
                oservice.RIArrangementLineKey = .RIArrangementLineKey
                oservice.SumInsured = .SumInsured
                oservice.ActionType = .ActionType
                oservice.PaymentToDate = .PaymentToDate

                If .RecoverToDate > 0 Then
                    oservice.RecoverToDate = .RecoverToDate
                    oservice.RecoverToDateSpecified = True
                Else
                    oservice.RecoverToDateSpecified = False
                End If

                oservice.ReserveToDate = .ReserveToDate
                oservice.ThisPayment = .ThisPayment
                oservice.ThisReserve = .ThisReserve

                If .BrokerParticipants.Count > 0 Then
                    'oservice.BrokerParticipants = .BrokerParticipants.ToList().ConvertAll(New Converter(Of BaseBrokerParticipants, BaseBrokerParticipants)(AddressOf ToBrokerParticipantList))
                    oservice.BrokerParticipants = .BrokerParticipants
                    oservice.BrokerParticipants = .BrokerParticipants.ToList().ConvertAll(New Converter(Of BaseClasses.BaseBrokerParticipants, BaseClasses.BaseBrokerParticipants)(AddressOf ToBrokerParticipantList))
                End If

            End With
        End If

        Return oservice
    End Function
    ''' <summary>
    ''' To get default bank account details for given params
    ''' </summary>
    ''' <param name="v_sProductCode"></param>
    ''' <param name="v_nMediaTypeId"></param>
    ''' <param name="v_nCashListTypeId"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetDefaultBankAccountWithCurrency(ByVal v_sProductCode As String, ByVal v_nMediaTypeId As Integer,
                                     ByVal v_nCashListTypeId As Integer, Optional ByVal v_sBranchCode As String = Nothing) As BankAccountDefaults

        SyncLock oLock
            'Dim oSAM As PureClaimServiceClient = Nothing
            Dim oGetDefaultBankAccountWithCurrencyRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDefaultBankAccountWithCurrencyQuery
            Dim oGetDefaultBankAccountWithCurrencyResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDefaultBankAccountWithCurrencyQueryResponse
            Dim oBankAccountDefaultCollection As BankAccountDefaults = Nothing
            Dim oBankAccountDefault As BankAccountDefault
            Dim sLogMessage As StringBuilder

            Try
                'oSAM = InitializeClaimServiceMethod()
                oGetDefaultBankAccountWithCurrencyRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDefaultBankAccountWithCurrencyQuery
                oBankAccountDefaultCollection = New BankAccountDefaults
                sLogMessage = New StringBuilder


                With oGetDefaultBankAccountWithCurrencyRequest
                    .LoginUserName = Current.Session(CNLoginName)
                    '.WCFSecurityToken = SecurityToken()
                    .ProductCode = v_sProductCode
                    .MediaTypeID = v_nMediaTypeId
                    .CashListTypeID = v_nCashListTypeId

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

                Using New Tracer(Category.Trace)
                    'oGetDefaultBankAccountWithCurrencyResponse = oSAM.GetDefaultBankAccountWithCurrency(oGetDefaultBankAccountWithCurrencyRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetDefaultBankAccountWithCurrency, oGetDefaultBankAccountWithCurrencyRequest)
                    oGetDefaultBankAccountWithCurrencyResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetDefaultBankAccountWithCurrencyQueryResponse)(result)

                End Using

                With oGetDefaultBankAccountWithCurrencyResponse.GetDefaultBankAccountWithCurrencyResponse

                    If .Results IsNot Nothing Then
                        For Each oBankAccountDeafultTypeRow As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetDefaultBankAccountWithCurrencyResponseTypeResultsRow In .Results
                            oBankAccountDefault = New BankAccountDefault
                            With oBankAccountDefault
                                .BankAccountCode = oBankAccountDeafultTypeRow.BankAccountCode
                                .BankAccountDefaultID = oBankAccountDeafultTypeRow.BankAccountDefaultID
                                .BankAccountID = oBankAccountDeafultTypeRow.BankAccountID
                                .CashListTypeCode = oBankAccountDeafultTypeRow.CashListTypeCode
                                .CashListTypeID = oBankAccountDeafultTypeRow.CashListTypeID
                                .Code = oBankAccountDeafultTypeRow.Code
                                .CurrencyCode = oBankAccountDeafultTypeRow.CurrencyCode
                                .CurrencyID = oBankAccountDeafultTypeRow.CurrencyID
                                .Description = oBankAccountDeafultTypeRow.Description
                                .EffectiveDate = oBankAccountDeafultTypeRow.EffectiveDate
                                .MediaTypeCode = oBankAccountDeafultTypeRow.MediaTypeCode
                                .MediaTypeID = oBankAccountDeafultTypeRow.MediaTypeID
                                .ProductCode = oBankAccountDeafultTypeRow.ProductCode
                                .ProductID = oBankAccountDeafultTypeRow.ProductID
                                .SourceID = oBankAccountDeafultTypeRow.SourceID
                            End With
                            oBankAccountDefaultCollection.Add(oBankAccountDefault)
                        Next
                    End If

                End With
                If Logger.IsLoggingEnabled Then
                    sLogMessage.AppendLine("GetDefaultBankAccountWithCurrency executed ok" & vbCrLf)
                    sLogMessage.AppendLine("Input :" & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sLogMessage)
                End If
                'Catch ex As FaultException(Of SAMMethodResponseData)
                'FaultErrorHandler(ex) ' handling fault error messages 
            Catch ex As Exception
                Throw
            Finally
                'oSAM.Close()
                oGetDefaultBankAccountWithCurrencyRequest = Nothing
                oGetDefaultBankAccountWithCurrencyResponse = Nothing
            End Try

            Return oBankAccountDefaultCollection

        End SyncLock
    End Function
    ''' <summary>
    ''' To generate cash list
    ''' </summary>
    ''' <param name="v_nClaimId"></param>
    ''' <remarks></remarks>
    Public Overrides Sub GenerateCashList(ByVal v_nClaimId As Integer, Optional ByVal v_sBranchCode As String = Nothing)
        SyncLock oLock
            Dim oRequest As BaseClasses.GenerateCashListCommand
            Dim oResponse As BaseClasses.GenerateCashListCommandResponse
            Dim sLogMessage As StringBuilder

            Try
                oRequest = New BaseClasses.GenerateCashListCommand
                sLogMessage = New StringBuilder

                With oRequest
                    .LoginUserName = Current.Session(CNLoginName)
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
                    .ClaimId = v_nClaimId
                End With

                Using New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.GenerateCashList, oRequest)
                    oResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of BaseClasses.GenerateCashListCommandResponse)(result)
                End Using

                With oResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If
                End With
                If Logger.IsLoggingEnabled Then
                    sLogMessage.AppendLine("GenerateCashList executed ok" & vbCrLf)
                    sLogMessage.AppendLine("Input :" & vbCrLf)
                    sLogMessage.AppendLine("v_iClaimId = " & v_nClaimId.ToString & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    LogMessageEntry(sLogMessage)
                End If
                'Catch ex As FaultException(Of SAMMethodResponseData)
                'FaultErrorHandler(ex) ' handling fault error messages 
            Catch ex As Exception
                Throw
            Finally
                oRequest = Nothing
                oResponse = Nothing
            End Try
        End SyncLock

    End Sub

    ''' <summary>
    ''' Method To Update the recommended status
    ''' </summary>
    ''' <param name="v_nClaimKey"></param>
    ''' <param name="tsTimeStamp"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <remarks></remarks>
    Public Overrides Sub UpdateRecommendStatus(ByVal v_nClaimKey As Integer,
                                                  ByRef tsTimeStamp() As Byte,
                                                  Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock

            Dim oUpdateRecommendStatusRequestType As New BaseClasses.UpdateRecommendStatusCommand
            Dim oUpdateRecommendStatusResponseType As New BaseClasses.UpdateRecommendStatusCommandResponse
            Dim sLogMessage As StringBuilder

            Try
                sLogMessage = New StringBuilder

                oUpdateRecommendStatusRequestType = New BaseClasses.UpdateRecommendStatusCommand
                oUpdateRecommendStatusResponseType = New BaseClasses.UpdateRecommendStatusCommandResponse


                With oUpdateRecommendStatusRequestType
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

                    .ClaimKey = v_nClaimKey
                    .Status = 0
                    .LoginUserName = Current.Session(CNLoginName)
                    .ApiTimeStamp = tsTimeStamp
                End With

                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Put(ApiMethods.UpdateRecommendStatus, oUpdateRecommendStatusRequestType)
                    oUpdateRecommendStatusResponseType = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of BaseClasses.UpdateRecommendStatusCommandResponse)(result)
                End Using

                With oUpdateRecommendStatusResponseType
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If
                End With

                sLogMessage.AppendLine("UpdateRecommendStatus executed ok" & vbCrLf)
                sLogMessage.AppendLine("Input :" & vbCrLf)
                sLogMessage.AppendLine("v_nClaimKey = " & v_nClaimKey.ToString & vbCrLf)

                If Not IsNothing(v_sBranchCode) Then
                    sLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                Else
                    sLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                End If
                LogMessageEntry(sLogMessage)
                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                oUpdateRecommendStatusRequestType = Nothing
                oUpdateRecommendStatusResponseType = Nothing
            End Try

        End SyncLock
    End Sub

    ''' <summary>
    ''' To GetProductDocuments configured.
    ''' </summary>
    ''' <param name="v_sProductCode"></param>
    ''' <param name="v_iFunctionalArea"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetProductDocuments(ByVal v_sProductCode As String,
                                                  ByVal v_nFunctionalArea As Integer,
                                                        Optional ByVal v_sBranchCode As String = Nothing) As ProductDocumentsCollection

        SyncLock oLock
            'Dim oSAM As PureClaimServiceClient
            Dim oGetProductDocumentsRequest As New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetProductDocumentsQuery
            Dim oGetProductDocumentsResponse As New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetProductDocumentsQueryResponse 'Response Type

            Dim sbLogMessage As StringBuilder

            Try
                'oSAM = InitializeClaimServiceMethod()
                oGetProductDocumentsRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetProductDocumentsQuery
                oGetProductDocumentsResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetProductDocumentsQueryResponse

                sbLogMessage = New StringBuilder

                With oGetProductDocumentsRequest
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
                    .FunctionalArea = v_nFunctionalArea
                    .ProductCode = v_sProductCode.ToUpper()
                    .LoginUserName = Current.Session(CNLoginName)
                    '.WCFSecurityToken = SecurityToken()
                End With

                Using trace As New Tracer(Category.Trace)
                    'oGetProductDocumentsResponse = oSAM.GetProductDocuments(oGetProductDocumentsRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetProductDocuments, oGetProductDocumentsRequest)
                    oGetProductDocumentsResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetProductDocumentsQueryResponse)(result)
                End Using

                Dim oProductDocument As ProductDocuments
                Dim oProductDocumentCollection As New ProductDocumentsCollection
                With oGetProductDocumentsResponse

                    If oGetProductDocumentsResponse IsNot Nothing Then
                        For Each oGetProductDocumentsRow As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseGetProductDocumentsResponseTypeProductDocumentsRow In .GetProductDocumentsResponse.ProductDocuments
                            oProductDocument = New ProductDocuments
                            With oProductDocument
                                .DocumentTypeKey = oGetProductDocumentsRow.DocumentTypeKey
                                .DTDescription = oGetProductDocumentsRow.DTDescription
                                .FunctionalArea = oGetProductDocumentsRow.FunctionalArea
                                .GenerateThroughBO = oGetProductDocumentsRow.GenerateThroughBO
                                .GenerateThroughSAM = oGetProductDocumentsRow.GenerateThroughSAM
                                .GISSchemeKey = oGetProductDocumentsRow.GISSchemeKey
                                .IsAgent = oGetProductDocumentsRow.IsAgent
                                .IsClient = oGetProductDocumentsRow.IsClient
                                .IsOffice = oGetProductDocumentsRow.IsOffice
                                .PMBDocLinkKey = oGetProductDocumentsRow.PMBDocLinkKey
                                .ProcessTypeKey = oGetProductDocumentsRow.ProcessTypeKey
                                .ProcessTypesDocsKey = oGetProductDocumentsRow.ProcessTypesDocsKey
                                .ProductionOrder = oGetProductDocumentsRow.ProductionOrder
                                .ProductKey = oGetProductDocumentsRow.ProductKey
                                .PTDDescription = oGetProductDocumentsRow.PTDDescription
                                .PTDescription = oGetProductDocumentsRow.PTDescription
                                .Description = oGetProductDocumentsRow.SDescription
                                .SourceKey = oGetProductDocumentsRow.SourceKey
                                .SpoolDocument = oGetProductDocumentsRow.SpoolDocument
                                .DocumentTemplateCode = oGetProductDocumentsRow.DocumentTemplateCode
                            End With
                            oProductDocumentCollection.Add(oProductDocument)
                        Next
                    End If
                End With
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("GetProductDocuments executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)

                    If Not IsNothing(v_sBranchCode) Then
                        sbLogMessage.AppendLine("v_sBranchCode = " & v_sBranchCode.ToString & vbCrLf)
                    Else
                        sbLogMessage.AppendLine("v_sBranchCode = nothing" & vbCrLf)
                    End If

                    sbLogMessage.AppendLine("Output : " & vbCrLf)
                    sbLogMessage.AppendLine(oProductDocumentCollection.Print().Replace("<br />", vbCrLf))

                    LogMessageEntry(sbLogMessage)
                End If

                Return oProductDocumentCollection
                'Catch ex As FaultException(Of PureService.SAMMethodResponseData)
                'FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw
            Finally
                'oSAM.Close()
                oGetProductDocumentsRequest = Nothing
                oGetProductDocumentsResponse = Nothing
            End Try

        End SyncLock
    End Function

    Public Overrides Sub CheckReserveRecovery(ByVal nClaimKey As Integer, ByRef r_nClaimStatus As Integer, ByRef r_dCurrentReserve As Decimal, ByRef r_dCurrentRecovery As Decimal, Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock
            Dim oCheckReserveRecoveryRequest As BaseClasses.CheckReserveRecoveryQuery
            Dim oCheckReserveRecoveryResponse As BaseClasses.CheckReserveRecoveryQueryResponse

            Try
                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls

                oCheckReserveRecoveryRequest = New BaseClasses.CheckReserveRecoveryQuery
                oCheckReserveRecoveryResponse = New BaseClasses.CheckReserveRecoveryQueryResponse

                With oCheckReserveRecoveryRequest

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
                    .LoginUserName = Current.Session(CNLoginName)
                    .ClaimKey = nClaimKey
                End With

                Using trace As New Tracer(Category.Trace)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.CheckReserveRecovery, oCheckReserveRecoveryRequest)
                    oCheckReserveRecoveryResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of BaseClasses.CheckReserveRecoveryQueryResponse)(result)
                End Using

                With oCheckReserveRecoveryResponse.CheckReserveRecoveryResponse
                    'If .Errors IsNot Nothing Then
                    'Process the error object if errors, and throw as a single exception
                    'Throw New NexusException(.Errors)
                    'Else
                    If .ClaimStatus <> 0 Then
                        r_nClaimStatus = .ClaimStatus
                        r_dCurrentReserve = .CurrentReserve
                        r_dCurrentRecovery = .CurrentRecovery
                    End If
                    'End If
                End With
            Catch ex As FaultException(Of PureService.SAMMethodResponseData)

                FaultErrorHandler(ex) ' handling fault error messages 

            Catch ex As Exception
                Throw (ex)
            Finally
                oCheckReserveRecoveryRequest = Nothing
                oCheckReserveRecoveryResponse = Nothing
            End Try

        End SyncLock
    End Sub
    ''' <summary>
    ''' Delete The abandon claim based on the passed input
    ''' </summary>
    ''' <param name="nClaimId"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    Public Overrides Function DeletAbandonClaim(ByVal nClaimId As Integer,
                                          Optional ByVal v_sBranchCode As String = Nothing) As Boolean
        SyncLock oLock

            'Dim oSAM As PureClaimServiceClient 'SAMForInsuranceV2's Object
            Dim oDeleteAbandonClaimRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.DeleteAbandonClaimCommand 'Request Type
            Dim oDeleteAbandonClaimResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.DeleteAbandonClaimCommandResponse 'Response Type
            Dim sbLogMessage As StringBuilder

            Try
                'oSAM = InitializeClaimServiceMethod()
                oDeleteAbandonClaimRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.DeleteAbandonClaimCommand
                oDeleteAbandonClaimResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.DeleteAbandonClaimCommandResponse
                sbLogMessage = New StringBuilder

                With oDeleteAbandonClaimRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    '.WCFSecurityToken = SecurityToken()
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
                    If nClaimId > 0 Then
                        .ClaimKey = nClaimId
                    Else
                        Throw New ArgumentNullException("Claim key")
                    End If
                End With
                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    'oDeleteAbandonClaimResponse = oSAM.DeleteAbandonClaim(oDeleteAbandonClaimRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Delete(ApiMethods.DeletAbandonClaim, oDeleteAbandonClaimRequest)
                    oDeleteAbandonClaimResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.DeleteAbandonClaimCommandResponse)(result)

                End Using
                If oDeleteAbandonClaimResponse IsNot Nothing Then
                    DeletAbandonClaim = True
                    With oDeleteAbandonClaimResponse

                    End With
                End If
                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("DeletAbandonClaim executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("Claim key = " & nClaimId & vbCrLf)

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
                'oSAM.Close()
                oDeleteAbandonClaimRequest = Nothing
                oDeleteAbandonClaimResponse = Nothing
            End Try

            Return DeletAbandonClaim
        End SyncLock
    End Function
    ''' <summary>
    ''' To get unpaid premium for the selected insurance file.
    ''' </summary>
    ''' <param name="v_sInsuranceRef"></param>
    ''' <param name="v_sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function CheckUnpaidPremium(ByVal v_sInsuranceRef As String, Optional ByVal sClaimNumber As String = "",
                                                  Optional ByVal v_sBranchCode As String = Nothing) As CheckUnPaidDetails
        SyncLock oLock

            'Dim oSAM As New PureClaimServiceClient
            Dim oCheckUnpaidPremiumRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.CheckUnpaidPremiumQuery
            Dim oCheckUnpaidPremiumResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.CheckUnpaidPremiumQueryResponse
            Dim otransactionsCollection As TransactionCollection
            Dim oTransactions As Transaction
            Dim oCheckUnPaidDetails As New CheckUnPaidDetails
            Dim sbLogMessage As StringBuilder

            Try
                'oSAM = InitializeClaimServiceMethod()
                oCheckUnpaidPremiumRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.CheckUnpaidPremiumQuery
                oCheckUnpaidPremiumResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.CheckUnpaidPremiumQueryResponse

                otransactionsCollection = New TransactionCollection
                sbLogMessage = New StringBuilder

                With oCheckUnpaidPremiumRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    ' .WCFSecurityToken = SecurityToken()
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
                    .ClaimNumber = sClaimNumber
                End With


                Using trace As New Tracer(Category.Trace)
                    'oCheckUnpaidPremiumResponse = oSAM.CheckUnpaidPremium(oCheckUnpaidPremiumRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.CheckUnpaidPremium, oCheckUnpaidPremiumRequest)
                    oCheckUnpaidPremiumResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.CheckUnpaidPremiumQueryResponse)(result)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.

                With oCheckUnpaidPremiumResponse

                    oCheckUnPaidDetails.InstalmentOverdue = oCheckUnpaidPremiumResponse.CheckUnpaidPremiumResponse.InstalmentOverdue

                    For Each oBaseCheckUnpaidPremiumResponseTypeRow As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseCheckUnpaidPremiumResponseTypeRow In .CheckUnpaidPremiumResponse.Transactions
                        oTransactions = New Transaction
                        oTransactions.Amount = oBaseCheckUnpaidPremiumResponseTypeRow.amount
                        oTransactions.ShortCode = oBaseCheckUnpaidPremiumResponseTypeRow.short_code
                        oTransactions.DocRef = oBaseCheckUnpaidPremiumResponseTypeRow.document_ref
                        oTransactions.DocumentDate = oBaseCheckUnpaidPremiumResponseTypeRow.document_date
                        oTransactions.OutstandingAmount = oBaseCheckUnpaidPremiumResponseTypeRow.outstanding
                        oTransactions.Branchcode = oBaseCheckUnpaidPremiumResponseTypeRow.BranchCode
                        oTransactions.BranchDescription = oBaseCheckUnpaidPremiumResponseTypeRow.BranchDescription
                        oTransactions.DocumentType = oBaseCheckUnpaidPremiumResponseTypeRow.document_type

                        otransactionsCollection.Add(oTransactions)
                    Next
                End With
                oCheckUnPaidDetails.PolicyTransactions = otransactionsCollection
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

            Catch ex As Exception
                Throw
            Finally
                'oSAM.Close()
                oCheckUnpaidPremiumRequest = Nothing
                oCheckUnpaidPremiumResponse = Nothing
            End Try
            Return oCheckUnPaidDetails
        End SyncLock
    End Function
#Region "WPR28"
    Public Overrides Sub ApproveCashListItem(ByRef r_CashListItemKey As PaymentCashListItemType,
                                                  Optional ByVal v_sBranchCode As String = Nothing)

        SyncLock oLock

            'Dim oSAM As PureClaimServiceClient
            Dim oApproveCashListItemRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.ApproveCashListItemCommand
            Dim oApproveCashListItemResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.ApproveCashListItemCommandResponse
            Dim sbLogMessage As StringBuilder

            Try
                'oSAM = InitializeClaimServiceMethod()
                oApproveCashListItemRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.ApproveCashListItemCommand
                oApproveCashListItemResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.ApproveCashListItemCommandResponse
                sbLogMessage = New StringBuilder


                With oApproveCashListItemRequest
                    .LoginUserName = Current.Session(Nexus.Constants.CNLoginName)
                    '.WCFSecurityToken = SecurityToken()
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

                    If r_CashListItemKey.CashListKey > 0 Then
                        .CashListItemKey = r_CashListItemKey.CashListKey
                    Else
                        Throw New ArgumentNullException("CashListItemKey")
                    End If
                    .Comments = r_CashListItemKey.Comments
                    .Declined = r_CashListItemKey.Declined
                    .ApiTimeStamp = r_CashListItemKey.TimeStamp
                    .CheckValidationOnly = r_CashListItemKey.CheckValidationOnly

                End With


                Using trace As New Tracer(Category.Trace)
                    'oApproveCashListItemResponse = oSAM.ApproveCashListItem(oApproveCashListItemRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.ApproveCashListItem, oApproveCashListItemRequest)
                    oApproveCashListItemResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.ApproveCashListItemCommandResponse)(result)

                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.




                With oApproveCashListItemResponse
                    If .Errors IsNot Nothing Then
                        'Process the error object if errors, and throw as a single exception
                        Throw New NexusException(.Errors)
                    End If

                    r_CashListItemKey.TimeStamp = .ApiTimeStamp

                End With

                If Logger.IsLoggingEnabled Then
                    sbLogMessage.AppendLine("FindBank executed ok" & vbCrLf)
                    sbLogMessage.AppendLine("Input:" & vbCrLf)
                    sbLogMessage.AppendLine("v_CashListItemKey = " & r_CashListItemKey.CashListItemKey.ToString & vbCrLf)
                    'sbLogMessage.AppendLine("r_ApproveCashListItem = " & r_ApproveCashListItem & vbCrLf)
                    sbLogMessage.AppendLine("v_Comments = " & r_CashListItemKey.Comments & vbCrLf)
                    sbLogMessage.AppendLine("v_Declined = " & r_CashListItemKey.Declined & vbCrLf)

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
                'oSAM.Close()
                oApproveCashListItemRequest = Nothing
                oApproveCashListItemResponse = Nothing
            End Try

        End SyncLock
    End Sub
    ''' <summary>
    ''' To Maintain Lock
    ''' </summary>
    ''' <param name="oLockCollection"></param>
    ''' <param name="bClearAll"></param>
    ''' <param name="bIsLogout"></param>
    ''' <param name="sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function MaintainLock(ByVal oLockCollection As LockCollection,
                                           Optional ByVal bClearAll As Boolean = False,
                                           Optional ByVal bIsLogout As Boolean = False,
                                           Optional ByVal sBranchCode As String = Nothing) As Boolean
        SyncLock oLock
            'Dim oSAM As PureCoreServiceClient
            Dim oMaintainLockDetailsRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.MaintainLockCommand
            Dim oMaintainLockDetailsResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.MaintainLockCommandResponse
            Try
                'oSAM = InitializeCoreServiceMethod()
                oMaintainLockDetailsRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.MaintainLockCommand
                oMaintainLockDetailsResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.MaintainLockCommandResponse
                With oMaintainLockDetailsRequest
                    'if the passed parameter sBranchCode is empty 
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
                        'use the passed parameter sBranchCode
                        .BranchCode = sBranchCode
                    End If

                    If bClearAll Then
                        .ClearAllLocks = bClearAll
                    Else
                        .ClearAllLocks = False
                    End If
                    .LockDetails = New List(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseLockDetails)
                    If bIsLogout Then
                        .LogOutSessionValue = HttpContext.Current.Session.SessionID.ToString()
                    End If
                    .LoginUserName = Current.Session(CNLoginName)
                    '.WCFSecurityToken = SecurityToken()

                    If oLockCollection IsNot Nothing AndAlso oLockCollection.Length > 0 Then
                        Dim oLockDetail As SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseLockDetails
                        Dim oLockDetails(oLockCollection.Length - 1) As BaseClasses.BaseLockDetails
                        Dim oClaimLocks As NexusProvider.Locks
                        Dim nLockCount As Integer = 0
                        For Each oClaimLocks In oLockCollection
                            oLockDetail = New SSP.PureInsuranceRestAPIHandler.BaseClasses.BaseLockDetails
                            oLockDetail.LockName = oClaimLocks.LockName
                            oLockDetail.LockValue = oClaimLocks.LockValue
                            oLockDetails.SetValue(oLockDetail, nLockCount)
                            nLockCount += 1
                        Next
                        If oMaintainLockDetailsRequest.LockDetails IsNot Nothing Then
                            .LockDetails.Add(oLockDetail)
                        End If
                    End If

                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    'oMaintainLockDetailsResponse = oSAM.MaintainLock(oMaintainLockDetailsRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Post(ApiMethods.MaintainLock, oMaintainLockDetailsRequest)
                    oMaintainLockDetailsResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.MaintainLockCommandResponse)(result)
                End Using

                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.


                With oMaintainLockDetailsResponse

                End With
            Catch ex As Exception
                Throw (ex)
            Finally
                'oSAM.Close()
                oMaintainLockDetailsRequest = Nothing
                oMaintainLockDetailsResponse = Nothing
            End Try
            Return False
        End SyncLock
    End Function
    ''' <summary>
    ''' GetLockDetails 
    ''' </summary>
    ''' <param name="sBranchCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Overrides Function GetLockDetails(ByVal sBranchCode As String) As LockCollection

        SyncLock oLock
            Dim oLockCollection As NexusProvider.LockCollection
            'Dim oSAM As PureCoreServiceClient
            Dim oGetLockDetailsRequest As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetLockDetailsQuery
            Dim oGetLockDetailsResponse As SSP.PureInsuranceRestAPIHandler.BaseClasses.GetLockDetailsQueryResponse
            Try
                'oSAM = InitializeCoreServiceMethod()
                oLockCollection = New NexusProvider.LockCollection
                oGetLockDetailsRequest = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetLockDetailsQuery
                oGetLockDetailsResponse = New SSP.PureInsuranceRestAPIHandler.BaseClasses.GetLockDetailsQueryResponse

                With oGetLockDetailsRequest
                    'if the passed parameter sBranchCode is empty 
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
                        'use the passed parameter sBranchCode
                        .BranchCode = sBranchCode
                    End If
                    .LoginUserName = Current.Session(CNLoginName)
                    '.WCFSecurityToken = SecurityToken()
                End With


                'Calling the SAM Method with Request Type
                'add trace to allow us to debug slow SAM calls
                Using trace As New Tracer(Category.Trace)
                    ' oGetLockDetailsResponse = oSAM.GetLockDetails(oGetLockDetailsRequest)
                    SSP.PureInsuranceRestAPIHandler.ApiClient._tokenModel = GetApiTokendetails()
                    Dim result As String = SSP.PureInsuranceRestAPIHandler.ApiClient.Get(ApiMethods.GetLockDetails, oGetLockDetailsRequest)
                    oGetLockDetailsResponse = SSP.PureInsuranceRestAPIHandler.ApiClient.DeserializeJson(Of SSP.PureInsuranceRestAPIHandler.BaseClasses.GetLockDetailsQueryResponse)(result)

                End Using

                With oGetLockDetailsResponse

                    Dim iCounter As Integer

                    If .Details IsNot Nothing Then
                        oLockCollection = New LockCollection()
                        For iCounter = 0 To .Details.Count - 1
                            Dim oLock As New NexusProvider.Locks

                            oLock.LockName = .Details(iCounter).LockName
                            oLock.LockValue = .Details(iCounter).LockValue
                            oLock.SessionID = .Details(iCounter).Lock3Value
                            oLock.LockUserName = .Details(iCounter).UserName
                            oLock.LockedTime = .Details(iCounter).LockedTime
                            oLock.IsExclusiveLock = (.Details(iCounter).IsExclusiveLock = 1)
                            oLock.LockedById = .Details(iCounter).LockedByID

                            oLockCollection.Add(oLock)
                        Next
                    End If


                End With
                'NO catches on the try as we want to cascade all exceptions back up the stack for handling.
            Catch ex As Exception
                Throw (ex)
            Finally
                'oSAM.Close()                     ' Disposing the SAM's object
                oGetLockDetailsRequest = Nothing
                oGetLockDetailsResponse = Nothing
            End Try

            Return oLockCollection
        End SyncLock

    End Function
#End Region

End Class
