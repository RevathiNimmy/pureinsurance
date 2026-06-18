SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

Execute DDLDropProcedure 'spu_SAM_CLM_Check_Claimkey'
GO

CREATE PROCEDURE spu_SAM_CLM_Check_Claimkey
     @Claim_cnt int,
@count int  OUTPUT
AS
SELECT @count=count(*) FROM Claim 

WHERE Claim_id = @Claim_cnt

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
