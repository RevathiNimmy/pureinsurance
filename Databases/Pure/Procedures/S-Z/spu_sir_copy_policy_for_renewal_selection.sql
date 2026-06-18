-- =============================================
-- Author:Vidya Rangdale
-- Create date: 12/09/2014
-- Description:	Sets the job for copy policies for renewal selection
-- =============================================

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO


EXECUTE DDLDROPPROCEDURE 'spu_sir_copy_policy_for_renewal_selection'  
GO

CREATE PROCEDURE spu_sir_copy_policy_for_renewal_selection  
    @old_insurance_file_cnt int,  
    @new_insurance_file_cnt int OUTPUT,  
    @event varchar(50),  
   	@DiscountPercentage numeric(11,8),
	@DiscountReasonID int = NULL,
	@MatchDiscountedPremiumFlag tinyint,
	@DiscountedPremium numeric(19,4),
	@DiscountRecurringTypeId int = NULL,
	@alternate_reference varchar(20) = 0,
	@OriginalProductID int,
	@ProductID int,
	@RenewalProductID int,	
	@CoverStartDate datetime,
    @ExpiryDate datetime,
	@RenewalDate datetime,
	@InceptionTPI datetime,
	@AnniversaryDate datetime,
	@AnniversaryCopy tinyint,
	@RenewalDayNumber int,
	@PutOnNextInstalmentRenewal bit,
	@InceptionDate datetime, 
	@EventDescription varchar(50),
	@UnderwritingYearID int,
	@PolicyVersion int,
	@UserId int = null,
	@Insurance_ref varchar(30) = null,
	@PaymentMethod VARCHAR(100)=NULL
AS  
  
BEGIN  
  
/*  
    Copy a policy, and its related text_file entries  
    CTAF 090701  
*/  
  
DECLARE @old_ins_folder_cnt int  
DECLARE @new_ins_folder_id int  
DECLARE @new_ins_file_id int  
DECLARE @table varchar(70)  
DECLARE @error int  
DECLARE @code varchar(30)  
DECLARE @old_source_id INT  
DECLARE @old_branch_id INT /*Sub Branch*/  
DECLARE @new_source_id INT  
DECLARE @new_branch_id INT /*Sub Branch*/  
DECLARE @original_insurance_file_type_id  INT

DECLARE @default_Preferred_Correspondence INT
DECLARE @party_cnt INT

 
DECLARE @insurance_file_type char(10) = 'RENEWAL'
DECLARE @insurance_file_type_id int
DECLARE @nEvent_type_id int = 14
--get the type id from the lookup table
SELECT @insurance_file_type_id = insurance_file_type_id FROM insurance_file_type with (nolock) WHERE code = @insurance_file_type

DECLARE @insurance_file_status_id int = NULL

IF @DiscountReasonID = 0
BEGIN
	SET @DiscountReasonID = NULL
END

IF @DiscountRecurringTypeId = 0
BEGIN
	SET @DiscountRecurringTypeId = NULL
END
-- Get the values for the original policy version using old insurance_file_cnt
SELECT  
    @old_ins_folder_cnt = insurance_folder_cnt,  
    @old_source_id = source_id,  
    @old_branch_id = branch_id,
    @original_insurance_file_type_id = insurance_file_type_id,	
	@party_cnt = insured_cnt
FROM Insurance_file  
WHERE insurance_file_cnt = @old_insurance_file_cnt  

SELECT @DEFAULT_PREFERRED_CORRESPONDENCE =p.correspondence_type_id FROM Party p WHERE P.party_cnt = @party_cnt

  
SELECT @new_source_id = @old_source_id  
SELECT @new_branch_id = @old_branch_id  
SET @new_ins_file_id = 0  


INSERT INTO Insurance_file  
(insurance_file_structure_id, insurance_file_type_id, insurance_file_status_id, insurance_file_id, source_id, insurance_folder_cnt, insurance_ref, product_id, lead_insurer_cnt, lead_agent_cnt, lead_agent_percent, account_handler_cnt, insured_cnt, business_type_id, collect_type_id, collection_from_cnt, branch_id, currency_id, language_id, date_issued, cover_start_date, expiry_date, renewal_date, renewal_method_id, renewal_frequency_id, is_referred_at_renewal, lapsed_reason_id, lapsed_date, lapsed_description, is_referred_on_mta, policy_version, gemini_policy_status, gemini_business_type, deferred_ind, policy_ignore, broker_cnt, risk_code_id, Analysis_code_id, proposal_date, diary_date, review_date, renewal_day_number, Policy_type_id, indicator, clause, cover, area, long_term_undertaking_date, renewal_stop_code_id, vbs_type, vbs_status, is_insurer_rate_table, is_related_policies, is_retained_documents, schemes_postcode, paid_direct, scheme, brokerage_amount, is_minimum_brokerage_flag, annual_premium, this_premium, net_premium, commission_amount, iptable_amount, ipt_percentage, is_ipt_overridden, tax_amount, vatable_amount, vat_percentage, vat_amount, payment_method, user_defined_data_id, commission_percentage, invariant_key, insured_name, 
alternate_reference, is_client_invoiced, old_policy_number, quote_expiry_date, alternate_account_cnt, account_executive_cnt, anniversary_date, fsa_customer_category_id, fsa_contract_location_id, fsa_underwriter_cnt, fsa_type_of_sale_id, fsa_renewal_consent, policy_style_id
, underwriting_year_id, policy_status_id, edi_message_sent, return_premium_currency_id, exchange_rate_override_reason_id, base_currency_id, currency_base_xrate, agent_account_currency_id, agent_account_base_xrate, system_base_xrate, currency_base_date, account_base_date, 
system_base_date, inception_date_tpi, country_id,put_on_next_instalment_renewal,anniversary_copy,discount_recurring_type_id,discount_percentage,discount_reason_id,discounted_premium,match_discounted_premium_flag,lead_allow_consolidated_commission,sub_allow_consolidated_commission,posting_period_id,addon_created,balance_type,
intermediary_agent_account_id,terms_agreed,terms_agreed_date,inception_Date,policy_documents_issued_date,policy_documents_correct,error_notification_date,Base_Insurance_File_Cnt,risk_transfer_agreement,Policy_Deductibles_id,Policy_limits_id,renewal_premium,renewal_product_id,original_product_id,risk_transfer_editable,manual_discount_percentage,marked_for_collection,marked_date,DOPaymentTerms_id,CollectionFrequency_id,out_of_sequence_replaced,risk_processed,coins_placement
,Correspondence_Type,Default_Preferred_Correspondence,Is_Agent_Correspondence ,Sender_Email,Receiver_Email)  

SELECT insurance_file_structure_id, @insurance_file_type_id, null, @new_ins_file_id, @new_source_id, @old_ins_folder_cnt, ISNULL(@Insurance_ref,insurance_ref), inf.product_id, lead_insurer_cnt, lead_agent_cnt, lead_agent_percent, account_handler_cnt, insured_cnt, business_type_id, 
collect_type_id, collection_from_cnt, @new_branch_id, currency_id, language_id, date_issued, @CoverStartDate, @ExpiryDate, @RenewalDate, renewal_method_id, renewal_frequency_id, is_referred_at_renewal, lapsed_reason_id, lapsed_date, lapsed_description, 
is_referred_on_mta, @PolicyVersion, gemini_policy_status, gemini_business_type, deferred_ind, policy_ignore, broker_cnt, risk_code_id, Analysis_code_id, proposal_date, diary_date, review_date, @RenewalDayNumber, Policy_type_id, indicator, clause, cover, area, long_term_undertaking_date, renewal_stop_code_id, vbs_type, vbs_status, is_insurer_rate_table, is_related_policies, is_retained_documents, schemes_postcode, paid_direct, scheme, brokerage_amount, is_minimum_brokerage_flag, annual_premium, 
this_premium, net_premium, commission_amount, iptable_amount, ipt_percentage, is_ipt_overridden, tax_amount, vatable_amount, vat_percentage, vat_amount, ISNULL(@PaymentMethod,payment_method), user_defined_data_id, commission_percentage, invariant_key, insured_name, @alternate_reference, is_client_invoiced, old_policy_number, DATEADD(day,p.grace_period,GETDATE()), alternate_account_cnt, account_executive_cnt, @AnniversaryDate, fsa_customer_category_id, fsa_contract_location_id, fsa_underwriter_cnt, fsa_type_of_sale_id, fsa_renewal_consent, 
inf.policy_style_id, underwriting_year_id, policy_status_id, edi_message_sent, return_premium_currency_id, exchange_rate_override_reason_id, base_currency_id, currency_base_xrate, agent_account_currency_id, agent_account_base_xrate, system_base_xrate, currency_base_date, account_base_date, getdate(), @InceptionTPI, country_id, @PutOnNextInstalmentRenewal, @AnniversaryCopy, @DiscountRecurringTypeId, @DiscountPercentage, @DiscountReasonID, @DiscountedPremium, @MatchDiscountedPremiumFlag,inf.lead_allow_consolidated_commission,inf.sub_allow_consolidated_commission,posting_period_id,addon_created,balance_type,
intermediary_agent_account_id,terms_agreed,terms_agreed_date,inception_Date,policy_documents_issued_date,policy_documents_correct,error_notification_date,null,risk_transfer_agreement,Policy_Deductibles_id,Policy_limits_id,renewal_premium,renewal_product_id,original_product_id,risk_transfer_editable,manual_discount_percentage,marked_for_collection,marked_date,DOPaymentTerms_id,CollectionFrequency_id,null,risk_processed,coins_placement
,Correspondence_Type,Default_Preferred_Correspondence,Is_Agent_Correspondence,
Sender_Email,Receiver_Email
FROM Insurance_file inf  join product p on inf.product_id = p.product_id
WHERE insurance_file_cnt = @old_insurance_file_cnt 
  
-- Get the insurance_file  
SELECT @new_insurance_file_cnt = @@IDENTITY

INSERT INTO Insurance_file_system  
( insurance_file_cnt, endorsement_count, created_by_id, date_created, modified_by_id, last_modified, last_trans_date, last_trans_type_id, last_trans_description, last_trans_debit_credit, last_trans_document_ref, last_trans_cover_start_date, last_trans_expiry_date )  
SELECT @new_insurance_file_cnt, endorsement_count, created_by_id, date_created, modified_by_id, getdate(), last_trans_date, last_trans_type_id, last_trans_description, last_trans_debit_credit, last_trans_document_ref, last_trans_cover_start_date, last_trans_expiry_date  
FROM Insurance_file_system  
WHERE insurance_file_cnt = @old_insurance_file_cnt  


 INSERT INTO event_log (party_cnt,  insurance_folder_cnt, insurance_file_cnt,claim_cnt,document_cnt,old_address_cnt,new_address_cnt,campaign_id,document_type_id,report_type_id,event_type_id,user_id,event_date,description,  
 old_party_type_id,  event_log_subject_id,  account_key,  fsa_complaint_folder_cnt,  short_description,  priority_code,  is_completed,  peril_id,  case_id,  rtf_text,  is_manual_description,  batch_id,  bg_id  )    
 SELECT  top 1 party_cnt,  insurance_folder_cnt, @new_insurance_file_cnt,claim_cnt,document_cnt,old_address_cnt,new_address_cnt,campaign_id,document_type_id,report_type_id,@nEvent_type_id,@UserId,getdate(),@event,  
 old_party_type_id,  event_log_subject_id,  account_key,  fsa_complaint_folder_cnt,  short_description,  priority_code,  is_completed,  peril_id,  case_id,  rtf_text,  is_manual_description,  batch_id,  bg_id  
 FROM event_log WHERE insurance_file_cnt=@old_insurance_file_cnt and event_type_id=5


EXEC spu_copy_policy_agents @OldInsuranceFileCnt=@old_insurance_file_cnt,@NewInsuranceFileCnt=@new_insurance_file_cnt
--removed as creating duplicates in insurance file risk link
--EXEC spu_copy_insurance_file_risk_link @OldInsuranceFileCnt=@old_insurance_file_cnt,@NewInsuranceFileCnt=@new_insurance_file_cnt,@CopyDeletedRisks=0  
EXEC spu_copy_coinsurance @OldInsuranceFileCnt=@old_insurance_file_cnt,@NewInsuranceFileCnt=@new_insurance_file_cnt
EXEC spu_copy_sub_agent @OldInsuranceFileCnt=@old_insurance_file_cnt,@NewInsuranceFileCnt=@new_insurance_file_cnt
EXEC spu_copy_policy_standard_wordings @old_insurance_file_cnt=@old_insurance_file_cnt,@new_insurance_file_cnt=@new_insurance_file_cnt
exec spu_SIR_copy_insurance_file_associates @old_insurance_file_cnt=@old_insurance_file_cnt,@new_insurance_file_cnt=@new_insurance_file_cnt

--agent commission not required according to Prabodh
--EXEC spu_copy_agent_commission @OldInsuranceFileCnt=@old_insurance_file_cnt,@NewInsuranceFileCnt=@new_insurance_file_cnt
  
END
GO
