SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_DT_GetDTLINK'
GO

CREATE PROCEDURE spu_SAM_DT_GetDTLINK  

@samstagingkey int,  
@keytype int,  
@siriuskey int OUTPUT
  
AS  
  
SELECT @siriuskey = siriuskey FROM DTLinks 
WHERE samstagingkey = @samstagingkey
AND keytype = @keytype



GO
