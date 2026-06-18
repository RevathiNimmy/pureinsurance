Execute DDLDropProcedure 'spu_SIR_Fee_amounts_UW_Insert'
GO

CREATE PROCEDURE spu_SIR_Fee_amounts_UW_Insert  
@party_cnt int,  
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
@nCalculationBasis tinyint,
@bIsProrated tinyint,
@nOverrideRateAmount tinyint,
@nUseWhenDeleted tinyint,
@nUserId INT,    
@nUniqueId VARCHAR(50),    
@nScreenHierarchy VARCHAR(500) 
AS  
  
BEGIN  
  
 INSERT INTO fee_amounts  
 (  
  party_cnt,  
  risk_type_group_id,  
  fee_percentage,  
  fee_amount,  
  transaction_type_id,  
  currency_id,  
  product_id,  
  peril_group_id,  
  transaction_sub_type,  
  tax_group_id,  
  is_fee_applied_to_cr,  
  effective_date,
  include_fee_in_instalments,
  spread_fee_across_instalments,
  MakeLiveOptions_id ,  
DoPaymentTerms_id ,
Calculation_Basis ,
Is_Prorated,
Is_Override,
Use_When_Deleted,
User_Id,
Date,
Timestamp,    
UniqueId,    
ScreenHierarchy
 )  
 VALUES  
 (  
  @party_cnt,  
  @risk_type_group_id,  
  @fee_percentage,  
  @fee_amount,  
  @transaction_type_id,  
  @currency_id,  
  @product_id,  
  @peril_group_id,  
  @transaction_sub_type,  
  @tax_group_id,  
  @is_fee_applied_to_cr,  
  @effective_date,
  @include_fee_in_instalments,
@spread_fee_across_instalments,
@nMakeLiveOptionsid ,  
@nDoPaymentTermsid ,
@nCalculationBasis ,
@bIsProrated ,
@nOverrideRateAmount,
@nUseWhenDeleted,
@nUserId,
convert(date,GETDATE()),
convert(timestamp,GETDATE()),    
@nUniqueId,    
@nScreenHierarchy  
 )  
END  

GO
