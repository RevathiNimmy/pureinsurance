Execute DDLDropProcedure 'spu_SIR_Fee_amounts_UW_Update'
GO

CREATE PROCEDURE spu_SIR_Fee_amounts_UW_Update  
  
@fee_amount_id int,
@risk_type_group_id int,
@fee_percentage money,
@fee_amount money,
@transaction_type_id int,
@currency_id smallint,
@product_id int,
@peril_group_id int,
@transaction_sub_type tinyint,
@tax_group_id int,
@is_fee_applied_to_cr tinyint,
@effective_date datetime,
@include_fee_in_instalments tinyint,
@spread_fee_across_instalments tinyint ,
@nMakeLiveOptionsid INT,
@nDoPaymentTermsid INT,
@nCalculationBasis TINYINT,
@bIsProrated TINYINT,
@nOverrideRateAmount TINYINT,
@nUseWhenDeleted TINYINT,    
@nUserId INT,   
@nUniqueId VARCHAR(50),  
@nScreenHierarchy VARCHAR(500)
AS

BEGIN

 UPDATE fee_amounts

 SET
  risk_type_group_id = @risk_type_group_id,
  fee_percentage = @fee_percentage,
  fee_amount = @fee_amount,
  transaction_type_id = @transaction_type_id,
  currency_id = @currency_id,
  product_id = @product_id,
  peril_group_id  = @peril_group_id,
  transaction_sub_type = @transaction_sub_type,
  tax_group_id= @tax_group_id,
  is_fee_applied_to_cr = @is_fee_applied_to_cr,
  effective_date =@effective_date,
  include_fee_in_instalments=@include_fee_in_instalments ,
  spread_fee_across_instalments=@spread_fee_across_instalments,
  MakeLiveOptions_id =@nMakeLiveOptionsid ,
  DoPaymentTerms_id =@nDoPaymentTermsid,
  Calculation_Basis =@nCalculationBasis,
  Is_Prorated = @bIsProrated,
  Is_Override = @nOverrideRateAmount,
  Use_When_Deleted=@nUseWhenDeleted,
  User_Id=@nUserId,
  Date=convert(date,GETDATE()),
  Timestamp=convert(timestamp,GETDATE()),  
  UniqueId = @nUniqueId,  
  ScreenHierarchy = @nScreenHierarchy

 WHERE fee_amount_id = @fee_amount_id  
  
END  

GO
