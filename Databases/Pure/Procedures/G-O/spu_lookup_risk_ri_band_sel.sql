SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_lookup_risk_ri_band_sel'
GO


CREATE PROCEDURE spu_lookup_risk_ri_band_sel
    @insurance_file_cnt int,
    @ri_band int,
    @risk_id int
AS


SELECT  risk_cnt,
    ri_band,
    @insurance_file_cnt,
    premium,
    sum_insured

FROM    risk_ri_band rrb

WHERE risk_cnt = @risk_id
AND   ri_band = @ri_band
--AND   insurance_file_cnt = @insurance_file_cnt
GO


