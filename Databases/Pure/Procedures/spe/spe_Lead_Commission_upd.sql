SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Lead_Commission_upd'
GO

CREATE PROCEDURE spe_Lead_Commission_upd
    @insurance_file_cnt int,
    @commission_band tinyint,
    @premium numeric(19,4),
    @percent numeric(12,8),
    @value numeric(19,4)
AS
BEGIN
UPDATE Lead_Commission
    SET
    premium=@premium,
    [percent]=@percent,
    value=@value
WHERE insurance_file_cnt = @insurance_file_cnt AND commission_band = @commission_band
END

GO

