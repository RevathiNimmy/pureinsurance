SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Is_AddOn_Saved'
GO

CREATE PROCEDURE spu_Is_AddOn_Saved
    @insurance_file_cnt int,
    @party_cnt int  
AS  
  
SELECT  
    fee_amount,
    commission_percentage,
    commission_amount,
    tax_amount,	
    fsa_type_of_sale_id

FROM policy_fee  
WHERE  
    insurance_file_cnt = @insurance_file_cnt  
AND
    party_cnt = @party_cnt

GO

