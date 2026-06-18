SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_insurance_file_risk_li_sel'
GO

CREATE PROCEDURE spe_insurance_file_risk_li_sel
    @insurance_file_cnt int,
    @risk_cnt int
AS

SELECT
    IFRL.insurance_file_cnt,
    IFRL.risk_cnt,
    IFRL.status_flag,
    IFRL.original_risk_cnt,
    IFRL.is_manually_changed,
    I.original_linked_insurance_file_cnt
FROM insurance_file_risk_link IFRL
LEFT JOIN mta_insurance_file_link I ON I.insurance_file_cnt=IFRL.insurance_file_cnt
WHERE IFRL.insurance_file_cnt = @insurance_file_cnt
AND IFRL.risk_cnt = @risk_cnt

GO

