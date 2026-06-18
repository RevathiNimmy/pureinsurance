SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_comments'
GO

CREATE PROCEDURE spu_get_comments  
    @perilid numeric  
AS  
  
BEGIN
	SELECT comments 
	FROM claim_peril 
	WHERE claim_peril_id=@perilid  
END




GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
