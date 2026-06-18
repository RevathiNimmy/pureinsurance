SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Insurer_rate_upd'
GO

CREATE PROCEDURE spe_Insurer_rate_upd
    @party_cnt int,
    @Scheme int,
    @risk_code_id int,
    @effective_date datetime,
    @rate1 numeric(7,4),
    @value1 numeric(19,4),
    @minimum_total1 numeric(19,4),
    @rate2 numeric(7,4),
    @value2 numeric(19,4),
    @minimum_total2 numeric(19,4),
    @rate3 numeric(7,4),
    @value3 numeric(19,4),
    @minimum_total3 numeric(19,4),
    @is_gemini_transferred tinyint
AS
BEGIN
UPDATE Insurer_rate
    SET
    rate1=@rate1,
    value1=@value1,
    minimum_total1=@minimum_total1,
    rate2=@rate2,
    value2=@value2,
    minimum_total2=@minimum_total2,
    rate3=@rate3,
    value3=@value3,
    minimum_total3=@minimum_total3,
    is_gemini_transferred=@is_gemini_transferred
WHERE party_cnt = @party_cnt
AND Scheme = @Scheme
AND risk_code_id = @risk_code_id
AND effective_date = @effective_date
END

GO

