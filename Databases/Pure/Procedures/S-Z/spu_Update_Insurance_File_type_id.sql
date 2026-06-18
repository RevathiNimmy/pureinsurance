SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_Update_Insurance_File_type_id'
GO

CREATE PROCEDURE spu_Update_Insurance_File_type_id
 @nInsurance_file_cnt INT,  
 @nInsurance_file_type_id INT

AS
UPDATE  Insurance_File    
SET   	insurance_file_type_id=@nInsurance_file_type_id   
WHERE 	insurance_file_cnt = @nInsurance_file_cnt

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
