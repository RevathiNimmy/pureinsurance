SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Agent_group_rate_add'
GO

CREATE PROCEDURE spe_Agent_group_rate_add
    @party_cnt int,
    @risk_group_id int,
    @effective_date datetime,
    @agent_rate1 numeric(7,4),
    @agent_value1 numeric(19,4),
    @minimum_total1 numeric(19,4),
    @agent_rate2 numeric(7,4),
    @agent_value2 numeric(19,4),
    @minimum_total2 numeric(19,4),
    @agent_rate3 numeric(7,4),
    @agent_value3 numeric(19,4),
    @minimum_total3 numeric(19,4)
AS
BEGIN
INSERT INTO Agent_group_rate (
    party_cnt ,
    risk_group_id ,
    effective_date ,
    agent_rate1 ,
    agent_value1 ,
    minimum_total1 ,
    agent_rate2 ,
    agent_value2 ,
    minimum_total2 ,
    agent_rate3 ,
    agent_value3 ,
    minimum_total3 )
VALUES (
    @party_cnt,
    @risk_group_id,
    @effective_date,
    @agent_rate1,
    @agent_value1,
    @minimum_total1,
    @agent_rate2,
    @agent_value2,
    @minimum_total2,
    @agent_rate3,
    @agent_value3,
    @minimum_total3)
END

GO

