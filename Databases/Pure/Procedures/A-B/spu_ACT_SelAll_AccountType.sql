SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_SelAll_AccountType'
GO


CREATE PROCEDURE spu_ACT_SelAll_AccountType
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
ORDER BY code
GO


