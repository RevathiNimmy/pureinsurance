EXECUTE DDLDropProcedure 'spu_PFScheme_MediaType_List'
GO

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE spu_PFScheme_MediaType_List

AS BEGIN

    SELECT MediaType_id, code, is_via_third_party
    FROM MediaType
    ORDER BY code
END
GO


SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS OFF 
GO