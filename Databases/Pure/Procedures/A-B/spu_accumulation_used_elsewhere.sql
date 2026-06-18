SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_accumulation_used_elsewhere'
GO


CREATE PROCEDURE spu_accumulation_used_elsewhere
    @gis_policy_link_id integer
AS


BEGIN

SELECT  gpl.gis_policy_link_id
FROM    gis_policy_link gpl,
    risk r
WHERE   gpl.risk_id = r.risk_cnt
AND r.accumulation_id IN (
    SELECT  r.accumulation_id
    FROM    gis_policy_link gpl,
        risk r
    WHERE   gpl.gis_policy_link_id = @gis_policy_link_id
    AND gpl.risk_id = r.risk_cnt
    )
AND gpl.insurance_file_cnt NOT IN (
    SELECT  gpl.insurance_file_cnt
    FROM    gis_policy_link gpl
    WHERE   gpl.gis_policy_link_id = @gis_policy_link_id
    )

END
GO


