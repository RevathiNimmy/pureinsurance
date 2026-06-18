SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_InsFolderFromInsRef'
GO

CREATE PROCEDURE spu_Get_InsFolderFromInsRef
   @insurance_ref varchar(30)
AS
SELECT TOP 1 insurance_folder_cnt FROM insurance_file
WHERE insurance_ref=@insurance_ref

GO