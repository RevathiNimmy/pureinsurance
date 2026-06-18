SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_SAM_update_Sub_Branch_Id'
GO

CREATE PROCEDURE spu_SAM_update_Sub_Branch_Id 
@insurance_file_cnt as int, 
@branch_code varchar(10),  
@sub_branch_code varchar(10)  
  
AS  
 
UPDATE Insurance_File
SET Branch_Id =
 	(SELECT sub_branch.sub_branch_id  
 	 FROM source  
  	 JOIN sub_Branch ON source.source_id = sub_branch.source_id  
 	 WHERE source.code = @branch_code and sub_branch.code = @sub_branch_code)  
WHERE Insurance_File_Cnt=@insurance_file_cnt

GO
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO