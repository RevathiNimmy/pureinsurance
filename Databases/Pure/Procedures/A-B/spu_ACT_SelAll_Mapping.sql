SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_SelAll_Mapping'
GO


CREATE PROCEDURE spu_ACT_SelAll_Mapping
    @company_id smallint = NULL,
    @maptype_id smallint = NULL
AS


SELECT
    mapping_id,
    company_id,
    maptype_id,
    description
FROM Mapping
     WHERE (@company_id = company_id OR @company_id = NULL)
     AND (maptype_id = @maptype_id OR @maptype_id = NULL)
GO


