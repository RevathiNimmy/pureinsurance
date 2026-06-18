EXECUTE DDLDropProcedure 'Spu_SAM_GetCurrencyDifference'
GO
CREATE PROCEDURE Spu_SAM_GetCurrencyDifference
@TransDetailId INTEGER,
@AccountShortCode VARCHAR(30),
@OldCurrencyRate  NUMERIC(19, 10)
AS
    SELECT amount - ( currency_amount * @OldCurrencyRate ) [currency_diff]
    FROM   TransDetail TD WITH(NOLOCK)
           INNER JOIN Account A WITH(NOLOCK)
                   ON TD.account_id = A.account_id
    WHERE  transdetail_id = @TransDetailId
           AND A.short_code = @AccountShortCode 
