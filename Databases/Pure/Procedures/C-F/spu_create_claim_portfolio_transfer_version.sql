SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_create_claim_portfolio_transfer_version'
GO


CREATE PROCEDURE spu_create_claim_portfolio_transfer_version  
 @nInsurance_file_cnt INT,  
 @nClaim_id INT,  
 @nPrePortfolioTransfer INT,  
 @nNew_claim_id INT OUT,  
 @nStatsFolderCnt INT OUTPUT  
AS  
  
  
DECLARE  
  @status INT,  
  @gis_policy_link_id INT,  
  @claim_peril_id INT,  
  @cobcode varchar(10),  
  @cob_id INT,  
  @ri_shortname varchar(20),  
  @ThisRevision MONEY,  
  @document_comment VARCHAR(60),  
  @data_model_code  varchar(30) ,  
  @base_claim_id INT  
        
  SELECT @nNew_claim_id=0, @status=0, @gis_policy_link_id=0  
  
  EXEC spu_CLM_Copy_Claim @Claim_id=@nClaim_id,@transaction_type_code='C_CR',@created_by_id=1,@copy_claim_id=@nNew_claim_id OUT,@status=@status OUT  
  SELECT @gis_policy_link_id= GIS_Policy_Link_id from GIS_Policy_Link WHERE claim_id=@nClaim_id  
  EXEC spu_gis_policy_link_claim_copy @gis_policy_link_id,@nClaim_id,@nNew_claim_id  
  SELECT @base_claim_id = base_claim_id from claim where Claim_id =@nClaim_id  
  
  IF @nPrePortfolioTransfer = 1  
    BEGIN  
     UPDATE reserve  
     SET  
     this_revision = paid_to_date - (initial_reserve + revised_reserve) ,  
     revised_reserve = revised_reserve + paid_to_date - (initial_reserve + revised_reserve) ,  
     revision_count = revision_count + 1  
     WHERE claim_peril_id in (  
     SELECT claim_Peril_id from Claim_Peril where Claim_id=@nNew_claim_id  
     )  
  
    END  
  ELSE  
    BEGIN  
  
     UPDATE R2 SET  
        this_revision = -1* R1.this_revision,
        revised_reserve = R2.revised_reserve + (-1* R1.this_revision),
        revision_count = R1.revision_count + 1  
		FROM (SELECT claim_id,base_reserve_id,initial_reserve,paid_to_date,revised_reserve,this_revision,revision_count from Reserve r join claim_peril cp on r.claim_peril_id=cp.claim_peril_id
		WHERE cp.claim_id=@nClaim_id) R1 JOIN 
		(SELECT c.claim_id,base_reserve_id,initial_reserve,paid_to_date,revised_reserve,this_revision,revision_count from Reserve r join claim_peril cp on r.claim_peril_id=cp.claim_peril_id
		JOIN claim c ON c.claim_id=cp.claim_id
		WHERE c.claim_id=@nNew_claim_id and base_claim_id=@base_claim_id) R2 
		ON R1.base_reserve_id=R2.base_reserve_id
		
    END  
		  	
   IF @nPrePortfolioTransfer = 1  
    EXEC spu_Copy_Reinsurance_Details_To_Claim_RI2007 @Claim_id =@nNew_claim_id, @is_created = 2  
   ELSE  
    EXEC spu_Copy_Reinsurance_Details_To_Claim_RI2007 @Claim_id =@nNew_claim_id, @is_created = 1  
	 
   DECLARE c_perils CURSOR FAST_FORWARD FOR  
   SELECT claim_peril_id, RTRIM(cob.code) ,cob.class_of_business_id  
   FROM claim_peril cp WITH(NOLOCK)  
   LEFT JOIN Peril_Type pt ON pt.peril_type_id = cp.Peril_type_id  
   LEFT JOIN Class_Of_Business cob ON cob.class_of_business_id = pt.class_of_business_id  
   WHERE cp.claim_id = @nNew_claim_id  
  
   DECLARE @is_folder_created INT  
   SELECT @is_folder_created=0,@nStatsFolderCnt=0  
   OPEN c_perils  
   FETCH NEXT FROM c_perils INTO @claim_peril_id , @cobcode ,@cob_id  
   WHILE @@FETCH_STATUS = 0  
   BEGIN  
       SET @ThisRevision=0  
  
       SELECT @ThisRevision = sum(isnull(this_revision,0))  FROM reserve  WHERE claim_Peril_id=@claim_peril_id  
  
       IF @ThisRevision<>0  
       BEGIN  
     IF @is_folder_created=0 BEGIN  
      SET @document_comment='Reserve for claim number ' + CAST(@nNew_claim_id as varchar(20))  
      EXEC spu_add_stats_folder_claims @stats_folder_cnt=@nStatsFolderCnt OUT,@Insurance_file_cnt=@nInsurance_file_cnt,@debit_credit='D',@document_comment=@document_comment,@transaction_type_id=1,@transaction_type_code='C_CR',@user_id=1,  
      @USER_NAME='sirius',@Claim_id=@nNew_claim_id,@documenttype_id=41  
      SET @is_folder_created=1  
     END  

     SELECT @ri_shortname='CLMRES' + @cobcode  
     EXEC spu_add_stats_details_claims  
     @stats_folder_cnt =@nStatsFolderCnt,  
     @Claim_id =@nNew_claim_id,  
     @peril_id =@claim_peril_id,  
     @stats_detail_type ='GRS',  
     @class_of_business_id =@cob_id,  
     @class_of_business_code =@cobcode,  
     @ri_party_cnt =0,  
     @ri_shortname =@ri_shortname,  
     @ri_party_type =0,  
     @ri_share_percent =0,  
     @transaction_amount =@ThisRevision,  
     @documenttype_id =41  
    END  
    FETCH NEXT FROM c_perils INTO @claim_peril_id , @cobcode ,@cob_id  
   END  
   CLOSE c_perils  
   DEALLOCATE c_perils  
  
   IF @nStatsFolderCnt>0  
   BEGIN  
  
   EXEC spu_add_claims_stats_details_coins @nNew_claim_id, @nStatsFolderCnt  
  
   EXEC spu_add_claims_stats_details_reins @nNew_claim_id, @nStatsFolderCnt, 41  
  
   EXEC spu_CLM_Finalise_stats @nNew_claim_id, 41,'C_CR', @nStatsFolderCnt, 0,0,0,1  

   END  
  
   IF @nPrePortfolioTransfer <> 1  
    Insert into claim_pt_log(base_claim_id, claim_id, insurance_file_cnt, status_id,Effective_Date) values (@base_claim_id, @nNew_claim_id, @nInsurance_file_cnt, 2,getdate())  
  
GO
