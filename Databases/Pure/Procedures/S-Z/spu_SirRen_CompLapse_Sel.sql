SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIRREN_CompLapse_Sel'
GO


CREATE PROCEDURE spu_SIRREN_CompLapse_Sel
    @effective_date datetime
AS

DECLARE @renewal_status_type_id integer

--Get the ID for 'Lapse Confirmed' renewal status
SELECT @renewal_status_type_id = renewal_status_type_id
FROM Renewal_Status_Type
WHERE code = 'LAPSECONF'

--Update affected renewal control records
UPDATE Renewal_Control
SET renewal_status_type_id = @renewal_status_type_id
FROM Renewal_Control rc
INNER JOIN GIS_Scheme gs ON gs.gis_scheme_id = rc.gis_scheme_id
INNER JOIN Insurance_File ifl ON rc.old_insurance_file_cnt = ifl.insurance_file_cnt
INNER JOIN Risk_Code rcd ON rcd.risk_code_id = rc.risk_code_id
INNER JOIN Renewal_Status_Type rst ON rst.renewal_status_type_id = rc.renewal_status_type_id
    AND rst.code NOT IN ('LAPSED', 'RENEWED', 'RENEWCONF')
INNER JOIN Renewal_Settings rsr ON rsr.product_id = rcd.risk_group_id
INNER JOIN GIS_Data_Model gdm ON gdm.gis_data_model_id = rc.gis_data_model_id
LEFT JOIN Renewal_EDI_Audit rea on rea.renewal_edi_audit_id = rc.renewal_edi_audit_id
WHERE @effective_date >= 
    DateAdd(d,
        CASE
            WHEN (SELECT IsNumeric(lapse_day_num) FROM GIS_Scheme WHERE gis_scheme_id = rc.gis_scheme_id) = 1 THEN
                (SELECT lapse_day_num FROM GIS_Scheme WHERE gis_scheme_id = rc.gis_scheme_id)
            WHEN (IsNumeric(rsr.lapse_day_num)) = 1 THEN
                rsr.lapse_day_num
            ELSE
                (SELECT IsNull(lapse_day_num, 0) FROM Renewal_Settings WHERE product_id = -1)
            END
    , rc.renewal_date)
AND (
        (
            --For SQ cases ensure EDI msg exists, scheme is insurer-led and policy not paid for by DD
            gdm.code IN ('GIIMotor','GIIHouse','GIITruck')
            AND rea.renewal_edi_status = 0
            AND gs.is_insurer_lead = 1
            AND Upper(LTrim(RTrim(ifl.payment_method))) <> 'DIRECT DEBIT'
        )
        OR (gdm.code NOT IN ('GIIMotor','GIIHouse','GIITruck'))
    )

GO



