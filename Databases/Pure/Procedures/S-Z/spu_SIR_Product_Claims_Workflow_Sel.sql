SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Product_Claims_Workflow_Sel'
GO

--*************************************************************************************************************
-- This SP will return claim roadmap configuration for a product and, if
-- supplied, for a roadmap (open/maintain/pay) or all three roadmaps
--*************************************************************************************************************
CREATE PROCEDURE spu_SIR_Product_Claims_Workflow_Sel 
    @product_id int,
    @claim_process_type_id tinyint = NULL
AS        
    SELECT  
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
	make_further_payments 
    FROM Product_Claims_Workflow 
    WHERE product_id = @product_id
	AND claim_process_type_id = ISNULL(@claim_process_type_id, claim_process_type_id)

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
