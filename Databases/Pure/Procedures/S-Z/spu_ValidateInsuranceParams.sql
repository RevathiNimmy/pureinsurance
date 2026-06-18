
SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_ValidateInsuranceParams'
GO

CREATE PROCEDURE spu_ValidateInsuranceParams

    @insurance_folder_cnt INT,
    @insurance_file_cnt INT,
    @risk_cnt INT
AS

SELECT F.source_id,F.insurance_folder_cnt, L.status_flag 
FROM Insurance_file IFL
JOIN Insurance_folder F ON F.insurance_folder_cnt = IFL.insurance_folder_cnt
JOIN Insurance_file_risk_link L ON
    L.insurance_file_cnt = IFL.insurance_file_cnt
 
WHERE IFL.insurance_file_cnt = @insurance_file_cnt AND IFL.insurance_folder_cnt = @insurance_folder_cnt
AND L.risk_cnt = @Risk_cnt

