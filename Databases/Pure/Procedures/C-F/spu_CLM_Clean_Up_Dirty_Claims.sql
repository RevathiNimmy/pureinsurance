SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_CLM_Clean_Up_Dirty_Claims'
GO


CREATE PROCEDURE spu_CLM_Clean_Up_Dirty_Claims  

@claim_id int
  
AS  
  
BEGIN  
  
DECLARE @dirty_claim_id int  
DECLARE @status int  
  
DECLARE dirty_claims CURSOR FOR  
 SELECT claim_id  
 FROM claim WITH (NOLOCK) 
 WHERE base_claim_id NOT IN (  
   SELECT lock_value  
   FROM pmlock WITH (NOLOCK) 
   WHERE lock_name = 'claim_id')  
 AND is_dirty = 1  
 AND base_claim_id IN ( 
 	SELECT base_claim_id 
	FROM claim WITH (NOLOCK)
	WHERE claim_id = @claim_id)
  
OPEN dirty_claims  
  
FETCH NEXT FROM dirty_claims  
INTO @dirty_claim_id  
  
WHILE @@FETCH_STATUS = 0  
BEGIN  
  
 EXEC spu_delete_claim  
  @claim_id = @dirty_claim_id,  
  @status = @status OUTPUT  
  
   -- Get the next author.  
   FETCH NEXT FROM dirty_claims  
   INTO @dirty_claim_id  
END  
  
CLOSE dirty_claims  
DEALLOCATE dirty_claims  
  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
