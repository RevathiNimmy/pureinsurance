SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Product_Claims_Workflow_Add'
GO

CREATE PROCEDURE spu_SIR_Product_Claims_Workflow_Add
    @product_id int,
    @claim_process_type_id tinyint = NULL,    
    @check_unpaid_status tinyint,
    @reinsurance_recovery tinyint,
    @salvage_recovery tinyint,
    @third_party_recovery tinyint,
    @external_claim_handling tinyint,
    @description_for_change_in_reserve tinyint,
    @claim_notification_doc_message tinyint,
    @generate_claim_notification_doc tinyint,
    @claim_payment_process tinyint,
    @check_deferred_reinsurance tinyint,
    @fast_track_claims tinyint,
    @reinsurance_payment tinyint,
    @description_for_change_in_payment tinyint,
    @cash_payment_process tinyint,
    @claim_payment_doc_message tinyint,
    @generate_claim_payment_doc tinyint,
    @make_further_payments tinyint,
	@UserId INT = NULL,
	@UniqueId VARCHAR(50) = NULL,
	@ScreenHierarchy VARCHAR(500) = NULL
AS    
    
    INSERT INTO Product_Claims_Workflow (
	product_id,
	claim_process_type_id,   
	check_unpaid_status, 
	reinsurance_recovery, 
	salvage_recovery, 
	third_party_recovery, 
	external_claim_handling, 
	description_for_change_in_reserve, 
	claim_notification_doc_message, 
	generate_claim_notification_doc, 
	claim_payment_process, 
	check_deferred_reinsurance, 
	fast_track_claims, 
	reinsurance_payment, 
	description_for_change_in_payment, 
	cash_payment_process, 
	claim_payment_doc_message, 
	generate_claim_payment_doc, 
	make_further_payments,
	UserId,
	UniqueId,
	ScreenHierarchy 
 	)
    VALUES (
	@product_id,
	@claim_process_type_id,
	@check_unpaid_status,
	@reinsurance_recovery,
	@salvage_recovery,
	@third_party_recovery,
	@external_claim_handling,
	@description_for_change_in_reserve,
	@claim_notification_doc_message,
	@generate_claim_notification_doc,
	@claim_payment_process,
	@check_deferred_reinsurance,
	@fast_track_claims,
	@reinsurance_payment,
	@description_for_change_in_payment,
	@cash_payment_process,
	@claim_payment_doc_message,
	@generate_claim_payment_doc,
	@make_further_payments,
	@UserId,
	@UniqueId,
	@ScreenHierarchy
	)
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
