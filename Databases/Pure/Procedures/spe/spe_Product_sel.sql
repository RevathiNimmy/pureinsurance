
EXECUTE DDLDropProcedure 'spe_Product_sel'
GO
CREATE PROCEDURE spe_Product_sel  
	@product_id INT
AS  
	SELECT          p.product_id,
 p.caption_id,  
 p.code,  
					p.DESCRIPTION,
 p.effective_date,  
 p.is_deleted,  
 p.scheme_agency_ref,  
 p.block_no,  
 p.is_tax_suppressed,  
 p.quote_auto_numbering_id,  
 p.is_short_period_rated,  
 p.is_midnight_renewal,  
 p.is_auto_renewable,  
 p.renewal_period,  
 p.policy_auto_numbering_id,  
 p.prov_claim_auto_numbering_id,  
 p.full_claim_auto_numbering_id,  
 p.is_accumulation,  
 p.ri_pointer,  
 p.report_pointer,  
 p.claim_year_to_check,  
 p.max_single_claim_value,  
 p.max_number_of_claim,  
 p.max_total_claim_value,  
 p.nb_prorata,  
 p.mta_prorata,  
 p.round_prem_to_nearest_unit,  
 p.rounding_section_id,  
 p.is_policy_number_at_quote,  
 p.follow_up_time_frame,  
 p.grace_period,  
 p.prevent_cancelled_agents,  
 p.allow_positive_cancellation,  
 p.media_type_mandatory,  
 p.policy_style_id,  
 p.policy_style_mandatory,  
 p.allow_currency_change,  
 p.allow_loss_currency_change,  
 p.change_policy_number_at_renewal,  
 p.allow_standard_wording_edit,  
 p.hide_summary_at_renewal_acceptance,  
 p.is_true_monthly_policy,  
 p.anniversary_renewal_weeks,  
 p.suppress_reserves,  
 p.suppress_payments,  
 p.suppress_recoveries,  
 p.unified_renewal_day,  
 p.lead_allow_consolidated_commission,  
					p.lead_month_in_cycle,
 a1.short_code,  
 sub_allow_consolidated_commission,  
 sub_month_in_cycle,  
 a.short_code,  
 can_make_live_invoice,  
 can_make_live_instalments,  
 can_make_live_paynow,  
 p.produce_schedule,  
 p.produce_certificate,  
 p.produce_debit_note,  
 p.default_payment_method,  
 p.TradeNBOnline,  
 p.TradeMTAOnline,  
 p.TradeRNLOnline,  
 p.OnlineTradingCommencedOn,  
 p.is_renewable,  
 p.is_renewal_selection_enabled,  
 p.true_monthly_policy_renewal_communication,  
 p.renewal_selection_man_review_template_id,  
 p.renewal_selection_man_review_attachment_template_id,  
 p.renewal_selection_invite_template_id,  
 p.renewal_selection_invite_attachment_template_id,  
 p.renewal_selection_update_template_id,  
 p.renewal_selection_update_attachment_template_id,  
 p.is_renewal_invite_enabled,  
 p.renewal_invite_man_review_template_id,  
 p.renewal_invite_man_review_attachment_template_id,  
 p.renewal_invite_invite_template_id,  
 p.renewal_invite_invite_attachment_template_id,  
 p.renewal_invite_update_template_id,  
 p.renewal_invite_update_attachment_template_id,  
 p.is_renewal_update_enabled,  
 p.renewal_update_man_review_template_id,  
 p.renewal_update_man_review_attachment_template_id,  
 p.renewal_update_invite_template_id,  
 p.renewal_update_invite_attachment_template_id,  
 p.renewal_update_update_template_id,  
 p.renewal_update_update_attachment_template_id,  
 p.is_agent_renewal_selection_enabled,  
 p.is_agent_renewal_invite_enabled,  
 p.is_agent_renewal_update_enabled,  
 p.agent_renewal_man_review_template_id,  
 p.agent_renewal_man_review_report_id,  
 p.agent_renewal_invite_template_id,  
 p.agent_renewal_invite_report_id,  
 p.agent_renewal_update_template_id,  
 p.agent_renewal_update_report_id,  
					dt1.code       AS renewal_selection_man_review_template_code,
					dt2.code       AS renewal_selection_man_review_attachment_template_code,
					dt3.code       AS renewal_selection_invite_template_code,
					dt4.code       AS renewal_selection_invite_attachment_template_code,
					dt5.code       AS renewal_selection_update_template_code,
					dt6.code       renewal_selection_update_attachment_template_code,
					dt7.code       AS renewal_invite_man_review_template_id,
					dt8.code       AS renewal_invite_man_review_attachment_template_code,
					dt9.code       AS renewal_invite_invite_template_code,
					dt10.code      AS renewal_invite_invite_attachment_template_code,
					dt11.code      AS renewal_invite_update_template_code,
					dt12.code      AS renewal_invite_update_attachment_template_code,
					dt13.code      AS renewal_update_man_review_template_code,
					dt14.code      AS renewal_update_man_review_attachment_template_code,
					dt15.code      AS renewal_update_invite_template_code,
					dt16.code      AS renewal_update_invite_attachment_template_code,
					dt17.code      AS renewal_update_update_template_code,
					dt18.code      AS renewal_update_update_attachment_template_code,
					dt19.code      AS agent_renewal_man_review_template_code,
					r1.DESCRIPTION AS agent_renewal_man_review_report_code,
					dt20.code      AS agent_renewal_invite_template_code,
					r2.DESCRIPTION AS agent_renewal_invite_report_code,
					dt21.code      AS agent_renewal_update_template_code,
					r3.DESCRIPTION AS agent_renewal_update_report_code,
 multiple_claims_payments,  
 max_unauthorised_claim_value,  
 max_unauthorised_no_claim_payments,  
 run_authorisation_scripts_claim_payments,  
 p.BankAccount_Id,  
 p.Claim_Value_For_Large_Loss_Advice,  
 p.Inclusion_of_CoInsurers_On_Claims,  
 p.Allow_Negative_Reserve,  
 p.Ext_Clm_Handler_Acknowledged_Task_Allowed_Time,  
 p.Ext_Clm_Handler_Supply_Pre_Report_Task_Allowed_Time,  
 p.Valid_Policy_Version_At_Loss_Date,  
 p.Is_Gross_Claim_Payment_Amount,  
 p.Claim_Task_Group,  
 p.Claim_User_Group,  
 p.Claims_UDT_A,  
 p.Claims_UDT_B,  
 p.Claims_UDT_C,  
 p.Claims_UDT_D,  
 p.Claims_UDT_E,  
 p.Is_Duplicate_Claim_Check_Enabled,  
 p.Is_Advanced_Tax_Script_Enabled,  
 p.Is_Payment_Ref_Check_Enabled,  
 p.is_Recommend_Claim_Payments,  
					p.out_of_sequence_mta_dates,
 p.out_of_sequence_mta_allocation,  
 p.default_renewal_months,  
 p.Payment_Cannot_Exceed_Reserve,  
 p.enable_mtc_rating_rule,  
     p.can_make_live_bankguarantee,  
 p.Cover_Note_Default_Period,  
 p.Cover_Note_reused_upto,  
 p.Cover_Note_doc_Template_id,  
 p.Cover_Note_numbering_id,  
 dt22.code,  
  p.allow_backdated_mtas,  
p.out_of_sequence_mta_UserGroup,  
p.out_of_sequence_mta_TaskGroup,  
p.use_nb_payment_term_at_renselection,  
					p.is_roundoff_to_zero,
check_mediatype_status_at_claim_payment,  
check_mediatype_status_at_policy_refund,
change_ren_policy_no_auto,
					can_make_live_cashDeposit,
p.ri_manual_premium_adjustment,
p.allow_backdated_can,
p.tmpautrenfac,
p.allow_written_status,   
p.written_task_manager_days,  
p.written_rem_user_group,  
p.written_rem_task_group,
					use_prior_term_scheme_at_ren,
					p.bind_renewal_without_invitation,
					p.is_enable_PrePayment,
					p.Mandatory_Risk_Type_Id,
					p.do_not_delete_renQuote_on_mta,				
					p.default_cover_to_date_to_last_day,
					p.unified_renewal_date_is_read_only,
					p.is_reserves_read_only,
					p.is_recoveries_read_only,
					p.is_payments_read_only,
					p.Quote_all_risk_NB,
					p.Quote_all_risk_MTC,
					p.Quote_all_risk_MTA,
					p.Auto_Renew_BDMPolicy,
					p.Delete_And_ReRun_RenQuote,
					p.Quote_all_risk_RENEWAL,
					p.is_retain_policy_number_on_copy,
					p.Anniversary_Date_Editable,
					P.disable_cover_start_date_on_REN,
					p.use_policy_inception_date,
					p.Authorisation_Threshold,
					p.void_policy_version,
					p.is_quote_versioning,
					p.delete_quote_after,
					p.recovery_instalments_enabled
	FROM            Product p
	LEFT OUTER JOIN ACCOUNT a
		ON a.account_id = p.sub_suspense_account_id
	LEFT OUTER JOIN ACCOUNT a1
		ON a1.account_id = p.lead_suspense_account_id
	LEFT JOIN       Document_Template dt1
		ON dt1.document_template_id = p.renewal_selection_man_review_template_id
	LEFT JOIN       Document_Template dt2
		ON dt2.document_template_id = p.renewal_selection_man_review_attachment_template_id
	LEFT JOIN       Document_Template dt3
		ON dt3.document_template_id = p.renewal_selection_invite_template_id
	LEFT JOIN       Document_Template dt4
		ON dt4.document_template_id = p.renewal_selection_invite_attachment_template_id
	LEFT JOIN       Document_Template dt5
		ON dt5.document_template_id = p.renewal_selection_update_template_id
	LEFT JOIN       Document_Template dt6
		ON dt6.document_template_id = p.renewal_selection_update_attachment_template_id
	LEFT JOIN       Document_Template dt7
		ON dt7.document_template_id = p.renewal_invite_man_review_template_id
	LEFT JOIN       Document_Template dt8
		ON dt8.document_template_id = p.renewal_invite_man_review_attachment_template_id
	LEFT JOIN       Document_Template dt9
		ON dt9.document_template_id = p.renewal_invite_invite_template_id
	LEFT JOIN       Document_Template dt10
		ON dt10.document_template_id = p.renewal_invite_invite_attachment_template_id
	LEFT JOIN       Document_Template dt11
		ON dt11.document_template_id = p.renewal_invite_update_template_id
	LEFT JOIN       Document_Template dt12
		ON dt12.document_template_id = p.renewal_invite_update_attachment_template_id
	LEFT JOIN       Document_Template dt13
		ON dt13.document_template_id = p.renewal_update_man_review_template_id
	LEFT JOIN       Document_Template dt14
		ON dt14.document_template_id = p.renewal_update_man_review_attachment_template_id
	LEFT JOIN       Document_Template dt15
		ON dt15.document_template_id = p.renewal_update_invite_template_id
	LEFT JOIN       Document_Template dt16
		ON dt16.document_template_id = p.renewal_update_invite_attachment_template_id
	LEFT JOIN       Document_Template dt17
		ON dt17.document_template_id = p.renewal_update_update_template_id
	LEFT JOIN       Document_Template dt18
		ON dt18.document_template_id = p.renewal_update_update_attachment_template_id
	LEFT JOIN       Document_Template dt19
		ON dt19.document_template_id = p.agent_renewal_man_review_template_id
	LEFT JOIN       Report r1
		ON r1.report_id = p.agent_renewal_man_review_report_id
	LEFT JOIN       Document_Template dt20
		ON dt20.document_template_id = p.agent_renewal_invite_template_id
	LEFT JOIN       Report r2
		ON r2.report_id = p.agent_renewal_invite_report_id
	LEFT JOIN       Document_Template dt21
		ON dt21.document_template_id = p.agent_renewal_update_template_id
	LEFT JOIN       Report r3
		ON r3.report_id = p.agent_renewal_update_report_id
	LEFT JOIN       Document_Template dt22
		ON dt22.document_template_id = p.Cover_Note_doc_Template_id
	WHERE           product_id = @product_id

GO
