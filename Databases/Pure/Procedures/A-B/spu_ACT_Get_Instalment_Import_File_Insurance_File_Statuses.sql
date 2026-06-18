SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_ACT_Get_Instalment_Import_File_Insurance_File_Statuses'
GO

CREATE PROCEDURE spu_ACT_Get_Instalment_Import_File_Insurance_File_Statuses

AS
	SELECT insurance_file_status_id, code, description 
	FROM insurance_File_status 
	WHERE code in ('CAN', 'LAP', 'REP')


GO
