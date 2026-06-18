SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Claim_Party_Link_Delete'
GO

CREATE PROCEDURE spu_CLM_Claim_Party_Link_Delete  
 @party_cnt int,  
 @claim_id int  
AS  
BEGIN  
 Delete from claim_party_link  
 where claim_id = @claim_id  
 and party_cnt = @party_cnt  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
