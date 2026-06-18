SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_party_prospect_add'
GO

CREATE PROCEDURE spe_party_prospect_add
    @party_cnt int,
    @agent_reference varchar(20),
    @current_intermediary int,
    @prospect_status_id int,
    @Strength_code_id int,
    @previous_insurer_cnt int,
    @previous_broker_cnt int
AS
BEGIN
INSERT INTO party_prospect (
    party_cnt ,
    agent_reference ,
    current_intermediary ,
    prospect_status_id ,
    Strength_code_id ,
    previous_insurer_cnt ,
    previous_broker_cnt )
VALUES (
    @party_cnt,
    @agent_reference,
    @current_intermediary,
    @prospect_status_id,
    @Strength_code_id,
    @previous_insurer_cnt,
    @previous_broker_cnt)
END

GO

