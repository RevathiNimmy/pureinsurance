SET QUOTED_IDENTIFIER ON 
GO
SET ANSI_NULLS ON 
GO

EXECUTE DDLDropProcedure 'spu_TXN_party_tax_rate_sel'
GO

CREATE PROCEDURE spu_TXN_party_tax_rate_sel
(
	@TaxGroupId int = NULL,
	@PartyCnt int,
	@RiskGroupId int,
	@RiskCodeId int,
	@CountryId int,
	@TransactionType int,
	@EffectiveDate datetime
)
AS
DECLARE @PartyCode char(10)
DECLARE @DomiciledForTax tinyint

SELECT @DomiciledForTax=1

SELECT
@PartyCode=PT.code
FROM
party P
INNER JOIN party_type PT ON P.party_type_id=PT.party_type_id
WHERE P.party_cnt=@PartyCnt

IF @PartyCode='IN'
	SELECT @DomiciledForTax=domiciled_for_tax FROM party_insurer WHERE party_cnt=@PartyCnt

IF @PartyCode='AG'
	SELECT @DomiciledForTax=domiciled_for_tax FROM party_agent WHERE party_cnt=@PartyCnt

CREATE TABLE
#TXN_PARTY_RATE_SEL
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
	calc_basis int NULL
)

IF @TaxGroupId IS NULL
BEGIN
	IF EXISTS (
		SELECT NULL
		FROM
		Tax_Group TG
		INNER JOIN Tax_Group_Tax_Band TGTB ON TG.tax_group_id=TGTB.tax_group_id
		INNER JOIN Tax_Band TB ON TGTB.tax_band_id=TB.tax_band_id
		INNER JOIN Tax_Band_Rate TBR ON TB.tax_band_id=TBR.tax_band_id
		WHERE	TG.is_deleted=0 AND
		TG.effective_date <= @EffectiveDate AND
		TB.is_deleted=0 AND
		TB.effective_date <= @EffectiveDate AND
		TBR.is_deleted=0 AND
		TBR.effective_date <= @EffectiveDate AND
		TBR.country_id = @CountryId
	)
	BEGIN
		INSERT INTO #TXN_PARTY_RATE_SEL
		SELECT
		TG.tax_group_id,
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
		TBR.calc_basis
		FROM
		Tax_Group TG
		INNER JOIN Tax_Group_Tax_Band TGTB ON TG.tax_group_id=TGTB.tax_group_id
		INNER JOIN Tax_Band TB ON TGTB.tax_band_id=TB.tax_band_id
		INNER JOIN Tax_Band_Rate TBR ON TB.tax_band_id=TBR.tax_band_id
		WHERE	TG.is_deleted=0 AND
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
		INSERT INTO #TXN_PARTY_RATE_SEL
		SELECT
		TG.tax_group_id,
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
		TBR.calc_basis
		FROM
		Tax_Group TG
		INNER JOIN Tax_Group_Tax_Band TGTB ON TG.tax_group_id=TGTB.tax_group_id
		INNER JOIN Tax_Band TB ON TGTB.tax_band_id=TB.tax_band_id
		INNER JOIN Tax_Band_Rate TBR ON TB.tax_band_id=TBR.tax_band_id
		WHERE	TG.is_deleted=0 AND
		TG.effective_date <= @EffectiveDate AND
		TB.is_deleted=0 AND
		TB.effective_date <= @EffectiveDate AND
		TBR.is_deleted=0 AND
		TBR.effective_date <= @EffectiveDate AND
		TBR.country_id IS NULL AND
		((@DomiciledForTax=1 AND ISNULL(TG.is_withholding_tax,0)=0) OR
		(@DomiciledForTax=0 AND ISNULL(TG.is_withholding_tax,0)=1))
	END
END
ELSE
BEGIN
	IF EXISTS (
		SELECT NULL
		FROM
		Tax_Group TG
		INNER JOIN Tax_Group_Tax_Band TGTB ON TG.tax_group_id=TGTB.tax_group_id
		INNER JOIN Tax_Band TB ON TGTB.tax_band_id=TB.tax_band_id
		INNER JOIN Tax_Band_Rate TBR ON TB.tax_band_id=TBR.tax_band_id
		WHERE	TG.tax_group_id=@TaxGroupId AND
		TG.is_deleted=0 AND
		TG.effective_date <= @EffectiveDate AND
		TB.is_deleted=0 AND
		TB.effective_date <= @EffectiveDate AND
		TBR.is_deleted=0 AND
		TBR.effective_date <= @EffectiveDate AND
		TBR.country_id = @CountryId
	)
	BEGIN
		INSERT INTO #TXN_PARTY_RATE_SEL
		SELECT
		TG.tax_group_id,
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
		TBR.calc_basis
		FROM
		Tax_Group TG
		INNER JOIN Tax_Group_Tax_Band TGTB ON TG.tax_group_id=TGTB.tax_group_id
		INNER JOIN Tax_Band TB ON TGTB.tax_band_id=TB.tax_band_id
		INNER JOIN Tax_Band_Rate TBR ON TB.tax_band_id=TBR.tax_band_id
		WHERE	TG.tax_group_id=@TaxGroupId AND
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
		INSERT INTO #TXN_PARTY_RATE_SEL
		SELECT
		TG.tax_group_id,
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
		TBR.calc_basis
		FROM
		Tax_Group TG
		INNER JOIN Tax_Group_Tax_Band TGTB ON TG.tax_group_id=TGTB.tax_group_id
		INNER JOIN Tax_Band TB ON TGTB.tax_band_id=TB.tax_band_id
		INNER JOIN Tax_Band_Rate TBR ON TB.tax_band_id=TBR.tax_band_id
		WHERE	TG.tax_group_id=@TaxGroupId AND
		TG.is_deleted=0 AND
		TG.effective_date <= @EffectiveDate AND
		TB.is_deleted=0 AND
		TB.effective_date <= @EffectiveDate AND
		TBR.is_deleted=0 AND
		TBR.effective_date <= @EffectiveDate AND
		TBR.country_id IS NULL
	END
END

IF @PartyCode='AG'
BEGIN
	DELETE
	FROM
	#TXN_PARTY_RATE_SEL
	WHERE TTAC <> 1
END

IF @PartyCode='FE'
BEGIN
	DELETE
	FROM
	#TXN_PARTY_RATE_SEL
	WHERE TTF <> 1
END

IF @PartyCode='EX'
BEGIN
	DELETE
	FROM
	#TXN_PARTY_RATE_SEL WHERE TTE <> 1
END

IF @PartyCode='IN'
BEGIN
	DELETE
	FROM
	#TXN_PARTY_RATE_SEL WHERE TTF <> 1
END

IF @TransactionType=0
BEGIN
	DELETE
	FROM
	#TXN_PARTY_RATE_SEL WHERE NB <> 1
END

IF @TransactionType=1
BEGIN
	DELETE
	FROM
	#TXN_PARTY_RATE_SEL WHERE AMTA <> 1
END

IF @TransactionType=2
BEGIN
	DELETE
	FROM
	#TXN_PARTY_RATE_SEL WHERE RMTA <> 1
END

IF @TransactionType=3
BEGIN
	DELETE
	FROM
	#TXN_PARTY_RATE_SEL WHERE CANC <> 1
END

IF @TransactionType=4
BEGIN
	DELETE
	FROM
	#TXN_PARTY_RATE_SEL WHERE REN <> 1
END

IF EXISTS(SELECT NULL FROM #TXN_PARTY_RATE_SEL WHERE COB_rating_section_id IS NOT NULL)
BEGIN
	SELECT TOP 1 tax_group_id, tax_group_code, rate FROM #TXN_PARTY_RATE_SEL
	WHERE COB_rating_section_id IS NOT NULL
END
ELSE
BEGIN
	IF EXISTS(SELECT NULL FROM #TXN_PARTY_RATE_SEL WHERE risk_code_id=@RiskCodeId)
	BEGIN
		SELECT TOP 1 tax_group_id, tax_group_code, rate FROM #TXN_PARTY_RATE_SEL
		WHERE risk_code_id=@RiskCodeId
	END
	ELSE
	BEGIN
		IF EXISTS(SELECT NULL FROM #TXN_PARTY_RATE_SEL WHERE risk_group_id=@RiskGroupId)
		BEGIN
			SELECT TOP 1 tax_group_id, tax_group_code, rate FROM #TXN_PARTY_RATE_SEL
			WHERE risk_group_id=@RiskGroupId
		END
		ELSE
		BEGIN
			SELECT TOP 1 tax_group_id, tax_group_code, rate FROM #TXN_PARTY_RATE_SEL
			WHERE (risk_group_id IS NULL) AND
			(risk_code_id IS NULL) AND
			(COB_rating_section_id IS NULL)
		END
	END
END

DROP TABLE #TXN_PARTY_RATE_SEL

GO
SET QUOTED_IDENTIFIER OFF 
GO
SET ANSI_NULLS ON 
GO

