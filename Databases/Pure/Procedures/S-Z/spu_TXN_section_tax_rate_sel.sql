SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS OFF 
GO
EXECUTE DDLDropProcedure 'spu_TXN_section_tax_rate_sel'
GO

CREATE PROCEDURE spu_TXN_section_tax_rate_sel
(
@TaxGroupId int = NULL,
@InsurerSection bit,
@RiskGroupId int,
@RiskCodeId int,
@COBRatingSectionId int,
@CountryId int,
@TransactionType int,
@EffectiveDate datetime,
@InsurerId int,
@rTaxGroupId int OUTPUT,
@rTaxGroupCode varchar(10) OUTPUT,
@rTaxRate float OUTPUT,
@rcalcBasis int OUTPUT,
@rTaxEditable tinyint OUTPUT
)

AS

DECLARE @DomiciledForTax tinyint

SELECT @DomiciledForTax=ISNULL(domiciled_for_tax,0) FROM party_insurer WHERE party_cnt=@InsurerId

SELECT @rTaxGroupId=0, @rTaxGroupCode='', @rTaxRate=0.0, @rcalcBasis=0, @rTaxEditable=0

--Temporary fix to exclude tax for premium sections for non-domiciled insurers
--This will be replaced by an additional filter on tax rates to specify if a given rate applies to the premium of a policy
IF @DomiciledForTax=0 AND @InsurerSection=0	
BEGIN
	RETURN
END

CREATE TABLE #TXN_SECTION_RATE_SEL
(
tax_group_id int NULL,
tax_group_code varchar(10) NULL,
rate float NULL,
NB tinyint NULL,
AMTA tinyint NULL,
RMTA tinyint NULL,
CANC tinyint NULL,
REN tinyint NULL,
TTAC tinyint NULL,
TTF tinyint NULL,
TTIC tinyint NULL,
TTE tinyint NULL,
risk_group_id int NULL,
risk_code_id int NULL,
COB_rating_section_id int NULL,
calc_basis int Null,
is_tax_amount_editable tinyint NULL
)

IF @TaxGroupId IS NULL
BEGIN
	IF EXISTS
	(
		SELECT NULL
		FROM
		Tax_Group TG
		INNER JOIN Tax_Group_Tax_Band TGTB ON TG.tax_group_id=TGTB.tax_group_id
		INNER JOIN Tax_Band TB ON TGTB.tax_band_id=TB.tax_band_id
		INNER JOIN Tax_Band_Rate TBR ON TB.tax_band_id=TBR.tax_band_id
		WHERE
		TG.is_deleted=0 AND
		TG.effective_date <= @EffectiveDate AND
		TB.is_deleted=0 AND
		TB.effective_date <= @EffectiveDate AND
		TBR.is_deleted=0 AND
		TBR.effective_date <= @EffectiveDate AND
		TBR.country_id = @CountryId
	)
	BEGIN
		INSERT INTO #TXN_SECTION_RATE_SEL
		SELECT	TG.tax_group_id,
		TG.code,
		TBR.rate,
		TBR.NB,
		TBR.AMTA,
		TBR.RMTA,
		TBR.CANC,
		TBR.REN,
		TBR.TTAC,
		TBR.TTF,
		TBR.TTIC,
		TBR.TTE,
		TBR.risk_group_id,
		TBR.risk_code_id,
		TBR.COB_rating_section_id,
		TBR.calc_basis,
		TG.is_tax_amount_editable
		FROM
		Tax_Group TG
		INNER JOIN Tax_Group_Tax_Band TGTB ON TG.tax_group_id=TGTB.tax_group_id
		INNER JOIN Tax_Band TB ON TGTB.tax_band_id=TB.tax_band_id
		INNER JOIN Tax_Band_Rate TBR ON TB.tax_band_id=TBR.tax_band_id
		WHERE
		TG.is_deleted=0 AND
		TG.effective_date <= @EffectiveDate AND
		TB.is_deleted=0 AND
		TB.effective_date <= @EffectiveDate AND
		TBR.is_deleted=0 AND
		TBR.effective_date <= @EffectiveDate AND
		TBR.country_id = @CountryId AND
		((@DomiciledForTax=1 AND ISNULL(TG.is_withholding_tax,0)=0) OR
		(@DomiciledForTax=0 AND ISNULL(TG.is_withholding_tax,0)=1))
	END
	ELSE
	BEGIN
		INSERT INTO #TXN_SECTION_RATE_SEL
		SELECT	TG.tax_group_id,
		TG.code,
		TBR.rate,
		TBR.NB,
		TBR.AMTA,
		TBR.RMTA,
		TBR.CANC,
		TBR.REN,
		TBR.TTAC,
		TBR.TTF,
		TBR.TTIC,
		TBR.TTE,
		TBR.risk_group_id,
		TBR.risk_code_id,
		TBR.COB_rating_section_id,
		TBR.calc_basis,
		TG.is_tax_amount_editable
		FROM
		Tax_Group TG
		INNER JOIN Tax_Group_Tax_Band TGTB ON TG.tax_group_id=TGTB.tax_group_id
		INNER JOIN Tax_Band TB ON TGTB.tax_band_id=TB.tax_band_id
		INNER JOIN Tax_Band_Rate TBR ON TB.tax_band_id=TBR.tax_band_id
		WHERE
		TG.is_deleted=0 AND
		TG.effective_date <= @EffectiveDate AND
		TB.is_deleted=0 AND
		TB.effective_date <= @EffectiveDate AND
		TBR.is_deleted=0 AND
		TBR.effective_date <= @EffectiveDate AND
		TBR.country_id IS NULL
	END
END
ELSE
BEGIN
	IF EXISTS
	( 	SELECT NULL FROM Tax_Group TG
		INNER JOIN Tax_Group_Tax_Band TGTB ON TG.tax_group_id=TGTB.tax_group_id
		INNER JOIN Tax_Band TB ON TGTB.tax_band_id=TB.tax_band_id
		INNER JOIN Tax_Band_Rate TBR ON TB.tax_band_id=TBR.tax_band_id
		WHERE
		TG.tax_group_id=@TaxGroupId AND
		TG.is_deleted=0 AND
		TG.effective_date <= @EffectiveDate AND
		TB.is_deleted=0 AND
		TB.effective_date <= @EffectiveDate AND
		TBR.is_deleted=0 AND
		TBR.effective_date <= @EffectiveDate AND
		TBR.country_id = @CountryId
	)
	BEGIN
		INSERT INTO #TXN_SECTION_RATE_SEL
		SELECT	TG.tax_group_id,
		TG.code,
		TBR.rate,
		TBR.NB,
		TBR.AMTA,
		TBR.RMTA,
		TBR.CANC,
		TBR.REN,
		TBR.TTAC,
		TBR.TTF,
		TBR.TTIC,
		TBR.TTE,
		TBR.risk_group_id,
		TBR.risk_code_id,
		TBR.COB_rating_section_id,
		TBR.calc_basis,
		TG.is_tax_amount_editable
		FROM
		Tax_Group TG
		INNER JOIN Tax_Group_Tax_Band TGTB ON TG.tax_group_id=TGTB.tax_group_id
		INNER JOIN Tax_Band TB ON TGTB.tax_band_id=TB.tax_band_id
		INNER JOIN Tax_Band_Rate TBR ON TB.tax_band_id=TBR.tax_band_id
		WHERE
		TG.tax_group_id=@TaxGroupId AND
		TG.is_deleted=0 AND
		TG.effective_date <= @EffectiveDate AND
		TB.is_deleted=0 AND
		TB.effective_date <= @EffectiveDate AND
		TBR.is_deleted=0 AND
		TBR.effective_date <= @EffectiveDate AND
		TBR.country_id = @CountryId
	END
	ELSE
	BEGIN
		INSERT INTO #TXN_SECTION_RATE_SEL
		SELECT	TG.tax_group_id,
		TG.code,
		TBR.rate,
		TBR.NB,
		TBR.AMTA,
		TBR.RMTA,
		TBR.CANC,
		TBR.REN,
		TBR.TTAC,
		TBR.TTF,
		TBR.TTIC,
		TBR.TTE,
		TBR.risk_group_id,
		TBR.risk_code_id,
		TBR.COB_rating_section_id,
		TBR.calc_basis,
		TG.is_tax_amount_editable
		FROM
		Tax_Group TG
		INNER JOIN Tax_Group_Tax_Band TGTB ON TG.tax_group_id=TGTB.tax_group_id
		INNER JOIN Tax_Band TB ON TGTB.tax_band_id=TB.tax_band_id
		INNER JOIN Tax_Band_Rate TBR ON TB.tax_band_id=TBR.tax_band_id
		WHERE
		TG.tax_group_id=@TaxGroupId AND
		TG.is_deleted=0 AND
		TG.effective_date <= @EffectiveDate AND
		TB.is_deleted=0 AND
		TB.effective_date <= @EffectiveDate AND
		TBR.is_deleted=0 AND
		TBR.effective_date <= @EffectiveDate AND
		TBR.country_id IS NULL
	END
END

IF @InsurerSection=1 DELETE FROM #TXN_SECTION_RATE_SEL WHERE TTIC <> 1
IF @TransactionType=0 DELETE FROM #TXN_SECTION_RATE_SEL WHERE NB <> 1
IF @TransactionType=3 DELETE FROM #TXN_SECTION_RATE_SEL WHERE AMTA <> 1
IF @TransactionType=3 DELETE FROM #TXN_SECTION_RATE_SEL WHERE RMTA <> 1
IF @TransactionType=4 DELETE FROM #TXN_SECTION_RATE_SEL WHERE CANC <> 1
IF @TransactionType=2 DELETE FROM #TXN_SECTION_RATE_SEL WHERE REN <> 1

IF EXISTS(SELECT NULL FROM #TXN_SECTION_RATE_SEL WHERE COB_rating_section_id=@COBRatingSectionId)
BEGIN
	SELECT TOP 1
	@rTaxGroupId=tax_group_id,
	@rTaxGroupCode=tax_group_code,
	@rTaxRate=rate,
	@rcalcBasis=calc_basis,
	@rTaxEditable=is_tax_amount_editable
	FROM
	#TXN_SECTION_RATE_SEL
	WHERE
	COB_rating_section_id=@COBRatingSectionId
END
ELSE
BEGIN

	IF EXISTS(SELECT NULL FROM #TXN_SECTION_RATE_SEL WHERE risk_code_id=@RiskCodeId)
	BEGIN

		SELECT TOP 1
		@rTaxGroupId=tax_group_id,
		@rTaxGroupCode=tax_group_code,
		@rTaxRate=rate,
		@rcalcBasis=calc_basis,
		@rTaxEditable=is_tax_amount_editable
		FROM
		#TXN_SECTION_RATE_SEL
		WHERE
		risk_code_id=@RiskCodeId

	END
	ELSE
	BEGIN

		IF EXISTS(SELECT NULL FROM #TXN_SECTION_RATE_SEL WHERE risk_group_id=@RiskGroupId)
		BEGIN

			SELECT TOP 1
			@rTaxGroupId=tax_group_id,
			@rTaxGroupCode=tax_group_code,
			@rTaxRate=rate,
			@rcalcBasis=calc_basis,
			@rTaxEditable=is_tax_amount_editable
			FROM
			#TXN_SECTION_RATE_SEL
			WHERE
			risk_group_id=@RiskGroupId

		END
		ELSE
		BEGIN

			SELECT TOP 1
			@rTaxGroupId=tax_group_id,
			@rTaxGroupCode=tax_group_code,
			@rTaxRate=rate,
			@rcalcBasis=calc_basis,
			@rTaxEditable=is_tax_amount_editable
			FROM
			#TXN_SECTION_RATE_SEL
			WHERE
			(risk_group_id IS NULL) AND
			(risk_code_id IS NULL) AND
			(COB_rating_section_id IS NULL)
			OR
			(risk_group_id = 0 AND
			risk_code_id = 0 AND
			COB_rating_section_id = 0)
		END
	END
END

DROP TABLE #TXN_SECTION_RATE_SEL

GO

SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO