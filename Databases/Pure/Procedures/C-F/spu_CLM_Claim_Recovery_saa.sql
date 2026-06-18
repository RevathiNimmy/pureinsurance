SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_CLM_Claim_Recovery_saa'
GO


CREATE PROCEDURE spu_CLM_Claim_Recovery_saa
    @peril_id int,
    @is_salvage tinyint
AS

	SELECT  r.recovery_id,
			r.claim_peril_id,
			rt.recovery_type_id,
			rt.description,
			c.currency_id,
			c.description,
			r.initial_reserve,
			r.revised_reserve,
			r.received_to_date,
			r.revision_count,
			r.tax_amount,
			cp.claim_id,
			rty.claims_is_post_taxes
	    FROM    recovery r
	    JOIN    claim_peril cp
	            ON cp.claim_peril_id = r.claim_peril_id
	    JOIN    recovery_type rt
	            ON rt.recovery_type_id = r.recovery_type_id
	    JOIN    currency c
	            ON c.currency_id = r.currency_id
	    JOIN    claim cl
	            ON cl.claim_id = cp.claim_id
	    JOIN    risk ri
	            ON ri.risk_cnt = cl.risk_type_id
	    JOIN    risk_type rty
	            ON rty.risk_type_id = ri.risk_type_id
	    WHERE   r.claim_peril_id = @peril_id
	    AND     rt.is_salvage = @is_salvage
	    ORDER BY
            r.recovery_id

GO

