SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_insurance_file_risk_li_del'
GO

CREATE PROCEDURE spe_insurance_file_risk_li_del
    @insurance_file_cnt int,
    @risk_cnt int
AS

DELETE FROM insurance_file_risk_link
    WHERE insurance_file_cnt = @insurance_file_cnt
    AND risk_cnt = @risk_cnt

GO

