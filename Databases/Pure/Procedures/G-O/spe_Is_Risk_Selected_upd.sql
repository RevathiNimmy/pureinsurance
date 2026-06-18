EXECUTE DDLDropProcedure 'spe_Is_Risk_Selected_upd'
GO

CREATE PROCEDURE [dbo].[spe_Is_Risk_Selected_upd]
@risk_cnt INT
AS
BEGIN
UPDATE Risk
SET is_risk_selected = 0 
WHERE risk_cnt = @risk_cnt
END
GO