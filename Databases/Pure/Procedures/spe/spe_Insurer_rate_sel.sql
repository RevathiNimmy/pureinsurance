SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Insurer_rate_sel'
GO

CREATE PROCEDURE spe_Insurer_rate_sel
    @party_cnt int,
    @Scheme int,
    @risk_code_id int,
    @effective_date datetime
AS
SELECT
    party_cnt,
    Scheme,
    risk_code_id,
    effective_date,
    rate1,
    value1,
    minimum_total1,
    rate2,
    value2,
    minimum_total2,
    rate3,
    value3,
    minimum_total3,
    is_gemini_transferred
 FROM Insurer_rate
WHERE party_cnt = @party_cnt AND Scheme = @Scheme AND risk_code_id = @risk_code_id AND effective_date <= @effective_date
ORDER BY effective_date DESC

GO

