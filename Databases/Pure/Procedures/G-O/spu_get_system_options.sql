SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

Execute DDLDropProcedure 'spu_get_system_options'
GO

CREATE PROCEDURE spu_get_system_options
    @branchCode int,
	@option_number int
AS

SELECT value from system_options 
WHERE branch_id = @branchCode and option_number = @Option_number

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
