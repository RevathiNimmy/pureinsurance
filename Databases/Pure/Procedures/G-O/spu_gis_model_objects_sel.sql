SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_gis_model_objects_sel
GO

CREATE PROCEDURE spu_gis_model_objects_sel
    @gis_data_model_code CHAR(10)  
AS  
  
BEGIN  
  
/********************************************************************************************************/  
/* Stored Procedure Selects all of the Ojects for the Supplied Data Model.                              */  
/********************************************************************************************************/  
  
/********************************************************************************************************/  
/* Revision             Description of Modification                                     Date        Who */  
/* --------             ---------------------------                                     ----        --- */  
/* 1.0                  Original                            24/03/1999  RFC */  
/* 1.1                  Parameter changed from data model id to data model code.        09/08/1999  RFC */  
/********************************************************************************************************/  
  
    DECLARE @gis_data_model_id INT
      
    SELECT  @gis_data_model_id = gis_data_model_id  
    FROM    gis_data_model  
    WHERE   code = @gis_data_model_code  
      
    SELECT
        go.gis_object_id ,
        go.gis_data_model_id ,
        go.object_name ,
        go.table_name ,
        go.max_instances ,
        go.is_quote_object ,
        gp.object_name parent_object_name ,
        go.polaris_object_id ,
        go.is_selectable_for_screen ,
        go.is_non_gis ,
        go.edit_flags
    FROM    
        gis_object go
        LEFT OUTER JOIN gis_object gp  
            ON go.parent_object_id = gp.gis_object_id 
    WHERE   
        go.gis_data_model_id = @gis_data_model_id
    ORDER BY 
        go.is_quote_object DESC ,
        go.parent_object_id ASC  

END 

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO