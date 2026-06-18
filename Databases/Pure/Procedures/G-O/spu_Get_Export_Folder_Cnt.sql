
EXECUTE DDLDropProcedure 'spu_Get_Export_Folder_Cnt'
GO


SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

CREATE PROCEDURE spu_Get_Export_Folder_Cnt
       @InsuranceFileCnt int,
       @TransactionExportFolderCnt int OUTPUT
      
AS

BEGIN

SELECT @TransactionExportFolderCnt = transaction_export_folder_cnt 
FROM   Transaction_Export_Folder
WHERE  insurance_file_cnt = @InsuranceFileCnt 

END 

GO