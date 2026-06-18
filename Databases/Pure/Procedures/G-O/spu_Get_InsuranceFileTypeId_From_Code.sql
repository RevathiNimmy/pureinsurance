SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_InsuranceFileTypeId_From_Code'
GO

CREATE PROCEDURE spu_Get_InsuranceFileTypeId_From_Code
    @InsuranceFileTypeCode varchar(250)
AS
BEGIN

 SELECT insurance_file_type_id FROM Insurance_File_Type IFT WHERE code = @InsuranceFileTypeCode

END

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
