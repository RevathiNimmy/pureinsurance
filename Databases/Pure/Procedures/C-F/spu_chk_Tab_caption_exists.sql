SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_chk_Tab_caption_exists'
GO

CREATE PROCEDURE spu_chk_Tab_caption_exists
	@Caption varchar(50)
 AS

SELECT Caption 
FROM Claim_Tab
WHERE Caption=@Caption







GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

