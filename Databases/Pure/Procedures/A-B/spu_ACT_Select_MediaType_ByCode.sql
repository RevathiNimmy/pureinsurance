
SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_ACT_Select_MediaType_ByCode'
GO

CREATE PROCEDURE spu_ACT_Select_MediaType_ByCode
    @Code varchar(10)
AS
BEGIN
    SELECT
    	mediatype_id
    FROM
    	MediaType
    WHERE
    	code=@Code
END

GO


