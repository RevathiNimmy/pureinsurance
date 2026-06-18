EXECUTE DDLDropProcedure 'spu_get_ProrataRate_for_UneditedRisk'
GO
SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE spu_get_ProrataRate_for_UneditedRisk
 @risk_cnt INT
AS
BEGIN

select pro_rata_rate from risk where risk_cnt = @risk_cnt 

END
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

