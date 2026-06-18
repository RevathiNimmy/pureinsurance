SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Risk_Public_Text_dar'
GO

CREATE PROCEDURE spe_Risk_Public_Text_dar
 @insurance_file_cnt int , @risk_id int
AS
DELETE
FROM Risk_Public_Text
WHERE risk_cnt = @risk_id

GO

