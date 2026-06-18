SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_claim_status'
GO

CREATE PROCEDURE spu_get_claim_status  
    @claim_id int  
AS  
  
BEGIN
	SELECT  c.claim_status_id,  
	        ps.code  
	FROM        claim c,  
	        progress_status ps  
	WHERE   c.claim_id = @claim_id  
	AND     ps.progress_status_id = c.progress_status_id  
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
