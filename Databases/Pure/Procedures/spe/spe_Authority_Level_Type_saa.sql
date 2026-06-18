SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_Authority_Level_Type_saa'
GO

CREATE PROCEDURE spe_Authority_Level_Type_saa
AS
SELECT
    authority_level_type_id,
    caption_id,
    code,
    description,
    is_deleted,
    effective_date
FROM Authority_Level_Type
ORDER BY authority_level_type_id ASC

GO

