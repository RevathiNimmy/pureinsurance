SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_data_model_from_screen
GO

CREATE PROCEDURE spu_data_model_from_screen
    @GIS_screen_id INT  
AS  
BEGIN

    DECLARE @bFound SMALLINT,  
        @GIS_data_model_id INT,  
        @code VARCHAR(20),  
        @data_model_type INT,  
        @parent INT  
      
    SELECT  @bFound = 0  
      
    WHILE   @bFound = 0  
    BEGIN  
      
        SELECT   @GIS_data_model_id = d.gis_data_model_id,  
                 @code = d.code,  
                 @parent = s.parent_id,  
                 @data_model_type = d.gis_data_model_type_id  
        FROM gis_screen s
            LEFT OUTER JOIN gis_data_model d  
                ON s.gis_data_model_id = d.gis_data_model_id  
        WHERE   s.gis_screen_id = @gis_screen_id  
     
        IF @code IS NULL
            SELECT @gis_screen_id = @parent  
        ELSE  
            SELECT  @bFound = 1  
    END  
      
    SELECT @GIS_data_model_id,  
           @code,  
           @data_model_type  

END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

