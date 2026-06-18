SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_SAM_CLM_Tax_Calculation_sel'
GO

CREATE proc spu_SAM_CLM_Tax_Calculation_sel
@claim_peril_id INT,
@claim_payment_id INT,
@claim_payment_item_id int,
@version_id int,
@reserve_id int=0
as
if @reserve_id<>0 

begin
declare @resrvetypecode varchar(30)
  select @resrvetypecode= rt.description from Reserve_type rt join Reserve r on r.Reserve_type_id =rt.Reserve_type_id 
  where r.Reserve_id =@reserve_id
  select TG.code AS 'tax_group_code',TB.code AS 'band_code',@resrvetypecode as'reserve_type_code',TC.percentage AS 'PERCENTAGE',TC.value AS 'value'  from Tax_Calculation TC
  INNER JOIN Tax_Group TG ON TC.tax_group_id =TG.tax_group_id 
  INNER JOIN Tax_Band TB ON TB.tax_band_id =TC.tax_band_id 
  left join claim_peril on tc.claim_peril_id =claim_peril.Claim_Peril_id 
  left join Reserve on reserve.claim_Peril_id = claim_peril.Claim_Peril_id 
  INNER JOIN Claim_Payment_Item CP ON CP.claim_payment_item_id = TC.claim_payment_item_id
  where tc.claim_peril_id =@claim_peril_id and tc.version_id =@version_id  
  and CP.Reserve_id =@reserve_id

end
else
begin
  select TG.code AS 'tax_group_code',TB.code AS 'band_code',@resrvetypecode as'reserve_type_code',TC.percentage AS 'PERCENTAGE',TC.value AS 'value'  from Tax_Calculation TC
  INNER JOIN Tax_Group TG ON TC.tax_group_id =TG.tax_group_id 
  INNER JOIN Tax_Band TB ON TB.tax_band_id =TC.tax_band_id 
  where tc.claim_peril_id =@claim_peril_id and tc.version_id =@version_id 
end
GO









