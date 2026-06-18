'===================================================
'Module Name        : Instalment
'Project            : Pure.Portal
'Copyright          : © SSP Limited 2013. All rights reserved.

'<Description of the file>

'This code adhers to the SSP Coding Standards Document V2013-01

'THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, 
'EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED 
'WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.

'===================================================

Imports CMS.Library
Imports Nexus.Library
Imports Nexus.Library.Config
Imports CMS.Library.Portal
Imports System.Web.Configuration.WebConfigurationManager
Imports System.Xml
Imports System.Globalization.CultureInfo
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session
Imports System.Data
Imports System.Web.Configuration
Imports Nexus.Utils
Imports Nexus
Imports System.Xml.XmlReader
Imports NexusProvider
Imports NexusProvider.Quote
Imports System.Configuration
Imports NexusProvider.Policy
Imports System.Web
Imports System.Linq
Imports System.Xml.Linq
Imports System.IO
Imports Nexus.Constants
Imports System.Resources




Namespace Nexus
    Public Class BaseInstalment

#Region "Private Types"

#End Region

#Region "Private Constants"

#End Region

#Region "Private Variables"

#End Region

#Region "Constructors"

#End Region

#Region "Finalizers"

#End Region

#Region "Public Properties"

#End Region

#Region "Public Methods"
        ''' <summary>
        ''' Get Instalment Quotes
        ''' </summary>
        ''' <param name="dAmountToFinance"></param>
        ''' <param name="dtStartDate"></param>
        ''' <param name="dtEndDate"></param>
        ''' <param name="dtPreferredDate"></param>
        ''' <param name="dtQuoteDate"></param>
        ''' <param name="iWeekDay"></param>
        ''' <param name="iMonthDay"></param>
        ''' <param name="iInsuranceFileKey"></param>
        ''' <param name="dOverrideInterestRate"></param>
        ''' <param name="dOverrideRate"></param>
        ''' <param name="bPaymentProtection"></param>
        ''' <param name="sBranchCode"></param>
        ''' <param name="iInstallmentType"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetInstalmentQuotes(ByVal dAmountToFinance As Double, _
                                                    ByVal dtStartDate As DateTime, _
                                                    ByVal dtEndDate As DateTime, _
                                                    ByVal dtPreferredDate As DateTime, _
                                                    ByVal dtQuoteDate As DateTime, _
                                                    ByVal iWeekDay As Integer, _
                                                    ByVal iMonthDay As Integer, _
                                                    ByVal iInsuranceFileKey As Integer, _
                                                    ByVal dOverrideInterestRate As Double, _
                                                    ByVal dOverrideRate As Double, _
                                                    ByVal bPaymentProtection As Boolean, _
                                                    Optional ByVal sBranchCode As String = Nothing, _
                                                    Optional ByVal iInstallmentType As NexusProvider.InstalmentType = Nothing, _
                                                    Optional ByVal sProcessPFMode As String = Nothing) As NexusProvider.InstallmentQuoteCollection
            Dim oInstalmentQuotes As NexusProvider.InstallmentQuoteCollection = Nothing
            Dim oWebService As NexusProvider.ProviderBase
            Try
                oWebService = New NexusProvider.ProviderManager().Provider

                oInstalmentQuotes = oWebService.GetInstalmentQuotes(dAmountToFinance, _
                                                    dtStartDate, dtEndDate, dtPreferredDate, _
                                                    dtQuoteDate, iWeekDay, iMonthDay, iInsuranceFileKey, dOverrideInterestRate, dOverrideRate, bPaymentProtection, sBranchCode, iInstallmentType)
            Catch ex As Exception
                Throw ex
            Finally
                oWebService = Nothing

            End Try
            Return oInstalmentQuotes

        End Function

        Public Sub GetPremiumFinancePlan(ByVal iPartyKey As Integer, _
                                                   ByVal sStatusKey As String, _
                                                   ByVal sBranchCode As String)
            Dim oFinancePlans As NexusProvider.FinancePlanCollection = Nothing
            Dim oWebService As NexusProvider.ProviderBase

            Try
                oWebService = New NexusProvider.ProviderManager().Provider

                oFinancePlans = oWebService.GetFinancePlan(iPartyKey, sStatusKey, sBranchCode)
                HttpContext.Current.Session("FinancePlans") = oFinancePlans
            Catch ex As Exception
                Throw ex
            Finally
                oWebService = Nothing

            End Try
        End Sub

        ''' <summary>
        ''' Get Premium Finance plans filtered by Claim Number.
        ''' Results are stored in Session("FinancePlans").
        ''' </summary>
        Public Sub GetPremiumFinancePlanByClaimNumber(ByVal iPartyKey As Integer,
                                                      ByVal sStatusKey As String,
                                                      ByVal sClaimNumber As String)
            Dim oFinancePlans As NexusProvider.FinancePlanCollection = Nothing
            Dim oWebService As NexusProvider.ProviderBase

            Try
                oWebService = New NexusProvider.ProviderManager().Provider
                oFinancePlans = oWebService.GetFinancePlanByClaimNumber(iPartyKey, sStatusKey, sClaimNumber)
                HttpContext.Current.Session("FinancePlans") = oFinancePlans
            Catch ex As Exception
                Throw ex
            Finally
                oWebService = Nothing
            End Try
        End Sub

        Public Sub GetPremiumFinancePlanDetails(ByVal sDocumentRef As String, ByVal iFinancePlanKey As Integer, _
                                                ByVal iFinancePlanVersion As Integer, _
                                                   ByVal sBranchCode As String)
            Dim oFinancePlanDetails As NexusProvider.PremiumFinancePlan = Nothing
            Dim oWebService As NexusProvider.ProviderBase
            Dim oOptionSettings As NexusProvider.OptionTypeSetting
            Try
                oWebService = New NexusProvider.ProviderManager().Provider

                oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ExclusiveLock) 'Exclusive Lock
                'HttpContext.Current.Session(CNFinancePlanDetails) = Nothing
                If oOptionSettings.OptionValue = "1" AndAlso HttpContext.Current.Session(CNInstalmentPlanMode) = InstalmentPlanType.edit Then
                    oFinancePlanDetails = oWebService.GetHeaderAndSummariesPFPlanByKey(sDocumentRef, iFinancePlanKey, iFinancePlanVersion, sBranchCode, bExclusiveLock:=True)
                Else
                    oFinancePlanDetails = oWebService.GetHeaderAndSummariesPFPlanByKey(sDocumentRef, iFinancePlanKey, iFinancePlanVersion, sBranchCode)
                End If

                If oFinancePlanDetails IsNot Nothing Then
                    FillSessionValues(oFinancePlanDetails)
                    FillQuoteSession()
                End If

            Catch ex As Exception
                Throw ex
            Finally
                oWebService = Nothing

            End Try
        End Sub

        Protected Sub FillSessionValues(ByVal oFinancePlanDetails As NexusProvider.PremiumFinancePlan)
            HttpContext.Current.Session(CNInsuranceFileKey) = oFinancePlanDetails.PremiumFinanceDetails.InsuranceFileKey

            HttpContext.Current.Session(CNFinancePlanDetails) = oFinancePlanDetails
            'Assigning Transactions value to Session since SAM requires it every time and shouldn't be blank
            HttpContext.Current.Session(CNTransactionDetails) = oFinancePlanDetails.Transactions
           
            If oFinancePlanDetails.PremiumFinanceDetails IsNot Nothing Then
                HttpContext.Current.Session(CNAmountToPay) = oFinancePlanDetails.PremiumFinanceDetails.FinanceAmount
            End If

        End Sub
        Public Sub FillQuoteSession()

            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oQuote As NexusProvider.Quote = Nothing
            If oQuote Is Nothing AndAlso HttpContext.Current.Session(CNInsuranceFileKey) IsNot Nothing AndAlso Convert.ToInt32(HttpContext.Current.Session(CNInsuranceFileKey)) <> 0 Then
                oQuote = oWebservice.GetHeaderAndSummariesByKey(Convert.ToInt32(HttpContext.Current.Session(CNInsuranceFileKey), Nothing))

                HttpContext.Current.Session(CNQuote) = oQuote
                HttpContext.Current.Session(CNCurrenyCode) = oQuote.CurrencyCode
            End If
            HttpContext.Current.Session(CNHasPremiumUpdated) = True

        End Sub

        Public Sub GetTransactionDetails(ByVal nAccountKey As Integer, _
                                  ByVal oAllocationDetailsCollections As NexusProvider.AllocationDetailsCollections, _
                                   Optional ByVal sShortCode As String = Nothing, _
                                  Optional ByVal dtToDate As String = Nothing, _
                                  Optional ByVal sInsuranceRef As String = Nothing, _
                                  Optional ByVal bIsNewPF As Boolean = False, _
                                  Optional ByVal sBranchCode As String = Nothing)

            Dim oTransactionDetails As NexusProvider.AllocationDetailsCollections = Nothing
            Dim oWebService As NexusProvider.ProviderBase

            Try
                oWebService = New NexusProvider.ProviderManager().Provider

                oTransactionDetails = oWebService.GetTransactionDetails(v_iAccountKey:=nAccountKey, v_oAllocationDetailsCollections:=oAllocationDetailsCollections, sShortCode:=sShortCode, dtToDate:=dtToDate, sInsuranceRef:=sInsuranceRef, bIsNewPF:=bIsNewPF, v_sBranchCode:=sBranchCode)
                Dim oTransaction As NexusProvider.FinancePlanTransactions
                Dim oTransactionsCollection As New NexusProvider.FinancePlanTransactionsCollection
                'HttpContext.Current.Session(CNFinancePlanDetails) IsNot Nothing AndAlso
                If oTransactionDetails IsNot Nothing AndAlso oTransactionDetails.Count > 0 Then

                    For Each oFinancePlanTransaction As NexusProvider.AllocationDetails In oTransactionDetails
                        Try
                            With oFinancePlanTransaction
                                oTransaction = New NexusProvider.FinancePlanTransactions
                                oTransaction.Account = .Account
                                oTransaction.AccountCode = .AccountCode
                                oTransaction.AccountKey = .AccountKey
                                oTransaction.AllocatedAmount = .AllocatedAmount
                                oTransaction.AllocatedDate = .AllocatedDate
                                oTransaction.Allocation = .Allocation
                                oTransaction.AllocationAmount = .AllocationAmount
                                oTransaction.AllocationStatus = .AllocationStatus
                                oTransaction.AllocationTimeStamp = .AllocationTimeStamp
                                oTransaction.AllocationTransDetailKey = .AllocationTransDetailKey
                                oTransaction.AltRef = .AltRef
                                oTransaction.Amount = .TransactionCurrencyAmount
                                oTransaction.BranchCode = .BranchCode
                                oTransaction.CashListItemKey = .CashListItemKey
                                oTransaction.Currency = .Currency
                                oTransaction.CurrencyDiff = .CurrencyDiff
                                oTransaction.DocRef = .DocRef
                                oTransaction.DocumentTypeId = .DocumentTypeID
                                oTransaction.DocRef = .DocRef
                                oTransaction.EffectiveDate = .EffectiveDate
                                oTransaction.InsuranceFileKey = .InsuranceFileKey
                                oTransaction.InsuranceRefIndex = .InsuranceRef
                                oTransaction.MediaRef = .MediaRef
                                oTransaction.MediaType = .MediaType
                                oTransaction.OriginalAmount = .OriginalAmount
                                oTransaction.PFTransactionKey = .TransdetailKey
                                oTransaction.Spare = .Spare
                                oTransaction.TaxBand = .TaxBand
                                oTransaction.TransactionCurrency = .TransactionCurrency
                                oTransaction.TransactionCurrencyAmount = .TransactionCurrencyAmount
                                oTransaction.TransDate = .TransDate
                                oTransaction.User = .User
                                oTransaction.WriteOffAmount = .WriteOffAmount
                                oTransaction.WriteOffReason = .WriteOffReason
                                oTransaction.DocType = .DocType
                                oTransaction.Period = .Period
                                oTransaction.PrimarySettled = .PrimarySettled
                                oTransaction.DocGroup = .DocGroup
                                oTransaction.OutStandingamount = .OutStandingamount
                                oTransaction.TransactionCurrencyCode = .TransactionCurrencyCode
                                oTransaction.SourceID = .SourceID
                            End With
                            oTransactionsCollection.Add(oTransaction)


                        Catch ex As Exception
                            Throw ex
                        End Try

                    Next

                    'CType(HttpContext.Current.Session(CNFinancePlanDetails), NexusProvider.PremiumFinancePlan).Transactions = oTransactionsCollection

                End If
                HttpContext.Current.Session(CNTransactionDetails) = oTransactionsCollection
            Catch ex As Exception
                Throw ex
            Finally
                oWebService = Nothing

            End Try
        End Sub

        ''' <summary>
        ''' Get Claim Recovery (CLR) transactions eligible for instalment plan creation.
        ''' Results are stored in Session(CNTransactionDetails).
        ''' </summary>
        ''' <param name="sShortCode">Client short code.</param>
        ''' <param name="sClaimNumber">Claim number filter.</param>
        Public Sub GetClaimRecoveryTransactions(Optional ByVal sShortCode As String = Nothing,
                                               Optional ByVal sClaimNumber As String = Nothing)
            Dim oWebService As NexusProvider.ProviderBase
            Dim oTransactionsCollection As NexusProvider.FinancePlanTransactionsCollection

            Try
                oWebService = New NexusProvider.ProviderManager().Provider
                oTransactionsCollection = oWebService.GetClaimRecoveryTransactions(sShortCode:=sShortCode, sClaimNumber:=sClaimNumber)
                HttpContext.Current.Session(CNTransactionDetails) = oTransactionsCollection
            Catch ex As Exception
                Throw ex
            Finally
                oWebService = Nothing
            End Try
        End Sub

        Public Function UpdatePremiumFinancePlan(ByVal oProcessPFPlanRequest As NexusProvider.PremiumFinancePlan, Optional ByVal sBranchCode As String = Nothing, Optional ByVal bSaveOnly As Boolean = False, Optional ByVal m_PFPlanType As NexusProvider.ProcessPFPlanType = Nothing, _
                                                 Optional ByVal oInstalmentType As InstalmentType = Nothing, Optional ByVal sTransType As String = Nothing) As PremiumFinancePlan
            Dim oWebService As NexusProvider.ProviderBase
            Dim oProcessPFPlan As New NexusProvider.PremiumFinancePlan
            Try
                oWebService = New NexusProvider.ProviderManager().Provider

                If oProcessPFPlanRequest.PremiumFinanceDetails Is Nothing Then
                    oProcessPFPlanRequest.PremiumFinanceDetails = New NexusProvider.FinancePlanDetails
                End If


                If HttpContext.Current.Session(CNParty) IsNot Nothing Then
                    Dim oParty As NexusProvider.BaseParty = HttpContext.Current.Session(CNParty)
                    oProcessPFPlanRequest.PartyCode = oParty.Key

                    Select Case True
                        Case TypeOf HttpContext.Current.Session(CNParty) Is NexusProvider.PersonalParty
                            oParty = CType(HttpContext.Current.Session(CNParty), NexusProvider.PersonalParty)

                            With CType(oParty, NexusProvider.PersonalParty)
                                If .ClientSharedData IsNot Nothing AndAlso String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                                    oProcessPFPlanRequest.PremiumFinanceDetails.ClientCode = CType(HttpContext.Current.Session(CNParty), NexusProvider.PersonalParty).ClientSharedData.ShortName.Trim()
                                ElseIf String.IsNullOrEmpty(.UserName) = False Then
                                    oProcessPFPlanRequest.PremiumFinanceDetails.ClientCode = .UserName.Trim()

                                End If
                                oProcessPFPlanRequest.PremiumFinanceDetails.ClientName = .Title & " " & .Forename & " " & .Lastname
                            End With
                        Case TypeOf HttpContext.Current.Session(CNParty) Is NexusProvider.CorporateParty
                            oParty = CType(HttpContext.Current.Session(CNParty), NexusProvider.CorporateParty)

                            With CType(oParty, NexusProvider.CorporateParty)
                                If String.IsNullOrEmpty(.ClientSharedData.ShortName) = False Then
                                    oProcessPFPlanRequest.PremiumFinanceDetails.ClientCode = .ClientSharedData.ShortName.Trim()

                                ElseIf String.IsNullOrEmpty(.UserName) = False Then
                                    oProcessPFPlanRequest.PremiumFinanceDetails.ClientCode = .UserName.Trim()

                                End If
                                oProcessPFPlanRequest.PremiumFinanceDetails.ClientName = .MainContact
                            End With
                    End Select

                    With oProcessPFPlanRequest.PremiumFinanceDetails

                        .ClientAddress1 = oParty.Addresses(0).Address1
                        .ClientAddress2 = oParty.Addresses(0).Address2
                        .ClientAddress3 = oParty.Addresses(0).Address3
                        .ClientAddress4 = oParty.Addresses(0).Address4
                        .ClientTown = oParty.Addresses(0).Address4
                        .ClientCountry = oParty.Addresses(0).CountryCode
                        .ClientCountryKey = oParty.Addresses(0).CountryKey
                        .ClientPcode = oParty.Addresses(0).PostCode
                        oProcessPFPlanRequest.PremiumFinanceDetails.DateBankDetailsChanged = DateTime.Now
                    End With

                End If

                oProcessPFPlanRequest.SaveOnly = bSaveOnly
                'This case executes only if Plantype is passed as MTA or NewPlan Type : bSaveOnly should be false in both cases
                If (m_PFPlanType = NexusProvider.ProcessPFPlanType.NewPlan Or m_PFPlanType = NexusProvider.ProcessPFPlanType.PlanMTA) AndAlso bSaveOnly = False Then
                    'oProcessPFPlanRequest.TransType = CType(m_PFPlanType, NexusProvider.ProcessPFPlanType)
                    Dim oPaymentDetails As NexusProvider.Payment
                    oPaymentDetails = CType(HttpContext.Current.Session(CNPayment), NexusProvider.Payment)
                    'Common request object for both MTA and new Plan
                    If HttpContext.Current.Session(CNPayment) IsNot Nothing Then

                        Dim oQuote As NexusProvider.Quote = CType(HttpContext.Current.Session(CNQuote), Quote)

                        oProcessPFPlanRequest.PFBankDetails = New NexusProvider.PFBankDetails
                        'Assigning PremiumFinanceKey and Version to inner collection
                        oProcessPFPlanRequest.PremiumFinanceDetails.PFPremiumFinanceKey = oProcessPFPlanRequest.PFPremFinanceKey
                        oProcessPFPlanRequest.PremiumFinanceDetails.PFPremiumFinanceVersionKey = oProcessPFPlanRequest.PFPremFinanceVersion
                        If oProcessPFPlanRequest.PremiumFinanceDetails.AutoGeneratedPlanRef Is Nothing OrElse oProcessPFPlanRequest.PremiumFinanceDetails.AutoGeneratedPlanRef <> String.Empty Then
                            oProcessPFPlanRequest.PremiumFinanceDetails.AutoGeneratedPlanRef = oProcessPFPlanRequest.PFPremFinanceKey
                        End If

                        ''pass overriden deposit amount else configured deposit is calculated
                        If oQuote.InstDepositAmount >= 0 Then
                            oProcessPFPlanRequest.PremiumFinanceDetails.Deposit = oQuote.InstDepositAmount
                        Else
                            oProcessPFPlanRequest.PremiumFinanceDetails.Deposit = -1
                        End If

                        With oPaymentDetails
                            'oProcessPFPlanRequest.PremiumFinanceDetails.PFFrequencyCode = oPaymentDetails.
                            oProcessPFPlanRequest.PremiumFinanceDetails.SelectedSchemeNo = .SelectedSchemeNo
                            oProcessPFPlanRequest.PremiumFinanceDetails.SelectedSchemeVersion = .SelectedSchemeVersion
                            oProcessPFPlanRequest.PremiumFinanceDetails.PFRFKEY = .Pref_ID
                            oProcessPFPlanRequest.PremiumFinanceDetails.OverrideInterestRate = .OverrideInterestRate
                            'oProcessPFPlanRequest.PremiumFinanceDetails.OverrideRate = .OverrideRate
                            oProcessPFPlanRequest.PremiumFinanceDetails.DayOfWeekOrMonth = .MonthDay
                            oProcessPFPlanRequest.PremiumFinanceDetails.PreferredDate = .PreferredDate
                            oProcessPFPlanRequest.PremiumFinanceDetails.FinanceAmount = .AmountToFinance
                            oProcessPFPlanRequest.PremiumFinanceDetails.PFFrequencyCode = "M" 'sending hardcoded "M" because no input field are available for week
                            oProcessPFPlanRequest.Type = CType(.InstallmentType, NexusProvider.InstalmentType)
                            oProcessPFPlanRequest.PremiumFinanceDetails.StartDate = .StartDate
                            oProcessPFPlanRequest.PremiumFinanceDetails.PaymentProtection = .PaymentProtection
                            oProcessPFPlanRequest.PremiumFinanceDetails.EndDate = .EndDate
                            oProcessPFPlanRequest.PremiumFinanceDetails.DaysDelay = .DaysDelay
                            oProcessPFPlanRequest.PremiumFinanceDetails.ProcessPFMode = HttpContext.Current.Session("PFProductCode")
                            oProcessPFPlanRequest.TransType = m_PFPlanType


                            oProcessPFPlanRequest.PremiumFinanceDetails.PartyBankKey = .PartyBankKey
                            oProcessPFPlanRequest.PremiumFinanceDetails.BankAccountName = .BankAccountName

                            oProcessPFPlanRequest.PremiumFinanceDetails.BankAccountNo = .BankAccountNo
                            oProcessPFPlanRequest.PremiumFinanceDetails.BankAreaCode = .BankAreaCode
                            oProcessPFPlanRequest.PremiumFinanceDetails.BankBranch = .BankBranch
                            oProcessPFPlanRequest.PremiumFinanceDetails.BankExtension = .BankExtn
                            oProcessPFPlanRequest.PremiumFinanceDetails.BankFaxNo = .BankFax
                            oProcessPFPlanRequest.PremiumFinanceDetails.BankFaxAreaCode = .BankFaxCode
                            oProcessPFPlanRequest.PremiumFinanceDetails.BankName = .BankName
                            oProcessPFPlanRequest.PremiumFinanceDetails.BankPhoneNo = .BankPhone
                            oProcessPFPlanRequest.PremiumFinanceDetails.BankSortCode = .BankSortCode

                            If .BankAddress IsNot Nothing Then

                                oProcessPFPlanRequest.PremiumFinanceDetails.BankAddress1 = .BankAddress.Address1
                                oProcessPFPlanRequest.PremiumFinanceDetails.BankAddress2 = .BankAddress.Address2
                                oProcessPFPlanRequest.PremiumFinanceDetails.BankAddress3 = .BankAddress.Address3

                                oProcessPFPlanRequest.PremiumFinanceDetails.BankRegion = .BankAddress.Address4
                                oProcessPFPlanRequest.PremiumFinanceDetails.BankPostCode = .BankAddress.PostCode
                                oProcessPFPlanRequest.PremiumFinanceDetails.BankCountryKey = GetCodeForKey(ListType.PMLookup, .BankAddress.CountryCode, "COUNTRY", False)
                                oProcessPFPlanRequest.PremiumFinanceDetails.BankCountry = .BankAddress.CountryCode
                            End If

                            If .CreditCard IsNot Nothing Then
                                oProcessPFPlanRequest.PFCreditCardDetails = New NexusProvider.CreditCard
                                oProcessPFPlanRequest.PFCreditCardDetails.AuthCode = .CreditCard.AuthCode
                                oProcessPFPlanRequest.PFCreditCardDetails.ManualAuthCode = .CreditCard.AuthCode
                                oProcessPFPlanRequest.PFCreditCardDetails.PartyBankKey = .CreditCard.PartyBankKey
                                oProcessPFPlanRequest.PFCreditCardDetails.Number = .CreditCard.Number

                                oProcessPFPlanRequest.PFCreditCardDetails.ExpiryDate = .CreditCard.ExpiryDate


                                oProcessPFPlanRequest.PFCreditCardDetails.NameOnCreditCard = .CreditCard.NameOnCreditCard
                                oProcessPFPlanRequest.PFCreditCardDetails.TrackingNumber = .CreditCard.TrackingNumber
                                oProcessPFPlanRequest.PFCreditCardDetails.VIAPaymentHub = .CreditCard.VIAPaymentHub
                            End If

                            If HttpContext.Current.Session(CNTransactionDetails) IsNot Nothing Then
                                ' ReSharper disable once VBWarnings::BC42016
                                oProcessPFPlanRequest.Transactions = HttpContext.Current.Session(CNTransactionDetails)
                            End If
                            If oQuote IsNot Nothing Then
                                oProcessPFPlanRequest.PremiumFinanceDetails.InsuranceFileKey = oQuote.InsuranceFileKey
                            End If
                        End With

                        If m_PFPlanType = NexusProvider.ProcessPFPlanType.NewPlan Then
                            If (sTransType <> "SED") Then
                                oProcessPFPlanRequest.Type = NexusProvider.InstalmentType.AddToNewPlan
                            End If

                            oProcessPFPlanRequest.PremiumFinanceDetails.StatusInd = NexusProvider.FinancePlanStatus.Item040
                        Else
                            HttpContext.Current.Session(CNMTAPlanType) = CType(oPaymentDetails.InstallmentType, NexusProvider.InstalmentType)

                            'send live in case of MTA too.
                            oProcessPFPlanRequest.PremiumFinanceDetails.StatusInd = NexusProvider.FinancePlanStatus.Item040
                            If oInstalmentType = InstalmentType.NoAmountChange Then
                                HttpContext.Current.Session(CNMTAPlanType) = InstalmentType.NoAmountChange
                                oProcessPFPlanRequest.PremiumFinanceDetails.FinanceAmount = 0
                                If (sTransType <> "SED") Then
                                    oProcessPFPlanRequest.Type = InstalmentType.NoAmountChange
                                End If
                            End If
                        End If
                    End If
                End If

                oProcessPFPlan = oWebService.ProcessPfPlan(oProcessPFPlanRequest, sBranchCode)

                'If oProcessPFPlan IsNot Nothing Then
                '    HttpContext.Current.Response.Redirect("~/PremiumFinance/FinancePlanDetails.aspx?FinancePlanKey=" & oProcessPFPlan.PFPremFinanceKey & "&FinancePlanVersion=" & oProcessPFPlan.PFPremFinanceVersion)
                'End If
                Dim oOptionSettings As NexusProvider.OptionTypeSetting
                oOptionSettings = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, NexusProvider.SystemOptions.ExclusiveLock) 'Exclusive Lock
                If oOptionSettings.OptionValue = "1" Then
                    FrameWorkFunctions.UnlockPolicy(oProcessPFPlanRequest.PFPremFinanceKey, HttpContext.Current.Session(CNBranchCode).ToString)
                End If

            Catch ex As Exception
                Throw ex
            Finally
                oWebService = Nothing
                HttpContext.Current.Session("PFProductCode") = Nothing
            End Try
            Return oProcessPFPlan
        End Function

        ''' <summary>
        ''' Used for CancelPremiumFInance plan and to provide debit transdetail key
        ''' </summary>
        ''' <param name="nPFPremiumFinanceKey"></param>
        ''' <param name="nPFPremiumFinanceVersionKey"></param>
        ''' <param name="sReasonCode"></param>
        ''' <param name="sRequestType"></param>
        ''' <param name="r_nDebitTransdetailKey"></param>
        ''' <remarks></remarks>
        Public Sub CancelPremiumFinancePlan(ByVal nPFPremiumFinanceKey As Integer, _
                                                        ByVal nPFPremiumFinanceVersionKey As Integer, _
                                                        ByVal sReasonCode As String, _
                                                        ByVal sRequestType As String, _
                                                        ByRef r_nDebitTransdetailKey As Integer)
            Dim oWebService As NexusProvider.ProviderBase

            Try
                oWebService = New NexusProvider.ProviderManager().Provider
                oWebService.CancelPremiumFinancePlan(nPFPremiumFinanceKey, nPFPremiumFinanceVersionKey, sReasonCode, sRequestType, _
                                                     r_nDebitTransdetailKey)

            Catch ex As Exception
                Throw ex

            Finally
                oWebService = Nothing
            End Try
        End Sub

        ''' <summary>
        ''' SettlePremiumFinancePlan used for settlement of Premium finance plan
        ''' </summary>
        ''' <param name="nPFPremiumFinanceKey"></param>
        ''' <param name="nPFPremiumFinanceVersionKey"></param>
        ''' <param name="r_nDebitTransdetailKey"></param>
        ''' <remarks></remarks>
        Public Sub SettlePremiumFinancePlan(ByVal nPFPremiumFinanceKey As Integer, ByVal nPFPremiumFinanceVersionKey As Integer, _
                                                         ByRef r_nDebitTransdetailKey As Integer)

            Dim oWebService As NexusProvider.ProviderBase

            Try
                oWebService = New NexusProvider.ProviderManager().Provider
                oWebService.CancelPremiumFinancePlan(nPFPremiumFinanceKey, nPFPremiumFinanceVersionKey, "", NexusProvider.CancelPFPlanType.SettlePlan, r_nDebitTransdetailKey)

            Catch ex As Exception
                Throw ex
            Finally
                oWebService = Nothing

            End Try
        End Sub

        Public Sub CancelPFPolicies(ByVal nPFPremiumFinanceKey As Integer, _
                                                ByVal nPFPremiumFinanceVersionKey As Integer, _
                                                ByVal sLapsedReasonCode As String, _
                                                ByVal bSpoolDoc As Boolean, _
                                                ByVal bWriteOff As Boolean, _
                                                ByVal dtPolicyLapsedDate As Date)
            Dim oWebService As NexusProvider.ProviderBase

            Try
                oWebService = New NexusProvider.ProviderManager().Provider
                oWebService.CancelPFPolicies(nPFPremiumFinanceKey, nPFPremiumFinanceVersionKey, sLapsedReasonCode, bSpoolDoc, bWriteOff, dtPolicyLapsedDate)

            Catch ex As Exception
                Throw ex
            Finally
                oWebService = Nothing

            End Try
        End Sub

        Public Sub UpdateInstalmentStatus(ByVal nPFInstalmentKey As Integer, _
                                                        ByVal sStatusCode As String, _
                                                        ByVal sBranchCode As String)

            Dim oWebService As NexusProvider.ProviderBase

            Try
                oWebService = New NexusProvider.ProviderManager().Provider
                oWebService.UpdateInstalmentStatus(nPFInstalmentKey, sStatusCode, sBranchCode)

            Catch ex As Exception
                Throw ex
            Finally
                oWebService = Nothing

            End Try
        End Sub

        Public Sub UpdateInstalmentDetails(ByVal nPFInstalmentKey As Integer, _
                                                       ByVal nPFInstalmentVersion As Integer, _
                                                       ByVal nInstalmentNo As Integer, _
                                                       ByVal dtDueDate As DateTime, _
                                                       ByVal sBranchCode As String)

            Dim oWebService As NexusProvider.ProviderBase

            Try
                oWebService = New NexusProvider.ProviderManager().Provider
                oWebService.UpdateInstalmentDetails(nPFInstalmentKey, nPFInstalmentVersion, nInstalmentNo, dtDueDate, sBranchCode)

            Catch ex As Exception

            Finally
                oWebService = Nothing

            End Try
        End Sub
#End Region

#Region "Private Methods"

#End Region

#Region "Event Handlers"

#End Region

    End Class
End Namespace

