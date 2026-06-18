SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_chk_Tab_Display_Order_exists'
GO

CREATE PROCEDURE spu_chk_Tab_Display_Order_exists
	@DisplayOrder TINYINT
AS

SELECT Display_Order 
FROM Claim_Tab 
WHERE Display_Order = @DisplayOrder




GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

