SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_GIS_upd_pm_lookup'
GO

CREATE PROCEDURE spu_GIS_upd_pm_lookup
    @newcode VARCHAR(100),
    @listtype VARCHAR(100),
    @oldcode VARCHAR(100),
    @desc VARCHAR(400),
    @fields VARCHAR(2000)
AS
BEGIN

DECLARE @initial VARCHAR(500)
DECLARE @stmt NVARCHAR(3500)
DECLARE @where1 VARCHAR(200)

SELECT @initial = 'Update UDL_' +  @ListType + '  SET code =' + '''' + @newcode + ''', description =' + '''' + @desc + ''''

SELECT @where1 = ' WHERE code=' + '''' +  @OldCode + ''''  

SELECT @stmt = @initial + @fields + @where1 

EXECUTE sp_executeSQL   @stmt

END

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO