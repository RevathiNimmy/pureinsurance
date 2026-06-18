SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_GetTaxTransdetail_ids'
GO

-- Created to retrieve all transdetail_id's for a given document and account.

-- 1.00 Created.							19/03/2002
-- RAW 13/01/2003 : PS187 : added new columns to result set
-- RAW 29/01/2003 : PS187 : added new columns to result set 
--                          and allow accountID parameter to become optional


CREATE PROCEDURE spu_ACT_GetTaxTransdetail_ids
			@document_id int,
			@account_id int = NULL

AS

SELECT 	transdetail_id, 
        comment             as tax_type_code,         -- RAW 13/01/2003 : PS187 : added
        amount              as base_tax_amount,       -- RAW 13/01/2003 : PS187 : added
        currency_amount     as currency_tax_amount,   -- RAW 13/01/2003 : PS187 : added
        currency_id,                                  -- RAW 13/01/2003 : PS187 : added
        currency_base_Xrate,                          -- RAW 13/01/2003 : PS187 : added
        -- RAW 29/01/2003 : PS187 : added
        (   SELECT  ISNULL(sum(aa.alloc_base_amount),0)          
                    FROM    allocationdetail aa    
                    WHERE   aa.transdetail_id = t.transdetail_id
        )                   as base_tax_amount_settled,   
        (   SELECT  ISNULL(sum(aa.alloc_ccy_amount),0)          
                    FROM    allocationdetail aa    
                    WHERE   aa.transdetail_id = t.transdetail_id
        )                   as currency_tax_amount_settled, 
        account_id   
FROM		TransDetail t
WHERE	document_id = @document_id
-- RAW 29/01/2003 : PS187 : added null condition
AND		(
            @account_id IS NULL OR
            @account_id = account_id 
        )
AND		spare like 'TAX%'




GO
SET QUOTED_IDENTIFIER  OFF    SET ANSI_NULLS  ON 
GO

