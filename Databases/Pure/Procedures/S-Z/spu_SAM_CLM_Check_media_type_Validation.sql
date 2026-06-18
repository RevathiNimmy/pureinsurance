SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_SAM_CLM_Check_media_type_Validation'
GO

CREATE PROCEDURE spu_SAM_CLM_Check_media_type_Validation
@Media_type_code varchar(50),    
@Media_validation_code varchar(50)= NULL OUTPUT  
    
AS    
    
SELECT  @Media_validation_code = MTV.Code    
FROM  	mediatype_validation MTV    
INNER   JOIN MediaType MT ON    
        MTV.mediatype_validation_id = MT.mediatype_validation_id    
WHERE 	MT.code = @Media_type_code    
        

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

