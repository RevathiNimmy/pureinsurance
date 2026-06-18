EXECUTE DDLDropProcedure 'spu_ACT_Get_InsuranceFileFromPF'
GO

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_ACT_Get_InsuranceFileFromPF
(  
    @PFPremFinanceCnt INT,  
    @PFPremFinanceVersion INT,  
    @InsuranceFileCnt INT OUTPUT,  
    @InsuranceRef VARCHAR(50) OUTPUT,  
    @IsRenewed TINYINT = NULL OUTPUT,
    @CoverStartDate DATETIME = NULL OUTPUT  

)  
AS  

   
SELECT  
    @InsuranceFileCnt=p.insurance_file_cnt,  
    @InsuranceRef=i.insurance_ref,
    @CoverStartDate=i.cover_start_date
		
FROM  
    PFPremiumFinance p  
INNER JOIN  
    Insurance_File i ON i.insurance_file_cnt=p.insurance_file_cnt  
WHERE  
    p.pfprem_finance_cnt=@PFPremFinanceCnt  
AND p.pfprem_finance_version=@PFPremFinanceVersion  

IF EXISTS(
		SELECT * FROM insurance_file if1
		INNER JOIN insurance_file if2
		ON if1.insurance_folder_cnt = if2.insurance_folder_cnt 
		AND if1.policy_version <= if2.policy_version
		WHERE if2.insurance_file_cnt = @InsuranceFileCnt
		AND if1.insurance_file_type_id=2
		AND if1.insurance_file_status_id IS NULL
		AND if1.policy_version >1)
	SELECT @IsRenewed = 1
ELSE
	SELECT @IsRenewed = 0

