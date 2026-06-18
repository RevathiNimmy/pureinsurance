SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_event_policy_narrative_add'
GO

CREATE PROCEDURE spe_event_policy_narrative_add
    @insurance_file_cnt int,
    @policy_narrative_id int,
    @Narrative_code_id int
AS
BEGIN
    INSERT INTO event_policy_narrative (
        insurance_file_cnt ,
        policy_narrative_id ,
        Narrative_code_id )
    VALUES (
        @insurance_file_cnt,
        @policy_narrative_id,
        @Narrative_code_id)
END
GO

