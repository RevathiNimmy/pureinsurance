SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_lookup_peril_sel'
GO


CREATE PROCEDURE spu_lookup_peril_sel
    @insurance_file_cnt int,
    @is_sum_insured int
AS


SELECT  ifr.insurance_file_cnt,
    p.sum_insured,
    p.ri_band,
    p.risk_cnt,
    p.peril_id

FROM    insurance_file_risk_link ifr,
    peril p

WHERE   ifr.insurance_file_cnt = @insurance_file_cnt
--AND ifr.status_flag <> 'D'
AND ifr.risk_cnt = p.risk_cnt
AND p.is_sum_insured = @is_sum_insured
GO


