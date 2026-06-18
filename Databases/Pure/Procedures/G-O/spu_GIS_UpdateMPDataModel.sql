DDLDropProcedure 'spu_GIS_UpdateMPDataModel'
GO

CREATE PROCEDURE spu_GIS_UpdateMPDataModel @sDataModelCode         VARCHAR(10),
                                           @bIsMPDataModel           TINYINT,
                                           @bIsImportedMPDataModel TINYINT = NULL
AS
    IF ISNULL(@bIsImportedMPDataModel, 0) = 0
      UPDATE gis_data_model
      SET    is_marketplace_data_model = @bIsMPDataModel
      WHERE  Code = @sDataModelCode
    ELSE
      UPDATE gis_data_model
      SET    is_marketplace_data_model = @bIsMPDataModel,
             is_imported_marketplace_data_model = @bIsImportedMPDataModel
      WHERE  Code = @sDataModelCode

GO 
