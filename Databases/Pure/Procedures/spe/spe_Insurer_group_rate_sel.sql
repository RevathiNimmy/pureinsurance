SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Insurer_group_rate_sel'
GO

CREATE PROCEDURE spe_Insurer_group_rate_sel
    @party_cnt int,
    @Scheme int,
    @risk_group_id int,
    @effective_date datetime
AS
SELECT
    party_cnt,
    Scheme,
    risk_group_id,
    effective_date,
    rate1,
    value1,
    minimum_total1,
    rate2,
    value2,
    minimum_total2,
    rate3,
    value3,
    minimum_total3
 FROM Insurer_group_rate
WHERE party_cnt = @party_cnt AND Scheme = @Scheme AND risk_group_id = @risk_group_id AND effective_date = @effective_date

GO

