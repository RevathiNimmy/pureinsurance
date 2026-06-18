EXECUTE DDLDropProcedure 'spu_GetProductsForAgentsViaBranch'
GO

CREATE PROCEDURE spu_GetProductsForAgentsViaBranch
    @agent_party_cnt int,
    @source_id int,
    @restrict_product_access tinyint = 0 
AS
BEGIN
    
    IF (@restrict_product_access = 1 AND @agent_party_cnt>0) -- Restrict to sources
    BEGIN
        -- Get the products
        SELECT  p.product_id,
                p.code,
                pmc.caption,
                p.scheme_agency_ref,
                p.block_no, 
				p.lead_allow_consolidated_commission,
				p.sub_allow_consolidated_commission
        FROM    product p
        INNER JOIN pmcaption pmc
        ON      p.caption_id = pmc.caption_id
       	INNER JOIN party_agent_product pap  
        ON     (p.product_id = pap.product_id AND pap.party_cnt=@agent_party_cnt)
		       
    END
    ELSE
    BEGIN

        -- Get the products
        SELECT  p.product_id,
                p.code,
                pmc.caption,
                p.scheme_agency_ref,
                p.block_no, 
				p.lead_allow_consolidated_commission,
				p.sub_allow_consolidated_commission
        FROM    product p
        INNER JOIN pmcaption pmc
        ON      p.caption_id = pmc.caption_id

    END

END 
GO
