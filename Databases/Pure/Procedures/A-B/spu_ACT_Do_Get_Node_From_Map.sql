SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Do_Get_Node_From_Map'
GO


CREATE PROCEDURE spu_ACT_Do_Get_Node_From_Map
    @mapping_description varchar(60),
    @company_id int
AS

/*************************************************************************/
/* ERWIN generated: Select Node From Mapping Text (Description)          */
/*************************************************************************/
/*************************************************************************/
/* 1.0 20/02/98 JY Original Import                                       */
/*************************************************************************/

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

SELECT node_id
FROM   StructureTree s
JOIN   Mapping m ON s.mapping_id = m.mapping_id
WHERE  m.description = @mapping_description
AND    m.company_id = @company_id

GO


