SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_get_risk_clause_info'
GO

CREATE PROCEDURE spu_get_risk_clause_info  
    @risk_cnt int,
    @col_name varchar(250),
    @table_prefix varchar(250),
	@table_name varchar(250)=null
AS
DECLARE @SQLString NVARCHAR(1000)

Set @SQLString = 'SELECT DISTINCT gp.gis_object_id, gobj.parent_object_id, gp.specials_type, gobj.table_name ' + CHAR(13)
Set @SQLString = @SQLString + 'FROM ' + @table_prefix + '_policy_binder pb ' + CHAR(13)
Set @SQLString = @SQLString + 'INNER JOIN GIS_policy_link gpl ON pb.gis_policy_link_id = gpl.gis_policy_link_id ' + CHAR(13)
Set @SQLString = @SQLString + 'INNER JOIN ' + @table_prefix + '_standard_wording sw ON sw.' + @table_prefix + '_policy_binder_id = pb.' + @table_prefix + '_policy_binder_id ' + CHAR(13)
Set @SQLString = @SQLString + 'INNER JOIN gis_property gp ON sw.gis_property_id = gp.gis_property_id ' + CHAR(13)
Set @SQLString = @SQLString + 'INNER JOIN gis_object gobj ON gp.gis_object_id = gobj.gis_object_id ' + CHAR(13)
Set @SQLString = @SQLString + 'WHERE gp.specials_type = 5 and gpl.risk_id = ' + CAST(@risk_cnt AS varchar(20)) + ' and column_name like "' + @col_name + '"'

IF (ISNULL(@table_name,'')<>'')
Set @SQLString = @SQLString + ' and table_name like ''' + @table_name + ''''

EXEC sp_executesql @SQLString

GO
