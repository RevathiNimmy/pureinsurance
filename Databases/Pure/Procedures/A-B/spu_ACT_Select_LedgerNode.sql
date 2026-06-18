SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_LedgerNode'
GO


CREATE PROCEDURE spu_ACT_Select_LedgerNode
    @ledger_name varchar(60),
    @company_id int
AS


/* DD 23/08/2002 */
/* Get the Product Option for multi-tree accounting */
DECLARE @Value VARCHAR(20)
SELECT
    @Value=Value
FROM
    Hidden_options
WHERE
    option_number=16

/*
    If Null/0 then there is only one tree.
    Hardcoded for performance reasons
*/
IF @Value IS NULL OR @Value=0
    SELECT @company_id=1

SELECT
        node_id
FROM
        StructureTree s
JOIN
        Mapping m ON m.mapping_id = s.mapping_id
WHERE
        m.description = @ledger_name
AND     m.company_id = @company_id

GO


