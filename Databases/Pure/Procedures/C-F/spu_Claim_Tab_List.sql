SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_Claim_Tab_List'
GO

CREATE PROCEDURE [spu_Claim_Tab_List] 
@RiskOrPeril int =-1

AS

IF @RiskOrPeril = -1
	SELECT *,0
	FROM Claim_Tab
	ORDER BY Display_Order
ELSE
	SELECT *,0
	FROM Claim_Tab
	WHERE Risk_Or_Peril = @RiskOrPeril
	ORDER BY Display_Order

RETURN @@ROWCOUNT






GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

