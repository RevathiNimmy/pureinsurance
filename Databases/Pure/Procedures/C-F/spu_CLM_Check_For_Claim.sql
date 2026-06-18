SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_CLM_Check_For_Claim'
GO

CREATE PROCEDURE spu_CLM_Check_For_Claim    
    @nInsuranceFileCNT INT
AS


DECLARE @nInsuranceFolderCnt As INT
SELECT	@nInsuranceFolderCnt = Insurance_folder_cnt 
		FROM Insurance_File(NOLOCK) WHERE insurance_file_cnt = @nInsuranceFileCNT


DECLARE @claim_year_to_check date 

SELECT   @claim_year_to_check=dateadd(year, 1 - ISNULL(claim_year_to_check,1), I.cover_start_date)   
FROM Product p join insurance_file i ON i.product_id=p.product_id 
WHERE insurance_folder_cnt=@nInsuranceFolderCnt
  
SELECT C.Claim_id FROM Claim C  
JOIN Insurance_File IFL ON IFL.insurance_file_cnt = C.policy_id  
JOIN Product P ON P.product_id = IFL.product_id  
WHERE IFL.insurance_folder_cnt = @nInsuranceFolderCnt  
AND C.Primary_Cause_id NOT IN (SELECT primary_cause_id FROM Product_Allowed_Causation pac WHERE   p.product_id = pac.product_id)  
AND C.loss_from_date >= @claim_year_to_check 