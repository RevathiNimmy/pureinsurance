SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_SIR_Get_Ren_PolVersion'
GO

CREATE PROCEDURE spu_SIR_Get_Ren_PolVersion
    @Insurance_File_Cnt Int 
AS
BEGIN
    
    SELECT    MAX(Policy_Version) + 1 
    FROM      Insurance_File 
    WHERE     insurance_folder_cnt = 
		  (
              SELECT   insurance_folder_cnt 
              FROM     Insurance_file 
              WHERE    insurance_file_cnt = @Insurance_File_Cnt
              )
		
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

