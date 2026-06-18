SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_clm_claim_link_add'
GO

CREATE PROCEDURE spu_clm_claim_link_add  
  
@claim_id int,  
@link_type_id int,  
@link_id int  
  
AS  
  
BEGIN  
  
 INSERT INTO claim_link (  
  claim_id,  
  link_type_id,  
  link_id,  
  processed)  
 VALUES (  
  @claim_id,  
  @link_type_id,  
  @link_id,  
  0)  
  
END  



GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
