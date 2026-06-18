SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_TransMatch'
GO


CREATE PROCEDURE spu_ACT_Select_TransMatch
    @transmatch_id int
AS


SELECT
    transmatch_id,
    allocationdetail_id,
    transdetail_id,
    match_id,
    currency_id,
    base_match_amount,
    currency_match_amount,
    currency_match_xrate
FROM TransMatch
WHERE transmatch_id = @transmatch_id
GO


