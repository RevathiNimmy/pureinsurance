SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Insurer_rate_saa'
GO

CREATE PROCEDURE spe_Insurer_rate_saa
    @party_cnt int,
    @risk_code_id int
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
WHERE party_cnt = @party_cnt AND risk_code_id = @risk_code_id
ORDER BY Scheme ASC, effective_date ASC

GO

