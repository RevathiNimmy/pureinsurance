SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_risktype_ri_model_usage_saa'
EXECUTE DDLDropProcedure 'spu_Risk_Type_RI_Model_Usage_saa'
GO


CREATE PROCEDURE spu_Risk_Type_RI_Model_Usage_saa
    @risk_type_id int,
    @is_deferred tinyint
AS

    IF IsNull(@is_deferred, 0) = 0
        SELECT  r.risk_type_id,
                r.ri_band,
                r.ri_model_id,
                r.[description],
                m.[description],
                r.is_deleted,
                r.effective_date,
                r.expiry_date,
            	r.portfolio_transfer_from_cnt,
            	r.risk_type_ri_model_usage_cnt,
            	0, -- item status, 0 = unchanged
                rb.description
        FROM    risk_type_ri_model_usage r
        JOIN    ri_model m 
                ON r.ri_model_id = m.ri_model_id
        JOIN    ri_band rb
                ON rb.ri_band_id = r.ri_band
        WHERE   r.risk_type_id = @risk_type_id
        AND     m.ri_model_type <> 2 -- not deferred
        ORDER BY 
                r.ri_band ASC,
                m.[description] ASC,
                r.is_deleted DESC,
                r.effective_date DESC
    ELSE
        SELECT  r.risk_type_id,
                r.ri_band,
                r.ri_model_id,
                r.[description],
                m.[description],
                r.is_deleted,
                r.effective_date,
                r.expiry_date,
            	r.portfolio_transfer_from_cnt,
            	r.risk_type_ri_model_usage_cnt,
            	0, -- item status, 0 = unchanged
                rb.description
        FROM    risk_type_ri_model_usage r
        JOIN    ri_model m 
                ON r.ri_model_id = m.ri_model_id
        JOIN    ri_band rb
                ON rb.ri_band_id = r.ri_band
        WHERE   r.risk_type_id = @risk_type_id
        AND     m.ri_model_type = 2 -- deferred
        ORDER BY 
                r.ri_band ASC,
                m.[description] ASC,
                r.is_deleted DESC,
                r.effective_date DESC
        
GO


