SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Agent_group_rate_upd'
GO

CREATE PROCEDURE spe_Agent_group_rate_upd
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
UPDATE Agent_group_rate
    SET
    agent_rate1=@agent_rate1,
    agent_value1=@agent_value1,
    minimum_total1=@minimum_total1,
    agent_rate2=@agent_rate2,
    agent_value2=@agent_value2,
    minimum_total2=@minimum_total2,
    agent_rate3=@agent_rate3,
    agent_value3=@agent_value3,
    minimum_total3=@minimum_total3
WHERE party_cnt = @party_cnt AND risk_group_id = @risk_group_id AND effective_date = @effective_date
END

GO

