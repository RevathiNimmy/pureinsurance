Imports CMS.Library
Imports Nexus.Library
Imports System.Web.Configuration.WebConfigurationManager
Imports Nexus.Constants.Constant
Imports Nexus.Constants.Session

Imports System.Data
Imports Nexus

Imports NexusProvider.Quote
Imports System.Linq
Imports System.Text
Imports System.Xml.Linq
Imports System.IO
Imports Nexus.Constants


Namespace Nexus

    Partial Class TransactionConfirmation : Inherits Frontend.clsCMSPage

        Private _InsuranceFileKey As Integer
        Private _ClaimKey As Integer
        Private _PartyKey As Integer

        Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
            'Load the TransactionConfirmation control on page init otherwise view state for Document manager will not get maintained
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oProductConfig As Config.Product = oNexusConfig.Portals.Portal(Portal.GetPortalID()).Products.Product(oQuote.ProductCode)

            Dim WebControlPath As String
            WebControlPath = "~/Products/" & oProductConfig.Name & "/TransactionConfirmation.ascx"
            If (System.IO.File.Exists(Request.MapPath(WebControlPath))) Then
                Dim tempControl As Control = LoadControl(WebControlPath)
                TransactionConfirmation.Controls.Clear()
                TransactionConfirmation.Controls.Add(tempControl)
            End If

        End Sub

        Protected Shadows Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
            HttpContext.Current.Response.Cache.SetCacheability(HttpCacheability.NoCache)
            HttpContext.Current.Response.Cache.SetNoServerCaching()
            HttpContext.Current.Response.Cache.SetNoStore()
            Dim paymentOptions As Config.PaymentTypes = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).PaymentTypes
            Dim oPayment As NexusProvider.Payment = Nothing
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oPolicySummary As NexusProvider.PolicySummary
            Dim bIsBackDatedMTA As Boolean = False 'To check that MTA is backdated or not
            Dim oCreditCard As NexusProvider.CreditCard
            Dim oNexusConfig As Config.NexusFrameWork = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork)
            Dim oPortal As Nexus.Library.Config.Portal = oNexusConfig.Portals.Portal(Portal.GetPortalID())

            If (Session(CNPaid) = True OrElse (Session(CNStatementsAgreed) Is Nothing AndAlso Session(CNMTAType) = MTAType.CANCELLATION)) AndAlso Session(CNIsTransactionConfirmationVisited) Is Nothing AndAlso
                (Session(CNIsCancelMTA) Is Nothing OrElse CType(Session(CNIsCancelMTA), Boolean) = False) Then

                If Not IsPostBack Then

                    SetPageProgress(7)

                    If Session(CNPaymentHubDetails) IsNot Nothing AndAlso Session(CNPayment) IsNot Nothing AndAlso Session(CNCardDetails) IsNot Nothing AndAlso Request.QueryString("Mode") Is Nothing Then
                        oPayment = Session(CNPayment)

                        If oPayment.PayNowPaymentDetails IsNot Nothing AndAlso oPayment.PayNowPaymentDetails.MediaTypeCode = "OCP" Then
                            oCreditCard = Session(CNCardDetails)
                            oPayment.PayNowDetails.CCAuthCode = oCreditCard.AuthCode
                            oPayment.PayNowDetails.CCManualAuthCode = oCreditCard.AuthCode
                            oPayment.PayNowDetails.CCExpiryDate = oCreditCard.ExpiryDate
                            oPayment.PayNowDetails.CCName = oCreditCard.NameOnCreditCard
                            oPayment.PayNowDetails.CCNumber = oCreditCard.Number
                            oPayment.PayNowDetails.PartyBankKey = oCreditCard.PartyBankKey
                            oPayment.PayNowDetails.CCTrackingNumber = oCreditCard.TrackingNumber


                        End If
                    End If
                    'this checks if in case of NB will display the schedule and Certificate
                    Dim dTotalPremium As Decimal
                    If oQuote.Risks.Count > 0 Then
                        dTotalPremium = Session(CNAmountToPay)
                    End If

                    If (dTotalPremium <= 0.0) And (Session(CNMTAType) <> MTAType.CANCELLATION) And Session(CNMTAType) IsNot Nothing Then
                        'In case of MTA Permanent or Temporary
                        If Not Session(CNMTAType) Is Nothing Then
                            If (dTotalPremium = 0.0) Then
                                lblMTAPremiumReturn.Text = GetLocalResourceObject("lbl_NoChange_Text").ToString()
                            ElseIf (dTotalPremium < 0.0) Then
                                lblMTAPremiumReturn.Text = GetLocalResourceObject("lbl_ReturnPremium_Text").ToString()
                            End If
                            If (dTotalPremium = 0.0) Then
                                oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.Cheque)
                            Else
                                Select Case paymentOptions.PaymentType(Session(CNSelectedPaymentIndex)).Type
                                    Case "Invoice"
                                        'oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.AgentCollection, CDec(Session(CNAmountToPay)))
                                        If Session(CNPayment) Is Nothing Then 'AgentCollection without Pre Payment
                                            oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.AgentCollection, CDec(Session(CNAmountToPay)))
                                        Else 'AgentCollection with Pre Payment
                                            oPayment = Session(CNPayment)
                                        End If
                                    Case "Pay Now"
                                        oPayment = Session(CNPayment) 'PayNow
                                    Case "DebitCard"
                                        oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.DebitCard, CDec(Session(CNAmountToPay)))
                                    Case "CreditCard"
                                        'oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.CreditCard, CDec(Session(CNAmountToPay)))
                                        If Session(CNPayment) Is Nothing Then
                                            oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.CreditCard, CDec(Session(CNAmountToPay)))
                                        Else 'AgentCollection with Pre Payment
                                            oPayment = Session(CNPayment)
                                        End If
                                    Case "BankersDraft"
                                        oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.BankersDraft, CDec(Session(CNAmountToPay)))
                                    Case "Cash"
                                        oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.Cash, CDec(Session(CNAmountToPay)))
                                    Case "Cheque"
                                        oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.Cheque, CDec(Session(CNAmountToPay)))
                                    Case Else
                                        oPayment = Session(CNPayment)
                                End Select
                            End If
                        End If
                    ElseIf (dTotalPremium = 0.0) And Session(CNMTAType) Is Nothing Then
                        'New Business
                        oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.Cheque)
                    ElseIf (dTotalPremium <= 0.0) And (Session(CNMTAType) = MTAType.CANCELLATION) Then
                        'In case of MTA Cancellation
                        'lblMTAPremiumReturn.Text = GetLocalResourceObject("lbl_ReturnPremium_Text").ToString()
                        If Session(CNPayment) IsNot Nothing Then
                            oPayment = Session(CNPayment)
                            If oPayment.PayNowPaymentDetails IsNot Nothing Then
                                lblMTAPremiumReturn.Text = GetLocalResourceObject("lbl_CancelTransaction_text").ToString()
                                Select Case UCase(oPayment.PayNowPaymentDetails.MediaTypeCode)
                                    Case "CA"
                                        lblMTAPremiumReturn.Text = lblMTAPremiumReturn.Text.Replace("#MediaType", "Cash")
                                    Case "CC"
                                        lblMTAPremiumReturn.Text = lblMTAPremiumReturn.Text.Replace("#MediaType", "Credit Card")
                                    Case "BD"
                                        lblMTAPremiumReturn.Text = lblMTAPremiumReturn.Text.Replace("#MediaType", "Bankers Draft")
                                    Case "DD"
                                        lblMTAPremiumReturn.Text = lblMTAPremiumReturn.Text.Replace("#MediaType", "Direct Debit")
                                    Case "CQ"
                                        lblMTAPremiumReturn.Text = lblMTAPremiumReturn.Text.Replace("#MediaType", "Cheque")
                                    Case "EFT"
                                        lblMTAPremiumReturn.Text = lblMTAPremiumReturn.Text.Replace("#MediaType", "EFT")
                                End Select
                            Else
                                lblMTAPremiumReturn.Text = GetLocalResourceObject("lbl_ReturnPremium_Text").ToString()
                            End If
                        Else
                            oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.Cheque)
                            lblMTAPremiumReturn.Text = GetLocalResourceObject("lbl_CancelTransaction_text").ToString()
                            lblMTAPremiumReturn.Text = lblMTAPremiumReturn.Text.Replace("#MediaType", "Cheque")
                        End If
                    Else

                        Select Case paymentOptions.PaymentType(Session(CNSelectedPaymentIndex)).Type
                            Case "Invoice"
                                'oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.AgentCollection, CDec(Session(CNAmountToPay)))
                                If Session(CNPayment) Is Nothing Then 'AgentCollection without Pre Payment
                                    oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.AgentCollection, CDec(Session(CNAmountToPay)))
                                Else 'AgentCollection with Pre Payment
                                    oPayment = Session(CNPayment)
                                End If
                            Case "PayNow"
                                oPayment = Session(CNPayment) 'PayNow
                            Case "DebitCard"
                                oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.DebitCard, CDec(Session(CNAmountToPay)))
                            Case "CreditCard"
                                'oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.CreditCard, CDec(Session(CNAmountToPay)))
                                If Session(CNPayment) Is Nothing Then
                                    oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.CreditCard, CDec(Session(CNAmountToPay)))
                                Else 'AgentCollection with Pre Payment
                                    oPayment = Session(CNPayment)
                                End If
                            Case "BankersDraft"
                                oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.BankersDraft, CDec(Session(CNAmountToPay)))
                            Case "Cash"
                                oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.Cash, CDec(Session(CNAmountToPay)))
                            Case "Cheque"
                                oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.Cheque, CDec(Session(CNAmountToPay)))
                            Case Else
                                oPayment = Session(CNPayment)
                        End Select

                    End If
                    If Session(CNInstalmentMediaType) IsNot Nothing AndAlso Session(CNInstalmentMediaType).ToString().ToUpper() = "CREDIT CARD" AndAlso Session(CNCardDetails) IsNot Nothing Then
                        If Session(CNPayment) Is Nothing Then
                            oPayment = New NexusProvider.Payment(NexusProvider.PaymentTypes.CreditCard, 0)
                        End If

                        oCreditCard = Session(CNCardDetails)
                        Dim oCreditCardType As New NexusProvider.CreditCardType

                        oCreditCardType.Number = oCreditCard.Number
                        oCreditCardType.AuthCode = oCreditCard.AuthCode
                        oCreditCardType.ExpiryDate = oCreditCard.ExpiryDate
                        oCreditCardType.TransactionCode = oCreditCard.TransactionCode
                        oCreditCardType.PartyBankKey = oCreditCard.PartyBankKey
                        oCreditCardType.NameOnCreditCard = oCreditCard.NameOnCreditCard
                        oCreditCardType.TrackingNumber = oCreditCard.TrackingNumber
                        oPayment.CreditCard = oCreditCardType

                    End If
                    If Session(CNSelectedAccount) IsNot Nothing AndAlso oPayment IsNot Nothing Then
                        If Convert.ToString(Session(CNSelectedAccount)) = "Client" Then
                            oPayment.DebitAgainstAccount = "CLIENT"
                            oPayment.DebitAgainst = NexusProvider.DebitAgainstType.DebitAgainstFloatBalance
                        End If
                        If Convert.ToString(Session(CNSelectedAccount)) = "Agent" Then
                            oPayment.DebitAgainstAccount = "AGENT"
                            oPayment.DebitAgainst = NexusProvider.DebitAgainstType.DebitAgainstFloatBalance
                        End If
                    End If

                    '[start]Changes as per WPR 63 --- to change the status of live quote
                    Dim oQuoteStatus As NexusProvider.Quote.QuoteStatusType
                    If Session(CNQuoteMode) = QuoteMode.FullQuote AndAlso Session(CNRenewal) Is Nothing AndAlso Session(CNMTAType) Is Nothing Then


                        Dim sQuoteVersioning As String = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsQuoteVersioning, NexusProvider.RiskTypeOptions.Code, oQuote.ProductCode, "")
                        If (Not String.IsNullOrEmpty(sQuoteVersioning) AndAlso sQuoteVersioning.Trim = "1") Then
                            oQuoteStatus = oQuote.QuoteStatusKey
                            oQuote.QuoteStatusKey = NexusProvider.Quote.QuoteStatusType.Live
                            oWebService.UpdateQuoteStatus(oQuote)
                        End If

                    End If
                    '[end]Changes as per WPR 63 --- to change the status of live quote


                    Try

                        If Session(CNIsBackDatedMTA) = True Or Session(CNBackDatedReinstatement) = True Then
                            bIsBackDatedMTA = True
                        End If

                        If Session(CNMTAType) = MTAType.PERMANENT Or Session(CNMTAType) = MTAType.TEMPORARY Then
                            oPolicySummary = New NexusProvider.PolicySummary(oQuote.Reference)
                            '6389 - MTA Refund Process on Instalments
                            If dTotalPremium < 0 AndAlso (Session(CNMTAType) <> MTAType.CANCELLATION) Then
                                oPolicySummary = oWebService.BindQuote(oQuote.InsuranceFileKey, oPayment, oQuote.TimeStamp, Nothing, Nothing, "MTA", bIsBackDatedMTA, v_bPayNegativePremiumMTABalance:=True)
                            Else
                                oPolicySummary = oWebService.BindQuote(oQuote.InsuranceFileKey, oPayment, oQuote.TimeStamp, Nothing, Nothing, "MTA", bIsBackDatedMTA)
                            End If
                            Try
                                If oQuote.DeleteRenQuoteReRunRenewal Then
                                    oQuote.Reference = oPolicySummary.Reference
                                    Dim oInsuranceFileDetailsCollection As NexusProvider.InsuranceFileDetailsCollection
                                    oInsuranceFileDetailsCollection = oWebService.FindPolicy(oQuote.InsuranceFileRef, "", "", NexusProvider.InsuranceFileTypes.RENEWAL, False)
                                    If oInsuranceFileDetailsCollection IsNot Nothing Then
                                        For Each oInsuranceFileDetails As NexusProvider.InsuranceFileDetails In oInsuranceFileDetailsCollection
                                            Dim oTempQuote As New NexusProvider.Quote
                                            oTempQuote = oWebService.GetHeaderAndSummariesByKey(oInsuranceFileDetails.InsuranceFileKey, , bExclusiveLock:=True)
                                            oWebService.DeleteRenewal(oTempQuote, oTempQuote.BranchCode)
                                        Next
                                    End If
                                    oWebService.RunRenewalSelectionByPolicy(oQuote, oQuote.BranchCode)
                                    lblMTAReRunRenewal.Text = "<br>Renewal Deletion/Selection was Successful"
                                    Session(CNMtaRenSelResponse) = lblMTAReRunRenewal.Text
                                End If
                            Catch ex As Exception
                                lblMTAReRunRenewal.Text = "<br>Renewal Deletion/Selection was Unsuccessful"
                                Session(CNMtaRenSelResponse) = lblMTAReRunRenewal.Text
                            End Try
                            LblOrderID.Text = GetLocalResourceObject("lbl_Transaction_text").ToString() & oPolicySummary.Reference

                            'If payment method is Instalment then need to show instalment plan reference number
                            If paymentOptions.PaymentType(Session(CNSelectedPaymentIndex)).Type = "PremiumFinance" Then
                                Dim sConfirmationMessage As String = ""
                                If dTotalPremium >= 0 Then
                                    If Not oPayment Is Nothing AndAlso oPayment.InstallmentType = NexusProvider.InstalmentType.AddToNewPlan Then
                                        sConfirmationMessage = GetLocalResourceObject("lbl_Transaction_textForInstalment")
                                        sConfirmationMessage = sConfirmationMessage.Replace("#InstalmentPlanRef", oPolicySummary.InstalmentPlanRef)
                                    Else
                                        sConfirmationMessage = GetLocalResourceObject("lbl_Transaction_textForInstalmentAmendment")
                                    End If
                                    sConfirmationMessage = sConfirmationMessage.Replace("#PolicyNumber", oPolicySummary.Reference)
                                    LblOrderID.Text = sConfirmationMessage
                                    lblMTAPremiumReturn.Text = ""
                                Else
                                    sConfirmationMessage = GetLocalResourceObject("lbl_Transaction_textForInstalmentAmendment")
                                    sConfirmationMessage = sConfirmationMessage.Replace("#PolicyNumber", oPolicySummary.Reference)
                                    LblOrderID.Text = sConfirmationMessage
                                    lblMTAPremiumReturn.Text = ""
                                End If
                            Else
                                If dTotalPremium < 0 AndAlso Session(CNMTAType) = MTAType.PERMANENT Then
                                    LblOrderID.Text = ""
                                    Dim sConfirmationMessage As String = GetLocalResourceObject("lbl_ReturnPremium_Text")
                                    sConfirmationMessage = sConfirmationMessage.Replace("#PolicyNumber", oPolicySummary.Reference)
                                    lblMTAPremiumReturn.Text = sConfirmationMessage
                                Else
                                    LblOrderID.Text = GetLocalResourceObject("lbl_Transaction_text").ToString() & oPolicySummary.Reference
                                End If
                            End If

                        ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
                            oPolicySummary = New NexusProvider.PolicySummary(oQuote.Reference)
                            oPolicySummary = oWebService.BindQuote(oQuote.InsuranceFileKey, oPayment, oQuote.TimeStamp, Nothing, oQuote.BranchCode, "MTC", bIsBackDatedMTA)
                            'LblOrderID.Text = GetLocalResourceObject("lbl_CancelTransaction_text").ToString() & " " & oPolicySummary.Reference
                            Dim sConfirmationMessage As String = lblMTAPremiumReturn.Text
                            lblMTAPremiumReturn.Text = sConfirmationMessage.Replace("#PolicyNumber", oPolicySummary.Reference)
                            LblOrderID.Text = ""
                        ElseIf Session(CNMTAType) = MTAType.REINSTATEMENT Then
                            oPolicySummary = New NexusProvider.PolicySummary(oQuote.Reference)
                            oPolicySummary = oWebService.BindQuote(oQuote.InsuranceFileKey, oPayment, oQuote.TimeStamp, Nothing, oQuote.BranchCode, "MTR", bIsBackDatedMTA)
                            If paymentOptions.PaymentType(Session(CNSelectedPaymentIndex)).Type.ToString = "PremiumFinance" Then
                                'If payment method is Instalment then need to show instalment plan referance number
                                Dim sConfirmationMessage As String = GetLocalResourceObject("lbl_Transaction_textForInstalment").ToString
                                sConfirmationMessage = sConfirmationMessage.Replace("#PolicyNumber", oPolicySummary.Reference)
                                sConfirmationMessage = sConfirmationMessage.Replace("#InstalmentPlanRef", oPolicySummary.InstalmentPlanRef)
                                LblOrderID.Text = sConfirmationMessage
                            Else
                                LblOrderID.Text = GetLocalResourceObject("lbl_Transaction_text").ToString() & oPolicySummary.Reference 'oQuote.InsuranceFileRef 'oPolicySummary.Reference
                            End If

                        ElseIf Session(CNRenewal) IsNot Nothing Then
                            oPolicySummary = New NexusProvider.PolicySummary(oQuote.Reference)
                            oPolicySummary = oWebService.BindQuote(oQuote.InsuranceFileKey, oPayment, oQuote.TimeStamp, True, oQuote.BranchCode, "REN", bIsBackDatedMTA)
                            If paymentOptions.PaymentType(Session(CNSelectedPaymentIndex)).Type.ToString = "PremiumFinance" Then
                                'If payment method is Instalment then need to show instalment plan referance number
                                Dim sConfirmationMessage As String = GetLocalResourceObject("lbl_Transaction_textForInstalment").ToString
                                sConfirmationMessage = sConfirmationMessage.Replace("#PolicyNumber", oPolicySummary.Reference)
                                sConfirmationMessage = sConfirmationMessage.Replace("#InstalmentPlanRef", oPolicySummary.InstalmentPlanRef)
                                LblOrderID.Text = sConfirmationMessage
                            Else
                                LblOrderID.Text = GetLocalResourceObject("lbl_Transaction_text").ToString() & oPolicySummary.Reference 'oQuote.InsuranceFileRef 'oPolicySummary.Reference
                            End If
                        Else
                            If oPayment IsNot Nothing Then
                                oPayment.StartDate = oQuote.CoverStartDate
                            End If
                            oPolicySummary = oWebService.BindQuote(oQuote.InsuranceFileKey, oPayment, oQuote.TimeStamp, Nothing, oQuote.BranchCode, "NB")
                            If paymentOptions.PaymentType(Session(CNSelectedPaymentIndex)).Type = "PremiumFinance" Then
                                'If payment method is Instalment then need to show instalment plan referance number
                                Dim sConfirmationMessage As String = GetLocalResourceObject("lbl_Transaction_textForInstalment")
                                sConfirmationMessage = sConfirmationMessage.Replace("#PolicyNumber", oPolicySummary.Reference)
                                sConfirmationMessage = sConfirmationMessage.Replace("#InstalmentPlanRef", oPolicySummary.InstalmentPlanRef)
                                LblOrderID.Text = sConfirmationMessage
                            Else
                                LblOrderID.Text = GetLocalResourceObject("lbl_Transaction_text").ToString() & oPolicySummary.Reference 'oQuote.InsuranceFileRef 'oPolicySummary.Reference
                            End If
                        End If
                        If oPayment IsNot Nothing Then
                            If oPayment.PayNowPaymentDetails IsNot Nothing AndAlso oPayment.PayNowPaymentDetails.MediaTypeCode IsNot Nothing Then
                                oPolicySummary.MediaTypeCode = oPayment.PayNowPaymentDetails.MediaTypeCode
                            Else
                                oPolicySummary.MediaTypeCode = ""
                            End If
                        End If
                        Session.Item(CNPolicy_Summary) = oPolicySummary

                        'NIA WPR 09 - create cashlist for the credit card deposit amount 
                        Dim paymentTypes As New Config.PaymentTypes

                        paymentTypes = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID).PaymentTypes

                        Dim PaymentCollectionUrl As String = paymentTypes.PaymentType(Session(CNSelectedPaymentIndex)).PaymentCollectionUrl

                        If oQuote.InstDepositAmount > 0 AndAlso oPolicySummary IsNot Nothing AndAlso oQuote.DepositTransactasInstalment = False AndAlso PaymentCollectionUrl <> "" _
                            AndAlso Request.QueryString("Mode") = "INSDEPOSIT" Then
                            If Session(CNCashListItem) IsNot Nothing Then
                                DoInstalmentDeposit()
                            Else
                                DoInstalmentDepositVersion2()
                            End If
                        End If

                        oQuote.InsuranceFileRef = oPolicySummary.Reference

                        'MOSS 796
                        If Not Session(CNWMTaskInstanceKey) Is Nothing Then
                            Dim oWorkManager As New NexusProvider.WorkManager

                            oWorkManager.TaskInstanceKey = Session(CNWMTaskInstanceKey)
                            oWebService.GetWmTask(oWorkManager)
                            oWorkManager.Client = oQuote.ClientCode
                            oWorkManager.WmActionType = NexusProvider.WMActionType.Complete
                            oWorkManager.TaskStatusKey = NexusProvider.TaskStatus.Complete
                            oWorkManager.TaskTimeStamp = oWorkManager.TimeStamp
                            oWebService.UpdateWmTask(oWorkManager)
                        End If
                        Dim oParty As NexusProvider.BaseParty = Session(CNParty)
                        Dim oPartyBankDetails As NexusProvider.BankCollection
                        If oParty IsNot Nothing Then
                            'Base on the session value is personal / corporate client is loaded
                            Select Case True
                                Case TypeOf Session(CNParty) Is NexusProvider.PersonalParty
                                    oParty = CType(Session(CNParty), NexusProvider.PersonalParty)
                                Case TypeOf Session(CNParty) Is NexusProvider.CorporateParty
                                    oParty = CType(Session(CNParty), NexusProvider.CorporateParty)
                            End Select
                            'Populate Party bank Details
                            oPartyBankDetails = oWebService.GetPartyBankDetails(oParty.Key)
                            oParty.BankDetails = oPartyBankDetails
                            Session(CNParty) = oParty
                        End If

                        Session.Remove(CNAmountToPay)
                        Session.Remove(CNPayment)
                        Session.Remove(CNOI)
                        Session.Remove(CNMode)
                        Session.Remove(CNPaid)
                        Session.Remove(CNRiskType)
                        Session.Remove(CNCardDetails)
                        Session.Remove(CNPaymentHubDetails)
                        Session(CNIsTransactionConfirmationVisited) = True
                    Catch ex As NexusProvider.NexusException
                        If Session(CNQuoteMode) = QuoteMode.FullQuote AndAlso Session(CNRenewal) Is Nothing AndAlso Session(CNMTAType) Is Nothing Then
                            Dim sQuoteVersioning2 As String = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsQuoteVersioning, NexusProvider.RiskTypeOptions.Code, oQuote.ProductCode, "")
                            If (Not String.IsNullOrEmpty(sQuoteVersioning2) AndAlso sQuoteVersioning2.Trim = "1") Then
                                oQuote.QuoteStatusKey = oQuoteStatus
                                oWebService.UpdateQuoteStatus(oQuote)
                            End If
                        End If

                        If oPolicySummary Is Nothing Then
                            LblOrderID.Text = GetLocalResourceObject("lbl_errorBindQuote").ToString()
                        End If

                        If ex.Errors(0).Code = "1000074" Then   'Code : 1000074 :: Description: ReturnPremiumIsGreaterThanBilledPremium
                            LblOrderID.Text = GetLocalResourceObject("ReturnPremiumIsGreaterThanBilledPremium").ToString()
                            lblMTAPremiumReturn.Text = String.Empty
                        ElseIf ex.Errors(0).Code = "335" Then   'Code :335 :: Description: ReturnPremiumIsGreaterThanBilledPremium
                            LblOrderID.Text = GetLocalResourceObject("lbl_Transaction_text").ToString() & oQuote.InsuranceFileRef
                        Else
                            Throw
                        End If
                    Catch
                        '[start]Changes as per WPR 63 --- to revert the status of live quote to issued if BindQuote Fails
                        If Session(CNQuoteMode) = QuoteMode.FullQuote AndAlso Session(CNRenewal) Is Nothing AndAlso Session(CNMTAType) Is Nothing Then
                            Dim sQuoteVersioning3 As String = oWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsQuoteVersioning, NexusProvider.RiskTypeOptions.Code, oQuote.ProductCode, "")
                            If (Not String.IsNullOrEmpty(sQuoteVersioning3) AndAlso sQuoteVersioning3.Trim = "1") Then
                                oQuote.QuoteStatusKey = oQuoteStatus
                                oWebService.UpdateQuoteStatus(oQuote)
                            End If
                        End If

                        ''For all other cases if error is catched error should appear.
                        If oPolicySummary Is Nothing Then
                            LblOrderID.Text = GetLocalResourceObject("lbl_errorBindQuote").ToString()
                        End If
                        If lblMTAPremiumReturn.Text.Contains("#PolicyNumber") Then
                            lblMTAPremiumReturn.Text = lblMTAPremiumReturn.Text.Replace("#PolicyNumber", oQuote.InsuranceFileRef)
                        End If
                        If (Session(CNPaymentHubDetails) IsNot Nothing) Then
                            'work manager task for payment hub
                            CreateTask(CType(Session(CNQuote), NexusProvider.Quote), "Payment received from the card but error occurred during policy make live process for client " & oQuote.PartyName & " and quote " & oQuote.InsuranceFileRef, "MEMO", "COMMON", GetPaymentHubHandlerTaskGroup())
                        End If



                    Finally
                        oWebService = Nothing
                        oPolicySummary = Nothing
                        Session(CNPaymentHubDetails) = Nothing
                        Session(CNCardDetails) = Nothing
                        Session(CNCashListItem) = Nothing
                    End Try
                End If


            Else
                oPolicySummary = Session.Item(CNPolicy_Summary)
                If oPolicySummary IsNot Nothing Then
                    If Session(CNMTAType) = MTAType.PERMANENT Or Session(CNMTAType) = MTAType.TEMPORARY Then
                        LblOrderID.Text = GetLocalResourceObject("lbl_Transaction_text").ToString() & oPolicySummary.Reference
                        If Session(CNMtaRenSelResponse) IsNot Nothing Then
                            lblMTAReRunRenewal.Text = Session(CNMtaRenSelResponse)
                        End If
                    ElseIf Session(CNMTAType) = MTAType.CANCELLATION Then
                        LblOrderID.Text = GetLocalResourceObject("lbl_CancelTransaction_text").Replace("#PolicyNumber", oPolicySummary.Reference)
                        ' If Session(oPolicySummary.MediaTypeCode) IsNot Nothing Then
                        Select Case UCase(oPolicySummary.MediaTypeCode)
                            Case "CA"
                                LblOrderID.Text += " Cash"
                            Case "CC"
                                LblOrderID.Text += " Credit Card"
                            Case "BD"
                                LblOrderID.Text += " Bankers Draft"
                            Case "DD"
                                LblOrderID.Text += " Direct Debit"
                            Case "CQ"
                                LblOrderID.Text += " Cheque"
                            Case Else
                                LblOrderID.Text += " Cheque"
                        End Select
                        'Else
                        '    LblOrderID.Text += " Cheque"
                        'End If


                    ElseIf Session(CNRenewal) IsNot Nothing Then
                        If paymentOptions.PaymentType(Session(CNSelectedPaymentIndex)).Type.ToString = "PremiumFinance" Then
                            'If payment method is Instalment then need to show instalment plan referance number
                            Dim sConfirmationMessage As String = GetLocalResourceObject("lbl_Transaction_textForInstalment").ToString
                            sConfirmationMessage = sConfirmationMessage.Replace("#PolicyNumber", oPolicySummary.Reference)
                            sConfirmationMessage = sConfirmationMessage.Replace("#InstalmentPlanRef", oPolicySummary.InstalmentPlanRef)
                            LblOrderID.Text = sConfirmationMessage
                        Else
                            LblOrderID.Text = GetLocalResourceObject("lbl_Transaction_text").ToString() & oPolicySummary.Reference
                        End If
                    Else
                        LblOrderID.Text = GetLocalResourceObject("lbl_Transaction_text").ToString() & oPolicySummary.Reference 'oQuote.InsuranceFileRef 'oPolicySummary.Reference
                    End If
                ElseIf ((Session(CNMTAType) = MTAType.PERMANENT OrElse Session(CNMTAType) = MTAType.TEMPORARY OrElse
                         Session(CNMTAType) = MTAType.REINSTATEMENT) And oPolicySummary Is Nothing) Then
                    LblOrderID.Text = GetLocalResourceObject("lbl_CancelMTATransaction_text").ToString()

                ElseIf Session(CNMTAType) = MTAType.CANCELLATION And oPolicySummary Is Nothing Then
                    LblOrderID.Text = GetLocalResourceObject("lbl_CancelMTAQCANTransaction_text").ToString()

                Else
                    If Session(CNPaymentHubDetails) IsNot Nothing AndAlso CType(Session(CNPaymentHubDetails), NexusProvider.PaymentHubDetails).ResultDescription <> PaymentHub.ResultDescription.Authorised Then
                        LblOrderID.Text = GetLocalResourceObject("lbl_errorPayment").ToString()
                    Else
                        LblOrderID.Text = GetLocalResourceObject("lbl_errorBindQuote").ToString()
                    End If

                End If

                Session(CNQuote) = oQuote
            End If

            ''[start]changes for WPR 73_74
            ''If an underwriter (non-agency user) is logged
            Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
            Dim sDesc As String = String.Empty
            Dim sTask As String = String.Empty
            Dim sTaskGroup As String = String.Empty

            If oUserDetails IsNot Nothing AndAlso oUserDetails.Key = 0 AndAlso oQuote.ContactUserName <> "" Then
                If (Session(CNQuoteMode) = QuoteMode.FullQuote AndAlso Session(CNRenewal) Is Nothing AndAlso Session(CNMTAType) <> MTAType.CANCELLATION AndAlso Session(CNMTAType) <> MTAType.REINSTATEMENT) Then 'If NB
                    sTask = "UNDERNB"
                    sTaskGroup = "UNDER"
                    sDesc = IIf(GetLocalResourceObject("lblTaskNB") Is Nothing, "Your Quote with Ref No. XXXXX is Live", GetLocalResourceObject("lblTaskNB"))
                ElseIf (Session(CNQuoteMode) = QuoteMode.MTAQuote) Then 'If MTA
                    sTask = "UNDERMTA"
                    sTaskGroup = "UNDER"
                    sDesc = IIf(GetLocalResourceObject("lblTaskMTA") Is Nothing, "Your Quote with Ref No. XXXXX is Live", GetLocalResourceObject("lblTaskMTA"))
                ElseIf (Session(CNMTAType) = MTAType.REINSTATEMENT) Then 'If MTR
                    sTask = "UNDERREINS"
                    sTaskGroup = "UNDER"
                    sDesc = IIf(GetLocalResourceObject("lblTaskREIN") Is Nothing, "Your Quote with Ref No. XXXXX is reinstated", GetLocalResourceObject("lblTaskREIN"))
                    '[start]New code added against the issue 1411
                ElseIf Session(CNRenewal) IsNot Nothing Then
                    sTask = "RENACCWAM"
                    sTaskGroup = "UWRENEWAL"
                    sDesc = IIf(GetLocalResourceObject("lblTaskRenewalAcceptance") Is Nothing, "Your renewal Quote with Ref No. XXXXX is Accepted", GetLocalResourceObject("lblTaskRenewalAcceptance"))
                    sDesc = sDesc.Replace("XXXXX", oQuote.InsuranceFileRef)
                    '[end]New code added against the issue 1411
                End If
                sDesc = sDesc.Replace("XXXXX", oQuote.InsuranceFileRef)
                ' GoTo gohere
                If (Not (String.IsNullOrEmpty(sDesc) Or String.IsNullOrEmpty(sTask) Or String.IsNullOrEmpty(sTaskGroup))) Then
                    FrameWorkFunctions.CreateTask(oQuote, sDesc, sTask, sTaskGroup)
                End If

                'SendMail()
            End If


            '[end]changes for WPR 73_74
            Session.Remove(CNIsCancelMTA)
            'Redirect to renewal catchup

            Dim autoRenewBDMPolicy As Integer
            Dim isTrueMonthlyPolicy As Integer
            Dim oiWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

            autoRenewBDMPolicy = ConvertToSafeInteger(oiWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.AutoRenewBDMPolicy, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing))

            isTrueMonthlyPolicy = ConvertToSafeInteger(oiWebService.GetProductRiskOptionValue(NexusProvider.ProductConfigActionType.ProductRiskMaintenance, NexusProvider.ProductRiskOptions.IsTrueMonthlyPolicy, NexusProvider.RiskTypeOptions.None, oQuote.ProductCode, Nothing))


            Dim sInsuranceFileTypeCode As String = oQuote.InsuranceFileTypeCode.Trim().ToUpper

            If bIsBackDatedMTA Then
                If autoRenewBDMPolicy = 1 AndAlso isTrueMonthlyPolicy = 1 AndAlso (sInsuranceFileTypeCode = "QUOTE" OrElse sInsuranceFileTypeCode = "MTAQREINS") Then
                    Dim oInsuranceFileDetailsCollection As NexusProvider.InsuranceFileDetailsCollection
                    oInsuranceFileDetailsCollection = Nothing
                    Dim nInsuranceFolderKey As Integer
                    Dim nInsuranceFileKey As Integer
                    Dim oService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider

                    Dim iMaxRowsToFetch As Integer = oPortal.MaxSearchResults

                    oInsuranceFileDetailsCollection = oService.FindPolicy(oQuote.InsuranceFileRef, Nothing, Nothing, NexusProvider.InsuranceFileTypes.POLICY, False, iMaxRowsToFetch, Nothing)
                    If oInsuranceFileDetailsCollection IsNot Nothing Then
                        nInsuranceFileKey = oInsuranceFileDetailsCollection(0).InsuranceFileKey
                        nInsuranceFolderKey = oInsuranceFileDetailsCollection(0).InsuranceFolderKey
                    End If
                    oQuote = oService.GetHeaderAndSummariesByKey(v_iInsuranceFileKey:=nInsuranceFileKey, bExclusiveLock:=True)
                End If
            End If

            If autoRenewBDMPolicy = 1 AndAlso isTrueMonthlyPolicy = 1 AndAlso (sInsuranceFileTypeCode = "QUOTE" OrElse sInsuranceFileTypeCode = "MTAQREINS") AndAlso oQuote.CoverStartDate < DateTime.Now.AddMonths(-1) Then
                Dim sUrl As String
                If oPayment IsNot Nothing Then
                    If HttpContext.Current.Session.IsCookieless Then
                        sUrl = "../Modal/RenewalCatchUp.aspx?InsuranceFileRef=" & oQuote.InsuranceFileRef & "&CoverStartDate=" & oQuote.CoverStartDate & "&CoverEndDate=" & oQuote.CoverEndDate & "&NetPremium=" & oQuote.NetTotal & "&PreferredDate=" & oPayment.PreferredDate & "&modal=true&KeepThis=true&FromPage=ctrlNewQuote&TB_iframe=true&height=400&width=600"
                    Else
                        sUrl = AppSettings("WebRoot") & "/Modal/RenewalCatchUp.aspx?InsuranceFileRef=" & oQuote.InsuranceFileRef & "&CoverStartDate=" & oQuote.CoverStartDate & "&CoverEndDate=" & oQuote.CoverEndDate & "&NetPremium=" & oQuote.NetTotal & "&PreferredDate=" & oPayment.PreferredDate & "&modal=true&KeepThis=true&FromPage=ctrlNewQuote&TB_iframe=true&height=400&width=600"
                    End If
                Else
                    If HttpContext.Current.Session.IsCookieless Then
                        sUrl = "../Modal/RenewalCatchUp.aspx?InsuranceFileRef=" & oQuote.InsuranceFileRef & "&CoverStartDate=" & oQuote.CoverStartDate & "&CoverEndDate=" & oQuote.CoverEndDate & "&NetPremium=" & oQuote.NetTotal & "&modal=true&KeepThis=true&FromPage=ctrlNewQuote&TB_iframe=true&height=400&width=600"
                    Else
                        sUrl = AppSettings("WebRoot") & "/Modal/RenewalCatchUp.aspx?InsuranceFileRef=" & oQuote.InsuranceFileRef & "&CoverStartDate=" & oQuote.CoverStartDate & "&CoverEndDate=" & oQuote.CoverEndDate & "&NetPremium=" & oQuote.NetTotal & "&modal=true&KeepThis=true&FromPage=ctrlNewQuote&TB_iframe=true&height=400&width=600"
                    End If
                End If

                Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show",
                                "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sUrl & "' , null);});</script>")
            End If


        End Sub



        ''' <summary>
        ''' Handles click event to send an email. Creates the XML to pass to the CreateBackgroundJob method
        ''' </summary>
        ''' <remarks></remarks>
        Protected Sub SendMailJob()
            'Set up the defaults
            Dim oEmailDefaults As New EmailDefaults
            If Session(CNIsTransactionConfirmationVisited) = True Then
                'bound policy so use the NewPolicy template
                oEmailDefaults = GetEmailDefaults("NewPolicy")
            Else
                'this is a new quote, so use the NewQuote template
                oEmailDefaults = GetEmailDefaults("NewQuote")
            End If

            'Store various values as they will be required again

            'store various values as we'll need it again
            Dim oQuote As NexusProvider.Quote
            Dim oClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            Dim oUserDetails As NexusProvider.UserDetails = Session(CNAgentDetails)
            Dim oParty As NexusProvider.BaseParty = Session(CNParty)

            If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                oQuote = Session(CNClaimQuote)
            Else
                oQuote = Session(CNQuote)
            End If
            If oQuote IsNot Nothing Then

                _InsuranceFileKey = oQuote.InsuranceFileKey

            End If

            If oClaim IsNot Nothing Then
                _ClaimKey = oClaim.ClaimKey
            End If

            _PartyKey = oParty.Key


            'set up the job to send emails to the requested addresses
            'create an html file on the disk in the temp docs directory, with contents taken from the body textbox
            Dim sFileName As String = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).TempFileLocation & "\" & Guid.NewGuid.ToString & ".html"
            Dim emailBodyFile As New StreamWriter(sFileName)
            'form html formatted string by replacing line breaks with "<br />" and adding basic html tags

            Dim sHtml As String = "<html><body>" & Replace(Convert.ToString(oEmailDefaults.MessageBody), Chr(13) & Chr(10), "<br />") & "</body></html>"
            emailBodyFile.Write(sHtml)
            emailBodyFile.Close()

            Dim xlJob As XElement =
               <BACKGROUND_JOB>
                   <JOB jobtype="DOCUPACK">
                       <PARAMETERS>
                           <PARAMETER name="emailTo" value=<%= oEmailDefaults.EmailTo %>/>
                           <PARAMETER name="emailCc" value=""/>
                           <PARAMETER name="emailSubject" value=<%= oEmailDefaults.Subject %>/>
                           <PARAMETER name="Destination" value="email"/>
                           <PARAMETER name="path" value=<%= sFileName %>/>
                           <PARAMETER name="PartyCnt" value=<%= _PartyKey %>/>
                           <PARAMETER name="ClaimID" value=<%= _ClaimKey %>/>
                           <PARAMETER name="InsuranceFileCnt" value=<%= _InsuranceFileKey %>/>
                       </PARAMETERS>
                       <ITEMS>

                       </ITEMS>
                   </JOB>
               </BACKGROUND_JOB>

            'Select Case Request.QueryString("loc")
            '    Case "docm"
            '        'documents from the document manager control, so we need to look in session for the details
            '        'Dim oDocumentCollection As NexusProvider.DocumentDefaultsCollection = CType(Session(CNCurrentDocumentCollection), NexusProvider.DocumentDefaultsCollection)
            '        Dim oDocumentCollection As NexusProvider.DocumentDefaultsCollection = CType(Cache.Item(Request.QueryString("key")), NexusProvider.DocumentDefaultsCollection)

            '        For Each chkAttachment As ListItem In chklstAttachments.Items
            '            If chkAttachment.Selected Then
            '                Dim xlItem As XElement
            '                Dim iDocID As Integer
            '                Integer.TryParse(chkAttachment.Value, iDocID)
            '                If oDocumentCollection(iDocID).FileLocation IsNot Nothing Then
            '                    'we've got an actual file so add the location as an item
            '                    xlItem = <ITEM path=<%= oDocumentCollection(iDocID).FileLocation %>/>
            '                Else
            '                    'add the document code as an item, it will get generated
            '                    xlItem = <ITEM code=<%= oDocumentCollection(iDocID).documentTemplateCode %>/>
            '                End If
            '                'if we are specifying a document to generate, or a document which has been generated and editted
            '                'we then need to specify a format in which to archive it, either docx or pdf depending on the setting in web.cofig
            '                If Not oDocumentCollection(iDocID).IsUpload Then
            '                    Dim xlParameters As XElement = New XElement("PARAMETERS")
            '                    'get the file type from config
            '                    Dim sFileType As String = CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).DocumentFormat.ToLower()
            '                    'we need to pass in the file name, which may change according to file type (e.g. quote.docx may archive as quote.pdf)
            '                    Dim sOutputFileName As String = oDocumentCollection(iDocID).DocumentName
            '                    sOutputFileName = Left(sOutputFileName, sOutputFileName.LastIndexOf(".")) & "." & sFileType.ToLower
            '                    'add output format and file name params
            '                    Dim xlDocumentFormat As XElement = <PARAMETER name="OutputFormat" value=<%= sFileType.ToUpper %>/>
            '                    xlParameters.Add(xlDocumentFormat)
            '                    Dim xlDestinationFilename As XElement = <PARAMETER name="DestinationFilename" value=<%= sOutputFileName %>/>
            '                    xlParameters.Add(xlDestinationFilename)

            '                    xlItem.Add(xlParameters)
            '                End If

            '                xlJob.Element("JOB").Element("ITEMS").Add(xlItem)
            '            End If
            '        Next
            '    Case "sharep"
            '        Dim oSPFileList As NexusProvider.SharepointFileList = CType(Cache.Item(Request.QueryString("key")), NexusProvider.SharepointFileList)

            '        For Each chkAttachment As ListItem In chklstAttachments.Items
            '            If chkAttachment.Selected Then
            '                'get the document ID from the checkbox, and use this to get the path frmo the item list
            '                Dim iDocID As Integer
            '                Integer.TryParse(chkAttachment.Value, iDocID)
            '                'set up the xml element to add to the job
            '                Dim xlItem As XElement
            '                xlItem = <ITEM path=<%= oSPFileList.ItemList(iDocID).URL %>/>
            '                'if we are specifying a document to generate, or a document which has been generated and editted
            '                'we then need to specify a format in which to archive it, either docx or pdf depending on the setting in web.cofig
            '                Dim xlParameters As XElement = New XElement("PARAMETERS")
            '                Dim xlDocumentFormat As XElement = <PARAMETER name="OutputFormat" value=<%= CType(GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).DocumentFormat.ToUpper() %>/>
            '                xlParameters.Add(xlDocumentFormat)

            '                xlItem.Add(xlParameters)

            '                xlJob.Element("JOB").Element("ITEMS").Add(xlItem)
            '            End If
            '        Next
            'End Select

            Dim strJob As String = xlJob.ToString 'this will be used as input to the SAM call
            Dim sDescription As String = "Email documents"
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            'call SAM to queue the docs for Archiving
            Dim iBackgroundJobID As Integer = oWebService.CreateBackgroundJob(sDescription, strJob, Now.Date)
            'Close the modal when the page reloads
            'ClientScript.RegisterStartupScript(GetType(String), "closeModal", "self.parent.hide_modal();", True)
        End Sub

        ''' <summary>
        ''' Gets the defaults for the email message and returns them as an email defaults object
        ''' </summary>
        ''' <param name="sTemplateCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetEmailDefaults(ByVal sTemplateCode As String) As EmailDefaults
            Dim oEmailDefaults As New EmailDefaults 'object to store properties which must be returned
            With oEmailDefaults
                'Get the "email to" first
                Dim oEmailTemplate As Config.EmailTemplate = CType(ConfigurationManager.GetSection("NexusFrameWork"), Config.NexusFrameWork).Portals.Portal(Portal.GetPortalID()).EmailTemplates.EmailTemplate(sTemplateCode)
                'create an array by splitting on comma
                Dim sTemp() As String = oEmailTemplate.Recipient.Split(",")
                'loop through the array, do the merge on each value to replace with the final value
                'For iCounter As Integer = 0 To sTemp.Length - 1
                '    If sTemp(iCounter).Contains("RiskData") Then
                '        'we will have a path specified in the risk, so we need to extract the data from there

                '        'strip out everything other than the path
                '        sTemp(iCounter) = sTemp(iCounter).Replace("[", "")
                '        sTemp(iCounter) = sTemp(iCounter).Replace("]", "")
                '        sTemp(iCounter) = sTemp(iCounter).Replace("!", "")
                '        sTemp(iCounter) = sTemp(iCounter).Replace("(", "")
                '        sTemp(iCounter) = sTemp(iCounter).Replace(")", "")
                '        sTemp(iCounter) = sTemp(iCounter).Replace("RiskData", "")
                '        sTemp(iCounter) = sTemp(iCounter).Replace("'", "")

                '        .EmailTo = FindInRiskData(sTemp(iCounter))
                '    Else
                '        'Select Case sTemp(iCounter)
                '        '    Case "[!LoggedInUserEmail!]"
                '        '        'we need to replace the value with the logged in users email address
                '        '            If Not String.IsNullOrEmpty(CType(Session(CNAgentDetails), NexusProvider.UserDetails).EmailAddress) Then
                '        '                .EmailTo = CType(Session(CNAgentDetails), NexusProvider.UserDetails).EmailAddress & ","
                '        '            End If
                '        'End Select

                '        'We


                '    End If
                'Next

                If Session(CNQuote) IsNot Nothing Then

                    .EmailTo = CType(Session(CNQuote), NexusProvider.Quote).EmailAddress
                End If

                If Right(.EmailTo, 1) = "," Then
                    'remove the trailing comma
                    .EmailTo = Left(.EmailTo, Len(.EmailTo) - 1)
                End If

                'Get the message body
                Dim sbTemplate As New StringBuilder

                'open the template from the given path
                Dim srTmp As New StreamReader(File.OpenRead(Server.MapPath(oEmailTemplate.Path)))
                sbTemplate = New StringBuilder(srTmp.ReadToEnd())
                srTmp.Close()
                .MessageBody = sbTemplate.ToString

                'replace the merge values with real ones

                'do the straight forward replacements
                Dim sInsuredName As String = String.Empty
                Dim sPolicyHeader_CoverStartDate As String = String.Empty
                Dim sPolicyHeader_CoverEndDate As String = String.Empty

                If Session(CNQuote) IsNot Nothing Then
                    'get the insured name from the quote object
                    sInsuredName = CType(Session(CNQuote), NexusProvider.Quote).InsuredName
                    sPolicyHeader_CoverStartDate = CType(Session(CNQuote), NexusProvider.Quote).CoverStartDate
                    sPolicyHeader_CoverEndDate = CType(Session(CNQuote), NexusProvider.Quote).CoverEndDate
                End If
                .MessageBody = .MessageBody.Replace("[!InsuredName!]", sInsuredName)
                .MessageBody = .MessageBody.Replace("[!PolicyHeader_CoverStartDate!]", sPolicyHeader_CoverStartDate)
                .MessageBody = .MessageBody.Replace("[!PolicyHeader_CoverEndDate!]", sPolicyHeader_CoverEndDate)

                Dim sLoggedInUserFullName As String = String.Empty
                If Session(CNAgentDetails) IsNot Nothing Then
                    'get the logged in user name from session
                    sLoggedInUserFullName = CType(Session(CNAgentDetails), NexusProvider.UserDetails).ResolvedName
                End If
                .MessageBody = .MessageBody.Replace("[!LoggedInUserFullName!]", sLoggedInUserFullName)

                'find instances of riskdata in the string builder and replace by searching the risk using specified xpath query
                'split the string to get each risk data part
                sTemp = .MessageBody.Split("[")
                'Clear the messagebody property then we rebuild it
                .MessageBody = String.Empty
                'loop through the array
                For Each sPart As String In sTemp
                    'take off the start of the merge string ("[!RiskData('")
                    'don't need this? sPart = Right(sPart, Len(sPart) - 12)
                    'extract the query. This will be the bit before the first " ')!]"
                    If Left(sPart, 11) = "!RiskData('" Then
                        'we need to do a query against the risk
                        Dim sQuery As String = Left(sPart, sPart.IndexOf("')!]"))
                        sQuery = Right(sQuery, Len(sQuery) - 11)
                        'put it all back together by adding the merged text, then the rest of the string after "')!]"
                        .MessageBody += FindInRiskData(sQuery) & Right(sPart, Len(sPart) - sPart.IndexOf("')!]") - 4)
                    Else
                        'just put the string back together, there shouldn't be any merge fields left in it
                        .MessageBody += sPart
                    End If
                Next

                'Get the subject
                .Subject = Replace(oEmailTemplate.Subject, "[!InsuredName!]", sInsuredName)
            End With

            Return oEmailDefaults
        End Function



        Private Function FindInRiskData(ByVal sQuery As String) As String
            Dim sReturn As String = String.Empty
            'loop through the risks and try to find a value for the broker email
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            If oQuote IsNot Nothing Then
                For iCount As Integer = 0 To oQuote.Risks.Count - 1
                    If oQuote.Risks(iCount).XMLDataset IsNot Nothing Then
                        Using strDataset As New System.IO.StringReader(oQuote.Risks(iCount).XMLDataset)
                            Dim NavString As String = "DATA_SET/RISK_OBJECTS/" & Session(CNDataModelCode) & "_POLICY_BINDER/" & sQuery
                            Dim Navigator As System.Xml.XPath.XPathNavigator
                            Using trDataset As New System.Xml.XmlTextReader(strDataset)
                                Dim Doc As System.Xml.XPath.XPathDocument = New System.Xml.XPath.XPathDocument(trDataset)
                                Navigator = Doc.CreateNavigator()
                                Dim NodeI As System.Xml.XPath.XPathNodeIterator = Navigator.Select(NavString)
                                While NodeI.MoveNext
                                    'if we've got a value then add this to the email string and get out
                                    If Not String.IsNullOrEmpty(NodeI.Current.Value) Then
                                        sReturn += NodeI.Current.Value
                                        Exit For
                                    End If
                                End While
                            End Using
                        End Using
                    End If
                Next
            End If
            Return sReturn
        End Function

        ''' <summary>
        ''' Holds defaults for the email to be sent
        ''' </summary>
        ''' <remarks></remarks>
        Private Class EmailDefaults
            Private _Subject As String
            Private _MessageBody As String
            Private _EmailTo As String

            Public Property Subject() As String
                Get
                    Return _Subject
                End Get
                Set(ByVal value As String)
                    _Subject = value
                End Set
            End Property

            Public Property MessageBody() As String
                Get
                    Return _MessageBody
                End Get
                Set(ByVal value As String)
                    _MessageBody = value
                End Set
            End Property

            Public Property EmailTo() As String
                Get
                    Return _EmailTo
                End Get
                Set(ByVal value As String)
                    _EmailTo = value
                End Set
            End Property
        End Class

        Protected Sub SendMail()
            Dim sURL As String
            Dim oParty As NexusProvider.BaseParty = Session(CNParty)
            Dim oQuote As NexusProvider.Quote
            Dim oClaim As NexusProvider.ClaimOpen = CType(Session.Item(CNClaim), NexusProvider.ClaimOpen)
            If Session(CNMode) = Mode.NewClaim Or Session(CNMode) = Mode.EditClaim Or Session(CNMode) = Mode.PayClaim Or Session(CNMode) = Mode.SalvageClaim Or Session(CNMode) = Mode.TPRecovery Then
                oQuote = Session(CNClaimQuote)
            Else
                oQuote = Session(CNQuote)
            End If
            If HttpContext.Current.Session.IsCookieless Then
                sURL = AppSettings("WebRoot") & "(S(" & Session.SessionID.ToString() + "))" & "/Modal/SendEmail.aspx?PartyKey=" & oParty.Key & "&key=" & "&InsuranceFileKey=" & oQuote.InsuranceFileKey & "&modal=true&loc=docm&n=p&Riskcheck=true&KeepThis=true&TB_iframe=true&height=300&width=750"
            Else
                sURL = AppSettings("WebRoot") & "/Modal/SendEmail.aspx?PartyKey=" & oParty.Key & "&key=" & "&InsuranceFileKey=" & oQuote.InsuranceFileKey & "&modal=true&loc=docm&n=p&Riskcheck=true&KeepThis=true&TB_iframe=true&height=300&width=750"
            End If
            Page.ClientScript.RegisterClientScriptBlock(Me.GetType(), "tb_show",
            "<script language=""JavaScript"" type=""text/javascript"">$(document).ready(function(){tb_show( null,'" & sURL & "' , null);});</script>")
            Exit Sub
        End Sub

        Function GetMasterPlaceHolder(ByVal v_oPage As Page,
                                   ByVal v_sPlaceHolderName As String) As ContentPlaceHolder

            'Assumes all pages have at least one masterpage
            Dim oTmp As Object = v_oPage.Master
            Dim oMaster As MasterPage = Nothing

            'Go down to the lowest nested masterpage
            Do
                oMaster = oTmp
                oTmp = oMaster.Master
            Loop Until oTmp Is Nothing

            'Return the request placeholder in the masterpage
            Return oMaster.FindControl(v_sPlaceHolderName)

        End Function
        'below method silent create cashlist for the credit card deposit amount
        Protected Sub DoInstalmentDeposit()
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oReceiptCashListItem As New NexusProvider.ReceiptCashListItemType
            Dim oCashListItem As New NexusProvider.PaymentItems
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim sBranchCode As String = oQuote.BranchCode
            Dim oPolicySummary As NexusProvider.PolicySummary = CType(Session.Item(CNPolicy_Summary), NexusProvider.PolicySummary)
            Dim iMediaTypeId As Integer
            Dim sMediaTypeCode As String
            Dim iBankAccountId As Integer
            Dim sBankAccountCode As String
            Dim oBaseParty As New NexusProvider.BaseParty
            oBaseParty = oWebservice.GetParty(oQuote.PartyKey)
            oWebservice = New NexusProvider.ProviderManager().Provider
            Dim oAccountSearchCr As New NexusProvider.AccountSearchCriteria
            Dim oAccountColl As NexusProvider.AccountSearchResultCollection
            'Dim oPayment As NexusProvider.Payment
            If oQuote.BusinessTypeCode = "DIRECT" Then 'Direct Business/Customer
                oAccountSearchCr.ShortCode = oQuote.ClientCode
            Else
                oAccountSearchCr.ShortCode = oQuote.AgentCode
            End If
            Try


                oAccountColl = oWebservice.FindAccounts(oAccountSearchCr)
                GetBankAccountDefault(iMediaTypeId, iBankAccountId, 2)

                If iBankAccountId > 0 Then
                    sMediaTypeCode = GetCodeForKey(NexusProvider.ListType.PMLookup, iMediaTypeId, "MediaType", True)
                    sBankAccountCode = GetCodeForKey(NexusProvider.ListType.PMLookup, iBankAccountId, "BankAccount", True)
                Else
                    Exit Sub
                End If


                Dim oReceiptCashListItems As NexusProvider.ReceiptCashListItemType = CType(Session(CNCashListItem), NexusProvider.ReceiptCashListItemType)

                ''change value based on data received from session
                With oReceiptCashListItem.CoreCashList
                    .BankAccountCode = sBankAccountCode
                    .CurrencyCode = oQuote.CurrencyCode
                    .ListDate = Today.Date
                    .TypeCode = "R"
                    .StatusCode = "E"
                End With

                oCashListItem.AccountShortCode = oReceiptCashListItems.ReceiptItems(0).AccountShortCode
                oCashListItem.Amount = oReceiptCashListItems.ReceiptItems(0).Amount
                oCashListItem.StatusCode = "ADD"
                oCashListItem.AllocationStatusCode = oReceiptCashListItems.ReceiptItems(0).AllocationStatusCode
                oCashListItem.Address = oReceiptCashListItems.ReceiptItems(0).Address
                oCashListItem.OurReference = oReceiptCashListItems.ReceiptItems(0).OurReference
                oCashListItem.TheirReference = oReceiptCashListItems.ReceiptItems(0).TheirReference
                oCashListItem.BankReference = oReceiptCashListItems.ReceiptItems(0).BankReference
                oCashListItem.TransactionDate = oReceiptCashListItems.ReceiptItems(0).TransactionDate
                oCashListItem.MediaTypeCode = oReceiptCashListItems.ReceiptItems(0).MediaTypeCode
                If String.IsNullOrEmpty(oReceiptCashListItems.ReceiptItems(0).MediaReference) Then
                    oCashListItem.MediaReference = oPolicySummary.Reference + "INSDEPOSIT"
                Else
                    oCashListItem.MediaReference = oReceiptCashListItems.ReceiptItems(0).MediaReference
                End If

                oCashListItem.TypeCode = oReceiptCashListItems.ReceiptItems(0).TypeCode
                Dim oPayment As NexusProvider.Payment = CType(Session(CNPayment), NexusProvider.Payment)

                If oReceiptCashListItems.ReceiptItems(0).MediaTypeCode.Trim() = "CC" AndAlso oReceiptCashListItems.ReceiptItems(0).CreditCard IsNot Nothing Then
                    oCashListItem.CreditCard = oReceiptCashListItems.ReceiptItems(0).CreditCard
                ElseIf oReceiptCashListItems.ReceiptItems(0).MediaTypeCode.Trim() = "CC" AndAlso oPayment.CreditCard IsNot Nothing Then
                    Dim oCreditCard As New NexusProvider.CreditCard
                    oCreditCard.Number = oPayment.CreditCard.Number
                    oCreditCard.AuthCode = oPayment.CreditCard.AuthCode
                    oCreditCard.ExpiryDate = oPayment.CreditCard.ExpiryDate
                    oCreditCard.TypeCode = oPayment.CreditCard.TypeCode
                    oCreditCard.NameOnCreditCard = oPayment.CreditCard.NameOnCreditCard
                    oCreditCard.TransactionCode = oPayment.CreditCard.TransactionCode
                    If oPayment.CreditCard.CreditCardType_CardHolder IsNot Nothing Then
                        oCreditCard.Address1 = oPayment.CreditCard.CreditCardType_CardHolder.Address1
                        oCreditCard.Address2 = oPayment.CreditCard.CreditCardType_CardHolder.Address2
                        oCreditCard.Address3 = oPayment.CreditCard.CreditCardType_CardHolder.Address3
                        oCreditCard.Address4 = oPayment.CreditCard.CreditCardType_CardHolder.Address4
                        oCreditCard.Address5 = oPayment.CreditCard.CreditCardType_CardHolder.Address5
                        oCreditCard.Address6 = oPayment.CreditCard.CreditCardType_CardHolder.Address6
                        oCreditCard.Address7 = oPayment.CreditCard.CreditCardType_CardHolder.Address7
                        oCreditCard.Address8 = oPayment.CreditCard.CreditCardType_CardHolder.Address8
                        oCreditCard.Address9 = oPayment.CreditCard.CreditCardType_CardHolder.Address9
                        oCreditCard.Address10 = oPayment.CreditCard.CreditCardType_CardHolder.Address10
                        oCreditCard.AddressType = oPayment.CreditCard.CreditCardType_CardHolder.AddressType
                        oCreditCard.PostCode = oPayment.CreditCard.CreditCardType_CardHolder.PostCode
                        oCreditCard.StateCode = oPayment.CreditCard.CreditCardType_CardHolder.StateCode
                        oCreditCard.CountryCode = oPayment.CreditCard.CreditCardType_CardHolder.CountryCode
                    End If
                    oCashListItem.CreditCard = oCreditCard
                End If
                oReceiptCashListItem.TransactionDate = oReceiptCashListItems.ReceiptItems(0).TransactionDate
                oReceiptCashListItem.ReceiptItems.Add(oCashListItem)
                Dim oReceiptCashListCollection As New NexusProvider.ReceiptCashListCollection
                Try
                    oReceiptCashListCollection = oWebservice.CreateReceiptcashListWithItem(oReceiptCashListItem)

                    Dim oTransAllocationDetails As New NexusProvider.AllocationDetails
                    Dim oAllocation As NexusProvider.Allocation
                    Dim oAccountDetails As New NexusProvider.AccountDetails
                    Dim oAccountDetailsDefaults As New NexusProvider.AccountDetailsDefaults
                    Dim oAllocationDetailsCollections As New NexusProvider.AllocationDetailsCollections
                    Dim oAllocationDetails As New NexusProvider.AllocationDetails
                    Dim oTrasactionDetails As New NexusProvider.AllocationDetailsCollections
                    Dim sJN_Number, sSPR_Number As String

                    'Assignment of the Transdetails Key
                    oAllocationDetails = New NexusProvider.AllocationDetails
                    oAllocationDetails.TransdetailKey = oPolicySummary.InstdepositTransDetailsId
                    oAllocationDetailsCollections.Add(oAllocationDetails)
                    oAllocationDetails = Nothing
                    oAllocationDetails = New NexusProvider.AllocationDetails

                    oAllocationDetails.TransdetailKey = oReceiptCashListCollection(0).TransDetailKey
                    oAllocationDetailsCollections.Add(oAllocationDetails)
                    oTrasactionDetails = oWebservice.GetTransactionDetails(oAccountColl(0).AccountKey, oAllocationDetailsCollections)

                    For Each oTempAllocationDetails As NexusProvider.AllocationDetails In oTrasactionDetails
                        If oTempAllocationDetails.TransdetailKey = oPolicySummary.InstdepositTransDetailsId Then
                            oAllocation = New NexusProvider.Allocation
                            oAllocation.AllocationAmount = oTempAllocationDetails.Amount
                            oAllocation.AllocationTimeStamp = oTempAllocationDetails.AllocationTimeStamp

                            oAllocation.AllocationTransdetailKey = oPolicySummary.InstdepositTransDetailsId
                            oTransAllocationDetails.Allocation.Add(oAllocation)
                            oAllocation = Nothing
                            sJN_Number = oTempAllocationDetails.DocRef
                        ElseIf oTempAllocationDetails.TransdetailKey = oReceiptCashListCollection(0).TransDetailKey Then
                            sSPR_Number = oTempAllocationDetails.DocRef 'SRP
                        End If
                    Next

                    oTransAllocationDetails.AccountKey = oAccountColl(0).AccountKey
                    oTransAllocationDetails.CashListItemKey = oReceiptCashListCollection(0).CashListItemKey
                    oTransAllocationDetails.Amount = -oQuote.InstDepositAmount
                    oTransAllocationDetails.TransdetailKey = oReceiptCashListCollection(0).TransDetailKey
                    oTransAllocationDetails.Currency = oQuote.CurrencyCode
                    Try
                        Dim bIsUpdated As Boolean = oWebservice.UpdateAllocation(oTransAllocationDetails)
                    Catch
                        CreateTask(CType(Session(CNQuote), NexusProvider.Quote), Session("NABcrn").ToString() _
                                & "- Receipt has been generated but Allocation of Receipt " & sSPR_Number & " has failed against Journal " & sJN_Number & ", needs to be done manually", "FINDTXN", "SLACS", GetAccountHandlerTaskGroup())

                    End Try

                Catch
                    CreateTask(CType(Session(CNQuote), NexusProvider.Quote), oPolicySummary.Reference _
                            & " - Policy has been made live but CashList Receipt has failed, needs to be generated manually", "ACTRCTV2", "SLACS", GetAccountHandlerTaskGroup())
                    oReceiptCashListCollection = Nothing
                End Try
            Catch
            Finally
                oWebservice = Nothing
                oPolicySummary = Nothing
                oQuote = Nothing

            End Try
        End Sub


        'FUNCTION :ConvertToSafeInteger
        'This function changes any string to Integer without exception
        'If string is blank then we assume integer value is 0
        'If string has a floating point value we take its integer part if it is there.
        Private Function ConvertToSafeInteger(ByVal sValue As String) As Integer
            Dim rIntegerValue As Integer = 0
            If IsNumeric(sValue) Then
                If sValue.IndexOf(".") > 0 Then
                    sValue = sValue.Remove(sValue.IndexOf("."))
                ElseIf sValue.IndexOf(".") = 0 Then
                    sValue = "0"
                End If
                rIntegerValue = Convert.ToInt32(sValue)
            End If
            Return rIntegerValue
        End Function

        Private Function GetAccountHandlerTaskGroup() As String
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim sReturnCode As NexusProvider.OptionTypeSetting
            Dim sTaskGroup As String = ""
            Try
                sReturnCode = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5042)
                sTaskGroup = sReturnCode.OptionValue
            Catch ex As System.Exception
                Throw
            Finally
                sReturnCode = Nothing
                oWebService = Nothing
            End Try
            Return sTaskGroup.Trim()
        End Function

        Private Function GetPaymentHubHandlerTaskGroup() As String
            Dim oWebService As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim sReturnCode As NexusProvider.OptionTypeSetting
            Dim sTaskGroup As String = ""
            Try
                sReturnCode = oWebService.GetOptionSetting(NexusProvider.OptionType.SystemOption, 5202)
                If sReturnCode.OptionValue = String.Empty Then
                    sReturnCode.OptionValue = 0
                End If
                sTaskGroup = GetCodeForKey(NexusProvider.ListType.PMLookup, Convert.ToInt32(sReturnCode.OptionValue), "PMUser_Group", True)
            Catch ex As System.Exception
                Throw
            Finally
                sReturnCode = Nothing
                oWebService = Nothing
            End Try
            Return sTaskGroup.Trim()
        End Function


        Protected Sub DoInstalmentDepositVersion2()
            Dim oWebservice As NexusProvider.ProviderBase = New NexusProvider.ProviderManager().Provider
            Dim oReceiptCashListItem As New NexusProvider.ReceiptCashListItemType
            Dim oCashListItem As New NexusProvider.PaymentItems
            Dim oQuote As NexusProvider.Quote = Session(CNQuote)
            Dim sBranchCode As String = oQuote.BranchCode
            Dim oPolicySummary As NexusProvider.PolicySummary = CType(Session.Item(CNPolicy_Summary), NexusProvider.PolicySummary)
            Dim iMediaTypeId As Integer
            Dim sMediaTypeCode As String
            Dim iBankAccountId As Integer
            Dim sBankAccountCode As String
            Dim oBaseParty As New NexusProvider.BaseParty
            oBaseParty = oWebservice.GetParty(oQuote.PartyKey)
            oWebservice = New NexusProvider.ProviderManager().Provider
            Dim oAccountSearchCr As New NexusProvider.AccountSearchCriteria
            Dim oAccountColl As NexusProvider.AccountSearchResultCollection
            'Dim oPayment As NexusProvider.Payment
            If oQuote.BusinessTypeCode = "DIRECT" Then 'Direct Business/Customer
                oAccountSearchCr.ShortCode = oQuote.ClientCode
            Else
                oAccountSearchCr.ShortCode = oQuote.AgentCode
            End If
            Try

                oAccountColl = oWebservice.FindAccounts(oAccountSearchCr)
                GetBankAccountDefault(iMediaTypeId, iBankAccountId, 2)

                If iBankAccountId > 0 Then
                    sMediaTypeCode = GetCodeForKey(NexusProvider.ListType.PMLookup, iMediaTypeId, "MediaType", True)
                    sBankAccountCode = GetCodeForKey(NexusProvider.ListType.PMLookup, iBankAccountId, "BankAccount", True)
                Else
                    'sFailureReason = "Bank"
                    Exit Sub
                End If

                With oReceiptCashListItem.CoreCashList
                    .BankAccountCode = sBankAccountCode
                    .CurrencyCode = oQuote.CurrencyCode
                    .ListDate = Today.Date
                    .TypeCode = "R"
                    .StatusCode = "E"
                End With

                oCashListItem.AccountShortCode = oQuote.ClientCode
                oCashListItem.Amount = oQuote.InstDepositAmount
                oCashListItem.StatusCode = "ADD"
                oCashListItem.AllocationStatusCode = "U"
                oCashListItem.AccountShortCode = oAccountColl(0).ShortCode
                oCashListItem.Address = oBaseParty.Addresses(0)
                oCashListItem.OurReference = ""
                oCashListItem.TransactionDate = Today.Date
                oCashListItem.MediaTypeCode = sMediaTypeCode
                oCashListItem.TypeCode = "INSTDEPT"
                Dim oPayment As NexusProvider.Payment = CType(Session(CNPayment), NexusProvider.Payment)

                If sMediaTypeCode.Trim() = "CC" AndAlso oPayment.CreditCard IsNot Nothing Then
                    Dim oCreditCard As New NexusProvider.CreditCard
                    oCreditCard.Number = oPayment.CreditCard.Number
                    oCreditCard.AuthCode = oPayment.CreditCard.AuthCode
                    oCashListItem.CreditCard = oCreditCard
                End If
                oReceiptCashListItem.TransactionDate = Today.Date
                oReceiptCashListItem.ReceiptItems.Add(oCashListItem)
                Dim oReceiptCashListCollection As New NexusProvider.ReceiptCashListCollection
                Try
                    oReceiptCashListCollection = oWebservice.CreateReceiptcashListWithItem(oReceiptCashListItem)

                    Dim oTransAllocationDetails As New NexusProvider.AllocationDetails
                    Dim oAllocation As NexusProvider.Allocation
                    Dim oAccountDetails As New NexusProvider.AccountDetails
                    Dim oAccountDetailsDefaults As New NexusProvider.AccountDetailsDefaults
                    Dim oAllocationDetailsCollections As New NexusProvider.AllocationDetailsCollections
                    Dim oAllocationDetails As New NexusProvider.AllocationDetails
                    Dim oTrasactionDetails As New NexusProvider.AllocationDetailsCollections
                    Dim sJN_Number, sSPR_Number As String

                    'Assignment of the Transdetails Key
                    oAllocationDetails = New NexusProvider.AllocationDetails
                    oAllocationDetails.TransdetailKey = oPolicySummary.InstdepositTransDetailsId
                    oAllocationDetailsCollections.Add(oAllocationDetails)
                    oAllocationDetails = Nothing
                    oAllocationDetails = New NexusProvider.AllocationDetails

                    oAllocationDetails.TransdetailKey = oReceiptCashListCollection(0).TransDetailKey
                    oAllocationDetailsCollections.Add(oAllocationDetails)
                    oTrasactionDetails = oWebservice.GetTransactionDetails(oAccountColl(0).AccountKey, oAllocationDetailsCollections)

                    For Each oTempAllocationDetails As NexusProvider.AllocationDetails In oTrasactionDetails
                        If oTempAllocationDetails.TransdetailKey = oPolicySummary.InstdepositTransDetailsId Then
                            oAllocation = New NexusProvider.Allocation
                            oAllocation.AllocationAmount = oTempAllocationDetails.Amount
                            oAllocation.AllocationTimeStamp = oTempAllocationDetails.AllocationTimeStamp

                            oAllocation.AllocationTransdetailKey = oPolicySummary.InstdepositTransDetailsId
                            oTransAllocationDetails.Allocation.Add(oAllocation)
                            oAllocation = Nothing
                            sJN_Number = oTempAllocationDetails.DocRef
                        ElseIf oTempAllocationDetails.TransdetailKey = oReceiptCashListCollection(0).TransDetailKey Then
                            sSPR_Number = oTempAllocationDetails.DocRef 'SRP
                        End If
                    Next

                    oTransAllocationDetails.AccountKey = oAccountColl(0).AccountKey
                    oTransAllocationDetails.CashListItemKey = oReceiptCashListCollection(0).CashListItemKey
                    oTransAllocationDetails.Amount = -oQuote.InstDepositAmount
                    oTransAllocationDetails.TransdetailKey = oReceiptCashListCollection(0).TransDetailKey
                    oTransAllocationDetails.Currency = oQuote.CurrencyCode
                    Try
                        Dim bIsUpdated As Boolean = oWebservice.UpdateAllocation(oTransAllocationDetails)
                    Catch
                        CreateTask(CType(Session(CNQuote), NexusProvider.Quote), Session("NABcrn").ToString() _
                                & "- Receipt has been generated but Allocation of Receipt " & sSPR_Number & "has failed against Journal " & sJN_Number & ", needs to be done manually", "FINDTXN", "SLACS", GetAccountHandlerTaskGroup())

                    End Try

                Catch
                    CreateTask(CType(Session(CNQuote), NexusProvider.Quote), oPolicySummary.Reference _
                            & " - Policy has been made live but CashList Receipt has failed, needs to be generated manually", "ACTRCTV2", "SLACS", GetAccountHandlerTaskGroup())
                    oReceiptCashListCollection = Nothing
                End Try
            Catch
            Finally
                oWebservice = Nothing
                oPolicySummary = Nothing
                oQuote = Nothing

            End Try
        End Sub
    End Class

End Namespace
