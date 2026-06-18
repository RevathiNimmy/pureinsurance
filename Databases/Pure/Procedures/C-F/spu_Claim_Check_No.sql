SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Claim_Check_No'
GO

CREATE PROCEDURE spu_Claim_Check_No  
    @Claim_id int  
AS  
  
    Select count(*) from Claim(NOLOCK) where Claim_id = @Claim_id  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
