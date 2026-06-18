SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_select_policies_for_renewal'
GO

CREATE PROCEDURE spu_select_policies_for_renewal
AS
BEGIN
    SELECT DISTINCT insurance_file_cnt ,insurance_folder_cnt, renewal_date, insurance_file_type_id, insurance_file_status_id
     FROM Insurance_File        I,
        Party           P,
        Risk_Code       R,
        Risk_Renewal_Days   D,
        Service_Level       S

    WHERE  I.insured_cnt = p.party_cnt
    AND I.risk_code_id = R.risk_code_id
    AND R.risk_group_id = D.risk_group_id
    AND insurance_file_status_id IS NULL
    AND( p.service_level_id = d.service_level_id OR d.service_level_id = 0)
    AND I.renewal_date <=DATEADD(Day, D.renewal_days,getdate())

    UNION

    SELECT DISTINCT insurance_file_cnt ,insurance_folder_cnt, renewal_date, insurance_file_type_id, insurance_file_status_id
     FROM Insurance_File        I,
        Party           P,
        Risk_Code       R,
        Risk_Renewal_Days   D,
        Service_Level       S

    WHERE  I.insured_cnt = p.party_cnt
    AND I.risk_code_id = R.risk_code_id
    AND insurance_file_status_id IS NULL
    AND R.risk_group_id not in
    (SELECT risk_group_id
        FROM risk_renewal_days
        WHERE risk_group_id = R.Risk_Group_id
        AND( service_level_id = p.service_level_id OR service_level_id = 0))
    AND I.renewal_date <=DATEADD(Day,30,getdate())

ORDER BY insurance_file_cnt

END
GO

