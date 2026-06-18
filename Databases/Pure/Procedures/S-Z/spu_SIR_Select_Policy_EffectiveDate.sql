SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_SIR_Select_Policy_EffectiveDate'
GO

CREATE PROCEDURE spu_SIR_Select_Policy_EffectiveDate
	@insurance_file_cnt INT,
	@effective_date DATETIME OUTPUT
AS

DECLARE @cover_start_date DATETIME,
        @inception_date DATETIME,
        @insurance_file_type VARCHAR(10)

SELECT 	
    @cover_start_date = ifile.cover_start_date, 
    @inception_date = ifile.inception_date_tpi,
    @insurance_file_type = ift.code
FROM Insurance_File ifile
JOIN Insurance_File_Type ift 
ON ift.insurance_file_type_id = ifile.insurance_file_type_id
WHERE ifile.insurance_file_cnt = @insurance_File_Cnt

IF @insurance_file_type IN ('QUOTE','POLICY','RENEWAL')
    SELECT @effective_date = @cover_start_date
ELSE
    SELECT @effective_date = @inception_date

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO