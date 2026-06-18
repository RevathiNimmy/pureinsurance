SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_Has_Currency_Changed'
GO

CREATE PROCEDURE spu_Get_Has_Currency_Changed
    @insurance_file_cnt INT,
	@has_currency_changed BIT OUTPUT
AS

DECLARE @old_currency_id SMALLINT
DECLARE @new_currency_id SMALLINT

SELECT 
	@new_currency_id = MAX(i.currency_id), 
	@old_currency_id = MAX(i2.currency_id)
FROM insurance_file i
JOIN insurance_file_risk_link ifrl
	ON ifrl.insurance_file_cnt = i.insurance_file_cnt
JOIN insurance_file_risk_link ifrl2
	ON ifrl2.risk_cnt = ifrl.risk_cnt
	AND ifrl2.status_flag = 'C'
JOIN insurance_file i2
	ON i2.insurance_file_cnt = ifrl2.insurance_file_cnt
WHERE i.insurance_file_cnt = @insurance_file_cnt
GROUP BY i.currency_id, i2.currency_id

IF @old_currency_id <> @new_currency_id 
BEGIN
	SELECT @has_currency_changed = 1
END
ELSE
BEGIN
	SELECT @has_currency_changed = 0
END

GO