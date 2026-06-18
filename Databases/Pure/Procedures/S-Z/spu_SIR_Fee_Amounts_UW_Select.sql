
Execute DDLDropProcedure 'spu_SIR_Fee_Amounts_UW_Select'
GO

CREATE PROCEDURE spu_SIR_Fee_Amounts_UW_Select  
  
@fee_amount_id int

AS

BEGIN
 SELECT
  fee_amount_id,
  product_id,
  risk_type_group_id,
  peril_group_id,
  fee_percentage,
  fee_amount,
  currency_id,
  transaction_sub_type,
  effective_date,
  tax_group_id,
  is_fee_applied_to_cr,
  include_fee_in_instalments,
  spread_fee_across_instalments,
  MakeLiveOptions_id ,
  DoPaymentTerms_id ,
  Calculation_Basis ,
  Is_Prorated,
  transaction_type_id,
  Is_Override,
  Use_When_Deleted
 FROM fee_amounts
 WHERE fee_amount_id = @fee_amount_id  
  
END  

GO
