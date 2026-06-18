SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_Get_Risk_Code_ID'
GO


CREATE PROCEDURE spu_Get_Risk_Code_ID
    @insurance_file_cnt int

AS
    SELECT    risk_code_id 
    FROM      Insurance_File
    WHERE     insurance_file_cnt = @insurance_file_cnt

GO

