SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_RI_Model_saa'
GO


CREATE PROCEDURE spu_RI_Model_saa
    @ri_model_id int = null
AS

    If IsNull(@ri_model_id, 0) = 0
        Select  ri.ri_model_id,
                ri.code,
                ri.description,
                ri.is_deleted,
                ri.effective_date,
                ri.expiry_date,
                ri.ri_model_type,
                ri.fac_premium_type,
                ri.claim_allocation_type,
                ri.currency_id,
                c.description,
                ri.xol_clm_ri_model_id, 
                ri.xol_clm_limit,
                ri.xol_cat_ri_model_id, 
                ri.xol_cat_limit,
                ri.xol_cat_reinstatements,
                ri.treaty_premium_type
        From    RI_Model ri
        Left Join
                Currency c
                On c.currency_id = ri.currency_id
    Else
        Select  ri.ri_model_id,
                ri.code,
                ri.description,
                ri.is_deleted,
                ri.effective_date,
                ri.expiry_date,
                ri.ri_model_type,
                ri.fac_premium_type,
                ri.claim_allocation_type,
                ri.currency_id,
                c.description,
                ri.xol_clm_ri_model_id, 
                ri.xol_clm_limit,
                ri.xol_cat_ri_model_id, 
                ri.xol_cat_limit,
                ri.xol_cat_reinstatements,
                ri.treaty_premium_type
        From    RI_Model ri
        Left Join
                Currency c
                On c.currency_id = ri.currency_id
        Where   ri_model_id = @ri_model_id
    

GO

