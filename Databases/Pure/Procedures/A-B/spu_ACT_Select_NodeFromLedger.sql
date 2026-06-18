SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_NodeFromLedger'
GO


CREATE PROCEDURE spu_ACT_Select_NodeFromLedger
    @ledger_name varchar(60),
    @sub_branch_id int
AS

DECLARE @Value VARCHAR(20)
SELECT
    @Value=Value
FROM
    Hidden_options
WHERE
    option_number=16

--If Null/0 then there is only one tree.
--Hardcoded for performance reasons

IF @Value IS NULL OR @Value=0
    SELECT @sub_branch_id=1

SELECT node_id
FROM   StructureTree s WITH(NOLOCK)
JOIN   ledger l WITH(NOLOCK) ON l.mapping_id = s.mapping_id
WHERE  l.ledger_name = @ledger_name
AND    l.sub_branch_id = @sub_branch_id

GO


