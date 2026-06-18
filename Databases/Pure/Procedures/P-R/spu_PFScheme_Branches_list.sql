EXECUTE DDLDropProcedure 'spu_PFScheme_Branches_list'
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS OFF
GO

CREATE PROCEDURE spu_PFScheme_Branches_list
AS BEGIN

    SELECT Source_id, description
    FROM source
    ORDER BY description

END
GO

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO