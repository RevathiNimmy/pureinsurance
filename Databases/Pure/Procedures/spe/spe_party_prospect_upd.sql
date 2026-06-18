SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_party_prospect_upd'
GO

CREATE PROCEDURE spe_party_prospect_upd
    @party_cnt int,
    @agent_reference varchar(20),
    @current_intermediary int,
    @prospect_status_id int,
    @Strength_code_id int,
    @previous_insurer_cnt int,
    @previous_broker_cnt int
AS
BEGIN
UPDATE party_prospect
    SET
    agent_reference=@agent_reference,
    current_intermediary=@current_intermediary,
    prospect_status_id=@prospect_status_id,
    Strength_code_id=@Strength_code_id,
    previous_insurer_cnt=@previous_insurer_cnt,
    previous_broker_cnt=@previous_broker_cnt
WHERE party_cnt = @party_cnt
END

GO

