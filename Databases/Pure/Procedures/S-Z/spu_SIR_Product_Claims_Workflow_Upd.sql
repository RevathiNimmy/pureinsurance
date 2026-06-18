SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Product_Claims_Workflow_Upd'
GO

CREATE PROCEDURE spu_SIR_Product_Claims_Workflow_Upd
    @product_id int,
    @claim_process_type_id tinyint,    
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
    IF NOT EXISTS (SELECT 1 FROM Product_Claims_Workflow WHERE product_id = @product_id)
  BEGIN 
    EXEC spu_SIR_Product_Claims_Workflow_Add @product_id ,  
    @claim_process_type_id ,  
    @check_unpaid_status ,  
    @reinsurance_recovery ,  
    @salvage_recovery ,  
    @third_party_recovery ,  
    @external_claim_handling ,  
    @description_for_change_in_reserve ,  
    @claim_notification_doc_message ,  
    @generate_claim_notification_doc ,  
    @claim_payment_process ,  
    @check_deferred_reinsurance ,  
    @fast_track_claims ,  
    @reinsurance_payment ,  
    @description_for_change_in_payment ,  
    @cash_payment_process ,  
    @claim_payment_doc_message ,  
    @generate_claim_payment_doc ,  
    @make_further_payments   
  END 
  ELSE
  
    UPDATE Product_Claims_Workflow 
      SET
	check_unpaid_status 			= @check_unpaid_status, 
	reinsurance_recovery 			= @reinsurance_recovery, 
	salvage_recovery 			= @salvage_recovery, 
	third_party_recovery 			= @third_party_recovery, 
	external_claim_handling 		= @external_claim_handling, 
	description_for_change_in_reserve 	= @description_for_change_in_reserve, 
	claim_notification_doc_message 		= @claim_notification_doc_message, 
	generate_claim_notification_doc 	= @generate_claim_notification_doc, 
	claim_payment_process 			= @claim_payment_process, 
	check_deferred_reinsurance 		= @check_deferred_reinsurance, 
	fast_track_claims 			= @fast_track_claims, 
	reinsurance_payment 			= @reinsurance_payment, 
	description_for_change_in_payment 	= @description_for_change_in_payment, 
	cash_payment_process 			= @cash_payment_process, 
	claim_payment_doc_message 		= @claim_payment_doc_message, 
	generate_claim_payment_doc 		= @generate_claim_payment_doc, 
	make_further_payments 			= @make_further_payments,
	UserId = @UserId,
    UniqueId = @UniqueId,
    ScreenHierarchy = @ScreenHierarchy
    WHERE product_id = @product_id
	AND claim_process_type_id = @claim_process_type_id

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
