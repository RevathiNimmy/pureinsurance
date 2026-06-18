SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SAM_Get_Standard_Wording_Values'
GO
/*********************************************************************************************************/    
/* spu_SAM_Get_Standard_Wording_Values - Gets the Standard_Wording values for a given Data Model*/    
/*                                                                                                       */    
/* RDT 7/6/2007                                                                                          */    
/*********************************************************************************************************/    
CREATE PROCEDURE spu_SAM_Get_Standard_Wording_Values    
    @gis_datamodel_code varchar(30),    
    @gis_policy_binder_id int,    
    @gis_object_name varchar(70)=NULL,    
    @OIKey varchar(20)=NULL,    
    @property_name varchar(70)=NULL,    
	@table_name varchar(70)=NULL    
AS    
BEGIN    
    
DECLARE @SQL varchar(5000)    
    
SELECT @SQL = 'SELECT go.object_name, gp.property_name, dt.code ''DocumentCode'',(REPLACE(dt.DESCRIPTION, ''&'', ''&amp;'')) ''DESCRIPTION'',sw.* FROM ' + @gis_datamodel_code + '_standard_wording sw '    
SELECT @SQL = @SQL + 'INNER JOIN Document_Template dt ON dt.document_template_id = sw.document_template_id '    
SELECT @SQL = @SQL + 'INNER JOIN Gis_Object go ON go.gis_object_id = sw.gis_object_id '    
SELECT @SQL = @SQL + 'INNER JOIN GIS_Property gp ON gp.gis_property_id = sw.gis_property_id '    
SELECT @SQL = @SQL + 'INNER JOIN GIS_Data_Model gdm ON gdm.gis_data_model_id = go.gis_data_model_id '    
SELECT @SQL = @SQL + 'WHERE sw.' + @gis_datamodel_code + '_policy_binder_id = ' + convert(varchar(10),@gis_policy_binder_id) + ' '    
SELECT @SQL = @SQL + 'AND gdm.code = ''' + @gis_datamodel_code + ''' '    
if ISNULL(@gis_object_name,'')<>''    
BEGIN    
SELECT @SQL = @SQL + 'AND go.object_name = ''' + @gis_object_name + ''' '    
SELECT @SQL = @SQL + 'AND ((sw.' + @table_name + '_id=''' + @OIKey + ''' AND sw.child=1) OR (sw.' + @table_name + '_id IS NULL AND (sw.child=0 OR sw.child IS NULL))) '    
SELECT @SQL = @SQL + 'AND gp.property_name = ''' + @property_name + ''' '    
    
END    
 SELECT @SQL = @SQL + 'ORDER BY sw.gis_object_id, sw.gis_property_id, sw.sequence_id'    
    
/* This will produce a statement of the following structure :-    
    
 SELECT   go.object_name,    
   gp.property_name,    
   dt.code 'DocumentCode',    
   sw.*    
 FROM    
   QBENZ_standard_wording sw    
 INNER JOIN Document_Template dt ON dt.document_template_id = sw.document_template_id    
 INNER JOIN Gis_Object go ON go.gis_object_id = sw.gis_object_id    
 INNER JOIN GIS_Property gp ON gp.gis_property_id = sw.gis_property_id    
 INNER JOIN GIS_Data_Model gdm ON gdm.gis_data_model_id = go.gis_data_model_id    
 WHERE   sw.QBENZ_policy_binder_id = 75    
   AND gdm.code = 'QBENZ'    
 ORDER BY sw.gis_object_id,    
   sw.gis_property_id,    
   sw.sequence_id    
    
*/    
    
exec(@SQL)    
--SELECT @SQL    
    
END 