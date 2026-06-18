SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Risk_del'
GO

CREATE PROCEDURE spe_Risk_del
    @risk_cnt int
AS

DELETE FROM Risk
WHERE risk_cnt = @risk_cnt

GO

