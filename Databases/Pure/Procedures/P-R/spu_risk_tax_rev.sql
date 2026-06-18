SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON
GO
Execute DDLDropProcedure 'spu_risk_tax_rev'
GO


CREATE PROCEDURE spu_risk_tax_rev
(
@nOldRiskCnt INT,
@nNewRiskCnt INT
)
AS
BEGIN  
if  @nOldRiskCnt<>@nNewRiskCnt
BEGIN
--TTR-Risk Tax,TTRITP-Tax on Treaty,TTRIFP- Tax on FAC,TTF- Tax on Fee,TTAC- Tax on agent commission  
DELETE FROM Tax_Calculation WHERE risk_cnt=@nNewRiskCnt AND transtype IN('TTR','TTIF','TTRITP','TTRIFP','TTF')  
  
DECLARE @nNewInsurance_File_Cnt INT  
  
SELECT @nNewInsurance_File_Cnt=Insurance_File_Cnt FROM insurance_file_risk_link WHERE risk_cnt=@nNewRiskCnt  
  
INSERT INTO tax_calculation (  
  risk_cnt,  
  tax_band_id,  
  premium,  
  percentage,  
  value,  
  is_value,  
  is_manually_changed,  
  Calc_Basis,  
  Basis_Value,  
  currency_id,  
  allow_tax_credit,  
  country_id,  
  state_id,  
  class_of_business_id,  
  tax_group_id,  
  sequence,  
  insurance_file_cnt,  
  transtype,  
  policy_fee_u_id,  
  is_not_applied_to_client,  
  include_tax_in_instalments,  
  spread_tax_across_instalments,  
  tax_band_rate_id,  
  is_suspended)  
    SELECT  
    @nNewRiskCnt,  
    tax_band_id,  
    premium * -1,  
  percentage,  
  value * -1,  
  is_value,  
  is_manually_changed,  
  Calc_Basis,  
  Basis_Value,  
  currency_id,  
  allow_tax_credit,  
  country_id,  
  state_id,  
  class_of_business_id,  
  tax_group_id,  
  sequence,  
  @nNewInsurance_File_Cnt,  
  transtype,  
  policy_fee_u_id,  
  is_not_applied_to_client,  
  include_tax_in_instalments,  
  spread_tax_across_instalments,  
  tax_band_rate_id,  
  is_suspended  
    FROM    tax_calculation WITH (NOLOCK)  
    WHERE   risk_cnt=@nOldRiskCnt AND transtype IN ('TTR','TTIF','TTRITP','TTRIFP','TTF')  
  
END  
END

GO
