SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_agent_product_usage_sel'
GO


CREATE PROCEDURE spu_agent_product_usage_sel
    @party_cnt INT
AS


SELECT product.product_id, product.code, product.description, product.scheme_agency_ref, product.block_no
FROM product, agent_product_usage
WHERE agent_product_usage.party_cnt = @party_cnt
AND agent_product_usage.effective_start_date <= GetDate()
AND agent_product_usage.effective_end_date >= GetDate()
AND agent_product_usage.product_id = product.product_id
GO


