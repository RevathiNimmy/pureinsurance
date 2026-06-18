SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
EXECUTE DDLDropProcedure 'spu_TXN_multi_tax_group_sel'
GO

CREATE PROCEDURE spu_TXN_multi_tax_group_sel
(
@effective_date datetime
)
AS

SELECT
tax_group_id,
code
FROM
tax_group
WHERE
is_coinsurer_multiple_tax_group=1 AND
is_deleted=0 AND
effective_date<=@effective_date

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO