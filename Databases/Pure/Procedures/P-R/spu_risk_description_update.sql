SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_risk_description_update'
GO
CREATE PROCEDURE spu_risk_description_update
	@risk_cnt INT,
	@description VARCHAR(255)
AS
BEGIN

	UPDATE Risk 
	SET description = @description 
	WHERE risk_cnt = @risk_cnt

END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

