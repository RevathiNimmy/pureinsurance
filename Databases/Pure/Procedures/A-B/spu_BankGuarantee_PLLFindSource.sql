SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure 'spu_BankGuarantee_PLLFindSource'
GO

CREATE PROCEDURE spu_BankGuarantee_PLLFindSource
    @WhereClause VARCHAR(50)
AS
BEGIN

 	SELECT
	  S.Source_Id,
	  S.description,
	  0 AS Chosen
  	FROM SOURCE S

  	WHERE  is_deleted <> 1
  	AND 	S.Description LIKE @WhereClause

END
GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
