SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_select_Insurance_FileCnt'
GO

CREATE PROCEDURE spu_select_Insurance_FileCnt
    @risk_cnt int,
    @cover_start_date date,
    @Trans_type varchar(5) = ''
AS

IF @Trans_type ='DRI'

SELECT
ifi.insurance_file_cnt,
ifi.cover_start_date,
ifi.expiry_date
FROM insurance_file_risk_link ifrl
JOIN insurance_file ifi
ON ifrl.insurance_file_cnt = ifi.insurance_file_cnt
INNER JOIN Insurance_File_Type ift
ON ift.insurance_file_type_id = ifi.insurance_file_type_id 
WHERE ifrl.risk_cnt = @risk_cnt
AND ISNULL(ifi.insurance_file_status_id, 3) in (1,2,3,4,5,6,309)
AND ift.code IN ('POLICY', 'MTA PERM','MTA TEMP', 'MTAREINS', 'MTAQREINS', 'MTAQUOTE', 'RENEWAL', 'MTACAN')
ORDER BY insurance_file_cnt DESC

ELSE

SELECT
ifi.insurance_file_cnt,
ifi.cover_start_date,
ifi.expiry_date
FROM insurance_file_risk_link ifrl
JOIN insurance_file ifi
ON ifrl.insurance_file_cnt = ifi.insurance_file_cnt
INNER JOIN Insurance_File_Type ift
ON ift.insurance_file_type_id = ifi.insurance_file_type_id 
WHERE ifrl.risk_cnt = @risk_cnt
AND ISNULL(ifi.insurance_file_status_id, 3) in (1,2,3,4,5,6,309)
AND ift.code IN ('POLICY', 'MTA PERM', 'MTAREINS', 'MTAQREINS', 'MTAQUOTE', 'RENEWAL', 'MTACAN')
ORDER BY insurance_file_cnt DESC