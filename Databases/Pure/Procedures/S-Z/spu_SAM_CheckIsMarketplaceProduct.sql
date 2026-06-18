SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CheckIsMarketplaceProduct'
GO

CREATE PROCEDURE spu_SAM_CheckIsMarketplaceProduct @sDataModelCode VARCHAR(10)
AS
    SELECT is_marketplace_data_model
    FROM   GIS_Data_Model
    WHERE  code = @sDataModelCode

GO 
