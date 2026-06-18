SET QUOTED_IDENTIFIER ON 
SET ANSI_NULLS ON
GO

EXECUTE DDLDROPPROCEDURE 'spu_wp_debitcashPolicycount'
GO
CREATE PROCEDURE spu_wp_debitcashPolicycount  
 @PartyCnt INT,      
    @InsuranceFileCnt INT,      
    @RiskID INT,      
    @ClaimCnt INT,      
    @DocumentRef VARCHAR(25),      
    @Instance1 INT,      
    @Instance2 INT,      
    @Instance3 INT     
AS  
  
  
   SELECT count(1) 'how_many'    
    FROM Insurance_File_Payment_Details  ifpd  
 JOIN document d on ifpd.document_id=d.document_id  
 JOIN Insurance_File i on i.Insurance_File_cnt=ifpd.Insurance_File_cnt  
    WHERE  d.Document_Ref = @DocumentRef  
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO  
  