SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_GetInsuranceFolderCnt'
GO


CREATE PROCEDURE spu_GetInsuranceFolderCnt
    @InsuranceFileCnt integer
AS


BEGIN
	SELECT insurance_folder_cnt FROM Insurance_File WHERE insurance_file_cnt = @InsuranceFileCnt
END
GO


