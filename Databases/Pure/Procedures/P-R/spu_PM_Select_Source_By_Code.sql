SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_PM_Select_Source_By_Code'
GO


CREATE PROCEDURE spu_PM_Select_Source_By_Code
@code varchar(20)
AS
	SELECT Source_id from Source where code = @code



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
