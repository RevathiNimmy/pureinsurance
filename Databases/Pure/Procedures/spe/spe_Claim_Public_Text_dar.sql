SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spe_Claim_Public_Text_dar'
GO

CREATE PROCEDURE spe_Claim_Public_Text_dar
 @claim_cnt int
AS
DELETE
FROM Claim_Public_Text
WHERE claim_cnt = @claim_cnt

GO

