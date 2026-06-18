SET QUOTED_IDENTIFIER OFF 
GO

SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_ACT_Select_MediaType_Against_ValidationId'
GO

CREATE PROCEDURE spu_ACT_Select_MediaType_Against_ValidationId
	@mediatype_validation_id int
As
BEGIN
	SELECT  mediatype_id,code,description
	FROM MediaType  
	WHERE mediatype_validation_id = @mediatype_validation_id  
END
	
GO