EXECUTE DDLDropProcedure 'spu_fee_amounts_u_sel_specific'
GO

SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO


CREATE PROCEDURE spu_fee_amounts_u_sel_specific

    @fee_amount_id INT,
    @party_cnt INT

AS

SELECT
	fa.fee_amount_id,
	p.description AS ProdDesc,
	tt.description AS TransDesc, 
	fa.fee_percentage, 
	fa.fee_amount,
	fa.effective_date,
	c.description
FROM Transaction_Type tt
INNER JOIN fee_amounts_u fa
	ON fa.transaction_type_id = tt.transaction_type_id 
INNER JOIN Product p
	ON p.product_id = fa.product_group_id
INNER JOIN Currency c
	ON c.currency_id = fa.currency_id
WHERE party_cnt = @party_cnt 
AND fee_amount_id = @fee_amount_id

GO

