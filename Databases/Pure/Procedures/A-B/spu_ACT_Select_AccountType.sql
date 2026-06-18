SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_AccountType'
GO


CREATE PROCEDURE spu_ACT_Select_AccountType
    @accounttype_id smallint
AS


SELECT
    accounttype_id,
    caption_id,
    is_deleted,
    effective_date,
    description,
    code,
    fundamental_type
FROM AccountType
WHERE accounttype_id = @accounttype_id
GO


