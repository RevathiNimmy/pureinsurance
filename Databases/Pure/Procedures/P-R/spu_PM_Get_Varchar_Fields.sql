SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_PM_Get_Varchar_Fields'
GO

CREATE PROCEDURE spu_PM_Get_Varchar_Fields
    @table_name varchar(255)
AS
/**************************************************************************************/
/* History : 09/08/2002 - Created */
/**************************************************************************************/

BEGIN
    DECLARE @objid integer

    SET NOCOUNT ON

    SELECT @objid = id from sysobjects where id = object_id(@table_name)

    SELECT 
	'Column_name'			= name,
	--'Type'					= type_name(xusertype),
	'Length'				= convert(int, length)

    FROM  syscolumns where id = @objid and (type_name(xusertype)='varchar' or
                                            type_name(xusertype)='nvarchar')


    SET NOCOUNT OFF

END
GO

