SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIRRen_auto_ren_inv_sel'
GO


CREATE PROCEDURE spu_SIRRen_auto_ren_inv_sel
    @effective_date datetime,
    @source_id      integer
AS


BEGIN
    DECLARE @daynum int

    SELECT rc.insurance_folder_cnt,
              rc.gis_scheme_id,
              rc.renewal_gis_scheme_id,
              rc.renewal_insurance_file_cnt,
              rc.product_id,
              rc.renewal_date,
              rc.party_cnt,
              rc.risk_code_id,
              rc.gis_data_model_id,
              ISNULL(rc.renewal_edi_audit_id, 0),
              g.code,
              isnull(rc.offer_alt, 0),
              s.scheme_type_flags
    FROM renewal_control rc
    INNER JOIN renewal_settings rsr
    ON rsr.product_id = rc.product_id
    INNER JOIN gis_data_model g
    ON g.gis_data_model_id = rc.gis_data_model_id
    INNER JOIN gis_scheme s
    ON s.gis_scheme_id = rc.renewal_gis_scheme_id
    INNER JOIN insurance_folder i
    ON rc.insurance_folder_cnt = i.insurance_folder_cnt
    WHERE rc.renewal_status_type_id = (SELECT renewal_status_type_id FROM renewal_status_type WHERE code = 'INVITED')
    AND @effective_date >= DATEADD(d,
          CASE
            WHEN (SELECT ISNULL(confirm_day_num, 0) FROM GIS_Scheme WHERE gis_scheme_id = rc.gis_scheme_id) <> 0 THEN
                    (SELECT confirm_day_num FROM GIS_Scheme WHERE gis_scheme_id = rc.gis_scheme_id)
            WHEN (ISNULL(rsr.confirm_day_num, 0)) <> 0 THEN
                    rsr.confirm_day_num
            ELSE
                    (SELECT ISNULL(confirm_day_num, 0) FROM Renewal_Settings WHERE product_id = -1)
          END
, rc.renewal_date)
    AND ISNULL(rc.suspension_level, 0) = 0
    AND 
       (@source_id = 0
        OR
          (@source_id <> 0 AND ISNULL(i.source_id, 0) = @source_id)
       )
END
GO


