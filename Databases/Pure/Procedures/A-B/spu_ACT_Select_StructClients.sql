SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_ACT_Select_StructClients'
GO


CREATE PROCEDURE spu_ACT_Select_StructClients
    @node_id int,
    @code varchar(20), 
    @company_id int = null
AS

IF ISNULL(@company_id, 0) = 0
    SELECT  StructureTree.node_id,
            StructureTree.element_id,
            RTRIM(Element.element_name),	-- RAG 2001-01-14: Trim the code
            StructureTree.account_id,
            Account.account_name,
            Account.accounttype_id,
            RTRIM(Account.short_code),		-- RAG 2001-01-14: Trim the code
            StructureTree.mapping_id,
            Mapping.description,
            Company.company_id,
            Company.description,
            Sub_Branch.sub_branch_id,
            Sub_Branch.description
    FROM    StructureTree 
    LEFT JOIN
            Mapping ON Mapping.mapping_id = StructureTree.mapping_id
    LEFT JOIN 
            Account ON Account.account_id = StructureTree.account_id
    INNER JOIN
            Element ON Element.element_id = StructureTree.element_id
    INNER JOIN
            Company ON Company.company_id = StructureTree.company_id
    LEFT JOIN
            Sub_Branch ON Sub_Branch.sub_branch_id = account.sub_branch_id
    WHERE   StructureTree.parent_node_id = @node_id
    AND     Account.short_code LIKE @code
ELSE
    SELECT  StructureTree.node_id,
            StructureTree.element_id,
            RTRIM(Element.element_name),	-- RAG 2001-01-14: Trim the code
            StructureTree.account_id,
            Account.account_name,
            Account.accounttype_id,
            RTRIM(Account.short_code),		-- RAG 2001-01-14: Trim the code
            StructureTree.mapping_id,
            Mapping.description,
            Company.company_id,
            Company.description,
            Sub_Branch.sub_branch_id,
            Sub_Branch.description
    FROM    StructureTree 
    LEFT JOIN
            Mapping ON Mapping.mapping_id = StructureTree.mapping_id
    LEFT JOIN
            Account ON Account.account_id = StructureTree.account_id
    INNER JOIN
            Element ON Element.element_id = StructureTree.element_id
    INNER JOIN
            Company ON Company.company_id = StructureTree.company_id
    LEFT JOIN
            Sub_Branch ON Sub_Branch.sub_branch_id = account.sub_branch_id
    WHERE   StructureTree.parent_node_id = @node_id
    AND     StructureTree.company_id = @company_id
    AND     Account.short_code LIKE @code

GO


