SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_MediaType'
GO


CREATE PROCEDURE spu_ACT_Select_MediaType
    @mediatype_id int
AS


SELECT
    mediatype_id,
    caption_id,
    is_deleted,
    effective_date,
    description,
    code,
-- *** pkh 07/10/2002 starts - Added for Front Office Receipting module
    mediatype_validation_id,
    is_banking,
    is_stoppable,
-- *** pkh 07/10/2002 ends   - Added for Front Office Receipting module
-- TR 02/07/03 Added Is_validation_enabled
    IsNull(is_validation_enabled,0)AS is_validation_enabled
FROM MediaType
WHERE mediatype_id = @mediatype_id
GO

