SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


Execute DDLDropProcedure 'spu_delete_claim_details'
GO


CREATE PROCEDURE spu_delete_claim_details  
    @claim_id int  
AS  
  
/****************************************************************************************************/  
/* spu_delete_claim_details deletes all claim details from the tables used for claims        */  
/* except the Claim table itself. This is to clean out old data prior to new data being     */  
/* added from temporary work tables. Care must be taken with the delete order due   */  
/* to foreign key constraints. This procedure should always be included in a        */  
/* transaction with the update procedure 'spu_update_claim_from_work'.       */  
/*                                                                                                      */  
/* 1 parameter is passed in - @claim_id                     */  
/*                                                                                                      */  
/*                                                                                                      */  
/* A failure in this procedure will be passed back to the calling procedure.        */  
/****************************************************************************************************/  
/* Revision Description of Modification     Date        Who         */  
/* --------         ---------------------------         ----        ---     */  
/* 1.0      Original                    26/04/2001  RWH */  
/*                                      */  
/***************************************************************************************************/  
  
---------------------------------------------  
-- Delete tax_calculation entries for payments
---------------------------------------------  
DELETE Tax_Calculation
FROM Tax_Calculation tc with (nolock)
INNER JOIN Claim_Payment cp with (nolock) ON 
	tc.claim_payment_id = cp.claim_payment_id
WHERE cp.claim_id = @claim_id  

---------------------------------------------  
-- Delete tax_calculation entries for receipts
---------------------------------------------  
DELETE Tax_Calculation
FROM Tax_Calculation tc with (nolock)
INNER JOIN Claim_Receipt cr with (nolock) ON 
	tc.claim_receipt_id = cr.claim_receipt_id
WHERE cr.claim_id = @claim_id  

---------------------------------------------  
-- Delete claim payment items
---------------------------------------------  
Delete Claim_Payment_Item
FROM Claim_Payment_Item cpi with (nolock)
INNER JOIN Claim_Payment cp  with (nolock) ON 
	cpi.claim_payment_id = cp.claim_payment_id
WHERE cp.claim_id = @claim_id  

---------------------------------------------  
-- Delete claim receipt items
---------------------------------------------  
Delete Claim_Receipt_Item
FROM Claim_Receipt_Item cri with (nolock)
INNER JOIN Claim_Receipt cr with (nolock) ON 
	cri.claim_receipt_id = cr.claim_receipt_id
WHERE cr.claim_id = @claim_id  

---------------------------------------------  
-- Delete from Payment  
---------------------------------------------  
DELETE FROM Claim_Payment  
WHERE claim_id = @claim_id  
  

---------------------------------------------  
-- Delete from Receipt  
---------------------------------------------  
DELETE FROM Claim_Receipt  
WHERE claim_id = @claim_id  
  
---------------------------------------------  
-- Delete from Reserve  
---------------------------------------------  
DELETE Reserve  
WHERE claim_peril_id IN (SELECT     claim_peril_id  
            FROM        claim_Peril  with (nolock)
            WHERE   claim_id = @claim_id)  
  
-----------------------------------------------  
-- Delete from Recovery  
-----------------------------------------------  
DELETE Recovery  
WHERE claim_peril_id IN (SELECT     claim_peril_id  
            FROM        claim_Peril  with (nolock)
            WHERE   claim_id = @claim_id)  
  
---------------------------------------------------  
-- Delete from Claim_Party_Claim - which is no longer used...  
---------------------------------------------------  
DELETE Claim_Party_Claim  
WHERE claim_id = @claim_id  
  
---------------------------------------------------  
-- Delete from Claim_Party_Link  
---------------------------------------------------  
DELETE Claim_Party_Link  
WHERE claim_id = @claim_id  
  
---------------------------------------------  
-- Delete from Peril_Party  
---------------------------------------------  
DELETE Peril_Party  
WHERE claim_id = @claim_id  
  
---------------------------------------------  
-- Delete from Claim_Peril  
---------------------------------------------  
DELETE Claim_Peril  
WHERE claim_id = @claim_id  
  
---------------------------------------------  
-- Delete from Claim_Risk  
---------------------------------------------  
DELETE Claim_Risk  
WHERE claim_id = @claim_id  
  
---------------------------------------------------------------------  
-- Delete from Claim_user_defined_risk_data  
---------------------------------------------------------------------  
DELETE Claim_user_defined_risk_data  
WHERE claim_id = @claim_id  
  
------------------------------------------------------------  
-- Delete from user_defined_peril_data  
------------------------------------------------------------  
DELETE user_defined_peril_data  
WHERE claim_id = @claim_id  
  
---------------------------------------------------------  
-- Delete from Claim_Expert_Service  
---------------------------------------------------------  
DELETE Claim_Expert_Service  
WHERE claim_id = @claim_id  
  
---------------------------------------------  
-- Delete from Claim_Party  
---------------------------------------------  
DELETE Claim_Party  
WHERE claim_id = @claim_id  
  
---------------------------------------------  
-- Delete from Claim_ri_arrangement_line  
---------------------------------------------  
DELETE Claim_ri_arrangement_line  
WHERE claim_id = @claim_id  
  
---------------------------------------------  
-- Delete from Claim_xol_arrangement  
---------------------------------------------  
DELETE Claim_xol_arrangement  
WHERE claim_id = @claim_id  
  
---------------------------------------------  
-- Delete from Claim_ri_arrangement  
---------------------------------------------  
DELETE Claim_ri_arrangement  
WHERE claim_id = @claim_id  
  


GO


