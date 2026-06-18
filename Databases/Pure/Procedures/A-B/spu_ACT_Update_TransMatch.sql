SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Update_TransMatch'
GO


CREATE PROCEDURE spu_ACT_Update_TransMatch
    @transmatch_id int,
    @allocationdetail_id int,
    @transdetail_id int,
    @match_id int,
    @currency_id smallint,
    @base_match_amount numeric(19,4),
    @currency_match_amount numeric(19,4),
    @currency_match_xrate numeric(12,8)
AS


BEGIN
UPDATE TransMatch
    SET
    allocationdetail_id=@allocationdetail_id,
    transdetail_id=@transdetail_id,
    match_id=@match_id,
    currency_id=@currency_id,
    base_match_amount=@base_match_amount,
    currency_match_amount=@currency_match_amount,
    currency_match_xrate=@currency_match_xrate
WHERE transmatch_id = @transmatch_id
END
GO


