SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Agent_group_rate_del'
GO

CREATE PROCEDURE spe_Agent_group_rate_del
    @party_cnt int,
    @risk_group_id int,
    @effective_date datetime
AS
DELETE FROM Agent_group_rate
WHERE party_cnt = @party_cnt AND risk_group_id = @risk_group_id AND effective_date = @effective_date

GO

