EXECUTE DDLDropProcedure 'spu_get_risk_screen_id'
GO

CREATE PROCEDURE spu_get_risk_screen_id 
	@risk_group_id int
AS
BEGIN

	SELECT gis_screen_id 
	FROM risk_group 
	WHERE risk_group_id = @risk_group_id

END
GO