SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_SAM_Get_Product_Claims_Workflow_Options
GO

--Start (Prakash C Varghese) - (Gap Fixing As told by Gaurav)
CREATE PROCEDURE spu_SAM_Get_Product_Claims_Workflow_Options
    @product_id INT,
    @claim_process_type_id INT  
AS  
BEGIN

    SELECT  
        check_unpaid_status AS CheckUnpaidStatus,  
        reinsurance_recovery AS ReinsuranceRecovery,                 
        salvage_recovery AS SalvageRecovery,                       
        third_party_recovery AS ThirdPartyRecovery,                    
        external_claim_handling AS ExternalClaimHandling,               
        description_for_change_in_reserve AS DescriptionForChangeInReserve,      
        claim_notification_doc_message AS ClaimNotificationDocMessage,         
        generate_claim_notification_doc AS GenerateClaimNotificationDoc,        
        claim_payment_process AS ClaimPaymentProcess,                  
        check_deferred_reinsurance AS CheckDeferredReinsurance,             
        fast_track_claims AS FastTrackClaims,                      
        reinsurance_payment AS ReinsurancePayment,                    
        description_for_change_in_payment AS DescriptionForChangeInPayment,      
        cash_payment_process AS CashPaymentProcess,                   
        claim_payment_doc_message AS ClaimPaymentDocMessage,               
        generate_claim_payment_doc AS GenerateClaimPaymentDoc,             
        make_further_payments AS MakeFurtherPayments                  
    FROM
        product_claims_workflow
    WHERE
        product_id=@product_id
        AND claim_process_type_id=@claim_process_type_id
    
END
--End (Prakash C Varghese) - (Gap Fixing As told by Gaurav)

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

