SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_SIR_renewal_scheme_timings_sel'
GO


CREATE PROCEDURE spu_SIR_renewal_scheme_timings_sel
    @insurance_file_cnt int
AS


BEGIN
    SELECT s.gis_scheme_id,
           s.quote_day_num,
           s.selection_day_num,
           s.invite_day_num,
           s.confirm_day_num,
           s.lapse_day_num
    FROM gis_policy_schemes_sel gs,
         gis_policy_link gl,
         gis_scheme s
    WHERE gs.gis_policy_link_id = gl.gis_policy_link_id
    AND gl.insurance_file_cnt = @insurance_file_cnt
    AND s.gis_scheme_id = gs.gis_scheme_id
END
GO


