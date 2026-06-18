EXECUTE DDLDropProcedure 'spu_clm_get_transdetails_RI'
GO

CREATE PROCEDURE spu_clm_get_transdetails_RI    
 @nStatsFolderCnt int =0  ,
 @nClaimId int = 0 , 
 @nInsuranceFileCnt int = 0,
 @nDocumentId INT=0,
 @sTransactionType VARCHAR(30)=NULL
AS    
BEGIN  
 DECLARE @nTemp_ClaimId INT
 DECLARE @nTemp_InsuranceFileCnt INT
 DECLARE @nTemp_StatsFolderCnt INT
 SET @nTemp_ClaimId = @nClaimId
 SET @nTemp_InsuranceFileCnt = @nInsuranceFileCnt 
 SET @nTemp_StatsFolderCnt = @nStatsFolderCnt 

 IF @nDocumentId <>0 
 BEGIN

  Select t.Account_id,transdetail_id , OutStanding_Amount ,transdetail_type_id 
  From TransDetail t join Account a on t.account_id=a.account_id left join party_insurer p on a.account_key=p.party_cnt 
  Where Document_id= @nDocumentId  AND reinsurance_type in (1,10,11)

 END
 ELSE
 BEGIN
 If @nTemp_ClaimId > 0
 BEGIN
 Select t.Account_id,
		transdetail_id , 
		OutStanding_Amount ,
		transdetail_type_id 
 From TransDetail t JOIN Account a on t.account_id=a.account_id LEFT JOIN party_insurer p on a.account_key=p.party_cnt
 Where Document_id=(
				Select  TOP 1 d.document_id  from Stats_Folder Join Document D On d.document_ref =stats_folder.document_ref 
                 Where stats_folder.stats_folder_cnt < @nStatsFolderCnt AND  stats_folder.transaction_type_code =@sTransactionType  And stats_folder.loss_id =@nClaimId )

END
ELSE
IF @nTemp_InsuranceFileCnt > 0
BEGIN
 Select t.Account_id,
		transdetail_id , 
		OutStanding_Amount ,
		transdetail_type_id 
 From TransDetail t JOIN Account a on t.account_id=a.account_id LEFT JOIN party_insurer p on a.account_key=p.party_cnt
 Where Document_id=( 
               Select  TOP 1 d.document_id  from Stats_Folder Join Document D On d.document_ref =stats_folder.document_ref 
               Where stats_folder.stats_folder_cnt <  @nTemp_StatsFolderCnt And stats_folder.insurance_file_cnt = @nInsuranceFileCnt )

 END
END    
END
