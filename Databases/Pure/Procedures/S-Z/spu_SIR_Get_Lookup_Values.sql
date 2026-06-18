SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Get_Lookup_Values'
GO


CREATE PROCEDURE spu_SIR_Get_Lookup_Values

@table varchar(40)

AS

BEGIN

	DECLARE @sql varchar(200)

	Set @sql =''
	Set @sql =@sql + ' Select ' + @table + '_id, code, description from ' + @table
	set @sql =@sql + ' where is_deleted = 0'
	set @sql =@sql + ' order by description'

	EXEC (@sql)


END






GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
