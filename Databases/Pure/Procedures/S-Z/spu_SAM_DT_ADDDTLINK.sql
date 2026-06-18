SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_DT_ADDDTLINK'
GO


CREATE PROCEDURE spu_SAM_DT_ADDDTLINK  

@siriuskey int,   
@samstagingkey int,  
@keytype int  
  
AS  
  
BEGIN  
  
INSERT into DTLINKS (siriuskey, samstagingkey, keytype) 
 VALUES (@siriuskey, @samstagingkey, @keytype)

END  



GO
