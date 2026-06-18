SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Agent_rate_del'
GO

CREATE PROCEDURE spe_Agent_rate_del
    @party_cnt int,
    @risk_code_id int,
    @effective_date datetime
AS
DELETE FROM Agent_rate
WHERE party_cnt = @party_cnt AND risk_code_id = @risk_code_id AND effective_date = @effective_date

GO

