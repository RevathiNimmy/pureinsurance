EXECUTE DDLDropProcedure spu_gis_property_index_link_sel
GO

SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO


CREATE PROCEDURE spu_gis_property_index_link_sel
    @effective_date DATETIME, 
    @gis_data_model_code CHAR(10)
AS

SELECT o.object_name, p.property_name, ild.effective_date, ild.percentage
FROM gis_object o
INNER JOIN gis_property p ON p.gis_object_id = o.gis_object_id
INNER JOIN index_linking il ON il.index_linking_id = p.index_linking_id 
INNER JOIN index_linking_detail ild ON ild.index_linking_id = il.index_linking_id
INNER JOIN gis_data_model d ON d.gis_data_model_id = o.gis_data_model_id
WHERE ild.effective_date <= @effective_date
AND d.code = @gis_data_model_code
ORDER BY o.object_name, p.property_name, ild.effective_date DESC

GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO
