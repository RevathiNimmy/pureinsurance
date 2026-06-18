SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Agent_rate_sel'
GO

CREATE PROCEDURE spe_Agent_rate_sel
    @party_cnt int,
    @risk_code_id int,
    @effective_date datetime
AS
SELECT
    party_cnt,
    risk_code_id,
    effective_date,
    agent_rate1,
    agent_value1,
    minimum_total1,
    agent_rate2,
    agent_value2,
    minimum_total2,
    agent_rate3,
    agent_value3,
    minimum_total3
 FROM Agent_rate
WHERE party_cnt = @party_cnt AND risk_code_id = @risk_code_id AND effective_date = @effective_date

GO

