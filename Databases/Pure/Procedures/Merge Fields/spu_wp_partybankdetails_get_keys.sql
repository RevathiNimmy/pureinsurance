SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_wp_partybankdetails_get_keys'
GO

CREATE PROCEDURE spu_wp_partybankdetails_get_keys
    @PartyCnt INT,  
    @InsuranceFileCnt INT,  
    @RiskId INT,  
    @ClaimCnt INT,  
    @DocumentRef VARCHAR(25),  
    @Instance1 INT,  
    @Instance2 INT,  
    @Instance3 INT  
AS  
  
Select party_bank_id from party_bank WHERE Account_id = 
    (Select Account_id FROM  Account Where Account_key= @PartyCnt)
    
GO 




