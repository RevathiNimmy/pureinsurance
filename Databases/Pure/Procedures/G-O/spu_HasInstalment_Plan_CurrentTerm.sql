
EXECUTE DDLDropProcedure 'spu_HasInstalment_Plan_CurrentTerm'

GO

CREATE PROCEDURE spu_HasInstalment_Plan_CurrentTerm
	@insurance_file_cnt INT
AS
DECLARE @insurance_folder_cnt INT  
DECLARE @init_payment_method VARCHAR(60)  
DECLARE @init_insurance_file_cnt INT  
DECLARE @latest_instalment_policy_version INT  
DECLARE @Renewal_version INT  
DECLARE @Inception_DateTPI DATE
 
SELECT  
 @insurance_folder_cnt = insurance_folder_cnt  
FROM  
 insurance_file  
WHERE  
 insurance_file_cnt = @insurance_file_cnt  
  
IF EXISTS(SELECT *  
   FROM insurance_file inf  
   INNER JOIN insurance_file_status ifs ON inf.insurance_file_status_id = ifs.insurance_file_status_id  
   WHERE ifs.code = 'REP' AND inf.insurance_folder_cnt = @insurance_folder_cnt)  
BEGIN  
 --The policy has been renewed, look for latest renewal  
 SELECT  
  @Renewal_version = MAX(policy_version)  
 FROM  
  insurance_file inf  
 LEFT JOIN  
  insurance_file_status ifs ON inf.insurance_file_status_id = ifs.insurance_file_status_id  
 WHERE  
  inf.insurance_file_type_id = 2 AND inf.insurance_folder_cnt = @insurance_folder_cnt  
END  
ELSE  
BEGIN  
 IF EXISTS(SELECT 1 FROM PFPremiumFinance PF INNER JOIN Insurance_File INF ON PF.Insurance_File_Cnt = INF.insurance_file_cnt WHERE INF.insurance_folder_cnt IN (SELECT insurance_folder_cnt FROM Insurance_File WHERE Insurance_File_Cnt = @insurance_file_cnt))  
 BEGIN  
 SELECT @Renewal_version = inf.policy_version FROM PFPremiumFinance PF INNER JOIN Insurance_File INF ON PF.Insurance_File_Cnt = INF.insurance_file_cnt  
 INNER JOIN Insurance_Folder FO ON INF.insurance_folder_cnt = FO.insurance_folder_cnt  
 WHERE FO.insurance_folder_cnt = (SELECT insurance_folder_cnt FROM Insurance_File WHERE Insurance_File_Cnt = @insurance_file_cnt) AND PF.StatusInd IN ('040','900')  ORDER BY pf.pfprem_finance_cnt, pfprem_finance_version DESC  
 END  
 ELSE  
 BEGIN  
 --The policy has not been renewed. Version 1 is the correct version  
 SET @Renewal_version = 1  
 END  
  
END  
  
--Get payment method from policy version  
SELECT  
 @init_payment_method = payment_method,  
 @init_insurance_file_cnt = insurance_file_cnt,
 @Inception_DateTPI = CONVERT(DATE,Inception_Date_TPI)
FROM  
 insurance_file  
WHERE  
 insurance_folder_cnt = @insurance_folder_cnt  
AND  
 policy_version = @Renewal_version  And insurance_file_type_id In (2,5,6,9)
  
--Get version of policy with the latest version of the instalments  
	SELECT	@latest_instalment_policy_version = MAX(pfp1.insurance_file_cnt)
	FROM	PFPremiumFinance pfp1
		INNER JOIN	PFPremiumFinance pfp2 
			ON pfp1.pfprem_finance_cnt = pfp2.pfprem_finance_cnt 
		INNER JOIN INSURANCE_FILE IFI
			ON IFI.Insurance_file_Cnt = pfp1.insurance_file_cnt	
		WHERE	IFI.insurance_folder_cnt = @insurance_folder_cnt 
		AND CONVERT(DATE,IFI.Inception_Date_TPI) = @Inception_DateTPI
		AND pfp1.STATUSIND  IN ('040','140','900','990','999')
 
IF (@latest_instalment_policy_version IS NULL OR  EXISTS(SELECT TOP 1 insurance_file_cnt FROM PFPremiumFinance WHERE STATUSIND = '999' AND insurance_file_cnt = @latest_instalment_policy_version ) ) AND UPPER(@init_payment_method) <> 'PAYNOW'
 SET @init_payment_method ='INVOICE'  
SELECT @init_payment_method Initial_Payment_Method,  
  @latest_instalment_policy_version Latest_Instalment_Policy_Version 

GO
