SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_delete_claim'
GO

CREATE PROCEDURE spu_delete_claim      
    @claim_id int,      
    @status int OUTPUT      
AS      
      
-- *************************************************************************************************      
-- sp_delete_claim deletes all claim details from the temporary work tables      
-- used for claims.      
--      
-- 1 parameter is passed in - @claim_id      
--      
-- A failure in this procedure will be passed back to the calling procedure.      
--**************************************************************************************************      
-- Revision Description of Modification     Date        Who      
-- -------- ---------------------------     ----        ---      
-- 1.0      Original                        26/04/2001  RWH      
-- 1.1      Original                        16/12/2004  RVH Allow for products with manual numbering      
--**************************************************************************************************      
DECLARE @claim_number_id int,      
        @claim_number as varchar(255),      
        @original_Claim_id int      
      
------------------------------------------      
-- delete associated event log entries      
-- defined in the work claim link table      
-- that havent already been processed      
------------------------------------------      
DELETE event_log WHERE event_cnt IN      
(SELECT link_id FROM claim_link WITH (NOLOCK)      
 WHERE claim_id = @claim_id      
 AND processed = 0      
 AND link_type_id = 1)      
      
IF @@ERROR <> 0      
    GOTO Error_Routine   

------------------------------------------
-- delete associated task keys
-- defined in the work claim link tables
-- that havent already been processed
------------------------------------------
DELETE PMWrk_Task_Inst_Key where pmwrk_task_instance_cnt in
(SELECT link_id FROM claim_link WITH (NOLOCK)
 WHERE claim_id = @claim_id
 AND processed = 0
 AND link_type_id = 2)

IF @@ERROR <> 0
    GOTO Error_Routine	
      
------------------------------------------      
-- delete associated tasks      
-- defined in the work claim link tables      
-- that havent already been processed      
------------------------------------------      
DELETE pmwrk_task_instance where pmwrk_task_instance_cnt in      
(SELECT link_id FROM claim_link WITH (NOLOCK)      
 WHERE claim_id = @claim_id      
 AND processed = 0      
 AND link_type_id = 2)      
      
IF @@ERROR <> 0      
    GOTO Error_Routine      
    
------------------------------------------      
-- delete work claim links      
------------------------------------------      
DELETE claim_link WITH (ROWLOCK)      
WHERE claim_id = @claim_id      
      
IF @@ERROR <> 0      
    GOTO Error_Routine      
      
------------------------------------------      
-- Delete from tax_calculation      
------------------------------------------      
DELETE FROM tax_calculation WHERE claim_payment_item_id IN 
      
(

select claim_payment_item_id from Claim_Payment_Item WITH (ROWLOCK)
WHERE claim_payment_id in (Select claim_payment_id from Claim_Payment WITH (NOLOCK) where claim_id =@claim_id)

) 
      
IF @@ERROR <> 0  
    GOTO Error_Routine  
  
DELETE tax_calculation WITH (ROWLOCK)  
FROM tax_calculation wtc   
INNER JOIN claim_payment cp WITH (NOLOCK) ON  
cp.claim_payment_id = wtc.claim_payment_id  
WHERE cp.claim_id = @claim_id 
  
      
      
IF @@ERROR <> 0  
    GOTO Error_Routine  
  
------------------------------------------      
-- Delete from tax_calculation      
------------------------------------------      
DELETE 
FROM tax_calculation 
where claim_receipt_item_id in 

(

select claim_receipt_item_id from claim_receipt_item WITH (ROWLOCK)
WHERE claim_receipt_id in (Select claim_receipt_id from claim_receipt WITH (NOLOCK) where claim_id =@claim_id)

)
      
IF @@ERROR <> 0  
    GOTO Error_Routine  

------------------------------------------      
-- Delete from receipt_item      
------------------------------------------      
DELETE claim_receipt_item WITH (ROWLOCK)       
WHERE claim_receipt_id in (Select claim_receipt_id from claim_receipt WITH (NOLOCK) where claim_id =@claim_id)      
      
IF @@ERROR <> 0      
    GOTO Error_Routine      
      
------------------------------------------      
-- Delete from receipt      
------------------------------------------      
DELETE claim_receipt WITH (ROWLOCK)      
WHERE claim_id = @claim_id      
      
IF @@ERROR <> 0      
    GOTO Error_Routine      
      
------------------------------------------      
-- Delete from receipt_item      
------------------------------------------      
DELETE claim_payment_item WITH (ROWLOCK)       
WHERE claim_payment_id in (Select claim_payment_id from claim_payment WITH (NOLOCK) where claim_id =@claim_id)      
      
IF @@ERROR <> 0      
    GOTO Error_Routine      

--PN: 56211    
---------------------------------------------    
-- Delete from CashListItem_Claim_Link    
---------------------------------------------    
    
DELETE CashListItem_Claim_Link WITH (ROWLOCK)    
WHERE claim_payment_id in (Select claim_payment_id from claim_payment WITH (NOLOCK) where claim_id =@claim_id)    
    
IF @@ERROR <> 0    
    GOTO Error_Routine
	
---------------------------------------------      
-- Delete from payment      
---------------------------------------------      
      
DELETE claim_payment WITH (ROWLOCK)       
WHERE claim_id = @claim_id      
      
IF @@ERROR <> 0      
    GOTO Error_Routine      
      
---------------------------------------------      
-- Delete from Reserve      
---------------------------------------------      
DELETE Reserve WITH (ROWLOCK)       
WHERE claim_peril_id IN (SELECT claim_peril_id      
            FROM       claim_Peril WITH (NOLOCK)      
            WHERE   claim_id = @claim_id)      
      
IF @@ERROR <> 0      
    GOTO Error_Routine      
      
-----------------------------------------------      
-- Delete from Recovery      
-----------------------------------------------      
DELETE Recovery WITH (ROWLOCK)       
WHERE claim_peril_id IN (SELECT     claim_peril_id      
               FROM        claim_Peril WITH (NOLOCK)      
           WHERE   claim_id = @claim_id)      
      
IF @@ERROR <> 0      
    GOTO Error_Routine      
      
------------------------------------------------------------------      
-- Delete from claim_party_claim - which is no longer used...      
------------------------------------------------------------------      
DELETE claim_party_claim WITH (ROWLOCK)       
WHERE claim_id = @claim_id      
      
IF @@ERROR <> 0      
    GOTO Error_Routine      
      
-----------------------------------------------------------      
-- Delete from claim_party_link      
-----------------------------------------------------------      
DELETE claim_party_link WITH (ROWLOCK)       
WHERE claim_id = @claim_id      
      
IF @@ERROR <> 0      
    GOTO Error_Routine      
      
-----------------------------------------------------------      
-- Delete from claim_party      
-----------------------------------------------------------      
DELETE claim_party WITH (ROWLOCK)      
WHERE claim_id = @claim_id      
      
IF @@ERROR <> 0      
    GOTO Error_Routine      

-----------------------------------------------------------  
-- Delete from Claim_RI_Arrangement_line_Broker_Participants  
-----------------------------------------------------------  
DELETE Claim_RI_Arrangement_line_Broker_Participants WITH (ROWLOCK)
WHERE claim_ri_arrangement_line_id in 
(SELECT ri_arrangement_line_id FROM  Claim_ri_Arrangement_Line
WHERE  Claim_Id  = @Claim_id)

IF @@ERROR <> 0  
    GOTO Error_Routine 
        
-----------------------------------------------------------      
-- Delete from claim_ri_arrangement_line      
-----------------------------------------------------------      
DELETE claim_ri_arrangement_line WITH (ROWLOCK)      
WHERE claim_id = @claim_id      
      
IF @@ERROR <> 0      
    GOTO Error_Routine      
      
-----------------------------------------------------------      
-- Delete from claim_xol_arrangement      
-----------------------------------------------------------      
Delete  claim_xol_arrangement WITH (ROWLOCK)      
Where   claim_id = @claim_id      
      
IF @@ERROR <> 0      
    GOTO Error_Routine      
      
-----------------------------------------------------------      
-- Delete from claim_risk_ri_arrangement      
-----------------------------------------------------------      
DELETE claim_ri_arrangement WITH (ROWLOCK)      
WHERE claim_id = @claim_id      
      
IF @@ERROR <> 0      
    GOTO Error_Routine      
      
------------------------------------------------      
-- Delete from peril_party      
------------------------------------------------      
DELETE peril_party WITH (ROWLOCK)      
WHERE claim_id = @claim_id      
      
IF @@ERROR <> 0      
    GOTO Error_Routine      
      
-----------------------------------------------------------      
-- Delete from claim_peril      
-----------------------------------------------------------      
DELETE claim_peril WITH (ROWLOCK)      
WHERE claim_id = @claim_id      
      
IF @@ERROR <> 0      
    GOTO Error_Routine      
      
-----------------------------------------------------------      
-- Delete from claim_risk      
-----------------------------------------------------------      
DELETE claim_risk WITH (ROWLOCK)      
WHERE claim_id = @claim_id      
      
IF @@ERROR <> 0      
    GOTO Error_Routine      
      
-------------------------------------------------------------------------------      
-- Delete from claim_user_defined_risk_data      
------------------------------------------------------------------------------      
DELETE claim_user_defined_risk_data WITH (ROWLOCK)      
WHERE claim_id = @claim_id      
      
IF @@ERROR <> 0      
    GOTO Error_Routine      
      
----------------------------------------------------------------------      
-- Delete from user_defined_peril_data      
----------------------------------------------------------------------      
DELETE user_defined_peril_data WITH (ROWLOCK)      
WHERE claim_id = @claim_id      
      
IF @@ERROR <> 0      
    GOTO Error_Routine      
      
-----------------------------------------------------------------      
-- Delete from claim_expert_service      
-----------------------------------------------------------------      
DELETE claim_expert_service WITH (ROWLOCK)      
WHERE claim_id = @claim_id      
      
IF @@ERROR <> 0      
    GOTO Error_Routine      
      
-------------------------------------------------      
-- Delete from claim_party      
-------------------------------------------------      
DELETE claim_party WITH (ROWLOCK)      
WHERE claim_id = @claim_id      
      
IF @@ERROR <> 0      
    GOTO Error_Routine      
      
------------------------------------------      
-- abandon claim number when cancel from open claim      
------------------------------------------    SELECT @original_claim_id = Original_Claim_id FROM claim WHERE claim_id = @claim_id      
IF @original_claim_id IS NULL OR @original_claim_id = 0      
BEGIN      
    SELECT  @claim_number_id = prd.full_claim_auto_numbering_id,      
            @claim_number = wc.claim_number      
    FROM    product prd WITH (NOLOCK),      
            insurance_file ifi WITH (NOLOCK),      
            claim wc WITH (NOLOCK)      
    WHERE   wc.claim_id = @claim_id      
    AND ifi.insurance_file_cnt = wc.policy_id      
    AND ifi.product_id = prd.product_id      
      
    IF @@ERROR <> 0      
        GOTO Error_Routine      
      
    -- 1.1 Not applicable if there is no numbering scheme assigned at the product level      
    IF @claim_number_id > 0      
    BEGIN      
      -- add to abandon table      
      IF NOT EXISTS (SELECT 1 FROM abandoned_numbers WITH (NOLOCK)   
       WHERE numbering_scheme_id = @claim_number_id   
       AND abandoned_number = @claim_number)      
          EXEC spe_abandoned_numbers_add @claim_number_id, @claim_number      
    END      
      
    IF @@ERROR <> 0      
        GOTO Error_Routine      
END  

------------------------------------------     
-- Delete from Claim_Public_Text      
------------------------------------------      
DELETE  Claim_Public_Text WITH (ROWLOCK)      
WHERE   claim_Cnt = @claim_id      
      
IF @@ERROR <> 0      
    GOTO Error_Routine    
      
------------------------------------------      
-- Delete from claim      
------------------------------------------      
DELETE  claim WITH (ROWLOCK)      
WHERE   claim_id = @claim_id      
      
IF @@ERROR <> 0      
    GOTO Error_Routine      
      
------------------------------------------      
-- Delete from transaction_export_detail      
------------------------------------------      
DELETE  transaction_export_detail WITH (ROWLOCK)      
WHERE   transaction_export_folder_cnt IN (      
    SELECT  transaction_export_folder_cnt      
    FROM    transaction_export_folder WITH (NOLOCK)      
    WHERE   loss_id = @claim_id )      
  IF @@ERROR <> 0      
    GOTO Error_Routine      
      
------------------------------------------      
-- Delete from transaction_export_folder      
------------------------------------------      
DELETE  transaction_export_folder WITH (ROWLOCK)      
WHERE   loss_id = @claim_id      
      
IF @@ERROR <> 0      
    GOTO Error_Routine      
      
------------------------------------------      
-- Delete from stats_detail      
------------------------------------------      
DELETE  stats_detail WITH (ROWLOCK)      
WHERE   stats_folder_cnt IN (      
    SELECT  stats_folder_cnt      
    FROM    stats_folder WITH (NOLOCK)      
    WHERE   loss_id = @claim_id )      
      
IF @@ERROR <> 0      
    GOTO Error_Routine      
      
------------------------------------------      
-- Delete from stats_folder      
------------------------------------------      
DELETE  stats_folder WITH (ROWLOCK)      
WHERE   loss_id = @claim_id      
      
IF @@ERROR <> 0      
    GOTO Error_Routine      
      
SELECT @status = 0      
      
RETURN      
      
Error_Routine:      
      
    SELECT  @status = -1      
    RETURN      
    
  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
