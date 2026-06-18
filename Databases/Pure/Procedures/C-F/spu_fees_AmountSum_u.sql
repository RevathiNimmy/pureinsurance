EXECUTE DDLDropProcedure 'spu_fees_AmountSum_u'
GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

CREATE PROCEDURE spu_fees_AmountSum_u
(
	@transaction_type_id int,
	@product_group_id int
	
)

AS

DECLARE @FeeAmount	numeric(21,6)

 -- get the total amount of flat rate fee charges for given transaction and product group types
SELECT @FeeAmount = SUM(fee_amount)
FROM fee_amounts_u
WHERE (transaction_type_id = @transaction_type_id) AND (product_group_id = @product_group_id)  AND (is_taxable = 1)

RETURN @FeeAmount

GO

