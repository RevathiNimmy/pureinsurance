SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_get_risk_details'
GO


CREATE PROCEDURE spu_get_risk_details
    @pol_id int,
    @clm_dt datetime
AS


--DC260101 use insurance_file instead of event_insurance_file
--SELECT Risk_code.risk_code_id, Risk_code.description
--FROM risk_code, Event_Insurance_File, event_log
--where Event_Insurance_File.insurance_file_cnt = event_log.event_cnt
-- AND Event_Insurance_File.risk_code_id = Risk_code.risk_code_id
-- AND event_log.event_cnt = @pol_id

SELECT Risk_code.risk_code_id, Risk_code.description
FROM risk_code, Insurance_File
where Insurance_File.insurance_file_cnt = @pol_id
AND Insurance_File.risk_code_id = Risk_code.risk_code_id

--AND (CONVERT(char(12),event_log.event_date,103) = CONVERT(char(12),@clm_dt, 103))
GO


