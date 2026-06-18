SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Lead_Commission_del'
GO

CREATE PROCEDURE spe_Lead_Commission_del
    @insurance_file_cnt int,
    @commission_band tinyint
AS
DELETE FROM Lead_Commission
WHERE insurance_file_cnt = @insurance_file_cnt AND commission_band = @commission_band

GO

