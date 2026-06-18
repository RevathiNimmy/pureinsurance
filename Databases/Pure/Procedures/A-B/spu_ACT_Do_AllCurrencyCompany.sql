SET QUOTED_IDENTIFIER ON
GO
SET ANSI_NULLS ON
GO
EXECUTE DDLDropProcedure spu_ACT_Do_AllCurrencyCompany
GO

CREATE PROCEDURE spu_ACT_Do_AllCurrencyCompany
    @company_id smallint
AS
BEGIN

SELECT
    cur.currency_id,
    cur.iso_code,
    cur.description
    FROM Currency cur
        LEFT OUTER JOIN CompanyCurrency cc
            ON cur.currency_id = cc.currency_id
    AND cc.company_id = @company_id
    WHERE
         cur.is_deleted<>1
ORDER BY
    cur.iso_code
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

