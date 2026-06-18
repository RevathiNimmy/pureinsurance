SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SIR_Insurance_File_Del'
GO

-- Initially written to remove what-if quotes that have not been saved. Only records from the necessary related
-- tables have been removed before the actual policy version. If this procedure is to be used elsewhere then it 
-- will have to be adapted to include deletes from any other required tables. 

CREATE PROCEDURE spu_SIR_Insurance_File_Del
    @insurance_file_cnt INT
AS

DECLARE  @risk_cnt INT

DELETE insurance_file_risk_link
WHERE  insurance_file_cnt = @insurance_file_cnt

DELETE insurance_file_persistent_risk_link
WHERE  insurance_file_cnt = @insurance_file_cnt

-- Assumes one risk per quote
SELECT @risk_cnt = risk_id 
FROM   gis_policy_link
WHERE  insurance_file_cnt = @insurance_file_cnt

IF @risk_cnt > 0 
BEGIN
   DELETE risk
   WHERE  risk_cnt = @risk_cnt
END

DELETE gis_policy_link
WHERE  insurance_file_cnt = @insurance_file_cnt

DELETE event_log
WHERE  insurance_file_cnt = @insurance_file_cnt

DELETE policy_agents
WHERE  insurance_file_cnt = @insurance_file_cnt

DELETE insurance_file_system
WHERE  insurance_file_cnt = @insurance_file_cnt

DELETE insurance_file
WHERE  insurance_file_cnt = @insurance_file_cnt

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO
