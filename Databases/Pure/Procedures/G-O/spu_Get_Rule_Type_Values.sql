SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spu_Get_Rule_Type_Values'
GO

CREATE PROCEDURE spu_Get_Rule_Type_Values
    @tax_group_id int
AS
BEGIN

SELECT rule_type, advanced_tax_script
FROM tax_group
WHERE tax_group_id= @tax_group_id

END

GO
