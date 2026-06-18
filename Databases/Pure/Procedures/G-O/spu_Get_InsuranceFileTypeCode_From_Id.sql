SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_InsuranceFileTypeCode_From_Id'
GO

CREATE PROCEDURE spu_Get_InsuranceFileTypeCode_From_Id
    @InsuranceFileTypeId int
AS
BEGIN

 SELECT code FROM Insurance_File_Type IFT WHERE insurance_file_type_id = @InsuranceFileTypeId

END

SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
