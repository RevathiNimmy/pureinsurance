SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_get_risk_clauses'
GO
CREATE PROCEDURE spu_get_risk_clauses
    @risk_cnt int,
    @StdWrdTableName varchar(250),
    @PolicyBinderTableName varchar(250),
    @PolicyBinderId varchar(250),
    @colName varchar(250),
    @parentCol varchar(250),
    @vID int
AS
DECLARE @SQLString NVARCHAR(2000)

Set @SQLString = 'SELECT Distinct sw.sequence_id, sw.document_template_id, ' + CHAR(13)  
Set @SQLString = @SQLString + 'dt.code, dt.description '  + CHAR(13)  
Set @SQLString = @SQLString + 'FROM ' + @StdWrdTableName + ' sw, ' + CHAR(13)
Set @SQLString = @SQLString + @PolicyBinderTableName + ' pb, GIS_policy_link gpl, GIS_Property gp,' + CHAR(13)  
Set @SQLString = @SQLString + 'document_template dt ' + CHAR(13)   
Set @SQLString = @SQLString + 'WHERE gpl.risk_id = ' + CAST(@risk_cnt AS varchar(20)) + CHAR(13)
Set @SQLString = @SQLString + 'AND pb.gis_policy_link_id = gpl.gis_policy_link_id ' + CHAR(13)
Set @SQLString = @SQLString + 'AND sw.' + @PolicyBinderId + ' = pb.' + @PolicyBinderId + CHAR(13)
Set @SQLString = @SQLString + 'AND sw.gis_property_id = gp.gis_property_id ' + CHAR(13)
Set @SQLString = @SQLString + 'AND sw.gis_object_id = gp.gis_object_id ' + CHAR(13)
Set @SQLString = @SQLString + 'AND gp.property_name like "' + @colName + '"' + CHAR(13)
Set @SQLString = @SQLString + 'AND sw.document_template_id = dt.document_template_id' + CHAR(13)
if (@vID is not null)
Set @SQLString = @SQLString + 'AND sw.' + @parentCol + '_Id = ' + CAST(@vID AS varchar(20)) + CHAR(13)
Set @SQLString = @SQLString + 'ORDER BY sw.sequence_id' 

EXEC sp_executesql @SQLString

GO
