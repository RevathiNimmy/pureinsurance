SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Risk_RI_Value'
GO


CREATE PROCEDURE spu_Risk_RI_Value
    @insurance_file_cnt int
AS

    /********************************************************************************************************
    -- Desc : (RISK LEVEL REINSURANCE)
    --        return number of risks and total reinsurance value for these risks
    -- Hist : 19/07/2001 TN - Created
    **********************************************************************************************************/
    DECLARE @band_sum_insured money,
        	@sum_insured money
    
    SELECT  @band_sum_insured = SUM(ISNULL(ra.sum_insured, 0))
    FROM    ri_arrangement ra
    JOIN    insurance_file_risk_link ifrl
            ON ifrl.risk_cnt = ra.risk_cnt
    WHERE   ifrl.insurance_file_cnt = @insurance_file_cnt

    
    SELECT  @sum_insured = SUM(ISNULL(ral.sum_insured, 0))
    FROM    ri_arrangement_line ral
    JOIN    ri_arrangement ra
            ON ra.ri_arrangement_id = ral.ri_arrangement_id
    JOIN    insurance_file_risk_link ifrl
            ON ifrl.risk_cnt = ra.risk_cnt
    WHERE   ifrl.insurance_file_cnt = @insurance_file_cnt

    
    --Tracy Richards 11/09/03 Changed this to accept tiny differences 
    --Done because new reinsurance routines have not yet ironed this out. 
    --This is for SR17 patch 6. SR19 will not need it.
    IF ABS(@band_sum_insured - @sum_insured) < 0.01
            SELECT 1
    ELSE
            SELECT 0


GO


