EXECUTE DDLDropProcedure 'spu_fees_Policy_u_delete'
GO


SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

CREATE PROCEDURE spu_fees_Policy_u_delete
(
	@insurance_file_cnt INT,
	@transaction_type_id INT,
	@product_id INT
)

AS 

DELETE policy_fee_u
WHERE insurance_file_cnt = @insurance_file_cnt
AND transaction_type_id = @transaction_type_id 
AND product_id = @product_id

GO

