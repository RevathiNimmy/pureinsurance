SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO

EXECUTE DDLDropProcedure 'spu_TXN_insurer_commission_rate_sel'
GO

CREATE PROCEDURE spu_TXN_insurer_commission_rate_sel(

@PartyCnt int,
@RiskGroupId int,
@RiskCodeId int,
@COBRatingSectionId int,
@TransactionType int,
@EffectiveDate datetime,
@rCommissionPercent numeric(7,4) OUTPUT,
@rCommissionAmount numeric(19,4) OUTPUT,
@rMinimumBrokerage numeric(19,4) OUTPUT,
@rTaxGroupId int OUTPUT,
@GisSchemeId int = 0
)
AS

DECLARE @bFound bit
DECLARE @Rate1 numeric(7,4), @Amount1 numeric(19,4), @Minimum1 numeric(19,4)
DECLARE @Rate2 numeric(7,4), @Amount2 numeric(19,4), @Minimum2 numeric(19,4)
DECLARE @Rate3 numeric(7,4), @Amount3 numeric(19,4), @Minimum3 numeric(19,4)
DECLARE @LevySection tinyint

SELECT @LevySection=ISNULL(crs.is_levy_section,0)
FROM cob_rating_section crs
WHERE
crs.cob_rating_section_id=@COBRatingSectionId

SELECT @bFound=0

if @GisSchemeId <> 0 AND EXISTS
(
SELECT NULL FROM insurer_scheme_rate WHERE Party_Cnt=@PartyCnt AND
scheme=@GisSchemeId AND 
Effective_Date <= @EffectiveDate 
)
Begin
	SET @bFound = 1
	SELECT TOP 1 
	@Rate1=Rate1,@Amount1=Value1,@Minimum1=Minimum_Total1,
	@Rate2=Rate2,@Amount2=Value2,@Minimum2=Minimum_Total2,
	@Rate3=Rate3,@Amount3=Value3,@Minimum3=Minimum_Total3,
	@rTaxGroupId=Tax_Group_id
	FROM 
	insurer_scheme_rate 
	WHERE 
	Party_Cnt=@PartyCnt AND
	scheme=@GisSchemeId AND 
	Effective_Date<=@EffectiveDate
	ORDER BY Effective_Date DESC
End

IF @bFound=0 and EXISTS
(
SELECT NULL FROM insurer_section_rate WHERE Party_Cnt=@PartyCnt AND
risk_code_id=@RiskCodeId AND 
Risk_code_COB_rating_section_id=@COBRatingSectionId AND 
Effective_Date <= @EffectiveDate 
)

BEGIN

	SET @bFound = 1
	SELECT TOP 1 
	@Rate1=Rate1,@Amount1=Value1,@Minimum1=Minimum_Total1,
	@Rate2=Rate2,@Amount2=Value2,@Minimum2=Minimum_Total2,
	@Rate3=Rate3,@Amount3=Value3,@Minimum3=Minimum_Total3,
	@rTaxGroupId=Tax_Group_id
	FROM 
	insurer_section_rate 
	WHERE 
	Party_Cnt=@PartyCnt AND
	risk_code_id=@RiskCodeId AND 
	Risk_code_COB_rating_section_id=@COBRatingSectionId AND
	Effective_Date<=@EffectiveDate
	ORDER BY Effective_Date DESC

END

IF @bFound=0 AND EXISTS
(
SELECT NULL FROM insurer_rate WHERE Party_Cnt=@PartyCnt AND
risk_code_id=@RiskCodeId AND 
Effective_Date <= @EffectiveDate 
)

BEGIN
	SET @bFound = 1
	SELECT TOP 1 
	@Rate1=Rate1,@Amount1=Value1,@Minimum1=Minimum_Total1,
	@Rate2=Rate2,@Amount2=Value2,@Minimum2=Minimum_Total2,
	@Rate3=Rate3,@Amount3=Value3,@Minimum3=Minimum_Total3,
	@rTaxGroupId=Tax_Group_id
	FROM 
	insurer_rate 
	WHERE 
	party_cnt=@PartyCnt AND 
	risk_code_id=@RiskCodeId AND 
	Effective_Date<=@EffectiveDate
	ORDER BY Effective_Date DESC
END

IF @bFound=0 AND EXISTS
(
SELECT NULL FROM insurer_group_rate WHERE Party_Cnt=@PartyCnt AND
risk_group_id=@RiskGroupId AND 
Effective_Date <= @EffectiveDate 
)

BEGIN
	SET @bFound = 1
	SELECT TOP 1 
	@Rate1=Rate1,@Amount1=Value1,@Minimum1=Minimum_Total1,
	@Rate2=Rate2,@Amount2=Value2,@Minimum2=Minimum_Total2,
	@Rate3=Rate3,@Amount3=Value3,@Minimum3=Minimum_Total3,
	@rTaxGroupId=Tax_Group_id
	FROM 
	insurer_group_rate 
	WHERE 
	party_cnt=@PartyCnt AND 
	risk_group_id=@RiskGroupId AND 	
	Effective_Date<=@EffectiveDate
	ORDER BY Effective_Date DESC
END

IF @LevySection=1
BEGIN
	SELECT @rCommissionPercent=0.0,@rCommissionAmount=0.0,@rMinimumBrokerage=0.0
END
ELSE
	BEGIN
	IF @TransactionType=0
	BEGIN
		SELECT @rCommissionPercent=@Rate1	
		SELECT @rCommissionAmount=@Amount1	
		SELECT @rMinimumBrokerage=@Minimum1
	END
	IF @TransactionType=4
	BEGIN
		SELECT @rCommissionPercent=@Rate2
		SELECT @rCommissionAmount=@Amount2
		SELECT @rMinimumBrokerage=@Minimum2
	END
	IF (@TransactionType=1 OR @TransactionType=2)
	BEGIN
		SELECT @rCommissionPercent=@Rate3
		SELECT @rCommissionAmount=@Amount3
		SELECT @rMinimumBrokerage=@Minimum3
	END
END

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO


