SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_PFPremiumFinance_Sel_Latest_Valid_Plan_2'
GO

CREATE PROCEDURE spu_PFPremiumFinance_Sel_Latest_Valid_Plan_2  
     @insurancefilecnt INTEGER,  
     @business_type VARCHAR(20) = NULL,
     @paymentMethod tinyint  = 0
AS  
  
DECLARE @filter1 AS VARCHAR(20)  
DECLARE @filter2 AS VARCHAR(20)  
DECLARE @filter3 AS VARCHAR(20)  
  
DECLARE @inception_date_tpi AS DATETIME  
DECLARE @insurance_folder_cnt AS INTEGER  
DECLARE @pfprem_finance_cnt INTEGER  
DECLARE @pfprem_finance_version INTEGER  
  
DECLARE @Orginsurancefilecnt INTEGER
  
SET @Orginsurancefilecnt = @insurancefilecnt
  
SELECT @insurance_folder_cnt = insurance_folder_cnt,  
  @inception_date_tpi = inception_date_tpi  
FROM Insurance_File  
WHERE  insurance_file_cnt = @insurancefilecnt  
  
IF @business_type = 'MTA' OR @business_type = 'REN'  
BEGIN  
 SET @filter1 = '040'  
 SET @filter2 = '900'  
END  
ELSE IF @business_type = 'MTC'  
BEGIN  
 SET @filter1 = '010' --Saved  
 SET @filter2 = '040' --Live  
 SET @filter3 = '140' --On Hold  
END  
ELSE  
BEGIN  
 SET @filter1 = '040'  
END  
  
SELECT TOP 1 @pfprem_finance_cnt=P.pfprem_finance_cnt,  
   @pfprem_finance_version=P.pfprem_finance_version,  
   @insurancefilecnt = P.insurance_file_cnt  
FROM PFPremiumFinance P  
 INNER JOIN insurance_file ifl ON ifl.insurance_file_cnt=P.insurance_file_cnt  
WHERE ifl.insurance_folder_cnt=@insurance_folder_cnt  
  AND ifl.inception_date_tpi=@inception_date_tpi  
  AND P.StatusInd IN (@filter1, @filter2,@filter3)  
ORDER BY ifl.insurance_file_cnt DESC, P.pfprem_finance_version DESC  
  
 IF @paymentMethod = 1 BEGIN
	 IF @pfprem_finance_cnt IS NOT NULL  
	   SELECT payment_method FROM insurance_file WHERE insurance_file_cnt = @insurancefilecnt 
	ELSE
	   SELECT payment_method FROM insurance_file WHERE insurance_file_cnt = @Orginsurancefilecnt 
 END
 ELSE begin
	 IF @pfprem_finance_cnt IS NOT NULL  
	    EXEC spu_PFPremiumFinance_Sel_SingleFromInsuranceFileCount @insurancefilecnt  
 END 
GO