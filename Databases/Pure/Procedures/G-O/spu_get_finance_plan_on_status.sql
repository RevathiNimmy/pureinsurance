EXECUTE DDLDropProcedure 'spu_get_finance_plan_on_status'
GO
-- PBI #37524: Added optional @ClaimNumber and @SourceType parameters for CLR plan filtering
CREATE PROCEDURE spu_get_finance_plan_on_status  
 @Insurance_file_cnt INT,  
@sStatus VARCHAR(50),
@nInsurance_folder_cnt As int=0,
@ClaimNumber VARCHAR(30) = NULL,
@SourceType VARCHAR(10) = NULL
AS  
SELECT PFPrem_Finance_Cnt,  
  PFPrem_Finance_Version,  
  StatusInd  
        FROM PFPremiumFinance PF
        INNER JOIN insurance_file ifl ON ifl.insurance_file_cnt=PF.insurance_file_cnt
WHERE

       (
        (ISNULL(@nInsurance_folder_cnt,0)=0 AND ifl.insurance_file_cnt=@Insurance_file_cnt AND StatusInd >= '010' and StatusInd <= '012')
        OR
        (ISNULL(@nInsurance_folder_cnt,0)<>0 AND StatusInd =@sStatus AND ifl.insurance_folder_cnt= @nInsurance_folder_cnt)
       )
       AND (@ClaimNumber IS NULL OR @ClaimNumber = '' OR PF.claim_number = @ClaimNumber)
       AND (@SourceType IS NULL OR @SourceType = '' OR ISNULL(PF.source_type, 'PF') = @SourceType)
GO