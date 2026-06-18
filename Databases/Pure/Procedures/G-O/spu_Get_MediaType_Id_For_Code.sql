SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_MediaType_Id_For_Code'
GO

CREATE PROCEDURE spu_Get_MediaType_Id_For_Code
    @MediaType_Code varchar(10)
AS

SELECT mediatype_id FROM MediaType WHERE code = @MediaType_Code

GO
