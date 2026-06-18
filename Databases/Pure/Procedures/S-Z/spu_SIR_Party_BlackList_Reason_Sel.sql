SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SIR_Party_BlackList_Reason_Sel'
GO

CREATE PROCEDURE spu_SIR_Party_BlackList_Reason_Sel  
 @party_cnt int  
AS  
  
BEGIN  

 SELECT p.blacklist_reason_id, br.description  
 FROM party p  
  LEFT JOIN blacklist_reason br ON  
   br.blacklist_reason_id = p.blacklist_reason_id    
 WHERE party_cnt = @party_cnt  

END  


GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
