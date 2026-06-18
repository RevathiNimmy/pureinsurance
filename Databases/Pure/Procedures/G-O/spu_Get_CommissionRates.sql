SET QUOTED_IDENTIFIER OFF SET ANSI_NULLS ON
GO


EXECUTE DDLDropProcedure 'spu_Get_CommissionRates'
GO

CREATE PROCEDURE spu_Get_CommissionRates
	@InsurerCnt int,
	@SchemeId int,
	@RiskGroupId int,
	@RiskCodeId int,
	@RiskSectionId int,
	@EffectiveDate datetime, 
	@Rate1 numeric(19,4) OUTPUT,
	@Value1 numeric (19,4) OUTPUT,
	@MinimumTotal1 numeric(19,4) OUTPUT,
	@Rate2 numeric(19,4) OUTPUT,
	@Value2 numeric (19,4) OUTPUT,
	@MinimumTotal2 numeric(19,4) OUTPUT,	
	@Rate3 numeric(19,4) OUTPUT,
	@Value3 numeric (19,4) OUTPUT,
	@MinimumTotal3 numeric(19,4) OUTPUT,
	@CommissionTaxGroupId varchar OUTPUT
AS
BEGIN
DECLARE @RateEffectiveDate as DateTime
SELECT 	@Rate1 = 0,@Value1=0,@MinimumTotal1=0,
       	@Rate2 = 0,@Value2=0,@MinimumTotal2=0,
	@Rate3= 0,@Value3=0,@MinimumTotal3=0,
	@CommissionTaxGroupId = NULL

IF @schemeId <> 0
BEGIN
	SELECT @RateEffectiveDate=max(effective_date) 
	FROM insurer_scheme_rate WHERE scheme = @SchemeId
			 AND effective_date <= @effectivedate
			 AND party_cnt = @InsurerCnt 
	SELECT @Rate1=Rate1,@Value1=Value1,@MinimumTotal1=Minimum_total1,
	@Rate2=Rate2,@Value2=Value2,@MinimumTotal2=Minimum_total2,
	@Rate3=Rate3,@Value3=Value3,@MinimumTotal3=Minimum_total3,
	@CommissionTaxGroupId=tax_group_id
	FROM Insurer_Scheme_Rate WHERE scheme = @SchemeId
			 AND effective_date = @RateEffectiveDate
			 AND party_cnt = @InsurerCnt
	 
END
ELSE
BEGIN
	IF exists(select NULL from insurer_section_rate where risk_code_id = @RiskCodeId 
					AND risk_code_COB_Rating_Section_Id = @RiskSectionId
					AND effective_date <= @effectivedate
					AND party_cnt = @InsurerCnt)
				AND @RiskSectionId <> 0
	BEGIN
	SELECT @RateEffectiveDate=max(effective_date) 
	FROM insurer_section_rate WHERE risk_code_id = @RiskCodeId
			AND risk_code_COB_Rating_Section_Id = @RiskSectionId
			 AND effective_date <= @effectivedate
			 AND party_cnt = @InsurerCnt 
	SELECT @Rate1=Rate1,@Value1=Value1,@MinimumTotal1=Minimum_total1,
	@Rate2=Rate2,@Value2=Value2,@MinimumTotal2=Minimum_total2,
	@Rate3=Rate3,@Value3=Value3,@MinimumTotal3=Minimum_total3,
	@CommissionTaxGroupId=tax_group_id
	FROM Insurer_Section_Rate WHERE risk_code_id = @RiskCodeId
			 AND risk_code_COB_Rating_Section_Id = @RiskSectionId
			 AND effective_date = @RateEffectivedate
			 AND party_cnt = @InsurerCnt
	END 
	ELSE
		BEGIN
		IF exists(select NULL from insurer_rate where risk_code_id = @RiskCodeId
				AND effective_date <= @effectivedate
				AND party_cnt = @InsurerCnt)
		BEGIN
		SELECT @RateEffectiveDate=max(effective_date) 
		FROM insurer_rate WHERE risk_code_id = @RiskCodeId
			 AND effective_date <= @effectiveDate
			 AND party_cnt = @InsurerCnt 
		SELECT @Rate1=Rate1,@Value1=Value1,@MinimumTotal1=Minimum_total1,
		@Rate2=Rate2,@Value2=Value2,@MinimumTotal2=Minimum_total2,
		@Rate3=Rate3,@Value3=Value3,@MinimumTotal3=Minimum_total3,
		@CommissionTaxGroupId=tax_group_id
		FROM Insurer_Rate WHERE risk_code_id = @RiskCodeId
			 AND effective_date = @RateEffectiveDate 
			 AND party_cnt = @InsurerCnt 
		END 
		ELSE 
			BEGIN
			If @RiskGroupId=0
 				SELECT @RiskGroupId=risk_group_id FROM risk_code WHERE risk_code_id = @RiskCodeId
			IF exists(SELECT NULL FROM insurer_group_rate WHERE risk_group_id = @RiskGroupId
						AND effective_date <= @effectivedate
						AND party_cnt = @InsurerCnt)
				BEGIN
				SELECT @RateEffectiveDate=max(effective_date) 
				FROM insurer_Group_rate WHERE risk_group_id = @RiskGroupId
			 	AND effective_date <= @effectiveDate
			 	AND party_cnt = @InsurerCnt 
				
				SELECT @Rate1=Rate1,@Value1=Value1,@MinimumTotal1=Minimum_total1,
				@Rate2=Rate2,@Value2=Value2,@MinimumTotal2=Minimum_total2,
				@Rate3=Rate3,@Value3=Value3,@MinimumTotal3=Minimum_total3,
				@CommissionTaxGroupId=tax_group_id
				FROM Insurer_Group_Rate WHERE risk_group_id = @RiskGroupId
				AND effective_date = @RateEffectiveDate
			 	AND party_cnt = @InsurerCnt
				END
			END
		END
END
END
GO