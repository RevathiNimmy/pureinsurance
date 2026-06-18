SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Insurer_rate_del'
GO

CREATE PROCEDURE spe_Insurer_rate_del
    @party_cnt int,
    @Scheme int,
    @risk_code_id int,
    @effective_date datetime
AS
DELETE FROM Insurer_rate
WHERE party_cnt = @party_cnt AND Scheme = @Scheme AND risk_code_id = @risk_code_id AND effective_date = @effective_date

GO

