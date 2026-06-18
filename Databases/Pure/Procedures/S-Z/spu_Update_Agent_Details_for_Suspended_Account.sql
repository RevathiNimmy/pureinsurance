SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure spu_Update_Agent_Details_for_Suspended_Account
GO

CREATE PROCEDURE spu_Update_Agent_Details_for_Suspended_Account  
@Agent_Code VARCHAR(20),  
@Insurance_ref VARCHAR(30)  
AS  
  
DECLARE @PreviousAgentCode AS VARCHAR(30),  
@InsuranceFileCnt AS INT  
if @Insurance_ref is not null and @Agent_Code is not null  
BEGIN  
  
 SELECT   @PreviousAgentCode=D.release_account_code,  
  @InsuranceFileCnt=F.insurance_file_cnt  
         FROM    Transaction_Export_Folder F  
         JOIN    Transaction_Export_Detail D ON D.transaction_export_folder_cnt = F.transaction_export_folder_cnt  
  WHERE  insurance_ref=@Insurance_ref AND suspended=1  
  
 if @PreviousAgentCode<>NULL  
 BEGIN  
  if @Agent_Code <> @PreviousAgentCode  
  BEGIN  
  DECLARE @party_agent_type_id AS INT
  SELECT party_agent_type_id FROM party_agent WHERE Party_cnt= (SELECT party_cnt FROM party WHERE shortname=@Agent_Code)
  if @party_agent_type_id<>2
  BEGIN
   DECLARE @Agent_ID AS INT  
   SELECT @Agent_ID=account_id FROM account WHERE short_code=@Agent_Code  
   UPDATE suspended_accounts_transactions SET destination_account_id=@Agent_ID WHERE insurance_file_cnt=@InsuranceFileCnt  
  END  
  END
 END  
END  
GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO
