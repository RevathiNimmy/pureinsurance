
SET Quoted_IdentIfier  Off
GO
SET Ansi_Nulls  ON
GO

EXECUTE ddlDropProcedure 'spu_cashlistitem_claim_link_Sel'
GO

CREATE Procedure spu_cashlistitem_claim_link_Sel  
    @Claim_Payment_id bigint  
As  
SELECT
    
    CL.CashList_id,
    CLI.cashlistitem_id,  
    CL.currency_id,  
    CL.Company_id,
    CLI.Amount,  
    CLI.Mediatype_id
  
FROM cashlistitem_claim_link L  
    JOIN CashListItem CLI 
    ON L.cashlistitem_id = CLI.cashlistitem_id   
    JOIN CashList CL 
    ON CLI.CashList_id = CL.CashList_Id  
WHERE is_deleted = 0 
AND Claim_Payment_id  = @Claim_Payment_id 
