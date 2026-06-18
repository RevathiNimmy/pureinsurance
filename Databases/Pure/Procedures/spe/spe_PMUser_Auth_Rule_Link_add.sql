SET QUOTED_IDENTIFIER ON SET ANSI_NULLS ON
GO

EXECUTE DDLDropProcedure 'spe_PMUser_Auth_Rule_Link_add'
GO

CREATE PROCEDURE spe_PMUser_Auth_Rule_Link_add
    @product_id int,
    @authority_level_type_id int,
    @transaction_type_id int,
    @is_underwriter tinyint,
    @rule_set_id int
AS
BEGIN

INSERT INTO PMUser_Authority_Rule_Set_Link (
    product_id ,
    authority_level_type_id ,
    transaction_type_id ,
    is_underwriter ,
    rule_set_id )
VALUES (
    @product_id,
    @authority_level_type_id,
    @transaction_type_id,
    @is_underwriter,
    @rule_set_id)
END

GO

