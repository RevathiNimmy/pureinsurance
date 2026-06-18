SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Event_Policy_Narrative_sel'
GO

CREATE PROCEDURE spe_Event_Policy_Narrative_sel
    @insurance_file_cnt int
AS
SELECT
    insurance_file_cnt,
    policy_narrative_id,
    Narrative_code_id
FROM Event_Policy_Narrative
WHERE insurance_file_cnt = @insurance_file_cnt

GO

