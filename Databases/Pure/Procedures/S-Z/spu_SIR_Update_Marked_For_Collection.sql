SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXEC DDLDropProcedure 'spu_SIR_Update_Marked_For_Collection'
GO

CREATE PROCEDURE spu_SIR_Update_Marked_For_Collection  
  
@insurance_file_cnt int,  
@marked_for_collection tinyint,
@marked_date datetime  
AS  
  
BEGIN  
  
 UPDATE insurance_file  
 SET marked_for_collection=@marked_for_collection,
     marked_date=@marked_date
 WHERE insurance_file_cnt =@insurance_file_cnt  
  
END  
GO

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

