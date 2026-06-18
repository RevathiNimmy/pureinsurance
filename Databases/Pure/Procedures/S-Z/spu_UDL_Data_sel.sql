SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO
EXECUTE DDLDropProcedure 'spu_UDL_Data_sel'
GO

CREATE PROCEDURE spu_UDL_Data_sel
    @table varchar(30),
    @code varchar(10)
AS

DECLARE @sql varchar(255)

BEGIN
	
	IF EXISTS(SELECT NULL FROM sysobjects WHERE [name] = @table)
	BEGIN 
		Set @SQL = 'SELECT caption_id FROM ' + @table  + ' WHERE code='+ ''''+  @code + ''''  
		EXEC (@SQL)
	END

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

