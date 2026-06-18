SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_Get_BG_Status'
GO

CREATE PROCEDURE spu_Get_BG_Status 
        @BG_id      INT,  
 @Insurance_File_Cnt  INT = NULL,  
 @Master_BG_Status  INT OUTPUT  
  
AS  
  
SELECT  @Master_BG_Status = BG_Status_Id  
FROM  Bank_Guarantee  
Where  BG_Id = @BG_Id  

if (@Insurance_File_Cnt IS NOT NULL) 
SELECT  ISNULL(BG_Status_Id,1)  
FROM  Insurance_File_BG_Link IFBL  
Where  
	IFBL.Insurance_File_Cnt = @Insurance_File_Cnt
   
ELSE

SELECT 1

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
