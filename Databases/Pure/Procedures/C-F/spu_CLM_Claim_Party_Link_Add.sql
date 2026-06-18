SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Claim_Party_Link_Add'
GO

CREATE PROCEDURE spu_CLM_Claim_Party_Link_Add  
 @party_cnt int,  
 @claim_id int  
AS  
BEGIN  

 DECLARE @version_id int 
 DECLARE @claim_party_link_id int 

 SELECT @version_id = version_id from claim where claim_id = @claim_id

 INSERT INTO claim_party_link (claim_id, party_cnt, version_id)  
 VALUES (@claim_id, @party_cnt, @version_id)  
 
 SELECT @claim_party_link_id = @@IDENTITY  
 
 UPDATE claim_party_link set base_claim_partY_link_id = @claim_party_link_id
 WHERE claim_party_link_id = @claim_party_link_id

END  


GO
