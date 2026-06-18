SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_Insurance_File_Details_Select_By_Key'
GO

CREATE PROCEDURE spu_SAM_Insurance_File_Details_Select_By_Key

@insurance_file_cnt int

AS

    SELECT alternate_reference,
           insurance_folder_cnt,
           insurance_ref
FROM insurance_file 
WHERE insurance_File_cnt = @insurance_file_cnt


GO
