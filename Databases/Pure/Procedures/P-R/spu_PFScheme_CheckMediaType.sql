SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_PFScheme_CheckMediaType'
GO

CREATE PROCEDURE spu_PFScheme_CheckMediaType
@media_type_id int
AS 
BEGIN
        SELECT mt.mediatype_id
        FROM MediaType mt
         INNER JOIN MediaType_Validation mtv
            ON mt.mediatype_validation_id = mtv.mediatype_validation_id
         WHERE mt.mediatype_id = @media_type_id
END

GO