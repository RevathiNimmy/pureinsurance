-- AK14072004: Create spu_GIS_Scheme_Types_Get.sql
-- Retrieves list of scheme types
-- JRD 10/10/2005 PN24195 - Removed constraint on is_deleted

SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_GIS_Scheme_Types_Get'
GO

CREATE PROCEDURE spu_GIS_Scheme_Types_Get

AS

SELECT gis_data_model_id,
       code,
       caption_id,
       description,
       is_deleted,
       effective_date,
       gis_data_model_type_id
  FROM GIS_Data_Model
 WHERE effective_date <= GetDate()

GO
