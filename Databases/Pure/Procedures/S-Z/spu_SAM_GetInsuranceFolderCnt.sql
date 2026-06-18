 
SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_SAM_GetInsuranceFolderCnt'
GO

CREATE PROCEDURE spu_SAM_GetInsuranceFolderCnt  

    @nInsuranceFileCnt int  

AS  

 BEGIN  

  

 SELECT top 1 insurance_folder_cnt from Insurance_file

 WHERE insurance_file_cnt = @nInsuranceFileCnt  

  

 END 