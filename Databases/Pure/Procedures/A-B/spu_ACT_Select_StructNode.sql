SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_StructNode'
GO


CREATE PROCEDURE spu_ACT_Select_StructNode
    @node_id int
AS

/******22/11/2000 Added account ledger ID as return parameter *******/
SELECT StructureTree.node_id,
    StructureTree.element_id,
    RTRIM(Element.element_name),
    StructureTree.account_id,
    RTRIM(Account.account_name),
    Account.accounttype_id,
    Account.short_code,
    StructureTree.mapping_id,
    Mapping.description,
    Account.ledger_id
    FROM Mapping
         RIGHT JOIN (Account
         RIGHT JOIN (Element
         INNER JOIN StructureTree
             ON Element.element_id = StructureTree.element_id)
             ON Account.account_id = StructureTree.account_id)
             ON Mapping.mapping_id = StructureTree.mapping_id
                       WHERE StructureTree.node_id = @node_id
GO


