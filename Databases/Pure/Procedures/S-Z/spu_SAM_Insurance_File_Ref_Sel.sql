SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Insurance_File_Ref_Sel'
GO

CREATE PROCEDURE spu_SAM_Insurance_File_Ref_Sel
    @insurance_file_ref varchar(30),  
    @source_id	int
AS  
  
 SELECT  insurance_ref 
 FROM  	Insurance_File  
 WHERE 	insurance_Ref = @insurance_file_ref 
	AND 
	source_id=@source_id

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO