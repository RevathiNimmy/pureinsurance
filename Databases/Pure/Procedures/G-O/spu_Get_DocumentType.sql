SET QUOTED_IDENTIFIER OFF 
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_DocumentType'
GO

CREATE PROCEDURE spu_Get_DocumentType
@code varchar(25)
AS
SELECT documenttype_id FROM DocumentType
WHERE code=@code
GO
