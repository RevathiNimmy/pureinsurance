SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_get_Insurance_Folder'
GO

CREATE PROCEDURE spu_get_Insurance_Folder
@insurance_file_cnt int

AS
BEGIN
	SELECT Top 1 insurance_Folder_cnt
	From insurance_file 
	Where insurance_file_cnt = @insurance_file_cnt
END

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

