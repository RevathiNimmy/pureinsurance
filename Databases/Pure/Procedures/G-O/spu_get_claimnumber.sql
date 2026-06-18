SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_claimnumber'
GO

CREATE PROCEDURE spu_get_claimnumber    
    @claim_id int    
AS    
    
BEGIN
    Select Claim_Number from Claim where claim_id=@claim_id    
END


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
