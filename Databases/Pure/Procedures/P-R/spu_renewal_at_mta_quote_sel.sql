SET QUOTED_IDENTIFIER ON SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_renewal_at_mta_quote_sel'
GO


CREATE PROCEDURE spu_renewal_at_mta_quote_sel
    @gis_policy_link_id int
AS


SELECT
    r.insurance_folder_cnt,
    t.code,
    r.renewal_edi_audit_id,
    s.is_insurer_lead,
    s.mta_at_renewal_day_num,
    r.renewal_date,
    r.party_cnt
FROM
    gis_policy_link l,
    insurance_file i,
    renewal_control r,
    renewal_status_type t,
    gis_scheme s
WHERE
    @gis_policy_link_id = l.gis_policy_link_id
AND
    l.insurance_file_cnt = i.insurance_file_cnt
AND
    r.insurance_folder_cnt = i.insurance_folder_cnt
AND
    t.renewal_status_type_id = r.renewal_status_type_id
AND
    s.gis_scheme_id = r.gis_scheme_id
GO


