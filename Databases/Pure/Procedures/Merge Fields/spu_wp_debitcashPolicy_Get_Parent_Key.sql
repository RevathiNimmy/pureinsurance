SET QUOTED_IDENTIFIER ON 
SET ANSI_NULLS ON
GO

EXECUTE DDLDROPPROCEDURE 'spu_wp_debitcashPolicy_Get_Parent_Key'
GO
CREATE PROCEDURE spu_wp_debitcashPolicy_Get_Parent_Key    
    @PartyCnt INT,    
    @InsuranceFileCnt INT,    
    @RiskID INT,    
    @ClaimCnt INT,    
    @DocumentRef VARCHAR(25),    
    @Instance1 INT,    
    @Instance2 INT,    
    @Instance3 INT    
AS    
    
SELECT document_id   
    FROM   
 document   
    WHERE  Document_Ref = @DocumentRef
 
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO  
  