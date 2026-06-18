SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'Spu_GetTransMatchCurrencyDiff' 
GO

CREATE PROCEDURE Spu_GetTransMatchCurrencyDiff
    @TransDetailId INT,
    @AccountId INT,
    @OldTransDetailId INT
AS
BEGIN
    DECLARE @OldCurrencyRate NUMERIC(19, 10);

    SELECT @OldCurrencyRate = currency_base_xrate
    FROM TransDetail
    WHERE transdetail_id = @OldTransDetailId;

    SELECT 
        amount - (currency_amount * @OldCurrencyRate) AS currency_diff
    FROM TransDetail TD WITH (NOLOCK)
    WHERE TD.transdetail_id = @TransDetailId
      AND TD.account_id = @AccountId;
END;
GO
