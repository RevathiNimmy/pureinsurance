SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_wp_PolicyAddOnCount'
GO


CREATE PROCEDURE spu_wp_PolicyAddOnCount
    @PartyCnt INT,  
    @InsuranceFileCnt INT,  
    @RiskId INT,  
    @ClaimCnt INT,  
    @DocumentRef VARCHAR(25),  
    @Instance1 INT,  
    @Instance2 INT,  
    @Instance3 INT  
AS  
  
SELECT  COUNT(1)
FROM policy_fee pf 
WHERE insurance_file_cnt = @InsuranceFileCnt

GO
