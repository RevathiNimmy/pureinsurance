SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO

Execute DDLDropProcedure 'spu_Update_Insurance_File_Details_OOS_Reinstate'
GO

CREATE PROCEDURE spu_Update_Insurance_File_Details_OOS_Reinstate  
 @nInsuranceFileCnt INT,  
 @nNewInsuranceFileCnt INT  
AS  
BEGIN
  
DECLARE @nInsuranceFolderCnt INT ,  
  @dtInceptionDateTpi Datetime ,  
  @nPaymentTerm INT,  
  @nOld_PaymentTerm INT,
  @nCollectionFrequency INT,  
  @nOld_CollectionFrequency INT,  
  @nBusinessType INT,  
  @nLead_Agent_Cnt INT,  
  @nOld_Lead_Agent_Cnt INT,
  @dtAnniversary_Date DATETIME,
  @dtOldAnniversary_Date DATETIME,
  @nOriginalInsuranceFileCnt INT  
  
  SELECT @nOriginalInsuranceFileCnt=MIN(original_linked_insurance_file_cnt) FROM mta_insurance_file_link 
	WHERE insurance_file_cnt=@nInsuranceFileCnt
  
SELECT  @nInsuranceFolderCnt = insurance_folder_cnt ,  
  @nPaymentTerm = DOPaymentTerms_id ,  
  @nCollectionFrequency = CollectionFrequency_id,  
  @nBusinessType= business_type_id,  
  @nLead_Agent_Cnt=lead_agent_cnt,  
  @dtAnniversary_Date=anniversary_date  
  FROM insurance_file WHERE insurance_file_cnt =@nInsuranceFileCnt  


SELECT   @nOld_PaymentTerm = DOPaymentTerms_id ,  
  @nOld_CollectionFrequency = CollectionFrequency_id,  
  @nOld_Lead_Agent_Cnt=lead_agent_cnt , @dtOldAnniversary_Date = anniversary_date  
  FROM insurance_file WHERE insurance_file_cnt =@nOriginalInsuranceFileCnt 
  
  
UPDATE insurance_file 
SET CollectionFrequency_id =CASE WHEN @nCollectionFrequency=@nOld_CollectionFrequency THEN CollectionFrequency_id Else @nCollectionFrequency END,
DOPaymentTerms_id = CASE WHEN @nPaymentTerm=@nOld_PaymentTerm THEN DOPaymentTerms_id ELSE @nPaymentTerm END,  
business_type_id=@nBusinessType,
lead_agent_cnt=CASE WHEN @nLead_Agent_Cnt=@nOld_Lead_Agent_Cnt THEN lead_agent_cnt ELSE @nLead_Agent_Cnt END,
anniversary_date=CASE WHEN @dtAnniversary_Date>=expiry_date THEN @dtAnniversary_Date Else anniversary_date END  

WHERE insurance_file_cnt = @nNewInsuranceFileCnt  
IF @nBusinessType=1 BEGIN  
 DELETE from Tax_Calculation WHERE insurance_file_cnt=@nNewInsuranceFileCnt AND agent_commission_cnt IS NOT NULL  
 DELETE from Agent_Commission WHERE insurance_file_cnt=@nNewInsuranceFileCnt  
END 
END  
