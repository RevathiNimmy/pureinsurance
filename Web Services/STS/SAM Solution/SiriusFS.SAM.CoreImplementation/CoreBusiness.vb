Option Strict On
Option Explicit On

Imports Microsoft.ApplicationBlocks.Data
Imports Microsoft.ApplicationBlocks.ExceptionManagement
Imports SiriusFS.SAM.Structure
Imports SiriusFS.SAM.Structure.Core
Imports SiriusFS.SAM.Structure.BaseImplementationTypes
Imports SiriusFS.SAM.Structure.SAMConstants
Imports dPMDAOBridge
Imports System.Security.Cryptography
Imports SSP.Shared.gPMConstants
Imports SSP.Shared
Public Class CoreBusiness

    Protected Const ACDefaultGISListDMC As String = "GIIM"

    Private TraceLevelSwitch As TraceSwitch = New TraceSwitch("TraceLevelSwitch", "TraceLevelSwitch")
    Private _SiriusUser As SIRIUSUSER
    Private _oCache As System.Web.Caching.Cache
    Public Enum LockName As Long
        InsuranceFolderCnt = 1
        InsuranceFileCnt = 2
        PartyCnt = 3
        ClaimId = 4
        TaskInstanceCnt = 5
        UserGroupCnt = 6
        TaskGroupCnt = 7
        CoverNoteBookId = 8
        BGId = 9
        RenewalProcess = 10
        TransDetailKey = 11
        CashListItemID = 12
        ClaimPaymentCnt = 13
        RiskKey = 14
        PartyBankKey = 15
        CashDepositKey = 15
        NextCashDepositNumber = 16
        ClaimPayment = 17
    End Enum

    Public ReadOnly Property LockNameString(ByVal LockName As LockName) As String
        Get
            Select Case LockName
                Case LockName.InsuranceFileCnt
                    Return "insurance_file_cnt"
                Case LockName.InsuranceFolderCnt
                    Return "insurance_folder_cnt"
                Case LockName.PartyCnt
                    Return "party_cnt"
                Case CoreBusiness.LockName.ClaimId
                    Return "claim_id"
                Case CoreBusiness.LockName.TaskInstanceCnt
                    Return "pmwrk_task_instance_cnt"

                Case CoreBusiness.LockName.UserGroupCnt
                    Return "pmuser_group_id"

                Case CoreBusiness.LockName.TaskGroupCnt
                    Return "pmwrk_task_group_id"

                Case CoreBusiness.LockName.CoverNoteBookId
                    Return "cover_note_book_id"

                Case LockName.BGId
                    Return "bg_id"

                Case CoreBusiness.LockName.RenewalProcess
                    Return "renewal_status_cnt"

                Case LockName.TransDetailKey
                    Return "transdetail_id"

                Case LockName.CashListItemID
                    Return "cashlistitem_id"

                Case CoreBusiness.LockName.ClaimPaymentCnt
                    Return "claim_payment_id"

                Case CoreBusiness.LockName.RiskKey
                    Return "risk_cnt"

                Case CoreBusiness.LockName.CashDepositKey
                    Return "cashdeposit_id"
                Case CoreBusiness.LockName.ClaimPayment
                    Return "claim_payment"
                Case Else
                    Return String.Empty
            End Select
        End Get
    End Property

    Public Enum BGStatus
        Active = 1
        Issued = 2
        Invoked = 3
        Expired = 4
        Deleted = 5
    End Enum

    Public Enum ProductRiskOptions

        '''Remarks
        CaptionId = 0

        '''Remarks
        Code = 1

        '''Remarks
        Description = 2

        '''Remarks
        ProductEffectiveDate = 3

        '''Remarks
        IsDeleted = 4

        '''Remarks
        SchemeAgencyRef = 5

        '''Remarks
        BlockNo = 6

        '''Remarks
        IsTaxSuppressed = 7

        '''Remarks
        QuoteAutoNumberingID = 8

        '''Remarks
        IsShortPeriodRated = 9

        '''Remarks
        IsMidnightRenewal = 10

        '''Remarks
        IsAutoRenewable = 11

        '''Remarks
        RenewalWeeks = 12

        '''Remarks
        PolicyAutoNumberingID = 13

        '''Remarks
        ProvClaimAutoNumberingID = 14

        '''Remarks
        FullClaimAutoNumberingID = 15

        '''Remarks
        Accumulation = 16

        '''Remarks
        RIPointer = 17

        '''Remarks
        ReportPointer = 18

        '''Remarks
        ClaimYearToCheck = 19

        '''Remarks
        MaxSingleClaimValue = 20

        '''Remarks
        MaxNumberOfClaim = 21

        '''Remarks
        MaxTotalClaimValue = 22

        '''Remarks
        NBProrata = 23

        '''Remarks
        MTAProrata = 24

        '''Remarks
        RoundPremium = 25

        '''Remarks
        RoundingSection = 26

        '''Remarks
        PolicyNumberAtQuote = 27

        '''Remarks
        FollowUpTimeFrame = 28

        '''Remarks
        GracePeriod = 29

        '''Remarks
        PreventCancelledAgents = 30

        '''Remarks
        AllowPositiveValues = 31

        '''Remarks
        MediaTypeMandatory = 32

        '''Remarks
        PolicyStyleID = 33

        '''Remarks
        PolicyStyleMandatory = 34

        '''Remarks
        CurrencyChange = 35

        '''Remarks
        LossCurrencyChange = 36

        '''Remarks
        ChangePolicyNumberAtRenewal = 37

        '''Remarks
        AllowStandardWordingEdit = 38

        '''Remarks
        HideSummaryAtRenewalAcceptance = 39

        '''Remarks
        ProductSuppressClaimTransactionsReserves = 40

        '''Remarks
        ProductSuppressClaimTransactionsPayments = 41

        '''Remarks
        ProductSuppressClaimTransactionsRecoveries = 42

        '''Remarks
        CanMakeLiveInvoice = 43

        '''Remarks
        CanMakeLiveInstalments = 44

        '''Remarks
        CanMakeLivePaynow = 45

        '''Remarks
        ProduceSchedule = 46

        '''Remarks
        ProduceCertificate = 47

        '''Remarks
        ProduceDebitNote = 48

        '''Remarks
        ClaimsTypeBasis = 49

        '''Remarks
        ClaimsCoverBasis = 50

        '''Remarks
        TradeNbOnline = 51

        '''Remarks
        TradeMtaOnline = 52

        '''Remarks
        TradeRnlOnline = 53

        '''Remarks
        OnlineTradingCommencedOn = 54

        '''Remarks
        MTCRatingRulesEnabled = 55

        '''Remarks
        CanMakeBankGuarantee = 56

        '''Remarks
        CoverNotedocTemplate = 57

        '''Remarks
        IsTrueMonthlyPolicy = 58

        '''Remarks
        AnniversaryRenewalWeeks = 59

        '''Remarks
        UnifiedRenewalDay = 60

        '''Remarks
        LeadAllowConsolidatedCommission = 61

        '''Remarks
        LeadMonthInCycle = 62

        '''Remarks
        LeadSuspenseAccountId = 63

        '''Remarks
        SubAllowConsolidatedCommission = 64

        '''Remarks
        SubMonthInCycle = 65

        '''Remarks
        SubSuspenseAccountId = 66

        '''Remarks
        DefaultPaymentMethod = 67

        '''Remarks
        IsRenewable = 68

        '''Remarks
        IsRenewalSelectionEnabled = 69

        '''Remarks
        TrueMonthlyPolicyRenewalCommunication = 70

        '''Remarks
        RenewalSelectionManReviewTemplateId = 71

        '''Remarks
        RenewalSelectionManReviewAttachmentTemplateId = 72

        '''Remarks
        RenewalSelectionInviteTemplateId = 73

        '''Remarks
        RenewalSelectionInviteAttachmentTemplateId = 74

        '''Remarks
        RenewalSelectionUpdateTemplateId = 75

        '''Remarks
        RenewalSelectionUpdateAttachmentTemplateId = 76

        '''Remarks
        IsRenewalInviteEnabled = 77

        '''Remarks
        RenewalInviteManReviewTemplateId = 78

        '''Remarks
        RenewalInviteManReviewAttachmentTemplateId = 79

        '''Remarks
        RenewalInviteInviteTemplateId = 80

        '''Remarks
        RenewalInviteInviteAttachmentTemplateId = 81

        '''Remarks
        RenewalInviteUpdateTemplateId = 82

        '''Remarks
        RenewalInviteUpdateAttachmentTemplateId = 83

        '''Remarks
        IsRenewalUpdateEnabled = 84

        '''Remarks
        RenewalUpdateManReviewTemplateId = 85

        '''Remarks
        RenewalUpdateManReviewAttachmentTemplateId = 86

        '''Remarks
        RenewalUpdateInviteTemplateId = 87

        '''Remarks
        RenewalUpdateInviteAttachmentTemplateId = 88

        '''Remarks
        RenewalUpdateUpdateTemplateId = 89

        '''Remarks
        RenewalUpdateUpdateAttachmentTemplateId = 90

        '''Remarks
        IsAgentRenewalSelectionEnabled = 91

        '''Remarks
        IsAgentRenewalInviteEnabled = 92

        '''Remarks
        IsAgentRenewalUpdateEnabled = 93

        '''Remarks
        AgentRenewalManReviewTemplateId = 94

        '''Remarks
        AgentRenewalManReviewReportId = 95

        '''Remarks
        AgentRenewalInviteTemplateId = 96

        '''Remarks
        AgentRenewalInviteReportId = 97

        '''Remarks
        AgentRenewalUpdateTemplateId = 98

        '''Remarks
        AgentRenewalUpdateReportId = 99

        '''Remarks
        MultipleClaimsPayments = 100

        '''Remarks
        MaxUnauthorisedClaimValue = 101

        '''Remarks
        MaxUnauthorisedNoClaimPayments = 102

        '''Remarks
        RunAuthorisationScriptsClaimPayments = 103

        '''Remarks
        BankAccountId = 104

        '''Remarks
        ClaimValueForLargeLossAdvice = 105

        '''Remarks
        InclusionOfCoInsurersOnClaims = 106

        '''Remarks
        AllowNegativeReserve = 107

        '''Remarks
        ExtClmHandlerAcknowledgedTaskAllowedTime = 108

        '''Remarks
        ExtClmHandlerSupplyPreReportTaskAllowedTime = 109

        '''Remarks
        ValidPolicyVersionAtLossDate = 110

        '''Remarks
        IsGrossClaimPaymentAmount = 111

        '''Remarks
        ClaimTaskGroup = 112

        '''Remarks
        ClaimUserGroup = 113

        '''Remarks
        ClaimsUDTA = 114

        '''Remarks
        ClaimsUDTB = 115

        '''Remarks
        ClaimsUDTC = 116

        '''Remarks
        ClaimsUDTD = 117

        '''Remarks
        ClaimsUDTE = 118

        '''Remarks
        IsDuplicateClaimCheckEnabled = 119

        '''Remarks
        IsAdvancedTaxScriptEnabled = 120

        '''Remarks
        IsPaymentRefCheckEnabled = 121

        '''Remarks
        IsRecommendClaimPayments = 122

        '''Remarks
        OutOfSequenceMTAAllocation = 123

        '''Remarks
        OutOfSequenceMTADates = 124

        '''Remarks
        DefaultRenewalMonths = 125

        '''Remarks
        PaymentCannotExceedReserve = 126

        '''Remarks
        AllowBackdatedMTAs = 127

        '''Remarks
        CoverNoteNumberingId = 128

        '''Remarks
        CoverNoteDefaultPeriod = 129

        '''Remarks
        CoverNoteReusedUpTo = 130

        '''Remarks
        CheckMediatypeStatusAtClaimPayment = 131

        '''Remarks
        CheckMediatypeStatusAtPolicyRefund = 132

        '''Remarks
        RoundOffToZero = 133

        '''Remarks
        CanMakeLiveCashDeposit = 134

        '''Remarks
        AllowBackdatedMTCs = 135

        '''Remarks
        BackdatedMTAUserGroup = 136

        '''Remarks
        BackdatedMTATaskGroup = 137

        '''Remarks
        TMPAutorenfac = 138

        '''Remarks
        IsPrepaymentOptionEnabled = 139

        '''Remarks
        CanMakeLiveInvoiceTMP = 140

        '''Remarks
        CanMakeLiveInstalmentTMP = 141

        '''Remarks
        AllowWrittenStatus = 142

        '''Remarks
        WrittenTaskManagerDays = 143

        '''Remarks
        WrittenReminderUserGroup = 144

        '''Remarks
        WrittenReminderTaskGroup = 145

        '''Remarks
        ChangePolicyNumberAtRenewalAutomatically = 146

        '''Remarks
        BindRenewalWithoutInvitation = 147

        '''Remarks
        DoNotDeleteRenQuoteOnMTA = 148

        '''Remarks
        DefaultCoverToDateToLastDay = 149

        '''Remarks
        UnifiedRenewalDateIsReadOnly = 150

        '''Remarks
        IsReservesReadOnly = 151

        '''Remarks
        IsRecoveriesReadOnly = 152

        '''Remarks
        IsPaymentsReadOnly = 153

        '''Remarks
        RiManualPremiumAdjustment = 154

        '''Remarks
        QuoteAllRiskNB = 155

        '''Remarks
        QuoteAllRiskMTC = 156

        '''Remarks
        QuoteAllRiskMTA = 157

        '''Remarks
        AutoRenewBDMPolicy = 158

        AllowReRateAllCancRein = 159
        AllowReRateAllNBQuotation = 160
        DeleteRenQuoteReRunRenewal = 161

        '''Remarks
        QuoteAllRiskRenewal = 162

        '''Remarks
        RetainPolicyNumberonCopy = 163

        '''Remarks
        AnniversaryDateEditable = 164

        '''Remarks
        DisableCoverStartDateOnREN = 165

        AuthorisationThreshold = 166
    End Enum

    ' Start (Sriram P) - (Tech Spec WR19 - Cover Note Functionality.doc)section 8.2.3.7
    Public Function ProductRiskOptionString(ByVal ProductRiskOptions As ProductRiskOptions) As String
        Select Case ProductRiskOptions
            Case ProductRiskOptions.CaptionId
                Return "caption_id"
            Case ProductRiskOptions.Code
                Return "code"
            Case ProductRiskOptions.Description
                Return "description"
            Case ProductRiskOptions.ProductEffectiveDate
                Return "effective_date"
            Case ProductRiskOptions.IsDeleted
                Return "is_deleted"
            Case ProductRiskOptions.SchemeAgencyRef
                Return "scheme_agency_ref"
            Case ProductRiskOptions.BlockNo
                Return "block_no"
            Case ProductRiskOptions.IsTaxSuppressed
                Return "is_tax_suppressed"
            Case ProductRiskOptions.QuoteAutoNumberingID
                Return "quote_auto_numbering_id"
            Case ProductRiskOptions.IsShortPeriodRated
                Return "is_short_period_rated"
            Case ProductRiskOptions.IsMidnightRenewal
                Return "is_midnight_renewal"
            Case ProductRiskOptions.IsAutoRenewable
                Return "is_auto_renewable"
            Case ProductRiskOptions.RenewalWeeks
                Return "renewal_period"
            Case ProductRiskOptions.PolicyAutoNumberingID
                Return "policy_auto_numbering_id"
            Case ProductRiskOptions.ProvClaimAutoNumberingID
                Return "prov_claim_auto_numbering_id"
            Case ProductRiskOptions.FullClaimAutoNumberingID
                Return "full_claim_auto_numbering_id"
            Case ProductRiskOptions.Accumulation
                Return "is_accumulation"
            Case ProductRiskOptions.RIPointer
                Return "ri_pointer"
            Case ProductRiskOptions.ReportPointer
                Return "report_pointer"
            Case ProductRiskOptions.ClaimYearToCheck
                Return "claim_year_to_check"
            Case ProductRiskOptions.MaxSingleClaimValue
                Return "max_single_claim_value"
            Case ProductRiskOptions.MaxNumberOfClaim
                Return "max_number_of_claim"
            Case ProductRiskOptions.MaxTotalClaimValue
                Return "max_total_claim_value"
            Case ProductRiskOptions.NBProrata
                Return "nb_prorata"
            Case ProductRiskOptions.MTAProrata
                Return "mta_prorata"
            Case ProductRiskOptions.RoundPremium
                Return "round_prem_to_nearest_unit"
            Case ProductRiskOptions.RoundingSection
                Return "rounding_section_id"
            Case ProductRiskOptions.PolicyNumberAtQuote
                Return "is_policy_number_at_quote"
            Case ProductRiskOptions.FollowUpTimeFrame
                Return "follow_up_time_frame"
            Case ProductRiskOptions.GracePeriod
                Return "grace_period"
            Case ProductRiskOptions.PreventCancelledAgents
                Return "prevent_cancelled_agents"
            Case ProductRiskOptions.AllowPositiveValues
                Return "allow_positive_cancellation"
            Case ProductRiskOptions.MediaTypeMandatory
                Return "media_type_mandatory"
            Case ProductRiskOptions.PolicyStyleID
                Return "policy_style_id"
            Case ProductRiskOptions.PolicyStyleMandatory
                Return "policy_style_mandatory"
            Case ProductRiskOptions.CurrencyChange
                Return "allow_currency_change"
            Case ProductRiskOptions.LossCurrencyChange
                Return "allow_loss_currency_change"
            Case ProductRiskOptions.ChangePolicyNumberAtRenewal
                Return "change_policy_number_at_renewal"
            Case ProductRiskOptions.AllowStandardWordingEdit
                Return "allow_standard_wording_edit"
            Case ProductRiskOptions.HideSummaryAtRenewalAcceptance
                Return "hide_summary_at_renewal_acceptance"
            Case ProductRiskOptions.ProductSuppressClaimTransactionsReserves
                Return "suppress_reserves"
            Case ProductRiskOptions.ProductSuppressClaimTransactionsPayments
                Return "suppress_payments"
            Case ProductRiskOptions.ProductSuppressClaimTransactionsRecoveries
                Return "suppress_recoveries"
            Case ProductRiskOptions.CanMakeLiveInvoice
                Return "can_make_live_invoice"
            Case ProductRiskOptions.CanMakeLiveInstalments
                Return "can_make_live_instalments"
            Case ProductRiskOptions.CanMakeLivePaynow
                Return "can_make_live_paynow"
            Case ProductRiskOptions.ProduceSchedule
                Return "produce_schedule"
            Case ProductRiskOptions.ProduceCertificate
                Return "produce_certificate"
            Case ProductRiskOptions.ProduceDebitNote
                Return "produce_debit_note"
            Case ProductRiskOptions.TradeNbOnline
                Return "TradeNBOnline"
            Case ProductRiskOptions.TradeMtaOnline
                Return "TradeMTAOnline"
            Case ProductRiskOptions.TradeRnlOnline
                Return "TradeRNLOnline"
            Case ProductRiskOptions.OnlineTradingCommencedOn
                Return "OnlineTradingCommencedOn"
            Case ProductRiskOptions.MTCRatingRulesEnabled
                Return "enable_mtc_rating_rule"
            Case ProductRiskOptions.CanMakeBankGuarantee
                Return "can_make_live_bankguarantee"
            Case CoreBusiness.ProductRiskOptions.ChangePolicyNumberAtRenewalAutomatically
                Return "Change_Ren_Policy_No_Auto"
            Case ProductRiskOptions.CoverNotedocTemplate
                Return "Cover_Note_doc_Template_id"
            Case ProductRiskOptions.IsTrueMonthlyPolicy
                Return "is_true_monthly_policy"
            Case ProductRiskOptions.AnniversaryRenewalWeeks
                Return "anniversary_renewal_weeks"
            Case ProductRiskOptions.UnifiedRenewalDay
                Return "unified_renewal_day"
            Case ProductRiskOptions.LeadAllowConsolidatedCommission
                Return "lead_allow_consolidated_commission"
            Case ProductRiskOptions.LeadMonthInCycle
                Return "lead_month_in_cycle"
            Case ProductRiskOptions.LeadSuspenseAccountId
                Return "lead_suspense_account_id"
            Case ProductRiskOptions.SubAllowConsolidatedCommission
                Return "sub_allow_consolidated_commission"
            Case ProductRiskOptions.SubMonthInCycle
                Return "sub_month_in_cycle"
            Case ProductRiskOptions.SubSuspenseAccountId
                Return "sub_suspense_account_id"
            Case ProductRiskOptions.DefaultPaymentMethod
                Return "Default_Payment_Method"
            Case ProductRiskOptions.IsRenewable
                Return "is_renewable"
            Case ProductRiskOptions.IsRenewalSelectionEnabled
                Return "is_renewal_selection_enabled"
            Case ProductRiskOptions.TrueMonthlyPolicyRenewalCommunication
                Return "true_monthly_policy_renewal_communication"
            Case ProductRiskOptions.RenewalSelectionManReviewTemplateId
                Return "renewal_selection_man_review_template_id"
            Case ProductRiskOptions.RenewalSelectionManReviewAttachmentTemplateId
                Return "renewal_selection_man_review_attachment_template_id"
            Case ProductRiskOptions.RenewalSelectionInviteTemplateId
                Return "renewal_selection_invite_template_id"
            Case ProductRiskOptions.RenewalSelectionInviteAttachmentTemplateId
                Return "renewal_selection_invite_attachment_template_id"
            Case ProductRiskOptions.RenewalSelectionUpdateTemplateId
                Return "renewal_selection_update_template_id"
            Case ProductRiskOptions.RenewalSelectionUpdateAttachmentTemplateId
                Return "renewal_selection_update_attachment_template_id"
            Case ProductRiskOptions.IsRenewalInviteEnabled
                Return "is_renewal_invite_enabled"
            Case ProductRiskOptions.RenewalInviteManReviewTemplateId
                Return "renewal_invite_man_review_template_id"
            Case ProductRiskOptions.RenewalInviteManReviewAttachmentTemplateId
                Return "renewal_invite_man_review_attachment_template_id"
            Case ProductRiskOptions.RenewalInviteInviteTemplateId
                Return "renewal_invite_invite_template_id"
            Case ProductRiskOptions.RenewalInviteInviteAttachmentTemplateId
                Return "renewal_invite_invite_attachment_template_id"
            Case ProductRiskOptions.RenewalInviteUpdateTemplateId
                Return "renewal_invite_update_template_id"
            Case ProductRiskOptions.RenewalInviteUpdateAttachmentTemplateId
                Return "renewal_invite_update_attachment_template_id"
            Case ProductRiskOptions.IsRenewalUpdateEnabled
                Return "is_renewal_update_enabled"
            Case ProductRiskOptions.RenewalUpdateManReviewTemplateId
                Return "renewal_update_man_review_template_id"
            Case ProductRiskOptions.RenewalUpdateManReviewAttachmentTemplateId
                Return "renewal_update_man_review_attachment_template_id"
            Case ProductRiskOptions.RenewalUpdateInviteTemplateId
                Return "renewal_update_invite_template_id"
            Case ProductRiskOptions.RenewalUpdateInviteAttachmentTemplateId
                Return "renewal_update_invite_attachment_template_id"
            Case ProductRiskOptions.RenewalUpdateUpdateTemplateId
                Return "renewal_update_update_template_id"
            Case ProductRiskOptions.RenewalUpdateUpdateAttachmentTemplateId
                Return "renewal_update_update_attachment_template_id"
            Case ProductRiskOptions.IsAgentRenewalSelectionEnabled
                Return "is_agent_renewal_selection_enabled"
            Case ProductRiskOptions.IsAgentRenewalInviteEnabled
                Return "is_agent_renewal_invite_enabled"
            Case ProductRiskOptions.IsAgentRenewalUpdateEnabled
                Return "is_agent_renewal_update_enabled"
            Case ProductRiskOptions.AgentRenewalManReviewTemplateId
                Return "agent_renewal_man_review_template_id"
            Case ProductRiskOptions.AgentRenewalManReviewReportId
                Return "agent_renewal_man_review_report_id"
            Case ProductRiskOptions.AgentRenewalInviteTemplateId
                Return "agent_renewal_invite_template_id"
            Case ProductRiskOptions.AgentRenewalInviteReportId
                Return "agent_renewal_invite_report_id"
            Case ProductRiskOptions.AgentRenewalUpdateTemplateId
                Return "agent_renewal_update_template_id"
            Case ProductRiskOptions.AgentRenewalUpdateReportId
                Return "agent_renewal_update_report_id"
            Case ProductRiskOptions.MultipleClaimsPayments
                Return "multiple_claims_payments"
            Case ProductRiskOptions.MaxUnauthorisedClaimValue
                Return "max_unauthorised_claim_value"
            Case ProductRiskOptions.MaxUnauthorisedNoClaimPayments
                Return "max_unauthorised_no_claim_payments"
            Case ProductRiskOptions.RunAuthorisationScriptsClaimPayments
                Return "run_authorisation_scripts_claim_payments"
            Case ProductRiskOptions.BankAccountId
                Return "bankAccount_Id"
            Case ProductRiskOptions.ClaimValueForLargeLossAdvice
                Return "Claim_Value_For_Large_Loss_Advice"
            Case ProductRiskOptions.InclusionOfCoInsurersOnClaims
                Return "inclusion_of_CoInsurers_On_Claims"
            Case ProductRiskOptions.AllowNegativeReserve
                Return "allow_Negative_Reserve"
            Case ProductRiskOptions.ExtClmHandlerAcknowledgedTaskAllowedTime
                Return "ext_Clm_Handler_Acknowledged_Task_Allowed_Time"
            Case ProductRiskOptions.ExtClmHandlerSupplyPreReportTaskAllowedTime
                Return "ext_Clm_Handler_Supply_Pre_Report_Task_Allowed_Time"
            Case ProductRiskOptions.ValidPolicyVersionAtLossDate
                Return "valid_Policy_Version_At_Loss_Date"
            Case ProductRiskOptions.IsGrossClaimPaymentAmount
                Return "is_Gross_Claim_Payment_Amount"
            Case ProductRiskOptions.ClaimTaskGroup
                Return "claim_Task_Group"
            Case ProductRiskOptions.ClaimUserGroup
                Return "claim_User_Group"
            Case ProductRiskOptions.ClaimsUDTA
                Return "claims_UDT_A"
            Case ProductRiskOptions.ClaimsUDTB
                Return "claims_UDT_B"
            Case ProductRiskOptions.ClaimsUDTC
                Return "claims_UDT_C"
            Case ProductRiskOptions.ClaimsUDTD
                Return "claims_UDT_D"
            Case ProductRiskOptions.ClaimsUDTE
                Return "claims_UDT_E"
            Case ProductRiskOptions.IsDuplicateClaimCheckEnabled
                Return "is_Duplicate_Claim_Check_Enabled"
            Case ProductRiskOptions.IsAdvancedTaxScriptEnabled
                Return "is_Advanced_Tax_Script_Enabled"
            Case ProductRiskOptions.IsPaymentRefCheckEnabled
                Return "is_Payment_Ref_Check_Enabled"
            Case ProductRiskOptions.IsRecommendClaimPayments
                Return "is_Recommend_Claim_Payments"
            Case ProductRiskOptions.OutOfSequenceMTAAllocation
                Return "out_of_sequence_mta_allocation"
            Case ProductRiskOptions.OutOfSequenceMTADates
                Return "out_of_sequence_mta_dates"
            Case ProductRiskOptions.DefaultRenewalMonths
                Return "default_renewal_months"
            Case ProductRiskOptions.PaymentCannotExceedReserve
                Return "payment_cannot_exceed_reserve"
            Case ProductRiskOptions.AllowBackdatedMTAs
                Return "allow_backdated_mtas"
            Case ProductRiskOptions.CoverNoteNumberingId
                Return "Cover_Note_numbering_id"
            Case ProductRiskOptions.CoverNoteDefaultPeriod
                Return "Cover_Note_Default_Period"
            Case ProductRiskOptions.CoverNoteReusedUpTo
                Return " Cover_Note_reused_upto"
            Case ProductRiskOptions.RoundOffToZero
                Return "is_roundoff_to_zero"
            Case ProductRiskOptions.CheckMediatypeStatusAtClaimPayment
                Return "check_mediatype_status_at_claim_payment"
            Case ProductRiskOptions.CheckMediatypeStatusAtPolicyRefund
                Return "check_mediatype_status_at_policy_refund"
            Case ProductRiskOptions.CanMakeLiveCashDeposit
                Return "can_make_live_cashdeposit"
            Case ProductRiskOptions.BindRenewalWithoutInvitation
                Return "bind_renewal_without_invitation"
            Case ProductRiskOptions.AllowBackdatedMTCs
                Return "allow_backdated_can"
            Case ProductRiskOptions.BackdatedMTAUserGroup
                Return "out_of_Sequence_MTA_UserGroup"
            Case ProductRiskOptions.BackdatedMTATaskGroup
                Return "out_of_Sequence_MTA_TaskGroup"
            Case ProductRiskOptions.TMPAutorenfac
                Return "TMPautrenfac"
            Case ProductRiskOptions.IsPrepaymentOptionEnabled
                Return "is_enable_PrePayment"
            Case CoreBusiness.ProductRiskOptions.AllowWrittenStatus
                Return "allow_written_status"
            Case CoreBusiness.ProductRiskOptions.WrittenTaskManagerDays
                Return "written_task_manager_days"
            Case CoreBusiness.ProductRiskOptions.WrittenReminderUserGroup
                Return "written_rem_user_group"
            Case CoreBusiness.ProductRiskOptions.WrittenReminderTaskGroup
                Return "written_rem_task_group"

            Case CoreBusiness.ProductRiskOptions.DoNotDeleteRenQuoteOnMTA
                Return "do_not_delete_renQuote_on_MTA"
            Case CoreBusiness.ProductRiskOptions.DefaultCoverToDateToLastDay
                Return "default_cover_to_date_to_last_day"
            Case ProductRiskOptions.UnifiedRenewalDateIsReadOnly
                Return "Unified_Renewal_Date_Is_Read_Only"
            Case CoreBusiness.ProductRiskOptions.IsReservesReadOnly
                Return "Is_Reserves_Read_only"
            Case CoreBusiness.ProductRiskOptions.IsRecoveriesReadOnly
                Return "Is_Recoveries_Read_only"
            Case CoreBusiness.ProductRiskOptions.IsPaymentsReadOnly
                Return "Is_Payments_Read_only"
            Case CoreBusiness.ProductRiskOptions.QuoteAllRiskNB
                Return "Quote_all_risk_NB"
            Case CoreBusiness.ProductRiskOptions.QuoteAllRiskMTC
                Return "Quote_all_risk_MTC"
            Case CoreBusiness.ProductRiskOptions.QuoteAllRiskMTA
                Return "Quote_all_risk_MTA"
            Case CoreBusiness.ProductRiskOptions.AutoRenewBDMPolicy
                Return "Auto_Renew_BDMPolicy"
            Case CoreBusiness.ProductRiskOptions.RiManualPremiumAdjustment
                Return "Ri_Manual_Premium_Adjustment"
            Case CoreBusiness.ProductRiskOptions.AllowReRateAllCancRein
                Return "allow_rerate_all_canc_rein"
            Case CoreBusiness.ProductRiskOptions.AllowReRateAllNBQuotation
                Return "allow_rerate_all_nb_quotation"
            Case CoreBusiness.ProductRiskOptions.DeleteRenQuoteReRunRenewal
                Return "Delete_And_ReRun_RenQuote"
            Case CoreBusiness.ProductRiskOptions.QuoteAllRiskRenewal
                Return "Quote_all_risk_RENEWAL"
            Case CoreBusiness.ProductRiskOptions.RetainPolicyNumberonCopy
                Return "is_retain_policy_number_on_copy"
            Case CoreBusiness.ProductRiskOptions.AnniversaryDateEditable
                Return "Anniversary_Date_Editable"
            Case CoreBusiness.ProductRiskOptions.DisableCoverStartDateOnREN
                Return "disable_cover_start_date_on_REN"
            Case CoreBusiness.ProductRiskOptions.AuthorisationThreshold
                Return "Authorisation_Threshold"
            Case Else
                Return String.Empty
        End Select
    End Function

    Public Enum FinancePlanStatus As Long
        Item000
        Item010
        Item011
        Item012
        Item040
        Item140
        Item900
        Item990
        Item999
    End Enum

    Public Function FinancePlanStatusNumber(ByVal eFinancePlanStatus As FinancePlanStatus) As Integer
        Select Case eFinancePlanStatus
            Case FinancePlanStatus.Item000
                Return 0
            Case FinancePlanStatus.Item010
                Return 10
            Case FinancePlanStatus.Item011
                Return 11
            Case FinancePlanStatus.Item012
                Return 12
            Case FinancePlanStatus.Item040
                Return 40
            Case FinancePlanStatus.Item140
                Return 140
            Case FinancePlanStatus.Item900
                Return 900
            Case FinancePlanStatus.Item990
                Return 990
            Case FinancePlanStatus.Item999
                Return 999
        End Select
    End Function

    Public Function FinancePlanStatusString(ByVal eFinancePlanStatus As FinancePlanStatus) As String
        Select Case eFinancePlanStatus
            Case FinancePlanStatus.Item000
                Return "000"
            Case FinancePlanStatus.Item010
                Return "010"
            Case FinancePlanStatus.Item011
                Return "011"
            Case FinancePlanStatus.Item012
                Return "012"
            Case FinancePlanStatus.Item040
                Return "040"
            Case FinancePlanStatus.Item140
                Return "140"
            Case FinancePlanStatus.Item900
                Return "900"
            Case FinancePlanStatus.Item990
                Return "990"
            Case FinancePlanStatus.Item999
                Return "999"
            Case Else
                Return "000"
        End Select
    End Function

    Public Function FinancePlanNumber(ByVal iFinancePlanStatus As Integer) As FinancePlanStatus
        Select Case iFinancePlanStatus
            Case 0
                Return FinancePlanStatus.Item000
            Case 10
                Return FinancePlanStatus.Item010
            Case 11
                Return FinancePlanStatus.Item011
            Case 12
                Return FinancePlanStatus.Item012
            Case 40
                Return FinancePlanStatus.Item040
            Case 140
                Return FinancePlanStatus.Item140
            Case 900
                Return FinancePlanStatus.Item900
            Case 990
                Return FinancePlanStatus.Item990
            Case 999
                Return FinancePlanStatus.Item999
        End Select
    End Function

    Public Enum RiskTypeOptions As Long
        Code
        Description
        EffectiveDate
        AccumulationLevel
        GisScreenId
        PrimarySort
        SecondarySort
        StampDutyRate1
        StampDutyRate2
        HeaderClauseDescription
        TrailerClauseDescription
        IsShareWithCoInsurers
        IsShareWithReInsurers
        IsSuppressPublicText
        IsSuppressPrivateText
        IsSuppressTaxes
        ClaimsIsPostTaxes
        VsectionMask
        IsAutoReinsured
        IsDeferredRiPermitted
        DisplayReinsurance
        DisplayClaimReinsurance
        AllowRatingSectionAdd
        AllowRatingSectionEdit
        AllowRatingSectionDelete
        AllowEditRatingSectionRateType
        AllowEditRatingSectionRate
        AllowEditRatingSectionSumInsured
        AllowEditRatingSectionThisPremium
        ClaimsTypeBasis
        ClaimsCoverBasis
        AttachClaimOutsideOfPolicyPeriod
    End Enum

    ' Start (Sriram P) - (Tech Spec WR19 - Cover Note Functionality.doc)section 8.2.3.7
    Public Function RiskTypeOptionString(ByVal RiskTypeOptions As RiskTypeOptions) As String
        Select Case RiskTypeOptions
            Case RiskTypeOptions.Code
                Return "code"
            Case RiskTypeOptions.Description
                Return "Description"
            Case RiskTypeOptions.EffectiveDate
                Return "effective_date"
            Case RiskTypeOptions.AccumulationLevel
                Return "accumulation_level"
            Case RiskTypeOptions.GisScreenId
                Return "gis_screen_id"
            Case RiskTypeOptions.PrimarySort
                Return "primary_sort"
            Case RiskTypeOptions.SecondarySort
                Return "secondary_sort"
            Case RiskTypeOptions.StampDutyRate1
                Return "stamp_duty_rate1"
            Case RiskTypeOptions.StampDutyRate2
                Return "stamp_duty_rate2"
            Case RiskTypeOptions.HeaderClauseDescription
                Return "dt1.description"
            Case RiskTypeOptions.TrailerClauseDescription
                Return "dt2.description"
            Case RiskTypeOptions.IsShareWithCoInsurers
                Return "is_share_with_co_insurers"
            Case RiskTypeOptions.IsShareWithReInsurers
                Return "is_share_with_re_insurers"
            Case RiskTypeOptions.IsSuppressPublicText
                Return "is_suppress_public_text"
            Case RiskTypeOptions.IsSuppressPrivateText
                Return "is_suppress_private_text"
            Case RiskTypeOptions.IsSuppressTaxes
                Return "is_suppress_taxes"
            Case RiskTypeOptions.ClaimsIsPostTaxes
                Return "claims_is_post_taxes"
            Case RiskTypeOptions.VsectionMask
                Return "section_mask"
            Case RiskTypeOptions.IsAutoReinsured
                Return "is_auto_reinsured"
            Case RiskTypeOptions.IsDeferredRiPermitted
                Return "is_deferred_RI_permitted"
            Case RiskTypeOptions.DisplayReinsurance
                Return "display_reinsurance_screen"

            Case RiskTypeOptions.DisplayClaimReinsurance
                Return "display_claims_reinsurance_screen"
            Case RiskTypeOptions.AllowRatingSectionAdd
                Return "allow_add_ratingsection"
            Case RiskTypeOptions.AllowRatingSectionEdit
                Return "allow_edit_ratingsection"
            Case RiskTypeOptions.AllowRatingSectionDelete
                Return "allow_delete_ratingsection"
            Case RiskTypeOptions.AllowEditRatingSectionRateType
                Return "allow_edit_ratingsection_ratetype"

            Case RiskTypeOptions.AllowEditRatingSectionRate
                Return "allow_edit_ratingsection_rate"
            Case RiskTypeOptions.AllowEditRatingSectionSumInsured
                Return "allow_edit_ratingsection_suminsured"
            Case RiskTypeOptions.AllowEditRatingSectionThisPremium
                Return "allow_edit_ratingsection_thispremium"

            Case RiskTypeOptions.ClaimsTypeBasis
                Return "claims_type_basis_id"
            Case RiskTypeOptions.ClaimsCoverBasis
                Return "claims_cover_basis_id"
            Case RiskTypeOptions.AttachClaimOutsideOfPolicyPeriod
                Return "Attach_Claim_Outside_Of_Policy_Period"
            Case Else
                Return String.Empty
        End Select
    End Function

    Public Overloads Function ProcessAccounts(ByVal ProcessAccountsInput As ProcessAccountsIn) As ProcessAccountsOut
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)

            Dim oResponse As ProcessAccountsOut

            oResponse = ProcessAccounts(con, ProcessAccountsInput)

            Return oResponse

        End Using

    End Function

    Public Overloads Function ProcessAccounts(ByVal con As SiriusConnection, ByVal ProcessAccountsInput As ProcessAccountsIn) As ProcessAccountsOut

        Dim oOut As New ProcessAccountsOut
        Dim ErrEx As Exception = Nothing

        ' Create the QuotePolicy Object

        Dim oGIS As bGIS.QuotePolicy = Nothing
        Try
            oGIS = New bGIS.QuotePolicy
        Catch ex As Exception
            ExceptionManager.Publish(ex)
            Debug.WriteLine(ex.Message)
        Finally
        End Try

        ' Initialise the GIS
        SAMFunc.InitialiseGISQP(Con:=con, oGIS:=oGIS, SiriusUser:=_SiriusUser)

        Dim iRet As System.Int32
        Dim TransactionArray As Object = Nothing

        ' Call the Method
        Try

            iRet = oGIS.ProcessAccounts(
                v_sGisDataModelCode:=ProcessAccountsInput.DataModelCode,
                v_sGisBusinessTypeCode:=ProcessAccountsInput.BusinessTypeCode,
                v_lInsuranceFileCnt:=ProcessAccountsInput.InsuranceFileCnt,
                v_sTransactionType:=ProcessAccountsInput.TransactionType,
                v_bMTAInstallments:=ProcessAccountsInput.MTAInstallments,
                v_sCancelRefundAmt:=ProcessAccountsInput.CancelRefundAmt,
                v_bRenewalInstallments:=ProcessAccountsInput.RenewalInstallments,
                r_vTransactionArray:=TransactionArray) ', _
            '                    v_iDebitAgainst:=ProcessAccountsInput.DebitAgainst, _
            '                    v_lPaymentAccountId:=ProcessAccountsInput.AccountID)

        Catch ex As Exception
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
            ErrEx = New Exception("Failed to call bGIS.QuotePolicy.ProcessAccounts", ex)
            ExceptionManager.Publish(ErrEx)
            Throw ErrEx
        Finally

            If (ErrEx Is Nothing) Then
                If (iRet <> 1) Then

                    ErrEx = New Exception("bGIS.QuotePolicy.ProcessAccounts FAILED. Return Value = " + iRet.ToString)
                    ExceptionManager.Publish(ErrEx)
                    If oGIS IsNot Nothing Then
                        oGIS.Dispose()
                        oGIS = Nothing
                    End If
                    Throw ErrEx

                End If

                oOut.TransactionArray = TransactionArray

            End If
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
        End Try

        Return oOut
    End Function

    Public Overloads Function FindParty(ByVal FindPartyRequest As FindPartyIn) As FindPartyOut
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)

            Dim oResponse As FindPartyOut

            oResponse = FindParty(con, FindPartyRequest)

            Return oResponse

        End Using

    End Function

    Public Overloads Function FindParty(ByVal con As SiriusConnection, ByVal FindPartyInput As FindPartyIn) As FindPartyOut

        Const ACMethodName As String = "FindParty"

        'SAMFunc.STSLogMessage(TraceLevelSwitch, TraceLevel.Verbose, "Creating FindParty Performance Counter")

        '' SiriusPerfCounters must be declared at the TOP of the method
        'Dim oCounters As New SiriusPerfCounters("FindParty")

        ' Local Variable for the results of the Call
        Dim vResultArray As Object = Nothing

        Dim oOut As FindPartyOut
        Dim iRet As System.Int32
        Dim sCalling As String = "STS"
        Dim oDatabase As Object = Nothing
        Dim oAddData As AdditionalData
        Dim iRow As System.Int32
        Dim vAdditionalData As Object(,) = Nothing
        Dim lUpper As Int32
        'Dim Utils As Utilities

        'SAMFunc.STSLogMessage(TraceLevelSwitch, TraceLevel.Verbose, "Entering FindParty")

        ' New instance of the output structure
        oOut = New FindPartyOut

        Dim oGIS As bGIS.QuotePolicy = Nothing
        Try
            oGIS = New bGIS.QuotePolicy
        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.FailedToCreateBackofficeComponent, "Failed to create bGIS.Application", ex.Message)
            STSErrorEx.SetContext(oOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "CreateBusiness", True)
            Return oOut
        End Try

        'SAMFunc.STSLogMessage(TraceLevelSwitch, TraceLevel.Verbose, "Calling oGIS.Initialise")

        Try

            'Rk modifies as part of SAM SFI Interop conversions by replacing .PMDAODatabase by .FromSirius.SqlConnection.Database for vDatabase parameter.
            'con = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

            If Not con Is Nothing Then
                oDatabase = con.PMDAODatabase
            End If

            iRet = oGIS.Initialise(sUserName:=_SiriusUser.Username,
                                   sPassword:=_SiriusUser.Password,
                                   iUserID:=CInt(_SiriusUser.UserID),
                                   iSourceID:=CInt(_SiriusUser.SourceID),
                                   iLanguageID:=CInt(_SiriusUser.LanguageID),
                                   iCurrencyID:=CInt(_SiriusUser.CurrencyID),
                                   iLogLevel:=SiriusUserDefaults.LogLevel,
                                   sCallingAppName:=sCalling,
                                   vDatabase:=oDatabase)

        Catch ex As Exception
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
            Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.FailedToInitialiseBackofficeComponent, "Failed to initialise bGIS.Application", ex.Message)
            STSErrorEx.SetContext(oOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "oGIS.Initialise", True)
            Return oOut
        End Try

        If (iRet <> 1) Then
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
            ' End the recording
            'oCounters.FailMethod()
            Dim STSError As New STSErrorPublisher(iRet, "Failed to initialise GIS")
            STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "GISInitialise", True)
        End If

        oAddData = New AdditionalData
        oAddData.Name = "POLICY_NO"
        oAddData.Value = FindPartyInput.sPolicyNo

        ' Add the extra policy_number
        With FindPartyInput
            If ((.vAdditionalDataArray Is Nothing) = False) Then
                lUpper = .vAdditionalDataArray.GetUpperBound(0)
                lUpper += 1
                ReDim Preserve .vAdditionalDataArray(lUpper)
                .vAdditionalDataArray(lUpper) = oAddData
            Else
                ReDim .vAdditionalDataArray(0)
                .vAdditionalDataArray(0) = oAddData
            End If

            ' CTAF 20030321 - Add the CalledFromSTS value
            Try
                Utilities.AddCallFromSTS(.vAdditionalDataArray)
            Catch ex As Exception
                If oGIS IsNot Nothing Then
                    oGIS.Dispose()
                    oGIS = Nothing
                End If
                ' End the recording
                'oCounters.FailMethod()
                Dim STSErrorEx As New STSErrorPublisher("Failed to AddCallFromSTS", ex)
                STSErrorEx.SetContext(oOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "oGIS.Initialise", True)
            End Try

        End With

        ' Do we have any additional data?
        If (IsArray(FindPartyInput.vAdditionalDataArray) = True) Then

            ReDim vAdditionalData(1, FindPartyInput.vAdditionalDataArray.GetUpperBound(0))

            iRow = 0
            For Each oAddData In FindPartyInput.vAdditionalDataArray
                vAdditionalData(1, iRow) = CType(oAddData.Value, Object)
                vAdditionalData(0, iRow) = CType(oAddData.Name, Object)
                iRow = iRow + 1
            Next

        End If

        'SAMFunc.STSLogMessage(TraceLevelSwitch, TraceLevel.Verbose, "Calling oGIS.FindParty")

        With FindPartyInput
            iRet = oGIS.FindParty(
                    v_sGisDataModelCode:=CType(.sGisDataModelCode, String),
                    v_sGisBusinessTypeCode:=CType(.sGisBusinessTypeCode, String),
                    v_sPartyType:=CType(.sPartyType, String),
                    v_sShortname:=CType(.sShortname, String),
                    v_sResolvedName:=CType(.sResolvedName, String),
                    v_sUserID:=CType(.sUserID, String),
                    v_sTelephoneNumber:=CType(.sTelephoneNumber, String),
                    v_sPostcode:=CType(.sPostcode, String),
                    r_vResultArray:=vResultArray,
                    v_vAdditionalDataArray:=vAdditionalData,
                    v_sAddress1:=CType(.sAddress1, String),
                    v_lLeadAgentCnt:= .lLeadAgentCnt,
                    v_sFileCode:= .FileCode)
        End With

        If (iRet = PMEReturnCode.PMNotFound) Then ' PMEReturnCode.PMNotFound
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
            ' Dont error, just return nothing
            oOut = Nothing
            Return oOut
        End If

        If (iRet <> PMEReturnCode.PMTrue) Then
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
            ' End the recording
            'oCounters.FailMethod()
            Dim STSError As New STSErrorPublisher(iRet, "Failed to FindParty")
            STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "oGIS.FindParty", True)
        End If

        If oGIS IsNot Nothing Then
            oGIS.Dispose()
            oGIS = Nothing
        End If

        ' Populate the output structure
        'oOut.Populate(vResultArray)
        Dim arrColHeaders(,) As Object = {{"PartyKey", "ShortName",
                                    "ResolvedName", "AddressLine1",
                                    "PostCode", "ContactTelephoneNumber",
                                    "DateOfBirth", "AgentKey"},
                                    {System.Type.GetType("System.Int32"),
                                    System.Type.GetType("System.String"),
                                    System.Type.GetType("System.String"),
                                    System.Type.GetType("System.String"),
                                    System.Type.GetType("System.String"),
                                    System.Type.GetType("System.String"),
                                    System.Type.GetType("System.DateTime"),
                                    System.Type.GetType("System.Int32")}}

        oOut.ResultArray = Utilities.ArrayToDataSet(vResultArray, arrColHeaders, "BaseFindPartyResponseTypeParties")

        ' End the recording
        'oCounters.EndMethod()

        Return oOut

    End Function

    Public Overloads Function AddQuote(ByVal con As SiriusConnection, ByVal AddQuoteInput As AddQuoteIn) As AddQuoteOut

        Const ACMethodName As String = "AddQuote"

        'SAMFunc.STSLogMessage(TraceLevelSwitch, TraceLevel.Verbose, "Creating AddQuote performance counter")

        ' SiriusPerfCounters must be declared at the TOP of the method
        'Dim oCounters As New SiriusPerfCounters("AddQuote")

        Dim iRet As System.Int32
        Dim oOut As New AddQuoteOut
        'Dim Utils As Utilities
        Dim lAgentCnt As Int32 = 0
        Dim lGISSchemeID As Int32 = 0

        Dim oSTSError As New STSErrorPublisher

        Dim ErrEx As Exception = Nothing

        If AddQuoteInput.EffectiveDate > AddQuoteInput.ExpirationDate Then
            oSTSError.AddInvalidField("CoverStartDate", STSErrorPublisher.STSErrorCodes.CoverEndDateIsBeforeCoverStartDate.ToString, "CoverEndDate is before CoverStartDate", AddQuoteInput.EffectiveDate & ", " & AddQuoteInput.ExpirationDate)
        End If

        If oSTSError.HasErrors Then
            oSTSError.SetContext(oOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Mandatory field validation", True)
            Return oOut
        End If

        ' Create the Application Object
        Dim oGIS As bGIS.Application = Nothing
        Try
            oGIS = New bGIS.Application
        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.FailedToCreateBackofficeComponent, "Failed to create bGIS.Application", ex.Message)
            STSErrorEx.SetContext(oOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "CreateBusiness", True)
            Return oOut
        Finally
        End Try

        ' Initialise the GIS
        Try
            SAMFunc.InitialiseGIS(Con:=con, oGIS:=oGIS, SiriusUser:=_SiriusUser)
        Catch ex As Exception
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
            Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.FailedToInitialiseBackofficeComponent, "Failed to initialise bGIS.Application", ex.Message)
            STSErrorEx.SetContext(oOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Initialise.Business", True)
            Return oOut
        End Try

        ' Dimension the Array
        Dim vAdditionalData As Object = Nothing

        Try
            Utilities.AddCallFromSTS(AddQuoteInput.AdditionalDataArray)
        Catch oe As Exception
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
            'oCounters.FailMethod()
            Dim STSError As New STSErrorPublisher("Failed to add parameter to AdditionalDataArray", oe)
            STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "UtilitiesAddQuoteFromSTS", True)
        End Try

        ' Move the Data from the Classes to the Array
        vAdditionalData = Utilities.ClassesToArray(AddQuoteInput.AdditionalDataArray)

        ' Call the Method
        Try
            With AddQuoteInput

                Try
                    lAgentCnt = CType(.AgentCnt, Integer)
                Catch
                    lAgentCnt = 0 ' missing so default it
                End Try


                Dim businessType As String = "DIRECT"

                If (lAgentCnt <> 0) Then
                    businessType = "AGENCY"
                End If

                iRet = oGIS.AddQuote(
                    v_sGisDataModelCode:= .DataModelCode,
                    v_sGisBusinessTypeCode:= .BusinessTypeCode,
                    v_dtEffectiveDate:= .EffectiveDate,
                    v_dtExpirationDate:= .ExpirationDate,
                    v_sInsuredName:= .InsuredName,
                    v_lPartyCnt:= .PartyCnt,
                    r_lAgentCnt:=lAgentCnt,
                    r_lInsuranceFolderCnt:= .InsuranceFolderCnt,
                    r_lInsuranceFileCnt:= .InsuranceFileCnt,
                    v_sInsuranceFolderDescription:= .InsuranceFolderDescription,
                    r_sInsuranceFileRef:= .InsuranceFileRef,
                    r_lRiskCodeId:= .RiskCodeId,
                    v_lSourceID:= .SourceId,
                    r_lInsurerCnt:= .InsurerCnt,
                    v_lscreenid:= .ScreenId,
                    v_vAlternateReference:= .AlternateReference,
                    r_lRiskCnt:= .RiskCnt,
                    r_lRiskGroupId:= .RiskGroupId,
                    r_lGisSchemeId:=lGISSchemeID,
                    v_lCurrencyID:= .CurrencyId,
                    v_lAnalysisCodeId:= .AnalysisId,
                    v_sPolicyStatusCode:= .PolicyStatusCode,
                    v_lPolicyVersion:= .PolicyVersion,
                    v_sPaymentMethod:="Cash",
                    v_sBusinessType:=businessType,
                    v_sRenewalFrequency:="ANNUAL",
                    v_sInsuranceFileStructure:="PMB",
                    r_vAdditionalDataArray:=vAdditionalData,
                    v_lLapsedReasonId:= .LapsedReasonID,
                    v_dtLapsedDate:= .LapsedDate,
                    v_sLapsedReasonDescription:= .LapsedReasonDescription,
                    v_dtInceptionDate:= .InceptionDate,
                    v_dtInceptionDateTPI:= .InceptionDateTPI,
                    v_dtRenewalDate:= .RenewalDate,
                    v_sOldPolicyNumber:= .OldPolicyNumber,
                    v_sAccountExecutiveShortname:= .AccountExecutiveShortname,
                    v_sAlternateReference:= .AlternateReference,
                    v_sAccountHandlerShortname:= .AccountHandlerShortname,
                    v_sInsuranceFileTypeCode:= .PolicyVersionTypeCode,
                    sCoInsurancePlacement:= .sCoInsurancePlacement)

            End With

        Catch ex As Exception
            'oCounters.FailMethod()

            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If

            Dim STSError As New STSErrorPublisher("bGIS.QuotePolicy.AddQuote failed", ex)
            STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "AddQuoteException", True)
        End Try

        If oGIS IsNot Nothing Then
            oGIS.Dispose()
            oGIS = Nothing
        End If

        If (iRet <> PMEReturnCode.PMTrue) Then
            'oCounters.FailMethod()

            Select Case iRet
                Case CInt(STSErrorPublisher.STSErrorCodes.BackofficeComponentReturnedRecordInUse)
                    'SAMFunc.STSLogMessage(TraceLevelSwitch, TraceLevel.Verbose, "Error: A policy already exists with code " & AddQuoteInput.InsuranceFileRef)
                    oSTSError = New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.BackofficeComponentReturnedRecordInUse, "Policy In Use", "A policy already exists with code " & AddQuoteInput.InsuranceFileRef)
                    oSTSError.SetContext("AddQuote")
                Case Else
                    oSTSError = New STSErrorPublisher(iRet, "bGIS.QuotePolicy.AddQuote failed. Please ensure that the GIS Datamodel is accessible via SAM by checking the setting in the GIS Datamodel Editor.")
                    oSTSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "AddQuoteReturn", True)
            End Select
        End If

        If (oSTSError.HasErrors) Then
            oSTSError.SetContext(oOut.STSError, "STS", "AddQuote", True)
            Return oOut
        End If

        ' Copy the Outputs from the In/Outs
        oOut.InsuranceFileCnt = AddQuoteInput.InsuranceFileCnt
        oOut.InsuranceFileRef = AddQuoteInput.InsuranceFileRef
        oOut.InsuranceFolderCnt = AddQuoteInput.InsuranceFolderCnt
        oOut.RiskCnt = AddQuoteInput.RiskCnt
        oOut.AgentCnt = lAgentCnt
        oOut.InsurerCnt = AddQuoteInput.InsurerCnt
        oOut.RiskGroupId = AddQuoteInput.RiskGroupId
        oOut.RiskCodeId = AddQuoteInput.RiskCodeId
        oOut.GISSchemeID = lGISSchemeID

        ' Create the return Additional Data
        oOut.AdditionalDataArray = Utilities.ArrayToClasses(vAdditionalData)

        ' End the recording
        'oCounters.EndMethod()

        Return oOut

    End Function

    Public Overloads Function AddRisk(ByVal con As SiriusConnection, ByVal AddRiskInput As AddRiskIn) As AddRiskOut

        Const ACMethodName As String = "AddRisk"

        'Dim oGIS As bGIS.Application = Nothing
        Dim iRet As System.Int32
        Dim oOut As New AddRiskOut
        'Dim Utils As Utilities

        Dim ErrEx As Exception = Nothing

        'SAMFunc.STSLogMessage(TraceLevelSwitch, TraceLevel.Verbose, "Entering AddRisk")
        'SAMFunc.STSLogMessageIndent(True)

        ' Create the application object
        Dim oGIS As bGIS.Application = Nothing
        Try
            oGIS = New bGIS.Application
        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.FailedToCreateBackofficeComponent, "Failed to create bGIS.Application", ex.Message)
            STSErrorEx.SetContext(oOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "CreateBusiness", True)
            Return oOut
        End Try

        'SAMFunc.STSLogMessage(TraceLevelSwitch, TraceLevel.Verbose, "Initialising bGIS.Application")

        Try
            ' Initialise the GIS
            SAMFunc.InitialiseGIS(Con:=con, oGIS:=oGIS, SiriusUser:=_SiriusUser)
        Catch ex As Exception
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
            Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.FailedToInitialiseBackofficeComponent, "Failed to initialise bGIS.Application", ex.Message)
            STSErrorEx.SetContext(oOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "InitialiseBusiness", True)
            Return oOut
        End Try

        ' Dimension the Array
        Dim vAdditionalData As Object = Nothing

        ' CTAF 20030321 - Add the CalledFromSTS value
        Try
            Utilities.AddCallFromSTS(AddRiskInput.AdditionalDataArray)
        Catch oe As Exception
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
            Dim STSError As New STSErrorPublisher("Failed to add parameter to AdditionalDataArray", oe)
            STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "AddCallFromSTS", True)
        End Try

        ' Move the Data from the Classes to the Array
        vAdditionalData = Utilities.ClassesToArray(AddRiskInput.AdditionalDataArray)

        ' Call the Method
        Try
            'SAMFunc.STSLogMessage(TraceLevelSwitch, TraceLevel.Verbose, "Calling bGIS.AddRisk")

            With AddRiskInput

                Dim riskFolderCnt As Integer = .RiskFolderCnt
                Dim riskCnt As Integer = .RiskCnt
                Dim xmlDataSet As String = .XMLDataset
                Dim xmlDataSetDef As String = .XMLDataSetDef
                Dim policylinkId As Integer = .PolicyLinkID
                Dim topOIKey As String = .TopOIKey
                Dim quoteRef As String = .QuoteRef
                Dim quoteRefPassword As String = .QuoteRefPassword

                iRet = oGIS.AddRisk(
                    v_sBackOfficeMapperCode:=CType(.BackOfficeMapperCode, String),
                    v_sGisDataModelCode:=CType(.DataModelCode, String),
                    v_sGisBusinessTypeCode:=CType(.BusinessTypeCode, String),
                    v_lInsuranceFolderCnt:=CType(.InsuranceFolderCnt, Integer),
                    v_lInsuranceFileCnt:=CType(.InsuranceFileCnt, Integer),
                    v_lPartyCnt:=CType(.PartyCnt, Integer),
                    v_lRiskTypeId:=CType(.RiskTypeId, Integer),
                    v_lRiskScreenId:=CType(.RiskScreenId, Integer),
                    v_sRiskDescription:=CType(.RiskDescription, String),
                    v_lProductID:=CType(.ProductID, Integer),
                    r_lRiskFolderCnt:=riskFolderCnt,
                    r_lRiskCnt:=riskCnt,
                    r_sXMLDataSetDef:=xmlDataSetDef,
                    r_sXMLDataset:=xmlDataSet,
                    r_lPolicyLinkID:=policylinkId,
                    r_sTopOIKey:=topOIKey,
                    r_sQuoteRef:=quoteRef,
                    r_sQuoteRefPassword:=quoteRefPassword,
                    r_vAdditionalDataArray:=vAdditionalData)

                .RiskFolderCnt = Cast.ToInt32(riskFolderCnt, 0)
                .RiskCnt = Cast.ToInt32(riskCnt, 0)
                .XMLDataset = Cast.ToString(xmlDataSet, String.Empty)
                .XMLDataSetDef = Cast.ToString(xmlDataSetDef, String.Empty)
                .PolicyLinkID = Cast.ToInt32(policylinkId, 0)
                .TopOIKey = Cast.ToString(topOIKey, String.Empty)
                .QuoteRef = Cast.ToString(quoteRef, String.Empty)
                .QuoteRefPassword = Cast.ToString(quoteRefPassword, String.Empty)

            End With

        Catch ex As Exception
            SAMFunc.STSLogMessage(TraceLevelSwitch, TraceLevel.Verbose, "Failed to call bGIS.AddRisk with an Exception")

            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If

            Dim STSError As New STSErrorPublisher("bGIS.Application.AddRisk failed", ex)
            STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "AddRiskException", True)
        End Try

        If oGIS IsNot Nothing Then
            oGIS.Dispose()
            oGIS = Nothing
        End If

        If (iRet <> PMEReturnCode.PMTrue) Then
            SAMFunc.STSLogMessage(TraceLevelSwitch, TraceLevel.Verbose, "Failed to call bGIS.AddRisk with a Sirius Error = " & iRet.ToString)
            Dim STSError As New STSErrorPublisher(iRet, "bGIS.Application.AddRisk failed")
            STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "AddRiskReturn", True)
        End If

        ' Copy the Outputs from the In/Outs
        With oOut
            .PolicyLinkID = AddRiskInput.PolicyLinkID
            .QuoteRef = AddRiskInput.QuoteRef
            .QuoteRefPassword = AddRiskInput.QuoteRefPassword
            .RiskCnt = AddRiskInput.RiskCnt
            .RiskFolderCnt = AddRiskInput.RiskFolderCnt
            .TopOIKey = AddRiskInput.TopOIKey
            .XMLDataset = AddRiskInput.XMLDataset
            .XMLDataSetDef = AddRiskInput.XMLDataSetDef
            ' Create the return Additional Data
            .AdditionalDataArray = Utilities.ArrayToClasses(vAdditionalData)
        End With

        iRet = UpdateRiskNumber(con, AddRiskInput.InsuranceFileCnt, AddRiskInput.RiskCnt)

        If (iRet <> PMEReturnCode.PMTrue) Then
            SAMFunc.STSLogMessage(TraceLevelSwitch, TraceLevel.Verbose, "Failed to call bSIRListRisks.Business with a Sirius Error = " & iRet.ToString)
            Dim STSError As New STSErrorPublisher(iRet, "bSIRListRisks.Business.UpdateRiskNumber failed")
            STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "UpdateRiskNumber", True)
        End If

        'SAMFunc.STSLogMessage(TraceLevelSwitch, TraceLevel.Verbose, "QuoteRef = " & oOut.QuoteRef)

        'SAMFunc.STSLogMessageIndent(False)
        'SAMFunc.STSLogMessage(TraceLevelSwitch, TraceLevel.Verbose, "Leaving AddRisk")

        Return oOut

    End Function

    Public Function UpdateRiskNumber(ByVal con As SiriusConnection, ByVal iInsurancefilecnt As Int32, ByVal iRiskId As Int32) As Integer
        Dim iRet As System.Int32
        Dim iRiskNumber As Int32

        Dim obSIRListRisks As bSIRListRisks.Business = Nothing

        Try
            obSIRListRisks = New bSIRListRisks.Business
        Catch ex As Exception
            Return iRet
        End Try

        SAMFunc.InitialiseSBOObject(Con:=con,
             oObject:=obSIRListRisks,
             SiriusUser:=_SiriusUser,
             sObjectName:="bSIRListRisks.Business")

        Try
            iRet = obSIRListRisks.GetNextRiskNo(v_lInsuranceFileCnt:=iInsurancefilecnt,
                                                    r_lRiskNumber:=iRiskNumber)
        Catch ex As Exception
            If obSIRListRisks IsNot Nothing Then
                obSIRListRisks.Dispose()
                obSIRListRisks = Nothing
            End If
            Return iRet
        End Try

        Try
            ' Save the risk number to the risk record
            iRet = obSIRListRisks.UpdateRiskNo(v_lRiskCnt:=iRiskId,
                                                 v_lRiskNumber:=iRiskNumber)
        Catch ex As Exception
            If obSIRListRisks IsNot Nothing Then
                obSIRListRisks.Dispose()
                obSIRListRisks = Nothing
            End If
            Return iRet
        End Try
        If obSIRListRisks IsNot Nothing Then
            obSIRListRisks.Dispose()
            obSIRListRisks = Nothing
        End If
        Return iRet

    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="DeleteRiskIn"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeleteRiskOnAdd(ByVal DeleteRiskIn As DeleteRiskIn) As DeleteRiskOut

        Const ACMethodName As String = "DeleteRiskOnAdd"

        Dim STSError As New STSErrorPublisher
        Dim iRet As Integer
        Dim oOutput As DeleteRiskOut = Nothing

        Dim oGIS As bGIS.STS = Nothing
        Try
            oGIS = New bGIS.STS
        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.FailedToCreateBackofficeComponent, "Failed to create bGIS.STS", ex.Message)
            STSErrorEx.SetContext(oOutput.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "bGISSTS", True)
            Return oOutput
        End Try

        ' Initialise the GIS
        Try
            SAMFunc.InitialiseGISSTS(oGIS, _SiriusUser)
        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.FailedToInitialiseBackofficeComponent, "Failed to intialise bGIS.STS", ex.Message)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "InitGIS", True)
        End Try

        Try

            iRet = oGIS.DeleteRisk(v_lRiskCnt:=DeleteRiskIn.RiskCnt, v_lInsuranceFileCnt:=DeleteRiskIn.InsuranceFileCnt, v_lInsuranceFolderCnt:=DeleteRiskIn.InsuranceFolderCnt, v_sTransactionType:=DeleteRiskIn.TransactionType, bViaSAM:=True)

            If iRet = PMEReturnCode.PMNotFound Then
                Dim STSErrorEX As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.RiskRecordNotFound, "Risk record not found", DeleteRiskIn.RiskCnt.ToString)
                STSErrorEX.SetContext(oOutput.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Risk record not found", True)

                ' Terminate the reference to the GIS
                oGIS.Dispose()
                oGIS = Nothing

                Return oOutput

            ElseIf iRet = PMEReturnCode.PMInvalidRiskStatus Then
                Dim STSErrorEX As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.StatusOfRiskPreventsDeletion, "Status of risk prevents deletion", DeleteRiskIn.RiskCnt.ToString)
                STSErrorEX.SetContext(oOutput.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Record Not deleted", True)

                ' Terminate the reference to the GIS
                oGIS.Dispose()
                oGIS = Nothing

                Return oOutput

            ElseIf (iRet <> PMEReturnCode.PMTrue) Then
                ' End the recording
                Dim STSErrorEX As New STSErrorPublisher(iRet, "bGIS.STS.DeleteRisk failed")
                STSErrorEX.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, ACMethodName, True)
            End If

        Catch ex As Exception

            ' Terminate the reference to the GIS
            oGIS.Dispose()
            oGIS = Nothing

            Dim STSErrorEx As New STSErrorPublisher("An error occured calling bGIS.STS.DeleteRisk", ex)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, ACMethodName, True)

        End Try

        Return oOutput

    End Function

    ''' <summary>
    ''' Delete Risk
    ''' </summary>
    ''' <param name="DeleteRiskIn"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeleteRisk(ByVal DeleteRiskIn As DeleteRiskIn) As DeleteRiskOut

        Const ACMethodName As String = "DeleteRisk"

        Dim STSError As New STSErrorPublisher
        Dim iRet As Integer
        Dim oOutput As DeleteRiskOut = Nothing

        Dim oGIS As bGIS.STS = Nothing
        Try
            oGIS = New bGIS.STS
        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.FailedToCreateBackofficeComponent, "Failed to create bGIS.STS", ex.Message)
            STSErrorEx.SetContext(oOutput.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "bGISSTS", True)
            Return oOutput
        End Try

        ' Initialise the GIS
        Try
            SAMFunc.InitialiseGISSTS(oGIS, _SiriusUser)
        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.FailedToInitialiseBackofficeComponent, "Failed to intialise bGIS.STS", ex.Message)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "InitGIS", True)
        End Try

        Try

            iRet = oGIS.DeleteRisk(v_lRiskCnt:=DeleteRiskIn.RiskCnt, v_lInsuranceFileCnt:=DeleteRiskIn.InsuranceFileCnt, v_lInsuranceFolderCnt:=DeleteRiskIn.InsuranceFolderCnt, v_sTransactionType:=DeleteRiskIn.TransactionType, bViaSAM:=True)

            If iRet = PMEReturnCode.PMNotFound Then
                Dim STSErrorEX As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.RiskRecordNotFound, "Risk record not found", DeleteRiskIn.RiskCnt.ToString)
                STSErrorEX.SetContext(oOutput.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Risk record not found", True)

                ' Terminate the reference to the GIS
                If oGIS IsNot Nothing Then
                    oGIS.Dispose()
                    oGIS = Nothing
                End If

                Return oOutput

            ElseIf iRet = PMEReturnCode.PMInvalidRiskStatus Then
                Dim STSErrorEX As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.StatusOfRiskPreventsDeletion, "Status of risk prevents deletion", DeleteRiskIn.RiskCnt.ToString)
                STSErrorEX.SetContext(oOutput.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Record Not deleted", True)

                ' Terminate the reference to the GIS
                If oGIS IsNot Nothing Then
                    oGIS.Dispose()
                    oGIS = Nothing
                End If

                Return oOutput

            ElseIf (iRet <> PMEReturnCode.PMTrue) Then
                ' End the recording
                Dim STSErrorEX As New STSErrorPublisher(iRet, "bGIS.STS.DeleteRisk failed")
                STSErrorEX.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, ACMethodName, True)
            End If

        Catch ex As Exception

            ' Terminate the reference to the GIS
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If

            Dim STSErrorEx As New STSErrorPublisher("An error occured calling bGIS.STS.DeleteRisk", ex)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, ACMethodName, True)

        End Try

        Return oOutput

    End Function


    Public Overloads Function GetQuoteRisks(ByVal con As SiriusConnection, ByVal GetQuoteRisksInput As GetQuoteRisksIn) As GetQuoteRisksOut

        'Dim oGIS As bGIS.QuotePolicy = Nothing
        Dim iRet As System.Int32
        Dim oOut As New GetQuoteRisksOut
        'Dim Utils As Utilities

        ' Local Variable for the results of the Call
        Dim vResultArray As Object = Nothing

        Dim ErrEx As Exception = Nothing

        Dim oGIS As bGIS.QuotePolicy = Nothing
        Try
            oGIS = New bGIS.QuotePolicy
        Catch ex As Exception
            ExceptionManager.Publish(ex)
            Debug.WriteLine(ex.Message)
        Finally
        End Try

        ' Initialise the GIS
        SAMFunc.InitialiseGISQP(Con:=con, oGIS:=oGIS, SiriusUser:=_SiriusUser)

        Try
            With GetQuoteRisksInput
                iRet = oGIS.GetQuoteRisks(
                    v_sGisDataModelCode:=CType(.DataModelCode, String),
                    v_sGisBusinessTypeCode:=CType(.BusinessTypeCode, String),
                    v_lInsuranceFileCnt:=CType(.InsuranceFileCnt, Integer),
                    r_vQuoteArray:=vResultArray)
            End With
        Catch ex As Exception
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
            ErrEx = New Exception("Failed to call bGIS.QuotePolicy.GetQuoteRisks", ex)
            ExceptionManager.Publish(ErrEx)
            Throw ErrEx
        Finally

            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If

            If (ErrEx Is Nothing) Then
                If (iRet <> PMEReturnCode.PMTrue) And (iRet <> PMEReturnCode.PMNotFound) Then
                    Dim ex As New Exception("bGIS.QuotePolicy.GetQuoteRisks FAILED. Return Value = " + iRet.ToString)
                    ExceptionManager.Publish(ex)
                    Throw ex
                End If
            End If
        End Try

        If IsArray(vResultArray) = True Then
            Dim arrColHeaders(,) As Object = {{"InsuranceFileKey", "RiskKey",
                "RiskDesc", "RiskTypeDesc", "CoverStartDate",
                "ExpiryDate", "RiskStatusDesc", "TotalSumInsured", "TotalThisPremium",
                "GISScreenKey", "RiskTypeKey", "InsuranceFolderKey", "StatusFlag",
                "RiskNumber", "VariationNumber", "IsRiskSelected",
                "Coverage", "InsuredItem", "Extensions", "ScreenCode", "DataModelCode",
                "TaxPercent", "TaxAmount", "CommAmount", "Comm"},
                {System.Type.GetType("System.Int32"), System.Type.GetType("System.Int32"),
                System.Type.GetType("System.String"), System.Type.GetType("System.String"),
                System.Type.GetType("System.DateTime"), System.Type.GetType("System.DateTime"),
                System.Type.GetType("System.String"), System.Type.GetType("System.Double"),
                System.Type.GetType("System.Double"), System.Type.GetType("System.Int32"),
                System.Type.GetType("System.Int32"), System.Type.GetType("System.Int32"),
                System.Type.GetType("System.String"), System.Type.GetType("System.Int32"),
                System.Type.GetType("System.Int32"), System.Type.GetType("System.Boolean"),
                System.Type.GetType("System.String"), System.Type.GetType("System.String"),
                System.Type.GetType("System.String"), System.Type.GetType("System.String"),
                System.Type.GetType("System.String"), System.Type.GetType("System.Double"),
                System.Type.GetType("System.Double"), System.Type.GetType("System.Double"),
                System.Type.GetType("System.String")}}

            ' Convert the Array into a Dataset
            oOut.Quotes = Utilities.ArrayToDataSet(vResultArray, arrColHeaders)
        Else
            oOut.Quotes = Nothing
        End If

        ' Return the Dataset
        Return oOut

    End Function

    Public Overloads Function LoadRiskFromDB(ByVal LoadRiskFromDBInput As LoadRiskFromDBIn) As LoadRiskFromDBOut

        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            Dim oResponse As LoadRiskFromDBOut

            oResponse = LoadRiskFromDB(con, LoadRiskFromDBInput)

            Return oResponse

        End Using

    End Function

    Public Overloads Function LoadRiskFromDB(ByVal con As SiriusConnection, ByVal LoadRiskFromDBInput As LoadRiskFromDBIn) As LoadRiskFromDBOut

        Dim iRet As System.Int32
        Dim oOut As LoadRiskFromDBOut

        'SAMFunc.STSLogMessage(TraceLevelSwitch, TraceLevel.Verbose, "Entering LoadRiskFromDB")
        'SAMFunc.STSLogMessageIndent(True)

        ' Create the Security Object
        Dim oGIS As bGIS.Application = Nothing
        Try
            oGIS = New bGIS.Application
        Catch ex As Exception
            Dim oSTSError As New STSErrorPublisher("Failed to get instance of bGIS.Application", ex)
            oSTSError.Raise("CoreBusiness", "LoadRiskFromDB", "Creating bGIS", True)
        End Try

        SAMFunc.InitialiseGIS(Con:=con, oGIS:=oGIS, SiriusUser:=_SiriusUser)

        ' Setup the OutputParameters
        oOut = New LoadRiskFromDBOut

        Try
            With LoadRiskFromDBInput
                iRet = oGIS.LoadRiskFromDB(
                                r_sXMLDataSetDef:= .XMLDataSetDef,
                                r_sXMLDataset:= .XMLDataSet,
                                r_sGISDataModelCode:= .DataModelCode,
                                v_lRiskID:= .RiskID,
                                v_lInsuranceFileCnt:= .InsuranceFileCnt)
                'v_lInsuranceFolderCnt:=.InsuranceFolderCnt)

            End With
        Catch ex As Exception
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
            SAMFunc.STSLogMessage(TraceLevelSwitch, TraceLevel.Verbose, "Error: LoadRiskFromDB caused an exception")
            Dim oSTSError As New STSErrorPublisher("Failed to call bGIS.Application.LoadRiskFromDB", ex)
            oSTSError.Raise("CoreBusiness", "LoadRiskFromDB", "LoadRiskFromDB", True)
        End Try

        If oGIS IsNot Nothing Then
            oGIS.Dispose()
            oGIS = Nothing
        End If

        If (iRet <> 1) Then
            SAMFunc.STSLogMessage(TraceLevelSwitch, TraceLevel.Verbose, "Error: LoadRiskFromDB returned " & iRet.ToString)
            Dim oSTSError As New STSErrorPublisher(iRet, "Failed to loadRiskFromDB")
            oSTSError.Raise("CoreBusiness", "LoadRiskFromDB", "LoadRiskFromDB", True)
        End If

        With LoadRiskFromDBInput
            oOut.DataModelCode = .DataModelCode
            oOut.InsuranceFileCnt = .InsuranceFileCnt
            oOut.InsuranceFolderCnt = .InsuranceFolderCnt
            oOut.RiskID = .RiskID
            oOut.XMLDataSet = .XMLDataSet
            oOut.XMLDataSetDef = .XMLDataSetDef
        End With

        'SAMFunc.STSLogMessageIndent(False)
        'SAMFunc.STSLogMessage(TraceLevelSwitch, TraceLevel.Verbose, "Leaving LoadRiskFromDB")

        ' Return the Data
        Return oOut

    End Function

    Public Function ProcessInstalments(
        ByVal ProcessInstalmentsInput As BaseBindQuoteRequestType,
        ByVal TransactionArray As Object) As BaseBindQuoteResponseType

        Const ACMethodName As String = "ProcessInstalments"
        Const k_PFQuotePFRF_ID As Integer = 32

        Dim oReturn As New BaseImplementationTypes.BaseBindQuoteResponseType

        Dim STSError As New STSErrorPublisher

        Dim iRet As Integer
        Dim iSourceId As Integer

        iSourceId% = 1
        ' Convert branch code to ID
        Try

            iSourceId% = GetListItemFromCode(
                Core.STSListType.PMLookup,
                "Source",
                ProcessInstalmentsInput.BranchCode)

        Catch ex As Exception
            STSError.AddInvalidField(
                "BranchCode",
                STSErrorPublisher.STSErrorCodes.BranchCodeInvalid.ToString,
                [String].Format(STSErrorPublisher.MandatoryInputInvalid,
                "BranchCode"),
                ProcessInstalmentsInput.BranchCode)
        End Try

        ' Create the Application Object

        Dim oPremiumFinance As bSIRPremiumFinance.Business = Nothing
        Try
            oPremiumFinance = New bSIRPremiumFinance.Business
        Catch ex As Exception
            ExceptionManager.Publish(ex)
            Debug.WriteLine(ex.Message)
        Finally
        End Try

        ' Initialise the GIS
        SAMFunc.InitialiseSBOObject(
            oObject:=oPremiumFinance,
            siriusUser:=_SiriusUser,
            sObjectName:="bSIRPremiumFinance.Business")

        Dim arrQuoteArrayObject As Object = Nothing
        Dim arrQuoteArray As Object(,) = Nothing

        ' need to indicate that this isnt view as the back office component
        ' now explicitly checks that task.... when calculating the instalments
        ' amounts
        Dim objectTask As Object = PMEComponentAction.PMEdit
        oPremiumFinance.SetProcessModes(vTask:=objectTask)

        ' Call the Method
        Try

            oPremiumFinance.InsuranceFileCnt = ProcessInstalmentsInput.InsuranceFileKey

            With ProcessInstalmentsInput

                oPremiumFinance.UseTransCurrency = 0

                If ProcessInstalmentsInput.IsUseTransactionCurrency Then
                    oPremiumFinance.UseTransCurrency = 1
                End If

                iRet = oPremiumFinance.Calculate_Quotes(
                    v_lSourceID:=iSourceId%,
                    v_sProductCode:=gPMConstants.PMTypeOfBusinessNB,
                    v_dtQuoteDate:=Date.MinValue,
                    v_dtStartDate:=Date.MinValue,
                    v_dtEndDate:=Date.MinValue,
                    v_dtPreferredDate:= .PreferredDate,
                    v_iDayInMonth:=CShort(.MonthDay),
                    v_iDayInWeek:=CShort(.WeekDay),
                    v_crAmountToFinance:=CDec(Cast.DefaultIfNull(.AmountToFinance, 0)),
                    v_bPaymentProtection:= .PaymentProtection,
                    v_dInterestOverrideRate:= .OverrideInterestRate,
                    v_bOverrideCommission:=False,
                    v_lPartyCnt:=0,
                    r_vQuoteArray:=arrQuoteArrayObject,
                    v_lInsuranceFileCnt:=ProcessInstalmentsInput.InsuranceFileKey)

            End With

            arrQuoteArray = DirectCast(arrQuoteArrayObject, Object(,))

        Catch ex As Exception
            If oPremiumFinance IsNot Nothing Then
                oPremiumFinance.Dispose()
                oPremiumFinance = Nothing
            End If
            STSError = New STSErrorPublisher(iRet, "The call to PremiumFinance.Calculate_Quotes failed")
            STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "bSIRPremiumFinance.Business", True)
        Finally

            If (STSError Is Nothing) Then
                If (iRet <> PMEReturnCode.PMTrue) Then
                    If oPremiumFinance IsNot Nothing Then
                        oPremiumFinance.Dispose()
                        oPremiumFinance = Nothing
                    End If
                    STSError = New STSErrorPublisher(iRet, "The call to PremiumFinance.Calculate_Quotes failed")
                    STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "bSIRPremiumFinance.Business", True)
                ElseIf IsArray(arrQuoteArray) = False Then
                    If oPremiumFinance IsNot Nothing Then
                        oPremiumFinance.Dispose()
                        oPremiumFinance = Nothing
                    End If
                    STSError = New STSErrorPublisher(iRet, "Failed to regenerate the selected Premium Finance Quotation for Insurance File Key - " & ProcessInstalmentsInput.InsuranceFileKey & ",  Scheme No - " & ProcessInstalmentsInput.SelectedSchemeNo & ", and Scheme Version - " & ProcessInstalmentsInput.SelectedSchemeVersion)
                    STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "bSIRPremiumFinance.Business", True)
                End If
            End If

        End Try

        Dim lPremiumFinanceCnt As Integer = 0
        Dim lPremiumFinanceVer As Integer = 0
        Dim oPFArrayObject As System.Array(,) = Nothing
        Dim oPFArray As System.Array(,)
        Dim iCnt As Integer = 0
        Dim blSchemeFound As Boolean = False
        Dim iUbnd As Integer = arrQuoteArray.GetUpperBound(1)
        Dim iLbnd As Integer = arrQuoteArray.GetLowerBound(1)

        For iCnt = iLbnd To iUbnd

            If Cast.ToInt32(arrQuoteArray(k_PFQuotePFRF_ID, iCnt), 0) = ProcessInstalmentsInput.PFRF_ID Then
                blSchemeFound = True
                Exit For
            End If

        Next

        If blSchemeFound = True Then

            iRet = oPremiumFinance.InsertOrUpdatePremiumFinance(
                arrQuoteArray,
                iCnt,
                Nothing,
                lPremiumFinanceCnt,
                lPremiumFinanceVer)

            If (iRet <> PMEReturnCode.PMTrue) Then
                If oPremiumFinance IsNot Nothing Then
                    oPremiumFinance.Dispose()
                    oPremiumFinance = Nothing
                End If
                STSError = New STSErrorPublisher(BackOfficeReturnValue:=iRet, Description:="The call to PremiumFinance.InsertOrUpdatePremiumFinance failed.  See the Sirius backoffice log for further details")
                STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "bSIRPremiumFinance.Business", True)
            End If

            iRet = oPremiumFinance.GetSingleFinancePlan(
                lPremiumFinanceCnt,
                lPremiumFinanceVer,
                CType(CObj(oPFArrayObject), Object(,)))

            oPFArray = oPFArrayObject
            If (iRet <> PMEReturnCode.PMTrue) Then
                If oPremiumFinance IsNot Nothing Then
                    oPremiumFinance.Dispose()
                    oPremiumFinance = Nothing
                End If
                STSError = New STSErrorPublisher(BackOfficeReturnValue:=iRet, Description:="The call to PremiumFinance.GetSingleFinancePlan failed.  See the Sirius backoffice log for further details")
                STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "bSIRPremiumFinance.Business", True)
            ElseIf IsArray(oPFArray) = False Then
                If oPremiumFinance IsNot Nothing Then
                    oPremiumFinance.Dispose()
                    oPremiumFinance = Nothing
                End If
                STSError = New STSErrorPublisher(iRet, "PremiumFinance.GetSingleFinancePlan failed to retrieve the Finance Plane for Premium Finance record - " & lPremiumFinanceCnt)
                STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "bSIRPremiumFinance.Business", True)
            End If

            'Update to bank details etc.
            oPFArray.SetValue(ProcessInstalmentsInput.BankName, k_PFPlanBankName, 0)
            oPFArray.SetValue(ProcessInstalmentsInput.BankSortCode, k_PFPlanBankSortCode, 0)
            oPFArray.SetValue(ProcessInstalmentsInput.BankAccountNo, k_PFPlanBankAccountNo, 0)
            oPFArray.SetValue(ProcessInstalmentsInput.BankAccountName, k_PFPlanBankAccountName, 0)
            oPFArray.SetValue(ProcessInstalmentsInput.BankBranch, k_PFPlanBankBranch, 0)
            oPFArray.SetValue(ProcessInstalmentsInput.BIC, bSIRPremFinConst.kBIC, 0)
            oPFArray.SetValue(ProcessInstalmentsInput.IBAN, bSIRPremFinConst.kIBAN, 0)

            If ProcessInstalmentsInput.BankAddress Is Nothing = False Then
                oPFArray.SetValue(ProcessInstalmentsInput.BankAddress.AddressLine1, k_PFPlanBankAddress1, 0)
                oPFArray.SetValue(ProcessInstalmentsInput.BankAddress.AddressLine2, k_PFPlanBankAddress2, 0)
                oPFArray.SetValue(ProcessInstalmentsInput.BankAddress.AddressLine3, k_PFPlanBankAddress3, 0)
                oPFArray.SetValue(ProcessInstalmentsInput.BankAddress.AddressLine4, k_PFPlanBankAddress4, 0)
                oPFArray.SetValue(ProcessInstalmentsInput.BankAddress.PostCode, k_PFPlanBankPostcode, 0)
                oPFArray.SetValue(ProcessInstalmentsInput.BankAddress.CountryCode, k_PFPlanBankCountry, 0)
            End If

            oPFArray.SetValue(ProcessInstalmentsInput.BankAreaCode, k_PFPlanBankAreaCode, 0)
            oPFArray.SetValue(ProcessInstalmentsInput.BankPhone, k_PFPlanBankPhone, 0)
            oPFArray.SetValue(ProcessInstalmentsInput.BankExtn, k_PFPlanBankExtn, 0)
            oPFArray.SetValue(ProcessInstalmentsInput.BankFaxCode, k_PFPlanBankFaxCode, 0)
            oPFArray.SetValue(ProcessInstalmentsInput.BankFax, k_PFPlanBankFax, 0)

            ' populate the credit card details into the array 
            ' if they are present in the request
            PopulateCreditCardDetails(ProcessInstalmentsInput, CType(CObj(oPFArray), Object(,)))

            Dim lPremiumFinanceCntObject As Integer = lPremiumFinanceCnt
            Dim lPremiumFinanceVerObject As Integer = lPremiumFinanceVer
            oPFArrayObject = oPFArray
            iRet = oPremiumFinance.UpdateExistingRecord(
                CType(CObj(oPFArrayObject), Object(,)),
                lPremiumFinanceCntObject,
                lPremiumFinanceVerObject)

            If (iRet <> PMEReturnCode.PMTrue) Then
                If oPremiumFinance IsNot Nothing Then
                    oPremiumFinance.Dispose()
                    oPremiumFinance = Nothing
                End If
                STSError = New STSErrorPublisher(BackOfficeReturnValue:=iRet, Description:="The call to PremiumFinance.UpdateExistingRecord failed.  See the Sirius backoffice log for further details")
                STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "bSIRPremiumFinance.Business", True)
            End If

            'oPFArray = DirectCast(oPFArray, Object(,))
            lPremiumFinanceCnt = Cast.ToInt32(lPremiumFinanceCntObject, 0)
            lPremiumFinanceVer = Cast.ToInt32(lPremiumFinanceVerObject, 0)

            ' create initial bank history record
            iRet = oPremiumFinance.SaveInstalmentsPlanMediaTypeDetails(
                v_lPfPremFinanceCnt:=lPremiumFinanceCnt,
                v_lPfPremFinanceVersion:=lPremiumFinanceVer,
                v_sActionCode:=InstalmentHistoryActionType.Setup)

            If (iRet <> PMEReturnCode.PMTrue) Then
                If oPremiumFinance IsNot Nothing Then
                    oPremiumFinance.Dispose()
                    oPremiumFinance = Nothing
                End If
                STSError = New STSErrorPublisher(BackOfficeReturnValue:=iRet, Description:="The call to PremiumFinance.SaveInstalmentsPlanMediaTypeDetails failed. See the Sirius backoffice log for further details")
                STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "bSIRPremiumFinance.Business", True)
            End If

            Dim bAllocationExist As Boolean
            iRet = oPremiumFinance.CheckAllocationAgainstPolicy(
                v_lInsuranceFileCnt:=ProcessInstalmentsInput.InsuranceFileKey,
                r_bAllocationExist:=bAllocationExist)

            If iRet <> PMEReturnCode.PMTrue Then
                If oPremiumFinance IsNot Nothing Then
                    oPremiumFinance.Dispose()
                    oPremiumFinance = Nothing
                End If
                STSError = New STSErrorPublisher(BackOfficeReturnValue:=iRet, Description:="The call to PremiumFinance.CheckAllocationAgainstPolicy failed.  See the Sirius backoffice log for further details")
                STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "bSIRPremiumFinance.Business.CheckAllocationAgainstPolicy", True)
            End If

            If bAllocationExist = True Then
                If oPremiumFinance IsNot Nothing Then
                    oPremiumFinance.Dispose()
                    oPremiumFinance = Nothing
                End If
                STSError = New STSErrorPublisher(BackOfficeReturnValue:=iRet, Description:="Payment Allocation against this Policy already exists.  The instalment plan cannot be transacted at this stage.")
                STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "bSIRPremiumFinance.Business.CheckAllocationAgainstPolicy", True)
            End If

            iRet = oPremiumFinance.DeletePFTransID(
                v_lPremFinanceCnt:=lPremiumFinanceCnt,
                v_lPremFinanceVersion:=lPremiumFinanceVer)

            If (iRet <> PMEReturnCode.PMTrue) Then
                If oPremiumFinance IsNot Nothing Then
                    oPremiumFinance.Dispose()
                    oPremiumFinance = Nothing
                End If
                STSError = New STSErrorPublisher(BackOfficeReturnValue:=iRet, Description:="The call to PremiumFinance.DeletePFTransID failed.  See the Sirius backoffice log for further details")
                STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "bSIRPremiumFinance.Business.DeletePFTransID", True)
            End If

            iRet = oPremiumFinance.InsertPFTransID(
                v_lPremFinanceCnt:=lPremiumFinanceCnt,
                v_lPremFinanceVersion:=lPremiumFinanceVer,
                v_vPFTransArray:=CType(CObj(TransactionArray), Object(,)))

            If (iRet <> PMEReturnCode.PMTrue) Then
                If oPremiumFinance IsNot Nothing Then
                    oPremiumFinance.Dispose()
                    oPremiumFinance = Nothing
                End If
                STSError = New STSErrorPublisher(BackOfficeReturnValue:=iRet, Description:="The call to PremiumFinance.InsertPFTransID failed.  See the Sirius backoffice log for further details")
                STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "bSIRPremiumFinance.Business.InsertPFTransID", True)
            End If

            iRet = oPremiumFinance.ProcessPlan(
                lMode:=3,
                v_vPremiumFinanceArray:=oPFArray,
                v_vTransArray:=CType(CObj(TransactionArray), Object(,)),
                v_iMTAType:=0)

            If (iRet <> PMEReturnCode.PMTrue) Then
                If oPremiumFinance IsNot Nothing Then
                    oPremiumFinance.Dispose()
                    oPremiumFinance = Nothing
                End If
                STSError = New STSErrorPublisher(BackOfficeReturnValue:=iRet, Description:="The call to PremiumFinance.ProcessPlan failed.  See the Sirius backoffice log for further details")
                STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "bSIRPremiumFinance.Business.ProcessPlan", True)
            End If

            lPremiumFinanceCntObject = lPremiumFinanceCnt
            lPremiumFinanceVerObject = lPremiumFinanceVer

            ' Update the plans status to live
            iRet = oPremiumFinance.StatusUpdate(
                vPremiumFinanceCnt:=lPremiumFinanceCntObject,
                vPremiumFinanceVersion:=lPremiumFinanceVerObject,
                vStatusInd:=InstalmentPlanStatus.Live)

            If (iRet <> PMEReturnCode.PMTrue) Then
                If oPremiumFinance IsNot Nothing Then
                    oPremiumFinance.Dispose()
                    oPremiumFinance = Nothing
                End If
                STSError = New STSErrorPublisher(BackOfficeReturnValue:=iRet, Description:="Failed to update the Finance Plan Status to Live.  See the Sirius backoffice log for further details")
                STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "bSIRPremiumFinance.Business.StatusUpdate", True)
            End If

            lPremiumFinanceCnt = Cast.ToInt32(lPremiumFinanceCntObject, 0)
            lPremiumFinanceVer = Cast.ToInt32(lPremiumFinanceVerObject, 0)

        End If

        ' Terminate the Com reference
        If oPremiumFinance IsNot Nothing Then
            oPremiumFinance.Dispose()
            oPremiumFinance = Nothing
        End If

        Return oReturn

    End Function

    Private Sub PopulateCreditCardDetails(
    ByRef request As BaseBindQuoteRequestType,
    ByRef premiumFinanceDetailsArray As Object(,))

        If request.PaymentMethodSpecified = True AndAlso
            request.PaymentMethod = PaymentMethodType.CreditCard Then

            If request.CreditCard IsNot Nothing Then

                premiumFinanceDetailsArray(k_PFPlanBankAccountName, 0) = request.CreditCard.NameOnCreditCard
                premiumFinanceDetailsArray(k_PFPlanCCNumber, 0) = request.CreditCard.Number
                premiumFinanceDetailsArray(k_PFPlanCCExpiryDate, 0) = request.CreditCard.ExpiryDate
                premiumFinanceDetailsArray(k_PFPlanCCStartDate, 0) = request.CreditCard.StartDate
                premiumFinanceDetailsArray(k_PFPlanCCIssue, 0) = request.CreditCard.Issue
                premiumFinanceDetailsArray(k_PFPlanCCPin, 0) = request.CreditCard.Pin
                premiumFinanceDetailsArray(k_PFPlanCardType, 0) = request.CreditCard.TypeCode

                premiumFinanceDetailsArray(k_PFPlanIsCardholder, 0) = request.CreditCard.CardHolder Is Nothing

                If request.CreditCard.CardHolder IsNot Nothing Then

                    premiumFinanceDetailsArray(k_PfPlanCardholderName, 0) = request.CreditCard.CardHolder.Name
                    premiumFinanceDetailsArray(k_PfPlanCardholderAddress1, 0) = request.CreditCard.CardHolder.AddressLine1
                    premiumFinanceDetailsArray(k_PfPlanCardholderAddress2, 0) = request.CreditCard.CardHolder.AddressLine2
                    premiumFinanceDetailsArray(k_PfPlanCardholderAddress3, 0) = request.CreditCard.CardHolder.AddressLine3
                    premiumFinanceDetailsArray(k_PfPlanCardholderAddress4, 0) = request.CreditCard.CardHolder.AddressLine4
                    premiumFinanceDetailsArray(k_PfPlanCardholderPostcode, 0) = request.CreditCard.CardHolder.PostCode
                End If

            End If

        End If

    End Sub

    Public Function RunDefaultRulesEdit(ByVal oDefaultRulesIn As DefaultRulesInput) As DefaultRulesOutput

        '' SiriusPerfCounters must be declared at the TOP of the method
        'Dim oCounters As New SiriusPerfCounters("RunDefaultsEdit")

        Dim oOut As New DefaultRulesOutput

        Dim oInQuote As New NBQuoteIn
        Dim oOutQuote As New NBQuoteOut

        ' Construct the input parameters
        With oInQuote
            .AdditionalDataArray = oDefaultRulesIn.AdditionalDataArray
            .BusinessTypeCode = oDefaultRulesIn.BusinessTypeCode
            .DataModelCode = oDefaultRulesIn.DataModelCode
            .EffectiveDate = oDefaultRulesIn.EffectiveDate
            .GISSchemeID = oDefaultRulesIn.GISSchemeID
            .RiskGroupID = oDefaultRulesIn.RiskGroupID
            .RiskScreenId = oDefaultRulesIn.RiskScreenId
            .XMLDataset = oDefaultRulesIn.XMLDataset
        End With

        ' Call NBQuote to run the default rules
        Try
            oOutQuote = RunQuote(NBQuoteInput:=oInQuote,
                                 lType:=QuoteTypeRules.PreScreen)
        Catch oe As Exception
            ' End the recording
            'oCounters.FailMethod()
            ExceptionManager.Publish(oe)
            Throw New Exception("Failed to run default rules in edit mode.", oe)
        End Try

        ' Grab the output
        With oOut
            .XMLDataset = oOutQuote.XMLDataset
        End With

        ' End the recording
        'oCounters.EndMethod()

        ' Return the dataset
        Return oOut

    End Function

    Public Function RunDefaultRulesAdd(ByVal oDefaultRulesIn As DefaultRulesInput) As DefaultRulesOutput

        '' SiriusPerfCounters must be declared at the TOP of the method
        'Dim oCounters As New SiriusPerfCounters("RunDefaultsAdd")

        Dim oOut As New DefaultRulesOutput

        Dim oInQuote As New NBQuoteIn
        Dim oOutQuote As New NBQuoteOut

        ' Construct the input parameters
        With oInQuote
            .AdditionalDataArray = oDefaultRulesIn.AdditionalDataArray
            .BusinessTypeCode = oDefaultRulesIn.BusinessTypeCode
            .DataModelCode = oDefaultRulesIn.DataModelCode
            .EffectiveDate = oDefaultRulesIn.EffectiveDate
            .GISSchemeID = oDefaultRulesIn.GISSchemeID
            .RiskGroupID = oDefaultRulesIn.RiskGroupID
            .RiskScreenId = oDefaultRulesIn.RiskScreenId
            .XMLDataset = oDefaultRulesIn.XMLDataset
        End With

        ' Call NBQuote to run the default rules
        Try
            oOutQuote = RunQuote(NBQuoteInput:=oInQuote,
                                 lType:=QuoteTypeRules.Default)
        Catch oe As Exception
            '' End the recording
            'oCounters.FailMethod()
            ExceptionManager.Publish(oe)
            Throw New Exception("Failed to run default rules in add mode.", oe)
        End Try

        ' Grab the output
        With oOut
            .XMLDataset = oOutQuote.XMLDataset
        End With

        ' End the recording
        'oCounters.EndMethod()

        ' Return the dataset
        Return oOut

    End Function

    Public Function RunValidationRules(ByVal oValidationRulesIn As ValidationRulesInput, Optional ByVal oSiriusUser As SIRIUSUSER = Nothing) As ValidationRulesOutput

        '' SiriusPerfCounters must be declared at the TOP of the method
        'Dim oCounters As New SiriusPerfCounters("RunValidationsEdit")

        Dim oOut As New ValidationRulesOutput

        Dim oInQuote As New NBQuoteIn
        Dim oOutQuote As New NBQuoteOut

        If oSiriusUser IsNot Nothing Then
            _SiriusUser = oSiriusUser
        End If
        ' Call NBQuote to run the Validation rules
        oOutQuote = RunQuote(NBQuoteInput:=oValidationRulesIn,
                                 lType:=QuoteTypeRules.Validate)

        ' Grab the output
        With oOut
            .XMLDataset = oOutQuote.XMLDataset
        End With

        ' End the recording
        'oCounters.EndMethod()

        ' Return the dataset
        Return oOut

    End Function

    Public Overloads Function RunQuote(ByVal NBQuoteInput As NBQuoteIn,
                                       ByVal lType As Int32) As NBQuoteOut

        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)

            Dim oResponse As NBQuoteOut

            oResponse = RunQuote(con, NBQuoteInput, lType)

            Return oResponse

        End Using

    End Function

    Public Overloads Function RunQuote(ByVal con As SiriusConnection, ByVal NBQuoteInput As NBQuoteIn,
                                       ByVal lType As Int32) As NBQuoteOut

        Dim oResponse As NBQuoteOut

        Dim transactionType As String = gPMConstants.PMTypeOfBusinessNB
        Dim transactionTypeID As Integer = 0

        If (NBQuoteInput.AdditionalDataArray Is Nothing) = False Then
            ' Find the Transaction Type in the Additional Data Arry
            For Each dataItem As AdditionalData In NBQuoteInput.AdditionalDataArray
                If dataItem.Name = "TRANSACTION_TYPE" Then
                    transactionType = dataItem.Value.ToString
                    Exit For
                End If
            Next
        End If

        If NBQuoteInput.ClaimTransactiontypeId <> 0 And (NBQuoteInput.AdditionalDataArray Is Nothing) Then
            ' transactionType - pmproduct lookup
            transactionTypeID = GetAndValidateListItemFromId(Core.STSListType.PMLookup, PMLookupTable.TransactionType, NBQuoteInput.ClaimTransactiontypeId, "TransactionType")
        Else
            transactionTypeID = GetAndValidateListItemFromCode(Core.STSListType.PMLookup, PMLookupTable.TransactionType, transactionType, "TransactionType")
        End If
        oResponse = RunQuote(con, NBQuoteInput, lType, transactionType, transactionTypeID)

        Return oResponse

    End Function

    Public Overloads Function RunQuote(
    ByVal con As SiriusConnection,
    ByVal NBQuoteInput As NBQuoteIn,
    ByVal lType As Int32,
    ByVal transactionType As String,
    ByVal transactionTypeID As Integer) As NBQuoteOut

        Dim iRet As System.Int32
        Dim oOut As New NBQuoteOut
        Dim lQuoteType As Int32
        Dim ErrEx As Exception = Nothing

        ' Create the Application Object

        Dim oGIS As bGIS.Application = Nothing
        Try
            oGIS = New bGIS.Application
        Catch ex As Exception
            ExceptionManager.Publish(ex)
            Debug.WriteLine(ex.Message)
        Finally
        End Try

        ' Initialise the GIS
        SAMFunc.InitialiseGIS(Con:=con, oGIS:=oGIS, SiriusUser:=_SiriusUser)

        iRet = oGIS.SetProcessModes(SAMComponentAction.PMEdit, 0, 0, Cast.ToObjString(transactionType), Now)

        ' CTAF 20030321 - Add the CalledFromSTS value
        Try
            Utilities.AddCallFromSTS(NBQuoteInput.AdditionalDataArray)
        Catch oe As Exception
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
            ExceptionManager.Publish(oe)
            Throw New Exception("Failed to add parameter to AdditionalDataArray", oe)
        End Try

        ' Dimension the Array
        Dim vAdditionalData As Object = Nothing

        ' Move the Data from the Classes to the Array
        vAdditionalData = Utilities.ClassesToArray(NBQuoteInput.AdditionalDataArray)

        'Calculate the QuoteType value
        Utilities.EncodeTransactionScreenAndType(r_lEncoded:=lQuoteType,
                                             v_lTransactionType:=transactionTypeID,
                                             v_lGISScreenId:=NBQuoteInput.RiskScreenId,
                                             v_lQuoteType:=lType)
        If NBQuoteInput.isClaimValidation Then
            vAdditionalData = Nothing
        End If
        ' Call the Method
        Try

            iRet = oGIS.NBQuote(
                v_sGisDataModelCode:=NBQuoteInput.DataModelCode,
                v_sGisBusinessTypeCode:=NBQuoteInput.BusinessTypeCode,
                v_lQuoteType:=lQuoteType,
                v_dtEffectiveDate:=NBQuoteInput.EffectiveDate,
                r_sXMLDataset:=NBQuoteInput.XMLDataset,
                v_lGISSchemeID:=NBQuoteInput.GISSchemeID,
                v_lRiskGroupID:=NBQuoteInput.RiskGroupID,
                r_vAdditionalDataArray:=vAdditionalData)

        Catch ex As Exception
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
            ErrEx = New Exception("Failed to call bGIS.Application.NBQuote", ex)
            ExceptionManager.Publish(ErrEx)
            Throw ErrEx
        Finally
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
            If (ErrEx Is Nothing) Then
                If (iRet <> PMEReturnCode.PMTrue) Then
                    If (iRet = PMEReturnCode.PMNBQuoteReferred) Then
                        iRet = 279
                    ElseIf (iRet = PMEReturnCode.PMNBQuoteDeclined) Then
                        iRet = 280
                    ElseIf (iRet = PMEReturnCode.PMPREFailed) Then
                        iRet = 281
                    Else
                        ErrEx = New Exception("bGIS.Application.NBQuote FAILED. Return Value = " + iRet.ToString)
                        ExceptionManager.Publish(ErrEx)
                        Throw ErrEx
                    End If
                End If

            End If
        End Try

        ' Copy the Outputs from the In/Outs
        oOut.XMLDataset = NBQuoteInput.XMLDataset

        ' Create the return Additional Data
        oOut.AdditionalDataArray = Utilities.ArrayToClasses(vAdditionalData)

        Return oOut

    End Function

    Public Overloads Function SaveToDB(ByVal SaveToDBInput As SaveToDBIn) As SaveToDBOut

        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)

            Dim oResponse As SaveToDBOut

            oResponse = SaveToDB(con, SaveToDBInput)

            Return oResponse

        End Using

    End Function

    Public Overloads Function SaveToDB(ByVal con As SiriusConnection, ByVal SaveToDBInput As SaveToDBIn) As SaveToDBOut

        Dim iRet As System.Int32
        Dim oOut As SaveToDBOut
        'Dim Utils As Utilities

        Dim ErrEx As Exception = Nothing

        ' Create the Application Object

        Dim oGIS As bGIS.Application = Nothing
        Try
            oGIS = New bGIS.Application
        Catch ex As Exception
            ExceptionManager.Publish(ex)
            Debug.WriteLine(ex.Message)
        Finally
        End Try

        SAMFunc.InitialiseGIS(Con:=con, oGIS:=oGIS, SiriusUser:=_SiriusUser)

        ' Setup the OutputParameters
        oOut = New SaveToDBOut

        ' Call the Method
        Try
            With SaveToDBInput
                iRet = oGIS.SaveToDB(
                     v_sGisDataModelCode:=CType(.DataModelCode, String),
                     r_sXMLDataset:= .XMLDataset)
            End With
        Catch ex As Exception
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
            ErrEx = New Exception("Failed to save the risk data to the database.  The call to bGIS.Application.SaveToDB failed.  Please check the Sirius Logs for more information.", ex)
            ExceptionManager.Publish(ErrEx)
            Throw ErrEx
        Finally
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
            If (ErrEx Is Nothing) Then
                If (iRet <> 1) Then
                    ErrEx = New Exception("Failed to save the risk data to the database.  The call to bGIS.Application.SaveToDB failed.  Please check the Sirius Logs for more information. Return Value = " + iRet.ToString)
                    ExceptionManager.Publish(ErrEx)
                    Throw ErrEx
                End If
            End If
        End Try

        ' Setup the Parameters
        ' In/Outs
        With SaveToDBInput
            oOut.XMLDataset = .XMLDataset
        End With

        ' Return the Data
        Return oOut

    End Function

    Public Overloads Function NBTransact(ByVal NBTransactInput As NBTransactIn) As NBTransactOut
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)

            Dim oResponse As NBTransactOut

            oResponse = NBTransact(con, NBTransactInput)

            Return oResponse

        End Using

    End Function

    Public Overloads Function NBTransact(ByVal con As SiriusConnection, ByVal NBTransactInput As NBTransactIn) As NBTransactOut

        Const ACMethodName As String = "NBTransact"

        'SAMFunc.STSLogMessage(TraceLevelSwitch, TraceLevel.Verbose, "Creating NBTransact Performance Counter")

        Dim iRet As Int32
        Dim oOut As New NBTransactOut
        'Dim Utils As Utilities

        ' Local Variable for the results of the Call
        Dim vResultArray As Object = Nothing

        'Dim ErrEx As Exception = Nothing

        'SAMFunc.STSLogMessage(TraceLevelSwitch, TraceLevel.Verbose, "Entering NBTransact")
        'SAMFunc.STSLogMessageIndent(True)

        Dim oGIS As bGIS.Application = Nothing
        Try
            oGIS = New bGIS.Application
        Catch ex As Exception
            ' End the recording
            'oCounters.FailMethod()
            Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.FailedToCreateBackofficeComponent, "Failed to create bGIS.Application", ex.Message)
            STSErrorEx.SetContext(oOut.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "CreateBusiness", True)
            Return oOut
        End Try

        ' Initialise the GIS
        SAMFunc.InitialiseGIS(Con:=con, oGIS:=oGIS, SiriusUser:=_SiriusUser)

        ' CTAF 20030321 - Add the CalledFromSTS value
        Try
            Utilities.AddCallFromSTS(NBTransactInput.AdditionalDataArray)
        Catch oe As Exception
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
            ' End the recording
            'oCounters.FailMethod()
            Dim STSErrorFileError As New STSErrorPublisher("An error occured attempting to call Utilities.AddCallFromSTS", oe)
            STSErrorFileError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "AddSTSCall", True)
        End Try

        ' Set up the extra data needed to transact under a broking system
        'Dim oData As AdditionalData

        With NBTransactInput

            'oData = New AdditionalData
            'oData.Name = "NB_InsuranceFileCnt"
            'oData.Value = .InsuranceFileCnt
            'ReDim Preserve .AdditionalDataArray(Microsoft.VisualBasic.UBound(.AdditionalDataArray) + 1)
            '.AdditionalDataArray(Microsoft.VisualBasic.UBound(.AdditionalDataArray)) = oData

            'oData = New AdditionalData
            'oData.Name = "NB_DebitCredit"
            'oData.Value = .DebitCredit
            'ReDim Preserve .AdditionalDataArray(Microsoft.VisualBasic.UBound(.AdditionalDataArray) + 1)
            '.AdditionalDataArray(Microsoft.VisualBasic.UBound(.AdditionalDataArray)) = oData

            'oData = New AdditionalData
            'oData.Name = "NB_LastTransType"
            'oData.Value = .LastTransType
            'ReDim Preserve .AdditionalDataArray(Microsoft.VisualBasic.UBound(.AdditionalDataArray) + 1)
            '.AdditionalDataArray(Microsoft.VisualBasic.UBound(.AdditionalDataArray)) = oData

            'oData = New AdditionalData
            'oData.Name = "NB_LastTransDate"
            'oData.Value = .LastTransDate
            'ReDim Preserve .AdditionalDataArray(Microsoft.VisualBasic.UBound(.AdditionalDataArray) + 1)
            '.AdditionalDataArray(Microsoft.VisualBasic.UBound(.AdditionalDataArray)) = oData

            'oData = New AdditionalData
            'oData.Name = "NB_PolicyStartDate"
            'oData.Value = .PolicyStartDate
            'ReDim Preserve .AdditionalDataArray(Microsoft.VisualBasic.UBound(.AdditionalDataArray) + 1)
            '.AdditionalDataArray(Microsoft.VisualBasic.UBound(.AdditionalDataArray)) = oData

            'oData = New AdditionalData
            'oData.Name = "NB_PostingAmount"
            'oData.Value = .PostingAmount
            'ReDim Preserve .AdditionalDataArray(Microsoft.VisualBasic.UBound(.AdditionalDataArray) + 1)
            '.AdditionalDataArray(Microsoft.VisualBasic.UBound(.AdditionalDataArray)) = oData

            'oData = New AdditionalData
            'oData.Name = "NB_Reason"
            'oData.Value = .Reason
            'ReDim Preserve .AdditionalDataArray(Microsoft.VisualBasic.UBound(.AdditionalDataArray) + 1)
            '.AdditionalDataArray(Microsoft.VisualBasic.UBound(.AdditionalDataArray)) = oData

            'oData = New AdditionalData
            'oData.Name = "NB_RealInsuranceFileCnt"
            'oData.Value = .RealInsuranceFileCnt
            'ReDim Preserve .AdditionalDataArray(Microsoft.VisualBasic.UBound(.AdditionalDataArray) + 1)
            '.AdditionalDataArray(Microsoft.VisualBasic.UBound(.AdditionalDataArray)) = oData

        End With

        ' Dimension the Array
        Dim vAdditionalData As Object = Nothing

        ' Move the Data from the Classes to the Array
        vAdditionalData = Utilities.ClassesToArray(NBTransactInput.AdditionalDataArray)

        ' Call the Method
        Try
            With NBTransactInput
                iRet = oGIS.NBTransact(v_sGisDataModelCode:=CType(.sGisDataModelCode, String),
                                       v_sGisBusinessTypeCode:=CType(.sGisBusinessTypeCode, String),
                                       v_lGISSchemeID:=CType(.lGISSchemeID, Int32),
                                       r_sXMLDataset:= .sXMLDataSet,
                                       r_vAdditionalDataArray:=vAdditionalData)

            End With

            If (iRet <> PMEReturnCode.PMTrue) Then
                If oGIS IsNot Nothing Then
                    oGIS.Dispose()
                    oGIS = Nothing
                End If

                Dim NBTransactsMessage As String = String.Empty

                oOut.AdditionalDataArray = Utilities.ArrayToClasses(vAdditionalData)

                'oCounters.FailMethod()
                If oOut.AdditionalDataArray IsNot Nothing Then
                    For Each AdditionalDataItem As AdditionalData In oOut.AdditionalDataArray
                        If AdditionalDataItem.Name = InternalSAMConstants.CNNBTransactMessage Then
                            NBTransactsMessage = AdditionalDataItem.Value.ToString
                        End If
                    Next
                End If

                Dim STSErrorFileError As New STSErrorPublisher(iRet, "bGIS.Application.NBTransact failed: " & NBTransactsMessage)
                STSErrorFileError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "NBTransactReturn", True)
            End If
        Catch ex As Exception
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
            'oCounters.FailMethod()

            Dim STSErrorFileError As New STSErrorPublisher("bGIS.Application.NBTransact failed", ex)
            STSErrorFileError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "NBTransactException", True)
        End Try

        oOut.sXMLDataset = NBTransactInput.sXMLDataSet

        ' Copy the Outputs from the In/Outs
        ' Create the return Additional Data
        oOut.AdditionalDataArray = Utilities.ArrayToClasses(vAdditionalData)

        ' End the recording
        'oCounters.EndMethod()

        'SAMFunc.STSLogMessageIndent(False)
        'SAMFunc.STSLogMessage(TraceLevelSwitch, TraceLevel.Verbose, "Leaving NBTransact")

        Return oOut

    End Function

    Protected Const ConnectionStringFrame As String = "Server={server};Database={database};Integrated Security=False; User ID={loginid}; Password={loginpassword}"

    Protected Const DefaultLanguageID As Int32 = 1

    Public Function GetPostCodeVisibility(ByVal lCountryID As Integer) As String

        Dim dsCountryConfigList As DataSet = Nothing
        Dim sCacheKey As String = String.Empty
        Dim dv As DataView = Nothing
        Dim dvID As DataView = Nothing
        Dim sFilter As String = String.Empty

        'Const ACMethodName As String = "GetPostCodeVisibility"

        _oCache = HttpRuntime.Cache()

        ' Generate a Cache Key using the ListCode appended with the List Type
        ' as we may have two lists with the same name (but differing types)
        sCacheKey = "CountryPostCodeVisibilityList"

        ' Try to get the Full List from the Cache
        dsCountryConfigList = CType(_oCache(sCacheKey), DataSet)

        ' Get the Dataset if we don't already have it
        If dsCountryConfigList Is Nothing Then
            Try
                ' BSJ April 09 - SQL Mixed Mode Compliance
                Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

                    ' Get Reserve Details
                    Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_get_country_postcode_visibility")
                        dsCountryConfigList = con.ExecuteDataSet(cmd, "dsCountryConfigList")
                    End Using
                End Using

                '' call SP
                'dsCountryConfigList = SqlHelper.ExecuteDataset(SAMFunc.ConnectionString, _
                '        CommandType.StoredProcedure, _
                '        "spu_SAM_get_country_postcode_visibility")

            Catch SQLex As System.Data.SqlClient.SqlException
                ' if the error returned indicates that a lookup table could not 
                ' be found then return an appropriate STS error.
                Throw
            Catch ex As Exception
                Dim MyError As New Exception("Unable to Execute spu_SAM_get_country_postcode_visibility", ex)
                ExceptionManager.Publish(MyError)
                Return InternalSAMConstants.PMPostCodeVisibilityHidden
            End Try

        End If

        ' Are we to Cache it
        If (Not dsCountryConfigList Is Nothing) Then

            dsCountryConfigList.Tables(0).DefaultView.AllowEdit = False

            ' Add the dataset into the cache
            _oCache.Insert(sCacheKey, dsCountryConfigList)

        End If

        ' Get the DataView For the table
        dv = dsCountryConfigList.Tables(0).DefaultView

        ' Specify the Filter
        sFilter = "country_id = " & lCountryID

        Try
            ' Create a New Data View filtered by the ID
            dvID = New DataView(dv.Table, sFilter, "", DataViewRowState.CurrentRows)

            If dvID Is Nothing OrElse dvID.Count = 0 Then
                Return InternalSAMConstants.PMPostCodeVisibilityHidden
            Else
                Return dvID.Item(0).Item("code").ToString.Trim
            End If
        Catch ex As Exception
            Return InternalSAMConstants.PMPostCodeVisibilityHidden
        End Try

    End Function

    Public Function CheckClaimKey(ByVal con As SiriusConnection, ByVal lClaimKey As Integer) As Boolean

        'Const ACMethodName As String = "CheckClaimKey"

        Dim count As Int32
        Dim ds As DataSet = Nothing
        ' Call the stored proc

        Using cmd1 As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_CLM_check_Claimkey")
            cmd1.AddInParameter("@Claim_cnt", SqlDbType.Int).Value = lClaimKey
            cmd1.AddOutParameter("@Count", SqlDbType.Int)
            Try
                con.ExecuteNonQuery(cmd1)
                count = Cast.ToInt32(cmd1.Parameters("@count").Value, 0)
            Catch ex As Exception
                ds = Nothing
                'If ExceptionPolicy.HandleException(ex, ExceptionPolicies.BusinessLayer) Then
                'Throw

            End Try
        End Using

        If count = 0 Then
            Return False
        End If

        Return True

    End Function

    Friend Overloads Function CheckAgentKey(ByVal lAgentKey As Integer) As Boolean

        Dim dsAgentList As DataSet = Nothing
        Dim sCacheKey As String = String.Empty
        Dim dv As DataView = Nothing
        Dim dvID As DataView = Nothing
        Dim sFilter As String = String.Empty
        Dim bClearAgentCache As Boolean = False

        _oCache = HttpRuntime.Cache()

        ' Generate a Cache Key using the ListCode appended with the List Type
        ' as we may have two lists with the same name (but differing types)
        sCacheKey = "AgentsList"

        dsAgentList = CType(_oCache(sCacheKey), DataSet)
        bClearAgentCache = True
        ' Get the Dataset if we don't already have it
        Try

            ' BSJ April 09 - SQL Mixed Mode Compliance
            Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

                ' Get Reserve Details
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Agent_List")
                    dsAgentList = con.ExecuteDataSet(cmd, "dsAgentList")
                End Using
            End Using

            '' call SP
            'dsAgentList = SqlHelper.ExecuteDataset(SAMFunc.ConnectionString, _
            '        CommandType.StoredProcedure, _
            '        "spu_SAM_Get_Agent_List")
        Catch SQLex As System.Data.SqlClient.SqlException
            ' if the error returned indicates that a lookup table could not 
            ' be found then return an appropriate STS error.
            Throw
        Catch ex As Exception
            Dim MyError As New Exception("Unable to Execute spu_SAM_Get_Agent_List", ex)
            ExceptionManager.Publish(MyError)
            Return False
        End Try


        'If cache exist and SP has not been called, dont reinsert into cache.
        If (Not dsAgentList Is Nothing) AndAlso bClearAgentCache = True Then
            dsAgentList.Tables(0).DefaultView.AllowEdit = False

            _oCache.Insert(sCacheKey, dsAgentList)

        End If

        dv = dsAgentList.Tables(0).DefaultView
        sFilter = "party_cnt = " & lAgentKey

        Try
            dvID = New DataView(dv.Table, sFilter, "", DataViewRowState.CurrentRows)
            If dvID Is Nothing OrElse dvID.Count = 0 Then

                'Portal is open and we are about to do an MTA using an agent which has just been created in BO.
                'Then in that case its not part of cache
                If bClearAgentCache = False Then
                    _oCache.Remove(sCacheKey) 'Remove old cache
                    If Not CheckAgentKey(lAgentKey) Then ' Recursively call the method
                        Return False
                    End If

                Else
                    Return False
                End If

            End If
        Catch ex As Exception
            Return False
        End Try

        Return True

    End Function

    Public Function GetList(ByVal GetListInput As Core.GetListInput, ByVal UseCache As Boolean) As Core.GetListOutput

        Dim GetListOutput As Core.GetListOutput
        Dim CacheKey As String = String.Empty
        Const ACMethodName As String = "GetList"

        GetListOutput = New Core.GetListOutput

        If UseCache Then

            '_oCache = HttpRuntime.Cache()()
            _oCache = HttpRuntime.Cache()

            ' Generate a Cache Key using the ListCode appended with the List Type
            ' as we may have two lists with the same name (but differing types)
            ' TO DO: Convert this to string builder
            CacheKey = GetListInput.ListCode.ToString + GetListInput.ListType.ToString

            ' Try to get the Full List from the Cache
            GetListOutput.ListItems = CType(_oCache(CacheKey), DataSet)

        End If

        ' Get the Dataset if we don't already have it
        If GetListOutput.ListItems Is Nothing Then
            Try

                ' What List Type is it
                Select Case GetListInput.ListType
                    Case Core.STSListType.PMLookup
                        'If GetListInput.Version > 0 Then
                        GetListOutput.ListItems = GetPMLookupAll(GetListInput.ListCode, GetListInput.ExcludeDeletedRecords, GetListInput.ExcludeEffectiveDate, GetListInput.ParentFieldName, GetListInput.ParentFieldValue, GetListInput.EffectiveDate, GetListInput.Version, GetListInput.valWhereClause)
                        'Else
                        'GetListOutput.ListItems = GetPMLookupAll(GetListInput.ListCode, GetListInput.ExcludeDeletedRecords, GetListInput.ExcludeEffectiveDate, GetListInput.ParentFieldName, GetListInput.ParentFieldValue)
                        'End If


                    Case Core.STSListType.GisList
                        GetListOutput.ListItems = GetGISListAll(GetListInput.ListCode)
                    Case Core.STSListType.UserDefinedTable
                        GetListOutput.ListItems = GetGISUserDefListAll(GetListInput.ListCode, GetListInput.ParentFieldValue, GetListInput.ExcludeDeletedRecords)
                    Case Else
                        Dim STSError As STSErrorPublisher = New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.ListTypeNotFound, "List Type not found", "Unknown ListType - " & GetListInput.ListType)
                        STSError.SetContext(GetListOutput.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "GetList Failed", True)
                        Return GetListOutput
                End Select

            Catch SQLex As System.Data.SqlClient.SqlException
                ' if the error returned indicates that a lookup table could not 
                ' be found then return an appropriate STS error.
                If SQLex.Number = 208 Then
                    Dim STSError As New STSErrorPublisher
                    STSError.AddInvalidField("ListCode", STSErrorPublisher.STSErrorCodes.InvalidLookupListValue.ToString, [String].Format(STSErrorPublisher.MandatoryInputInvalid, "ListCode"), GetListInput.ListCode)
                    STSError.SetContext(GetListOutput.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Lookup field validation", True)
                    Return GetListOutput
                Else
                    Throw
                End If
            Catch ex As Exception
                Dim MyError As New Exception("GetList failed", ex)
                ExceptionManager.Publish(MyError)
            End Try

        End If

        ' Are we to Cache it
        If UseCache Then

            If (Not GetListOutput.ListItems Is Nothing) Then

                GetListOutput.ListItems.Tables(0).DefaultView.AllowEdit = False

                ' Add the dataset into the cache
                _oCache.Insert(CacheKey, GetListOutput.ListItems)

            End If

        End If

        Return GetListOutput

    End Function

    Public Function GetListSPUICCS(ByVal GetListInput As Core.GetListInput, ByVal UseCache As Boolean) As Core.GetListOutput

        Dim GetListOutput As Core.GetListOutput
        Dim CacheKey As String = String.Empty
        Const ACMethodName As String = "GetListSPUICCS"

        GetListOutput = New Core.GetListOutput


        'need to validate the spu name conforms here otherwise don't proceed
        If GetListInput.SpuICCSName.Substring(0, 9) <> "spu_ICCS_" Then
            Dim STSError As New STSErrorPublisher
            STSError.AddInvalidField("SpuICCSName", STSErrorPublisher.STSErrorCodes.InvalidLookupListValue.ToString, [String].Format(STSErrorPublisher.MandatoryInputInvalid, "SpuICCSName"), GetListInput.SpuICCSName)
            STSError.SetContext(GetListOutput.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Lookup field validation", True)
            Return GetListOutput
        End If

        If UseCache Then
            _oCache = HttpRuntime.Cache()

            ' Generate a Cache Key using the ListCode appended with the List Type
            ' as we may have two lists with the same name (but differing types)
            ' TO DO: Convert this to string builder
            CacheKey = GetListInput.ListCode.ToString + GetListInput.ListType.ToString

            ' Try to get the Full List from the Cache
            GetListOutput.ListItems = CType(_oCache(CacheKey), DataSet)

        End If

        ' Get the Dataset if we don't already have it
        If GetListOutput.ListItems Is Nothing Then
            Try

                ' What List Type is it
                Select Case GetListInput.ListType
                    Case Core.STSListType.PMLookup, Core.STSListType.GisList, Core.STSListType.UserDefinedTable

                        GetListOutput.ListItems = GetLookupSPUICCS(GetListInput.SpuICCSName, GetListInput.SpuICCSParameters)
                    Case Else
                        Dim STSError As STSErrorPublisher = New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.ListTypeNotFound, "List Type not found", "Unknown ListType - " & GetListInput.ListType)
                        STSError.SetContext(GetListOutput.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "GetList Failed", True)
                        Return GetListOutput
                End Select

            Catch SQLex As System.Data.SqlClient.SqlException
                ' if the error returned indicates that a lookup table could not 
                ' be found then return an appropriate STS error.
                If SQLex.Number = 208 Then
                    Dim STSError As New STSErrorPublisher
                    STSError.AddInvalidField("ListCode", STSErrorPublisher.STSErrorCodes.InvalidLookupListValue.ToString, [String].Format(STSErrorPublisher.MandatoryInputInvalid, "ListCode"), GetListInput.ListCode)
                    STSError.SetContext(GetListOutput.STSError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "Lookup field validation", True)
                    Return GetListOutput
                Else
                    Throw SQLex
                End If
            Catch ex As Exception
                Dim MyError As New Exception("GetList failed", ex)
                ExceptionManager.Publish(MyError)
            End Try

        End If

        ' Are we to Cache it
        If UseCache Then

            If (Not GetListOutput.ListItems Is Nothing) Then

                GetListOutput.ListItems.Tables(0).DefaultView.AllowEdit = False

                ' Add the dataset into the cache
                _oCache.Insert(CacheKey, GetListOutput.ListItems)

            End If

        End If

        Return GetListOutput

    End Function


    Private Function GetLookupSPUICCS(ByVal storedProc As String, ByVal Params As Dictionary(Of String, String)) As DataSet

        ' Dataset that will hold the returned results		
        Dim dr As DataSet = Nothing
        Dim iRet As Int32
        Dim sParentKeyField As String = String.Empty

        ' call SP using a SqlParameter array
        ' BSJ April 09 - SQL Mixed Mode Compliance
        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure(storedProc)

                For Each kvp As KeyValuePair(Of String, String) In Params
                    cmd.AddInParameter(kvp.Key, SqlDbType.VarChar, 255).Value = kvp.Value
                Next

                dr = con.ExecuteDataSet(cmd, "dr")
            End Using
        End Using


        With dr.Tables(0).Columns
            ' give the table_name_id column its 'standard' name of id
            .Item(0).ColumnName = "id"

            If String.IsNullOrEmpty(sParentKeyField) Then
                ' Add the other 'standard' columns for the get list method
                .Add("parent_id", iRet.GetType)
            End If
        End With

        'this is the block of code from getgislistall - not sure if I need it 
        'Dim arrColHeaders(,) As Object = {{"caption", "code"}, _
        '        {System.Type.GetType("System.String"), System.Type.GetType("System.String")}}
        'dr = Utilities.ArrayToDataSet(, arrColHeaders)

        With dr.Tables(0).Columns
            ' Add the other 'standard' columns for the get list method
            .Add("effective_date", Now.GetType)
            .Add("is_deleted", iRet.GetType)
            .Add("id", iRet.GetType)
            .Add("parent_id", iRet.GetType)
        End With



        ' Return the Dataset (Note: There is only one Table in the Dataset)
        ' ONLY Datasets and Datatables can be remoted (Although I couldn't get the Datatable to work)
        Return dr

    End Function

    Private Function GetParentKeyFields(ByVal PMTableName As String) As String

        Select Case PMTableName.ToUpper

            Case PMLookupTable.SecondaryCause.ToUpper
                Return "primary_cause_id"
            Case Else
                Return String.Empty

        End Select

    End Function

    Private Function GetPMLookupAll(ByVal PMTableName As String, Optional ByVal ExcludeDeletedRecords As Boolean = False, Optional ByVal ExcludeEffectiveDate As Boolean = False, Optional ByVal ParentName As String = "", Optional ByVal ParentValue As Int32 = Nothing, Optional ByVal EffectiveDate As DateTime = Nothing, Optional ByVal version As Integer = 0, Optional ByVal WhereClause As System.Collections.ObjectModel.Collection(Of BaseListFilterOptions) = Nothing) As DataSet

        ' Dataset that will hold the returned results		
        Dim dr As DataSet = Nothing
        Dim iRet As Int32
        Dim sParentKeyField As String = String.Empty

        sParentKeyField = GetParentKeyFields(PMTableName)

        ' call SP using a SqlParameter array

        ' BSJ April 09 - SQL Mixed Mode Compliance
        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_PM_get_lookups_all")
                cmd.AddInParameter("@tablename", SqlDbType.VarChar, 255).Value = PMTableName
                cmd.AddInParameter("@language_id", SqlDbType.Int).Value = DefaultLanguageID
                cmd.AddInParameter("@parentkeyfield", SqlDbType.VarChar, 255).Value = sParentKeyField
                cmd.AddInParameter("@excludedeletedrecords", SqlDbType.Bit).Value = ExcludeDeletedRecords
                cmd.AddInParameter("@excludeeffectivedate", SqlDbType.Bit).Value = ExcludeEffectiveDate
                cmd.AddInParameter("@parentfield", SqlDbType.VarChar, 255).Value = ParentName
                cmd.AddInParameter("@parentvalue", SqlDbType.Int).Value = ParentValue

                If EffectiveDate <= Date.MinValue Then '
                    EffectiveDate = Now
                End If
                cmd.AddInParameter("@Effectivedate", SqlDbType.Date).Value = EffectiveDate
                cmd.AddInParameter("@version", SqlDbType.Int).Value = version
                cmd.AddInParameter("@WhereClause", SqlDbType.VarChar, 255).Value = GetWhereClause(WhereClause)


                dr = con.ExecuteDataSet(cmd, "dr")
            End Using
        End Using

        'Dim arParams(6) As SqlParameter
        '
        'arParams(0) = New SqlParameter("@tablename", PMTableName)
        'arParams(1) = New SqlParameter("@language_id", DefaultLanguageID)
        'arParams(2) = New SqlParameter("@parentkeyfield", sParentKeyField)

        'arParams(3) = New SqlParameter("@excludedeletedrecords", ExcludeDeletedRecords)
        'arParams(4) = New SqlParameter("@excludeeffectivedate", ExcludeEffectiveDate)
        'arParams(5) = New SqlParameter("@parentfield", ParentName)
        'arParams(6) = New SqlParameter("@parentvalue", ParentValue)

        '
        'dr = SqlHelper.ExecuteDataset(SAMFunc.ConnectionString, _
        '        CommandType.StoredProcedure, _
        '        "spu_PM_get_lookups_all", _
        '        arParams)

        With dr.Tables(0).Columns
            ' give the table_name_id column its 'standard' name of id
            .Item(0).ColumnName = "id"

            If String.IsNullOrEmpty(sParentKeyField) Then
                ' Add the other 'standard' columns for the get list method
                .Add("parent_id", iRet.GetType)
            End If
        End With

        ' Return the Dataset (Note: There is only one Table in the Dataset)
        ' ONLY Datasets and Datatables can be remoted (Although I couldn't get the Datatable to work)
        Return dr

    End Function

    Private Function GetWhereClause(ByVal valParameter As System.Collections.ObjectModel.Collection(Of BaseListFilterOptions)) As String

        Dim sWhereClause As New StringBuilder
        'grabbing (column, operator, value)values from collection that is passed from getlist
        'validate the values if they don't contain sql rerseved word or injection

        If valParameter IsNot Nothing Then
            For Each item In valParameter
                sWhereClause.AppendFormat(" AND {0} {1} {2} ", ValidateStatement(item.ColumnName), ValidateStatement(item.FilterOperator), QueryBuilder(ValidateStatement(item.FilterValue)))
            Next
        End If

        If sWhereClause.Length = 0 Then
            Return Nothing
        Else
            Return sWhereClause.ToString()
        End If

    End Function
    Private Function ValidateStatement(ByVal sParameter As String) As String

        'sql validation
        Dim sPattern As String = "[^a-zA-Z0-9_/<>= %(),'!]+|(\b(ALTER|FROM|WHERE|CREATE|DELETE|DROP|EXEC(UTE){0,1}|INSERT( +INTO){0,1}|MERGE|SELECT|UPDATE|UNION( +ALL){0,1}))"
        Dim ErrEx As Exception = Nothing
        Dim result As String = sParameter

        '
        If Regex.IsMatch(sParameter.ToUpper(), sPattern) Then

            ErrEx = New Exception("Invalid Parameter(s)")
            ExceptionManager.Publish(ErrEx)
            'Throw ErrEx

        End If

        Return result

    End Function
    Private Function QueryBuilder(ByVal sValue As String) As String

        Dim sbuilder As New StringBuilder

        If Regex.IsMatch(sValue.ToUpper(), "^[0-9 ]+$") Or Regex.IsMatch(sValue.ToUpper(), "\((.*?)\)") Or Regex.IsMatch(sValue.ToUpper(), "(.*(AND).*)") Then
            sbuilder.Append(sValue)
        ElseIf Regex.IsMatch(sValue.ToUpper(), "[a-zA-Z]") Then
            sbuilder.AppendFormat("'{0}'", sValue)
        ElseIf Regex.IsMatch(sValue.ToUpper(), "[\d/\d/\d]") Then
            sbuilder.AppendFormat("'{0}'", sValue)
        End If

        Return sbuilder.ToString()

    End Function

    Private Function GetGISUserDefListAll(ByVal GISUserDefHeaderCode As String, Optional ByVal ParentValue As Int32 = Nothing, Optional ByVal ExcludeDeletedRecords As Boolean = True) As DataSet

        ' Dataset that will hold the returned results		
        Dim dr As DataSet = Nothing

        ' call SP using a SqlParameter array

        Try
            ' BSJ April 09 - SQL Mixed Mode Compliance
            Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_gis_get_user_def_lookups_all")
                    cmd.AddInParameter("@gis_user_def_header_code", SqlDbType.VarChar, 10).Value = GISUserDefHeaderCode
                    cmd.AddInParameter("@language_id", SqlDbType.Int).Value = DefaultLanguageID
                    cmd.AddInParameter("@parent_value", SqlDbType.Int).Value = ParentValue
                    cmd.AddInParameter("@ExcludeDeletedRecords", SqlDbType.Bit).Value = ExcludeDeletedRecords

                    dr = con.ExecuteDataSet(cmd, "dr")

                End Using
            End Using

            'Dim arParams(2) As SqlParameter
            '
            ''arParams(0) = New SqlParameter("@gis_user_def_header_id", System.DBNull.Value)
            'arParams(0) = New SqlParameter("@gis_user_def_header_code", GISUserDefHeaderCode)
            'arParams(1) = New SqlParameter("@language_id", DefaultLanguageID)

            'arParams(2) = New SqlParameter("@parent_value", ParentValue)

            'Try
            '    dr = SqlHelper.ExecuteDataset(SAMFunc.ConnectionString, _
            '        CommandType.StoredProcedure, _
            '        "spu_gis_get_user_def_lookups_all", _
            '        arParams)
        Catch ex As Exception
            Dim MyError As New Exception("Unable to Execute spu_gis_get_user_def_lookups_all", ex)
            ExceptionManager.Publish(MyError)
            dr = Nothing
            Throw MyError
        End Try

        With dr.Tables(0).Columns
            ' give the table_name_id column its 'standard' name of id
            .Item(0).ColumnName = "id"
        End With

        ' Return the DataReader
        Return dr

    End Function

    Private Function GetGISListAll(ByVal GISListID As String) As DataSet

        Const ACMethodName As String = "GetGISListAll"

        ' Dataset that will hold the returned results		
        Dim dr As DataSet = Nothing
        Dim iRet As Int32
        'Dim ErrEx As System.Exception
        Dim vListData As Object = Nothing
        'Dim Utils As Utilities

        Dim sBranchCode As String = String.Empty

        ' Create the Application Object
        Dim oGIS As bGIS.STS = Nothing
        Try
            oGIS = New bGIS.STS
        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.FailedToCreateBackofficeComponent, "Failed to create bGIS.STS", ex.Message)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "CreateBusiness", True)
        End Try

        Try
            ' Initialise the GIS
            SAMFunc.InitialiseGISSTS(oGIS, _SiriusUser, sBranchCode)
        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.FailedToInitialiseBackofficeComponent, "Failed to initialise bGIS.STS", ex.Message)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "InitialiseBusiness", True)
        End Try

        ' Initialise the GIS Lists
        ' NOTE: We will only allow access to the standard set of GIS_Lists as used by Sirius Back Office 
        ' on the Party screens. i.e. the GIIM lists
        Try
            iRet = oGIS.InitialiseListManager(ACDefaultGISListDMC)

            If (iRet <> PMEReturnCode.PMTrue) Then
                Dim STSErrorEx As New STSErrorPublisher(iRet, "bGIS.STS.InitialiseListManager failed")
                STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "InitialiseListManagerReturn", True)
            End If
        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher("bGIS.STS.InitialiseListManager failed", ex)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "InitialiseListManagerException", True)
        End Try

        ' Call the Method
        Try
            iRet = oGIS.GetList(
                    v_sPropertyId:=GISListID,
                    r_vListData:=vListData)

            'If (iRet <> PMEReturnCode.PMTrue) Then
            'Dim STSErrorEx As New STSErrorPublisher(iRet, "bGIS.STS.GetList failed")
            'STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "GetListReturn", True)

        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher("bGIS.STS.GetList failed", ex)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "GetListException", True)
        End Try

        Try
            If oGIS IsNot Nothing Then
                oGIS.Dispose()
                oGIS = Nothing
            End If
        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher("Failed to terminate business", ex)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "Terminate", True)
        End Try

        If vListData Is Nothing Then
            ' Set it to an empty dataset
            dr = Nothing
        Else
            ' Return the Dataset (Note: There is only one Table in the Dataset)
            ' ONLY Datasets and Datatables can be remoted (Although I couldn't get the Datatable to work)
            Dim arrColHeaders(,) As Object = {{"caption", "code"},
                {System.Type.GetType("System.String"), System.Type.GetType("System.String")}}
            dr = Utilities.ArrayToDataSet(vListData, arrColHeaders)

            With dr.Tables(0).Columns
                ' Add the other 'standard' columns for the get list method
                .Add("effective_date", Now.GetType)
                .Add("is_deleted", iRet.GetType)
                .Add("id", iRet.GetType)
                .Add("parent_id", iRet.GetType)
            End With

        End If

        Return dr

    End Function

    Public Function GetAndValidateListItemFromCode(
                        ByVal ListType As Core.STSListType,
                        ByVal ListCode As String,
                        ByVal ListItemCode As String,
                        ByVal FieldName As String,
                        Optional ByVal oSAMErrorCollection As SAMErrorCollection = Nothing,
                        Optional ByRef r_sDescription As String = "NOTPASSED") As Int32

        'Const ACMethodName As String = "GetAndValidateListItemFromCode"

        Dim RaiseErrorInSitu As Boolean = False
        Dim MTAReasonDesc As String = ""
        Dim sFieldName As String = FieldName

        ' if no fieldname was specified then use the 
        ' lookup table in the fieldname
        If String.IsNullOrEmpty(sFieldName) Then
            sFieldName = ListCode
        End If

        ' if the error object hasnt been passed
        If oSAMErrorCollection Is Nothing Then
            oSAMErrorCollection = New SAMErrorCollection
            RaiseErrorInSitu = True
        End If

        ' Data View that will hold the returned results		
        Dim dv As DataView = Nothing
        Dim dvID As DataView = Nothing
        Dim sFilter As String = String.Empty

        Dim oIn As New Core.GetListInput
        Dim oOut As Core.GetListOutput
        Dim iID As Int32

        oIn.BranchCode = "BRANCH1"
        oIn.ListType = ListType
        oIn.ListCode = ListCode
        oIn.ExcludeDeletedRecords = True

        ' Get the List
        oOut = GetList(oIn, True)

        ' If we have not get the data set, either directly from the database
        ' or from the cache then error.
        If oOut.ListItems Is Nothing Then
            Dim sList As String = oIn.ListType & ":" & oIn.ListCode
            oSAMErrorCollection.AddInvalidData(
                SAMInvalidData.InvalidLookupListValue,
                SAMInvalidData.InvalidLookupListValue.ToString,
                sFieldName,
                ListItemCode)
            If RaiseErrorInSitu Then
                oSAMErrorCollection.CheckForErrors()
            End If
            Return SAMGetAndValidateListItemFromCodeReturnValues.ListNotFound
        End If

        ' Get the DataView For the table
        dv = oOut.ListItems.Tables(0).DefaultView

        ' Specify the Filter
        If (ListCode = "MTA_Event_Description") Then
            If ListItemCode.Contains(" - ") Then
                MTAReasonDesc = ListItemCode.Substring((ListItemCode.IndexOf(" - ")), ListItemCode.Length - (ListItemCode.IndexOf(" - ")))
                ListItemCode = ListItemCode.Substring(0, (ListItemCode.IndexOf(" - ")))
            End If
        End If
        sFilter = "code = " + "'" + ListItemCode + "'"

        Try
            ' Create a New Data View filtered by the ID
            dvID = New DataView(dv.Table, sFilter, "", DataViewRowState.CurrentRows)

            If dvID Is Nothing Then
                Dim sListItem As String = oIn.ListType & ":" & oIn.ListCode & ":" & ListItemCode
                oSAMErrorCollection.AddInvalidData(
                    SAMInvalidData.InvalidLookupListValue,
                    SAMInvalidData.InvalidLookupListValue.ToString,
                    sFieldName,
                    ListItemCode)
                If RaiseErrorInSitu Then
                    oSAMErrorCollection.CheckForErrors()
                End If
                Return SAMGetAndValidateListItemFromCodeReturnValues.ListItemNotFound
            End If
        Catch ex As Exception
            Dim sListItem As String = oIn.ListType & ":" & oIn.ListCode & ":" & ListItemCode
            oSAMErrorCollection.AddInvalidData(
                SAMInvalidData.InvalidLookupListValue,
                SAMInvalidData.InvalidLookupListValue.ToString,
                sFieldName,
                ListItemCode)
            If RaiseErrorInSitu Then
                oSAMErrorCollection.CheckForErrors()
            End If
            Return SAMGetAndValidateListItemFromCodeReturnValues.ListItemNotFound
        End Try

        Try
            'Return Id as 0 (Is "All" selected from Dropdown and Filtering has none for DataView)
            If dvID.Count = 0 Then
                iID = 0
            Else
                ' Return the ID (Its always the first column)
                iID = Cast.ToInt32(dvID.Item(0).Item(0), 0)
            End If

            ' Return the Description, if required
            If r_sDescription <> "NOTPASSED" Then
                r_sDescription = Cast.ToString(dvID.Item(0).Item("caption"), String.Empty)
                If (ListCode = "MTA_Event_Description") Then

                    r_sDescription += MTAReasonDesc

                End If
            End If
        Catch ex As Exception
            Dim sListItem As String = " : Details : Lookup Type : " & oIn.ListType.ToString & " : Table : " & oIn.ListCode & " : Field : " & sFieldName
            oSAMErrorCollection.AddInvalidData(
                SAMInvalidData.InvalidLookupListValue,
                SAMInvalidData.InvalidLookupListValue.ToString & sListItem,
                sFieldName,
                ListItemCode)
            If RaiseErrorInSitu Then
                oSAMErrorCollection.CheckForErrors()
            End If
            Return SAMGetAndValidateListItemFromCodeReturnValues.ListItemNotFound
        End Try

        Return iID

    End Function

    Public Function GetAndValidateListItemFromId(
                        ByVal ListType As Core.STSListType,
                        ByVal ListCode As String,
                        ByVal ListItemId As Integer,
                        ByVal FieldName As String,
                        Optional ByVal oSAMErrorCollection As SAMErrorCollection = Nothing,
                        Optional ByRef Description As String = "NOTPASSED",
                        Optional ByRef Code As String = "NOTPASSED") As Int32

        'Const ACMethodName As String = "GetAndValidateListItemFromId"

        Dim RaiseErrorInSitu As Boolean = False

        Dim sFieldName As String = FieldName

        ' if no fieldname was specified then use the 
        ' lookup table in the fieldname
        If String.IsNullOrEmpty(sFieldName) Then
            sFieldName = ListCode
        End If

        ' if the error object hasnt been passed
        If oSAMErrorCollection Is Nothing Then
            oSAMErrorCollection = New SAMErrorCollection
            RaiseErrorInSitu = True
        End If

        ' Data View that will hold the returned results		
        Dim dv As DataView = Nothing
        Dim dvID As DataView = Nothing
        Dim sFilter As String = String.Empty

        Dim oIn As New Core.GetListInput
        Dim oOut As Core.GetListOutput
        Dim iID As Int32

        oIn.BranchCode = "BRANCH1"
        oIn.ListType = ListType
        oIn.ListCode = ListCode

        ' Get the List
        oOut = GetList(oIn, True)

        ' If we have not get the data set, either directly from the database
        ' or from the cache then error.
        If oOut.ListItems Is Nothing Then
            Dim sList As String = oIn.ListType & ":" & oIn.ListCode
            oSAMErrorCollection.AddInvalidData(
                SAMInvalidData.InvalidLookupListValue,
                SAMInvalidData.InvalidLookupListValue.ToString,
                sFieldName,
                "Supplied Id Value :" & ListItemId.ToString)
            If RaiseErrorInSitu Then
                oSAMErrorCollection.CheckForErrors()
            End If
            Return SAMGetAndValidateListItemFromCodeReturnValues.ListNotFound
        End If

        ' Get the DataView For the table
        dv = oOut.ListItems.Tables(0).DefaultView

        ' Specify the Filter
        sFilter = "id = " + ListItemId.ToString

        Try
            ' Create a New Data View filtered by the ID
            dvID = New DataView(dv.Table, sFilter, "", DataViewRowState.CurrentRows)

            If dvID Is Nothing Then
                Dim sListItem As String = oIn.ListType & ":" & oIn.ListCode & ":" & ListItemId.ToString()
                oSAMErrorCollection.AddInvalidData(
                    SAMInvalidData.InvalidLookupListValue,
                    SAMInvalidData.InvalidLookupListValue.ToString,
                    sFieldName,
                    "Supplied Id Value :" + ListItemId.ToString)
                If RaiseErrorInSitu Then
                    oSAMErrorCollection.CheckForErrors()
                End If
                Return SAMGetAndValidateListItemFromCodeReturnValues.ListItemNotFound
            End If
        Catch ex As Exception
            Dim sListItem As String = oIn.ListType & ":" & oIn.ListCode & ":" & ListItemId.ToString()
            oSAMErrorCollection.AddInvalidData(
                SAMInvalidData.InvalidLookupListValue,
                SAMInvalidData.InvalidLookupListValue.ToString,
                sFieldName,
                "Supplied Id Value :" + ListItemId.ToString)
            If RaiseErrorInSitu Then
                oSAMErrorCollection.CheckForErrors()
            End If
            Return SAMGetAndValidateListItemFromCodeReturnValues.ListItemNotFound
        End Try

        Try
            ' Return the ID (Its always the first column)
            iID = Cast.ToInt32(dvID.Item(0).Item(0), 0)

            ' Return the Description, if required
            If Description <> "NOTPASSED" Then
                Description = Cast.ToString(dvID.Item(0).Item("caption"), String.Empty)
            End If

            ' return the code if requested
            If Code <> "NOTPASSED" Then
                Code = Cast.ToString(dvID.Item(0).Item("code"), String.Empty)
            End If

        Catch ex As Exception
            Dim sListItem As String = oIn.ListType & ":" & oIn.ListCode & ":" & ListItemId
            oSAMErrorCollection.AddInvalidData(
                SAMInvalidData.InvalidLookupListValue,
                SAMInvalidData.InvalidLookupListValue.ToString,
                sFieldName,
                "Supplied Id Value :" + ListItemId.ToString)

            If RaiseErrorInSitu Then
                oSAMErrorCollection.CheckForErrors()
            End If

            Return SAMGetAndValidateListItemFromCodeReturnValues.ListItemNotFound
        End Try

        Return iID

    End Function

    Public Function GetListItemFromCode(
                        ByVal ListType As Core.STSListType,
                        ByVal ListCode As String,
                        ByVal ListItemCode As String,
                        Optional ByRef r_sDescription As String = "NOTPASSED") As Int32

        Const ACMethodName As String = "GetListItemFromCode"

        ' Data View that will hold the returned results		
        Dim dv As DataView = Nothing
        Dim dvID As DataView = Nothing
        Dim sFilter As String = String.Empty

        Dim oIn As New Core.GetListInput
        Dim oOut As Core.GetListOutput
        Dim iID As Int32

        oIn.BranchCode = "BRANCH1"
        oIn.ListType = ListType
        oIn.ListCode = ListCode
        oIn.ExcludeDeletedRecords = True

        ' Get the List
        oOut = GetList(oIn, True)

        ' If we have not get the data set, either directly from the database
        ' or from the cache then error.
        If oOut.ListItems Is Nothing Then
            Dim ex As New Exception("No Effective List retrieved for List : " + ListCode.ToString)
            ExceptionManager.Publish(ex)
            Throw ex
        End If

        ' Get the DataView For the table
        dv = oOut.ListItems.Tables(0).DefaultView

        ' Specify the Filter
        ' added OR condition by Gaurav
        sFilter = "code = " + "'" + Trim(ListItemCode) + "' AND (is_deleted = 0 or is_deleted IS NULL)"

        Try
            ' Create a New Data View filtered by the ID
            dvID = New DataView(dv.Table, sFilter, "", DataViewRowState.CurrentRows)

            If dvID Is Nothing Then
                Dim STSError As New STSErrorPublisher(PMEReturnCode.PMNotFound, "No row found for STSList : " + ListCode.ToString + " Code : " + ListItemCode.ToString)
                STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "DataView", True)
            End If
        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher("No row found for STSList : " + ListCode.ToString + " Code : " + ListItemCode.ToString, ex)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "GetDataView", True)
        End Try

        Try
            ' Return the ID (Its always the first column)
            iID = Cast.ToInt32(dvID.Item(0).Item(0), 0)
            ' Return the Description, if required
            If r_sDescription <> "NOTPASSED" Then
                r_sDescription = Cast.ToString(dvID.Item(0).Item("caption"), String.Empty)
            End If
        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher("No row found for STSList : " + ListCode.ToString + " Code : " + ListItemCode.ToString, ex)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "GetDataView", True)
        End Try

        Return iID

    End Function

    'Corebusiness
    Public Function GetCurrentIdFromBaseIdAndVersionId(ByVal con As SiriusConnection,
                               ByVal TableName As String,
                               ByVal BaseId As Integer,
                               ByVal VersionId As Integer) As Int32

        ' Const ACMethodName As String = "GetListIDFromBaseId"

        Dim dt As DataTable = Nothing

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_CLM_Get_Id_FromBaseID")

            cmd.AddInParameter("@tablename", SqlDbType.VarChar, 100).Value = TableName
            cmd.AddInParameter("@baseid", SqlDbType.Int).Value = BaseId
            cmd.AddInParameter("@versionid", SqlDbType.Int).Value = VersionId

            dt = con.ExecuteDataTable(cmd)

            If dt IsNot Nothing AndAlso (dt.Rows.Count <> 0) Then
                Return Cast.ToInt32(dt.Rows(0).Item(0)).GetValueOrDefault(0)
            Else
                Dim oSAMErrorCollection As New SAMErrorCollection
                oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.CurrentIdLookupUsingBaseIdAndVersionIDFailed,
                        SAMBusinessErrors.CurrentIdLookupUsingBaseIdAndVersionIDFailed.ToString(),
                        "Id Lookup on Table :" + TableName + " using base_id " + BaseId.ToString +
                            " and version_id " + VersionId.ToString + " Failed To return any details")
            End If

        End Using

    End Function

    Public Function GetListItemFromCaption(
                        ByVal ListType As Core.STSListType,
                        ByVal ListCode As String,
                        ByVal ListItemCaption As String) As Int32

        Const ACMethodName As String = "GetListItemFromCaption"

        ' Data View that will hold the returned results		
        Dim dv As DataView = Nothing
        Dim dvID As DataView = Nothing
        Dim sFilter As String = String.Empty

        Dim oIn As New Core.GetListInput
        Dim oOut As Core.GetListOutput
        Dim iID As Int32

        oIn.BranchCode = "BRANCH1"
        oIn.ListType = ListType
        oIn.ListCode = ListCode

        ' Get the List
        oOut = GetList(oIn, True)

        ' If we have not get the data set, either directly from the database
        ' or from the cache then error.
        If oOut.ListItems Is Nothing Then
            Dim ex As New Exception("No Effective List retrieved for List : " + ListCode.ToString)
            ExceptionManager.Publish(ex)
            Throw ex
        End If

        ' Get the DataView For the table
        dv = oOut.ListItems.Tables(0).DefaultView
        ListItemCaption = Replace(ListItemCaption, "'", "''")

        ' Specify the Filter
        sFilter = "caption = " + "'" + ListItemCaption + "' AND is_deleted = 0"

        Try
            ' Create a New Data View filtered by the ID
            dvID = New DataView(dv.Table, sFilter, "", DataViewRowState.CurrentRows)

            If dvID Is Nothing Then
                Dim STSError As New STSErrorPublisher(PMEReturnCode.PMNotFound, "No row found for STSList : " + ListCode.ToString + " Code : " + ListItemCaption.ToString)
                STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "DataView", True)
            End If

        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher("No row found for STSList : " + ListCode.ToString + " Code : " + ListItemCaption.ToString, ex)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "GetDataView", True)
        End Try

        Try
            If ListType = Core.STSListType.GisList Then
                ' Return the code 
                iID = Cast.ToInt32(dvID.Item(0).Item(1), 0)
            Else
                ' Return the ID (Its always the first column)
                iID = Cast.ToInt32(dvID.Item(0).Item(0), 0)
            End If
        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher("No row found for STSList : " + ListCode.ToString + " Code : " + ListItemCaption.ToString, ex)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "GetDataView", True)
        End Try

        Return iID

    End Function

    Public Function GetListItemFromID(
                        ByVal ListType As Core.STSListType,
                        ByVal ListCode As String,
                        ByVal ListItemID As Int32,
                        Optional ByRef r_sDescription As String = "NOTPASSED",
                        Optional ByVal bIsViewClaim As Boolean = False) As String

        Const ACMethodName As String = "GetListItemFromID"

        ' Data View that will hold the returned results		
        Dim dv As DataView = Nothing
        Dim dvID As DataView = Nothing
        Dim sFilter As String = String.Empty

        Dim oIn As New Core.GetListInput
        Dim oOut As Core.GetListOutput
        Dim sCode As String = String.Empty

        oIn.BranchCode = "BRANCH1"
        oIn.ListType = ListType
        oIn.ListCode = ListCode

        ' Get the List
        oOut = GetList(oIn, True)

        ' If we have not get the data set, either directly from the database
        ' or from the cache then error.
        If oOut.ListItems Is Nothing Then
            Dim ex As New Exception("No Effective List retrieved for List : " + ListCode.ToString)
            ExceptionManager.Publish(ex)
            Throw ex
        End If

        ' Get the DataView For the table
        dv = oOut.ListItems.Tables(0).DefaultView

        ' Specify the Filter
        If (bIsViewClaim) Then
            sFilter = "id = " & ListItemID
        Else
            sFilter = "id = " & ListItemID & " AND is_deleted = 0"
        End If

        Try
            ' Create a New Data View filtered by the ID
            dvID = New DataView(dv.Table, sFilter, "", DataViewRowState.CurrentRows)

            If dvID Is Nothing Then
                Dim STSError As New STSErrorPublisher(PMEReturnCode.PMNotFound, "No row found for STSList : " + ListCode.ToString + " ID : " + ListItemID.ToString)
                STSError.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "DataView", True)
            End If
        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher("No row found for STSList : " + ListCode.ToString + " ID : " + ListItemID.ToString, ex)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "GetDataView", True)
        End Try

        Try
            ' Return the code 
            sCode = Cast.ToString(dvID.Item(0).Item("code"), String.Empty)
            If r_sDescription <> "NOTPASSED" Then
                r_sDescription = Cast.ToString(dvID.Item(0).Item("caption"), String.Empty)
            End If
        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher("No row found for STSList : " + ListCode.ToString + " Code : " + ListItemID.ToString, ex)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "GetDataView", True)
        End Try

        Return sCode

    End Function

    Public Shared Function PartyIsAgent(ByVal sPartyType As String) As Boolean
        Return (sPartyType = PartyType.Agent)
    End Function

    Public Shared Function PartyIsThirdParty(ByVal sPartyType As String) As Boolean
        Return sPartyType.StartsWith(PartyType.ThirdParty)
    End Function

    Public Shared Function PartyIsEmployee(ByVal sPartyType As String) As Boolean
        Return (Not PartyIsAgent(sPartyType) And Not PartyIsThirdParty(sPartyType))
    End Function

    Friend Overloads Function AgentSecurityCheck(
                            ByVal UserName As String,
                            ByVal PrimaryKey As Integer,
                            ByVal EntityType As PMEEntityType) As Boolean
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)

            Dim oResponse As Boolean

            oResponse = AgentSecurityCheck(con, UserName, PrimaryKey, EntityType)

            Return oResponse

        End Using

    End Function

    Friend Overloads Function AgentSecurityCheck(ByVal con As SiriusConnection,
                            ByVal UserName As String,
                            ByVal PrimaryKey As Integer,
                            ByVal EntityType As PMEEntityType) As Boolean

        'Const ACMethodName As String = "AgentSecurityCheck"

        Const ACUserAgentPermissionKey As String = "UserAgentPermissionKey_"

        ' Dataset that will hold the returned results		
        'Dim dr As DataSet =Nothing
        Dim CacheKey As String = String.Empty
        'Dim iUserId As Int16
        'Dim iLanguageId As Int16
        'Dim sPassword As String = String.Empty
        Dim sProcName As String = String.Empty
        Dim blPermission As Boolean

        ' Get a reference to the cache object
        _oCache = HttpRuntime.Cache()

        ' Append Screen Code because we will have seperate datasets for each type of screen.
        CacheKey = ACUserAgentPermissionKey & UserName & "_" & EntityType & "_" & PrimaryKey

        If (_oCache(CacheKey) Is Nothing) = False Then

            Return CType(_oCache(CacheKey), Boolean)

        Else

            ' call SP using a SqlParameter array

            If EntityType = PMEEntityType.Party Then
                sProcName$ = "spu_STS_Check_Agent_Party_Relationship"
            ElseIf EntityType = PMEEntityType.InsuranceFile Then
                sProcName$ = "spu_SAM_Check_Agent_Insurance_File_Relationship_UW"
            ElseIf EntityType = PMEEntityType.Risk Then
            ElseIf EntityType = PMEEntityType.Source Then
                sProcName$ = "spu_STS_Check_Agent_Branch_Relationship"
            End If

            Using cmd As SiriusCommand = SiriusCommand.FromProcedure(sProcName)

                If (UserName <> "") Then
                    cmd.AddInParameter("@Username", SqlDbType.VarChar, 255).Value = UserName
                End If

                If (PrimaryKey <> 0) Then
                    cmd.AddInParameter("@PrimaryKey", SqlDbType.Int).Value = PrimaryKey
                End If

                cmd.AddOutParameter("@Permission", SqlDbType.Int)

                con.ExecuteNonQuery(cmd)

                If Cast.ToInt32(cmd.Parameters("@Permission").Value, 0) <> -1 Then
                    'The Agent has permission so....
                    blPermission = True
                Else
                    'Agent Doesn't have permission
                    blPermission = False
                End If

            End Using

            ' Add the Permission indicator into the cache
            _oCache.Insert(CacheKey, blPermission, Nothing, DateTime.MaxValue, TimeSpan.FromHours(1))

            Return blPermission

        End If

    End Function

    ' Should these go into SAMFunc ?

    Friend Overloads Function CheckInsuranceFolder(ByVal lInsuranceFolderCnt As Integer, Optional ByRef r_lSourceID As Integer = 0) As Boolean
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)

            Dim oResponse As Boolean

            oResponse = CheckInsuranceFolder(con, lInsuranceFolderCnt, r_lSourceID)

            Return oResponse

        End Using

    End Function

    Friend Overloads Function CheckInsuranceFolder(ByVal con As SiriusConnection, ByVal lInsuranceFolderCnt As Integer, Optional ByRef r_lSourceID As Integer = 0) As Boolean

        'Const ACMethodName As String = "CheckInsuranceFolder"

        Dim ds As DataSet = Nothing

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_Insurance_Folder_sel")

            cmd.AddInParameter("@insurance_folder_cnt", SqlDbType.Int).Value = lInsuranceFolderCnt

            ds = con.ExecuteDataSet(cmd, "InsuranceFolderDetails")

            If ds.Tables("InsuranceFolderDetails").Rows.Count = 0 Then
                Return False
            Else
                ' Get the source id
                r_lSourceID = Cast.ToInt32(ds.Tables(0).Rows(0).Item("source_id"), 0)
                Return True
            End If

        End Using

    End Function

    Friend Overloads Function CheckInsuranceFile(ByVal lInsuranceFileCnt As Integer, Optional ByRef r_lInsuranceFolderCnt As Integer = 0, Optional ByRef r_lSourceID As Integer = 0) As Boolean
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)

            Dim oResponse As Boolean

            oResponse = CheckInsuranceFile(con, lInsuranceFileCnt, r_lInsuranceFolderCnt, r_lSourceID)

            Return oResponse

        End Using

    End Function

    Friend Overloads Function CheckInsuranceFile(ByVal con As SiriusConnection, ByVal lInsuranceFileCnt As Integer, Optional ByRef r_lInsuranceFolderCnt As Integer = 0, Optional ByRef r_lSourceID As Integer = 0) As Boolean

        Dim ds As DataSet = Nothing

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_Insurance_File_sel")

            cmd.AddInParameter("@insurance_File_cnt", SqlDbType.Int).Value = lInsuranceFileCnt

            ds = con.ExecuteDataSet(cmd, "InsuranceFileDetails")

            If ds.Tables("InsuranceFileDetails").Rows.Count = 0 Then
                Return False
            Else
                ' Get the insurance folder and source id
                r_lInsuranceFolderCnt = Cast.ToInt32(ds.Tables(0).Rows(0).Item("insurance_folder_cnt"), 0)
                r_lSourceID = Cast.ToInt32(ds.Tables(0).Rows(0).Item("source_id"), 0)
                Return True
            End If

        End Using

    End Function

    Friend Overloads Function CheckRisk(ByVal lRiskCnt As Integer) As Boolean

        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)

            Dim oResponse As Boolean

            oResponse = CheckRisk(con, lRiskCnt)

            Return oResponse

        End Using

    End Function

    Friend Overloads Function CheckRisk(ByVal con As SiriusConnection, ByVal lRiskCnt As Integer) As Boolean

        'Const ACMethodName As String = "CheckRisk"

        Dim ds As DataSet = Nothing

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_Risk_sel")

            cmd.AddInParameter("@risk_cnt", SqlDbType.Int).Value = lRiskCnt

            ds = con.ExecuteDataSet(cmd, "RiskDetails")

            If ds.Tables("RiskDetails").Rows.Count = 0 Then
                Return False
            Else
                Return True
            End If

        End Using

    End Function
    Friend Overloads Function CheckPolicyRiskLink(ByVal lInsuranceFileCnt As Integer, ByVal lRiskCnt As Integer) As Boolean
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)

            Dim oResponse As Boolean

            oResponse = CheckPolicyRiskLink(con, lInsuranceFileCnt, lRiskCnt)

            Return oResponse

        End Using

    End Function
    ''' <summary>
    ''' Check Policy Risk Link
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="lInsuranceFileCnt"></param>
    ''' <param name="lRiskCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Overloads Function CheckPolicyRiskLink(ByVal con As SiriusConnection, ByVal lInsuranceFileCnt As Integer, ByVal lRiskCnt As Integer, Optional ByVal bCheckStatusForUpdating As Boolean = False) As Boolean

        'Const ACMethodName As String = "CheckPolicyRiskLink"

        Dim ds As DataSet = Nothing

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spe_insurance_file_risk_li_sel")

            cmd.AddInParameter("@insurance_file_cnt", SqlDbType.Int).Value = lInsuranceFileCnt
            cmd.AddInParameter("@risk_cnt", SqlDbType.Int).Value = lRiskCnt

            ds = con.ExecuteDataSet(cmd, "RiskDetails")

            If ds.Tables("RiskDetails").Rows.Count = 0 Then
                Return False
            Else
                ' EM Copy Risk change, ensure that the risk link is valid for updating
                If Not bCheckStatusForUpdating Then
                    Return True
                End If
                Dim riskStatus As String = Cast.ToString(ds.Tables("RiskDetails").Rows(0)("status_flag"), RiskLinkStatusType.Unchanged)
                If riskStatus = RiskLinkStatusType.Unchanged OrElse riskStatus = RiskLinkStatusType.Renewed Then
                    Return False
                Else
                    Return True
                End If
            End If

        End Using

    End Function

    Friend Overloads Function CheckInsuranceFileRef(ByVal sInsuranceFileRef As String, ByVal iSourceId As Int32) As Boolean

        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            Dim oResponse As Boolean

            oResponse = CheckInsuranceFileRef(con, sInsuranceFileRef, iSourceId)

            Return oResponse

        End Using

    End Function

    Friend Overloads Function CheckInsuranceFileRef(ByVal con As SiriusConnection, ByVal sInsuranceFileRef As String, ByVal iSourceId As Int32) As Boolean

        Dim ds As DataSet = Nothing

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Insurance_File_Ref_Sel")

            cmd.AddInParameter("@insurance_File_ref", SqlDbType.VarChar).Value = sInsuranceFileRef
            cmd.AddInParameter("@source_id", SqlDbType.Int).Value = iSourceId
            ds = con.ExecuteDataSet(cmd, "InsuranceFileDetails")

            If ds.Tables("InsuranceFileDetails").Rows.Count = 0 Then
                Return False
            Else
                Return True
            End If

        End Using

    End Function

    Public Overloads Sub GetSAMTimestamp(
                ByVal BranchCode As String,
                ByVal Lockname As LockName,
                ByVal LockValue As Int32,
                ByRef TStamp As Byte(),
                ByRef bCurrentlyLocked As Boolean,
       Optional ByRef iLockedByUserID As Integer = -1,
       Optional ByRef sLockedByUser As String = "")
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            GetTimestamp(con, BranchCode,
                        Lockname,
                        LockValue,
                        TStamp,
                        bCurrentlyLocked,
                        iLockedByUserID,
                        sLockedByUser)

        End Using

    End Sub

    Public Overloads Sub GetSAMTimestamp(ByVal Con As SiriusConnection,
                ByVal BranchCode As String,
                ByVal Lockname As LockName,
                ByVal LockValue As Int32,
                ByRef TStamp As Byte(),
                ByRef bCurrentlyLocked As Boolean,
       Optional ByRef iLockedByUserID As Integer = -1,
       Optional ByRef sLockedByUser As String = "")

        'Const ACMethodName As String = "GetTimestamp"

        'Dim oGIS As bGIS.STS = Nothing
        'Try
        '    oGIS = New bGIS.STS
        'Catch ex As Exception
        '    Dim oSAMErrorCollection As New SAMErrorCollection
        '    oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.FailedToCreateCOMComponent, _
        '                                        SAMBusinessErrors.FailedToCreateCOMComponent.ToString, _
        '                                         "Failed to create bGIS.STS")
        '    oSAMErrorCollection.CheckForErrors()
        'End Try

        'Try
        '    SAMFunc.InitialiseGISSTS(Con, oGIS, _SiriusUser, BranchCode)
        'Catch ex As Exception
        '    Dim oSAMErrorCollection As New SAMErrorCollection
        '    oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.FailedToInitialiseCOMComponent, _
        '                                        SAMBusinessErrors.FailedToInitialiseCOMComponent.ToString, _
        '                                         "Failed to initialise bGIS.STS")
        '    oSAMErrorCollection.CheckForErrors()
        'End Try

        Try
            Dim iRet As Int32

            Dim tStampObject As Object = TStamp
            Dim ds As DataSet = Nothing
            'Dim bCurrentlyLockedObject As Object = bCurrentlyLocked
            'Dim iLockedByUserIDObject As Object = iLockedByUserID
            'Dim sLockedByUserObject As Object = sLockedByUser

            'iRet = oGIS.GetLastUnlockTimestamp(LockNameString(Lockname), LockValue, tStampObject, bCurrentlyLockedObject, iLockedByUserIDObject, sLockedByUserObject)
            'If iRet <> PMEReturnCode.PMTrue Then
            '    Dim oSAMErrorCollection As New SAMErrorCollection
            '    oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed, _
            '                                        SAMBusinessErrors.COMComponentMethodFailed.ToString, _
            '                                         "bGIS.STS.GetLastUnlockTimestamp Failed")
            '    oSAMErrorCollection.CheckForErrors()

            'End If
            'check for Exclusive Locking seting
            Dim sOptionValue As String = String.Empty
            GetSystemOption(BranchCode, SystemOption.EnableExclusiveLocking, sOptionValue)
            If sOptionValue = "1" AndAlso (Lockname = CoreBusiness.LockName.ClaimId OrElse
                                           Lockname = CoreBusiness.LockName.InsuranceFolderCnt _
                                           OrElse Lockname = CoreBusiness.LockName.RenewalProcess) Then
                bCurrentlyLocked = False
                TStamp = DirectCast(BitConverter.GetBytes(DateTime.Now.Ticks), Byte())
                Exit Sub
            Else
                iRet = PMEReturnCode.PMTrue
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_pmlock_last_unlock_sel")
                    cmd.AddInParameter("@lock_name", SqlDbType.VarChar, 30).Value = LockNameString(Lockname)
                    cmd.AddInParameter("@lock_value", SqlDbType.Int).Value = LockValue

                    ds = Con.ExecuteDataSet(cmd, "lock")
                    If ds.Tables("lock").Rows.Count > 0 Then
                        ' return the timestamp
                        'r_vTimestamp = m_oDatabase.Records.Item(1).Fields.Item("tstamp").Value
                        'tStampObject = DirectCast(ds.Tables("lock").Rows(0)("tstamp"), Byte)
                        tStampObject = ds.Tables("lock").Rows(0)("tstamp")
                    End If
                    If IsDBNull(ds.Tables("lock").Rows(0)("currently_locked_by_id")) Then
                        bCurrentlyLocked = False
                        iLockedByUserID = 0
                        sLockedByUser = ""
                    Else
                        '' Yes, so return the details of who has it locked.
                        bCurrentlyLocked = True
                        iLockedByUserID = Cast.ToInt32(ds.Tables("lock").Rows(0).Item("currently_locked_by_id"), 0)
                        sLockedByUser = Cast.ToString(ds.Tables("lock").Rows(0)("currently_locked_by"))
                    End If
                End Using
            End If
            TStamp = DirectCast(tStampObject, Byte())

            ' bCurrentlyLocked = CBool(bCurrentlyLockedObject)
            'iLockedByUserID = Cast.ToInt32(iLockedByUserIDObject, 0)
            'sLockedByUser = Cast.ToString(sLockedByUserObject, String.Empty)

        Catch ex As Exception
            Dim oSAMErrorCollection As New SAMErrorCollection
            oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                                SAMBusinessErrors.COMComponentMethodFailed.ToString,
                                                 "bGIS.STS.GetLastUnlockTimestamp Failed")
            oSAMErrorCollection.CheckForErrors()
        End Try

    End Sub

    Public Overloads Function GetTimestamp(
                        ByVal BranchCode As String,
                        ByVal Lockname As LockName,
                        ByVal LockValue As Int32,
                        ByRef TStamp As Byte(),
                        ByRef bCurrentlyLocked As Boolean,
               Optional ByRef iLockedByUserID As Integer = -1,
               Optional ByRef sLockedByUser As String = "") As STSErrorType
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            Dim oResponse As STSErrorType

            oResponse = GetTimestamp(con, BranchCode,
                                        Lockname,
                                        LockValue,
                                        TStamp,
                                        bCurrentlyLocked,
                                        iLockedByUserID,
                                        sLockedByUser)

            Return oResponse

        End Using

    End Function

    Public Overloads Function GetTimestamp(ByVal Con As SiriusConnection,
                        ByVal BranchCode As String,
                        ByVal Lockname As LockName,
                        ByVal LockValue As Int32,
                        ByRef TStamp As Byte(),
                        ByRef bCurrentlyLocked As Boolean,
               Optional ByRef iLockedByUserID As Integer = -1,
                Optional ByRef sLockedByUser As String = "",
                Optional ByVal bReturnError As Boolean = False) As STSErrorType

        Const ACMethodName As String = "GetTimestamp"

        'Dim oGIS As bGIS.STS = Nothing
        'Try
        '    oGIS = New bGIS.STS
        'Catch ex As Exception
        '    Dim TheError As STSErrorType = Nothing
        '    Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.FailedToCreateBackofficeComponent, "Failed to create bGIS.Application", ex.Message)
        '    STSErrorEx.SetContext(TheError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "CreateBusiness", True)
        '    Return TheError
        'End Try

        'Try
        '    SAMFunc.InitialiseGISSTS(Con:=Con, oGIS:=oGIS, SiriusUser:=_SiriusUser, sBranchCode:=BranchCode)
        'Catch ex As Exception
        '    SAMFunc.DestroyCOMInterop(CObj(oGIS))
        '    Dim TheError As STSErrorType = Nothing
        '    Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.FailedToInitialiseBackofficeComponent, "Failed to initialise bGIS.Application", ex.Message)
        '    STSErrorEx.SetContext(TheError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "InitialiseBusiness", True)
        '    Return TheError
        'End Try

        Try
            Dim iRet As Int32

            Dim tStampObject As Object = TStamp
            Dim bCurrentlyLockedObject As Object = bCurrentlyLocked 'DD
            Dim iLockedByUserIDObject As Object = iLockedByUserID 'DD
            Dim sLockedByUserObject As Object = sLockedByUser 'DD
            Dim ds As DataSet = Nothing

            'iRet = oGIS.GetLastUnlockTimestamp(LockNameString(Lockname), LockValue, tStampObject, bCurrentlyLockedObject, iLockedByUserIDObject, sLockedByUserObject) 'DD
            iRet = PMEReturnCode.PMTrue
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_pmlock_last_unlock_sel")
                cmd.AddInParameter("@lock_name", SqlDbType.VarChar, 30).Value = LockNameString(Lockname)
                cmd.AddInParameter("@lock_value", SqlDbType.Int).Value = LockValue

                ds = Con.ExecuteDataSet(cmd, "lock")
                If ds.Tables("lock").Rows.Count > 0 Then
                    ' return the timestamp
                    'r_vTimestamp = m_oDatabase.Records.Item(1).Fields.Item("tstamp").Value
                    tStampObject = ds.Tables("lock").Rows(0)("tstamp")
                    'tStampObject = DirectCast(ds.Tables("lock").Rows(0)("tstamp"), Byte)
                End If
                If IsDBNull(ds.Tables("lock").Rows(0)("currently_locked_by_id")) Then
                    bCurrentlyLocked = False
                    iLockedByUserID = 0
                    sLockedByUser = ""
                Else
                    '' Yes, so return the details of who has it locked.
                    bCurrentlyLocked = True
                    iLockedByUserID = Cast.ToInt32(ds.Tables("lock").Rows(0).Item("currently_locked_by_id"), 0)
                    sLockedByUser = Cast.ToString(ds.Tables("lock").Rows(0)("currently_locked_by"))
                End If
            End Using
            TStamp = DirectCast(tStampObject, Byte())

            'DD]
            'If iRet <> PMEReturnCode.PMTrue Then
            '    SAMFunc.DestroyCOMInterop(CObj(oGIS))
            '    Dim STSErrorEx As New STSErrorPublisher(iRet, "Call to bGIS.STS.GetLastUnlockTimestamp failed.")
            '    STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "GetLastUnlockTimestamp", True)
            'End If


            'bCurrentlyLocked = CBool(bCurrentlyLockedObject)
            'iLockedByUserID = Cast.ToInt32(iLockedByUserIDObject, 0)
            'sLockedByUser = Cast.ToString(sLockedByUserObject, String.Empty)

            If iLockedByUserID <> 0 AndAlso bReturnError Then

                Dim TheError As STSErrorType = Nothing
                Dim STSError As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.RecordLockedByAnotherUser, "The record is locked by user ", sLockedByUser)
                STSError.SetContext(TheError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "RecordLocked", True)
                Return TheError

            End If


        Catch ex As Exception
            'SAMFunc.DestroyCOMInterop(CObj(oGIS))
            Dim STSErrorEx As New STSErrorPublisher("Call to spu_pmlock_last_unlock_sel failed with an exception.", ex)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "GetLastUnlockTimestamp", True)
        End Try

        'SAMFunc.DestroyCOMInterop(CObj(oGIS))

        ' Don't return an Error
        Return Nothing

    End Function

    Public Overloads Sub CheckSAMTSAndLock(ByVal Con As SiriusConnection, ByVal BranchCode As String, ByVal Lockname As LockName, ByVal LockValue As Int32, ByVal TStamp As Byte())

        'Const ACMethodName As String = "CheckTSAndLock"

        'Dim oGIS As bGIS.STS = Nothing
        Dim iRet As Int32
        Dim LockedBy As String = ""
        Dim TimestampMatches As Boolean
        'Try
        '    oGIS = New bGIS.STS
        'Catch ex As Exception
        '    Dim oSAMErrorCollection As New SAMErrorCollection
        '    oSAMErrorCollection.AddInvalidData(SAMBusinessErrors.FailedToCreateCOMComponent, _
        '                                    SAMBusinessErrors.FailedToCreateCOMComponent.ToString, _
        '                                    "Failed to create bGIS.STS")
        '    oSAMErrorCollection.CheckForErrors()
        'End Try

        'Try
        '    SAMFunc.InitialiseGISSTS(Con:=Con, oGIS:=oGIS, SiriusUser:=_SiriusUser, sBranchCode:=BranchCode)
        'Catch ex As Exception
        '    Dim oSAMErrorCollection As New SAMErrorCollection
        '    oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.FailedToInitialiseCOMComponent, _
        '                                        SAMBusinessErrors.FailedToInitialiseCOMComponent.ToString, _
        '                                         "Failed to initialise bGIS.STS")
        '    oSAMErrorCollection.CheckForErrors()
        'End Try

        Try


            'Dim TimestampMatches As Boolean
            'Check for Exclusive Locking setting
            Dim sOptionValue As String = ""
            GetSystemOption(BranchCode,
                                      SystemOption.EnableExclusiveLocking,
                                      sOptionValue)

            If sOptionValue = "1" AndAlso (Lockname = CoreBusiness.LockName.ClaimId OrElse
                                           Lockname = CoreBusiness.LockName.InsuranceFolderCnt _
                                           OrElse Lockname = CoreBusiness.LockName.RenewalProcess) Then
                Return
            Else

                ' iRet = CheckTSAndLock(BranchCode, LockNameString(Lockname), LockValue, TStamp, LockedBy, TimestampMatches)
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_pmlock_last_unlock_check")
                    cmd.AddOutParameter("@tstamp_matches", SqlDbType.Int)
                    cmd.AddInParameter("@lock_name", SqlDbType.VarChar, 255).Value = LockNameString(Lockname)
                    cmd.AddInParameter("@lock_value", SqlDbType.Int).Value = LockValue
                    cmd.AddInParameter("@tstamp", SqlDbType.Binary).Value = TStamp

                    Con.ExecuteNonQuery(cmd)
                    If CType(cmd.Parameters("@tstamp_matches").Value, PMEReturnCode) = PMEReturnCode.PMFalse Then
                        TimestampMatches = False
                        iRet = PMEReturnCode.PMRecordChanged
                    Else
                        ' It matches, so lock
                        TimestampMatches = True
                    End If
                End Using

                If TimestampMatches Then
                    'Lock 
                    iRet = LockKey(Con, Lockname, LockValue, _SiriusUser, LockedBy)
                    If iRet <> PMEReturnCode.PMTrue Then '
                        If (LockedBy <> "") AndAlso (LockedBy <> "ERROR") Then
                            ' Locked, by another user
                            iRet = PMEReturnCode.PMRecordInUse
                        End If
                    End If
                End If
            End If
            If iRet <> PMEReturnCode.PMTrue Then
                Select Case iRet
                    Case PMEReturnCode.PMRecordChanged
                        Dim oSAMErrorCollection As New SAMErrorCollection
                        oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.RecordChanged,
                                                            SAMBusinessErrors.RecordChanged.ToString,
                                                            "The timestamp for this record has changed and the record has therefore been changed by another user:" & LockNameString(Lockname) & " = " & LockValue.ToString)
                        oSAMErrorCollection.CheckForErrors()

                    Case PMEReturnCode.PMRecordInUse
                        ' The record is already locked
                        Dim oSAMErrorCollection As New SAMErrorCollection
                        oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.RecordLockedByAnotherUser,
                                                            SAMBusinessErrors.RecordLockedByAnotherUser.ToString,
                                                            "The record is locked by another user:" & LockNameString(Lockname) & " = " & LockValue.ToString & " Locked By = " & LockedBy)
                        oSAMErrorCollection.CheckForErrors()

                    Case Else
                        ' Some other sort of error
                        Dim oSAMErrorCollection As New SAMErrorCollection
                        oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                                            SAMBusinessErrors.COMComponentMethodFailed.ToString,
                                                             "bGIS.STS.CheckTSAndLock Failed")
                        oSAMErrorCollection.CheckForErrors()
                End Select
            End If

        Catch ex As Exception
            Dim oSAMErrorCollection As New SAMErrorCollection
            If iRet = PMEReturnCode.PMRecordChanged Then
                oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.RecordChanged,
                                                                            SAMBusinessErrors.RecordChanged.ToString,
                                                                            "The timestamp for this record has changed and the record has therefore been changed by another user:" & LockNameString(Lockname) & " = " & LockValue.ToString)

            ElseIf iRet = PMEReturnCode.PMRecordInUse Then
                oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.RecordLockedByAnotherUser,
                                                                            SAMBusinessErrors.RecordLockedByAnotherUser.ToString,
                                                                            "The record is locked by another user:" & LockNameString(Lockname) & " = " & LockValue.ToString & " Locked By = " & LockedBy)

            Else
                oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                                    SAMBusinessErrors.COMComponentMethodFailed.ToString,
                                                     "bGIS.STS.CheckTSAndLock Failed")

            End If
            oSAMErrorCollection.CheckForErrors()
        End Try


    End Sub
    Public Overloads Function CheckTSAndLock(ByVal BranchCode As String, ByVal Lockname As LockName, ByVal LockValue As Int32, ByVal TStamp As Byte()) As STSErrorType
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            Dim oResponse As STSErrorType

            oResponse = CheckTSAndLock(con, BranchCode,
                                        Lockname,
                                        LockValue,
                                        TStamp)

            Return oResponse

        End Using

    End Function

    Public Overloads Function CheckTSAndLock(ByVal Con As SiriusConnection, ByVal BranchCode As String,
                                            ByVal Lockname As LockName, ByVal LockValue As Int32, ByVal TStamp As Byte()) As STSErrorType

        Const ACMethodName As String = "CheckTSAndLock"

        'Dim oGIS As bGIS.STS = Nothing
        'Try
        '    oGIS = New bGIS.STS
        'Catch ex As Exception
        '    Dim TheError As STSErrorType = Nothing
        '    Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.FailedToCreateBackofficeComponent, "Failed to create bGIS.Application", ex.Message)
        '    STSErrorEx.SetContext(TheError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "CreateBusiness", True)
        '    Return TheError
        'End Try

        'Try
        '    SAMFunc.InitialiseGISSTS(Con:=Con, oGIS:=oGIS, SiriusUser:=_SiriusUser, sBranchCode:=BranchCode)
        'Catch ex As Exception
        '    Dim TheError As STSErrorType = Nothing
        '    Dim STSErrorEx As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.FailedToInitialiseBackofficeComponent, "Failed to initialise bGIS.Application", ex.Message)
        '    STSErrorEx.SetContext(TheError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "InitialiseBusiness", True)
        '    Return TheError
        'End Try

        Try
            Dim iRet As Int32
            Dim LockedBy As String = ""
            Dim TimestampMatches As Boolean
            'Check for Exclusive Locking setting
            Dim sOptionValue As String = String.Empty
            GetSystemOption(BranchCode, SystemOption.EnableExclusiveLocking, sOptionValue)
            If sOptionValue = "1" AndAlso (Lockname = CoreBusiness.LockName.ClaimId OrElse
                                           Lockname = CoreBusiness.LockName.InsuranceFolderCnt _
                                           OrElse Lockname = CoreBusiness.LockName.RenewalProcess) Then
                Return Nothing
            Else
                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_pmlock_last_unlock_check")
                    cmd.AddOutParameter("@tstamp_matches", SqlDbType.Int)
                    cmd.AddInParameter("@lock_name", SqlDbType.VarChar, 255).Value = LockNameString(Lockname)
                    cmd.AddInParameter("@lock_value", SqlDbType.Int).Value = LockValue
                    cmd.AddInParameter("@tstamp", SqlDbType.Binary).Value = TStamp

                    Con.ExecuteNonQuery(cmd)
                    If CType(cmd.Parameters("@tstamp_matches").Value, PMEReturnCode) = PMEReturnCode.PMFalse Then
                        TimestampMatches = False
                        iRet = PMEReturnCode.PMRecordChanged
                    Else
                        ' It matches, so lock
                        TimestampMatches = True
                    End If
                End Using
            End If

            If TimestampMatches Then
                'Lock 
                iRet = LockKey(Con, Lockname, LockValue, _SiriusUser, LockedBy)
                If iRet <> PMEReturnCode.PMTrue Then '
                    If (LockedBy <> "") AndAlso (LockedBy <> "ERROR") Then
                        ' Locked, by another user
                        iRet = PMEReturnCode.PMRecordInUse
                    End If
                End If
            End If

            'iRet = oGIS.CheckTSAndLock(LockNameString(Lockname), LockValue, TStamp, LockedBy, TimestampMatches)
            If iRet <> PMEReturnCode.PMTrue Then
                Select Case iRet
                    Case PMEReturnCode.PMRecordChanged
                        ' The Quote timestamp doesn't match and the quote, has been changed by another user
                        Dim TheError As STSErrorType = Nothing
                        Dim STSError As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.RecordChanged, "The timestamp for this record has changed and the record has therefore been changed by another user", LockNameString(Lockname) & " = " & LockValue.ToString)
                        STSError.SetContext(TheError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "RecordChanged", True)
                        Return TheError

                    Case PMEReturnCode.PMRecordInUse
                        ' The Quote is already locked
                        Dim TheError As STSErrorType = Nothing
                        Dim STSError As New STSErrorPublisher(STSErrorPublisher.STSErrorCodes.RecordLockedByAnotherUser, "The record is locked by another user", LockNameString(Lockname) & " = " & LockValue.ToString & " Locked By = " & LockedBy)
                        STSError.SetContext(TheError, HttpContext.Current.Request.Url.ToString(), ACMethodName, "RecordLocked", True)
                        Return TheError

                    Case Else
                        ' Some other sort of error
                        Dim STSErrorEx As New STSErrorPublisher(iRet, "Call to bGIS.STS.CheckTSAndLock failed.")
                        STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "CheckTSAndLock", True)
                End Select
            End If

        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher("Call to bGIS.STS.CheckTSAndLock failed with an exception.", ex)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "CheckTSAndLock", True)
        End Try

        ' Don't return an Error
        Return Nothing

    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="Con"></param>
    ''' <param name="Lockname"></param>
    ''' <param name="LockValue"></param>
    ''' <param name="_SiriusUser"></param>
    ''' <param name="LockedBy"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function LockKey(ByVal Con As SiriusConnection, ByVal Lockname As LockName,
                             ByVal LockValue As Integer,
                             ByVal _SiriusUser As SIRIUSUSER,
                             ByRef LockedBy As String) As Integer

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_add_pmlock")
            cmd.AddOutParameter("@locked_by", SqlDbType.VarChar, 255)
            cmd.AddInParameter("@key_name", SqlDbType.VarChar, 255).Value = LockNameString(Lockname)
            cmd.AddInParameter("@key_value", SqlDbType.Int).Value = LockValue
            cmd.AddInParameter("@user_id", SqlDbType.Int).Value = _SiriusUser.UserID
            Con.ExecuteNonQuery(cmd)

            If IsDBNull(cmd.Parameters("@locked_by").Value) Then
                LockedBy = ""
                LockKey = PMEReturnCode.PMTrue
            Else
                LockedBy = CType(cmd.Parameters("@locked_by").Value, String)
                Select Case LockedBy
                    Case Is = "ERROR"
                        ' Database error encountered
                        LockKey = PMEReturnCode.PMError
                        ' User already holds lock
                    Case Else
                        LockKey = PMEReturnCode.PMFalse
                End Select
            End If
        End Using
    End Function


    Private Function UnLockKey(ByVal Con As SiriusConnection, ByVal Lockname As LockName, ByVal LockValue As Integer, ByVal _SiriusUser As SIRIUSUSER) As Integer

        Dim nReturn As Integer

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_delete_pmlock")


            cmd.AddInParameter("@key_name", SqlDbType.VarChar, 30).Value = LockNameString(Lockname)
            cmd.AddInParameter("@key_value", SqlDbType.Int).Value = LockValue
            cmd.AddInParameter("@user_id", SqlDbType.Int).Value = _SiriusUser.UserID
            Dim nRecordsAffected As Integer = Con.ExecuteNonQuery(cmd)

            Select Case nRecordsAffected
                Case Is = 1
                    nReturn = PMEReturnCode.PMTrue
                    ' No lock removed
                Case Is = 0
                    ' PW091105 - Add optional param to prohibit error
                    ' when unlocking and the lock has gone...
                    nReturn = PMEReturnCode.PMFalse
                    ' Database error encountered
                Case Else
                    nReturn = PMEReturnCode.PMError
            End Select
            If nRecordsAffected <> PMEReturnCode.PMTrue Then
                ' Return NotFound if we can't unlock it.
                nReturn = PMEReturnCode.PMNotFound
                Exit Function
            End If
        End Using
        Return nReturn
    End Function
    Public Overloads Sub UnlockAndGetSAMTS(ByVal Con As SiriusConnection, ByVal BranchCode As String, ByVal Lockname As LockName, ByVal LockValue As Int32, ByRef TStamp As Byte())

        Try
            Dim iRet As Int32
            Dim tStampObject As Object = TStamp
            Dim ds As DataSet = Nothing

            'Check for Exclusive Locking setting
            Dim sOptionValue As String = ""
            GetSystemOption(BranchCode,
                                      SystemOption.EnableExclusiveLocking,
                                      sOptionValue)

            If sOptionValue = "1" AndAlso (Lockname = CoreBusiness.LockName.ClaimId _
                                           OrElse Lockname = CoreBusiness.LockName.InsuranceFolderCnt _
                                           OrElse Lockname = CoreBusiness.LockName.RenewalProcess) Then

                TStamp = DirectCast(BitConverter.GetBytes(DateTime.Now.Ticks), Byte())
                Return
            Else
                iRet = UnLockKey(Con, Lockname, LockValue, _SiriusUser)

                If iRet <> PMEReturnCode.PMTrue Then
                    Dim oSAMErrorCollection As New SAMErrorCollection
                    oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.RecordNotLockedByCurrentUser,
                                                        SAMBusinessErrors.RecordNotLockedByCurrentUser.ToString,
                                                         "The record was not locked by this user and has therefore not been unlocked:" & LockNameString(Lockname) & " = " & LockValue.ToString)
                    oSAMErrorCollection.CheckForErrors()

                End If

                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_pmlock_last_unlock_sel")
                    'cmd.AddOutParameter("@locked_by", SqlDbType.VarChar)

                    cmd.AddInParameter("@lock_name", SqlDbType.VarChar, 255).Value = LockNameString(Lockname)
                    cmd.AddInParameter("@lock_value", SqlDbType.Int).Value = LockValue

                    ds = Con.ExecuteDataSet(cmd, "unlocksel")
                End Using


                If ds.Tables("unlocksel").Rows.Count > 0 Then
                    ' return the timestamp
                    'r_vTimestamp = m_oDatabase.Records.Item(1).Fields.Item("tstamp").Value
                    tStampObject = ds.Tables("unlocksel").Rows(0)("tstamp")
                    TStamp = DirectCast(tStampObject, Byte())
                End If
            End If

        Catch ex As Exception
            Dim oSAMErrorCollection As New SAMErrorCollection
            oSAMErrorCollection.AddBusinessRule(SAMBusinessErrors.COMComponentMethodFailed,
                                                SAMBusinessErrors.COMComponentMethodFailed.ToString,
                                                 "bGIS.STS.UnlockAndGetTS Failed")
            oSAMErrorCollection.CheckForErrors()
        End Try

    End Sub

    Public Overloads Function UnlockAndGetTS(ByVal BranchCode As String, ByVal Lockname As LockName, ByVal LockValue As Int32, ByRef TStamp As Byte()) As STSErrorType
        Using con As SiriusConnection = New SiriusConnectionPMDAO(
                                            _SiriusUser.Username, _SiriusUser.SourceID,
                                            _SiriusUser.LanguageID,
                                            SiriusUserDefaults.AppName)
            ' begin transaction
            con.BeginTransaction()
            Try
                UnlockAndGetSAMTS(con, BranchCode, Lockname, LockValue, TStamp)
                ' commit transaction
                con.CommitTransaction()
            Catch
                ' rollback transaction
                con.RollbackTransaction()
                Throw
            End Try
        End Using
        Return Nothing
    End Function

    Public Overloads Function UnlockAndGetTS(ByVal Con As SiriusConnection, ByVal BranchCode As String, ByVal Lockname As LockName, ByVal LockValue As Int32, ByRef TStamp As Byte()) As STSErrorType

        Const ACMethodName As String = "UnlockAndGetTS"
        Try
            '''Check for Exclusive Locking setting
            Dim sOptionValue As String = String.Empty
            GetSystemOption(BranchCode, SystemOption.EnableExclusiveLocking, sOptionValue)
            If sOptionValue = "1" AndAlso (Lockname = CoreBusiness.LockName.ClaimId OrElse
                                           Lockname = CoreBusiness.LockName.InsuranceFolderCnt _
                                           OrElse Lockname = CoreBusiness.LockName.RenewalProcess) Then
                TStamp = DirectCast(BitConverter.GetBytes(DateTime.Now.Ticks), Byte())
                Return Nothing
            Else
                UnlockAndGetSAMTS(Con, BranchCode, Lockname, LockValue, TStamp)
            End If
        Catch ex As Exception
            Dim STSErrorEx As New STSErrorPublisher("Call to UnlockAndGetSAMTS failed with an exception.", ex)
            STSErrorEx.Raise(HttpContext.Current.Request.Url.ToString(), ACMethodName, "UnlockAndGetTS", True)
        End Try

        ' Don't return an Error
        Return Nothing

    End Function
    Public Sub New()

        ' Use a default one 
        _SiriusUser = New SIRIUSUSER

    End Sub

    Public Sub New(ByVal SiriusUser As SIRIUSUSER)

        ' Use the provided user
        _SiriusUser = SiriusUser

    End Sub

    Public Sub GetSystemOption(ByVal BranchCode As String, ByVal OptionNumber As Integer, ByRef OptionValue As String)

        Dim sCacheKey As String = String.Empty

        'get the cache
        '_oCache = HttpContext.Current.Cache()
        _oCache = HttpRuntime.Cache()

        'build a key from the code and option number
        sCacheKey = BranchCode + Convert.ToString(OptionNumber)

        'try to get value from cache first
        OptionValue = Convert.ToString(_oCache.Get(sCacheKey))

        If OptionValue Is Nothing Or OptionValue = "" Then

            Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)

                Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_System_Option")

                    cmd.AddInParameter("code", SqlDbType.VarChar).Value = Cast.NullIfDefault(BranchCode)
                    cmd.AddInParameter("option_number", SqlDbType.Int).Value = OptionNumber
                    cmd.AddOutParameter("option_value", SqlDbType.VarChar, 255)

                    con.ExecuteNonQuery(cmd)

                    OptionValue = Cast.ToString(cmd.Parameters("option_value").Value)

                End Using

            End Using

            'now add the item to the cache
            If Not OptionValue Is Nothing Then
                _oCache.Insert(sCacheKey, OptionValue)
            End If

        End If

    End Sub

    ''' <summary>
    ''' Retrieve the product option value
    ''' </summary>
    ''' <param name="lOptionNumber"></param>
    ''' <param name="lBranchId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetProductOption(ByVal lOptionNumber As Integer,
                              ByVal lBranchId As Integer,
                              Optional ByVal iClaimid As Integer = 0) As String

        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
            GetProductOption = GetProductOption(con, lOptionNumber, lBranchId, iClaimid)
        End Using

    End Function

    Public Function GetProductOption(ByVal con As SiriusConnection, ByVal lOptionNumber As Integer,
                              ByVal lBranchId As Integer,
                              Optional ByVal iClaimid As Integer = 0) As String

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Product_option")
            cmd.AddInParameter("@option_number", SqlDbType.Int).Value = lOptionNumber
            cmd.AddInParameter("@branch_id", SqlDbType.Int).Value = lBranchId
            cmd.AddOutParameter("@option_value", SqlDbType.VarChar, 10)
            If iClaimid <> 0 Then
                cmd.AddInParameter("@claim_id", SqlDbType.Int).Value = iClaimid
            End If
            con.ExecuteNonQuery(cmd)
            Return Cast.ToString(cmd.Parameters("@option_value").Value)
        End Using

    End Function

    Public Function GetProductDocumentOption(ByVal lProductID As Integer,
                               ByRef bPrintCertificate As Boolean, ByRef bPrintSchedule As Boolean, ByRef bPrintDebitNote As Boolean) As Integer
        Dim dt As DataTable = Nothing
        Using con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_ProductDocument_option")
                cmd.AddInParameter("@product_id", SqlDbType.Int).Value = lProductID
                dt = con.ExecuteDataTable(cmd)
            End Using
            If dt IsNot Nothing AndAlso dt.Rows.Count > 0 Then
                Dim dr As DataRow = dt.Rows(0)
                bPrintCertificate = Cast.ToBoolean(dr.Item("produce_certificate"), False)
                bPrintSchedule = Cast.ToBoolean(dr.Item("produce_schedule"), False)
                bPrintDebitNote = Cast.ToBoolean(dr.Item("produce_debit_note"), False)
            End If

        End Using

    End Function

    ''' <summary>
    ''' Generic Function designed to retrieve the values from a database on supplying the table name and 
    ''' column name and its value.
    ''' Presently it supports only two columns in where condition
    ''' </summary>
    ''' <param name="sTableName"></param>
    ''' <param name="sColumnName1"></param>
    ''' <param name="sValue1"></param>
    ''' <param name="r_dsOptionalValues"></param>
    ''' <param name="sColumnName2"></param>
    ''' <param name="sValue2"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Sub setDefaultValues(ByVal sTableName As String,
                                        ByVal sColumnName1 As String,
                                        ByVal sValue1 As String,
                                        ByRef r_dsOptionalValues As DataSet,
                                        Optional ByVal sColumnName2 As String = "",
                                        Optional ByVal sValue2 As String = "")

        Using Con As SiriusConnection = SiriusConnection.FromAny(connectionString:=SAMFunc.ConnectionString)
            Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_SAM_Get_Default_Values")
                cmd.AddInParameter("@table_name", SqlDbType.VarChar, 100).Value = sTableName
                cmd.AddInParameter("@column_name1", SqlDbType.VarChar, 100).Value = sColumnName1
                cmd.AddInParameter("@value1", SqlDbType.VarChar, 50).Value = sValue1
                If Not sColumnName2 = "" Then
                    cmd.AddInParameter("@column_name2", SqlDbType.VarChar, 100).Value = sColumnName2
                    cmd.AddInParameter("@value2", SqlDbType.VarChar, 50).Value = sValue2
                End If
                r_dsOptionalValues = Con.ExecuteDataSet(cmd, "DefaultValues")
            End Using
        End Using
    End Sub

    ''' <summary>
    ''' This function used to return the column name depends on the enum type
    '''</summary>   
    '''<param name="con">An object of the class SiriusConnection</param>
    Public Function UserAuthorityOptionString(ByVal UserAuthorityOptions As BaseImplementationTypes.UserAuthorityOptions) As String
        Select Case UserAuthorityOptions
            Case UserAuthorityOptions.HasUnrestrictedEnquiry
                Return "has_unrestricted_enquiry"
            Case UserAuthorityOptions.HasUnrestrictedUpdate
                Return "has_unrestricted_update"
            Case UserAuthorityOptions.HasWriteOffAuthority
                Return "has_write_off_authority"
            Case UserAuthorityOptions.HasPaymentsAuthority
                Return "has_payments_authority"
            Case UserAuthorityOptions.CanOverridePrePolicyDate
                Return "can_change_prepolicy_exchange_date"
            Case UserAuthorityOptions.CanOverridePrePolicyRate
                Return "can_change_prepolicy_exchange_rate"
            Case UserAuthorityOptions.CanOverrideDate
                Return "can_change_exchange_date"
            Case UserAuthorityOptions.CanOverrideRate
                Return "can_change_exchange_rate"
            Case UserAuthorityOptions.CanDuplicateClaimOverride
                Return "can_override_duplicate_claims"
            Case UserAuthorityOptions.CanOverridePostingPeriod
                Return "can_override_posting_period"
            Case UserAuthorityOptions.CanPerformBrokerTransfer
                Return "can_perform_broker_transfer"
            Case UserAuthorityOptions.HasClaimPaymentsAuthority
                Return "has_claim_Payments_authority"
            Case UserAuthorityOptions.CanUserChangeReserves
                Return "can_change_reserves_on_claim_payments"
            Case UserAuthorityOptions.CanMakeLiveInvoice
                Return "can_make_live_invoice"
            Case UserAuthorityOptions.CanMakeLivePayNow
                Return "can_make_live_paynow"
            Case UserAuthorityOptions.CanMakeLiveInstalments
                Return "can_make_live_instalments"
            Case UserAuthorityOptions.HasPaynowWriteOffAuthority
                Return "has_paynow_write_off_authority"
            Case UserAuthorityOptions.AllowAddRemoveRatingSections
                Return "allow_ratingsection_adddelete"
            Case UserAuthorityOptions.AllowEditRatingSections
                Return "allow_ratingsection_editing"
            Case UserAuthorityOptions.CanMakeLiveBankGuarantee
                Return "can_make_live_BankGuarantee"
            Case UserAuthorityOptions.CanOverrideCollectionDate
                Return "can_backdate_collection_date"

            Case UserAuthorityOptions.AllowEditAgentCommission
                Return "Edit_Default_Commission"

            Case UserAuthorityOptions.CanMakeLiveCashDeposit
                Return "can_make_live_cashdeposit"

            Case UserAuthorityOptions.BackDatedMtaAndMtcAuthorityType
                Return "out_of_sequence_mta_authority"

            Case UserAuthorityOptions.IsRecommender
                Return "is_recommender"
            Case UserAuthorityOptions.RecommenderCurrencyKey
                Return "recommender_currency_id"
            Case UserAuthorityOptions.RecommenderCurrencyAmount
                Return "recommender_currency_amount"
            Case UserAuthorityOptions.BackDatedMtaAndMtcAuthorityType
                Return "out_of_sequence_mta_authority"
            Case UserAuthorityOptions.DisplayReinsurance
                Return "display_reinsurance"
            Case UserAuthorityOptions.DisplayClaimReinsurance
                Return "display_claim_reinsurance"
            Case UserAuthorityOptions.IsClientManagerViewonly
                Return "is_view_only_client_manager"
            Case UserAuthorityOptions.AllowReverseAllocation
                Return "allow_reverse_allocations"
            Case UserAuthorityOptions.AgentEditableDuringMTAMTC
                Return "Agent_Editable_During_MTA_MTC"
            Case UserAuthorityOptions.EditDefaultCommissionNBRN
                Return "Edit_Default_Commission_NB_RN"
            Case UserAuthorityOptions.EditDefaultCommissionMTA
                Return "Edit_Default_Commission_MTA"
            Case UserAuthorityOptions.EditDefaultCommissionMTC
                Return "Edit_Default_Commission_MTC"
            Case UserAuthorityOptions.EditDefaultCommissionMTR
                Return "Edit_Default_Commission_MTR"
            Case UserAuthorityOptions.AllowReverseReceipt
                Return "allow_receipt_reversal"
                'wpr10
            Case UserAuthorityOptions.CanChangeInstalmentDefaultCurrency
                Return "can_change_instalment_default_currency"
            Case UserAuthorityOptions.CanUpdateInstalmentStatus
                Return "can_update_instalment_status"
            Case UserAuthorityOptions.CanUpdateInstalmentDueDate
                Return "can_edit_instalment_date"
            Case UserAuthorityOptions.EditInstalmentNoOfDays
                Return "edit_instalment_by_no_of_days"
            Case UserAuthorityOptions.HasManualJournalAuthority
                Return "has_manualjournal_authority"
            Case Else
                Return String.Empty
        End Select
    End Function

    ''' <summary>
    ''' A new method to check the wild card search
    '''</summary>   
    '''<param name="v_bDisableWildcardSearchOption"> </param>
    '''<param name="v_bEnablePartialWildcardSearchOption"> </param>
    '''<param name="r_sFieldValue"> </param>
    '''<param name="r_sErrorMessage"> </param>
    '''<remarks></remarks> 

    Public Function ValidWildcardSearch(
            ByVal v_bDisableWildcardSearchOption As Boolean,
            ByVal v_bEnablePartialWildcardSearchOption As Boolean,
            ByRef r_sFieldValue As String,
            Optional ByRef r_sErrorMessage As String = Nothing) As Boolean

        ValidWildcardSearch = True

        'If Niether option not set then valid
        If v_bDisableWildcardSearchOption = False And v_bEnablePartialWildcardSearchOption = False Then
            Exit Function
        End If

        'If no value then valid
        If Trim(r_sFieldValue) = "" Then
            Exit Function
        End If

        If v_bEnablePartialWildcardSearchOption = True Then
            'Allow wildcards but not the first character
            If Left(r_sFieldValue, 1) = "%" Then
                ValidWildcardSearch = False
                r_sErrorMessage = "Wildcard searches cannot begin with %"
                Exit Function
            End If
        ElseIf v_bDisableWildcardSearchOption = True Then
            'Cannot Contain %
            If Informations.inStr(r_sFieldValue, "%") <> 0 Then
                ValidWildcardSearch = False
                r_sErrorMessage = "Wilcard search not enabled. Please remove all % characters."
                Exit Function
            End If
        End If
    End Function


    Friend Overloads Function CheckInsuranceParams(ByVal con As SiriusConnection,
                                                   ByVal nInsurancefolderCnt As Integer,
                                                   ByVal nInsuranceFileCnt As Integer,
                                                   ByVal nRiskCnt As Integer,
                                                   Optional ByRef r_nInsuranceFolderCnt As Integer = 0,
                                                   Optional ByRef r_nSourceID As Integer = 0,
                                                   Optional ByVal bCheckStatusForUpdating As Boolean = False) As Boolean

        Dim ds As DataSet = Nothing

        Using cmd As SiriusCommand = SiriusCommand.FromProcedure("spu_ValidateInsuranceParams")

            cmd.AddInParameter("@insurance_folder_cnt", SqlDbType.Int).Value = nInsurancefolderCnt
            cmd.AddInParameter("@insurance_File_cnt", SqlDbType.Int).Value = nInsuranceFileCnt
            cmd.AddInParameter("@risk_cnt", SqlDbType.Int).Value = nRiskCnt

            ds = con.ExecuteDataSet(cmd, "details")

            If ds.Tables("details").Rows.Count = 0 Then
                Return False
            Else
                ' Get the insurance folder and source id
                r_nInsuranceFolderCnt = Cast.ToInt32(ds.Tables(0).Rows(0).Item("insurance_folder_cnt"), 0)
                r_nSourceID = Cast.ToInt32(ds.Tables(0).Rows(0).Item("source_id"), 0)

                ' EM Copy Risk change, ensure that the risk link is valid for updating
                If Not bCheckStatusForUpdating Then
                    Return True
                End If

                Dim riskStatus As String = Cast.ToString(ds.Tables("details").Rows(0)("status_flag"), RiskLinkStatusType.Unchanged)
                If riskStatus = RiskLinkStatusType.Unchanged OrElse riskStatus = RiskLinkStatusType.Renewed Then
                    Return False
                Else
                    Return True
                End If
            End If
            ' EM Copy Risk change, ensure that the risk link is valid for updating
        End Using

    End Function


    Public Overloads Function RunPRERuleset(
    ByVal con As SiriusConnection,
    ByVal NBQuoteInput As NBQuoteIn,
    ByVal lType As Int32,
    ByVal branchCode As String,
    ByVal transactionType As String,
    ByVal transactionTypeID As Integer,
    ByVal vQEMDREAdditionalDataArray As Object,
    ByVal vAdditionalDataArray As Object) As NBQuoteOut

        Dim iRet As System.Int32
        Dim oOut As New NBQuoteOut
        Dim lQuoteType As Int32
        Dim ErrEx As Exception = Nothing
        Dim oDataSetDefinitionRequest As New BaseGetDatasetDefinitionRequestType
        Dim oDataSetDefinitionResponse As New BaseGetDatasetDefinitionResponseType
        Dim oGISQEMDRE As bGISQEMDRE.DRE = Nothing
        Dim oSFIBusiness As New CoreSAMBusiness(_SiriusUser.Username, branchCode)
        Dim oGISDataSetControl As cGISDataSetControl.Application = Nothing

        Try
            oGISDataSetControl = New cGISDataSetControl.Application
        Catch ex As Exception
            ExceptionManager.Publish(ex)
            Throw New Exception("Failed to create cGISDatasetControl.Application class", ex)
        Finally
        End Try

        With oDataSetDefinitionRequest
            .DataModelCode = NBQuoteInput.DataModelCode
            .BranchCode = branchCode
        End With

        Try
            oDataSetDefinitionResponse = oSFIBusiness.GetDatasetDefinition(oDataSetDefinitionRequest)
        Catch ex As Exception
            ExceptionManager.Publish(ex)
            Throw New Exception("Failed to get Dataset Definition", ex)
        End Try

        Try
            oGISDataSetControl.LoadFromXML(oDataSetDefinitionResponse.XMLDatasetDefinition, NBQuoteInput.XMLDataset)
        Catch ex As Exception
            ExceptionManager.Publish(ex)
            Debug.WriteLine(ex.Message)
        End Try

        'create and initialise the bGISQEMDRE class correctly.
        Try
            oGISQEMDRE = New bGISQEMDRE.DRE
            ' Initialise the GIS
            SAMFunc.InitialisePRE(Con:=con, oGISQEMPRE:=oGISQEMDRE, SiriusUser:=_SiriusUser)
            oGISQEMDRE.InitialiseEngine(oDataSetDefinitionRequest.DataModelCode, transactionType)
            oGISQEMDRE.SetProcessModes(SAMComponentAction.PMEdit, 0, lType, Cast.ToObjString(transactionType), Now)
        Catch ex As Exception
            If oGISQEMDRE IsNot Nothing Then
                oGISQEMDRE.Dispose()
                oGISQEMDRE = Nothing
            End If
            ExceptionManager.Publish(ex)
            Throw New Exception("Failed to initialise bGISQEMDRE correctly", ex)
        End Try

        ' Dimension the Array
        Dim vAdditionalData As Object = Nothing

        ' Move the Data from the Classes to the Array
        vAdditionalData = Utilities.ClassesToArray(NBQuoteInput.AdditionalDataArray)

        'Calculate the QuoteType value
        Utilities.EncodeTransactionScreenAndType(r_lEncoded:=lQuoteType,
                                             v_lTransactionType:=transactionTypeID,
                                             v_lGISScreenId:=NBQuoteInput.RiskScreenId,
                                             v_lQuoteType:=lType)

        Try
            iRet = oGISQEMDRE.NBQuote(v_vQEMDREAdditionalArray:=vQEMDREAdditionalDataArray, v_lQuoteType:=lQuoteType, r_oDataset:=oGISDataSetControl, v_dtEffectiveDate:=NBQuoteInput.EffectiveDate,
                                      r_vAdditionalDataArray:=vAdditionalData)


        Catch ex As Exception
            If oGISQEMDRE IsNot Nothing Then
                oGISQEMDRE.Dispose()
                oGISQEMDRE = Nothing
            End If
            ErrEx = New Exception("Failed to call bGIS.Application.NBQuote", ex)
            ExceptionManager.Publish(ErrEx)
            Throw ErrEx
        Finally
            If oGISQEMDRE IsNot Nothing Then
                oGISQEMDRE.Dispose()
                oGISQEMDRE = Nothing
            End If
            If (ErrEx Is Nothing) Then
                If (iRet <> PMEReturnCode.PMTrue) Then
                    ErrEx = New Exception("bGISQEMDRE.DRE.NBQuote FAILED. Return Value = " + iRet.ToString)
                    ExceptionManager.Publish(ErrEx)
                    Throw ErrEx
                End If

            End If
        End Try

        ' Copy the Outputs from the In/Outs
        oGISDataSetControl.ReturnAsXML(oDataSetDefinitionResponse.XMLDatasetDefinition, oOut.XMLDataset)

        Return oOut

    End Function


    Public Function GetOtherPartyNumbering(ByVal con As SiriusConnection, ByVal nBranchId As Integer, ByVal OtherPartyName As String) As String

        Dim oOTNumbering As bSIRPolicyNumMaint.Business = Nothing
        Dim nRetrun As Integer
        Dim sFailureReason As String
        Dim sCode As String = ""

        oOTNumbering = New bSIRPolicyNumMaint.Business
        SAMFunc.InitialiseSBOObject(con, oOTNumbering, _SiriusUser, "bSIRPolicyNumMaint.Business")

        If sCode = "" Then
            sCode = ""
            nRetrun = oOTNumbering.GenerateClientCode(v_sPartyType:="OTHERPARTY", v_iSourceID:=nBranchId, r_sGeneratedClientCode:=sCode, r_sFailureReason:=sFailureReason, v_sType:="OTHERPARTY", v_sTradeName:=OtherPartyName)
        End If

        Return sCode

    End Function
End Class

