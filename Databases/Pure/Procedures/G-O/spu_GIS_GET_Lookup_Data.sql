SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_gis_get_lookup_data'
GO


CREATE PROCEDURE spu_gis_get_lookup_data
    @nSpecialType TINYINT,
	@sSpecialTypeReference AS VARCHAR(50),
	@sValue AS VARCHAR(255),
	@nCode TINYINT
AS
BEGIN


IF ISNULL(@nSpecialType,0) = 2 BEGIN --PMLookup
	DECLARE @sSQL AS NVARCHAR(MAX)

	IF ISNULL(@nCode,0) = 0 BEGIN
		SELECT @sSQL = 'SELECT description FROM ' + @sSpecialTypeReference + ' WHERE ' + @sSpecialTypeReference + '_id  = ' + @sValue
	END
	ELSE BEGIN
		SELECT @sSQL = 'SELECT description FROM ' + @sSpecialTypeReference + ' WHERE code  = ' + '''' + @sValue + ''''
	END

	EXECUTE SP_EXECUTESQL @sSQL
	--PRINT @sSQL
END
ELSE IF ISNULL(@nSpecialType,0) = 6 BEGIN -- UDL
	SELECT description FROM gis_user_def_detail WHERE gis_user_def_header_id = CONVERT(INT,@sSpecialTypeReference) AND gis_user_def_detail_id = @sValue
END
ELSE IF ISNULL(@nSpecialType,0) = 99 BEGIN
	IF UPPER(@sSpecialTypeReference)  ='PARTY' BEGIN
		SELECT resolved_name FROM Party WHERE party_cnt = @sValue
	END 
	ELSE IF UPPER(@sSpecialTypeReference) = 'PARTY_R' BEGIN
		SELECT shortname FROM Party WHERE party_cnt = @sValue 
	END

END 

END


