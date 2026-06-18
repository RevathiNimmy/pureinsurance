--CREATED - 19th February 2004 Tracy Richards
--Gets an IPT rate for a given Extras Account Code

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Extra_IPT_Rate_sel'
GO


CREATE PROCEDURE spu_Extra_IPT_Rate_sel
    @Extras_AccountName varchar(20),
    @IPT_Rate numeric(19, 4) OUTPUT
AS

BEGIN

DECLARE @Party_cnt integer

--Get the party_cnt for this ExtrasAccount (AccountName = party.shortname)
SELECT @Party_cnt = Party_cnt FROM Party WHERE shortname LIKE '' + @Extras_AccountName + ''

--Get the rate from IPT_Extra for this Party_cnt valid for this date
SELECT @IPT_Rate = rate 
FROM IPT_Extras 
WHERE Party_cnt = @Party_cnt 
    AND effective_date <= GetDate()

END