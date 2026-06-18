
EXECUTE DDLDropProcedure 'spu_Product_Sel'
GO

CREATE PROCEDURE spu_Product_Sel
	@Product_id INT
AS
	SELECT product_id,
		   Code,
		   DESCRIPTION,
		   is_midnight_renewal,
		   is_auto_renewable,
		   ISNULL(is_true_monthly_policy,0) is_true_monthly_policy, 
		   ISNULL(unified_renewal_day,0) unified_renewal_day,
		   ISNULL(nb_prorata,0) nb_prorata,
		   ISNULL(grace_period,0) grace_period
	FROM   Product
	WHERE  product_id = @Product_id 
