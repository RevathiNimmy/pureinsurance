SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_Agent_Single_Instalment_Plan'
GO

CREATE PROCEDURE spu_Get_Agent_Single_Instalment_Plan  
    @insurance_file_cnt INT,  
    @StatusInd  varchar(3)  
AS  
  
DECLARE @PartyCnt INT  
DECLARE @InsuranceFileCnt INT  
DECLARE @IsSingleInstalmentPlan Tinyint  
DECLARE @product_id Int  
DECLARE @CurrInsFileCnt Int  
  
Select @CurrInsFileCnt = (Select top 1 Insurance_file_cnt from insurance_file ifile  
        Where Insurance_folder_cnt = ( Select insurance_folder_cnt from  
              Insurance_file Where Insurance_file_cnt =@insurance_file_cnt)  
        Order By ifile.Insurance_file_cnt desc)  
  
SET @product_id=(SELECT ifl.product_id  
FROM insurance_file ifl  
WHERE ifl.insurance_file_cnt=@insurance_file_cnt)  
  
SET @PartyCnt=(SELECT pa.party_cnt  
FROM party_agent pa  
LEFT JOIN party p ON p.party_cnt=pa.party_cnt  
LEFT JOIN insurance_file ifl ON ifl.lead_agent_cnt=p.party_cnt  
WHERE ifl.insurance_file_cnt=@CurrInsFileCnt  
AND pa.is_single_instalment_plan=1)  
  
IF  @StatusInd<>'990' 
BEGIN  
	IF @PartyCnt IS NOT NULL 
	BEGIN
	SET @InsuranceFileCnt=(SELECT TOP 1 ifl.insurance_file_cnt  
	FROM insurance_file ifl  
	INNER JOIN PFPremiumFinance pmf ON pmf.insurance_file_cnt=ifl.insurance_file_cnt  
	WHERE ifl.lead_agent_cnt=@PartyCnt   
	AND ifl.product_id=@product_id   
	ORDER BY pmf.pfprem_finance_cnt,pfprem_finance_version  DESC)  
	END  

IF @StatusInd='999'  OR  @StatusInd='900'  
BEGIN  
 SET @IsSingleInstalmentPlan=(SELECT TOP 1 pa.is_single_instalment_plan  
 FROM party_agent pa  
 LEFT JOIN party p ON p.party_cnt=pa.party_cnt  
 LEFT JOIN insurance_file ifl ON ifl.lead_agent_cnt=p.party_cnt  
 INNER JOIN PFPremiumFinance pmf ON pmf.insurance_file_cnt=ifl.insurance_file_cnt  
 WHERE ifl.insurance_file_cnt=@InsuranceFileCnt  
 AND ifl.product_id=@product_id  
 AND pa.is_single_instalment_plan=1)  
END  
ELSE  
BEGIN  
 SET @IsSingleInstalmentPlan=(SELECT TOP 1 pa.is_single_instalment_plan  
 FROM party_agent pa  
 LEFT JOIN party p ON p.party_cnt=pa.party_cnt  
 LEFT JOIN insurance_file ifl ON ifl.lead_agent_cnt=p.party_cnt  
 INNER JOIN PFPremiumFinance pmf ON pmf.insurance_file_cnt=ifl.insurance_file_cnt  
 WHERE ifl.insurance_file_cnt=@InsuranceFileCnt  
 AND ifl.product_id=@product_id  
 AND pa.is_single_instalment_plan=1 AND StatusInd not in ('999','900'))  
END  
END  
ELSE  
BEGIN  
   SET @IsSingleInstalmentPlan=(SELECT TOP 1 pa.is_single_instalment_plan  
 FROM party_agent pa  
 LEFT JOIN party p ON p.party_cnt=pa.party_cnt  
 LEFT JOIN insurance_file ifl ON ifl.lead_agent_cnt=p.party_cnt  
 INNER JOIN PFPremiumFinance pmf ON pmf.insurance_file_cnt=ifl.insurance_file_cnt  
 WHERE ifl.insurance_file_cnt=@insurance_file_cnt  
 AND ifl.product_id=@product_id  
 AND pa.is_single_instalment_plan=1 AND StatusInd not in ('999','900'))  
END  
IF @IsSingleInstalmentPlan=1  
BEGIN  
select @IsSingleInstalmentPlan  
SELECT @InsuranceFileCnt  
END 
Go 
