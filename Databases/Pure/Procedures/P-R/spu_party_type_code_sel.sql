SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_party_type_code_sel'
GO

CREATE PROCEDURE spu_party_type_code_sel
    @party_type_id int
AS
SELECT Code
FROM   Party_Type
WHERE  party_type_id = @party_type_id

SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
