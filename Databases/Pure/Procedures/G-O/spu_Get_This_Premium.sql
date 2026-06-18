SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_Get_This_Premium'
GO

CREATE PROCEDURE spu_Get_This_Premium  
    @insurance_file_cnt int  
AS  
  
SELECT  
    this_premium  

FROM Insurance_File  
WHERE  
    insurance_file_cnt = @insurance_file_cnt  

GO

