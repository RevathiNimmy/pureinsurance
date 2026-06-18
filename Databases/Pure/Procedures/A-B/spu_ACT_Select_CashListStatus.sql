SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_CashListStatus'
GO


CREATE PROCEDURE spu_ACT_Select_CashListStatus
    @cashliststatus_id int
AS


SELECT
    cashliststatus_id,
    caption_id,
    is_deleted,
    effective_date,
    description,
    code
FROM CashListStatus
WHERE cashliststatus_id = @cashliststatus_id
GO


