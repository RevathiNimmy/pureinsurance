SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS OFF
GO

EXECUTE DDLDropProcedure 'spu_get_prod_auto_num_ids'
GO

CREATE PROCEDURE spu_get_prod_auto_num_ids
    @product_id int
AS

SELECT  code,
	quote_auto_numbering_id,
	policy_auto_numbering_id,
	prov_claim_auto_numbering_id,
	full_claim_auto_numbering_id,
   	ISNULL(is_policy_number_at_quote,0) is_policy_number_at_quote,
	Cover_Note_Numbering_Id
FROM Product
WHERE
product_id = @product_id

GO
SET QUOTED_IDENTIFIER OFF
GO
SET ANSI_NULLS ON
GO
