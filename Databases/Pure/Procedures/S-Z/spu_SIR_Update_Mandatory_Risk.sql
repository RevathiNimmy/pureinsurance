SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SIR_Update_Mandatory_Risk'
GO

CREATE PROCEDURE spu_SIR_Update_Mandatory_Risk  
  
 @riskId INT

 AS  
    BEGIN  
  
		UPDATE Risk 
		SET  Is_Mandatory_Risk = 1			
		WHERE risk_cnt = @riskId  
	  
    END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO