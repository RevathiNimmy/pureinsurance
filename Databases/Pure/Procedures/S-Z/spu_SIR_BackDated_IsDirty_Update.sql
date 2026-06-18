SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_SIR_BackDated_IsDirty_Update'
GO

CREATE PROCEDURE spu_SIR_BackDated_IsDirty_Update  
    @baseinsurancefilecnt	int,  
    @Transaction_Type		VARCHAR(10) = NULL  
AS  
  
--*******************************************************************************************  
-- Version      Author  Date        Desc  
-- 1.00.0001    KKG     08/Dec/2010 To update isdirty flag for backjdated versions  
--  
--*******************************************************************************************  
-- Retrive  First Insurance File Cnt
BEGIN


DECLARE		@Insurance_File_Cnt	INT,
			@Policy_Version		INT

SELECT @Insurance_File_Cnt = @baseinsurancefilecnt

SELECT	@Policy_Version = policy_version
		FROM insurance_file 
WHERE	insurance_file_cnt = @baseinsurancefilecnt
	
IF @Transaction_Type = 'MTC' 
	SELECT  @Insurance_File_Cnt = Insurance_File_Cnt FROM insurance_file
		WHERE	base_insurance_file_cnt = @baseinsurancefilecnt
		AND		Policy_version = @Policy_Version
		

UPDATE mta_insurance_file_link 
	SET IsDirty = 0
WHERE insurance_file_cnt=@Insurance_File_Cnt   

END

GO
