SET QUOTED_IDENTIFIER OFF 

SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SAM_SubAgent_Add'
GO

CREATE PROCEDURE spu_SAM_SubAgent_Add    
 @insurance_file_cnt int,    
 @agent_cnt int    
AS    
    
DECLARE @ssp_sub_agent int  
  
DECLARE @relation_cnt INT  
DECLARE @InsuranceFileCnt  int  
SELECT @ssp_sub_agent = ISNULL(party_cnt,0) FROM party_agent WHERE is_ssp_subagent=1  
  
IF ISNULL(@agent_cnt,0) > 0  
BEGIN  

DECLARE cur_relation_cnt CURSOR FAST_FORWARD FOR 

	SELECT  
	 PR.relation_cnt  
	 FROM  
	 party_relationship PR  
	 JOIN  
	 relationship_type RT ON RT.relationship_type_id=PR.relationship_type_id  
	 JOIN  
	 party_relationship_group PRG ON PRG.party_relationship_group_id=RT.party_relationship_group_id  
	 JOIN  
	 party_agent PA ON PA.party_cnt=PR.relation_cnt  
	 JOIN  
	 party_agent_type PAT ON PAT.party_agent_type_id=PA.party_agent_type_id  
	 WHERE  
	 PR.party_cnt=@agent_cnt  
	 AND  
	 PRG.code='002'  
	 AND  
	 PAT.code='Sub-Agent'  
	 AND  
	 PR.relation_cnt <> @ssp_sub_agent  
	 
OPEN cur_relation_cnt
    FETCH NEXT FROM cur_relation_cnt INTO @relation_cnt
        WHILE @@FETCH_STATUS = 0
        BEGIN
 
           IF  ISNULL(@relation_cnt,0)<>0  
			 BEGIN  
			 SELECT @InsuranceFileCnt=insurance_file_cnt FROM insurance_file_agent WHERE  insurance_file_cnt=@insurance_file_cnt AND party_cnt=@relation_cnt  
			  
			 IF ISNULL(@InsuranceFileCnt,0)=0  
			 BEGIN  
			 INSERT INTO insurance_file_agent  
			 (insurance_file_cnt, party_cnt, percentage, amount)  VALUES(@insurance_file_cnt,@relation_cnt,0,0)  
			 END  
		   END  	 
 
        FETCH NEXT FROM cur_relation_cnt INTO @relation_cnt
        END
    CLOSE cur_relation_cnt
DEALLOCATE cur_relation_cnt

END  
   